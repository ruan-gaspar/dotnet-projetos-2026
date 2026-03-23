using LivrariaApi.Data;
using LivrariaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LivrariaApi.Messaging;

namespace LivrariaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly AppDbContext _ctx;

    public LivrosController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var livros = await _ctx.Livros.ToListAsync();
        return Ok(livros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var livro = await _ctx.Livros.FindAsync(id);

        if (livro == null)
            return NotFound();

        return Ok(livro);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Livro livro)
    {
        try
        {
            _ctx.Livros.Add(livro);
            await _ctx.SaveChangesAsync();

            try
            {
                RabbitMqProducer.Publish(livro);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[RABBITMQ ERROR]");
                Console.WriteLine(ex.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = livro.Id }, livro);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[POST LIVRO ERROR]");
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Erro ao criar livro");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Livro livro)
    {
        if (id != livro.Id)
            return BadRequest();

        _ctx.Entry(livro).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();

        try
        {
            RabbitMqProducer.PublishUpdate(livro);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[RABBITMQ UPDATE ERROR]");
            Console.WriteLine(ex.Message);
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var livro = await _ctx.Livros.FindAsync(id);

        if (livro == null)
            return NotFound();

        _ctx.Livros.Remove(livro);
        await _ctx.SaveChangesAsync();

        return NoContent();
    }
}
