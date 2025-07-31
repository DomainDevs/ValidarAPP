using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class SearchComboControl:ConceptControl
    {
        /// <summary>
        /// Llave Foranea de la Entidad
        /// </summary>
        [DataMember]
        public int ForeignEntity { get; set; }
    }
}
