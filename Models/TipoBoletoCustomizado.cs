using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class TipoBoletoCustomizado
    {
        public int Id { get; set; }
        public string Descricao { get; set; } //CobrançaCustomizada ; BoletoExterno ; FaturaCartao
        // Relacionamento 1-N com BoletoCustomizado
        public List<BoletoCustomizado> BoletosCustomizados { get; set; }
    }
}
