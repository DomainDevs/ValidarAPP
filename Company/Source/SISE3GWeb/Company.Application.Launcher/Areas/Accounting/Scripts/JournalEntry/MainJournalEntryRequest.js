class MainJournalEntryRequest {
    static UpdateTempJournalEntry(journalEntryModel)
    {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "JournalEntry/UpdateTempJournalEntry",
            data: { "journalEntryModel": journalEntryModel}
        });
    }
    static SaveJournalEntryApplication(journalEntryCode, tempImputationId, imputationTypeId, statusId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "JournalEntry/SaveJournalEntryApplication",
            data: { "tempJournalEntryId": journalEntryCode, "tempImputationId": tempImputationId, "imputationTypeId": imputationTypeId, "statusId": statusId}
        });
    }
    static UpdateReceiptApplication(tempImputationId, ImputationTypeJournalEntry, journalEntryCode, comments, statusId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
            data: { "imputationId": tempImputationId, "imputationTypeId": ImputationTypeJournalEntry, "sourceCode": journalEntryCode, "comments": comments, "statusId": statusId }
        });
    }
    static GetDebitsAndCreditsMovementTypes(tempImputationId, amount) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes",
            data: { "tempImputationId": tempImputationId, "amount": amount}
        });
    }
    static GetLatestCurrencyExchangeRate(accountingDate, currencyId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
            data: { "rateDate": accountingDate, "currencyId": currencyId }
        });
    }
    static SaveTempJournalEntry(journalEntryId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "JournalEntry/SaveTempJournalEntry",
            data: { "journalEntryId": journalEntryId }
        });
    }
    static GetTempImputationBySourceCode(imputationTypeId, sourceCode) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
            data: { "imputationTypeId": imputationTypeId, "sourceCode": sourceCode }
        });
    }
    static SaveTempApplication(imputationTypeId, sourceCode, individualId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "JournalEntry/SaveTempApplication",
            data: { "imputationTypeId": imputationTypeId, "sourceCode": sourceCode, "individualId": individualId }
        });
    }
    static DeleteTemporaryApplication(temporals) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "TemporarySearch/DeleteTemporaryApplication",
            data: { "temporals": imputationTypeId}
        });
    }
    static GetSalePointsJournalEntry(branchId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Common/GetSalesPointByBranchId",
            data: { "branchId": branchId }
        });
    }

    static UpdateTempJournalEntry(model) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "JournalEntry/UpdateTempJournalEntry",
            data: JSON.stringify({ "journalEntryModel": model }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveApplication(tempApplicationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/SaveApplication",
            data: {
                "tempApplicationId": tempApplicationId
            }
        });
    }

    static GetListBranchesbyUserName() {
        return $.ajax({
            async: false,
            type: 'GET',
            url: ACC_ROOT + 'Common/GetListBranchesbyUserName'
        });
    }
}