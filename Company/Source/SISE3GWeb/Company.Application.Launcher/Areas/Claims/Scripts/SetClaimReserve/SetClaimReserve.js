var policyConsulted = false;
var SalaryMinimumMounth = null;
var policyEndorsementNumber = null;
var policyBusinessTypeId = 0;
var policyTypeId = 0;
var policyProductId = 0;
var policyCurrency = null;
var policyBranchId = null;
var exchangeRate = null;
var enableZeroEstimation = false;
var MinimumSalaryValue = null;
var accountingDate = null;

class SetClaimReserve extends Uif2.Page {

    getInitialState() {
        SetClaimReserve.InitialSetClaimReserve();
    }

    bindEvents() {
        // Events
        $('#btnSearchClaim').on('click', SetClaimReserve.SearchClaim);
        $('#btnHistoryReserve').on('click', SetClaimReserve.GetClaimModifiesByClaimId)
        $('#btnViewDataPolicy').on('click', SetClaimReserve.OpenModalPolicyConsult);
        $('#btnSaveEditReserve').on('click', SetClaimReserve.SaveReserve);
        $('#btnCancelEditReserve').on('click', SetClaimReserve.CancelReserve);
        $('#btnSaveClaimReserve').on('click', SetClaimReserve.ExecuteReserveOperations);
        $('#btnClearFormReserve').on('click', SetClaimReserve.CleanForm);
        $("#inputReserve").focusin(Salvage.NotFormatMoneyIn);
        $("#inputReserve").focusout(Salvage.FormatMoneyOut);
        $("#selectPrefix").on('itemSelected', SetClaimReserve.OnChange);
        $("#selectBranch").on('itemSelected', SetClaimReserve.OnChange);
        $("#InputClaimNumberSearch").change(SetClaimReserve.OnChange);

        // DataTables
        $('#tblEstimation').on('rowEdit', function (event, data, position) {
            SetClaimReserve.ToEditEstimation(event, data, position);
        });

        $('#tblEstimation').on('rowSelected', function (event, selectedRow) {
            SetClaimReserve.GetInsuredAmount(event, selectedRow);
            SetClaimReserve.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(event, selectedRow);
            SetClaimReserve.LoadCoverageNameInTableTitles(event, selectedRow);
        });
        // Selects
        $('#selectStateCode').on('itemSelected', SetClaimReserve.GetReasonsByStatusIdPrefixId);
        $('#selectAmountType').on('itemSelected', SetClaimReserve.SelectedAmountType);
        $("#inputSalaryNumeric").on("change", SetClaimReserve.CalculateEstimationBySalaryNumber);
        $('#selectClaimCurrency').on('itemSelected', Claim.GetExchangeRate);
    }

    /////////////////////////////////
    // InitialState
    static InitialSetClaimReserve() {

        if (modelSearchCriteria.claimNumber == null && modelSearchCriteria.policyDocumentNumber == null && modelSearchCriteria.prefixId == null && modelSearchCriteria.branchId == null && modelSearchCriteria.paymentRequestId == null) {
            SetClaimReserve.GetPrefixes();
            SetClaimReserve.GetBranches();
        }

        $('#editForm').hide();

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

        //Obtener fecha contable
        RecoveryRequest.GetAccountingDate().done(function (response) {
            if (response.success) {
                accountingDate = response.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        $("#labelEstimation").text(0);
        $("#labelPayments").text(0);
        $("#labelIncome").text(0);
        $("#labelReservations").text(0);

        SetClaimReserve.DoSetClaimReserve();
    }

    /////////////////////////////////
    // Metodos de Consulta

    static GetBranches(callback) {
        SetClaimReserveRequest.GetBranches().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result)
                $('#selectBranch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPrefixes(callback) {
        SetClaimReserveRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result)
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetInsuredAmount(event, selectedRow) {
        lockScreen();
        SetClaimReserveRequest.GetInsuredAmount($('#_policyId').val(), selectedRow.RiskNumber, selectedRow.CoverageId, selectedRow.CoverageNumber, selectedRow.ClaimId, selectedRow.SubClaim).done(function (data) {
            $("#tblLimits").UifDataTable('clear');
            if (data.success) {
                $("#tblLimits").UifDataTable({ sourceData: [data.result] });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });
    }
    static GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(event, selectedRow) {
        lockScreen();
        SetClaimReserveRequest.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber($('#_policyId').val(), selectedRow.RiskNumber, selectedRow.CoverageId, selectedRow.CoverageNumber).done(function (data) {
            $("#tblDeductibles").UifDataTable('clear');
            if (data.success) {
                $("#tblDeductibles").UifDataTable({ sourceData: data.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });

    }


    static LoadCoverageNameInTableTitles(event, selectedRow) {
        $(".panel-table h3.panel-title").each(function (index, element) {
            if (index > 0) {

                var indexSpace = $(element).text().indexOf(" ");
                var title;

                if (indexSpace >= 0) {
                    title = $(element).text().substring(0, indexSpace);
                }
                else {
                    title = $(element).text();
                }

                $(element).text(title + " (" + selectedRow.CoverageDescription + ")");
            }
        });
    }

    static GetReasonsByStatusIdPrefixId(event, seletedItem, callback) {
        var prefixId = $('#selectPrefix').UifSelect('getSelected');
        SetClaimReserveRequest.GetReasonsByStatusIdPrefixId(seletedItem.Id, prefixId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                enableZeroEstimation = $("#selectStateCode").UifSelect('getSelectedSource').InternalStatus.Id == 2;
                $("#selectReason").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
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

    static GetPolicyInfo(endorsementId) {
        lockScreen();
        SetClaimReserveRequest.GetPolicyByEndorsementIdModuleType(endorsementId).done(function (data) {
            if (data.success) {
                $("#_businessTypeId").val(data.result.BusinessTypeId);
                $('#_documentNumber').val(data.result.DocumentNumber);
                $('#InputPolicyDocumentNumber').val(data.result.DocumentNumber);
                $('#InputPolicyEndorsement').val(data.result.EndorsementDocumentNum);
                $('#InputPolicyCurrency').val(data.result.CurrencyDescription);
                $('#InputPolicyHolder').val(data.result.HolderName);
                $('#selectPrefix').UifSelect('setSelected', data.result.PrefixId);
                $('#selectBranch').UifSelect('setSelected', data.result.BranchId);

                //Pestaña Datos Póliza
                policyEndorsementNumber = data.result.EndorsementDocumentNum;
                policyBusinessTypeId = data.result.BusinessTypeId;
                policyTypeId = data.result.PolicyTypeId;
                policyProductId = data.result.ProductId;
                policyCurrency = data.result.CurrencyId;
                policyBranchId = data.result.BranchId;
                SetClaimReserve.ValidateCurrencyEstimation(policyCurrency);
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
                $('#divLabelInformationPolicy').css('display', 'block');
                $('#inputInformClaimReserve').text(data.result.BranchDescription.substring(0, 3) + '-' + data.result.PrefixDescription.substring(0, 3) + '-' + data.result.DocumentNumber + '-' + data.result.EndorsementDocumentNum);
                policyConsulted = true;
                SetClaimReserve.CalculateEstimationSubSinister();
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
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

    /////////////////////////////////
    // Seccion Editar
    static ToEditEstimation(event, estimation, position) {
        enableZeroEstimation = false;
        ClaimRequest.GetEstimationTypesSalariesEstimation().done(function (response) {
            if (response.success) {
                if (response.result.some(x => x == estimation.EstimationTypeId)) {
                    $("#selectAmountType").parent().parent().show();
                    $('#selectAmountType').UifSelect('setSelected', estimation.IsMinimumSalary ? 2 : 1);
                    $('#selectAmountType').trigger('change');
                    $('#inputSalaryNumeric').val(estimation.MinimumSalariesNumber);
                } else {
                    $('#selectAmountType').UifSelect('setSelected', 1);
                    $("#selectAmountType").parent().parent().hide();
                    $('#inputSalaryNumeric').val('');
                    $("#inputSalaryNumeric").parent().hide();
                    $("#LabelEstimation").text(Resources.Language.Reserve);
                    $('#inputReserve').prop("disabled", false);

                    $("#inputCoverage").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                    $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                    $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                    $("#inputReserve").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        if (parseInt(estimation.EstimationTypeEstatus) > 0) {
            $('#selectClaimCurrency').UifSelect('setSelected', estimation.CurrencyId);
            $('#selectClaimCurrency').trigger('change');

            SetClaimReserve.GetStatusesByEstimationTypeId(estimation.EstimationTypeId, function (statuses) {
                $("#selectStateCode").UifSelect({ sourceData: statuses });
                $("#selectStateCode").UifSelect('setSelected', estimation.EstimationTypeEstatus);
            });

            var seletedItem = {
                Id: estimation.EstimationTypeEstatus
            };

            if (!CompareClaimDates(moment(estimation.CreationDate).format('DD/MM/YYYY'), "01/01/0001")) {
                $("#selectStateCode").UifSelect('disabled', true);
                $('#selectClaimCurrency').UifSelect('disabled', false);

                if (SalaryMinimumMounth != null)
                    $('#selectAmountType').UifSelect('disabled', false);

            } else {
                $('#selectClaimCurrency').UifSelect('disabled', true);
                $('#selectAmountType').UifSelect('disabled', true);
            }

            SetClaimReserve.GetReasonsByStatusIdPrefixId(null, seletedItem, function (reasons) {
                $("#selectReason").UifSelect({ sourceData: reasons });
                $("#selectReason").UifSelect('setSelected', estimation.EstimationTypeEstatusReasonCode);
                $('#editForm').show();
            });
        } else {
            if (policyCurrency != null) {
                $('#selectClaimCurrency').UifSelect('setSelected', policyCurrency);
                $('#selectClaimCurrency').trigger('change');
            }

            if (!CompareClaimDates(moment(estimation.CreationDate).format('DD/MM/YYYY'), "01/01/0001")) {
                $('#selectClaimCurrency').UifSelect('disabled', false);

                if (SalaryMinimumMounth != null)
                    $('#selectAmountType').UifSelect('disabled', false);
            }

            SetClaimReserve.GetStatusesByEstimationTypeId(estimation.EstimationTypeId);
            $("#selectReason").UifSelect({ sourceData: null });
            $('#editForm').show();
        }

        $('#inputRisk').val(estimation.Risk);
        $('#inputCoverage').val(estimation.CoverageDescription);
        $('#inputEstimation').val(FormatMoney(estimation.OriginalEstimation));
        $('#inputReserve').val(FormatMoney(estimation.Reserve));
        $('#_position').val(position);
    }

    /////////////////////////////////
    // Seccion Guardar
    static SaveReserve() {
        $("#editForm").validate();
        if ($("#editForm").valid()) {

            var index = $('#_position').val();
            var estimation = $('#tblEstimation').UifDataTable('getData')[index];

            if (exchangeRate != null) {
                var coverageInsuredAmount = parseInt($("#selectClaimCurrency").UifSelect("getSelected")) != policyCurrency ? (policyCurrency > 0 ? parseFloat((estimation.InsuredAmountTotal * exchangeRate.SellAmount).toFixed(2)) : parseFloat((estimation.InsuredAmountTotal / exchangeRate.SellAmount).toFixed(2))) : estimation.InsuredAmountTotal;
                if (parseFloat(NotFormatMoney($('#inputEstimation').val())) == 0 && parseFloat(NotFormatMoney($('#inputReserve').val())) == 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ReservationHigherZero, 'autoclose': true });
                    return;
                } else if (estimation.EstimationTypeId == 1 /*INDEMNIZACIÓN*/ && (parseFloat(NotFormatMoney($('#inputReserve').val()).replace(',', '.')).toFixed(2) > coverageInsuredAmount)) {
                    $.UifNotify('show', { 'type': 'info', 'message': String.format(Resources.Language.TheEstimateMustNotExceedTheCoverage, $("#selectClaimCurrency").UifSelect('getSelectedText'), FormatMoney(coverageInsuredAmount)), 'autoclose': true });
                    return;
                } else if (!enableZeroEstimation && parseFloat(NotFormatMoney($('#inputReserve').val()).replace(',', '.')).toFixed(2) < estimation.PaymentValue) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ReserveValueMustBeGreater, 'autoclose': true });
                    return;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExchangeRateNotParametrized, 'autoclose': true });
                return false;
            }

            var IsMinimumSalary = 0;
            MinimumSalaryValue = null;

            if ($('#selectAmountType').UifSelect('getSelected') == 2) {
                IsMinimumSalary = 1;
                MinimumSalaryValue = SalaryMinimumMounth;
            }

            var statusSelected = $('#selectStateCode').UifSelect("getSelectedSource");

            estimation.Reserve = enableZeroEstimation ? estimation.PaymentValue : parseFloat(NotFormatMoney($('#inputReserve').val()).replace(',', '.')).toFixed(2)
            estimation.EstimationTypeEstatusReasonCode = $('#selectReason').UifSelect("getSelected");
            estimation.EstimationTypeEstatus = statusSelected.Id;
            estimation.Currency = $("#selectClaimCurrency").UifSelect("getSelectedText");
            estimation.CurrencyId = $("#selectClaimCurrency").UifSelect("getSelected");
            estimation.InternalStatusId = statusSelected.InternalStatus.Id;
            estimation.IsMinimumSalary = IsMinimumSalary;
            estimation.CreationDate = accountingDate;
            estimation.MinimumSalariesNumber = $('#inputSalaryNumeric').val();
            estimation.MinimumSalaryValue = MinimumSalaryValue;
            estimation.ExchangeRate = exchangeRate.SellAmount;
            estimation.IsEdited = true;
            $('#tblEstimation').UifDataTable('editRow', estimation, index);
            SetClaimReserve.CalculateEstimationSubSinister();
            $('#editForm').hide();
            $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MessageUpdate, 'autoclose': true });
        }
    }

    /////////////////////////////////
    // Seccion Buttons Events
    static SearchClaim() {
        $("#formClaimSearch").validate();
        if ($("#formClaimSearch").valid()) {
            lockScreen();
            var prefixId = $("#selectPrefix").UifSelect('getSelected');
            var branchId = $("#selectBranch").UifSelect('getSelected');
            var policyDocumentNumber = $("#inputPolicyDocumentNumber").val();
            var claimNumber = $("#InputClaimNumberSearch").val();

            SetClaimReserveRequest.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber).done(function (data) {
                if (data.success && data.result.length > 0) {

                    $('#_endorsementId').val(data.result[0].EndorsementId);
                    $('#_riskId').val(data.result[0].RiskId);
                    $('#_number').val(data.result[0].Number);
                    $('#_individualId').val(data.result[0].IndividualId);
                    $('#_claimId').val(data.result[0].ClaimId);
                    $('#btnHistoryReserve').prop("disabled", false);
                    data.result = data.result.map(function (dataResult) {
                        dataResult.OriginalEstimation = dataResult.EstimateAmount;
                        dataResult.OriginalReserve = dataResult.EstimateAmount - dataResult.PaymentValue;
                        dataResult.Affected = dataResult.Insured;
                        dataResult.Reserve = 0;
                        return dataResult;
                    });

                    $("#tblEstimation").UifDataTable({ sourceData: data.result });

                    SetClaimReserve.GetPolicyInfo(data.result[0].EndorsementId);
                }
                else {
                    SetClaimReserve.CleanForm();
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotFindClaim, 'autoclose': true });
                }
            }).always(function () {
                unlockScreen();
            });
        }
    }

    static GetClaimModifiesByClaimId() {
        $("#formClaimSearch").validate();
        if ($("#formClaimSearch").valid()) {
            var claimid = $('#_claimId').val();
            SetClaimReserveRequest.GetClaimModifiesByClaimId(claimid).done(function (data) {
                if (data.success) {
                    data.result = data.result.map(function (dataResult) {
                        dataResult.OriginalEstimation = dataResult.EstimateAmount;
                        dataResult.OriginalReserve = dataResult.EstimateAmount - dataResult.PaymentValue;
                        dataResult.Affected = dataResult.Insured;
                        dataResult.CreationDate = moment(dataResult.CreationDate).format('YYYY-MM-DD HH:mm');
                        dataResult.AccountingDate = moment(dataResult.AccountingDate).format('YYYY-MM-DD HH:mm');
                        dataResult.Reserve = 0;
                        return dataResult;
                    });

                    $("#tblHistoryReserve").UifDataTable({ sourceData: data.result });
                    $('#modalHistoryClaim').UifModal('showLocal', Resources.Language.HistoryReserve);
                }
            });
        }
    }

    static OnChange() {
        $('#_endorsementId').val();
        $('#_riskId').val();
        $('#_number').val();
        $('#_individualId').val();
        $('#_claimId').val();

        $("#tblEstimation").UifDataTable('clear');
        $("#tblLimits").UifDataTable('clear');
        $("#tblDeductibles").UifDataTable('clear');
    }

    static CancelReserve() {
        $('#editForm').hide();
    }

    static OpenModalPolicyConsult() {
        if (policyConsulted) {
            SetClaimReserve.GetClaimsByPolicyId($('#_policyId').val());
            $('#modalPolicyConsult').UifModal('showLocal', Resources.Language.InfoSummaryPolicy);
            $('#modalPolicyConsult .modal-dialog.modal-lg').attr('style', 'width: 55%');
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNeedPolicy, 'autoclose': true });
        }
    }

    static GetClaimsByPolicyId(policyId) {
        SetClaimReserveRequest.GetClaimsByPolicyId(policyId).done(function (data) {
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

    static ExecuteReserveOperations() {
        $("#btnSaveClaimReserve").attr("disabled", true);
        $("#formClaimSearch").validate();
        if ($("#formClaimSearch").valid() && $("#tblEstimation").UifDataTable('getData').length > 0) {
            $("#btnSaveClaimReserve").attr("disabled", false);
            lockScreen();
            var claimReserveDTO = {
                ClaimId: parseInt($('#_claimId').val()),
                BusinessTypeId: $('#tblEstimation').UifDataTable('getData')[0].BusinessTypeId,
                IsTotalParticipation: $('#tblEstimation').UifDataTable('getData')[0].IsTotalParticipation,
                PolicyBusinessTypeId: policyBusinessTypeId,
                EndorsementId: $('#tblEstimation').UifDataTable('getData')[0].EndorsementId,
                EndorsementNumber: policyEndorsementNumber,
                PolicyId: $('#tblEstimation').UifDataTable('getData')[0].PolicyId,
                OccurrenceDate: FormatDate($('#tblEstimation').UifDataTable('getData')[0].OccurrenceDate),
                JudicialDecisionDate: FormatDate($('#tblEstimation').UifDataTable('getData')[0].JudicialDecisionDate),
                NoticeId: $('#tblEstimation').UifDataTable('getData')[0].NoticeId,
                NoticeDate: FormatDate($('#tblEstimation').UifDataTable('getData')[0].NoticeDate),
                CauseId: $('#tblEstimation').UifDataTable('getData')[0].CauseId,
                Description: $('#tblEstimation').UifDataTable('getData')[0].Operation,
                Location: $('#tblEstimation').UifDataTable('getData')[0].Location,
                CountryId: $('#tblEstimation').UifDataTable('getData')[0].CountryId,
                StateId: $('#tblEstimation').UifDataTable('getData')[0].StateId,
                CityId: $('#tblEstimation').UifDataTable('getData')[0].CityId,
                PrefixId: $('#tblEstimation').UifDataTable('getData')[0].PrefixId,
                DamageTypeId: $('#tblEstimation').UifDataTable('getData')[0].DamageTypeCode,
                DamageResponsabilityId: $('#tblEstimation').UifDataTable('getData')[0].DamageResponsibilityCode,

                BranchId: policyBranchId,
                PolicyTypeId: policyTypeId,
                PolicyProductId: policyProductId,
                Number: $('#_number').val(),
                PolicyDocumentNumber: $('#_documentNumber').val(),

                Modifications: {
                    ClaimCoverages: {
                        Estimations: {
                        }
                    }
                }
            };

            var risksCoverage = [];
            var estimations = [];
            var claimCoverages = [];
            var modifications = [];

            var tblEstimation = $('#tblEstimation').UifDataTable('getData').filter(function (data) { return parseFloat(data.Reserve) > 0 || parseFloat(data.OriginalEstimation) > 0; });

            tblEstimation.forEach(function (item, index) {
                var coverageId = risksCoverage.find(function (data) { return data.CoverageId == item.CoverageId });
                if (coverageId == null) {
                    risksCoverage.push(item);
                }
            });

            var objectModification = {
                AccountingDate: $("#InputAccountingDate").val(),
                RegistrationDate: new Date(),
                UserId: "0",
                ClaimId: parseInt($('#_claimId').val()),
            };

            risksCoverage.forEach(function (item, index) {
                var objectClaimCoverage = {
                    Id: item.ClaimCoverageId,
                    IndividualId: item.IndividualId,
                    CoverageNum: item.CoverageNumber,
                    RiskNum: item.RiskNumber,
                    RiskId: item.RiskId,
                    CoverageId: item.CoverageId,
                    SubClaim: item.SubClaim,
                    ClaimCode: item.ClaimId,
                    ClaimCoverageCode: item.ClaimCoverageId,
                    IsInsured: item.IsInsured,
                    IsProspect: item.IsProspect,
                    EndorsementId: item.EndorsementId,
                    CoverageInsuredAmount: item.InsuredAmountTotal,
                    ClaimedAmount: item.ClaimedAmount,
                    IsClaimedAmout: item.IsClaimedAmout
                };

                claimCoverages.push(objectClaimCoverage);
            });

            objectModification.ClaimCoverages = claimCoverages;

            claimCoverages.forEach(function (item, index) {
                var estimation = tblEstimation.find(function (data) { return data.CoverageId == item.CoverageId && (parseFloat(data.Reserve) > 0 || parseFloat(data.OriginalEstimation) > 0); });
                tblEstimation.forEach(function (itemEstimation, index) {
                    if (itemEstimation.CoverageId == estimation.CoverageId && (parseFloat(itemEstimation.Reserve) > 0 || parseFloat(itemEstimation.OriginalEstimation) > 0)) {
                        var objectEstimations = {
                            CurrencyId: parseInt(itemEstimation.CurrencyId),
                            ReasonId: parseInt(itemEstimation.EstimationTypeEstatusReasonCode),
                            EstimateAmount: parseFloat(itemEstimation.Reserve) > 0 ? parseFloat(itemEstimation.Reserve) : parseFloat(itemEstimation.OriginalEstimation),
                            DeductibleAmount: parseInt(itemEstimation.DeductibleAmount),
                            Id: parseInt(itemEstimation.EstimationTypeId),
                            StatusCodeId: parseInt(itemEstimation.EstimationTypeEstatus),
                            InternalStatusId: parseInt(itemEstimation.InternalStatusId),
                            CreationDate: moment(itemEstimation.CreationDate).toDate(),
                            IsMinimumSalary: itemEstimation.IsMinimumSalary,
                            MinimumSalariesNumber: parseFloat(itemEstimation.MinimumSalariesNumber),
                            MinimumSalaryValue: itemEstimation.MinimumSalaryValue,
                            ExchangeRate: itemEstimation.ExchangeRate,
                            EstimateAmountAccumulate: itemEstimation.EstimateAmountAccumulate,
                            CoverageInsuredAmountEquivalent: (policyCurrency != parseInt(itemEstimation.CurrencyId) ? (policyCurrency > 0 ? parseFloat((itemEstimation.InsuredAmountTotal * itemEstimation.ExchangeRate).toFixed(2)) : parseFloat((itemEstimation.InsuredAmountTotal / itemEstimation.ExchangeRate).toFixed(2))) : itemEstimation.InsuredAmountTotal)
                        };
                        estimations.push(objectEstimations);
                    }
                });
                if (estimations != null) {
                    objectModification.ClaimCoverages[index].Estimations = estimations;
                    estimations = [];
                }
            });

            modifications.push(objectModification);

            claimReserveDTO.Modifications = modifications;

            SetClaimReserveRequest.ExecuteReserveOperations(claimReserveDTO).done(function (data) {
                if (data.success) {
                    LaunchPolicies.ValidateInfringementPolicies(data.result.AuthorizationPolicies, true);
                    var countAutorizationPolicies = data.result.AuthorizationPolicies.filter(x => x.Type == TypeAuthorizationPolicies.Authorization || x.Type == TypeAuthorizationPolicies.Restrictive).length;
                    if (countAutorizationPolicies > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.AuthorizationPolicies, data.result.TemporalId, FunctionType.Claim);
                    } else {
                        $.UifDialog('alert', {
                            message: String.format(Resources.Language.ReserveSuccessfullyAdded + ' ' + Resources.Language.OfClaim, $('#_number').val())
                        }, function (result) {
                            SetClaimReserve.CleanForm();
                        });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            }).always(function () {
                unlockScreen();
            });
        }
        else {
            $("#btnSaveClaimReserve").attr("disabled", false);
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.QueryClaim, 'autoclose': true });
        }
    }

    static SelectedAmountType(event, seletedItem) {
        switch (parseInt(seletedItem.Id)) {
            case 1:
                $("#inputSalaryNumeric").parent().hide();
                $("#LabelEstimation").text(Resources.Language.Reserve);
                $('#inputReserve').prop("disabled", false);
                $("#selectClaimCurrency").UifSelect('setSelected', '0');
                $("#selectClaimCurrency").trigger('change');

                $("#inputCoverage").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                $("#inputReserve").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                break;
            case 2:
                $("#inputSalaryNumeric").parent().show();
                $("#LabelEstimation").text(Resources.Language.LabelSalaryEquivalent);
                $('#inputReserve').prop("disabled", true);

                $("#inputCoverage").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputReserve").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputSalaryNumeric").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                break;
            default:
                $("#inputSalaryNumeric").parent().hide();
                $("#LabelEstimation").text(Resources.Language.Reserve);
                $('#inputReserve').prop("disabled", false);

                $("#inputCoverage").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#inputEstimation").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-4");
                $("#selectClaimCurrency").parent().parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                $("#inputReserve").parent().removeClass("uif-col-4 uif-col-3 uif-col-6").addClass("uif-col-6");
                break;
        }
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

    static CalculateEstimationBySalaryNumber() {
        if ($('#inputSalaryNumeric').val() != "") {
            var SalaryNumber = $('#inputSalaryNumeric').val();
            $('#inputReserve').val(FormatMoney(SalaryNumber * SalaryMinimumMounth));
        }
    }

    static CalculateEstimationSubSinister() {
        var tableData = $("#tblEstimation").UifDataTable('getData');
        var estimation = 0;
        var payments = 0;
        var reserve = 0;

        $.each(tableData, function (index, value) {
            if (this.EstimateAmount != '') {
                estimation += parseFloat(this.EstimateAmount == null || isNaN(this.EstimateAmount) ? 0 : this.EstimateAmount);
                // reserve += (policyCurrency > 0 ? this.EstimateAmount / this.ExchangeRate : this.EstimateAmount * this.ExchangeRate) - parseFloat(this.PaymentValue == null || isNaN(this.PaymentValue) ? 0 : this.PaymentValue);
                //reserve += (policyCurrency > 0 ? this.EstimateAmount / this.ExchangeRate - this.PaymentValue / this.ExchangeRate : this.EstimateAmount * this.ExchangeRate - this.PaymentValue * this.ExchangeRate);               
                payments += parseFloat(this.PaymentValue == null || isNaN(this.PaymentValue) ? 0 : this.PaymentValue);
                reserve = estimation - payments;
            }
        });

        $("#labelEstimation").text(FormatMoney(estimation.toFixed(2)));
        $("#labelPayments").text(FormatMoney(payments));
        $("#labelReservations").text(FormatMoney(reserve.toFixed(2)));
    }

    static DoSetClaimReserve() {
        if (modelSearchCriteria.claimNumber == null || modelSearchCriteria.policyDocumentNumber == null || modelSearchCriteria.branchId == null || modelSearchCriteria.prefixId == null)
            return;

        SetClaimReserve.GetPrefixes(function (prefixes) {
            $('#selectPrefix').UifSelect({ sourceData: prefixes });
            $('#selectPrefix').UifSelect('setSelected', modelSearchCriteria.prefixId);

            SetClaimReserve.GetBranches(function (branches) {
                $('#selectBranch').UifSelect({ sourceData: branches });
                $('#selectBranch').UifSelect('setSelected', modelSearchCriteria.branchId);

                $('#inputPolicyDocumentNumber').val(modelSearchCriteria.policyDocumentNumber);

                $('#InputClaimNumberSearch').val(modelSearchCriteria.claimNumber);

                SetClaimReserve.SearchClaim();
            });
        });
    }

    /////////////////////////////////
    // Acciones Adicionales
    static CleanForm() {
        policyConsulted = false;
        $('#InputPolicyHolder').val('');
        $('#InputPolicyCurrency').val('');
        $('#InputPolicyEndorsement').val('');
        $('#InputPolicyDocumentNumber').val('');
        $('#tblEstimation').UifDataTable('clear');
        SetClaimReserve.CleanTableTitles();
        $('#tblLimits').UifDataTable('clear');
        $('#tblDeductibles').UifDataTable('clear');

        $('#selectBranch').UifSelect('setSelected', null);
        $('#selectPrefix').UifSelect('setSelected', null);
        $('#InputClaimNumberSearch').val('');
        SetClaimReserve.CleanReserve();
        $('#inputInformClaimReserve').val('');
        $('#divLabelInformationPolicy').css('display', 'none');

        $('#selectPrefix').UifSelect("disabled", false);
        $('#selectBranch').UifSelect("disabled", false);
        $('#inputPolicyDocumentNumber').prop('disabled', false);
        $('#InputClaimNumberSearch').prop('disabled', false);

        $("#labelEstimation").text(0);
        $("#labelPayments").text(0);
        $("#labelIncome").text(0);
        $("#labelReservations").text(0);

        $('#editForm').hide();
    }

    static CleanReserve() {
        $('#selectStateCode').UifSelect('setSelected', null);
        $('#selectReason').UifSelect('setSelected', null);
        $('#inputReserve').val('');
        $('#inputSalaryNumeric').val('');
        $('#selectBranch').UifSelect('setSelected', null);
        $('#selectPrefix').UifSelect('setSelected', null);
        $('#InputClaimNumberSearch').val('');
        $('#inputPolicyDocumentNumber').val('');
    }

    static CleanTableTitles() {
        $(".panel-table h3.panel-title").each(function (index, element) {
            if (index > 0) {
                var indexSpace = $(element).text().indexOf(" ");

                if (indexSpace >= 0) {
                    $(element).text($(element).text().substring(0, indexSpace));
                }
            }
        });
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if ($(this).val() != '')
            $(this).val(FormatMoney($(this).val().includes(',') ? $(this).val().replace(',', '.') : $(this).val()));
    }
}