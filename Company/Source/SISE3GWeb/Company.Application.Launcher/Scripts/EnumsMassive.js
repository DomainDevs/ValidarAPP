
MassiveProcessType = {
    Emission: 1,
    Modification: 2,
    Renewal: 3,
    Cancellation: 4
}

SubMassiveProcessType = {
    MassiveEmissionWithRequest: 1,
    MassiveEmissionWithoutRequest: 2,
    CollectiveEmission: 3,
    Inclusion: 4,
    Exclusion: 5,
    MassiveRenewal: 6,
    CollectiveRenewal: 7,
    Cancellation: 8
}

CancellationLoadType = {
    Massive: 8
}

MassiveLoadStatus = {
    Validating: 1,
    Validated: 2,
    Tariffing: 3,
    Tariffed: 4,
    Issuing: 5,
    Issued: 6
}

RenewalStatusType =
    {
        Temporals: 6,
        Renewals: 7
    }

MenuTypeGrouping =
    {
        REQUESTGROUPING: 0,
        AGENTS: 1,
        GROUP: 2
    }
