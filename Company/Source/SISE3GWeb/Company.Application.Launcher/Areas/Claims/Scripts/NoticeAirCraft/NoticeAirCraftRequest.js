class NoticeAirCraftRequest {
    static GetAirCraftMakes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeAirCraft/GetAirCraftMakes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAirCraftModelsByMakeId(makeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeAirCraft/GetAirCraftModelsByMakeId',
            data: JSON.stringify({ "makeId": makeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAirCraftUsesByPrefixId() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeAirCraft/GetAirCraftUsesByPrefixId',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAirCraftRegisters() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeAirCraft/GetAirCraftRegisters',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAirCraftOperators() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeAirCraft/GetAirCraftOperators',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static ExecuteNoticeOperations(noticeAirCraftDTO, contactInformationDTO, airCraftDTO, noticeCoverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeAirCraft/ExecuteNoticeOperations',
            data: JSON.stringify(
                {
                    "noticeAirCraftDTO": noticeAirCraftDTO,
                    "contactInformationDTO": contactInformationDTO,
                    "airCraftDTO": airCraftDTO,
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
            url: rootPath + 'Claims/NoticeAirCraft/GetRisksByInsuredId',
            data: JSON.stringify({ "insuredId": insuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskAirCraftByClaimNoticeId(claimNoticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeAirCraft/GetRiskAirCraftByClaimNoticeId',
            data: JSON.stringify({ "claimNoticeId": claimNoticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskAirCraftByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeAirCraft/GetRiskAirCraftByRiskId',
            data: JSON.stringify({ "riskId": riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}