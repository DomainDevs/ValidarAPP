using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    ///  Garantias del Asegurado Asociadas a la Poliza
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.Individual" />
    [DataContract]
    public class InsuredGuaranteePolicy : Individual
    {
        /// <summary>
        /// Gets or sets the guarantee identifier.
        /// </summary>
        /// <value>
        /// The guarantee identifier.
        /// </value>
        [DataMember]
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Gets or sets the document number.
        /// </summary>
        /// <value>
        /// The document number.
        /// </value>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the document number.
        /// </summary>
        /// <value>
        /// The document number.
        /// </value>
        [DataMember]
        public int DocumentNum { get; set; }

        /// <summary>
        /// Gets or sets the description branch.
        /// </summary>
        /// <value>
        /// The description branch.
        /// </value>
        [DataMember]
        public string DescriptionBranch { get; set; }

        /// <summary>
        /// Gets or sets the description prefix.
        /// </summary>
        /// <value>
        /// The description prefix.
        /// </value>
        [DataMember]
        public string DescriptionPrefix { get; set; }

    }
}
