class FinancialPlan extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        FinancialPlan.GetBranches(0);
        FinancialPlan.GetPrefixes(0);
        FinancialPlan.GetPaymentMethod(0);
    }
    //Seccion Eventos
    bindEvents() {
        $('#FinancialPrefixId').on('itemSelected', this.ChangeBranch);
        $('#PolicyFinNumber').on('itemSelected', this.GetPolicyNumber);
        $('#FinancialEndorsement').on('itemSelected', this.ChangeEndorsement);
        $("#FinancialSearch").on("click", this.FinancialSearch);
        $("#FinancialPlanCalculation").on("click", this.QuotePaymentPlan);
        $("#FinancialPlanSave").on("click", this.CreatePaymentPlan);
        $("#FinancialClean").on("click", this.FinancialClean);

    }
    //Funciones
    FinancialClean() {
        FinancialPlan.ClearControls();
    }


    ChangeEndorsement(event, selectedItem) {
        if (selectedItem.Id > 0) {
            this.EndorsementId = selectedItem.Id;
            FinancialPlan.EndorsementId = selectedItem.Id;
            FinancialPlan.GetPayersByEndorsementId(selectedItem.Id);
        }
    }
    ChangeBranch(event, selectedItem) {
        if (selectedItem.Id > 0) {
            FinancialPlan.Policy();
        }
    }

    GetPolicyNumber(event, selectedItem) {
        if (selectedItem.Id > 0) {
            FinancialPlan.GetEndorsementsByDocumentNumber(selectedItem.Id);
        }
    }

    FinancialSearch(event) {
        FinancialPlan.ClearSearch();
        $("#formFinancialPlan").validate();
        if ($("#formFinancialPlan").valid()) {
            lockScreen();
            FinancialPlan.FinancialPolicy();
        }
    }

    static FinancialPolicy() {
        FinancialPlanRequest.GetFinancialPolicyInfo($('#FinancialEndorsement').val()).done(function (data) {
            if (data.success) {
                FinancialPlan.SetSummary(FinancialPlan.FormatModel(data.result.SummaryDTO));
                $("#FinancialIssuanceInfo").fadeIn("slow");
                $("#FinancialIssuanceCurrentPlanTable").UifDataTable({ sourceData: FinancialPlan.FormatFinancialPlan(data.result.FinancialPlanDTO) });
                FinancialPlan.LoadPlanDto();
            }
        });
    }
    static ModelPolicy(PolicyNumber) {
        var policy = { DocumentNumber: PolicyNumber, PrefixId: $('#FinancialPrefixId').val(), BranchId: $('#FinancialBranchId').val() }
        return policy;
    }

    static GetBranches(selectedId) {
        FinancialPlanRequest.GetBranches().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#FinancialBranchId").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#FinancialBranchId").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    static GetPrefixes(selectedId) {
        FinancialPlanRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#FinancialPrefixId").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#FinancialPrefixId").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    static GetPaymentMethod(selectedId) {
        FinancialPlanRequest.GetPaymentMethod().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#FinancialNewPaymentMethod").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#FinancialNewPaymentMethod").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }


    static GetEndorsementsByDocumentNumber(documentNumber) {
        FinancialPlanRequest.GetEndorsementsByPolicyFilter(FinancialPlan.ModelPolicy(documentNumber)).done(function (data) {
            if (data.success) {
                $("#FinancialEndorsement").UifSelect({ sourceData: data.result });
            }
        });
    }

    static GetPayersByEndorsementId(endorsementId) {
        FinancialPlanRequest.GetPayersByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                $("#FinancialPayer").UifSelect({ sourceData: data.result });
            }
        });
    }

    static Policy() {

        $("#PolicyFinNumber").UifAutoComplete({
            source: rootPath + "Accounting/FinancialPlan/GetPoliciesByQuery/",
            displayKey: "Id",
            queryParameter: "prefixId=" + $('#FinancialPrefixId').val() + "&branchId=" + $('#FinancialBranchId').val() + "&query"

        });
    }
    static GetFinancialPlanModel() {
        var FinancialPlanModel = $("#formFinancialPlan").serializeObject();
    }
    static SetSummary(data) {
        $("#FinancialContratorName").val(data.Holder.Description);
        $("#FinancialCurrencyName").val(data.BaseCurrencyDTO.Description);
        $("#FinancialIssuanceDate").val(data.IssuanceDate);
        $("#FinancialValidFromDate").val(data.FromDate);
        $("#FinancialValidToDate").val(data.ToDate);
        $("#FinancialConduitName").val(data.ConduitName);
        $("#FinancialMainAgentName").val(data.MainAgent.Description);
        $("#FinancialCurrentPlan").val(data.CurrentPlan);
    }
    static LoadPlanDto() {
        FinancialPlanRequest.GetPaymentsScheduleByProductId(1).done(function (data) {
            if (data.success) {
                $("#FinancialNewPaymentPlan").UifSelect({ sourceData: data.result });
            }
        });

    }

    QuotePaymentPlan() {
        lockScreen();
        FinancialPlanRequest.QuotePaymentPlan(FinancialPlan.CreateFilterModel()).done(function (data) {
            if (data.success) {               
                $("#FinancialNewPaymentPlanTable").UifDataTable({ sourceData: FinancialPlan.FormatFinancialPlan(data.result.Quotas) });
                FinancialPlan.CreateTotal(data.result);

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    CreatePaymentPlan() {
        lockScreen();
        FinancialPlanRequest.CreatePaymentPlan(FinancialPlan.CreateFilterModel()).done(function (data) {
            if (data.success) {
                FinancialPlan.ClearSearch();
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.SuccessSaveQuota, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCreatePaymentPlan, 'autoclose': true });
        });
    }

    static FormatModel(model) {
        model.IssuanceDate = FormatDate(model.IssuanceDate);
        model.FromDate = FormatDate(model.FromDate);
        model.ToDate = FormatDate(model.ToDate);
        return model;
    }

    static CreateFilterModel() {
        let filterFinancialPlanDTO = {
            EndorsementId: FinancialPlan.EndorsementId,
            PaymentPlanId: $("#FinancialNewPaymentPlan").val(),
            PaymentMethodId: $("#FinancialNewPaymentMethod").val(),
            AccountDate: $("#FinancialAccountDate").val(),
            PayerId: $("#FinancialPayer").val(),
            ReasonforChange: $("#FinancialChangeMotive").val(),
            IsQuota: true
        }
        return filterFinancialPlanDTO;

    }
    static CreateTotal(model) {
        $("#FinancialPremiumTotal").val(FormatMoney(model.Premium));
        $("#FinancialTaxTotal").val(FormatMoney(model.Tax));
        $("#FinancialExpensesTotal").val(FormatMoney(model.Expenses));
        $("#FinancialOtherChargesDiscountsTotal").val(0);
        $("#FinancialTotal").val(FormatMoney(model.Total));

    }

    static FormatFinancialPlan(financialValues) {
        for (let i = 0; i < financialValues.length; i++) {
            financialValues[i].ExpirationDate = FormatDate(financialValues[i].ExpirationDate);
            financialValues[i].Amount = FormatMoney(financialValues[i].Amount);
            financialValues[i].AmountPending = FormatMoney(financialValues[i].AmountPending);
            financialValues[i].Premium = FormatMoney(financialValues[i].Premium);
            financialValues[i].Tax = FormatMoney(financialValues[i].Tax);
            financialValues[i].Expenses = FormatMoney(financialValues[i].Expenses);
        }
        return financialValues;
    }
    static ClearControls() {
        $("#FinancialBranchId").val("");
        $("#FinancialPrefixId").val("");
        $("#PolicyFinNumber").val("");
        $("#FinancialEndorsement").val("");
        $("#FinancialPayer").val("");
        $("#FinancialChangeMotive").val("");
        $("#FinancialIssuanceCurrentPlanTable").UifDataTable('clear');
        $("#FinancialNewPaymentPlanTable").UifDataTable('clear');
    }
    static ClearSearch() {
        $("#SearchReversion :text :select").each(function () {
            $($(this)).val('');
        });
        $("#SummaryNewFinancialInfo :text").each(function () {
            $($(this)).val('');
        });
        $("#FinancialIssuanceCurrentPlanTable").UifDataTable('clear');
        $("#FinancialNewPaymentPlanTable").UifDataTable('clear');
    }
}

$(() => {
    new FinancialPlan();
});