using ProjetoFinalBD.Models;
using System;
using MySql.Data.MySqlClient;

namespace ProjetoFinalBD.DAO;
public class ContaDAO : DAOBase
{
    private readonly ReservaDAO reservaDAO;
    
    public ContaDAO(string connectionString) : base(connectionString) 
    {
        reservaDAO = new ReservaDAO(connectionString);
    }

    public void Insert(Conta conta)
    {
        using (var connection = GetConnection())        
        {
            connection.Open();

            var transaction = connection.BeginTransaction();

            string query = "INSERT INTO Conta (Saldo, LimiteNegativo, ClienteId, TipoContaId) " +
                "VALUES (@Saldo, @LimiteNegativo, @ClienteId, @TipoContaId)";
            try
            {
                //Adiciona conta
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Saldo", conta.Saldo);
                    command.Parameters.AddWithValue("@LimiteNegativo", conta.LimiteNegativo);
                    command.Parameters.AddWithValue("@ClienteId", conta.ClienteId);
                    command.Parameters.AddWithValue("@TipoContaId", conta.TipoConta.Id);

                    command.ExecuteNonQuery();
                }

                //Adiciona TipoConta
                query = "INSERT INTO TipoConta (Descricao) VALUES (@Descricao)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descricao", conta.TipoConta.Descricao);

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

    public Conta GetById(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            
            string query = "SELECT * FROM Conta WHERE Id = @Id";
            
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Conta
                        {
                            Id = reader.GetInt32("Id"),
                            Saldo = reader.GetDouble("Saldo"),
                            LimiteNegativo = reader.GetDouble("LimiteNegativo"),
                            ClienteId = reader.IsDBNull(reader.GetOrdinal("ClienteId")) ? (int?)null : reader.GetInt32("ClienteId"),
                            TipoContaId = reader.IsDBNull(reader.GetOrdinal("TipoContaId")) ? (int?)null : reader.GetInt32("TipoContaId")
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }    
    
    public List<Conta> GetContasByClienteId(int clienteId)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = "SELECT * FROM Conta WHERE ClienteId = @ClienteId";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClienteId", clienteId);

                using (var reader = command.ExecuteReader())
                {
                    List<Conta> contas = new List<Conta>();

                    while (reader.Read())
                    {
                        contas.Add(new Conta
                        {
                            Id = reader.GetInt32("Id"),
                            Saldo = reader.GetDouble("Saldo"),
                            LimiteNegativo = reader.GetDouble("LimiteNegativo"),
                            ClienteId = reader.GetInt32("ClienteId"),
                            TipoContaId = reader.GetInt32("TipoContaId")
                        });
                    }

                    return contas;
                }
            }
        }
    }   
    
    public void Update(Conta conta)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            
            string query = "UPDATE Conta SET Saldo = @Saldo, LimiteNegativo = @LimiteNegativo," +
                " ClienteId = @ClienteId, TipoContaId = @TipoContaId WHERE Id = @Id";
            
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", conta.Id);
                command.Parameters.AddWithValue("@Saldo", conta.Saldo);
                command.Parameters.AddWithValue("@LimiteNegativo", conta.LimiteNegativo);
                command.Parameters.AddWithValue("@ClienteId", conta.ClienteId);
                command.Parameters.AddWithValue("@TipoContaId", conta.TipoContaId);
                
                command.ExecuteNonQuery();
            }
        }
    }
    
    public void Delete(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            
            string query = "DELETE FROM Conta WHERE Id = @Id";
            
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                
                command.ExecuteNonQuery();
            }
        }
    }
    
    public void DeleteByClienteId(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            //Deletar reservas
            var contas = GetContasByClienteId(id);

            reservaDAO.DeleteByContaId(contas);

            //Deletar contas

            string query = "DELETE FROM Conta WHERE ClienteId = @ClienteId";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClienteId", id);

                command.ExecuteNonQuery();
            }
        }
    }
}