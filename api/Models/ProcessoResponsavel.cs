using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ProcessoResponsavel
    {
        public int ProcessoId { get; set; }
        public int ResponsavelId { get; set; }

        public Processo Processo { get; set; }
        public Responsavel Responsavel { get; set; }
    }
}
