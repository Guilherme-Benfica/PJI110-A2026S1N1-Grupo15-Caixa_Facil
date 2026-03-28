using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    public class Conta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; } = "";

        [MaxLength(100)]
        public string? Banco { get; set; }

        [MaxLength(50)]
        public string? Tipo { get; set; }

        // Navegação
        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }
}
