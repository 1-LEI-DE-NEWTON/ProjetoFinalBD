using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class MovimentacaoReservaDAO : DAOBase
    {
        public MovimentacaoReservaDAO(string connectionString) : base(connectionString) {}

        public void Insert(MovimentacaoReserva movimentacaoReserva)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO MovimentacaoReserva (DataMovimentacao, Valor, TipoMovimentacao, ReservaId) VALUES (@DataMovimentacao, @Valor, @TipoMovimentacao, @ReservaId)";
                    command.Parameters.AddWithValue("DataMovimentacao", movimentacaoReserva.DataMovimentacao);
                    command.Parameters.AddWithValue("Valor", movimentacaoReserva.Valor);
                    command.Parameters.AddWithValue("TipoMovimentacao", movimentacaoReserva.TipoMovimentacao);
                    command.Parameters.AddWithValue("ReservaId", movimentacaoReserva.ReservaId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<MovimentacaoReserva> GetByReservaId(int reservaId)
        {
            List<MovimentacaoReserva> movimentacoesReserva = new List<MovimentacaoReserva>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM MovimentacaoReserva WHERE ReservaId = @ReservaId";
                    command.Parameters.AddWithValue("ReservaId", reservaId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MovimentacaoReserva movimentacaoReserva = new MovimentacaoReserva
                            {
                                Id = reader.GetInt32("Id"),
                                DataMovimentacao = reader.GetDateTime("DataMovimentacao"),
                                Valor = reader.GetDouble("Valor"),
                                TipoMovimentacao = reader.GetString("TipoMovimentacao"),
                                ReservaId = reader.GetInt32("ReservaId")
                            };

                            movimentacoesReserva.Add(movimentacaoReserva);
                        }
                    }
                }
            }

            return movimentacoesReserva;
        }

        public void DeleteByReservaId(int reservaId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM MovimentacaoReserva WHERE ReservaId = @ReservaId";
                    command.Parameters.AddWithValue("ReservaId", reservaId);

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
                    command.CommandText = "DELETE FROM MovimentacaoReserva WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(MovimentacaoReserva movimentacaoReserva)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE MovimentacaoReserva SET DataMovimentacao = @DataMovimentacao, Valor = @Valor, TipoMovimentacao = @TipoMovimentacao, ReservaId = @ReservaId WHERE Id = @Id";
                    command.Parameters.AddWithValue("DataMovimentacao", movimentacaoReserva.DataMovimentacao);
                    command.Parameters.AddWithValue("Valor", movimentacaoReserva.Valor);
                    command.Parameters.AddWithValue("TipoMovimentacao", movimentacaoReserva.TipoMovimentacao);
                    command.Parameters.AddWithValue("ReservaId", movimentacaoReserva.ReservaId);
                    command.Parameters.AddWithValue("Id", movimentacaoReserva.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }    
}
