// Sistran Core
using Sistran.Core.Application.ReportingServices.Models.Formats;
using ReportingModels = Sistran.Core.Application.ReportingServices.Models;

using System;

namespace Sistran.Core.Application.ReportingServices.Provider.Assemblers
{
    internal static class EntityAssembler
    {
        #region MassiveReport

        /// <summary>
        /// CreateMassiveReport
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns></returns>
        public static Entities.MassiveReport CreateMassiveReport(ReportingModels.MassiveReport massiveReport)
        {
            return new Entities.MassiveReport(massiveReport.Id)
            {
                Description = massiveReport.Description,
                EndDate = massiveReport.EndDate,
                GenerationDate = massiveReport.GenerationDate,
                MassiveReportCode = massiveReport.Id,
                StartDate = massiveReport.StartDate,
                Status = Convert.ToInt32(massiveReport.Success),
                UrlFile = massiveReport.UrlFile,
                UserId = massiveReport.UserId,
                ModuleId = massiveReport.ModuleId
            };
        }

        #endregion

        #region Formats

        #region FormatModule
        
        /// <summary>
        /// CreateFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public static Entities.FormatModule CreateFormatModule(FormatModule formatModule)
        {
            return new Entities.FormatModule(formatModule.Id)
            {
                FormatModuleCode = formatModule.Id,
                Description = formatModule.Description,
            };
        }
        
        #endregion
        
        #region Format

        /// <summary>
        /// CreateFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public static Entities.Format CreateFormat(Format format)
        {
            return new Entities.Format(format.Id)
            {
                FormatCode = format.Id,
                FormatModuleCode = format.FormatModule.Id,
                FileType = Convert.ToInt32(format.FileType),
                Description = format.Description,
                DateFrom = format.DateFrom,
                DateTo = format.DateTo
            };
        }

        #endregion
        
        #region FormatDetail
        
        /// <summary>
        /// CreateFormatType
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatType</returns>
        public static Entities.FormatType CreateFormatType(FormatDetail formatDetail)
        {
            return new Entities.FormatType(formatDetail.Id)
            {
                FormatTypeCode = formatDetail.Id,
                FormatCode = formatDetail.Format.Id,
                FormatAreaCode = Convert.ToInt32(formatDetail.FormatType),
                Separator = formatDetail.Separator
            };
        }
        
        #endregion
        
        #region FormatField
        
        /// <summary>
        /// CreateFormatTypeField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatTypeField</returns>
        public static Entities.FormatTypeField CreateFormatTypeField(FormatField formatField)
        {
            return new Entities.FormatTypeField(formatField.Id)
            {
                FormatTypeFieldCode = 0,
                FormatTypeCode = formatField.Id,
                Order = formatField.Order,
                Description = formatField.Description,
                Start = formatField.Start,
                Length = formatField.Length,
                Value = formatField.Value,
                Filled = formatField.Filled,
                Align = formatField.Align,
                ColumnNumber = formatField.ColumnNumber,
                RowNumber = formatField.RowNumber,
                Field = formatField.Field,
                FieldFormat = formatField.Mask,
                IsRequired = formatField.IsRequired
            };
        }
        
        #endregion
        
        #endregion
    }
}
