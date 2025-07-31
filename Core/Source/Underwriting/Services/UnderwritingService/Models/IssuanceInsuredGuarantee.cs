using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Contrarantias del Asegurado
    /// </summary>
    [DataContract]
    public class IssuanceInsuredGuarantee : BaseIssuanceInsuredGuarantee
    {
        /// <summary>
        /// País
        /// </summary>
        [DataMember]
        public Country Country { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public IssuanceGuaranteeStatus GuaranteeStatus { get; set; }

        /// <summary>
        /// Departamento
        /// </summary>
        [DataMember]
        public State State { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        [DataMember]
        public City City { get; set; }

        ///// <summary>
        ///// Tipo de pagaré
        ///// </summary>
        //[DataMember]
        //public PromissoryNoteType PromissoryNoteType { get; set; }

        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        //[DataMember]
        //public MeasurementType MeasurementType { get; set; }

        /// <summary>
        /// Contragarantes
        /// </summary>
        //[DataMember]
        //public List<Models.Guarantor> Guarantors { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Compañía aseguradora
        /// </summary>
        //[DataMember]
        //public CoInsuranceCompany InsuranceCompany { get; set; }

        /// <summary>
        /// Guarantee Log
        /// </summary>
        //[DataMember]
        //public InsuredGuaranteeLog InsuredGuaranteeLog { get; set; }

        /// <summary>
        /// List Guarantee Documentation
        /// </summary>
        //[DataMember]
        //public List<InsuredGuaranteeDocumentation> listDocumentation { get; set; }

        /// <summary>
        /// List Guarantee Documentation
        /// </summary>
        //[DataMember]
        //public List<InsuredGuaranteePrefix> listPrefix { get; set; }

    }
}
