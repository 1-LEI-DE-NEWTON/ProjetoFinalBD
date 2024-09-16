using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class MovimentacaoCartaoDAO : DAOBase
    {
        public MovimentacaoCartaoDAO(string connectionString) : base(connectionString) { }

        public void Insert(MovimentacaoCartao movimentacaoCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO movimentacaoCartao (DataMovimentacao, Valor, CartaoId) " +
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

                string query = "SELECT * FROM movimentacaoCartao WHERE Id = @Id";

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

                string query = "SELECT * FROM movimentacaoCartao";

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

        public List<MovimentacaoCartao> GetByCartaoTransacaoId(int cartaoTransacaoId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM movimentacaoCartao WHERE CartaoTransacaoId = @CartaoTransacaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoTransacaoId", cartaoTransacaoId);

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

                string query = "UPDATE movimentacaoCartao SET DataMovimentacao = @DataMovimentacao, " +
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

                string query = "DELETE FROM movimentacaoCartao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteByCartaoTransacaoId(int cartaoTransacaoId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM movimentacaoCartao WHERE cartaoTransacaoId = @cartaoTransacaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("cartaoTransacaoId", cartaoTransacaoId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
