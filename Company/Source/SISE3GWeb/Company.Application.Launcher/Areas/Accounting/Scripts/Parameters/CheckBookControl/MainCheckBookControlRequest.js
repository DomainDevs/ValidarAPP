class MainCheckBookControlRequest {

    static GetCheckBookControls(accountBankId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/Parameters/GetCheckBookControls',
            data: JSON.stringify({ accountBankId: accountBankId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}