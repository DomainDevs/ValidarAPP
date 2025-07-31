using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    public class MinimumSalaryDTO
    {
        [DataMember]
        public int year { get; set; }

        [DataMember]
        public decimal SalaryMinimumMounth { get; set; }

        [DataMember]
        public decimal SalaryMinimumDay { get; set; }
    }
}
