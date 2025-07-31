// -----------------------------------------------------------------------
// <copyright file="ConditionType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    [Flags]
    public enum ConditionType
    {
        Independent = 1,
        Prefix=2,
        TypeRisk=3,
        TechnicalBranch=4,
        Coverage=5
    }
}
