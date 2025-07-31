using Sistran.Company.Application.ModelServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.CommonParamService.EEProvider.DAOs
{
    public class CompanyFileDAO
    {
        /// <summary>
        /// Genera archivo excel ramo comercial
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPrefix(List<CompanyPrefix> Prefix, string fileName)
        {
            FileDAO commonFileDAO = new FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationPrefix;

            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (CompanyPrefix Prefixes in Prefix)
                {
                    if (Prefixes.LineBusiness != null && Prefixes.LineBusiness.Count > 0)
                    {
                        foreach (LineBusiness linebusiness in Prefixes.LineBusiness)
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
                            fields[0].Value = Prefixes.Id.ToString();
                            fields[1].Value = Prefixes.Description;
                            fields[2].Value = Prefixes.SmallDescription;
                            fields[3].Value = Prefixes.TinyDescription;
                            fields[4].Value = Prefixes.PrefixType.Description;
                            fields[5].Value = linebusiness.Description;

                            rows.Add(new Row
                            {
                                Fields = fields,
                            });
                        }
                    }
                    else
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x=> x.Order).Select(x => new Field
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
                        fields[0].Value = Prefixes.Id.ToString();
                        fields[1].Value = Prefixes.Description;
                        fields[2].Value = Prefixes.SmallDescription;
                        fields[3].Value = Prefixes.TinyDescription;
                        fields[4].Value = Prefixes.PrefixType.Description;
                        fields[5].Value = " ";

                        rows.Add(new Row
                        {
                            Fields = fields,
                        });
                    }
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
