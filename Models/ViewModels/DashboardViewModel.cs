namespace CaixaFacil.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalEntradas { get; set; }
        public decimal TotalSaidas { get; set; }
        public decimal Saldo => TotalEntradas - TotalSaidas;
        public List<Lancamento> UltimosLancamentos { get; set; } = new();
        public string MesAno { get; set; } = "";

        // Dados para gráfico (Chart.js)
        public List<string> GraficoLabels { get; set; } = new();
        public List<decimal> GraficoEntradas { get; set; } = new();
        public List<decimal> GraficoSaidas { get; set; } = new();
    }
}
