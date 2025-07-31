using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base
{
    /// <summary>
    /// Articulo
    /// </summary>
    [DataContract]
    public class BaseArticle
    {
        /// <summary>
        /// Id para el articulo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del articulo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// SmallDescription del articulo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
