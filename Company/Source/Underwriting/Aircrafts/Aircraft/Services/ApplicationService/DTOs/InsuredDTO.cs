
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    [DataContract]
    public class InsuredDTO
    {

        /// <summary>
        /// Id de asegurado
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del asegurado
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
		public InsuredDTO()
        {

        }

    }
    }
