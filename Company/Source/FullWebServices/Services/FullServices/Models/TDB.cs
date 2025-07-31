using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class TDB
    {
        string _nameTable;

        [DataMember]
        public string NameTable
        {
            get { return _nameTable; }
            set { _nameTable = value; }
        }
        
        string _prefix;

        [DataMember]
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }     
        
    }
}
