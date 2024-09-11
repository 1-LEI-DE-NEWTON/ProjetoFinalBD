using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string FatorRisco { get; set; }
        public string RendaMensal { get; set; }
        public int PessoaId { get; set; } // Foreign Key para Pessoa
    }
}
