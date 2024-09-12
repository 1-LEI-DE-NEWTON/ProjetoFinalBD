using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public double Valor { get; set; }
        public int QuantidadeParcelas { get; set; }
        public double TaxaParcelamento { get; set; }
        public string Credor { get; set; }
        public int? CorretorId { get; set; } // Foreign Key para Corretor
        public int? CartaoTransacaoId { get; set; } // Foreign Key para CartaoTransacao
        public DateTime DataCompra { get; set; }
    }
}
