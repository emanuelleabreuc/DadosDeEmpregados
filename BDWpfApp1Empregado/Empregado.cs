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

        // 🟪 Bloco roxo de baixo: ToString
        public override string? ToString()
        {
            //return base.ToString();
            return $"Empregado -> Matricula: {Matricula} | CPF: {CPF}" +
                   $"Nome: {Nome} | Endereco: {Endereco}";
        }

        // 🟨 Bloco amarelo: métodos (vazios por enquanto)
        public void Salvar() { }

        public void Pesquisar() { }

        public void Excluir() { }

        public void Alterar() { }
    }
}
