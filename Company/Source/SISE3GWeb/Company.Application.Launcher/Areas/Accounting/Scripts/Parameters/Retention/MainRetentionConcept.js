var currentEditIndex = 0;
var index = 0;
var designFormatId = 0;
var formatId = 0;
var columnNumber = 0;
var bankNetworkId = 0;

var retentionConceptId = 0;

function RowRetentionConcept() {
    this.Description;
    this.Id;
    this.RetentionBase;
    this.Status;
    this.DifferenceAmount;
}

function RowBase() {
    this.Description;
    this.Id;
}

function RowStatus() {
    this.Description;
    this.Id;
}

function RowPercentageRetentionConcept() {
    this.Id;
    this.DateFrom;
    this.DateTo;
    this.Percentage;
    this.ExternalCode;
    this.RetentionConcept;
}

/////////////////////////////////////////////
//  Valida fechas                          //
/////////////////////////////////////////////
$("#ValidationUntil").blur(function () {
    $("#alertFieldForm").UifAlert('hide');

    if ($("#ValidationUntil").val() != '') {

        if (IsDate($("#ValidationUntil").val()) == true) {
            if ($("#ValidationUntil").val() != '') {
                if (CompareDates($('#ValidationSince').val(), $("#ValidationUntil").val())) {
                    $("#alertFieldForm").UifAlert('show', Resources.MessageValidateDateTo, "warning");
                    $("#ValidationUntil").val('');
                    return true;
                }
            }
        } else {
            $("#alertFieldForm").UifAlert('show', Resources.EntryDateTo, "danger");
            $("#ValidationUntil").val("");
        }
    }
});

/// Tabla de formatos: obtener detalle diseño de formato
$('#tableRetentionConcept').on('rowSelected', function (event, selectedRow) {
    $("#alertRetentionConcept").UifAlert('hide');
    $("#alertDetailRetentionConcept").UifAlert('hide');

    retentionConceptId = selectedRow.Id;

    var controller = ACC_ROOT + "Retention/GetRetentionConceptPercentages?retentionConceptId=" + selectedRow.Id;
    $("#tableDetailsRetentionConcept").UifDataTable({ source: controller });
});


/// Grabar en la tabla de conceptos de retención
$('#saveAdd').click(function () {
    $("#addForm").validate();
    if ($("#addForm").valid()) {
        if (ValidateAddForm() == true) {
            var rowModel = new RowRetentionConcept();
            var rowModelBase = new RowBase();
            var rowModelStatus = new RowStatus();

            rowModel.Id = 0;
            rowModel.Description = $("#addForm").find("#RetentionConceptDescription").val();
            rowModelBase.Id = $("#addForm").find("#RetentionConceptTaxBase").val();
            rowModel.RetentionBase = rowModelBase;
            rowModelStatus.Id = $("#addForm").find("#RetentionConceptStatus").val();
            rowModel.Status = $("#addForm").find("#RetentionConceptStatus").val();
            rowModel.DifferenceAmount = ReplaceDecimalPoint($("#addForm").find("#Tolerance").val());
            SaveRetentionConcept(rowModel, "I");
            $("#addForm").formReset();
            $('#modalAdd').UifModal('hide');
        }
    }
});

/// Tabla de Conceptos de retención: Agregar nuevo registro
$('#tableRetentionConcept').on('rowAdd', function (event, data) {
    var controller = ACC_ROOT + "Retention/GetTaxBase";
    $('#addForm').find("#RetentionConceptTaxBase").UifSelect({ source: controller });

    controller = ACC_ROOT + "Retention/GetActive";
    $('#addForm').find("#RetentionConceptStatus").UifSelect({ source: controller });

    $('#modalAdd').UifModal('showLocal', Resources.AddRetentionConcept);
});

/// Actualiza en la tabla de conceptos de retención
$('#SaveEdit').click(function () {
    $("#editForm").validate();
    if ($("#editForm").valid()) {
        if (ValidateEditForm() == true) {
            var rowModel = new RowRetentionConcept();
            var rowModelBase = new RowBase();
            var rowModelStatus = new RowStatus();

            rowModel.Id = retentionConceptId;
            rowModel.Description = $("#editForm").find("#RetentionConceptDescription").val();
            rowModelBase.Id = $("#editForm").find("#RetentionConceptTaxBase").val();
            rowModel.RetentionBase = rowModelBase;
            rowModelStatus.Id = $("#editForm").find("#RetentionConceptStatus").val();
            rowModel.Status = $("#editForm").find("#RetentionConceptStatus").val();//rowModelStatus;
            rowModel.DifferenceAmount = ReplaceDecimalPoint($("#editForm").find("#Tolerance").val());
            UpdateRetentionConcept(rowModel);
            $("#editForm").formReset();
            $('#modalEdit').UifModal('hide');
        }
    }
});

$('#tableRetentionConcept').on('rowEdit', function (event, data, position) {

    var controller = ACC_ROOT + "Retention/GetTaxBase";
    $('#editForm').find("#RetentionConceptTaxBase").UifSelect({ source: controller, selectedId: data.RetentionBaseId });

    controller = ACC_ROOT + "Retention/GetActive";
    $('#editForm').find("#RetentionConceptStatus").UifSelect({ source: controller, selectedId: data.StatusId });

    $("#editForm").find("#RetentionConceptDescription").val(data.Description);
    $("#editForm").find("#RetentionConceptTaxBase").val(data.RetentionBaseId);
    $("#editForm").find("#RetentionConceptStatus").val(data.StatusId);
    $("#editForm").find("#Tolerance").val(data.DifferenceAmount);
    retentionConceptId = data.Id;

    $('#modalEdit').UifModal('showLocal', Resources.EditRetentionConcept);
});

/**/
/// Grabar en la tabla de conceptos de retención
$('#saveAddField').click(function () {
    $("#addFieldForm").validate();

    if ($("#addFieldForm").valid()) {

        if (ValidateAddFormPercentage() == true) {
            var rowModelPercentage = new RowPercentageRetentionConcept();

            var rowModel = new RowRetentionConcept();
            var rowModelBase = new RowBase();
            var rowModelStatus = new RowStatus();

            rowModel.Id = retentionConceptId;
            rowModel.Description = "";
            rowModelBase.Id = 0;
            rowModel.RetentionBase = rowModelBase;
            rowModelStatus.Id = "";
            rowModel.Status = rowModelStatus;
            rowModel.DifferenceAmount = 0;

            rowModelPercentage.Id = 0;
            //rowModelPercentage.RetentionConcept.Id = retentionConceptId;
            rowModelPercentage.DateFrom = $("#addFieldForm").find("#ValidationSince").val();
            rowModelPercentage.DateTo = $("#addFieldForm").find("#ValidationUntil").val();
            rowModelPercentage.Percentage = ReplaceDecimalPoint($("#addFieldForm").find("#Percentage").val());
            rowModelPercentage.ExternalCode = $("#addFieldForm").find("#Codes").val();
            rowModelPercentage.RetentionConcept = rowModel;

            SaveRetentionConceptPercentage(rowModelPercentage, "I");
            $("#addFieldForm").formReset();
            $('#modalAddField').UifModal('hide');
        }
    }
});

/// Tabla de campos de formato: Agregar nuevo registro
$('#tableDetailsRetentionConcept').on('rowAdd', function (event, data) {

    $("#addFieldForm").find("#IdRetencionConceptDetail").prop("disabled", false);

    var fields = $('#tableDetailsRetentionConcept').UifDataTable('getData');
    var columnId = 0;
    var start = 0;
    var length = 0;

    if (retentionConceptId > 0) {
        if (fields.length == 0) {
            $("#addFieldForm").find("#IdRetencionConceptDetail").prop("disabled", false);
        }

        else {
            $("#addFieldForm").find("#IdRetencionConceptDetail").prop("disabled", true);
            var inicialDate = fields[0].DateTo;
            var dateFormat = addDate(1, inicialDate);

            $("#addFieldForm").find("#ValidationSince").prop("disabled", true);
            $("#addFieldForm").find("#ValidationSince").val(dateFormat);
        }

        for (var j = 0; j < fields.length; j++) {
            columnId = fields[j].Id;
            start = fields[j].InitialPosition;
            length = fields[j].Length;
        }
        if (start == 0 && length == 0) {
            start = 1;
        }

        //$("#addFieldForm").find("#IdRetencionConceptDetail").val(columnId + 1);
        $("#addFieldForm").find("#InitialPosition").val(start + length);
        $("#addFieldForm").find("#IdRetencionConceptDetail").prop("disabled", true);
        $("#addFieldForm").find("#InitialPosition").prop("disabled", true);

        $('#modalAddField').UifModal('showLocal', Resources.AddPercentageByRetentionConcept);
    }
    else {
        $("#alertDetailRetentionConcept").UifAlert('show', 'Seleccione un Concepto de Retencion', "warning");
    }
});

/// Tabla de campos de formato: editar un registro
$('#tableDetailsRetentionConcept').on('rowEdit', function (event, data, position) {

    var fields = $('#tableDetailsRetentionConcept').UifDataTable('getData');

    if (position == 0) {
        // Enable #ColumnNumber
        $("#editFieldForm").find("#IdRetencionConceptDetail").prop("disabled", true);
        $("#editFieldForm").find("#ValidationSince").prop("disabled", true);

        $("#editFieldForm").find("#IdRetencionConceptDetail").val(data.Id);
        $("#editFieldForm").find("#ValidationSince").val(data.DateFrom);
        $("#editFieldForm").find("#ValidationUntil").val(data.DateTo);
        $("#editFieldForm").find("#Percentage").val(data.Percentage);
        $("#editFieldForm").find("#Codes").val(data.ExternalCode);

        $('#modalEditField').UifModal('showLocal', Resources.EditPercentageByRetentionConcept);

    }
    else {
        $("#alertDetailRetentionConcept").UifAlert('show', Resources.CanNotBeModified, "danger");
    }

});

/// Actualiza en la tabla de porcentajes de conceptos de retención
$('#saveEditField').click(function () {
    if (ValidateEditFormPercentage() == true) {
        var rowModelPercentage = new RowPercentageRetentionConcept();

        var rowModel = new RowRetentionConcept();
        var rowModelBase = new RowBase();
        var rowModelStatus = new RowStatus();

        rowModel.Id = retentionConceptId;
        rowModel.Description = "";
        rowModelBase.Id = 0;
        rowModel.RetentionBase = rowModelBase;
        rowModelStatus.Id = "";
        rowModel.Status = rowModelStatus;
        rowModel.DifferenceAmount = 0;

        rowModelPercentage.Id = $("#editFieldForm").find("#IdRetencionConceptDetail").val();
        rowModelPercentage.DateFrom = $("#editFieldForm").find("#ValidationSince").val();
        rowModelPercentage.DateTo = $("#editFieldForm").find("#ValidationUntil").val();
        rowModelPercentage.Percentage = ReplaceDecimalPoint($("#editFieldForm").find("#Percentage").val());
        rowModelPercentage.ExternalCode = $("#editFieldForm").find("#Codes").val();
        rowModelPercentage.RetentionConcept = rowModel;

        UpdateRetentionConceptPercentage(rowModelPercentage);
        $("#editFieldForm").formReset();
        $('#modalEditField').UifModal('hide');
    }
});

$('#tableRetentionConcept').on('rowSelected', function (event, data, position) {
    var controller = ACC_ROOT + "Retention/GetRetentionConceptPercentages?retentionConceptId=" + data.Id;
    $("#tableDetailsRetentionConcept").UifDataTable({ source: controller });
});


//////FUNCIONES

function SaveRetentionConcept(rowModel, operationType) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Retention/SaveRetentionConcept",
        data: JSON.stringify({ "retentionConcept": rowModel, "operationType": operationType }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result == false) {
                $("#alertRetentionConcept").UifAlert('show', data[0].MessageError, "danger");
            }
            else {
                $("#alertRetentionConcept").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "Retention/GetRetentionConcepts";
                $('#tableRetentionConcept').UifDataTable({ source: controller })
            }
        }   
    });
}

function UpdateRetentionConcept(rowModel) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Retention/UpdateRetentionConcept",
        data: JSON.stringify({ "retentionConcept": rowModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data.result == false) {
                $("#alertRetentionConcept").UifAlert('show', data[0].MessageError, "danger");
            }
            else {
                $("#alertRetentionConcept").UifAlert('show', Resources.UpdateSucces, "success");
                var controller = ACC_ROOT + "Retention/GetRetentionConcepts";
                $('#tableRetentionConcept').UifDataTable({ source: controller })
            }
        }
    });
}

function SaveRetentionConceptPercentage(rowModel, operationType) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Retention/SaveRetentionConceptPercentage",
        data: JSON.stringify({ "retentionConceptPercentage": rowModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            
            if (data.result == false) {
                $("#alertDetailRetentionConcept").UifAlert('show', 'Duplicado', "danger");
            }
            else {
                $("#alertDetailRetentionConcept").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "Retention/GetRetentionConceptPercentages?retentionConceptId=" + retentionConceptId;
                $("#tableDetailsRetentionConcept").UifDataTable({ source: controller });
            }
        }
    });
}


function UpdateRetentionConceptPercentage(rowModel) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Retention/UpdateRetentionConceptPercentage",
        data: JSON.stringify({ "retentionConceptPercentage": rowModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result == false) {
                $("#alertDetailRetentionConcept").UifAlert('show', data[0].MessageError, "danger");
            }
            else {
                $("#alertDetailRetentionConcept").UifAlert('show', Resources.UpdateSucces, "success");
                var controller = ACC_ROOT + "Retention/GetRetentionConceptPercentages?retentionConceptId=" + retentionConceptId;
                $("#tableDetailsRetentionConcept").UifDataTable({ source: controller });
            }
        }
    });
}


var addDate = function (d, dateInitial) {
    var NewDate = new Date();
    var sDate = dateInitial || (NewDate.getDate() + "/" + (NewDate.getMonth() + 1) + "/" + NewDate.getFullYear());
    var sep = sDate.indexOf('/') != -1 ? '/' : '-';
    var aDate = sDate.split(sep);
    var dateFormat = aDate[2] + '/' + aDate[1] + '/' + aDate[0];
    dateFormat = new Date(dateFormat);
    dateFormat.setDate(dateFormat.getDate() + parseInt(d));
    var year = dateFormat.getFullYear();
    var month = dateFormat.getMonth() + 1;
    var day = dateFormat.getDate();
    month = (month < 10) ? ("0" + month) : month;
    day = (day < 10) ? ("0" + day) : day;
    var dateFinal = day + sep + month + sep + year;
    return (dateFinal);
}

function ValidateAddFormPercentage() {

    if ($("#addFieldForm").find('#ValidationSince').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTaxableBaseOfRetentions, "warning");
        return false;
    }

    if ($("#addFieldForm").find('#ValidationUntil').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectFile, "warning");
        return false;
    }

    if ($("#addFieldForm").find('#Percentage').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectFile, "warning");
        return false;
    }
    else {
        if (parseInt($("#addFieldForm").find('#Percentage').val()) > 100) {
            $("#alertFieldForm").UifAlert('show', Resources.Percentage + " " + Resources.MessageLevelPercentage, "warning");
            return false;
        }
    }

    if ($("#addFieldForm").find('#Codes').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectFile, "warning");
        return false;
    }

    return true;
}


function ValidateAddForm() {
    if ($("#addForm").find('#RetentionConceptDescription').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EnterRetentionConcept, "warning");
        return false;
    }
    if ($("#addForm").find('#RetentionConceptTaxBase').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTaxableBaseOfRetentions, "warning");
        return false;
    }

    if ($("#addForm").find('#RetentionConceptStatus').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectFile, "warning");
        return false;
    }

    return true;
}

function ValidateEditForm() {
    if ($("#editForm").find('#RetentionConceptDescription').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EnterRetentionConcept, "warning");
        return false;
    }
    if ($("#editForm").find('#RetentionConceptTaxBase').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTaxableBaseOfRetentions, "warning");
        return false;
    }

    if ($("#editForm").find('#RetentionConceptStatus').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectFile, "warning");
        return false;
    }

    return true;
}

function ValidateEditFormPercentage() {

    if ($("#editFieldForm").find("#ValidationSince").val() == "") {
        $("#alertForm").UifAlert('show', Resources.EnterRetentionConcept, "warning");
        return false;
    }

    if ($("#editFieldForm").find("#ValidationUntil").val() == "") {
        $("#alertForm").UifAlert('show', Resources.EnterRetentionConcept, "warning");
        return false;
    }

    if ($("#editFieldForm").find("#Percentage").val() == "") {
        $("#alertForm").UifAlert('show', Resources.EnterRetentionConcept, "warning");
        return false;
    }

    if ($("#editFieldForm").find("#Codes").val() == "") {
        $("#alertForm").UifAlert('show', Resources.EnterRetentionConcept, "warning");
        return false;
    }

    return true;
}
