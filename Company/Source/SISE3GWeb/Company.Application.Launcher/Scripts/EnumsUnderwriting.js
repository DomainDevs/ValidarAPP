Rol =
    {
        Insured: 1,
        Contractor: 2
    };

BusinessType =
    {
        Direct: 1,
        Accepted: 2,
        Assigned: 3
    };

CoveredRiskType =
    {
        Vehicles: 1,
        Location: 2,
        Sureties: 7,
        Transport: 8,
        Aeronavigation: 9
    };

SubCoveredRiskType =
    {
        Vehicle: 1,
        ThirdPartyLiability: 2,
        Property: 3,
        Liability: 4,
        Surety: 5,
        JudicialSurety: 6,
        Transport: 7,
        Lease: 8,
        Marine: 9,
        Fidelity: 10,
        FidelityNewVersion: 11
    };

CalculationType =
    {
        Prorrate: 1,
        Direct: 2,
        ShortTerm: 3
    };

RateType =
    {
        Percentage: 1,
        Permilage: 2,
        FixedValue: 3
    };

TypeRisk =
    {
        Caution: 1,
        surety: 2,
        Aircraft: 3,
        Maritime: 4,
        Vehicle: 5,
        Passengers: 6,
        Subtraction: 7,
        BreakingMachinery: 8,
        Fire: 9,
        WeakCurrent: 10,
        RCivil: 11,
        Terrorism: 12,
        Transpot: 13,
        Multirisk: 15
    };

InsuredSearchType =
    {
        DocumentNumber: 1,
        IndividualId: 2,
        CodeIndividual: 3

    };

RouteType = {
    Street: 1
};

Levels =
    {
        General: 1,
        Risk: 2,
        Coverage: 3,
        Component: 4,
        Comission: 5
    };

ConditionLevel =
    {
        Independent: 1,
        Prefix: 2,
        Risk: 3,
        Linebusiness: 4,
        Coverage: 5
    };

EndorsementType =
    {
        Emission: 1,
        Modification: 2,
        Cancellation: 3,
        EffectiveExtension: 5,
        Renewal: 6,
        LastEndorsementCancellation: 22,
        ChangeTerm: 27,
        ChangeAgent: 29,
        CreditNote: 26,
        Adjustment: 25,
        Declaration: 24,
        PolicyHonderChange: 30,
        ChangeConsolidation: 31,
        ChangeCoinsurance: 32
    };

CancellationType =
    {
        BeginDate: 1,
        FromDate: 2,
        Nominative: 3,
        ShortTerm: 4
    }

TemporalType =
    {
        Quotation: 1,
        Policy: 2,
        Endorsement: 3,
        TempQuotation: 4
    }

CoverStatus =
    {
        Modified: 4
    }

RiskStatus =
    {
        Original: 1,
        Included: 2,
        Excluded: 3,
        Rehabilitated: 4,
        Modified: 5,
        NotModified: 6,
        Cancelled: 7
    }

CoverageStatus =
    {
        None: 0,
        Original: 1,
        Included: 2,
        Excluded: 3,
        Modified: 4,
        NotModified: 5,
        Cancelled: 6,
        Rehabilitated: 7
    }

CustomerType = {
    Individual: 1,
    Prospect: 2
}

ControlType = {
    Select: 'UifSelect',
    Datepicker: 'UifDatepicker',
    Input: 'Text'
}

EnumRateType = {
    porcentaje: 1,
    Milaje: 2,
    Importe: 3
}

SarlaftValidationState = {
    NOT_EXISTS: 0,
    EXPIRED: 1,
    OVERCOME: 2,
    ACCURATE: 3,
    PENDING: 4
}

EnumRateTypePrev = {
    PreviCredit10: 38,
    PreviCredit30: 39,
    PreviCredit20: 40,
    PremiumFinancing: 14
}

PrefixCollective = {
    Surety: 2,
    Vehicle: 7,
    Liability: 29
}

DeclaredPeriod = {
    MENSUAL: 1,
    BIMENSUAL: 2,
    TRIMESTRAL: 3,
    CUATRIMESTRAL: 4,
    SEMESTRAL: 5,
    ANUAL: 6
};

AdjustPeriod = {
    MENSUAL: 3,
    BIMESTRAL: 4,
    TRIMESTRAL: 5,
    CUATRIMESTRAL: 6,
    SEMESTRAL: 7,
    SEGUN_VIGENCIA: 8,
    ANUAL: 9
};

CountryEnumType = {
    Colombia: 1
}

PrefixType = {
    INCENDIO: 3,
    SUSTRACCION: 4,
    ROTURA_DE_MAQUINARIA: 81,
    CORRIENTE_DEBIL: 83,
    TRANSPORTE: 5,
    RESPONSABILIDA_CIVIL: 15,
    CUMPLIMIENTO: 30,
    CAUCION_JUDICIAL:31,
    ARRENDAMIENTO: 32


}

PolicyOrigin = {
    Individual: 1,
    Massive: 2,
    Collective: 3
}

HolderType = {
    Generador: 1,
    Transportador: 2
}

EndorsementMovements =
    {
        Search: 'Search',
        EndorsementChangeText: 'ModificationText',
        EndorsementModificationClause: "ModificationClauses",
        Extension: "Extension",
        Renewal: "Renewal",
        Modification: "Modification",
        Cancellation: "Cancellation",
        ChangeTerm: "ChangeTerm",
        ChangeAgent: "ChangeAgent",
        CreditNote: "CreditNote",
        Reversion: "Reversion",
        Billing: "Billing",
        Printer: "Printer",
        Adjustment: "Adjustment",
        Declaration: "Declaration",
        ChangePolicyHolder: "ChangePolicyHolder",
        ChangeConsolidation: "ChangeConsolidation",
        ChangeCoinsurance: "ChangeCoinsurance"
    }

EndorsementController =
    {
        Modification: "Modification",
        Extension: "Extension",
        Renewal: "Renewal",
        Cancellation: "Cancellation",
        ChangeTerm: "ChangeTerm",
        ChangeAgent: "ChangeAgent",
        CreditNote: "TransportCreditNote",
        Reversion: "Reversion",
        Billing: "Billing",
        Printer: "Printer/Printer",
        Adjustment: "Adjustment",
        Declaration: "Declaration",
        ChangePolicyHolder: "ChangePolicyHolder",
        ChangeConsolidation: "ChangeConsolidation",
        ChangeCoinsurance: "ChangeCoinsurance"

    }
FormsEndorsement =
    {
        formAdjustment: "#formAdjustment",
        formTexts: "#formTexts",
        formCancellation: "#formCancellation",
        formChangeAgent: "#formChangeAgent",
        formChangeTerm: "#formChangeTerm",
        formClauses: "#formClauses",
        formCreditNote: "#formCreditNote",
        formDeclaration: "#formDeclaration",
        formRenewal: "#formRenewal",
        formReversion: "#formReversion",
        formExtension: "#formExtension",
        formModification: "#formModification",
        formChangePolicyHolder: "#formChangePolicyHolder",
        formChangeConsolidation: "#formChangeConsolidation",
        formChangeCoinsurance: "#formChangeCoinsurance"
    }

EnumAgentType = {
    Directo: 1,
    AgenteNatural: 2,
    Agencia: 3,
    Corredores: 4
}

enumModificationType =
    {
        Modification: 1,
        Prorogation: 2,
        ModificationProrogation: 3
    }
