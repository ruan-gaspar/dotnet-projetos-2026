// Models/AuditoriaProduto.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
 
namespace CadastroProdutos.Models;
 
public class AuditoriaProduto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
 
    public int ProdutoId { get; set; }
    public string Acao { get; set; } = string.Empty;      // CRIADO | ATUALIZADO | REMOVIDO
    public string Usuario { get; set; } = string.Empty;
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public string? Detalhes { get; set; }
}