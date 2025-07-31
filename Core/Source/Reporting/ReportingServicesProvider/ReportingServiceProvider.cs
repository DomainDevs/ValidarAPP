// Sistran Core
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.ReportingServices.Provider.DAOs;
using Sistran.Core.Framework.BAF;
using System.Collections.Generic;

namespace Sistran.Core.Application.ReportingServices.Provider
{
    public class ReportingServiceProvider : IReportingService
    {
        #region Constants

        #endregion Constants

        #region Instance Variables

        #region Interfaz

        #endregion Interfaz

        #region DAOs

        private readonly FormatDAO _formatDAO = new FormatDAO();
        private readonly FormatDetailDAO _formatDetailDAO = new FormatDetailDAO();
        private readonly FormatFieldDAO _formatFieldDAO = new FormatFieldDAO();
        private readonly FormatModuleDAO _formatModuleDAO = new FormatModuleDAO();
        private readonly MassiveReportDAO _massiveReportDAO = new MassiveReportDAO();
        private readonly ProcessDAO _processDAO = new ProcessDAO();
        private readonly ReportDAO _reportDAO = new ReportDAO();
        private readonly StructureDAO _structureDAO = new StructureDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region Report

        /// <summary>
        /// GenerateReport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public void GenerateReport(Report report)
        {
            _reportDAO.GenerateReportFile(report);
        }

        /// <summary>
        /// GenerateFileByStructure
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public void GenerateFileByReport(Report report)
        {
            try
            {
                _structureDAO.GenerateStructureReport(report);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region MassiveReport

        /// <summary>
        /// GetMassiveReports
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns>List<MassiveReport></returns>
        public List<MassiveReport> GetMassiveReports(MassiveReport massiveReport)
        {
            try
            {
                return _massiveReportDAO.GetMassiveReportsByUser(massiveReport);
            }
            catch(BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GenerateMassiveReport
        /// </summary>
        /// <param name="report"></param>
        /// <param name="massiveReport"></param>
        /// <returns></returns>
        public void GenerateMassiveReport(Report report, MassiveReport massiveReport)
        {
            try
            {
                _processDAO.GenerateReport(report, massiveReport);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTotalRecordsMassiveReport
        /// </summary>
        /// <param name="report"></param>
        /// <returns>int</returns>
        public int GetTotalRecordsMassiveReport(Report report)
        {
            try
            {
                return _processDAO.GetTotalRecords(report);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Formats

        #region FormatModule

        /// <summary>
        /// SaveFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public FormatModule SaveFormatModule(FormatModule formatModule)
        {
            return _formatModuleDAO.SaveFormatModule(formatModule);
        }

        /// <summary>
        /// UpdateFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public FormatModule UpdateFormatModule(FormatModule formatModule)
        {
            return _formatModuleDAO.UpdateFormatModule(formatModule);
        }

        /// <summary>
        /// DeleteFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public FormatModule DeleteFormatModule(FormatModule formatModule)
        {
            FormatModule deleteFormatModule = new FormatModule();
            // Consulta dependencias para su eliminación
            if (GetExistFormatsByFormatModule(formatModule))
            {
                deleteFormatModule = _formatModuleDAO.DeleteFormatModule(formatModule);
            }
            else
            {
                deleteFormatModule.Description = "relatedObject";
            }

            return deleteFormatModule;
        }

        /// <summary>
        /// GetFormatModules
        /// </summary>
        /// <returns>List<FormatModule></returns>
        public List<FormatModule> GetFormatModules()
        {
            return _formatModuleDAO.GetFormatModules();
        }

        #endregion

        #region Format

        /// <summary>
        /// SaveFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format SaveFormat(Format format)
        {
            return _formatDAO.SaveFormat(format);
        }

        /// <summary>
        /// UpdateFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format UpdateFormat(Format format)
        {
            return _formatDAO.UpdateFormat(format);
        }

        /// <summary>
        /// DeleteFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format DeleteFormat(Format format)
        {
            Format deleteFormat = new Format();
            if (GetExistFormatDetailsByFormat(format))
            {
                deleteFormat = _formatDAO.DeleteFormat(format);
            }
            else
            {
                deleteFormat.Description = "relatedObject";
                deleteFormat.FileType = FileTypes.Excel;
            }

            return deleteFormat;
        }

        /// <summary>
        /// GetFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format GetFormat(Format format)
        {
            return _formatDAO.GetFormat(format);
        }

        /// <summary>
        /// GetFormatsByFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns> List<Format></returns>
        public List<Format> GetFormatsByFormatModule(FormatModule formatModule)
        {
            return _formatDAO.GetFormatsByFormatModule(formatModule);
        }

        #endregion

        #region FormatDetail

        /// <summary>
        /// SaveFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail SaveFormatDetail(FormatDetail formatDetail)
        {
            FormatDetail newFormatDetail = _formatDetailDAO.SaveFormatDetail(formatDetail);

            FormatField formatField = new FormatField();
            foreach (FormatField field in formatDetail.Fields)
            {
                formatField.Id = formatDetail.Id;
                formatField.Description = field.Description;
                formatField.Order = field.Order;
                formatField.Start = field.Start;
                formatField.Length = field.Length;
                formatField.Value = field.Value;
                formatField.Filled = field.Filled;
                formatField.Align = field.Align;
                formatField.RowNumber = field.RowNumber;
                formatField.ColumnNumber = field.ColumnNumber;
                formatField.IsRequired = field.IsRequired;
                SaveFormatField(formatField);
            }

            return newFormatDetail;
        }

        /// <summary>
        /// UpdateFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail UpdateFormatDetail(FormatDetail formatDetail)
        {
            FormatDetail updateFormatDetail = new FormatDetail();

            if (formatDetail.Fields == null)
            {
                updateFormatDetail = _formatDetailDAO.UpdateFormatDetail(formatDetail);
            }
            else
            {
                FormatField formatField = new FormatField();
                foreach (FormatField field in formatDetail.Fields)
                {
                    formatField.Id = field.Id;
                    formatField.Description = field.Description;
                    formatField.Order = field.Order;
                    formatField.Start = field.Start;
                    formatField.Length = field.Length;
                    formatField.Value = field.Value;
                    formatField.Filled = field.Filled;
                    formatField.Align = field.Align;
                    formatField.RowNumber = field.RowNumber;
                    formatField.ColumnNumber = field.ColumnNumber;
                    formatField.IsRequired = field.IsRequired;
                    formatField.Mask = field.Mask;
                    formatField.Field = field.Field;

                    UpdateFormatField(formatField);
                }
                Format format = new Format();
                format.FileType = FileTypes.Text;
                updateFormatDetail.Format = format;
                updateFormatDetail.FormatType = FormatTypes.Head;
            }
            return updateFormatDetail;
        }

        /// <summary>
        /// DeleteFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail DeleteFormatDetail(FormatDetail formatDetail)
        {
            return _formatFieldDAO.DeleteFormatFieldsByFormatDetail(formatDetail);
        }

        /// <summary>
        /// GetFormatDetailsByFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>List<FormatDetail></returns>
        public List<FormatDetail> GetFormatDetailsByFormat(Format format)
        {
            try
            {
                return _formatDetailDAO.GetFormatDetailsByFormat(format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region FormatField

        /// <summary>
        /// SaveFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        public FormatField SaveFormatField(FormatField formatField)
        {
            return _formatFieldDAO.SaveFormatField(formatField);
        }

        /// <summary>
        /// UpdateFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        public FormatField UpdateFormatField(FormatField formatField)
        {
            return _formatFieldDAO.UpdateFormatField(formatField);
        }

        /// <summary>
        /// DeleteFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        public FormatField DeleteFormatField(FormatField formatField)
        {
            return _formatFieldDAO.DeleteFormatField(formatField);
        }

        /// <summary>
        /// GetFormatFieldsByFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns> List<FormatField></returns>
        public List<FormatField> GetFormatFieldsByFormatDetail(FormatDetail formatDetail)
        {
            return _formatFieldDAO.GetFormatFieldsByFormatDetail(formatDetail);
        }

        #endregion

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region Formats

        /// <summary>
        /// GetExistFormatsByFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>bool</returns>
        private bool GetExistFormatsByFormatModule(FormatModule formatModule)
        {
            List<Format> formats = GetFormatsByFormatModule(formatModule);
            if (formats.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// GetExistFormatDetailsByFormat
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>bool</returns>
        private bool GetExistFormatDetailsByFormat(Format formatModule)
        {
            List<FormatDetail> formatDetails = GetFormatDetailsByFormat(formatModule);
            if (formatDetails.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Formats

        #endregion Private Methods

    }

}
