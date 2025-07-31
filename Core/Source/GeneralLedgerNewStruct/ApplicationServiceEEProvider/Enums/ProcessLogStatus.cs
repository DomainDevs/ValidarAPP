using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums
{
    /// <summary>
    ///   Estados de Log de Procesos Masivos 
    /// </summary>
    [DataContract]
    public enum ProcessLogStatus
    {  
            [EnumMember]
            Started=0, 
            [EnumMember]
            Finished=1,
            [EnumMember]
            Canceled=2,        
    }
}

