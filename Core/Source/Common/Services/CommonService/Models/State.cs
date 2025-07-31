using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class State : BaseState
    {
        public State()
        {
            Country = new Country();
        }

        /// <summary>
        /// Páis
        /// </summary>
        [DataMember]
        public Country Country { get; set; }            

        /// <summary>
        /// Listado de ciudades
        /// </summary>
        [DataMember]
        public List<City> Cities { get; set; }           
    

    }
}
