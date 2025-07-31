class AccountingRedux {
    static redux(state, actions) {
        state = state || {
            Persist: true,
            TempApplicationId: null,
            ViewModel: {
                Id: null,
                Branch: { Id: null, Description: null },
                SalePoint: { Id: null, Description: null },
                AccountingConcept: { Id: null, Description: null },
                Beneficiary: {
                    IndividualId: null,
                    Name: null,
                    IdentificationDocument: {
                        Number: null
                    }
                },
                BookAccount: {
                    Id: null,
                    AccountNumber: null,
                    AccountName: null
                },
                AccountingNature: null,
                Amount: {
                    Currency: {
                        Id: null
                    },
                    Value: null
                },
                LocalAmount: {
                    Value: null
                },
                ExchangeRate: {
                    SellAmount: null
                },
                Description: null,
                BankReconciliationId: null,
                ReceiptNumber: null,
                ReceiptDate: null,
                AccountingAnalysisCodes: [],
                AccountingCostCenters: [],
                AnalysisId: null,
                ConceptKeys: {},
                AnalysisCode: {
                    AnalysisId: null,
                    AnalysisConcept: {
                        AnalysisConceptId: null
                    }
                },
                CostCenterCode: {
                    CostCenter: { CostCenterId: null, Description: "" },
                    Percentage: 0
                }
            },
            uifNotifyModel: null,
            Bags: {},
            ViewState: { Counter: 0 },
            Accountings: [],
            Movements: [],
            ViewModelExtra: {}
        };
        switch (actions.type) {
            case 'ACC_EDIT_MOVEMENT':
                state.ViewModel = actions.ApplicationAccounting;
                state.ViewModel.AnalysisCode = {
                    AnalysisId: null,
                    AnalysisConcept: {
                        AnalysisConceptId: null
                    }                    
                };
                state.ViewState.LoadTemporal = true;
                return state;

            case 'ACC_EDIT_CODE_ANALYSIS':
                state.ViewModel.AccountingAnalysisCodes = actions.ApplicationAccountingCodeAnalysis;
                state.ViewState.ShowAnalysisCodes = true;
                return state;

            case 'ACC_LOAD_CODE_ANALYSIS':
                state.ViewModel.AnalysisCode = {
                    AnalysisId: actions.LoadApplicationAccountingCodeAnalysis.AnalysisId,
                    AnalysisConcept: {
                        AnalysisConceptId: actions.LoadApplicationAccountingCodeAnalysis.AnalysisConceptId
                    }
                };
                state.ViewModel.DescriptionCodeAnalysis = actions.LoadApplicationAccountingCodeAnalysis.Description;
                state.ViewState.LoadAnalysisCode = true;
                return state;
            case 'ACC_SET_LOAD_CODE_ANALYSIS':
                var conceptKeysValues = "";
                conceptKeysValues = actions.AnalysisCode.ConceptKey.split("|");
                state.ViewModel.DescriptionCodeAnalysis = actions.AnalysisCode.Description;
                $.each(state.ViewModel.ConceptKeys, function (index, item) {
                    $("#" + item.ColumnName).val(conceptKeysValues[index].trim());
                });
                return state;

            case 'ACC_ASSIGN_TEMP_APPLICATION_ID':
                state.TempApplicationId = actions.TempImputationId;
                return state;

            case 'ACC_SET_BRANCH':
                if (actions.selectedItem.Id != state.ViewModel.Branch.Id) {
                    state.ViewModel.Branch = {
                        Id: actions.selectedItem.Id,
                        Description: actions.selectedItem.Text
                    };
                }
                return state;

            case 'ACC_SET_NATURE':
                if (actions.selectedItem.Id != state.ViewModel.AccountingNature) {
                    state.ViewModel.AccountingNature = actions.selectedItem.Id;
                    state.ViewModel.AccountingNatureDescription = actions.selectedItem.Text;
                }
                return state;

            case 'ACC_SET_CURRENCY':
                if (actions.selectedItem.Id != state.ViewModel.Amount.Currency.Id) {
                    state.ViewModel.Amount.Currency = {
                        Id: actions.selectedItem.Id,
                        Description: actions.selectedItem.Text
                    };
                    state.ViewState.LoadExchangeRate = false;
                }
                return state;

            case 'ACC_SET_SALEPOINT':
                if (actions.selectedItem.Id != state.ViewModel.SalePoint.Id) {
                    state.ViewModel.SalePoint = {
                        Id: actions.selectedItem.Id,
                        Description: actions.selectedItem.Text
                    };
                }
                return state;

            case 'ACC_SET_ANALYSIS':
                if (actions.selectedItem.Id > 0) {
                    state.ViewModel.AnalysisCode.AnalysisId = actions.selectedItem.Id;
                    state.ViewState.clearConceptsControl = false;
                    state.ViewState.LoadAnalysisConcepts = true;
                    state.ViewState.selectedItemAnalysis = true;
                } else {
                    state.ViewState.LoadAnalysisConcepts = false;
                }

                return state;

            case 'ACC_SET_ANALYSIS_CONCEPT':
                state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId = actions.selectedItem.Id;
                return state;

            case 'ACC_SET_PREMIUM_DEPOSIT':
                if (actions.selectedItem !== null || actions.selectedItem !== undefined) {
                    state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId = actions.selectedItem.Id;
                }
                return state;
            case 'ACC_CLEAN_ANALYSIS':
                state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId = null;
                state.ViewModel.DescriptionCodeAnalysis = "";
                state.ViewState.selectedItemConcepts = true;
                return state;
            case 'ACC_CLEAN':
                state.ViewModel = {
                    Id: null,
                    Branch: { Id: null, Description: null },
                    SalePoint: { Id: null, Description: null },
                    AccountingConcept: { Id: null, Description: null },
                    Beneficiary: {
                        IndividualId: null,
                        Name: null,
                        IdentificationDocument: {
                            Number: null
                        }
                    },
                    BookAccount: {
                        Id: null,
                        AccountNumber: null,
                        AccountName: null
                    },
                    AccountingNature: null,
                    Amount: {
                        Currency: {
                            Id: null
                        },
                        Value: null
                    },
                    LocalAmount: {
                        Value: null
                    },
                    ExchangeRate: {
                        SellAmount: null
                    },
                    Description: null,
                    BankReconciliationId: null,
                    ReceiptNumber: null,
                    ReceiptDate: null,
                    AccountingAnalysisCodes: [],
                    AccountingCostCenters: [],
                    AnalysisId: null,
                    AnalysisCode: {
                        AnalysisId: null,
                        AnalysisConcept: {
                            AnalysisConceptId: null
                        }
                    },
                    CostCenterCode: {
                        CostCenter: { CostCenterId: null, Description: "" },
                        Percentage: 0
                    }
                };
                if (!isNull(state.ViewState.DefaultBranch)) {
                    state.ViewModel.Branch.Id = parseInt(state.ViewState.DefaultBranch);
                }
                state.ViewState.Clear = true;
                state.Movements = [];
                return state;

            case 'ACC_LOAD_COST_CENTER':
                state.Bags.CostCenter = [];
                if (!AccountingRedux.isNull(actions.Bags.CostCenter)) {
                    state.Bags.CostCenter = actions.Bags.CostCenter;
                }
                state.ViewState.LoadCostCenter = true;
                return state;

            case 'ACC_LOAD_ANALYSIS':
                if (!AccountingRedux.isNull(actions.Bags.Analysis)) {
                    state.Bags.Analysis = actions.Bags.Analysis;
                    state.ViewState.LoadAnalysis = true;
                }
                return state;

            case 'ACC_LOADS':
                state.Bags.AccountingNumb = [];
                if (!AccountingRedux.isNull(actions.Bags.AccountingNumb)) {
                    state.Bags.AccountingNumb = actions.Bags.AccountingNumb;
                }
                return state;

            case 'ACC_LOAD':

                if (!AccountingRedux.isNull(actions.Bags.Branches)) {
                    state.Bags.Branches = actions.Bags.Branches;
                    if (state.Bags.Branches.length == 1) {
                        state.ViewModel.Branch.Id = state.Bags.Branches[0].Id;
                    }
                    state.ViewState.LoadBranches = true;
                }
                if (!AccountingRedux.isNull(actions.Bags.Natures)) {
                    state.Bags.Natures = actions.Bags.Natures;
                    state.ViewState.LoadNatures = true;
                }
                if (!AccountingRedux.isNull(actions.Bags.AccountingCompanies)) {
                    state.Bags.AccountingCompanies = actions.Bags.AccountingCompanies;
                    state.ViewState.LoadCompanies = true;
                }
                if (!AccountingRedux.isNull(actions.Bags.Currencies)) {
                    state.Bags.OriginalCurrencies = actions.Bags.Currencies;
                    state.Bags.Currencies = state.Bags.OriginalCurrencies;
                    state.ViewState.LoadCurrencies = true;
                }
                if (!AccountingRedux.isNull(actions.Bags.AnalysisConcepts)) {
                    state.Bags.AnalysisConcepts = actions.Bags.AnalysisConcepts;
                    if (state.Bags.AnalysisConcepts.length == 1) {
                        state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId = state.Bags.AnalysisConcepts[0].AnalysisConceptId;
                    }
                    state.ViewState.LoadAnalysisConcepts = true;
                }

                return state;

            case 'ACC_POSTDATED_ADD':
                if (state.ViewModel.Postdated == null || state.ViewModel.Postdated == undefined) {
                    state.ViewModel.Postdated = [];
                }
                state.ViewState.AddNewPosdated = true;
                state.ViewModel.Posdated.push(actions.postdated);
                return state;

            case 'ACC_POSTDATED_DELETE':
                if (state.ViewModel.Postdated != null && state.ViewModel.Postdated != undefined
                    && index >= state.ViewModel.Postdated.length && index < state.ViewModel.Postdated.length) {
                    state.ViewModel.Posdated.splice(index, 1);
                }
                return state;

            case 'ACC_SET_VALUE':
                state.ViewState[actions.label] = actions.value;
                return state;

            case 'ACC_SET_VALUES':
                for (var i = 0; i < actions.values.length; i++) {
                    state.ViewState[actions.values[i].label] = actions.values[i].value;
                }
                return state;

            case 'ACC_SET_ACCOUNTING_DOCUMENT':
                state.ViewModel.Beneficiary.IndividualId = actions.selectedItem.IndividualId ? actions.selectedItem.IndividualId : actions.selectedItem.Id;
                state.ViewModel.Beneficiary.Name = actions.selectedItem.Name;
                state.ViewModel.Beneficiary.IdentificationDocument.Number = actions.selectedItem.DocumentNumber;
                return state;

            case 'ACC_SET_PAYMENT_CONCEPT':
                state.ViewModel.AccountingConcept = {
                    Id: actions.selectedItem.Id,
                    Description: actions.selectedItem.Description
                };
                state.ViewModel.BookAccount = {
                    Id: actions.selectedItem.GeneralLedgerId,
                    AccountNumber: actions.selectedItem.AccountingNumber,
                    AccountName: actions.selectedItem.AccountingName
                };
                if (actions.selectedItem.CurrencyId >= 0) {
                    state.ViewModel.Amount.Currency = {
                        Id: actions.selectedItem.CurrencyId
                    };
                    state.ViewState.LoadExchangeRate = true;
                }
                if (!AccountingRedux.isEmpty(actions.selectedItem.AccountingNature)
                    && actions.selectedItem.AccountingNature > 0) {
                    state.ViewModel.AccountingNature = actions.selectedItem.AccountingNature;
                }
                if (actions.selectedItem.MultiCurrency === false) {
                    var result = state.Bags.OriginalCurrencies.filter(currency => currency.Id == actions.selectedItem.CurrencyId);
                    if (Array.isArray(result) && result.length > 0) {
                        state.Bags.Currencies = result;
                        state.ViewState.LoadCurrencies = true;
                    }
                }
                state.ViewModelExtra.RequiresAnalysis = actions.selectedItem.RequiresAnalysis;
                state.ViewModelExtra.RequiresCostCenter = actions.selectedItem.RequiresCostCenter;
                state.ViewState.LockScreen = true;
                return state;

            case 'ACC_SET_ACCOUNTING_AMOUNT':
                if (AccountingRedux.isEmpty(actions.Amount)) {
                    state.ViewModel.Amount.Value = 0;
                    state.ViewModel.LocalAmount.Value = 0;
                } else {
                    state.ViewModel.Amount.Value = actions.Amount;
                    if (!AccountingRedux.isEmpty(state.ViewModel.ExchangeRate.SellAmount)) {
                        state.ViewModel.LocalAmount.Value = fix(actions.Amount * state.ViewModel.ExchangeRate.SellAmount);
                    }
                }
                return state;

            case 'ACC_SET_ACCOUNTING_DECRIPTION':
                if (!AccountingRedux.isEmpty(actions.Description)) {
                    state.ViewModel.Description = actions.Description;
                }
                return state;

            case 'ACC_SET_RATE_DATA':
                state.ViewModel.ExchangeRate.SellAmount = actions.accountingExchangeRate;
                return state;

            case 'ACC_LOAD_MOVEMENTS_LIST':

                state.ViewModelExtra.TotalCreditsMovements = 0;
                state.ViewModelExtra.TotalDebitMovements = 0;

                if (!AccountingRedux.isNull(actions.Bags.Movements)) {
                    state.Movements = actions.Bags.Movements;
                    state.ViewState.LoadMovements = true;

                    for (var i = 0; i < state.Movements.length; i++) {
                        if (state.Movements[i].AccountingNature === 1) {
                            state.ViewModelExtra.TotalCreditsMovements = state.ViewModelExtra.TotalCreditsMovements + state.Movements[i].LocalAmount.Value;
                        } else {
                            state.ViewModelExtra.TotalDebitMovements = state.ViewModelExtra.TotalDebitMovements + state.Movements[i].LocalAmount.Value;
                        }
                        state.Movements[i].FormatAmount = FormatMoneySymbol(state.Movements[i].LocalAmount.Value);
                        state.Movements[i].ItHasCodeAnalysis = false;
                        state.Movements[i].ItHasCostCenter = false;
                        
                        state.ViewState.ClearConceptsKeys = false;
                        if (state.Movements[i].AccountingAnalysisCodes.length > 0) {
                            state.Movements[i].ItHasCodeAnalysis = true;
                            state.ViewState.ClearConceptsKeys = true;
                        }
                        if (state.Movements[i].AccountingCostCenters.length > 0) {
                            state.Movements[i].ItHasCostCenter = true;
                        }
                    }
                }
                return state;

            case 'ACC_LOAD_SALES_POINTS':
                if (!AccountingRedux.isNull(actions.Bags.SalesPoints)) {
                    state.Bags.SalesPoints = actions.Bags.SalesPoints;
                    state.ViewState.LoadSalesPoint = true;
                }
                return state;

            case 'ACC_DELETE_MOVEMENT':

                for (var i = 0; i < state.Movements.length; i++) {
                    if (state.Movements[i].Index == actions.Index) {
                        state.Movements.splice(i, 1);
                        break;
                    }
                }
                return state;

            case 'ACC_SET_KEYS_VALUES':
                state.ViewModel.ConceptKeys = actions.data;
                return state;
            case 'ACC_ADD_KEYS_VALUES':
                var fieldsDescription = "";
                state.ViewState.ShowAnalysisCodes = true;
                state.ViewState.ClearConceptsKeys = true;
                state.ViewModel.AccountingAnalysisCodes = [];

                $.each(actions.data, function (index, value) {
                    fieldsDescription += $("#" + value.ColumnName).val() + '|';
                });

                let codeAnalysis = {
                    AnalysisId: state.ViewModel.AnalysisCode.AnalysisId,
                    AnalysisConcept: {
                        AnalysisConceptId: state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId,
                    },
                    ConceptKey: fieldsDescription,
                    Description: actions.accountingAnalysisDescription,
                    DescriptionConcept: actions.accountingAnalysisConceptDescription
                };
                state.ViewModel.AccountingAnalysisCodes.push(Object.assign({}, codeAnalysis));
                state.ViewModel.DescriptionCodeAnalysis = "";
                state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId = null;
                return state;
            case 'ACC_SET_NOTIFY_MESSAGE':
                state.ViewModelExtra.MessageInfo = actions.Message;
                state.ViewState.showMessageInfoNotify = true;
                return state;
            case 'ACC_EDIT_COST_CENTER':
                $.each(actions.ApplicationAccountingCostCenter, function (i, value) {
                    value.CostCenter = AccountingRedux.getCostCenterById(state.Bags.CostCenter, value.CostCenter.CostCenterId);
                    state.ViewModel.AccountingCostCenters.push(Object.assign({}, value));
                });

                state.ViewModel.CostCenterCode = {};
                state.ViewState.ShowCostCenterCode = true;
                return state;
            case 'ACC_ADD_COST_CENTER_VALUES':
                var newPercentage = parseFloat(actions.percentage);
                state.ViewModelExtra.TotalPercentage = 0;
                state.ViewModelExtra.message = "";
                state.ViewState.ShowCostCenterCode = true;
                if (isEmpty(actions.costCenterId)) {
                    state.ViewModelExtra.message = Resources.SelectCostCenter;
                    return state;
                }

                if (isEmptyorZero(actions.percentage)) {
                    state.ViewModelExtra.message = Resources.EnterPercentage;
                    return state;
                }

                var index = -1;
                var percentage = 0;
                $.each(state.ViewModel.AccountingCostCenters, function (i, value) {
                    if (parseInt(value.CostCenter.CostCenterId) == parseInt(actions.costCenterId)) {
                        index = i;
                    } else {
                        percentage = percentage + parseFloat(value.Percentage);
                    }
                });

                if (percentage + newPercentage > 100) {
                    state.ViewModel.CostCenterCode.Percentage = newPercentage;
                    state.ViewModelExtra.message = Resources.ValidateCostCenterPercentage;
                    return state;
                }
                state.ViewModel.CostCenterCode = {
                    CostCenter: { CostCenterId: actions.costCenterId, Description: actions.costCenterDescription },
                    Percentage: newPercentage,
                };

                if (index >= 0) {
                    state.ViewModel.AccountingCostCenters[index] = state.ViewModel.CostCenterCode;
                } else {
                    state.ViewModel.AccountingCostCenters.push(Object.assign({}, state.ViewModel.CostCenterCode));
                }


                $.each(state.ViewModel.AccountingCostCenters, function (index, value) {
                    state.ViewModelExtra.TotalPercentage += parseFloat(value.Percentage);
                });
                state.ViewModel.CostCenterCode = [];
                return state;

            case 'ACC_LOAD_EDIT_COST_CENTER':
                state.ViewModel.CostCenterCode = {
                    CostCenter: { CostCenterId: actions.LoadApplicationAccountingCostCenter.CostCenter.CostCenterId, Description: actions.LoadApplicationAccountingCostCenter.CostCenter.Description },
                    Percentage: actions.LoadApplicationAccountingCostCenter.Percentage,
                };
                state.ViewState.LoadCostCenterVal = true;
                return state;
            case 'ACC_SET_VALUE_TO_SHOW':
                state.ViewState.ShowAnalysisCodes = false;
                state.ViewState.ClearConceptsKeys = false;
                state.ViewState.showMessageInfoNotify = false;
                return state;
            case 'ACC_CLEAN_CODE_ANALYSIS_COST_CENTER':
                state.ViewModel.AccountingAnalysisCodes = [];
                state.ViewModel.AccountingCostCenters = [];
                return state;

            case 'ACC_DELETE_CODE_ANALYSIS':
                $.each(state.ViewModel.AccountingAnalysisCodes, function (i, value) {
                    if (parseInt(value.AnalysisId) == parseInt(actions.gAnalysisCodeId)) {
                        state.ViewModel.AccountingCostCenters.splice(i, 1);
                        state.ViewState.ShowAnalysisCodes = true;
                        return state;
                    }
                });
                return state;

            case 'ACC_DELETE_COST_CENTER':
                var index = -1;
                $.each(state.ViewModel.AccountingCostCenters, function (i, value) {
                    if (parseInt(value.CostCenter.CostCenterId) == parseInt(actions.CostCenterId)) {
                        index = i;
                        return;
                    }
                });
                if (index >= 0) {
                    state.ViewModel.AccountingCostCenters.splice(index, 1);
                    state.ViewState.ShowCostCenterCode = true;
                }
                return state;

            case 'ACC_CLEAN_SEARCH_PERSON':
                state.ViewModel.AccountingDocumentNumber = "";
                state.ViewModel.AccountingName = "";
                state.ViewModel.accountingAgentDocumentNumber = "";
                state.ViewModel.accountingAgentName = "";
                state.ViewModel.accountingCompanyDocumentNumber = "";
                state.ViewModel.accountingCompanyName = "";
                state.ViewModel.accountingInsuredDocumentNumber = "";
                state.ViewModel.accountingInsuredName = "";
                state.ViewModel.accountingReinsuranceDocumentNumber = "";
                state.ViewModel.accountingReinsuranceName = "";
                state.ViewModel.accountingSupplierDocumentNumber = "";
                state.ViewModel.accountingtSupplierName = "";
                state.ViewModel.Beneficiary.IndividualId = "";
                state.ViewModel.Beneficiary.Name = "";
                state.ViewModel.Beneficiary.IdentificationDocument.Number = "";
                state.ViewModel.Branch.Id = actions.branchId;
                state.ViewState.DefaultBranch = actions.branchId;
                state.ViewState.LoadBranches = true;
                state.Imputation = new Array();
                return state;
            case 'ACC_SET_SEARCH_TYPE':
                state.ViewState.ShowSelectSearch = true;
                state.ViewState.AgentSearch = false;
                state.ViewState.PersonSearch = false;
                state.ViewState.InsuredSearch = false;
                state.ViewState.SearchPersonSection = false;
                state.ViewState.CoinsuranceSearch = false;
                state.ViewState.ReinsuranceSearch = false;
                state.ViewState.SupplierSearch = false;
                let selectedItemId = actions.selectedItemSearchById;
                state.ViewModel.SelectedItemSearchById = parseInt(selectedItemId);
                if (selectedItemId > 0) {
                    state.ViewState.SearchPersonSection = true;
                    if (selectedItemId == $("#ViewBagInsuredCode").val()) {
                        state.ViewState.InsuredSearch = true;
                    } else if (selectedItemId == $("#ViewBagOthersCode").val() ||
                        selectedItemId == $("#ViewBagCollectorCode").val() ||
                        selectedItemId == $("#ViewBagTradeAdviserCode").val()) {
                        state.ViewState.PersonSearch = true;
                    } else if (selectedItemId == $("#ViewBagAgentCode").val()) {
                        state.ViewState.AgentSearch = true;
                    } else if (selectedItemId == $("#ViewBagCoinsurerCode").val()) {
                        state.ViewState.CoinsuranceSearch = true;
                    } else if (selectedItemId == $("#ViewBagReinsurerCode").val()) {
                        state.ViewState.ReinsuranceSearch = true;
                    } else if (selectedItemId == $("#ViewBagSupplierCode").val()) {
                        state.ViewState.SupplierSearch = true;
                    }
                }
                return state;
            default: return state;
        }
    }

    static getCostCenterById(costCenters, costCenterId) {
        var costCenter = {};
        $.each(costCenters, function (i, value) {
            if (costCenterId == value.CostCenterId) {
                costCenter = value;
                return;
            }
        });
        return costCenter;
    }

    static isEmpty(text) {
        return (text == undefined || text == null || (typeof text === "string" && text.trim() == ""));
    }

    static isNull(obj) {
        return (obj == undefined || obj == null || obj === "");
    }

    static PrepareRequest(obj) {
        for (var k in obj) {
            if (Array.isArray(obj[k]) && obj[k].length > 0) {
                for (var i = 0; i < obj[k].length; i++) {
                    obj[k][i] = AccountingRedux.PrepareRequest(obj[k][i]);
                }
            }
            else if (typeof obj[k] === 'object') {
                obj[k] = AccountingRedux.PrepareRequest(obj[k]);
            }
            else if (typeof obj[k] === 'number' || (typeof obj[k] === 'string' && /^(\d+)\.(\d+)$/.test(obj[k]))) {
                obj[k] = ReplaceDecimalPoint(obj[k]);
            }
        }
        return obj;
    }

}

class AccountingActioning {

    static actionCreators(getState, dispatch) {

        $('#AccountingAnalysisConcept').on('itemSelected', function (event, selectedItem) {
            $("#analysisCodeAlert").UifAlert('hide');            
            if (selectedItem.Id > 0 && selectedItem.Id != "") {
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadAnalysisCode', value: false });
                dispatch({ type: 'ACC_SET_ANALYSIS_CONCEPT', selectedItem: selectedItem });

                AccountingRequest.GetConceptKeysByAnalysisConceptId(selectedItem.Id)
                    .done(function (data) {
                        $('#DynamicFields').html("");
                        $.each(data, function (index, value) {
                            var ColumnDescription = value.ColumnDescription.toLowerCase();
                            $('#DynamicFields').append('<div class="uif-col-12">' +
                                '<br />' +
                                '<label>' + ColumnDescription.replace(/\b[a-z]/g, c => c.toUpperCase()) + '</label>' +
                                '<br />' +
                                '<input id="' + value.ColumnName + '"' + 'name="' + value.ColumnName.replace(" ", "_") + "_" +
                                value.ColumnDescription.replace(" ", "_") + '"' + 'type="Text"' + "/>"
                                + '</div> ')
                        });
                        dispatch({ type: 'ACC_SET_VALUE', label: 'LoadAnalysisValidate', value: false });
                        dispatch({ type: 'ACC_SET_KEYS_VALUES', data: data });
                        dispatch({ type: 'ACC_SET_PREMIUM_DEPOSIT', selectedItem: selectedItem });
                        dispatch({ type: 'ACC_SET_VALUE', label: 'AlertParametrizerCodeAnalysis', value: false });
                    });
            }
            else {
                $('#DynamicFields').html("");
            }
        });

        $('#AddAccountingAnalysisMovement').on("click", function () {
            var conceptKeys = getState.ViewModel.ConceptKeys;

            var hasValidationError = false;
            if (conceptKeys.length > 0) {
                $.each(conceptKeys, function (index, value) {
                    if (isEmpty($("#" + value.ColumnName).val())) {
                        hasValidationError = true;
                        dispatch({ type: 'ACC_SET_NOTIFY_MESSAGE', Message: Resources.AnalysisCodeAdd });
                        dispatch({ type: 'ACC_SET_VALUE', label: 'showMessageInfoNotify', value: false });
                    }

                });
                if (!hasValidationError) {
                    var msg = "";
                    $.each(conceptKeys, function (index, value) {
                        AccountingRequest.CheckoutAnalysisCodeByAnalysisConceptKeyId(value.Id, $("#" + value.ColumnName).val()).done(function (data) {
                            msg = data.Info;
                            if (data.Success != true) {
                                hasValidationError = true;
                                return;
                            }
                        });
                        if (hasValidationError) {
                            return;
                        }
                    });
                    if (hasValidationError) {
                        dispatch({ type: 'ACC_SET_NOTIFY_MESSAGE', Message: msg });
                        dispatch({ type: 'ACC_SET_VALUE', label: 'showMessageInfoNotify', value: false });
                    } else {
                        dispatch({ type: 'ACC_ADD_KEYS_VALUES', data: conceptKeys, accountingAnalysisConceptDescription: $('#AccountingAnalysisConcept').UifSelect('getSelectedText'), accountingAnalysisDescription: $('#AccountingAnalysisDescription').val() });
                        dispatch({ type: 'ACC_SET_VALUE_TO_SHOW', value: false });
                    }
                }
            }
            else {
                dispatch({ type: 'ACC_SET_NOTIFY_MESSAGE', Message: Resources.AnalysisParametizer });
                dispatch({ type: 'ACC_SET_VALUE', label: 'showMessageInfoNotify', value: false });
            }
        });

        $('#CancelAddAccountingAnalysisMovement').on("click", function () {
            dispatch({ type: 'ACC_CLEAN_ANALYSIS' });
            dispatch({ type: 'ACC_SET_VALUE', label: 'selectedItemConcepts', value: false });
        });

        $("#AddAccountingCostCenterMovement").on('click', function () {
            $("#costCenterAlert").UifAlert('hide');
            dispatch({ type: 'ACC_ADD_COST_CENTER_VALUES', costCenterId: $("#AccountingCostCenter").UifSelect('getSelected'), costCenterDescription: $("#AccountingCostCenter").UifSelect('getSelectedText'), percentage: $("#AccountingPercentageAmount").val(), movementsCostCenter: $('#CostCenterList').UifListView("getData") });
            dispatch({ type: 'ACC_SET_VALUE', label: 'ShowCostCenterCode', value: false });

        })


        $('#AccountingBranch').on('itemSelected', function (event, selectedItem) {

            if (selectedItem.Id > 0) {
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadBranches', value: false });
                dispatch({ type: 'ACC_SET_BRANCH', selectedItem: selectedItem });

                AccountingRequest.GetSalesPointByBranchId(selectedItem.Id).done(function (data) {
                    dispatch({ type: 'ACC_LOAD_SALES_POINTS', Bags: { SalesPoints: data.data } });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'LoadSalesPoint', value: false });
                });
            }
        });

        $("#AccoutingNature").on('itemSelected', function (event, selectedItem) {
            $("#AccountingCurrency").removeAttr("disabled");
            if (selectedItem.Id > 0) {
                dispatch({ type: 'ACC_SET_NATURE', selectedItem: selectedItem });
            }
        });

        $("#AccountingSalePoint").on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                dispatch({ type: 'ACC_SET_SALEPOINT', selectedItem: selectedItem });
            }
        });

        $('#accountingPostdatedList').on('rowAdd', function (event, data) {
            dispatch({
                type: 'ACC_POSTDATED_ADD', postdated: {
                    ValueTypeId: $("#PostdatedValueType").val(),
                    ValueTypeDescription: $("#PostdatedValueType option:selected").text(),
                    ValueNumberId: 0,
                    ValueNumberDescription: $("#PostdatedValueNumber").val(),
                    Amount: $("#PostdatedAmount").val()
                }
            });
        });

        $('#accountingPostdatedList').on('rowDelete', function (event, data, index) {
            dispatch({
                type: 'ACC_POSTDATED_DELETE', index: index
            });
            event.resolve();
        });

        $('#accountingPostdatedList').on('rowDelete', function (event, data, index) {
            event.resolve();
        });


        $('#accountingMovementsAnalysisCode').on('rowDelete', function (event, data, index) {
            event.resolve();
        });

        $("#AccountingAnalysisCode").on('itemSelected', function (event, selectedItem) {
            $("#analysisCodeAlert").UifAlert('hide');
            if (selectedItem.Id > 0) {
                AccountingActioning.loadAnalysisConceptByAnalyisId(dispatch, selectedItem);
                dispatch({ type: 'ACC_SET_VALUE', label: 'AlertParametrizerCodeAnalysis', value: false });
            }
        });

        $("#AccountingCostCenter").on('itemSelected', function (event, selectedItem) {

        });

        //autocomplete por número de documento
        $('#AccountingDocumentNumber').on('itemSelected', function (event, selectedItem) {
            console.log('AccountingDocumentNumber itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: 'ACC_SET_ACCOUNTING_DOCUMENT', selectedItem: selectedItem });
            }
        });

        //autocomplete por nombre
        $('#AccountingName').on('itemSelected', function (event, selectedItem) {
            console.log('AccountingName itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: '-', selectedItem: selectedItem });
            }
        });

        $('#accountingAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
            console.log('accountingAgentDocumentNumber itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: 'ACC_SET_ACCOUNTING_DOCUMENT', selectedItem: selectedItem });
            }
        });

        //autocomplete por nombre
        $('#accountingAgentName').on('itemSelected', function (event, selectedItem) {
            console.log('accountingAgentName itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: '-', selectedItem: selectedItem });
            }
        });


        $('#accountingCompanyDocumentNumber').on('itemSelected', function (event, selectedItem) {
            console.log('accountingCompanyDocumentNumber itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: 'ACC_SET_ACCOUNTING_DOCUMENT', selectedItem: selectedItem });
            }
        });

        //autocomplete por nombre
        $('#accountingCompanyName').on('itemSelected', function (event, selectedItem) {
            console.log('accountingCompanyName itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: '-', selectedItem: selectedItem });
            }
        });


        $('#accountingInsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
            console.log('accountingInsuredDocumentNumber itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: 'ACC_SET_ACCOUNTING_DOCUMENT', selectedItem: selectedItem });
            }
        });

        //autocomplete por nombre
        $('#accountingInsuredName').on('itemSelected', function (event, selectedItem) {
            console.log('accountingInsuredName itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: '-', selectedItem: selectedItem });
            }
        });

        $('#accountingReinsuranceDocumentNumber').on('itemSelected', function (event, selectedItem) {
            console.log('accountingReinsuranceDocumentNumber itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: 'ACC_SET_ACCOUNTING_DOCUMENT', selectedItem: selectedItem });
            }
        });

        //autocomplete por nombre
        $('#accountingReinsuranceName').on('itemSelected', function (event, selectedItem) {
            console.log('accountingReinsuranceName itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: '-', selectedItem: selectedItem });
            }
        });

        $('#accountingSupplierDocumentNumber').on('itemSelected', function (event, selectedItem) {
            console.log('accountingSupplierDocumentNumber itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: 'ACC_SET_ACCOUNTING_DOCUMENT', selectedItem: selectedItem });
            }
        });

        //autocomplete por nombre
        $('#accountingtSupplierName').on('itemSelected', function (event, selectedItem) {
            console.log('accountingtSupplierName itemSelected');
            console.log(selectedItem);

            if (selectedItem != null) {
                dispatch({ type: '-', selectedItem: selectedItem });
            }
        });
        //autocomplete por código de concepto
        $('#AccountingPaymentConceptId').on('itemSelected', function (event, selectedItem) {
            AccountingActioning.loadAccount(getState, dispatch, selectedItem);
        });

        //autocomplete por nombre de concepto
        $('#AccountingPaymentConceptDescription').on('itemSelected', function (event, selectedItem) {
            AccountingActioning.loadAccount(getState, dispatch, selectedItem);
        });

        //autocomplete por número de cuenta
        $('#AccountingAccountNumber').on('itemSelected', function (event, selectedItem) {
            AccountingActioning.loadAccount(getState, dispatch, selectedItem);
        });

        //autocomplete por nombre de cuenta
        $('#AccountingAccountName').on('itemSelected', function (event, selectedItem) {
            AccountingActioning.loadAccount(getState, dispatch, selectedItem);
        });

        //control de borrado de autocomplete en campo de nombre de concepto de pago.
        $("#AccountingPaymentConceptDescription").on('blur', function (event) {
            setTimeout(function () {
                dispatch({ type: 'ACC_SET_VALUE', label: 'ShowPaymentConceptDescription', value: true });
                dispatch({ type: 'ACC_SET_VALUE', label: 'ShowPaymentConceptDescription', value: false });
            }, 50);
        });

        $("#AccountingAnalysisDescription").on('blur', function (event) {
            setTimeout(function () {
                dispatch({ type: 'ACC_SET_VALUE', label: 'ShowAnalysisDescription', value: true });
                dispatch({ type: 'ACC_SET_VALUE', label: 'ShowAnalysisDescription', value: false });
            });
        });

        //cambio en el dropdown de moneda
        $('#AccountingCurrency').on('itemSelected', function (event, selectedItem) {
            if (!AccountingRedux.isNull(selectedItem)) {
                if (selectedItem.Id >= 0) {
                    dispatch({ type: 'ACC_SET_CURRENCY', selectedItem: selectedItem });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: true });
                    AccountingRequest.GetCurrencyExchangeRate($("#ViewBagDateAccounting").val(), selectedItem.Id).done(function (data) {
                        let response = {
                            rate: data,
                            alert: true
                        };
                        dispatch({ type: 'ACC_SET_RATE_DATA', accountingExchangeRate: response.rate, showAlert: response.alert });
                    }).always(function () {
                        dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: false });
                    });
                }
            }
        });

        //formato de moneda en importe de contabilidad
        $("#AccountingDescription").on('blur', function () {
            dispatch({ type: 'ACC_SET_ACCOUNTING_DECRIPTION', Description: $("#AccountingDescription").val().trim() });
        });

        //formato de moneda en importe de contabilidad
        $("#AccountingAmount").on('blur', function () {
            dispatch({ type: 'ACC_SET_ACCOUNTING_AMOUNT', Amount: RemoveFormatMoney($("#AccountingAmount").val().trim()) });
        });

        //añadir movimiento contable
        $("#AddAccountingMovement").on('click', function () {
            if (RemoveFormatMoney($("#AccountingAmount").val()) > 0) {
                $("#AccountingAmount").removeAttr("min");
            }
            else {
                $("#AccountingAmount").attr("min", 0);
            }

            $("#addAccountingMovementForm").validate();

            if ($("#addAccountingMovementForm").valid()) {
                if (getState.ViewModelExtra.RequiresAnalysis && getState.ViewModel.AccountingAnalysisCodes.length === 0) {
                    $.UifNotify('show', { type: 'info', message: Resources.AnalysisRequired, autoclose: true });
                    return;
                }
                if (getState.ViewModelExtra.RequiresCostCenter && getState.ViewModel.AccountingCostCenters.length === 0) {
                    $.UifNotify('show', { type: 'info', message: Resources.RequiresCostCenter, autoclose: true });
                    return;
                }
                dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: true });
                let request = getState.ViewModel;
                request.ApplicationAccountingId = getState.TempApplicationId;
                AccountingRequest.SaveTempAccountingTransactionRequest(AccountingRedux.PrepareRequest(request)).done(function (data) {
                    if (data > 0) {
                        AccountingActioning.LoadAccountingMovements(getState, dispatch);

                        dispatch({ type: 'ACC_LOAD', Bags: { Currencies: getState.Bags.OriginalCurrencies } });
                        dispatch({ type: 'ACC_SET_VALUE', label: 'LoadCurrencies', value: false });
                    }
                }).always(function () {
                    dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: false });
                });
            }
        });

        $("#accountingMovementsList").on('rowDelete', function (event, data, index) {
            AccountingActioning.deleteAccountingMovement(data, getState, dispatch);
        });

        $("#AnalysisCodesList").on('rowDelete', function (event, data, index) {
            dispatch({ type: 'ACC_DELETE_CODE_ANALYSIS', indexTodelete: index });
            dispatch({ type: 'ACC_SET_VALUE', label: 'ShowAnalysisCodes', value: false });
        });

        $("#CostCenterList").on('rowDelete', function (event, data, index) {
            dispatch({ type: 'ACC_DELETE_COST_CENTER', CostCenterId: data.CostCenter.CostCenterId });
            dispatch({ type: 'ACC_SET_VALUE', label: 'ShowCostCenterCode', value: false });
        });

        $("#accountingMovementsList").on('rowEdit', function (event, data, index) {
            AccountingActioning.editingMovement(data, getState, dispatch);
            AccountingActioning.editingAnalysisTemporal(data, getState, dispatch);
            AccountingActioning.editingCostCenterTemporal(data, getState, dispatch);
        });

        $("#AnalysisCodesList").on('rowEdit', function (event, data, index) {
            AccountingActioning.editingAnalysis(data, dispatch)
        });

        $("#CostCenterList").on('rowEdit', function (event, data, index) {
            AccountingActioning.editingCostCenter(data, index, dispatch);
        });

        $("#AccountingAcceptMovement").on('click', function () {
            dispatch({ type: 'ACC_CLEAN_CODE_ANALYSIS_COST_CENTER' });
            AccountingActioning.ClearAnalysisCodeFields();
            AccountingActioning.ClearCostCenterFields();
        });

        $(document).on('ready', function () {

            AccountingRequest.GetBranchs().done(function (data) {
                dispatch({ type: 'ACC_LOAD', Bags: { Branches: data.data } });
                dispatch({
                    type: 'ACC_SET_VALUES', values: [{ label: 'LoadBranches', value: false },
                    { label: 'LoadAnalysis', value: false }]
                });
            });

            AccountingRequest.GetNatures().done(function (data) {
                dispatch({ type: 'ACC_LOAD', Bags: { Natures: data.data } });
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadNatures', value: false });
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadAnalysis', value: false });
            });

            AccountingRequest.GetAccountingCompanies().done(function (data) {
                /*if (isMulticompany == 0)
                    $("#AccountingCompany").UifSelect({ sourceData: data.data, selectedId: idCompany, enable: false });
                else
                    $("#AccountingCompany").UifSelect({ sourceData: data.data });*/
                dispatch({ type: 'ACC_LOAD', Bags: { AccountingCompanies: data.data } });
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadCompanies', value: false });
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadAnalysis', value: false });
            });

            AccountingRequest.GetCurrencies().done(function (data) {
                dispatch({ type: 'ACC_LOAD', Bags: { Currencies: data.data } });
                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadCurrencies', value: false });
            });

            AccountingRequest.GetAnalysis().done(function (data) {
                if (!AccountingRedux.isNull(data.data) && Array.isArray(data.data)
                    && data.data.length > 0) {
                    dispatch({ type: 'ACC_LOAD_ANALYSIS', Bags: { Analysis: data.data } });
                    dispatch({
                        type: 'ACC_SET_VALUES', values: [{ label: 'LoadAnalysis', value: false },
                        { label: 'AlertParametrizerCodeAnalysis', value: false }]
                    });
                }
            });

            AccountingActioning.initialLoad(getState);
            AccountingActioning.loadMovements(getState, dispatch);
        });
    }

    static labelHide() {
        $("#AccountingMovementModal").find("#rowAccInsured").hide();
        $("#AccountingMovementModal").find("#rowAccPerson").hide();
        $("#AccountingMovementModal").find("#rowAccAgent").hide();
        $("#AccountingMovementModal").find("#rowAccCoinsurance").hide();
        $("#AccountingMovementModal").find("#rowAccReinsurance").hide();
        $("#AccountingMovementModal").find("#rowAccSupplier").hide();
    }

    static deleteAccountingMovement(data, getState, dispatch) {
        if (data != null) {
            dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: true });
            AccountingRequest.DeleteTempApplicationAccounting(data.Id)
                .done(function () {
                    AccountingActioning.loadMovements(getState, dispatch);
                })
                .always(function () {
                    dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: false });
                });
        }
    }

    static editingMovement(data, getState, dispatch) {
        AccountingRequest.GetTempAccountingTransactionByTempAccountingApplicationId(data.Id)
            .done(function (response) {
                if (response.Id > 0) {
                    dispatch({ type: 'ACC_EDIT_MOVEMENT', ApplicationAccounting: response });
                }
            });
    }
    static editingAnalysisTemporal(data, getState, dispatch) {
        AccountingRequest.GetTempApplicationAccountingAnalysisByTempAppAccountingId(data.Id)
            .done(function (response) {
                if (response.length > 0) {
                    dispatch({ type: 'ACC_EDIT_CODE_ANALYSIS', ApplicationAccountingCodeAnalysis: response });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'ShowAnalysisCodes', value: false });
                }
            });
    }
    static editingAnalysis(data, dispatch) {

        dispatch({ type: 'ACC_LOAD_CODE_ANALYSIS', LoadApplicationAccountingCodeAnalysis: data });
        dispatch({ type: 'ACC_SET_VALUE', label: 'LoadAnalysisCode', value: false });

        dispatch({ type: 'ACC_SET_LOAD_CODE_ANALYSIS', AnalysisCode: data });

    }

    static editingCostCenterTemporal(data, getState, dispatch) {
        AccountingRequest.GetTempApplicationAccountingCostCenterByTempAppAccountingId(data.Id)
            .done(function (response) {
                if (response.length > 0) {
                    dispatch({ type: 'ACC_EDIT_COST_CENTER', ApplicationAccountingCostCenter: response });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'ShowCostCenterCode', value: false });
                }
            });
    }
    static editingCostCenter(data, index, dispatch) {
        dispatch({ type: 'ACC_LOAD_EDIT_COST_CENTER', LoadApplicationAccountingCostCenter: data, costCenterIndex: index });
        dispatch({ type: 'ACC_SET_VALUE', label: 'LoadCostCenterVal', value: false });
    }

    static LoadAccountingMovements(getState, dispatch) {
        AccountingRequest.GetTempAccountingTransactionItemByTempApplicationId(getState.TempApplicationId).done(function (data) {
            dispatch({ type: 'ACC_CLEAN_CODE_ANALYSIS_COST_CENTER' });
            dispatch({ type: 'ACC_CLEAN' });
            dispatch({ type: 'ACC_LOAD_MOVEMENTS_LIST', Bags: { Movements: data } });
            dispatch({
                type: 'ACC_SET_VALUES', values: [{ label: 'LoadMovements', value: false },
                { label: 'LoadAnalysis', value: false },
                { label: 'ClearConceptsKeys', value: false },
                { label: 'Clear', value: false }]
            });
        }).always(function () {
            dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: false });
        });
    }

    static loadMovements(state, dispatch) {
        if (state.Persist === true) {
            if (!AccountingRedux.isNull(state.TempApplicationId)) {
                AccountingActioning.LoadAccountingMovements(state, dispatch);
            }
        } else {
            dispatch({ type: 'ACC_SET_VALUE', label: 'LoadMovements', value: true });
            dispatch({ type: 'ACC_SET_VALUE', label: 'LoadMovements', value: false });
        }
    }

    static loadPaymentConcept(dispatch, event, selectedItem) {
        dispatch({ type: 'ACC_SET_PAYMENT_CONCEPT', selectedItem: selectedItem });
        dispatch({
            type: 'ACC_SET_VALUES', values: [{ label: 'LoadCurrencies', value: false },
            { label: 'LoadExchangeRate', value: false },
            { label: 'LockScreen', value: false }]
        });

        AccountingRequest.IsBankReconciliation(selectedItem.GeneralLedgerId)
            .done(function (data) {
                if (data.result) {
                    dispatch({ type: 'ACC_SET_VALUE', label: 'ShowAccountingBankAccount', value: true });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'ShowAccountingBankAccount', value: false });
                }
            }).always(function () {
                dispatch({ type: 'ACC_SET_VALUE', label: 'LockScreen', value: false });
            });
    }

    static initialLoad(state) {
        $("#RowPolicy").hide();
        $("#RowBranch").hide();
        $("#RowPrefix").hide();
        $("#RowDocument").hide();
        $("#RowDocumentCompanyReinsure").hide();
        $("#AccountingPolicyId").hide();
        $("#RowDocument").hide();
        $("#AcconuntingBranchCode").hide();
        $("#AcconuntingPrefixCode").hide();
        $("#AcconuntingDocumentInsured").hide();
        $("#DocumentCompanyReinsure").hide();

        //Xhr de autocomplete de varios parametros
        $(document).ajaxSend(function (event, xhr, settings) {
            if (settings.url.indexOf("GetAccountingConcepstByFilter") != -1
                || settings.url.indexOf("GetAccountingAccountByNumber") != -1
                || settings.url.indexOf("GetPaymentConceptByDescription") != -1
                || settings.url.indexOf("GetAccountingAccountByDescription") != -1) {
                var paramCache = settings.url.split('&');
                var branch = $('#AccountingBranch').UifSelect('getSelected');
                if (AccountingRedux.isEmpty(branch)) {
                    branch = -1;
                }
                settings.url = paramCache[0] + '&filter=' + branch + '/0';

                var paramCache = settings.url.split('&');
                var branch = $('#AccountingBranch').UifSelect('getSelected');
                if (AccountingRedux.isEmpty(branch)) {
                    branch = -1;
                }
                settings.url = paramCache[0] + '&filter=' + branch + '/0';
                if (paramCache.length > 1) {
                    settings.url = settings.url + '&' + paramCache[1];
                }
            }
        });

        $("#AccountingBankAccount").hide();

        $("#accountingMovementsCostCenter").UifListView({
            autoHeight: true,
            theme: 'dark',
            source: null,
            customDelete: false, //true
            customEdit: true,
            add: false,
            edit: false, //true
            delete: true, //false
            addTemplate: '#costCenterAdd-template',
            displayTemplate: "#accountingCostCenter-display-template"
        });

        $("#accountingMovementsAnalysisCode").UifListView({
            autoHeight: true,
            theme: 'dark',
            source: null,
            customDelete: false, //true
            customEdit: true,
            add: false,
            edit: false, // true
            delete: true, //false
            addTemplate: '#analysisAdd-template',
            displayTemplate: "#accountingAnalysisCode-display-template"
        });

        //cuando se cierra el modal de edición
        $('#AccountingMovementModal').on('hidden.bs.modal', function () {
            $("#AcceptAccountingMovement").trigger('click');
        });


        $("#accountingPostdatedList").UifListView({
            autoHeight: true,
            theme: 'dark',
            source: null,
            customAdd: false,
            customDelete: false,
            customEdit: false,
            add: true,
            edit: false,
            delete: true,
            addTemplate: '#postdatedAdd-template',
            displayTemplate: "#accountingPostdatedList-display-template"
        });

        $("#accountingMovementsList").UifListView({
            height: 460,
            customDelete: true,
            customEdit: true,
            edit: true,
            delete: true,
            displayTemplate: "#accountingMovementsList-display-template"
        });
        $("#AnalysisCodesList").UifListView({
            height: 280,
            customDelete: true,
            customEdit: true,
            edit: true,
            delete: true,
            displayTemplate: "#AnalysisCodesList-display-template"
        });
        $("#CostCenterList").UifListView({
            height: 280,
            customDelete: true,
            customEdit: true,
            edit: true,
            delete: true,
            displayTemplate: "#CostCenterList-display-template"
        });
    }

    static CheckLoadedMovements() {
        var isLoaded;
        return loadedMovementsPromise = new Promise(function (resolve, reject) {
            time = setInterval(function () {
                var summaries = $("#listViewAplicationReceipt").UifListView("getData");
                if (summaries.length > 0) {
                    isLoaded = true;
                    resolve(isLoaded);
                }
            }, 3);
        });
    }

    static ShowAccountingForm() {
        //$("#tabAnalysisCode").hide();
        $("#tabCostCenter").hide();
        $("#tabAccounting").fadeIn();
        $("#AccountingMovementModal").find("#AccountingAcceptMovement").prop('disabled', false);
    }

    static ClearAnalysisCodeFields() {
        $("#AccountingMovementModal").find("#AccountingAnalysisConcept").UifSelect({ source: null });
        $("#AccountingMovementModal").find("#KeyFields").hide();
        $("#AccountingMovementModal").find("#AccountingAnalysisDescription").val("");
        $('#DynamicFields').html("");
    }

    static ClearCostCenterFields() {
        $("#AccountingMovementModal").find("#AccountingCostCenter").val("");
        $("#AccountingMovementModal").find("#AccountingPercentageAmount").val("");
    }

    static SetClearDocumentsField(state) {
        $("#AccountingDocumentNumber").UifAutoComplete('setValue', state.ViewModel.AccountingDocumentNumber);
        $("#AccountingName").UifAutoComplete('setValue', state.ViewModel.AccountingName);
        $("#accountingAgentDocumentNumber").UifAutoComplete('setValue', state.ViewModel.accountingAgentDocumentNumber);
        $("#accountingAgentName").UifAutoComplete('setValue', state.ViewModel.accountingAgentName);
        $("#accountingCompanyDocumentNumber").UifAutoComplete('setValue', state.ViewModel.accountingCompanyDocumentNumber);
        $("#accountingCompanyName").UifAutoComplete('setValue', state.ViewModel.accountingCompanyName);
        $("#accountingInsuredDocumentNumber").UifAutoComplete('setValue', state.ViewModel.accountingInsuredDocumentNumber);
        $("#accountingInsuredName").UifAutoComplete('setValue', state.ViewModel.accountingInsuredName);
        $("#accountingReinsuranceDocumentNumber").UifAutoComplete('setValue', state.ViewModel.accountingReinsuranceDocumentNumber);
        $("#accountingReinsuranceName").UifAutoComplete('setValue', state.ViewModel.accountingReinsuranceName);
        $("#accountingSupplierDocumentNumber").UifAutoComplete('setValue', state.ViewModel.accountingSupplierDocumentNumber);
        $("#accountingtSupplierName").UifAutoComplete('setValue', state.ViewModel.accountingtSupplierName);
    }

    static loadAccount(state, dispatch, selectedItem) {
        if (selectedItem != null) {
            AccountingActioning.loadPaymentConcept(dispatch, event, selectedItem);

            AccountingRequest.GetCostCenterByGeneralLedgerId(selectedItem.GeneralLedgerId).done(function (data) {
                if (data.success === true) {
                    if (Object.values(data)) {
                        dispatch({ type: 'ACC_LOAD_COST_CENTER', Bags: { CostCenter: data.result } });
                        dispatch({ type: 'ACC_SET_VALUE', label: 'LoadCostCenter', value: false });
                    }
                } else {
                    if (state.ViewModelExtra.RequiresCostCenter === true) {
                        $.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
                    }
                }
            });
            if (selectedItem.AnalysisId > 0) {
                let selectItem = { Id: selectedItem.AnalysisId };
                AccountingActioning.loadAnalysisConceptByAnalyisId(dispatch, selectItem);

            }
        }
    }

    static loadAnalysisConceptByAnalyisId(dispatch, selectedItem) {
        dispatch({ type: 'ACC_SET_ANALYSIS', selectedItem: selectedItem });
        AccountingRequest.GetAnalysisConceptByAnalysisId(selectedItem.Id).done(function (data) {
            dispatch({ type: 'ACC_LOAD', Bags: { AnalysisConcepts: data.data } });
        });
        dispatch({
            type: 'ACC_SET_VALUES', values: [{ label: 'LoadAnalysisConcepts', value: false },
            { label: 'selectedItemConcepts', value: false }, { label: 'selectedItemAnalysis', value: false }]
        });
    }

    static outputState(state, dispatch) {

        if (state.ViewState.ShowAnalysisCodes === true) {
            $("#AnalysisCodesList").UifListView("clear");
            if (!isEmptyArray(state.ViewModel.AccountingAnalysisCodes)) {                
                for (var i = 0; i < state.ViewModel.AccountingAnalysisCodes.length; i++) {
                    $("#AnalysisCodesList").UifListView("addItem", state.ViewModel.AccountingAnalysisCodes[i]);
                }
            } 
        }
        if (state.ViewState.ShowCostCenterCode === true) {
            $("#CostCenterList").UifListView("clear");
            if (!isEmptyArray(state.ViewModel.AccountingCostCenters)) {                
                for (var i = 0; i < state.ViewModel.AccountingCostCenters.length; i++) {
                    $("#CostCenterList").UifListView("addItem", state.ViewModel.AccountingCostCenters[i]);
                }
                $("#TotalPercentage").text(state.ViewModelExtra.TotalPercentage);
                $("#AccountingCostCenter").val(state.ViewModel.CostCenterCode.CostCenterId);
                $("#AccountingPercentageAmount").val(state.ViewModel.CostCenterCode.Percentage);
            } 
            if (!isEmpty(state.ViewModelExtra.message)) {
                $("#costCenterAlert").UifAlert('show', state.ViewModelExtra.message, "warning");
            }

        }
        if (state.ViewState.LoadCompanies === true) {
            $("#AccountingCompany").UifSelect({ sourceData: state.Bags.AccountingCompanies });
        }
        if (state.ViewState.LoadBranches === true) {
            $('#AccountingBranch').UifSelect({ sourceData: state.Bags.Branches });
            $('#AccountingBranch').trigger('itemSelected', state.ViewModel.Branch);
        }
        if (state.ViewState.LoadNatures === true) {
            $("#AccoutingNature").UifSelect({ sourceData: state.Bags.Natures });
        }
        if (state.ViewState.LoadCurrencies === true) {
            $("#AccountingCurrency").UifSelect({ sourceData: state.Bags.Currencies });
        }

        if (state.ViewState.LoadAnalysis === true) {
            $("#AccountingAnalysisCode").UifSelect({ sourceData: state.Bags.Analysis });
        }

        if (state.ViewState.LoadCostCenter === true) {
            $("#AccountingCostCenter").UifSelect({ sourceData: state.Bags.CostCenter });
        }

        if (state.alert === false) {
            $("#alertPaymentOrder").UifAlert('show', Resources.ExchageRateNotUpToDate, "warning");
        }

        if (state.ViewState.LockScreen === true) {
            lockScreen();
        } else {
            unlockScreen();
        }

        if (!AccountingRedux.isEmpty(state.ViewModel.AccountingConcept.Id)) {
            $("#AccountingPaymentConceptId").UifAutoComplete('setValue', state.ViewModel.AccountingConcept.Id.toString());
        } else {
            $("#AccountingPaymentConceptId").UifAutoComplete('setValue', null);
        }
        if (!AccountingRedux.isEmpty(state.ViewModel.AccountingConcept.Description)) {
            $("#AccountingPaymentConceptDescription").UifAutoComplete('setValue', state.ViewModel.AccountingConcept.Description.toString());
        } else {
            $("#AccountingPaymentConceptDescription").UifAutoComplete('setValue', null);
        }
        if (!AccountingRedux.isEmpty(state.ViewModel.BookAccount.AccountNumber)) {
            $("#AccountingAccountNumber").UifAutoComplete('setValue', state.ViewModel.BookAccount.AccountNumber.toString());
        } else {
            $("#AccountingAccountNumber").UifAutoComplete('setValue', null);
        }
        if (!AccountingRedux.isEmpty(state.ViewModel.BookAccount.AccountName)) {
            $("#AccountingAccountName").UifAutoComplete('setValue', state.ViewModel.BookAccount.AccountName.toString());
        } else {
            $("#AccountingAccountName").UifAutoComplete('setValue', null);
        }
        if (!AccountingRedux.isEmpty(state.ViewModel.Beneficiary.Name)) {
            $('#AccountingName').UifAutoComplete('setValue', state.ViewModel.Beneficiary.Name.toString());
            $('#accountingAgentName').UifAutoComplete('setValue', state.ViewModel.Beneficiary.Name.toString());
            $('#accountingCompanyName').UifAutoComplete('setValue', state.ViewModel.Beneficiary.Name.toString());
            $('#accountingInsuredName').UifAutoComplete('setValue', state.ViewModel.Beneficiary.Name.toString());
            $('#accountingReinsuranceName').UifAutoComplete('setValue', state.ViewModel.Beneficiary.Name.toString());
            $('#accountingtSupplierName').UifAutoComplete('setValue', state.ViewModel.Beneficiary.Name.toString());
        } else {
            AccountingActioning.SetClearDocumentsField(state);
        }
        if (!AccountingRedux.isEmpty(state.ViewModel.Beneficiary.IdentificationDocument.Number)) {
            $('#AccountingDocumentNumber').UifAutoComplete('setValue', state.ViewModel.Beneficiary.IdentificationDocument.Number.toString());
            $('#accountingAgentDocumentNumber').UifAutoComplete('setValue', state.ViewModel.Beneficiary.IdentificationDocument.Number.toString());
            $('#accountingCompanyDocumentNumber').UifAutoComplete('setValue', state.ViewModel.Beneficiary.IdentificationDocument.Number.toString());
            $('#accountingInsuredDocumentNumber').UifAutoComplete('setValue', state.ViewModel.Beneficiary.IdentificationDocument.Number.toString());
            $('#accountingReinsuranceDocumentNumber').UifAutoComplete('setValue', state.ViewModel.Beneficiary.IdentificationDocument.Number.toString());
            $('#accountingSupplierDocumentNumber').UifAutoComplete('setValue', state.ViewModel.Beneficiary.IdentificationDocument.Number.toString());
        } else {
            AccountingActioning.SetClearDocumentsField(state);
        }

        $("#AccountingAmount").val(FormatMoneySymbol(state.ViewModel.Amount.Value));
        $("#AccountingDescription").val(state.ViewModel.Description);

        if (state.ViewState.LoadSalesPoint === true) {
            $("#AccountingSalePoint").UifSelect({ sourceData: state.Bags.SalesPoints });
        }

        $("#AccoutingNature").UifSelect("setSelected", state.ViewModel.AccountingNature);
        $('#AccountingBranch').UifSelect("setSelected", state.ViewModel.Branch.Id);
        $("#AccountingSalePoint").UifSelect("setSelected", state.ViewModel.SalePoint.Id);
        $("#AccountingExchangeRate").val(state.ViewModel.ExchangeRate.SellAmount);
        $("#AccountingCurrency").UifSelect("setSelected", state.ViewModel.Amount.Currency.Id);
        if (state.ViewState.LoadExchangeRate === true) {
            state.ViewState.LoadExchangeRate = false;
            $('#AccountingCurrency').trigger('itemSelected', state.ViewModel.Amount.Currency);
        }

        if (state.ViewState.Clear === true) {
            $("#AnalysisCodesList").UifListView("clear");
            $("#CostCenterList").UifListView("clear");
        }

        if (state.ViewState.AddNewPosdated === true && state.ViewModel.Postdated != undefined
            && state.ViewModel.Postdated != null && state.ViewModel.Postdated.length > 0) {
            $('#accountingPostdatedList').UifListView("addItem", state.ViewModel.Postdated[state.ViewModel.Postdated.length - 1]);
        }

        if (state.ViewState.ClearConceptsControl === true) {
            $("#analysisCodeAlert").UifAlert('hide');
            $("#KeyFields").html("");
        }

        if (state.ViewState.LoadAnalysisConcepts === true) {
            $("#AccountingAnalysisConcept").UifSelect({ sourceData: state.Bags.AnalysisConcepts });
        }

        if (state.ViewState.selectedItemConcepts === true) {
            $('#AccountingAnalysisConcept').UifSelect("setSelected", state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId);
            $('#AccountingAnalysisDescription').val(state.ViewModel.DescriptionCodeAnalysis);
            $('#AccountingAnalysisConcept').trigger("change");
        }
        if (state.ViewState.selectedItemAnalysis === true) {
            $("#AccountingAnalysisCode").UifSelect("setSelected", state.ViewModel.AnalysisCode.AnalysisId);
        }
        if (state.ViewState.HideCostCenter === true) {
            $("#costCenterAlert").UifAlert('hide');
        }

        $("#TotalAccountingMovement").text(FormatMoneySymbol(state.ViewModelExtra.TotalCreditsMovements));
        $("#TotalDebitAccountingMovement").text(FormatMoneySymbol(state.ViewModelExtra.TotalDebitMovements));

        if (state.ViewState.LoadMovements === true && !AccountingRedux.isNull(state.Movements)) {
            state.ViewState.LoadMovements = false;
            $("#accountingMovementsList").UifListView("clear");
            if (state.Movements.length > 0) {                
                for (var i = 0; i < state.Movements.length; i++) {
                    $("#accountingMovementsList").UifListView("addItem", state.Movements[i]);
                }
            } 
        }

        if (state.ViewModelExtra.RequiresAnalysis === true) {
            $("#analysisCode").prop('disabled', false);
        }

        if (state.ViewState.showMessageInfoNotify == true) {
            $.UifNotify('show', { type: 'info', message: state.ViewModelExtra.MessageInfo, autoclose: true });
        }

        if (state.ViewState.showMessageDangerNotify == true) {
            $.UifNotify('show', { type: 'Danger', message: state.ViewModelExtra.MessageInfo, autoclose: true });
        }

        if (state.ViewState.LoadAnalysisCode == true) {
            $('#AccountingAnalysisCode').UifSelect("setSelected", state.ViewModel.AnalysisCode.AnalysisId);
            $('#AccountingAnalysisConcept').UifSelect("setSelected", state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId);
            $('#AccountingAnalysisDescription').val(state.ViewModel.DescriptionCodeAnalysis);
            $('#AccountingAnalysisConcept').trigger("change");
        }
        if (state.ViewState.LoadCostCenterVal == true) {
            $("#AccountingCostCenter").val(state.ViewModel.CostCenterCode.CostCenter.CostCenterId);
            $("#AccountingPercentageAmount").val(state.ViewModel.CostCenterCode.Percentage);
        }

        if (state.ViewState.ClearConceptsKeys == true) {
            $('#DynamicFields').html("");
            $('#AccountingAnalysisCode').UifSelect("setSelected", state.ViewModel.AnalysisCode.AnalysisId);
            $('#AccountingAnalysisConcept').UifSelect("setSelected", state.ViewModel.AnalysisCode.AnalysisConcept.AnalysisConceptId);
            $('#AccountingAnalysisDescription').val("");
            $('#analysisCodeAlert').hide();
        }

        if (state.ViewState.IsClearDocumentsField == true) {
            AccountingActioning.SetClearDocumentsField(state);
            AccountingActioning.labelHide();
        }

        if (state.ViewState.ShowSelectSearch === true) {
            if (state.ViewState.SearchPersonSection) {
                AccountingActioning.labelHide();
                if (state.ViewState.InsuredSearch) {
                    $("#AccountingMovementModal").find("#rowAccInsured").show();
                } else if (state.ViewState.PersonSearch) {
                    $("#AccountingMovementModal").find("#rowAccPerson").show(state.ViewState.PayerSearch);
                } else if (state.ViewState.AgentSearch) {
                    $("#AccountingMovementModal").find("#rowAccAgent").show();
                } else if (state.ViewState.CoinsuranceSearch) {
                    $("#AccountingMovementModal").find("#rowAccCoinsurance").show();
                } else if (state.ViewState.ReinsuranceSearch) {
                    $("#AccountingMovementModal").find("#rowAccReinsurance").show();
                } else if (state.ViewState.SupplierSearch) {
                    $("#AccountingMovementModal").find("#rowAccSupplier").show();
                }
                state.ViewState.ShowSelectSearch = false;
            } else {
                $("#AccountingMovementModal").find("#rowAccInsured").show();
            }
        }
    }
}