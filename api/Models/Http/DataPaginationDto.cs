using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Http
{
    public class DataPaginationDto
    {
        public dynamic data { get; set; }
        public uint page { get; set; }
        public uint size { get; set; }
        public int? next { get; set; }
        public int? previous { get; set; }
    }
}
