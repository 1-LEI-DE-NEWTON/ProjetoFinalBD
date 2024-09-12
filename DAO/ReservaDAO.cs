using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public class ReservaDAO : DAOBase
    {
        public ReservaDAO(string connectionString) : base(connectionString) { }

        public void Insert(Reserva reserva)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                var transaction = connection.BeginTransaction();
                
                try
                {
                    //Inserir reserva

                    string query = "INSERT INTO Reserva (Id, Saldo, Taxa, ReservaCol, ContaId) " +
                            "VALUES (@Id, @Saldo, @Taxa, @ReservaCol, @ContaId)";
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.AddWithValue("Id", reserva.Id);
                        command.Parameters.AddWithValue("Saldo", reserva.Saldo);
                        command.Parameters.AddWithValue("Taxa", reserva.Taxa);
                        command.Parameters.AddWithValue("ReservaCol", reserva.ReservaCol);
                        command.Parameters.AddWithValue("ContaId", reserva.Conta.Id);

                        command.ExecuteNonQuery();
                    }

                    //Inserir movimentações
                    foreach (var movimentacaoReserva in reserva.MovimentacoesReserva)
                    {
                        //Criar string para query                        
                        string insertMovimentacaoQuery = "INSERT INTO movimentacaoReserva " +
                            "(Id, DataMovimentacao, TipoMovimentacao, Valor, ReservaId) VALUES " +
                            "(@Id, @DataMovimentacao, @TipoMovimentacao, @Valor, @ReservaId)";

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = insertMovimentacaoQuery;
                            command.Parameters.AddWithValue("Id", movimentacaoReserva.Id);
                            command.Parameters.AddWithValue("DataMovimentacao", movimentacaoReserva.DataMovimentacao);
                            command.Parameters.AddWithValue("TipoMovimentacao", movimentacaoReserva.TipoMovimentacao);
                            command.Parameters.AddWithValue("Valor", movimentacaoReserva.Valor);
                            command.Parameters.AddWithValue("ReservaId", reserva.Id);

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
