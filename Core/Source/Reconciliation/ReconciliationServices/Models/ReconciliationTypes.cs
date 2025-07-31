
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReconciliationServices.Models
{
    /// <summary>
    /// ReconciliationTypes: Tipo de Concilicacion
    /// </summary>
    [DataContract]
    public enum ReconciliationTypes
    {
        [EnumMember]
        Manual = 1,
        [EnumMember]
        Automatic=2,
     }
}
