var recuperatorId = null;
var debtorId = null;
var recoveryId = null;
var recoveryTypes = null;
var paymentPlanId = null;
var endorsementId = null;
var policyId = null;
var debtorIsParticipant = null;
var accountingDate = null;
class Recovery extends Uif2.Page {
    getInitialState() {
        Recovery.InitialRecovery();
    }

    bindEvents() {

        // Uppercase
        $("#_FullName").TextTransform(ValidatorType.UpperCase);
        $("#_recoveryCompany").TextTransform(ValidatorType.UpperCase);
        $("#_recoveryLossResponsible").TextTransform(ValidatorType.UpperCase);
        $("#_recoveryAssignedCourt").TextTransform(ValidatorType.UpperCase);
        $("#_DebtorFullName").TextTransform(ValidatorType.UpperCase);
        $("#_recoveryDebtorAddress").TextTransform(ValidatorType.UpperCase);
        $("#_recoveryDescription").TextTransform(ValidatorType.UpperCase);

        //Clicks
        $("#Search").on('click', Recovery.SearchSubClaims);
        $("#btnClearRecovery").on('click', Recovery.ClearRecovery);
        $("#btnGetPolicyInfo").on('click', Recovery.OpenModalPolicyConsult);
        $("#btnSaveRecovery").on('click', Recovery.ExecuteRecoveryOperations);                

        //Autocompletar
        $("#_DebtorDocumentNumber").on('itemSelected', Recovery.SetDebtorInformation);
        $("#_DebtorFullName").on('itemSelected', Recovery.SetDebtorInformation);
        $("#_DocumentNumber").on('itemSelected', Recovery.setRecuperatorInformation);
        $("#_FullName").on('itemSelected', Recovery.setRecuperatorInformation);

        //Focus
        $("#_RecoveryTax").blur(Recovery.CalculateTotalOfRecovery);
        $("#_RecoveryPaymentsNumber").blur(Recovery.CalculateRecoveryAgreedPlan);
        $("#_RecoveryNetAmount").focusin(Recovery.NotFormatMoneyIn);
        $("#_RecoveryNetAmount").focusout(Recovery.FormatMoneyOut);

        //Tablas rowSelected
        $("#tblSubClaims").on('rowSelected', Recovery.GetRecoveriesByClaimIdSubClaimId);

        //Tablas rowEdit
        $("#tblRecoveries").on('rowEdit', Recovery.ToEditRecovery);

        //Numeros
        $("#claimRecoverySearchClaimNumber").on('keypress', Recovery.OnlyNumbers);        
        $("#_recoveryExpedientNumber").on('keypress', Recovery.OnlyNumbers);        
        $("#_recoveryDebtorPhone").on('keypress', Recovery.OnlyNumbers);
        $("#_RecoveryTax").on('keypress', Recovery.OnlyNumbers);
        $("#_RecoveryPaymentsNumber").on('keypress', Recovery.OnlyNumbers);
        $("#inputPolicyDocumentNumber").ValidatorKey(ValidatorType.Number, 2, 1);
        $("#claimRecoverySearchClaimNumber").ValidatorKey(ValidatorType.Number, 2, 1);
        $("#_RecoveryNetAmount").OnlyDecimals(2);
        $("#_RecoveryNetAmount").ValidatorKey(ValidatorType.Decimal, 2, 1);

        //Change
        $("#_DebtorDocumentNumber").on('keydown', Recovery.ClearDebtorInformation);
        $("#_DebtorFullName").on('keydown', Recovery.ClearDebtorInformation);

        $("#recoveryCreatedDate").on("datepicker.change", function (event, date) {
            Recovery.ValidateRecoveryCreatedDate();
        });

    }

    static InitialRecovery() {
        $('#_policyId').val('');
        if (modelSearchCriteria.claimNumber == null && modelSearchCriteria.prefixId == null && modelSearchCriteria.branchId == null) {
            Recovery.GetPrefixes();
            Recovery.GetBranches();
        }

        Recovery.DoRecoveryClaim();

        //Obtener Motivo de cancelación
        RecoveryRequest.GetCancellationReasons().done(function (response) {
            if (response.success) {
                $("#selectCancellationReasons").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        //Obtener Tipos de Recobro
        RecoveryRequest.GetRecoveryTypes().done(function (response) {
            if (response.success) {
                $("#selectRecoveryType").UifSelect({ sourceData: response.result });
                recoveryTypes = response.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });


        //Obtener Moneda
        RecoveryRequest.GetCurrencies().done(function (response) {
            if (response.success) {
                $("#selectCurrency").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        //Obtener tipo de pago
        RecoveryRequest.GetPaymentClasses().done(function (response) {
            if (response.success) {
                $("#selectPaymentClass").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        //Obtener fecha contable
        RecoveryRequest.GetAccountingDate().done(function (response) {
            if (response.success) {                
                accountingDate = FormatDate(response.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
        

        $("#accCancellationRecovery").hide();
    }

    static GetPrefixes(callback) {
        //Obtener Ramos
        RecoveryRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#selectPrefix").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetBranches(callback) {
        //Obtener Sucursales
        RecoveryRequest.GetBranches().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#selectBranch").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    //Obtener Total de Recobro
    static CalculateTotalOfRecovery() {
        var iNet, tax, total;

        iNet = parseFloat(NotFormatMoney($("#_RecoveryNetAmount").val()));

        if ($("#_RecoveryTax").val() != "") {
            tax = parseFloat($("#_RecoveryTax").val()) / 100;
        } else {
            tax = 0;
        }

        total = iNet + tax * iNet;
        $("#_RecoveryTotal").val(FormatMoney(total));
    }

    // Rellena Tabla de Plan Pactado
    static CalculateRecoveryAgreedPlan() {
        Recovery.CalculateTotalOfRecovery();
        var totalSale = NotFormatMoney($("#_RecoveryTotal").val());
        var currencyDescription = $("#selectCurrency").UifSelect("getSelectedSource").Description;
        var payments = $("#_RecoveryPaymentsNumber").val();
        var dateStart = $("#_RecoveryFirtPaymentDue").UifDatepicker('getValue');

        if (payments == "") {
            payments = 1;
        }

        RecoveryRequest.CalculateRecoveryAgreedPlan(dateStart, totalSale, payments, currencyDescription).done(function (response) {
            if (response.success) {
                $.each(response.result, function (index, value) {
                    this.ExpirationDate = FormatDate(this.ExpirationDate);
                });
                $("#tblAgreedPlan").UifDataTable({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    //Abre modal de consulta de poliza
    static OpenModalPolicyConsult() {
        Recovery.GetClaimsByPolicyId(policyId);
        $('#modalPolicyConsult').UifModal('showLocal', Resources.Language.LabelPoliceInformation);
    }

    static GetClaimsByPolicyId(policyId) {
        RecoveryRequest.GetClaimsByPolicyId(policyId).done(function (data) {
            if (data.success) {
                $.each(data.result, function (index, value) {
                    this.OccurrenceDate = FormatDate(this.OccurrenceDate);
                });
                $("#tblClaimsList").UifDataTable({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.Error, 'autoclose': true });
            }
        });
    }
    //Buscar Subreclamo
    static SearchSubClaims() {
        $("#formRecoveryClaims").validate();
        if ($("#formRecoveryClaims").valid()) {
            Recovery.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber();
        }
    }

    //Obtiene Subreclamo por sucursal, ramo, y numero de poliza 
    static GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber() {
        var prefixId = $("#selectPrefix").UifSelect('getSelected');
        var branchId = $("#selectBranch").UifSelect('getSelected');
        var policyDocumentNumber = $("#inputPolicyDocumentNumber").val();
        var claimNumber = $("#claimRecoverySearchClaimNumber").val();

        RecoveryRequest.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber).done(function (response) {
            if (response.success) {
                if (response.result.length > 0) {
                    $("#selectPrefix").UifSelect('setSelected', response.result[0].PrefixId);
                    $("#selectBranch").UifSelect('setSelected', response.result[0].BranchCode);
                    $('#tblSubClaims').UifDataTable({ sourceData: response.result });
                    Recovery.GetPolicyInformation();
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ClaimNotFound, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    //Obtener por informacion de la políza
    static GetPolicyInformation() {
        var subClaim = $("#tblSubClaims").UifDataTable('getData')[0];
        if (subClaim != null) {
            endorsementId = subClaim.EndorsementId;
            Recovery.GetPolicyByEndorsementIdModuleType();
        }
        else {
            Recovery.ClearPolicyInformation();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoDataFound, 'autoclose': true });
        }
    }

    //Obtener Políza por Id Endoso
    static GetPolicyByEndorsementIdModuleType() {
        if (endorsementId === null || endorsementId === "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.IncorrectNumberEndorsement, 'autoclose': true });
            return;
        }

        RecoveryRequest.GetPolicyByEndorsementIdModuleType(endorsementId).done(function (response) {
            if (response.result.length === 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotEnoughData, 'autoclose': true });
                return;
            }

            if (response.result != null) {
                policyId = response.result.Id;
                //Datos de póliza en pantalla
                $("#_branch").val(response.result.BranchDescription);
                $("#_prefix").val(response.result.PrefixDescription);
                $("#_policy").val(response.result.DocumentNumber);
                $("#_endorsement").val(response.result.EndorsementDocumentNum);
                $("#_nameInsured").val(response.result.HolderName);

                //Modal consulta de póliza
                $("#_policyHolder").val(response.result.HolderName);
                $("#_policyType").val(response.result.PolicyType);
                $("#_policyEndorsement").val(response.result.EndorsementDocumentNum);
                $("#_policyBusinessType").val(response.result.BusinessType);
                $("#_policyIssuingDate").val(response.result.IssueDate);
                $("#_policyValidSince").val(FormatDate(response.result.CurrentFrom));
                $("#_policyValidTo").val(FormatDate(response.result.CurrentTo));

                //Pestaña Datos Póliza
                $('#_policyHolder').val(response.result.HolderName);
                $('#_policuInsured').val('');
                $('#_policyBeneficiary').val('');
                $('#_policyIntermediary').val(response.result.Agent);
                $('#_policyEndorsement').val(response.result.EndorsementDocumentNum);
                $('#_policyType').val(response.result.PolicyType);
                $('#_policyBusinessType').val(response.result.BusinessType);
                $('#_policyIssuingDate').val(FormatDate(response.result.IssueDate));
                $('#_policyValidSince').val(FormatDate(response.result.CurrentFrom));
                $('#_policyValidTo').val(FormatDate(response.result.CurrentTo));

                //Pestaña Datos Cartera 
                $('#_policyIssuedPrime').val('');
                $('#_policuTableExpenses').val('');
                $('#_policyTableTaxes').val(response.result.TaxExpenses);
                $('#_policyTotalBonus').val('');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoDataFound, 'autoclose': true });
            }
        });
    }

    //Vacía el Modal de informacion de poliza
    static ClearPolicyInformation() {
        $("#_branch").val("");
        $("#_prefix").val("");
        $("#_policy").val("");
        $("#_endorsement").val("");
        $("#_nameInsured").val("");
    }

    //Llena la información del deudor
    static SetDebtorInformation(event, debtor) {
        $('#_DebtorDocumentNumber').UifAutoComplete('setValue', debtor.DocumentNumber);
        $('#_DebtorFullName').UifAutoComplete('setValue', debtor.FullName);
        $('#_recoveryDebtorAddress').val(debtor.Address);
        $('#_recoveryDebtorPhone').val(debtor.Phone);
        debtorId = debtor.IndividualId;
    }

    static ClearDebtorInformation() {
        debtorId = null;
    }

    //Llena el campo de Recuperador por Nombre o documento
    static setRecuperatorInformation(event, recovery) {
        $('#_DocumentNumber').UifAutoComplete('setValue', recovery.DocumentNumber);
        $('#_FullName').UifAutoComplete('setValue', recovery.FullName);
        recuperatorId = recovery.IndividualId;
    }

    static GetRecoveriesByClaimIdSubClaimId(event, subClaim) {

        if (subClaim == null) {
            subClaim = $("#tblSubClaims").UifDataTable('getSelected')[0];
        }

        var claimId = subClaim.ClaimId;
        var subClaimId = subClaim.SubClaim;

        RecoveryRequest.GetRecoveriesByClaimIdSubClaimId(claimId, subClaimId).done(function (response) {
            var recoveryTypes = null;
            if (response.success) {

                $.each(response.result, function (index, value) {
                    this.CreationDate = FormatDate(this.CreationDate);
                    this.EndDate = FormatDate(this.EndDate);
                    this.AssignmentDate = FormatDate(this.AssignmentDate);
                    this.TotalAmount = this.TotalAmount + this.TotalAmount * (this.TaxPercentage / 100);
                });

                $('#tblRecoveries').UifDataTable({ sourceData: response.result });
                $('#tblRecoveriesSummary').UifDataTable({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static ExecuteRecoveryOperations() {
        $("#RecoveryForm").validate();
        if ($("#RecoveryForm").valid()) {

            if (debtorId != null) {

                if ($("#tblSubClaims").UifDataTable('getSelected') == null) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MustSelectSubClaim, 'autoclose': true });
                    return;
                }

                if (recuperatorId == null || recuperatorId == 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecuperator, 'autoclose': true });
                    return;
                }

                var subClaim = $("#tblSubClaims").UifDataTable('getSelected')[0];

                if (subClaim != null) {
                    lockScreen();
                    var recovery = {
                        PaymentQuotas: []
                    };

                    recovery.Id = $("#recoveryNumber").val();
                    recovery.CreatedDate = $("#recoveryCreatedDate").val();
                    recovery.Description = $("#_recoveryDescription").val();
                    recovery.CancellationReasonId = $("#selectCancellationReasons").UifSelect("getSelected");
                    recovery.CancellationDate = $("#recoveryCancellationdDate").val();
                    recovery.RecuperatorId = recuperatorId;
                    recovery.RecuperatorFullName = $("#_FullName").val();
                    recovery.RecuperatorDocumentNumber = $("#_DocumentNumber").val();
                    recovery.RecoveryTypeId = $("#selectRecoveryType").UifSelect("getSelected");
                    recovery.PrescriptionDate = $("#_recoveryPrescriptionDate").val();
                    recovery.Voucher = $("#_recoveryReceipt").val();
                    recovery.LossResponsible = $("#_recoveryLossResponsible").val();
                    recovery.AssignedCourt = $("#_recoveryAssignedCourt").val();
                    recovery.ExpedientNumber = $("#_recoveryExpedientNumber").val();
                    recovery.AttorneyAssignmentDate = $("#_recoveryAttorneyAssignmentDate").val();
                    recovery.LastReportDate = $("#_recoveryLastReportDate").val();
                    recovery.DebtorId = debtorId;
                    recovery.DebtorFullName = $("#_DebtorFullName").val();
                    recovery.DebtorDocumentNumber = $("#_DebtorDocumentNumber").val();
                    recovery.DebtorAddress = $("#_recoveryDebtorAddress").val();
                    recovery.DebtorPhone = $("#_recoveryDebtorPhone").val();
                    recovery.DebtorIsParticipant = debtorIsParticipant;
                    recovery.CompanyId = $("#_recoveryCompany").val();

                    var PaymentQuotas = $("#tblAgreedPlan").UifDataTable('getData');

                    $.each(PaymentQuotas, function (index, value) {
                        recovery.PaymentQuotas[index] = {};
                        recovery.PaymentQuotas[index].Id = this.Id;
                        recovery.PaymentQuotas[index].Amount = this.Amount;
                        recovery.PaymentQuotas[index].ExpirationDate = this.ExpirationDate;
                        recovery.PaymentQuotas[index].Number = this.Number;
                        recovery.PaymentQuotas[index].CurrencyDescription = this.CurrencyDescription;
                    });

                    recovery.TotalAmount = NotFormatMoney($("#_RecoveryNetAmount").val());
                    recovery.ClaimId = subClaim.ClaimId;
                    recovery.SubClaimId = subClaim.SubClaim;
                    recovery.PrefixId = $("#selectPrefix").UifSelect("getSelected");
                    recovery.BranchId = $("#selectBranch").UifSelect("getSelected");
                    recovery.ClaimNumber = $("#claimRecoverySearchClaimNumber").val();

                    if (document.getElementById("radioRecoveryClassJudicial").checked == true) {
                        recovery.RecoveryClassId = 1;
                    }
                    else if (document.getElementById("radioRecoveryClassExtraJudicial").checked == true) {
                        recovery.RecoveryClassId = 2;
                    }

                    recovery.PaymentPlanId = paymentPlanId;
                    recovery.CurrencyId = $("#selectCurrency").UifSelect("getSelected");
                    recovery.PaymentClassId = $("#selectPaymentClass").UifSelect("getSelected");
                    recovery.TaxPercentage = $("#_RecoveryTax").val();
                    if (Recovery.validateRecoveryPaymentForm(recovery)) {
                        RecoveryRequest.ExecuteRecoveryOperations(recovery).done(function (response) {
                            if (response.success) {
                                $.UifDialog('alert', {
                                    message: String.format(Resources.Language.Recovery + ' No. ' + response.result.Id + ', ' + (recovery.Id == 0 ? ' ' + Resources.Language.Created : ' ' + Resources.Language.Updated) + ' ' + Resources.Language.Correctly)
                                }, function (result) {
                                    Recovery.GetRecoveriesByClaimIdSubClaimId();
                                    Recovery.ClearRecovery();
                                });
                            } else {
                                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                            }
                        }).always(function () {
                            unlockScreen();
                        });
                    } else {
                        unlockScreen();
                        $.UifNotify('show', { 'type': 'danger', 'message': "Por favor llenar todos los campos de plan de pagos", 'autoclose': true });
                    }
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MustSelectSubClaim, 'autoclose': true });
                }

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDebtorSelected, 'autoclose': true });
            }

        } else {
            ScrollTop();
        }
    }

    static validateRecoveryPaymentForm(recovery) {
        let dateStart = $("#_RecoveryFirtPaymentDue").UifDatepicker('getValue');
        let payments = $("#_RecoveryPaymentsNumber").val();
        if (recovery.CurrencyId != "" && recovery.PaymentClassId != "" && recovery.TotalAmount != "" && recovery.TaxPercentage != "" && dateStart != null && payments != null && recovery.PaymentQuotas.length > 0) {
            return true;
        } else if (recovery.CurrencyId == "" && recovery.PaymentClassId == "" && recovery.TotalAmount == "" && recovery.TaxPercentage == "" && dateStart == null && payments == "" && recovery.PaymentQuotas.length == 0) {
            return true;
        } else {
            return false;
        }
        
    }

    static ToEditRecovery(event, recovery) {

        if (recovery == null) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectedRecover, 'autoclose': true });
            return;
        }
        else {
            RecoveryRequest.GetRecoveryByRecoveryId(recovery.Id).done(function (response) {
                if (response.success) {
                    $("#accCancellationRecovery").show();
                    recoveryId = response.result.Id;
                    $("#recoveryNumber").val(recoveryId);
                    $("#recoveryCreatedDate").val(FormatDate(response.result.CreatedDate));
                    $("#_recoveryDescription").val(response.result.Description);
                    $("#selectCancellationReasons").UifSelect("setSelected", response.result.CancellationReasonId);
                    $("#recoveryCancellationdDate").val(FormatDate(response.result.CancellationDate));
                    recuperatorId = response.result.RecuperatorId;
                    $("#_DocumentNumber").val(response.result.RecuperatorDocumentNumber);
                    $("#_FullName").val(response.result.RecuperatorFullName);
                    $("#selectRecoveryType").UifSelect("setSelected", response.result.RecoveryTypeId);
                    $("#_recoveryPrescriptionDate").val(FormatDate(response.result.PrescriptionDate));
                    $("#_recoveryReceipt").val(response.result.Voucher);
                    $("#_recoveryLossResponsible").val(response.result.LossResponsible);
                    $("#_recoveryAssignedCourt").val(response.result.AssignedCourt);
                    $("#_recoveryExpedientNumber").val(response.result.ExpedientNumber);
                    $("#_recoveryAttorneyAssignmentDate").val(FormatDate(response.result.AttorneyAssignmentDate));
                    $("#_recoveryLastReportDate").val(FormatDate(response.result.LastReportDate));
                    debtorId = response.result.DebtorId;
                    debtorIsParticipant = response.result.DebtorIsParticipant;
                    $("#_DebtorDocumentNumber").val(response.result.DebtorDocumentNumber);
                    $("#_DebtorFullName").val(response.result.DebtorFullName);
                    $("#_recoveryDebtorAddress").val(response.result.DebtorAddress);
                    $("#_recoveryDebtorPhone").val(response.result.DebtorPhone);
                    $("#selectPaymentClass").UifSelect("setSelected", response.result.PaymentClassId);
                    $("#selectCurrency").UifSelect("setSelected", response.result.CurrencyId);

                    var netAmount = 0;

                    $.each(response.result.PaymentQuotas, function (index, value) {
                        netAmount += this.Amount;
                        this.ExpirationDate = FormatDate(this.ExpirationDate);
                        this.CurrencyDescription = $("#selectCurrency").UifSelect("getSelectedText");
                    });

                    $("#tblAgreedPlan").UifDataTable({ sourceData: response.result.PaymentQuotas });

                    $("#_recoveryCompany").val(response.result.CompanyId);
                    $("#_RecoveryTotal").val(FormatMoney(netAmount));
                    $("#_RecoveryNetAmount").val(FormatMoney(response.result.TotalAmount));
                    $('[name="RecoveryClass"]:checked').val(response.result.RecoveryClassId);
                    $("#_RecoveryTax").val(response.result.TaxPercentage);
                    if (response.result.PaymentQuotas.length > 0) {
                        $("#_RecoveryFirtPaymentDue").UifDatepicker('setValue',(FormatDate(response.result.PaymentQuotas[0].ExpirationDate)));
                        $("#_RecoveryPaymentsNumber").val(response.result.PaymentQuotas.length);
                        paymentPlanId = response.result.PaymentPlanId;
                    }
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
    }

    static SetRecoverySumary(recovery, financialPlan) {
        $("#tblSalvagesSummary").UifDataTable('clear');
        var subClaim = $("#tblSubClaims").UifDataTable('getSelected')[0];
        var RecoverySummary = {};
        $.each(financialPlan, function (index, value) {
            RecoverySummary.ClaimCode = recovery.ClaimId;
            RecoverySummary.SubClaimId = recovery.SubClaimId;
            RecoverySummary.Risk = subClaim.RiskDescription;
            RecoverySummary.Coverage = subClaim.CoverageDescription;
            RecoverySummary.InsuredThird = subClaim.Insured;
            RecoverySummary.Currency = subClaim.Currency;
            RecoverySummary.Number = recovery.Id;
            RecoverySummary.recoveryType = recovery.recoveryType;
            RecoverySummary.Description = recovery.Description;
            RecoverySummary.DebtorFullName = recovery.debtor.Description;
            RecoverySummary.TotalAmount = recovery.TotalAmount;
            RecoverySummary.RemainingAmmount = recovery.RemainingAmmount;

            $("#tblRecoveriesSummary").UifDataTable('addRow', RecoverySummary);
        });
    }

    static ValidateRecoveryCreatedDate() {
        var recoveryCreatedDate = $("#recoveryCreatedDate").val();

        var partsDate = accountingDate.split("/");
        var lastDateMonthClose = new Date(partsDate[2], partsDate[1]);
        lastDateMonthClose.setDate(0);

        if (CompareClaimDates(recoveryCreatedDate, lastDateMonthClose.getFromFormat("DD/MM/YYYY"))) {
            var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.EntryDate + ': ' + recoveryCreatedDate + '" ' + Resources.Language.GreaterThanDate + ' "' + lastDateMonthClose + '"';
            $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
            $("#recoveryCreatedDate").UifDatepicker('clear');
        }
    }

    static DoRecoveryClaim() {
        if (modelSearchCriteria.claimNumber == null || modelSearchCriteria.prefixId == null || modelSearchCriteria.branchId == null)
            return;

        Recovery.GetPrefixes(function (prefixes) {
            $("#selectPrefix").UifSelect({ sourceData: prefixes, enable: false });
            $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
            Recovery.GetBranches(function (branches) {
                $("#selectBranch").UifSelect({ sourceData: branches, enable: false });
                $("#selectBranch").UifSelect('setSelected', modelSearchCriteria.branchId);

                $("#inputPolicyDocumentNumber").val(modelSearchCriteria.policyDocumentNumber);
                $('#inputPolicyDocumentNumber').prop('disabled', true);

                $('#claimRecoverySearchClaimNumber').val(modelSearchCriteria.claimNumber);
                $('#claimRecoverySearchClaimNumber').prop('disabled', true);

                Recovery.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber();
            });
        });
    }

    //Limpia todos los campos en pantalla
    static ClearRecovery() {
        $("#recoveryNumber").val('');
        $("#recoveryCreatedDate").val('');
        $("#_recoveryDescription").val('');
        $("#recoveryCancellationdDate").val('');
        $("#selectCancellationReasons").UifSelect('setSelected', null);
        recuperatorId = null;
        $("#_DocumentNumber").UifAutoComplete('clean');
        $("#_FullName").UifAutoComplete('clean');
        $("#selectRecoveryType").UifSelect('setSelected', null);
        $("#_recoveryPrescriptionDate").val('');
        $("#_recoveryCompany").val('');
        $("#_recoveryReceipt").val('');
        $("#_recoveryLossResponsible").val('');
        $("#_recoveryAssignedCourt").val('');
        $("#_recoveryExpedientNumber").val('');
        $("#_recoveryAttorneyAssignmentDate").val('');
        $("#_recoveryLastReportDate").val('');
        debtorId = null;
        $("#_DebtorDocumentNumber").UifAutoComplete('clean');
        $("#_DebtorFullName").UifAutoComplete('clean');
        $("#_recoveryDebtorAddress").val('');
        $("#_recoveryDebtorPhone").val('');
        $("#_recoveryLastReportDate").val('');        
        $("#selectPaymentClass").UifSelect('setSelected', null);
        $("#selectCurrency").UifSelect('setSelected', null);
        $("#_RecoveryNetAmount").val('');
        $("#_RecoveryTax").val('');
        $("#_RecoveryTotal").val('');
        $("#_RecoveryFirtPaymentDue").UifDatepicker('setValue', null);
        $("#_RecoveryPaymentsNumber").val('');

        $("#tblAgreedPlan").UifDataTable('clear');
        $("#tblPayedPlan").UifDataTable('clear');
        debtorIsParticipant = null;
    }

    static OnlyNumbers(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if ($(this).val() != '')
            $(this).val(FormatMoney($(this).val().includes(',') ? $(this).val().replace(',', '.') : $(this).val()));
    }
}