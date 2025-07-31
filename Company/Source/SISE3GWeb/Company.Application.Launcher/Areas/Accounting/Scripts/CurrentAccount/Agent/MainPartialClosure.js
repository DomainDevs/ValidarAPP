/*--------------------------------------------------------------------------------------------------------------------------------------*/
/*                                              DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                              */
/*--------------------------------------------------------------------------------------------------------------------------------------*/
var year = $("#ViewBagYear").val();
var month = $("#ViewBagMonth").val();
var day = $("#ViewBagDay").val();

/*--------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                   DEFINICIÓN DE FUNCIONES GLOBALES                                                   */
/*--------------------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////
/// Obtiene la fecha contable     ///
/////////////////////////////////////
function GetAccountingDateClosure() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {
        $.ajax({
            url: ACC_ROOT + "Common/GetAccountingDate",
            success: function (data) {
                $("#DatePartialClosure").val(data);
            }
        });
    }
}


function CleanFieldsPartialClosure() {
    $("#DatePartialClosure").val("");
};


///////////////////////////////////////
/// Fecha cierre parcial comisiones ///
///////////////////////////////////////
$('#DatePartialClosure').on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (!IsDate($('#DatePartialClosure').val())) {
        $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
    }
});


/////////////////////////////////////////////////
// Botón generar cierre parcial de comisiones ///
/////////////////////////////////////////////////
$("#AcceptPartialClosure").click(function () {
    $("#formPartialClosure").validate();
    $("#alertForm").UifAlert('hide');

    if ($("#formPartialClosure").valid()) {

        if (IsDate($("#DatePartialClosure").val()) == true) {
            //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });
            lockScreen();

            day = $("#DatePartialClosure").val().substring(0, 2);
            month = $("#DatePartialClosure").val().substring(3, 5);
            year = $("#DatePartialClosure").val().substring(6, 10);

            // Ejecutar el proceso
            $.ajax({
                url: ACC_ROOT + 'Agent/SavePartialClousureAgentsRequest',
                data: { "year": year, "month": month, "day": day }
            }).done(function (data) {
                if (data.success == false) {
                    $("#alertForm").UifAlert('show', data.result, "danger");
                } else {

                    if (data.result == true) {
                        $("#alertForm").UifAlert('show', Resources.CloseGeneratedSuccessfully, "success");
                    }
                    else {
                        $("#alertForm").UifAlert('show', Resources.WarningValidatePartialClousureCommissions, "warning");
                    }
                }
                unlockScreen();
            });
        }
        else {
            $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
        }
    }
});


/////////////////////////////////////////////////////////////
// Botón cancelar generación cierre parcial de comisiones ///
/////////////////////////////////////////////////////////////
$("#CancelPartialClosure").click(function (e) {
    CleanFieldsPartialClosure();
});

$(document).ready(function () {
    GetAccountingDateClosure();
});