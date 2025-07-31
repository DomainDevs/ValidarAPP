class ClaimsPaymentRequest {
    static GetPrefixes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetBranches() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCurrency() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetCurrency',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPaymentSource() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetPaymentSource',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPaymentMovementTypesByPaymentSourceId(sourceId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/PaymentRequest/GetPaymentMovementTypesByPaymentSourceId',
            data: JSON.stringify({ sourceId: sourceId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEstimationsTypesIdByMovementTypeId(movementTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/PaymentRequest/GetEstimationsTypesIdByMovementTypeId',
            data: JSON.stringify({ movementTypeId: movementTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetPersonTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetVoucherType() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetVoucherType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPaymentMethod() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetPaymentMethod',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetModuleDateByModuleTypeMovementDate() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetModuleDateByModuleTypeMovementDate',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(branch, movementType, personTypeId, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId",
            data: JSON.stringify({ brachId: branch, movementTypeId: movementType, personTypeId: personTypeId, individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetExchangeRate(currency) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetExchangeRate",
            data: JSON.stringify({ currencyId: currency }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber,claimNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber",
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, policyDocumentNumber: policyDocumentNumber ,claimNumber: claimNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SavePaymentRequest(paymentRequestModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/SavePaymentRequest",
            data: JSON.stringify({ paymentRequestDTO: paymentRequestModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentRequestByPaymentRequestId(paymentRequestId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetPaymentRequestByPaymentRequestId",
            data: JSON.stringify({ paymentRequestId: paymentRequestId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualTaxesByIndividualIdRoleId(individualId, roleId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetIndividualTaxesByIndividualIdRoleId",
            data: JSON.stringify({ individualId: individualId, roleId: roleId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CalculatePaymentTaxesByIndividualIdAccountingConceptIdBranchIdAmount(individualId, accountingConceptId, branchId, amount)
    {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/PaymentRequest/CalculatePaymentTaxesByIndividualIdAccountingConceptIdBranchIdAmount',
            data: JSON.stringify({
                individualId: individualId,
                accountingConceptId: accountingConceptId,
                branchId: branchId,
                amount: amount
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyByEndorsementIdModuleType(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/PaymentRequest/GetPolicyByEndorsementIdModuleType',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredsByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetInsuredsByIndividualId",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetInsuredByRiskId",
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultPaymentCurrency() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetDefaultPaymentCurrency',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetBranchDescriptionByBranchId(branchId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetBranchDescriptionByBranchId",
            data: JSON.stringify({ branchId: branchId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetPrefixByPrefixId",
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSearchPersonType(prefixId, searchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/GetSearchPersonTypes",
            data: JSON.stringify({ prefixId: prefixId, searchType: searchType}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(individualId, accountingConceptId, attributes, amount) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/PaymentRequest/CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount",
            data: JSON.stringify({
                individualId: individualId,
                accountingConceptId: accountingConceptId,
                attributes: attributes,
                amount: amount
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTaxAttributes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetTaxAttributes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTaxCodeOfRetetionToIndustryAndCommerce() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/PaymentRequest/GetTaxCodeOfRetetionToIndustryAndCommerce',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/PaymentRequest/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/PaymentRequest/GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/PaymentRequest/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetDefaultCountry() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/PaymentRequest/GetDefaultCountry',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}