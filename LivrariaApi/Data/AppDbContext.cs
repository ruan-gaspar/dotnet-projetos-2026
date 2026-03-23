using LivrariaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LivrariaApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Livro> Livros { get; set; }
    public ICollection<Emprestimo>? Emprestimos { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Livro>()
            .Property(l => l.Preco)
            .HasPrecision(10, 2);
        }
    }
