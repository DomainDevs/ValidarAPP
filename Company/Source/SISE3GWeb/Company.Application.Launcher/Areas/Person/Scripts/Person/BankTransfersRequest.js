class BankTransfersRequest {

    static GetBank() {
        return $.ajax({
            type: 'GET',
            url: rootPath + "Person/Person/GetBanks",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetBankById(bankId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetBankById",
            data: JSON.stringify({ bankId: bankId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAccountType() {
        return $.ajax({
            type: 'GET',
            async: false,
            url: rootPath + "Person/Person/loadAccountTypes",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBankBranches(bankId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetBankBranches",
            data: JSON.stringify({ bankId: bankId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrencies() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetCurrencies",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: 'GET',
            url: rootPath + "Person/Person/GetCountries",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCities(countryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetCities",
            data: JSON.stringify({ countryId: countryId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static SaveBankTransfers(dataBank, accountNumber) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/CreateBankTransfers",
            data: JSON.stringify({ listBankTransfers: dataBank, accountNumber: accountNumber }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    
    static EditBankTransfers(data) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/UpdateBankTransfers",
            data: JSON.stringify({ listBankTransfers: data }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetBankTransfers(individual) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetCompanyBankTransfersByIndividualId",
            data: JSON.stringify({ individualId: individual }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    
}