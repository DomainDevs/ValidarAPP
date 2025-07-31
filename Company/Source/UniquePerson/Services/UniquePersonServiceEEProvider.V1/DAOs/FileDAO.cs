using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class FileDAO
    {
        /// <summary>
        /// Genera archivo excel de Tipo de dirección.
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url del archivo generado</returns>
        public string GenerateFileToAddressType(string fileName)
        {
            
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.AddressType;
            CompanyAddressTypeDAO companyAddressTypeDAO = new CompanyAddressTypeDAO();
            List<CompanyAddressType> listAddressType = companyAddressTypeDAO.GetAllCompanyAddressType();

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (CompanyAddressType companyAddressType in listAddressType)
                {
                    ///<summary>
                    ///Se realiza un ordenamiento de columnas utilizando el código OrderBy(x => x.Order) al momento de estar armando la tabla, 
                    ///para corregir los inconvenientes presentados por encabezados de tabla en desorden al generar el archivo Excel. 
                    ///</summary>
                    ///<author>Diego Leon</author>
                    ///<date>17/07/2018</date>
                    ///<purpose>REQ_#020</purpose>
                    ///<returns></returns>
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

                    fields[0].Value = companyAddressType.AddressTypeCode.ToString();
                    fields[1].Value = companyAddressType.SmallDescription;
                    fields[2].Value = companyAddressType.TinyDescription;
                    if (companyAddressType.IsElectronicMail == true)
                    {
                        fields[3].Value = "Correo Electrónico";
                    }
                    else
                    {
                        fields[3].Value = "";
                    }
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

        /// <summary>
        /// Genera archivo excel de Tipo de teléfono.
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url del archivo generado</returns>
        public string GenerateFileToPhoneType(string fileName)
        {
            
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.PhoneType;
            CompanyPhoneTypeDAO companyPhoneTypeDAO = new CompanyPhoneTypeDAO();
            List<CompanyPhoneType> listPhoneType = companyPhoneTypeDAO.GetAllCompanyPhoneType();

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (CompanyPhoneType companyPhoneType in listPhoneType)
                {
                    ///<summary>
                    ///Se realiza un ordenamiento de columnas utilizando el código OrderBy(x => x.Order) al momento de estar armando la tabla, 
                    ///para corregir los inconvenientes presentados por encabezados de tabla en desorden al generar el archivo Excel. 
                    ///</summary>
                    ///<author>Diego Leon</author>
                    ///<date>17/07/2018</date>
                    ///<purpose>REQ_#031</purpose>
                    ///<returns></returns>
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

                    fields[0].Value = companyPhoneType.PhoneTypeCode.ToString();
                    fields[1].Value = companyPhoneType.SmallDescription;
                    fields[2].Value = companyPhoneType.Description;
                    fields[3].Value = companyPhoneType.RegExpression;
                    fields[4].Value = companyPhoneType.ErrorMessage;
                    if (companyPhoneType.IsCellphone == true)
                    {
                        fields[5].Value = "Es Celular";
                    }
                    else
                    {
                        fields[5].Value = "";
                    }
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
