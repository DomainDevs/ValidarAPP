// -----------------------------------------------------------------------
// <copyright file="DelegateService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ProductParamService.EEProvider
{
    using Sistran.Core.Application.RulesScriptsServices;
    using Sistran.Core.Services.UtilitiesServices;
    using Sistran.Core.Framework.SAF;
    using Sistran.Core.Application.CommonService;

    /// <summary>
    /// Delegate Service
    /// </summary>
    public class DelegateService
    {
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IRulesService ruleServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static IScriptsService scriptServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IScriptsService>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();

    }
}
