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
        private readonly CartaoCreditoDAO cartaoCreditoDAO;
        private readonly MovimentacaoCartaoDAO movimentacaoCartaoDAO;
        public CartaoTransacaoDAO(string connectionString) : base(connectionString) 
        {
            bandeiraCartaoDAO = new BandeiraCartaoDAO(connectionString);
            cartaoCreditoDAO = new CartaoCreditoDAO(connectionString);
            movimentacaoCartaoDAO = new MovimentacaoCartaoDAO(connectionString);
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

                //Adiciona MovimentacoesCartao
                foreach (var movimentacaoCartao in cartaoTransacao.MovimentacoesCartao)
                {
                    movimentacaoCartao.CartaoTransacaoId = cartaoTransacao.Id;
                    movimentacaoCartaoDAO.Insert(movimentacaoCartao);
                }
            }
        }        
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
                                NomeCartao = reader.GetString(reader.GetOrdinal("NomeCartao")),
                                TipoTransacao = reader.GetString(reader.GetOrdinal("TipoTransacao")),

                                BandeiraCartao = bandeiraCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("BandeiraCartaoId"))),

                                CartaoCredito = cartaoCreditoDAO.GetById(reader.GetInt32(reader.GetOrdinal("CartaoId"))),

                                MovimentacoesCartao = movimentacaoCartaoDAO.GetByCartaoTransacaoId(id)
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
        public List<CartaoTransacao> GetTransacoesByCartaoId(int cartaoId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM cartaoTransacao WHERE CartaoId = @CartaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoId", cartaoId);

                    using (var reader = command.ExecuteReader())
                    {
                        List<CartaoTransacao> cartaoTransacoes = new List<CartaoTransacao>();

                        while (reader.Read())
                        {
                            cartaoTransacoes.Add(new CartaoTransacao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                NumeroCartao = reader.GetString(reader.GetOrdinal("NumeroCartao")),
                                Cvc = reader.GetString(reader.GetOrdinal("Cvc")),
                                CartaoId = reader.GetInt32(reader.GetOrdinal("CartaoId")),
                                TipoCartao = reader.GetString(reader.GetOrdinal("TipoCartao")),
                                IsInternacional = reader.GetBoolean(reader.GetOrdinal("IsInternacional")),
                                BandeiraCartaoId = reader.GetInt32(reader.GetOrdinal("BandeiraCartaoId")),
                                NomeCartao = reader.GetString(reader.GetOrdinal("NomeCartao")),
                                TipoTransacao = reader.GetString(reader.GetOrdinal("TipoTransacao")),

                                BandeiraCartao = bandeiraCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("BandeiraCartaoId"))),

                                CartaoCredito = cartaoCreditoDAO.GetById(cartaoId),

                                MovimentacoesCartao = new List<MovimentacaoCartao>()
                            });
                        }

                        if (cartaoTransacoes.Count > 0)
                        {
                            foreach (var cartaoTransacao in cartaoTransacoes)
                            {
                                cartaoTransacao.MovimentacoesCartao = movimentacaoCartaoDAO.GetByCartaoTransacaoId(cartaoTransacao.Id);
                            }
                        }
                        return cartaoTransacoes;
                    }
                }
            }
        }        
        public void Update(CartaoTransacao cartaoTransacao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();                

                string query = "UPDATE cartaoTransacao SET NumeroCartao = @NumeroCartao, Cvc = @Cvc, CartaoId = @CartaoId, " +
                    "TipoCartao = @TipoCartao, NomeCartao = @NomeCartao, " +
                    "TipoTransacao = @TipoTransacao, IsInternacional = @IsInternacional, " +
                    "BandeiraCartaoId = @BandeiraCartaoId WHERE Id = @Id";

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
                    command.Parameters.AddWithValue("Id", cartaoTransacao.Id);

                    command.ExecuteNonQuery();
                }

                //Atualiza movimentacoesCartao
                foreach (var movimentacaoCartao in cartaoTransacao.MovimentacoesCartao)
                {
                    movimentacaoCartao.CartaoTransacaoId = cartaoTransacao.Id;
                    movimentacaoCartaoDAO.Update(movimentacaoCartao);
                }
            }
        }
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM cartaoTransacao WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }

                //Deleta movimentacoesCartao
                movimentacaoCartaoDAO.DeleteByCartaoTransacaoId(id);
            }
        }

        //Deleta todas as transacoes de um cartao
        public void DeleteByCartaoId(int cartaoId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var transacoes = GetTransacoesByCartaoId(cartaoId);

                string query = "DELETE FROM cartaoTransacao WHERE CartaoId = @CartaoId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("CartaoId", cartaoId);

                    command.ExecuteNonQuery();
                }

                //Deleta movimentacoesCartao
                foreach (var transacao in transacoes)
                {                    
                    movimentacaoCartaoDAO.DeleteByCartaoTransacaoId(transacao.Id);
                }
            }
        }
    }
}
