using ProjetoFinalBD.DAO;
using ProjetoFinalBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Testes
{
    public class TestePessoaDAO
    {
        private readonly PessoaDAO _pessoaDAO;
        
        public TestePessoaDAO()
        {
            // Aqui você coloca sua connection string para acessar o banco de dados
            string connectionString = "server=localhost;user=root;database=projetofinaldb";
            _pessoaDAO = new PessoaDAO(connectionString);
        }

        public void RunTests()
        {
            Console.WriteLine("---- TESTANDO PessoaDAO ----");

            // 1. Teste de Inserção
            Pessoa novaPessoa = new Pessoa
            {
                Nome = "João Silva",
                Cpf = "1234567890"
            };
            _pessoaDAO.Insert(novaPessoa);
            Console.WriteLine("Inserção de Pessoa: OK");

            // 2. Teste de Atualização
            novaPessoa.Nome = "João da Silva";            
            _pessoaDAO.Update(novaPessoa);
            Console.WriteLine("Atualização de Pessoa: OK");


            // 3. Teste de Exclusão
            //_pessoaDAO.Delete(_pessoaDAO.GetByCpf(novaPessoa.Cpf).Id);
            //Console.WriteLine("Exclusão de Pessoa: OK");

            //Console.WriteLine("---- TESTES FINALIZADOS ----");
        }
    }
}
