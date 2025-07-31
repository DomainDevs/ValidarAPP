class UnderwritingRequest {

    static GetModuleDateIssue() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetModuleDateIssue',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBranches() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRoundRate(rateDecimal, rate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetRoundRate',
            dataType: "json",
            data: JSON.stringify({ rateDecimal: rateDecimal, rate: rate }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSalePointsByBranchId(branchId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetSalePointsByBranchId',
            data: JSON.stringify({ branchId: branchId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAgentsByDescription(agentId, description, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAgenciesByDesciption',
            data: JSON.stringify({ agentId: agentId, description: description, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetProductsByAgentIdPrefixId(agentId, prefixId, isCollective) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetProductsByAgentIdPrefixId',
            data: JSON.stringify({ agentId: agentId, prefixId: prefixId, isCollective: isCollective }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyTypesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPolicyTypesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrenciesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetCurrenciesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultValues() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetDefaultValues',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultAgent() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetDefaultAgent',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHolders(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ConvertProspectToInsured(temporalId, individualId, documentNumber, controllerRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/' + controllerRisk + '/ConvertProspectToInsured',
            data: JSON.stringify({ temporalId: temporalId, individualId: individualId, documentNumber: documentNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveTemporal(policyData, dynamicProperties, polities) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/CompanySaveTemporal',
            data: JSON.stringify({ policyModel: policyData, dynamicProperties: dynamicProperties, polities: polities}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTitle(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTitle',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParameterByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetParameterByDescription',
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUserAgenciesByAgentIdDescription(agentId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetUserAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUserAgenciesByAgentId(agentId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetUserAgenciesByAgentId',
            data: JSON.stringify({ agentId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExistProductAgentByAgentIdPrefixIdProductId(agentId, prefixId, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/ExistProductAgentByAgentIdPrefixIdProductId',
            data: JSON.stringify({ agentId: agentId, prefixId: prefixId, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetExchangeRateByCurrencyId(currencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetExchangeRateByCurrencyId',
            data: JSON.stringify({ currencyId: currencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetIndividualDetailsByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/CompanyGetNotificationByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgencyByAgentIdAgencyId(agentId, agencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAgencyByAgentIdAgencyId',
            data: JSON.stringify({ agentId: agentId, agencyId: agencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static RunRulesPolicyPre(policyData, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/RunRulesPolicyPre',
            data: JSON.stringify({ policyModel: policyData, dynamicConcepts: dynamicProperties }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateRisks(temporalId, controllerRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/' + controllerRisk + '/UpdateRisks',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExistRiskByTemporalId(tempId, controllerRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/' + controllerRisk + '/ExistRiskByTemporalId',
            data: JSON.stringify({ tempId: tempId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateAvailableAmountByTemporalId(temporalId, controllerRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/' + controllerRisk + '/ValidateAvailableAmountByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateAvailableTextRiskByTemporalId(temporalId, controllerRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/' + controllerRisk + '/ValidateAvailableTextRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateAvailableProspectRiskByTemporalId(temporalId, controllerRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/' + controllerRisk + '/ValidateAvailableProspectRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBillingGroupByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetBillingGroupByDescription',
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoRequestByRequestIdDescription(description, billingGroupId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetCoRequestByRequestIdDescription',
            data: JSON.stringify({ description: description, billingGroupId: billingGroupId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteTemporalByOperationId(operationId, documentNum, prefixId, branchId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/DeleteTemporalByOperationId',
            data: JSON.stringify({
                operationId: operationId,
                documentNum: documentNum, prefixId: prefixId, branchId: branchId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixesByAgentId(agentId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPrefixesByAgentIdAgents',
            data: JSON.stringify({ agentId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdatePolicyComponents(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/UpdatePolicyComponents',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAuthorizedPolicies() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAuthorizedPolicies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetJustificationSarlaft() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetJustificationSarlaft',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateGetSarlaft(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/ValidateGetSarlaft',
            async: false,
            data: JSON.stringify({ IndividualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateGetSarlaftsExoneration(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetJustificationSarlaft',
            data: JSON.stringify({ IndividualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadProductForm(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/GetProductForm",
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //Printing

    static GenerateReportTemporary(tempId, prefixId, operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GenerateReportTemporary',
            data: JSON.stringify({ temporaryId: tempId, prefixId: prefixId, riskSince: 0, riskUntil: 0, operationId: operationId, tempAuthorization: false }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static PrintReportFromOutside(branchId, prefixId, policyNumber) {

        var dfd = jQuery.Deferred();

        $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetEndorsementsByFilterPolicy',
            data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done((data) => {
            if (data.success) {
                $.ajax({
                    type: 'POST',
                    url: rootPath + 'Printing/Printer/GenerateReportPolicyToEndoso',
                    data: JSON.stringify({ policyId: data.result[0].PolicyId, endorsementId: data.result[0].Id, prefixId: prefixId }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                }).done((data) => {
                    dfd.resolve(data);
                }).fail((jqXHR, textStatus, errorThrown) => {
                    dfd.reject(jqXHR, textStatus, errorThrown);
                });
            }
        }).fail((jqXHR, textStatus, errorThrown) => {
            dfd.reject({ success: false, result: AppResources.FailGetEndorsements });

        });
        return dfd.promise();
    }

    static LoadPolicyCombos(policyData, isCollective) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPolicyLists',
            data: JSON.stringify({ policyModel: policyData, isCollective: isCollective }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPoliciesTemporal(Id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/Policies/GetPoliciesTemporalByTemporalId',
            data: JSON.stringify({ id: Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ValidateMainInsured(RisksInsured) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/ValidateMainInsured',
            data: JSON.stringify({ riskInsured: RisksInsured }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateProspect(TempId, IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/UpdateProspect',
            data: JSON.stringify({ operationId: TempId, individualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCompanyBusinessByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetCompanyBusinnesByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDecimalByProductIdCurrencyId(productId, currencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetDecimalByProductIdCurrencyId',
            data: JSON.stringify({ productId: productId, currencyId: currencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteRisk(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/DeleteRiskByTemporalId',
            data: JSON.stringify({ temporalId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParticipantEconomicGroupByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetParticipantEconomicGroupByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParticipantConsortiumOrConsortiumByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetParticipantConsortiumOrConsortiumByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetOperatingQuotaEventByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetOperatingQuotaEventByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class TextRequest {
    static GetTextsByNameLevelIdConditionLevelId(name, levelId, conditionLevelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTextsByNameLevelIdConditionLevelId',
            data: JSON.stringify({ name: name, levelId: levelId, conditionLevelId: conditionLevelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveTexts(temporalId, textModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SaveTexts',
            data: JSON.stringify({ temporalId: temporalId, textModel: textModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveTextsRisk(riskId, textModel, riskController) {
        return $.ajax({
            type: "POST",
            url: rootPath + riskController + '/SaveTexts',
            data: JSON.stringify({ riskId: riskId, textModel: textModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class ClausesRequest {

    static GetClausesByLevelsConditionLevelId(levels, conditionLevelId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetClausesByLevelsConditionLevelId',
            data: JSON.stringify({ levels: levels, conditionLevelId: conditionLevelId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveClauses(temporalId, clauses) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/SaveClauses',
            data: JSON.stringify({ temporalId: temporalId, clauses: clauses }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveClausesRisk(temporalId, riskId, clauses, riskController) {
        return $.ajax({
            type: 'POST',
            url: rootPath + riskController + '/SaveClauses',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, clauses: clauses }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}

class TemporalRequest {
    static GetTemporalByIdTemporalType(id, temporalType, policieId) {

        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTemporalByIdTemporalType',
            data: JSON.stringify({ id: id, temporalType: temporalType, policieId: policieId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateEndorsement(temporalId, policyNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/EndorsementBase/CreateEndorsement',
            data: JSON.stringify({ temporalId: temporalId, policyNumber: policyNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreatePolicy(temporalId, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/CreatePolicy',
            data: JSON.stringify({ temporalId: temporalId, temporalType: temporalType }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTemporalAdvancedSearch(policyAdvancedSearch) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTemporalAdvancedSearch',
            data: JSON.stringify({ policy: policyAdvancedSearch }),
            datatype: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //static ValidationAccessAndHierarchy() {
    //    return $.ajax({
    //        type: "POST",
    //        url: rootPath + 'Underwriting/Underwriting/ValidationAccessAndHierarchy',
    //        dataType: 'json',
    //        contentType: 'application/json; charset=utf-8'
    //    });
    //}

    static DeleteEndorsementControl(Id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/DeleteEndorsementControl',
            data: JSON.stringify({ EndorsementId: Id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}

class QuotationRequest {
    static GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPoliciesByQuotationIdVersionPrefixId',
            data: JSON.stringify({ operationId: quotationId, version: version, prefixId: prefixId, branchId: branchId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateNewVersionQuotation(operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/CreateNewVersionQuotation',
            data: JSON.stringify({ operationId: operationId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTemporalFromQuotation(operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/CreateTemporalFromQuotation',
            data: JSON.stringify({ operationId: operationId }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateQuotation(quotationId, prefixId, branchId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPolicyByQuotationId',
            data: JSON.stringify({ quotationId: quotationId, prefixId: prefixId, branchId: branchId }),
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetPoliciesByPolicy(quotation, issueFrom, issueTo) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPoliciesByPolicy',
            data: JSON.stringify({ policy: quotation, issueFrom: issueFrom, issueTo: issueTo }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetQuotationById(quotation) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetQuotationById',
            //data: JSON.stringify({ policy: quotation, issueFrom: issueFrom, issueTo: issueTo }),
            data: JSON.stringify({ policy: quotation }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class PaymentPlanRequest {
    static GetPaymentPlanIdByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPaymentPlanIdByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetQuotasbyPaymentPlanId(paymentPlanId, summary, currentFrom, currentTo, issueDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetQuotasbyPaymentPlanId',
            data: JSON.stringify({ paymentPlanId: paymentPlanId, summary: summary, currentFrom: currentFrom, currentTo: currentTo, issueDate: issueDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SavePaymentPlan(temporalId, paymentPlan, quotasValues) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SavePaymentPlan',
            data: JSON.stringify({ temporalId: temporalId, paymentPlan: paymentPlan, quotas: quotasValues }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentPlanByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPaymentPlanByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class PremiumFinance {
    static SavePaymentPremiumFinance(temporalId, premiumFinance) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SavePaymentPremiumFinance',
            data: JSON.stringify({ temporalId: temporalId, premiumFinance: premiumFinance }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class CoInsuranceRequest {
    static SaveCoinsurance(coInsurance, assignedCompanies) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SaveCoinsurance',
            data: JSON.stringify({ coInsuranceModel: coInsurance, assignedCompanies: assignedCompanies }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBusinessTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetBusinessTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class BeneficiariesRequest {
    static GetBeneficiariesByDescription(description, insuredSearchType, customerType) {
        customerType = customerType || 1;
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetBeneficiariesByDescription',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerTyp: customerType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveBeneficiaries(temporalId, beneficiaries) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/SaveBeneficiaries',
            data: JSON.stringify({ temporalId: temporalId, beneficiaries: beneficiaries }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetBeneficiaryTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetBeneficiaryTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveBeneficiariesRisk(riskId, beneficiaries, riskController) {
        return $.ajax({
            type: "POST",
            url: rootPath + riskController + '/SaveBeneficiaries',
            data: JSON.stringify({ riskId: riskId, beneficiaries: beneficiaries }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class UnderwritingAgentRequest {
    static SaveCommissions(temporalId, agencies) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/SaveCommissions',
            data: JSON.stringify({ temporalId: temporalId, agencies: agencies }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCommissions(temporalId, agency, agencies) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetCommissions',
            data: JSON.stringify({ temporalId: temporalId, agency: agency, agencies: agencies }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}

class AdditionalDataRequest {
    static SaveAdditionalDAta(temporalId, additionalDataModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SaveAdditionalDAta',
            data: JSON.stringify({ temporalId: temporalId, additionalDataModel: additionalDataModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveCompanyCorrelativePolicy(temporalId, additionalDataModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SaveCompanyCorrelativePolicy',
            data: JSON.stringify({ temporalId: temporalId, additionalDataModel: additionalDataModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCalculateMinPremiumByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetCalculateMinPremiumByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class SurchargesRequest {
    static GetSurchargesQuotation(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetSurchargesByQuotation',
            data: JSON.stringify({ Id: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveSurcharges(temporalId, surcharges) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/SaveSurcharges',
            data: JSON.stringify({ temporalId: temporalId, surcharges: surcharges }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static DeleteRisks(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/DeleteRiskByTemporalId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class HoldersRequest {

    static GetHoldersByDescription(descp, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetHoldersByDocumentOrDescription',
            data: JSON.stringify({ description: descp, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHoldersByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetHoldersByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class TemporalRiskUnderwriting {

    static GetCompanyEndorsementsByFilterPolicy(BranchId, PrefixId, PolicyNumber, isCurrent) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetCompanyEndorsementsByFilterPolicy?branchId=' + BranchId + '&prefixId=' + PrefixId + '&policyNumber=' + PolicyNumber + '&current=' + isCurrent,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + isCurrent,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetRiskByPolicyId(policyId, tempId, prefixId, temporalType, isCopyRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetRiskByPolicyId',
            data: JSON.stringify({ policyId: policyId, tempId: tempId, prefixId: prefixId, temporalType: temporalType, isCopyRisk: isCopyRisk }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskByTemporald(tempId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetRiskByTemporald',
            data: JSON.stringify({ tempId: tempId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskByTemporalId(tempId, tempPolicyId, prefixId, temporalType, isCopyRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetRiskByTemporalId',
            data: JSON.stringify({ tempId: tempId, tempPolicyId: tempPolicyId, prefixId: prefixId, temporalType: temporalType, isCopyRisk: isCopyRisk }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}