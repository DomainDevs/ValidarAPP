using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.PrintingServices.Models
{
    [DataContract]
    public class LogErrorPrintDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        //[DataMember]
        //public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// DateError
        /// </summary>
        [DataMember]
        public DateTime DateError { get; set; }

        /// <summary>
        /// EndorsementId
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// PolicyId
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }


    }
}
