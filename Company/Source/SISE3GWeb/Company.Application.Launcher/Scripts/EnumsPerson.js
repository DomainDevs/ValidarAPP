TypePerson =
    {
        PersonNatural: 1,
        PersonLegal: 2,
        ProspectNatural: 3,
        ProspectLegal: 4,
        All: 5
    }

IndividualTypePerson =
    {
        Natural: 1,
        Legal: 2
    }
EmailType =
    {
        Home: 1,
        ElectronicBilling: 23
    }
RolType =
    {
        Agent: 1,
        Insured: 2,
        Address: 3,
        Email: 4,
        PaymentMetod: 5,
        Partner: 6,
        TypeConsortiumMembers: 7,
        StaffLabour: 8,
        Sarlaft: 9,
        LegalRepresent: 10,
        BusinessName: 11,
        OperatingQuota: 12,
        People: 13,
        prospectNatural: 14,
        prospectLegal: 15,
        Phone: 16,
        Provider: 17,
        Tax: 18,
        CoInsurer: 19,
        ReInsurer: 20,
        AdditionalData: 21,
        Third: 22,
        Employee: 23,
        BankTranferens: 24



    }
Person =
    {
        New: -1
    }

AgentEnum =
    {
        Agent: 1,
        AgentPrefix: 2,
        AgentAgency: 3,
        AgentRecorAgency: 4,
        AgentRecorPrefix: 5
    }
RolInsured =
    {
        Insured: 1,
        Holder: 2,
        Beneficiary: 3,
        Payer: 4
    }

MethodPaymentEnum = {
    Cash: 1
}

DocumentType = {
    CC: 1,
    NIT: 2
}

TypePartnership = {
    Individual: 1,
    Consortium: 2,
    TemporalUnion:3,
    Future: 4
}

IndividualRol = {
    Insured: 1,
    Intermediary: 2,
    Employee: 3,
    Provider: 4,
    Third: 5,
    Reinsured: 6,
    Coinsurant: 7
}

ParameterTypePartnership = {
    Future: 1009
}
GuaranteeType =
    {
        Mortage: 1,
        Pledge: 2,
        Fixedtermdeposit: 3,
        PromissoryNote: 4,
        Others: 5,
        Actions:6
    };

CommGuarantee =
    {
        Mortage: 4,
        Pledge: 6,
        FixedtermDeposit: 2,
        PromissoryNote: 13,
    Others: 7,
        Actions:9
    };
GenderType =
    {
        Male: 1,
        Female: 2
    }
Country =
    {
        Colombia: 1
    }
ModalListTypePerson =
    {
        Holder: 8
    }
CurrencyType = {
    COP: 0,
    USD: 1,
    JPY: 2,
    EUR: 3
}
InsuredSearchType = {
    DocumentNumber: 1,
    IndividualId: 2
}

EventOperatingQuota = {
    ASSIGN_INDIVIDUAL_OPERATION_QUOTA: 1,
    APPLYENDORSEMENT: 2
}

EventConsortium = {
    INICIAL_EVENT: 0,
    CREATE_CONSORTIUM: 1,
    ASSIGN_INDIVIDUAL_TO_CONSORTIUM: 2,
    MODIFY_INDIVIDUAL_TO_CONSORTIUM: 3,
    DISABLED_INDIVIDUAL_TO_CONSORTIUM: 4
}

EventEconomicGroup = {
    CREATE_ECONOMIC_GROUP: 1,
    ENABLED_INDIVIDUAL_TO_ECONOMIC_GROUP: 2,
    DISABLED_INDIVIDUAL_TO_ECONOMIC_GROUP: 3
}

TypeOfAssociation = {
    INDIVIDUAL: 1,
    CONSORTIUM: 2,
    TEMPORAL_UNION : 3,
    FUTURE_SOCIETY:4
}

RiskListEventEnum = {
    INCLUDED: 1,
    EXCLUDED: 2
}

//AutomaticQuota = {
//    Thid: 1,
//    Utility: 2,
//    Indicator: 3
//}