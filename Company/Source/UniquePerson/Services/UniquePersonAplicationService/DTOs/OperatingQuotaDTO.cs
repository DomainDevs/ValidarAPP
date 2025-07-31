using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class OperatingQuotaDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int LineBusinessId { get; set; }

        [DataMember]
        public decimal AmountValue { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public DateTime CurrentTo  { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }

    }
}
