using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    public class BankTransfers
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// IndividualId
        /// </summary>
        /// 
        [DataMember]
        public int Individual { get; set; }

        /// <summary>
        /// Bank
        /// </summary>
        /// 
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// BankBranch
        /// </summary>
        public string BankBranch { get; set; }


        /// <summary>
        /// BankSquare
        /// </summary>
        public string BankSquare { get; set; }
        /// <summary>
        /// AccountType
        /// </summary>
        public UniquePersonService.V1.Models.AccountType AccountType { get; set; }


        /// <summary>
        /// AccountNumber
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public Currency Currency { get; set; }


        /// <summary>
        /// PaymentBeneficiary
        /// </summary>
        public string PaymentBeneficiary { get; set; }

        /// <summary>
        /// ActiveAccount
        /// </summary>
        public bool ActiveAccount { get; set; }

        /// <summary>
        /// DefaultAccount
        /// </summary>
        public bool DefaultAccount { get; set; }

        /// <summary>
        /// IntermediaryBank
        /// </summary>
        public bool IntermediaryBank { get; set; }

        public DateTime? InscriptionDate { get; set; }











    }
}
