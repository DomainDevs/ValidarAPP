using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Sarlaft
    /// </summary>
    [DataContract]
    public class Sarlaft
    {
        /// <summary>
        /// Obtiene o setea Identifiador Sarlaft
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the income amount.
        /// </summary>
        /// <value>
        /// The income amount.
        /// </value>
        [DataMember]
        public decimal IncomeAmount { get; set; }

        /// <summary>
        /// Gets or sets the expense amount.
        /// </summary>
        /// <value>
        /// The expense amount.
        /// </value>
        [DataMember]
        public decimal ExpenseAmount { get; set; }

        /// <summary>
        /// Gets or sets the extra income amount.
        /// </summary>
        /// <value>
        /// The extra income amount.
        /// </value>
        [DataMember]
        public decimal ExtraIncomeAmount { get; set; }

        /// <summary>
        /// Gets or sets the assets amount.
        /// </summary>
        /// <value>
        /// The assets amount.
        /// </value>
        [DataMember]
        public decimal AssetsAmount { get; set; }

        /// <summary>
        /// Gets or sets the liabilities amount.
        /// </summary>
        /// <value>
        /// The liabilities amount.
        /// </value>
        [DataMember]
        public decimal LiabilitiesAmount { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

    }
}
