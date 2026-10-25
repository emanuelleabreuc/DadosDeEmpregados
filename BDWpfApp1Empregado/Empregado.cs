using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BDWpfApp1Empregado
{
    public class Empregado
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

            return $"Empregado -> Matricula: {Matricula} | CPF: {CPF} | " +
                   $"Nome: {Nome} | Endereco: {Endereco}";
        }


        public void Salvar(SqlConnection conexao)
        {
            if (Matricula == 0)
            {
                using var Cmd = conexao.CreateCommand();
                Cmd.CommandText = "INSERT INTO Empregado (CPF, Nome, Endereco) VALUES (@CPF, @Nome, @Endereco)";

                Cmd.Parameters.Add(new SqlParameter("@CPF", CPF));
                Cmd.Parameters.Add(new SqlParameter("@Nome", Nome));
                Cmd.Parameters.Add(new SqlParameter("@Endereco", Endereco));

                Cmd.ExecuteNonQuery();
            }



        }

        public List<Empregado>? Pesquisar(int matriculaParam, SqlConnection conexao)
        {
            List<Empregado>? EmpregadoSLido = null;

            using var Cmd = conexao.CreateCommand();

            if(matriculaParam == 0)
            {
                Cmd.CommandText = "SELECT * FROM Empregado";
            }
            else
            {
                Cmd.CommandText = "SELECT * FROM Empregado WHERE Matricula = @matriculaBusca";
                Cmd.Parameters.Add(new SqlParameter("@matriculaBusca", matriculaParam));
            }
            // o using fecha o reader mesmo se der erro no meio da leitura
            using var resultado = Cmd.ExecuteReader();

            EmpregadoSLido = new();

            while (resultado.Read())
            {
                Empregado EmpregadoLido = new()
                {
                    Matricula = resultado.GetInt32("Matricula"),
                    CPF = LerTexto(resultado, "CPF"),
                    Nome = LerTexto(resultado, "Nome"),
                    Endereco = LerTexto(resultado, "Endereco")
                };

                EmpregadoSLido.Add(EmpregadoLido);
            }

            return EmpregadoSLido;
        }

        // as colunas aceitam NULL, e GetString lanca excecao nesse caso
        private static string? LerTexto(SqlDataReader leitor, string coluna)
        {
            int indice = leitor.GetOrdinal(coluna);
            return leitor.IsDBNull(indice) ? null : leitor.GetString(indice);
        }



        public int Excluir(SqlConnection conexao)
        {
            using var Cmd = conexao.CreateCommand();
            Cmd.CommandText = "DELETE FROM Empregado WHERE Matricula = @Matricula";
            Cmd.Parameters.Add(new SqlParameter("@Matricula", Matricula));

            return Cmd.ExecuteNonQuery();
        }

        public void Alterar() { }
    }
}
