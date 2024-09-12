using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class ItensFatura
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int? FaturaCartaoId { get; set; } // Foreign Key para FaturaCartao
    }
}
