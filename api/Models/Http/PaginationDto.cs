using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Http
{
    public class PaginationDto
    {
        public uint Size { get; set; } = 2;
        public uint Page { get; set; } = 1;
    }
}
