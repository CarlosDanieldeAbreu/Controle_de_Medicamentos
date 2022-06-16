using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        private Funcionario funcionario;
        private RepositorioFuncionarioEmBancoDados repositorio;

        public RepositorioFuncionarioEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            funcionario = new Funcionario("Juca da Silva", "12345", "3234DSS56");
            repositorio = new RepositorioFuncionarioEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_Funcionario()
        {
            //action
            repositorio.Inserir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.Numero);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_Funcionario()
        {
            //arrange                      
            repositorio.Inserir(funcionario);

            //action
            funcionario.Nome = "Tião de Morguel";
            funcionario.Senha = "987654321";
            funcionario.Login = "Morgul";
            repositorio.Editar(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.Numero);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_Funcionario()
        {
            //arrange           
            repositorio.Inserir(funcionario);

            //action           
            repositorio.Excluir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.Numero);
            Assert.IsNull(funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_Funcionario()
        {
            //arrange          
            repositorio.Inserir(funcionario);

            //action
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.Numero);

            //assert
            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_Funcionario()
        {
            //arrange
            var f01 = new Funcionario("Albert Eisten", "AlbertGenio", "344545546556565");
            var f02 = new Funcionario("Madre Tereza de Calcuta", "Calcuta1234", "3431sdasd");
            var f03 = new Funcionario("Patricia Abravanel", "AbravanelSantos", "SILVIO3343");
            var f04 = new Funcionario("Irmão do Jorel", "JorelBrothers", "Jorel3443");

            var repositorio = new RepositorioFuncionarioEmBancoDados();
            repositorio.Inserir(f01);
            repositorio.Inserir(f02);
            repositorio.Inserir(f03);
            repositorio.Inserir(f04);

            //action
            var funcionarios = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(4, funcionarios.Count);

            Assert.AreEqual(f01.Nome, funcionarios[0].Nome);
            Assert.AreEqual(f02.Nome, funcionarios[1].Nome);
            Assert.AreEqual(f03.Nome, funcionarios[2].Nome);
            Assert.AreEqual(f04.Nome, funcionarios[3].Nome);
        }
    }
}
