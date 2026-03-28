using CaixaFacil.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Banco de dados ──────────────────────────────────────────────────────────
var connStr = builder.Configuration.GetConnectionString("MySql")
    ?? throw new InvalidOperationException("Connection string 'MySql' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

// ── MVC ─────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── Autenticação por cookie ──────────────────────────────────────────────────
builder.Services.AddAuthentication("CaixaFacilCookie")
    .AddCookie("CaixaFacilCookie", opt =>
    {
        opt.LoginPath  = "/Account/Login";
        opt.LogoutPath = "/Account/Logout";
        opt.ExpireTimeSpan = TimeSpan.FromDays(7);
        opt.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ── Garante criação do schema ────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ── Pipeline ─────────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
