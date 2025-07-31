#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///    AnalysisConceptKey
    /// </summary>
    [DataContract]
    public class AnalysisConceptKeyDTO
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     AnalysisConcept
        /// </summary>
        [DataMember]
        public AnalysisConceptDTO AnalysisConcept { get; set; }

        /// <summary>
        ///     TableName
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        ///     ColumnName
        /// </summary>
        [DataMember]
        public string ColumnName { get; set; }

        /// <summary>
        ///     ColumnDescription
        /// </summary>
        [DataMember]
        public string ColumnDescription { get; set; }

    }
}