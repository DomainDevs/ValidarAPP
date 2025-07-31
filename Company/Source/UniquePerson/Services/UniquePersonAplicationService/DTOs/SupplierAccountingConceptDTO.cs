using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class SupplierAccountingConceptDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember]
        public int SupplierId { get; set; }
        
        /// <summary>
        /// AccountingConceptId
        /// </summary>
        [DataMember]
        public int AccountingConceptId { get; set; }

    }
}
