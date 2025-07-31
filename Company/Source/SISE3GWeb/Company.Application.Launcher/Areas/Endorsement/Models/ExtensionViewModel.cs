using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ExtensionViewModel : EndorsementViewModel
    {
        /// <summary>
        /// fecha de vigencia desde. del endoso
        /// </summary>
        [DataMember]
        public DateTime validityDateFrom { get; set; }
        /// <summary>
        /// fecha de vigencia hasta. del endoso
        /// </summary>
        [DataMember]
        public DateTime validityDateTo { get; set; }
        /// <summary>
        /// Id Motivo de modificacion
        /// </summary>
        public int DaysPolicy { get; set; }
        /// <summary>
        /// Ultimo endoso
        /// </summary>
        public int Endorsement { get; set; }
    }
}