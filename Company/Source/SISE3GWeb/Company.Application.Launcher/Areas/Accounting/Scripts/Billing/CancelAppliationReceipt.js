

$("#DeleteTempApplicationItem").click(function () {
    const selected = $("#TemApplicationsTable").UifDataTable('getSelected');
    if (selected != null) {
        showConfirm(selected);
    }
});

function showConfirm(selected) {
    $.UifDialog('confirm', { 'message': Resources.ConfirmDelete, 'title': Resources.DeleteRecord }, function (result) {
        if (result === true) {

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "ReceiptApplication/CancelAppliationReceiptByTempImputationId",
                data: { "tempImputationId": selected[0].TempAppId },
                success: function (data) {

                    if (data.result == true && data.success == true) {
                        LoadTempApplicationsData();
                        $.UifNotify('show', {
                            'type': 'info', 'message': Resources.DeleteSucces + ' ' + Resources.DialogTitleTemporary + selected[0].TempAppId, 'autoclose': true
                        });

                    } else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': data.result, 'autoclose': true
                        });
                    }
                }
            });
        }

    });
};



function LoadTempApplicationsData() {
    tempApplicationToDrop();

    tempApplicationToDropPromise.then(function (tempApplicationsData) {
        $("#TemApplicationsTable").UifDataTable('clear');
        for (var i = 0; i <= tempApplicationsData.aaData.length - 1; i++) {
            $("#TemApplicationsTable").UifDataTable('addRow', tempApplicationsData.aaData[i]);
        }

    });
}



function tempApplicationToDrop() {
    return tempApplicationToDropPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Billing/GetTempApplicationsByUserId",
        }).done(function (tempApplicationsData) {
            resolve(tempApplicationsData);
        });
    });
}

$(document).ready(function () {
    LoadTempApplicationsData();
});