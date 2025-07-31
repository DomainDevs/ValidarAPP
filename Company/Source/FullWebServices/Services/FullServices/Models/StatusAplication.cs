using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class StatusAplication 
    {

        public StatusAplication(int idAplication, string keyAplication)
        {
            this.IdAplication = idAplication;
            this.KeyAplication = keyAplication;
        }

        int _idAplication;

        [DataMember]
        public int IdAplication
        {
            get { return _idAplication; }
            set { _idAplication = value; }
        }

        string _keyAplication;

        [DataMember]
        public string KeyAplication
        {
            get { return _keyAplication; }
            set { _keyAplication = value; }
        }     
        
    }
}
