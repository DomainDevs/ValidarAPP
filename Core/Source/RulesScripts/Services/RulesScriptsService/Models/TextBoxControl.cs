using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class TextBoxControl : ConceptControl
    {
        /// <summary>
        /// Maximo de caracteres
        /// </summary>
        [DataMember]
        public int Maxlength { get; set; }
    }
}
