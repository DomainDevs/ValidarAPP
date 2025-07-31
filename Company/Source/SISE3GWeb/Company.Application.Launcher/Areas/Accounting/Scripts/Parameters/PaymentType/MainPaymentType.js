$("#addPaymentType").find("#saveButton").click(function () {

    var paymentMethodTypeModel = {
        PaymentMethodTypeCode: 0,
        PaymentMethodTypeDescription: "",
        EnabledBilling: 0,
        EnabledPaymentOrder: 0,
        EnabledTicket: 0,
        EnabledPaymentRequest: 0
    };

    paymentMethodTypeModel.PaymentMethodTypeCode = $("#PaymentMethodTypeCode").val();
    paymentMethodTypeModel.PaymentMethodTypeDescription = $("#PaymentMethodTypeDescription").val();
    paymentMethodTypeModel.EnabledBilling = ($('#cashReceiptCheck').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    paymentMethodTypeModel.EnabledPaymentOrder = ($('#paymentOrderCheck').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    paymentMethodTypeModel.EnabledTicket = ($('#internalBallotCheck').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
    paymentMethodTypeModel.EnabledPaymentRequest = ($('#variousPaymentRequestCheck').hasClass('glyphicon glyphicon-check')) ? 1 : 0;

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "/Parameters/UpdatePaymentMethodType",
        data: JSON.stringify({ "model": paymentMethodTypeModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#addPaymentType").formReset();
            $('#modalPaymentType').UifModal('hide');
            $('#tblPaymentType').UifDataTable();

            $("#alertPaymentType").UifAlert('show', Resources.EditSuccessfully, "success");
        }
    });
});

$('#tblPaymentType').on('rowEdit', function (event, selectedRow) {
    $('#alertPaymentType').UifAlert('hide');
    $('#modalPaymentType').appendTo("body").UifModal('show', ACC_ROOT + "Parameters/PaymentTypeModal?id="
        + selectedRow.Id + "&description=" + selectedRow.Description + "&internalBallot="
        + selectedRow.InternalBallot + "&billing=" + selectedRow.Billing + "&paymentOrder="
        + selectedRow.PaymentOrder + "&paymentRequest=" + selectedRow.PaymentRequest,
        Resources.EditPaymentType);
});

$('#tblPaymentType').on('rowSelected', function (event, selectedRow) {
    $("#alertPaymentType").UifAlert('hide');
});

$('#tblPaymentType').on('rowSelected', function (event, selectedRow) {
    $("#alertPaymentType").UifAlert('hide');
});