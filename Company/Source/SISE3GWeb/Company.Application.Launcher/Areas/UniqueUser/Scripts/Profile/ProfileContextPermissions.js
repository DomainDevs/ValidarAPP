class ProfileContextPermissions extends Uif2.Page {
    getInitialState() {

    }
    bindEvents() {
        
        $('#btnProfileContextPermissions').on('click', this.saveAndLoad);
        $('#selectPermissionsAccess').on('itemSelected', ProfileContextPermissions.GetContextPermissions);
        $('#tableContextPermissions').on('rowSelected', ProfileContextPermissions.SelectAccessUser);
        $('#tableContextPermissions').on('rowDeselected', ProfileContextPermissions.SelectAccessUser);
        $('#tableContextPermissions').on('selectAll', ProfileContextPermissions.SelectAllAccessUser);
        $('#tableContextPermissions').on('desSelectAll', ProfileContextPermissions.SelectAllAccessUser);
        $('#btnPermissionsSave').on('click', ProfileContextPermissions.SavecontextPermissions)
    }

    saveAndLoad()
    {
        
        ProfileContextPermissions.ClearcontextPermissions();

        if (glbProfile.Id != 0) {

            if (Profile.validateForm()) {
                ProfileContextPermissions.loadUserPermissions();
            }

        }
        else {
            Profile.validateForm();
        }
    }
    static loadUserPermissions() {
        ProfileContextPermissions.getPermissions();
        Profile.showPanelsProfile(MenuType.ProfileContextPermissions);
    }

    static getPermissions() {
        var profileId = $("#hiddenId").val();
        ProfileContextPermissionsRequest.GetPermissionsByProfileId(profileId).done(function (data) {
            if (data.success) {
                $("#selectPermissionsAccess").UifSelect({ sourceData: data.result });
            }
        });
    }
    static GetContextPermissions() { 
        var profileId = $("#hiddenId").val();
        ProfileContextPermissionsRequest.GetContextPermissionsByProfileIdPermissionsId(profileId, $("#selectPermissionsAccess").UifSelect("getSelected")).done(function (data) {
            $('#tableContextPermissions').UifDataTable({ sourceData: data.result, checkColumnName: 'Assigned' });
          
            if (data.result == null || data.result.length <= 0) {
                
                $('#tableContextPermissions').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NotAssociatedProfiles, 'autoclose': true });
            }
        });
    }

    static SelectAccessUser(event, data, position) {
        event.preventDefault();
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
        if (ProfileContextPermissions.validateForm())
        {
            glbUserAccess = $('#tableContextPermissions').UifDataTable('getData');
            var profileId = $("#hiddenId").val();
            glbUserAccess.forEach(function (item) {
                item.ProfileId = profileId;
                item.PermissionsId = $("#selectPermissionsAccess").UifSelect("getSelected");
            });

            ProfileContextPermissionsRequest.SaveContextPermissions(glbUserAccess).done(function (data) {
                
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



class ProfileContextPermissionsRequest {

    static GetPermissionsByUserId(UserId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetPermissionsByUserId',
            data: JSON.stringify({ UserId: UserId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPermissionsByProfileId(profileId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetPermissionsByProfileId',
            data: JSON.stringify({ profileId: profileId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetContextPermissionsByProfileIdPermissionsId(UserId,permissionsId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetContextPermissionsByProfileIdPermissionsId',
            data: JSON.stringify({ UserId: UserId, permissionsId: permissionsId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveContextPermissions(contextPermissions) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/SaveContextPermissions',
            data: JSON.stringify({ contextPermissions: contextPermissions}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

