var hierarchyIndex = null;
var hierarchy = {};
//$.ajaxSetup({ async: true });

class HierarchyRequest {
    /**
  * @summary 
  * peticion ajax que Obtiene las jerarquias
  **/
    static GetCoHierarchiesAssociationByModuleSubModule(idModule, idSubmodule) {
        return $.ajax({
            type: "GET",
            data: { "moduleId": idModule, "SubModuleId": idSubmodule },
            url: rootPath + "UniqueUser/SubModule/GetCoHierarchiesAssociationByModuleSubModule"
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
}

class Hierarchy extends Uif2.Page {
    static deleteCallback(deferred, data) {
        deferred.resolve();
    }
    getInitialState() {
        $("#listHierarchy").UifListView({ displayTemplate: "#hierarchyTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Hierarchy.deleteCallback, height: 300 });
        
        Modules.GetModules().done(function (data) {
            if (data.success) {
                $('#selectModule').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    bindEvents() {
        $('#btnHierarchy').on('click', this.saveAndLoad);
        $('#btnHierarchyAccept').on('click', this.addItemHierarchy);
        $('#btnHierarchyCancel').on('click', UniqueUser.hidePanelsUser(MenuType.Hierarchy));
        $('#btnNewHierarchy').on('click', Hierarchy.clearPanel);
        $('#btnHierarchySave').on('click', this.SaveHierarchies);
        $('#selectModule').on('itemSelected', Hierarchy.getSubModule);
        $('#selectSubModule').on('itemSelected', Hierarchy.getHierarchies);
        $('#listHierarchy').on('rowEdit', this.setHierarchy);
        $('#listHierarchy').on('rowDelete', this.deleteItemHierarchy);
    }

    saveAndLoad() {
        if ($("#LoginName").val().trim() != "") {
            if (glbUser.UserId == 0) {
                if (UniqueUser.validateForm()) {
                    Hierarchy.loadPartialHierarquies();
                }
            }
            else {
                Hierarchy.loadPartialHierarquies();
            }
        }
        else {
            UniqueUser.validateForm();
        }
    }

    static getSubModule() {
        HierarchyRequest.GetSubModule($("#selectModule").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                $("#selectSubModule").UifSelect({ sourceData: data.result, native: false, filter: true });
            }
        });
    }
    static getHierarchies() {
        var controller = rootPath + 'UniqueUser/SubModule/GetCoHierarchiesAssociationByModuleSubModule?moduleId=' + $("#selectModule").val() + "&SubModuleId=" + $("#selectSubModule").val();
        $("#selectHierarchy").UifSelect({ source: controller });
    }
    addItemHierarchy() {
        $("#formHierarchy").validate();
        if ($("#formHierarchy").valid()) {
            hierarchy =
                {
                    Module: $("#selectModule").UifSelect("getSelectedText"),
                    ModuleId: $("#selectModule").val(),
                    SubModule: $("#selectSubModule").UifSelect("getSelectedText"),
                    SubModuleId: $("#selectSubModule").val(),
                    Hierarchy: $("#selectHierarchy").UifSelect("getSelectedText"),
                    HierarchyId: $("#selectHierarchy").val(),
                    StatusType: 1
                }

            if ($("#selectHierarchy").val() != null && $("#selectHierarchy").val() != "") {
                //Validación repetida
                var canAdd = true;
                var listAdd = $("#listHierarchy").UifListView('getData');
                if (listAdd.length >= 0) {
                    $.each(listAdd, function (index, value) {
                        if (value.ModuleId == hierarchy.ModuleId && value.SubModuleId == hierarchy.SubModuleId && value.HierarchyId == hierarchy.HierarchyId) {
                            canAdd = false;
                        }
                    });
                }
                if (canAdd) {
                    if (hierarchyIndex == null) {
                        hierarchy.StatusTypeService = ParametrizationStatus.Create;
                        $("#listHierarchy").UifListView("addItem", hierarchy);
                    }
                    else {
                        hierarchy.StatusTypeService = ParametrizationStatus.Update;
                        $('#listHierarchy').UifListView('editItem', hierarchyIndex, hierarchy);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.HierarchieFound, 'autoclose': true });
                }
                listViewColors("listHierarchy");
                Hierarchy.clearPanel();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDocumentControlHierarchy, 'autoclose': true });
            }
        }
    }
    static clearPanel() {
        hierarchyIndex = null;
        $("#selectModule").UifSelect("setSelected", null);
        $("#selectSubModule").UifSelect("setSelected", null);
        $("#selectHierarchy").UifSelect("setSelected", null);
        ClearValidation('#formHierarchy');
    }

    static loadPartialHierarquies() {
        UniqueUser.showPanelsUser(MenuType.Hierarchy);
        Hierarchy.loadHierarchies();
        
        
    }
    static loadHierarchiesNull() {
        //$("#listHierarchy").UifListView({ source: null, displayTemplate: "#hierarchyTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Hierarchy.deleteCallback, height: 300 });
        $("#listHierarchy").empty();
    }
    static loadHierarchies() {
        $("#listHierarchy").UifListView({ source: null, displayTemplate: "#hierarchyTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Hierarchy.deleteCallback,  height: 300 });
        if (glbUser.Hierarchies != undefined) {
            $.each(glbUser.Hierarchies, function (key, value) {
                hierarchy =
                    {
                        Module: this.Module.Description,
                        ModuleId: this.Module.Id,
                        SubModule: this.SubModule.Description,
                        SubModuleId: this.SubModule.Id,
                        Hierarchy: this.Description,
                        HierarchyId: this.Id,
                        StatusType: 1
                    }
                $("#listHierarchy").UifListView("addItem", hierarchy);
            })
        }
    }
    setHierarchy(event, data, index) {
        if (data != undefined) {
            hierarchyIndex = index;
            $("#selectModule").UifSelect("setSelected", data.ModuleId);
            Hierarchy.getSubModuleHierarchyById(data.SubModuleId, data.HierarchyId);
        }
    }
    static getSubModuleHierarchyById(subModuleId, hierarchyId) {
        HierarchyRequest.GetSubModule($("#selectModule").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                if (subModuleId != 0) {
                    $("#selectSubModule").UifSelect({ sourceData: data.result, selectedId: subModuleId, native: false, filter: true });
                }
                else {
                    $("#selectSubModule").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
            }
        });

        var controller = rootPath + 'UniqueUser/SubModule/GetCoHierarchiesAssociationByModuleSubModule?moduleId=' + $("#selectModule").val() + "&SubModuleId=" + subModuleId;     
        $('#selectHierarchy').UifSelect({ source: controller, selectedId: hierarchyId });
    }
    static getHierarchies() {
        var controller = rootPath + 'UniqueUser/SubModule/GetCoHierarchiesAssociationByModuleSubModule?moduleId=' + $("#selectModule").val() + "&SubModuleId=" + $("#selectSubModule").val();
        $("#selectHierarchy").UifSelect({ source: controller });
    }

    deleteItemHierarchy(event, data) {
        if (data != undefined) {
            var hierarchies = $("#listHierarchy").UifListView('getData');
            $("#listHierarchy").UifListView({ source: null, displayTemplate: "#hierarchyTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Hierarchy.deleteCallback,  height: 300 });

            $.each(hierarchies, function (index, value) {
                if (!(this.ModuleId == data.ModuleId && this.SubmoduleId == data.SubmoduleId && this.HierarchyId == data.HierarchyId)) {
                    $("#listHierarchy").UifListView("addItem", this);
                }
            });
            Hierarchy.clearPanel();
        }
    }
    SaveHierarchies() {
        var hierarchies = $("#listHierarchy").UifListView('getData');
        glbUser.Hierarchies = [];
        $.each(hierarchies, function (index, value) {
            hierarchy = {
                Id: value.HierarchyId, Description: value.Hierarchy,
                Module: { Id: value.ModuleId, Description: value.Module },
                SubModule: { Id: value.SubModuleId, Description: value.SubModule }
            }
            glbUser.Hierarchies.push(hierarchy);
        });
        UniqueUser.hidePanelsUser(MenuType.Hierarchy);
        UniqueUser.LoadSubTitles();
    }
}
