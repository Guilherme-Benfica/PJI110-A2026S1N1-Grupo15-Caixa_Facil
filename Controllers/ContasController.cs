using CaixaFacil.Data;
using CaixaFacil.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Controllers
{
    [Authorize]
    public class ContasController : Controller
    {
        private readonly AppDbContext _db;
        public ContasController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.Contas.OrderBy(c => c.Nome).ToListAsync());

        [HttpGet] public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Conta model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Contas.Add(model);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Conta criada!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Contas.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Conta model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Conta atualizada!";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Contas.FindAsync(id);
            if (c == null) return NotFound();
            try
            {
                _db.Contas.Remove(c);
                await _db.SaveChangesAsync();
                TempData["Sucesso"] = "Conta excluída!";
            }
            catch
            {
                TempData["Erro"] = "Não é possível excluir: conta em uso.";
            }
            return RedirectToAction("Index");
        }
    }
}
