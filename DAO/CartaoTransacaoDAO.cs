﻿using System;
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
        private readonly MovimentacaoCartaoDAO movimentacaoCartaoDAO;
        private readonly CompraDAO compraDAO;
        public CartaoTransacaoDAO(string connectionString) : base(connectionString) 
        {
            bandeiraCartaoDAO = new BandeiraCartaoDAO(connectionString);            
            movimentacaoCartaoDAO = new MovimentacaoCartaoDAO(connectionString);
            compraDAO = new CompraDAO(connectionString);
        }

        public void Insert(CartaoTransacao cartaoTransacao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                //Adiciona a bandeira do Cartão de Crédito                    
                bandeiraCartaoDAO.Insert(cartaoTransacao.BandeiraCartao);

                //Obtem bandeiraCartaoId pelo ultimo adicionado
                cartaoTransacao.BandeiraCartaoId = bandeiraCartaoDAO.GetLastAdded().Id;

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

                //Obtem cartaoTransacaoId pelo ultimo inserido
                cartaoTransacao.Id = GetLastAdded().Id;

                //Adiciona MovimentacoesCartao
                foreach (var movimentacaoCartao in cartaoTransacao.MovimentacoesCartao)
                {
                    movimentacaoCartao.CartaoTransacaoId = cartaoTransacao.Id;
                    movimentacaoCartaoDAO.Insert(movimentacaoCartao);
                }

                //Adiciona compras
                foreach (var compra in cartaoTransacao.Compras)
                {
                    compra.CartaoTransacaoId = cartaoTransacao.Id;
                    compraDAO.Insert(compra);
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

                                MovimentacoesCartao = movimentacaoCartaoDAO.GetByCartaoTransacaoId(id),

                                Compras = compraDAO.GetByCartaoTransacaoId(id)
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

                                MovimentacoesCartao = new List<MovimentacaoCartao>(),

                                Compras = new List<Compra>()
                            });
                        }

                        if (cartaoTransacoes.Count > 0)
                        {
                            foreach (var cartaoTransacao in cartaoTransacoes)
                            {
                                cartaoTransacao.MovimentacoesCartao = movimentacaoCartaoDAO.GetByCartaoTransacaoId(cartaoTransacao.Id);
                                cartaoTransacao.Compras = compraDAO.GetByCartaoTransacaoId(cartaoTransacao.Id);
                            }
                        }
                        return cartaoTransacoes;
                    }
                }
            }
        }
        
        public CartaoTransacao GetLastAdded()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM cartaoTransacao ORDER BY Id DESC LIMIT 1";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

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

                                MovimentacoesCartao = movimentacaoCartaoDAO.GetByCartaoTransacaoId(reader.GetInt32(reader.GetOrdinal("Id"))),

                                Compras = compraDAO.GetByCartaoTransacaoId(reader.GetInt32(reader.GetOrdinal("Id")))
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
        public void Update(CartaoTransacao cartaoTransacao)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                //Atualiza bandeiraCartao
                cartaoTransacao.BandeiraCartao = bandeiraCartaoDAO.GetById(cartaoTransacao.BandeiraCartaoId);
                bandeiraCartaoDAO.Update(cartaoTransacao.BandeiraCartao);

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

                //Atualiza compras
                foreach (var compra in cartaoTransacao.Compras)
                {
                    compra.CartaoTransacaoId = cartaoTransacao.Id;
                    compraDAO.Update(compra);
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
                bandeiraCartaoDAO.Delete(GetById(id).BandeiraCartaoId);

                movimentacaoCartaoDAO.DeleteByCartaoTransacaoId(id);

                compraDAO.DeleteByCartaoTransacaoId(id);
            }
        }        
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

                //Deleta bandeiraCartao
                foreach (var transacao in transacoes)
                {
                    bandeiraCartaoDAO.Delete(transacao.BandeiraCartaoId);
                }

                //Deleta compras
                foreach (var transacao in transacoes)
                {
                    compraDAO.DeleteByCartaoTransacaoId(transacao.Id);
                }
            }
        }
    }
}
