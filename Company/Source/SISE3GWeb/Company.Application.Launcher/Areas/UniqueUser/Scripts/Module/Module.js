var moduleIndex = null;
var module = {};
var inputSearch = "";
var glbModule = {};
var glbModuleDelete = [];
var dropDownSearchAdvModule = null;
$.ajaxSetup({ async: true });

class Modules {
    static GetModules() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Module/GetModules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetEnabledModules() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Module/GetEnabledModules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Module/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class Module extends Uif2.Page {


    getInitialState() {
        $("#listModule").UifListView({
            displayTemplate: "#ModuleTemplate",
            selectionType: 'single',
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: Module.deleteCallbackList,
            height: 300
        });
        Modules.GetModules().done(function (data) {
            if (data.success) {
                glbModule = data.result;
                Module.loadModules();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        dropDownSearchAdvModule = uif2.dropDown({
            source: rootPath + 'UniqueUser/Module/ModuleAdvancedSearch',
            element: '#inputSearch',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: Module.componentLoadedCallback            
        });
        $("#lvSearchAdvModule").UifListView({
            displayTemplate: "#ModuleTemplate",
            source: null,
            selectionType: "single",
            height: 400
        });
    }

	static deleteCallbackList(deferred, result) {
        if (result.Id !== "" && result.Id !== undefined && result.Id !== "0") //Se elimina unicamente si existe en DB
		{
			result.Status = 'Deleted';
            		result.StatusTypeService = ParametrizationStatus.Delete;
            		result.StatusType = ParametrizationStatus.Delete;
			result.allowEdit = false;
			result.allowDelete = false;
			$("#listModule").UifListView("addItem", result); 
		}
		deferred.resolve();
    }
    bindEvents() {
        $('#btnModuleAccept').on('click', this.addItemModule);
        $('#btnNewModule').on('click', Module.clearPanel);
        $('#btnSaveModule').on('click', this.saveModules);
        $('#listModule').on('rowEdit', Module.showData);
        $('#listModule').on('rowDelete', this.deleteItemModule);
        $('#btnSendExcel').on('click', this.sendExcelModule);
        $('#btnExit').click(this.Exit);
        $('#inputSearch').on('search', this.searchModules);
        $('#btnSearchAdvModule').on('click', this.searchAllModules);
        GetAccessButtonsByPath(window.location.pathname + window.location.search);
        $("#Description").off();
        $("#Description").ValidatorKey(ValidatorType.lettersandnumbersAccent, 3, 0);
    }

    SearchAdvModule() {
        Module.ShowSearchAdv();
    }

    CancelSearchAdv() {
        Module.HideSearchAdv();
    }

    static componentLoadedCallback() {
        $("#btnOkModuleSearchAdv").on("click", Module.OkSearchAdv);
        $("#btnCancelModuleAdv").on('click', function () {
            Module.HideSearchAdv();
        })
    }
    static OkSearchAdv() {
        let data = $("#lvSearchAdvModule").UifListView("getSelected");
        if (data.length === 1) {
            Module.showData(null, data, data.key);
        }
        Module.HideSearchAdv();
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvModule").UifListView({
            displayTemplate: "#ModuleTemplate",
            source: null,
            selectionType: "single",
            height: 400
        });
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvModule").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvModule.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvModule.hide();
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }
    static showData(event, result, index) {
        Module.clearPanel();
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
            if (!result.Enabled) {
                $('#chkDisabled').prop("checked", true);
            }
            if (result.ExpirationDate != null) {
                $("#hiddenExpirationDate").val(FormatDate(result.ExpirationDate));
            }
            if (result.VirtualFolder != null) {
                $("#hiddenVirtualFolder").val(result.VirtualFolder);
            }
            ClearValidation("#formModule");
        }
    }


    SelectSearch() {
        Module.getModule($(this).children()[0].innerHTML, "");
        $('#modalDefaultSearch').UifModal("hide");
    }
    static getModule(id, inputSearch) {
        var find = false;
        var data = [];
        var search = $("#listModule").UifListView('getData');
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
                    moduleIndex = key;
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });
        }
        if (find === false) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ModuleNotFound, 'autoclose': true })
        } else {
            if (data.length === 1) {
                Module.showData(null, data, data.key);
            } else {
                Module.ShowSearchAdv(data);
            }
        }

    }
    searchModules(event, selectedItem) {
        if (selectedItem.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            inputSearch = selectedItem;
            Module.clearPanel();
            Module.getModule(0, inputSearch);
        }
    }
    searchAllModules(event, selectedItem) {
        Module.clearPanel();
        var data = $("#listModule").UifListView('getData');
        Module.ShowSearchAdv(data);
    }
    static loadModules() {
        $("#listModule").UifListView({
            source: null,
            displayTemplate: "#ModuleTemplate",
            selectionType: 'single',
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: Module.deleteCallbackList,
            height: 300
        });
        $.each(glbModule, function (key, value) {
            module =
                {
                    Description: this.Description,
                    Enabled: this.Enabled,
                    ExpirationDate: FormatDate(this.ExpirationDate),
                    Id: this.Id,
                    VirtualFolder: this.VirtualFolder,
                    Status: 'NoModified',
                    EnabledDescription: this.EnabledDescription,
                    StatusType: 1
                }
            $("#listModule").UifListView("addItem", module);
        })
    }
    static clearPanel() {
        moduleIndex = null;
        $("#Description").val('');
        $("#Description").focus();
        $("#hiddenId").val('');
        $("#hiddenExpirationDate").val('');
        $("#inputSearch").val('');
        $("#hiddenVirtualFolder").val('');
        $('#chkDisabled').prop("checked", false);
        ClearValidation("#formModule");
    }

    deleteItemModule(event, data, index) {
        var submodules = [];
        if (data.Id != "") {
            SubModules.GetSubModulesByModuleId(data.Id).done(function (result) {
                submodules = result.data;
            });

        }
        if (submodules.length > 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MustDeleteSubModulesFirst, 'autoclose': true });
        }
        else {
            var modules = $("#listModule").UifListView('getData');
            $("#listModule").UifListView({
                source: null,
                displayTemplate: "#ModuleTemplate",
                selectionType: 'single',
                edit: true,
                delete: true,
                customEdit: true,
                deleteCallback: Module.deleteCallbackList,
                height: 300
            });

            $.each(modules, function (index, value) {
                if (this.Id == data.Id && this.Description == data.Description) {
                    value.Status = 'Deleted';
                    glbModuleDelete.push(value);
                }
                else {
                    $("#listModule").UifListView("addItem", this);
                }
            });
            Module.clearPanel();
        }
    }

    addItemModule() {
        $("#formModule").validate();
        if ($("#formModule").valid()) {
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
                    ExpirationDate: $("#hiddenExpirationDate").val(),
                    Id: $("#hiddenId").val(),
                    VirtualFolder: $("#hiddenVirtualFolder").val(),
                    Status: 'Modified',
                    EnabledDescription: enabledDescription
                }
            if (moduleIndex == null) {
                var lista = $("#listModule").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == module.Description.toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistModuleName, 'autoclose': true });
                }
                else {
                    module.StatusTypeService = ParametrizationStatus.Create;
                    $("#listModule").UifListView("addItem", module);
                }

            }
            else {
                module.StatusTypeService = ParametrizationStatus.Update;
                $('#listModule').UifListView('editItem', moduleIndex, module);
            }
         
            Module.clearPanel();

        }
    }
    sendExcelModule() {
        Modules.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    saveModules() {
        var itemModified = [];
        var modules = $("#listModule").UifListView('getData');
        $.each(modules, function (index, value) {
            if (value.Status != undefined && value.Status != "NoModified" && value.StatusType != ParametrizationStatus.Original) {
                itemModified.push(value);
            }
        });
        if (itemModified.length > 0) {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/Module/SaveModule',
                data: JSON.stringify({ modulesModelView: modules }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                var dataNum = data;
                if (data.success) {
                    $("#Description").val('');
                    $('#chkDisabled').attr('checked', false);
                    Modules.GetModules().done(function (data) {
                        if (data.success) {
                            glbModule = data.result;
                            Module.loadModules();
                            $.UifNotify('show', {
                                'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                                    "Agregados" + ':' + dataNum.result.TotalAdded + '<br> ' +
                                    "Modificados" + ':' + dataNum.result.TotalModified + '<br> ' +
                                    "Eliminados" + ':' + dataNum.result.TotalDeleted + '<br> ' +
                                    "Errores" + ':' + dataNum.result.Message,
                                'autoclose': true
                            });
                            //$.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ModulesSaveSuccessfully, 'autoclose': true });
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
}


