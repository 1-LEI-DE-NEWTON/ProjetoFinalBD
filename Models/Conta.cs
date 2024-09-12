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
        public int? ClienteId { get; set; } // Foreign Key para Cliente
        public int? TipoContaId { get; set; } // Foreign Key para TipoConta
    }
}
