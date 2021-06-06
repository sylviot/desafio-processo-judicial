using System.Collections;
using System.Collections.Generic;

namespace api.Models
{
    public class Responsavel : ModelBase
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }

        public ICollection<ProcessoResponsavel> Processos { get; set; }
    }
}
