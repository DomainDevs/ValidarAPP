using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.PrintingServices.Models
{
    [DataContract]
    public class LogPrintStatusDTO
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PolicyId
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// EndorsementId
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public string Observacion { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        [DataMember]
        public int StatusId { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DataMember]
        public string Url { get; set; }
    }
}
