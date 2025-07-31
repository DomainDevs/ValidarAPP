var individualSearchType = 1;
var IndividualType;
var isnCalculate = false;
var riskController;
var dynamicProperties = null;
var policyIsFloating;
var State
var IsEditing = false;
var aircraftDto = null;
var TotalInsuredAmmount = 0;
var TotalPremium = 0;
var LeapYear = null;
var totaldays = null;
var insuredObject = { Id: 0, Amount: 0, RateType: 0, Rate: 0 }
var riskData = null;
var NewInsured = null;
var isCalculatePremiunCoverange = false;
class RiskAircraft extends Uif2.Page {
    getInitialState() {
        riskController = 'RiskAircraft';
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskAircraft", formRisk: "#formAircraft", RecordScript: false, Class: RiskAircraft };
        }
        RiskAircraft.LoadCombobox(10);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
        RiskAircraft.LoadHolderInsured();
        //RiskAircraft.LoadInitialize();
        //RiskAircraft.ShowPanelsRisk(MenuType.Risk);

    }
    static LoadHolderInsured() {
        if (glbPolicy != null && glbPolicy.Holder.IndividualId != "0" && glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {

            RiskAircraft.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        }
    }
    static LoadCombobox(prefixId) {
        RiskAircraftRequest.GetComboboxes(prefixId).done(function (data) {
            if (data.success) {
                $("#selectAircraftMake").UifSelect({ sourceData: data.result.Makes });
                $("#selectAircraftType").UifSelect({ sourceData: data.result.Types });
                $("#selectAirCraftUseIds").UifSelect({ sourceData: data.result.Uses });
                $("#selectAirCraftEnrollment").UifSelect({ sourceData: data.result.Registers });
                $("#selectAirCraftOperator").UifSelect({ sourceData: data.result.Operators });
            }
        });
        RiskAircraft.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
    }

    static getPersonOnline() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskAircraft.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
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
                    RiskAircraft.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskAircraft.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }

    bindEvents() {
        $('#selectGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $('#selectRisk').on('itemSelected', RiskAircraft.ChangeRisk);
        $('#listCoverages').on('rowAdd', this.AddCoverage);
        $('#listCoverages').on('rowEdit', this.EditCoverage);
        $('#listCoverages').on('rowDelete', this.CoverageDelete);
        $("#btnAccept").on('click', this.Accept);
        $("#btnClose").on('click', this.Close);
        $("#btnAddRisk").on('click', RiskAircraft.ClearControls);
        $("#btnBeneficiary").on('click', { Menu: MenuType.Beneficiaries }, RiskAircraft.ShowPanelsRisk);
        $("#btnClauses").on('click', { Menu: MenuType.Clauses }, RiskAircraft.ShowPanelsRisk);
        $("#btnTexts").on('click', { Menu: 5 }, RiskAircraft.ShowPanelsRisk);
        $("#btnDeleteRisk").on('click', RiskAircraft.DeleteRisk);

        $("#inputInsured").on('buttonClick', this.SearchInsured);
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividual);
        $('#selectAircraftMake').on('itemSelected', RiskAircraft.GetModels);
    }
    CoverageDelete(event, data) {
        RiskAircraft.DeleteCoverage(data);
    }

    static GetModels(risk) {
        riskData = risk;
        var makeId = $("#selectAircraftMake").UifSelect("getSelected");
        RiskAircraftRequest.GetModels(makeId).done(function (data) {
            if (data.success) {
                $("#selectModel").UifSelect({ sourceData: data.result });
                if (riskData != null) {
                    $('#selectModel').UifSelect('setSelected', riskData.ModelId);
                }
            }
        });
    }
    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteInsuredObject, 'autoclose': true });
        }
        else {
            var InsuredObjects = $("#listCoverages").UifListView('getData');
            $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });

            $.each(InsuredObjects, function (index, value) {
                if (this.Id != data.Id) {
                    $("#listCoverages").UifListView("addItem", this);
                }
            });
            RiskAircraft.UpdatePremiunAndAmountInsured();
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeletedSuccessfully, 'autoclose': true });
        }
    }
    EditCoverage(event, data, index) {

        if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
            return false;
        }
        else {
            IsEditing = true;
            isCalculatePremiunCoverange = true;
            RiskAircraft.SaveRisk(MenuType.Coverage, data.Id);
        }
    }

    RedirectClauses(event) {
        RiskAircraft.SaveRisk(MenuType.Clauses, 0);
    }

    SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            RiskAircraft.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);
        }
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
        if (isCalculatePremiunCoverange === true) {
            if (isNaN(Menu)) {
                Menu = Menu.data.Menu;
            }
            switch (Menu) {
                case MenuType.Risk:
                    RiskAircraft.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
                    break;
                case MenuType.Texts:
                    RiskAircraft.GetTextsByRiskId(glbRisk.Id);
                    $("#modalTexts").UifModal('showLocal', AppResources.LabelDataTexts);
                    new RiskText();
                    break;
                case MenuType.Clauses:
                    $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                    new RiskClause();
                    break;
                case MenuType.Beneficiaries:
                    $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                    new RiskBeneficiary();
                    break;
                case MenuType.Concepts:
                    $('#modalConcepts').UifModal('showLocal', AppResources.LabelTitleAdditionalData + ': ' + $('#inputPlate').val());
                    break;
                case MenuType.Script:
                    RiskAircraft.LoadScript();
                    break;
                default:
                    break;
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.PremiumCero, 'autoclose': true });
        }

    }

    static GetTextsByRiskId(riskId) {
        RiskAircraftRequest.GetTextsByRiskId(riskId).done(function (data) {
            if (data.success) {
                glbRisk.Text = data.result;
                if (glbRisk.Text != undefined) {
                    $('#inputText').val(glbRisk.Text.TextBody);
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    SelectIndividual(e) {
        RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
        $('#modalIndividualSearch').UifModal("hide");
    }

    static GetRisksByTemporalId(temporalId, selectedId) {
        RiskAircraftRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId > 0) {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                    else {
                        $('#selectRisk').UifSelect({ sourceData: data.result });
                    }
                }
                if (glbPersonOnline != null) {
                    RiskAircraft.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                    RiskAircraft.GetRiskById($("#selectRisk option[Value!='']")[0].value);

                }
                else if (glbRisk.Id > 0) {
                    RiskAircraft.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskAircraft.SetInitialAircraft();
                }
                RiskAircraft.GetPolicyTypes(glbPolicy.Prefix.Id, glbPolicy.PolicyType.Id);
                RiskAircraft.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskById(id) {
        if (id != null && id != "") {
            RiskAircraftRequest.GetRiskById(id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        glbRisk.RecordScript = true;
                        glbRisk.Id = id;
                        RiskAircraft.LoadRisk(data.result);
                        RiskAircraft.LoadCoverages(data.result.InsuredObjects, data.result.CoverageGroupId);
                        RiskAircraft.UpdatePremiunAndAmountInsured();

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

    static ChangeRisk(event, selectedItem) {
        if (selectedItem.Id > 0) {
            $("#listCoverages").UifListView("clear");
            RiskAircraft.GetRiskById(selectedItem.Id);
        }
        else {
            RiskAircraft.ClearControls();
        }
    }

    static SetInitialAircraft() {
        RiskAircraft.GetGroupCoverages(glbPolicy.Product.Id, 0);
        RiskAircraft.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            if (customerType == null && glbPolicy.TemporalType == TemporalType.Policy) {
                customerType = CustomerType.Individual;
            }
            UnderwritingRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0) {
                        UnderwritingPersonOnline.ShowOnlinePerson();
                    }
                    else if (data.result.length == 1) {
                        IndividualType = data.result[0].IndividualType;
                        RiskAircraft.LoadInsured(data.result[0]);
                    }
                    else if (data.result.length > 1) {
                        modalListType = 1;
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription
                            });
                        }

                        RiskAircraft.ShowModalList(dataList);
                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                    }
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

                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsured, 'autoclose': true });
            });
        }
    }

    static LoadInsured(insured) {

        if (individualSearchType == 1) {
            $("#inputInsured").data("Object", insured);
            if ($("#inputInsured").data("Object").IndividualType == 0) {
                var description = $("#inputInsured").data("Object").IdentificationDocument.Number;
                var insuredSearchType = 1;
                var customerType = null;
                UnderwritingRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                    if (data.success) {
                        $("#inputInsured").data("Object").IndividualType = data.result[0].IndividualType
                    }
                });
            }

            $("#inputInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                $("#inputInsured").data("Detail", RiskAircraft.GetIndividualDetails(insured.CompanyName));
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
                $("#inputBeneficiaryName").data("Detail", RiskAircraft.GetIndividualDetails(insured.CompanyName));
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
                    if ($("#inputInsured").data("Object").CompanyName == null) {
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
        var resultData = $.Deferred();
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //resultSave = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });

        return resultData.promise;
    }

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static RunRules(ruleSetId) {
        RiskAircraftRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (data.success) {
                RiskAircraft.LoadRisk(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
    }

    //llenar el riesgos
    static LoadRisk(riskData) {
        RiskAircraft.GetGroupCoverages(glbPolicy.Product.Id, riskData.CoverageGroupId);
        if (riskData.RiskId != null) {
            $("#selectGroupCoverage").UifSelect("setSelected", riskData.CoverageGroupId);
            $("#selectAircraftMake").UifSelect("setSelected", riskData.MakeId);
            $("#selectAircraftType").UifSelect("setSelected", riskData.TypeId);
            $("#selectAirCraftUseIds").UifSelect("setSelected", riskData.UseId);
            $("#selectAirCraftEnrollment").UifSelect("setSelected", riskData.RegisterId);
            $("#selectAirCraftOperator").UifSelect("setSelected", riskData.OperatorId);
            $("#inputCurrentManufacturing").val(riskData.CurrentManufacturing);
            $("#inputNumberRegister").val(riskData.NumberRegister);
            $("#hiddenRiskId").val(riskData.RiskId);






            if (riskData.MakeId != null) {
                RiskAircraft.GetModels(riskData);
                $("#selectModel").UifSelect("setSelected", riskData.ModelId);
            }
            RiskAircraft.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(riskData.MainInsured, InsuredSearchType.DocumentNumber, null);
            //RiskAircraftRequest.GetModels(riskData.MakeId).done(function (data) {
            //      if (data.success) {
            //          $("#selectModel").UifSelect("setSelected", riskData.ModelId);
            //      }
            //      else {
            //          $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            //      }
            //  }).fail(function (jqXHR, textStatus, errorThrown) {
            //      $.UifNotify('show', { 'type': 'info', 'message': 'Modelo no encontrado', 'autoclose': true });
            //  });              

        }
    }

    AddCoverage(event) {
        IsEditing = false;
        RiskAircraft.SaveRisk(MenuType.Coverage, 0);
    }

    static SaveRisk(redirec, coverageId) {
        var AmountAccesoriesNew = 0;
        var AmountAccesoriesOld = 0;

        $("#formAircraft").validate();
        if ($("#formAircraft").valid()) {
            if (isCalculatePremiunCoverange === true) {
                riskData = RiskAircraft.GetRiskDataModel();
                aircraftDto = riskData;
                RiskAircraftRequest.SaveRisk(glbPolicy.Id, riskData, dynamicProperties).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            RiskAircraft.UpdateGlbRisk(data.result);
                            RiskAircraft.LoadSubTitles(0);
                            RiskAircraft.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                            isCalculatePremiunCoverange = false;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                            isCalculatePremiunCoverange = false;
                        }
                    }
                    else {
                        if (Array.isArray(data.result)) {
                            $.each(data.result, function (key, value) {
                                $.UifNotify('show', { 'type': 'info', 'message': value, 'autoclose': true });
                            });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.PremiumCero, 'autoclose': true });
            }
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields, 'autoclose': true });
        }
    }
    static ShowSaveRisk(riskId, redirec, coverageId) {
        //if (redirec != 9) {
        //    if ($("#selectRisk").UifSelect("getSelected") === null || $("#selectRisk").UifSelect("getSelected") === 0 || $("#selectRisk").UifSelect("getSelected") === "") {
        //        RiskAircraft.GetRisksByTemporalId(glbPolicy.Id, riskId);
        //    }
        //}
        var events = null;
        //lanza los eventos para la creación de el riesgo
        if (glbRisk.InfringementPolicies != null) {
            events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
        }
        if (events !== TypeAuthorizationPolicies.Restrictive) {
            RiskAircraft.RedirectAction(redirec, riskId, coverageId);

        }
        //fin - lanza los eventos para la creación de el riesgo
    }

    static RedirectAction(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskAircraft.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.Coverage:
                RiskAircraft.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskAircraft.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.AdditionalData:
                RiskAircraft.ShowPanelsRisk(MenuType.AdditionalData);
                break;
            case MenuType.Texts:
                RiskAircraft.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskAircraft.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Concepts:
                RiskAircraft.ShowPanelsRisk(MenuType.Concepts);
                break;
            case MenuType.Script:
                RiskAircraft.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                break;
        }
    }

    static ReturnCoverage(coverageId) {
        glbCoverage = {
            CoverageId: coverageId,
            Class: RiskAircraftCoverage
        }
        router.run("prtRiskAircraftCoverage");
    }

    static GetPolicyTypes(prefixId, id) {
        RiskAircraftRequest.GetPolicyType(prefixId, id).done(function (data) {
            if (data.success) {
                policyIsFloating = data.result;
                if (!data.result) {
                    $('#SpecificAircraft').show();
                    $('#AutomaticAircraft').hide();
                } else {
                    $('#AutomaticAircraft').show();
                    $('#SpecificAircraft').hide();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static CalculateDays(CurrentFrom, CurrentTo) {
        var aFecha1 = CurrentFrom.toString().split('/');
        var aFecha2 = CurrentTo.toString().split('/');
        var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(dias)) {
            totaldays = 0;
        }
        else {
            totaldays = dias;
        }
    }

    ChangeGroupCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskAircraft.GetInsuredObjectsByProductIdGroupCoverageId(selectedItem.Id);
        }
        else {
            $("#listCoverages").UifListView("refresh");
        }
        $('#inputPremium').text(0);
        $("#inputAmountInsured").text(0);
    }

    static GetInsuredObjectsByProductIdGroupCoverageId(groupCoverageId) {
        RiskAircraftRequest.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, groupCoverageId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
                RiskAircraft.LoadCoverages(data.result, groupCoverageId);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    static LoadCoverages(coverages, groupCoverageId) {
        coverages.forEach(e => {
            e.Id = e.Id;
            e.InsuredLimitAmount = FormatMoney(e.InsuredLimitAmount);
            e.PremiumAmount = FormatMoney(e.PremiumAmount);
            e.DisplayRate = FormatMoney(e.Rate, 2);
            e.Rate = FormatMoney(e.Rate);
            e.CurrentFrom = FormatDate(e.CurrentFrom);
            e.CurrentTo = FormatDate(e.CurrentTo);
            e.CurrentFromOriginal = FormatDate(e.CurrentFromOriginal);
            e.CurrentToOriginal = FormatDate(e.CurrentToOriginal);
            if (e.Deductible != null) {
                e.DeductibleDescription = e.Deductible.Description,
                    e.DeductibleId = e.Deductible.Id
            }

            $("#listCoverages").UifListView("addItem", e);
        })
    }

    static GetGroupCoverages(productId, selectedId) {
        RiskAircraftRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, filter: true });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskDataModel() {
        var riskData = $("#formAircraft").serializeObject();
        //riskData.FreightAmount = NotFormatMoney(riskData.FreightAmount);
        //riskData.LimitMaxRealeaseAmount = NotFormatMoney(riskData.LimitMaxRealeaseAmount);
        //riskData.MinimumPremium = NotFormatMoney(riskData.MinimumPremium);
        riskData.MakeId = $('#selectAircraftMake').UifSelect('getSelected');
        riskData.MakeDescription = $('#selectAircraftMake').UifSelect('getSelectedText');
        riskData.ModelId = $('#selectModel').UifSelect('getSelected');
        riskData.ModelDescription = $('#selectModel').UifSelect('getSelectedText');
        riskData.TypeId = $('#selectAircraftType').UifSelect('getSelected');
        riskData.UseId = $('#selectAirCraftUseIds').UifSelect('getSelected');
        riskData.RegisterId = $('#selectAirCraftEnrollment').UifSelect('getSelected');
        riskData.CurrentManufacturing = $('#inputCurrentManufacturing').val();
        riskData.OperatorId = $('#selectAirCraftOperator').UifSelect('getSelected');
        riskData.NumberRegister = $('#inputNumberRegister').val();

        //riskData.AircraftTypeIds = $('#selectAircraftTypeIds').UifMultiSelect('getSelected');
        riskData.PolicyId = glbPolicy.Id;
        //riskData.Description = $('#selectAircraftType').UifSelect('getSelectedText');
        riskData.Description = $('#selectAircraftMake').UifSelect('getSelectedText') + ' - ' + $('#selectModel').UifSelect('getSelectedText');

        var insuredObjectsValues = $("#listCoverages").UifListView('getData');
        if (insuredObjectsValues != null) {
            $.each(insuredObjectsValues, function (key, value) {
                this.InsuredLimitAmount = NotFormatMoney(this.InsuredLimitAmount);
                this.Rate = NotFormatMoney(this.Rate);
                this.PremiumAmount = NotFormatMoney(this.PremiumAmount, 2);
            });
        }
        riskData.InsuredObjects = insuredObjectsValues;

        if ($('#inputInsured').data("Object") == undefined) {
            $("#inputInsured").data("Detail", RiskAircraft.GetIndividualDetails(insured.CompanyName))
        }
        riskData.BirthDate = FormatDate($('#inputInsured').data("Object").BirthDate);
        riskData.Gender = $('#inputInsured').data("Object").Gender;
        riskData.IndividualId = $('#inputInsured').data("Object").IndividualId;
        riskData.DocumentType = $('#inputInsured').data("Object").IdentificationDocument.DocumentType.Id;
        riskData.DocumentNumber = $('#inputInsured').data("Object").IdentificationDocument.Number;
        riskData.Name = $('#inputInsured').data("Object").Name;

        riskData.DocumentTypeDescription = $('#inputInsured').data("Object").IdentificationDocument.DocumentType.Description;
        riskData.DocumentTypeSmallDescription = $('#inputInsured').data("Object").IdentificationDocument.DocumentType.SmallDescription;
        //var Expiration = $('#inputInsured').data("Object").IdentificationDocument.ExpeditionDate;
        //Expiration = Expiration.substring(6, Expiration.length - 2);
        riskData.DocumentExpedition = new Date();
        riskData.Profile = $('#inputInsured').data("Object").Profile;
        riskData.ScoreCredit = $('#inputInsured').data("Object").ScoreCredit;
        riskData.CustomerType = $('#inputInsured').data("Object").CustomerType;
        riskData.CustomerTypeDescription = $('#inputInsured').data("Object").CustomerTypeDescription;
        riskData.IndividualType = $('#inputInsured').data("Object").IndividualType;

        riskData.TradeName = $('#inputInsured').data("Object").CompanyName.TradeName;
        riskData.NameNum = $('#inputInsured').data("Object").CompanyName.NameNum;
        riskData.Address = $('#inputInsured').data("Object").CompanyName.Address.Description;
        riskData.Phone = $('#inputInsured').data("Object").CompanyName.Phone.Description;

        var email = $('#inputInsured').data("Object").CompanyName.Email;
        if (email != null) {
            riskData.Email = $('#inputInsured').data("Object").CompanyName.Email.Description
        } else {
            riskData.Email = "";
        }
        riskData.IsMain = $('#inputInsured').data("Object").CompanyName.IsMain;
        riskData.RiskId = $("#hiddenRiskId").val();
        return riskData;
    }

    static UpdateGlbRisk(data) {
        var recordScript = glbRisk.RecordScript;
        glbRisk = data
        $.extend(glbRisk, data.Risk);
        glbRisk.Object = "RiskAircraft";
        glbRisk.formRisk = "#formAircraft";
        glbRisk.RecordScript = recordScript;
        glbRisk.Class = RiskAircraft;

    }

    static UpdatePolicyComponents() {

        UnderwritingRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                RiskAircraft.ReturnUnderwriting();
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
            glbPolicy.EndorsementController = "AircraftModification";
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


    static GetPremium() {
        if ($('#inputInsured').data("Object") != null) {
            $("#formAircraft").validate();

            if ($("#formAircraft").valid()) {
                var recordScript = false;

                if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0) {
                    recordScript = true;
                }
                else {
                    recordScript = glbRisk.RecordScript;
                }

                if (recordScript) {
                    var riskData = RiskAircraft.GetRiskDataModel();
                    RiskAircraftRequest.GetPremium(glbPolicy.Id, riskData, dynamicProperties).done(function (data) {
                        if (data.success) {
                            RiskAircraft.LoadRisk(data.result);
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
                    RiskAircraft.LoadScript();
                }
            }
        } else {
            UnderwritingPersonOnline.ShowOnlinePerson();
        }
        dynamicProperties = riskData.Risk.DynamicProperties;
        dynamicProperties = this.ValidFormatDynamicProperties(dynamicProperties);
    }

    static DeleteRisk() {
        if ($("#selectRisk").UifSelect("getSelected") > 0) {
            RiskAircraftRequest.DeleteRisk(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                        RiskAircraft.ClearControls();
                        RiskAircraft.GetRisksByTemporalId(glbPolicy.Id);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                        ScrollTop();
                    }
                    else {
                        var riskId = $("#selectRisk").UifSelect("getSelected");
                        RiskAircraft.GetRisksByTemporalId(glbPolicy.Id, riskId);
                        RiskAircraft.GetRiskById(riskId);
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

        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
        $("#selectRisk").UifSelect("setSelected", null);
        $("#selectGroupCoverage").UifSelect("setSelected", null);
        $("#selectGroupCoverage").UifSelect("setSelected", null);
        $("#selectedBeneficiaries").text("");
        $("#selectedAdditionalData").text("");
        $("#selectedTexts").text("");
        $("#selectedClauses").text("");
        $("#selectedConceptsRisk").text("");
        isCalculatePremiunCoverange = false;
    }

    Accept() {
        var listCoveranges = $("#listCoverages").UifListView("getData");
        $.each(listCoveranges, function (index, value) {
            if (parseInt(value.InsuredLimitAmount) > 0 || parseInt(value.PremiumAmount) > 0) {
                isCalculatePremiunCoverange = true;
            }
        });
        RiskAircraft.SaveRisk(MenuType.Risk, 0, 1);
        ScrollTop();
        aircraftDto = null;
    }

    Close() {
        glbRisk = { Id: 0, Object: "RiskAircraft", Class: RiskAircraft };
        RiskAircraft.UpdatePolicyComponents();
        aircraftDto = null;

    }
    static UpdatePremiunAndAmountInsured() {
        var insuredObjects = $("#listCoverages").UifListView('getData');
        var totalSumInsured = 0;
        var totalPremium = 0;
        if (insuredObjects != null) {
            $.each(insuredObjects, function (index, value) {
                totalPremium = totalPremium + parseFloat(NotFormatMoney(insuredObjects[index].PremiumAmount, 2));
                totalSumInsured = totalSumInsured + parseFloat(NotFormatMoney(insuredObjects[index].InsuredLimitAmount, 2));
            });
        }

        $("#PremiumAmount").text(FormatMoney(totalPremium, 2));
        $("#SubLimitAmount").text(FormatMoney(totalSumInsured, 2));
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
            if (glbRisk.Concepts != null) {
                if (glbRisk.Concepts.length > 0) {
                    $('#selectedConceptsRisk').text("(" + glbRisk.Clauses.length + ")");
                }
                else {
                    $('#selectedConceptsRisk').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedConceptsRisk').text("(" + AppResources.LabelWithoutData + ")");
            }
        }
    }

    static ValidateNumberRegister() {
        var regexNumberRegister = /^[A-záéíóúÁÉÍÓÚ \-\_]{1,50}$/;
        return regexNumberRegister.test($('#inputNumberRegister').val());
    }
}