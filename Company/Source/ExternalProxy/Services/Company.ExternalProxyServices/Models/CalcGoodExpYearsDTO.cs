using System;
using Sistran.Company.Application.ExternalProxyServices.ExperienciaAseguradoBiztalkService;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [Serializable]
    public class CalcGoodExpYearsDTO
    {
        public HistoricoPolizaCexper HistoryPolicy { get; set; }
        public int PolicyWithSiniester { get; set; }
        public int YearsWithPolicy { get; set; }
    }
}
