using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class ItensFaturaDAO : DAOBase
    {
        public ItensFaturaDAO(string connectionString) : base(connectionString) { }

        public void Insert(ItensFatura itensFatura)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO ItensFatura (Descricao, FaturaCartaoId) " +
                        "VALUES (@Descricao, @FaturaCartaoId)";
                    command.Parameters.AddWithValue("Descricao", itensFatura.Descricao);
                    command.Parameters.AddWithValue("FaturaCartaoId", itensFatura.FaturaCartaoId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public ItensFatura GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao, FaturaCartaoId FROM ItensFatura WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ItensFatura
                            {
                                Id = reader.GetInt32(0),
                                Descricao = reader.GetString(1),
                                FaturaCartaoId = reader.GetInt32(2)
                            };
                        }
                    }
                }
                return null;
            }
        }
        public List<ItensFatura> GetByFaturaCartaoId(int id)
        {
            var itensFaturas = new List<ItensFatura>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao, FaturaCartaoId FROM ItensFatura WHERE FaturaCartaoId = @FaturaCartaoId";
                    command.Parameters.AddWithValue("FaturaCartaoId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itensFaturas.Add(new ItensFatura
                            {
                                Id = reader.GetInt32(0),
                                Descricao = reader.GetString(1),
                                FaturaCartaoId = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return itensFaturas;
        }
        public ItensFatura GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao, FaturaCartaoId FROM ItensFatura ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ItensFatura
                            {
                                Id = reader.GetInt32(0),
                                Descricao = reader.GetString(1),
                                FaturaCartaoId = reader.GetInt32(2)
                            };
                        }
                    }
                }
                return null;
            }
        }
        public void Update(ItensFatura itensFatura)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE ItensFatura SET Descricao = @Descricao, FaturaCartaoId = @FaturaCartaoId WHERE Id = @Id";
                    command.Parameters.AddWithValue("Descricao", itensFatura.Descricao);
                    command.Parameters.AddWithValue("FaturaCartaoId", itensFatura.FaturaCartaoId);
                    command.Parameters.AddWithValue("Id", itensFatura.Id);

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
                    command.CommandText = "DELETE FROM ItensFatura WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteByFaturaCartaoId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM ItensFatura WHERE FaturaCartaoId = @FaturaCartaoId";
                    command.Parameters.AddWithValue("FaturaCartaoId", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
