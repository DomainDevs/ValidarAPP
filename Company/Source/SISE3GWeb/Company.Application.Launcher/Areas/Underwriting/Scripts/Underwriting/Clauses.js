//Codigo de la pagina Clauses.cshtml
class UnderwritingClauses extends Uif2.Page {
    getInitialState() {
    }

    //Seccion Eventos
    bindEvents() {
        $('#btnClauses').on('click', this.ClausesLoad);
        $('#tableClauses tbody').on('click', 'tr', this.SelectSearchClauses);
        $('#btnClausesSave').on('click', UnderwritingClauses.SaveClauses);
    }

    ClausesLoad() {
        if (glbPolicy.Id == 0 && glbPolicy.TemporalType != TemporalType.Quotation) {
            if ($("#formUnderwriting").valid()) {
                Underwriting.SaveTemporalPartial(MenuType.CoInsurance);      
                //$.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
            }
        }
        else {
            UnderwritingClauses.LoadPartialClauses();
        }
    }

    SelectSearchClauses(e) {
        e.preventDefault();
        var title = $(this).context.textContent;
        //$('#textClausesDetail').val('');
        //var title = $(this).children()[1].innerHTML;
        if (!$(this).hasClass('row-selected')) {
            $('#textClausesDetail').val('');    
            $.each($('#tableClauses').UifDataTable('getData'), function (key, value) {
                if (title.trim() == (this.Name+ this.Title)) {
                    $('#textClausesDetail').val(this.Text);
                }
            });
        } else {
            $('#textClausesDetail').val('');
        }
    } 
    

    static LoadPartialClauses() {

        $('#textClausesDetail').val('');
        $('#formUnderwriting').validate();

        if (glbPolicy.Id > 0 && $('#formUnderwriting').valid()) {
            Underwriting.ShowPanelsIssuance(MenuType.Clauses);
            UnderwritingClauses.GetClausesByLevelsConditionLevelId($('#selectPrefixCommercial').val());

        }
    }

    static GetClausesByLevelsConditionLevelId(conditionLevelId) {
        ClausesRequest.GetClausesByLevelsConditionLevelId(Levels.General, conditionLevelId).done(function (data) {
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

                    if (glbPolicy.Clauses != null) {
                        var object = [];
                        $.each(glbPolicy.Clauses, function (idC, itemC) {
                            object.push(this.Id);
                            if ($('#textClausesDetail').val() != "") {
                                $('#textClausesDetail').val(itemC.Text + '\r\n  \r\n' + $('#textClausesDetail').val());
                            } else {
                                $('#textClausesDetail').val(itemC.Text);
                            }
                        });
                        var value = {
                            label: 'Id',
                            values: object
                        }

                        $("#tableClauses").UifDataTable('setSelect', value)

                    }
                }
                else
                {
                    $('#tableClauses').UifDataTable('clear');
                }
            }
            else {
                $('#tableClauses').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchClauses, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchClauses, 'autoclose': true });
        });
    }

    static SaveClauses() {
        ClausesRequest.SaveClauses(glbPolicy.Id, $('#tableClauses').UifDataTable('getSelected')).done(function (data) {
            if (data.success) {
                Underwriting.HidePanelsIssuance(MenuType.Clauses);
                glbPolicy.Clauses = data.result;
                Underwriting.LoadSubTitles(5);
                if (glbPolicy.TemporalType == TemporalType.Quotation) {
                    Underwriting.SaveTemporal(false);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveClauses, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveClauses, 'autoclose': true });
        });
    }
}