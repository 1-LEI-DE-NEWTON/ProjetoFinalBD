using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class MovimentacaoCartao
    {
        public int Id { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public double Valor { get; set; }
        public int? CartaoTransacaoId { get; set; } // Foreign Key para CartaoTransacao
        public CartaoTransacao CartaoTransacao { get; set; }
        public string TipoMovimentacao { get; set; } // entrada/saida
    }
}
