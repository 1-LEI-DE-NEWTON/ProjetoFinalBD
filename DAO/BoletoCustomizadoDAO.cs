﻿using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.DAO
{
    public class BoletoCustomizadoDAO : DAOBase
    {
        private readonly PagamentoDAO pagamentoDAO;
        private readonly TipoBoletoCustomizadoDAO tipoBoletoCustomizadoDAO;
        public BoletoCustomizadoDAO(string connectionString) : base(connectionString) 
        {
            pagamentoDAO = new PagamentoDAO(connectionString);
            tipoBoletoCustomizadoDAO = new TipoBoletoCustomizadoDAO(connectionString);
        }

        public void Insert(BoletoCustomizado boletoCustomizado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {                    
                    command.CommandText = "INSERT INTO BoletoCustomizado (Valor, DataVencimento, " +
                        "DataGeracao, CodigoBarras, TipoBoletoCustomizadoId, FaturaCartaoId) VALUES (@Valor, " +
                        "@DataVencimento, @DataGeracao, @CodigoBarras, @TipoBoletoCustomizadoId, @FaturaCartaoId)";

                    command.Parameters.AddWithValue("Valor", boletoCustomizado.Valor);
                    command.Parameters.AddWithValue("DataVencimento", boletoCustomizado.DataVencimento);
                    command.Parameters.AddWithValue("DataGeracao", boletoCustomizado.DataGeracao);
                    command.Parameters.AddWithValue("CodigoBarras", boletoCustomizado.CodigoBarras);
                    command.Parameters.AddWithValue("TipoBoletoCustomizadoId", boletoCustomizado.TipoBoletoCustomizadoId);
                    command.Parameters.AddWithValue("FaturaCartaoId", boletoCustomizado.FaturaCartaoId);
                    
                    command.ExecuteNonQuery();
                }

                //Obtem BoletoCustomizadoId pelo ultimo adicionado
                boletoCustomizado.Id = GetLastAdded().Id;

                //Adiciona Pagamentos
                foreach (var pagamento in boletoCustomizado.Pagamentos)
                {
                    pagamento.BoletoCustomizadoId = boletoCustomizado.Id;
                    pagamentoDAO.Insert(pagamento);
                }
            }
        }
        public BoletoCustomizado GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM BoletoCustomizado ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BoletoCustomizado
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Valor = reader.GetString(reader.GetOrdinal("Valor")),
                                DataVencimento = reader.GetDateTime(reader.GetOrdinal("DataVencimento")),
                                DataGeracao = reader.GetDateTime(reader.GetOrdinal("DataGeracao")),
                                CodigoBarras = reader.GetString(reader.GetOrdinal("CodigoBarras")),
                                TipoBoletoCustomizadoId = reader.GetInt32(reader.GetOrdinal("TipoBoletoCustomizadoId")),
                                FaturaCartaoId = reader.GetInt32(reader.GetOrdinal("FaturaCartaoId")),

                                Pagamentos = pagamentoDAO.GetByBoletoCustomizadoId(reader.GetInt32(reader.GetOrdinal("Id")))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public BoletoCustomizado GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM BoletoCustomizado WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BoletoCustomizado
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Valor = reader.GetString(reader.GetOrdinal("Valor")),
                                DataVencimento = reader.GetDateTime(reader.GetOrdinal("DataVencimento")),
                                DataGeracao = reader.GetDateTime(reader.GetOrdinal("DataGeracao")),
                                CodigoBarras = reader.GetString(reader.GetOrdinal("CodigoBarras")),
                                TipoBoletoCustomizadoId = reader.GetInt32(reader.GetOrdinal("TipoBoletoCustomizadoId")),
                                FaturaCartaoId = reader.GetInt32(reader.GetOrdinal("FaturaCartaoId")),

                                Pagamentos = pagamentoDAO.GetByBoletoCustomizadoId(reader.GetInt32(reader.GetOrdinal("Id")))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<BoletoCustomizado> GetByFaturaCartaoId(int faturaCartaoId)
        {
            var boletosCustomizados = new List<BoletoCustomizado>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM BoletoCustomizado WHERE FaturaCartaoId = @FaturaCartaoId";
                    command.Parameters.AddWithValue("FaturaCartaoId", faturaCartaoId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boletosCustomizados.Add(new BoletoCustomizado
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Valor = reader.GetString(reader.GetOrdinal("Valor")),
                                DataVencimento = reader.GetDateTime(reader.GetOrdinal("DataVencimento")),
                                DataGeracao = reader.GetDateTime(reader.GetOrdinal("DataGeracao")),
                                CodigoBarras = reader.GetString(reader.GetOrdinal("CodigoBarras")),
                                TipoBoletoCustomizadoId = reader.GetInt32(reader.GetOrdinal("TipoBoletoCustomizadoId")),
                                FaturaCartaoId = reader.GetInt32(reader.GetOrdinal("FaturaCartaoId")),

                                Pagamentos = pagamentoDAO.GetByBoletoCustomizadoId(reader.GetInt32(reader.GetOrdinal("Id")))
                            });
                        }
                    }
                }
            }
            return boletosCustomizados;
        }

        public void Update(BoletoCustomizado boletoCustomizado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE BoletoCustomizado SET Valor = @Valor, DataVencimento = @DataVencimento, DataGeracao = @DataGeracao, CodigoBarras = @CodigoBarras, TipoBoletoCustomizadoId = @TipoBoletoCustomizadoId, FaturaCartaoId = @FaturaCartaoId WHERE Id = @Id";
                    command.Parameters.AddWithValue("Valor", boletoCustomizado.Valor);
                    command.Parameters.AddWithValue("DataVencimento", boletoCustomizado.DataVencimento);
                    command.Parameters.AddWithValue("DataGeracao", boletoCustomizado.DataGeracao);
                    command.Parameters.AddWithValue("CodigoBarras", boletoCustomizado.CodigoBarras);
                    command.Parameters.AddWithValue("TipoBoletoCustomizadoId", boletoCustomizado.TipoBoletoCustomizadoId);
                    command.Parameters.AddWithValue("FaturaCartaoId", boletoCustomizado.FaturaCartaoId);
                    command.Parameters.AddWithValue("Id", boletoCustomizado.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM BoletoCustomizado WHERE Id = @Id";
                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }

                pagamentoDAO.DeleteByBoletoCustomizadoId(id);

                tipoBoletoCustomizadoDAO.Delete(GetById(id).TipoBoletoCustomizadoId);
            }
        }
        
        public void DeleteByFaturaCartaoId(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM BoletoCustomizado WHERE FaturaCartaoId = @FaturaCartaoId";
                    command.Parameters.AddWithValue("FaturaCartaoId", id);

                    command.ExecuteNonQuery();
                }
            }

            //Deleta Pagamentos
            foreach (var boletoCustomizado in GetByFaturaCartaoId(id))
            {
                pagamentoDAO.DeleteByBoletoCustomizadoId(boletoCustomizado.Id);
            }

            //Deleta TipoBoletoCustomizado
            foreach (var boletoCustomizado in GetByFaturaCartaoId(id))
            {
                tipoBoletoCustomizadoDAO.Delete(boletoCustomizado.TipoBoletoCustomizadoId);
            }
        }
    }
}
