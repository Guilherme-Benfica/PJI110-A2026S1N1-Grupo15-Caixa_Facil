using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    /// <summary>
    /// Lançamento que se repete automaticamente todo mês em um dia fixo.
    /// </summary>
    public class LancamentoRecorrente
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
        public string Tipo { get; set; } = "Saída"; // "Entrada" | "Saída"

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, 9999999.99, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Dia de vencimento é obrigatório")]
        [Range(1, 28, ErrorMessage = "Dia deve ser entre 1 e 28")]
        public int DiaVencimento { get; set; } = 1;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [MaxLength(500)]
        public string Descricao { get; set; } = "";

        public bool Ativo { get; set; } = true;

        // Controle de geração automática
        public DateTime? UltimaGeracao { get; set; }
    }
}
