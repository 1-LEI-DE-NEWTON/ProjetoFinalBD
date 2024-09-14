using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class ClienteDAO : DAOBase
    {
        private readonly PessoaDAO pessoaDAO;
        private readonly ContaDAO contaDAO;
        private readonly TipoContaDAO tipoContaDAO;
        public ClienteDAO(string connectionString) : base(connectionString)
        {
            pessoaDAO = new PessoaDAO(connectionString);
            tipoContaDAO = new TipoContaDAO(connectionString);
            contaDAO = new ContaDAO(connectionString);
            
        }

        public void Insert(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    //Adiciona pessoa
                    pessoaDAO.Insert(cliente.Pessoa);

                    cliente.Pessoa.Id = pessoaDAO.GetByCpf(cliente.Pessoa.Cpf).Id;

                    //Adiciona cliente                    
                    string query = "INSERT INTO cliente (FatorRisco, RendaMensal, PessoaId) " +
                        "VALUES (@FatorRisco, @RendaMensal, @PessoaId)";

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.AddWithValue("FatorRisco", cliente.FatorRisco);
                        command.Parameters.AddWithValue("RendaMensal", cliente.RendaMensal);
                        command.Parameters.AddWithValue("PessoaId", cliente.Pessoa.Id);

                        command.ExecuteNonQuery();
                    }

                    cliente.Id = GetByPessoaId(cliente.Pessoa.Id).Id;

                    //Adiciona contas
                    foreach (var conta in cliente.Contas)
                    {
                        conta.ClienteId = cliente.Id;
                        contaDAO.Insert(conta);                                          
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
        public Cliente GetById(int id)  //Necessario atualizar
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM cliente WHERE id = @id";
                    command.Parameters.AddWithValue("id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                Id = reader.GetInt32("id"),
                                FatorRisco = reader.GetString("FatorRisco"),
                                RendaMensal = reader.GetString("RendaMensal"),

                                Pessoa = pessoaDAO.GetById(reader.GetInt32("PessoaId")),

                                Contas = contaDAO.GetContasByClienteId(reader.GetInt32("id"))
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public Cliente GetByPessoaId(int id)
        {

            Cliente cliente = null;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM cliente WHERE PessoaId = @PessoaId";
                    command.Parameters.AddWithValue("PessoaId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                Id = reader.GetInt32("id"),
                                FatorRisco = reader.GetString("FatorRisco"),
                                RendaMensal = reader.GetString("RendaMensal"),

                                Pessoa = pessoaDAO.GetById(reader.GetInt32("PessoaId")),

                                //Retorna as contas
                                Contas = new List<Conta>()
                            };
                        }
                    }

                    if (cliente != null)
                    {
                        cliente.Contas = contaDAO.GetContasByClienteId(cliente.Id);

                        if (cliente.Contas.Count > 0)
                        {
                            foreach (var conta in cliente.Contas)
                            {
                                conta.TipoConta = tipoContaDAO.GetById(conta.TipoContaId);
                            }
                        }
                    }
                }
            }
            return cliente;
        }   
        public void Update(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    pessoaDAO.Update(cliente.Pessoa);
                    
                    string query = "UPDATE cliente SET FatorRisco = @FatorRisco, RendaMensal = @RendaMensal WHERE id = @id";

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.AddWithValue("FatorRisco", cliente.FatorRisco);
                        command.Parameters.AddWithValue("RendaMensal", cliente.RendaMensal);
                        command.Parameters.AddWithValue("Id", cliente.Id);

                        command.ExecuteNonQuery();
                    }

                    //Atualiza contas
                    cliente.Contas = contaDAO.GetContasByClienteId(cliente.Id);
                    
                    foreach (var conta in cliente.Contas)
                    {

                        contaDAO.Update(conta);                                                
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
                    Cliente cliente = GetById(id);

                    //Deletar contas
                    contaDAO.DeleteByClienteId(cliente.Id);

                    string query = "DELETE FROM cliente WHERE id = @id";

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.AddWithValue("id", id);

                        command.ExecuteNonQuery();
                    }

                    pessoaDAO.Delete(cliente.Pessoa.Id);
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

