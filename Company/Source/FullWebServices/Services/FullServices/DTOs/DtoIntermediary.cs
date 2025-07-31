using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.DTOs;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class DtoIntermediary
    {
        
        [DataMember]
        public List<Referencias> List_Referencias { get; set; }

    }
}
