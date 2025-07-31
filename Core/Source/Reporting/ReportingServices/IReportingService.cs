using System.Collections.Generic;
using System.ServiceModel;

// Sistran
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.ReportingServices.Models.Formats;

namespace Sistran.Core.Application.ReportingServices
{
    [ServiceContract]
    public interface IReportingService
    {
        #region Report

        /// <summary>
        /// GenerateReport
        /// Genera el reporte en formato pdf o xls a partir del .rpt
        /// </summary>            
        /// <param name="report"></param>
        /// <returns></returns>
        [OperationContract]
        void GenerateReport(Report report);

      

        /// <summary>
        /// GenerateFileByStructure
        /// Generar el reporte en formato: excel, txt o plantilla de excel
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        [OperationContract]
        void GenerateFileByReport(Report report);
		
        #endregion

        #region MassiveReport

        /// <summary>
        /// GetReportsByUser
        /// Obtiene los reportes masivos generados por el usuario
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns></returns>
        [OperationContract]
        List<MassiveReport> GetMassiveReports(MassiveReport massiveReport);

        /// <summary>
        /// GenerateReportByStoredProcedure
        /// Genera un archivo excel con el resultado de la ejecución de un procedimiento almacenado
        /// </summary>
        /// <param name="report"></param>
        /// <param name="massiveReport"></param>
        [OperationContract]
        void GenerateMassiveReport(Report report, MassiveReport massiveReport);

        /// <summary>
        /// GetTotalRecordsMassiveReport
        /// Obtiene el total de registros del reporte masivo generado por procedimiento almacenado
        /// </summary>
        /// <param name="report"></param>
        [OperationContract]
        int GetTotalRecordsMassiveReport(Report report);

        
        #endregion

        #region Formats

        #region FormatModule

        /// <summary>
        /// SaveFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        [OperationContract]
        FormatModule SaveFormatModule(FormatModule formatModule);

        /// <summary>
        /// UpdateFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        [OperationContract]
        FormatModule UpdateFormatModule(FormatModule formatModule);

        /// <summary>
        /// DeleteFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        [OperationContract]
        FormatModule DeleteFormatModule(FormatModule formatModule);


        /// <summary>
        /// GetFormatModules
        /// </summary>
        /// <returns> List<FormatModule></returns>
        [OperationContract]
        List<FormatModule> GetFormatModules();

        #endregion

        #region Format

        /// <summary>
        /// SaveFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        [OperationContract]
        Format SaveFormat(Format format);

        /// <summary>
        /// UpdateFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        [OperationContract]
        Format UpdateFormat(Format format);

        /// <summary>
        /// DeleteFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        [OperationContract]
        Format DeleteFormat(Format format);


        /// <summary>
        /// GetFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        [OperationContract]
        Format GetFormat(Format format);

        /// <summary>
        /// GetFormatsByFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns> List<Format></returns>
        [OperationContract]
        List<Format> GetFormatsByFormatModule(FormatModule formatModule);

        #endregion

        #region FormatDetail

        /// <summary>
        /// SaveFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        [OperationContract]
        FormatDetail SaveFormatDetail(FormatDetail formatDetail);

        /// <summary>
        /// UpdateFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        [OperationContract]
        FormatDetail UpdateFormatDetail(FormatDetail formatDetail);

        /// <summary>
        /// DeleteFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        [OperationContract]
        FormatDetail DeleteFormatDetail(FormatDetail formatDetail);


        /// <summary>
        /// GetFormatDetailsByFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns> List<FormatDetail></returns>
        [OperationContract]
        List<FormatDetail> GetFormatDetailsByFormat(Format format);

        #endregion

        #region FormatField

        /// <summary>
        /// SaveFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        [OperationContract]
        FormatField SaveFormatField(FormatField formatField);

        /// <summary>
        /// UpdateFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        [OperationContract]
        FormatField UpdateFormatField(FormatField formatField);

        /// <summary>
        /// DeleteFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        [OperationContract]
        FormatField DeleteFormatField(FormatField formatField);


        /// <summary>
        /// GetFormatFieldsByFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns> List<FormatField></returns>
        [OperationContract]
        List<FormatField> GetFormatFieldsByFormatDetail(FormatDetail formatDetail);

        #endregion

        #endregion
    }
}
