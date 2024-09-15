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
        private readonly FaturaCartaoDAO faturaCartaoDAO;
        private readonly CartaoTransacaoDAO cartaoTransacaoDAO;

        public CartaoCreditoDAO(string connectionString) : base(connectionString) 
        {
            categoriaCartaoDAO = new CategoriaCartaoDAO(connectionString);
            contaDAO = new ContaDAO(connectionString);
            faturaCartaoDAO = new FaturaCartaoDAO(connectionString);
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

                    categoriaCartaoDAO
                    //Baseado em 
                    //public class CartaoCredito
                    // {
                    //     public int Id { get; set; }
                    //     public string DataFechamento { get; set; }
                    //     public int? ContaId { get; set; } // Foreign Key para Conta
                    //     public Conta conta { get; set; }
                    //     public int? CategoriaCartaoId { get; set; } // Foreign Key para CategoriaCartao
                    //     public CategoriaCartao CategoriaCartao { get; set; }
                    //     public double LimiteCredito { get; set; }
                    //     // Relacionamento 1-N com FaturaCartao
                    //     public List<FaturaCartao> FaturasCartao { get; set; }
                    // }

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

    }
}
