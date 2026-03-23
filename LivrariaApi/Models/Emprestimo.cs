using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrariaApi.Models;

[Table("Emprestimos")]
public class Emprestimo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int LivroId { get; set; }

    [ForeignKey("LivroId")]
    public Livro? Livro { get; set; }

    [Required]
    [StringLength(100)]
    public string NomeLeitor { get; set; } = string.Empty;

    [Required]
    public DateTime DataEmprestimo { get; set; }

    [Required]
    public DateTime DataDevolucaoPrevista { get; set; }
}
