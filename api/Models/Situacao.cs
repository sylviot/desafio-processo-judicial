using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Situacao : ModelBase
    {
        public string Nome { get; set; }
        public bool Finalizado { get; set; }
    }
}
