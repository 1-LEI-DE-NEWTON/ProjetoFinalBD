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
        public Conta conta { get; set; }
        public int? CategoriaCartaoId { get; set; } // Foreign Key para CategoriaCartao
        public CategoriaCartao CategoriaCartao { get; set; }
        public double LimiteCredito { get; set; }
        // Relacionamento 1-N com FaturaCartao
        public List<FaturaCartao> FaturasCartao { get; set; }
        
        //Relacionamento 1-N com cartaoTransacao
        public List<CartaoTransacao> CartaoTransacoes { get; set; }
    }
}
