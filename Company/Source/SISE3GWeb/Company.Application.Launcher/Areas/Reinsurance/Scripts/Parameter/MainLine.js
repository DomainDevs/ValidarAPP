//LINEAS DE CONTRATO 
var lineId = 0;
var contractLineId = 0;


//ASOCIACION DE LINEAS
var lineId = 0;
var associationLineId = 0;
var associationColumnValueId = 0;

// LINEAS
var lineId = 0;
var contractId = 0;

$('#tableLine').on('rowSelected', function (event, selectedRow) {
    $('#alertLine').UifAlert('hide');
    $('#pnlContractLine').text(selectedRow.Description);
    lineId = parseInt(selectedRow.LineId);
    GetContractLineByLineId(lineId).done(function (data) {
        if (data.success) {
            $("#tableContractLine").UifDataTable({ sourceData: data.result });
        }
    });
});

$('#tableLine').on('rowAdd', function (event, data) {
    $('#alertLine').UifAlert('hide');
    $("#LineId").val(0);
    $('#modalLine').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddLine?lineId=0',
        Resources.LineNewTitle);
});

$('#tableLine').on('rowEdit', function (event, selectedRow) {
    $('#alertLine').UifAlert('hide');
    $('#modalLine').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddLine?lineId=' + selectedRow.LineId,
        Resources.LineEditTitle + ': ' + selectedRow.LineId);
});

$('#tableLine').on('rowDelete', function (event, data) {
    $('#alertLine').UifAlert('hide');
    $('#modalDeleteLine').UifModal('showLocal', Resources.Lines);
    lineId = data.LineId;
});

$("#modalDeleteLine").find("#btnDelete").click(function () {
    $('#modalDeleteLine').modal('hide');
    DeleteLine(lineId).done(function (data) {
        if (!data.success) {
            if (data.result != "") {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateNoDeleteLine, 'autoclose': true });
            }
        }
        else {
            lineId = 0;
            $('#tableLine').UifDataTable();
            $("#tableContractLine").UifDataTable('clear');
        }
    });
});

function onAddComplete(data) {
    if (data.success) {
        lineId = 0;
        $('#tableLine').UifDataTable();
        $('#modalLine').UifModal('hide');
    } else {
        $('#modalConfirm').appendTo("body").UifModal('showLocal');
    }
}

//LINEAS DE CONTRATO

$('#tableContractLine').on('rowAdd', function (event, data) {
    $('#alertMainLineContract').UifAlert('hide');
    if (lineId > 0) {
        $('#modalContractLine').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddContractLine?contractLineId=0' + "&lineId=" + lineId,
            Resources.ContractLine);
    } else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.LineSelect, 'autoclose': true });
    }
});

$('#tableContractLine').on('rowEdit', function (event, selectedRow) {
    $('#alertLine').UifAlert('hide');
    contractId = selectedRow.ContractId;

    $('#modalContractLine').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddContractLine?contractLineId=' +
        selectedRow.ContractLineId + "&lineId=" + selectedRow.LineId, Resources.ContractLine + ': ' + selectedRow.LineId);

    searchContracts($("#ContractYear").val(), $("#ContractTypeId").val());
});

$("#tableContractLine").on('rowDelete', function (event, selectedRow) {
    $('#alertLine').UifAlert('hide');
    contractLineId = selectedRow.ContractLineId;
    LineIsUsed(lineId).done(function (data) {
        if (data.success) {
            if (data.result) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.LineIsUsed, 'autoclose': true });
            }
            else {
                $('#modalDeleteContractLine').UifModal('showLocal', Resources.ContractLine);
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        }
    });
});

$("#modalDeleteContractLine").find("#btnDeleteContractLine").click(function () {
    $('#modalDeleteContractLine').modal('hide');
    DeleteContractLine(contractLineId, lineId).done(function (data) {
        if (!data.success) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateNoDeleteContractLine, 'autoclose': true });
        }
        else {
            GetContractLineByLineId(lineId).done(function (response) {
                if (response.success) {
                    $("#tableContractLine").UifDataTable({ sourceData: response.result });
                }
            });
        }
    });
});

function onAddCompleteContractLine(data) {
    if (data.success) {
        $('#modalContractLine').UifModal('hide');
        GetContractLineByLineId(lineId).done(function (data) {
            if (data.success) {
                $("#tableContractLine").UifDataTable({ sourceData: data.result });
            }
        });
    }
    else if (data == 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateNoContract, 'autoclose': true });
    }
    else if (!data.success) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ContractSelectedExist, 'autoclose': true });
    }
}

//ASOCIACION DE LINEAS

$('#msgValidLineAssociationLine').hide();

$('#tableAssociationLine').on('rowAdd', function (event, data) {
    if (lineId > 0) {
        $('#modalAssociationLine').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddAssociationLine?lineId=' + lineId
            + '&associationLineId=0' + '&associationColumnValueId=0' + '&associationColumnId=0'
            + '&associationTypeId=0', Resources.AssociationOfLines);
    } else {
        $('#msgValidLineAssociationLine').show();
    }
});

$('#tableAssociationLine').on('rowEdit', function (event, selectedRow) {
    $('#modalAssociationLine').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddAssociationLine?lineId=' + selectedRow.LineId
        + '&associationLineId=' + selectedRow.AssociationLineId + '&associationColumnValueId=' + selectedRow.AssociationColumnValueId
        + '&associationColumnId=' + selectedRow.AssociationColumnId + '&associationTypeId=' + selectedRow.AssociationTypeId,
        Resources.LineEditTitle + ': ' + selectedRow.LineId);
});

$('#tableAssociationLine').on('rowDelete', function (event, data) {
    associationLineId = data.AssociationLineId;
    associationColumnValueId = data.AssociationColumnValueId;
    $('#modalDeleteAssociationLine').appendTo("body").UifModal('showLocal');
});

function DeleteLine(lineId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteLine",
        data: JSON.stringify({ lineId: lineId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function GetContractLineByLineId(lineId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractLineByLineId",
        data: JSON.stringify({ lineId: lineId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function DeleteContractLine(contractLineId, lineId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/DeleteContractLine",
        data: JSON.stringify({ contractLineId: contractLineId, lineId: lineId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function LineIsUsed(lineId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/LineIsUsed",
        data: JSON.stringify({ lineId: lineId }),
        dataType: "json",
        contentType: "application/json; charse=utf-8"
    });
}
