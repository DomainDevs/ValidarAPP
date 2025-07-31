using System.Runtime.Serialization;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    [DataContract]
    public class TypeDTO
    {

        /// <summary>
        /// tipo aeronave
        /// </summary>
		[DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id aeronave
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
		public TypeDTO()
        {

        }

    }

    }
