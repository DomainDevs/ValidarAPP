using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredGuaranteeOthersDTO
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string DescriptionOthers { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public GuaranteeDTO Guarantee { get; set; }

        [DataMember]
        public SelectDTO Branch { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool ClosedInd { get; set; }

        /// <summary>
        /// fecha registro
        /// </summary>
        [DataMember]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// fecha ultima actualizacion
        /// </summary>
        [DataMember]
        public DateTime LastChangeDate { get; set; }

        [DataMember]
        public SelectDTO Status { get; set; }



    }
}
