var selectDayPrefix = '#selectDayPrefix';
var selectPrefixValue = $("select#selectDayPrefix");
var tableDayByPrefix = '#tableDayByPrefix';
var idCancellationLimit = 0;
var editPosition = -1;

$(tableDayByPrefix).on('rowAdd', function (event, data) {
    $("#alertDayPrefix").UifAlert('hide');
    $('#alertDayPrefixModal').UifAlert('hide');
    bindgPrefix(0, '', "#modalAddDayPrefixForm");
    $('#modalAddDayPrefixForm').UifModal('showLocal', Resources.AddDayPrefix);
});

$(tableDayByPrefix).on('rowEdit', function (event, data, position) {
    $("#alertDayPrefix").UifAlert('hide');
    $('#alertDayPrefixModal').UifAlert('hide');

    idCancellationLimit = data.Id;
    editPosition = position;

    bindgPrefix(data.PrefixCode, data.PrefixDescription, "#editDayPrefixForm");
    $(editDayPrefixForm).find("#DaysCancelation").val(data.Days);
    
    $('#modalEditDayPrefix').UifModal('showLocal', Resources.EditTemplate);

});

$(tableDayByPrefix).on('rowDelete', function (event, data) {
    $('#alertDayPrefixModal').UifAlert('hide');
    $('#alertDayPrefix').UifAlert('hide');
    idCancellationLimit = data.Id;
    $('#modalDeleteDayPrefixForm').appendTo("body").modal('show');
});

$(selectDayPrefix).on('itemSelected', function (event, selectedItem) {
    $("#alertDayPrefix").UifAlert('hide');
    $('#alertDayPrefixModal').UifAlert('hide');

    var fields = $(tableDayByPrefix).UifDataTable('getData');
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if (selectedItem.Id == fields[j].PrefixCode) {
                $(selectDayPrefix).val("");
                $("#alertDayPrefixModal").UifAlert('show', Resources.MessageExistDayPrefix, "warning");
                break;
            }
        }
    }
});

$('#saveAddDayPrefix').click(function () {
    $("#addFoaddDayPrefixFormrmType").validate();
    if ($("#addDayPrefixForm").valid()) {
        saveCancellationLimit();
       
    }
});

$('#saveEditDayPrefix').click(function () {
    $("#editDayPrefixForm").validate();
    if ($("#editDayPrefixForm").valid()) {
        var cancelationLimitData = CancellationLimitControl("#editDayPrefixForm");

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/UpdateCancellationLimit",
            data: JSON.stringify({ "cancellationLimit": cancelationLimitData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#alertDayPrefix").UifAlert('show', Resources.EditSuccessfully, "success");
                    var controller = ACC_ROOT + "Parameters/GetCancellationLimits";
                    $(tableDayByPrefix).UifDataTable({ source: controller });
                }
                else {
                    $("#alertDayPrefix").UifAlert('show', Resources.ErrorTransaction, "danger");
                }
            }
        });
        $("#editDayPrefixForm").formReset();
        $('#modalEditDayPrefix').UifModal('hide');
     }
});

$("#deleteDayPrefixForm").on('click', function () {
    $('#modalDeleteDayPrefixForm').modal('hide');

    var cancellationLimit = DeleteCancellationLimit();

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteCancellationLimit",
        data: JSON.stringify({ "cancellationLimit": cancellationLimit }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#alertDayPrefix").UifAlert('show', Resources.DeleteSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetCancellationLimits";
            $(tableDayByPrefix).UifDataTable({ source: controller });
        }
        else {
            $("#alertDayPrefix").UifAlert('show', data.result, "danger");
        }
    });
});

/*FUNCIONES*/
function bindgPrefix(prefixCode, prefixDescription, modalForm) {
    var controller = ACC_ROOT + "Common/GetPrefixes";
    
    if (prefixCode > 0) {
        while ($(modalForm).find(selectDayPrefix)[0].options.length > 0) {
            $(modalForm).find(selectDayPrefix)[0].remove(0);
        }
        $(modalForm).find(selectDayPrefix).append('<option value=' + prefixCode + '>' + prefixDescription + '</option>');
    }
    else {
        $(modalForm).find(selectDayPrefix).UifSelect({ source: controller });
    }
   

}

function saveCancellationLimit() {
    var cancelationLimitData = CancellationLimitControl("#addDayPrefixForm");
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveCancellationLimit",
        data: JSON.stringify({ "cancellationLimit": cancelationLimitData }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#alertDayPrefix").UifAlert('show', Resources.SaveSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetCancellationLimits";
            $(tableDayByPrefix).UifDataTable({ source: controller });
        } else {
            $("#alertDayPrefix").UifAlert('show', Resources.SaveError, "danger");
        }
        $("#addDayPrefixForm").formReset();
        $('#modalAddDayPrefixForm').UifModal('hide');
    });
}

function CancellationLimitControl(modalForm) {
    var listSalesPoint = {
        SalePoints: []
    };
    var field = {
        Id: 0,
        Description: "",
    }
    listSalesPoint.SalePoints.push(field);

    var branch = {
        Id: $(modalForm).find(selectDayPrefix).val(),
        Description: $(modalForm).find("#selectDayPrefix option:selected").html(),
        SalePoints: listSalesPoint.SalePoints
    }
    
    return {
        Id: idCancellationLimit,
        Branch: branch,
        CancellationLimitDays: $(modalForm).find("#DaysCancelation").val(),
    }
};

function DeleteCancellationLimit() {
    var listSalesPoint = {
        SalePoints: []
    };
    var field = {
        Id: 0,
        Description: "",
    }
    listSalesPoint.SalePoints.push(field);

    var branch = {
        Id: 0,
        Description: "",
        SalePoints: listSalesPoint.SalePoints
    }

    return {
        Id: idCancellationLimit,
        Branch: branch,
        CancellationLimitDays: 0,
    }
};