using System.Security.Claims;
using CaixaFacil.Data;
using CaixaFacil.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var hoje   = DateTime.Today;
            var inicio = new DateTime(hoje.Year, hoje.Month, 1);
            var fim    = inicio.AddMonths(1);

            var lancamentos = await _db.Lancamentos
                .Include(l => l.Categoria)
                .Include(l => l.Conta)
                .Include(l => l.TipoMovimento)
                .Where(l => l.UsuarioId == userId && l.Data >= inicio && l.Data < fim)
                .OrderByDescending(l => l.Data)
                .ToListAsync();

            var entradas = lancamentos.Where(l => l.Tipo == "Entrada").Sum(l => l.Valor);
            var saidas   = lancamentos.Where(l => l.Tipo == "Saída").Sum(l => l.Valor);

            // Dados para gráfico dos últimos 7 dias
            var labels       = new List<string>();
            var grafEntradas = new List<decimal>();
            var grafSaidas   = new List<decimal>();

            for (int i = 6; i >= 0; i--)
            {
                var dia  = hoje.AddDays(-i);
                var diaL = lancamentos.Where(l => l.Data.Date == dia.Date).ToList();
                labels.Add(dia.ToString("dd/MM"));
                grafEntradas.Add(diaL.Where(l => l.Tipo == "Entrada").Sum(l => l.Valor));
                grafSaidas.Add(diaL.Where(l => l.Tipo == "Saída").Sum(l => l.Valor));
            }

            var vm = new DashboardViewModel
            {
                TotalEntradas      = entradas,
                TotalSaidas        = saidas,
                UltimosLancamentos = lancamentos.Take(10).ToList(),
                MesAno             = hoje.ToString("MMMM/yyyy"),
                GraficoLabels      = labels,
                GraficoEntradas    = grafEntradas,
                GraficoSaidas      = grafSaidas
            };

            return View(vm);
        }
    }
}
