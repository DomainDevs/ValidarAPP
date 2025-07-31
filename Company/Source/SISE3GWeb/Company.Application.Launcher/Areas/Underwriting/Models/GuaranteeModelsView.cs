namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class GuaranteeModelsView
    {        
        /// <summary>
        /// Identificador
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>        
        public string Description { get; set; }

        /// <summary>
        /// Valor
        /// </summary>        
        public string Amount { get; set; }

        /// <summary>
        /// Esta abierta?
        /// </summary>        
        public bool IsOpen { get; set; }

        /// <summary>
        /// Estado
        /// </summary>        
        public string Status{ get; set; }
    }
}