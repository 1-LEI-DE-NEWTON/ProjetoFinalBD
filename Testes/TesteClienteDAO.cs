using ProjetoFinalBD.DAO;
using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Testes
{
    public class TesteClienteDAO
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly PessoaDAO _pessoaDAO;

        public TesteClienteDAO()
        {
            string connectionString = "server=localhost;user=root;database=projetofinaldb";
            _pessoaDAO = new PessoaDAO(connectionString);
            _clienteDAO = new ClienteDAO(connectionString);            
        }

        public void RunTests()
        {
            Console.WriteLine("Teste de inserção de cliente");
            var pessoa = new Pessoa
            {
                Nome = "João",
                Cpf = "12345678902"
            };
            var cliente = new Cliente
            {
                Pessoa = pessoa,
                FatorRisco = "Baixo",
                RendaMensal = "1000"
            };

            _clienteDAO.Insert(cliente);            

            var idPessoa = _pessoaDAO.GetByCpf(pessoa.Cpf).Id;

            var clienteInserido = _clienteDAO.GetByPessoaId(idPessoa);

            if (clienteInserido == null || clienteInserido.Pessoa.Cpf != pessoa.Cpf)
            {
                throw new Exception("Insert test failed");
            }

            Console.WriteLine("Teste de inserção de cliente passou");
            
            Console.WriteLine("Teste de atualização de cliente");                          

            clienteInserido.FatorRisco = "Médio";
            clienteInserido.RendaMensal = "5000";
            _clienteDAO.Update(clienteInserido);

            var clienteAtualizado = _clienteDAO.GetByPessoaId(idPessoa);

            if (clienteAtualizado.FatorRisco != "Médio" || clienteAtualizado.RendaMensal != "5000")
            {
                throw new Exception("Update test failed");
            }

            Console.WriteLine("Teste de atualização de cliente passou");

            Console.WriteLine("Teste de exclusão de cliente");

            _clienteDAO.Delete(clienteAtualizado.Id);

            _pessoaDAO.Delete(idPessoa);

            var clienteExcluido = _clienteDAO.GetByPessoaId(idPessoa);

            if (clienteExcluido != null)
            {
                throw new Exception("Delete test failed");
            }

            Console.WriteLine("Teste de exclusão de cliente passou");            
        }
    }
}
