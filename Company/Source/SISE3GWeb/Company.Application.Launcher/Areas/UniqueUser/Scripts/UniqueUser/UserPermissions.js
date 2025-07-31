class UserPermissions extends Uif2.Page {
    getInitialState() {

    }
    bindEvents() {
        $('#btnPermissions').on('click', this.saveAndLoad);
        $('#selectPermissionsAccess').on('itemSelected', UserPermissions.GetContextPermissions);
        $('#tableContextPermissions').on('rowSelected', UserPermissions.SelectAccessUser);
        $('#tableContextPermissions').on('rowDeselected', UserPermissions.SelectAccessUser);
        $('#tableContextPermissions').on('selectAll', UserPermissions.SelectAllAccessUser);
        $('#tableContextPermissions').on('desSelectAll', UserPermissions.SelectAllAccessUser);
        $('#btnPermissionsSave').on('click', UserPermissions.SavecontextPermissions)
    }

    saveAndLoad() {
        UserPermissions.ClearcontextPermissions();
        if ($("#LoginName").val().trim() != "") {
            if (glbUser.UserId == 0) {
                if (UniqueUser.validateForm()) {
                    UserPermissions.loadUserPermissions();
                }
            }
            else {
                UserPermissions.loadUserPermissions();
            }
        }
        else {
            UniqueUser.validateForm();
        }
    }
    static loadUserPermissions() {
        UserPermissions.getPermissions();
        UniqueUser.showPanelsUser(MenuType.Permissions);
    }

    static getPermissions() {
     
        UserPermissionsRequest.GetPermissionsByUserId(glbUser.UserId).done(function (data) {
            if (data.success) {
                $("#selectPermissionsAccess").UifSelect({ sourceData: data.result });
            }
        });
    }
    static GetContextPermissions() {
        UserPermissionsRequest.GetContextPermissionsByUserIdPermissionsId(glbUser.UserId, $("#selectPermissionsAccess").UifSelect("getSelected")).done(function (data) {
            $('#tableContextPermissions').UifDataTable({ sourceData: data.result, checkColumnName: 'Assigned' });
          
            if (data.result == null || data.result.length <= 0) {
                
                $('#tableContextPermissions').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NotAssociatedProfiles, 'autoclose': true });
            }
        });
    }

    static SelectAccessUser(event, data, position) {
        
        event.preventDefault();
        var value = {
            label: 'securityContextId',
            values: [data.securityContextId]
        }
        data.Assigned = !data.Assigned;
        
    }
    static SelectAllAccessUser(event) {
        event.preventDefault();
        const selecteds = $("#tableContextPermissions").UifDataTable('getSelected');
        var dataTable = $('#tableContextPermissions').UifDataTable('getData');
        dataTable.forEach(function (item) {
            item.Assigned = false;
        });
        if (selecteds != null) {
            selecteds.forEach(function (selected) {
                dataTable.forEach(function (item) {
                    if (selected.securityContextId == item.securityContextId) {
                        item.Assigned = true;
                    }
                });
            });
        }
        
    }
    static SavecontextPermissions() {
        if (UserPermissions.validateForm())
        {
            glbUserAccess = $('#tableContextPermissions').UifDataTable('getData');
            glbUserAccess.forEach(function (item) {
                item.UserId = glbUser.UserId;
            });

            UserPermissionsRequest.SaveContextPermissions(glbUserAccess, $("#selectPermissionsAccess").UifSelect("getSelected")).done(function (data) {
                
                if (data.result != null) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation, 'autoclose': true });
                }

            });
            $("#modalPermissions").UifModal('hide');
        }
    }
    static ClearcontextPermissions()
    {
        $('#tableContextPermissions').UifDataTable('clear');
        $("#selectPermissionsAccess").UifSelect({ sourceData: null });
     
    };
    static validateForm() {
        $("#formUserAccessPermissions").validate();
        return $("#formUserAccessPermissions").valid();
    }
        
}



class UserPermissionsRequest {

    static GetPermissionsByUserId(UserId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetPermissionsByUserId',
            data: JSON.stringify({ UserId: UserId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetContextPermissionsByUserIdPermissionsId(UserId,permissionsId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetContextPermissionsByUserIdPermissionsId',
            data: JSON.stringify({ UserId: UserId, permissionsId: permissionsId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveContextPermissions(contextPermissions, permisionsId, UserId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/SaveContextPermissions',
            data: JSON.stringify({ contextPermissions: contextPermissions, permisionsId: permisionsId, UserId: UserId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

