/*-------------------------------------------------------------------------------------------------------*/
//          PARA QUE FUNCIONE CORRECTAMENTE CON EL LAUNCHER         
/*-------------------------------------------------------------------------------------------------------*/
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

var oContractType = {
    ContractTypeId: 0
};

var oEpiType = {
    Id: 0
};

var oAffectationType = {
    Id: 0
};

var oResettlementType = {
    Id: 0
};

var oCurrency = {
    CurrencyId: 0
}

var timerLoadContractType = 0;
var count = 0;

function validateDuplicateContract(oContract) {
    $.ajax({
        async: false,
        type: "POST",
        url: REINS_ROOT + "Parameter/ValidateDuplicateContract",
        data: JSON.stringify({ "contractDTO": oContract }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                if (!data.result) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: REINS_ROOT + "Parameter/SaveContract",
                        data: JSON.stringify({ "contract": oContract }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            onAddCompleteContract(data);
                        }
                    });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.ContractDescriptionDuplicate, 'autoclose': true });
                }
            }
        }
    });
}

function disableContractType() {
    if ($('select[Id="cmbContractType"]')[0].options.length > 0) {
        $("#cmbContractType").attr("disabled", true);
        clearInterval(timerLoadContractType);
    }
}

function manageControls(option) {
    //Funcionalidad 2 PROPORCIONAL
    if (option == "2") {
        $("#rowContractType1").show(); // div row1
        $("#colRisk").hide();// col1 row1  cantidad de riesgo /vidas afectadas
        $("#colCoinsurance").show(); //col1 row1 % capacidad coaseguro
        $("#rowContractTypePrimary1").hide(); // div row2.1
        $("#rowContractTypePrimary2").hide(); // div row2.2
        $("#colMonthReserve").show();
    }
    else if (option == "3" || option == "4") {
        //Funcionalidad 3 no PROPORCIONAL o catastrofico 
        // exeso de perdida 
        $("#rowContractType1").show();
        $("#colRisk").show();
        $("#colCoinsurance").hide();
        $("#rowContractTypePrimary1").show(); // div row2.1
        $("#rowContractTypePrimary2").show(); // div row2.2
        $("#colMonthReserve").hide();

    }
    else {
        // otro tipos de funcionalidad  
        $("#rowContractType1").hide();
        $("#rowContractTypePrimary1").hide(); // div row2.1
        $("#rowContractTypePrimary2").hide(); // div row2.2 
        $("#colMonthReserve").show();
    }
}

function calculateDepositPremiumAmount() {
    if ($("#epiTypes").val() == 1) {
        var premiumAmount = parseFloat(NotFormatMoney($.trim($("#premiumAmount").val())));
        var depositPercentageAmount = $.trim($("#depositPercentageAmount").val().replace(",", "."));
        var depositPremiumAmount = (premiumAmount * depositPercentageAmount) / 100;
        if (depositPercentageAmount > 0) {
            FormatMoney($("#depositPremiumAmount").val(depositPremiumAmount));
        } else {
            FormatMoney($("#depositPremiumAmount").val(0));
        }
    }

    if ($("#depositPremiumAmount").val() != "") {
        var depositPremiumAmount = NotFormatMoney($.trim($("#depositPremiumAmount").val()));
        $("#depositPremiumAmount").val(FormatMoney(depositPremiumAmount));
    }
}

function showFields() {
    if ($("#epiTypes").val() == "1") {  // Prima Neta Retenida
        $("#premiumAmount").prop("disabled", false);
        $("#depositPercentageAmount").prop("disabled", false);
        $("#depositPremiumAmount").prop("disabled", true);
        $("#premiumAmountLabel").addClass("field-required");
        $("#DepositPercentageAmountLabel").addClass("field-required");

    }
    else {
        $("#premiumAmount").prop("disabled", true);
        $("#depositPercentageAmount").prop("disabled", true);
        $("#depositPremiumAmount").prop("disabled", false);
        $("#premiumDepositLabel").addClass("field-required");
        $("#depositPremiumAmount").prop("required", true);
        $("#premiumAmountLabel").removeClass("field-required");
        $("#DepositPercentageAmountLabel").removeClass("field-required");
    }
}

function cleanEstimatedPremium() {
    $("#premiumAmount").val(0);
    $("#depositPercentageAmount").val(0)
    $("#depositPremiumAmount").val(0)
}

$('#modalContract').on('change', '#CoInsurancePercentage ', function (event) {
    $('#CoInsurancePercentage').OnlyDecimals(2);
    $('#CoInsurancePercentage').on('focusin', this.inputMoneyOnFocusin);
    $('#CoInsurancePercentage').focusout(this.assignZero);
    $('#CoInsurancePercentage').focusout(function () {
        var value = NotFormatMoney($.trim($(this).val()));
        value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
        var text = $(this).val().split(',');
        if (text.length == 2) {
            if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                $(this).val(0);
            }
        } else {
            if (parseInt(text[0]) > 100) {
                $(this).val(0);
            }
        }
    });
});

$('#modalContract').on('itemSelected', '#cmbContractType ', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    $.ajax({
        async: false,
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractFuncionalityId",
        data: JSON.stringify({ "contractTypeId": selectedItem.Id }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var opcion = data.data[0].ContractFunctionality.ContractFunctionalityId;
            manageControls(opcion);
        }
    });
});

$('#modalContract').on('change', '#CurrencyId ', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    count = count + 1;
    if (parseInt($("#ContractId").val()) > 0) {
        if (count > 1) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.MsjContractModified, 'autoclose': true });
        }
    }
});

$('#modalContract').on('blur', '#premiumAmount ', function (event) {
    $("#alert").UifAlert('hide');
    if ($.trim($("#premiumAmount").val()) != "") {
        var premiumAmount = NotFormatMoney($.trim($("#premiumAmount").val()));
        $("#premiumAmount").val($.trim(FormatMoney(premiumAmount)));
    }
    else {
        $("#premiumAmount").val("0");
    }
});

$('#modalContract').on('click', '#checkstatus ', function (event) {
    $("#alert").UifAlert('hide');
    if ($("#status").attr("disabled") == undefined) {
        if ($("#checkstatus").hasClass("glyphicon glyphicon-unchecked")) {
            $("#checkstatus").removeClass("glyphicon glyphicon-unchecked");
            $("#checkstatus").addClass("glyphicon glyphicon-check");
        }
        else if ($("#checkstatus").hasClass("glyphicon glyphicon-check")) {
            $("#checkstatus").removeClass("glyphicon glyphicon-check");
            $("#checkstatus").addClass("glyphicon glyphicon-unchecked");
        }
    }
});

$('#modalContract').on('modal.opened', function () {
    $('#epiTypes').on('binded', function (event, data, index) {
        showFields();
    });
    $('#epiTypes').UifSelect();
    var premiumAmount = FormatMoney($("#premiumAmount").val());
    var depositPremiumAmount = FormatMoney($("#depositPremiumAmount").val());
    $("#premiumAmount").val(premiumAmount);
    $("#depositPremiumAmount").val(depositPremiumAmount);
});

$('#modalContract').on('change', '#epiTypes', function (event, selectedItem) {
    showFields();
    cleanEstimatedPremium();
});

$('#modalContract').on('change', '#premiumAmount', function (event, selectedItem) {
    calculateDepositPremiumAmount();
});

$('#modalContract').on('change', '#depositPercentageAmount ', function (event) {
    $('#depositPercentageAmount').OnlyDecimals(2);
    $('#depositPercentageAmount').on('focusin', this.inputMoneyOnFocusin);
    $('#depositPercentageAmount').focusout(this.assignZero);
    $('#depositPercentageAmount').focusout(function () {
        var value = NotFormatMoney($.trim($(this).val()));
        value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
        var text = $(this).val().split(',');
        if (text.length == 2) {
            if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                $(this).val(0);
            } else {
                calculateDepositPremiumAmount();
            }
        } else {
            if (parseInt(text[0]) > 100) {
                $(this).val(0);
                FormatMoney($("#depositPremiumAmount").val(0));
            } else {
                calculateDepositPremiumAmount();
            }
        }
    });
});

$('#modalContract').on('blur', '#depositPremiumAmount ', function (event) {
    $("#alert").UifAlert('hide');
    if ($.trim($("#depositPremiumAmount").val()) != "") {
        var depositPremiumAmount = NotFormatMoney($.trim($("#depositPremiumAmount").val()));
        $("#depositPremiumAmount").val($.trim(FormatMoney(depositPremiumAmount)));
    }
    else {
        $("#depositPremiumAmount").val("0");
    }

});

$('#modalContract').on('click', '#btnSaveContract ', function (event, selectedItem) {

    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }

    var depositPremiumAmount = parseFloat(NotFormatMoney($("#depositPremiumAmount").val()))
    $("#depositPremiumAmount").val(depositPremiumAmount);

    var premiumAmount = parseFloat(NotFormatMoney($("#premiumAmount").val()));
    $("#premiumAmount").val(premiumAmount);

    var depositPercentageAmount = parseFloat($("#depositPercentageAmount").val().replace(",", "."));
    $("#depositPercentageAmount").val(depositPercentageAmount);
    $("#CoInsurancePercentage").val(parseFloat($("#CoInsurancePercentage").val().replace(",", ".")));

    if (parseFloat($("#CoInsurancePercentage").val()) == 100) {
        $("#CoInsurancePercentage").val(100)
    }

    $("#formContract").validate()
    if ($("#formContract").valid()) {
        oContractType.ContractTypeId = $("#cmbContractType").val();

        if ($("#epiTypes").val() == "") {
            oEpiType.Id = 0;
        }
        else {
            oEpiType.Id = $("#epiTypes").val();
        }

        if ($("#affectationTypes").val() == "") {
            oAffectationType.Id = 0;
        }
        else {
            oAffectationType.Id = $("#affectationTypes").val();
        }

        if ($("#reestablishmentTypes").val() == "") {
            oResettlementType.Id = 0;
        }
        else {
            oResettlementType.Id = $("#reestablishmentTypes").val();
        }

        if ($("#CurrencyId").val() == "") {
            oCurrency.CurrencyId = -1
        } else {
            oCurrency.CurrencyId = $("#CurrencyId").val();
        }

        //ARMA EL OBJETO
        oContract.ContractId = $("#ContractId").val();
        oContract.SmallDescription = $("#SmallDescription").val();
        oContract.Description = $("#Description").val();
        oContract.Year = $("#Year").val();
        oContract.DateFrom = $("#DateFrom").val();
        oContract.DateTo = $("#DateTo").val();
        oContract.Currency = oCurrency;
        oContract.ReleaseTimeReserve = $("#ReleaseTimeReserve").val();
        oContract.ContractType = oContractType;
        oContract.AffectationType = oAffectationType;
        oContract.ResettlementType = oResettlementType;
        oContract.PremiumAmount = $("#premiumAmount").val();
        oContract.EpiType = oEpiType;
        oContract.DepositPercentageAmount = $("#depositPercentageAmount").val();
        oContract.DepositPremiumAmount = $("#depositPremiumAmount").val();

        var stausValue = ($('#checkstatus').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
        oContract.Enabled = stausValue;

        if (($("#grouper").val() == "") || ($("#grouper").val() == null)) {
            oContract.GroupContract = $("#SmallDescription").val();
        }
        else {
            oContract.GroupContract = $("#grouper").UifSelect("getSelectedText");
        }

        oContract.CoInsurancePercentage = parseFloat($("#CoInsurancePercentage").val());
        oContract.RisksNumber = parseInt($("#quantityRisk").val());

        if ($("#EstimatedDate").val() == "") {

            oContract.EstimatedDate = null;
        }
        else {
            oContract.EstimatedDate = $("#EstimatedDate").val();
        }

        validateDuplicateContract(oContract);
    }

});

$(document).ready(function () {
    $('#modalContract').on('modal.opened', function () {
        timerLoadContractType = window.setInterval(disableContractType, 100);
    });
});



