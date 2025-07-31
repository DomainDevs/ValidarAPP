// -----------------------------------------------------------------------
// <copyright file="FileDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@Etriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Core.Application.Utilities.DataFacade;
    using COMMEN = Sistran.Core.Application.Parameters.Entities;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Assemblers;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using EnumUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Enums;
    using Sistran.Core.Framework.BAF;

    /// <summary>
    /// FileDAOs. Clase genérica encargada de Construir y Generar todos los archivos Excel que presta el servicio.
    /// </summary>
    public class FileDAOs
    {
      
        public String Ruta { get; set; }
        FileDAO TcommonFileDAO = new FileDAO();
        FileProcessValue TfileProcessValue = new FileProcessValue();


        /// <summary>
        /// Clase generica para crear los archivos excel
        /// </summary>
        /// <param name="BusinessTList"></param>
        /// <param name="fileName"></param>
        /// <param name="ProcessType"></param>
        /// <returns></returns>
        public string GERERATEFILE<T>(List<T> BusinessTList, string fileName, FileProcessType ProcessType) where T:CompanyGeneric
        {

            try
            {
                TfileProcessValue.Key1 = (int)ProcessType; 
                File file = TcommonFileDAO.GetFileByFileProcessValue(TfileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (var BusinessType in BusinessTList)
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

                        fields= AssignValues(BusinessType, fields);
                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    return TcommonFileDAO.GenerateFile(file);
                }
                else
                {
                    return string.Empty;
                }

                    
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            
        }

        /// <summary>
        /// Clase par Agregar los parametros de cada proceso.
        /// </summary>
        /// <param name="ItemBillingPeriod"></param>
        /// <param name="Fields"></param>
        /// <returns>List<Field> </returns>
        private List<Field>  AssignValues(CompanyGeneric ItemBusinessType, List<Field> Fields)
        {
            switch (ItemBusinessType)
            {
                case CompanyParamBillingPeriod a:
                    Fields[0].Value = ((CompanyParamBillingPeriod)ItemBusinessType).BILLING_PERIOD_CD.ToString();
                    Fields[1].Value = ((CompanyParamBillingPeriod)ItemBusinessType).Description;
                    break;
                case CompanyParamBusinessType b:
                    Fields[0].Value = ((CompanyParamBusinessType)ItemBusinessType).BUSINESS_TYPE_CD.ToString();
                    Fields[1].Value = ((CompanyParamBusinessType)ItemBusinessType).SMALL_DESCRIPTION;
                    break;
                 default:
                    Fields[0].Value = ItemBusinessType.id.ToString();
                    Fields[1].Value = ItemBusinessType.Description;
                    break;
            }
          
            return Fields; 
        }

        #region city
        /// <summary>
        /// GenerateFileToCity: genera archivo excel con el listado de la ciudades
        /// </summary>     
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToCity(List<CompanyParamCity> CityList, string fileName)
        {
            try
            {
               
                FileProcessValue fileProcessValue = new FileProcessValue();
                fileProcessValue.Key1 = (int)FileProcessType.ParametrizationCity; 

                File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (CompanyParamCity citylist in CityList)
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

                        fields[0].Value = citylist.Id.ToString();
                        fields[1].Value = citylist.Description;
                        fields[2].Value = citylist.Country.Description;
                        fields[3].Value = citylist.State.Description;

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
        #endregion


    }
}
