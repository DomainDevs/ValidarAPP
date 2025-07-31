using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Cupo Operativo
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.Individual" />
    [DataContract]
    public class OperatingQuota : BaseOperatingQuota
    {
    }
}
