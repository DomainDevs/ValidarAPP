const mainAppRedux = (state, actions) => {

    state = state || {
        Request: {
            AutomaticReceiptApplication: {}, FrmBill: {}
        },
        ViewState: { SaveBill: false, TotalReceipt: false, LockScreen: false, ShowPremiumModal: false, ShowAccountingModal: false, IsClearFields: false, DisableSave: false },
        ViewModel: {
            ApplyResponse: {}, BillItems: [], TempImputationId: -1, PayerId: -1
        },
        Bags: {}
    };

    switch (actions.type) {

        case 'MBR_ASSIGN_MOVEMENTS':
            state.Request.AutomaticReceiptApplication.AccountingTransactionModel = actions.Movements;
            return state;

        case 'MBR_ASSIGN_TEMP_APPLICATION_ID':
            state.ViewModel.TempImputationId = actions.TempImputationId;
            return state;

        case 'MBR_SET_VALUES_ITEM_BILLING':
            state.ViewModel.BillItems = [];
            if (state.Request.AutomaticReceiptApplication.applicationPremium != undefined &&
                state.Request.AutomaticReceiptApplication.applicationPremium.ApplicationPremiumItems != undefined) {
                state.Request.AutomaticReceiptApplication.applicationPremium.ApplicationPremiumItems.forEach(function (item) {
                    item.MovementType = 0;
                    state.ViewModel.BillItems.push(Object.assign({}, item));
                });

            }
            let accountingTransaction = state.Request.AutomaticReceiptApplication.AccountingTransactionModel;
            if (accountingTransaction != null && accountingTransaction != undefined
                && Array.isArray(accountingTransaction) && accountingTransaction.length > 0) {
                accountingTransaction.forEach(function (item) {
                    item.MovementType = 1;
                    state.ViewModel.BillItems.push(Object.assign({}, item));
                });

            }
            return state;

        case 'MBR_ASSIGN_TOTAL':
            state.Request.AutomaticReceiptApplication.AccountingTransactionModel = actions.Movements;
            state.Request.AutomaticReceiptApplication.applicationPremium = { ImputationId: 0, IsDiscountedCommisson: false, ApplicationPremiumItems: actions.Premiums };
            state.Request.AutomaticReceiptApplication.usedDepositPremiumModel = actions.usedDepositPremiumModel;

            if (state.Request.AutomaticReceiptApplication.usedDepositPremiumModel == null || state.Request.AutomaticReceiptApplication.usedDepositPremiumModel == undefined) {
                state.Request.AutomaticReceiptApplication.usedDepositPremiumModel = [];
            }

            var totalAmount = 0;
            var totalDiscountedCommisson = 0;
            var totalUsedAmount = 0;
            var premiumReceivableItems = [];
            if (state.Request.AutomaticReceiptApplication.applicationPremium != undefined &&
                state.Request.AutomaticReceiptApplication.applicationPremium.ApplicationPremiumItems != undefined) {
                premiumReceivableItems = state.Request.AutomaticReceiptApplication.applicationPremium.ApplicationPremiumItems;
            }
            var usedDepositPremiumModel = state.Request.AutomaticReceiptApplication.usedDepositPremiumModel.UsedAmounts;
            var accountingTransactionModel = state.Request.AutomaticReceiptApplication.AccountingTransactionModel;

            if (premiumReceivableItems.length > 0) {
                for (var i = 0; i < premiumReceivableItems.length; i++) {
                    totalAmount += RemoveFormatMoney(premiumReceivableItems[i].ValueToCollect);
                    totalDiscountedCommisson += parseFloat(premiumReceivableItems[i].DiscountedCommission);
                }
            }
            if (usedDepositPremiumModel.length > 0) {
                for (var i in usedDepositPremiumModel) {
                    totalUsedAmount += parseFloat(usedDepositPremiumModel[i].Amount);
                }
            }

            var totalCreditMovements = 0;
            var totalDebitMovements = 0;

            if (accountingTransactionModel != null && accountingTransactionModel != undefined
                && Array.isArray(accountingTransactionModel) && accountingTransactionModel.length > 0) {
                for (var i = 0; i < accountingTransactionModel.length; i++) {
                    if (accountingTransactionModel[i].AccountingNature == 1) {
                        totalCreditMovements += Math.abs(accountingTransactionModel[i].LocalAmount.Value);
                    } else {
                        totalDebitMovements += Math.abs(accountingTransactionModel[i].LocalAmount.Value);
                    }
                }
            }

            state.ViewModel.TotalPremium = fix(totalAmount - totalUsedAmount - totalDiscountedCommisson + totalCreditMovements - totalDebitMovements);
            state.ViewState.TotalReceipt = true;
            return state;

        case 'MBR_ASSIGN_ALERT_MESSAGE':
            state.ViewState.ShowAlert = true;
            state.ViewState.Message = actions.Message;
            if (isEmpty(actions.TypeMessage)) {
                state.ViewState.TypeMessage = 'warning';
            } else {
                state.ViewState.TypeMessage = actions.TypeMessage;
            }
            return state;

        case 'MBR_CLEAR_VALUES':
            state.ViewModel.BillItems = [];
            state.ViewModel.TempImputationId = -1;
            state.ViewModel.TotalPremium = 0;
            state.ViewModel.PayerId = -1;
            state.ViewModel.Observation = "";
            state.ViewModel.inputName = "";
            state.ViewModel.inputDocumentNumber = "";
            state.ViewModel.DocumentTypeSearchCode
            state.ViewModel.DocumentType
            state.ViewModel.inputAgentDocumentNumber = "";
            state.ViewModel.inputAgentName = "";
            state.ViewModel.inputCompanyDocumentNumber = "";
            state.ViewModel.inputCompanyName = "";
            state.ViewModel.inputOtherDocumentNumber = "";
            state.ViewModel.inputOtherName = "";
            state.ViewModel.inputInsuredDocumentNumber = "";
            state.ViewModel.inputInsuredName = "";
            state.ViewModel.inputSupplierDocumentNumber = "";
            state.ViewModel.inputSupplierName = "";
            state.ViewModel.inputReinsuranceName = "";
            state.ViewModel.inputReinsuranceDocumentNumber = "";
            state.ViewModel.inputContractorDocumentNumber = "";
            state.ViewModel.inputContractorName = "";
            state.ViewModel.inputThirdName = "";
            state.ViewModel.inputThirdDocumentNumber = "";
            state.ViewState.IsClearFields = true;
            return state;

        case 'MBR_SET_APPLY_VALUES':
            state.ViewModel.ApplyResponse.TechnicalTransaction = actions.TechnicalTransaction;
            state.ViewModel.ApplyResponse.ReceiptNumber = actions.ReceiptNumber;
            state.ViewModel.ApplyResponse.DepositerBilling = actions.DepositerBilling;
            state.ViewModel.ApplyResponse.Amount = actions.Amount;
            state.ViewModel.ApplyResponse.LocalAmount = actions.LocalAmount;
            state.ViewModel.ApplyResponse.Branch = actions.Branch;
            state.ViewModel.ApplyResponse.IncomeConcept = actions.IncomeConcept;
            state.ViewModel.ApplyResponse.PostedValue = actions.PostedValue;
            state.ViewModel.ApplyResponse.Description = actions.Description;
            state.ViewModel.ApplyResponse.AccountingDate = actions.AccountingDate;
            state.ViewModel.ApplyResponse.ImputationId = actions.ImputationId;
            state.ViewModel.ApplyResponse.Comments = actions.Comments;
            state.ViewModel.ApplyResponse.BillId = actions.BillId;
            return state;

        case 'MBR_SET_VALUE':
            state.ViewState[actions.label] = actions.value;
            return state;

        case 'MBR_ONINIT':
            //Bags
            if (!isNull(actions.Bags)) {
                if (!isNull(actions.Bags.CollectionTo)) {
                    state.Bags.CollectionTo = actions.Bags.CollectionTo;
                    state.ViewState.loadCollectionTo = true;
                }
                if (!isNull(actions.Bags.IncomeConcept)) {
                    state.Bags.IncomeConcept = actions.Bags.IncomeConcept;
                }
            }
            return state;

        case 'MBR_ASSIGN_FORM_DATA':
            state.Request.Date = actions.Date;
            state.Request.TotalReceipt = actions.TotalReceipt;
            state.Request.branchId = actions.branchId;
            if (actions.preliquidationBranch > 0) {
                state.Request.preliquidationBranch = actions.preliquidationBranch;
            } else {
                state.Request.preliquidationBranch = 0;
            }
            state.Request.FrmBill.PaymentSummary = actions.PaymentSummary;
            state.Request.FrmBill.PaymentSummary.PaymentMethodId = actions.PaymentSummary.PaymentMethodId
            state.Request.FrmBill.BillId = 0; //autonumerico
            state.Request.FrmBill.BillingConceptId = actions.FormData.selectIncomeConcept;
            state.Request.FrmBill.BillControlId = billControlId;
            state.Request.FrmBill.RegisterDate = actions.Date;
            state.Request.FrmBill.Description = actions.FormData.Observation;
            state.Request.FrmBill.PaymentsTotal = ReplaceDecimalPoint(actions.TotalReceipt);
            state.Request.FrmBill.SourcePaymentId = 0;

            // Revisar asignación para que contemple los diferentes tipos de usuario
            state.Request.FrmBill.PayerId = payerId;
            if (actions.AssignPersonData === true) {
                state.Request.FrmBill.PayerDocumentNumber = actions.PersonDocumentNumber;
                state.Request.FrmBill.PayerName = actions.PersonName;
            }

            state.Request.FrmBill.PayerDocumentTypeId = actions.FormData.DocumentType;
            state.Request.FrmBill.PayerDocumentNumber = personDocumentNumber;
            state.Request.FrmBill.PayerName = personName;

            return state;
        case 'MBR_SET_OBSERVATION_POLICIES':
            var obs = "";
            state.Request.AutomaticReceiptApplication.applicationPremium.ApplicationPremiumItems.forEach(function (item) {
                obs += item.PrefixTyniDescription + " " + item.PolicyDocumentNumber + "-" + item.EndorsementDocumentNumber + "/";
            });
            if (obs.length > 0) {
                obs = obs.substring(0, obs.lastIndexOf('/'));
            }
            state.ViewModel.Observation = Resources.labelPayment+ " " + obs;
            state.ViewState.showObservations = true;
            return state;
        default: return state;
    }
};

class MainAppRedux {

    static loadItems(getState, dispatch) {
        dispatch({ type: 'MBR_ASSIGN_TOTAL', Movements: getState.accountingRedux.Movements, Premiums: getState.dialogSearchPoliciesRedux.ApplicationPremiumModel.ApplicationPremiumItems, usedDepositPremiumModel: getState.dialogSearchPoliciesRedux.UsedDepositPremiumModel });
        dispatch({ type: 'MBR_SET_VALUES_ITEM_BILLING' });
        dispatch({ type: 'MBR_SET_VALUE', label: 'TotalReceipt', value: false });
    }

    static acceptClosePoliciesMovement(getState, dispatch) {
        if (!isNull(getState.dialogSearchPoliciesRedux.ApplicationPremiumItem.ApplicationPremiumId)) {
            $.UifNotify('show', { type: 'info', message: Resources.PolicyIsEditing, autoclose: false });
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowPremiumModal', value: true });
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowPremiumModal', value: false });
        } else {
            MainAppRedux.loadItems(getState, dispatch);
            DialogSearchPolicies.ClearSearchPolicy(getState.dialogSearchPoliciesRedux, dispatch);
            if (!isEmptyArray(getState.mainAppRedux.Request.AutomaticReceiptApplication.applicationPremium.ApplicationPremiumItems)) {
                dispatch({ type: 'MBR_SET_OBSERVATION_POLICIES' });
                dispatch({ type: 'MBR_SET_VALUE', label: 'showObservations', value: false });
            }
        }
    }
    
    static setFocusControl(idElemento) {
        document.getElementById(idElemento).focus();
    }

    static SaveTempImputation(dispatch) {
        if (isEmpty($("#ViewBagTempImputationId").val())) {
            MainBillingRequest.SaveTempImputation(Resources.ImputationTypeBill, 0, payerId).done(function (dataImputation) {
                dispatch({ type: 'MBR_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                dispatch({ type: 'ACC_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                dispatch({ type: 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                dispatch({ type: 'PREMPAY_ASSIGN_ACCOUNTING_DATE', AccountingDate: $("#ViewBagDateAccounting").val() });
            });
        } else {
            dispatch({ type: 'MBR_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: $("#ViewBagTempImputationId").val() });
            dispatch({ type: 'ACC_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: $("#ViewBagTempImputationId").val() });
            dispatch({ type: 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: $("#ViewBagTempImputationId").val() });
            dispatch({ type: 'PREMPAY_ASSIGN_ACCOUNTING_DATE', AccountingDate: $("#ViewBagDateAccounting").val() });
        }
    }

    static actionCreators(getState, dispatch) {

        $(document).on('ready', function () {
            MainAppRedux.SaveTempImputation(dispatch);
            MainBillingRequest.GetCollectionTo().done(function (data) {
                dispatch({ type: 'MBR_ONINIT', Bags: { CollectionTo: data.data } });
                dispatch({ type: 'MBR_SET_VALUE', label: 'loadCollectionTo', value: false });
            });
            MainBillingRequest.GetIncomeConcept().done(function (data) {
                dispatch({ type: 'MBR_ONINIT', Bags: { IncomeConcept: data.data } });
                dispatch({ type: 'MBR_SET_VALUE', label: 'loadIncomeConcept', value: true });
                dispatch({ type: 'MBR_SET_VALUE', label: 'loadIncomeConcept', value: false });
            });
            MainAppRedux.setFocusControl('inputInsuredDocumentNumber');

            $("#modalOpeningBilling").find('#AcceptOpening').click(function () {
                MainBillingRequest.SaveBillControl($("#selectBranch").val(), $("#modalOpeningBilling").find("#AccountingDate").val())
                    .done(function (data) {
                        if (!data.success) {
                            $("#alertModalOpeningBilling").UifAlert('show', data.result, "danger", data.result);
                        } else {
                            var openDate = data.result[0].openDate;
                            $('#AccountingDateInit').text(openDate);
                            $("#ViewBagDateAccounting").val(openDate);
                            billControlId = data.result[0].Id;
                            isOpen = true;
                            $("#modalOpeningBilling").modal('hide');

                            $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDate").val(openDate);
                            $("#modalConsignmentCheckAdd").find("#CheckDate").val(openDate);
                            $("#modalCheckAdd").find("#CheckDate").val(openDate);
                            $("#modalConsignmentAdd").find("#ConsignmentDate").val(openDate);
                            dispatch({ type: 'PREMPAY_ASSIGN_ACCOUNTING_DATE', AccountingDate: $("#ViewBagDateAccounting").val() });
                        }
                    });
            });

            $("#modalOpeningBilling").on('closed.modal', function () {
                if (isEmptyorZero(billControlId)) {
                    redirectIndex();
                }
            });

            $("#modalOpeningBilling").find('#DimmisOpening').click(function () {
                redirectIndex();
            });
        });

        $('#OpenPremium').click(function () {
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowPremiumModal', value: true });
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowPremiumModal', value: false });
        });

        $('#OpenAccounting').click(function () {
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowAccountingModal', value: true });
            dispatch({ type: 'ACC_CLEAN_SEARCH_PERSON', branchId: $("#selectBranch").val() });
            dispatch({ type: 'ACC_SET_VALUE', label: 'LoadBranches', value: false });
            dispatch({ type: 'ACC_SET_VALUE', label: 'IsClearDocumentsField', value: true });
            dispatch({ type: 'ACC_SET_VALUE', label: 'IsClearDocumentsField', value: false });
            dispatch({ type: 'ACC_SET_SEARCH_TYPE', selectedItemSearchById: $("#CollectionTo").val() });
        });

        $('#AccountingAcceptMovement').on('click', function () {
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowAccountingModal', value: false });
            MainAppRedux.loadItems(getState, dispatch);
        });
        $('#AccountingMovementModal').on('closed.modal', function () {
            dispatch({ type: 'MBR_SET_VALUE', label: 'ShowAccountingModal', value: false });
            MainAppRedux.loadItems(getState, dispatch);
        });

        $('#SaveSearchPoliciesButton').on('click', function () {
            MainAppRedux.acceptClosePoliciesMovement(getState, dispatch);
        });
        $('#ModalPremiums').on('closed.modal', function () {
            MainAppRedux.acceptClosePoliciesMovement(getState, dispatch);
        });
        $('#ApplyReceipt').on('click', function () {
            if (getState.mainAppRedux.ViewModel.ApplyResponse.TechnicalTransaction != null) {
                cleanRiskList();
                $('#modalSaveBill').UifModal('hide');

                window.location.href = $("#ViewBagLoadApplicationReceiptLink").val()
                    + "?incomeConcept=" + getState.mainAppRedux.ViewModel.ApplyResponse.IncomeConcept
                    + "&technicalTransaction=" + getState.mainAppRedux.ViewModel.ApplyResponse.TechnicalTransaction
                    + "&pagetoredirect=" + Resources.ToRedirectMainbill//MainBilling
            }
        });

        $('#policiesListView').on('rowDelete', function (event, data, index) {
            if (data.MovementType == 0) {
                DialogSearchPolicies.DeletePremiumListView(data, getState.dialogSearchPoliciesRedux, dispatch);
                MainAppRedux.loadItems(getState, dispatch);
            } else if (data.MovementType == 1) {
                AccountingActioning.deleteAccountingMovement(data, getState.accountingRedux, dispatch);
                MainAppRedux.loadItems(getState, dispatch);
            }
        });


        $('#SaveBill').on('click', function () {

            dispatch({ type: 'MBR_SET_VALUE', label: 'DisableSave', value: true });
            dispatch({ type: 'MBR_SET_VALUE', label: 'LockScreen', value: true });

            var result = MainAppRedux.ValidateMainBill();
            if (result.HasError === true) {
                dispatch({ type: 'MBR_ASSIGN_ALERT_MESSAGE', Message: result.Message });
                dispatch({ type: 'MBR_SET_VALUE', label: 'DisableSave', value: false });
                dispatch({ type: 'MBR_SET_VALUE', label: 'LockScreen', value: false });
                dispatch({ type: 'MBR_SET_VALUE', label: 'ShowAlert', value: true });
                return;
            }

            let list = [];

            Array.prototype.push.apply(list, MainAppRedux.ProcessList('cashsListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('checksListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('cardsListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('consignmentsListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('ConsignmentChecksListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('transferListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('datafonoListView'));
            Array.prototype.push.apply(list, MainAppRedux.ProcessList('retentionListView'));

            dispatch({
                type: 'MBR_ASSIGN_FORM_DATA',
                FormData: $("#formBilling").serializeObject(),
                Date: $("#BillingDate").val(),
                TotalReceipt: RemoveFormatMoney($("#TotalReceipt").text()),
                branchId: $("#selectBranch").val(), preliquidationBranch: $("#ViewBagPreliquidationBranch").val(),
                PaymentSummary: list,
                AssignPersonData: $("#CollectionTo").val() == $("#ViewBagOthersCode").val() || $("#CollectionTo").val() == $("#ViewBagCollectorCode").val(),
                PersonDocumentNumber: $("#inputOtherDocumentNumber").val(),
                PersonName: $("#inputOtherName").val()
            });
            $.ajax({
                async: true,
                type: "POST",
                url: ACC_ROOT + "Billing/NeedCloseBill",
                data: { "branchId": getState.mainAppRedux.Request.branchId, "accountingDatePresent": $("#ViewBagDateAccounting").val() }
            }).done(function (data) {

                    let request = MainAppRedux.PrepareRequest(getState.mainAppRedux.Request);
                    request.tempImputationId = getState.mainAppRedux.ViewModel.TempImputationId;
                    if (!isNull(data) && Array.isArray(data) && data.length > 0) {
                        request.FrmBill.BillControlId = data[0].Id;
                    }
                    $('#modalSaveBill').find('#accountingMessageDiv').removeClass('error-accounting');
                    $.ajax({
                        async: true,
                        type: "POST",
                        url: ACC_ROOT + "Billing/SaveBillRequestReciptApplication",
                        data: request
                    }).done(function (data) {

                            if (data.success === false) {
                                var msg = Resources.PolicyComponentsValidationMessage;
                                if (data.result != -2) {
                                    msg = data.result;
                                }
                                dispatch({ type: 'MBR_ASSIGN_ALERT_MESSAGE', Message: msg, TypeMessage: 'danger' });
                                dispatch({ type: 'MBR_SET_VALUE', label: 'LockScreen', value: false });
                                dispatch({ type: 'MBR_SET_VALUE', label: 'ShowAlert', value: false });
                            }
                            else {
                                if (!isNull(data.result.mainApplicationReceipt)) {

                                    $("#modalSaveBill").find("#ReceiptDescription").text(data.result.mainApplicationReceipt.Description);
                                    $("#modalSaveBill").find("#ReceiptTotalAmount").text(FormatMoneySymbol(data.result.mainApplicationReceipt.PaymentsTotal));
                                    $("#modalSaveBill").find("#BillingId").text("00000" + data.result.mainApplicationReceipt.BillId);
                                    $("#modalSaveBill").find("#TransactionNumber").text("00000" + data.result.mainApplicationReceipt.TechnicalTransaction);
                                    applyTechnicalTransaction = data.result.mainApplicationReceipt.TechnicalTransaction;
                                    GetAccountingDateMainBilling();
                                    $("#modalSaveBill").find("#ReceiptUser").text($("#ViewBagUserNick").val());
                                    if (data.result.mainApplicationReceipt.ShowMessage == "False") {
                                        $("#modalSaveBill").find("#accountingLabelDiv").hide();
                                        $("#modalSaveBill").find("#accountingMessageDiv").hide();
                                    }
                                    else {
                                        $("#modalSaveBill").find("#accountingLabelDiv").show();
                                        $("#modalSaveBill").find("#accountingMessageDiv").show();
                                    }
                                    if (data.result.mainApplicationReceipt.GeneralLedgerSuccess !== true) {
                                        $('#modalSaveBill').find('#accountingMessageDiv').addClass('error-accounting');
                                    }
                                    $("#modalSaveBill").find("#accountingMessage").val(data.result.mainApplicationReceipt.Message);
                                    if (data.result.mainApplicationReceipt.StatusId == 3)//se aplicó el recibo
                                    {
                                        applyReceiptNumber = data.result.mainApplicationReceipt.BillId;
                                        applyTechnicalTransaction = data.result.mainApplicationReceipt.TechnicalTransaction;
                                        $("#modalSaveBill").find("#ApplyReceipt").hide();
                                        if (data.result.mainApplicationReceipt.ShowImputationMessage == "False") {
                                            $("#modalSaveBill").find("#applicationIntegration").hide();
                                        }
                                        else {
                                            $("#modalSaveBill").find("#applicationIntegration").show();
                                            $("#modalSaveBill").find("#accountingIntegrationMessage").val(data.result.mainApplicationReceipt.ImputationMessage);
                                        }
                                    }
                                    else {
                                        $("#modalSaveBill").find("#applicationIntegration").hide();
                                    }

                                } else {

                                    $("#modalSaveBill").find("#ReceiptDescription").text(data.result.Description);
                                    $("#modalSaveBill").find("#ReceiptTotalAmount").text(data.result.PaymentsTotal);
                                    $("#modalSaveBill").find("#BillingId").text("00000" + data.result.BillId);
                                    $("#modalSaveBill").find("#TransactionNumber").text("00000" + data.result.TechnicalTransaction);
                                    applyTechnicalTransaction = data.result.TechnicalTransaction;
                                    GetAccountingDateMainBilling();
                                    $("#modalSaveBill").find("#ReceiptUser").text($("#ViewBagUserNick").val());
                                    if (data.result.ShowMessage == "False") {
                                        $("#modalSaveBill").find("#accountingLabelDiv").hide();
                                        $("#modalSaveBill").find("#accountingMessageDiv").hide();
                                    }
                                    else {
                                        $("#modalSaveBill").find("#accountingLabelDiv").show();
                                        $("#modalSaveBill").find("#accountingMessageDiv").show();
                                    }
                                    if (data.result.GeneralLedgerSuccess !== true) {
                                        $('#modalSaveBill').find('#accountingMessageDiv').addClass('error-accounting');
                                    }
                                    $("#modalSaveBill").find("#accountingMessage").val(data.result.Message);

                                    if (data.result.StatusId == 1) {
                                        $("#modalSaveBill").find("#ApplyReceipt").show();
                                        applyReceiptNumber = data.result.BillId;
                                        applyTechnicalTransaction = data.result.TechnicalTransaction;

                                        applyDepositerBilling = personDocumentNumber + " - " + personName;
                                        applyAmount = $("#TotalReceipt").text();
                                        applyLocalAmount = $("#TotalReceipt").text();
                                        applyBranch = $("#selectBranch option:selected").text();
                                        applyIncomeConcept = $("#selectIncomeConcept option:selected").text();
                                        applyPostedValue = "$0.00";
                                        applyDescription = $("#Observation").val();
                                        applyAccountingDate = $("#ViewBagAccountingDate").val();
                                    }
                                    else {
                                        $("#modalSaveBill").find("#ApplyReceipt").hide();
                                    }
                                }
                                MainBillingRequest.GetApplicationIdByTechnicalTransaction(data.result.mainApplicationReceipt.TechnicalTransaction).done(function (applicationId) {
                                    if (applicationId > 0) {
                                        $("#modalSaveBill").find("#ApplyReceipt").hide();
                                    }
                                });
                                $('#modalSaveBill').UifModal('showLocal', Resources.ReceiptSaveSuccess);
                                dispatch({ type: 'MBR_SET_VALUE', label: 'LockScreen', value: false });
                                dispatch({ type: 'ACC_CLEAN' });
                                dispatch({ type: 'ACC_CLEAN_MOVEMENTS' });
                                dispatch({ type: 'ACC_SET_VALUE', label: 'LoadMovements', value: false });
                                dispatch({ type: 'PREMPAY_CLEAN' });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'isCleaningForm', value: false });
                                dispatch({ type: 'PREMPAY_CLEAN_PREMIUM' });
                                dispatch({ type: 'PREMPAY_SET_VALUE', label: 'ShowItemApplyViewRowEdit', value: false });
                                dispatch({
                                    type: 'MBR_SET_APPLY_VALUES',
                                    TechnicalTransaction: data.result.mainApplicationReceipt.TechnicalTransaction,
                                    ReceiptNumber: data.result.mainApplicationReceipt.BillId,
                                    DepositerBilling: personDocumentNumber + " - " + personName,
                                    Amount: $("#TotalReceipt").text(),
                                    LocalAmount: $("#TotalReceipt").text(),
                                    Branch: $("#selectBranch option:selected").text(),
                                    IncomeConcept: $("#selectIncomeConcept option:selected").text(),
                                    PostedValue: "$0.00",
                                    Description: $("#Observation").val(),
                                    AccountingDate: $("#ViewBagAccountingDate").val(),
                                    ImputationId: "",
                                    Comments: "",
                                    BillId: data.result.mainApplicationReceipt.BillId
                                });
                                MainAppRedux.clearFieldsMain(dispatch);
                                ClearFieldsMain();
                                MainAppRedux.SaveTempImputation(dispatch);
                                DialogSearchPolicies.LoadPremiumPayments(getState.dialogSearchPoliciesRedux, dispatch);
                                AccountingActioning.LoadAccountingMovements(getState.accountingRedux, dispatch);
                                MainAppRedux.setFocusControl('inputInsuredDocumentNumber');
                            }
                        })
                        .always(function () {
                            dispatch({ type: 'MBR_SET_VALUE', label: 'DisableSave', value: false });
                            dispatch({ type: 'MBR_SET_VALUE', label: 'LockScreen', value: false });
                        });
                }).
                fail(function () {
                    dispatch({ type: 'MBR_SET_VALUE', label: 'DisableSave', value: false });
                    dispatch({ type: 'MBR_SET_VALUE', label: 'LockScreen', value: false });
                });
        });

        $('#New').click(function () {
            MainAppRedux.showConfirm(getState, dispatch);
        });

    }

    static showConfirm(getState, dispatch) {
        $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.ApplicationBillsMain }, function (result) {
            if (result) {
                if (!isNull(getState.mainAppRedux.ViewModel.TempImputationId)) {
                    MainBillingRequest.DeleteTempApplication(getState.mainAppRedux.ViewModel.TempImputationId).done(function (data) {
                        MainAppRedux.clearFieldsMain(dispatch);
                        ClearFieldsMain();
                        DialogSearchPolicies.LoadPremiumPayments(getState.dialogSearchPoliciesRedux, dispatch);
                        AccountingActioning.LoadAccountingMovements(getState.accountingRedux, dispatch);
                        MainBillingRequest.SaveTempImputation(Resources.ImputationTypeBill, 0, payerId).done(function (dataImputation) {
                            dispatch({ type: 'MBR_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                            dispatch({ type: 'ACC_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                            dispatch({ type: 'PREMPAY_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                        });
                        MainAppRedux.setFocusControl('inputInsuredDocumentNumber');
                    }).fail(function () {

                    });
                }
            }
        });
    };

    static clearFieldsMain(dispatch) {
        dispatch({ type: 'MBR_CLEAR_VALUES' });
        dispatch({ type: 'MBR_SET_VALUE', label: 'IsClearFields', value: false });
    }

    static PrepareRequest(obj) {
        for (var k in obj) {
            if (Array.isArray(obj[k]) && obj[k].length > 0) {
                for (var i = 0; i < obj[k].length; i++) {
                    obj[k][i] = MainAppRedux.PrepareRequest(obj[k][i]);
                }
            }
            else if (typeof obj[k] === 'object') {
                obj[k] = MainAppRedux.PrepareRequest(obj[k]);
            }
            else if (typeof obj[k] === 'number' || (typeof obj[k] === 'string' && /^(\d+)\.(\d+)$/.test(obj[k]))) {
                obj[k] = ReplaceDecimalPoint(obj[k]);
            }
        }
        return obj;
    }

    static ValidateMainBill() {
        $("#formBilling").validate();

        if ($("#formBilling").valid()) {

            if ($("#Observation").val() == "") {
                return { HasError: true, Message: Resources.ObservationRequired };
            }
            if ($("#selectIncomeConcept").val() == "") {
                return { HasError: true, Message: Resources.IncomeConceptRequired };
            }
            if ($("#selectBranch").val() == "") {
                return { HasError: true, Message: Resources.SelectBranch };
            }

            if (RemoveFormatMoney($("#TotalReceipt").text()) === 0) {
                return { HasError: true, Message: Resources.TotalReceiptGreaterZero };
            }

            if ($("#TotalApplication").val() == "") {
                return { HasError: true, Message: Resources.EntryTotalApplication };
            }
            else {
                if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
                    return { HasError: true, Message: Resources.TotalApplicationGreaterZero };
                }
            }
            // Validación de cierre de caja
            if ($("#selectBranch").val() != -1 && RemoveFormatMoney($("#Difference").text()) === 0) {
                return { HasError: false };
            }
            else {
                return { HasError: true, Message: Resources.RemainingDifference };
            }
        } else {
            return { HasError: true, Message: 'Formulario inválido' };
        }
    }

    static ProcessList(modalId) {
        var data = $("#" + modalId).UifListView("getData");

        let list = [];
        if (data != null) {

            let model = {};
            for (var j = 0; j < data.length; j++) {
                // Check
                model = {
                    'PaymentId': 0,
                    'BillId': 0,
                    'PaymentMethodId': data[j].PaymentTypeId,
                    'Amount': RemoveFormatMoney(data[j].Amount),
                    'CurrencyId': data[j].CurrencyId,
                    'LocalAmount': RemoveFormatMoney(data[j].LocalAmount),
                    'ExchangeRate': RemoveFormatMoney(data[j].ExchangeRate),
                    'CheckPayments': [],
                    'CreditPayments': [],
                    'TransferPayments': [],
                    'ConsignmentChecks': [],
                    'DepositVouchers': [],
                    'RetentionReceipts': []
                };

                // Check
                if (modalId == 'checksListView') {
                    model.CheckPayments.push({
                        'DocumentNumber': data[j].DocumentNumber,
                        'IssuingBankId': data[j].IssuingBankId,
                        'IssuingAccountNumber': data[j].IssuingBankAccountNumber,
                        'IssuerName': data[j].IssuerName,
                        'Date': data[j].Date
                    });
                }

                // Credit Card and Datafono
                else if (modalId == 'datafonoListView' || modalId == 'cardsListView') {
                    model.CreditPayments.push({
                        'CardNumber': data[j].CardNumber,
                        'Voucher': data[j].VoucherNumber,
                        'AuthorizationNumber': data[j].AuthorizationNumber,
                        'CreditCardTypeId': data[j].CardType,
                        'IssuingBankId': data[j].IssuingBankId,
                        'Holder': data[j].dataName,
                        'ValidThruYear': data[j].ValidThruYear,
                        'ValidThruMonth': data[j].ValidThruMonth,
                        'TaxBase': RemoveFormatMoney(data[j].TaxBase)
                    });
                }

                // Consignment
                else if (modalId == 'consignmentsListView') {
                    model.DepositVouchers.push({
                        'VoucherNumber': data[j].DocumentNumber,
                        'ReceivingAccountBankId': data[j].RecievingBankId,
                        'ReceivingAccountNumber': data[j].RecievingBankAccountNumber,
                        'Date': data[j].Date,
                        'DepositorName': data[j].IssuerName
                    });
                }

                // ConsignmentCheck
                else if (modalId == 'ConsignmentChecksListView') {
                    model.ConsignmentChecks.push({
                        'VoucherNumber': data[j].DocumentNumber,
                        'ReceivingAccountBankId': data[j].RecievingBankId,
                        'ReceivingAccountNumber': data[j].RecievingBankAccountNumber,
                        'Date': data[j].Date,
                        'DepositorName': data[j].IssuerName,
                        'DocumentNumber': data[j].CheckDocumentNumber,
                        'IssuingBankId': data[j].IssuingBankId,
                        'IssuingAccountNumber': data[j].IssuingBankCheckAccountNumber,
                        'IssuerName': data[j].IssuerCheckName,
                        'DateCheck': data[j].DateCheck,
                        'BankAccountingAccountId': data[j].RecievingBankAccountId
                    });
                }

                // Transfer
                else if (modalId == 'transferListView') {
                    model.RetentionReceipts.push({
                        'DocumentNumber': data[j].DocumentNumber,
                        'IssuingBankId': data[j].IssuingBankId,
                        'IssuingAccountNumber': data[j].IssuingBankAccountNumber,
                        'ReceivingAccountBankId': data[j].RecievingBankId,
                        'ReceivingAccountNumber': data[j].RecievingBankAccountNumber,
                        'Date': data[j].Date,
                        'IssuerName': data[j].IssuerName
                    });
                }

                // Retention
                else if (modalId == 'retentionListView') {
                    model.RetentionReceipts.push({
                        'AuthorizationNumber': data[j].AuthorizationNumber,
                        'BillNumber': data[j].DocumentNumber,
                        'SerialNumber': data[j].SerialNumber,
                        'SerialVoucherNumber': data[j].SerialVoucher,
                        'VoucherNumber': data[j].VoucherNumber,
                        'Date': data[j].Date,
                        'IssueDate': data[j].IssueDate,
                        'ExpirationDate': data[j].ExpirationDate,
                        'dataConceptId': data[j].dataConceptId,
                        'PolicyNumber': data[j].PolicyNumber,
                        'EndorsementNumber': data[j].EndorsementNumber,
                        'InvoiceNumber': data[j].DocumentNumber,
                        'InvoiceDate': data[j].Date
                    });
                }
                list.push(model);
            }
        }
        return list;
    }

    static ApplyBill(billId) {
        // Consultar si existe temporal
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
            data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": billId }
        }).done(function (data) {
            if (data.Id == 0) {
                // Graba a temporales
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                    data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": billId }
                }).done(function (dataImputation) {
                    dispatch({ type: 'MAR_ASSIGN_TEMP_APPLICATION_ID', TempImputationId: dataImputation.Id });
                });
            }
        });
    }

    static setClearfields(state) {
        $("#selectBranch").val($("#ViewBagBranchDefault").val());
        $("#selectBranch").trigger('change');
        $("#Observation").val(state.ViewModel.Observation);
        $("#inputName").UifAutoComplete('setValue', state.ViewModel.inputName);
        $("#inputDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputDocumentNumber);
        $("#SearchCode").val(state.ViewModel.SearchCode);
        $("#DocumentType").val(state.ViewModel.DocumentType);
        $("#inputAgentDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputAgentDocumentNumber);
        $("#inputAgentName").UifAutoComplete('setValue', state.ViewModel.inputAgentName);
        $("#inputCompanyDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputCompanyDocumentNumber);
        $("#inputCompanyName").UifAutoComplete('setValue', state.ViewModel.inputCompanyName);
        $("#inputOtherDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputOtherDocumentNumber);
        $("#inputOtherName").UifAutoComplete('setValue', state.ViewModel.inputOtherName);
        $("#inputInsuredDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputInsuredDocumentNumber);
        $("#inputInsuredName").UifAutoComplete('setValue', state.ViewModel.inputInsuredName);
        $("#inputSupplierDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputSupplierDocumentNumber);
        $("#inputSupplierName").UifAutoComplete('setValue', state.ViewModel.inputSupplierName);
        $("#inputReinsuranceName").UifAutoComplete('setValue', state.ViewModel.inputReinsuranceName);
        $("#inputReinsuranceDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputReinsuranceDocumentNumber);
        $("#inputContractorDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputContractorDocumentNumber);
        $("#inputContractorName").UifAutoComplete('setValue', state.ViewModel.inputContractorName);
        $("#inputThirdName").UifAutoComplete('setValue', state.ViewModel.inputThirdName);
        $("#inputThirdDocumentNumber").UifAutoComplete('setValue', state.ViewModel.inputThirdDocumentNumber);
        $("#ViewBagTempImputationId").val("");
    }

    static outputState(state, dispatch) {

        if (state.mainAppRedux.ViewState.TotalReceipt) {
            $('#ModalPremiums').UifModal('hide');
            $('#TotalApplication').val(FormatMoneySymbol(state.mainAppRedux.ViewModel.TotalPremium));
            $('#TotalPolicy').html(FormatMoneySymbol(state.mainAppRedux.ViewModel.TotalPremium));
            $("#policiesListView").UifListView({
                sourceData: state.mainAppRedux.ViewModel.BillItems,
                autoHeight: true,
                customDelete: true,
                allowDelete: true,
                delete: true,
                customAdd: false,
                customEdit: true,
                edit: false,
                displayTemplate: "#policies-template",
                selectionType: 'multiple'
            });
            SetTotalApplicationPayment();
        }

        if (state.mainAppRedux.ViewState.LockScreen === true) {
            lockScreen();
        } else {
            unlockScreen();
        }

        $('#SaveBill').prop('disabled', state.mainAppRedux.ViewState.DisableSave);

        if (state.mainAppRedux.ViewState.ShowAlert === true) {
            $("#alert").UifAlert('show', state.mainAppRedux.ViewState.Message, state.mainAppRedux.ViewState.TypeMessage);
        }

        if (state.mainAppRedux.ViewState.ShowPremiumModal === true) {
            $('#ModalPremiums').UifModal('showLocal', Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + state.mainAppRedux.ViewModel.TempImputationId);
        }

        if (state.mainAppRedux.ViewState.ShowAccountingModal === true) {
            $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + " " + Resources.DialogTitleTemporary + " " + state.mainAppRedux.ViewModel.TempImputationId);
        } else {
            $('#AccountingMovementModal').UifModal('hide');
        }

        if (state.mainAppRedux.ViewState.loadCollectionTo === true) {
            if (!isNull(state.mainAppRedux.Bags.CollectionTo)) {
                MainBillingRequest.GetControlKey("InsuredControlId").done(function (data) {
                    if (data > 0) {
                        $("#CollectionTo").UifSelect({ sourceData: state.mainAppRedux.Bags.CollectionTo, selectedId: data });
                    }
                });
            }
        }
        if (state.mainAppRedux.ViewState.loadIncomeConcept === true) {
            if (!isNull(state.mainAppRedux.Bags.IncomeConcept)) {
                $("#selectIncomeConcept").UifSelect({ sourceData: state.mainAppRedux.Bags.IncomeConcept, selectedId: 1 });
            }
        }
        if (state.mainAppRedux.ViewState.IsClearFields == true) {
            MainAppRedux.setClearfields(state.mainAppRedux);
        }
        if (state.mainAppRedux.ViewState.showObservations === true) {
            $("#Observation").val(state.mainAppRedux.ViewModel.Observation);
        }
    }
}


var dialogSearchPoliciesRedux = DialogSearchPoliciesRedux.redux;
var accountingRedux = AccountingRedux.redux;
const reversionPremium = ReversionPremiumRedux.redux;
var reducer = Redux.combineReducers(
    { dialogSearchPoliciesRedux, accountingRedux, mainAppRedux, reversionPremium }
);

const store = Redux.createStore(reducer);

store.getState().accountingRedux.Persist = true;
store.getState().dialogSearchPoliciesRedux.Persist = true;
store.getState().reversionPremium.Persist = true;
DialogSearchPolicies.actionCreators(store.getState().dialogSearchPoliciesRedux, store.dispatch);
AccountingActioning.actionCreators(store.getState().accountingRedux, store.dispatch);
MainAppRedux.actionCreators(store.getState(), store.dispatch);

store.subscribe(() => {
    var state = store.getState();
    DialogSearchPolicies.outputState(state.dialogSearchPoliciesRedux, store.dispatch);
    AccountingActioning.outputState(state.accountingRedux, store.dispatch);
    MainAppRedux.outputState(state, store.dispatch);
});
