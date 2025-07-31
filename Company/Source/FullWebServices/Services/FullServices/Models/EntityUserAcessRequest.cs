using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{    
        [DataContract]
        public class EntityUserAcessRequest 
        {
            [DataMember]
            public string UserName { get; set; }

            [DataMember]
            public string Passwoord { get; set; }

            [DataMember]
            public int IdWs { get; set; }
            
            [DataMember]
            public int IdMethod { get; set; }            
        }    
}
