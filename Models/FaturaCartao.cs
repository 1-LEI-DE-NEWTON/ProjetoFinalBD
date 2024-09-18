using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class FaturaCartao
    {
        public int Id { get; set; }
        public string MesReferencia { get; set; }
        public string AnoReferencia { get; set; }
        public string Valor { get; set; }
        public string? DataPagamento { get; set; }
        public int? CartaoCreditoId { get; set; } // Foreign Key para CartaoCredito
        public CartaoCredito CartaoCredito { get; set; }
        // Relacionamento 1-N com ItensFatura
        public List<ItensFatura> ItensFaturas { get; set; }
        // Relacionamento 1-N com BoletoCustomizado
        public List<BoletoCustomizado> BoletosCustomizados { get; set; }
        // Relacionamento 1-N com Pagamento
        public List<Pagamento> Pagamentos { get; set; }
    }
}
