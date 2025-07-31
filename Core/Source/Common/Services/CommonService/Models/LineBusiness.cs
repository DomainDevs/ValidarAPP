
using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class LineBusiness : BaseLineBusiness
    {
        /// <summary>
        /// Lista para los Objetos de seguro del ramo tecnico
        /// </summary>
        [DataMember]
        public List<int> ListInsurectObjects { get; set; }
    }
}