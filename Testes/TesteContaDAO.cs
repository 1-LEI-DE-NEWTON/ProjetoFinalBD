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
            List<string> fatorRisco = new List<string> { "Baixo", "Médio", "Alto" };

            Random random = new Random();

            // Gerar valores aleatórios para o cliente
            string nomeAleatorio = nomes[random.Next(nomes.Count)];
            string cpfAleatorio = random.Next(100000000, 999999999).ToString();
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
        public Compra GerarCompraAleatoria(Random random)
        {            
            // Listas de valores predefinidos
            List<string> credores = new List<string> { "Credor 1", "Credor 2", "Credor 3" };
            List<string> tiposMovimentacao = new List<string> { "Depósito", "Saque", "Transferência" };
            List<string> nomesCorretor = new List<string> { "Corretor 1", "Corretor 2", "Corretor 3" };

            // Gerar valores aleatórios para a compra
            string credorAleatorio = credores[random.Next(credores.Count)];
            string tipoMovimentacaoAleatoria = tiposMovimentacao[random.Next(tiposMovimentacao.Count)];
            double valorAleatorio = random.Next(0, 10000);
            double taxaParcelamentoAleatoria = random.Next(0, 1000);
            int quantidadeParcelasAleatoria = random.Next(0, 1000);

            // Criar objeto Compra
            var compra = new Compra
            {
                Id = random.Next(1, 1000),
                Valor = valorAleatorio,
                QuantidadeParcelas = quantidadeParcelasAleatoria,
                TaxaParcelamento = taxaParcelamentoAleatoria,
                Credor = credorAleatorio,
                CorretorId = random.Next(1, 1000),
                CartaoTransacaoId = random.Next(1, 1000),
                DataCompra = DateTime.Now,
                Corretor = new Corretor
                {
                    Id = random.Next(1, 1000),
                    Nome = nomesCorretor[random.Next(nomesCorretor.Count)]
                }
            };
            return compra;
        }
        
        public CartaoTransacao GerarTransacaoAleatoria(Random random)
        {
            // Listas de valores predefinidos
            List<string> tiposTransacao = new List<string> { "Debito", "Credito", "Estorno" };
            List<string> tiposCartao = new List<string> { "Físico", "Virtual", "Temporário", "Pré-pago" };
            List<string> isInternacional = new List<string> { "Sim", "Não" };
            List<string> bandeiras = new List<string> { "Visa", "Mastercard", "Elo", "American Express" };

            // Gerar valores aleatorios
            string tipoTransacaoAleatoria = tiposTransacao[random.Next(tiposTransacao.Count)];
            string tipoCartaoAleatorio = tiposCartao[random.Next(tiposCartao.Count)];
            bool isInternacionalAleatorio = isInternacional[random.Next(isInternacional.Count)] == "Sim" ? true : false;
            string nomeCartao = "Cartão " + random.Next(1, 1000).ToString();

            //Criar objeto CartaoTransacao
            var cartaoTransacao = new CartaoTransacao
            {
                Id = random.Next(1, 1000),
                NumeroCartao = random.Next(100000000, 999999999).ToString(),
                NomeCartao = nomeCartao,
                Cvc = random.Next(100, 999).ToString(),
                TipoTransacao = tipoTransacaoAleatoria,
                TipoCartao = tipoCartaoAleatorio,
                IsInternacional = isInternacionalAleatorio,
                CartaoId = random.Next(1, 1000),
                MovimentacoesCartao = new List<MovimentacaoCartao>
                {
                    new MovimentacaoCartao
                    {
                        Id = random.Next(1, 1000),
                        DataMovimentacao = DateTime.Now,
                        Valor = random.Next(0, 1000),
                        TipoMovimentacao = tipoTransacaoAleatoria
                    }
                },
                BandeiraCartao = new BandeiraCartao
                {
                    Id = random.Next(1, 1000),
                    Descricao = bandeiras[random.Next(bandeiras.Count)]
                },
                Compras = new List<Compra>
                {
                    GerarCompraAleatoria(random)
                }
            };
            return cartaoTransacao;
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
                LimiteCredito = limiteCreditoAleatorio,

                FaturasCartao = new List<FaturaCartao>
                {
                   GerarFaturaCartaoAleatoria(random)
                },

                //Adiciona as trasações do cartao                
                CartaoTransacoes = new List<CartaoTransacao>
                {
                    GerarTransacaoAleatoria(random)
                }
            };
            return cartaoCredito;
        }
        public FaturaCartao GerarFaturaCartaoAleatoria(Random random)
        {
            // Listas de valores predefinidos
            List<string> meses = new List<string> { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho",
                "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            // Gerar valores aleatórios para a fatura
            string mesReferenciaAleatorio = meses[random.Next(meses.Count)];
            string anoReferenciaAleatorio = random.Next(2010, 2021).ToString();
            double valorAleatorio = random.Next(0, 1000);

            // Criar objeto FaturaCartao
            var faturaCartao = new FaturaCartao
            {
                Id = random.Next(1, 1000),
                MesReferencia = mesReferenciaAleatorio,
                AnoReferencia = anoReferenciaAleatorio,
                Valor = valorAleatorio.ToString(),
                CartaoCreditoId = random.Next(1, 1000),
                DataPagamento = DateTime.Now.ToString(),
                ItensFaturas = new List<ItensFatura>
                {
                    new ItensFatura
                    {
                        Id = random.Next(1, 1000),
                        Descricao = "Item " + random.Next(1, 1000).ToString(),                        
                        FaturaCartaoId = random.Next(1, 1000)
                    }
                },
                BoletosCustomizados = new List<BoletoCustomizado>
                {
                    GerarBoletoCustomizadoAleatorio(random)                    
                },
                Pagamentos = new List<Pagamento>
                {
                    GerarPagamentoAleatorio(random)                    
                }
            };
            return faturaCartao;
        }
        public BoletoCustomizado GerarBoletoCustomizadoAleatorio(Random random)
        {
            var boletoCustomizado = new BoletoCustomizado
            {
                Id = random.Next(1, 1000),
                DataVencimento = DateTime.Now,
                DataGeracao = DateTime.Now,
                CodigoBarras = random.Next(100000000, 999999999).ToString(),
                Valor = random.Next(1, 1000).ToString(),
                TipoBoletoCustomizado = new TipoBoletoCustomizado
                {
                    Id = random.Next(1, 1000),
                    Descricao = "Boleto " + random.Next(1, 1000).ToString()
                },
                TipoBoletoCustomizadoId = random.Next(1, 1000),
                FaturaCartaoId = random.Next(1, 1000),
                Pagamentos = new List<Pagamento>
                {
                    GerarPagamentoAleatorio(random)
                }
            };
            return boletoCustomizado;
        }

        public Pagamento GerarPagamentoAleatorio(Random random)
        {
            var pagamento = new Pagamento
            {
                Id = random.Next(1, 1000),
                DataPagamento = DateTime.Now,
                ValorTotal = random.Next(1, 1000).ToString(),
                ValorParcial = random.Next(1, 900).ToString(),                
                FaturaCartaoId = random.Next(1, 1000)
            };
            return pagamento;
        }

        public void RunTests()
        {

            var cliente = GerarClienteAleatorio();

            clienteDAO.Insert(cliente); //ATE AQUI INSERE NORMAL

            var idPessoa = pessoaDAO.GetByCpf(cliente.Pessoa.Cpf).Id;

            var clienteInserido = clienteDAO.GetByPessoaId(idPessoa);  //OK

            //Adicionar uma nova conta a um cliente existente
            var conta2 = GerarContaAleatoria(new Random());

            conta2.ClienteId = clienteInserido.Id;

            contaDAO.Insert(conta2);

            //Atualizar a conta
            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            clienteDAO.Update(clienteInserido); //FATAL ERROR

            //Cria reservas para conta 0 (Conta 1)
            var reserva1 = GerarReservaAleatoria(new Random());

            reserva1.ContaId = clienteInserido.Contas[0].Id;

            //Insere a reserva para conta 1 // OK
            reservaDAO.Insert(reserva1);

            var reservaInserida = reservaDAO.GetReservasByContaId(clienteInserido.Contas[0].Id);

            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            //Cria um cartao de credito para conta 0 (Conta 1)
             var cartaoCredito = GerarCartaoCreditoAleatorio(new Random());
            cartaoCredito.ContaId = clienteInserido.Contas[0].Id;                        

            //Insere o cartao de credito para conta 1
            cartaoCreditoDAO.Insert(cartaoCredito);         
            //Obtem o cliente atualizado
            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            //Deleta o cliente e suas contas
            //clienteDAO.Delete(clienteInserido.Id);

            // OK!!
        }

        public void DeleteClienteTeste(int ClienteId)
        {
            clienteDAO.Delete(ClienteId);
        }        
    }
}
