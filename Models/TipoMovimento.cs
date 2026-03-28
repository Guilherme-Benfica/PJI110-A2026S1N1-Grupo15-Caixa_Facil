using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    public class TipoMovimento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(80)]
        public string Nome { get; set; } = "";

        // Navegação
        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }
}
