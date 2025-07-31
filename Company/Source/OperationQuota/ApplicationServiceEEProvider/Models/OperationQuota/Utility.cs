using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class Utility
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public UtilityDetails UtilityDetails { get; set; }

        [DataMember]
        public decimal Start_Values { get; set; }
        [DataMember]
        public decimal End_value { get; set; }
        [DataMember]
        public decimal Var_Abs { get; set; }
        [DataMember]
        public decimal Var_Relativa { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }
    }
}
