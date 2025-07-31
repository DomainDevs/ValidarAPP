using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base
{
    /// <summary>
    /// Juzgado
    /// </summary>
    [DataContract]
    public  class BaseCourt
    {
        /// <summary>
        /// Id para tipo de Juzgado
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del tipo de juzgado
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Descripcion del tipo de juzgado
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
        /// <summary>
        /// Descripcion del tipo de juzgado
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
