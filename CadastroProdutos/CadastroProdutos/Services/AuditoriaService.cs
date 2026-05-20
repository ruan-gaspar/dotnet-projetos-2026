// Services/AuditoriaService.cs

using CadastroProdutos.Models;
using MongoDB.Driver;

namespace CadastroProdutos.Services;
public interface IAuditoriaService{
    Task RegistrarAsync(int produtoId, string acao, string usuario, string? detalhes = null);
    Task<IEnumerable<AuditoriaProduto>> ListarAsync();
}
public class AuditoriaService : IAuditoriaService{
    private readonly IMongoCollection<AuditoriaProduto> _col;

    public AuditoriaService(IConfiguration cfg)
    {
        var client = new MongoClient(cfg["MongoDb:ConnectionString"]);
        var db = client.GetDatabase(cfg["MongoDb:Database"]);
        _col = db.GetCollection<AuditoriaProduto>(cfg["MongoDb:AuditCollection"]);
    }

    public Task RegistrarAsync(int produtoId, string acao, string usuario, string? detalhes = null)
        => _col.InsertOneAsync(new AuditoriaProduto        {
            ProdutoId = produtoId,
            Acao = acao,
            Usuario = usuario,
            Detalhes = detalhes        });

    public async Task<IEnumerable<AuditoriaProduto>> ListarAsync()
        => await _col.Find(_ => true)
            .SortByDescending(a => a.DataHora)
            .Limit(100)
            .ToListAsync();
}