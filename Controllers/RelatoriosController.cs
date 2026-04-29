using System.Security.Claims;
using CaixaFacil.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CaixaFacil.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _db;
        public RelatoriosController(AppDbContext db) => _db = db;

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ── Página de relatórios ──────────────────────────────────────────
        public IActionResult Index()
        {
            ViewBag.MesAtual = DateTime.Today.ToString("yyyy-MM");
            return View();
        }

        // ── Exportar PDF ─────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> ExportarPdf(string? mes)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            DateTime inicio, fim;
            if (DateTime.TryParseExact(mes + "-01", "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var mesD))
            { inicio = mesD; fim = mesD.AddMonths(1); }
            else
            { var h = DateTime.Today; inicio = new DateTime(h.Year, h.Month, 1); fim = inicio.AddMonths(1); }

            var userName = User.Identity?.Name ?? "Usuário";

            var lancamentos = await _db.Lancamentos
                .Include(l => l.Categoria)
                .Include(l => l.Conta)
                .Include(l => l.TipoMovimento)
                .Where(l => l.UsuarioId == UserId && l.Data >= inicio && l.Data < fim)
                .OrderByDescending(l => l.Data)
                .ToListAsync();

            var totalEntradas = lancamentos.Where(l => l.Tipo == "Entrada").Sum(l => l.Valor);
            var totalSaidas   = lancamentos.Where(l => l.Tipo == "Saída").Sum(l => l.Valor);
            var saldo         = totalEntradas - totalSaidas;
            var mesAno        = inicio.ToString("MMMM/yyyy");

            // ── Gera o PDF com QuestPDF ───────────────────────────────────
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // Cabeçalho
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("CaixaFácil").FontSize(20).Bold().FontColor("#198754");
                                c.Item().Text("Relatório Financeiro").FontSize(12).FontColor("#6c757d");
                            });
                            row.ConstantItem(150).AlignRight().Column(c =>
                            {
                                c.Item().Text(mesAno).FontSize(12).Bold();
                                c.Item().Text(userName).FontSize(9).FontColor("#6c757d");
                                c.Item().Text($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor("#adb5bd");
                            });
                        });
                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#198754");
                    });

                    // Conteúdo
                    page.Content().PaddingTop(16).Column(col =>
                    {
                        // Cards de resumo
                        col.Item().Row(row =>
                        {
                            // Entradas
                            row.RelativeItem().Border(1).BorderColor("#198754").Padding(10).Column(c =>
                            {
                                c.Item().Text("ENTRADAS").FontSize(9).Bold().FontColor("#198754");
                                c.Item().Text($"R$ {totalEntradas:N2}").FontSize(14).Bold().FontColor("#198754");
                            });
                            row.ConstantItem(10);
                            // Saídas
                            row.RelativeItem().Border(1).BorderColor("#dc3545").Padding(10).Column(c =>
                            {
                                c.Item().Text("SAÍDAS").FontSize(9).Bold().FontColor("#dc3545");
                                c.Item().Text($"R$ {totalSaidas:N2}").FontSize(14).Bold().FontColor("#dc3545");
                            });
                            row.ConstantItem(10);
                            // Saldo
                            var saldoColor = saldo >= 0 ? "#0d6efd" : "#ffc107";
                            row.RelativeItem().Border(1).BorderColor(saldoColor).Padding(10).Column(c =>
                            {
                                c.Item().Text("SALDO").FontSize(9).Bold().FontColor(saldoColor);
                                c.Item().Text($"R$ {saldo:N2}").FontSize(14).Bold().FontColor(saldoColor);
                            });
                        });

                        col.Item().PaddingTop(20);

                        // Tabela de lançamentos
                        col.Item().Text("Lançamentos do período").FontSize(12).Bold();
                        col.Item().PaddingTop(6);

                        if (!lancamentos.Any())
                        {
                            col.Item().Text("Nenhum lançamento encontrado neste período.")
                                .FontColor("#6c757d").Italic();
                        }
                        else
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(70);  // Data
                                    columns.ConstantColumn(60);  // Tipo
                                    columns.RelativeColumn(2);   // Categoria
                                    columns.RelativeColumn(2);   // Conta
                                    columns.RelativeColumn(3);   // Descrição
                                    columns.ConstantColumn(80);  // Valor
                                });

                                // Header
                                static IContainer HeaderCell(IContainer c) =>
                                    c.Background("#198754").Padding(5);

                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderCell).Text("Data").FontColor("#ffffff").Bold().FontSize(9);
                                    header.Cell().Element(HeaderCell).Text("Tipo").FontColor("#ffffff").Bold().FontSize(9);
                                    header.Cell().Element(HeaderCell).Text("Categoria").FontColor("#ffffff").Bold().FontSize(9);
                                    header.Cell().Element(HeaderCell).Text("Conta").FontColor("#ffffff").Bold().FontSize(9);
                                    header.Cell().Element(HeaderCell).Text("Descrição").FontColor("#ffffff").Bold().FontSize(9);
                                    header.Cell().Element(HeaderCell).AlignRight().Text("Valor (R$)").FontColor("#ffffff").Bold().FontSize(9);
                                });

                                // Linhas
                                var i = 0;
                                foreach (var l in lancamentos)
                                {
                                    var bg = i % 2 == 0 ? "#ffffff" : "#f8f9fa";
                                    i++;

                                    static IContainer DataCell(IContainer c, string bg) =>
                                        c.Background(bg).BorderBottom(1).BorderColor("#dee2e6").Padding(5);

                                    table.Cell().Element(c => DataCell(c, bg)).Text(l.Data.ToString("dd/MM/yyyy")).FontSize(9);
                                    table.Cell().Element(c => DataCell(c, bg)).Text(l.Tipo).FontColor(l.Tipo == "Entrada" ? "#198754" : "#dc3545").Bold().FontSize(9);
                                    table.Cell().Element(c => DataCell(c, bg)).Text(l.Categoria?.Nome ?? "—").FontSize(9);
                                    table.Cell().Element(c => DataCell(c, bg)).Text(l.Conta?.Nome ?? "—").FontSize(9);
                                    table.Cell().Element(c => DataCell(c, bg)).Text(l.Descricao ?? "—").FontSize(9);
                                    table.Cell().Element(c => DataCell(c, bg)).AlignRight()
                                        .Text($"{l.Valor:N2}").FontColor(l.Tipo == "Entrada" ? "#198754" : "#dc3545").Bold().FontSize(9);
                                }
                            });
                        }
                    });

                    // Rodapé
                    page.Footer().AlignCenter()
                        .Text(x =>
                        {
                            x.Span("CaixaFácil — Página ").FontSize(8).FontColor("#adb5bd");
                            x.CurrentPageNumber().FontSize(8).FontColor("#adb5bd");
                            x.Span(" de ").FontSize(8).FontColor("#adb5bd");
                            x.TotalPages().FontSize(8).FontColor("#adb5bd");
                        });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            var fileName = $"CaixaFacil_{inicio:yyyy-MM}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
