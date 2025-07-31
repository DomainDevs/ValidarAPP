using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    /// <summary>
    /// resultado de la poliza
    /// </summary>
    public class PolicyResultViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the issue date.
        /// </summary>
        /// <value>
        /// The issue date.
        /// </value>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Id póliza
        /// </summary>
        /// <value>
        /// The policy identifier.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has event.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has event; otherwise, <c>false</c>.
        /// </value>
        public bool HasEvent { get; set; }

    }
}