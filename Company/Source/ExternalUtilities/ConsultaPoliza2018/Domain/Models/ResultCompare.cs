using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Domain.Models.Entities
{
    public class ResultCompare
    {
        public SearchPolicies criterios { get; set; }
        public string poliza { get; set; }
        public string poliza2 { get; set; } = string.Empty;
        public string resume { get; set; } = string.Empty;
        public long polizaNum { get; set; }
        public long polizaNum2 { get; set; }
    }
}