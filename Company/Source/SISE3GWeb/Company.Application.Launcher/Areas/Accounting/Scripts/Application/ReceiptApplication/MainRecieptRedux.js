class MainRecieptRedux {
    static redux(State, Actions) {
        State = State || {
            Persist: false,
            TempImputationId: -1,
            ViewModel: {}, uifNotifyModel: null, Bags: { Movements: [] }, ViewState: {
                Supplier: false, Insured: false, Others: false, Coinsurer: false, Third: false, Employee: false, Reinsurer: false, Agent: false, OpenAccounting: false, OpenPremium: false, DocumentTypeDisabled: false
            },
            Data: {}
        };
        switch (Actions.type) {
            case 'INIT_BILL':
                if (Actions.Bags.Supplier) {
                    State.ViewModel.TypeSupplier = Actions.Bags.Supplier
                }
                if (Actions.Bags.Insured) {
                    State.ViewModel.TypeInsured = Actions.Bags.Insured
                }
                if (Actions.Bags.Others) {
                    State.ViewModel.TypeOthers = Actions.Bags.Others
                }
                if (Actions.Bags.Coinsurer) {
                    State.ViewModel.TypeCoinsurer = Actions.Bags.Coinsurer
                }
                if (Actions.Bags.Third) {
                    State.ViewModel.TypeThird = Actions.Bags.Third
                }
                if (Actions.Bags.Employee) {
                    State.ViewModel.TypeEmployee = Actions.Bags.Employee
                }
                if (Actions.Bags.Reinsurer) {
                    State.ViewModel.TypeReinsurer = Actions.Bags.Reinsurer
                }
                if (Actions.Bags.Agent) {
                    State.ViewModel.TypeAgent = Actions.Bags.Agent
                }
                return State;
            case 'BILL_SELECTED_COLLECTION':

                if (Actions.Bags.Collection.Id == State.ViewModel.TypeSupplier) {
                    State.ViewState.Supplier = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeInsured) {
                    State.ViewState.Insured = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeOthers) {
                    State.ViewState.Others = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeCoinsurer) {
                    State.ViewState.Coinsurer = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeThird) {
                    State.ViewState.Third = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeEmployee) {
                    State.ViewState.Employee = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeReinsurer) {
                    State.ViewState.Reinsurer = true;
                } else if (Actions.Bags.Collection.Id == State.ViewModel.TypeAgent) {
                    State.ViewState.Agent = true;
                }
                State.ViewState.DocumentTypeDisabled = true;
                return State;
            case 'BILL_SELECTED_AGENT':

                State.ViewModel.AgentId = Actions.Bags.Agent.DocumentNumber;
                State.ViewModel.AgentName = Actions.Bags.Agent.Name;
                return State;
            case 'BILL_SELECTED_INSURED':

                State.ViewModel.InsuredId = Actions.Bags.Insured.DocumentNumber;
                State.ViewModel.InsuredName = Actions.Bags.Insured.Name;
                return State;
            case 'BILL_SELECTED_EMPLOYEE':

                State.ViewModel.EmployeeId = Actions.Bags.Employee.DocumentNumber;
                State.ViewModel.EmployeeName = Actions.Bags.Employee.Name;
                return State;
            case 'BILL_SELECTED_SUPPLIER':

                State.ViewModel.SupplierId = Actions.Bags.Supplier.DocumentNumber;
                State.ViewModel.SupplierName = Actions.Bags.Supplier.Name;
                return State;
            case 'BILL_SELECTED_REINSURANCE':

                State.ViewModel.ReinsuranceId = Actions.Bags.Reinsurance.DocumentNumber;
                State.ViewModel.ReinsuranceName = Actions.Bags.Reinsurance.Name;
                return State;
            case 'BILL_SELECTED_THIRD':

                State.ViewModel.ThirdId = Actions.Bags.Third.DocumentNumber;
                State.ViewModel.ThirdName = Actions.Bags.Third.Fullname;
                return State;
            case 'BILL_CLEAR_SELECTED':

                State.ViewModel.Agent = null;
                State.ViewModel.Employee = null;
                State.ViewModel.Supplier = null;
                State.ViewModel.Reinsurance = null;
                State.ViewModel.Third = null;
                State.ViewModel.Insured = null;
                State.ViewModel.Others = null;
                State.ViewModel.Coinsurer = null;
                return State;
            case 'BILL_GET_TYPE_DOCUMENT':

                State.ViewState.IndividualTypeDocument = Actions.IndividualTypeDocument;
                State.ViewState.DocumentTypeDisabled = true;
                return State;
            case 'BILL_OPEN_PREMIUM':

                State.ViewState.OpenPremium = true;
                return State;
            case 'BILL_OPEN_ACCOUNTING':

                State.ViewState.OpenAccounting = true;
                return State;
            case 'BILL_SET_VALUE':

                State.ViewState[Actions.Label] = Actions.Value;
                return State;
            case 'BILL_SET_VALUES':

                for (var i = 0; i < Actions.Labels.length; i++) {
                    State.ViewState[Actions.Labels[i]] = Actions.Value;
                }
                return State;

            default: return State;
        }
    }
}

class MainBillings {
    static actionCreators(getState, dispatch) {
        $(document).on('ready', function () {
            dispatch({
                type: 'INIT_BILL', Bags: {
                    Supplier: $("#ViewBagSupplierCode").val(),
                    Insured: $("#ViewBagInsuredCode").val(),
                    Others: $("#ViewBagOthersCode").val(),
                    Coinsurer: $("#ViewBagCoinsurerCode").val(),
                    Third: $("#ViewBagThird").val(),
                    Employee: $("#ViewBagEmployeeCode").val(),
                    Reinsurer: $("#ViewBagReinsurerCode").val(),
                    Agent: $("#ViewBagAgentCode").val(),
                    Contractor: $("#ViewBagContractorCode").val(),
                }
            });
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
        });

        $('#CollectionTo').on('itemSelected', function (event, selectedItem) {
            dispatch({
                type: 'BILL_SET_VALUES',
                Labels: ['Supplier', 'Insured', 'Others', 'Coinsurer', 'Third', 'Employee', 'Reinsurer', 'Agent', 'Contractor'],
                Value: false
            });
            dispatch({ type: 'BILL_SELECTED_COLLECTION', Bags: { Collection: selectedItem } });
        });

        $('#inputAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_AGENT', Bags: { Agent: selectedItem } });
            MainBillingRequest.GetDocumentTypeId(selectedItem.IndividualId).done(function (data) {
                dispatch({ type: 'BILL_GET_TYPE_DOCUMENT', IndividualTypeDocument: data.Id });
            });

        });

        $('#inputAgentName').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_AGENT', Bags: { Agent: selectedItem } });
        });

        $('#inputInsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_INSURED', Bags: { Insured: selectedItem } });
        });

        $('#inputInsuredName').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_INSURED', Bags: { Insured: selectedItem } });
        });

        $('#inputDocumentNumber').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_EMPLOYEE', Bags: { Employee: selectedItem } });
        });

        $('#inputName').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_EMPLOYEE', Bags: { Employee: selectedItem } });
        });

        $('#inputSupplierDocumentNumber').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_SUPPLIER', Bags: { Supplier: selectedItem } });
        });

        $('#inputSupplierName').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_SUPPLIER', Bags: { Supplier: selectedItem } });
        });

        $('#inputReinsuranceDocumentNumber').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_REINSURANCE', Bags: { Reinsurance: selectedItem } });
        });

        $('#inputReinsuranceName').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_REINSURANCE', Bags: { Reinsurance: selectedItem } });
        });

        $('#inputThirdDocumentNumber').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_THIRD', Bags: { Third: selectedItem } });
        });

        $('#inputThirdName').on('itemSelected', function (event, selectedItem) {
            dispatch({ type: 'BILL_CLEAR_SELECTED' });
            dispatch({ type: 'BILL_SELECTED_THIRD', Bags: { Third: selectedItem } });
        });

        $('#OpenPremium').click(function () {
            dispatch({ type: 'BILL_OPEN_PREMIUM' });
            dispatch({ type: 'BILL_SET_VALUE', Label: ['OpenPremium'], Value: false });
        });

        $('#OpenAccounting').click(function () {
            dispatch({ type: 'BILL_OPEN_ACCOUNTING' });
            dispatch({ type: 'BILL_SET_VALUE', Label: ['OpenAccounting'], Value: false });
        });
    }

    static outputState(State, dispatch) {

        $('#rowSupplier').toggle(State.ViewState.Supplier);
        $('#rowInsured').toggle(State.ViewState.Insured);
        $('#rowOther').toggle(State.ViewState.Others);
        $('#rowCompany').toggle(State.ViewState.Coinsurer);
        $('#rowThird').toggle(State.ViewState.Third);
        $('#rowPerson').toggle(State.ViewState.Employee);
        $('#rowReinsurance').toggle(State.ViewState.Reinsurer);
        $('#rowAgent').toggle(State.ViewState.Agent);
        $('#rowContractor').toggle(State.ViewState.Contractor);
        $("#inputAgentDocumentNumber").UifAutoComplete('setValue', State.ViewModel.AgentId);
        $("#inputAgentName").UifAutoComplete('setValue', State.ViewModel.AgentName);
        $("#inputInsuredDocumentNumber").UifAutoComplete('setValue', State.ViewModel.InsuredId);
        $("#inputInsuredName").UifAutoComplete('setValue', State.ViewModel.InsuredName);
        $("#inputDocumentNumber").UifAutoComplete('setValue', State.ViewModel.EmployeeId);
        $("#inputName").UifAutoComplete('setValue', State.ViewModel.EmployeeName);
        $("#inputSupplierDocumentNumber").UifAutoComplete('setValue', State.ViewModel.SupplierId);
        $("#inputSupplierName").UifAutoComplete('setValue', State.ViewModel.SupplierName);
        $("#inputReinsuranceDocumentNumber").UifAutoComplete('setValue', State.ViewModel.ReinsuranceId);
        $("#inputReinsuranceName").UifAutoComplete('setValue', State.ViewModel.ReinsuranceName);
        $("#inputThirdDocumentNumber").UifAutoComplete('setValue', State.ViewModel.ThirdId);
        $("#inputThirdName").UifAutoComplete('setValue', State.ViewModel.ThirdName);

        if (State.ViewState.OpenPremium) {
            $('#ModalPremiums').UifModal('showLocal', Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + State.TempImputationId);
        }

        if (State.ViewState.DocumentTypeDisabled) {
            $("#DocumentType").UifSelect({ sourceData: State.ViewState.IndividualTypeDocument, enable: false });
        }

        if (State.ViewState.OpenAccounting) {
            $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + " " + Resources.DialogTitleTemporary + " " + State.TempImputationId);
        }
    }
}