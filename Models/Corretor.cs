using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class Corretor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        // Relacionamento 1-N com Compra
        public List<Compra> Compras { get; set; }
    }
}
