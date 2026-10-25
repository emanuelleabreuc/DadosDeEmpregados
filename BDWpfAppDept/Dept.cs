using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BDWpfAppDept
{
    public class Dept
    {
        public int Id { get; set; } = 0;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Dept() { }

        public Dept(string name, string email)
        {
            Name = name;
            Email = email;
        }


        public override string? ToString()
        {

            return $"Dept -> Id: {Id} | Name: {Name} | Email: {Email}";
        }


        public void Salvar(SqlConnection conexao)
        {
            if (Id == 0) //o id sera gerado pelo BD
            {
                using var Cmd = conexao.CreateCommand();
                Cmd.CommandText = "INSERT INTO DEPT (name, email) VALUES (@name, @email)";

                Cmd.Parameters.Add(new SqlParameter("@name", Name));
                Cmd.Parameters.Add(new SqlParameter("@email", Email));

                Cmd.ExecuteNonQuery();
            }
        }

        public List<Dept>? Recuperar(int idParam, SqlConnection conexao)
        {
            List<Dept>? DeptSLido = null;

            using var Cmd = conexao.CreateCommand();

            if (idParam == 0)
            {
                Cmd.CommandText = "SELECT * FROM DEPT";
            }
            else
            {
                Cmd.CommandText = "SELECT * FROM DEPT WHERE id = @idBusca";
                Cmd.Parameters.Add(new SqlParameter("@idBusca", idParam));
            }

            using var resultado = Cmd.ExecuteReader();

            DeptSLido = new();

            while (resultado.Read())
            {
                Dept DeptLido = new()
                {
                    Id = resultado.GetInt32("id"),
                    Name = resultado.GetString("name"),
                    Email = resultado.GetString("email")
                };

                DeptSLido.Add(DeptLido);
            }

            return DeptSLido;
        }
    }
}
