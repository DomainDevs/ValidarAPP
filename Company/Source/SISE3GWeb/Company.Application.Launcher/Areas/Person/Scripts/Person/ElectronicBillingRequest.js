class ElectronicBillingRequest {
    static GetFiscalResponsibility() {
        return $.ajax({
            type: 'GET',
            url: rootPath + "Person/Person/GetFiscalResponsibility",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveElectronicBilling(data, responsibleForVat, electronicBiller) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/CreateElectronicBilling",
            data: JSON.stringify({
                listInsuredFiscalResponsibility: data, responsibleForVat: responsibleForVat,
                electronicBiller: electronicBiller}),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompanyElectronicBillingByIndividualId(individual) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetCompanyElectronicBillingByIndividualId",
            data: JSON.stringify({ individualId: individual }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetFiscalResponsibilityId(Id) {
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + "Person/Person/GetFiscalResponsibilityById",
            data: JSON.stringify({ Id: Id }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRegimeTypeId(individual) {
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + "Person/Person/GetInsuredByIndividualId",
            data: JSON.stringify({ individualId: individual }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteElectronicBilling(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/DeleteFiscalResponsibility',
            data: JSON.stringify({ "fiscalDto": data }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

   
}