
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base
{
    /// <summary>
    ///Apoderado
    /// </summary>
    [DataContract]
    public class BaseAttorney
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// tipo de identificacion del apoderado
        /// </summary>
        [DataMember]
        public string Name { get; set; }


        /// <summary>
        /// Numero de la tarjeta profesional
        /// </summary>
        [DataMember]
        public string IdProfessionalCard { get; set; }
        /// <summary>
        /// Asegurado a imprimir
        /// </summary>
        [DataMember]
        public string InsuredToPrint { get; set; } 


    }
}
