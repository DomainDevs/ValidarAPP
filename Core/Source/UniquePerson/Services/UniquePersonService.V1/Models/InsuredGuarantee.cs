using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Contrarantias del Asegurado
    /// </summary>
    [DataContract]
    public class InsuredGuarantee : BaseInsuredGuarantee
    {
        /// <summary>
        /// País
        /// </summary>
        [DataMember]
        public Country Country { get; set; }


        /// <summary>
        /// Departamento
        /// </summary>
        [DataMember]
        public State State { get; set; }
      
        /// <summary>
        /// Tipo de pagaré
        /// </summary>
        [DataMember]
        public PromissoryNoteType PromissoryNoteType { get; set; }
      

        /// <summary>
        /// Unidad de medida
        /// </summary>
        [DataMember]
        public MeasurementType MeasurementType { get; set; }
        /// <summary>
        /// Contragarantes
        /// </summary>
        [DataMember]
        public List<Models.Guarantor> Guarantors { get; set; }
      
        /// <summary>
        /// Compañía aseguradora
        /// </summary>
        [DataMember]
        public CoInsuranceCompany InsuranceCompany { get; set; }
        /// <summary>
        /// Guarantee Log
        /// </summary>
        [DataMember]
        public List<InsuredGuaranteeLog> InsuredGuaranteeLog { get; set; }


        //propiedades faltantes
        /// <summary>
        /// Guarantee Log
        /// </summary>
        [DataMember]
        public InsuredGuaranteeLog InsuredGuaranteeLogObject { get; set; }

        ///// <summary>
        ///// Guarantee Log
        ///// </summary>
        //[DataMember]
        //public string InsuredGuaranteeLogDescription { get; set; }

        /// <summary>
        /// List Guarantee Documentation
        /// </summary>
        [DataMember]
        public List<InsuredGuaranteeDocumentation> listDocumentation { get; set; }

        /// <summary>
        /// List Guarantee Documentation
        /// </summary>
        [DataMember]
        public List<InsuredGuaranteePrefix> listPrefix { get; set; }





    }
}
