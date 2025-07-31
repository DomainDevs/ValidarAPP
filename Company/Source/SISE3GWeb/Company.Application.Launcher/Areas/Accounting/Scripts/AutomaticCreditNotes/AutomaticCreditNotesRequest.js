class AutomaticCreditNotesRequest {

    static GetNotesCredit(statusId) {
        return $.ajax({
            type: "POST",
            url: ACC_ROOT + 'AutomaticCreditNotes/GetProcessByStatus?statusId=' + statusId,
            //data: JSON.stringify({
            //    "statusId": statusId
            //}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}