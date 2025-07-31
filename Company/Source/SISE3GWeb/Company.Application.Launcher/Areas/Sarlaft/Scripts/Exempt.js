var gblLink = {};
var linkStatus = null;
var individualId = null;
class Exempt extends Uif2.Page {
    getInitialState() {

        //Links.GetRelationship();
        //Links.GetLinkTypeBeneficiary();
        //Links.GetLinkTypeInsured();
        //Links.GetLinkTypeInsuredBeneficiary();
        Exempt.InitialLinks();

    }

    //Seccion Eventos
    bindEvents() {

        
        $('#btnExemptSave').on('click', Exempt.ExemptSave);
        $('#chExenta').on('click', Exempt.ChangeExents);
        
        //$('#selectPolicyholderInsured').on('itemSelected', this.EnableOtherPolicyholderInsured);
        //$('#selectPolicyholderBeneficiary').on('itemSelected', this.EnableOtherPolicyholderBeneficiary);
        //$('#ExposedS').on('itemSelected', this.EnableOtherInsuredBeneficiary);
        //$('#selectTypeExemption').on('click', Exempt.ExecuteOperationsLink);
    }

    static InitialLinks() {
        
        Exempt.GetExemptTypeExemption();
        
        
    }

    static GetExemptTypeExemption() {

        SarlaftRequest.LoadInitialData(false).done(function (data) {
            if (data.success) {
                $("#selectTypeExemption").UifSelect({ sourceData: data.result.ExonerationTypes });
                
                
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
            
        });
        

    }

    static ExemptSave() {
        var sarlaftExonerationtDTO = [];

        if ($("#selectTypeExemption").UifSelect("getSelected") != "" )  {



            sarlaftExonerationtDTO = {
                IndividualId: gblIndivualId,
                ExonerationType: $("#selectTypeExemption").UifSelect("getSelected"),
                IsExonerated: $("#chExenta").prop("checked"),
                RegistrationDate: null,
                RoleId: 0

            };

            if (sarlaftExonerationtDTO.IsExonerated) {

                SarlaftRequest.SaveExoneration(sarlaftExonerationtDTO).done(function (data) {
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'success ', 'message': "Persona Exenta de Sarlaft", 'autoclose': true });
                        $("#btnNewSarlaft").prop("disabled", true);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            $('#modalExempt').UifModal('hide');
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe seleccionar un tipo de Exoneración", 'autoclose': true });
        }


        

    }

    static ChangeExents() {
        if ($("#chExenta").prop("checked")) {
            $("#selectTypeExemption").UifSelect("disabled", false);
        }
        else {
            $("#selectTypeExemption").UifSelect("disabled", true);
        }
    }

    static disableControles() {

        if (sarlaftExoneration.length == 0 ) {
            $("#selectTypeExemption").UifSelect("disabled", true);
        }
        else {
            $("#selectTypeExemption").UifSelect("disabled", false);
            $("#chExenta").prop("checked", true);
        }
        
    }
     

    
   

}
