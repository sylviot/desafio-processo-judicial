using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Http
{
    public class ProcessoFilterDto : PaginationDto
    {
        public string NumeroUnificado { get; set; }
        public DateTime? DataDistribuicaoInicio { get; set; }
        public DateTime? DataDistribuicaoFim { get; set; }
        public int? SituacaoId { get; set; }
        public bool? SegredoJustica { get; set; }
        public string PastaFisicaCliente { get; set; }
        public string Responsavel { get; set; }
    }
}
