var oContractLine = {
    LineId: 0,
    ContractLines: []
};

var oContractLines = {
    ContractLineId: 0,
    Priority: 0,
    Contract: {}
};

var oContract = {
    ContractId: 0,
    Year: 0,
    ContractType: {}
};

var oContractType = {
    ContractTypeId: 0
};

$(document).ready(function () {
    searchContracts($("#ContractYear").val(), $("#ContractTypeId").val());
});

$('#modalContractLine').on('rowSelected', '#tableContract', function (event, selectedItem) {
    $("#msgNoEditContract").UifAlert('hide');
    $("#msgSelectOneContract").UifAlert('hide');
    $('#ContractId').val(selectedItem.ContractId);
    $("#ContractCode").val(selectedItem.ContractId);

    ValidateCompleteContract(selectedItem.ContractId).done(function (data) {
        if (data.success) {
            if (!data.result) {

                var value = {
                    label: 'ContractId',
                    values: [selectedItem.ContractId]
                }
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.ValidateCompleteContract, 'autoclose': true });
                $("#tableContract").UifDataTable('setUnselect', value);
            }
        }
    });
});

$('#modalContractLine').on('itemSelected', '#selectContractYear', function (event, selectedItem) {
    $("#msgNoEditContract").UifAlert('hide');
    $("#msgSelectOneContract").UifAlert('hide');
    searchContracts($('#selectContractYear').val(), $('#selectContractType').val());
});

$('#modalContractLine').on('itemSelected', '#selectContractType', function (event, selectedItem) {
    $("#msgNoEditContract").UifAlert('hide');
    $("#msgSelectOneContract").UifAlert('hide');
    searchContracts($('#selectContractYear').val(), $('#selectContractType').val());
});

$('#modalContractLine').on('click', '#btnSaveContractLine', function (event, selectedItem) {
    if ($('#ContractId').val() != contractId) {
        if ($("#formContractLine").valid()) {
            oContractType.ContractTypeId = $("#selectContractType").val();
            oContract.ContractId = $('#ContractId').val();
            oContract.Year = $("#selectContractYear").val();
            oContract.ContractType = oContractType;
            oContractLines.ContractLineId = $("#ContractLineId").val();
            oContractLines.Priority = $("#Priority").val();
            oContractLines.Contract = oContract;
            oContractLine.LineId = $("#LineDialogId").val();
            oContractLine.ContractLines.push(oContractLines);

            SaveContractLine(oContractLine).done(function (data) {
                if (data.success) {
                    onAddCompleteContractLine(data);
                }
            });
        }
    }
    else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ContractSelectedExist, 'autoclose': true });
    }

});

function searchContracts(year, contractTypeId) {
    if (year != undefined && contractTypeId != undefined) {
        GetContractsByYearAndContractTypeId(year, contractTypeId).done(function (data) {
            if (data.success) {
                $("#tableContract").UifDataTable({ sourceData: data.result });
                var value = { label: 'ContractId', value: contractId };
                $("#tableContract").UifDataTable('setSelect', value)
                $('#ContractId').val(contractId);
            }
        });
    }
}

function searchContractById(contractId) {
    if (contractId > 0) {
        GetContractById(contractId).done(function (data) {
            if (data.success) {
                $("#tableContract").UifDataTable({ sourceData: data.result });
            }
        })
    }
}

function GetContractsByYearAndContractTypeId(year, contractTypeId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractsByYearAndContractTypeId",
        data: JSON.stringify({ year: year, contractTypeId: contractTypeId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    });
}

function GetContractById(contractId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/GetContractById",
        data: JSON.stringify({ contractId: contractId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    });
}

function SaveContractLine(oContractLine) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/SaveContractLine",
        data: JSON.stringify({ line: oContractLine }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function ValidateCompleteContract(contractId) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/ValidateCompleteContract",
        data: JSON.stringify({ contractId: contractId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}
