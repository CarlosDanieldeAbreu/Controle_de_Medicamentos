using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDados
    {
        private static DBendreco bancoDadosEndereco;
        public RepositorioFornecedorEmBancoDados()
        {
            bancoDadosEndereco = new DBendreco();
        }

        private string enderecoBanco = bancoDadosEndereco.EnderecoBanco();

        #region SQL Queries
        private const string sqlInserir =
            @"INSERT INTO [TBFornecedor]
                (
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
                )    
                 VALUES
                (
                    @NOME,
                    @TELEFONE,
                    @EMAIL,
                    @CIDADE,
                    @ESTADO
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBFornecedor]	
		        SET
                    [NOME] = @NOME,
                    [TELEFONE] = @TELEFONE,
                    [EMAIL] = @EMAIL,
                    [CIDADE] = @CIDADE,
                    [ESTADO] = @ESTADO
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBFornecedor]
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
		            [ID], 
		            [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO] 
	            FROM 
		            [TBFornecedor]";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
		            [ID], 
		            [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
	            FROM 
		            [TBFornecedor]
		        WHERE
                    [ID] = @ID";

        #endregion

        public ValidationResult Inserir(Fornecedor novaFornecedor)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(novaFornecedor);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            SqlCommand cmdInserir = new SqlCommand(sqlInserir, conexao);

            ConfigurarParametrosFornecedor(novaFornecedor, cmdInserir);
            conexao.Open();

            var numero = cmdInserir.ExecuteScalar();

            novaFornecedor.Numero = Convert.ToInt32(numero);
            conexao.Close();

            return resultadoValidacao;

        }

        public ValidationResult Editar(Fornecedor fornecedor)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(fornecedor);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosFornecedor(fornecedor, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Fornecedor fornecedor)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", fornecedor.Numero);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Fornecedor> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            List<Fornecedor> fornecedores = new List<Fornecedor>();

            while (leitorFornecedor.Read())
            {
                Fornecedor fornecedor = ConverterParaFornecedor(leitorFornecedor);

                fornecedores.Add(fornecedor);
            }

            conexaoComBanco.Close();

            return fornecedores;
        }

        public Fornecedor SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", numero);

            conexaoComBanco.Open();
            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            Fornecedor fornecedor = null;
            if (leitorFornecedor.Read())
                fornecedor = ConverterParaFornecedor(leitorFornecedor);

            conexaoComBanco.Close();

            return fornecedor;
        }

        private Fornecedor ConverterParaFornecedor(SqlDataReader leitorFornecedor)
        {
            int numero = Convert.ToInt32(leitorFornecedor["ID"]);
            string nome = Convert.ToString(leitorFornecedor["NOME"]);
            string telefone = Convert.ToString(leitorFornecedor["TELEFONE"]);
            string email = Convert.ToString(leitorFornecedor["EMAIL"]);
            string cidade = Convert.ToString(leitorFornecedor["CIDADE"]);
            string estado = Convert.ToString(leitorFornecedor["ESTADO"]);

            var fornecedor = new Fornecedor
            {
                Numero = numero,
                Nome = nome,
                Telefone = telefone,
                Email = email,
                Cidade = cidade,
                Estado = estado
            };

            return fornecedor;
        }

        private static void ConfigurarParametrosFornecedor(Fornecedor novaFornecedor, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", novaFornecedor.Numero);
            cmdInserir.Parameters.AddWithValue("NOME", novaFornecedor.Nome);
            cmdInserir.Parameters.AddWithValue("TELEFONE", novaFornecedor.Telefone);
            cmdInserir.Parameters.AddWithValue("EMAIL", novaFornecedor.Email);
            cmdInserir.Parameters.AddWithValue("CIDADE", novaFornecedor.Cidade);
            cmdInserir.Parameters.AddWithValue("ESTADO", novaFornecedor.Estado);
        }
    }
}
