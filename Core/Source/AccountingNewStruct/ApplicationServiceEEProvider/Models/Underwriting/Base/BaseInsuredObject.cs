using Sistran.Core.Application.Extensions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    [DataContract]
    public class BaseInsuredObject : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad Small Description
        /// </summary> 
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsDeclarative
        /// </summary> 
        [DataMember]
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsMandatory
        /// </summary> 
        [DataMember]
        public bool? IsMandatory { get; set; }

        ///<summary>
        /// prima de coberturas asociadas al objeto
        /// </summary>
        [DataMember]
        public decimal Premium { get; set; }

        /// <summary>
        /// monto del objeto
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Objeto inicialmente incluido
        /// </summary>
        [DataMember]
        public bool? IsSelected { get; set; }


        /// <summary>
        /// Gets or sets the parametrization status.
        /// </summary>
        /// <value>
        /// The parametrization status.
        /// </value>
        [DataMember]
        public ParametrizationStatus? ParametrizationStatus { get; set; }
    }
}
