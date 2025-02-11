﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public string ValorTotal { get; set; }
        public DateTime DataPagamento { get; set; }
        public int? FaturaCartaoId { get; set; } // Foreign Key para FaturaCartao
        public FaturaCartao FaturaCartao { get; set; }
        public string ValorParcial { get; set; }
        public int? BoletoCustomizadoId { get; set; } // Foreign Key para BoletoCustomizado
        public BoletoCustomizado BoletoCustomizado { get; set; }
    }
}
