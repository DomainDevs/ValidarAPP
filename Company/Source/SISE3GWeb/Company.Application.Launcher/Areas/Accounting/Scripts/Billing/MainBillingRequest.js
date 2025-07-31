class MainBillingRequest {
    static GetPersonsByIndividualId(individualId) {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Billing/GetPersonsByIndividualId",
            data: { "individualId": individualId }
        });
    }

    static DeleteTempApplication(tempApplicationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/CancelAppliationReceiptByTempImputationId",
            data: {
                "tempImputationId": tempApplicationId
            }
        });
    }

    static SaveTempImputation(imputationTypeId, sourceCode, individualId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
            data: {
                "imputationTypeId": imputationTypeId,
                "sourceCode": sourceCode,
                "individualId": individualId
            }
        });
    }

    static GetCollectionTo() {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + 'Common/GetPersonTypesByBillEnabled',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIncomeConcept() {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + 'Common/GetIncomeConcepts',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetControlKey(controlName) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/GetControlKey",
            data: { "controlName": controlName},
        });

    }

    static GetApplicationIdByTechnicalTransaction(technicalTransaction) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetApplicationIdByTechnicalTransaction",
            data: { "technicalTransaction": technicalTransaction },
        });

    }

    static GetCurrencies() {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Common/GetCurrencies"
        });
    }

    static SaveBillControl(branchId, accountingDate) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/SaveBillControl",
            data: {
                "branchId": branchId,
                "accountingDate": accountingDate
            }
        });
    }

    static AllowOpenBill(branchId, accoutingDate) {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Billing/AllowOpenBill",
            data: { "branchId": branchId, "accountingDate": accoutingDate }
        });
    }

    static NeedCloseBill(branchId, accoutingDate) {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Billing/NeedCloseBill",
            data: { "branchId": parseInt(branchId), "accountingDatePresent": accoutingDate }
        });
    }

    static GetBranchs() {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Common/GetBranchs"
        });
    }

    static GetBankAccountCompaniesByBankIdCurrencyCode(bankId, currencyCode) {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Billing/GetBankAccountCompaniesByBankIdCurrencyCode",
            data: { "BankId": parseInt(bankId), "CurrencyCode": parseInt(currencyCode) }
        });
    }

    static GetAvaibleCurrencies() {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Billing/GetAvaibleCurrencies"
        });
    }

    static GetAvaibleBanksByCurrencyId(currencyId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/GetAvaibleBanksByCurrencyId",
            data: {
                "currencyId": currencyId
            }
        });
    }

    static GetAvaibleAccoutingBanksByCurrencyIdBankId(currencyId, bankId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/GetAvaibleAccoutingBanksByCurrencyIdBankId",
            data: {
                "currencyId": currencyId,
                "bankId": bankId
            }
        });
    }

    static ValidateListRiskPerson(documentNumber, fullName) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Person/Person/ValidateListRiskPerson",
            data: { "documentNumber": documentNumber, "fullName": fullName },
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetNextSequence() {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Billing/GetPaymentTicketSequence"
        });
    }  
}