const mainAppRecieptRedux = (state, actions) => {

    state = state || {
        Request: {
            AutomaticReceiptApplication: {}, FrmBill: {}
        },
        ViewState: {
            SaveBill: false, SetReceiptValues: false, LockScreen: false, ShowDebitsandCredits: false,
            HideModals: false, ClearTechnicalTransaction: false
        },
        ViewModel: { Technicaltransaction: null }
    };

    switch (actions.type) {
        case 'MAR_ASSIGN_MOVEMENTS':
            state.ViewModel.DebitsandCredits = actions.Movements;

            if (!isNull(state.ViewModel.DebitsandCredits)
                && Array.isArray(state.ViewModel.DebitsandCredits)) {

                let totalDebits = 0;
                let totalCredits = 0;
                let total = 0;

                for (var i = 0; i < state.ViewModel.DebitsandCredits.length; i++) {
                    totalDebits += RemoveFormatMoney(state.ViewModel.DebitsandCredits[i].Debits);
                    totalCredits += RemoveFormatMoney(state.ViewModel.DebitsandCredits[i].Credits);
                }

                state.ViewModel.TotalData = {
                    TotalDebit: FormatMoneySymbol(totalDebits),
                    TotalCredit: FormatMoneySymbol(totalCredits)
                };
                total = state.ViewModel.applyAmount - (totalCredits - totalDebits);
                state.ViewModel.TotalControl = total;
            } else {
                state.ViewModel.TotalData = {
                    TotalDebit: FormatMoneySymbol(0),
                    TotalCredit: FormatMoneySymbol(0)
                };
                state.ViewModel.TotalControl = state.mainAppRecieptRedux.ViewModel.applyAmount;
            }
            state.ViewState.ShowDebitsandCredits = true;
            return state;

        case 'MAR_ASSIGN_TEMP_APPLICATION_ID':
            state.ViewModel.TempImputationId = isNull(actions.TempImputationId) ? 0 : actions.TempImputationId;
            return state;

        case 'MAR_ASSIGN_ALERT_MESSAGE':
            state.ViewState.ShowAlert = true;
            state.ViewState.Message = actions.Message;
            if (isEmpty(actions.TypeMessage)) {
                state.ViewState.TypeMessage = 'warning';
            } else {
                state.ViewState.TypeMessage = actions.TypeMessage;
            }
            return state;

        case 'MAR_SET_VALUE':
            state.ViewState[actions.label] = actions.value;
            return state;

        case 'MAR_SET_TECHNICALTRANSACTION':
            state.ViewModel.Technicaltransaction = actions.Technicaltransaction;
            return state;

        case 'MAR_SET_TECHNICAL_INFORMATION':
            state.ViewState.SetReceiptValues = true;
            state.ViewModel.applyPostedValue = actions.Data[0].PostdatedValue;
            state.ViewModel.applyIncomeConcept = actions.Data[0].CollectConceptDescription;
            state.ViewModel.applyPostedValue = actions.Data[0].PostdatedValue;
            state.ViewModel.applyDescription = actions.Data[0].CollectDescription;
            state.ViewModel.applyComments = actions.Data[0].Comments;
            state.ViewModel.applyBillId = actions.Data[0].CollectCode;
            state.ViewModel.applyAccountingDate = actions.Data[0].AccountingDateImputation;
            state.ViewModel.applyReceiptNumber = actions.Data[0].TechnicalTransaction;
            state.ViewModel.applyIndividualId = actions.Data[0].PayerId;
            state.ViewModel.applyTransactionNumber = actions.Data[0].TechnicalTransaction;
            state.ViewModel.applyDepositer = actions.Data[0].PayerDocumentNumber + " - " + actions.Data[0].Payer;
            state.ViewModel.applyAmount = actions.Data[0].PaymentsTotal;
            state.ViewModel.applyBranchId = actions.Data[0].BranchId;
            state.ViewModel.applyBranchDescription = actions.Data[0].BranchDescription;
            state.ViewModel.applyCollectCode = actions.Data[0].CollectCode;
            return state;

        case 'MAR_SET_EMPTY_TECHNICAL_INFORMATION':
            state.ViewState.SetReceiptValues = true;
            state.ViewModel.applyPostedValue = "";
            state.ViewModel.applyIncomeConcept = "";
            state.ViewModel.applyPostedValue = "";
            state.ViewModel.applyDescription = "";
            state.ViewModel.applyComments = "";
            state.ViewModel.applyBillId = "";
            state.ViewModel.applyAccountingDate = "";
            state.ViewModel.applyReceiptNumber = "";
            state.ViewModel.applyIndividualId = "";
            state.ViewModel.applyTransactionNumber = "";
            state.ViewModel.applyDepositer = "";
            state.ViewModel.applyAmount = "";
            state.ViewModel.applyBranchId = "";
            state.ViewModel.applyBranchDescription = "";
            state.ViewModel.applyCollectCode = "";
            return state;

        default: return state;
    }
};

class MainAppRecieptRedux {
    static actionCreators(getState, dispatch) {

        $("#modalSuccess").find("#AcceptSuccess").click(function () {
            MainAppRecieptRedux.ProcessCloseorSave(getState, dispatch);
        });

        $("#modalSuccess").on('closed.modal', function () {
            MainAppRecieptRedux.ProcessCloseorSave(getState, dispatch);
        });

        $('#AccountingAcceptMovement').on('click', function () {
            MainAppRecieptRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
            dispatch({ type: 'MAR_SET_VALUE', label: 'HideModals', value: true });
        });

        $('#SaveSearchPoliciesButton').on('click', function () {
            MainAppRecieptRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
            dispatch({ type: 'MAR_SET_VALUE', label: 'HideModals', value: true });
        });

        $('#AccountingMovementModal').on('closed.modal', function () {
            MainAppRecieptRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
        });

        $('#ModalPremiums').on('closed.modal', function () {
            MainAppRecieptRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
        });

        $("#listViewAplicationReceipt").on('rowEdit', function (event, data, index) {
            if (!isNull(getState.mainAppRecieptRedux.ViewModel.TempImputationId)) {
                // Primas por cobrar
                if (data.Id === 1) {
                    dispatch({ type: 'MAR_SET_VALUE', label: 'HideModals', value: false });
                    $('#ModalPremiums').UifModal('showLocal', Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + getState.mainAppRecieptRedux.ViewModel.TempImputationId);

                }
                // Contabilidad
                if (data.Id === 7) {
                    dispatch({ type: 'MAR_SET_VALUE', label: 'HideModals', value: false });
                    $("#addAccountingMovementForm").formReset(); //reseteo el formulario        

                    $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + " " + Resources.DialogTitleTemporary + " " + getState.mainAppRecieptRedux.ViewModel.TempImputationId);
                    dispatch({ type: 'MBR_SET_VALUE', label: 'ShowAccountingModal', value: true });
                    dispatch({ type: 'ACC_CLEAN_SEARCH_PERSON', branchId: getState.mainAppRecieptRedux.ViewModel.applyBranchId });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'LoadBranches', value: false });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'IsClearDocumentsField', value: true });
                    dispatch({ type: 'ACC_SET_VALUE', label: 'IsClearDocumentsField', value: false });
                    dispatch({ type: 'ACC_SET_SEARCH_TYPE', selectedItemSearchById: 0 });
                }
            }
        });

        $('#TechnicalTransactionSearch').on('click', function () {
            if (!isNull($("#TechnicalTransaction").val())) {
                MainAppRecieptRedux.SearchApplicationReceipt(dispatch, getState, $("#TechnicalTransaction").val(), true);
            }
            else {
                $("#alert").UifAlert('show', Resources.RequiredReceiptNumberField, "warning");
            }

        });

        //BOTÓN CANCELAR
        $("#ApplyCancelReceipt").click(function () {
            MainAppRecieptRedux.CloseDialogReceipt(Resources.CancelApplicationMessage, getState, dispatch);
        });

        $("#ApplyAcceptReceipt").click(function () {
            if (TotalDebit() !== 0 || TotalCredit() !== 0) {
                if (RemoveFormatMoney($("#TotalControl").val()) !== 0) {
                    //DIALOGO DE COMFIRMACION PARA GRABAR IMPUTATION EN TEMPORALES, ACTUALIZA ESTADO DE BILL
                    SaveTemporalConfirmationReceipt();
                } else {
                    dispatch({ type: 'MAR_SET_VALUE', label: 'LockScreen', value: true });
                    $("#modalSuccess").find("#accountingApplicationMessageDiv").removeClass('error-accounting');
                    MainApplicationReceiptRequest.SaveApplication(getState.mainAppRecieptRedux.ViewModel.TempImputationId)
                        .done(function (data) {
                            if (data.success === true) {
                                MainAppRecieptRedux.AssignTempApplicationId(getState, dispatch, null);
                                
                                $("#modalSuccess").find("#receiptApplicationMessage").text(Resources.SaveRealSuccessTransactionMessage + " " + $("#TransactionNumber").val());

                                if (data.IsEnabledGeneralLedger === false) {
                                    $("#modalSuccess").find("#accountingApplicationLabelDiv").hide();
                                    $("#modalSuccess").find("#accountingApplicationMessageDiv").hide();
                                } else {
                                    $("#modalSuccess").find("#accountingApplicationLabelDiv").show();
                                    $("#modalSuccess").find("#accountingApplicationMessageDiv").show();
                                }
                                if (data.generalLedgerSuccess !== true) {
                                    $("#modalSuccess").find("#accountingApplicationMessageDiv").addClass('error-accounting');
                                }
                                $("#modalSuccess").find("#receiptApplicationAccountingIntegrationMessage").text(data.result);
                                $('#modalSuccess').UifModal('showLocal', Resources.ReceiptsApplication);

                                //$.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
                            } else {
                                $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
                            }
                        })
                        .always(function () {
                            dispatch({ type: 'MAR_SET_VALUE', label: 'LockScreen', value: false });
                        });
                }
            }
        });

        $(document).ready(function () {
            $("#listViewAplicationReceipt").UifListView({
                autoHeight: true, theme: 'dark',
                sourceData: [],
                customDelete: true,
                customEdit: true,
                edit: true,
                delete: false,
                displayTemplate: "#display-aplication-receipt-template"
            });
            if (!isNull($('#ViewBagTempImputationId').val()) && parseInt($('#ViewBagTempImputationId').val()) > 0) {
                $('#formTechnicalTransaction').remove();
                MainAppRecieptRedux.SearchApplicationReceipt(dispatch, getState, $('#ViewBagTransactionNumber').val());
            }
        });
    }

    static CloseDialogReceipt(message, getState, dispatch) {
        $.UifDialog('confirm', { 'message': message, 'title': Resources.CashReceipt },
            function (result) {
                if (result) {
                    if (!isNull(getState.mainAppRecieptRedux.ViewModel.TempImputationId)) {
                        MainApplicationReceiptRequest.CancelAppliationReceipt(getState.mainAppRecieptRedux.ViewModel.TempImputationId).done(function (data) {
                            if (data.result) {
                                MainAppRecieptRedux.ProcessCloseorSave(getState, dispatch);
                            }
                        });
                    }
                }
            });
    };

    static ProcessCloseorSave(getState, dispatch) {
        $('#modalSuccess').UifModal('hide');
        if ($('#ViewBagRedirectToPage').val() === Resources.ToRedirectMainbill) {
            location.href = $("#ViewBagMainBillingLink").val();
        } else if ($('#ViewBagRedirectToPage').val() === Resources.ToRedirectMainbillSearch) {
            location.href = $("#ViewBagMainBillSearchLink").val();
        }
        CleanGlobalVariablesReceipt();
        dispatch({ type: 'MAR_SET_EMPTY_TECHNICAL_INFORMATION' });
		dispatch({ type: 'MAR_SET_VALUE', label: 'ClearTechnicalTransaction', value: true });
        dispatch({ type: 'MAR_SET_VALUE', label: 'ClearTechnicalTransaction', value: false });
        MainAppRecieptRedux.AssignTempApplicationId(getState, dispatch, null);
        dispatch({ type: 'MAR_SET_VALUE', label: 'SetReceiptValues', value: false });
        if (!isEmptyorZero(getState.mainAppRecieptRedux.ViewModel.TempImputationId)) {
            DialogSearchPolicies.LoadPremiumPayments(getState.dialogSearchPoliciesRedux, dispatch);
            AccountingActioning.LoadAccountingMovements(getState.accountingRedux, dispatch);
        }
        SetTotalApplicationReceipt();
    }

    static SearchApplicationReceipt(dispatch, getState, technicalTransaction, searching = false) {
        dispatch({ type: 'MAR_SET_TECHNICALTRANSACTION', Technicaltransaction: technicalTransaction });
        if (!isEmpty(getState.mainAppRecieptRedux.ViewModel.Technicaltransaction)) {
            $("#alert").UifAlert('hide');
            MainAppRecieptRedux.RequestReceiptApplication(getState, dispatch);
            if (!isNull(getState.mainAppRecieptRedux.ViewModel.TempImputationId)) {
                if (searching === true) {
                    DialogSearchPolicies.LoadPremiumPayments(getState.dialogSearchPoliciesRedux, dispatch);
                    AccountingActioning.LoadAccountingMovements(getState.accountingRedux, dispatch);
                }
            }
        }
    }
    static AssignTempApplicationId(getState, dispatch, tempApplicaitonId) {
        dispatch({ type: 'ACC_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: tempApplicaitonId });
        dispatch({ type: 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: tempApplicaitonId });
        dispatch({ type: 'MAR_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: tempApplicaitonId });
        dispatch({ type: 'PREMPAY_ASSIGN_ACCOUNTING_DATE', AccountingDate: $("#ViewBagDateAccounting").val() });
        tempImputationId = tempApplicaitonId;

        MainAppRecieptRedux.GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch);
    }

    static RequestReceiptApplication(getState, dispatch) {
        //consulta por numero de transaccion el recibo
        MainApplicationReceiptRequest.GetReceiptApplicationInformationByTechnicalTransaction(getState.mainAppRecieptRedux.ViewModel.Technicaltransaction).done(function (data) {
            if (data.length > 0) {
                dispatch({ type: 'MAR_SET_TECHNICAL_INFORMATION', Data: data });
                dispatch({ type: 'MAR_SET_VALUE', label: 'SetReceiptValues', value: false });
                MainAppRecieptRedux.RequestReceiptTemporal(getState, dispatch);
            }
            else {
                dispatch({ type: 'MAR_SET_EMPTY_TECHNICAL_INFORMATION' });
                dispatch({ type: 'MAR_SET_VALUE', label: 'SetReceiptValues', value: false });
                $("#alert").UifAlert('show', Resources.SourceCodeNotExists, "danger");
            }
        });
    }

    static RequestReceiptTemporal(getState, dispatch) {
        // CONSULTAR SI EXISTE TEMPORAL
        MainApplicationReceiptRequest.GetTempImputationBySourceCode(getState.mainAppRecieptRedux.ViewModel.applyCollectCode).done(function (data) {
            if (data.Id === 0) {
                // GRABA A TEMPORALES
                MainApplicationReceiptRequest.SaveTempImputation(Resources.ImputationTypeBill, getState.mainAppRecieptRedux.ViewModel.applyCollectCode, getState.mainAppRecieptRedux.ViewModel.applyIndividualId)
                    .done(function (dataImputation) {
                        MainAppRecieptRedux.AssignTempApplicationId(getState, dispatch, dataImputation.Id);
                    });
            } else {
                // YA EXISTE EN TEMPORALES
                MainAppRecieptRedux.AssignTempApplicationId(getState, dispatch, data.Id);
            }
        });
    }
    //////////////////////////////////////////////////////
    /// Se refresca el listview resumen de movimientos ///
    //////////////////////////////////////////////////////
    static GetDebitsAndCreditsMovementTypesReceipt(getState, dispatch) {

        MainApplicationReceiptRequest.GetDebitsAndCreditsMovementTypes(getState.mainAppRecieptRedux.ViewModel.TempImputationId, 0)
            .done(function (data) {
                dispatch({ type: 'MAR_ASSIGN_MOVEMENTS', Movements: data });
                dispatch({ type: 'MAR_SET_VALUE', label: 'ShowDebitsandCredits', value: false });
            });
    }

    static outputState(state, dispatch) {

        if (state.mainAppRecieptRedux.ViewState.SetReceiptValues === true) {
            MainAppRecieptRedux.SetReceiptValues(state.mainAppRecieptRedux.ViewModel);
        }

        if (state.mainAppRecieptRedux.ViewState.LockScreen === true) {
            lockScreen();
        } else {
            unlockScreen();
        }

        if (state.mainAppRecieptRedux.ViewState.ShowAlert === true) {
            $("#alert").UifAlert('show', state.mainAppRecieptRedux.ViewState.Message, state.mainAppRecieptRedux.ViewState.TypeMessage);
        }

        if (state.mainAppRecieptRedux.ViewState.ShowDebitsandCredits === true) {
            $("#listViewAplicationReceipt").UifListView('clear');
            if (!isNull(state.mainAppRecieptRedux.ViewModel.DebitsandCredits)
                && Array.isArray(state.mainAppRecieptRedux.ViewModel.DebitsandCredits)) {
                for (var i = 0; i < state.mainAppRecieptRedux.ViewModel.DebitsandCredits.length; i++) {
                    $("#listViewAplicationReceipt").UifListView("addItem", state.mainAppRecieptRedux.ViewModel.DebitsandCredits[i]);
                }
            }

            $("#totalAplicationTable").UifDataTable('clear');
            $("#totalAplicationTable").UifDataTable('addRow', state.mainAppRecieptRedux.ViewModel.TotalData);
            $('#TotalControl').val(FormatMoneySymbol(state.mainAppRecieptRedux.ViewModel.TotalControl));
        }

        if (state.mainAppRecieptRedux.ViewState.HideModals === true) {
            $('#AccountingMovementModal').UifModal('hide');
            $('#ModalPremiums').UifModal('hide');
        }

        if (state.mainAppRecieptRedux.ViewState.ClearTechnicalTransaction === true) {
            $("#TechnicalTransaction").val('');
        }
    }

    static SetReceiptValues(data) {
        $("#ReceiptNumber").val(data.applyReceiptNumber);
        $("#DocumentNumberApplicationReceipt").val(data.applyDepositer);
        $("#ReceiptAmountApplication").val(FormatCurrency(FormatDecimal(data.applyAmount)));
        $("#BranchName").val(data.applyBranchDescription);
        $("#IncomeConcept").val(data.applyIncomeConcept);
        $("#PostdatedValue").val(data.applyPostedValue);
        $("#ReceiptDescription").val(data.applyDescription);
        $("#AccountingDate").val(data.applyAccountingDate);
        $("#_Observations").val(data.applyComments);
        $("#TransactionNumber").val(data.applyTransactionNumber);
        $("#TotalControl").val(FormatCurrency(FormatDecimal(data.applyAmount)));
    }

}

var dialogSearchPoliciesRedux = DialogSearchPoliciesRedux.redux;
var accountingRedux = AccountingRedux.redux;
var reversionPremium = ReversionPremiumRedux.redux;
var reducer = Redux.combineReducers(
    { dialogSearchPoliciesRedux, accountingRedux, mainAppRecieptRedux, reversionPremium }
);

const store = Redux.createStore(reducer);

store.getState().accountingRedux.Persist = true;
store.getState().dialogSearchPoliciesRedux.Persist = true;
store.getState().reversionPremium.Persist = true;
DialogSearchPolicies.actionCreators(store.getState().dialogSearchPoliciesRedux, store.dispatch);
AccountingActioning.actionCreators(store.getState().accountingRedux, store.dispatch);
MainAppRecieptRedux.actionCreators(store.getState(), store.dispatch);

store.subscribe(() => {
    var state = store.getState();
    DialogSearchPolicies.outputState(state.dialogSearchPoliciesRedux, store.dispatch);
    AccountingActioning.outputState(state.accountingRedux, store.dispatch);
    MainAppRecieptRedux.outputState(state, store.dispatch);
});
