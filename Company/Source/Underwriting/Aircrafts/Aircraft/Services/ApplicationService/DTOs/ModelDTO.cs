using System.Runtime.Serialization;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    [DataContract]
    public class ModelDTO
    {
        /// <summary>
        /// Modelo aircraft
        /// </summary>
		[DataMember]
        public string Description { get; set; }

        /// <summary>
        /// id Modelo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
		public ModelDTO()
        {

        }

    }

}
