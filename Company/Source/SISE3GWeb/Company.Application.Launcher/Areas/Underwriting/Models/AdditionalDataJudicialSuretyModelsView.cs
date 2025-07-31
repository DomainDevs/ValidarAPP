using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class AdditionalDataJudicialSuretyModelsView
    {

        public int RiskId { get; set; }

        /// <summary>
        /// Id Apoderado
        /// </summary>
        public int? AttorneyId { get; set; }

        /// <summary>
        /// Nombre Apoderado
        /// </summary>
        public string AttorneyName { get; set; }

        /// <summary>
        /// tipo de documento
        /// </summary>      
        public int DocumentType { get; set; }
        
      
        public string NumberId { get; set; }

        /// <summary>
        /// Tarjeta profesional
        /// </summary>     
        public string CardProfessional { get; set; }

        /// <summary>
        ///nombre impresion
        /// </summary>

        public string AttorneyNameToPrint { get; set; }
        /// <summary>
        ///nombre asegurado a imprimir
        /// </summary>
        public string InsuredToPrint { get; set; }
    }
}