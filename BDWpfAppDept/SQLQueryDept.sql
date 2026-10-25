-- Banco: ExerciciosDB
IF OBJECT_ID('dbo.DEPT', 'U') IS NULL
CREATE TABLE [dbo].[DEPT] (
    [id]    INT          IDENTITY (1, 1) NOT NULL,
    [name]  VARCHAR (30) NOT NULL,
    [email] VARCHAR (30) NOT NULL,
    CONSTRAINT [DEPT_PK] PRIMARY KEY ([id])
);
