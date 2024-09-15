using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class movimentacaoCartaoDAO : DAOBase
    {
        public movimentacaoCartaoDAO(string connectionString) : base(connectionString) { }

        public void Insert(MovimentacaoCartao movimentacaoCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO movimentacao_cartao (DataMovimentacao, Valor, CartaoId) " +
                    "VALUES (@DataMovimentacao, @Valor, @CartaoId)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("DataMovimentacao", movimentacaoCartao.DataMovimentacao);
                    command.Parameters.AddWithValue("Valor", movimentacaoCartao.Valor);
                    command.Parameters.AddWithValue("CartaoId", movimentacaoCartao.CartaoTransacaoId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public MovimentacaoCartao GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM movimentacao_cartao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MovimentacaoCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataMovimentacao = reader.GetDateTime(reader.GetOrdinal("DataMovimentacao")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoId")),
                                TipoMovimentacao = reader.GetString(reader.GetOrdinal("TipoMovimentacao"))                                
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<MovimentacaoCartao> GetAll()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM movimentacao_cartao";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        List<MovimentacaoCartao> movimentacoes = new List<MovimentacaoCartao>();

                        while (reader.Read())
                        {
                            movimentacoes.Add(new MovimentacaoCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataMovimentacao = reader.GetDateTime(reader.GetOrdinal("DataMovimentacao")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoId")),
                                TipoMovimentacao = reader.GetString(reader.GetOrdinal("TipoMovimentacao"))
                            });
                        }

                        return movimentacoes;
                    }
                }
            }
        }

        public List<MovimentacaoCartao> GetByCartaoId(int cartaoId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM movimentacao_cartao WHERE CartaoId = @CartaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoId", cartaoId);

                    using (var reader = command.ExecuteReader())
                    {
                        List<MovimentacaoCartao> movimentacoes = new List<MovimentacaoCartao>();

                        while (reader.Read())
                        {
                            movimentacoes.Add(new MovimentacaoCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataMovimentacao = reader.GetDateTime(reader.GetOrdinal("DataMovimentacao")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoId")),
                                TipoMovimentacao = reader.GetString(reader.GetOrdinal("TipoMovimentacao"))
                            });
                        }

                        return movimentacoes;
                    }
                }
            }
        }

        public void Update(MovimentacaoCartao movimentacaoCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE movimentacao_cartao SET DataMovimentacao = @DataMovimentacao, " +
                    "Valor = @Valor, CartaoId = @CartaoId " +
                    "WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("DataMovimentacao", movimentacaoCartao.DataMovimentacao);
                    command.Parameters.AddWithValue("Valor", movimentacaoCartao.Valor);
                    command.Parameters.AddWithValue("CartaoId", movimentacaoCartao.CartaoTransacaoId);
                    command.Parameters.AddWithValue("Id", movimentacaoCartao.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM movimentacao_cartao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteByCartaoId(int cartaoId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM movimentacao_cartao WHERE CartaoId = @CartaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoId", cartaoId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
