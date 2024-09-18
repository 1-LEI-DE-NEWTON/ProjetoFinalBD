using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class CartaoCreditoDAO : DAOBase
    {
        private readonly CategoriaCartaoDAO categoriaCartaoDAO;       
        private readonly CartaoTransacaoDAO cartaoTransacaoDAO;

        public CartaoCreditoDAO(string connectionString) : base(connectionString) 
        {
            categoriaCartaoDAO = new CategoriaCartaoDAO(connectionString);            
            cartaoTransacaoDAO = new CartaoTransacaoDAO(connectionString);
         }

        public void Insert(CartaoCredito cartaoCredito)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    
                    //Insere a categoria do Cartão de Crédito
                    categoriaCartaoDAO.Insert(cartaoCredito.CategoriaCartao);
                    //Obtem o categoriaCartaoId pelo ultimo inserido
                    cartaoCredito.CategoriaCartaoId = categoriaCartaoDAO.GetByLastAdded().Id;

                    //Insere a fatura do Cartão de Crédito       //A FAZER
                    
                    string query = "INSERT INTO cartaoCredito (DataFechamento, ContaId, " +
                        "CategoriaCartaoId, LimiteCredito) VALUES (@DataFechamento, " +
                        "@ContaId, @CategoriaCartaoId, @LimiteCredito)";

                    command.CommandText = query;
                    command.Parameters.AddWithValue("DataFechamento", cartaoCredito.DataFechamento);
                    command.Parameters.AddWithValue("ContaId", cartaoCredito.ContaId);
                    command.Parameters.AddWithValue("CategoriaCartaoId", cartaoCredito.CategoriaCartaoId);
                    command.Parameters.AddWithValue("LimiteCredito", cartaoCredito.LimiteCredito);                                                            

                    command.ExecuteNonQuery();

                    //Obtem cartaoId pelo ultimo adicionado
                    cartaoCredito.Id = GetLastAdded().Id;

                    //Adiciona as transações do Cartão de Crédito
                    foreach (var transacao in cartaoCredito.CartaoTransacoes)
                    {
                        transacao.CartaoId = cartaoCredito.Id;
                        cartaoTransacaoDAO.Insert(transacao);
                    }                                    
                }
            }
        }
        
        public CartaoCredito GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM cartaoCredito ORDER BY Id DESC LIMIT 1";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CartaoCredito
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataFechamento = reader.GetString(reader.GetOrdinal("DataFechamento")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),
                                CategoriaCartaoId = reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId")),
                                LimiteCredito = reader.GetDouble(reader.GetOrdinal("LimiteCredito")),

                                CategoriaCartao = categoriaCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId"))),

                                CartaoTransacoes = cartaoTransacaoDAO.GetTransacoesByCartaoId(reader.GetInt32(reader.GetOrdinal("Id")))
                            };
                        }
                    }
                }
            }
            return null;
        }                
        public CartaoCredito GetById(int id)
        {
            using (var connection = GetConnection())
            {
                CartaoCredito cartaoCredito = null;
                connection.Open();

                string query = "SELECT * FROM cartaoCredito WHERE Id = @Id";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cartaoCredito = new CartaoCredito
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataFechamento = reader.GetString(reader.GetOrdinal("DataFechamento")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),
                                CategoriaCartaoId = reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId")),
                                LimiteCredito = reader.GetDouble(reader.GetOrdinal("LimiteCredito")),

                                CategoriaCartao = categoriaCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId"))),


                                CartaoTransacoes = cartaoTransacaoDAO.GetTransacoesByCartaoId(id)
                            };
                        }
                    }
                }
                return cartaoCredito;
            }
        }        
        public List<CartaoCredito> GetCartoesCreditoByContaId(int contaId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM cartaoCredito WHERE ContaId = @ContaId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("ContaId", contaId);

                    using (var reader = command.ExecuteReader())
                    {
                        List<CartaoCredito> cartoes = new List<CartaoCredito>();

                        while (reader.Read())
                        {
                            cartoes.Add(new CartaoCredito
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataFechamento = reader.GetString(reader.GetOrdinal("DataFechamento")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),
                                CategoriaCartaoId = reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId")),
                                LimiteCredito = reader.GetDouble(reader.GetOrdinal("LimiteCredito")),

                                CategoriaCartao = categoriaCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId"))),

                                CartaoTransacoes = new List<CartaoTransacao>()
                            });
                        }

                        if (cartoes.Count > 0)
                        {
                            foreach (var cartao in cartoes)
                            {
                                cartao.CartaoTransacoes = cartaoTransacaoDAO.GetTransacoesByCartaoId(cartao.Id);
                            }
                        }
                        return cartoes;
                    }
                }
            }
        }        
        public void Update(CartaoCredito cartaoCredito)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Atualiza a categoria do Cartão de Crédito
                    cartaoCredito.CategoriaCartao.Id = categoriaCartaoDAO.GetById(cartaoCredito.CategoriaCartaoId).Id;
                    categoriaCartaoDAO.Update(cartaoCredito.CategoriaCartao);

                    string query = "UPDATE cartaoCredito SET DataFechamento = @DataFechamento, " +
                        "ContaId = @ContaId, CategoriaCartaoId = @CategoriaCartaoId, " +
                        "LimiteCredito = @LimiteCredito WHERE Id = @Id";

                    command.CommandText = query;
                    command.Parameters.AddWithValue("DataFechamento", cartaoCredito.DataFechamento);
                    command.Parameters.AddWithValue("ContaId", cartaoCredito.ContaId);
                    command.Parameters.AddWithValue("CategoriaCartaoId", cartaoCredito.CategoriaCartaoId);
                    command.Parameters.AddWithValue("LimiteCredito", cartaoCredito.LimiteCredito);
                    command.Parameters.AddWithValue("Id", cartaoCredito.Id);

                    command.ExecuteNonQuery();

                    //Atualiza cartaoTransacoes
                    foreach (var transacao in cartaoCredito.CartaoTransacoes)
                    {
                        transacao.CartaoId = cartaoCredito.Id;
                        cartaoTransacaoDAO.Update(transacao);
                    }
                }
            }
        }
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                //Deleta a categoriaCartao do cartao
                categoriaCartaoDAO.Delete(GetById(id).CategoriaCartaoId);

                using (var command = connection.CreateCommand())
                {
                    string query = "DELETE FROM cartaoCredito WHERE Id = @Id";

                    command.CommandText = query;
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }

                //Deleta as transações do cartão
                cartaoTransacaoDAO.DeleteByCartaoId(id);
            }
        }
        public void DeleteByContaId(int contaId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var cartoes = GetCartoesCreditoByContaId(contaId);

                //Delete categoriaCartao dos cartoes associados
                foreach (var cartao in cartoes)
                {
                    categoriaCartaoDAO.Delete(cartao.CategoriaCartaoId);
                }                

                using (var command = connection.CreateCommand())
                {
                    string query = "DELETE FROM cartaoCredito WHERE ContaId = @ContaId";

                    command.CommandText = query;
                    command.Parameters.AddWithValue("ContaId", contaId);

                    command.ExecuteNonQuery();
                }

                //Deleta as transações associadas
                foreach (var cartao in cartoes)
                {
                    cartaoTransacaoDAO.DeleteByCartaoId(cartao.Id);
                }
            }
        }                
    }
}
