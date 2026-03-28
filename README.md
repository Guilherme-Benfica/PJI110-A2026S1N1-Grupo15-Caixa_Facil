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
- **Tema claro/escuro:** alternância de tema com persistência no navegador.

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C# (.NET 8 — ASP.NET Core MVC)
- **Frontend:** HTML, CSS, Bootstrap 5, jQuery
- **Banco de Dados:** MySQL 8.0
- **ORM:** Entity Framework Core com Pomelo (MySQL)
- **Autenticação:** Cookie Authentication + BCrypt
- **Gráficos:** Chart.js
- **Controle de versão:** Git + GitHub

---

## ⚙️ Instalação e Uso

Siga os passos abaixo para rodar o projeto localmente:

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server 8.0](https://dev.mysql.com/downloads/mysql/)
- [VS Code](https://code.visualstudio.com/) ou qualquer IDE de sua preferência

### Passo a passo

**1. Clone o repositório**
```bash
git clone https://github.com/Guilherme-Benfica/PJI110-A2026S1N1-Grupo15-Caixa_Facil.git
cd PJI110-A2026S1N1-Grupo15-Caixa_Facil
```

**2. Configure o banco de dados**

No MySQL, crie o banco:
```sql
CREATE DATABASE caixafacildb CHARACTER SET utf8mb4;
```

**3. Configure a connection string**

Abra o arquivo `appsettings.json` e substitua com suas credenciais:
```json
"MySql": "Server=localhost;Port=3306;Database=caixafacildb;User=root;Password=SUA_SENHA;"
```

**4. Restaure os pacotes e execute**
```bash
dotnet restore
dotnet run
```

**5. Acesse no navegador**
```
http://localhost:5000
```

> 💡 Na primeira execução, o sistema cria automaticamente todas as tabelas e insere dados iniciais (categorias, contas e tipos de movimento).

---

## 📁 Estrutura do Projeto

```
CaixaFacil/
├── Controllers/       # Lógica de cada tela
├── Data/              # AppDbContext (EF Core)
├── Models/            # Entidades e ViewModels
├── Views/             # Telas Razor (.cshtml)
├── wwwroot/           # CSS, JS e arquivos estáticos
├── sql/               # Script manual do banco
├── appsettings.json   # Configurações e connection string
└── Program.cs         # Inicialização da aplicação
```

---

## 🤝 Contribuição

Este projeto está aberto para contribuições. Sinta-se à vontade para enviar sugestões, correções ou novas funcionalidades através de *pull requests* 🚀

---

*Projeto desenvolvido para fins acadêmicos — UNIVESP 2026*
