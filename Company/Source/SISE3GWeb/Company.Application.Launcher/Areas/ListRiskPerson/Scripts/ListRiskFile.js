var steps = {};
class ListRiskFile extends Uif2.Page {
    getInitialState() {
        ListRiskFile.GetListRisk();
        ListRiskFile.InitialStateControls(true);
        $('#stepsListRisk').hide();
        steps = uif2.steps({
            container: '#wizard',
            onStep: ListRiskFile.stepFunction
        });
        $("#btnRequestProcess").hide();
    }

    bindEvents() {
        /*Inputs*/
        $("#selectListRisk").on('itemSelected', this.ValdateListRiskType);
        $('#inputFileRisk').on('change', this.UploadFileListRisk);
        $('#inputFileRisk').click(ListRiskFile.PrevalidateForm);
        /*Buttons*/
        $('#btnAddFiles').click(ListRiskFile.ValidateFileStructure);
        $('#btnLoad').click(this.GenerateLoadMassiveListRisk);
        $('#btnErrors').click(ListRiskFile.GetErrorExcelProcessListRisk);
        $('#inputProcess').on('buttonClick', ListRiskFile.GetProcessStatusById);
        $('#btnProcess').click(ListRiskFile.setInitialProcessFile);
        $('#btnNew').click(ListRiskFile.NewLoadFile);
        $('#btnExit').click(ListRiskFile.Exit);
        $('#btnRequestProcess').click(this.GenerateListRiskProcess);

        /*Upload*/
        $('#inputFileRisk').fileupload({});


    }

    GenerateListRiskProcess() {
        ListRiskFileRequest.GenerateListRiskRequest();
        $.UifNotify('show', { 'type': 'info', 'message': "El proceso se inicio correctamente", 'autoclose': true });
    }

    ValdateListRiskType() {
        ListRiskFileRequest.GetListRisk().done(function (data) {
            if (data.success) {
                var currentList = $("#selectListRisk option:selected").val();
                var listRisks = data.result;

                var listRiskType = listRisks.find(x => x.Id == currentList).RiskListType;

                if (listRiskType == 3) {

                    $('#inputFileRisk').prop('disabled', true);
                    $('#btnAddFiles').prop('disabled', true);

                    $("#btnRequestProcess").show();

                } else {
                    $('#inputFileRisk').prop('disabled', false);
                    $('#btnAddFiles').prop('disabled', false);
                }
            }
        });
    }

    static setInitialProcessFile() {
        var listRiskProcessId = $('#inputProcess').val();
        ListRiskFileRequest.SetInitialProcessFile(listRiskProcessId).done(function (data) {
            if (data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': "El proceso se inicio correctamente", 'autoclose': true });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "Ocurrio un error iniciando el proceso del archivo", 'autoclose': true });
            }

        });

    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }


    static NewLoadFile() {
        $('#inputCodigoFaseColda').val(null)
        $('#inputCodigoFile').data('Object', null);
        $('#progress-bar-code').css('width', 0 + '%');

        $('#inputValueFaseColda').val(null);
        $('#inputValueFile').data('Object', null);
        $('#progress-bar-value').css('width', 0 + '%');

        $('#inputProcess').val("");
        $('#inputProcess').prop('disabled', false);
        $('#btnAddFiles').prop('disabled', true);
        $('#selectListRisk').UifSelect('setSelected', null);
        $('#inputListRiskFile').val('');
        ListRiskFile.Controls(true);
    }

    static Controls(state) {
        //$("#btnExit").prop('disabled', state);
        $("#btnErrors").prop('disabled', state);
        $("#btnLoad").prop('disabled', state);
        $("#btnProcess").prop('disabled', state);
        $('#stepsListRisk').hide();
    }

    static PrevalidateForm(e) {
        if ($('#selectListRisk').UifSelect('getSelected') == "") {
            e.preventDefault();
            $.UifNotify('show', { 'type': 'info', 'message': "Debe seleccionar una lista de riesgo", 'autoclose': true });
        }
    }
    static stepFunction(event, currentIndex, newIndex) {
        return true;
    }


    UploadFileListRisk(event) {
        if (event.target.files.length > 0) {
            if ($('#formFileListRisk').valid()) {
                $(this).fileupload({
                    dataType: 'json',
                    url: rootPath + 'ListRiskPerson/ListRiskFile/UploadFileListRisk',
                    done: function (event, data) {
                        if (data.result.success) {

                            if (data.files) {
                                $('#inputFileRisk').data('Object', data.result.result);
                                $('#inputListRiskFile').val(data.files[0].name);
                                $('#progress-bar-code').css('width', 100 + '%');
                                $('#btnLoad').prop('disabled', true);
                            }

                            if ($('#inputListRiskFile').val() != "")
                                $('#btnAddFiles').prop('disabled', false);
                        }
                        else {
                            $('#inputFileRisk').data('Object', null);
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

    static GetProcessStatusById() {
        var processId = $('#inputProcess').val();
        if (processId != "" && processId > 0) {
            ListRiskFileRequest.GetProcessStatusById(processId).done(function (data) {
                if (data.success) {

                    if (data.result.ErrorDescription != "") {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result.ErrorDescription, 'autoclose': true });
                    }
                    $('#inputTotalRecords').val(data.result.ProcessCount);
                    $('#inputProcessed').val(data.result.InsertedCount);
                    var pendingRows = data.result.ProcessCount - data.result.InsertedCount - data.result.HasError;
                    if (pendingRows != 0) {
                        $.UifNotify('show', { 'type': 'info', 'message': "Cargando", 'autoclose': true });
                    } else if (pendingRows == 0 && data.result.ProcessCount != 0 && data.result.ProcessStatus <= 5){
                        $.UifNotify('show', { 'type': 'info', 'message': "Cargue finalizado", 'autoclose': true });
                    }
                    $('#inputPending').val(pendingRows);
                    $('#inputErrors').val(data.result.HasError);
                    $('#stepsListRisk').show();
                    if (data.result.HasError >= data.result.ProcessCount) {
                        $('#btnErrors').prop('disabled', true);
                        $('#btnProcess').prop('disabled', true);
                    } else {
                        $('#btnErrors').prop('disabled', false);
                        $('#btnProcess').prop('disabled', false);
                    }
                    if (data.result.ProcessStatus > 5) {
                        $('#lblProcess').text("Registrados");
                        if (pendingRows == 0) {
                            $.UifNotify('show', { 'type': 'info', 'message': "Proceso finalizado", 'autoclose': true });
                        } else {
                            $.UifNotify('show', { 'type': 'info', 'message': "Procesando", 'autoclose': true });
                        }
                    } else {
                        $('#lblProcess').text("Procesados");
                    }


                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });

                }

            })
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': "Debe Ingresar un numero de proceso ", 'autoclose': true });
        }

    }

    static GetErrorExcelProcessListRisk() {
        var processId = $('#inputProcess').val();
        if (processId > 0) {
            ListRiskFileRequest.GetErrorExcelProcessListRisk(processId).done(function (data) {
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

    GenerateLoadMassiveListRisk() {
        $('#formFileListRisk').validate();
        if ($('#formFileListRisk').valid()) {
            ListRiskFile.ControlsLoading(true);

            var listRiskDTO = $('#formFileListRisk').serializeObject();
            var FileName = $('#inputFileRisk').data('Object');
            var LoadName = $('#inputListRiskFile').val().split('.')[0];

            listRiskDTO = BuildCompanylistRiskDTO(FileName, LoadName);

            ListRiskFile.ShowMessageByUploadFile({ ProcessStatus: 4 });

            ListRiskFileRequest.CreateLoadListRiskFile(listRiskDTO).done(function (data) {
                if (data.success) {

                    $.UifDialog('alert', {
                        'message': 'Se ha realizado el cargue ' + data.result.ProcessId + ' exitosamente'
                    }, null)

                    ListRiskFile.ShowMessageByUploadFile({ ProcessStatus: 5 });


                    //ListRiskFile.InitialStateControls(false);
                    $('#btnErrors').prop('disabled', false);
                    //$('#btnLoad').prop('disabled', true);

                    $('#inputProcess').val(data.result.ProcessId);
                }
                else {
                    if (data.result.HasError) {
                        $.UifDialog('alert', {
                            'message': 'Se ha realizado el cargue ' + data.result.ProcessId + ' con errores'
                        }, null)
                        ListRiskFile.ShowMessageByUploadFile(3)
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result.ErrorDescription, 'autoclose': true });
                    }
                }
            });
        }
    }

    static ValidateFileStructure() {
        $('#formFileListRisk').validate();
        if ($('#formFileListRisk').valid()) {
            var nameFile = $('#inputListRiskFile').val();
            nameFile = nameFile.split('.');

            $('#btnLoad').prop('disabled', false);
            $('#btnAddFiles').prop('disabled', true);
            ListRiskFile.ShowMessageByUploadFile({ ProcessStatus: 2 });

        } else {
            $.UifNotify('show', { 'type': 'info', 'message': "Los campos con (*) son obligatorios", 'autoclose': true });
        }
    }

    static ShowMessageByUploadFile(process) {
        switch (process.ProcessStatus) {
            case 1:
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Validating, 'autoclose': true });
                break;
            case 2:
                $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.SuccessfullyValidated, 'autoclose': true })
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

    static GetListRisk(selectedId) {
        ListRiskFileRequest.GetListRisk().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId > 0) {
                        $("#selectListRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                    else {
                        $('#selectListRisk').UifSelect({ sourceData: data.result });
                    }
                }
            }
        });
    }

    static InitialStateControls(disabled) {
        $('#btnLoad').prop('disabled', disabled);
        $('#btnErrors').prop('disabled', disabled);
        $('#btnProcess').prop('disabled', disabled);
    }

    static ControlsLoading(disabled) {
        $('#btnLoad').prop('disabled', disabled);
    }
}

function BuildCompanylistRiskDTO(FileName, LoadName) {
    var listRiskDTO =
    {
        Description: LoadName,
        FileName: FileName,
        BeginDate: new Date,
        EndDate: new Date,
        ListRisk: $('#selectListRisk').UifSelect('getSelectedSource')
    }
    return listRiskDTO;
}