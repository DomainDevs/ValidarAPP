var policyConsulted = false;
var dataCatastrophe = {};
var coveredRiskType = null;
var noticeId = null;
var policyId = null;
var claimModifyId = null;
var endorsementId = null;
var coverageClaimId = null;
var SalaryMinimumMounth = null;
var policyBusinessTypeId = 0;
var policyTypeId = 0;
var policyProductId = 0;
var policyCurrency = null;
var policyCurrencyDescription = null;
var policyHolderId = 0;
var companyParticipationPercentage = 0;
var exchangeRate = null;
var endorsements = [];

var modelSearchCriteria = {
    claimId: "",
    prefixId: ""
};
var individualId = 0;
class Claim extends Uif2.Page {

    getInitialState() {
        Claim.InitialClaim();
    }

    bindEvents() {
        // Buttons - ClaimPartial
        $("#btnPolicy").on('click', Claim.OpenModalPolicyConsult);
        $("#btnAdjusterAnalyst").on('click', Claim.OpenModalAdjusterAnalyst);
        $("#btnOpenEventCatastrophic").on('click', Claim.OpenEventCatastrophic);
        $('#btnOpenCoInsurance').on('click', Claim.OpenCoInsuranceCompany);
        $('input[type=radio][name=coinsurance]').on('change', Claim.ChangeCoinsurance);
        $('#btnRecord').on('click', Claim.SaveCatastrophic);
        $('#btnSaveAdjusterAnalyst').on('click', Claim.SaveAdjusterAnalyst);
        $('#btnClaimEstimations').on('click', Claim.ViewClaimEstimations);
        $('#btnCloseEstimation').on('click', Claim.CloseEstimation);
        $('#btnShowClaim').on('click', Claim.LoadClaim);

        // Selects - ClaimPartial
        $('#selectPrefix').on('itemSelected', this.SelectedPrefix);
        $('#selectCountry').on('itemSelected', this.selectCountry);
        $('#selectDepartment').on('itemSelected', this.selectDepartment);

        $("#selectEndorsement").on('itemSelected', Claim.LoadEndorsementData);

        $("#ddlCountry").on('itemSelected', this.selectClaimCountry);
        $("#ddlState").on('itemSelected', this.selectClaimDepartment);

        // Inputs  - ClaimPartial
        $('#InputPolicyNumber').on('buttonClick', Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber);
        $('#inputCatastrophe').on('itemSelected', function (event, selectedItem) {
            Claim.selectCatastrophe(event, selectedItem);
        });
        $('#InputDateIncident').on("datepicker.change", function (event, date) {
            Claim.ValidateClaimOccurrenceAndNoticeDate();
            Claim.ClearPolicyInformation();
        });

        $('#InputDateNotice').on("datepicker.change", function (event, date) {
            Claim.ValidateClaimOccurrenceAndNoticeDate();
        });

        $("#timepickerFrom").UifTimepicker('setValue', '00:00:00 PM');
        $("#timepickerTo").UifTimepicker('setValue', '00:00:00 PM');
        $("#selectHour").UifTimepicker('setValue', '00:00:00 PM');
        $("#InputTimeIncident").UifTimepicker('setValue', '00:00:00 PM');
        $('#InputClaimDetail').TextTransform(ValidatorType.UpperCase);
        $('#InputInfringementDescription').TextTransform(ValidatorType.UpperCase);
        $('#InputAddress').TextTransform(ValidatorType.UpperCase);
        $('#ClaimLocation').TextTransform(ValidatorType.UpperCase);
    }

    static InitialClaim() {
        $('#titleClaimVehicle').hide();
        $("#vehicleClaimNumberEdit").hide();
        $('#titleClaimLocation').hide();
        $("#locationClaimNumberEdit").hide();
        $('#titleClaimSurety').hide();
        $("#suretyClaimNumberEdit").hide();
        $('#titleClaimTransport').hide();
        $("#transportClaimNumberEdit").hide();
        $('#titleClaimAirCraft').hide();
        $("#airCraftClaimNumberEdit").hide();
        $('#titleClaimFidelity').hide();
        $("#fidelityClaimNumberEdit").hide();
        $("#rdbCompanyParticipation").prop("disabled", true);
        $("#rdbTotal").prop("disabled", true);
        ClaimRequest.GetDamageTypes().done(function (data) {
            if (data.success) {
                $('#selecDamageClassification').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetDamageResponsibilities().done(function (data) {
            if (data.success) {
                $('#selectDamageResponsibility').UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetAnalyst().done(function (data) {
            if (data.success) {
                $('#selectAnalyst').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetAdjuster().done(function (data) {
            if (data.success) {
                $('#selectAjuster').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetInvestigator().done(function (data) {
            if (data.success) {
                $('#selectInvestigator').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetCountries().done(function (data) {
            if (data.success) {
                $('#selectCountry').UifSelect({ sourceData: data.result });
                $('#ddlCountry').UifSelect({ sourceData: data.result });

                if (noticeToClaim == null && modelSearchCriteria.claimId == null) {
                    ClaimRequest.GetDefaultCountry().done(function (data) {
                        if (data.success) {
                            $('#selectCountry').UifSelect("setSelected", data.result);
                            $('#ddlCountry').UifSelect("setSelected", data.result);
                            $('#selectCountry').trigger("change");
                            $('#ddlCountry').trigger("change");
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetAccountingDate().done(function (data) {
            if (data.success) {
                $("#InputAccountingDate").UifDatepicker('setValue', FormatDate(data.result));
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetAmountType().done(function (data) {
            if (data.success) {
                $('#selectAmountType').UifSelect({ sourceData: data.result });
                $('#selectAmountType').UifSelect('setSelected', 1);
                $("#inpuntSalaryEquivalent").parent().hide();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetMinimumSalaryByYear(moment().format("YYYY")).done(function (data) {
            if (data.success) {
                SalaryMinimumMounth = data.result.SalaryMinimumMounth;
            }
            else {
                $("#selectAmountType").UifSelect('disabled', true);
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        $('#ClaimVehiclePartial').hide();
        $('#ClaimLocationPartial').hide();
        $('#ClaimSuretyPartial').hide();
        $('#ClaimTransportPartial').hide();
        $('#ClaimAirCraftPartial').hide();
        $('#ClaimFidelityPartial').hide();
        $('#EstimationsSummaryAircraftPartial').hide();
        $('#EstimationsSummaryVehiclePartial').hide();
        $('#EstimationsSummaryLocationPartial').hide();
        $('#EstimationsSummarySuretyPartial').hide();
        $('#EstimationsSummaryTransportPartial').hide();
        $('#EstimationsSummaryFidelityPartial').hide();
        $('#titleEstimations').hide();

        if (modelSearchCriteria.claimId == null) {
            $('#InputDateNotice').UifDatepicker('setValue', GetCurrentFromDate());
        }

        $('#InputAccountingDate').UifDatepicker('disabled', true);
        $('#pNoRecords').show();
        $('#btnCloseEstimation').hide();
        $('#claimEstimation').hide();
        $('#labelEstimation').text("0");
        $('#labelPayments').text("0");
        $('#labelIncome').text("0");
        $('#labelReservations').text("0");
        $('#InputClaimNumber').prop('disabled', true);
        $('#InputClaimDate').prop('disabled', true);
        $('#_damageCode').val('');
        $('#_damageResponsabilitie').val('');
        $('#_policyId').val('');
        policyBusinessTypeId = 0;
        policyProductId = 0;
        policyCurrency = null;
        policyTypeId = 0;
        $('#_endorsementId').val('');
        $('#_riskId').val('');
        $('#_number').val('');
        $('#InputAffectedProperty').val('');
        $('#inputDriverDocumentNumber').val('');
        $('#inputDriverFullName').val('');
        $('#_claimId').val('');
        $('#divtableSummaryClaim').hide();
        LoadTime('selectHour', 24);
        $('.hideSearchClaim').hide();
    }

    static GetClaimSupplier() {
        ClaimRequest.GetClaimSupplierByClaimId(parseInt($('#_claimId').val())).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.AnalizerId > 0) {
                        $('#selectAnalyst').UifSelect('setSelected', data.result.AnalizerId.toString());
                    }
                    if (data.result.AdjusterId > 0) {
                        $('#selectAjuster').UifSelect('setSelected', data.result.AdjusterId.toString());
                    }
                    if (data.result.ResearcherId > 0) {
                        $('#selectInvestigator').UifSelect('setSelected', data.result.ResearcherId.toString());
                    }

                    $('#InputClaimDate').UifDatepicker('setValue', FormatDate(data.result.DateInspection));
                    $('#InputInspectionDate').UifDatepicker('setValue', FormatDate(data.result.DateInspection));
                    $('#InputClaimNumber').val($('#_number').val());
                    $('#InputPropertyAffected').val(data.result.AffectedProperty);
                    $('#InputDescriptionOfLost').val(data.result.LossDescription);
                } else {
                    $('#selectAnalyst').UifSelect('setSelected', null);
                    $('#selectAjuster').UifSelect('setSelected', null);
                    $('#selectInvestigator').UifSelect('setSelected', null);
                    $('#InputInspectionDate').UifDatepicker('setValue', "");
                    $('#InputPropertyAffected').val("");
                    $('#InputClaimNumberId').val("");
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimRequest.GetClaimCatastrophicInformationByClaimId(parseInt($('#_claimId').val())).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    var result = data.result;
                    $('#pNoRecords').hide();
                    $('#pEventRecords').show();
                    $('#inputCatastrophe').val(result.Description);
                    $('#InputDateTimeFrom').UifDatepicker('setValue', FormatDate(result.DateTimeFrom));
                    $('#InputDateTimeTo').UifDatepicker('setValue', FormatDate(result.DateTimeTo));
                    $('#InputInfringementDescription').val(result.CatastropheDescription);
                    $('#InputAddress').val(result.Address);

                    ClaimRequest.GetStatesByCountryId(result.City.State.Country.Id).done(function (dataState) {
                        if (dataState.success) {
                            if (dataState.result != null) {
                                $('#selectDepartment').UifSelect({ sourceData: dataState.result });
                                $('#selectDepartment').UifSelect('setSelected', result.City.State.Id);

                                ClaimRequest.GetCitiesByCountryIdStateId(result.City.State.Country.Id, result.City.State.Id).done(function (dataCitie) {
                                    if (dataCitie.success) {
                                        if (dataCitie.result != null) {
                                            $('#selectCity').UifSelect({ sourceData: dataCitie.result });
                                            $('#selectCity').UifSelect('setSelected', result.City.Id);
                                            $('#selectCountry').UifSelect('setSelected', result.City.State.Country.Id);
                                        }
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'danger', 'message': dataCitie.result, 'autoclose': true });
                                    }
                                });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': dataState.result, 'autoclose': true });
                        }
                    });

                } else {
                    $('#inputCatastrophe').val("");
                    $('#InputDateTimeFrom').UifDatepicker('setValue', "");
                    $('#InputDateTimeTo').UifDatepicker('setValue', "");
                    $('#InputInfringementDescription').val("");
                    $('#InputAddress').val("");
                    $('#selectCountry').UifSelect('setSelected', "");
                    $('#selectDepartment').UifSelect('setSelected', "");
                    $('#selectCity').UifSelect('setSelected', "");
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        Claim.AddDataTableSummaryClaim();
    }

    static selectCatastrophe(event, selectedItem) {
        dataCatastrophe = selectedItem;
    }

    ////////////////////////
    ///Sección Claim Partial
    static GetBranches() {
        ClaimRequest.GetBranches().done(function (data) {
            if (data.success) {
                $('#selectBranchClaim').UifSelect({ sourceData: data.result });
                $('#selectBranch').UifSelect({ sourceData: data.result });

                switch (glClaimOption) {
                    case 1:
                        if (modelSearchCriteria.number == null) {
                            ClaimVehicle.preLoadClaimVehicle();
                        } else {
                            Claim.preNewClaim();
                        }
                        break;
                    case 2:
                        if (modelSearchCriteria.number == null) {
                            ClaimLocation.preLoadClaimLocation();
                        } else {
                            Claim.preNewClaim();
                        }
                        break;
                    case 3:
                        if (modelSearchCriteria.number == null) {
                            ClaimSurety.preLoadClaimSurety();
                        } else {
                            Claim.preNewClaim();
                        }
                        break;
                    case 4:
                        if (modelSearchCriteria.number == null) {
                            ClaimTransport.preLoadClaimTransport();
                        } else {
                            Claim.preNewClaim();
                        }
                        break;
                    case 5:
                        if (modelSearchCriteria.number == null) {
                            ClaimAirCraft.preLoadClaimAirCraft();
                        } else {
                            Claim.preNewClaim();
                        }
                        break;
                    case 6:
                        if (modelSearchCriteria.number == null) {
                            ClaimFidelity.preLoadClaimFidelity();
                        } else {
                            Claim.preNewClaim();
                        }
                        break;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static preNewClaim() {
        $("#InputDateIncident").UifDatepicker('setValue', moment().format("DD/MM/YYYY"));
        $('#selectBranchClaim').UifSelect('setSelected', modelSearchCriteria.branchId);
        $('#selectBranch').UifSelect('setSelected', modelSearchCriteria.branchId);
        $('#InputPolicyNumber').val(modelSearchCriteria.number);

        switch (glClaimOption) {
            case 1:
                ClaimVehicle.GetPrefixesByCoveredRiskType(function (prefixes) {
                    $("#selectPrefix").UifSelect({ sourceData: prefixes });
                    $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
                    Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
                });
                break;
            case 2:
                ClaimLocation.GetPrefixesByCoveredRiskType(function (prefixes) {
                    $("#selectPrefix").UifSelect({ sourceData: prefixes });
                    $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
                    Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
                });
                break;
            case 3:
                ClaimSurety.GetPrefixesByCoveredRiskType(function (prefixes) {
                    $("#selectPrefix").UifSelect({ sourceData: prefixes });
                    $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
                    Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
                });
                break;
            case 4:
                ClaimTransport.GetPrefixesByCoveredRiskType(function (prefixes) {
                    $("#selectPrefix").UifSelect({ sourceData: prefixes });
                    $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
                    Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
                });
                break;
            case 5:
                ClaimAirCraft.GetPrefixesByCoveredRiskType(function (prefixes) {
                    $("#selectPrefix").UifSelect({ sourceData: prefixes });
                    $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
                    Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
                });
                break;
            case 6:
                ClaimFidelity.GetPrefixesByCoveredRiskType(function (prefixes) {
                    $("#selectPrefix").UifSelect({ sourceData: prefixes });
                    $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
                    Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
                });
                break;
        }
        $("#selectPrefix").trigger('change');
        Claim.GetCausesByPrefixId(modelSearchCriteria.prefixId);
    }

    static GetEndorsementsByPrefixIdBranchIdPolicyNumber() {
        Claim.clearTables();
        $("#selectEndorsement").rules("remove");
        $("#selectCauseClaim").rules("remove");
        $("#InputClaimDetail").rules('remove');
        $("#formClaim").validate();
        if ($("#formClaim").valid()) {
            $("#selectEndorsement").rules("add", { required: true });
            $("#selectCauseClaim").rules("add", { required: true });
            $("#InputClaimDetail").rules("add", { required: true });
            var prefixId = $('#selectPrefix').UifSelect('getSelected');
            var branchId = $('#selectBranch').UifSelect('getSelected');
            var policyNumber = $('#InputPolicyNumber').val();
            var claimDate = $("#InputDateIncident").UifDatepicker('getValue');

            ClaimRequest.GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdPolicyNumber(prefixId, branchId, Claim.GetCoveredRiskTypeId(), policyNumber, claimDate).done(function (data) {
                if (data.success) {
                    if (data.result != null) {

                        if (glClaimOption == 3) {
                            endorsements = data.result;
                            $("#selectEndorsement").UifSelect({ sourceData: data.result });
                            $("#selectEndorsement").UifSelect("setSelected", (data.result[data.result.length - 1]).Id);
                        }

                        // Se carga el endoso más reciente
                        endorsementId = (data.result[data.result.length - 1]).Id;
                        Claim.GetPolicyByEndorsementIdModuleType((data.result[data.result.length - 1]).Id);

                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PolicyNotValid, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static LoadEndorsementData(event, selectedItem) {
        Claim.clearTables();

        var endorsementFind = endorsements.find(x => x.Description == selectedItem.Text);

        if (endorsementFind != null) {
            endorsementId = endorsementFind.Id;
            Claim.GetPolicyByEndorsementIdModuleType(endorsementFind.Id);

            if (endorsementFind.Description != (endorsements[endorsements.length - 1]).Description) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EndorsementIsNotTheLastForOcurrenceDate, 'autoclose': true });
            }
        }

    }

    static ConvertNoticeCoverageToEstimation(dataRisk) {

        var indexRisk = -1;

        if (noticeToClaim.Coverages.length > 0) {
            indexRisk = dataRisk.findIndex(x => x.RiskNumber === noticeToClaim.Coverages[0].RiskNum);
        }

        if (indexRisk < 0) return;

        ClaimRequest.GetEstimationTypesByPrefixId($('#selectPrefix').UifSelect('getSelected')).done(function (data) {
            if (data.success) {
                var estimationTypes = data.result;
                ClaimEstimationRequest.GetReasonsByStatusIdPrefixId(EstimationTypeStatuses.Open, $('#selectPrefix').UifSelect('getSelected')).done(function (response) {
                    if (response.success) {
                        var estimationTypeStatusReasons = response.result;

                        var claimCoverage = noticeToClaim.Coverages.map(function (coverage) {
                            var coverageEstimation = {};
                            var estimationTypeDescription = estimationTypes.find(x => x.Id == coverage.EstimateTypeId).Description;

                            coverageEstimation.ClaimCoverageId = null;
                            coverageEstimation.CoverageDescription = coverage.CoverageName;
                            coverageEstimation.CoverageId = coverage.CoverageId;
                            coverageEstimation.CoverageNumber = coverage.CoverNum;
                            coverageEstimation.DeductibleAmount = 0;
                            coverageEstimation.EstimationConcept = coverage.CoverageName + estimationTypeDescription;
                            coverageEstimation.EstimationType = estimationTypeDescription;
                            coverageEstimation.EstimationTypeEstatus = EstimationTypeStatuses.Open;
                            coverageEstimation.EstimationTypeEstatusReasonCode = estimationTypeStatusReasons[0].Id;
                            coverageEstimation.EstimationTypeId = coverage.EstimateTypeId;
                            coverageEstimation.EstimateAmount = coverage.EstimateAmount;
                            coverageEstimation.EstimateAmountAccumulate = 0;
                            coverageEstimation.InsuredAmountTotal = coverage.InsurableAmount;
                            coverageEstimation.IndividualId = coverage.IndividualId;
                            coverageEstimation.Insured = (coverage.IsInsured) ? Resources.Language.MsgInsured : Resources.Language.MsgThird;
                            coverageEstimation.IsInsured = coverage.IsInsured;
                            coverageEstimation.IsProspect = !coverage.IsInsured;
                            coverageEstimation.RiskDescription = Claim.GetRiskDescrition(dataRisk[indexRisk]);
                            coverageEstimation.AffectedFullName = coverage.FullName;
                            coverageEstimation.RiskId = dataRisk[indexRisk].RiskId;
                            coverageEstimation.RiskNumber = coverage.RiskNum;
                            coverageEstimation.ClaimedAmount = noticeToClaim.ClaimedAmount;
                            coverageEstimation.IsClaimedAmount = noticeToClaim.ClaimedAmount > 0 ? false : true;
                            coverageEstimation.TotalConcept = 0;
                            coverageEstimation.TotalTax = 0;
                            coverageEstimation.TypeEstatusId = null;
                            coverageEstimation.TypeEstatusReasonId = null;
                            coverageEstimation.ExchangeRate = 1;
                            coverageEstimation.Currency = policyCurrencyDescription;
                            coverageEstimation.CurrencyId = policyCurrency;
                            coverageEstimation.ThirdAffectedDTO = {
                                Id: coverage.IndividualId,
                                DocumentNumber: coverage.DocumentNumber,
                                FullName: coverage.FullName
                            };

                            if (coverageEstimation.IsProspect) {
                                affectedId = coverage.IndividualId;
                            }

                            return coverageEstimation;
                        });

                        $("#" + Claim.GetTableSubSinister()).UifDataTable({ sourceData: claimCoverage });
                        Claim.AddDataTableSummaryClaim();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static clearTables() {
        $("#" + Claim.GetTableSubSinister()).UifDataTable('clear');
        $("#" + Claim.GetTableRisk()).UifDataTable('clear');
        $("#tableCoverage").UifDataTable('clear');
        $("#tableEstimations").UifDataTable('clear');

        Claim.AddDataTableSummaryClaim();
    }

    static GetCausesByPrefixId(prefixId, callback) {
        ClaimRequest.GetCausesByPrefixId(prefixId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
                $('#selectCauseClaim').UifSelect({ sourceData: data.result });
            }
            else {
                $('#selectCauseClaim').UifSelect({ sourceData: "" });
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPolicyByEndorsementIdModuleType(policyEndorsementId) {
        if (policyEndorsementId > 0) {
            ClaimRequest.GetPolicyByEndorsementIdModuleType(policyEndorsementId).done(function (data) {
                if (data.success) {
                    Claim.GetJudicialDecisionDateIsActiveByPrefixId(data.result.PrefixId);
                    policyId = data.result.Id;
                    var claimDate = $("#InputDateIncident").UifDatepicker('getValue');

                    if (modelSearchCriteria.claimId == null) {
                        Claim.GetClaimsByPolicyIdOccurrenceDate(policyId, claimDate);

                        //if (!data.result.IsReinsurance) {
                        //    $.UifDialog('alert', {
                        //        message: String.format(Resources.Language.EndorsementWithoutReinsurance)
                        //    }, function (result) {

                        //    });
                        //    return;
                        //}
                    }

                    if (data.result.BusinessTypeId == BusinessType.Assigned) {
                        $('#divCoInsurance').show();
                    }
                    $('#inputSearchPolicyByclaims').text(data.result.BranchDescription.substring(0, 3) + '-' + data.result.PrefixDescription.substring(0, 3) + '-' + data.result.DocumentNumber + '-' + data.result.EndorsementDocumentNum);
                    $('.hideSearchClaim').show();
                    //Constantes
                    endorsementId = data.result.EndorsementId;
                    policyBusinessTypeId = data.result.BusinessTypeId;
                    policyTypeId = data.result.PolicyTypeId;
                    policyProductId = data.result.ProductId;
                    policyCurrency = data.result.CurrencyId;
                    policyCurrencyDescription = data.result.CurrencyDescription;
                    $("#tableEstimations tr:eq(0) th:eq(7)").text(Resources.Language.PendantReserve + " (" + data.result.CurrencyDescription + ")");
                    policyHolderId = data.result.HolderId;
                    if (data.result.CoInsurance != null && data.result.CoInsurance.length > 0) {
                        companyParticipationPercentage = data.result.CoInsurance[0].ParticipationOwn;
                    }
                    Claim.ValidateCurrencyEstimation(policyCurrency);
                    //Pestaña Datos Póliza
                    $('#_policyId').val(data.result.Id);
                    $('#_policyHolder').val(data.result.HolderName);
                    $('#_policuInsured').val('');
                    $('#_policyBeneficiary').val('');
                    $('#_policyIntermediary').val(data.result.Agent);
                    $('#_policyEndorsement').val(data.result.EndorsementDocumentNum);
                    $('#_policyType').val(data.result.PolicyType);
                    $('#_policyBusinessType').val(data.result.BusinessType);
                    $('#_policyIssuingDate').val(FormatDate(data.result.IssueDate));
                    $('#_policyValidSince').val(FormatDate(data.result.CurrentFrom));
                    $('#_policyValidTo').val(FormatDate(data.result.CurrentTo));

                    //Pestaña Datos Cartera 
                    $('#_policyIssuedPrime').val('');
                    $('#_policuTableExpenses').val('');
                    $('#_policyTableTaxes').val(data.result.TaxExpenses);
                    $('#_policyTotalBonus').val('');
                    $('#tblPortfolio').val('');

                    //Pestaña Datos Siniestro
                    $('#_claimsList').val('');

                    ///Seccion Principal
                    $("#InputHolder").val(data.result.HolderName);
                    $('#InputCurrency').val(data.result.CurrencyDescription);
                    $('#InputPendingDebt').val(data.result.PendingDebt);
                    $('#InputNumberClaims').val(data.result.ClaimsQuantity);
                    $('#InputReasonPolicy').val(data.result.PolicyType);
                    $('#InputBusinessType').val(data.result.BusinessType);
                    $('#InputSinestrablePeriod').val(FormatDate(data.result.CurrentFrom) + ' - ' + FormatDate(data.result.CurrentTo));
                    $('#InputLastPeriodBilled').val(FormatDate(data.result.CurrentFrom) + ' - ' + FormatDate(data.result.CurrentTo));
                    $('#InputAgentPrincipal').val(data.result.Agent);
                    $('#InputTypeTransaction').val(data.result.EndorsementType);

                    //Modal Coinsure
                    $("#tableCoInsurance").UifDataTable({ sourceData: data.result.CoInsurance });

                    switch (data.result.BusinessTypeId) {
                        case BusinessType.Direct:
                        case BusinessType.Accepted:
                            $("#rdbTotal").prop("checked", true);
                            $("#rdbCompanyParticipation").prop("disabled", true);
                            $("#rdbTotal").prop("disabled", true);
                            break;
                        case BusinessType.Assigned:
                            if (!$("#rdbCompanyParticipation").is(':checked') && !$("#rdbTotal").is(':checked')) {
                                $("#rdbTotal").prop("checked", true);
                            }
                            $("#rdbCompanyParticipation").prop("disabled", false);
                            $("#rdbTotal").prop("disabled", false);
                            break;
                        default:
                            $("#rdbTotal").prop("checked", true);
                            $("#rdbCompanyParticipation").prop("disabled", true);
                            $("#rdbTotal").prop("disabled", true);
                            break;
                    }

                    //Sección estimación
                    $('#rdbtnInsured').prop("checked", true);
                    $('#rdbtnThirdParty').prop("disabled", true);
                    $('#rdbtnInsured').prop("disabled", true);
                    $('#divDriverData').hide();
                    $('#divThirdPartyVehicle').hide();
                    $('#divAffectedProperty').hide();
                    $('#divThirdAffected').hide();
                    $('#divClaimVehicleThirdVehicle').hide();

                    // Sección consulta póliza
                    $('#selectBranch').UifSelect("setSelected", data.result.BranchId);
                    $('#selectPrefix').UifSelect("setSelected", data.result.PrefixId);

                    var prefixId = $('#selectPrefix').UifSelect('getSelected');
                    switch (glClaimOption) {
                        case 1:
                            ClaimVehicle.GetRisksByEndorsementId(policyEndorsementId);
                            break;
                        case 2:
                            ClaimLocation.GetRisksByEndorsementId(policyEndorsementId);
                            break;
                        case 3:
                            ClaimSurety.GetRisksByEndorsementIdPrefixId(policyEndorsementId, prefixId);
                            break;
                        case 4:
                            ClaimTransport.GetRisksByEndorsementId(policyEndorsementId);
                            break;
                        case 5:
                            ClaimAirCraft.GetRisksByEndorsementIdPrefixId(policyEndorsementId, prefixId);
                            break;
                        case 6:
                            ClaimFidelity.GetRisksByEndorsementId(policyEndorsementId);
                            break;
                    }

                    Claim.GetEstimationTypesByPrefixId(prefixId);

                    policyConsulted = true;
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            Claim.cleanForm();
        }
    }

    static GetClaimsByPolicyIdOccurrenceDate(policyId, claimOccurrenceDate) {
        ClaimRequest.GetClaimsByPolicyIdOccurrenceDate(policyId, claimOccurrenceDate).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $.UifDialog('confirm', {
                        message: String.format(Resources.Language.MessagePolicyClaimsOccurrenceDate, $("#InputPolicyNumber").val(), data.result.length, moment(claimOccurrenceDate).format("DD/MM/YYYY"))
                    }, function (result) {
                        if (result) {
                            // Aceptar
                            $.each(data.result, function (index, value) {
                                this.OccurrenceDate = FormatDate(this.OccurrenceDate);
                            });
                            $("#tblClaimsByPolicy").UifDataTable({ sourceData: data.result });

                            $('#modalClaimsPolicy').UifModal('showLocal', Resources.Language.InfoClaims);
                            $('#modalClaimsPolicy .modal-dialog.modal-lg').attr('style', 'width: 55%');
                        }
                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetExchangeRate(event, currency) {
        return new Promise(function (success) {
            if (policyCurrency != currency.Id) {
                var currencyExchange = policyCurrency > 0 ? policyCurrency : currency.Id;
                ClaimRequest.GetExchangeRate(currencyExchange).done(function (data) {
                    if (data.success) {
                        exchangeRate = data.result;
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': 'Moneda sin tasa de cambio parametrizada, se usará la moneda de emisión.', 'autoclose': true });
                        $('#selectClaimCurrency').UifSelect('setSelected', policyCurrency);
                        $('#selectClaimCurrency').trigger("change");
                    }
                    success();
                });
            } else if (currency.Id == policyCurrency) {
                exchangeRate = {
                    SellAmount: 1
                };
                success();
            }
        });
    }

    static ValidateCurrencyEstimation(currencyId) {
        ClaimRequest.GetCurrency().done(function (data) {
            if (data.success) {
                var currencies = currencyId == 0 ? data.result : data.result.filter(x => { return x.Id == currencyId || x.Id == 0 });
                $('#selectClaimCurrency').UifSelect({ sourceData: currencies });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(claimModifyId, prefixId, coverageId, individualId) {
        ClaimRequest.GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(claimModifyId, prefixId, coverageId, individualId).done(function (response) {
            if (response.success) {
                if (response.result.length > 0) {
                    var estimationsData = response.result.map(function (Data) {
                        Data.EstimationConcept = Data.Description;
                        Data.EstimationId = Data.Id;
                        Data.EstimationAmount = Data.EstimateAmount;
                        Data.Currency = Data.CurrencyReason;
                        Data.CurrencyId = Data.CurrencyReasonId;
                        Data.PendingReservation = parseFloat(Data.EstimationAmount) - parseFloat(Data.Payments);
                        return Data;
                    });

                    estimationsDataTable = estimationsData;
                    $("#tableEstimations").UifDataTable({ sourceData: estimationsData });
                    ClaimEstimation.UpdateIndicators();
                }
                else {
                    Claim.GetEstimationTypesByPrefixId(prefixId);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetEstimationTypesByPrefixId(prefixId, callback) {
        ClaimRequest.GetEstimationTypesByPrefixId(prefixId).done(function (data) {
            if (data.success) {
                var estimationsData = data.result.map(function (Data) {
                    Data.EstimationConcept = Data.Description;
                    Data.EstimationId = Data.Id;
                    Data.EstimationAmount = Data.EstimateAmount;
                    Data.Currency = Data.CurrencyReason;
                    Data.CurrencyId = Data.CurrencyReasonId;
                    return Data;
                });

                if (typeof callback === "function")
                    return callback(estimationsData);

                estimationsDataTable = estimationsData;
                $("#tableEstimations").UifDataTable({ sourceData: estimationsData });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    SelectedPrefix(event, selectedItem) {
        if (selectedItem.Id > 0) {
            Claim.GetCausesByPrefixId(selectedItem.Id);
        }
        else {
            $('#selectCauseClaim').UifSelect();
        }
    }

    static ViewClaimEstimations() {
        if (policyConsulted) {
            $('#titleEstimations').show();
            $('#claim').hide();
            $('#btnSaveClaim').hide();
            $('#claimEstimation').show();
            $('#btnCloseEstimation').show();

            switch (glClaimOption) {
                case 1:
                    $('#ClaimVehiclePartial').show();
                    $('#EstimationsSummaryVehiclePartial').show();
                    $('#titleClaimVehicle').hide();
                    break;
                case 2:
                    $('#ClaimLocationPartial').show();
                    $('#EstimationsSummaryLocationPartial').show();
                    $('#titleClaimLocation').hide();
                    break;
                case 3:
                    $('#ClaimSuretyPartial').show();
                    $('#EstimationsSummarySuretyPartial').show();
                    $('#titleClaimSurety').hide();
                    break;
                case 4:
                    $('#ClaimTransportPartial').show();
                    $('#EstimationsSummaryTransportPartial').show();
                    $('#titleClaimTransport').hide();
                    break;
                case 5:
                    $('#ClaimAirCraftPartial').show();
                    $('#EstimationsSummaryAircraftPartial').show();
                    $('#titleClaimAirCraft').hide();
                    break;
                case 6:
                    $('#ClaimFidelityPartial').show();
                    $('#EstimationsSummaryFidelityPartial').show();
                    $('#titleClaimFidelity').hide();
                    break;
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNeedPolicy, 'autoclose': true });
        }

    }

    static CloseEstimation() {
        $('#claimEstimation').hide();
        $('#btnCloseEstimation').hide();
        $('#titleEstimations').hide();

        switch (glClaimOption) {
            case 1:
                $('#ClaimVehiclePartial').hide();
                $('#EstimationsSummaryVehiclePartial').hide();
                $('#titleClaimVehicle').show();
                break;
            case 2:
                $('#ClaimLocationPartial').hide();
                $('#EstimationsSummaryLocationPartial').hide();
                $('#titleClaimLocation').show();
                break;
            case 3:
                $('#ClaimSuretyPartial').hide();
                $('#EstimationsSummarySuretyPartial').hide();
                $('#titleClaimSurety').show();
                break;
            case 4:
                $('#ClaimTransportPartial').hide();
                $('#EstimationsSummaryTransportPartial').hide();
                $('#titleClaimTransport').show();
                break;
            case 5:
                $('#ClaimAirCraftPartial').hide();
                $('#EstimationsSummaryAircraftPartial').hide();
                $('#titleClaimAirCraft').show();
                break;
            case 6:
                $('#ClaimFidelityPartial').hide();
                $('#EstimationsSummaryFidelityPartial').hide();
                $('#titleClaimFidelity').show();
                break;
        }
        $('#claim').show();
        $('#btnSaveClaim').show();
        ClaimEstimation.ClearSubClaims();
        if ($('#tableSubSinisterSurety').UifDataTable('getData').length > 0 && $('#inputClaimedAmount').val() != "" && $('#tableCoverage').UifDataTable('getSelected') != null) {
            ClaimEstimation.ModifyClaimedAmount();
        }
        ScrollTop();
    }

    static cleanForm() {
        //ClaimPartial

        $("#suretyClaimNumberEdit").hide();
        $("#locationClaimNumberEdit").hide();
        $("#transportClaimNumberEdit").hide();
        $("#airCraftClaimNumberEdit").hide();
        $("#fidelityClaimNumberEdit").hide();
        $("#vehicleClaimNumberEdit").hide();
        $('#InputDateNotice').UifDatepicker('setValue', GetCurrentFromDate());
        $('#InputDateNotice').UifDatepicker('disabled', false);
        $('#InputDateIncident').UifDatepicker('disabled', false);
        $('#InputIntermediaryClaimNum').val("");
        $('#InputInformer').val("");
        $('#InputClaimDetail').val("");
        $('#InputCurrency').val("");
        $('#InputSinestrablePeriod').val("");
        $('#InputLastPeriodBilled').val("");
        $('#InputPendingDebt').val("");
        $('#InputNumberClaims').val("");
        $('#InputReasonPolicy').val("");
        $('#InputTypeTransaction').val("");
        $('#InputBusinessType').val("");
        $('#labelEstimation').text(0);
        $('#labelPayments').text(0);
        $('#labelIncome').text(0);
        $('#labelReservations').text(0);
        $('#tableSubSinister').UifDataTable('clear');
        $('#tableLocationRiskVehicle').UifDataTable('clear');
        $('#tableCoverage').UifDataTable('clear');
        $('#tableEstimations').UifDataTable('clear');
        $('#selecDamageClassification').UifSelect('disabled', false);
        $('#selectDamageResponsibility').UifSelect('disabled', false);
        $('#selecDamageClassification').UifSelect('setSelected', null);
        $('#selectDamageResponsibility').UifSelect('setSelected', null);
        $('#InputPolicyNumber').val('');
        $('#InputPolicyNumber').UifInputSearch('disabled', false);
        $('#selectPrefix').UifSelect('disabled', false);
        $('#selectBranch').UifSelect('disabled', false);
        $('#selectBranchClaim').UifSelect('disabled', false);
        $('#selectPrefix').UifSelect('setSelected', null);
        $('#selectBranch').UifSelect('setSelected', null);
        $('#selectBranchClaim').UifSelect('setSelected', null);
        $('#InputClaimDetail').val('');
        $('#InputInformer').val('');
        $('#InputDateofFact').val('');
        $('#selectCauseClaim').UifSelect('setSelected', null);
        $('#InputDateIncident').UifDatepicker('clear');
        $("#InputTimeIncident").UifTimepicker('setValue', '00:00:00 PM');
        $('#coverageDeductibleCode').val('');
        $('#coverageDeductibleDescription').val('');
        $('#pNoRecords').show();
        $('#pEventRecords').hide();
        $('#_number').val('');
        $('#_claimId').val('');
        $('#_damageCode').val('');
        $('#_damageResponsabilitie').val('');

        Claim.ClearPolicyInformation();

        //Clear DataTable
        $("#" + Claim.GetTableSubSinister()).UifDataTable('clear');
        $("#" + Claim.GetTableRisk()).UifDataTable('clear');
        $('#divtableSummaryClaim').hide();

        //Clear claim location information
        Claim.LockClaimLocation(false);
        $("#ClaimLocation").val("");
        $("#ddlCountry").UifSelect('setSelected', null);
        $("#ddlState").UifSelect();
        $("#ddlCity").UifSelect();

        noticeId = null;
        modelSearchCriteria.claimId = null;
        modelSearchCriteria.claimNumber = null;
        modelSearchCriteria.claprefixIdimId = null;
        modelSearchCriteria.paymentRequestId = null;
        modelSearchCriteria.number = null;
        modelSearchCriteria.branchId = null;
        modelSearchCriteria.endorsementId = null;
        noticeToClaim = null;

        $("#inputThirdAffectedDocumentNumber").val('');
        $("#inputThirdAffectedDataFullName").val('');

        $("#InputJudicialDecisionDate").parent().parent().hide();
        $('.hideSearchClaim').hide();
    }

    static ClearPolicyInformation() {
        policyConsulted = false;

        //Se borran las variables
        $('#_endorsementId').val('');
        $('#_riskId').val('');
        $('#InputHolder').val("");
        $('#InputAgentPrincipal').val("");
        $('.hideSearchClaim').hide();
        //Constantes
        policyId = null;
        policyBusinessTypeId = null;
        policyTypeId = null;
        policyProductId = null;
        policyCurrency = null;
        policyHolderId = 0;
        $('#divCoInsurance').hide();
        exchangeRate = null;
        //Pestaña Datos Póliza
        $('#_policyId').val('');
        $('#_policyHolder').val('');
        $('#_policuInsured').val('');
        $('#_policyBeneficiary').val('');
        $('#_policyIntermediary').val('');
        $('#_policyEndorsement').val('');
        $('#_policyType').val('');
        $('#_policyBusinessType').val('');
        $('#_policyIssuingDate').val('');
        $('#_policyValidSince').val('');
        $('#_policyValidTo').val('');
        $("#selectEndorsement").UifSelect({ sourceData: null });

        //Pestaña Datos Cartera 
        $('#_policyIssuedPrime').val('');
        $('#_policuTableExpenses').val('');
        $('#_policyTableTaxes').val('');
        $('#_policyTotalBonus').val('');
        $('#tblPortfolio').val('');

        //Pestaña Datos Siniestro
        $('#_claimsList').val('');

        ///Seccion Principal
        $("#InputHolder").val('');
        $('#InputCurrency').val('');
        $('#InputPendingDebt').val('');
        $('#InputNumberClaims').val('');
        $('#InputReasonPolicy').val('');
        $('#InputBusinessType').val('');
        $('#InputSinestrablePeriod').val('');
        $('#InputLastPeriodBilled').val('');
        $('#InputAgentPrincipal').val('');
        $('#InputTypeTransaction').val('');
        $("#rdbTotal").prop("checked", false);
        $("#rdbCompanyParticipation").prop("checked", false);

        //Modal Coinsure
        $("#tableCoInsurance").UifDataTable('clear');
    }


    ////////////////////////////////////
    ///Sección Consulta Póliza
    static OpenModalPolicyConsult() {
        if (policyConsulted) {
            Claim.GetClaimsByPolicyId(policyId);
            $('#modalPolicyConsult').UifModal('showLocal', Resources.Language.InfoSummaryPolicy);
            $('#modalPolicyConsult .modal-dialog.modal-lg').attr('style', 'width: 55%');
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNeedPolicy, 'autoclose': true });
        }
    }

    static GetClaimsByPolicyId(policyId) {
        ClaimRequest.GetClaimsByPolicyId(policyId).done(function (data) {
            if (data.success) {
                $.each(data.result, function (index, value) {
                    this.OccurrenceDate = FormatDate(this.OccurrenceDate);
                });
                $("#tblClaimsList").UifDataTable({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Error, 'autoclose': true });
            }
        });
    }

    ////////////////////////////////////
    ///Sección Analista - Ajustador
    static OpenModalAdjusterAnalyst() {
        if (policyConsulted) {
            $('#InputClaimDate').val($('#InputDateIncident').val());
            $('#modalAdjusterAnalyst').UifModal('showLocal', Resources.Language.AnalystAdjuster);
            $('#modalAdjusterAnalyst .modal-dialog.modal-lg').attr('style', 'width: 70%');
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNeedPolicy, 'autoclose': true });
        }
    }

    static SaveAdjusterAnalyst() {
        $("#formAdjusterAnalyst").validate();
        if ($("#formAdjusterAnalyst").valid()) {
            $('#modalAdjusterAnalyst').UifModal('hide');
            $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MessageUpdate, 'autoclose': true });
        }
    }

    ////////////////////////////////////
    /// Sección Evento Catastrofico
    static GetStatesByCountryId(countryId, callback) {
        ClaimRequest.GetStatesByCountryId(countryId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
                $('#selectDepartment').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId, callback) {
        ClaimRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
                $('#selectCity').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static OpenEventCatastrophic() {
        if (policyConsulted) {
            $('#modalCatastrophicInformation').UifModal('showLocal', Resources.Language.CatastrophicInformation);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNeedPolicy, 'autoclose': true });
        }
    }

    selectCountry(event, selectedItem) {
        Claim.GetStatesByCountryId(selectedItem.Id);
    }

    selectClaimCountry(event, selectedItem) {
        Claim.GetStatesByCountryId(selectedItem.Id, function (states) {
            $("#ddlState").UifSelect({ sourceData: states });
        });
    }

    selectDepartment(event, selectedItem) {
        var countryId = $('#selectCountry').UifSelect('getSelected');
        Claim.GetCitiesByCountryIdStateId(countryId, selectedItem.Id);
    }

    selectClaimDepartment(event, selectedItem) {
        var countryId = $('#ddlCountry').UifSelect('getSelected');
        Claim.GetCitiesByCountryIdStateId(countryId, selectedItem.Id, function (cities) {
            $("#ddlCity").UifSelect({ sourceData: cities });
        });
    }

    static SaveCatastrophic() {
        $("#formCatastrophicInformation").validate();
        if ($("#formCatastrophicInformation").valid()) {
            $('#pNoRecords').hide();
            $('#pEventRecords').show();
            $('#modalCatastrophicInformation').UifModal('hide');
            $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MessageUpdate, 'autoclose': true });
        }
    }

    ////////////////////////////////////
    /// Sección Coinsurance
    static OpenCoInsuranceCompany() {
        if (policyConsulted) {
            $('#modalCoInsuranceCompany').UifModal('showLocal', Resources.Language.LabelCoInsuranceProvided);

        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNeedPolicy, 'autoclose': true });
        }
    }

    static ChangeCoinsurance() {
        if ($("#" + Claim.GetTableRisk()).UifDataTable('getSelected') != null && $("#" + Claim.GetTableRisk()).UifDataTable('getSelected').length > 0) {
            var risk = $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0];
            ClaimEstimation.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(risk);
        }
    }

    static AddDataTableSummaryClaim() {
        $("#tableSummaryClaim").UifDataTable('clear');
        var estimationList = $('#' + Claim.GetTableSubSinister()).UifDataTable('getData');

        $.each(estimationList, function (index, value) {
            this.EstimationConcept = this.AffectedFullName + " - " + this.CoverageDescription + " (" + this.EstimationType + ")";
            this.PendingReservation = (policyCurrency > 0 ? this.EstimateAmount / this.ExchangeRate - this.PaymentValue / this.ExchangeRate : this.EstimateAmount * this.ExchangeRate - this.PaymentValue * this.ExchangeRate);

            if (this.PendingReservation < 0) {
                this.EstimateAmount = this.PaymentValue;
                this.PendingReservation = 0;
            }
        });

        $('#divtableSummaryClaim').show();
        $("#tableSummaryClaim").UifDataTable({ sourceData: estimationList });
        ClaimEstimation.CalculateEstimationSubSinister();
    }

    static ValidateTableSummaryClaim() {
        return ($("#tableSummaryClaim").UifDataTable('getData').length > 0);
    }

    static LockClaimLocation(disabled) {
        $("#ClaimLocation").prop('disabled', disabled);
        $("#ddlCountry").UifSelect('disabled', disabled);
        $("#ddlState").UifSelect('disabled', disabled);
        $("#ddlCity").UifSelect('disabled', disabled);
    }

    static GetJudicialDecisionDateIsActiveByPrefixId(prefixId) {
        ClaimRequest.GetJudicialDecisionDateIsActiveByPrefixId(prefixId).done(function (response) {
            if (response.success) {
                if (response.result) {
                    $("#InputJudicialDecisionDate").parent().parent().show();
                }
                else {
                    $("#InputJudicialDecisionDate").parent().parent().hide();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        })
    }

    static ValidateClaimOccurrenceAndNoticeDate() {
        var occurrenceDate = $("#InputDateIncident").val();
        var noticeDate = $("#InputDateNotice").val();

        if (occurrenceDate && noticeDate) {
            if (CompareClaimDates(occurrenceDate, noticeDate)) {
                var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.LabelDateIncident + ': ' + (occurrenceDate + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.LabelDateNotice + ': ' + noticeDate) + '"';
                $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
                $("#InputDateIncident").UifDatepicker('clear');
            }
        }
    }

    static GetTableSubSinister() {
        var tableSubSinister = "";
        switch (glClaimOption) {
            case 1:
                tableSubSinister = "tableSubSinisterVehicle";
                break;
            case 2:
                tableSubSinister = "tableSubSinisterLocation";
                break;
            case 3:
                tableSubSinister = "tableSubSinisterSurety";
                break;
            case 4:
                tableSubSinister = "tableSubSinisterTransport";
                break;
            case 5:
                tableSubSinister = "tableSubSinisterAirCraft";
                break;
            case 6:
                tableSubSinister = "tableSubSinisterFidelity";
                break;
        }
        return tableSubSinister;
    }

    static GetTableRisk() {
        var tableRisk = "";

        switch (glClaimOption) {
            case 1:
                tableRisk = "tableRiskVehicle";
                break;
            case 2:
                tableRisk = "tableRiskLocation";
                break;
            case 3:
                tableRisk = "tableRiskSurety";
                break;
            case 4:
                tableRisk = "tableRiskTransport";
                break;
            case 5:
                tableRisk = "tableRiskAirCraft";
                break;
            case 6:
                tableRisk = "tableRiskFidelity";
                break;
        }
        return tableRisk;
    }

    static GetRiskDescrition(risk) {
        switch (glClaimOption) {
            case 1:
                return risk.Plate;
                break;
            case 2:
                return risk.FullAddress;
                break;
            case 3:
                return risk.Bonded;
                break;
            case 4:
            case 6:
            case 5:
                return risk.Risk;
                break;
        }
    }

    static LoadClaim() {
        var claimSelected = $("#tblClaimsByPolicy").UifDataTable('getSelected');

        if (claimSelected != null) {

            modelSearchCriteria.claimId = claimSelected[0].ClaimId;

            switch (glClaimOption) {
                case 1:
                    ClaimVehicle.preLoadClaimVehicle();
                    break;
                case 2:
                    ClaimLocation.preLoadClaimLocation();
                    break;
                case 3:
                    ClaimSurety.preLoadClaimSurety();
                    break;
                case 4:
                    ClaimTransport.preLoadClaimTransport();
                    break;
                case 5:
                    ClaimAirCraft.preLoadClaimAirCraft();
                    break;
                case 6:
                    ClaimFidelity.preLoadClaimFidelity();
                    break;
            }
        }
    }

    static GetCoveredRiskTypeId() {
        switch (glClaimOption) {
            case 1:
                return CoveredRiskType.Vehicles;
                break;
            case 2:
                return CoveredRiskType.Location;
                break;
            case 3:
                return CoveredRiskType.Sureties;
                break;
            case 4:
                return CoveredRiskType.Transport;
                break;
            case 5:
                return CoveredRiskType.Aeronavigation;
                break;
            //case 6:
            //    ClaimFidelity.preLoadClaimFidelity();
            //    break;
        }
    }

}

////////////////////////////////////
/// Execute Claim (Coberturas, estimaciones y datos adicionales)
function BuildExecuteClaim() {
    var claimDTO = {
        //revisar esto
        ClaimId: parseInt($('#_claimId').val()),
        Number: $('#_number').val(),
        Modifications: {
            ClaimCoverages: {
                Estimations: {
                }
            }
        },
        JudicialDecisionDate: $("#InputJudicialDecisionDate").UifDatepicker('getValue'),
        BranchId: $('#selectBranchClaim').UifSelect('getSelected'),
        PrefixId: $('#selectPrefix').UifSelect('getSelected'),
        OccurrenceDate: $("#InputDateIncident").val() + (' ') + $("#InputTimeIncident").val(),
        NoticeId: noticeId,
        NoticeDate: $("#InputDateNotice").UifDatepicker('getValue'),
        DamageTypeId: $('#selecDamageClassification').UifSelect('getSelected') == "" ? null : parseInt($('#selecDamageClassification').UifSelect('getSelected')),
        DamageResponsabilityId: $('#selectDamageResponsibility').UifSelect('getSelected') == "" ? null : parseInt($('#selectDamageResponsibility').UifSelect('getSelected')),
        IndividualId: policyHolderId,
        Location: $("#ClaimLocation").val(),
        CityId: $("#ddlCity").UifSelect('getSelected'),
        StateId: $("#ddlState").UifSelect('getSelected'),
        CountryId: $("#ddlCountry").UifSelect('getSelected'),
        CauseId: $("#selectCauseClaim").UifSelect('getSelected'),
        Description: $("#InputClaimDetail").val(),
        IsTotalParticipation: $("#rdbTotal").is(':checked'),
        BusinessTypeId: policyBusinessTypeId,
        PolicyId: $('#_policyId').val(),
        PolicyDocumentNumber: $('#InputPolicyNumber').val(),
        EndorsementId: endorsementId,
        EndorsementNumber: $('#_policyEndorsement').val(),
        CoveredRiskType: $("#" + Claim.GetTableRisk()).UifDataTable('getData')[0].CoveredRiskType,
        PolicyBusinessTypeId: policyBusinessTypeId,
        PolicyTypeId: policyTypeId,
        PolicyProductId: policyProductId,
        Catastrophe: {
        },
    };

    var risksCoverage = [];
    var estimations = [];
    var claimCoverages = [];
    var claimCoveragesNews = [];
    var modifications = [];
    var tableRiskCoverage = $("#" + Claim.GetTableSubSinister()).UifDataTable('getData');

    tableRiskCoverage.forEach(function (item, index) {
        var claimCoverage = risksCoverage.find(function (data) {
            if (item.IndividualId == 0) {
                return data.CoverageId == item.CoverageId
                    && data.ThirdAffectedDTO.DocumentNumber == item.ThirdAffectedDTO.DocumentNumber
                    && data.ThirdAffectedDTO.FullName == item.ThirdAffectedDTO.FullName;
            }
            else {
                return data.CoverageId == item.CoverageId && data.IndividualId == item.IndividualId;
            }
        });

        if (claimCoverage == null) {
            risksCoverage.push(item);
        }
    });

    var objectModification = {
        ClaimId: parseInt($('#_claimId').val()),
        RegistrationDate: new Date(),
        AccountingDate: $("#InputAccountingDate").val(),
        UserId: "0",
        UserProfileId: "0"
    };

    if ($('#inputCatastrophe').val() != "") {
        var Id = 0;
        var Description = $('#inputCatastrophe').val();

        if (dataCatastrophe.Description == Description) {
            Id = dataCatastrophe.Id;
        }

        var catastrophe = {
            Id: Id,
            Description: $('#inputCatastrophe').val()
        };

        claimDTO.Catastrophe = catastrophe;

        claimDTO.CatastrophicId = Id;
        claimDTO.CatastropheDescription = $('#InputInfringementDescription').val();
        claimDTO.CatastropheDateTimeFrom = $('#InputDateTimeFrom').val() + (' ') + $('#timepickerFrom').val();
        claimDTO.CatastropheDateTimeTo = $('#InputDateTimeTo').val() + (' ') + $('#timepickerTo').val();
        claimDTO.CatastropheAddress = $('#InputAddress').val();
        claimDTO.CatastropheCityId = $('#selectCity').UifSelect('getSelected') == "" ? 0 : $('#selectCity').UifSelect('getSelected');
        claimDTO.CatastropheStateId = $('#selectDepartment').UifSelect('getSelected') == "" ? 0 : $('#selectDepartment').UifSelect('getSelected');
        claimDTO.CatastropheCountryId = $('#selectCountry').UifSelect('getSelected') == "" ? 0 : $('#selectCountry').UifSelect('getSelected');
    }

    risksCoverage.forEach(function (item, index) {
        var coverageClaim = tableRiskCoverage.find(function (data) {
            return data.CoverageId == item.CoverageId && data.IndividualId == item.IndividualId && data.ClaimCoverageId != null;
        });

        var objectClaimCoverage = {
            Id: coverageClaim == null ? null : coverageClaim.ClaimCoverageId,
            SubClaim: item.SubClaim,
            RiskId: item.RiskId,
            RiskNum: item.RiskNumber,
            CoverageId: item.CoverageId,
            CoverageNum: item.CoverageNumber,
            IndividualId: item.IndividualId,
            IsInsured: item.IsInsured,
            EndorsementId: endorsementId,
            IsProspect: item.IsProspect,
            InsuredAmountTotal: item.InsuredAmountTotal,
            Description: item.Description,
            DriverInformationDTO: item.DriverInformationDTO,
            ThirdAffectedDTO: item.ThirdAffectedDTO,
            ThirdPartyVehicleDTO: item.ThirdPartyVehicleDTO,
            AffectedProperty: item.AffectedProperty,
            ClaimedAmount: item.ClaimedAmount.toString().replace('.', ','),
            IsClaimedAmount: item.IsClaimedAmount
        };

        if (objectClaimCoverage.Id == null) {
            claimCoveragesNews.push(objectClaimCoverage);
        }
        else if (objectClaimCoverage.Id != null) {
            claimCoverages.push(objectClaimCoverage);
        }
    });

    claimCoverages.sort(function (a, b) {
        if (a.SubClaim < b.SubClaim) {
            return -1;
        }

        if (a.SubClaim > b.SubClaim) {
            return 1;
        }

        return 0;
    });

    claimCoverages = claimCoverages.concat(claimCoveragesNews);

    objectModification.ClaimCoverages = claimCoverages;

    claimCoverages.forEach(function (item, index) {
        var estimation = tableRiskCoverage.filter(function (data) {
            if (item.IndividualId == 0) {
                return data.CoverageId == item.CoverageId
                    && data.ThirdAffectedDTO.DocumentNumber == item.ThirdAffectedDTO.DocumentNumber
                    && data.ThirdAffectedDTO.FullName == item.ThirdAffectedDTO.FullName;
            }
            else {
                return data.CoverageId == item.CoverageId && data.IndividualId == item.IndividualId;
            }
        });

        estimation.forEach(function (item, index) {
            var objectEstimations = {
                Id: parseInt(item.EstimationTypeId),
                StatusCodeId: parseInt(item.EstimationTypeEstatus),
                ReasonId: parseInt(item.EstimationTypeEstatusReasonCode),
                CurrencyId: parseInt(item.CurrencyId),
                EstimateAmount: item.EstimateAmount.toString().replace('.', ','),
                EstimateAmountAccumulate: item.EstimateAmountAccumulate.toString().replace('.', ','),
                DeductibleAmount: item.DeductibleAmount.toString().replace('.', ','),
                CreationDate: $("#InputAccountingDate").val(),
                IsMinimumSalary: item.IsMinimumSalary,
                MinimumSalaryValue: item.MinimumSalaryValue,
                MinimumSalariesNumber: item.MinimumSalariesNumber != null ? item.MinimumSalariesNumber.toString().replace('.', ',') : 0,
                ExchangeRate: item.ExchangeRate,
                CoverageInsuredAmountEquivalent: (policyCurrency != parseInt(item.CurrencyId) ? (policyCurrency > 0 ? parseFloat((item.InsuredAmountTotal * item.ExchangeRate).toFixed(2)) : parseFloat((item.InsuredAmountTotal / item.ExchangeRate).toFixed(2))) : item.InsuredAmountTotal),
                InternalStatusId: item.InternalStatusId
            };

            estimations.push(objectEstimations);
        });

        if (estimations != null) {
            objectModification.ClaimCoverages[index].Estimations = estimations;
            estimations = [];
        }
    });

    modifications.push(objectModification);

    claimDTO.Modifications = modifications;

    if ($('#selectAnalyst').UifSelect('getSelected') != null && $('#selectAnalyst').UifSelect('getSelected') != "") {
        claimDTO.AdjusterId = $('#selectAjuster').UifSelect('getSelected') == "" ? 0 : $('#selectAjuster').UifSelect('getSelected');
        claimDTO.AnalizerId = $('#selectAnalyst').UifSelect('getSelected') == "" ? 0 : $('#selectAnalyst').UifSelect('getSelected');
        claimDTO.ResearcherId = $('#selectInvestigator').UifSelect('getSelected') == "" ? 0 : $('#selectInvestigator').UifSelect('getSelected');
        claimDTO.DateInspection = $('#InputInspectionDate').val() + (' ') + $('#selectHour').val();
        claimDTO.HourInspection = $('#selectHour').UifSelect('getSelected') == "" ? "00" : $('#selectHour').UifSelect('getSelected');
        claimDTO.AffectedProperty = $('#InputPropertyAffected').val();
        claimDTO.LossDescription = $('#InputDescriptionOfLost').val();
    }

    return claimDTO;
}
