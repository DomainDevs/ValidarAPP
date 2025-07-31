class DialogSearchPoliciesRedux {
    static redux(state, actions) {
        var selectedItemId;
        state = state || {
            Persist: true,
            ViewModel: {},
            ViewSearchModel: {},
            uifNotifyModel: null,
            Bags: {},
            ViewState: {},
            EnchangeRate: 1,
            Data: {
            },
            ApplicationPremiumModel: {
                ImputationId: 0,
                TotalPremium: 0,
                IsDiscountedCommisson: null,
                ApplicationPremiumItems: [],
                CommissionsDiscounted: []
            },
            ApplicationPremiumItem: {
                ApplicationPremiumId: null,
                PolicyId: null,
                EndorsementId: null,
                PaymentNum: null,
                PaymentAmount: null,
                PaymentLocalAmount: null,
                PayerId: null,
                IncomeAmount: null,
                LocalAmount: null,
                CurrencyCode: null,
                ExchangeRate: null,
                Amount: null,
                PayableAmount: null,
                UserId: null,
                RegisterDate: null,
                DiscountedCommisson: null
            },
            UsedDepositPremiumModel: {
                ApplicationPremiumId: 0, UsedAmounts: []
            },
            UsedDepositPremiumAmountModel:
                { UsedDepositPremiumId: 0, DepositPremiumTrasactionId: 0, Amount: 0 },
            DepositPremiumModel: {},
            DiscountedCommissions: [],
            CommissionDiscountedItem: {
                Id: null,//DiscountedCommissionId
                TempApplicationPremiumId: null,
                AgentTypeCode: null,
                AgentIndividualId: null,
                CurrencyCode: null,
                ExchangeRate: null,
                BaseIncomeAmount: null,
                BaseAmount: null,
                CommissionPercentage: null,
                CommissionType: null,
                Amount: null,//CommissionDiscountIncomeAmount
                LocalAmount: null,//CommissionDiscountIncomeAmount
                CommissionDiscountAmount: null,
                AgentAgencyId: null
            },
            TotalCommissionFoot: null
        };
        switch (actions.type) {

            case 'PREMPAY_SET_EXCHANGE_RATE':
                state.ApplicationPremiumItem.ExchangeRate = actions.ExchangeRate;
                state.ApplicationPremiumItem.OriginalExchangeRate = actions.ExchangeRate;

                if (!isNull(state.ApplicationPremiumItem.PaymentAmount)) {
                    state.ApplicationPremiumItem.PaymentLocalAmount = state.ApplicationPremiumItem.PaymentAmount * state.ApplicationPremiumItem.ExchangeRate;
                }
                return state;

            case 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID':
                state.ViewModel.TempImputationId = actions.TempImputationId;
                return state;

            case 'PREMPAY_ASSIGN_ACCOUNTING_DATE':
                state.ViewModel.AccountingDate = actions.AccountingDate;
                return state;

            case 'PREMPAY_SET_SEARCH_TYPE':
                state.ViewSearchModel.InsuredId = "";
                state.ViewSearchModel.DocumentNumber = "";
                state.ViewSearchModel.InsuredName = "";
                state.ViewSearchModel.InsuredIdControl = "";
                state.ViewSearchModel.InsuredDocNumLast = "";
                state.ViewState.ShowSelectSearch = true;
                state.ViewState.AgentSearch = false;
                state.ViewState.PayerSearch = false;
                state.ViewState.InsuredSearch = false;
                state.ViewState.SearchAdvancedSection = false;
                state.ViewState.GroupSearch = false;
                state.ViewState.PolicySearch = false;
                state.ViewState.SalesTicketSearch = false;
                selectedItemId = actions.selectedItemSearchBy.Id;
                state.ViewSearchModel.SelectedItemSearchById = parseInt(selectedItemId);
                if (selectedItemId == 1 || selectedItemId == 2 || selectedItemId == 3) {
                    state.ViewState.SearchAdvancedSection = true;
                    if (selectedItemId == 1) {
                        state.ViewState.InsuredSearch = true;
                    } else if (selectedItemId == 2) {
                        state.ViewState.PayerSearch = true;
                    } else if (selectedItemId == 3) {
                        state.ViewState.AgentSearch = true;
                    }
                } else if (selectedItemId == 4) {
                    state.ViewState.GroupSearch = true;
                } else if (selectedItemId == 5) {
                    state.ViewState.PolicySearch = true;
                } else if (selectedItemId == 6) {
                    state.ViewState.SalesTicketSearch = true;
                }
                return state;
            case 'PREMPAY_SET_SEARCH_AGENT_DOCUMENT_NUMBER':
                state.ViewState.ShowAgentData = true;
                state.ViewSearchModel.AgentId = parseInt(actions.ViewModel.selectedItem.Id);
                state.ViewSearchModel.AgentDocumentNumber = actions.ViewModel.selectedItem.DocumentNumber;
                state.ViewSearchModel.AgentName = actions.ViewModel.selectedItem.Name;
                state.ViewSearchModel.AgentIdControl = actions.ViewModel.agentIdControl;
                state.ViewSearchModel.AgentDocNumLast = actions.ViewModel.agentDocNumLast;
                return state;
            case 'PREMPAY_SET_EMPTY_SEARCH_ANGET_DOCUMENT_NUMBER':
                state.ViewState.ShowagentData = false;
                state.ViewSearchModel.AgentId = "";
                state.ViewSearchModel.AgentDocumentNumber = "";
                state.ViewSearchModel.AgentName = "";
                state.ViewSearchModel.AgentIdControl = "";
                state.ViewSearchModel.AgentDocNumLast = "";
                return state;
            case 'PREMPAY_BLUR_SEARCH_AGENT_DOCUMENT_NUMBER':
                state.ViewState.OnBlurAgentData = true;
                if (state.ViewSearchModel.InsuredDocNumLast != actions.insuredId) {
                    if (state.ViewSearchModel.InsuredIdControl == "") {
                        state.ViewSearchModel.InsuredId = "";
                    }
                }
                else {
                    state.ViewSearchModel.InsuredIdControl = "";
                }
                return state;
            case 'PREMPAY_BLUR_SET_NEW_AMOUNT':
                if (DialogSearchPolicies.isNull(state.ApplicationPremiumItem.ExchangeRate)) {
                    return state;
                }
                state.ApplicationPremiumItem.PaymentAmount = actions.newAmount;
                state.ApplicationPremiumItem.PaymentLocalAmount = (state.ApplicationPremiumItem.ExchangeRate * actions.newAmount);
                return state;

            case 'PREMPAY_BLUR_SET_NEW_EXCHANGE_RATE':
                state.ApplicationPremiumItem.ExchangeRate = actions.newExchangeRate;
                state.ApplicationPremiumItem.PaymentLocalAmount = (state.ApplicationPremiumItem.PaymentAmount * actions.newExchangeRate);
                return state;

            case 'PREMPAY_BLUR_SET_NEW_TAX':
                state.ApplicationPremiumItem.Tax = actions.newTax;
                return state;

            case 'PREMPAY_BLUR_DATE_QUOTA_FROM':
                state.ViewState.OnBlurDateQuotaFrom = true;
                state.ViewSearchModel.DateFrom = actions.dateFrom;
                return state;
            case 'PREMPAY_BLUR_DATE_QUOTA_TO':
                state.ViewState.OnBlurDateQuotaTo = true;
                state.ViewSearchModel.DateTo = actions.dateTo;
                return state;
            case 'PREMPAY_SET_SEARCH_PAYER_DOCUMENT_NUMBER':
                state.ViewState.ShowPayerData = true;
                state.ViewSearchModel.PayerId = parseInt(actions.ViewModel.selectedItem.Id);
                state.ViewSearchModel.PayerDocumentNumber = actions.ViewModel.selectedItem.DocumentNumber;
                state.ViewSearchModel.PayerName = actions.ViewModel.selectedItem.Name;
                state.ViewSearchModel.PayerIdControl = actions.ViewModel.PayerIdControl;
                state.ViewSearchModel.PayerDocNumLast = actions.ViewModel.PayerDocNumLast;
                return state;
            case 'PREMPAY_SET_EMPTY_SEARCH_PAYER_DOCUMENT_NUMBER':
                state.ViewState.ShowPayerData = false;
                state.ViewSearchModel.PayerId = "";
                state.ViewSearchModel.PayerDocumentNumber = "";
                state.ViewSearchModel.PayerName = "";
                state.ViewSearchModel.PayerIdControl = "";
                state.ViewSearchModel.PayerDocNumLast = "";
                return state;
            case 'PREMPAY_BLUR_SEARCH_PAYER_DOCUMENT_NUMBER':
                state.ViewState.OnBlurPayerData = true;
                if (state.ViewSearchModel.PayerDocNumLast != actions.newPayerId) {
                    if (state.ViewSearchModel.PayerIdControl == "") {
                        state.ViewSearchModel.PayerId = "";
                        state.ViewSearchModel.PayerName = "";
                        state.ViewSearchModel.payerDocument = "";
                    }
                }
                else {
                    state.ViewSearchModel.PayerIdControl = "";
                }
                return state;
            case 'PREMPAY_SET_INSURED_DOCUMENT_NUMBER':
                state.ViewState.ShowInsuredData = true;
                state.ViewSearchModel.InsuredId = parseInt(actions.ViewModel.selectedItem.Id);
                state.ViewSearchModel.InsuredDocumentNumber = actions.ViewModel.selectedItem.DocumentNumber;
                state.ViewSearchModel.InsuredName = actions.ViewModel.selectedItem.Name;
                state.ViewSearchModel.InsuredIdControl = actions.ViewModel.insuredIdControl;
                state.ViewSearchModel.InsuredDocNumLast = actions.ViewModel.insuredDocument;
                return state;
            case 'PREMPAY_SET_SEARCH_INSURED_DOCUMENT_NUMBER':
                state.ViewState.ShowInsuredDataSearch = true;
                state.ViewSearchModel.InsuredId = parseInt(actions.ViewModel.selectedItem.Id);
                state.ViewSearchModel.InsuredDocumentNumber = actions.ViewModel.selectedItem.DocumentNumber;
                state.ViewSearchModel.InsuredName = actions.ViewModel.selectedItem.Name;
                state.ViewSearchModel.InsuredIdControl = actions.ViewModel.insuredIdControl;
                state.ViewSearchModel.InsuredDocNumLast = actions.ViewModel.insuredDocument;
                return state;
            case 'PREMPAY_SET_SEARCH_GROUP_NUMBER':
                state.ViewState.ShowGroupData = true;
                state.ViewSearchModel.GroupId = parseInt(actions.ViewModel.selectedItem.Id);
                state.ViewSearchModel.GroupName = actions.ViewModel.selectedItem.Description;
                state.ViewSearchModel.GroupIdControl = actions.ViewModel.GroupIdControl;
                state.ViewSearchModel.GroupNumberLast = actions.ViewModel.groupNumber;
                return state;

            case 'PREMPAY_SET_EMPTY_SEARCH_INSURED_DOCUMENT_NUMBER':
                state.ViewSearchModel.InsuredId = "";
                state.ViewSearchModel.InsuredDocumentNumber = "";
                state.ViewSearchModel.InsuredName = "";
                state.ViewSearchModel.InsuredIdControl = "";
                state.ViewSearchModel.InsuredDocNumLast = "";
                return state;
            case 'PREMPAY_SET_EMPY_SEARCH_GROUP_NUMBER':
                state.ViewSearchModel.GroupId = "";
                state.ViewSearchModel.GroupDocumentNumber = "";
                state.ViewSearchModel.GroupName = "";
                state.ViewSearchModel.GroupIdControl = "";
                state.ViewSearchModel.GroupNumberLast = "";
                return state;

            case 'PREMPAY_BLUR_SEARCH_INSURED_DOCUMENT_NUMBER':
                state.ViewState.OnBlurInsuredData = true;
                if (state.ViewSearchModel.InsuredDocNumLast != actions.insuredId) {
                    if (state.ViewSearchModel.InsuredIdControl == "") {
                        state.ViewSearchModel.InsuredId = "";
                    }
                }
                else {
                    state.ViewSearchModel.InsuredIdControl = "";
                }
                return state;
            case 'PREMPAY_BLUR_SEARCH_INSURED_DOCUMENT_NAME':
                state.ViewState.OnBlurInsuredData = true;
                if (state.ViewSearchModel.InsuredName != actions.insuredName) {
                    if (state.ViewSearchModel.InsuredIdControl == "") {
                        state.ViewSearchModel.InsuredId = "";
                    }
                }
                else {
                    state.ViewSearchModel.InsuredIdControl = "";
                }
                return state;
            case 'PREMPAY_BLUR_SEARCH_GROUP_NUMBER':
                state.ViewState.OnBlurGroupNumberData = true;
                if (state.ViewSearchModel.GroupNumberLast != actions.GroupId) {
                    if (state.ViewSearchModel.GroupIdControl == "") {
                        state.ViewSearchModel.GroupId = "";
                    }
                }
                else {
                    state.ViewSearchModel.GroupIdControl = "";
                }
                return state;
            case 'PREMPAY_BLUR_SEARCH_GROUP_NAME':
                state.ViewState.OnBlurGroupNameData = true;
                if (state.ViewSearchModel.GroupName != actions.GroupName) {
                    if (state.ViewSearchModel.GroupIdControl == "") {
                        state.ViewSearchModel.GroupId = "";
                    }
                }
                else {
                    state.ViewSearchModel.GroupIdControl = "";
                }
                return state;
            case 'PREMPAY_CREATE_SRC':
                if (isEmpty(state.ViewSearchModel.InsuredId)) {
                    state.ViewSearchModel.InsuredId = '';
                }
                if (isEmpty(state.ViewSearchModel.PayerId)) {
                    state.ViewSearchModel.PayerId = '';
                }
                if (isEmpty(state.ViewSearchModel.AgentId)) {
                    state.ViewSearchModel.AgentId = '';
                }
                if (isEmpty(state.ViewSearchModel.EndorsementDocumentNumber)) {
                    state.ViewSearchModel.EndorsementDocumentNumber = '';
                }
                if (isEmpty(state.ViewSearchModel.GroupId)) {
                    state.ViewSearchModel.GroupId = '';
                }
                if (isEmpty(state.ViewSearchModel.PolicyId)) {
                    state.ViewSearchModel.PolicyId = '';
                }
                if (isEmpty(state.ViewSearchModel.InsuredId)) {
                    state.ViewSearchModel.InsuredId = '';
                }
                if (isEmpty(state.ViewSearchModel.PolicyDocumentNumber)) {
                    state.ViewSearchModel.PolicyDocumentNumber = '';
                }
                if (isEmpty(state.ViewSearchModel.SalesTicket)) {
                    state.ViewSearchModel.SalesTicket = '';
                }
                if (isEmpty(state.ViewSearchModel.InsuredDocumentNumber)) {
                    state.ViewSearchModel.InsuredDocumentNumber = '';
                }
                if (isEmpty(state.ViewSearchModel.DateTo)) {
                    state.ViewSearchModel.DateTo = '';
                }
                if (isEmpty(state.ViewSearchModel.DateFrom)) {
                    state.ViewSearchModel.DateFrom = '';
                }
                return state;
            case 'PREMPAY_SET_SEARCH_POLICIES_DATA':
                state.ViewSearchModel.SearchPoliciesData = actions.Data;
                return state;
            case 'PREMPAY_ACCEPT_DEPOSIT_PRIMEDIALOG_IS_CHEKED':

                state.UsedDepositPremiumModel.ApplicationPremiumId = state.ApplicationPremiumModel.ApplicationPremiumItems[0].ApplicationPremiumId;
                for (var j in actions.Data) {
                    state.UsedDepositPremiumAmountModel = {
                        UsedDepositPremiumId: 0,
                        DepositPremiumTrasactionId: 0,
                        Amount: 0
                    };

                    state.UsedDepositPremiumAmountModel.UsedDepositPremiumId = 0;
                    state.UsedDepositPremiumAmountModel.DepositPremiumTrasactionId = actions.Data[j].DepositPremiumTransactionId;
                    state.UsedDepositPremiumAmountModel.Amount = parseFloat(RemoveFormatMoney(actions.Data[j].UsedAmount));

                    state.UsedDepositPremiumModel.UsedAmounts.push(state.UsedDepositPremiumAmountModel);
                }

                return state;

            case 'PREMPAY_LOAD_PREMIUMS':
                state.ApplicationPremiumModel.ApplicationPremiumItems = actions.Premiums;//todo
                state.ApplicationPremiumModel.TotalPremium = fix(actions.TotalPremium);
                state.ViewState.LoadPremiums = true;
                return state;
            case 'PREMPAY_SET_INCOME_AMOUNT':
                state.ApplicationPremiumItem.IncomeAmount = actions.IncomeAmount;
                return state;

            case 'PREMPAY_ITEM_APPLY_VIEW_ROW_EDIT':
                $.each(state.ApplicationPremiumModel.ApplicationPremiumItems, function (i, value) {
                    if (actions.Data.ApplicationPremiumId == value.ApplicationPremiumId) {
                        state.ApplicationPremiumItem = value;
                        state.QuotaList = [];
                        var found = true;
                        if (value.EndorsementId == state.ApplicationPremiumItem.EndorsementId) {
                            var found = false;
                            for (var j = 0; j < state.QuotaList.length; j++) {
                                if (state.QuotaList[j].PaymentNumber == state.ApplicationPremiumItem.PaymentNumber) {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                state.QuotaList.push({
                                    'PaymentNumber': state.ApplicationPremiumItem.PaymentNumber
                                });
                            }
                        }
                        return;
                    }
                });

                state.ApplicationPremiumItem.CurrencyCode = state.ApplicationPremiumItem.CurrencyId;
                state.ApplicationPremiumItem.PaymentNum = state.ApplicationPremiumItem.PaymentNumber;

                var tax = 0;
                var expenses = 0;
                var paymentAmount = 0;
                var commissionLocalValue = 0;

                var result = actions.DataTempApplicationPremiumComponents.filter(tempApplicationPremiumComponent => tempApplicationPremiumComponent.ComponentTinyDescription == "I");
                if (Array.isArray(result) && result.length > 0) {
                    tax = result[0].LocalAmount;
                    var resultTax = actions.DataPremiumComponents.filter(premiumComponent => premiumComponent.ComponentId == result[0].ComponentCode);
                    if (Array.isArray(resultTax) && resultTax.length > 0) {
                        state.ApplicationPremiumItem.TaxOriginal = resultTax[0].LocalAmount;
                    }
                }

                result = actions.DataTempApplicationPremiumComponents.filter(tempApplicationPremiumComponent => tempApplicationPremiumComponent.ComponentTinyDescription == "P");
                if (Array.isArray(result) && result.length > 0) {
                    paymentAmount = result[0].Amount;
                    state.ApplicationPremiumItem.ExchangeRate = result[0].ExchangeRate;
                    var resultPremium = actions.DataPremiumComponents.filter(premiumComponent => premiumComponent.ComponentId == result[0].ComponentCode);
                    if (Array.isArray(resultPremium) && resultPremium.length > 0) {
                        state.ApplicationPremiumItem.PremiumOriginalAmount = resultPremium[0].Amount;
                    }
                }
                result = actions.DataTempApplicationPremiumComponents.filter(tempApplicationPremiumComponent => tempApplicationPremiumComponent.ComponentTinyDescription == "G");
                if (Array.isArray(result) && result.length > 0) {
                    expenses = result[0].LocalAmount;
                    var resultExpenses = actions.DataPremiumComponents.filter(premiumComponent => premiumComponent.ComponentId == result[0].ComponentCode);
                    if (Array.isArray(resultExpenses) && resultExpenses.length > 0) {
                        state.ApplicationPremiumItem.OriginalExpenses = resultExpenses[0].LocalAmount;
                    }
                }

                if (expenses == 0) {
                    state.ViewState.CheckNoExpenses = true;
                } else {
                    state.ViewState.CheckNoExpenses = false;
                }
                var paymentLocalAmount = paymentAmount * state.ApplicationPremiumItem.ExchangeRate;
                state.ApplicationPremiumItem.Tax = tax;
                state.ApplicationPremiumItem.PaymentAmount = paymentAmount;
                state.ApplicationPremiumItem.PaymentLocalAmount = paymentLocalAmount;
                state.ApplicationPremiumItem.Expenses = expenses;

                if (!isNull(state.ApplicationPremiumItem.DiscountedCommission)) {
                    commissionLocalAmount = fix(state.ApplicationPremiumItem.DiscountedCommission);
                }
                state.ApplicationPremiumItem.TotalPremium = fix(expenses + tax + paymentLocalAmount - commissionLocalAmount);

                state.EndorsementList = [];
                var found = false;

                for (var j = 0; j < state.EndorsementList.length; j++) {
                    if (state.EndorsementList[j].EndorsementId == state.ApplicationPremiumItem.EndorsementId) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    state.EndorsementList.push({
                        'EndorsementId': state.ApplicationPremiumItem.EndorsementId,
                        'EndorsementNumber': state.ApplicationPremiumItem.EndorsementNumber
                    });
                }


                state.ViewState.ShowItemApplyViewRowEdit = true;
                state.ViewState.DisabledCheckNoExpenses = true;
                return state;

            case 'PREMPAY_DPOSIT_PREMIUM_VIEW_ROW_EDIT':
                state.ViewState.IsDepositPremiumRowEdit = true;
                state.Data.DepositPremiumRowEdit = actions.Data;
                state.Data.DepositPremiumRowEdit.CurrentEditIndex = actions.Position;
                return state;
            case 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT':
                state.ApplicationPremiumItem = actions.Data;
                state.ApplicationPremiumItem.PaymentNum = state.ApplicationPremiumItem.PaymentNumber;
                state.ApplicationPremiumItem.CurrencyCode = state.ApplicationPremiumItem.CurrencyId;

                if (state.EndorsementList == undefined) {
                    state.EndorsementList = [];
                }
                var foundIt = false;

                for (var j = 0; j < state.EndorsementList.length; j++) {
                    if (state.EndorsementList[j].EndorsementId == actions.Data.EndorsementId) {
                        foundIt = true;
                        break;
                    }
                }
                if (!foundIt) {
                    state.EndorsementList.push({
                        'EndorsementId': actions.Data.EndorsementId,
                        'EndorsementNumber': actions.Data.EndorsementNumber
                    });
                }

                state.QuotaList = [];
                foundIt = true;
                if (actions.Data.EndorsementId == state.ApplicationPremiumItem.EndorsementId) {
                    foundIt = false;
                    for (var j = 0; j < state.QuotaList.length; j++) {
                        if (state.QuotaList[j].PaymentNumber == actions.Data.PaymentNumber) {
                            foundIt = true;
                            break;
                        }
                    }
                    if (!foundIt) {
                        state.QuotaList.push({
                            'PaymentNumber': actions.Data.PaymentNumber
                        });
                    }
                }
                return state;
            case 'PREMPAY_SEARCH_BUTTON_CLICK':
                actions.ApplicationPremiumItem = actions.Data[0];

                actions.ApplicationPremiumItem.CurrencyCode = actions.ApplicationPremiumItem.CurrencyId;
                actions.ApplicationPremiumItem.PaymentNum = actions.Data[0].PaymentNumber;
                console.log(state.ApplicationPremiumItem);
                state.ApplicationPremiumItem = actions.ApplicationPremiumItem;
                state.EndorsementList = [];
                var found = false;

                for (var i = 0; i < actions.Data.length; i++) {
                    found = false;
                    for (var j = 0; j < state.EndorsementList.length; j++) {
                        if (state.EndorsementList[j].EndorsementId == actions.Data[i].EndorsementId) {
                            found = true;
                            break;
                        }
                    }

                    if (!found) {
                        state.EndorsementList.push({
                            'EndorsementId': actions.Data[i].EndorsementId,
                            'EndorsementNumber': actions.Data[i].EndorsementNumber
                        });
                    }
                }

                state.QuotaList = [];
                var found = true;
                for (var i = 0; i < actions.Data.length; i++) {
                    if (actions.Data[i].EndorsementId == state.ApplicationPremiumItem.EndorsementId) {
                        found = false;
                        for (var j = 0; j < state.QuotaList.length; j++) {
                            if (state.QuotaList[j].PaymentNumber == actions.Data[i].PaymentNumber) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            state.QuotaList.push({
                                'PaymentNumber': actions.Data[i].PaymentNumber
                            });
                        }
                    }
                }
                return state;
            case 'PREMPAY_SET_COMPONENTS_VALUES_SEARCH':
                state.ApplicationPremiumItem.PaymentNum = actions.Data.PaymentNumber;

                var taxComp = 0;
                var expensesComp = 0;
                var paymentAmountComp = 0;

                var result = actions.DataPremiumComponents.filter(PremiumComponent => PremiumComponent.TinyDescription == "I");
                if (Array.isArray(result) && result.length > 0) {
                    taxComp = result[0].LocalAmount;
                    state.ApplicationPremiumItem.TaxOriginal = result[0].LocalAmount;
                }

                result = actions.DataPremiumComponents.filter(PremiumComponent => PremiumComponent.TinyDescription == "P");
                if (Array.isArray(result) && result.length > 0) {
                    paymentAmountComp = result[0].Amount;
                    state.ApplicationPremiumItem.PremiumOriginalAmount = result[0].Amount;
                }
                result = actions.DataPremiumComponents.filter(PremiumComponent => PremiumComponent.TinyDescription == "G");
                if (Array.isArray(result) && result.length > 0) {
                    expensesComp = result[0].LocalAmount;
                    state.ApplicationPremiumItem.OriginalExpenses = result[0].LocalAmount;
                }

                var paymentLocalAmountComp = paymentAmountComp * state.ApplicationPremiumItem.ExchangeRate;
                state.ApplicationPremiumItem.Tax = taxComp;
                state.ApplicationPremiumItem.PaymentAmount = paymentAmountComp;
                state.ApplicationPremiumItem.PaymentLocalAmount = paymentLocalAmountComp;
                state.ViewState.DisabledCheckNoExpenses = true;
                if (expensesComp == 0) {
                    state.ViewState.CheckNoExpenses = true;
                } else {
                    state.ViewState.CheckNoExpenses = false;
                }
                state.ApplicationPremiumItem.Expenses = expensesComp;

                state.ApplicationPremiumItem.TotalPremium = fix(expensesComp + taxComp + paymentLocalAmountComp);
                return state;
            case 'PREMPAY_SET_CHECK_EXPENSES':
                if (actions.isChecked == true) {
                    state.ApplicationPremiumItem.Expenses = 0;
                } else {
                    if (!DialogSearchPolicies.isNull(state.ApplicationPremiumItem.OriginalExpenses)) {
                        state.ApplicationPremiumItem.Expenses = state.ApplicationPremiumItem.OriginalExpenses;
                    }
                }
                state.ApplicationPremiumItem.NoExpenses = actions.isChecked;
                return state;
            case 'PREMPAY_SET_TOTAL_PREMIUM':
                state.ViewState.ShowItemApplyViewRowEdit = true;
                var commissionLocalAmount = 0;
                if (!isNull(state.CommissionDiscountedItem) && !isEmptyorZero(state.CommissionDiscountedItem.Amount)) {
                    commissionLocalAmount = fix(state.CommissionDiscountedItem.Amount * state.CommissionDiscountedItem.ExchangeRate);//descontada
                } else if (!isNull(state.ApplicationPremiumItem) && !isEmptyorZero(state.ApplicationPremiumItem.DiscountedCommission)) {
                    commissionLocalAmount = fix(state.ApplicationPremiumItem.DiscountedCommission);
                }
                state.ApplicationPremiumItem.TotalPremium = fix(state.ApplicationPremiumItem.Expenses + state.ApplicationPremiumItem.Tax + state.ApplicationPremiumItem.PaymentLocalAmount - commissionLocalAmount);
                return state;

            case 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT_2':

                if (actions.PaymentNumber == -1) {
                    state.QuotaList = [];
                    var found = false;
                    for (var i = 0; i < state.ApplicationPremiumModel.ApplicationPremiumItems.length; i++) {
                        if (state.ApplicationPremiumModel.ApplicationPremiumItems[i].EndorsementId == state.ApplicationPremiumItem.EndorsementId) {
                            found = false;
                            for (var j = 0; j < state.QuotaList.length; j++) {
                                if (state.QuotaList[j].PaymentNumber == state.ApplicationPremiumModel.ApplicationPremiumItems[i].PaymentNumber) {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                state.QuotaList.push({
                                    'PaymentNumber': state.ApplicationPremiumModel.ApplicationPremiumItems[i].PaymentNumber
                                });
                            }
                        }
                    }
                }

                for (var i = 0; i < state.ApplicationPremiumModel.ApplicationPremiumItems.length; i++) {
                    if (state.ApplicationPremiumModel.ApplicationPremiumItems[i].EndorsementId == actions.EndorsementId
                        && (actions.PaymentNumber == -1 || actions.PaymentNumber == state.ApplicationPremiumModel.ApplicationPremiumItems[i].PaymentNumber)
                    ) {
                        state.ApplicationPremiumItem = state.ApplicationPremiumModel.ApplicationPremiumItems[i];
                        state.ApplicationPremiumItem.PaymentNum = state.ApplicationPremiumItem.PaymentNumber;
                        state.ViewState.SetDataQuota = true;
                        break;
                    }
                }


                return state;

            case 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT_SAVE':
                state.ApplicationPremiumModel.IsDiscountedCommisson = actions.IsDiscountedCommisson;
                state.ApplicationPremiumItem.EnchangeRate = state.EnchangeRate;//consulta
                state.ApplicationPremiumItem.PaymentNum = state.ApplicationPremiumItem.PaymentNumber;
                state.ApplicationPremiumItem.IncomeAmount = state.ApplicationPremiumModel.ApplicationPremiumItems[actions.index].PayableAmount;
                state.ApplicationPremiumItem.ExchangeRate = state.EnchangeRate;//
                state.ApplicationPremiumItem.Amount = state.ApplicationPremiumModel.ApplicationPremiumItems[actions.index].PayableAmount * parseFloat(state.EnchangeRate);
                state.ApplicationPremiumItem.RegisterDate = getCurrentDate();
                state.ApplicationPremiumItem.DiscountedCommisson = state.DiscountedCommisson;
                state.ApplicationPremiumModel.ApplicationPremiumItems[actions.index] = state.ApplicationPremiumItem;
                return state;
            case 'PREMPAY_ACTION_VIEW_SAVE_CLICK':
                state.ViewState.OnActionSaveClick = true;
                state.Data.DepositPremiumRowEdit.UsedAmount = actions.UsedAmount;
                return state;
            case 'PREMPAY_SET_VALUE_ITEMS_TOAPPLY':
                state.Data.ApplicationPremiumItems = actions.DataRowEditItemApplylabel;
                return state;
            case 'PREMPAY_ENCHANGERATE_LOAD':
                state.EnchangeRate = actions.EchangeRateData;
                return state;
            case 'PREMPAY_SET_ENCHANGERATE_VALUE':
                state.ViewState.changeExchangeRate = true;
                state.ApplicationPremiumItem.ExchangeRate = actions.EchangeRateData;
                state.ApplicationPremiumItem.CurrencyCode = actions.CurrencyCode;
                state.EnchangeRate = actions.EchangeRateData;
                state.CurrencyCode = actions.CurrencyCode;
                state.ApplicationPremiumItem.PaymentLocalAmount = state.ApplicationPremiumItem.PaymentAmount * actions.EchangeRateData;
                return state;
            case 'PREMPAY_CHANGE_DISCOUNTED_COMMISSION_CHEKED':
                state.DiscountedCommisson = actions.DiscountedCommisson;
                return state;
            case 'PREMPAY_CLEAN_SEARCH_POLICY':
                state.ViewSearchModel.InsuredDocumentNumberSearch = "";
                state.ViewSearchModel.InsuredDocumentNumber = "";
                state.ViewSearchModel.InsuredNameSearch = "";
                state.ViewSearchModel.SelectedItemSearchById = "";
                state.ViewSearchModel.DateFrom = "";
                state.ViewSearchModel.DateTo = "";
                state.ViewSearchModel.GroupId = "";
                state.ViewSearchModel.GroupName = "";
                state.ViewSearchModel.PolicyDocumentNumber = "";
                state.ViewSearchModel.Endrosementctrl = "";
                state.ViewSearchModel.PayerDocumentNumber = "";
                state.ViewSearchModel.PayerName = "";
                state.ViewSearchModel.AgentDocumentNumberDialog = "";
                state.ViewSearchModel.AgentNameDialog = "";
                state.ViewSearchModel.Endorsement = "";
                state.ViewSearchModel.EndorsementId = "";
                state.ViewSearchModel.InsuredId = "";
                state.ViewSearchModel.PayerId = "";
                state.ViewSearchModel.AgentId = "";
                state.ViewSearchModel.EndorsementDocumentNumber = "";
                state.ViewSearchModel.BranchId = "";
                state.ViewSearchModel.PrefixId = "";
                //state.ViewModel.PolicyId = "";
                state.PreviusTempApplication = null;
                return state;
            case 'PREMPAY_CLEAN':
                state.ViewState.ShowSelectSearch = false;
                state.ViewState.SearchAdvancedSection = false;
                state.ViewState.InsuredSearch = false;
                state.ViewState.PayerSearch = false;
                state.ViewState.AgentSearch = false;
                state.ViewState.SalesTicketSearch = false;
                state.ViewState.GroupSearch = false;
                state.ViewState.PolicySearch = false;
                state.ViewState.ShowInsuredDataSearch = false;
                state.ViewState.ShowSelectSearch = false;
                state.ViewState.ShowInsuredData = false;

                state.searchBy = {};
                state.PreviusTempApplication = null;
                state.ViewModel.InsuredId = "";
                state.ViewModel.InsuredDocumentNumber = "";
                state.ViewModel.PayerDocumentNumber = "";
                state.ViewModel.InsuredName = "";
                state.ViewModel.InsuredIdControl = "";
                state.ViewModel.InsuredDocNumLast = "";
                state.ViewModel.AgentId = "";
                state.ViewModel.EndorsementDocumentNumber = "";
                state.ViewModel.GroupId = "";
                state.ViewModel.PolicyDocumentNumber = "";
                state.ViewModel.SalesTicket = "";
                state.ViewModel.BranchId = "";
                state.ViewModel.PrefixId = "";
                state.ViewModel.DateFrom = "";
                state.ViewModel.DateTo = "";
                state.ViewModel.PayViewSrc = "";
                //state.ViewModel.PolicyId = "";
                state.ViewModel.SelectedItemSearchById = "";
                state.ApplicationPremiumItem.QuotaValue = "";
                state.ApplicationPremiumItem.Tax = "";
                state.ApplicationPremiumItem.Expenses = "";
                state.ApplicationPremiumItem = [];
                state.DiscountedCommissions = [];
                state.TotalCommissionFoot = 0;
                return state;
            case 'PREMPAY_CLEAN_SEARCH':
                state.ViewState.isCleaningSearchForm = true;
                state.ViewSearchModel = {};
                return state;
            case 'PREMPAY_INIT':
                state.ApplicationPremiumModel.ImputationId = state.ViewModel.TempImputationId;

                state.ViewState.OnInit = true;
                //ViewModels                
                if (!DialogSearchPolicies.isNull(actions.ViewModel)) {
                    state.ViewModel.ItemToApplyViewSrc = "PremiumsReceivable/GetTempPremiumReceivableItemByTempImputationId?tempImputationId=" + state.ViewModel.TempImputationId;
                    state.ViewModel.ApplyCollecId = actions.ViewModel.applyCollecId;
                    state.ViewModel.AccountingDate = actions.ViewModel.AccountingDate;
                }

                return state;
            case 'PREMPAY_ONINIT':
                //Bags
                if (!DialogSearchPolicies.isNull(actions.Bags)) {
                    if (!DialogSearchPolicies.isNull(actions.Bags.searchType)) {
                        state.Bags.SearchType = actions.Bags.searchType;
                    }
                    if (!DialogSearchPolicies.isNull(actions.Bags.branches)) {
                        state.Bags.Branches = actions.Bags.branches
                    }
                    if (!DialogSearchPolicies.isNull(actions.Bags.prefixes)) {
                        state.Bags.Prefixes = actions.Bags.prefixes;
                    }
                }
                return state;

            case 'PREMPAY_OFFINIT':
                state.ViewModel.ItemToApplyViewSrc = "";
                return state;

            case 'PREMPAY_SET_DATAFIELDS_EMPTY_SEARCH':

                state.UsedDepositPremiumModel = {
                    ApplicationPremiumId: 0, UsedAmounts: []
                };
                state.UsedDepositPremiumAmountModel =
                    { UsedDepositPremiumId: 0, DepositPremiumTrasactionId: 0, Amount: 0 };

                return state;

            case 'PREMPAY_SET_PAYMENTNUMBER_VALUE':
                state.ApplicationPremiumItem.PaymentNum = actions.value
                return state;
            case 'PREMPAY_DELETE_VALUE_ITEM_TO_APPLY':
                $.each(state.ApplicationPremiumModel.ApplicationPremiumItems, function (i, value) {
                    if (value.PolicyId == actions.Data.PolicyId &&
                        value.EndorsementId == actions.Data.EndorsementId &&
                        value.PaymentNumber == actions.Data.PaymentNumber
                    ) {
                        state.IndexToDeleteItem = i;
                    }
                });
                state.ApplicationPremiumModel.ApplicationPremiumItems.splice(state.IndexToDeleteItem, 1);

                return state;
            case 'PREMPAY_DELETE_ITEM_BALANCE_UPD':
                state.ApplicationPremiumModel.ApplicationPremiumItems.splice(actions.Id, 1);
                return state;
            case 'PREMPAY_SET_DATA_DEPOSIT_PREMIUM':
                state.DepositPremiumModel = actions.Data;
                return state;
            case 'PREMPAY_SET_DATA_DISCOUNTED_COMMISSION':
                state.TotalCommissionFoot = 0;
                state.ViewState.SetDataDiscountedCommissions = true;
                $.each(actions.Data, function (i, value) {
                    value.BaseAmount = fix(state.ApplicationPremiumItem.PaymentAmount * value.CommissionPercentage / 100) * (value.AgentPercentageParticipation / 100);
                    state.TotalCommissionFoot += value.BaseAmount;
                    value.CurrencyDescription = actions.CurrencyDescription;
                    value.CurrencyCode = actions.CurrencyCode;
                    value.ExchangeRate = actions.ExchangeRate;
                });
                state.DiscountedCommissions = actions.Data;
                return state;
            case 'PREMPAY_SET_VALUE':
                state.ViewState[actions.label] = actions.value;
                return state;
            case 'PREMPAY_SET_DROPDOWN_SEARCH':
                state.dropDownSearchAdv = actions.dropDown;
                return state;
            case 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_DISCOUNTED_VAL':
                state.ViewState.SetDataDiscountedCommissions = true;
                state.CommissionDiscountedItem.Amount = 0;
                state.CommissionDiscountedItem.Amount = parseFloat(actions.DiscountedAmount);
                $.each(state.DiscountedCommissions, function (i, value) {
                    if (value.AgentIndividualId == state.CommissionDiscountedItem.AgentIndividualId) {
                        value = state.CommissionDiscountedItem;
                    }
                });
                state.CommissionDiscountedItem.MaxCommission = 0;
                return state;
            case 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_ROW':
                state.CommissionDiscountedItem = actions.Data;
                state.CommissionDiscountedItem.MaxCommission = 0;
                state.CommissionDiscountedItem.MaxCommission = fix(state.ApplicationPremiumItem.PaymentAmount * actions.Data.CommissionPercentage / 100) * (actions.Data.AgentPercentageParticipation / 100);
                return state;
            case 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_ROW_EMPTY':
                state.CommissionDiscountedItem = {};
                return state;

            case 'PREMPAY_SET_USED_COMMISSION':
                state.CommissionDiscountedItem.Amount = actions.UsedCommission;//state.CommissionDiscountedItem.CommissionDiscountIncomeAmount = actions.UsedCommission;
                return state;
            case 'PREMPAY_SET_CHECK_PREMIUM_VALUES':
                state.PreviusTempApplication = actions.PreviusTempApplication;
                return state;
            case 'PREMPAY_SET_VALUES_SEARCH_ADV':
                state.searchBy = {};
                state.searchBy.id = actions.searchBy;
                state.searchBy.documentId = actions.documentId;
                state.searchBy.documentName = actions.documentName;
                state.ViewState.SetDataSearchBy = true;
                return state;

            case 'PREMPAY_SET_BRANCH':
                state.ViewSearchModel.BranchId = actions.selectedItem.Id;
                return state;

            case 'PREMPAY_SET_PREFIX':
                state.ViewSearchModel.PrefixId = actions.selectedItem.Id;
                return state;

            case 'PREMPAY_SET_POLICY_DOCUMENT_NUMBER':
                state.ViewSearchModel.PolicyDocumentNumber = actions.PolicyDocumentNumber;
                return state;

            case 'PREMPAY_SET_POLICY_ENDORSEMENT_NUMBER':
                state.ViewSearchModel.EndorsementDocumentNumber = actions.EndorsementNumber;
                return state;

            case 'PREMPAY_SET_POLICY_ENDORSEMENT_ID':
                state.ViewSearchModel.EndorsementId = actions.EndorsementId;
                return state;

            case 'PREMPAY_SET_POLICY_ID':
                state.ViewSearchModel.PolicyId = actions.PolicyId;
                return state;
            case 'PREMPAY_PREPARE_COMMISSION_DISCOUNT':
                var modelcommission = [];

                $.each(actions.data, function (i, value) {
                    if (!isEmptyorZero(value.Amount) || !isEmptyorZero(value.BaseAmount)) {
                        modelcommission.push(Object.assign({}, DialogSearchPolicies.SetDataDiscountedCommission(value, 0)));
                    }
                });

                state.DiscountedCommissions = modelcommission;
                return state;

            case 'PREMPAY_UPDATE_COMMISSION_DISCOUNT':
                var _total = 0;
                if (!isEmptyArray(state.DiscountedCommissions)) {
                    $.each(state.DiscountedCommissions, function (i, value) {
                        var baseAmount = actions.baseAmount;
                        var calculatedAmount = fix(baseAmount * value.CommissionPercentage / 100);
                        value.BaseAmount = calculatedAmount;

                        if (value.Amount != 0) {
                            var negatives = calculatedAmount < 0;
                            if (negatives) {
                                if (value.Amount < calculatedAmount) {
                                    value.Amount = calculatedAmount;
                                }
                            } else {
                                if (value.Amount > calculatedAmount) {
                                    value.Amount = calculatedAmount;
                                }
                            }
                            var localAmount = fix(value.Amount * actions.exchangeRate);
                            _total += fix(value.Amount);
                            value.ExchangeRate = actions.exchangeRate;
                            value.LocalAmount = localAmount;
                        }
                    });
                }
                state.TotalCommissionFoot = _total;
                state.ViewState.SetDataDiscountedCommissions = true;
                return state;
            default: return state;
        }
    }

}

class DialogSearchPolicies {

    static SetExchangeRate(getState, dispatch) {
        if (!isNull(getState.ApplicationPremiumItem) && !isNull(getState.ApplicationPremiumItem.ExchangeRate)) {
            DialogSearchPoliciesRequest.CalculateExchangeRateTolerance(getState.ApplicationPremiumItem.ExchangeRate, getState.ApplicationPremiumItem.CurrencyCode).done(function (result) {
                if (result === true) {
                    dispatch({ type: 'PREMPAY_SET_EXCHANGE_RATE', ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate });
                    if (!isNull(getState.ApplicationPremiumItem.PaymentAmount)) {
                        dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
                    }
                } else {
                    DialogSearchPolicies.GetExchangeRate(getState, dispatch);
                }
            });
        } else {
            DialogSearchPolicies.GetExchangeRate(getState, dispatch);
        }
    }

    static GetExchangeRate(getState, dispatch) {
        DialogSearchPoliciesRequest.GetCurrencyExchangeRate(getState.ViewModel.AccountingDate, getState.ApplicationPremiumItem.CurrencyCode)
            .done(function (response) {
                dispatch({ type: 'PREMPAY_SET_EXCHANGE_RATE', ExchangeRate: response });
                if (!isNull(getState.ApplicationPremiumItem.PaymentAmount)) {
                    dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
                }
            });
    }

    static SetDataDiscountedCommission(data, applicationPremiumItemId) {
        var localAmount = data.Amount;
        var updTempApplicationPremiumComission = {};
        var applicationPremiumId = applicationPremiumItemId;
        if (data.ApplicationPremiumId > 0) {
            applicationPremiumId = data.ApplicationPremiumId;
        }
        if (data.CurrencyCode > 0) {
            localAmount = data.Amount * data.ExchangeRate;
        }
        return updTempApplicationPremiumComission =
            {
                ExchangeRate: data.ExchangeRate,
                Amount: data.Amount,
                LocalAmount: localAmount,
                BaseAmount: data.BaseAmount,
                TempApplicationPremiumId: applicationPremiumId,
                Id: data.Id,
                CurrencyCode: data.CurrencyCode,
                CommissionType: data.CommissionType,
                AgentTypeId: data.AgentTypeCode,
                AgentId: data.AgentIndividualId,
                AgentAgencyId: data.AgentAgencyId
            };


    }

    static actionCreators(getState, dispatch) {

        $("#ModalPremiums").find("#BranchSearchDrop").on('itemSelected', function (event, item) {
            dispatch({ type: 'PREMPAY_SET_BRANCH', selectedItem: item });
        });

        $("#ModalPremiums").find("#PrefixDrop").on('itemSelected', function (event, item) {
            dispatch({ type: 'PREMPAY_SET_PREFIX', selectedItem: item });
        });

        ///Comision descontada
        $("#ModalPremiums").find("#AddCommissionButton").click(function () {
            if (!isEmptyorZero(getState.CommissionDiscountedItem.AgentIndividualId)) {
                dispatch({ type: 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_DISCOUNTED_VAL', DiscountedAmount: $("#ModalPremiums").find("#CommissionTxt").val() });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
                dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
                $("#ModalPremiums").find("#TotalCommissionTxt").text(getState.CommissionDiscountedItem.Amount);
                $("#ModalPremiums").find('#totalCommissionfooter').text(getState.TotalCommissionFoot);
                $("#ModalPremiums").find('#CommissionTxt').val(getState.CommissionDiscountedItem.MaxCommission);

                dispatch({ type: 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_ROW_EMPTY' });
            } else {
                $.UifNotify('show', { type: 'info', message: Resources.ErrorNotSelectedEntity, autoclose: false });
            }
            //

        });

        $("#ModalPremiums").find('#CommissionTxt').on('blur', function (event) {
            var newCommiss = RemoveFormatMoney($("#ModalPremiums").find('#CommissionTxt').val());
            if (DialogSearchPolicies.validateAmount(newCommiss, getState.CommissionDiscountedItem.MaxCommission) === false) {
                $("#ModalPremiums").find('#CommissionTxt').val(getState.CommissionDiscountedItem.MaxCommission);
            }
        });
        ////BOTON LIMPIAR
        $("#ModalPremiums").find("#CleanPoliciesButton").click(function () {
            DialogSearchPolicies.ClearSearchPolicy(getState, dispatch);
            dispatch({ type: 'PREMPAY_CLEAN_SEARCH' });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: false });
        });
        //presiona Aceptar en modificar prima
        $("#ModalPremiums").find("#AcceptDepositPrimeDialog").click(function () {
            $.each(getState.ApplicationPremiumModel.ApplicationPremiumItems, function (i, value) {
                if (value.ApplicationPremiumId == getState.ApplicationPremiumItem.ApplicationPremiumId) {
                    dispatch({ type: 'PREMPAY_ACCEPT_DEPOSIT_PRIMEDIALOG_IS_CHEKED', label: 'isCleaningForm', Data: $("#ModalPremiums").find("#DepositPremiumsTable").UifDataTable('getData') });
                    DialogSearchPolicies.SaveCheckDepositPrime(getState, dispatch);
                    DialogSearchPoliciesRequest.EchangeRateCollect(value.CurrencyCode, getState.ViewModel.AccountingDate, getState.ViewModel.ApplyCollecId).done(function (data) {
                        dispatch({ type: 'PREMPAY_ENCHANGERATE_LOAD', EchangeRateData: data });
                        dispatch({ type: 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT_SAVE', IsDiscountedCommisson: false, index: i });
                    });
                    DialogSearchPoliciesRequest.SaveTempPremiumReceivableRequest(getState.ApplicationPremiumModel).done(function (data) {
                        DialogSearchPolicies.LoadPremiums(getState, dispatch);
                        DialogSearchPolicies.setDataFieldsEmptySearch();
                    });
                }
            });

        });

        //Botón Cancelar
        $("#ModalPremiums").find("#CancelDepositPrimeDialog").click(function () {
            DialogSearchPolicies.setDataFieldsEmptySearch();
            //$("#ModalPremiums").find("#DepositPremiumsDialog").hide();
            $("#ModalPremiums").find("#DepositPremiumsDialog").slideDown("slow");
        });
        //Botón Aceptar en comisiones descontadas
        $("#ModalPremiums").find("#AcceptCommissionDialog").click(function () {

            if (parseFloat(getState.DiscountedCommission) >
                parseFloat(getState.ApplicationPremiumItem.PendingCommission)) {

                var mesj = Resources.CannotInputValueGreatherThan + Resources.DialogCommissionRetainedOutstandingCommission;
                $("#ModalPremiums").find("#alertCommissionRetained").UifAlert('show', mesj, "warning");

                $("#ModalPremiums").find("#ChangeAmountDiscountedCommission").val(0);
            }
            else {
                dispatch({ type: 'PREMPAY_CHANGE_DISCOUNTED_COMMISSION_CHEKED', DiscountedCommisson: parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#ChangeAmountDiscountedCommission").val())) });
                DialogSearchPoliciesRequest.SaveTempPremiumReceivableRequest(getState.ApplicationPremiumModel).done(function (data) {

                    DialogSearchPolicies.LoadPremiums(getState, dispatch);
                    DialogSearchPolicies.setDataFieldsEmptySearch();
                    dispatch({ type: 'PREMPAY_SET_DATAFIELDS_EMPTY_SEARCH', EchangeRateData: data });

                    $("#ModalPremiums").find("#SelecEditPartial").hide();
                    $("#ModalPremiums").find("#CommissionRetainedDialog").hide();
                    $("#ModalPremiums").find("#CommissionRetainedDialog").slideUp("slow");
                });
            }
        });

        $("#ModalPremiums").find("#AddPoliciesButton").click(function () {
            var msj = "";
            lockScreen();
            var newAmount = fix(RemoveFormatMoney($("#ModalPremiums").find("#CashAmount").val()));

            if (getState.ApplicationPremiumItem.PremiumOriginalAmount)
            {
                if (getState.ApplicationPremiumItem.PremiumOriginalAmount > 0) {
                    if (newAmount > getState.ApplicationPremiumItem.PremiumOriginalAmount) {
                        $.UifNotify('show', { type: 'info', message: Resources.YouCanNotEnterValueGreater, autoclose: true });
                        return;
                    }
                }
                else {
                    if (newAmount < getState.ApplicationPremiumItem.PremiumOriginalAmount) {
                        $.UifNotify('show', { type: 'info', message: Resources.YouCanNotEnterValueGreater, autoclose: true });
                        return;
                    }
                }
            }
                if (!isNull(getState.ApplicationPremiumItem.ApplicationPremiumId)) {
                    //DialogSearchPolicies.UpdTempApplicationPremiumComponents
                    var updTempApplicationPremiumComponent =
                    {
                        TempApplicationPremiumCode: getState.ApplicationPremiumItem.ApplicationPremiumId,
                        ComponentCurrencyCode: getState.ApplicationPremiumItem.CurrencyCode,
                        ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate,
                        PremiumAmount: getState.ApplicationPremiumItem.PaymentAmount,
                        ExpensesLocalAmount: getState.ApplicationPremiumItem.Expenses,
                        TaxLocalAmount: getState.ApplicationPremiumItem.Tax,
                    };
                    $.each(getState.DiscountedCommissions, function (index, data) {

                        DialogSearchPoliciesRequest.UpdTempApplicationPremiumCommission(DialogSearchPolicies.SetDataDiscountedCommission(data, getState.ApplicationPremiumItem.ApplicationPremiumId))
                            .done(function (response) {
                                if (response.success == true) {
                                    return 1;
                                } else {
                                    return 0;
                                }
                            })
                            .fail(function () {
                                return 0;
                            });
                    });

                    DialogSearchPoliciesRequest.UpdTempApplicationPremiumComponents(updTempApplicationPremiumComponent)
                        .always(function () {
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataPolicY', value: true });
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataPolicY', value: false });
                            DialogSearchPolicies.ClearSearchPolicy(getState, dispatch);
                            dispatch({ type: 'PREMPAY_CLEAN_SEARCH' });
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: false });

                            DialogSearchPolicies.LoadPremiums(getState, dispatch);
                            DialogSearchPolicies.setDataFieldsEmptySearch();
                        });
    
                } else {

                    dispatch({ type: 'PREMPAY_PREPARE_COMMISSION_DISCOUNT', data: getState.DiscountedCommissions });
                    DialogSearchPolicies.saveTempApplicationPremium(getState, dispatch);
                }
        
        });
        
        $("#ModalPremiums").find("#CancelDescontinuedCommissionButton").click(function () {
            dispatch({ type: 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_ROW_EMPTY' });
        });

        $("#ModalPremiums").find('#GroupNumber').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                dispatch({
                    type: 'PREMPAY_SET_SEARCH_GROUP_NUMBER', ViewModel: {
                        selectedItem: selectedItem, GroupIdControl: "S", groupNumber: $("#ModalPremiums").find("#GroupNumber").val()
                    }
                });
            }
            else {
                dispatch({
                    type: 'PREMPAY_SET_EMPY_SEARCH_GROUP_NUMBER'
                });
            }
        });

        $("#ModalPremiums").find('#GroupName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                dispatch({
                    type: 'PREMPAY_SET_SEARCH_GROUP_NUMBER', ViewModel: {
                        selectedItem: selectedItem, GroupIdControl: "S", groupNumber: $("#ModalPremiums").find("#GroupNumber").val()
                    }
                });
            }
            else {
                dispatch({
                    type: 'PREMPAY_SET_EMPY_SEARCH_GROUP_NUMBER'
                });
            }

        });
        $("#ModalPremiums").find('#PayerDocumentNumber').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                dispatch({
                    type: 'PREMPAY_SET_SEARCH_PAYER_DOCUMENT_NUMBER', ViewModel: {
                        selectedItem: selectedItem, PayerIdControl: "S", PayerDocumentNumber: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                    }
                });
            }
            else {
                dispatch({
                    type: 'PREMPAY_SET_EMPTY_SEARCH_PAYER_DOCUMENT_NUMBER'
                });
            }
        });
        $("#ModalPremiums").find('#PayerName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                dispatch({
                    type: 'PREMPAY_SET_SEARCH_PAYER_DOCUMENT_NUMBER', ViewModel: {
                        selectedItem: selectedItem, PayerIdControl: "S", PayerDocumentNumber: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                    }
                });
            }
            else {
                dispatch({
                    type: 'PREMPAY_SET_EMPTY_SEARCH_PAYER_DOCUMENT_NUMBER'
                });
            }
        });
        $("#ModalPremiums").find('#AgentDocumentNumberDialog').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                dispatch({
                    type: 'PREMPAY_SET_SEARCH_AGENT_DOCUMENT_NUMBER', ViewModel: {
                        selectedItem: selectedItem, agentIdControl: "S", agentDocumentNumber: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                    }
                });
            }
            else {
                dispatch({
                    type: 'PREMPAY_SET_EMPTY_SEARCH_ANGET_DOCUMENT_NUMBER'
                });
            }
        });
        $("#ModalPremiums").find('#searchEndorsementNumber').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != undefined) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                if (selectedItem.Id > 0) {
                    dispatch({ type: 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT_2', EndorsementId: selectedItem.Id, PaymentNumber: -1 });
                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataQuota', value: false });
                    dispatch({
                        type: 'PREMPAY_SET_POLICY_ENDORSEMENT_ID', EndorsementId: selectedItem.Id
                    });
                    $("#ModalPremiums").find("#SearchSearchPoliciesButton").trigger('click');
                }
            }

        });
        $("#ModalPremiums").find('#searchPaymentNumber').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != undefined) {
                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                if (selectedItem.Id > 0) {
                    dispatch({ type: 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT_2', EndorsementId: $("#ModalPremiums").find('#searchEndorsementNumber').val(), PaymentNumber: selectedItem.Id });
                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataQuota', value: false });
                }
            }

        });
        $("#ModalPremiums").find('#selectCashCurrency').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != undefined) {
                if (selectedItem.Id > -1) {
                    DialogSearchPoliciesRequest.GetEchangeRateByCurrencyId(selectedItem.Id).done(function (data) {
                        dispatch({ type: 'PREMPAY_SET_ENCHANGERATE_VALUE', EchangeRateData: data, CurrencyCode: selectedItem.Id });
                    });
                }
            }

        });


        //BLUR        
        //Valida que no ingresen una fecha invalida.
        $("#ModalPremiums").find('#DepositAmountPayingAmount').on('blur', function (event) {
            dispatch({ type: 'PREMPAY_SET_INCOME_AMOUNT', IncomeAmount: RemoveFormatMoney($("#ModalPremiums").find('#DepositAmountPayingAmount').val()) });
        });
        $("#ModalPremiums").find('#GroupNumber').on('blur', function (event) {
            dispatch({
                type: 'PREMPAY_BLUR_SEARCH_GROUP_NUMBER', groupNumber: $("#ModalPremiums").find("#GroupNumber").val()
            });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurGroupData', value: false });
        });
        $("#ModalPremiums").find('#GroupName').on('blur', function (event) {
            dispatch({
                type: 'PREMPAY_BLUR_SEARCH_GROUP_NAME', GroupName: $("#ModalPremiums").find("#GroupName").val()
            });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurGroupData', value: false });

        });
        $("#ModalPremiums").find('#InsuredDocumentNumber').on('blur', function (event) {
            dispatch({
                type: 'PREMPAY_BLUR_SEARCH_INSURED_DOCUMENT_NUMBER', InsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
            });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurInsuredData', value: false });
        });
        $("#ModalPremiums").find('#PayerDocumentNumber').on('blur', function (event) {
            dispatch({
                type: 'PREMPAY_BLUR_SEARCH_PAYER_DOCUMENT_NUMBER', newInsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
            });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurPayerData', value: false });

        });
        $("#ModalPremiums").find('#ChangeAmountDiscountedCommission').on('blur', function (event) {
            dispatch({
                type: 'PREMPAY_BLUR_SEARCH_AGENT_DOCUMENT_NUMBER', newInsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
            });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurPayerData', value: false });
        });
        //Prima
        $("#ModalPremiums").find('#CashAmount').on('blur', function (event) {
            var newAmount = fix(RemoveFormatMoney($("#ModalPremiums").find("#CashAmount").val()));
            if (!DialogSearchPolicies.validatePremiumAmount(newAmount, getState.ApplicationPremiumItem.PremiumOriginalAmount)) {
                newAmount = getState.ApplicationPremiumItem.PremiumOriginalAmount;
            }
            dispatch({ type: 'PREMPAY_BLUR_SET_NEW_AMOUNT', newAmount: newAmount });
            dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'ShowItemApplyViewRowEdit', value: false });

            dispatch({
                type: 'PREMPAY_UPDATE_COMMISSION_DISCOUNT', exchangeRate: getState.ApplicationPremiumItem.ExchangeRate, baseAmount: newAmount
            });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
            //dispatch({ type: 'PREMPAY_SET_DATA_DISCOUNTED_COMMISSION', Data: getState.DiscountedCommissions, CurrencyCode: getState.ApplicationPremiumItem.CurrencyCode, CurrencyDescription: getState.ApplicationPremiumItem.CurrencyDescription, ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate }); dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });

            $("#ModalPremiums").find('#totalCommissionfooter').text(getState.TotalCommissionFoot);
        });
        $("#ModalPremiums").find('#CashAmount').on('focus', function (event) {
            $("#ModalPremiums").find("#CashAmount").val(RemoveFormatMoney($("#ModalPremiums").find("#CashAmount").val()));
        });
        //Impuestos
        $("#ModalPremiums").find('#Tax_txt').on('blur', function (event) {
            var newTax = RemoveFormatMoney($("#ModalPremiums").find("#Tax_txt").val());

            if (DialogSearchPolicies.validateAmount(newTax, getState.ApplicationPremiumItem.TaxOriginal)) {
                dispatch({ type: 'PREMPAY_BLUR_SET_NEW_TAX', newTax: newTax });
                dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
                $("#alertPolicyView").UifAlert('hide');
            }
            else {
                $("#ModalPremiums").find("#Tax_txt").val(getState.ApplicationPremiumItem.Tax);
                $("#alertPolicyView").UifAlert('show', Resources.YouCanNotEnterValueGreater, "warning");
            }
            $("#ModalPremiums").find("#Tax_txt").val(FormatMoneySymbol($("#ModalPremiums").find("#Tax_txt").val()));
        });
        $("#ModalPremiums").find('#Tax_txt').on('focus', function (event) {
            $("#ModalPremiums").find("#Tax_txt").val(RemoveFormatMoney($("#ModalPremiums").find("#Tax_txt").val()));
        });

        //Tasa
        $("#ModalPremiums").find('#CashExchangeRate').on('blur', function (event) {
            $("#alertPolicyView").UifAlert('hide');
            var newExchageRate = fix(RemoveFormatMoney($("#ModalPremiums").find("#CashExchangeRate").val()));
            DialogSearchPoliciesRequest.CalculateExchangeRateTolerance(newExchageRate, getState.ApplicationPremiumItem.CurrencyCode).done(function (result) {
                if (result) {
                    dispatch({ type: 'PREMPAY_BLUR_SET_NEW_EXCHANGE_RATE', newExchangeRate: newExchageRate });
                    dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });

                    dispatch({
                        type: 'PREMPAY_UPDATE_COMMISSION_DISCOUNT', exchangeRate: newExchageRate, baseAmount: getState.ApplicationPremiumItem.PaymentAmount
                    });
                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
                }
                else {
                    $("#ModalPremiums").find("#CashExchangeRate").val(getState.ApplicationPremiumItem.ExchangeRate);
                    $("#alertPolicyView").UifAlert('show', Resources.ValueIsNotRangeTolerance, "warning");
                }
            });
        });
        $("#ModalPremiums").find('#SearchPolicyNumber').on('blur', function (event) {
            if (!DialogSearchPolicies.isNull($("#SearchPolicyNumber").val())) {
                dispatch({
                    type: 'PREMPAY_SET_POLICY_DOCUMENT_NUMBER', PolicyDocumentNumber: $("#SearchPolicyNumber").val()
                });
                $("#ModalPremiums").find("#SearchSearchPoliciesButton").trigger('click');
                $("#ModalPremiums").find("#AddPoliciesButton").focus();
            }
        });

        $("#ModalPremiums").find('#ItemToApplyView').on('rowEdit', function (event, data, index) {
            if (!data.IsReversion) {
                $("#ModalPremiums").find("#alertCommissionInfo").UifAlert('hide');
                DialogSearchPoliciesRequest.GetTempApplicationPremiumComponentsByTemApp(data.ApplicationPremiumId).done(function (dataTempApplicationPremiumComponents) {
                    DialogSearchPoliciesRequest.GetPremiumComponentsByEndorsementIdQuotaNumber(data.EndorsementId, data.QuotaNumber).done(function (dataPremiumComponents) {
                        dispatch({ type: 'PREMPAY_ITEM_APPLY_VIEW_ROW_EDIT', Data: data, DataTempApplicationPremiumComponents: dataTempApplicationPremiumComponents, DataPremiumComponents: dataPremiumComponents.result, Index: index });
                        DialogSearchPolicies.SetExchangeRate(getState, dispatch);
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'ShowItemApplyViewRowEdit', value: false });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'DisabledCheckNoExpenses', value: false });

                        DialogSearchPoliciesRequest.SearchDepositPremiums(data.PayerId).done(function (data) {
                            if (data.aaData.length > 0) {
                                if (getState.DepositPremiumModel.UsedAmount != null) {
                                    dispatch({ type: 'PREMPAY_SET_DATA_DEPOSIT_PREMIUM', Data: getState.DepositPremiumModel });
                                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: true });
                                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: false });
                                }
                                else {
                                    dispatch({ type: 'PREMPAY_SET_DATA_DEPOSIT_PREMIUM', Data: data.aaData });
                                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: true });
                                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: false });
                                }
                            }
                        });
                    });
                    var policyNum = data.PolicyId;
                    var endorsementNum = data.EndorsementId;
                    DialogSearchPoliciesRequest.SearhTempDiscountedCommission(policyNum, endorsementNum, data.ApplicationPremiumId).done(function (data) {
                        if (data.length > 0) {
                            dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
                            dispatch({ type: 'PREMPAY_SET_DATA_DISCOUNTED_COMMISSION', Data: data, CurrencyCode: getState.ApplicationPremiumItem.CurrencyCode, CurrencyDescription: getState.ApplicationPremiumItem.CurrencyDescription, ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate });

                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
                            $("#ModalPremiums").find('#totalCommissionfooter').text(getState.TotalCommissionFoot);
                        }
                        else {
                            DialogSearchPoliciesRequest.SearchDisconutedCommission(policyNum, endorsementNum).done(function (data) {
                                if (data.length > 0) {
                                    dispatch({ type: 'PREMPAY_SET_DATA_DISCOUNTED_COMMISSION', Data: data, CurrencyCode: getState.ApplicationPremiumItem.CurrencyCode, CurrencyDescription: getState.ApplicationPremiumItem.CurrencyDescription, ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate });
                                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
                                }
                            });
                        }
                    });
                });
            }
        });
        $("#ModalPremiums").find('#DepositPremiumsTable').on('rowEdit', function (event, data, position) {
            dispatch({ type: 'PREMPAY_DPOSIT_PREMIUM_VIEW_ROW_EDIT', Data: data, Position: position });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsDepositPremiumRowEdit', value: false });
        });
        $("#ModalPremiums").find("#DiscountedCommissionTable").on('rowSelected', function (event, data, position) {
            dispatch({ type: 'PREMPAY_DISCOUNTED_COMMISSION_EDIT_ROW', Data: data });

            $("#ModalPremiums").find('#CommissionTxt').val(getState.CommissionDiscountedItem.MaxCommission);


        });


        //radio click
        //selecciona radio importe a pagar
        $("#ModalPremiums").find('#AmountToPayCheck').on('click', function (event, selectedItem) {
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'AmountToPayCheckClick', value: true });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'AmountToPayCheckClick', value: false });
        });
        //selecciona radio comisión descontada
        $("#ModalPremiums").find('#CommissionCheck').on('click', function (event, selectedItem) {
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'CommissionCheckClick', value: true });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'CommissionCheckClick', value: false });
        });
        //Check click
        //Click en Utilizar Primas
        $("#ModalPremiums").find("#DepositAmountUsePaymentBalanceCheck").click(function () {

            if ($("#ModalPremiums").find("#DepositAmountUsePaymentBalanceCheck").is(":checked")) {
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsDepositAmountUsePaymentBalanceCheck', value: true });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsDepositAmountUsePaymentBalanceCheck', value: false });
                $.each(getState.ApplicationPremiumModel.ApplicationPremiumItems, function (i, value) {
                    if (value.ApplicationPremiumId == getState.ApplicationPremiumItem.ApplicationPremiumId) {
                        DialogSearchPoliciesRequest.GetDepositPremiumTransactionByPayerId(value.PayerId).done(function (data) {
                            $("#ModalPremiums").find("#DepositPremiumsTable").UifDataTable({ sourceData: data.aaData });
                        });
                    }
                });


            } else {

                DialogSearchPolicies.confirmUncheckDepositPrimeUse(getState, dispatch);

            }
        });



        $("#ModalPremiums").find('#editAction').on('Save', function () {

            $("#ModalPremiums").find("#alertDepositPremiumsDialog").UifAlert('hide');

            if (parseFloat(RemoveFormatMoney(getState.Data.DepositPremiumRowEdit.UsedAmount) > parseFloat(RemoveFormatMoney(getState.Data.DepositPremiumRowEdit.TotalAmount)))
            ) {

                $("#ModalPremiums").find("#alertDepositPremiumsDialog").UifAlert('show', Resources.PaymentExcessValidation, "warning");
            }
            else if (parseFloat(RemoveFormatMoney(getState.Data.DepositPremiumRowEdit.UsedAmount)) > parseFloat(getState.ApplicationPremiumModel.ApplicationPremiumItems[0].PayableAmount)) {

                $("#ModalPremiums").find("#alertDepositPremiumsDialog").UifAlert('show', Resources.PaymentBalanceExcessValidation, "warning");
            }
            else {
                dispatch({ type: 'PREMPAY_ACTION_VIEW_SAVE_CLICK', UsedAmount: FormatCurrency(FormatDecimal($("#ModalPremiums").find("#UsedAmountForm").val())) });

                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'OnActionSaveClick', value: false });
            }
        });

        $("#ModalPremiums").find('#editAction').on('Next', function () {
            $("#ModalPremiums").find('#DepositPremiumsTable').UifDataTable("next");
        });

        $("#ModalPremiums").find('#editAction').on('Previous', function () {
            $("#ModalPremiums").find('#DepositPremiumsTable').UifDataTable("previous");
        });
        //delete listView
        $("#ModalPremiums").find('#ItemToApplyView').on('rowDelete', function (event, data, index) {
            if (isNull(getState.ApplicationPremiumItem.ApplicationPremiumId)) {
                DialogSearchPolicies.DeletePremiumListView(data, getState, dispatch);
            } else {
                $.UifNotify('show', { type: 'info', message: Resources.PolicyIsEditing, autoclose: false });
            }

        });
        $("#ModalPremiums").find("#checkNoExpenses").click(function () {
            dispatch({ type: 'PREMPAY_SET_CHECK_EXPENSES', isChecked: $('#checkNoExpenses').is(':checked') });
            dispatch({ type: 'PREMPAY_SET_TOTAL_PREMIUM' });
        });
        $("#ModalPremiums").find("#btnSearchAdvPolicy").click(function () {
            DialogSearchPolicies.ShowSearchAdvPolicy(getState, dispatch);
        });
        $("#ModalPremiums").find("#policyIdTxt").on('keypress', function (event) {
            if (event.keyCode === 13) {
                $("#ModalPremiums").find("#SearchSearchPoliciesButton").trigger('click');
                $("#ModalPremiums").find("#AddPoliciesButton").focus();
            }
        });
        $("#ModalPremiums").find('#policyIdTxt').on('blur', function (event) {
            dispatch({
                type: 'PREMPAY_SET_POLICY_ID', PolicyId: $("#policyIdTxt").val()
            });
        });
        ///Click buscar
        $("#ModalPremiums").find("#policyIdTxt").on('buttonClick', function (event) {
            DialogSearchPolicies.ClearSearchPolicy(getState, dispatch);
            DialogSearchPolicies.searchPolicy(getState, dispatch);
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: true });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: false });
        });
        $("#ModalPremiums").find("#SearchSearchPoliciesButton").click(function () {
            $("#ModalPremiums").find("#alertCommissionInfo").UifAlert('hide');
            //DialogSearchPolicies.ClearSearchPolicy(getState, dispatch);
            DialogSearchPolicies.searchPolicy(getState, dispatch);
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: true });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: false });
        });

        //exportar  A excel
        $("#ModalPremiums").find("#ExportPolicyInformation").click(function () {
            if (!isEmptyArray(getState.ApplicationPremiumModel.ApplicationPremiumItems)) {
                lockScreen();
                DialogSearchPoliciesRequest.showReportExcelPolicyPayments(getState.ViewModel.TempImputationId).done(function (data) {
                    if (!isNull(data)) {
                        DownloadFile(data, true, function () {
                            unlockScreen();
                            return "excel.xls";
                        });
                    } else {
                        $.UifNotify('show', { type: 'info', message: data, autoclose: true });
                    }
                }).always(function (data) {
                    unlockScreen();
                });
            }            
        });
        

        $(document).ready(function () {
            var dropDownSearchAdvPolicy;
            $("#ModalPremiums").find("#ItemToApplyView").UifListView({
                autoHeight: true,
                theme: 'dark',
                customDelete: true,
                customEdit: true,
                edit: true,
                delete: true,
                displayTemplate: "#ItemToApplyViewTemplate"
            });
            dropDownSearchAdvPolicy = uif2.dropDown({
                source: ACC_ROOT + '/AdvancedSearchPolicies/AdvancedSearchPolicies',
                element: '#documentSearch',
                container: '#ModalPremiums',
                align: 'left',
                width: 1000,
                height: 630,
                loadedCallback: function () {
                    $("#ModalPremiums").find('#SearchBy').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem != undefined) {
                            if (selectedItem.Id > 0) {
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataSearchBy', value: false });
                                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: true });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: false });
                                dispatch({ type: 'PREMPAY_SET_SEARCH_TYPE', selectedItemSearchBy: selectedItem });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadSearchType', value: true });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadSearchType', value: false });

                            }
                            else {
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataSearchBy', value: false }); dispatch({ type: 'PREMPAY_SET_SEARCH_TYPE', selectedItemSearchBy: selectedItem });
                                dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: true });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: false });
                            }
                        }

                    });
                    $('#InsuredDocumentNumberSearch').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_INSURED_DOCUMENT_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, insuredIdControl: "S", insuredName: $("#ModalPremiums").find("#InsuredDocumentNumberSearch").val()
                                }
                            });
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'ShowInsuredDataSearch', value: false });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPTY_SEARCH_INSURED_DOCUMENT_NUMBER'
                            });
                        }
                    });
                    $('#InsuredNameSearch').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_INSURED_DOCUMENT_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, insuredIdControl: "S", insuredDocument: $("#ModalPremiums").find("#InsuredDocumentNumberSearch").val()
                                }
                            });
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'ShowInsuredDataSearch', value: false });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPTY_SEARCH_INSURED_DOCUMENT_NUMBER'
                            });
                        }
                    });
                    $("#ModalPremiums").find('#GroupNumber').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_GROUP_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, GroupIdControl: "S", groupNumber: $("#ModalPremiums").find("#GroupNumber").val()
                                }
                            });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPY_SEARCH_GROUP_NUMBER'
                            });
                        }
                    });
                    $("#ModalPremiums").find('#GroupName').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_GROUP_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, GroupIdControl: "S", groupNumber: $("#ModalPremiums").find("#GroupNumber").val()
                                }
                            });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPY_SEARCH_GROUP_NUMBER'
                            });
                        }

                    });
                    $("#ModalPremiums").find('#PayerDocumentNumber').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_PAYER_DOCUMENT_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, PayerIdControl: "S", PayerDocumentNumber: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                                }
                            });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPTY_SEARCH_PAYER_DOCUMENT_NUMBER'
                            });
                        }
                    });
                    $("#ModalPremiums").find('#PayerName').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_PAYER_DOCUMENT_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, PayerIdControl: "S", PayerDocumentNumber: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                                }
                            });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPTY_SEARCH_PAYER_DOCUMENT_NUMBER'
                            });
                        }
                    });
                    $("#ModalPremiums").find('#AgentDocumentNumberDialog').on('itemSelected', function (event, selectedItem) {
                        if (selectedItem.Id > 0) {
                            dispatch({
                                type: 'PREMPAY_SET_SEARCH_AGENT_DOCUMENT_NUMBER', ViewModel: {
                                    selectedItem: selectedItem, agentIdControl: "S", agentDocumentNumber: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                                }
                            });
                        }
                        else {
                            dispatch({
                                type: 'PREMPAY_SET_EMPTY_SEARCH_PAYER_DOCUMENT_NUMBER'
                            });
                        }
                    });
                    $("#ModalPremiums").find("#BranchSearch").on('itemSelected', function (event, item) {
                        dispatch({ type: 'PREMPAY_SET_BRANCH', selectedItem: item });
                    });
                    $("#ModalPremiums").find("#PrefixSearch").on('itemSelected', function (event, item) {
                        dispatch({ type: 'PREMPAY_SET_PREFIX', selectedItem: item });
                    });
                    //BLUR
                    //Valida que no ingresen una fecha invalida.
                    $("#ModalPremiums").find('#DepositAmountPayingAmount').on('blur', function (event) {
                        dispatch({ type: 'PREMPAY_SET_INCOME_AMOUNT', IncomeAmount: RemoveFormatMoney($("#ModalPremiums").find('#DepositAmountPayingAmount').val()) });
                    });
                    $("#ModalPremiums").find("#PayExpDateQuotaFrom").on('blur', function () {
                        dispatch({
                            type: 'PREMPAY_BLUR_DATE_QUOTA_FROM', dateFrom: $("#ModalPremiums").find("#PayExpDateQuotaFrom").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurDateQuotaFrom', value: false });
                    });
                    $("#ModalPremiums").find("#PayExpDateQuotaTo").on('blur', function () {
                        dispatch({
                            type: 'PREMPAY_BLUR_DATE_QUOTA_TO', dateTo: $("#ModalPremiums").find("#PayExpDateQuotaTo").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurDateQuotaTo', value: false });
                    });
                    $("#ModalPremiums").find('#GroupNumber').on('blur', function (event) {
                        dispatch({
                            type: 'PREMPAY_BLUR_SEARCH_GROUP_NUMBER', groupNumber: $("#ModalPremiums").find("#GroupNumber").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurGroupData', value: false });
                    });
                    $("#ModalPremiums").find('#GroupName').on('blur', function (event) {
                        dispatch({
                            type: 'PREMPAY_BLUR_SEARCH_GROUP_NAME', GroupName: $("#ModalPremiums").find("#GroupName").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurGroupData', value: false });

                    });
                    $("#ModalPremiums").find('#InsuredDocumentNumber').on('blur', function (event) {
                        dispatch({
                            type: 'PREMPAY_BLUR_SEARCH_INSURED_DOCUMENT_NUMBER', InsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurInsuredData', value: false });
                    });
                    $("#ModalPremiums").find('#PayerDocumentNumber').on('blur', function (event) {
                        dispatch({
                            type: 'PREMPAY_BLUR_SEARCH_PAYER_DOCUMENT_NUMBER', newInsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurPayerData', value: false });

                    });
                    $("#ModalPremiums").find('#AgentDocumentNumberDialog').on('blur', function (event) {
                        dispatch({
                            type: 'PREMPAY_BLUR_SEARCH_AGENT_DOCUMENT_NUMBER', newInsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurPayerData', value: false });
                    });
                    $("#ModalPremiums").find('#ChangeAmountDiscountedCommission').on('blur', function (event) {
                        dispatch({
                            type: 'PREMPAY_BLUR_SEARCH_AGENT_DOCUMENT_NUMBER', newInsuredId: $("#ModalPremiums").find("#PayerDocumentNumber").val()
                        });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'onBlurPayerData', value: false });
                    });

                    $("#ModalPremiums").find('#PolicyDocumentNumber').on('blur', function (event) {
                        if (!DialogSearchPolicies.isNull($("#PolicyDocumentNumber").val())) {
                            dispatch({
                                type: 'PREMPAY_SET_POLICY_DOCUMENT_NUMBER', PolicyDocumentNumber: $("#PolicyDocumentNumber").val()
                            });
                        }
                    });
                    $("#ModalPremiums").find('#Endorsement').on('blur', function (event) {

                        dispatch({
                            type: 'PREMPAY_SET_POLICY_ENDORSEMENT_NUMBER', EndorsementNumber: $("#Endorsement").val()
                        });

                    });

                    //BOTON BUSCAR
                    $("#ModalPremiums").find("#AdvancedSearchButton").click(function () {
                        dispatch({
                            type: 'PREMPAY_CREATE_SRC'
                        });
                        DialogSearchPoliciesRequest.GetPolicyPaymentBySearch(getState.ViewSearchModel)
                            .done(function (data) {
                                dispatch({ type: 'PREMPAY_SET_SEARCH_POLICIES_DATA', Data: data });
                            });

                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isValidatingForm', value: true });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isValidatingForm', value: false });
                    });
                    //BOTON ACEPTAR
                    $("#ModalPremiums").find("#AcceptButtonSearch").click(function () {
                        if ($("#ModalPremiums").find("#ItemToPayView").UifListView("getSelected").length > 0) {
                            $.each($("#ModalPremiums").find("#ItemToPayView").UifListView("getSelected"), function (index, data) {
                                var msj = "";
                                lockScreen();
                                dispatch({ type: 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT', Data: data });
                                DialogSearchPolicies.GetExchangeRate(getState, dispatch);
                                DialogSearchPolicies.GetPremiumComponentsByEndorsementIdQuotaNumber(getState, dispatch);
                                DialogSearchPolicies.saveTempApplicationPremium(getState, dispatch);
                            });
                        }
                    });

                    $("#ModalPremiums").find("#ItemToPayView").UifListView({
                        autoHeight: true,
                        theme: 'dark',
                        customDelete: true,
                        customEdit: true,
                        edit: true,
                        delete: true,
                        selectionType: "multiple",
                        displayTemplate: "#ItemToPayViewTemplate"
                    });
                    //BOTON LIMPIAR
                    $("#ModalPremiums").find("#CleanSearchPoliciesButton").click(function () {
                        DialogSearchPolicies.ClearSearchPolicy(getState, dispatch);
                        dispatch({ type: 'PREMPAY_CLEAN_SEARCH' });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: false });
                    });
                    //EDIT ITEM
                    $("#ModalPremiums").find('#ItemToPayView').on('rowEdit', function (event, data, index) {
                        dispatch({ type: 'PREMPAY_ITEM_PAY_VIEW_ROW_EDIT', Data: data });
                        DialogSearchPolicies.GetExchangeRate(getState, dispatch);
                        DialogSearchPolicies.GetPremiumComponentsByEndorsementIdQuotaNumber(getState, dispatch);
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataEditItem', value: true });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataEditItem', value: false });
                        DialogSearchPoliciesRequest.SearchDepositPremiums(getState.ApplicationPremiumItem.PayerId).done(function (data) {
                            dispatch({ type: 'PREMPAY_SET_DATA_DEPOSIT_PREMIUM', Data: data });
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: true });
                            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: false });
                        });
                        DialogSearchPoliciesRequest.SearchDisconutedCommission(data.PolicyId, data.EndorsementId).done(function (data) {
                            if (data.length > 0) {
                                dispatch({ type: 'PREMPAY_SET_DATA_DISCOUNTED_COMMISSION', Data: data, CurrencyCode: getState.ApplicationPremiumItem.CurrencyCode, CurrencyDescription: getState.ApplicationPremiumItem.CurrencyDescription, ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
                            }
                        });
                        dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: true });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: false });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetSerchFieldsEmpty', value: true });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetSerchFieldsEmpty', value: false });
                    });

                }
            });
            dispatch({ type: 'PREMPAY_SET_DROPDOWN_SEARCH', dropDown: dropDownSearchAdvPolicy });
            DialogSearchPoliciesRequest.GetLoadSearchByForPolicies().done(function (data) {
                dispatch({ type: 'PREMPAY_ONINIT', Bags: { searchType: data.data } });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadSearchType', value: true });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadSearchType', value: false });
                dispatch({ type: 'PREMPAY_OFFINIT' });
            });
            DialogSearchPoliciesRequest.GetLoadBranch().done(function (data) {
                dispatch({ type: 'PREMPAY_ONINIT', Bags: { branches: data.data } });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadBranches', value: true });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadBranches', value: false });
                dispatch({ type: 'PREMPAY_OFFINIT' });
            });
            DialogSearchPoliciesRequest.GetLoadPrefix().done(function (data) {
                dispatch({ type: 'PREMPAY_ONINIT', Bags: { prefixes: data.data } });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadPrefixes', value: true });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadPrefixes', value: false });
                dispatch({ type: 'PREMPAY_OFFINIT' });
            });
            DialogSearchPolicies.labelHide();
            dispatch({ type: 'PREMPAY_INIT', ViewModel: { applyCollecId: $("#ViewBagApplyCollecId").val(), AccountingDate: $("#ViewBagDateAccounting").val() } });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'OnInit', value: false });
            DialogSearchPolicies.LoadPremiums(getState, dispatch);

            $("#ModalPremiums").find("#DepositAmountUsePaymentBalanceCheck").removeAttr("disabled");
            $("#ModalPremiums").find("#CommissionRetainedDialog").slideUp("slow");


            $("#ModalPremiums").find("#SelecEditPartial").slideUp("slow");

        });

    }

    static searchPolicy(getState, dispatch) {


        dispatch({
            type: 'PREMPAY_CREATE_SRC'
        });
        DialogSearchPoliciesRequest.GetPolicyPaymentBySearch(getState.ViewSearchModel).done(function (data) {

            if (data.length > 0) {
                DialogSearchPolicies.ClearSearchPolicy(getState, dispatch);
                dispatch({ type: 'PREMPAY_SEARCH_BUTTON_CLICK', Data: data });
                DialogSearchPolicies.GetExchangeRate(getState, dispatch);
                DialogSearchPolicies.GetPremiumComponentsByEndorsementIdQuotaNumber(getState, dispatch);
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataEditItem', value: true });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataEditItem', value: false });
                DialogSearchPoliciesRequest.SearchDepositPremiums(getState.ApplicationPremiumItem.PayerId).done(function (data) {
                    if (data.length > 0) {
                        dispatch({ type: 'PREMPAY_SET_DATA_DEPOSIT_PREMIUM', Data: data });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: true });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDepositPremiums', value: false });
                    }
                });
                DialogSearchPoliciesRequest.SearchDisconutedCommission(data[0].PolicyId, data[0].EndorsementId).done(function (data) {
                    if (data.length > 0) {
                        dispatch({ type: 'PREMPAY_SET_DATA_DISCOUNTED_COMMISSION', Data: data, CurrencyCode: getState.ApplicationPremiumItem.CurrencyCode, CurrencyDescription: getState.ApplicationPremiumItem.CurrencyDescription, ExchangeRate: getState.ApplicationPremiumItem.ExchangeRate });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataDiscountedCommissions', value: false });
                    }
                });
            }
        });


    }

    static GetPremiumComponentsByEndorsementIdQuotaNumber(state, dispatch) {
        DialogSearchPoliciesRequest.GetPremiumComponentsByEndorsementIdQuotaNumber(state.ApplicationPremiumItem.EndorsementId, state.ApplicationPremiumItem.PaymentNumber).done(function (data) {
            dispatch({ type: 'PREMPAY_SET_COMPONENTS_VALUES_SEARCH', Data: state.ApplicationPremiumItem, DataPremiumComponents: data.result });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'DisabledCheckNoExpenses', value: false });

        });
    }

    static ShowSearchAdvPolicy(state, dispatch) {
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadSearchType', value: true });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'loadSearchType', value: false });
        if (!isNull($("#CollectionTo").val())) {
            if ($("#CollectionTo").val() == $("#ViewBagInsuredCode").val() && !isEmpty($("#inputInsuredDocumentNumber").UifAutoComplete('getValue'))) {
                dispatch({ type: 'PREMPAY_SET_VALUES_SEARCH_ADV', searchBy: 1, documentId: $("#inputInsuredDocumentNumber").UifAutoComplete('getValue'), documentName: $("#inputInsuredName").UifAutoComplete('getValue') });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataSearchBy', value: false });
                $("#ModalPremiums").find("#AdvancedSearchButton").trigger('click');
            } else if ($("#CollectionTo").val() == $("#ViewBagAgentCode").val() && !isEmpty($("#inputAgentDocumentNumber").UifAutoComplete('getValue'))) {
                dispatch({ type: 'PREMPAY_SET_VALUES_SEARCH_ADV', searchBy: 3, documentId: $("#inputAgentDocumentNumber").UifAutoComplete('getValue'), documentName: $("#inputAgentName").UifAutoComplete('getValue') });
                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetDataSearchBy', value: false });
                $("#ModalPremiums").find("#AdvancedSearchButton").trigger('click');
            }

        }

        state.dropDownSearchAdv.show();
    }

    //Limpiar Campos
    static ClearSearchPolicy(state, dispatch) {
        dispatch({ type: 'PREMPAY_CLEAN_SEARCH_POLICY' });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: true });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'IsClearAdvancedSearchField', value: false });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetSerchFieldsEmpty', value: true });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'SetSerchFieldsEmpty', value: false });

        dispatch({ type: 'PREMPAY_CLEAN' });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningForm', value: true });
        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningForm', value: false });
    }

    //Controla el orden en que se eliminan las cuotas ya agregadas
    static DeleteQuotaOrderControlPremiums(branchPrefixPolicyEndorsement, quotaSelect) {

        var result = false;
        var chkErr = 0;
        var ids = $("#ModalPremiums").find("#ItemToApplyView").UifListView("getData");//pólizas ya seleccionadas

        for (var k in ids) {

            var rowData = ids[k];

            //Establecer control para evitar desbordamiento
            if (!(parseInt(k) < ids.length)) {
                break;
            }

            if (rowData.BranchPrefixPolicyEndorsement == branchPrefixPolicyEndorsement) {

                if (quotaSelect < rowData.PaymentNumber) {
                    chkErr += 1;
                }
            }
        }
        if (chkErr > 0) {

            result = false;
        } else {
            result = true;
        }

        return result;
    }

    static DeletePremiumListView(data, getState, dispatch) {

        //Valida el orden en que se eliminan las cuotas
        var resultQuota = false;
        if (!data.IsReversion) {
            resultQuota = DialogSearchPolicies.DeleteQuotaOrderControlPremiums(data.BranchPrefixPolicyEndorsement, data.PaymentNumber);
        }
        else {
            resultQuota = true;
        }
        dispatch({ type: 'PREMPAY_SET_PAYMENTNUMBER_VALUE', value: data.PaymentNumber });
        if (resultQuota) {
            DialogSearchPoliciesRequest.DeleteTempPremiumRecievableTransactionItem(getState.ViewModel.TempImputationId, parseInt(data.ApplicationPremiumId), data.IsReversion).done(function (data) {
                if (!data) {
                    $("#ModalPremiums").find("#alertPrimeApply").UifAlert('show', Resources.PaymentItemPartialQuotaDeleteNotAllowed, "warning");
                }
                else {
                    DialogSearchPolicies.LoadPremiums(getState, dispatch);
                    DialogSearchPolicies.setDataFieldsEmptySearch();
                }
            });
        } else {
            $("#ModalPremiums").find("#alertPrimeApply").UifAlert('show', Resources.QuotaSequenceDelete, "warning");
        }
    }

    //Controla el orden en que se añaden las cuotas
    static QuotaOrderControlPremiums(branchPrefixPolicyEndorsement, quotaSelect) {
        var result = false;
        var chkErr = 0;
        var ids = $("#ItemToPayView").UifListView("getData");
        var toApplyIds = $("#ItemToApplyView").UifListView("getData");
        var isApply = false;

        for (var k in ids) {

            var rowData = ids[k];

            //Establecer control para evitar desbordamiento
            if (!(k < ids.length - 1)) {

                break;
            }

            var key = rowData.BranchPrefixPolicyEndorsement;

            if (key == branchPrefixPolicyEndorsement) {

                //validar si ya esta seleccionada ItemToApplyView
                for (var q in toApplyIds) {

                    var rowApply = toApplyIds[q];

                    if (key == rowApply.BranchPrefixPolicyEndorsement) {

                        if ((quotaSelect - rowApply.PaymentNumber) == 1) {
                            isApply = true;
                        } else {
                            isApply = false;
                        }
                    }
                }

                if (!isApply) {
                    if (rowData.PaymentNumber < quotaSelect) {
                        chkErr += 1;
                    }
                }
            }
        }
        if (chkErr > 0) {

            result = false;
        } else {
            result = true;
        }

        return result;
    }

    static saveTempPremium(getState, dispatch) {
        DialogSearchPoliciesRequest.SaveTempPremiumReceivableRequest(getState.ApplicationPremiumModel).done(function () {
            DialogSearchPolicies.LoadPremiums(getState, dispatch);
        });
    }

    static saveTemporalPremium(getState, dispatch) {
        return DialogSearchPoliciesRequest.SaveTempPremiumReceivableRequest(getState.ApplicationPremiumModel);
    }

    static SaveTempPremiumComponentReceivableRequest(getState) {
        return DialogSearchPoliciesRequest.SaveTempPremiumComponentReceivableRequest(getState.ApplicationPremiumItem, getState.DiscountedCommissions, getState.ViewModel.TempImputationId);
    }

    static LoadPremiumPayments(getState, dispatch) {

        DialogSearchPoliciesRequest.GetTempPremiumReceivableItemByTempImputationId(getState.ViewModel.TempImputationId).done(function (data) {//todo:
            var totalPremiumWithCommission = 0;
            var totalPremium = 0;
            totalPremium = JSLINQ(data).Sum(function (el) {
                return el.ValueToCollect;
            });
            var totalPremiumCommison = JSLINQ(data).Sum(function (el) {
                return el.DiscountedCommission;
            });
            for (var i = 0; i < data.length; i++) {
                data[i].ValueToCollect = FormatMoneySymbol(data[i].ValueToCollect);
            }
            totalPremium = totalPremium - totalPremiumCommison;
            dispatch({ type: 'PREMPAY_LOAD_PREMIUMS', Premiums: data, TotalPremium: totalPremium });
            dispatch({ type: 'PREMPAY_SET_VALUE', label: 'LoadPremiums', value: false });
        });
    }

    static LoadPremiums(getState, dispatch) {
        if (!DialogSearchPolicies.isNull(getState.ViewModel.TempImputationId) && getState.ViewModel.TempImputationId != 0) {
            DialogSearchPolicies.LoadPremiumPayments(getState, dispatch);
        }
    }

    static SaveCheckDepositPrime(getState, dispatch) {
        if ($("#ModalPremiums").find("#DepositAmountPayingAmount").val() != "") {
            var amount = parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPaymentBalance").val()));
            var payingAmount = parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPayingAmount").val()));

            if (DialogSearchPolicies.totalUsedAmount() != false || DialogSearchPolicies.totalUsedAmount() >= 0) {
                if ((payingAmount) <= amount) {
                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "PremiumsReceivable/SaveTempDepositPrime",
                        data: JSON.stringify({ "usedDepositPremiumModel": getState.UsedDepositPremiumModel }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    }).done(function () {
                        DialogSearchPolicies.setDataFieldsEmptySearch();

                        $("#ModalPremiums").find("#DepositPremiumsDialog").slideUp("slow");
                        DialogSearchPolicies.LoadPremiums(getState, dispatch)
                    });
                } else {
                    var mesj = Resources.AmountToPay + " " + " " + Resources.MightNotBeGreaterThan + " " + Resources.DialogDepositPremiumsFeeBalance;
                    $("#ModalPremiums").find("#alertDepositPremiumsDialog").UifAlert('show', mesj, "warning");
                    $("#ModalPremiums").find("#DepositAmountPayingAmount").val("");
                    return;
                }
            }

        } else {
            var msj = Resource.ValidationSave + " " + Resources.AmountToPay;
            $("#ModalPremiums").find("#alertDepositPremiumsDialog").UifAlert('show', msj, "warning");
        }
    }

    static confirmUncheckDepositPrimeUse(state, dispatch) {
        $.UifDialog('confirm', {
            'message': Resources.DialogMovementPrimeDepositConfirmationMessage,
            'Resources.PremiumDepositLabel': 'Resources.DialogMovementPrimeDepositConfirmationMessage'
        }, function (result) {
            if (result) {
                $.each(state.ApplicationPremiumModel.ApplicationPremiumItems, function (i, value) {
                    if (state.ApplicationPremiumItem.ApplicationPremiumId == value.ApplicationPremiumId) {
                        DialogSearchPoliciesRequest.DeleteTempUPD(parseInt(value.ApplicationPremiumId));
                        dispatch({ type: 'PREMPAY_DELETE_ITEM_BALANCE_DAUP', label: 'isDeleteDAUPaymentBalanceCheck', value: true });
                        dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isDeleteDAUPaymentBalanceCheck', value: false });
                    }
                });

            }
        });
    }

    static saveTempApplicationPremium(state, dispatch) {
        //Valida si la poliza esta Creada correctamente con los componentes necesarios
        DialogSearchPoliciesRequest.ValidatePolicyComponents(state.ApplicationPremiumItem.PolicyId, state.ApplicationPremiumItem.EndorsementId)
            .done(function (dataResult) {
                if (dataResult) {
                    //valida si existe el registro en tabla temporal o real  por endoso numero de cuota y pagador
                    DialogSearchPoliciesRequest.GetTemporalApplicationByEndorsementIdPaymentNumber(state.ViewModel.TempImputationId,
                        state.ApplicationPremiumItem.EndorsementId, state.ApplicationPremiumItem.PaymentNumber)
                        .done(function (previusTempApplication) {
                            if (previusTempApplication.success) {
                                if (previusTempApplication.result.Id == 0) {
                                    //Valida el orden en que se van añadiendo las cuotas
                                    var resultQuota = DialogSearchPolicies.QuotaOrderControlPremiums(state.ApplicationPremiumItem.BranchPrefixPolicyEndorsement, state.ApplicationPremiumItem.PaymentNumber);
                                    if (resultQuota) {
                                        DialogSearchPolicies.SaveTempPremiumComponentReceivableRequest(state).done(function (response) {
                                            if (response.success) {
                                                if (response.result) {
                                                    DialogSearchPolicies.ClearSearchPolicy(state, dispatch);
                                                    DialogSearchPolicies.LoadPremiums(state, dispatch);
                                                    dispatch({ type: 'PREMPAY_CLEAN_SEARCH' });
                                                    dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningSearchForm', value: false });
                                                } else {
                                                    $.UifNotify('show', { type: 'danger', message: Resources.CouldNotSaveTempPremiumComponent, autoclose: true });
                                                }
                                            } else {
                                                $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
                                            }
                                        }).always(function () {
                                            unlockScreen();
                                        });
                                    }
                                    else {
                                        $.UifNotify('show', { type: 'danger', message: Resources.QuotaSequenceAdded, autoclose: true });
                                        unlockScreen();
                                    }
                                } else {
                                    var message = Resources.BranchPrefixPolicyEndorsement + ": " + state.ApplicationPremiumItem.BranchPrefixPolicyEndorsement + ", " +
                                        Resources.Quota + ": " + state.ApplicationPremiumItem.PaymentNumber + ", " + Resources.AlreadyAppliedTemporalImputationQuotaWarning + " No.: " + previusTempApplication.result.Id + " - " + Resources.Branch + ": " + state.ApplicationPremiumItem.BranchName;
                                    $.UifNotify('show', { type: 'danger', message: message, autoclose: false });
                                    unlockScreen();
                                }
                            } else {
                                $.UifNotify('show', { type: 'info', message: previusTempApplication.result, autoclose: true });
                                unlockScreen();
                            }
                        }).fail(function () {
                            unlockScreen();
                        });
                } else {
                    msj = Resources.BranchPrefixPolicyEndorsement + ": " + state.ApplicationPremiumItem.BranchPrefixPolicyEndorsement + ", " +
                        Resources.Quota + ": " + state.ApplicationPremiumItem.PaymentNumber + ", " + Resources.SinglePolicyComponentsValidationMessage + " ";
                    $.UifNotify('show', { type: 'danger', message: msj, autoclose: false });
                    unlockScreen();
                }
            }).fail(function () {
                unlockScreen();
            });
    }

    static outputState(state, dispatch) {
        //Bags
        if (state.ViewState.loadBranches === true) {
            if (!this.isNull(state.Bags.Branches)) {
                $("#ModalPremiums").find("#BranchSearchDrop").UifSelect({ sourceData: state.Bags.Branches });
            }
        }
        if (state.ViewState.loadSearchType === true) {
            if (!this.isNull(state.Bags.SearchType)) {
                $("#ModalPremiums").find("#SearchBy").UifSelect({ sourceData: state.Bags.SearchType, selectedId: state.ViewSearchModel.SelectedItemSearchById });
            }
            if (!this.isNull(state.Bags.Prefixes)) {
                $("#ModalPremiums").find("#PrefixSearch").UifSelect({ sourceData: state.Bags.Prefixes });
            }
            if (!this.isNull(state.Bags.Branches)) {
                $("#ModalPremiums").find("#BranchSearch").UifSelect({ sourceData: state.Bags.Branches });
            }
        }
        if (state.ViewState.loadPrefixes === true) {
            if (!this.isNull(state.Bags.Prefixes)) {
                $("#ModalPremiums").find("#PrefixDrop").UifSelect({ sourceData: state.Bags.Prefixes });
            }
        }
        if (state.ViewState.SetDataSearchBy) {
            $("#ModalPremiums").find("#SearchBy").UifSelect("setSelected", state.searchBy.id);
            $("#ModalPremiums").find("#SearchBy").trigger('change');
            if (state.searchBy.id == 1) {
                $("#ModalPremiums").find("#InsuredDocumentNumberSearch").UifAutoComplete('setValue', state.searchBy.documentId);
                $("#ModalPremiums").find("#InsuredDocumentNumberSearch").trigger('itemSelected', selectedInsured);
            }
            else if (state.searchBy.id == 3) {
                $("#ModalPremiums").find("#AgentDocumentNumberDialog").UifAutoComplete('setValue', state.searchBy.documentId);
                $("#ModalPremiums").find("#AgentDocumentNumberDialog").trigger('itemSelected', selectedAgent);
            }
        }

        //eventos 

        if (state.ViewState.ShowInsuredData === true) {

            $("#ModalPremiums").find("#InsuredDocumentNumber").UifAutoComplete('setValue', state.ViewSearchModel.InsuredDocumentNumber);
            $("#ModalPremiums").find("#InsuredNameSearch").UifAutoComplete('setValue', state.ViewSearchModel.InsuredName);
        }
        if (state.ViewState.ShowInsuredDataSearch === true) {

            $("#ModalPremiums").find("#InsuredDocumentNumberSearch").UifAutoComplete('setValue', state.ViewSearchModel.InsuredDocumentNumber);
            $("#ModalPremiums").find("#InsuredNameSearch").UifAutoComplete('setValue', state.ViewSearchModel.InsuredName);
        }

        if (state.ViewState.ShowPayerData === true) {
            $("#ModalPremiums").find("#PayerDocumentNumber").UifAutoComplete('setValue', state.ViewSearchModel.PayerDocumentNumber);
            $("#ModalPremiums").find("#PayerName").UifAutoComplete('setValue', state.ViewSearchModel.PayerName);
        }
        if (state.ViewState.ShowAgentData === true) {
            $("#ModalPremiums").find("#AgentDocumentNumberDialog").UifAutoComplete('setValue', state.ViewSearchModel.AgentDocumentNumber);
            $("#ModalPremiums").find("#AgentNameDialog").UifAutoComplete('setValue', state.ViewSearchModel.AgentName);
        }

        //list view
        if (state.ViewState.isValidatingForm === true) {
            $("#ModalPremiums").find("#alertItemToPayView").toggle('hide');
            $("#ModalPremiums").find('#SearchAdvancedSection').toggle(state.ViewState.SearchAdvancedSection);
            $("#ItemToPayView").UifListView("clear");

            if (!isNull(state.ViewSearchModel.SearchPoliciesData) && Array.isArray(state.ViewSearchModel.SearchPoliciesData)) {
                for (var i = 0; i < state.ViewSearchModel.SearchPoliciesData.length; i++) {
                    $("#ItemToPayView").UifListView("addItem", state.ViewSearchModel.SearchPoliciesData[i]);
                }
            }
        }

        if (state.ViewState.ShowSelectSearch === true) {

            if (state.ViewState.SearchAdvancedSection) {
                DialogSearchPolicies.labelHide();
                if (state.ViewState.InsuredSearch) {
                    $("#ModalPremiums").find("#insuredSearch").show();
                } else if (state.ViewState.PayerSearch) {
                    $("#ModalPremiums").find("#payerSearch").show(state.ViewState.PayerSearch);
                } else if (state.ViewState.AgentSearch) {
                    $("#ModalPremiums").find("#agentSearch").show();
                }
                $("#ModalPremiums").find('#SearchAdvancedSection').show();
            } else if (state.ViewState.salesTicketSearch) {
                DialogSearchPolicies.labelHide();
                $("#ModalPremiums").find("#salesTicketSearch").show();
            } else if (state.ViewState.GroupSearch) {
                DialogSearchPolicies.labelHide();
                $("#ModalPremiums").find("#groupSearch").show();
            } else if (state.ViewState.PolicySearch) {
                DialogSearchPolicies.labelHide();
                $("#ModalPremiums").find("#policySearch").show();
                $("#ModalPremiums").find("#SearchAdvancedSection").show();
            }
            $("#ModalPremiums").find("#PayExpDateQuotaFrom").val(state.ViewModel.DateFrom);
            $("#ModalPremiums").find("#PayExpDateQuotaTo").val(state.ViewModel.DateTo);
        }
        else { DialogSearchPolicies.labelHide(); }

        if (state.ViewState.ShowItemApplyViewRowEdit === true) {
            DialogSearchPolicies.SetDataEditItem(state.ApplicationPremiumItem, state);

        }

        if (state.ViewState.SuccessSaveTempPremiumReceibavle === true) {
            $("#ModalPremiums").find("#ItemToApplyView").UifListView("refresh");
            $("#ModalPremiums").find("#alertItemToPayView").UifAlert('show', Resources.AddQuota, "success");
        }

        if (state.ViewState.isCleaningForm === true) {
            DialogSearchPolicies.SetClearSearchFields(state);
            $("#ModalPremiums").find("#SearchBy").trigger('itemSelected');
        }
        if (state.ViewState.isCleaningSearchForm === true) {
            DialogSearchPolicies.labelHide();
            $("#ModalPremiums").find("#alertItemToPayView").UifAlert('hide');
            $("#ModalPremiums").find("#alertPolicyView").UifAlert('hide');
            $("#ModalPremiums").find("#alertPrimeApply").UifAlert('hide');
            $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
            $("#ModalPremiums").find("#SelecEditPartial").hide();
            $("#ModalPremiums").find("#DepositPremiumsDialog").hide();
            $("#ModalPremiums").find("#CommissionRetainedDialog").hide();
            $("#ModalPremiums").find("#policyIdTxt").val(state.ViewModel.PolicyId);
            $("#ItemToPayView").UifListView("clear");
        }


        //blur
        if (state.ViewState.OnBlurDepositAmountPayingAmount === true) {
            $("#ModalPremiums").find("#DepositAmountPayingAmount").val("$ " + NumberFormatSearch(ClearFormatCurrency($("#ModalPremiums").find("#DepositAmountPayingAmount").val()), "2", ".", ","));
        }


        if (state.ViewState.OnBlurDateQuotaFrom === true) {
            $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
            if ($("#ModalPremiums").find("#PayExpDateQuotaFrom").val() != '') {
                if (IsDate($("#ModalPremiums").find("#PayExpDateQuotaFrom").val()) == true) {
                    if ($("#ModalPremiums").find("#PayExpDateQuotaTo").val() != '') {
                        if (CompareDates($("#ModalPremiums").find("#PayExpDateQuotaFrom").val(), $("#ModalPremiums").find("#PayExpDateQuotaTo").val())) {
                            $("#ModalPremiums").find("#PayExpDateQuotaFrom").val(getCurrentDate);
                        }
                    }
                } else {
                    $("#ModalPremiums").find("#alertPrime").UifAlert('show', Resources.InvalidDates, "warning");
                    $("#ModalPremiums").find("#PayExpDateQuotaFrom").val(state.ViewModel.DateFrom);
                }
            }
        }
        if (state.ViewState.OnBlurDateQuotaTo === true) {
            $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
            if ($("#ModalPremiums").find("#PayExpDateQuotaTo").val() != '') {
                if (IsDate($("#ModalPremiums").find("#PayExpDateQuotaTo").val()) == true) {
                    if ($("#ModalPremiums").find("#PayExpDateQuotaFrom").val() != '') {
                        if (CompareDates($("#ModalPremiums").find("#PayExpDateQuotaFrom").val(), $("#ModalPremiums").find("#PayExpDateQuotaTo").val())) {
                            $("#ModalPremiums").find("#PayExpDateQuotaTo").val(getCurrentDate);
                        }
                    }
                } else {
                    $("#ModalPremiums").find("#alertPrime").UifAlert('show', Resources.InvalidDates, "warning");
                    $("#ModalPremiums").find("#PayExpDateQuotaTo").val(state.ViewModel.DateTo);
                }
            }
        }
        if (state.ViewState.OnBlurPayerData === true) {
            if ((state.ViewModel.PayerId == "0") || ($("#ModalPremiums").find("#PayerDocumentNumber").val() == "")) {
                $("#ModalPremiums").find('#PayerDocumentNumber').val(state.ViewModel.PayerDocumentNumber);
                $("#ModalPremiums").find('#PayerName').val(state.ViewModel.PayerDocumentNumber);
            }
        }
        if (state.ViewState.OnBlurInsuredData === true) {
            if (state.ViewModel.InsuredDocumentNumber == "0" || $("#ModalPremiums").find("#InsuredDocumentNumber").val() == "") {
                $("#ModalPremiums").find('#InsuredDocumentNumber').val(state.ViewModel.InsuredDocumentNumber);
            }
            if (state.ViewModel.InsuredName == "0" || $("#ModalPremiums").find("#InsuredName").val() == "") {
                $("#ModalPremiums").find('#InsuredDocumentNumber').val(state.ViewModel.InsuredDocumentNumber);
            }
        }
        if (state.ViewState.onBlurGroupData === true) {
            if ((state.ViewModel.groupNumber == "0") || ($("#ModalPremiums").find("#GroupNumber").val() == "")) {
                $("#ModalPremiums").find('#GroupNumber').val(state.ViewModel.groupNumber);
                $("#ModalPremiums").find('#GroupName').val(state.ViewModel.GroupName);
            }
            if ((state.ViewModel.GroupName == "0") || ($("#ModalPremiums").find("#GroupName").val() == "")) {
                $("#ModalPremiums").find('#GroupNumber').val(state.ViewModel.groupNumber);
                $("#ModalPremiums").find('#GroupName').val(state.ViewModel.GroupName);
            }
        }
        //radio
        if (state.ViewState.AmountToPayCheckClick === true) {
            $("#ModalPremiums").find("#CommissionRetainedDialog").hide();
            $("#ModalPremiums").find("#DepositPremiumsDialog").slideDown("slow");
        }
        if (state.ViewState.CommissionCheckClick === true) {
            $("#ModalPremiums").find("#DepositPremiumsDialog").hide();
            $("#ModalPremiums").find("#CommissionRetainedDialog").slideDown("slow");
        }
        //Checks
        if (state.ViewState.IsDepositAmountUsePaymentBalanceCheck === true) {
            $("#ModalPremiums").find("#DepositAmountPayingAmount").val("0");

            $("#ModalPremiums").find("#DepositPremiumsHeader").slideUp("slow");
            $("#ModalPremiums").find("#DepositPremiumsDiv").slideDown("slow");
        }
        if (state.ViewState.IsDepositPremiumRowEdit === true) {
            $("#ModalPremiums").find("#editForm").find("#UsedAmountForm").val(state.Data.DepositPremiumRowEdit.UsedAmount);
            $("#ModalPremiums").find('#editAction').UifInline('show');
        }
        if (state.ViewState.OnActionSaveClick === true) {
            $("#ModalPremiums").find('#DepositPremiumsTable').UifDataTable('editRow', state.Data.DepositPremiumRowEdit, state.Data.DepositPremiumRowEdit.CurrentEditIndex);

            $("#ModalPremiums").find("#editForm").formReset();
            $("#ModalPremiums").find('#editAction').UifInline('hide');

        }
        if (state.ViewState.isDeleteDAUPaymentBalanceCheck === true) {
            $("#ModalPremiums").find("#DepositAmountPayingAmount").val(0);
            $("#ModalPremiums").find("#DepositPremiumsHeader").slideDown("slow");
            $("#ModalPremiums").find("#DepositPremiumsDiv").slideUp("slow");

            $("#ModalPremiums").find("#DepositAmountUsePaymentBalance").removeAttr('checked');
        }




        if (state.ViewState.LoadPremiums) {
            $("#ItemToApplyView").UifListView("clear");
            $("#ItemToApplyView").UifListView({
                autoHeight: true,
                theme: 'dark',
                customDelete: true,
                customEdit: true,
                edit: true,
                delete: true,
                sourceData: state.ApplicationPremiumModel.ApplicationPremiumItems,
                displayTemplate: "#ItemToApplyViewTemplate"
            });
            $("#ModalPremiums").find('#TotalPremiumSearch').text(FormatMoneySymbol(state.ApplicationPremiumModel.TotalPremium));

        }
        if (state.ViewState.SetDataDepositPremiums) {
            $("#ModalPremiums").find("#DepositPremiumsDialog").show();
            $("#ModalPremiums").find("#DepositPremiumsTable").UifDataTable({ sourceData: state.DepositPremiumModel });
        }
        if (state.ViewState.SetDataDiscountedCommissions) {
            $("#ModalPremiums").find("#DiscountedCommissionTable").UifDataTable('clear');
            $.each(state.DiscountedCommissions, function (i, value) {
                if (state.DiscountedCommissions[i].IsUsedCommission == true) {
                    $("#ModalPremiums").find("#DiscountedCommissionTable").UifDataTable('addRow', value);
                    $("#ModalPremiums").find('#totalCommissionfooter').text(state.TotalCommissionFoot);
                }
                else { $("#ModalPremiums").find("#alertCommissionInfo").UifAlert('show', Resources.UserNotCommissionAvailable, "warning"); }
            });

            $("#ModalPremiums").find("#TotalCommissionTxt").text(state.TotalCommissionFoot);
        }
        if (state.ViewState.SetDataPolicY) {

            if (!isNull(state.ApplicationPremiumItem.ApplicationPremiumId)) {
                $.each(state.ApplicationPremiumModel.ApplicationPremiumItems, function (i, value) {
                    if (state.ApplicationPremiumModel.ApplicationPremiumItems[i].ApplicationPremiumId == state.ApplicationPremiumItem.ApplicationPremiumId) {
                        $("#ModalPremiums").find("#ItemToApplyView").UifListView("editItem", i, state.ApplicationPremiumItem);
                    }
                });
            } else {
                // $("#ModalPremiums").find("#ItemToApplyView").UifListView("clear");
                $("#ItemToApplyView").UifListView({
                    autoHeight: true,
                    theme: 'dark',
                    customDelete: true,
                    customEdit: true,
                    edit: true,
                    delete: true,
                    sourceData: state.ApplicationPremiumModel.ApplicationPremiumItems,
                    displayTemplate: "#ItemToApplyViewTemplate"
                });
            }

        }
        if (state.ViewState.SetDataEditItem) {
            DialogSearchPolicies.SetDataEditItem(state.ApplicationPremiumItem, state);
        }
        if (state.ViewState.SetDataQuota) {
            $("#ModalPremiums").find("#searchPaymentNumber").UifSelect();
            $("#ModalPremiums").find("#searchPaymentNumber").UifSelect({
                sourceData: state.QuotaList,
                id: "PaymentNumber",
                name: "PaymentNumber",
                native: false,
                filter: true,
                enable: true
            });
            $("#ModalPremiums").find("#searchPaymentNumber").UifSelect("setSelected", state.ApplicationPremiumItem.PaymentNumber);
        }

        if (state.ViewState.SetSerchFieldsEmpty) {
            $("#ModalPremiums").find("#DepositAmountUsePaymentBalanceCheck").attr('checked', false);

            $("#ModalPremiums").find("#alertItemToPayView").UifAlert('hide');
            $("#ModalPremiums").find("#alertPrimeApply").UifAlert('hide');
            $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
            state.dropDownSearchAdv.hide();
            $("#ModalPremiums").find("#SelecEditPartial").hide();
            $("#ModalPremiums").find("#DepositPremiumsDialog").hide();
            $("#ModalPremiums").find("#CommissionRetainedDialog").hide();
        }

        if (state.ViewState.DisabledCheckNoExpenses === true) {
            $("#checkNoExpenses").prop("disabled", "disabled");
            if (state.ApplicationPremiumItem.OriginalExpenses > 0) {
                $("#checkNoExpenses").removeAttr("disabled");
            }
        }

        if (!DialogSearchPolicies.isNull(state.ApplicationPremiumItem)) {
            $("#ModalPremiums").find("#CashAmount").val(FormatMoneySymbol(state.ApplicationPremiumItem.PaymentAmount));
            $("#ModalPremiums").find("#CashLocalAmount").val(FormatMoneySymbol(state.ApplicationPremiumItem.PaymentLocalAmount));
            $("#ModalPremiums").find("#CashExchangeRate").val(state.ApplicationPremiumItem.ExchangeRate);
            $("#ModalPremiums").find("#Premium_txt").val(FormatMoneySymbol(state.ApplicationPremiumItem.TotalPremium));
            $("#ModalPremiums").find("#Tax_txt").val(FormatMoneySymbol(state.ApplicationPremiumItem.Tax));
            $("#ModalPremiums").find("#Expenses_txt").val(FormatMoneySymbol(state.ApplicationPremiumItem.Expenses));
        }
        if (state.ViewState.changeExchangeRate === true) {
            $("#ModalPremiums").find("#CashExchangeRate").val(state.EnchangeRate);
        }
        if (state.ViewState.IsClearAdvancedSearchField) {
            DialogSearchPolicies.SetClearAdvancedSearchField(state);
            DialogSearchPolicies.labelHide();
        }
        if (state.ViewState.ShowPolicyWarning) {
            state.ViewState.ShowPolicyWarning = false;
            $("#alertPolicyView").UifAlert('show', state.ViewState.ShowPolicyWarning, "warning");
        }
    }

    static setDataFieldsEmptySearch() {
        $("#ModalPremiums").find("#ChangeAmountBranchName").val('');
        $("#ModalPremiums").find("#ChangeAmountPrefix").val('');
        $("#ModalPremiums").find("#ChangeAmountPolicy").val('');
        $("#ModalPremiums").find("#ChangeAmountEndorsement").val('');
        $("#ModalPremiums").find("#ChangeAmountPayment").val('');
        $("#ModalPremiums").find("#ChangeAmountPaymentBalance").val('');
        $("#ModalPremiums").find("#ChangeAmountDepositPrimes").val('');
        $("#ModalPremiums").find("#ChangeAmountPayingAmount").val('');
        $("#ModalPremiums").find("#ChangeAmountExcessPayment").val(0);
        $("#ModalPremiums").find("#ChangeAmountAgent").val('');
        $("#ModalPremiums").find("#ChangeAmountPendantCommission").val('');
        $("#ModalPremiums").find("#ChangeAmountDiscountedCommission").val('');

        $("#ModalPremiums").find("#DepositAmountUsePaymentBalanceCheck").attr('checked', false);
    }

    static totalUsedAmount() {
        var total = 0;

        var ids = $("#ModalPremiums").find("#DepositPremiumsTable").UifDataTable('getData');

        for (var i in ids) {

            var row = ids[i];

            if (row.UsedAmount == "")
                return false;

            total += parseFloat(RemoveFormatMoney(row.UsedAmount));
        }
        if (parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPayingAmount").val())) == 0 ||
            parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPaymentBalance").val())) == 0) {
            $("#ModalPremiums").find("#DepositAmountPayingAmount").val(total);
        }
        if (parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPayingAmount").val())) >= total) {

            var amount = RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPayingAmount").val());
            $("#ModalPremiums").find("#DepositAmountPayingAmount").val(amount);
        }
        if (parseFloat(RemoveFormatMoney($("#ModalPremiums").find("#DepositAmountPayingAmount").val())) <= total) {
            $("#ModalPremiums").find("#DepositAmountPayingAmount").val(total);
        }
        return total;
    }

    static SetDataEditItem(data, state) {
        $("#ModalPremiums").find("#policyIdTxt").val(data.PolicyId);
        $("#ModalPremiums").find("#SearchPolicyNumber").val(data.PolicyNumber);
        $("#ModalPremiums").find("#selectCashCurrency").UifSelect("setSelected", data.CurrencyCode);
        if (data.CurrencyCode >= 0) {
            $("#ModalPremiums").find("#CurrencyDescriptionText").html($("#ModalPremiums").find("#selectCashCurrency").UifSelect("getSelectedText"));
        } else {
            $("#ModalPremiums").find("#CurrencyDescriptionText").html("");
        }
        $("#ModalPremiums").find("#searchEndorsementNumber").UifSelect();
        $("#ModalPremiums").find("#searchPaymentNumber").UifSelect();
        $("#ModalPremiums").find("#searchPaymentNumber").UifSelect({
            sourceData: state.QuotaList,
            id: "PaymentNumber",
            name: "PaymentNumber",
            native: false,
            filter: true,
            enable: true
        });
        $("#ModalPremiums").find("#searchEndorsementNumber").UifSelect({
            sourceData: state.EndorsementList,
            id: "EndorsementId",
            name: "EndorsementNumber",
            native: false,
            filter: true,
            enable: true
        });
        $("#ModalPremiums").find("#searchEndorsementNumber").UifSelect("setSelected", data.EndorsementId);
        $("#ModalPremiums").find("#searchPaymentNumber").UifSelect("setSelected", data.PaymentNumber);

        $("#ModalPremiums").find("#searchPaymentNumber").val(data.QuotaNumber);
        $("#ModalPremiums").find("#InsuredDocumentNumber").text(data.InsuredDocumentNumber);
        $("#ModalPremiums").find("#BranchSearchDrop").UifSelect("setSelected", data.BranchId);
        $("#ModalPremiums").find("#PrefixDrop").UifSelect("setSelected", data.PrefixId);


    }

    static SetClearSearchFields(state) {

        $("#ModalPremiums").find("#SearchBy").val(state.ViewModel.SelectedItemSearchById);
        $("#ModalPremiums").find("#InsuredDocumentNumber").text(state.ViewModel.SelectedItemSearchById);
        $("#ModalPremiums").find("#InsuredName").val(state.ViewModel.InsuredName);
        $("#ModalPremiums").find("#PayerDocumentNumber").val(state.ViewModel.PayerDocumentNumber);
        $("#ModalPremiums").find("#PayerName").val(state.ViewModel.PayerName);
        $("#ModalPremiums").find("#AgentDocumentNumber").val(state.ViewModel.AgentDocumentNumber);
        $("#ModalPremiums").find("#AgentName").val(state.ViewModel.AgentName);
        $("#ModalPremiums").find("#checkNoExpenses").prop('checked', false);
        $("#ModalPremiums").find('#checkNoExpenses').prop("disabled", true);

        $("#ModalPremiums").find("#SaleTicketDocumentNumber").val(state.ViewModel.SalesTicket);
        $("#ModalPremiums").find("#SaleTicketName").val(state.ViewModel.salesTicketName);
        $("#ModalPremiums").find("#BranchSearchDrop").val(state.ViewModel.BranchId);
        $("#ModalPremiums").find("#PrefixDrop").val(state.ViewModel.PrefixId);
        $("#ModalPremiums").find("#Endorsement").val(state.ViewModel.EndorsementDocumentNumber);
        $("#ModalPremiums").find('#totalCommissionfooter').text(state.TotalCommissionFoot);


        $("#ModalPremiums").find("#SearchPolicyNumber").val(state.ApplicationPremiumItem.PolicyNumber);
        $("#ModalPremiums").find("#selectCashCurrency").UifSelect("setSelected", state.ApplicationPremiumItem.CurrencyCode);
        if (state.ApplicationPremiumItem.CurrencyCode >= 0) {
            $("#ModalPremiums").find("#CurrencyDescriptionText").html($("#ModalPremiums").find("#selectCashCurrency").UifSelect("getSelectedText"));
        } else {
            $("#ModalPremiums").find("#CurrencyDescriptionText").html("");
        }
        $("#ModalPremiums").find("#searchEndorsementNumber").UifSelect({
            source: null,
            native: false,
            filter: true,
            enable: true,
        });
        $("#ModalPremiums").find("#searchPaymentNumber").UifSelect({
            source: null,
            native: false,
            filter: true,
            enable: true,
        });
        $("#ModalPremiums").find("#CashExchangeRate").val(state.ApplicationPremiumItem.ExchangeRate);
        $("#ModalPremiums").find("#CashAmount").val(state.ApplicationPremiumItem.QuotaValue);
        $("#ModalPremiums").find("#CashLocalAmount").val(state.ApplicationPremiumItem.QuotaValue);
        $("#ModalPremiums").find("#InsuredDocumentNumber").val(state.ApplicationPremiumItem.InsuredDocumentNumber);
        $("#ModalPremiums").find("#Premium_txt").val(FormatMoneySymbol(state.ApplicationPremiumItem.QuotaValue));
        $("#ModalPremiums").find("#Tax_txt").val("");
        $("#ModalPremiums").find("#Expenses_txt").val("");
        $("#ModalPremiums").find('#CommissionTxt').val("");
        if (!isNull(state.CommissionDiscountedItem.Amount)) {
            $("#ModalPremiums").find("#TotalCommissionTxt").text(state.CommissionDiscountedItem.Amount);
        } else {
            $("#ModalPremiums").find("#TotalCommissionTxt").text(state.TotalCommissionFoot);
        }

        $("#ModalPremiums").find("#DiscountedCommissionTable").UifDataTable('clear');
        $("#ModalPremiums").find("#DepositPremiumsTable").UifDataTable('clear');


        //$("#ModalPremiums").find("#ItemToPayView").UifListView();


        $("#ModalPremiums").find("#alertItemToPayView").UifAlert('hide');
        $("#ModalPremiums").find("#alertPrimeApply").UifAlert('hide');
        $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
        $("#ModalPremiums").find("#SelecEditPartial").hide();
        $("#ModalPremiums").find("#DepositPremiumsDialog").hide();
        $("#ModalPremiums").find("#CommissionRetainedDialog").hide();
        //$("#ModalPremiums").find("#CashAmount").attr("disabled", true);
        $("#alertPolicyView").UifAlert('hide');
    }

    static SetClearAdvancedSearchField(state) {
        $("#ModalPremiums").find("#InsuredDocumentNumberSearch").UifAutoComplete('setValue', state.ViewSearchModel.InsuredDocumentNumberSearch);
        $("#ModalPremiums").find("#InsuredNameSearch").UifAutoComplete('setValue', state.ViewSearchModel.InsuredNameSearch);
        $("#ModalPremiums").find("#BranchSearch").val(state.ViewSearchModel.SelectedItemSearchById);
        $("#ModalPremiums").find("#PrefixSearch").val(state.ViewSearchModel.SelectedItemSearchById);
        $("#ModalPremiums").find("#PayExpDateQuotaFrom").val(state.ViewSearchModel.DateFrom);
        $("#ModalPremiums").find("#PayExpDateQuotaTo").val(state.ViewSearchModel.DateTo);
        $("#ModalPremiums").find("#GroupNumber").val(state.ViewSearchModel.GroupId);
        $("#ModalPremiums").find("#GroupName").val(state.ViewSearchModel.GroupName);
        $("#ModalPremiums").find("#PolicyDocumentNumber").val(state.ViewSearchModel.PolicyDocumentNumber);
        $("#ModalPremiums").find("#Endorsement").val(state.ViewSearchModel.Endrosementctrl);
        $("#ModalPremiums").find("#PayerDocumentNumber").val(state.ViewSearchModel.PayerDocumentNumber);
        $("#ModalPremiums").find("#PayerName").val(state.ViewSearchModel.PayerName);
        $("#ModalPremiums").find("#AgentDocumentNumberDialog").val(state.ViewSearchModel.AgentDocumentNumberDialog);
        $("#ModalPremiums").find("#AgentNameDialog").val(state.ViewSearchModel.AgentNameDialog);
        $("#ModalPremiums").find("#Endorsement").val(state.ViewSearchModel.Endorsement);
        //$("#ModalPremiums").find("#ItemToPayView").UifListView();
    }

    static labelHide() {
        $("#ModalPremiums").find("#insuredSearch").hide();
        $("#ModalPremiums").find("#payerSearch").hide();
        $("#ModalPremiums").find("#agentSearch").hide();
        $("#ModalPremiums").find("#groupSearch").hide();
        $("#ModalPremiums").find("#policySearch").hide();
        $("#ModalPremiums").find("#salesTicketSearch").hide();
        $("#ModalPremiums").find('#SearchAdvancedSection').hide();
    }

    static isNull(obj) {
        return (obj == undefined || obj == null || obj === "");
    }

    static isEmpty(text) {
        return (text == undefined || text == null || (typeof text === "string" && text.trim() == ""));
    }

    static validateAmount(newAmount, maxAmount) {
        return (maxAmount >= 0) ?
            newAmount >= 0 && newAmount <= maxAmount :
            newAmount <= 0 && newAmount >= maxAmount;
    }

    static validatePremiumAmount(newAmount, originalAmount) {
        if (originalAmount < 0) {
            return originalAmount <= newAmount && newAmount <= 0;
        } else {
            return newAmount >= 0;
        }
    }
}