using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class RoleView
    {
        int _idRole;

        [DataMember]
        public int IdRole
        {
            get { return _idRole; }
            set { _idRole = value; }
        }

        string _nameView;

        [DataMember]
        public string NameView
        {
            get { return _nameView; }
            set { _nameView = value; }
        }

        bool _main;

        [DataMember]
        public bool Main
        {
            get { return _main; }
            set { _main = value; }
        }   
    }
}
