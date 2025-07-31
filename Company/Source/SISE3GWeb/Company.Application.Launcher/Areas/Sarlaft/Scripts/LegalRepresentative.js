var countryId = null;
var stateId = null;
var LegalRepresent = {};
var SubstituteLegalRepresent = {};
var legalStatus = 0;
var stateForm = 1; // 1 Inicial - 2 Consulta - 3 Modificar  
var LegalRepresents = [];
class LegalRepresentative extends Uif2.Page {
    getInitialState() {
        LegalRepresentative.InitialLegal();
    }

    //Seccion Eventos
    bindEvents() {
        //Selects
        $('#MainSelectLegalCountry').on('itemSelected', this.GetStates);
        $('#MainSelectState').on('itemSelected', this.GetCities);
       // $('#SubstituteSelectLegalCountry').on('itemSelected', this.GetSubstituteStates);
       // $('#SubstituteSelectState').on('itemSelected', this.GetSubstituteCities);

        //Funcionalidad
        $("#btnLegalSave").on('click', LegalRepresentative.ExecuteOperationsRL);

        //Numeros
        //$("#MainDocumentNumber").on('keypress', LegalRepresentative.OnlyNumbers);
        //$("#SubstituteDocumentNumber").on('keypress', LegalRepresentative.OnlyNumbers);
        $("#MainPhone").on('keypress', LegalRepresentative.OnlyNumbers);
        $("#MainCell").on('keypress', LegalRepresentative.OnlyNumbers);
        $("#MainInputAuthoAmt").focusin(SarlaftParam.NotFormatMoneyIn);
        $("#MainInputAuthoAmt").focusout(SarlaftParam.FormatMoneyOut);
        //$("#SubstituteDocumentNumber").on('keypress', LegalRepresentative.OnlyNumbers);
        //$("#SubstitutePhone").on('keypress', LegalRepresentative.OnlyNumbers);
        //$("#SubstituteCell").on('keypress', LegalRepresentative.OnlyNumbers);
        //$("#SubstituteInputAuthoAmt").focusin(SarlaftParam.NotFormatMoneyIn);
        //$("#SubstituteInputAuthoAmt").focusout(SarlaftParam.FormatMoneyOut);

        //Letras Mayusculas
        $("#MainFullName").TextTransform(ValidatorType.UpperCase);
        $("#MainInputPlaceOfExpeditionRepresentLegal").TextTransform(ValidatorType.UpperCase);
        $("#MainBirthplace").TextTransform(ValidatorType.UpperCase);
        $("#MainNationality").TextTransform(ValidatorType.UpperCase);
        $("#Mainoffice").TextTransform(ValidatorType.UpperCase);
        $("#MainEmail").TextTransform(ValidatorType.UpperCase);
        $("#MainAddress").TextTransform(ValidatorType.UpperCase);
        $("#MainInputObservations").TextTransform(ValidatorType.UpperCase);

        $('#selectTypeOfOption').on('itemSelected', LegalRepresentative.selectTypeOfOption);
        //$('#MainLegalDocumentType').on('itemSelected', this.ChangeTypeAndNumberDocument);
        //$('#SubstituteLegalDocumentType').on('itemSelected', this.ChangeTypeAndNumberDocumentSubstitute);
        //$("#SubstituteFullName").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteInputPlaceOfExpeditionRepresentLegal").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteBirthplace").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteNationality").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteOffice").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteEmail").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteAddress").TextTransform(ValidatorType.UpperCase);
        //$("#SubstituteInputObservations").TextTransform(ValidatorType.UpperCase);
    }

    static InitialLegal() {
        SarlaftRequest.GetCountry().done(function (data) {
            if (data.success) {
                $('#MainSelectLegalCountry').UifSelect({ sourceData: data.result });
              //  $('#SubstituteSelectLegalCountry').UifSelect({ sourceData: data.result });
                var colombia = $.grep(data.result, function (item) { return item.Id === 1 });
                $('#MainSelectLegalCountry').UifSelect('setSelected', colombia[0].Id);
              //  $('#SubstituteSelectLegalCountry').UifSelect('setSelected', colombia[0].Id);
                LegalRepresentative.GetState(colombia[0].Id);
              //  LegalRepresentative.GetSubstituteState(colombia[0].Id)
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetCurrencies().done(function (data) {
            if (data.success) {
                $("#MainSelectLegalCurrency").UifSelect({ sourceData: data.result });
            //    $("#SubstituteSelectLegalCurrency").UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        SarlaftRequest.GetTypeDocument().done(function (data) {
            if (data.success) {
                $("#MainLegalDocumentType").UifSelect({ sourceData: data.result });
                $("#SubstituteLegalDocumentType").UifSelect({ sourceData: data.result });
            }

            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
        

        SarlaftRequest.GetNationality().done(function (data) {
            if (data.success) {
                $("#MainNationality").UifSelect({ sourceData: data.result });
                $("#selectNationalityOtherLr").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        $("#selectTypeOfOption").UifSelect({ sourceData: [{ "Id": "1", "Description": "Principal - Suplente" }, { "Id": "2", "Description": "Suplente - Principal" }] });
        $('#selectTypeOfOption').UifSelect("disabled", true);
        
    }

    GetStates(event, selectedItem) {
        countryId = selectedItem.Id;
        LegalRepresentative.GetState(countryId);
    }

    GetSubstituteStates(event, selectedItem) {
        countryId = selectedItem.Id;
        LegalRepresentative.GetSubstituteState(countryId);
    }

    static GetState(countryId) {
        SarlaftRequest.GetState(countryId).done(function (data) {
            if (data.success) {
                $('#MainSelectState').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetSubstituteState(countryId) {
        SarlaftRequest.GetState(countryId).done(function (data) {
            if (data.success) {
                $('#SubstituteSelectState').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    GetCities(event, selectedItem) {
        countryId = $('#MainSelectLegalCountry').val();
        stateId = selectedItem.Id;

        SarlaftRequest.GetCities(countryId, stateId).done(function (data) {
            if (data.success) {
                $('#MainSelectTown').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    //GetSubstituteCities(event, selectedItem) {
    //    countryId = $('#SubstituteSelectLegalCountry').val();
    //    stateId = selectedItem.Id;

    //    SarlaftRequest.GetCities(countryId, stateId).done(function (data) {
    //        if (data.success) {
    //            $('#SubstituteSelectTown').UifSelect({ sourceData: data.result });
    //        }
    //        else {
    //            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
    //        }
    //    })
    //}

    ChangeTypeAndNumberDocument() {
        if ($("#MainLegalDocumentType").UifSelect("getSelectedSource").IsAlphanumeric) {
            $('#MainDocumentNumber').val("");
            $('#MainDocumentNumber').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        } else {
            $('#MainDocumentNumber').val("");
            var regex = /^\d+$/;
            if (!regex.exec($('#MainDocumentNumber').val())) {
                $('#MainDocumentNumber').val("");
            }
            $("#MainDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        }
    }

    ChangeTypeAndNumberDocumentSubstitute() {
        if ($("#SubstituteLegalDocumentType").UifSelect("getSelectedSource").IsAlphanumeric) {
            $('#SubstituteDocumentNumber').val("");
            $('#SubstituteDocumentNumber').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        } else {
            $('#SubstituteDocumentNumber').val("");
            var regex = /^\d+$/;
            if (!regex.exec($('#SubstituteDocumentNumber').val())) {
                $('#SubstituteDocumentNumber').val("");
            }
            $("#SubstituteDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        }
    }
 

    static ClearFields() {
        $("#MainFullName").val("");
        $("#MainExpeditionDate").val("");
        $("#MainInputPlaceOfExpeditionRepresentLegal").val("");
        $("#MainBirthdate").val("");
        $("#MainBirthplace").val("");
        $("#MainSelectTown").val("");
        $("#MainSelectState").val("");
        $("#MainSelectLegalCountry").val("");
        $("#MainPhone").val("");
        $("#Mainoffice").val("");
        $("#MainCell").val("");
        $("#MainEmail").val("");
        $("#MainNationality").val("");
        $("#MainInputAuthoAmt").val("");
        $("#MainSelectLegalCurrency").val("");
        $("#MainInputObservations").val("");
        $("#MainDocumentNumber").val("");
        $("#MainLegalDocumentType").val("");
        $("#selectNationalityOtherLr").val("");
        legalStatus = 0;
    }

    static ClearSubstituteFields() {
        $("#SubstituteFullName").val("");
        $("#SubstituteExpeditionDate").val("");
        $("#SubstituteInputPlaceOfExpeditionRepresentLegal").val("");
        $("#SubstituteBirthdate").val("");
        $("#SubstituteBirthplace").val("");
        $("#SubstituteSelectTown").val("");
        $("#SubstituteSelectState").val("");
        $("#SubstituteSelectLegalCountry").val("");
        $("#SubstitutePhone").val("");
        $("#Substituteoffice").val("");
        $("#SubstituteCell").val("");
        $("#SubstituteEmail").val("");
        $("#SubstituteNationality").val("");
        $("#SubstituteInputAuthoAmt").val("");
        $("#SubstituteSelectLegalCurrency").val("");
        $("#SubstituteInputObservations").val("");
        $("#SubstituteDocumentNumber").val("");
        $("#SubstituteLegalDocumentType").val("");
    }

    static Validate() {

        var msj = "";

        if ($("#MainLegalDocumentType").UifSelect("getSelected") == null || $("#MainLegalDocumentType").UifSelect("getSelected") == "") {
            msj += AppResources.LabelTypeDocument + "<br>"
        }
        if ($("#MainDocumentNumber").val() == null || $("#MainDocumentNumber").val() == "") {
            msj += AppResources.LabelNumberDocument + "<br>"
        }
        if ($("#MainFullName").val() == null || $("#MainFullName").val() == "") {
            msj += AppResources.LabelFullName + "<br>"
        }
        if ($("#MainExpeditionDate").val() == null || $("#MainExpeditionDate").val() == "") {
            msj += AppResources.LabelExpeditionDate + "<br>"
        }
        if ($("#MainInputPlaceOfExpeditionRepresentLegal").val() == null || $("#MainInputPlaceOfExpeditionRepresentLegal").val() == "") {
            msj += AppResources.ExpeditionPlace + "<br>"
        }
        if ($("#MainBirthdate").val() == null || $("#MainBirthdate").val() == "") {
            msj += AppResources.Birthdate + "<br>"
        }
        
        if ($("#MainBirthplace").val() == null || $("#MainBirthplace").val() == "") {
            msj += AppResources.Birthplace + "<br>"
        }
        if ($("#MainNationality").val() == null || $("#MainNationality").val() == "") {
            msj += AppResources.LabelNationality + "<br>"
        }
        if ($("#selectNationalityOtherLr").val() == null || $("#selectNationalityOtherLr").val() == "") {
            msj += AppResources.LabelNationality + " 2 <br>"
        }
        if ($("#MainOffice").val() == null || $("#MainOffice").val() == "") {
            msj += AppResources.LabelOffice + "<br>"
        }
        if ($("#MainPhone").val() == null || $("#MainPhone").val() == "") {
            msj += AppResources.Phone + "<br>"
        }
        if ($("#MainSelectLegalCountry").UifSelect("getSelected") == null || $("#MainSelectLegalCountry").UifSelect("getSelected") == "") {
            msj += AppResources.Country + "<br>"
        }
        if ($("#MainSelectState").UifSelect("getSelected") == null || $("#MainSelectState").UifSelect("getSelected") == "") {
            msj += AppResources.LabelState + "<br>"
        }
        if ($("#MainSelectTown").UifSelect("getSelected") == null || $("#MainSelectTown").UifSelect("getSelected") == "") {
            msj += AppResources.Town + "<br>"
        }
        if ($("#MainEmail").val() == "" || $("#MainEmail").val() == null) {
            msj += AppResources.AdressEmail + "<br>"
        }
        if ($("#MainEmail").val() != "" && !ValidateEmail($('#MainEmail').val())) {
            msj += AppResources.AdressEmail + "<br>"
        }
        if ($("#MainAddress").val() == null || $("#MainAddress").val() == "") {
            msj += AppResources.Address + "<br>"
        }
        
        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.LegalRepresentativeNotFilledOut, 'autoclose': true });
            //$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }

        return true;
    }

    static GetLegalRepresent() {
        glblValidateLegalRepresent = false;
        if (gblSarlaft.LegalRepresentDTO !== null && gblSarlaft.LegalRepresentDTO !== undefined && gblSarlaft.LegalRepresentDTO.length > 0) {
            LegalRepresentative.EditLegalRepresent(gblSarlaft.LegalRepresentDTO);
            LegalRepresentative.TypeOfOption();
            legalStatus = gblSarlaft.LegalRepresentDTO[0].Status;
        }
        else {
            var legalRepresentSarlaftId = 0;
            if (newSarlaftId != null) {
                legalRepresentSarlaftId = newSarlaftId
            }
            else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
                legalRepresentSarlaftId = gblSarlaft.SarlaftDTO.Id;
            }
            else {
                legalRepresentSarlaftId = $('#SarlaftId').val();
            }
            SarlaftRequest.GetLegalRepresentByIndividualId(gblIndivualId, legalRepresentSarlaftId).done(function (data) {
                if (data.success) {
                    LegalRepresent = data.result;
                    if (LegalRepresent.length > 0) {
                        LegalRepresentative.EditLegalRepresent(LegalRepresent);
                    }
                }
            });
        }
    }

   

    static ExecuteOperationsRL() {
        if (LegalRepresentative.stateForm == 1 || gblSarlaft.LegalRepresentDTO.length == 0) {
            LegalRepresents = [];
            var Principal = 1;
            var Alterno = 2;
            if (!$("#chkPrincipal").is(":checked")) { Principal = 2; Alterno = 1 }

            if ($("#formLegalRepresentative").valid()) {
                LegalRepresent = {
                    IndividualId: gblIndivualId,
                    LegalRepresentativeName: $("#MainFullName").val(),
                    ExpeditionDate: $("#MainExpeditionDate").val(),
                    ExpeditionPlace: $("#MainInputPlaceOfExpeditionRepresentLegal").val(),
                    BirthDate: $("#MainBirthdate").val(),
                    BirthPlace: $("#MainBirthplace").val(),
                    City: $("#MainSelectTown").UifSelect("getSelectedText"),
                    CityId: $("#MainSelectTown").UifSelect("getSelected"),
                    StateId: $("#MainSelectState").UifSelect("getSelected"),
                    CountryId: $("#MainSelectLegalCountry").UifSelect("getSelected"),
                    Phone: $("#MainPhone").val(),
                    JobTitle: $("#MainOffice").val(),
                    CellPhone: $("#MainCell").val(),
                    Email: $("#MainEmail").val(),
                    Address: $("#MainAddress").val(),
                    //Nationality: UifSelect("getSelected"),
                    AuthorizationAmount: NotFormatMoney($("#MainInputAuthoAmt").val()),
                    CurrencyId: $("#MainSelectLegalCurrency").UifSelect("getSelected"),
                    Description: $("#MainInputObservations").val(),
                    IdCardNo: $("#MainDocumentNumber").val(),
                    IdCardTypeCode: $("#MainLegalDocumentType").UifSelect("getSelected"),
                    Status: legalStatus,
                    //IsMain: $("#MainLegalRepresentative").is(":checked"),
                    IsMain: true,
                    NationalityOtherType: $("#selectNationalityOtherLr").UifSelect("getSelected"),
                    NationalityType: $("#MainNationality").UifSelect("getSelected"),
                    LegalRepresentType: Principal
                };
                if (legalStatus != 0) {
                    if (legalStatus == 1) {
                        if (newSarlaftId == null) {
                            LegalRepresent.Status = 3;
                        }
                        else {
                            LegalRepresent.Status = 2;
                        }
                    }
                    else {
                        LegalRepresent.Status = legalStatus;
                    }
                } else if (legalStatus == 0) {
                    LegalRepresent.Status = 2;
                }


                LegalRepresents.push(LegalRepresent);
            }
            

            if ($("#formLegalRepresentative").valid()) {
                SubstituteLegalRepresent = {
                    IndividualId: gblIndivualId,
                    LegalRepresentativeName: $("#SubstituteFullName").val(),
                    IdCardNo: $("#SubstituteDocumentNumber").val(),
                    IdCardTypeCode: $("#SubstituteLegalDocumentType").UifSelect("getSelected"),
                    LegalRepresentType: Alterno,
                    IsMain: false,
                    Status: legalStatus
                };
                if (legalStatus != 0) {
                    if (legalStatus == 1) {
                        if (newSarlaftId == null) {
                            SubstituteLegalRepresent.Status = 3;
                        }
                        else {
                            SubstituteLegalRepresent.Status = 2;
                        }
                    }
                    else {
                        SubstituteLegalRepresent.Status = legalStatus;
                    }
                } else if (legalStatus == 0) {
                    SubstituteLegalRepresent.Status = 2;
                }

                LegalRepresents.push(SubstituteLegalRepresent);
            }
            if (CompareDates($("#MainExpeditionDate").val(), $("#MainBirthdate").val()) == false) {
                $.UifNotify('show', { 'type': 'danger', 'message': 'Fecha de expedición debe ser mayor a nacimiento', 'autoclose': true });
                return false;
            }


            if (LegalRepresents.length == 2)
                LegalRepresentative.SaveLegalRepresent(LegalRepresents);

        }
        else
            if (LegalRepresentative.stateForm == 3) {
                if (LegalRepresentative.ValidateSubstitute() && LegalRepresentative.Validate())
                {
                    var indice = $('#selectTypeOfOption').UifSelect("getSelected");

                    if ($('#chkPrincipal').is(":checked") && indice == 2)
                        LegalRepresentative.SetDataModify(gblSarlaft.LegalRepresentDTO[1], gblSarlaft.LegalRepresentDTO[0]);
                    else
                    if ($('#chkPrincipal').is(":checked") && indice == 1)
                            LegalRepresentative.SetDataModify(gblSarlaft.LegalRepresentDTO[0], gblSarlaft.LegalRepresentDTO[1]);
                    else
                    if ($('#chkAlternate').is(":checked") && indice == 1)
                                LegalRepresentative.SetDataModify(gblSarlaft.LegalRepresentDTO[1], gblSarlaft.LegalRepresentDTO[0]);
                    else
                    if ($('#chkAlternate').is(":checked") && indice == 2)
                                    LegalRepresentative.SetDataModify(gblSarlaft.LegalRepresentDTO[0], gblSarlaft.LegalRepresentDTO[1]);

                    $.each(gblSarlaft.LegalRepresentDTO, function (index, value) {
                        value.Status = 3
                    });
                    LegalRepresentative.SaveLegalRepresent(gblSarlaft.LegalRepresentDTO);
                }
            }
        
    }
    static SaveLegalRepresent(respRepresentant) {
        gblSarlaft.LegalRepresentDTO = respRepresentant;
        $.UifNotify('show', { 'type': 'success ', 'message': AppResources.SaveLegalRepresentativeSuccessfully, 'autoclose': true });
        $('#modalRepresentlegal').UifModal('hide');
    }

    static EditLegalRepresent(data) {
        if (data !== null) {
            gblSarlaft.LegalRepresentDTO = data;
        }
        if (gblSarlaft.LegalRepresentDTO.length == 1)
        {
            LegalRepresentative.SetLegalRepresent(gblSarlaft.LegalRepresentDTO[0]);
        }
        else
        $.each(gblSarlaft.LegalRepresentDTO, function (index, value) {
            if (value.IsMain) {
                LegalRepresentative.SetLegalRepresent(value);
            }
            else {
            
                SubstituteLegalRepresent.IndividualId = value.IndividualId;
                $("#SubstituteFullName").val(value.LegalRepresentativeName);
            
                $("#SubstituteDocumentNumber").val(value.IdCardNo);
                $("#SubstituteLegalDocumentType").UifSelect("setSelected", value.IdCardTypeCode);
            
            } 

        });

    }

    static DisabledLegalRepresent() {
        $("#MainFullName").prop("disabled", true);
        $('#MainExpeditionDate').prop("disabled", true);
        $('#MainInputPlaceOfExpeditionRepresentLegal').prop("disabled", true);
        $('#MainBirthdate').prop("disabled", true);
        $('#MainBirthplace').prop("disabled", true);
        $('#MainPhone').prop("disabled", true);
        $('#MainOffice').prop("disabled", true);
        $('#MainCell').prop("disabled", true);
        $('#MainEmail').prop("disabled", true);
        $('#MainAddress').prop("disabled", true);
        $('#MainNationality').prop("disabled", true);
        $('#MainInputAuthoAmt').prop("disabled", true);
        $('#MainInputObservations').prop("disabled", true);
        $('#MainDocumentNumber').prop("disabled", true);
        $('#MainSelectLegalCurrency').UifSelect("disabled", true);
        $('#MainLegalDocumentType').UifSelect("disabled", true);
        $('#MainSelectLegalCountry').UifSelect("disabled", true);
        $('#MainSelectState').UifSelect("disabled", true);
        $('#selectNationalityOtherLr').UifSelect("disabled", true);
        $('#chkPrincipal').prop("disabled", true);
        $('#chkAlternate').prop("disabled", true);
        $('#selectTypeOfOption').UifSelect("disabled", true);
        
    }

    static DisabledSubstituteLegalRepresent() {
        $("#SubstituteFullName").prop("disabled", true);
        $('#SubstituteExpeditionDate').prop("disabled", true);
        $('#SubstituteInputPlaceOfExpeditionRepresentLegal').prop("disabled", true);
        $('#SubstituteBirthdate').prop("disabled", true);
        $('#SubstituteBirthplace').prop("disabled", true);
        $('#SubstitutePhone').prop("disabled", true);
        $('#SubstituteOffice').prop("disabled", true);
        $('#SubstituteCell').prop("disabled", true);
        $('#SubstituteEmail').prop("disabled", true);
        $('#SubstituteAddress').prop("disabled", true);
        $('#SubstituteNationality').prop("disabled", true);
        $('#SubstituteInputAuthoAmt').prop("disabled", true);
        $('#SubstituteInputObservations').prop("disabled", true);
        $('#SubstituteDocumentNumber').prop("disabled", true);

        
        //$('#SubstituteSelectLegalCurrency').UifSelect("disabled", true);
        $('#SubstituteLegalDocumentType').UifSelect("disabled", true);
        //$('#SubstituteSelectLegalCountry').UifSelect("disabled", true);
        $("#btnLegalSave").hide();
        SarlaftParam.DisabledSarlaft();
        $('#chkPrincipal').prop("disabled", true);
        $('#chkAlternate').prop("disabled", true);
    }

    static EnabledLegalRepresent() { 
        $('#MainLegalDocumentType').UifSelect("disabled", false);
        $("#MainFullName").prop("disabled", false);
        $('#MainExpeditionDate').prop("disabled", false);
        $('#MainInputPlaceOfExpeditionRepresentLegal').prop("disabled", false);
        $('#MainBirthdate').prop("disabled", false);
        $('#MainBirthplace').prop("disabled", false);
        $('#MainPhone').prop("disabled", false);
        $('#MainOffice').prop("disabled", false);
        $('#MainCell').prop("disabled", false);
        $('#MainEmail').prop("disabled", false);
        $('#MainAddress').prop("disabled", false);
        $('#MainNationality').prop("disabled", false);
        $('#MainInputAuthoAmt').prop("disabled", false);
        $('#MainDocumentNumber').prop("disabled", false);
        $('#MainInputObservations').prop("disabled", false);
        $('#MainSelectLegalCurrency').UifSelect("disabled", false);
        $("#selectNationalityOtherLr").UifSelect("disabled", false);
        $("#MainNationality").UifSelect("disabled", false);
        $("#MainSelectLegalCountry").UifSelect("disabled", false);
        $("#MainSelectState").UifSelect("disabled", false);
        $("#MainSelectTown").UifSelect("disabled", false);

        $("#select1").UifSelect("disabled", false);
        $("#select2").UifSelect("disabled", false);
        $("#select3").UifSelect("disabled", false);

        $("#SubstituteDocumentNumber").prop("disabled", false);
        $("#SubstituteFullName").prop("disabled", false);
        $("#SubstituteLegalDocumentType").UifSelect("disabled", false);
        $('#chkPrincipal').prop("disabled", false);
        $('#chkAlternate').prop("disabled", false);
        
        

        
        $("#btnLegalSave").show();
    }

    static OnlyNumbers(event) {
        if (event.keyCode !== 8 && event.keyCode !== 46 && event.keyCode !== 37 && event.keyCode !== 39 && event.keyCode !== 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    static ValidateSubstitute() {

        var msj = "";

        if ($("#SubstituteFullName").val() == null || $("#SubstituteFullName").val() == "") {
            msj += AppResources.LabelFullName + "<br>"
        }

        //if ($("#SubstituteExpeditionDate").val() == null || $("#SubstituteExpeditionDate").val() == "") {
        //    msj = AppResources.LabelExpeditionDate + "<br>"
        //}

        //if ($("#SubstituteBirthdate").val() == null || $("#SubstituteBirthdate").val() == "") {
        //    msj = AppResources.Birthdate + "<br>"
        //}

        //if ($("#SubstituteBirthplace").val() == null || $("#SubstituteBirthplace").val() == "") {
        //    msj = AppResources.Birthplace + "<br>"
        //}

        //if ($("#SubstituteSelectTown").UifSelect("getSelected") == null || $("#SubstituteSelectTown").UifSelect("getSelected") == "") {
        //    msj = AppResources.Town + "<br>"
        //}

        //if ($("#SubstituteSelectState").UifSelect("getSelected") == null || $("#SubstituteSelectState").UifSelect("getSelected") == "") {
        //    msj = AppResources.LabelState + "<br>"
        //}

        //if ($("#SubstituteSelectLegalCountry").UifSelect("getSelected") == null || $("#SubstituteSelectLegalCountry").UifSelect("getSelected") == "") {
        //    msj = AppResources.Country + "<br>"
        //}
        //if ($("#SubstitutePhone").val() == null || $("#SubstitutePhone").val() == "") {
        //    msj = AppResources.Phone + "<br>"
        //}
        //if ($("#SubstituteOffice").val() == null || $("#SubstituteOffice").val() == "") {
        //    msj = AppResources.LabelOffice + "<br>"
        //}
        //if ($("#SubstituteAddress").val() == null || $("#SubstituteAddress").val() == "") {
        //    msj = AppResources.Address + "<br>"
        //}
        //if ($("#SubstituteNationality").val() == null || $("#SubstituteNationality").val() == "") {
        //    msj = AppResources.LabelNationality + "<br>"
        //}
        if ($("#SubstituteDocumentNumber").val() == null || $("#SubstituteDocumentNumber").val() == "") {
            msj += AppResources.LabelNumberDocument + "<br>"
        }
        if ($("#SubstituteLegalDocumentType").UifSelect("getSelected") == null || $("#SubstituteLegalDocumentType").UifSelect("getSelected") == "") {
            msj += AppResources.LabelTypeDocument + "<br>"
        }

        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.LegalRepresentativeAdditionalNotFilledOut, 'autoclose': true });
            //$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }

        return true;
    }


    static TypeOfOption() {

        if ($('#chkAlternate').is(":checked")) {
            $('#selectTypeOfOption').UifSelect("setSelected", 2);
        }
        else {
            $('#selectTypeOfOption').UifSelect("setSelected", 1);
        }

        $('#selectTypeOfOption').UifSelect("disabled", false);
    }


    static selectTypeOfOption() {


        if ($("#selectTypeOfOption").UifSelect("getSelected") != null || $("#selectTypeOfOption").UifSelect("getSelected") != "") {

            var indice = $('#selectTypeOfOption').UifSelect("getSelected");

            if ($('#chkPrincipal').is(":checked") && indice == 2 && gblSarlaft.LegalRepresentDTO != null && gblSarlaft.LegalRepresentDTO.length > 0)
               LegalRepresentative.FillData(gblSarlaft.LegalRepresentDTO[1], gblSarlaft.LegalRepresentDTO[0])
             else
                if ($('#chkPrincipal').is(":checked") && indice == 1 && gblSarlaft.LegalRepresentDTO != null && gblSarlaft.LegalRepresentDTO.length > 0)
                   LegalRepresentative.FillData(gblSarlaft.LegalRepresentDTO[0], gblSarlaft.LegalRepresentDTO[1])
                else
                    if ($('#chkAlternate').is(":checked") && indice == 1 && gblSarlaft.LegalRepresentDTO != null && gblSarlaft.LegalRepresentDTO.length > 0)
                        LegalRepresentative.FillData(gblSarlaft.LegalRepresentDTO[1], gblSarlaft.LegalRepresentDTO[0])
                    else
                        if ($('#chkAlternate').is(":checked") && indice == 2 && gblSarlaft.LegalRepresentDTO != null && gblSarlaft.LegalRepresentDTO.length > 0)
                            LegalRepresentative.FillData(gblSarlaft.LegalRepresentDTO[0], gblSarlaft.LegalRepresentDTO[1])
            
                
        }
       

    }

    static FillData(LegalRepresentDTO_P, LegalRepresentDTO_S)
    {

        $("#MainFullName").val(LegalRepresentDTO_P.LegalRepresentativeName);
        if (LegalRepresentDTO_P.IdCardNo != null) {
            $("#MainExpeditionDate").val(FormatDate(LegalRepresentDTO_P.ExpeditionDate));
            $("#MainBirthdate").val(FormatDate(LegalRepresentDTO_P.BirthDate));
        }
        $("#MainInputPlaceOfExpeditionRepresentLegal").val(LegalRepresentDTO_P.ExpeditionPlace);
        $("#MainBirthplace").val(LegalRepresentDTO_P.BirthPlace);
        $("#MainPhone").val(LegalRepresentDTO_P.Phone);
        $("#MainOffice").val(LegalRepresentDTO_P.JobTitle);
        $("#MainCell").val(LegalRepresentDTO_P.CellPhone);
        $("#MainEmail").val(LegalRepresentDTO_P.Email);
        $("#MainAddress").val(LegalRepresentDTO_P.Address);
        $("#MainNationality").val(LegalRepresentDTO_P.Nationality);
        $("#MainInputAuthoAmt").val(LegalRepresentDTO_P.AuthorizationAmount);
        $("#MainSelectLegalCurrency").UifSelect("setSelected", LegalRepresentDTO_P.CurrencyId);
        $("#MainInputObservations").val(LegalRepresentDTO_P.Description);
        $("#MainDocumentNumber").val(LegalRepresentDTO_P.IdCardNo);
        $("#MainLegalDocumentType").UifSelect("setSelected", LegalRepresentDTO_P.IdCardTypeCode);
        $("#MainSelectLegalCountry").UifSelect("setSelected", LegalRepresentDTO_P.CountryId);
        $("#MainSelectState").UifSelect("setSelected", LegalRepresentDTO_P.StateId);
        $("#MainSelectTown").UifSelect("setSelected", LegalRepresentDTO_P.CityId);
        
        
        $("#selectNationalityOtherLr").UifSelect("setSelected", LegalRepresentDTO_P.NationalityOtherType);
        $("#MainNationality").UifSelect("setSelected", LegalRepresentDTO_P.NationalityType);




        $("#SubstituteFullName").val(LegalRepresentDTO_S.LegalRepresentativeName);
        $("#SubstituteDocumentNumber").val(LegalRepresentDTO_S.IdCardNo);
        $("#SubstituteLegalDocumentType").UifSelect("setSelected", LegalRepresentDTO_S.IdCardTypeCode);

    }

    static SetDataModify(Legal, SubstituteLegal) {


        if (LegalRepresentative.Validate()) {
            
            Legal.IndividualId = gblIndivualId;
            Legal.LegalRepresentativeName= $("#MainFullName").val();
            Legal.ExpeditionDate= $("#MainExpeditionDate").val();
            Legal.ExpeditionPlace= $("#MainInputPlaceOfExpeditionRepresentLegal").val();
            Legal.BirthDate= $("#MainBirthdate").val();
            Legal.BirthPlace= $("#MainBirthplace").val();
            Legal.City= $("#MainSelectTown").UifSelect("getSelectedText");
            Legal.CityId= $("#MainSelectTown").UifSelect("getSelected");
            Legal.StateId= $("#MainSelectState").UifSelect("getSelected");
            Legal.CountryId= $("#MainSelectLegalCountry").UifSelect("getSelected");
            Legal.Phone= $("#MainPhone").val();
            Legal.JobTitle= $("#MainOffice").val();
            Legal.CellPhone= $("#MainCell").val();
            Legal.Email= $("#MainEmail").val();
            Legal.Address= $("#MainAddress").val();
            Legal.AuthorizationAmount= NotFormatMoney($("#MainInputAuthoAmt").val());
            Legal.CurrencyId= $("#MainSelectLegalCurrency").UifSelect("getSelected");
            Legal.Description= $("#MainInputObservations").val();
            Legal.IdCardNo= $("#MainDocumentNumber").val();
            Legal.IdCardTypeCode= $("#MainLegalDocumentType").UifSelect("getSelected");
            Legal.NationalityOtherType= $("#selectNationalityOtherLr").UifSelect("getSelected");
            Legal.NationalityType= $("#MainNationality").UifSelect("getSelected");
           
         }

        if (LegalRepresentative.ValidateSubstitute()) {
            
                
            SubstituteLegal.LegalRepresentativeName= $("#SubstituteFullName").val();
            SubstituteLegal.IdCardNo = $("#SubstituteDocumentNumber").val();
                SubstituteLegal.IdCardTypeCode = $("#SubstituteLegalDocumentType").UifSelect("getSelected");
                
            
        }
    }

    static SetLegalRepresent(value)
    {
        LegalRepresent.IndividualId = value.IndividualId;
        $("#MainFullName").val(value.LegalRepresentativeName);
        if (value.IdCardNo != null) {
            $("#MainExpeditionDate").val(FormatDate(value.ExpeditionDate));
            $("#MainBirthdate").val(FormatDate(value.BirthDate));
        }
        $("#MainInputPlaceOfExpeditionRepresentLegal").val(value.ExpeditionPlace);
        $("#MainBirthplace").val(value.BirthPlace);
        $("#MainPhone").val(value.Phone);
        $("#MainOffice").val(value.JobTitle);
        $("#MainCell").val(value.CellPhone);
        $("#MainEmail").val(value.Email);
        $("#MainAddress").val(value.Address);
        $("#MainNationality").val(value.Nationality);
        $("#MainInputAuthoAmt").val(value.AuthorizationAmount);
        $("#MainSelectLegalCurrency").UifSelect("setSelected", value.CurrencyId);
        $("#MainInputObservations").val(value.Description);
        $("#MainDocumentNumber").val(value.IdCardNo);
        $("#MainLegalDocumentType").UifSelect("setSelected", value.IdCardTypeCode);
        $("#MainSelectLegalCountry").UifSelect("setSelected", value.CountryId);
        $("#selectNationalityOtherLr").UifSelect("setSelected", value.NationalityOtherType);
        $("#MainNationality").UifSelect("setSelected", value.NationalityType);
        if (value.LegalRepresentType == 1) {
            $('#chkPrincipal').prop("checked", true);
            $('#chkAlternate').prop("checked", false);
        }

        else {
            $('#chkPrincipal').prop("checked", false);
            $('#chkAlternate').prop("checked", true);
        }

        SarlaftRequest.GetState(value.CountryId).done(function (result) {
            if (result.success) {
                $('#MainSelectState').UifSelect({ sourceData: result.result, enable: false });
                $("#MainSelectState").UifSelect("setSelected", value.StateId);
                SarlaftRequest.GetCities(value.CountryId, value.StateId).done(function (result2) {
                    if (result2.success) {
                        $("#MainSelectTown").UifSelect({ sourceData: result2.result, enable: false });
                        $("#MainSelectTown").UifSelect("setSelected", value.CityId);
                    }
                });
            }
        });
    }
}