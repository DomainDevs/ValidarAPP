using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    public class ResponseInfringement
    {
        public List<ExternalInfrigement> ExternalInfrigements { get; set; }

        public Error Error { get; set; }

    }
}
