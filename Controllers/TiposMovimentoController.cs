using CaixaFacil.Data;
using CaixaFacil.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Controllers
{
    [Authorize]
    public class TiposMovimentoController : Controller
    {
        private readonly AppDbContext _db;
        public TiposMovimentoController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.TiposMovimento.OrderBy(t => t.Nome).ToListAsync());

        [HttpGet] public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoMovimento model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.TiposMovimento.Add(model);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Tipo de movimento criado!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var t = await _db.TiposMovimento.FindAsync(id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoMovimento model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Tipo de movimento atualizado!";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.TiposMovimento.FindAsync(id);
            if (t == null) return NotFound();
            try
            {
                _db.TiposMovimento.Remove(t);
                await _db.SaveChangesAsync();
                TempData["Sucesso"] = "Tipo excluído!";
            }
            catch
            {
                TempData["Erro"] = "Não é possível excluir: tipo em uso.";
            }
            return RedirectToAction("Index");
        }
    }
}
