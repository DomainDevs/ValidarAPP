var documentTypeRangeId = 0;

$.ajaxSetup({ async: true });

class DocumentTypeRangeParametrization extends Uif2.Page
{
    getInitialState()
    {
        DocumentTypeRangeParametrization.getDocumentTypeRange();
        DocumentTypeRange.GetListIndividualTypes();

        //TIPOS PESONA
        DocumentTypeRangeParametrization.GetIndividualTypes();

        //TIPOS DOCUMENTO
        DocumentTypeRangeParametrization.GetDocumentType();

        //GENERO
        DocumentTypeRangeParametrization.GetGenderType();
    }

    bindEvents()
    {
        //EVENTOS
        $('#tblTypePerson').on('rowAdd', function (event)
        {
            documentTypeRangeId = 0;
            $('#AddDocumentTypeRangeModal').UifModal('showLocal', Resources.Language.Add);

            //Validar solo numeros
            $('#Inicial').ValidatorKey(ValidatorType.Number);
            $('#Final').ValidatorKey(ValidatorType.Number);

            //Limpiar Form
            DocumentTypeRangeParametrization.Clear();
           
        });

        $('#tblTypePerson').on('rowEdit', function (event, data, index)
        {
            $('#AddDocumentTypeRangeModal').UifModal('showLocal', Resources.Language.Add);

            //Validar solo numeros
            $('#Inicial').ValidatorKey(ValidatorType.Number);
            $('#Final').ValidatorKey(ValidatorType.Number);

            //Limpiar Form
            DocumentTypeRangeParametrization.Clear();
           
            //Obtiene los datos
            documentTypeRangeId = data.Id;

            DocumentTypeRangeParametrization.GetDocumentTypeById(documentTypeRangeId);
        });

        //Boton guardar Rangos de Cedulación
        $("#btnSaveDocumentTypeRange").click(this.SaveDocumentTypeRange);

        $('#btnExit').click(DocumentTypeRangeParametrization.redirectIndex)
    }

    static GetDocumentType()
    {
        DocumentTypeRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#selectTypeDocument").UifSelect({ sourceData: data.result });
            }
        });

    }

    static GetGenderType()
    {
        GenderRequest.GetGenderTypes().done(function (data)
        {
            if (data.success) {
                $("#selectGender").UifSelect({ sourceData: data.result });
            }
        });
    }

    static GetIndividualTypes()
    {
        DocumentTypeRange.GetIndividualTypes().done(function (data)
        {
            if (data.success) {
                $("#selectPersonType").UifSelect({ sourceData: data.result });
            }
        });
    }

    static Clear()
    {
        $('#Inicial').val('');
        $('#Final').val('');
        $("#selectPersonType").UifSelect('setSelected', null);
        $("#selectGender").UifSelect('setSelected', null);
        $("#selectTypeDocument").UifSelect('setSelected', null);
    }

    SaveDocumentTypeRange(e)
    {
        $("#formDocumentTypeRange").validate();
        if ($("#formDocumentTypeRange").valid())
        {
            e.stopPropagation()
            var form = $("#formDocumentTypeRange").serializeObject();

            if (documentTypeRangeId == 0) {
                request('Parametrization/DocumentTypeRange/SaveDocumentTypeRange', JSON.stringify({ ListDocumentTypeRange: form }), 'POST', Resources.Language.ErrorRequired, DocumentTypeRangeParametrization.successSave);
            }
            else
            {
                request('Parametrization/DocumentTypeRange/UpdateDocumentTypeRange', JSON.stringify({ ListDocumentTypeRange: form, documentTypeRangeId: documentTypeRangeId }), 'POST', Resources.Language.ErrorRequired, DocumentTypeRangeParametrization.successSave);
            }
        }
    }

    static successSave(data)
    {
        DocumentTypeRangeParametrization.Clear();
        $.UifNotify('show', {
            'type': 'info',
            'message': data + '',
            'autoclose': true
        });

        $('#AddDocumentTypeRangeModal').UifModal('hide');

        DocumentTypeRangeParametrization.getDocumentTypeRange();
    }

    static redirectIndex()
    {
        window.location = rootPath + "Home/Index";
    }

    static getDocumentTypeRange()
    {
        $.ajax({
            type: "POST",
            url: rootPath + 'DocumentTypeRange/GetDocumentTypeRange',
            data: JSON.stringify({  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $('#tblTypePerson').UifDataTable('clear');
                if (data.result.length > 0)
                {
                    $('#tblTypePerson').UifDataTable({ sourceData: data.result  });
                }
            }
        });
    }

    static GetDocumentTypeById(documentTypeRangeId)
    {
        $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "DocumentTypeRange/GetDocumentTypeRangeId",
            data: JSON.stringify({ "documentTypeRangeId": documentTypeRangeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        }).done(function (data)
        {
            if (data.success)
            {
                if (data.result.length > 0)
                {
                    setTimeout(function ()
                    {
                        $("#selectPersonType").UifSelect('setSelected', data.result[0].IndividualTypeId);
                        $("#selectGender").UifSelect('setSelected', data.result[0].Gender == "M" ? "1" : "2");
                        $("#selectTypeDocument").UifSelect('setSelected', data.result[0].CardTypeCode.Id);
                        $("#Inicial").val(data.result[0].CardNumberFrom);
                        $("#Final").val(data.result[0].CardNumberTo);
                    }, 100);                
                }
            }
        });
    }
     

}

 