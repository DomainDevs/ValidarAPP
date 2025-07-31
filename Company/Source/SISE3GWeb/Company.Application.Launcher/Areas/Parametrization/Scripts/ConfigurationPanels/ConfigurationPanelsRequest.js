class ConfigurationPanelsRequest
{
    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Parametrization/CauseCoverage/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLineBusinessByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetLinesBusinessByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }
    
    static GetSubLineBusinessByLineBussinessId(lineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetSubLinesBusinessByLineBusinessId',
            data: JSON.stringify({ lineBusinessId: lineBusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ConfigurationPanels/GetCoveragesByLineBusinessIdSubLineBusinessId',
            data: JSON.stringify({ lineBusinessId: lineBusinessId , subLineBusinessId: subLineBusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetCoveragesByLineBusinessIdSubLineBusiness(lineBusinessId, subLineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ConfigurationPanels/GetCoveragesByLineBusinessIdSubLineBusiness',
            data: JSON.stringify({ lineBusinessId: lineBusinessId, subLineBusinessId: subLineBusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static SaveClaimCoverageActivePanel(claimCoverageActivePanel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ConfigurationPanels/CreateActivePanel',
            data: JSON.stringify({ claimCoverageActivePanels: claimCoverageActivePanel}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static UpdateClaimCoverageActivePanel(claimCoverageActivePanel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ConfigurationPanels/UpdateActivePanel',
            data: JSON.stringify({ claimCoverageActivePanels: claimCoverageActivePanel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }
}