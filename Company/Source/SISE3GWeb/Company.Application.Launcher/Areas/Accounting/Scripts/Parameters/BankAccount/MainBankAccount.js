var accountingAccountId = 0;
var accountingAccountNumber = "";
var accountBankId = 0;
var DisableDate;
var description = 0;
var date = "";
var status = 0;
var accountName = "";
var accountNumber = "";

var oBankAccountComm =
    {
        Id: 0,
        AccountTypeId: 0,
        Number: null,
        BankId: 0,
        Enabled: 0,
        Default: 0,
        CurrencyId: 0,
        GeneralLedgerId: 0,
        DisabledDate: 0,
        BranchId: 0,
        Description: null,
        GeneralLedgerNumber: ""
    };


if ($("#ViewBagBranchDisableBankAccount").val() == "1") {
    setTimeout(function () {
        $("#BranchSelect").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchSelect").removeAttr("disabled");
}

setTimeout(function () {
    GetBanks();
}, 2000);


//$(document).ready(function () {

var saveMainBankAccount = function (deferred, data) {
    $("#alertAccountBank").UifAlert('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveBankAccount",
        data: JSON.stringify({ "accountBankComm": SetDataBankAccount() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data > 0) {
            deferred.resolve();
            $("#alertAccountBank").UifAlert('show', Resources.SaveSuccessfully, "success");
            $("#BankSelectBankAccount").removeAttr("disabled");
            $("#BranchSelect").removeAttr("disabled");
        } else if (data == -1) {
            $("#alertAccountBank").UifAlert('show', Resources.AccountNumberAlreadyExists, "danger");
        }
        else {
            deferred.reject();
            $("#alertAccountBank").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};



$('#BranchSelect').on('itemSelected', function (event, selectedItem) {
    $("#alertAccountBank").UifAlert('hide');

    if ($('#BranchSelect').val() > 0) {
        var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelect').val();
        $("#BankSelectBankAccount").UifSelect({ source: controller });
    }
    else {
        $("#BankSelectBankAccount").UifSelect();
        $("#accountBankListView").find('.cancel-button').click();
        $("#accountBankListView").UifListView(
            {
                source: null
            });
        $('.add-button').hide();
    }
});

$('#BankSelectBankAccount').on('itemSelected', function (event, selectedItem) {
    $("#alertAccountBank").UifAlert('hide');
 
    if ($('#BankSelectBankAccount').val() > 0) {
        $("#accountBankListView").UifListView(
        {
            source: ACC_ROOT + "Parameters/GetAccountBankByBranchIdBankId?branchId="
                            + $('#BranchSelect').val() + "&bankId=" + $('#BankSelectBankAccount').val(),
            customDelete: false,
            customAdd: false,
            customEdit: true,
            add: true,
            edit: true,
            delete: false,
            displayTemplate: "#listTemplate",
            addTemplate: '#add-templateMainBank',
            addCallback: saveMainBankAccount
        });
    }
    else {
        $("#accountBankListView").find('.cancel-button').click();
        $("#accountBankListView").UifListView(
            {
                source: null
            });
        $('.add-button').hide();
    }
});



$("#accountBankListView").on("rowEdit", function (event, data, position) {
    $("#alertAccountBank").UifAlert('hide');
    $("#modalAccountBank").find("#Description").val(data.Description);
    accountBankId = data.AccountBankId;
    DisableDate = data.DisableDate;
    bindgStatus(data.Enabled);
    $('#modalAccountBank').UifModal('showLocal', Resources.EditRecord);
    $("#modalAccountBank").find("#dateBankAccount").val("");
    if (data.DisableDate != "") {
        $("#modalAccountBank").find("#dateBankAccount").val(DisableDate);
    }
});

$("#AccountBankRefresh").click(function () {
    $("#alertAccountBank").UifAlert('hide');
    $("#accountBankListView").UifListView("refresh");
});

$('#statusBankAccount').on('itemSelected', function () {
    $("#alertModelBankAccount").UifAlert('hide');
    if ($('#statusBankAccount').val() != 0 || $('#statusBankAccount').val() == null || $('#statusBankAccount').val() == "") {
        $("#dateBankAccount").attr("disabled", "disabled");
        $("#dateBankAccount").val("");
    }
    else {
        $("#dateBankAccount").attr("disabled", false);
    }
});

function validateDateDisabled() {
    var result = true;
    if (status == "0") {// && (date == "" || date == null)) {
        if (date == "" || date == null) {
            result = false;
        }
    }
    else if (status == "") {
        result = false;
    }

    return result;
}

$("#saveButtonBankAccount").click(function () {
    $("#editAccountBank").validate();

    if ($("#editAccountBank").valid()) {
        $("#alertModelBankAccount").UifAlert('hide');
        description = $('#modalAccountBank').find('#Description').val();
        date = $('#modalAccountBank').find('#dateBankAccount').val();
        status = $('#modalAccountBank').find('#statusBankAccount').val();

        if (validateDateDisabled()) {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "Parameters/UpdateBankAccount",
                data: JSON.stringify({
                    "bankAccountId": accountBankId, "description": description,
                    "disabledDate": date, "enabled": status
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#editAccountBank").formReset();
                    $('#modalAccountBank').UifModal('hide');
                    $("#accountBankListView").UifListView('refresh');

                    $("#alertAccountBank").UifAlert('show', Resources.EditSuccessfully, "success");
                }
            });
        }
        else {
            $("#alertModelBankAccount").UifAlert('show', Resources.DateRequired, "danger");
        }
    }
});

function bindgStatus(statusCode) {
    var statusId = statusCode ? 1 : 0;

    var controller = ACC_ROOT + "Parameters/GetStatus";
    $("#modalAccountBank").find('#statusBankAccount').UifSelect({ source: controller, selectedId: statusId });

    if (statusId == 0) {
        $("#dateBankAccount").attr("disabled", false);
    }
    else {
        $("#dateBankAccount").attr("disabled", "disabled");
    }
}

function SetDataBankAccount() {
    oBankAccountComm.Id = 0;
    oBankAccountComm.AccountTypeId = $("#AccountTypeId").val();
    oBankAccountComm.Number = $("#AccountNumber").val();
    oBankAccountComm.BankId = $("#BankSelectBankAccount").val();
    oBankAccountComm.Enabled = $("#Enabled").val();
    oBankAccountComm.Default = 1;
    oBankAccountComm.CurrencyId = $("#CurrencyId").val();
    oBankAccountComm.GeneralLedgerId = accountingAccountId;
    oBankAccountComm.DisabledDate = "";//$("#").val();
    oBankAccountComm.BranchId = $("#BranchSelect").val();
    oBankAccountComm.Description = $("#Description").val().toUpperCase();
    oBankAccountComm.GeneralLedgerNumber = accountingAccountNumber; //geneneralNumber;

    return oBankAccountComm;
}

function GetBanks() {
    if ($('#BranchSelect').val() > 0) {
        var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelect').val();
        $("#BankSelectBankAccount").UifSelect({ source: controller });
    }
};