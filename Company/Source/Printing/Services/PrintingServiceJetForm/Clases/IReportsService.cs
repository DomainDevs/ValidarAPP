using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    // Servicio para generar reportes.
    /// <summary>
    /// Servicio para generar reportes.
    /// </summary>
    [ServiceContract]
    public interface IReportsService
    {
        /// <summary>
        /// Genera el reporte para una Poliza o un Temporario.
        /// </summary>
        /// <param name="xmlRequest">Estructura XML con los datos requeridos</param>
        /// <returns>Ruta del reporte</returns>
        [OperationContract]
        string GenerateReport(string xmlRequest);
    }
}
