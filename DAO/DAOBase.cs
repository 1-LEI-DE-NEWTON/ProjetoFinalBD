using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalBD.Models;

namespace ProjetoFinalBD.DAO
{
    public abstract class DAOBase
    {
        private readonly string connectionString;

        public DAOBase(string connectionString)
        {
            this.connectionString = connectionString;
        }
        
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        protected void ExecuteNonQuery(string query)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }               
    }
}
