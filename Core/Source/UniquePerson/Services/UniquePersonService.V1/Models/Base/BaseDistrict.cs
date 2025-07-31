using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    public class BaseDistrict
    {
        /// <summary>
        /// DISTRICT_CD
        /// </summary>
        [DataMember]
        public int DISTRICT_CD { get; set; }

        /// <summary>
        /// COUNTRY_CD
        /// </summary>
        [DataMember]
        public int COUNTRY_CD { get; set; }

        /// <summary>
        /// STATE_CD
        /// </summary>
        [DataMember]
        public int STATE_CD { get; set; }

        /// <summary>
        /// CITY_CD
        /// </summary>
        [DataMember]
        public int CITY_CD { get; set; }

        /// <summary>
        /// DESCRIPTION
        /// </summary>
        [DataMember]
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// SMALL_DESCRIPTION
        /// </summary>
        [DataMember]
        public string SMALL_DESCRIPTION { get; set; }

        /// <summary>
        /// GEOLOCATION_CD
        /// </summary>
        [DataMember]
        public int GEOLOCATION_CD { get; set; }

    }
}
