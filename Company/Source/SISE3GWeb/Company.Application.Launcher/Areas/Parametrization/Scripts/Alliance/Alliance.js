var glbAlliancesCreate = [];
var objectIndex = null;
var objectStatus = null;

var dropDownSearchAdvAlliance = null;

$(() => {
    new Alliance();
});

class Alliance extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        Alliance.GetAlliance().done(function (data) {
            if (data.success) {
                Alliance.LoadListViewAlliance(data.result);
            }
        });
        dropDownSearchAdvAlliance = uif2.dropDown({
            source: rootPath + 'Parametrization/Alliance/AllianceAdvancedSearch',
            element: '#inputAllied',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: function () { }
        });
    }

    bindEvents() {
        $('#btnNewAlliance').click(Alliance.Clear);
        $('#btnAddAlliance').click(this.AddAlliance);
        $('#listViewAlliance').on('rowEdit', Alliance.ShowData);
        $('#btnSalePointAlliance').click(this.ShowModal);
        $('#btnExit').click(this.Exit);
        $('#btnSave').click(this.SaveAlliances);
        $('#inputAllied').on('buttonClick', this.SearchAlliance);
        $('#btnExport').click(this.ExportFile);
    }

    AddAlliance() {
        $("#formAlliance").validate();
        if ($("#formAlliance").valid()) {
            var allied = Alliance.GetForm();
            if (allied.IsScore == true) {
                allied.lblServices = Resources.Language.Services;
                allied.ServicesDescription = Resources.Language.LabelIsScore;
            }
            if (allied.IsFine == true && allied.IsScore == true) {
                allied.lblServices = Resources.Language.Services;
                allied.ServicesDescription += ', ' + Resources.Language.LabelIsFine;
            }
            else if (allied.IsFine == true) {
                allied.lblServices = Resources.Language.Services;
                allied.ServicesDescription = Resources.Language.LabelIsFine;
            }
            if (objectIndex == null) {
                if (Alliance.ValidAliance(allied)) {
                    Alliance.SetStatus(allied, "create");
                    $("#listViewAlliance").UifListView("addItem", allied);
                }
            }
            else {
                if (Alliance.ValidAliance(allied)) {
                    if (objectIndex != undefined && objectStatus != undefined) {
                        Alliance.SetStatus(allied, objectStatus);
                    } else {
                        Alliance.SetStatus(allied, "update");
                    }
                    $('#listViewAlliance').UifListView('editItem', objectIndex, allied);
                }
            }
            Alliance.Clear();
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    SaveAlliances() {
        Alliance.SetListToSend();
        if (glbAlliancesCreate.length > 0) {
            $.ajax({
                type: "POST",
                url: 'SaveAlliances',
                data: JSON.stringify({ lstAlliances: glbAlliancesCreate }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    glbAlliancesCreate = [];
                    Alliance.LoadListViewAlliance(data.result[1]);
                    $.each(data.result[0], function (index, item) {
                        $.UifNotify('show', { 'type': 'info', 'message': item, 'autoclose': true })
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSavingAlliance, 'autoclose': true });
                }
            });
        }
    }

    SearchAlliance() {
        var inputAllied = $('#inputAllied').val();
        if (inputAllied.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else if (inputAllied.length > 15) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMaximumChar, autoclose: false });
        }
        else {
            $.ajax({
                type: "POST",
                url: 'GetAllianceByDescription',
                data: JSON.stringify({ description: inputAllied }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var alliances = data.result;
                    if (alliances.length == 0) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AlliedNotExist, 'autoclose': true });
                    }
                    else {
                        if (alliances.length > 1) {
                            Alliance.ShowSearchAdv(alliances);
                        }
                        else {
                            $.each(alliances, function (key, value) {
                                var lista = $("#listViewAlliance").UifListView('getData')
                                var index = lista.findIndex(function (item) {
                                    return item.AlliedCode == value.AlliedCode;
                                });
                                Alliance.ShowData(null, value, index);
                            });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        $('#inputAllied').val('');
    }

    ExportFile() {
        Alliance.GenerateFileToExport().done(function (data) {
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

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static CancelSearchAdv() {
        dropDownSearchAdvAlliance.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvAlliance").UifListView("getSelected");
        if (data.length === 1) {
            var allied =
                {
                    AlliedCode: data[0].AlliedCode,
                    Description: data[0].Description,
                    IsScore: data[0].IsScore,
                    IsFine: data[0].IsFine
                };
            var lista = $("#listViewAlliance").UifListView('getData');
            var index = lista.findIndex(function (item) {
                return item.AlliedCode == allied.AlliedCode;
            });
            Alliance.ShowData(null, allied, index);
        }
        dropDownSearchAdvAlliance.hide();
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvAlliance").UifListView({
            displayTemplate: "#AllianceTemplate",
            selectionType: "single",
            height: 300
        });
        $("#btnCancelSearchAdv").on("click", Alliance.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", Alliance.OkSearchAdv);
        $("#lvSearchAdvAlliance").UifListView("clear");
        if (data) {
            data.forEach(item => {
                if (item.IsScore) {
                    item.lblServices = Resources.Language.Services;
                    item.ServicesDescription = Resources.Language.LabelIsScore;
                }
                if (item.IsFine && item.IsScore) {
                    item.lblServices = Resources.Language.Services;
                    item.ServicesDescription += ', ' + Resources.Language.LabelIsFine;
                }
                else if (item.IsFine) {
                    item.lblServices = Resources.Language.Services;
                    item.ServicesDescription = Resources.Language.LabelIsFine;
                }
                $("#lvSearchAdvAlliance").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvAlliance.show();
    }

    static DeleteAlliance(event, data, index) {
        event.resolve();
        if (data.AlliedCode != 0 && data.AlliedCode != undefined && data.Status !== "create") {
            event.resolve();
            data.Status = "delete";
            //glbAlliancesCreate.push(data);
            data.allowEdit = false;
            data.allowDelete = false;
            $("#listViewAlliance").UifListView("addItem", data);
        }
        
        Alliance.Clear();
    }

    static GetAlliance() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Alliance/GetAllAlliances',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static Clear() {
        $("#chkIsScore").prop("checked", "");
        $("#chkIsFine").prop("checked", "");
        $("#inputAlliedCode").val("");
        $("#inputDescription").val("");
        $("#inputDescription").focus();
        objectIndex = null;
        objectStatus = null;
        ClearValidation("#formAlliance"); 
    }

    static GetForm() {
        var data = {
        };
        $("#formAlliance").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Description = $("#inputDescription").val();
        data.IsFine = $("#chkIsFine").is(":checked");
        data.IsScore = $("#chkIsScore").is(":checked");
        data.AlliedCode = $("#inputAlliedCode").val();
        return data;
    }

    static SetStatus(object, status) {
        object.Status = status;
    }

    static ShowData(event, result, index) {
        Alliance.Clear();
        if (result.AlliedCode != undefined) {
            objectIndex = index;
            objectStatus = result.Status;
            $("#inputDescription").val(result.Description);
            $("#inputAlliedCode").val(result.AlliedCode);
            if (result.IsFine) {
                $('#chkIsFine').prop("checked", true);
            }
            if (result.IsScore) {
                $('#chkIsScore').prop("checked", true);
            }
            if (result.Status == "update" || result.Status == null) {
                $("#inputAlliedCode").attr("readonly", true);
            }
        }
    }

    static SetListToSend() {
        var objectsData = $('#listViewAlliance').UifListView('getData');
        $.each(objectsData, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.Status != undefined) {
                glbAlliancesCreate.push(value);
            }
        });
    }

    static LoadListViewAlliance(data) {
        $("#listViewAlliance").UifListView({ displayTemplate: "#AllianceTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Alliance.DeleteAlliance, height: 300 });
        $.each(data, function (key, value) {
            var allied =
                {
                    AlliedCode: this.AlliedCode,
                    Description: this.Description,
                    IsScore: this.IsScore,
                    IsFine: this.IsFine
                };
            if (allied.IsScore) {
                allied.lblServices = Resources.Language.Services;
                allied.ServicesDescription = Resources.Language.LabelIsScore;
            }
            if (allied.IsFine && allied.IsScore) {
                allied.lblServices = Resources.Language.Services;
                allied.ServicesDescription += ', ' + Resources.Language.LabelIsFine;
            }
            else if (allied.IsFine) {
                allied.lblServices = Resources.Language.Services;
                allied.ServicesDescription = Resources.Language.LabelIsFine;
            }
            $("#listViewAlliance").UifListView("addItem", allied);
        })
    }

    static ValidAliance(allied) {
        var lista = $("#listViewAlliance").UifListView('getData');
        var valid = true;
        var ifExist = lista.filter(function (item) {
            return item.Description.toUpperCase() == allied.Description.toUpperCase();
        });
        if (ifExist.length > 0 && objectStatus != "create" && objectIndex == undefined) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistAlliance, 'autoclose': true });
            valid = false;
        }
        return valid;
    }
}