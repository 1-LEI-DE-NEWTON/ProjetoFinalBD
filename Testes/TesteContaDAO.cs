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
        private readonly CartaoCreditoDAO cartaoCreditoDAO;

        public TesteContaDAO()
        {
            string connectionString = "server=localhost;user=root;database=projetofinaldb";
            
            contaDAO = new ContaDAO(connectionString);
            clienteDAO = new ClienteDAO(connectionString);
            pessoaDAO = new PessoaDAO(connectionString);
            reservaDAO = new ReservaDAO(connectionString);
            cartaoCreditoDAO = new CartaoCreditoDAO(connectionString);
        }  
        
public Cliente GerarClienteAleatorio()
    {
        // Listas de valores predefinidos
        List<string> nomes = new List<string> { "João", "Maria", "Pedro", 
            "Ana", "Carlos", "Beatriz", "Paulo", "Fernanda", "Lucas", "Juliana" };
        List<string> cpfs = new List<string> { "12345678900", "09876543211", 
            "11223344556", "66778899000", "11122233344", "55566677788", "99988877766", "44455566677", "33322211100", "00011122233" };
        List<string> descricoesTipoConta = new List<string> { "Conta Corrente", "Conta Poupança", "Conta Salário" };
        List<string> tiposMovimentacao = new List<string> { "Depósito", "Saque", "Transferência" };
        List<string> fatorRisco = new List<string> { "Baixo", "Médio", "Alto" };

        Random random = new Random();

        // Gerar valores aleatórios
        string nomeAleatorio = nomes[random.Next(nomes.Count)];
        string cpfAleatorio = cpfs[random.Next(cpfs.Count)];
        string descricaoTipoContaAleatoria = descricoesTipoConta[random.Next(descricoesTipoConta.Count)];
        string tipoMovimentacaoAleatoria = tiposMovimentacao[random.Next(tiposMovimentacao.Count)];
        double saldoAleatorio = random.Next(0, 10000);
        double limiteNegativoAleatorio = random.Next(0, 1000);
        double valorMovimentacaoAleatoria = random.Next(0, 5000);
        string fatorRiscoAleatorio = fatorRisco[random.Next(fatorRisco.Count)];

        // Criar objeto Cliente
        var cliente = new Cliente
        {
            Id = random.Next(1, 1000),
            FatorRisco = fatorRiscoAleatorio,
            RendaMensal = random.Next(1000, 10000).ToString(),
            Pessoa = new Pessoa
            {
                Id = random.Next(1, 1000),
                Nome = nomeAleatorio,
                Cpf = cpfAleatorio
            },
            Contas = new List<Conta>
            {
                new Conta
                {
                    Id = random.Next(1, 1000),
                    Saldo = saldoAleatorio,
                    LimiteNegativo = limiteNegativoAleatorio,
                    TipoConta = new TipoConta
                    {
                        Id = random.Next(1, 10),
                        Descricao = descricaoTipoContaAleatoria
                    },
                    ClienteId = random.Next(1, 1000),
                    MovimentacoesConta = new List<MovimentacaoConta>
                    {
                        new MovimentacaoConta
                        {
                            Id = random.Next(1, 1000),
                            DataMovimentacao = DateTime.Now,
                            Valor = valorMovimentacaoAleatoria.ToString(),
                            TipoMovimentacao = tipoMovimentacaoAleatoria
                        }
                    }
                }
            }
        };
        return cliente;
    }

        public void RunTests()
        {
            //Gera um nome de pessoa aleatoriamente            

            // var conta = new Conta
            // {
            //     Id = 1,
            //     Saldo = 1000,
            //     LimiteNegativo = 100,
            //     TipoConta = new TipoConta
            //     {
            //         Id = 1,
            //         Descricao = "Conta Corrente"
            //     },
            //     ClienteId = 1,
            //     MovimentacoesConta = new List<MovimentacaoConta>
            //     {
            //         new MovimentacaoConta
            //         {
            //             Id = 1,
            //             DataMovimentacao = DateTime.Now,
            //             Valor = "0",
            //             TipoMovimentacao = "Depósito"
            //         }
            //     }                                

            // };

            // var cliente = new Cliente
            // {
            //     Id = 1,
            //     Pessoa = new Pessoa { Id = 1, Nome = "Fulano", Cpf = "12345678900" },
            //     FatorRisco = "Médio",
            //     RendaMensal = "5000",
            //     Contas = new List<Conta> { conta }
            // };

            var cliente = GerarClienteAleatorio();

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
            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

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

            var reservaInserida = reservaDAO.GetReservasByContaId(clienteInserido.Contas[0].Id);

            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            //Cria um cartao de credito
             var cartaoCredito = new CartaoCredito
             {
                 Id = 1,
                 DataFechamento = "10",
                 ContaId = clienteInserido.Contas[0].Id,
                 CategoriaCartao = new CategoriaCartao { Id = 1, Descricao = "Gold" },
                 LimiteCredito = 1000
             };

            //Insere o cartao de credito
            cartaoCreditoDAO.Insert(cartaoCredito);

            // //Deleta o cliente e suas contas
            // clienteDAO.Delete(clienteInserido.Id);            
        }
    }
}
