class RiskSuretyContract extends Uif2.Page {
    getInitialState() { }

    bindEvents() {
        
        $("#btnContractObject").on('click', function () {
            
            $('#inputTextPrecataloged').val('');
            $('#Observations').hide();
            RiskSuretyContract.LoadPartialContractObject();
        });

        $("#inputTextPrecataloged").on('buttonClick', function () {


            if ($.trim($("#inputTextPrecataloged").val()) != "") {
                RiskSuretyContract.GetContratcObjectsByNameLevelIdConditionLevelId($("#inputTextPrecataloged").val().trim(), glbPolicy.Prefix.Id);
            }
        });

        $("#btnContractObjectClose").on("click", function () {

            RiskSurety.HidePanelsRisk(MenuType.ContractObject);
            if (loadRisks) {
                RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
                loadRisks = false;
            }
        });

        $("#btnContractObjectSave").on("click", function () {
            RiskSuretyContract.SaveContractObject();
        });

        $('#tableTextResults tbody').on('click', 'tr', function (e) {
            e.preventDefault();
            $("#inputTextPrecataloged").val($(this).children()[1].innerHTML);
            $("#inputTextEdit").val($(this).children()[2].innerHTML);
        });

        $("#btnTextCancel").on("click", function () {
            $('#modalTextSearch').UifModal("hide");
        });

        $("#btnTextSelect").on("click", function () {
            $('#modalTextSearch').UifModal("hide");
        });
    }

    static LoadPartialContractObject() {
        $("#mainRiskSurety").validate();
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            $('#inputText').prop('disabled', true);
            $('#btnContractObjectSave').prop('disabled', true);
        }
        if (glbRisk.Id == 0) {
            RiskSurety.SaveRisk(MenuType.ContractObject, 0, false, false);
            loadRisks = true;
        }
        if (glbRisk.Id > 0) {
            RiskSurety.ShowPanelsRisk(MenuType.ContractObject);
            if (glbRisk.ContractObject != undefined) {
                $("#inputText").val(glbRisk.ContractObject.TextBody);
            }
            else {
                $("#inputText").val('');
            }
        }
    }

    static GetContratcObjectsByNameLevelIdConditionLevelId(name, conditionLevelId) {
        var number = parseInt(name, 10);

        if (!isNaN(number) || name.length > 2) {
            RiskSuretyContractRequest.GetContratcObjectsByNameLevelIdConditionLevelId(name, Levels.Risk, conditionLevelId).done(function (data) {
                if (data.success) {
                    //if (data.result.length > 0) {
                    //    RiskSuretyContract.ShowContractObjectList(data.result);
                    //}
                    //else {
                    //    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorContractObject, 'autoclose': true });
                    //}
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorLoadingTexts, 'autoclose': true });
                }
            });
        }
    }

    static ValidateContractObject() {
        //SE HACE MODIFICACION DE OBLIGATORIEDAD AL CAMPO TEXTO POR COMPATIBILIDAD CON R1
        var textModel = null;
        if (glbRisk.Id != 0) {
            textModel = glbRisk.Text;
        } 
         else {
            textModel = $("#formTexts").serializeObject();
        }
        if (textModel != null && textModel.TextBody != "") {
            return true;
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoDataTextRisk, 'autoclose': true });
            return false;
        }
    }

    static SaveContractObject() {


        $("#formTexts").validate();

        if ($("#formTexts").valid()) {
            RiskSuretyContractRequest.SaveContractObject(glbRisk.Id, $("#formTexts").serializeObject()).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        RiskSurety.HidePanelsRisk(MenuType.ContractObject);
                        if (loadRisks) {
                            RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
                            loadRisks = false;
                        }
                        else {
                            glbRisk.ContractObject = data.result;
                        }
                        if (window[glbRisk.Object] != null) {
                            window[glbRisk.Object].LoadSubTitles(4);
                        }
                    }
                    else
                    {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveTexts, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveTexts, 'autoclose': true });
                }
            });
        }
    }

    static ShowContractObjectList(dataTable) {
        $('#tableTextResults').UifDataTable('clear');
        $('#tableTextResults').UifDataTable('addRow', dataTable);
        $('#modalTextSearch').UifModal('showLocal', Resources.Language.LabelSelectText);
        $('#inputTextEdit').val('');
    }
}
