class MainApplicationReceiptRequest {
    static GetReceiptApplicationInformationByTechnicalTransaction(technicaltransaction) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetReceiptApplicationInformationByTechnicalTransaction",
            data: { "technicaltransaction": technicaltransaction },
        });
    }

    static GetTempImputationBySourceCode(sourceCode) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
            data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": sourceCode }
        });
    }

    static GetDebitsAndCreditsMovementTypes(tempImputationId, amount) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes",
            data: { "tempImputationId": tempImputationId, "amount": amount }
        });
    }

    static SaveTempImputation(imputationTypeId, sourceCode, individualId) {
        return $.ajax({
            c: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
            data: {
                "imputationTypeId": imputationTypeId,
                "sourceCode": sourceCode,
                "individualId": individualId
            }
        });
    }

    static CancelAppliationReceipt(tempImputationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/CancelAppliationReceiptByTempImputationId",
            data: {
                "tempImputationId": tempImputationId
            }
        });
    }

    static SaveTemporalReceipt(imputationId, typeId, sourceCode, comments, status) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
            data: {
                "imputationId": imputationId, "imputationTypeId": typeId,
                "sourceCode": sourceCode, "comments": comments, "statusId": status
            }
        });
    }

    static SaveTemporalReceipt(sourceCode, tempImputationId, typeId, comments, status, applyIndividualId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/SaveReceiptApplication",
            data: {
                "sourceCode": sourceCode,
                "tempImputationId": tempImputationId,
                "imputationTypeId": typeId,
                "comments": comments,
                "statusId": status,
                "individualId": applyIndividualId
            }
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
}