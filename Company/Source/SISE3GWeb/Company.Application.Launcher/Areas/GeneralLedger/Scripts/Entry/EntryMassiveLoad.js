/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

$("#progressBarPanel").hide();
$("#showFailed").hide();
$("#generateEntry").hide();
$("#failedRecordsPanel").hide();

var documentFormatType = 0;

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

//botón de carga masiva
$('#uploadFile').click(function () {
    $("#progressBarPanel").hide();
    $("#showFailed").hide();
    $("#generateEntry").hide();
    $("#failedRecordsPanel").hide();

    $("#uploadFileForm").validate();

    if ($("#uploadFileForm").valid()) {
        $("#progressBarPanel").show();
        $("#alert").UifAlert('hide');

        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });

        uploadAjaxEntryMassive();
    } else {
        $("#alert").UifAlert('show', Resources.UploadFile, "danger");
        $('#imageform').each(function () {
            this.reset();
        });
    }
});

//click para mostrar registros fallidos
$("#showFailed").click(function () {
    var controller = GL_ROOT + "Entry/GetMassiveEntryFailedRecords";
    $('#failedRecordsList').UifDataTable({ source: controller });
    $("#failedRecordsPanel").show();
});

//click para generar el asiento
$("#generateEntry").click(function () {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: "<h1>" + Resources.MessageWaiting + "</h1>"
    });

    $.ajax({
        url: GL_ROOT + "Entry/GenerateEntry",
        type: 'POST',
        contentType: false,
        processData: false,
        cache: false,
        success: function (data) {
            $("#generateEntry").hide();

            if (data.success == false) {
                //$("#alert").UifAlert('show', Resources.EntrySavedFailed, 'danger');
                $("#alert").UifAlert('show', data.result, 'danger');
            } else if (data > 0) {
                $("#alert").UifAlert('show', Resources.EntrySuccessfullySaved + " " + data, 'success');
            }
            $.unblockUI();
        }
    });
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function uploadAjaxEntryMassive() {
    $('div#container').show();
    var inputFileImage = document.getElementById("inputControlSingle");
    var file = inputFileImage.files[0];
    if (file == undefined) {
        $("#alert").UifAlert('show', Resources.UploadFile, "danger");
    }
    else {
        var data = new FormData();
        data.append('uploadedFile', file);
        data.append('fileType', documentFormatType);

        var url = GL_ROOT + "Entry/ReadFileInMemory";
        $.ajax({
            url: url,
            type: 'POST',
            contentType: false,
            data: data,
            processData: false,
            cache: false,
            success: function (data) {
                if (data.length > 0) {
                    if (data[1]) {
                        //obtengo los resultados del proceso.
                        $.ajax({
                            url: GL_ROOT + "Entry/GetEntryMassiveLoadRecords",
                            type: 'POST',
                            contentType: false,
                            processData: false,
                            cache: false,
                            success: function (data) {

                                $("#alert").UifAlert('show', Resources.RecordsNumber + ": " + data.TotalRecords + ", " +
                                    Resources.Successful + ": " + data.SuccessfulRecords + ", " +
                                    Resources.Failed + ": " + data.FailedRecords, 'success');
                                if (data.FailedRecords > 0) {
                                    //mostrar botón para registro de errores
                                    $("#showFailed").show();
                                    $("#generateEntry").hide();
                                }
                                if (data.TotalRecords == data.SuccessfulRecords) {
                                    //muestra el botón de generación de asiento
                                    $("#generateEntry").show();
                                    $("#showFailed").hide();
                                }
                            }
                        });
                    }
                    if (data[0] == "NoBranchId") {
                        $("#alert").UifAlert('show', Resources.NoBranchValidation, 'danger');
                    }
                    if (data[0] == "NoSalePointId") {
                        $("#alert").UifAlert('show', Resources.NoSalePointValidation, 'danger');
                    }
                    if (data[0] == "NoAccountingCompanyId") {
                        $("#alert").UifAlert('show', Resources.NoAccountingCompanyValidation, 'danger');
                    }
                    if (data[0] == "NoEntryDestinationId") {
                        $("#alert").UifAlert('show', Resources.NoEntryDestinationValidation, 'danger');
                    }
                    if (data[0] == "NoAccountingMovementTypeId") {
                        $("#alert").UifAlert('show', Resources.NoAccountingMovementTypeValidation, 'danger');
                    }
                    if (data[0] == "NoOperationDate") {
                        $("#alert").UifAlert('show', Resources.NoOperationDateValidation, 'danger');
                    }
                    if (data[0] == "NoCurrencyId") {
                        $("#alert").UifAlert('show', Resources.NoCurrencyValidation, 'danger');
                    }
                    if (data[0] == "NoAccountingNature") {
                        $("#alert").UifAlert('show', Resources.NoAccountingNatureValidation, 'danger');
                    }
                    if (data[0] == "Unbalanced") {
                        $("#alert").UifAlert('show', Resources.UnbalancedEntry, 'danger');
                    }
                    if (data[0] == "IncompleteHeader") {
                        $("#alert").UifAlert('show', Resources.IncompleteHeaderValidation, 'danger');
                    }
                    if (data[0] == "Exception") {
                        $("#alert").UifAlert('show', Resources.Exception, 'danger');
                    }

                    $("#progressBarPanel").hide();
                }
                $.unblockUI();
            }
        });
    }
}



