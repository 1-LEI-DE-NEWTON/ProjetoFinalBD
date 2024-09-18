using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class BoletoCustomizado
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataGeracao { get; set; }
        public string CodigoBarras { get; set; }
        public int TipoBoletoCustomizadoId { get; set; }  // Chave estrangeira 
        public TipoBoletoCustomizado TipoBoletoCustomizado { get; set; }
        public int? FaturaCartaoId { get; set; }  // Chave estrangeira 
        public FaturaCartao FaturaCartao { get; set; }        
        public List<Pagamento> Pagamentos { get; set; } // Relacionamento 1-N com Pagamento
    }
}
