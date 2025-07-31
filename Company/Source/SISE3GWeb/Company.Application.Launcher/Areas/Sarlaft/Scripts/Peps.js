
class Peps extends Uif2.Page {
    getInitialState() {
      
        Peps.InitialLegal();
        Peps.ClearData();
        Peps.DisabledForm(false);
    }

    //Seccion Eventos
    bindEvents() {

        $("#inputJobOffice").TextTransform(ValidatorType.UpperCase);

        $("#btnPEPSSave").on('click', Peps.SavePeps);
        $("#chkExposedS").on('click', Peps.ControlFieds);
        $("#chkExposedN").on('click', Peps.ControlFieds);
    }

    static InitialLegal() {

        SarlaftRequest.GetCategoria().done(function (data) {
            if (data.success) {
                $("#Category").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetConsanguinidad().done(function (data) {
            if (data.success) {
                $("#Affinity").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetRelacion().done(function (data) {
            if (data.success) {
                $("#Link").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });


        $("#Unlinked").UifSelect({ sourceData: [{ "Id": "1", "Description": "Vinculado" }, { "Id": "2", "Description": "Desvinculado" }]});
        


       

    }

    static GetIndividualPeps() {
        glbValidatePeps = false;
        //Peps.ControlFieds();
        if (gblSarlaft.PepsDTO == undefined) {
            var pepsSarlaftId = 0;
            if (newSarlaftId != null) {
                pepsSarlaftId = newSarlaftId
            }
            else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
                pepsSarlaftId = gblSarlaft.SarlaftDTO.Id;
            }
            else {
                pepsSarlaftId = $('#SarlaftId').val();
            }
            SarlaftRequest.GetPepsByIndividualId(gblIndivualId, pepsSarlaftId).done(function (data) {
                if (data.success) {
                    if (data.result.Exposed || data.result.Exposed == null) {
                        glbValidatePeps = true;
                        $("#chkExposedN").prop("checked", false);
                        $("#chkExposedS").prop("checked", true);
                        Peps.EnabledControles();
                    }
                    else {
                        $("#chkExposedN").prop("checked", true);
                        $("#chkExposedS").prop("checked", false);
                    }
                    $("#Affinity").UifSelect('setSelected', data.result.Affinity);
                    $("#Category").UifSelect('setSelected', data.result.Category);
                    $("#Link").UifSelect('setSelected', data.result.Link);
                    $("#Entit").val(data.result.Entity);
                    $("#Observations").val(data.result.Observations);
                    $("#NamePeps").val(data.result.Trade_Name);
                    $("#Unlinked").UifSelect('setSelected', data.result.Unlinked);
                    $("#Unlinked_DATE").val(FormatDate(data.result.Unlinked_DATE));
                    $("#inputJobOffice").val(data.result.JobOffice);
                }
            });
        }
        if ($("#chkExposedS").prop("checked")) {
            Peps.EnabledControles();
        }
        else if ($("#chkExposedN").prop("checked")) {
            Peps.DisabledControles();
        }
    }

    static DisabledControles() {
        
        $("#Category").UifSelect("disabled", true);
        $("#Link").UifSelect("disabled", true);
        $("#Affinity").UifSelect("disabled", true);
        $("#Unlinked").UifSelect("disabled", true);
        $("#Unlinked_DATE").attr("disabled", true);
        $("#NamePeps").prop("disabled", true);
        $("#Entit").prop("disabled", true);
        $("#Observations").prop("disabled", true);
        $("#inputJobOffice").prop("disabled", true);

        //$("#btnPEPSSave").hide();
    }

    static EnabledControles() {

      

        $("#Category").UifSelect("disabled", false);
        $("#Link").UifSelect("disabled", false);
        $("#Affinity").UifSelect("disabled", false);
        $("#Unlinked").UifSelect("disabled", false);
        $("#Unlinked_DATE").attr("disabled", false);
        $("#NamePeps").prop("disabled", false);
        $("#Entit").prop("disabled", false);
        $("#Observations").prop("disabled", false);
        $("#inputJobOffice").prop("disabled", false);

        $("#btnPEPSSave").show();
    }

    static ControlFieds() {
           
        if ($("#chkExposedS").is(":checked"))
            Peps.EnabledControles();
        else
            Peps.DisabledControles();
    }

    static SavePeps() {
        $('#formPeps').validate();
        if (Peps.Validate()) 
        if ($('#formPeps').valid()) {


            var PEPS = [];
            var modelPeps = $("#formPeps").serializeObject();

            // if (PEPS.Validete()) {
            PEPS = {
                Individual_Id: parseInt(gblIndivualId),
                Exposed: $("#chkExposedS").is(":checked") ? 1 : 0,
                Category: parseInt($("#Category").UifSelect("getSelected")),
                Link: parseInt($("#Link").UifSelect("getSelected")),
                Affinity: parseInt($("#Affinity").UifSelect("getSelected")),
                Unlinked: parseInt($("#Unlinked").UifSelect("getSelected")),

                Unlinked_DATE: $("#Unlinked_DATE").val(),

                Trade_Name: $("#NamePeps").val(),
                Entity: $("#Entit").val(),
                Observations: $("#Observations").val(),
                JobOffice: $("#inputJobOffice").val()


            };

            gblSarlaft.PepsDTO = PEPS;
            glbValidatePeps = false;
            $.UifNotify('show', { 'type': 'success ', 'message': AppResources.SavePepsSuccessfully, 'autoclose': true });
            $('#modalPeps').UifModal('hide');
        }
    }


    static EnabledPeps() {
        $("#btnPEPSSave").show();
    }

    
    static ClearData() {
        if ($("#chkExposedS").is(':checked')==false) {
            $("#chkExposedS").prop("checked", false);
            $("#chkExposedN").prop("checked", true);
            $("#Category").UifSelect('setSelected', "");
            $("#Link").UifSelect('setSelected', "");
            $("#Affinity").UifSelect('setSelected', "");
            $("#Unlinked").UifSelect('setSelected', "");
            $("#Unlinked_DATE").val("");
            $("#NamePeps").val("");
            $("#Entit").val("");
            $("#Observations").val("");
            $("#inputJobOffice").val("");
           
        }
        Peps.ControlFieds();
    }

    static ClearControls() {
            $("#chkExposedS").prop("checked", true);
            $("#chkExposedN").prop("checked", false);
            $("#Category").UifSelect('setSelected', "");
            $("#Link").UifSelect('setSelected', "");
            $("#Affinity").UifSelect('setSelected', "");
            $("#Unlinked").UifSelect('setSelected', "");
            $("#Unlinked_DATE").val("");
            $("#NamePeps").val("");
            $("#Entit").val("");
            $("#Observations").val("");
            $("#inputJobOffice").val("");
    }

    static DisabledForm(disabled) {
        $("#chkExposedN").prop("disabled", disabled);
        $("#chkExposedS").prop("disabled", disabled);
   }

    static Validate() {

        var msj = "";

        if (($("#Unlinked").UifSelect("getSelected") != null || $("#Unlinked").UifSelect("getSelected") != "") && $("#Unlinked").UifSelect("getSelected") =="2" ) {
            if ($("#Unlinked_DATE").val() == null || $("#Unlinked_DATE").val() == "")
            {
                msj = AppResources.LabelDateUnlinked + "<br>"
            }
        }

      
        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }

        return true;
    }
    
  }


     

    