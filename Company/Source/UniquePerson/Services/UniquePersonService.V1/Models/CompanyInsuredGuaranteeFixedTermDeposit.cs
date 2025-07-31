using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyInsuredGuaranteeFixedTermDeposit : BaseInsuredGuarantee
    {
        /// <summary>
        /// Numero de documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Valor Nominal
        /// </summary>
        [DataMember]
        public Decimal DocumentValueAmount { get; set; }

        /// <summary>
        /// Fecha constitucion
        /// </summary>
        [DataMember]
        public DateTime ConstitutionDate { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [DataMember]
        public DateTime ExtDate { get; set; }

        /// <summary>
        /// Entidad emisora
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }
    }
}
