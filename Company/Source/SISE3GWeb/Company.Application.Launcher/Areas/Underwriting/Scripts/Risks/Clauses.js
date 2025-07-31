//Codigo de la pagina Clauses.cshtml
class RiskClause extends Uif2.Page {
    getInitialState() {
        RiskClause.GetClausesByLevelsConditionLevelId()
    }
    //Seccion Eventos
    bindEvents() {
        $('#btnClauses').on('click', RiskClause.LoadPartialClauses);
        $('#tableClauses tbody').on('click', 'tr', this.SelectSearchClauses);
        $('#btnClausesSave').on('click', RiskClause.SaveClauses);
    }
    ClausesLoad() {
        if (glbRisk.Id == 0) {
            RiskClause.GetClausesByLevelsConditionLevelId()
        }
        else {
            RiskClause.LoadPartialClauses();
        }
    }

    SelectSearchClauses(e) {
        e.preventDefault();
        //var title = $(this).context.textContent;
        var title = $(this).children()[1].innerHTML;
        if (!$(this).hasClass('row-selected')) {
            $.each($('#tableClauses').UifDataTable('getData'), function (key, value) {
                if (title.trim() == this.Title.trim()) {
                    $('#textClausesDetail').val(this.Text);
                }
            });
        } else {
            $('#textClausesDetail').val('');
        }
    }

    static LoadPartialClauses() {

        $('#textClausesDetail').val('');
        $('#formVehicle').validate();
        if (glbRisk.Id == 0) {
            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].SaveRisk(MenuType.Clauses, 0);
            }
            else {
                glbRisk.Class.SaveRisk(MenuType.Clauses, 0);
            }
        }

        if (glbRisk.Id > 0) {
            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].ShowPanelsRisk(MenuType.Clauses);
            }
            else {
                glbRisk.Class.ShowPanelsRisk(MenuType.Clauses);
            }
            RiskClause.GetClausesByLevelsConditionLevelId();
            $('#textClausesDetail').val('');

        }
    }

    static GetClausesByLevelsConditionLevelId() {

        var conditionLevel = ConditionLevel.Independent;
        if (glbPolicy.Product.CoveredRisk != null && glbPolicy.Product.CoveredRisk.CoveredRiskType > 0) {
            conditionLevel = glbPolicy.Product.CoveredRisk.CoveredRiskType
        }

        ClausesRequest.GetClausesByLevelsConditionLevelId(Levels.Risk, conditionLevel).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#tableClauses').UifDataTable('clear');
                    if (data.result.length > 0) {
                        $('#tableClauses').UifDataTable('addRow', data.result);
                        $.each(data.result, function (id, item) {
                            if (item.IsMandatory == true) {
                                $('#tableClauses tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                                $('#tableClauses tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                            }
                        });
                    }
                    $('#textClausesDetail').val('');


                    if (glbRisk.Clauses != null && glbRisk.Clauses.length > 0) {
                        var object = [];
                        $.each(glbRisk.Clauses, function (idC, itemC) {
                            object.push(this.Id);
                        });
                        var value = {
                            label: 'Id',
                            values: object
                        }

                        $("#tableClauses").UifDataTable('setSelect', value)
                    }
                }
            }
            else {
                $('#tableClauses').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchClauses, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchClauses, 'autoclose': true });
        });
    }

    static SaveClauses() {
        ClausesRequest.SaveClausesRisk(glbPolicy.Id, glbRisk.Id, $('#tableClauses').UifDataTable('getSelected'), riskController).done(function (data) {
            if (data.success) {
                if (glbRisk.Class == undefined) {
                    window[glbRisk.Object].ShowPanelsRisk(MenuType.Risk);
                    glbRisk.Clauses = data.result;
                    window[glbRisk.Object].LoadSubTitles(3);
                }
                else {
                    glbRisk.Class.ShowPanelsRisk(MenuType.Risk);
                    glbRisk.Clauses = data.result;
                    glbRisk.Class.LoadSubTitles(3);
                }
                $('#modalClauses').UifModal('hide');
            }
            else {
                if (data != null && data.result != null)
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                else
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveClauses, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveClauses, 'autoclose': true });
        });
    }
}
