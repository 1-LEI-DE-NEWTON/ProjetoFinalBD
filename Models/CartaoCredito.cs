using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class CartaoCredito
    {
        public int Id { get; set; }
        public string DataFechamento { get; set; }
        public int? ContaId { get; set; } // Foreign Key para Conta
        public int? CategoriaCartaoId { get; set; } // Foreign Key para CategoriaCartao
        public double LimiteCredito { get; set; }
    }
}
