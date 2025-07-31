﻿class SalvageRequest {
     static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Salvage/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

     static GetBranches() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Salvage/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
     }

     static GetPaymentClasses() {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetPaymentClasses',
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetCurrencies() {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetCurrencies',
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetCancellationReasons() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Salvage/GetCancellationReasons',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
     }

     static GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber',
             data: JSON.stringify({ prefixId: prefixId, branchId: branchId, policyDocumentNumber: policyDocumentNumber, claimNumber: claimNumber}),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetPolicyByEndorsementIdModuleType(endorsementId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetPolicyByEndorsementIdModuleType',
             data: JSON.stringify({ endorsementId: endorsementId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetSalvagesByClaimIdSubClaimId(claimId, subClaimId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetSalvagesByClaimIdSubClaimId',
             data: JSON.stringify({ claimId: claimId, subClaimId: subClaimId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetSalvageBySalvageId(salvageId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetSalvageBySalvageId',
             data: JSON.stringify({ salvageId: salvageId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetSalesBySalvageId(salvageId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetSalesBySalvageId',
             data: JSON.stringify({ salvageId: salvageId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static CalculateSaleAgreedPlan(dateStart, totalSale, payments, currencyDescription) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/CalculateSaleAgreedPlan',
             data: JSON.stringify({ dateStart: dateStart, totalSale: totalSale, payments: payments, currencyDescription: currencyDescription }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetSaleBySaleId(saleId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetSaleBySaleId',
             data: JSON.stringify({ saleId: saleId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static ExecuteSalvageOperations(salvage) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/ExecuteSalvageOperations',
             data: JSON.stringify({ salvage: salvage }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static ExecuteSaleOperations(sale, salvageId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/ExecuteSaleOperations',
             data: JSON.stringify({ sale: sale, salvageId: salvageId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static DeleteSalvage(salvageId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/DeleteSalvage',
             data: JSON.stringify({ salvageId: salvageId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static DeleteSale(saleId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/DeleteSale',
             data: JSON.stringify({ saleId: saleId }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }

     static GetClaimsByPolicyId(policyId) {
         return $.ajax({
             type: "POST",
             url: rootPath + 'Claims/Salvage/GetClaimsByPolicyId',
             data: JSON.stringify({ policyId: parseInt(policyId) }),
             dataType: "json",
             contentType: "application/json; charset=utf-8"
         });
     }
}