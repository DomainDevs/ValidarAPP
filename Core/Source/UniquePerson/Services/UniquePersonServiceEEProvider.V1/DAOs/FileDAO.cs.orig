using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using daosUtilitiesServicesProvider = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{

    /// <summary>
    /// Clase para generar archivos a exportar.
    /// </summary>
    public class FileDAO
    {
        /// <summary>
        /// Genera archivo excel de Perfiles de Asegurado.
        /// </summary>
        /// <param name="insuredProfileList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToInsuredProfile(List<InsuredProfile> insuredProfileList, string fileName)
        {
            daosUtilitiesServicesProvider.FileDAO commonFileDAO = new daosUtilitiesServicesProvider.FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationInsuredProfile;

            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (InsuredProfile insuredProfiles in insuredProfileList)
                {
                    var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = insuredProfiles.Id.ToString();
                    fields[1].Value = insuredProfiles.SmallDescription;
                    fields[2].Value = insuredProfiles.Description;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
		
		/// <summary>
        /// Genera archivo excel de Segmentos de Asegurado.
        /// </summary>
        /// <param name="insuredSegmentList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToInsuredSegment(List<InsuredSegmentV1> insuredSegmentList, string fileName)
        {
            daosUtilitiesServicesProvider.FileDAO commonFileDAO = new daosUtilitiesServicesProvider.FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationInsuredSegment;

            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (InsuredSegmentV1 insuredSegments in insuredSegmentList)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = insuredSegments.Id.ToString();
                    fields[1].Value = insuredSegments.ShortDescription;
                    fields[2].Value = insuredSegments.LongDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
    }
}
