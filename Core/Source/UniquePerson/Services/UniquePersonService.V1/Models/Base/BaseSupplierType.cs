using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseSupplierType: BaseGeneric
    {
        public bool? Enable { get; set; }
    }
}
