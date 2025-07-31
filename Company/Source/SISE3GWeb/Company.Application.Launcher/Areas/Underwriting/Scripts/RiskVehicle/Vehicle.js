var individualSearchType = 1;
var IndividualType;
var isnCalculate = false;
var riskController;
var dynamicProperties = [];
var fasecoldaCode = '';
var currentSavedRisk = null;
var endorsementType = null;
var expressionPlate = '';
var riskDescriptionValue = '';

class RiskVehicle extends Uif2.Page {
    getInitialState() {       
        $(document).ajaxStop($.unblockUI);
        riskController = 'RiskVehicle';
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskVehicle", formRisk: "#formVehicle", RecordScript: false, Class: RiskVehicle, redirectData: false };
        }
        UnderwritingQuotation.DisabledButtonsQuotation();
        glbRisk.redirectData = false;
        endorsementType = parseInt(glbPolicy.Endorsement.EndorsementType, 10);
        riskDescriptionValue = Underwriting.getQueryVariable("descriptionRisk");
        $("#btnConvertProspect").hide();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputTextObservations").TextTransform(ValidatorType.UpperCase);
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("#inputPlate").ValidatorKey(7, 0, 1);
        $("#inputFasecoldaCode").ValidatorKey(1, 0, 1);
        $("#inputEngine").ValidatorKey(7, 0, 1);
        $("#inputEngine").ValidatorKey(7, 0, 1);
        $("#inputChassis").ValidatorKey(7, 0, 1);
        $("#inputPrice").OnlyDecimals(UnderwritingDecimal);
        $("#inputPriceAccesories").OnlyDecimals(UnderwritingDecimal);
        $("#inputRate").OnlyDecimals(UnderwritingDecimal);
        $("#inputInsured").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $("#inputPriceAccesories").val(0);
        $("#inputPremium").text(0);
        $('#btnAccesories').prop('disabled', true);
        $('#inputPlateConfirmation').prop('disabled', true);
        $('#inputEngineConfirmation').prop('disabled', true);
        $('#inputChassisConfirmation').prop('disabled', true);
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });
        RiskVehicle.ShowPanelsRisk(MenuType.Risk);
        RiskVehicle.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification || glbPolicy.Endorsement.EndorsementType == EndorsementType.Renewal) {
            glbPolicy.Endorsement.IsCollective;
            if (glbRisk != null && glbRisk.Id != null && glbRisk.Id>0)
            $.UifDialog('confirm', { 'message': '¿Desea recuperar valor Fasecolda?' },
                function (result) {
                    if (result) {
                        RiskVehicle.GetPriceByMakeIdModelIdVersionId($("#selectMake").val(), $("#selectModel").val(), $("#selectVersion").val(), $('#selectYear').val());
                    }
                });
        }

        if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
            $("#EngineFieldRequired").removeClass("field-required");
            $("#ChassisFieldRequired").removeClass("field-required");
            $("#EngineFieldRequired2").removeClass("field-required");
            $("#ChassisFieldRequired2").removeClass("field-required");
        }
        RiskVehicle.ExpresionPlate();
        
    }

    bindEvents() {
        $("#inputPlate").focusout(RiskVehicle.FasecoldaValidation);
        $("#inputPrice").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputPrice").focusout(this.PriceFocusOut);
        $("#inputPriceAccesories").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputPriceAccesories").focusout(Underwriting.FormatMoneyOut);
        $('#selectRisk').on('itemSelected', RiskVehicle.ChangeRisk);
        $('#selectLimitRC').on('itemSelected', this.ClearCalculate);
        $("#selectRatingZone").on('itemSelected', this.ClearCalculate);
        $("#btnAddRisk").on('click', this.AddRisk);
        $("#btnDeleteRisk").on('click', RiskVehicle.DeleteRisk);
        $("#inputFasecoldaCode").focusin(this.FasecoldaCodeFocusIn);
        $("#inputFasecoldaCode").focusout(this.FasecoldaCodeFocusOut);
        $("#selectMake").on('itemSelected', this.ChangeMake);
        $("#selectModel").on('itemSelected', this.ChangeModel);
        $("#selectVersion").on('itemSelected', this.ChangeVersion);
        $('#selectYear').on('itemSelected', this.ChangeYear);
        $('#selectGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $("#btnCalculate").on('click', this.Calculate);
        $("#inputInsured").on('buttonClick', this.SearchInsured);
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividual);
        $("#btnDetail").on('click', this.ShowDetail);
        $('#listCoverages').on('rowAdd', this.AddCoverage);
        $('#listCoverages').on('rowEdit', this.EditCoverage);
        $('#listCoverages').on('rowDelete', this.CoverageDelete);
        $("#btnAccept").on('click', this.Accept);
        $("#btnClose").on('click', this.Close);
        $("#btnIndividualDetailAccept").on('click', RiskVehicle.SetIndividualDetail);
        $("#btnScript").on('click', RiskVehicle.LoadScript);
        $("#btnAcceptNewPersonOnline").click(this.AcceptNewPersonOnline);
        $('#btnConvertProspect').click(Underwriting.OpenSup);
        $("#SearchindividualId").on('buttonClick', UnderwritingTemporal.SearchByindividualId);
        $("#SearchCodeId").on('buttonClick', UnderwritingTemporal.SearchByindividualCode);
    }

    static ValidationPlate() {        
        var regexValidation = new RegExp(expressionPlate);
        ///(^[a-zA-Z]{3}[0-9]{3}|[a-zA-Z]{3}[0-9]{2}[a-zA-Z]{1}|[rRsS]{1}[0-9]{5}|[a-zA-z]{2}[0-9]{4}|[0-9]{3}[a-zA-z]{3}|[TLtl]{2}[0-9]{0,3}$)/;
        if (expressionPlate != '' && regexValidation.test($("#inputPlate").val())) {
        return true;
        } else {            
            return false;
        }
    }

    static ExpresionPlate() {

        RiskVehicleRequest.GetRegularExpression().done(function (data) {
            if (data.success) {
                expressionPlate = data.result;
            }
        });
        




    }

    static FasecoldaValidation() {
        if ($("#inputPlate").val() != "" && RiskVehicle.ValidationPlate()) { //RiskVehicle.ValidationPlate()) {
            var regexFourValues = /^(?:\D*\d){4}\D*$/
            if ($("#inputPlate").val() == "NOO" || (!$("#inputPlate").val().startsWith("TL") && regexFourValues.test($("#inputPlate").val()))) {
                $.UifNotify('show', { 'type': 'danger', 'message': "La placa ingresada no es valida", 'autoclose': true });
                $("#inputPlate").focus();
                return;
            } else {
                if ($("#inputPlate").val() == "") {//&& !regex.test($("#inputPlate").val())) {//|| !regex2.test($("#inputPlate").val())) {
                    $.UifNotify('show', { 'type': 'info', 'message': "El formato de placa no es valido", 'autoclose': true });
                    return;
                } else {
                    glbRisk.LicensePlate = $("#inputPlate").val();
                    glbRisk.ChassisSerial = $("#inputChassis").val();
                    glbRisk.EngineSerial = $("#inputEngine").val();
                    lockScreen();
                    RiskVehicleRequest.GetFasecoldaByPlate(glbRisk, glbPolicy.Branch.Id).done(function (data) {

                        if (data.success) {                         
                            $("#inputPlate").val(data.result.Plate);
                            $("#inputPlateConfirmation").val(data.result.Plate);
                            //Las placas TL en endosos de modificación pueden conservar el mismo chasis y motor si la anterio era TL
                            if (($("#inputPlate").val().startsWith("TL") && regexFourValues.test($("#inputPlate").val()) && glbPolicy.Endorsement.EndorsementType != 1)) {
                                //Conservar
                                if (glbRisk.Description !== null) {
                                    if (!(glbRisk.Description.startsWith("TL") && regexFourValues.test(glbRisk.Description))) {
                                        RiskVehicle.AddDataPlate(data);
                                    }
                                }
                                else {
                                    RiskVehicle.AddDataPlate(data);
                                }
                            } else {
                                RiskVehicle.AddDataPlate(data);
                            }

                            if (data.result.GuiedCode != null) {
                                $("#inputFasecoldaCode").val(data.result.GuiedCode);
                                RiskVehicle.FasecoldaFuntion(data.result.Model);
                            }

                        } else {
                            if (glbPolicy != null && glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
                                RiskVehicle.ClearFasecolda();
                                RiskVehicle.ClearFasecoldaPlate()
                                RiskVehicle.GetMakes();
                                if ($("#inputPlate").val().trim().length > 2 && $("#inputPlate").val() != "TL")
                                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            }
                        }                      
                    });
                }
            }
        }
    }

    static AddDataPlate(data) {
        let caracteresEspeciales = /[^a-z0-9\s]/gi;
        $("#inputEngine").val(data.result.Engine.replace(caracteresEspeciales, ''));
        $("#inputEngineConfirmation").val(data.result.Engine.replace(caracteresEspeciales, ''));
        $("#inputChassis").val(data.result.Chassis.replace(caracteresEspeciales, ''));
        $("#inputChassisConfirmation").val(data.result.Chassis.replace(caracteresEspeciales, ''));
    }

    static ClearFasecoldaPlate() {
        $("#inputEngine").val('');
        $("#inputChassis").val('');
        $("#inputPlateConfirmation").val('');
        $("#inputEngineConfirmation").val('');
        $("#inputChassisConfirmation").val('');
        $("#inputFasecoldaCode").val('');
        $('#selectMake').prop('disabled', false);
        $('#inputFasecoldaCode').prop('disabled', false);
        $('#selectModel').prop('disabled', false);
        $('#selectVersion').prop('disabled', false);
    }

    static setYear(year) {
        $("#selectYear").UifSelect("setSelected", year);
        $("#selectYear").change();
    }

    static getPersonOnline() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType,"IndividualId");
                    if (glbPersonOnline.CustomerType == CustomerType.Individual) {
                        Underwriting.ConvertProspectToInsured(glbPersonOnline.IndividualId, glbPersonOnline.DocumentNumber);
                    }
                }
                else {
                    $("#inputInsured").data("Object", null);
                    $("#inputInsured").data("Detail", null);
                    if (glbPersonOnline.DocumentNumber.length > 0) {
                        $("#inputInsured").val(glbPersonOnline.DocumentNumber);
                    }
                    else {
                        $("#inputInsured").val(glbPersonOnline.Name);
                    }
                }
                if (glbRisk.Id > 0) {
                    RiskVehicle.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskVehicle.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }



    PriceFocusOut() {
        if ($.trim($(this).val()) != "") {
            $(this).val(FormatMoney($(this).val()));
        }
        $('#inputAmountInsured').text(0);
        $('#inputPremium').text(0);
        isnCalculate = false;
    }
    
    static ChangeRisk(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicle.GetRiskById(selectedItem.Id);
            RiskVehicle.DisabledControlByEndorsementType(true);
            UnderwritingQuotation.DisabledButtonsQuotation();
        }
        else {
            RiskVehicle.ClearControls();
        }
    }

    ClearCalculate(event, selectedItem) {
        $('#inputAmountInsured').text(0);
        $('#inputPremium').text(0);
        isnCalculate = false;
    }

    AddRisk() {        
        if (glbPolicy.Endorsement.EndorsementType == 2) {
            localStorage.setItem("Status", CoverageStatus.Included);
        }
        RiskVehicle.ClearControls();
        RiskVehicle.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    FasecoldaCodeFocusIn() {
        fasecoldaCode = $("#inputFasecoldaCode").val().trim();
    }

    FasecoldaCodeFocusOut() {
        if ($("#inputFasecoldaCode").val().trim().length > 0 && $("#inputFasecoldaCode").val().trim() != fasecoldaCode) {
            RiskVehicle.GetFasecoldaByCode($("#inputFasecoldaCode").val().trim());
        }
    }

    static FasecoldaFuntion(year) {
        if ($("#inputFasecoldaCode").val().trim().length > 0 && $("#inputFasecoldaCode").val().trim() != fasecoldaCode) {
            RiskVehicle.GetFasecoldaByCode($("#inputFasecoldaCode").val().trim());
            RiskVehicle.setYear(year)
        }
    }

    ChangeMake(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicle.GetModelsByMakeId(selectedItem.Id, 0);
        }
        else {
            $('#selectModel').UifSelect();
            $('#selectVersion').UifSelect();
            $('#selectYear').UifSelect();
            $("#selectType").UifSelect();
            $('#inputPrice').val(0);
        }
        $('#inputFasecoldaCode').val('');
    }

    ChangeModel(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicle.GetVersionsByMakeIdModelId($("#selectMake").UifSelect("getSelected"), selectedItem.Id, 0);
        }
        else {
            $('#selectVersion').UifSelect();
            $('#selectYear').UifSelect();
            $("#selectType").UifSelect();
            $('#inputPrice').val(0);
        }
        $('#inputFasecoldaCode').val('');
    }

    ChangeVersion(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicle.GetYearsByMakeIdModelIdVersionId($("#selectMake").UifSelect("getSelected"), $("#selectModel").UifSelect("getSelected"), selectedItem.Id, 0);
            RiskVehicle.GetFasecoldaCodeByMakeIdModelIdVersionId($("#selectMake").UifSelect("getSelected"), $("#selectModel").UifSelect("getSelected"), $("#selectVersion").UifSelect("getSelected"));
        }
        else {
            $('#selectYear').UifSelect();
            $('#inputFasecoldaCode').val('');
            $("#selectType").UifSelect();
            $('#inputPrice').val(0);
        }
    }

    ChangeYear(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicle.GetPriceByMakeIdModelIdVersionId($("#selectMake").UifSelect("getSelected"), $("#selectModel").UifSelect("getSelected"), $("#selectVersion").UifSelect("getSelected"), selectedItem.Id);
        }
        else {
            $('#inputPrice').val(0);
        }
        $('#inputPremium').text(0);
        isnCalculate = false;
    }

    ChangeGroupCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            lockScreen();
            RiskVehicle.GetCoveragesByGroupCoverageId(selectedItem.Id);
        }
        else {
            $("#listCoverages").UifListView("refresh");            
        }
        $('#inputPremium').text(0);
        $("#inputAmountInsured").text(0);
    }

    Calculate(event) {
        lockScreen();
        if ($("#formVehicle").valid()) {
            isnCalculate = true;
            $('#inputPlateConfirmation').val($('#inputPlate').val());
            $('#inputEngineConfirmation').val($('#inputEngine').val());
            $('#inputChassisConfirmation').val($('#inputChassis').val());
            RiskVehicle.GetPremium();
        }
    }

    SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);
        }
    }

    SelectIndividual(e) {
        if (individualSearchType == 2) {
            RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
            //RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
        }
        else {
            RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        }
        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    ShowDetail() {
        if ($("#inputInsured").data("Object") != undefined) {
            RiskVehicle.ShowInsuredDetail();
        }
    }

    AddCoverage(event) {
        glbRisk.redirectController = MenuType.Coverage;
        if (glbPolicy.TemporalType != TemporalType.Quotation) {
            RiskVehicle.SaveRisk(MenuType.Coverage, 0);
        }
    }

    EditCoverage(event, data, index) {
        if (data.IsVisible || data.Id != 0) {
            glbRisk.redirectController = MenuType.Coverage;
            RiskVehicle.SaveRisk(MenuType.Coverage, data.Id);
        }
        else {
            showInfoToast(AppResources.MessageEditCoverage)
            return false;
        }

    }

    CoverageDelete(event, data) {
        RiskVehicle.DeleteCoverage(data);
    }

    Accept() {    
        if ($("#formVehicle").valid())
        {
            lockScreen();
            $('#inputPlateConfirmation').val($('#inputPlate').val());
            $('#inputEngineConfirmation').val($('#inputEngine').val());
            $('#inputChassisConfirmation').val($('#inputChassis').val());
            localStorage.removeItem("Status");
            glbRisk.redirectController = MenuType.Risk;
            RiskVehicle.SaveRisk(MenuType.Risk, 0);
            ScrollTop();
            
        }        
    }

    Close() {
        localStorage.removeItem("Status");
        glbRisk = { Id: 0, Object: "RiskVehicle", Class: RiskVehicle };
        lockScreen();
        RiskVehicle.UpdatePolicyComponents();
    }

    AcceptNewPersonOnline() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskVehicle.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsured").val().trim());
    }

    ConvertProspect() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskVehicle.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsured").data("Object").IndividualId, $("#inputInsured").data("Object").IndividualType, $("#inputInsured").data("Object").CustomerType, 0);
    }

    static HidePanelsRisk(Menu) {

        switch (Menu) {
            case MenuType.Risk:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('hide');
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('hide');
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").UifModal('hide');
                break;
            case MenuType.AdditionalData:
                $("#modalAdditionalData").UifModal('hide');
                break;
            default:
                break;
        }
    }

    static ShowPanelsRisk(Menu) {
        switch (Menu) {
            case MenuType.Risk:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('showLocal', AppResources.LabelDataTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                break;
            case MenuType.AdditionalData:
                $('#modalAdditionalData').UifModal('showLocal', AppResources.LabelTitleAdditionalData + ': ' + $('#inputPlate').val());
                break;
            case MenuType.Script:
                RiskVehicle.LoadScript();
                break;
            default:
                break;
        }
    }

    static GetRisksByTemporalIdQuotation(temporalId, individualId, documentNumber) {
        RiskVehicleRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {

                if (data.result != null) {
                    var Riskid = data.result[0].Id;
                    if (data.result[0].Id > 0) {

                        RiskVehicleRequest.GetRiskById(glbPolicy.Endorsement.EndorsementType, glbPolicy.Id, Riskid).done(function (datar) {
                            if (datar.success) {

                                if (datar.result != null) {
                                    var risk = datar.result.Risk;
                                    /*if (risk.MainInsured.CustomerType == CustomerType.Prospect) {
                                        UnderwritingRequest.ConvertProspectToInsured(glbPolicy.Id, individualId, documentNumber, Underwriting.GetControllerRisk());

                                    }*/

                                }
                            }
                        });
                    }
                }
            }
        });
    }


    static GetRisksByTemporalId(temporalId, selectedId) {
        lockScreen();
        RiskVehicleRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {

                if (data.result != null) {
                    $('#selectRisk').UifSelect({ sourceData: data.result });
                    if (data.result.length == 1 && selectedId == 0) {
                        selectedId = data.result[0].Id;
                        glbRisk.Id = selectedId;

                        if (glbPolicy.IsCollective != null && glbPolicy.IsCollective && glbPolicy.Endorsement != null && glbPolicy.Endorsement.EndorsementReasonId != null && (glbPolicy.Endorsement.EndorsementReasonId == 1 || glbPolicy.Endorsement.EndorsementReasonId == 9))
                        {
                            selectedId = 0;
                            glbRisk.Id = 0; 
                        }
                    }
                    if (data.result.length > 0) {
                        if (selectedId > 0) {
                            $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }
                }
                if (glbPersonOnline != null) {
                    RiskVehicle.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    if (riskDescriptionValue != null && riskDescriptionValue != "null" && riskDescriptionValue != '') {
                        $.each(data.result, function (index, value) {
                            if (value.Description == riskDescriptionValue) {
                                $("#selectRisk").UifSelect("setSelected", value.Id);
                                RiskVehicle.GetRiskById(value.Id);
                            }
                        })
                    } else {
                        if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective  && glbPolicy.Endorsement != null && glbPolicy.Endorsement.EndorsementReasonId != null && (glbPolicy.Endorsement.EndorsementReasonId == 1 || glbPolicy.Endorsement.EndorsementReasonId == 9)) {
                            if (glbRisk.Id > 0) {
                                RiskVehicle.GetRiskById(glbRisk.Id);
                            }
                            else {
                                RiskVehicle.SetInitialVehicle();
                            }
                        }
                        else {
                            $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                            RiskVehicle.GetRiskById($("#selectRisk option[Value!='']")[0].value);
                        }
                    }
                }

                else if (glbRisk.Id > 0) {
                    RiskVehicle.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskVehicle.SetInitialVehicle();
                }

                if (!glbPolicy.Product.IsFlatRate) {
                    $('#inputRate').prop('disabled', true);
                }

                RiskVehicle.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRisksByTemporalIdFirst(temporalId, selectedId) {
        RiskVehicleRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#selectRisk').UifSelect({ sourceData: data.result });
                    if (data.result.length == 1 && selectedId == 0) {
                        selectedId = data.result[0].Id;
                        glbRisk.Id = selectedId;
                    }
                    if (data.result.length > 0) {
                        if (selectedId > 0) {
                            $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }
                }
                if (glbPersonOnline != null) {
                    RiskVehicle.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                    RiskVehicle.GetRiskById($("#selectRisk option[Value!='']")[0].value);

                }

                else if (glbRisk.Id > 0) {
                    RiskVehicle.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskVehicle.SetInitialVehicle();
                }

                if (!glbPolicy.Product.IsFlatRate) {
                    $('#inputRate').prop('disabled', true);
                }
                RiskVehicle.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SetInitialVehicle() {
        RiskVehicle.GetMakes(0);
        RiskVehicle.GetUses(0);
        RiskVehicle.GetColors(0);
        RiskVehicle.GetListCompanyServiceType(0);
        RiskVehicle.GetRatingZonesByPrefixId(glbPolicy.Prefix.Id, 0);
        RiskVehicle.GetLimitsRcByPrefixIdProductIdPolicyTypeId(glbPolicy.Prefix.Id, glbPolicy.Product.Id, glbPolicy.PolicyType.Id, 0);
        RiskVehicle.GetGroupCoverages(glbPolicy.Product.Id, 0);
        RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        RiskVehicle.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    static GetRiskById(id) {
        if (id != null && id != "" && glbPolicy.Endorsement != null && glbPolicy != null) {
            RiskVehicleRequest.GetRiskById(glbPolicy.Endorsement.EndorsementType, glbPolicy.Id, id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Emission && data.result.CalculateMinPremium) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MinimumPremiumAplicated, 'autoclose': true });
                        }
                        glbRisk.RecordScript = true;
                        if (dynamicProperties === null || dynamicProperties.length === 0)
                            data.result.DynamicProperties = data.result.Risk.DynamicProperties;
                        RiskVehicle.LoadRisk(data.result);

                        if (glbRisk.redirectData) {
                            var events = null;
                            //lanza los eventos para la creación de el riesgo
                            if (glbRisk.InfringementPolicies != null) {
                                events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
                            }
                            if (events !== TypeAuthorizationPolicies.Restrictive) {
                                RiskVehicle.RedirectAction(glbRisk.redirectController, glbRisk.Id, glbRisk.CoverageId);
                            }
                            //fin - lanza los eventos para la creación de el riesgo
                        }

                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
            });
        }
    }

    static LoadRisk(riskData) {                
        RiskVehicle.LoadRiskCombos(riskData, glbPolicy);
        if (riskData.Risk.Policy != null && riskData.Risk.Id > 0 &&
            (riskData.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Modification &&
            !riskData.LicensePlate.toUpperCase().startsWith("TL")) ||
            (riskData.Risk.Policy.Endorsement != null && riskData.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Renewal))
        {
            $("#inputPlate").prop("disabled", true)
        }
        $("#inputPlate").val(riskData.LicensePlate);
        $("#inputPlateConfirmation").val(riskData.LicensePlate);
        if (riskData.Fasecolda != null) {
            $("#inputFasecoldaCode").val(riskData.Fasecolda.Description);
        }
        if (riskData.Year != 0) {

            $("#selectYear").UifSelect("setSelected", riskData.Year);
        }

        if (riskData.IsNew) {
            $('#chkIsNew').prop('checked', true);
        }
        else {
            $('#chkIsNew').prop('checked', false);
        }
        $("#inputEngine").val(riskData.EngineSerial);
        $("#inputEngineConfirmation").val(riskData.EngineSerial);
        $("#inputChassis").val(riskData.ChassisSerial);
        $("#inputChassisConfirmation").val(riskData.ChassisSerial);

        $("#inputPrice").val(FormatMoney(riskData.Price));
        $("#hiddenStandardVehiclePrice").val(riskData.StandardVehiclePrice);
        $("#inputPriceAccesories").val(FormatMoney(riskData.PriceAccesories));
        if (riskData.Rate < 0)
            $("#inputRate").val(FormatMoney(riskData.Rate * -1));
        else
            $("#inputRate").val(FormatMoney(riskData.Rate));
        

        if (riskData.Risk.MainInsured != null && riskData.Risk.MainInsured.IndividualId > 0) {
            riskData.Risk.MainInsured.details = riskData.Risk.MainInsured.CompanyName;
            RiskVehicle.LoadInsured(riskData.Risk.MainInsured);
        }

        $("#inputPremium").text(FormatMoney(riskData.Risk.Premium, 2));

        if (riskData.Risk.Coverages != null) {
            RiskVehicle.LoadCoverages(riskData.Risk.Coverages, riskData.Risk.GroupCoverage.Id);
        }
        else if (riskData.Risk.GroupCoverage != null) {
            RiskVehicle.GetCoveragesByGroupCoverageId(riskData.Risk.GroupCoverage.Id);
        }

        $("#listAccesories").UifListView({ displayTemplate: "#accessoryTemplate", edit: true, delete: true, customEdit: true, customDelete: true, height: 200 });
        if (riskData.Accesories != null) {
            $.each(riskData.Accesories, function (index, value) {
                if (this.Amount != null) {
                    this.Amount = FormatMoney(this.Amount);
                }
                else {
                    this.Amount = 0;
                }

                if (this.Premium != null) {
                    this.Premium = FormatMoney(this.Premium);
                }
                else {
                    this.Premium = 0;
                }

                if (this.Rate != null) {
                    this.Rate = FormatMoney(this.Rate);
                }
                else {
                    this.Rate = 0;
                }

                if (this.AccumulatedPremium != null) {
                    this.AccumulatedPremium = FormatMoney(this.AccumulatedPremium);
                }
                else {
                    this.AccumulatedPremium = 0;
                }

                if (this.IsOriginal) {
                    this.Original = "Original";
                }
                else {
                    this.Original = "No Original";
                }

                switch (this.Status) {
                    case CoverageStatus.Original:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Original)];
                        break;
                    case CoverageStatus.Included:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Included)];
                        break;
                    case CoverageStatus.Excluded:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Excluded)];
                        break;
                    case CoverageStatus.Modified:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Modified)];
                        break;
                    case CoverageStatus.NotModified:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.NotModified)];
                        break;
                    case CoverageStatus.Cancelled:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Cancelled)];
                        break;
                    case CoverageStatus.Rehabilitated:
                        this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Rehabilitated)];
                        break;
                }
            });
            riskData.Risk.Accesories = riskData.Accesories;
            RiskVehicleAccessories.LoadAccessories(riskData.Accesories);
            RiskVehicleAccessories.UpdateSummaryAccessories();
        }

                $("#hiddenOriginalPrice").val(riskData.OriginalPrice);
                $("#hiddenOriginalRiskId").val(riskData.Risk.RiskId);
                $("#inputAdditionalDataPrice").val(riskData.NewPrice);



                if (riskData.Version != null) {
                    if (riskData.Version.Body != null) {
                        RiskVehicleAdditionalData.GetBodies(riskData.Version.Body.Id);
                    }
                }

                if (riskData.Risk.Premium == 0 && (riskData.Risk.Status == RiskStatus.Original || riskData.Risk.Status == RiskStatus.Included)) {
                    isnCalculate = false;
                }
                else {
                    isnCalculate = true;
                }

                if (glbRisk.RecordScript !== undefined)
                    riskData.RecordScript = glbRisk.RecordScript;

                if (riskData.DynamicProperties !== undefined)
                    dynamicProperties = riskData.DynamicProperties;

        RiskVehicle.UpdateGlbRisk(riskData);
        RiskVehicle.LoadSubTitles(0);        
    }

    static LoadCoverages(coverages, groupCoverageId) {
        var insuredAmount = 0;
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });
        Object.keys(coverages).forEach(function (index, val) {
            insuredAmount += coverages[index].LimitAmount;
            coverages[index].LimitAmount = FormatMoney(coverages[index].LimitAmount);
            coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
            coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);

            if (coverages[index].RateType == EnumRateType.Importe) {
                coverages[index].DisplayRate = FormatMoney(coverages[index].Rate);
                coverages[index].Rate = FormatMoney(coverages[index].Rate);
            } else {
                UnderwritingRequest.GetRoundRate(RoundDecimal, coverages[index].Rate).done(function (data) {
                    if (data.success) {
                        coverages[index].DisplayRate = FormatMoney(data.result);
                        coverages[index].Rate = FormatMoney(coverages[index].Rate);
                    }
                });

            }


            coverages[index].CurrentFrom = FormatDate(coverages[index].CurrentFrom);
            coverages[index].CurrentTo = FormatDate(coverages[index].CurrentTo);
            coverages[index].CurrentFromOriginal = FormatDate(coverages[index].CurrentFromOriginal);
            coverages[index].CurrentToOriginal = FormatDate(coverages[index].CurrentToOriginal);
            if (coverages[index].Deductible != null) {
                coverages[index].DeductibleDescription = coverages[index].Deductible.Description,
                    coverages[index].DeductibleId = coverages[index].Deductible.Id
            }

            if (coverages[index].DisplayRate != null) {
                var indexVar = coverages[index].DisplayRate.toString().indexOf('.');

                if (indexVar == -1) {
                    indexVar = coverages[index].DisplayRate.toString().indexOf(',');
                    if (indexVar == -1) {
                        coverages[index].DisplayRate = FormatMoney(coverages[index].DisplayRate);
                    }
                }
            }

        });
        $("#listCoverages").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });

        if ($("#selectGroupCoverage").UifSelect("getSelected") != null) {
            RiskVehicle.EnabledButtonAccessories(groupCoverageId);
        }
        $('#inputAmountInsured').text(FormatMoney(insuredAmount));
        $('#inputPremium').text();
    }

    static GetFasecoldaByCode(code) {
        if (code.length == 8) {
            RiskVehicleRequest.GetFasecoldaByCode(code, 0).done(function (data) {
                if (data.success) {
                    RiskVehicle.LoadVehicleByFasecolda(data.result);
                }
                else {
                    RiskVehicle.ClearFasecolda();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                RiskVehicle.ClearFasecolda();
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchFasecolda, 'autoclose': true });
            });
        }
    }

    static LoadVehicleByFasecolda(vehicle) {
        RiskVehicle.GetMakes(vehicle.Make.Id);
        RiskVehicle.GetModelsByMakeId(vehicle.Make.Id, vehicle.Model.Id);
        RiskVehicle.GetVersionsByMakeIdModelId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
        RiskVehicle.GetYearsByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id, vehicle.Year);
        if (vehicle.Version.Type != null) {
            RiskVehicle.GetTypesByTypeId(vehicle.Version.Type.Id, vehicle.Version.Type.Id);
        }
        if (vehicle.Year == 0) {
            RiskVehicle.GetPriceByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id, 0);
        }
        if (vehicle.Version.Fuel != null) {
            RiskVehicleAdditionalData.GetFuels(vehicle.Version.Fuel.Id);

        }
    }

    static GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId) {
        RiskVehicleRequest.GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId).done(function (data) {
            if (data.success) {
                $("#inputFasecoldaCode").val(data.result.Fasecolda.Description);
                RiskVehicle.GetTypesByTypeId(data.result.Version.Type.Id, data.result.Version.Type.Id);
                RiskVehicle.GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, 0);
                if (data.result.Version.Fuel != null) {
                    RiskVehicleAdditionalData.GetFuels(data.result.Version.Fuel.Id);
                }
                if (data.result.Version.Body != null) {
                    RiskVehicleAdditionalData.GetBodies(data.result.Version.Body.Id);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchFasecolda, 'autoclose': true });
        });
    }

    static GetModelsByMakeId(makeId, selectedId) {
        RiskVehicleRequest.GetModelsByMakeId(makeId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectModel").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectModel").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    if (endorsementType == EndorsementType.Renewal) {
                        $('#selectModel').UifSelect('disabled', 'disabled');
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetVersionsByMakeIdModelId(makeId, modelId, selectedId) {
        RiskVehicleRequest.GetVersionsByMakeIdModelId(makeId, modelId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectVersion").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectVersion").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId, selectedId) {
        RiskVehicleRequest.GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectYear").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectYear").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year) {
        RiskVehicleRequest.GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year).done(function (data) {
            if (data.success) {
                if (year == 0) {
                    $("#inputAdditionalDataPrice").val(FormatMoney(data.result));
                }
                else {
                    $("#hiddenStandardVehiclePrice").val(data.result);
                    $("#inputPrice").val(FormatMoney(data.result));
                }

                $('#inputAmountInsured').text();
                $('#inputPremium').text();
                isnCalculate = false;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchPrice, 'autoclose': true });
        });
    }

    static GetTypesByTypeId(typeId, selectedId) {
        RiskVehicleRequest.GetTypesByTypeId(typeId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $('#selectType').UifSelect('disabled', 'disabled');
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRatingZonesByPrefixId(prefixId, selectedId) {
        RiskVehicleRequest.GetRatingZonesByPrefixId(prefixId).done(function (data) {
            if (data.success) {

                UnderwritingQuotation.DisabledButtonsQuotation();
                if (selectedId == 0) {
                    $("#selectRatingZone").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRatingZone").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId, selectedId) {
        RiskVehicleRequest.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $.each(data.result, function (id, item) {
                        if (item.IsDefault == true) {
                            selectedId = item.Id;
                            return;
                        }
                    });
                    if (selectedId == 0) {
                        $("#selectLimitRC").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectLimitRC").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
                else {
                    $("#selectLimitRC").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetGroupCoverages(productId, selectedId) {
        RiskVehicleRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    RiskVehicle.EnabledButtonAccessories(selectedId);
                    UnderwritingQuotation.DisabledButtonsQuotation();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetCoveragesByGroupCoverageId(groupCoverageId) {
        RiskVehicleRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, groupCoverageId).done(function (data) {
            if (data.success) {
                RiskVehicle.LoadCoverages(data.result, groupCoverageId);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
    }

    static GetListCompanyServiceType(selectedId) {
        var dfd = $.Deferred();
        RiskVehicleRequest.GetListServiceType().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectServiceType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectServiceType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                dfd.reject();
            }
        });
        return dfd.promise();
    }

    static EnabledButtonAccessories(groupCoverageId) {
        RiskVehicleRequest.GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, groupCoverageId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                if (data.result == true) {
                    $('#btnAccesories').prop('disabled', false);
                }
                else {
                    $('#btnAccesories').prop('disabled', true);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
    }

    static GetPremium() {
        if ($("#inputRate").val() != "") {
            if ($('#inputInsured').data("Object") != null) {
                $("#formVehicle").validate();                

                if ($("#formVehicle").valid()) {
                    lockScreen();
                    var recordScript = false;

                    if (glbPolicy.Product.IsFlatRate.toString() == 'true' && $('#inputRate').val() == 0) {                        
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDocumentControlRate, 'autoclose': true });
                    }
                    if (glbPolicy.Product.IsFlatRate.toString() != 'false' && $('#inputRate').val() > 0) {                        
                        recordScript = true;
                    }

                    if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0) {                        
                        recordScript = true;
                    }
                    else {                        
                        recordScript = glbRisk.RecordScript;

                        if (glbRisk.RecordScript && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && glbRisk.Status == RiskStatus.NotModified)
                            glbRisk.Status = RiskStatus.Modified;
                    }

                    if (recordScript) {
                        lockScreen();
                        var riskData = RiskVehicle.GetRiskDataModel();
                        var additionalData = RiskVehicle.GetAdditionalDataModel();
                        RiskVehicleRequest.GetPremium(glbPolicy.Id, riskData, dynamicProperties, additionalData).done(function (data) {                            
                            if (data.success) {
                                if (data.result.CalculateMinPremium) {
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MinimumPremiumAplicated, 'autoclose': true });
                                }                                
                                RiskVehicle.LoadRisk(data.result);
                            }
                            else {
                                $("#inputPremium").text(0);
                                $("#inputAmountInsured").text(0);
                                isnCalculate = false;
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        }).fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculatePremium, 'autoclose': true });
                        });
                    }
                    else {
                        RiskVehicle.LoadScript();
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorPercentageRate, 'autoclose': true });
            }
        }        
    }
    
    static LoadInsured(insured) {
        if (individualSearchType == 1) {
            $("#inputInsured").data("Object", insured);
            $("#inputInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');

            if (insured.CustomerType == CustomerType.Individual) {
                RiskVehicle.GetDetailsByIndividualId(insured)
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        else if (individualSearchType == 2) {
            $("#inputBeneficiaryName").data("Object", insured);
            $("#inputBeneficiaryName").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                RiskVehicle.GetDetailsByIndividualId(insured)
                //$("#inputBeneficiaryName").data("Detail", RiskVehicle.GetIndividualDetails(insured.CompanyName));
                $("#btnConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        else if (individualSearchType == 3) {
            $("#inputAdditionalDataInsured").data("Object", insured);
            $("#inputAdditionalDataInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
        }
    }

    static GetDetailsByIndividualId(insured) {
        if (individualSearchType == 2) {
            $("#inputBeneficiaryName").data('Detail', RiskVehicle.GetIndividualDetails(insured.details));
        }
        else {
            $("#inputInsured").data('Detail', RiskVehicle.GetIndividualDetails(insured.details));
        }
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType,searchBy) {
       
        var descrip = parseInt(description) || description;

        if (typeof (descrip) !== "number" && descrip.length < 3) {
            showInfoToast(AppResources.HolderSearchMinLength);
            return;
        }

        if (customerType == null && glbPolicy.TemporalType == TemporalType.Policy) {
            customerType = CustomerType.Individual;
        }
        
        if (insuredSearchType === 2) {
            HoldersRequest.GetHoldersByIndividualId(description, customerType).done(function (resp) {
                if (resp.success) {
                    let insured = resp.result.holder
                    insured.details = resp.result.details || [insured.CompanyName]
                    IndividualType = insured.IndividualType
                    RiskVehicle.LoadInsured(insured);
                } else {
                    if (individualSearchType == 1) {
                        $('#inputInsured').data("Object", null);
                        $('#inputInsured').data("Detail", null);
                        $('#inputInsured').val('');
                    }
                    else if (individualSearchType == 3) {
                        $('#inputBeneficiaryName').data("Object", null);
                        $('#inputBeneficiaryName').data("Detail", null);
                        $('#inputBeneficiaryName').val('');
                    }
                    else if (individualSearchType == 3) {
                        $('#inputAdditionalDataInsured').data("Object", null);
                        $('#inputAdditionalDataInsured').data("Detail", null);
                        $('#inputAdditionalDataInsured').val('');
                    }
                    showInfoToast(resp.result);
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                showErrorToast(AppResources.ErrorConsultingInsured);
            });
                
        } else {
            
            HoldersRequest.GetHoldersByDescription(description, customerType)
                .done(function (resp) {
                    if (resp.success) {
                        if (resp.result.length == 0) {
                            showErrorToast(AppResources.MessagePerson)
                        }
                        else if (resp.result.length == 1) {
                            let insured = resp.result[0]
                            insured.details = [insured.CompanyName]
                            IndividualType = insured.IndividualType
                            RiskVehicle.LoadInsured(insured);

                        }
                        else if (resp.result.length > 1) {
                            var dataList = [];
                            modalListType = 1;
                            for (var i = 0; i < resp.result.length; i++) {
                                dataList.push({
                                    Id: resp.result[i].IndividualId,
                                    CustomerType: resp.result[i].CustomerType,
                                    Code: resp.result[i].InsuredId,
                                    DocumentNum: resp.result[i].IdentificationDocument.Number,
                                    Description: resp.result[i].Name,
                                    CustomerTypeDescription: resp.result[i].CustomerTypeDescription,
                                    DocumentType: resp.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            RiskVehicle.ShowModalList(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                        }
                    } else {
                        showErrorToast(resp.result)
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    showErrorToast(AppResources.ErrorSearchHolders);
                }); 
        }
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static ShowInsuredDetail() {
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($("#inputInsured").data("Object").CustomerType == CustomerType.Individual) {
            if ($("#inputInsured").data('Detail') != null) {
                var insuredDetails = $("#inputInsured").data('Detail');

                if (insuredDetails.length > 0) {
                    $.each(insuredDetails, function (id, item) {
                        $('#tableIndividualDetails').UifDataTable('addRow', item)
                        if ($("#inputInsured").data("Object").CompanyName.NameNum > 0 && $("#inputInsured").data("Object").CompanyName.NameNum == this.NameNum) {
                            $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                            $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                        else if ($("#inputInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain == true) {
                            $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                            $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                    });

                    if ($('#tableIndividualDetails').UifDataTable('getSelected') == null) {
                        $('#tableIndividualDetails tbody tr:eq(0)').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(0) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                }
            }

            $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelInsuredDetail);
        }
    }

    static SaveRisk(redirec, coverageId) {
        var AmountAccesoriesNew = 0;
        var AmountAccesoriesOld = 0;
        
        if ($("#formVehicle").valid()) {            

            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Renewal && $("#inputPlate").val().trim() == "TL" ) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidationPlateTL, 'autoclose': true });
                return;
            }

            var riskData = RiskVehicle.GetRiskDataModel();
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && riskData.AmountInsured > 0) {
                isnCalculate = true;
            }
            if (isnCalculate === true) {
                var additionalData = RiskVehicle.GetAdditionalDataModel();
                RiskVehicleRequest.SaveRisk(glbPolicy.Id, riskData, dynamicProperties, additionalData).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            currentSavedRisk = data.result.Risk;
                            if (data.result.Alerts != null && data.result.Alerts.length > 0) {
                                if (data.result.Alerts[0].startsWith("**")) {
                                    //mostrar como cuadro de dialogo
                                    $.UifDialog('alert', { 'message': data.result.Alerts[0] },
                                        function (result) {
                                            //No hacer nada, sólo informativo
                                        });
                                    $('.modal-body.modal-body-dialog-alert p').prop('style', 'white-space: pre-line')
                                } else {
                                    showInfoToast(data.result.Alerts[0])
                                }
                            }
                            RiskVehicle.UpdateGlbRisk(data.result);
                            RiskVehicle.LoadSubTitles(0);
                            RiskVehicle.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                        }
                        else {
                            showErrorToast(AppResources.ErrorSaveRisk);
                        }
                    }
                    else {             
                        if (Array.isArray(data.result)) {
                            $.each(data.result, function (key, value) {
                                showErrorToast(value)
                            });
                        }
                        else {
                            if (data.result.startsWith("**")) {
                                $.UifDialog('alert',
                                    { 'message': data.result },
                                    function (result) {
                                        //No hacer nada, sólo informativo
                                    });
                                $('.modal-body.modal-body-dialog-alert p').prop('style', 'white-space: pre-line')
                            }
                            else {
                                showErrorToast(data.result)
                            }
                        }
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {                    
                    showErrorToast(AppResources.ErrorSaveRisk);
                });
            }
            else {
                showInfoToast(AppResources.FirstValueRiskCalculated);
            }
        }
        
    }

    static GetRiskDataModel() {
        var riskData = $("#formVehicle").serializeObject();
        riskData.MakeDescription = $("#selectMake").UifSelect('getSelectedText');
        riskData.ModelDescription = $("#selectModel").UifSelect('getSelectedText');
        riskData.VersionDescription = $("#selectVersion").UifSelect('getSelectedText');
        riskData.IsNew = $('#chkIsNew').is(':checked');
        if ($('#inputInsured').data("Object") != null) {
            riskData.InsuredName = ReplaceCharacter($('#inputInsured').data("Object").Name);
            riskData.InsuredId = $('#inputInsured').data("Object").IndividualId;
            if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CustomerType != 0)
                riskData.InsuredCustomerType = $("#inputInsured").data("Object").CustomerType;
            else
                riskData.InsuredCustomerType = CustomerType.Individual;


            riskData.InsuredBirthDate = FormatDate($("#inputInsured").data("Object").BirthDate);
            riskData.InsuredGender = $("#inputInsured").data("Object").Gender;
            if ($("#inputInsured").data("Object").IdentificationDocument != null) {
                riskData.InsuredDocumentNumber = $("#inputInsured").data("Object").IdentificationDocument.Number;
            }
            if ($("#inputInsured").data("Object").CompanyName != null) {
                riskData.InsuredDetailId = $("#inputInsured").data("Object").CompanyName.NameNum;
                riskData.InsuredAddressId = $("#inputInsured").data("Object").CompanyName.Address.Id;
                if ($("#inputInsured").data("Object").CompanyName.Phone != null) {
                    riskData.InsuredPhoneId = $("#inputInsured").data("Object").CompanyName.Phone.Id;
                }
                if ($("#inputInsured").data("Object").CompanyName.Email != null) {
                    riskData.InsuredEmailId = $("#inputInsured").data("Object").CompanyName.Email.Id;
                }
            }
        }
        riskData.Price = NotFormatMoney($("#inputPrice").val());
        riskData.StandardVehiclePrice = $("#hiddenStandardVehiclePrice").val();
        riskData.PriceAccesories = NotFormatMoney($("#inputPriceAccesories").val());
        riskData.AmountInsured = NotFormatMoney($("#inputAmountInsured").text());
        riskData.Premium = NotFormatMoney($("#inputPremium").text());
        riskData.Status = glbRisk.Status;
        var coveragesValues = $("#listCoverages").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentFromOriginal = Isnull(FormatDate(this.CurrentFromOriginal)) ? FormatDate(this.CurrentFrom) : FormatDate(this.CurrentFromOriginal);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.CurrentToOriginal = Isnull(FormatDate(this.CurrentToOriginal)) ? FormatDate(this.CurrentTo) : FormatDate(this.CurrentToOriginal);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            if (this.RateType == EnumRateType.Importe) {
                this.Rate = NotFormatMoney(this.Rate);
                this.OriginalRate = NotFormatMoney(this.OriginalRate);
            } else {
                this.Rate = this.Rate;
            }

        });
        riskData.Coverages = coveragesValues;
        var accessories = glbRisk.Accesories;
        if (glbRisk.Accesories != null && $("#listAccesories").UifListView('getData').length == 0)
            accessories = glbRisk.Accesories;
        else
            accessories = $("#listAccesories").UifListView('getData');
        if (accessories != null) {
            $.each(accessories, function (key, value) {
                this.Amount = NotFormatMoney(this.Amount);
                this.Premium = NotFormatMoney(this.Premium);
                this.Rate = this.Rate;
                this.AccumulatedPremium = NotFormatMoney(this.AccumulatedPremium);
                this.Status = parseInt(this.Status)
            });

        }
        riskData.Accessories = accessories;
        riskData.Fuel = $("#selectFuelType").UifSelect("getSelected");
        riskData.Body = $("#selectBodyType").UifSelect("getSelected");
        riskData.AdditionalPrice = NotFormatMoney($("#inputAdditionalDataPrice").val());
        riskData.Clauses = glbRisk.Clauses;
        riskData.Text = glbRisk.Text;      
        riskData.Beneficiaries = glbRisk.Beneficiaries;  
        if (riskData.Beneficiaries != null) 
        $.each(riskData.Beneficiaries, function (key, value) {
            this.DeclinedDate = FormatDate(this.DeclinedDate);
        });
        
        riskData.RiskId = glbRisk.Id;
        return riskData;
    }

    static ReturnUnderwriting() {
        var url = '';

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "VehicleModification";
            router.run("prtModification")
        }
        else {
            if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
                router.run("prtQuotation");
            }
            else {
                router.run("prtTemporal");
            }
        }
    }

    static ReturnCoverage(coverageId) {
        glbCoverage = {
            CoverageId: coverageId,
            Class: RiskVehicleCoverage
        }
        router.run("prtCoverageRiskVehicle");
    }

    static DeleteRisk() {
        if ($("#selectRisk").UifSelect("getSelected") > 0) {            
            lockScreen();
            RiskVehicleRequest.DeleteRisk(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                        RiskVehicle.ClearControls();
                        RiskVehicle.GetRisksByTemporalId(glbPolicy.Id);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                        ScrollTop();
                    }
                    else {
                        var riskId = $("#selectRisk").UifSelect("getSelected");
                        RiskVehicle.GetRisksByTemporalId(glbPolicy.Id, riskId);
                        RiskVehicle.GetRiskById(riskId);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }              
            }).fail(function (jqXHR, textStatus, errorThrown) {            
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDeleteRisk, 'autoclose': true });
            });            
        }   
    }

    static ClearControls() {


        $('#inputPlate').prop('disabled', false);
        $('#inputFasecoldaCode').prop('disabled', false);
        $('#selectMake').prop('disabled', false);
        $('#selectModel').prop('disabled', false);
        $('#selectType').prop('disabled', false);
        //$('#inputEngine').prop('disabled', false);
        //$('#inputChassis').prop('disabled', false);
        $('#selectGroupCoverage').prop('disabled', false);
        $('#inputInsured').prop('disabled', false);
        $("#selectRisk").UifSelect("setSelected", null);
        $("#inputPlate").val('');
        fasecoldaCode = '';
        $("#inputFasecoldaCode").val('');

        $("#selectMake").UifSelect("setSelected", null);
        $('#selectModel').UifSelect();
        $('#selectVersion').UifSelect();

        $("#selectUse").UifSelect("setSelected", null);
        $("#selectYear").UifSelect();
        $('#chkIsNew').prop('checked', false);
        $("#inputEngine").val('');
        $("#inputChassis").val('');
        $("#selectColor").UifSelect("setSelected", null);
        $("#selectRatingZone").UifSelect("setSelected", null);
        $("#inputPrice").val('');
        $("#hiddenStandardVehiclePrice").val('');
        $("#inputPriceAccesories").val('');
        $("#selectGroupCoverage").UifSelect("setSelected", null);
        $("#listCoverages").UifListView("refresh");
        $("#inputRate").val(0);
        $("#inputAmountInsured").text(0);
        $("#inputPremium").text(0);
        $("#inputPriceAccesories").val(0);
        $("#listAccesories").UifListView({ source: null, displayTemplate: "#accessoryTemplate", edit: true, delete: true, customEdit: true, customDelete: true, height: 200 });
        $("#hiddenTotalNoOriginals").val(0);
        $("#hiddenAccumulatedPremium").val(0);

        $("#selectedBeneficiaries").text("");
        $("#selectedAdditionalData").text("");
        $("#selectedTexts").text("");
        $("#selectedClauses").text("");

        if (glbPolicy.Product.IsFlatRate.toString() == 'false') {
            $('#inputRate').prop('disabled', true);
        }
        else {
            $('#inputRate').prop('disabled', false);
        }
        $("#hiddenOriginalRiskId").val(0);
        $("#hiddenOriginalPrice").val(0);
        dynamicProperties = [];
        isnCalculate = false;
        RiskVehicle.ClearGlbRisk();
        $("#inputPlateConfirmation").val("");
        $("#inputEngineConfirmation").val("");
        $("#inputChassisConfirmation").val("");
        $("#selectServiceType").UifSelect("setSelected", null);
    }

    static ClearFasecolda() {
        $("#selectMake").UifSelect("setSelected", null);
        $('#selectModel').UifSelect();
        $('#selectVersion').UifSelect();

        $("#selectYear").UifSelect();
        $("#inputPrice").val(0);
        $("#hiddenStandardVehiclePrice").val(0);
        $("#selectType").UifSelect();
    }

    static GetIndividualDetails(individualDetails) {
        if (individualDetails.length > 0) {
            $.each(individualDetails, function (id, item) {
                this.Detail = this.Address.Description;
                if (this.TradeName != null) {
                    this.Detail = '<b>' + this.TradeName + '</b>' + '<br/>' + this.Detail;
                }
                if (this.Phone != null) {
                    this.Detail += '<br/>' + this.Phone.Description;
                }
                if (this.Email != null) {
                    this.Detail += '<br/>' + this.Email.Description;
                }
                if (individualSearchType == 1) {
                    if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName == null) {
                        if (this.IsMain) {
                            $("#inputInsured").data("Object").CompanyName = this;
                        }
                    }
                    else if ($("#inputInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                        $("#inputInsured").data("Object").CompanyName = this;
                    }
                }
                else if (individualSearchType == 2) {
                    if ($("#inputBeneficiaryName").data("Object") != undefined)
                        if ($("#inputBeneficiaryName").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputBeneficiaryName").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = this;
                        }
                }
                else if (individualSearchType == 3) {
                    if ($("#inputAdditionalDataInsured").data("Object").CompanyName == null) {
                        if (this.IsMain) {
                            $("#inputAdditionalDataInsured").data("Object").CompanyName = this;
                        }
                    }
                    else if ($("#inputAdditionalDataInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                        $("#inputAdditionalDataInsured").data("Object").CompanyName = this;
                    }
                }
            });
        }

        return individualDetails;
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        var dfd = $.Deferred();
        var resultData = {};
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
                dfd.resolve(resultData);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfd.reject();
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });
        return dfd.promise();
    }

    static SetIndividualDetail() {
        var details = $('#tableIndividualDetails').UifDataTable('getSelected');

        if (details == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectAddress, 'autoclose': true });
        }
        else {
            if (individualSearchType == 1) {
                $("#inputInsured").data("Object").CompanyName = details[0];
            }
            else if (individualSearchType == 2) {
                $("#inputBeneficiaryName").data("Object").CompanyName = details[0];
            }
            else if (individualSearchType == 3) {
                $("#inputAdditionalDataInsured").data("Object").CompanyName = details[0];
            }

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        }
        else {
            var coverages = $("#listCoverages").UifListView('getData');

            $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });

            if (data.EndorsementType == EndorsementType.Modification) {
                var coverage = RiskVehicle.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description)

                $.when(coverage).done(function (coverageData) {

                    coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount, numberDecimal);
                    coverageData.Rate = FormatMoney(coverageData.Rate);
                    coverageData.LimitAmount = FormatMoney(coverageData.LimitAmount);

                    $.each(coverages, function (index, value) {
                        if (this.Id == coverageData.Id) {
                            coverages[index] = coverageData
                        }
                    });

                    $("#listCoverages").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });

                    var premium = RiskVehicle.PremiumRiskTotal();
                    var sumInsured = RiskVehicle.CalculateInsuredSum();
                    $("#inputAmountInsured").text(FormatMoney(sumInsured));
                    $("#inputPremium").text(FormatMoney(premium));
                });
            }
            else {
                $.each(coverages, function (index, value) {
                    if (this.Id != data.Id) {
                        $("#listCoverages").UifListView("addItem", this);
                    }
                });

                $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });

                var premium = RiskVehicle.PremiumRiskTotal();
                var sumInsured = RiskVehicle.CalculateInsuredSum();
                $("#inputAmountInsured").text(FormatMoney(sumInsured));
                $("#inputPremium").text(FormatMoney(premium));
            }
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = $.Deferred();
        RiskVehicleRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
            if (data.success) {
                coverage.resolve(data.result);
            }
            else {
                coverage.reject(AppResources.ErrorExcludeCoverage);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorExcludeCoverage, 'autoclose': true });
        });

        return coverage.promise();
    }

    static PremiumRiskTotal() {
        var premiumTotal = 0;
        var premiumBase = 0;
        var coveragesValues = $("#listCoverages").UifListView('getData');
        if (coveragesValues != null) {
            $.each(coveragesValues, function (key, value) {
                premiumBase = NotFormatMoney(value.PremiumAmount).replace(separatorDecimal, ".");
                premiumTotal = premiumTotal + parseFloat(premiumBase.replace(separatorDecimal, '.'));
            });
            return premiumTotal;
        }
        else {
            return 0;
        }
    }

    static CalculateInsuredSum() {
        var sumTotal = 0;
        var sumBase = 0;
        var coveragesValues = $("#listCoverages").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            sumBase = NotFormatMoney(value.LimitAmount).replace(separatorDecimal, ".");
            sumTotal = sumTotal + parseFloat(sumBase.replace(separatorDecimal, '.'));
        });
        return sumTotal;
    }

    static DisabledControlByEndorsementType(disabled) {
        switch (endorsementType) {
            case EndorsementType.Renewal: {
                $('#inputFasecoldaCode').prop('disabled', disabled);
                $('#selectMake').UifSelect('disabled', 'disabled');
                $('#selectModel').UifSelect('disabled', 'disabled');

                $('#selectType').UifSelect('disabled', 'disabled');
                $('#selectGroupCoverage').UifSelect('disabled', 'disabled');
                $('#inputInsured').prop('disabled', disabled);
                break;
            }
            case EndorsementType.Modification: {
                if (glbRisk.Status != 2) {
                    $('#inputFasecoldaCode').prop('disabled', disabled);
                    $('#selectMake').UifSelect('disabled', 'disabled');
                    $('#selectModel').UifSelect('disabled', 'disabled');

                    $('#selectType').UifSelect('disabled', 'disabled');
                    $('#selectGroupCoverage').UifSelect('disabled', 'disabled');
                    $('#inputInsured').prop('disabled', disabled);
                    //$('#inputPlate').prop('disabled', disabled);
                    //$('#inputEngine').prop('disabled', disabled);
                    //$('#inputChassis').prop('disabled', disabled);
                }
                break;
            }
            default:
                break;
        }
    }

    static ShowSaveRisk(riskId, redirec, coverageId) {
        glbRisk.redirectData = true;
        glbRisk.CoverageId = coverageId;
        var events = null;
        //lanza los eventos para la creación de el riesgo

        //if ($("#selectRisk").UifSelect("getSelected") == null || $("#selectRisk").UifSelect("getSelected") == 0) {
        //    RiskVehicle.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
        //    });

        //}
        //else {
        if (glbRisk.InfringementPolicies != null) {
            events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
        }
        if (events !== TypeAuthorizationPolicies.Restrictive) {
            RiskVehicle.RedirectAction(redirec, riskId, coverageId);
        }
        //}

    }

    static RedirectAction(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskVehicle.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.Coverage:
                RiskVehicle.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskVehicle.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.AdditionalData:
                RiskVehicle.ShowPanelsRisk(MenuType.AdditionalData);
                break;
            case MenuType.Texts:
                RiskVehicle.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskVehicle.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Script:
                RiskVehicle.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                break;
        }
    }

    static LoadSubTitles(subTitle) {
        if (subTitle == 0 || subTitle == 1) {
            if (glbRisk.Beneficiaries != null) {
                if (glbRisk.Beneficiaries.length > 0) {
                    $("#selectedBeneficiaries").text("(" + glbRisk.Beneficiaries.length + ")");
                }
                else {
                    $("#selectedBeneficiaries").text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $("#selectedBeneficiaries").text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 2) {
            if (glbRisk.Text != null) {
                if (glbRisk.Text.TextBody == null) {
                    glbRisk.Text.TextBody = "";
                }

                if (glbRisk.Text.TextBody.length > 0) {
                    $('#selectedTexts').text("(" + AppResources.LabelWithText + ")");
                }
                else {
                    $('#selectedTexts').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedTexts').text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 3) {
            if (glbRisk.Clauses != null) {
                if (glbRisk.Clauses.length > 0) {
                    $('#selectedClauses').text("(" + glbRisk.Clauses.length + ")");
                }
                else {
                    $('#selectedClauses').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedClauses').text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 4) {
            $("#selectedAdditionalData").text("(" + AppResources.LabelVarious + ")");
        }
    }

    static LoadScript() {
         
        if ($("#formVehicle").valid() && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, glbRisk.Class, dynamicProperties);
         }
    }

    static UpdateGlbRisk(data) {
        var grisk = glbRisk;
        glbRisk = data
        glbRisk.redirectController = grisk.redirectController;
        glbRisk.CoverageId = grisk.CoverageId;
        glbRisk.redirectData = grisk.redirectData;
        $.extend(glbRisk, data.Risk);
        glbRisk.Risk = null;
        glbRisk.Object = "RiskVehicle";
        glbRisk.formRisk = "#formVehicle";
        glbRisk.Class = RiskVehicle;
    }

    static ClearGlbRisk() {
        glbRisk = { Id: 0, Object: "RiskVehicle", formRisk: "#formVehicle", RecordScript: true, Class: RiskVehicle, redirectData: false };
    }

    static GetAdditionalDataModel() {
        var additionalData = $("#formAdditionalData").serializeObject();
        additionalData.NewPrice = NotFormatMoney($("#inputAdditionalDataPrice").val());
        if (additionalData.BodyType == null) {
            RiskVehicleAdditionalData.GetFuels(0);
            RiskVehicleAdditionalData.GetBodies(0);
        }
        if ($("#inputAdditionalDataInsured").data("Object") != null) {
            additionalData.InsuredId = $("#inputAdditionalDataInsured").data("Object").IndividualId;
        }

        return additionalData;
    }

    static RunRules(ruleSetId) {
        RiskVehicleRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (data.success) {
                RiskVehicle.LoadRisk(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
    }

    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbRisk.RecordScript = isModify;
        if (isModify) {
            RiskVehicle.GetPremium();
        }
    }

    static LoadViewModel(viewModel) {
        $("#inputPlate").val(viewModel.LicensePlate);
        $("#inputFasecoldaCode").val(viewModel.FasecoldaCode);
        RiskVehicle.GetMakes(viewModel.Make);
        if (viewModel.Make != "") {
            RiskVehicle.GetModelsByMakeId(viewModel.Make, viewModel.Model);
            if (viewModel.Model != "") {
                RiskVehicle.GetVersionsByMakeIdModelId(viewModel.Make, viewModel.Model, viewModel.Version);
                if (viewModel.Type != "") {
                    RiskVehicle.GetTypesByTypeId(viewModel.Type, viewModel.Type);
                }
            }
            else {
                RiskVehicle.GetVersionsByMakeIdModelId(viewModel.Make, viewModel.Model, 0);
            }
        }

        if (viewModel.Make != "" && viewModel.Model != "" && viewModel.Version != "") {
            RiskVehicle.GetYearsByMakeIdModelIdVersionId(viewModel.Make, viewModel.Model, viewModel.Version, 0);
            if (viewModel.Type != "") {
                RiskVehicle.GetTypesByTypeId(viewModel.Type, viewModel.Type);
                RiskVehicle.GetPriceByMakeIdModelIdVersionId(viewModel.Make, viewModel.Model, viewModel.Version, 0);
            }
        }


        if (viewModel.Fuel != null) {
            RiskVehicleAdditionalData.GetFuels(viewModel.Fuel);
        }

        if (viewModel.AdditionalPrice != null) {
            $("#inputAdditionalDataPrice").val(FormatMoney(viewModel.AdditionalPrice));
        }
        RiskVehicle.GetUses(viewModel.Use);
        if (viewModel.IsNew) {
            $('#chkIsNew').prop('checked', true);
        }
        else {
            $('#chkIsNew').prop('checked', false);
        }
        $("#inputEngine").val(viewModel.Engine);
        $("#inputChassis").val(viewModel.Chassis);
        RiskVehicle.GetColors(viewModel.Color);
        RiskVehicle.GetRatingZonesByPrefixId(glbPolicy.Prefix.Id, viewModel.RatingZone);
        $("#inputPrice").val(FormatMoney(viewModel.Price));
        $("#hiddenStandardVehiclePrice").val(viewModel.StandardVehiclePrice);
        $("#inputPriceAccesories").val(FormatMoney(viewModel.PriceAccesories));
        RiskVehicle.GetLimitsRcByPrefixIdProductIdPolicyTypeId(glbPolicy.Prefix.Id, glbPolicy.Product.Id, glbPolicy.PolicyType.Id, viewModel.LimitRC);
        RiskVehicle.GetGroupCoverages(glbPolicy.Product.Id, viewModel.GroupCoverage);

        $("#inputRate").val(FormatMoney(viewModel.Rate));
        $("#inputPremium").text(FormatMoney(viewModel.Premium, 2));
        if (viewModel.Coverages != "") {
            $.each(viewModel.Coverages, function (key, value) {
                this.LimitAmount = parseFloat((this.LimitAmount).replace(',', '.'));
                this.SubLimitAmount = parseFloat((this.SubLimitAmount).replace(',', '.'));
                this.PremiumAmount = parseFloat((this.PremiumAmount).replace(',', '.'));
            });
            RiskVehicle.LoadCoverages(viewModel.Coverages, viewModel.GroupCoverage);
        }
        else if (viewModel.GroupCoverage != "") {
            RiskVehicle.GetCoveragesByGroupCoverageId(viewModel.GroupCoverage);
        }
        $("#listAccesories").UifListView({ displayTemplate: "#accessoryTemplate", edit: true, delete: true, customEdit: true, customDelete: true, height: 200 });
        if (viewModel.Accesories != null) {
            $.each(viewModel.Accesories, function (index, value) {
                if (this.Amount != null) {
                    this.Amount = FormatMoney(this.Amount);
                }
                else {
                    this.Amount = 0;
                }

                if (this.Premium != null) {
                    this.Premium = FormatMoney(this.Premium);
                }
                else {
                    this.Premium = 0;
                }

                if (this.Rate != null) {
                    this.Rate = FormatMoney(this.Rate);
                }
                else {
                    this.Rate = 0;
                }

                if (this.AccumulatedPremium != null) {
                    this.AccumulatedPremium = FormatMoney(this.AccumulatedPremium);
                }
                else {
                    this.AccumulatedPremium = 0;
                }

                if (this.IsOriginal) {
                    this.Original = "Original";
                }
                else {
                    this.Original = "No Original";
                }
            })
            RiskVehicleAccessories.LoadAccessories(viewModel.Accesories);
            RiskVehicleAccessories.UpdateSummaryAccessories();
        }

        $("#hiddenOriginalPrice").val(viewModel.OriginalPrice);
        $("#hiddenOriginalRiskId").val(viewModel.OriginalRiskId);
        if (viewModel.Body != "") {
            RiskVehicleAdditionalData.GetBodies(viewModel.Body);
        }

        if (viewModel.Premium == 0 && (viewModel.Status == RiskStatus.Original || viewModel.Status == RiskStatus.Included)) {
            isnCalculate = false;
        }
        else {
            isnCalculate = true;
        }
        RiskVehicle.LoadSubTitles(0);
    }

    static UpdatePolicyComponents() {
        UnderwritingRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                RiskVehicle.ReturnUnderwriting();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {            
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
    }

    static GetMakes(selectedId) {

        RiskVehicleRequest.GetMakes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectMake").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectMake").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    if (endorsementType == EndorsementType.Renewal) {
                        $('#selectMake').prop('disabled', true);
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetUses(selectedId) {
        RiskVehicleRequest.GetUses().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectUse").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectUse").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }

    static GetColors(selectedId) {
        RiskVehicleRequest.GetColors().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectColor").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectColor").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }

    static LoadRiskCombos(riskData, glbPolicy) {
        RiskVehicleRequest.LoadRiskCombos(riskData, glbPolicy).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    //Cargar usos
                    if (riskData.Use == null || riskData.Use.Id == 0) {
                        $("#selectUse").UifSelect({ sourceData: data.result.Uses });
                    }
                    else {
                        $("#selectUse").UifSelect({ sourceData: data.result.Uses, selectedId: riskData.Use.Id });
                    }
                    //Cargar marcas
                    if (riskData.Make == null || riskData.Make.Id == 0) {
                        $("#selectMake").UifSelect({ sourceData: data.result.Makes });
                    }
                    else {
                        $("#selectMake").UifSelect({ sourceData: data.result.Makes, selectedId: riskData.Make.Id });
                    }
                    //Cargar modelos
                    if (riskData.Model == null || riskData.Model.Id == 0) {
                        $("#selectModel").UifSelect({ sourceData: data.result.Models });
                    }
                    else {
                        $("#selectModel").UifSelect({ sourceData: data.result.Models, selectedId: riskData.Model.Id });
                    }
                    //Cargar version
                    if (riskData.Version == null || riskData.Version.Id == 0) {
                        $("#selectVersion").UifSelect({ sourceData: data.result.Versions });
                    }
                    else {
                        $("#selectVersion").UifSelect({ sourceData: data.result.Versions, selectedId: riskData.Version.Id });
                    }
                    //Cargar años
                    if (riskData.Year == 0) {
                        $("#selectYear").UifSelect({ sourceData: data.result.Years });
                    }
                    else {
                        $("#selectYear").UifSelect({ sourceData: data.result.Years, selectedId: riskData.Year });
                    }
                    //Cargar tipos
                    if (riskData.Version != null && riskData.Version.Type != null) {
                        if (riskData.Version.Type.Id == 0) {
                            $("#selectType").UifSelect({ sourceData: data.result.Types });
                        }
                        else {
                            $("#selectType").UifSelect({ sourceData: data.result.Types, selectedId: riskData.Version.Type.Id });
                            $('#selectType').UifSelect('disabled', 'disabled');
                        }
                    }

                    //Cargar color
                    if (riskData.Color != null) {
                        if (riskData.Color.Id == -1) {
                            $("#selectColor").UifSelect({ sourceData: data.result.Colors });
                        }
                        else {
                            $("#selectColor").UifSelect({ sourceData: data.result.Colors, selectedId: riskData.Color.Id });
                        }
                    }

                    //Cargar rating zone
                    if (riskData.RatingZone != null) {
                        UnderwritingQuotation.DisabledButtonsQuotation();
                        if (riskData.RatingZone.Id == 0) {
                            $("#selectRatingZone").UifSelect({ sourceData: data.result.RatingZones });
                        }
                        else {
                            $("#selectRatingZone").UifSelect({ sourceData: data.result.RatingZones, selectedId: riskData.RatingZone.Id });
                        }
                    }
                    //Cargar rating LimitRcs
                    if (riskData.LimitRc != null) {
                        if (riskData.LimitRc.Id == 0) {
                            var selectedIdLimtRC = 0;
                            $.each(data.result, function (id, item) {
                                if (item.IsDefault == true) {
                                    selectedIdLimtRC = item.Id;
                                    return;
                                }
                            });
                            if (selectedIdLimtRC == 0) {
                                $("#selectLimitRC").UifSelect({ sourceData: data.result.LimitRcs });
                            }
                            else {
                                $("#selectLimitRC").UifSelect({ sourceData: data.result.LimitRcs, selectedId: selectedIdLimtRC });
                            }
                        }
                        else {
                            $("#selectLimitRC").UifSelect({ sourceData: data.result.LimitRcs, selectedId: riskData.LimitRc.Id });
                        }

                    }
                    //Cargar rating GroupCoverages
                    if (riskData.GroupCoverage != null) {
                        if (riskData.GroupCoverage.Id == 0) {
                            $("#selectGroupCoverage").UifSelect({ sourceData: data.result.GroupCoverages });
                        }
                        else {
                            $("#selectGroupCoverage").UifSelect({ sourceData: data.result.GroupCoverages, selectedId: riskData.GroupCoverage.Id });
                            if ($("#selectGroupCoverage").UifSelect("getSelected") != null) {
                                RiskVehicle.EnabledButtonAccessories(riskData.GroupCoverage.Id);
                            }
                        }
                    }

                    //Cargar tipos de servicio CompanyServiceTypes
                    if (riskData.ServiceType != null) {
                        if (riskData.ServiceType.Id == 0) {
                            $("#selectServiceType").UifSelect({ sourceData: data.result.CompanyServiceTypes });
                        }
                        else {
                            $("#selectServiceType").UifSelect({ sourceData: data.result.CompanyServiceTypes, selectedId: riskData.ServiceType.Id });
                        }
                    }

                    //Cargar tipos de combustible
                    if (riskData.Version != null && riskData.Version.Fuel != null) {
                        if (riskData.Version.Fuel.Id == 0) {
                            $("#selectFuelType").UifSelect({ sourceData: data.result.Fuels });
                        }
                        else {
                            $("#selectFuelType").UifSelect({ sourceData: data.result.Fuels, selectedId: riskData.Version.Fuel.Id });
                        }
                    }
                    if (riskData.Id != 0) {
                        RiskVehicle.DisabledControlByEndorsementType(true);
                    }
                }
            }

        });
    }
}