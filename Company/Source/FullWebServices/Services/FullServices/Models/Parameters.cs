using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class Parameters
    {
        string _nameTable;

        [DataMember]
        public string NameTable
        {
            get { return _nameTable; }
            set { _nameTable = value; }
        }


        string _parameterType;

        [DataMember]
        public string ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        int _parameterSize;

        [DataMember]
        public int ParameterSize
        {
            get { return _parameterSize; }
            set { _parameterSize = value; }
        }


        string _parameter;
        
        [DataMember]
        public string Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        string _value;

        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }       
        
    }
}
