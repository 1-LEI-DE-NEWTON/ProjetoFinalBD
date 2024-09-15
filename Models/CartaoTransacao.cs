using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class CartaoTransacao
    {
        public int Id { get; set; }
        public string NumeroCartao { get; set; }
        public string Cvc { get; set; }
        public int CartaoId { get; set; } // Foreign Key para CartaoCredito
        public CartaoCredito CartaoCredito { get; set; }
        public string TipoCartao { get; set; } // fisico/virtual
        public string NomeCartao { get; set; }
        public string TipoTransacao { get; set; } // debito/credito
        public bool IsInternacional { get; set; }
        public int? BandeiraCartaoId { get; set; } // Foreign Key para BandeiraCartao
        public BandeiraCartao BandeiraCartao { get; set; }
    }
}
