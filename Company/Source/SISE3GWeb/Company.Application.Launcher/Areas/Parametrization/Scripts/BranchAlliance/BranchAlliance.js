var gblBranchAlliances = [];
var gblSalesPointsAlliances = [];
var gblSalesPointsAlliancesDelete = [];
var objectBranchIndex = null;
var objectBranchStatus = null;
var objectSalePointIndex = null;
var objectSalePointStatus = null;
var objectsData = null;
var idState;
var idCity;
var modified = false;
var dropDownSearchAdvBranchAlliance = null;

$(() => {
    new BranchAlliance();
});

class BranchAlliance extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        BranchAlliance.GetAlliance().done(function (data) {
            if (data.success) {
                $("#selectAllied").UifSelect({ sourceData: data.result });
            }
        });
        BranchAlliance.LoadCountries();
        dropDownSearchAdvBranchAlliance = uif2.dropDown({
            source: rootPath + 'Parametrization/BranchAlliance/BranchAllianceSearch',
            element: '#inputBranchAllied',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: function () { }
        });

        $("#inputBranchSalePointCode").UifMask({
            pattern: BranchAlliance.OnlyNumbers(200)
        });
    }

    bindEvents() {
        $('#btnNewBranchAlliance').click(BranchAlliance.ClearBranch);
        $('#btnNewSalesPointAlliance').click(BranchAlliance.ClearSalePoint);
        $('#btnAddBranchAlliance').click(this.AddBranchAlliance);
        $('#btnAddSalesPointAlliance').click(this.AddSalePointAlliance);
        $('#listViewBranchAlliance').on('rowEdit', BranchAlliance.ShowBranchData);
        $('#listViewBranchAlliance').on('rowDelete', BranchAlliance.EventDeleteBranchAlliance);
        $('#listViewSalesPointAlliance').on('rowEdit', BranchAlliance.ShowSalePoint);
        $('#btnSalePointAlliance').click(this.ShowModal);
        $('#btnExitBranch').click(this.ExitBranch);
        $('#selectAllied').on('itemSelected', BranchAlliance.LoadBranches);
        $('#selectCountry').on('itemSelected', BranchAlliance.LoadStates);
        $('#selectState').on('itemSelected', BranchAlliance.LoadCities);
        $('#btnSaveBranch').click(this.SaveBranchAlliances);
        $('#btnSaveSalePoint').click(BranchAlliance.SetListSalesPoints);
        $('#inputBranchAllied').on('buttonClick', this.SearchBranchAlliance);
        $('#btnExitSalesPoint').click(this.Exit);
        $('#btnExportBranch').click(this.ExportFileBranchAlliance);
        $('#btnExportSalesPoint').click(this.ExportFileSalePoints);
    }

    AddBranchAlliance() {
        $("#formBranchAlliance").validate();
        if ($("#formBranchAlliance").valid()) {
            var branch = BranchAlliance.GetBranchForm();
            if (branch.SalesPoints.length <= 0 || objectsData.length <= 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMustSalePoint, 'autoclose': true })
            }
            else {
                branch.lblCity = Resources.Language.LabelCity;
                branch.StateName = $('#selectState option:selected').text();
                branch.CityName = $('#selectCity option:selected').text();
                if (objectBranchIndex == null) {
                    if (BranchAlliance.ValidBranch(branch)) {
                        BranchAlliance.SetStatus(branch, "create");
                        $("#listViewBranchAlliance").UifListView("addItem", branch);
                    }
                }
                else {
                    if (BranchAlliance.ValidBranch(branch)) {
                        if (objectBranchIndex != undefined && objectBranchStatus == "create") {
                            BranchAlliance.SetStatus(branch, objectBranchStatus);
                        } else {
                            BranchAlliance.SetStatus(branch, "update");
                        }
                        $('#listViewBranchAlliance').UifListView('editItem', objectBranchIndex, branch);
                    }
                }
                BranchAlliance.ClearBranch();
            }
        }
    }

    static OnlyNumbers(max) {
        var maxLength = '';
        for (var i = 0; i < max; i++) {
            maxLength = maxLength + '0';
        }
        return maxLength;
    }

    AddSalePointAlliance() {
        $("#formSalesPointAlliance").validate();
        if ($("#formSalesPointAlliance").valid()) {
            var salePoint = BranchAlliance.GetSalePointForm();
            if (objectSalePointIndex == null) {
                var lista = $("#listViewSalesPointAlliance").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.SalePointId == salePoint.SalePointId;
                });
                if (ifExist.length > 0 && objectSalePointStatus != "create") {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistCodeSalePoint, 'autoclose': true });
                }
                else {
                    BranchAlliance.SetStatus(salePoint, "create");
                    $("#listViewSalesPointAlliance").UifListView("addItem", salePoint);
                }
            }
            else {
                if (objectSalePointIndex != undefined && objectSalePointStatus != undefined) {
                    BranchAlliance.SetStatus(salePoint, objectSalePointStatus);
                } else {
                    BranchAlliance.SetStatus(salePoint, "update");
                }
                $('#listViewSalesPointAlliance').UifListView('editItem', objectSalePointIndex, salePoint);
            }
            BranchAlliance.ClearSalePoint();
        }
    }

    ShowModal() {
        $("#formBranchAlliance").validate();
        if ($("#formBranchAlliance").valid()) {
            var branchAllied = BranchAlliance.GetBranchForm();
            $("#listViewSalesPointAlliance").UifListView({ displayTemplate: "#SalesPointTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: BranchAlliance.DeleteSalePoint, height: 300 });
            if (branchAllied.SalesPoints.length > 0) {
                $.each(branchAllied.SalesPoints, function (key, value) {
                    var branch =
                        {
                            SalePointId: this.SalePointId,
                            SalePointDescription: this.SalePointDescription,
                            BranchName: branchAllied.BranchDescription,
                            AlliedName: branchAllied.AlliedName
                        };
                    $("#listViewSalesPointAlliance").UifListView("addItem", branch);
                });
            }
            $("#inputSalePointName").val(branchAllied.AllianceName);
            $("#inputSalePointBranchName").val(branchAllied.BranchDescription);
            $('#ModalSalesPointsAlliance').UifModal('showLocal', 'Punto de Venta Aliado');
            objectsData = branchAllied.SalesPoints;
        }
    }

    ExitBranch() {
        window.location = rootPath + "Home/Index";
    }

    SaveBranchAlliances() {
        BranchAlliance.SetBranchListToSend();
        $.ajax({
            type: "POST",
            url: 'SaveBranchAlliances',
            data: JSON.stringify({ lstBranchAlliances: gblBranchAlliances }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            error: function (error) {
                //alert('Error occurs!' + error.responseText);
                var x = error.responseText;
            }
        }).done(function (data) {
            if (data.success) {
                gblBranchAlliances = [];
                BranchAlliance.LoadBranches();
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SavedBranchAlliance, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSavingBranchAlliance, 'autoclose': true });
            }
            });
    }

    SearchBranchAlliance() {
        var inputBranchAllied = $('#inputBranchAllied').val();
        if (inputBranchAllied.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else if (inputBranchAllied.length > 15) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMaximumChar, autoclose: false });
        }
        else {
            $.ajax({
                type: "POST",
                url: 'GetBranchAllianceByDescription',
                data: JSON.stringify({ description: inputBranchAllied }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var branches = data.result;
                    if (branches.length == 0) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchAllianceNotExist, 'autoclose': true });
                    }
                    else {
                        if (branches.length > 1) {
                            BranchAlliance.ShowSearchAdv(branches);
                        }
                        else {
                            $.each(branches, function (key, value) {
                                var lista = $("#listViewBranchAlliance").UifListView('getData')
                                var index = lista.findIndex(function (item) {
                                    return item.BranchCode == value.BranchCode;
                                });
                                $('#selectAllied').UifSelect("setSelected", value.AllianceId);
                                BranchAlliance.ShowBranchData(null, value, index);
                                BranchAlliance.GetBranchAlliance($('#selectAllied').UifSelect("getSelected")).done(function (data) {
                                    if (data.success) {
                                        BranchAlliance.LoadListViewBranchAlliance(data);
                                    }
                                });
                                BranchAlliance.GetSalesPoints(data.BranchId, data.AllianceId).done(function (data) {
                                    if (data.success) {
                                        $.each(data.result, function (key, value) {
                                            var branch =
                                                {
                                                    SalePointId: this.SalePointId,
                                                    SalePointDescription: this.SalePointDescription,
                                                    BranchName: result.BranchDescription,
                                                    AlliedName: result.AlliedName
                                                };
                                            gblSalesPointsAlliances.push(branch);
                                        });
                                    }
                                });
                            });
                        }
                    }
                    $('#inputBranchAllied').val('');
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    Exit() {
        BranchAlliance.ClearSalePoint();
        $('#ModalSalesPointsAlliance').UifModal('hide');
    }

    ExportFileBranchAlliance() {
        var allianceId = $('#selectAllied').UifSelect('getSelected');
        if (allianceId != '') {
            BranchAlliance.GenerateFileBranchesToExport(allianceId).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Seleccione un aliado', 'autoclose': true });
        }
    }

    ExportFileSalePoints() {
        BranchAlliance.GenerateFileSalesPointsToExport().done(function (data) {
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

    static GenerateFileBranchesToExport(allianceId) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileBranchesToExport',
            data: JSON.stringify({ allianceId: allianceId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileSalesPointsToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileSalesPointsToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SetBranchListToSend() {
        var objects = $('#listViewBranchAlliance').UifListView('getData');
        $.each(objects, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.Status != undefined && value.Status !="Original") {
                gblBranchAlliances.push(value);
            }
        });
    }

    static GetAlliance() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Alliance/GetAllAlliances',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static LoadCountries() {
        CountryRequest.GetCountries().done(function (data) {
            if (data.success) {
                $("#selectCountry").UifSelect({ sourceData: data.result });
            }
        });
    }

    static LoadStates() {
        StateRequest.GetStatesByCountryId($('#selectCountry').UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                var source = data.result;
                $("#selectState").UifSelect({ sourceData: source });
                $('#selectCity').UifSelect({ sourceData: null });
                if (modified) {
                    $('#selectState').UifSelect("setSelected", idState);
                    BranchAlliance.LoadCities();
                }
            }
        });
    }

    static LoadCities() {
        StateRequest.GetCitiesByCountryIdStateId($('#selectCountry').UifSelect("getSelected"), $('#selectState').UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                var source = data.result;
                $("#selectCity").UifSelect({ sourceData: source });
                if (modified) {
                    $('#selectCity').UifSelect("setSelected", idCity);
                    modified = false;
                }
            }
        });
    }

    static LoadBranches() {
        BranchAlliance.GetBranchAlliance($('#selectAllied').UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                BranchAlliance.LoadListViewBranchAlliance(data);
            }
        });
        BranchAlliance.ClearBranch();
    }

    static LoadListViewBranchAlliance(data) {
        $("#listViewBranchAlliance").UifListView({ displayTemplate: "#BranchAllianceTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(data.result, function (key, value) {
            var branch =
                {
                    BranchId: this.BranchId,
                    BranchDescription: this.BranchDescription,
                    AllianceId: this.AllianceId,
                    CityCD: this.CityCD,
                    CityName: this.CityName,
                    CountryCD: this.CountryCD,
                    CountryName: this.CountryName,
                    StateCD: this.StateCD,
                    StateName: this.StateName,
                    lblCity: Resources.Language.LabelCity,
                    Status: "Original"
                };
            $("#listViewBranchAlliance").UifListView("addItem", branch);
        });
    }

    static GetBranchAlliance(allianceId) {
        return $.ajax({
            type: 'POST',
            url: 'GetBranchsByAlliancesId',
            data: JSON.stringify({ allianceId: allianceId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ClearBranch() {
        $("#inputBranchAlliedDescription").val("");
        $("#inputBranchCode").val("");       
        $('#selectCity').UifSelect({ sourceData: null });
        $('#selectState').UifSelect({ sourceData: null });
        $('#selectCountry').UifSelect("setSelected", null);
        gblSalesPointsAlliances = [];
        objectBranchIndex = null;
       
        ClearValidation("#formBranchAlliance");
    }

    static EventDeleteBranchAlliance(event, result, index) {
        var lista = $("#listViewBranchAlliance").UifListView('getData');
        $.each(lista, function (key, value) {
            if (result.BranchId == value.BranchId) {
                index = key;
                BranchAlliance.GetSalesPoints(result.BranchId, result.AllianceId).done(function (data) {
                    if (data.success) {
                        $.each(data.result, function (key, value) {
                            var branch =
                                {
                                    SalePointId: this.SalePointId,
                                    SalePointDescription: this.SalePointDescription,
                                    BranchName: result.BranchDescription,
                                    AlliedName: result.AlliedName
                                };
                            gblSalesPointsAlliances.push(branch);
                        });
                    }
                    result.SalesPoints = gblSalesPointsAlliances;
                });
            }
        });
        if ($("#listViewBranchAlliance").find(".item.active").length > 0) {
        } else {
            $("#listViewBranchAlliance").find('div .item:eq(' + index + ')').addClass("active");
            $('<div class="delete"><div class="alert alert-danger alert-dismissible error" role="alert">Se eliminarán los puntos de venta. ¿Esta seguro que quiere eliminar el registro? <a onclick="BranchAlliance.DeleteBranchAlliance(' + index + ')">Eliminar</a> <span class="separator">|</span><a onclick="RemoveDelete()">Cancelar</a></div></div>').insertAfter($("#listViewBranchAlliance").find('div .display.columns')[index]);
        }
    }

    static ShowBranchData(event, result, index) {
        if (result.BranchId != undefined) {
            objectBranchIndex = index;
            objectBranchStatus = result.Status;
            $("#inputBranchCode").val(result.BranchId);
            $("#inputBranchAlliedDescription").val(result.BranchDescription);
            $('#selectCountry').UifSelect("setSelected", result.CountryCD);
            modified = true;
            idState = result.StateCD;
            idCity = result.CityCD;
            BranchAlliance.LoadStates();
            gblSalesPointsAlliances = [];
            if (result.SalesPoints != undefined) {
                gblSalesPointsAlliances = result.SalesPoints;
            } else {
                BranchAlliance.GetSalesPoints(result.BranchId, result.AllianceId).done(function (data) {
                    if (data.success) {
                        $.each(data.result, function (key, value) {
                            var branch =
                                {
                                    SalePointId: this.SalePointId,
                                    SalePointDescription: this.SalePointDescription,
                                    BranchName: result.BranchDescription,
                                    AlliedName: result.AlliedName
                                };
                            gblSalesPointsAlliances.push(branch);
                        });
                        objectsData = gblSalesPointsAlliances;
                    }
                });
            }
        }
    }

    static ShowSalePoint(event, result, index) {
        if (result.SalePointId != undefined) {
            objectSalePointIndex = index;
            objectSalePointStatus = result.Status;
            $("#inputBranchSalePointCode").val(result.SalePointId);
            $("#inputSalePontAlliedDescription").val(result.SalePointDescription);
        }
    }

    static DeleteBranchAlliance(index) {
        var branch = $("#listViewBranchAlliance").UifListView("getData")[index];
       
            $("#listViewBranchAlliance").UifListView("deleteItem", index);
            if (branch.BranchId != 0 && branch.BranchId != undefined && branch.Status == "Original") {
            branch.Status = "delete";
            BranchAlliance.GetSalesPoints(branch.BranchId, branch.AllianceId).done(function (data) {
                //gblBranchAlliances.push(branch);
                branch.allowEdit = false;
                branch.allowDelete = false;
                $("#listViewBranchAlliance").UifListView("addItem", branch);
                BranchAlliance.ClearBranch();
            });
        }
    }

    static DeleteSalePoint(event, data, index) {
        if (data.SalePointId != 0) {
            data.Status = "delete";
            gblSalesPointsAlliancesDelete.push(data);
            BranchAlliance.ClearSalePoint();
        }
        event.resolve();
    }

    static GetSalesPoints(branchId, allianceId) {
        return $.ajax({
            type: 'POST',
            url: 'GetSalesPointsByBranchId',
            data: JSON.stringify({ branchId: branchId, allianceId: allianceId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ClearSalePoint() {
        $("#inputBranchSalePointCode").val("");
        $("#inputSalePontAlliedDescription").val("");
    }

    static GetBranchForm() {
        var data = {
        };
        $("#formBranchAlliance").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.AllianceId = $('#selectAllied').UifSelect('getSelected');
        data.AllianceName = $('#selectAllied option:selected').text();
        data.BranchCode = $("#inputBranchCode").val();
        data.BranchDescription = $("#inputBranchAlliedDescription").val();
        data.CityCD = $('#selectCity').UifSelect("getSelected");
        data.StateCD = $('#selectState').UifSelect("getSelected");
        data.CountryCD = $('#selectCountry').UifSelect("getSelected");
        data.SalesPoints = gblSalesPointsAlliances;
        return data;
    }

    static GetSalePointForm() {
        var data = {
        };
        $("#formSalesPointAlliance").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.AlliedCode = $('#selectAllied').UifSelect("getSelected");
        data.SalePointId = $("#inputBranchSalePointCode").val();
        data.SalePointDescription = $("#inputSalePontAlliedDescription").val();
        data.BranchName = $("#inputSalePointBranchName").val();
        data.AlliedName = $("#inputSalePointName").val();
        return data;
    }

    static SetStatus(object, status) {
        object.Status = status;
    }

    static SetListSalesPoints() {
        objectsData = $('#listViewSalesPointAlliance').UifListView('getData');
        if (gblSalesPointsAlliancesDelete.length > 0 && objectsData.length > 0) {
            gblSalesPointsAlliances = gblSalesPointsAlliancesDelete;
        }

        $.each(objectsData, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.Status != undefined) {
                gblSalesPointsAlliances.push(value);
            }
        });
        BranchAlliance.ClearSalePoint();
        $('#ModalSalesPointsAlliance').UifModal('hide');
    }

    static CancelSearchAdv() {
        dropDownSearchAdvBranchAlliance.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvBranchAlliance").UifListView("getSelected");
        if (data.length === 1) {
            var branch =
                {
                    BranchId: data[0].BranchId,
                    BranchDescription: data[0].BranchDescription,
                    AllianceId: data[0].AllianceId,
                    CityCD: data[0].CityCD,
                    CityName: data[0].CityName,
                    CountryCD: data[0].CountryCD,
                    CountryName: data[0].CountryName,
                    StateCD: data[0].StateCD,
                    StateName: data[0].StateName,
                    lblCity: Resources.Language.LabelCity
                };
            var lista = $("#listViewAlliance").UifListView('getData');
            var index = lista.findIndex(function (item) {
                return item.AlliedCode == branch.AlliedCode;
            });
            $('#selectAllied').UifSelect("setSelected", branch.AllianceId);
            BranchAlliance.ShowBranchData(null, branch, index);
            BranchAlliance.GetBranchAlliance($('#selectAllied').UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    BranchAlliance.LoadListViewBranchAlliance(data);
                }
            });
            BranchAlliance.GetSalesPoints(branch.BranchId, branch.allianceId).done(function (data) {
                if (data.success) {
                    $.each(data.result, function (key, value) {
                        var branch =
                            {
                                SalePointId: this.SalePointId,
                                SalePointDescription: this.SalePointDescription,
                                BranchName: result.BranchDescription,
                                AlliedName: result.AlliedName
                            };
                        gblSalesPointsAlliances.push(branch);
                    });
                }
            });
        }
        dropDownSearchAdvBranchAlliance.hide();
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvBranchAlliance").UifListView({
            displayTemplate: "#BranchAllianceSearchTemplate",
            selectionType: "single",
            height: 300
        });
        $("#btnCancelSearchAdv").on("click", BranchAlliance.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", BranchAlliance.OkSearchAdv);
        $("#lvSearchAdvBranchAlliance").UifListView("clear");
        if (data) {
            data.forEach(item => {
                item.lblCity = Resources.Language.LabelCity;
                $("#lvSearchAdvBranchAlliance").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvBranchAlliance.show();
    }

    static ValidBranch(branch) {
        var lista = $("#listViewBranchAlliance").UifListView('getData');
        var valid = true;
        var ifExistId = lista.filter(function (item) {
            return item.BranchId == branch.BranchId;
        });
        var ifExistDescription = lista.filter(function (item) {
            return item.BranchDescription == branch.BranchDescription;
        });
        if ((ifExistId.length > 0 || ifExistDescription.length > 0) && objectBranchStatus != "create" && objectBranchIndex == undefined) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorExistCodeBranchAlliance + ' o ' +
                Resources.Language.ErrorExistNameBranchAlliance, 'autoclose': true
            });
            valid = false;
        }
        return valid;
    }
}

function RemoveDelete() {
    $("#listViewBranchAlliance .delete").remove();
    $("#listViewBranchAlliance").find(".item.active").removeClass("active");
}