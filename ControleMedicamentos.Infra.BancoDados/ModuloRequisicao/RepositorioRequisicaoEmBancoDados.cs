using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private static DBendreco bancoDadosEndereco = new DBendreco();

        private string enderecoBanco = bancoDadosEndereco.EnderecoBanco();

        private const string sqlInserir =
          @"INSERT INTO [TBRequisicao]
                (
                    [FUNCIONARIO_ID],                    
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID],
                    [QUANTIDADEMEDICAMENTO],
                    [DATA]
	            )
	            VALUES
                (
                    @FUNCIONARIO_ID,
                    @PACIENTE_ID,
                    @MEDICAMENTO_ID,
                    @QUANTIDADEMEDICAMENTO,
                    @DATA

                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBRequisicao]	
		        SET
                    [FUNCIONARIO_ID] = @FUNCIONARIO_ID,            
                    [PACIENTE_ID] = @PACIENTE_ID,
                    [MEDICAMENTO_ID] = @MEDICAMENTO_ID,
                    [QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
                    [DATA] = @DATA
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBRequisicao]
		        WHERE
			        [ID] = @ID";


        private const string sqlSelecionarTodos =
             @"SELECT TBR.ID
                     ,TBR.MEDICAMENTO_ID
                     ,TBR.QUANTIDADEMEDICAMENTO
                     ,TBR.[DATA]
                     ,TBF.ID FUNCIONARIO_NUMERO
                     ,TBF.NOME FUNCIONARIO_NOME
                     ,TBP.ID PACIENTE_NUMERO
                     ,TBP.NOME PACIENTE_NOME
                     ,TBP.CARTAOSUS
                     ,TBM.ID MEDICAMENTO_NUMERO
                     ,TBM.NOME MEDICAMENTO_NOME
                     ,TBM.LOTE
                     ,TBM.VALIDADE
                 
                 FROM 
                    TBREQUISICAO TBR INNER JOIN TBFORNECEDOR TBF 
                 ON
                    TBF.ID = TBR.FUNCIONARIO_ID INNER JOIN TBPACIENTE TBP
                 ON
                    TBP.ID = TBR.PACIENTE_ID INNER JOIN TBMEDICAMENTO TBM
                 ON
                   TBM.ID = TBR.MEDICAMENTO_ID";

        private const string sqlSelecionarPorNumero =
            @"SELECT TBR.ID
                     ,TBR.QUANTIDADEMEDICAMENTO
                     ,TBR.[DATA]
                     ,TBF.ID FUNCIONARIO_NUMERO
                     ,TBF.NOME FUNCIONARIO_NOME
                     ,TBP.ID PACIENTE_NUMERO
                     ,TBP.NOME PACIENTE_NOME
                     ,TBP.CARTAOSUS
                     ,TBM.ID MEDICAMENTO_NUMERO
                     ,TBM.NOME MEDICAMENTO_NOME
                     ,TBM.LOTE
                     ,TBM.VALIDADE
                 
                 FROM 
                    TBREQUISICAO TBR INNER JOIN TBFORNECEDOR TBF 
                 ON
                    TBF.ID = TBR.FUNCIONARIO_ID INNER JOIN TBPACIENTE TBP
                 ON
                    TBP.ID = TBR.PACIENTE_ID INNER JOIN TBMEDICAMENTO TBM
                 ON
                   TBM.ID = TBR.MEDICAMENTO_ID
                WHERE 
	                TBR.ID = @ID";

        public ValidationResult Inserir(Requisicao requisicao)
        {
            ValidadorRequisicao validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            requisicao.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Requisicao requisicao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", requisicao.Numero);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (leitorRequisicao.Read())
            {
                Requisicao requisicao = ConverterParaRiquisicao(leitorRequisicao);

                requisicoes.Add(requisicao);
            }

            conexaoComBanco.Close();

            return requisicoes;
        }

        public Requisicao SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", numero);

            conexaoComBanco.Open();
            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            Requisicao requisicao = null;

            if (leitorRequisicao.Read())
            {
                requisicao = ConverterParaRiquisicao(leitorRequisicao);
            }

            conexaoComBanco.Close();

            return requisicao;
        }

        private void ConfigurarParametrosRequisicao(Requisicao riquisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", riquisicao.Numero);
            comando.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", riquisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("DATA", riquisicao.Data);
            comando.Parameters.AddWithValue("FUNCIONARIO_ID", riquisicao.Funcionario.Numero);
            comando.Parameters.AddWithValue("PACIENTE_ID", riquisicao.Paciente.Numero);
            comando.Parameters.AddWithValue("MEDICAMENTO_ID", riquisicao.Medicamento.Numero);
        }

        private Requisicao ConverterParaRiquisicao(SqlDataReader leitorRequisicao)
        {
            int numero = Convert.ToInt32(leitorRequisicao["ID"]);
            int qtdMedicamento = Convert.ToInt32(leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            int numeroFuncionario = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_NUMERO"]);
            string nomeFuncionario = Convert.ToString(leitorRequisicao["FUNCIONARIO_NOME"]);

            int numeroPaciente = Convert.ToInt32(leitorRequisicao["PACIENTE_NUMERO"]);
            string nomePaciente = Convert.ToString(leitorRequisicao["PACIENTE_NOME"]);
            string cartaoSUS = Convert.ToString(leitorRequisicao["CARTAOSUS"]);

            int numeroMedicamento = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_NUMERO"]);
            string nomeMedicamento = Convert.ToString(leitorRequisicao["MEDICAMENTO_NOME"]);
            string lote = Convert.ToString(leitorRequisicao["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorRequisicao["VALIDADE"]);

            var funcionario = new Funcionario
            {
                Numero = numeroFuncionario,
                Nome = nomeFuncionario
            };

            var paciente = new Paciente
            {
                Numero = numeroPaciente,
                Nome = nomePaciente,
                CartaoSUS = cartaoSUS
            };

            var requisicao = new Requisicao
            {
                Numero = numero,
                QtdMedicamento = qtdMedicamento,
                Data = data
            };

            var medicamento = new Medicamento
            {
                Numero = numeroMedicamento,
                Nome = nomeMedicamento,
                Lote = lote,
                Validade = validade
            };

            requisicao.ConfigurarMedicamento(medicamento);
            requisicao.ConfigurarFuncionario(funcionario);
            requisicao.ConfigurarPaciente(paciente);

            return requisicao;
        }
    }
}
