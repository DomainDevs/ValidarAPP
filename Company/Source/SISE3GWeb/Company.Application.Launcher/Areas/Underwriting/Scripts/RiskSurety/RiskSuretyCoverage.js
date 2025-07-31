//VARIABLES
var coverageData = { Id: 0 };
var coverageIndex = null;
var validCoverage = true;
var coverageText = null;
var coverageClauses = null;
var IsEdit = false;
class RiskSuretyCoverage extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        RiskSuretyCoverage.initialize();
        $("#DecimalPlaceRequired").val(UnderwritingDecimal);
    }

    bindEvents() {
        $('#listCoveragesEdit').on('rowAdd', this.CoverageAdd);
        $('#listCoveragesEdit').on('rowEdit', this.CoverageEdit);
        $('#listCoveragesEdit').on('rowDelete', function (event, data) {
            RiskSuretyCoverage.DeleteCoverage(data);
        });
        $("#btnCloseCoverage").on("click", this.CloseCoverage);

        $("#btnAcceptCoverage").on("click", RiskSuretyCoverage.AddCoverage);
        $("#btnCalculateCoverage").on("click", RiskSuretyCoverage.CalculatCoverage);
        $("#btnCancelCoverage").on("click", this.CancelCoverage);
        //guardar cobertura
        $("#btnSaveCoverage").on("click", RiskSuretyCoverage.SaveCoverages);
        $("#inputAmountCoverage").focusin(this.NotFormatMoneyIn);
        $("#inputAmountCoverage").focusout(this.AmountCoverageFocusOut);
        $("#inputSubLimitAmount").focusin(this.NotFormatMoneyIn);
        $("#inputSubLimitAmount").focusout(this.SubLimitAmountFocusOut);
        $("#inputDaysCoverage").focusout(this.DaysCoverage);
        $('#selectCoverage').on('itemSelected', this.ChangeCoverage);
        $("#inputRateCoverage").focusin(this.RateCoverageFocusIn);
        $("#inputRateCoverage").focusout(this.RateCoverageFocusOut);

        $('#selectCalculeType').on('itemSelected', this.ChangeCalculeType);
        $('#selectRateType').on('itemSelected', this.ChangeCalculeTypeRateType);
        $("#inputContractAmountPercentage").focusout(this.ContractAmountPercentage);
        $("input[type='text']").on("click", function () {
            $(this).select();
        });

        //validar fechas
        $("#inputFromCoverage").on("blur", function () {
            RiskSuretyCoverage.DateFromCoverage();
        });

        $("#inputFromCoverage").on("datepicker.change", (event, date) => { RiskSuretyCoverage.DateFromCoverage(event, date) });
        $("#inputToCoverage").on("datepicker.change", (event, date) => { RiskSuretyCoverage.DateToCoverage(event, date) });

        //Pasa el valor de la tasa a la prima si es tipo de calculo directo y tasa importe
        $("#inputRateCoverage").focusout(this.GetRatePremium);
    }

    CoverageAdd(event) {
        lockScreen();
        RiskSuretyCoverage.ClearCoverage();
        RiskSuretyCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        DisableModificationDate(false);
    }

    CoverageEdit(event, data, index) {
        lockScreen();
        coverageData = data;
        coverageIndex = index;
        RiskSuretyCoverage.EditCoverage(index);
    }

    CloseCoverage() {
        lockScreen();
        router.run("prtRiskSurety");
    }

    CancelCoverage() {
        lockScreen();
        RiskSuretyCoverage.ClearCoverage();
        RiskSuretyCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
    }


    NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    AmountCoverageFocusOut(event) {

        event.stopPropagation();
        RiskSuretyCoverage.ValidatePorcentage();
    }

    SubLimitAmountFocusOut(event) {

        var sublimitedAmount = 0;
        event.stopPropagation();
        if ($.trim($("#inputSubLimitAmount").val()) != "") {
            var inputAmountCoverage = NotFormatMoney($("#inputAmountCoverage").val());
            $("#inputSubLimitAmount").val(FormatMoney($("#inputSubLimitAmount").val()));
            sublimitedAmount = NotFormatMoney($("#inputSubLimitAmount").val());
            if (sublimitedAmount > inputAmountCoverage) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSubLimitAmount, 'autoclose': true });
                $("#inputSubLimitAmount").val($("#inputAmountCoverage").val());
            }
        }
    }

    DaysCoverage(event) {
        if ($.trim($("#inputDaysCoverage").val() != "")) {
            var daysAdd = parseInt($("#inputDaysCoverage").val(), 10);
            var currentFrom = $("#inputFromCoverage").val().split('/');
            var currentTo = new Date(currentFrom[2], currentFrom[1] - 1, currentFrom[0]);
            currentTo.setDate(currentTo.getDate() + daysAdd);
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && enumModificationType.Modification == glbPolicy.Endorsement.ModificationTypeId) {
                if (CompareDates(RiskSuretyCoverage.ConvertDate(currentTo), $("#inputToCoverage").val()) == 1) {
                    $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalPolicyDate, 'autoclose': true });
                }
                else {
                    $("#inputToCoverage").UifDatepicker('setValue', RiskSuretyCoverage.ConvertDate(currentTo));
                }
            }
            else {
                $("#inputToCoverage").UifDatepicker('setValue', RiskSuretyCoverage.ConvertDate(currentTo));
            }

        }
    }

    ChangeCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskSuretyCoverage.GetCoverageByCoverageId(selectedItem.Id);
        }
        else {
            $('#selectDeductible').UifSelect();
        }
    }

    RateCoverageFocusIn(event) {
        if ($.trim($(this).val()) != "") {
            $(this).select();
            $(this).val(NotFormatMoney($(this).val()));
        }
    }

    RateCoverageFocusOut(event) {

        event.stopPropagation();
        if ($.trim($(this).val()) != "") {
            $(this).val(FormatMoney($(this).val()));

        }
    }

    ChangeCalculeType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskSuretyCoverage.GetPremium();
        }
    }

    ChangeCalculeTypeRateType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskSuretyCoverage.ValidateRateType();
        }
        else {
            $('#inputPremiumCoverage').val(0);
        }
    }

    ContractAmountPercentage(event) {

        event.stopPropagation();
        RiskSuretyCoverage.ValidateAmount();
    }

    //Pasa el valor de la tasa a la prima si es tipo de calculo directo y tasa importe
    GetRatePremium() {
        if ($("#selectCalculeType").UifSelect("getSelected") == CalculationType.Direct && $("#selectRateType").UifSelect("getSelected") == RateType.FixedValue) {
            $("#inputPremiumCoverage").val($("#inputRateCoverage").val());
        }
    }
    static DateFromCoverage(event, date) {
        if (CompareDates(glbPolicy.CurrentFrom, $("#inputFromCoverage").val()) == 1) {
            $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInitialPolicyDate, 'autoclose': true });
        }
        else if (CompareDates($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 1) {
            $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInitialDate, 'autoclose': true });
        }
        else if (CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 0) {
            $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInitialDate, 'autoclose': true });
        }
        else {
            $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));
        }

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && enumModificationType.Prorogation == glbPolicy.Endorsement.ModificationTypeId) {
            if (CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()) > 1) {
                $("#inputFromCoverage").UifDatepicker('setValue', $("#inputToCoverage").val());
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateProrogation, 'autoclose': true });
            }
        }

    }

    static DateToCoverage(event, date) {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && enumModificationType.Modification == glbPolicy.Endorsement.ModificationTypeId) {
            if (CompareDates($("#inputToCoverage").val(), glbCoverage.CurrentTo) == 1) {
                $("#inputToCoverage").UifDatepicker('setValue', coverageData.CurrentTo);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateModification, 'autoclose': true });
            }

        }
        if (CompareDates($("#inputToCoverage").val(), glbPolicy.CurrentTo) == 1) {
            $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalPolicyDate, 'autoclose': true });
        }
        else if (CompareDates($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 1) {
            $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
        }
        else if (CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 0) {
            $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
        }
        else {
            $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));
        }
    }

    static CalculatCoverage() {
        RiskSuretyCoverage.GetPremium();
    }
    static ValidateRateType() {
        var RateCoverage = parseInt($("#selectRateType").UifSelect("getSelected"), 10);
        switch (RateCoverage) {
            case RateType.Percentage:
                $("#inputRateCoverage").attr("maxLength", 9);
                break;
            case RateType.Permilage:
                $("#inputRateCoverage").attr("maxLength", 9);
                break;
            default:
                //FixedValue
                $("#inputRateCoverage").attr("maxLength", 20);
                break;
        }
    }

    //Metodos
    static GetCoveragesByProductIdGroupCoverageIdPrefixId() {
        var coveragesAdd = '';
        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        });

        RiskSuretyRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description" });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetDeductiblesByCoverageId(coverageId, selectedId) {
        CoverageRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectDeductible").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectDeductible").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SaveCoverages() {
        var Disponible = 0;
        if (EconomicGroupEvent != undefined) {
            //Grupo Economico
            Disponible = EconomicGroupEvent.OperationCuotaAvalible;
        } else if (ConsortiumEvent != undefined && IsConsortium) {
            //Consorcio
            Disponible = ConsortiumEvent.OperationCuotaAvalible;
        } else if (OperationQuotaEvent != undefined && ConsortiumEvent != undefined) {
            //Participante Consorcio
            Disponible = ConsortiumEvent.OperationCuotaAvalible;
        } else if (OperationQuotaEvent != null && ConsortiumEvent == undefined) {
            // Individual
            Disponible = OperationQuotaEvent.OperationCuotaAvalible;
        }
        if (Disponible >= parseInt(NotFormatMoney($('#inputAmountInsured').text())) && glbPolicy.TemporalType == TemporalType.Policy || glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.Endorsement || glbPolicy.TemporalType == TemporalType.TempQuotation) {
            lockScreen();
            var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
            $.each(coveragesValues, function (key, value) {
                this.CurrentFrom = FormatDate(this.CurrentFrom);
                this.CurrentTo = FormatDate(this.CurrentTo);
                this.LimitAmount = NotFormatMoney(this.LimitAmount);
                this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
                this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
                this.EndorsementLimitAmount = NotFormatMoney(this.EndorsementLimitAmount);
                this.EndorsementSublimitAmount = NotFormatMoney(this.EndorsementSublimitAmount);
                this.OriginalSubLimitAmount = NotFormatMoney(this.OriginalSubLimitAmount);
                this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
                this.ContractAmountPercentage = NotFormatMoney(this.ContractAmountPercentage);
                this.Rate = NotFormatMoney(this.Rate);
                this.CalculatePorcentage = false;
            });
            RiskSuretyCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, $("#listCoveragesEdit").UifListView('getData')).done(function (data) {
                if (data.success) {
                    if (glbPolicy.Endorsement.EndorsementType == 2) {
                        glbRisk.Coverages = coveragesValues;
                        RiskSurety.UpdatePolicyComponents(false);
                    }
                    previousValue = glbRisk.Value.Value;
                    $("#selectSuretyCountry").UifSelect({ sourceData: gdlCountries });
                    router.run("prtRiskSurety");
                }
                else {

                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {

                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
            });
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
        }
    }

    ReturnRisk() {
        RiskSuretyCoverageRequest.ReturnRisk(glbPolicy.Id).done(function (data) {
            if (data.success) {
                var policyModelsView = data.result;
                policyModelsView.riskId = glbRisk.Id;

                //lanza los eventos para la creación del riesgo
                var events1 = GetGeneratedEvents({ ObjectName: Location.RiskSurety, IdTemp: glbPolicy.Id, key1: glbRisk.Id });
                if (events1 == EventType.Nothing) {
                    $.redirect(rootPath + 'Underwriting/RiskSurety/Surety', policyModelsView);
                }
                if (events1 == EventType.Notification) {
                    setTimeout(function () {
                        $.redirect(rootPath + 'Underwriting/RiskSurety/Surety', policyModelsView)
                    }, 1500);
                }
                //fin - lanza los eventos para la creación del riesgo
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
        });
    }

    static GetCoveragesByRiskId(riskId) {
        var coverages = null;

        RiskSuretyCoverageRequest.GetCoveragesByRiskId(riskId).done(function (data) {
            if (data.success) {
                coverages = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });


        if (coverages != null) {
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 440 });

            $.each(coverages, function (key, value) {
                this.LimitAmount = FormatMoney(this.LimitAmount);
                this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
                this.Rate = FormatMoney(this.Rate);
                this.PremiumAmount = FormatMoney(this.PremiumAmount);
                this.FlatRatePorcentage = FormatMoney(this.FlatRatePorcentage);
                this.ContractAmountPercentage = FormatMoney(this.ContractAmountPercentage);
            });
            $("#listCoveragesEdit").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
        }
    }

    static CalculateDays(f1, f2) {

        var cFecha1 = f1.split('/');
        var cFecha2 = f2.split('/');
        var fcFecha1 = Date.UTC(cFecha1[2], cFecha1[1] - 1, cFecha1[0]);
        var fcFecha2 = Date.UTC(cFecha2[2], cFecha2[1] - 1, cFecha2[0]);
        var dif = fcFecha2 - fcFecha1;
        var days = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(days)) {
            return 0;
        }
        else {
            return days;
        }
    }

    static initialize() {
        $("#inputFromCoverage").ValidatorKey(ValidatorType.Dates, 1, 1);
        $("#inputToCoverage").ValidatorKey(ValidatorType.Dates, 1, 1);
        $("#inputAmountCoverage").OnlyDecimals(UnderwritingDecimal);
        $("#inputSubLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputPremiumCoverage").OnlyDecimals(UnderwritingDecimal);
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputRateCoverage").OnlyDecimals(3);
        } else {
            $("#inputRateCoverage").OnlyDecimals1(3);
        }
        //purpose: Se modifica con el fin de permitir reemplazar texto sobre un select
        //begin
        $("#inputContractAmountPercentage").OnlyDecimals(UnderwritingDecimal);
        //End
        $("#inputPremium").val(FormatMoney($("#inputPremium").val()));
        if ($("#listCoveragesEdit").UifListView('getData').length <= 0) {
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 440 });
        }

        if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Emission) {
            glbPolicy.currentFrom = glbPolicy.Endorsement.CurrentFrom;
            glbPolicy.currentTo = glbPolicy.Endorsement.CurrentTo;
            //$("#inputToCoverage").prop('disabled', true);
        }

        RiskSuretyCoverage.LoadCoverages(glbRisk.Coverages);
        if (glbCoverage.CoverageId > 0) {
            RiskSuretyCoverage.LoadCoverage();
        }
        else {
            $("#inputToCoverage").val(FormatDate(glbPolicy.CurrentTo));
            RiskSuretyCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
            RiskSuretyCoverage.GetCalculeTypes(0);
            RiskSuretyCoverage.GetRateTypes(0);
        }

        RiskSuretyCoverage.ValidateRateType();
        RiskSuretyCoverage.LoadTitle();
        $('#inputDaysCoverage').val(CalculateDays($('#inputFromCoverage').val(), $('#inputToCoverage').val()));
        //--------------------------*****************************************-----------------
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        RiskSuretyCoverage.ShowPanelsCoverage(MenuType.Coverage);
        $("#inputAmountInsured").text(FormatMoney(glbRisk.AmountInsured));
        $("#inputPremium").text(FormatMoney(glbRisk.Premium));
    }


    static AddCoverage() {

        if (!RiskSuretyCoverage.ValidateCoverage()) {
            validCoverage = false;
            return false;
        }
        if (!RiskSuretyCoverage.ValidCoverageValue()) {
            return false;
        }
        $("#mainCoverage").validate();

        if (true/*$("#mainCoverage").valid()*/) {
            RiskSuretyCoverage.DisableModificationDate(false);
            RiskSuretyCoverage.QuotationCoverage(true, true);
        }
        else {
            validCoverage = false;
            return false;
        }
    }

    static EditCoverage(index) {
        RiskSuretyCoverage.DisableModificationDate(true);
        if (coverageData.IsVisible) {
            lockScreen();
            coverageText = coverageData.Text;
            coverageClauses = coverageData.Clauses;
            //coverageData.OriginalSubLimitAmount = coverageData.SubLimitAmount;
            if (coverageData.LimitAmount == null) {
                coverageData.LimitAmount = 0;
                coverageData.DeclaredAmount = 0;
            }

            if (coverageData.PremiumAmount == null) {
                coverageData.PremiumAmount = 0;
            }

            if (coverageData.Rate == null) {
                coverageData.Rate = 0;
            }

            if (index != null) {
                RiskSuretyCoverage.SetCoverageEdit();
                $("#inputAmountCoverage").val(coverageData.LimitAmount);
                $("#inputSubLimitAmount").val(coverageData.SubLimitAmount);
                $("#inputContractAmountPercentage").val(coverageData.ContractAmountPercentage);
                $("#inputRateCoverage").val(coverageData.Rate);
                $("#inputPremiumCoverage").val(coverageData.PremiumAmount);
            }
            else {
                $("#inputAmountCoverage").val(FormatMoney(coverageData.LimitAmount));
                $("#inputSubLimitAmount").val(FormatMoney(coverageData.SubLimitAmount));
                $("#inputRateCoverage").val(FormatMoney(coverageData.Rate));
                $("#inputPremiumCoverage").val(FormatMoney(coverageData.PremiumAmount));
            }

            if (coverageData.IsPostcontractual) {
                var CantDaysOriginal = CalculateDays(coverageData.CurrentFrom, coverageData.CurrentTo);
                $("#inputToCoverage").UifDatepicker('setValue', FormatDate(coverageData.CurrentTo));
                if (glbRisk.RiskSuretyPost.ChkContractDate) {
                    $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(glbRisk.RiskSuretyPost.ContractDate));
                }
                else {
                    $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(glbRisk.RiskSuretyPost.IssueDate));
                }
                if (CantDaysOriginal > 0) {
                    var currentFromCalc = $("#inputFromCoverage").val().split('/');
                    var currentToCalc = new Date(currentFromCalc[2], currentFromCalc[1] - 1, currentFromCalc[0]);
                    currentToCalc.setDate(currentToCalc.getDate() + CantDaysOriginal);
                    $("#inputToCoverage").UifDatepicker('setValue', RiskSuretyCoverage.ConvertDate(currentToCalc));
                }
                $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));
            }
            else {
                //Modificacion USP-697 $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(coverageData.CurrentFrom));

                if (CompareDates((FormatDate(coverageData.CurrentFrom)), (FormatDate(glbPolicy.CurrentFrom))) == 1) {
                    $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(coverageData.CurrentFrom));
                }
                else {
                    if (CompareDates((FormatDate(coverageData.CurrentFrom)), (FormatDate(glbPolicy.CurrentFrom))) == 1) {
                        $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(coverageData.CurrentFrom));
                    }
                    else {
                        if (coverageData.IsPostcontractual && glbRisk.Policy.Endorsement.EndorsementType != EndorsementType.Emission) {
                            if (glbRisk.RiskSuretyPost.ChkContractDate) {
                                $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(glbRisk.RiskSuretyPost.ContractDate));
                            }
                            else {
                                $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(glbRisk.RiskSuretyPost.IssueDate));
                            }
                        }
                        else {
                            if (CompareDates(FormatDate(glbPolicy.CurrentFrom), coverageData.CurrentTo) == 1) {
                                $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(coverageData.CurrentTo));
                            } else {
                                $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
                            }

                        }

                    }

                }
                //fin cambio USP-697

                var CantDaysOriginal = CalculateDays(coverageData.CurrentFrom, coverageData.CurrentTo);
                if (CantDaysOriginal > 0) {
                    var currentFromCalc = $("#inputFromCoverage").val().split('/');
                    if (glbRisk.Policy.Endorsement.ModificationTypeId != 1) {
                        var currentToCalc = new Date(currentFromCalc[2], currentFromCalc[1] - 1, currentFromCalc[0]);
                        currentToCalc.setDate(currentToCalc.getDate() + CantDaysOriginal);
                        $("#inputToCoverage").UifDatepicker('setValue', RiskSuretyCoverage.ConvertDate(currentToCalc));
                    }
                    else { $("#inputToCoverage").UifDatepicker('setValue', FormatDate(coverageData.CurrentTo));}
                }
                $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));

            }

            if (coverageData.Deductible != null) {
                RiskSuretyCoverage.GetDeductiblesByCoverageId(coverageData.Id, coverageData.Deductible.Id);
            }
            else {
                RiskSuretyCoverage.GetDeductiblesByCoverageId(coverageData.Id, 0);
            }
            if (coverageData.CalculatePorcentage == undefined) {
                var Amount = parseFloat(NotFormatMoney($("#inputAmountCoverage").val()).replace(separatorDecimal, separatorThousands));
                var TotalAmount = parseFloat(glbRisk.Value.Value);

                RiskSuretyCoverage.ValidateAmount();
            }
            RiskSuretyCoverage.GetCalculeTypes(coverageData.CalculationType);
            RiskSuretyCoverage.GetRateTypes(coverageData.RateType);
        }
        else {
            unlockScreen();
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
        }
    }
    static enableText(input) {

        var boolIs;
        var startPos = input[0].selectionStart;
        var endPos = input[0].selectionEnd;
        var doc = document.selection;

        if (doc && doc.createRange().text.length != 0) {
            boolIs = true;
        } else if (!doc && input.value.substring(startPos, endPos).length != 0) {
            boolIs = true;
        }
        boolIs = false;
    }

    static SetCoverageEdit() {
        var coveragesData = [];

        coveragesData.push(coverageData);

        $("#selectCoverage").UifSelect();
        $("#selectCoverage").UifSelect({ sourceData: coveragesData, id: "Id", name: "Description", selectedId: coverageData.Id });
        $("#selectCoverage").prop('disabled', true);
    }

    //limpiamos formularios de coberturas --tipo tasa ty tipo caculo se asigna valor por defecto
    static ClearCoverage() {
        $('#selectCoverage').UifSelect();
        $('#selectCalculeType').data('Value', CalculationType.Prorrate);
        $("#selectCalculeType").UifSelect("setSelected", CalculationType.Prorrate);
        $('#inputAmountCoverage').val('0');
        $('#inputSubLimitAmount').val('0');
        $('#selectRateType').data('Value', RateType.Percentage);
        $("#selectRateType").UifSelect("setSelected", RateType.Percentage);
        $('#inputRateCoverage').val("");
        $('#inputPremiumCoverage').val("");
        $('#selectDeductible').data('Id', null);
        $('#selectDeductible').val("");

        $('#inputContractAmountPercentage').val('0');

        $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
        $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);

        $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val().toString(), $("#inputToCoverage").val().toString()));
        coverageText = null;
        coverageClauses = null;
        coverageIndex = null;
    }

    static DeleteCoverage(data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        }
        else {

            var coverages = $("#listCoveragesEdit").UifListView('getData');

            if (coverages != null && coverages != "" && coverages.length == 1 && data.EndorsementType == EndorsementType.Modification) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
            }
            else {

                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });

                if (data.EndorsementType == EndorsementType.Modification) {
                    if (data.RiskCoverageId > 0) {
                        var coverage = RiskSurety.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description)
                        $.when(coverage).done(function (coverageData) {
                            coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount);
                            coverageData.Rate = FormatMoney(coverageData.Rate);
                            coverageData.CurrentTo = FormatDate(coverageData.CurrentTo);
                            coverageData.CurrentFrom = FormatDate(coverageData.CurrentFrom);
                            coverageData.LimitAmount = FormatMoney(coverageData.LimitAmount);
                            $.each(coverages, function (index, value) {
                                if (this.Id == coverageData.Id) {
                                    coverages[index] = coverageData
                                }
                            });
                            $("#listCoveragesEdit").UifListView({ sourceData: coverages, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: heightListView, title: AppResources.LabelTitleCoverages });
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });

                            RiskSuretyCoverage.ClearCoverage();
                            RiskSuretyCoverage.PremiumTotal();
                            RiskSuretyCoverage.CalculateInsuredSum();
                        });
                    }
                    else {
                        $.each(coverages, function (index, value) {
                            if (this.Id != data.Id) {
                                $("#listCoveragesEdit").UifListView("addItem", this);
                            }
                        });
                        RiskSuretyCoverage.ClearCoverage();
                        RiskSuretyCoverage.PremiumTotal();
                        RiskSuretyCoverage.CalculateInsuredSum();
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                    }

                }
                else {
                    $.each(coverages, function (index, value) {
                        if (this.Id != data.Id) {
                            $("#listCoveragesEdit").UifListView("addItem", this);
                        }
                    });
                    if ($("#listCoveragesEdit").UifListView('getData').length > 0) {
                        RiskSuretyCoverage.GetListCoverageTemporal("listCoveragesEdit")
                            .done(function (data) {
                                if (data.success) {
                                    var coverageList = RiskSurety.formatCoverage(data.result);
                                    $("#listCoveragesEdit").UifListView({ sourceData: coverageList, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });
                                    RiskSuretyCoverage.ClearCoverage();
                                    RiskSuretyCoverage.PremiumTotal();
                                    RiskSuretyCoverage.CalculateInsuredSum();
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                }
                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
                            });
                    }
                    else {
                        RiskSuretyCoverage.ClearCoverage();
                        RiskSuretyCoverage.PremiumTotal();
                        RiskSuretyCoverage.CalculateInsuredSum();
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
                    }
                }
            }
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskSuretyCoverageRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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


    static GetPremium() {
        if (RiskSuretyCoverage.ValidCoverageTariff()) {
            RiskSuretyCoverage.QuotationCoverage(false, false);
        }
        else {
            $("#inputPremiumCoverage").val(0);
            return false;
        }
    }

    static QuotationCoverage(runRulesPost, reload) {

        var coverageModel = RiskSuretyCoverage.CreateCoverageModel();
        var listCoverage = RiskSuretyCoverage.NotFormatCoverage();
        coverageData.Clauses = coverageModel.Clauses;
        if (glbPolicy.Endorsement.EndorsementType == 2) {
            coverageData.ModificationTypeId = glbPolicyEndorsement.Endorsement.ModificationTypeId;
        }
        RiskSuretyCoverageRequest.QuotationCoverage(coverageModel, coverageData, runRulesPost, glbPolicy.Endorsement.PolicyId, listCoverage).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (glbPolicy.Endorsement.EndorsementType == 2 && glbPolicyEndorsement.Endorsement.ModificationTypeId == 4) {
                        glbRisk.Coverages.forEach(function (item, index) {
                            data.result.forEach(function (item1, index) {
                                if (item.Id == item1.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 1) {
                                    item1.PremiumAmount = (parseInt(NotFormatMoney(item.OriginalSubLimitAmount)) * (item.OriginalRate / 100) * CalculateDays(item.CurrentFromOriginal, item.CurrentToOriginal) / 365 - item1.PremiumAmount) * -1;
                                }
                            });
                        });
                    }

                    //viene dl boton Calcular
                    if (!reload) {
                        var coverageValue = JSLINQ(data.result).Where(function (item) { return item.Id == coverageModel.CoverageId; });
                        $("#inputPremiumCoverage").val(FormatMoney(coverageValue.items[0].PremiumAmount));
                    }
                    else {
                        IsEdit = reload;
                        RiskSuretyCoverage.LoadCoverages(data.result);
                        RiskSuretyCoverage.PremiumTotal();
                        RiskSuretyCoverage.CalculateInsuredSum();
                        RiskSuretyCoverage.ClearCoverage();
                        RiskSuretyCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
                    }

                }
                else {
                    $("#inputPremiumCoverage").val(0);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });
    }
    static CreateRiskSuretyCoverage(coverageModel) {
        if (coverageModel != null) {
            coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
            coverageModel.LimitAmount = FormatMoney(coverageModel.LimitAmount);
            coverageModel.DeclaredAmount = FormatMoney(coverageModel.DeclaredAmount);
            coverageModel.SubLimitAmount = FormatMoney(coverageModel.SubLimitAmount);
            coverageModel.Rate = FormatMoney(coverageModel.Rate);
            coverageModel.PremiumAmount = FormatMoney(coverageModel.PremiumAmount);
            coverageModel.FlatRatePorcentage = FormatMoney(coverageModel.FlatRatePorcentage);
            coverageModel.ContractAmountPercentage = FormatMoney(coverageModel.ContractAmountPercentage);
            coverageModel.Text = coverageText;
            coverageModel.Clauses = coverageClauses;
            coverageModel.CalculatePorcentage = false;
            coverageModel.CoverStatusName = coverageModel.CoverStatusName;
            if (coverageIndex == null) {
                $("#listCoveragesEdit").UifListView("addItem", coverageModel);
            }
            else {
                $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverageModel);
            }

            RiskSuretyCoverage.PremiumTotal();
            RiskSuretyCoverage.CalculateInsuredSum();
            RiskSuretyCoverage.ClearCoverage();
            RiskSuretyCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        }
        coverageIndex == null;
    }

    static CreateCoverageModel() {
        var Deductible = new Object();
        var coverageModel = $("#mainCoverage").serializeObject();
        coverageModel.CoverageId = $('#selectCoverage').UifSelect('getSelected');
        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.LimitAmount = NotFormatMoney($('#inputAmountCoverage').val());
        coverageModel.DeclaredAmount = NotFormatMoney($('#inputAmountCoverage').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.Rate = NotFormatMoney($('#inputRateCoverage').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumCoverage').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;
        if (coverageText != null && coverageText.TextBody != null)
            coverageModel.Text = coverageText.TextBody;
        if (clausesCoverage != null) {
            coverageModel.Clauses = clausesCoverage;
            clausesCoverage = null;
        }

        coverageModel.ContractAmountPercentage = NotFormatMoney($('#inputContractAmountPercentage').val());
        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
            Deductible.Id = $("#selectDeductible").UifSelect('getSelected');
            Deductible.Description = $("#selectDeductible").UifSelect('getSelectedText');
            coverageModel.DeductibleId = $("#selectDeductible").UifSelect('getSelected');
            coverageModel.DeductibleDescription = $("#selectDeductible").UifSelect('getSelectedText');
            coverageModel.Deductible = Deductible;
        }

        return coverageModel;
    }

    static ValidateCoverage() {
        var msj = "";
        var Amount = parseFloat(NotFormatMoney(glbRisk.Value.Value).replace(separatorDecimal, separatorThousands));
        var TotalAmount = parseFloat(NotFormatMoney($("#inputAmountCoverage").val()).replace(separatorDecimal, separatorThousands));

        if (TotalAmount > Amount) {
            msj = AppResources.ErrorValidateSumContract;

        }
        if ($("#inputContractAmountPercentage").val() > 100) {
            msj = msj + AppResources.ErrorValidateContractPercentage;
        }

        if ($('#inputDaysCoverage').val() < 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Los dias de vigencia no pueden ser negativos.', 'autoclose': true });
            return false;
        }

        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': msj, 'autoclose': true });
            return false;
        }
        else {
            return true;
        }
    }

    static PremiumTotal() {
        var premium = 0;

        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
        });

        $("#inputPremium").text(FormatMoney(premium));
    }

    static HidePanelCoverage(Menu) {

        switch (Menu) {
            case MenuType.Coverage:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('hide');
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('hide');
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
            default:
                break;
        }
    }

    static ValidCoverageValue() {
        var validCover = true;
        if ($("#selectCoverage").UifSelect("getSelected") == null || $("#selectCoverage").UifSelect("getSelected") == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
            validCover = false;
        }
        if ($("#selectCalculeType").UifSelect("getSelected") == null || $("#selectCalculeType").UifSelect("getSelected") == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCalculeType, 'autoclose': true });
            validCover = false;
        }

        if ($.trim($("#inputSubLimitAmount").val()) == '' || $("#inputSubLimitAmount").val() == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSubLimit, 'autoclose': true });
            validCover = false;
        }
        if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
            if (($.trim($("#inputRateCoverage").val()) == '' || $("#inputRateCoverage").val() == 0)) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRate, 'autoclose': true });
                validCover = false;
            }
        }


        if ($("#selectRateType").UifSelect("getSelected") == null || $("#selectRateType").UifSelect("getSelected") == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRateType, 'autoclose': true });
            validCover = false;
        }
        if (validCover) {
            return true;
        }
        else {
            return false;
        }
    }

    static ValidCoverageTariff() {

        if ($("#selectCoverage").UifSelect("getSelected") == null || $("#selectCoverage").UifSelect("getSelected") == 0) {
            return false;
        }
        if ($("#selectCalculeType").UifSelect("getSelected") == null || $("#selectCalculeType").UifSelect("getSelected") == 0) {
            return false;
        }

        if ($.trim($("#inputSubLimitAmount").val()) == '' || $("#inputSubLimitAmount").val() == 0) {
            return false;
        }

        if ($.trim($("#inputRateCoverage").val()) == '' || $("#inputRateCoverage").val() == 0) {
            return false;
        }
        if ($("#selectRateType").UifSelect("getSelected") == null || $("#selectRateType").UifSelect("getSelected") == 0) {
            return false;
        }
        return true;
    }

    static CalculateInsuredSum() {
        var Amount = 0;
        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            Amount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
        });
        $("#inputAmountInsured").text(FormatMoney(Amount));
    }

    static LoadCoverage() {
        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            if (glbCoverage.CoverageId == this.Id) {
                coverageIndex = key;
                coverageData = this;
            }
        });

        RiskSuretyCoverage.EditCoverage(coverageIndex);
    }

    static GetCoverageByCoverageId(coverageId) {

        RiskSuretyCoverageRequest.GetCoverageByCoverageId(coverageId, glbRisk.Id, glbPolicy.Id, glbRisk.GroupCoverage.Id).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    coverageData = data.result;
                    RiskSuretyCoverage.EditCoverage(null);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
        });
    }

    static LoadTitle() {
        var title = AppResources.LabelTitleCoverage + " - " + AppResources.LabeltitleRisk + ": " + glbRisk.Description;
        $.uif2.helpers.setGlobalTitle(title);
    }

    static ConvertDate(date) {
        return date.getDate() + DateSplit + (date.getMonth() + 1) + DateSplit + date.getFullYear()
    }

    static GetCalculeTypes(selectedId) {
        CoverageRequest.GetCalculeTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectCalculeType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectCalculeType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
                }
            }
        });
    }

    static GetRateTypes(selectedId) {
        CoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectRateType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRateType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectRateType").UifSelect("setSelected", coverageData.RateType);
                }

            }
        });
    }

    static ValidatePorcentage() {
        var Amount = parseFloat(NotFormatMoney($("#inputAmountCoverage").val()).replace(separatorDecimal, separatorThousands));
        //var TotalAmount = parseFloat(NotFormatMoney(glbRisk.Value.Value).replace(separatorDecimal, separatorThousands));
        var TotalAmount = parseFloat(glbRisk.Value.Value);
        if (Amount != 0 && !isNaN(Amount)) {

            if (Amount > TotalAmount) {
                $("#inputAmountCoverage").val(0);
                $("#inputSubLimitAmount").val(0);
                $("#inputMaxLiabilityAmount").val(0);

                $("#inputContractAmountPercentage").val(0);
                $("#inputPremiumCoverage").val(0);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSumContract, 'autoclose': true });
            }
            else {
                if (TotalAmount != 0) {

                    $("#inputContractAmountPercentage").val(((Amount / TotalAmount) * 100).toFixed(2));
                    $("#inputContractAmountPercentage").val(FormatMoney($("#inputContractAmountPercentage").val()));
                    $("#inputSubLimitAmount").val(FormatMoney(Amount));

                }
            }
            if ($.trim($("#inputAmountCoverage").val()) != "") {
                $("#inputAmountCoverage").val(FormatMoney($("#inputAmountCoverage").val()));
            }
        }
    }

    static ValidateAmount() {
        var contractAmountPercentage = parseFloat(NotFormatMoney($("#inputContractAmountPercentage").val()).replace(separatorDecimal, separatorThousands));
        if (!isNaN(contractAmountPercentage)) {
            if (contractAmountPercentage > 100) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateContractPercentage, 'autoclose': true });
                $("#inputAmountCoverage").val(0);
                $("#inputSubLimitAmount").val(0);
                $("#inputContractAmountPercentage").val(0);

                //FormatMoney($("#inputContractAmountPercentage").val().toFixed(2))
                $("#inputPremiumCoverage").val(0);

                return false;
            }
            else {
                if (contractAmountPercentage != 0) {
                    //var AmountCoverage = parseFloat(NotFormatMoney(glbRisk.Value.Value).replace(separatorDecimal, separatorThousands));
                    var AmountCoverage = parseFloat(glbRisk.Value.Value);


                    $("#inputAmountCoverage").val(FormatMoney((AmountCoverage * (contractAmountPercentage / 100)).toFixed(UnderwritingDecimal)));
                    $("#inputSubLimitAmount").val(FormatMoney((AmountCoverage * (contractAmountPercentage / 100)).toFixed(UnderwritingDecimal)));
                    //$("#inputContractAmountPercentage").val(FormatMoney(contractAmountPercentage.toFixed(2)));
                }
            }
        }
    }
    static LoadCoverages(coverages) {
        if (coverages != null) {
            var coverageList = RiskSurety.formatCoverage(coverages);
            $("#listCoveragesEdit").UifListView({ sourceData: coverageList, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });
        }
    }

    static NotFormatCoverage() {
        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
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
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.EndorsementLimitAmount = NotFormatMoney(this.EndorsementLimitAmount);
            this.EndorsementSublimitAmount = NotFormatMoney(this.EndorsementSublimitAmount);
            this.OriginalSubLimitAmount = NotFormatMoney(this.OriginalSubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.ContractAmountPercentage = NotFormatMoney(this.ContractAmountPercentage);
            this.Rate = NotFormatMoney(this.Rate);
            this.CalculatePorcentage = false;
        });
        return coveragesValues;
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

    static GetListCoverageTemporal(nameListCoverages) {
        //var coverageDelete = $.Deferred();
        nameListCoverages = "#" + nameListCoverages;
        var coveragesValues = $(nameListCoverages).UifListView('getData');
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
        return RiskSuretyCoverageRequest.GetListCoverageTemporal(glbPolicy.Id, glbRisk.Id, coveragesValues);

        //return coverageDelete.promise();
    }
    static DisableModificationDate(disabled) {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && enumModificationType.Modification == glbPolicy.Endorsement.ModificationTypeId) {
            $("#inputToCoverage").prop('disabled', disabled);
            $("#inputDaysCoverage").prop('disabled', disabled);
        }
    }
}

