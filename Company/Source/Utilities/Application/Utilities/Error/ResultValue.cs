// -----------------------------------------------------------------------
// <copyright file="ResultValue.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.Utilities.Error
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
