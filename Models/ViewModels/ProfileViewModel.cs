using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(120)]
        public string Nome { get; set; } = "";

        [MaxLength(20)]
        public string Tema { get; set; } = "system";

        [MaxLength(500)]
        public string? FotoPerfilUrl { get; set; }
    }
}
