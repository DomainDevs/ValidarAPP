using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{    
        [DataContract]
        public class EntityExampleResponse 
        {
            [DataMember]
            public int cod_agente { get; set; }

            [DataMember]
            public string txt_cheque_a_nom { get; set; }            
        }    
}
