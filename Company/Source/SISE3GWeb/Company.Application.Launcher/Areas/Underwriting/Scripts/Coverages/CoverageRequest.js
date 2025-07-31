class CoverageRequest {
    static GetDeductiblesByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Coverage/GetDeductiblesByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRateTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Coverage/GetRateTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCalculeTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Coverage/GetCalculeTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}