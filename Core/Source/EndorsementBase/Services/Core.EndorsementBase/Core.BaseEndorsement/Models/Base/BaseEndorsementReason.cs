using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.BaseEndorsementService.Models.Base
{
    /// <summary>
    /// Razon del endoso
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Extensions.Extension" />
    [DataContract]
    public class BaseEndorsementReason : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Motivo del endoso
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
