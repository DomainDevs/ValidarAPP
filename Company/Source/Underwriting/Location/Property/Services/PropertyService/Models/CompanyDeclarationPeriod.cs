using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class CompanyDeclarationPeriod : BaseGeneric
    {
        [DataMember]
        public bool IsEnabled { get; set; }

    }
}