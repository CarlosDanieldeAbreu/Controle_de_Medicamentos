using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorio;

        public RepositorioFornecedorEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new Fornecedor("ACME", "321654987", "acmeSolucoes@gmail.com", "algum lugar", "Texas");
            repositorio = new RepositorioFornecedorEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_fornecedor()
        {
            //action
            repositorio.Inserir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.Numero);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_fornecedor()
        {
            //arrange                      
            repositorio.Inserir(fornecedor);

            //action
            fornecedor.Nome = "ACME Remédios";
            fornecedor.Telefone = "987654321";
            fornecedor.Email = "acmeremedios@gmail.com";
            fornecedor.Cidade = "Em algum lugar";
            fornecedor.Estado = "Texas";
            repositorio.Editar(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.Numero);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange           
            repositorio.Inserir(fornecedor);

            //action           
            repositorio.Excluir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.Numero);
            Assert.IsNull(fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_fornecedor()
        {
            //arrange          
            repositorio.Inserir(fornecedor);

            //action
            var pacienteEncontrado = repositorio.SelecionarPorNumero(fornecedor.Numero);

            //assert
            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(fornecedor, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_fornecedores()
        {
            //arrange
            var f01 = new Fornecedor("ACME", "321654987", "acmeSolucoes@gmail.com", "algum lugar", "Texas");
            var f02 = new Fornecedor("Mitshubish", "453454564", "MitshubishRemedios@gmail.com", "Hiroshima", "Japao");
            var f03 = new Fornecedor("VaticanoSolution", "676756756", "vaticanoTeAjuda@gmail.com", "algum lugar no Vaticano", "Vaticano");

            var repositorio = new RepositorioFornecedorEmBancoDados();
            repositorio.Inserir(f01);
            repositorio.Inserir(f02);
            repositorio.Inserir(f03);

            //action
            var fornecedores = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, fornecedores.Count);

            Assert.AreEqual(f01.Nome, fornecedores[0].Nome);
            Assert.AreEqual(f02.Nome, fornecedores[1].Nome);
            Assert.AreEqual(f03.Nome, fornecedores[2].Nome);
        }
    }
}
