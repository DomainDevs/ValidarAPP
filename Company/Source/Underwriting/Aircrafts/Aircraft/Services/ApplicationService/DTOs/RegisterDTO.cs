
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    [DataContract]
    public class RegisterDTO
    {

        /// <summary>
        /// Matricula
        /// </summary>
		[DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id de la matricula
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
		public RegisterDTO()
        {

        }
    }
    }
