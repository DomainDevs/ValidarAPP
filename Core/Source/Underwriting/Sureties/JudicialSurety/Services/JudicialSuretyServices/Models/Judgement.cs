using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models
{
    [DataContract]
    public class Judgement : BaseJudgement
    {
        /// <summary>
        /// Gets or sets the risk.
        /// </summary>
        /// <value>
        /// The risk.
        /// </value>
        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Datos del Articulo
        /// </summary>
        [DataMember]
        public Article Article { get; set; }

        /// <summary>
        /// Datos del Tipo de Juzgado
        /// </summary>
        [DataMember]
        public Court Court { get; set; }

        /// <summary>
        /// Datos de la Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }


        [DataMember]
        public List<Guarantee> Guarantees { get; set; }

        [DataMember]
        public Attorney Attorney { get; set; }

    }
}
