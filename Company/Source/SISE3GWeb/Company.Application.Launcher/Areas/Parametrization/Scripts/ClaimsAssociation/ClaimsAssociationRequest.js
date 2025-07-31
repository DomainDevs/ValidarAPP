class ClaimsAssociationRequest {
    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLinesBusinessByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetLinesBusinessByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubLinesBusinessByLineBusinessId(lineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetSubLinesBusinessByLineBusinessId',
            data: JSON.stringify({ lineBusinessId: lineBusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCausesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetCausesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBussinessId, subLineBussinessId, causeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId',
            data: JSON.stringify({ lineBussinessId: lineBussinessId, subLineBussinessId: subLineBussinessId, causeId: causeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    

    static DeletePaymentConcept(conceptId, coverageId, estimationTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/DeletePaymentConcept',
            data: JSON.stringify({ conceptId: conceptId, coverageId: coverageId, estimationTypeId: estimationTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    
    static GetCoveragesByLineBusinessId(lineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetCoveragesByLineBusinessId',
            data: JSON.stringify({lineBusinessId: lineBusinessId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    static GetEstimationsType() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetEstimationsType',
            data: JSON.stringify({ }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    static GetPaymentConcepts() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/ClaimsAssociation/GetPaymentConcepts",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentConceptsByCoverageIdEstimationTypeId(coverageId, estimationTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/GetPaymentConceptsByCoverageIdEstimationTypeId',
            data: JSON.stringify({coverageId: coverageId, estimationTypeId: estimationTypeId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    static CreatePaymentConcept(coveragePaymentConceptDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsAssociation/CreatePaymentConcept',
            data: JSON.stringify({ coveragePaymentConceptDTO: coveragePaymentConceptDTO}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}