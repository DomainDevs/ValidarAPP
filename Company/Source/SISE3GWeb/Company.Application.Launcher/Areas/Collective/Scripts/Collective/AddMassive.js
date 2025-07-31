
$(() => {
    new LoadType();
});

class LoadType extends Uif2.Page {
    bindEvents() {
        $("#inputExcelFile").on('change', this.AddFile);
        $('#inputExcelFile').fileupload({});
        $("#btnLoad").on('click', this.SaveMassive);

    }
    AddFile(event) {
        if (event.target.files.length > 0) {
            if ($('#formFile').valid()) {
                $(this).fileupload({
                    dataType: 'json',
                    url: 'UploadFile',
                    done: function (event, data) {
                        if (data.result.success) {
                            $('#inputExcelFile').data('Object', data.result.result);
                            $('.progress .progress-bar').css('width', 100 + '%');
                        }
                        else {
                            $('#inputFile').data('Object', null);
                            $('.progress .progress-bar').css('width', 0 + '%');
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.result, 'autoclose': true });
                        }
                    }
                })
            }
        }
    }

    SaveMassive() {
        var validateMsj = LoadType.ValidateLoad();
        if (validateMsj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelInformative + ":<br>" + validateMsj })
            return false;
        }
        var $btn = $('#btnLoad');
        $btn.button('loading');
        CollectiveModel.LoadTypeId = $("#selectTypeLoad").UifSelect("getSelected")
        CollectiveModel.FileName = $('#inputExcelFile').data('Object');
        CollectiveModel.LoadName = $('#inputNameLoad').val();
        Massive.AddMassive(CollectiveModel).done(function (data) {
            if (data.success) {
                $btn.button('reset');
                var massiveId = data.result;
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelLoad + ": " + massiveId, 'autoclose': true })
                Collective.HidePanels(1)             
                Collective.CollectiveLoadReady();
            }
            else {
                $btn.button('reset');
                $('.progress .progress-bar').css('width', 0 + '%');
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
            }
        });
    }
    static ValidateLoad() {
        var msjLoad = "";
        if ($("#selectTypeLoad").UifSelect("getSelected") == "") {
            msjLoad = msjLoad + "Tipo de Cargue <br>";
        }
        if ($("#inputNameLoad").val() == "") {
            msjLoad = msjLoad + "Nombre del Cargue <br>";
        }
        if ($('#selectTypeLoad').UifSelect("getSelected") == "") {
            msjLoad = msjLoad + AppResources.UploadTypeObligatory + "<br>";
        }
        return msjLoad;
    }
}