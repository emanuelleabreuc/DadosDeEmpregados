using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDWpfApp1Empregado
{
    class Empregado
    {
        public int Matricula { get; set; } = 0;
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? Endereco { get; set; }
        public Empregado() { }

        public Empregado(string cPF, string nome, string endereco)
        {
            CPF = cPF;
            Nome = nome;
            Endereco = endereco;
        }


        public override string? ToString()
        {

            return $"Empregado -> Matricula: {Matricula} | CPF: {CPF}" +
                   $"Nome: {Nome} | Endereco: {Endereco}";
        }


        public void Salvar(SqlConnection conexao)
        {
            Console.WriteLine("Salvando empregado...");
            if (Matricula == 0)
            {
                var Cmd = conexao.CreateCommand();
                Cmd.CommandText = "INSERT INTO Empregado (CPF, Nome, Endereco) VALUES (@CPF, @Nome, @Endereco)";

                Cmd.Parameters.Add(new SqlParameter("cpf", CPF));
                Cmd.Parameters.Add(new SqlParameter("nome", Nome));
                Cmd.Parameters.Add(new SqlParameter("endereco", Endereco));

                Cmd.ExecuteNonQuery();
            }



        }

        public void Pesquisar() { }

        public void Excluir() { }

        public void Alterar() { }
    }
}
