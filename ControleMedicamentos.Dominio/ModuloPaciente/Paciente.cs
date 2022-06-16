using System;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class Paciente : EntidadeBase<Paciente>
    {
        public Paciente()
        {

        }
        public Paciente(int numero) : this()
        {
            Numero = numero;
        }

        public Paciente(string nome, string cartaoSUS) : this()
        {
            Nome = nome;
            CartaoSUS = cartaoSUS;
        }

        public string Nome { get; set; }
        public string CartaoSUS { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Paciente paciente &&
                   Numero == paciente.Numero &&
                   Nome == paciente.Nome &&
                   CartaoSUS == paciente.CartaoSUS;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numero, Nome, CartaoSUS);
        }
    }
}
