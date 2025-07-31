using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    public class PolicyIssuanceVehicleResult
    {
        public int PolicyNumber { get; set; }
        public int BranchCode { get; set; }
        public int PrefixCode { get; set; }
        public int EndorsementNumber { get; set; }
        public int ProductId { get; set; }
        public DateTime CurrentFrom { get; set; }
        public DateTime CurrentTo { get; set; }
        public string LicensePlate { get; set; }
        public decimal LimitRcSum { get; set; }
        public decimal VehiclePrice { get; set; }
        public decimal VehiclePriceDetail { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal AssistanceAmount { get; set; }
        public decimal ExpensesAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPremiumAmount { get; set; }
        public bool Status { get; set; }
        public string ProcessMessage { get; set; }

    }
}
