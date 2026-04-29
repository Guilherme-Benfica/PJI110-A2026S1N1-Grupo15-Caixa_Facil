using CaixaFacil.Models;
using Microsoft.EntityFrameworkCore;

namespace CaixaFacil.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario>               Usuarios               => Set<Usuario>();
        public DbSet<Categoria>             Categorias             => Set<Categoria>();
        public DbSet<Conta>                 Contas                 => Set<Conta>();
        public DbSet<TipoMovimento>         TiposMovimento         => Set<TipoMovimento>();
        public DbSet<Lancamento>            Lancamentos            => Set<Lancamento>();
        public DbSet<LancamentoRecorrente>  LancamentosRecorrentes => Set<LancamentoRecorrente>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único para e-mail
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Precisão decimal — Lancamento
            modelBuilder.Entity<Lancamento>()
                .Property(l => l.Valor)
                .HasPrecision(10, 2);

            // Precisão decimal — LancamentoRecorrente
            modelBuilder.Entity<LancamentoRecorrente>()
                .Property(r => r.Valor)
                .HasPrecision(10, 2);

            // Dados iniciais (seed)
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Vendas",       Descricao = "Receita de vendas do dia" },
                new Categoria { Id = 2, Nome = "Serviços",     Descricao = "Receita de serviços prestados" },
                new Categoria { Id = 3, Nome = "Insumos",      Descricao = "Compra de materiais e ingredientes" },
                new Categoria { Id = 4, Nome = "Aluguel",      Descricao = "Aluguel do espaço" },
                new Categoria { Id = 5, Nome = "Energia/Água", Descricao = "Contas de utilidades" },
                new Categoria { Id = 6, Nome = "Funcionários", Descricao = "Salários e pró-labore" },
                new Categoria { Id = 7, Nome = "Marketing",    Descricao = "Divulgação e propaganda" },
                new Categoria { Id = 8, Nome = "Outros",       Descricao = "Demais receitas e despesas" }
            );

            modelBuilder.Entity<Conta>().HasData(
                new Conta { Id = 1, Nome = "Caixa",      Tipo = "Dinheiro" },
                new Conta { Id = 2, Nome = "Pix / TED",  Tipo = "Transferência" },
                new Conta { Id = 3, Nome = "Maquininha", Tipo = "Cartão" }
            );

            modelBuilder.Entity<TipoMovimento>().HasData(
                new TipoMovimento { Id = 1, Nome = "Dinheiro" },
                new TipoMovimento { Id = 2, Nome = "Pix" },
                new TipoMovimento { Id = 3, Nome = "Cartão de Débito" },
                new TipoMovimento { Id = 4, Nome = "Cartão de Crédito" },
                new TipoMovimento { Id = 5, Nome = "Transferência" },
                new TipoMovimento { Id = 6, Nome = "Nota Fiscal" }
            );
        }
    }
}
