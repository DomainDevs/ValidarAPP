// -----------------------------------------------------------------------
// <copyright file="AuthorizationAnswerGroup.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    using System;
    using System.Runtime.Serialization;
    using Enums;

    [DataContract]
    public class AuthorizationAnswerGroup
    {
        [DataMember]
        public int GroupPoliciesId { get; set; }
        [DataMember]
        public string DescriptionGroup { get; set; }
        [DataMember]
        public int PoliciesId { get; set; }
        [DataMember]
        public string DescriptionPolicie { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public DateTime DateRequest { get; set; }
        [DataMember]
        public bool Required { get; set; }

        [DataMember]
        public string DescriptionRequest { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int UserAnswerId { get; set; }

        [DataMember]
        public int HierarchyAnswerId { get; set; }

        [DataMember]
        public string DescriptionAnswer { get; set; }

        [DataMember]
        public DateTime? DateAnswer { get; set; }

        /// <summary>
        /// Atributo para la propiedad FunctionType.
        /// </summary>
        [DataMember]
        public TypeFunction FunctionType { set; get; }
    }
}