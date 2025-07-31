var isnCalculate = false;
var riskController;
var dynamicProperties = null;
var individualSearchType = 1;
var IndividualType;
var coverageIdprimary = 0;


class RiskThirdPartyLiability extends Uif2.Page {
    getInitialState() {
        riskController = "RiskThirdPartyLiability";
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskThirdPartyLiability", formRisk: "#formRiskTPL", RecordScript: false, Class: RiskThirdPartyLiability };
        }
        $("#inputRegistrationNumber").ValidatorKey(1, 1, 1);
        $("#inputPassengerQuantity").ValidatorKey(1, 1, 1);
        $("#inputYearModel").ValidatorKey(1, 1, 1);
        $("#inputGallonTankCapacity").ValidatorKey(1, 1, 1);
        $("#btnConvertProspect").hide();
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [0, 1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("#inputPlate").ValidatorKey(7, 0, 1);
        $('#inputEngine').ValidatorKey(7, 1, 1);
        $('#inputChassis').ValidatorKey(7, 1, 1);
        $('#inputRate').OnlyDecimals(UnderwritingDecimal);
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $('#inputAmountInsured').text(0);
        $('#inputPremium').text(0);
        $('input[type=text]').keyup(function () {
            $(this).val($(this).val().toUpperCase());
        });
        UnderwritingQuotation.DisabledButtonsQuotation();
        if (glbPolicy.TemporalType == 1) {
            $('#listCoverages').UifListView({ source: null, customDelete: false, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages });
        }
        else {
            $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages });
        }
        RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Risk);
        RiskThirdPartyLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        if (glbRisk.GroupCoverage != null) {
            RiskThirdPartyLiability.GetGroupCoverages(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id);
        }
        else {
            RiskThirdPartyLiability.GetGroupCoverages(glbPolicy.Product.Id, 0);
        }
        RiskThirdPartyLiability.GetCargoTypes();
        RiskThirdPartyLiability.GetRatingZonesByPrefixId(glbPolicy.Prefix.Id);
        RiskThirdPartyLiability.GetServicesTypeByProduct(glbPolicy.Product.Id);
        RiskThirdPartyLiability.GetRateTypes();
        RiskThirdPartyLiability.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
        RiskThirdPartyLiability.LoadTitle();
        RiskThirdPartyLiability.GetDeductiblesByCoverageId(coverageIdprimary);
        $('#additionalData').show();
        $("#chkIsFacultative").on("change", RiskThirdPartyLiability.ChangeIsFacultative);
        $("#chkIsRetention").on("change", RiskThirdPartyLiability.ChangeIsRetention);
        $("#chkRePoweredVehicle").on("change", RiskThirdPartyLiability.ChangeRePoweredVehicle);
        $('#inputRepoweringYear').prop("disabled", true);
    }
    bindEvents() {
        //Eventos
        $('#selectRisk').on('itemSelected', RiskThirdPartyLiability.ChangeRisk);
        $("#btnAddRisk").on('click', RiskThirdPartyLiability.AddRisk);
        $("#btnDeleteRisk").on('click', RiskThirdPartyLiability.DeleteRisk);
        $('#selectGroupCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                RiskThirdPartyLiability.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id);
            }
            else {
                $('#listCoverages').UifListView("refresh");
            }
            $('#inputPremium').text(0);
            $("#inputAmountInsured").text(0);
        });

        $('#btnCalculate').on('click', function (event) {
            isnCalculate = true;
            RiskThirdPartyLiability.GetPremium();
        });
        $('#btnDetail').on('click', function () {
            RiskThirdPartyLiability.ShowInsuredDetail();
        });
        $('#listCoverages').on('rowAdd', function (event) {
            RiskThirdPartyLiability.SaveRisk(MenuType.Coverage, 0);
        });
        $('#listCoverages').on('rowEdit', function (event, data, index) {
                if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && $('#inputPremium').text() == 0) {
                    isnCalculate = true;
                }
                RiskThirdPartyLiability.SaveRisk(MenuType.Coverage, data.Id);
           
        });

        $('#listCoverages').on('rowDelete', function (event, data) {
            RiskThirdPartyLiability.DeleteCoverage(data);
        });
        //guardar riesgo
        $('#btnAccept').on('click', function (event, redirect, index) {
            RiskThirdPartyLiability.SaveRisk(redirect, index);
            ScrollTop();
        });
        $('#btnClose').on('click', function () {
            glbRisk = { Id: 0, Object: "ObjectRiskThirdPartyLiability" };
            RiskThirdPartyLiability.UpdatePolicyComponents();
        });
        $('#btnIndividualDetailAccept').on('click', function () {
            RiskThirdPartyLiability.SetIndividualDetail();
        });

        $("#btnScript").on('click', function () {
            RiskThirdPartyLiability.LoadScript();
        });
        $("#btnAcceptNewPersonOnline").click(function () {
            glbPersonOnline = {
                Rol: 2,
                ViewModel: RiskThirdPartyLiability.GetRiskDataModel()
            };

            UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsured").val().trim());
        });
        $('#btnConvertProspect').click(function () {
            glbPersonOnline = {
                Rol: 2,
                ViewModel: RiskThirdPartyLiability.GetRiskDataModel()
            };

            UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsured").data("Object").IndividualId, $("#inputInsured").data("Object").IndividualType, $("#inputInsured").data("Object").CustomerType, 0);
        });
        $('#selectRateType').on('itemSelected', RiskThirdPartyLiability.ChangeCalculeTypeRateType);
        $("#selectMake").on('itemSelected', RiskThirdPartyLiability.ChangeMake);

        $("#inputInsured").on('buttonClick', RiskThirdPartyLiability.SearchInsured);
        $('#tableIndividualResults tbody').on('click', 'tr', RiskThirdPartyLiability.SelectIndividual);

        $("#inputRate").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
        });
        $("#inputPlate").focusout(RiskThirdPartyLiability.ValidationPlate);
    }

    static ShowPanelsRisk(Menu) {
        switch (Menu) {
            case MenuType.Risk:
                $('#modalRiskTPL').show();
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
            case MenuType.AdditionalData:
                $('#modalAdditionalData').UifModal('showLocal', AppResources.LabelTitleAdditionalData + ': ' + $('#inputPlate').val());
                break;
            case MenuType.Script:
                RiskThirdPartyLiability.LoadScript();
                break;
            default:
                break;
        }
    }
    static getOnlinePerson() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskThirdPartyLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
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
                    RiskThirdPartyLiability.GetRiskById(glbPolicy.Id, glbRisk.Id);
                }
                else {
                    RiskThirdPartyLiability.LoadViewModel(glbPersonOnline.ViewModel);
                }
                glbPersonOnline = null;
            }
        }
    }

    static GetRisksByTemporalId(temporalId, selectedId) {
        RiskThirdPartyLiabilityRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#selectRisk').UifSelect({ sourceData: data.result });
                    if (data.result.length > 0) {
                        if (selectedId > 0) {
                            $('#selectRisk').UifSelect({ sourceData: data.result, selectedId: selectedId });
                        } else if (data.result.length == 1) {
                            $('#selectRisk').UifSelect({ sourceData: data.result, selectedId: data.result[0].Id });
                            selectedId = data.result[0].Id;
                            RiskThirdPartyLiability.GetRiskById(selectedId);
                        }
                        if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                            $("#selectRisk").UifSelect('setSelected', $("#selectRisk option[Value!='']")[0].value);
                            RiskThirdPartyLiability.GetRiskById(glbPolicy.Id, $("#selectRisk option[Value!='']")[0].value);
                            RiskThirdPartyLiability.DisabledControlByEndorsementType(true);
                        }
                    }
                }
                if (glbPersonOnline != null) {
                    RiskThirdPartyLiability.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0) {
                    $("#selectRisk").UifSelect('setSelected', $("#selectRisk option[Value!='']")[0].value);
                    RiskThirdPartyLiability.GetRiskById($("#selectRisk option[Value!='']")[0].value);

                }
                else if (glbRisk.Id > 0) {
                    RiskThirdPartyLiability.GetRiskById(glbRisk.Id);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
        });
    }

    static SetInitialVehicle() {
        RiskThirdPartyLiability.GetRatingZonesByPrefixId(glbPolicy.Prefix.Id, 0);
        RiskThirdPartyLiability.GetGroupCoverages(glbPolicy.Product.Id, 0);
        RiskThirdPartyLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        RiskThirdPartyLiability.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    static ClearControls() {
        $('#selectGroupCoverage').prop('disabled', false);
        $('#inputPlate').prop('disabled', false);
        $('#inputEngine').prop('disabled', false);
        $('#inputChassis').prop('disabled', false);
        $('#selectMake').prop('disabled', false);
        $('#selectType').prop('disabled', false);
        $('#selectYear').prop('disabled', false);
        $('#selectRatingZone').prop('disabled', false);
        $('#inputInsured').prop('disabled', false);
        $('#SelectCargoType').prop('disabled', false);
        $('#selectShuttle').prop('disabled', false);
        $('#selectServiceType').prop('disabled', false);
        $('#selectDeductible').prop('disabled', false);
        $('#inputPassengerQuantity').prop('disabled', false);
        $("#chkRePoweredVehicle").prop("checked", false);
        $("#chkIsRetention").prop("checked", false);
        $("#chkIsFacultative").prop("checked", false);
        $('#inputRepoweringYear').prop("disabled", true);
        $('#chkIsRetention').prop("disabled", false);
        $('#chkRePoweredVehicle').prop("disabled", false);
        $('#inputYearModel').prop("disabled", false);
        $('#selectRisk').UifSelect('setSelected', null);
        $('#selectGroupCoverage').UifSelect('setSelected', null);
        $('#inputPlate').val('');
        $('#inputEngine').val('');
        $('#inputChassis').val('');
        $('#selectMake').UifSelect('setSelected', null);
        $('#selectType').UifSelect('setSelected', null);
        $('#selectShuttle').UifSelect('setSelected', null);
        $('#selectServiceType').UifSelect('setSelected', null);
        $('#selectDeductible').UifSelect('setSelected', null);
        $('#selectRatingZone').UifSelect('setSelected', null);
        $('#inputPassengerQuantity').val(0);
        $('#selectRateType').UifSelect('setSelected', null);
        $('#inputRate').val(0);
        $('#listCoverages').UifListView("refresh");
        $('#inputAmountInsured').text(0);
        $('#inputPremium').text(0);
        $("#hiddenOriginalRiskId").val(0);
        $('#selectedBeneficiaries').text('');
        $('#selectedTexts').text('');
        $('#selectedClauses').text('');
        $('#inputGallonTankCapacity').val(0);
        $('#inputRepoweringYear').val(0);
        $('#inputYearModel').val(0);
        isnCalculate = false;
        dynamicProperties = null;
        glbRisk.RecordScript = false;
        RiskThirdPartyLiability.UpdateGlbRisk({ Id: 0 });
        $('#selectCargoType').UifSelect('setSelected', null);
    }

    static GetRiskById(id) {
        if (id != '') {
            RiskThirdPartyLiabilityRequest.GetRiskById(glbPolicy.Endorsement.EndorsementType, glbPolicy.Id, id).done(function (data) {
                if (data.success) {
                    setTimeout(function () {
                        RiskThirdPartyLiability.LoadRisk(data.result);
                        RiskThirdPartyLiability.DisabledControlByEndorsementType(true);
                    }, 1500);
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
        if (riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        if (riskData.Risk.GroupCoverage != null) {
                $('#selectGroupCoverage').UifSelect('setSelected', riskData.Risk.GroupCoverage.Id);
        }
        $('#inputPlate').val(riskData.LicensePlate);
        $('#inputEngine').val(riskData.EngineSerial);
        $('#inputChassis').val(riskData.ChassisSerial);
        if (riskData.Make != null) {
                $('#selectMake').UifSelect('setSelected', riskData.Make.Id);
            if (riskData.Model != null) {
                if (riskData.Model.Id != 0) {
                    RiskThirdPartyLiability.GetModelsByMakeId(riskData.Make.Id, riskData.Model.Id);
                }
                else {
                    RiskThirdPartyLiability.GetModelsByMakeId(riskData.Make.Id);
                }
            }
        }
        if (riskData.Shuttle != null) {
                $('#selectShuttle').UifSelect('setSelected', riskData.Shuttle.Id);
        }
        if (riskData.ServiceType != null) {
                $('#selectServiceType').UifSelect('setSelected', riskData.ServiceType.Id);
        }
        if (riskData.Risk.RatingZone != null) {
                $('#selectRatingZone').UifSelect('setSelected', riskData.Risk.RatingZone.Id);
        }
        $('#inputPassengerQuantity').val(riskData.PassengerQuantity);
        $('#selectRateType').UifSelect('setSelected', riskData.RateType);

        $('#inputRate').val(FormatMoney(riskData.Rate));
        if (riskData.Risk != null && riskData.Risk.MainInsured != null) {
            $('#inputInsured').data('Object', riskData.Risk.MainInsured);
            $('#inputInsured').data('Detail', RiskThirdPartyLiability.GetIndividualDetails(RiskThirdPartyLiability.GetIndividualDetailsByIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CustomerType)));
            $('#inputInsured').val(riskData.Risk.MainInsured.Name + ' (' + riskData.Risk.MainInsured.IdentificationDocument.Number + ')');
        }
        $('#inputAmountInsured').text(FormatMoney(riskData.Risk.AmountInsured));
        $('#inputPremium').text(FormatMoney(riskData.Risk.Premium));
        if (riskData.Risk.Coverages != null) {
            RiskThirdPartyLiability.LoadCoverages(riskData.Risk.Coverages);
        }
        if (riskData.Deductible != null) {
            $('#selectDeductible').UifSelect('setSelected', riskData.Deductible.Id);
        }
        if (riskData.Version != null) {
            $('#hiddenVersion').val(riskData.Version.Id);
            if (riskData.Version.Type != undefined) {
                    $('#selectType').UifSelect('setSelected', riskData.Version.Type.Id);
            }
        }

        if (riskData.Model != null) {
            $('#hiddenModel').val(riskData.Model.Id);
        }

        $("#hiddenOriginalRiskId").val(riskData.Risk.RiskId);
        if (riskData.Make != null && riskData.Model != null && riskData.Version != null) {
            RiskThirdPartyLiability.GetYearsByMakeIdModelIdVersionId(riskData.Make.Id, riskData.Model.Id, riskData.Version.Id, riskData.Year);
        }

        if (riskData.Risk.Premium == 0 && (riskData.Risk.Status == RiskStatus.Original || riskData.Risk.Status == RiskStatus.Included)) {
            isnCalculate = true;
        }
        else
        {
            isnCalculate = true;
        }
        $('#inputGallonTankCapacity').val(riskData.GallonTankCapacity);
        $('#selectCargoType').UifSelect('setSelected', riskData.TypeCargoId);
        $('#inputRepoweringYear').val(riskData.RepoweringYear);
        $('#inputYearModel').val(riskData.YearModel);

        if (riskData.RePoweredVehicle == true) {
            $("#chkRePoweredVehicle").prop('checked', true);
            RiskThirdPartyLiability.ChangeRePoweredVehicle()
        }
        else
        {
            $('#inputRepoweringYear').prop("disabled", true);
            $("#chkRePoweredVehicle").prop('checked', false);
        }
        if (riskData.Risk.Retention == true) {
            $("#chkIsRetention").prop('checked', true);
        }
        else {
            $("#chkIsRetention").prop('checked', false);
        }
        if (riskData.Risk.IsFacultative == true) {
            $("#chkIsFacultative").prop('checked', true);
        }
        else {
            $("#chkIsFacultative").prop('checked', false);
        }
        dynamicProperties = riskData.Risk.DynamicProperties;
        RiskThirdPartyLiability.UpdateGlbRisk(riskData);
        RiskThirdPartyLiability.LoadSubTitles(0);
       
    }


    static GetCoveragesByProductIdGroupCoverageId(productId, groupCoverageId) {
        RiskThirdPartyLiabilityRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, productId, groupCoverageId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                RiskThirdPartyLiability.LoadCoverages(data.result.Listcoverages);
                $("#selectDeductible").UifSelect({ sourceData: data.result.ListDeductible });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    static GetPremium() {
        $('#formRiskTPL').validate();
        if ($('#formRiskTPL').valid()) {
            var recordScript = false;

            if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0) {
                recordScript = true;
            }
            else {
                recordScript = glbRisk.RecordScript;
            }

            if (recordScript) {
                var riskData = RiskThirdPartyLiability.GetRiskDataModel();
                RiskThirdPartyLiabilityRequest.GetPremium(glbPolicy.Id, riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                    if (data.success) {
                        RiskThirdPartyLiability.LoadRisk(data.result);
                    }
                    else
                    {
                        $('#inputPremium').text(0);
                        isnCalculate = false;
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculatePremium, 'autoclose': true });
                });
            }
            else {
                RiskThirdPartyLiability.LoadScript();
            }
        }
    }

    static ShowInsuredDetail() {
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($('#inputInsured').data('Detail') != null) {
            var insuredDetails = $('#inputInsured').data('Detail');

            if (insuredDetails.length > 0) {
                $.each(insuredDetails, function (id, item) {
                    $('#tableIndividualDetails').UifDataTable('addRow', item);
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

    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else {
            var coverages = $('#listCoverages').UifListView('getData');
            $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages });

            $.each(coverages, function (index, value) {
                if (this.Id != data.Id) {
                    $('#listCoverages').UifListView('addItem', this);
                }
                else {
                    if (data.EndorsementType == EndorsementType.Modification && data.RiskCoverageId > 0) {
                        var coverage = RiskThirdPartyLiability.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                        if (coverage != null) {
                            coverage.Rate = FormatMoney(coverage.Rate);
                            coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                            $('#listCoverages').UifListView('addItem', coverage);
                        }
                    }
                }
            });
            RiskThirdPartyLiability.UpdatePremium();
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskThirdPartyLiabilityRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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

    static SaveRisk(redirec, coverageId) {
        if ($('#formRiskTPL').valid()) {
            if (isnCalculate === true) {
                var riskData = RiskThirdPartyLiability.GetRiskDataModel();
                RiskThirdPartyLiabilityRequest.SaveRisk(glbPolicy.Id, riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                             $.UifProgress('close');
                             currentSavedRisk = data.result.Risk;
                             if (data.result.Alerts != null && data.result.Alerts.length > 0) {
                            if (data.result.Alerts[0].startsWith("**")) {
                                //mostrar como cuadro de dialogo
                                $.UifDialog('alert',
                                    { 'message': data.result.Alerts[0] },
                                    function (result) {
                                        //No hacer nada, sólo informativo
                                    });
                                $('.modal-body.modal-body-dialog-alert p').prop('style', 'white-space: pre-line')
                            } else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result.Alerts[0], 'autoclose': true });
                            }
                        }
                             RiskThirdPartyLiability.UpdateGlbRisk(data.result);
                             RiskThirdPartyLiability.LoadSubTitles(0);
                             RiskThirdPartyLiability.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                        }
                        else
                        {
                        $.UifProgress('close');
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifProgress('close');
                        if (Array.isArray(data.result)) {
                            $.each(data.result, function (key, value) {
                                $.UifNotify('show', { 'type': 'danger', 'message': value, 'autoclose': true });
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
                                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            }
                        }
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifProgress('close');
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                });
            }
            else
            {
                $.UifProgress('close');
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.FirstValueRiskCalculated, 'autoclose': true });
            }
        }
    }

    static Redirect(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskThirdPartyLiability.ReturnUnderwriting();
                break;
            case MenuType.Coverage:
                RiskThirdPartyLiability.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.Texts:
                RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Script:
                RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
        }
    }

    static ReturnUnderwriting() {
        var url = '';
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "ThirdPartyLiabilityModification";
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
            Object: "RiskThirdPartyLiabilityCoverage",
            Class: RiskThirdPartyLiabilityCoverage

        }

        router.run("prtCoverageRiskThirdPartyLiability");
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

        RiskThirdPartyLiabilityRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });
        });
        return resultData;
    }

    static LoadCoverages(coverages) {
        if (glbPolicy.TemporalType == 1) {
            $('#listCoverages').UifListView({ source: null, customDelete: false, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages });
        }
        else {
            $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages });
        }
        $.each(coverages, function (index, val) {
                coverages[index].DeclaredAmount = FormatMoney(coverages[index].DeclaredAmount);
                coverages[index].LimitAmount = FormatMoney(coverages[index].LimitAmount);
                coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
                coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);
                coverages[index].DisplayRate = FormatMoney(coverages[index].Rate);
                if (coverages[index].AllyCoverageId !=null) {
                    coverages[index].allowEdit = false;
                    coverages[index].allowDelete = false;
                } else {
                    coverages[index].allowEdit = true;
                    coverages[index].allowDelete = true;
                }
                coverages[index].TypeCoverage = RiskThirdPartyLiability.ValidateTypeCoverage(this);

                $('#listCoverages').UifListView('addItem', coverages[index]);
        });

    }

    static DeleteRisk() {
        if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
            if ($('#selectRisk').UifSelect('getSelected') > 0) {
                RiskThirdPartyLiabilityRequest.DeleteRisk(glbPolicy.Id, $('#selectRisk').UifSelect('getSelected')).done(function (data) {
                    if (data.success) {
                        if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                            RiskThirdPartyLiability.ClearControls();
                            RiskThirdPartyLiability.GetRisksByTemporalId(glbPolicy.Id, 0);
                            RiskThirdPartyLiability.LoadTitle();
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                            ScrollTop();
                        }
                        else {
                            RiskThirdPartyLiability.GetRisksByTemporalId(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected"));
                            RiskThirdPartyLiability.GetRiskById(glbPolicy.Id, $("#selectRisk").UifSelect("getSelected"));
                            RiskThirdPartyLiability.LoadTitle();
                        }
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDeleteRisk, 'autoclose': true });
                });
            }
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.WarningDeleteRisk, 'autoclose': true });
        }

    }

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static GetModelsByMakeId(makeId, selectedId) {
        RiskThirdPartyLiabilityRequest.GetModelsByMakeId(makeId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $('#hiddenModel').val(data.result[0].Id);
                    RiskThirdPartyLiability.GetVersionsByMakeIdModelId($("#selectMake").UifSelect("getSelected"), $('#hiddenModel').val(), 0);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetVersionsByMakeIdModelId(makeId, modelId) {
        RiskThirdPartyLiabilityRequest.GetVersionsByMakeIdModelId(makeId, modelId).done(function (data) {
            if (data.success) {
                $('#hiddenVersion').val(data.result[0].Id);
                RiskThirdPartyLiability.GetYearsByMakeIdModelIdVersionId($('#selectMake').UifSelect('getSelected'), $('#hiddenModel').val(), $('#hiddenVersion').val(), 0);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': 'no exist', 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchFasecolda, 'autoclose': true });
        });
    }

    static GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId, selectedId) {
        isnCalculate = false;

        RiskThirdPartyLiabilityRequest.GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $('#hiddenYear').val(data.result[0].Description);
                    if (selectedId == 0) {
                        $('#selectYear').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $('#selectYear').UifSelect({ sourceData: data.result, selectedId: selectedId });
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

    static DisabledControlByEndorsementType(disabled) {
        var endorsementType = null;
        if (glbPolicy.Endorsement != null) {
            endorsementType = parseInt(glbPolicy.Endorsement.EndorsementType, 10);
        }
        else {
            endorsementType = parseInt(glbPolicy.EndorsementType, 10);
        }
        switch (endorsementType) {
            case EndorsementType.Renewal: {
                $('#selectGroupCoverage').prop('disabled', disabled);
                $('#inputPlate').prop('disabled', disabled);
                $('#inputEngine').prop('disabled', disabled);
                $('#inputChassis').prop('disabled', disabled);
                $('#selectMake').prop('disabled', disabled);
                $('#selectType').prop('disabled', disabled);
                $('#selectYear').prop('disabled', disabled);
                $('#selectRatingZone').prop('disabled', disabled);
                $('#inputInsured').prop('disabled', disabled);
                break;
            }
            case EndorsementType.Modification: {
                $('#selectGroupCoverage').prop('disabled', disabled);
                $('#inputPlate').prop('disabled', disabled);
                $('#inputEngine').prop('disabled', disabled);
                $('#selectMake').prop('disabled', disabled);
                $('#selectYear').prop('disabled', disabled);
                $('#selectRatingZone').prop('disabled', disabled);
                $('#selectShuttle').prop('disabled', disabled);
                $('#selectServiceType').prop('disabled', disabled);
                $('#selectDeductible').prop('disabled', disabled);
                $('#inputRepoweringYear').prop("disabled", disabled);
                $('#chkIsRetention').prop("disabled", disabled);
                $('#chkIsFacultative').prop("disabled", disabled);
                $('#inputPassengerQuantity').prop('disabled', disabled);
                $('#chkRePoweredVehicle').prop("disabled", disabled);
                $('#inputYearModel').prop("disabled", disabled);

                break;
            }
            default:
                break;
        }
    }

    static UpdatePremium() {
        var premium = 0;
        var totalLimitAmount = 0;

        $.each($('#listCoverages').UifListView('getData'), function (key, value) {
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
            totalLimitAmount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
        });

        $('#inputAmountInsured').text(FormatMoney(totalLimitAmount));
        $('#inputPremium').text(FormatMoney(premium));
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
        if ($('#formRiskTPL').valid() && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, dynamicProperties);
        }
    }

    static GetRiskDataModel() {
        var riskData = $('#formRiskTPL').serializeObject();
        riskData.TemporalId = glbPolicy.Id;
        riskData.PrefixId = glbPolicy.Prefix.Id;
        riskData.ProductId = glbPolicy.Product.Id;
        riskData.PolicyTypeId = glbPolicy.PolicyType.Id;
        riskData.InsuredId = glbPolicy.Holder.IndividualId;
        riskData.HolderId = glbPolicy.Holder.IndividualId;
        riskData.EndorsementType = glbPolicy.Endorsement.EndorsementType;
        riskData.Id = glbRisk.Id;
        riskData.RiskId = glbRisk.Id;
       
        riskData.MakeDescription = $('#selectMake').UifSelect('getSelectedText');
        riskData.RateType = $('#selectRateType').UifSelect('getSelected');
        riskData.AmountInsured = NotFormatMoney($('#inputAmountInsured').text());
        riskData.Premium = NotFormatMoney($('#inputPremium').text());

        if ($('#inputInsured').data("Object") != null) {
            riskData.InsuredName = ReplaceCharacter($('#inputInsured').data("Object").Name);
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
         riskData.TrailerQuantity = glbRisk.TrailerQuantity;
         riskData.Tons = glbRisk.Tons;
         riskData.PhoneNumber = glbRisk.PhoneNumber;
        var coverages = $('#listCoverages').UifListView('getData');
        riskData.IsFacultative = $("#chkIsFacultative").prop('checked');
        riskData.IsRetention = $("#chkIsRetention").prop('checked');
        $.each(coverages, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);              
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
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
            this.TypeCoverage = this.TypeCoverage;

        });
        riskData.Coverages = coverages;
        riskData.RePoweredVehicle = $("#chkRePoweredVehicle").prop('checked');
        riskData.GallonTankCapacity = $('#inputGallonTankCapacity').val();
        riskData.Model = $('#hiddenModel').val();
        riskData.TypeCargoId = $('#selectCargoType').UifSelect('getSelected');
        riskData.RepoweringYear = $('#inputRepoweringYear').val();
        riskData.YearModel = $('#inputYearModel').val();
        riskData.IndividualType = $('#inputInsured').data("Object").IndividualType;

        if (riskData.Shuttle == "" || riskData.Shuttle == null || riskData.Shuttle== undefined) {
            riskData.Shuttle = 1;
        }
        if (riskData.ServiceType == "" || riskData.ServiceType == null || riskData.ServiceType == undefined) {
            riskData.ServiceType = 8;// valor por defecto para tipo de servicio se genera insert en bd  
        }
        return riskData;
    }

    static ShowSaveRisk(riskId, redirec, coverageId) {
        if (redirec != 9) {
            if ($("#selectRisk").UifSelect("getSelected") == null || $("#selectRisk").UifSelect("getSelected") == 0 || $("#selectRisk").UifSelect("getSelected") == "") {
                RiskThirdPartyLiability.GetRisksByTemporalId(glbPolicy.Id, riskId);
                RiskThirdPartyLiability.LoadTitle();
            }
            
            
        }
        var events = null;
        if (glbRisk.InfringementPolicies != null) {

            /// lanza los eventos para la creación de el riesgo

            events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
            if (events !== TypeAuthorizationPolicies.Restrictive) {
                RiskThirdPartyLiability.Redirect(redirec, riskId, coverageId);
            }
        }
        /// fin - lanza los eventos para la creación de el riesgo
    }

    static UpdateGlbRisk(data) {
        glbRisk = data;
        $.extend(glbRisk, data.Risk);
        glbRisk.Object = "ObjectRiskThirdPartyLiability";
        formRisk: "#formRiskTPL";
        glbRisk.Class = RiskThirdPartyLiability;
    }
    
    static GetGroupCoverages(productId, selectedId) {
        var controller = rootPath + "Underwriting/RiskThirdPartyLiability/GetGroupCoverages?productId=" + productId;
        RiskThirdPartyLiabilityRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result });
                }
                else
                {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else
            {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetServicesTypeByProduct(productId) {
        RiskThirdPartyLiabilityRequest.GetServicesTypeByProduct(productId).done(function (data) {
            if (data.success) {
                if (glbRisk.ServiceType == null) {
                    $("#selectServiceType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectServiceType").UifSelect({ sourceData: data.result, selectedId: glbRisk.ServiceType.Id });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryServiceTypes, 'autoclose': true });
        });
    }

    
    static GetRatingZonesByPrefixId(prefixId, selectedId) {

        RiskThirdPartyLiabilityRequest.GetRatingZonesByPrefixId(prefixId).done(function (data) {
            if (data.success) {
                if (glbRisk.RatingZone == null) {
                    $("#selectRatingZone").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRatingZone").UifSelect({ sourceData: data.result, selectedId: glbRisk.RatingZone.Id });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryChargingZones, 'autoclose': true });
        });
    }

    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbRisk.RecordScript = isModify;
        if (isModify) {
            RiskThirdPartyLiability.GetPremium();
        }
    }

    static RunRules(ruleSetId) {

        RiskThirdPartyLiabilityRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            if (data.success) {
                RiskThirdPartyLiability.LoadRisk(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
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
                RiskThirdPartyLiability.GetIndividualDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
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
                RiskThirdPartyLiability.GetIndividualDetailsByIndividualId(insured.IndividualId, insured.CustomerType)
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

        if (viewModel.GroupCoverage != null) {
            $('#selectGroupCoverage').UifSelect('setSelected', viewModel.GroupCoverage);
        }
        $('#inputPlate').val(viewModel.LicensePlate);
        $('#inputEngine').val(viewModel.Engine);
        $('#inputChassis').val(viewModel.Chassis);

        RiskThirdPartyLiability.GetMakes(viewModel.Make);
        if (viewModel.Make != "") {
            RiskThirdPartyLiability.GetModelsByMakeId(viewModel.Make, viewModel.Model);
            if (viewModel.Model != "") {
                RiskThirdPartyLiability.GetVersionsByMakeIdModelId(viewModel.Make, viewModel.Model, viewModel.Version);
            }
            else {
                RiskVehicle.GetVersionsByMakeIdModelId(viewModel.Make, viewModel.Model, 0);
            }
        }

        if (viewModel.TypeVehicle != null) {
            $('#selectType').UifSelect('setSelected', viewModel.TypeVehicle);
        }
        if (viewModel.Shuttle != null) {
            $('#selectShuttle').UifSelect('setSelected', viewModel.Shuttle);
        }
        if (viewModel.ServiceType != null) {
            $('#selectServiceType').UifSelect('setSelected', viewModel.ServiceType);
        }
        if (viewModel.Deductible != null) {
            $('#selectDeductible').UifSelect('setSelected', viewModel.Deductible);
        }
        if (viewModel.RatingZone != null) {
            $('#selectRatingZone').UifSelect('setSelected', viewModel.RatingZone);
        }
        if (viewModel.TypeCargoId != null) {
            $('#SelectCargoType').UifSelect('setSelected', viewModel.TypeCargoId);
        }
        $('#inputPassengerQuantity').val(viewModel.PassengerQuantity);
        $('#selectRateType').UifSelect('setSelected', viewModel.RateType);
        $('#inputRate').val(FormatMoney(viewModel.Rate));

        $('#inputAmountInsured').text(FormatMoney(viewModel.AmountInsured));
        $('#inputPremium').text(FormatMoney(viewModel.Premium));
        if (viewModel.Coverages != null) {
            RiskThirdPartyLiability.LoadCoverages(viewModel.Coverages);
        }
        if (viewModel.Model != null) {
            $('#hiddenModel').val(viewModel.Model);
        }
        if (viewModel.Version != null) {
            $('#hiddenVersion').val(viewModel.Version);
        }
        $("#hiddenOriginalRiskId").val(viewModel.RiskId);
        if (viewModel.Make != "" && viewModel.Model != null && !isEmpty(viewModel.Version)  ) {
            RiskThirdPartyLiability.GetYearsByMakeIdModelIdVersionId(viewModel.Make, viewModel.Model, viewModel.Version, viewModel.Year);
        }

        if (viewModel.Premium == 0 && (viewModel.Status == RiskStatus.Original || viewModel.Status == RiskStatus.Included)) {
            isnCalculate = false;
        }
        else {
            isnCalculate = true;
        }

        RiskThirdPartyLiability.LoadSubTitles(0);
    }

    static UpdatePolicyComponents() {

        RiskThirdPartyLiabilityRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                RiskThirdPartyLiability.ReturnUnderwriting();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });
    }

    static ChangeRisk(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskThirdPartyLiability.GetRiskById(selectedItem.Id);
            RiskThirdPartyLiability.DisabledControlByEndorsementType(true);
        }
        else {
            RiskThirdPartyLiability.ClearControls();
        }
    }

    static AddRisk() {
        RiskThirdPartyLiability.ClearControls();
        RiskThirdPartyLiability.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
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

    static ChangeCalculeTypeRateType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var Rate = parseInt(selectedItem.Id);
            switch (Rate) {
                case RateType.Percentage:
                    $("#inputRate").attr("maxLength", 8);
                    break;
                case RateType.Permilage:
                    $("#inputRate").attr("maxLength", 9);
                    break;
                default:
                    //FixedValue
                    $("#inputRate").attr("maxLength", 20);
                    break;
            }
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

    static ChangeIsRetention() {
        if ($("#chkIsRetention").prop("checked")) {
            $("#chkIsFacultative").prop('checked', false);
            $('#chkIsFacultative').prop("disabled", true);
        } else {
            $("#chkIsFacultative").prop("disabled", false);
            $('#chkIsRetention').prop("disabled", false);
        }
    }
    static ChangeIsFacultative() {
        if ($("#chkIsFacultative").prop("checked")) {
            $("#chkIsRetention").prop('checked', false);
            $('#chkIsRetention').prop("disabled", true);
        } else {
            $("#chkIsFacultative").prop("disabled", false);
            $('#chkIsRetention').prop("disabled", false);
        }
    }

    static ChangeMake(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskThirdPartyLiability.GetModelsByMakeId(selectedItem.Id, 0);
        }
    }

    static GetCargoTypes() {
        RiskThirdPartyLiabilityRequest.GetCargoTypes().done(function (data) {
            if (data.success) {
                if (glbRisk.CargoType == null) {
                    $('#selectCargoType').UifSelect({ sourceData: data.result });
                }
                else {
                    $('#selectCargoType').UifSelect({ sourceData: data.result, selectedId: glbRisk.CargoType });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });
    }

    static ChangeModel(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskThirdPartyLiability.GetVersionsByMakeIdModelId($("#selectMake").UifSelect("getSelected"), selectedItem.Id, 0);
        }
        else {
            $('#selectVersion').UifSelect();
            $('#selectYear').UifSelect();
            $("#selectType").UifSelect();
            $('#inputPrice').val(0);
        }
        $('#inputFasecoldaCode').val('');
    }
    static ChangeRePoweredVehicle() {
        if ($("#chkRePoweredVehicle").prop("checked")) {

            $('#inputRepoweringYear').prop("disabled", false);
        }
        else {
            $('#inputRepoweringYear').val(0);
            $('#inputRepoweringYear').prop("disabled", true);
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
                        RiskThirdPartyLiability.LoadInsured(data.result[0]);
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
                        RiskThirdPartyLiability.ShowModalList(dataList);
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

    static ValidateTypeCoverage(model) {
        if (model.IsPrimary) {
            coverageIdprimary = model.Id;
            return AppResources.CoveragePrincipal;
        }
        else if (model.MainCoverageId > 0 && model.AllyCoverageId == null) {
            return AppResources.CoverageAdditional;
        }
        else if (model.AllyCoverageId > 0 && model.AllyCoverageId != null) {
            return AppResources.CoverageAllied
        }
        else {
            return AppResources.CoverageBasic
        }
    }
    //#region Asegurado detalle
    static SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            RiskThirdPartyLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);
        }
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
    static SelectIndividual(e) {
        RiskThirdPartyLiability.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        $('#modalIndividualSearch').UifModal('hide');
    }

    static GetRateTypes() {
        //Se habilita si el producto seleccionado opera tasa única.
        if (glbPolicy.Product.IsFlatRate == true) {
            $('#inputRate').prop('disabled', false);
            RiskThirdPartyLiabilityRequest.GetRateTypes().done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $('#selectRateType').UifSelect({ sourceData: data.result });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryRateTypes, 'autoclose': true });
            });
        } else
        {
            $('#inputRate').prop('disabled', true);
        } 
    }
    //#endregion

    static ValidationPlate() {
        var regex = /(^[a-zA-Z]{3}[0-9]{3}$)/;
        var regex2 = /(^[a-zA-Z]{3}[0-9]{2}[a-zA-Z]{1}$)/
        var regex3 = /(^[rRsS]{1}[0-9]{5}$)/
        var regex4 = /(^[a-zA-z]{2}[0-9]{4}$)/
        if (!regex.test($("#inputPlate").val()) && !regex2.test($("#inputPlate").val()) && !regex3.test($("#inputPlate").val()) && !regex4.test($("#inputPlate").val())) {
            $.UifNotify('show', { 'type': 'danger', 'message': "El formato de placa no es valido", 'autoclose': true });
        } else {
            return true;
        }
    }

    static ValidateCoverageRate() {
        var type = $("#selectRateType").UifSelect("getSelected");
        type = parseInt(type);
        var rate = parseFloat($("#inputRate").val().replace(',', '.'));

        if (rate > 0) {
            switch (type) {
                case RateType.FixedValue:
                    return true;
                case RateType.Percentage:
                case RateType.Permilage:
                    if (rate > 100 || rate < 0 || rate == 0) {
                        $("#inputRateCoverage").val(0);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValueRate, 'autoclose': true });
                        return false
                    } else {
                        return true;
                    }
            }
        }
        return false;
    }


    static GetDeductiblesByCoverageId(coverageId) {
        if (coverageIdprimary > 0)
        {
            RiskThirdPartyLiabilityRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
                if (data.success) {
                if (glbRisk.Deductible == null)
                {
                    $('#selectDeductible').UifSelect({ sourceData: data.result });
                }
                else
                {
                    $('#selectDeductible').UifSelect({ sourceData: data.result, selectedId: glbRisk.Deductible.Id });
                }
            }
                else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
             }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingDeductibles, 'autoclose': true });
            });
        }
    }
}