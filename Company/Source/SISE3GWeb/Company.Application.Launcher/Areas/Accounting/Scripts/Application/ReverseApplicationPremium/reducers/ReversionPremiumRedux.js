class ReversionPremiumRedux {
    static redux(state, actions) {
        state = state || {
            Aplicate: false,
            Imputation: {},
            ViewModel: {},
            reversionPremiumItem: {}
        };
        switch (actions.type) {
            case 'ADD_PREMIUM':
                state.reversionPremiumItem = actions.reversionPremiumItem;
                return state;
            case 'ADD_IMPUTATION_REV':
                state.Imputation = actions.Imputation;
                return state;

            case 'PAY_APLICATION':
                state.Aplicate = actions.Aplicate;
                return state;
            case 'PREMPAY_ITEM_ACCEPT_BUTTON':
                return state;
            default: return state;


        }
    }
}
class ReversionPremium {
    static async Createpremium(state, dispatch) {
        let msj = "";
        dispatch({ type: 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: store.getState().accountingRedux.TempApplicationId });
        var data = { Id: state.reversionPremiumItem.EndorsementId, Number: state.reversionPremiumItem.PaymentNumber, PayerId: state.reversionPremiumItem.PayerId, PremiumId: state.reversionPremiumItem.Id, IsReversion: true };
        if (DialogSearchPolicies.isNull(store.getState().dialogSearchPoliciesRedux.CurrentIndex)) {
            return await ReverseApplicationPremiumRequest.ValidatePremiumTemporal(data).done(function (checkResult) {
                if (checkResult.success) {
                    dispatch({ type: 'ADD_IMPUTATION_REV', Imputation: checkResult.result });
                    if (state.Imputation != null && state.Imputation.Id == 0) {
                        dispatch({ type: 'PAY_APLICATION', Aplicate: true });
                        state.ViewModel = { TempImputationId: state.Imputation.Id };
                    }
                    else {
                        dispatch({ type: 'PAY_APLICATION', Aplicate: false });
                        msj = Resources.BranchPrefixPolicyEndorsement + ": " + state.reversionPremiumItem.BranchPrefixPolicyEndorsement + ", " +
                            Resources.Quota + ": " + state.reversionPremiumItem.PaymentNumber + ", " + Resources.AlreadyAppliedTemporalImputationQuotaWarning + " No.: " + state.Imputation.Id
                        $("#alertReversionView").UifAlert('show', msj, "warning");
                    }
                }
                else {
                    dispatch({ type: 'PAY_APLICATION', Aplicate: false });
                    msj = Resources.BranchPrefixPolicyEndorsement + ": " + data.BranchPrefixPolicyEndorsement + ", " +
                        Resources.Quota + ": " + data.PaymentNumber + ", " + Resources.AlreadyAppliedTemporalImputationQuotaWarning + " No.: " + state.Imputation.Id + " - " + Resources.Branch + ": " + state.Imputation.Id;
                    $("#alertReversionView").UifAlert('show', msj, "warning");

                }
            });
        }       
     
    }   
}

