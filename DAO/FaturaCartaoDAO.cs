using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class FaturaCartaoDAO : DAOBase
    {        
        private readonly ItensFaturaDAO itensFaturaDAO;
        private readonly BoletoCustomizadoDAO boletoCustomizadoDAO;
        private readonly PagamentoDAO pagamentoDAO;
        public FaturaCartaoDAO(string connectionString) : base(connectionString) 
        {            
            itensFaturaDAO = new ItensFaturaDAO(connectionString);
            boletoCustomizadoDAO = new BoletoCustomizadoDAO(connectionString);
            pagamentoDAO = new PagamentoDAO(connectionString);
        }
        
        public void Insert(FaturaCartao faturaCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();                
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO FaturaCartao (MesReferencia, " +
                        "AnoReferencia, Valor, DataPagamento, CartaoCreditoId) VALUES (@MesReferencia, " +
                        "@AnoReferencia, @Valor, @DataPagamento, @CartaoCreditoId)";

                    command.Parameters.AddWithValue("MesReferencia", faturaCartao.MesReferencia);
                    command.Parameters.AddWithValue("AnoReferencia", faturaCartao.AnoReferencia);
                    command.Parameters.AddWithValue("Valor", faturaCartao.Valor);
                    command.Parameters.AddWithValue("DataPagamento", faturaCartao.DataPagamento);
                    command.Parameters.AddWithValue("CartaoCreditoId", faturaCartao.CartaoCreditoId);
                    
                    command.ExecuteNonQuery();
                }

                //Obtem o FaturaCartaoId pelo ultimo adicionado
                faturaCartao.Id = GetLastAdded().Id;

                //Adiciona os itens da fatura
                foreach (var item in faturaCartao.ItensFaturas)
                {
                    item.FaturaCartaoId = faturaCartao.Id;
                    itensFaturaDAO.Insert(item);
                }
                
                //Adiciona os BoletosCustomizados
                foreach (var boleto in faturaCartao.BoletosCustomizados)
                {
                    boleto.FaturaCartaoId = faturaCartao.Id;
                    boletoCustomizadoDAO.Insert(boleto);

                    //Adiciona os pagamentos que estão associados a Boletos Customizados
                    foreach (var pagamento in boleto.Pagamentos)
                    {
                        pagamento.FaturaCartaoId = faturaCartao.Id;
                        pagamento.BoletoCustomizadoId = boleto.Id;
                        pagamentoDAO.Insert(pagamento);
                    }
                }

                //Adiciona os pagamentos que não estão associados a Boletos Customizados
                foreach (var pagamento in faturaCartao.Pagamentos)
                {
                    if (pagamento.BoletoCustomizadoId == null)
                    {
                        pagamento.FaturaCartaoId = faturaCartao.Id;
                        pagamentoDAO.Insert(pagamento);
                    }                    
                }
            }
        }
        
        public FaturaCartao GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM FaturaCartao ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FaturaCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                MesReferencia = reader.GetString(reader.GetOrdinal("MesReferencia")),
                                AnoReferencia = reader.GetString(reader.GetOrdinal("AnoReferencia")),
                                Valor = reader.GetString(reader.GetOrdinal("Valor")),
                                DataPagamento = reader.GetString(reader.GetOrdinal("DataPagamento")),
                                CartaoCreditoId = reader.GetInt32(reader.GetOrdinal("CartaoCreditoId"))
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
        public FaturaCartao GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM FaturaCartao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FaturaCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                MesReferencia = reader.GetString(reader.GetOrdinal("MesReferencia")),
                                AnoReferencia = reader.GetString(reader.GetOrdinal("AnoReferencia")),
                                Valor = reader.GetString(reader.GetOrdinal("Valor")),
                                DataPagamento = reader.GetString(reader.GetOrdinal("DataPagamento")),
                                CartaoCreditoId = reader.GetInt32(reader.GetOrdinal("CartaoCreditoId")),
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
