$.ajaxSetup({ async: false });
//Codigo de la pagina Clauses.cshtml
$(document).ready(function () {
    $("#btnClause").on('click', function () {
        $("#textClausesDetail").hide();
        $("#labelClausesDetail").hide();
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.Clauses);
        GetClausesByLevelsConditionLevelId($('#selectPrefix').val());

        $('#textClausesDetail').val('');

        if (policy.Clauses != undefined) {
            $.each($('#tableClauses').UifDataTable('getData'), function (id, item) {
                var clauseId = this.Id;
                $.each(policy.Clauses, function (idC, itemC) {
                    if (clauseId == this.Id) {
                        $('#tableClauses tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                        $('#tableClauses tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                });
            });
        }

    });

    $("#btnClauseClose").on("click", function () {
        
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.Search);
    });
});


function GetClausesByLevelsConditionLevelId(conditionLevelId) {
    

    $.ajax({
        type: "POST",
        url: rootPath + 'Underwriting/Underwriting/GetClausesByLevelsConditionLevelId',
        data: JSON.stringify({ levels: Levels.General, conditionLevelId: conditionLevelId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $('#tableClauses').UifDataTable('clear');
            if (data.result.length > 0) {
                $('#tableClauses').UifDataTable('addRow', data.result);
            }
            $("#tableClauses").find("button").attr("disabled", "disabled")
        }
        else {
            $('#tableClauses').UifDataTable('clear');
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchClauses, 'autoclose': true })
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchClauses, 'autoclose': true });
    });
}
