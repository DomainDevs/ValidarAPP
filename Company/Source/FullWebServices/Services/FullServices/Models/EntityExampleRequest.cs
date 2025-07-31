using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{    
        [DataContract]
        public class EntityExampleRequest 
        {
            [DataMember]
            public int Campo1 { get; set; }

            [DataMember]
            public int Campo2 { get; set; }

            [DataMember]
            public int Campo3 { get; set; }            
        }    
}
