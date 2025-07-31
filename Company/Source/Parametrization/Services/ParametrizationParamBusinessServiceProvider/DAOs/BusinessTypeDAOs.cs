// -----------------------------------------------------------------------
// <copyright file="BusinessTypeDAOs.cs" company="SISTRAN">
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

    /// <summary>
    /// BusinessTypeDAOs. Clase static que reliza las diferenes funcionalidades desde  CORE.DAF_BD.
    /// </summary>
    public class BusinessTypeDAOs
    {
        /// <summary>
        /// GetAllBusinessType. Gets los tipos de negocio en listado Company
        /// </summary>
        /// <returns></returns>
        public static  List<CompanyParamBusinessType> GetAllBusinessType()
        {
            List<CompanyParamBusinessType> companyParamBusinessType = null;
            try
            {
                BusinessCollection businessCollection;
                using (var daf = DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.BusinessType)))
                {
                    businessCollection = new BusinessCollection(daf);
                }
              
                companyParamBusinessType = ModelAssembler.CreateBusinessTypes(businessCollection);
                return companyParamBusinessType;

            }
            catch (System.Exception ex)
            {
               throw new System.Exception("Excepcion en " + ex.TargetSite.ReflectedType + "." + ex.TargetSite.Name, ex);
            }
          
        }


    }
}
