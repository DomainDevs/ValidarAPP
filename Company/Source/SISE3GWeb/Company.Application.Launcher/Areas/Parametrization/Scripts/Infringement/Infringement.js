var InfringementIndex = null;
var InfringementStatus = null;
var InfringementCode = null;
var glbInfringementCreate = [];
var dropDownSearchAdvInfringement = null;

$(() => {
    new Infringement();
});

class Infringement extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        Infringement.GetAllInfrigementActive().done(function (data) {
            if (data.success) {
                $("#selectInfringementGroup").UifSelect({ sourceData: data.result });
            }
        });
        Infringement.GetAllInfrigement().done(function (data) {
            if (data.success) {
                Infringement.LoadListViewInfrigement(data.result);
            }
        });

        dropDownSearchAdvInfringement = uif2.dropDown({
            source: rootPath + 'Parametrization/Infringement/InfringementSearch',
            element: '#btnSearchAdvInfringement',
            align: 'right',
            width: 600,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });
    }

    bindEvents() {
        $('#btnNewInfringement').click(Infringement.Clear);
        $('#btnAddInfringement').click(this.AddInfringement);
        $('#listViewInfringement').on('rowEdit', Infringement.ShowData);
        $('#btnExitInfringement').click(this.Exit);
        $('#btnExportInfringement').click(this.ExportFile);
        $('#btnSaveInfringement').click(this.SaveInfringement);
        $('#inputInfringement').on('buttonClick', this.SearchInfringement);
    }

    AddInfringement() {
        $("#formInfringement").validate();
        if ($("#formInfringement").valid()) {
            var group = Infringement.GetForm();
            if (InfringementIndex == null) {
                var lista = $("#listViewInfringement").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.InfringementCode.toUpperCase() == group.InfringementCode.toUpperCase();
                });
                var ifExistPC = lista.filter(function (item) {
                    return item.InfringementPreviousCode == group.InfringementPreviousCode;
                });
                if ((ifExist.length > 0 || ifExistPC.length > 0) && InfringementStatus != ParametrizationStatus.Create) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistInfringement, 'autoclose': true });
                }
                else {
                    Infringement.SetStatus(group, ParametrizationStatus.Create);
                    $("#listViewInfringement").UifListView("addItem", group);
                }
            }
            else {
                if (InfringementIndex != undefined && InfringementStatus != undefined) {
                    //Infringement.SetStatus(group, InfringementStatus);
                    Infringement.SetStatus(group, ParametrizationStatus.Update);
                } else {
                    Infringement.SetStatus(group, ParametrizationStatus.Update);
                }
                $('#listViewInfringement').UifListView('editItem', InfringementIndex, group);
            }
            Infringement.Clear();
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    SaveInfringement() {
        Infringement.SetListToSend();
        $.ajax({
            type: "POST",
            url: 'SaveInfringement',
            data: JSON.stringify({ lstInfringement: glbInfringementCreate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            glbInfringementCreate = [];
            var error = data.result.Message == null ? 0 : data.result.Message;
            $.UifNotify('show', {
                'type': 'info', 'message': Resources.Language.MessageUpdate + ': <br>' +
                    Resources.Language.Aggregates + ': ' + data.result.TotalAdded + '<br> ' +
                    Resources.Language.Updated + ': ' + data.result.TotalModified + '<br> ' +
                    Resources.Language.Removed + ': ' + data.result.TotalDeleted + '<br> ' +
                    Resources.Language.Errors + ': ' + error,
                'autoclose': true
            });
            Infringement.GetAllInfrigement().done(function (data) {
                if (data.success) {
                    Infringement.LoadListViewInfrigement(data.result);
                }
            });
        });
    }

    SearchInfringement() {
        var inputInfringement = $('#inputInfringement').val();
        if (inputInfringement.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar});
        }
        else {
            $.ajax({
                type: "POST",
                url: 'GetInfringementByDescription',
                data: JSON.stringify({ description: inputInfringement, code: '', group: null }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var infringement = data.result;
                    if (data.result.length > 1) {
                        Infringement.ShowSearchAdv(infringement);
                    }
                    else {
                        $.each(infringement, function (key, value) {
                            var lista = $("#listViewInfringement").UifListView('getData')
                            var index = lista.findIndex(function (item) {
                                return item.InfringementCode == value.InfringementCode;
                            });
                            Infringement.ShowData(null, value, index);
                        });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorDescription, 'autoclose': true });
                }
            });
        }
        $('#inputInfringement').val('');
    }

    ExportFile() {
        Infringement.GenerateFileToExport().done(function (data) {
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

    componentLoadedCallback() {
        $("#lvSearchAdvInfringement").UifListView({
            displayTemplate: "#InfringementTemplate",
            selectionType: "single",
            height: 240,
            width: 450
        });
        $("#btnSearchAdvInfringement").on("click", Infringement.SearchAdvInfringement);
        $("#btnCancelSearchAdv").on("click", Infringement.CancelSearchAdv);
        $("#btnSearch").on("click", Infringement.SearchAdv);
        $("#btnOkSearchAdv").on("click", Infringement.OkSearchAdv);
    }

    static SearchAdvInfringement() {
        $("#lvSearchAdvInfringement").UifListView("clear");
        $("#inputInfringementDescriptionSrch").val('');
        $("#inputInfringementCodeSrch").val('');
        Infringement.GetInfringementGroup().done(function (data) {
            if (data.success) {
                $("#selectInfringementGroupSrch").UifSelect({ sourceData: data.result });
            }
        });
        dropDownSearchAdvInfringement.show();
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
        $('#inputInfringementDescriptionSrch').val('');
        $('#inputInfringementCodeSrch').val('');
        $('#selectInfringementGroupSrch').UifSelect("setSelected", null);
        $('#lvSearchAdvInfringement').UifListView("clear");
        dropDownSearchAdvInfringement.hide();
    }

    static GetAllInfrigement() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Infringement/GetInfringement',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetAllInfrigementActive() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Infringement/GetInfringementGroupActive',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetInfringementGroup() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Infringement/GetInfringementGroup',
            async: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static LoadListViewInfrigement(data) {
        $("#listViewInfringement").UifListView({ displayTemplate: "#InfringementTemplate", edit: true, delete: false, customAdd: true, customEdit: true, height: 300 });
        $.each(data, function (key, value) {
            var infringement =
                {
                    InfringementCode: this.InfringementCode,
                    InfringementDescription: this.InfringementDescription,
                    InfringementPreviousCode: this.InfringementPreviousCode,
                    InfringementGroupDescription: this.InfringementGroupDescription,
                    InfringementGroupCode: this.InfringementGroupCode
                };
            $("#listViewInfringement").UifListView("addItem", infringement);
        })
    }

    static Clear() {
        $("#inputInfringementCode").val("").removeAttr("disabled");
        $("#inputInfringementCode").focus();
        $("#inputInfringementPreviousCode").val("");
        $("#inputInfringementDescription").val("");
        $('#selectInfringementGroup').UifSelect("setSelected", null);
        InfringementIndex = null;
        InfringementStatus = null;
        InfringementCode = null;
    }

    static GetForm() {
        var data = {
        };
        $("#formInfringement").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.InfringementCode = $("#inputInfringementCode").val();
        data.InfringementPreviousCode = $("#inputInfringementPreviousCode").val();
        data.InfringementDescription = $("#inputInfringementDescription").val();
        //if ($('#selectInfringementGroup').UifSelect("getSelected").InfringementGroupCode > 0) {
        if ($('#selectInfringementGroup').UifSelect("getSelected") > 0) {
            data.InfringementGroupDescription = $('#selectInfringementGroup option:selected').text();
        }
        else {
            data.InfringementGroupDescription = 'NINGUNO';
        }
        data.InfringementGroupCode = $('#selectInfringementGroup').UifSelect('getSelected');
        return data;
    }

    static SetStatus(object, status) {
        object.StatusTypeService = status;
    }

    static SearchAdv() {
        var infringementDescription = $('#inputInfringementDescriptionSrch').val();
        var infringementCode = $('#inputInfringementCodeSrch').val();
        var infringementGroup = $('#selectInfringementGroupSrch').UifSelect("getSelected");
        if (infringementDescription.length <= 0 && infringementCode.length <= 0 && infringementGroup <= 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelSearchAdvancedCriteria, 'autoclose': true });
        }
        else {
            if (infringementDescription != '' && infringementDescription.length < 3) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchAdvancedMiniumCharacters, 'autoclose': true });
            } else {
                $.ajax({
                    type: "POST",
                    url: 'GetInfringementByDescription',
                    data: JSON.stringify({ description: infringementDescription, code: infringementCode, group: infringementGroup }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success > 0) {
                        Infringement.ShowSearchAdv(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorDescription, 'autoclose': true });
                    }
                });
            }
        }
        //Infringement.CancelSearchAdv();
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvInfringement").UifListView("clear");
        data.forEach(item => {
            $("#lvSearchAdvInfringement").UifListView("addItem", item);
        });
        dropDownSearchAdvInfringement.show();
    }

    static OkSearchAdv() {
        $("#lvSearchAdvInfringement").children().children().val(null);
        let data = $("#lvSearchAdvInfringement").UifListView("getSelected");
        if (data.length == 1) {
            var infringement =
                {
                    InfringementCode: data[0].InfringementCode,
                    InfringementDescription: data[0].InfringementDescription,
                    InfringementPreviousCode: data[0].InfringementPreviousCode,
                    InfringementGroupDescription: data[0].InfringementGroupDescription,
                    InfringementGroupCode: data[0].InfringementGroupCode
                };
            var lista = $("#listViewInfringement").UifListView('getData');
            var index = lista.findIndex(function (item) {
                return item.InfringementCode == infringement.InfringementCode;
            });
            Infringement.ShowData(null, infringement, index);
        }
        
        Infringement.CancelSearchAdv();
    }

    static ShowData(event, result, index) {
        Infringement.Clear();
        InfringementIndex = index;
        InfringementStatus = result.StatusTypeService;
        $("#inputInfringementCode").val(result.InfringementCode).attr("disabled", "disabled");
        $("#inputInfringementDescription").val(result.InfringementDescription);
        $("#inputInfringementPreviousCode").val(result.InfringementPreviousCode);
        $('#selectInfringementGroup').UifSelect("setSelected", result.InfringementGroupCode);
    }

    static SetListToSend() {
        var infringementsData = $('#listViewInfringement').UifListView('getData');
        $.each(infringementsData, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.StatusTypeService != undefined) {
                glbInfringementCreate.push(value);
            }
        });
    }
}