using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class BranchDTO
    {
        /// <summary>
        /// Identificador del ramo
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Nombre del ramo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
