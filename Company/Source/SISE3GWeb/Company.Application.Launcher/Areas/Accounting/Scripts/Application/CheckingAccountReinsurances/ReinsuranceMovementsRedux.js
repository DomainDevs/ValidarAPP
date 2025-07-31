class ReinsuranceMovementsRedux {
    static redux(State, Actions) {
        State = State || {
            ViewModel: {}, uifNotifyModel: null, Bags: { Movements: [] }, ViewState: {}, Data: {}
        };
        switch (Actions.type) {
            case 'INIT_REINSURANCE':
                if (Actions.Bags.Branches != null) {
                    State.Bags.Branches = Actions.Bags.Branches;
                }
                if (Actions.Bags.Prefixes != null) {
                    State.Bags.Prefixes = Actions.Bags.Prefixes;
                }
                if (Actions.Bags.ContractType != null) {
                    State.Bags.ContractType = Actions.Bags.ContractType;
                }
                if (Actions.Bags.Natures != null) {
                    State.Bags.Natures = Actions.Bags.Natures;
                }
                if (Actions.Bags.Currencies != null) {
                    State.Bags.Currencies = Actions.Bags.Currencies;
                }
                if (Actions.Bags.ApplicationsMoth != null) {
                    State.Bags.ApplicationsMoth = Actions.Bags.ApplicationsMoth;
                }
                if (Actions.Bags.accountingCompanies != null) {
                    State.Bags.accountingCompanies = Actions.Bags.AccountingCompanies;
                }
                return State;
        }
    }
}

class ReinsuranceMovements {
    static actionCreators(getState, dispatch) {
        $(document).on('ready', function () {

            ReinsuraceMovementsRequest.GetBranchs().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { Branches: data.data } });
            });

            ReinsuraceMovementsRequest.GetTechnicalPrefixes().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { Prefixes: data.data } });
            });

            ReinsuraceMovementsRequest.GetContractTypeEnabled().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { ContractType: data.data } });
            });

            ReinsuraceMovementsRequest.GetNatures().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { Natures: data.data } });
            });

            ReinsuraceMovementsRequest.GetCurrencies().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { Currencies: data.data } });
            });

            ReinsuraceMovementsRequest.GetYearMonths().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { ApplicationsMoth: data.data } });
            });

            ReinsuraceMovementsRequest.GetAccountingCompanies().done(function (data) {
                dispatch({ type: 'INIT_REINSURANCE', Bags: { AccountingCompanies: data.data } });
            });

        });
    }

    static outputState(State, dispatch) {

        $("#selectReinsuranceBranch").UifSelect({ sourceData: state.bags.Branches, selectedId: state.viewModel.selectedBranchId });
        $("#selectReinsurancePrefix").UifSelect({ sourceData: state.bags.Prefixes, selectedId: state.viewModel.selectedPrefixId });
        $("#selectReinsuranceContractType").UifSelect({ sourceData: state.bags.ContractType, selectedId: state.viewModel.selectedContractTypeId });
        $("#selectReinsuranceNature").UifSelect({ sourceData: state.bags.Natures, selectedId: state.viewModel.selectedNatureId });
        $("#selectReinsuranceCurrency").UifSelect({ sourceData: state.bags.Currencies, selectedId: state.viewModel.selectedCurrencyId });
        $("#selectReinsuranceApplicationMonth").UifSelect({ sourceData: state.bags.ApplicationsMoth, selectedId: state.viewModel.selectedApplicationsMothId });
        $("#selectReinsuranceSalePoint").UifSelect({ sourceData: state.bags.SalesPoint, selectedId: state.viewModel.selectedSalesPointId });
        $("#selectReinsuranceAccountingAccountConcept").UifSelect({ sourceData: state.bags.Concepts, selectedId: state.viewModel.selectedConceptId });
        $("#selectReinsuranceSubPrefix").UifSelect({ sourceData: state.bags.subPrefix, selectedId: state.viewModel.selectedSubPrefixId });
        $("#selectReinsuranceStretch").UifSelect({ sourceData: state.bags.Stretch, selectedId: state.viewModel.selectedStretchId });
        $('#ReinsuranceMovementDescription').val(state.viewModel.selectedMovementDescription);
        $("#selectReinsuranceCompany").UifSelect({ sourceData: state.bags.accountingCompanies, selectedId: state.viewModel.selectedaccountingCompanyId });
        $("#ReinsuranceApplicationYear").val(state.viewModel.selectedApplicationYear);
        $("#selectReinsurancePolicyBranch").UifSelect({ selectedId: state.viewModel.selectedReinsurancePolicyBranchId });
        $("#selectReinsurancePolicyPrefix").UifSelect({ selectedId: state.viewModel.selectedReinsurancePolicyPrefixId });
        $('#ReinsurancePolicy').val(state.viewModel.selectedReinsurancePolicy);
        $('#ReinsuranceEndorsement').val(state.viewModel.selectedReinsuranceEndorsement);
        $("#TotalReinsuranceMovement").text(FormatMoney(state.Data.TotalReinsuranceMovements));
        $("#TotalDebitReinsuranceMovement").text(FormatMoney(state.Data.TotalDebitReinsuranceMovements));

    }
}