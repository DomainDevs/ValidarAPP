var PaymentMethosDefault = 1;
var PaymentMethodRowId = -1;
var PaymentMethod = [];
var heightListViewPaymentMeans = 256;
var individualId = -1;
var PaymentMethodId = null;
class MethodPayment extends Uif2.Page {

    getInitialState() {
        MethodPayment.UnBindEvents();
        PaymentMethodRequest.GetPaymentMethods().done(function (data) {
            if (data.success) {
                $("#selectMethodpayment").UifSelect({ sourceData: data.result });
            }
        });

        BankRequest.GetBanks().done(function (data) {
            if (data.success) {
                $("#selectBank").UifSelect({ sourceData: data.result });
            }
        });

        PaymentMethodRequest.GetPaymentTypes().done(function (data) {
            if (data.success) {
                $("#selectAccountType").UifSelect({ sourceData: data.result });
            }
        });

        this.InitializeMeansPayment();
    }
    //Seccion Eventos
    bindEvents() {
        $("#btnrNewMeansPayment").click(this.NewMeansPayment);
        $("#btnCreateMeansPayment").click(this.CreateMeansPayment);
        $("#btnAcceptMeasnPayment").click(this.AcceptMeasnPayment);
        $('#listMeansPayment').on('rowEdit', this.MeansPaymentEdit);
        $("#selectMethodpayment").on("itemSelected", this.MethodpaymentSelected);
        $("#selectBank").on("itemSelected", MethodPayment.BankSelected, null);
        $("#btnCancelMeasnPayment").click(this.CleatLislistMeansPayment);
    }

    static UnBindEvents() {
        $("#btnrNewMeansPayment").unbind();
        $("#btnCreateMeansPayment").unbind();
        $("#btnAcceptMeasnPayment").unbind();
        $('#listMeansPayment').unbind();
        $("#selectMethodpayment").unbind();
        $("#selectBank").unbind();
        $("#btnCancelMeasnPayment").unbind();
    }

    static UnbindEvents() {
        $("#btnrNewMeansPayment").unbind();
        $("#btnCreateMeansPayment").unbind();
        $("#btnAcceptMeasnPayment").unbind();
        $('#listMeansPayment').unbind();
        $('#selectMethodpayment').unbind();
        $("#selectBank").unbind();
        $("btnCancelMeasnPayment").unbind();
    }


    CleatLislistMeansPayment()
    {
        //$('#listMeansPayment').UifListView('clear');
    }
    
    static FillObjectMethodPayment() {
        PaymentMethod.push({
            Id: 1,
            AccountNumber: 0,
            AccountType: null,
            Bank: null,
            MethodPayment: null
        });
    }

    NewMeansPayment() {
        MethodPayment.ClearControlMethodPayment(false);
    }

    CreateMeansPayment() {
        MethodPayment.CreateAcceptMethodPayment();
    }

    AcceptMeasnPayment() {
        MethodPayment.SavePlanPayment();
        Persons.AddSubtitlesRightBar();
    }

    MeansPaymentEdit(event, data, index) {
        if (data.Method.Id == MethodPaymentEnum.Cash) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessagePaymentMethod + " <br>", 'autoclose': true })
            return false;
        }
        else {
            MethodPayment.MethodPaymentEnabledDisabled(false);
            PaymentMethodRowId = index;
            MethodPayment.EditPaymentMethod(data, index);
        }
    }

    MethodpaymentSelected(e, selectedItem) {
        if (selectedItem.Id > 0) {
            if ($("#selectMethodpayment").UifSelect("getSelected") == MethodPaymentEnum.Cash) {
                MethodPayment.MethodPaymentEnabledDisabled(true)
            } else {
                MethodPayment.MethodPaymentEnabledDisabled(false)
            };
        }
    }

    static BankSelected(e, selectedItem, selectOffice) {
        if (selectedItem.Id > 0) {
            BankRequest.GetBankBranches(selectedItem.Id).done(function (data) {
                if (data.success === true) {
                    if (data.result.length > 0) {
                        $("#selectOffice").UifSelect({ sourceData: data.result });
                        if (selectOffice != null) {
                            $("#selectOffice").UifSelect("setSelected", selectOffice.Id);
                        }
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result + " <br>", 'autoclose': true })
                }
               
            });
        }
    }

    //Seccion Funciones
    InitializeMeansPayment() {
        $('#inputAccountNumber').ValidatorKey(ValidatorType.Number);
        $("#selectMethodpayment").val(PaymentMethosDefault);
    }

    static CreateAcceptMethodPayment() {
        if ($('#selectMethodpayment').val() == 1) {
            var listMethodofPayment = $("#listMeansPayment").UifListView("getData");
            var methodEfecty = listMethodofPayment.filter(function (item) { return item.Method.Id == 1 })
            if (methodEfecty !== null) {
                $.UifDialog('alert', {
                    message: AppResourcesPerson.DuplicateCashPaymentMethod,
                    class: 'modal-sm'
                });
                return;
            }

        }
        if (MethodPayment.ValidatePaymentMethod()) {
            MethodPayment.CreatePaymentMethod();
            MethodPayment.ClearControlMethodPayment(true);
        }    
    }

    static CreatePaymentMethod() {
        var PaymentMethod = MethodPayment.CreatePaymentMethodModel();
        if (PaymentMethodRowId == -1) {
            $("#listMeansPayment").UifListView("addItem", PaymentMethod);
        }
        else {
            $("#listMeansPayment").UifListView("editItem", PaymentMethodRowId, PaymentMethod);
        }
    }

    static CreatePaymentMethodModel() {

        var IndividualPayment = {
            Id: null,
            Account: {
                Number: null,
                BankBranch: { Id: null, Description: null },
                Currency: { Id: null, Description: null },
                Bank: { Id: null, Description: null },
                Type: { Id: null, Description: null }
            },
            Method: { Id: null, Description: null }
        };

        if (PaymentMethodId != null) {
            IndividualPayment.Id = PaymentMethodId;
        }

        IndividualPayment.Account.Number = $("#inputAccountNumber").val();

        IndividualPayment.Account.Type.Id = $("#selectAccountType").UifSelect("getSelected");
        IndividualPayment.Account.Type.Description = $("#selectAccountType").UifSelect("getSelectedText");

        IndividualPayment.Account.Bank.Id = $("#selectBank").UifSelect("getSelected");
        IndividualPayment.Account.Bank.Description = $("#selectBank").UifSelect("getSelectedText");

        IndividualPayment.Account.BankBranch.Id = $("#selectOffice").UifSelect("getSelected");
        IndividualPayment.Account.BankBranch.Description = $("#selectOffice").UifSelect("getSelectedText");
        
        IndividualPayment.Method.Id = $("#selectMethodpayment").UifSelect("getSelected");
        IndividualPayment.Method.Description = $("#selectMethodpayment").UifSelect("getSelectedText");

        return IndividualPayment;
    }

    static SavePlanPayment() {
        if ($("#listMeansPayment").UifListView('getData').length > 0) {
            PaymentMethod = $("#listMeansPayment").UifListView('getData');
            lockScreen();
            PaymentMethodRequest.CreateIndividualpayment(PaymentMethod, individualId).done(function (data) {
                if (data.success) {
                    var dataPaymentMethod = data.result[0];
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(dataPaymentMethod.InfringementPolicies, true);
                    let countAuthorization = dataPaymentMethod.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(dataPaymentMethod.InfringementPolicies, data.result[0].OperationId, FunctionType.PersonPaymentMethods);
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.SuccessfulCreatedPaymentMethod, 'autoclose': true });
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmpty, 'autoclose': false })
        }
    }

    static CleanObjectsPaymentMeans() {
        PaymentMethod = [];
        $("#listMeansPayment").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#PaymentMeansTemplate", height: heightListViewPaymentMeans });
    }

    static ClearControlMethodPayment(enabledDisabled) {
        PaymentMethodRowId = -1;
        PaymentMethodId = null;
        $("#selectAccountType").UifSelect("setSelected", null);
        $("#selectBank").UifSelect("setSelected", null);
        $("#selectOffice").UifSelect("setSelected", null);
        $('#inputAccountNumber').val("");
        $("#selectMethodpayment").UifSelect("setSelected", null);
        MethodPayment.MethodPaymentEnabledDisabled(enabledDisabled);
    }

    static MethodPaymentEnabledDisabled(control) {
        $("#selectBank").prop('disabled', control);
        $("#selectOffice").prop('disabled', control);
        $("#inputAccountNumber").prop('disabled', control);
        $("#selectAccountType").prop('disabled', control);
    }

    //Seccion Load
    static GetPaymentMethod() {
        var contPayment = 0;
        if (individualId != null) {
            $.UifProgress('show');
            PaymentMethodRequest.GetIndividualpaymentMethodByIndividualId(individualId).done(function (data) {
                $.UifProgress('close');
                if (data.success) {
                    $("#listMeansPayment").UifListView({ sourceData: data.result, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#PaymentMeansTemplate", height: heightListViewPaymentMeans });

                    var contRowsPaymentMethod = $("#listMeansPayment").UifListView('getData').length;
                    if (contRowsPaymentMethod > 0) {
                        $.each($("#listMeansPayment").UifListView('getData'), function (key, value) {
                            if (this.PaymentMethod != null) {
                                if (this.PaymentMethod.Id) {
                                    contPayment = contPayment + 1;
                                }
                            }
                        });
                    }

                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
                }
            }).fail(function (data) {
                $.UifProgress('close');
            });
        }
    }

    //seccion edit
    static EditPaymentMethod(data, index) {
        PaymentMethodId = data.Id;
        $("#inputAccountNumber").val(data.Account.Number);
        $("#selectAccountType").UifSelect("setSelected", data.Account.Type.Id);
        if (data.Account.Bank != null) {
            $("#selectBank").UifSelect("setSelected", data.Account.Bank.Id);
            MethodPayment.BankSelected(null, { Id: data.Account.Bank.Id }, data.Account.BankBranch);
        }
        $("#selectMethodpayment").UifSelect("setSelected", data.Method.Id);
    }

    //Validaciones
    static DuplicatePaymentMethod(IdPayment) {
        var duplicate = false;
        $.each($("#listMeansPayment").UifListView('getData'), function (i, value) {
            if (this.PaymentMethod != null) {
                if (this.PaymentMethod.Id == $("#selectMethodpayment").UifSelect("getSelected") && PaymentMethodRowId != i) {
                    duplicate = true;
                    return false;
                }
            }
        });
        return duplicate;
    }

    static ValidatePaymentMethod() {
        var error = "";
        if ($("#selectMethodpayment").UifSelect("getSelected") == null || $("#selectMethodpayment").UifSelect("getSelected") == "") {
            error = error + AppResourcesPerson.LabelMethodPayment + "<br>";
        }
        if ($("#selectMethodpayment").UifSelect("getSelected") != "" && $("#selectMethodpayment").UifSelect("getSelected") != MethodPaymentEnum.Cash) {
            if ($("#selectAccountType").UifSelect("getSelected") == null || $("#selectAccountType").UifSelect("getSelected") == "") {
                error = error + AppResourcesPerson.LabelAccountType + "<br>";
            }

            if ($("#inputAccountNumber").val() == "") {
                error = error + AppResourcesPerson.LabelAccountNumber + "<br>";
            }
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + error, 'autoclose': true })
            return false;
        } else if (MethodPayment.DuplicatePaymentMethod($("#selectMethodpayment").UifSelect("getSelected"))) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelPaymentMethodAlreadyExists + " <br>", 'autoclose': true })
            return false;
        }
        return true;
    }

}