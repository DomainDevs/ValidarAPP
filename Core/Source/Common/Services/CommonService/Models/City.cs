using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class City : BaseCity
    {
        public City()
        {
            State = new State();
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public State State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DANECode { get; set; }

    }
}
