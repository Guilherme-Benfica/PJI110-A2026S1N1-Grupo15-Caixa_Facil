# CaixaFácil 💰
### Sistema de Controle Financeiro para Profissionais Autônomos e Pequenas Empresas

Este é o repositório do Projeto Integrador da disciplina **DRP05 - Projeto Integrador em Computação I - Turma 001** da **UNIVESP**. O tema abordado é *"Desenvolvimento de um software com framework web que utilize noções de banco de dados, praticando controle de versão"*.

---

## 👥 Integrantes

- Ana Beatriz Kutil Mejia — RA 24204745
- Antonio Pedro Da Silva — RA 24206532
- Denis Araujo — RA 24204415
- Gisele Cunha Da Silva Braga — RA 1713908
- Guilherme Augusto Benfica Da Costa — RA 24205141
- Janieuson Vicente Leite — RA 1700435
- Leticia Ribeiro — RA 2105997
- Zildineia Conceicao Magri — RA 24228400

**Polos:** Cabreúva e Iperó  
**Orientadora:** Jessika Martins Ribeiro

---

## 📋 Descrição do Projeto

O **CaixaFácil** nasceu da necessidade real identificada em profissionais autônomos e pequenas empresas — como manicures, eletricistas, lanchonetes e prestadores de serviços gerais — que enfrentam dificuldades no controle financeiro do dia a dia, frequentemente resultando em prejuízos por falta de organização e visibilidade sobre ganhos e gastos.

O sistema tem como objetivo **facilitar o registro e a análise de dados financeiros**, tornando a gestão mais intuitiva e acessível, mesmo para quem não tem conhecimento contábil.

---

## ✅ Funcionalidades Principais

- **Cadastro de usuários:** registro seguro com senha criptografada (BCrypt).
- **Dashboard financeiro:** painel com totais de entradas, saídas e saldo do mês, além de gráfico dos últimos 7 dias.
- **Lançamentos:** registro de entradas e saídas com categoria, conta, forma de pagamento, valor, data e descrição. Filtros por tipo, categoria e mês.
- **Categorias:** cadastro de categorias personalizadas (ex: Vendas, Insumos, Aluguel).
- **Contas / Caixas:** gerenciamento de caixas e contas (Dinheiro, Pix, Maquininha, etc.).
- **Tipos de Movimento:** formas de pagamento configuráveis (Pix, Cartão, Dinheiro, etc.).
- **Lançamentos recorrentes:** cadastro de despesas e receitas fixas mensais (aluguel, internet, energia) com geração automática por um clique.
- **Relatório em PDF:** exportação do resumo financeiro mensal com tabela de lançamentos, totais e saldo.
- **Tema claro/escuro:** alternância de tema com persistência no navegador.

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C# (.NET 8 — ASP.NET Core MVC)
- **Frontend:** HTML, CSS, Bootstrap 5, jQuery
- **Banco de Dados:** MySQL 8.0 ou 9.x
- **ORM:** Entity Framework Core com Pomelo (MySQL)
- **Autenticação:** Cookie Authentication + BCrypt
- **Gráficos:** Chart.js
- **Controle de versão:** Git + GitHub

---

## ⚙️ Instalação e Uso

### Pré-requisitos

Certifique-se de ter instalado na sua máquina:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server 8.0 ou 9.x](https://dev.mysql.com/downloads/mysql/)
- [VS Code](https://code.visualstudio.com/) ou qualquer IDE de sua preferência
- [Git](https://git-scm.com/download/win)

---

### 🗄️ Instalando e configurando o MySQL (Windows)

**1. Baixe e instale o MySQL**

Acesse https://dev.mysql.com/downloads/mysql/ e baixe o **MySQL Installer for Windows**.
Durante a instalação escolha **Developer Default** ou **Server Only** e defina uma senha para o usuário `root`.

**2. Verifique se o serviço está rodando**

Abra o PowerShell como **Administrador**:
```powershell
Get-Service | Where-Object {$_.Name -like "*mysql*"}
```

Se o Status aparecer como `Running`, pule para o passo 4. Se aparecer `Stopped`, continue.

**3. Configure o serviço manualmente (caso não inicie automaticamente)**

> Use os comandos abaixo substituindo `9.7` pela versão instalada na sua máquina (ex: `8.0`)

```powershell
# Cria a pasta de dados
New-Item -ItemType Directory -Force "C:\ProgramData\MySQL\MySQL Server 9.7"

# Inicializa o banco (sem senha inicial)
& "C:\Program Files\MySQL\MySQL Server 9.7\bin\mysqld.exe" --initialize-insecure --datadir="C:\ProgramData\MySQL\MySQL Server 9.7\Data"

# Cria o arquivo de configuração
@"
[mysqld]
datadir=C:/ProgramData/MySQL/MySQL Server 9.7/Data
port=3306
"@ | Out-File -Encoding ascii "C:\ProgramData\MySQL\MySQL Server 9.7\my.ini"

# Registra e inicia o serviço
& "C:\Program Files\MySQL\MySQL Server 9.7\bin\mysqld.exe" --install MySQL97 --defaults-file="C:\ProgramData\MySQL\MySQL Server 9.7\my.ini"
Start-Service -Name "MySQL97"
```

**4. Defina a senha do root e crie o banco**

```powershell
cd "C:\Program Files\MySQL\MySQL Server 9.7\bin"
.\mysql -u root
```

Dentro do `mysql>`:
```sql
ALTER USER 'root'@'localhost' IDENTIFIED BY 'SUA_SENHA';
CREATE DATABASE caixafacildb CHARACTER SET utf8mb4;
EXIT;
```

---

### 🚀 Rodando o projeto

**1. Clone o repositório**
```bash
git clone https://github.com/Guilherme-Benfica/PJI110-A2026S1N1-Grupo15-Caixa_Facil.git
cd PJI110-A2026S1N1-Grupo15-Caixa_Facil
```

**2. Configure a connection string**

Copie o arquivo de exemplo e edite com suas credenciais:
```bash
copy appsettings.example.json appsettings.json
notepad appsettings.json
```

Substitua `SUA_SENHA_AQUI` pela sua senha do MySQL:
```json
{
  "ConnectionStrings": {
    "MySql": "Server=localhost;Port=3306;Database=caixafacildb;User=root;Password=SUA_SENHA;"
  }
}
```

> ⚠️ O arquivo `appsettings.json` está no `.gitignore` e **não é versionado** para proteger suas credenciais. Cada integrante precisa criar o seu localmente a partir do `appsettings.example.json`.

**3. Restaure os pacotes e execute**
```bash
dotnet restore
dotnet run
```

**4. Acesse no navegador**
```
http://localhost:5000
```

> 💡 Na primeira execução, o sistema cria automaticamente todas as tabelas e insere dados iniciais (categorias, contas e tipos de movimento).

---

### ❗ Problemas comuns

| Erro | Causa | Solução |
|---|---|---|
| `Unable to connect to any of the specified MySQL hosts` | MySQL não está rodando ou `appsettings.json` não foi criado | Verifique se o serviço MySQL está ativo e se o `appsettings.json` existe |
| `You must install or update .NET` | Versão do .NET diferente da compilação | Delete as pastas `bin/` e `obj/` e rode `dotnet run` novamente |
| `Access denied for user 'root'` | Senha incorreta no `appsettings.json` | Corrija a senha no `appsettings.json` |
| Serviço MySQL não inicia | Conflito entre versões ou pasta de dados não inicializada | Siga o passo 3 de configuração manual do MySQL acima |

---

## 📁 Estrutura do Projeto

```
CaixaFacil/
├── Controllers/              # Lógica de cada tela
├── Data/                     # AppDbContext (EF Core)
├── Models/
│   ├── Usuario.cs
│   ├── Categoria.cs
│   ├── Conta.cs
│   ├── TipoMovimento.cs
│   ├── Lancamento.cs
│   └── ViewModels/           # ViewModels das telas
├── Views/                    # Telas Razor (.cshtml)
├── wwwroot/                  # CSS, JS e arquivos estáticos
├── sql/                      # Script manual do banco
├── appsettings.example.json  # Modelo de configuração (versionado)
├── appsettings.json          # Configuração local (NÃO versionado)
└── Program.cs                # Inicialização da aplicação
```

---

*Projeto desenvolvido para fins acadêmicos — UNIVESP 2026*