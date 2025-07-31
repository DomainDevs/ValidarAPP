using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PolicyBaseViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Id póliza
        /// </summary>
        /// <value>
        /// The policy identifier.
        /// </value>
        public int PolicyId { get; set; }


        /// <summary>
        /// Gets or sets the endorsement identifier.
        /// </summary>
        /// <value>
        /// The endorsement identifier.
        /// </value>
        public int EndorsementId { get; set; }


        /// <summary>
        /// Gets or sets the type of the modification.
        /// </summary>
        /// <value>
        /// The type of the modification.
        /// </value>
        public byte EndorsementType { get; set; }

        /// <summary>
        /// Gets or sets the type of the modification.
        /// </summary>
        /// <value>
        /// The type of the modification.
        /// </value>
        public byte EndorsementTypeOriginal { get; set; }
        /// <summary>
        /// Gets or sets the endorment temporal.
        /// </summary>
        /// <value>
        /// The endorment temporal.
        /// </value>
        public byte EndorsementTemporal { get; set; }

        /// <summary>
        /// Tipo de endoso de modificacion
        /// </summary>
        public string ModificationEndorsementType { get; set; }
    }
}