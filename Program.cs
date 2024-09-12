using MySql.Data.MySqlClient;
using ProjetoFinalBD.DAO;
using ProjetoFinalBD.Testes;


class Program
{    
    static void Main(string[] args)
    {
        string connectionString = "server=localhost;user=root;database=projetofinaldb";
        
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso!");
                // Aqui podemos iniciar as operações de banco de dados



                #region Testes
                #region Testes Pessoa
                //TestePessoaDAO pessoaDAOTeste = new TestePessoaDAO();
                //pessoaDAOTeste.RunTests();
                #endregion Testes Pessoa

                #region Testes Cliente
                //TesteClienteDAO clienteDAOTeste = new TesteClienteDAO();
                //clienteDAOTeste.RunTests();
                #endregion

                #region Testes Conta
                TesteContaDAO contaDAOTeste = new TesteContaDAO();
                contaDAOTeste.RunTests();
                #endregion

                #endregion Testes


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }
    }
}