using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class ClienteDAO : DAOBase
    {
        public ClienteDAO(string connectionString) : base(connectionString) { }

        public void Insert(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO cliente (FatorRisco, RendaMensal, PessoaId) " +
                    "VALUES (@FatorRisco, @RendaMensal, @PessoaId)";

                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FatorRisco", cliente.FatorRisco);
                    command.Parameters.AddWithValue("@RendaMensal", cliente.RendaMensal);
                    command.Parameters.AddWithValue("@PessoaId", cliente.PessoaId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Cliente GetByCpf(string cpf)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM cliente WHERE PessoaId = @PessoaId";

                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PessoaId", cpf);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                Id = reader.GetInt32("Id"),
                                FatorRisco = reader.GetString("FatorRisco"),
                                RendaMensal = reader.GetString("RendaMensal"),
                                PessoaId = reader.GetInt32("PessoaId")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Update(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                connection.Open();                                   
                
                string query = "UPDATE cliente SET FatorRisco = " +
                    "@FatorRisco, RendaMensal = @RendaMensal, PessoaId = @PessoaId WHERE Id = @Id";

                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FatorRisco", cliente.FatorRisco);
                    command.Parameters.AddWithValue("@RendaMensal", cliente.RendaMensal);
                    command.Parameters.AddWithValue("@id", cliente.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM cliente WHERE Id = @Id";
                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

