using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveCollectiveDetail
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece IdMassive
        /// </summary>
        [DataMember]
        public int IdMassive { get; set; }

        /// <summary>
        /// Obtiene o establece IdExcelRisk
        /// </summary>
        [DataMember]
        public int? IdExcelRisk { get; set; }

        /// <summary>
        /// Obtiene o establece IdRisk
        /// </summary>
        [DataMember]
        public int? IdRisk { get; set; }

        /// <summary>
        /// Obtiene o establece TempId
        /// </summary>
        [DataMember]
        public int? TempId { get; set; }

        /// <summary>
        /// Obtiene o establece SnError
        /// </summary>
        [DataMember]
        public bool? SnError { get; set; }

        /// <summary>
        /// Obtiene o establece Error
        /// </summary>
        [DataMember]
        public string Error { get; set; }

        /// <summary>
        /// Obtiene o establece SnTariffed
        /// </summary>
        [DataMember]
        public bool? SnTariffed { get; set; }

        /// <summary>
        /// Obtiene o establece SnFinish
        /// </summary>
        [DataMember]
        public bool? SnFinish { get; set; }

        /// <summary>
        /// Obtiene o establece SnIssue
        /// </summary>
        [DataMember]
        public bool? SnIssue { get; set; }

        /// <summary>
        /// Obtiene o establece EndorsementId
        /// </summary>
        [DataMember]
        public int? EndorsementId { get; set; }

        /// <summary>
        /// Obtiene o establece IssueDate
        /// </summary>
        [DataMember]
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Obtiene o establece ProcessId
        /// </summary>
        [DataMember]
        public int? ProcessId { get; set; }

        /// <summary>
        /// Obtiene o establece SubprocessId
        /// </summary>
        [DataMember]
        public int? SubprocessId { get; set; }

        /// <summary>
        /// Obtiene o establece EndorsementType
        /// </summary>
        [DataMember]
        public int? EndorsementType { get; set; }

        /// <summary>
        /// Obtiene o establece LicensePlate
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Obtiene o establece StepError
        /// </summary>
        [DataMember]
        public int? StepError { get; set; }

        /// <summary>
        /// Obtiene o establece IsEvent
        /// </summary>
        [DataMember]
        public bool? IsEvent { get; set; }

        /// <summary>
        /// Obtiene o establece UserId
        /// </summary>
        [DataMember]
        public int? UserId { get; set; }

        /// <summary>
        /// Obtiene o establece TariffedDate
        /// </summary>
        [DataMember]
        public DateTime? TariffedDate { get; set; }

        /// <summary>
        /// Obtiene o establece DifferencePremium
        /// </summary>
        [DataMember]
        public string DifferencePremium { get; set; }
    }
}
