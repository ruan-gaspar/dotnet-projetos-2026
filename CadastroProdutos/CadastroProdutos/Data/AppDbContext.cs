
// Data/AppDbContext.csusing CadastroProdutos.Models;using Microsoft.EntityFrameworkCore;

using CadastroProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroProdutos.Data;
public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Produto> Produtos => Set<Produto>();
}

