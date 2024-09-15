using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public class ReservaDAO : DAOBase
    {
        public ReservaDAO(string connectionString) : base(connectionString) { }

        public void Insert(Reserva reserva) //atualizar
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                var transaction = connection.BeginTransaction();
                
                try
                {
                    //Inserir reserva
                    string query = "INSERT INTO Reserva (Id, Saldo, Taxa, ReservaCol, ContaId) " +
                            "VALUES (@Id, @Saldo, @Taxa, @ReservaCol, @ContaId)";
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.AddWithValue("Id", reserva.Id);
                        command.Parameters.AddWithValue("Saldo", reserva.Saldo);
                        command.Parameters.AddWithValue("Taxa", reserva.Taxa);
                        command.Parameters.AddWithValue("ReservaCol", reserva.ReservaCol);
                        command.Parameters.AddWithValue("ContaId", reserva.ContaId);

                        command.ExecuteNonQuery();
                    }

                    //Inserir movimentações

                    var reservaId = GetbyContaId(reserva.ContaId);

                    foreach (var movimentacaoReserva in reserva.MovimentacoesReserva)
                    {
                        //Criar string para query                        
                        string insertMovimentacaoQuery = "INSERT INTO movimentacaoReserva " +
                            "(Id, DataMovimentacao, TipoMovimentacao, Valor, ReservaId) VALUES " +
                            "(@Id, @DataMovimentacao, @TipoMovimentacao, @Valor, @ReservaId)";

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = insertMovimentacaoQuery;
                            command.Parameters.AddWithValue("Id", movimentacaoReserva.Id);
                            command.Parameters.AddWithValue("DataMovimentacao", movimentacaoReserva.DataMovimentacao);
                            command.Parameters.AddWithValue("TipoMovimentacao", movimentacaoReserva.TipoMovimentacao);
                            command.Parameters.AddWithValue("Valor", movimentacaoReserva.Valor);
                            command.Parameters.AddWithValue("ReservaId", reserva.Id);

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public void Delete(int id) //atualizar
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                var transaction = connection.BeginTransaction();
                
                try
                {
                    //Deletar movimentações
                    string deleteMovimentacaoQuery = "DELETE FROM movimentacaoReserva WHERE ReservaId = @ReservaId";
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = deleteMovimentacaoQuery;
                        command.Parameters.AddWithValue("ReservaId", id);

                        command.ExecuteNonQuery();
                    }

                    //Deletar reserva
                    string deleteReservaQuery = "DELETE FROM Reserva WHERE Id = @Id";
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = deleteReservaQuery;
                        command.Parameters.AddWithValue("Id", id);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        //Função DeleteByContaId, que recebe uma lista de contas e deleta todas as reservas e movimentações associadas a elas
                
        public void DeleteByContaId(List<Conta> contas) //atualizar
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
        
                try
                {
                    foreach (var conta in contas)
                    {
                        // Deletar movimentações
                        string deleteMovimentacaoQuery = "DELETE FROM movimentacaoReserva WHERE ReservaId IN " +
                            "(SELECT Id FROM Reserva WHERE ContaId = @ContaId)";
        
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = deleteMovimentacaoQuery;
                            command.Parameters.AddWithValue("ContaId", conta.Id);
                            command.Transaction = transaction;
        
                            command.ExecuteNonQuery();
                        }
        
                        // Deletar reservas
                        string deleteReservaQuery = "DELETE FROM Reserva WHERE ContaId = @ContaId";
        
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = deleteReservaQuery;
                            command.Parameters.AddWithValue("ContaId", conta.Id);
                            command.Transaction = transaction;
        
                            command.ExecuteNonQuery();
                        }
                    }
        
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }                

        public Reserva GetById(int id) //necesario atualizar
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                string query = "SELECT * FROM Reserva WHERE Id = @Id";
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var reserva = new Reserva
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Saldo = reader.GetDouble(reader.GetOrdinal("Saldo")),
                                Taxa = reader.GetDouble(reader.GetOrdinal("Taxa")),
                                ReservaCol = reader.GetString(reader.GetOrdinal("ReservaCol")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId"))
                            };

                            return reserva;
                        }
                    }
                }
            }
            
            return null;
        }

        public Reserva GetbyContaId(int contaId) //atualizar
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                string query = "SELECT * FROM Reserva WHERE ContaId = @ContaId";
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("ContaId", contaId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var reserva = new Reserva
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Saldo = reader.GetDouble(reader.GetOrdinal("Saldo")),
                                Taxa = reader.GetDouble(reader.GetOrdinal("Taxa")),
                                ReservaCol = reader.GetString(reader.GetOrdinal("ReservaCol")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId"))
                            };

                            return reserva;
                        }
                    }
                }
            }
            
            return null;
        }
    }
}
