
var structureType = '#StructureType';
var format = '#Format';
var tableFormatType = "#tableFormatType";
var tableFieldFormat = "#tableFieldFormat";
var addFormType = "#addFormType";
var editFormType = "#editFormType";
var addFieldForm = "#addFieldForm";
var editFieldForm = "#editFieldForm";
var selectFormatType = "#selectFormatType";

var idStructure = 0;
var structureDescription = "";
var idFormat = 0;
var FormatDescription = "";
var idFormatType = 0;
var idFielFormat = 0;
var itHasDefault = 1;
var editPosition = -1;
var Start = 0;
var documentArea = 0;
var rowHeaderDetail = 0;
var coumnAvailable = 0;
var repetitiveColumn = false;
var oRowNumber = 1;
var validateUpdateRow = 0;
var validateUpdateColumn = 0;
var templateId = 0;
var formatTypeCd;
var isDefault = 0;

$(structureType).on('itemSelected', function (event, selectedItem) {
    idStructure = selectedItem.Id;
    structureDescription = selectedItem.Text;

    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Parameters/GetFormatSelect?idStructure=" + selectedItem.Id;
        $(format).UifSelect({ source: controller });
    }
    $(tableFieldFormat).UifDataTable('clear');
    $(tableFormatType).UifDataTable('clear');
});

$(selectFormatType).on('itemSelected', function (event, selectedItem) {
    $("#alertFormTemplateTypeModal").UifAlert('hide');
    var fields = $(tableFormatType).UifDataTable('getData');
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if (selectedItem.Id == fields[j].FormatTypeCd) {
                $(selectFormatType).val("");
                $("#alertFormTemplateTypeModal").UifAlert('show', Resources.AlreadyExistsByTemplate, "warning");
                break;
            }
            if (selectedItem.Id == 2) {
                $(addFormType).find("#ColumnNumber").prop("disabled", true);
                $(addFormType).find("#RowNumber").removeAttr("disabled");
            }
            else if (selectedItem.Id == 3) {
                $("#RowNumber").val(0);
                $(addFormType).find("#ColumnNumber").removeAttr("disabled");
                $(addFormType).find("#RowNumber").prop("disabled", true);
            }
            else {
                $(addFormType).find("#ColumnNumber").removeAttr("disabled");
                $(addFormType).find("#RowNumber").removeAttr("disabled");
            }
        }
    }
    else {
        if (selectedItem.Id == 2) {
            $(addFormType).find("#ColumnNumber").prop("disabled", true);
        }
        else {
            $(addFormType).find("#ColumnNumber").removeAttr("disabled");
            $(addFormType).find("#RowNumber").removeAttr("disabled");
        }
    }
});

$(format).on('itemSelected', function (event, selectedItem) {
    idFormat = selectedItem.Id;
    FormatDescription = selectedItem.Text;

    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Parameters/GetFormatTypeByFormat?idFormat=" + selectedItem.Id;
        $(tableFormatType).UifDataTable({ source: controller });
        $("#tableFieldFormat").UifDataTable('clear');
    }
});

/*tableFormatType///////////////////////////////////////////////////*/
$(tableFormatType).on('rowAdd', function (event, data) {
    $("#alertFormTemplateTypeModal").UifAlert('hide');
    var order = 0;
    var columnId = 0;
    var index = 0;
    var length = 0;

    getFormatType(0, '', "#addFormType");
    
    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    $(selectFormatType).val("");
    idFormatType = 0;
    if ($(format).val() == "" || $(format).val() == null) {
        $("#alertTemplate").UifAlert('show', Resources.WarningTemplateGroup, "warning");
    }
    else {

        if ($("#StructureType").val() == "2") {
            $('#modalAddFormType').find("#divFormatUse").hide();
            $('#modalAddFormType').find("#divFieldAutomaticDebit").show();
        }
        else {
            $('#modalAddFormType').find("#divFormatUse").hide();
            $('#modalAddFormType').find("#divFieldAutomaticDebit").hide();
        }

        $(tableFieldFormat).UifDataTable('clear');
        var fields = $(tableFieldFormat).UifDataTable('getData');
        /*Se consulta la ultima fila de la cabecera para validar que no se vuelva a ingresar en el detalle*/
        var headers = $(tableFormatType).UifDataTable('getData');
        if (headers.length > 0) {
            formatTypeCd = headers[0].Id;
            if (headers[0].FormatTypeCd == 1) {
                getRowsNumberHeader();
            }
        }

        /*se obtine el secuencial de fila y columna*/
        if (fields.length > 0) {
            for (var j = 0; j < fields.length; j++) {
                order = fields[j].Order;
                columnId = fields[j].ColumnNumber;
                index = fields[j].Start;
                length = fields[j].Length;
            }
        }
        else {
            index = 1;
        }

        $(addFormType).find("#Order").val(order + 1);
        $(addFormType).find("#Start").val(index + length);
        $(addFormType).find("#ColumnNumber").val(columnId + 1);
        $(addFormType).find("#Order").prop("disabled", true);
        $(addFormType).find("#Start").prop("disabled", true);
        $(addFormType).find("#ColumnNumber").prop("disabled", true);
        cleanTemplateParametrization('#modalAddFormType');
        $('#modalAddFormType').UifModal('showLocal', Resources.AddFormatDesign);
    }
});

$(tableFormatType).on('rowSelected', function (event, selectedRow) {
    $("#alertTemplate").UifAlert('hide');
    idFormatType = selectedRow.Id;
    idFielFormat = selectedRow.Id;
    isDefault = selectedRow.Default;
    documentArea = selectedRow.FormatTypeCd;

    var controller = ACC_ROOT + "Parameters/GetFormatFieldByFormatTypeId?formatTypeId=" + selectedRow.Id;
    $(tableFieldFormat).UifDataTable({ source: controller });
});

$(tableFormatType).on('rowEdit', function (event, data, position) {
    $("#alertFormTemplateTypeModal").UifAlert('hide');
    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    templateId = data.Id;

    if ($("#StructureType").val() == "2") {
        $('#modalEditFormType').find("#divFormatUse").hide();
        $('#modalEditFormType').find("#divFieldAutomaticDebit").show();
    }
    else {
        $('#modalEditFormType').find("#divFormatUse").hide();
        $('#modalEditFormType').find("#divFieldAutomaticDebit").hide();
    }
    getFormatType(data.FormatTypeCd, data.FormatTypeDescription, "#editFormType");

   
    $(editFormType).find("#Separator").val(data.Separator);
    $('#modalEditFormType').UifModal('showLocal', Resources.EditFormatDesign);
    $(editFormType).find('#fieldModal').hide();
});

$(tableFormatType).on('rowDelete', function (event, data) {
    $("#alertFormTemplateTypeModal").UifAlert('hide');
    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    templateId = data.Id;
    rangeId = data.Id;
    $('#modalDeleteFormatType').appendTo("body").modal('show');
});

//tableFieldFormat/////////////////////////////////////////////////

$(tableFieldFormat).on('rowAdd', function (event, data) {
    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    $("#modalAddFieldForm").find("#alertFormTemplateTypeModal").UifAlert('hide');
    editPosition = -1;

    $(selectFormatType).val("");
    var headerfields = $(tableFormatType).UifDataTable('getSelected');
    idFielFormat = 0;
    if ($(format).val() == "" || $(format).val() == null) {
        $("#alertTemplate").UifAlert('show', Resources.WarningTemplateGroup, "warning");
    }
    else if (headerfields != null) {
        var fields = $(tableFieldFormat).UifDataTable('getData');

        /*secuencial de columna*/
        var order = 0;
        var columnId = 0;
        var startAdd = 0;
        var length = 0;

        if (fields.length > 0) {
            oRowNumber = fields[0].RowNumber;
            $(addFieldForm).find("#RowNumber").prop("disabled", true);

            for (var j = 0; j < fields.length; j++) {
                order = fields[j].Order;
                columnId = fields[j].ColumnNumber;
                startAdd = fields[j].Start;
                length = fields[j].Length;
            }
        }
        else {
            startAdd = 1;
        }

        $(addFieldForm).find('#modalFormatHeader').hide();
        $(addFieldForm).find("#Order").val(order + 1);
        $(addFieldForm).find("#Start").val(startAdd + length);
        $(addFieldForm).find("#Order").prop("disabled", true);
        $(addFieldForm).find("#Start").prop("disabled", true);


        if (documentArea == 1) {
            $(addFieldForm).find("#ColumnNumber").val(columnId + 2);
            $(addFieldForm).find("#ColumnNumber").removeAttr("disabled");
            $(addFieldForm).find("#RowNumber").removeAttr("disabled");
        }
        else if (documentArea == 2) {
            $(addFieldForm).find("#ColumnNumber").val(columnId + 1);
            $(addFieldForm).find("#RowNumber").val(oRowNumber);
            $(addFieldForm).find("#ColumnNumber").prop("disabled", true);
            //$(addFieldForm).find("#RowNumber").prop("disabled", true);
            if (startAdd > 1) {
                $(addFieldForm).find("#RowNumber").prop("disabled", true);
            }
        }
        else {
            $(addFieldForm).find("#RowNumber").val(0);
            $(addFieldForm).find("#RowNumber").prop("disabled", true);
            $(addFieldForm).find("#ColumnNumber").removeAttr("disabled");
        }

        /*Consulta la fila de cabecera o detalle y evitar duplicidad (1: cabecera; 2:detalle)*/
        if (documentArea != 3) {
            getRowsNumber();
        }

        if ($("#StructureType").val() == "2") {
            $('#modalAddFieldForm').find("#divFormatUse").hide();
            $('#modalAddFieldForm').find("#divFieldAutomaticDebit").show();
        }
        else {
            $('#modalAddFieldForm').find("#divFormatUse").hide();
            $('#modalAddFieldForm').find("#divFieldAutomaticDebit").hide();
        }
        cleanTemplateParametrization('#modalAddFieldForm');
        $('#modalAddFieldForm').UifModal('showLocal', Resources.AddFormatDesign);
    }
    else {
        $("#alertTemplate").UifAlert('show', Resources.SelectDetail, "warning");
    }

});

$(tableFieldFormat).on('rowEdit', function (event, data, position) {
    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('hide');

    idFielFormat = data.Id;
    editPosition = position;

    validateUpdateRow = data.RowNumber;
    validateUpdateColumn = data.ColumnNumber;

    $(editFieldForm).find("#DescriptionTemplate").val(data.Description);
    $(editFieldForm).find("#Order").val(data.Order);
    $(editFieldForm).find("#Start").val(data.Start);
    $(editFieldForm).find("#Length").val(data.Length);
    $(editFieldForm).find("#Value").val(data.Value);
    $(editFieldForm).find("#Filled").val(data.Filled);
    $(editFieldForm).find("#Align").val(data.Align);
    $(editFieldForm).find("#ColumnNumber").val(data.ColumnNumber);
    $(editFieldForm).find("#RowNumber").val(data.RowNumber);
    $(editFieldForm).find("#FieldName").val(data.Field);
    $(editFieldForm).find("#selectFieldFormat").val(data.Mask);

    if (data.IsRequired == true) {
        check("#editFieldForm");
    }
    else {
        uncheck("#editFieldForm");
    }

    $(editFieldForm).find("#Order").prop("disabled", true);
    $(editFieldForm).find("#Start").prop("disabled", true);
    if (documentArea == 2) {
        $(editFieldForm).find("#ColumnNumber").prop("disabled", true);
        $(editFieldForm).find("#RowNumber").removeAttr("disabled");
    }
    else {
        if (documentArea == 3) {
            $(addFieldForm).find("#RowNumber").val(0);
            $(editFieldForm).find("#ColumnNumber").removeAttr("disabled");
            $(editFieldForm).find("#RowNumber").prop("disabled", true);
        }
        else {
            $(editFieldForm).find("#ColumnNumber").removeAttr("disabled");
            $(editFieldForm).find("#RowNumber").removeAttr("disabled");
        }

    }

    if (documentArea != 3) {
        getRowsNumber();
    }

    if ($("#StructureType").val() == "2") {
        $('#modalEditFieldForm').find("#divFormatUse").hide();
        $('#modalEditFieldForm').find("#divFieldAutomaticDebit").show();
    }
    else {
        $('#modalEditFieldForm').find("#divFormatUse").hide();
        $('#modalEditFieldForm').find("#divFieldAutomaticDebit").hide();
    }

    $(editFieldForm).find('#modalFormatHeader').hide();
    $('#modalEditFieldForm').UifModal('showLocal', Resources.EditTemplate);
});

$(tableFieldFormat).on('rowDelete', function (event, data) {
    $('#alertTemplate').UifAlert('hide');
    $('#alertDetail').UifAlert('hide');
    idFielFormat = data.Id;
    editPosition = data.Order - 1;

    /*verifica si el registro seleccionado es el ultimo para no actualizar, caso contrario se debe actualizar orden y posicion inicial*/
    var tableData = $(tableFieldFormat).UifDataTable('getData');
    if (tableData.length > editPosition) {
        Start = data.Start;
    }
    else {
        Start = 0;
    }

    $('#modalDeleteFiledFormat').appendTo("body").modal('show');
});

/*////////////////////////////////////////////////////////////////*/

/*Botones///////////////////////////////////////////////////////*/
/*FormatType*/
$('#saveAddFormType').click(function () {
    $("#addFormType").validate();
    if ($("#addFormType").valid()) {

        if ($(selectFormatType).val() == 2) {
            if (rowHeaderDetail < parseInt($("#RowNumber").val())) {
                saveFormatParametrization();
            }
            else {
                $("#RowNumber").val(rowHeaderDetail + 1);
                $("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageValidationRows + rowHeaderDetail, "warning");
            }
        }
        else {
            saveFormatParametrization();
        }
    }
});

$('#saveEditFormType').click(function () {
    $(editFormType).validate();

    if ($(editFormType).valid()) {
        var formatType = editFormatType();

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/UpdateFormatDetail",
            data: JSON.stringify({ "formatDetail": formatType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#alertTemplate").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "Parameters/GetFormatTypeByFormat?idFormat=" + $(format).val();
                    $(tableFormatType).UifDataTable({ source: controller });
                }
                else {
                    $("#alertTemplate").UifAlert('show', Resources.ErrorTransaction, "danger");
                }
            }
        });
        $("#editFormType").formReset();
        $('#modalEditFormType').UifModal('hide');
    }
});

//DeleteFormatType
$("#deleteFieldModal").on('click', function () {
    $('#modalDeleteFormatType').modal('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteFormatDetail",
        data: JSON.stringify({ "formatTypeId": templateId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#alertTemplate").UifAlert('show', Resources.DeleteSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetFormatTypeByFormat?idFormat=" + $(format).val();
            $(tableFormatType).UifDataTable({ source: controller });
            $(tableFieldFormat).UifDataTable('clear');
        }
        else {
            $("#alertTemplate").UifAlert('show', data.result, "danger");
        }
        $('#modalDeleteRange').UifModal('hide');
    });
});

/*saveAddFieldForm*/
$('#saveAddFieldForm').click(function () {
    $("#addFieldForm").validate();
    if ($("#addFieldForm").valid()) {
        /*se valida el ingreso de la fila revisando en Cabecera o detalle ;(documentArea(1)Cabecera;(2)detalle)*/
        if (documentArea == 1) {
            if (rowHeaderDetail > 0) {
                if ((parseInt($("#addFieldForm").find("#RowNumber").val())) < rowHeaderDetail) {
                    if (validateColumn("#addFieldForm")) {
                        saveFieldFomatParametrization();
                    }
                    else {
                        if (repetitiveColumn) {
                            $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.QuestionNumberColumn + $("#addFieldForm").find("#ColumnNumber").val() + Resources.MessageValidateRow + $("#addFieldForm").find("#RowNumber").val(), "warning");
                        }
                        else {
                            $("#addFieldForm").find("#ColumnNumber").val(coumnAvailable);
                            $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageColumnAvailable, "warning");
                        }
                    }
                }
                else {
                    $("#addFieldForm").find("#RowNumber").val("");
                    $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageValidationRowLess + rowHeaderDetail, "warning");
                }
            }
            else {
                if (validateColumn("#addFieldForm")) {
                    saveFieldFomatParametrization();
                }
                else {
                    if (repetitiveColumn) {
                        $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.QuestionNumberColumn + $("#addFieldForm").find("#ColumnNumber").val() + ' ya existe para la fila ' + $("#addFieldForm").find("#RowNumber").val(), "warning");
                    }
                    else {
                        $("#addFieldForm").find("#ColumnNumber").val(coumnAvailable);
                        $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageColumnAvailable, "warning");
                    }
                }
            }
        }/*detalle*/
        else if (documentArea == 2) {
            if (rowHeaderDetail > 0) {
                if (rowHeaderDetail < (parseInt($("#addFieldForm").find("#RowNumber").val()))) {
                    saveFieldFomatParametrization();
                }
                else {
                    $("#addFieldForm").find("#RowNumber").val("");
                    $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageValidationRows + rowHeaderDetail + ' en la cabecera', "warning");
                }
            }
            else {
                saveFieldFomatParametrization();
            }
        }/*sumario*/
        else {
            if (validateColumnSumary("#addFieldForm")) {
                saveFieldFomatParametrization();
            }
            else {
                $("#addFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.QuestionNumberColumn + $("#addFieldForm").find("#ColumnNumber").val() + ' ya existe para la fila ' + $("#addFieldForm").find("#RowNumber").val(), "warning");
            }
        }
    }
});

$('#saveEditFieldForm').click(function () {
    $("#editFieldForm").validate();
    if ($("#editFieldForm").valid()) {
        /*se valida el ingreso de la fila revisando en Cabecera o detalle ;(documentArea(1)Cabecera;(2)detalle)*/
        if (documentArea == 1) {
            if (validateUpdateColumn != $("#modalEditFieldForm").find("#ColumnNumber").val() && validateUpdateRow == $("#modalEditFieldForm").find("#RowNumber").val()) {
                if (rowHeaderDetail > 0) {
                    if ((parseInt($("#modalEditFieldForm").find("#RowNumber").val())) < rowHeaderDetail) {
                        if (validateColumn("#modalEditFieldForm")) {
                            saveEditFieldFormat();
                        }
                        else {
                            if (repetitiveColumn) {
                                $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.QuestionNumberColumn + $("#modalEditFieldForm").find("#ColumnNumber").val() + Resources.MessageValidateRow + $("#modalEditFieldForm").find("#RowNumber").val(), "warning");
                            }
                            else {
                                $("#modalEditFieldForm").find("#ColumnNumber").val(coumnAvailable);
                                $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageColumnAvailable, "warning");
                            }
                        }
                    }
                    else {
                        $("#modalEditFieldForm").find("#RowNumber").val("");
                        $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageValidationRowLess + rowHeaderDetail, "warning");
                    }
                }
                else {
                    if (validateColumn("#modalEditFieldForm")) {
                        saveEditFieldFormat();
                    }
                    else {
                        if (repetitiveColumn) {
                            $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.QuestionNumberColumn + $("#modalEditFieldForm").find("#ColumnNumber").val() + Resources.MessageValidateRow + $("#modalEditFieldForm").find("#RowNumber").val(), "warning");
                        }
                        else {
                            $("#modalEditFieldForm").find("#ColumnNumber").val(coumnAvailable);
                            $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageColumnAvailable, "warning");
                        }
                    }
                }
            }
            else {
                saveEditFieldFormat();
            }
        }/*detalle*/
        else if (documentArea == 2) {
            if (rowHeaderDetail > 0) {
                if (rowHeaderDetail < (parseInt($("#modalEditFieldForm").find("#RowNumber").val()))) {
                    saveEditFieldFormat();
                }
                else {
                    $("#modalEditFieldForm").find("#RowNumber").val("");
                    $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.MessageValidationRows + rowHeaderDetail, "warning");
                }
            }
            else {
                saveEditFieldFormat();
            }
        }/*sumario*/
        else {
            /*Si el numero de columna no ha sido modificado no se realiza la validacion*/
            if (validateUpdateColumn != $("#modalEditFieldForm").find("#ColumnNumber").val()) {
                if (validateColumnSumary("#modalEditFieldForm")) {
                    saveEditFieldFormat();
                }
                else {
                    $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('show', Resources.QuestionNumberColumn + $("#modalEditFieldForm").find("#ColumnNumber").val() + Resources.MessageValidateRow + $("#modalEditFieldForm").find("#RowNumber").val(), "warning");
                }
            }
            else {
                saveEditFieldFormat();
            }
        }
    }
});

$("#deleteFieldFormat").on('click', function () {
    $('#modalDeleteFiledFormat').modal('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteFormatField",
        data: JSON.stringify({ "idFieldFormat": idFielFormat }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            if (Start > 0) {
                editFieldStart("delete");
            }
            $("#alertDetail").UifAlert('show', Resources.DeleteSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetFormatFieldByFormatTypeId?formatTypeId=" + idFormatType;
            $(tableFieldFormat).UifDataTable({ source: controller });
        }
        else {
            $("#alertDetail").UifAlert('show', data.result, "danger");
        }
        $('#modalDeleteFiledFormat').UifModal('hide');
    });
});

/*check*/
$("#addFormType").find('#checkDefault').click(function () {
    if (itHasDefault == 0) {
        check("#addFormType");
    }
    else if (itHasDefault == 1) {
        uncheck("#addFormType");
    }
});

$("#addFieldForm").find('#checkDefault').click(function () {
    if (itHasDefault == 0) {
        check("#addFieldForm");
    }
    else if (itHasDefault == 1) {
        uncheck("#addFieldForm");
    }
});

$("#editFieldForm").find('#checkDefault').click(function () {
    if (itHasDefault == 0) {
        check("#editFieldForm");
    }
    else if (itHasDefault == 1) {
        uncheck("#editFieldForm");
    }
});

//FUNCIONES//////////////////////////////////////////////////////////////
function editFieldStart(process) {
    var order = 0;
    var orderUpdate = 0;
    var updatePosition = 0;

    updatePosition = editPosition + 1;
    var arrayList = {
        FormatModels: []
    };
    var tableData = $(tableFieldFormat).UifDataTable('getData');

    if (documentArea == 1 || documentArea == 3) {
        for (var j = updatePosition ; j < tableData.length; j++) {
            order = tableData[j].Order - 1;
            if (process == "delete") {
                order = tableData[j].Order - 1;
            }
            else {
                order = tableData[j].Order;
            }
            var field = {
                Id: tableData[j].Id,
                Description: tableData[j].Description,
                Order: order,
                Start: Start,
                Length: tableData[j].Length,
                Value: tableData[j].Value,
                Filled: tableData[j].Filled,
                Align: tableData[j].Align,
                RowNumber: tableData[j].RowNumber,
                ColumnNumber: tableData[j].ColumnNumber,
                Field: tableData[j].Field,
                Mask: tableData[j].Mask,
                IsRequired: tableData[j].IsRequired
            };

            Start = Start + parseInt(tableData[j].Length);
            arrayList.FormatModels.push(field);
        }
    }/*Edicion*/
    else {
        if (process == "edit") {
            var fieldFormat = fieldFormatTypeFunction("#editFieldForm", idFielFormat);

            for (var j = 0; j < tableData.length; j++) {
                if (process == "delete") {
                    editPosition = -1;
                }
                if (j == editPosition) {
                    var field = {
                        Id: fieldFormat.Id,
                        Description: fieldFormat.Description,
                        Order: fieldFormat.Order,
                        Start: fieldFormat.Start,
                        Length: fieldFormat.Length,
                        Value: fieldFormat.Value,
                        Filled: fieldFormat.Filled,
                        Align: fieldFormat.Align,
                        RowNumber: fieldFormat.RowNumber,
                        ColumnNumber: fieldFormat.ColumnNumber,
                        Field: fieldFormat.Field,
                        Mask: fieldFormat.Mask,
                        IsRequired: fieldFormat.IsRequired
                    }
                    arrayList.FormatModels.push(field);
                }
                else {
                    if (j >= updatePosition) {
                        orderUpdate = Start;
                    }
                    else {
                        orderUpdate = tableData[j].Start;
                    }

                    var field = {
                        Id: tableData[j].Id,
                        Description: tableData[j].Description,
                        Order: tableData[j].Order,
                        Start: orderUpdate,
                        Length: tableData[j].Length,
                        Value: tableData[j].Value,
                        Filled: tableData[j].Filled,
                        Align: tableData[j].Align,
                        RowNumber: tableData[j].RowNumber,
                        ColumnNumber: tableData[j].ColumnNumber,
                        Field: tableData[j].Field,
                        Mask: tableData[j].Mask,
                        IsRequired: tableData[j].IsRequired
                    }
                    if (j >= updatePosition) {
                        Start = Start + parseInt(tableData[j].Length);
                    }
                    if (process == "delete") {
                        if (tableData[j].Id != idFielFormat) {
                            if (j >= updatePosition) {
                                field.Order = field.Order - 1;
                            }
                            arrayList.FormatModels.push(field);
                        }
                    }
                    else {
                        arrayList.FormatModels.push(field);
                    }
                }
            }
        }
    }

    var format = { Id: 0, Fields: arrayList.FormatModels, FormatType: "1" }

    //Reorganiza las posiciones iniciales del detalle de formato en caso de haber mas de uno
    if (tableData.length >= 1 && format.Fields.length > 0) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/UpdateFormatDetail",
            data: JSON.stringify({ "formatDetail": format }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (!data.success) {
                    $("#alertTemplate").UifAlert('show', Resources.ErrorTransaction, "danger");
                }
            }
        });
    }
}

function editFormatType() {
    return {
        Id: templateId,
        FormatType: $(editFormType).find("#selectFormatType").val(),
        Separator: $(editFormType).find("#Separator").val(),
    }
}

function fieldFormatTypeFunction(modalForm, id) {
    Start = parseInt($(modalForm).find("#Start").val()) + parseInt($(modalForm).find("#Length").val());
    return {
        Id: id,
        Description: $(modalForm).find("#DescriptionTemplate").val(),
        Order: $(modalForm).find("#Order").val(),
        Start: $(modalForm).find("#Start").val(),
        Length: $(modalForm).find("#Length").val(),
        Value: $(modalForm).find("#Value").val(),
        Filled: $(modalForm).find("#Filled").val(),
        Align: $(modalForm).find("#Align").val(),
        RowNumber: $(modalForm).find("#RowNumber").val(),
        ColumnNumber: $(modalForm).find("#ColumnNumber").val(),
        Field: $(modalForm).find("#FieldName").val(),
        Mask: $(modalForm).find("#selectFieldFormat").val(),
        IsRequired: itHasDefault
    }
}

function templateControl() {
    var format = { Id: $("#Format").val() }
    var oListFormat = {
        FormatModels: []
    };
    var field = {
        Id: idFielFormat,
        Description: $("#DescriptionTemplate").val(),
        Order: $("#Order").val(),
        Start: $("#Start").val(),
        Length: $("#Length").val(),
        Value: $("#Value").val(),
        Filled: $("#Filled").val(),
        Align: $("#Align").val(),
        RowNumber: $("#RowNumber").val(),
        ColumnNumber: $("#ColumnNumber").val(),
        Field: $("#FieldName").val(),
        Mask: $("#selectFieldFormat").val(),
        IsRequired: itHasDefault
    }

    oListFormat.FormatModels.push(field);
    return {
        Id: idFormatType, //Id: formatTypeCd,//Id: idFormatType,
        Format: format,
        FormatType: $("#selectFormatType").val(),
        Separator: $("#Separator").val(),
        Fields: oListFormat.FormatModels
    }
};

function check(form) {
    $(form).find('#checkDefault').removeClass("glyphicon glyphicon-unchecked");
    $(form).find('#checkDefault').addClass("glyphicon glyphicon-check");
    itHasDefault = 1;
}

function uncheck(form) {
    $(form).find('#checkDefault').removeClass("glyphicon glyphicon-check");
    $(form).find('#checkDefault').addClass("glyphicon glyphicon-unchecked");
    itHasDefault = 0;
}

function getRowsNumber() {
    var tableFormat = $(tableFormatType).UifDataTable('getData');
    var areaCd = 0;
    formatTypeCd = 0;
    rowHeaderDetail = 0;
    $.each(tableFormat, function (index, value) {
        if (documentArea == 1) {
            areaCd = 2;
            if (value.FormatTypeCd == areaCd) {
                formatTypeCd = value.Id;
            }
        }
        else if (documentArea == 2) {
            areaCd = 1;
            if (value.FormatTypeCd == areaCd) {
                formatTypeCd = value.Id;
            }
        }
    });

    if (formatTypeCd > 0) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/GetRowsNumber",
            data: JSON.stringify({ "idFormatType": formatTypeCd, "documentArea": areaCd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                rowHeaderDetail = data.result;
            } else {
                $("#alertTemplate").UifAlert('show', Resources.SaveError, "danger");
            }
        });
    }
}

function getRowsNumberHeader() {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/GetRowsNumber",
        data: JSON.stringify({ "idFormatType": formatTypeCd, "documentArea": 1 }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            rowHeaderDetail = data.result;
        } else {
            $("#alertTemplate").UifAlert('show', Resources.SaveError, "danger");
        }
    });
}
function saveFieldFomatParametrization() {
    var fieldFormat = fieldFormatTypeFunction("#addFieldForm", idFormatType);
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveFormatField",
        data: JSON.stringify({ "formatField": fieldFormat }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {

            $("#alertDetail").UifAlert('show', Resources.SaveSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetFormatFieldByFormatTypeId?formatTypeId=" + idFormatType;
            $(tableFieldFormat).UifDataTable({ source: controller });
            $("#addFieldForm").formReset();
            $('#modalAddFieldForm').UifModal('hide');
        } else {
            $("#alertDetail").UifAlert('show', Resources.SaveError, "danger");
        }
    });

}

function saveFormatParametrization() {
    var formatType = templateControl();
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveFormatDetail",
        data: JSON.stringify({ "formatDetail": formatType }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#alertTemplate").UifAlert('show', Resources.SaveSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetFormatTypeByFormat?idFormat=" + $(format).val();
            $(tableFormatType).UifDataTable({ source: controller });
            $("#addFormType").formReset();
            $('#modalAddFormType').UifModal('hide');

        } else {
            $("#alertTemplate").UifAlert('show', Resources.SaveError, "danger");
        }
    });
}

/*se valida el ingreso de la columna*/
function validateColumn(modalFieldForm) {
    var fields = $(tableFieldFormat).UifDataTable('getData');
    var rangeFrom = 0;
    var rangeTo = 0;
    var columnNumber = $(modalFieldForm).find("#ColumnNumber").val();

    var validateColumn = true;

    $.each(fields, function (index, value) {
        /*consulta las columnas del grupo de filas*/
        if (value.RowNumber == $(modalFieldForm).find("#RowNumber").val()) {
            rangeFrom = value.ColumnNumber - 2;
            rangeTo = value.ColumnNumber + 2;

            if (rangeFrom < columnNumber && rangeTo > columnNumber) {
                validateColumn = false;
                repetitiveColumn = false;
            }
            if (rangeTo > coumnAvailable) {
                coumnAvailable = rangeTo;
            }
            if (value.ColumnNumber == columnNumber) {
                validateColumn = false;
                repetitiveColumn = true;
            }
        }
    });
    return validateColumn;
}

function validateColumnSumary(modalFieldForm) {
    var fields = $(tableFieldFormat).UifDataTable('getData');
    var columnNumber = $(modalFieldForm).find("#ColumnNumber").val();

    var validateColumn = true;

    $.each(fields, function (index, value) {
        /*consulta las columnas del grupo de filas*/
        if (value.RowNumber == $(modalFieldForm).find("#RowNumber").val()) {
            if (value.ColumnNumber == columnNumber) {
                validateColumn = false;
            }
        }
    });
    return validateColumn;
}
function cleanTemplateParametrization(form) {
    $(form).find("#DescriptionTemplate").val("");
    $(form).find("#Length").val("");
    $(form).find("#Value").val
    $(form).find("#Filled").val("");
    $(form).find("#Align").val("");
    $(form).find("#FieldName").val("");
    $(form).find("#selectFieldFormat").val("");
    $(form).find('#checkDefault').removeClass("glyphicon glyphicon-check");
    $(form).find('#checkDefault').addClass("glyphicon glyphicon-unchecked");
}

function saveEditFieldFormat() {
    if (documentArea == 1 || documentArea == 3) {
        $("#modalEditFieldForm").find("#alertFormTemplateTypeModal").UifAlert('hide');
        var fieldFormat = fieldFormatTypeFunction("#editFieldForm", idFielFormat);
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/UpdateFormatField",
            data: JSON.stringify({ "formatField": fieldFormat }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                /*Reorganiza las posisiones iniciales*/
                editFieldStart("edit");
                $("#alertDetail").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "Parameters/GetFormatFieldByFormatTypeId?formatTypeId=" + idFormatType;
                $(tableFieldFormat).UifDataTable({ source: controller });
                $("#editFieldForm").formReset();
                $('#modalEditFieldForm').UifModal('hide');
            } else {
                $("#alertDetail").UifAlert('show', Resources.SaveError, "danger");
            }
        });
    }
    else {
        editFieldStart("edit");
        $("#alertDetail").UifAlert('show', Resources.SaveSuccessfully, "success");
        var controller = ACC_ROOT + "Parameters/GetFormatFieldByFormatTypeId?formatTypeId=" + idFormatType;
        $(tableFieldFormat).UifDataTable({ source: controller });
        $("#editFieldForm").formReset();
        $('#modalEditFieldForm').UifModal('hide');
    }
}



function getFormatType(typeCode, tyepeDescription, modalForm) {
    var controller = ACC_ROOT + "Parameters/GetFormatTypeList";

    if (typeCode > 0) {
        while ($(modalForm).find(selectFormatType)[0].options.length > 0) {
            $(modalForm).find(selectFormatType)[0].remove(0);
        }
        $(modalForm).find(selectFormatType).append('<option value=' + typeCode + '>' + tyepeDescription + '</option>');
    }
    else {
        $(modalForm).find(selectFormatType).UifSelect({ source: controller });
    }
}