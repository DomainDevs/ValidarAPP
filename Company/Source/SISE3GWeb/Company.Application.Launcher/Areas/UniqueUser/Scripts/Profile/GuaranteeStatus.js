class RequestGuaranteeStatus {
    static GetGuaranteeStatusByProfile(profileId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Guarantees/Guarantees/GetProfileGuaranteeStatus',
            data: JSON.stringify({ profileId: parseInt(profileId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"

        });
    }
    static GetGuaranteeStatus(profileId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Guarantees/Guarantees/GetGuaranteeStatus',
            dataType: "json",
            contentType: "application/json; charset=utf-8"

        });
    }
    static CreateGuaranteeStatus(profileGuaranteeStatusModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Guarantees/Guarantees/CreateGuaranteeStatus',
            data: JSON.stringify({ profileGuaranteeStatus: profileGuaranteeStatusModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}
var dataGuarantee = [];
class GuaranteeStatus extends Uif2.Page {
    getInitialState() {
        RequestGuaranteeStatus.GetGuaranteeStatus().done(function (data) {
            for (var i = 0; i < data.data.length; i++) { data.data[i].Enabled = false; data.data[i].IdGuaranteeStatus = 0 }
            dataGuarantee = data.data;
            $("#tableGuaranteeStatus").UifDataTable({ sourceData: data.data });
        });
    }
    bindEvents() {
        $('#btnProfileGuaranteeStatus').on('click', this.saveAndLoad);
        $('#tableGuaranteeStatus').on('rowSelected', GuaranteeStatus.SelectGuarantee);
        $('#tableGuaranteeStatus').on('rowDeselected', GuaranteeStatus.SelectGuarantee);
        $('#tableGuaranteeStatus').on('selectAll', GuaranteeStatus.SelectAllGuarantee);
        $('#tableGuaranteeStatus').on('desSelectAll', GuaranteeStatus.SelectAllGuarantee);
        $('#btnStatusGuaranteeSave').on('click', this.SaveStatusGuarantee);
    }

    SaveStatusGuarantee() {
        glbStatusGuarantee = $('#tableGuaranteeStatus').UifDataTable('getData');
        $("#modalGuaranteeStatus").UifModal('hide');
    }

    saveAndLoad() {
        if (glbProfile.Id != 0) {

            if (Profile.validateForm()) {
                GuaranteeStatus.getGuarantees();
            }

        }
        else {
            Profile.validateForm();
        }
    }
    static getGuarantees() {
        var profileId = $("#hiddenId").val();
        RequestGuaranteeStatus.GetGuaranteeStatusByProfile(profileId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    var dataTable = $('#tableGuaranteeStatus').UifDataTable('getData');
                    for (var i = 0; i < data.result.length; i++) {
                        dataTable[i].Enabled = data.result[i].Enabled;
                    }
                    var toEnabled = data.result.filter(x => x.Enabled).map(x => x.StatusId);
                    var toDisabled = data.result.map(x => x.StatusId);
                    var value = {
                                    label: 'Id',
                                    values: toDisabled
                    }
                    $("#tableGuaranteeStatus").UifDataTable('setUnselect', value);
                    value.values = toEnabled;
                    $("#tableGuaranteeStatus").UifDataTable('setSelect', value);
                }
                else { $("#tableGuaranteeStatus").UifDataTable(dataGuarantee); }
                GuaranteeStatus.loadPartialGuaranteeStatus();
            } else {
                $("#selectedGuaranteeStatus").text('(0)');
            }

        });
    }

    static loadPartialGuaranteeStatus() {
        Profile.showPanelsProfile(MenuType.GuaranteeStatus);
    }

    static SelectAllGuarantee(event) {
        event.preventDefault();
        const selecteds = $("#tableGuaranteeStatus").UifDataTable('getSelected');
        var dataTable = $('#tableGuaranteeStatus').UifDataTable('getData');
        dataTable.forEach(function (item) {
            item.Enabled = false;
        });
        if (selecteds != null) {
            selecteds.forEach(function (selected) {
                dataTable.forEach(function (item) {
                    if (selected.securityContextId == item.securityContextId) {
                        item.Enabled = true;
                    }
                });
            });
        }

    }

    static SelectGuarantee(event, data, position) {
        event.preventDefault();
        data.Enabled = !data.Enabled;
    }

}
