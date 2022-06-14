using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        private static DB bancoDadosEndereco;
        public RepositorioMedicamentoEmBancoDados()
        {
            bancoDadosEndereco = new DB();
        }

        private string enderecoBanco = bancoDadosEndereco.EnderecoBanco();

        #region Sql Queries
        private const string sqlInserir =
         @"INSERT INTO [TBMedicamento]
                (
                    [NOME],                    
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNCEDOR_ID]
	            )
	            VALUES
                (
                    @NOME,                    
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,
                    @FORNCEDOR_ID

                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBMedicamento]	
		            SET
                        [NOME] = @NOME,                 
                        [DESCRICAO] = @DESCRICAO,
                        [LOTE] = @LOTE,
                        [VALIDADE] = @VALIDADE,
                        [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                        [FORNCEDOR_ID] = @FORNCEDOR_ID
		            WHERE
			            [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBMedicamento]
		            WHERE
			            [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                     TBM.ID
                    ,TBM.NOME
                    ,TBM.DESCRICAO
                    ,TBM.LOTE
                    ,TBM.VALIDADE
                    ,TBM.QUANTIDADEDISPONIVEL
	                ,TBF.ID AS FORNECEDOR_ID
	                ,TBF.NOME AS FORNECEDOR_NOME
                  FROM 
                	TBMEDICAMENTO TBM INNER JOIN TBFORNECEDOR TBF 
                  ON
                	TBM.FORNCEDOR_ID = TBF.ID";

        private const string sqlSelecionarPorNumero =
           @"SELECT 
                     TBM.ID
                    ,TBM.NOME
                    ,TBM.DESCRICAO
                    ,TBM.LOTE
                    ,TBM.VALIDADE
                    ,TBM.QUANTIDADEDISPONIVEL
	                ,TBF.ID AS FORNECEDOR_ID
	                ,TBF.NOME AS FORNECEDOR_NOME
                  FROM 
                	TBMEDICAMENTO TBM INNER JOIN TBFORNECEDOR TBF 
                  ON
                	TBM.FORNCEDOR_ID = TBF.ID
                WHERE 
                    TBM.ID = @ID";

        #endregion

        public ValidationResult Inserir(Medicamento medicamento)
        {
            ValidadorMedicamento validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(medicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosMedicamento(medicamento, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            medicamento.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Medicamento medicamento)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", medicamento.Numero);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            List<Medicamento> medicamentos = new List<Medicamento>();

            while (leitorMedicamento.Read())
            {
                Medicamento medicamento = ConverterParaMedicamento(leitorMedicamento);

                medicamentos.Add(medicamento);
            }

            conexaoComBanco.Close();

            return medicamentos;
        }

        public Medicamento SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", numero);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            Medicamento medicamento = null;

            if (leitorMedicamento.Read())
            {
                medicamento = ConverterParaMedicamento(leitorMedicamento);
            }

            conexaoComBanco.Close();

            return medicamento;
        }

        private void ConfigurarParametrosMedicamento(Medicamento medicamento, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", medicamento.Numero);
            comando.Parameters.AddWithValue("NOME", medicamento.Nome);
            comando.Parameters.AddWithValue("DESCRICAO", medicamento.Descricao);
            comando.Parameters.AddWithValue("LOTE", medicamento.Lote);
            comando.Parameters.AddWithValue("VALIDADE", medicamento.Validade);
            comando.Parameters.AddWithValue("QUANTIDADEDISPONIVEL", medicamento.QuantidadeDisponivel);
            comando.Parameters.AddWithValue("FORNCEDOR_ID", medicamento.Fornecedor.Numero);
        }

        private void ConfigurarParametrosFornecedor(Fornecedor fornecedor, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", fornecedor.Numero);
            comando.Parameters.AddWithValue("NOME", fornecedor.Nome);
        }

        private Medicamento ConverterParaMedicamento(SqlDataReader leitorMedicamento)
        {
            int numero = Convert.ToInt32(leitorMedicamento["ID"]);
            string nome = Convert.ToString(leitorMedicamento["NOME"]);
            string descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            string lote = Convert.ToString(leitorMedicamento["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);
            int quantidadeDisponivel = Convert.ToInt32(leitorMedicamento["QUANTIDADEDISPONIVEL"]);

            int numeroFornecedor = Convert.ToInt32(leitorMedicamento["FORNCEDOR_ID"]);
            string nomeFornecedor = Convert.ToString(leitorMedicamento["FORNECEDOR_NOME"]);

            var fornecedor = new Fornecedor
            {
                Numero = numeroFornecedor,
                Nome = nomeFornecedor
            };

            var medicamento = new Medicamento
            {
                Numero = numero,
                Nome = nome,
                Descricao = descricao,
                Lote = lote,
                Validade = validade,
                QuantidadeDisponivel = quantidadeDisponivel
            };

            medicamento.ConfigurarFornecedor(fornecedor);

            return medicamento;
        }
    }
}
