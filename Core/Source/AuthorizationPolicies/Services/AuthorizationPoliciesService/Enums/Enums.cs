using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Enums
{
    public enum TypePolicies
    {
        [EnumMember]
        Notification = 1,

        [EnumMember]
        Restrictive = 2,

        [EnumMember]
        Authorization = 3,
    }

    public enum TypeStatus
    {
        [EnumMember]
        Pending = 1,

        [EnumMember]
        Authorized = 2,

        [EnumMember]
        Rejected = 3,
    }

    public enum TypeFunction
    {
        [EnumMember]
        Individual = 1,

        [EnumMember]
        Massive = 2,

        [EnumMember]
        Collective = 3,

        [EnumMember]
        Claim = 4,

        [EnumMember]
        PaymentRequest = 5,

        [EnumMember]
        ClaimNotice = 6,

        [EnumMember]
        PersonGeneral = 7,

        [EnumMember]
        PersonInsured = 8,

        [EnumMember]
        PersonProvider = 9,

        [EnumMember]
        PersonThird = 10,

        [EnumMember]
        PersonIntermediary = 11,

        [EnumMember]
        PersonEmployed = 12,

        [EnumMember]
        PersonPersonalInf = 13,

        [EnumMember]
        PersonPaymentMethods = 14,

        [EnumMember]
        PersonGuarantees = 15,

        [EnumMember]
        PersonOperatingQuota = 16,

        [EnumMember]
        PersonTaxes = 17,

        [EnumMember]
        PersonBankTransfers = 18,

        [EnumMember]
        PersonReinsurer = 19,

        [EnumMember]
        PersonCoinsurer = 20,

        [EnumMember]
        PersonConsortiates = 21,

        [EnumMember]
        PersonBusinessName = 22,

        [EnumMember]
        SarlaftGeneral = 23,

        [EnumMember]
        ChargeRequest = 24,

        [EnumMember]
        PersonBasicInfo = 25,

        [EnumMember]
        AutomaticQuota = 26
    }
}
