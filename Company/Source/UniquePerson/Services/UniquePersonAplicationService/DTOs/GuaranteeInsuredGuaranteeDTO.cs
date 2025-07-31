using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class GuaranteeInsuredGuaranteeDTO
    {

        /// <summary>
        /// indentificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo contragarantia
        /// </summary>
        [DataMember]
        public int typeId { get; set; }

    }
}
