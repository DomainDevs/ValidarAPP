class UserGroup extends Uif2.Page {
    bindEvents() {
        $('#btnUserGroup').on('click', this.saveAndLoad);
        $('#btnUserGroupSave').on('click', this.SaveUserGroups);
    }

    saveAndLoad() {
        if ($("#LoginName").val().trim() != "") {
            if (glbUser.UserId == 0) {
                if (UniqueUser.validateForm()) {
                    UserGroup.loadPartialUserGroup();
                }
            }
            else {
                UserGroup.loadPartialUserGroup();
            }
        }
        else {
            UniqueUser.validateForm();
        }
    }

    static loadPartialUserGroup() {        
        var number = glbUser.UserId;        
        if (number != "" || number != 0) {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetUserGroups',
                data: JSON.stringify({ userId: number }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $('#tableUserGroup').UifDataTable('clear');
                    $('#tableUserGroup').UifDataTable('addRow', data.result);
                    $.each(data.result, function (id, item) {
                        if (item.Check == true) {
                            $('#tableUserGroup tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                            $('#tableUserGroup tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                    });
                    $('#modalGroup').UifModal('showLocal');
                }
                else {
                    $("#modalGroup").UifModal('hide');
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearch, 'autoclose': true })
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $("#modalGroup").UifModal('hide');
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearch, 'autoclose': true })
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserNotExist, 'autoclose': true })
        }
    }

    SaveUserGroups() {
        var number = glbUser.UserId;    
        if (number != "" || number != 0) {
            var groupsSelected = [];
            $.each($("#tableUserGroup").UifDataTable("getSelected"), function (Id, item) {
                groupsSelected.push({
                    IdUser: number,
                    IdGroup: item.IdGroup
                });
            });
            if (groupsSelected.length == 0) {
                groupsSelected.push({
                    IdUser: number,
                    IdGroup: 0
                });
            }
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/SaveUserGroups',
                data: JSON.stringify({ groupsSelected }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $("#modalGroup").UifModal('hide');
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedSuccessfully, 'autoclose': true })
                }
                else {
                    $("#modalGroup").UifModal('hide');
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveData, 'autoclose': true })
                }                
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $("#modalGroup").UifModal('hide');
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveData, 'autoclose': true })
            });
        }
        else {
            $("#modalGroup").UifModal('hide');
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserNotExist, 'autoclose': true })
        }        
    }
}