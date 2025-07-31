// -----------------------------------------------------------------------
// <copyright file="ResultUnit.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.Utilities.Error
{
    public sealed class ResultUnit<TSuccess, TError> : Result<TSuccess, TError>
    {
        public ResultUnit()
        {
        }
    }
}
