using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        private Requisicao requisicao;
        private Paciente paciente;
        private Funcionario funcionario;
        private Medicamento medicamento;
        private RepositorioRequisicaoEmBancoDados repositorio;

        private DateTime validade = new DateTime(2022, 10, 04);
        public RepositorioRequisicaoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            paciente = new Paciente(1);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            requisicao = new Requisicao(medicamento, paciente, 3, DateTime.Now, funcionario);
            repositorio = new RepositorioRequisicaoEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_medicamento()
        {
            //action
            repositorio.Inserir(requisicao);

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorNumero(requisicao.Numero);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao.Numero, requisicaoEncontrado.Numero);
        }

        [TestMethod]
        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorio.Inserir(requisicao);

            //action
            requisicao.Medicamento.Numero = 1;
            requisicao.Paciente.Numero = 2;
            requisicao.QtdMedicamento = 5;
            requisicao.Data = DateTime.Now;
            requisicao.Funcionario.Numero = 1;

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorNumero(requisicao.Numero);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao.Numero, requisicaoEncontrado.Numero);
        }

        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorio.Inserir(requisicao);

            //action           
            repositorio.Excluir(requisicao);

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorNumero(requisicao.Numero);
            Assert.IsNull(requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorio.Inserir(requisicao);

            //action
            var requisicaoEncontrado = repositorio.SelecionarPorNumero(requisicao.Numero);

            //assert
            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao.Numero, requisicaoEncontrado.Numero);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_medicamento()
        {
            //arrange
            paciente = new Paciente(1);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            var r01 = new Requisicao(medicamento, paciente, 1, DateTime.Now, funcionario);

            paciente = new Paciente(1);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            var r02 = new Requisicao(medicamento, paciente, 2, DateTime.Now, funcionario);

            paciente = new Paciente(1);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            var r03 = new Requisicao(medicamento, paciente, 3, DateTime.Now, funcionario);

            paciente = new Paciente(1);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            var r04 = new Requisicao(medicamento, paciente, 4, DateTime.Now, funcionario);

            var repositorio = new RepositorioRequisicaoEmBancoDados();
            repositorio.Inserir(r01);
            repositorio.Inserir(r02);
            repositorio.Inserir(r03);
            repositorio.Inserir(r04);

            //action
            var requisicoes = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(4, requisicoes.Count);

            Assert.AreEqual(r01.Numero, requisicoes[0].Numero);
            Assert.AreEqual(r02.Numero, requisicoes[1].Numero);
            Assert.AreEqual(r03.Numero, requisicoes[2].Numero);
            Assert.AreEqual(r04.Numero, requisicoes[3].Numero);
        }
    }
}
