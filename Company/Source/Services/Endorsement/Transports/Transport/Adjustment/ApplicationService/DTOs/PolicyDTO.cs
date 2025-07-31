using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs
{
     public class PolicyDTO
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
        public int PolicyNumber { get; set; }

        /// <summary>
        /// Póliza
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// fecha de vigencia desde. del endoso
        /// </summary>
        public DateTime CurrentFrom { get; set; }
        /// <summary>
        /// fecha de vigencia hasta. del endoso
        /// </summary>
        public DateTime CurrentTo { get; set; }
        /// <summary>
        ///numero de dias se calcula entre la vigencia hasta menos la vigencia desde
        /// </summary>
        public int Days { get; set; }
        public int ProducId { get; set; }
    }
}
