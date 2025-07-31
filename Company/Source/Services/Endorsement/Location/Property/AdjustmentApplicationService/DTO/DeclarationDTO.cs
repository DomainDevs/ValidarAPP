using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentApplicationService.DTO
{
    [DataContract]
    public class DeclarationDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Monto de declaración
        /// </summary>
        [DataMember]
        public decimal DeclaredAmount { get; set; }

        /// <summary>
        /// Descripción del endoso
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
  
}
