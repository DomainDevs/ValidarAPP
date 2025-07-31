var riskController;
var dynamicProperties = null;
var countryParameterLiability = 1;
var individualSearchType = 1;
var isnCalculate;
var IsEditing = false;
var glbSuretyRiskSubActivity = null;
var glbSuretyRiskSubActivity = null;
var addRiskControl = false;
class RiskLiability extends Uif2.Page {

    getInitialState() {
        //VARIABLES
        riskController = 'RiskLiability'
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskLiability", formRisk: "#formLiability", RecordScript: false, Class: RiskLiability, AddressId: 0, PhoneID: 0 };
        }
        if (glbPolicy.TemporalType == 1) {
            $("#listCoverages").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });
        }
        else {
            $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });
        }
        UnderwritingQuotation.DisabledButtonsQuotation();
        RiskLiability.validateLiability();
        $("#btnConvertProspect").hide();
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [0, 1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        RiskLiability.ShowPanelsRisk(MenuType.Risk);
        RiskLiability.GetAssuranceMode();
        if ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) && gblCurrentRiskTemporalNumberOld == null) {
            RiskLiability.GetRisksByTemporalId(gblCurrentRiskTemporalNumber, glbRisk.Id);
        }
        else {
            RiskLiability.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
        }

        $("#chkIsDeclarative").on("change", RiskLiability.ChangeIsRetention);
        $("#chkIsRetention").on("change", RiskLiability.ChangeIsFacultative);
    }

    bindEvents() {

        $('#selectRisk').on('itemSelected', RiskLiability.ChangeRisk);
        $("#inputInsured").on('buttonClick', this.SearchInsured);
        $("#btnDetail").on('click', this.ShowDetail);
        $('#btnIndividualDetailAccept').on('click', RiskLiability.SetIndividualDetail);
        $('#selectCountry').on('itemSelected', this.selectCountry);
        $("#inputDaneCode").on('itemSelected focusout', this.inputDaneCodeItemSelected);
        $('#selectState').on('itemSelected', this.selectState);
        $('#selectCity').on('itemSelected', this.selectCity);
        $('#selectGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $('#listCoverages').on('rowAdd', this.AddCoverage);
        $('#listCoverages').on('rowEdit', this.EditCoverage);
        $('#listCoverages').on('rowDelete', this.CoverageDelete);
        $("#btnAccept").on('click', this.Accept);
        $("#btnClose").on('click', RiskLiability.UpdatePolicyComponents.bind(true));
        $("#btnIndividualDetailAccept").on('click', RiskLiability.SetIndividualDetail);
        $("#btnScript").on('click', RiskLiability.LoadScript);
        $("#btnAcceptNewPersonOnline").click(this.AcceptNewPersonOnline);
        $('#btnConvertProspect').click(this.ConvertProspect);
        $("#btnAddRisk").on('click', this.AddRisk);
        $('#btnDeleteRisk').on('click', RiskLiability.DeleteRisk);
        $("#btnAcceptNewPersonOnline").click(this.AcceptNewPersonOnline);
        $('#btnConvertProspect').click(this.ConvertProspect);
        $('#tableIndividualResults tbody').on('click', 'tr', function (e) {
            if (individualSearchType == 2) {
                RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
            }
            else {
                RiskLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
            }
            $('#modalIndividualSearch').UifModal('hide');
        });
        $('#selectRiskActivity').on('itemSelected', RiskLiability.GetSubActivityRisksByActivityRiskId);

        $('#inputFullAddress').focusout(RiskLiability.ValidateAddress);
        $('#inputName').on('itemSelected', this.ChangeReasonSocial);
    }
    static getPersonOnline() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
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
                    RiskLiability.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskLiability.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }
    static GetRisksByTemporalId(temporalId, selectedId) {
        var controller = "";

        RiskLiabilityRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    if (selectedId == 0) {
                        if (data.result.length == 1) {
                            $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: data.result[0].Id });
                            RiskLiability.GetRiskById(data.result[0].Id);
                        } else {

                            $("#selectRisk").UifSelect({ sourceData: data.result });
                        }
                    }
                    else {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                } else {
                    $("#selectRisk").UifSelect({ sourceData: null });
                }

                if (glbPersonOnline != null) {
                    RiskLiability.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                    //.GetRiskById($("#selectRisk option[Value!='']")[0].value);
                }

                else if (glbRisk.Id > 0) {
                    RiskLiability.GetRiskById(glbRisk.Id);
                }
                else {

                    RiskLiability.GetCountriesLiability(null);
                    RiskLiability.GetGroupCoverages(glbPolicy.Product.Id, 0);
                    RiskLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
                    RiskLiability.GetRiskActivities(0);
                }

                if (!glbPolicy.Product.IsFlatRate) {
                    $('#inputRate').prop('disabled', true);
                }
                RiskLiability.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorTempNoExist, 'autoclose': true });
            }


        });
    }
    ChangeGroupCoverage(event, selectedItem) {
        var Address = $("#inputFullAddress").val();
        if (Address == undefined || Address == "") {
            $("#ReqinputAddress").show();
            $('#selectGroupCoverage').UifSelect("setSelected", null)
            return;
        } else {
            $("#ReqinputAddress").hide();
            if ($('#formLiability').valid()) {
                if (selectedItem.Id > 0) {
                    RiskLiability.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id);
                }
                else {
                    $("#listCoverages").UifListView("refresh");
                }
                $('#inputPremium').text(0);
                $("#inputAmountInsured").text(0);
            } else {
                $('#selectGroupCoverage').UifSelect("setSelected", null)
            }
        }
    }

    AddCoverage(event) {
        RiskLiability.SaveRisk(MenuType.Coverage, 0);
    }

    EditCoverage(event, data, index) {
        if (data.Id != null && data.Id > 0) {
            IsEditing = true;
            RiskLiability.SaveRisk(MenuType.Coverage, data.Id);
        }
    }

    CoverageDelete(event, data) {
        RiskLiability.DeleteCoverage(data);
    }

    Accept() {
        RiskLiability.SaveRiskGeneral(MenuType.Risk, 0);
        RiskLiability.UpdatePolicyComponents(false);
        ScrollTop();
    }

    AddRisk() {
        addRiskControl = true;
        const data = $(".uif-listview").UifListView("getData");
        $.each(data, function (index, item) {
            $(".uif-listview").UifListView("deleteItem", index);
        });
        RiskLiability.ClearControls();
        RiskLiability.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    ShowDetail() {
        if ($("#inputInsured").data("Object") != undefined) {
            RiskLiability.ShowInsuredDetail();
        }
    }

    AcceptNewPersonOnline() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskLiability.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsured").val().trim());
    }

    ConvertProspect() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskLiability.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsured").data("Object").IndividualId, $("#inputInsured").data("Object").IndividualType, $("#inputInsured").data("Object").CustomerType, 0);
    }

    selectCountry(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = "";
            RiskLiabilityRequest.GetStatesByCountryId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    controller = data.result;

                    $('#selectState').UifSelect({ sourceData: controller });

                    $('#inputDaneCode').val('');
                    $('#selectCity').UifSelect();

                    if (selectedItem.Id == countryParameterLiability) {
                        $('#inputDaneCode').prop('disabled', false);
                    }
                    else {
                        $('#inputDaneCode').prop('disabled', true);
                    }
                }

            });
        }
    }

    inputDaneCode(event, selectedItem) {
        if (selectedItem.DANECode.length > 0) {
            RiskLiabilityRequest.GetStateCityByDaneCode(selectedItem.DANECode, $('#selectCountry').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    var controllerStates = "";
                    RiskLiabilityRequest.GetStatesByCountryId(data.result.id).done(function (data) {
                        if (data.success) {
                            controllerStates = data.result;
                            $('#selectState').UifSelect({ sourceData: controllerStates, selectedId: data.result.State.Id });
                        }

                    });
                    var controllerCities = "";
                    RiskLiabilityRequest.GetCitiesByCountryIdStateId().done(function (data) {
                        if (data.success) {
                            controllerCities = data.result;
                            $('#selectCity').UifSelect({ sourceData: controllerCities, selectedId: selectedItem.Id });
                            $('#inputDaneCode').val(selectedItem.DANECode);
                        }

                    });
                    RiskLiability.RiskLiability($('#selectCountry').UifSelect('getSelected'), data.result.State.Id);
                }
            });
        }
        else {
            $('#inputDaneCode').val('');
        }
    }
    inputDaneCodeItemSelected(e) {
        var inputDaneCode = $("#inputDaneCode").val();
        if (inputDaneCode != null && inputDaneCode != "") {
            if ($("#selectCountry").UifSelect("getSelected") != null && $("#selectCountry").UifSelect("getSelected") != "") {
                var isSearchDaneCodeCompany = true;
                RiskLiabilityRequest.GetStateCityByDaneCode(inputDaneCode, $('#selectCountry').UifSelect("getSelected")).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            var citySelectedPerson = data.result.Id;
                            var stateSelectedPerson = data.result.State.Id;
                            RiskLiabilityRequest.GetStatesByCountryId($("#selectCountry").UifSelect("getSelected")).done(function (data) {
                                if (data.success) {
                                    $("#selectState").UifSelect({ sourceData: data.result, selectedId: stateSelectedPerson });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                }
                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                            });



                            RiskLiabilityRequest.GetCitiesByCountryIdStateId($("#selectCountry").UifSelect("getSelected"), data.result.State.Id).done(function (data) {
                                if (data.success) {
                                    $("#selectCity").UifSelect({ sourceData: data.result, selectedId: citySelectedPerson });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                }
                                $('#inputDaneCode').UifAutoComplete('setValue', inputDaneCode);
                                RiskLiability.GetRatingZone(data.result[0].State.Country.Id, data.result[0].State.Id);
                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                            });
                        }
                    }
                });
            }
        }
    }

    selectState(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = "";
            RiskLiabilityRequest.GetCitiesByCountryIdStateId($('#selectCountry').UifSelect('getSelected'), selectedItem.Id).done(function (data) {
                if (data.success) {
                    controller = data.result;
                    $('#selectCity').UifSelect({ sourceData: controller });
                    RiskLiability.GetRatingZone($('#selectCountry').UifSelect('getSelected'), selectedItem.Id);
                    $('#inputDaneCode').val('');
                }
            });
        }
    }

    selectCity(event, selectedItem) {
        if (selectedItem.Id > 0 && $('#selectCountry').UifSelect('getSelected') == countryParameterLiability) {
            RiskLiabilityRequest.GetDaneCodeByCountryIdByStateIdByCityId($('#selectCountry').UifSelect('getSelected'), $('#selectState').UifSelect('getSelected'), selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#inputDaneCode').UifAutoComplete('setValue', data.result);
                }
            });
        }
        else {
            $('#inputDaneCode').UifAutoComplete('setValue', '');
        }
    }

    SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            RiskLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);

        }
    }

    static ChangeRisk(event, selectedItem) {
        addRiskControl = false;
        if (selectedItem.Id > 0) {
            RiskLiability.GetRiskById(selectedItem.Id);
            RiskLiability.DisabledControlByEndorsementType(true);
        }
        else {
            RiskLiability.ClearControls();
        }
    }


    static GetGroupCoverages(productId, selectedId) {
        var controller = "";
        RiskLiabilityRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: controller, selectedId: selectedId });
                    RiskLiabilityCoverage.LoadCoverage(data.result);
                }
            }
        });
    }

    static GetCountriesLiability(city) {
        if (city != null) {
            if (city.State != null && city.State.Id != 0) {
                var controllerCountries = "";
                var controllerByCountryId = "";
                var controllerByCountryIdStateId = "";


                RiskLiabilityRequest.GetCountries().done(function (data) {
                    if (data.success) {
                        controllerCountries = data.result;
                        $('#selectCountry').UifSelect({ sourceData: controllerCountries, selectedId: city.State.Country.Id });
                        RiskLiabilityRequest.GetStatesByCountryId(city.State.Country.Id).done(function (data) {
                            if (data.success) {
                                controllerByCountryId = data.result;
                                $('#selectState').UifSelect({ sourceData: controllerByCountryId, selectedId: city.State.Id });
                                RiskLiabilityRequest.GetCitiesByCountryIdStateId(city.State.Country.Id, city.State.Id).done(function (data) {
                                    if (data.success) {
                                        controllerByCountryIdStateId = data.result;
                                        $('#selectCity').UifSelect({ sourceData: controllerByCountryIdStateId, selectedId: city.Id });

                                        if (city.DANECode != null) {
                                            $('#inputDaneCode').UifAutoComplete('setValue', city.DANECode);
                                        } else {
                                            RiskLiability.GetDaneCodebyCountryIdStateIdCityId(city.State.Country.Id, city.State.Id, city.Id);
                                        }
                                        RiskLiability.GetRatingZone(city.State.Country.Id, city.State.Id);
                                    }
                                });
                            }
                        });
                    }
                });
            }

        }
        else {
            var controllerCountries = "";
            RiskLiabilityRequest.GetCountries().done(function (data) {
                if (data.success) {
                    controllerCountries = data.result;
                    $('#selectCountry').UifSelect({ sourceData: controllerCountries, selectedId: countryParameterLiability });

                    RiskLiabilityRequest.GetStatesByCountryId(countryParameterLiability).done(function (data) {
                        if (data.success) {
                            controllerByCountryId = data.result;
                            $('#selectState').UifSelect({ sourceData: controllerByCountryId });
                            $('#selectCity').UifSelect();
                        }

                    });

                }
            });

            $('#inputDaneCode').UifAutoComplete('setValue', '');
        }
    }

    static ShowPanelsRisk(Menu) {
        switch (Menu) {
            case MenuType.Risk:
                break;
            case MenuType.Texts:
                $('#modalTexts').UifModal('showLocal', AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $('#modalClauses').UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Beneficiaries:
                $('#modalBeneficiaries').UifModal('showLocal', AppResources.LabelBeneficiaries);
                break;
            case MenuType.Script:
                RiskLiability.LoadScript();
                break;
        }
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    static GetRiskById(id) {
        if (id > 0 && glbPolicy.Endorsement != null) {
            RiskLiabilityRequest.GetRiskById(id).done(function (data) {
                if (data.success) {
                    glbRisk.RecordScript = true;
                    if (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumber != undefined) {
                        RiskLiability.LoadTemporalRiskSeach(data.result);
                    }
                    else {
                        if (glbPolicy.Endorsement.EndorsementType == 2) {
                            if (glbRisk.Coverages != undefined && glbRisk.Coverages.length > 0 && glbRisk.Id == data.result.Risk.Id) {
                                data.result.Risk.Coverages = glbRisk.Coverages;
                                data.result.Risk.Premium = 0;
                                glbRisk.Coverages.forEach(function (item) {
                                    data.result.Risk.Premium = data.result.Risk.Premium + parseFloat(item.PremiumAmount.toString().replace(",", "."));
                                });
                            }
                            if (data.result.Risk.OriginalStatus == null) {
                                addRiskControl = true;
                            }
                            else {
                                addRiskControl = false;
                            }
                        }
                        RiskLiability.LoadRisk(data.result);
                    }
                    glbRisk.Clauses = data.result.Risk.Clauses;
                    glbRisk.Beneficiaries = data.result.Risk.Beneficiaries;
                    glbRisk.Text = data.result.Text;
                    RiskLiability.DisabledControlByEndorsementType(true);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
            });
        }
    }

    static LoadRisk(riskData) {

        if (riskData.Risk != null && riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        if (riskData.Risk != null) {
            if (riskData.Risk.Id > 0) {
                $('#selectRisk').UifSelect('setSelected', riskData.Risk.Id);
            }

            if (riskData.Risk.MainInsured != null && riskData.Risk.MainInsured !== null && riskData.Risk.MainInsured != undefined) {
                $('#inputInsured').data('Object', riskData.Risk.MainInsured);
                var individualDetails = Property.GetIndividualDetailsByIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CustomerType)
                $('#inputInsured').data('Detail', RiskLiability.GetIndividualDetails(individualDetails));
                $('#inputInsured').val(decodeURIComponent(riskData.Risk.MainInsured.Name + ' (' + riskData.Risk.MainInsured.IdentificationDocument.Number + ')'));
                RiskLiability.getReasonSocialIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CompanyName.NameNum);
            }
            if (riskData.Risk.RiskActivity != null) {
                RiskLiability.GetRiskActivities(riskData.Risk.RiskActivity.Id);
            }
            else {
                RiskLiability.GetRiskActivities(0);
            }
            if (riskData.RiskSubActivity != null) {
                RiskLiability.GetRiskSubActivities(riskData.Risk.RiskActivity.Id, riskData.RiskSubActivity.Id);
            }
            else {
                $("#selectRiskSubActivity").UifSelect('setSelected', 0);
            }

            if (riskData.Risk.GroupCoverage != null) {
                RiskLiability.GetGroupCoverages(glbPolicy.Product.Id, riskData.Risk.GroupCoverage.Id);
            }

            if (riskData.Risk.Coverages != null) {
                RiskLiability.LoadCoverages(riskData.Risk.Coverages);
            }
            else if (riskData.Risk.GroupCoverage != null) {
                RiskLiability.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, riskData.Risk.GroupCoverage.Id);
                $('#inputAmountInsured').text(0);
                $('#inputPremium').text(0);
            }

            dynamicProperties = riskData.Risk.DynamicProperties;

        }

        RiskLiability.GetCountriesLiability(riskData.City);

        $('#chkIsDeclarative').prop('checked', riskData.IsDeclarative);
        $('#chkIsRetention').prop('checked', riskData.Risk.IsRetention);

        $('#inputFullAddress').val(riskData.FullAddress);

        if (riskData.FullAddress != null) {
            $("#inputFullAddress").val(decodeURIComponent(riskData.FullAddress));
        }
        if (riskData.AssuranceMode != null) {
            RiskLiability.GetAssuranceMode(riskData.AssuranceMode.Id);
            //$("#selectAssuranceMode").UifSelect("setSelected", riskData.AssuranceMode.Id);
        }
        if (riskData.RiskSubActivity != null) {
            glbSuretyRiskSubActivity = riskData.RiskSubActivity.Id;
            $("#selectRiskSubActivity").UifSelect("setSelected", riskData.RiskSubActivity.Id);
        }
        if ($("#chkIsDeclarative").prop("checked")) {
            $('#chkIsRetention').prop("disabled", true);
        } else {
            $("#chkIsRetention").prop("disabled", false);
        }
        if ($("#chkIsRetention").prop("checked")) {
            $('#chkIsDeclarative').prop("disabled", true);
        } else {
            $("#chkIsDeclarative").prop("disabled", false);
        }
        RiskLiability.UpdateGlbRisk(riskData);
        RiskLiability.LoadSubTitles(0);
        RiskLiability.ValidarEnabledCampos();
    }

    static LoadTemporalRiskSeach(riskData) {

        if (riskData.Risk != null && riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        dynamicProperties = riskData.Risk.DynamicProperties;
        //RiskLiability.GetCountriesLiability(riskData.City);
        //if (riskData.FullAddress != null) {
        //    $("#inputFullAddress").val(decodeURIComponent(riskData.FullAddress));
        //}
        //if (riskData.AssuranceMode != null) {
        //    RiskLiability.GetAssuranceMode(riskData.AssuranceMode.Id);
        //    //$("#selectAssuranceMode").UifSelect("setSelected", riskData.AssuranceMode.Id);
        //}
        //if (riskData.RiskSubActivity != null) {
        //    $("#selectRiskSubActivity").UifSelect("setSelected", riskData.RiskSubActivity.Id);
        //}
        //if ($("#chkIsDeclarative").prop("checked")) {
        //    $('#chkIsRetention').prop("disabled", true);
        //} else {
        //    $("#chkIsRetention").prop("disabled", false);
        //}
        //if ($("#chkIsRetention").prop("checked")) {
        //    $('#chkIsDeclarative').prop("disabled", true);
        //} else {
        //    $("#chkIsDeclarative").prop("disabled", false);
        //}
        RiskLiability.UpdateGlbRisk(riskData);
        RiskLiability.LoadSubTitles(0);
        RiskLiability.ValidarEnabledCampos();
    }

    static GetDaneCodebyCountryIdStateIdCityId(countryId, stateId, cityId) {
        RiskLiabilityRequest.GetDaneCodebyCountryIdStateIdCityId(countryId, stateId, cityId).done(function (data) {
            if (data.success) {
                $('#inputDaneCode').UifAutoComplete('setValue', data.result);
            }
        });
    }

    static ValidateTypeCoverage(model) {

        if (model.IsPrimary) {
            return AppResources.CoveragePrincipal;
        }
        else if (model.MainCoverageId > 0 && model.AllyCoverageId == null) {
            return AppResources.CoverageAdditional;
        }
        else if (model.AllyCoverageId > 0 && model.AllyCoverageId != null) {
            return AppResources.CoverageAllied
        }
        else {
            return AppResources.CoverageAdditional;
        }

    }
    static LoadCoverages(coverages) {
        var totalAmount = 0;
        var totalPremium = 0;
        if (glbPolicy.TemporalType == 1) {
            $('#listCoverages').UifListView({ source: null, customDelete: false, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: '#coverageTemplate', height: 340, title: AppResources.LabelTitleCoverages, focusItem: false });
        }
        else {
            $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', height: 340, title: AppResources.LabelTitleCoverages, focusItem: false });
        }

        $.each(coverages, function (index, val) {

            if (this.IsPrimary == true) {
                if (glbPolicy.Endorsement.EndorsementType == 2) {
                    totalPremium = totalPremium + parseFloat(coverages[index].PremiumAmount.toString().replace(",", "."));
                    if (isNaN(coverages[index].LimitAmount)) {
                        totalAmount = totalAmount + parseFloat(NotFormatMoney(coverages[index].LimitAmount).toString().replace(",", "."));
                    }
                    else {
                        totalAmount = totalAmount + parseFloat(coverages[index].LimitAmount);
                    }
                } else {
                    totalPremium = totalPremium + coverages[index].PremiumAmount;
                    totalAmount = totalAmount + coverages[index].LimitAmount;
                }
            }

            coverages[index].CurrentFrom = glbPolicy.CurrentFrom;
            coverages[index].CurrentTo = glbPolicy.CurrentFrom;
            coverages[index].TypeCoverage = RiskLiability.ValidateTypeCoverage(coverages[index]);
            coverages[index].DeclaredAmount = FormatMoney(coverages[index].DeclaredAmount);
            coverages[index].LimitAmount = FormatMoney(coverages[index].LimitAmount);
            coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
            coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);
            coverages[index].Rate = FormatMoney(coverages[index].Rate);
            coverages[index].LimitOccurrenceAmount = FormatMoney(coverages[index].LimitOccurrenceAmount);
            coverages[index].LimitClaimantAmount = FormatMoney(coverages[index].LimitClaimantAmount);
            coverages[index].MaxLiabilityAmount = FormatMoney(coverages[index].MaxLiabilityAmount);
            coverages[index].DisplayRate = FormatMoney(coverages[index].Rate, 2);
            coverages[index].FlatRatePorcentage = FormatMoney(coverages[index].FlatRatePorcentage);
            coverages[index].PreRuleSetId = FormatMoney(coverages[index].PreRuleSetId);
            coverages[index].RuleSetId = FormatMoney(coverages[index].RuleSetId);

            if (coverages[index].AllyCoverageId != null) {
                coverages[index].allowEdit = false;
                coverages[index].allowDelete = false;
            } else {
                coverages[index].allowEdit = true;
                coverages[index].allowDelete = true;
            }

            if (glbPolicy.Endorsement.EndorsementType == 2) {
                if (glbPolicyEndorsement.Endorsement.ModificationTypeId == 4) {
                    coverages[index].CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
                    coverages[index].CurrentTo = FormatDate(glbPolicy.CurrentTo);
                }
            }
            if (glbPolicy.Endorsement.EndorsementType == 2) {
                if (glbPolicyEndorsement.Endorsement.ModificationTypeId != 4 && coverages[index].CalculationType == 2 && coverages[index].RateType == 3) {
                    if (IsEdit == undefined || IsEdit == null || IsEdit == false) {
                        coverages[index].Rate = 0;
                        coverages[index].PremiumAmount = 0;
                    }
                }
            }
            $("#listCoverages").UifListView("addItem", coverages[index]);

        });

        glbRisk.Premium = totalPremium;
        glbRisk.AmountInsured = FormatMoney(totalAmount);

        $('#inputAmountInsured').text(FormatMoney(glbRisk.AmountInsured));
        $('#inputPremium').text(FormatMoney(glbRisk.Premium));
        if (totalAmount == 0 || totalPremium == 0) {
            isnCalculate = false;
        } else {
            isnCalculate = true;
        }
    }

    static GetCoveragesByProductIdGroupCoverageId(productId, groupCoverageId) {
        var coverage = "";
        RiskLiabilityRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, productId, groupCoverageId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                coverage = data.result;
                RiskLiability.LoadCoverages(coverage);
                var riskData = RiskLiability.GetRiskDataModel();
                RiskLiabilityRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                    if (data.success) {
                        var recordScript = glbRisk.RecordScript;
                        glbRisk = data.result;
                        $.extend(glbRisk, data.result.Risk);

                        glbRisk.Object = "RiskLiability";
                        glbRisk.formRisk = "#formLiability";
                        glbRisk.RecordScript = recordScript;
                        glbRisk.Class = RiskLiability;

                        RiskLiability.LoadSubTitles(0);
                        if (IsEdit) {
                            IsEdit = false;
                        }
                        RiskLiabilityCoverageRequest.RunRulesCoveragePreLiability(glbPolicy.Id, glbRisk.Id, coverage).done(function (data) {
                            if (data.success) {
                                coverage = data.result;
                                RiskLiability.LoadCoverages(coverage);
                            }
                        });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
                });
            }
        });
    }

    static DisabledControlByEndorsementType(disabled) {
        var endorsementType = parseInt(glbPolicy.Endorsement.EndorsementType, 10);
        if (addRiskControl) {
            disabled = false;
        }
        switch (endorsementType) {
            case EndorsementType.Renewal: {
                $('#inputInsured').prop('disabled', disabled);
                $('#selectCountry').prop('disabled', disabled);
                $('#selectCity').prop('disabled', disabled);
                $('#selectState').prop('disabled', disabled);
                $('#inputDaneCode').prop('disabled', disabled);
                break;
            }
            case EndorsementType.Modification: {
                $('#inputInsured').prop('disabled', disabled);
                $('#selectCountry').UifSelect('disabled', disabled);
                $('#selectCity').UifSelect('disabled', disabled);
                $('#selectState').UifSelect('disabled', disabled);
                $('#inputDaneCode').prop('disabled', disabled);
                break;
            }
        }
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            if (customerType == null && glbPolicy.TemporalType == TemporalType.Policy) {
                customerType = CustomerType.Individual;
            }
            RiskLiabilityRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (individualSearchType == 1 || individualSearchType == 2) {
                        if (data.result.length == 0 && individualSearchType != 2) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessagePerson, 'autoclose': true });
                            RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                        }
                        else if (data.result.length == 0) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchInsureds, 'autoclose': true });
                            RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                        }
                        else if (data.result.length == 1) {
                            RiskLiability.LoadInsured(data.result[0]);
                        }
                        else if (data.result.length > 1) {
                            modalListType = 1;
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].IdentificationDocument.Number,
                                    DocumentNum: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            RiskLiability.ShowModalList(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
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
                    RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                RiskJudicialSurety.getReasonSocialIndividualId(-1, 1);
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsured, 'autoclose': true });
            });
        }
    }

    static ShowInsuredDetail() {
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($("#inputInsured").data('Detail') != null) {
            var insuredDetails = $("#inputInsured").data('Detail');

            if (insuredDetails != null && insuredDetails.length > 0) {
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

    static ShowModalHolderDetail() {

        $('#tableIndividualDetails').UifDataTable('clear');

        if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CustomerType == CustomerType.Individual) {
            detailType = 1;

            if ($("#inputHolder").data('Detail') != null) {
                holderDetails = $("#inputHolder").data('Detail');

                if (holderDetails.length > 0) {
                    $.each(holderDetails, function (id, item) {
                        $('#tableIndividualDetails').UifDataTable('addRow', item)
                        if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.NameNum > 0 && $("#inputHolder").data("Object").CompanyName.NameNum == this.NameNum) {
                            $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                            $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                        else if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.NameNum == 0 && this.IsMain == true) {
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

            $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelHolderDetail);
        }
    }

    static GetIndividualDetails(individualDetails) {
        if (individualDetails.length > 0) {
            $.each(individualDetails, function (id, item) {
                if (this.Address != null)
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
        var resultData = {};
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                if (individualSearchType == 2) {
                    $("#inputBeneficiaryName").data('Detail', RiskThirdPartyLiability.GetIndividualDetails(data.result));

                }
                else {
                    $("#inputInsured").data('Detail', RiskThirdPartyLiability.GetIndividualDetails(data.result));
                }
                resultData = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
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

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    static SetCountryOrigin(address) {
        if (address != null) {
            $('#inputFullAddress').val(address.Description);
            if (address.City != null) {
                if (address.City.State != null) {
                    var controller = rootPath + 'Underwriting/RiskLiability/GetCountries';
                    $('#selectCountry').UifSelect({ source: controller, selectedId: address.City.State.Country.Id });
                    controller = rootPath + 'Underwriting/RiskLiability/GetStatesByCountryId?countryId=' + address.City.State.Country.Id;
                    $('#selectState').UifSelect({ source: controller, selectedId: address.City.State.Id });
                    controller = rootPath + 'Underwriting/RiskLiability/GetCitiesByCountryIdStateId?countryId=' + address.City.State.Country.Id + '&stateId=' + address.City.State.Id;
                    $('#selectCity').UifSelect({ source: controller, selectedId: address.City.Id });
                    if (address.City.DANECode != null) {
                        $('#inputDaneCode').UifAutoComplete('setValue', address.City.DANECode);
                    }
                    RiskLiability.GetRatingZone(address.City.State.Country.Id, address.City.State.Id);
                }
            }
        }
    }

    static ClearControls() {

        $('#selectRisk').UifSelect("setSelected", null);
        $('#selectedTexts').val('');
        $('#selectedClauses').val('');
        $('#selectedBeneficiaries').val('');
        $('#inputFullAddress').val('');
        $('#inputDaneCode').val('').prop('disabled', false);
        $('#selectState').UifSelect();
        $('#selectCity').UifSelect();
        $('#inputRateZone').val('').prop('disabled', false);
        $('#inputPrice').val('');
        $('#selectGroupCoverage').prop('disabled', false);
        $('#selectGroupCoverage').UifSelect('setSelected', null);
        $("#listCoverages").UifListView("refresh");
        $('#inputRate').text(0);
        $('#lblAddress').val('');
        $('#lblPhone').val('');
        $('#lblEmail').val('');
        $('#inputAmountInsured').text(0);
        $('#inputPremium').text(0);
        $('#selectRiskActivity').UifSelect('setSelected', null);
        $('#chkIsDeclarative').prop('checked', false);
        $('#selectRiskSubActivity').UifSelect('setSelected', null);
        $('#chkIsRetention').prop('checked', false);
        $('#selectAssuranceMode').UifSelect('setSelected', null);
        RiskLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        glbRisk.RecordScript = false;
        RiskLiability.UpdateGlbRisk({ Id: 0 });
    }

    static GetRatingZone(country, state) {
        RiskLiabilityRequest.GetRatingZone(glbPolicy.Prefix.Id, country, state).done(function (data) {
            if (data.success) {
                $('#inputRateZone').val(data.result.Description).prop('disabled', true);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRatingZone, 'autoclose': true });
        });
    }

    static SaveRisk(redirec, coverageId) {
        var recordScript = false;
        $('#formLiability').validate();
        if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0 || redirec == MenuType.Script) {
            recordScript = true;
        }
        else {
            recordScript = glbRisk.RecordScript;
        }

        if ($('#formLiability').valid()) {
            if (recordScript) {
                var riskData = RiskLiability.GetRiskDataModel();
                RiskLiabilityRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                    if (data.success) {
                        RiskLiability.UpdateGlbRisk(data.result);
                        RiskLiability.LoadSubTitles(0);
                        RiskLiability.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                });
            }
            else {
                RiskLiability.LoadScript(coverageId);
            }
        }
    }

    static SaveTemporalRiskSearch(redirec, coverageId) {
        var recordScript = false;
        if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0 || redirec == MenuType.Script) {
            recordScript = true;
        }
        else {
            recordScript = glbRisk.RecordScript;
        }
        if (recordScript) {
            var riskData = RiskLiability.GetTemporalRiskDataModel();
            RiskLiabilityRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                if (data.success) {
                    RiskLiability.UpdateGlbRisk(data.result);
                    RiskLiability.LoadSubTitles(0);
                    RiskLiability.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                    glbPolicy.Summary.RiskCount = 1;
                    $("#modalRiskFromPolicy").UifModal("hide");
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
            });
        }
        else {
            RiskLiability.LoadScript(coverageId);
        }
    }

    static SaveGeneralTemporalRiskSearch(redirec, coverageId) {
        var recordScript = false;
        if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0 || redirec == MenuType.Script) {
            recordScript = true;
        }
        else {
            recordScript = glbRisk.RecordScript;
        }
        var riskData = RiskLiability.GetTemporalRiskDataModel();
        if (riskData.AmountInsured != 0) {
            isnCalculate = true;
        }
        if (isnCalculate === true) {
            if (glbRisk.Beneficiaries != null) {

                if (glbRisk.Beneficiaries[0].CustomerType == CustomerType.Prospect && glbPolicy.TemporalType == TemporalType.Policy) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true });
                    return false;
                }
            }
            if (recordScript) {
                var riskData = RiskLiability.GetTemporalRiskDataModel();
                RiskLiabilityRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                    if (data.success) {
                        RiskLiability.UpdateGlbRisk(data.result);
                        RiskLiability.LoadSubTitles(0);
                        RiskLiability.ShowSaveRisk(glbRisk.Id, redirec, coverageId);

                        if (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumberOld == null) {
                            gblCurrentRiskTemporalNumberOld = gblCurrentRiskTemporalNumber;
                            gblCurrentRiskTemporalNumber = data.result.Id;
                            glbPolicy.Summary.RiskCount = 1;
                            $("#selectedInclusionRisk").text(glbPolicy.Summary.RiskCount);
                            $("#modalRiskFromPolicy").UifModal("hide");
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                });
            }
            else {
                RiskLiability.LoadScript();
            }


        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRiskPremiumAndAmountInsured, 'autoclose': true });
        }
    }

    static SaveRiskGeneral(redirec, coverageId) {
        var recordScript = false;
        $('#formLiability').validate();
        if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0 || redirec == MenuType.Script) {
            recordScript = true;
        }
        else {
            recordScript = glbRisk.RecordScript;
        }
        var riskData = RiskLiability.GetRiskDataModel();
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && riskData.AmountInsured != 0) {
            isnCalculate = true;
        }
        if (isnCalculate === true) {

            if ($('#formLiability').valid()) {
                if (glbRisk.Beneficiaries != null) {

                    if (glbRisk.Beneficiaries[0].CustomerType == CustomerType.Prospect && glbPolicy.TemporalType == TemporalType.Policy) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true });
                        return false;
                    }
                }
                if (recordScript) {
                    //RiskLiability.QuotationRisk();
                    var riskData = RiskLiability.GetRiskDataModel();
                    RiskLiabilityRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                        if (data.success) {
                            RiskLiability.UpdateGlbRisk(data.result);
                            RiskLiability.LoadSubTitles(0);
                            RiskLiability.ShowSaveRisk(glbRisk.Id, redirec, coverageId);

                            const dataSelectRiskSurety = [];
                            $('#selectRisk').children('option').each(function () {
                                if ($(this).val() == glbRisk.Id)
                                    dataSelectRiskSurety.push({ Id: $(this).val(), Description: data.result.Description })
                                else if ($(this).val())
                                    dataSelectRiskSurety.push({ Id: $(this).val(), Description: $(this).text() })
                            })
                            $('#selectRisk').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Id });
                            if (dataSelectRiskSurety.length == 1) {
                                $('#selectRisk').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Id, enable: false });
                                glbPolicy.Summary.RiskCount = $("#selectRisk").length;
                            }
                            else if (dataSelectRiskSurety.length > 1)
                                $('#selectRisk').UifSelect({ sourceData: dataSelectRiskSurety, selectedId: glbRisk.Id });

                            if (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumberOld == null) {
                                gblCurrentRiskTemporalNumberOld = gblCurrentRiskTemporalNumber;
                                gblCurrentRiskTemporalNumber = data.result.Id;
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                    });
                }
                else {
                    RiskLiability.LoadScript();
                }
            };

        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRiskPremiumAndAmountInsured, 'autoclose': true });
        }
    }


    static GetRiskDataModel() {
        var riskData = $('#formLiability').serializeObject();
        //riskData.riskId = $('#selectRisk').val();
        riskData.TemporalId = glbPolicy.Id;
        riskData.PrefixId = glbPolicy.Prefix.Id;
        riskData.ProductId = glbPolicy.Product.Id;
        riskData.PolicyTypeId = glbPolicy.PolicyType.Id;
        riskData.InsuredId = glbPolicy.Holder.IndividualId;
        riskData.HolderId = glbPolicy.Holder.IndividualId;
        riskData.EndorsementType = glbPolicy.Endorsement.EndorsementType;
        riskData.Id = glbPolicy.Id;
        riskData.RiskId = glbRisk.Id;
        if ($('#inputInsured').data('Object') != null) {
            riskData.InsuredName = ReplaceCharacter($('#inputInsured').data("Object").Name);
            riskData.HolderIdentificationDocument = $("#inputInsured").data("Object").IdentificationDocument.Number;
            if ($("#inputInsured").data("Object").IdentificationDocument != null && $("#inputInsured").data("Object").IdentificationDocument.DocumentType != null && $("#inputInsured").data("Object").IdentificationDocument.DocumentType.Id != null)
                riskData.InsuredDocumentTypeId = $("#inputInsured").data("Object").IdentificationDocument.DocumentType.Id;
            else
                if ($("#inputInsured").data("Object").IndividualType > 0)
                    riskData.InsuredDocumentTypeId = $("#inputInsured").data("Object").IndividualType;
                else
                    riskData.InsuredDocumentTypeId = 1;

            riskData.InsuredId = $('#inputInsured').data('Object').IndividualId;
            riskData.InsuredCustomerType = $('#inputInsured').data('Object').CustomerType;
            riskData.InsuredBirthDate = FormatDate($("#inputInsured").data("Object").BirthDate);
            riskData.InsuredGender = $("#inputInsured").data("Object").Gender;
            riskData.InsuredIndividualType = $("#inputInsured").data("Object").IndividualType;
            if ($("#inputInsured").data("Object").IdentificationDocument != null) {
                riskData.InsuredDocumentNumber = $("#inputInsured").data("Object").IdentificationDocument.Number;
            }
            if ($('#inputInsured').data('Object').CompanyName != null)
                if ($("#inputName").UifSelect("getSelected") != 0 && $("#inputName").UifSelect("getSelected") != null) { riskData.InsuredDetailId = $("#inputName").UifSelect("getSelected"); }
                else { riskData.InsuredDetailId = $("#inputInsured").data("Object").CompanyName.NameNum; }
            riskData.InsuredAddressId = glbRisk.AddressId;

            if ($("#inputInsured").data("Object").CompanyName.Phone != null) {
                if ($("#inputPhone").data("Object") != undefined && $("#inputPhone").data("Object") != null) {
                    riskData.InsuredPhoneId = $("#inputPhone").data("Object").Id;
                } else if (glbRisk.PhoneID != undefined) {
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
                else if ($("#inputInsured").data('Detail') != null) {
                    riskData.InsuredPhoneId = $("#inputInsured").data('Detail')[0].Phone.Id;
                }
            }
            if ($("#inputInsured").data("Object").CompanyName.Email != null) {
                riskData.InsuredEmailId = $("#inputInsured").data("Object").CompanyName.Email.Id;
            }
            if ($("#inputInsured").data("Object").AssociationType != null) {
                riskData.InsuredAssociationType = $("#inputInsured").data("Object").AssociationType.Id;
            }
        }

        if ($("#inputAmountInsured").text() != "" && $("#inputAmountInsured").text() != "0") {
            riskData.AmountInsured = NotFormatMoney($("#inputAmountInsured").text());
        }
        if ($("#inputPremium").text() != "" && $("#inputPremium").text() != "0") {
            riskData.Premium = NotFormatMoney($("#inputPremium").text());
        }
        riskData.FullAddress = ReplaceCharacter($('#inputFullAddress').val());
        riskData.CountryCode = $('#selectCountry').UifSelect('getSelected');
        riskData.DaneCode = $('#inputDaneCode').UifSelect('getSelected');
        riskData.StateCode = $('#selectState').UifSelect('getSelected');
        riskData.CityCode = $('#selectCity').UifSelect('getSelected');
        riskData.RateZoneDescription = $('#inputRateZone').val();
        riskData.RateZoneId = $('#inputRateZone').data('Id');
        riskData.RiskActivityId = $("#selectRiskActivity").UifSelect("getSelected");
        riskData.GroupCoverage = $('#selectGroupCoverage').val();
        riskData.IsDeclarative = $('#chkIsDeclarative').is(':checked');
        riskData.IsRetention = $('#chkIsRetention').is(':checked');
        ///campos adicionales
        riskData.RiskSubActivityId = $("#selectRiskSubActivity").UifSelect("getSelected");
        riskData.RiskSubActivityDescripcion = $("#selectRiskSubActivity option:selected").text();
        if ($("#selectAssuranceMode").UifSelect("getSelected") > 0) {
            riskData.AssuranceModeId = $("#selectAssuranceMode").UifSelect("getSelected");
            riskData.AssuranceModeDescripcion = $("#selectAssuranceMode option:selected").text();
        } else {
            riskData.AssuranceModeDescripcion = "";
        }
        var coveragesValues = $("#listCoverages").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = glbPolicy.CurrentFrom;
            this.CurrentTo = glbPolicy.CurrentTo;

            this.TypeCoverage = RiskLiability.ValidateTypeCoverage(this);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.LimitOccurrenceAmount = NotFormatMoney(this.LimitOccurrenceAmount);
            this.LimitClaimantAmount = NotFormatMoney(this.LimitClaimantAmount);
            this.MaxLiabilityAmount = NotFormatMoney(this.MaxLiabilityAmount);
            this.DisplayRate = NotFormatMoney(this.Rate, 2);
            this.FlatRatePorcentage = NotFormatMoney(this.FlatRatePorcentage);
            this.PreRuleSetId = NotFormatMoney(this.PreRuleSetId);
            this.ruleSetId = NotFormatMoney(this.ruleSetId);

            if (this.Rate != null || this.Rate > 0) {
                this.Rate = NotFormatMoney(this.Rate);
            }
            /*if (this.PremiumAmount != null && this.PremiumAmount != 0) {
                this.EndorsementLimitAmount = this.LimitAmount;
                this.EndorsementSublimitAmount = this.SubLimitAmount;
            }
            else {
                this.EndorsementLimitAmount = 0;
                this.EndorsementSublimitAmount = 0;
            }*/
        });

        riskData.Coverages = coveragesValues;
        ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) && gblCurrentRiskTemporalNumberOld == null) ? riskData.RiskId = 0 : riskData.RiskId = riskData.RiskId;

        return riskData;
    }

    static GetTemporalRiskDataModel() {
        var riskData = $('#formLiability').serializeObject();
        riskData.TemporalId = glbPolicy.Id;
        riskData.PrefixId = glbPolicy.Prefix.Id;
        riskData.ProductId = glbPolicy.Product.Id;
        riskData.PolicyTypeId = glbPolicy.PolicyType.Id;
        riskData.InsuredId = glbPolicy.Holder.IndividualId;
        riskData.HolderId = glbPolicy.Holder.IndividualId;
        riskData.EndorsementType = glbPolicy.Endorsement.EndorsementType;
        riskData.Id = glbRisk.Id;
        riskData.RiskId = glbRisk.Id;
        if (glbRisk.Risk.MainInsured != null) {
            riskData.InsuredName = glbRisk.Risk.MainInsured.Name;
            riskData.HolderIdentificationDocument = glbRisk.Risk.MainInsured.IdentificationDocument;
            riskData.InsuredId = glbRisk.Risk.MainInsured.IndividualId;
            riskData.InsuredCustomerType = glbRisk.Risk.MainInsured.CustomerType;
            riskData.InsuredBirthDate = FormatDate(glbRisk.Risk.MainInsured.BirthDate);
            riskData.InsuredGender = glbRisk.Risk.MainInsured.Gender;
            if (glbRisk.Risk.MainInsured.IdentificationDocument != null) {
                riskData.InsuredDocumentNumber = glbRisk.Risk.MainInsured.IdentificationDocument.Number;
            }
            if (glbRisk.Risk.MainInsured.CompanyName != null) {
                riskData.InsuredDetailId = glbRisk.Risk.MainInsured.CompanyName.NameNum;
                riskData.InsuredAddressId = glbRisk.Risk.MainInsured.CompanyName.Address.Id;
                if (glbRisk.Risk.MainInsured.CompanyName.Phone != null) {
                    riskData.InsuredPhoneId = glbRisk.Risk.MainInsured.CompanyName.Phone.Id;
                }
                if (glbRisk.Risk.MainInsured.CompanyName.Email != null) {
                    riskData.InsuredEmailId = glbRisk.Risk.MainInsured.CompanyName.Email.Id;
                }
            }
            if (glbRisk.Risk.MainInsured.IdentificationDocument != null && glbRisk.Risk.MainInsured.IdentificationDocument.DocumentType != null && glbRisk.Risk.MainInsured.IdentificationDocument.DocumentType.Id != null)
                riskData.InsuredDocumentTypeId = glbRisk.Risk.MainInsured.IdentificationDocument.DocumentType.Id;
            else
                if (glbRisk.Risk.MainInsured.IndividualType > 0)
                    riskData.InsuredDocumentTypeId = glbRisk.Risk.MainInsured.IndividualType;
                else
                    riskData.InsuredDocumentTypeId = 1;

        }
        if (glbRisk.Risk.AmountInsured != null && glbRisk.Risk.AmountInsured != 0) {
            riskData.AmountInsured = NotFormatMoney(glbRisk.Risk.AmountInsured);
        }
        if (glbRisk.Risk.Premium != null && glbRisk.Risk.PremiumS != undefined) {
            riskData.Premium = NotFormatMoney(glbRisk.Risk.Premium);
        }

        riskData.FullAddress = ReplaceCharacter(glbRisk.FullAddress);
        riskData.CountryCode = glbRisk.City.State.Country.Id;
        riskData.DaneCode = glbRisk.City.DANECode;
        riskData.StateCode = glbRisk.City.State.Id;
        riskData.CityCode = glbRisk.City.Id;
        riskData.RateZoneDescription = glbRisk.Risk.RatingZone.Description;
        riskData.RateZoneId = glbRisk.Risk.RatingZone.Id;
        if (glbSuretyRiskSubActivity != null) {
            riskData.RiskActivityId = glbSuretyRiskSubActivity;
        }
        riskData.GroupCoverage = glbRisk.GroupCoverage;
        riskData.IsDeclarative = glbRisk.IsDeclarative;
        riskData.IsRetention = glbRisk.Risk.IsRetention;
        ///campos adicionales
        if (glbRisk.RiskSubActivity != null) {
            riskData.RiskSubActivityId = glbRisk.RiskSubActivity.Id
        }
        if (glbRisk.RiskSubActivity != null) {
            riskData.RiskSubActivityDescripcion = glbRisk.RiskSubActivity.Description;
        }

        if (glbRisk.AssuranceMode > 0) {
            riskData.AssuranceModeId = glbRisk.AssuranceMode.Id;
            riskData.AssuranceModeDescripcion = glbRisk.AssuranceMode.Description;
        } else {
            riskData.AssuranceModeDescripcion = "";
        }
        var coveragesValues = glbRisk.Coverages;
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = glbPolicy.CurrentFrom;
            this.CurrentTo = glbPolicy.CurrentTo;

            this.TypeCoverage = RiskLiability.ValidateTypeCoverage(this);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.LimitOccurrenceAmount = NotFormatMoney(this.LimitOccurrenceAmount);
            this.LimitClaimantAmount = NotFormatMoney(this.LimitClaimantAmount);
            this.MaxLiabilityAmount = NotFormatMoney(this.MaxLiabilityAmount);
            this.DisplayRate = NotFormatMoney(this.Rate, 2);
            this.FlatRatePorcentage = NotFormatMoney(this.FlatRatePorcentage);
            this.PreRuleSetId = NotFormatMoney(this.PreRuleSetId);
            this.ruleSetId = NotFormatMoney(this.ruleSetId);

            if (this.Rate != null || this.Rate > 0) {
                this.Rate = NotFormatMoney(this.Rate);
            }
        });

        riskData.Coverages = coveragesValues;
        ((gblCurrentRiskTemporalNumber != undefined && gblCurrentRiskTemporalNumber != null) && gblCurrentRiskTemporalNumberOld == null) ? riskData.RiskId = 0 : riskData.RiskId = riskData.RiskId;

        return riskData;
    }

    static ShowSaveRisk(riskId, redirec, coverageId) {
        if (redirec != 9) {
            if ($("#selectRisk").UifSelect("getSelected") == null || $("#selectRisk").UifSelect("getSelected") == 0) {
                RiskLiability.GetRisksByTemporalId(glbPolicy.Id, riskId);
            }
        }

        //lanza los eventos para la creación de el riesgo
        let events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);

        if (events !== TypeAuthorizationPolicies.Restrictive) {
            RiskLiability.RedirectAction(redirec, riskId, coverageId);
        }
        //fin - lanza los eventos para la creación de el riesgo
    }

    static RedirectAction(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskLiability.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.Coverage:
                RiskLiability.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskLiability.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.Texts:
                RiskLiability.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskLiability.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Script:
                RiskLiability.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
        }
    }

    static ReturnUnderwriting() {
        var url = '';

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "LiabilityModification";
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

    static ReturnCoverage(coverageId) {
        glbCoverage = {
            CoverageId: coverageId,
            Object: "RiskLiabilityCoverage",
            Class: RiskLiabilityCoverage

        }
        router.run("prtCoverageRiskLiability");
    }

    static DeleteRisk() {
        glbRisk.Id = 0;
        if ($('#selectRisk').val() > 0) {
            RiskLiabilityRequest.DeleteRisk(glbPolicy.Id, $('#selectRisk').val()).done(function (data) {
                if (data.success) {
                    if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                        RiskLiability.ClearControls();
                        RiskLiability.GetRisksByTemporalId(glbPolicy.Id, 0);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                        $('#selectRisk').UifSelect("setSelected", null);
                        ScrollTop();
                    }
                    else {
                        RiskLiability.GetRisksByTemporalId(glbPolicy.Id, $('#selectRisk').UifSelect('getSelected'));
                        RiskLiability.GetRiskById($('#selectRisk').UifSelect('getSelected'));
                        RiskLiability.UpdatePremiunAndAmountInsured();
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteRisk, 'autoclose': true });
            });
        }
    }

    static LoadSubTitles(subTitle) {
        if (subTitle == 0 || subTitle == 1) {
            if (glbRisk.Beneficiaries != null) {
                if (glbRisk.Beneficiaries.length > 0) {
                    $('#selectedBeneficiaries').text('(' + glbRisk.Beneficiaries.length + ')');
                }
                else {
                    $('#selectedBeneficiaries').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedBeneficiaries').text('(' + AppResources.LabelWithoutData + ')');
            }
        }

        if (subTitle == 0 || subTitle == 2) {
            if (glbRisk.Text != null) {
                if (glbRisk.Text.TextBody == null) {
                    glbRisk.Text.TextBody = '';
                }

                if (glbRisk.Text.TextBody.length > 0) {
                    $('#selectedTexts').text('(' + AppResources.LabelWithText + ')');
                }
                else {
                    $('#selectedTexts').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedTexts').text('(' + AppResources.LabelWithoutData + ')');
            }
        }

        if (subTitle == 0 || subTitle == 3) {
            if (glbRisk.Clauses != null) {
                if (glbRisk.Clauses.length > 0) {
                    $('#selectedClauses').text('(' + glbRisk.Clauses.length + ')');
                }
                else {
                    $('#selectedClauses').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedClauses').text('(' + AppResources.LabelWithoutData + ')');
            }
        }
    }

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static DeleteCoverage(data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else {
            var coverages = $('#listCoverages').UifListView('getData');
            if (coverages != null && coverages != "" && coverages.length == 1 && data.EndorsementType == EndorsementType.Modification) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
            }
            else {
                $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages, height: 340 });

                if (data.EndorsementType == EndorsementType.Modification) {
                    var coverage = RiskLiability.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);
                    $.when(coverage).done(function (coverageData) {
                        coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount);
                        coverageData.Rate = FormatMoney(coverageData.Rate);
                        coverageData.LimitAmount = FormatMoney(coverageData.LimitAmount);
                        $.each(coverages, function (index, value) {
                            if (this.Id == coverageData.Id) {
                                coverages[index] = coverageData
                            }
                        });
                        $("#listCoverages").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                    });
                }
                else {
                    $.each(coverages, function (index, value) {
                        if (this.Id != data.Id) {
                            $("#listCoverages").UifListView("addItem", this);
                        }
                    });

                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                }
                RiskLiability.UpdatePremiunAndAmountInsured();
            }
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskLiabilityRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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

    static UpdatePremiunAndAmountInsured() {
        var coverages = $('#listCoverages').UifListView('getData');
        var totalSumInsured = 0;
        var totalPremium = 0;
        $.each(coverages, function (index, value) {
            totalPremium = totalPremium + parseFloat(NotFormatMoney(coverages[index].PremiumAmount));
            totalSumInsured = totalSumInsured + parseFloat(NotFormatMoney(coverages[index].EndorsementLimitAmount));

        });
        $('#inputPremium').text(FormatMoney(totalPremium));
        $('#inputAmountInsured').text(FormatMoney(totalSumInsured));
    }

    static LoadScript(cover) {
        if (glbRisk.Id == 0) {
            cover = (cover == undefined) ? 0 : cover;
            if (gblCurrentRiskTemporalNumber != null && gblCurrentRiskTemporalNumber != undefined) {
                RiskLiability.SaveTemporalRiskSearch(MenuType.Script, cover);
            }
            else {
                RiskLiability.SaveRisk(MenuType.Script, cover);
            }
        }

        if (glbRisk.Id > 0 && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, glbRisk.Class, dynamicProperties);
        }
    }

    static UpdateGlbRisk(data) {
        var recordScript = glbRisk.RecordScript;
        glbRisk = data;
        $.extend(glbRisk, data.Risk);

        glbRisk.Object = "RiskLiability";
        glbRisk.formRisk = "#formLiability";
        glbRisk.RecordScript = recordScript;
        glbRisk.Class = RiskLiability;
    }

    static RunRules(ruleSetId) {
        RiskLiabilityRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (glbPolicy.Endorsement.EndorsementType == 2) {
                if (glbRisk.Coverages != undefined && glbRisk.Coverages.length > 0) {
                    data.result.Risk.Coverages = glbRisk.Coverages;
                    data.result.Risk.Premium = 0;
                    glbRisk.Coverages.forEach(function (item) {
                        data.result.Risk.Premium = data.result.Risk.Premium + parseInt(item.PremiumAmount);
                    });
                }
            }
            RiskLiability.LoadRisk(data.result);
        }).fail(function () {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunRules, 'autoclose': true });
        });
    }

    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbRisk.RecordScript = isModify;
        if (isModify) {
            RiskLiability.GetPremium();
        }
    }

    static GetPremium() {
        $("#formLiability").validate();

        if ($("#formLiability").valid()) {
            var riskData = RiskLiability.GetRiskDataModel();

            /*$.ajax({
                type: "POST",
                url: rootPath + 'Underwriting/RiskLiability/GetPremium',
                data: JSON.stringify({ riskModel: riskData, coverages: riskData.Coverages, dynamicProperties: dynamicProperties }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            })*/
            RiskLiabilityRequest.GetPremium(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (glbPolicy.Endorsement.EndorsementType == 2) {
                            if (glbRisk.Coverages != undefined && glbRisk.Coverages.length > 0) {
                                data.result.Risk.Coverages = glbRisk.Coverages;
                                data.result.Risk.Premium = 0;
                                glbRisk.Coverages.forEach(function (item) {
                                    data.result.Risk.Premium = data.result.Risk.Premium + parseInt(item.PremiumAmount);
                                });
                            }
                        }
                        RiskLiability.LoadRisk(data.result);
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
    static GetDetailsByIndividualId(individualId, customerType) {
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                if (individualSearchType == 2) {
                    $("#inputBeneficiaryName").data('Detail', RiskLiability.GetIndividualDetails(data.result));
                }
                else {
                    $("#inputInsured").data('Detail', RiskLiability.GetIndividualDetails(data.result));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        });
    }
    static LoadInsured(insured) {
        if (individualSearchType == 1) {
            $("#inputInsured").data("Object", insured);
            if ($("#inputInsured").data("Object").IndividualType == IndividualType) //$("#inputInsured").data("Object").IndividualType = IndividualType;
            {
                var description = $("#inputInsured").data("Object").IdentificationDocument.Number;
                var insuredSearchType = 1;
                var customerType = null;
                UnderwritingRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                    if (data.success) {
                        if (data.result != undefined) {
                            $("#inputInsured").data("Object").IndividualType = data.result[0].IndividualType;
                        }
                    }
                });
            }
            $("#inputInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            $("#inputFullAddress").val(insured.CompanyName.Address.Description);

            if (insured.CompanyName.Address.City != null && insured.CompanyName.Address.City.Id != 0) {
                if (insured.CompanyName.Address.City.State.Id != 0 && insured.CompanyName.Address.City.State.Country.Id != 0) {
                    RiskLiabilityRequest.GetCitiesByCountryIdStateId(insured.CompanyName.Address.City.State.Country.Id, insured.CompanyName.Address.City.State.Id).done(function (data) {
                        if (data.success) {
                            $("#selectCity").UifSelect({ sourceData: data.result, selectedId: insured.CompanyName.Address.City.Id });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }

                    });
                }
                if (insured.CompanyName.Address.City.State != null && insured.CompanyName.Address.City.State.Id != 0) {
                    $("#selectState").UifSelect("setSelected", insured.CompanyName.Address.City.State.Id);
                    if (insured.CompanyName.Address.City.State.Country != null && insured.CompanyName.Address.City.State.Country.Id != 0) {
                        $("#selectCountry").UifSelect("setSelected", insured.CompanyName.Address.City.State.Country.Id);
                    }
                }
            }

            $("#inputInsured").data("Object").IndividualType = insured.IndividualType;
            if (insured.CustomerType == CustomerType.Individual) {
                RiskLiability.GetDetailsByIndividualId(insured.IndividualId, insured.CustomerType);
                //$("#inputInsured").data("Detail", RiskLiability.GetIndividualDetails(insured.CompanyName));
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
                RiskLiability.GetDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
                //$("#inputBeneficiaryName").data("Detail", RiskLiability.GetIndividualDetails(insured.CompanyName));
                $("#btnConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        RiskJudicialSurety.getReasonSocialIndividualId(insured.IndividualId, 1);

    }
    static LoadViewModel(viewModel) {
        if (viewModel.FullAddress != null) {
            $('#inputFullAddress').val(decodeURIComponent(viewModel.FullAddress));
        }
        var city = { Id: viewModel.CityCode, DANECode: viewModel.DaneCode, State: { Id: viewModel.StateCode, Country: { Id: viewModel.CountryCode } } };
        RiskLiability.GetCountriesLiability(city);

        RiskLiability.GetRiskActivities(viewModel.RiskActivityId);
        $('#chkIsDeclarative').prop('checked', viewModel.IsDeclarative);

        $('#chkIsRetention').prop('checked', viewModel.IsRetention);
        RiskLiability.GetRiskSubActivities(viewModel.RiskActivityId);
        if (viewModel.Coverages != "") {
            $.each(viewModel.Coverages, function (key, value) {
                this.LimitAmount = parseFloat((this.LimitAmount).replace(',', '.'));
                this.SubLimitAmount = parseFloat((this.SubLimitAmount).replace(',', '.'));
                this.PremiumAmount = parseFloat((this.PremiumAmount).replace(',', '.'));
            });
            RiskLiability.LoadCoverages(viewModel.Coverages);
        }
        else if (viewModel.GroupCoverage != "") {
            RiskLiability.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, viewModel.GroupCoverage.Id);
        }
        RiskLiability.LoadSubTitles(0);
    }

    static GetRiskActivities(selectedId) {
        var controller = "";
        RiskLiabilityRequest.GetRiskActivitiesByProductId(glbPolicy.Product.Id).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $("#selectRiskActivity").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectRiskActivity").UifSelect({ sourceData: controller, selectedId: selectedId });
                }
                RiskLiability.ValidarEnabledCampos();
            }
        });
    }

    static GetRiskSubActivities(ActivityId, selectedId) {
        var controller = "";
        RiskLiabilityRequest.GetSubActivityRisksByActivityRiskId(ActivityId).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    glbSuretyRiskSubActivity = controller;
                    $("#selectRiskSubActivity").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectRiskSubActivity").UifSelect({ sourceData: controller, selectedId: selectedId });
                }
            }
        });

    }
    static GetGroupCoverages(productId, selectedId) {
        RiskLiabilityRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }


    static GetSubActivityRisksByActivityRiskId(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskLiabilityRequest.GetSubActivityRisksByActivityRiskId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        $("#selectRiskSubActivity").UifSelect({ sourceData: data.result });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }


    static GetAssuranceMode(selectedId) {
        RiskLiabilityRequest.GetAssuranceMode().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectAssuranceMode").UifSelect({ sourceData: data.result });

                } else {
                    $("#selectAssuranceMode").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ValidateAddress() {
        RiskLiabilityRequest.GetNomenclaturesAll().done(function (data) {
            if (data.success) {
                var nomenclaturesArray = data.result;

                var fullAddressResult = "";
                var fullAddresInit = $('#inputFullAddress').val().split(' ')

                $.each(fullAddresInit, function (index, val) {
                    if (isNaN(val)) {
                        var index = "";
                        $.each(nomenclaturesArray, function (i, value) {
                            if (val == value.Nomenclatura) {
                                index = i;
                            }
                        })
                        if (index != "") {
                            fullAddressResult += nomenclaturesArray[index].Abreviatura + " ";
                        }
                        else {
                            fullAddressResult += val + " ";
                        }
                    }
                    else {
                        fullAddressResult += val + " ";
                    }

                });
                $('#inputFullAddress').UifAutoComplete('setValue', fullAddressResult);

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }
    static validateLiability() {
        //validaciones

        $("#inputFullAddress").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    }

    static SelectIndividual(e) {
        RiskLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        $('#modalIndividualSearch').UifModal("hide");
    }

    static ChangeIsRetention() {
        if ($("#chkIsDeclarative").prop("checked")) {
            $('#chkIsRetention').prop("disabled", true);
        } else {
            $("#chkIsRetention").prop("disabled", false);
        }
    }

    static ChangeIsFacultative() {
        if ($("#chkIsRetention").prop("checked")) {
            $('#chkIsDeclarative').prop("disabled", true);
        } else {
            $("#chkIsDeclarative").prop("disabled", false);
        }
    }
   

    static UpdatePolicyComponents(redir) {
        RiskLiabilityRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
              
                if (redir) {
                    RiskLiability.ReturnUnderwriting();
                }
            } else {
                $.UifNotify("show", { "type": "danger", "message": AppResources.ErrorSaveTemporary, 'autoclose': true });
            }
        });
    }
    static getReasonSocialIndividualId(individualId, Namenum) {
        $("#inputName").val("");
        $("#inputAdress").val("");
        $("#inputPhone").val("");

        if (individualId != null && individualId > 0)
            UnderwritingRequest.GetCompanyBusinessByIndividualId(individualId).done(function (data) {

                if (data.success) {
                    if (data.result.length > 0) {
                        $('#reasonSocialWrapper').css("display", "block");
                        var TradeName = data.result[0].TradeName;
                        var Direccion = data.result[0].AddressID;
                        var Telefono = data.result[0].PhoneID;
                        $("#inputName").UifSelect({ sourceData: data.result, selectedId: Namenum });
                        $("#inputAdress").val(Direccion);
                        $("#inputPhone").val(Telefono);

                        RiskLiabilityRequest.GetIndividualDetailsByIndividualId(individualId, 1).done(function (elemento) {
                            if (elemento.success) {
                                if (elemento.result.length > 0) {
                                    var companyReasonSocial = elemento.result.find(function (item) {
                                        return item.NameNum == Namenum
                                    });
                                    if (companyReasonSocial.Address != null && companyReasonSocial.Address.Description != null) {
                                        $("#inputAdress").val(companyReasonSocial.Address.Description);
                                    }
                                    if (companyReasonSocial.Phone != null && companyReasonSocial.Phone.Description) {
                                        $("#inputPhone").val(companyReasonSocial.Phone.Description);
                                    }
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
            var individualIdN = $("#inputInsured").data("Object").IndividualId;
            UnderwritingRequest.GetDetailsByIndividualId(individualIdN, 1).done(function (elemento) {
                if (elemento.success) {
                    if (elemento.result.length > 0) {
                        elemento.result.forEach(function (item) {
                            if (item.NameNum == selectedItem.Id && band == 0) {
                                band = 1;

                                if (item.Address != null && item.Address.Description != null)
                                    $("#inputAdress").val(item.Address.Description);
                                if (item.Address.Id != null) { glbRisk.AddressId = item.Address.Id }
                                if (item.Phone.Id != null) { glbRisk.PhoneID = item.Phone.Id }
                                if (item.Phone != null && item.Phone.Description) {
                                    $("#inputPhone").data("Object", item.Phone);
                                    $("#inputPhone").val(item.Phone.Description);
                                }
                            }
                        });

                    }

                }
            }).fail(function (jqXHR, textStatus, errorThrown) {

            });
        }
    }

    static ValidarEnabledCampos() {
        if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) != EndorsementType.Emission && parseInt(glbPolicy.Endorsement.EndorsementType, 10) != EndorsementType.Renewal && !addRiskControl) {
            $("#selectCountry").UifSelect("disabled", true);
            $("#selectState").UifSelect("disabled", true);
            $("#selectCity").UifSelect("disabled", true);
            $("#selectAssuranceMode").UifSelect("disabled", true);
            $("#selectRiskSubActivity").UifSelect("disabled", true);
            $("#selectRiskActivity").UifSelect("disabled", true);
            $("#inputFullAddress").prop('disabled', true);
            $("#inputDaneCode").prop('disabled', true);
            $("#selectGroupCoverage").UifSelect("disabled", true);

        }
        else {
            $('#inputInsured').prop('disabled', false);
            $("#inputFullAddress").prop('disabled', false);
            $("#inputDaneCode").prop('disabled', false);
            if (document.getElementById("selectState") != null)
                $("#selectState").UifSelect("disabled", false);
            if (document.getElementById("selectCity") != null)
                $("#selectCity").UifSelect("disabled", false);
            if (document.getElementById("selectAssuranceMode") != null)
                $("#selectAssuranceMode").UifSelect("disabled", false);
            if (document.getElementById("selectCountry") != null)
                $("#selectCountry").UifSelect("disabled", false);
        }

    }

    static QuotationRisk() {

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            var coverages = $("#listCoverages").UifListView('getData');
            if (coverages != null && coverages.length > 0) {
                coverages = RiskSurety.UnformatCoverage(coverages);
                RiskLiabilityCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, coverages).done(function (data) {
                });
            }
        }
    }
}