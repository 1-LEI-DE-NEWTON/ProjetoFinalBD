using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataPagamento { get; set; }
        public int FaturaCartaoId { get; set; } // Foreign Key para FaturaCartao
        public string ValorParcial { get; set; }
        public int BoletoCustomizadoId { get; set; } // Foreign Key para BoletoCustomizado
    }
}
