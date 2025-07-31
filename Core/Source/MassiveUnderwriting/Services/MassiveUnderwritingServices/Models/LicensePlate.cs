
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class LicensePlate
    {   
        /// <summary>
        ///  Obtiene o establece el ID de licencia
        /// </summary>
        [DataMember]
        public int LicenseId { set; get; }
        /// <summary>
        /// Obtiene o establece la Placa
        /// </summary>
        [DataMember]
        public string Plate { set; get; }        
    }
}
