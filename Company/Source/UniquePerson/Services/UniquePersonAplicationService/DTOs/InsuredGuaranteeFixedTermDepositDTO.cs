using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredGuaranteeFixedTermDepositDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public GuaranteeDTO Guarantee { get; set; }

        [DataMember]
        public SelectDTO Branch { get; set; }

        [DataMember]
        public SelectDTO City { get; set; }

        [DataMember]
        public SelectDTO Country { get; set; }

        [DataMember]
        public SelectDTO State { get; set; }


        [DataMember]
        public SelectDTO Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool ClosedInd { get; set; }

        /// <summary>
        /// fecha registro
        /// </summary>
        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public DateTime ConstitutionDate { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [DataMember]
        public DateTime ExtDate { get; set; }

        /// <summary>
        /// fecha ultima actualizacion
        /// </summary>
        [DataMember]
        public DateTime LastChangeDate { get; set; }

        [DataMember]
        public SelectDTO Status { get; set; }

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
        /// Entidad emisora
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }

    }
}
