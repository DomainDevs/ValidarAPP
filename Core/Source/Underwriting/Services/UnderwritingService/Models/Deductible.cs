using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Deducibles
    /// </summary>
    [DataContract]
    public class Deductible : BaseDeductible
    {

        /// <summary>
        /// Atributo para la propiedad DeductibleSubject
        /// </summary> 
        [DataMember]
        public DeductibleSubject DeductibleSubject { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeductibleUnit
        /// </summary> 
        [DataMember]
        public DeductibleUnit DeductibleUnit { get; set; }

        /// <summary>
        /// Atributo para la propiedad MaxDeductibleSubject
        /// </summary> 
        [DataMember]
        public DeductibleSubject MaxDeductibleSubject { get; set; }

        /// <summary>
        /// Atributo para la propiedad MaxDeductibleUnit
        /// </summary> 
        [DataMember]
        public DeductibleUnit MaxDeductibleUnit { get; set; }

        /// <summary>
        /// Atributo para la propiedad MinDeductibleSubject
        /// </summary> 
        [DataMember]
        public DeductibleSubject MinDeductibleSubject { get; set; }

        /// <summary>
        /// Atributo para la propiedad MinDeductibleUnit
        /// </summary> 
        [DataMember]
        public DeductibleUnit MinDeductibleUnit { get; set; }

        /// <summary>
        /// Atributo para la propiedad Currency
        /// </summary> 
        [DataMember]
        public Currency Currency { get; set; }

        [DataMember]
        public LineBusiness LineBusiness { get; set; }
    }
}
