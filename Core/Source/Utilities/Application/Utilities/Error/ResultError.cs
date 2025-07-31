// -----------------------------------------------------------------------
// <copyright file="ResultError.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.Utilities.Error
{
    public sealed class ResultError<TSuccess, TError> : Result<TSuccess, TError>
    {
        public TError Message { get; }
        public ResultError(TError errorMessage)
        {
            Message = errorMessage;
        }
    }
}
