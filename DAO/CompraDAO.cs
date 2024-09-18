using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class CompraDAO : DAOBase
    {
        private readonly CorretorDAO corretorDAO;        
        public CompraDAO(string connectionString) : base(connectionString) 
        {
            corretorDAO = new CorretorDAO(connectionString);            
        }

        public void Insert(Compra compra)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                //Adiciona Corretor
                corretorDAO.Insert(compra.Corretor);
                //Obtem CorretorId pelo ultimo adicionado
                compra.CorretorId = corretorDAO.GetLastAdded().Id;


                //Query para compra
                string query = "INSERT INTO compra (Valor, QuantidadeParcelas, TaxaParcelamento, Credor, " +
                    "CorretorId, CartaoTransacaoId, DataCompra) VALUES (@Valor, @QuantidadeParcelas, " +
                    "@TaxaParcelamento, @Credor, @CorretorId, @CartaoTransacaoId, @DataCompra)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Valor", compra.Valor);
                    command.Parameters.AddWithValue("QuantidadeParcelas", compra.QuantidadeParcelas);
                    command.Parameters.AddWithValue("TaxaParcelamento", compra.TaxaParcelamento);
                    command.Parameters.AddWithValue("Credor", compra.Credor);
                    command.Parameters.AddWithValue("CorretorId", compra.CorretorId);
                    command.Parameters.AddWithValue("CartaoTransacaoId", compra.CartaoTransacaoId);
                    command.Parameters.AddWithValue("DataCompra", compra.DataCompra);                    
                    
                    command.ExecuteNonQuery();
                }
            }
        }
        public Compra GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM compra WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Compra
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataCompra = reader.GetDateTime(reader.GetOrdinal("DataCompra")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoTransacaoId")),
                                QuantidadeParcelas = reader.GetInt32(reader.GetOrdinal("QuantidadeParcelas")),
                                TaxaParcelamento = reader.GetDouble(reader.GetOrdinal("TaxaParcelamento")),
                                CorretorId = reader.GetInt32(reader.GetOrdinal("CorretorId")),
                                Credor = reader.GetString(reader.GetOrdinal("Credor")),

                                Corretor = corretorDAO.GetById(reader.GetInt32(reader.GetOrdinal("CorretorId"))),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Compra> GetAll()
        {
            List<Compra> compras = new List<Compra>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM compra";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            compras.Add(new Compra
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataCompra = reader.GetDateTime(reader.GetOrdinal("DataCompra")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoTransacaoId")),
                                QuantidadeParcelas = reader.GetInt32(reader.GetOrdinal("QuantidadeParcelas")),
                                TaxaParcelamento = reader.GetDouble(reader.GetOrdinal("TaxaParcelamento")),
                                CorretorId = reader.GetInt32(reader.GetOrdinal("CorretorId")),
                                Credor = reader.GetString(reader.GetOrdinal("Credor")),

                                Corretor = corretorDAO.GetById(reader.GetInt32(reader.GetOrdinal("CorretorId"))),
                            });
                        }
                    }
                }
            }
            return compras;
        }

        public Compra GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM compra ORDER BY Id DESC LIMIT 1";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Compra
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataCompra = reader.GetDateTime(reader.GetOrdinal("DataCompra")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoTransacaoId")),
                                QuantidadeParcelas = reader.GetInt32(reader.GetOrdinal("QuantidadeParcelas")),
                                TaxaParcelamento = reader.GetDouble(reader.GetOrdinal("TaxaParcelamento")),
                                CorretorId = reader.GetInt32(reader.GetOrdinal("CorretorId")),
                                Credor = reader.GetString(reader.GetOrdinal("Credor")),

                                Corretor = corretorDAO.GetById(reader.GetInt32(reader.GetOrdinal("CorretorId"))),
                            };
                        }
                    }
                }
            }
            return null;
        }
        
        public List<Compra> GetByCartaoTransacaoId(int id)
        {
            List<Compra> compras = new List<Compra>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM compra WHERE CartaoTransacaoId = @CartaoTransacaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoTransacaoId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            compras.Add(new Compra
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataCompra = reader.GetDateTime(reader.GetOrdinal("DataCompra")),
                                Valor = reader.GetDouble(reader.GetOrdinal("Valor")),
                                CartaoTransacaoId = reader.GetInt32(reader.GetOrdinal("CartaoTransacaoId")),
                                QuantidadeParcelas = reader.GetInt32(reader.GetOrdinal("QuantidadeParcelas")),
                                TaxaParcelamento = reader.GetDouble(reader.GetOrdinal("TaxaParcelamento")),
                                CorretorId = reader.GetInt32(reader.GetOrdinal("CorretorId")),
                                Credor = reader.GetString(reader.GetOrdinal("Credor")),

                                Corretor = corretorDAO.GetById(reader.GetInt32(reader.GetOrdinal("CorretorId"))),
                            });
                        }
                    }
                }
            }
            return compras;
        }

        public void Update(Compra compra)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                //Atualiza Corretor
                compra.Corretor = corretorDAO.GetById(compra.CorretorId);
                corretorDAO.Update(compra.Corretor);

                string query = "UPDATE compra SET Valor = @Valor, QuantidadeParcelas = @QuantidadeParcelas, " +
                    "TaxaParcelamento = @TaxaParcelamento, Credor = @Credor, CorretorId = @CorretorId, " +
                    "CartaoTransacaoId = @CartaoTransacaoId, DataCompra = @DataCompra WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Valor", compra.Valor);
                    command.Parameters.AddWithValue("QuantidadeParcelas", compra.QuantidadeParcelas);
                    command.Parameters.AddWithValue("TaxaParcelamento", compra.TaxaParcelamento);
                    command.Parameters.AddWithValue("Credor", compra.Credor);
                    command.Parameters.AddWithValue("CorretorId", compra.CorretorId);
                    command.Parameters.AddWithValue("CartaoTransacaoId", compra.CartaoTransacaoId);
                    command.Parameters.AddWithValue("DataCompra", compra.DataCompra);
                    command.Parameters.AddWithValue("Id", compra.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM compra WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
                corretorDAO.Delete(GetById(id).CorretorId);
            }
        }
        public void DeleteByCartaoTransacaoId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var compras = GetByCartaoTransacaoId(id);

                string query = "DELETE FROM compra WHERE CartaoTransacaoId = @CartaoTransacaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoTransacaoId", id);

                    command.ExecuteNonQuery();
                }
                
                foreach (var compra in compras)
                {
                    corretorDAO.Delete(compra.CorretorId);
                }
            }
        }
    }
}
