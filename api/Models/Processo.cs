using System;
using System.Collections.Generic;

namespace api.Models
{
    public class Processo : ModelBase
    {
        public int? ProcessoPai { get; set; }
        public string NumeroUnificado { get; set; }
        public DateTime DataDistribuicao { get; set; }
        public bool SegredoJustica { get; set; }
        public string PastaFisicaCliente { get; set; }
        public string Descricao { get; set; }
        public int SituacaoId { get; set; }

        public ICollection<ProcessoResponsavel> Responsaveis { get; set; }
        public virtual Situacao Situacao { get; set; }
    }
}
