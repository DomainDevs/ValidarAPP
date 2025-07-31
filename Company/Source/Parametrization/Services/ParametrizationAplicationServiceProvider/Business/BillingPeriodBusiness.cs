// -----------------------------------------------------------------------
// <copyright file="BillingPeriodBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationAplicationServiceProvider.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.ParametrizationAplicationServiceProvider.Assemblers;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Core.Framework.BAF;

    /// <summary>
    /// BillingPeriodBusiness. Clase static Controladora de las diferentes funcionalidades del negocio expuesto
    /// </summary>
    public class BillingPeriodBusiness
    {
        /// <summary>
        /// GetApplicationBillingPeriod. Obtiene el Listado de BillingPeriod desde el ParamBusinesService
        /// </summary>
        /// <returns>BillingPeriodQueryDTO</returns>
        public static BillingPeriodQueryDTO GetApplicationBillingPeriod()
        {   
           List<CompanyParamBillingPeriod> BillingPeriodList = DelegateService.provider.GetBusinessBillingPeriod();
           BillingPeriodQueryDTO BillingPeriodDTOS = CompanyAplicationAssembler.CreateBillingPeriods(BillingPeriodList);

            if (BillingPeriodDTOS.BillingPeriodQueryDTOs != null && BillingPeriodDTOS.BillingPeriodQueryDTOs.Count == 0)
                BillingPeriodDTOS.Error.ErrorType = Utilities.Enums.ErrorType.NotFound;
            else
            BillingPeriodDTOS.Error.ErrorType = Utilities.Enums.ErrorType.Ok;
                               
                return BillingPeriodDTOS;
        }

        /// <summary>
        /// GenerateFileToBillingPeriods. Genera el archivo de reporte en formato excel desde el ParamBusinesService
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ExcelFileDTO</returns>
        public static ExcelFileDTO GenerateFileToBillingPeriods(string fileName)
        {
          List<CompanyParamBillingPeriod> BillingPeriodList = DelegateService.provider.GetBusinessBillingPeriod();
          ExcelFileDTO File = new ExcelFileDTO
          {
                File = DelegateService.provider.GenerateFileToBusinessBillingPeriod(BillingPeriodList, fileName)
          };

            if (File.File == String.Empty)
                File.ErrorType = Utilities.Enums.ErrorType.NotFound;
            else
                File.ErrorType = Utilities.Enums.ErrorType.Ok;

            return File;
        }

    }
}
