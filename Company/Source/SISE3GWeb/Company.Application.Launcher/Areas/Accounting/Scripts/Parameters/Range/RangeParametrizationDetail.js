var rangeParametrizationRangeId = 0;
var headerDetail = "";
var rangeParametrizationIsDefault = false;
var modalAdd = "#rangeParametrizationModalAddRangeDetail";
var mnodalEdit = "#rangeParametrizationModalEditField";
var checkType = '#checkDefault';
var editCheckType = $("#rangeParametrizationEditFieldForm").find('#checkDefault');

var oRangeControl = {
    Id: 0,
    Description: 0,
    IsDefault: false,
    RangeItems: []
};

var RangeItem = {
    Id: 0,
    Order: 0,
    RangeFrom: 0,
    RangeTo: 0,
};

var rangeParamItHasDefault = false;
var tableRangeDetailId = 0;
var countCheck = 0;
var containsItems = false;

/*tableRange*/
$('#tableRange').on('rowAdd', function (event, data) {
    $("#alertRangeHeader").UifAlert('hide');

    $("#rangeParametrizationModalAddField").find("#checkDefault").show();
    $("#rangeParametrizationEditFieldForm").find("#checkDefault").prop("checked", false);
    rangeParamItHasDefault = false;

    $("#rangeParametrizationAddFieldForm").find("#ColumnNumber").val(1);
    $("#rangeParametrizationAddFieldForm").find("#ColumnNumber").prop("disabled",true);

    $("#rangeParametrizationAddFieldForm").find("#InitialDay").val(1);
    $('#rangeParametrizationModalAddField').UifModal('showLocal', Resources.AddRange);
});

$('#tableRange').on('rowSelected', function (event, selectedRow) {
    $("#alertRangeHeader").UifAlert('hide');
    $("#rangeParametrizationAlert").UifAlert('hide');
    rangeParametrizationRangeId = selectedRow.Id;
    rangeParametrizationIsDefault = selectedRow.Default;

    var controller = ACC_ROOT + "Parameters/GetRangesItem?rangeId=" + selectedRow.Id;
    $("#tableRangeDetail").UifDataTable({ source: controller });   
});

$('#tableRange').on('rowEdit', function (event, data, position) {
    $("#alertRangeHeader").UifAlert('hide');
    $("#rangeParametrizationEditFieldForm").find('#alertRange').UifAlert('hide');
    var isDefaultTable = false;
    rangeParametrizationRangeId = data.Id;
    var itemsRange = queryRangeItems(rangeParametrizationRangeId);
    if (!itemsRange) {
        $("#rangeParametrizationEditFieldForm").find('#checkDefaultDiv').hide();        
        $("#rangeParametrizationEditFieldForm").find('#alertRange').UifAlert('show', Resources.WarningDetailDefaultRange,"warning");
    } else {
        $("#rangeParametrizationEditFieldForm").find("#checkDefaultDiv").show();
    }
    $("#rangeParametrizationEditFieldForm").find("#ColumnNumber").hide();
    $("#rangeParametrizationEditFieldForm").find("#InitialDay").hide();
    $("#rangeParametrizationEditFieldForm").find("#EndDay").hide();
    $("#rangeParametrizationEditFieldForm").find("#lblOrden").hide();
    $("#rangeParametrizationEditFieldForm").find("#lblDiasDesde").hide();
    $("#rangeParametrizationEditFieldForm").find("#lblDiasHasta").hide();

    $("#rangeParametrizationEditFieldForm").find("#Description").val(data.Description);

    if (data.Default == true) {
        
        setTimeout(function () {
            if (!$("#rangeParametrizationEditFieldForm").find("#checkDefault").hasClass('glyphicon glyphicon-check')) {
                $("#rangeParametrizationEditFieldForm").find("#checkDefault").removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
            }
        }, 250);
    }
    else {

        setTimeout(function () {
            if ($("#rangeParametrizationEditFieldForm").find("#checkDefault").hasClass('glyphicon glyphicon-check')) {
                $("#rangeParametrizationEditFieldForm").find("#checkDefault").removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
            }
        }, 250);
    }

    $('#rangeParametrizationModalEditField').UifModal('showLocal', Resources.EditNetwork);
});

$('#tableRange').on('rowDelete', function (event, data) {
    $('#rangeParametrizationAlert').UifAlert('hide');
    headerDetail = "Header";
    rangeParametrizationRangeId = data.Id;
    $('#rangeParametrizationModalDeleteRange').appendTo("body").modal('show');
});

/*tableRangeDetail*/
$('#tableRangeDetail').on('rowAdd', function (event, data) {

    if (rangeParametrizationRangeId == null || rangeParametrizationRangeId == undefined
        || rangeParametrizationRangeId == 0) {
        $.UifNotify('show', { type: 'danger', message: Resources.ErrorBeforeAddRangeDetails, autoclose: true });
        return;
    }

    $("#rangeParametrizationAddFieldForm").find("#ColumnNumber").prop("disabled", false);

    var fields = $('#tableRangeDetail').UifDataTable('getData');
    var columnId = 0;
    var start = 0;
    var length = 0;
    var limitedRange = $("#ViewBagLimitedRange").val();
    if (rangeParametrizationRangeId > 0) {
        for (var j = 0; j < fields.length; j++) {
            columnId = fields[j].ColumnNumber;
            start = fields[j].DayFrom;
            length = fields[j].DayTo;
        }

        if (start == 0 && length == 0) {
            start = 1;
        }

        if (columnId == $("#ViewBagLimitedRange").val()) {
            $("#rangeParametrizationAlert").UifAlert('show', 'Limte maximo de Rangos 5', "warning");
        }
        else {
            $("#rangeParametrizationAddRangeDetailForm").find("#Order").val(columnId + 1);
            $("#rangeParametrizationAddRangeDetailForm").find("#RangeFrom").val(length + 1);

            $("#rangeParametrizationAddRangeDetailForm").find("#Order").prop("disabled", true);
            $("#rangeParametrizationAddRangeDetailForm").find("#RangeFrom").prop("disabled", true);
            $('#rangeParametrizationModalAddRangeDetail').UifModal('showLocal', Resources.AddRangeDetail);
        }
    }
    else {
        $("#rangeParametrizationAlert").UifAlert('show', 'Seleccione un Rango', "warning");
    }

});

$('#tableRangeDetail').on('rowDelete', function (event, data) {

    headerDetail = "Detail";
    var ValidateDeleteFormatDetails = $('#tableRangeDetail').UifDataTable('getData');
    var maxLengh = ValidateDeleteFormatDetails.length - 1;

    if (ValidateDeleteFormatDetails[maxLengh].ColumnNumber == data.ColumnNumber) {
        $('#rangeParametrizationAlert').UifAlert('hide');
        cleanArrayRange();
        RangeItem.Id = data.Id,
            RangeItem.Order = data.ColumnNumber

        $('#rangeParametrizationModalDeleteRange').appendTo("body").modal('show');

    }
    else {
        $("#rangeParametrizationAlert").UifAlert('show', Resources.DoNotEliminateDetailsLitter, "warning");
    }
});


////////////////////////////////////////////////////////
/*Validaciones de rangos Varios Modale*/
////////////////////////////////////////////////////////
$('#rangeValueEditModal').find("#RangeFrom").blur(function () {
    $('#rangeValueEditModal').find("#alertFieldForm").UifAlert('hide');
    var result = true;
    var RangeFrom = $('#rangeValueEditModal').find("#RangeFrom").val() == "" ? 0 : $('#rangeValueEditModal').find("#RangeFrom").val();
    var rangeTo = $('#rangeValueEditModal').find("#RangeTo").val();
    if (parseInt(rangeTo) <= parseInt(RangeFrom)) {
        $('#rangeValueEditModal').find("#alertFieldForm").UifAlert('show', Resources.MessageValidateRangeFrom, "warning");
    }
});
$('#rangeValueEditModal').find("#RangeTo").blur(function () {
    $('#rangeValueEditModal').find("#alertFieldForm").UifAlert('hide');
    var result = true;
    var RangeFrom = $('#rangeValueEditModal').find("#RangeFrom").val() == "" ? 0 : $('#rangeValueEditModal').find("#RangeFrom").val();
    var rangeTo = $('#rangeValueEditModal').find("#RangeTo").val();
    if (parseInt(rangeTo) <= parseInt(RangeFrom)) {
        $('#rangeValueEditModal').find("#alertFieldForm").UifAlert('show', Resources.MessageValidateRangeTo, "warning");
    }
});


$("#RangeFrom").blur(function () {
    $("#alertFieldForm").UifAlert('hide');
    if (!validateRanges("#RangeFrom", "#RangeTo")) {
        $("#RangeFrom").val("");
        $("#alertFieldForm").UifAlert('show', Resources.MessageValidateRangeFrom, "warning");
    }
});
$("#RangeTo").blur(function () {
    $("#alertFieldForm").UifAlert('hide');
    if (!validateRanges("#RangeFrom", "#RangeTo")) {
        $("#RangeTo").val("");
        $("#alertFieldForm").UifAlert('show', Resources.MessageValidateRangeTo, "warning");
    }
});


$("#InitialDay").blur(function () {
    $("#alertRange").UifAlert('hide');
    if (!validateRanges("#InitialDay", "#EndDay")) {
        $("#InitialDay").val("");
        $("#alertRange").UifAlert('show', Resources.MessageValidateRangeFrom, "warning");
    }
});

$("#EndDay").blur(function () {
    $("#alertRange").UifAlert('hide');
    if (!validateRanges("#InitialDay", "#EndDay")) {
        $("#EndDay").val("");
        $("#alertRange").UifAlert('show', Resources.MessageValidateRangeFrom, "warning");
    }
});

//////////////////////////////////////////////////////////////////////////////////////////////////////////////


/*Graba Cabecera-Detalle*/
$('#rangeParametrizationSaveAddField').click(function () {
    $("#rangeParametrizationAddFieldForm").validate();

    if ($("#rangeParametrizationAddFieldForm").valid()) {
        if (ValidateAddFormHeader() == true) {
            if (ValidateAddRangeDetailForm() == true) {
                cleanArrayRange();

                oRangeControl.Id = 0;
                oRangeControl.Description = $("#rangeParametrizationAddFieldForm").find("#Description").val();
                oRangeControl.IsDefault = rangeParamItHasDefault;
                RangeItem.Order = $("#rangeParametrizationAddFieldForm").find("#ColumnNumber").val();
                RangeItem.RangeFrom = $("#rangeParametrizationAddFieldForm").find("#InitialDay").val();
                RangeItem.RangeTo = $("#rangeParametrizationAddFieldForm").find("#EndDay").val();
                oRangeControl.RangeItems.push(RangeItem);                

                SaveRanges("H");
                /*Se actualiza el anterior rango por defecto*/
                if (rangeParamItHasDefault) {
                    onlyOneDefault();
                }

                $("#rangeParametrizationAddFieldForm").formReset();
                $('#rangeParametrizationModalAddField').UifModal('hide');
            }
        }
        else {
            $("#alertFieldForm").UifAlert('hide');
        }
    }
});

/*Graba Detalle de rangos*/
$('#saveAddRangeDetail').click(function () {
    $("#rangeParametrizationAddRangeDetailForm").validate();

    if ($("#rangeParametrizationAddRangeDetailForm").valid()) {
        if (ValidateAddRangeDetail() == true) {
            cleanArrayRange();
            oRangeControl.Id = rangeParametrizationRangeId;
            RangeItem.Order = $("#rangeParametrizationAddRangeDetailForm").find("#Order").val();
            RangeItem.RangeFrom = $("#rangeParametrizationAddRangeDetailForm").find("#RangeFrom").val();
            RangeItem.RangeTo = $("#rangeParametrizationAddRangeDetailForm").find("#RangeTo").val();
            oRangeControl.RangeItems.push(RangeItem);

            SaveRanges("D");

            $("#rangeParametrizationAddRangeDetailForm").formReset();
            $('#rangeParametrizationModalAddRangeDetail').UifModal('hide');
        }


    }
});


//Actualiza cabecera
$('#rangeParametrizationSaveEditField').click(function () {
    $("#rangeParametrizationEditFieldForm").validate();

    if ($("#rangeParametrizationEditFieldForm").valid()) {

        if (!$("#rangeParametrizationEditFieldForm").find("#checkDefault").hasClass('glyphicon glyphicon-check')) {
            rangeParamItHasDefault = false;
        } else {
            rangeParamItHasDefault = true;
        }

        if ($("#rangeParametrizationEditFieldForm").find('#Description').val() == "") {
            $("#alertFieldForm").UifAlert('show', Resources.EntryDescription, "warning");
        }
        else {
            cleanArrayRange();
            oRangeControl.Id = rangeParametrizationRangeId;
            oRangeControl.Description = $("#rangeParametrizationEditFieldForm").find("#Description").val();
            oRangeControl.IsDefault = rangeParamItHasDefault;  

            UpdateRanges(1);
            if (rangeParamItHasDefault && countCheck > 0) {
                onlyOneDefault();
            }
            countCheck = 0;
            $("#rangeParametrizationEditFieldForm").formReset();
            $('#rangeParametrizationModalEditField').UifModal('hide');                      
        }
    }
});

//Check box
$("#rangeParametrizationAddFieldForm").find('#checkDefault').on('click', function () {
    if (rangeParamItHasDefault) {
        rangeParamItHasDefault = false;
    }
    else {
        rangeParamItHasDefault = true;
    }

    if ($(checkType).hasClass("glyphicon glyphicon-unchecked")) {
        checkDefaulRange(checkType);
    }
    else if ($(checkType).hasClass("glyphicon glyphicon-check")) {
        uncheckDefaulRange(checkType);
    }
});

$("#rangeParametrizationEditFieldForm").find('#checkDefault').on('click', function () {

    if ($(editCheckType).hasClass("glyphicon glyphicon-unchecked")) {
        checkDefaulRange(editCheckType);
        countCheck++;
    }
    else if ($(editCheckType).hasClass("glyphicon glyphicon-check")) {
        uncheckDefaulRange(editCheckType);
    }
});


//------------------------------------------------------
//FUNCIONES
//------------------------------------------------------
function cleanArrayRange() {

    oRangeControl = {
        Id: 0,
        Description: 0,
        IsDefault: false,
        RangeItems: []
    };

    RangeItem = {
        Id: 0,
        Order: 0,
        RangeFrom: 0,
        RangeTo: 0,
    };
};
function ValidateAddFormHeader() {
    if ($("#rangeParametrizationAddFieldForm").find('#Description').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }
    return true;
}

function ValidateAddRangeDetailForm() {
    if ($("#rangeParametrizationAddFieldForm").find('#ColumnNumber').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryColumnNumber, "warning");
        return false;
    }

    if ($("#rangeParametrizationAddFieldForm").find('#InitialDay').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryInitialPosition, "warning");
        return false;
    }
    if ($("#rangeParametrizationAddFieldForm").find('#EndDay').val() == "") {
        $("#alertFieldForm").UifAlert('show', 'Error', "warning");
        return false;
    }

    return true;
}

function ValidateAddRangeDetail() {
    if ($("#rangeParametrizationAddFieldForm").find('#Order').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryColumnNumber, "warning");
        return false;
    }

    if ($("#rangeParametrizationAddFieldForm").find('#RangeFrom').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryInitialPosition, "warning");
        return false;
    }
    if ($("#rangeParametrizationAddFieldForm").find('#RangeTo').val() == "") {
        $("#alertFieldForm").UifAlert('show', 'Error', "warning");
        return false;
    }
    return true;
}

function SaveRanges(option) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveRanges",
        data: JSON.stringify({ "fieldRangeModel": oRangeControl }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Id == 0) {
                $("#rangeParametrizationAlert").UifAlert('show', Resources.SaveError, "danger");
            }
            else {
                if (option == "H") {
                    var controller = ACC_ROOT + "Parameters/GetRanges";
                    $("#tableRange").UifDataTable({ source: controller });
                    $("#alertRangeHeader").UifAlert('show', Resources.SaveSuccessfully, "success");
                }
                else {
                    var controller = ACC_ROOT + "Parameters/GetRangesItem?rangeId=" + rangeParametrizationRangeId;
                    $("#tableRangeDetail").UifDataTable({ source: controller });
                    $("#rangeParametrizationAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                }
            }
        }
    });
}

function UpdateRanges(process) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/UpdateRanges",
        data: JSON.stringify({ "oRangeControl": oRangeControl }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data == false) {
                $("#rangeParametrizationAlert").UifAlert('show', "", "danger");
            }
            else {
                if (process == 1) {
                    $("#alertRangeHeader").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "Parameters/GetRanges";
                    $("#tableRange").UifDataTable({ source: controller });
                }
            }
        }
    });
}

//consulta si un rango tiene items(detalle)
function queryRangeItems(idRange) {
    $.ajax({
        type: "POST",
        async: false,
        url: ACC_ROOT + "Parameters/GetCountRangeItem",
        data: JSON.stringify({ "rangeId": idRange }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {

        if (data > 0) {
            containsItems = true;
        } else {
            containsItems = false;
        }
    });
    return containsItems;
}

//confirmacion de eliminacion
$("#rangeParametrizationDeleteFieldModal").on('click', function () {
    $('#rangeParametrizationModalDeleteRange').modal('hide');
    if (headerDetail == "Header") {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/DeleteRanges",
            data: JSON.stringify({ "rangeId": rangeParametrizationRangeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data) {
                $("#alertRangeHeader").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "Parameters/GetRanges";
                $("#tableRange").UifDataTable({ source: controller });
            }
            else {
                $("#alertRangeHeader").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "danger");
            }
            $('#rangeParametrizationModalDeleteRange').UifModal('hide');
        });
    }
    else {

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/DeleteRangesDetails",
            data: JSON.stringify({ "rangeControl": RangeItem }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data) {
                $("#rangeParametrizationAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "Parameters/GetRangesItem?rangeId=" + RangeItem.Id;
                $("#tableRangeDetail").UifDataTable({ source: controller });
            }
            else {
                $("#rangeParametrizationAlert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "danger");
            }
            $('#rangeParametrizationModalDeleteRange').UifModal('hide');

        });
    }
});

function onlyOneDefault() {

    cleanArrayRange();
    var fields = $('#tableRange').UifDataTable('getData');
    var isDefaultTable = false;
    var idRangeHeader = 0;
    if (fields != null) {
        for (var j = 0; j < fields.length; j++) {
            if (fields[j].Default == true) {
                isDefaultTable = true;
                oRangeControl.Id = fields[j].Id;
                oRangeControl.Description = fields[j].Description;
                oRangeControl.IsDefault = false;
                break;
            }
        }
    }

    /*Se modifica el rango default anterior */
    if (isDefaultTable) {
        UpdateRanges(0);
    }
}

function validateRanges(rangeFrom, RangeTo) {
    var result = true;
    var RangeFrom = $(rangeFrom).val() == "" ? 0 : $(rangeFrom).val();
    var rangeTo = $(RangeTo).val();
    if (parseInt(rangeTo) <= parseInt(RangeFrom)) {
        result = false;
    }
    return result;
}

function checkDefaulRange(checkbox) {
    $(checkbox).removeClass("glyphicon glyphicon-unchecked");
    $(checkbox).addClass("glyphicon glyphicon-check");
}

function uncheckDefaulRange(checkbox) {
    $(checkbox).removeClass("glyphicon glyphicon-check");
    $(checkbox).addClass("glyphicon glyphicon-unchecked");
}
