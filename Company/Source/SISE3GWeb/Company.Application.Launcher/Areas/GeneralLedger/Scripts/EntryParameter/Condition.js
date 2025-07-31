var accountingRuleId = 0;
var accountingRuleDescription = "";
var saveAccountingRulePromise;
var loadConditionsPromise;
var conditionId = 0;
var excecutingType = 0; //indica si es (1) regla, (2) condicion o (3) resultado.
var modalPromise;
var getAccountingAccountMaskListPromise;
var validateMaskUniqueParameterPromise;
var validateMaskAccountLengthPromise;
var validateUniqueMaskPromise;
var validateMaskPositionPromise;
var resultId = 0;
var time;
var lastConditionIdPromise;
var loadMasksPromise;

$(() => {
    new Condition();
});

class ConditionModel {
    constructor() {
        this.If = "";
        this.ConditionId = 0;
        this.ParameterId = 0;
        this.ParameterDescription = "";
        this.Operator = "";
        this.Value = "";
        this.RightResultId = 0;
        this.LeftResultId = 0;
        this.ResultId = 0;
        this.AccountingRuleId = 0;
    }
}

class ResultModel {
    constructor() {
        this.ResultId = 0;
        this.AccountingNatureId = "";
        this.AccountingNatureDescription = "";
        this.AccountingAccountId = 0;
        this.AccountingAccountNumber = "";
        this.AccountingAccountName = "";
        this.ParameterId = 0;
        this.ParameterDescription = "";
        this.Value = "";
    }
}

class AccountingRuleModel {
    constructor() {
        this.ModuleId = 0;
        this.AccountingRuleId = 0;
        this.AccountingRuleDescription = "";
        this.AccountingRuleObservations = "";
    }
};

class AccountingAccountMaskModel {
    constructor() {
        this.Id = 0;
        this.ParameterId = 0;
        this.Position = 0;
        this.Mask = "";
        this.ResultId = 0;
    }
};

class Condition extends Uif2.Page {
    getInitialState() {
        $("#setUpAccount").removeClass("glyphicon glyphicon-check");
        $("#setUpAccount").addClass("glyphicon glyphicon-unchecked");
        $("#setUpAccountCheck").prop("checked", false);
        $("#accountingAccountMaskRegion").hide();
        $("#accountingAccountMaskEditor").hide();
        $("#accountingAccountMaskList").UifListView({
            customDelete: false,
            customAdd: true,
            customEdit: true,
            add: true,
            edit: false,
            delete: true,
            displayTemplate: "#accountingAccountMaskListTemplate",
            addTemplate: '#accountingAccountMask-add-template',
            deleteCallback: deleteAccountingAccountMask,
            selectionType: 'simple',
            title: Resources.Parameters,
            height: 100
        });
    }
    bindEvents() {
        //modulo
        $('#Module').on('itemSelected', this.ListAccountingRules);
        //delete
        $("#deleteRecordConfirmationModal").find("#confirmDelete").on('click', this.Delete);
        //reglas
        $("#accountingRules").on('rowSelected', this.LoadAccountingRuleDetails);
        $("#accountingRules").on('rowAdd', this.ShowAccountingRuleModal);
        $("#accountingRuleModal").find("#updateAccountingRule").on('click', this.UpdateAccountingRule);
        $("#accountingRuleModal").on('hidden.bs.modal', this.ClearAccountingRuleModalFields);
        $("#accountingRules").on('rowEdit', this.GetAccountingRule);
        $("#accountingRules").on('rowDelete', this.ShowAccountingRuleDeleteConfirmationModal);
        //condiciones
        $("#conditions").on('rowAdd', this.ShowConditionModal);
        $("#conditions").on('rowEdit', this.GetCondition);
        $("#conditions").on('rowDelete', this.ShowConditionDeleteConfirmationModal);
        $("#conditionModal").find("#addCondition").on('click', this.SaveCondition);
        $("#conditionModal").on('hidden.bs.modal', this.ClearConditionModalFields);
        //resultados
        $("#results").on('rowAdd', this.ShowResultModal);
        $("#resultModal").find("#addResult").on('click', this.SaveResult);
        $("#resultModal").on('hidden.bs.modal', this.ClearResultModalFields);
        $("#results").on('rowEdit', this.GetResult);
        $("#results").on('rowDelete', this.ShowResultDeleteConfirmationModal);
        //check
        $("#resultModal").find("#setUpAccount").on('click', this.ShowSetUpAccount);
        //AccountMask
        $("#resultModal").find("#accountingAccountMaskList").on('rowAdd', this.LoadAddMask);
        $("#resultModal").find("#accountingAccountMaskList").on('rowEdit', this.LoadEditMask);
        $("#resultModal").find("#cancelMask").on('click', this.HideMaskEditor);
        $("#resultModal").find("#addMask").on('click', this.SaveMask);
        $("#resultModal").find("#accountingAccount").on('blur', this.AssembledAccount);
        //$('#conditionValue').ValidatorKey(ValidatorType.Number, 1, 1);
    }
    //region module
    ListAccountingRules() {
        if ($('#Module').val() > 0) {
            $("#accountingRules").UifDataTable('clear');

            var loadAccountingRulesPromise = new Promise(function (resolve, reject) {
                $.ajax({
                    type: "POST",
                    url: GL_ROOT + "EntryParameter/LoadAccountingRuleModelListByModuleId",
                    data: JSON.stringify({
                        "moduleId": $('#Module').val()
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (accountingRulesData) {
                        resolve(accountingRulesData);
                    }
                });
                $("#accountingRulesAlert").UifAlert('hide');
                $("#accountingRuleInformationMessage").UifAlert('hide');
                $("#selectedAccountingRuleAlert").UifAlert('hide');
                $('#conditions').UifDataTable('clear');
                $('#results').UifDataTable('clear')
            });

            loadAccountingRulesPromise.then(function (accountingRulesData) {
                if (accountingRulesData.data.length > 0) {
                    for (var i = 0; i < accountingRulesData.data.length; i++) {
                        var accountingRuleModel = new AccountingRuleModel();
                        accountingRuleModel.ModuleId = accountingRulesData.data[i].ModuleId;
                        accountingRuleModel.AccountingRuleId = accountingRulesData.data[i].AccountingRuleId;
                        accountingRuleModel.AccountingRuleDescription = accountingRulesData.data[i].AccountingRuleDescription;
                        accountingRuleModel.AccountingRuleObservations = accountingRulesData.data[i].AccountingRuleObservations;

                        $("#accountingRules").UifDataTable('addRow', accountingRuleModel)
                    }
                } else {
                    $("#accountingRules").UifDataTable('clear');
                }
            });
        } else {
            $("#accountingRules").UifDataTable('clear');
        }
    }
    //endregion module

    //region delete
    Delete() {
        $("#deleteRecordConfirmationModal").UifModal('hide');
        DeleteRecord(excecutingType);
    }
    //endregion delete

    //region rules
    LoadAccountingRuleDetails(event, data, position) {
        $("#rulesAlert").UifAlert('hide');
        accountingRuleId = data.AccountingRuleId;
        accountingRuleDescription = data.AccountingRuleDescription;
        $("#conditions").UifDataTable("clear");
        $("#results").UifDataTable("clear");
        //Se cargan las condiciones.
        GetConditionsByAccountingRuleIdAndModuleId(accountingRuleId, $("#Module").val()).then(function (accountingRuleData) {
            if (accountingRuleData.length > 0) {
                for (var i = 0; i < accountingRuleData.length; i++) {

                    var conditionModel = new ConditionModel();
                    conditionModel.If = Resources.Yes;
                    conditionModel.ConditionId = accountingRuleData[i].ConditionId;
                    conditionModel.ParameterId = accountingRuleData[i].ParameterId;
                    conditionModel.ParameterDescription = accountingRuleData[i].ParameterDescription;
                    conditionModel.Operator = accountingRuleData[i].OperatorDescription;
                    conditionModel.Value = accountingRuleData[i].Value;

                    $("#conditions").UifDataTable("addRow", conditionModel);
                }

                //Se carga el resultado.
                var conditionModel = new ConditionModel();
                GetLastConditionId(accountingRuleId).then(function (conditionData) {
                    conditionModel.ConditionId = conditionData;

                    GetResultByCondition(conditionModel).then(function (resultData) {
                        $("#results").UifDataTable("clear");

                        if (resultData.ResultId > 0) {
                            var resultModel = new ResultModel();
                            resultModel.ResultId = resultData.ResultId;
                            resultModel.AccountingNatureId = resultData.AccountingNatureId;
                            resultModel.AccountingNatureDescription = resultData.AccountingNatureDescription;
                            resultModel.AccountingAccountId = resultData.AccountingAccountId;
                            resultModel.AccountingAccountNumber = resultData.AccountingAccountNumber;
                            resultModel.AccountingAccountName = resultData.AccountingAccountName;
                            resultModel.ParameterId = resultData.ParameterId;
                            resultModel.ParameterDescription = resultData.ParameterDescription;
                            resultModel.Value = resultData.Value;

                            $("#results").UifDataTable("addRow", resultModel);
                        }
                    });
                });
            }
        });

        $("#accountingRuleInformationMessage").UifAlert('show', Resources.RuleCompletionWarning, "info");
        setTimeout(function () {
            $("#accountingRuleInformationMessage").UifAlert('hide');
        }, 5000)
        $("#selectedAccountingRuleAlert").UifAlert('show', Resources.SelectedConcept + ": " + accountingRuleDescription);
        setTimeout(function () {
            $("#selectedAccountingRuleAlert").UifAlert('hide');
        }, 5000)

        $("#rulesTab").click();
    }
    ShowAccountingRuleModal() {
        $("#conditions").UifDataTable("clear");
        $("#results").UifDataTable("clear");
        if ($('#Module').val() > 0) {
            $("#accountingRuleModal").UifModal('showLocal', Resources.AddNewRule)
            $("#accountingRulesAlert").UifAlert('hide');
        } else {
            $("#accountingRulesAlert").UifAlert('show', Resources.SelectModule, "warning");
            setTimeout(function () {
                $("#accountingRulesAlert").UifAlert('hide');
            }, 5000)
        }
    }
    UpdateAccountingRule() {
        var accountingRuleModel = new AccountingRuleModel();
        accountingRuleModel.AccountingRuleId = $("#accountingRuleModal").find("#AccountingRuleId").val();
        accountingRuleModel.ModuleId = $("#Module").val();
        accountingRuleModel.AccountingRuleDescription = $("#accountingRuleModal").find("#AccountingRuleDescription").val();
        accountingRuleModel.AccountingRuleObservations = $("#accountingRuleModal").find("#AccountingRuleObservations").val();

        SaveAccountingRule(accountingRuleModel);
        saveAccountingRulePromise.then(function (accountingRuleData) {
            if (accountingRuleData.Id > 0) {
                $("#accountingRulesAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                setTimeout(function () {
                    $("#accountingRulesAlert").UifAlert('hide');
                }, 5000)
                accountingRuleId = accountingRuleData.Id;
                accountingRuleDescription = accountingRuleData.Description;
                $("#accountingRuleModal").UifModal("hide");
                ClearAccountingRuleModal();
                $("#conditions").UifDataTable("clear");
                $("#results").UifDataTable("clear");
                GetConditionsByAccountingRuleIdAndModuleId(accountingRuleId, $("#Module").val()).then(function (accountingRuleData) {
                    if (accountingRuleData.length > 0) {
                        for (var i = 0; i < accountingRuleData.length; i++) {

                            var conditionModel = new ConditionModel();
                            conditionModel.If = Resources.Yes;
                            conditionModel.ConditionId = accountingRuleData[i].ConditionId;
                            conditionModel.ParameterId = accountingRuleData[i].ParameterId;
                            conditionModel.ParameterDescription = accountingRuleData[i].ParameterDescription;
                            conditionModel.Operator = accountingRuleData[i].OperatorDescription;
                            conditionModel.Value = accountingRuleData[i].Value;

                            $("#conditions").UifDataTable("addRow", conditionModel);
                        }

                        GetLastConditionId(accountingRuleId).then(function (conditionData) {
                            var conditionModel = new ConditionModel();
                            conditionModel.ConditionId = conditionData;

                            GetResultByCondition(conditionModel).then(function (resultData) {
                                $("#results").UifDataTable("clear");

                                if (resultData.ResultId > 0) {
                                    var resultModel = new ResultModel();
                                    resultModel.ResultId = resultData.ResultId;
                                    resultModel.AccountingNatureId = resultData.AccountingNatureId;
                                    resultModel.AccountingNatureDescription = resultData.AccountingNatureDescription;
                                    resultModel.AccountingAccountId = resultData.AccountingAccountId;
                                    resultModel.AccountingAccountNumber = resultData.AccountingAccountNumber;
                                    resultModel.AccountingAccountName = resultData.AccountingAccountName;
                                    resultModel.ParameterId = resultData.ParameterId;
                                    resultModel.ParameterDescription = resultData.ParameterDescription;
                                    resultModel.Value = resultData.Value;

                                    $("#results").UifDataTable("addRow", resultModel);
                                }
                            });
                        });
                    }

                    $("#accountingRuleInformationMessage").UifAlert('show', Resources.RuleCompletionWarning, "info");
                    setTimeout(function () {
                        $("#accountingRuleInformationMessage").UifAlert('hide');
                    }, 5000)
                    $("#selectedAccountingRuleAlert").UifAlert('show', Resources.SelectedConcept + ": " + accountingRuleDescription);
                    setTimeout(function () {
                        $("#selectedAccountingRuleAlert").UifAlert('hide');
                    }, 5000)
                    $("#rulesTab").click();
                });
            } else {
                $("#conditionsAlert").UifAlert('show', Resources.SaveError, "danger");
            }
            $("#accountingRules").UifDataTable("clear");
            $('#Module').trigger('change');

            var value = { label: 'AccountingRuleId', values: [accountingRuleId] };
            $("#accountingRules").UifDataTable('setSelect', value)
        });
    }
    ClearAccountingRuleModalFields() {
        ClearAccountingRuleModal();
    }
    GetAccountingRule(event, data, position) {
        accountingRuleId = data.AccountingRuleId;

        var editAccountingRulePromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: GL_ROOT + "EntryParameter/GetAccountingRule",
                data: JSON.stringify({
                    "accountingRuleId": accountingRuleId,
                    "moduleId": $("#Module").val()
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (accountingRuleData) {
                    resolve(accountingRuleData);
                }
            });
        });
        editAccountingRulePromise.then(function (accountingRuleData) {
            $("#accountingRuleModal").find("#AccountingRuleId").val(accountingRuleData.AccountingRuleId);
            $("#accountingRuleModal").find("#AccountingRuleDescription").val(accountingRuleData.AccountingRuleDescription);
            $("#accountingRuleModal").find("#AccountingRuleObservations").val(accountingRuleData.AccountingRuleObservations);

            $("#accountingRuleModal").UifModal('showLocal', Resources.EditAccountingRule);
        });
    }
    ShowAccountingRuleDeleteConfirmationModal(event, data, position) {
        accountingRuleId = data.AccountingRuleId;
        excecutingType = 1;
        $("#deleteRecordConfirmationModal").UifModal('showLocal', Resources.DeleteRecord);
    }
    //endregion rules

    //region Conditions
    ShowConditionModal() {
        if (accountingRuleId != 0) {
            $("#rulesAlert").UifAlert('hide');
            var controller = GL_ROOT + "EntryParameter/GetParameters?moduleId=" + $("#Module").val();
            $("#conditionModal").find("#conditionParameter").UifSelect({
                source: controller
            });
            $('#conditionModal').UifModal('showLocal', Resources.AddCondition);
            ClearConditionModal();
        } else {
            $("#rulesAlert").UifAlert('show', Resources.SelectARuleToContinue, 'warning');
        }
    }
    GetCondition(event, data, position) {
        conditionId = data.ConditionId;
        GetCondition(conditionId).then(function (conditionData) {
            $("#rulesAlert").UifAlert('hide');
            var controller = GL_ROOT + "EntryParameter/GetParameters?moduleId=" + $("#Module").val();
            $("#conditionModal").find("#conditionParameter").UifSelect({
                source: controller
            });
            $('#conditionModal').UifModal('showLocal', Resources.AddCondition);
            CheckModal('conditionModal');
            modalPromise.then(function (isShown) {
                if (isShown) {
                    clearTimeout(time);

                    setTimeout(function () {
                        $('#conditionModal').find("#conditionId").val(conditionData.ConditionId);
                        $('#conditionModal').find("#conditionParameter").val(conditionData.ParameterId);
                        $('#conditionModal').find("#operation").val(conditionData.OperatorId);
                        $('#conditionModal').find("#conditionValue").val(conditionData.Value);
                        $('#conditionModal').find("#RightResultId").val(conditionData.RightResultId);
                        $('#conditionModal').find("#ConditionResultId").val(conditionData.ResultId);
                    }, 500);

                }
            });
        });
    }
    ShowConditionDeleteConfirmationModal(event, data, position) {
        conditionId = data.ConditionId;
        excecutingType = 2;
        $("#deleteRecordConfirmationModal").UifModal('showLocal', Resources.DeleteRecord);
    }
    SaveCondition() {
        if (ValidateConditionAddForm()) {
            $("#conditionModal").find("#alertFormCondition").UifAlert('hide');
            var conditionModel = new ConditionModel();
            conditionModel.ConditionId = $("#conditionModal").find("#conditionId").val();
            conditionModel.ParameterId = $("#conditionModal").find("#conditionParameter").val();
            conditionModel.OperatorId = $("#conditionModal").find("#operation").val();
            conditionModel.Value = $("#conditionModal").find("#conditionValue").val();
            conditionModel.AccountingRuleId = accountingRuleId;
            conditionModel.RightResultId = $("#conditionModal").find("#RightResultId").val();
            conditionModel.ResultId = $("#conditionModal").find("#ConditionResultId").val();

            SaveCondition(conditionModel, $("#Module").val()).then(function (conditionData) {
                if (conditionData.Id > 0) {
                    $("#rulesAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    setTimeout(function () {
                        $("#rulesAlert").UifAlert('hide');
                    }, 5000)
                    $('#conditionModal').UifModal('hide');
                    ClearConditionModal();
                    $("#conditions").UifDataTable("clear");
                    GetConditionsByAccountingRuleIdAndModuleId(accountingRuleId, $("#Module").val()).then(function (accountingRuleData) {
                        if (accountingRuleData.length > 0) {
                            for (var i = 0; i < accountingRuleData.length; i++) {

                                var conditionModel = new ConditionModel();
                                conditionModel.If = Resources.Yes;
                                conditionModel.ConditionId = accountingRuleData[i].ConditionId;
                                conditionModel.ParameterId = accountingRuleData[i].ParameterId;
                                conditionModel.ParameterDescription = accountingRuleData[i].ParameterDescription;
                                conditionModel.Operator = accountingRuleData[i].OperatorDescription;
                                conditionModel.Value = accountingRuleData[i].Value;

                                $("#conditions").UifDataTable("addRow", conditionModel);
                            }
                        }

                        GetLastConditionId(accountingRuleId).then(function (conditionData) {
                            var conditionModel = new ConditionModel();
                            conditionModel.ConditionId = conditionData;

                            GetResultByCondition(conditionModel).then(function (resultData) {
                                $("#results").UifDataTable("clear");

                                if (resultData.ResultId > 0) {
                                    var resultModel = new ResultModel();
                                    resultModel.ResultId = resultData.ResultId;
                                    resultModel.AccountingNatureId = resultData.AccountingNatureId;
                                    resultModel.AccountingNatureDescription = resultData.AccountingNatureDescription;
                                    resultModel.AccountingAccountId = resultData.AccountingAccountId;
                                    resultModel.AccountingAccountNumber = resultData.AccountingAccountNumber;
                                    resultModel.AccountingAccountName = resultData.AccountingAccountName;
                                    resultModel.ParameterId = resultData.ParameterId;
                                    resultModel.ParameterDescription = resultData.ParameterDescription;
                                    resultModel.Value = resultData.Value;

                                    $("#results").UifDataTable("addRow", resultModel);
                                }
                            });
                        });
                    });
                } else {
                    $("#rulesAlert").UifAlert('show', Resources.SaveError, "danger");
                }
            });
        }
        
    }
    ClearConditionModalFields() {
        ClearConditionModal();
    }
    //endregion Conditions

    //region results
    ShowResultModal() {
        //se valida que existan condiciones.
        var conditionRecords = $("#conditions").UifDataTable('getData');

        if (conditionRecords.length > 0) {
            //se valida que no exista más de un registro en la tabla de resultados.
            var resultRecords = $("#results").UifDataTable('getData');

            if (resultRecords.length >= 1) {
                $("#rulesAlert").UifAlert("show", Resources.CanNotAddResult, "danger");
            } else {
                $("#resultModal").UifModal('showLocal', Resources.AddResult);
                CheckModal('resultModal');
                modalPromise.then(function (isShown) {
                    if (isShown) {
                        clearTimeout(time);
                        var controller = GL_ROOT + "EntryParameter/GetResultParameters?moduleId=" + $("#Module").val();
                        $("#resultModal").find("#resultParameter").UifSelect({
                            source: controller
                        });
                    }
                });
            }
        } else {
            $("#rulesAlert").UifAlert("show", Resour.NoConditionsWarning, "danger");
        }
    }
    SaveResult() {

        //Cuando se graba un resultado, este va ligado a la última condición del listado de condiciones.
        GetLastConditionId(accountingRuleId).then(function (conditionData) {
            if (conditionData > 0) {
                conditionId = conditionData;
            }

            $("#resultModal").find("#resultForm").validate();
            if ($("#resultModal").find("#resultForm").valid()) {

                var resultModel = new ResultModel();
                resultModel.ResultId = $("#resultModal").find("#ResultId").val();
                resultModel.AccountingAccountNumber = $("#resultModal").find("#accountingAccount").val();
                resultModel.AccountingNatureId = $("#resultModal").find("#accountingNature").val();
                resultModel.ParameterId = $("#resultModal").find("#resultParameter").val();

                var conditionModel = new ConditionModel();
                conditionModel.ConditionId = conditionId;
                conditionModel.AccountingRuleId = accountingRuleId;

                SaveResult(resultModel, conditionModel, $('#Module').val()).then(function (resultData) {
                    if (resultData.ResultId > 0) {

                        var resultModel = new ResultModel();
                        resultModel.ResultId = resultData.ResultId;
                        resultModel.AccountingNatureId = resultData.AccountingNatureId;
                        resultModel.AccountingNatureDescription = resultData.AccountingNatureDescription;
                        resultModel.AccountingAccountId = resultData.AccountingAccountId;
                        resultModel.AccountingAccountNumber = resultData.AccountingAccountNumber;
                        resultModel.AccountingAccountName = resultData.AccountingAccountName;
                        resultModel.ParameterId = resultData.ParameterId;
                        resultModel.ParameterDescription = resultData.ParameterDescription;
                        resultModel.Value = resultData.Value;

                        $("#results").UifDataTable("clear");

                        $("#results").UifDataTable("addRow", resultModel);
                        $("#rulesAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        setTimeout(function () {
                            $("#accountingRulesAlert").UifAlert('hide');
                        }, 5000)
                        $("#resultModal").UifModal('hide');
                    } else {
                        $("#rulesAlert").UifAlert('show', Resources.SaveError, "danger");
                        $("#resultModal").UifModal('hide');
                    }
                });
            }
        });
    }
    ClearResultModalFields() {
        ClearResultModalFields();
    }
    GetResult(event, data, position) {
        resultId = data.ResultId;
        GetResult(resultId);
    }
    ShowResultDeleteConfirmationModal(event, data, position) {
        resultId = data.ResultId;
        excecutingType = 3;
        $("#deleteRecordConfirmationModal").UifModal('showLocal', Resources.DeleteRecord);
    }
    //endregion results

    //region check
    ShowSetUpAccount() {
        if ($("#setUpAccount").hasClass("glyphicon glyphicon-check")) {
            $("#setUpAccount").removeClass("glyphicon glyphicon-check");
            $("#setUpAccount").addClass("glyphicon glyphicon-unchecked");
            $("#setUpAccountCheck").prop("checked", false);
        } else {
            $("#setUpAccount").removeClass("glyphicon glyphicon-unchecked");
            $("#setUpAccount").addClass("glyphicon glyphicon-check");
            $("#setUpAccountCheck").prop("checked", true);
        }

        if ($("#setUpAccountCheck").is(":checked")) {
            var controller = GL_ROOT + "EntryParameter/GetParameters?moduleId=" + $("#Module").val();
            $("#resultModal").find("#parameterId").UifSelect({
                source: controller
            });
            $("#resultModal").find("#accountingAccountMaskRegion").show();
        } else {
            $("#accountingAccountMaskRegion").hide();
        }
    }
    //endregion check

    //region mask
    LoadAddMask() {
        $("#accountingAccountMaskEditor").show();
    }
    LoadEditMask(event, data, position) {
        var accountingAccountMaskModel = new AccountingAccountMaskModel();
        accountingAccountMaskModel.Id = data.Id;
        accountingAccountMaskModel.ResultId = $("#resultModal").find("#ResultId").val();

        var editAccountingAccountMaskPromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: GL_ROOT + "EntryParameter/GetAccountingAccountMask",
                data: JSON.stringify({
                    "accountingAccountMaskModel": accountingAccountMaskModel
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (resultData) {
                    resolve(resultData);
                }
            });
        });

        editAccountingAccountMaskPromise.then(function (resultData) {

            $("#resultModal").find("#accountingAccountMaskId").val(resultData.Id);
            $("#resultModal").find("#parameterId").val(resultData.ParameterId);
            $("#resultModal").find("#position").val(resultData.Position);
            $("#resultModal").find("#mask").val(resultData.Mask);

            $("#resultModal").find("#accountingAccountMaskEditor").show();
        });
    }
    HideMaskEditor() {
        $("#resultModal").find("#accountingAccountMaskForm").formReset();
        $("#resultModal").find("#accountingAccountMaskEditor").hide();
    }
    SaveMask() {
        //comprueba si están ingresados los datos del resultado.
        $("#resultModal").find("#resultForm").validate();
        if ($("#resultModal").find("#resultForm").valid()) {
            //si el resultado está grabado
            if ($("#resultModal").find("#ResultId").val() != "" && $("#resultModal").find("#ResultId").val() != "0") {

                $("#resultModal").find("#accountingAccountMaskForm").validate();
                if ($("#resultModal").find("#accountingAccountMaskForm").valid()) {
                    var accountingAccountMaskModel = new AccountingAccountMaskModel();
                    accountingAccountMaskModel.Id = $("#resultModal").find("#accountingAccountMaskId").val();
                    accountingAccountMaskModel.ParameterId = $("#resultModal").find("#parameterId").val();
                    accountingAccountMaskModel.Position = $("#resultModal").find("#position").val();
                    accountingAccountMaskModel.Mask = $("#resultModal").find("#mask").val();
                    accountingAccountMaskModel.ResultId = $("#resultModal").find("#ResultId").val();

                    //realizo las validaciones
                    ValidateMask(accountingAccountMaskModel);
                }
            } else { //si el resultado no está grabado.
                //método de grabación de resultado.
                $("#resultModal").find("#resultForm").validate();
                if ($("#resultModal").find("#resultForm").valid()) {
                    // ClearRightResultList();
                    // ClearLeftResultList();

                    var resultModel = new ResultModel();
                    resultModel.ResultId = $("#resultModal").find("#ResultId").val();
                    resultModel.AccountingAccountNumber = $("#resultModal").find("#accountingAccount").val();
                    resultModel.AccountingNatureId = $("#resultModal").find("#accountingNature").val();
                    resultModel.ParameterId = $("#resultModal").find("#resultParameter").val();

                    var conditionModel = new ConditionModel();
                    GetLastConditionId(accountingRuleId).then(function (conditionData) {
                        conditionModel.ConditionId = conditionData;
                        conditionModel.AccountingRuleId = accountingRuleId;
                        SaveResult(resultModel, conditionModel, $('#Module').val()).then(function (resultData) {
                            if (resultData.ResultId > 0) {
                                var resultModel = new ResultModel();
                                resultModel.ResultId = resultData.ResultId;
                                resultModel.AccountingNatureDescription = resultData.AccountingNatureDescription;
                                resultModel.AccountingAccountId = resultData.AccountingAccountId;
                                resultModel.AccountingAccountNumber = resultData.AccountingAccountNumber;
                                resultModel.AccountingAccountName = resultData.AccountingAccountName;
                                resultModel.ParameterId = resultData.ParameterId;
                                resultModel.ParameterDescription = resultData.ParameterDescription;
                                resultModel.Value = resultData.Value;
                                $("#resultModal").find("#ResultId").val(resultData.ResultId);

                                $("#resultModal").find("#accountingAccountMaskForm").validate();
                                if ($("#resultModal").find("#accountingAccountMaskForm").valid()) {
                                    var accountingAccountMaskModel = new AccountingAccountMaskModel();
                                    accountingAccountMaskModel.Id = $("#resultModal").find("#accountingAccountMaskId").val();
                                    accountingAccountMaskModel.ParameterId = $("#resultModal").find("#parameterId").val();
                                    accountingAccountMaskModel.Position = $("#resultModal").find("#position").val();
                                    accountingAccountMaskModel.Mask = $("#resultModal").find("#mask").val();
                                    accountingAccountMaskModel.ResultId = $("#resultModal").find("#ResultId").val();

                                    //realizo las validaciones
                                    ValidateMask(accountingAccountMaskModel);
                                }
                            } else {
                                $("#rulesAlert").UifAlert('show', Resources.SaveError, "danger");
                                $("#resultModal").UifModal('hide');
                            }
                        });
                    });
                }
            }
        }
    }
    AssembledAccount() {
        var accountNumber = "";
        accountNumber = $("#resultModal").find("#accountingAccount").val();

        GetLastConditionId(accountingRuleId).then(function (conditionData) {
            UpdateAssembledMaskAccount(accountNumber, conditionData);
        });
    }
    //endregion mask
}

function DeleteRecord(excecutingType) {
    if (excecutingType == 1) {
        $("#accountingRulesAlert").UifAlert('hide');

        lockScreen();
        setTimeout(function () {

            $.ajax({
                type: "POST",
                url: GL_ROOT + "EntryParameter/DeleteAccountingRule",
                data: JSON.stringify({
                    "accountingRuleId": accountingRuleId
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (accountingRuleData) {

                    unlockScreen();
                    if (accountingRuleData > 0) {
                        $("#accountingRulesAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    } else {
                        $("#accountingRulesAlert").UifAlert('show', Resources.ErrorDeletingRecord + " / " + Resources.LabelUnassign + " " + Resources.RulePackage, "danger");
                    }
                    accountingRuleId = 0;
                    $("#accountingRules").UifDataTable("clear");
                    $('#Module').trigger('change');
                }
            });

        }, 500);
    }
    if (excecutingType == 2) {
        $("#rulesAlert").UifAlert('hide');

        var conditionModel = new ConditionModel();
        conditionModel.ConditionId = conditionId;
        conditionModel.AccountingRuleId = accountingRuleId;

        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/DeleteCondition",
            data: JSON.stringify({
                "conditionModel": conditionModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (conditionData) {
                if (conditionData > 0) {
                    $("#rulesAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                } else {
                    $("#rulesAlert").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
                conditionId = 0;
                $("#conditions").UifDataTable("clear");
                $("#results").UifDataTable("clear");
                GetConditionsByAccountingRuleIdAndModuleId(accountingRuleId, $("#Module").val()).then(function (accountingRuleData) {
                    if (accountingRuleData.length > 0) {
                        for (var i = 0; i < accountingRuleData.length; i++) {

                            var conditionModel = new ConditionModel();
                            conditionModel.If = Resources.Yes;
                            conditionModel.ConditionId = accountingRuleData[i].ConditionId;
                            conditionModel.ParameterId = accountingRuleData[i].ParameterId;
                            conditionModel.ParameterDescription = accountingRuleData[i].ParameterDescription;
                            conditionModel.Operator = accountingRuleData[i].OperatorDescription;
                            conditionModel.Value = accountingRuleData[i].Value;

                            $("#conditions").UifDataTable("addRow", conditionModel);
                        }

                        var conditionModel = new ConditionModel();
                        GetLastConditionId(accountingRuleId).then(function (conditionData) {
                            conditionModel.ConditionId = conditionData;

                            GetResultByCondition(conditionModel).then(function (resultData) {
                                if (resultData.ResultId > 0) {
                                    var resultModel = new ResultModel();
                                    resultModel.ResultId = resultData.ResultId;
                                    resultModel.AccountingNatureId = resultData.AccountingNatureId;
                                    resultModel.AccountingNatureDescription = resultData.AccountingNatureDescription;
                                    resultModel.AccountingAccountId = resultData.AccountingAccountId;
                                    resultModel.AccountingAccountNumber = resultData.AccountingAccountNumber;
                                    resultModel.AccountingAccountName = resultData.AccountingAccountName;
                                    resultModel.ParameterId = resultData.ParameterId;
                                    resultModel.ParameterDescription = resultData.ParameterDescription;
                                    resultModel.Value = resultData.Value;

                                    $("#results").UifDataTable("addRow", resultModel);
                                }
                            });

                        });
                    }

                });
            }
        });
    }
    if (excecutingType == 3) {

        var conditionModel = new ConditionModel();

        GetLastConditionId(accountingRuleId).then(function (conditionData) {
            conditionModel.ConditionId = conditionData;
            conditionModel.AccountingRuleId = accountingRuleId;
            conditionModel.ResultId = resultId;

            var deleteResultPromise = new Promise(function (resolve, reject) {
                $.ajax({
                    type: "POST",
                    url: GL_ROOT + "EntryParameter/DeleteResult",
                    data: JSON.stringify({
                        "conditionModel": conditionModel
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (deletedData) {
                        resolve(deletedData);
                    }
                });
            });

            deleteResultPromise.then(function (deletedData) {
                if (deletedData.Id > 0) {
                    $("#rulesAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    $("#results").UifDataTable('clear');
                } else {
                    $("#rulesAlert").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
            });
        });
    }
}

function GetConditionsByAccountingRuleIdAndModuleId(accountingRuleId, moduleId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/LoadConditionsByAccountingRule",
            data: JSON.stringify({
                "accountingRuleId": accountingRuleId,
                "moduleId": moduleId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (accountingRuleData) {
                resolve(accountingRuleData);
            }
        });
    });
}

function SaveAccountingRule(accountingRuleModel) {
    return saveAccountingRulePromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/SaveAccountingRule",
            data: JSON.stringify({
                "accountingRuleModel": accountingRuleModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (accountingRuleData) {
                resolve(accountingRuleData);
            }
        });
    });
}

function ClearAccountingRuleModal() {
    $("#accountingRuleModal").find("#AccountingRuleId").val(0);
    $("#accountingRuleModal").find("#AccountingRuleDescription").val("");
    $("#accountingRuleModal").find("#AccountingRuleObservations").val("");
}

function ClearConditionModal() {
    $("#conditionModal").find("#conditionId").val("");
    $("#conditionModal").find("#conditionParameter").UifSelect({ source: null });
    $("#conditionModal").find("#operation").val("");
    $("#conditionModal").find("#conditionValue").val("");
}

function SaveCondition(conditionModel, moduleId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/SaveCondition",
            data: JSON.stringify({
                "conditionModel": conditionModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (conditionData) {
                resolve(conditionData);
            }
        });
    });
}

function GetCondition(conditionId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetCondition",
            data: JSON.stringify({
                "conditionId": conditionId,
                "accountingRuleId": accountingRuleId,
                "moduleId": $("#Module").val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (conditionData) {
                resolve(conditionData);
            }
        });
    });
}

function CheckModal(modalName) {
    var isShown;
    return modalPromise = new Promise(function (resolve, reject) {
        time = setInterval(function () {
            if ($('#' + modalName).is(":visible")) {
                isShown = true;
                resolve(isShown);
            }
        }, 3);
    });
}

function UpdateAssembledMaskAccount(accountingAccountNumber, conditionId) {
    var conditionModel = new ConditionModel();

    conditionModel.ConditionId = conditionId;
    conditionModel.ResultId = $("#resultModal").find("#ResultId").val();

    var assembledAccountPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetAssembledMaskedAccountingAccount",
            data: JSON.stringify({
                "conditionModel": conditionModel,
                "accountingAccountNumber": accountingAccountNumber
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (conditiontData) {
                resolve(conditiontData);
            }
        });
    });

    assembledAccountPromise.then(function (conditiontData) {
        $("#resultModal").find("#accountingAccount").val(conditiontData);
    });
}

var deleteAccountingAccountMask = function (deferred, data) {

    var accountingAccountMaskModel = new AccountingAccountMaskModel();
    accountingAccountMaskModel.Id = data.Id;
    accountingAccountMaskModel.ResultId = $("#resultModal").find("#ResultId").val();

    var conditionId = 0;

    GetLastConditionId(accountingRuleId).then(function (conditionData) {
        conditionId = conditionData;

        var deleteAccountingAccountMaskPromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: GL_ROOT + "EntryParameter/DeleteAccountingAccountMask",
                data: JSON.stringify({
                    "accountingAccountMaskModel": accountingAccountMaskModel,
                    "conditionId": conditionId
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (resultData) {
                    resolve(resultData);
                }
            });
        });

        deleteAccountingAccountMaskPromise.then(function (resultData) {
            $("#resultModal").find("#accountingAccount").val(resultData.result.AccountingAccount)
            GetLastConditionId(accountingRuleId).then(function (conditionData) {
                UpdateAssembledMaskAccount(resultData.result.AccountingAccount, conditionData);
            });

            deferred.resolve();
        });
    });
};

function SaveResult(resultModel, conditionModel, moduleId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/SaveResult",
            data: JSON.stringify({
                "resultModel": resultModel,
                "conditionModel": conditionModel,
                "moduleId": $('#Module').val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {
                resolve(resultData);
            }
        });
    });
}

function ClearResultModalFields() {
    $("#resultModal").find("#ResultId").val(0);
    $("#resultModal").find("#accountingNature").val("");
    $("#resultModal").find("#accountingAccount").val("");
    $("#resultModal").find("#value").val("");
    $("#resultModal").find("#ResultType").prop('disabled', false);
    $("#resultModal").find("#ResultType").val("");
    $("#resultModal").find("#ResultType").trigger('change');
    $("#setUpAccount").removeClass("glyphicon glyphicon-check");
    $("#setUpAccount").addClass("glyphicon glyphicon-unchecked");
    $("#setUpAccountCheck").prop("checked", false);
    $("#resultModal").find("#accountingAccountMaskRegion").hide();
    $("#resultModal").find("#resultForm").formReset();
    $("#accountingAccountMaskList").UifListView("refresh");
}

function SaveResult(resultModel, conditionModel, moduleId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/SaveResult",
            data: JSON.stringify({
                "resultModel": resultModel,
                "conditionModel": conditionModel,
                "moduleId": $('#Module').val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {
                resolve(resultData);
            }
        });
    });
}

function GetResultByCondition(conditionModel) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetResult",
            data: JSON.stringify({
                "conditionModel": conditionModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {
                resolve(resultData);
            }
        });
    });
}

function GetLastConditionId(accountingRuleId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetLastConditionId",
            data: JSON.stringify({
                "accountingRuleId": accountingRuleId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (conditionData) {
                resolve(conditionData);
            }
        });
    });
}

function GetResult(resultId) {
    $("#accountingAccountMaskList").UifListView("refresh");

    var conditionModel = new ConditionModel();
    GetLastConditionId(accountingRuleId).then(function (conditionData) {
        conditionModel.ConditionId = conditionData;
        conditionModel.ResultId = resultId;

        GetResultByCondition(conditionModel).then(function (resultData) {
            $("#resultModal").find("#ResultId").val(resultData.ResultId);
            $("#resultModal").find("#accountingNature").val(resultData.AccountingNatureId);
            $("#resultModal").find("#accountingAccount").val(resultData.AccountingAccountNumber);
            $('#resultModal').UifModal('showLocal', Resources.EditResult);
            CheckModal('resultModal');
            modalPromise.then(function (isShown) {
                if (isShown) {
                    clearTimeout(time);
                    var controller = GL_ROOT + "EntryParameter/GetResultParameters?moduleId=" + $("#Module").val();
                    $("#resultModal").find("#resultParameter").UifSelect({
                        source: controller
                    });
                    setTimeout(() => {
                        $("#resultModal").find("#resultParameter").val(resultData.ParameterId);
                        $("#resultModal").find("#resultParameter").trigger('change');
                    }, 2000);

                    LoadAccountingAccountMaskList(resultData.ResultId);
                    loadMasksPromise.then(function (resultData) {
                        if (resultData.length > 0) {
                            for (var j = 0; j < resultData.length; j++) {
                                $("#accountingAccountMaskList").UifListView("addItem", resultData[j]);
                            }
                            //actualizo la cuenta contable.
                            GetLastConditionId(accountingRuleId).then(function (conditionData) {
                                UpdateAssembledMaskAccount($("#resultModal").find("#accountingAccount").val(), conditionData);
                            });
                        }
                    });
                    //compruebo que existan registros de mascara para mostrar.
                    GetAccountingAccountMaskList(resultData.ResultId);
                    getAccountingAccountMaskListPromise.then(function (maskList) {
                        if (maskList.length > 0) {
                            $("#setUpAccount").removeClass("glyphicon glyphicon-unchecked");
                            $("#setUpAccount").addClass("glyphicon glyphicon-check");
                            $("#setUpAccountCheck").prop("checked", true);
                            $("#accountingAccountMaskRegion").slideDown("fast");
                        } else {
                            $("#setUpAccount").removeClass("glyphicon glyphicon-check");
                            $("#setUpAccount").addClass("glyphicon glyphicon-unchecked");
                            $("#setUpAccountCheck").prop("checked", false);
                            $("#accountingAccountMaskRegion").slideUp("fast");
                        }
                    });
                }
            });
        });
    });
}

function LoadAccountingAccountMaskList(resultId) {
    $("#resultModal").find("#accountingAccountMaskList").UifListView("refresh");

    var accountingAccountMaskModel = new AccountingAccountMaskModel();
    accountingAccountMaskModel.ResultId = resultId;

    loadMasksPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetAccountingAccountMasks",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel,
                "moduleId": $("#Module").val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {
                resolve(resultData);
            }
        });
    });
}

function GetAccountingAccountMaskList(resultId) {

    var accountingAccountMaskModel = new AccountingAccountMaskModel();
    accountingAccountMaskModel.ResultId = resultId;

    return getAccountingAccountMaskListPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetAccountingAccountMasks",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel,
                "moduleId": $("#Module").val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (maskList) {
                resolve(maskList);
            }
        });
    });
}

function ValidateMaskUniqueParameter(accountingAccountMaskModel) {
    return validateMaskUniqueParameterPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/ValidateMaskUniqueParameter",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (uniqueParameterValidation) {
                resolve(uniqueParameterValidation);
            }
        });
    });
}

function ValidateMaskAccountLength(accountingAccountMaskModel) {
    return validateMaskAccountLengthPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/ValidateMaskAccountLength",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (maskAccountLengthValidation) {
                resolve(maskAccountLengthValidation);
            }
        });
    });
}

function ValidateUniqueMask(accountingAccountMaskModel) {
    return validateUniqueMaskPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/ValidateUniqueMask",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (uniqueMaskValidation) {
                resolve(uniqueMaskValidation);
            }
        });
    });
}

function ValidateMaskPosition(accountingAccountMaskModel) {
    return validateMaskPositionPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/ValidateMaskPosition",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (maskPositionValidation) {
                resolve(maskPositionValidation);
            }
        });
    });
}

function SaveAccountingAccountMask(accountingAccountMaskModel) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/SaveAccountingAccountMask",
            data: JSON.stringify({
                "accountingAccountMaskModel": accountingAccountMaskModel
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {
                resolve(resultData);
            }
        });
    });
}

function ValidateMask(accountingAccountMaskModel) {
    ValidateMaskUniqueParameter(accountingAccountMaskModel);
    validateMaskUniqueParameterPromise.then(function (uniqueParameterValidation) {
        if (uniqueParameterValidation) {
            ValidateMaskAccountLength(accountingAccountMaskModel);
            validateMaskAccountLengthPromise.then(function (maskAccountLengthValidation) {
                if (maskAccountLengthValidation) {
                    ValidateUniqueMask(accountingAccountMaskModel);
                    validateUniqueMaskPromise.then(function (uniqueMaskValidation) {
                        if (uniqueMaskValidation) {
                            ValidateMaskPosition(accountingAccountMaskModel);
                            validateMaskPositionPromise.then(function (maskPositionValidation) {
                                if (maskPositionValidation) {
                                    SaveAccountingAccountMask(accountingAccountMaskModel).then(function (resultData) {
                                        $("#resultModal").find("#maskAlert").UifAlert("show", Resources.AddSuccessfully, "success");
                                        $("#resultModal").find("#accountingAccountMaskEditor").hide();
                                        $("#resultModal").find("#accountingAccountMaskForm").formReset();
                                        LoadAccountingAccountMaskList($("#resultModal").find("#ResultId").val());
                                        loadMasksPromise.then(function (resultData) {
                                            if (resultData.length > 0) {
                                                for (var j = 0; j < resultData.length; j++) {
                                                    $("#accountingAccountMaskList").UifListView("addItem", resultData[j]);
                                                }
                                                //actualizo la cuenta contable.
                                                GetLastConditionId(accountingRuleId).then(function (conditionData) {
                                                    UpdateAssembledMaskAccount($("#resultModal").find("#accountingAccount").val(), conditionData);
                                                });
                                            }
                                        });
                                    });
                                } else {
                                    $("#resultModal").find("#maskAlert").UifAlert("show", Resources.MaskPositionValidationMessage, "danger");
                                }
                            });
                        } else {
                            $("#resultModal").find("#maskAlert").UifAlert("show", Resources.UniqueMaskValidationMessage, "danger");
                        }
                    });
                } else {
                    $("#resultModal").find("#maskAlert").UifAlert("show", Resources.MaskAccountLengthValidationMessage, "danger");
                }
            });
        } else {
            $("#resultModal").find("#maskAlert").UifAlert("show", Resources.UniqueParameterValidationMessage, "danger");
        }
    });

}

//////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios     //
//////////////////////////////////////////////////
function ValidateConditionAddForm() {
    if ($('#conditionParameter').val() == "") {
        $("#conditionModal").find("#alertFormCondition").UifAlert('show', Resources.EnterParameterSearch, "warning");
        return false;
    }
    if ($('#operation').val() == "") {
        $("#conditionModal").find("#alertFormCondition").UifAlert('show', Resources.SelectOperationType, "warning");
        return false;
    }
    if ($("#conditionValue").val() == null || $("#conditionValue").val() == "") {
        $("#conditionModal").find("#alertFormCondition").UifAlert('show', Resources.MessageValue, "warning");
        return false;
    }
    return true;
}
