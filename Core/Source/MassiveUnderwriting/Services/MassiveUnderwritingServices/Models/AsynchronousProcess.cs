using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{

    [DataContract]
    public class AsynchronousProcess 
    {
        /// <summary>
        /// Obtiene o establece el Identificador del proceso
        /// </summary>
        [DataMember]
        public int ProcessId { set; get; }

        /// <summary>
        /// Obtiene o establece el Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        ///Obtiene o establece el BeginDate.
        /// </summary>
        [DataMember]
        public DateTime? BeginDate { set; get; }

        /// <summary>
        /// Obtiene o establece el EndDate.
        /// </summary>
        [DataMember]
        public DateTime? EndDate { set; get; }

        /// <summary>
        /// Obtiene o establece el UserId
        /// </summary>
        [DataMember]
        public int? UserId { set; get; }

        /// <summary>
        /// Obtiene o establece el Status.
        /// </summary>
        [DataMember]
        public bool? Status { set; get; }

        /// <summary>
        /// Obtiene o establece el HasError.
        /// </summary>
        [DataMember]
        public bool? HasError { set; get; }

        /// <summary>
        /// Obtiene o establece el ErrorDescription.
        /// </summary>
        [DataMember]
        public string ErrorDescription { set; get; }

        /// <summary>
        /// Obtiene o establece el IssuanceStatus.
        /// </summary>
        [DataMember]
        public int? IssuanceStatus { set; get; }
     
    }
}
