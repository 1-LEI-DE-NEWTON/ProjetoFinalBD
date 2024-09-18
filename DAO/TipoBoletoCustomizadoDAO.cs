using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public class TipoBoletoCustomizadoDAO : DAOBase
    {
        public TipoBoletoCustomizadoDAO(string connectionString) : base(connectionString) { }
        
        public void Insert(TipoBoletoCustomizado tipoBoletoCustomizado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO TipoBoletoCustomizado (Descricao) VALUES (@Descricao)";
                    command.Parameters.AddWithValue("Descricao", tipoBoletoCustomizado.Descricao);

                    command.ExecuteNonQuery();
                }
            }
        }

        public TipoBoletoCustomizado GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao FROM TipoBoletoCustomizado WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TipoBoletoCustomizado
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

        public void Update(TipoBoletoCustomizado tipoBoletoCustomizado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE TipoBoletoCustomizado SET Descricao = @Descricao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Descricao", tipoBoletoCustomizado.Descricao);
                    command.Parameters.AddWithValue("Id", tipoBoletoCustomizado.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public TipoBoletoCustomizado GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Descricao FROM TipoBoletoCustomizado ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TipoBoletoCustomizado
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

        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM TipoBoletoCustomizado WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}