using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public double Saldo { get; set; }
        public double Taxa { get; set; }
        public string ReservaCol { get; set; }
        
        // Foreign Key para Conta
        public int ContaId { get; set; }
        public Conta Conta { get; set; }

        // Relacionamento 1-N com MovimentacaoReserva
        public List<MovimentacaoReserva> MovimentacoesReserva { get; set; }
    }
}
