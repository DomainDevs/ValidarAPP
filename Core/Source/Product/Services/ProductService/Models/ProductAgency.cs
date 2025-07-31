using Sistran.Core.Application.ProductServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ProductServices.Models
{
    [DataContract]
    public class ProductAgency : BaseProductAgency
    {
        [DataMember]
        public ProductAgent Agent { get; set; }
    }
}