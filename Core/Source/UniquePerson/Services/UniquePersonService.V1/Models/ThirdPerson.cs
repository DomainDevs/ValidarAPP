using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{

    [DataContract]
    public class ThirdPerson
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

        /// <summary>
        /// Tipo de Documento
        /// </summary>
        [DataMember]
        public int? DocumentTypeId { get; set; }

        /// <summary>
        /// Nombre completo
        /// </summary>
        [DataMember]
        public string Fullname { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }
    }
}
