-- ============================================================
-- CaixaFácil — Schema MySQL
-- Banco: caixafacildb
-- Uso: execute este script antes de rodar o projeto pela
--      primeira vez se preferir criar as tabelas manualmente.
--      Com EnsureCreated(), o EF Core cria automaticamente.
-- ============================================================

CREATE DATABASE IF NOT EXISTS caixafacildb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE caixafacildb;

-- Usuários
CREATE TABLE IF NOT EXISTS Usuarios (
    Id           INT          NOT NULL AUTO_INCREMENT,
    Nome         VARCHAR(120) NOT NULL,
    Email        VARCHAR(200) NOT NULL,
    SenhaHash    VARCHAR(255) NOT NULL,
    Tema         VARCHAR(20)  NOT NULL DEFAULT 'system',
    FotoPerfilUrl VARCHAR(500) NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY UQ_Usuarios_Email (Email)
) ENGINE=InnoDB;

-- Categorias
CREATE TABLE IF NOT EXISTS Categorias (
    Id       INT          NOT NULL AUTO_INCREMENT,
    Nome     VARCHAR(100) NOT NULL,
    Descricao VARCHAR(250) NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB;

-- Contas / Caixas
CREATE TABLE IF NOT EXISTS Contas (
    Id    INT          NOT NULL AUTO_INCREMENT,
    Nome  VARCHAR(100) NOT NULL,
    Banco VARCHAR(100) NULL,
    Tipo  VARCHAR(50)  NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB;

-- Tipos de Movimento (forma de pagamento)
CREATE TABLE IF NOT EXISTS TiposMovimento (
    Id   INT         NOT NULL AUTO_INCREMENT,
    Nome VARCHAR(80) NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB;

-- Lançamentos
CREATE TABLE IF NOT EXISTS Lancamentos (
    Id              INT            NOT NULL AUTO_INCREMENT,
    UsuarioId       INT            NOT NULL,
    CategoriaId     INT            NOT NULL,
    ContaId         INT            NOT NULL,
    TipoMovimentoId INT            NOT NULL,
    Tipo            VARCHAR(10)    NOT NULL,
    Valor           DECIMAL(10,2)  NOT NULL,
    Data            DATE           NOT NULL,
    Descricao       VARCHAR(500)   NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Lancamentos_Usuarios       FOREIGN KEY (UsuarioId)       REFERENCES Usuarios(Id),
    CONSTRAINT FK_Lancamentos_Categorias     FOREIGN KEY (CategoriaId)     REFERENCES Categorias(Id),
    CONSTRAINT FK_Lancamentos_Contas         FOREIGN KEY (ContaId)         REFERENCES Contas(Id),
    CONSTRAINT FK_Lancamentos_TiposMovimento FOREIGN KEY (TipoMovimentoId) REFERENCES TiposMovimento(Id),
    CONSTRAINT CK_Lancamentos_Tipo CHECK (Tipo IN ('Entrada','Saída'))
) ENGINE=InnoDB;

-- Dados iniciais de demonstração
INSERT IGNORE INTO Categorias (Id, Nome, Descricao) VALUES
(1, 'Vendas',       'Receita de vendas do dia'),
(2, 'Serviços',     'Receita de serviços prestados'),
(3, 'Insumos',      'Compra de materiais e ingredientes'),
(4, 'Aluguel',      'Aluguel do espaço'),
(5, 'Energia/Água', 'Contas de utilidades'),
(6, 'Funcionários', 'Salários e pró-labore'),
(7, 'Marketing',    'Divulgação e propaganda'),
(8, 'Outros',       'Demais receitas e despesas');

INSERT IGNORE INTO Contas (Id, Nome, Banco, Tipo) VALUES
(1, 'Caixa',      NULL,      'Dinheiro'),
(2, 'Pix / TED',  NULL,      'Transferência'),
(3, 'Maquininha', NULL,      'Cartão');

INSERT IGNORE INTO TiposMovimento (Id, Nome) VALUES
(1, 'Dinheiro'),
(2, 'Pix'),
(3, 'Cartão de Débito'),
(4, 'Cartão de Crédito'),
(5, 'Transferência'),
(6, 'Nota Fiscal');
