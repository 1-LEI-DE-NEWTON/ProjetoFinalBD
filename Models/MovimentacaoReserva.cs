using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class MovimentacaoReserva
    {
        public int Id { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public double Valor { get; set; }
        public string TipoMovimentacao { get; set; } // entrada/saida
        public int? ReservaId { get; set; } // Foreign Key para Reserva
    }
}
