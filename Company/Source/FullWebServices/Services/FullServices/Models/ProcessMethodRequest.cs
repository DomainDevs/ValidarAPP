using System;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{    
        [DataContract]
        public class ProcessMethodRequest 
        {
            [DataMember]
            public int IdWs { get; set; }

            [DataMember]
            public int IdMethod { get; set; }

            [DataMember]
            public DateTime? BeginDate { get; set; }

            [DataMember]
            public DateTime? EndDate { get; set; }

            [DataMember]
            public int State { get; set; }            
        }    
}
