using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs
{
    public class PolicyDTO:RiskDTO
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Ramo
        /// </summary>
        public int PrefixId { get; set; }

        /// <summary>
        /// Póliza
        /// </summary>
        public string PolicyNumber { get; set; }
        /// <summary>
        /// Fechas
        /// </summary>
        public DateTime Current{ get; set; }
        /// <summary>
        /// Dias
        /// </summary>
        public int Days { get; set; }   
               
        
        /// <summary>
        /// Producto
        /// </summary>
        public int ProductId { get; set; }
    }
}
