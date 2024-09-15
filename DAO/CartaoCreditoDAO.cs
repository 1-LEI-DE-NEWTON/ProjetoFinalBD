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
        private readonly ContaDAO contaDAO;
        //private readonly FaturaCartaoDAO faturaCartaoDAO;
        //private readonly CartaoTransacaoDAO cartaoTransacaoDAO;

        public CartaoCreditoDAO(string connectionString) : base(connectionString) 
        {
            categoriaCartaoDAO = new CategoriaCartaoDAO(connectionString);
            contaDAO = new ContaDAO(connectionString);
            //faturaCartaoDAO = new FaturaCartaoDAO(connectionString);
            //cartaoTransacaoDAO = new CartaoTransacaoDAO(connectionString);
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
                                LimiteCredito = reader.GetDouble(reader.GetOrdinal("LimiteCredito"))
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
                            return new CartaoCredito
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DataFechamento = reader.GetString(reader.GetOrdinal("DataFechamento")),
                                ContaId = reader.GetInt32(reader.GetOrdinal("ContaId")),
                                CategoriaCartaoId = reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId")),
                                LimiteCredito = reader.GetDouble(reader.GetOrdinal("LimiteCredito")),

                                CategoriaCartao = categoriaCartaoDAO.GetById(reader.GetInt32(reader.GetOrdinal("CategoriaCartaoId")))
                                //CartaoTransacoes = cartaoTransacaoDAO.GetByCartaoCreditoId(cartao.Id);
                        };
                        }
                    }
                }
            }
            return null;
        }

        //GetCartoesByContaId
        public List<CartaoCredito> GetCartoesByContaId(int contaId)
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
                                LimiteCredito = reader.GetDouble(reader.GetOrdinal("LimiteCredito"))
                            });
                        }
                        return cartoes;
                    }
                }
            }
        }                        
    }
}
