/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var completeWithCeros = $("#ViewBagCompleteWithCeros").val();
var accoutingAccountLength = $("#ViewBagAccoutingAccountLength").val();
var selectedNode;
var checkCostCenterId = false;
var checkAnalysisId = false;
var idAccountingAccount = 0;
var dropDownSearchAdvInfringement = null;
var ModelAccountingAccount = {
    AccountingAccountId: 0,
    AccountingAccountParentId: 0,
    AccountingAccountNumber: "",
    AccountingAccountName: "",
    BranchId: 0,
    AccountingNatureId: 0,
    CurrencyId: 0,
    RequireAnalysis: 0,
    AnalysisId: 0,
    RequireCostCenter: 0,
    CostCenterId: 0,
    Comments: " ",
    AccountingAccountApplication: 0,
    AccountingAccountType: 0,
    CostCenters: [],
    PrefixId: 0,
    ReevaluePositive: "",
    ReevalueNegative: "",
    AccountClassify: "",
    ApplyReevaluate: 0,
    ApplyReclassification: 0,
};
var accountingAccountReplicationPromise;
var time;
var modalPromise;
var dataTableDat;
var dataAccountDat;
var dataNameDat;
var counterParent = 0;
var textDigit;
var AccountAdvanced;

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

dropDownSearchAdvInfringement = uif2.dropDown({
    source: rootPath + 'GeneralLedger/AccountingAccount/AdvancedConsultationSearch',
    element: '#btnSearchAdvInfringement',
    align: 'right',
    width: 600,
    height: 600,
    loadedCallback: this.componentLoadedCallback
});

$('#accountingAccountReplicationModal').find('#accountReplicationAccept').click(function () {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: "<h1>" + Resources.MessageWaiting + "</h1>"
    });

    $('#accountingAccountReplicationModal').UifModal('hide');
    ReplicateAccount(idAccountingAccount);
    accountingAccountReplicationPromise.then(function (accountData) {
        if (accountData) {
            //Mensaje de éxito
            if (accountData.success) {
                $.unblockUI();
                ClearAccountingAccountModal();
                $("#alertAccountingAccountTransaction").UifAlert('show', Resources.ReplicationSuccessful, "success");
            } else {
                $("#alertAccountingAccountTransaction").UifAlert('show', Resources.MessageProcessFailed, "warning");
                $.unblockUI();

            }
        }
    });
    //ReplicateAccount(idAccountingAccount);

});

$("#inputTemporal").ValidatorKey(ValidatorType.Number, 2, 0);

$("#saveButton").on('click', function () {
    $("#AccountSearchNumber").val("");
    $("#addAccountingAccount").validate();
    if ($("#addAccountingAccount").valid()) {

        ModelAccountingAccount.AccountingAccountId = $("#AccountingAccountId").val();
        if (ModelAccountingAccount.AccountingAccountParentId == 0) {
            $("#AccountingAccountNumber").trigger('blur');
        }

        ModelAccountingAccount.AccountingAccountParentId = parseInt($("#AccountingAccountNumber").val().substring(0, 1));
        ModelAccountingAccount.AccountingAccountNumber = $("#AccountingAccountNumber").val();
        ModelAccountingAccount.AccountingAccountName = $("#AccountingAccountName").val();
        ModelAccountingAccount.BranchId = $("#BranchId").val();
        ModelAccountingAccount.AccountingNatureId = $("#AccountingNatureId").val();
        if ($("#CurrencyId").val() == "") {
            ModelAccountingAccount.CurrencyId = -1;
        }
        else {
            ModelAccountingAccount.CurrencyId = $("#CurrencyId").val();
        }

        ModelAccountingAccount.RequireAnalysis = ($('#checkAnalysisId').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
        ModelAccountingAccount.AnalysisId = $("#AnalysisId").val();
        ModelAccountingAccount.RequireCostCenter = ($('#checkCostCenterId').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
        ModelAccountingAccount.CostCenterId = $("#CostCenterId").val();
        ModelAccountingAccount.Comments = $("#Comments").val();
        ModelAccountingAccount.AccountingAccountType = $("#type  option:selected").val();

        if ($("#EnableIsOfficialNomenclature").is(':checked')) {
            ModelAccountingAccount.AccountingAccountApplication = 3
        }
        else {
            if ($("#EnableBilling").is(':checked') && $("#EnableAccountingTitle").is(':checked')) {
                ModelAccountingAccount.AccountingAccountApplication = 4
            }
            else {
                if ($("#EnableBilling").is(':checked')) {
                    ModelAccountingAccount.AccountingAccountApplication = 1
                }

                if ($("#EnableAccountingTitle").is(':checked')) {
                    ModelAccountingAccount.AccountingAccountApplication = 2
                }
            }
        }

        ModelAccountingAccount.CostCenters = $('#costCenters').val();
        ModelAccountingAccount.PrefixId = $('#PrefixId').val();

        if ($("#inputNumberAccountPositive").val() != ""
            && $("#inputNumberAccountNegative").val() != "") {
            ModelAccountingAccount.ReevaluePositive = $("#inputNumberAccountPositive").val();
            ModelAccountingAccount.ReevalueNegative = $("#inputNumberAccountNegative").val();
        }
        else {
            ModelAccountingAccount.ReevaluePositive = "";
            ModelAccountingAccount.ReevalueNegative = "";
        }


        if ($('#inputNumberAccountReclasify').val() != "") {
            ModelAccountingAccount.AccountClassify = $('#inputNumberAccountReclasify').val();
        }
        else {
            ModelAccountingAccount.AccountClassify = "";
        }

        var correct = [];
        var edit = (ModelAccountingAccount.AccountingAccountId == 0) ? 0 : 1;

        ValidateAccountingAccount(ModelAccountingAccount, edit).then(function (validatedAccount) {
            correct = validatedAccount;

            if (correct.IsSucessful) {
                SaveAccountingAccount(ModelAccountingAccount).then(function (accountingAccount) {
                    if (edit == 0) {
                        if (accountingAccount.AccountingAccountId > 0) {
                            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AddSuccessfully, "success");
                            const newNode = { id: accountingAccount.AccountingAccountId, text: accountingAccount.Number + ' - ' + accountingAccount.Description };
                            $("#treeAccountingAccount").UifTreeView('createNode', selectedNode, newNode);
                            GetAccountingAccountsByParentId(accountingAccount.AccountingAccountParentId).then(function (accountChildrenData) {
                                LoadNodeChildren(accountChildrenData, selectedNode);
                            });
                        }
                    } else {
                        if (accountingAccount.AccountingAccountId > 0) {
                            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.EditSuccessfully, "success");
                            const newName = accountingAccount.Number + ' - ' + accountingAccount.Description;
                            $("#treeAccountingAccount").UifTreeView('renameNode', selectedNode, newName);
                        }
                    }
                });
                ClearAccountingAccountModal();
            } else {
                switch (correct.TypeId) {
                    case 1:
                        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AccountThereAlreadyIs, "warning");
                        break;
                    case 2:
                        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.ParentAccountError, "warning");
                        break;
                    case 3:
                        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AccountingNumberLengthError, "warning");
                        break;
                    case 4:
                        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.ParentAccountError, "warning");
                        break;
                    case 5:
                        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AccountNoParent, "warning");
                        break;
                    case 6:
                        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.LedgerAccountNumber + " " + Resources.TooMuchBig, "warning");
                        break;
                }
            }
        });
    }
});

$("#newButton").on('click', function () {
    $("#alertAccountingAccountTransaction").UifAlert('hide');
    ClearAccountingAccountModal();
});

$("#cancelButton").on('click', function () {
    ClearAccountingAccountModal();
});

$('#checkAnalysisId').on('click', function () {
    if ($('#RequireAnalysis').val() == 1) {
        $('#checkAnalysisId').removeClass("glyphicon glyphicon-check");
        $('#checkAnalysisId').addClass("glyphicon glyphicon-unchecked");
        $('#AnalysisId').val("");
        $('#AnalysisId').prop("disabled", true);
        $('#RequireAnalysis').val(0);
    } else {
        $('#checkAnalysisId').removeClass("glyphicon glyphicon-unchecked");
        $('#checkAnalysisId').addClass("glyphicon glyphicon-check");
        $('#AnalysisId').prop("disabled", false);
        $('#RequireAnalysis').val(1);
    }
});

$('#EnableBilling').on('click', function () {
    if ($("#EnableIsOfficialNomenclature").is(':checked')) {
        $("#EnableIsOfficialNomenclature").click();
    }
});

$('#EnableAccountingTitle').on('click', function () {
    if ($("#EnableIsOfficialNomenclature").is(':checked')) {
        $("#EnableIsOfficialNomenclature").click();
    }
});

$('#EnableIsOfficialNomenclature').on('click', function () {
    if ($("#EnableBilling").is(':checked')) {
        $("#EnableBilling").click();
    }

    if ($("#EnableAccountingTitle").is(':checked')) {
        $("#EnableAccountingTitle").click();
    }
});

$("#ApplyReevaluate").on("click", function () {
    buttonBehavior();
    if ($("#ApplyReevaluate").is(':checked')) {
        $("#divReevaluePositive").removeAttr("style").show();
        $("#divReevalueNegative").removeAttr("style").show();
    }
    else {
        $("#inputNumberAccountPositive").val("");
        $("#inputNumberAccountNegative").val("");
        $("#divReevaluePositive").removeAttr("style").hide();
        $("#divReevalueNegative").removeAttr("style").hide();
    }
});

$("#ApplyReclassification").on("click", function () {
    buttonBehavior();
    if ($("#ApplyReclassification").is(':checked')) {
        $("#divAccountClassify").removeAttr("style").show();
    }
    else {
        $("#inputNumberAccountReclasify").val("");
        $("#divAccountClassify").removeAttr("style").hide();
    }
});

$('#costCenters').on('itemSelected', function (event, selectedItems) {
    ModelAccountingAccount.CostCenters = selectedItems;
});

$('#prefixes').on('itemSelected', function (event, selectedItems) {
    ModelAccountingAccount.Prefixes = selectedItems;
});

$("#DeleteConfirmationModal").find("#deleteConfirmationAccept").on('click', function () {
    $('#DeleteConfirmationModal').modal('hide');
    $.ajax({
        type: "POST",
        url: GL_ROOT + "AccountingAccount/DeleteAccountingAccount",
        data: JSON.stringify({ "accountingAccountId": idAccountingAccount }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data == true) {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.DeleteSuccessfully, "success");
            $("#treeAccountingAccount").UifTreeView('deleteNode', selectedNode);
            ClearAccountingAccountModal();
        } else {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
        }
    });
});

$('#modalAccountingAccount').on('hidden.bs.modal', function () {
    ClearAccountingAccountModal();
});

$('#treeAccountingAccount').on('selected', function (event, data) {
    const selectedNode = $("#treeAccountingAccount").UifTreeView('getSelected');
    GetAccountingAccountsByParentId(data.id).then(function (accountChildrenData) {
        LoadNodeChildren(accountChildrenData, selectedNode);
    });
});

$('#createNode').click(function () {
    selectedNode = $("#treeAccountingAccount").UifTreeView('getSelected');
    if (selectedNode[0]) {
        $("#AccountingAccountParentId").val(selectedNode[0]);
        $('#modalAccountingAccount').UifModal('showLocal', Resources.AddRecord);
    } else {
        $("#alertAccountingAccount").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
    }
});

//Validar si es un Padre
$('#editNode').click(function () {
    selectedNode = $("#treeAccountingAccount").UifTreeView('getSelected');
    if (!selectedNode[0]) {
        $("#alertAccountingAccount").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
    } else if (selectedNode[0] >= 10) {
        $('#modalAccountingAccount').UifModal('showLocal', Resources.EditRecord);
        CheckModal('modalAccountingAccount');
        modalPromise.then(function (isShown) {
            if (isShown) {
                clearTimeout(time);

                var result = [];
                //PARENT ID PARAMETRO
                GetAccountingAccount(selectedNode[0]).then(function (accountingAccountData) {
                    result = accountingAccountData;

                    $("#AccountingAccountId").val(result.AccountingAccountId);
                    $("#AccountingAccountNumber").val(result.AccountingAccountNumber);
                    $("#AccountingAccountName").val(result.AccountingAccountName);
                    $("#BranchId").val(result.BranchId <= 0 ? null : result.BranchId);
                    $("#PrefixId").val(result.PrefixId == 0 ? null : result.PrefixId);
                    $("#AccountingNatureId").val(result.AccountingNatureId);
                    $("#CurrencyId").val(result.CurrencyId < 0 ? null : result.CurrencyId);
                    $("#type").val(result.AccountingAccountType == 0 ? null : result.AccountingAccountType);
                    $("#AnalysisId").val(result.AnalysisId);
                    $("#Comments").val(result.Comments);
                    $('#RequireCostCenter').val(result.RequireCostCenter);
                    $('#RequireAnalysis').val(result.RequireAnalysis);
                    $("#AccountingAccountParentId").val(result.AccountingAccountParentId);

                    if (!(result.AccountingAccountApplication >= 0)) {
                        result.AccountingAccountApplication = 0;
                    }

                    switch (result.AccountingAccountApplication) {
                        case 1:
                            $("#EnableBilling").click();
                            break;
                        case 2:
                            $("#EnableAccountingTitle").click();
                            break;
                        case 3:
                            $("#EnableIsOfficialNomenclature").click();
                            break;
                        case 4:
                            $("#EnableBilling").click();
                            $("#EnableAccountingTitle").click();
                            break;
                        case 0:
                            break;
                    }

                    checkCostCenterId = ($('#RequireCostCenter').val() == 1) ? true : false;
                    checkAnalysisId = ($('#RequireAnalysis').val() == 1) ? true : false;
                    SetChecks(checkCostCenterId, checkAnalysisId);
                    $('#costCenters').UifMultiSelect('setSelected', result.CostCenters);
                });
            }
        });
    } else {
        $("#AccountingAccountParentId").val(selectedNode[0]);
        $("#alertAccountingAccount").UifAlert('show', Resources.AccountParentRestrictionWarning, 'danger');
    }
});

$('#deleteNode').click(function () {
    GetListAccountAccounting($("#AccountingAccountNumber").val()).then(function (data) {
        if (data.length > 1) {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
        } else if (HasChildren(data[0].AccountingAccountId)) {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.ParentAccountNotDelete, 'danger');
        } else if (OnEntry(data[0].AccountingAccountId)) {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.SeatsAccountNotDelete, 'danger');
        } else if (OnConcept(data[0].AccountingAccountId)) {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.OnConceptAccountNotDelete, 'danger');
        }
        else {
            $('#DeleteConfirmationModal').UifModal('showLocal', Resources.DeleteRecord);
            idAccountingAccount = data[0].AccountingAccountId;

        }
    });
});

$('#replicateNode').click(function () {
    GetListAccountAccounting($("#AccountingAccountNumber").val()).then(function (data) {
        if (data.length > 1) {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
        }
        else if (data[0].AccountingAccountId > 10) {
            idAccountingAccount = data[0].AccountingAccountId;
            $('#accountingAccountReplicationModal').appendTo("body").UifModal('showLocal', Resources.Replicate);
        }
        else {
            $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AccountParentReplicationRestrictionWarning, 'danger');
        }
    });
});

$("#AccountingIdTxt").click(function () {
    dropDownSearchAdvInfringement.hide();
});

$("#AccountingIdTxt").keypress(function (func) {
    if (func.which == 13) {
        $("#AccountingIdTxt").trigger("buttonClick");
    }
});

$("#AccountingIdTxt").on('buttonClick', function (event) {
    $("#AccountSearchNumber").val("");
    $("#alertAccountingAccountTransaction").UifAlert('hide');
    if ($("#AccountingIdTxt").val() == "") {
        $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AccountingTypeAccount, "warning");
    }
    else {
        ClearAccountingAccountModal();
        GetListAccountAccounting($("#AccountingIdTxt").val()).then(function (data) {
            if (data.length == 1) {
                LoadDataModalAccountingAccount(data);
            }
            else {
                $("#formsearch").find("#ListViewAccount").UifListView("clear");
                for (var i = 0; i < data.length; i++) {
                    $("#formsearch").find("#ListViewAccount").UifListView("addItem", data[i]);
                }
                $("#formsearch").find("#AccountSearchNumber").val($("#AccountingIdTxt").val());
                $("#AccountingIdTxt").val("");
                dropDownSearchAdvInfringement.show();
                $("#formsearch").find("#AccountSearchNumber").focus();
            }
        });
    }
});

$("#AccountingAccountNumber").blur(function () {
    counterParent = 0;
    $("#alertAccountingAccountTransaction").UifAlert('hide');
    accountingAccountComplete();
    if ($("#AccountingAccountNumber").val() != "") {
        LoadTree().then(function (data) {
            textDigit = parseInt($("#AccountingAccountNumber").val().substring(0, 1));
            for (var i = 0; i < data.length; i++) {
                if (parseInt(data[i].id.indexOf(textDigit)) >= 0) {
                    counterParent = 1;
                }
            }
            if (counterParent == 0) {
                $("#alertAccountingAccountTransaction").UifAlert('hide');
                $("#alertAccountingAccountTransaction").UifAlert('show', Resources.AccountNoParentCount + " " + textDigit + " " + AccountIncludeParent, "warning");
                $("#AccountingAccountNumber").val("");
            }
            else {
                if (ModelAccountingAccount.AccountingAccountParentId == 0) {
                    ModelAccountingAccount.AccountingAccountParentId = textDigit;
                }
            }
        })
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function accountingAccountComplete() {
    $("#alertAccountingAccountTransaction").find('.close').click();
    var accountNumber = $("#AccountingAccountNumber").val();

    if (completeWithCeros == "1") {
        accountNumber = PaddingRight(accountNumber, '0', accoutingAccountLength);
    }

    $("#AccountingAccountNumber").val(accountNumber);
}

//funcion para comprobar que la cuenta contable no tenga cuentas hijas.
function HasChildren(accountingAccountId) {
    var result = false;
    $.ajax({
        async: false,
        type: "POST",
        url: 'HasChildren',
        data: JSON.stringify({ accountingAccountId: accountingAccountId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data == true) {
            result = true;
        }
    });
    return result;
}

//función para comprobar que la cuenta no está siendo usada en asientos de diario o de mayor.
function OnEntry(accountingAccountId) {
    var result = false;
    $.ajax({
        async: false,
        type: "POST",
        url: 'OnEntry',
        data: JSON.stringify({ accountingAccountId: accountingAccountId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data == true) {
            result = true;
        }
    });
    return result;
}

//funcion para comprobar que la cuenta no esté en un concepto contable
function OnConcept(accountingAccountId) {
    var result = false;
    $.ajax({
        async: false,
        type: "POST",
        url: 'OnConcept',
        data: JSON.stringify({ accountingAccountId: accountingAccountId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data == true) {
            result = true;
        }
    });
    return result;
}

function SetChecks(checkCostCenterId, checkAnalysisId) {
    if (checkCostCenterId == true) {
        if (!$('#checkCostCenterId').hasClass('glyphicon glyphicon-check')) {
            $('#checkCostCenterId').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
        }
        $('#CostCenterId').attr("disabled", false);
    } else {
        if ($('#checkCostCenterId').hasClass('glyphicon glyphicon-check')) {
            $('#checkCostCenterId').removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
        }
        $('#CostCenterId').attr("disabled", "disabled");
    }

    if (checkAnalysisId == true) {
        if (!$('#checkAnalysisId').hasClass('glyphicon glyphicon-check')) {
            $('#checkAnalysisId').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
        }
        $('#AnalysisId').attr("disabled", false);
    } else {
        if ($('#checkAnalysisId').hasClass('glyphicon glyphicon-check')) {
            $('#checkAnalysisId').removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
        }
        $('#AnalysisId').attr("disabled", "disabled");
    }
}

//llena la cadena con un caracter especificado a la derecha
function PaddingRight(string, character, stringLength) {
    if (!string || !character || string.length >= stringLength) {
        return string;
    }
    var max = (stringLength - string.length) / character.length;
    for (var i = 0; i < max; i++) {
        string += character;
    }
    return string;
}

function ReplicateAccount(accountingAccountId) {
    return accountingAccountReplicationPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountingAccount/ReplicateAccount",
            data: { "accountingAccountId": accountingAccountId }
        }).done(function (accountData) {
            resolve(accountData);
        });
    });
}

function GetAccountingAccountsByParentId(accountParentId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountingAccount/GetAccountingAccountsByParentId",
            data: { "parentId": accountParentId }
        }).done(function (accountChildrenData) {
            resolve(accountChildrenData);
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
        }, 1);
    });
}

function GetAccountingAccount(accountingAccountId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: 'GetAccountingAccount',
            data: JSON.stringify({ "accountingAccountId": accountingAccountId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (accountingAccountData) {
            resolve(accountingAccountData);
        });
    });
}

function GetListAccountParentList(accountnumber) {
    return $.ajax({
        type: "POST",
        url: "GetBlockadeSpreadingAccountingAccounts",
        data: JSON.stringify({ "query": accountnumber }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function ValidateAccountingAccount(accountingAccountModel, isEdit) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: 'ValidateAccountingAccount',
            data: JSON.stringify({ "accountingAccountModel": accountingAccountModel, "edit": isEdit }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (validatedAccount) {
            resolve(validatedAccount);
        });
    });
}

function SaveAccountingAccount(accountingAccountModel) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: 'SaveAccountingAccount',
            data: JSON.stringify({ "accountingAccountModel": accountingAccountModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (accountingAccount) {
                resolve(accountingAccount);
            }
        });
    });
}

function GetListAccountAccounting(accountnumber) {
    return $.ajax({
        type: "GET",
        url: ACC_ROOT + "Parameters/GetAccountintAccountByNumber?query=" + accountnumber,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function LoadTree() {
    return $.ajax({
        type: "POST",
        url: "LoadTree",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function ClearAccountingAccountModal() {
    $("#AccountingAccountId").val(0);
    $("#AccountingAccountParentId").val("");
    $("#AccountingAccountNumber").val("");
    $("#AccountingAccountName").val("");
    $("#BranchId").val(null);
    $("#PrefixId").val(null);
    $("#AccountingNatureId").val(null);
    $("#CurrencyId").val(null);
    $("#type").val(null);
    $("#AnalysisId").val(null);
    $('#RequireAnalysis').val(1);
    $('#checkAnalysisId').trigger('click');
    $("#Comments").val("");
    $('#RequireCostCenter').val(0);
    $('#costCenters').UifMultiSelect('deSelectAll');
    if ($("#EnableBilling").is(':checked')) {
        $("#EnableBilling").click();
    }
    if ($("#EnableAccountingTitle").is(':checked')) {
        $("#EnableAccountingTitle").click();
    }
    if ($("#EnableIsOfficialNomenclature").is(':checked')) {
        $("#EnableIsOfficialNomenclature").click();
    }


    if ($("#ApplyReevaluate").is(':checked')) {
        $('#inputNumberAccountPositive').val("");
        $('#inputNumberAccountNegative').val("");
        $("#ApplyReevaluate").click();
    }

    if ($("#ApplyReclassification").is(':checked')) {
        $("#ApplyReclassification").click();
        $('inputNumberAccountReclasify').val("");
    }

    idAccountingAccount = 0;
}

//Carga los hijos del nodo seleccionado
function LoadNodeChildren(accountChildrenData, selectedNode) {
    if (accountChildrenData.length > 0) {
        //se eliminan los elementos hijos
        accountChildrenData.forEach(element => {
            $("#treeAccountingAccount").UifTreeView('deleteNode', element.Id);
        });
        //se vuelven a añadir los elementos hijos
        accountChildrenData.forEach(element => {
            const newNode = { id: element.Id, text: element.Text };
            $("#treeAccountingAccount").UifTreeView('createNode', selectedNode, newNode);
        });
    }
}

function LoadDataModalAccountingAccount(data) {
    var result = [];
    GetAccountingAccount(data[0].AccountingAccountId).then(function (accountingAccountData) {
        result = accountingAccountData;

        $("#AccountingAccountId").val(result.AccountingAccountId);
        $("#AccountingAccountNumber").val(result.AccountingAccountNumber);
        $("#AccountingAccountName").val(result.AccountingAccountName);
        $("#BranchId").val(result.BranchId <= 0 ? null : result.BranchId);
        $("#PrefixId").val(result.PrefixId == 0 ? null : result.PrefixId);
        $("#AccountingNatureId").val(result.AccountingNatureId);
        $("#CurrencyId").val(result.CurrencyId < 0 ? null : result.CurrencyId);
        $("#type").val(result.AccountingAccountType == 0 ? null : result.AccountingAccountType);
        $("#AnalysisId").val(result.AnalysisId);
        $("#Comments").val(result.Comments);
        $('#RequireCostCenter').val(result.RequireCostCenter);
        $('#RequireAnalysis').val(result.RequireAnalysis);
        $("#AccountingAccountParentId").val(result.AccountingAccountParentId);

        if (!(result.AccountingAccountApplication >= 0)) {
            result.AccountingAccountApplication = 0;
        }

        switch (result.AccountingAccountApplication) {
            case 1:
                $("#EnableBilling").click();
                break;
            case 2:
                $("#EnableAccountingTitle").click();
                break;
            case 3:
                $("#EnableIsOfficialNomenclature").click();
                break;
            case 4:
                $("#EnableBilling").click();
                $("#EnableAccountingTitle").click();
                break;
            case 0:
                break;
        }

        checkCostCenterId = ($('#RequireCostCenter').val() == 1) ? true : false;
        checkAnalysisId = ($('#RequireAnalysis').val() == 1) ? true : false;

        SetChecks(checkCostCenterId, checkAnalysisId);

        $('#costCenters').UifMultiSelect('setSelected', result.CostCenters);

        if ((result.ReevaluePositive != "" && result.ReevaluePositive != null) && (result.ReevalueNegative != "" && result.ReevalueNegative != null)) {
            $("#ApplyReevaluate").click();
            $('#inputNumberAccountPositive').val(result.ReevaluePositive);
            $('#inputNumberAccountNegative').val(result.ReevalueNegative);
        }

        if ((result.AccountClassify != "" && result.AccountClassify != null)) {
            $("#ApplyReclassification").click();
            $('#inputNumberAccountReclasify').val(result.AccountClassify);
        }

    });

    $("#AccountingIdTxt").val("");
}

function buttonBehavior() {
    if (!$("#ApplyReevaluate").is(':checked') && $("#ApplyReclassification").is(':checked')) {
        $("#longxy").attr('class', "uif-col-6");
    }
    else {
        $("#longxy").removeAttr("class");
    }
}

function componentLoadedCallback() {
    $("#formsearch").find("#ListViewAccount").UifListView({
        displayTemplate: "#ListViewAccount-template",
        selectionType: 'single',
        source: null,
        height: 340
    });

    $("#btnSearchAdvInfringement").on("click", function () {
        dropDownSearchAdvInfringement.show();
        $("#AccountingIdTxt").val("");
        $("#formsearch").find("#AccountSearchNumber").val("");
        $("#formsearch").find("#AccountSearchName").val("");
        $("#alertAccountingAccountTransaction").UifAlert('hide');
        $("#formsearch").find("#ListViewAccount").UifListView("clear");
        $("#formsearch").find("#AccountSearchNumber").focus();
    });

    $("#formsearch").find("#AccountSearch").on('click', function () {
        AdvancedSearchEvent();
    });

    $("#formsearch").on('click', '#saveButton', function () {
        AdvancedSaveEvent();
    });

    $("#formsearch").on('click', '#cancelButton', function () {
        AdvancedCancelEvent();
    })


}

function AdvancedSearchEvent() {
    $("#formsearch").find("#ListViewAccount").UifListView("clear");
    if ($("#formsearch").find("#AccountSearchNumber").val() == "" && $("#formsearch").find("#AccountSearchName").val() == "") {
        $("#formsearch").find("#alertSearchAccountName").UifAlert('show', Resources.AccountingEnterFilterSearch, "success");
    }
    else {
        $("#formsearch").find("#alertSearchAccountName").UifAlert('hide');
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });
        GetListAccountAccounting($("#formsearch").find("#AccountSearchNumber").val()).then(function (data) {
            dataTableDat = data;
            dataAccountDat = $("#formsearch").find("#AccountSearchNumber").val();
            dataNameDat = $("#formsearch").find("#AccountSearchName").val();
            if (dataAccountDat != "" && dataNameDat != "") {
                for (var i = 0; i < dataTableDat.length; i++) {
                    if ((dataTableDat[i].AccountingName).indexOf(dataNameDat) >= 0 && (dataTableDat[i].AccountingNumber).indexOf(dataAccountDat) >= 0) {
                        $("#formsearch").find("#ListViewAccount").UifListView("addItem", dataTableDat[i]);
                    }
                }
            } else if (dataAccountDat != "") {
                for (var i = 0; i < dataTableDat.length; i++) {
                    if ((dataTableDat[i].AccountingNumber).indexOf(dataAccountDat) >= 0) {
                        $("#formsearch").find("#ListViewAccount").UifListView("addItem", dataTableDat[i]);
                    }
                }
            } else if (dataNameDat != "") {
                for (var i = 0; i < dataTableDat.length; i++) {
                    if ((dataTableDat[i].AccountingName).lastIndexOf(dataNameDat, 0) >= 0) {
                        $("#formsearch").find("#ListViewAccount").UifListView("addItem", dataTableDat[i]);
                    }
                }
            }
            else {
                for (var i = 0; i < dataTableDat.length; i++) {
                    $("#formsearch").find("#ListViewAccount").UifListView("addItem", dataTableDat[i]);
                }
            }

            dataTableDat = "";
            dataNameDat = "";
            dataAccountDat = "";
            $.unblockUI();
        });
    }
}

function AdvancedSaveEvent() {
    const data = $("#formsearch").find("#ListViewAccount").UifListView("getSelected")
    if (data.length > 0) {
        ClearAccountingAccountModal();
        LoadDataModalAccountingAccount(data);
        $("#formsearch").find("#AccountSearchNumber").val("");
        $("#formsearch").find("#AccountSearchName").val("");
        $("#formsearch").find("#alertSearchAccountName").UifAlert('hide');
        $("#alertAccountingAccountTransaction").UifAlert('hide');
        $("#formsearch").find("#ListViewAccount").UifListView("clear");
        dropDownSearchAdvInfringement.hide();
    }
    else {
        $("#formsearch").find("#alertSearchAccountName").UifAlert('show', "Por favor seleccione una cuenta", "success");
    }
}

function AdvancedCancelEvent() {
    $("#formsearch").find("#AccountSearchNumber").val("");
    $("#formsearch").find("#AccountSearchName").val("");
    $("#AccountingIdTxt").val("");
    $("#formsearch").find("#alertSearchAccountName").UifAlert('hide');
    $("#formsearch").find("#ListViewAccount").UifListView("clear");
    dropDownSearchAdvInfringement.hide();
}