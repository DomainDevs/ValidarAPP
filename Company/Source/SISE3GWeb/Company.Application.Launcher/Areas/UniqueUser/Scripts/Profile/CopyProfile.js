
class CopyProfile extends Uif2.Page {
    getInitialState() {

    }
    bindEvents() {
        $('#btnCopyProfile').on('click', this.saveAndLoad);
        $('#btnCopySave').on('click', CopyProfile.copy);
    }
    static clearPanel() {
        $('#Description').val('');
    }
    saveAndLoad() {
        if (glbProfile != null && glbProfile.Id != undefined) {
            if (glbProfile.Id == 0) {
                if (Profile.saveProfile()) {
                    CopyProfile.loadPartialCopy();
                }
            }
            else {
                CopyProfile.loadPartialCopy();
            }
        }
    }
    static loadPartialCopy() {
        Profile.showPanelsProfile(MenuType.Copy);
        CopyProfile.clearPanel();
    }
    static copy() {
        if (glbProfile.Id != 0) {
            $("#formCopy").validate();
            if ($("#formCopy").valid()) {
                glbProfile.Description = $('#Description').val();
                $.ajax({
                    type: "POST",
                    url: rootPath + 'UniqueUser/Profile/CopyProfile',
                    data: JSON.stringify({ profileModel: glbProfile }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        if (data.result != null && data.result.Id != 0) {
                            Profile.getProfileByDescription(data.result.Id, "");
                            $('#modalCopy').UifModal("hide");
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ProfileCreated, 'autoclose': true })
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveUser, 'autoclose': true })
                });
            }
        }
    }

}