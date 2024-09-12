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


        public TesteContaDAO()
        {
            string connectionString = "server=localhost;user=root;database=projetofinaldb";
            
            contaDAO = new ContaDAO(connectionString);
            clienteDAO = new ClienteDAO(connectionString);
            pessoaDAO = new PessoaDAO(connectionString);
        }

        //atribuir uma nova conta a um cliente ja existente

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
            };

            var cliente = new Cliente
            {
                Id = 1,
                Pessoa = new Pessoa { Id = 1, Nome = "Fulano", Cpf = "12345678900" },
                FatorRisco = "Médio",
                RendaMensal = "5000",
                Contas = new List<Conta> { conta }
            };

            clienteDAO.Insert(cliente);

            var idPessoa = pessoaDAO.GetByCpf(cliente.Pessoa.Cpf).Id;

            var clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

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
                ClienteId = clienteInserido.Id
            };

            clienteInserido.Contas.Add(conta2);

            contaDAO.Insert(conta2);

            //Atualizar a conta

            clienteInserido = clienteDAO.GetByPessoaId(idPessoa);

            clienteDAO.Update(clienteInserido);

            //Deletar conta



        }
    }
}
