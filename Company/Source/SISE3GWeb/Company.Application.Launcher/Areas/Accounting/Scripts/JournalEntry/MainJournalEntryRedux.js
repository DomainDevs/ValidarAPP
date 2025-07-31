
const mainJournalEntryRedux = (state, actions) => {

    state = state || {
        Request: {
            AutomaticReceiptApplication: {}, FrmBill: {}
        },
        ViewState: { SaveBill: false, LockScreen: false, ShowDebitsandCredits: false, ShowAccountingModal: false, HideModals: false, SetJournalText: false },
        ViewModel: { Technicaltransaction: null, TempApplicationId: null, JournalEntryId: null, BranchId: null, SalePointId: null },
        Bags: { Branches: [] }
    };

    switch (actions.type) {
        case 'MJE_ASSIGN_MOVEMENTS':
            state.ViewModel.DebitsandCredits = actions.Movements;

            if (!isNull(state.ViewModel.DebitsandCredits)
                && Array.isArray(state.ViewModel.DebitsandCredits)) {

                let totalDebits = 0;
                let totalCredits = 0;
                let total = 0;

                for (var i = 0; i < state.ViewModel.DebitsandCredits.length; i++) {
                    totalDebits += fix(RemoveFormatMoney(state.ViewModel.DebitsandCredits[i].Debits));
                    totalCredits += fix(RemoveFormatMoney(state.ViewModel.DebitsandCredits[i].Credits));
                }

                state.ViewModel.TotalData = {
                    TotalDebit: FormatMoneySymbol(fix(totalDebits)),
                    TotalCredit: FormatMoneySymbol(fix(totalCredits))
                };
                total = fix(fix(totalCredits) - fix(totalDebits));
                state.ViewModel.TotalControl = total;
            } else {
                state.ViewModel.TotalData = {
                    TotalDebit: FormatMoneySymbol(0),
                    TotalCredit: FormatMoneySymbol(0)
                };
                state.ViewModel.TotalControl = 0;
            }
            state.ViewState.ShowDebitsandCredits = true;
            return state;
        case 'MJE_SET_VALUE':
            state.ViewState[actions.label] = actions.value;
            return state;
        case 'MJE_TEMP_JOURNAL_ENTRY':
            state.ViewModel.JournalEntryId = actions.dataJournalEntryId;
            return state;
        case 'MJE_ASSIGN_TEMP_APPLICATION_ID':
            state.ViewModel.TempApplicationId = actions.TempImputationId;
            state.ViewState.SetJournalText = true;
            return state;
        case 'MJE_LOAD':
            if (!isNull(actions.Branches) && Array.isArray(actions.Branches) && actions.Branches.length > 0) {
                state.Bags.Branches = actions.Branches;
                if (!isEmptyorZero(actions.BranchId)) {
                    state.ViewModel.BranchId = actions.BranchId;
                }
                state.ViewState.LoadBranches = true;
            }
            else if (!isNull(actions.SalePoints) && Array.isArray(actions.SalePoints) && actions.SalePoints.length > 0) {
                state.Bags.SalePoints = actions.SalePoints;
                if (!isEmptyorZero(actions.SalePointId)) {
                    state.ViewModel.SalePointId = actions.SalePointId;
                }
                state.ViewState.LoadSalePoints = true;
            }
            return state;

        default: return state;
    }
};

class MainJournalEntryRedux {

    static isEmpty(text) {
        return (text == undefined || text == null || (typeof text === "string" && text.trim() == ""));
    }

    static isNull(obj) {
        return (obj == undefined || obj == null);
    }
    static SaveTempJournalEntryZero(dispatch)
    {
        MainJournalEntryRequest.SaveTempJournalEntry(0).done(function (dataJournalEntry) {
            dispatch({ type: 'MJE_TEMP_JOURNAL_ENTRY', dataJournalEntryId: dataJournalEntry.Id });
            MainJournalEntryRequest.GetTempImputationBySourceCode(Resources.ImputationTypeJournalEntry, dataJournalEntry.Id).done(function (dataSourceCode) {
                if (dataSourceCode.Id === 0) {
                    MainJournalEntryRequest.SaveTempApplication(Resources.ImputationTypeJournalEntry, dataJournalEntry.Id, $("#ViewBagIndividualId").val()).done(function (data) {
                        dispatch({ type: 'MJE_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: data.Id });
                        dispatch({ type: 'MJE_SET_VALUE', label: 'SetJournalText', value: false });
                        tempImputationId = data.Id; 
                    });
                }
                else
                {
                    dispatch({ type: 'MJE_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataJournalEntry.Id });
                    dispatch({ type: 'MJE_SET_VALUE', label: 'SetJournalText', value: false });
                    tempImputationId = data.Id;
                }
            });
        });
    }
    static LoadSaveTempJournalEntryByTempApplication(dispatch, getState) {
        MainJournalEntryRequest.SaveTempJournalEntry(0).done(function (dataJournalEntry) {
            dispatch({ type: 'MJE_TEMP_JOURNAL_ENTRY', dataJournalEntryId: dataJournalEntry.Id});
                    MainJournalEntryRequest.SaveTempApplication(Resources.ImputationTypeJournalEntry, dataJournalEntry.Id, 0).done(function (data) {
                        MainJournalEntryRedux.AssignTempApplicationId(getState, dispatch, data.Id);
                        dispatch({ type: 'MJE_SET_VALUE', label: 'SetJournalText', value: false });
                    });
        });
    }

    static AssignTempApplicationId(getState, dispatch, tempApplicaitonId) {
        dispatch({ type: 'ACC_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: tempApplicaitonId });
        dispatch({ type: 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: tempApplicaitonId });
        dispatch({ type: 'MJE_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: tempApplicaitonId });
        dispatch({ type: 'PREMPAY_ASSIGN_ACCOUNTING_DATE', AccountingDate: $("#ViewBagDateAccounting").val() });
        tempImputationId = tempApplicaitonId;
    }

    static actionCreators(getState, dispatch) {
        if ($("#ViewBagBranchDisable").val() == "1") {
            $("#BranchJournalEntry").attr("disabled", "disabled");
        } else {
            $("#BranchJournalEntry").removeAttr("disabled");
        }
        $(document).ready(function () {

            dispatch({ type: 'MJE_TEMP_JOURNAL_ENTRY', dataJournalEntryId: parseInt($("#ViewBagJournalId").val()) });
            MainJournalEntryRedux.AssignTempApplicationId(getState, dispatch, parseInt($("#ViewBagTempImputationId").val()));

            MainJournalEntryRequest.GetListBranchesbyUserName().done(function (data) {
                if (!isNull(data) && !isNull(data.data)) {
                    var defaultBranch = $('#ViewBagBranchUserDefault').val();
                    if (!isEmptyorZero($('#ViewBagBranchId').val())) {
                        defaultBranch = $('#ViewBagBranchId').val();
                    }
                    dispatch({ type: 'MJE_LOAD', Branches: data.data, BranchId: defaultBranch });
                    dispatch({ type: 'MJE_SET_VALUE', label: 'LoadBranches', value: false });
                }
            });
        });
        
        $("#BranchJournalEntry").on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'MJE_SET_VALUE', label: 'LoadBranches', value: false });
            if (selectedItem.Id > 0) {
                MainJournalEntryRequest.GetSalePointsJournalEntry(selectedItem.Id).done(function (data) {
                    if (!isNull(data) && !isNull(data.data)) {
                        var defaultSalePoint = $('#ViewBagSalePointId').val();
                        dispatch({ type: 'MJE_LOAD', SalePoints: data.data, SalePointId: defaultSalePoint });
                        dispatch({ type: 'MJE_SET_VALUE', label: 'LoadSalePoints', value: false });
                    }
                });
            }
        });

        $("#listViewAplicationReceipt").on('rowEdit', function (event, data, index) {
            if (!isNull(getState.ViewModel.TempApplicationId)) {
                // Primas por cobrar
                if (data.Id == 1) {
                    dispatch({ type: 'MJE_SET_VALUE', label: 'HideModals', value: false });
                    $('#ModalPremiums').UifModal('showLocal', Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + getState.ViewModel.TempApplicationId);

                }
                //// Contabilidad
                if (data.Id == 7) {
                    var branchId = $('#BranchJournalEntry').UifSelect('getSelected');
                    if (isEmptyorZero(branchId)) {
                        branchId = $('#ViewBagBranchUserDefault').val();
                    }
                    $("#addAccountingMovementForm").formReset(); //reseteo el formulario
                    dispatch({ type: 'ACC_CLEAN_SEARCH_PERSON', branchId: branchId });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'IsClearDocumentsField', value: true });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'IsClearDocumentsField', value: false });
                    dispatch({ type: 'ACC_SET_SEARCH_TYPE', selectedItemSearchById: $('#PayerTypeJournal').UifSelect('getSelected') });
                    dispatch({ type: 'MJE_SET_VALUE', label: 'HideModals', value: false });
                    $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + " " + Resources.DialogTitleTemporary + " " + getState.ViewModel.TempApplicationId);
                }
            }
        });
        $("#JournalAccept").click(function () {
            $("#alertJournaly").UifAlert('hide');
            $("#JournalyFormulario").validate();
            if (!$("#JournalyFormulario").valid()) {
                return;
            }
            if (getState.ViewModel.TotalControl != 0) {
                $.UifNotify('show', { type: 'danger', message: 'El total del asiento debe ser 0', autoclose: true });
                return;
            }
            if (RemoveFormatMoney(getState.ViewModel.TotalControl.TotalCredit) == 0
                || RemoveFormatMoney(getState.ViewModel.TotalControl.TotalDedit) == 0) {
                $.UifNotify('show', { type: 'danger', message: 'El asiento debe tener valores asociados', autoclose: true });
                return;
            }
            
            if (RemoveFormatMoney($("#TotalControl").val()) != 0) {
                //DIALOGO DE COMFIRMACION PARA GRABAR IMPUTATION EN TEMPORALES, ACTUALIZA ESTADO DE BILL
                SaveTemporalConfirmationReceipt();
            } else {
                lockScreen();
                $('#modalSuccess').find("#accountingMessageDiv").removeClass('error-accounting');

                MainJournalEntryRequest.UpdateTempJournalEntry(SetDataJournalEntry(statusApplied))
                    .done(function (updateData) {
                        if (updateData.success === true) {
                            MainJournalEntryRequest.SaveApplication(getState.ViewModel.TempApplicationId)
                                .done(function (data) {
                                    console.log(data);
                                    if (data.success == true) {

                                        if (data.IsEnabledGeneralLedger == false) {
                                            $("#accountingLabelDiv").hide();
                                            $("#accountingMessageDiv").hide();
                                        } else {
                                            $("#accountingLabelDiv").show();
                                            $("#accountingMessageDiv").show();
                                        }
                                        if (data.generalLedgerSuccess !== true) {
                                            $('#modalSuccess').find("#accountingMessageDiv").addClass('error-accounting');
                                        }
                                        $("#journalEntryId").val(data.sourceCode + " - " + Resources.TransactionNumber + " " + data.code);
                                        $('#modalSuccess').find("#accountingMessage").val(data.result);
                                        $('#modalSuccess').UifModal('showLocal', Resources.JournalEntrySaveSuccess);
                                        
                                        MainJournalEntryRedux.AssignTempApplicationId(getState, dispatch, null);
                                        SetDataJournalEntryEmpty();
                                        SaveTempJournalEntryZero();
                                        setDataFieldsEmptyJournalEntry();

                                    } else {
                                        $("#alertJournaly").UifAlert('show', data.result, "danger");
                                        $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
                                    }
                                })
                                .always(function () {
                                    unlockScreen();
                                });
                        } else {
                            $("#alertJournaly").UifAlert('show', data.result, "danger");
                            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
                        }
                    })
                    .always(function () {
                        unlockScreen();
                    });
            }
        });
         
        $('#SaveSearchPoliciesButton').on('click', function () {
            MainJournalEntryRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
            dispatch({ type: 'MJE_SET_VALUE', label: 'HideModals', value: true });
            dispatch({ type: 'MJE_SET_VALUE', label: 'HideModals', value: false });
        });
        $('#ModalPremiums').on('closed.modal', function () {
            MainJournalEntryRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
        });
        $('#AccountingAcceptMovement').on('click', function () {
            MainJournalEntryRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
            dispatch({ type: 'MJE_SET_VALUE', label: 'HideModals', value: true });
            dispatch({ type: 'MJE_SET_VALUE', label: 'HideModals', value: false });
        });
        $('#AccountingMovementModal').on('closed.modal', function () {
            MainJournalEntryRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
        });

    }
    //////////////////////////////////////////////////////
    /// Se refresca el listview resumen de movimientos ///
    //////////////////////////////////////////////////////
    static GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch) {
        //se utiliza el request de Mainapplication ya que este sirve para la misma funcionalidad
        MainJournalEntryRequest.GetDebitsAndCreditsMovementTypes(getState.ViewModel.TempApplicationId, 0)
            .done(function (data) {
                dispatch({ type: 'MJE_ASSIGN_MOVEMENTS', Movements: data });
                dispatch({ type: 'MJE_SET_VALUE', label: 'ShowDebitsandCredits', value: false });
            });
    }
    static outputState(state, dispatch) {

        if (state.ViewState.LockScreen === true) {
            lockScreen();
        } else {
            unlockScreen();
        }
        
        if (state.ViewState.SetJournalText === true) {
            $("#_JournalEntryCode").val(state.ViewModel.TempApplicationId);
        }
        if (state.ViewState.ShowDebitsandCredits === true) {
            $("#listViewAplicationReceipt").UifListView('clear');
            if (!isNull(state.ViewModel.DebitsandCredits)
                && Array.isArray(state.ViewModel.DebitsandCredits)) {
                for (var i = 0; i < state.ViewModel.DebitsandCredits.length; i++) {
                    $("#listViewAplicationReceipt").UifListView("addItem", state.ViewModel.DebitsandCredits[i]);
                }
            }

            $("#totalAplicationTable").UifDataTable('clear');
            $("#totalAplicationTable").UifDataTable('addRow', state.ViewModel.TotalData);
            $('#TotalControl').val(FormatMoneySymbol(state.ViewModel.TotalControl));
        }
        if (state.ViewState.HideModals === true) {
            $('#AccountingMovementModal').UifModal('hide');
            $('#ModalPremiums').UifModal('hide');
        }

        if (state.ViewState.LoadBranches === true) {
            $("#BranchJournalEntry").UifSelect({ sourceData: state.Bags.Branches });
            $("#BranchJournalEntry").UifSelect('setSelected', state.ViewModel.BranchId);
            $("#BranchJournalEntry").trigger('change');
        }

        if (state.ViewState.LoadSalePoints === true) {
            $("#SalesPointJournal").UifSelect({ sourceData: state.Bags.SalePoints });
            $("#SalesPointJournal").UifSelect('setSelected', state.ViewModel.SalePointId);
        }
    }
}

var dialogSearchPoliciesRedux = DialogSearchPoliciesRedux.redux;
var accountingRedux = AccountingRedux.redux;
const reversionPremium = ReversionPremiumRedux.redux;
var reducer = Redux.combineReducers(
    { dialogSearchPoliciesRedux, accountingRedux, mainJournalEntryRedux, reversionPremium}
);

const store = Redux.createStore(reducer);

store.getState().accountingRedux.Persist = true;
store.getState().dialogSearchPoliciesRedux.Persist = true;
store.getState().reversionPremium.Persist = true;
DialogSearchPolicies.actionCreators(store.getState().dialogSearchPoliciesRedux, store.dispatch);
AccountingActioning.actionCreators(store.getState().accountingRedux, store.dispatch);
MainJournalEntryRedux.actionCreators(store.getState().mainJournalEntryRedux, store.dispatch);

store.subscribe(() => {
    var state = store.getState();
    DialogSearchPolicies.outputState(state.dialogSearchPoliciesRedux, store.dispatch);
    AccountingActioning.outputState(state.accountingRedux, store.dispatch);
    MainJournalEntryRedux.outputState(state.mainJournalEntryRedux, store.dispatch);
});