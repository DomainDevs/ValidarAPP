using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Marines.MarineBusinessService.Models
{
    /// <summary>
    /// Marinee - Riesgo
    /// </summary>
    [DataContract]
    public class Marine : BaseMarine
    {
        public Marine()
        {
            Risk = new Risk();
            CityFrom = new City();
            CityTo = new City();
            List<MarineType> Types = new List<MarineType>();
         }

        [DataMember]
        public Risk Risk { get; set; }
      
        /// <summary>
        /// Desde
        /// </summary>
        [DataMember]
        public City CityFrom { get; set; }
        /// <summary>
        /// Hasta
        /// </summary>
        [DataMember]
        public City CityTo { get; set; }
        /// <summary>
        /// Medio de Marinee
        /// </summary>
        [DataMember]
        public List<MarineType> Types { get; set; }

        
    }
}
