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
}
