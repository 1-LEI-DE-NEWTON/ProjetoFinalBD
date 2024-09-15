using ProjetoFinalBD.DAO;
using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Testes
{
    public class TesteContaDAO
    {
        private readonly ContaDAO contaDAO;
        private readonly ClienteDAO clienteDAO;
        private readonly PessoaDAO pessoaDAO;
        private readonly ReservaDAO reservaDAO;


        public TesteContaDAO()
        {
            string connectionString = "server=localhost;user=root;database=projetofinaldb";
            
            contaDAO = new ContaDAO(connectionString);
            clienteDAO = new ClienteDAO(connectionString);
            pessoaDAO = new PessoaDAO(connectionString);
            reservaDAO = new ReservaDAO(connectionString);
        }        

        public void RunTests()
        {
            var conta = new Conta
            {
                Id = 1,
                Saldo = 1000,
                LimiteNegativo = 100,
                TipoConta = new TipoConta
                {
                    Id = 1,
                    Descricao = "Conta Corrente"
                },
                ClienteId = 1,
                MovimentacoesConta = new List<MovimentacaoConta>
                {
                    new MovimentacaoConta
                    {
                        Id = 1,
                        DataMovimentacao = DateTime.Now,
                        Valor = "0",
                        TipoMovimentacao = "Depósito"
                    }
                }                                

            };

            var cliente = new Cliente
            {
                Id = 1,
                Pessoa = new Pessoa { Id = 1, Nome = "Fulano", Cpf = "12345678900" },
                FatorRisco = "Médio",
                RendaMensal = "5000",
                Contas = new List<Conta> { conta }
            };

            clienteDAO.Insert(cliente); //ATE AQUI INSERE NORMAL

            var idPessoa = pessoaDAO.GetByCpf(cliente.Pessoa.Cpf).Id;

            var clienteInserido = clienteDAO.GetByPessoaId(idPessoa);  //OK

            //Adicionar uma nova conta a um cliente existente

            var conta2 = new Conta
            {
                Id = 2,
                Saldo = 2000,
                LimiteNegativo = 200,
                TipoConta = new TipoConta
                {
                    Id = 2,
                    Descricao = "Conta Poupança"
                },
                ClienteId = clienteInserido.Id,
                MovimentacoesConta = new List<MovimentacaoConta>
                {
                    new MovimentacaoConta
                    {
                        Id = 1,
                        DataMovimentacao = DateTime.Now,
                        Valor = "0",
                        TipoMovimentacao = "Depósito"
                    }
                }
            };

            clienteInserido.Contas.Add(conta2);

            contaDAO.Insert(conta2);

            //Atualizar a conta

            clienteInserido = clienteDAO.GetByPessoaId(idPessoa); //nao puxa movConta

            clienteDAO.Update(clienteInserido);

            //Cria reservas para conta 1

            var reserva1 = new Reserva
            {
                Id = 1,
                Saldo = 100,
                Taxa = 0.1,
                ReservaCol = "Reserva 1",
                ContaId = clienteInserido.Contas[0].Id,
                MovimentacoesReserva = new List<MovimentacaoReserva>
                {
                    new MovimentacaoReserva
                    {
                        Id = 1,
                        DataMovimentacao = DateTime.Now,
                        Valor = 0,
                        TipoMovimentacao = "Reserva",
                        ReservaId = 1
                    }
                }
            };

            //Insere a reserva para conta 1 // OK
            reservaDAO.Insert(reserva1);

            var reservaInserida = reservaDAO.GetbyContaId(clienteInserido.Contas[0].Id);

            //Deleta o cliente e suas contas
            clienteDAO.Delete(clienteInserido.Id);
            

        }
    }
}
