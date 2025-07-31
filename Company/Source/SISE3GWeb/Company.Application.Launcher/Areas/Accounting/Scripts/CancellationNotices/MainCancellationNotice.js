/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var branchId = 0;
var agentType = 0;
var agentCode = 0;

/*----------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*----------------------------------------------------------------------------------------------------------------------------------------*/
        
if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchCancelation").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#BranchCancelation").removeAttr("disabled");
}

$('#AcceptCancelation').click(function () {
    $("#alertCancelation").UifAlert('hide');
    $("#FormularioCancelation").validate();
    if ($("#FormularioCancelation").valid()) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "CancellationNotices/LoadCancellationNoticeReport",
            data: { "branchCode": $("#BranchCancelation").val(), "prefixCode": $("#PrefixCancelation").val(), "policyNumber": $("#policyNumber").val(), "sufix": $("#sufix").val(), "endorsmentNumber": $("#endorsment").val() },
            success: function (data) {
                if (data == false) {
                    $("#alertCancelation").UifAlert('show', Resources.MovementsTypeValidation, "warning");
                }
                else {
                    showCoinsuranceReport();
                    $("#alertCancelation").UifAlert('show', Resources.SuccessfullPrinting, "success");
                }
            }
        });
    }
});

$('#CancelCancelation').click(function () {
    $("#alertCancelation").UifAlert('hide');
    $("#BranchCancelation").val("");
    $("#PrefixCancelation").val("");
    $("#policyNumber").val("");
    $("#sufix").val("");
    $("#endorsment").val("");
});

/*---------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------*/

function showCoinsuranceReport() {
    var controller = ACC_ROOT + "CancellationNotices/ShowCancellationNoticeReport";
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}