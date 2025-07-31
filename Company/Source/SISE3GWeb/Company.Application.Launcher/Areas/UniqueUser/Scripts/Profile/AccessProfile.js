class AccessProfileServices {
    static GetAccessProfile(moduleId, subModuleId, typeId, profileId, parent) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'UniqueUser/Profile/GetAccessProfile',
			data: JSON.stringify({ moduleId: moduleId, subModuleId: subModuleId, typeId: typeId, profileId: profileId, parent: parent }),
			dataType: "json",
			contentType: "application/json; charset=utf-8"
			
        });
    }

}

class AccessProfile extends Uif2.Page {
	getInitialState() {
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
                $('#selectTypeAccess').UifSelect({ sourceData: data.result });
                $('#selectTypeAccess').UifSelect('disabled', true);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        glbProfile.Accesses = $('#TableAccess').UifDataTable('getData');
        $('#tableProfileAccess').UifDataTable({ sourceData: null });
    }
    bindEvents() {
        $('#selectSubModuleAccess').UifSelect('setSelected', "- Seleccione un Item -");
        $('#btnAssignedAccesses').on('click', this.saveAndLoad);
        $('#btnAccessSave').on('click', this.SaveAccess);
        $('#selectModuleAccess').on('itemSelected', AccessProfile.getSubModule);
        $('#selectSubModuleAccess').on('itemSelected', AccessProfile.ValidateAndSearch);
        $('#selectTypeAccess').on('itemSelected', AccessProfile.ValidateAndSearch);
        $('#selectModuleAccess').on('itemSelected', AccessProfile.clearTable);
        $('#selectHierarchyParent').on('itemSelected', AccessProfile.getAccessProfile);
        $('#btnAccessClose').on('click', this.clear);
        $('#tableProfileAccess').on('rowSelected', AccessProfile.SelectAccessProfile);
        $(".case").prop("checked", true);
        $('#tableProfileAccess').on('rowDeselected', AccessProfile.SelectAccessProfile);
        $(".case").prop("checked", false);
        $('#tableProfileAccess').on('selectAll', AccessProfile.SelectAllAccessProfile);
        $(".case").prop("checked", true);
        $('#tableProfileAccess').on('desSelectAll', AccessProfile.SelectAllAccessProfile);
        $(".case").prop("checked", false);
    }

    static SelectAllAccessProfile(event) {
        event.preventDefault();
        const selecteds = $("#tableProfileAccess").UifDataTable('getSelected');
        var dataTable = $('#tableProfileAccess').UifDataTable('getData');
        if (selecteds != null) {
            dataTable.forEach(function (item) {
                item.Assigned = true;
            });
        } else {
            dataTable.forEach(function (item) {
                item.Assigned = false;
            });
        }
    }

    

    static SelectAccessProfile(event, data, position) {
        event.preventDefault();
        var value = {
            label: 'AccessId',
            values: [data.AccessId]
        }
        data.Assigned = !data.Assigned;
       
    }
    static getSubModule() {
        var controller = rootPath + 'UniqueUser/SubModule/GetSubModulesByModuleId?moduleId=' + $("#selectModuleAccess").val();
        $("#selectSubModuleAccess").UifSelect({ source: controller });
        $("#selectTypeAccess").UifSelect('disabled', true);
        $("#selectTypeAccess").UifSelect('setSelected', "");
    }
    static ValidateAndSearch() {
        AccessProfile.validateAccessType();
        AccessProfile.getAccessProfile();
    }
    static getAccessProfile() {
        $("#selectTypeAccess").UifSelect('disabled', false);
		var parent = 0, moduleId, subModuleId, typeId, profileId;
        if ($("#selectTypeAccess").UifSelect("getSelected") == AccessObjectType.Button && $("#selectHierarchyParent").val() != "" && $("#selectHierarchyParent").val() != null) {
            $("#selectTypeAccess").UifSelect('disabled', true);
            parent = $("#selectHierarchyParent").val();
        }
        if ($("#selectSubModuleAccess").val() != 0 && $("#selectTypeAccess").val() != 0) {

            moduleId = $("#selectModuleAccess").val(),
                subModuleId = $("#selectSubModuleAccess").val(),
                typeId = $("#selectTypeAccess").val(),
                profileId = $("#hiddenId").val(),
				parent = parent
			
			AccessProfileServices.GetAccessProfile(moduleId, subModuleId, typeId, profileId, parent).done(function (data) {
                $('#tableProfileAccess').UifDataTable({ sourceData: data.aaData });
                
                if (data.aaData.length > 0) {
                    var AccessIsAssigned =
                        {
                            label: 'AccessId',
                            values: []
                        };
                    $('#tableProfileAccess').UifDataTable('clear');
                    if (data.aaData.length  > 0) {
                        $('#tableProfileAccess').UifDataTable('addRow', data.aaData);
                        $.each(data.aaData, function (id, item) {

                            if (item.Assigned == true) {
                                AccessIsAssigned.values.push(item.AccessId);
                            }
                        });
                        $("#tableProfileAccess").UifDataTable('setSelect', AccessIsAssigned)
                    }
                    
                    $('#tableProfileAccess').UifDataTable('order', [3, 'asc']);
                }
                else {
                    $('#tableProfileAccess').UifDataTable('clear');
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.NotAssociatedProfiles, 'autoclose': true });
                }
            })
        }
    }

    clear() {
        itemSelected = [];
        //$("#listViewProfileAccess").UifListView({ sourceData: null, displayTemplate: "#listViewProfileAccessTemplate", selectionType: 'multiple', height: 300 });
        $('#tableProfileAccess').UifDataTable('clear');
        AccessProfile.clearSelects();
    }

    static clearTable(index) {       
        $('.uif-listview').UifListView('clear');
        $('#tableProfileAccess').UifDataTable('clear');
    }

    static clearSelects() {
        $('#selectModuleAccess').UifSelect('setSelected', "");
        $("#selectSubModuleAccess").UifSelect('setSelected', "");
        $("#selectTypeAccess").UifSelect('setSelected', "");
        $("#selectSubModuleAccess").UifSelect("disabled", true);
        $("#selectTypeAccess").UifSelect("disabled", true);
    }

    saveAndLoad() {
    //Guardar los accesos
        $.each(itemSelected, function (id, item) {
            if (this.Assigned == false) {
                this.Assigned = true;
            }
        });
		if (glbProfile != null) {
            if (glbProfile.Id == 0) {
                AccessProfile.clearSelects();
                AccessProfile.clearTable();
                Profile.completeSaveProfile();
                Profile.saveProfile();
            }
            else {                
                AccessProfile.loadPartialAccess();
                AccessProfile.clearSelects();
                AccessProfile.clearTable();
            }
        }
    }
    static loadPartialAccess() {
        Profile.showPanelsProfile(MenuType.Access);
    }
    SaveAccess() { 
        if (glbProfileAccess.length > 0) {
            glbProfileAccess = glbProfileAccess.concat($('#tableProfileAccess').UifDataTable('getData'));
            $("#modalAccess").UifModal('hide');
        }
        else {
            glbProfileAccess = ($('#tableProfileAccess').UifDataTable('getData'));
            $("#modalAccess").UifModal('hide');
        }
        
        
    }

    static validateAccessType() {
        $("#selectHierarchyParent").UifSelect("disabled", true);
        $("#selectTypeAccess").UifSelect('disabled', true);
        if ($("#selectTypeAccess").UifSelect("getSelected") == AccessObjectType.Button
            && $("#selectModuleAccess").val() != ""
            && $("#selectSubModuleAccess").val() != "") {
            var controller = rootPath + 'UniqueUser/Access/GetListAccessObject?moduleId=' + $("#selectModuleAccess").val() + "&subModuleId=" + $("#selectSubModuleAccess").val();
            $("#selectHierarchyParent").UifSelect({ source: controller });
            $("#selectHierarchyParent").UifSelect("disabled", false);
        }
    }
}
