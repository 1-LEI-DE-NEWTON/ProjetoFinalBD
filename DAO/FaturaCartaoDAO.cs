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
                                CartaoCreditoId = reader.GetInt32(reader.GetOrdinal("CartaoCreditoId")),

                                ItensFaturas = itensFaturaDAO.GetByFaturaCartaoId(reader.GetInt32(reader.GetOrdinal("Id"))),

                                BoletosCustomizados = boletoCustomizadoDAO.GetByFaturaCartaoId(reader.GetInt32(reader.GetOrdinal("Id"))),

                                Pagamentos = pagamentoDAO.GetByFaturaCartaoId(reader.GetInt32(reader.GetOrdinal("Id")))                                
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

                                ItensFaturas = itensFaturaDAO.GetByFaturaCartaoId(reader.GetInt32(reader.GetOrdinal("Id"))),

                                BoletosCustomizados = boletoCustomizadoDAO.GetByFaturaCartaoId(reader.GetInt32(reader.GetOrdinal("Id"))),

                                Pagamentos = pagamentoDAO.GetByFaturaCartaoId(reader.GetInt32(reader.GetOrdinal("Id")))                                
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
        public List<FaturaCartao> GetByCartaoId(int id)
        {
            List<FaturaCartao> faturas = new List<FaturaCartao>();

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM FaturaCartao WHERE CartaoCreditoId = @CartaoCreditoId";
                    command.Parameters.AddWithValue("CartaoCreditoId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            faturas.Add(new FaturaCartao
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                MesReferencia = reader.GetString(reader.GetOrdinal("MesReferencia")),
                                AnoReferencia = reader.GetString(reader.GetOrdinal("AnoReferencia")),
                                Valor = reader.GetString(reader.GetOrdinal("Valor")),
                                DataPagamento = reader.GetString(reader.GetOrdinal("DataPagamento")),
                                CartaoCreditoId = reader.GetInt32(reader.GetOrdinal("CartaoCreditoId")),

                                ItensFaturas = new List<ItensFatura>(),
                                BoletosCustomizados = new List<BoletoCustomizado>(),
                                Pagamentos = new List<Pagamento>()
                            });
                        }

                        if (faturas.Count > 0)
                        {
                            foreach (var fatura in faturas)
                            {
                                fatura.ItensFaturas = itensFaturaDAO.GetByFaturaCartaoId(fatura.Id);
                                fatura.BoletosCustomizados = boletoCustomizadoDAO.GetByFaturaCartaoId(fatura.Id);
                                fatura.Pagamentos = pagamentoDAO.GetByFaturaCartaoId(fatura.Id);
                            }
                        }                        
                    }
                }
            }
            return faturas;
        }

        public void Update(FaturaCartao faturaCartao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE FaturaCartao SET MesReferencia = @MesReferencia, " +
                        "AnoReferencia = @AnoReferencia, Valor = @Valor, DataPagamento = " +
                        "@DataPagamento, CartaoCreditoId = @CartaoCreditoId WHERE Id = @Id";
                    
                    command.Parameters.AddWithValue("MesReferencia", faturaCartao.MesReferencia);
                    command.Parameters.AddWithValue("AnoReferencia", faturaCartao.AnoReferencia);
                    command.Parameters.AddWithValue("Valor", faturaCartao.Valor);
                    command.Parameters.AddWithValue("DataPagamento", faturaCartao.DataPagamento);
                    command.Parameters.AddWithValue("CartaoCreditoId", faturaCartao.CartaoCreditoId);
                    command.Parameters.AddWithValue("Id", faturaCartao.Id);

                    command.ExecuteNonQuery();
                }

                //Atualiza os itens da fatura
                foreach (var item in faturaCartao.ItensFaturas)
                {
                    itensFaturaDAO.Update(item);
                }
                
                //Atualiza BoletosCustomizados
                foreach (var boleto in faturaCartao.BoletosCustomizados)
                {
                    boletoCustomizadoDAO.Update(boleto);
                }
                
                //Atualiza Pagamentos
                foreach (var pagamento in faturaCartao.Pagamentos)
                {
                    pagamentoDAO.Update(pagamento);
                }
            }
        }
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                //Deleta os itens da fatura
                itensFaturaDAO.DeleteByFaturaCartaoId(id);

                //Deleta os Boletos Customizados
                boletoCustomizadoDAO.DeleteByFaturaCartaoId(id);

                //Deleta os pagamentos
                pagamentoDAO.DeleteByFaturaCartaoId(id);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM FaturaCartao WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteByCartaoId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                //Obtem a lista das faturas
                var faturas = GetByCartaoId(id);

                //Deleta os itens da fatura
                foreach (var fatura in faturas)
                {
                    itensFaturaDAO.DeleteByFaturaCartaoId(fatura.Id);
                }                

                //Deleta os Boletos Customizados
                foreach (var fatura in faturas)
                {
                    boletoCustomizadoDAO.DeleteByFaturaCartaoId(fatura.Id);
                }                                

                //Deleta os pagamentos
                foreach (var fatura in faturas)
                {
                    pagamentoDAO.DeleteByFaturaCartaoId(fatura.Id);
                }                

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM FaturaCartao WHERE CartaoCreditoId = @CartaoCreditoId";
                    command.Parameters.AddWithValue("CartaoCreditoId", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
