using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Company.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class FileDAOs
    {
        /// <summary>
        /// GenerateFileToReportAuthorizationPolicies: genera archivo excel con el Reporte de Grupos de politicas
        /// </summary>     
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToReportAuthorizationPolicies(List<CompanyReportPolicy> ReportPolicyList, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue();
                fileProcessValue.Key1 = (int)FileProcessType.ReportAuthorizationPolicies;

                File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (CompanyReportPolicy reportpolicylist in ReportPolicyList)
                    {
                        List<Field> fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
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

                        fields[0].Value = reportpolicylist.idPolicies.ToString();
                        fields[1].Value = reportpolicylist.descriptionPolicy;
                        fields[2].Value = reportpolicylist.userPolicy;
                        fields[3].Value = reportpolicylist.datePolicy.ToString();
                        fields[4].Value = reportpolicylist.prefix;
                        fields[5].Value = reportpolicylist.numberPolicy.ToString();
                        fields[6].Value = reportpolicylist.typeEndosment;
                        fields[7].Value = reportpolicylist.user;
                        fields[8].Value = reportpolicylist.dateAuthorization.ToString();
                        fields[9].Value = reportpolicylist.branch;
                        fields[10].Value = reportpolicylist.groupPolicies;
                        fields[11].Value = reportpolicylist.statusPolicy;
                        fields[12].Value = reportpolicylist.waitingTime.ToString();

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    return DelegateService.utilitiesService.GenerateFile(file);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
