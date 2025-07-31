using Sistran.Core.Application.UniquePersonService.V1.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BasePerson: BaseIndividual
    {
       
        /// <summary>
        /// nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///primer  Apellido
        /// </summary>
        [DataMember]
        public string SurName { get; set; }

        /// <summary>
        ///segundo  Apellido
        /// </summary>
        [DataMember]
        public string SecondSurName { get; set; }

        /// <summary>
        ///genero
        /// </summary>
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        ///fecha nacimiento
        /// </summary>
        [DataMember]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Children
        /// </summary>
        public int? Children { get; set; }

        /// <summary>
        ///SpouceName
        /// </summary>
        [DataMember]
        public string SpouseName { get; set; }

        /// <summary>
        ///BirthCountryId
        /// </summary>
        [DataMember]
        public int BirthCountryId { get; set; }

        /// <summary>
        ///BirthPlace
        /// </summary>
        [DataMember]
        public string BirthPlace { get; set; }

        /// <summary>
        ///HasDataProtection
        /// </summary>
        [DataMember]
        public bool HasDataProtection { get; set; }

    }
}
