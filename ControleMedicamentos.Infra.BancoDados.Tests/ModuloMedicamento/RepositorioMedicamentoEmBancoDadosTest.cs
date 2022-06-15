using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private RepositorioMedicamentoEmBancoDados repositorio;

        private DateTime validade = new DateTime(2022,10,04);

        //public RepositorioMedicamentoEmBancoDadosTest()
        //{
        //    Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
        //    medicamento = new Medicamento("Dipirona", "Para dores intensas", "3234DSS56", validade, 23, 1);
        //    repositorio = new RepositorioMedicamentoEmBancoDados();
        //}

        //[TestMethod]
        //public void Deve_inserir_novo_medicamento()
        //{
        //    //action
        //    repositorio.Inserir(medicamento);

        //    //assert
        //    var medicamentoEncontrado = repositorio.SelecionarPorNumero(medicamento.Numero);

        //    Assert.IsNotNull(medicamentoEncontrado);
        //    Assert.AreEqual(medicamento, medicamentoEncontrado);
        //}

        //[TestMethod]
        //public void Deve_editar_informacoes_medicamento()
        //{
        //    //arrange                      
        //    repositorio.Inserir(medicamento);

        //    //action
        //    medicamento.Nome = "Mata Leão";
        //    medicamento.Descricao = "derruba ate o maior dos animais";
        //    medicamento.Lote = "3424rer";
        //    medicamento.Validade = DateTime.Now;
        //    medicamento.QuantidadeDisponivel = 34;
        //    medicamento.Fornecedor.Numero = 1;
        //    repositorio.Editar(medicamento);

        //    //assert
        //    var medicamentoEncontrado = repositorio.SelecionarPorNumero(medicamento.Numero);

        //    Assert.IsNotNull(medicamentoEncontrado);
        //    Assert.AreEqual(medicamento, medicamentoEncontrado);
        //}

        //[TestMethod]
        //public void Deve_excluir_medicamento()
        //{
        //    //arrange           
        //    repositorio.Inserir(medicamento);

        //    //action           
        //    repositorio.Excluir(medicamento);

        //    //assert
        //    var medicamentoEncontrado = repositorio.SelecionarPorNumero(medicamento.Numero);
        //    Assert.IsNull(medicamentoEncontrado);
        //}

        //[TestMethod]
        //public void Deve_selecionar_apenas_um_medicamento()
        //{
        //    //arrange          
        //    repositorio.Inserir(medicamento);

        //    //action
        //    var medicamentoEncontrado = repositorio.SelecionarPorNumero(medicamento.Numero);

        //    //assert
        //    Assert.IsNotNull(medicamentoEncontrado);
        //    Assert.AreEqual(medicamento, medicamentoEncontrado);
        //}

        //[TestMethod]
        //public void Deve_selecionar_todos_um_medicamento()
        //{
        //    var validade1 = new DateTime(2022, 10, 20);
        //    var validade2 = new DateTime(2022, 11, 20);
        //    var validade3 = new DateTime(2022, 09, 20);
        //    var validade4 = new DateTime(2022, 07, 20);

        //    //arrange
        //    var m01 = new Medicamento("Diploquiu", "????", "5435fsdfsd", validade1, 24);
        //    var m02 = new Medicamento("Água Benta", "Bom para espirito ruim", "dfsrwr435fs", validade2, 27);
        //    var m03 = new Medicamento("Varinha de marmelo", "Bom para criança teimosa", "sfs34234fds", validade3, 56);
        //    var m04 = new Medicamento("Amansa Louco", "Como o nome já diz 'amansa louco'", "sfs5345sdfdfs", validade4, 67);

        //    var repositorio = new RepositorioMedicamentoEmBancoDados();
        //    repositorio.Inserir(m01);
        //    repositorio.Inserir(m02);
        //    repositorio.Inserir(m03);
        //    repositorio.Inserir(m04);

        //    //action
        //    var medicamentos = repositorio.SelecionarTodos();

        //    //assert

        //    Assert.AreEqual(4, medicamentos.Count);

        //    Assert.AreEqual(m01.Nome, medicamentos[0].Nome);
        //    Assert.AreEqual(m02.Nome, medicamentos[1].Nome);
        //    Assert.AreEqual(m03.Nome, medicamentos[2].Nome);
        //    Assert.AreEqual(m04.Nome, medicamentos[3].Nome);
        //}
    }
}
