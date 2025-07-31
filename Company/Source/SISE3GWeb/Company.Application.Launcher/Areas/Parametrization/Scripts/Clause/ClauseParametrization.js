//Clase formulario principal
var clauseParametrizationData = {};
var glbClauseEdit;
var glbListClauseDelete = [];
class ClauseParametrization extends Uif2.Page {

    getInitialState() {
        new ClauseDetail();
        ClauseParametrization.GetLevels();
        ClauseParametrization.GetCommercialBranch();
        ClauseParametrization.GetCoveredRisk();
        ClauseParametrization.GetLineBusiness();

        $("#listClause").UifListView({
            displayTemplate: "#templateClause",
            source: null,
            selecttionType: 'single',
            height: 400
        });
        request('Parametrization/Clauses/GetParametrizationClause', null, 'GET', AppResources.ErrorSearchClauses, ClauseParametrization.getClause);
        //Texto enriquecido para el textarea ClauseText
        //CKEDITOR.replace('ClauseText', {
        //    resize_enabled: false,
        //    height: 100
        //}); 


        //$("#ClauseText").css("visibility", "show");
    }

    bindEvents() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#listClause').on('rowEdit', ClauseParametrization.editClause);
        $("#btnClauseNew").on('click', ClauseParametrization.clearFormClause);//function () {
            //CKEditor Instance must be clear from here, in clearFormClause get into a bug when is called from EditClause.
            //CKEDITOR.instances.ClauseText.setData('');

            //$('#ClauseText').val('');
            //$("#inputPrecatalogedText").data("Object").TextBody = '';
            //ClauseParametrization.clearFormClause();
        //});
        //muestra modal textos
        $('#inputPrecatalogedText').on("search", this.SearchTextClause);
        $('#tblResultListClause').on('rowSelected', this.SelectSearchClause);
        $('#inputClauseSearch').on("search", this.SearchClause);
        $('#tblResultListClauseTotal').on('rowSelected', this.SelectSearchClauseByNameAndTitle);
        //muestra modal coberturas
        $('#Coverage').on("search", this.SearchTextCoverage);
        $('#tblResultListCoverage').on('rowSelected', this.SelectSearchCoverage);
        $('#btnClauseAccept').click(this.addClause);
        $("#btnSaveClause").click(ClauseParametrization.createClause);
        $('#btnMore').on('click', this.SelectTextArea);
        $('#Level').on('itemSelected', (event, data, index) => { this.eventClick(event, data, index) });
        $("#btnExport").click(this.exportExcel);
        $('#divBranch').hide();
        $('#divCoveredRisk').hide();
        $("#divCoverage").hide();
        $("#divObj").hide();
        $("#divLineBusiness").hide();
        $("#divProtection").hide();
        $("#btnExitClause").click(this.redirectIndex);
    }

    SelectTextArea() {
        if ($("#inputPrecatalogedText").data("Object") != undefined) {
            $('#ClauseText').val($("#inputPrecatalogedText").data("Object").TextBody);
            //CKEDITOR.instances.ClauseText.setData($("#inputPrecatalogedText").data("Object").TextBody);
        }
    }

    eventClick(event, data, index) {
        switch (parseInt($("#Level").val())) {
            case ParametrizationClauseConditionLevel.Independent: //Independiente
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.Prefix: // ramo comercial
                $("#divBranch").show();
                $("#divCoveredRisk").hide();
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.RiskType:// tipo de riesgo
                $("#divCoveredRisk").show();
                $("#divBranch").hide();
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.TechnicalBranch: //Ramo Tecnico
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                $("#divLineBusiness").show();
                break;
            case ParametrizationClauseConditionLevel.Coverage:// Cobertura
                $("#divCoverage").show();
                $("#divObj").show();
                $("#divProtection").show();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                $("#divLineBusiness").hide();
                break;
        }
    }

    SelectSearchClauseByNameAndTitle(event, dataListClause, position) {
        const findId = function (element, index, array) {
            return element.Id === dataListClause.Id
        }
        var indexClause = $("#listClause").UifListView("findIndex", findId);
        ClauseParametrization.editClause(event, dataListClause, indexClause);

        if (dataListClause.ClauseLevelServiceModel.IsMandatory) {
            $('#required').prop('checked', true);
        }
        else {
            $('#required').prop('checked', false);
        }
        $("#Level").UifSelect('setSelected', dataListClause.ConditionLevelServiceQueryModel.Id);

        switch (parseInt($("#Level").UifSelect('getSelected'))) {
            case ParametrizationClauseConditionLevel.Independent: //Independiente
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.Prefix: // ramo comercial
                if (dataListClause.PrefixServiceQueryModel !== null) {
                    $('#CommercialBranch').UifSelect('setSelected', dataListClause.PrefixServiceQueryModel.PrefixCode);
                    $("#divBranch").show();
                    $("#divCoveredRisk").hide();
                    $("#divCoverage").hide();
                    $("#divObj").hide();
                    $("#divProtection").hide();
                    $("#divLineBusiness").hide();
                } else {
                    $('#CommercialBranch').UifSelect('setSelected', null);
                    $("#divBranch").show();
                    $("#divCoveredRisk").hide();
                    $("#divCoverage").hide();
                    $("#divObj").hide();
                    $("#divProtection").hide();
                    $("#divLineBusiness").hide();
                }


                break;
            case ParametrizationClauseConditionLevel.RiskType:// tipo de riesgo
                if (dataListClause.RiskTypeServiceQueryModel !== null) {
                    $('#CoveredRisk').UifSelect('setSelected', dataListClause.RiskTypeServiceQueryModel.Id);
                    $("#divCoveredRisk").show();
                    $("#divBranch").hide();
                    $("#divCoverage").hide();
                    $("#divObj").hide();
                    $("#divProtection").hide();
                    $("#divLineBusiness").hide();
                }
                else {
                    $('#CoveredRisk').UifSelect('setSelected', null);
                    $("#divCoveredRisk").show();
                    $("#divBranch").hide();
                    $("#divCoverage").hide();
                    $("#divObj").hide();
                    $("#divProtection").hide();
                    $("#divLineBusiness").hide();
                }

                break;
            case ParametrizationClauseConditionLevel.TechnicalBranch: //Ramo Tecnico
                if (dataListClause.LineBusinessServiceQueryModel !== null) {
                    $("#divCoverage").hide();
                    $("#divObj").hide();
                    $("#divProtection").hide();
                    $("#divBranch").hide();
                    $("#divCoveredRisk").hide();
                    $("#divLineBusiness").show();
                    $('#LineBusiness').UifSelect('setSelected', dataListClause.LineBusinessServiceQueryModel.Id);
                }
                else {
                    $("#divCoverage").hide();
                    $("#divObj").hide();
                    $("#divProtection").hide();
                    $("#divBranch").hide();
                    $("#divCoveredRisk").hide();
                    $("#divLineBusiness").show();
                    $('#LineBusiness').UifSelect('setSelected', null);
                }

                break;
            case ParametrizationClauseConditionLevel.Coverage:// Cobertura
                if (dataListClause.CoverageServiceQueryModel !== null) {
                    $('#Coverage').val(dataListClause.CoverageServiceQueryModel.Description + '(' + dataListClause.CoverageServiceQueryModel.Id + ')');
                    $("#ObjectInsurance").val(dataListClause.CoverageServiceQueryModel.InsuredObjectServiceQueryModel.Description);
                    $("#Protection").val(dataListClause.CoverageServiceQueryModel.PerilServiceQueryModel.Description);
                    $("#divCoverage").show();
                    $("#divObj").show();
                    $("#divProtection").show();
                    $("#divBranch").hide();
                    $("#divCoveredRisk").hide();
                    $("#divLineBusiness").hide();
                }
                else {
                    $('#Coverage').val("");
                    $("#ObjectInsurance").val("");
                    $("#Protection").val("");
                    $("#divCoverage").show();
                    $("#divObj").show();
                    $("#divProtection").show();
                    $("#divBranch").hide();
                    $("#divCoveredRisk").hide();
                    $("#divLineBusiness").hide();
                }

                break;
        }
        $('#modalListSearchClause').UifModal('hide');
    }

    SelectSearchCoverage(event, dataCoverage, position) {
        $("#Coverage").data("Object", dataCoverage);
        $("#Coverage").val(dataCoverage.Description + '(' + dataCoverage.Id + ')');
        $("#ObjectInsurance").val(dataCoverage.InsuredObjectServiceQueryModel.Description);
        $("#Protection").val(dataCoverage.PerilServiceQueryModel.Description);
        $('#modalListSearchCoverage').UifModal('hide');
    }

    SearchClause(event, value) {
        if ($.trim($("#inputClauseSearch").val()) != "") {
            ClauseParametrization.GetClauses($("#inputClauseSearch").val());
        }
    }

    SearchTextClause(event, value) {
        if ($.trim($("#inputPrecatalogedText").val()) != "") {
            ClauseParametrization.GetTextClause($("#inputPrecatalogedText").val());
        }
    }

    SearchTextCoverage(event, value) {
        if ($.trim($("#Coverage").val()) != "") {
            ClauseParametrization.GetTextCoverage($("#Coverage").val());
        }
    }

    static GetTextClause(description) {
        ClauseParametrizationRequest.GetTextClause(description).done(function (data) {
            if (data.success) {
                if (data.result.TextServiceModels.length > 1) {
                    ClauseParametrization.ShowTextList(data.result.TextServiceModels);
                    //$("#ClauseText").val("");
                }
                else if (data.result.TextServiceModels.length == 1) {
                    ClauseParametrization.TextListStatic(data.result.TextServiceModels[0]);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchTexts, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorLoadingTextsPrec, 'autoclose': true });
            }
        });


    }

    SelectSearchClause(event, dataClause, position) {
        $("#inputPrecatalogedText").data("Object", dataClause);
        $("#inputPrecatalogedText").val(dataClause.Description + '(' + dataClause.Id + ')');
        $('#modalListSearchText').UifModal('hide');
    }

    static GetClauses(description) {
        ClauseParametrizationRequest.GetClausesByNameAnTitle(description).done(function (data) {
            if (description.length >= 3) {
                if (data.success) {
                    if (data.result.length > 0) {
                        if (data.result.length === 1) {

                            const findId = function (element, index, array) {
                                return element.Id === data.result[0].Id
                            }
                            var indexClause = $("#listClause").UifListView("findIndex", findId);
                            ClauseParametrization.editClause(null, data.result[0], indexClause);
                            if (data.result[0].ClauseLevelServiceModel.IsMandatory) {
                                $('#required').prop('checked', true);
                            }
                            else {
                                $('#required').prop('checked', false);
                            }

                            $("#Level").UifSelect('setSelected', data.result[0].ConditionLevelServiceQueryModel.Id);

                            switch (parseInt($("#Level").UifSelect('getSelected'))) {
                                case ParametrizationClauseConditionLevel.Independent: //Independiente
                                    $("#divCoverage").hide();
                                    $("#divObj").hide();
                                    $("#divProtection").hide();
                                    $("#divBranch").hide();
                                    $("#divCoveredRisk").hide();
                                    $("#divLineBusiness").hide();
                                    break;
                                case ParametrizationClauseConditionLevel.Prefix: // ramo comercial
                                    if (data.result[0].PrefixServiceQueryModel !== null) {
                                        $('#CommercialBranch').UifSelect('setSelected', data.result[0].PrefixServiceQueryModel.PrefixCode);
                                        $("#divBranch").show();
                                        $("#divCoveredRisk").hide();
                                        $("#divCoverage").hide();
                                        $("#divObj").hide();
                                        $("#divProtection").hide();
                                        $("#divLineBusiness").hide();
                                    }
                                    else {
                                        $('#CommercialBranch').UifSelect('setSelected', null);
                                        $("#divBranch").show();
                                        $("#divCoveredRisk").hide();
                                        $("#divCoverage").hide();
                                        $("#divObj").hide();
                                        $("#divProtection").hide();
                                        $("#divLineBusiness").hide();
                                    }

                                    break;
                                case ParametrizationClauseConditionLevel.RiskType:// tipo de riesgo
                                    if (data.result[0].RiskTypeServiceQueryModel !== null) {
                                        $('#CoveredRisk').UifSelect('setSelected', data.result[0].RiskTypeServiceQueryModel.Id);
                                        $("#divCoveredRisk").show();
                                        $("#divBranch").hide();
                                        $("#divCoverage").hide();
                                        $("#divObj").hide();
                                        $("#divProtection").hide();
                                        $("#divLineBusiness").hide();
                                    }
                                    else {
                                        $('#CoveredRisk').UifSelect('setSelected', null);
                                        $("#divCoveredRisk").show();
                                        $("#divBranch").hide();
                                        $("#divCoverage").hide();
                                        $("#divObj").hide();
                                        $("#divProtection").hide();
                                        $("#divLineBusiness").hide();
                                    }

                                    break;
                                case ParametrizationClauseConditionLevel.TechnicalBranch: //Ramo Tecnico
                                    if (data.result[0].LineBusinessServiceQueryModel !== null) {
                                        $("#divCoverage").hide();
                                        $("#divObj").hide();
                                        $("#divProtection").hide();
                                        $("#divBranch").hide();
                                        $("#divCoveredRisk").hide();
                                        $("#divLineBusiness").show();
                                        $('#LineBusiness').UifSelect('setSelected', data.result[0].LineBusinessServiceQueryModel.Id);
                                    }
                                    else {
                                        $("#divCoverage").hide();
                                        $("#divObj").hide();
                                        $("#divProtection").hide();
                                        $("#divBranch").hide();
                                        $("#divCoveredRisk").hide();
                                        $("#divLineBusiness").show();
                                        $('#LineBusiness').UifSelect('setSelected', null);
                                    }

                                    break;
                                case ParametrizationClauseConditionLevel.Coverage:// Cobertura
                                    if (data.result[0].CoverageServiceQueryModel !== null) {
                                        $('#Coverage').val(data.result[0].CoverageServiceQueryModel.Description + '(' + data.result[0].CoverageServiceQueryModel.Id + ')');
                                        $("#ObjectInsurance").val(data.result[0].CoverageServiceQueryModel.InsuredObjectServiceQueryModel.Description);
                                        $("#Protection").val(data.result[0].CoverageServiceQueryModel.PerilServiceQueryModel.Description);
                                        $("#divCoverage").show();
                                        $("#divObj").show();
                                        $("#divProtection").show();
                                        $("#divBranch").hide();
                                        $("#divCoveredRisk").hide();
                                        $("#divLineBusiness").hide();
                                    }
                                    else {
                                        $('#Coverage').val("");
                                        $("#ObjectInsurance").val("");
                                        $("#Protection").val("");
                                        $("#divCoverage").show();
                                        $("#divObj").show();
                                        $("#divProtection").show();
                                        $("#divBranch").hide();
                                        $("#divCoveredRisk").hide();
                                        $("#divLineBusiness").hide();
                                    }
                                    break;
                            }
                        }
                        else {
                            ClauseParametrization.ShowListClause(data.result);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchClauses, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchClauses, 'autoclose': true });
                    ClauseParametrization.clearFormClause();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true });
            }
        });
    }


    static TextListStatic(text) {
        $("#inputPrecatalogedText").data("Object", text);
        $("#inputPrecatalogedText").val(text.Description + '(' + text.Id + ')');
    }

    static ShowTextList(dataTable) {
        $('#tblResultListClause').UifDataTable('clear');
        $('#tblResultListClause').UifDataTable('addRow', dataTable);
        $('#modalListSearchText').UifModal('showLocal', AppResources.LabelSelectText);
    }
    static ShowTextListCoverage(dataTable) {
        $('#tblResultListCoverage').UifDataTable('clear');
        $('#tblResultListCoverage').UifDataTable('addRow', dataTable);
        $('#modalListSearchCoverage').UifModal('showLocal', AppResources.LabelSelectText);
    }

    static ShowListClause(dataTable) {
        $('#tblResultListClauseTotal').UifDataTable('clear');
        $('#tblResultListClauseTotal').UifDataTable('addRow', dataTable);
        $('#modalListSearchClause').UifModal('showLocal', AppResources.LabelSelectText);
    }

    static GetTextsByNameLevelIdConditionLevelId(name, conditionLevelId) {
        TextRequest.GetTextsByNameLevelIdConditionLevelId(name, Levels.General, conditionLevelId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    UnderwritingText.ShowTextList(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchTexts, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorLoadingTexts, 'autoclose': true });
            }
        });
    }

    static GetTextCoverage(description) {
        if (description.length >= 0) {
            ClauseParametrizationRequest.GetTextCoverage(description).done(function (data) {
                if (data.success) {
                    if (data.result.CoverageServiceModels !== null && data.result.CoverageServiceModels.length > 0) {
                        if (data.result.CoverageServiceModels.length === 1) {
                            $("#Coverage").data("Object", data.result.CoverageServiceModels[0]);
                            $("#Coverage").val(data.result.CoverageServiceModels[0].Description + '(' + data.result.CoverageServiceModels[0].Id + ')');
                            $("#ObjectInsurance").val(data.result.CoverageServiceModels[0].InsuredObjectServiceQueryModel.Description);
                            $("#Protection").val(data.result.CoverageServiceModels[0].PerilServiceQueryModel.Description);
                        }
                        else {
                            ClauseParametrization.ShowTextListCoverage(data.result.CoverageServiceModels);
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    static GetLevels() {
        ClauseParametrizationRequest.GetLevels().done(response => {
            let result = response.result;
            if (response.success) {
                $("#Level").UifSelect({ sourceData: result });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static GetCommercialBranch() {
        ClauseParametrizationRequest.GetCommercialBranch().done(response => {
            let result = response.result;
            if (response.success) {
                $("#CommercialBranch").UifSelect({ sourceData: result });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static GetCoveredRisk() {
        ClauseParametrizationRequest.GetCoveredRisk().done(response => {
            let result = response.result;
            if (response.success) {
                let comboConfig = { sourceData: result };
                $("#CoveredRisk").UifSelect(comboConfig);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static GetLineBusiness() {
        ClauseParametrizationRequest.GetLineBusiness().done(response => {
            let result = response.result;
            if (response.success) {
                let comboConfig = { sourceData: result };
                $("#LineBusiness").UifSelect(comboConfig);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static getClause(data) {

        $("#listClause").UifListView({
            displayTemplate: "#templateClause",
            sourceData: data,
            selectionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: ClauseParametrization.deleteCallbackListClause,
        });
    }

    static editClause(event, result, index) {
        //ClauseParametrization.clearFormClause();
        glbClauseEdit = result;
        glbClauseEdit.Index = index;
        $("#Id").val(result.Id);
        $("#inputNameLoad").val(result.Name);
        $("#inputTitleLoad").val(result.Title);
        $("#Level").UifSelect("setSelected", result.Level);
        $("#InputStartDate").UifDatepicker('setValue', FormatDate(result.InputStartDate, 1));
        $("#DueDate").UifDatepicker('setValue', FormatDate(result.DueDate, 1));
        $("#inputPrecatalogedText").val(result.inputPrecatalogedText);
        $("#ClauseText").val(result.ClauseText);
        //Take the html data from CKEditor ClauseText instance
        //CKEDITOR.instances.ClauseText.setData(result.ClauseText);
        if (result.Required) {
            $('#required').prop('checked', true);
        }
        else {
            $('#required').prop('checked', false);
        }

        switch (parseInt($("#Level").val())) {

            case ParametrizationClauseConditionLevel.Independent: //Independiente
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.Prefix: // ramo comercial
                $('#CommercialBranch').UifSelect('setSelected', result.CommercialBranch);
                $("#divBranch").show();
                $("#divCoveredRisk").hide();
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.RiskType:// tipo de riesgo
                $('#CoveredRisk').UifSelect('setSelected', result.CoveredRisk);
                $("#divCoveredRisk").show();
                $("#divBranch").hide();
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divLineBusiness").hide();
                break;
            case ParametrizationClauseConditionLevel.TechnicalBranch: //ramo tecnico
                $("#divCoverage").hide();
                $("#divObj").hide();
                $("#divProtection").hide();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                $("#divLineBusiness").show();
                $("#LineBusiness").UifSelect('setSelected', result.LineBusiness);

                break;
            case ParametrizationClauseConditionLevel.Coverage:// Cobertura

                if (result.CoverageName == null && result.Coverage == null) {
                    $("#Coverage").val("");
                    $("#ObjectInsurance").val(result.ObjectInsurance);
                    $("#Protection").val(result.Protection);
                    $("#divCoverage").show();
                    $("#divObj").show();
                    $("#divProtection").show();
                    $("#divBranch").hide();
                    $("#divCoveredRisk").hide();
                    $("#divLineBusiness").hide();
                }
                else {
                    $("#Coverage").data("Object", { Id: result.Coverage, Description: result.CoverageName });
                    $('#Coverage').val(result.CoverageName + '(' + result.Coverage + ')');
                    $("#ObjectInsurance").val(result.ObjectInsurance);
                    $("#Protection").val(result.Protection);
                    $("#divCoverage").show();
                    $("#divObj").show();
                    $("#divProtection").show();
                    $("#divBranch").hide();
                    $("#divCoveredRisk").hide();
                    $("#divLineBusiness").hide();
                }


                break;
        }
    }

    static clearFormClause() {
        ClearValidation("#formClause");
        glbClauseEdit = null;
        $("#Id").val("");
        $("#Name").val("");
        $("#Name").focus();
        $("#Title").val("");
        $("#Level").UifSelect("setSelected", null);
        $("#InputStartDate").val("");
        $("#DueDate").val("");
        $("#required").val("");
        $("#inputPrecatalogedText").val("");
        //$("#ClauseText").val(CKEDITOR.instances.ClauseText.setData(''));
        $("#ClauseText").val("");
        $("#ClauseText").val("");
        //$("#inputPrecatalogedText").data("Object").TextBody = '';
        $("#ObjectInsurance").val("");
        $("#Protection").val("");
        $('#required').prop('checked', false);
        $("#Coverage").val("");
        $("#Coverage").data("Object", { Id: null, Description: null });
        $("#inputClauseSearch").val("");
        $("#CoveredRisk").UifSelect("setSelected", null);
        $("#divCoveredRisk").hide();
        $("#CommercialBranch").UifSelect("setSelected", null);
        $("#LineBusiness").UifSelect('setSelected', null);
        $("#divCoverage").hide();
        $("#divObj").hide();
        $("#divProtection").hide();
        $("#divLineBusiness").hide();
        $("#divBranch").hide();
    }

    addClause() {
        
        //ClauseText toma la informacion almacenada en la instancia del editos
        //var htmlDescription = CKEDITOR.instances.ClauseText.getData();
        $('#ClauseText').val();
        var formClause = $("#formClause").serializeObject();
        if (formClause.DueDate != "" && !(formClause.DueDate === formClause.InputStartDate) && CompareDates(formClause.DueDate, formClause.InputStartDate) == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': "La fecha de vencimiento debe ser mayor a la fecha  de inicio", 'autoclose': true })
        }
        else {
            if (formClause.LevelDescription = $("#Level").UifSelect("getSelectedText") === "Cobertura") {
                formClause.LevelDescription = $("#Level").UifSelect("getSelectedText")
                formClause.Coverage = $("#Coverage").data("Object").Id;
                formClause.CoverageName = $("#Coverage").data("Object").Description;
            }
            else {
                formClause.LevelDescription = $("#Level").UifSelect("getSelectedText")
                //formClause.CommercialBranch = '';
            }

            if ($('#required').is(':checked')) {
                formClause.Required = true;
            }

            if (ClauseParametrization.validateAddClause(formClause)) {

                if (parseInt(formClause.Id) > 0) {
                    switch (parseInt($("#Level").val())) {
                        case ParametrizationClauseConditionLevel.Independent: //Independiente
                            formClause.CommercialBranch = '';
                            break;
                        case ParametrizationClauseConditionLevel.Prefix: // ramo comercial
                            formClause.CommercialBranch = $('#CommercialBranch').val();
                            break;
                        case ParametrizationClauseConditionLevel.RiskType:// tipo de riesgo
                            formClause.CommercialBranch = $('#CoveredRisk').val();
                            break;
                        case ParametrizationClauseConditionLevel.TechnicalBranch: //Ramo Tecnico
                            formClause.CommercialBranch = $('#LineBusiness').val();
                            break;
                        case ParametrizationClauseConditionLevel.Coverage:// Cobertura
                            formClause.CommercialBranch = $('#Coverage').val();
                            break;
                    }
                    ClauseParametrization.UpdateClauseParametrization(formClause);
                }
                else {
                    ClauseParametrization.InsertClauseParametrization(formClause);
                }
                //CKEditor Instance must be clear from here, in clearFormClause get into a bug when is called from EditClause.
                //CKEDITOR.instances.ClauseText.setData('');

                $('#ClauseText').val('');

                ClauseParametrization.clearFormClause();
            }
        }
    }

    static UpdateClauseParametrization(formClause) {

        formClause.StatusTypeService = ParametrizationStatus.Update;
        $("#listClause").UifListView("editItem", glbClauseEdit.Index, formClause);
    }

    static InsertClauseParametrization(formClause) {
        formClause.StatusTypeService = ParametrizationStatus.Create;
        if (glbClauseEdit != null) {
            $("#listClause").UifListView("editItem", glbClauseEdit.Index, formClause);
        }
        else {
            $("#listClause").UifListView("addItem", formClause);
        }
    }

    static createClause() {
        var clauses = $("#listClause").UifListView('getData');
        //$.each(glbListClauseDelete, function (index, item) {
        //    clauses.push(item);
        //})
        var clauseFilter = clauses.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        if (clauseFilter.length > 0) {
            request('Parametrization/Clauses/CreateParametrizationClause', JSON.stringify({ parametrizationClauseVM: clauseFilter }), 'POST', AppResources.ErrorSaveClauses, ClauseParametrization.confirmCreateParametrizationClause);
            request('Parametrization/Clauses/GetParametrizationClause', null, 'GET', AppResources.ErrorSearchClauses, ClauseParametrization.getClause);
        }
    }

    static confirmCreateParametrizationClause(data) {
        glbListClauseDelete = [];
        ClauseParametrization.clearFormClause();
        request('Parametrization/Clauses/GetParametrizationClause', null, 'GET', AppResources.ErrorSearchClauses, ClauseParametrization.getClause)
        if (data.Message === null) {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                "Agregados" + ':' + data.TotalAdded + '<br> ' +
                "Actualizados" + ':' + data.TotalModified + '<br> ' +
                "Eliminados" + ':' + data.TotalDeleted + '<br> ' +
                "Errores" + ':' + data.Message,
            'autoclose': true
        });
    }

    static deleteCallbackListClause(deferred, result) {
        deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined) //Los registros q' esten en DB no se eliminan de la listview se muestra mensaje de eliminado
        {
            result.StatusTypeService = ParametrizationStatus.Delete;
            //glbListClauseDelete.push(result);
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listClause").UifListView("addItem", result);
        }

    }

    static validateAddClause(formClause) {
        var band = false;
        if ($("#formClause").valid()) {
            band = true;
        }
        return band;
    }

    static validateDueDate(formClause) {

        if (formClause.InputStartDate < formClause.DueDate && formClause.DueDate != "") {
            $.UifNotify('show', { 'type': 'info', 'message': "La fecha de vencimiento debe ser mayor o igual a la fecha  de inicio", 'autoclose': true })
        }
        else {

        }
    }

    exportExcel() {
        request('Parametrization/Clauses/GenerateFileToExport', null, 'GET', "Error descargando archivo", ClauseParametrization.generateFileToExport);
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    static generateFileToExport(data) {
        DownloadFile(data);
    }
}