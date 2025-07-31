// -----------------------------------------------------------------------
// <copyright file="BusinessTypeBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationAplicationServiceProvider.Business
{
    using System;
    using System.Collections.Generic;
    using Sistran.Company.Application.ParametrizationAplicationServiceProvider.Assemblers;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs;
    using Sistran.Company.Application.Utilities.DTO;

    /// <summary>
    ///  BusinessTypeBusiness. Clase static Controladora de las diferentes funcionalidades del negocio expuesto
    /// </summary>
    public class BusinessTypeBusiness
    {

        /// <summary>
        /// GetApplicationBusinessType. Obtiene el Listado de BusinessType desde el ParamBusinesService
        /// </summary>
        /// <returns> BusinessTypeQueryDTO </returns>
        public static BusinessTypeQueryDTO GetApplicationBusinessType()
        {  
            List<CompanyParamBusinessType> BusinessTypeList = DelegateService.provider.GetBusinessBusinessType();
            BusinessTypeQueryDTO BusinessTypeDTOS = CompanyAplicationAssembler.CreateBusinessTypes(BusinessTypeList);

            if (BusinessTypeDTOS.BusinessTypeQueryDTOs != null && BusinessTypeDTOS.BusinessTypeQueryDTOs.Count == 0)
                BusinessTypeDTOS.Error.ErrorType = Utilities.Enums.ErrorType.NotFound;
            else
                BusinessTypeDTOS.Error.ErrorType = Utilities.Enums.ErrorType.Ok;

           return BusinessTypeDTOS;

        }

        /// <summary>
        /// GenerateFileToBusinessTypes. Genera el archivo de reporte en formato excel desde el ParamBusinesService
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ExcelFileDTO</returns>
        public static ExcelFileDTO GenerateFileToBusinessTypes(string fileName)
        {
            List<CompanyParamBusinessType> BusinessTypeList = DelegateService.provider.GetBusinessBusinessType();
            ExcelFileDTO File = new ExcelFileDTO
            {
                File = DelegateService.provider.GenerateFileToBusinessBusinessType(BusinessTypeList, fileName)
            };

            if (File.File == String.Empty)
                File.ErrorType = Utilities.Enums.ErrorType.NotFound;
            else
                File.ErrorType = Utilities.Enums.ErrorType.Ok;

            return File;
        }

    }
}
