// -----------------------------------------------------------------------
// <copyright file="DelegateService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider
{
    using Sistran.Core.Application.CommonService;
    using Sistran.Core.Framework.SAF;
    using Sistran.Core.Services.UtilitiesServices;

    /// <summary>
    /// Delegate Service
    /// </summary>
    public class DelegateService
    {
        /// <summary>
        /// Common ServiceCore
        /// </summary>
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}
