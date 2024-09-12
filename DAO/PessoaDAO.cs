using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class PessoaDAO : DAOBase
    {
        public PessoaDAO(string connectionString) : base(connectionString) {}

        public void Insert(Pessoa pessoa)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Pessoa (Nome, Cpf) VALUES (@Nome, @Cpf)";
                    command.Parameters.AddWithValue("Nome", pessoa.Nome);
                    command.Parameters.AddWithValue("Cpf", pessoa.Cpf);                    
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Pessoa pessoa)
        {
            using (var connection = GetConnection())
            {
                connection.Open();                

                var pessoaExistente = GetByCpf(pessoa.Cpf);

                if (pessoaExistente == null)
                {
                    throw new Exception("Pessoa não encontrada");
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Pessoa SET Nome = @Nome, Cpf = @Cpf WHERE Id = @Id";
                    command.Parameters.AddWithValue("Nome", pessoa.Nome);
                    command.Parameters.AddWithValue("Cpf", pessoa.Cpf);
                    command.Parameters.AddWithValue("Id", pessoaExistente.Id);
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
                    command.CommandText = "DELETE FROM Pessoa WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Pessoa GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Nome, Cpf FROM Pessoa WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pessoa
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Cpf = reader.GetString(reader.GetOrdinal("Cpf"))
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public List<Pessoa> GetAll()
        {
            var pessoas = new List<Pessoa>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Nome, Cpf FROM Pessoa";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pessoas.Add(new Pessoa
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Cpf = reader.GetString(reader.GetOrdinal("Cpf"))
                            });
                        }
                    }
                }
            }
            return pessoas;
        }

        public Pessoa GetByCpf(string cpf)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Nome, Cpf FROM Pessoa WHERE Cpf = @Cpf";
                    command.Parameters.AddWithValue("Cpf", cpf);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pessoa
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Cpf = reader.GetString(reader.GetOrdinal("Cpf"))
                            };
                        }
                        return null;
                    }
                }
            }
        }
    }
}
