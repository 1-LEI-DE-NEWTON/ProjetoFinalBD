using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public class CorretorDAO : DAOBase
    {
        public CorretorDAO(string connectionString) : base(connectionString) { }

        public void Insert(Corretor corretor)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Corretor (Nome) VALUES (@Nome)";
                    command.Parameters.AddWithValue("Nome", corretor.Nome);

                    command.ExecuteNonQuery();
                }
            }
        }

        public Corretor GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Nome FROM Corretor WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Corretor
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1)
                            };
                        }
                    }
                }
                return null;
            }
        }

        public Corretor GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Nome FROM Corretor ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Corretor
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1)
                            };
                        }
                    }
                }
                return null;
            }
        }

        public void Update(Corretor corretor)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var corretorExistente = GetById(corretor.Id);

                if (corretorExistente == null)
                {
                    throw new Exception("Corretor não encontrado");
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Corretor SET Nome = @Nome WHERE Id = @Id";
                    command.Parameters.AddWithValue("Nome", corretor.Nome);
                    command.Parameters.AddWithValue("Id", corretorExistente.Id);
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
                    command.CommandText = "DELETE FROM Corretor WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
