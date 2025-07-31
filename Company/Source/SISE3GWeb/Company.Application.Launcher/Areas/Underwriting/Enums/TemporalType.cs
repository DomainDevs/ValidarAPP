using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Enums
{
    public class Enums
    {
        [Flags]
        public enum TemporalType
        {
            [EnumMember]
            Quotation = 1,
            [EnumMember]
            Policy = 2,
            [EnumMember]
            Endorsement = 3,
            [EnumMember]
            TempQuotation = 4
        }

        [Flags]
        public enum InsuredAsType
        {
            [EnumMember]
            Applicant = 1,
            [EnumMember]
            Defendant = 2,
            [EnumMember]
            LabelOthers = 3
        }

        [Flags]
        public enum MenuWorkFlow
        {
            [EnumMember]
            Impresion = 24,
            [EnumMember]
            Emision = 26,
            [EnumMember]
            Entrega = 23
        }

        public enum ParameterEnum
        {
            [EnumMember]
            HardRiskType = 10004,
            [EnumMember]
            FutureSociety = 1009,
            [EnumMember]
            IvaParameter = 6020,
            [EnumMember]
            UniquePolicy = 12150
        }

    }
}