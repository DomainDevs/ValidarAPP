// -----------------------------------------------------------------------
// <copyright file="DelegateService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.UnderwritingParamService;
using Sistran.Company.Application.UnderwritingParamApplicationService;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Services
{
    public class DelegateService
    {
       internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
       internal static IUnderwritingServiceCore UnderwritingServices = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
       internal static IUnderwritingParamServiceWeb UnderwritingParamServices = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingParamServiceWeb>();       
    }
}
