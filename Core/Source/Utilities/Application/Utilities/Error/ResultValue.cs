// -----------------------------------------------------------------------
// <copyright file="ResultValue.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.Utilities.Error
{
    public sealed class ResultValue<TSuccess, TError> : Result<TSuccess, TError>
    {
        public TSuccess Value { get; }
        public ResultValue(TSuccess newValue)
        {
            Value = newValue;
        }
    }
}
