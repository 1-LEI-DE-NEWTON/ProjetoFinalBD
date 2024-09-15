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
        private readonly CartaoTransacaoDAO cartaoTransacaoDAO;
        public CompraDAO(string connectionString) : base(connectionString) 
        {
            corretorDAO = new CorretorDAO(connectionString);
            cartaoTransacaoDAO = new CartaoTransacaoDAO(connectionString);
        }

        public void Insert(Compra compra)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO compra (DataCompra, Valor, CartaoId, CategoriaId) " +
                    "VALUES (@DataCompra, @Valor, @CartaoId, @CategoriaId)";

                using (var command = connection.CreateCommand())
                {
                    

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
                                
                                //Corretor = 
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
