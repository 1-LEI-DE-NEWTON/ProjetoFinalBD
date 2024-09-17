using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public class BandeiraCartaoDAO : DAOBase
    {
        public BandeiraCartaoDAO(string connectionString) : base(connectionString) { }

        public void Insert(BandeiraCartao bandeiraCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO BandeiraCartao (Descricao) VALUES (@Descricao)";
                    command.Parameters.AddWithValue("Descricao", bandeiraCartao.Descricao);

                    command.ExecuteNonQuery();
                }
            }
        }

        public BandeiraCartao GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao FROM BandeiraCartao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BandeiraCartao
                            {
                                Id = reader.GetInt32(0),
                                Descricao = reader.GetString(1)
                            };
                        }
                    }
                }
                return null;
            }
        }

        public BandeiraCartao GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM BandeiraCartao ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BandeiraCartao
                            {
                                Id = reader.GetInt32(0),
                                Descricao = reader.GetString(1)
                            };
                        }
                    }
                }
                return null;
            }
        }

        public void Update(BandeiraCartao bandeiraCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE BandeiraCartao SET Descricao = @Descricao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Descricao", bandeiraCartao.Descricao);
                    command.Parameters.AddWithValue("Id", bandeiraCartao.Id);

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
                    command.CommandText = "DELETE FROM BandeiraCartao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
