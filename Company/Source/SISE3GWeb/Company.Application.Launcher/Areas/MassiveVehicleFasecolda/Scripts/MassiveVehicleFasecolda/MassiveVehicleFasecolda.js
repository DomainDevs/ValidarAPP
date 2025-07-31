var steps1 = {};
var steps2 = {};
var processId = 0;

class MassiveVehicleFasecolda extends Uif2.Page {
    getInitialState() {
        $('#table').hide();

        steps1 = uif2.steps({
            container: '#wizardCodes'
        });
        steps1 = uif2.steps({
            container: '#wizardValues'
        });

        MassiveVehicleFasecolda.LoadStateItems();

        /*wizard*/
        $('#wizardCodes').hide();
        $('#wizardValues').hide();
    }

    bindEvents() {
        /*Change*/
        $('#inputCodigoFile').on('change', this.UploadFileMassiveCodes);
        $('#inputValueFile').on('change', this.UploadFileMassiveValues);
        $('#inputTemporal').on("search", function (event, value) {
            MassiveVehicleFasecolda.GetVehicleFasecoldaByProccessId();
        });
        $("#inputTemporal").on('keypress', MassiveVehicleFasecolda.OnlyNumbers);

        /*Upload*/
        $('#inputValueFile').fileupload({});
        $('#inputCodigoFile').fileupload({});

        /*Click Events*/
        $('#btnLoad').click(this.GenerateLoadMassiveVehicleFasecolda);
        $('#btnAddFiles').click(MassiveVehicleFasecolda.UploadFileMassiveVehicleFasecolda);
        $('#btnNew').click(MassiveVehicleFasecolda.NewLoadFile);
        $("#btnExit").click(MassiveVehicleFasecolda.Exit);
        $('#btnErrors').click(MassiveVehicleFasecolda.GetErrorExcelProcessVehicleFasecolda);
        $('#btnProcess').click(MassiveVehicleFasecolda.GenerateProccessMassiveVehicleFasecolda);
        
    }

    static ShowProcessByState(State) {
        $('#stepCodes1').hide();
        $('#stepCodes2').hide();
        $('#stepValues1').hide();
        $('#stepValues2').hide();

        $('#wizardCodes').show();
        $('#wizardValues').show();
        $('#stepCodes' + State).show();
        $('#stepValues' + State).show();
        $('#stepsFasecoldaCodes').show();
        $('#stepsFasecoldaValues').show();
    }

    static LoadStateItems() {
        $("#stepsFasecolda").hide();
        $('#btnAddFiles').prop('disabled', true);

        $('#stepsFasecoldaCodes').hide();
        $('#stepsFasecoldaValues').hide();

        MassiveVehicleFasecolda.Controls(true);
    }

    static Controls(state) {
        
        $("#btnErrors").prop('disabled', state);
        $("#btnLoad").prop('disabled', state);
        $("#btnProcess").prop('disabled', state);

        if (state) {
            $('#wizardCodes').hide();
            $('#wizardValues').hide();
            $('#stepsFasecoldaCodes').hide();
            $('#stepsFasecoldaValues').hide();
        }
        else {
            $('#wizardCodes').show();
            $('#wizardValues').hide();
            $('#stepsFasecoldaCodes').hide();
            $('#stepsFasecoldaValues').hide();
        }
    }

    static GetVehicleFasecoldaByProccessId() {
        $("#btnErrors").prop('disabled', false);
        processId = $('#inputTemporal').val();
        MassiveVehicleFasecoldaRequest.GetVehicleFasecoldaByProccessId($('#inputTemporal').val()).done(function (data) {
            if (data.success) {
                if (data.result[0].TotalRows > 0 || data.result[1].TotalRows > 0) {
                    MassiveVehicleFasecolda.LoadTableSummary(data)
                } else {
                    $.UifNotify('show', {'type': 'info', 'message': 'No se encontraron cargues', 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadTableSummary(data) {
        let selector = ["Values", "Codes"]
        let fileType = ["Valores", "Codigos"]

        for (var i = 0; i < selector.length; i++) {
            $('#input' + selector[i] + 'TotalRecords').val(data.result.find(function (data) { return data.TypeFile == i + 1 }).TotalRows);
            $('#input' + selector[i] + 'Processed').val(data.result.find(function (data) { return data.TypeFile == i + 1 }).TotalRowsProcesseds);
            $('#input' + selector[i] + 'Pending').val(data.result.find(function (data) { return data.TypeFile == i + 1 }).Pendings);
            $('#input' + selector[i] + 'Errors').val(data.result.find(function (data) { return data.TypeFile == i + 1 }).TotalRowsLoaded);
            $('#stepsFasecolda' + selector[i]).show();
            MassiveVehicleFasecolda.ShowProcessByState(data.result.find(function (data) { return data.TypeFile == i + 1 }).Status);

            if (data.result[i].EnableProcessing == 1) {
                $('#btnProcess').prop('disabled', false);
            } else {
                $('#btnProcess').prop('disabled', true);
            }
            
            if ($('#input' + selector[i] + 'Errors').val() != '0') {
                $.UifNotify('show', { 'type': 'info', 'message': 'El archivo de ' + fileType[i] + ' tiene errores, descargue los errores para mayor información' , 'autoclose': true });
            }
        }

        $('.number')[0].innerText = "1";
        $('.number')[1].innerText = "2";
    }

    static GenerateProccessMassiveVehicleFasecolda() {
        if (processId > 0) {
            MassiveVehicleFasecoldaRequest.GenerateProccessMassiveVehicleFasecolda(processId).done(function (data) {
                if (data.success) {
                    console.log(data);
                    MassiveVehicleFasecolda.ShowMessageByUploadFile({ ProcessStatus: data.ProcessStatusType });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterParameterSearch, 'autoclose': true });
        }
    }

    static UploadFileMassiveVehicleFasecolda() {
        MassiveVehicleFasecolda.Controls(true);
        MassiveVehicleFasecolda.ValidateFileStructureMassiveVehicle();
    }

    static ShowMessageByUploadFile(process) {
        switch (process.ProcessStatus) {
            case 1:
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Validating, 'autoclose': true });
                break;
            case 2:
                $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.SuccessfullyValidated, 'autoclose': true });
                $("#btnLoad").prop('disabled', false);
                $('#btnAddFiles').prop('disabled', true);
                break;
            case 3:
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ValidatedWithErrors, 'autoclose': true });
                break;
            case 4:
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Loading, 'autoclose': true });
                break;
            case 5:
                $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.Loaded, 'autoclose': true });
                break;
            case 6:
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Processing, 'autoclose': true });
                break;
            case 7:
                $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.FinishedProcess, 'autoclose': true });
                break;
        };
    }

    static ValidateFileStructureMassiveVehicle() {
        $('#formVehicleFasecolda').validate();
        if ($('#formVehicleFasecolda').valid()) {
            var nameFileCode = $('#inputCodigoFaseColda').val();
            var nameFileValue = $('#inputValueFaseColda').val();
            let extensionFileCode = nameFileCode.split('.');
            let extensionFileValue = nameFileValue.split('.');
            nameFileCode = nameFileCode.split('_');
            nameFileValue = nameFileValue.split('_');
            
            if (nameFileCode.length > 2 && nameFileValue.length > 2) {
                if ((extensionFileCode[extensionFileCode.length - 1] == "txt") && (extensionFileValue[extensionFileValue.length - 1] == "txt")) {
                    if (nameFileCode[2].split('.')[0] == nameFileValue[2].split('.')[0]) {
                        if (nameFileCode[0].toLowerCase().indexOf("codigos") > 0 && nameFileValue[0].toLowerCase().indexOf("valores") > 0) {
                            MassiveVehicleFasecolda.ShowMessageByUploadFile({ ProcessStatus: 2 });
                        } else {
                            $.UifNotify('show', { 'type': 'danger', 'message': "El formato del nombre no coincide con lo requerido", 'autoclose': true });
                            MassiveVehicleFasecolda.Controls(true);
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': "El número de guia no coincide", 'autoclose': true });
                        MassiveVehicleFasecolda.Controls(true);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': "El archivo debe ser de tipo texto", 'autoclose': true });
                    MassiveVehicleFasecolda.Controls(true);
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "El formato del nombre no coincide con el solicitado", 'autoclose': true });
                MassiveVehicleFasecolda.Controls(true);
            }
        }
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static GetErrorExcelProcessVehicleFasecolda() {
        if (processId > 0) {
            MassiveVehicleFasecoldaRequest.GetErrorExcelProcessVehicleFasecolda(processId).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result, true, (url) => url.match(/([^\\]+.xlsx)/));
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EnterParameterSearch, 'autoclose': true });
        }
    }

    static NewLoadFile() {
        $('#inputCodigoFaseColda').val(null)
        $('#inputCodigoFile').data('Object', null);
        $('#progress-bar-code').css('width', 0 + '%');

        $('#inputValueFaseColda').val(null);
        $('#inputValueFile').data('Object', null);
        $('#progress-bar-value').css('width', 0 + '%');

        $('#inputTemporal').val("");
        $('#inputTemporal').prop('disabled', false);
        $('#stepCodes1').hide();
        $('#stepCodes2').hide();
        $('#stepsFasecoldaCodes').hide();

        $('#btnAddFiles').prop('disabled', true);
        MassiveVehicleFasecolda.Controls(true);
    }

    UploadFileMassiveCodes(event) {
        if (event.target.files.length > 0) {
            if ($('#formVehicleFasecolda').valid()) {
                $(this).fileupload({
                    dataType: 'json',
                    url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/UploadFileMassiveVehicleFasecolda',
                    done: function (event, data) {
                        if (data.result.success) {

                            if (data.files) {
                                $('#inputCodigoFile').data('Object', data.result.result);
                                $('#inputCodigoFaseColda').val(data.files[0].name)
                                $('#progress-bar-code').css('width', 100 + '%');
                            }

                            if ($('#inputValueFaseColda').val() != "")
                                $('#btnAddFiles').prop('disabled', false);

                            $('#inputTemporal').val('');
                            $('#inputTemporal').prop('disabled', false);
                            MassiveVehicleFasecolda.Controls(true);
                        }
                        else {
                            $('#inputCodigoFile').data('Object', null);
                            $('#progress-bar-code').css('width', 0 + '%');
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.result, 'autoclose': true });
                        }
                    }
                })
            }
            else {
                $('.bootstrap-filestyle > input').val('');
                $('#progress-bar-code').css('width', 0 + '%');
            }
        }
        else {
            $('.bootstrap-filestyle > input').val('');
            $('#progress-bar-code').css('width', 0 + '%');
        }
    }

    UploadFileMassiveValues() {
        if (event.target.files.length > 0) {
            if ($('#formVehicleFasecolda').valid()) {
                $('#progress-bar-value').css('width', 0 + '%');
                $(this).fileupload({
                    url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/UploadFileMassiveVehicleFasecolda',
                    dataType: 'json',
                    done: function (event, data) {
                        if (data.result.success) {
                            if (data.files) {
                                console.log(data);
                                $('#inputValueFile').data('Object', data.result.result);
                                $('.bootstrap-filestyle > input').val(data.files[0].name);
                                $('#inputValueFaseColda').val(data.files[0].name);
                                $('#progress-bar-value').css('width', 100 + '%');
                            }

                            if ($('#inputCodigoFaseColda').val() != "")
                                $('#btnAddFiles').prop('disabled', false);

                            $('#inputTemporal').val('');
                            $('#inputTemporal').prop('disabled', false);
                            MassiveVehicleFasecolda.Controls(true);
                        }
                        else {
                            $('#inputCodigoFile').data('Object', null);
                            $('#progress-bar-value').css('width', 0 + '%');
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.result, 'autoclose': true });
                        }
                    }
                })
            }
            else {
                $('.bootstrap-filestyle > input').val('');
                $('#progress-bar-value').css('width', 0 + '%');
            }
        }
        else {
            $('.bootstrap-filestyle > input').val('');
            $('#progress-bar-value').css('width', 0 + '%');
        }
    }

    GenerateLoadMassiveVehicleFasecolda() {
        $('#formVehicleFasecolda').validate();
        if ($('#formVehicleFasecolda').valid()) {
            MassiveVehicleFasecolda.ControlsLoading(true);
            
            var fasecoldaDTO = $('#formVehicleFasecolda').serializeObject();
            var FileName = $('#inputValueFile').data('Object');
            var LoadName = $('#inputValueFaseColda').val().split('_')[2].split('.')[0];

            fasecoldaDTO = BuildCompanyFasecolaDTO(FileName, LoadName, 1);

            MassiveVehicleFasecolda.ShowMessageByUploadFile({ ProcessStatus: 4 });

            $("#btnExit").prop('disabled', true);
            
            MassiveVehicleFasecoldaRequest.CreateLoadMassiveVehicleFasecolda(fasecoldaDTO).done(function (dataValue) {
                if (dataValue.success) {
                    console.log(dataValue)
                    FileName = $('#inputCodigoFile').data('Object');
                    LoadName = $('#inputCodigoFaseColda').val().split('_')[2].split('.')[0];

                    fasecoldaDTO = BuildCompanyFasecolaDTO(FileName, LoadName, 2);
                    fasecoldaDTO.ProcessId = dataValue.result.ProcessId;

                    MassiveVehicleFasecoldaRequest.CreateLoadMassiveVehicleFasecolda(fasecoldaDTO).done(function (data) {
                        if (data.success) {
                            processId = data.result.ProcessId;
                            $.UifDialog('alert', {
                                'message': 'Se ha realizado el cargue ' + processId + ' exitosamente'
                            }, null)

                            MassiveVehicleFasecolda.GetVehicleFasecoldaByProccessId();
                            MassiveVehicleFasecolda.ShowMessageByUploadFile({ ProcessStatus: 5 });

                            MassiveVehicleFasecolda.ControlsLoading(false);

                            $('#btnLoad').prop('disabled', true);
                            $('#inputTemporal').val(processId);
                            $("#btnExit").prop('disabled', false);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            $("#btnExit").prop('disabled', false);
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': dataValue.result, 'autoclose': true });
                    $("#btnExit").prop('disabled', false);
                }
            });
        }
    }

    static ControlsLoading(disabled) {
        $('#btnLoad').prop('disabled', disabled);
        $('#btnNew').prop('disabled', disabled);
        $('#inputValueFile').prop('disabled', disabled);
        $('#inputCodigoFile').prop('disabled', disabled);
        $('#inputTemporal').prop('disabled', disabled);
    }

    static OnlyNumbers(event) {
        if (event.keyCode !== 8 && event.keyCode !== 46 && event.keyCode !== 37 && event.keyCode !== 39 && event.keyCode !== 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } 
        }
    }
}

function BuildCompanyFasecolaDTO(FileName, LoadName, typeFile) {
    var fasecoldaDTO =
        {
            Description: LoadName,
            FileName: FileName,
            Active: true,
            BeginDate: new Date,
            EndDate: new Date,
            ProcessStatus: 0,
            ProcessId: 0,
            ProcessMassiveLoad: []
        }

    fasecoldaDTO.ProcessMassiveLoad.push({ TypeFile: typeFile })

    return fasecoldaDTO;
}



