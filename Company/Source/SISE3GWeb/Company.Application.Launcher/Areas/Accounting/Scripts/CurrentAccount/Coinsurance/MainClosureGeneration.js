//////////////////////////////////////////////////////////////////////
/// Setea los valores para la generación del balance de comisiones ///
//////////////////////////////////////////////////////////////////////
function GetAccountingDate() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined
        && $("#ViewBagBillControlId").val() == undefined) {

        $.ajax({
            url: ACC_ROOT + 'Common/GetAccountingDate',
            success: function (data) {
                $("#GenerationYear").val(data.substring(6, 10));
                $("#GenerationMonth").val(data.substring(3, 5));
                $("#DateClosureGeneration").val(data);
            }
        });
    }
}



setTimeout(function () {
    GetAccountingDate();
}, 500);



//////////////////////////////////////////////////
// Botón generación del cierre                 ///
//////////////////////////////////////////////////
$("#AcceptClosureGeneration").click(function () {
    $("#formClosureGeneration").validate();
    $("#alertForm").UifAlert('hide');
    var day = $("#DateClosureGeneration").val().substring(0, 2);

    if ($("#formClosureGeneration").valid()) {
        // Ejecutar el proceso

        lockScreen();
        setTimeout(function () {    
            $.ajax({
            async: false,
            type: "POST",
            dataType: "json",
            url: ACC_ROOT + "Agent/ExecuteClosureGeneration",
            data: { "closureDate": $("#DateClosureGeneration").val() },
            success: function (data) {
                unlockScreen();
                if (data.success == false && data.result != undefined) {

                    $("#alertForm").UifAlert('show', data.result, "danger");
                    
                } else {
                    if (data == true) {
                        $("#alertForm").UifAlert('show', Resources.ClosureGenerationSuccessfully, "success");
                    } else {
                        $("#alertForm").UifAlert('show', Resources.WarningValidatePartialClousureCommissions, "warning");
                    }
                }
            }
            });
        }, 500);
    }
});
