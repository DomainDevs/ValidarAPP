using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class PersonOperationDTO
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
        public string Proccess { get; set; }

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
    }
}
