var typetext = 0;
class ContractObject extends Uif2.Page {

    getInitialState() {
        ContractObjectRequest.GetBranches().done(function (data) {
            if (data.success) {
                $("#selectBranchRisk").UifSelect({ sourceData: data.result });
            }
        });
        ContractObjectRequest.GetPrefixes().done(dataPrefix => {
            if (dataPrefix.success) {
                $('#selectPrefixRisk').UifSelect({ sourceData: dataPrefix.result });
            }
        });
        if (glbPolicy == null) {
            glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, Endorsement: {}, Change: false, BoolParameter: false };
        }
        $("#inputTextPolicy").prop('disabled', true);
        $("#inputTextRisk").prop('disabled', true);
        $("#inputTextPrecatalogedPolicy").prop('disabled', true);
        $("#inputTextPrecatalogedRisk").prop('disabled', true);
    }

    bindEvents() {
        $("#inputTextPrecataloged").on('buttonClick', UnderwritingText.SearchText);
        $('#inputPolicyNumber1').on('buttonClick', function () {
            glbPolicy.Message = "";
            if ($('#inputPolicyNumber1').val().trim().length > 0) {
                ContractObject.searchPolicyNumber();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchPrefixAndDocumentNumRequired, 'autoclose': true });
            }
        });

        $('#chkUpdatePolicy').on("click", ContractObject.UpdatePolicy);
        $('#chkUpdateRisk').on("click", ContractObject.UpdateRisk);
        $('#btnSave').on('click', ContractObject.SaveContractObject);
        $('#selectRisk').on('itemSelected', function (event, selectedItem) {
            ContractObject.selectrisk(event, selectedItem);

        });

        $("#inputTextPrecatalogedPolicy").on('buttonClick', function () {
            if ($.trim($("#inputTextPrecatalogedPolicy").val()) != "") {
                typetext = 1;
                ContractObject.GetTextsByNameLevelIdConditionLevelId($("#inputTextPrecatalogedPolicy").val(), $("#selectPrefixRisk").UifSelect("getSelected"), Levels.General);
            }
        });

        $("#inputTextPrecatalogedRisk").on('buttonClick', function () {
            if ($.trim($("#inputTextPrecatalogedRisk").val()) != "") {
                typetext = 2;
                ContractObject.GetTextsByNameLevelIdConditionLevelId($("#inputTextPrecatalogedRisk").val(), $("#selectPrefixRisk").UifSelect("getSelected"), Levels.Risk);
            }
        });

        $('#tableTextResults tbody').on('click', 'tr', function (e) {
            e.preventDefault();
            $("#inputTextPrecataloged").val($(this).children()[1].innerHTML);
            $("#inputTextEdit").val($(this).children()[2].innerHTML);
        });

        $("#btnTextCancel").on("click", this.TextCancel);
        $("#btnTextSelect").click(this.TextSelect);   
    }

    TextCancel() {
        $('#modalTextSearch').UifModal("hide");
    }

    TextSelect()
    {
        if (typetext == 1) {

            if ($("#inputTextPolicy").val().trim().length == 0) {
                $("#inputTextPolicy").val($("#inputTextEdit").val());
            }
            else {
                $("#inputTextPolicy").val($("#inputTextPolicy").val() + ' \n ' + $("#inputTextEdit").val());
            }
        }
        if (typetext == 2) {

            if ($("#inputTextRisk").val().trim().length == 0) {
                $("#inputTextRisk").val($("#inputTextEdit").val());
            }
            else {
                $("#inputTextRisk").val($("#inputTextRisk").val() + ' \n ' + $("#inputTextEdit").val());
            }
        }
            $('#modalTextSearch').UifModal("hide");
        

        typetext = 0;
    }

    static searchPolicyNumber() {
        $('#formContractObject').validate();
        if ($('#formContractObject').valid()) {
            ContractObject.ValidateTemporal($('#inputPolicyNumber1').val(), $("#selectBranchRisk").UifSelect("getSelected"), $("#selectPrefixRisk").UifSelect("getSelected")).then(function (dataTemporal) {
                var branch = $("#selectBranchRisk").UifSelect("getSelected");
                var prefix = $("#selectPrefixRisk").UifSelect("getSelected");

                if (branch == "" && prefix == "") {
                    branch = 0
                    prefix = 0
                }
                ContractObjectRequest.GetCompanyEndorsementsByFilterPolicy(branch, prefix, $('#inputPolicyNumber1').val(), true).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            ContractObjectRequest.GetCurrentPolicyByEndorsementId(data.result.Id, true).done(function (data) {
                                if (data.success) {
                                    if (data.result != null) {
                                        data.result.Id = dataTemporal.Id;
                                        data.result.EndorsementTemporal = dataTemporal.EndorsementType;
                                        data.result.TemporalType = TemporalType.Endorsement;
                                        $("#selectBranchRisk").UifSelect("setSelected", data.result.Branch.Id);
                                        $("#selectPrefixRisk").UifSelect("setSelected", data.result.Prefix.Id);
                                        $("#inputTextPolicy").val(data.result.Endorsement.Text.TextBody);
                                        glbPolicy = data.result;
                                        ContractObjectRequest.GetRiskByPolicyId(data.result.Endorsement.PolicyId, $("#selectPrefixRisk").val()).done(function (dataRisk) {
                                            if (dataRisk.success) {
                                                glbRisk = dataRisk.result;
                                                glbRisk.forEach(function (item, index) {
                                                    item.Description = item.Risk.MainInsured.Name;
                                                    item.Id = item.Risk.RiskId;
                                                });
                                                $("#selectRisk").UifSelect({ sourceData: glbRisk });
                                                if (dataRisk.result.length == 1) {
                                                    $("#inputTextRisk").val(glbRisk[0].Risk.Text.TextBody);
                                                    $("#selectRisk").UifSelect("setSelected", glbRisk[0].Id);
                                                    $("#selectRisk").UifSelect("disabled", true);
                                                } else {
                                                    $("#selectRisk").UifSelect("disabled", false);
                                                }
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'danger', 'message': dataRisk.result, 'autoclose': true });
                                            }
                                        });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                }
                            });
                            $("#btnEndorsementList").show();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Datos Faltantes en el Formulario", 'autoclose': true });
        }
    }

    static ValidateTemporal(policyNumber, branchId, prefixId) {
        var dfLoad = $.Deferred();
        if (branchId == "" && prefixId == "") {
            branchId = 0
            prefixId = 0
        }
        ContractObjectRequest.GetValidateOriginPolicy(policyNumber, prefixId, branchId).done(function (data) {
            glbPolicy.Id = 0;
            if (data.success) {
                if (data.result != null) {
                    dfLoad.resolve(data.result);
                }
                dfLoad.resolve({ Id: 0 });
            }
            else {
                glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, Endorsement: { EndorsementType: EndorsementType.Modification }, Change: false };
                dfLoad.resolve({ Id: -1 });
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ValidateOriginPolicy, 'autoclose': true });
            }
        });
        return dfLoad.promise();
    }

    static UpdatePolicy() {
        if ($('#inputPolicyNumber1').val().trim().length > 0) {
            if ($('#chkUpdatePolicy').prop("checked")) {
                $("#inputTextPolicy").prop('disabled', false);
                $("#inputTextPrecatalogedPolicy").prop('disabled', false);
            } else {
                $("#inputTextPolicy").prop('disabled', true);
                $("#inputTextPrecatalogedPolicy").prop('disabled', true);
            }

        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchPrefixAndDocumentNumRequired, 'autoclose': true });
            $('#chkUpdatePolicy').prop("checked", false);
        }
    }

    static UpdateRisk() {
        if ($('#inputPolicyNumber1').val().trim().length > 0) {
            if ($('#chkUpdateRisk').prop("checked")) {
                $("#inputTextRisk").prop('disabled', false);
                $("#inputTextPrecatalogedRisk").prop('disabled', false);
            } else {
                $("#inputTextRisk").prop('disabled', true);
                $("#inputTextPrecatalogedRisk").prop('disabled', true);
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchPrefixAndDocumentNumRequired, 'autoclose': true });
            $('#chkUpdateRisk').prop("checked", false);
        }
    }

    static SaveContractObject() {
        $('#formContractObject').validate();
        if ($('#formContractObject').valid()) {
            if ($('#chkUpdatePolicy').prop("checked") || $('#chkUpdateRisk').prop("checked")) {
                ContractObjectRequest.SaveContractObjectPolicyId(glbPolicy.Endorsement.Id, parseInt($("#selectRisk").val()), $("#inputTextRisk").val(), $("#inputTextPolicy").val()).done(function (data) {
                    if (data.success) {
                        if (data.result) {
                            var endoChangeText = {
                                policiId: glbPolicy.Endorsement.PolicyId,
                                endorsementId: glbPolicy.Endorsement.Id,
                                textOldPolicy: glbPolicy.Endorsement.Text.TextBody,
                                textNewPolicy: $("#inputTextPolicy").val(),
                                textOldRisk: glbRisk[0].Risk.Text.TextBody,
                                textnewRisk: $("#inputTextRisk").val(),
                                riskId: parseInt($("#selectRisk").val()),
                                userId: glbPolicy.UserId,
                                reason: $("#inputReason").val()
                            };
                            ContractObjectRequest.SaveLog(endoChangeText).done(function (datalog) {
                                if (datalog.success) {
                                    if (datalog.result) {
                                        ContractObject.ClearContractObject();
                                    }
                                } else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveContractObject, 'autoclose': true });
                                }
                            });

                        }
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveContractObject, 'autoclose': true });
                    }
                });
            }
        }
    }

    static ClearContractObject() {
        $('#selectBranchRisk').UifSelect('setSelected', null);
        $('#selectPrefixRisk').UifSelect('setSelected', null);
        $('#selectRisk').UifSelect('setSelected', null);
        $('#inputTextPolicy').val("");
        $('#inputTextRisk').val("");
        $('#inputReason').val("");
        $('#inputTextPrecatalogedPolicy').val("");
        $('#inputTextPrecatalogedRisk').val("");
        $('#inputPolicyNumber1').val("");
        $('#chkUpdateRisk').prop("checked", false);
        $('#chkUpdatePolicy').prop("checked", false);
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UpdateContractObjectRight, 'autoclose': true });
    }

    static selectrisk(event, selectedItem) {
        glbRisk.forEach(function (item, index) {
            if (selectedItem.Id == item.Id) {
                $("#inputTextRisk").val(item.Risk.Text.TextBody);
            }
        });
    }

    static GetTextsByNameLevelIdConditionLevelId(name, conditionLevelId, Level) {
        ContractObjectRequest.GetTextsByNameLevelIdConditionLevelId(name, Level, conditionLevelId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    ContractObject.ShowTextList(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchTexts, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorLoadingTexts, 'autoclose': true });
            }
        });
    }

    static ShowTextList(dataTable) {
        $('#tableTextResults').UifDataTable('clear');
        $('#tableTextResults').UifDataTable('addRow', dataTable);
        $('#modalTextSearch').UifModal('showLocal', Resources.Language.LabelSelectText);
        $('#inputTextEdit').val('');
    }
}
