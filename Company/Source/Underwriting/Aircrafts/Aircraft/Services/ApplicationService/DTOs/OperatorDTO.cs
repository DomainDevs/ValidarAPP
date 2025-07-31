
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    [DataContract]
    public class OperatorDTO
    {

        /// <summary>
        /// Explotadores de aeronaves
        /// </summary>
		[DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id de Explotadores de aeronaves
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// constructor
        /// </summary>
		public OperatorDTO()
        {

        }

    }
}

