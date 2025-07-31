$.ajaxSetup({ async: false });
//Codigo para la pantalla Issue
var temporal = 0;
$(document).ready(function () {
    EventsIssueMassive();
});

function EventsIssueMassive() {

    $("#btnIssueP").on('click', function () {
        IssueMassiveP();
    });

    $("#btnIssuePWithEvents").on('click', function () {
        IssueMassiveWithEvent();
    });

}

function IssueMassiveP() {
    var msg;
    if (objIssue == null && objIssue != 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPolicyObject });
    } else {
        var cargues = objIssue.Description;
        cargues = cargues.replace("*", "");
        temporal = objIssue.TempId

        $.UifDialog('confirm', { 'message': Resources.Language.ProcessIssueTemporary+': ' + $('#inputTemporalIssue').val() + ' Desea Continuar?' }, function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Collective/Collective/SetIssueMassive',
                    data: JSON.stringify({
                        massiveLoads: cargues, tempId: temporal,
                        operationId: $('#inputTemporalIssue').val(),
                        endorsementType: $("#hiddenEndorsementId").val(),
                        "prefixId": $("#hiddenPrefixCommercial").val(),
                        productId: $("#hiddenProduct").val(),
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        if (data.result == "") {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Error+" en SetIssueMassive" });
                        }
                        else {
                            if ($.isNumeric(data.result)) {
                                if (data.result == "Temporal Riesgos Vigentes") {
                                    window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=0" + "&errors=" + true + "&tariffed=" + true + "&temporalId=" + temporal;
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                                }
                                else {
                                    Collective.HidePanels(1)
                                    ShowPanelsCollective(MenuType.Massive)
                                    if (EndorsementType.Emission == $("#hiddenEndorsementId").val()) {
                                        $('#hiddenPolicyNum').val(data.result);
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelPolicyNumber+": " + data.result });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SuccessfullyEndorsementGenerated+"."+Resources.Language.LabelPolicyNumber+": " + $('#hiddenPolicyNum').val() + " " + Resources.Language.EndorsementNumber+".: " + data.result });
                                    }
                                    $('#TableLoad').UifDataTable('clear');
                                }
                            }
                            else {
                                if (data.result == "Temporal Riesgos Vigentes") {
                                    window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=0" + "&errors=" + true + "&tariffed=" + true + "&temporalId=" + temporal;                                  
                                }
                                $.UifNotify('show', { 'type': 'info', 'message': data.result });
                            }

                        }

                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorIssue, 'autoclose': true })
                });

            }
        });
    }
}

function IssueMassiveWithEvent() {
    var msg;
    if (objIssue == null && objIssue != 0) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPolicyObject, 'autoclose': true });
    } else {
        var cargues = objIssue.Description;
        cargues = cargues.replace("*", "");
        var temporal = objIssue.TempId
        $.UifDialog('confirm', {
            'message': Resources.Language.ProcessIssueTemporary+': ' + temporal + Resources.Language.WishContinue
        }, function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Collective/Collective/SetIssueMassive',
                    data: JSON.stringify({
                        massiveLoads: cargues, tempId: temporal,
                        operationId: $('#inputTemporalIssue').val(),
                        endorsementType: $("#hiddenEndorsementId").val(),
                        "prefixId": $("#hiddenPrefixCommercial").val()
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        if (data.result == "") {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Error+ " en SetIssueMassive" });
                        }
                        else {
                            if ($.isNumeric(data.result)) {
                                if (data.result == "Temporal Riesgos Vigentes") {
                                    window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=0" + "&errors=" + true + "&tariffed=" + true + "&temporalId=" + temporal;
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                                }
                                else {
                                    Collective.HidePanels(1)
                                    ShowPanelsCollective(MenuType.Massive)
                                    if (EndorsementType.Emission == $("#hiddenEndorsementId").val()) {
                                        $('#hiddenPolicyNum').val(data.result);
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelNumberPolicy+": " + data.result });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message':Resources.Language.SuccessfullyEndorsementGenerated+ ". " + Resources.Language.LabelNumberPolicy+": " + $('#hiddenPolicyNum').val() + ". "+ Resources.Language.EndorsementNumber+": " + data.result });
                                    }
                                    $('#TableLoad').UifDataTable('clear');
                                }
                            }
                            else {
                                if (data.result == "Temporal Riesgos Vigentes") {
                                    window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=0" + "&errors=" + true + "&tariffed=" + true + "&temporalId=" + temporal;
                                }
                                $.UifNotify('show', { 'type': 'info', 'message': data.result });
                            }

                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorIssue, 'autoclose': true })
                });

            }
        });
    }

}


function LoadRiskView() {
    var url = rootPath + 'Underwriting/Underwriting/Index?type=2&isCollective=true';
    location.href = url;
}
