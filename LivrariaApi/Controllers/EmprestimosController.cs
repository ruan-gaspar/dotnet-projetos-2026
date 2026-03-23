using LivrariaApi.Data;
using LivrariaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivrariaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmprestimosController : ControllerBase
{
    private readonly AppDbContext _ctx;

    public EmprestimosController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var emprestimos = await _ctx.Emprestimos!
            .Include(e => e.Livro)
            .ToListAsync();

        return Ok(emprestimos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var emprestimo = await _ctx.Emprestimos!
            .Include(e => e.Livro)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (emprestimo == null)
            return NotFound();

        return Ok(emprestimo);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Emprestimo emprestimo)
    {
        try
        {
            var livro = await _ctx.Livros.FindAsync(emprestimo.LivroId);

            if (livro == null)
                return BadRequest("Livro não encontrado.");

            _ctx.Emprestimos!.Add(emprestimo);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[POST EMPRESTIMO] Erro: {ex.Message}");
            return StatusCode(500, "Erro ao criar empréstimo.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Emprestimo emprestimo)
    {
        if (id != emprestimo.Id)
            return BadRequest();

        var existente = await _ctx.Emprestimos!.FindAsync(id);

        if (existente == null)
            return NotFound();

        try
        {
            existente.NomeLeitor = emprestimo.NomeLeitor;
            existente.DataEmprestimo = emprestimo.DataEmprestimo;
            existente.DataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista;
            existente.LivroId = emprestimo.LivroId;

            await _ctx.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PUT EMPRESTIMO] Erro: {ex.Message}");
            return StatusCode(500, "Erro ao atualizar empréstimo.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var emprestimo = await _ctx.Emprestimos!.FindAsync(id);

        if (emprestimo == null)
            return NotFound();

        _ctx.Emprestimos.Remove(emprestimo);
        await _ctx.SaveChangesAsync();

        return NoContent();
    }
}
