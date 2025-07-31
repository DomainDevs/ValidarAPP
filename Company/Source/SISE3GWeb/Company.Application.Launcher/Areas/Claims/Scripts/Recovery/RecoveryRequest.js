class RecoveryRequest {
    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Recovery/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Recovery/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCancellationReasons() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Recovery/GetCancellationReasons',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRecoveryTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Recovery/GetRecoveryTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrencies() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Recovery/GetCurrencies',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentClasses() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Recovery/GetPaymentClasses',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CalculateRecoveryAgreedPlan(dateStart, totalSale, payments, currencyDescription) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/CalculateRecoveryAgreedPlan',
            data: JSON.stringify({ dateStart: dateStart, totalSale: totalSale, payments: payments, currencyDescription: currencyDescription }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber,claimNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber',
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, policyDocumentNumber: policyDocumentNumber ,claimNumber: claimNumber}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyByEndorsementIdModuleType(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/GetPolicyByEndorsementIdModuleType',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRecoveriesByClaimIdSubClaimId(claimId, subClaimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/GetRecoveriesByClaimIdSubClaimId',
            data: JSON.stringify({ claimId: claimId, subClaimId: subClaimId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteRecoveryOperations(recovery) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/ExecuteRecoveryOperations',
            data: JSON.stringify({ recovery: recovery }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRecoveryByRecoveryId(recoveryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/GetRecoveryByRecoveryId',
            data: JSON.stringify({ recoveryId: recoveryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimsByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/GetClaimsByPolicyId',
            data: JSON.stringify({ policyId: parseInt(policyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAccountingDate() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Recovery/GetAccountingDate',
            data: {},
            dataType: "json",
            contentType: "application/json;charset=utf-8"
        });
    }

}