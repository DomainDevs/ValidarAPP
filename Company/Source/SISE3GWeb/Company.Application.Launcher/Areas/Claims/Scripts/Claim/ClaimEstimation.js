var estimationsDataTable = null;
var coverageDriverInformation = [];
var coverageThirdPartyVehicle = [];
var editEstimationTable = null;
var individualId = null;
var affectedId = 0;
var AffectedFullName = null;
var enabledThird = false;
var coverageSelected = null;
var prefixId = 0;
var estimationTypeStatus = null;
var tableSubSinisterposition = null;
var estimationTypeStatusReasons = null;
var estimationTypeStatuses = null;
var estimationAmount = null;
var AmountType = null;
var enableZeroEstimation = false;
var MinimumSalaryValue = null;
var ValPositionTop = 0;

class ClaimEstimation extends Uif2.Page {
    getInitialState() {
        $("#btnSaveSubClaim").parent().hide();
    }

    bindEvents() {
        $('#btnSaveEstimationClaim').on('click', ClaimEstimation.SaveEstimation);
        $('#btnCancelModalEstimation').on('click', ClaimEstimation.CloseModalEstimation);
        $("#_chkIsClaimedAmount").on('change', ClaimEstimation.ValidateClaimedAmount);

        $('#inputEstimation').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $("#inputEstimation").focusin(ClaimEstimation.NotFormatMoneyIn);
        $("#inputEstimation").focusout(ClaimEstimation.FormatMoneyOut);
        $("#inputEstimation").OnlyDecimals(2);

        $("#inputSalaryNumeric").OnlyDecimals(2);

        $('#inputDeductible').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $("#inputDeductible").focusin(ClaimEstimation.NotFormatMoneyIn);
        $("#inputDeductible").focusout(ClaimEstimation.FormatMoneyOut);
        $("#inputDeductible").OnlyDecimals(2);

        $('#inputDriverFullName').TextTransform(ValidatorType.UpperCase);
        $('#inputDriverLicenseType').TextTransform(ValidatorType.UpperCase);
        $('#inpuTthirdVehiclePlate').TextTransform(ValidatorType.UpperCase);
        $('#inpuTVehicleMake').TextTransform(ValidatorType.UpperCase);
        $('#inpuTVehicleModel').TextTransform(ValidatorType.UpperCase);
        $('#selectVehicleColor').TextTransform(ValidatorType.UpperCase);
        $('#inpuTVehicleEngine').TextTransform(ValidatorType.UpperCase);
        $('#inpuTVehicleChasis').TextTransform(ValidatorType.UpperCase);
        $('#inputVehicleVIN').TextTransform(ValidatorType.UpperCase);
        $('#textareaObservations').TextTransform(ValidatorType.UpperCase);
        $('#InputAffectedProperty').TextTransform(ValidatorType.UpperCase);
        $("#inputClaimedAmount").ValidatorKey(ValidatorType.Decimal, 2, 1);
        $("#inputClaimedAmount").focusin(ClaimEstimation.NotFormatMoneyIn);
        $("#inputClaimedAmount").focusout(ClaimEstimation.FormatMoneyOut);
        $('#inputClaimedAmount').val("0");

        //Tables
        $('#tableEstimations').on('rowSelected', function (event, estimation, position) {
            ClaimEstimation.OpenModalEstimation(estimation, position);
        });

        $('#tableCoverage').on('rowSelected', function (event, coverage, position) {
            ClaimEstimation.GetActivePanelsByCoverageId(coverage);
            var risk = $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0];
            ClaimEstimation.GetCoverageDeductibleByCoverageId(coverage.Id, risk.RiskNumber, coverage.Number);
        });

        // RadioButtons - EstimationsPartial
        $('#rdbtnInsured').on('click', function () {
            ClaimEstimation.ClickdRbtnInsuredOrThirdParty(true)
        });
        $('#rdbtnThirdParty').on('click', function () {
            ClaimEstimation.ClickdRbtnInsuredOrThirdParty(false)
        });

        //Selects
        $('#selectStateCode').on('itemSelected', ClaimEstimation.GetReasonsByStatusIdPrefixId);

        // Input - AutoCompletes - EstimationsPartial
        $('#inputThirdDocumentNumber').on('itemSelected', function (event, selectedItem) {
            ClaimVehicle.selectedThirdDocumentNumber(event, selectedItem);
        });

        $('#inputThirdDataFullName').on('itemSelected', function (event, selectedItem) {
            ClaimVehicle.selectedThirdDataFullName(event, selectedItem);
        });

        $('#inputDriverDocumentNumber').on('itemSelected', function (event, selectedItem) {
            ClaimVehicle.selectedDriverDocumentNumber(event, selectedItem);
        });

        $('#inputDriverFullName').on('itemSelected', function (event, selectedItem) {
            ClaimVehicle.selectedDriverFullName(event, selectedItem);
        });

        $('#inpuTthirdVehiclePlate').on('itemSelected', function (event, selectedItem) {
            ClaimVehicle.selectedThirdVehiclePlate(selectedItem);
        });

        $('#ModalEstimations').on('closed.modal', function () {
            $("#tableEstimations").UifDataTable('unselect');
        });

        $("#inputThirdAffectedDocumentNumber").on("keydown", ClaimEstimation.ClearParticipantInformation);
        $("#inputThirdAffectedDataFullName").on("keydown", ClaimEstimation.ClearParticipantInformation);
        $("#inputThirdAffectedDocumentNumber").on('itemSelected', ClaimEstimation.SetParticipantInformation);
        $("#inputThirdAffectedDataFullName").on('itemSelected', ClaimEstimation.SetParticipantInformation);

        $('#btnSaveSubClaim').on('click', ClaimEstimation.AddSubClaims);
        $('#selectAmountType').on('itemSelected', ClaimEstimation.SelectedAmountType);
        $('#selectClaimCurrency').on('itemSelected', Claim.GetExchangeRate);
        $("#inputSalaryNumeric").on("change", ClaimEstimation.CalculateEstimationBySalaryNumber);
        $("#inputEstimation").on("change", ClaimEstimation.CalculateSalaryNumberByEstimation);
    }

    static GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(risk, callback) {
        lockScreen();
        ClaimEstimation.GetStatuses().then(function () {
            ClaimEstimation.GetReasonsByPrefixId().then(function () {
                var occurrenceDate = $("#InputDateIncident").val() + (' ') + $("#InputTimeIncident").val();
                var companyParticipationPct = $("#rdbTotal").is(':checked') ? 100 : companyParticipationPercentage;

                ClaimEstimationRequest.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(risk.RiskId, occurrenceDate, companyParticipationPct).done(function (data) {
                    if (data.success) {
                        if (data.result.length > 0) {
                            if (callback)
                                return callback(data.result);

                            $("#tableCoverage").UifDataTable('unselect');

                            data.result = data.result.map(function (coverages) {
                                coverages.RiskId = risk.RiskId;
                                return coverages;
                            });

                            $("#tableCoverage").UifDataTable({ sourceData: data.result });
                            $("#tableEstimations").UifDataTable({ sourceData: Object.assign([], estimationsDataTable) });
                        } else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RiskWithoutCoverages, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                }).always(function () {
                    if (!callback)
                        ValPositionTop = $("#inputClaimedAmount").offset().top - 140;

                    unlockScreen();
                });

                if (!callback) {
                    ClaimEstimationRequest.GetInsuredsByIndividualId(risk.InsuredId).done(function (data) {
                        if (data.success && data.result.length > 0) {
                            $("#coverageDocument").val(data.result[0].DocumentNumber);
                            $("#coverageFullName").val(data.result[0].FullName);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorConsultingInsured, 'autoclose': true });
                        }
                    }).always(function () {
                        unlockScreen();
                    });
                }
            });
        });

    }

    static OpenModalEstimation(estimation, position) {
        enableZeroEstimation = false;
        if (enabledThird && ($('#inputThirdAffectedDocumentNumber').val() === "" || $('#inputThirdAffectedDataFullName').val() === "")) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EstimateCompletingThird, 'autoclose': true });
            $("#tableEstimations").UifDataTable('unselect');
        } else if ($('#tableCoverage').UifDataTable('getSelected') != null && $('#' + Claim.GetTableRisk()).UifDataTable('getSelected') != null) {

            if (ClaimEstimation.ValidateAmountPretention()) {

                $('#ModalEstimations').UifModal('showLocal', Resources.Language.EditEstimation);
                $('#inputEstimation').val(FormatMoney(estimation.EstimateAmount));
                $('#inputDeductible').val(FormatMoney(estimation.DeductibleAmount));

                ClaimRequest.GetEstimationTypesSalariesEstimation().done(function (response) {
                    if (response.success) {
                        if (response.result.some(x => x == estimation.Id)) {
                            $("#selectAmountType").parent().parent().show();
                            $('#selectAmountType').UifSelect('setSelected', estimation.IsMinimumSalary ? 2 : 1);
                            $('#selectAmountType').trigger('change');
                            $('#inputSalaryNumeric').val(estimation.MinimumSalariesNumber);

                            if (SalaryMinimumMounth == null)
                                $.UifNotify('show', { 'type': 'info', 'message': 'Salario Mínimo no parametrizado, no podrás estimar en éste Tipo de Importe', 'autoclose': true });
                        }
                        else {
                            $("#selectAmountType").parent().parent().hide();
                            $('#selectAmountType').UifSelect('setSelected', estimation.IsMinimumSalary ? 2 : 1);
                            $("#inputSalaryNumeric").parent().hide();
                            $("#inputSalaryNumeric").val("");
                            $("#LabelEstimation").text(Resources.Language.Estimation);
                            $('#inputEstimation').prop("disabled", false);

                            // MANEJO DE DEDUCIBLES PARA COBERTURAS PAGADAS A TERCEROS.
                            if ($('#rdbtnThirdParty').prop("checked")) {
                                $("#selectStateCode").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                                $("#selectReason").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                                $("#inputDeductible").parent().hide();
                            }
                            else {
                                $("#selectStateCode").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                                $("#selectReason").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                                $("#inputDeductible").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                                $("#inputDeductible").parent().show();
                            }
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
                    }
                });

                if (estimation.StatusCodeId > 0) {
                    $('#selectClaimCurrency').UifSelect('setSelected', estimation.CurrencyId);
                    $('#selectClaimCurrency').trigger('change');

                    ClaimEstimation.GetStatusesByEstimationTypeId(estimation.EstimationId, function (statuses) {
                        $("#selectStateCode").UifSelect({ sourceData: statuses });
                        $("#selectStateCode").UifSelect("setSelected", estimation.StatusCodeId);

                        if (!$("#" + Claim.GetTableSubSinister()).UifDataTable('getData').find(x => x.EstimationTypeId == estimation.EstimationId).ClaimId) {
                            $("#selectStateCode").UifSelect('disabled', true);
                            $('#selectClaimCurrency').UifSelect('disabled', false);

                            if (SalaryMinimumMounth != null)
                                $('#selectAmountType').UifSelect('disabled', false);

                        } else {
                            $('#selectClaimCurrency').UifSelect('disabled', true);
                            $('#selectAmountType').UifSelect('disabled', true);
                        }

                        var seletedStatus = {
                            Id: estimation.StatusCodeId
                        };

                        ClaimEstimation.GetReasonsByStatusIdPrefixId(null, seletedStatus, function (reasons) {
                            $("#selectReason").UifSelect({ sourceData: reasons });
                            $("#selectReason").UifSelect("setSelected", estimation.ReasonId);
                        });
                    });
                }
                else {
                    $('#selectClaimCurrency').UifSelect('disabled', false);
                    if (SalaryMinimumMounth != null)
                        $('#selectAmountType').UifSelect('disabled', false);

                    if (policyCurrency != null) {
                        $('#selectClaimCurrency').UifSelect('setSelected', policyCurrency);
                        $('#selectClaimCurrency').trigger('change');
                    }
                    ClaimEstimation.GetStatusesByEstimationTypeId(estimation.EstimationId);
                }

            }
            else {
                $('body, .main-container').animate({
                    scrollTop: ValPositionTop
                }, 1500, function () {
                    $("#inputClaimedAmount").focus();
                });

                $("#tableEstimations").UifDataTable('unselect');
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPretention, 'autoclose': true });
            }

        } else {
            $("#tableEstimations").UifDataTable('unselect');
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectARiskAndCoverage, 'autoclose': true });
        }
    }

    static ValidateAmountPretention() {
        return (parseFloat($("#inputClaimedAmount").val()) > 0 || $("#_chkIsClaimedAmount").is(":checked"));
    }

    static GetStatusesByEstimationTypeId(estimationTypeId, callback) {
        ClaimEstimationRequest.GetStatusesByEstimationTypeId(estimationTypeId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                $("#selectStateCode").UifSelect({ sourceData: data.result });
                $("#selectStateCode").UifSelect('setSelected', EstimationTypeStatuses.Open);
                $("#selectStateCode").UifSelect('disabled', true);
                $("#selectStateCode").trigger('change');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetReasonsByStatusIdPrefixId(event, seletedItem, callback) {
        var prefixId = $('#selectPrefix').UifSelect('getSelected');
        ClaimEstimationRequest.GetReasonsByStatusIdPrefixId(seletedItem.Id, prefixId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                enableZeroEstimation = (estimationTypeStatuses.find(function (status) { return status.Id == seletedItem.Id }).InternalStatus.Id == 2);

                $("#selectReason").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SaveEstimation() {
        $("#formEstimations").validate();
        if ($("#formEstimations").valid()) {

            var estimationSelected = $("#tableEstimations").UifDataTable('getSelected')[0];
            var coverageSelected = $("#tableCoverage").UifDataTable('getSelected')[0];

            if (ClaimEstimation.ValidateEstimationsAmount(estimationSelected, coverageSelected)) {

                if ($('#selectAmountType').UifSelect('getSelected') == 1) {
                    MinimumSalaryValue = null;
                    AmountType = 0;
                } else {
                    MinimumSalaryValue = SalaryMinimumMounth;
                    AmountType = 1;
                }

                var statusSelected = $("#selectStateCode").UifSelect("getSelectedSource");

                var estimation = {
                    Id: estimationSelected.Id,
                    EstimationId: estimationSelected.Id,
                    EstimationConcept: estimationSelected.EstimationConcept,
                    StatusCode: statusSelected.Description,
                    StatusCodeId: statusSelected.Id,
                    InternalStatusId: statusSelected.InternalStatus.Id,
                    Reason: $("#selectReason").UifSelect("getSelectedText"),
                    ReasonId: $("#selectReason").UifSelect("getSelected"),
                    Currency: $("#selectClaimCurrency").UifSelect("getSelectedText"),
                    CurrencyId: $("#selectClaimCurrency").UifSelect("getSelected"),
                    ClaimedAmount: parseFloat(NotFormatMoney($("#inputClaimedAmount").val()).replace(',', '.')).toFixed(2),
                    IsClaimedAmount: $("#_chkIsClaimedAmount").is(':checked'),
                    EstimateAmount: parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2),
                    EstimateAmountAccumulate: estimationSelected.EstimateAmountAccumulate ? estimationSelected.EstimateAmountAccumulate : 0,
                    DeductibleAmount: parseFloat(NotFormatMoney($('#inputDeductible').val()).replace(',', '.')).toFixed(2),
                    DeductibleNet: parseFloat(NotFormatMoney($('#inputDeductible').val()).replace(',', '.')).toFixed(2),
                    Payments: parseFloat(estimationSelected.Payments),
                    PendingReservation: parseFloat(estimationSelected.PendingReservation),
                    IsMinimumSalary: AmountType,
                    MinimumSalariesNumber: $('#inputSalaryNumeric').val(),
                    MinimumSalaryValue: MinimumSalaryValue,
                    ExchangeRate: exchangeRate.SellAmount,
                    PendingReservation: (policyCurrency > 0 ? ((parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2)) / exchangeRate.SellAmount) : ((parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2)) * exchangeRate.SellAmount)) - parseFloat(estimationSelected.PaymentValue ? estimationSelected.PaymentValue : 0),
                    PaymentValue: parseFloat(estimationSelected.Payments)
                };

                var indexOfEstimation = $("#tableEstimations").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(estimationSelected.Id);
                $("#tableEstimations").UifDataTable('editRow', estimation, indexOfEstimation);
                estimationsDataTable = $("#tableEstimations").UifDataTable("getData");

                if (!enabledThird) {
                    var indexOfSubSinister = $('#' + Claim.GetTableSubSinister()).UifDataTable('getData').findIndex(x =>
                        x.CoverageId == coverageSelected.CoverageId && x.EstimationTypeId == estimation.Id && x.RiskId == coverageSelected.RiskId && x.IndividualId == $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0].InsuredId
                    );

                    ClaimEstimation.SaveSubSinister(estimation, indexOfSubSinister);
                    $("#tableEstimations").UifDataTable('unselect');
                    $('#ModalEstimations').UifModal('hide');
                    ClaimEstimation.ClearSubClaims();
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MessageUpdate, 'autoclose': true });
                } else {
                    $('#ModalEstimations').UifModal('hide');
                }
            }

        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorEmptyRequiredFields, 'autoclose': true });
        }
    }

    static SaveSubSinister(estimation, indexOfSubSinister) {
        var coverageSelected = $("#tableCoverage").UifDataTable('getSelected')[0];
        var riskSelected = $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0];

        var ThirdAffected = {
            Id: affectedId,
            DocumentNumber: $('#inputThirdAffectedDocumentNumber').UifAutoComplete('getValue'),
            FullName: $('#inputThirdAffectedDataFullName').UifAutoComplete('getValue').toUpperCase()
        };

        var DriverInformation = {
            Id: 0,
            LicenseType: $('#inputDriverLicenseType').val(),
            LicenseNumber: $('#inputDriverDataLicenseNumber').val(),
            LicenseValidThru: $('#inputDriverDataVto').val(),
            Age: $('#inputDataAge').val(),
            DocumentNumber: $('#inputDriverDocumentNumber').val(),
            DocumentType: 0,
            Name: $('#inputDriverFullName').val()
        };

        var ThirdPartyVehicle = {
            Id: 0,
            Plate: $('#inpuTthirdVehiclePlate').val(),
            VehicleMake: $('#inpuTVehicleMake').val(),
            VehicleModel: $('#inpuTVehicleModel').val(),
            VehicleYear: $('#inpuTthirdVehicleYear').val(),
            VehicleColorId: $('#selectVehicleColor').val(),
            VehicleColorDescription: $('#selectVehicleColor').val(),
            ChasisNumber: $('#inpuTVehicleChasis').val(),
            EngineNumber: $('#inpuTVehicleEngine').val(),
            VinCode: $('#inputVehicleVIN').val(),
            Description: $('#textareaObservations').val(),
        };

        var AffectedProperty = $('#InputAffectedProperty').val();

        individualId = enabledThird ? affectedId : $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0].InsuredId;
        AffectedFullName = enabledThird ? $('#inputThirdAffectedDataFullName').UifAutoComplete('getValue').toUpperCase() : $("#coverageFullName").val() //TODO: Ajustar nombre de asegurado;

        var subClaim = {
            ClaimCoverageId: null,
            CoverageDescription: coverageSelected.Description,
            CoverageId: coverageSelected.CoverageId,
            CoverageNumber: coverageSelected.Number,
            InsuredAmountTotal: coverageSelected.InsuredAmountTotal,
            Currency: estimation.Currency,
            CurrencyId: estimation.CurrencyId,
            DamageResponsibilityCode: null,
            DamageTypeCode: null,
            DeductibleAmount: estimation.DeductibleAmount,
            DeductibleNet: estimation.DeductibleNet,
            EstimationType: estimation.EstimationConcept,
            EstimationTypeEstatus: estimation.StatusCodeId,
            EstimationTypeEstatusReasonCode: estimation.ReasonId,
            EstimationTypeId: estimation.Id,
            EstimateAmount: estimation.EstimateAmount,
            EstimateAmountAccumulate: estimation.EstimateAmountAccumulate,
            InternalStatusId: estimation.InternalStatusId,
            IndividualId: individualId,
            Insured: $('#rdbtnInsured').is(':checked') ? Resources.Language.MsgInsured : Resources.Language.MsgThirdParty,
            IsInsured: $('#rdbtnInsured').is(':checked'),
            IsProspect: $('#rdbtnThirdParty').is(':checked'),
            PaymentValue: parseFloat(estimation.Payments),
            RiskDescription: Claim.GetRiskDescrition(riskSelected),
            RiskId: riskSelected.RiskId,
            RiskNumber: riskSelected.RiskNumber,
            TotalConcept: 0,
            TotalTax: 0,
            TypeEstatusId: null,
            TypeEstatusReasonId: null,
            AffectedProperty: AffectedProperty,
            ThirdAffectedDTO: ThirdAffected,
            DriverInformationDTO: DriverInformation,
            ThirdPartyVehicleDTO: ThirdPartyVehicle,
            AffectedFullName: AffectedFullName,
            ClaimedAmount: estimation.ClaimedAmount,
            IsClaimedAmount: estimation.IsClaimedAmount,
            IsMinimumSalary: estimation.IsMinimumSalary,
            MinimumSalariesNumber: estimation.MinimumSalariesNumber,
            MinimumSalaryValue: estimation.MinimumSalaryValue,
            ExchangeRate: estimation.ExchangeRate
        };

        if (indexOfSubSinister > -1) {
            subClaim.SubClaim = $("#" + Claim.GetTableSubSinister()).UifDataTable('getData')[indexOfSubSinister].SubClaim;
            subClaim.ClaimId = $("#" + Claim.GetTableSubSinister()).UifDataTable('getData')[indexOfSubSinister].ClaimId;
            $("#" + Claim.GetTableSubSinister()).UifDataTable('deleteRow', indexOfSubSinister);
        }
        $('#' + Claim.GetTableSubSinister()).UifDataTable('addRow', subClaim);
        Claim.AddDataTableSummaryClaim();
    }

    static ValidateEstimationsAmount(estimation, coverage) {
        var estimations = $("#tableEstimations").UifDataTable('getData');
        var estimationValue = parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2)

        estimations.forEach(function (item, index) {
            if (item.Id != estimation.Id) {
                estimationValue += item.EstimateAmount;
            }
        });

        if ($("#selectClaimCurrency").UifSelect('getSelected') != policyCurrency) {
            if (exchangeRate != null) {
                var coverageInsuredAmount = policyCurrency > 0 ? parseFloat((coverage.InsuredAmountTotal * exchangeRate.SellAmount).toFixed(2)) : parseFloat((coverage.InsuredAmountTotal / exchangeRate.SellAmount).toFixed(2));

                if (FormatDate(exchangeRate.RateDate) != moment().format("DD/MM/YYYY")) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.OutdatedExchangeRate + ' ' + FormatDate(exchangeRate.RateDate), 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExchangeRateNotParametrized, 'autoclose': true });
                return false;
            }
        }
        else {
            var coverageInsuredAmount = coverage.InsuredAmountTotal;
        }

        if (!enableZeroEstimation && parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2) == 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EstimationBiggerThanZero, 'autoclose': true });
            return false;
        }
        else if (estimation.Id == 1 /*INDEMNIZACIÓN*/ && parseFloat(parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2)) > coverageInsuredAmount) {
            $.UifNotify('show', { 'type': 'danger', 'message': String.format(Resources.Language.TheEstimateMustNotExceedTheCoverage, $("#selectClaimCurrency").UifSelect('getSelectedText'), FormatMoney(coverageInsuredAmount)), 'autoclose': true });
            return false;
        }
        else if (parseFloat(parseFloat(NotFormatMoney($('#inputDeductible').val()).replace(',', '.')).toFixed(2)) > parseFloat(parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2))) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DeductibleCanNotExceed, 'autoclose': true });
            return false;
        }
        else {
            return true;
        }
    }

    static CalculateEstimationSubSinister() {
        var tableData = $("#" + Claim.GetTableSubSinister()).UifDataTable('getData');
        var estimation = 0;
        var reserve = 0;
        var payments = 0;

        $.each(tableData, function (index, value) {
            if (this.EstimateAmount != '') {
                estimation += parseFloat(this.EstimateAmount == null || isNaN(this.EstimateAmount) ? 0 : this.EstimateAmount);
                payments += parseFloat(this.PaymentValue == null || isNaN(this.PaymentValue) ? 0 : this.PaymentValue);
                reserve = estimation - payments;
            }
        });

        estimation = parseFloat(estimation.toString().replace(',', '.')).toFixed(2);
        reserve = parseFloat(reserve.toString().replace(',', '.')).toFixed(2) > 0 ? parseFloat(reserve.toString().replace(',', '.')).toFixed(2) : 0;
        payments = parseFloat(payments.toString().replace(',', '.')).toFixed(2);

        $('#labelEstimation').text(FormatMoney(estimation));
        $('#labelReservations').text(FormatMoney(reserve));
        $('#labelPayments').text(FormatMoney(payments));
    }

    static GetEstimationsByClaimId(claimId) {
        ClaimEstimationRequest.GetEstimationByClaimId(claimId).done(function (data) {
            if (data.success) {
                $("#" + Claim.GetTableSubSinister()).UifDataTable('clear');

                $('#_riskId').val(data.result[0].RiskId);
                if (modelSearchCriteria.claimId != null)
                    Claim.GetPolicyByEndorsementIdModuleType(modelSearchCriteria.endorsementId);

                $("#" + Claim.GetTableSubSinister()).UifDataTable({ sourceData: data.result });
                ClaimEstimation.CalculateEstimationSubSinister();
                Claim.GetClaimSupplier();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static UpdateIndicators() {
        var payments = 0;
        var reservations = 0;

        var tableEstimationData = $('#tableEstimations').UifDataTable('getData');

        for (var i = 0; i < tableEstimationData.length; i++) {
            if (tableEstimationData[i].Payments != '') {
                payments += tableEstimationData[i].Payments;
                reservations += tableEstimationData[i].PendingReservation;
            }
        }

        $('#labelPayments').text(FormatMoney(payments));
        $('#labelReservations').text(FormatMoney(reservations));
    }

    static GetActivePanelsByCoverageId(coverage, callback) {
        enabledThird = false;
        ClaimEstimation.ClearEstimations();
        ClaimEstimation.ClearAffectedPropertyPanel();
        ClaimEstimation.ClearDriverManDataPanel();
        ClaimEstimation.ClearThirdAffectedPanel();
        ClaimEstimation.ClearThirdPartyVehiclePanel();

        lockScreen();
        ClaimEstimationRequest.GetActivePanelsByCoverageId(coverage.Id).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (typeof callback === "function")
                        return callback(data.result);

                    var result = data.result;
                    enabledThird = result.IsEnabledThird;

                    $('#divDriverData').hide();
                    $('#divThirdPartyVehicle').hide();
                    $('#divAffectedProperty').hide();
                    $('#divThirdAffected').hide();

                    $('#rdbtnInsured').prop("disabled", true);
                    $('#rdbtnThirdParty').prop("disabled", true);

                    if (result.IsEnabledDriver) {
                        $('#divDriverData').show();
                    }
                    else {
                        $('#divDriverData').hide();
                    }

                    if (result.IsEnabledThirdPartyVehicle) {
                        $('#divThirdPartyVehicle').show();
                    }
                    else {
                        $('#divThirdPartyVehicle').hide();
                    }

                    if (result.IsEnabledAffectedProperty) {
                        $('#divAffectedProperty').show();
                    } else {
                        $('#divAffectedProperty').hide();
                    }

                    if (result.IsEnabledThird) {
                        $("#btnSaveSubClaim").parent().show();
                        $('#divThirdAffected').show();
                        $('#rdbtnThirdParty').prop("disabled", false);
                        $('#rdbtnInsured').prop("disabled", false);

                        $('#rdbtnThirdParty').prop("checked", true);
                        $('#rdbtnInsured').prop("checked", false);
                    }
                    else {
                        $("#btnSaveSubClaim").parent().hide();
                        $('#rdbtnThirdParty').prop("disabled", true);
                        $('#rdbtnInsured').prop("disabled", true);

                        $('#rdbtnThirdParty').prop("checked", false);
                        $('#rdbtnInsured').prop("checked", true);
                    }
                } else {
                    $('#rdbtnInsured').prop("checked", true);
                    $('#rdbtnThirdParty').prop("disabled", true);
                    $('#rdbtnInsured').prop("disabled", true);
                    $('#divDriverData').hide();
                    $('#divThirdPartyVehicle').hide();
                    $('#divAffectedProperty').hide();
                    $('#divThirdAffected').hide();
                }

                ClaimEstimation.ValidateSubClaims();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }

        }).always(function () {
            unlockScreen();
        });
    }

    static GetCoverageDeductibleByCoverageId(coverageId, riskNum, coverNumber) {
        ClaimEstimationRequest.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNumber).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    //$('#coverageDeductibleCode').val(data.result.Code);
                    $('#coverageDeductibleDescription').val(data.result[0].Description);
                } else {
                    //$('#coverageDeductibleCode').val("");
                    $('#coverageDeductibleDescription').val("");
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ValidateSubClaims() {
        var prefixId = $("#selectPrefix").UifSelect("getSelected");
        var riskSelected = $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0];
        var coverageSelected = $("#tableCoverage").UifDataTable("getSelected")[0];
        var estimations = [];

        if (coverageSelected != null && riskSelected != null) {
            var subSinisters = [];

            if (!enabledThird) {
                subSinisters = $("#" + Claim.GetTableSubSinister()).UifDataTable('getData').filter(function (data) {
                    return data.CoverageId == coverageSelected.Id && data.RiskId == riskSelected.RiskId && data.IndividualId == $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0].InsuredId;
                });
            }
            else if (enabledThird && affectedId != 0) {
                subSinisters = $("#" + Claim.GetTableSubSinister()).UifDataTable('getData').filter(function (data) {
                    return data.CoverageId == coverageSelected.Id && data.RiskId == riskSelected.RiskId && data.IndividualId == affectedId;
                });
            }

            if (subSinisters.length > 0) {

                var claimCoverage = subSinisters.filter(x => x.ClaimCoverageId != null)[0];
                if (claimCoverage != null) {
                    ClaimEstimation.GetAffectedPropertyByClaimCoverageId(claimCoverage.ClaimCoverageId);
                    ClaimEstimation.GetClaimedAmountByClaimCoverageId(claimCoverage.ClaimCoverageId);
                    ClaimEstimation.GetClaimDriverInformationByClaimCoverageId(claimCoverage.ClaimCoverageId);
                    ClaimEstimation.GetClaimThirdPartyVehicleByClaimCoverageId(claimCoverage.ClaimCoverageId);
                }

                estimationsDataTable.forEach(function (item, index) {
                    var subSinister = subSinisters.find(function (data) {
                        return data.EstimationTypeId == item.EstimationId;
                    });
                    if (subSinister == null) {
                        estimations.push(item);
                    } else {
                        var estimation = {
                            Currency: subSinister.Currency,
                            CurrencyId: subSinister.CurrencyId,
                            Deductible: subSinister.DeductibleAmount,
                            DeductibleAmount: subSinister.DeductibleAmount,
                            DeductibleNet: subSinister.DeductibleNet,
                            Description: subSinister.EstimationType,
                            EstimateAmount: subSinister.EstimateAmount,
                            EstimateAmountAccumulate: subSinister.EstimateAmountAccumulate,
                            EstimationConcept: subSinister.EstimationType,
                            EstimationId: subSinister.EstimationTypeId,
                            EstimationType: subSinister.EstimationTypeId,
                            Id: subSinister.EstimationTypeId,
                            Payments: subSinister.PaymentValue,
                            PendingReservation: subSinister.PendingReservation,
                            Reason: estimationTypeStatusReasons.find(function (reason) { return reason.Id == subSinister.EstimationTypeEstatusReasonCode }).Description,
                            ReasonId: subSinister.EstimationTypeEstatusReasonCode,
                            StatusCode: estimationTypeStatuses.find(function (status) { return status.Id == subSinister.EstimationTypeEstatus }).Description,
                            StatusCodeId: subSinister.EstimationTypeEstatus,
                            IsMinimumSalary: subSinister.IsMinimumSalary,
                            MinimumSalariesNumber: subSinister.MinimumSalariesNumber,
                            MinimumSalaryValue: subSinister.MinimumSalaryValue,
                            ExchangeRate: subSinister.ExchangeRate,
                            ClaimedAmount: subSinister.ClaimedAmount,
                            IsClaimedAmount: subSinister.IsClaimedAmount
                        };

                        estimations.push(estimation);
                    }
                });

                $('#tableEstimations').UifDataTable({ sourceData: Object.assign([], estimations) });
            }
            else {

                Claim.GetEstimationTypesByPrefixId(prefixId);
            }

        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectARiskAndCoverage, 'autoclose': true });
        }
    }

    static CloseModalEstimation() {
        $("#tableEstimations").UifDataTable('unselect');
        $('#ModalEstimations').UifModal('hide');
    }

    static GetAffectedPropertyByClaimCoverageId(claimCoverageId) {
        ClaimEstimationRequest.GetAffectedPropertyByClaimCoverageId(claimCoverageId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#InputAffectedProperty').val(data.result);
                    $('#divAffectedProperty').show();
                } else {
                    ClaimEstimation.ClearAffectedPropertyPanel();
                }
            }
        });
    }

    static GetClaimedAmountByClaimCoverageId(claimCoverageId) {
        ClaimEstimationRequest.GetClaimedAmountByClaimCoverageId(claimCoverageId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#inputClaimedAmount').val(FormatMoney(data.result.ClaimedAmount));
                    $('#inputClaimedAmount').prop('disabled', data.result.IsClaimedAmount);
                    $("#_chkIsClaimedAmount").prop("checked", data.result.IsClaimedAmount);
                } else {
                    ClaimEstimation.ClearAmountPretention();
                }
            }
        });
    }

    static GetClaimDriverInformationByClaimCoverageId(claimCoverageId) {
        ClaimEstimationRequest.GetClaimDriverInformationByClaimCoverageId(claimCoverageId).done(function (response) {
            if (response.success) {
                if (response.result != null) {
                    $('#inputDriverDocumentNumber').val(response.result.DocumentNumber);
                    $('#inputDriverFullName').val(response.result.Name);
                    $('#inputDriverLicenseType').val(response.result.LicenseType);
                    $('#inputDriverDataLicenseNumber').val(response.result.LicenseNumber);
                    $('#inputDriverDataVto').val(FormatDate(daresponseta.result.LicenseValidThru));
                    $('#inputDataAge').val(response.result.Age);
                    $('#divDriverData').show();
                } else {
                    ClaimEstimation.ClearDriverManDataPanel();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetClaimThirdPartyVehicleByClaimCoverageId(claimCoverageId) {
        ClaimEstimationRequest.GetClaimThirdPartyVehicleByClaimCoverageId(claimCoverageId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#inpuTthirdVehiclePlate').val(data.result.Plate);
                    $('#inpuTVehicleMake').val(data.result.Make);
                    $('#inpuTVehicleModel').val(data.result.Model);
                    $('#inpuTthirdVehicleYear').val(data.result.Year);
                    $('#selectVehicleColor').UifSelect('setSelected', data.result.ColorCode);
                    $('#inpuTVehicleEngine').val(data.result.EngineNumber);
                    $('#inpuTVehicleChasis').val(data.result.ChasisNumber);
                    $('#inputVehicleVIN').val(data.result.VinCode);
                    $('#textareaObservations').val(data.result.Description);
                    $('#divThirdPartyVehicle').show();
                } else {
                    ClaimEstimation.ClearThirdPartyVehiclePanel();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SetParticipantInformation(event, participant) {
        $('#inputThirdAffectedDocumentNumber').UifAutoComplete('setValue', participant.DocumentNumber);
        $('#inputThirdAffectedDataFullName').UifAutoComplete('setValue', participant.Name);
        affectedId = participant.IndividualId;

        ClaimEstimation.ValidateSubClaims();
    }

    static ClearParticipantInformation() {
        if (affectedId != 0) {
            ClaimEstimationRequest.GetThirdPartyByIndividualId(affectedId).done(function (response) {
                if (response.success) {
                    if ($('#inputThirdAffectedDocumentNumber').UifAutoComplete('getValue') != response.result.DocumentNumber || $('#inputThirdAffectedDataFullName').UifAutoComplete('getValue') != response.result.Name) {
                        affectedId = 0;
                    }
                }
            });
        }
    }

    static AddSubClaims() {
        if (!enabledThird)
            return;

        if ($('#inputThirdAffectedDocumentNumber').val() != "" && $('#inputThirdAffectedDataFullName').val() != "" && affectedId != 0) {

            $("#tableEstimations").UifDataTable('getData').forEach(function (estimation) {
                if ($('#tableCoverage').UifDataTable('getSelected') != null) {
                    var subSinisterSelected = null;
                    var coverage = null;

                    if ($('#' + Claim.GetTableSubSinister()).UifDataTable('getSelected') == null) {
                        //validacion en la tabla estimacion
                        coverage = $("#tableCoverage").UifDataTable('getSelected').map(function (data) {
                            data.CoverageClaimId = data.CoverageId;
                            return data;
                        });
                        coverage = coverage[0];
                    } else {
                        //validacion en la tabla de subsiniestro
                        subSinisterSelected = $('#' + Claim.GetTableSubSinister()).UifDataTable('getSelected')[0];
                        coverage = $('#tableCoverage').UifDataTable('getData').find(function (data) { return data.Id == subSinisterSelected.CoverageId; });
                    }

                    individualId = enabledThird ? affectedId : $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0].InsuredId;

                    if (individualId > 0) {
                        var indexOfSubSinister = $('#' + Claim.GetTableSubSinister()).UifDataTable('getData').findIndex(function (x) {
                            return x.CoverageId == coverage.CoverageId && x.EstimationTypeId == estimation.Id && x.RiskId == coverage.RiskId && x.IndividualId == individualId
                        });
                    }
                    else if (enabledThird && $('#inputThirdAffectedDocumentNumber').UifAutoComplete('getValue') != "" && $('#inputThirdAffectedDataFullName').UifAutoComplete('getValue') != "") {
                        var indexOfSubSinister = $('#' + Claim.GetTableSubSinister()).UifDataTable('getData').findIndex(function (x) {
                            return x.CoverageId == coverage.CoverageId
                                && x.EstimationTypeId == estimation.Id
                                && x.RiskId == coverage.RiskId
                                && x.ThirdAffectedDTO.DocumentNumber == $('#inputThirdAffectedDocumentNumber').UifAutoComplete('getValue')
                                && x.ThirdAffectedDTO.FullName == $('#inputThirdAffectedDataFullName').UifAutoComplete('getValue').toUpperCase();
                        });
                    }

                    if (indexOfSubSinister >= 0 || parseFloat(estimation.EstimateAmount) > 0) {
                        estimation.ClaimedAmount = parseFloat(NotFormatMoney($("#inputClaimedAmount").val()).replace(',', '.')).toFixed(2);
                        estimation.IsClaimedAmount = $("#_chkIsClaimedAmount").is(':checked');
                        ClaimEstimation.SaveSubSinister(estimation, indexOfSubSinister);
                    }

                }
                else {
                    $("#tableEstimations").UifDataTable('unselect');
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectARiskAndCoverage, 'autoclose': true });
                }
            });
            ClaimEstimation.ClearSubClaims();
            ClaimEstimation.ClearEstimations();
            ClaimEstimation.ClearAffectedPropertyPanel();
            ClaimEstimation.ClearDriverManDataPanel();
            ClaimEstimation.ClearThirdAffectedPanel();
            ClaimEstimation.ClearThirdPartyVehiclePanel();
            ClaimEstimation.ClearAmountPretention();
            $("#tableCoverage").UifDataTable('unselect');
            $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MessageUpdate, 'autoclose': true });

        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EstimateCompletingThird, 'autoclose': true });
        }
    }

    static EditSubClaims(event, subClaim, position) {
        enabledThird = false;
        $('#divDriverData').hide();
        $('#divThirdPartyVehicle').hide();
        $('#divAffectedProperty').hide();
        $('#divThirdAffected').hide();

        $("#" + Claim.GetTableRisk()).UifDataTable('setSelect', { label: 'RiskId', value: parseInt(subClaim.RiskId) });

        ClaimEstimation.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage({ RiskId: subClaim.RiskId }, function (coverages) {
            Claim.GetEstimationTypesByPrefixId($('#selectPrefix').UifSelect('getSelected'), function (estimationTypes) {
                estimationsDataTable = estimationTypes;

                $("#tableCoverage").UifDataTable('unselect');
                coverages = coverages.map(function (coverage) {
                    coverage.RiskId = subClaim.RiskId;
                    return coverage;
                });

                ValPositionTop = $("#inputClaimedAmount").offset().top + 2070;

                $("#tableCoverage").UifDataTable({ sourceData: coverages });
                $("#tableCoverage").UifDataTable('setSelect', { label: 'Id', value: parseInt(subClaim.CoverageId) });

                var risk = $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0];

                if (risk != null) {
                    ClaimEstimationRequest.GetInsuredsByIndividualId(risk.InsuredId).done(function (data) {
                        if (data.success) {
                            if (data.result.length > 0) {
                                $("#coverageDocument").val(data.result[0].DocumentNumber);
                                $("#coverageFullName").val(data.result[0].FullName);
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorConsultingInsured, 'autoclose': true });
                        }
                    });
                }

                prefixId = $('#selectPrefix').UifSelect('getSelected');
                coverageSelected = $('#tableCoverage').UifDataTable('getSelected')[0];

                $('#rdbtnThirdParty').prop("checked", subClaim.IsProspect);
                $('#rdbtnInsured').prop("checked", subClaim.IsInsured);

                ClaimEstimation.GetActivePanelsByCoverageId(coverageSelected, function (activePanels) {
                    if (activePanels != null) {
                        if (activePanels.IsEnabledThird && subClaim.IsProspect) {
                            $('#divThirdAffected').show();
                            $("#btnSaveSubClaim").parent().show();
                        }

                        if (activePanels.IsEnabledAffectedProperty) {
                            $('#divAffectedProperty').show();
                        }

                        if (activePanels.IsEnabledDriver) {
                            $('#divDriverData').show();
                        }

                        if (activePanels.IsEnabledThirdPartyVehicle) {
                            $('#divThirdPartyVehicle').show();
                        }
                    }

                    if (modelSearchCriteria.claimId != null && subClaim.SubClaim != null && subClaim.ClaimCoverageId != null) {
                        if (coverageSelected != null) {
                            ClaimEstimation.GetAffectedPropertyByClaimCoverageId(subClaim.ClaimCoverageId);
                            ClaimEstimation.GetClaimedAmountByClaimCoverageId(subClaim.ClaimCoverageId);
                            ClaimEstimation.GetClaimDriverInformationByClaimCoverageId(subClaim.ClaimCoverageId);
                            ClaimEstimation.GetClaimThirdPartyVehicleByClaimCoverageId(subClaim.ClaimCoverageId);

                            if (subClaim.IsProspect) {
                                affectedId = subClaim.IndividualId;
                                enabledThird = true;
                                ClaimEstimationRequest.GetThirdPartyByIndividualId(affectedId).done(function (data) {
                                    if (data.success) {
                                        $('#inputThirdAffectedDocumentNumber').UifAutoComplete('setValue', data.result.DocumentNumber);
                                        $('#inputThirdAffectedDataFullName').UifAutoComplete('setValue', data.result.Name);
                                        $('#divThirdAffected').show();
                                        $("#btnSaveSubClaim").parent().show();
                                    }
                                });
                            }
                        }
                    }
                    else {

                        //ThirdAffected
                        if (subClaim.ThirdAffectedDTO != null && subClaim.ThirdAffectedDTO.FullName != "" && subClaim.IsProspect) {
                            $('#inputThirdAffectedDataFullName').UifAutoComplete('setValue', subClaim.ThirdAffectedDTO.FullName);
                            $('#inputThirdAffectedDocumentNumber').UifAutoComplete('setValue', subClaim.ThirdAffectedDTO.DocumentNumber);
                            affectedId = subClaim.ThirdAffectedDTO.Id;
                            enabledThird = true;
                            $('#divThirdAffected').show();
                        }
                        //AffectedProperty
                        if (subClaim.AffectedProperty != null && subClaim.AffectedProperty != "") {
                            $('#InputAffectedProperty').val(subClaim.AffectedProperty);
                            $('#divAffectedProperty').show();
                        }
                        //ThirdVehiclePlate
                        if (subClaim.ThirdPartyVehicleDTO != null && subClaim.ThirdPartyVehicleDTO.Plate != "") {
                            $('#inpuTVehicleChasis').val(subClaim.ThirdPartyVehicleDTO.ChasisNumber);
                            $('#textareaObservations').val(subClaim.ThirdPartyVehicleDTO.Description);
                            $('#inpuTVehicleEngine').val(subClaim.ThirdPartyVehicleDTO.EngineNumber);
                            $('#inpuTthirdVehiclePlate').val(subClaim.ThirdPartyVehicleDTO.Plate);
                            $('#selectVehicleColor').val(subClaim.ThirdPartyVehicleDTO.VehicleColorDescription);
                            $('#inpuTVehicleMake').val(subClaim.ThirdPartyVehicleDTO.VehicleMake);
                            $('#inpuTVehicleModel').val(subClaim.ThirdPartyVehicleDTO.VehicleModel);
                            $('#inpuTthirdVehicleYear').val(subClaim.ThirdPartyVehicleDTO.VehicleYear);
                            $('#inputVehicleVIN').val(subClaim.ThirdPartyVehicleDTO.VinCode);
                            $('#divThirdPartyVehicle').show();
                        }
                        //DriverInformation
                        if (subClaim.DriverInformationDTO != null && subClaim.DriverInformationDTO.DocumentNumber != "") {
                            $('#inputDataAge').val(subClaim.DriverInformationDTO.Age);
                            $('#inputDriverDocumentNumber').val(subClaim.DriverInformationDTO.DocumentNumber);
                            $('#inputDriverDataLicenseNumber').val(subClaim.DriverInformationDTO.LicenseNumber);
                            $('#inputDriverLicenseType').val(subClaim.DriverInformationDTO.LicenseType);
                            $('#inputDriverDataVto').val(subClaim.DriverInformationDTO.LicenseValidThru);
                            $('#inputDriverFullName').val(subClaim.DriverInformationDTO.Name);
                            $('#divDriverData').show();
                        }

                        if (!isNaN(subClaim.ClaimedAmount)) {
                            $('#inputClaimedAmount').val(FormatMoney(subClaim.ClaimedAmount));
                            $("#_chkIsClaimedAmount").prop("checked", subClaim.IsClaimedAmount);
                            $('#inputClaimedAmount').prop('disabled', subClaim.IsClaimedAmount);
                        }
                        else {
                            $('#inputClaimedAmount').val("0");
                            $("#_chkIsClaimedAmount").prop("checked", false);
                        }
                    }

                    ClaimEstimation.ValidateSubClaims();
                });
            });
        });
    }

    static DeleteSubclaims(event, data, position) {
        if (data.SubClaim === undefined) {
            $('#' + Claim.GetTableSubSinister()).UifDataTable('deleteRow', position);
            Claim.AddDataTableSummaryClaim();
            $("#tableCoverage").UifDataTable('unselect');
            ClaimEstimation.ClearEstimations();
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NotClaimDelete, 'autoclose': true });
        }
    }

    static GetStatuses() {
        return new Promise(function (success) {
            ClaimEstimationRequest.GetStatuses().done(function (data) {
                if (data.success) {
                    estimationTypeStatuses = data.result;
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
                success();
            });
        });

    }

    static GetReasonsByPrefixId() {
        var prefixId = $('#selectPrefix').UifSelect('getSelected');
        return new Promise(function (success) {
            ClaimEstimationRequest.GetReasonsByPrefixId(prefixId).done(function (data) {
                if (data.success) {
                    estimationTypeStatusReasons = data.result;
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
                success();
            });
        });
    }

    static ModifyClaimedAmount() {
        var coverage = $('#tableCoverage').UifDataTable('getSelected')[0];
        var subclaim = $('#' + Claim.GetTableSubSinister()).UifDataTable('getData');
        subclaim.forEach(function (item, index) {
            if (item.CoverageId == coverage.CoverageId && item.IndividualId == $("#" + Claim.GetTableRisk()).UifDataTable('getSelected')[0].InsuredId) {
                item.ClaimedAmount = NotFormatMoney($('#inputClaimedAmount').val());
                item.IsClaimedAmount = $("#_chkIsClaimedAmount").is(':checked');
            }
        });
    }

    static SelectedAmountType(event, seletedItem) {
        if (seletedItem.Id == 1) {
            if (!($("#" + Claim.GetTableSubSinister()).UifDataTable('getData').find(x => x.EstimationTypeId == $("#tableEstimations").UifDataTable("getSelected")[0].EstimationId) && $("#" + Claim.GetTableSubSinister()).UifDataTable('getData').find(x => x.EstimationTypeId == $("#tableEstimations").UifDataTable("getSelected")[0].EstimationId).ClaimId))
                $("#selectClaimCurrency").UifSelect("disabled", false);

            //TERCERO
            if ($('#rdbtnThirdParty').prop("checked")) {
                $("#selectStateCode").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectReason").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectAmountType").parent().parent().removeClass("uif-col-3 uif-col-4 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                $("#inputDeductible").parent().hide();
                $("#inputSalaryNumeric").parent().hide();
                $("#LabelEstimation").text(Resources.Language.Estimation);
                $('#inputEstimation').prop("disabled", false);
            } else {//ASEGURADO
                $("#selectStateCode").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectReason").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputDeductible").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectAmountType").parent().parent().removeClass("uif-col-3 uif-col-4 uif-col-6").addClass("uif-col-4");
                $("#inputDeductible").parent().show();
                $("#inputSalaryNumeric").parent().hide();
                $("#LabelEstimation").text(Resources.Language.Estimation);
                $('#inputEstimation').prop("disabled", false);
            }

        } else {
            if (!($("#" + Claim.GetTableSubSinister()).UifDataTable('getData').find(x => x.EstimationTypeId == $("#tableEstimations").UifDataTable("getSelected")[0].EstimationId) && $("#" + Claim.GetTableSubSinister()).UifDataTable('getData').find(x => x.EstimationTypeId == $("#tableEstimations").UifDataTable("getSelected")[0].EstimationId).ClaimId))
                $("#selectClaimCurrency").UifSelect("disabled", true);

            $("#selectClaimCurrency").UifSelect('setSelected', '0');
            $('#selectClaimCurrency').trigger('change');
            //TERCERO
            if ($('#rdbtnThirdParty').prop("checked")) {
                $("#selectStateCode").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectReason").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectAmountType").parent().parent().removeClass("uif-col-3 uif-col-4 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputSalaryNumeric").parent().removeClass("uif-col-3 uif-col-4 uif-col-6").addClass("uif-col-4");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputDeductible").parent().hide();
                $("#inputSalaryNumeric").parent().show();
                $("#LabelEstimation").text(Resources.Language.LabelSalaryEquivalent);
            } else {//ASEGURADO
                $("#selectStateCode").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectReason").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectAmountType").parent().parent().removeClass("uif-col-3 uif-col-4 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-3");
                $("#inputSalaryNumeric").parent().removeClass("uif-col-3 uif-col-4 uif-col-6").addClass("uif-col-3");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-3");
                $("#inputDeductible").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-3");
                $("#inputDeductible").parent().show();
                $("#inputSalaryNumeric").parent().show();
                $("#LabelEstimation").text(Resources.Language.LabelSalaryEquivalent);
            }
        }
    }

    static CalculateEstimationBySalaryNumber() {
        if (SalaryMinimumMounth != null && $('#inputSalaryNumeric').val() != "") {
            var SalaryNumber = parseFloat($('#inputSalaryNumeric').val().replace(',', '.')).toFixed(2);
            $('#inputEstimation').val(FormatMoney(SalaryNumber * SalaryMinimumMounth));
            $('#inputSalaryNumeric').val(SalaryNumber);
        }
    }

    static CalculateSalaryNumberByEstimation() {
        if (SalaryMinimumMounth != null && $('#inputEstimation').val() != "" && $("#selectAmountType").UifSelect('getSelected') == 2) {
            var EstimationAmount = parseFloat(NotFormatMoney($('#inputEstimation').val()).replace(',', '.')).toFixed(2);
            var SalaryNumber = parseFloat(EstimationAmount / SalaryMinimumMounth).toFixed(2);
            $('#inputEstimation').val(EstimationAmount.replace('.', ','));
            $('#inputSalaryNumeric').val(SalaryNumber.replace('.', ','));
        }
    }

    static ValidateClaimedAmount() {
        if ($("#_chkIsClaimedAmount").is(':checked')) {
            $('#inputClaimedAmount').val("0");
            $('#inputClaimedAmount').prop('disabled', true);
        } else {
            $('#inputClaimedAmount').val("0");
            $('#inputClaimedAmount').prop('disabled', false);
        }
    }

    static ClickdRbtnInsuredOrThirdParty(isInsured) {
        enabledThird = false;
        if ($("#tableCoverage").UifDataTable('getSelected') != null && $("#tableCoverage").UifDataTable('getSelected').length > 0) {
            var coverageSeleceted = $("#tableCoverage").UifDataTable('getSelected')[0];
            ClaimEstimationRequest.GetActivePanelsByCoverageId(coverageSeleceted.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        var result = data.result;

                        $('#divDriverData').hide();
                        $('#divThirdPartyVehicle').hide();
                        $('#divAffectedProperty').hide();
                        $('#divThirdAffected').hide();

                        $('#rdbtnInsured').prop("disabled", true);
                        $('#rdbtnThirdParty').prop("disabled", true);

                        if (result.IsEnabledDriver) {
                            $('#divDriverData').show();
                        }
                        else {
                            $('#divDriverData').hide();
                        }

                        if (result.IsEnabledThirdPartyVehicle) {
                            $('#divThirdPartyVehicle').show();
                        }
                        else {
                            $('#divThirdPartyVehicle').hide();
                        }

                        if (result.IsEnabledAffectedProperty) {
                            $('#divAffectedProperty').show();
                        } else {
                            $('#divAffectedProperty').hide();
                        }

                        if (!isInsured && result.IsEnabledThird) {
                            enabledThird = result.IsEnabledThird;
                            $("#btnSaveSubClaim").parent().show();
                            $('#divThirdAffected').show();
                            $('#rdbtnThirdParty').prop("disabled", false);
                            $('#rdbtnInsured').prop("disabled", false);

                            $('#rdbtnThirdParty').prop("checked", true);
                            $('#rdbtnInsured').prop("checked", false);
                        }
                        else {
                            $("#btnSaveSubClaim").parent().hide();
                            $('#rdbtnThirdParty').prop("disabled", false);
                            $('#rdbtnInsured').prop("disabled", false);
                            $('#divThirdAffected').hide();
                            $('#rdbtnThirdParty').prop("checked", false);
                            $('#rdbtnInsured').prop("checked", true);
                        }
                    } else {
                        $('#rdbtnInsured').prop("checked", true);
                        $('#rdbtnThirdParty').prop("disabled", true);
                        $('#rdbtnInsured').prop("disabled", true);
                        $('#divDriverData').hide();
                        $('#divThirdPartyVehicle').hide();
                        $('#divAffectedProperty').hide();
                        $('#divThirdAffected').hide();
                    }

                    ClaimEstimation.ValidateSubClaims();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectARiskAndCoverage, 'autoclose': true });
        }
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if ($(this).val() != '')
            $(this).val(FormatMoney($(this).val().includes(',') ? $(this).val().replace(',', '.') : $(this).val()));
    }

    static DeselectedRisk() {
        $("#tableCoverage").UifDataTable('clear');
        ClaimEstimation.ClearInsured();
        ClaimEstimation.ClearSubClaims();
        ClaimEstimation.ClearEstimations();
        ClaimEstimation.ClearAffectedPropertyPanel();
        ClaimEstimation.ClearDriverManDataPanel();
        ClaimEstimation.ClearThirdAffectedPanel();
        ClaimEstimation.ClearThirdPartyVehiclePanel();
        ClaimEstimation.ClearAmountPretention();
    }

    static ClearAmountPretention() {
        $('#inputClaimedAmount').val("0");
        $("#inputClaimedAmount").prop("disabled", false);
        $("#_chkIsClaimedAmount").prop("checked", false);
    }

    static ClearInsured() {
        $("#coverageDocument").val("");
        $("#coverageFullName").val("");
    }

    static ClearSubClaims() {
        if ($('#rdbtnThirdParty').is(":checked")) {
            Claim.GetEstimationTypesByPrefixId($('#selectPrefix').UifSelect('getSelected'), function (estimationTypes) {
                $("#tableEstimations").UifDataTable({ sourceData: Object.assign([], estimationTypes) });
            });
        }
        else {
            $("#tableEstimations").UifDataTable({ sourceData: Object.assign([], estimationsDataTable) });
        }

        individualId = null;
    }

    static ClearEstimations() {
        $("#tableEstimations").UifDataTable('clear');
    }

    static ClearThirdPartyVehiclePanel() {
        $('#inpuTthirdVehiclePlate').UifAutoComplete('clean');
        $('#inpuTVehicleMake').UifAutoComplete('clean');
        $('#inpuTVehicleModel').UifAutoComplete('clean');
        $('#inpuTthirdVehicleYear').val("");
        $('#selectVehicleColor').UifSelect('setSelected', -1);
        $('#inpuTVehicleEngine').val("");
        $('#inpuTVehicleChasis').val("");
        $('#inputVehicleVIN').val("");
        $('#textareaObservations').val("");
        $('#divThirdPartyVehicle').hide();
    }

    static ClearDriverManDataPanel() {
        $('#inputDriverDocumentNumber').UifAutoComplete('clean');
        $('#inputDriverFullName').UifAutoComplete('clean');
        $('#inputDriverLicenseType').val("");
        $('#inputDriverDataLicenseNumber').val("");
        $('#inputDriverDataVto').val("");
        $('#inputDataAge').val("");
        $('#divDriverData').hide();
    }

    static ClearAffectedPropertyPanel() {
        $('#InputAffectedProperty').val("");
        $('#divAffectedProperty').hide();
    }

    static ClearThirdAffectedPanel() {
        $('#inputThirdAffectedDocumentNumber').UifAutoComplete('clean');
        $('#inputThirdAffectedDataFullName').UifAutoComplete('clean');
        affectedId = 0;
        $('#divThirdAffected').hide();
        $("#btnSaveSubClaim").parent().hide();
    }
}
