// -----------------------------------------------------------------------
// <copyright file="ResultUnit.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.Utilities.Error
{
    public sealed class ResultUnit<TSuccess, TError> : Result<TSuccess, TError>
    {
        public ResultUnit()
        {
        }
    }
}
