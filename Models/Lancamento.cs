using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    public class Lancamento
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [Required(ErrorMessage = "Conta é obrigatória")]
        public int ContaId { get; set; }
        public Conta? Conta { get; set; }

        [Required(ErrorMessage = "Tipo de Movimento é obrigatório")]
        public int TipoMovimentoId { get; set; }
        public TipoMovimento? TipoMovimento { get; set; }

        [Required(ErrorMessage = "Tipo é obrigatório")]
        [MaxLength(10)]
        public string Tipo { get; set; } = ""; // "Entrada" | "Saída"

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, 9999999.99, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Data é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; } = DateTime.Today;

        [MaxLength(500)]
        public string? Descricao { get; set; }
    }
}
