// -----------------------------------------------------------------------
// <copyright file="BillingPeriodBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@ETriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Enums;

    /// <summary>
    /// BillingPeriodBusiness. Clase Controladora de las diferentes funcionalidades del negocio expuesto
    /// </summary>
    public class BillingPeriodBusiness
    {
        /// <summary>
        /// GetBusinessBillingPeriod. Obtiene el Listado de BillingPeriod desde la Negocio
        /// </summary>
        /// <returns>List<CompanyParamBillingPeriod></returns>
        public List<CompanyParamBillingPeriod> GetBusinessBillingPeriod()
        {
            return BillingPeriodDAOs.GetAllBillingPeriod();
        }

        /// <summary>
        /// GenerateFileToBillingPeriod. Genera el archivo de reporte en formato excel desde el DAOs
        /// </summary>
        /// <param name="BillingPeriodList"></param>
        /// <param name="fileName"></param>
        /// <returns>v</returns>
        public string GenerateFileToBillingPeriod(List<CompanyParamBillingPeriod> BillingPeriodList, string fileName)
        {
            
            FileDAOs file = new FileDAOs();
            return file.GERERATEFILE(BillingPeriodList, fileName, FileProcessType.ParametrizationBillingPeriod);

        }
    }

}