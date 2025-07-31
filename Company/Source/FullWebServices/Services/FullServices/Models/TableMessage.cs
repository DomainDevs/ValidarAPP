using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract] 
    public class TableMessage
    {
        string _nameTable;

        [DataMember]
        public string NameTable
        {
            get { return _nameTable; }
            set { _nameTable = value; }
        }


        string _message;
        [DataMember]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }        	
    }
}
