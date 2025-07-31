var countryParameterProperty = 1;
var dynamicProperties = null;
var Height = 420;
var individualSearchType = 1;
var IndividualType;
var IsEditing = false;
var disableModificationEndorsement = true;

class Property extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        riskController = 'RiskProperty'
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "Property", formRisk: "#formProperty", RecordScript: false, Class: Property };
        }

        UnderwritingQuotation.DisabledButtonsQuotation();
        Property.validateProperty();
        $.UifProgress('show');
        Property.InitializeProperty();
        $.UifProgress('close');

    }
    static InitializeProperty() {
        $("#listInsuredObjects").UifListView({
            localMode: true,
            add: true, edit: true, delete: true,
            displayTemplate: "#insuredObjectDisplayTemplate",
            customAdd: true, customEdit: true, title: AppResources.LabelInsuredObject
        });

        Property.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
    }
    static initializeData() {
        $("#SeismicZone").attr("disabled", true);
        $("#panelAddressNomenclature").hide();
        Property.GetCountriesProperty(null);
        Property.GetRiskActivities(0);
        Property.GetRiskType(0);
        Property.GetGroupCoverages(glbPolicy.Product.Id, 0);
        Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
    }
    bindEvents() {
        //eventos       
        $("#withOutNomenclature").on('click', this.withOutNomenclatureClick);
        $("#withNomenclature").on('click', this.withNomenclatureClick);
        $("#inputInsured").on('buttonClick', this.inputInsuredClick);
        $('#tableIndividualResults tbody').on('click', 'tr', this.tableIndividualResultsClick);
        $(".Nomenclature").focusout(this.NomenclatureFocusOut);
        $('#inputFullAddress').focusout(Property.ValidateAddress);
        $('#selectCountry').on('itemSelected', this.selectCountryItemSelected);
        $('#selectState').on('itemSelected', this.selectStateItemSelected);
        $('#selectCity').on('itemSelected', this.selectCityItemSelected);
        $('#selectGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $('#selectRisk').on('itemSelected', this.selectRiskItemSelected);
        $('#selectRiskActivity').on('itemSelected', Property.GetSubActivityRisksByActivityRiskId);
        $("#inputDaneCode").on('itemSelected focusout', this.inputDaneCodeItemSelected);

        $('#listInsuredObjects').on('rowAdd', this.listInsuredObjectsRowwAdd);
        $('#listInsuredObjects').on('rowEdit', this.listInsuredObjectsRowEdit);
        $('#listInsuredObjects').on('rowDelete', this.listInsuredObjectsRowDelete);

        $("#btnDetail").on('click', Property.ShowInsuredDetail);
        $("#btnAccept").on('click', this.btnAcceptClick);
        $("#btnClose").on('click', Property.UpdatePolicyComponents);
        $("#btnAddRisk").on('click', this.btnAddRiskClick);
        $("#btnDeleteRisk").on('click', Property.DeleteRisk);
        $("#btnIndividualDetailAccept").on('click', Property.SetIndividualDetail);
        $("#btnScript").on('click', Property.LoadScript);
        $("#btnAcceptNewPersonOnline").click(this.btnAcceptNewPersonOnlineClick);
        $('#btnConvertProspect').click(this.btnConvertProspectClick);
        $("#chkIsFacultative").on("change", Property.ChangeIsRetention);
        $("#chkIsRetention").on("change", Property.ChangeiIsFacultative);
    }

    selectRiskItemSelected(event, selectItem) {
        if (selectItem.Id > 0) {
            var riskId = $('#selectRisk').UifSelect("getSelected");
            $('#formProperty').get(0).reset();
            Property.GetRiskById(glbPolicy.Id, riskId);
        }
        else {
            Property.ClearControls();
            Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        }
    }

    btnConvertProspectClick() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: Property.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsured").data("Object").IndividualId, $("#inputInsured").data("Object").IndividualType, $("#inputInsured").data("Object").CustomerType, 0);
    }

    btnAcceptNewPersonOnlineClick() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: Property.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsured").val().trim());
    }

    btnAddRiskClick() {
        Property.ClearControls();
        Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        Property.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    btnAcceptClick() {
        $("#formProperty").validate();
        $.UifProgress('show');
        var insuredObjects = $('#listInsuredObjects').UifListView("getData");
        if ($("#formProperty").valid()) {
            if (insuredObjects.length > 0) {
                Property.SaveRisk(MenuType.Risk, 0);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.RiskWithoutInsurancedObjects, 'autoclose': true });
            }
        }
        ScrollTop();
        $.UifProgress('close');
    }

    listInsuredObjectsRowDelete(event, data) {
        if ($('#listInsuredObjects').UifListView("getData") != null && $('#listInsuredObjects').UifListView("getData").length > 1) {
            Property.DeleteInsuredObject(data);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInsuredObject, 'autoclose': true });
        }
    }

    listInsuredObjectsRowEdit(event, data, index) {
        IsEditing = true;
        Property.SaveRisk(MenuType.InsuredObject, data.Id);
    }

    listInsuredObjectsRowwAdd() {
        IsEditing = false;
        Property.SaveRisk(MenuType.InsuredObject, 0)
    }

    ChangeGroupCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            //-----------
            if (glbRisk.Risk == undefined || glbRisk.Risk == null) {
                lockScreen();
                Property.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id, true);
            } else if (glbRisk.Risk.GroupCoverage == undefined || glbRisk.Risk.GroupCoverage == null) {
                lockScreen();
                Property.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id, true);
            } else {
                if (glbRisk.groupCoverageId != selectedItem.Id) {
                    $.UifDialog('confirm', { 'message': AppResources.ChangeGroupCoverages }, function (result) {
                        if (result) {
                            lockScreen();
                            Property.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id, true);
                        } else {
                            lockScreen();
                            Property.GetGroupCoverages(glbPolicy.Product.Id, glbRisk.Risk.GroupCoverage.Id);
                        }
                    });
                }
            }
        }
        else {
            $("#listInsuredObjects").UifListView("refresh");
        }
        $('#inputPremium').text(0);
        $("#inputAmountInsured").text(0);
        
    }
    selectCity(event, selectedItem) {
        if (selectedItem.Id > 0 && $('#selectCountry').UifSelect('getSelected') == countryParameterProperty) {
            Property.getDaneCode($('#selectCountry').UifSelect('getSelected'), $('#selectState').UifSelect('getSelected'), selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#inputDaneCode').UifAutoComplete('setValue', data.result);
                }
            });
        }
        else {
            $('#inputDaneCode').UifAutoComplete('setValue', '');
        }
    }

    selectCityItemSelected(e) {
        var cityId = $("#selectCity").UifSelect("getSelected");
        var countryId = $("#selectCountry").UifSelect("getSelected");
        var stateId = $('#selectState').UifSelect("getSelected");
        if (cityId != null && cityId > 0 && countryId != null && countryId > 0) {
            //Validacion incluida si el Dane solo aplica para Colombia
            if (countryId == CountryEnumType.Colombia) {
                $("#inputDaneCode").UifAutoComplete('disabled', false);
                Property.getDaneCode(countryId, stateId, cityId);
            } else {
                $("#inputDaneCode").UifAutoComplete('disabled', true);
            }
        }
    }

    selectStateItemSelected(e) {
        if ($("#selectCountry").UifSelect("getSelected") != null && $("#selectState").UifSelect("getSelected") != null && $("#selectState").UifSelect("getSelected") != "") {
            PropertyRequest.GetCitiesByCountryIdStateId($("#selectCountry").UifSelect("getSelected"), $("#selectState").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $('#inputDaneCode').val("");
                        if (glbRisk.City != null) {
                            $("#selectCity").UifSelect({ sourceData: data.result, selectedId: glbRisk.City.Id });
                            Property.GetRatingZone(glbRisk.City.State.Country.Id, glbRisk.City.State.Id);
                        } else {
                            $("#selectCity").UifSelect({ sourceData: data.result });
                            Property.GetRatingZone($("#selectCountry").val(), $('#selectState').val());
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
            });
        }
    }

    inputDaneCodeItemSelected(e) {
        var inputDaneCode = $("#inputDaneCode").val();
        if (inputDaneCode != null && inputDaneCode != "") {
            if ($("#selectCountry").UifSelect("getSelected") != null && $("#selectCountry").UifSelect("getSelected") != "") {
                PropertyRequest.inputDaneCodeItemSelectedFocusout(inputDaneCode, $('#selectCountry').UifSelect("getSelected")).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            var cityId = data.result.Id;
                            var stateId = data.result.State.Id;

                            PropertyRequest.GetStatesByCountryId($("#selectCountry").UifSelect("getSelected")).done(function (data) {
                                if (data.success) {
                                    $("#selectState").UifSelect({ sourceData: data.result, selectedId: stateId });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                }
                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                            });

                            PropertyRequest.GetCitiesByCountryIdStateId($("#selectCountry").UifSelect("getSelected"), data.result.State.Id).done(function (data) {
                                if (data.success) {
                                    $("#selectCity").UifSelect({ sourceData: data.result, selectedId: cityId });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                }
                                $('#inputDaneCode').UifAutoComplete('setValue', inputDaneCode);
                                Property.GetRatingZone(data.result[0].State.Country.Id, data.result[0].State.Id);
                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                            });
                        }
                    }
                });
            } else {
                $('#inputDaneCode').UifAutoComplete('setValue', "");
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDocumentControlCountry, 'autoclose': true });
            }
        }
    }

    selectCountryItemSelected(event, itemSelected) {
        var dataParameter;
        if ($("#selectCountry").UifSelect("getSelected") == 1) {
            $('#inputDaneCode').attr('disabled', false);
            $('#selectCity').attr('disabled', false);
            $('#selectState').attr('disabled', false);
            dataParameter = 1;

        }
        else {
            $('#inputDaneCode').attr('disabled', true);
            $('#selectCity').attr('disabled', true);
            $("#selectState").val("");
            $('#inputDaneCode').val("");
            dataParameter = $(this).val();
        }

        PropertyRequest.SelectCountryItemSelected(dataParameter).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (glbRisk.City != null) {
                        $("#selectState").UifSelect({ sourceData: data.result, selectedId: glbRisk.City.State.Id });
                    } else {
                        $("#selectState").UifSelect({ sourceData: data.result });
                        $("#selectCity").UifSelect({ sourceData: null });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingDepartments, 'autoclose': true });
        });
    }

    tableIndividualResultsClick(e) {
        if (individualSearchType == 2) {
            RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
        }
        else {
            Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        }
        $('#modalIndividualSearch').UifModal("hide");
    }

    NomenclatureFocusOut() {
        $("#inputFullAddress").val(Property.GetNomenclature());
    }

    withOutNomenclatureClick() {
        $("#panelAddressNomenclature").hide();
        Property.ClearNomenclatureControl();
        $("#inputFullAddress").attr("disabled", false);
    }

    withNomenclatureClick() {
        Property.GetRouteTypes(0);
        Property.GetApartmentsOrOffices(0);
        Property.GetSuffixes1(0);
        Property.GetSuffixes2(0);
        $("#panelAddressNomenclature").show();
        $("#inputFullAddress").attr("disabled", true);
    }

    inputInsuredClick() {
        if ($("#inputInsured").val() != null && $("#inputInsured").val().trim() != null && $("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);
        }
    }

    static validateProperty() {
        //validaciones
        $("#btnConvertProspect").hide();
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [0, 1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputInsured").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputFullAddress").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    }

    static loadPersonOnline() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
                    if (glbPersonOnline.CustomerType == CustomerType.Individual) {
                        Underwriting.ConvertProspectToInsured(glbPersonOnline.IndividualId, glbPersonOnline.DocumentNumber);
                    }
                }
                else {
                    $("#inputInsured").data("Object", null);
                    $("#inputInsured").data("Detail", null);
                    if (glbPersonOnline.DocumentNumber != null && glbPersonOnline.DocumentNumber.length > 0) {
                        $("#inputInsured").val(glbPersonOnline.DocumentNumber);
                    }
                    else {
                        $("#inputInsured").val(glbPersonOnline.Name);
                    }
                }
                if (glbRisk.Id > 0) {
                    Property.GetRiskById(glbPolicy.Id, glbRisk.Id);
                }
                else {
                    Property.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }
    static GetGroupCoverages(productId, selectedId) {
        if (productId != null && productId != "") {
            PropertyRequest.GetGroupCoverages(productId).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectGroupCoverage").UifSelect({ sourceData: data.result, enable: disableModificationEndorsement });
                        }
                        else {
                            $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId, enable: disableModificationEndorsement });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryCoverageGroups, 'autoclose': true });
            });
        }
    }
    static GetRiskType(selectedId) {
        //if (selectedId == 0) {
        PropertyRequest.GetRiskTypes(selectedId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == 0) {
                        $("#selectRiskTypeLocation").UifSelect({ sourceData: data.result, enable: disableModificationEndorsement });
                    }
                    else {
                        $("#selectRiskTypeLocation").UifSelect({ sourceData: data.result, selectedId: selectedId, enable: disableModificationEndorsement });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorType, 'autoclose': true });
        });
        //} else {
        //    $("#selectRiskTypeLocation").UifSelect('setSelected', selectedId);
        //}
    }

    static GetNomenclature() {
        var address = "";
        if ($("#selectRouteType").val() != "" && $("#selectRouteType").val() != null) {
            address += $("#selectRouteType option:selected").text() + " ";
        }
        if ($("#inputName").val() != "") {
            address += $("#inputName").val();
        }
        if ($("#inputLetter").val() != "") {
            address += " " + $("#inputLetter").val();
        }
        if ($("#selectSuffix1").val() != "" && $("#selectSuffix1").val() != null) {
            address += " " + $("#selectSuffix1 option:selected").text();
        }
        if ($("#inputNro1").val() != "") {
            address += ' ' + AppResources.LabelNro + ' ' + $("#inputNro1").val();
        }
        if ($("#inputLetter2").val() != "") {
            address += $("#inputLetter2").val();
        }
        if ($("#inputNro2").val() != "") {
            address += " - " + $("#inputNro2").val();
        }
        if ($("#selectSuffix2").val() != "" && $("#selectSuffix2").val() != null) {
            address += " " + $("#selectSuffix2 option:selected").text();
        }
        if ($("#selectAparmentOrOffice").val() != "" && $("#selectAparmentOrOffice").val() != null) {
            address += " " + $("#selectAparmentOrOffice option:selected").text();
        }
        if ($("#inputNro3").val() != "") {
            address += ' ' + AppResources.LabelNro + ' ' + $("#inputNro3").val();
        }

        return address;
    }
    static ClearNomenclatureControl() {
        $("#selectRouteType").val("");
        $("#inputName").val("");
        $("#inputLetter").val("");
        $("#selectSuffix1").val("");
        $("#inputNro1").val("");
        $("#inputLetter2").val("");
        $("#inputNro2").val("");
        $("#selectSuffix2").val();
        $("#selectAparmentOrOffice").val("");
        $("#inputNro3").val("");
        $("#inputLetter2").val("");
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
    static GetRiskById(temporalId, id) {
        if (id != null && id != "") {
            PropertyRequest.GetRiskById(temporalId, id).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.Risk != null) {
                        glbRisk.RecordScript = true;
                        Property.LoadRisk(data.result);
                        glbRisk.Clauses = data.result.Risk.Clauses;
                        glbRisk.Beneficiaries = data.result.Risk.Beneficiaries;
                        glbRisk.Text = data.result.Text;
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
            });
        }
    }
    static GetRiskSubActivities(ActivityId, selectedId) {
        var controller = "";
        PropertyRequest.GetSubActivityRisksByActivityRiskId(ActivityId).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $("#selectRiskSubActivity").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectRiskSubActivity").UifSelect({ sourceData: controller, selectedId: selectedId, enable: disableModificationEndorsement });
                }
            }
        });

    }
    static LoadRisk(riskData) {
        if (riskData != null) {
            if (glbRisk.DeclarationPeriodCode != null && glbRisk.DeclarationPeriodCode > 0) {
                var declaration = glbRisk.DeclarationPeriodCode;
            }
            if (glbRisk.BillingPeriodDepositPremium != null && glbRisk.BillingPeriodDepositPremium > 0) {
                var adjust = glbRisk.BillingPeriodDepositPremium;
            }
            Property.UpdateGlbRisk(riskData);
            //Si es estado no modificado es false...
            //si voy y edito un objeto del seguro el estado cambia a modificado, sin embargo tampoco me debe dejar editar campos
            if (glbRisk.Risk.Status == 6 || glbRisk.Risk.Status == 5) {
                disableModificationEndorsement = false;
            } else {
                disableModificationEndorsement = true;
            }
            if (declaration != undefined && declaration > 0) {
                glbRisk.DeclarationPeriodCode = declaration;
            }
            if (adjust != undefined && adjust > 0) {
                glbRisk.BillingPeriodDepositPremium = adjust;
            }
            if (riskData.Risk.Id > 0) {
                if (typeof ($('#selectRisk').val()) != "undefined" && $('#selectRisk').val() != null) { }
                $("#selectRisk").UifSelect("setSelected", riskData.Risk.Id);
            }
            if (riskData.City != null && riskData.City.DANECode != "") {
                $("#inputDaneCode").val(riskData.City.DANECode)
            }
            if (riskData.Risk.MainInsured != null) {
                if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                    $("#btnConvertProspect").hide();
                } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                    $("#btnConvertProspect").show();
                }
            }
            if (riskData.HasNomenclature) {
                $('#withNomenclature').attr('checked', true);
                $('#withOutNomenclature').attr('checked', false);
                $("#panelAddressNomenclature").show();
                $("#inputFullAddress").attr("disabled", false);
            }
            else {
                $("#inputFullAddress").attr("disabled", false);
                $("#panelAddressNomenclature").hide();
                $('#withNomenclature').attr('checked', false);
                $('#withOutNomenclature').attr('checked', true);
            }
            if (riskData.IndividualId > 0) {
                Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(riskData.IndividualId, 2, riskData.CustomerType);
            }

            if (riskData.Risk.MainInsured != null) {
                if (riskData.Risk.MainInsured !== null && riskData.Risk.MainInsured !== undefined) {
                    $("#inputInsured").data("Object", riskData.Risk.MainInsured);
                    var individualDetails = Property.GetIndividualDetailsByIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CustomerType)
                    $("#inputInsured").data("Detail", Property.GetIndividualDetails(individualDetails));
                    $("#inputInsured").val(decodeURIComponent(riskData.Risk.MainInsured.Name + ' (' + riskData.Risk.MainInsured.IdentificationDocument.Number + ')'));
                }
            }
            if (riskData.NomenclatureAddress != null) {
                if (riskData.NomenclatureAddress.Type.Description != null) {
                    Property.GetRouteTypes(riskData.NomenclatureAddress.Type.Id);
                    if (riskData.NomenclatureAddress.Suffix1 != null) {
                        Property.GetSuffixes1(riskData.NomenclatureAddress.Suffix1.Id);
                    }
                    else {
                        $("#selectSuffix1").attr("disabled", true);
                    }
                    if (riskData.NomenclatureAddress.Suffix2 != null) {
                        Property.GetSuffixes2(riskData.NomenclatureAddress.Suffix2.Id);
                    }
                    else {
                        $("#selectSuffix2").attr("disabled", true);
                    }
                    if (riskData.NomenclatureAddress.ApartmentOrOffice != null) {

                        Property.GetApartmentsOrOffices(riskData.NomenclatureAddress.ApartmentOrOffice.Id);
                    } else {
                        $("#selectAparmentOrOffice").attr("disabled", true);
                    }
                    if (riskData.NomenclatureAddress.Name != null) {
                        $("#inputName").val(decodeURIComponent(riskData.NomenclatureAddress.Name));
                    }
                    $("#inputLetter").val(riskData.NomenclatureAddress.Letter);
                    $("#inputNro1").val(riskData.NomenclatureAddress.Number1);
                    $("#inputLetter2").val(riskData.NomenclatureAddress.Letter2);
                    $("#inputNro2").val(riskData.NomenclatureAddress.Number2);
                    $("#inputNro3").val(riskData.NomenclatureAddress.Number3);
                } else {
                    $("#selectRouteType").attr("disabled", true);
                }
            } else {
                $("#selectRouteType").attr("disabled", true);
            }

            if (riskData.Risk.RiskActivity != null) {
                Property.GetRiskActivities(riskData.Risk.RiskActivity.Id);
            }
            else {
                $("#selectRiskActivity").UifSelect("setSelected", 0);
            }

            if (riskData.RiskSubActivity != null) {
                Property.GetRiskSubActivities(riskData.Risk.RiskActivity.Id, riskData.RiskSubActivity.Id);
                if (typeof ($('#selectRiskSubActivity').val()) != "undefined" && $('#selectRiskSubActivity').val() != null)
                    $("#selectRiskSubActivity").UifSelect("setSelected", riskData.RiskSubActivity.Id);
            }
            else {
                $("#selectRiskSubActivity").UifSelect('setSelected', 0);
            }

            if (riskData.IsFacultative) {
                $("#chkIsFacultative").attr("checked", true);
            } else {
                $("#chkIsFacultative").attr("checked", false);
            }
            if (riskData.FullAddress != null) {
                $("#inputFullAddress").val(decodeURIComponent(riskData.FullAddress));
            }

            if (riskData.Risk.IsFacultative) {
                $("#chkIsFacultative").prop("checked", true);
                $('#chkIsRetention').prop("disabled", true);
            } else {
                $("#chkIsFacultative").prop("checked", false);
            }

            if (riskData.Risk.Retention) {
                $("#chkIsRetention").prop("checked", true);
                $('#chkIsFacultative').prop("disabled", true);
            } else {
                $("#chkIsRetention").prop("checked", false);
            }

            $('#checkPrincipalRisk').prop('checked', riskData.PrincipalRisk);

            $("#inputAmountInsured").text(FormatMoney(riskData.Risk.AmountInsured));
            $("#inputPremium").text(FormatMoney(riskData.Risk.Premium, 2));
            if (riskData.Risk.GroupCoverage != null) {
                Property.GetGroupCoverages(glbPolicy.Product.Id, riskData.Risk.GroupCoverage.Id);
            }
            else {
                $("#selectGroupCoverage").UifSelect('setSelected', 0);
            }

            if (glbRisk.Id == 0) {
                // $("#inputPML").val(riskData.PML);
                $("#inputSquare").val(riskData.Square);
            }
            if (riskData.City != null) {
                Property.GetCountriesProperty(riskData.City);
            }
            dynamicProperties = riskData.Risk.DynamicProperties;
            Property.LoadSubTitles(0);
            if (riskData.ConstructionType != null) {
                RiskPropertyAdditionalData.GetConstructionType(riskData.ConstructionType);
            }
            if (riskData.RiskType != null) {
                Property.GetRiskType(riskData.RiskType.Id);
            } else {
                Property.GetRiskType(0);
            }
            if (riskData.RiskUse != null) {
                RiskPropertyAdditionalData.GetRiskUses(riskData.RiskUse);
            }
            Property.DisabledControlByEndorsementType(true);
            if (riskData.InsuredObjects != null) {
                $.each(riskData.InsuredObjects, function (key, value) {
                    if (this.Amount != 0) {
                        if (typeof this.Amount == 'string') {
                            this.Amount = parseFloat((this.Amount).replace(',', '.'));
                        }
                        if (typeof this.Premium == 'string') {
                            this.Premium = parseFloat((this.Premium).replace(',', '.'));
                        }
                    }
                });
                Property.LoadInsuredObjects(riskData.InsuredObjects);
            }
        } 
    }
    static GetRiskDataModel() {
        var riskData = $("#formProperty").serializeObject();
        //        riskData.RiskId = $("#selectRisk").UifSelect("getSelected");
        riskData.RiskId = glbRisk.Id;
        if ($('#inputInsured').data("Object") != null) {
            riskData.InsuredName = ReplaceCharacter($('#inputInsured').data("Object").Name);
            riskData.RiskType = $("#selectRiskTypeLocation").UifSelect("getSelected");
            riskData.InsuredId = $('#inputInsured').data("Object").IndividualId;
            riskData.InsuredCustomerType = $("#inputInsured").data("Object").CustomerType;
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
        if ($("#inputAmountInsured").text() != "" && $("#inputAmountInsured").text() != "0") {
            riskData.AmountInsured = NotFormatMoney($("#inputAmountInsured").text());
        }
        if ($("#inputPremium").text() != "" && $("#inputPremium").text() != "0") {
            riskData.Premium = NotFormatMoney($("#inputPremium").text());
        }
        riskData.HasNomenclature = $('#withNomenclature').is(':checked');
        riskData.RouteTypeId = ($("#selectRouteType").val() == null || $("#selectRouteType").val() == "") ? RouteType.Street : $("#selectRouteType").UifSelect("getSelected");
        riskData.Name = ReplaceCharacter($("#inputName").val());
        riskData.Letter = $("#inputLetter").val();
        riskData.Suffix1Id = $("#selectSuffix1").UifSelect("getSelected");
        riskData.Number1Id = $("#inputNro1").val();
        riskData.Letter2 = $("#inputLetter2").val();
        riskData.Number2 = $("#inputNro2").val();
        riskData.Suffix2Id = $("#selectSuffix2").UifSelect("getSelected");
        riskData.ApartmentOrOfficeId = ($("#selectAparmentOrOffice").UifSelect("getSelected") == null || $("#selectAparmentOrOffice").UifSelect("getSelected") == "") ? $("#selectAparmentOrOffice option:eq(1)").val() : $("#selectAparmentOrOffice").UifSelect("getSelected");
        riskData.Number3 = $("#inputNro3").val();
        riskData.FullAddress = ReplaceCharacter($("#inputFullAddress").val());
        riskData.CountryCode = $("#selectCountry").UifSelect("getSelected");
        riskData.DaneCode = $("#inputDaneCode").val();
        riskData.StateCode = $("#selectState").UifSelect("getSelected");
        riskData.CityCode = $("#selectCity").UifSelect("getSelected");
        riskData.RateZoneDescription = ReplaceCharacter($("#inputRateZone").val());
        riskData.RateZoneId = $("#inputRateZone").data("Id");
        riskData.RiskActivityId = $("#selectRiskActivity").UifSelect("getSelected");
        riskData.GroupCoverage = $("#selectGroupCoverage").UifSelect("getSelected");
        riskData.IsFacultative = $("#chkIsFacultative").is(":checked");
        riskData.AmountInsured = NotFormatMoney($("#inputAmountInsured").text());
        riskData.Premium = NotFormatMoney($("#inputPremium").text());
        if ($("#inputInsured").data("Object") != null) {
            riskData.HolderIdentificationDocument = $("#inputInsured").data("Object").IdentificationDocument.Number;
        }
        riskData.TemporalId = glbPolicy.Id;
        riskData.PrefixId = glbPolicy.Prefix.Id;
        riskData.ProductId = glbPolicy.Product.Id;
        riskData.PolicyTypeId = glbPolicy.PolicyType.Id;
        riskData.HolderId = glbPolicy.Holder.IndividualId;
        riskData.EndorsementType = glbPolicy.Endorsement.EndorsementType;

        var insuredObjectsValues = $("#listInsuredObjects").UifListView('getData');
        if (insuredObjectsValues != null) {
            $.each(insuredObjectsValues, function (key, value) {
                this.Amount = NotFormatMoney(this.Amount);
                this.Rate = NotFormatMoney(this.Rate);
                this.Premium = NotFormatMoney(this.Premium, 2);
                this.DeclarationPeriod = glbRisk.DeclarationPeriod;
                this.BillingPeriodDepositPremium = glbRisk.BillingPeriodDepositPremium;
            });
        }
        riskData.InsuredObjects = insuredObjectsValues;


        if (glbRisk.Id == 0) {
            riskData.AdditionalDataViewModel = {};
            riskData.AdditionalDataViewModel.PML = $("#inputPML").val();
            riskData.AdditionalDataViewModel.Square = $("#inputSquare").val();
            riskData.AdditionalDataViewModel.ConstructionYear = $("#inputConstructionYear").val();
            riskData.AdditionalDataViewModel.RiskAge = $("#inputRiskAge").val();
            riskData.AdditionalDataViewModel.ConstructionYear = $("#inputConstructionYear").val();
            riskData.AdditionalDataViewModel.RiskAge = $("#inputRiskAge").val();
            riskData.AdditionalDataViewModel.FloorNumber = $("#inputFloorNumber").val();
            riskData.AdditionalDataViewModel.PrincipalRisk = $("#checkPrincipalRisk").is(":checked");
            if ($("#selectRiskTypeLocation").val() != 0) {
                riskData.AdditionalDataViewModel.RiskType = $("#selectRiskTypeLocation").val();
            }
            if ($("#selectUseRisk").val() != 0) {
                riskData.AdditionalDataViewModel.RiskUse = $("#selectUseRisk").val();
            }
            if ($("#selectConstructionType").val() != 0) {
                riskData.AdditionalDataViewModel.ConstructionType = $("#selectConstructionType").val();
            }

        }
        //add
        else {
            riskData.AdditionalDataViewModel = {};
            riskData.AdditionalDataViewModel.ConstructionYear = $("#inputConstructionYear").val();
            riskData.AdditionalDataViewModel.RiskAge = $("#inputRiskAge").val();
            riskData.AdditionalDataViewModel.ConstructionYear = $("#inputConstructionYear").val();
            riskData.AdditionalDataViewModel.RiskAge = $("#inputRiskAge").val();
            riskData.AdditionalDataViewModel.FloorNumber = $("#inputFloorNumber").val();
            riskData.AdditionalDataViewModel.PrincipalRisk = $("#checkPrincipalRisk").is(":checked");
            if ($("#selectRiskTypeLocation").val() != 0) {
                riskData.AdditionalDataViewModel.RiskType = $("#selectRiskTypeLocation").val();
            }
            if ($("#selectUseRisk").val() != 0) {
                riskData.AdditionalDataViewModel.RiskUse = $("#selectUseRisk").val();
            }
            if ($("#selectConstructionType").val() != 0) {
                riskData.AdditionalDataViewModel.ConstructionType = $("#selectConstructionType").val();
            }

            riskData.AdditionalDataViewModel.PML = $("#inputPML").val();
            riskData.AdditionalDataViewModel.Square = $("#inputSquare").val();
        }

        //datos adicionales
        riskData.RiskSubActivityId = $("#selectRiskSubActivity").UifSelect("getSelected");
        riskData.RiskSubActivityDescripcion = $("#selectRiskSubActivity option:selected").text();
        riskData.IsRetention = $('#chkIsRetention').is(':checked');

        var coveragesValues = $("#listInsuredObjects").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
        });

        riskData.Coverages = coveragesValues;
        riskData.Clauses = glbRisk.Clauses;
        riskData.Text = glbRisk.Text;

        return riskData;
    }

    static SaveRisk(redirec, insuredObjectId) {
        var Address = $("#inputFullAddress").val();
        $("#formProperty").validate();
        if (Address == undefined || Address == "") {
            $("#ReqinputAddress").show();
            return;
        } else {
            $("#ReqinputAddress").hide();
        }
        if ($("#formProperty").valid()) {
            var riskData = Property.GetRiskDataModel();
            if (glbRisk.DeclarationPeriodCode != null && glbRisk.DeclarationPeriodCode > 0) {
                var declaration = glbRisk.DeclarationPeriodCode;
            }
            if (glbRisk.BillingPeriodDepositPremium != null && glbRisk.BillingPeriodDepositPremium > 0) {
                var adjust = glbRisk.BillingPeriodDepositPremium;
            }
            PropertyRequest.SaveRisk(glbPolicy.Id, riskData, riskData.InsuredObjects, dynamicProperties).done(function (data) {
                if (data.success) {
                    Property.UpdateGlbRisk(data.result);
                    if (declaration != undefined && declaration > 0) {
                        glbRisk.DeclarationPeriodCode = declaration;
                    }
                    if (adjust != undefined && adjust > 0) {
                        glbRisk.BillingPeriodDepositPremium = adjust;
                    }
                    Property.LoadSubTitles(0);
                    if (redirec != MenuType.InsuredObject) {
                        if ($("#selectRisk").UifSelect("getSelected") == null || $("#selectRisk").UifSelect("getSelected") == 0) {
                            Property.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
                        }
                    }
                    var events = null;
                    //lanza los eventos para la creación de el riesgo
                    events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
                    if (events !== TypeAuthorizationPolicies.Restrictive) {
                        Property.Redirect(redirec, data, insuredObjectId);
                    }
                    //fin - lanza los eventos para la creación de el riesgo

                    if (data.result.Premium > 0 && redirec == MenuType.Risk && glbPolicy.Product.CoveredRisk.ScriptId > 0 && glbRisk.RecordScript == false) {
                        Property.LoadScript();
                    }
                    RiskClause.GetClausesByLevelsConditionLevelId();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
        }
    }
    static Redirect(redirec, data, insuredObjectId) {

        switch (redirec) {
            case MenuType.Underwriting:
                Property.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.InsuredObject:
                Property.ReturnInsuredObject(insuredObjectId);
                break;
            case MenuType.Beneficiaries:
                Property.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.AdditionalData:
                Property.ShowPanelsRisk(MenuType.AdditionalData);
                break;
            case MenuType.Texts:
                Property.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                Property.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Script:
                Property.ShowPanelsRisk(MenuType.Script);
                break;
            //case MenuType.Coverage:
            //    RiskTransport.ReturnCoverage(coverageId);
            //    break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                break;
        }
    }
    static ReturnUnderwriting() {
        var url = '';

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "PropertyModification";
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

    static ReturnInsuredObject(insuredObjectId) {
        glbCoverage = {
            insuredObjectId: insuredObjectId,
            Object: "ObjectRiskPropertyInsured",
            Class: Property
        }

        router.run("prtInsuredObjectRiskProperty");
    }
    static DeleteRisk() {
        if ($("#selectRisk").UifSelect("getSelected") > 0) {
            PropertyRequest.DeleteRisk(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                            Property.ClearControls();
                            Property.GetRisksByTemporalId(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected"));
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                            ScrollTop();
                            declarationPeriodId = null;
                            billingPeriodId = null;
                        }
                        else {
                            Property.GetRisksByTemporalId(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected"));
                            Property.GetRiskById(glbPolicy.Id, null);
                        }
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

    static GetCountriesProperty(city) {
        if (city != null) {
            if (city.State != null && city.State.Id != 0) {
                PropertyRequest.GetCountries().done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            $('#selectCountry').UifSelect({ sourceData: data.result, selectedId: city.State.Country.Id, enable: disableModificationEndorsement });
                        }
                        PropertyRequest.SelectCountryItemSelected(city.State.Country.Id).done(function (data) {

                            if (data.success) {
                                if (data.result != null) {
                                    $("#selectState").UifSelect({ sourceData: data.result, selectedId: city.State.Id, enable: disableModificationEndorsement });
                                }
                                PropertyRequest.GetCitiesByCountryIdStateId(city.State.Country.Id, city.State.Id).done(function (data) {
                                    if (data.success) {
                                        $("#selectCity").UifSelect({ sourceData: data.result, selectedId: city.Id, enable: disableModificationEndorsement });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }

                                    Property.GetRatingZone(data.result[0].State.Country.Id, data.result[0].State.Id);
                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                                });

                                if (city.DANECode != null) {
                                    $("#inputDaneCode").UifAutoComplete('setValue', city.DANECode);
                                } else {
                                    Property.getDaneCode(city.State.Country.Id, city.State.Id, city.Id);
                                }
                                Property.GetRatingZone(city.State.Country.Id, city.State.Id);
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        }).fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                        });

                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
                });

            }
        } else {
            PropertyRequest.GetCountries().done(function (data) {
                if (data.success) {
                    if (data.result) {
                        countryParameterProperty = 1;
                        $('#selectCountry').UifSelect({ sourceData: data.result, selectedId: countryParameterProperty });
                    }

                    PropertyRequest.SelectCountryItemSelected(countryParameterProperty).done(function (data) {
                        if (data.success) {
                            if (data.result) {
                                $('#selectState').UifSelect({ sourceData: data.result });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingDepartments, 'autoclose': true });
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
            });


        }
    }

    static GetRisksByTemporalId(temporalId, selectedId) {
        lockScreen();
        PropertyRequest.GetCompanyRisksByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    if (selectedId == 0) {
                        if (data.result.length == 1) {
                            $('#selectRisk').UifSelect({ sourceData: data.result, selectedId: data.result[0].Id });
                            Property.GetRiskById(glbPolicy.Id, data.result[0].Id);
                        }
                        else {
                            $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }
                    else {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
                else {
                    $("#selectRisk").UifSelect({ source: null });
                }

                if (glbPersonOnline != null) {
                    Property.loadPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect({ sourceData: data.result });
                    $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                    Property.GetRiskById(glbPolicy.Id, $("#selectRisk option[Value!='']")[0].value);
                }
                else if (glbRisk.Id > 0) {
                    Property.GetRiskById(glbPolicy.Id, glbRisk.Id);
                } else {
                    Property.initializeData();
                }

                if (!glbPolicy.Product.IsFlatRate) {
                    $('#inputRate').prop('disabled', true);
                }
                Property.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        });
    }
    static HidePanelsRisk(Menu) {

        switch (Menu) {
            case MenuType.Risk:
                $("#modalProperty").show();
                break;
            case MenuType.Texts:
                $("#modalTexts").show();
                break;
            case MenuType.Clauses:
                $("#modalClauses").show();
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").show();
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
                $("#modalTexts").UifModal('showLocal', AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                break;
            case MenuType.CrossGuarantees:
                $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                break;
            case MenuType.AdditionalData:
                $("#modalAdditionalData").UifModal('showLocal', AppResources.LabelAdditionalData);
                break;
            case MenuType.Script:
                Property.LoadScript();
                break;
            default:
                break;
        }
    }
    static ShowPanelsCoverage(Menu) {
        switch (Menu) {
            case MenuType.Coverage:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('showLocal', AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Deductibles:
                $("#modalDeductibles").UifModal('showLocal', AppResources.LabelDeductibles);
                break;
        }
    }
    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        var descrip = parseInt(description) || description;

        if (typeof (descrip) !== "number" && descrip.length < 3) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.HolderSearchMinLength, 'autoclose': true });
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
                    Property.LoadInsured(insured);
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
                    $.UifNotify('show', { 'type': 'danger', 'message': resp.result, 'autoclose': true });
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsured, 'autoclose': true });
            });

        } else {

            HoldersRequest.GetHoldersByDescription(description, customerType)
                .done(function (resp) {
                    if (resp.success) {
                        if (resp.result.length == 0) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessagePerson, 'autoclose': true });
                        }
                        else if (resp.result.length == 1) {
                            let insured = resp.result[0]
                            insured.details = [insured.CompanyName]
                            IndividualType = insured.IndividualType
                            Property.LoadInsured(insured);

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

                            Property.ShowModalList(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': resp.result, 'autoclose': true });
                        var ini = String(description).indexOf("(");
                        var fin = String(description).indexOf(")");
                        if (ini != undefined && ini > -1 && fin != undefined && fin > -1) {
                            var res = String(description).substring(ini + 1, fin);
                            Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
                        }
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
                });
        }
    }
    static GetInsuredObjectsByTemporalIdRiskId() {
        if (glbPolicy.Id != 0 && $("#selectRisk").UifSelect("getSelected") != null && $("#selectRisk").UifSelect("getSelected") != "" && $("#selectRisk").UifSelect("getSelected") > 0) {
            PropertyRequest.GetInsuredObjectsByTemporalIdRiskId(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        Property.LoadInsuredObjects(data.result);
                    }
                }
            });
        }
    }
    static LoadInsuredObjects(insuredObjects) {
        if (insuredObjects != null) {
            var totalAmount = 0;
            var totalPremium = 0;

            $.each(insuredObjects, function (index, val) {
                totalPremium = totalPremium + this.Premium;
                totalAmount = totalAmount + this.Amount;
            });
            $.each(insuredObjects, function (index, val) {
                this.Amount = FormatMoney(this.Amount),
                    this.Premium = FormatMoney(this.Premium, 2)
                this.Rate = FormatMoney(this.Rate, 4)

            });
            if (glbPolicy.TemporalType == 1) {
                $("#listInsuredObjects").UifListView({ sourceData: insuredObjects, customDelete: false, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#insuredObjectDisplayTemplate", height: Height, title: AppResources.LabelInsuredObject });
            }
            else {
                $("#listInsuredObjects").UifListView({ sourceData: insuredObjects, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", height: Height, title: AppResources.LabelInsuredObject });
            }
            $("#inputAmountInsured").text(FormatMoney(totalAmount));
            $("#inputPremium").text(FormatMoney(totalPremium, 2));
        }

    }
    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }
    static ClearControls() {

        $('#selectRisk').UifSelect("setSelected", null);
        $("#selectRouteType").UifSelect("setSelected", null);
        $("#inputName").val("");
        $("#inputLetter").val("");
        $("#selectSuffix1").val("").attr("disabled", false);
        $("#inputNro1").val("");
        $("#inputLetter2").val("");
        $("#inputNro2").val("");
        $("#selectSuffix2").UifSelect("setSelected", null);
        $("#selectSuffix2").UifSelect("disabled", false);
        $("#selectAparmentOrOffice").UifSelect("setSelected", null);
        $("#inputNro3").val("");
        $("#inputFullAddress").val("").attr("disabled", false);
        $('#inputDaneCode').UifAutoComplete('setValue', '');
        $('#selectState').UifSelect("setSelected", null);
        $('#selectCity').UifSelect("setSelected", null);
        $("#inputRateZone").data("Id", null);
        $("#inputRateZone").val("").attr("disabled", false);
        $("#inputPrice").val('');
        $("#selectGroupCoverage").UifSelect('setSelected', null);
        $("#listInsuredObjects").UifListView();
        $("#inputRate").val('');
        $("#inputInsured").val('');
        $("#inputInsured").data("Id", null);
        $("#lblAddress").val('');
        $("#lblPhone").val('');
        $("#lblEmail").val('');
        $("#inputAmountInsured").text(0);
        $("#inputPremium").text(0);
        $('#selectRiskActivity').UifSelect('setSelected', null);
        $("#chkIsFacultative").attr("checked", false);
        $('#selectedTexts').text("");
        $('#selectedClauses').text("");
        $('#selectedBeneficiaries').text("");
        $("#selectedAdditionalData").text("");
        $('#selectRiskSubActivity').UifSelect('setSelected', null);
        $('#chkIsRetention').prop('checked', false);
        Property.GetCountriesProperty(null);
        Property.DisabledControlByEndorsementType(false);
        glbRisk.RecordScript = false;
        Property.UpdateGlbRisk({ Id: 0 });
    }
    static ClearPanelAddressNomenclature() {
        $("input").val("");
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, isSelected) {
        $("#formProperty").validate();
        var Address = $("#inputFullAddress").val();
        $("#formProperty").validate();
        if (Address == undefined || Address == "") {
            $("#ReqinputAddress").show();
            $("#selectGroupCoverage").UifSelect("setSelected", null);
            return;
        } else {
            $("#ReqinputAddress").hide();
        }
        if (!$("#formProperty").valid()) {
            $("#selectGroupCoverage").UifSelect("setSelected", null);
            $("#inputAmountInsured").text(0);
            $("#inputPremium").text(0);
        }
        else {
            var totalAmount = 0;
            var totalPremium = 0;
            if (isSelected == true) {
                var RiskData = Property.GetRiskDataModel();
            }

            PropertyRequest.GetInsuredObjectsByProductIdGroupCoverageId(true, RiskData, isSelected).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $("#listInsuredObjects").UifListView({ sourceData: data.result, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", title: AppResources.LabelInsuredObject });
                        ///Calcular Prima
                        var insuredObjectsValues = $("#listInsuredObjects").UifListView('getData');
                        if (insuredObjectsValues != null) {
                            $.each(insuredObjectsValues, function (key, value) {
                                totalAmount = totalAmount + parseFloat(NotFormatMoney(this.Amount));
                                totalPremium = totalPremium + parseFloat(NotFormatMoney(this.Premium, 2));

                            });
                        }
                        $("#inputAmountInsured").text(FormatMoney(totalAmount));
                        $("#inputPremium").text(FormatMoney(totalPremium, 2));
                    }
                }
                else {
                    $("#listInsuredObjects").UifListView({ sourceData: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", title: AppResources.LabelInsuredObject });
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsuranceObjects, 'autoclose': true });
            });
        }


    }
    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        var resultData = {};
        PropertyRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            var resultSave = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });

        return resultData;
    }
    static GetDetailsByIndividualId(individualId, customerType) {
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                if (individualSearchType == 2) {
                    $("#inputBeneficiaryName").data('Detail', Property.GetIndividualDetails(data.result));
                }
                else {
                    $("#inputInsured").data('Detail', Property.GetIndividualDetails(data.result));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        });
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
    static DeleteInsuredObject(dataObject) {
        $("#formProperty").validate();
        if ($("#formProperty").valid()) {
            if (stringToBoolean(dataObject.IsMandatory) == true) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryInsuredObject, 'autoclose': true });
            }
            else {
                var insuredObjects = $("#listInsuredObjects").UifListView('getData');
                if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                    PropertyRequest.DeleteInsuredObject(glbPolicy.Id, $("#selectRisk").val(), dataObject.Id).done(function (data) {
                        if (data.success) {
                            if (data.result != null) {
                                $("#listInsuredObjects").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", title: AppResources.LabelInsuredObject });
                                $.each(insuredObjects, function (index, value) {
                                    if (this.Id != dataObject.Id) {
                                        $("#listInsuredObjects").UifListView("addItem", this);
                                    } else if (data.result != null) {
                                        this.Amount = FormatMoney(data.result.Amount, 2);
                                        this.Premium = FormatMoney(data.result.Premium, 2);
                                        $("#listInsuredObjects").UifListView("addItem", this);
                                    }
                                    value.allowEdit = false;
                                    value.allowDelete = false;
                                });
                                Property.UpdatePremiunAndAmountInsured();
                            }
                        } else {
                            $.UifNotify('show', { 'type': 'success', 'message': data.result, 'autoclose': true });
                        }
                    });
                } else if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
                    if ($("#selectRisk").val() == null || $("#selectRisk").val() == "") {
                        $("#listInsuredObjects").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", title: AppResources.LabelInsuredObject });
                        $.each(insuredObjects, function (index, value) {
                            if (this.Id != dataObject.Id) {
                                $("#listInsuredObjects").UifListView("addItem", this);
                            }
                        });
                        Property.UpdatePremiunAndAmountInsured(); //Por alguna razón, no recalcula cuando se llama el metodo en la 1389
                        $.UifNotify('show', { 'type': 'success', 'message': AppResources.InsuranceObjectDeletedCorrectly, 'autoclose': true });
                    } else {
                        PropertyRequest.DeleteInsuredObject(glbPolicy.Id, $("#selectRisk").val(), dataObject.Id).done(function (data) {
                            if (data.success) {
                                if (data.result != null) {
                                    $("#listInsuredObjects").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", title: AppResources.LabelInsuredObject });
                                    $.each(insuredObjects, function (index, value) {
                                        if (this.Id != data.result.Id) {
                                            $("#listInsuredObjects").UifListView("addItem", this);
                                        }
                                    });
                                    Property.UpdatePremiunAndAmountInsured(); //Por alguna razón, no recalcula cuando se llama el metodo en la 1389
                                    $.UifNotify('show', { 'type': 'success', 'message': AppResources.InsuranceObjectDeletedCorrectly, 'autoclose': true });
                                } else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRemovingObjectInsurance, 'autoclose': true });
                                }
                            }
                        });
                    }
                } else {
                    $("#listInsuredObjects").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#insuredObjectDisplayTemplate", title: AppResources.LabelInsuredObject });
                    $.each(insuredObjects, function (index, value) {
                        if (this.Id != dataObject.Id) {
                            $("#listInsuredObjects").UifListView("addItem", this);
                        }
                    });
                }
                Property.UpdatePremiunAndAmountInsured();
            }
        }
    }
    static UpdatePremiunAndAmountInsured() {
        var insuredObjects = $("#listInsuredObjects").UifListView('getData');
        var totalSumInsured = 0;
        var totalPremium = 0;
        if (insuredObjects != null) {
            $.each(insuredObjects, function (index, value) {
                totalPremium = totalPremium + parseFloat(NotFormatMoney(insuredObjects[index].Premium, 2));
                if (insuredObjects[index].LimitAmount == null) {
                    insuredObjects[index].LimitAmount = 0;
                }
                totalSumInsured = totalSumInsured + parseFloat(NotFormatMoney(insuredObjects[index].Amount, 2));
            });
        }

        $("#inputPremium").text(FormatMoney(totalPremium, 2));
        $("#inputAmountInsured").text(FormatMoney(totalSumInsured, 2));
    }
    static GetRatingZone(country, state) {

        if (country != null && state != null) {
            PropertyRequest.GetRatingZone(glbPolicy.Prefix.Id, country, state).done(function (data) {
                if (data.success) {
                    $("#inputRateZone").data("Id", data.result.Id);
                    $("#inputRateZone").val(data.result.Description).attr("disabled", true);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                resultSave = false;
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRatingZone, 'autoclose': true });
            });
        }
    }

    static CalculateBlock() {
        var routetype = $("#selectRouteType").val();
        var numberName = $("#inputName").val();
        var firstLetter = $("#inputLetter").val();
        var firstSuffix = $("#selectSuffix1").val();
        var firstNumber = $("#inputNro1").val();
        var secondLetter = $("#inputLetter2").val();
        var secondNumber = $("#inputNro2").val();
        var secondSuffix = $("#selectSuffix2").val();
        var region = "";// $("#inputLetter").val();
        var block = $("#inputSquare").val();
        $("#inputSquare").val("");
        var error = "";

        if (routetype == null || routetype == "") {
            error = error + AppResources.ErrorType + "<br>"
        }

        if (numberName == '') {
            error = error + AppResources.ErrorName
        }
        if (firstNumber == '') {
            error = error + AppResources.ErrorNumber1
        }

        if (secondNumber == '') {
            error = error + AppResources.ErrorNumber2
        }

        if (error == "") {
            var firstSuffixText = $("#selectSuffix1 option:selected").text().toUpperCase();
            var secondSuffixText = $("#selectSuffix2 option:selected").text().toUpperCase()

            //Sí es Calle
            if (routetype == '1') {
                if (region.value == 'A') {
                    //Sí Nro.(2) es par
                    if (Property.IsEvenNumber(parseInt(secondNumber, 10))) {
                        block = (parseInt(numberName, 10) - 1) +
                            firstLetter.toUpperCase() +
                            firstSuffixText +
                            secondSuffixText +
                            '-' +
                            firstNumber +
                            secondLetter;
                    }
                    else { //Sí Nro.(2) es impar
                        var strVale = numberName;
                        if (strVale.match(/^\d+$/)) {
                            block = numberName +
                                firstLetter +
                                firstSuffixText +
                                secondSuffixText +
                                '-' +
                                firstNumber +
                                secondLetter;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculateSquare, 'autoclose': true });
                        }
                    }
                }

                if (region == 'B') {
                    //Sí Nro.(2) es par
                    if (PropertyIsEvenNumber(parseInt(secondNumber, 10))) {
                        block = numberName +
                            firstLetter +
                            firstSuffixText +
                            secondSuffixText +
                            '-' +
                            firstNumber +
                            secondLetter;
                    }
                    else {   //Sí Nro.(2) es impar
                        var strVale = numberName;
                        if (strVale.match(/^\d+$/)) {
                            block = (parseInt(numberName, 10) - 1) +
                                firstLetter +
                                firstSuffixText +
                                secondSuffixText +
                                '-' +
                                firstNumber +
                                secondLetter;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculateSquare, 'autoclose': true });
                        }
                    }
                }

                if (region != 'A' && region != 'B') {
                    //Sí Nro.(2) es par
                    if (PropertyIsEvenNumber(parseInt(secondNumber, 10))) {
                        block = (parseInt(numberName, 10)) +
                            firstLetter +
                            firstSuffixText +
                            secondSuffixText +
                            '-' +
                            (parseInt(firstNumber, 10) + 1) +
                            secondLetter;
                    }
                    else {   //Sí Nro.(2) es impar
                        var strVale = numberName;
                        if (strVale.match(/^\d+$/)) {
                            block = (parseInt(numberName, 10) - 1) +
                                firstLetter +
                                firstSuffixText +
                                secondSuffixText +
                                '-' +
                                (parseInt(firstNumber, 10) + 1) +
                                secondLetter;
                        }
                        else { $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorCalculateSquare, 'autoclose': true }); }
                    }
                }

            }
            //Sí es Carrera
            if (routetype == '2') {
                if (region == 'A') {
                    //Sí Nro.(2) es par
                    if (PropertyIsEvenNumber(parseInt(secondNumber, 10))) {
                        block = numberName +
                            firstLetter +
                            secondSuffixText +
                            '-' +
                            firstNumber +
                            secondLetter +
                            firstSuffixText;
                    }
                    else { //Sí Nro.(2) es impar
                        var strVale = numberName;
                        if (strVale.match(/^\d+$/)) {
                            block = numberName +
                                firstLetter +
                                secondSuffixText +
                                '-' +
                                (parseInt(firstNumber, 10) + 1) +
                                secondLetter +
                                firstSuffixText;
                        }
                        else { $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorCalculateSquare, 'autoclose': true }); }
                    }
                }

                if (region.value == 'B') {
                    //Sí Nro.(2) es par
                    if (PropertyIsEvenNumber(parseInt(secondNumber, 10))) {
                        block = numberName.value +
                            firstLetter +
                            secondSuffixText +
                            '-' +
                            (parseInt(firstNumber, 10) + 1) +
                            secondLetter +
                            firstSuffixText;
                    }
                    else { //Sí Nro.(2) es impar
                        var strVale = numberName;
                        if (strVale.match(/^\d+$/)) {
                            block = numberName.value +
                                firstLetter +
                                secondSuffixText +
                                '-' +
                                firstNumber +
                                secondLetter +
                                firstSuffixText;
                        }
                        else { $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorCalculateSquare, 'autoclose': true }); }
                    }
                }

                if (region != 'A' && region != 'B') {
                    //Sí Nro.(2) es par
                    if (PropertyIsEvenNumber(parseInt(secondNumber, 10))) {
                        block = (parseInt(numberName, 10)) +
                            firstLetter +
                            secondSuffixText +
                            '-' +
                            firstNumber +
                            secondLetter +
                            firstSuffixText;
                    }
                    else { //Sí Nro.(2) es impar
                        var strVale = numberName;
                        if (strVale.match(/^\d+$/)) {
                            block = (parseInt(numberName, 10)) +
                                firstLetter +
                                secondSuffixText +
                                '-' +
                                (parseInt(firstNumber, 10) + 1) +
                                secondLetter +
                                firstSuffixText;
                        }
                        else { $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorCalculateSquare, 'autoclose': true }); }
                    }
                }
            }

            //Sí es Avenida
            if (routetype == '5') {
                block = 'AV.' + numberName + firstNumber;
            }

            //Sí es <> Calle, Carrera y Avenida
            if (routetype != '1' && routetype != '2' && routetype != '5') {
                block = 'AV.' + numberName + '-SN';
            }
            $("#inputSquare").val(block);
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': error, 'autoclose': true });
        }
    }
    static IsEvenNumber(n) {
        return ((n % 2) == 0);
    }
    static CalculateAgeByYear(year) {
        var now = new Date();
        var nowYear = now.getFullYear();

        var age = 0;
        if (year != 0 && year < nowYear) {
            age = nowYear - year;
        }
        return age;
    }
    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
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
                    if ($("#inputBeneficiaryName").data("Object") != undefined) {
                        if ($("#inputBeneficiaryName").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputBeneficiaryName").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = this;
                        }
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
    static DisabledControlByEndorsementType(disabled) {
        var endorsementType = parseInt(glbPolicy.Endorsement.EndorsementType, 10);
        switch (endorsementType) {
            case EndorsementType.Renewal: {
                $('#selectCountry').UifSelect('disabled', disabled);
                $('#selectCity').UifSelect('disabled', disabled);
                $('#selectState').UifSelect('disabled', disabled);
                break;
            }
            case EndorsementType.Modification: {
                if (!disableModificationEndorsement) {
                    $('#inputInsured').prop('disabled', disabled);
                    $('#withNomenclature').attr('disabled', disabled);
                    $('#withOutNomenclature').attr('disabled', disabled);
                    $("#inputFullAddress").prop("disabled", disabled);
                    $("#inputDaneCode").prop("disabled", disabled);
                    $('#chkIsFacultative').attr('disabled', disabled);
                    $('#chkIsRetention').attr('disabled', disabled);
                    $('#chkIsRetention').attr('disabled', disabled);
                    $('#selectRiskActivity').attr('disabled', disabled);
                    $('#selectGroupCoverage').attr('disabled', disabled);
                }
                break;
            }
            default:
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
        if (subTitle == 0 || subTitle == 4) {
            if (glbRisk.SecondInsured != null) {
                $("#selectedAdditionalData").text("(" + AppResources.LabelVarious + ")");
            }
            else {
                $("#selectedAdditionalData").text("(" + AppResources.LabelWithoutData + ")");
            }
        }
        if (subTitle == 0 || subTitle == 2) {
            if (glbRisk.Text != null) {
                if (glbRisk.Text.TextBody == null) {
                    glbRisk.Text.TextBody = "";
                }

                if (glbRisk.Text != null && glbRisk.Text.TextBody != null && glbRisk.Text.TextBody.length > 0) {
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
    }
    static LoadScript() {
        if (glbRisk.Id == 0) {
            Property.SaveRisk(MenuType.Script, 0);
        }

        if (glbRisk.Id > 0 && glbPolicy.Product != null && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, dynamicProperties);
        }
    }
    static RunRules(ruleSetId) {
        if (ruleSetId != null && ruleSetId > 0) {
            PropertyRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
                if (data.result != null) {
                    Property.LoadRisk(data.result);
                    //Property.UpdateGlbRisk(data.result);
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunRules, 'autoclose': true });
            });
        }
    }
    static UpdateGlbRisk(data) {
        var recordScript = glbRisk.RecordScript;
        glbRisk = data;
        $.extend(glbRisk, data.Risk);
        glbRisk.Object = "Property";
        glbRisk.formRisk = "#formProperty";
        glbRisk.RecordScript = recordScript;
        glbRisk.Class = Property;
    }
    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbRisk.RecordScript = isModify;
        if (isModify) {
            Property.GetPremium();
        }
    }
    static GetPremium() {
        $("#formProperty").validate();

        if ($("#formProperty").valid()) {
            var riskData = Property.GetRiskDataModel();

            PropertyRequest.GetPremium(riskData, dynamicProperties).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        Property.LoadRisk(data.result);
                        Property.UpdateGlbRisk(data.result);
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
    static getDaneCode(countryId, stateId, cityId) {
        PropertyRequest.getDaneCode(countryId, stateId, cityId).done(function (data) {
            if (data.success) {
                $("#inputDaneCode").UifAutoComplete('setValue', "");
                $("#inputDaneCode").UifAutoComplete('setValue', data.result);
            } else {
                $('#inputDaneCode').val("");
            }
        });
    }
    static LoadInsured(insured) {
        if (individualSearchType == 1) {
            $("#inputInsured").data("Object", insured);
            if ($("#inputInsured").data("Object").IndividualType == 0) {
                var description = $("#inputInsured").data("Object").IdentificationDocument.Number;
                var insuredSearchType = 1;
                var customerType = null;
                if (isNaN(description)) {
                    if (description.trim().split(" ")[0] != null && description.trim().split(" ")[0] != undefined) {
                        description = description.trim().split(" ")[0];
                    }
                }
                UnderwritingRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                    if (data.success) {
                        $("#inputInsured").data("Object").IndividualType = data.result[0].IndividualType;
                    }
                });
            }
            $("#inputInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                Property.GetDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        else if (individualSearchType == 2) {
            $("#inputBeneficiaryName").data("Object", insured);
            $("#inputBeneficiaryName").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                Property.GetDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
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
    static LoadViewModel(viewModel) {
        if (viewModel.HasNomenclature) {
            $('#withNomenclature').attr('checked', true);
            $('#withOutNomenclature').attr('checked', false);
            $("#panelAddressNomenclature").show();
            $("#inputFullAddress").attr("disabled", false);
        }
        else {
            $("#inputFullAddress").attr("disabled", false);
            $("#panelAddressNomenclature").hide();
            $('#withNomenclature').attr('checked', false);
            $('#withOutNomenclature').attr('checked', true);
        }


        if (viewModel.Type != null) {
            $("#selectRouteType").val(viewModel.Type);
        }
        else {
            $("#selectRouteType").attr("disabled", true);
        }
        if (viewModel.Suffix1 != null) {
            $("#selectSuffix1").val(viewModel.Suffix1);
        }
        else {
            $("#selectSuffix1").attr("disabled", true);
        }
        if (viewModel.Suffix2 != null) {
            $("#selectSuffix2").val(viewModel.Suffix2);
        }
        else {
            $("#selectSuffix2").attr("disabled", true);
        }
        if (viewModel.ApartmentOrOffice != null) {
            $("#selectAparmentOrOffice").val(viewModel.ApartmentOrOffice);
        } else {
            $("#selectAparmentOrOffice").attr("disabled", true);
        }
        if (viewModel.Name != null) {
            $("#inputName").val(decodeURIComponent(viewModel.Name));
        }
        $("#inputLetter").val(viewModel.Letter);
        $("#inputNro1").val(viewModel.Number1);
        $("#inputLetter2").val(viewModel.Letter2);
        $("#inputNro2").val(viewModel.Number2);
        $("#inputNro3").val(viewModel.Number3);
        Property.GetRiskActivities(viewModel.RiskActivityId);
        if (viewModel.IsFacultative) {
            $("#chkIsFacultative").attr("checked", true);
        } else {
            $("#chkIsFacultative").attr("checked", false);
        }
        if (viewModel.GroupCoverage != null) {
            Property.GetGroupCoverages(glbPolicy.Product.Id, viewModel.GroupCoverage);
        }
        if (viewModel.FullAddress != null) {
            $("#inputFullAddress").val(decodeURIComponent(viewModel.FullAddress));
        }

        var city = null;
        if (viewModel.StateCode != "" && viewModel.StateCode != 0) {
            city = { Id: viewModel.CityCode, DANECode: viewModel.DaneCode, State: { Id: viewModel.StateCode, Country: { Id: viewModel.CountryCode } } };
        }
        Property.GetCountriesProperty(city);
        $("#inputAmountInsured").text(FormatMoney(viewModel.AmountInsured));
        $("#inputPremium").text(FormatMoney(viewModel.Premium, 2));
        if (viewModel.InsuredObjects != null) {
            $.each(viewModel.InsuredObjects, function (key, value) {
                this.Amount = parseFloat((this.Amount).replace(',', '.'));
                this.Premium = parseFloat((this.Premium).replace(',', '.'));
            });
            Property.LoadInsuredObjects(viewModel.InsuredObjects);
        }
        if (glbRisk.Id == 0) {
            $("#inputSquare").val(viewModel.Square);
        }

        Property.LoadSubTitles(0);
    }
    static UpdatePolicyComponents() {
        PropertyRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                glbRisk = { Id: 0, Object: "Property" };
                Property.ReturnUnderwriting();
            } else {
                $.UifNotify("show", { "type": "danger", "message": AppResources.ErrorSaveTemporary, 'autoclose': true });
            }
        });
    }

    static GetRiskActivities(selectedId) {
        if ($("#selectRiskActivity").length > 1 && selectedId != 0) {
            $("#selectRiskActivity").UifSelect("setSelected", selectedId);
        } else {
            PropertyRequest.GetRiskActivitiesByProductId(glbPolicy.Product.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectRiskActivity").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $("#selectRiskActivity").UifSelect({ sourceData: data.result, selectedId: selectedId, enable: disableModificationEndorsement });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
            });
        }
    }
    //
    static GetRouteTypes(selectedId) {
        if ($("#selectRouteType").length > 1 && selectedId != 0) {
            $("#selectRouteType").UifSelect("setSelected", selectedId)
        } else {
            PropertyRequest.GetRouteTypes().done(function (data) {
                if (data.success) {
                    $("#selectRouteType").attr("disabled", false);
                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectRouteType").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $("#selectRouteType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
            });
        }
    }

    static GetSuffixes1(selectedId) {

        if ($("#selectSuffix1").length > 1 && selectedId != 0) {
            $("#selectSuffix1").UifSelect("setSelected", selectedId)
        } else {
            PropertyRequest.GetSuffixes().done(function (data) {
                if (data.success) {
                    $("#selectSuffix1").attr("disabled", false);
                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectSuffix1").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $("#selectSuffix1").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
            });
        }
    }
    static GetSuffixes2(selectedId) {
        if ($("#selectSuffix2").length > 1 && selectedId != 0) {
            $("#selectSuffix2").UifSelect("setSelected", selectedId)
        } else {
            PropertyRequest.GetSuffixes().done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectSuffix2").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $("#selectSuffix2").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
            });
        }
    }
    static GetApartmentsOrOffices(selectedId) {
        if ($("#selectAparmentOrOffice").length > 1 && selectedId != 0) {
            $("#selectAparmentOrOffice").UifSelect("setSelected", selectedId)
        } else {
            PropertyRequest.GetApartmentsOrOffices().done(function (data) {
                if (data.success) {

                    if (data.result != null) {
                        if (selectedId == 0) {
                            $("#selectAparmentOrOffice").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $("#selectAparmentOrOffice").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
            });
        }
    }
    static GetDaneCodeByQuery(selectedId) {
        PropertyRequest.GetDaneCodeByQuery().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedId == 0) {
                        $("#inputDaneCode").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#inputDaneCode").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
        });
    }


    static GetSubActivityRisksByActivityRiskId(event, selectedItem) {
        if (selectedItem.Id > 0) {
            PropertyRequest.GetSubActivityRisksByActivityRiskId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        if (data.result.length == 1) {
                            $('#selectRiskSubActivity').UifSelect({ sourceData: data.result, selectedId: data.result.length });
                        } else {
                            $("#selectRiskSubActivity").UifSelect({ sourceData: data.result });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static ValidateAddress() {
        PropertyRequest.GetNomenclaturesAll().done(function (data) {
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

    static ChangeIsRetention() {
        if ($("#chkIsFacultative").prop("checked")) {
            $('#chkIsRetention').prop("disabled", true);
        } else {
            $("#chkIsRetention").prop("disabled", false);
        }
    }

    static ChangeIsFacultative() {
        if ($("#chkIsRetention").prop("checked")) {
            $('#chkIsFacultative').prop("disabled", true);
        } else {
            $("#chkIsFacultative").prop("disabled", false);
        }
    }
}

