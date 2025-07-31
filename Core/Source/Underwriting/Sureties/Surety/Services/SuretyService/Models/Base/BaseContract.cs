using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.SuretyServices.Models.Base
{
    [DataContract]
    public class BaseContract
    {
        /// <summary>
        /// numero de contrato
        /// </summary>
        [DataMember]
        public string SettledNumber { get; set; }

        /// <summary>
        /// monto disponible
        /// </summary>
        [DataMember]
        public decimal Available { get; set; }

        /// <summary>
        /// cumulo
        /// </summary>
        [DataMember]
        public decimal Aggregate { get; set; }
    }
}
