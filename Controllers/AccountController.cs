using System.Security.Claims;
using CaixaFacil.Data;
using CaixaFacil.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) => _db = db;

        // ── Login ────────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(vm.Senha, user.SenhaHash))
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(vm);
            }

            await SignInAsync(user);
            return LocalRedirect(returnUrl ?? "/");
        }

        // ── Cadastro ─────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _db.Usuarios.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(vm);
            }

            var user = new Usuario
            {
                Nome      = vm.Nome,
                Email     = vm.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(vm.Senha),
                Tema      = "system"
            };

            _db.Usuarios.Add(user);
            await _db.SaveChangesAsync();
            await SignInAsync(user);
            return RedirectToAction("Index", "Dashboard");
        }

        // ── Perfil ────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login");

            var vm = new ProfileViewModel
            {
                Nome         = user.Nome,
                Tema         = user.Tema,
                FotoPerfilUrl = user.FotoPerfilUrl
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login");

            user.Nome         = vm.Nome;
            user.Tema         = vm.Tema;
            user.FotoPerfilUrl = vm.FotoPerfilUrl;
            await _db.SaveChangesAsync();

            // Atualiza claim do nome
            await HttpContext.SignOutAsync("CaixaFacilCookie");
            await SignInAsync(user);

            TempData["Sucesso"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Profile");
        }

        // ── Logout ────────────────────────────────────────────────────────────
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CaixaFacilCookie");
            return RedirectToAction("Login");
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private async Task SignInAsync(Usuario user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name,           user.Nome),
                new(ClaimTypes.Email,          user.Email),
                new("Tema",                    user.Tema)
            };
            var identity  = new ClaimsIdentity(claims, "CaixaFacilCookie");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("CaixaFacilCookie", principal);
        }

        private async Task<Usuario?> GetCurrentUserAsync()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idStr, out var id)) return null;
            return await _db.Usuarios.FindAsync(id);
        }
    }
}
