using System;

namespace api.Models
{
    public class Processo : ModelBase
    {
        public string NumeroUnificado { get; set; }
        public DateTime DataDistribuicao { get; set; }
        public bool SegredoJustica { get; set; }
        public string PastaFisicaCliente { get; set; }
        public string Descricao { get; set; }
    }
}
