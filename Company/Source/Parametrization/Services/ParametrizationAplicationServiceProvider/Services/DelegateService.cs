// -----------------------------------------------------------------------
// <copyright file="DelegateService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@Etriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServiceProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.ParametrizationParamBusinessService;
    using Sistran.Core.Framework.SAF;

    /// <summary>
    ///  DelegateService. Conector de Servicios
    /// </summary>
    public class DelegateService
    {
      // provider interno
      internal static ParametrizationParamBusinessServiceProvider.ParametrizationParamBusinessServiceProvider provider = new ParametrizationParamBusinessServiceProvider.ParametrizationParamBusinessServiceProvider();
        
      //called as a service 
      // internal static IParametrizationParamBusinessService ParametrizationParamBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IParametrizationParamBusinessService>();
    }
}
