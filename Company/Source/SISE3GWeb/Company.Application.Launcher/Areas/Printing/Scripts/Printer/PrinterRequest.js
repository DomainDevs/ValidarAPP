class PrinterRequest {
    static PrintReportFromOutside(branchId, prefixId, policyNumber, endorsementId) {
        var dfd = jQuery.Deferred();
        //Evitar valores especiales después de numeros
        if (typeof (policyNumber) === "string") {
            policyNumber = policyNumber.split(".")[0];
            //Evitar enviar cadenas textuales, obtener solo el ID
            policyNumber = (policyNumber.match(/[A-Za-z]/i)) ? policyNumber.slice(0, policyNumber.indexOf(' ')) : policyNumber;
        }

        $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetEndorsementsByFilterPolicy',
            data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done((data) => {
            if (data.success) {
                endorsementId = (endorsementId == null || endorsementId == 0 || isNaN(parseFloat(endorsementId))) ? data.result[0].Id : endorsementId;
                var idPolicyToPrint = data.result.find(x => x.Id == endorsementId).PolicyId;
                $.ajax({
                    type: 'POST',
                    url: rootPath + 'Printing/Printer/GenerateReportPolicyToEndoso',
                    data: JSON.stringify({ policyId: idPolicyToPrint, endorsementId: endorsementId, prefixId: prefixId }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                }).done((data) => {
                    dfd.resolve(data);
                }).fail((jqXHR, textStatus, errorThrown) => {
                    dfd.reject(jqXHR, textStatus, errorThrown);
                });
            }
        }).fail((jqXHR, textStatus, errorThrown) => {
            dfd.reject({ success: false, result: AppResources.FailGetEndorsements });

        });
        return dfd.promise();
    }

    static PrintReportEndorsement(prefixId, policyId, endorsementId) {
        var dfd = jQuery.Deferred();
        $.ajax({
            type: 'POST',
            url: rootPath + 'Printing/Printer/GenerateReportPolicyToEndoso',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId, prefixId: prefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done((data) => {
            dfd.resolve(data);
        }).fail((jqXHR, textStatus, errorThrown) => {
            dfd.reject(jqXHR, textStatus, errorThrown);
        });
        return dfd.promise();
    }

    static PrintTemporaryOutside(tempId, prefixId) {
        var dfd = jQuery.Deferred();

        $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetEndorsementsByFilterPolicy',
            data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done((data) => {
            if (data.success) {
                endorsementId = (!endorsementId) ? data.result[0].Id : endorsementId;
                $.ajax({
                    type: 'POST',
                    url: rootPath + 'Printing/Printer/GenerateReportTemporary',
                    data: JSON.stringify({ temporaryId: temporary.TempId, prefixId: temporary.CommonProperties.PrefixId }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                }).done((data) => {
                    dfd.resolve(data);
                }).fail((jqXHR, textStatus, errorThrown) => {
                    dfd.reject(jqXHR, textStatus, errorThrown);
                });
            }
        }).fail((jqXHR, textStatus, errorThrown) => {
            dfd.reject({ success: false, result: AppResources.FailGetEndorsements });

        });
        return dfd.promise();
    }

    static PrintQuotationOutside(temporaryId, prefixId, quotationId, versionId) {

        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GenerateReportQuotation',
            data: JSON.stringify({ temporaryId: temporaryId, prefixId, quotationId: quotationId, versionId: versionId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ValidationAccessAndHierarchy() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/ValidationAccessAndHierarchy',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetReinsurancePolicy(policyId, endorsementId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GetReinsurancePolicy',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId, prefixId: prefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}