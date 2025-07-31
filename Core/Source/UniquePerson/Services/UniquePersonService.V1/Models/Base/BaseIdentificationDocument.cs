using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseIdentificationDocument : Extension
    {


        /// <summary>
        /// Number
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// ExpeditionDate
        /// </summary>
        [DataMember]
        public DateTime ExpeditionDate { get; set; }

    }
}
