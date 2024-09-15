using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class CategoriaCartaoDAO : DAOBase
    {
        public CategoriaCartaoDAO(string connectionString) : base(connectionString) {}

        public void Insert(CategoriaCartao categoriaCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO categoriaCartao (Descricao) VALUES (@Descricao)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Descricao", categoriaCartao.Descricao);

                    command.ExecuteNonQuery();
                }
            }
        }

        public CategoriaCartao GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM categoriaCartao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CategoriaCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public CategoriaCartao GetByLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM categoriaCartao ORDER BY Id DESC LIMIT 1";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CategoriaCartao
                            {
                                Id = (int)reader["Id"],
                                Descricao = (string)reader["Descricao"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<CategoriaCartao> GetAll()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM categoria_cartao";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        var categorias = new List<CategoriaCartao>();

                        while (reader.Read())
                        {
                            categorias.Add(new CategoriaCartao
                            {
                                Id = (int)reader["Id"],
                                Descricao = (string)reader["Descricao"]
                            });
                        }

                        return categorias;
                    }
                }
            }
        }

        public void Update(CategoriaCartao categoriaCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE categoria_cartao SET Descricao = @Descricao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Descricao", categoriaCartao.Descricao);
                    command.Parameters.AddWithValue("Id", categoriaCartao.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM categoria_cartao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
