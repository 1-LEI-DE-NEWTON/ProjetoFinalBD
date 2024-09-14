using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class TipoContaDAO : DAOBase
    {
        public TipoContaDAO(string connectionString) : base(connectionString) { }

        public void Insert(TipoConta tipoConta)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO TipoConta (Descricao) VALUES (@Descricao)";
                    command.Parameters.AddWithValue("Descricao", tipoConta.Descricao);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(TipoConta tipoConta)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var tipoContaExistente = GetById(tipoConta.Id);

                if (tipoContaExistente == null)
                {
                    throw new Exception("Tipo de conta não encontrado");
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE TipoConta SET Descricao = @Descricao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Descricao", tipoConta.Descricao);
                    command.Parameters.AddWithValue("Id", tipoContaExistente.Id);
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
                    command.CommandText = "DELETE FROM TipoConta WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public TipoConta GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao FROM TipoConta WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TipoConta
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

        public TipoConta GetByLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao FROM TipoConta ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TipoConta
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

        public List<TipoConta> GetAll()
        {
            var tiposConta = new List<TipoConta>();

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao FROM TipoConta";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tiposConta.Add(new TipoConta
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao"))
                            });
                        }
                    }
                }
            }

            return tiposConta;
        }
    }
}
