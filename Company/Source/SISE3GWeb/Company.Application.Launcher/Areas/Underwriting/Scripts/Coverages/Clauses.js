//Codigo de la pagina Clauses.cshtml
"use strict";
var clausesCoverage = null;
class ClausesCoverage extends Uif2.Page {
    getInitialState() {}

    bindEvents() {
        $('#btnClausesCoverage').on('click', ClausesCoverage.GetClausesByLevelsConditionLevelId);
        $('#tableClauses tbody').on('click', 'tr', this.SelectTableClauses);
        $('#btnClausesSave').on('click',ClausesCoverage.SaveClauses);
    }

    SelectTableClauses(e) {
       
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

        if ($('#selectCoverage').val() > 0) {

            if (glbCoverage.Class == undefined) {
                window[glbCoverage.Object].ShowPanelsCoverage(MenuType.Clauses);
            }
            else {
                glbCoverage.Class.ShowPanelsCoverage(MenuType.Clauses);
            }
            
            $('#textClausesDetail').val('');

            if (clausesCoverage != null) {
                var object = [];
                $.each(clausesCoverage, function (idC, itemC) {
                    object.push(this.Id);
                });
                var value = {
                    label: 'Id',
                    values: object
                }

                $("#tableClauses").UifDataTable('setSelect', value)
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }
    }

    static GetClausesByLevelsConditionLevelId() {
        ClausesRequest.GetClausesByLevelsConditionLevelId(Levels.Coverage, coverageData.Id).done(function (data) {
            if (data.success) {
                $('#tableClauses').UifDataTable('clear');
                if (data.result.length > 0) {
                    $('#tableClauses').UifDataTable('addRow', data.result);
                    $.each(data.result, function (id, item) {
                        if (item.IsMandatory == true) {
                            $('#tableClauses tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                            $('#tableClauses tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                        if (coverageData.Clauses != null && coverageData.Clauses.length > 0) {
                            $.each(coverageData.Clauses, function (index, itemClause) {
                                if (item.Id == itemClause.Id) {
                                    $('#tableClauses tbody tr:eq(' + index + ' )').removeClass('row-selected').addClass('row-selected');
                                    $('#tableClauses tbody tr:eq(' + index + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                                }
                            });
                        }
                    });
                }
                ClausesCoverage.LoadPartialClauses()
            }
            else {
                $('#tableClauses').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchClauses, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchClauses, 'autoclose': true });
        });
    }

    static SaveClauses() {

        clausesCoverage = $('#tableClauses').UifDataTable('getSelected');
        if (glbCoverage.Class == undefined) {
            window[glbCoverage.Object].ShowPanelsCoverage(MenuType.Coverage);
        }
        else {
            glbCoverage.Class.ShowPanelsCoverage(MenuType.Coverage);
        }
        $('#modalClauses').UifModal('hide');
    }
}