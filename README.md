# CaixaFácil 💰
**Controle financeiro simples para pequenos estabelecimentos**
> Ideal para: manicures, lanchonetes (hot-dog, pastel), salões, barbearias e similares.

---

## Funcionalidades
- ✅ Cadastro e login com senha criptografada (BCrypt)
- ✅ Dashboard com totais do mês (Entradas, Saídas, Saldo) + gráfico de barras (Chart.js)
- ✅ Lançamentos com filtro por tipo, categoria e mês
- ✅ CRUDs: Categorias, Contas/Caixas, Tipos de Movimento
- ✅ Tema claro/escuro com persistência em localStorage
- ✅ Layout responsivo (Bootstrap 5 + jQuery)

---

## Requisitos
| Ferramenta       | Versão mínima |
|------------------|---------------|
| .NET SDK         | 8.0           |
| MySQL Server     | 8.0           |
| VS Code          | qualquer      |
| C# Extension     | recomendada   |

---

## Configuração rápida (Windows + VS Code)

### 1. Configurar o banco de dados
```sql
-- No MySQL Workbench ou terminal mysql:
CREATE DATABASE caixafacildb CHARACTER SET utf8mb4;
```

### 2. Ajustar a connection string
Abra `appsettings.json` e troque `SUA_SENHA_AQUI` pela sua senha do MySQL:
```json
"MySql": "Server=localhost;Port=3306;Database=caixafacildb;User=root;Password=SENHA_AQUI;"
```

### 3. Restaurar pacotes e executar
```bash
cd CaixaFacil
dotnet restore
dotnet build
dotnet run
```

Acesse em: **https://localhost:5001** ou **http://localhost:5000**

---

## Estrutura do projeto
```
CaixaFacil/
├── Controllers/
│   ├── AccountController.cs        # Login, Cadastro, Perfil
│   ├── DashboardController.cs      # Painel principal
│   ├── LancamentosController.cs    # CRUD lançamentos
│   ├── CategoriasController.cs
│   ├── ContasController.cs
│   └── TiposMovimentoController.cs
├── Data/
│   └── AppDbContext.cs             # EF Core + seed data
├── Models/
│   └── Models.cs                   # Entidades + ViewModels
├── Views/
│   ├── Shared/_Layout.cshtml       # Layout com Navbar e tema
│   ├── Dashboard/Index.cshtml      # Painel
│   ├── Account/                    # Login, Cadastro, Perfil
│   ├── Lancamentos/                # CRUD + filtros
│   ├── Categorias/
│   ├── Contas/
│   └── TiposMovimento/
├── wwwroot/
│   ├── css/site.css
│   └── js/theme.js
├── sql/
│   └── schema.sql                  # Script manual do banco
├── appsettings.json
├── Program.cs
└── CaixaFacil.csproj
```

---

## Banco de dados

O projeto usa **`Database.EnsureCreated()`** — ao rodar pela primeira vez, o EF Core cria todas as tabelas automaticamente, incluindo os dados de seed (categorias, contas, tipos de movimento).

Se preferir criar manualmente, execute: `sql/schema.sql`

### Migrations (opcional, para evoluções futuras)
```bash
# Adicionar pacote de tools (uma vez)
dotnet tool install --global dotnet-ef

# Criar migration inicial
dotnet ef migrations add InitialCreate

# Aplicar ao banco
dotnet ef database update
```

---

## Dados de seed incluídos
**Categorias:** Vendas, Serviços, Insumos, Aluguel, Energia/Água, Funcionários, Marketing, Outros

**Contas:** Caixa (dinheiro), Pix/TED, Maquininha

**Tipos de Movimento:** Dinheiro, Pix, Cartão de Débito, Cartão de Crédito, Transferência, Nota Fiscal

---

## Exemplos de uso
- **Manicure:** Lança "Entrada > Serviços > Maquininha" para cada atendimento; "Saída > Insumos > Pix" para compra de esmaltes.
- **Lanchonete:** Lança "Entrada > Vendas > Dinheiro" ao fechar o caixa; "Saída > Insumos > Pix" para compra de ingredientes.
