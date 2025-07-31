//Sistran Core
using Sistran.Core.Application.ReportingServices.Models.Formats;
using ReportingModels = Sistran.Core.Application.ReportingServices.Models;

//Sistran FWK
using Sistran.Core.Framework.DAF;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Sistran.Core.Application.ReportingServices.Provider.Assemblers
{
    internal static class ModelAssembler
    {
        #region MassiveReport

        /// <summary>
        /// CreateMassiveReport
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns>MassiveReport</returns>
        public static ReportingModels.MassiveReport CreateMassiveReport(Entities.MassiveReport massiveReport)
        {
            return new ReportingModels.MassiveReport()
            {
                Description = massiveReport.Description,
                EndDate = Convert.ToDateTime(massiveReport.EndDate),
                GenerationDate = massiveReport.GenerationDate,
                Id = massiveReport.MassiveReportCode,
                StartDate = massiveReport.StartDate,
                Success = Convert.ToBoolean(massiveReport.Status),
                UrlFile = massiveReport.UrlFile,
                UserId = Convert.ToInt32(massiveReport.UserId)
            };
        }
        
        #endregion

        #region Formats

        #region FormatModule

        /// <summary>
        /// CreateFormatModules
        /// </summary>
        /// <param name="formatModuleCollection"></param>
        /// <returns>List<FormatModule/></returns>
        public static List<FormatModule> CreateFormatModules(BusinessCollection formatModuleCollection)
        {
            List<FormatModule> formatModules = new List<FormatModule>();
            foreach (Entities.FormatModule formatModule in formatModuleCollection.OfType<Entities.FormatModule>())
            {
                formatModules.Add(new FormatModule()
                {
                    Id = formatModule.FormatModuleCode,
                    Description = formatModule.Description,
                });
            }
            return formatModules;
        }

        /// <summary>
        /// CreateFormat
        /// </summary>
        /// <param name="formatCollection"></param>
        /// <returns>Format</returns>
        public static Format CreateFormat(BusinessCollection formatCollection)
        {
            Format format = new Format();

            foreach (Entities.Format formatEntity in formatCollection.OfType<Entities.Format>())
            {
                format.Id = formatEntity.FormatCode;
                format.Description = formatEntity.Description;
                format.FileType = (FileTypes)formatEntity.FileType;
                format.DateFrom = Convert.ToDateTime(formatEntity.DateFrom);
                format.DateTo = Convert.ToDateTime(formatEntity.DateTo);
            }
            return format;
        }

        #endregion

        #endregion
    }
}
