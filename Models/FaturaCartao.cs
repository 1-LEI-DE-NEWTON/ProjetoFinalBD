using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class FaturaCartao
    {
        public int Id { get; set; }
        public string MesReferencia { get; set; }
        public string AnoReferencia { get; set; }
        public double Valor { get; set; }
        public int CartaoCreditoId { get; set; } // Foreign Key para CartaoCredito
    }
}
