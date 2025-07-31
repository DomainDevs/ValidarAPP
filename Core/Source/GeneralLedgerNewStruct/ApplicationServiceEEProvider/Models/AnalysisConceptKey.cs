#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///    AnalysisConceptKey
    /// </summary>
    [DataContract]
    public class AnalysisConceptKey
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
        public AnalysisConcept AnalysisConcept { get; set; }

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