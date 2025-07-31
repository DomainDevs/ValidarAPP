using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyPersonOperation
    {
        /// <summary>
        /// OperationId
        /// </summary>
        [DataMember]
        public int OperationId { get; set; }

        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int? IndividualId { get; set; }

        /// <summary>
        /// Operation
        /// </summary>
        [DataMember]
        public string Operation { get; set; }

        /// <summary>
        /// Process
        /// </summary>
        [DataMember]
        public string Process { get; set; }

        /// <summary>
        /// Process
        /// </summary>
        [DataMember]
        public string ProcessType { get; set; }

        /// <summary>
        /// FunctionId
        /// </summary>
        [DataMember]
        public int FunctionId { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        [DataMember]
        public int StatusId { get; set; }

        /// <summary>
        /// RequestId
        /// </summary>
        [DataMember]
        public int RequestId { get; set; }
    }
}
