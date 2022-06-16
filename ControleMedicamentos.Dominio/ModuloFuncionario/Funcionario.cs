using System;

namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class Funcionario : EntidadeBase<Funcionario>
    {
        public Funcionario()
        {

        }
        public Funcionario(int numero) : this()
        {
            Numero = numero;
        }
        public Funcionario(string nome, string login, string senha) : this()
        {
            Nome = nome;
            Login = login;
            Senha = senha;
        }

        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Funcionario funcionario &&
                   Numero == funcionario.Numero &&
                   Nome == funcionario.Nome &&
                   Login == funcionario.Login &&
                   Senha == funcionario.Senha;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numero, Nome, Login, Senha);
        }
    }
}
