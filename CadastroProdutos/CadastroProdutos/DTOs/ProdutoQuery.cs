// DTOs/ProdutoQuery.cs
namespace CadastroProdutos.DTOs;
 
public class ProdutoQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "Id";
    public string? SortDir { get; set; } = "asc";
    public string? Categoria { get; set; }
    public decimal? PrecoMin { get; set; }
    public decimal? PrecoMax { get; set; }
}