using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public double Saldo { get; set; }
        public double LimiteNegativo { get; set; }
        // Foreign Key para Cliente
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // Foreign Key para TipoConta
        public int? TipoContaId { get; set; }
        public TipoConta TipoConta { get; set; }

        // Relacionamento 1-N com MovimentacaoConta
        public List<MovimentacaoConta> MovimentacoesConta { get; set; }

        // Relacionamento 1-N com Reserva
        public List<Reserva> Reservas { get; set; }

        // Relacionamento 1-N com CartaoCredito
        public List<CartaoCredito> CartoesCredito { get; set; }
    }
}
