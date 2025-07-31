using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using EnumUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.QuotationServices.EEProvider.DAOs
{
    public class FileDAO
    {

        /// <summary>
        /// Genera archivo excel amparos
        /// </summary>
        /// <param name="peril"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPeril(List<Peril> peril, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilitiesServices.FileProcessType.ParametrizationPeril;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Peril perils in peril)
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

                    fields[0].Value = perils.Id.ToString();
                    fields[1].Value = perils.Description;
                    fields[2].Value = perils.SmallDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
    }
}
