class RequestCancellationRequest {

    static GetPaymentChargeRequestByPrefixIdBranchIdNumber(prefixId, branchId, number) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/RequestCancellation/GetPaymentChargeRequestByPrefixIdBranchIdNumber',
            data: JSON.stringify({
                prefixId: prefixId,
                branchId: branchId,
                Number: number,
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentRequestClaimsByPaymentRequestId(paymentRequestId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/RequestCancellation/GetPaymentRequestClaimsByPaymentRequestId',
            data: JSON.stringify({
                paymentRequestId: paymentRequestId,                
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveRequestCancellation(paymentRequestId, isChargeRequest) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/RequestCancellation/SaveRequestCancellation',
            data: JSON.stringify({
                paymentRequestId: paymentRequestId,
                isChargeRequest: isChargeRequest
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSurety/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentMovementTypesByPaymentSourceId(sourceId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Charge/GetPaymentMovementTypesByPaymentSourceId',
            data: JSON.stringify({ sourceId: sourceId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}