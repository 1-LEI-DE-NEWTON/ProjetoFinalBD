using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;


namespace ProjetoFinalBD.DAO
{
    public class MovimentacaoContaDAO : DAOBase
    {
        public MovimentacaoContaDAO(string connectionString) : base(connectionString) { }
        
        public void Insert(MovimentacaoConta movimentacaoConta)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO MovimentacaoConta (ContaId, valor, dataMovimentacao, TipoMovimentacao) " +
                    "VALUES (@ContaId, @valor, @dataMovimentacao, @TipoMovimentacao)";
                command.Parameters.AddWithValue("ContaId", movimentacaoConta.ContaId);
                command.Parameters.AddWithValue("valor", movimentacaoConta.Valor);
                command.Parameters.AddWithValue("dataMovimentacao", movimentacaoConta.DataMovimentacao);
                command.Parameters.AddWithValue("TipoMovimentacao", movimentacaoConta.TipoMovimentacao);

                command.ExecuteNonQuery();
            }
        }

        public List<MovimentacaoConta> GetMovimentacaoContaByContaId(int contaId)
        {
            List<MovimentacaoConta> movimentacoes = new List<MovimentacaoConta>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM MovimentacaoConta WHERE ContaId = @ContaId";
                command.Parameters.AddWithValue("ContaId", contaId);
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    MovimentacaoConta movimentacao = new MovimentacaoConta();
                    movimentacao.Id = result.GetInt32("Id");
                    movimentacao.ContaId = result.GetInt32("ContaId");
                    movimentacao.Valor = result.GetString("valor");
                    movimentacao.DataMovimentacao = result.GetDateTime("dataMovimentacao");
                    movimentacao.TipoMovimentacao = result.GetString("TipoMovimentacao");
                    movimentacoes.Add(movimentacao);
                }
            }
            return movimentacoes;
        }

        public void DeleteByContaId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM MovimentacaoConta WHERE ContaId = @ContaId";
                    command.Parameters.AddWithValue("ContaId", id);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM MovimentacaoConta WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(MovimentacaoConta movimentacaoConta)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE MovimentacaoConta SET ContaId = @ContaId, valor = @valor, " +
                    "dataMovimentacao = @dataMovimentacao, TipoMovimentacao = @TipoMovimentacao WHERE Id = @Id";
                command.Parameters.AddWithValue("ContaId", movimentacaoConta.ContaId);
                command.Parameters.AddWithValue("valor", movimentacaoConta.Valor);
                command.Parameters.AddWithValue("dataMovimentacao", movimentacaoConta.DataMovimentacao);
                command.Parameters.AddWithValue("TipoMovimentacao", movimentacaoConta.TipoMovimentacao);
                command.Parameters.AddWithValue("Id", movimentacaoConta.Id);

                command.ExecuteNonQuery();
            }
        }

        public MovimentacaoConta GetById(int id)
        {
            MovimentacaoConta movimentacao = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM MovimentacaoConta WHERE Id = @Id";
                command.Parameters.AddWithValue("Id", id);
                var result = command.ExecuteReader();
                if (result.Read())
                {
                    movimentacao = new MovimentacaoConta();
                    movimentacao.Id = result.GetInt32("Id");
                    movimentacao.ContaId = result.GetInt32("ContaId");
                    movimentacao.Valor = result.GetString("valor");
                    movimentacao.DataMovimentacao = result.GetDateTime("dataMovimentacao");
                    movimentacao.TipoMovimentacao = result.GetString("TipoMovimentacao");
                }
            }
            return movimentacao;
        }
    }
}
