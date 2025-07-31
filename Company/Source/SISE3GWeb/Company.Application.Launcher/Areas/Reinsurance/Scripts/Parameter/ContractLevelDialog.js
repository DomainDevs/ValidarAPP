var oContractLevel = {
    ContractLevelId: 0,
    LevelNumber: 0,
    ContractLimit: 0,
    Percentage: 0,
    RetentionLimit: 0,
    LinesNumber: 0,
    FullRetention: 0,
    EventLimit: 0,
    AdjustmentPercentage: 0,
    FixedRatePercentage: 0,
    MaximumRatePercentage: 0,
    MinimumRatePercentage: 0,
    LifeRate: 0,
    CalculationType: 0,
    ApplyOnType: 0,
    AnnualAddedLimit: 0,
    PremiumType: 0,
    Contract: {},
};

var oContract = {
    ContractId: 0,
    ContractType: {}
};

var oContractType = {
    ContractTypeId: 0
}

$("#modalContractLevel").on('modal.opened', function () {
    $("#ContractLimit").val(NotFormatMoney($("#ContractLimit").val()));
    $("#RetentionLimit").val(NotFormatMoney($("#RetentionLimit").val()));
    $("#EventLimit").val(NotFormatMoney($("#EventLimit").val()));
    $("#annualAddedLimit").val(NotFormatMoney($("#annualAddedLimit").val()));
    $("#ContractLimit").val(FormatMoney($("#ContractLimit").val()));
    $("#RetentionLimit").val(FormatMoney($("#RetentionLimit").val()));
    $("#EventLimit").val(FormatMoney($("#EventLimit").val()));
    $("#annualAddedLimit").val(FormatMoney($("#annualAddedLimit").val()));
    $("#LinesNumber").val(FormatMoney($("#LinesNumber").val()));
});

$('#modalContractLevel').on('change', '#ContractLimit ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevel').on('change', '#Percentage ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevel').on('change', '#RetentionLimit ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevel').on('change', '#EventLimit ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevel').on('change', '#LinesNumber ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevel').on('change', '#RetentionLimit ', function (event, selectedItem) {
    // Si el tipo de contrato es diferente de 1: Cuota parte, 3: Exceso de pérdida se calcula el límite de contrato
    validExcedenteContractLimit();
});

$('#modalContractLevel').on('change', '#LinesNumber ', function (event, selectedItem) {
    validExcedenteContractLimit();
});

$('#modalContractLevel').on('focusout', '#EventLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(FormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusin', '#LinesNumber ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val($(this).val().toString().replace(/\,/g, "."));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusout', '#LinesNumber ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(FormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusin', '#EventLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(NotFormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusout', '#ContractLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(FormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusin', '#ContractLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(NotFormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusout', '#RetentionLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(FormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusin', '#RetentionLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(NotFormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusout', '#annualAddedLimit ', function (event, selectedItem) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(FormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('focusin', '#annualAddedLimit ', function (event) {
    $("#alertLayer").UifAlert('hide');
    if (parseInt($(this).val()) > 0) {
        $(this).val(NotFormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalContractLevel').on('click', '#btnSaveContractLevel ', function (event, selectedItem) {
    //Grabación de Contrato de Nivel

    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }
    
    $('#formContractLevel').validate();
    if ($("#formContractLevel").valid()) {
        oContract.ContractId = $("#ContractCode").val();
        oContractLevel.Contract = oContract;
        oContractLevel.ContractLevelId = $("#ContractLevelId").val();
        oContractLevel.Number = $("#Number").val();
        oContractLevel.AssignmentPercentage = ReplaceDecimalPoint($("#Percentage").val());
        oContractLevel.RetentionLimit = NotFormatMoney($('#RetentionLimit').val());
        oContractLevel.ContractLimit = NotFormatMoney($('#ContractLimit').val());
        oContractLevel.EventLimit = NotFormatMoney($('#EventLimit').val());
        oContractLevel.LinesNumber = NotFormatMoney($("#LinesNumber").val());
        oContractLevel.FixedRatePercentage = ReplaceDecimalPoint($("#fixedRatePercentage").val());
        oContractLevel.MaximumRatePercentage = ReplaceDecimalPoint($("#maximumRatePercentage").val());
        oContractLevel.MinimumRatePercentage = ReplaceDecimalPoint($("#minimumRatePercentage").val());
        oContractLevel.AdjustmentPercentage = ReplaceDecimalPoint($("#adjustmentPercentage").val());
        oContractLevel.LifeRate = ReplaceDecimalPoint($("#lifeRate").val());
        oContractLevel.CalculationType = $("#calculationTypes").val();
        oContractLevel.ApplyOnType = $("#applyOnTypes").val();
        oContractLevel.AnnualAddedLimit = NotFormatMoney($('#annualAddedLimit').val());
        oContractLevel.PremiumType = $("#premiumTypes").val();
        oContractType.ContractTypeId = $("#ContractType").val();
        oContractLevel.Contract.ContractType = oContractType;

        $.ajax({
            async: false,
            type: "POST",
            url: REINS_ROOT + "Parameter/SaveContractLevel",
            data: JSON.stringify({ "contractLevelDTO": oContractLevel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                onAddContractLevelComplete(data);
            }
        });
    }
});

function validExcedenteContractLimit() {
    var contractLimit;

    if ($("#ContractType").val() == "2" || $("#ContractType").val() == "4") //validación para tipo de contrato excedente
    {
        if ($("#RetentionLimit").val() != '' && $("#LinesNumber").val() != '') {

            //Para Nivel 1: Límite de Retención x (Nro. Líneas+1)
            if ($("#LevelNumber").val() == 1) {
                contractLimit = parseFloat(NotFormatMoney($("#RetentionLimit").val())) * (parseFloat($("#LinesNumber").val()) + 1);
            }
            else {
                //Para el resto de niveles: Límite de Retención x Nro. Líneas
                contractLimit = parseFloat(NotFormatMoney($("#RetentionLimit").val())) * parseFloat($("#LinesNumber").val());
            }
            $("#ContractLimit").val(FormatMoney(contractLimit));
        }
    }
}
