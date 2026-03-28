using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(120)]
        public string Nome { get; set; } = "";

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [MaxLength(200)]
        public string Email { get; set; } = "";

        [MaxLength(255)]
        public string SenhaHash { get; set; } = "";

        [MaxLength(20)]
        public string Tema { get; set; } = "system";

        [MaxLength(500)]
        public string? FotoPerfilUrl { get; set; }

        // Navegação
        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }
}
