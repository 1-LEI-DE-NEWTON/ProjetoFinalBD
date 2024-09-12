﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalBD.Models
{
    public class BandeiraCartao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        // Relacionamento 1-N com CartaoTransacao
        public List<CartaoTransacao> CartoesTransacao { get; set; }
    }
}
