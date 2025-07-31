// -----------------------------------------------------------------------
// <copyright file="CompanyType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETRIANA.</author>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonService.Enums
{
    public enum Peripheraltype
    {
        [EnumMember]
        EDI_INF_BASIC=0,

        [EnumMember]
        INF_LAB,

        [EnumMember]
        PAY_MET,

        [EnumMember]
        COUNT_GUAR,

        [EnumMember]
        OPER_QUOT,

        [EnumMember]
        TAXES,

        [EnumMember]
        BANK_TRANSF,

        [EnumMember]
        BUSINSS_NAME
    }
}
