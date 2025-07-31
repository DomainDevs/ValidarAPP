var moduleIndex = null;
var inputSearch = "";
var module = {};
var glbSubModule = {};
var modules = null;
var dropDownSearchAdvSubModule = null;
$.ajaxSetup({ async: true });

class SubModules {
    static GetSubModules() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/SubModule/GetListSubModules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetSubModulesByModuleId(moduleId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/SubModule/GetSubModulesByModuleId?moduleId=' + moduleId,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/SubModule/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class SubModule extends Uif2.Page {
    getInitialState() {
        $("#listModule").UifListView({
            displayTemplate: "#SubModuleTemplate",
            source: null,
            selectionType: 'single',
            height: 300
        });
        // $("#Description").ValidatorKey(ValidatorType.Letter, 4, 0);
        Modules.GetEnabledModules().done(function (data) {
            if (data.success) {
                $('#selectModule').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        SubModules.GetSubModules().done(function (data) {
            if (data.success) {
                $.each(data.result, function (key, value) {
                    value.StatusType = 1;
                });
                glbSubModule = data.result;
                SubModule.loadModules();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        dropDownSearchAdvSubModule = uif2.dropDown({
            source: rootPath + 'UniqueUser/SubModule/SubModuleAdvancedSearch',
            element: '#inputSearch',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: SubModule.componentLoadedCallbackSub
        });
        $("#lvSearchAdvSubModule").UifListView({
            displayTemplate: "#SubModuleTemplate",
            selectionType: "single",
            height: 400
        });
    }
    bindEvents() {
        $('#btnModuleAccept').on('click', this.addItemModule);
        $('#btnNewModule').on('click', SubModule.clearPanel);
        $('#btnSaveModule').on('click', this.saveModules);
        $('#listModule').on('rowEdit', SubModule.showData);
        $('#listModule').on('rowDelete', this.deleteItemModule);
        $('#btnExit').click(this.Exit);
        $('#btnSendExcel').on('click', this.sendExcelModule);
        $('#inputSearch').on('search', this.searchSubModules);
        $('#btnSearchAdvSubModule').on('click', this.searchAllSubModules);
        GetAccessButtonsByPath(window.location.pathname + window.location.search);
        $('#selectModule').on('itemSelected', SubModule.filterListView);
        $("#Description").off();
        $("#Description").ValidatorKey(ValidatorType.lettersandnumbersAccent, 3, 0);
    }

    SearchAdvSubModule() {
        SubModule.ShowSearchAdv();
    }

    CancelSearchAdv() {
        SubModule.HideSearchAdv();
    }
    static componentLoadedCallbackSub() {
        $("#btnOkSubSearchAdv").on("click", SubModule.OkSearchAdv);
        $("#btnCancelSubModuleAdv").on('click', function () {
            SubModule.HideSearchAdv();
        })
    }
    static OkSearchAdv() {
        let data = $("#lvSearchAdvSubModule").UifListView("getSelected");
        if (data.length === 1) {
            SubModule.showData(null, data, data.key);
        }
        SubModule.HideSearchAdv();
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvSubModule").UifListView({
            displayTemplate: "#SubModuleTemplate",
            source: null,
            selectionType: "single",
            height: 400
        });
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvSubModule").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvSubModule.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvSubModule.hide();
    }

    SelectSearch() {
        SubModule.getModule($(this).children()[0].innerHTML, "");
        $('#modalDefaultSearch').UifModal("hide");
        SubModule.filterListView();
    }
    searchSubModules(event, selectedItem) {
        if (selectedItem.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            inputSearch = selectedItem;
            SubModule.clearPanel();
            $("#selectModule").UifSelect("setSelected", null);
            SubModule.getModule(0, inputSearch);
            SubModule.filterListView();
        }
    }
    searchAllSubModules(event, selectedItem) {
        moduleIndex == null;
        SubModule.clearPanel();
        SubModule.ShowSearchAdv(glbSubModule);
    }
    sendExcelModule() {
        SubModules.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static getModule(id, inputSearch) {
        var find = false;
        var data = [];
        var search = glbSubModule;
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
                if (value.Id == id) {
                    moduleIndex = key
                    value.key = key;
                    data.push(value);
                    find = true;
                    return false;
                }
            });
        }

        if (find === false) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SubModuleNotFound, 'autoclose': true })
        } else {
            if (data.length === 1) {
                SubModule.showData(null, data, data.key);
            } else {
                SubModule.ShowSearchAdv(data);
            }
        }

    }
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    static loadModules() {
        $("#listModule").UifListView({
            source: null,
            displayTemplate: "#SubModuleTemplate",
            selectionType: 'single',
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: SubModule.deleteCallbackList,
            height: 300
        });
        $.each(glbSubModule, function (key, value) {
            var status = "";
            var add = "";
            var update = "";
            if (this.Enabled) {
                status = Resources.Language.Enabled;
            }
            if (this.bandAdd != undefined) {

                add = this.bandAdd;
            }
            if (this.bandUpdate != undefined) {
                update = this.bandUpdate;
            }
            module =
                {
                    Description: this.Description,
                    Enabled: this.Enabled,
                    Id: this.Id,
                    Status: 'NoModified',
                    EnabledDescription: this.EnabledDescription,
                    ModuleId: this.ModuleId,
                    ModuleDescription: this.ModuleDescription,
                    bandAdd: add,
                    bandUpdate: update,
                    StatusType: this.StatusType
                }
            $("#listModule").UifListView("addItem", module);
        })
        SubModule.clearPanel();
        $("#selectModule").UifSelect("setSelected", null);
    }

    static deleteCallbackList(deferred, result) {
        if (result.Id !== "" && result.Id !== undefined && result.Id !== "0") //Se elimina unicamente si existe en DB
        {
			result.Status = 'Deleted';
			result.StatusTypeService = ParametrizationStatus.Delete; 
			result.allowEdit = false;
			result.allowDelete = false; 
			$("#listModule").UifListView("addItem", result); 
        }

        for (var i = 0; i < glbSubModule.length; i++) {
            if (glbSubModule[i].Id == result.Id && glbSubModule[i].ModuleId == result.ModuleId && glbSubModule[i].Description == result.Description) {
                glbSubModule.splice(i, 1);
                continue;
            }
        }
        deferred.resolve();
    }

    static showData(event, result, index) {
        SubModule.clearPanel();
        if (result.length == 1) {
            var search = $("#listModule").UifListView('getData');
            if (result[0].Id != 0) {
                $.each(search, function (key, value) {
                    if (value.Id == result[0].Id) {
                        index = key;
                        result = result[0];
                        return false;
                    }
                });
            }
        }
        if (result.Id != undefined) {
            moduleIndex = index;
            $("#Description").val(result.Description);
            $("#hiddenId").val(result.Id);
            $("#selectModule").UifSelect("setSelected", result.ModuleId);
            $("#selectModule").UifSelect("disabled", true);
            if (!result.Enabled) {
                $('#chkDisabled').prop("checked", true);
            }
            $("#inputSearch").val(result.Description);
            ClearValidation("#formSubModule");
        }
    }

    static clearPanel() {
        moduleIndex = null;
        $("#Description").val('');
        $("#hiddenId").val('');
        $("#selectModule").UifSelect("disabled", false);
        $("#inputSearch").val('');
        $('#chkDisabled').prop("checked", false);
        ClearValidation("#formSubModule");
    }

    static filterListView() {
        if (glbSubModule != null) {
            if ($("#selectModule").UifSelect("getSelected") != "") {
                $("#listModule").UifListView({
                    source: null,
                    displayTemplate: "#SubModuleTemplate",
                    selectionType: 'single',
                    edit: true,
                    delete: true,
                    customEdit: true,
                    deleteCallback: SubModule.deleteCallbackList,
                    height: 300
                });
                $.each(glbSubModule, function (index, value) {
                    if ((this.ModuleId == $("#selectModule").UifSelect("getSelected"))) {
                        $("#listModule").UifListView("addItem", this);
                    }
                });
            }
            else {
                SubModule.loadModules();
            }

        }
    }
    addItemModule() {
        $("#formSubModule").validate();
        if ($("#formSubModule").valid()) {
            var enabled = true;
            var enabledDescription = Resources.Language.LabelEnabled;

            if ($('#chkDisabled').is(':checked')) {
                enabled = false;
                enabledDescription = Resources.Language.Disabled;
            }
            module =
                {
                    Description: $("#Description").val(),
                    Enabled: enabled,
                    Id: $("#hiddenId").val(),
                    Status: 'Modified',
                    EnabledDescription: enabledDescription,
                    ModuleId: $("#selectModule").UifSelect("getSelected"),
                    ModuleDescription: $("#selectModule").UifSelect("getSelectedText")
                }
            if (moduleIndex == null) {
                var lista = $("#listModule").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == module.Description.toUpperCase() && item.ModuleId == module.ModuleId;
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistSubmoduleName, 'autoclose': true });
                }
                else {
                    glbSubModule.push(module);
                    module.StatusTypeService = ParametrizationStatus.Create;
                    $("#listModule").UifListView("addItem", module);
                    $("#selectModule").UifSelect("setSelected", null);
                }
            }
            else {
                for (var i = 0; i < glbSubModule.length; i++) {
                    if (glbSubModule[i].Id == module.Id && glbSubModule[i].ModuleId == module.ModuleId) {
                        glbSubModule.splice(i, 1);
                        break;
                    }
                }
                module.StatusTypeService = ParametrizationStatus.Update;                
                glbSubModule.push(module);
                $('#listModule').UifListView('editItem', moduleIndex, module);
                $("#selectModule").UifSelect("setSelected", null);
            }
            SubModule.clearPanel();
        }
    }
	saveModules() {
		var modules = $("#listModule").UifListView('getData');
        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/SubModule/SaveSubModule',
            data: JSON.stringify({ modulesModelView: modules }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            var dataNum = data;
            if (dataNum.result.Message == null && dataNum.result.TotalAdded == 0 && dataNum.result.TotalModified == 0 && dataNum.result.TotalDeleted == 0) {
               
            }
            else if  (data.success) {
                SubModules.GetSubModules().done(function (data) {
                    if (data.success) {
                        glbSubModule = data.result;
                        SubModule.loadModules();
                        $.UifNotify('show', {
                            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                                "Agregados" + ':' + dataNum.result.TotalAdded + '<br> ' +
                                "Modificados" + ':' + dataNum.result.TotalModified + '<br> ' +
                                "Eliminados" + ':' + dataNum.result.TotalDeleted + '<br> ' +
                                "Errores" + ':' + dataNum.result.Message,
                            'autoclose': true
                        });
                       // $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SubModulesSaveSuccessfully, 'autoclose': true });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveModule, 'autoclose': true })
        });
    }
}
