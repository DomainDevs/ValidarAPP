
/**
 * Variables Locales y Globales
 */
var marcaId = 0;
var modeloId = 0;
var versionId = 0;
var makeCodeVehicle = 0;
var modelCodeVehicle = 0;
var marcaName;
var modelName;
var versionName;
var gblFasecoldCodesList = [];
var gblFasecoldCodesListAdded = [];
var gblFasecoldCodesListModified = [];
var fasecoldaCodes = [];
var marcasList = [];
var fasecoldaIndex = null;
var fasecoldaAdvIndex = null;
var inputSearch = "";
var ObjFasecolda = {};
var validate = false;
var Search = false;

class Fasecolda extends Uif2.Page {

    /**
     * Funcion para inicilizar la vista
     */
    getInitialState() {
        $("#listViewFasecolda").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
        Fasecolda.GetMakes();
        $('#InputBrandCodeFasecolda').prop("disabled", true);
        $('#InputVersionCodeFasecolda').prop("disabled", true);
        //VehicleFasecolda.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(0, 0, 0, 0, 0);
    }

    /**
     * Eventos de los controles de la clase
     */
    bindEvents() {
        $("#btnShowAdvancedSearchFsecolda").on("click", this.ShowAdvanced);
        $("#selectBrandVehicle").on('itemSelected', Fasecolda.ChangeMake);
        $("#selectModelVehicle").on('itemSelected', Fasecolda.ChangeModel);
        $("#selectVersionVehicle").on('itemSelected', this.ChangeVersion);
        $("#btnAcceptFasecolda").on("click", this.AcceptFasecolda);
        $("#btnSaveFasecolda").on("click", this.Save);
        $('#listViewFasecolda').on('rowEdit', Fasecolda.ShowData);
        $("#btnAdvancedSearchFasecolda").on("click", this.SearchAdvFasecolda);
        $('#inputFasecoldaCode').on('buttonClick', this.SearchFasecolda);
        $("#listViewFasecolda").on('rowDelete', this.DeleteItemFasecolda);
        $("#btnCancelViewFasecolda").on("click", this.Exit);
        $("#btnExportXLSFasecolda").on("click", this.sendExcelFasecolda);
        $("#btnNewFasecolda").on("click", this.newFasecolda);

    }

    newFasecolda() {
        Fasecolda.Clear();
        $("#listViewFasecolda").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
        $('#inputFasecoldaCode').val(null);

    }

    sendExcelFasecolda() {
        Fasecolda.GenerateFileVehicleTypeToExport().done(function (data) {
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

    static GenerateFileVehicleTypeToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Vehicle/GenerateFileVehicleTypeToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    ShowAdvanced() {
        dropDownSearchAdvFasecolda.show();
    }

    static GetMakes(selectedId) {
        VehicleFasecolda.GetMakes().done(function (data) {
            if (data.success) {
                glbMakes = data.result;
                if (selectedId == 0) {
                    if (windowSearchAdv) {
                        $("#selectBrandVehicleAdvSearch").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectBrandVehicle").UifSelect({ sourceData: data.result });
                    }
                }
                else {
                    if (windowSearchAdv) {
                        $("#selectBrandVehicleAdvSearch").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                    else {
                        $("#selectBrandVehicle").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }

                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetModelsByMakeId(makeId, selectedId) {
        VehicleFasecolda.GetModelsByMakeId(makeId).done(function (data) {
            if (data.success) {
                glbModels = data.result;
                if (selectedId == 0) {
                    if (windowSearchAdv) {
                        $("#selectModelVehicleAdvSearch").UifSelect({ sourceData: data.result });
                        $("#selectModelVehicle").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectModelVehicle").UifSelect({ sourceData: data.result });
                    }

                }
                else {
                    if (windowSearchAdv) {
                        $("#selectModelVehicleAdvSearch").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                    else {
                        $("#selectModelVehicle").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }

                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ChangeMake(event, selectedItem) {
        //Fasecolda.Clear();
        if (selectedItem.Id > 0) {
            Fasecolda.GetModelsByMakeId(selectedItem.Id, 0);
            //VehicleFasecolda.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(selectedItem.Id, 0, 0, 0, 0);
        }
        else {

        }
        $('#inputFasecoldaCode').val('');
    }

    static ChangeModel(event, selectedItem) {
        if (selectedItem.Id > 0) {
            Fasecolda.GetVersionsByMakeIdModelId($("#selectBrandVehicle").UifSelect("getSelected"), selectedItem.Id, 0);
            VehicleFasecolda.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId($("#selectBrandVehicle").UifSelect("getSelected"), $("#selectModelVehicle").UifSelect("getSelected"), 0, 0, 0);
        }
        else {
            $('#selectVersion').UifSelect();
            $('#selectYear').UifSelect();
            $("#selectType").UifSelect();
            $('#inputPrice').val(0);
        }
        $('#inputFasecoldaCode').val('');
    }

    ChangeVersion(event, selectedItem) {
        $("#InputBrandCodeFasecolda").val(null);
        $("#InputVersionCodeFasecolda").val(null);
        if (selectedItem.Id > 0) {
            $("#InputBrandCodeFasecolda").val(null);
            $("#InputVersionCodeFasecolda").val(null);
            Fasecolda.GetVersionsByMakeIdModelId($("#selectBrandVehicle").UifSelect("getSelected"), $("#selectModelVehicle").UifSelect("getSelected"), selectedItem.Id);
            VehicleFasecolda.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId($("#selectBrandVehicle").UifSelect("getSelected"), $("#selectModelVehicle").UifSelect("getSelected"), selectedItem.Id, 0, 0);
        }
        else {

        }

    }

    static LoadValues() {
        if (windowSearchAdv) {
            marcasList[0].makeVehicle.Id = $("#selectBrandVehicleAdvSearch").UifSelect('getSelected');
            marcasList[0].modelVehicle.Id = $("#selectModelVehicleAdvSearch").UifSelect('getSelected');
            marcasList[0].versionVehicle.Id = $("#selectVersionVehicleAdvSearch").UifSelect('getSelected');
            marcasList[0].makeVehicle.Description = $("#selectBrandVehicleAdvSearch").UifSelect('getSelectedText');
            marcasList[0].modelVehicle.Description = $("#selectModelVehicleAdvSearch").UifSelect('getSelectedText');
            marcasList[0].versionVehicle.Description = $("#selectVersionVehicleAdvSearch").UifSelect('getSelectedText');
            marcasList[0].ModelVehicleCode = $("#inputMakeCodeFasecoldaAdvSearch").val();
            marcasList[0].MakeVehicleCode = $("#inputModeloCodeFasecoldaAdvSearch").val();
        }
        else {
            marcasList[0].makeVehicle.Id = $("#selectBrandVehicle").UifSelect('getSelected');
            marcasList[0].modelVehicle.Id = $("#selectModelVehicle").UifSelect('getSelected');
            marcasList[0].versionVehicle.Id = $("#selectVersionVehicle").UifSelect('getSelected');
            marcasList[0].makeVehicle.Description = $("#selectBrandVehicle").UifSelect('getSelectedText');
            marcasList[0].modelVehicle.Description = $("#selectModelVehicle").UifSelect('getSelectedText');
            marcasList[0].versionVehicle.Description = $("#selectVersionVehicle").UifSelect('getSelectedText');
        }
    }

    static MapValues() {
        if (windowSearchAdv) {

        }
        else {
            var version = $("#selectVersionVehicle").UifSelect('getSelectedSource');
            if (version !== null && version !== undefined) {
                $("#InputBrandCodeFasecolda").val(marcasList[0].MakeVehicleCode.replace(/ /g, ""));
                $("#InputVersionCodeFasecolda").val(marcasList[0].ModelVehicleCode.replace(/ /g, ""));
                $('#InputBrandCodeFasecolda').prop("disabled", false);
                $('#InputVersionCodeFasecolda').prop("disabled", false);
            }
        }
    }

    static GetVersionsByMakeIdModelId(makeId, modelId, selectedId) {
        VehicleFasecolda.GetVersionsByMakeIdModelId(makeId, modelId).done(function (data) {
            if (data.success) {
                glbVersions = data.result;
                if (selectedId == 0) {
                    if (windowSearchAdv) {
                        $("#selectVersionVehicleAdvSearch").UifSelect({ sourceData: data.result });
                        $("#selectVersionVehicle").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectVersionVehicle").UifSelect({ sourceData: data.result });

                    }

                }
                else {
                    if (windowSearchAdv) {
                        $("#selectVersionVehicleAdvSearch").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        $("#selectVersionVehicle").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectVersionVehicle").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    FasecoldaEdit(event, data, index) {
        fasecoldaIndex = index;
        Fasecolda.EditFasecolda(data);
    }

    static EditFasecolda(fasecolda) {

        $("#selectBrandVehicle").UifSelect('setSelected', fasecolda.makeVehicle.Id);
        $("#selectModelVehicle").UifSelect('setSelected', fasecolda.modelVehicle.Id);
        $("#selectVersionVehicle").UifSelect('setSelected', fasecolda.versionVehicle.Id);
        $("#InputBrandCodeFasecolda").val(fasecolda.MakeVehicleCode);
        $("#InputVersionCodeFasecolda").val(fasecolda.ModelVehicleCode);
        $('#InputBrandCodeFasecolda').prop("disabled", false);
        $('#InputVersionCodeFasecolda').prop("disabled", false);

        fasecoldaCodes.State = 3;
    }

    AddItemFasecolda() {
        Fasecolda.GetValuesOfControls();
        if (fascoldaIndex == null) {
            var list = $("#listViewFasecolda").UifListView('getData');
            var ifExist = list.filter(function (item) {
                return item.Format.toLowerCase()
                    == marcaId;
            });
            if (ifExist.length > 0) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': Resources.Language.ErrorExistAlliancePrintFormatName, 'autoclose': true
                });
            }
            else {
                lAlliancePrintFormat.Status = 'Added';
                AlliancePrintFormatControls.listAlliancePrintFormat.UifListView("addItem", lAlliancePrintFormat);
            }
        }
        else {
            if (lAlliancePrintFormat.Id != 0) {
                lAlliancePrintFormat.Status = 'Modified';
            } else {
                lAlliancePrintFormat.Status = 'Added';
            }

            AlliancePrintFormatControls.listAlliancePrintFormat.UifListView('editItem', alliancePrintFormatIndex, lAlliancePrintFormat);
        }
        ParametrizationAlliancePrintFormat.CleanForm();
    }

    AcceptFasecolda() {
        $("#formFasecolda").validate();
        if ($("#formFasecolda").valid()) {
            Fasecolda.Initialize();
            Fasecolda.ConstructFasecolda();
            //Search item to add in currente filter list
            var marca = $("#selectBrandVehicle").UifSelect('getSelectedSource');
            var modelo = $("#selectModelVehicle").UifSelect('getSelectedSource');
            var version = $("#selectVersionVehicle").UifSelect('getSelectedSource');

            var lista = $("#listViewFasecolda").UifListView('getData').filter(x => x.makeVehicle.Id == marca.Id && x.modelVehicle.Id == modelo.Id && x.versionVehicle.Id == version.Id);
            if (lista.length <= 0) {
                //New fasecolda in list
                ObjFasecolda.State = ParametrizationStatus.Create;
                $("#listViewFasecolda").UifListView("addItem", ObjFasecolda);
            }
            else {
                //Edit fasecolda in list
                ObjFasecolda.State = ParametrizationStatus.Update;
                var index = $("#listViewFasecolda").UifListView("findIndex", function (element, index, array) {
                    return (element._id == lista[0]._id);
                });
                $("#listViewFasecolda").UifListView('editItem', index, ObjFasecolda, );
            }

        }



        //if (fasecoldaIndex == null) {
        //    var ifExist = lista.filter(function (item) {
        //        return item.MakeVehicleCode.toUpperCase() == ObjFasecolda.MakeVehicleCode;
        //    });
        //    if (ifExist.length > 0) {
        //        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExitsVehicleTypeName, 'autoclose': true });
        //    }
        //    else {
        //        ObjFasecolda.State = 2;
        //        $("#listViewFasecolda").UifListView("addItem", ObjFasecolda);
        //    }
        //}
        //else {
        //    $("#listViewFasecolda").UifListView('editItem', fasecoldaIndex, ObjFasecolda);
        //}
        $("#selectBrandVehicle").prop("disabled", false);
        $("#selectModelVehicle").prop("disabled", false);
        $("#selectVersionVehicle").prop("disabled", false);
        Fasecolda.Clear();
    }

    static ConstructFasecolda() {
        if (windowSearchAdv) {
            ObjFasecolda.makeVehicle.Id = marcasList[0].makeVehicle.Id;
            ObjFasecolda.modelVehicle.Id = marcasList[0].modelVehicle.Id;
            ObjFasecolda.versionVehicle.Id = marcasList[0].versionVehicle.Id;
            ObjFasecolda.makeVehicle.Description = marcasList[0].makeVehicle.Description;
            ObjFasecolda.modelVehicle.Description = marcasList[0].modelVehicle.Description;
            ObjFasecolda.versionVehicle.Description = marcasList[0].versionVehicle.Description;
            ObjFasecolda.MakeVehicleCode = marcasList[0].MakeVehicleCode;
            ObjFasecolda.ModelVehicleCode = marcasList[0].ModelVehicleCode;

        }
        else {
            ObjFasecolda.makeVehicle.Id = $("#selectBrandVehicle").UifSelect('getSelected');
            ObjFasecolda.modelVehicle.Id = $("#selectModelVehicle").UifSelect('getSelected');
            ObjFasecolda.versionVehicle.Id = $("#selectVersionVehicle").UifSelect('getSelected');
            ObjFasecolda.MakeVehicleCode = $("#InputBrandCodeFasecolda").val();
            ObjFasecolda.ModelVehicleCode = $("#InputVersionCodeFasecolda").val();
            ObjFasecolda.makeVehicle.Description = $("#selectBrandVehicle").UifSelect('getSelectedText');
            ObjFasecolda.modelVehicle.Description = $("#selectModelVehicle").UifSelect('getSelectedText');
            ObjFasecolda.versionVehicle.Description = $("#selectVersionVehicle").UifSelect('getSelectedText');

            if (ObjFasecolda.MakeVehicleCode > 0) {
                ObjFasecolda.State = 3;
            }
            else {
                ObjFasecolda.State = 2;
            }
        }
    }

    static AddFasecolda() {
        var ExistAgent = false;
        var IndexFasecolda;
        gblFasecoldCodesList = $("#listViewFasecolda").UifListView("getData");
        $.each(gblFasecoldCodesList, function (index, value) {
            if (value.makeVehicle.Id == marcaId) {
                ExistAgent = true;
                IndexFasecolda = index;
                return false;
            }
        });
        if (!ExistAgent) {

            $("#listViewFasecolda").UifListView("addItem", Fasecolda.CreateFasecolda());
        }
    }

    static GetValuesOfControls() {
        marcaId = $("#selectBrandVehicle").UifSelect('getSelected');
        modeloId = $("#selectModelVehicle").UifSelect('getSelected');
        versionId = $("#selectVersionVehicle").UifSelect('getSelected');
        makeCodeVehicle = $("#InputBrandCodeFasecolda").val();
        modelCodeVehicle = $("#InputVersionCodeFasecolda").val();
        marcaName = $("#selectBrandVehicle").UifSelect('getSelectedText');
        modelName = $("#selectModelVehicle").UifSelect('getSelectedText');
        versionName = $("#selectVersionVehicle").UifSelect('getSelectedText');
    }

    ClearValues() {
        marcaId = 0;
        modeloId = 0;
        versionId = 0;
    }

    static CreateFasecolda() {
        Fasecolda.Initialize();
        fasecoldaCodes.makeVehicle.Id = marcaId;
        fasecoldaCodes.modelVehicle.Id = modeloId;
        fasecoldaCodes.versionVehicle.Id = versionId;
        fasecoldaCodes.makeVehicle.Description = marcaName;
        fasecoldaCodes.modelVehicle.Description = modelName;
        fasecoldaCodes.versionVehicle.Description = versionName;
        fasecoldaCodes.MakeVehicleCode = makeCodeVehicle;
        fasecoldaCodes.ModelVehicleCode = modelCodeVehicle;
        fasecoldaCodes.State = 2;
        return fasecoldaCodes;
    }

    static ShowData(event, result, index) {
        Fasecolda.Clear();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        if (result.MakeVehicleCode != undefined) {
            fasecoldaIndex = index;
            $("#selectBrandVehicle").UifSelect("setSelected", result.makeVehicle.Id);
            $("#selectModelVehicle").UifSelect("setSelected", result.modelVehicle.Id);
            $("#selectVersionVehicle").UifSelect("setSelected", result.versionVehicle.Id);
            $("#selectBrandVehicle").prop("disabled", true);
            $("#selectModelVehicle").prop("disabled", true);
            $("#selectVersionVehicle").prop("disabled", true);
            $("#InputBrandCodeFasecolda").val(result.MakeVehicleCode.replace(/ /g, ""));
            $("#InputVersionCodeFasecolda").val(result.ModelVehicleCode.replace(/ /g, ""));
            $("#InputBrandCodeFasecolda").prop("disabled", false);
            $("#InputVersionCodeFasecolda").prop("disabled", false);
        }
    }

    static ShowInformationToSearchAdv(result) {

        $("#selectBrandVehicle").UifSelect("setSelected", result.makeVehicle.Id);
        $("#selectModelVehicle").UifSelect("setSelected", result.modelVehicle.Id);
        $("#selectVersionVehicle").UifSelect("setSelected", result.versionVehicle.Id);
        $("#InputBrandCodeFasecolda").val(result.MakeVehicleCode.replace(/ /g, ""));
        $("#InputVersionCodeFasecolda").val(result.ModelVehicleCode.replace(/ /g, ""));
        $("#InputBrandCodeFasecolda").prop("disabled", true);
        $("#InputVersionCodeFasecolda").prop("disabled", true);
    }

    static Clear() {
        $("#selectBrandVehicle").UifSelect("disabled", false)
        $("#selectModelVehicle").UifSelect("disabled", true)
        $("#selectVersionVehicle").UifSelect("disabled", true)

        $("#selectBrandVehicle").UifSelect("setSelected", null);
        $("#selectModelVehicle").UifSelect("setSelected", null);
        $("#selectVersionVehicle").UifSelect("setSelected", null);

        $("#InputBrandCodeFasecolda").val(null);
        $("#InputVersionCodeFasecolda").val(null);

        $('#InputBrandCodeFasecolda').prop("disabled", true);
        $('#InputVersionCodeFasecolda').prop("disabled", true);
        ObjFasecolda = {};
        fasecoldaIndex = null;

    }

    static Initialize() {

        ObjFasecolda =
            {
                MakeVehicleCode: null,
                ModelVehicleCode: null
            }
        ObjFasecolda.makeVehicle =
            {
                Id: null,
                Description: null
            }

        ObjFasecolda.modelVehicle =
            {
                Id: null,
                Description: null
            }
        ObjFasecolda.versionVehicle =
            {
                Id: null,
                Description: null
            }
    }

    static ClearControls() {
        $("#InputBrandCodeFasecolda").val('');
        $("#InputVersionCodeFasecolda").val('');
    }

    Save() {
        var gblFasecoldCodesList = $("#listViewFasecolda").UifListView('getData');

        if (gblFasecoldCodesList.length > 0) {
            Fasecolda.SaveFasecolda(gblFasecoldCodesList)
                .done(function (data) {
                    if (data.success) {
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

                        var messageErrorCreated = Fasecolda.ConstructMessageArray(Resources.Language.ErrorCreated, data.result.errorCreated);
                        var messageErrorUpdated = Fasecolda.ConstructMessageArray(Resources.Language.ErrorUpdated, data.result.errorUpdated);
                        var messageErrorDeleted = Fasecolda.ConstructMessageArray(Resources.Language.ErrorDeleted, data.result.errorDeleted);
                        if (messageErrorCreated != null) {
                            $.UifNotify('show', { 'type': 'danger', 'message': messageErrorCreated, 'autoclose': true });
                        }
                        if (messageErrorUpdated != null) {
                            $.UifNotify('show', { 'type': 'danger', 'message': messageErrorUpdated, 'autoclose': true });
                        }
                        if (messageErrorDeleted != null) {
                            $.UifNotify('show', { 'type': 'danger', 'message': messageErrorDeleted, 'autoclose': true });
                        }

                        $("#listViewFasecolda").UifListView({
                            source: null,
                            add: false,
                            edit: true,
                            customEdit: true,
                            delete: true,
                            deleteCallback: Fasecolda.deleteFasecoldaCallback,
                            displayTemplate: "#FasecoldaTemplate",
                            selectionType: 'single',
                            height: 310
                        });

                        $("#selectBrandVehicle").prop("disabled", false);
                        $("#selectModelVehicle").prop("disabled", false);
                        $("#selectVersionVehicle").prop("disabled", false);

                        $('#inputFasecoldaCode').val(null);
                    }
                    else {
                        $('#inputFasecoldaCode').val(null);
                        $.UifNotify('show', {
                            'type': 'danger', 'message': data.result, 'autoclose': true
                        });
                    }
                })
                .fail(function () {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorSavedFasecolda, 'autoclose': true
                    })

                });
        }
        //}
    }

    SaveVehicleTypes() {
        var vehicleTypes = $("#listVehicleType").UifListView('getData');
        glbVehicleTypeDelete.forEach(function (item) {
            vehicleTypes.push(item);
        });
        VehicleType.Save(vehicleTypes)
            .done(function (data) {
                if (data.success) {
                    Fasecolda.Clear();
                    glbVehicleTypeDelete = [];
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

                    var messageErrorCreated = Fasecolda.ConstructMessageArray(Resources.Language.ErrorCreated, data.result.errorCreated);
                    var messageErrorUpdated = Fasecolda.ConstructMessageArray(Resources.Language.ErrorUpdated, data.result.errorUpdated);
                    var messageErrorDeleted = Fasecolda.ConstructMessageArray(Resources.Language.ErrorDeleted, data.result.errorDeleted);
                    if (messageErrorCreated != null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': messageErrorCreated, 'autoclose': true });
                    }
                    if (messageErrorUpdated != null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': messageErrorUpdated, 'autoclose': true });
                    }
                    if (messageErrorDeleted != null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': messageErrorDeleted, 'autoclose': true });
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveVehicleTypes, 'autoclose': true })
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

    static SaveFasecolda(gblFasecoldCodesListAddedValues) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Vehicle/SaveFasecolda',
            data: JSON.stringify({ fasecoldaViewModel: gblFasecoldCodesListAddedValues }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }


    static DeleteItemFasecolda(deferred, data) {
        deferred.resolve();
        var fasecoldaList = $("#listViewFasecolda").UifListView('getData');
        //$("#listViewFasecolda").UifListView({ sourceData: glbVersionVehicleFascolda, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
        //$("#listViewFasecolda").UifListView({ source: null, displayTemplate: "#FasecoldaTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(fasecoldaList, function (index, value) {
            if (data.State !== 2) {
                if (value.MakeVehicleCode == data.MakeVehicleCode && value.ModelVehicleCode == data.ModelVehicleCode) {
                    data.State = ParametrizationStatus.Delete;
                    data.allowEdit = false;
                    data.allowDelete = false;
                    $("#listViewFasecolda").UifListView('addItem', data);
                }
            }
        });
        Fasecolda.Clear();
    }

    ///esta funcion es para adecuar la del listvieww
    static deleteFasecoldaCallback(deferred, data) {
        Fasecolda.DeleteItemFasecolda(deferred, data)
        //deferred.resolve();
    }


    SearchAdvFasecolda() {
        evaluate = true;
        Fasecolda.ShowSearchAdv();
    }

    static ShowSearchAdv(data) {
        $("#listviewSearchADvFasecolda").UifListView({ displayTemplate: '#searchTemplateFasecolda', selectionType: "single", height: 400 });
        $("#listviewSearchADvFasecolda").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#listviewSearchADvFasecolda").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvFasecolda.show();
        $("#btnAdvancedSearchFasecolda").off("click");
        $("#btnCancelVehicleTypeAdv").off("click");
        $("#btnAdvancedSearchFasecolda").on("click", this.OkSearchVehicleTypeAdv);
        $("#btnCancelVehicleTypeAdv").on("click", this.CancelSearchAdv)
    }

    static OkSearchVehicleTypeAdv() {

        let data = $("#listviewSearchADvFasecolda").UifListView("getSelected");
        if (data.length == 1) {
            Fasecolda.ShowData(null, data, data.key);
        }
        Fasecolda.HideSearchAdv();
    }

    SearchFasecolda(event, selectedItem) {

        inputSearch = selectedItem;
        if (inputSearch.length < 4) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true })
        }
        else if (inputSearch.length > 9) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageInfoMaximumEightChar, 'autoclose': true })
        }
        else {
            $("#listViewFasecolda").UifListView({
                source: null,
                add: false,
                edit: true,
                customEdit: true,
                delete: true,
                deleteCallback: Fasecolda.deleteFasecoldaCallback,
                displayTemplate: "#FasecoldaTemplate",
                selectionType: 'single',
                height: 310
            });
            $('#InputBrandCodeFasecolda').val(null);
            $('#InputVersionCodeFasecolda').val(null);

            Search = true;
            //ParametrizationVehicleType.Clear();
            VehicleFasecoldaRequest.GetVersionVehicleFasecoldaByFasecoldaId(inputSearch.replace(/ /g, "")).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        VehicleFasecolda.GetMakes().done(function (dataMakes) {
                            if (dataMakes.success) {
                                glbMakes = dataMakes.result;
                                $("#selectBrandVehicle").UifSelect({ sourceData: dataMakes.result, selectedId: data.result[0].makeVehicle.Id });
                                VehicleFasecolda.GetModelsByMakeId(data.result[0].makeVehicle.Id).done(function (dataModels) {
                                    if (dataModels.success) {
                                        glbModels = dataModels.result;
                                        $("#selectModelVehicle").UifSelect({ sourceData: dataModels.result, selectedId: data.result[0].modelVehicle.Id });
                                        VehicleFasecolda.GetVersionsByMakeIdModelId(data.result[0].makeVehicle.Id, data.result[0].modelVehicle.Id).done(function (dataVersion) {
                                            if (dataVersion.success) {
                                                glbVersions = dataVersion.result;
                                                $("#selectVersionVehicle").UifSelect({ sourceData: dataVersion.result, selectedId: data.result[0].versionVehicle.Id });

                                                $.each(data.result, function (key, vehicleFasecolda) {
                                                    $.each(glbMakes, function (key, make) {
                                                        if (make.Id == vehicleFasecolda.makeVehicle.Id) {
                                                            vehicleFasecolda.makeVehicle.Description = make.Description;
                                                        }
                                                    });

                                                    $.each(glbModels, function (key, model) {
                                                        if (model.Id == vehicleFasecolda.modelVehicle.Id) {
                                                            vehicleFasecolda.modelVehicle.Description = model.Description;
                                                        }
                                                    });
                                                    $.each(glbVersions, function (key, version) {
                                                        if (version.Id == vehicleFasecolda.versionVehicle.Id) {
                                                            vehicleFasecolda.versionVehicle.Description = version.Description;
                                                        }
                                                    });
                                                });
                                                marcasList = data.result;
                                                $("#InputBrandCodeFasecolda").val(data.result[0].MakeVehicleCode);
                                                $("#InputVersionCodeFasecolda").val(data.result[0].ModelVehicleCode);
                                                $('#InputBrandCodeFasecolda').prop("disabled", false);
                                                $('#InputVersionCodeFasecolda').prop("disabled", false);
                                                $("#selectBrandVehicle").prop("disabled", true);
                                                $("#selectModelVehicle").prop("disabled", true);
                                                $("#selectVersionVehicle").prop("disabled", true);
                                                $('#inputFasecoldaCode').val(null);

                                                $("#listViewFasecolda").UifListView({ sourceData: marcasList, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                            }
                                        });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': dataModels.result, 'autoclose': true });
                                    }
                                });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': dataMakes.result, 'autoclose': true });
                            }
                        });
                    } else {
                        Fasecolda.Clear();
                        $('#inputFasecoldaCode').val(null);
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchVehicleFasecolda, 'autoclose': true })
                    }
                }
            });


        }

    }


    static Get(id, inputSearch) {
        var find = false;
        var data = [];
        var search = $("#listViewFasecolda").UifListView('getData');
        if (id == 0) {

            var valores = glbVersionVehicleFascolda;
        }
        else {
            var valores = glbVersionVehicleFascolda;

        }

    }

}
