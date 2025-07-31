// -----------------------------------------------------------------------
// <copyright file="CompanyCoClintonList.cs" company="Sistran">
// Copyright (c). All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CompanyCoClintonList
    {
        [DataMember]
        public string IdentificationTypeCode { set; get; }

        [DataMember]
        public string IdentificationNro { set; get; }

        [DataMember]
        public string Descripction { set; get; }

        [DataMember]
        public int? CausalCode { set; get; }

        [DataMember]
        public string IncomeDate { set; get; }

        [DataMember]
        public string ModifierUserCode { set; get; }

        [DataMember]
        public DateTime? LastUpdateDate { set; get; }

        [DataMember]
        public string HourLastUpdate { set; get; }
    }
}