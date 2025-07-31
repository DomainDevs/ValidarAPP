
/**
 * Variables Locales y Globales
 */

var RiskJudicialCustodyModel = {};
var ArticleModel = {};
var ListDepartamentoModel = {};
var ListCityModel = {};
var OperationalCapacity = null;
var Accumulation = null;
//Disponible=Cupo Oerativo - Comulo - la suma del Riesgo
var Available = null;
var ArrayCapacityOf = [];
var glbTypesInsured = [];
var glbArticle = null;
var ArrayArticle = [];
var countryJudicialSurety = 1;//este valor debe cargar de base de datos queda pendiente
var isnCalculate = false;
var validateAdditionalData = false;
var validateText = false;
var validateBeneficiarie = false;
var validateCrossGuarantees = false;
var resultSave = null;
var glbSettledNumber = null;
var glbSuretyGroupCoverage = null;
/**
 * Clase Principal de ramo Judicial
 */
var riskController = 'RiskJudicialSurety';


class RiskJudicialSurety extends Uif2.Page {

    /**
     * Funcion que inicializa la clase con los controles
     */
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        riskController = "RiskJudicialSurety"
        $("#btnConvertProspect").hide();
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskJudicialSurety", Class: RiskJudicialSurety, formRisk: "#formJudicialSurety", RecordScript: false, AddressId: 0, PhoneID: 0, NameNum: 0 };
        }
        RiskJudicialSurety.setDefault();
        RiskJudicialSurety.LoadInitialize();
        individualSearchType = 1;
        if (gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) {
            if (gblCurrentRiskTemporalNumberOld == gblCurrentRiskTemporalNumber) {
                RiskJudicialSurety.GetRisksByTemporalId(gblCurrentRiskTemporalNumber, glbRisk.Id);
            }
            else {
                RiskJudicialSurety.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
            }

        }
        else {
            RiskJudicialSurety.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
        }

    }
    static setDefault() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listCoveragesJudSur").UifListView({
            source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true
            , delete: true, displayTemplate: "#coverageTemplate", height: 430, title: AppResources.LabelTitleCoverages
        });
    }
    static LoadInitialize() {
        if (glbPersonOnline != null) {
            RiskJudicialSurety.LoadData();
        }
        if (glbPolicy != null && glbRisk != null && glbRisk.Id > 0) {
            RiskJudicialSurety.setInitializeSurety();
            RiskJudicialSurety.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
            RiskJudicialSurety.GetRiskById(glbRisk.Id);
        }
        else {
            RiskJudicialSurety.setInitializeSurety();
            RiskJudicialSurety.LoadHolderInsured();
            RiskJudicialSurety.GetCountriesJudicialSurety(1, 0, 0);
        }
        if (glbPolicy.IssueDate.indexOf("Date") > -1) {
            glbPolicy.IssueDate = FormatDate(glbPolicy.IssueDate);
        }
        RiskJudicialSurety.SetModificaction();
        RiskJudicialSurety.LoadTitle();
    }

    static LoadHolderInsured() {
        if (glbPolicy != null && glbPolicy.Holder.IndividualId != "0" && glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {

            RiskJudicialSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        }
    }
    static SetModificaction() {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            var riskId = $("#selectRisk option:eq(1)").val();

            if (riskId != null && riskId != "") {
                $("#selectRisk").val(riskId);
                RiskJudicialSurety.GetRiskById($("#selectRisk option[Value!='']")[0].value);

            }
        }
    }
    static setInitializeSurety() {
        RiskJudicialSurety.GetInsuredType(0);
        RiskJudicialSurety.GetArticle();
        RiskJudicialSurety.GetCourt();
        RiskJudicialSurety.GetRiskJudicialSuretyGroupCoverages(glbPolicy.Product.Id, 0);
        RiskJudicialSurety.GetRiskActivities(glbPolicy.Product.Id, 0);
        RiskJudicialSurety.GetGroupCoverages(glbPolicy.Product.Id, 0);
        RiskJudicialSurety.GetDeductiblesByProductId(glbPolicy.Product.Id, 0);
    }
    Accept() {
        RiskJudicialSurety.SaveRisk(MenuType.Risk, 0);
        ScrollTop();
    }
    /**
     * Eventos de los controles de la clase
     */
    bindEvents() {
        $("#btnScript").click(this.OpenScript);
        //$('#listCoveragesJudSur').on('rowAdd', function (event) {
        //    RiskJudicialSurety.SaveRisk(MenuType.Coverage, 0);
        //});
        $("#btnDetailInsured").click(this.OpenShowDetailInsured);
        $("#inputInsuredJudSur").on("buttonClick", RiskJudicialSurety.SearchInsured);
        $("#btnDetailArticle").click(this.OpenShowDetailArticle);
        $("#btnIndividualDetailArticleAccept").click(this.HideDetailArticle);
        $("#tableIndividualResults tbody").on('click', 'tr', RiskJudicialSurety.SelectItemIndividualResults);
        $("#btnIndividualDetailAccept").click(RiskJudicialSurety.SetIndividualDetail);
        $("#btnConvertProspect").click(RiskJudicialSurety.ConvertProspect);
        $('#tblResultListCities tbody').on('click', 'tr', this.SelectSearchCities);
        $('#selectInsuredActsAs').on('itemSelected', (event, selectedItem) => { RiskJudicialSurety.restrict_multiple($("#selectInsuredActsAs").val(), $("#selectHolderActAs")); });
        $('#selectHolderActAs').on('itemSelected', (event, selectedItem) => { RiskJudicialSurety.restrict_multiple($("#selectHolderActAs").val(), $("#selectInsuredActsAs")); });
        $("#selectState").on('itemSelected', (event, selectedItem) => { RiskJudicialSurety.GetCities(0); });
        $('#selectGroupCoverageJudSur').on('itemSelected', (event, selectedItem) => { RiskJudicialSurety.ChangeGroupCoverage(); });
        $('#selectRisk').on('itemSelected', (event, selectedItem) => { RiskJudicialSurety.ItemSelectedselectedItem($("#selectRisk").val()); });
        $('#btnAcceptRiskJudicialSurety').on('click', this.Accept);
        $('#listCoveragesJudSur').on('rowEdit', function (event, data, index) {
            if (data.IsVisible == false) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
                return false;
            }
            else {
                if (data.PremiumAmount == "0" && (data.CoverStatus == RiskStatus.Original || data.CoverStatus == RiskStatus.Included)) {
                    isnCalculate = true;
                }
                RiskJudicialSurety.SaveRisk(MenuType.Coverage, data.Id);
            }
        });
        $('#listCoveragesJudSur').on('rowAdd', this.AddCoverageJudicialSurety);
        $('#listCoveragesJudSur').on('rowDelete', (event, data) => { this.DeleteCoverageJudicialSurety(data); });
        $('#btnCalculate').on('click', RiskJudicialSurety.Calculate);
        $('#btnClose').on('click', RiskJudicialSurety.CloseRisk);
        $('#btnDeleteRisk').on('click', RiskJudicialSurety.DeleteRisk);
        $('#btnAddRisk').on('click', RiskJudicialSurety.NewRisk);
        $("#InsuredValue").OnlyDecimals(UnderwritingDecimal);
        $("#InsuredValue").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $("#InsuredValue").focusout(function () {
            if ($.trim($(this).val()) != "") {
                $(this).val(FormatMoney($(this).val()));
            }
        });
        $("#btnAcceptNewPersonOnline").click(this.NewPersonOnline);
        $('#selectCountry').on('itemSelected', this.selectCountry);
        $('#inputName').on('itemSelected', this.ChangeReasonSocial);
    }

    static GetGroupCoverages(productId, selectedId) {
        RiskJudicialRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == 0) {
                        $("#selectGroupCoverage").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskActivities(productId, selectedId) {
        RiskJudicialRequest.GetRiskActivities(productId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == 0) {
                        $("#selectRiskActivity").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectRiskActivity").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
        });
    }

    static GetDeductiblesByProductId(productId, selectedId) {
        var controller = rootPath + "Underwriting/RiskJudicialSurety/GetDeductiblesByProductId?productId=" + productId;
        if (selectedId == 0) {
            $("#selectDeductible").UifSelect({ source: controller });
        }
        else {
            $("#selectDeductible").UifSelect({ source: controller, selectedId: selectedId });
        }
    }

    static LoadData() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskJudicialSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
                    if (glbPersonOnline.CustomerType == CustomerType.Individual) {
                        Underwriting.ConvertProspectToInsured(glbPersonOnline.IndividualId, glbPersonOnline.DocumentNumber);
                    }
                }
                else {
                    $("#inputInsuredJudSur").data("Object", null);
                    $("#inputInsuredJudSur").data("Detail", null);
                    if (glbPersonOnline.DocumentNumber.length > 0) {
                        $("#inputInsuredJudSur").val(glbPersonOnline.DocumentNumber);
                    }
                    else {
                        $("#inputInsuredJudSur").val(glbPersonOnline.Name);
                    }
                }
                if (glbRisk.Id > 0) {

                    RiskJudicialSurety.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskJudicialSurety.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }

        }
    }

    static Calculate() {
        isnCalculate = true;
        RiskJudicialSurety.GetPremium();
    }

    static CloseRisk() {
        glbRisk = { Id: 0, Object: RiskJudicialSurety };
        RiskJudicialSurety.UpdatePolicyComponents();
    }

    static DeleteRisk() {
        if ($("#selectRisk").UifSelect("getSelected") > 0) {
            RiskJudicialRequest.DeleteRisk(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                        RiskJudicialSurety.ClearControls();
                        RiskJudicialSurety.GetRisksByTemporalId(glbPolicy.Id);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                        ScrollTop();
                    }
                    else {
                        var riskId = $("#selectRisk").UifSelect("getSelected");
                        RiskJudicialSurety.GetRisksByTemporalId(glbPolicy.Id, riskId);
                        RiskJudicialSurety.GetRiskById(riskId);
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

    static NewRisk() {
        RiskJudicialSurety.ClearControls();
        RiskJudicialSurety.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    static RunRules(ruleSetId) {

        RiskJudicialRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (data.success) {
                RiskJudicialSurety.LoadRisk(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    static ClearControls() {
        $('#inputAmountInsured').val('');
        $("#inputPremium").val('');
        $("#selectInsuredActsAs").UifSelect("setSelected", null);
        $("#selectHolderActAs").UifSelect("setSelected", null);
        $("#selectArticleJudSur").UifSelect("setSelected", null);
        $("#selectTypeOfCourt").UifSelect("setSelected", null);
        $("#selectRiskActivity").UifSelect("setSelected", null);
        $("#selectState").UifSelect("setSelected", null);
        $("#selectCity").UifSelect("setSelected", null);
        $("#selectGroupCoverageJudSur").UifSelect("setSelected", null);
        $("#selectRisk").UifSelect("setSelected", null);
        $("#ProcessAndOrFiled").val('');
        $("#InsuredValue").val('');
        dynamicProperties = null;
        isnCalculate = false;
        glbRisk.RecordScript = false;
        RiskJudicialSurety.ChangeGroupCoverage();
        RiskJudicialSurety.UpdateGlbRisk({ Id: 0 });
        $('#selectedConceptsRisk').text("");
    }

    static UpdatePolicyComponents() {
        UnderwritingRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                RiskJudicialSurety.ReturnUnderwriting();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
    }

    static ReturnUnderwriting() {
        var url = '';

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "JudicialSuretyModification";
            router.run("prtModification");
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

    static GetPremium() {
        if ($('#inputInsuredJudSur').data("Object") != null) {
            $("#formJudicialSurety").validate();
            if ($("#formJudicialSurety").valid()) {
                var recordScript = false;

                if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0) {
                    recordScript = true;
                }
                else {
                    recordScript = glbRisk.RecordScript;
                }

                if (recordScript) {
                    var riskData = RiskJudicialSurety.GetRiskDataModel();
                    var additionalData = RiskJudicialSurety.GetAdditionalDataModel();


                    RiskJudicialRequest.GetPremium(glbPolicy.Id, riskData, dynamicProperties).done(function (data) {
                        if (data.success) {
                            RiskJudicialSurety.LoadRisk(data.result);
                        }
                        else {
                            $('#inputPremium').text(0);
                            isnCalculate = false;
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculatePremium, 'autoclose': true });
                    });
                }
                else {
                    RiskJudicialSurety.LoadScript();
                }
            }
        } else {
            UnderwritingPersonOnline.ShowOnlinePerson();
        }
    }
    static LoadScript() {
        if ($('#formJudicialSurety').valid() && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExcecuteScript(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object);
        }
    }

    static LoadRisk(riskData) {
        if (riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation || glbPolicy.TemporalType != TemporalType.TempQuotation) {
                $("#btnConvertProspect").show();
            }

        }
        if (riskData.InsuredActAs != null) {
            RiskJudicialSurety.GetInsuredAct(riskData.InsuredActAs);
        }
        if (riskData.HolderActAs != null) {
            RiskJudicialSurety.GetHolderAct(riskData.HolderActAs);
        }
        //$("#selectInsuredActsAs").UifSelect("setSelected", riskData.InsuredActAs);
        //$("#selectHolderActAs").UifSelect("setSelected", riskData.HolderActAs);
        if (riskData.Article != null) {
            if ($("#selectArticleJudSur").data("config") != null) {
                $("#selectArticleJudSur").UifSelect("setSelected", riskData.Article.Id);
            }
        }
        if (riskData.Court != null && $("#selectTypeOfCourt").data("config") != null) {
            $("#selectTypeOfCourt").UifSelect("setSelected", riskData.Court.Id);
        }
        if (riskData.Risk.RiskActivity != null) {
            $("#selectRiskActivity").UifSelect("setSelected", riskData.Risk.RiskActivity.Id);
        }

        $("#ProcessAndOrFiled").val(riskData.SettledNumber);
        $("#InsuredValue").val(riskData.InsuredValue);



        if (riskData.Risk.Premium == 0 && (riskData.Risk.Status == RiskStatus.Original || riskData.Risk.Status == RiskStatus.Included)) {
            isnCalculate = true;
        }
        /*else {
            isnCalculate = true;
        }*/
        if (riskData.City != null) {
            RiskJudicialSurety.GetCountriesJudicialSurety(riskData.City.State.Country.Id, riskData.City.State.Id, riskData.City.Id);
        }
        if (riskData.Risk.Coverages != null) {
            RiskJudicialSurety.LoadCoverages(riskData.Risk.Coverages, riskData.Risk.GroupCoverage.Id);
        }
        else if (riskData.Risk.GroupCoverage != null) {
            if (riskData.Risk.GroupCoverage.Id != 0) {
                RiskJudicialSurety.GetCoveragesByGroupCoverageId(riskData.Risk.GroupCoverage.Id);
            }
            else {
                if (glbSuretyGroupCoverage != null) {
                    RiskJudicialSurety.GetCoveragesByGroupCoverageId(glbSuretyGroupCoverage);
                }
            }
        }

        if (riskData.Risk.Premium == 0 && (riskData.Risk.Status == RiskStatus.Original || riskData.Risk.Status == RiskStatus.Included)) {
            isnCalculate = false;
        }
        else {
            isnCalculate = true;
        }
        if (riskData.Risk.MainInsured != null && riskData.Risk.MainInsured.IndividualId > 0) {
            RiskJudicialSurety.LoadInsured(riskData.Risk.MainInsured);
            RiskJudicialSurety.GetIndividualDetails(RiskJudicialSurety.GetIndividualDetailsByIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CustomerType));
            RiskJudicialSurety.getReasonSocialIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CompanyName.NameNum);
        }
        if (riskData.Risk.GroupCoverage != null) {
            var selectGroupCoverage = riskData.Risk.GroupCoverage.Id;
            if (glbSuretyGroupCoverage != null) {
                if (glbSuretyGroupCoverage.Id != 0) {
                    $("#selectGroupCoverageJudSur").UifSelect("setSelected", glbSuretyGroupCoverage.Id);
                };
            }
            else {
                $("#selectGroupCoverageJudSur").UifSelect("setSelected", selectGroupCoverage);
            }
        }
        $('#inputAmountInsured').text(FormatMoney(riskData.Risk.AmountInsured));
        $("#inputPremium").text(FormatMoney(riskData.Risk.Premium));
        $('#chkIsDeclarative').prop('checked', riskData.Risk.IsFacultative);

        $("#inputName").UifSelect("setSelected", riskData.Risk.MainInsured.CompanyName.NameNum);
        dynamicProperties = riskData.Risk.DynamicProperties;
        RiskJudicialSurety.UpdateGlbRisk(riskData);
        RiskJudicialSurety.LoadSubTitles(0);
    }

    static LoadTempRisk(riskData) {
        glbSettledNumber = riskData.SettledNumber;
        if (riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation || glbPolicy.TemporalType != TemporalType.TempQuotation) {
                $("#btnConvertProspect").show();
            }
        }
        dynamicProperties = riskData.Risk.DynamicProperties;
        RiskJudicialSurety.UpdateGlbRisk(riskData);
        RiskJudicialSurety.LoadSubTitles(0);
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        var resultData = {};
        RiskJudicialRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
                $('#inputBeneficiaryName').data('Detail', data.result);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });

        return resultData;
    }

    static GetAdditionalDataModel() {
        var additionalData = $("#formAdditionalData").serializeObject();
        additionalData.NewPrice = NotFormatMoney($("#inputAdditionalDataPrice").val());

        if ($("#inputAdditionalDataInsured").data("Object") != null) {
            additionalData.InsuredId = $("#inputAdditionalDataInsured").data("Object").IndividualId;
            ObjectAdditionalDataRiskJudicialSurety.GetAdditionalData();
        }

        return additionalData;
    }

    static GetRiskDataModel() {



        var riskData = $("#formJudicialSurety").serializeObject();
        riskData.IdInsuredActsAs = $("#selectInsuredActsAs").UifSelect("getSelected");
        riskData.ArticleName = $("#selectArticleJudSur").UifSelect("getSelected");
        riskData.InsuredActsAs = $("#selectInsuredActsAs").val();
        riskData.IdHolderActAs = $("#selectHolderActAs").val();
        riskData.IdArticle = $("#selectArticleJudSur").val();
        riskData.IdTypeOfCourt = $("#selectTypeOfCourt").val();
        riskData.CountryId = $("#selectCountry").UifSelect("getSelected");
        riskData.IdDepartment = $("#selectCity").UifSelect("getSelected");
        riskData.IdMunicipality = $("#selectState").UifSelect("getSelected");
        riskData.InsuredValue = $("#InsuredValue").val();
        riskData.IsRetention = $('#chkIsDeclarative').is(':checked');
        if (glbRisk.Id > 0) {
            riskData.RiskId = glbRisk.Id;
        }
        riskData.RiskActivityId = $("#selectRiskActivity").UifSelect("getSelected");

        if ($('#inputInsuredJudSur').data("Object") != null) {
            riskData.InsuredName = ReplaceCharacter($('#inputInsuredJudSur').data("Object").Name);
            riskData.InsuredId = $('#inputInsuredJudSur').data("Object").IndividualId;
            riskData.InsuredCustomerType = $("#inputInsuredJudSur").data("Object").CustomerType;
            riskData.InsuredBirthDate = FormatDate($("#inputInsuredJudSur").data("Object").BirthDate);
            riskData.InsuredGender = $("#inputInsuredJudSur").data("Object").Gender;
            riskData.InsuredIndividualTypeId = $("#inputInsuredJudSur").data("Object").IndividualType;
            if ($("#inputInsuredJudSur").data("Object").IdentificationDocument != null) {
                riskData.InsuredDocumentNumber = $("#inputInsuredJudSur").data("Object").IdentificationDocument.Number;
            }
            if ($("#inputInsuredJudSur").data("Object").CompanyName != null) {
                if ($("#inputName").UifSelect("getSelected") != 0 && $("#inputName").UifSelect("getSelected") != null) { riskData.InsuredDetailId = $("#inputName").UifSelect("getSelected"); }
                else { riskData.InsuredDetailId = $("#inputInsuredJudSur").data("Object").CompanyName.NameNum; }
                if ($("#inputInsuredJudSur").data("Object").CompanyName.Phone != null) {
                    riskData.InsuredPhoneId = glbRisk.PhoneID;
                    riskData.InsuredAddressId = glbRisk.AddressId;

                }
                if ($("#inputInsuredJudSur").data("Object").CompanyName.Email != null) {
                    riskData.InsuredEmailId = $("#inputInsuredJudSur").data("Object").CompanyName.Email.Id;
                }
            }

            if ($("#inputInsuredJudSur").data("Object").IdentificationDocument != null && $("#inputInsuredJudSur").data("Object").IdentificationDocument.DocumentType != null && $("#inputInsuredJudSur").data("Object").IdentificationDocument.DocumentType.Id != null)
                riskData.InsuredDocumentTypeId = $("#inputInsuredJudSur").data("Object").IdentificationDocument.DocumentType.Id;
            else
                if ($("#inputInsuredJudSur").data("Object").IndividualType != null && $("#inputInsuredJudSur").data("Object").IndividualType > 0)
                    riskData.InsuredDocumentTypeId = $("#inputInsuredJudSur").data("Object").IndividualType;
                else
                    riskData.InsuredDocumentTypeId = 1;

            if ($("#inputInsuredJudSur").data("Object").AssociationType != null) {
                riskData.InsuredAssociationType = $("#inputInsuredJudSur").data("Object").AssociationType.Id;
            }
        }
        riskData.Premium = NotFormatMoney($('#inputPremium').text());
        riskData.AmountInsured = NotFormatMoney($('#inputAmountInsured').text());

        var coverages = $('#listCoveragesJudSur').UifListView('getData');

        $.each(coverages, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
        });

        riskData.AgentName = RiskJudicialCustodyModel.InsuredName;
        riskData.Coverages = coverages;
        (gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumber == gblCurrentRiskTemporalNumberOld) ? riskData.RiskId = 0 : riskData.RiskId = riskData.RiskId;

        return riskData;
    }

    static GetTempRiskDataModel() {
        var riskData = $("#formJudicialSurety").serializeObject();
        riskData.ProcessAndOrFiled = glbSettledNumber;
        riskData.IdInsuredActsAs = glbRisk.InsuredActAs;
        if (glbRisk.Article != null) {
            riskData.ArticleName = glbRisk.Article.Description;
        }
        riskData.InsuredActsAs = glbRisk.InsuredActAs;
        riskData.IdHolderActAs = glbRisk.HolderActAs;
        riskData.IdArticle = glbRisk.Article.Id;
        riskData.IdTypeOfCourt = glbRisk.Court.Id;
        riskData.CountryId = glbRisk.City.State.Country.Id;
        riskData.IdDepartment = glbRisk.City.Id;
        riskData.IdMunicipality = glbRisk.City.State.Id;
        riskData.InsuredValue = glbRisk.InsuredValue;
        riskData.RiskActivityId = glbRisk.Risk.RiskActivity.Id

        if (glbRisk.MainInsured != null) {
            riskData.InsuredName = ReplaceCharacter(glbRisk.MainInsured.Name);
            riskData.InsuredId = glbRisk.MainInsured.IndividualId;
            riskData.InsuredCustomerType = glbRisk.MainInsured.CustomerType;
            riskData.InsuredBirthDate = FormatDate(glbRisk.MainInsured.BirthDate);
            riskData.InsuredGender = glbRisk.MainInsured.Gender;
            if (glbRisk.MainInsured.IdentificationDocument != null) {
                riskData.InsuredDocumentNumber = glbRisk.MainInsured.IdentificationDocument.Number;
            }
            if (glbRisk.MainInsured.CompanyName != null) {
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
        riskData.Premium = NotFormatMoney(glbRisk.Risk.Premium);
        riskData.AmountInsured = NotFormatMoney(glbRisk.Risk.AmountInsured);

        var coverages = glbRisk.Risk.Coverages;

        $.each(coverages, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);

            if (Isnull(FormatDate(this.CurrentFromOriginal))) {
                this.CurrentFromOriginal = this.CurrentFrom;
            }
            else {
                this.CurrentFromOriginal = FormatDate(this.CurrentFromOriginal);
            }
            if (Isnull(FormatDate(value.CurrentToOriginal))) {
                this.CurrentToOriginal = this.CurrentTo;
            }
            else {
                this.CurrentToOriginal = FormatDate(this.CurrentToOriginal);
            }
        });

        riskData.AgentName = glbRisk.MainInsured.Name;
        glbSuretyGroupCoverage = glbRisk.Risk.GroupCoverage;
        riskData.Coverages = coverages;
        (gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumber == gblCurrentRiskTemporalNumberOld) ? riskData.RiskId = 0 : riskData.RiskId = riskData.RiskId;

        return riskData;
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
            case MenuType.CrossGuarantees:
                $("#modalCrossGuarantees").UifModal('hide');
                break;
            default:
                break;
        }
    }

    EditCoverageJudicialSurety(data) {
        if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
            return false;
        }
        else {
            RiskJudicialSurety.SaveRisk(MenuType.Coverage, data.Id);
        }
    }

    AddCoverageJudicialSurety() {
        RiskJudicialSurety.SaveRisk(MenuType.Coverage, 0);
    }

    DeleteCoverageJudicialSurety(data) {
        RiskJudicialSurety.DeleteCoverage(data);
    }

    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        }
        else {
            var coverages = $("#listCoveragesJudSur").UifListView('getData');
            if (coverages != null && coverages != "" && coverages.length == 1 && data.EndorsementType == EndorsementType.Modification) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
            }
            else {
                $("#listCoveragesJudSur").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });

                $.each(coverages, function (index, value) {
                    if (this.Id != data.Id) {
                        $("#listCoveragesJudSur").UifListView("addItem", this);
                    }
                    else {
                        if (data.EndorsementType == EndorsementType.Modification && data.RiskCoverageId > 0) {
                            var coverage = RiskJudicialSurety.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                            if (coverage != null) {
                                coverage.Rate = NotFormatMoney(coverage.Rate);
                                coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                                $("#listCoveragesJudSur").UifListView("addItem", coverage);
                            }
                        }
                    }
                });
            }
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;

        RiskJudicialRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
            if (data.success) {
                coverage = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorExcludeCoverage, 'autoclose': true });
        });

        return coverage;
    }
    /**
     * Operaciones de apertura y cerrado de las Modales con las parciales
     */


    OpenScripts() {
        if ($("#formJudicialSurety").valid()) {
            if (glbRisk == null) {
                glbRisk = { Id: 0, Object: RiskJudicialSurety, formRisk: "#formJudicialSurety", RecordScript: false };
            }
            //ExcecuteScript(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object);//se comenta mientras se averigua como se carga el valor de scripId
            ExcecuteScript(104, glbRisk.Object);
        }
    }

    /**
     * FUncion para abrir la vista parcial de beneficiarios
     */
    OpenAdditionalData() {
        if ($("#formJudicialSurety").valid()) {
            $('#modalAdditionalData').UifModal('showLocal', AppResources.LabelAdditionalData);
        }
    }

    /**
     * Funcion para abrir la vista parcial de Beneficiarios
     */
    OpenBeneficiaries() {
        if ($("#formJudicialSurety").valid()) {
            $('#modalBeneficiaries').UifModal('showLocal', AppResources.LabelBeneficiaries);
        }
    }

    /**
     * Funcion para abrir la vista parcial de textos
     */
    OpenTexts() {
        if ($("#formJudicialSurety").valid()) {
            $('#modalTexts').UifModal('showLocal', AppResources.LabelText);
        }
    }

    /**
     * Funcion para abrir la vista parcial de contragarantias
     */
    OpenCounterGuarantees() {
        if ($("#formJudicialSurety").valid()) {
            $('#modalCrossGuarantees').UifModal('showLocal', AppResources.LabelCounterGuarantees);
        }
    }

    /**
     * Funcion para abrir la vista parcial de Clausulas
     */
    OpenClauses() {
        if ($("#formJudicialSurety").valid()) {
            $('#modalClauses').UifModal('showLocal', AppResources.LabelClauses);
        }
    }

    /**
     * Funcion para ver la informacion de detalle del tomador
     */
    OpenShowDetailInsured() {


        $('#tableIndividualDetails').UifDataTable('clear');

        if ($("#inputInsuredJudSur").data('Detail') != null) {
            var secureDetails = $("#inputInsuredJudSur").data('Detail');

            if (secureDetails.length > 0) {
                $.each(secureDetails, function (id, item) {

                    $('#tableIndividualDetails').UifDataTable('addRow', item)
                    if ($("#inputInsuredJudSur").data("Object").CompanyName.NameNum > 0 && $("#inputInsuredJudSur").data("Object").CompanyName.NameNum == this.NameNum) {
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

    /**
     * Funcion para visualizar el detalle del articulo seleccionado
     */
    OpenShowDetailArticle() {
        ArticleModel.IdArticle = $("#selectArticleJudSur").UifSelect("getSelected");
        if (ArticleModel.IdArticle != "") {
            $.each(glbArticle, function (id, item) {
                if (ArticleModel.IdArticle == item.Id) {
                    ArticleModel.ArticleName = item.Description;
                    $('#tableIndividualDetailArticle').UifDataTable('clear');
                    if ($('#tableIndividualDetailArticle').UifDataTable('getSelected') == null) {
                        $('#tableIndividualDetailArticle').UifDataTable('addRow', ArticleModel)
                    }
                    $('#modalIndividualDetailArticle').UifModal('showLocal', AppResources.LabelDetailArticle);
                }
            });
        }
    }

    /**
     * FUncion para ocultar la vista de detalle de articulo
     */
    HideDetailArticle() {
        $('#modalIndividualDetailArticle').UifModal("hide");
    }

    /**
     * FUncion para cargar la vista de Coberturas
     */

    static ReturnCoverage(coverageId) {
        glbCoverage = {
            CoverageId: coverageId,
            Class: CoverageJudicialSurety
        }
        router.run("prtCoverageRiskJudicialSurety");
    }


    OpenScript() {
        RiskJudicialSurety.LoadScript();
    }



    /*
     * Funcion para obtener la informacion del asegurado por descripcion
     * @param {any} description
     * @param {any} insuredSearchType
     * @param {any} customerType
     */
    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            if (customerType == null && glbPolicy.TemporalType == TemporalType.Policy) {
                customerType = CustomerType.Individual;
            }
            if (isNaN(description)) {
                if (description.trim().split(" ")[0] != null && description.trim().split(" ")[0] != undefined) {
                    description = description.trim().split(" ")[0];
                }
            }
            RiskJudicialRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (individualSearchType == 1 || individualSearchType == 2) {
                        if (data.result.length == 0 && individualSearchType != 2) {
                            UnderwritingPersonOnline.ShowOnlinePerson();
                            RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                        }
                        else if (data.result.length == 1) {
                            RiskJudicialSurety.LoadInsured(data.result[0]);
                        }
                        else if (data.result.length > 1) {
                            modalListType = 1;
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentNum: data.result[i].IdentificationDocument.Number,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            RiskJudicialSurety.ShowModalListRiskSurety(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                        }
                    }
                    else if (individualSearchType == 3) {
                        if (data.result.length == 0) {
                            UnderwritingPersonOnline.ShowOnlinePerson();
                        }
                        else if (data.result.length == 1) {
                            RiskJudicialSurety.LoadSecure(data.result[0]);
                        }
                        else if (data.result.length > 1) {
                            modalListType = 1;
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentNum: data.result[i].IdentificationDocument.Number,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            RiskJudicialSurety.ShowModalListRiskSurety(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                        }
                    }

                    if (glbPolicy.TemporalType != TemporalType.Quotation) {
                        RiskJudicialSurety.SaveHolderGuarantee();
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
                    RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsured, 'autoclose': true });
            });
        }
    }

    static SaveHolderGuarantee() {
        var policyDataGuarantee = {};
        if ($('#inputSecure').data("Object") != null) {
            policyDataGuarantee.HolderId = $("#inputSecure").data("Object").IndividualId;
            policyDataGuarantee.HolderIdentificationDocument = $("#inputSecure").data("Object").IdentificationDocument.Number;
            if ($("#inputSecure").data("Object").Name != null)
                policyDataGuarantee.HolderName = $("#inputSecure").data("Object").Name;
            else
                policyDataGuarantee.HolderName = $("#inputSecure").data("Object").TradeName;
            if ($("#inputSecure").data("Object").IdentificationDocument.DocumentType != null)
                documentType = $("#inputSecure").data("Object").IdentificationDocument.DocumentType.Id;
            if ($("#inputSecure").data("Object").Address != null)
                policyDataGuarantee.HolderAddress = $("#inputSecure").data("Object").Address.Description;
            if ($("#inputSecure").data("Object").Phone != null)
                policyDataGuarantee.HolderPhone = $("#inputSecure").data("Object").Phone.Description;
        }
    }

    /**
     * Funcion para visualizar la informacion del asegurado elegido
     * @param {any} dataTable
     */
    static ShowModalListRiskSurety(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static LoadSecure(secure) {
        if (individualSearchType == 3) {
            $("#inputInsuredJudSur").data("Object", secure);
            $("#inputInsuredJudSur").val(secure.Name + ' (' + secure.IdentificationDocument.Number + ')');
            if (secure.CustomerType == CustomerType.Individual) {
                $("#inputInsuredJudSur").data("Detail", RiskJudicialSurety.GetIndividualDetails(secure.CompanyName));
                RiskJudicialSurety.LoadAggregate(secure.IndividualId);
                $("#btnSecureConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnSecureConvertProspect").show();
            }
        }
    }

    static LoadAggregate(InsuredId) {
        OperationalCapacity = 0;
        Accumulation = 0;
        Available = 0;
        //Cargar Linea Negocioo
        var prefixCd = 0;
        prefixCd = glbPolicy.Prefix.Id;
        var issueDate = glbPolicy.IssueDate;
        if (issueDate.indexOf("Date") > -1) {
            issueDate = FormatDate(issueDate);
        }
    }

    static validateOperatingQuota() {
        if ((OperationalCapacity == 0) || (OperationalCapacity == "")) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateOperatingQuota, 'autoclose': true });
            Accumulation = 0;
            //Disponible=Cupo Oerativo - Comulo - la suma del Riesgo
            Available = 0;
            return false
        }
        else if ((Available <= 0) || (Available == "")) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
            return false
        }
        return true;
    }

    static GetIndividualDetails(individualDetails) {
        if (individualDetails.length > 0) {

            $.each(individualDetails, function (key, value) {
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
                    if ($("#inputInsuredJudSur").data("Object").CompanyName == null) {
                        if (this.IsMain) {
                            $("#inputInsuredJudSur").data("Object").CompanyName = this;
                        }
                    }
                    else if ($("#inputInsuredJudSur").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                        $("#inputInsuredJudSur").data("Object").CompanyName = this;
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
                    if ($("#inputInsuredJudSur").data("Object").CompanyName == null) {
                        if (this.IsMain) {
                            $("#inputInsuredJudSur").data("Object").CompanyName = this;
                        }
                    }
                    else if ($("#inputInsuredJudSur").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                        $("#inputInsuredJudSur").data("Object").CompanyName = this;
                    }
                }
            });
        }

        return individualDetails;
    }

    static LoadInsured(insured) {
        if (individualSearchType == 1) {
            $("#inputInsuredJudSur").data("Object", insured);

            if ($("#inputInsuredJudSur").data("Object").IndividualType == 0) {
                var description = $("#inputInsuredJudSur").data("Object").IdentificationDocument.Number;
                var insuredSearchType = 1;
                var customerType = null;

                if (isNaN(description)) {
                    if (description.trim().split(" ")[0] != null && description.trim().split(" ")[0] != undefined) {
                        description = description.trim().split(" ")[0];
                    }
                }
                RiskJudicialRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                    if (data.success) {
                        $("#inputInsuredJudSur").data("Object").IndividualType = data.result[0].IndividualType;
                    }

                });

            }
            $("#inputInsuredJudSur").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                $("#inputInsuredJudSur").data("Detail", RiskJudicialSurety.GetIndividualDetails(insured.CompanyName));
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
                $("#inputBeneficiaryName").data("Detail", RiskJudicialSurety.GetIndividualDetails(insured.CompanyName));
                $("#btnConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }

        }
        else if (individualSearchType == 3) {
            $("#inputInsuredJudSur").data("Object", insured);
            $("#inputInsuredJudSur").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            $("#inputAdditionalDataInsured").data("Object", insured);
            $("#inputAdditionalDataInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType != CustomerType.Individual && glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        RiskJudicialSurety.getReasonSocialIndividualId(insured.IndividualId, 1);
    }

    static UpdateGlbRisk(data) {
        var recordScript = glbRisk.RecordScript;
        glbRisk = data
        $.extend(glbRisk, data.Risk);
        //glbRisk.Risk = null;
        glbRisk.Object = "RiskJudicialSurety";
        glbRisk.formRisk = "#formJudicialSurety";
        glbRisk.Class = RiskJudicialSurety;
        glbRisk.RecordScript = recordScript;
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

        if (subTitle == 0 || subTitle == 5) {
            if (glbRisk.Guarantees != null) {
                if (glbRisk.Guarantees.length > 0)
                    $("#selecteCounterGuarantees").text("(" + glbRisk.Guarantees.length + ")");
            } else {
                $("#selecteCounterGuarantees").text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 6) {
            if (glbRisk.Concepts != null) {
                if (glbRisk.Concepts.length > 0) {
                    $('#selectedConceptsRisk').text('(' + glbRisk.Concepts.length + ')');
                }
                else {
                    $('#selectedConceptsRisk').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedConceptsRisk').text('(' + AppResources.LabelWithoutData + ')');
            }
        }
    }

    static GetRiskGroupCoveragesJudSur(productId, selectedId) {
        RiskJudicialRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == 0) {
                        $("#selectGroupCoverage").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * FUncion para realizar la busqueda de un registro para el asegurado
     */
    static SearchInsured() {
        $('#inputInsuredJudSur').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        if ($.trim($("#inputInsuredJudSur").val()).length > 0) {
            // individualSearchType = 3;
            RiskJudicialSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsuredJudSur").val(), InsuredSearchType.DocumentNumber, null)
            $('#hiddenContractorId').val($('#inputInsuredJudSur').data("Object").IndividualId);
        }
    }

    /**
     * Funcion que invoca el controlador que carga las capacidades de tomador y asegurador
     */
    static GetInsuredType(selectedId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetInsuredType',
            data: JSON.stringify(),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
            if (data.success) {
                if (data.result != null) {

                    glbTypesInsured = data.result;
                    if (selectedId == 0) {

                        $('#selectInsuredActsAs').UifSelect({ sourceData: data.result });
                        $('#selectHolderActAs').UifSelect({ sourceData: data.result });
                    } else {
                        $('#selectInsuredActsAs').UifSelect({ sourceData: data.result, selectedId: selectedId });
                        $('#selectHolderActAs').UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static GetInsuredAct(selectedId) {
        RiskJudicialRequest.GetHolderAct().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    glbTypesInsured = data.result;
                    if (selectedId == 0) {
                        $('#selectInsuredActsAs').UifSelect({ sourceData: data.result });

                    } else {
                        $('#selectInsuredActsAs').UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

            }
        });
    }
    static GetHolderAct(selectedId) {
        RiskJudicialRequest.GetHolderAct().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    glbTypesInsured = data.result;
                    if (selectedId == 0) {
                        $('#selectHolderActAs').UifSelect({ sourceData: data.result });
                    } else {
                        $('#selectHolderActAs').UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * Funcion que invoca el controlador que carga los tipos de juzgado
     */
    static GetCourt() {
        RiskJudicialRequest.GetCourt().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#selectTypeOfCourt').UifSelect({ sourceData: data.result });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * Funcion para cargar los articulos para el ramo judicial
     */
    static GetArticle() {
        RiskJudicialRequest.GetArticle(glbPolicy.Product.Id).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    glbArticle = data.result;
                    $('#selectArticleJudSur').UifSelect({ sourceData: data.result });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * Funcion para visualizar el detalle de la informacion del asegurado
     */
    static SetIndividualDetail() {
        var details = $('#tableIndividualDetails').UifDataTable('getSelected');

        if (details == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectAddress, 'autoclose': true });
        }
        else {
            if (individualSearchType == 1) {
                //$("#inputInsured").data("Object").CompanyName = details[0];
            }
            else if (individualSearchType == 2) {
                //$("#inputBeneficiaryName").data("Object").CompanyName = details[0];
            }
            else if (individualSearchType == 3) {
                $("#inputInsuredJudSur").data("Object").CompanyName = details[0];
            }

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    /**
     * Funcion donde se captura un registro de la table de resultados consultada
     */
    static SelectItemIndividualResults() {
        if (individualSearchType == 2) {
            RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
        }
        else {
            RiskJudicialSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        }
        $('#modalIndividualSearch').UifModal("hide");
    }

    /**
     * 
     */
    static ConvertProspect() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskJudicialSurety.GetRiskDataModel()
        };
        UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsuredJudSur").data("Object").IndividualId, $("#inputInsuredJudSur").data("Object").IndividualType, $("#inputInsuredJudSur").data("Object").CustomerType, 0);
    }

    /**
     * Funcion para la carga del grupo de coberturas para el ramo judicial
     * @param {any} productId
     * @param {any} selectedId
     */
    static GetRiskJudicialSuretyGroupCoverages(productId, selectedId) {
        //grupo Coberturas  
        RiskJudicialRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == 0) {
                        $("#selectGroupCoverageJudSur").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectGroupCoverageJudSur").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }

                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * Funcion para el evento change del departamento
     */
    static GetCities(selectedId) {
        if ($('#selectState').UifSelect("getSelected") != null && $('#selectState').UifSelect("getSelected") != "") {
            RiskJudicialRequest.GetCitiesByCountryIdByStateId($('#selectCountry').UifSelect("getSelected"), $('#selectState').UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectCity").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $("#selectCity").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }

                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    /**
     * Funcion para que los combos de capacidades de tomador y asegurador sean excluyentes
     * @param {any} index
     * @param {any} selector
     */
    static restrict_multiple(index, selector) {
        var data = [];
        var valor = $(selector).val();
        if (index != 0 && index != "") {

            $.each(glbTypesInsured, function (i, val) {
                if (val.Value != index) {
                    data.push(val);
                }
            });

            $(selector).UifSelect({ sourceData: data });
            $(selector).UifSelect('setSelected', valor);
        }
        else {
            $('#selectInsuredActsAs').UifSelect({ sourceData: glbTypesInsured });
            $('#selectHolderActAs').UifSelect({ sourceData: glbTypesInsured });
        }
    }

    static GetCountriesJudicialSurety(countryId, stateId, cityId) {
        RiskJudicialRequest.GetCountries().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (countryId == 0) {
                        $("#selectCountry").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectCountry").UifSelect({ sourceData: data.result, selectedId: countryId });
                        RiskJudicialRequest.GetStatesByCountryId(countryId).done(function (data) {
                            if (data.success) {
                                if (data.result != null) {
                                    if (stateId == 0) {
                                        $("#selectState").UifSelect({ sourceData: data.result });
                                    }
                                    else {
                                        $("#selectState").UifSelect({ sourceData: data.result, selectedId: stateId })
                                        //$("#selectCountry").UifSelect("disabled", true);
                                        //$("#selectState").UifSelect("disabled", true);
                                        RiskJudicialRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (data) {
                                            if (data.success) {
                                                if (data.result != null) {
                                                    if (stateId == 0) {
                                                        $("#selectCity").UifSelect({ sourceData: data.result });
                                                    }
                                                    else {
                                                        $("#selectCity").UifSelect({ sourceData: data.result, selectedId: cityId });
                                                        //$("#selectCity").UifSelect("disabled", true);
                                                    }
                                                }
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                            }
                                        });
                                    }
                                }
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * FUncion para interactuar con el evento chage del combo de grupo de coberturas
     * @param {any} selectedItem
     */
    static ChangeGroupCoverage(selectedItem) {

        if ($("#selectGroupCoverageJudSur").val() > 0) {
            RiskJudicialSurety.GetCoveragesByGroupCoverageId($("#selectGroupCoverageJudSur").val());
        }
        else {
            $("#listCoveragesJudSur").UifListView("refresh");
        }
        $('#inputPremium').text(0);
        $("#inputAmountInsured").text(0);
    }

    static GetCoveragesByProductIdGroupCoverageId(productId, groupCoverageId) {
        RiskJudicialRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, groupCoverageId).done(function (data) {
            if (data.success) {
                RiskJudicialSurety.LoadCoverages(data.result, groupCoverageId);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
    }

    /**
     * Funcion para obtener el grupo de coberturas por id
     * @param {any} groupCoverageId
     */
    static GetCoveragesByGroupCoverageId(groupCoverageId) {
        RiskJudicialRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, groupCoverageId).done(function (data) {
            if (data.success) {
                RiskJudicialSurety.LoadCoverages(data.result, groupCoverageId);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
    }

    /**
     * FUncion para cargar las coberturas encontradas por pantalla en el listview
     * @param {any} coverages
     */
    static LoadCoverages(coverages, groupCoverageId) {
        var insuredAmount = 0;

        $("#listCoveragesJudSur").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });
        $.each(coverages, function (index, val) {
            insuredAmount += this.LimitAmount;
            this.LimitAmount = FormatMoney(this.LimitAmount);
            this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
            this.PremiumAmount = FormatMoney(this.PremiumAmount);
            this.DisplayRate = FormatMoney(this.Rate, 4);
            this.Rate = FormatMoney(this.Rate);
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.CurrentTo = FormatDate(this.CurrentTo);
            if (Isnull(FormatDate(this.CurrentFromOriginal))) {
                this.CurrentFromOriginal = this.CurrentFrom;
            }
            else {
                this.CurrentFromOriginal = FormatDate(this.CurrentFromOriginal);
            }
            if (Isnull(FormatDate(this.CurrentToOriginal))) {
                this.CurrentToOriginal = this.CurrentTo;
            }
            else {
                this.CurrentToOriginal = FormatDate(this.CurrentToOriginal);
            }
            if (this.Deductible != null) {
                this.DeductibleDescription = this.Deductible.Description,
                    this.DeductibleId = this.Deductible.Id
            }

            if (glbPolicy.Endorsement.EndorsementType == 2) {
                if (glbPolicyEndorsement.Endorsement.ModificationTypeId != 4 && this.CalculationType == 2 && this.RateType == 3) {
                    if (IsEdit == undefined || IsEdit == null || IsEdit == false) {
                        this.Rate = 0;
                        this.PremiumAmount = 0;
                        this.DisplayRate = 0;
                    }
                }
            }
            $("#listCoveragesJudSur").UifListView("addItem", this);
        });
        if ($("#selectGroupCoverage").UifSelect("getSelected") != null) {
            RiskVehicle.EnabledButtonAccessories(groupCoverageId);
        }
    }

    /**
     * FUncion para Guardar el Riesgo
     */
    static SaveRisk(redirec, coverageId) {
        var AmountAccesoriesNew = 0;
        var AmountAccesoriesOld = 0;
        if ($("#formJudicialSurety").valid()) {
            var riskData = RiskJudicialSurety.GetRiskDataModel();
            var additionalData = RiskJudicialSurety.GetAdditionalDataModel();
            RiskJudicialRequest.SaveRisk(glbPolicy.Id, riskData, dynamicProperties, additionalData).done(function (data) {
                if (data.success) {
                    RiskJudicialSurety.UpdateGlbRisk(data.result);
                    RiskJudicialSurety.LoadSubTitles(0);
                    RiskJudicialSurety.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                    if (gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) {
                        gblCurrentRiskTemporalNumberOld = gblCurrentRiskTemporalNumber;
                        gblCurrentRiskTemporalNumber = data.result.Risk.Id;
                    }
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

    static SaveTempRisk(redirec, coverageId) {
        var AmountAccesoriesNew = 0;
        var AmountAccesoriesOld = 0;
        var riskData = RiskJudicialSurety.GetTempRiskDataModel();
        var additionalData = RiskJudicialSurety.GetAdditionalDataModel();
        lockScreen().then(function () {
            RiskJudicialRequest.SaveRisk(glbPolicy.Id, riskData, dynamicProperties, additionalData).done(function (data) {
                if (data.success) {
                    RiskJudicialSurety.UpdateGlbRisk(data.result);
                    RiskJudicialSurety.LoadSubTitles(0);
                    RiskJudicialSurety.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                    if (gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) {
                        gblCurrentRiskTemporalNumber = data.result.Risk.Id;
                        glbPolicy.Summary.RiskCount = 1;
                        $('#selectedInclusionRisk').text("(1)")
                        $("#modalRiskFromPolicy").UifModal("hide");
                        unlockScreen();
                    }
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
        })
    }

    static ShowSaveRisk(riskId, redirec, coverageId) {
        var events = null;

        if (glbRisk.InfringementPolicies != null) {
            events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
        }
        if (events === null || events !== TypeAuthorizationPolicies.Restrictive) {

            RiskJudicialSurety.RedirectAction(redirec, riskId, coverageId);
        }
    }

    static RedirectAction(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskJudicialSurety.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.Coverage:
                RiskJudicialSurety.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskJudicialSurety.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.AdditionalData:
                RiskJudicialSurety.ShowPanelsRisk(MenuType.AdditionalData);
                break;
            case MenuType.Texts:
                RiskJudicialSurety.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskJudicialSurety.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Concepts:
                RiskJudicialSurety.ShowPanelsRisk(MenuType.Concepts);
                break;
            case MenuType.Script:
                RiskJudicialSurety.ShowPanelsRisk(MenuType.Script);
                break;
            case MenuType.CrossGuarantees:
                $("#modalCrossGuarantees").UifModal('showLocal', AppResources.LabelGuarantees);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                break;
        }
    }

    static GetRisksByTemporalId(temporalId, selectedId) {
        RiskJudicialRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {

            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    if (selectedId == 0) {
                        if (data.result.length == 1) {
                            $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: data.result[0].Id });
                            RiskJudicialSurety.GetRiskById(data.result[0].Id);
                        } else {

                            $("#selectRisk").UifSelect({ sourceData: data.result });
                        }
                    }
                    else {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
                else {
                    $("#selectRisk").UifSelect({ sourceData: null });
                }

                if (glbPersonOnline != null) {
                    RiskJudicialSurety.LoadData();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                    RiskJudicialSurety.GetRiskById($("#selectRisk option[Value!='']")[0].value);
                }
                else if (glbRisk.Id > 0) {
                    RiskJudicialSurety.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskJudicialSurety.setInitializeSurety();
                }

                if (!glbPolicy.Product.IsFlatRate) {
                    $('#inputRate').prop('disabled', true);
                }
                RiskJudicialSurety.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
        });
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    static ItemSelectedselectedItem(selectedItem) {
        if (selectedItem > 0) {
            RiskJudicialSurety.GetRiskById(selectedItem);

        }
        else {
            RiskJudicialSurety.ClearControls();
        }
    }

    static GetRiskById(id) {
        if (id != null && id != "" && glbPolicy.Endorsement != null && glbPolicy != null) {
            RiskJudicialRequest.GetRiskById(glbPolicy.Endorsement.EndorsementType, glbPolicy.Id, id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        glbRisk.RecordScript = true;
                        if (gblCurrentRiskTemporalNumber != null) {
                            if (gblCurrentRiskTemporalNumberOld != null && gblCurrentRiskTemporalNumber != gblCurrentRiskTemporalNumberOld) {
                                RiskJudicialSurety.LoadRisk(data.result);
                            }
                            else {
                                RiskJudicialSurety.LoadTempRisk(data.result);
                            }
                        }
                        else {
                            RiskJudicialSurety.LoadRisk(data.result);
                        }

                        if (guaranteeModel != null && guaranteeModel) {
                            guaranteeModel = null;
                            ObjectCrossGuaranteesRiskJudicialSurety.LoadCrossGuarantee();
                        }
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

    static ShowPanelsRisk(Menu) {
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
            case MenuType.AdditionalData:
                $('#modalAdditionalData').UifModal('showLocal', AppResources.LabelAdditionalData);
                break;
            case MenuType.Concepts:
                $('#modalConcepts').UifModal('showLocal', AppResources.Concepts + ': ' + $('#inputPlate').val());
                break;
            default:
            case MenuType.Script:
                RiskJudicialSurety.LoadScript();
                break;
        }
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    NewPersonOnline() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskJudicialSurety.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsuredJudSur").val().trim());
    }

    static LoadViewModel(viewModel) {
        if (viewModel != null) {

            $("#selectInsuredActsAs").UifSelect("setSelected", viewModel.InsuredActsAs);
            $("#selectHolderActAs").UifSelect("setSelected", viewModel.IdHolderActAs);
            $("#selectArticleJudSur").UifSelect("setSelected", viewModel.IdArticle);
            $("#selectTypeOfCourt").UifSelect("setSelected", viewModel.IdTypeOfCourt);
            $("#selectState").UifSelect("setSelected", viewModel.IdDepartment);
            RiskJudicialSurety.GetCities(viewModel.IdMunicipality);
            $("#ProcessAndOrFiled").val(viewModel.ProcessAndOrFiled);
            $("#InsuredValue").val(viewModel.InsuredValue);
            $('#chkIsDeclarative').prop('checked', viewModel.IsRetention);
            var selectGroupCoverage = viewModel.GroupCoverage;
            $("#selectGroupCoverageJudSur").UifSelect("setSelected", selectGroupCoverage);
            $('#inputAmountInsured').text(FormatMoney(viewModel.AmountInsured));
            $('#selectRiskActivity').UifSelect("setSelected", viewModel.RiskActivityId);
            $("#inputPremium").text(FormatMoney(viewModel.Premium));
            if (viewModel.Premium == 0 && (viewModel.Status == RiskStatus.Original || viewModel.Status == RiskStatus.Included)) {
                isnCalculate = false;
            }
            else {
                isnCalculate = true;
            }
            if (viewModel.Coverages != "") {

                $.each(viewModel.Coverages, function (key, value) {
                    this.LimitAmount = parseFloat((this.LimitAmount).replace(',', '.'));
                    this.SubLimitAmount = parseFloat((this.SubLimitAmount).replace(',', '.'));
                    this.PremiumAmount = parseFloat((this.PremiumAmount).replace(',', '.'));
                });
                RiskJudicialSurety.LoadCoverages(viewModel.Coverages, viewModel.GroupCoverage);
            }
            else if (viewModel.GroupCoverage != "") {
                RiskJudicialSurety.GetCoveragesByGroupCoverageId(viewModel.GroupCoverage);
            }
        }
        RiskJudicialSurety.LoadSubTitles(0);
    }

    static getDaneCode(countryId, stateId, cityId) {
        RiskJudicialRequest.getDaneCode(selectedItem.Id).done(function (data) {
            if (data.success) {
                $("#inputDaneCode").UifAutoComplete('setValue', "");
                $("#inputDaneCode").UifAutoComplete('setValue', data.result);
            } else {
                $('#inputDaneCode').val("");
            }
        });
    }
    static GetRatingZone(country, state) {
        if (country != null && state != null) {
            RiskJudicialRequest.GetRatingZonesByPrefixIdCountryIdState(prefixId).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $("#inputRateZone").data("Id", data.result.Id);
                        $("#inputRateZone").val(data.result.Description).attr("disabled", true);
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                resultSave = false;
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
            });
        }
    }
    selectCountry(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskJudicialRequest.GetStatesByCountryId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $('#selectState').UifSelect({ sourceData: data.result });
                        $('#selectCity').UifSelect();
                    }
                }

            });
        }
    }
    static getReasonSocialIndividualId(individualId, Namenum) {
        $("#inputName").val("");
        $("#inputAdress").val("");
        $("#inputPhone").val("");

        if (individualId != null && individualId > 0)
            UnderwritingRequest.GetCompanyBusinessByIndividualId(individualId).done(function (data) {

                if (data.success) {
                    if (data.result.length > 0) {
                        //$('#reasonSocialWrapper').css("display", "block");
                        var TradeName = data.result[0].TradeName;
                        var Direccion = data.result[0].AddressID;
                        var Telefono = data.result[0].PhoneID;
                        $("#inputName").UifSelect({ sourceData: data.result, selectedId: Namenum });
                        $("#inputAdress").val(Direccion);
                        $("#inputPhone").val(Telefono);

                        RiskJudicialRequest.GetIndividualDetailsByIndividualId(individualId, 1).done(function (elemento) {
                            if (elemento.success) {
                                if (elemento.result.length > 0) {
                                    if (elemento.result[0].Address != null && elemento.result[0].Address.Description != null)
                                        $("#inputAdress").val(elemento.result[0].Address.Description);
                                    if (elemento.result[0].Phone != null && elemento.result[0].Phone.Description)
                                        $("#inputPhone").val(elemento.result[0].Phone.Description);
                                }

                            }
                        }).fail(function (jqXHR, textStatus, errorThrown) {

                        });
                    }
                    else
                        $('#reasonSocialWrapper').css("display", "none");
                }
                else
                    $('#reasonSocialWrapper').css("display", "none");
            });
        else
            $('#reasonSocialWrapper').css("display", "none");
    }

    ChangeReasonSocial(event, selectedItem) {

        if (selectedItem.Id > 0) {
            var band = 0;
            var individualIdN = $("#inputInsuredJudSur").data("Object").IndividualId;
            UnderwritingRequest.GetDetailsByIndividualId(individualIdN, 1).done(function (elemento) {
                if (elemento.success) {
                    if (elemento.result.length > 0) {
                        elemento.result.forEach(function (item) {
                            if (item.NameNum == selectedItem.Id && band == 0) {
                                band = 1;
                                glbRisk.NameNum = item.NameNum;
                                if (item.Address != null && item.Address.Description != null)
                                    $("#inputAdress").val(item.Address.Description);
                                if (item.Address.Id != null) { glbRisk.AddressId = item.Address.Id }
                                if (item.Phone.Id != null) { glbRisk.PhoneID = item.Phone.Id }
                                if (item.Phone != null && item.Phone.Description)
                                    $("#inputPhone").val(item.Phone.Description);
                            }
                        });
                    }

                }
            }).fail(function (jqXHR, textStatus, errorThrown) {

            });
        }
    }
}