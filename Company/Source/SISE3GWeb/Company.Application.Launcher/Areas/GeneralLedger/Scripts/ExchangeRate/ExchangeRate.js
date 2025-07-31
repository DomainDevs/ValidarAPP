///////////////////////////////////////////////////////////////////////////*VARIABLES*///////////////////////////////////////////////////////////////////////////
var dateExchangeRate = null;
var currencyId = 0;
var sellAmountEdit = 0;
var buyAmountEdit = 0;

///////////////////////////////////////////////////////////////////////////*EVENTOS TABLA*///////////////////////////////////////////////////////////////////////////
/*tableRange*/
$('#tableExchangeRate').on('rowAdd', function (event, data) {
    $('#exchangeRateParametrizationAlert').UifAlert('hide');
    $('#exchangeRateParametrizationAddModal').UifModal('showLocal', Resources.AddExchangeRate);
    $("#RateDate").UifDatepicker('setValue', getDate());
    $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('hide');
    $("#exchangeRateAdd").find("#SellAmount").val("");
    $("#exchangeRateAdd").find("#BuyAmount").val("");
    $("#exchangeRateAdd").find("#ExchangeRateCurrency").val("");
});

$('#tableExchangeRate').on('rowEdit', function (event, data, position) {
    $("#exchangeRateParametrizationAlert").UifAlert('hide');
    $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('hide');
    $('#exchangeRateParametrizationModalEditField').UifModal('showLocal', Resources.EditExchangeRate);
    $("#exchangeRateEditFieldForm").find("#RateDate").prop("disabled", true);
    $("#exchangeRateEditFieldForm").find("#ExchangeRateCurrency").prop("disabled", true);

    $("#exchangeRateEditFieldForm").find("#RateDate").val(data.RateDate);
    $("#exchangeRateEditFieldForm").find("#ExchangeRateCurrency").val(data.CurrencyCode);    
    $("#exchangeRateEditFieldForm").find("#SellAmount").val(data.SellAmount);
    $("#exchangeRateEditFieldForm").find("#BuyAmount").val(data.BuyAmount);
    sellAmountEdit = data.SellAmount;
    buyAmountEdit = data.BuyAmount;
});

$('#tableExchangeRate').on('rowDelete', function (event, data) {
    $('#exchangeRateParametrizationAlert').UifAlert('hide');

    dateExchangeRate = data.RateDate;
    currencyId = data.CurrencyCode;
    $('#exchangeRateDeleteModal').appendTo("body").modal('show');
});

///////////////////////////////////////////////////////////////////////////*EVENTOS BOTONES*///////////////////////////////////////////////////////////////////////////
/*Grabar Nuevo registro*/
$('#exchangeRateParametrizationSaveAddField').click(function () {
    if ($("#exchangeRateAdd").valid()) {

        SaveExchangeRate();

        $("#exchangeRateAdd").formReset();
        $('#exchangeRateParametrizationAddModal').UifModal('hide');
    } else {
        $("#alertExchangeRate").UifAlert('hide');
    }
});


/*Actualiza Registro*/
$('#exchangeRateParametrizationSaveEditField').click(function () {
    if ($("#exchangeRateEditFieldForm").valid()) {
           
            UpdateExchangeRate();

            $("#exchangeRateEditFieldForm").formReset();
            $('#exchangeRateParametrizationModalEditField').UifModal('hide');
    }

});

//confirmacion de eliminacion
$("#btnDeleteModal").on('click', function () {
    $("#exchangeRateParametrizationAlert").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: GL_ROOT + "ExchangeRate/DeleteExchangeRates",
        data: JSON.stringify({ "rateDate": dateExchangeRate, "currencyId": currencyId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data) {
            $("#exchangeRateParametrizationAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
            var controller = GL_ROOT + "ExchangeRate/GetExchangeRate";
            $("#tableExchangeRate").UifDataTable({ source: controller });
        }
        else {
            $("#exchangeRateParametrizationAlert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "danger");
        }
        $('#exchangeRateDeleteModal').UifModal('hide');
    });

});

///////////////////////////////////////////////////////////////////////////*FUNCIONES*///////////////////////////////////////////////////////////////////////////

function SaveExchangeRate() {
    var resultDataExchangeRateSave = dataExchangeRateSave("#exchangeRateAdd");
    $.ajax({
        type: "POST",
        url: GL_ROOT + "ExchangeRate/SaveExchangeRate",
        data: JSON.stringify({ "oExchangeRate": resultDataExchangeRateSave }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success == false) {
                if (data.exceptionType == "danger") {
                    $("#exchangeRateParametrizationAlert").UifAlert('show', data.result, "danger");
                }
                else {
                    $("#exchangeRateParametrizationAlert").UifAlert('show', data.result, "warning");
                }
            }
            else {

                var controller = GL_ROOT + "ExchangeRate/GetExchangeRate";
                $("#tableExchangeRate").UifDataTable({ source: controller });
                $("#exchangeRateParametrizationAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
            }
        }
    });
}


function dataExchangeRateSave(exchangeRateModal) {
    var BuyAmount = {
        Value: ClearFormatCurrency($(exchangeRateModal).find("#BuyAmount").val())
    }
    var Currency = {
        Id: $(exchangeRateModal).find("#ExchangeRateCurrency").val()
    }
    var SellAmount = {
        Value: ClearFormatCurrency($(exchangeRateModal).find("#SellAmount").val())
    }
    return {
        BuyAmount: BuyAmount,
        Currency: Currency,
        RateDate: $(exchangeRateModal).find("#RateDate").val(),
        SellAmount: SellAmount
    }

}

function UpdateExchangeRate() {
    $.ajax({
        type: "POST",
        url: GL_ROOT + "ExchangeRate/UpdateExchangeRate",
        data: JSON.stringify({ "oExchangeRate": dataExchangeRateSave("#exchangeRateEditFieldForm") }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == false) {
                $("#exchangeRateParametrizationAlert").UifAlert('show', Resources.SaveError, "danger");
            }
            else {

                $("#exchangeRateParametrizationAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = GL_ROOT + "ExchangeRate/GetExchangeRate";
                $("#tableExchangeRate").UifDataTable({ source: controller });
            }
        }
    });
}

///////////////////////////////////////////////////
//  Obtiene fecha del servidor                   //
//////////////////////////////////////////////////
function getDate() {
    var systemDate;
    $.ajax({
        type: "GET",
        async: false,
        url: ACC_ROOT + "Common/GetDate",
        success: function (data) {
            systemDate = data;
        }
    });

    return systemDate;
}

//////////////////////////////////////////////////////////
/// Función que permite números con cuatro decimales ///
//////////////////////////////////////////////////////////
function OnlyNumberFourDecimal(e, field) {

    key = e.keyCode ? e.keyCode : e.which

    if (key == 46) {
        if (field.value == "") return false
        regexp = /^[0-9]+$/
        return regexp.test(field.value)
    }

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida cuatro digitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") return true

                regexp = /[0-9]{4}$/
                return !(regexp.test(field.value))
            }
        }
    }

    // backspace
    if (key == 8 || (key > 47 && key < 58)) {
        if (field.value == "") return true
        regexp = /[0-9]{18}/

        return !(regexp.test(field.value))
    }

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida cuatro digitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") return true

                regexp = /[0-9]{4}$/
                return !(regexp.test(field.value))
            }
        }
    }

    return false;
}

//Cuenta el numero de digitos de un numero
function validateDigit(number) {
    var count = 0;
    var tempNumber = number;

    while (tempNumber > 0) {
        tempNumber = tempNumber / 10;
        tempNumber = Math.trunc(tempNumber);
        count++;
    }
    return count;
}

//Valida que el valor ingresado sea un número
function validateIsNumber(value) {       
    var expression = /^\d*\.?\d*$/;
    if (expression.test(value)) {
        return true;
    } else {
        return false;
    }
}
///////////////////////////////////////////////////////////////////////////*EVENTOS CAMPOS*///////////////////////////////////////////////////////////////////////////

//formato en decimal campo SellAmount - Agregar
$("#exchangeRateParametrizationAddModal").find("#SellAmount").change(function () {
    if ($("#SellAmount").val() != "") {
        var sellAmount = $("#SellAmount").val();
        var isNumber = validateIsNumber(sellAmount);
        
        if (isNumber) {
            if (sellAmount > 0) {

                var numberDigit = validateDigit(Math.trunc(sellAmount));
                if (numberDigit <= 4) {

                    $("#SellAmount").val("$ " + NumberFormatSearch(sellAmount, "4", ".", ","));
                    $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('hide');

                } else {

                    $("#SellAmount").val("");
                    $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('show', Resources.RestrictionAmountExchangeRate, "danger");
                }


            } else {
                
                $("#SellAmount").val("");
                $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('show', Resources.ValidateAmountExchangeRate, "danger");
            }
        } else {            
            $("#SellAmount").val("");
        }
    }
});

//formato en decimal campo BuyAmount - Agregar
$("#exchangeRateParametrizationAddModal").find("#BuyAmount").change(function () {
    if ($("#BuyAmount").val() != "") {
        var buyAmount = $("#BuyAmount").val();
        var isNumber = validateIsNumber(buyAmount);

        if (isNumber) {
            if (buyAmount > 0) {

                var numberDigit = validateDigit(Math.trunc(buyAmount));
                if (numberDigit <= 4) {

                    $("#BuyAmount").val("$ " + NumberFormatSearch(buyAmount, "4", ".", ","));
                    $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('hide');

                } else {

                    $("#BuyAmount").val("");
                    $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('show', Resources.RestrictionAmountExchangeRate, "danger");
                }


            } else {
                
                $("#BuyAmount").val("");
                $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('show', Resources.ValidateAmountExchangeRate, "danger");
            }
        } else {
            $("#BuyAmount").val("");           
        }
    }
});



//formato en decimal campo SellAmount - Editar
$("#exchangeRateParametrizationModalEditField").find("#SellAmount").change(function () {
    if ($("#exchangeRateEditFieldForm").find("#SellAmount").val() != "") {
        var sellAmount = ClearFormatCurrency($("#exchangeRateEditFieldForm").find("#SellAmount").val());

        if (sellAmount != " " && sellAmount != "") {
            if (sellAmount > 0) {
                var numberDigit = validateDigit(Math.trunc(sellAmount));

                if (numberDigit <= 4) {

                    $("#exchangeRateEditFieldForm").find("#SellAmount").val("$ " + NumberFormatSearch(sellAmount, "4", ".", ","));
                    $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('hide');

                } else {

                    $("#exchangeRateEditFieldForm").find("#SellAmount").val("");
                    $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('show', Resources.RestrictionAmountExchangeRate, "danger");

                }
            } else {
                
                $("#exchangeRateEditFieldForm").find("#SellAmount").val("");
                $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('show', Resources.ValidateAmountExchangeRate, "danger");
            }
        } else {
            
            $("#exchangeRateEditFieldForm").find("#SellAmount").val("");            
        }
    }
});


//formato en decimal campo BuyAmount - Editar    
$("#exchangeRateParametrizationModalEditField").find("#BuyAmount").change(function () {
    if ($("#exchangeRateEditFieldForm").find("#BuyAmount").val() != "") {

        var buyAmount = ClearFormatCurrency($("#exchangeRateEditFieldForm").find("#BuyAmount").val());       

        if (buyAmount != " " && buyAmount != "") {
            if (buyAmount > 0) {
                var numberDigit = validateDigit(Math.trunc(buyAmount));
                if (numberDigit <= 4) {

                    $("#exchangeRateEditFieldForm").find("#BuyAmount").val("$ " + NumberFormatSearch(buyAmount, "4", ".", ","));
                    $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('hide');

                } else {

                    $("#exchangeRateEditFieldForm").find("#BuyAmount").val("");
                    $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('show', Resources.RestrictionAmountExchangeRate, "danger");
                }
            } else {
                
                $("#exchangeRateEditFieldForm").find("#BuyAmount").val("");
                $("#exchangeRateParametrizationModalEditField").find("#alertExchangeRate").UifAlert('show', Resources.ValidateAmountExchangeRate, "danger");

            }
        } else {

            $("#exchangeRateEditFieldForm").find("#BuyAmount").val("");
        }

    }
});



// Valida que no ingresen una fecha invalida.
$("#RateDate").change(function () {

    if ($("#RateDate").val() != '') {

        if (!IsDate($("#RateDate").val())) {
            $("#RateDate").val("");
            $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('show', Resources.InvalidDates, "danger");
        } else {
            $("#exchangeRateParametrizationAddModal").find("#alertExchangeRate").UifAlert('hide');
        }
    }
});




