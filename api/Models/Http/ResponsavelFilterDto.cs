using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Http
{
    public class ResponsavelFilterDto : PaginationDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string NumeroProcesso { get; set; }
    }
}
