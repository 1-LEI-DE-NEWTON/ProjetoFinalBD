using ProjetoFinalBD.Models;
using System;
using MySql.Data.MySqlClient;

namespace ProjetoFinalBD.DAO;
public class ContaDAO : DAOBase
{
    private readonly ReservaDAO reservaDAO;
    private readonly TipoContaDAO tipoContaDAO;
    private readonly MovimentacaoContaDAO movimentacaoContaDAO;

    public ContaDAO(string connectionString) : base(connectionString) 
    {
        reservaDAO = new ReservaDAO(connectionString);
        tipoContaDAO = new TipoContaDAO(connectionString);
        movimentacaoContaDAO = new MovimentacaoContaDAO(connectionString);
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
                //Adiciona tipoConta
                tipoContaDAO.Insert(conta.TipoConta);
                
                //Obtem o tipoContaId pela ultima TipoConta adicionada
                conta.TipoConta.Id = tipoContaDAO.GetByLastAdded().Id;

                //Adiciona conta
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Saldo", conta.Saldo);
                    command.Parameters.AddWithValue("@LimiteNegativo", conta.LimiteNegativo);
                    command.Parameters.AddWithValue("@ClienteId", conta.ClienteId);
                    command.Parameters.AddWithValue("@TipoContaId", conta.TipoConta.Id);

                    command.ExecuteNonQuery();
                }
                
                //Obtem ContaId pela ultima conta adicionada
                conta.Id = GetLastAdded().Id;

                //Adiciona movimentacaoConta
                foreach (var movimentacaoConta in conta.MovimentacoesConta)
                {
                    movimentacaoConta.ContaId = conta.Id;
                    movimentacaoContaDAO.Insert(movimentacaoConta);
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
                            ClienteId = reader.GetInt32("ClienteId"),
                            TipoContaId = reader.GetInt32("TipoContaId"),
                            
                            TipoConta = tipoContaDAO.GetById(reader.GetInt32("TipoContaId")),
                            
                            MovimentacoesConta = movimentacaoContaDAO.GetMovimentacoesContaByContaId(id),

                            Reservas = reservaDAO.GetReservasByContaId(id)
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
                            TipoContaId = reader.GetInt32("TipoContaId"),
                            
                            TipoConta = tipoContaDAO.GetById(reader.GetInt32("TipoContaId")),
                            
                            MovimentacoesConta = new List<MovimentacaoConta>(),

                            Reservas = new List<Reserva>()
                        });                                                
                    }

                    if (contas.Count > 0)
                    {
                        foreach (var conta in contas)
                        {
                            conta.MovimentacoesConta = movimentacaoContaDAO.GetMovimentacoesContaByContaId(conta.Id);
                            conta.Reservas = reservaDAO.GetReservasByContaId(conta.Id);
                        }
                    }

                    return contas;
                }
            }
        }
    }

    public Conta GetLastAdded()
    {
        using (var connection = GetConnection())
        {
            Conta conta = null;
            connection.Open();

            string query = "SELECT * FROM Conta ORDER BY Id DESC LIMIT 1";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        conta = new Conta
                        {
                            Id = reader.GetInt32("Id"),
                            Saldo = reader.GetDouble("Saldo"),
                            LimiteNegativo = reader.GetDouble("LimiteNegativo"),
                            ClienteId = reader.GetInt32("ClienteId"),
                            TipoContaId = reader.GetInt32("TipoContaId"),
                            
                            TipoConta = tipoContaDAO.GetById(reader.GetInt32("TipoContaId")),
                            
                            MovimentacoesConta = new List<MovimentacaoConta>(),

                            Reservas = new List<Reserva>()
                        };
                    }

                    if (conta != null)
                    {
                        conta.MovimentacoesConta = movimentacaoContaDAO.GetMovimentacoesContaByContaId(conta.Id);
                        conta.Reservas = reservaDAO.GetReservasByContaId(conta.Id);
                    }   
                    else
                    {
                        return null;
                    }
                }
            }
            return conta;
        }
    }

    public void Update(Conta conta)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            
            string query = "UPDATE Conta SET Saldo = @Saldo, LimiteNegativo = @LimiteNegativo," +
                " ClienteId = @ClienteId, TipoContaId = @TipoContaId WHERE Id = @Id";

            var transaction = connection.BeginTransaction();
                       
            try
            {
                //Atualiza o tipoConta
                conta.TipoConta = tipoContaDAO.GetById(conta.TipoContaId);
                tipoContaDAO.Update(conta.TipoConta); 

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", conta.Id);
                    command.Parameters.AddWithValue("@Saldo", conta.Saldo);
                    command.Parameters.AddWithValue("@LimiteNegativo", conta.LimiteNegativo);
                    command.Parameters.AddWithValue("@ClienteId", conta.ClienteId);
                    command.Parameters.AddWithValue("@TipoContaId", conta.TipoContaId);

                    command.ExecuteNonQuery();
                }

                //Atualiza movimentacoesConta
                foreach (var movimentacaoConta in conta.MovimentacoesConta)
                {
                    movimentacaoConta.ContaId = conta.Id;
                    movimentacaoContaDAO.Update(movimentacaoConta);
                }

                //Atualiza reservas
                foreach (var reserva in conta.Reservas)
                {
                    reserva.ContaId = conta.Id;
                    reservaDAO.Update(reserva);
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

            //Deleta as reservas associadas a essa conta
            reservaDAO.DeleteByContaId(id);           

            string query = "DELETE FROM Conta WHERE Id = @Id";
            
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                
                command.ExecuteNonQuery();
            }

            //Deletar movimentacoesConta
            movimentacaoContaDAO.DeleteByContaId(id);

            //Deletar tipoConta
            tipoContaDAO.Delete(GetById(id).TipoContaId);
        }
    }
    
    public void DeleteByClienteId(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            
            var contas = GetContasByClienteId(id);

            //Deletar reservas
            foreach (var conta in contas)
            {
                reservaDAO.DeleteByContaId(conta.Id);
            }            

            //Deletar contas
            string query = "DELETE FROM Conta WHERE ClienteId = @ClienteId";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClienteId", id);

                command.ExecuteNonQuery();
            }

            //Deletar tipoConta das contas associadas            
            foreach (var conta in contas)
            {
                tipoContaDAO.Delete(conta.TipoContaId);
            }

            //Deletar movimentacoesConta das contas associadas
            foreach (var conta in contas)
            {
                movimentacaoContaDAO.DeleteByContaId(conta.Id);
            }
        }
    }
}