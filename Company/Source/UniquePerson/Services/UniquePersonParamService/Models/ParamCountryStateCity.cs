using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    public class ParamCountryStateCity
    {
        public int CityCd { get; set; }
        public string CityDescription { get; set; }
        public int CountryCd { get; set; }
        public string CountryDescription { get; set; }
        public int StateCd { get; set; }
        public string StateDescription { get; set; }
    }
}
