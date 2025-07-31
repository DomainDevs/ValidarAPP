using Sistran.Company.Application.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO
{
    public class ListRiskPersonQueryDTO
    {
        public List<ListRiskPersonDTO> ListRiskPerson { get; set; }
        public ErrorDTO Error { get; set; }
    }
}
