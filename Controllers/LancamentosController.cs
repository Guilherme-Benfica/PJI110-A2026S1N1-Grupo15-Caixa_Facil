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
    public class LancamentosController : Controller
    {
        private readonly AppDbContext _db;
        public LancamentosController(AppDbContext db) => _db = db;

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ── Index ─────────────────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? tipo, int? categoriaId, string? mes)
        {
            DateTime inicio, fim;
            if (DateTime.TryParseExact(mes + "-01", "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var mesD))
            { inicio = mesD; fim = mesD.AddMonths(1); }
            else
            { var h = DateTime.Today; inicio = new DateTime(h.Year, h.Month, 1); fim = inicio.AddMonths(1); }

            var query = _db.Lancamentos
                .Include(l => l.Categoria)
                .Include(l => l.Conta)
                .Include(l => l.TipoMovimento)
                .Where(l => l.UsuarioId == UserId && l.Data >= inicio && l.Data < fim);

            if (!string.IsNullOrEmpty(tipo))       query = query.Where(l => l.Tipo == tipo);
            if (categoriaId.HasValue)              query = query.Where(l => l.CategoriaId == categoriaId);

            ViewBag.Categorias = new SelectList(await _db.Categorias.OrderBy(c => c.Nome).ToListAsync(), "Id", "Nome", categoriaId);
            ViewBag.FiltroTipo = tipo;
            ViewBag.FiltroMes  = inicio.ToString("yyyy-MM");

            return View(await query.OrderByDescending(l => l.Data).ToListAsync());
        }

        // ── Create ────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateSelectsAsync();
            return View(new Lancamento { Data = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lancamento model)
        {
            model.UsuarioId = UserId;
            ModelState.Remove("Usuario");
            ModelState.Remove("Categoria");
            ModelState.Remove("Conta");
            ModelState.Remove("TipoMovimento");

            if (!ModelState.IsValid) { await PopulateSelectsAsync(model); return View(model); }

            _db.Lancamentos.Add(model);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Lançamento registrado!";
            return RedirectToAction("Index");
        }

        // ── Edit ──────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var l = await _db.Lancamentos.FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == UserId);
            if (l == null) return NotFound();
            await PopulateSelectsAsync(l);
            return View(l);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lancamento model)
        {
            if (id != model.Id) return BadRequest();

            model.UsuarioId = UserId;
            ModelState.Remove("Usuario");
            ModelState.Remove("Categoria");
            ModelState.Remove("Conta");
            ModelState.Remove("TipoMovimento");

            if (!ModelState.IsValid) { await PopulateSelectsAsync(model); return View(model); }

            // Verifica ownership
            var exists = await _db.Lancamentos.AnyAsync(l => l.Id == id && l.UsuarioId == UserId);
            if (!exists) return Forbid();

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Lançamento atualizado!";
            return RedirectToAction("Index");
        }

        // ── Delete ────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var l = await _db.Lancamentos.FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == UserId);
            if (l == null) return NotFound();
            _db.Lancamentos.Remove(l);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Lançamento excluído!";
            return RedirectToAction("Index");
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private async Task PopulateSelectsAsync(Lancamento? model = null)
        {
            ViewBag.Categorias     = new SelectList(await _db.Categorias.OrderBy(c => c.Nome).ToListAsync(), "Id", "Nome", model?.CategoriaId);
            ViewBag.Contas         = new SelectList(await _db.Contas.OrderBy(c => c.Nome).ToListAsync(), "Id", "Nome", model?.ContaId);
            ViewBag.TiposMovimento = new SelectList(await _db.TiposMovimento.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", model?.TipoMovimentoId);
        }
    }
}
