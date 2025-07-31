var inputSearch = "";
var idProfile = 0;
var glbProfile = { Id: 0 };
var itemSelected = [];
var completeSave = false;
var glbProfileAccess = [];
var glbStatusGuarantee = [];

$.ajaxSetup({ async: true });

class ProfileFile {
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Profile/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class Profile extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }
    bindEvents() {
        $('#btnNewProfile').on('click', Profile.clearPanel);
        $('#btnSaveProfile').on('click', Profile.saveProfile);
        $('#inputSearch').on('search', this.inputSearch);
        $('#tableResults tbody').on('click', 'tr', this.SelectSearch);
        $('#btnExit').click(this.Exit);
        GetAccessButtonsByPath(window.location.pathname + window.location.search);
        $('#btnSendExcel').on('click', this.sendExcelModule);
        $('#tableGuaranteeStatus').UifDataTable('clear');
       // $('#inputSearch').on('itemSelected', this.inputSearch);
    }
    SelectSearch() {
        Profile.getProfileByDescription($(this).children()[0].innerHTML, "");
        $('#modalDefaultSearch').UifModal("hide");
    }
    inputSearch(event, selectedItem) {
        if (!selectedItem || !selectedItem.trim() || selectedItem.length < 3) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInputSearchCoverage, 'autoclose': true })
            return;
        }
        else {
            inputSearch = selectedItem;
            Profile.getProfileByDescription(0, selectedItem);         
        }        
    }
    Exit() {
        window.location = rootPath + "Home/Index";
	}
    sendExcelModule() {
        ProfileFile.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static clearPanel() {
        moduleIndex = null;
        $("#InputDescription").val('');
        $("#InputDescription").focus();
        $("#inputSearch").val('');
        $('#chkDisabled').prop("checked", false);
        AccessProfile.clearTable();
        glbProfile = { Id: 0 };
		$("#hiddenId").val('0');
		$('#selectedAssignedAccesses').text("");
        ClearValidation("#formProfile");
    }
    static validateForm() {
        $("#formProfile").validate();
        return $("#formProfile").valid();
    }
	static saveProfile() {
        var description = $("#InputDescription").val();
        $("#formProfile").validate();
        var model = Profile.getProfileModel();
        //model.profileAccesses = itemSelected;
        model.profileAccesses = glbProfileAccess;
        model.guaranteeStatus = glbStatusGuarantee;
        //glbProfile.Accesses;
        if ($("#formProfile").valid()) {
            var data = $("#formProfile").serializeObject();
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/Profile/SaveProfile',
                data: JSON.stringify({ profileModel: model }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
				if (data.success) {
					Profile.clearPanel();
                    itemSelected = [];
                    glbProfileAccess = [];
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation, 'autoclose': true })
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
                    completeSave = false;
                }
                if (completeSave) {
                    Profile.getProfileByDescription(0, description);
                    AccessProfile.loadPartialAccess();
                    AccessProfile.clearSelects();
                    AccessProfile.clearTable();
                    completeSave = false;
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveUser, 'autoclose': true })
            });
        }
    }

    static completeSaveProfile() {
        completeSave = true;
    }

    static getProfileModel() {
        glbProfile.Description = $("#InputDescription").val();
        glbProfile.Id = $("#hiddenId").val();
        if ($('#chkDisabled').is(':checked')) {
            glbProfile.Enabled = false;
        }
        else {
            glbProfile.Enabled = true;
        }
        return glbProfile;
    }
    static getProfileByDescription(id, description) {
        if ((id != 0 || description.length != "")) {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/Profile/GetProfileByDescription',
                data: JSON.stringify({ description: description, id: id }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        Profile.showData(data.result);
                    }
                    else {
                        Profile.showDataAdvanced(data.result);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ProfileNotFound, 'autoclose': true })
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearch, 'autoclose': true })
            });
        }
    }
    static showData(data) {
        if (data.length == 1) {
            glbProfile = data[0];
            AccessProfile.clearTable();
            glbProfile.Accesses = $('#TableAccess').UifDataTable('getData');
            if (glbProfile.HasAccess) {
                $('#selectedAssignedAccesses').text("(" + Resources.Language.LabelVarious + ")");
            }
            else {
                $('#selectedAssignedAccesses').text("(" + Resources.Language.WithoutAccess + ")");
            }
            $("#InputDescription").val(glbProfile.Description);
            $("#inputSearch").val(glbProfile.Description);
            $("#hiddenId").val(glbProfile.Id);
            if (glbProfile.Enabled) {
                $('#chkDisabled').prop("checked", false);
            }
            else {
                $('#chkDisabled').prop("checked", true);
            }
            ClearValidation("#formProfile");
        }
        else if (data.length > 1) {
            //modalListType = 2;
            var dataList = [];
            for (var i = 0; i < data.length; i++) {
                dataList.push({
                    Id: data[i].Id,
                    Description: data[i].Description
                });
            }


            //$("#listViewSearchAdvancedP").UifListView({
            //    displayTemplate: "#advancedSearchTemplateP",
            //    selectionType: 'single',
            //    sourceData: dataList,
            //    height: 150

            //});
            ProfileAdvSearch.ClearAdvanced();
            dropDownSearch.show();           
        }
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }
    hidePanelsProfile(Menu) {
        switch (Menu) {
            case MenuType.Profile:
                break;
            case MenuType.Access:
                $("#modalAccess").UifModal('hide');
                break;
            case MenuType.Copy:
                $("#modalCopy").UifModal('hide');
                break;
            default:
                break;
        }
    }
    static showPanelsProfile(Menu) {
        switch (Menu) {
            case MenuType.Profile:
                break;
            case MenuType.Access:
                $("#modalAccess").UifModal('showLocal', Resources.Language.AccessAllowProfile);
                break;
            case MenuType.Copy:
                $("#modalCopy").UifModal('showLocal', Resources.Language.CopyProfile);
                break;
            case MenuType.ProfileContextPermissions:
                $("#modalProfileAccessPermissions").UifModal('showLocal', Resources.Language.ContextPermission);
                break;
            case MenuType.GuaranteeStatus:
                $("#modalGuaranteeStatus").UifModal('showLocal', Resources.Language.GuaranteeStatus);
                break;
                
            default:
                break;
        }
    }
    static showDataAdvanced(data) {
        ProfileAdvSearch.LoadAdvanced(data);
        dropDownSearch.show();
    }
}
