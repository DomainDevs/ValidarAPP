// -----------------------------------------------------------------------
// <copyright file="IUniqueUserParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniqueUserParamService
{
    using System.ServiceModel;

    /// <summary>
    /// Defines the <see cref="IUniqueUserParamServiceWeb" />
    /// </summary>
    [ServiceContract]
    public interface IUniqueUserParamServiceWeb
    {
        [OperationContract]
        string Prueba();
    }
}
