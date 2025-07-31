// -----------------------------------------------------------------------
// <copyright file="CompanyCoOnuList.cs" company="Sistran">
// Copyright (c). All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CompanyCoOwnList
    {
        [DataMember]
        public string IdentificationNro { set; get; }

        [DataMember]
        public string Description { set; get; }
    }
}