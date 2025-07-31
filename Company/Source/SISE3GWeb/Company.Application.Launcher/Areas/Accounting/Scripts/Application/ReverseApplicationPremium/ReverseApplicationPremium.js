class ReverseApplicationPremium extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        ReverseApplicationPremium.GetBranches(0);
        ReverseApplicationPremium.GetPrefixes(0);
    }
    bindEvents() {
        $('#PrefixRevSearch').on('itemSelected', this.ChangePrefix);
        $('#PolicyRevNumber').on('itemSelected', this.GetPolicyNumber);     
        $('#searchRevEndorsementNumber').on('itemSelected', this.ChangeEndorsement);
        $("#SearchPoliciesReversionButton").on("click", this.SearchPoliciesReversionButton);
        $("#AddPremiumReversionButton").on("click", this.SaveReversionApplicationPremium);
    }

    ChangePrefix(event, selectedItem) {

    }
    GetPolicyNumber(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ReverseApplicationPremium.GetEndorsementsByDocumentNumber(selectedItem.Id);
        }
    }
    SearchPoliciesReversionButton(event) {
        ReverseApplicationPremium.ClearControls();
        lockScreen();
        ReverseApplicationPremium.SearchPremium();
    }
    static SearchPremium() {
        ReverseApplicationPremiumRequest.GetApplicationPremium($('#searchRevEndorsementNumber').val()).done(function (data) {
            if (data.success) {
                $("#ApplicationPremiumTable").UifDataTable({ sourceData: ReverseApplicationPremium.FormatReverseApplicationPremium(data.result) });

            }
        });
    }

    SaveReversionApplicationPremium() {
        var rowSelected = $("#ApplicationPremiumTable").UifDataTable("getSelected");
        if (rowSelected != null && rowSelected != undefined && Array.isArray(rowSelected) && rowSelected.length > 0) {
            store.dispatch({ type: 'ADD_PREMIUM', reversionPremiumItem: ReverseApplicationPremium.CreatePremiumReversion(rowSelected[0]) });
            ReversionPremium.Createpremium(store.getState().reversionPremium, store.dispatch).then(function (data) {
                if (store.getState().reversionPremium.Aplicate) {
                    ReverseApplicationPremiumRequest.SaveTempPremiumReversion({ Id: store.getState().accountingRedux.TempApplicationId, AppId: rowSelected[0].Id }).done(function (data) {
                        if (data.success) {
                            DialogSearchPolicies.LoadPremiums(store.getState().dialogSearchPoliciesRedux, store.dispatch);
                            ReverseApplicationPremium.ClearSearch();
                        }
                        else {
                            $("#alertReversionView").UifAlert('show', Resources.ApplicationReverseFailureLabel + ": " + rowSelected[0].Id, "warning");
                        }
                    });
                }
            });


        }
        else {
            $("#alertReversionView").UifAlert('show', Resources.SelectedAplication, "warning");
        }
    }

    static CreatePremiumReversion(rowSelected) {
        var result =
        {
            Id: rowSelected.Id,
            EndorsementId: $('#searchRevEndorsementNumber').val(),
            PayerId: rowSelected.PayerId,
            PaymentNumber: rowSelected.Number,
            PaymentAmount: rowSelected.Amount,
            PaymentLocalAmount: null,
            IncomeAmount: null,
            LocalAmount: null,
            BranchPrefixPolicyEndorsement: $("#BranchRevSearch").val() + " " + $("#PrefixRevSearch").val() + $("#PolicyNumber").val() + " " + $('#searchRevEndorsementNumber').val()
        }
        return result;
    }
    static GetBranches(selectedId) {
        ReverseApplicationPremiumRequest.GetBranches().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#BranchRevSearch").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#BranchRevSearch").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    static GetPrefixes(selectedId) {
        ReverseApplicationPremiumRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#PrefixRevSearch").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#PrefixRevSearch").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    static GetEndorsementsByDocumentNumber(documentNumber) {
        ReverseApplicationPremiumRequest.GetEndorsementsByPolicyFilter(ReverseApplicationPremium.ModelPolicy(documentNumber)).done(function (data) {
            if (data.success) {
                $("#searchRevEndorsementNumber").UifSelect({ sourceData: data.result });
            }
        });
    }
    static ClearControls() {
        $("#ApplicationPremiumTable").UifDataTable('clear');
    }
    static ClearSearch() {
        $("#SearchReversion :text,#SearchReversion .uif-select").each(function () {
            $($(this)).val('');
        });
        $("#PolicyRevNumber").val("");
        $("#searchRevEndorsementNumber").val('')
        $("#ApplicationPremiumTable").UifDataTable('clear');
        $("#alertReversionView").UifAlert('hide')
    }
    static ModelPolicy(PolicyNumber) {
        var policy = { DocumentNumber: PolicyNumber, PrefixId: $('#PrefixRevSearch').val(), BranchId: $('#BranchRevSearch').val() }
        return policy;
    }
    static FormatReverseApplicationPremium(quotas) {
        for (let i = 0; i < quotas.length; i++) {
            quotas[i].AccountingDate = FormatDate(quotas[i].AccountingDate);
            quotas[i].Amount = FormatMoney(quotas[i].Amount);
        }
        return quotas;
    }

}
$(() => {
    new ReverseApplicationPremium();
});