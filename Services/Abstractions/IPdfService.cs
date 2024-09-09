﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstractions
{
    public interface IPdfService
    {
        Task<byte[]> GenerateReportPdf(GetReportDto report);
    }
}
