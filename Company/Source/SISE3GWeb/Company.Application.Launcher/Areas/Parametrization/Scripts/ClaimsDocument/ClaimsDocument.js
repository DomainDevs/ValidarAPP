var documentationId = 0;
class ClaimsDocument extends Uif2.Page
{
    getInitialState() {
        ClaimsDocument.GetCompanyModule();
        ClaimsDocument.GetPrefixes();
    }

    bindEvents() {
        $('#selectModules').on('itemSelected', ClaimsDocument.selectedModules);
        $('#selectSubModules').on('itemSelected', ClaimsDocument.SelectedSubModule);
        $('#tblsubroles').on('rowAdd', ClaimsDocument.HideSubroles);  
        $('#btnSave').on('click', ClaimsDocument.ExecuteDocumentOperatios);
        $('#tblsubroles').on('rowEdit', ClaimsDocument.ToEditDocument);
        $('#tblsubroles').on('rowDelete', ClaimsDocument.ToDeleteDocument);
    }

    static GetPrefixes() {
        ClaimsDocumentRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#Prefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetCompanyModule() {
        ClaimsDocumentRequest.GetCompanyModule().done(function (data) {
            if (data.success) {
                $('#selectModules').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static selectedModules(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsDocumentRequest.GetCompanySubModule(selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#selectSubModules').UifSelect({ sourceData: data.result });
                }                
            });
        }
        else {
            $('#selectSubModules').UifSelect();
        }
    }

    static SelectedSubModule(event, selectedItem) {
            if (selectedItem.Id > 0) {
            ClaimsDocumentRequest.GetDocumentBySubmoduleId($('#selectSubModules').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#tblsubroles').UifDataTable({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectSubLineBusiness').UifSelect();
        }
    }
     

    static ExecuteDocumentOperatios() {
        $("#formDescription").validate();
        if ($("#formDescription").valid()) {
            var claimsDocumentationDTO = {
                Id: documentationId,
                Description: $("#Description").val(),
                Prefix: $('#Prefix').UifSelect('getSelected'),
                ModuleId: $('#selectModules').UifSelect('getSelected'),
                SubmoduleId: $('#selectSubModules').UifSelect('getSelected'),
                Enable: true,
                IsRequired: true
            }

            ClaimsDocumentRequest.ExecuteDocumentOperatios(claimsDocumentationDTO).done(function (data) {
                if (data.success) {
                    $('#modalClaimsDocument').UifModal('hide');
                    ClaimsDocumentRequest.GetDocumentBySubmoduleId($('#selectSubModules').UifSelect('getSelected')).done(function (data) {
                        if (data.success) {
                            $("#tblsubroles").UifDataTable({ sourceData: data.result });
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
    static ToDeleteDocument(event, claimsDocumentationDTO, position) {
        ClaimsDocumentRequest.DeleteDocumentation(claimsDocumentationDTO.Id).done(function (data) {
            if (data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                ClaimsDocumentRequest.GetDocumentBySubmoduleId($('#selectSubModules').UifSelect('getSelected')).done(function (data) {
                    if (data.success) {
                        $("#tblsubroles").UifDataTable({ sourceData: data.result });
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

    static ToEditDocument(event, claimsDocumentationDTO, position) {
        ClaimsDocument.HideSubroles();
        documentationId = claimsDocumentationDTO.Id;
        $("#Description").val(claimsDocumentationDTO.Description);
    }
    static HideSubroles() {
        $("#formClaimsDocument").validate();
        if ($("#formClaimsDocument").valid()) {
            ClaimsDocument.ClearSubrolesModal();
            $('#modalClaimsDocument').UifModal('showLocal', "Agregar documento de siniestro");
        }
    }

    static ClearSubrolesModal() {
        documentationId = 0;
        $("#Description").val("");
    }
}
