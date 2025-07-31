$(() => {
    new ParametrizationVehicleType();
    new ParametrizationVehicleBody();
});

//var glbVehicleType = [];
var vehicleType = {};
var vehicleTypeIndex = null;
var inputSearch = "";

var dropDownSearchAdvVehicleType = null;
class VehicleType {

    static GetVehicleTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/VehicleType/GetVehicleTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static Save(vehicleTypes) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/VehicleType/Save',
            data: JSON.stringify({ vehicleTypesView: vehicleTypes }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GenerateFileVehicleTypeToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/VehicleType/GenerateFileVehicleTypeToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateFileVehicleBodyToExport(vehicleTypeView) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/VehicleType/GenerateFileVehicleBodyToExport',
            data: JSON.stringify({ vehicleType: vehicleTypeView }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}


class ParametrizationVehicleType extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);

        VehicleType.GetVehicleTypes().done(function (data) {
            if (data.success) {
                ParametrizationVehicleType.LoadListViewVehicleTypes(data);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $("#listVehicleType").UifListView({
            displayTemplate: "#VehicleTypeTemplate",
            edit: true,
            delete: true,
            customAdd: true,
            customEdit: true,
            height: 300,
            deleteCallback: this.DeleteItemVehicleType
        });

        dropDownSearchAdvVehicleType = uif2.dropDown({
            source: rootPath + 'Parametrization/VehicleType/AdvancedSearch',
            element: '#btnSearchAdvVehicleType',
            align: 'right',
            width: 600,
            height: 551,
            loadedCallback: function () { }
        });

        $("#listViewSearchAdv").UifListView({
            displayTemplate: '#VehicleTypeTemplateAdv',
            selectionType: "single",
            height: 450
        });
    }

    bindEvents() {
        $("#btnAsociateBody").click(this.BtnAsociateBody);
        $("#btnNewVehicleType").click(ParametrizationVehicleType.Clear);
        $("#btnAcceptVehicleBody").click(this.BtnAcceptVehicleBody);
        $("#btnAcceptVehicleType").click(ParametrizationVehicleType.BtnAcceptVehicleType);

        $('#inputSearchVehicleType').on('buttonClick', this.SearchVehicleTypes);
        $('#inputSearchVehicleType').on('itemSelected', this.SearchVehicleTypes);


        $("#listVehicleType").on('rowEdit', ParametrizationVehicleType.ShowData);
        $("#listVehicleType").on('rowDelete', this.DeleteItemVehicleType);


        $("#btnSearchAdvVehicleType").on("click", this.SearchAdvVehicleType);

        $("#btnSaveVehicleType").on("click", this.SaveVehicleTypes);
        $("#btnExport").on("click", this.sendExcelVehicleType);
        $("#btnExportVehicleBody").on("click", this.sendExcelVehicleBody);
        $("#btnExit").click(this.redirectIndex);

    }

    sendExcelVehicleType() {
        VehicleType.GenerateFileVehicleTypeToExport().done(function (data) {
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

    sendExcelVehicleBody() {
        ParametrizationVehicleType.ConstructVehicleType();
        VehicleType.GenerateFileVehicleBodyToExport(vehicleType).done(function (data) {
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

    static ConstructMessageArray(title, array) {
        var arr = [];
        if (array == null) {
            return null;
        }
        if (array.length == 0) {
            return null;
        }
        var messageResult = title + "<br />";
        messageResult += array.join("<br />");
        return messageResult;
    }

    SaveVehicleTypes() {
        var vehicleTypes = $("#listVehicleType").UifListView('getData');
        VehicleType.Save(vehicleTypes)
            .done(function (data) {
                if (data.success) {
                    ParametrizationVehicleType.Clear();
                    var messageResult = "";
                    if (data.result.messageCreated != null) {
                        messageResult += data.result.messageCreated + "<br />";
                    }

                    if (data.result.messageUpdated != null) {
                        messageResult += data.result.messageUpdated + "<br />";
                    }

                    if (data.result.messageDeleted != null) {
                        messageResult += data.result.messageDeleted + "<br />";
                    }

                    if (messageResult !== "") {
                        $.UifNotify('show', { 'type': 'info', 'message': messageResult, 'autoclose': true });
                    }

                    var messageErrorCreated = ParametrizationVehicleType.ConstructMessageArray(Resources.Language.ErrorCreated, data.result.errorCreated);
                    var messageErrorUpdated = ParametrizationVehicleType.ConstructMessageArray(Resources.Language.ErrorUpdated, data.result.errorUpdated);
                    var messageErrorDeleted = ParametrizationVehicleType.ConstructMessageArray(Resources.Language.ErrorDeleted, data.result.errorDeleted);
                    if (messageErrorCreated != null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': messageErrorCreated, 'autoclose': true });
                    }
                    if (messageErrorUpdated != null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': messageErrorUpdated, 'autoclose': true });
                    }
                    if (messageErrorDeleted != null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': messageErrorDeleted, 'autoclose': true });
                    }

                    VehicleType.GetVehicleTypes().done(function (data) {
                        if (data.success) {
                            //glbVehicleType = data.result;
                            ParametrizationVehicleType.LoadListViewVehicleTypes(data);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveVehicleTypes, 'autoclose': true })
            });
    }

    SearchAdvVehicleType() {
        ParametrizationVehicleType.ShowSearchAdv();
    }

    BtnAsociateBody() {
        if (vehicleTypeIndex != null) {
            $("#btnExportVehicleBody").prop("disabled", false);
        } else {
            $("#btnExportVehicleBody").prop("disabled", true);
        }
        ParametrizationVehicleBody.OpenVehicleBody($("#inputDescription").val());
    }

    SearchVehicleTypes(event, selectedItem) {
        inputSearch = selectedItem;
        if (inputSearch.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
            return false;
        }
        if (inputSearch.length > 30) {
            $.UifNotify('show', { 'type': 'info', 'message': "La búsqueda no puede ser mayor a 30 caracteres", 'autoclose': true });
            return false;
        }
        ParametrizationVehicleType.Clear();

        ParametrizationVehicleType.Get(0, inputSearch);
    }

    static OkSearchVehicleTypeAdv() {
        //$("#listViewSearchAdv").UifListView({ displayTemplate: '#VehicleTypeTemplateAdv', selectionType: "single", height: 400 });
        let data = $("#listViewSearchAdv").UifListView("getSelected");
        if (data.length == 1) {
            ParametrizationVehicleType.ShowData(null, data, data.key);
        }
        ParametrizationVehicleType.HideSearchAdv();
    }

    static CancelSearchAdv() {
        ParametrizationVehicleType.HideSearchAdv();
    }

    DeleteItemVehicleType(event, data, index) {
        event.resolve();
        var vehicleTypeList = $("#listVehicleType").UifListView('getData');
        $.each(vehicleTypeList, function (index, value) {
            if (value.TypeCode !== "" && value.TypeCode !== undefined) {
                if (this.TypeCode == data.TypeCode && this.Description == data.Description) {
                    value.State = ParametrizationStatus.Delete;
                    value.allowEdit = false;
                    value.allowDelete = false;
                    $("#listVehicleType").UifListView('addItem', this);
                }
            }
        });
        ParametrizationVehicleType.Clear();
    }

    BtnAcceptVehicleBody() {
        if ($("#tableVehicleBody").UifDataTable("getSelected") != null) {
            vehicleType.VehicleBodies = ParametrizationVehicleBody.SaveBodies();
        } else {
            $('#modalBodies').UifModal("hide");
        }

    }

    static ConstructVehicleType() {
        vehicleType.Description = $("#inputDescription").val();
        vehicleType.ShortDescription = $("#inputShortDescription").val();
        vehicleType.TypeCode = $("#inputCode").val();
        vehicleType.IsTruck = $('#checkIsTruck').is(':checked');
        vehicleType.IsActive = $('#checkIsActive').is(':checked');
        if (vehicleType.TypeCode > 0) {
            vehicleType.State = 3;
        }
        else {
            vehicleType.State = 2;
        }
    }

    static BtnAcceptVehicleType() {
        $("#formVehicleType").validate();
        if ($("#formVehicleType").valid()) {
            ParametrizationVehicleType.ConstructVehicleType();
            if (vehicleTypeIndex == null) {
                var lista = $("#listVehicleType").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == vehicleType.Description.toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExitsVehicleTypeName, 'autoclose': true });
                }
                else {
                    vehicleType.State = ParametrizationStatus.Create;
                    $("#listVehicleType").UifListView("addItem", vehicleType);
                }
            }
            else {
                vehicleType.State = ParametrizationStatus.Update;
                $("#listVehicleType").UifListView('editItem', vehicleTypeIndex, vehicleType);
            }
            ParametrizationVehicleType.Clear();
        }
    }

    static Clear() {
        $("#inputCode").val(null);
        $("#inputDescription").val(null);
        $("#inputDescription").focus();
        $("#inputShortDescription").val(null);
        $("#inputSearchVehicleType").val(null);
        $('#checkIsTruck').attr('checked', false);
        $('#checkIsActive').attr('checked', false);

        ParametrizationVehicleBody.ClearBodies();
        vehicleType = {};
        vehicleTypeIndex = null;
    }

    static Get(id, inputSearch) {
        var find = false;
        var data = [];
        var search = $("#listVehicleType").UifListView('getData');
        if (id == 0) {
            $.each(search, function (key, value) {
                if (value.Description.toLowerCase().sistranReplaceAccentMark().includes(inputSearch.toLowerCase().sistranReplaceAccentMark())) {
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });
        }
        else {
            $.each(search, function (key, value) {
                if (value.TypeCode == id) {
                    vehicleTypeIndex = key;
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });
        }
        if (find === false) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.VehicleTypeNotFound, 'autoclose': true });
        }
        else {
            if (data.length === 1) {
                ParametrizationVehicleType.ShowData(null, data, data.key);
            }
            else {
                ParametrizationVehicleType.ShowSearchAdv(data);
            }
        }
    }

    static LoadListViewVehicleTypes(data) {
        if (data.success) {
            // Limpiar ListView
            $("#listVehicleType").UifListView("clear");

            $.each(data.result, function (key, value) {
                var tmpVehicleType = {
                    TypeCode: this.TypeCode,
                    Description: this.Description,
                    ShortDescription: this.ShortDescription,
                    IsTruck: this.IsTruck,
                    IsActive: this.IsActive,
                    VehicleBodies: this.VehicleBodies,
                    State: ParametrizationStatus.Original
                };
                $("#listVehicleType").UifListView("addItem", tmpVehicleType);
            });
        }
    }

    static ShowData(event, result, index) {
        ParametrizationVehicleType.Clear();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        if (result.TypeCode != undefined) {
            vehicleTypeIndex = index;
            $("#inputCode").val(result.TypeCode);
            $("#inputDescription").val(result.Description);
            $("#inputShortDescription").val(result.ShortDescription);

            $("#checkIsTruck").prop("checked", result.IsTruck);
            $("#checkIsActive").prop("checked", result.IsActive);
            vehicleType.VehicleBodies = result.VehicleBodies;
            ParametrizationVehicleBody.SelectBodies(result.VehicleBodies);
        }
    }

    static ShowSearchAdv(data) {
        $("#listViewSearchAdv").UifListView({ displayTemplate: '#VehicleTypeTemplateAdv', selectionType: "single", height: 450 });
        $("#listViewSearchAdv").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#listViewSearchAdv").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvVehicleType.show();
        $("#btnOkSearchVehicleTypeAdv").off("click");
        $("#btnCancelVehicleTypeAdv").off("click");
        $("#btnOkSearchVehicleTypeAdv").on("click", this.OkSearchVehicleTypeAdv);
        $("#btnCancelVehicleTypeAdv").on("click", this.CancelSearchAdv)
    }

    static HideSearchAdv() {
        dropDownSearchAdvVehicleType.hide();
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }
}
