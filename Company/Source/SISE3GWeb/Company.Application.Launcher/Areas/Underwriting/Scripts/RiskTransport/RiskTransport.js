var individualSearchType = 1;
var IndividualType;
var isnCalculate = false;
var riskController;
var dynamicProperties = null;
var policyIsFloating;
var State;
var IsEditing = false;
var transportDto = null;
var TotalInsuredAmmount = 0;
var TotalPremium = 0;
var LeapYear = null;
var totaldays = null;
var riskCommercialClass = {};
var firstDeclarationPeriod = null;
var insuredObject = { Id: 0, Amount: 0, RateType: 0, Rate: 0 };
var QtyRisk = 0;
var disableModificationEndorsement = true;
class RiskTransport extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        riskController = 'RiskTransport';
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskTransport", formRisk: "#formTransport", RecordScript: false, Class: RiskTransport };
        }
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $('#inputSource').ValidatorKey(7, 1, 1);
        $('#inputTarget').ValidatorKey(7, 1, 1);
        $('#inputMinimumPremium').OnlyDecimals(UnderwritingDecimal);
        $('#SpecificTransport').hide();
        $('#AutomaticTransport').hide();
        $('#inputAnualBudget').OnlyDecimals(UnderwritingDecimal);
        $('#inputReleaseAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputReleaseAmountEsp').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitMaxReleaseAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputFreightAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputFreightAmountEsp').OnlyDecimals(UnderwritingDecimal);
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });

        RiskTransport.GetLeapYear();
        RiskTransport.GetPolicyTypes(glbPolicy.Prefix.Id, glbPolicy.PolicyType.Id);
        RiskTransport.ShowPanelsRisk(MenuType.Risk);
        RiskTransport.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
        RiskTransport.GetCountries(1);

        if (glbRisk.Id == 0) {
            RiskTransport.GetHolderTypes();
            RiskTransport.GetBillingPeriods(AdjustPeriod.ANUAL, function (billingPeriod) {
                $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, selectedId: AdjustPeriod.ANUAL, filter: true });
            });
            RiskTransport.GetDeclarationPeriods(null);
        }
        UnderwritingQuotation.DisabledButtonsQuotation();

        $("#chkIsFacultative").on("change", RiskTransport.ChangeIsRetention);
        $("#chkIsRetention").on("change", RiskTransport.ChangeIsFacultative);
        


    }

    static GetLeapYear() {
        RiskTransportRequest.GetLeapYear().done(function (data) {
            if (data.success) {
                LeapYear = data.result;
            }
        });
    }

    static getPersonOnline() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
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
                    RiskTransport.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskTransport.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }

    bindEvents() {
        $("#btnScript").on('click', RiskTransport.LoadScript);
        $('#selectGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $('#selectRisk').on('itemSelected', RiskTransport.ChangeRisk);
        $('#listCoverages').on('rowAdd', this.AddCoverage);
        $('#listCoverages').on('rowEdit', this.EditCoverage);
        $('#listCoverages').on('rowDelete', this.CoverageDelete);
        $("#selectFromCountry").on('itemSelected', this.GetFromStatesByCountryId);
        $("#selectToCountry").on('itemSelected', this.GetToStatesByCountryId);
        $("#btnAccept").on('click', this.Accept);
        $("#btnClose").on('click', this.Close);
        $("#btnAddRisk").on('click', RiskTransport.NewRisk);
        $('#selectFromDepartment').on('itemSelected', this.GetFromCitiesByStateId);
        $('#selectToDepartment').on('itemSelected', this.GetToCitiesByStateId);
        $("#btnDeleteRisk").on('click', RiskTransport.ConfirmDeleteRisk);
        $('#inputMinimumPremium').on('focusin', this.inputMoneyOnFocusin);
        $('#inputMinimumPremium').focusout(Underwriting.FormatMoneyOut);
        $('#inputReleaseAmount').focusin(Underwriting.NotFormatMoneyIn);
        $('#inputReleaseAmount').focusout(Underwriting.FormatMoneyOut);
        $('#inputReleaseAmountEsp').focusin(Underwriting.NotFormatMoneyIn);
        $('#inputReleaseAmountEsp').focusout(Underwriting.FormatMoneyOut);
        $('#inputLimitMaxReleaseAmount').focusin(Underwriting.NotFormatMoneyIn);
        $('#inputLimitMaxReleaseAmount').focusout(Underwriting.FormatMoneyOut);
        $('#inputFreightAmount').focusin(Underwriting.NotFormatMoneyIn);
        $('#inputFreightAmount').focusout(Underwriting.FormatMoneyOut);
        $('#inputFreightAmountEsp').focusin(Underwriting.NotFormatMoneyIn);
        $('#inputFreightAmountEsp').focusout(Underwriting.FormatMoneyOut);
        $("#btnIndividualDetailAccept").on('click', RiskTransport.SetIndividualDetail);
        //Beneficiario
        $("#inputInsured").on('buttonClick', this.SearchInsured);
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividual);
        //Seleccionar un elemento de la modal de busqueda
        $('#tableIndividualResults tbody').on('click', 'tr', function (e) {
            if (individualSearchType == 2) {
                RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
            }
            else {
                RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
            }
            $('#modalIndividualSearch').UifModal('hide');
        });
        $('#btnDetail').on('click', function () {
            RiskTransport.ShowInsuredDetail();
        });
        $("#inputRiskCommercialClass").on('itemSelected', this.SelectedRiskCommercialClass);
        $("#selectHolderTypes").on('itemSelected', this.SelectedHolderType);
        $("#autSelectHolderTypes").on('itemSelected', this.SelectedHolderType);
        $('#selectDeclarationPeriod').on('itemSelected', this.ChangeDeclarationPeriod);
    }
    static LoadScript() {
        if (glbRisk.Id == 0) {
            RiskTransport.SaveRisk(MenuType.Script, 0);
        }

        if (glbRisk.Id > 0 && glbPolicy.Product != null && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, dynamicProperties);
        }
    }

    inputMoneyOnFocusin() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == 0 ? $(this).val("") : $(this).val(value);
    }
    static ShowInsuredDetail() {
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($('#inputInsured').data('Detail') != null) {
            var insuredDetails = $('#inputInsured').data('Detail');

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
    CoverageDelete(event, data) {
        RiskTransport.DeleteCoverage(data);
    }
    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteInsuredObject, 'autoclose': true });
        }
        else {
            var InsuredObjects = $("#listCoverages").UifListView('getData');
            $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });

            $.each(InsuredObjects, function (index, value) {
                if (this.Id != data.Id) {
                    $("#listCoverages").UifListView("addItem", this);
                }
            });
            RiskTransport.UpdatePremiunAndAmountInsured();
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
            if ($("#selectTransportTypeIds").UifSelect("getSelected") == null) {
                $("#reqSelectTransportTypeIds").show();
            } else {
                $("#reqSelectTransportTypeIds").hide();
            }

            RiskTransport.SaveRisk(MenuType.Coverage, data.Id);
        }
    }

    RedirectClauses(event) {
        RiskTransport.SaveRisk(MenuType.Clauses, 0);
    }

    SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);
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
        if (isNaN(Menu)) {
            Menu = Menu.data.Menu;
        }
        switch (Menu) {
            case MenuType.Risk:
                if (policyIsFloating != null && policyIsFloating != undefined) {
                    if (!policyIsFloating) {
                        $('#SpecificTransport').show();
                        $('#AutomaticTransport').hide();
                    } else {
                        $('#AutomaticTransport').show();
                        $('#SpecificTransport').hide();
                    }
                }
                RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
                break;
            case MenuType.Texts:
                RiskTransport.GetTextsByRiskId(glbRisk.Id);
                $("#modalTexts").UifModal('showLocal', AppResources.LabelDataTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                break;
            case MenuType.Script:
                RiskTransport.LoadScript();
                break;
            default:
                break;
        }
    }

    static GetTextsByRiskId(riskId) {
        RiskTransportRequest.GetTextsByRiskId(riskId).done(function (data) {
            if (data.success) {
                glbRisk.Text = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    SelectIndividual(e) {
        if (individualSearchType == 2) {
            RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
        }
        else {
            RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        }
        $('#modalIndividualSearch').UifModal("hide");
    }

    static GetRisksByTemporalId(temporalId, selectedId) {

        RiskTransportRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    QtyRisk = data.result.length;
                    if (selectedId > 0) {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        var selectedItem = {
                            Id: selectedId
                        };
                    }
                    else {
                        if (data.result.length == 1) {
                            selectedId = data.result[0].Id;
                            var selectedItem = {
                                Id: selectedId
                            };
                            $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        } else {
                            var selectedItem = {
                                Id: 0
                            };
                            $('#selectRisk').UifSelect({ sourceData: data.result, selectedId: selectedId });
                        }
                        if (data.result.length == 0) {
                            RiskTransport.GetDeclarationPeriods(null);
                            RiskTransport.GetBillingPeriods(AdjustPeriod.ANUAL, function (billingPeriod) {
                                $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, selectedId: AdjustPeriod.ANUAL, filter: true });
                                $("#selectBillingPeriod").UifSelect('disabled', false);
                            });
                        }
                    }
                    RiskTransport.ChangeRisk(null, selectedItem);
                }
                RiskTransport.GetPolicyTypes(glbPolicy.Prefix.Id, glbPolicy.PolicyType.Id);

                if (glbPersonOnline != null) {
                    RiskTransport.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect("setSelected", $("#selectRisk option[Value!='']")[0].value);
                    RiskTransport.GetRiskById($("#selectRisk option[Value!='']")[0].value);

                }
                else if (glbRisk.Id > 0) {
                    if (selectedId != glbRisk.Id) {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: glbRisk.Id });
                        RiskTransport.GetRiskById(glbRisk.Id);
                    }
                }
                else {
                    RiskTransport.SetInitialTransport();
                }
                RiskTransport.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static loadBeneficiaries(beneficieries) {
        var result = beneficieries;
        $.each(result, function (id, item) {
            this.BeneficiaryTypeDescription = this.Description;
            this.IdentificationDocument = { Number: this.CardNumber };
        });
        return result;
    }
    static GetRiskById(id) {
        if (id != null && id != "") {
            RiskTransportRequest.GetRiskById(id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        glbRisk.RecordScript = true;
                        riskCommercialClass.Id = data.result.RiskCommercialClassId;
                        RiskTransport.LoadRisk(data.result);
                        RiskTransport.LoadCoverages(data.result.InsuredObjects, data.result.CoverageGroupId);
                        RiskTransport.UpdatePremiunAndAmountInsured();
                        glbRisk.Clauses = data.result.Clauses;
                        glbRisk.Beneficiaries = RiskTransport.loadBeneficiaries(data.result.Beneficiaries);
                        glbRisk.Text = data.result.Text;
                        RiskTransport.LoadSubTitles(0);

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
        if (selectedItem != undefined && selectedItem.Id > 0) {
            $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
            RiskTransport.GetRiskById(selectedItem.Id);
            glbRisk.Id = selectedItem.Id;
        }
        else {
            RiskTransport.ClearControls();
        }
    }

    static SetInitialTransport() {
        RiskTransport.GetCargoTypes();
        RiskTransport.GetPackagingTypes();
        RiskTransport.GetTransportTypes();
        RiskTransport.GetGroupCoverages(glbPolicy.Product.Id, 0);
        RiskTransport.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    static GetCargoTypes(SelectedId) {
        var dfd = $.Deferred();
        RiskTransportRequest.GetCargoTypes().done(function (data) {
            if (data.success) {
                if (SelectedId != undefined) {
                    $('#selectCargoType').UifSelect({ sourceData: data.result, selectedId: SelectedId, filter: true });

                } else {
                    $("#selectCargoType").UifSelect({ sourceData: data.result });

                }
                dfd.resolve();
            }
            else {
                dfd.reject(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfd.reject(AppResources.ErrorGetSurcharge);
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetSurcharge, 'autoclose': true });
        });
        return dfd.promise();
    }

    static GetPackagingTypes(selectedId) {
        var dfd = $.Deferred();
        RiskTransportRequest.GetPackagingTypes().done(function (data) {
            if (data.success) {
                if (selectedId != undefined) {
                    $('#selectPackagingType').UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true });

                } else {
                    $("#selectPackagingType").UifSelect({ sourceData: data.result });
                }
                dfd.resolve();
            }
            else {
                dfd.reject(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfd.reject(AppResources.ErrorGetPackagingTypes);
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetPackagingTypes, 'autoclose': true });
        });
    }

    static GetTransportTypes(selectedItem) {
        RiskTransportRequest.GetTransportTypes().done(function (data) {
            if (data.success) {
                if (selectedItem != undefined && selectedItem != null) {
                    $('#selectTransportTypeIds').UifMultiSelect({ sourceData: data.result });
                    $("#selectTransportTypeIds").UifMultiSelect("setSelected", selectedItem);

                } else {
                    $("#selectTransportTypeIds").UifMultiSelect({ sourceData: data.result, filter: true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        })
    }

    static GetCountries(selectedId) {
        RiskTransportRequest.GetCountries().done(function (data) {
            if (data.success) {
                if (selectedId != undefined) {

                    $("#selectFromCountry").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectToCountry").UifSelect({ sourceData: data.result, selectedId: selectedId });

                } else {
                    $("#selectFromCountry").UifSelect({ sourceData: data.result });
                    $("#selectToCountry").UifSelect({ sourceData: data.result });
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    GetFromStatesByCountryId(event, itemSelected) {

        RiskTransport.GetFromStatesByCountryIdModel(itemSelected.Id);
    }

    static GetFromStatesByCountryIdModel(countryID, selectId) {
        RiskTransportRequest.GetStatesByCountryId(countryID).done(function (data) {
            if (data.success) {
                if (selectId == null) {
                    $("#selectFromDepartment").UifSelect({ sourceData: data.result });
                } else {
                    $("#selectFromDepartment").UifSelect({ sourceData: data.result, selectedId: selectId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    GetFromCitiesByStateId(event, itemSelected) {
        var countryId = $('#selectFromCountry option:selected').val();
        RiskTransport.GetCitiesByStateIdModel(countryId, itemSelected.Id);
    }

    static GetCitiesByStateIdModel(countryId, stateId, selectId) {
        RiskTransportRequest.GetCitiesByStateIdByCountryId(countryId, stateId).done(function (data) {
            if (data.success) {
                if (selectId == null) {
                    $("#selectFromTown").UifSelect({ sourceData: data.result });
                } else {
                    $("#selectFromTown").UifSelect({ sourceData: data.result, selectedId: selectId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        })
    }

    GetToStatesByCountryId(event, itemSelected) {
        RiskTransport.GetToStatesByCountryIdModel(itemSelected.Id);
    }

    static GetToStatesByCountryIdModel(countryId, selectId) {
        RiskTransportRequest.GetStatesByCountryId(countryId).done(function (data) {
            if (data.success) {
                if (selectId == null) {
                    $("#selectToDepartment").UifSelect({ sourceData: data.result });

                } else {
                    $("#selectToDepartment").UifSelect({ sourceData: data.result, selectedId: selectId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    GetToCitiesByStateId(event, itemSelected) {
        var countryId = $('#selectToCountry option:selected').val()
        RiskTransport.GetToCitiesByStateIdModel(countryId, itemSelected.Id);
    }

    static GetToCitiesByStateIdModel(countryId, stateId, selectId) {
        RiskTransportRequest.GetCitiesByStateIdByCountryId(countryId, stateId).done(function (data) {
            if (data.success) {
                if (selectId == null) {
                    $("#selectToTown").UifSelect({ sourceData: data.result });

                } else {
                    $("#selectToTown").UifSelect({ sourceData: data.result, selectedId: selectId });
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        })
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
            UnderwritingRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0) {
                        UnderwritingPersonOnline.ShowOnlinePerson();
                    }
                    else if (data.result.length == 1) {
                        IndividualType = data.result[0].IndividualType;
                        RiskTransport.LoadInsured(data.result[0]);
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

                        RiskTransport.ShowModalList(dataList);
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
                RiskTransport.GetDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
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
                RiskTransport.GetDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
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
    static GetDetailsByIndividualId(individualId, customerType) {
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                if (individualSearchType == 2) {
                    $("#inputBeneficiaryName").data('Detail', RiskTransport.GetIndividualDetails(data.result));
                }
                else {
                    $("#inputInsured").data('Detail', RiskTransport.GetIndividualDetails(data.result));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        });
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
        var resultData = {};
        RiskTransportRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            var resultSave = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });

        return resultData;
    }

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static RunRules(ruleSetId) {
        RiskTransportRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (data.success) {
                RiskTransport.LoadRisk(data.result);
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
        //Si es endoso de modificacion...
        if (glbPolicy.Endorsement.EndorsementType == 2) {
            disableModificationEndorsement = false;
        } else {
            disableModificationEndorsement = true;
        }
        RiskTransport.GetPolicyTypes(glbPolicy.Prefix.Id, glbPolicy.PolicyType.Id);
        riskData.IsFloating = policyIsFloating;
        RiskTransport.GetGroupCoverages(glbPolicy.Product.Id, riskData.CoverageGroupId);
        $("#selectGroupCoverage").UifSelect("setSelected", riskData.CoverageGroupId);
        RiskTransport.GetCargoTypes(riskData.CargoTypeId);
        RiskTransport.GetPackagingTypes(riskData.TransportPackagingId);
        RiskTransport.GetTransportTypes(riskData.TransportTypeIds);

        if ($("#selectRisk").UifSelect("getSelected") != null || $("#selectRisk").UifSelect("getSelected") != 0 || $("#selectRisk").UifSelect("getSelected") != "") {
            firstDeclarationPeriod = riskData.DeclarationPeriodId;
        }
        if (riskData.DeclarationPeriodId != null) {
            RiskTransport.GetDeclarationPeriods(riskData.DeclarationPeriodId);
        }
        if (riskData.BillingPeriodId != null) {
            RiskTransport.GetBillingPeriods(riskData.BillingPeriodId);
        }

        var inputReleaseAmount = (policyIsFloating) ? "inputReleaseAmount" : "inputReleaseAmountEsp";
        var inputFreightAmount = (policyIsFloating) ? "inputFreightAmount" : "inputFreightAmountEsp";
        $("#inputMinimumPremium").val(FormatMoney(riskData.MinimumPremium));
        $("#hiddenRiskId").val(riskData.Id);
        $("#inputSource").val(riskData.Source);
        $("#inputTarget").val(riskData.Target);
        $("#" + inputReleaseAmount).val(FormatMoney(riskData.ReleaseAmount));
        $("#inputLimitMaxReleaseAmount").val(FormatMoney(riskData.LimitMaxRealeaseAmount));
        $("#" + inputFreightAmount).val(FormatMoney(riskData.FreightAmount));
        $("#inputRiskCommercialClass").UifAutoComplete('setValue', riskData.RiskCommercialClassDescription);

        RiskTransport.GetHolderTypes(riskData.HolderTypeId);
        //if (policyIsFloating) {
        $('#' + inputFreightAmount).prop("disabled", false);
        $("#" + inputReleaseAmount).prop("disabled", false);
        if (riskData.HolderTypeId == "1") {
            $("#" + inputFreightAmount).val("");
            $('#' + inputFreightAmount).prop("disabled", true);
            //validar orden de ejecucion para qitar el try
            try {
                $('.releaseAmtlbl').addsClass("field-required");
                $('.freigthAmtlbl').removeClass("field-required");
            } catch (e) {
                console.log("Error: " + e);
            }
        }
        if (riskData.HolderTypeId == "2") {
            $("#" + inputReleaseAmount).val("");
            $("#" + inputReleaseAmount).prop("disabled", true);
            //validar orden de ejecucion para qitar el try
            try {
                $('.freigthAmtlbl').addClass("field-required");
                $('.releaseAmtlbl').removeClass("field-required");
            } catch (e) {
                console.log("Error: " + e);
            }

        }
        if (riskData.IsFacultative) {
            $("#chkIsFacultative").prop("checked", true);
            $('#chkIsRetention').prop("disabled", true);
        } else {
            $("#chkIsFacultative").prop("checked", false);
        }

        if (riskData.IsRetention) {
            $("#chkIsRetention").prop("checked", true);
            $('#chkIsFacultative').prop("disabled", true);
        } else {
            $("#chkIsRetention").prop("checked", false);
        }

        if (riskData.FromCountryId != 0 && riskData.FromCountryId != null) {
            $("#selectFromCountry").UifSelect("setSelected", riskData.FromCountryId);
        }
        if (riskData.ToCountryId != 0 && riskData.ToCountryId != null) {
            $("#selectToCountry").UifSelect("setSelected", riskData.ToCountryId);
        }
        if (!policyIsFloating && riskData.Id != null) {
            RiskTransport.GetFromStatesByCountryIdModel(riskData.FromCountryId, riskData.FromStateId);
            RiskTransport.GetCitiesByStateIdModel(riskData.FromCountryId, riskData.FromStateId, riskData.FromCityId);
            RiskTransport.GetToStatesByCountryIdModel(riskData.ToCountryId, riskData.ToStateId);
            RiskTransport.GetToCitiesByStateIdModel(riskData.ToCountryId, riskData.ToStateId, riskData.ToCityId);
            RiskTransport.GetViaTypes(riskData.ViaId);
        } else {
            RiskTransport.GetFromStatesByCountryIdModel(1);
            RiskTransport.GetToStatesByCountryIdModel(1);
            RiskTransport.GetViaTypes(null);
        }
        if (riskData.IndividualId > 0) {
            RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(riskData.IndividualId, 2, riskData.CustomerType);
        }

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            var holderId = (policyIsFloating) ? 'autSelectHolderTypes' : 'selectHolderTypes';
            glbRisk.IsFloating = policyIsFloating;
            $('#' + holderId).UifSelect('disabled', true);
            $('#selectDeclarationPeriod').UifSelect('disabled', true);
        }
        RiskTransport.DisableControls(glbPolicy.Endorsement.EndorsementType);
        
    }

    AddCoverage(event) {
        IsEditing = false;
        RiskTransport.SaveRisk(MenuType.Coverage, 0);
    }

    static SaveRisk(redirec, coverageId) {
        var AmountAccesoriesNew = 0;
        var AmountAccesoriesOld = 0;
        $("#formTransport").validate();
        var formValid = $("#formTransport").valid();
        if ($("#selectCargoType").val() == null) {
            formValid = false;
        }
        if ($("#selectPackagingType").val() == null) {
            formValid = false;
        }

        if (formValid) {
            if (RiskTransport.Validations()) {
                if ($("#selectTransportTypeIds").UifSelect("getSelected") == null) {
                    $("#reqSelectTransportTypeIds").show();
                    return;
                } else {
                    $("#reqSelectTransportTypeIds").hide();
                }

                $("#ReqinputReleaseAmount").hide();
                $("#ReqinputFreightAmount").hide();
                var origen = $("#inputSource").val();
                var destino = $("#inputTarget").val();
                if (origen == "") {
                    $("#ReqinputSource").show();
                    return;
                } else {
                    $("#ReqinputSource").hide();
                }
                if (destino == "") {
                    $("#ReqinputTarget").show();
                    return;
                } else {
                    $("#ReqinputTarget").hide();
                }

                if ($("#autSelectHolderTypes").UifSelect("getSelected") == "1" && $("#inputReleaseAmount").val() == "") {
                    $("#ReqinputReleaseAmount").show();
                    return;
                }

                if ($("#autSelectHolderTypes").UifSelect("getSelected") == "2" && $("#inputFreightAmount").val() == "") {
                    $("#ReqinputFreightAmount").show();
                    return;
                }
                //valiadcion Proyeccion Anual de Fletes/Presupuesto Anual Especifica
                if ($("#selectHolderTypes").UifSelect("getSelected") == "1" && $("#inputReleaseAmountEsp").val() == "") {
                    $("#ReqinputReleaseAmount").show();
                    return;
                }

                if ($("#selectHolderTypes").UifSelect("getSelected") == "2" && $("#inputFreightAmountEsp").val() == "") {
                    $("#ReqinputFreightAmount").show();
                    return;
                }


                var riskData = RiskTransport.GetRiskDataModel();
                if (riskData.InsuredObjects.length > 0) {
                    transportDto = riskData;
                    RiskTransportRequest.SaveRisk(glbPolicy.Id, riskData, dynamicProperties).done(function (data) {
                        if (data.success) {
                            if (data.result != null) {
                                RiskTransport.UpdateGlbRisk(data.result);
                                RiskTransportRequest.GetRiskById(glbRisk.Id).done(function (data) {
                                    if (data.success) {
                                        if (data.result != null) {
                                            RiskTransport.LoadCoverages(data.result.InsuredObjects, data.result.CoverageGroupId);
                                            RiskTransport.UpdatePremiunAndAmountInsured();
                                            RiskTransport.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                                        }
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
                                });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                            }
                            RiskClause.GetClausesByLevelsConditionLevelId();
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
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsuredObjects, 'autoclose': true });
                }
            }
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields, 'autoclose': true });
        }
    }

    static ShowSaveRisk(riskId, redirec, coverageId) {
        if (redirec != 9) {
            if ($("#selectRisk").UifSelect("getSelected") == null || $("#selectRisk").UifSelect("getSelected") == 0 || $("#selectRisk").UifSelect("getSelected") == "") {
                RiskTransport.GetRisksByTemporalId(glbPolicy.Id, riskId);
            }
        }
        var events = null;
        //lanza los eventos para la creación de el riesgo
        if (glbRisk.InfringementPolicies != null) {
            events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
        }
        if (events !== TypeAuthorizationPolicies.Restrictive) {
            RiskTransport.RedirectAction(redirec, riskId, coverageId);
        }
        //fin - lanza los eventos para la creación de el riesgo
    }

    static RedirectAction(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskTransport.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.Coverage:
                RiskTransport.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskTransport.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.AdditionalData:
                RiskTransport.ShowPanelsRisk(MenuType.AdditionalData);
                break;
            case MenuType.Texts:
                RiskTransport.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskTransport.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Script:
                RiskTransport.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                break;
        }
    }

    static ReturnCoverage(coverageId) {
        glbCoverage = {
            CoverageId: coverageId,
            Class: RiskTransportCoverage
        }
        router.run("prtRiskTransportCoverage");
    }

    static GetPolicyTypes(prefixId, id) {

        //RiskTransportRequest.GetPolicyType(prefixId, id).done(function (data) { //No se requiere consultar nuevamente la propiedad se alimenta a nivel de la poliza
        //if (data.success) {
        policyIsFloating = glbPolicy.PolicyType.IsFloating;
        if (!policyIsFloating) {
            $('#SpecificTransport').show();
            $('#AutomaticTransport').hide();
        } else {
            $('#AutomaticTransport').show();
            $('#SpecificTransport').hide();
        }
        //}
        //else {
        //    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        //}
        //})
    }

    static GetViaTypes(selectedId) {
        RiskTransportRequest.GetViaTypes().done(function (data) {
            if (data.success) {
                $.each(data.result, function (index, val) {
                    data.result[index] = { "Id": val.Id, "Description": val.Description.toUpperCase() }
                });
                if (selectedId != null) {
                    $('#selectVia').UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true });

                } else {
                    $('#selectVia').UifSelect({ sourceData: data.result, filter: true });
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

    static GetDeclarationPeriods(selectedId) {
        RiskTransportRequest.GetDeclarationPeriods().done(function (data) {
            if (data.success) {
                if (selectedId == 0 || selectedId == null || selectedId == undefined) {
                    $("#selectDeclarationPeriod").UifSelect({ sourceData: data.result, filter: true });

                } else {
                    $("#selectDeclarationPeriod").UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true });
                }

                RiskTransport.CalculateDays(glbPolicy.CurrentFrom, glbPolicy.CurrentTo);
                if (LeapYear && policyIsFloating) {
                    if (totaldays == 365 || totaldays == 366) {
                        $("#selectDeclarationPeriod").prop('disabled', false);
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result });
                        if (selectedId != undefined) {
                            $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true, enable: disableModificationEndorsement });
                        } else {
                            $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result, filter: true, enable: disableModificationEndorsement});
                        }
                    } else {
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result });
                        $("#selectDeclarationPeriod").prop('disabled', true);
                        $("#selectDeclarationPeriod").UifSelect("setSelected", DeclaredPeriod.ANUAL);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.PolicyNotIsAnnual, 'autoclose': true });
                    }

                }
                if (!LeapYear && policyIsFloating) {
                    if (totaldays == 365 || totaldays == 366) {
                        if (selectedId != undefined) {
                            $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true, enable: disableModificationEndorsement });
                        } else {
                            $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result, filter: true, enable: disableModificationEndorsement });
                        }
                    } else {
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: data.result });
                        $("#selectDeclarationPeriod").prop('disabled', true);
                        $("#selectDeclarationPeriod").UifSelect("setSelected", DeclaredPeriod.ANUAL);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.PolicyNotIsAnnual, 'autoclose': true });
                    }

                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        })
    }

    static GetBillingPeriods(selectedId, callback) {
        RiskTransportRequest.GetBillingPeriods().done(function (data) {
            if (data.success) {
                RiskTransport.CalculateDays(glbPolicy.CurrentFrom, glbPolicy.CurrentTo);
                if (callback)
                    return callback(data.result)
                if (LeapYear && policyIsFloating) {
                    if (totaldays == 365 || totaldays == 366) {
                        $('#selectBillingPeriod').UifSelect({ sourceData: data.result, filter: true });
                        if (selectedId != undefined) {
                            $('#selectBillingPeriod').UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true, enable: disableModificationEndorsement });
                        } else {
                            $('#selectBillingPeriod').UifSelect({ sourceData: data.result, filter: true, enable: disableModificationEndorsement});
                        }
                    } else {
                        $('#selectBillingPeriod').UifSelect({ sourceData: data.result, filter: true });
                        $("#selectBillingPeriod").prop('disabled', true);
                        $("#selectBillingPeriod").UifSelect("setSelected", AdjustPeriod.ANUAL);
                    }
                }
                if (LeapYear == false && policyIsFloating) {
                    if (totaldays == 365 || totaldays == 366) {
                        if (selectedId != undefined) {
                            $('#selectBillingPeriod').UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true, enable: disableModificationEndorsement });
                        } else {
                            $('#selectBillingPeriod').UifSelect({ sourceData: data.result, filter: true, enable: disableModificationEndorsement });
                        }
                    } else {
                        $('#selectBillingPeriod').UifSelect({ sourceData: data.result, filter: true });
                        $("#selectBillingPeriod").UifSelect('disabled', true);
                        $("#selectBillingPeriod").UifSelect("setSelected", AdjustPeriod.ANUAL);
                    }

                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    ChangeGroupCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskTransport.GetInsuredObjectsByProductIdGroupCoverageId(selectedItem.Id);
        }
        else {
            $("#listCoverages").UifListView("refresh");
        }
        $('#inputPremium').text(0);
        $("#inputAmountInsured").text(0);
    }

    static GetInsuredObjectsByProductIdGroupCoverageId(groupCoverageId) {
        RiskTransportRequest.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, groupCoverageId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
                RiskTransport.LoadCoverages(data.result, groupCoverageId);
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
        if (glbPolicy.TemporalType == 1) {
            $("#listCoverages").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
        }
        else {
            $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
        }
        coverages.forEach(e => {
            e.Id = e.Id;
            e.InsuredLimitAmount = FormatMoney(e.InsuredLimitAmount);
            e.PremiumAmount = FormatMoney(e.PremiumAmount);
            e.DisplayRate = FormatMoney(e.Rate, 2);
            e.Rate = FormatMoney(e.Rate);
            e.CurrentFrom = FormatDate(e.CurrentFrom);
            e.CurrentTo = FormatDate(e.CurrentTo);
            if (Isnull(FormatDate(e.CurrentFromOriginal))) {
                e.CurrentFromOriginal = e.CurrentFrom;
            }
            else {
                e.CurrentFromOriginal = FormatDate(e.CurrentFromOriginal);
            }
            if (Isnull(FormatDate(e.CurrentToOriginal))) {
                e.CurrentToOriginal = e.CurrentTo;
            }
            else {
                e.CurrentToOriginal = FormatDate(e.CurrentToOriginal);
            }
            if (e.Deductible != null) {
                e.DeductibleDescription = e.Deductible.Description,
                    e.DeductibleId = e.Deductible.Id
            }

            $("#listCoverages").UifListView("addItem", e);
        })
    }

    static GetGroupCoverages(productId, selectedId) {
        RiskTransportRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                //if (callback)
                //    return callback(data.result);

                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, filter: true, enable: disableModificationEndorsement });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId, filter: true, enable: disableModificationEndorsement });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskDataModel() {
        //var riskData = $("#formTransport").serializeObject();
        var riskData = $("#formTransport").find(':visible').serializeObject()
        riskData.ReleaseAmount = NotFormatMoney(riskData.ReleaseAmount);
        if (!policyIsFloating) {
            if ($('#selectHolderTypes').UifSelect('getSelectedText') == HolderType.Generador) {
                riskData.LimitMaxRealeaseAmount = NotFormatMoney(riskData.ReleaseAmount);
            } else {
                riskData.LimitMaxRealeaseAmount = NotFormatMoney(riskData.FreightAmount);
            }
        } else {
            riskData.LimitMaxRealeaseAmount = NotFormatMoney(riskData.LimitMaxRealeaseAmount);
        }
        riskData.MinimumPremium = NotFormatMoney(riskData.MinimumPremium);

        riskData.TransportTypeIds = $('#selectTransportTypeIds').UifMultiSelect('getSelected');
        riskData.PolicyId = glbPolicy.Id;
        riskData.IsFloating = policyIsFloating;
        riskData.Description = $($('#selectTransportTypeIds option:selected')[0]).text() + ' - ' + $("#selectCargoType").UifSelect("getSelectedText");
        riskData.RiskId = $("#hiddenRiskId").val();
        riskData.Id = $('#selectRisk').UifSelect('getSelected');
        var insuredObjectsValues = $("#listCoverages").UifListView('getData');
        if (insuredObjectsValues != null) {
            $.each(insuredObjectsValues, function (key, value) {
                this.InsuredLimitAmount = NotFormatMoney(this.InsuredLimitAmount);
                this.Rate = NotFormatMoney(this.Rate);
                this.PremiumAmount = NotFormatMoney(this.PremiumAmount, 2);

            });
        }
        riskData.InsuredObjects = insuredObjectsValues;

        riskData.BillingDescription = $('#selectBillingPeriod').UifSelect('getSelectedText');
        riskData.CargoDescription = $('#selectCargoType').UifSelect('getSelectedText');
        riskData.DeclarationDescription = $("#selectDeclarationPeriod").UifSelect("getSelectedText");
        riskData.PackagingDescription = $('#selectPackagingType').UifSelect("getSelectedText");
        riskData.ViaDescription = $('#selectVia').UifSelect("getSelectedText");
        if ($('#inputInsured').data("Object") == undefined) {
            $("#inputInsured").data("Detail", RiskTransport.GetIndividualDetails(insured.CompanyName));
        }
        riskData.BirthDate = FormatDate($('#inputInsured').data("Object").BirthDate);
        riskData.Gender = $('#inputInsured').data("Object").Gender;
        riskData.IndividualId = $('#inputInsured').data("Object").IndividualId;
        riskData.DocumentType = $('#inputInsured').data("Object").IdentificationDocument.DocumentType.Id;
        riskData.DocumentNumber = $('#inputInsured').data("Object").IdentificationDocument.Number;
        riskData.Name = $('#inputInsured').data("Object").Name;

        riskData.DocumentTypeDescription = $('#inputInsured').data("Object").IdentificationDocument.DocumentType.Description;
        riskData.DocumentTypeSmallDescription = $('#inputInsured').data("Object").IdentificationDocument.DocumentType.SmallDescription;

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

        if (riskData.IsFloating) {
            riskData.HolderTypeId = $("#autSelectHolderTypes").UifSelect("getSelected");
            riskData.FreightAmount = NotFormatMoney($("#inputFreightAmount").val());
        } else {
            riskData.HolderTypeId = $("#selectHolderTypes").UifSelect("getSelected");
            riskData.FreightAmount = NotFormatMoney($("#inputFreightAmountEsp").val());
        }

        riskData.RiskCommercialClassId = riskCommercialClass.Id;
        riskData.RiskCommercialClassDescription = $('#inputRiskCommercialClass').UifAutoComplete('getValue');

        riskData.IsRetention = $("#chkIsRetention").prop("checked");
        riskData.IsFacultative = $("#chkIsFacultative").prop("checked");

        return riskData;
    }

    static UpdateGlbRisk(data) {
        var recordScript = glbRisk.RecordScript;
        glbRisk = data;
        $.extend(glbRisk, data.Risk);
        glbRisk.Object = "RiskTransport";
        glbRisk.formRisk = "#formTransport";
        glbRisk.RecordScript = recordScript;
        glbRisk.Class = RiskTransport;
    }

    static UpdatePolicyComponents() {
        UnderwritingRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                RiskTransport.ReturnUnderwriting();
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
            glbPolicy.EndorsementController = "TransportModification";
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
        if ($('#inputInsured').data("Object") != null) {
            $("#formTransport").validate();

            if ($("#formTransport").valid()) {
                var recordScript = false;

                if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0) {
                    recordScript = true;
                }
                else {
                    recordScript = glbRisk.RecordScript;
                }

                if (recordScript) {
                    var riskData = RiskTransport.GetRiskDataModel();
                    RiskTransportRequest.GetPremium(glbPolicy.Id, riskData, dynamicProperties).done(function (data) {
                        if (data.success) {
                            RiskTransport.LoadRisk(data.result);
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
                    RiskTransport.LoadScript();
                }
            }
        } else {
            UnderwritingPersonOnline.ShowOnlinePerson();
        }

    }

    static DeleteRisk() {
        if ($("#selectRisk").UifSelect("getSelected") > 0) {
            RiskTransportRequest.DeleteRisk(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    //if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                    RiskTransport.ClearControls();
                    RiskTransport.GetRisksByTemporalId(glbPolicy.Id);
                    RiskTransport.UpdatePremiunAndAmountInsured();
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                    ScrollTop();
                    //}
                    //else {
                    //    var riskId = $("#selectRisk").UifSelect("getSelected");
                    //    RiskTransport.GetRisksByTemporalId(glbPolicy.Id, riskId);
                    //    RiskTransport.GetRiskById(riskId);
                    //}
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
        if (policyIsFloating) {
            RiskTransport.EnableControls();
            RiskTransport.ClearControls();
            RiskTransport.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.AddSpecificRisk, 'autoclose': true });
        }
    }
    static ClearControls() {
        $('#inputSource').val('');
        $('#inputTarget').val('');
        $('#inputMinimumPremium').val('');
        $('#inputReleaseAmount').val('');
        $("#inputLimitMaxReleaseAmount").val("");
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelInsuredObjects, height: 400 });
        $("#selectRisk").UifSelect("setSelected", null);
        $("#selectGroupCoverage").UifSelect("setSelected", null);
        $("#selectCargoType").UifSelect("setSelected", null);
        $("#selectPackagingType").UifSelect("setSelected", null);
        $("#selectTransportTypeIds").UifMultiSelect('deSelectAll');
        $("#selectFromCountry").UifSelect("setSelected", null);
        $("#selectFromDepartment").UifSelect("setSelected", null);
        $("#selectFromTown").UifSelect("setSelected", null);
        $("#selectToCountry").UifSelect("setSelected", null);
        $("#selectToDepartment").UifSelect("setSelected", null);
        $("#selectToTown").UifSelect("setSelected", null);
        $("#selectVia").UifSelect("setSelected", null);

        $("#selectFromDepartment").UifSelect('disabled', true);
        $("#selectFromTown").UifSelect('disabled', true);
        $("#selectToDepartment").UifSelect('disabled', true);
        $("#selectToTown").UifSelect('disabled', true);

        var declaration = $("#selectDeclarationPeriod").UifSelect('getSelected');
        var billingPeriod = $("#selectBillingPeriod").UifSelect('getSelected');
        (declaration == "") ? $("#selectDeclarationPeriod").UifSelect("setSelected", null) : $("#selectDeclarationPeriod").UifSelect('disabled', true);
        (billingPeriod == "") ? $("#selectBillingPeriod").UifSelect("setSelected", null) : $("#selectBillingPeriod").UifSelect('disabled', true);

        if ($("#selectRisk").UifSelect("getSelected") == null) {
            $("#selectBillingPeriod").UifSelect('disabled', false);
        }

        $("#inputFreightAmount").val('');
        $("#autSelectHolderTypes").UifSelect("setSelected", null);
        $("#selectHolderTypes").UifSelect("setSelected", null);
        $('#inputRiskCommercialClass').UifAutoComplete('clean');
    }

    Accept() {
        var premiumValue = NotFormatMoney($('#PremiumAmount').text());
        if (!policyIsFloating && (parseFloat(premiumValue) <= 0)) {
            //Aplica para las polizas especificas
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidationValuePremium, 'autoclose': true });
        } else {
            RiskTransport.SaveRisk(MenuType.Risk, 0);
            ScrollTop();
            transportDto = null;
        }
    }

    Close() {
        glbRisk = { Id: 0, Object: "RiskTransport", Class: RiskTransport };
        if (!policyIsFloating) {
            var premiumValue = NotFormatMoney($('#PremiumAmount').text());
            if (parseFloat(premiumValue) > 0) {
                RiskTransport.UpdatePolicyComponents();
                transportDto = null;
            }
            else if ($("#selectRisk").UifSelect("getSelected") != null) {
                $.UifDialog('confirm', { 'message': Resources.Language.LeaveWithoutPremium }, function (result) {
                    if (result) {
                        RiskTransport.ReturnUnderwriting();
                    }
                });
            } else {
                RiskTransport.UpdatePolicyComponents();
                transportDto = null;
            }
        } else {
            RiskTransport.UpdatePolicyComponents();
            transportDto = null;
        }
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
    }

    static ConfirmDeleteRisk() {
        if ($("#selectRisk").UifSelect("getSelected") > 0) {
            $.UifDialog('confirm', { 'message': Resources.Language.ConfirmDeleteRisk }, function (result) {
                if (result) {
                    RiskTransport.DeleteRisk();
                } else {

                }

            });
        }
    }

    static GetHolderTypes(selectedId) {
        RiskTransportRequest.GetHolderTypes().done(function (data) {
            if (data.success) {
                var selectId = (policyIsFloating) ? "autSelectHolderTypes" : "selectHolderTypes";
                if (selectedId != 0 && selectedId != null) {
                    $('#' + selectId).UifSelect({ sourceData: data.result, selectedId: selectedId });
                } else {
                    $('#' + selectId).UifSelect({ sourceData: data.result });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
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

    static EnableControls() {
        $("#inputInsured").prop('disabled', false);
        $("#selectGroupCoverage").UifSelect('disabled', false);
        $("#inputLimitMaxReleaseAmount").prop('disabled', false);
    }

    static DisableControls(typeEndorsement) {
        switch (typeEndorsement) {
            case EndorsementType.Modification:
                $("#inputInsured").prop('disabled', true);
                $("#selectGroupCoverage").UifSelect('disabled', true);
                $("#inputLimitMaxReleaseAmount").prop('disabled', true);
                break;
        }
    }

    static Validations() {
        var LimitMaxReleaseAmount = NotFormatMoney($("#inputLimitMaxReleaseAmount").val());
        var LimitReleaseAmount = NotFormatMoney($("#inputReleaseAmount").val());
        var FreightAmount = NotFormatMoney($("#inputFreightAmount").val());

        if ((parseInt(LimitMaxReleaseAmount) > parseInt(FreightAmount)) && (parseInt(FreightAmount) > 0)) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorAnnualProjectionFreightsLessThan, 'autoclose': true })
            return false;
        }
        else if ((parseInt(LimitReleaseAmount) < parseInt(LimitMaxReleaseAmount)) && (parseInt(LimitMaxReleaseAmount) > 0)) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NotValidateReleaseAmount, 'autoclose': true })
            return false;
        }
        return true;
    }

    SelectedRiskCommercialClass(event, selectedItem) {
        riskCommercialClass = selectedItem;
    }

    SelectedHolderType(event, selectedItem) {
        switch (parseInt(selectedItem.Id)) {
            case 1:
                $('#inputReleaseAmount').prop("disabled", false);
                $('#inputReleaseAmountEsp').prop("disabled", false);
                $('#inputFreightAmount').prop("disabled", true);
                $('#inputFreightAmountEsp').prop("disabled", true);
                $('.releaseAmtlbl').addClass("field-required");
                $('.freigthAmtlbl').removeClass("field-required");
                $('#inputFreightAmount').val("");
                $('#inputFreightAmountEsp').val("");
                break;
            case 2:
                $('#inputReleaseAmount').prop("disabled", true);
                $('#inputReleaseAmountEsp').prop("disabled", true);
                $('#inputFreightAmount').prop("disabled", false);
                $('#inputFreightAmountEsp').prop("disabled", false);
                $('.freigthAmtlbl').addClass("field-required");
                $('.releaseAmtlbl').removeClass("field-required");
                $('#inputReleaseAmount').val("");
                $('#inputReleaseAmountEsp').val("");
                break;
        }
    }

    ChangeDeclarationPeriod(event, selectedItem) {
        if (firstDeclarationPeriod != null) {
            if (firstDeclarationPeriod != selectedItem) {
                if (QtyRisk > 1) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.NotModifyDeclarationPeriod, 'autoclose': true });
                }
            }
        }
    }
}