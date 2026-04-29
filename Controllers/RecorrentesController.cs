using System.Security.Claims;
using CaixaFacil.Data;
using CaixaFacil.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Controllers
{
    [Authorize]
    public class RecorrentesController : Controller
    {
        private readonly AppDbContext _db;
        public RecorrentesController(AppDbContext db) => _db = db;

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ── Index ─────────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var lista = await _db.LancamentosRecorrentes
                .Include(r => r.Categoria)
                .Include(r => r.Conta)
                .Include(r => r.TipoMovimento)
                .Where(r => r.UsuarioId == UserId)
                .OrderBy(r => r.DiaVencimento)
                .ToListAsync();

            return View(lista);
        }

        // ── Create ────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateSelectsAsync();
            return View(new LancamentoRecorrente());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LancamentoRecorrente model)
        {
            model.UsuarioId = UserId;
            ModelState.Remove("Usuario");
            ModelState.Remove("Categoria");
            ModelState.Remove("Conta");
            ModelState.Remove("TipoMovimento");

            if (!ModelState.IsValid) { await PopulateSelectsAsync(model); return View(model); }

            _db.LancamentosRecorrentes.Add(model);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Lançamento recorrente criado!";
            return RedirectToAction("Index");
        }

        // ── Edit ──────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _db.LancamentosRecorrentes
                .FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == UserId);
            if (r == null) return NotFound();
            await PopulateSelectsAsync(r);
            return View(r);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LancamentoRecorrente model)
        {
            if (id != model.Id) return BadRequest();
            model.UsuarioId = UserId;
            ModelState.Remove("Usuario");
            ModelState.Remove("Categoria");
            ModelState.Remove("Conta");
            ModelState.Remove("TipoMovimento");

            if (!ModelState.IsValid) { await PopulateSelectsAsync(model); return View(model); }

            var exists = await _db.LancamentosRecorrentes
                .AnyAsync(r => r.Id == id && r.UsuarioId == UserId);
            if (!exists) return Forbid();

            _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Lançamento recorrente atualizado!";
            return RedirectToAction("Index");
        }

        // ── Delete ────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.LancamentosRecorrentes
                .FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == UserId);
            if (r == null) return NotFound();
            _db.LancamentosRecorrentes.Remove(r);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Lançamento recorrente excluído!";
            return RedirectToAction("Index");
        }

        // ── Gerar lançamentos do mês atual ────────────────────────────────
        /// <summary>
        /// Verifica os recorrentes ativos e gera os lançamentos do mês corrente
        /// se ainda não foram gerados.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GerarMesAtual()
        {
            var hoje   = DateTime.Today;
            var inicio = new DateTime(hoje.Year, hoje.Month, 1);

            var recorrentes = await _db.LancamentosRecorrentes
                .Where(r => r.UsuarioId == UserId && r.Ativo)
                .ToListAsync();

            int gerados = 0;

            foreach (var r in recorrentes)
            {
                // Verifica se já foi gerado este mês
                var jaExiste = await _db.Lancamentos.AnyAsync(l =>
                    l.UsuarioId  == UserId &&
                    l.CategoriaId == r.CategoriaId &&
                    l.ContaId    == r.ContaId &&
                    l.Valor      == r.Valor &&
                    l.Descricao  == r.Descricao &&
                    l.Data.Year  == hoje.Year &&
                    l.Data.Month == hoje.Month);

                if (!jaExiste)
                {
                    // Garante que o dia existe no mês (ex: dia 31 em fevereiro → último dia)
                    var diasNoMes = DateTime.DaysInMonth(hoje.Year, hoje.Month);
                    var dia       = Math.Min(r.DiaVencimento, diasNoMes);

                    _db.Lancamentos.Add(new Lancamento
                    {
                        UsuarioId       = UserId,
                        CategoriaId     = r.CategoriaId,
                        ContaId         = r.ContaId,
                        TipoMovimentoId = r.TipoMovimentoId,
                        Tipo            = r.Tipo,
                        Valor           = r.Valor,
                        Data            = new DateTime(hoje.Year, hoje.Month, dia),
                        Descricao       = r.Descricao + " (recorrente)"
                    });

                    r.UltimaGeracao = hoje;
                    gerados++;
                }
            }

            await _db.SaveChangesAsync();

            TempData["Sucesso"] = gerados > 0
                ? $"{gerados} lançamento(s) recorrente(s) gerado(s) para {hoje:MMMM/yyyy}!"
                : "Todos os lançamentos recorrentes já foram gerados este mês.";

            return RedirectToAction("Index");
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private async Task PopulateSelectsAsync(LancamentoRecorrente? model = null)
        {
            ViewBag.Categorias     = new SelectList(await _db.Categorias.OrderBy(c => c.Nome).ToListAsync(), "Id", "Nome", model?.CategoriaId);
            ViewBag.Contas         = new SelectList(await _db.Contas.OrderBy(c => c.Nome).ToListAsync(), "Id", "Nome", model?.ContaId);
            ViewBag.TiposMovimento = new SelectList(await _db.TiposMovimento.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", model?.TipoMovimentoId);
        }
    }
}
