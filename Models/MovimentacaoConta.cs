using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class MovimentacaoConta
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string TipoMovimentacao { get; set; } // entrada/saida
        
        // Foreign Key para Conta
        public int ContaId { get; set; }
        public Conta Conta { get; set; }
    }
}
