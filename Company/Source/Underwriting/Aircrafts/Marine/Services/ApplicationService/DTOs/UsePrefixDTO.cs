using System.Runtime.Serialization;

namespace Sistran.Company.Application.Marines.MarineApplicationService.DTOs
{
    [DataContract]
    public class UsePrefixDTO
    {
        /// <summary>
        /// Uso de la aeronave
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id de uso de la aeronave
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
		public UsePrefixDTO()
        {

        }
    }
}
