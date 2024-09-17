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
                //        public class Compra
                //{
                //    public int Id { get; set; }
                //    public double Valor { get; set; }
                //    public int QuantidadeParcelas { get; set; }
                //    public double TaxaParcelamento { get; set; }
                //    public string Credor { get; set; }
                //    public int? CorretorId { get; set; } // Foreign Key para Corretor
                //    public Corretor Corretor { get; set; }
                //    public int? CartaoTransacaoId { get; set; } // Foreign Key para CartaoTransacao
                //    public CartaoTransacao CartaoTransacao { get; set; }
                //    public DateTime DataCompra { get; set; }
                //}
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
