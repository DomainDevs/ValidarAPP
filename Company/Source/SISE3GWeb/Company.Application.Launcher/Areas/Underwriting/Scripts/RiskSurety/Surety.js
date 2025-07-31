var loadRisks = false;
var glbOpertionQuota = null;
var glbSuretyOperationQuotaEvent;
var glbSuteryConsortiumEvent;
var glbSuteryEconomicGroupEvent;
var IndividualType;
//REQ_#1010: Se crea var que permite controlar el mssg de validación para caso cotización
var controlMessageQuo = null;
var riskController = 'RiskSurety';
var policyDataGuarantee = {};
var dynamicProperties = [];
var heightListView = 430;
var individualSearchType = 1;
var modalListType;
var previousValue = 0;
var oldValueContrat = -1;
var events;
var operatingQuota = 0; //Esta variable almacena el cupo operativo
var searchHolderTo = "";
var gdlStates = [];
var gdlCity = [];
var gdlCountries = [];
var DecimalQuantity = 0;
var glbClassRiskTempPropierty = null;
var differenceContract = 0;
var PreviousContractValue = 0;

class RiskSurety extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);

        riskController = "RiskSurety";
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskSurety", formRisk: "#mainRiskSurety", RecordScript: false, Class: RiskSurety, AddressId: 0, PhoneID: 0 };
        }
        glbRisk.formRisk = "#mainRiskSurety";
        glbRisk.Class = RiskSurety;
        RiskSurety.setDefault();
        lockScreen();
        this.SetInitialLoad();
        RiskSurety.LoadTitle();
        if (glbPolicy.TemporalTypeDescription == "Temporal") {
            if (glbRisk.Id == 0) {
                RiskSurety.loadCountries();
            }

        }
        if (changeHolder) {
            (async () => {
                $('#datepicker').val(FormatDate(glbPolicy.CurrentTo));
                await RiskSurety.LoadInputHolderByIndividual(policyHolderData.HolderId, policyHolderData.HolderCustomerType, "#inputSecure");
                await RiskSurety.LoadAggregate(policyHolderData.HolderId);
                RiskSurety.SaveRiskEmission(MenuType.Risk, 0, true, true, false)
                changeHolder = false;
            })();

        }
    }
    bindEvents() {
        this.EventsRiskSurety();
        $("#SearchindividualId").on('buttonClick', UnderwritingTemporal.SearchByindividualId);
        $("#SearchCodeId").on('buttonClick', UnderwritingTemporal.SearchByindividualCode);

    }
    EventsRiskSurety() {
        $("#chkFacultativeSurety").click(function () { $("#chkRetentionSurety").prop("checked", false); });
        $("#chkRetentionSurety").click(function () { $("#chkFacultativeSurety").prop("checked", false); });

        $('#NacionalLevel').on('click', this.OnCheckedLevel);
        $('#selectSuretyCountry').on('itemSelected', this.ChangeCountries);
        $('#inputName').on('itemSelected', this.ChangeReasonSocial);
        $('#selectSuretyState').on('itemSelected', this.ChangeStates);

        //Cargar asegurado
        $("#inputInsured").on('buttonClick', this.SearchInsured);
        //Buscar Afianzado
        $("#inputSecure").on("buttonClick", this.SearchSecure);
        $('#selectRiskSuretyGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $("#btnRiskAccept").on('click', this.Accept);
        //Crear Riesgo
        $("#btnAddRiskSurety").on('click', this.AddRisk);
        //Detalle Asegurado
        $("#btnDetailInsuredRiskSurety").on('click', function () {
            RiskSurety.ShowDetailInsuredRiskSurety();
        });
        //Valor asegurado
        $('#inputContractValue').focusout(this.CreateContractValue);
        //Riesgos
        $('#selectRiskSurety').on('itemSelected', RiskSurety.ChangeRisk);
        //Detalle Afianzado
        $("#btnDetailSecureRiskSecure").on('click', function () {
            RiskSurety.ShowSecureDetails();
        });

        $("#btnIndividualDetailAccept").on('click', function () {
            RiskSurety.SetIndividualDetail();
        });

        $("#btnSecureDetailAccept").on('click', function () {
            RiskSurety.SetIndividualDetail();
        });

        $("#btnDeleteRisk").on('click', function () {
            RiskSurety.DeleteRisk();
        });

        $("#btnClose").on('click', async () => {
              if ((glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) || glbRisk.AmountInsured <= parseFloat(NotFormatMoney($("#inputAvailable").val())) || glbRisk.AmountInsured == undefined || glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && differenceContract <= parseFloat(NotFormatMoney($("#inputAvailable").val()))) {
                if (glbRisk != null && glbRisk.Id != 0) {
                    lockScreen();

                    var data = await RiskSurety.GetEnableCrossGuarantee();
                    if (data) {
                        if (glbRisk.Guarantees != null && glbRisk.Guarantees.length > 0) {
                            RiskSurety.SaveRisk(MenuType.Risk, 0, true, true, false);
                            RiskSurety.UpdatePolicyComponents(true);
                            glbRisk = null;
                            glbCoverage = null;
                            ScrollTop();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NoDataGuarantee, 'autoclose': true });
                        }
                    }
                    else {
                        RiskSurety.SaveRisk(MenuType.Risk, 0, true, true, false);
                        RiskSurety.UpdatePolicyComponents(true);
                        glbRisk = null;
                        glbCoverage = null;
                        ScrollTop();
                    }

                    unlockScreen();
                }
                else {
                    glbRisk = { Id: 0, Object: "RiskSurety", formRisk: "#mainRiskSurety", RecordScript: false, Class: RiskSurety };
                    if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
                        router.run("prtQuotation");
                    } else {
                        router.run("prtTemporal");
                    }
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
            }
        });

        $("#inputContractValue").focusin(function () {
            if ($.trim($(this).val()) != "") {
                $(this).val(NotFormatMoney($(this).val()));
            }
        });

        //Coberturas
        $('#listCoverages').on('rowAdd', async (event) => {
            var dataGuarantee = RiskSurety.GetEnableCrossGuarantee();
            if (dataGuarantee) {
                if (glbRisk.Guarantees != null && glbRisk.Guarantees.length > 0) {
                    RiskSurety.SaveRisk(MenuType.Coverage, 0, true, false, false, true);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NoDataGuarantee, 'autoclose': true });
                }
            } else {
                RiskSurety.SaveRisk(MenuType.Coverage, 0, true, false, false, true);
            }

        });

        $('#listCoverages').on('rowEdit', async (event, data, index) => {
            var dataGuarantee = await RiskSurety.GetEnableCrossGuarantee();
            if (dataGuarantee) {
                if (glbRisk.Guarantees != null && glbRisk.Guarantees.length > 0) {

                    if (data.IsVisible == false) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
                    }
                    else {
                        RiskSurety.SaveRisk(MenuType.Coverage, data.Id, true, true, false, true);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NoDataGuarantee, 'autoclose': true });
                }
            } else {
                if (data.IsVisible == false) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
                }
                else {
                    RiskSurety.SaveRisk(MenuType.Coverage, data.Id, true, true, false, true);
                }
            }
        });

        $('#listCoverages').on('rowDelete', async (event, data) =>{
            var dataGuarantee = await RiskSurety.GetEnableCrossGuarantee();
            if (dataGuarantee) {
                if (glbRisk.Guarantees != null && glbRisk.Guarantees.length > 0) {
                    RiskSurety.DeleteCoverage(data);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NoDataGuarantee, 'autoclose': true });
                }
            } else {
                RiskSurety.DeleteCoverage(data);
            }
        });

        //Seleccionar un elemento de la modal de busqueda
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividual);

        $("#btnAcceptNewPersonOnline").click(function () {
            glbPersonOnline = {
                Rol: 2,
                ViewModel: RiskSurety.GetRiskDataModel()
            };
            UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsured").val().trim());
        });

        $('#btnConvertProspect').click(function () {
            if ($("#inputInsured").data("Object") != null) {
                glbPersonOnline = {
                    Rol: 2,
                    ViewModel: RiskSurety.GetRiskDataModel()
                };

                UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsured").data("Object").IndividualId, $("#inputInsured").data("Object").IndividualType, $("#inputInsured").data("Object").CustomerType, 2);
            }
        });

        $('#btnSecureConvertProspect').click(function () {
            if ($("#inputSecure").data("Object") != null) {
                glbPersonOnline = {
                    Rol: 2,
                    ViewModel: RiskSurety.GetRiskDataModel()
                };

                UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputSecure").data("Object").IndividualId, $("#inputSecure").data("Object").IndividualType, $("#inputSecure").data("Object").CustomerType, 3);
            }
        });

        $("#btnScript").on('click', function () {
            RiskSurety.LoadScript();
        });

        $('#chkTerminalUnitContract').attr('checked', true);

        $('#NacionalLevel').on('change', function () {
            if ($('#NacionalLevel').is(":checked") == true) {
                $('#selectSuretyState').prop('disabled', true);
                $('#selectSuretyCity').prop('disabled', true);
            } else {
                var selectedItem = $("#selectSuretyCountry").UifSelect('getSelected');
                if (selectedItem.Id > 0) {
                    RiskSuretyRequest.GetStates(selectedItem.Id, 0).done(function (data) {
                        if (data.success) {
                            gdlStates = data.result;
                            $("#selectSuretyState").UifSelect({ sourceData: data.result });
                            $('#selectSuretyState').prop('disabled', false);
                            $('#selectSuretyCity').prop('disabled', false);
                        }
                    });
                }
                else {
                    $("#selectSuretyState").UifSelect();
                    $('#selectSuretyState').prop('disabled', false);
                    $('#selectSuretyCity').prop('disabled', false);
                }
            }
        });

    }

    SearchInsured() {
        searchHolderTo = "inputInsured";
        RiskSurety.LoadInputHolderByDescription();
    }

    SearchSecure() {
        searchHolderTo = "inputSecure";
        RiskSurety.LoadInputHolderByDescription();
    }

    async Accept() {
        ValidityParticipant1 = null;
        if (glbPolicy.TemporalType != 4) {
            if (await RiskSurety.LoadAggregate($('#inputSecure').data("Object").IndividualId) == false) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
            }

            if (ValidityParticipant1 != undefined && ValidityParticipant1 != null) {
                if (ValidityParticipant1.some(x => x.consortiumEventDTO.IsConsortium == true) || ValidityParticipant1.some(x => x.IndividualOperatingQuota.IndividualID == 0)) {
                    ValidityParticipant1.forEach((x) => {
                        if (x.consortiumEventDTO.IsConsortium == true) {
                            $.UifNotify('show', { 'type': 'danger', 'message': `El integrante ${x.consortiumEventDTO.ConsortiumpartnersDTO.PartnerName} no tiene cupo disponible`, 'autoclose': true });
                        }
                    });

                    ValidityParticipant1 = null;

                    return;
                }
                ValidityParticipant1 = null;
            }

            lockScreen();
            var data = await RiskSurety.GetEnableCrossGuarantee();
            if (data) {
                if (glbRisk.Guarantees != null && glbRisk.Guarantees.length > 0) {
                    $("#btnClose").prop("disabled", false);
                    RiskSurety.SaveRisk(MenuType.Risk, 0, true, true, false);
                    RiskSurety.UpdatePolicyComponents(false);
                    ScrollTop();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NoDataGuarantee, 'autoclose': true });
                    $("#btnClose").prop("disabled", true);
                    return false;
                }
            }
            else {
                RiskSurety.SaveRisk(MenuType.Risk, 0, true, true, false);
                RiskSurety.UpdatePolicyComponents(false);
                ScrollTop();
            }

            unlockScreen();
        }
        else {
            RiskSurety.SaveRisk(MenuType.Risk, 0, true, true, false);
            RiskSurety.UpdatePolicyComponents(false);
            ScrollTop();
        }
    }

    OnCheckedLevel() {
        var enabled = $('#NacionalLevel').is(':checked', true);
        $("#selectSuretyState").UifSelect("disabled", enabled);
        if (enabled) {
            $("#selectSuretyState").UifSelect("setSelected", null);
            $("#selectSuretyCity").UifSelect("setSelected", null);
        }
        else {
            var selectedItem = $("#selectSuretyCountry").UifSelect("getSelected");
            RiskSuretyRequest.GetStates(selectedItem, 0).done(function (data) {
                if (data.success) {
                    gdlStates = data.result;
                    $("#selectSuretyState").UifSelect({ sourceData: data.result });
                    $("#selectSuretyCity").UifSelect("disabled", true);
                }
            });
        }
    }


    ChangeGroupCoverage(event, selectedItem) {
        $('#inputPremiumRisk').val(0);
        $('#inputTotalSumInsuredRisk').text(0);
        if (selectedItem.Id > 0) {
            RiskSurety.GetRiskSuretyCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id);
        }
        else {
            $("#selectRiskSuretyGroupCoverage").UifListView("refresh");
        }
    }

    static loadCountries() {
        RiskSuretyRequest.GetCountries().done(function (data) {
            if (data.success) {
                $("#selectSuretyCountry").UifSelect({ sourceData: data.result });
            }
            RiskSuretyRequest.GetDefaultCountry().done(function (data) {
                if (data.success && data.result.length > 0) {
                    $("#selectSuretyCountry").UifSelect('setSelected', data.result[0].Value);
                    RiskSuretyRequest.GetStates(data.result[0].Value, 0).done(function (data) {
                        if (data.success) {
                            gdlStates = data.result;
                            $("#selectSuretyState").UifSelect({ sourceData: data.result });
                        }
                    });
                } else {
                    $("#selectSuretyCountry").UifSelect('setSelected', null);
                }
            });
        });
    }

    ChangeCountries(event, selectedItem) {
        if ($('#NacionalLevel').is(":checked") == true) {
            $('#selectSuretyState').prop('disabled', true);
        } else {
            if (selectedItem.Id > 0) {
                RiskSuretyRequest.GetStates(selectedItem.Id, 0).done(function (data) {
                    if (data.success) {
                        gdlStates = data.result;
                        $("#selectSuretyState").UifSelect({ sourceData: data.result });
                        $('#selectSuretyState').prop('disabled', false);
                    }
                });
            }
            else {
                $("#selectSuretyState").UifSelect();
                $('#selectSuretyState').prop('disabled', false);
            }
        }
    }

    ChangeStates(event, selectedItem) {
        if ($('#NacionalLevel').is(":checked") == true) {
            $('#selectSuretyState').prop('disabled', true);
            $('#selectSuretyCity').prop('disabled', true);
        } else {
            var countryId = $('#selectSuretyCountry').UifSelect("getSelected");
            if (selectedItem.Id > 0) {
                RiskSuretyRequest.GetCities(countryId, selectedItem.Id).done(function (data) {
                    if (data.success) {
                        gdlCity = data.result;
                        $("#selectSuretyCity").UifSelect({ sourceData: data.result });
                    }
                });
            }
            else {
                $("#selectSuretyCity").UifSelect();
            }
        }

    }

    AddRisk() {
        RiskSurety.ClearControlsRiskSurety();
        RiskSurety.DisableControls(false);
        if (glbPolicy != null && glbPolicy.Product != null && glbPolicy.Product.CoveredRisk != null && glbPolicy.Product.CoveredRisk.PreRuleSetId > 0) {
            RiskSurety.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
        }
    }

    CreateContractValue() {
        if ($.trim($(this).val()) != "" && $.trim($(this).val()) != "0") {
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
                if (oldValueContrat != 0 && $(this).val() != oldValueContrat) {
                    oldValueContrat = parseFloat($(this).val());
                    var coverages = $("#listCoverages").UifListView('getData');
                    if (coverages != null && coverages.length > 0) {
                        coverages = RiskSurety.UnformatCoverage(coverages);
                        var ContractValue = NotFormatMoney($("#inputContractValue").val());
                        RiskSurety.QuotationCoverages(coverages, ContractValue);
                    }
                    var formatMoney = FormatMoney($(this).val());
                    $("#inputContractValue").val(formatMoney);
                }
                else {
                    oldValueContrat = parseFloat($(this).val());
                }
            }
        }
    }
    SetInitialLoad() {
        if ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) && gblCurrentRiskTemporalNumberOld == null) {
            RiskSuretyRequest.GetCiaRiskByTemporalId(gblCurrentRiskTemporalNumber).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        RiskSurety.GetRisksSuretyByTemporalId(gblCurrentRiskTemporalNumber, glbRisk.Id);
                    }
                    else {
                        RiskSurety.LoadInitialize();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.TemporaryNotExist, 'autoclose': true });
                }
            });
        }
        else {
            RiskSuretyRequest.GetCiaRiskByTemporalId(glbPolicy.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        RiskSurety.GetRisksSuretyByTemporalId(glbPolicy.Id, glbRisk.Id);
                    }
                    else {
                        RiskSurety.LoadInitialize();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.TemporaryNotExist, 'autoclose': true });
                }
            });
        }

    }

    SelectIndividual(e) {
        var individualId = $(this).children()[0].innerHTML;
        var customerType = $(this).children()[1].innerHTML;
        RiskSurety.LoadInputHolderByIndividual(individualId, customerType, "#" + searchHolderTo);
        if (searchHolderTo == "inputSecure") {
            RiskSurety.LoadAggregate(individualId);
        }


        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    static async LoadInputHolderByIndividual(individual, customerType, input) {
        $(input).val('');
        individual = individual.toString()
        try {
            var data = await HoldersRequest.GetHoldersByIndividualId(individual, customerType);
            if (data.success) {
                RiskSurety.SetInputFromHoldersResult(input, data.result);
            } else {
                showErrorToast(data.result);
            }
        } catch (e) {
            showErrorToast(AppResources.ErrorConsultingInsured);
        }
    }

    static LoadInputHolderByDescription() {
        let selector = "#" + searchHolderTo;
        let description = $(selector).val();
        var descrip = parseInt(description) || description.trim();
        if (typeof (descrip) !== "number" && descrip.length < 3) {

            showInfoToast(AppResources.HolderSearchMinLength);
            return;
        }

        var customerType = 1;
        if (searchHolderTo == "inputInsured" || searchHolderTo == "inputSecure") {
            customerType = 1
        } else {
            customerType = glbPolicy.Holder.CustomerType;
        }

        HoldersRequest.GetHoldersByDescription(description, customerType)
            .done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.length == 0) {
                            RiskSurety.CleanInputFromHolderResult(selector);
                            showInfoToast(AppResources.MessageSearchInsureds);
                            if (selector === "inputInsured") {
                                RiskSurety.getReasonSocialIndividualId(-1, 1);
                            }
                        }
                        else if (data.result.length == 1) {
                            RiskSurety.SetInputFromHoldersResult(selector, { holder: data.result[0] });
                        }
                        else {
                            modalListType = 1;
                            RiskSurety.ShowModalListRiskSurety(data.result);
                        }
                    }
                } else {
                    RiskSurety.CleanInputFromHolderResult(selector);
                    showInfoToast(data.result);
                    if (selector === "#inputInsured") {
                        RiskSurety.getReasonSocialIndividualId(-1, 1);
                    }
                }
            }).fail(function (args) {
                showErrorToast(AppResources.ErrorConsultingInsured);
                RiskSurety.CleanInputFromHolderResult(selector);
                if (selector === "#inputInsured") {
                    RiskSurety.getReasonSocialIndividualId(-1, 1);
                }
            });
    }

    static SetInputFromHoldersResult(selector, result) {
        let contractor = result.holder;
        IndividualType = contractor.IndividualType;
        contractor.details = result.details || [contractor.CompanyName];
        $(selector).data("Object", contractor);
        $(selector).val(contractor.Name + ' (' + contractor.IdentificationDocument.Number + ')');
        if (selector === "#inputSecure") {

            RiskSurety.LoadAggregate(contractor.IndividualId)
        }
        if (selector === "#inputInsured") {
            if (contractor.InsuredId == 0 && contractor.CustomerType != CustomerType.Prospect) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInsuredNoInsuredRol, 'autoclose': true });
                RiskSurety.CleanInputFromHolderResult(selector);
                return false;
            }
            else {
                RiskSurety.getReasonSocialIndividualId(contractor.IndividualId, 1);
            }
        }

    }

    static CleanInputFromHolderResult(selector) {
        $(selector).data("Object", null);
        $(selector).val('');
    }

    static ChangeRisk(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskSurety.GetRiskSuretyById(glbPolicy.Id, selectedItem.Id);
        }
        else {
            RiskSurety.ClearControlsRiskSurety();
        }
    }


    static setDefault() {
        $("#btnConvertProspect").hide();
        $("#btnSecureConvertProspect").hide();
        $('#inputSecure').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputTextObservations").TextTransform(ValidatorType.UpperCase);
        $('#inputContractValue').text = "";
        $('#inputContractValue').val("");
        //$('#inputContractValue').OnlyDecimals(DecimalQuantity);
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
        RiskSurety.getReasonSocialIndividualId(-1, 1);
        if (glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
            $('#datepicker').val(FormatDate(glbPolicy.CurrentTo));
        }

    }
    static LoadInitialize() {

        RiskSurety.LoadPerson();
        if (glbPolicy != null && glbRisk != null && glbRisk.Id > 0) {
            RiskSurety.GetRisksSuretyByTemporalId(glbPolicy.Id, glbRisk.Id);
            RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
        }
        else {
            RiskSurety.GetListSuretyContractTypes(0);
            //RiskSurety.loadCountries();
            RiskSurety.GetListSuretyContractCategories(0);
            if (glbPolicy.Product != null && glbPolicy.Product.CoveredRisk != null && glbPolicy.Product.CoveredRisk.PreRuleSetId > 0) {
                var runRules = RiskSurety.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
                runRules.done(function (riskSurety) {
                    RiskSurety.setInitializeSurety();
                    RiskSurety.LoadHolderInsured();
                });
            }
            else {
                RiskSurety.setInitializeSurety();
                RiskSurety.LoadHolderInsured();
            }

        }
        if (glbPolicy.IssueDate.indexOf("Date") > -1) {
            glbPolicy.IssueDate = FormatDate(glbPolicy.IssueDate);
        }
        RiskSurety.SetModificaction();
        //RiskSurety.LoadTitle();
    }
    static SetModificaction() {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            var riskId = $("#selectRiskSurety option:eq(1)").val();
            if (riskId != null && riskId != "") {
                $("#selectRiskSurety").val(riskId);
                RiskSurety.GetRiskSuretyById(glbPolicy.Id, riskId);
                $("#selectRiskSurety").attr("disabled", true);
                $("#btnDeleteRisk").attr("disabled", true);
                $("#btnAddRiskSurety").attr("disabled", true);
            }
        }
    }
    static LoadHolderInsured() {
        if (glbPolicy != null && glbPolicy.Holder.IndividualId != "0" && glbRisk.Contractor == undefined && glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
            individualSearchType = 3;
            RiskSurety.LoadInputHolderByIndividual(glbPolicy.Holder.IndividualId, glbPolicy.Holder.CustomerType, '#inputSecure');
        }
    }
    static setInitializeSurety() {
        RiskSurety.GetRiskGroupCoverages(glbPolicy.Product.Id, 0);
    }
    static LoadPerson() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    if (glbPersonOnline.IndividualSearchType != null) {
                        individualSearchType = glbPersonOnline.IndividualSearchType;
                    }
                    RiskSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
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
                    RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
                }
                else {
                    RiskSurety.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }


    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }


    static SaveRiskSurety(event, ControlsRisk) {
        var indexRisk = glbRisk.Id;
        var redirect = 0;
        if (ControlsRisk != null) {
            redirect = ControlsRisk.redirect;
            indexRisk = ControlsRisk.indexRisk;
            if (ControlsRisk.validPremium) {
                if ($('#inputPremiumRisk').text() == "0" || $.trim($('#inputPremiumRisk').text()) == "") {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageCalculatePremium, 'autoclose': true });
                    return false;
                }
            }
        }
        else {
            if (RiskSurety.PremiumRiskTotal() != 0) {
                if ($('#inputPremiumRisk').text() == "0" || $.trim($('#inputPremiumRisk').text()) == "") {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageCalculatePremium, 'autoclose': true });
                    return false;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidatePremium, 'autoclose': true });
                return false;
            }
        }
        RiskSurety.SaveRisk(redirect, indexRisk, true);
        ScrollTop();
    }
    static ValidateRiskSurety() {

        $("#mainRiskSurety").validate();
        if ($("#mainRiskSurety").valid()) {
            return true;
        }
        else {
            return false;
        }
    }

    static ValidateLimiteQuotaOperative() {
        if (parseFloat(NotFormatMoney($('#inputAccumulation').val())) >= MaxDecimalQuota) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValueExceedsQuotaOperative, 'autoclose': true });
            return false;
        }
        else if (parseFloat(NotFormatMoney($('#inputOperationalCapacity').val())) >= MaxDecimalQuota) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValueExceedsQuota, 'autoclose': true });
            return false;
        }
        else if (parseFloat(NotFormatMoney($('#inputAvailable').val())) >= MaxDecimalQuota) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValueExceedsQuotaAvailable, 'autoclose': true });
            return false;
        }
        else
            return true;

    }


    static LoadInsured(insured) {
        if (individualSearchType == 1) {
            $("#inputInsured").data("Object", insured);
            $("#inputInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                $("#inputInsured").data("Detail", RiskSurety.GetIndividualDetails(insured.CompanyName));
                $("#btnConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        else if (individualSearchType == 2) {
            $("#inputBeneficiaryName").data("Object", insured);
            $("#inputBeneficiaryName").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                $("#inputBeneficiaryName").data("Detail", RiskSurety.GetIndividualDetails(insured.CompanyName));
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

    static LoadSecure(secure) {
        $("#inputSecure").data("Object", secure);
        $("#inputSecure").val(secure.Name + ' (' + secure.IdentificationDocument.Number + ')');
        if (secure.CustomerType == CustomerType.Individual) {

            $("#btnSecureConvertProspect").hide();
        } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
            $("#btnSecureConvertProspect").show();
        }
        RiskSurety.LoadAggregate(secure.IndividualId);
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {


        var descrip = parseInt(description) || description;
        if (typeof (descrip) !== "number" && descrip.length < 3) {

            showInfoToast(AppResources.HolderSearchMinLength);
            return;
        }
        if (customerType == null && glbPolicy.TemporalType == TemporalType.Policy) {
            customerType = CustomerType.Individual;
        }
        HoldersRequest.GetHoldersByDescription(descrip, glbPolicy.Holder.CustomerType)
            .done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (individualSearchType == 1 || individualSearchType == 2) {
                            if (data.result.length == 0 && individualSearchType != 2) {
                                showErrorToast(AppResources.MessagePerson);
                            }
                            else if (data.result.length == 0) {
                                showInfoToast(AppResources.MessageSearchInsureds);
                            }
                            else if (data.result.length == 1) {
                                let defaultInsured = data.result[0];
                                IndividualType = defaultInsured.IndividualType;
                                defaultInsured.details = [data.result[0].CompanyName];
                                RiskSurety.LoadInsured(defaultInsured);

                            }
                            else if (data.result.length > 1) {
                                modalListType = 1;
                                RiskSurety.ShowModalListRiskSurety(data.result);
                            }
                        }
                        else if (individualSearchType == 3) {
                            controlMessageQuo = 0;
                            if (data.result.length == 0) {
                                showErrorToast(AppResources.MessagePerson);
                            }
                            else if (data.result.length == 1) {
                                let defaultSecure = data.result[0];
                                IndividualType = defaultSecure.IndividualType;
                                defaultSecure.details = [defaultSecure.CompanyName];
                                RiskSurety.LoadSecure(defaultSecure);
                            }
                            else if (data.result.length > 1) {
                                modalListType = 1;
                                RiskSurety.ShowModalListRiskSurety(data.result);
                            }
                        }

                        if (glbPolicy.TemporalType != TemporalType.Quotation) {
                            RiskSurety.SaveHolderGuarantee();
                        }
                    }
                } else {
                    if (individualSearchType == 1) {
                        $('#inputInsured').data("Object", null);
                        $('#inputInsured').data("Detail", null);
                        $('#inputInsured').val('');
                    }
                    else if (individualSearchType == 2) {
                        $('#inputBeneficiaryName').data("Object", null);
                        $('#inputBeneficiaryName').data("Detail", null);
                        $('#inputBeneficiaryName').val('');
                    }
                    else if (individualSearchType == 3) {
                        $('#inputSecure').data("Object", null);
                        $('#inputSecure').data("Detail", null);
                        $('#inputSecure').val('');
                    }
                    showInfoToast(data.result);
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                showErrorToast(AppResources.ErrorConsultingInsured);
            });
    }

    static ShowModalListRiskSurety(dataTable) {
        var datalist = [];
        dataTable.forEach(function (item) {
            datalist.push({
                Id: item.IndividualId,
                CustomerType: item.CustomerType,
                Code: item.InsuredId,
                DocumentNum: item.IdentificationDocument.Number,
                Description: item.Name,
                CustomerTypeDescription: item.CustomerTypeDescription,
                DocumentType: item.IdentificationDocument.DocumentType.Description
            });
        });
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', datalist);
        $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
    }

    static ShowPanelsRisk(Menu) {
        if (RiskSurety.ValidateLimiteQuotaOperative()) {
            switch (Menu) {
                case MenuType.Risk:
                    break;
                case MenuType.Texts:
                    $("#buttonsContractObjects").hide();
                    $("#buttonsTexts").show();
                    $("#modalTexts").UifModal('showLocal', AppResources.LabelDataTexts);
                    break;
                case MenuType.Clauses:
                    $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                    break;
                case MenuType.Beneficiaries:
                    $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                    break;
                case MenuType.CrossGuarantees:
                    $("#modalCrossGuarantees").UifModal('showLocal', AppResources.LabelGuarantees);
                    break;
                case MenuType.ContractObject:
                    $("#buttonsContractObjects").show();
                    $("#buttonsTexts").hide();
                    $("#modalTexts").UifModal('showLocal', AppResources.LabelContractObject);
                    break;
                default:
                case MenuType.Script:
                    RiskSurety.LoadScript();
                    break;
            }
        }
    }

    static HidePanelsRisk(Menu) {

        switch (Menu) {
            case MenuType.Risk:
                break;
            case MenuType.ContractObject:
            case MenuType.Texts:
                $("#modalTexts").UifModal('hide');
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('hide');
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").UifModal('hide');
                break;
            case MenuType.CrossGuarantees:
                $("#modalCrossGuarantees").UifModal('hide');
                break;
            default:
                break;
        }
    }

    static LoadAggregate(individualId) {
        return OperationQuotaCumulus.GetInformationAfianzado(individualId).then(function (data) {
            return OperationQuotaCumulus.SetInformationAfianzado() && RiskSurety.validateOperatingQuota();
        });
    }

    static GetAvaliableOperationQuota(InsuredId, prefixCd, issueDate) {
        var dfd = $.Deferred();
        RiskSuretyRequest.IsConsortiumindividualId(InsuredId).done(function (data) {
            if (data.success) {

                if (data.result == false) {
                    glbRisk.IsConsortium = true;

                    $("#inputAvaliableOperationQuota").hide();
                    $("#LabelAvaliableOperationQuota").hide();
                }
                else {
                    $("#inputAvaliableOperationQuota").show();
                    $("#LabelAvaliableOperationQuota").show();

                    RiskSuretyRequest.GetAvailableCumulus(InsuredId, prefixCd, issueDate).done(function (data) {
                        if (data.success) {
                            $("#inputAvaliableOperationQuota").val(FormatMoney(data.result));
                        }
                        else
                            $("#inputAvaliableOperationQuota").val(0);
                    });
                }
                dfd.resolve(data.result);
            }
            else {
                dfd.reject();
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        });
        return dfd.promise();
    }

    static GetRiskGroupCoverages(productId, selectedId) {
        var dfd = $.Deferred();
        RiskSuretyRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == null || selectedId == 0) {
                        $("#selectRiskSuretyGroupCoverage").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectRiskSuretyGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }

                UnderwritingQuotation.DisabledButtonsQuotation();
                dfd.resolve(data.result);
            }
            else {
                dfd.reject();
            }
        });
        return dfd.promise();
    }

    static GetRiskSuretyById(temporalId, id) {
        var dfd = $.Deferred();
        if (id != null && id != "" && glbPolicy.Endorsement != null && glbPolicy != null) {
            RiskSuretyRequest.GetRiskSuretyById(temporalId, id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        //(gblCurrentRiskTemporalNumber != 0 && gblCurrentRiskTemporalNumber != null) ? RiskSurety.LoadRiskTemporalSearch(data.result) : RiskSurety.LoadRiskSurety(data.result);
                        if (glbPolicy.Endorsement.EndorsementType == 2) {
                            if (glbRisk.Coverages != undefined && glbRisk.Coverages.length > 0) {
                                data.result.Risk.Coverages = glbRisk.Coverages;
                                data.result.Risk.Premium = 0;
                                data.result.Risk.Coverages.forEach(function (item) {
                                    data.result.Risk.Premium = data.result.Risk.Premium + parseFloat(item.PremiumAmount.toString().replace(",", "."));
                                });
                            }
                        }
                        RiskSurety.LoadRiskSurety(data.result);
                        dfd.resolve();
                    }
                    else {
                        dfd.reject(AppResources.ErrorSearchRisk);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
                    }

                }
                else {
                    dfd.reject(data.result);
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                dfd.reject(AppResources.ErrorSearchRisk);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });

            });
        }
        return dfd.promise();
    }

    static getReasonSocialIndividualId(individualId, Namenum) {

        if ($('#inputInsured').data("Object") != undefined && individualId != null && individualId > 0) {
            if ($('#inputInsured').data("Object").IndividualType == IndividualTypePerson.Natural) {
                var insuredToPrint = $('#inputInsured').data("Object");
                if (insuredToPrint.CompanyName != null)
                {
                    Namenum = insuredToPrint.CompanyName.NameNum;
                }
                var insured = [];
                insuredToPrint.TradeName = insuredToPrint.Name;
                if (insuredToPrint.CompanyName != null)
                {
                insuredToPrint.NameNum = insuredToPrint.CompanyName.NameNum;
                }
                insured[0] = insuredToPrint;
                $('#reasonSocialWrapper').css("display", "block");
                $("#inputName").val(insuredToPrint.Name);
                $("#inputAdress").val(insuredToPrint.CompanyName.Address.Description);
                $("#inputPhone").val(insuredToPrint.CompanyName.Phone.Description);
                $("#inputName").UifSelect({ sourceData: insured, selectedId: Namenum });
            }
            else {
                $("#inputName").val("");
                $("#inputAdress").val("");
                $("#inputPhone").val("");
                var arrayPromise = new Array();
                arrayPromise.push(
                    new Promise((resolve, reject) => {
                        UnderwritingRequest.GetCompanyBusinessByIndividualId(individualId).done(function (data) {
                            if (data.success) {
                                if (data.result.length > 0) {
                                    $('#reasonSocialWrapper').css("display", "block");
                                    var companyReasonSocial = data.result.filter(function (item) {
                                        return item.NameNum == Namenum
                                    });
                                    if (companyReasonSocial.length == 0) {
                                        Namenum = data.result[0].NameNum;
                                        companyReasonSocial = data.result.filter(function (item) {
                                            return item.NameNum == Namenum
                                        });
                                    }
                                    var TradeName = companyReasonSocial[0].TradeName;
                                    var Direccion = companyReasonSocial[0].AddressID;
                                    var Telefono = companyReasonSocial[0].PhoneID;
                                    glbRisk.PhoneID = Telefono;
                                    glbRisk.AddressId = Direccion;
                                    $("#inputName").val(TradeName);
                                    $("#inputName").UifSelect({ sourceData: data.result, selectedId: Namenum });
                                    $("#inputAdress").val(Direccion);
                                    $("#inputPhone").val(Telefono);
                                    if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName != null) {
                                        $("#inputInsured").data("Object").CompanyName.NameNum = Namenum;
                                    }

                                    RiskSuretyRequest.GetIndividualDetailsByIndividualId(individualId, 1).done(function (elemento) {
                                        if (elemento.success) {
                                            if (elemento.result.length > 0) {
                                                companyReasonSocial = elemento.result.filter(function (item) {
                                                    return item.NameNum == Namenum
                                                });

                                                if (companyReasonSocial[0].Address != null && companyReasonSocial[0].Address.Description != null) {
                                                    $("#inputAdress").val(companyReasonSocial[0].Address.Description);
                                                    if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName != null) {
                                                        $("#inputInsured").data("Object").CompanyName.Address.Id = companyReasonSocial[0].Address.Id;
                                                        $("#inputInsured").data("Object").CompanyName.Address.Description = companyReasonSocial[0].Address.Description;
                                                    }
                                                }
                                                if (companyReasonSocial[0].Phone != null && companyReasonSocial[0].Phone.Description != null) {
                                                    $("#inputPhone").val(companyReasonSocial[0].Phone.Description);
                                                    if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName != null) {
                                                        $("#inputInsured").data("Object").CompanyName.Phone.Id = companyReasonSocial[0].Phone.Id;
                                                        $("#inputInsured").data("Object").CompanyName.Phone.Description = companyReasonSocial[0].Phone.Description;
                                                    }
                                                }
                                            }
                                            resolve(elemento);
                                        }
                                    }).fail(xhr => {
                                        reject(xhr);
                                    });

                                }
                                else
                                    $('#reasonSocialWrapper').css("display", "none");
                            }
                            else
                                $('#reasonSocialWrapper').css("display", "none");
                        }).fail(xhr => {
                            reject(xhr);
                        })
                    }));
                return Promise.all(arrayPromise);
            }

        } else {
            $('#reasonSocialWrapper').css("display", "none");
        }
    }

    static LoadRiskSurety(riskData) {
        if (riskData != null && riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }

        }
        if (riskData != null && riskData.Risk.MainInsured != null && riskData.Risk.MainInsured.CompanyName != null) {
            $("#inputSecure").data("Object", riskData.Contractor)
            $("#inputInsured").data("Object", riskData.Risk.MainInsured);
            $("#inputInsured").data("Detail", RiskSurety.GetIndividualDetails(RiskSurety.GetIndividualDetailsByIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CustomerType)));
            $("#inputInsured").val(riskData.Risk.MainInsured.Name + ' (' + riskData.Risk.MainInsured.IdentificationDocument.Number + ')');

            if ($("#inputInsured").data("Object") != undefined) {


                if (riskData.Risk.MainInsured.CompanyName.Phone.Description == null || riskData.Risk.MainInsured.CompanyName.Address.Description == null) {
                    HoldersRequest.GetHoldersByDescription(riskData.Risk.MainInsured.IdentificationDocument.Number, riskData.Risk.MainInsured.CustomerType)
                        .done(function (data) {
                            if (data.success) {
                                if (data.result.length = 1) {
                                    $("#inputInsured").data("Object", data.result[0]);
                                    if (data.result[0] != null && data.result[0].CompanyName != null && data.result[0].CompanyName.Phone != null && data.result[0].CompanyName.Phone.Description != null) {
                                        $("#inputInsured").data("Object").CompanyName.Phone.Description = data.result[0].CompanyName.Phone.Description;
                                        $("#inputPhone").val(data.result[0].CompanyName.Phone.Description);
                                    }
                                    if (data.result[0] != null && data.result[0].CompanyName != null && data.result[0].CompanyName.Address != null && data.result[0].CompanyName.Address.Description != null) {
                                        $("#inputInsured").data("Object").CompanyName.Address.Description = data.result[0].CompanyName.Address.Description;

                                        $("#inputAdress").val(data.result[0].CompanyName.Address.Description);
                                    }

                                    RiskSurety.getReasonSocialIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CompanyName.NameNum);
                                }
                            }

                        });
                }
                else {
                    $("#inputInsured").data("Object").CompanyName.Phone.Description = riskData.Risk.MainInsured.CompanyName.Phone.Description;
                    $("#inputInsured").data("Object").CompanyName.Address.Description = riskData.Risk.MainInsured.CompanyName.Address.Description;
                    $("#inputPhone").val(riskData.Risk.MainInsured.CompanyName.Phone.Description);
                    $("#inputAdress").val(riskData.Risk.MainInsured.CompanyName.Address.Description);
                    RiskSurety.getReasonSocialIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CompanyName.NameNum);
                }
            }
            else {
                RiskSurety.getReasonSocialIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CompanyName.NameNum);
            }


        }

        if (riskData.Value != null) {
            $("#inputContractValue").val(FormatMoney(riskData.Value.Value));
            if (previousValue == 0) {
                previousValue = riskData.Value.Value;
            }
            if (PreviousContractValue == 0) {
                PreviousContractValue = riskData.Risk.AmountInsured;
            }
        }
        $("#inputContractNumber").val(riskData.SettledNumber);
        var result = RiskSurety.LoadRiskSuretyCombos(riskData, glbPolicy);

        if (riskData.Risk.Text != null) {
            $("#inputText").val(riskData.Risk.Text.TextBody);
        }

        if (riskData.Class != null) {
            var elementSelected = riskData.Class.Id;
            glbClassRiskTempPropierty = riskData.Class.Id;
        }

        if (riskData.Contractor != null) {
            $("#inputSecure").data("Object", riskData.Contractor);
            $("#inputSecure").val(riskData.Contractor.Name + ' (' + riskData.Contractor.IdentificationDocument.Number + ')');
            var asc = RiskSurety.LoadAggregate(riskData.Contractor.IndividualId);//pendiente validar el risk
        }

        if (riskData.Risk.AmountInsured != null) {
            $("#inputTotalSumInsuredRisk").text(FormatMoney(riskData.Risk.AmountInsured));
        }
        $("#inputPremiumRisk").text(FormatMoney(riskData.Risk.Premium));
        if (riskData.Isfacultative) {
            $('#chkFacultativeSurety').attr('checked', true);
        }
        else {
            $('#chkFacultativeSurety').attr('checked', false);
        }
        if (riskData.IsRetention) {
            $('#chkRetentionSurety').attr('checked', true);
        }
        else {
            $('#chkRetentionSurety').attr('checked', false);
        }

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            $('#chkFacultativeSurety').prop('disabled', true);
            $('#chkRetentionSurety').prop('disabled', true);
            $('#inputText').prop('disabled', true);
        }
        if (riskData.RiskSuretyPost != null) {

            if (riskData.RiskSuretyPost.ChkContractDate == true) {
                $('#chkTerminalUnitContract').prop("checked", true);
                $('#datepicker').val(FormatDate(riskData.RiskSuretyPost.ContractDate));

                $("#datepicker").attr("disabled", false);
                $('#chkFinalDeliveryDate').attr("disabled", false);
                $('#chkTerminalUnitContract').attr('disabled', false);
            }
            if (riskData.RiskSuretyPost.ChkContractFinalyDate == true) {
                $('#chkFinalDeliveryDate').prop("checked", true);
                $('#datepicker').val(FormatDate(riskData.RiskSuretyPost.IssueDate));
                if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Emission) {
                    $("#datepicker").attr("disabled", true);
                    $('#chkFinalDeliveryDate').attr("disabled", true);
                    $('#chkTerminalUnitContract').attr('disabled', true);
                    $('#chkFacultativeSurety').attr("disabled", false);
                    $('#chkRetentionSurety').attr('disabled', false);
                }

            }
        }

        var isNational = riskData.Risk.IsNational;


        result.then(function (data) {
            if (isNational) {
                $('#NacionalLevel').attr('checked', true);
                $("#selectSuretyState").attr('disabled', true);
                $("#selectSuretyCity").attr('disabled', true);
                //$("#selectSuretyState").UifSelect("disabled", isNational);
                //$("#selectSuretyCity").UifSelect("disabled", isNational);
            }
            else {
                $('#NacionalLevel').attr('checked', isNational);
            }
        });




        dynamicProperties = riskData.DynamicProperties;
        RiskSurety.UpdateGlbRisk(riskData);
        RiskSurety.LoadSubTitles(0);
    }

    static GetRisksSuretyByTemporalId(temporalId, selectedId, funcionRedirect) {
        RiskSuretyRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {

                    if (data.result.length == 1) {
                        $("#selectRiskSurety").UifSelect({ sourceData: data.result, selectedId: data.result[0].Id });

                        $("#selectRiskSurety").attr("disabled", true);
                        if ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) && gblCurrentRiskTemporalNumberOld == null) {
                            var result = RiskSurety.GetRiskSuretyById(gblCurrentRiskTemporalNumber, data.result[0].Id);
                        }
                        else {
                            var result = RiskSurety.GetRiskSuretyById(glbPolicy.Id, data.result[0].Id);
                        }

                        result.then(function (data) {
                            if (window.TemporalId_Guarantee) {
                                $('#btnCounterGuarantees').click();
                                window.TemporalId_Guarantee = undefined;
                            }
                        });

                    }
                    else if (data.result.length > 1) {
                        $("#selectRiskSurety").UifSelect({ sourceData: data.result });
                        $("#selectRiskSurety").attr("disabled", false);
                        RiskSurety.LoadInitialize();
                    }
                    //End

                } else {
                    RiskSurety.LoadInitialize();
                }
            }
            if (glbPersonOnline != null) {
                RiskSurety.LoadPerson();
            }
            else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                $("#selectRiskSurety").UifSelect("setSelected", $("#selectRiskSurety option[Value!='']")[0].value);
                // RiskSurety.GetRiskSuretyById(temporalId, $("#selectRiskSurety option[Value!='']")[0].value);
            }
            if (funcionRedirect != null)
                funcionRedirect()
        });
    }

    static GetRiskSuretyCoveragesByProductIdGroupCoverageId(productId, groupCoverageId) {
    
        var riskData = RiskSurety.GetRiskDataModel();
        if (typeof (riskData.InsuredId) === "undefined") {
            $('#selectRiskSuretyGroupCoverage').UifSelect('setSelected', null);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectInsured, 'autoclose': true });
        } else if ($("#listCoverages").UifListView('getData') === null || $("#listCoverages").UifListView('getData').length === 0) {

            if (glbRisk.Id == "" || glbRisk.Id == null) {
                glbRisk.Id = 0;
            }
            RiskSuretyRequest.GetRiskSuretyCoveragesByProductIdGroupCoverageId(riskData).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        RiskSurety.LoadCoverages(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ProductHasNotCoverage, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectInsured, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetCoverages, 'autoclose': true });
            });
        }
    }

    static SaveRisk(redirect, coverageId, validateRisk, validateCoverage, validate, inCoverage) {

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            if ($("#inputTotalSumInsuredRisk").text() != null && $("#inputTotalSumInsuredRisk").text() != undefined && $("#inputTotalSumInsuredRisk").text() != "") {
                if (PreviousContractValue > parseFloat(NotFormatMoney($("#inputTotalSumInsuredRisk").text()))) {
                    differenceContract = PreviousContractValue - parseFloat(NotFormatMoney($("#inputTotalSumInsuredRisk").text()));
                    differenceContract = differenceContract * -1
                }
                if (PreviousContractValue < parseFloat(NotFormatMoney($("#inputTotalSumInsuredRisk").text()))) {
                    differenceContract = parseFloat(NotFormatMoney($("#inputTotalSumInsuredRisk").text())) - PreviousContractValue;
                }
            }
        }


        if (RiskSurety.ValidateLimiteQuotaOperative()) {
            // RiskSurety.QuotationRisk();
            if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
                RiskSurety.SaveRiskTempOrQuo(redirect, coverageId, validateRisk, validateCoverage, validate);
            }
            else {
                if (parseFloat(NotFormatMoney($("#inputOperationalCapacity").val())) >= 0
                    || glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && parseFloat(NotFormatMoney($("#inputOperationalCapacity").val())) >= 0 && parseFloat(NotFormatMoney($("#inputAvailable").val())) >= differenceContract) {
                    RiskSurety.SaveRiskEmission(redirect, coverageId, validateRisk, validateCoverage, validate, inCoverage);
                } else {
                    if (gblCurrentRiskTemporalNumber != null) {
                        RiskSurety.SaveRiskEmissionGetTemporalRisk(redirect, coverageId, validateRisk, validateCoverage, validate);
                    }
                    else {
                        if ($("#inputContractValue").val() === undefined || $("#inputContractValue").val() === "") {
                            $.UifNotify('show', { 'type': 'danger', 'message': "Valor Contrato es obligariorio ", 'autoclose': true });
                        }
                        else
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
                    }

                }
            }
        }
    }

    static SaveRiskTempOrQuo(redirect, coverageId, validateRisk, validateCoverage, validate) {
        if (typeof (validateRisk) === "undefined") {
            validateRisk = true;
        }
        if (typeof (validateCoverage) === "undefined") {
            validateCoverage = true;
        }
        if (typeof (validate) === "undefined") {
            validate = true;
        }

        $("#mainRiskSurety").validate();
        if ($("#mainRiskSurety").valid()) {
            if (validateRisk) {

                if ($("#listCoverages").UifListView('getData').length > 0) {
                    if (!RiskSuretyContract.ValidateContractObject()) {
                        return false;
                    }
                }

            }
            if ($('#inputInsured').data("Object") == null) {

                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectInsured, 'autoclose': true });
                return false;
            }
            else if ($('#inputSecure').data("Object") == null) {
                $("#inputOperationalCapacity").val(0);
                $("#inputAccumulation").val(0);
                $("#inputAvailable").val(0);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectSecure, 'autoclose': true });
                return false;
            }
            else if (validateCoverage) {
                if ($("#listCoverages").UifListView('getData').length == 0) {

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateCoverage, 'autoclose': true });
                    return false;
                }
            }

            if ($('#datepicker').val() == "") {

                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MissingDateContract, 'autoclose': true });
                return false;
            }
            //Valida si en el listado de beneficiarios hay uno dado de baja
            if (glbRisk.Beneficiaries != null) {
                if (glbRisk.Beneficiaries.some(x => x.DeclinedDate != null)) {

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInsuredDisabled, 'autoclose': true });
                    return false;
                }
                if (glbRisk.Beneficiaries[0].CustomerType == CustomerType.Prospect) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true });
                    return false;
                }
            }

            var riskData = RiskSurety.GetRiskDataModel();

            if ($('#chkTerminalUnitContract').prop('checked')) {
                //Se agrega para comparar fechas el proceso no continua si la fecha postcontractual es menor a la vdesde de la poliza
                var datePart1 = riskData.TerminalUnitContract.split("/");
                var datePart2 = glbPolicy.CurrentFrom.split("/"); //inicio de vigencia de poliza vista inicial de endoso
                var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
                var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

                if (compare1 < compare2) {

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidationDateTerminalUnitContract, 'autoclose': true });
                    return false;
                }

            }
            else { //Se agrega para comparar fechas el proceso no continua si la fecha postcontractual es menor a la vdesde de la poliza
                var datePart1 = riskData.FinalDeliveryDate.split("/");
                var datePart2 = glbPolicy.CurrentFrom.split("/");
                var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
                var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

                if (compare1 < compare2) {

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidationDateFinalDeliveryDate, 'autoclose': true });
                    return false;
                }

            }

            lockScreen();
            RiskSuretyRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties, validate).done(function (data) {
                if (data.success) {
                    RiskSurety.UpdateGlbRisk(data.result);
                    RiskSurety.LoadSubTitles(0);
                    RiskSurety.ShowSaveRisk(glbRisk.Id, redirect, coverageId);
                    //Al momento de guardar el riesgo, se desabilita la lista de riesgos
                    if ($("#selectRiskSurety").length == 1) {
                        const dataSelectRiskSurety = [];
                        $('#selectRiskSurety').children('option').each(function () {
                            if ($(this).val())
                                dataSelectRiskSurety.push({ Id: $(this).val(), Description: $(this).text() })
                            else if ($(this).val() == glbRisk.Risk.Id)
                                dataSelectRiskSurety.push({ Id: $(this).val(), Description: data.Risk.Description })
                        })
                        $('#selectRiskSurety').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Risk.Id });
                        $("#selectRiskSurety").attr("disabled", true);
                        glbPolicy.Summary.RiskCount = $("#selectRiskSurety").length;
                    }
                    else if ($("#selectRiskSurety").length > 1)
                        $("#selectRiskSurety").attr("disabled", false);
                    glbPolicy.Summary.RiskCount = $("#selectRiskSurety").length;
                    if (data.result.Premium > 0 && redirect == 0 && glbPolicy.Product.CoveredRisk.ScriptId > 0 && glbRisk.RecordScript === false) {
                        RiskSurety.LoadScript();
                    }
                    return true;
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    return false;
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                return false;
            });
        }
    }

    static SaveRiskEmission(redirect, coverageId, validateRisk, validateCoverage, validate, inCoverage) {
        if (parseFloat(NotFormatMoney($("#inputOperationalCapacity").val())) >= 0
            || glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && parseFloat(NotFormatMoney($("#inputOperationalCapacity").val())) >= 0 && parseFloat(NotFormatMoney($("#inputAvailable").val())) >= differenceContract) {

            if (typeof (validateRisk) === "undefined") {
                validateRisk = true;
            }
            if (typeof (validateCoverage) === "undefined") {
                validateCoverage = true;
            }
            if (typeof (validate) === "undefined") {
                validate = true;
            }

            $("#mainRiskSurety").validate();
            if ($("#mainRiskSurety").valid()) {

                if (validateRisk) {

                    if ($("#listCoverages").UifListView('getData').length > 0) {
                        if (!RiskSuretyContract.ValidateContractObject()) {

                            return false;
                        }
                    }

                }
                if ($('#inputInsured').data("Object") == null) {

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectInsured, 'autoclose': true });
                    return false;
                }
                else if ($('#inputSecure').data("Object") == null) {
                    $("#inputOperationalCapacity").val(0);
                    $("#inputAccumulation").val(0);
                    $("#inputAvailable").val(0);

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectSecure, 'autoclose': true });
                    return false;
                }
                else if (validateCoverage) {
                    if ($("#listCoverages").UifListView('getData').length == 0) {

                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateCoverage, 'autoclose': true });
                        return false;
                    }
                }

                if ($('#datepicker').val() == "") {

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MissingDateContract, 'autoclose': true });
                    return false;
                }
                //Valida si en el listado de beneficiarios hay uno dado de baja
                if (glbRisk.Beneficiaries != null) {
                    if (glbRisk.Beneficiaries.some(x => x.DeclinedDate != null)) {

                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInsuredDisabled, 'autoclose': true });
                        return false;
                    }
                    if (glbRisk.Beneficiaries[0].CustomerType == CustomerType.Prospect) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true });
                        return false;
                    }
                }

                var riskData = RiskSurety.GetRiskDataModel();

                if ($('#chkTerminalUnitContract').prop('checked')) {
                    //Se agrega para comparar fechas el proceso no continua si la fecha postcontractual es menor a la vdesde de la poliza
                    var datePart1 = riskData.TerminalUnitContract.split("/");
                    var datePart2 = glbPolicy.CurrentFrom.split("/"); //inicio de vigencia de poliza vista inicial de endoso
                    var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
                    var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

                    if (compare1 < compare2) {

                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidationDateTerminalUnitContract, 'autoclose': true });
                        return false;
                    }

                }
                else { //Se agrega para comparar fechas el proceso no continua si la fecha postcontractual es menor a la vdesde de la poliza
                    var datePart1 = riskData.FinalDeliveryDate.split("/");
                    var datePart2 = glbPolicy.CurrentFrom.split("/");
                    var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
                    var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

                    if (compare1 < compare2) {

                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidationDateFinalDeliveryDate, 'autoclose': true });
                        return false;
                    }

                }
                if (inCoverage != true) {
                    if (ValidityParticipant1 != undefined && ValidityParticipant1 != null) {
                        if (ValidityParticipant1.some(x => x.consortiumEventDTO.IsConsortium == true)//valida si la participacion del Integrante no es suficiente frente a la emision
                            || ValidityParticipant1.some(x => x.IndividualOperatingQuota.IndividualID == 0)// valida si uno de los consorciados no registro de cupo para el ramo
                            || ValidityParticipant1.some(x => CompareDateEquals(x.IndividualOperatingQuota.EndDateOpQuota), glbPolicy.CurrentFrom)) {// valida vencimiento del cupo delos consorciados
                            OperationQuotaCumulus.SetInformationAfianzado()
                            return;
                        }
                    }
                    inCoverage = parseInt(NotFormatMoney($('#inputTotalSumInsuredRisk').text())) <= parseFloat(NotFormatMoney($("#inputAvailable").val())) || (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && differenceContract <= parseFloat(NotFormatMoney($("#inputAvailable").val())));
                }

                if (EconomicGroupEvent != undefined && !CompareDateEquals(EconomicGroupEvent.DateTo, glbPolicy.CurrentFrom)) { // valida vigencia del cupo
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.QuotaIsNotCurrent, 'autoclose': true });
                    return;
                } else if (OperationQuotaEvent != null && ConsortiumEvent == undefined && !CompareDateEquals(OperationQuotaEvent.DateTo, glbPolicy.CurrentFrom)) { // valida vigencia del cupo
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.QuotaIsNotCurrent, 'autoclose': true });
                    return;
                }

                lockScreen();

                //if (inCoverage != true) {
                //    inCoverage = parseInt(NotFormatMoney($('#inputTotalSumInsuredRisk').text())) <= parseFloat(NotFormatMoney($("#inputAvailable").val())) || (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && differenceContract <= parseFloat(NotFormatMoney($("#inputAvailable").val())));
                //}

                if (inCoverage) {
                    RiskSuretyRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties, validate).done(function (data) {
                        if (data.success) {
                            RiskSurety.UpdateGlbRisk(data.result);
                            RiskSurety.LoadSubTitles(0);
                            RiskSurety.ShowSaveRisk(glbRisk.Id, redirect, coverageId);

                            //Al momento de guardar el riesgo, se desabilita la lista de riesgos
                            const dataSelectRiskSurety = [];
                            $('#selectRiskSurety').children('option').each(function () {
                                if ($(this).val() == glbRisk.Risk.Id)
                                    dataSelectRiskSurety.push({ Id: $(this).val(), Description: data.result.Description })
                                else if ($(this).val())
                                    dataSelectRiskSurety.push({ Id: $(this).val(), Description: $(this).text() })
                            })
                            $('#selectRiskSurety').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Risk.Id });
                            if (dataSelectRiskSurety.length == 1) {
                                $('#selectRiskSurety').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Risk.Id, enable: false });
                                glbPolicy.Summary.RiskCount = $("#selectRiskSurety").length;
                            }
                            else if (dataSelectRiskSurety.length > 1)
                                $('#selectRiskSurety').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Risk.Id });
                            glbPolicy.Summary.RiskCount = $("#selectRiskSurety").length;
                            if (data.result.Premium > 0 && redirect == 0 && glbPolicy.Product.CoveredRisk.ScriptId > 0 && glbRisk.RecordScript === false) {
                                RiskSurety.LoadScript();
                            }
                            if (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumberOld == null) {
                                gblCurrentRiskTemporalNumberOld = gblCurrentRiskTemporalNumber;
                                gblCurrentRiskTemporalNumber = data.result.Id;

                            }
                            return true;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            return false;
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                        return false;
                    });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
                }
                unlockScreen();
            }
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
        }
    }

    static SaveRiskEmissionGetTemporalRisk(redirect, coverageId, validateRisk, validateCoverage, validate) {
        if (validateRisk) {

            if ($("#listCoverages").UifListView('getData').length > 0) {
                if (!RiskSuretyContract.ValidateContractObject()) {

                    return false;
                }
            }

        }
        if (glbRisk.MainInsured == null) {

            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectInsured, 'autoclose': true });
            return false;
        }
        else if (glbRisk.Contractor == null) {
            $("#inputOperationalCapacity").val(0);
            $("#inputAccumulation").val(0);
            $("#inputAvailable").val(0);

            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectSecure, 'autoclose': true });
            return false;
        }
        else if (validateCoverage) {
            if (glbRisk.Coverages.length == 0) {

                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateCoverage, 'autoclose': true });
                return false;
            }
        }

        if ($('#datepicker').val() == "") {

            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MissingDateContract, 'autoclose': true });
            return false;
        }
        //Valida si en el listado de beneficiarios hay uno dado de baja
        if (glbRisk.Beneficiaries != null) {
            if (glbRisk.Beneficiaries.some(x => x.DeclinedDate != null)) {

                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInsuredDisabled, 'autoclose': true });
                return false;
            }
            if (glbRisk.Beneficiaries[0].CustomerType == CustomerType.Prospect) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true });
                return false;
            }
        }
        var riskData = RiskSurety.GetTemporalRiskDataModel()
        RiskSuretyRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties, validate).done(function (data) {
            if (data.success) {
                RiskSurety.UpdateGlbRisk(data.result);
                RiskSurety.LoadSubTitles(0);
                RiskSurety.ShowSaveRisk(glbRisk.Id, redirect, coverageId);

                //Al momento de guardar el riesgo, se desabilita la lista de riesgos
                if ($("#selectRiskSurety").length == 1) {
                    $('#selectRiskSurety').UifSelect("setSelected", glbRisk.Id);
                    $("#selectRiskSurety").attr("disabled", true);
                    glbPolicy.Summary.RiskCount = $("#selectRiskSurety").length;
                }
                else if ($("#selectRiskSurety").length > 1)
                    $("#selectRiskSurety").attr("disabled", false);
                (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumber != undefined) ? glbPolicy.Summary.RiskCount = 1 : glbPolicy.Summary.RiskCount = 0;
                $("#modalRiskFromPolicy").UifModal("hide");
                $("#selectedInclusionRisk").text(glbPolicy.Summary.RiskCount);
                if (data.result.Premium > 0 && redirect == 0 && glbPolicy.Product.CoveredRisk.ScriptId > 0 && glbRisk.RecordScript === false) {
                    RiskSurety.LoadScript();
                }
                if (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumberOld == null) {
                    gblCurrentRiskTemporalNumberOld = gblCurrentRiskTemporalNumber;
                    gblCurrentRiskTemporalNumber = data.result.Id;

                }
                unlockScreen();
                return true;
            }
            else {
                unlockScreen();
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                return false;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            unlockScreen();
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
            return false;
        });
    }

    static UpdatePolicyComponents(Redir) {

        RiskSuretyRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                if (Redir) {
                    RiskSurety.ReturnUnderwritingRiskSurety();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
    }

    static ReturnUnderwritingRiskSurety() {
        var url = '';

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "SuretyModification";
            router.run("prtModification");
        }
        else {

            if ((glbPolicy.TemporalType == TemporalType.Quotation && !window.isPolicy) || (glbPolicy.TemporalType == TemporalType.TempQuotation && !window.isPolicy)) {

                window.isPolicy = undefined;
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
            Object: "RiskSuretyCoverage",
            Class: RiskSuretyCoverage
        }
        router.run("prtCoverageRiskSurety");
    }

    static CoverageModelView() {
        var premiumRisk = 0;
        var TotalSumInsured = 0;
        var ContractValue = 0;
        $("#inputPremiumRisk").val($("#inputPremiumRisk").val() == "" ? 0 : $("#inputPremiumRisk").val());
        premiumRisk = NotFormatMoney($("#inputPremiumRisk").val()).replace(separatorDecimal, ".");
        premiumRisk.replace(separatorDecimal, ".");
        TotalSumInsured = NotFormatMoney($("#inputTotalSumInsuredRisk").text());
        TotalSumInsured.replace(separatorDecimal, ".");
        ContractValue = NotFormatMoney(parseFloat($("#inputContractValue").val())).replace(separatorDecimal, ".");
        ContractValue.replace(separatorDecimal, ".");

    }

    static ClearControlsRiskSurety() {

        $("#selectRiskSurety").UifSelect("setSelected", "");
        $("#selectRiskSurety").attr("disabled", false);
        $("#inputInsured").val("");
        $("#inputContractValue").val("");
        $("#inputContractNumber").val("");
        $("#selectContractType").UifSelect("setSelected", null);
        $("#selectSuretyCountry").UifSelect("setSelected", null);
        $("#selectSuretyState").UifSelect("setSelected", null);
        $("#selectSuretyCity").UifSelect("setSelected", null);
        $("#selectClassofContract").UifSelect("setSelected", null);
        $("#inputContractNumber").val("");
        $("#selectRiskSuretyGroupCoverage").UifSelect("setSelected", "");
        $("#inputTotalSumInsuredRisk").text(0);
        $("#inputPremiumRisk").text(0);
        $("#tableIndividualDetails").UifDataTable('clear');
        $("#listCoverages").UifListView("refresh");
        $("#chkFacultativeSurety").val(false);
        $("#chkRetentionSurety").val(false);
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
        RiskSurety.getReasonSocialIndividualId(-1, 1);
        glbRisk.RecordScript = false;
        RiskSurety.UpdateGlbRisk({ Id: 0 });
        if (glbPolicy.Holder.IndividualId != "0") {
            individualSearchType = 3;
            RiskSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        }
    }

    static ShowDetailInsuredRiskSurety() {
        searchHolderTo = 'inputInsured';
        $('#tableIndividualDetails').UifDataTable('clear');
        let insured = $("#inputInsured").data('Object');
        RiskSurety.LoadHolderDetails(insured);
        $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelInsuredDetail);
    }

    static ShowSecureDetails() {
        searchHolderTo = 'inputSecure';
        $('#tableIndividualDetails').UifDataTable('clear');
        let secure = $("#inputSecure").data("Object");
        RiskSurety.LoadHolderDetails(secure);
    }

    static LoadHolderDetails(holder) {
        if (holder.details != null && holder.details.length > 0) {
            if (holder.CompanyName != null) {
                $.each(holder.details, function (id, item) {
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

                    $('#tableIndividualDetails').UifDataTable('addRow', item)
                    if (holder.CompanyName.NameNum == this.NameNum) {
                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                    else if (holder.CompanyName.NameNum == 0 && this.IsMain == true) {
                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                });
            }

            if ($('#tableIndividualDetails').UifDataTable('getSelected') == null) {
                $('#tableIndividualDetails tbody tr:eq(0)').removeClass('row-selected').addClass('row-selected');
                $('#tableIndividualDetails tbody tr:eq(0) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
            }

            $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelSecureDetail);
        }
    }

    static ShowDetailSecureRiskSurety() {
        $('#tableIndividualDetails').UifDataTable('clear');

        let insured = {}
        if (individualSearchType == 1) {
            insured = $("#inputInsured").data("Object");
        }
        else if (individualSearchType == 2) {
            insured = $("#inputBeneficiaryName").data("Object");
        }
        else if (individualSearchType == 3) {
            insured = $("#inputAdditionalDataInsured").data("Object");
        }

        if (insured != null) {
            $("#inputSecure").data("Detail", RiskSurety.GetIndividualDetails(insured.details));
            var secureDetails = insured.details;
            var arraySecureDetails = [];
            arraySecureDetails.push(secureDetails);
            if (arraySecureDetails.length > 0) {
                $.each(arraySecureDetails, function (id, item) {
                    $('#tableIndividualDetails').UifDataTable('addRow', item)
                    if (insured > 0 && insured.CompanyName.NameNum == this.NameNum) {
                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                    else if (insured.CompanyName.NameNum == 0 && this.IsMain == true) {
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

        $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelSecureDetail);
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        var resultData = {};
        RiskSuretyRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });

        return resultData;
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
                $("#inputSecure").data("Object").CompanyName = details[0];
            }

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    static DeleteRisk() {
        if ($("#selectRiskSurety").val() > 0) {
            lockScreen();
            RiskSuretyRequest.DeleteRisk(glbPolicy.Id, $("#selectRiskSurety").val()).done(function (data) {
                if (data.success) {

                    RiskSurety.ClearControlsRiskSurety();
                    if (glbRisk == null) {
                        glbRisk = { Id: 0, Object: "RiskSurety", formRisk: "#mainRiskSurety", RecordScript: false, Class: RiskSurety };
                    }

                    RiskSurety.setDefault();
                    RiskSurety.LoadSubTitles(0);
                    RiskSurety.GetRisksSuretyByTemporalId(glbPolicy.Id, glbRisk.id);
                    RiskSurety.DisableControls(false);
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                    ScrollTop();
                }
                else {

                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {

                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteRisk, 'autoclose': true });
            });
        }
        else
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRiskNoExist, 'autoclose': true });
    }

    static DeleteCoverage(data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        } else {

            var coverages = $("#listCoverages").UifListView('getData');
            if (coverages != null && coverages != "" && coverages.length == 1 && data.EndorsementType == EndorsementType.Modification) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
            }
            else {

                $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });

                if (data.EndorsementType == EndorsementType.Modification) {
                    if (data.RiskCoverageId > 0) {
                        var coverage = RiskSurety.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description)
                        $.when(coverage).done(function (coverageData) {
                            coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount);
                            coverageData.Rate = FormatMoney(coverageData.Rate);
                            coverageData.ContractAmountPercentage = FormatMoney(coverageData.ContractAmountPercentage);
                            coverageData.CurrentTo = FormatDate(coverageData.CurrentTo);
                            coverageData.CurrentFrom = FormatDate(coverageData.CurrentFrom);
                            coverageData.LimitAmount = FormatMoney(coverageData.LimitAmount);
                            $.each(coverages, function (index, value) {
                                if (this.Id == coverageData.Id) {
                                    coverages[index] = coverageData
                                }
                            });
                            $("#listCoverages").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });

                            var premium = RiskSurety.PremiumRiskTotal();
                            var sumInsured = RiskSurety.CalculateInsuredSum();
                            $("#inputTotalSumInsuredRisk").text(FormatMoney(sumInsured));
                            $("#inputPremiumRisk").text(FormatMoney(premium));
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                        });
                    }
                    else {
                        $.each(coverages, function (index, value) {
                            if (this.Id != data.Id) {
                                $("#listCoverages").UifListView("addItem", this);
                            }
                        });
                        var premium = RiskSurety.PremiumRiskTotal();
                        var sumInsured = RiskSurety.CalculateInsuredSum();
                        $("#inputTotalSumInsuredRisk").text(FormatMoney(sumInsured));
                        $("#inputPremiumRisk").text(FormatMoney(premium));
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                    }

                }
                else {
                    $.each(coverages, function (index, value) {
                        if (this.Id != data.Id) {
                            $("#listCoverages").UifListView("addItem", this);
                        }
                    });
                    if ($("#listCoverages").UifListView('getData').length > 0) {
                        RiskSuretyCoverage.GetListCoverageTemporal("listCoverages").done(function (data) {
                            if (data.success) {
                                var coverageList = RiskSurety.formatCoverage(data.result);
                                $("#listCoverages").UifListView({ sourceData: coverageList, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });
                                var premium = RiskSurety.PremiumRiskTotal();
                                var sumInsured = RiskSurety.CalculateInsuredSum();
                                glbRisk.AmountInsured = sumInsured;
                                $("#inputTotalSumInsuredRisk").text(FormatMoney(sumInsured));
                                $("#inputPremiumRisk").text(FormatMoney(premium));
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        }).fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
                        });
                    }
                    else
                        RiskSurety.TotalPremiumAmount();
                }
            }

        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var covergaExcluded = $.Deferred();

        RiskSuretyRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
            if (data.success) {
                covergaExcluded.resolve(data.result);
            }
            else {
                covergaExcluded.reject(AppResources.ErrorExcludeCoverage);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorExcludeCoverage, 'autoclose': true });
        });
        return covergaExcluded.promise();
    }

    static LoadCoverages(coverages) {
        if (coverages != null) {
            //$.each(coverages, function (index, val) {
            Object.keys(coverages).forEach(function (index) {
                coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
                coverages[index].OriginalSubLimitAmount = FormatMoney(coverages[index].OriginalSubLimitAmount);
                coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);
                coverages[index].Rate = FormatMoney(coverages[index].Rate);
                coverages[index].LimitAmount = FormatMoney(coverages[index].LimitAmount);
                coverages[index].DeclaredAmount = FormatMoney(coverages[index].DeclaredAmount);
                coverages[index].EndorsementLimitAmount = FormatMoney(coverages[index].EndorsementLimitAmount);
                coverages[index].EndorsementSublimitAmount = FormatMoney(coverages[index].EndorsementSublimitAmount);
                coverages[index].CurrentFrom = FormatDate(coverages[index].CurrentFrom);
                coverages[index].CurrentTo = FormatDate(coverages[index].CurrentTo);
                coverages[index].OriginalCurrentFrom = FormatDate(coverages[index].OriginalCurrentFrom);
                coverages[index].OriginalCurrentTo = FormatDate(coverages[index].OriginalCurrentTo);
                coverages[index].CoverStatusName = coverages[index].CoverStatusName;
                coverages[index].ContractAmountPercentage = FormatMoney(coverages[index].ContractAmountPercentage);
                //if (coverages[index].CoverStatus == CoverageStatus.NotModified && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                //    coverages[index].EndorsementLimitAmount = FormatMoney(0);
                //    coverages[index].EndorsementSublimitAmount = FormatMoney(0);
                //    //coverages[index].ContractAmountPercentage = FormatMoney(0);
                //}
            });
            $("#listCoverages").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
        }
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

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static validateOperatingQuota() {
        var avalaible = parseFloat($('#inputAvailable').val());
        operatingQuota = $('#inputOperationalCapacity').val();//Almacena el valor de la cuota Operativa del formulario anterior
        if (operatingQuota == 0 && RiskSurety.VerifyBusinessProcess()) {
            if (($('#inputOperationalCapacity').val() == 0) || ($.trim($("#inputOperationalCapacity").val()) == "")) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateOperatingQuota, 'autoclose': true });
                $("#inputAccumulation").val(0);
                $("#inputAvaliableOperationQuota").val(0);
                //Disponible=Cupo Oerativo - Comulo - la suma del Riesgo
                $("#inputAvailable").val(0);
                return false;
            }
            else if (/*(avalaible <= 0) ||*/ ($.trim($("#inputAvailable").val()) == "")) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
                return false;
            }
        }
        return true;
    }

    static GetIndividualDetails(individualDetails) {
        if (individualDetails != undefined && individualDetails != null) {
            if ($.type(individualDetails) === "array") {
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
                        if ($("#inputSecure").data("Object") != null && $("#inputSecure").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputSecure").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputSecure").data("Object") != null && $("#inputSecure").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputSecure").data("Object").CompanyName = this;
                        }
                    }
                    else if (individualSearchType == 2) {
                        if ($("#inputInsured").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputInsured").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputInsured").data("Object").CompanyName = this;
                        }


                    }
                    else if (individualSearchType == 3) {
                        if ($("#inputBeneficiaryName").data("Object") != null && $("#inputBeneficiaryName").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputBeneficiaryName").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = this;
                        }
                    }
                });
            }
            else {

                if (individualDetails.Address != null && individualDetails.Address.Description != null) {
                    individualDetails.Detail = 'Dirección: ' + individualDetails.Address.Description + '<br/>';
                }
                else {
                    individualDetails.Detail = 'Dirección: -- <br/>';
                }

                //if (individualDetails.TradeName != null && individualDetails.TradeName.Description ) {
                //    individualDetails.Detail += '<b> :' + individualDetails.TradeName + '<br/>' ;
                //}
                //else {
                //    individualDetails.Detail += '<b> : -- <br/>' ;
                //}

                if (individualDetails.Phone != null && individualDetails.Phone.Description) {
                    individualDetails.Detail += '<b> Teléfono: ' + individualDetails.Phone.Description + '<br/>';
                }
                else {
                    individualDetails.Detail += '<b> Teléfono: --  <br/>';
                }

                if (individualDetails.Email != null && individualDetails.Email.Description) {
                    individualDetails.Detail += '<b> Correo: ' + individualDetails.Email.Description;
                }
                else {
                    individualDetails.Detail += '<b> Correo: --';
                }

                if (individualSearchType == 1) {
                    if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName == null) {
                        if (individualDetails.IsMain) {
                            $("#inputInsured").data("Object").CompanyName = individualDetails;
                        }
                    }
                    else if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName.NameNum == 0 && individualDetails.IsMain) {
                        $("#inputInsured").data("Object").CompanyName = individualDetails;
                    }
                }
                else if (individualSearchType == 2) {
                    if ($("#inputBeneficiaryName").data("Object") != null && $("#inputBeneficiaryName").data("Object").CompanyName == null) {
                        if (individualDetails.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = individualDetails;
                        }
                    }
                    else if ($("#inputBeneficiaryName").data("Object") != null && $("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && individualDetails.IsMain) {
                        $("#inputBeneficiaryName").data("Object").CompanyName = individualDetails;
                    }
                }
                else if (individualSearchType == 3) {
                    if ($("#inputSecure").data("Object") != null && $("#inputSecure").data("Object").CompanyName == null) {
                        if (individualDetails.IsMain) {
                            $("#inputSecure").data("Object").CompanyName = individualDetails;
                        }
                    }
                    else if ($("#inputSecure").data("Object") != null && $("#inputSecure").data("Object").CompanyName.NameNum == 0 && individualDetails.IsMain) {
                        $("#inputSecure").data("Object").CompanyName = individualDetails;
                    }
                }
            }
        }

        return individualDetails;
    }

    static DisableControls(disabled) {
        $('#inputSecure').prop('disabled', disabled);
        $('#inputInsured').prop('disabled', disabled);
        $('#selectContractType').prop('disabled', disabled);
        $('#selectClassofContract').prop('disabled', disabled);
        $('#inputContractNumber').prop('disabled', disabled);
        $('#chkFacultativeSurety').prop('disabled', disabled);
        $('#chkRetentionSurety').prop('disabled', disabled);
        $('#selectSuretyCountry').prop('disabled', disabled);
        $('#selectSuretyState').prop('disabled', disabled);
        $('#selectSuretyCity').prop('disabled', disabled);

    }

    static Redirect(redirect, riskId, coverageId) {
        switch (redirect) {
            case MenuType.Underwriting:
                RiskSurety.ReturnUnderwritingRiskSurety();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
    
                break;
            case MenuType.Coverage:
                RiskSurety.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskSurety.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.Texts:
                RiskSurety.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.CrossGuarantees:
                RiskSurety.ShowPanelsRisk(MenuType.CrossGuarantees);
                break;
            case MenuType.ContractObject:
                RiskSurety.ShowPanelsRisk(MenuType.ContractObject);
                break;
            case MenuType.Script:
                RiskSurety.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                break;
            //$.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
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
            if (glbRisk.ContractObject != null) {
                if (glbRisk.ContractObject.TextBody == null) {
                    glbRisk.ContractObject.TextBody = "";
                }
                if (glbRisk.ContractObject.TextBody.length > 0) {
                    $("#selectedContractObject").text("(" + AppResources.LabelWithObject + ")");
                }
                else {
                    $('#selectedContractObject').text("(" + AppResources.LabelWithoutData + ")");
                }
            } else {
                $("#selectedContractObject").text("(" + AppResources.LabelWithoutData + ")");
            }
        }
        if (subTitle == 0 || subTitle == 5) {
            if (glbRisk.Guarantees != null) {
                if (glbRisk.Guarantees.length > 0)
                    $("#selecteCounterGuarantees").text("(" + glbRisk.Guarantees.length + ")");
            } else {
                $("#selecteCounterGuarantees").text("(" + AppResources.LabelWithoutData + ")");
            }
        }
    }

    static SaveHolderGuarantee() {
        policyDataGuarantee = {};
        if ($('#inputSecure').data("Object") != null) {
            policyDataGuarantee.HolderId = $("#inputSecure").data("Object").IndividualId;
            policyDataGuarantee.HolderIdentificationDocument = $("#inputSecure").data("Object").IdentificationDocument.Number;
            if ($("#inputSecure").data("Object").Name != null)
                policyDataGuarantee.HolderName = $("#inputSecure").data("Object").Name;
            else
                policyDataGuarantee.HolderName = $("#inputSecure").data("Object").TradeName;
            if ($("#inputSecure").data("Object").IdentificationDocument.DocumentType != null)
                policyDataGuarantee.documentType = $("#inputSecure").data("Object").IdentificationDocument.DocumentType.Id;
            if ($("#inputSecure").data("Object").Address != null)
                policyDataGuarantee.HolderAddress = $("#inputSecure").data("Object").Address.Description;
            if ($("#inputSecure").data("Object").Phone != null)
                policyDataGuarantee.HolderPhone = $("#inputSecure").data("Object").Phone.Description;
        }
    }

    //Tarifacion
    static QuotationCoverages(CoveragesSurety, ContractValue) {
        if (CoveragesSurety != null) {
            var coverages = RiskSurety.UnformatCoverage(CoveragesSurety);
            RiskSuretyRequest.QuotationCoverages(glbPolicy.Id, glbRisk.Id, coverages, ContractValue, glbPolicy.Endorsement.PolicyId).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        var coveragesTarif = RiskSurety.formatCoverage(data.result);
                        $("#listCoverages").UifListView({ sourceData: coveragesTarif, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
                        RiskSurety.TotalPremiumAmount();
                    }

                }
                else {
                    error = true;
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static TotalPremiumAmount() {
        var premium = RiskSurety.PremiumRiskTotal();
        var sumInsured = RiskSurety.CalculateInsuredSum();
        $("#inputTotalSumInsuredRisk").text(FormatMoney(sumInsured));
        $("#inputPremiumRisk").text(FormatMoney(premium));
    }


    static CreateCoverageModel(coverage) {
        var coverageModel = {};
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;
        coverageModel.CoverageId = coverage.Id;
        coverageModel.Description = coverage.Id;
        coverageModel.CurrentFrom = coverage.CurrentFrom;
        coverageModel.CurrentTo = coverage.CurrentTo;
        coverageModel.LimitAmount = coverage.LimitAmount;
        coverageModel.SubLimitAmount = coverage.SubLimitAmount;
        coverageModel.DeclaredAmount = coverage.DeclaredAmount;
        coverageModel.EndorsementLimitAmount = coverage.EndorsementLimitAmount;
        coverageModel.EndorsementSublimitAmount = coverage.EndorsementSublimitAmount;
        coverageModel.Rate = coverage.Rate;
        coverageModel.PremiumAmount = coverage.PremiumAmount;
        coverageModel.ContractAmountPercentage = coverage.ContractAmountPercentage;
        coverageModel.CalculationTypeId = coverage.CalculationType;
        coverageModel.RateType = coverage.RateType;
        coverageModel.CalculationTypeId = coverage.CalculationType;
        coverageModel.CoverStatus = coverage.CoverStatus;
        return coverageModel;
    }


    static CalculateCoverageInsuredSum(coverages) {
        var Amount = 0;
        $.each(coverages, function (key, value) {
            Amount += this.LimitAmount;
        });
        $("#inputTotalSumInsuredRisk").text(FormatMoney(Amount));
    }

    static CalculatePremiumRiskCoverage(coverages) {
        var premium = 0;
        $.each(coverages, function (key, value) {
            premium += this.PremiumAmount
        });
        $("#inputPremiumRisk").text(FormatMoney(premium));
    }

    static GetRiskDataModel() {
        var riskData = $("#mainRiskSurety").serializeObject();
        riskData.TemporalId = glbPolicy.Id;
        if ($('#inputInsured').data("Object") != null) {
            riskData.InsuredName = $('#inputInsured').data("Object").Name;
            riskData.InsuredId = $('#inputInsured').data("Object").IndividualId;
            riskData.InsuredCustomerType = $("#inputInsured").data("Object").CustomerType;
            riskData.InsuredIndividualType = $("#inputInsured").data("Object").IndividualType;
            if ($("#inputInsured").data("Object").BirthDate != null) {
                riskData.InsuredBirthDate = FormatDate($("#inputInsured").data("Object").BirthDate);
            }
            if ($("#inputInsured").data("Object").Gender != null) {
                riskData.InsuredGender = $("#inputInsured").data("Object").Gender;
            }

            if ($("#inputInsured").data("Object").AssociationType != null) {
                riskData.InsuredAssociationTypeId = $("#inputInsured").data("Object").AssociationType.Id;
            }

            if ($("#inputInsured").data("Object").IdentificationDocument != null) {
                riskData.InsuredDocumentTypeId = $("#inputInsured").data("Object").IdentificationDocument.DocumentType.Id;
                riskData.InsuredDocumentTypeDescription = $("#inputInsured").data("Object").IdentificationDocument.DocumentType.Description;
                riskData.InsuredIdentificationDocument = $("#inputInsured").data("Object").IdentificationDocument.Number;
            }
            if ($("#inputInsured").data("Object").CompanyName != null) {
                if ($("#inputName").UifSelect("getSelected") != 0 && $("#inputName").UifSelect("getSelected") != null) { riskData.InsuredDetailId = $("#inputName").UifSelect("getSelected"); }
                else { riskData.InsuredDetailId = $("#inputInsured").data("Object").CompanyName.NameNum; }

                riskData.InsuredAddressId = glbRisk.AddressId;
                if ($("#inputInsured").data("Object").CompanyName != null) {
                    if ($("#inputInsured").data("Object").CompanyName.Address != null && glbRisk.AddressId == undefined) {
                        riskData.InsuredAddressId = $("#inputInsured").data("Object").CompanyName.Address.Id;
                    }
                }
                if ($("#inputInsured").data("Object").CompanyName.Phone != null) {
                    if ($("#inputPhone").data("Object") != undefined && $("#inputPhone").data("Object") != null) {
                        riskData.InsuredPhoneId = $("#inputPhone").data("Object").Id;
                    }
                    else if (glbRisk.PhoneID != undefined) {
                        riskData.InsuredPhoneId = glbRisk.PhoneID;
                    }
                    else if ($("#inputName").data() != null && $("#inputName").data().config != undefined && $("#inputName").data().config != null
                        && $("#inputName").data().config.sourceData != undefined && $("#inputName").data().config.sourceData != null && $("#inputName").data().config.sourceData.length > 0) {
                        var reasonSocial = $("#inputName").data().config.sourceData.find(function (item) {
                            return item.NameNum == riskData.InsuredDetailId
                        });
                        if (reasonSocial.PhoneID != undefined && reasonSocial.PhoneID != null && reasonSocial.PhoneID > 0) {
                            riskData.InsuredPhoneId = reasonSocial.PhoneID;
                        }
                        else {
                            riskData.InsuredPhoneId = $("#inputInsured").data("Object").CompanyName.Phone.Id;
                        }
                    }
                }
                if ($("#inputInsured").data("Object").CompanyName.Email != null) {
                    riskData.InsuredEmailId = $("#inputInsured").data("Object").CompanyName.Email.Id;
                }
            }
        }

        if ($('#inputSecure').data("Object") != null) {

            riskData.ContractorId = $('#inputSecure').data("Object").IndividualId;
            riskData.ContractorIdentificationDocument = $("#inputSecure").data("Object").IdentificationDocument.Number;
            riskData.ContractorDocumentTypeId = $("#inputSecure").data("Object").IdentificationDocument.DocumentType.Id;
            riskData.ContractorDocumentTypeDescription = $("#inputSecure").data("Object").IdentificationDocument.DocumentType.Description;
            riskData.ContractorName = $("#inputSecure").data("Object").Name;
            riskData.ContractorInsuredId = $('#inputSecure').data("Object").InsuredId;
            riskData.ContractorCustomerType = $('#inputSecure').data("Object").CustomerType;
            riskData.ContractorIndividualType = $('#inputSecure').data("Object").IndividualType;
            if ($("#inputInsured").data("Object") != undefined && $("#inputInsured").data("Object").AssociationType != null) {
                riskData.ContractorAssociationTypeId = $("#inputInsured").data("Object").AssociationType.Id;
            }
            if ($('#inputSecure').data("Object").CompanyName != null) {
                riskData.ContractorDetailId = $('#inputSecure').data("Object").CompanyName.NameNum;
                riskData.ContractorAddressId = $("#inputSecure").data("Object").CompanyName.Address.Id;
                riskData.ContractorAddressDescription = $("#inputSecure").data("Object").CompanyName.Address.Description;
                riskData.ContractorPhoneDescription = $("#inputSecure").data("Object").CompanyName.Phone != null ? $("#inputSecure").data("Object").CompanyName.Phone.Description : "";
            } else {
                riskData.ContractorDetailId = 0;
            }
            riskData.CustomerType = $('#inputSecure').data("Object").CustomerType;
        }

        riskData.OperatingQuota = NotFormatMoney($('#inputOperationalCapacity').val());
        riskData.Aggregate = NotFormatMoney($('#inputAccumulation').val());
        riskData.Available = NotFormatMoney($('#inputAvailable').val());
        riskData.AvailableOperatingQuota = NotFormatMoney($('#inputAvaliableOperationQuota').val()); // si existe
        //riskData.ContractValue = parseFloat(NotFormatMoney($("#inputContractValue").val()));
        riskData.ContractValue = NotFormatMoney($("#inputContractValue").val());
        riskData.ContractNumber = $("#inputContractNumber").val();
        if ($("#inputTotalSumInsuredRisk").text() != null && $("#inputTotalSumInsuredRisk").text() != undefined && $("#inputTotalSumInsuredRisk").text() != "" && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            riskData.AmountInsured = differenceContract;
        }
        riskData.AmountInsured = NotFormatMoney($('#inputTotalSumInsuredRisk').text());
        riskData.Premium = NotFormatMoney($('#inputPremiumRisk').text());
        riskData.IsfacultativeId = $('#chkFacultativeSurety').is(':checked');
        riskData.IsRetention = $('#chkRetentionSurety').is(':checked');
        riskData.CountryId = $("#selectSuretyCountry").UifSelect("getSelected");
        riskData.StateId = $("#selectSuretyState").UifSelect("getSelected");
        riskData.CityId = $("#selectSuretyCity").UifSelect("getSelected");
        riskData.IsNational = $('#NacionalLevel').prop('checked');

        if ($('#chkTerminalUnitContract').prop('checked')) {
            riskData.TerminalUnitContract = FormatDate($('#datepicker').val());
            riskData.ChkContractDate = $('#chkTerminalUnitContract').is(':checked');

        }
        if ($('#chkFinalDeliveryDate').prop('checked')) {
            riskData.FinalDeliveryDate = FormatDate($('#datepicker').val());
            riskData.ChkContractFinalyDate = $('#chkFinalDeliveryDate').is(':checked');
        }

        if ($('#inputText').val() != null) {
            riskData.ContractObject = $('#inputText').val();
        }

        var coveragesValues = RiskSurety.UnformatCoverage($("#listCoverages").UifListView('getData'));
        riskData.Coverages = coveragesValues;
        //$.each(riskData.Coverages, function (key, value) {
        //    if (value.CoverStatus == 5) {
        //        if (value.PremiumAmount != null && value.PremiumAmount != 0) {
        //            value.EndorsementLimitAmount = value.LimitAmount;
        //            value.EndorsementSublimitAmount = value.SubLimitAmount;
        //        }
        //        else {
        //            value.EndorsementLimitAmount = 0;
        //            value.EndorsementSublimitAmount = 0;
        //        }
        //    }
        //});

        ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) != gblCurrentRiskTemporalNumberOld == null) ? riskData.RiskId = 0 : riskData.RiskId = glbRisk.Id;
        return riskData;
    }

    static GetTemporalRiskDataModel() {
        var riskData = $("#mainRiskSurety").serializeObject();
        riskData.ContractType = glbRisk.ContractType.Id;
        riskData.chkNacionalLevel = glbRisk.IsNational;
        riskData.GroupCoverage = glbRisk.Coverages.length;
        riskData.TemporalId = glbPolicy.Id;
        riskData.Class = glbClassRiskTempPropierty;
        if (glbRisk.MainInsured != null) {
            riskData.InsuredName = glbRisk.MainInsured.Name
            riskData.InsuredId = glbRisk.MainInsured.IndividualId;
            riskData.InsuredCustomerType = glbRisk.MainInsured.CustomerType;
            riskData.InsuredIndividualType = glbRisk.MainInsured.IndividualType;
            if (glbRisk.MainInsured.BirthDate != null) {
                riskData.InsuredBirthDate = FormatDate(glbRisk.MainInsured.BirthDate);
            }
            if (glbRisk.MainInsured.Gender != null) {
                riskData.InsuredGender = glbRisk.MainInsured.Gender;
            }
            if (glbRisk.MainInsured.IdentificationDocument != null) {
                riskData.InsuredIdentificationDocument = glbRisk.MainInsured.IdentificationDocument.Number;
            }
            if (glbRisk.MainInsured.CompanyName != null) {
                riskData.InsuredDetailId = glbRisk.MainInsured.CompanyName.NameNum;
                riskData.InsuredAddressId = glbRisk.MainInsured.CompanyName.Address.Id;
                if (glbRisk.MainInsured.CompanyName.Phone != null) {
                    riskData.InsuredPhoneId = glbRisk.MainInsured.CompanyName.Phone.Id;
                }
                if (glbRisk.MainInsured.CompanyName.Email != null) {
                    riskData.InsuredEmailId = glbRisk.MainInsured.CompanyName.Email.Id;
                }
            }

            if (glbRisk.MainInsured.IdentificationDocument != null && glbRisk.MainInsured.IdentificationDocument.DocumentType != null && glbRisk.MainInsured.IdentificationDocument.DocumentType.Id != null)
                riskData.InsuredDocumentTypeId = glbRisk.MainInsured.IdentificationDocument.DocumentType.Id;
            else
                if (glbRisk.MainInsured.IndividualType > 0)
                    riskData.InsuredDocumentTypeId = glbRisk.MainInsured.IndividualType;
                else
                    riskData.InsuredDocumentTypeId = 1;
        }

        if (glbRisk.Contractor != null) {

            riskData.ContractorId = glbRisk.Contractor.IndividualId;
            riskData.ContractorIdentificationDocument = glbRisk.Contractor.IdentificationDocument.Number;
            riskData.ContractorName = glbRisk.Contractor.Name;
            riskData.ContractorInsuredId = glbRisk.Contractor.InsuredId;
            riskData.ContractorCustomerType = glbRisk.Contractor.CustomerType;
            riskData.ContractorIndividualType = glbRisk.Contractor.IndividualType;
            if (glbRisk.Contractor.CompanyName != null) {
                riskData.ContractorDetailId = glbRisk.Contractor.CompanyName.NameNum;
                riskData.ContractorAddressId = glbRisk.Contractor.CompanyName.Address.Id;
                riskData.ContractorAddressDescription = glbRisk.Contractor.CompanyName.Address.Description;
                riskData.ContractorPhoneDescription = glbRisk.Contractor.CompanyName.Phone.Description;
            } else {
                riskData.ContractorDetailId = 0;
            }
            riskData.CustomerType = glbRisk.Contractor.CustomerType;
        }
        RiskSuretyRequest.IsConsortiumindividualIdTempRisk(glbRisk.Contractor.IndividualId).done(function (data) {
            if (data.result) {
                glbRisk.IsConsortium = true;
            }
            OperationQuotaCumulusRequest.GetOperatingQuotaEventByIndividualIdByLineBusinessIdTempRisk(glbRisk.Contractor.IndividualId, glbPolicy.Prefix.Id)
                .done((OperationQuota) => {
                    if (OperationQuota.success) {
                        if (OperationQuota.result) {
                            if (OperationQuota.result.IdentificationId != 0) {
                                OperationQuotaEvent = {
                                    Id: OperationQuota.result.OperatingQuotaEventID,
                                    Belongingto: '1.' + glbPolicy.Holder.Name,
                                    OperationCuotaInitial: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT / glbPolicy.ExchangeRate.SellAmount,
                                    OperationCuotaAvalible: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT / glbPolicy.ExchangeRate.SellAmount - OperationQuota.result.ApplyEndorsement.AmountCoverage / glbPolicy.ExchangeRate.SellAmount,
                                    DateTo: FormatDate(OperationQuota.result.IndividualOperatingQuota.EndDateOpQuota),
                                    DateRegisty: FormatDate(OperationQuota.result.IssueDate),
                                    Cumulu: OperationQuota.result.ApplyEndorsement.AmountCoverage / glbPolicy.ExchangeRate.SellAmount
                                }
                                glbSuretyOperationQuotaEvent = OperationQuotaEvent;
                                glbRisk.IsConsortium = false;
                            }
                        }
                    }
                });
        });
        lockScreen();
        OperationQuotaCumulusRequest.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessIdTempRisk(glbRisk.Contractor.IndividualId, glbPolicy.Prefix.Id)
            .done((Consortium) => {
                if (Consortium.success) {
                    if (Consortium.result) {
                        if (Consortium.result.consortiumEventDTO != null) {

                            ConsortiumEvent = {
                                Id: Consortium.result.OperatingQuotaEventID,
                                Belongingto: '2.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName,
                                OperationQuotaConsortium: Consortium.result.consortiumEventDTO.OperationQuotaConsortium / glbPolicy.ExchangeRate.SellAmount,
                                OperationCuotaInitial: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT / glbPolicy.ExchangeRate.SellAmount,
                                OperationCuotaAvalible: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT / glbPolicy.ExchangeRate.SellAmount - Consortium.result.ApplyEndorsement.AmountCoverage / glbPolicy.ExchangeRate.SellAmount,
                                DateTo: FormatDate(Consortium.result.IndividualOperatingQuota.EndDateOpQuota),
                                DateRegisty: FormatDate(Consortium.result.IssueDate),
                                Cumulu: Consortium.result.ApplyEndorsement.AmountCoverage / glbPolicy.ExchangeRate.SellAmount
                            }
                            glbSuteryConsortiumEvent = ConsortiumEvent;
                        }
                    }
                }
            });

        OperationQuotaCumulusRequest.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessIdTempRisk(glbRisk.Contractor.IndividualId, glbPolicy.Prefix.Id)
            .done(function (EconomicGroup) {
                if (EconomicGroup.success) {
                    if (EconomicGroup.result.EconomicGroupEventDTO != null) {
                        EconomicGroupEvent = {
                            Id: EconomicGroup.result.OperatingQuotaEventID,
                            Belongingto: '3.' + EconomicGroup.result.EconomicGroupEventDTO.economicgroupoperatingquotaDTO.EconomicGroupName,
                            OperationCuotaInitial: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT / glbPolicy.ExchangeRate.SellAmount,
                            OperationCuotaAvalible: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT / glbPolicy.ExchangeRate.SellAmount - EconomicGroup.result.ApplyEndorsement.AmountCoverage / glbPolicy.ExchangeRate.SellAmount,
                            DateTo: FormatDate(EconomicGroup.result.IndividualOperatingQuota.EndDateOpQuota),
                            DateRegisty: FormatDate(EconomicGroup.result.IssueDate),
                            Cumulu: EconomicGroup.result.ApplyEndorsement.AmountCoverage / glbPolicy.ExchangeRate.SellAmount
                        }
                        glbSuteryEconomicGroupEvent = EconomicGroupEvent
                    }
                }
            });

        let QuotaCumulusData = RiskSurety.SetInformationAfianzadoTempRisk();
        riskData.OperatingQuota = QuotaCumulusData.OperatingQuota;
        riskData.Aggregate = QuotaCumulusData.Aggregate
        riskData.Available = QuotaCumulusData.Available;
        riskData.AvailableOperatingQuota = (QuotaCumulusData.AvailableOperatingQuota != null) ? QuotaCumulusData.AvailableOperatingQuota : 0;
        riskData.ContractValue = NotFormatMoney(glbRisk.Value.Value);
        riskData.ContractNumber = glbRisk.SettledNumber;
        riskData.AmountInsured = NotFormatMoney(glbRisk.Risk.AmountInsured);
        riskData.Premium = NotFormatMoney(glbRisk.Risk.Premium);
        riskData.IsfacultativeId = glbRisk.Isfacultative;
        riskData.IsRetention = glbRisk.IsRetention;
        riskData.CountryId = glbRisk.Country.Id;
        riskData.StateId = glbRisk.State.Id;
        riskData.CityId = glbRisk.City.Id;
        riskData.IsNational = glbRisk.IsNational;

        if (glbRisk.RiskSuretyPost.ChkContractDate) {
            riskData.TerminalUnitContract = FormatDate(glbRisk.RiskSuretyPost.ContractDate);
            riskData.ChkContractDate = glbRisk.RiskSuretyPost.ChkContractDate;

        }
        if (glbRisk.RiskSuretyPost.ChkContractFinalyDate) {
            riskData.FinalDeliveryDate = FormatDate(glbRisk.RiskSuretyPost.IssueDate);
            riskData.ChkContractFinalyDate = glbRisk.RiskSuretyPost.ChkContractFinalyDate;
        }

        if (glbRisk.Risk.Text.TextBody != null) {
            riskData.ContractObject = glbRisk.Risk.Text.TextBody;
        }

        var coveragesValues = RiskSurety.UnformatCoverage(glbRisk.Coverages);
        riskData.Coverages = coveragesValues;
        (gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) ? riskData.RiskId = 0 : riskData.RiskId = glbRisk.Id;
        return riskData;
    }

    static LoadScript() {
        if (glbRisk.Id == 0) {
            RiskSurety.SaveRisk(MenuType.Script, 0);
        }

        if (glbRisk.Id > 0 && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, dynamicProperties);
        }
    }

    static ShowSaveRisk(riskId, redirect, coverageId) {
        var events = null;

        var functionRedirect = () => {
            if (glbRisk.InfringementPolicies != null) {
                events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
            }
            if (events == null || events !== TypeAuthorizationPolicies.Restrictive) {
                RiskSurety.Redirect(redirect, riskId, coverageId);
            }
        };

        if (redirect == MenuType.Coverage) {
            functionRedirect();
        }
        else {
            if ($("#selectRiskSurety").UifSelect("getSelected") == null || $("#selectRiskSurety").UifSelect("getSelected") == 0) {
                RiskSurety.GetRisksSuretyByTemporalId(glbPolicy.Id, riskId, functionRedirect);
            } else {
                functionRedirect();
            }
        }
        //lanza los eventos para la creación de el riesgo

        //fin - lanza los eventos para la creación de el riesgo
    }

    static UpdateGlbRisk(data) {
        glbRisk = data;
        $.extend(glbRisk, data.Risk);
        glbRisk.Object = "RiskSurety";
        formRisk: "#mainRiskSurety";
        glbRisk.Class = RiskSurety;
    }

    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbRisk.RecordScript = isModify;
        if (isModify) {
            RiskSurety.GetPremium();
        }
    }

    static GetPremium() {
        $("#mainRiskSurety").validate();

        if ($("#mainRiskSurety").valid()) {
            var riskData = RiskSurety.GetRiskDataModel();

            RiskSuretyRequest.GetPremium(riskData, riskData.Coverages, dynamicProperties, glbRisk.ContractObject).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        RiskSurety.LoadRiskSurety(data.result);
                    }
                }
                else {
                    $("#inputPremium").text(0);
                    $("#inputAmountInsured").text(0);
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculatePremium, 'autoclose': true });
            });
        }
    }

    static RunRules(ruleSetId) {
        var defRules = $.Deferred();
        RiskSuretyRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (glbPolicy.Endorsement.EndorsementType == 2) {
                        if (glbPolicyEndorsement.Endorsement.ModificationTypeId == 4 && glbRisk.Coverages != undefined && glbRisk.Coverages.length > 0) {
                            data.result.Risk.Coverages = glbRisk.Coverages;
                            data.result.Risk.Premium = 0;
                            glbRisk.Coverages.forEach(function (item) {
                                data.result.Risk.Premium = data.result.Risk.Premium + parseInt(item.PremiumAmount);
                            });
                        }
                    }
                    RiskSurety.LoadRiskSurety(data.result);
                    defRules.resolve();
                }
            }
            else {
                //dfd.reject(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfd.reject(AppResources.ErrorRunRulesPolicyPre);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
        return defRules.promise();
    }

    static LoadViewModel(viewModel) {
        if (glbPersonOnline.IndividualSearchType != 3) {
            if (viewModel.ContractorId != null) {
                individualSearchType = 3;
                RiskSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(viewModel.ContractorId, InsuredSearchType.IndividualId, viewModel.CustomerType);
                RiskSurety.LoadAggregate(viewModel.ContractorId);
            }
        }

        if (viewModel.ContractValue != null) {
            $("#inputContractValue").val(FormatMoney(viewModel.ContractValue));
        }
        $("#inputContractNumber").val(viewModel.ContractNumber);
        if (viewModel.ContractType != null) {
            $("#selectContractType").UifSelect("setSelected", viewModel.ContractType);
        }
        if (viewModel.Class != null) {

            $("#selectClassofContract").UifSelect("setSelected", viewModel.Class.Id);
        }

        if (viewModel.GroupCoverage != null) {
            if (viewModel.GroupCoverage.Id > 0) {
                $("#selectRiskSuretyGroupCoverage").UifSelect("setSelected", viewModel.GroupCoverage.Id);

                if (viewModel.Coverages != null) {
                    RiskSurety.LoadCoverages(viewModel.Coverages);
                }
            }
        }
        if (viewModel.AmountInsured != null) {
            $("#inputTotalSumInsuredRisk").text(FormatMoney(viewModel.AmountInsured));
        }
        $("#inputPremiumRisk").text(FormatMoney(viewModel.Premium));
        if (viewModel.Isfacultative) {
            $('#chkFacultativeSurety').attr('checked', true);
        }
        else {
            $('#chkFacultativeSurety').attr('checked', false);
        }
        if (viewModel.IsRetention) {
            $('#chkRetentionSurety').attr('checked', true);
        }
        else {
            $('#chkRetentionSurety').attr('checked', false);
        }

        if ($('#NacionalLevel').is(":checked") == true) {
            $("#selectSuretyCountry").UifSelect("setSelected", viewModel.CountryId);
        } else {
            $("#selectSuretyCountry").UifSelect("setSelected", viewModel.CountryId);
            $("#selectSuretyState").UifSelect("setSelected", viewModel.StateId);
            $("#selectSuretyCity").UifSelect("setSelected", viewModel.CityId);
        }

        if (viewModel.IsNational) {
            $('#NacionalLevel').attr('checked', true);
        }
        else {
            $('#NacionalLevel').attr('checked', false);
        }

        if (glbPolicy.TemporalType != TemporalType.Quotation && viewModel.RiskId != 0) {
            RiskSurety.validateOperatingQuota();
        }

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            $('#chkFacultativeSurety').prop('disabled', true);
            $('#chkRetentionSurety').prop('disabled', true);
            //  RiskSurety.DisableControls(true);
        }
        RiskSurety.LoadSubTitles(0);
    }


    //*Metodos get*//
    static GetListSuretyContractCategories(selectedId) {
        var dfd = $.Deferred();
        RiskSuretyRequest.GetSuretyContractCategories().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectClassofContract").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectClassofContract").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
                dfd.resolve();
            }
            else {
                dfd.reject(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfd.reject(AppResources.ErrorSearchCoveragesAccesories);
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
        return dfd.promise();
    }

    static GetListSuretyContractTypes(selectedId) {
        var dfdTypeContract = $.Deferred();
        RiskSuretyRequest.GetSuretyContractTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectContractType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectContractType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
                dfdTypeContract.resolve();
            }
            else {
                dfdTypeContract.reject(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfdTypeContract.reject(AppResources.ErrorSearchCoveragesAccesories);
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
        return dfdTypeContract.promise();
    }

    //Metodo de consulta parametro de validacion para definir obligatoriedad de contragarantias
    static async GetEnableCrossGuarantee() {
        if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) { return false; }

        var data = await RiskSuretyRequest.GetValCrossGuarantee();
        if (data.success) {
            return data.result;
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCrossGaranteesMandatory, 'autoclose': true });
        }
    }
    //*Metodos get*//

    //// ***************Formato coberturas *****************////////
    static UnformatCoverage(coverages) {
        $.each(coverages, function (key, value) {
            this.PercentageContract = NotFormatMoney(value.PercentageContract);
            if (this.LimitAmount != null) {
                this.LimitAmount = NotFormatMoney(this.LimitAmount);
            }
            if (this.SubLimitAmount != null) {
                this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);

            }
            if (this.OriginalSubLimitAmount != null) {
                this.OriginalSubLimitAmount = NotFormatMoney(this.OriginalSubLimitAmount);
            }
            if (this.PremiumAmount != null) {
                this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            }
            if (this.Rate != null) {
                this.Rate = NotFormatMoney(this.Rate);
            }
            if (this.DeclaredAmount != null) {
                this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            }
            if (this.EndorsementLimitAmount != null) {
                this.EndorsementLimitAmount = NotFormatMoney(value.EndorsementLimitAmount);
            }
            if (this.EndorsementSublimitAmount != null) {
                this.EndorsementSublimitAmount = NotFormatMoney(value.EndorsementSublimitAmount);
            }
            if (this.ContractAmountPercentage != null) {
                this.ContractAmountPercentage = NotFormatMoney(this.ContractAmountPercentage);
            }

            //Vigencias
            if (this.CurrentFrom == undefined) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInvalidCoverageDates + ': ' + value.Description, 'autoclose': true });
                return false;
            }
            else if (value.CurrentFrom.indexOf("Date") > -1) {
                value.CurrentFrom = FormatDate(value.CurrentFrom);
                value.CurrentTo = FormatDate(value.CurrentTo);
            }
            if (Isnull(FormatDate(value.CurrentFromOriginal))) {
                value.CurrentFromOriginal = value.CurrentFrom;
            }
            else {
                value.CurrentFromOriginal = FormatDate(value.CurrentFromOriginal);
            }
            if (Isnull(FormatDate(value.CurrentToOriginal))) {
                value.CurrentToOriginal = value.CurrentTo;
            }
            else {
                value.CurrentToOriginal = FormatDate(value.CurrentToOriginal);
            }

        });
        return coverages;
    }
    static formatCoverage(coverages) {
        $.each(coverages, function (key, value) {
            this.PercentageContract = NotFormatMoney(value.PercentageContract);
            if (this.LimitAmount != null) {
                this.LimitAmount = FormatMoney(this.LimitAmount);
            }
            if (this.SubLimitAmount != null) {
                this.SubLimitAmount = FormatMoney(this.SubLimitAmount)

            }
            if (this.OriginalSubLimitAmount != null) {
                this.OriginalSubLimitAmount = FormatMoney(this.OriginalSubLimitAmount)
            }
            if (this.PremiumAmount != null) {
                this.PremiumAmount = FormatMoney(this.PremiumAmount)
            }
            if (this.Rate != null) {
                this.Rate = FormatMoney(this.Rate)
            }
            if (this.DeclaredAmount != null) {
                this.DeclaredAmount = FormatMoney(this.DeclaredAmount)
            }
            if (this.EndorsementLimitAmount != null) {
                this.EndorsementLimitAmount = FormatMoney(value.EndorsementLimitAmount)
            }
            if (this.EndorsementSublimitAmount != null) {
                this.EndorsementSublimitAmount = FormatMoney(value.EndorsementSublimitAmount)
            }
            if (this.ContractAmountPercentage != null) {
                this.ContractAmountPercentage = FormatMoney(this.ContractAmountPercentage);
            }
            //Vigencias
            if (this.CurrentFrom == undefined) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInvalidCoverageDates + ': ' + value.Description, 'autoclose': true });
                return false;
            }
            else if (value.CurrentFrom.indexOf("Date") > -1) {
                value.CurrentFrom = FormatDate(value.CurrentFrom);
                value.CurrentTo = FormatDate(value.CurrentTo);
            }
            if (Isnull(FormatDate(value.CurrentFromOriginal))) {
                value.CurrentFromOriginal = value.CurrentFrom;
            }
            else {
                value.CurrentFromOriginal = FormatDate(value.CurrentFromOriginal);
            }
            if (Isnull(FormatDate(value.CurrentToOriginal))) {
                value.CurrentToOriginal = value.CurrentTo;
            }
            else {
                value.CurrentToOriginal = FormatDate(value.CurrentToOriginal);
            }
        });
        return coverages;
    }

    static LoadRiskSuretyCombos(riskData, glbPolicy) {
        var dfd = $.Deferred();
        var idClass = 0;
        if (riskData.Class != null) {
            idClass = riskData.Class.Id;
        }

        RiskSuretyRequest.LoadRiskSuretyCombos(glbPolicy).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    var enabled = glbPolicy.TemporalTypeDescription == "Temporal" && !$('#NacionalLevel').is(":checked");
                    //Cargar Tipo de Contrato
                    if (riskData.ContractType != null && riskData.ContractType.Id > 0) {
                        $("#selectContractType").UifSelect({ sourceData: data.result.ContractTypes, selectedId: riskData.ContractType.Id });

                    }
                    else {
                        $("#selectContractType").UifSelect({ sourceData: data.result.ContractTypes });
                    }
                    //Cargar Clase de Contrato
                    if (riskData.Class != null) {

                        if (idClass == 0) {
                            $("#selectClassofContract").UifSelect({ sourceData: data.result.ContractCategories });
                        }
                        else {
                            $("#selectClassofContract").UifSelect({ sourceData: data.result.ContractCategories, selectedId: idClass });
                        }
                    }

                    //Cargar Grupo de Cobertura
                    if (riskData.Risk != null && riskData.Risk.GroupCoverage != null && glbPolicy.Product != null) {
                        if (riskData.Risk.GroupCoverage.Id == null || riskData.Risk.GroupCoverage.Id == 0) {
                            $("#selectRiskSuretyGroupCoverage").UifSelect({ sourceData: data.result.GroupCoverages });
                        }
                        else {
                            $("#selectRiskSuretyGroupCoverage").UifSelect({ sourceData: data.result.GroupCoverages, selectedId: riskData.Risk.GroupCoverage.Id });
                        }
                        if (riskData.Risk.Coverages != null) {
                            RiskSurety.LoadCoverages(riskData.Risk.Coverages);
                        }
                    }

                    var promesas = [];

                    //cargar país
                    if (riskData.Country != null && riskData.Country.Id > 0) {
                        promesas.push(new Promise((resolve, reject) => {
                            RiskSuretyRequest.GetCountries().done(function (data) {
                                if (data.success) {
                                    gdlCountries = data.result;

                                    $("#NacionalLevel").attr('disabled', !(glbPolicy.TemporalTypeDescription == "Temporal"));
                                    $("#selectSuretyCountry").UifSelect({ sourceData: data.result, selectedId: riskData.Country.Id, enable: glbPolicy.TemporalTypeDescription == "Temporal" });
                                }
                                resolve();
                            }).fail(xhr => {
                                reject(xhr);
                            })
                        }));
                    }

                    //Cargar departamento
                    if (riskData.State != null && riskData.State.Id > 0) {
                        promesas.push(new Promise((resolve, reject) => {
                            RiskSuretyRequest.GetStates(riskData.Country.Id, 0).done(function (data) {
                                if (data.success) {
                                    $("#selectSuretyState").UifSelect({ sourceData: data.result, selectedId: riskData.State.Id, enable: enabled });
                                }
                                resolve();
                            }).fail(xhr => {
                                reject(xhr);
                            });
                        }));
                    }

                    //Cargar Ciudad
                    if (riskData.City != null && riskData.City.Id > 0) {
                        promesas.push(new Promise((resolve, reject) => {
                            RiskSuretyRequest.GetCities(riskData.Country.Id, riskData.State.Id).done(function (data) {
                                if (data.success) {
                                    $("#selectSuretyCity").UifSelect({ sourceData: data.result, selectedId: riskData.City.Id, enable: enabled });
                                }
                                resolve();
                            }).fail(xhr => {
                                reject(xhr);
                            });
                        }));
                    }

                    Promise.all(promesas).then(() => {
                        dfd.resolve(data.result);
                    });
                }
            }
        });
        return dfd.promise();
    }

    static ValidateSarlaftSecure(individualId) {

        if (glbPolicy.TemporalType == TemporalType.Policy) {
            var result = false;
            UnderwritingRequest.ValidateGetSarlaft(individualId).done(function (data) {
                if (data.success) {

                    switch (data.result) {
                        case SarlaftValidationState.NOT_EXISTS:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExistsSecure, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.EXPIRED:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExpiredSecure, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.OVERCOME:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftOvercomeSecure, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.ACCURATE:
                            result = true;
                            break;
                        case SarlaftValidationState.PENDING:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftPendingSecure, 'autoclose': true });
                            result = false;
                            break;
                        default:
                            result = false;
                            break;
                    }

                    return result;
                }
                else {
                    return false;
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                return false;
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
            });
            return result;
        }
        else {
            return true;
        }
    }

    static SaveCoverages() {

        var coveragesValues = $("#listCoverages").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.OriginalSubLimitAmount = NotFormatMoney(this.OriginalSubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.ContractAmountPercentage = NotFormatMoney(this.ContractAmountPercentage);
            this.Rate = NotFormatMoney(this.Rate);
            this.CalculatePorcentage = false;
        });
        RiskSuretyCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, coveragesValues).done(function (data) {
            if (data.success) {
                previousValue = glbRisk.Value.Value;
                router.run("prtRiskSurety");
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
    }

    static SearchRiskTemporal() {
        if ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) && gblCurrentRiskTemporalNumberOld == null) {
            RiskSuretyRequest.GetCiaRiskByTemporalId(gblCurrentRiskTemporalNumber).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        RiskSurety.GetRisksSuretyByTemporalId(gblCurrentRiskTemporalNumber, glbRisk.Id);
                    }
                    else {
                        RiskSurety.LoadInitialize();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.TemporaryNotExist, 'autoclose': true });
                }
            });
        }
        else {
            RiskSuretyRequest.GetCiaRiskByTemporalId(glbPolicy.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        RiskSurety.GetRisksSuretyByTemporalId(glbPolicy.Id, glbRisk.Id);
                    }
                    else {
                        RiskSurety.LoadInitialize();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.TemporaryNotExist, 'autoclose': true });
                }
            });
        }
    }

    ChangeReasonSocial(event, selectedItem) {

        if (selectedItem.Id > 0) {
            var band = 0;
            var individualIdN = $("#inputInsured").data("Object").IndividualId;
            UnderwritingRequest.GetDetailsByIndividualId(individualIdN, 1).done(function (elemento) {
                if (elemento.success) {
                    if (elemento.result.length > 0) {
                        elemento.result.forEach(function (item) {
                            if (item.NameNum == selectedItem.Id && band == 0) {
                                band = 1;
                                if (item.Address != null && item.Address.Description != null)
                                    $("#inputAdress").val(item.Address.Description);
                                if (item.Address.Id != null) { glbRisk.AddressId = item.Address.Id; $("#inputInsured").data("Object").CompanyName.Address.Id = item.Address.Id; }
                                if (item.Phone.Id != null) { glbRisk.PhoneID = item.Phone.Id }
                                if (item.Phone != null && item.Phone.Description) {
                                    $("#inputPhone").data("Object", item.Phone);
                                    $("#inputPhone").val(item.Phone.Description);
                                    $("#inputInsured").data("Object").CompanyName.Phone.Id = item.Phone.Id;
                                }
                            }
                        });
                    }

                }
            }).fail(function (jqXHR, textStatus, errorThrown) {

            });
        }
    }

    static SetInformationAfianzadoTempRisk() {
        let riskData = {}
        if (glbSuteryEconomicGroupEvent != undefined) {
            //Grupo Economico
            riskData.OperatingQuota = EconomicGroupEvent.OperationCuotaInitial;
            riskData.Aggregate = EconomicGroupEvent.Cumulu;
            riskData.Available = EconomicGroupEvent.OperationCuotaInitial - EconomicGroupEvent.Cumulu;
        } else if (glbSuteryConsortiumEvent != undefined && glbRisk.IsConsortium) {
            //Consorcio
            riskData.OperatingQuota = glbSuteryConsortiumEvent.OperationCuotaInitial;
            riskData.Aggregate = glbSuteryConsortiumEvent.Cumulu;
            riskData.Available = glbSuteryConsortiumEvent.OperationCuotaInitial - glbSuteryConsortiumEvent.Cumulu;
            riskData.AvailableOperatingQuota = glbSuteryConsortiumEvent.OperationQuotaConsortium;

        } else if (glbSuretyOperationQuotaEvent != undefined && glbSuteryConsortiumEvent != undefined) {
            riskData.OperatingQuota = glbSuretyOperationQuotaEvent.OperationCuotaInitial;
            riskData.Aggregate = glbSuretyOperationQuotaEvent.Cumulu
            riskData.Available = glbSuretyOperationQuotaEvent.OperationCuotaInitial - glbSuretyOperationQuotaEvent.Cumulu;
        } else if (glbSuretyOperationQuotaEvent != null && glbSuteryConsortiumEvent == undefined) {
            // Individual
            riskData.OperatingQuota = glbSuretyOperationQuotaEvent.OperationCuotaInitial;
            riskData.Aggregate = glbSuretyOperationQuotaEvent.Cumulu;
            riskData.Available = glbSuretyOperationQuotaEvent.OperationCuotaInitial - glbSuretyOperationQuotaEvent.Cumulu;
        } else {
            riskData.OperatingQuota = 0;
            riskData.Aggregate = 0;
            riskData.Available = 0;
        }
        return riskData
    }

    static QuotationRisk() {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            var coverages = $("#listCoverages").UifListView('getData');
            if (coverages != null && coverages.length > 0) {
                coverages = RiskSurety.UnformatCoverage(coverages);
                RiskSuretyCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, coverages).done(function (data) {
                    if (data.success) {
                        var ContractValue = NotFormatMoney($("#inputContractValue").val());
                    }

                });
            }
        }
    }

    static VerifyBusinessProcess() {

        if ((glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) && ($('#inputPremiumRisk').text() == "0")) {
            return false;

        }
        else {
            return true;
        }

    }
}