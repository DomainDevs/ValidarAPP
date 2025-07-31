using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class SupRoles
    {

        int _idRole;

        [DataMember]
        public int IdRole
        {
            get { return _idRole; }
            set { _idRole = value; }
        } 
        
        string _nameRole;

        [DataMember]
        public string NameRole
        {
            get { return _nameRole; }
            set { _nameRole = value; }
        } 

    }
}
