var InfringementStateIndex = null;
var InfringementStateStatus = null;
var StateCode = null;
var glbInfringementStateCreate = [];
var dropDownSearchAdvInfringementState = null;

$(() => {
    new InfringementState();
});

class InfringementState extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        InfringementState.GetAllInfringementState().done(function (data) {
            if (data.success) {
                InfringementState.LoadListViewInfrigement(data.result);
            }
        });

        dropDownSearchAdvInfringementState = uif2.dropDown({
            source: rootPath + 'Parametrization/InfringementState/InfringementStateSearch',
            element: '#inputInfringementState',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: function () { }
        });
    }

    bindEvents() {
        $('#btnNewInfringementState').click(InfringementState.Clear);
        $('#btnAddInfringementState').click(this.AddInfringementState);
        $('#listViewInfringementState').on('rowEdit', InfringementState.ShowData);
        $('#btnExitInfringementState').click(this.Exit);
        $('#btnExportInfringementState').click(this.ExportFile);
        $('#btnSaveInfringementState').click(this.SaveInfringementState);
        $('#inputInfringementState').on('buttonClick', this.SearchInfringementState);
    }

    AddInfringementState() {
        $("#formInfringementState").validate();
        if ($("#formInfringementState").valid()) {
            var group = InfringementState.GetForm();
            if (InfringementStateIndex == null) {
                var lista = $("#listViewInfringementState").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.InfringementStateDescription.toUpperCase() == group.InfringementStateDescription.toUpperCase();
                });
                if (ifExist.length > 0 && InfringementStateStatus != ParametrizationStatus.Create) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistInfringementState, 'autoclose': true });
                }
                else {
                    InfringementState.SetStatus(group, ParametrizationStatus.Create);
                    $("#listViewInfringementState").UifListView("addItem", group);
                }
            }
            else {
                if (InfringementStateIndex != undefined && InfringementStateStatus != undefined) {
                    InfringementState.SetStatus(group, InfringementStateStatus);
                } else {
                    InfringementState.SetStatus(group, ParametrizationStatus.Update);
                }
                $('#listViewInfringementState').UifListView('editItem', InfringementStateIndex, group);
            }
            InfringementState.Clear();
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    SaveInfringementState() {
        InfringementState.SetListToSend();
        $.ajax({
            type: "POST",
            url: 'SaveInfringementState',
            data: JSON.stringify({ lstInfringementStates: glbInfringementStateCreate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            glbInfringementStateCreate = [];
            var error = data.result.Message == null ? 0 : data.result.Message;
            $.UifNotify('show', {
                'type': 'info', 'message': Resources.Language.MessageUpdate + ': <br>' +
                Resources.Language.Aggregates + ': ' + data.result.TotalAdded + '<br> ' +
                Resources.Language.Updated + ': ' + data.result.TotalModified + '<br> ' +
                Resources.Language.Removed + ': ' + data.result.TotalDeleted + '<br> ' +
                Resources.Language.Errors + ': ' + error,
                'autoclose': true
            });
            InfringementState.GetAllInfringementState().done(function (data) {
                if (data.success) {
                    InfringementState.LoadListViewInfrigement(data.result);
                }
            });
        });
    }

    SearchInfringementState() {
        var inputInfringementState = $('#inputInfringementState').val();
        if (inputInfringementState.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $.ajax({
                type: "POST",
                url: 'GetInfringementStatesByDescription',
                data: JSON.stringify({ description: inputInfringementState }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var states = data.result;
                    if (data.result.length > 1) {
                        InfringementState.ShowSearchAdv(states);
                    }
                    else {
                        $.each(states, function (key, value) {
                            var lista = $("#listViewInfringementState").UifListView('getData')
                            var index = lista.findIndex(function (item) {
                                return item.InfringementStateCode == value.InfringementStateCode;
                            });
                            InfringementState.ShowData(null, value, index);
                        });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorDescription, 'autoclose': true });
                }
            });
        }
        $('#inputInfringementState').val('');
    }

    ExportFile() {
        InfringementState.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }

    static GetAllInfringementState() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/InfringementState/GetInfringementState',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static LoadListViewInfrigement(data) {
        $("#listViewInfringementState").UifListView({ displayTemplate: "#InfringementStateTemplate", edit: true, delete: false, customAdd: true, customEdit: true, height: 300 });
        $.each(data, function (key, value) {
            var state =
                {
                    InfringementStateCode: this.InfringementStateCode,
                    InfringementStateDescription: this.InfringementStateDescription
                };
            $("#listViewInfringementState").UifListView("addItem", state);
        })
    }

    static Clear() {
        $("#inputInfringementStateDescription").val("");
        $("#inputInfringementStateDescription").focus();
        InfringementStateIndex = null;
        InfringementStateStatus = null;
        StateCode = null;
    }

    static GetForm() {
        var data = {
        };
        $("#formInfringementState").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.InfringementStateCode = StateCode;
        data.InfringementStateDescription = $("#inputInfringementStateDescription").val();
        return data;
    }

    static SetStatus(object, status) {
        object.StatusTypeService = status;
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvInfringementState").UifListView({
            displayTemplate: "#InfringementStateTemplate",
            selectionType: "single",
            height: 300
        });
        $("#btnCancelSearchAdv").on("click", InfringementState.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", InfringementState.OkSearchAdv);
        $("#lvSearchAdvInfringementState").UifListView("clear");
        if (data) {
            data.forEach(item => {
                if (item.IsScore) {
                    item.lblServices = Resources.Language.Services;
                    item.ServicesDescription = Resources.Language.LabelIsScore;
                }
                if (item.IsFine && allied.IsScore) {
                    item.lblServices = Resources.Language.Services;
                    item.ServicesDescription += ', ' + Resources.Language.LabelIsFine;
                }
                else if (item.IsFine) {
                    item.lblServices = Resources.Language.Services;
                    item.ServicesDescription = Resources.Language.LabelIsFine;
                }
                $("#lvSearchAdvInfringementState").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvInfringementState.show();
    }

    static ShowData(event, result, index) {
        InfringementState.Clear();
        InfringementStateIndex = index;
        InfringementStateStatus = result.StatusTypeService;
        StateCode = result.InfringementStateCode;
        $("#inputInfringementStateDescription").val(result.InfringementStateDescription);
    }

    static SetListToSend() {
        var groupsData = $('#listViewInfringementState').UifListView('getData');
        $.each(groupsData, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.StatusTypeService != undefined) {
                glbInfringementStateCreate.push(value);
            }
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static CancelSearchAdv() {
        dropDownSearchAdvInfringementState.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvInfringementState").UifListView("getSelected");
        if (data.length == 1) {
            var state =
                {
                    InfringementStateCode: data[0].InfringementStateCode,
                    InfringementStateDescription: data[0].InfringementStateDescription
                };
            var lista = $("#listViewInfringementState").UifListView('getData');
            var index = lista.findIndex(function (item) {
                return item.InfringementStateCode == state.InfringementStateCode;
            });
            InfringementState.ShowData(null, state, index);
        }
        dropDownSearchAdvInfringementState.hide();
    }
}