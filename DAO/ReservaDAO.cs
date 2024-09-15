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
        private readonly MovimentacaoReservaDAO movimentacaoReservaDAO;
        public ReservaDAO(string connectionString) : base(connectionString) 
        {
            movimentacaoReservaDAO = new MovimentacaoReservaDAO(connectionString);
        }

        public void Insert(Reserva reserva)
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

                    //Obtem reservaId pela ultima reserva adicionada
                    reserva.Id = GetLastAdded().Id;

                    //Inserir movimentações                    
                    foreach (var movimentacaoReserva in reserva.MovimentacoesReserva)
                    {
                        movimentacaoReserva.ReservaId = reserva.Id;
                        movimentacaoReservaDAO.Insert(movimentacaoReserva);

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
        
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                var transaction = connection.BeginTransaction();
                
                try
                {
                    movimentacaoReservaDAO.DeleteByReservaId(id);

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

        //Função DeleteByContaId, que  deleta todas as reservas e movimentações associadas a uma ContaId                
        public void DeleteByContaId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                var reservas = GetReservasByContaId(id);
                try
                {                    
                    // Deletar movimentações
                    foreach (var reserva in reservas)
                    {
                        movimentacaoReservaDAO.DeleteByReservaId(reserva.Id);
                    }

                    // Deletar reservas
                    string deleteReservaQuery = "DELETE FROM Reserva WHERE ContaId = @ContaId";
       
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = deleteReservaQuery;
                        command.Parameters.AddWithValue("ContaId", id);                        
        
                        command.ExecuteNonQuery();
                    }                                                
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }                

        public Reserva GetById(int id)
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
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),
                                
                                MovimentacoesReserva = movimentacaoReservaDAO.GetByReservaId(id)
                            };
                            return reserva;
                        }
                    }                    
                }
            }
            
            return null;
        }

        public List<Reserva> GetReservasByContaId(int contaId)
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
                        List<Reserva> reservas = new List<Reserva>();
                        
                        if (reader.Read())
                        {
                            reservas.Add(new Reserva
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Saldo = reader.GetDouble(reader.GetOrdinal("Saldo")),
                                Taxa = reader.GetDouble(reader.GetOrdinal("Taxa")),
                                ReservaCol = reader.GetString(reader.GetOrdinal("ReservaCol")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),
                                
                                MovimentacoesReserva = new List<MovimentacaoReserva>()
                            });                            
                        }

                        if (reservas.Count > 0)
                        {
                            foreach (var reserva in reservas)
                            {
                                reserva.MovimentacoesReserva = movimentacaoReservaDAO.GetByReservaId(reserva.Id);
                            }
                        }
                        return reservas;
                    }                    
                }
            }
        }

        public Reserva GetLastAdded()
        {
            Reserva reserva = null;
            
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Reserva ORDER BY Id DESC LIMIT 1";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reserva = new Reserva
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Saldo = reader.GetDouble(reader.GetOrdinal("Saldo")),
                                Taxa = reader.GetDouble(reader.GetOrdinal("Taxa")),
                                ReservaCol = reader.GetString(reader.GetOrdinal("ReservaCol")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),

                                MovimentacoesReserva = new List<MovimentacaoReserva>()
                            };                            
                        }

                        if (reserva != null)
                        {
                            reserva.MovimentacoesReserva = movimentacaoReservaDAO.GetByReservaId(reserva.Id);
                        }
                    }
                }
                return reserva;
            }
        }
            
        
        public void Update(Reserva reserva)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                var transaction = connection.BeginTransaction();
                
                try
                {
                    //Atualizar reserva
                    string query = "UPDATE Reserva SET Saldo = @Saldo, Taxa = @Taxa, ReservaCol = @ReservaCol, " +
                        "ContaId = @ContaId WHERE Id = @Id";
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

                    //Atualizar movimentações
                    foreach (var movimentacaoReserva in reserva.MovimentacoesReserva)
                    {
                        movimentacaoReserva.ReservaId = reserva.Id;
                        movimentacaoReservaDAO.Update(movimentacaoReserva);
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
    }        
}
