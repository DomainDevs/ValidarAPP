using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Enums
{
    [DataContract]
    public enum AgentType
    {
        [EnumMember]
        CORREDOR = 1,

        [EnumMember]
        AGENCIA = 2,

        [EnumMember]
        AGENTE = 3,

        [EnumMember]
        COASEGURADORA = 4, 

        [EnumMember]
        CORREDOR_DE_REASEGURO = 5,

        [EnumMember]
        CORREDOR_DE_REASEGURO_EXTERNO = 6,

        [EnumMember]
        DIRECTO = 7,

        [EnumMember]
        WINS = 8
    }
}
