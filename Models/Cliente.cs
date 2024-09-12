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
        
        // Foreign Key para Pessoa
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        // Relacionamento 1-N com Conta
        public List<Conta> Contas { get; set; }
    }
}
