DROP TABLE [dbo].[Table];

CREATE TABLE [dbo].[Empregado] (
    [Matricula] INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [CPF]       NVARCHAR (MAX) NULL,
    [Nome]      NVARCHAR (MAX) NULL,
    [Endereco]  NVARCHAR (MAX) NULL
);