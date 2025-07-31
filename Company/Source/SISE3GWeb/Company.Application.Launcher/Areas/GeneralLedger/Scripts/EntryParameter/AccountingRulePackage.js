var savePromise;
var time;
var modalPromise;
var isShown;

$(() => {
    new AccountingRulePackage();
});

class AccountingRulePackageModel {
    constructor() {
        this.AccountingRulePackageId = 0;
        this.ModuleId = 0;
        this.AccountingRulePackageDescription = "";
        this.AccountingRules = [];
    }
};

class AccountingRuleModel {
    constructor() {
        this.ModuleId = 0;
        this.AccountingRuleId = 0;
        this.AccountingRuleDescription = "";
        this.AccountingRuleObservations = "";
    }
};

class AccountingRulePackage extends Uif2.Page {
    getInitialState() {
        $('.cancel-button').click(function () {
            $("#alertConcept").UifAlert('hide');
        });
    }
    bindEvents() {
        $('#Module').on('itemSelected', this.LoadList);
        $("#accountingRulePackageView").on('rowAdd', this.ShowAccountingRulePackageModal);
        $('#accountingRulePackageView').on('rowEdit', this.EditAccountingRulePackage);
        $("#accountingRulePackageModal").find("#addAccountingRulePackage").on("click", this.SaveAccountingRulePackage);
        $("#accountingRulePackageModal").find("#cancelAccountingRulePackage").on("click", this.CancelAccountingRulePackage);
    }
    LoadList() {
        $("#accountingRulePackageAlert").UifAlert('hide');
        if ($('#Module').val() > 0) {
            $("#accountingRulePackageView").find('.cancel-button').click();
            $("#accountingRulePackageView").UifListView(
                {
                    source: GL_ROOT + "EntryParameter/GetAccountingRulePackageListByModuleId?moduleId=" + $('#Module').val(),
                    customDelete: false,
                    customAdd: true,
                    customEdit: true,
                    add: true,
                    edit: true,
                    delete: true,
                    displayTemplate: "#accountingRulePackageTemplate",
                    deleteCallback: deleteAccountingRulePackage
                });
        } else {
            $("#accountingRulePackageView").find('.cancel-button').click();
            $("#accountingRulePackageView").UifListView({ source: null });
            $('.add-button').hide();
        }
    }
    ShowAccountingRulePackageModal() {
        $('#accountingRulePackageModal').UifModal('showLocal', Resources.AddRulePackage);
        CheckModal('accountingRulePackageModal');
        modalPromise.then(function (isShown) {
            if (isShown) {
                clearTimeout(time);
                $('#accountingRulePackageModal').find("#AccountingRulePackageDescription").val("");
                var controller = GL_ROOT + "EntryParameter/LoadAccountingRuleModelListByModuleId?moduleId=" + $('#Module').val();
                $('#accountingRulePackageModal').find("#accountingRules").UifMultiSelect({ source: controller });

                $('#accountingRulePackageModal').find("#accountingRules").on('binded', function () {
                    $('#accountingRulePackageModal').find("#accountingRules").UifMultiSelect('setSelected', [0]);
                });
            }
        });
    }
    SaveAccountingRulePackage() {
        var accountingRulePackageModel = new AccountingRulePackageModel();
        accountingRulePackageModel.AccountingRulePackageId = $("#accountingRulePackageModal").find("#AccountingRulePackageId").val();
        accountingRulePackageModel.ModuleId = $('#Module').val();
        accountingRulePackageModel.AccountingRulePackageDescription = $("#accountingRulePackageModal").find("#AccountingRulePackageDescription").val();
        var rulePackageId = $("#accountingRulePackageModal").find("#rulePackageId").val();
        if (rulePackageId != "") {
            accountingRulePackageModel.RulePackageId = parseInt($("#accountingRulePackageModal").find("#rulePackageId").val());
        }

        if ($('#accountingRulePackageModal').find("#accountingRules").val() != null) {
            if ($('#accountingRulePackageModal').find("#accountingRules").val().length > 0) {
                for (var i = 0; i < $('#accountingRulePackageModal').find("#accountingRules").val().length; i++) {
                    var accountingRuleModel = new AccountingRuleModel();
                    accountingRuleModel.AccountingRuleId = $('#accountingRulePackageModal').find("#accountingRules").val()[i];
                    accountingRulePackageModel.AccountingRules.push(accountingRuleModel);
                }
            }
        }

        if (accountingRulePackageModel.AccountingRulePackageId > 0) {
            saveAccountingRulePackageRequest(accountingRulePackageModel);
            savePromise.then(function (savedData) {
                if (savedData.Id > 0)
                    $("#accountingRulePackageAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                else
                    $("#accountingRulePackageAlert").UifAlert('show', Resources.SaveError, "danger");

                $('#accountingRulePackageModal').UifModal('hide');
                clearModalFields();
                $("#accountingRulePackageView").UifListView("refresh");
            });
        }
        else {
            if (validateAccountingRulePackageExists(accountingRulePackageModel.AccountingRulePackageDescription)) {
                $("#accountingRulePackageAlert").UifAlert('show', Resources.AccountingRulePackageAlreadyExists, "warning");
                $('#accountingRulePackageModal').UifModal('hide');
            } else {
                saveAccountingRulePackageRequest(accountingRulePackageModel);
                savePromise.then(function (savedData) {
                    if (savedData.Id > 0)
                        $("#accountingRulePackageAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    else
                        $("#accountingRulePackageAlert").UifAlert('show', Resources.SaveError, "danger");

                    $('#accountingRulePackageModal').UifModal('hide');
                    clearModalFields();
                    $("#accountingRulePackageView").UifListView("refresh");
                });
            }
        }
    }
    CancelAccountingRulePackage() {
        clearModalFields();
    }
    EditAccountingRulePackage(event, data, position) {

        lockScreen();

        setTimeout(function () {

            $.ajax({
                type: "POST",
                url: GL_ROOT + "EntryParameter/GetAccountingRulePackage",
                data: JSON.stringify({ "moduleId": $('#Module').val(), "accountingRulePackageId": data.AccountingRulePackageId }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    unlockScreen();
                    if (data.AccountingRulePackageId > 0) {

                        $("#accountingRulePackageModal").find("#AccountingRulePackageId").val(data.AccountingRulePackageId);
                        $("#accountingRulePackageModal").find("#AccountingRulePackageDescription").val(data.AccountingRulePackageDescription);
                        if (data.RulePackageId > 0) {
                            $("#accountingRulePackageModal").find("#rulePackageId").val(data.RulePackageId);
                        } else {
                            $("#accountingRulePackageModal").find("#rulePackageId").val("");
                        }

                        var controller = GL_ROOT + "EntryParameter/LoadAccountingRuleModelListByModuleId?moduleId=" + $('#Module').val();
                        $('#accountingRulePackageModal').find("#accountingRules").UifMultiSelect({ source: controller });

                        setTimeout(function () {

                            $('#accountingRulePackageModal').UifModal('showLocal', Resources.EditRulePackage);
                        }, 1500);


                        setTimeout(function () {
                            CheckModal('accountingRulePackageModal');
                        }, 1800);

                        setTimeout(function () {

                            modalPromise.then(function (isShown) {
                                if (isShown) {
                                    clearTimeout(time);
                                    var selectedConcepts = [];
                                    if (data.AccountingRules.length > 0) {
                                        for (var i = 0; i < data.AccountingRules.length; i++) {
                                            selectedConcepts.push(data.AccountingRules[i].AccountingRuleId);
                                        }
                                        $('#accountingRulePackageModal').find("#accountingRules").UifMultiSelect('setSelected', selectedConcepts);
                                    }
                                }
                            });
                        }, 2000);
                    } else
                        $("#accountingRulePackageAlert").UifAlert('show', Resources.ErrorGettingRecord, "warning");
                }
            });
        }, 500);
    }
}

var deleteAccountingRulePackage = function (deferred, data) {
    $.ajax({
        type: "POST",
        url: GL_ROOT + "EntryParameter/DeleteAccountingRulePackage",
        data: JSON.stringify({ "accountingRulePackageId": data.AccountingRulePackageId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data > 0)
                $("#accountingRulePackageAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
            else
                $("#accountingRulePackageAlert").UifAlert('show', Resources.ErrorDeletingRecord, "danger");

            deferred.resolve();
            $("#accountingRulePackageView").UifListView("refresh");
        }
    });
};

function validateAccountingRulePackageExists(description) {
    var exists = false;
    var fields = $("#accountingRulePackageView").UifListView("getData");
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if (description.toUpperCase() == fields[j].AccountingRulePackageDescription.toUpperCase()) {
                exists = true;
                break;
            }
        }
    }
    return exists;
}

function saveAccountingRulePackageRequest(accountingRulePackageModel) {
    return savePromise = new Promise(function (resolve, reject) {

        lockScreen();
        setTimeout(function () {

            $.ajax({
                type: "POST",
                url: GL_ROOT + "EntryParameter/SaveAccountingRulePackage",
                data: JSON.stringify({
                    "accountingRulePackageModel": accountingRulePackageModel
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (savedData) {
                    unlockScreen();
                    resolve(savedData);
                }
            });
        }, 500);
    });
}

function clearModalFields() {
    $("#accountingRulePackageModal").find("#AccountingRulePackageId").val("");
    $("#accountingRulePackageModal").find("#AccountingRulePackageDescription").val("");
}

function CheckModal(modalName) {

    return modalPromise = new Promise(function (resolve, reject) {
        time = setInterval(function () {
            if ($('#' + modalName).is(":visible")) {
                isShown = true;
                resolve(isShown);
            }
        }, 3);
    });
}
