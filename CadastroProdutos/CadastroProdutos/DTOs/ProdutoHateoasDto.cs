// DTOs/ProdutoHateoasDto.csusing CadastroProdutos.Models;

using CadastroProdutos.Models;

namespace CadastroProdutos.DTOs;
public class ProdutoHateoasDto{
    public Produto Produto { get; set; } = new();
    public List<LinkDto> Links { get; set; } = new();
}