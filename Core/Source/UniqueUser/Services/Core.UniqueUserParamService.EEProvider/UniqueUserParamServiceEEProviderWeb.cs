// -----------------------------------------------------------------------
// <copyright file="UniqueUserParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------
using System.ServiceModel;

namespace Sistran.Core.Application.UniqueUserParamService.EEProvider
{
    /// <summary>
    /// Defines the <see cref="UniqueUserParamServiceEEProviderWeb" />
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniqueUserParamServiceEEProviderWeb : IUniqueUserParamServiceWeb
    {
        public string Prueba()
        {
            return "Hola";
        }

    }
}
