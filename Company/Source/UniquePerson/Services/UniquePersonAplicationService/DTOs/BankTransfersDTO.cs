using Sistran.Core.Application.CommonService.Models;
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
    public class BankTransfersDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]    
        public int BankId { get; set; }

        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string BankBranch { get; set; }

        [DataMember]
        public string BankSquare { get; set; }

        [DataMember]
        public int AccountTypeId { get; set; }


        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public string PaymentBeneficiary { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public bool ActiveAccount { get; set; }

        [DataMember]
        public bool DefaultAccount { get; set; }

        [DataMember]
        public bool IntermediaryBank { get; set; }
        [DataMember]
        public DateTime? InscriptionDate { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int OperationId { get; set; }


    }
}
