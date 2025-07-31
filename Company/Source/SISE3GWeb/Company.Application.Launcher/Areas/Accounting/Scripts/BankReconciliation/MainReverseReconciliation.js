var conciliationDateReverse = $("#ViewBagAccountingDateReverse").val();

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


oConciliation = {
    Id: 0,
    AccountBankId: 0,
    DateTo: "",
    ConciliationDate: "",
    Statements: []
};

oReverseConciliationModel = {
    Id: 0,
    ReverseDate: "",
    Conciliations: []
};

$(document).ready(function ()
{
    //////////////////////////////////////
    /// Botón Grabar desde modal       ///
    //////////////////////////////////////
    $('#btnSaveModalReverse').click(function () {
        $('#modalReverse').modal('hide');

        //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        //Se valida que la fecha de reversión este dentro de una fecha contable
        $("#alertReverse").UifAlert('hide');
        if (ValidateReverseDate(conciliationDateReverse, $("#ReversalDate").val()) == false) {
            $("#alertReverse").UifAlert('show', Resources.ValidationReversalDate, "warning");
            return true;
        }

        // Se valida que ingrese el número de conciliación
        if ($("#ConciliationNumber").val() == "") {
            $("#alertReverse").UifAlert('show', Resources.ConciliationNumberEntry, "warning");
            return true;
        }

        $('div#container').show();

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


        oReverseConciliationModel.Id = 0;
        oReverseConciliationModel.ReverseDate = $("#ReversalDate").val();

        var conciliations = $("#tableReverseConciliation").UifDataTable("getSelected");

        for (var j = 0; j < conciliations.length; j++) {
            oReverseConciliationModel.Conciliations.push({
                "AccountBankId": 0,
                "ConciliationDate": $("#ReversalDate").val(),
                "DateTo": $("#ReversalDate").val(),
                "Id": conciliations[j].ConciliationId,
                "Statements": []
            });
        }

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankReconciliation/ReverseConciliation",
            data: JSON.stringify({ "reverseConciliationModel": oReverseConciliationModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('div#container').hide();

                $("#tableReverseConciliation").dataTable().fnClearTable();

                $("#alertReverse").UifAlert('show', Resources.MessageReverseConciliation, "success");
                $("#ConciliationNumber").val("");

                LoadConciliationMovements();
                $.unblockUI();
            }
        });
    });

})
$('div#container').hide();

//////////////////////////////////////
/// Texto Número Conciliación      ///
//////////////////////////////////////
$("#ConciliationNumber").change(function () {
    $("#alertReverse").UifAlert('hide');
    // Se valida que ingrese el número de conciliación
    $("#tableReverseConciliation").UifDataTable();
    if ($("#ConciliationNumber").val() == "") {
        $("#alertReverse").UifAlert('show', Resources.ConciliationNumberEntry, "warning");
        return true;
    }

    var controller = ACC_ROOT + "BankReconciliation/GetConciliationSummaryByConciliationId?conciliationId=" +
                                        $("#ConciliationNumber").val();

    $("#tableReverseConciliation").UifDataTable({ source: controller });
});

//////////////////////////////////////
/// Fecha Reversión                ///
//////////////////////////////////////
$('#ReversalDate').on("dateChanged.datepicker", function (event, date) {
    if (IsDate($('#ReversalDate').val())) {
        $("#alertReverse").UifAlert('hide');
        if (ValidateReverseDate(conciliationDateReverse, $("#ReversalDate").val()) == false) {
            $("#alertReverse").UifAlert('show', Resources.ValidationReversalDate, "warning");
            return true;
        }
    }
});

//////////////////////////////////////
/// Tabla movimientos conciliados  ///
//////////////////////////////////////
$('#tableReverseConciliation').on('rowEdit', function (event, data, position) {
    $('#modalViewReverse').UifModal('showLocal', Resources.MovementDetails);

    var controller = ACC_ROOT + "BankReconciliation/GetConciliationDetailsByConciliationId?conciliationId=" +
                                 data.ConciliationId;

    $('#modalViewReverse').find("#tableDetails").UifDataTable({ source: controller });
});

//////////////////////////////////////
/// Botón Reversar                 ///
//////////////////////////////////////
$('#btnReverseConciliation').click(function () {
    $("#formReverseConciliation").validate();

    if ($("#formReverseConciliation").valid()) {
        var conciliations = $("#tableReverseConciliation").UifDataTable("getSelected");
        var count = 0;

        //Se valida que la fecha de reversión este dentro de una fecha contable
        $("#alertReverse").UifAlert('hide');
        if (ValidateReverseDate(conciliationDateReverse, $("#ReversalDate").val()) == false) {
            $("#alertReverse").UifAlert('show', Resources.ValidationReversalDate, "warning");
            return true;
        }

        // Se valida que ingrese el número de conciliación
        if ($("#ConciliationNumber").val() == "") {
            $("#alertReverse").UifAlert('show', Resources.ConciliationNumberEntry, "warning");
            return true;
        }

        if (conciliations != null) {
            count = conciliations.length;
        }

        if (count > 0) {
            //$('#modalReverse').appendTo("body").modal('show');
            $('#modalReverse').UifModal('showLocal', Resources.MessageReverseConciliationTitle);
        }
        else {
            $("#alertReverse").UifAlert('show', Resources.SelectReverseMovement, "warning");
        }
        return true;
    }

});

//////////////////////////////////////
/// Botón Cancelar                 ///
//////////////////////////////////////
$('#btnCancelReverse').click(function () {
    $("#tableReverseConciliation").dataTable().fnClearTable();
    $("#ConciliationNumber").val("");
});

////////////////////////////////////////////////////////////////////
/// Funciones                                                    ///
////////////////////////////////////////////////////////////////////
function LoadConciliationMovements() {
    if ($("#ConciliationNumber").val() != "") {
        var controller = ACC_ROOT + "BankReconciliation/GetConciliationSummaryByConciliationId?conciliationId=" +
                                            $("#ConciliationNumber").val();

        $("#tableReverseConciliation").UifDataTable({ source: controller });
    }
}