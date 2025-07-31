var glbObjectsDelete = [];
var objectIndex = null;
var formSalePoint = {};
var index;
var list;
var listViewSearch = [];
var busqueda = false;
$.ajaxSetup({ async: true });

class SalePointParametrization extends Uif2.Page {
    /**
  * @summary 
     *  Metodo que se ejecuta al instanciar la clase     
  */
    getInitialState() {
        //Se cargan los datos en los campos iniciales
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        request('Parametrization/SalePoint/GetBranch', null, 'GET', Resources.Language.ErrorExistSalePoint, SalePointParametrization.LoadBranch);
        request('Parametrization/SalePoint/GetSalePointByDescription', null, 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.GetAllSalesPoint);
    }

    /**
   * @summary 
       *  Metodo con los eventos de todos los controles 
   */
    bindEvents() {
        $("#inputIntPointCode").ValidatorKey(ValidatorType.Number, 2, 0);
        $('#btnNewObject').click(SalePointParametrization.ClearListView);
        $('#inputSalePoint').on('buttonClick', this.SearchSalePoint);
        $('#btnExit').click(this.Exit);
        $('#btnAcceptObject').click(this.AddObject);
        $('#btnSave').click(this.SaveSalePointes);
        $('#listViewSalePoint').on('rowEdit', SalePointParametrization.ShowData);
        $('#listViewSalePoint').on('rowDelete', (event, data, index) => { this.DeleteItemSalePointt(event, data, index) });
        $('#btnExport').click(this.sendExcelSalePoint);
        $('#selectBranch').on("itemSelected", SalePointParametrization.SalePoint);
    }

    static SalePoint(e, branchCode) {
        request('Parametrization/SalePoint/GetSalePointsByBranchCode', JSON.stringify({ branchCode: branchCode.Id }), 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.LoadListViewSalePoint);
    }

    static SalePointSearch(branchCode) {
        listViewSearch = branchCode;
        busqueda = true;
        request('Parametrization/SalePoint/GetSalePointsByBranchCode', JSON.stringify({ branchCode: branchCode.BranchId }), 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.LoadListViewSalePoint);
    }

    AddObject() {
        $("#formSalePoint").validate();
        if ($("#formSalePoint").valid()) {
            var object = SalePointParametrization.GetForm();
            object.Id = parseInt($("#inputIntPointCode").val());
            if (objectIndex == null) {
                var lista = $("#listViewSalePoint").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == object.Description.toUpperCase() && item.BranchId.toString() == $("#selectBranch").UifSelect("getSelected")
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExistNameSalePoint, 'autoclose': true });
                }
                else {
                    object.Status = ParametrizationStatus.Create;
                    $("#listViewSalePoint").UifListView({ displayTemplate: "#SalePointTemplate", edit: true, delete: true, customAdd: true, customEdit: true, height: 300, deleteCallback: SalePointParametrization.DeleteItemSalePointt, });
                    $("#listViewSalePoint").UifListView("addItem", object);
                }
            }
            else {
                var lista = $("#listViewSalePoint").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == object.Description.toUpperCase() && item.BranchId.toString() == $("#selectBranch").UifSelect("getSelected") && item.Id != $("#Id").val();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExistNameSalePoint, 'autoclose': true });
                }
                else {
                    if (object.Id == 0) {
                        object.Status = ParametrizationStatus.Create;
                    } else {
                        object.Status = ParametrizationStatus.Update;
                    }
                    $('#listViewSalePoint').UifListView('editItem', objectIndex, object);
                }
            }
            SalePointParametrization.Clear();
        }
    }


    static LoadBranch(data) {
        let comboConfig = { sourceData: data };
        $("#selectBranch").UifSelect(comboConfig);
    }

    SaveSalePointes() {
        var itemModified = [];
        var dataTable = $("#listViewSalePoint").UifListView('getData');
        $.each(glbObjectsDelete, function (index, item) {
            item.Status = ParametrizationStatus.Delete;
            itemModified.push(item);
        })
        $.each(dataTable, function (index, value) {
            if (value.Status == 0 || value.Status == undefined) {
                value.Status = ParametrizationStatus.Original
            }
            itemModified.push(value);
        });

        if (itemModified.length > 0) {
            request('Parametrization/SalePoint/SaveSalePointes', JSON.stringify({ salePoint: itemModified }), 'POST', Resources.Language.ErrorSaveSalePoint, SalePointParametrization.ResultSave);
        }
        glbObjectsDelete = [];
    }

    static ResultSave(data) {
        //$("#listViewSalePoint").UifListView("clear");
        request('Parametrization/SalePoint/GetSalePointByDescription', null, 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.GetAllSalesPoint);
        if (data.message != null && data.message != "") {
            $.UifNotify('show', { 'type': 'info', 'message': data.message, 'autoclose': true });
        }
    }

    static DeleteItemSalePointt(event, data) {
        event.resolve();
        if (data.Id !== 0 && data.Id !== "" && data.Id !== undefined) //Se elimina unicamente si existe en DB
        {
            data.Status = ParametrizationStatus.Delete;
            data.allowEdit = false;
            data.allowDelete = false;
            $("#listViewSalePoint").UifListView("addItem", data);
            //glbObjectsDelete.push(result);
        }

        SalePointParametrization.Clear();
    }

    SearchSalePoint(event, data, index) {
        var inputObject = $('#inputSalePoint').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }

        else {
            request('Parametrization/SalePoint/GetSalePointByDescription', JSON.stringify({ description: inputObject }), 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.ShowSearchAdv)
        }
        $('#inputSalePoint').val("");
        SalePointParametrization.ClearListView();
    }

    static GetAllSalesPoint(data) {
        if (data.length != 0) {
            $("#listViewSalePoint").UifListView({
                displayTemplate: "#SalePointTemplate",
                sourceData: data,
                edit: true,
                delete: true,
                customAdd: true,
                customEdit: true,
                height: 300,
                deleteCallback: SalePointParametrization.DeleteItemSalePointt
            });
        }
        else {
            $.UifNotify('show', { type: 'info', message: Resources.Language.TechnicalPlanNotFound, autoclose: true });
        }
    }

    static ShowSearchAdv(data) {
        if (data.length != 0) {
            $("#lvSearchAdvSalePoint").UifListView("clear");
            if (data.length === 1) {
                var list = $("#listViewSalePoint").UifListView('getData');
                $.each(list, function (key, value) {
                    if (value.Id == data[0].Id) {
                        index = key;
                    }
                });
                SalePointParametrization.SalePointSearch(data[0]);
                SalePointParametrization.ShowData(null, data[0], index);

            } else {
                $.each(data, function (key, value) {
                    var object =
                    {
                        Id: this.Id,
                        Description: this.Description,
                        SmallDescription: this.SmallDescription,
                        Branch: this.Branch,
                        BranchId: this.BranchId,
                        Enabled: this.Enabled
                    };
                    $("#lvSearchAdvSalePoint").UifListView("addItem", object);
                });
                SalePointAdvancedSearch.SearchAdvSalePoint();
            }
        } else {
            $.UifNotify('show', { type: 'info', message: Resources.Language.TechnicalPlanNotFound, autoclose: true });
        }
    }

    static DownloadFile(data) {
        DownloadFile(data);
    }

    sendExcelSalePoint() {
        request('Parametrization/SalePoint/GenerateFileToExport', null, 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.DownloadFile);
    }

    SelectSearch() {
        var list = $("#listViewSalePoint").UifListView('getData')
        var index = $(this).children()[0].innerHTML;
        var object = [];
        $.each(list, function (key, value) {
            if (value.Id == index) {
                object.push(value);
            }
        });
        SalePointParametrization.ShowData(null, object[0], index);
        $('#inputSalePoint').val("");
        $('#modalDefaultSearch').UifModal("hide");
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    static LoadListViewSalePoint(data) {
        if (data.length == 0) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.TechnicalPlanNotFound, autoclose: true });

        } else {
            $("#listViewSalePoint").UifListView({ displayTemplate: "#SalePointTemplate", sourceData: data, edit: true, delete: true, customAdd: true, customEdit: true, height: 300, deleteCallback: SalePointParametrization.DeleteItemSalePointt, });
            //$.each(data, function (key, value) {
            //    var object =
            //        {
            //            Id: this.Id,
            //            Description: this.Description,
            //            SmallDescription: this.SmallDescription,
            //            Branch: this.Branch,
            //            BranchId: this.BranchId
            //        };
            //    $("#listViewSalePoint").UifListView("addItem", object);
            //})
        }

        if (busqueda) {
            busqueda = false;
            let dataSearch = $("#lvSearchAdvSalePoint").UifListView("getSelected");
            list = $("#listViewSalePoint").UifListView('getData');
            if (dataSearch.length > 0) {
                $.each(list, function (key, value) {
                    if (value.Id == dataSearch[0].Id) {
                        index = key;
                    }
                });
            } else {
                $.each(list, function (key, value) {
                    if (value.Id == listViewSearch.Id) {
                        index = key;
                    }
                });
            }
            SalePointParametrization.ShowData(null, listViewSearch, index);

        }
    }

    static GetForm() {
        var data = {};
        $("#formSalePoint").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Description = $("#inputLongDescription").val();
        data.SmallDescription = $("#inputShortDescription").val();
        data.Id = $("#inputIntPointCode").val();
        data.BranchId = $("#selectBranch").UifSelect("getSelected");
        data.Branch = $("#selectBranch").UifSelect("getSelectedText");
        data.Enabled = $('#chkEnabledSalesPoint').prop('checked');

        return data;
    }

    static ShowData(event, result, index) {
        SalePointParametrization.Clear();
        if (result.Id != undefined) {
            objectIndex = index;
            $("#Id").val(result.Id);
            $("#inputIntPointCode").val(result.Id);
            $("#inputLongDescription").val(result.Description);
            $("#inputShortDescription").val(result.SmallDescription);
            $("#selectBranch").UifSelect("setSelected", result.BranchId);
            $("#chkEnabledSalesPoint").prop('checked', result.Enabled);
        }
    }
    /**
       * @summary 
       * Limpiar formulario
    */
    static Clear() {
        $("#Id").val("");
        $("#inputIntPointCode").val("");
        $("#inputLongDescription").val("");
        $("#inputShortDescription").val("");
        $("#selectBranch").UifSelect('setSelected', null);
        ClearValidation("#formSalePoint");
        $("#chkEnabledSalesPoint").prop('checked', false);
        objectIndex = null;
    }

    static ClearListView() {
        $("#Id").val("");
        $("#inputIntPointCode").val("");
        $("#inputLongDescription").val("");
        $("#inputShortDescription").val("");
        $("#selectBranch").UifSelect('setSelected', null);
        $("#chkEnabledSalesPoint").prop('checked', false);
        ClearValidation("#formSalePoint");
        objectIndex = null;
        request('Parametrization/SalePoint/GetSalePointByDescription', null, 'POST', Resources.Language.ErrorExistSalePoint, SalePointParametrization.GetAllSalesPoint);
    }

}