using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Model
{
    public class Tax
    {
        public int CountryId { get; set; }
        public int ExtentPercentage { get; set; }
        public string ResolutionNumber { get; set; }
        public int StateCode { get; set; }
        public string StateCodeDescription { get; set; }
        public int TaxId { get; set; }
        public string TaxDescription { get; set; }
        public int TaxCategoryId { get; set; }
        public string TaxCategoryDescription { get; set; }
        public int TaxCondition { get; set; }
        public string TaxConditionDescription { get; set; }
        public bool TotalRetention { get; set; }
        public int RoleId { get; set; }
        public int TaxRateId { get; set; }
    }
}
