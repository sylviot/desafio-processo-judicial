﻿using api.Models.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Interfaces
{
    public interface IResponsavelService
    {
        bool Unique(ResponsavelDto responsavel);
    }
}
