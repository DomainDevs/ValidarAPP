// -----------------------------------------------------------------------
// <copyright file="BillingPeriodDAOs.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@ETriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs
{
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using COMMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Assemblers;
using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
using Sistran.Core.Framework.BAF;



    /// <summary>
    /// BillingPeriodDAOs. Clase static que reliza las diferenes funcionalidades desde  CORE.DAF_BD.
    /// </summary>
    public class BillingPeriodDAOs
    {
        /// <summary>
        /// GetAllBillingPeriod. Gets los periodos de facturacion en listado Company 
        /// </summary>
        /// <returns> List<CompanyParamBillingPeriod></returns>
        public static  List<CompanyParamBillingPeriod> GetAllBillingPeriod()
        {
            List<CompanyParamBillingPeriod> companyParamBillingPeriod = null;
            try
            {
                BusinessCollection businessCollection;
                using (var daf = DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.BillingPeriod)))
                {
                    businessCollection = new BusinessCollection(daf);
                }

                companyParamBillingPeriod = ModelAssembler.CreateBillingPeriods(businessCollection);
                return companyParamBillingPeriod;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Excepcion en "+ ex.TargetSite.ReflectedType+"." + ex.TargetSite.Name, ex);
               
            }


        }

    }
}
