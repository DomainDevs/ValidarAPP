// -----------------------------------------------------------------------
// <copyright file="ErrorModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.Utilities.Error
{
    using Sistran.Core.Application.Utilities.Enums;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ErrorModel
    {
        readonly List<string> _errorDescription;
        [DataMember]
        public List<string> ErrorDescription { get { return this._errorDescription; } }

        readonly ErrorType _errorType;

        [DataMember]
        public ErrorType ErrorType { get { return _errorType; } }

        readonly Exception _exception;

        [DataMember]
        public Exception Exception { get { return _exception; } }

        private ErrorModel(List<string> errorDescription, ErrorType errorType, Exception exception)
        {
            _errorDescription = errorDescription;
            _errorType = errorType;
            _exception = exception;
        }

        public static ErrorModel CreateErrorModel(List<string> errorDescription, ErrorType errorType, Exception exception)
        {
            return new ErrorModel(errorDescription, errorType, exception);
        }
    }
}
