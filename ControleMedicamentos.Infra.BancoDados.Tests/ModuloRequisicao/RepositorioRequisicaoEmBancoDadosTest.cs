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
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
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
            Assert.AreEqual(requisicao, requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorio.Inserir(requisicao);

            //action
            requisicao.Medicamento.Numero = 3;
            requisicao.Paciente.Numero = 2;
            requisicao.QtdMedicamento = 5;
            requisicao.Data = DateTime.Now;
            requisicao.Funcionario.Numero = 1;

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorNumero(requisicao.Numero);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao, requisicaoEncontrado);
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
            Assert.AreEqual(requisicao, requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_medicamento()
        {
            //arrange
            paciente = new Paciente(2);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            var r01 = new Requisicao(medicamento, paciente, 3, DateTime.Now, funcionario);
            paciente = new Paciente(1);
            funcionario = new Funcionario(4);
            medicamento = new Medicamento(2);
            var r02 = new Requisicao(medicamento, paciente, 6, DateTime.Now, funcionario);
            paciente = new Paciente(3);
            funcionario = new Funcionario(2);
            medicamento = new Medicamento(3);
            var r03 = new Requisicao(medicamento, paciente, 4, DateTime.Now, funcionario);
            paciente = new Paciente(1);
            funcionario = new Funcionario(3);
            medicamento = new Medicamento(4);
            var r04 = new Requisicao(medicamento, paciente, 2, DateTime.Now, funcionario);

            var repositorio = new RepositorioRequisicaoEmBancoDados();
            repositorio.Inserir(r01);
            repositorio.Inserir(r02);
            repositorio.Inserir(r03);
            repositorio.Inserir(r04);

            //action
            var medicamentos = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(4, medicamentos.Count);

            Assert.AreEqual(r01.Data, medicamentos[0].Data);
            Assert.AreEqual(r02.Data, medicamentos[1].Data);
            Assert.AreEqual(r03.Data, medicamentos[2].Data);
            Assert.AreEqual(r04.Data, medicamentos[3].Data);
        }
    }
}
