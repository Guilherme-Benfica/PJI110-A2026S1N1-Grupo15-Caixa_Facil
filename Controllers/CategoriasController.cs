using CaixaFacil.Data;
using CaixaFacil.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _db;
        public CategoriasController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.Categorias.OrderBy(c => c.Nome).ToListAsync());

        [HttpGet] public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Categorias.Add(model);
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Categoria criada!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Categorias.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["Sucesso"] = "Categoria atualizada!";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Categorias.FindAsync(id);
            if (c == null) return NotFound();
            try
            {
                _db.Categorias.Remove(c);
                await _db.SaveChangesAsync();
                TempData["Sucesso"] = "Categoria excluída!";
            }
            catch
            {
                TempData["Erro"] = "Não é possível excluir: categoria em uso.";
            }
            return RedirectToAction("Index");
        }
    }
}
