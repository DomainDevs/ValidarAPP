//Variables globales
var AffectedDocumentNumber = "";
var plate = "";
var _enabled;
var _coveredRiskType = null;
var _endorsementId = null;
var policyHolderId = 0;
var policyId = null;
var riskId = null;
var riskNum = null;
var noticeId = 0;
var noticeStateId = 0;
var policyConsult = false;
var insuredId = null;
var individualIdGeneral = null;
var idIndividualByCoverage = 0;
var participantId = 0;
var insuredDocumentTypeId = 0;
var currentEditIndex = -1;
var notice = null;
var documentNumber = 0;
var policyBusinessTypeId = 0;
var policyTypeId = 0;
var policyProductId = 0;
var policyCurrencyId = 0;
var policyCurrencyDescription = null;
var isCreation = false;
var _endorsementTypeId = null;
var policyFromDate = null;
var policyToDate = null;
var focusinDate = false;
var messageCount = 0;

class Notice extends Uif2.Page {

    getInitialState() {
        this.LoadTime('selectHour', 24);
        Notice.InitialNotice();
    }

    bindEvents() {
        // UpperCase
        $('#inputContactInformationName').TextTransform(ValidatorType.UpperCase);
        $('#inputdriverInformation').TextTransform(ValidatorType.UpperCase);
        $('#Location').TextTransform(ValidatorType.UpperCase);
        $('#Description').TextTransform(ValidatorType.UpperCase);
        $('#OthersAffected').TextTransform(ValidatorType.UpperCase);
        $('#inputContactInformationMail').TextTransform(ValidatorType.UpperCase);
        $('#insuredFullName').TextTransform(ValidatorType.UpperCase);
        $('#inputThirdFullName').TextTransform(ValidatorType.UpperCase);
        $('#txtCoverage').TextTransform(ValidatorType.UpperCase);

        // Pattern
        $('#inputContactInformationPhone').ValidatorKey(ValidatorType.Number, 2, 1);
        $('#inputContactInformationMail').on("blur", Notice.ValidateEmail);           
        $('#estimatedValue').ValidatorKey(ValidatorType.Number, 1, 1);

        // ItemSelected
        $('#insuredDocumentNumber').on('itemSelected', Notice.SetNoticeInformation);
        $('#insuredDocumentNumber').on('keyup', function () {
            if (insuredDocumentTypeId != 0) {
                NoticeRequest.GetInsuredsByIndividualId(insuredId).done(function (response) {
                    if (response.success && response.result.length > 0) {
                        if ($("#insuredDocumentNumber").UifAutoComplete('getValue') != response.result[0].DocumentNumber) {
                            insuredId = 0;
                            insuredDocumentTypeId = 0;                            
                            $("#insuredFullName").UifAutoComplete('setValue', "");
                        }
                    }
                });
            }
        });

        $("#insuredFullName").on('itemSelected', Notice.SetNoticeInformation);
        $("#insuredFullName").on('keyup', function () {
            if (insuredDocumentTypeId != 0) {
                NoticeRequest.GetInsuredsByIndividualId(insuredId).done(function (response) {
                    if (response.success && response.result.length > 0) {
                        if ($("#insuredFullName").UifAutoComplete('getValue') != response.result[0].FullName) {
                            insuredId = 0;
                            insuredDocumentTypeId = 0;                            
                            $("#insuredDocumentNumber").UifAutoComplete('setValue', "");
                        }
                    }
                });
            }            
        });

        $('#Country').on('itemSelected', Notice.LoadDepartments);
        $('#State').on('itemSelected', Notice.LoadCities);
        $('#txtCoverage').on('itemSelected', Notice.GetCoverageLimit);
        $("#inputThirdDocumentNumber").on('itemSelected', Notice.LoadThirdInformation);        
        $("#inputThirdFullName").on('itemSelected', Notice.LoadThirdInformation);
        $('input[type=radio][name=MotivoAviso]').change(Notice.ValidateRadioNotice);

        $("#Date").on("datepicker.change", function () {
            Notice.ValidateNoticeOccurrenceAndNoticeDate();
            Notice.ValidateOcurrenceDate(null, $("#Date").UifDatepicker('getValue'));            
        });        

        $("#Date").focusin(function () {
            focusinDate = true;
        });

        $("#_noticeDate").on("datepicker.change", function () {
            Notice.ValidateNoticeOccurrenceAndNoticeDate();
        });

        // Click
        $('#btnGetPolicyInfo').on("click", Notice.LoadPolicyInfo);
        $("#ClaimedAmount").focusin(Notice.NotFormatMoneyIn);
        $("#ClaimedAmount").focusout(Notice.FormatMoneyOut);

        //Agendar
        $("#btnScheduleClaimNotice").on("click", Notice.ScheduleNotice);
        $("#btnSchedule").on("click", Notice.SaveSchedule);
        $("#btnCancelSchedule").on("click", Notice.CloseModalScheduleNotice)
        //$('#btnSchedule').on('click', ClaimSearch.SaveSchedule);

        //Objetar
        $("#btnObjectClaimNotice").on("click", Notice.NoticeObjection);
        $('#btnObjectedYes').on('click', Notice.UpdateNoticeObjection);
        $('#btnObjectedNo').on('click', Notice.CloseModalNoticeObjection);
        
        //Denunciar
        $('#btnConvertClaimNoticeToClaim').on('click', Notice.ToConvertNoticeToClaim);
        $('#btnClaimNoticeYes').on('click', Notice.ConvertNoticeToClaim);
        $('#btnClaimNoticeNo').on('click', Notice.CloseModalNoticeToClaim);

        $('#coveragesTable').on('rowAdd', Notice.ModalCoverage);
        $('#coveragesTable').on('rowEdit', Notice.ModalEditCoverage);
        $('#coveragesTable').on('rowDelete', Notice.DeleteCoverage);
        $('#btnAddCoverage').on("click", Notice.AddCoverage);

        $('#victim').on("change", Notice.TypeVictimSetting);
        $("#InputTimeIncident").UifTimepicker('setValue', '00:00:00 PM');

        // Focus 
        $("#estimatedValue").focusin(Notice.NotFormatMoneyIn);
        $("#estimatedValue").focusout(Notice.FormatMoneyOut);

        // Limpiar Formulario
        $("#btnCleanFormNotice").on("click", Notice.CleanFields);
    }

    /////////////////////////////////////////////////
    // LoadPage()
    static InitialNotice() {
        $('#titleVehicle').hide();
        $('#NoticeVehicle').hide();
        $('#TitleSurety').hide();
        $('#NoticeLocation').hide();
        $('#TitleProperty').hide();
        $('#NoticeSurety').hide();
        $('#TitleTransport').hide();
        $('#NoticeTransport').hide();
        $('#TitleAirCraft').hide();
        $('#NoticeAirCraft').hide();
        $('#TitleFidelity').hide();
        $('#NoticeFidelity').hide();
        $('#_policyId').val('');
        $('.hideSearchClaim').hide();
        Notice.GetBranchesByUserId();

        switch (glNoticeOption) {
            case 1:
                $('#titleVehicle').show();
                $('#NoticeVehicle').show();
                break;
            case 2:
                $('#TitleProperty').show();
                $('#NoticeLocation').show();
                break;
            case 3:
                $('#TitleSurety').show();
                $('#NoticeSurety').show();
                break;
            case 4:
                $('#TitleTransport').show();
                $('#NoticeTransport').show();
                break;
            case 5:
                $('#TitleAirCraft').show();
                $('#NoticeAirCraft').show();
                break;
            case 6:
                $('#TitleFidelity').show();
                $('#NoticeFidelity').show();
                break;
        }

        NoticeRequest.GetEstimationsType().done(function (response) {
            if (response.success) {
                $('#ddlEstimationType').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeRequest.GetDocumentTypes().done(function (response) {
            if (response.success) {
                $('#ddlDocumentTypeCoverageModal').UifSelect({ sourceData: response.result.DocumentTypeServiceModel });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeRequest.GetCountries().done(function (response) {
            if (response.success) {
                $('#Country').UifSelect({ sourceData: response.result });

                NoticeRequest.GetDefaultCountry().done(function (data) {
                    if (data.success) {
                        $("#Country").UifSelect("setSelected", data.result);
                        $('#Country').trigger("change");
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });                
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeRequest.GetClaimBranchesbyUserId().done(function (response) {
            if (response.success) {
                $('#ddlClaimBranchSearch').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeRequest.GetBranches().done(function (response) {
            if (response.success) {
                $('#ddlBranch').UifSelect({ sourceData: response.result, enable: false });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                $('#ddlPrefix').UifSelect({ sourceData: response.result, enable: false });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        $("#btnScheduleClaimNotice").prop("disabled", modelSearchNotice.claimNoticeId == null ? true : false);
        $("#btnObjectClaimNotice").prop("disabled", modelSearchNotice.claimNoticeId == null ? true : false);
        $("#btnConvertClaimNoticeToClaim").prop("disabled", modelSearchNotice.claimNoticeId == null ? true : false);
        
        $("#_noticeDate").UifDatepicker("disabled", modelSearchNotice.claimNoticeId == null ? false : true);

        if (modelSearchNotice.claimNoticeId == null) {
            $("#_noticeDate").UifDatepicker("setValue", GetCurrentFromDate());
        }        
    }

    /////////////////////////////////////////////////
    // Metodos de Consulta - Ubicación del Siniestro
    static LoadDepartments(event, selectedItem) {
        Notice.GetStatesByCountryId(selectedItem.Id);
    }

    static LoadCities(event, selectedItem) {
        var countryId = $('#Country').UifSelect('getSelected');
        var stateId = selectedItem.Id;
        Notice.GetCitiesByCountryIdStateId(countryId, stateId);
    }

    static GetInsuredsByIndividualId(individualId) {
        NoticeRequest.GetInsuredsByIndividualId(individualId).done(function (response) {
            if (response.success && response.result.length > 0) {
                $('#insuredDocumentNumber').UifAutoComplete('setValue', response.result[0].DocumentNumber);
                $('#insuredFullName').UifAutoComplete('setValue', response.result[0].FullName);
                $("#txtInsuredName").val(response.result[0].FullName);
                insuredId = response.result[0].IndividualId;
                insuredDocumentTypeId = response.result[0].DocumentTypeId;
            }
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        NoticeRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
            if (response.success) {
                $('#City').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetStatesByCountryId(countryId) {
        if (countryId !== "" && countryId !== undefined && countryId !== null) {
            NoticeRequest.GetStatesByCountryId(countryId).done(function (response) {
                if (response.success) {
                    $('#State').UifSelect({ sourceData: response.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#State").UifSelect('setSelected', null);
            $('#City').UifSelect('setSelected', null);
        }
    }

    /////////////////////////////////////////////////
    // Sección de Eventos y Consultas
    static SetNoticeInformation(event, selectedItem) {
        $('#insuredDocumentNumber').UifAutoComplete('setValue', selectedItem.DocumentNumber);
        $('#insuredFullName').UifAutoComplete('setValue', selectedItem.FullName);
        insuredDocumentTypeId = selectedItem.DocumentTypeId;        
        insuredId = selectedItem.IndividualId;        
        switch (glNoticeOption) {
            case 1:
                NoticeVehicle.GetRisksByInsuredId(insuredId);
                break;
            case 2:
                NoticeLocation.GetRisksByInsuredId(insuredId);
                break;
            case 3:
                NoticeSurety.GetRisksByInsuredId(insuredId);
                break;
            case 4:
                NoticeTransport.GetRisksByInsuredId(insuredId);
                break;
            case 5:
                NoticeAirCraft.GetRisksByInsuredId(insuredId);
                break;
            case 6:
                NoticeFidelity.GetRisksByInsuredId(insuredId);
                break;
        }
    }

    static GetCoverageLimit(event, selectedItem) {
            $("#txtInsuredAmount").val(FormatMoney(selectedItem.InsurableAmount));
            $("#txtCoverage").data('info', { 'CoverageId': selectedItem.Id, 'CoverageNumber': selectedItem.Number });
    }

    static LoadThirdInformation(event, selectedItem) {
        idIndividualByCoverage = 0;
        $("#inputThirdFullName").UifAutoComplete('setValue',selectedItem.Name);
        $("#inputThirdDocumentNumber").UifAutoComplete('setValue',selectedItem.DocumentNumber);
        $('#ddlDocumentTypeCoverageModal').UifSelect('setSelected', selectedItem.DocumentTypeId);
        idIndividualByCoverage = selectedItem.IndividualId;
    }
       
    static AddCoverage(){

        var estimatedValue = NotFormatMoney($("#estimatedValue").val()).replace(',', '.');
        var insuredAmount = NotFormatMoney($("#txtInsuredAmount").val()).replace(',','.');

        var estimationTypeId = $('#ddlEstimationType').UifSelect("getSelected");
        var coverageNumer = $("#txtCoverage").data("info").CoverageNumber;
        var coverages = $('#coveragesTable').UifDataTable('getData');
        var AffectedDocumentNumber = $("#inputThirdDocumentNumber").val();


        if ($("#victim").UifSelect("getSelected") != TypeVictim.Insured && idIndividualByCoverage == 0)
        {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ThirdAffectedIsMandatory, 'autoclose': true });
        }
        else if (Number(estimatedValue) > Number(insuredAmount)) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoveragesPartialInsuredAmountMessage, 'autoclose': true });
        }
        else {
            if (Notice.ValidateCoverageIndividualId){        
                if ($("#txtInsuredAmount").val() == "") {
                    $("#txtInsuredAmount").val(0);
                }

                var dataCoverage = {
                    'AffectedType': $("#victim").UifSelect("getSelectedText"),
                    'Name': $("#inputThirdFullName").val(),
                    'EstimationTypeDescription': $("#ddlEstimationType option:selected").text(),
                    'EstimationTypeId': estimationTypeId,
                    'CoverageName': $("#txtCoverage").val(),
                    'EstimatedValue': estimatedValue,
                    'InsuredAmount': insuredAmount,
                    'IsInsured': $("#victim").UifSelect("getSelected") == 1 ? true: false,
                    'RiskNum': riskNum,
                    'CoverageNumber': $("#txtCoverage").data("info").CoverageNumber,
                    'CoverageId': $("#txtCoverage").data("info").CoverageId,
                    'Driver': "",
                    'DocumentTypeId': $("#ddlDocumentTypeCoverageModal").val(),
                    'DocumentNumber': AffectedDocumentNumber,
                    'IndividualId': ($("#victim").UifSelect("getSelected") == TypeVictim.Insured) ? insuredId : idIndividualByCoverage,
                    'CurrencyId': policyCurrencyId,
                    'CurrencyDescription': policyCurrencyDescription
                };
                                
                if (isCreation) {
                    // Add                    
                    var result = coverages.findIndex(item => item.CoverageNumber == coverageNumer && item.EstimationTypeId == estimationTypeId && item.DocumentNumber == AffectedDocumentNumber);

                    if (result > -1) {                        
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageValidationNoticeCoverage, 'autoclose': true });
                        return;
                    } 

                    dataCoverage.IsProspect = (idIndividualByCoverage == 0 ) ? true : false;
                    $("#coveragesTable").UifDataTable('addRow', dataCoverage);        //debe ir el id de la cobertura
                }
                else {
                    // Edit
                    if (participantId > 0 && idIndividualByCoverage == 0) {
                        dataCoverage.IndividualId = participantId;
                        dataCoverage.IsProspect = true;
                    }
                    else if (participantId == 0 && idIndividualByCoverage > 0) {
                        dataCoverage.IndividualId = idIndividualByCoverage;
                        dataCoverage.IsProspect = false;
                    }
                    $("#coveragesTable").UifDataTable('editRow', dataCoverage, currentEditIndex); 
                }

                $('#addCoverageForm').trigger("reset");
                $('#modalAddCoverage').UifModal('hide');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectedCoverageAlreadyAdded, 'autoclose': true });                
            }
            currentEditIndex = -1;
            idIndividualByCoverage = 0;
            participantId = 0;
        }        
    }

    static DeleteCoverage(event, data, position) {
        $.UifDialog('confirm', { 'message': Resources.Language.WantDeleteRegister + ' ' + data.CoverageName + ' - ' + data.EstimationTypeDescription + ' - ' + data.Name, 'title': Resources.Language.DeleteCoverage }, function (result) {
            if (result) {
                NoticeRequest.DeleteNoticeCoverageByCoverage(noticeId, data.CoverageId, data.IndividualId, data.EstimationTypeId).done(function (response) {
                    if (response.success) {
                        $('#coveragesTable').UifDataTable('deleteRow', position);  
                    }              
                });
            }
        });
    }


    static ValidateCoverageIndividualId() {
        if ($("#victim").val() === TypeVictim.Insured) //Si es asegurado
        {
            individualIdGeneral = individualId;
        }
        else if ($("#victim").val() === TypeVictim.Third && idIndividualByCoverage !== 0) //Si es Tercero y existe en la BDD
        {
            individualIdGeneral = idIndividualByCoverage;
        }
        else if ($("#victim").val() === TypeVictim.Third && idIndividualByCoverage === 0) //Si es Tercero y no existe en la BDD
        {
            individualIdGeneral = $("#inputThirdDocumentNumber").val();
        }

        var exist = 0;
        $("#coveragesTable").UifDataTable('getData').each(function (item) {
            if (item.IndividualId == individualIdGeneral && item.CoverageId == coverageIdCoverageDialog) {//&& edit == 0
                exist = exist + 1;
            }
        });

        return (!(exist > 0));
    }


    static LoadPolicyInfo() {
        if (policyConsult) {
            Notice.GetClaimsByPolicyId(policyId);
            $('#modalPolicyConsult').UifModal('showLocal', Resources.Language.PolicyInformation);
            $('#modalPolicyConsult .modal-dialog.modal-lg').attr('style', 'width: 55%');
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoPolicyData, 'autoclose': true });
        }
    }

    static GetClaimsByPolicyId(policyId) {
        NoticeRequest.GetClaimsByPolicyId(policyId).done(function (data) {
            if (data.success) {
                $.each(data.result, function (index, value) {
                    this.OccurrenceDate = FormatDate(this.OccurrenceDate);
                });
                $("#tblClaimsList").UifDataTable({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPolicyByEndorsementIdModuleType(endorsementId) {
        NoticeVehicleRequest.GetPolicyByEndorsementIdModuleType(endorsementId).done(function (response) {
            if (response.success) {
                $('#modalSearchbyInsured').UifModal('hide');

                policyHolderId = response.result.HolderId;
                policyId = response.result.Id;
                policyBusinessTypeId = response.result.BusinessTypeId;
                policyTypeId = response.result.PolicyTypeId;
                policyProductId = response.result.ProductId;
                policyCurrencyId = response.result.CurrencyId;
                policyCurrencyDescription = response.result.CurrencyDescription;

                //Necesario para agendamiento
                if (notice != null) {
                    notice.DocumentNumber = response.result.DocumentNumber
                }
                
                //
                $('#inputSearchPolicyByclaims').text(response.result.BranchDescription.substring(0, 3) + '-' + response.result.PrefixDescription.substring(0, 3) + '-' + response.result.DocumentNumber + '-' + response.result.EndorsementDocumentNum);
                $('.hideSearchClaim').show();

                $("#ddlBranch").UifSelect('setSelected', response.result.BranchId);
                $("#ddlPrefix").UifSelect('setSelected', response.result.PrefixId);
                $("#_policy").val(response.result.DocumentNumber);
                $("#_endorsement").val(response.result.EndorsementDocumentNum);
                _endorsementTypeId = response.result.EndorsementTypeId;
                policyFromDate = FormatDate(response.result.CurrentFrom);
                policyToDate = FormatDate(response.result.CurrentTo);
                $("#_startDate").val(policyFromDate + " - " + policyToDate);
                $('#_startInvoiced').val(response.result.IssueDate);

                Notice.ValidateOcurrenceDate(null, $("#Date").UifDatepicker('getValue'));

                //Pestaña Datos Póliza
                $('#_policyId').val(response.result.Id);
                $('#_policyHolder').val(response.result.HolderName);
                $('#_policuInsured').val('');
                $('#_policyBeneficiary').val('');
                $('#_policyIntermediary').val(response.result.Agent);
                $('#_policyEndorsement').val(response.result.EndorsementDocumentNum);
                $('#_policyType').val(response.result.PolicyType);
                $('#_policyBusinessType').val(response.result.BusinessType);
                $('#_policyIssuingDate').val(FormatDate(response.result.IssueDate));
                $('#_policyValidSince').val(FormatDate(response.result.CurrentFrom));
                $('#_policyValidTo').val(FormatDate(response.result.CurrentTo));
                $('#InputCurrency').val(response.result.CurrencyDescription);

                //Pestaña Datos Cartera 
                $('#_policyIssuedPrime').val('');
                $('#_policuTableExpenses').val('');
                $('#_policyTableTaxes').val(response.result.TaxExpenses);
                $('#_policyTotalBonus').val('');
                $('#tblPortfolio').val('');

                //Pestaña Datos Siniestro
                $('#_claimsList').val('');

                _enabled = false;
                policyConsult = true;
                $("#btnSaveClaimNotice").prop("disabled", false);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                $("#btnSaveClaimNotice").prop("disabled", true);
            }
        });
    }


    /////////////////////////////////////////////////
    // Eventos DataTable


    static ModalCoverage() {
        if ($("#_policy").val() != "") {

            if (_endorsementTypeId != EndorsementType.Cancellation) {
                
                isCreation = true;

                $("#victim").prop('disabled', false);
                $("#txtCoverage").prop('disabled', false);

                $("#victim").val(TypeVictim.Insured);
                $('#ddlEstimationType').UifSelect("setSelected", EstimateType.Compensation);

                Notice.TypeVictimSetting();

                $("#txtCoverage").UifAutoComplete('clean');
                $("#txtInsuredAmount").val("");
                $("#estimatedValue").val("");

                var insuredFullName = $('#insuredFullName').UifAutoComplete('getValue');
                var insuredDocumentNumber = $('#insuredDocumentNumber').UifAutoComplete('getValue');

                $('#inputThirdFullName').UifAutoComplete('setValue', insuredFullName);
                $('#inputThirdFullName').UifAutoComplete('disabled', true);

                $("#inputThirdDocumentNumber").UifAutoComplete('setValue', insuredDocumentNumber);
                $('#inputThirdDocumentNumber').UifAutoComplete('disabled', true);

                $('#modalAddCoverage').UifModal('showLocal', Resources.Language.AddCoverage);

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCanceledPolicy, 'autoclose': true });
            }
                                            
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoPolicyData, 'autoclose': true });
        }
    }

    /**
     * Habilita o deshabilita controles de modal de coberturas dependiendo de Tipo se Afectado: Asegurado o Tercero
     * */
    static TypeVictimSetting() {
        if ($("#victim").val() == TypeVictim.Third)  
        {	
            $("#ddlEstimationType").UifSelect("disabled", false);
            $("#inputThirdDocumentNumber").UifAutoComplete('disabled', false);
            $("#inputThirdFullName").UifAutoComplete('disabled', false);
            $("#ddlDocumentTypeCoverageModal").UifSelect("disabled", false);
            $('#inputThirdDocumentNumber').UifAutoComplete('setValue', '');
            $("#inputThirdFullName").UifAutoComplete('setValue', '');
            $("#ddlDocumentTypeCoverageModal").UifSelect("setSelected", "");
        }
        else if($("#victim").val() == TypeVictim.Insured) 
        {  
            idIndividualByCoverage = 0;
            $("#ddlEstimationType").UifSelect("disabled", true);
            var insuredFullName = $('#insuredFullName').UifAutoComplete('getValue');
            var insuredDocumentNumber = $('#insuredDocumentNumber').UifAutoComplete('getValue');

            $('#inputThirdFullName').UifAutoComplete('setValue', insuredFullName);
            $('#inputThirdFullName').UifAutoComplete('disabled', true);

            $("#inputThirdDocumentNumber").UifAutoComplete('setValue', insuredDocumentNumber);
            $('#inputThirdDocumentNumber').UifAutoComplete('disabled', true);
            $("#ddlDocumentTypeCoverageModal").UifSelect("setSelected", insuredDocumentTypeId);
            $("#ddlDocumentTypeCoverageModal").UifSelect("disabled", true);            
            $('#ddlEstimationType').UifSelect("setSelected", EstimateType.Compensation);
        }
    }

    static ModalEditCoverage(event, data, position) {

        isCreation = false;

        currentEditIndex = position;
        if (data.IsProspect) {
            participantId = data.IndividualId;
            idIndividualByCoverage = 0;
        }
        else {
            idIndividualByCoverage = data.IndividualId;
            participantId = 0;
        }               

        $("#victim").val(data.IsInsured ? TypeVictim.Insured : TypeVictim.Third);
        Notice.TypeVictimSetting();        

        $("#inputThirdFullName").UifAutoComplete('disabled', true);
        $("#victim").prop('disabled', true);
        $("#txtCoverage").prop('disabled', true);
        $("#inputThirdDocumentNumber").prop('disabled', true);
        $("#ddlDocumentTypeCoverageModal").UifSelect("disabled", true);
        $("#ddlEstimationType").UifSelect("disabled", true);

        $("#inputThirdFullName").val(data.Name);
        $("#ddlEstimationType").val(data.EstimationTypeId);
        $("#ddlDocumentTypeCoverageModal").val(data.DocumentTypeId);        
        $("#txtCoverage").val(data.CoverageName);
        $("#txtCoverage").data('info', { 'CoverageId': data.CoverageId, 'CoverageNumber': data.CoverageNumber });
        $("#estimatedValue").val(FormatMoney(data.EstimatedValue));
        $("#txtInsuredAmount").val(FormatMoney(data.InsuredAmount));
        $("#inputThirdDocumentNumber").val(data.DocumentNumber);
        
        $('#modalAddCoverage').UifModal('showLocal', Resources.Language.LabelEdit);
    }

    /////////////////////////////////////////////////
    // Validar correo
    static ValidateEmail(event, selectedItem) {
        var mail = $("#inputContactInformationMail").val();
        if (!Notice.isValidEmailAddress(mail)) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.BadMailAdderess, 'autoclose': true });
            $("#inputContactInformationMail").val("");
        }
    }

    static isValidEmailAddress(emailAddress) {
        if (emailAddress != "") {
            var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
            return pattern.test(emailAddress);
        } else {
            return true;
        }
    }

    /////////////////////////////////////////////////
    // Resetear Controles 
    static CleanFields() {
        $("#insuredDocumentNumber").UifAutoComplete('clean');
        $("#insuredFullName").UifAutoComplete('clean');
        $('#inputContactInformationName').val("");
        $('#inputContactInformationPhone').val("");
        $('#inputContactInformationMail').val("");        
        $("#ddlPrefix").UifSelect('setSelected', null);
        $("#ddlBranch").UifSelect('setSelected', null);       
        $("#_policy").val("");
        $("#_endorsement").val("");
        $("#_startDate").val("");
        $("#_startInvoiced").val("");
        $('#InputCurrency').val("");
        
        //Botones
        $("#btnObjectClaimNotice").prop("disabled", true);
        $("#btnScheduleClaimNotice").prop("disabled", true);        
        $("#btnConvertClaimNoticeToClaim").prop("disabled", true);

        //Variables ocultas para cargar coberturas
        $("#_header").val("");
        $("#_item").val("");
        $("#Number").val("");
        $("#ObjectedDescription").val("");        
        $("#Date").val("");
        $("#_noticeDate").UifDatepicker("setValue", "");
        $("#_noticeDate").UifDatepicker('disabled',false);
        $("#InputTimeIncident").UifTimepicker("clear");
        $("#Location").val("");     
        //$('#Country').UifSelect('setSelected', null);
        $("#City").UifSelect();
        $("#State").UifSelect();
                
        $("#ClaimNoticeReasonOthers").val("");
        $("#Description").val("");
        $("#ClaimedAmount").val("");
        $("#Name").val("");
        $("#Phone").val("");
        $("#OthersAffected").val("");
        $('#inputInternalConsecutive').val("");
        $("#Mail").val("");
                
        AffectedDocumentNumber = "";
        plate = "";
        _enabled;
        _coveredRiskType = null;
        _endorsementId = null;
        policyHolderId = 0;
        policyId = null;
        riskId = null;
        policyBusinessTypeId = 0;
        policyTypeId = 0;
        policyProductId = 0;
        policyCurrencyId = 0;
        policyCurrencyDescription = null;
        notice = null;

        $("#coveragesTable").UifDataTable('clear');
        $('.hideSearchClaim').hide();               

        switch (glNoticeOption) {
            case 1:
                NoticeVehicle.CleanFieldsVehicle();
                break;
            case 2:
                NoticeLocation.CleanFieldsLocation();
                break;
            case 3:
                NoticeSurety.CleanFieldsSurety();
                break;
            case 4:
                NoticeTransport.CleanFieldsTransport();
                break;
            case 5:
                NoticeAirCraft.CleanFieldsAirCraft();
                break;
            case 6:
                NoticeFidelity.CleanFieldsFidelity();
                break;
        }
    }

    static CleanFieldsPolicy() {
        $("#ddlPrefix").UifSelect('setSelected', null);
        $("#ddlBranch").UifSelect('setSelected', null);
        $("#_policy").val("");
        $("#_endorsement").val("");
        $("#_startDate").val("");
        $("#_startInvoiced").val("");

    }

    ///////////////////////////////////////////////
    // Utils
    LoadTime(selectId, amountItems) {
        $('#' + selectId).prop('disabled', false);
        var selectedTime = 0;
        for (var i = 0; i < amountItems; i++) {
            if (i < 10) {
                if (i == selectedTime) {
                    $('#' + selectId).append($('<option>', {
                        value: '0' + i,
                        text: '0' + i,
                        selected: true
                    }));
                }
                else {
                    $('#' + selectId).append($('<option>', {
                        value: '0' + i,
                        text: '0' + i
                    }));
                }
            }
            else {
                $('#' + selectId).append($('<option>', {
                    value: i,
                    text: i
                }));
            }
        }
    }

    /////////////////////////////////////////////////
    /// Funcion para validar los datos de los radio boton
    static ValidateRadioNotice() {
        var type = $('input[type=radio][name=MotivoAviso]:checked').val();
        switch (type) {
            //asistencia
            case '1':
                $("#ClaimNoticeReasonOthers").hide();
                break;
            //robo
            case '2':
                $("#ClaimNoticeReasonOthers").hide();
                break;
            //otros
            case '3':
                $("#ClaimNoticeReasonOthers").show();
                break;
            default:
        }
    }

    static ScheduleNotice() {
        if (notice != null) {
            $("#subject").val("Aviso de Siniestros Nro. " + notice.Number);

            $('#msj').text(Resources.Language.Policy + ': ' + notice.DocumentNumber + '\n' + Resources.Language.FullName + ': ' + notice.ContactName + '\n' + Resources.Language.Phone + ': ' + notice.PhoneNumber + '\n' + Resources.Language.Mail + ': ' + notice.Email);
            $('#dest').val(notice.Email);
            $('#modalSchedule').UifModal('showLocal', Resources.Language.AppointmentDate);
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotScheduleUnsafeNotice, 'autoclose': true });
        }
    }

    static CloseModalScheduleNotice() {
        $('#modalSchedule').UifModal('hide');
    }

    static SaveSchedule() {

        if (!CompareClaimDates($("#dateTimeIni").val(),$("#dateTimeFin").val())) {

            //SendEmail
            var subject = $("#subject").val();
            var message = $("#msj").val();
            var mailDestination = $("#dest").val();
            var startEventDate = $("#dateTimeIni").val();
            var finishEventDate = $("#dateTimeFin").val();
            var noticeNumber = notice.Number;

            ClaimSearchRequest.SendEmailToAgendNotice(subject, message, mailDestination).done(function (response) {
                if (response.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': response.result, 'autoclose': true });
                    //Agend
                    ClaimSearchRequest.ScheduleNotice(subject, message, startEventDate, finishEventDate).done(function (response) {
                        var blob = new Blob([response], { type: "application/ics" });
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        var fileName = noticeNumber + ".ics";
                        link.download = fileName;
                        link.click();
                        link.remove();
                    });
                    $("#modalSchedule").UifModal('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#dateTimeIni").val("");
            $("#dateTimeFin").val("");
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TheFinalDateLessthanInitialDate, 'autoclose': true });
        }
    }  

    static ValidateNoticeOccurrenceAndNoticeDate() {
        var occurrenceDate = $("#Date").val();
        var noticeDate = $("#_noticeDate").val();

        if (occurrenceDate && noticeDate) {
            if (CompareClaimDates(occurrenceDate, noticeDate)) {
                var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.LabelDateIncident + ': ' + (occurrenceDate + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.LabelDateNotice + ': ' + noticeDate) + '"';
                $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
                $("#Date").UifDatepicker('clear');
            }
        }
    }

    static NoticeObjection() {
        if (notice.ObjectedDescription != "" && notice.ObjectedDescription != null) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ObjectedNotice, 'autoclose': true });
            return;
        }

        if (notice != null) {
            $('#modalObjected').UifModal('showLocal', Resources.Language.ToObjectClaimNotice);
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectNoticeClaim, 'autoclose': true });
        }
    }

    static UpdateNoticeObjection() {
        notice.ObjectedDescription = $("#ClaimNoticeObject_Description").val();

        ClaimSearchRequest.UpdateObjectedClaimNotice(notice).done(function (response) {
            if (response.success) {
                $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.ObjectedNoticeSuccessfully, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }

            $("#modalObjected").UifModal('hide');
        });
    }

    static CloseModalNoticeObjection()
    {   
        $("#modalObjected").UifModal('hide');
    }

    static HideTextBoxClaimNoticeReasonOthers() {
        $("#ClaimNoticeReasonOthers").hide();
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if ($(this).val() != '')
            $(this).val(FormatMoney($(this).val().includes(',') ? $(this).val().replace(',', '.') : $(this).val()));
    }


    //Convierte el aviso en denuncia
    static ToConvertNoticeToClaim() {        
        if (notice.ObjectedDescription != null && notice.ObjectedDescription != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotDenounceObjectedNotice, 'autoclose': true });
        }
        else if (notice) {
            if (notice.RiskId == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotConvertUnsafeNoticeToClaim, 'autoclose': true });
                return;
            }

            $('#modalNoticeBranch').UifModal('showLocal', Resources.Language.DeductibleNotice);
            noticeToClaim = notice;
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectNoticeClaim, 'autoclose': true });
        }
    }

    static ConvertNoticeToClaim() {
        $("#noticeBranchForm").validate();
        if ($("#noticeBranchForm").valid()) {
            $("#modalNoticeBranch").UifModal('hide');
            modelSearchNewClaimByNotice.claimBranch = $("#selectNoticeBranchSearch").UifSelect('getSelected');
                       
            switch (noticeToClaim.CoveredRiskTypeId) {
                case parseInt(Resources.Language.CoveredRiskTypeVehicle):
                    glClaimOption = 1;
                    router.run("prtClaimVehicle");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeProperty):
                    glClaimOption = 2;
                    router.run("prtClaimLocation");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeSurety):
                    if (noticeToClaim.CountryId == 0) {
                        glClaimOption = 3;
                        router.run("prtClaimSurety");
                    } else {
                        glClaimOption = 6;
                        router.run("prtClaimFidelity");
                    }
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeTransport):
                    glClaimOption = 4;
                    router.run("prtClaimTransport");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeAirCraft):
                    glClaimOption = 5;
                    router.run("prtClaimAirCraft");
                    break;
                default:
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoveredRiskTypeNotParametrized, 'autoclose': true });
                    break;
            }

            $("#btnObjectClaimNotice").hide();
            $("#btnScheduleClaimNotice").hide()
            $("#btnConvertClaimNoticeToClaim").hide();
            $("#btnCleanFormNotice").hide();
            $("#btnSaveClaimNotice").hide();
        }
    }

    static CloseModalNoticeToClaim()
    {
        $("#modalNoticeBranch").UifModal('hide');
    }


    static GetBranchesByUserId(callback) {
        ClaimSearchRequest.GetBranchesByUserId().done(function (response) {
            if (response.success) {
                if (callback)
                {
                    return callback(response.result);
                }   
                $('#selectNoticeBranchSearch').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }           

    static ValidateOcurrenceDate(event, ocurrenceDate) {
        if (policyFromDate != null && policyToDate != null && ocurrenceDate != null) {             
                       
            var policyFromDatePart = policyFromDate.split('/');
            var policyFromDateValidate = new Date(policyFromDatePart[2], policyFromDatePart[1] - 1, policyFromDatePart[0]);

            var policyToDatePart = policyToDate.split('/');
            var policyToDateValidate = new Date(policyToDatePart[2], policyToDatePart[1] - 1, policyToDatePart[0]);

            if (focusinDate == true) {
                                
                if (messageCount == 0 && (ocurrenceDate < policyFromDateValidate || ocurrenceDate > policyToDateValidate)) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.OcurrenceDateOutPolicyValidity, 'autoclose': true });
                    messageCount = 1;
                }
                else {
                    focusinDate = false;
                    messageCount = 0;
                }
            }
            else {
                if (ocurrenceDate < policyFromDateValidate || ocurrenceDate > policyToDateValidate) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.OcurrenceDateOutPolicyValidity, 'autoclose': true });                    
                }
            }
                            
        }    
    }

}

/////////////////////////////////////////////////
/// Función para crear DTO para crear el Aviso 
function SetDataInformation() {
    var contactInformationDTO = {
        Name: $("#inputContactInformationName").val(),
        Phone: $("#inputContactInformationPhone").val(),
        Mail: $("#inputContactInformationMail").val()
    };

    return contactInformationDTO;
}

function SetDataNoticeModel() {

    var NoticeDTO = {
        Id: noticeId,
        NoticeDate: $("#_noticeDate").UifDatepicker('getValue'),
        BranchId: $('#ddlBranch').UifSelect('getSelected'),
        PrefixId: $('#ddlPrefix').UifSelect('getSelected'),
        NoticeTypeId: 1,
        IndividualId: policyHolderId,
        OcurrenceDate: $("#Date").val() + (' ') + $("#InputTimeIncident").val(),
        HourOcurrence: new Date(), // Pendiente
        Location: $("#Location").is(":visible") ? $("#Location").val() : null,
        CountryId: $("#Country").is(":visible") ? $("#Country").UifSelect('getSelected') : null,
        StateId: $("#State").is(":visible") ? $("#State").UifSelect('getSelected') : null,
        CityId: $("#City").is(":visible") ? $("#City").UifSelect('getSelected') : null,
        Description: $("#Description").val(),
        PolicyId: policyId,
        PolicyCurrentFrom: $("#_policyValidSince").val(),
        PolicyCurrentTo: $("#_policyValidTo").val(),
        RiskId: riskId,
        CoveredRiskTypeId: _coveredRiskType,
        ObjectedDescription: null,
        EndorsementId: _endorsementId,
        DamageResponsibilityId: $("#ddlDamageResponsibility").UifSelect('getSelected'),
        DamageTypeId: $("#ddlDamageType").UifSelect('getSelected'),
        NoticeStateId: noticeStateId,
        NoticeReasonId: $("[Name=MotivoAviso]:checked").val(),
        NumberObjected: $("#ObjectedDescription").val(),
        OthersAffected: $("#OthersAffected").val(),
        InternalConsecutive: $("#inputInternalConsecutive").val(),
        ClaimedAmount: NotFormatMoney($("#ClaimedAmount").val()),
        ClaimReasonOthers: $("#inputdriverInformation").val(),
        Number: $("#Number").val(),
        PolicyTypeId: policyTypeId,
        PolicyBusinessTypeId: policyBusinessTypeId,
        PolicyProductId: policyProductId,
        DocumentNumber: $("#_policy").val(),
        EndorsementNumber: $("#_endorsement").val()
    };

    return NoticeDTO;
}

/////////////////////////////////////////////////
///  Arma la matriz de objetos de Coberturas para pasarlo al controlador y grabarlas
function CoverageList() {

    var coveragesDTO = $("#coveragesTable").UifDataTable('getData').map(function (item) {
        var coverage = {};
        coverage.RiskNum = riskNum;
        coverage.CoverNum = item.CoverageNumber;
        coverage.CoverageId = item.CoverageId;
        coverage.IndividualId = item.IndividualId;
        coverage.IsInsured = (item.IsInsured == TypeVictim.Insured) ? true : false;
        coverage.IsProspect = item.IsProspect;
        coverage.EstimateTypeId = NotFormatMoney(item.EstimationTypeId);
        coverage.EstimateAmount = NotFormatMoney(item.EstimatedValue);
        coverage.CoverageName = ""; 
        coverage.DocumentNumber = item.DocumentNumber;
        coverage.DocumentTypeId = item.DocumentTypeId;
        coverage.FullName = item.Name;
        coverage.CurrencyId = item.CurrencyId;
        return coverage;
    });

    return coveragesDTO;
}


$(document).ajaxSend(function (event, xhr, settings) {
    if (settings.url.indexOf("GetRiskCoveragesByDescription") != -1) {
        var parameterUrl = settings.url.split("?");
        var coberageName = settings.url.split("=");
        settings.url = parameterUrl[0] + "?riskId=" + riskId + "&description=" + coberageName[1];
    }
});
