var contractId = 0;
var contractLevelId = 0;
var contractLevelCompanyId = 0;
var titleContract = "";
var smallTitleContract = "";
var levelContract = 0;
var totalPayment = 0;
var minimumDate = "";
var maximumDate = "";
var contractFuncionalityProportional = false;
var levelPayId = 0;
var levelRestId = 0;
var paymentAmount = 0;
var rowselectLevelPayment = 0;
var rowselectLevelRestore = 0;
var existAllocation = false;
var validAllowPromise;

var oLevelRestore = {
    Id: 0,
    Level: {},
    Number: 0,
    RestorePercentage: 0,
    NoticePercentage: 0
};

var oLevelPayment = {
    Id: 0,
    Level: {},
    Number: 0,
    Amount: {},
    Date: null
};

var oContract = {
    ContractId: 0,
    SmallDescription: "",
    Description: "",
    Year: 0,
    DateFrom: null,
    DateTo: null,
    Currency: {},
    ReleaseTimeReserve: 0,
    PremiumAmount: 0,
    Enabled: 1,
    GroupContract: "",
    CoInsurancePercentage: 0,
    RisksNumber: 0,
    EstimatedDate: null,
    ContractType: {},
    EpiType: {},
    AffectationType: {},
    ResettlementType: {},
    DepositPercentageAmount: 0,
    DepositPremiumAmount: 0
};

$(document).ready(function () {
    $("#btnCopyContract").hide();
    $("#EstimatedDate").mask("99/99/9999");
    $("#DateTo").mask("99/99/9999");
    $("#DateFrom").mask("99/99/9999");
    $("#paymentDate").mask("99/99/9999");

    $.validator.addMethod("greaterThanDateTo",
        function (value, element, params) {
            if (!((IsDate(value)) && (IsDate($(params).val())))) {
                return false;
            }
            return CompareDates($(params).val(), value) == 0 ? true : false;

        });

    $.validator.addMethod("lessThanDateTo",
        function (value, element, params) {
            if (!((IsDate(value)) && (IsDate($(params).val())))) {
                return false;
            }
            return CompareDates($(params).val(), value) == 1 ? true : false;

        });

    $.validator.addMethod("greaterThan",
        function (value, element, params) {
            if (parseFloat(NotFormatMoney(value)) < parseFloat(NotFormatMoney($(params).val()))) {
                return true;
            } else {
                return false;
            }
        });

    $.validator.addMethod("lessThan",
        function (value, element, params) {
            if (parseFloat(NotFormatMoney(value)) > parseFloat(NotFormatMoney($(params).val()))) {
                return true;
            } else {
                return false;
            }
        });
});

$('#tableContract').on('rowAdd', function (event, data) {
    existAllocation = false;
    if ($('#selectContractYear').val() == "") {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractYear, 'autoclose': true });
        return;
    }
    if ($('#selectContractType').val() == "") {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractType, 'autoclose': true });
        return;
    }

    //validar que el año mínimo sea del año actual uno menos
    $.ajax({
        url: REINS_ROOT + "Parameter/ValidateMinYearContract",
        data: { "year": $("#selectContractYear").val() },
        success: function (dat) {
            if (dat > 0) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateMinYearContract, 'autoclose': true });
            }
            else {
                $("#alertMainContract").UifAlert('hide');
                $('#modalContract').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddContract?contractId=0&contractYear=' +
                    $('#selectContractYear').val() + '&contractTypeId=' + $('#selectContractType').val(), Resources.ContractNewTitle);
            }
        }
    });
});

$('#tableContract').on('rowEdit', function (event, selectedRow) {
    $("#alertMainContract").UifAlert('hide');
    lockScreen();
    ValidateAllocation(selectedRow.ContractId);
    validAllowPromise.then(function (exist) {
        $('#modalContract').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddContract?contractId=' +
            selectedRow.ContractId + '&contractYear=' + $('#selectContractYear').val() + '&contractTypeId=' +
            $('#selectContractType').val(), Resources.ContractEditTitle + ' ' + selectedRow.ContractId);

        setTimeout(function () {
            if (exist == true) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
            }
            unlockScreen();
        }, 1000);

    });
});

$('#tableContract').on('rowDelete', function (event, data) {
    $("#alertMainContract").UifAlert('hide');
    ValidateAllocation(data.ContractId);
    validAllowPromise.then(function (exist) {
        if (exist == false) {
            $('#modalDeleteContract').appendTo("body").UifModal('showLocal');
            contractId = data.ContractId;
        }
    });
});

$('#tableContract').on('rowSelected', function (event, selectedRow) {
    $("#alertMainContract").UifAlert('hide');
    contractId = selectedRow.ContractId;
    titleContract = selectedRow.Description;
    smallTitleContract = selectedRow.SmallDescription;
    ValidateAllocation(contractId);
    $('#panelContractLevel').text(selectedRow.Description);
    var indexColumnsToHide = getContractLevelColumnsToHideByContractTypeId($('#selectContractType').val());

    if (selectedRow.ContractType.ContractFunctionality.ContractFunctionalityId == 3) {
        $("#rightBarLinks").show();
    }
    else {
        $("#rightBarLinks").hide();
    }

    GetContractLevelByContractId(contractId).done(function (data) {
        if (data.success) {
            $("#tableContractLevel").UifDataTable({ sourceData: data.result, hiddenColumns: indexColumnsToHide });
            if (data.result.length == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.ContractLevelIsNecesary, 'autoclose': true });
            }
        }
    });

    if (contractId > 0) {
        $("#btnCopyContract").show();
    }

});

$('#tableContract').on('rowDeselected', function (event, data, position) {
    $("#btnCopyContract").hide();
    $('#panelContractLevel').text("");
});

$("#btnDelete").click(function () {
    $('#modalDeleteContract').modal('hide');
    DeleteContract(contractId).done(function (data) {
        if (data.success) {
            GetContractsByYearAndContractTypeId($('#selectContractYear').val(), $('#selectContractType').val()).done(function (response) {
                if (response.success) {
                    $("#tableContract").UifDataTable({ sourceData: response.result });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageDeleteContract, 'autoclose': true });
        }
    });
});

$("#btnSearchContract").click(function () {
    existAllocation = false;
    $('#formContractSearch').validate();
    if ($('#formContractSearch').valid()) {
        if ($('#selectContractYear').val() == "") {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractYear, 'autoclose': true });
            return;
        }
        if ($('#selectContractType').val() == "") {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractType, 'autoclose': true });
            return;
        }

        GetContractsByYearAndContractTypeId($('#selectContractYear').val(), $('#selectContractType').val()).done(function (data) {
            if (data.success) {
                $("#tableContract").UifDataTable({ sourceData: data.result });
                $("#tableContractLevel").UifDataTable('clear');
                $("#rightBarLinks").hide();
                $("#alertMainContract").UifAlert('hide');
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }
});

$('#selectContractYear').on('itemSelected', function (event, selectedItem) {
    $("#tableContract").UifDataTable('clear');
    if (selectedItem.Id > 0) {
        $("#alertMainContract").UifAlert('hide');
    }
});

$('#selectContractType').on('itemSelected', function (event, selectedItem) {
    $("#tableContract").UifDataTable('clear');
    if (selectedItem.Id > 0) {

        $("#alertMainContract").UifAlert('hide');
        if (selectedItem.Id == 6) {

            $('#tabs').UifTabHeader("disabled", "#IdContractLevelCompany", true);
        } else {
            $('#tabs').UifTabHeader("disabled", "#IdContractLevelCompany", false);
        }
    }
});

$("#btnCopyContract").on('click', function () {
    $("#modalCopyContract").UifModal("showLocal", "Copiar Contrato");
});

$("#btnCopyContractSave").on('click', function () {
    $("#formCopyContract").validate();
    if ($("#formCopyContract").valid()) {
        CopyContract(contractId, $('#copyContractSmallDescription').val(), $('#selectCopyContractYear').val(), $('#copyContractDescription').val()).done(function (data) {
            if (data.success) {
                if (data.result) {
                    GetContractsByYearAndContractTypeId($('#selectContractYear').val(), $('#selectContractType').val()).done(function (response) {
                        if (response.success) {
                            $("#tableContract").UifDataTable({ sourceData: response.result });
                            $("#tableContractLevel").UifDataTable('clear');
                            $("#rightBarLinks").hide();
                            $("#alertMainContract").UifAlert('hide');
                        } else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });

                        }
                    });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.ContractDescriptionDuplicate, 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
            $('#modalCopyContract').UifModal('hide');
        });
    }
});

$('#modalCopyContract').on('closed.modal', function () {
    $('#copyContractSmallDescription').val("");
    $('#selectCopyContractYear').val("");
    $('#copyContractDescription').val("");
});

// NIVEL CONTRATO
$('#tableContractLevel').on('rowAdd', function (event, data) {
    $("#alertMainContractLevel").UifAlert('hide');
    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }
    $('#modalContractLevel').find("#ContractType").val($("#selectContractType").val());
    $("#ContractType").val($("#selectContractType").val());

    if ($('#selectContractYear').val() == "0") {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractYear, 'autoclose': true });
        return;
    }
    if ($('#selectContractType').val() == "0") {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractType, 'autoclose': true });
        return;
    }
    if (contractId == 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContract, 'autoclose': true });
        return;
    }

    var titleContractLevel = titleContract;
    $('#panelContractLevelCompany').text(titleContractLevel);
    $('#modalContractLevel').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddContractLevel?contractId=' + contractId + '&contractLevelId=0&contractTypeId=' + $('#selectContractType').val(), Resources.ContractLevelNewTitle);

});

$('#tableContractLevel').on('rowEdit', function (event, selectedRow) {
    $("#alertMainContractLevel").UifAlert('hide');
    levelContract = selectedRow.LayerNumber;
    $('#modalContractLevel').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddContractLevel?contractId='
        + contractId + '&contractLevelId=' + selectedRow.ContractLevelId + '&contractTypeId=' + $('#selectContractType').val(),
        smallTitleContract + ": " + Resources.ContractLevel + ' ' + selectedRow.Number);
});

$('#tableContractLevel').on('rowSelected', function (event, selectedRow) {
    $("#alertMainContract").UifAlert('hide');
    $("#alertMainContractLevel").UifAlert('hide');
    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
    }
    contractLevelId = selectedRow.ContractLevelId;
    levelContract = selectedRow.Number;
    var indexColumnsToHide = getCompanyLevelColumnsToHideByContractTypeId($('#selectContractType').val());
    var titleContractLevel = titleContract + '-' + Resources.ContractLevel + ' ' + levelContract;
    $('#panelContractLevelCompany').text(titleContractLevel);

    GetContractLevelCompanyByContractLevelId(contractLevelId).done(function (response) {
        if (response.success) {
            $("#tableContractLevelCompany").UifDataTable({ sourceData: response.result, hiddenColumns: indexColumnsToHide });
            if (response.result.length == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.LevelCompanyIsNecesary, 'autoclose': true });
            }
        }
    });
});

$('#tableContractLevel').on('rowDelete', function (event, data) {

    $("#alertMainContractLevel").UifAlert('hide');
    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }

    $('#modalContractLevelDelete').appendTo("body").UifModal('showLocal');

    contractLevelId = data.ContractLevelId;
    levelContract = data.Number;
});

$('#tableContractLevel').on('rowDeselected', function (event, data, position) {
    $('#panelContractLevelCompany').text("");
});

$("#btnContractLevelDelete").click(function () {
    $('#modalContractLevelDelete').modal('hide');
    $("#alertMainContractLevel").UifAlert('hide');
    lockScreen();

    DeleteContractLevel(contractId, contractLevelId, levelContract).done(function (data) {
        if (data.success == false) {
            if (data.result == null) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageDeleteContractLevel, 'autoclose': true });
            }
            else if (data.result == 0) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.DeleteContractOrder, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }
        else {
            GetContractLevelByContractId(contractId).done(function (response) {
                if (response.success) {
                    $("#tableContractLevel").UifDataTable({ sourceData: response.result });
                }
            });
        }
        unlockScreen();
    });
});

$('#tabs').on('change', function (event, tabs) {
    $("#alertMainContractLevel").UifAlert('hide');
    if (tabs != undefined) {
        if (tabs.hash == "#tabContractLevel") {
            var selected = $("#tableContract").UifDataTable('getSelected');
            if (selected == null) {
                contractId = 0;
                $("#tableContractLevel").UifDataTable('clear')
            }
        }
        if (tabs.hash == "#tabContractLevelCompany") {
            $("#alertMainLevelCompany").UifAlert('hide');
            var selectedContractlevel = $("#tableContractLevel").UifDataTable('getSelected');
            if (selectedContractlevel == null) {
                contractLevelId = 0;
                $("#tableContractLevelCompany").UifDataTable('clear')
            }
        }
    }
});

function onAddContractLevelComplete(data) {
    var indexColumnToHide = getContractLevelColumnsToHideByContractTypeId($('#selectContractType').val());

    if (data.success) {
        GetContractLevelByContractId(data.result).done(function (response) {
            if (response.success) {
                $("#tableContractLevel").UifDataTable({ sourceData: response.result, hiddenColumns: indexColumnToHide });
                $('#modalContractLevel').UifModal('hide');

                GetContractLevelCompanyByContractLevelId(oContractLevel.ContractLevelId).done(function (response2) {
                    if ((response2.result.length == 0) && ($('#selectContractType').val() != "6")) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.LevelCompanyIsNecesary, 'autoclose': true });
                    }
                });
            }
        });
    }
    else {
        $('#tableContractLevel').UifDataTable();
        $("#tableContractLevel").UifDataTable({ hiddenColumns: indexColumnToHide });
        $('#modalContractLevel').UifModal('hide');
        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
    }
}
// FIN NIVEL CONTRATO

// PAGOS NIVEL DE CONTRATO INI -*********************
function LevelPaymentModal() {
    $("#alertLevelPayment").UifAlert('hide');
    $("#alertMainContractLevel").UifAlert('hide');
    var tablelevel = $("#tableContractLevel").UifDataTable('getSelected');
    GetContractById(contractId).done(function (data) {
        if (data.success) {
            oContract = data.result;
            if (tablelevel != null) {
                ReloadLevelPayment().done(function () {
                    LoadlevelPaymentListView().done(function () {
                        SetTotalLevelPayment();
                        $('#paymentModalAdd').UifModal('showLocal', Resources.PaymentByLevelContract + " " + contractId);
                    });
                })
            }
            else {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractLevel, 'autoclose': true });
            }
        }
    });
};

function ReloadLevelPayment() {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetNextLevelNumberByLevelId",
        data: JSON.stringify({ "levelId": contractLevelId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#noPayment").val(parseInt(data));
            ClearFormLevelPayment();
        }
    });

}

function CancelLevelPayment() {
    $.ajax({
        async: false,
        type: "POST",
        url: REINS_ROOT + "Parameter/GetNextLevelNumberByLevelId",
        data: JSON.stringify({ "levelId": contractLevelId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#noPayment").val(parseInt(data));
            $("#levelPaymentListView").UifListView("refresh");
            ClearFormLevelPayment();
        }
    });
}

function LoadlevelPaymentListView() {
    return $.ajax({
        url: REINS_ROOT + "Parameter/GetLevelPaymentsByLevelId?levelId=" + contractLevelId,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var items = data.map(function (item) {
                item.Date = moment(item.Date).format('DD/MM/YYYY');
                return item;
            });
            $("#levelPaymentListView").UifListView({
                sourceData: items,
                customDelete: false,
                customAdd: false,
                customEdit: true,
                add: false,
                edit: true,
                delete: true,
                displayTemplate: "#paymentDisplayTemplate",
                addTemplate: '#add-template',
                deleteCallback: deleteLevelPayment
            });
        }
    });
}

function SetTotalLevelPayment() {
    var totalLevelPaymentAmount = 0;
    totalPayment = 0;
    totalLevelPaymentAmount = NotFormatMoney(TotalListLevelPayment("levelPaymentListView"));
    $("#totalLevelPaymentAmount").text(totalLevelPaymentAmount);
    $("#totalPayments").text(totalLevelPaymentAmount);
    totalPayment = NotFormatMoney(oContract.DepositPremiumAmount) - NotFormatMoney($("#totalPayments").val());
    $("#depositPremiumPayments").text(FormatMoney(totalPayment));
}

function TotalListLevelPayment(listView) {
    var total = 0;
    var list = $("#" + listView).UifListView("getData");
    list.forEach(function (data) {
        total += data.Amount.Value;
    });
    return total;
}

function ClearFormLevelPayment() {
    $("#paymentAmount").val(" ");
    $("#paymentDate").val(" ");
    levelPayId = 0;
}

function ValidNumberLevelPayment(listView, number) {
    var listView = $("#" + listView).UifListView("getData");
    if (listView.length == number) {
        return true;
    }
    else return false;
}

var deleteLevelPayment = function (deferred, data) {
    $("#alertLevelPayment").UifAlert('hide');
    if (ValidNumberLevelPayment("levelPaymentListView", data.Number) == false) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.DeleteContractOrder, 'autoclose': true });
        return;
    }

    $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteLevelPayment",
        data: JSON.stringify({
            "levelPaymentId": data.Id
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $.UifNotify('show', { 'type': 'success', 'message': Resources.DeleteSuccessfully, 'autoclose': true });
            ReloadLevelPayment().done(function () {
                LoadlevelPaymentListView().done(function () {
                    SetTotalLevelPayment();
                });
            })
        } else {
            deferred.reject();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.ErrorDeletingRecord, 'autoclose': true });
        }
    });
};

$("#levelPaymentListView").on('rowEdit', function (event, data, index) {
    rowselectLevelPayment = 1;
    $("#alertLevelPayment").UifAlert('hide');
    levelPayId = data.Id;

    $("#paymentAmount").val(data.Amount.Value);
    $("#paymentAmount").blur();

    $("#paymentDate").val(data.Date);
    $("#paymentDate").blur();
    $("#noPayment").val(data.Number);
});

$("#SaveAddLevelPayment").click(function () {
    $("#alertLevelPayment").UifAlert('hide');
    if ($("#formLevelPayment").valid()) {
        $("#paymentAmount").val($("#paymentAmount").val().trim());

        if ($("#paymentAmount").val() != "" || $("#paymentAmount").val() != null) {
            $("#paymentAmount").val(parseFloat(NotFormatMoney($("#paymentAmount").val())));
        } else if ($("#paymentAmount").val() <= 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.PaymentMustGreaterThanZero, 'autoclose': true });
        }
        $("#paymentDate").val($("#paymentDate").val().trim());

        oLevelPayment.Id = levelPayId;
        oLevelPayment.Level.ContractLevelId = contractLevelId;
        oLevelPayment.Number = $("#noPayment").val();
        oLevelPayment.Amount.Value = parseFloat($('#paymentAmount').val());
        oLevelPayment.Date = $("#paymentDate").val();

        if (paymentAmount < 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.PaymentExceedsDepositPremium, 'autoclose': true });
        }
        else {
            $.ajax({
                async: false,
                type: "POST",
                url: REINS_ROOT + "Parameter/SaveLevelPayment",
                data: JSON.stringify({ "levelPaymentDTO": oLevelPayment }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    ReloadLevelPayment().done(function () {
                        LoadlevelPaymentListView().done(function () {
                            SetTotalLevelPayment();
                        });
                    })
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.SaveSuccessfully, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.SaveError, 'autoclose': true });
                }
            });
        }
    }
});

$("#CancelLevelPayment").click(function () {
    $("#alertLevelPayment").UifAlert('hide');
    if (rowselectLevelPayment == 1) {
        CancelLevelPayment();
        rowselectLevelPayment = 0;
    }
    else {
        $("#paymentAmount").val(" ");
        $("#paymentDate").val(" ");
    }
});

$('#paymentAmount').on('blur', function (event) {
    $("#alertLevelPayment").UifAlert('hide');
    if ($("#paymentAmount").val() != "") {
        $("#paymentAmount").val(FormatMoney($("#paymentAmount").val()));
    }

    paymentAmount = (parseFloat(NotFormatMoney(totalPayment)) - parseFloat(NotFormatMoney($('#paymentAmount').val())));

    if (paymentAmount < 0) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.PaymentExceedsDepositPremium, 'autoclose': true });
        $("#SaveAddLevelPayment").prop("disabled", true);
    }
    else if (parseFloat(NotFormatMoney($('#paymentAmount').val())) <= 0) {
        $("#SaveAddLevelPayment").prop("disabled", true);
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.PaymentMustGreaterThanZero, 'autoclose': true });
    } else {
        $("#SaveAddLevelPayment").prop("disabled", false);
    }

});

$('#paymentDate').on('blur', function (event) {
    $("#alertLevelPayment").UifAlert('hide');
    if ($("#paymentDate").val() != '') {
        if (IsDate($("#paymentDate").val()) == false) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.InvalidDates, 'autoclose': true });
            $("#paymentDate").val("");
            $("#paymentDate").focus();
        }
    }
});

// PAGOS NIVEL DE CONTRATO FIN -*********************

// RESTABLECIMIENTOS NIVEL DE CONTRATO INI
function LevelRestoreModal() {
    $("#alertRestorePayment").UifAlert('hide');
    $("#alertMainContractLevel").UifAlert('hide');
    var tablelevel = $("#tableContractLevel").UifDataTable('getSelected');
    if (tablelevel != null) {
        ReloadLevelRestore();
        setTimeout(function () {
            $('#restoreModalAdd').UifModal('showLocal', Resources.RestoreByLevelContract + " " + contractId);
        }, 400);
    }
    else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractLevel, 'autoclose': true });
    }
}

function ReloadLevelRestore() {
    $.ajax({
        async: false,
        type: "POST",
        url: REINS_ROOT + "Parameter/GetNextNumberRestoreByLevelId",
        data: JSON.stringify({ "levelId": contractLevelId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#noRestore").val(parseInt(data));
            LoadlevelRestoreListView();
            ClearFormLevelRestore();
        }
    });
}

function LoadlevelRestoreListView() {
    $("#levelRestoreListView").UifListView({
        source: REINS_ROOT + "Parameter/GetLevelRestoresByLevelId?levelId=" + contractLevelId,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#restoreTemplate",
        addTemplate: '#add-template',
        deleteCallback: deleteLevelRestore
    });
}

function ClearFormLevelRestore() {
    $("#restorePercentage").val("0");
    $("#noticePercentage").val("0");
    levelRestId = 0;
}

function ValidateRestorePercentage(percentageValue) {
    if ((percentageValue == "") || (percentageValue > 100) || (percentageValue == 0)) {
        return true;
    }
    return false;
}

function valideRestoreSave() {
    if (ValidateRestorePercentage($("#noticePercentage").val())) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.NoticePercentage + ' ' + Resources.MessageLevelPercentage, 'autoclose': true });
        return true;
    }

    if (ValidateRestorePercentage($("#restorePercentage").val())) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ReestablishmentPercentage + ' ' + Resources.MessageLevelPercentage, 'autoclose': true });
        return true;
    }

    return false;
}

var deleteLevelRestore = function (deferred, data) {
    $("#alertRestorePayment").UifAlert('hide');
    if (ValidNumberLevelPayment("levelRestoreListView", data.Number) == false) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.DeleteContractOrder, 'autoclose': true });
        return;
    }

    $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteLevelRestore",
        data: JSON.stringify({
            "levelRestoreId": data.Id
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $.UifNotify('show', { 'type': 'success', 'message': Resources.DeleteSuccessfully, 'autoclose': true });
            ReloadLevelRestore();
        } else {
            deferred.reject();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.ErrorDeletingRecord, 'autoclose': true });
        }
    });
};

$("#levelRestoreListView").on('rowEdit', function (event, data, index) {
    rowselectLevelRestore = 1;
    $("#alertRestorePayment").UifAlert('hide');
    levelRestId = data.Id;
    $("#restorePercentage").val(data.RestorePercentage);
    $("#restorePercentage").blur();

    $("#noticePercentage").val(data.NoticePercentage);
    $("#noticePercentage").blur();

    $("#noRestore").val(data.Number);
});

$("#SaveAddLevelRestore").click(function () {
    $("#alertRestorePayment").UifAlert('hide');

    $("#restorePercentage").val($("#restorePercentage").val().trim());
    $("#noticePercentage").val($("#noticePercentage").val().trim());

    if ($("#formLevelRestore").valid()) {

        if (valideRestoreSave() == false) {

            oLevelRestore.Id = levelRestId;
            oLevelRestore.Level.ContractLevelId = contractLevelId;
            oLevelRestore.Number = $("#noRestore").val();
            oLevelRestore.RestorePercentage = parseFloat($('#restorePercentage').val());
            oLevelRestore.NoticePercentage = parseFloat($('#noticePercentage').val());

            $.ajax({
                async: false,
                type: "POST",
                url: REINS_ROOT + "Parameter/SaveLevelRestore",
                data: JSON.stringify({ "levelRestoreDTO": oLevelRestore }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"

            }).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.SaveSuccessfully, 'autoclose': true });
                    ReloadLevelRestore();
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.SaveError, 'autoclose': true });
                }
            });
        }
    }
});

$("#CancelLevelRestore").click(function () {
    $("#alertRestorePayment").UifAlert('hide');
    if (rowselectLevelRestore == 1) {
        ReloadLevelRestore();
        rowselectLevelRestore = 0;
    }
    else {
        $("#restorePercentage").val("0");
        $("#noticePercentage").val("0");
    }


});

$('#restorePercentage').on('blur', function (event) {
    $("#alertRestorePayment").UifAlert('hide');

    if (($("#restorePercentage").val() == "") || ($("#restorePercentage").val() > 100) || ($("#restorePercentage").val() == 0)) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ReestablishmentPercentage + ' ' + Resources.MessageLevelPercentage, 'autoclose': true });
    }
});

$('#noticePercentage').on('blur', function (event) {
    $("#alertRestorePayment").UifAlert('hide');
    if (ValidateRestorePercentage($("#noticePercentage").val())) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.NoticePercentage + ' ' + Resources.MessageLevelPercentage, 'autoclose': true });
    }
});

// RESTABLECIMIENTOS NIVEL DE CONTRATO FIN -*********************
// FIN NIVEL CONTRATO

// NIVEL CONTRATO COMPAÑÍA

$('#tableContractLevelCompany').on('rowAdd', function (event, data) {
    $("#alertMainLevelCompany").UifAlert('hide');
    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }
    if ($('#selectContractYear').val() == "0") {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractYear, 'autoclose': true });
        return;
    }
    if ($('#selectContractType').val() == "0") {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractType, 'autoclose': true });
        return;
    }
    if (contractLevelId == 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectContractLevel, 'autoclose': true });
        return;
    }
    var totalParticipationPercentage = 0;

    GetContractLevelCompanyByContractLevelId(contractLevelId).done(function (data) {
        if (data.success) {
            for (var i = 0; i < data.result.length; i++) {
                totalParticipationPercentage = totalParticipationPercentage + data.result[i].GivenPercentage;
            }

            if (totalParticipationPercentage < 100) {
                $('#modalContractLevelCompany').appendTo("body").UifModal('show', REINS_ROOT +
                    'Parameter/GetContractLevelCompany?contractLevelId=' + contractLevelId +
                    '&contractLevelCompanyId=0&brokerName=0&reinsuranceCompanyName=0' +
                    '&contractTypeId=' + $('#selectContractType').val(), Resources.ContractLevelCompanyNewTitle);

            } else {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidGivenPercentage, 'autoclose': true });
            }
        }
    });
});

$('#tableContractLevelCompany').on('rowEdit', function (event, selectedRow) {

    $("#alertMainContract").UifAlert('hide');
    $("#alertMainLevelCompany").UifAlert('hide');

    oCompany.IndividualId = selectedRow.Company.IndividualId;
    oAgent.IndividualId = selectedRow.Agent.IndividualId;
    $('#modalContractLevelCompany').appendTo("body").UifModal('show',
        REINS_ROOT + 'Parameter/GetContractLevelCompany?contractLevelId='
        + contractLevelId + '&contractLevelCompanyId=' + selectedRow.LevelCompanyId + '&brokerName=' + selectedRow.Agent.FullName
        + '&reinsuranceCompanyName=' + selectedRow.Company.FullName + '&contractTypeId=' + $('#selectContractType').val(),
        smallTitleContract + ": " + Resources.ContractLevel + ' ' + levelContract);
});

$('#tableContractLevelCompany').on('rowDelete', function (event, data) {
    $("#alertMainLevelCompany").UifAlert('hide');
    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }
    $('#modalContractLevelCompanyDelete').appendTo("body").UifModal('showLocal');
    contractLevelCompanyId = data.LevelCompanyId;
});

$("#btnContractLevelCompanyDelete").click(function () {
    $('#modalContractLevelCompanyDelete').modal('hide');
    DeleteContractLevelCompany(contractLevelId, contractLevelCompanyId).done(function (data) {
        if (data.success) {
            var contractLevelId = data.result;
            GetContractLevelCompanyByContractLevelId(contractLevelId).done(function (response) {
                if (response.success) {
                    $("#tableContractLevelCompany").UifDataTable({ sourceData: response.result });
                }
            });
        }
    });
});

function onAddContractLevelCompanyComplete(data) {
    var indexColumnsToHide = getCompanyLevelColumnsToHideByContractTypeId($('#selectContractType').val());
    var totalParticipationPercentage = 0;
    $("#alertMainContract").UifAlert('hide');
    if (data.success) {
        GetContractLevelCompanyByContractLevelId(data.result).done(function (response) {
            if (response.success) {
                $("#tableContractLevelCompany").UifDataTable({ sourceData: response.result, hiddenColumns: indexColumnsToHide });
                $('#modalContractLevelCompany').UifModal('hide');
                for (var i = 0; i < response.result.length; i++) {
                    totalParticipationPercentage = totalParticipationPercentage + response.result[i].GivenPercentage;
                }
                if (totalParticipationPercentage < 100) {
                    $.UifNotify('show', { 'type': 'danger', 'message': String.format(Resources.ValidatePercentageParticipationCompany, totalParticipationPercentage, 100 - totalParticipationPercentage), 'autoclose': true });
                }
            }
        });
    }
}

// FIN NIVEL CONTRATO COMPAÑÍA

function onAddCompleteContract(data) {
    if (data.success) {
        GetContractsByYearAndContractTypeId($('#selectContractYear').val(), $('#selectContractType').val()).done(function (response) {
            if (response.success) {
                $("#tableContract").UifDataTable({ sourceData: response.result });
                $('#modalContract').UifModal('hide');
                GetContractLevelByContractId(data.result).done(function (response2) {
                    if (response2.result.length == 0) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.ContractLevelIsNecesary, 'autoclose': true });
                    }
                });
            }
        });
    }
}

function convertToUpperCase(field) {
    field.value = field.value.toUpperCase();
}

function validatePercentage(field) {
    $("#" + field.id + "Msg").children().remove();
    if (parseFloat(field.value) < 0 || parseFloat(field.value) > 100) {
        $("#" + field.id + "Msg").append('<span> ' + Resources.ErrorPercentageHigher + '</span>');
        field.focus();
    }
    else {
        if (field.value == "") {
            field.value = 0
        }
    }
}

function getContractLevelColumnsToHideByContractTypeId(id) {
    switch (parseInt(id)) {
        case 1:
        case 13:
            return [3, 5]
        case 4:
        case 2:
            return [2]
        case 3:
            return [2, 3];
        default:
            return [];
    }
}

function getCompanyLevelColumnsToHideByContractTypeId(id) {
    switch (parseInt(id)) {
        case 3:
            return [6];
        default:
            return [];
    }
}

function GetContractsByYearAndContractTypeId(year, contractTypeId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractsByYearAndContractTypeId",
        data: JSON.stringify({ year: year, contractTypeId: contractTypeId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function GetContractLevelByContractId(contractId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractLevelByContractId",
        data: JSON.stringify({ contractId: contractId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function GetContractLevelCompanyByContractLevelId(contractLevelId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractLevelCompanyByContractLevelId",
        data: JSON.stringify({ contractLevelId: contractLevelId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function DeleteContract(contractId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteContract",
        data: JSON.stringify({ "contractId": contractId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    });
}

function DeleteContractLevel(contractId, contractLevelId, levelContract) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteContractLevel",
        data: JSON.stringify({
            contractId: contractId,
            contractLevelId: contractLevelId,
            level: levelContract
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function DeleteContractLevelCompany(contractLevelId, contractLevelCompanyId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteContractLevelCompany",
        data: JSON.stringify({ "contractLevelId": contractLevelId, "contractLevelCompanyId": contractLevelCompanyId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function GetContractById(contractId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractById",
        data: JSON.stringify({ "contractId": contractId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    });
}

function ValidateAllocation(contractId) {
    return validAllowPromise = new Promise(function (resolve) {
        $.ajax({
            type: "POST",
            url: REINS_ROOT + "Parameter/ValidateContractIssueAllocation",
            data: { "contractId": contractId },
            success: function (data) {
                if (data.success == false) {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
                else {
                    existAllocation = data.result;
                    if (data.result == true) {
                        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
                    }
                }
                resolve(data.result);
            }
        });
    });
}

function CopyContract(contractId, smallDescription, year, description) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/CopyContract",
        data: JSON.stringify({ contractId: contractId, smallDescription: smallDescription, year: year, description: description }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}
