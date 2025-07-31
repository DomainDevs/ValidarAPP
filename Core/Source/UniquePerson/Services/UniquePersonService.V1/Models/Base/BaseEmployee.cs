using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseEmployee:Extension
    {
        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// BranchId
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// FileNumber
        /// </summary>
        [DataMember]
        public string  FileNumber { get; set; }

        /// <summary>
        /// EntryDate
        /// </summary>
        [DataMember]
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// EgressDate
        /// </summary>
        [DataMember]
        public DateTime? EgressDate { get; set; }


        /// <summary>
        /// DeclinedTypeId
        /// </summary>
        [DataMember]
        public int? DeclinedTypeId { get; set; }

        /// <summary>
        /// Annotation
        /// </summary>
        [DataMember]
        public string Annotation { get; set; }

         /// <summary>
        /// Fecha de Modificacion
        /// </summary>
        [DataMember]
        public DateTime? ModificationDate { get; set; }
    }
}
