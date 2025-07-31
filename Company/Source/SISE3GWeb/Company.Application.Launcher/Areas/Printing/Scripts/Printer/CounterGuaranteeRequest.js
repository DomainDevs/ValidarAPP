class CounterGuaranteeRequest {
    static GetHoldersByDescription(descp, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GetHoldersByDocumentOrDescription',
            data: JSON.stringify({ description: descp, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetHoldersByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GetHoldersByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCounterGuaranteesByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GetCounterGuaranteesByIndividualId',
            data: JSON.stringify({ individualId: individualId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static PrintCounterGuarantees(guaranteeId, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/PrintCounterGuarantees',
            data: JSON.stringify({ guaranteeId: guaranteeId, individualId: individualId,  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}