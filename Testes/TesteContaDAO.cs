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
            List<string> nomes = new List<string> { "João", "Maria", "Pedro", "Ana", "Carlos", "Beatriz", "Paulo", "Fernanda", "Lucas", "Juliana" };
            List<string> cpfs = new List<string> { "12345678900", "09876543211", "11223344556", "66778899000", "11122233344", "55566677788", "99988877766", "44455566677", "33322211100", "00011122233" };
            List<string> fatorRisco = new List<string> { "Baixo", "Médio", "Alto" };

            Random random = new Random();

            // Gerar valores aleatórios para o cliente
            string nomeAleatorio = nomes[random.Next(nomes.Count)];
            string cpfAleatorio = cpfs[random.Next(cpfs.Count)];
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
                    GerarContaAleatoria(random)
                }
            };
            return cliente;
        }

        public Conta GerarContaAleatoria(Random random)
        {
            // Listas de valores predefinidos
            List<string> descricoesTipoConta = new List<string> { "Conta Corrente", "Conta Poupança", "Conta Salário" };
            List<string> tiposMovimentacao = new List<string> { "Depósito", "Saque", "Transferência" };

            // Gerar valores aleatórios para a conta
            string descricaoTipoContaAleatoria = descricoesTipoConta[random.Next(descricoesTipoConta.Count)];
            string tipoMovimentacaoAleatoria = tiposMovimentacao[random.Next(tiposMovimentacao.Count)];
            double saldoAleatorio = random.Next(0, 10000);
            double limiteNegativoAleatorio = random.Next(0, 1000);
            double valorMovimentacaoAleatoria = random.Next(0, 5000);

            // Criar objeto Conta
            var conta = new Conta
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
                },

                Reservas = new List<Reserva>
                {
                    GerarReservaAleatoria(random)
                }
            };
            return conta;
        }

        public Reserva GerarReservaAleatoria(Random random)
        {
            // Listas de valores predefinidos
            List<string> reservas = new List<string> { "Reserva 1", "Reserva 2", "Reserva 3" };
            List<string> tiposMovimentacao = new List<string> { "Depósito", "Saque", "Transferência" };

            // Gerar valores aleatórios para a reserva
            string reservaAleatoria = reservas[random.Next(reservas.Count)];
            string tipoMovimentacaoAleatoria = tiposMovimentacao[random.Next(tiposMovimentacao.Count)];
            double saldoAleatorio = random.Next(0, 10000);
            double taxaAleatoria = random.Next(0, 1000);
            double valorMovimentacaoAleatoria = random.Next(0, 5000);

            // Criar objeto Reserva
            var reserva = new Reserva
            {
                Id = random.Next(1, 1000),
                Saldo = saldoAleatorio,
                Taxa = taxaAleatoria,
                ReservaCol = reservaAleatoria,
                ContaId = random.Next(1, 1000),
                MovimentacoesReserva = new List<MovimentacaoReserva>
                {
                    new MovimentacaoReserva
                    {
                        Id = random.Next(1, 1000),
                        DataMovimentacao = DateTime.Now,
                        Valor = valorMovimentacaoAleatoria,
                        TipoMovimentacao = tipoMovimentacaoAleatoria
                    }
                }
            };
            return reserva;
        }

        public CartaoCredito GerarCartaoCreditoAleatorio(Random random)
        {
            // Listas de valores predefinidos
            List<string> categoriasCartao = new List<string> { "Gold", "Platinum", "Black", "Silver", "Standard", "Enterprise" };

            // Gerar valores aleatórios para o cartão de crédito
            string categoriaCartaoAleatoria = categoriasCartao[random.Next(categoriasCartao.Count)];
            string dataFechamentoAleatoria = random.Next(1, 28).ToString();
            double limiteCreditoAleatorio = random.Next(0, 10000);

            // Criar objeto CartaoCredito
            var cartaoCredito = new CartaoCredito
            {
                Id = random.Next(1, 1000),
                DataFechamento = dataFechamentoAleatoria,
                ContaId = random.Next(1, 1000),
                CategoriaCartao = new CategoriaCartao
                {
                    Id = random.Next(1, 10),
                    Descricao = categoriaCartaoAleatoria
                },
                LimiteCredito = limiteCreditoAleatorio
            };
            return cartaoCredito;
        }

        public void RunTests()
        {

            var cliente = GerarClienteAleatorio();

            clienteDAO.Insert(cliente); //ATE AQUI INSERE NORMAL

            var idPessoa = pessoaDAO.GetByCpf(cliente.Pessoa.Cpf).Id;

            var clienteInserido = clienteDAO.GetByPessoaId(idPessoa);  //OK

            //Adicionar uma nova conta a um cliente existente
            var conta2 = GerarContaAleatoria(new Random());

            clienteInserido.Contas.Add(conta2);

            contaDAO.Insert(conta2);

            //Atualizar a conta
            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            clienteDAO.Update(clienteInserido);

            //Cria reservas para conta 1
            var reserva1 = GerarReservaAleatoria(new Random());

            //Insere a reserva para conta 1 // OK
            reservaDAO.Insert(reserva1);

            var reservaInserida = reservaDAO.GetReservasByContaId(clienteInserido.Contas[0].Id);

            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            //Cria um cartao de credito
             var cartaoCredito = GerarCartaoCreditoAleatorio(new Random());

            //Insere o cartao de credito
            cartaoCreditoDAO.Insert(cartaoCredito);

            //Obtem o cliente atualizado
            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            // //Deleta o cliente e suas contas
            // clienteDAO.Delete(clienteInserido.Id);            
        }
    }
}
