using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrariaApi.Models;

[Table("Livros")]
public class Livro
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    public decimal Preco { get; set; }

    [Required]
    public int AnoPublicacao { get; set; }
}
