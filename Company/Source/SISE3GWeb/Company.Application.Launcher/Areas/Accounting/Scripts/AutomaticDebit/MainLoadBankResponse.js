var time;

$('#LoadBankResponseLoadingDiv').hide();

////////////////////////////////////
/// Combo red                    ///
////////////////////////////////////
$('#loadBankResponseSelectNet').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        loadBankResponseRefreshGrid();

        var controller = ACC_ROOT + "Common/GetLotNumbersByBankNetworkId?bankNetworkId=" + $('#loadBankResponseSelectNet').val() + "&sendDate=" + $("#loadBankResponseSendDateTime").val() + "&statusCode=3";
        $("#loadBankResponseSelectLotNumber").UifSelect({ source: controller });
    }
    else {
        $("#loadBankResponseSelectLotNumber").UifSelect();
    }
});

//////////////////////////////////////
/// Fecha y hora de envio          ///
//////////////////////////////////////
$('#loadBankResponseSendDateTime').on("datepicker.change", function (event, date) {
    if (IsDate($('#loadBankResponseSendDateTime').val())) {
        $("#alert").UifAlert('hide');

        var controller = ACC_ROOT + "Common/GetLotNumbersByBankNetworkId?bankNetworkId=" + $('#loadBankResponseSelectNet').val() + "&sendDate=" + $("#loadBankResponseSendDateTime").val() + "&statusCode=3";
        $("#loadBankResponseSelectLotNumber").UifSelect({ source: controller });
    }
});

////////////////////////////////////
/// Botón Cargar                 ///
////////////////////////////////////
$('#btnBankResponseLoad').click(function () {
    $("#alert").UifAlert('hide');

    $("#formBankResponse").validate();

    if ($("#formBankResponse").valid()) {

        $("#btnBankResponseLoad").attr("disabled", "disabled");
        var fileType = $('input:radio[name=options]:checked').val();


        if ($('#loadBankResponseSelectNet').val() != "") {
            if ($('#loadBankResponseSelectLotNumber').val() != "" && $('#loadBankResponseSelectLotNumber').val()!= null) {
                if ($('#loadBankResponseSendDateTime').val() != "") {
                    uploadAjax(fileType);
                    $('#imageform').each(function () {
                        this.reset();
                    });
                }
                else {
                    $("#alert").UifAlert('show', Resources.EntrySendDateTime, "danger");
                    $('#imageform').each(function () {
                        this.reset();
                    });
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectLotNumber, "danger");
                $('#imageform').each(function () {
                    this.reset();
                });
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.SelectNet, "danger");
            $('#imageform').each(function () {
                this.reset();
            });
        }

        setTimeout(function () {
            $("#alert").UifAlert('hide');
            $("#btnBankResponseLoad").removeAttr("disabled");
        }, 3000);
    }

});

////////////////////////////////////
/// Botón Cancelar               ///
////////////////////////////////////
$('#btnBankResponseCancel').click(function () {
    $("#alert").UifAlert('hide');
    $('#loadBankResponseSelectNet').val("");
    $('#loadBankResponseSelectLotNumber').val("");
    $("#loadBankResponseOptionDebit").prop("checked", true);
    $("#loadBankResponseOptionNotification").prop("checked", false);
    $("#loadBankResponseSendDateTime").val($("#ViewBagCurrentDate").val());
    $("#loadBankResponseSelectLotNumber").UifSelect();
    $('#tableProcess').dataTable().fnClearTable();
});

/////////////////////////////////////////////////////////////////////////
/// Tabla procesos pendientes de carga respuesta banco                ///
/////////////////////////////////////////////////////////////////////////
$('#tableProcess').on('rowSelected', function (event, data, position) {
    if (data.RecordsFailed > 0) {
        var url = ACC_ROOT + "/AutomaticDebit/ExportToExcelErrorLoadBankResponse?bankNetworkId=" + data.BankNetworkId
                  + "&lotNumber=" + data.LotNumber;
        var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
        setTimeout(function () {
            newPage.open('', '_self', '');
        }, 100);
    }
});

////////////////////////////////////////
/// Funciones                        ///
////////////////////////////////////////
function uploadAjax(fileType) {
    
    $('#LoadBankResponseLoadingDiv').show();
    var inputFileImage = document.getElementById("loadBankResponseInputControlSingle");
    var file = inputFileImage.files[0];
    if (file == undefined) {
        $("#alert").UifAlert('show', Resources.SelectFile, "danger");
        setTimeout(function () {
            $("#alert").UifAlert('hide');
        }, 3000);
    }
    else {
        var data = new FormData();
        data.append('uploadedFile', file);
        data.append('bankNetworkId', $('#loadBankResponseSelectNet').val());
        data.append('lotNumber', $('#loadBankResponseSelectLotNumber option:selected').html());
        data.append('fileType', fileType);

        var url = ACC_ROOT + "AutomaticDebit/ReadFileInMemory";
        var validationPromise = new Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: false,
                data: data,
                processData: false,
                cache: false,
                success: function (data) {
                    resolve(data);
                }
            });
        });
        validationPromise.then(function (data) {
            if (data.length > 0) {
                if (data == "BadFileExtension") {
                    $("#alert").UifAlert('show', Resources.WrongFormatBlankColumns, "danger");
                }
                else if (data == "NegativeId") {
                    $("#alert").UifAlert('show', Resources.IdsNoNegative, "danger");
                }
                else if (data == "NotParameterizedFormat") {
                    $("#alert").UifAlert('show', Resources.WrongNotParametrizedDesignFormat, "danger");
                }
                else if (data == "NotAllowedPreNotification") {
                    $("#alert").UifAlert('show', Resources.WrongNotAllowedPreNotification, "danger");
                }
                else if (data == "SuccessfulLoadBankResponse") {
                    BankResponseProcess();
                }
                else if (data == "ErrorLoadBankResponse") {
                    $("#alert").UifAlert('show', Resources.WrongLoadBankResponseMistakes, "danger");
                }
                else if (data == "InvalidSeparator") {
                    $("#alert").UifAlert('show', Resources.InvalidSeparator, "warning");
                }
                else {
                    $("#alert").UifAlert('show', Resources.DocumentErrors, "danger");
                }
            }
            if (data.length == 0) {
                $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
            }
            $('#LoadBankResponseLoadingDiv').hide();
        });

    }
}

function BankResponseProcess() {

    var bankresponsePromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "AutomaticDebit/UpdateBankResponse",
            data: JSON.stringify({ "bankNetworkId": $('#loadBankResponseSelectNet').val(), "lotNumber": $('#loadBankResponseSelectLotNumber option:selected').html() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                resolve(data);
            }
        });
    });

    bankresponsePromise.then(function (data) {
        if (data[0].LotNumber > 0) {
            $("#alert").UifAlert('show', Resources.ProcessLoadBankResponse, "success");
            var controller = ACC_ROOT + "Common/GetLotNumbersByBankNetworkId?bankNetworkId=" +
                             $('#loadBankResponseSelectNet').val() + "&sendDate=" + $("#loadBankResponseSendDateTime").val() + "&statusCode=3";
            $("#loadBankResponseSelectLotNumber").UifSelect({ source: controller });
            time = window.setInterval(loadBankResponseRefreshGrid, 8000);
        }
        else {
            $("#alert").UifAlert('show', data[0].MessageError, "danger");
        }
        $("#btnBankResponseLoad").removeAttr("disabled");
    });

}

function loadBankResponseRefreshGrid() {
    var networkId = 0;

    if ($("#loadBankResponseSelectNet").val() == "") {
        networkId = 0;
    }
    else {
        networkId = $("#loadBankResponseSelectNet").val();
    }
    var controller = ACC_ROOT + "Common/GetDebitPendingProcess?bankNetworkId=" + parseInt(networkId) + "&processTypeId=6";
    $("#tableProcess").UifDataTable({ source: controller });
}