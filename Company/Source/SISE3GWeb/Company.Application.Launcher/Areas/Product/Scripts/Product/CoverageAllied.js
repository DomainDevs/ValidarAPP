$.ajaxSetup({ async: false });
$(document).ready(function () {
    $("#listModalAllyCoverage").UifListView({ source: null, displayTemplate: "allyCoverageTemplate", height: 310 });
    InitCoverageAllied();
});
function InitCoverageAllied() {

    $("#btnModalAssignCoverageAllyCoverage").on('click', function () {

        var coverage = $("#listModalAssignCoverage").UifListView('getData')[coverageIndex];
        if (coverage != null && coverageIndex > 0) {
            $("#listModalAllyCoverage").UifListView({ source: null, displayTemplate: "allyCoverageTemplate", height: 310 });
            $('#inputCoverage').val(coverage.Description);
            $('#inputInsuredObject').val(coverage.InsuredObjectDescription);
            if (coverage.CoverageAllied == null || coverage.CoverageAllied.length == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoverageNoHaveAllyCoverage, 'autoclose': true });
            } else {
                LoadCoverageAllied(coverage.CoverageAllied);
                $("#modalAllyCoverage").UifModal('showLocal', Resources.Language.LabelCoverageAllied);
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectCoverage, 'autoclose': true });
        }
    });
}

function LoadCoverageAllied(coverageAlliedList) {
    if (coverageAlliedList != null && coverageAlliedList.length > 0) {
        $("#listModalAllyCoverage").UifListView({ sourceData: coverageAlliedList, displayTemplate: "#allyCoverageTemplate", height: 310 });
    }
}

