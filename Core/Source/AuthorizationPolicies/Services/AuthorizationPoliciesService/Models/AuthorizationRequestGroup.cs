// -----------------------------------------------------------------------
// <copyright file="AuthorizationRequestGroup.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    using Enums;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class AuthorizationRequestGroup
    {
        [DataMember]
        public int AuthorizationRequestId { get; set; }

        [DataMember]
        public int GroupPoliciesId { get; set; }

        [DataMember]
        public string DescriptionGroup { get; set; }

        [DataMember]
        public int PoliciesId { get; set; }

        [DataMember]
        public string DescriptionPolicie { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public DateTime DateRequest { get; set; }

        [DataMember]
        public string DescriptionRequest { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public TypeStatus Status { get; set; }

        [DataMember]
        public string StatusDescription { get; set; }

        [DataMember]
        public string ProcessDescription { get; set; }

        [DataMember]
        public TypeFunction FunctionType { set; get; }
    }
}