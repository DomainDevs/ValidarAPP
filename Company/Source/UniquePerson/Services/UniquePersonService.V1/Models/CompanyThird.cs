using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyThird
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// IndividualID Person
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? EnteredDate { get; set; }

        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }




        /// <summary>
        /// Fecha de modificacion
        /// </summary>
        [DataMember]
        public DateTime? ModificationDate { get; set; }


        /// <summary>
        /// Id Tipo de baja
        /// </summary>
        [DataMember]
        public int? DeclinedTypeId { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public string Annotation { get; set; }



        ///// <summary>
        ///// Fecha de creación
        ///// </summary>
        //[DataMember]
        //public DateTime CreationDate { get; set; }

    }
}
