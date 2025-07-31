var glbindividualId = 0;
var glbuserId = 0;
var glbnumForm = 0;
var gblSarlaft = {};
var mainEconomicActivity = {};
var secondaryEconomicActivity = {};
var gblIndivualId;
var gblDocumentNum;
var pndEvents;
var gblPerson;
var typePerson;
var sarlaftIsNew = null;
var isFirstTime = false;
var sarlaftExoneration = [];
var glblValidateLegalRepresent = false;
var glblValidatePartner = false;
var glbValidatePeps = false;
var newSarlaftId = null;

class SarlaftParam extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        SarlaftParam.LoadSarlaft();
        pndEvents = false;
        $("#btnSaveSarlaft").hide();
        $("#btnConsultSarlaft").hide();
        $("#btnNewSarlaft").hide();
        $("#btnEditSarlaft").hide();
        $("#btnExentoSarlaft").hide();
        $("#AdressEmail").ValidatorKey(ValidatorType.Emails, 1, 1);
        SarlaftParam.Heritage();
    }

    //Seccion Eventos
    bindEvents() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputDescription").TextTransform(ValidatorType.UpperCase);
        $('#btnLinks').on('click', this.ViewLinks);
        $('#btnLinks').on('click', Links.GetIndividualLinks);

        $('#btnPEPS').on('click', this.ViewPeps);
        $('#btnPEPS').on('click', Peps.GetIndividualPeps);

        $('#btnRepresentlegal').on('click', this.ViewRepresentlegal);
        $('#btnRepresentlegal').on('click', LegalRepresentative.GetLegalRepresent);
        $('#btnTitlePartner').on('click', this.ViewTitlePartner);
        $('#btnTitlePartner').on('click', PartnersParam.GetPartners);
        $('#btnInternationalOperations').on('click', SarlaftParam.ViewInternationalOperations);
        $('#btnInternationalOperations').on('click', InternationalOperations.GetInternationalOperations);
        $('#btnSaveSarlaft').on('click', this.SaveSarlaft);
        $('#btnNewSarlaft').on('click', SarlaftParam.NewSarlaft);
        $('#btnConsultSarlaft').on('click', this.GetSarlaft);
        $('#btnExitSarlaft').on('click', this.ExitSarlaft);
        $('#btnExentoSarlaft').on('click', this.ExemptSarlaft);
        $('#btnExentoSarlaft').on('click', Exempt.disableControles);
        $('#inputSearch').on('buttonClick', SarlaftParam.SearchSarlaft);
        $("#btnEditSarlaft").on("click", this.EditSarlaft);
        $("#inputDiligenceDate").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputConstitutionDate").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputVerificationDate").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputInterviewDate").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#selectResult").on('itemSelected', this.BlockDescription);

        $('#selectCIIUCodeMainActivity').on("buttonClick", this.SearchCIIUCodeMainActivity);
        $("#selectSecondaryActivity").on('buttonClick', this.SearchCIIUCodeSecondActivity);

        //$('#inputSearch').ValidatorKey(ValidatorType.Number, 1, 1);
        $("#inputIncome").OnlyDecimals(2);
        $("#inputOtherIncome").OnlyDecimals(2);
        $("#inputTotalAssets").OnlyDecimals(2);
        $("#inputExpenses").OnlyDecimals(2);
        $("#inputTotalLiabilities").OnlyDecimals(2);
        $("#inputIncome").focusin(SarlaftParam.NotFormatMoneyIn);
        $("#inputIncome").focusout(SarlaftParam.FormatMoneyOut);
        $("#inputOtherIncome").focusin(SarlaftParam.NotFormatMoneyIn);
        $("#inputOtherIncome").focusout(SarlaftParam.FormatMoneyOut);
        $("#inputTotalAssets").focusin(SarlaftParam.NotFormatMoneyIn);
        $("#inputTotalAssets").focusout(SarlaftParam.FormatMoneyOut);
        $("#inputExpenses").focusin(SarlaftParam.NotFormatMoneyIn);
        $("#inputExpenses").focusout(SarlaftParam.FormatMoneyOut);
        $("#inputTotalLiabilities").focusin(SarlaftParam.NotFormatMoneyIn);
        $("#inputTotalLiabilities").focusout(SarlaftParam.FormatMoneyOut);
        $("#inputTotalAssets").focusout(this.ValidateAssets);
        $("#inputTotalLiabilities").focusout(this.ValidateAssets);
        $("#ckeckExtraIncome").on("change", SarlaftParam.ValidateExtraIncome);
        $("#updateCIIU").on("change", SarlaftParam.UpdateCIIU);
        $('#selectSState').on('itemSelected', this.ChangeStates);
        $('#selectSCountry').on('itemSelected', this.ChangeCountries);

        $('#inputInterviewManager').on("blur", this.SearchInterviewManager);

        $('#tblResult tbody').on('click', 'tr', function (e) {

            var data = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            var input = $("#hdnTypeTblResult").val();
            $(`#${input}`).val(data.Description);
            $('#modalListSearch').UifModal('hide');

            switch (input) {
                case "selectCIIUCodeMainActivity":
                    mainEconomicActivity = {
                        Id: parseInt(data.Description.split("-")[0]),
                        Description: data.Description
                    };
                    break;

                case "selectSecondaryActivity":
                    secondaryEconomicActivity = {
                        Id: parseInt(data.Description.split("-")[0]),
                        Description: data.Description
                    };
                    SarlaftParam.RepeatedActivity();
                    break;
            }
        });
    }

    static LoadSarlaft() {
        SarlaftParam.loadCountries();
        SarlaftParam.GetPersonTypes();
        SarlaftParam.GetBranch();
        SarlaftParam.GetTypeDocument();
        SarlaftParam.GetInterviewResults();
        var fecha = new Date();
        $('#inputSignYear').val(fecha.getFullYear());

        var output = GetCurrentFromDate;

        $('#inputVerificationDate').val(output);

        SarlaftRequest.GetUserByUserId().done(function (data) {
            if (data.success) {

                $("#inputUserName").val(data.result.Name);
                $("#inputUserId").val(data.result.UserId);
                $("#selectSignatureBranch").UifSelect('setSelected', data.result.BranchId);
                $("#inputFormNumber").val(data.result.FormNum);

                glbuserId = data.result.UserId;
                glbnumForm = data.result.FormNum;
            } else {
                SarlaftParam.DisabledSarlaft();
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetUsers, 'autoclose': true });

            }
        });

        SarlaftParam.GetExemptTypeExemption();
        SarlaftParam.GetNationality();
        SarlaftParam.GetSocietyType();
        SarlaftParam.GetOppositorType();
        SarlaftParam.GetNationality();
        SarlaftParam.GetSocietyType();
        SarlaftParam.GetOppositorType();
        $('#inputVerificationDate').UifDatepicker('disabled', true);

    }

    static FillUserInformation() {
        SarlaftRequest.GetUserByUserId().done(function (data) {
            if (data.success) {
                $("#inputUserName").val(data.result.Name);
                $("#inputUserId").val(data.result.UserId);
                $("#selectSignatureBranch").UifSelect('setSelected', data.result.BranchId);
                $("#inputFormNumber").val(data.result.FormNum);
                glbnumForm = data.result.FormNum;

            } else {
                SarlaftParam.DisabledSarlaft();
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetUsers, 'autoclose': true });

            }
        });
    }
    static GetPersonTypes() {
        SarlaftRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $('#selectSearchPersonType').UifSelect({ sourceData: data.result });
                $('#selectPersonType').UifSelect({ sourceData: data.result });
                if (GetQueryParameter("TypePerson") !== undefined && GetQueryParameter("DocumentNum") !== undefined) {
                    $("#selectSearchPersonType").UifSelect("setSelected", GetQueryParameter("TypePerson"));
                    $("#inputSearch").val(GetQueryParameter("DocumentNum"));
                    SarlaftParam.SearchSarlaft();
                    SarlaftParam.GetSarlaft();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetInterviewResults() {
        SarlaftRequest.GetInterviewResult().done(function (data) {
            if (data.success) {
                $('#selectResult').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetBranch() {
        SarlaftRequest.GetBranch().done(function (data) {
            if (data.success) {
                $('#selectSignatureBranch').UifSelect({ sourceData: data.result });
                $('#selectSignatureBranch').UifSelect("disabled", true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetTypeDocument() {
        SarlaftRequest.GetTypeDocument().done(function (data) {
            if (data.success) {
                $('#selectTypeDocument').UifSelect({ sourceData: data.result });
                $('#selectTypeDocument').UifSelect("disabled", true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })

    }

    ViewLinks() {
        $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
    }

    ViewPeps() {
        $('#modalPeps').UifModal('showLocal', Resources.Language.Peps);
    }

    ViewRepresentlegal() {
        $('#modalRepresentlegal').UifModal('showLocal', Resources.Language.Representlegal);
    }

    ViewTitlePartner() {
        $('#modalPartner').UifModal('showLocal', Resources.Language.TitlePartner);
    }

    static ViewInternationalOperations() {
        var auxSarlaftId;
        if (sarlaftId == null) {
            SarlaftRequest.GetLastSarlaftId(gblPerson).done(function (data) {
                if (data.success) {
                    auxSarlaftId = data.result.Id;
                    var internationalOperations = $('#rdInternationalOperations').is(':checked');

                    if (internationalOperations == true) {
                        $('#modalInternationalOperations').UifModal('showLocal', Resources.Language.InternationalOperations);
                        InternationalOperations.GetSarlaftId();

                        gblSarlaft.SarlaftDTO = data.result;
                        gblSarlaft.SarlaftDTO.Id = null;
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': "No seleccionó operaciones Internacionales", 'autoclose': true });
                    }
                }
            });
        }
        else {
            auxSarlaftId = sarlaftId
            var internationalOperations = $('#rdInternationalOperations').is(':checked');

            if (internationalOperations == true) {
                $('#modalInternationalOperations').UifModal('showLocal', Resources.Language.InternationalOperations);
                InternationalOperations.GetSarlaftId();
            }
        }
        //var internationalOperations = gblSarlaft.SarlaftDTO.InternationalOperations;

    }
    static LoadUniqueSarlaft(dataSarlaft) {
        sarlaftExoneration = [];
        SarlaftParam.ClearData();
        gblPerson = dataSarlaft[0];
        if (gblPerson.PersonType !== 1) {
            $('#inputCompanyDigit').val(" - " + Shared.CalculateDigitVerify(gblPerson.DocumentNumber));
        } else {
            $('#inputCompanyDigit').hide()
        }

        var individualId = gblPerson.IndividualId;
        glbindividualId = individualId;
        SarlaftRequest.GetSarlaftExonerationByIndividualId(individualId).done(function (response) {
            if (response.success) {
                if (gblPerson.PersonType == 1) {
                    $('#liLinks').show(); $('#liRepresentlegal').hide(); $('#liTitlePartner').hide(); $('#liTitlePEPS').show();
                }
                else {
                    $('#liLinks').show(); $('#liRepresentlegal').show(); $('#liTitlePartner').show(); $('#liTitlePEPS').show();
                }
                $("#IndividualId").val(gblPerson.IndividualId);
                gblIndivualId = $("#IndividualId").val();
                $("#selectTypeDocument").UifSelect('setSelected', gblPerson.DocumentTypeId);
                $("#inputNumDocument").val(gblPerson.DocumentNumber);
                $("#inputName").val(gblPerson.Name);
                $("#selectCIIUCodeMainActivity").val(gblPerson.EconomicActivityId +" - "+ gblPerson.EconomicActivityDesc);

                sarlaftExoneration = response.result;

                SarlaftRequest.GetSarlaft(individualId).done(function (data2) {
                    if (data2.success) {

                        SarlaftRequest.ValidationAccessAndHierarchysByUser().done(function (data) {
                            if (data.success) {
                                $('#tblSarlaft').UifDataTable('clear');
                                if (data2.result.length > 0) {
                                    var lastRegistrationDate = FormatDate(data2.result[0].RegistrationDate);
                                    var currentDate = GetCurrentFromDate();
                                    var finalDate = AddToDate(lastRegistrationDate, 0, 0, 1);
                                    var finalDateArray = finalDate.toString().split(DateSplit);

                                    if (CompareDates(currentDate, finalDate) == 1) {
                                        if (!sarlaftExoneration.length == 0 && !sarlaftExoneration[0].IsExonerated == false) {
                                            $.UifDialog('alert', {
                                                title: 'Mensaje de información',
                                                message: 'La contraparte es un caso de exento, diligenciar la información conforme al procedimiento'
                                            });
                                        }
                                        $.UifNotify('show', { 'type': 'info', 'message': 'El formulario con vigencia hasta el año' + " " + finalDateArray[2] + " " + 'venció, se debe diligenciar uno nuevo', 'autoclose': true });
                                    }

                                    $('#FormQuery').show();
                                    $('#FormMain').hide();
                                    $('#SarlaftMenu').hide();

                                    $.each(data2.result, function (key, value) {
                                        $('#tblSarlaft').UifDataTable("addRow", {
                                            Id: this.Id,
                                            Year: this.Year,
                                            BranchName: this.BranchName,
                                            FillingDate: FormatDate(this.RegistrationDate),
                                            FormNum: this.FormNum
                                        });
                                    });

                                    if (GetQueryParameter("SarlaftId") !== undefined && !isFirstTime) {
                                        isFirstTime = true;
                                        SarlaftParam.GetSarlaft(GetQueryParameter("SarlaftId"));
                                    } else {
                                        SarlaftParam.GetSarlaft();
                                    }
                                } else {
                                    $('#FormQuery').show();
                                    $('#FormMain').hide();
                                    $('#SarlaftMenu').hide();
                                    $.each(data2.result, function (key, value) {
                                        $('#tblSarlaft').UifDataTable("addRow", {
                                            Id: this.Id,
                                            Year: this.Year,
                                            BranchName: this.BranchName,
                                            FillingDate: FormatDate(this.RegistrationDate),
                                            FormNum: this.FormNum
                                        });
                                    });
                                }
                                $("#labelnamesarlaft").text(gblPerson.Name);
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                });


            }
            //Asignar tipo de persona
            typePerson = $('#selectSearchPersonType').val();
            $("#btnConsultSarlaft").show();
            $("#btnNewSarlaft").show();

            $("#btnNewSarlaft").prop("disabled", false);

            $("#btnEditSarlaft").show();
            if (typePerson == IndividualTypePerson.Legal) {
                $("#LegalFields").show();
                $(".NaturalFields").hide();
            }
            else {
                $("#LegalFields").hide();
                $(".NaturalFields").show();
            }

        });
    }
    static SearchSarlaft() {
        if ($('#selectSearchPersonType').val() != "") {
            if ($('#inputSearch').val() != "") {
                $("#btnNewSarlaft").hide();
                $("#btnExentoSarlaft").hide();
                var searchType = $('#selectSearchPersonType').val();
                var documentNum = $('#inputSearch').val();
                gblDocumentNum = documentNum;
                newSarlaftId = null;
                //Consulta la persona por numero de documento
                SarlaftRequest.GetPersonByDocumentNumberAndSearchType(documentNum, searchType).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            SarlaftParam.LoadUniqueSarlaft(data.result);
                        } else if (data.result.length > 1) {
                            ObjectSarlaftSearch.LoadSarlaftSimple(data.result);
                            dropDownSearchSarlaft.show();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true });
                            $('#labelnamesarlaft').text("");

                            SarlaftParam.ClearData();
                            $('#tblSarlaft').UifDataTable('clear');
                            $('#SarlaftMenu').hide();
                            $('#FormMain').hide();
                            $("#btnSaveSarlaft").hide();
                            $("#btnConsultSarlaft").hide();
                            $("#btnEditSarlaft").hide();
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        SarlaftParam.ClearData();
                        $('#tblSarlaft').UifDataTable('clear');
                        $('#SarlaftMenu').hide();
                        $('#FormMain').hide();
                        $("#btnSaveSarlaft").hide();
                        $("#btnConsultSarlaft").hide();
                        $("#btnEditSarlaft").hide();
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterTextToSearch, 'autoclose': true });
                return false;
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectTypePerson, 'autoclose': true });
            return false;
        }
    }

    static ObtainSarlaftData(id) {
        return new Promise((resolve, reject) => {
            SarlaftRequest.GetSarlaftBySarlaftId(id).done(function (data) {
                if (data.success) {
                    if (data.result.SarlaftDTO.PendingEvent) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingEvents, 'autoclose': true });
                    }
                    resolve(data.result);
                }
                else {
                    reject(data.result);
                }
                reject(Resources.Language.ErrorGetSarlaft);
            });
        });
    }

    static ObtainLastSarlaftData(gblPerson) {
        return new Promise((resolve, reject) => {
            SarlaftRequest.GetLastSarlaftId(gblPerson).done(function (data) {
                if (data.success) {
                    if (data.result.IsAuthorizationRequest) {
                        SarlaftParam.ValidateAuthorizationRequest(data.result);
                        resolve(null);
                    }
                    resolve(data.result);
                }
                reject(Resources.Language.ErrorGetSarlaft);
            });
        });
    }

    static ValidateRepresentant(LegalRepresentant) {
        var events = [];
        var messageErr = "";
        return new Promise((resolve, reject) => {
            if (LegalRepresentant !== null && LegalRepresentant !== undefined) {
                SarlaftRequest.GetvalidationRepresentant(LegalRepresent).done(function (data) {
                    //solo queremos lista de eventos
                    if (data.success) {
                        if (Array.isArray(data.result)) {
                            //añadir eventos del representante
                            $.each(data.result, function (key, value) {
                                events.push(data.result[key]);
                            });
                        }
                    }
                    else {
                        messageErr = data.result;
                    }
                });
            }
            else {
                resolve(true);
            }

            //Devolver mensaje de error o se muestran eventos si los hay     
            if (messageErr !== "") {
                resolve(messageErr);
            }
            if (events.length <= 0) {
                resolve(true);
            }
            else {
                resolve(events);//retorna eventos
            }
        });
    }

    static ValidatePartners(Partners) {
        var events = [];
        var messageErr = "";
        return new Promise((resolve, reject) => {
            setTimeout(() => resolve(1), 1000);
            if (Partners !== null && Partners !== undefined) {
                SarlaftRequest.GetValidationListPartners(Partners).done(function (data) {
                    //solo lista de eventos
                    if (data.success) {
                        if (Array.isArray(data.result)) {
                            //añadir eventos de los Asociados
                            $.each(data.result, function (key, value) {
                                events.push(data.result);
                            });
                        }
                    } else {
                        messageErr = data.result;
                    }
                });
            }
            //Devolver mensaje de error o se muestran eventos si los hay
            if (messageErr !== "") {
                reject(messageErr);
            }
            if (events.length > 0) {
                resolve(events);
            }
            else {
                resolve(true);
            }
        });
    }

    static CheckPerson() {
        return new Promise((resolve, reject) => {
            if (typePerson == IndividualTypePerson.Legal && $("#selectExemptionType").UifSelect("getSelected") != "1") {
                if (gblSarlaft.LegalRepresentDTO === null || gblSarlaft.LegalRepresentDTO === undefined) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Debe añadir un representante legal", 'autoclose': true });
                    resolve(false);
                } else if (newSarlaftId != null && gblSarlaft.LegalRepresentDTO.some(x => x.Status != ParametrizationStatus.Create)) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Debe añadir un representante legal", 'autoclose': true });
                    resolve(false);
                }
                if (gblSarlaft.PartnerDTO === null || gblSarlaft.PartnerDTO === undefined || gblSarlaft.PartnerDTO.length <= 0 || !(gblSarlaft.PartnerDTO.some(x => x.Status != ParametrizationStatus.Delete))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Debe añadir minimo un accionista/asociado", 'autoclose': true });
                    resolve(false);
                } else if (newSarlaftId != null && gblSarlaft.PartnerDTO.some(x => x.Status != ParametrizationStatus.Create)) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Debe añadir minimo un accionista/asociado", 'autoclose': true });
                    resolve(false);
                }
                if (glblValidatePartner) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Debe validar la informacion de accionista/asociado", 'autoclose': true });
                    resolve(false);
                }
                if (glblValidateLegalRepresent) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Debe validar la informacion de representante legal", 'autoclose': true });
                    resolve(false);
                }
                resolve(true);
            }
            else {
                resolve(true);
            }
        });
    }

    SaveSarlaft() {
        var active = false;
        var LoadEconomicActivity = false;
        var LoadmainEconomicActivity = {};
        var description;
        if ($('#formSarlaft').valid()) {
            //Valida para persona juridica-> perifericos 
            SarlaftParam.CheckPerson().then((resp) => {
                if (resp == true) {
                    if (typePerson == IndividualTypePerson.Legal) {
                        LegalRepresentative.GetLegalRepresent();
                        if ($("#selectExemptionType").UifSelect("getSelected") != "" && $("#selectExemptionType").UifSelect("getSelected") == 0) {
                            if (!SarlaftParam.ValidateLegalRepresent() || !LegalRepresentative.Validate() || !LegalRepresentative.ValidateSubstitute()) {
                                $('#modalRepresentlegal').UifModal('showLocal', Resources.Language.Representlegal);
                                return;
                            }
                        }
                    }

                    if ($("#AdressEmail").val() != "" && !ValidateEmail($('#AdressEmail').val())) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorEmail, 'autoclose': true });
                        return;
                    }

                    //Validacion montos informacion financiera
                    if ($("#selectExemptionType").UifSelect("getSelected") != "1")
                        if (parseFloat($("#inputIncome").val().replaceAll(".", "").replaceAll(",", "")) == 0 ||
                            parseFloat($("#inputExpenses").val().replaceAll(".", "").replaceAll(",", "")) == 0 ||
                            parseFloat($("#inputTotalAssets").val().replaceAll(".", "").replaceAll(",", "")) == 0 ||
                            parseFloat($("#inputTotalLiabilities").val().replaceAll(".", "").replaceAll(",", "")) == 0) {

                            if (gblPerson.AssociationType < 2) {
                                $.UifNotify('show', { 'type': 'danger', 'message': "La información financiera debe ser superior a 0", 'autoclose': true });
                                return;
                            }
                        }

                    if ($('#selectOppositorType').UifMultiSelect('getSelected') == null || $('#selectOppositorType').UifMultiSelect('getSelected') == "") {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + Resources.Language.OppositorType, 'autoclose': true });
                        return;
                    }
                    //Validamos que venga cargado la EconomicActivity
                    if ((mainEconomicActivity == null || mainEconomicActivity.Description == null) && gblPerson.EconomicActivityDesc != null) {
                        LoadEconomicActivity = true;
                        LoadmainEconomicActivity = $("#selectCIIUCodeMainActivity").val();
                        var LOADmain = LoadmainEconomicActivity.split("-");
                        mainEconomicActivity = { "Id": LOADmain[0], "Description": LoadmainEconomicActivity }

                    }
                    if (mainEconomicActivity != null || mainEconomicActivity.Description != null)
                        mainEconomicActivity.Description = SarlaftParam.ValidateEconomicActivity(mainEconomicActivity.Description);
                    var main = mainEconomicActivity.Description.split("-");
                    main = main[1];
                    main = main.replace(/ /g, "");
                    //Separa actividad principal por código y descripción
                    var CIIUMainActivity = $("#selectCIIUCodeMainActivity").val().replace(/ /g, "")
                    var CIIUMainActivityDesc = CIIUMainActivity.split("-");


                    var second = "";
                    var CIIUSecondActivity = "";
                    var CIIUSecondActivityDesc = "";

                    if ($("#ckeckExtraIncome").prop("checked")) {
                        secondaryEconomicActivity.Description = $("#selectSecondaryActivity").val();
                        if (secondaryEconomicActivity != null && secondaryEconomicActivity.Description != null) {
                            second = secondaryEconomicActivity.Description.split("-");
                            secondaryEconomicActivity = { "Id": second[0], "Description": second[1] }
                            second = second[1];
                            second = second.replace(/ /g, "");
                        }
                        CIIUSecondActivity = $("#selectSecondaryActivity").val().replace(/ /g, "")
                        CIIUSecondActivityDesc = CIIUSecondActivity.split("-");
                    }


                    if (($("#chkExposedS").is(":checked")) && !SarlaftParam.ValidatePeps() && $("#selectExemptionType").UifSelect("getSelected") == 0) {
                        return;
                    }

                    if ($('#rdInternationalOperations').is(':checked')) {
                        var validateInternationalOperations = false;
                        if (gblSarlaft.InternationalOperationDTO != undefined) {
                            if (gblSarlaft.InternationalOperationDTO.length > 0 && InternationalOperations.Validate()) {
                                if (gblSarlaft.InternationalOperationDTO.find(x => x.Status == 3 || x.Status == 2 || x.Status == 1) == undefined) {
                                    validateInternationalOperations = true;
                                }
                            }
                            else {
                                validateInternationalOperations = true;
                            }
                        }
                        else {
                            validateInternationalOperations = true;
                        }

                        active = true;
                        var intOperations = 0;
                        ($("#OperationType").UifSelect("getSelected") != null && $("#OperationType").UifSelect("getSelected") != "") ? intOperations++ : 0;
                        ($("#ProductType").UifSelect("getSelected") != null && $("#ProductType").UifSelect("getSelected") != "") ? intOperations++ : 0;
                        $("#ProductNumber").val() != "" ? intOperations++ : 0;
                        $("#Entity").val() != "" ? intOperations++ : 0;
                        $("#ProductAmount").val() != "" ? intOperations++ : 0;
                        ($("#Currency").UifSelect("getSelected") != null && $("#Currency").UifSelect("getSelected") != "") ? intOperations++ : 0;
                        ($("#CountryOrigin").UifSelect("getSelected") != null && $("#CountryOrigin").UifSelect("getSelected") != "") ? intOperations++ : 0;
                        ($("#State").UifSelect("getSelected") != null && $("#State").UifSelect("getSelected") != "") ? intOperations++ : 0;
                        ($("#Town").UifSelect("getSelected") != null && $("#Town").UifSelect("getSelected") != "") ? intOperations++ : 0;
                        //Valida si existen operaciones internacionales en el objeto global
                        //if (gblSarlaft.InternationalOperationDTO.length == 1) {
                        //    gblSarlaft.InternationalOperationDTO = gblSarlaft.InternationalOperationDTO[0];
                        //}
                        if ((validateInternationalOperations || intOperations != 9) && ($("#selectExemptionType").UifSelect("getSelected") != "" && $("#selectExemptionType").UifSelect("getSelected") == 0)) {
                            //SarlaftParam.ViewInternationalOperations();
                            $.UifNotify('show', { 'type': 'danger', 'message': "Operaciones Internacionales Pendientes", 'autoclose': true });
                            return;
                        }
                        else {
                            if ($('#selectSignatureBranch').val() == "") {
                                $.UifNotify('show', { 'type': 'danger', 'message': "debe seleccionar una sucursal valida", 'autoclose': true });
                                return;
                            }
                            if ($('#inputDescription').val() === null || $('#inputDescription').val() === "") {
                                description = $("#selectResult").UifSelect("getSelectedText");
                            } else {
                                description = $('#inputDescription').val();
                            }

                            //Validación de las actividades económicas
                            if (mainEconomicActivity.Id === null || mainEconomicActivity.Id === undefined) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMainEconomicActivity, 'autoclose': true });
                            }

                            else if (CIIUMainActivityDesc[1] !== main) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMainEconomicActivity, 'autoclose': true });
                            }
                            else if ($("#ckeckExtraIncome").prop("checked")) {


                                if ($("#inputOtherIncome").val() == 0 || $("#inputOtherIncome").val() == "") {
                                    $.UifNotify('show', { 'type': 'danger', 'message': "El valor de otros Ingresos debe ser superior a 0", 'autoclose': true });
                                    return;
                                }

                                if (secondaryEconomicActivity.Id === null || secondaryEconomicActivity.Id === undefined) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSecondEconomicActivity, 'autoclose': true });
                                }
                                else if (CIIUSecondActivityDesc[1] !== second) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSecondEconomicActivity, 'autoclose': true });
                                }
                                else {
                                    SarlaftParam.SaveSarlaftToDataBase(active, description);

                                }
                            }
                            else {
                                SarlaftParam.SaveSarlaftToDataBase(active, description);

                            }
                        }
                    }
                    else {
                        if ($('#inputDescription').val() === null || $('#inputDescription').val() === "") {
                            description = $("#selectResult").UifSelect("getSelectedText");
                        } else {
                            description = $('#inputDescription').val();
                        }

                        //Validación de las actividades económicas
                        if (mainEconomicActivity.Id === null || mainEconomicActivity.Id === undefined) {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMainEconomicActivity, 'autoclose': true });
                        }
                        else if (CIIUMainActivityDesc[1] !== main) {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMainEconomicActivity, 'autoclose': true });
                        }
                        else if ($("#ckeckExtraIncome").prop("checked")) {

                            if ($("#inputOtherIncome").val() == 0 || $("#inputOtherIncome").val() == "") {
                                $.UifNotify('show', { 'type': 'danger', 'message': "El valor de otros Ingresos debe ser superior a 0", 'autoclose': true });
                                return;
                            }

                            if (secondaryEconomicActivity.Id === null || secondaryEconomicActivity.Id === undefined) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSecondEconomicActivity, 'autoclose': true });
                            }
                            else if (CIIUSecondActivityDesc[1] !== second) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSecondEconomicActivity, 'autoclose': true });
                            }
                            else {
                                SarlaftParam.SaveSarlaftToDataBase(active, description);
                            }
                        }
                        else {
                            SarlaftParam.SaveSarlaftToDataBase(active, description);
                        }
                    }
                }
            });
        }
    }

    static ValidatePeps() {
        let isValid = true;

        if (glbValidatePeps) {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe validar la informacion de Peps", 'autoclose': true });
            isValid = false;
        } else if (gblSarlaft.PepsDTO == undefined && newSarlaftId != null) {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe validar la informacion de Peps", 'autoclose': true });
            isValid = false;
        }
        else {
            if ($("#chkExposedS").is(":checked")) {
                let Category = $("#Category").UifSelect("getSelected");
                let Link = $("#Link").UifSelect("getSelected");
                let Affinity = $("#Affinity").UifSelect("getSelected");
                let NamePeps = $("#NamePeps").val().trim();
                let Entit = $("#Entit").val().trim();
                let inputJobOffice = $("#inputJobOffice").val().trim();
                let Unlinked = $("#Unlinked").UifSelect("getSelected").trim();

                if (Category == "" || Link == "" || Affinity == "" || NamePeps == "" || Entit == "" || inputJobOffice == "" || Unlinked == "" || !Peps.Validate()) {
                    $('#modalPeps').UifModal('showLocal', Resources.Language.Peps);
                    setTimeout(() => {
                        $('#formPeps').validate();
                        $('#formPeps').valid();
                    }, 1000);

                    isValid = false;
                }
            }
        }
        return isValid;
    }

    static ValidateEconomicActivity(Descriptcion) {

        if (Descriptcion.indexOf("-") < 0) {
            if ($("#selectCIIUCodeMainActivity").val().length > 0)
                return $("#selectCIIUCodeMainActivity").val();
        }
        return Descriptcion;
    }

    static SaveSarlaftToDataBase(active, description) {
        Links.GetIndividualLinks();

        SarlaftParam.BuildSarlaft(active, description).then((withExtraInformation) => {
            if (!withExtraInformation) {
                if (gblSarlaft.CoSarlaftDTO.ExonerationTypeCode != "1") {
                    if (gblSarlaft.LinksDTO != null && gblSarlaft.LinksDTO.length > 0) {
                        if (gblSarlaft.LinksDTO.length != 3) {
                            $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
                            $.UifNotify('show', { 'type': 'danger', 'message': "Los vínculos son obligatorios", 'autoclose': true });
                            return false;
                        } else if (newSarlaftId != null && gblSarlaft.LinksDTO.some(x => x.Status != ParametrizationStatus.Create)) {
                            $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
                            $.UifNotify('show', { 'type': 'danger', 'message': "Los vínculos son obligatorios", 'autoclose': true });
                            return false;
                        }
                        else {
                            lockScreen();
                            SarlaftRequest.SaveSarlaft(gblSarlaft).done(function (data) {
                                if (data.success) {
                                    SarlaftParam.ValidateAuthorizationPolicies(data.result).then(() => {
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + "<br>" + Resources.Language.FormNumber + ": " + gblSarlaft.SarlaftDTO.FormNum, 'autoclose': true });
                                        SarlaftParam.SearchSarlaft();
                                        gblSarlaft = null;
                                    });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                                }
                                unlockScreen();
                            }).fail(() => unlockScreen());
                        }
                    }
                    else {
                        $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
                        $.UifNotify('show', { 'type': 'danger', 'message': "Los vínculos son obligatorios", 'autoclose': true });
                    }
                }
                else {
                    if (gblSarlaft.LinksDTO != null) {
                        if (gblSarlaft.LinksDTO.length > 0 && gblSarlaft.LinksDTO.length != 3) {
                            $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
                            $.UifNotify('show', { 'type': 'danger', 'message': "Los vínculos son obligatorios", 'autoclose': true });
                            return false;
                        } else if (newSarlaftId != null && gblSarlaft.LinksDTO.some(x => x.Status != ParametrizationStatus.Create)) {
                            $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
                            $.UifNotify('show', { 'type': 'danger', 'message': "Los vínculos son obligatorios", 'autoclose': true });
                            return false;
                        } else {
                            lockScreen();
                            SarlaftRequest.SaveSarlaft(gblSarlaft).done(function (data) {
                                if (data.success) {
                                    SarlaftParam.ValidateAuthorizationPolicies(data.result).then(() => {
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + "<br>" + Resources.Language.FormNumber + ": " + gblSarlaft.SarlaftDTO.FormNum, 'autoclose': true });
                                        SarlaftParam.SearchSarlaft();
                                        gblSarlaft = null;
                                    });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                                }
                                unlockScreen();
                            }).fail(() => unlockScreen());
                        }
                    } else {
                        $('#modalLinks').UifModal('showLocal', Resources.Language.Links);
                        $.UifNotify('show', { 'type': 'danger', 'message': "Los vínculos son obligatorios", 'autoclose': true });
                    }
                }
            }
            else {
                lockScreen();
                SarlaftRequest.SaveSarlaft(gblSarlaft).done(function (data) {
                    if (data.success) {
                        SarlaftParam.ValidateAuthorizationPolicies(data.result).then(() => {
                            gblSarlaft = data.result;
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + "<br>" + Resources.Language.FormNumber + ": " + gblSarlaft.SarlaftDTO.FormNum, 'autoclose': true });
                            gblSarlaft = null;
                            $('#FormQuery').show();
                            $('#FormMain').hide();
                            $('#SarlaftMenu').hide();
                        });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        gblSarlaft = null;
                    }
                    unlockScreen();
                }).fail(() => unlockScreen());
            }
        });
    }


    static ValidateAuthorizationPolicies(customerKnowledgeDto) {
        return new Promise((resolve, reject) => {
            var policyType = LaunchPolicies.ValidateInfringementPolicies(customerKnowledgeDto.InfringementPolicies, true);
            let countAuthorization = customerKnowledgeDto.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

            if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                if (countAuthorization > 0) {
                    LaunchPolicies.RenderViewAuthorizationPolicies(customerKnowledgeDto.InfringementPolicies, customerKnowledgeDto.OperationId, FunctionType.SarlaftGeneral);
                }
                reject();
            } else {
                resolve();
            }
        });
    }

    //Construye objeto sarlaft
    static BuildSarlaft(active, description) {
        return new Promise((resolve, reject) => {
            var financialSarlaft = {};
            var individualSarlaft = {};
            var coindividualSarlaft = {};

            individualSarlaft =
                {
                    Id: null,
                    TypePerson: $('#selectSearchPersonType').val(),
                    IndividualId: $('#IndividualId').val(),
                    Year: $('#inputSignYear').val(),
                    BranchId: $('#selectSignatureBranch').val(),
                    RegistrationDate: $('#inputDiligenceDate').val(),
                    TypeDocument: $('#selectTypeDocument').val(),
                    DocumentNumber: $('#inputNumDocument').val(),
                    Name: $('#inputName').val(),
                    FormNum: glbnumForm,
                    AuthorizedBy: $('#inputApprovedby').val(),
                    FillingDate: $('#inputConstitutionDate').val(),
                    UserId: glbuserId,
                    VerifyingEmployee: $('#inputVerifyingOfficial').val(),
                    CheckDate: $('#inputVerificationDate').val(),
                    InterviewDate: $('#inputInterviewDate').val(),
                    InterviewerName: $('#inputInterviewManager').val(),

                    InterviewPlace: $('#inputInterviewPlace').val(),
                    InterviewResultId: $('#selectResult').val(),
                    InternationalOperations: active,

                    EconomicActivityId: mainEconomicActivity.Id,
                    SecondEconomicActivityId: secondaryEconomicActivity.Id
                };

            coindividualSarlaft = [];
            coindividualSarlaft =
                {
                    sarlaftid: null,
                    individualid: individualSarlaft.IndividualId,
                    OppositorTypeCode: JSON.stringify($('#selectOppositorType').UifMultiSelect('getSelected')),
                    PersonTypeCode: $("#selectPersonType").UifSelect("getSelected"),
                    SocietyTypeCode: $("#selectSocietyType").UifSelect("getSelected"),
                    NationalityCode: $("#selectNationality").UifSelect("getSelected"),
                    NationalityOtherCode: $("#selectNationalityOther").UifSelect("getSelected"),
                    countryCode: $("#selectSCountry").UifSelect("getSelected"),
                    stateCode: $("#selectSState").UifSelect("getSelected"),
                    cityCode: $("#selectSCity").UifSelect("getSelected"),
                    email: $('#AdressEmail').val(),
                    heritage: $('#inputHeritage').val(),
                    Phone: $('#inputPhone').val(),
                    ExonerationTypeCode: $("#selectExemptionType").UifSelect("getSelected"),
                    MainAddressNatural: $('#MainAddressNatural').val()

                };
            var sarlaftExonerationtDTO = [];
            sarlaftExonerationtDTO = {
                IndividualId: gblIndivualId,
                ExonerationType: $("#selectExemptionType").UifSelect("getSelected"),
                IsExonerated: $("#selectExemptionType").UifSelect("getSelected") == "1",
                RegistrationDate: GetCurrentFromDate(),
                RoleId: 0

            };



            if (gblSarlaft != null && gblSarlaft.SarlaftDTO != null && gblSarlaft.SarlaftDTO.Id != null) {
                individualSarlaft.Id = gblSarlaft.SarlaftDTO.Id;
            }

            financialSarlaft =
                {
                    IncomeAmount: NotFormatMoney($('#inputIncome').val()),
                    ExtraIncomeAmount: NotFormatMoney($('#inputOtherIncome').val()),
                    AssetsAmount: NotFormatMoney($('#inputTotalAssets').val()),
                    ExpenseAmount: NotFormatMoney($('#inputExpenses').val()),
                    LiabilitiesAmount: NotFormatMoney($('#inputTotalLiabilities').val()),
                    Description: description
                };

            gblSarlaft.SarlaftDTO = individualSarlaft;
            gblSarlaft.FinancialSarlaftDTO = financialSarlaft;
            gblSarlaft.CoSarlaftDTO = coindividualSarlaft;
            gblSarlaft.SarlaftExonerationtDTO = sarlaftExonerationtDTO


            var withoutLegalRepresent = false;

            if (gblSarlaft != null && gblSarlaft.LegalRepresentDTO !== undefined && gblSarlaft.LegalRepresentDTO !== null) {
                $.each(gblSarlaft.LegalRepresentDTO, function (i, value) {
                    value.ExpeditionDate = FormatDate(value.ExpeditionDate);
                    value.BirthDate = FormatDate(value.BirthDate);
                });
            }
            else {
                withoutLegalRepresent = true;
            }

            var withExtraInformation = true;

            //Si algún periférico viene vacío
            if (withoutLegalRepresent || gblSarlaft.LinksDTO === undefined || gblSarlaft.LinksDTO === null || gblSarlaft.LinksDTO.length === 0 ||
                gblSarlaft.PartnerDTO === undefined || gblSarlaft.PartnerDTO === null || gblSarlaft.PartnerDTO.length === 0 ||
                gblSarlaft.InternationalOperationDTO === undefined || gblSarlaft.InternationalOperationDTO === null || gblSarlaft.InternationalOperationDTO.length === 0 ||
                (newSarlaftId != null && gblSarlaft.LinksDTO.some(x => x.Status != ParametrizationStatus.Create))) {
                withExtraInformation = false;
            }
            resolve(withExtraInformation);
        });
    }


    static GetLegalRepresent() {
        var legalRepresentSarlaftId = 0;
        if (newSarlaftId != null) {
            legalRepresentSarlaftId = newSarlaftId
        }
        else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
            legalRepresentSarlaftId = gblSarlaft.SarlaftDTO.Id;
        }
        SarlaftRequest.GetLegalRepresentByIndividualId(gblSarlaft.SarlaftDTO.IndividualId, legalRepresentSarlaftId).done(function (data) {
            if (data.success) {
                LegalRepresent = data.result;
                legalStatus = LegalRepresent.Status;
                LegalRepresentative.EditLegalRepresent(LegalRepresent)
            }
        });
    }

    static GetPartners() {
        var partnerSarlaftId = 0;
        if (newSarlaftId != null) {
            partnerSarlaftId = newSarlaftId
        }
        else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
            partnerSarlaftId = gblSarlaft.SarlaftDTO.Id;
        }
        SarlaftRequest.GetPartnersByIndividualId(gblSarlaft.SarlaftDTO.IndividualId, partnerSarlaftId).done(function (data) {
            if (data.success) {
                //Filtro Tipo de documento
                var entityData = docTypesList;
                var i;
                var j;
                for (i = 0; i < data.result.length; i++) {
                    for (j = 0; j < entityData.length; j++) {
                        if (data.result[i].DocumentTypeId == entityData[j].Id) {
                            data.result[i].DocumentTypeDescription = "";
                            data.result[i].DocumentTypeDescription = entityData[j].DescriptionLong;
                        }
                    }
                }
                $.each(data.result, function (index, item) {
                    item.index = index;
                    $("#listPartners").UifListView("addItem", item);
                });
                if (data.result !== null) {
                    gblSarlaft.PartnerDTO = data.result;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }


    EditSarlaft() {
        var item = $("#tblSarlaft").UifDataTable('getSelected');
        if (item != null) {
            sarlaftId = item[0].Id;
            var currentTime = new Date();
            if (item[0].Year == currentTime.getFullYear()) {

                SarlaftParam.ObtainSarlaftData(glbindividualId).then((respSarlaft) => {
                    if (respSarlaft !== null) {
                        LegalRepresentative.stateForm = 3;
                        gblSarlaft = respSarlaft;
                        newSarlaftId = null;
                        mainEconomicActivity.Id = respSarlaft.SarlaftDTO.EconomicActivityId;
                        secondaryEconomicActivity.Id = respSarlaft.SarlaftDTO.SecondEconomicActivityId;
                        mainEconomicActivity.Description = respSarlaft.SarlaftDTO.EconomicActivityDesc;
                        secondaryEconomicActivity.Description = respSarlaft.SarlaftDTO.SecondEconomicActivityDesc;
                        $('#FormMain').show();
                        $('#FormQuery').hide();
                        $('#SarlaftMenu').show();

                        SarlaftParam.FillData(respSarlaft);
                        SarlaftParam.EnabledSarlaft();
                        InternationalOperations.EnabledInternationalOperations();
                        PartnersParam.EnabledPartners();
                        LegalRepresentative.EnabledLegalRepresent();
                        Links.EnabledLinks();
                        Peps.EnabledControles();
                        Peps.DisabledForm(false);
                        $("#selectSCity").UifSelect("disabled", false);
                        $("#Town").UifSelect("disabled", false);


                        $("#rdInternationalOperations").prop("checked", respSarlaft.SarlaftDTO.InternationalOperations);
                    }
                }).catch((err) => {
                    $.UifNotify('show', { 'type': 'danger', 'message': err, 'autoclose': true });
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': "Este Sarlaft no es editable", 'autoclose': true });
            }
        }
    }

    GetSarlaft() {
        LegalRepresentative.stateForm = 2;
        SarlaftParam.GetSarlaft();

    }

    static GetSarlaft(SarlaftId) {
        sarlaftIsNew = false;
        var datos = $("#tblSarlaft").UifDataTable('getSelected');

        if (SarlaftId) {
            datos = [{ Id: SarlaftId }];
        }

        if (datos != null) {
            sarlaftId = datos[0].Id;
            SarlaftRequest.GetSarlaftBySarlaftId(sarlaftId).done(function (data) {
                if (data.success) {

                    if (data.result.SarlaftDTO.PendingEvent) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingEvents, 'autoclose': true });
                    }

                    $('#FormMain').show();
                    $('#FormQuery').hide();
                    $('#SarlaftMenu').show();


                    if (data.result.SarlaftDTO.InternationalOperations == true) {
                        $("#rdInternationalOperations").prop("checked", true);
                    }

                    var result = SarlaftParam.FillData(data.result);
                    InternationalOperations.DisabledInternationalOperations();
                    Links.DisabledLinks();
                    LegalRepresentative.DisabledLegalRepresent();
                    LegalRepresentative.DisabledSubstituteLegalRepresent();
                    PartnersParam.DisabledPartners();
                    Peps.DisabledControles();
                    Peps.DisabledForm(true);

                    $.when(result).done(function (dataResult) {
                        SarlaftParam.DisabledSarlaft();
                    });

                }
            });
        }
    }
    /**
     * Creación
     * */
    static NewSarlaft() {
        PartnersParam.ClearPartnerFields();
        sarlaftIsNew = true;
        if (glbindividualId > 0) {
            SarlaftParam.FillUserInformation();
            SarlaftParam.ObtainLastSarlaftData(gblPerson).then((respSarlaft) => {
                if (respSarlaft !== null) {
                    newSarlaftId = respSarlaft.Id;
                    var lastRegistrationDate = FormatDate((respSarlaft.RegistrationDate == null) ? "01/01/1900" : respSarlaft.RegistrationDate);
                    var currentDate = GetCurrentFromDate();
                    var finalDate = AddToDate(lastRegistrationDate, 0, 0, 1);
                    var days = parseInt(respSarlaft.YearParameter);
                    if (respSarlaft.YearParameter == null)
                        days = 1;
                    var daysRest = CalculateDays(currentDate, finalDate);
                    var fecha = new Date();

                    $("#btnExentoSarlaft").hide();
                    LegalRepresentative.stateForm = 1;
                    //Cargar representantes
                    SarlaftRequest.GetLegalRepresentByIndividualId(gblIndivualId, newSarlaftId).done(function (data) {
                        if (data.success) {
                            if (data.result !== null) {
                                gblSarlaft.LegalRepresentDTO = data.result;
                                glblValidateLegalRepresent = true;
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    });
                    //Cargar asociados
                    SarlaftRequest.GetPartnersByIndividualId(gblIndivualId, newSarlaftId).done(function (data) {
                        if (data.success) {
                            if (data.result !== null) {
                                gblSarlaft.PartnerDTO = data.result;
                                glblValidatePartner = true;
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    });
                    //Cargar links
                    SarlaftRequest.GetIndividualLinksByIndividualId(gblIndivualId, newSarlaftId).done(function (data) {
                        if (data.success) {
                            if (data.result.length > 0) {
                                gblSarlaft.Links = data.result;
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    });

                    if (daysRest <= days) {
                        var sarObject = respSarlaft;
                        $("#inputUserId").val(glbuserId);
                        $('#FormMain').show();
                        $('#FormQuery').hide();
                        $('#SarlaftMenu').show();
                        sarlaftId = null;
                        SarlaftParam.ClearData();
                        $('#IndividualId').val(glbindividualId);
                        $('#inputSignYear').val(fecha.getFullYear());
                        SarlaftParam.EnabledSarlaft();
                        SarlaftParam.ValidateExtraIncome();
                        InternationalOperations.EnabledInternationalOperations();
                        InternationalOperations.GetInternationalOperations();
                        PartnersParam.EnabledPartners();
                        LegalRepresentative.EnabledLegalRepresent();
                        Links.EnabledLinks();
                        Peps.DisabledForm(false);

                        if (gblPerson != null && gblPerson.EconomicActivityId > 0) {
                            sarObject.EconomicActivityId = gblPerson.EconomicActivityId;
                            sarObject.EconomicActivityDesc = gblPerson.EconomicActivityDesc;
                        }
                        //CIIU GEt Juridico
                        if (gblPerson.PersonType == 2)
                            SarlaftParam.SetCIIUCode(gblPerson.EconomicActivityId, gblPerson.EconomicActivityDesc,
                                sarObject.SecondEconomicActivityId, sarObject.SecondEconomicActivityDesc);
                        //CIIU Get
                        SarlaftParam.SetCIIUCode(sarObject.EconomicActivityId, sarObject.EconomicActivityDesc,
                            sarObject.SecondEconomicActivityId, sarObject.SecondEconomicActivityDesc);
                        $("#btnConsultSarlaft").hide();
                        $("#btnSaveSarlaft").show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCreatingNewSarlaft, 'autoclose': true });
                        $("#btnConsultSarlaft").show();
                        $("#btnSaveSarlaft").hide();
                    }

                }
            }).catch((err) => {
                $.UifNotify('show', { 'type': 'danger', 'message': err, 'autoclose': true });
            });
        }
    }

    static EnabledSarlaft() {
        $("#inputApprovedby").prop("disabled", false);
        $('#inputDiligenceDate').prop("disabled", false);
        $('#inputVerifyingOfficial').prop("disabled", false);
        $('#inputInterviewDate').prop("disabled", false);
        $('#inputInterviewManager').prop("disabled", false);

        $('#inputInterviewPlace').prop("disabled", false);
        $('#selectCIIUCodeMainActivity').prop("disabled", false);
        $('#selectSecondaryActivity').prop("disabled", false);
        $('#selectResult').UifSelect("disabled", false);

        $('#inputIncome').prop("disabled", false);
        $('#inputTotalAssets').prop("disabled", false);
        $('#inputExpenses').prop("disabled", false);
        $('#inputTotalLiabilities').prop("disabled", false);
        $('#inputDescription').prop("disabled", false);
        $('#inputOtherIncome').prop("disabled", true);

        $('#ckeckExtraIncome').prop("disabled", false);
        $('#updateCIIU').prop("disabled", false);
        $('#updateCIIU').prop("checked", true);
        $('#rdInternationalOperations').prop("disabled", false);
        $('#rdInternationalOperations').prop("checked", true);

        $('#AdressEmail').prop("disabled", false);
        $("#selectSCompanyType").UifSelect("disabled", false);
        $("#selectSCountry").UifSelect("disabled", false);
        $("#selectSState").UifSelect("disabled", false);
        $("#selectSCity").UifSelect("disabled", false);
        $('#inputConstitutionDate').UifDatepicker('disabled', false);

        $("#selectOppositorType").UifMultiSelect("disabled", false);
        $("#selectPersonType").UifSelect("disabled", false);
        $("#selectSocietyType").UifSelect("disabled", false);
        $("#selectNationality").UifSelect("disabled", false);
        $("#selectNationalityOther").UifSelect("disabled", false);
        $("#MainAddressNatural").prop("disabled", false);


        $("#selectExemptionType").UifSelect("disabled", false);

        $('#inputPhone').prop("disabled", false);


        $('#btnSaveSarlaft').show();
    }

    static DisabledSarlaft() {
        $("#inputApprovedby").prop("disabled", true);
        $('#inputDiligenceDate').prop("disabled", true);
        $('#inputVerifyingOfficial').prop("disabled", true);
        $('#inputVerificationDate').UifDatepicker("disabled", true);
        $('#inputInterviewDate').prop("disabled", true);
        $('#inputInterviewManager').prop("disabled", true);

        $('#inputInterviewPlace').prop("disabled", true);
        $('#selectResult').UifSelect("disabled", true);
        $('#selectCIIUCodeMainActivity').prop("disabled", true);
        $('#selectSecondaryActivity').prop("disabled", true);

        $('#inputIncome').prop("disabled", true);
        $('#inputTotalAssets').prop("disabled", true);
        $('#inputExpenses').prop("disabled", true);
        $('#inputTotalLiabilities').prop("disabled", true);
        $('#inputDescription').prop("disabled", true);
        $('#inputOtherIncome').prop("disabled", true);
        $('#ckeckExtraIncome').prop("disabled", true);
        $('#updateCIIU').prop("disabled", true);
        $('#rdInternationalOperations').prop("disabled", true);

        $("#selectSCountry").UifSelect("disabled", true);
        $("#selectSState").UifSelect("disabled", true);


        $('#AdressEmail').prop("disabled", true);
        $("#selectSCompanyType").UifSelect("disabled", true);
        $("#selectSCity").UifSelect("disabled", true);

        $("#selectOppositorType").UifMultiSelect("disabled", true);
        $("#selectPersonType").UifSelect("disabled", true);
        $("#selectSocietyType").UifSelect("disabled", true);
        $("#selectNationality").UifSelect("disabled", true);
        $("#selectNationalityOther").UifSelect("disabled", true);
        $('#inputPhone').prop("disabled", true);
        $("#MainAddressNatural").prop("disabled", true);

        $("#selectExemptionType").UifSelect("disabled", true);
        $('#inputConstitutionDate').UifDatepicker('disabled', true);


        $('#btnSaveSarlaft').hide();

    }

    static FillData(result) {
        var dfdFD = $.Deferred();
        $("#inputUserName").val(result.SarlaftDTO.UserName);
        $("#inputFormNumber").val(result.SarlaftDTO.FormNum);

        //$('#selectTypeDocument').val(data.result.SarlaftDTO);
        //$('#inputName').val(data.result.SarlaftDTO);

        $('#SarlaftId').val(result.SarlaftDTO.Id);
        $('#IndividualId').val(result.SarlaftDTO.IndividualId);
        $('#inputSignYear').val(result.SarlaftDTO.Year);
        $("#selectSignatureBranch").UifSelect('setSelected', result.SarlaftDTO.BranchId);
        $('#inputDiligenceDate').val(FormatDate(result.SarlaftDTO.RegistrationDate));
        //  $('#inputNumDocument').val(data.result.SarlaftDTO.FormNum);
        $('#inputFormNumber').val(result.SarlaftDTO.FormNum);
        $('#inputApprovedby').val(result.SarlaftDTO.AuthorizedBy);
        $('#inputConstitutionDate').val(FormatDate(result.SarlaftDTO.FillingDate));
        $('#inputVerifyingOfficial').val(result.SarlaftDTO.VerifyingEmployee);
        $('#inputVerificationDate').val(FormatDate(result.SarlaftDTO.CheckDate));
        $('#inputInterviewDate').val(FormatDate(result.SarlaftDTO.InterviewDate));
        $('#inputInterviewManager').val(result.SarlaftDTO.InterviewerName);
        $('#inputInterviewPlace').val(result.SarlaftDTO.InterviewPlace);
        $('#selectResult').UifSelect('setSelected', result.SarlaftDTO.InterviewResultId);
        $('#rdInternationalOperations').val(result.SarlaftDTO.InternationalOperations);
        $('#selectCIIUCodeMainActivity').val(result.SarlaftDTO.EconomicActivityId+" - "+result.SarlaftDTO.EconomicActivityDesc);
        $('#selectCIIUCodeMainActivity').trigger("blur");

        if (result.SarlaftDTO.SecondEconomicActivityId == 0 || result.SarlaftDTO.SecondEconomicActivityId == 1) {
            result.SarlaftDTO.SecondEconomicActivityDesc = "";
        }

        if (result.SarlaftDTO.SecondEconomicActivityDesc != null && result.SarlaftDTO.SecondEconomicActivityDesc.trim() != "") {
            $('#selectSecondaryActivity').val(result.SarlaftDTO.SecondEconomicActivityId + " - " +result.SarlaftDTO.SecondEconomicActivityDesc);
            $('#selectSecondaryActivity').trigger("blur");
        }

        $('#inputIncome').val(result.FinancialSarlaftDTO.IncomeAmount);

        if (result.SarlaftDTO.EconomicActivityId >= 0) {
            $("#updateCIIU").prop("checked", true);
        }

        if ((result.FinancialSarlaftDTO.ExtraIncomeAmount !== null && result.FinancialSarlaftDTO.ExtraIncomeAmount !== 0) || result.SarlaftDTO.SecondEconomicActivityId > 1) {
            $("#ckeckExtraIncome").prop("checked", true);
            $('#inputOtherIncome').prop("disabled", false);
        }

        SarlaftParam.ValidateExtraIncome();
        $('#inputOtherIncome').val(result.FinancialSarlaftDTO.ExtraIncomeAmount);
        $('#inputTotalAssets').val(result.FinancialSarlaftDTO.AssetsAmount);
        $('#inputExpenses').val(result.FinancialSarlaftDTO.ExpenseAmount);
        $('#inputTotalLiabilities').val(result.FinancialSarlaftDTO.LiabilitiesAmount);
        $('#inputDescription').val(result.FinancialSarlaftDTO.Description);
        SarlaftParam.Heritage();

        if (result.CoSarlaftDTO != null) {
            $('#AdressEmail').val(result.CoSarlaftDTO.email);
            $('#inputHeritage').val(result.CoSarlaftDTO.heritage);
            $('#inputPhone').val(result.CoSarlaftDTO.Phone);

            //if (result.CoSarlaftDTO.idCompanyTypeCode != null && result.CoSarlaftDTO.idCompanyTypeCode > 0)
            //    $("#selectSCompanyType").UifSelect('setSelected', result.CoSarlaftDTO.idCompanyTypeCode);

            if (result.CoSarlaftDTO.countryCode != null && result.CoSarlaftDTO.countryCode > 0) {
                $("#selectSCountry").UifSelect('setSelected', result.CoSarlaftDTO.countryCode);
                SarlaftParam.GetStateCity(result.CoSarlaftDTO.countryCode, result.CoSarlaftDTO.stateCode, result.CoSarlaftDTO.cityCode);  
            }

            //if (result.CoSarlaftDTO.stateCode != null && result.CoSarlaftDTO.stateCode > 0)
            //    $("#selectSState").UifSelect('setSelected', result.CoSarlaftDTO.stateCode);

            //var CityData = SarlaftParam.GetCity();

            //$.when(CityData).done(function (CityData) {
            //    if (result.CoSarlaftDTO.cityCode != null && result.CoSarlaftDTO.cityCode > 0)
            //        $("#selectSCity").UifSelect('setSelected', result.CoSarlaftDTO.cityCode);
            //    dfdFD.resolve(CityData);
            //});

            if (result.CoSarlaftDTO.OppositorTypeCode != null && result.CoSarlaftDTO.OppositorTypeCode.length > 0)
                $("#selectOppositorType").UifMultiSelect('setSelected', JSON.parse(result.CoSarlaftDTO.OppositorTypeCode));

            if (result.CoSarlaftDTO.PersonTypeCode != null && result.CoSarlaftDTO.PersonTypeCode > 0)
                $("#selectPersonType").UifSelect('setSelected', result.CoSarlaftDTO.PersonTypeCode);

            if (result.CoSarlaftDTO.SocietyTypeCode != null && result.CoSarlaftDTO.SocietyTypeCode > 0)
                $("#selectSocietyType").UifSelect('setSelected', result.CoSarlaftDTO.SocietyTypeCode);

            if (result.CoSarlaftDTO.NationalityCode != null && result.CoSarlaftDTO.NationalityCode > 0)
                $("#selectNationality").UifSelect('setSelected', result.CoSarlaftDTO.NationalityCode);

            if (result.CoSarlaftDTO.NationalityOtherCode != null && result.CoSarlaftDTO.NationalityOtherCode > 0)
                $("#selectNationalityOther").UifSelect('setSelected', result.CoSarlaftDTO.NationalityOtherCode);

            if (result.CoSarlaftDTO.ExonerationTypeCode != null && result.CoSarlaftDTO.ExonerationTypeCode > 0)
                $("#selectExemptionType").UifSelect('setSelected', result.CoSarlaftDTO.ExonerationTypeCode);

            if (result.CoSarlaftDTO.MainAddressNatural != null)
                $('#MainAddressNatural').val(result.CoSarlaftDTO.MainAddressNatural);





        }

        if (result.PepsDTO != null) {

            if (result.PepsDTO.Exposed || result.PepsDTO.Exposed == null) {
                glbValidatePeps = true;
                $("#chkExposedN").prop("checked", false);
                $("#chkExposedS").prop("checked", true);

            }
            else {
                $("#chkExposedN").prop("checked", true);
                $("#chkExposedS").prop("checked", false);
            }


            $("#Affinity").UifSelect('setSelected', result.PepsDTO.Affinity);
            $("#Category").UifSelect('setSelected', result.PepsDTO.Category);
            $("#Link").UifSelect('setSelected', result.PepsDTO.Link);

            $("#Entit").val(result.PepsDTO.Entity);
            $("#Observations").val(result.PepsDTO.Observations);
            $("#NamePeps").val(result.PepsDTO.Trade_Name);
            $("#Unlinked").UifSelect('setSelected', result.PepsDTO.Unlinked);
            $("#Unlinked_DATE").val(FormatDate(result.PepsDTO.Unlinked_DATE));
            $("#inputJobOffice").val(result.PepsDTO.JobOffice);
        }
        return dfdFD.promise();
    }

    static GetCity() {
        var dfd = $.Deferred();
        var countryIdL = $('#selectSCountry').UifSelect("getSelected");
        var StatedIdL = $('#selectSState').UifSelect("getSelected");
        SarlaftRequest.GetCities(countryIdL, StatedIdL).done(function (data) {
            if (data.success) {
                $("#selectSCity").UifSelect({ sourceData: data.result });

                dfd.resolve(data.result);
            }
            else
                dfd.reject();
        });
        return dfd.promise();
    }

    static GetStateCity(countryId, stateId, cityId) {
        SarlaftRequest.GetStates(countryId, 0).done(function (data) {
            if (data.success) {
                gdlStates = data.result;
                $("#selectSState").UifSelect({ sourceData: data.result });
                SarlaftRequest.GetCities(countryId, stateId).done(function (data) {
                    if (data.success) {
                        $("#selectSCity").UifSelect({ sourceData: data.result });
                        if (stateId != null && stateId > 0) {
                            $("#selectSState").UifSelect('setSelected', stateId);
                        }
                        if (cityId != null && cityId > 0) {
                            $("#selectSCity").UifSelect('setSelected', cityId);
                        }
                    }
                });
            }
        });
    }

    static SetCIIUCode(activityIdPrim, descActivityPrim, activityIdSecond, descActivitySecond) {
        if ((activityIdPrim !== null && activityIdPrim > 0) && (descActivityPrim != null && descActivityPrim !== "")) {
            $('#selectCIIUCodeMainActivity').val(activityIdPrim +" - "+ descActivityPrim);
            $('#selectCIIUCodeMainActivity').trigger("blur");
        }
        if ((activityIdSecond !== null && activityIdSecond !== "") && (descActivitySecond != null && descActivitySecond !== "")) {
            $('#selectSecondaryActivity').val(activityIdSecond+" - " + descActivitySecond);
            $('#selectSecondaryActivity').trigger("blur");

        }
    }
    static ClearData(data) {
        gblSarlaft = {};
        $('#SarlaftId').val("");
        $('#IndividualId').val("");
        $('#inputApprovedby').val("");
        $('#inputVerifyingOfficial').val("");
        $('#inputVerificationDate').val(GetCurrentFromDate());

        $('#inputVerificationDate').UifDatepicker('disabled', true);
        $('#inputInterviewManager').val("");

        $('#inputDiligenceDate').val("");
        $('#inputConstitutionDate').val("");
        $('#inputInterviewDate').val("");

        $('#selectOppositorType').UifMultiSelect('deSelectAll');
        $("#inputOtherIncome").val("");
        $("#ckeckExtraIncome").prop("checked", false);
        $("#ckeckExtraIncome").prop("disabled", false);
        $("#selectSecondaryActivity").prop("disabled", false);

        $('#inputInterviewPlace').val("");
        $('#selectResult').UifSelect('setSelected', "");
        $("#rdInternationalOperations").prop("checked", false);
        $('#selectCIIUCodeMainActivity').val('');
        $('#selectSecondaryActivity').val('');

        $('#inputIncome').val("");
        $('#inputTotalAssets').val("");
        $('#inputExpenses').val("");
        $('#inputTotalLiabilities').val("");
        $('#inputDescription').val("");
        $('#labelnamesarlaft').text("");
        $('#AdressEmail').val("");
        SarlaftParam.Heritage();

        $("#selectSCountry").UifSelect('setSelected', "");
        $('#selectSCity').UifSelect('setSelected', "");
        $('#selectSState').UifSelect('setSelected', "");
        $('#selectSCompanyType').UifSelect('setSelected', "");

        $("#selectOppositorType").UifMultiSelect('setSelected', "");
        $("#selectPersonType").UifSelect('setSelected', "");
        $("#selectSocietyType").UifSelect('setSelected', "");
        $("#selectNationality").UifSelect('setSelected', "");
        $("#selectNationalityOther").UifSelect('setSelected', "");
        $('#inputPhone').val("");
        $('#MainAddressNatural').val("");

        $("#selectExemptionType").UifSelect('setSelected', 0);

        LegalRepresentative.ClearFields();
        LegalRepresentative.ClearSubstituteFields();
        Peps.ClearControls();
        InternationalOperations.ClearFields();
        Links.ClearFields();
    }

    ExitSarlaft() {
        window.location = rootPath + "Home/Index";
    }

    ExemptSarlaft() {
        $('#modalExempt').UifModal('showLocal', Resources.Language.TitleExempt);
    }



    BlockDescription(event, selectedItem) {
        if (selectedItem.Id == 1)
            $('#inputDescription').prop("required", true);
        else
            $('#inputDescription').prop("required", false);
    }

    static RepeatedActivity() {
        if (mainEconomicActivity.Id == secondaryEconomicActivity.Id) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDuplicateActivity, 'autoclose': true });
            $('#selectSecondaryActivity').val("");
        }
    }

    ValidateAssets() {
        var totalAssets = NotFormatMoney($("#inputTotalAssets").val());
        var totalLiabilities = NotFormatMoney($("#inputTotalLiabilities").val());
        SarlaftParam.Heritage();

        if (parseInt(totalAssets) < parseInt(totalLiabilities)) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorTotalAssets, 'autoclose': true });
        }
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        $(this).val(FormatMoney($(this).val()));

    }
    static Heritage() {
        let inputTotalAssets = $("#inputTotalAssets").val() == "" ? 0 : parseInt(NotFormatMoney($("#inputTotalAssets").val()));
        let inputTotalLiabilities = $("#inputTotalLiabilities").val() == "" ? 0 : parseInt(NotFormatMoney($("#inputTotalLiabilities").val()));

        $("#inputHeritage").val(inputTotalAssets - inputTotalLiabilities);
    }

    SearchCIIUCodeMainActivity() {
        if ($("#selectCIIUCodeMainActivity").val().trim().length > 2) {
            SarlaftParam.GetCIIUCodeMainActivityByDescription($("#selectCIIUCodeMainActivity").val().trim());
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
            $("#selectCIIUCodeMainActivity").val("");
        }
    }

    static GetCIIUCodeMainActivityByDescription(description) {
        SarlaftRequest.GetEconomicActivities(description).done(function (data) {
            $("#selectCIIUCodeMainActivity").val("");
            if (data.success) {
                if (data.result !== null && data.result.length > 0) {
                    var dataResult = [];
                    if (data.result.length == 1) {
                        $("#selectCIIUCodeMainActivity").val(data.result[0].Description);
                        mainEconomicActivity = data.result[0];
                    }
                    else
                        if (data.result.length > 1) {

                            $.each(data.result, function (index, value) {
                                dataResult.push({
                                    Id: index + 1,
                                    Description: value.Description
                                });
                            });

                            $("#hdnTypeTblResult").val("selectCIIUCodeMainActivity");
                            $('#tblResult').UifDataTable('clear');
                            $("#tblResult").UifDataTable('addRow', dataResult);
                            $('#modalListSearch').UifModal('showLocal', AppResources.CIIUCodeMainActivity);
                        }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMainEconomicActivity, 'autoclose': true })
            }
        });
    }


    SearchCIIUCodeSecondActivity() {
        if ($("#selectSecondaryActivity").val().trim().length > 2) {
            SarlaftParam.GetCIIUCodeSecondActivityByDescription($("#selectSecondaryActivity").val().trim());
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
            $("#selectSecondaryActivity").val("");
        }
    }

    static GetCIIUCodeSecondActivityByDescription(description) {
        SarlaftRequest.GetEconomicActivities(description).done(function (data) {
            $("#selectSecondaryActivity").val("");
            if (data.success) {
                if (data.result !== null && data.result.length > 0) {
                    var dataResult = [];
                    if (data.result.length == 1) {
                        $("#selectSecondaryActivity").val(data.result[0].Description);
                        secondaryEconomicActivity = data.result[0];
                        SarlaftParam.RepeatedActivity();
                    }
                    else
                        if (data.result.length > 1) {

                            $.each(data.result, function (index, value) {
                                dataResult.push({
                                    Id: index + 1,
                                    Description: value.Description
                                });
                            });

                            $("#hdnTypeTblResult").val("selectSecondaryActivity");
                            $('#tblResult').UifDataTable('clear');
                            $("#tblResult").UifDataTable('addRow', dataResult);
                            $('#modalListSearch').UifModal('showLocal', AppResources.SecondaryActivity);
                        }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMainEconomicActivity, 'autoclose': true })
            }
        });
    }


    static ValidateExtraIncome() {
        if ($("#ckeckExtraIncome").prop("checked")) {
            $("#selectSecondaryActivity").prop("disabled", false);
            $('#inputOtherIncome').prop("disabled", false);
            $("#selectSecondaryActivity").prop("required", true);
            $('#inputOtherIncome').prop("required", true);

        } else {
            $("#selectSecondaryActivity").prop("disabled", true);
            $('#inputOtherIncome').prop("disabled", true);
            $("#selectSecondaryActivity").prop("required", false);
            $('#inputOtherIncome').prop("required", false);
        }
    }

    static UpdateCIIU() {
        if ($("#updateCIIU").prop("checked")) {
            $("#selectCIIUCodeMainActivity").prop("disabled", false);
        } else {
            $("#selectCIIUCodeMainActivity").prop("disabled", true);
        }
    }


    static ValidateAuthorizationRequest(authorizationsModel) {
        if (authorizationsModel.IsAuthorizationRequest) {
            if (authorizationsModel.AuthorizationRequests.every(x => x.Status == 1)) { //Pendientes
                $.UifDialog('alert', {
                    message: Resources.Language.SarlaftWithAutorizationRequestPolicies
                });

            }
            else if (authorizationsModel.AuthorizationRequests.every(x => x.Status == 3)) { //Rechazadas
                $.UifDialog('confirm', {
                    message: String.format(Resources.Language.SarlaftWithRejectedPolicies)
                }, function (result) {
                    if (result) {
                        SarlaftParam.DisablePolicies(authorizationsModel.AuthorizationRequests);
                    }
                });
            }
        }
    }

    static DisablePolicies(authorizationRequests) {
        SarlaftRequest.DisablePolicies(authorizationRequests).done(function (response) {
            if (response.success) {
                $.UifNotify('show', {
                    'type': 'success', 'message': Resources.Language.AllPoliciesRejected
                });
                SarlaftParam.NewSarlaft();
            } else {
                $.UifNotify('show', {
                    'type': 'danger', 'message': Resources.Language.CannotRejectPolicies
                });
            }
        });
    }

    static loadCountries() {
        SarlaftRequest.GetCountries().done(function (data) {
            if (data.success) {
                $("#selectSCountry").UifSelect({ sourceData: data.result });
            }
        });


        SarlaftRequest.LoadInitialLegalData(2).done(function (data) {
            if (data.success) {
                $("#selectSCompanyType").UifSelect({ sourceData: data.result.CompanyTypes });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });


    }

    ChangeCountries(event, selectedItem) {

        if (selectedItem.Id > 0) {
            SarlaftRequest.GetStates(selectedItem.Id, 0).done(function (data) {
                if (data.success) {
                    gdlStates = data.result;
                    $("#selectSState").UifSelect({ sourceData: data.result });
                    $('#selectSState').prop('disabled', false);
                }
            });
        }
        else {
            $("#selectSState").UifSelect();
            $('#selectSState').prop('disabled', false);
        }

    }

    ChangeStates(event, selectedItem) {
        var countryId = $('#selectSCountry').UifSelect("getSelected");
        if (selectedItem.Id > 0) {
            SarlaftRequest.GetCities(countryId, selectedItem.Id).done(function (data) {
                if (data.success) {
                    gdlCity = data.result;
                    $("#selectSCity").UifSelect({ sourceData: data.result });
                }
            });
        }
        else {
            $("#selectSCity").UifSelect();
        }
    }

    static GetExemptTypeExemption() {
        $("#selectExemptionType").UifSelect({ sourceData: [{ "Id": "0", "Description": "No" }, { "Id": "1", "Description": "Si" }], selectedId: 0 });
    }

    static GetOppositorType() {
        SarlaftRequest.GetOppositor().done(function (data) {
            if (data.success) {
                $("#selectOppositorType").UifMultiSelect({ sourceData: data.result, numberDisplayed: data.result.length });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetSocietyType() {

        SarlaftRequest.GetSociety().done(function (data) {
            if (data.success) {
                $("#selectSocietyType").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });




    }

    static GetNationality() {

        SarlaftRequest.GetNationality().done(function (data) {
            if (data.success) {
                $("#selectNationality").UifSelect({ sourceData: data.result });
                $("#selectNationalityOther").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });


    }

    SearchInterviewManager() {
        if ($("#inputInterviewManager").val().trim().length > 2) {
            SarlaftParam.GetInterviewManagerByDescription($("#inputInterviewManager").val().trim());
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
            $("#inputInterviewManager").val("");
        }
    }

    static GetInterviewManagerByDescription(InterviewManager) {
        SarlaftRequest.GetInterviewManagerByDescription(InterviewManager).done(function (data) {
            $("#inputInterviewManager").val("");
            if (data.success) {
                if (data.result !== null && data.result.length > 0) {
                    var dataInterviewManager = [];
                    if (data.result.length == 1) {
                        $("#inputInterviewManager").val(data.result[0]);
                    }
                    else
                        if (data.result.length > 1) {

                            $.each(data.result, function (index, value) {
                                dataInterviewManager.push({
                                    Id: index + 1,
                                    Description: value
                                });
                            });

                            $("#hdnTypeTblResult").val("inputInterviewManager");
                            $('#tblResult').UifDataTable('clear');
                            $("#tblResult").UifDataTable('addRow', dataInterviewManager);
                            $('#modalListSearch').UifModal('showLocal', AppResources.InterviewManager);
                        }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NotFoundInterviewManager, 'autoclose': true })
            }
        });
    }

    static ExemptSave() {
        var sarlaftExonerationtDTO = [];
        if ($("#selectExemptionType").UifSelect("getSelected") != "1") {

            sarlaftExonerationtDTO = {
                IndividualId: gblIndivualId,
                ExonerationType: $("#selectExemptionType").UifSelect("getSelected"),
                IsExonerated: 1,
                RegistrationDate: GetCurrentFromDate(),
                RoleId: 0
            };
        }
    }

    static ValidateLegalRepresent() {

        if (gblPerson.PersonType == 2 && $("#selectExemptionType").UifSelect("getSelected") != "1") {

            if (gblSarlaft.LegalRepresentDTO != null) {
                if (gblSarlaft.LegalRepresentDTO.length == 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "La Información de Representante Legal es obligatoria.", 'autoclose': true });
                    return false;
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "La Información de Representante Legal es obligatoria.", 'autoclose': true });
            }
        }
        return true;

    }
}