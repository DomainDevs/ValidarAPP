using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class TmpSarlaftOperationDTO
    {
        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int SarlaftId { get; set; }

        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string Proccess { get; set; }

        [DataMember]
        public string TypeProccess { get; set; }

        [DataMember]
        public int FunctionId { get; set; }
    }
}
