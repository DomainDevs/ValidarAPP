var fileName = "";
class MatchingProcess extends Uif2.Page {
    bindEvents() {
        $('#validateMatchPerson').click(MatchingProcess.ValidateMatchPerson);
        $('#matchingProcess').click(MatchingProcess.StartMatchingProcess);
        $('#inputIdNumber').change(MatchingProcess.onChangeInput);
        $('#searchMatchProcess').on('search', MatchingProcess.SearchProcess);
        $('#exportExcel').click(MatchingProcess.ExportExcel);
    }

    static ValidateMatchPerson() {
        if ($("#inputIdNumber").val() == "" || $("#inputIdNumber").val() == null) {
            $.UifNotify('show', {
                'type': 'info', 'message': 'Por favor ingrese un valor para iniciar la búsqueda'
            })
        } else {
            $.UifNotify('show', {
                'type': 'info', 'message': 'El proceso se inició correctamente'
            })
            MatchingProcessRequest.GetPersonRiskList($("#inputIdNumber").val().trim()).done(function (data) {
                if (data.success) {
                    $.UifDialog('alert', {
                        'message': 'Se ha realizado el proceso ' + data.result + ' exitosamente'
                    }, null)
                }
            });
        }
    }

    static StartMatchingProcess() {
        $.UifNotify('show', {
            'type': 'info', 'message': 'Se iniciará el proceso de validación de coincidencias totales.'
        })

        $.UifNotify('show', {
            'type': 'info', 'message': 'El proceso se inició correctamente'
        })
        MatchingProcessRequest.StartCompleteProcess().done(function (data) {
            if (data.success) {

                $.UifDialog('alert', {
                    'message': 'Se ha realizado el proceso ' + data.result + ' exitosamente'
                }, null)
            }
        });
    }

    static onChangeInput() {
        if ($('#inputIdNumber').val() != null && $('#inputIdNumber').val() != "") {
            $('#validateMatchPerson').prop("disabled", false);
        } else {
            $('#validateMatchPerson').prop("disabled", true);
        }
    }

    static SearchProcess() {
        if ($('#searchMatchProcess').val() != null && $('#searchMatchProcess').val() != "") {
            MatchingProcessRequest.SearchProcess($('#searchMatchProcess').val()).done(function (data) {
                if (data.success) {
                    var status = "";

                    if (data.result.Status == 9) {
                        status = 'No se encontraron coincidencias';
                        $('#exportExcel').prop("disabled", true);
                    } else if (data.result.Status == 7) {
                        status = "El proceso se encuentra en estado procesado";
                        $('#exportExcel').prop("disabled", false);
                        fileName = "http:" + data.result.fileName + ".xlsx";
                    } else {
                        status = "La validacion se encuentra en proceso";
                        $('#exportExcel').prop("disabled", true);
                    }
                    $.UifDialog('alert', {
                        'message': status
                    }, null)
                }
            });
        } else {
            $.UifNotify('show', {
                'type': 'info', 'message': 'Ingrese un valor para iniciar la búsqueda'
            });
        }
    }

    static ExportExcel() {
        if (fileName != "") {
            DownloadFile(fileName, true, (url) => url.match(/([^\\]+.xlsx)/));
        }
    }
}