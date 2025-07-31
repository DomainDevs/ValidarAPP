var tableFinalBeneficiary;
var glbIndexBeneficiary = null;

class FinalBeneficiary extends Uif2.Page {
    getInitialState() {
        FinalBeneficiary.InitialFinalBeneficiary();
    }

    //Seccion Eventos
    bindEvents() {
        
        $('#btnFinalBeneficiaryAdd').on('click', FinalBeneficiary.AddFinalBeneficiary);
        $('#btnFinalBeneficiarySave').on('click', this.SaveFinalBeneficiary);
        //$("#DocumentNumberFb").on('keypress', PartnersParam.OnlyNumbers);
        //$('#selectDocumentTypeFb').on('itemSelected', FinalBeneficiary.ChangeBeneficiaryTypeAndNumberDocument);
        $('#tableParticipants').on('rowEdit', this.EditFinalBeneficiary);
        $('#tableParticipants').on('rowDelete', this.DeleteFinalBeneficiary);
    }

    static InitialFinalBeneficiary() {

        SarlaftRequest.GetTypeDocument().done(function (data) {
            if (data.success) {
                $("#selectDocumentTypeFb").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
        
       

    }

    static GetIndividualPeps() {
        //Peps.ControlFieds();
    }

    static DisabledControles() {
        
    
    }

    static EnabledControles() {

      

      
    }

    static ControlFieds() {
           
      
    }

    static AddFinalBeneficiary() {
        $('#formFinalBeneficiary').validate();
        

        if ($('#formFinalBeneficiary').valid()) {

            var DataFinalBeneficiary = {};

            DataFinalBeneficiary.DocumentTypeId = $('#selectDocumentTypeFb').UifSelect('getSelected');
            DataFinalBeneficiary.IdCardNumero = $('#DocumentNumberFb').val();
            DataFinalBeneficiary.TradeName = $('#nameFinalBeneficiary').val();
            
            
            if (FinalBeneficiary.VerifyDuplicate(DataFinalBeneficiary)) {

                if (glbIndexBeneficiary == null) {
                    $('#tableParticipants').UifDataTable('addRow', DataFinalBeneficiary);
                }
                else {
                    $('#tableParticipants').UifDataTable('editRow', DataFinalBeneficiary, glbIndexBeneficiary);
                }
                FinalBeneficiary.ClearData();
            }
            else
                $.UifNotify('show', { 'type': 'success ', 'message': "Beneficiario final repetido.", 'autoclose': true });
        }
        $('#tableParticipants tbody td').find('.div-button-single').children('.select-button').hide();
    }

   static  VerifyDuplicate(FinalBeneficiary) {
        var temps = $('#tableParticipants').UifDataTable('getData');
       var trafficlight = true;
       temps.every(item => {
           if (item.DocumentTypeId == FinalBeneficiary.DocumentTypeId && FinalBeneficiary.IdCardNumero == item.IdCardNumero && glbIndexBeneficiary == null)
            {
                trafficlight = false;
            }
        });
       return trafficlight; 
    }

    SaveFinalBeneficiary() {
        tableFinalBeneficiary= $('#tableParticipants').UifDataTable('getData');
        $.UifNotify('show', { 'type': 'success ', 'message': Resources.Language.MessageInformation, 'autoclose': true });
        $('#modalFinalBeneficiary').UifModal('hide');
    }

    EditFinalBeneficiary(event, finalBeneficiary, index) {
        glbIndexBeneficiary = index;
        $('#selectDocumentTypeFb').UifSelect('setSelected', finalBeneficiary.DocumentTypeId);
        $('#DocumentNumberFb').val(finalBeneficiary.IdCardNumero);
        $('#nameFinalBeneficiary').val(finalBeneficiary.TradeName);
    }
   
    DeleteFinalBeneficiary(event, finalBeneficiary, index) {
        $.UifDialog('confirm', {
            'message': Resources.Language.DeleteBenefeciary,
            'title': Resources.LabelFinalBeneficiary
        }, function (result) {
            if (result) { 
                $('#tableParticipants').UifDataTable('deleteRow', index);
            }
        });
    }
    
    static ClearData() {
        $('#nameFinalBeneficiary').val("");
        $('#DocumentNumberFb').val("");
        $('#selectDocumentTypeFb').UifSelect('setSelected', null);
        glbIndexBeneficiary = null;
    }

    static DisabledForm(disabled) {

        $('#nameFinalBeneficiary').prop("disabled", disabled);
        $('#DocumentNumberFb').prop("disabled", disabled);
        $('#selectDocumentTypeFb').UifSelect("disabled", disabled);
     
   }

    static ChangeBeneficiaryTypeAndNumberDocument() {
        if ($("#selectDocumentTypeFb").UifSelect("getSelectedSource").IsAlphanumeric) {
            $('#DocumentNumberFb').val("");
            $('#DocumentNumberFb').ValidatorKey(null, 0, 0);
        } else {
            $('#DocumentNumberFb').val("");
            var regex = /^\d+$/;
            if (!regex.exec($('#DocumentNumberFb').val())) {
                $('#DocumentNumberFb').val("");
            }
            $("#DocumentNumberFb").off('keypress').OnlyDecimals(0);
        }
    }
    
  }


     

    