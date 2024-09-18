using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class PagamentoDAO : DAOBase
    {        
        public PagamentoDAO(string connectionString) : base(connectionString) {}

        public void Insert(Pagamento pagamento)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Pagamento (ValorTotal, DataPagamento, FaturaCartaoId, " +
                        "ValorParcial, BoletoCustomizadoId) VALUES (@ValorTotal, @DataPagamento, " +
                        "@FaturaCartaoId, @ValorParcial, @BoletoCustomizadoId)";

                    command.Parameters.AddWithValue("ValorTotal", pagamento.ValorTotal);
                    command.Parameters.AddWithValue("DataPagamento", pagamento.DataPagamento);
                    command.Parameters.AddWithValue("FaturaCartaoId", pagamento.FaturaCartaoId);
                    command.Parameters.AddWithValue("ValorParcial", pagamento.ValorParcial);
                    command.Parameters.AddWithValue("BoletoCustomizadoId", pagamento.BoletoCustomizadoId);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public Pagamento GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, ValorTotal, DataPagamento, FaturaCartaoId, " +
                        "ValorParcial, BoletoCustomizadoId FROM Pagamento WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pagamento
                            {
                                Id = reader.GetInt32(0),
                                ValorTotal = reader.GetString(1),
                                DataPagamento = reader.GetDateTime(2),
                                FaturaCartaoId = reader.GetInt32(3),
                                ValorParcial = reader.GetString(4),
                                BoletoCustomizadoId = reader.GetInt32(5)                                
                            };
                        }
                    }
                }
                return null;
            }
        }
        public Pagamento GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, ValorTotal, DataPagamento, FaturaCartaoId, " +
                        "ValorParcial, BoletoCustomizadoId FROM Pagamento ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pagamento
                            {
                                Id = reader.GetInt32(0),
                                ValorTotal = reader.GetString(1),
                                DataPagamento = reader.GetDateTime(2),
                                FaturaCartaoId = reader.GetInt32(3),
                                ValorParcial = reader.GetString(4),
                                BoletoCustomizadoId = reader.GetInt32(5)
                            };
                        }
                    }
                }
                return null;
            }
        }
        public List<Pagamento> GetByBoletoCustomizadoId(int id)
        {
            List<Pagamento> pagamentos = new List<Pagamento>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, ValorTotal, DataPagamento, FaturaCartaoId, " +
                        "ValorParcial, BoletoCustomizadoId FROM Pagamento WHERE BoletoCustomizadoId = @BoletoCustomizadoId";
                    command.Parameters.AddWithValue("BoletoCustomizadoId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pagamentos.Add(new Pagamento
                            {
                                Id = reader.GetInt32(0),
                                ValorTotal = reader.GetString(1),
                                DataPagamento = reader.GetDateTime(2),
                                FaturaCartaoId = reader.GetInt32(3),
                                ValorParcial = reader.GetString(4),
                                BoletoCustomizadoId = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return pagamentos;
        }
        public void Update(Pagamento pagamento)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Pagamento SET ValorTotal = @ValorTotal, DataPagamento = " +
                        "@DataPagamento, FaturaCartaoId = @FaturaCartaoId, ValorParcial = @ValorParcial, " +
                        "BoletoCustomizadoId = @BoletoCustomizadoId WHERE Id = @Id";

                    command.Parameters.AddWithValue("ValorTotal", pagamento.ValorTotal);
                    command.Parameters.AddWithValue("DataPagamento", pagamento.DataPagamento);
                    command.Parameters.AddWithValue("FaturaCartaoId", pagamento.FaturaCartaoId);
                    command.Parameters.AddWithValue("ValorParcial", pagamento.ValorParcial);
                    command.Parameters.AddWithValue("BoletoCustomizadoId", pagamento.BoletoCustomizadoId);
                    command.Parameters.AddWithValue("Id", pagamento.Id);

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
                    command.CommandText = "DELETE FROM Pagamento WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void DeleteByBoletoCustomizadoId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Pagamento WHERE BoletoCustomizadoId = @BoletoCustomizadoId";
                    command.Parameters.AddWithValue("BoletoCustomizadoId", id);

                    command.ExecuteNonQuery();
                }
            }            
        }
        
        public void DeleteByFaturaCartaoId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Pagamento WHERE FaturaCartaoId = @FaturaCartaoId";
                    command.Parameters.AddWithValue("FaturaCartaoId", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
