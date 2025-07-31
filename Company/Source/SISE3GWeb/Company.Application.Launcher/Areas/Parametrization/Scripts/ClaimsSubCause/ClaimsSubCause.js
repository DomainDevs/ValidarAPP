var subCauseId = 0;
class ClaimsSubCause extends Uif2.Page {
    getInitialState() {
        ClaimsSubCause.GetPrefixes();
    }

    bindEvents() {
        $('#selectPrefix').on('itemSelected', ClaimsSubCause.SelectedPrefix);
        $('#selectCause').on('itemSelected', ClaimsSubCause.SelectedCause);
        $('#tblsubcause').on('rowAdd', ClaimsSubCause.HidePanelsSubcauses);
        $('#btnSave').on('click', ClaimsSubCause.ExecuteSubCauseOperatios);
        $('#tblsubcause').on('rowEdit', ClaimsSubCause.ToEditSubCauses);
        $('#tblsubcause').on('rowDelete', ClaimsSubCause.ToDeleteSubCause);
    }

    static GetPrefixes() {
        ClaimsSubCauseRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static SelectedPrefix(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsSubCauseRequest.GetCausesByPrefixId($('#selectPrefix').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectCause').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectCause').UifSelect();
        }

        $('#tblsubcause').UifDataTable('clear');
    }

    static SelectedCause(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsSubCauseRequest.GetSubCausesByCause($('#selectCause').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $("#tblsubcause").UifDataTable({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectCause').UifSelect();
        }

        $('#tblsubcause').UifDataTable('clear');
    }

    static ExecuteSubCauseOperatios() {
        $("#formDescription").validate();
        if ($("#formDescription").valid()) {
            var subCause = {
                Id: subCauseId,
                Description: $("#Description").val(),
                CauseId: $('#selectCause').UifSelect('getSelected')
            }

            ClaimsSubCauseRequest.ExecuteSubCauseOperations(subCause).done(function (data) {
                if (data.success) {
                    $('#modalClaimSubcause').UifModal('hide');
                    ClaimsSubCause.ClearSubcauseModal();
                    ClaimsSubCauseRequest.GetSubCausesByCause($('#selectCause').UifSelect('getSelected')).done(function (data) {
                        if (data.success) {
                            $("#tblsubcause").UifDataTable({ sourceData: data.result });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });

                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static ToDeleteSubCause(event, subCause, position) {
        ClaimsSubCauseRequest.DeleteSubCause(subCause.Id).done(function (data) {
            if (data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                ClaimsSubCauseRequest.GetSubCausesByCause($('#selectCause').UifSelect('getSelected')).done(function (data) {
                    if (data.success) {
                        $("#tblsubcause").UifDataTable({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });

            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ToEditSubCauses(event, subCause, position) {
        subCauseId = subCause.Id;
        $("#Description").val(subCause.Description);
        ClaimsSubCause.HidePanelsSubcauses();
    }

    static HidePanelsSubcauses() {
        $("#formSubCause").validate();
        if ($("#formSubCause").valid()) {
            $('#modalClaimSubcause').UifModal('showLocal', "Subcausa del Siniestro");
        }
    }

    static ClearSubcauseModal() {
        subCauseId = 0;
        $("#Description").val("");
    }
}
