using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Grupo de Coberturas
    /// </summary>
    [DataContract]
    public class GroupCoverage : BaseGroupCoverage
    {
        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        public virtual Product Product { get; set; }

        /// <summary>
        /// Cobertura
        /// </summary>
        [DataMember]
        public virtual Coverage Coverage { get; set; }

        /// <summary>
        /// Cobertura
        /// </summary>
        [DataMember]
        public virtual List<Coverage> Coverages { get; set; }


        /// <summary>
        /// Obtiene o establece el porcentaje del limite
        /// </summary>
        [DataMember]
        public virtual List<InsuredObject> InsuredObjects { get; set; }

    }
}
