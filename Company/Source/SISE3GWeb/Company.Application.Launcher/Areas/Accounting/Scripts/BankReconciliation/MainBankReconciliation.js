var countRecord = 1;
var bankAmount = 0;
var accountingAmount = 0;
var conciliationDate = $("#ViewBagAccountingDateBankReconcilation").val();
var reconcilationCode = 0;

var checkType = '#checkType';
var checkDateReconciliation = '#checkDateReconciliation';
var checkMonth = '#checkMonth';
var checkBranch = '#checkBranch';
var checkVoucher = '#checkVoucher';
var checkAutoType = '#checkAutoType';
var checkAutoDate = '#checkAutoDate';
var checkAutoMonth = '#checkAutoMonth';
var checkAutoBranch = '#checkAutoBranch';
var checkAutoVoucher = '#checkAutoVoucher';
var checkOriginBank = '#checkOriginBank';
var checkOriginCAccounting = '#checkOriginCAccounting';
var checkOriginDAccounting = '#checkOriginDAccounting';
var indexTableConciliation = 0;

oBranch = {
    Id: 0,
    Description: ""
};

oBankStatement = {
    Id: 0,
    Origin: "",
    MovementTypeCode: 0,
    MovementTypeDescription: "",
    MovementDate: "",
    DocumentNumber: "",
    Description: "",
    Amount: 0,
    //BranchId: 0,
    Branch: oBranch
};

oStatement = {
    BankStatementId: 0,
    BankId: 0,
    BankDescription: "",
    BankingMovementTypeId: 0,
    BankingMovementTypeDescription: "",
    BranchId: 0,
    BranchDescription: "",
    VoucherNumber: "",
    MovementDate: "",
    MovementAmount: "",
    MovementDescription: "",
    MovementThird: "",
    MovementOrigin: ""
};

oAccountBank = {
    Id: 0,
    Description: ""
};

oConciliationModel = {
    Id: 0,
    //AccountBank: oAccountBank,
    AccountBankId: 0,
    DateTo: "",
    ConciliationDate: "",
    Statements: []
};

var array = [];
var index = [];
var result = -1;

function RowModel() {
    this.Id;
    this.BankId;
    this.MovementOrigin;
    this.MovementId;
    this.BankingMovementTypeId;
    this.BankingMovementTypeDescription;
    this.BranchId;
    this.BranchDescription;
    this.VoucherNumber;
    this.MovementDate;
    this.MovementBankAmount;
    this.MovementAccountingAmount;
    this.MovementDescription;
    this.MovementDifference;
    this.ConciliationId;
}

function ConciliationParameterModel() {
    this.AccountBankId;
    this.SortingType;
    this.SortingDate;
    this.SortingMonth;
    this.SortingBranch;
    this.SortingVoucher;
    this.Bank;
    this.CentralAccounting;
    this.DailyAccounting;
    this.AutomaticType;
    this.AutomaticDate;
    this.AutomaticMonth;
    this.AutomaticBranch;
    this.AutomaticVoucher;
}

$(document).ready(function () {

    $("#btnSaveConciliation").attr("disabled", true);

    //////////////////////////////////////
    // Botón Grabar desde el modal      //
    //////////////////////////////////////
    $('#btnSaveModal').click(function () {
        $('#modalSave').modal('hide');
        var ids = $("#tableConciliation").UifDataTable("getData");

        if (ids.length > 0) {
            //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

            // Se valida que la fecha de conciliación este dentro de una fecha contable
            $("#alertReconciliation").UifAlert('hide');
            if (ValidateConciliationDate(conciliationDate, $("#ConciliationDate").val()) == false) {
                $("#alertReconciliation").UifAlert('show', Resources.ValidationConciliationDate, "warning");
                return true;
            }

            // Se valida que la fecha hasta no sea mayor a la fecha actual
            if (ValidateDateTo($("#DateToReconciliation").val()) == false) {
                $("#alertReconciliation").UifAlert('show', Resources.ValidationDateTo, "warning");
                return true;
            }

            // Se valida que seleccione el banco
            if ($("#selectBankReconciliation").val() == "") {
                $("#alertReconciliation").UifAlert('show', Resources.BankNameRequired, "warning");
                return true;
            }

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


            oConciliationModel.AccountBankId = $('#selectBankReconciliation').val();
            oConciliationModel.ConciliationDate = $("#ConciliationDate").val();
            oConciliationModel.DateTo = $("#DateToReconciliation").val();
            oConciliationModel.Id = -1;
            oConciliationModel.Statements.push({
                "MovementAmount": 0,
                "BranchId": 0,
                "BranchDescription": "",
                "Description": "U",
                "VoucherNumber": "",
                "BankStatementId": 0,
                "MovementDate": "",
                "BankingMovementTypeId": 0,
                "BankingMovementTypeDescription": "",
                "MovementOrigin": "B",
                "BankId": $("#selectBankReconciliation").val(),
                "BankDescription": "",
                "MovementDescription": "U",
                "MovementThird": ""
            });

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "BankReconciliation/SaveConciliation",
                data: JSON.stringify({ "conciliationModel": oConciliationModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //$("#tableConciliation").dataTable().fnClearTable();
                    $('#tableConciliation').UifDataTable('deleteRow', indexTableConciliation);
                    $("#alertReconciliation").UifAlert('show', Resources.MessageSaveConciliation + ' ' + reconcilationCode, "success");
                    LoadMovementsPending();
                    $("#btnSaveConciliation").attr("disabled", true);
                    $.unblockUI();
                }
            });
        }
        else {
            setTimeout(function () {
                $("#alertReconciliation").UifAlert('show', Resources.MovementsThereUnreconciled, "warning");
            }, 3000);
        }
    });
});

$('div#container').hide();
var conciliationParameterModel = new ConciliationParameterModel();

/////////////////////////////////////////////////////////////////////////
// Tabla movimientos pendientes de conciliar                           //
/////////////////////////////////////////////////////////////////////////
$('#tablePendingReconciliation').on('rowSelected', function (event, data, position) {

    $("#record").text(countRecord++);

    if (data.MovementOrigin == "B") {
        bankAmount += parseFloat(data.MovementAmount);
        bankAmount = decimalAdjust('round', bankAmount, -2);
        $("#bankAmount").text(bankAmount);
    }
    if (data.MovementOrigin == "C" || data.MovementOrigin == "D") {
        accountingAmount += parseFloat(data.MovementAmount);
        accountingAmount = decimalAdjust('round', accountingAmount, -2);
        $("#accountingAmount").text(accountingAmount);
    }
    var difference = parseFloat(bankAmount) - parseFloat(accountingAmount);
    difference = decimalAdjust('round', difference, -2);

    $("#movementType").text(data.BankingMovementTypeId);
    $("#origin").text(data.MovementOrigin);
    $("#movementDifference").text(difference);

    // Guarda los items seleccionados
    result = $.inArray(position, index);

    if (array.length == 0) {
        array.push(data.Id);
        index.push(position);
        oConciliationModel.Statements.push({
            "MovementAmount": data.MovementAmount,
            "BranchId": data.BranchId,
            "BranchDescription": "",
            "Description": data.MovementDescription,
            "VoucherNumber": data.VoucherNumber,
            "BankStatementId": data.MovementId,
            "MovementDate": data.MovementDate,
            "BankingMovementTypeId": data.BankingMovementTypeId,
            "BankingMovementTypeDescription": data.BankingMovementTypeDescription,
            "MovementOrigin": data.MovementOrigin,
            "BankId": $("#selectBankReconciliation").val(),
            "BankDescription": "",
            "MovementDescription": "",
            "MovementThird": ""
        });
    }
    else {
        if (result == -1) {
            array.push(data.Id);
            index.push(position);
            oConciliationModel.Statements.push({
                "MovementAmount": data.MovementAmount,
                "BranchId": data.BranchId,
                "BranchDescription": "",
                "Description": data.MovementDescription,
                "VoucherNumber": data.VoucherNumber,
                "BankStatementId": data.MovementId,
                "MovementDate": data.MovementDate,
                "BankingMovementTypeId": data.BankingMovementTypeId,
                "BankingMovementTypeDescription": data.BankingMovementTypeDescription,
                "MovementOrigin": data.MovementOrigin,
                "BankId": $("#selectBankReconciliation").val(),
                "BankDescription": "",
                "MovementDescription": "",
                "MovementThird": ""
            });
        }
        else {
            index.splice(result, 1);
            array.splice(result, 1);
        }
    }
});

var table = $('#tablePendingReconciliation').DataTable();

$('body').delegate('#tablePendingReconciliation tbody tr', "click", function () {

    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
    }

    var pendings = $("#tablePendingReconciliation").UifDataTable("getSelected");

    if (pendings != null) {
        countRecord = 1;
        bankAmount = 0;
        accountingAmount = 0;

        for (var j = 0; j < pendings.length; j++) {

            $("#record").text(countRecord++);

            if (pendings[j].MovementOrigin == "B") {
                bankAmount += parseFloat(pendings[j].MovementAmount);
                bankAmount = decimalAdjust('round', bankAmount, -2)
            }
            if (pendings[j].MovementOrigin == "C" || pendings[j].MovementOrigin == "D") {
                accountingAmount += parseFloat(pendings[j].MovementAmount);
                accountingAmount = decimalAdjust('round', accountingAmount, -2)
            }
            var difference = parseFloat(bankAmount) - parseFloat(accountingAmount);
            difference = decimalAdjust('round', difference, -2)

            $("#bankAmount").text(bankAmount);
            $("#accountingAmount").text(accountingAmount);
            $("#movementType").text(pendings[j].BankingMovementTypeId);
            $("#origin").text(pendings[j].MovementOrigin);
            $("#movementDifference").text(difference);
        }
    }
    else {
        $("#record").text("");
        $("#bankAmount").text("");
        $("#accountingAmount").text("");
        $("#movementType").text("");
        $("#origin").text("");
        $("#movementDifference").text("");
    }
});

/////////////////////////////////////////////////////////////////////////
// Tabla movimientos temporales conciliados                            //
/////////////////////////////////////////////////////////////////////////
$('#tableConciliation').on('rowSelected', function (event, data, position) {

    indexTableConciliation = position;
    $("#btnSaveConciliation").attr("disabled", false);
    $('#modalView').UifModal('showLocal', Resources.MovementDetails);

    var controller = ACC_ROOT + "BankReconciliation/GetConciliationDetailsByConciliationId?conciliationId=" +
                                     data.ConciliationId;

    $("#modalView").find("#tableDetails").UifDataTable({ source: controller });
    reconcilationCode = data.ConciliationId;

});

//////////////////////////////////////
// Botón Conciliar                  //
//////////////////////////////////////
$('#btnReconcile').click(function () {
    //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

    var difference = $("#movementDifference").text();
    var maximumDeviation = $("#ViewBagMaximumDeviation").val();

    if ((parseFloat(difference) >= parseFloat(-maximumDeviation)) && (parseFloat(difference) <= parseFloat(maximumDeviation))) {
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

        oConciliationModel.AccountBankId = $('#selectBankReconciliation').val();
        oConciliationModel.ConciliationDate = $("#ConciliationDate").val();
        oConciliationModel.DateTo = $("#DateToReconciliation").val();
        oConciliationModel.Id = 0;
        oConciliationModel.Statements = [];

        var pendings = $("#tablePendingReconciliation").UifDataTable("getSelected");

        for (var j = 0; j < pendings.length; j++) {
            oConciliationModel.Statements.push({
                "MovementAmount": pendings[j].MovementAmount,
                "BranchId": pendings[j].BranchId,
                "BranchDescription": "",
                "Description": pendings[j].MovementDescription,
                "VoucherNumber": pendings[j].VoucherNumber,
                "BankStatementId": pendings[j].MovementId,
                "MovementDate": pendings[j].MovementDate,
                "BankingMovementTypeId": pendings[j].BankingMovementTypeId,
                "BankingMovementTypeDescription": pendings[j].BankingMovementTypeDescription,
                "MovementOrigin": pendings[j].MovementOrigin,
                "BankId": $("#selectBankReconciliation").val(),
                "BankDescription": "",
                "MovementDescription": "",
                "MovementThird": ""
            });
        }

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankReconciliation/SaveConciliation",
            data: JSON.stringify({ "conciliationModel": oConciliationModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var rowModel = new RowModel();
                rowModel.Id = "**";
                rowModel.BankId = $('#selectBankReconciliation').val();
                rowModel.MovementOrigin = "**";
                rowModel.MovementId = "**";
                rowModel.BankingMovementTypeId = "**";
                rowModel.BankingMovementTypeDescription = "**";
                rowModel.BranchId = "1";
                rowModel.BranchDescription = "";
                rowModel.VoucherNumber = "**";
                rowModel.MovementDate = "**";
                rowModel.MovementBankAmount = $("#bankAmount").text();
                rowModel.MovementAccountingAmount = $("#accountingAmount").text();
                rowModel.MovementDescription = "";
                rowModel.MovementDifference = difference;
                rowModel.ConciliationId = data[0].ConciliationId;

                $('#tableConciliation').UifDataTable('addRow', rowModel);

                $("#alertReconciliation").UifAlert('show', Resources.MessageSaveTemporaryConciliation, "success");

                $("#bankAmount").text('');
                $("#accountingAmount").text('');
                $("#movementType").text('');
                $("#origin").text('');
                $("#movementDifference").text('');
                $("#record").text('');
                countRecord = 1;
                bankAmount = 0;
                accountingAmount = 0;

                oConciliationModel.Statements = [];

                LoadMovementsPending();
                $.unblockUI();
            }
        });
    }
    else {
        $("#alertReconciliation").UifAlert('show', Resources.MessageValueReconcile, "warning");
    }
});

$('#tableMultiple').on('selectAll', function (event) {
    //alert("Se han seleccionado todos los items.");
});

$('#selectBankReconciliation').on('itemSelected', function (event, selectedItem) {
    $("#alertReconciliation").UifAlert('hide');
    $("#tablePendingReconciliation").dataTable().fnClearTable();
    $("#tableConciliation").dataTable().fnClearTable();
});

$('#tabMainConciliation').on('change', function (event, newly, old) {
    //alert("Ha seleccionado el tab: " + newly);
    //alert("El tab anterior era: " + old)
});

//////////////////////////////////////
// Botón Grabar                     //
//////////////////////////////////////
$('#btnSaveConciliation').click(function () {
    $("#formBankConciliation").validate();

    if ($("#formBankConciliation").valid()) {
        $('#modalSave').UifModal('showLocal', Resources.MessageSaveConciliationTitle);
        return true;
    }
});

//////////////////////////////////////
// Botón Cancelar                   //
//////////////////////////////////////
$('#btnCancelReconciliation').click(function () {
    $("#tablePendingReconciliation").dataTable().fnClearTable();
    $("#tableConciliation").dataTable().fnClearTable();
    $("#selectBankReconciliation").val("");
    $("#btnSaveConciliation").attr("disabled", true);
});

//////////////////////////////////////
// CheckBox                         //
//////////////////////////////////////

$(checkType).click(function () {
    if ($(checkType).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkType);
    }
    else if ($(checkType).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkType);
    }
});

$(checkDateReconciliation).click(function () {
    if ($(checkDateReconciliation).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkDateReconciliation);
    }
    else if ($(checkDateReconciliation).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkDateReconciliation);
    }
});

$(checkMonth).click(function () {
    if ($(checkMonth).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkMonth);
    }
    else if ($(checkMonth).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkMonth);
    }
});

$(checkBranch).click(function () {
    if ($(checkBranch).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkBranch);
    }
    else if ($(checkBranch).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkBranch);
    }
});

$(checkVoucher).click(function () {
    if ($(checkVoucher).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkVoucher);
    }
    else if ($(checkVoucher).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkVoucher);
    }
});

$(checkAutoType).click(function () {
    if ($(checkAutoType).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkAutoType);
    }
    else if ($(checkAutoType).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkAutoType);
    }
});

$(checkAutoDate).click(function () {
    if ($(checkAutoDate).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkAutoDate);
    }
    else if ($(checkAutoDate).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkAutoDate);
    }
});

$(checkAutoMonth).click(function () {
    if ($(checkAutoMonth).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkAutoMonth);
    }
    else if ($(checkAutoMonth).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkAutoMonth);
    }
});

$(checkAutoBranch).click(function () {
    if ($(checkAutoBranch).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkAutoBranch);
    }
    else if ($(checkAutoBranch).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkAutoBranch);
    }
});

$(checkAutoVoucher).click(function () {
    if ($(checkAutoVoucher).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkAutoVoucher);
    }
    else if ($(checkAutoVoucher).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkAutoVoucher);
    }
});

$(checkOriginBank).click(function () {
    if ($(checkOriginBank).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkOriginBank);
    }
    else if ($(checkOriginBank).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkOriginBank);
    }
});

$(checkOriginCAccounting).click(function () {
    if ($(checkOriginCAccounting).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkOriginCAccounting);
    }
    else if ($(checkOriginCAccounting).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkOriginCAccounting);
    }
});

$(checkOriginDAccounting).click(function () {
    if ($(checkOriginDAccounting).hasClass("glyphicon glyphicon-unchecked")) {
        checkBankReconciliation(checkOriginDAccounting);
    }
    else if ($(checkOriginDAccounting).hasClass("glyphicon glyphicon-check")) {
        uncheckBankReconciliation(checkOriginDAccounting);
    }
});
/*Fin CheckBox*/

//////////////////////////////////////
// Botón Procesar                   //
//////////////////////////////////////
$('#btnProcessReconciliation').click(function () {
    $("#tablePendingReconciliation").UifDataTable();
    $("#tableConciliation").UifDataTable();

    $("#record").text("");
    $("#bankAmount").text("");
    $("#origin").text("");
    $("#accountingAmount").text("");
    $("#movementType").text("");
    $("#movementDifference").text("");

    $("#formBankConciliation").validate();
    $("#tablePendingReconciliation").UifDataTable();
    if ($("#formBankConciliation").valid()) {
        //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        if ($('#selectBankReconciliation').val() != null) {

            //Se valida que la fecha de conciliación este dentro de una fecha contable
            $("#alertReconciliation").UifAlert('hide');
            if (ValidateConciliationDate(conciliationDate, $("#ConciliationDate").val()) == false) {
                $("#alertReconciliation").UifAlert('show', Resources.ValidationConciliationDate, "warning");
                return true;
            }

            //Se valida que la fecha hasta no sea mayor a la fecha actual
            if (ValidateDateTo($("#DateToReconciliation").val()) == false) {
                $("#alertReconciliation").UifAlert('show', Resources.ValidationDateTo, "warning");
                return true;
            }

            // Se valida que seleccione el banco
            if ($("#selectBankReconciliation").val() == "") {
                $("#alertReconciliation").UifAlert('show', "Seleccione el banco", "warning");
                return true;
            }

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

            var sortingType = ($('#checkType').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var sortingDate = ($('#checkDateReconciliation').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var sortingMonth = ($('#checkMonth').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var sortingBranch = ($('#checkBranch').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var sortingVoucher = ($('#checkVoucher').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var bank = ($('#checkOriginBank').hasClass('glyphicon glyphicon-check')) ? 'B' : '';
            var centralAccounting = ($('#checkOriginCAccounting').hasClass('glyphicon glyphicon-check')) ? 'C' : '';
            var dailyAccounting = ($('#checkOriginDAccounting').hasClass('glyphicon glyphicon-check')) ? 'D' : '';
            var automaticType = ($('#checkAutoType').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var automaticDate = ($('#checkAutoDate').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var automaticMonth = ($('#checkAutoMonth').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var automaticBranch = ($('#checkAutoBranch').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            var automaticVoucher = ($('#checkAutoVoucher').hasClass('glyphicon glyphicon-check')) ? 1 : 0;

            $('div#container').show();

            var concile = ACC_ROOT + "BankReconciliation/GetMovementConciliationByAccountBankId?accountBankId=" +
                                $('#selectBankReconciliation').val() + "&dateTo=" + $('#DateToReconciliation').val() + "&conciliationDate=" +
                                $("#ConciliationDate").val() + "&bank=" + bank + "&centralAccounting=" +
                                centralAccounting + "&dailyAccounting=" + dailyAccounting + "&automaticType=" +
                                automaticType + "&automaticDate=" + automaticDate + "&automaticMonth=" +
                                automaticMonth + "&automaticBranch=" + automaticBranch + "&automaticVoucher=" +
                                automaticVoucher;
            $("#tableConciliation").UifDataTable({ source: concile });


            setTimeout(function () {
                $("#tablePendingReconciliation").UifDataTable();
                var controller = ACC_ROOT + "BankReconciliation/GetMovementPendingByAccountBankId?accountBankId=" +
                                $('#selectBankReconciliation').val() + "&dateTo=" + $('#DateToReconciliation').val() + "&sortingType=" +
                                sortingType + "&sortingDate=" + sortingDate + "&sortingMonth=" + sortingMonth +
                                "&sortingBranch=" + sortingBranch + "&sortingVoucher=" + sortingVoucher +
                                "&bank=" + bank + "&centralAccounting=" + centralAccounting + "&dailyAccounting=" +
                                dailyAccounting + "&automaticType=" + automaticType + "&automaticDate=" +
                                automaticDate + "&automaticMonth=" + automaticMonth + "&automaticBranch=" +
                                automaticBranch + "&automaticVoucher=" + automaticVoucher;
                $("#tablePendingReconciliation").UifDataTable({ source: controller });
                $('div#container').hide();
                $.unblockUI();
            }, 6000);
        }
        else {
            $("#alertReconciliation").UifAlert('show', Resources.BillingSummaryPayBankIdWarning, "warning");
        }
    }
});

////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones
////////////////////////////////////////////////////////////////////////////////////////////
function LoadMovementsPending() {
    var sortingType = ($('#checkType').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var sortingDate = ($('#checkDateReconciliation').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var sortingMonth = ($('#checkMonth').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var sortingBranch = ($('#checkBranch').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var sortingVoucher = ($('#checkVoucher').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var bank = ($('#checkOriginBank').hasClass('glyphicon glyphicon-check')) ? 'B' : '';
    var centralAccounting = ($('#checkOriginCAccounting').hasClass('glyphicon glyphicon-check')) ? 'C' : '';
    var dailyAccounting = ($('#checkOriginDAccounting').hasClass('glyphicon glyphicon-check')) ? 'D' : '';
    var automaticType = ($('#checkAutoType').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var automaticDate = ($('#checkAutoDate').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var automaticMonth = ($('#checkAutoMonth').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var automaticBranch = ($('#checkAutoBranch').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    var automaticVoucher = ($('#checkAutoVoucher').hasClass('glyphicon glyphicon-check')) ? 1 : 0;

    var controller = ACC_ROOT + "BankReconciliation/GetMovementPendingByAccountBankId?accountBankId=" +
                                     $('#selectBankReconciliation').val() + "&dateTo=" + $('#DateToReconciliation').val() + "&sortingType=" +
                                     sortingType + "&sortingDate=" + sortingDate + "&sortingMonth=" + sortingMonth +
                                     "&sortingBranch=" + sortingBranch + "&sortingVoucher=" + sortingVoucher +
                                     "&bank=" + bank + "&centralAccounting=" + centralAccounting + "&dailyAccounting=" +
                                     dailyAccounting + "&automaticType=" + automaticType + "&automaticDate=" +
                                     automaticDate + "&automaticMonth=" + automaticMonth + "&automaticBranch=" +
                                     automaticBranch + "&automaticVoucher=" + automaticVoucher;
    $("#tablePendingReconciliation").UifDataTable({ source: controller });
}

function SetDataParameter() {
    conciliationParameterModel.AccountBankId = $('#selectBankReconciliation').val();
    if ($('#checkType').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.SortingType = 1;
    }
    else {
        conciliationParameterModel.SortingType = 0;
    }
    if ($('#checkDateReconciliation').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.SortingDate = 1;
    }
    else {
        conciliationParameterModel.SortingDate = 0;
    }
    if ($('#checkMonth').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.SortingMonth = 1;
    }
    else {
        conciliationParameterModel.SortingMonth = 0;
    }
    if ($('#checkBranch').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.SortingBranch = 1;
    }
    else {
        conciliationParameterModel.SortingBranch = 0;
    }
    if ($('#checkVoucher').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.SortingVoucher = 1;
    }
    else {
        conciliationParameterModel.SortingVoucher = 0;
    }
    if ($('#checkOriginBank').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.Bank = "B";
    }
    else {
        conciliationParameterModel.Bank = "";
    }
    if ($('#checkOriginCAccounting').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.CentralAccounting = "C";
    }
    else {
        conciliationParameterModel.CentralAccounting = "";
    }
    if ($('#checkOriginDAccounting').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.DailyAccounting = "D";
    }
    else {
        conciliationParameterModel.DailyAccounting = "";
    }
    if ($('#checkAutoType').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.AutomaticType = 1;
    }
    else {
        conciliationParameterModel.AutomaticType = 0;
    }
    if ($('#checkAutoDate').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.AutomaticDate = 1;
    }
    else {
        conciliationParameterModel.AutomaticDate = 0;
    }
    if ($('#checkAutoMonth').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.AutomaticMonth = 1;
    }
    else {
        conciliationParameterModel.AutomaticMonth = 0;
    }
    if ($('#checkAutoBranch').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.AutomaticBranch = 1;
    }
    else {
        conciliationParameterModel.AutomaticBranch = 0;
    }
    if ($('#checkAutoVoucher').hasClass('glyphicon glyphicon-check')) {
        conciliationParameterModel.AutomaticVoucher = 1;
    }
    else {
        conciliationParameterModel.AutomaticVoucher = 0;
    }
}

function checkSpan() {
    var sortingType = ($('#checkType').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var sortingDate = ($('#checkDateReconciliation').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var sortingMonth = ($('#checkMonth').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var sortingBranch = ($('#checkBranch').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var sortingVoucher = ($('#checkVoucher').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var bank = ($('#checkOriginBank').hasClass('glyphicon glyphicon-check')) ? 'B' : '';
    var centralAccounting = ($('#checkOriginCAccounting').hasClass('glyphicon glyphicon-check')) ? 'C' : '';
    var dailyAccounting = ($('#checkOriginDAccounting').hasClass('glyphicon glyphicon-check')) ? 'D' : '';
    var automaticType = ($('#checkAutoType').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var automaticDate = ($('#checkAutoDate').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var automaticMonth = ($('#checkAutoMonth').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var automaticBranch = ($('#checkAutoBranch').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
    var automaticVoucher = ($('#checkAutoVoucher').hasClass('glyphicon glyphicon-check')) ? -1 : 0;
}

function checkBankReconciliation(checkbox) {
    $(checkbox).removeClass("glyphicon glyphicon-unchecked");
    $(checkbox).addClass("glyphicon glyphicon-check");
}

function uncheckBankReconciliation(checkbox) {
    $(checkbox).removeClass("glyphicon glyphicon-check");
    $(checkbox).addClass("glyphicon glyphicon-unchecked");
}