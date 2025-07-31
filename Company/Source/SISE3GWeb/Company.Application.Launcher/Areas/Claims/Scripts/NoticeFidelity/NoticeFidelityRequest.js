class NoticeFidelityRequest {

    static GetRiskCommercialClasses() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeFidelity/GetRiskCommercialClasses',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetOccupations() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeFidelity/GetOccupations',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static ExecuteNoticeOperations(noticeFidelityDTO, contactInformationDTO, fidelityDTO, noticeCoverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeFidelity/ExecuteNoticeOperations',
            data: JSON.stringify(
                {
                    "noticeFidelityDTO": noticeFidelityDTO,
                    "contactInformationDTO": contactInformationDTO,
                    "fidelityDTO": fidelityDTO,
                    "noticeCoverageDTO": noticeCoverageDTO
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByInsuredId(insuredId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeFidelity/GetRisksByInsuredId',
            data: JSON.stringify({ "insuredId": insuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskFidelityByClaimNoticeId(claimNoticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeFidelity/GetRiskFidelityByClaimNoticeId',
            data: JSON.stringify({ "claimNoticeId": claimNoticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskFidelityByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeFidelity/GetRiskFidelityByRiskId',
            data: JSON.stringify({ "riskId": riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}