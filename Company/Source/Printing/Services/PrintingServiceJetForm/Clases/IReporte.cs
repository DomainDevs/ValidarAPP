using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    public interface IReporte
    {
        ArrayList File { get; set; }
        string FileName { get; set; }
        string PdfFilePath { get; set; }

        /// <summary>
        /// Ejecuta los sp correspondientes al tipo de reporte para el llenado de tablas con los datos del reporte.
        /// Obtiene los datos del reporte (dataset) para ser colocados en el archivo '.dat'
        /// </summary>
        /// <param name="poliza">Datos de la póliza a imprimir</param>
        /// <param name="paramSp">Parámetros del procedimiento almacenado correspondiente</param>
        /// <param name="coveredRiskType">Tipo de riesgos de la póliza que se va a imprimir</param>
        void create(Policy policy, NameValue[] paramSetSp, int coveredRiskType, int operationId, bool IsCollective);
    }
}
