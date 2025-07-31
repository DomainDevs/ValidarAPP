class ClaimRequest {
    static GetClaimByClaimId(claimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimByClaimId',
            data: JSON.stringify({ claimId: claimId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCausesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCausesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }  

    static GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdPolicyNumber(prefixId, branchId, coveredRiskTypeId, policyNumber, claimDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdPolicyNumber',
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, coveredRiskTypeId: coveredRiskTypeId, policyNumber: policyNumber, claimDate: claimDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyByEndorsementIdModuleType(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetPolicyByEndorsementIdModuleType',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDamageTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetDamageTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetDamageResponsibilities() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetDamageResponsibilities',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAnalyst() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetAnalyst',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAdjuster() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetAdjuster',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInvestigator() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetInvestigator',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, StateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCatastrophesByDescription(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCatastrophesByDescription',
            data: JSON.stringify({ query: query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAccountingDate() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetAccountingDate',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrency() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetCurrency',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimSupplierByClaimId(claimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimSupplierByClaimId',
            data: JSON.stringify({ claimId: claimId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimCatastrophicInformationByClaimId(claimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimCatastrophicInformationByClaimId',
            data: JSON.stringify({ claimId: claimId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEstimationTypesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetEstimationTypesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(claimModifyId, prefixId, coverageId, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId',
            data: JSON.stringify({ claimModifyId: claimModifyId, prefixId: prefixId, coverageId: coverageId, individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimsByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimsByPolicyId',
            data: JSON.stringify({ policyId: parseInt(policyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateParticipant(claimParticipantDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/CreateParticipant',
            data: JSON.stringify({claimParticipantDTO: claimParticipantDTO}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParticipantsByDescriptionInsuredSearchTypeCustomerType(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetParticipantsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ query: query}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetThirdPartyByDescriptionInsuredSearchType(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetThirdPartyByDescriptionInsuredSearchType',
            data: JSON.stringify({ query: query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAmountType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetAmountType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetMinimumSalaryByYear(year) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetMinimumSalaryByYear',
            data: JSON.stringify({ year: year }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetJudicialDecisionDateIsActiveByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetJudicialDecisionDateIsActiveByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEstimationTypesSalariesEstimation() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetEstimationTypesSalariesEstimation',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultCountry() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetDefaultCountry',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetExchangeRate(currency) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/Claim/GetExchangeRate",
            data: JSON.stringify({ currencyId: currency }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimsByPolicyIdOccurrenceDate(policyNumber, claimDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimsByPolicyIdOccurrenceDate',
            data: JSON.stringify({ policyId: policyNumber, occurrenceDate: claimDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}
