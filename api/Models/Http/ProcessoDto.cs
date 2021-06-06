using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Http
{
    public class ProcessoDto
    {
        public int Id { get; set; }
        public int? ProcessoPai { get; set; }
        public string NumeroUnificado { get; set; }
        public DateTime DataDistribuicao { get; set; }
        public bool SegredoJustica { get; set; }
        public string PastaFisicaCliente { get; set; }
        public string Descricao { get; set; }
        public int Situacao { get; set; }

        public ResponsavelDto[] Responsaveis { get; set; }
    }
}
