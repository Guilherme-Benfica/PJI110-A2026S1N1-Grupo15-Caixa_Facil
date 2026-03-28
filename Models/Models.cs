using System.ComponentModel.DataAnnotations;

namespace CaixaFacil.Models
{
    // ─── Usuário ────────────────────────────────────────────────────────────
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

    // ─── Categoria ──────────────────────────────────────────────────────────
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; } = "";

        [MaxLength(250)]
        public string? Descricao { get; set; }

        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }

    // ─── Conta / Caixa ──────────────────────────────────────────────────────
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

        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }

    // ─── Tipo de Movimento (ex: Venda, Aluguel, Insumo…) ───────────────────
    public class TipoMovimento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(80)]
        public string Nome { get; set; } = "";

        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
    }

    // ─── Lançamento ─────────────────────────────────────────────────────────
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

    // ─── ViewModels ─────────────────────────────────────────────────────────
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = "";
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(120)]
        public string Nome { get; set; } = "";

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = "";

        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; } = "";
    }

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

    public class DashboardViewModel
    {
        public decimal TotalEntradas { get; set; }
        public decimal TotalSaidas { get; set; }
        public decimal Saldo => TotalEntradas - TotalSaidas;
        public List<Lancamento> UltimosLancamentos { get; set; } = new();
        public string MesAno { get; set; } = "";
        // Dados para gráfico (preparado para Chart.js)
        public List<string> GraficoLabels { get; set; } = new();
        public List<decimal> GraficoEntradas { get; set; } = new();
        public List<decimal> GraficoSaidas { get; set; } = new();
    }
}
