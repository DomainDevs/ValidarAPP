using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseEmail : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es Email principal?
        /// </summary>
        [DataMember]
        public bool IsMailingAddress { get; set; }

        /// <summary>
        /// Usuario que modifico
        /// </summary>
        [DataMember]
        public string UpdateUser { get; set; }

        /// <summary>
        /// Fecha de modificacion
        /// </summary>
        [DataMember]
        public string UpdateDate { get; set; }
    }
}
