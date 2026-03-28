using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; } = "";

        [MaxLength(250)]
        public string? Descricao { get; set; }

        // Navegação
        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }
}
