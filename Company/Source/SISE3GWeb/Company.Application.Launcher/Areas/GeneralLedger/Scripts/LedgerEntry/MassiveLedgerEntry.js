/**
    * @file   MassiveLedgerEntry.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var documentFormatType = 0;

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MassiveLedgerEntry();
});

class MassiveLedgerEntry extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        $("#GenerateLedgerEntry").hide();
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#uploadFile").on("click", this.UploadExcelFile);
        $("#uploadFile").on('change', this.ChangeInputFile);
        $("#GenerateLedgerEntry").on("click", this.GenerateLedgerEntry);
    }

    /**
        * Permite leer el archivo de asientos de mayor.
        *
        */
    UploadExcelFile() {

        $("#uploadFileForm").validate();

        if ($("#uploadFileForm").valid()) {
            $("#alert").UifAlert('hide');
            $("#tableFailedRecords").UifDataTable('clear');

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

            MassiveLedgerEntry.UploadAjaxMassiveEntry();
        } else {
            $("#alert").UifAlert('show', Resources.UploadFile, "danger");
            $('#imageform').each(function () {
                this.reset();
            });
        }
    }

    /**
        * Permite generar el asiento una vez validado el archivo.
        *
        */
    GenerateLedgerEntry() {
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
            url: GL_ROOT + "LedgerEntry/GenerateEntry",
            type: 'POST',
            contentType: false,
            processData: false,
            cache: false,
            success: function (data) {
                $("#GenerateLedgerEntry").hide();

                if (data.success == false) {
                    $("#alert").UifAlert('show', data.result, 'danger');
                } else if (data > 0) {
                    $("#alert").UifAlert('show', Resources.EntrySuccessfullySaved + " " + data, 'success');
                }
                $.unblockUI();
            }
        });
    }

    /**
        * Permite limpiar el inputFile después de cargar el archivo.
        *
        */
    ChangeInputFile() {
        $(this).get(0).value = '';
        $(this).get(0).type = '';
        $(this).get(0).type = 'file';
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Lee y valida el archivo excel.
        *
        */
    static UploadAjaxMassiveEntry() {
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

            var url = GL_ROOT + "LedgerEntry/ReadFileInMemory";
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
                            //Se obtiene los resultados del proceso.
                            $.ajax({
                                url: GL_ROOT + "LedgerEntry/GetEntryMassiveLoadRecords",
                                type: 'POST',
                                contentType: false,
                                processData: false,
                                cache: false,
                                success: function (data) {

                                    $("#alert").UifAlert('show', Resources.RecordsNumber + ": " + data[0].TotalRecords + ", " +
                                        Resources.Successful + ": " + data[0].SuccessfulRecords + ", " +
                                        Resources.Failed + ": " + data[0].FailedRecords, 'success');
                                    if (data[0].FailedRecords > 0) {
                                        $("#GenerateLedgerEntry").hide();
                                        MassiveLedgerEntry.LoadFailedRecords();
                                    }
                                    if (data[0].TotalRecords == data[0].SuccessfulRecords) {
                                        //Muestra el botón de generación de asiento
                                        $("#GenerateLedgerEntry").show();
                                    }
                                    //MassiveLedgerEntry.CleanInputFile("inputControlSingle");
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
                        if (data[0] == "BadFileExtension") {
                            $("#alert").UifAlert('show', Resources.WrongFormatBlankRows, 'danger');
                        }

                        //$("#progressBarPanel").hide();
                    }
                    $.unblockUI();
                }
            });
        }
    }

    /**
        * Carga los registros fallidos, es decir, los que no pasaron la validación.
        *
        */
    static LoadFailedRecords() {
        var controller = GL_ROOT + "LedgerEntry/GetMassiveEntryFailedRecords";
        $('#tableFailedRecords').UifDataTable({ source: controller });
    }

    /**
        * Permite limpiar el contenido del inputFile.
        *
        * @param {String} id - Nombre del control inputFile.
        */
    static CleanInputFile(tagId) {
        $("#inputControlSingle").val(null);
        $("#inputControlSingle").replaceWith($("#inputControlSingle").val('').clone(true));
        //document.getElementById(tagId).innerHTML = document.getElementById(tagId).innerHTML;
        document.getElementById("divUploadFile").innerHTML = document.getElementById("divUploadFile").innerHTML;

        /*
        var input = $('#' + id);
        var clon = input.clone();  // Creamos un clon del elemento original
        input.replaceWith(clon);   // Y sustituimos el original por el clon
        //input.replaceWith(input.val('').clone(true));
        */
        //$("#inputControlSingle").closest('form').trigger('reset');
    }
}
