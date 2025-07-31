var gblindex = null;
var gblInternationalOperation;
var InternationalOperation = {};
var countryId = null;
var stateId = null;
var sarlaftId = null;
var sarlaftOperationId = null;
var sarlaftStatus = null;
var glbDeletedInternationalOperations = [];

class InternationalOperations extends Uif2.Page {
    getInitialState() {
        $("#listInternationalOperations").UifListView({
            source: null,
            customDelete: true,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: true,
            displayTemplate: "#ListInternationalOperationsTemplate",
            height: 200
        });
        InternationalOperations.InitialOperations();
        $("#Entity").TextTransform(ValidatorType.UpperCase);
        $("#ProductNumber").on('keypress', InternationalOperations.OnlyNumbers);
        $("#ProductAmount").on('keypress', InternationalOperations.OnlyNumbers);
        $('#State').UifSelect();
        $("#Town").UifSelect();
    }

    //Seccion Eventos
    bindEvents() {
        $('#CountryOrigin').on('itemSelected', this.GetStates);
        $('#State').on('itemSelected', this.GetCities);
        $('#btnInternationalOperationsSave').on('click', InternationalOperations.ExecuteOperations);
        $('#listInternationalOperations').on('rowEdit', InternationalOperations.ToEditItem);
        $('#listInternationalOperations').on('rowDelete', InternationalOperations.DeleteItem);

        $("#ProductAmount").focusout(function () {
            var num = parseInt($(this).val());
            if (!isNaN(num)) {
                var form = FormatMoney(num);
                $(this).val(form);
            }
        });
    }

    static InitialOperations() {

        SarlaftRequest.GetCountry().done(function (data) {
            if (data.success) {
                $('#CountryOrigin').UifSelect({ sourceData: data.result });
                $('#CountryOrigin').UifSelect("setSelected", 1);
                InternationalOperations.GetStateDB($('#CountryOrigin').UifSelect("getSelected"));
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetOperationTypes().done(function (response) {
            if (response.success) {
                $("#OperationType").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetProductType().done(function (response) {
            if (response.success) {
                $("#ProductType").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetCurrencies().done(function (response) {
            if (response.success) {
                $("#Currency").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static Validate() {

        var msj = "";

        if ($("#Town").UifSelect("getSelected") === null || $("#Town").UifSelect("getSelected") === "") {
            msj = AppResources.Town + "<br>";
        }

        if ($("#State").UifSelect("getSelected") === null || $("#State").UifSelect("getSelected") === "") {
            msj = AppResources.LabelState + "<br>";
        }

        if ($("#CountryOrigin").UifSelect("getSelected") === null || $("#CountryOrigin").UifSelect("getSelected") === "") {
            msj = AppResources.LabelCountryOrigin + "<br>";
        }

        if ($("#Currency").UifSelect("getSelected") === null || $("#Currency").UifSelect("getSelected") === "") {
            msj = AppResources.Currency + "<br>";
        }

        if ($("#ProductAmount").val() === null || $("#ProductAmount").val() === "") {
            msj = AppResources.ProductAmount + "<br>";
        }

        if ($("#Entity").val() === null || $("#Entity").val() === "") {
            msj = AppResources.LabelEntity + "<br>";
        }

        if ($("#ProductNumber").val() === null || $("#ProductNumber").val() === "") {
            msj = AppResources.ProductNumber + "<br>";
        }

        if ($("#ProductNumber").val() === null || $("#ProductNumber").val() === "") {
            msj = AppResources.ProductNumber + "<br>";
        }

        if ($("#ProductType").UifSelect("getSelected") === null || $("#ProductType").UifSelect("getSelected") === "") {
            msj = AppResources.ProductType + "<br>";
        }

        if ($("#OperationType").UifSelect("getSelected") === null || $("#OperationType").UifSelect("getSelected") === "") {
            msj = AppResources.OperationType + "<br>";
        }

        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }

        return true;
    }

    static ClearFields() {
        $("#OperationType").val("");
        $("#ProductType").val("");
        $("#ProductNumber").val("");
        $("#Entity").val("");
        $("#ProductAmount").val("");
        $("#Currency").val("");
        $("#CountryOrigin").val("");
        $("#State").val("");
        $("#Town").val("");
        $("#StatusOperation").val("");
        $("#InternationalOperationId").val("");
    }

    static GetInternationalOperations() {
        if (sarlaftId !== null || newSarlaftId !== null) {
            if (sarlaftId == null) {
                sarlaftId = newSarlaftId;
            }
            var elementsEdit = false;
            if (gblSarlaft.InternationalOperationDTO != undefined) {
                if (gblSarlaft.InternationalOperationDTO.length > 0) {
                    if (gblSarlaft.InternationalOperationDTO.find(x => x.Status == 3 || x.Status == 2) != undefined) {
                        elementsEdit = true;
                    }
                }
            }
            if (elementsEdit) {
                InternationalOperations.FillDataInternationalOperations(gblSarlaft.InternationalOperationDTO);
            }
            else {
                SarlaftRequest.GetInternationalOperationsBySarlaftId(sarlaftId).done(function (data) {
                    if (data.success) {
                        InternationalOperations.FillDataInternationalOperations(data.result);
                        //InternationalOperations.ClearFields();
                        //gblInternationalOperation = data.result;
                        //$("#listInternationalOperations").UifListView("refresh");
                        //$.each(gblInternationalOperation, function (index, item) {
                        //    $("#listInternationalOperations").UifListView("addItem", item);
                        //});
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

    static FillDataInternationalOperations(data) {
        gblInternationalOperation = data;
        if (gblInternationalOperation.length > 0) {
            $("#OperationType").UifSelect('setSelected', gblInternationalOperation[0].OperationTypeId);
            $("#ProductType").UifSelect('setSelected', gblInternationalOperation[0].ProductTypeId);
            $("#ProductNumber").val(gblInternationalOperation[0].ProductNum);
            $("#Entity").val(gblInternationalOperation[0].Entity);
            $("#ProductAmount").val(gblInternationalOperation[0].ProductAmt);
            $("#Currency").UifSelect('setSelected', gblInternationalOperation[0].CurrencyId);
            $("#CountryOrigin").UifSelect('setSelected', gblInternationalOperation[0].CountryId);
            $("#State").UifSelect('setSelected', gblInternationalOperation[0].StateId);
            $('#StatusOperation').val(gblInternationalOperation[0].Status);
            $('#InternationalOperationId').val(gblInternationalOperation[0].Id);
            var CityData = InternationalOperations.GetCityOI();

            $.when(CityData).done(function (CityData) {
                $("#Town").UifSelect('setSelected', gblInternationalOperation[0].CityId);
                
            });

            
        }
        var listIntOperations = [];

        $("#listInternationalOperations").UifListView("refresh");
        if (glbDeletedInternationalOperations.length > 0) {
            $.each(gblInternationalOperation, function (index, item) {
                InternationalOperations.IsDeletedIntOperation(item, glbDeletedInternationalOperations, listIntOperations);
            });
        }

        $.each(gblInternationalOperation, function (index, item) {
            $("#listInternationalOperations").UifListView("addItem", item);
        });
    }

    static IsDeletedIntOperation(operation, arrDeletedOperations, listOperations) {

        var found = arrDeletedOperations.some(function (item) {
            return item.Id === operation.Id;
        });
        if (!found) {
            listOperations.push(operation);
            gblInternationalOperation = listOperations;
        }
    }

    static ExecuteOperations() {
        if (InternationalOperations.Validate()) {
            InternationalOperation = {};
            InternationalOperation.OperationTypeId = $("#OperationType").UifSelect("getSelected");
            InternationalOperation.OperationDescription = $("#OperationType").UifSelect("getSelectedText");
            InternationalOperation.ProductTypeId = $("#ProductType").UifSelect("getSelected");
            InternationalOperation.ProductDescription = $("#ProductType").UifSelect("getSelectedText");
            InternationalOperation.ProductNum = $("#ProductNumber").val();
            InternationalOperation.Entity = $("#Entity").val();
            InternationalOperation.ProductAmt = NotFormatMoney($("#ProductAmount").val());
            InternationalOperation.CurrencyId = $("#Currency").UifSelect("getSelected");
            InternationalOperation.CountryId = $("#CountryOrigin").UifSelect("getSelected");
            InternationalOperation.CityId = $("#Town").UifSelect("getSelected");
            InternationalOperation.StateId = $("#State").UifSelect("getSelected");
            InternationalOperation.SarlaftId = sarlaftId;
            InternationalOperation.Status = $("#StatusOperation").val();

            if (InternationalOperation.Status != "") {
                if (InternationalOperation.Status == 1) {
                    InternationalOperation.Id = $("#InternationalOperationId").val();
                    if (newSarlaftId == null) {
                        InternationalOperation.Status = 3;
                    }
                    else{
                        InternationalOperation.Status = 2;
                    }
                }
                else if (InternationalOperation.Status == 3) {
                    InternationalOperation.Id = $("#InternationalOperationId").val();
                }
                if (gblindex == null) {
                    gblindex = 0;
                }
                $("#listInternationalOperations").UifListView("editItem", gblindex, InternationalOperation);

            }
            else if (InternationalOperation.Status === "") {
                InternationalOperation.Status = 2;
                $("#listInternationalOperations").UifListView("addItem", InternationalOperation);
            }

            InternationalOperations.Save(InternationalOperation);
        }
    }

    static Save(data) {
        //SarlaftRequest.ExecuteOperation(data).done(function (data) {
        //    if (data.success) {
        gblSarlaft.InternationalOperationDTO = $("#listInternationalOperations").UifListView('getData');

        if (glbDeletedInternationalOperations.length > 0) {
            $.each(glbDeletedInternationalOperations, function (index, itemDeleted) {
                var found = gblSarlaft.InternationalOperationDTO.some(function (item) {
                    return item.Id === itemDeleted.Id;
                });
                if (!found) {
                    gblSarlaft.InternationalOperationDTO.push(itemDeleted);
                }
            });
        }

        $.UifNotify('show', { 'type': 'info', 'message': AppResources.InternationalOperationSuccessfully, 'autoclose': true });
        //InternationalOperations.ClearFields();
        $("#listInternationalOperations").UifListView("refresh");
        //InternationalOperations.GetInternationalOperations();
        $('#modalInternationalOperations').UifModal('hide');
        //} else {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.InternationalOperationSuccessfully, 'autoclose': true });
        //}
        //});
    }

    static ToEditItem(event, data, index) {
        gblindex = index;
        $('#InternationalOperationId').val(data.Id);
        $("#StatusOperation").val(data.Status);
        $("#OperationType").UifSelect("setSelected", data.OperationTypeId);
        $("#ProductType").UifSelect("setSelected", data.ProductTypeId);
        $("#ProductNumber").val(data.ProductNum);
        $("#Entity").val(data.Entity);
        $("#ProductAmount").val(data.ProductAmt);
        $("#Currency").UifSelect("setSelected", data.CurrencyId);
        $("#CountryOrigin").UifSelect("setSelected", data.CountryId);
        SarlaftRequest.GetState(data.CountryId).done(function (result) {
            if (result.success) {
                $('#State').UifSelect({ sourceData: result.result });
                $("#State").UifSelect("setSelected", data.StateId);
                SarlaftRequest.GetCities(data.CountryId, data.StateId).done(function (result2) {
                    if (result2.success) {
                        $("#Town").UifSelect({ sourceData: result2.result });
                        $("#Town").UifSelect("setSelected", data.CityId);
                    }
                });
            }
        });

    }

    static DeleteItem(event, data) {
        $.UifDialog('confirm', {
            title: AppResources.WishContinue,
            message: AppResources.RemoveInternationalOperation
        }, function (result) {
            if (result) {
                InternationalOperation = data;
                InternationalOperation.Status = 4;
                glbDeletedInternationalOperations.push(data);
                //InternationalOperations.Save(InternationalOperation);
                const index = $("#listInternationalOperations").UifListView("findIndex", (x) => { return x.Id === data.Id; });
                $("#listInternationalOperations").UifListView("deleteItem", index);
            }
            $('#modalInternationalOperations').UifModal('hide');
        });
    }

    static OnlyNumbers(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            }
        }
    }

    GetStates(event, selectedItem) {
        countryId = selectedItem.Id;
        if (countryId) {
            InternationalOperations.GetStateDB(countryId);
        }
        else {
            $('#State').UifSelect();
            $("#Town").UifSelect();
        }
    }

    static GetStateDB(countryId) {
        SarlaftRequest.GetState(countryId).done(function (data) {
            if (data.success) {
                $('#State').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    GetCities(event, selectedItem) {
        countryId = $("#CountryOrigin").val();
        stateId = selectedItem.Id;

        if (stateId) {
            SarlaftRequest.GetCities(countryId, stateId).done(function (data) {

                if (data.success) {
                    $("#Town").UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $("#Town").UifSelect();
        }
    }

    static GetSarlaftId() {
        if (gblSarlaft.SarlaftDTO !== null && gblSarlaft.SarlaftDTO !== undefined) {
            sarlaftId = gblSarlaft.SarlaftDTO.Id;
        }
    }

    static DisabledInternationalOperations() {
        $("#ProductNumber").prop("disabled", true);
        $('#Entity').prop("disabled", true);
        $('#ProductAmount').prop("disabled", true);
        $('#OperationType').UifSelect("disabled", true);
        $('#ProductType').UifSelect("disabled", true);
        $('#Currency').UifSelect("disabled", true);
        $('#CountryOrigin').UifSelect("disabled", true);
        $('#State').UifSelect("disabled", true);
        $('#Town').UifSelect("disabled", true);

        $("#listInternationalOperations").UifListView({
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ListInternationalOperationsTemplate",
            height: 200
        });

        $("#btnInternationalOperationsSave").hide();
    }

    static EnabledInternationalOperations() {
        $("#ProductNumber").prop("disabled", false);
        $('#Entity').prop("disabled", false);
        $('#ProductAmount').prop("disabled", false);
        $('#OperationType').UifSelect("disabled", false);
        $('#ProductType').UifSelect("disabled", false);
        $('#Currency').UifSelect("disabled", false);
        $('#CountryOrigin').UifSelect("disabled", false);
        $('#State').UifSelect("disabled", false);
        $('#Town').UifSelect("disabled", false);
        $("#btnInternationalOperationsSave").show();

        $("#listInternationalOperations").UifListView({
            customDelete: true,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: true,
            displayTemplate: "#ListInternationalOperationsTemplate",
            height: 200
        });
    }

    static GetCityOI() {
        var dfd = $.Deferred();
        var countryIdL = $('#CountryOrigin').UifSelect("getSelected");
        var StatedIdL = $('#State').UifSelect("getSelected");
        SarlaftRequest.GetCities(countryIdL, StatedIdL).done(function (data) {
            if (data.success) {
                $("#Town").UifSelect({ sourceData: data.result });
                
                dfd.resolve(data.result);
            }
            else
                dfd.reject();
        });
        return dfd.promise();
    }
}