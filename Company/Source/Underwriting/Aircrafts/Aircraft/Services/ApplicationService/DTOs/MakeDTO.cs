using System.Runtime.Serialization;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    [DataContract]
    public class MakeDTO
    {

        /// <summary>
        /// Marca aircraft
        /// </summary>
		[DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id de la marca aircraft
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
		public MakeDTO()
        {

        }


    }
    }
