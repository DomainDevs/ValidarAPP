using Sistran.Core.Application.Extensions;

namespace Sistran.Company.Application.ProductParamService.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseCiaParamAssistanceType : Extension
    {
        /// <summary>
        /// Obtiene o establece el id de la asistencia
        /// </summary>
        public int AssistanceId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del tipo de asistencia
        /// </summary>
        public string Description { get; set; }
    }
}
