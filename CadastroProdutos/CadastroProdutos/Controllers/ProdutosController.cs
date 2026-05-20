// Controllers/ProdutosController.cs

using CadastroProdutos.Data;

using CadastroProdutos.DTOs;

using CadastroProdutos.Models;

using CadastroProdutos.Services;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
 
namespace CadastroProdutos.Controllers;
 
[Authorize]

[ApiController]

[Route("api/[controller]")]

public class ProdutosController : ControllerBase

{

    private List<LinkDto> GerarLinksProduto(int id)

    {

        var baseUrl = $"{Request.Scheme}://{Request.Host}/api/produtos";

        return new List<LinkDto>

    {

        new($"{baseUrl}/{id}", "self",   "GET"),

        new($"{baseUrl}/{id}", "update", "PUT"),

        new($"{baseUrl}/{id}", "delete", "DELETE")

    };

    }
 
    private readonly AppDbContext _db;

    private readonly IAuditoriaService _audit;
 
    public ProdutosController(AppDbContext db, IAuditoriaService audit)

    {

        _db = db;

        _audit = audit;

    }
 
    [HttpGet]

    [HttpGet]

    public async Task<IActionResult> Listar([FromQuery] ProdutoQuery query)

    {

        // Saneamento de entrada

        if (query.Page < 1) query.Page = 1;

        if (query.PageSize < 1 || query.PageSize > 100) query.PageSize = 10;
 
        var consulta = _db.Produtos.AsQueryable();
 
        // Filtros condicionais

        if (!string.IsNullOrWhiteSpace(query.Categoria))

            consulta = consulta.Where(p => p.Categoria == query.Categoria);
 
        if (query.PrecoMin.HasValue)

            consulta = consulta.Where(p => p.Preco >= query.PrecoMin.Value);
 
        if (query.PrecoMax.HasValue)

            consulta = consulta.Where(p => p.Preco <= query.PrecoMax.Value);
 
        // Ordenação via pattern matching

        consulta = (query.SortBy?.ToLower(), query.SortDir?.ToLower()) switch

        {

            ("nome", "desc") => consulta.OrderByDescending(p => p.Nome),

            ("nome", _) => consulta.OrderBy(p => p.Nome),

            ("preco", "desc") => consulta.OrderByDescending(p => p.Preco),

            ("preco", _) => consulta.OrderBy(p => p.Preco),

            (_, "desc") => consulta.OrderByDescending(p => p.Id),

            _ => consulta.OrderBy(p => p.Id)

        };
 
        // Materialização: aqui sim o SQL é executado

        var total = await consulta.CountAsync();

        var items = await consulta

            .Skip((query.Page - 1) * query.PageSize)

            .Take(query.PageSize)

            .ToListAsync();
 
 
        //HATEOAS

        var baseUrl = $"{Request.Scheme}://{Request.Host}/api/produtos";

        var totalPages = (int)Math.Ceiling((double)total / query.PageSize);
 
        var linksPaginacao = new List<LinkDto>

{

    new($"{baseUrl}?page={query.Page}&pageSize={query.PageSize}", "self", "GET")

};
 
        if (query.Page > 1)

            linksPaginacao.Add(new(

                $"{baseUrl}?page={query.Page - 1}&pageSize={query.PageSize}", "prev", "GET"));
 
        if (query.Page < totalPages)

            linksPaginacao.Add(new(

                $"{baseUrl}?page={query.Page + 1}&pageSize={query.PageSize}", "next", "GET"));
 
        return Ok(new

        {

            Items = items.Select(p => new ProdutoHateoasDto

            {

                Produto = p,

                Links = GerarLinksProduto(p.Id)

            }),

            query.Page,

            query.PageSize,

            TotalItems = total,

            TotalPages = totalPages,

            Links = linksPaginacao

        });
 
 
 
    }
 
    [HttpGet("{id:int}")]

    public async Task<IActionResult> ObterPorId(int id)

    {

        var produto = await _db.Produtos.FindAsync(id);

        if (produto is null) return NotFound();
 
        return Ok(new ProdutoHateoasDto

        {

            Produto = produto,

            Links = GerarLinksProduto(produto.Id)

        });

    }
 
    [HttpPost]

    public async Task<IActionResult> Criar(Produto produto)

    {

        _db.Produtos.Add(produto);

        await _db.SaveChangesAsync();
 
        await _audit.RegistrarAsync(

    produto.Id, "CRIADO",

    User.Identity?.Name ?? "anonimo",

    $"Nome={produto.Nome}, Preco={produto.Preco}");
 
        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);

    }
 
    [HttpPut("{id:int}")]

    public async Task<IActionResult> Atualizar(int id, Produto produto)

    {

        if (id != produto.Id) return BadRequest();

        _db.Entry(produto).State = EntityState.Modified;

        await _db.SaveChangesAsync();

        await _audit.RegistrarAsync(

    id, "ATUALIZADO",

    User.Identity?.Name ?? "anonimo");

        return NoContent();

    }
 
    [HttpDelete("{id:int}")]

    public async Task<IActionResult> cRemover(int id)

    {

        var produto = await _db.Produtos.FindAsync(id);

        if (produto is null) return NotFound();

        _db.Produtos.Remove(produto);

        await _db.SaveChangesAsync();

        await _audit.RegistrarAsync(

    id, "REMOVIDO",

    User.Identity?.Name ?? "anonimo");

        return NoContent();

    }

}
 