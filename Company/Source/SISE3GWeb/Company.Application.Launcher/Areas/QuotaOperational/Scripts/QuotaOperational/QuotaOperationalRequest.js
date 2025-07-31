class QuotaOperationalRequestScript {

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetIndividualDetailsByIndividualId(description, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'QuotaOperational/QuotaOperational/GetIndividualDetailsByIndividualId',
            data: JSON.stringify({ individualId: description, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetOperatingQuotaByIndividualId(insuredCode, individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/GetOperatingQuotaByIndividualId',
            data: JSON.stringify({ insuredCode: insuredCode, individualId: individualId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteQuotaOperation(objDeleteQuotaOperation) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/DeleteQuotaOperation',
            data: JSON.stringify({ operatingQuotaModelView: objDeleteQuotaOperation }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static UpdateQuotaOperation(quotaOperationModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/UpdateQuotaOperation',
            data: JSON.stringify({ operatingQuotaModelView: quotaOperationModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateQuotaOperation(objQuotaOperation) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/CreateQuotaOperation',
            data: JSON.stringify({ operatingQuotaModelView: objQuotaOperation }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrencies() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'QuotaOperational/QuotaOperational/GetCurrencies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetOperatingQuotaByIndividualId(insuredCode, individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'QuotaOperational/QuotaOperational/GetOperatingQuotaByIndividualId',
            data: JSON.stringify({ insuredCode: insuredCode, individualId: individualId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}

