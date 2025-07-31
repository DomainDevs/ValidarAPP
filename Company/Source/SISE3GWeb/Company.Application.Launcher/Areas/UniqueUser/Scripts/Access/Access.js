var accessIndex = null;
var inputSearch = "";
var accesses = {};
var accessDelete = [];
$.ajaxSetup({ async: true });
var IsNew = false;

class AccessType {
    static GetAccessType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Profile/GetAccessType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}
class AccessObject {
    static GetAccessObject() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Access/GetListAccessObject',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ moduleId: 0, subModuleId: 0 })
        });
    }
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Access/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetSubModule(moduleId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/SubModule/GetEnabledSubModulesByModuleId?moduleId=' + moduleId,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetParentAccess(moduleAccess, subModuleAccess) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Access/GetListAccessObject?moduleId=' + moduleAccess + "&subModuleId=" + subModuleAccess,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }
}
class Access extends Uif2.Page {
    getInitialState() {
        $("#listAccess").UifListView({
            displayTemplate: "#AccessTemplate",
            selectionType: 'single',
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: Access.deleteCallbackList,
            height: 300
        });

        Modules.GetModules().done(function (data) {
            if (data.success) {
                $('#selectModuleAccess').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        AccessType.GetAccessType().done(function (data) {
            if (data.success) {
                $('#selectAccessType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        AccessObject.GetAccessObject().done(function (data) {
            if (data.success) {
                $.each(data.result, function (key, value) {
                    value.StatusType = ParametrizationStatus.Original;
                });
                accesses = data.result;
                Access.loadAccess();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }
    bindEvents() {
        
        $('#listAccess').on('rowEdit', Access.showData);
        $('#listAccess').on('rowDelete', this.deleteItemAccess);
        $('#inputSearch').on('buttonClick', this.searchAccess);
        $("#Path").off();
        //$('#btnNew').on('click', Access.clearPanel);
        $("#btnNew").click(Access.clearPanel);
        //$('#btnAdd').on('click', this.addItemAccess);
        $("#btnAdd").click(this.addItemAccess);
        $('#btnSaveAccess').on('click', this.saveAccess);
        $('#btnExit').click(this.Exit);
        $('#btnSendExcel').on('click', this.sendExcelModule);
        $('#selectModuleAccess').on('itemSelected', Access.getSubModuleByModule);
        $('#selectSubModuleAccess').on('itemSelected', Access.validateAccessType);
        $('#selectAccessType').on('itemSelected', Access.validateAccessType);
        $("#Description").off();
        $("#Description").ValidatorKey(ValidatorType.lettersandnumbersAccent, 3, 0);
    }
    static filterListView() {
        if (accesses != null) {
            if ($("#selectModuleAccess").UifSelect("getSelected") != "") {
                $("#listAccess").UifListView({
                    source: null,
                    displayTemplate: "#AccessTemplate",
                    selectionType: 'single',
                    edit: true,
                    delete: true,
                    customEdit: true,
                    deleteCallback: Access.deleteCallbackList,
                    height: 300
                });

                $.each(accesses, function (index, value) {
                    if (this.ModuleId == parseInt($("#selectModuleAccess").UifSelect("getSelected"))) {
                        if ($("#selectSubModuleAccess").UifSelect("getSelected") != "") {
                            if (this.SubModuleId == parseInt($("#selectSubModuleAccess").UifSelect("getSelected"))) {
                                if ($("#selectAccessType").UifSelect("getSelected") != "") {
                                    if (this.AccessTypeId == parseInt($("#selectAccessType").UifSelect("getSelected"))) {
                                        $("#listAccess").UifListView("addItem", this);
                                    }
                                }
                                else {
                                    $("#listAccess").UifListView("addItem", this);
                                }
                            }
                        }
                        else {
                            $("#listAccess").UifListView("addItem", this);
                        }
                    }
                });
            }
            else {
                Access.loadAccess();
            }

        }
    }
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    sendExcelModule() {
        AccessObject.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static getSubModuleByModule() {
        var moduleId = 0;
        if ($("#selectModuleAccess").val() != null && $("#selectModuleAccess").val() != "") {
            moduleId = $("#selectModuleAccess").val();
        }

        AccessObject.GetSubModule(moduleId).done(function (data) {
            if (data.success) {
                $("#selectSubModuleAccess").UifSelect({ sourceData: data.result, native: false, filter: true });
            }
        });

        Access.filterListView();
    }

    static getSubModule(submoduleId) {
        var moduleId = 0;
        if ($("#selectModuleAccess").val() != null && $("#selectModuleAccess").val() != "") {
            moduleId = $("#selectModuleAccess").val();
        }

        AccessObject.GetSubModule(moduleId).done(function (data) {
            if (data.success) {
                if (submoduleId != 0) {
                    $("#selectSubModuleAccess").UifSelect({ sourceData: data.result, selectedId: submoduleId, native: false, filter: true });
                }
                else {
                    $("#selectSubModuleAccess").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
            }
        });
    }


    static loadAccess() {
        $("#listAccess").UifListView({
            source: null,
            displayTemplate: "#AccessTemplate",
            selectionType: 'single',
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: Access.deleteCallbackList,
            height: 300
        });
        $.each(accesses, function (key, value) {
            var access =
                {
                    Id: this.Id,
                    AccessObjectId: this.AccessObjectId,
                    ModuleId: this.ModuleId,
                    ModuleDescription: this.ModuleDescription,
                    Path: this.Path,
                    SubModuleId: this.SubModuleId,
                    SubModuleDescription: this.SubModuleDescription,
                    Description: this.Description,
                    AccessTypeId: this.AccessTypeId,
                    AccessTypeDescription: this.AccessTypeDescription,
                    Enabled: this.Enabled,
                    EnabledDescription: this.EnabledDescription,
                    Status: 'NoModified',
                    ParentAccessId: this.ParentAccessId,
                    StatusType: this.StatusType
                }
            $("#listAccess").UifListView("addItem", access);
        })
        IsNew = true;
        Access.clearPanel();
        $("#selectModuleAccess").UifSelect("setSelected", null);
        $("#selectSubModuleAccess").UifSelect("setSelected", null);
        $("#selectAccessType").UifSelect("setSelected", null);
    }
    searchAccess(event, selectedItem) {
        if (selectedItem.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            inputSearch = selectedItem;
            IsNew = true;
            Access.clearPanel();
            $("#selectModuleAccess").UifSelect("setSelected", null);
            $("#selectSubModuleAccess").UifSelect("setSelected", null);
            $("#selectAccessType").UifSelect("setSelected", null);
            Access.getAccess(0, inputSearch);
        }
    }

    static getAccess(id, inputSearch) {
        var find = false;
        var data = [];
        if ($("#listAccess").UifListView('getData').length == 0) {
            Access.loadAccess();
        }
        var search = $("#listAccess").UifListView('getData');
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
        if (data.length == 1) {
            Access.showData(null, data[0], data[0].key);
        }
        else if (data.length > 1) {
            Access.showDataAdvanced(data);
        }

        if (find == false) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.AccessNotFound, 'autoclose': true })
        }
    }

    static showDataAdvanced(data) {
        AccessAdvSearch.LoadAdvanced(data);
        dropDownSearch.show();
    }

    static showData(event, data, index) {
        $("#selectModuleAccess").UifSelect("setSelected", data.ModuleId);
        Access.getSubModule(data.SubModuleId);
        accessIndex = index;
        $("#hiddenId").val(data.Id);
        $("#hiddenAccessObjectId").val(data.AccessObjectId);
        $("#Description").val(data.Description);
        $("#Path").val(data.Path);

        $("#selectAccessType").UifSelect("setSelected", data.AccessTypeId)
        if (!data.Enabled) {
            $('#chkDisabled').prop("checked", true);
        } else {
            $('#chkDisabled').prop("checked", false);
        }
        $("#inputSearch").val(data.Description);
        Access.validateAccessType(data.ParentAccessId);
    }
    static clearPanel() {
        
        ClearValidation("#formAccess");
        accessIndex = null;
        $("#hiddenId").val('');
        $("#hiddenAccessObjectId").val('');
        $("#Description").val('');
        $("#inputSearch").val('');
        $("#Path").val('');
        $('#chkDisabled').prop("checked", false);
        $("#selectParentAccess").UifSelect("setSelected", null);
        $("#selectSubModuleAccess").UifSelect("setSelected", null);
        $("#selectModuleAccess").UifSelect("setSelected", null);
        $("#selectAccessType").UifSelect("setSelected", null);        

        if (!IsNew) {
            IsNew = true;
            AccessObject.GetAccessObject().done(function (data) {
                if (data.success) {
                    $.each(data.result, function (key, value) {
                        value.StatusType = ParametrizationStatus.Original;
                    });
                    accesses = data.result;
                    Access.loadAccess();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        IsNew = false;

    }

    static deleteCallbackList(deferred, result) {
        //jtorres deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined && result.Id !== "0") //Se elimina unicamente si existe en DB
        {
            result.Status = 'Deleted';
            result.StatusType = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            //accessDelete.push(result);
            $("#listAccess").UifListView("addItem", result);//jtorres
        }

        for (var i = 0; i < accesses.length; i++) {
            if (accesses[i].Id == result.Id && accesses[i].ModuleId == result.ModuleId && accesses[i].AccessTypeId == result.AccessTypeId && accesses[i].Description == result.Description) {
                accesses.splice(i, 1);
                continue;
                //jtorres$("#listAccess").UifListView("addItem", result);
            }
        }
        //jtorres IsNew = true;
        //jtorres Access.clearPanel();
        //Access.filterListView();
        //jtorres $("#listAccess").UifListView("addItem", result);
        deferred.resolve();//jtorres
    }

    //deleteItemAccess(event, data) {
    //    $("#listAccess").UifListView({
    //        source: null,
    //        displayTemplate: "#AccessTemplate",
    //        selectionType: 'single',
    //        edit: true,        
    //        delete: true,    
    //        customEdit: true,
    //        deleteCallback: Access.deleteCallbackList,
    //        height: 300
    //    });

    //    accessIndex = null;
    //    $("#Description").val('');
    //    Access.filterListView();
    //}
	addItemAccess() {
        var formAccess = $("#formAccess").serializeObject();
        if (Access.validateAddAccess(formAccess)) {
            var enabled = true;
            var enabledDescription = Resources.Language.LabelEnabled;

            if ($('#chkDisabled').is(':checked')) {
                enabled = false;
                enabledDescription = Resources.Language.Disabled;
            }
            var access =
                {
                    Id: $("#hiddenId").val(),
                    AccessObjectId: parseInt($("#hiddenAccessObjectId").val()),
                    ModuleId: parseInt($("#selectModuleAccess").UifSelect("getSelected")),
                    ModuleDescription: $("#selectModuleAccess").UifSelect("getSelectedText"),
                    Path: $("#Path").val(),
                    SubModuleId: parseInt($("#selectSubModuleAccess").UifSelect("getSelected")),
                    SubModuleDescription: $("#selectSubModuleAccess").UifSelect("getSelectedText"),
                    Description: $("#Description").val(),
                    AccessTypeId: parseInt($("#selectAccessType").UifSelect("getSelected")),
                    AccessTypeDescription: $("#selectAccessType").UifSelect("getSelectedText"),
                    Enabled: enabled,
                    EnabledDescription: enabledDescription,
                    Status: 'Modified',
                    ParentAccessId: $("#selectParentAccess").UifSelect("getSelected")
                }
            if (accessIndex == null) {
                access.StatusType = ParametrizationStatus.Create;
                accesses.push(access);
                $("#listAccess").UifListView("addItem", access);
            }
            else {
                for (var i = 0; i < accesses.length; i++) {
                    if (accesses[i].Id == access.Id && accesses[i].ModuleId == access.ModuleId && accesses[i].AccessTypeId == access.AccessTypeId && accesses[i].Description == access.Description) {
                        accesses.splice(i, 1);
                        break;
                    }
                }
                access.StatusType= ParametrizationStatus.Update;
                accesses.push(access);
                $.each($("#listAccess").UifListView("getData"), function (index, value) {
                    if (value.Id == access.Id) {
                        $('#listAccess').UifListView('editItem', index, access);
                    }
                });
            }
            IsNew = true;
            Access.clearPanel();
        }
    }

    static validateAddAccess(formClause) {
        var band = false;
        if ($("#formAccess").valid()) {
            band = true;
        }
        return band;
    }

    saveAccess() {
		var access = $("#listAccess").UifListView('getData');
		access = access.filter(x => x.StatusType != null && x.StatusType != ParametrizationStatus.Original);
		if (access.length > 0) {
			$.ajax({
				type: "POST",
				url: rootPath + 'UniqueUser/Access/SaveAccess',
				data: JSON.stringify({ accessModelView: access }),
				dataType: "json",
				contentType: "application/json; charset=utf-8"
			}).done(function (data) {
				var dataNum = data;
				if (data.success) {
					accessDelete = [];
					AccessObject.GetAccessObject().done(function (data) {
						if (data.success) {
							accesses = data.result;
							Access.loadAccess();
							$.UifNotify('show', {
								'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
									"Agregados" + ':' + dataNum.result.TotalAdded + '<br> ' +
									"Modificados" + ':' + dataNum.result.TotalModified + '<br> ' +
									"Eliminados" + ':' + dataNum.result.TotalDeleted + '<br> ' +
									"Errores" + ':' + dataNum.result.Message,
								'autoclose': true
							});
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
    static validateAccessType(selectedId) {
        $("#selectParentAccess").UifSelect("disabled", true);
        $("#selectParentAccess").UifSelect("setSelected", null);
        $("#labelPath").text(Resources.Language.Path);
        if ($("#selectAccessType").UifSelect("getSelected") == AccessObjectType.Button
            && $("#selectModuleAccess").val() != ""
            && $("#selectSubModuleAccess").val() != "") {
            var ModuleAccess = $("#selectModuleAccess").UifSelect("getSelected");
            var SubModuleAccess = $("#selectSubModuleAccess").UifSelect("getSelected");
            AccessObject.GetParentAccess(ModuleAccess, SubModuleAccess)
                .done(function (data) {
                    if (data.success) {
                        if (selectedId != undefined) {
                            $("#selectParentAccess").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                        }
                        else {
                            $("#selectParentAccess").UifSelect({ sourceData: data.result, native: false, filter: true });
                        }

                        $("#selectParentAccess").UifSelect("disabled", false);
                        $("#labelPath").text(Resources.Language.Button);
                    }
                });
        }
        Access.filterListView();
    }


}
