class NoticeTransportRequest {
    static ExecuteNoticeOperations(noticeTransportDTO, contactInformationDTO, transportDTO, noticeCoverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeTransport/ExecuteNoticeOperations',
            data: JSON.stringify(
                {
                    "noticeTransportDTO": noticeTransportDTO,
                    "contactInformationDTO": contactInformationDTO,
                    "transportDTO": transportDTO,
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
            url: rootPath + 'Claims/NoticeTransport/GetRisksByInsuredId',
            data: JSON.stringify({ "insuredId": insuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskTransportByClaimNoticeId(claimNoticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeTransport/GetRiskTransportByClaimNoticeId',
            data: JSON.stringify({ "claimNoticeId": claimNoticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskTransportByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeTransport/GetRiskTransportByRiskId',
            data: JSON.stringify({ "riskId": riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}