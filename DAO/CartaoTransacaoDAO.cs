using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public class CartaoTransacaoDAO : DAOBase        
    {
        private readonly BandeiraCartaoDAO bandeiraCartaoDAO;
        public CartaoTransacaoDAO(string connectionString) : base(connectionString) 
        {
            bandeiraCartaoDAO = new BandeiraCartaoDAO(connectionString);
         }

        public void Insert(CartaoTransacao cartaoTransacao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO cartaoTransacao (NumeroCartao, Cvc, CartaoId, TipoCartao, NomeCartao, " +
                    "TipoTransacao, IsInternacional, BandeiraCartaoId) VALUES (@NumeroCartao, " +
                    "@Cvc, @CartaoId, @TipoCartao, @NomeCartao, @TipoTransacao, @IsInternacional, @BandeiraCartaoId)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("NumeroCartao", cartaoTransacao.NumeroCartao);
                    command.Parameters.AddWithValue("Cvc", cartaoTransacao.Cvc);
                    command.Parameters.AddWithValue("CartaoId", cartaoTransacao.CartaoId);
                    command.Parameters.AddWithValue("TipoCartao", cartaoTransacao.TipoCartao);
                    command.Parameters.AddWithValue("NomeCartao", cartaoTransacao.NomeCartao);
                    command.Parameters.AddWithValue("TipoTransacao", cartaoTransacao.TipoTransacao);
                    command.Parameters.AddWithValue("IsInternacional", cartaoTransacao.IsInternacional);
                    command.Parameters.AddWithValue("BandeiraCartaoId", cartaoTransacao.BandeiraCartaoId);

                    
                    command.ExecuteNonQuery();
                }
            }
        }

        //GetById
        public CartaoTransacao GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM cartaoTransacao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CartaoTransacao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                NumeroCartao = reader.GetString(reader.GetOrdinal("NumeroCartao")),
                                Cvc = reader.GetString(reader.GetOrdinal("Cvc")),
                                CartaoId = reader.GetInt32(reader.GetOrdinal("CartaoId")),
                                TipoCartao = reader.GetString(reader.GetOrdinal("TipoCartao")),
                                IsInternacional = reader.GetBoolean(reader.GetOrdinal("IsInternacional")),
                                BandeiraCartaoId = reader.GetInt32(reader.GetOrdinal("BandeiraCartaoId")),

                                BandeiraCartao = bandeiraCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("BandeiraCartaoId")))

                                //CartaoCredito = 
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
