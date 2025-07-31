var buyerId = null;
var saleId = null;
var paymentPlanId = null;
var endorsementId = null;
var policyId = null;
var isParticipant = null;

﻿class Salvage extends Uif2.Page {
    getInitialState() {
        Salvage.InitialSalvage();
    }

    bindEvents() {

        // Uppercase
        $("#SalvageDescription").TextTransform(ValidatorType.UpperCase);
        $("#SalvageLocation").TextTransform(ValidatorType.UpperCase);
        $("#SalvageObservations").TextTransform(ValidatorType.UpperCase);
        
        $("#_saleDescription").TextTransform(ValidatorType.UpperCase);
        $("#_BuyerFullName").TextTransform(ValidatorType.UpperCase);
        $("#_BuyerAddress").TextTransform(ValidatorType.UpperCase);

        /* Clicks */
        $("#btnSalvageSearch").on('click', Salvage.SearchSubClaims);
        $("#btnGetPolicyInfo").on('click', Salvage.OpenModalPolicyConsult);
        $("#btnClearSale").on('click', Salvage.ClearSale);
        $("#btnSaveSalvage").on('click', Salvage.ExecuteSalvageOperations);
        $("#btnSaveSale").on('click', Salvage.ExecuteSaleOperations);
        $("#btnCancelSalvage").on('click', Salvage.ClearSalvageModal);
        $("#tabSalvageId").on('click', function () {
            $("#btnSaveSale").parent().hide();
            $("#btnClearSale").parent().hide();
            ClearValidation("#salvageForm");
        });
        $("#_tabSale").on('click', function () {
            $("#btnSaveSale").parent().show();
            $("#btnClearSale").parent().show();
        });
        $("#tabSalvageSummaryId").on('click', function () {
            $("#btnSaveSale").parent().hide();
            $("#btnClearSale").parent().hide();
            ClearValidation("#salvageForm");
        });

        /* Rows add */
        $("#tblSalvages").on('rowAdd', Salvage.OpenSalvageModal);
        $("#tblSales").on('rowAdd', Salvage.ToAddSale);

        /* Rows Edit */
        $("#tblSales").on('rowEdit', Salvage.ToEditSale);
        $("#tblSalvages").on('rowEdit', Salvage.ToEditSalvage);

        /* Rows Delete */
        $("#tblSales").on('rowDelete', Salvage.DeleteSale);
        $("#tblSalvages").on('rowDelete', Salvage.DeleteSalvage);

        /* Rows selected */
        $("#tblSubClaims").on('rowSelected', Salvage.GetSalvagesByClaimIdSubClaimId);
        $("#tblSalvages").on('rowSelected', Salvage.GetSalesBySalvageId);

        /* Items selected */
        $("#_BuyerDocumentNumber").on('itemSelected', Salvage.SetBuyerInformation);
        $("#_BuyerFullName").on('itemSelected', Salvage.SetBuyerInformation);
        $("#SalvageEstimatedSale").focusin(Salvage.NotFormatMoneyIn);
        $("#SalvageEstimatedSale").focusout(Salvage.FormatMoneyOut);
        $("#_SaleNetAmount").focusin(Salvage.NotFormatMoneyIn);
        $("#_SaleNetAmount").focusout(Salvage.FormatMoneyOut);

        /* Triggers */
        $("#_SaleTax").blur(Salvage.CalculateTotalOfSale);
        $("#_SalePaymentsNumber").blur(Salvage.CalculateSaleAgreedPlan);
        $('#modalSalvage').on('hidden.bs.modal', Salvage.ClearSalvageModal);

        /* Only Numbers */
        $("#inputClaimNumber").on('keypress', Salvage.OnlyNumbers);
        $("#_saleQuantity").on('keypress', Salvage.OnlyNumbers);        
        $("#_BuyerPhone").on('keypress', Salvage.OnlyNumbers);
        $("#_SaleNetAmount").on('keypress', Salvage.OnlyNumbers);
        $("#_SaleTax").on('keypress', Salvage.OnlyNumbers);
        $("#_SalePaymentsNumber").on('keypress', Salvage.OnlyNumbers);
        $("#SalvageEstimatedSale").on('keypress', Salvage.OnlyNumbers);
        $("#SalvageUnitQuantity").on('keypress', Salvage.OnlyNumbers);

        /* Change */
        $("#_BuyerDocumentNumber").on('keydown', Salvage.ClearBuyerInformation);
        $("#_BuyerFullName").on('keydown', Salvage.ClearBuyerInformation);
    }

    static InitialSalvage() {
        $('#_policyId').val('');
        $("#_tabSale").hide();
        if (modelSearchCriteria.claimNumber == null && modelSearchCriteria.prefixId == null && modelSearchCriteria.branchId == null) {
            Salvage.GetPrefixes();
            Salvage.GetBranches();
        }

        Salvage.DoSalvageClaim();

        SalvageRequest.GetPaymentClasses().done(function (response) {
            if (response.success) {
                $("#ddlSalePaymentType").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        SalvageRequest.GetCurrencies().done(function (response) {
            if (response.success) {
                $("#ddlSalePaymentCurrency").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

        SalvageRequest.GetCancellationReasons().done(function (response) {
            if (response.success) {
                $("#ddlSaleCancellationdReason").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetPrefixes(callback) {
        SalvageRequest.GetPrefixes().done(function (response) {
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
        SalvageRequest.GetBranches().done(function (response) {
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

    static SearchSubClaims() {
        $("#salvageForm").validate();

        if ($("#salvageForm").valid()) {
            Salvage.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber();
        }
    }

    static GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber() {

        var prefixId = $("#selectPrefix").UifSelect('getSelected');
        var branchId = $("#selectBranch").UifSelect('getSelected');
        var policyDocumentNumber = $("#inputPolicyDocumentNumber").val();
        var claimNumber = $("#inputClaimNumber").val();
        SalvageRequest.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber).done(function (response) {
            if (response.success) {
                if (response.result.length > 0) {
                    $("#selectPrefix").UifSelect('setSelected', response.result[0].PrefixId);
                    $("#selectBranch").UifSelect('setSelected', response.result[0].BranchCode);
                    $('#tblSubClaims').UifDataTable({ sourceData: response.result });
                    Salvage.GetPolicyInformation();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ClaimNotFound, 'autoclose': true });
                }
            }
            else {
                Salvage.ClearClaimInformationSerach();
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetSalesBySalvageId(event, rowSelected) {
        if (rowSelected == null) {
            rowSelected = $("#tblSalvages").UifDataTable('getSelected')[0];
        }

        var salvageId = rowSelected.Id;
        $("#_tabSale").show();

        SalvageRequest.GetSalesBySalvageId(salvageId).done(function (response) {
            if (response.success) {
                $.each(response.result, function (index, value) {
                    this.Date = FormatDate(this.Date);
                });
                $('#tblSales').UifDataTable({ sourceData: response.result });

                if (response.result.length > 0) {
                    Salvage.SetSalvageSumary(rowSelected, response.result);
                }

            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetSalvagesByClaimIdSubClaimId(event, rowSelected) {

        if (rowSelected == null) {
            rowSelected = $("#tblSubClaims").UifDataTable('getSelected')[0];
        }

        var claimId = rowSelected.ClaimId;
        var subClaimId = rowSelected.SubClaim;

        SalvageRequest.GetSalvagesByClaimIdSubClaimId(claimId, subClaimId).done(function (response) {
            if (response.success) {
                $.each(response.result, function (index, value) {
                    this.CreationDate = FormatDate(this.CreationDate);
                    this.EndDate = FormatDate(this.EndDate);
                    this.AssignmentDate = FormatDate(this.AssignmentDate);
                });

                $('#tblSalvages').UifDataTable({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetSalvageBySalvageId(salvageId) {
        SalvageRequest.GetSalvageBySalvageId(salvageId).done(function (response) {
            if (response.success) {
                $('#tblSalvagesSummary').UifDataTable({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetPolicyInformation() {
        var subClaim = $("#tblSubClaims").UifDataTable('getData')[0];
        if (subClaim != null) {
            endorsementId = subClaim.EndorsementId;
            Salvage.GetPolicyByEndorsementIdModuleType();
        }
        else {
            Salvage.ClearPolicyInformation();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PolicyNotFound, 'autoclose': true });
        }
    }

    static GetPolicyByEndorsementIdModuleType() {
        if (endorsementId === null || endorsementId === "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.IncorrectNumberEndorsement, 'autoclose': true });
            return;
        }

        SalvageRequest.GetPolicyByEndorsementIdModuleType(endorsementId).done(function (response) {
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

    static SetSalvageSumary(salvage, sales) {
        $("#tblSalvagesSummary").UifDataTable('clear');
        var subClaim = $("#tblSubClaims").UifDataTable('getSelected')[0];
        var SalvageSummary = {};
        $.each(sales, function (index, value) {
            SalvageSummary.ClaimCode = salvage.ClaimId;
            SalvageSummary.SubClaim = salvage.SubClaimId;
            SalvageSummary.Risk = subClaim.RiskDescription;
            SalvageSummary.Coverage = subClaim.CoverageDescription;
            SalvageSummary.InsuredThird = subClaim.Insured;
            SalvageSummary.Currency = subClaim.Currency;
            SalvageSummary.Number = salvage.Id;
            SalvageSummary.Description = salvage.Description;
            SalvageSummary.Quantity = this.QuantitySold;
            SalvageSummary.SaleNumber = this.Id;
            SalvageSummary.SaleBuyer = this.BuyerFullName;
            SalvageSummary.SaleQuantity = this.QuantitySold;
            SalvageSummary.SaleAmount = this.TotalAmount;

            $("#tblSalvagesSummary").UifDataTable('addRow', SalvageSummary);
        });
    }

    static SetBuyerInformation(event, buyer) {
        $('#_BuyerDocumentNumber').UifAutoComplete('setValue', buyer.DocumentNumber);
        $('#_BuyerFullName').UifAutoComplete('setValue', buyer.FullName);
        $("#_BuyerAddress").val(buyer.Address);
        $("#_BuyerPhone").val(buyer.Phone);

        if (buyer.Address == "") {
            $("#_BuyerAddress").prop('disabled', true);
        }

        if (buyer.Phone == "") {
            $("#_BuyerPhone").prop('disabled', true);
        }

        buyerId = buyer.IndividualId;
    }

    static ClearBuyerInformation() {
        buyerId = null;
    }

    static ToEditSalvage(event, salvage) {
        $("#SalvageId").val(salvage.Id);
        $("#SalvageCreationDate").val(FormatDate(salvage.CreationDate));
        $("#SalvageAssignmentDate").val(FormatDate(salvage.AssignmentDate));
        $("#SalvageEndDate").val(FormatDate(salvage.EndDate));
        $("#SalvageDescription").val(salvage.Description);
        $("#SalvageUnitQuantity").val(salvage.UnitsQuantity);
        $("#SalvageEstimatedSale").val(FormatMoney(salvage.EstimatedSale));
        $("#SalvageLocation").val(salvage.Location);
        $("#SalvageObservations").val(salvage.Observations);

        Salvage.OpenSalvageModal();
    }

    static ExecuteSalvageOperations() {
        $("#modalSalvageForm").validate();
        if ($("#modalSalvageForm").valid()) {
            var subClaim = $("#tblSubClaims").UifDataTable('getSelected')[0];

            if (subClaim != null) {
                lockScreen();
                var salvage = {};
                salvage.Id = $("#SalvageId").val();
                salvage.BranchId = $("#selectBranch").UifSelect("getSelected");
                salvage.PrefixId = $("#selectPrefix").UifSelect("getSelected");
                salvage.ClaimId = subClaim.ClaimId;
                salvage.ClaimNumber = $("#inputClaimNumber").val();
                salvage.Description = $("#SalvageDescription").val();
                salvage.AssignmentDate = $("#SalvageAssignmentDate").val();
                salvage.CreationDate = $("#SalvageCreationDate").val();
                salvage.EndDate = $("#SalvageEndDate").val();
                salvage.Location = $("#SalvageLocation").val();
                salvage.Observations = $("#SalvageObservations").val();
                salvage.SubClaimId = subClaim.SubClaim;
                salvage.EstimatedSale = NotFormatMoney($("#SalvageEstimatedSale").val());
                salvage.UnitsQuantity = $("#SalvageUnitQuantity").val();

                SalvageRequest.ExecuteSalvageOperations(salvage).done(function (response) {
                    if (response.success) {
                        $.UifDialog('alert', {
                            message: Resources.Language.Salvage + ' No. ' + response.result.Id + ', ' + (salvage.Id == 0 ? ' ' + Resources.Language.Created : ' ' + Resources.Language.Updated)
                        }, function (result) {
                            Salvage.GetSalvagesByClaimIdSubClaimId();
                            Salvage.ClearSalvageModal();
                        });
                        $("#modalSalvage").UifModal('hide');
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                    }                    
                }).always(function () {
                    unlockScreen();
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MustSelectSubClaim, 'autoclose': true });

            }
        } else {
            $(".error").each(function (index) {
                $(this).addClass("text-danger");
            });
        }
    }

    static DeleteSalvage(event, salvage) {
        SalvageRequest.DeleteSalvage(salvage.Id).done(function (response) {
            if (response.success) {
                $.UifDialog('alert', {
                    message: Resources.Language.DeleteSalvage
                }, function (result) {
                });

                Salvage.GetSalvagesByClaimIdSubClaimId();
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static ToAddSale() {
        $("#accSaleCancellation").hide();
        Salvage.ValidateSale();
        Salvage.ClearSale();
        ScrollTop();
        $("#btnSaveSale").parent().show();
        $("#btnClearSale").parent().show();
    }

    static ToEditSale(event, sale) {
        SalvageRequest.GetSaleBySaleId(sale.Id).done(function (response) {
            if (response.success) {
                if (response.result != null) {
                    saleId = sale.Id;
                    paymentPlanId = response.result.PaymentPlanId;
                    isParticipant = response.result.IsParticipant;
                    buyerId = response.result.BuyerId;
                    $("#_saleDate").val(FormatDate(response.result.Date));
                    $("#_saleCancellationDate").val(FormatDate(response.result.CancellationDate));
                    $("#_saleDescription").val(response.result.Description);
                    $("#_saleQuantity").val(response.result.QuantitySold);

                    $("#_BuyerDocumentNumber").val(response.result.BuyerDocumentNumber);
                    $("#_BuyerFullName").val(response.result.BuyerFullName);
                    $("#_BuyerAddress").val(response.result.BuyerAddress);
                    $("#_BuyerPhone").val(response.result.BuyerPhone);

                    $("#_SaleTotal").val(FormatMoney(response.result.TotalAmount));
                    $("#_SalePaymentsNumber").val(response.result.PaymentQuotas.length);
                    $("#ddlSalePaymentCurrency").UifSelect("setSelected", response.result.CurrencyId);
                    $("#ddlSalePaymentType").UifSelect("setSelected", response.result.PaymentClassId);
                    $("#_SaleTax").val(response.result.Tax);

                    $.each(response.result.PaymentQuotas, function (index, value) {
                        this.ExpirationDate = FormatDate(this.ExpirationDate);
                        this.CurrencyDescription = $("#ddlSalePaymentCurrency").UifSelect("getSelectedText");
                    });

                    $("#_SaleFirtPaymentDue").val(FormatDate(response.result.PaymentQuotas[0].ExpirationDate));
                    var saleNetAmount = (response.result.TotalAmount * 100) / (100 + response.result.Tax);
                    $("#_SaleNetAmount").val(FormatMoney(saleNetAmount));
                    $("#ddlSaleCancellationdReason").UifSelect("setSelected", response.result.CancellationReasonId == 0 ? null : response.result.CancellationReasonId);
                    $("#tblSaleAgreedPlan").UifDataTable({ sourceData: response.result.PaymentQuotas });
                    Salvage.ValidateSale();
                    $("#accSaleCancellation").show();
                    $("#btnSaveSale").parent().show();
                    $("#btnClearSale").parent().show();
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static ExecuteSaleOperations() {
        $("#salvageForm").validate();
        if ($("#salvageForm").valid()) {
            if (buyerId != null) {
                lockScreen();
                var sale = {
                    PaymentQuotas: []
                };
                var salvageId = $("#tblSalvages").UifDataTable('getSelected')[0].Id;
                sale.Id = saleId;
                sale.Date = $("#_saleDate").val();
                sale.Description = $("#_saleDescription").val();
                sale.QuantitySold = $("#_saleQuantity").val();
                sale.CancellationReasonId = $("#ddlSaleCancellationdReason").UifSelect("getSelected");
                sale.CancellationDate = $("#_saleCancellationDate").val();
                sale.TotalAmount = NotFormatMoney($("#_SaleTotal").val());
                sale.saleNetAmount = FormatMoney($("#_SaleNetAmount").val());
                sale.BuyerId = buyerId;
                sale.BuyerDocumentNumber = $("#_BuyerDocumentNumber").val();
                sale.BuyerFullName = $("#_BuyerFullName").val();
                sale.BuyerAddress = $("#_BuyerAddress").val();
                sale.BuyerPhone = $("#_BuyerPhone").val();
                sale.Tax = $("#_SaleTax").val();
                sale.CurrencyId = $("#ddlSalePaymentCurrency").UifSelect("getSelected");
                sale.PaymentClassId = $("#ddlSalePaymentType").UifSelect("getSelected");
                sale.PaymentPlanId = paymentPlanId;
                sale.IsParticipant = isParticipant;


                var paymentQuotas = $("#tblSaleAgreedPlan").UifDataTable('getData');

                $.each(paymentQuotas, function (index, value) {
                    sale.PaymentQuotas[index] = {};
                    sale.PaymentQuotas[index].Id = this.Id;
                    sale.PaymentQuotas[index].Amount = this.Amount;
                    sale.PaymentQuotas[index].ExpirationDate = this.ExpirationDate;
                    sale.PaymentQuotas[index].Number = this.Number;
                    sale.PaymentQuotas[index].CurrencyDescription = this.CurrencyDescription;
                });

                SalvageRequest.ExecuteSaleOperations(sale, salvageId).done(function (response) {
                    if (response.success) {
                        $.UifDialog('alert', {
                            message: Resources.Language.Sale + ' No. ' + response.result.Id + ', ' + (sale.Id == null ? ' ' + Resources.Language.CreatedSale : ' ' + Resources.Language.UpdatedSale)
                        }, function (result) {
                            Salvage.ClearSale();
                            Salvage.GetSalesBySalvageId();
                        });
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                    }
                }).always(function () {
                    unlockScreen();
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorBuyerSelected, 'autoclose': true });
            }          

        }
    }

    static DeleteSale(event, sale) {
        console.log(sale);
        SalvageRequest.DeleteSale(sale.Id).done(function (response) {
            if (response.success) {
                $.UifDialog('alert', {
                    message: Resources.Language.DeleteSale
                }, function (result) {
                });
                Salvage.GetSalesBySalvageId();
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static DoSalvageClaim() {
        if (modelSearchCriteria.claimNumber == null || modelSearchCriteria.prefixId == null || modelSearchCriteria.branchId == null)
            return;

        Salvage.GetPrefixes(function (prefixes) {
            $("#selectPrefix").UifSelect({ sourceData: prefixes, enable: false });
            $("#selectPrefix").UifSelect('setSelected', modelSearchCriteria.prefixId);
            Salvage.GetBranches(function (branches) {
                $("#selectBranch").UifSelect({ sourceData: branches, enable: false });
                $("#selectBranch").UifSelect('setSelected', modelSearchCriteria.branchId);

                $("#inputPolicyDocumentNumber").val(modelSearchCriteria.policyDocumentNumber);
                $('#inputPolicyDocumentNumber').prop('disabled', true);

                $('#inputClaimNumber').val(modelSearchCriteria.claimNumber);
                $('#inputClaimNumber').prop('disabled', true);

                Salvage.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber();
            });
        });
    }

    static OpenSalvageModal() {
        var subClaims = $("#tblSubClaims").UifDataTable('getSelected');
        if (subClaims == null) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MustSelectSubClaim, 'autoclose': true });
            return;
        } else {
            $("#modalSalvage").UifModal("showLocal", Resources.Language.Salvages);
        }
    }

    static OpenModalPolicyConsult() {
        Salvage.GetClaimsByPolicyId(policyId);
        $('#modalPolicyConsult').UifModal('showLocal', Resources.Language.LabelPoliceInformation);
    }

    static GetClaimsByPolicyId(policyId) {
        SalvageRequest.GetClaimsByPolicyId(policyId).done(function (data) {
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
    static ClearPolicyInformation() {
        $("#_branch").val("");
        $("#_prefix").val("");
        $("#_policy").val("");
        $("#_endorsement").val("");
        $("#_nameInsured").val("");
    }

    static ClearSale() {
        saleId = null;
        buyerId = null;
        paymentPlanId = null;
        isParticipant = null;
        $("#_saleDate").val("");
        $("#_saleCancellationDate").val("");
        $("#_saleDescription").val("");
        $("#_saleQuantity").val("");
        $("#_BuyerDocumentNumber").UifAutoComplete('clean');
        $("#_BuyerFullName").UifAutoComplete('clean');
        $("#_BuyerAddress").val("");
        $("#_BuyerPhone").val("");
        $("#_SaleNetAmount").val("");
        $("#_SaleTax").val("");
        $("#_SaleTotal").val("");
        $("#_SaleFirtPaymentDue").val("");
        $("#_SalePaymentsNumber").val("");
        $("#ddlSaleCancellationdReason").UifSelect('setSelected', "");
        $("#ddlSalePaymentCurrency").UifSelect('setSelected', "");
        $("#ddlSalePaymentType").UifSelect('setSelected', "");
        $("#accSaleCancellation").hide();
        $("#tblSaleAgreedPlan").UifDataTable('clear');

    }

    static ClearSalvageModal() {
        $("#SalvageId").val("");
        $("#SalvageDescription").val("");
        $("#SalvageAssignmentDate").val("");
        $("#SalvageCreationDate").val("");
        $("#SalvageEndDate").val("");
        $("#SalvageLocation").val("");
        $("#SalvageObservations").val("");
        $("#SalvageEstimatedSale").val("");
        $("#SalvageUnitQuantity").val("");
        ClearValidation("#modalSalvageForm");
    }

    static ClearClaimInformationSerach() {
        $("#selectBranch").UifSelect('setSelected', null);
        $("#selectBranch").UifSelect("disabled", false);
        $("#selectPrefix").UifSelect('setSelected', null);
        $("#selectPrefix").UifSelect("disabled", false);
        $("#inputPolicyDocumentNumber").val("");
        $('#inputPolicyDocumentNumber').prop('disabled', false);
        $('#inputClaimNumber').val("");
        $('#inputClaimNumber').prop('disabled', false);
    }

    static OnlyNumbers(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    static CalculateTotalOfSale() {
        //Se crea un nuevo plan de pago
        paymentPlanId = 0;

        var iNet, tax, total;

        iNet = parseFloat(NotFormatMoney($("#_SaleNetAmount").val()));

        if ($("#_SaleTax").val() != "") {
            tax = parseFloat($("#_SaleTax").val()) / 100;
        } else {
            tax = 0;
        }

        total = iNet + tax * iNet;
        $("#_SaleTotal").val(FormatMoney(total));
    }

    static CalculateSaleAgreedPlan() {
        //Se crea un nuevo plan de pago
        paymentPlanId = 0;

        Salvage.CalculateTotalOfSale();
        var totalSale = NotFormatMoney($("#_SaleTotal").val());
        var currencyDescription = $("#ddlSalePaymentCurrency").UifSelect("getSelectedSource").Description;
        var payments = $("#_SalePaymentsNumber").val();
        var dateStart = $("#_SaleFirtPaymentDue").UifDatepicker('getValue');

        if (dateStart == "") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AddExpirationDate, 'autoclose': true });
            return;
        }

        if (payments == "") {
            payments = 1;
        }

        SalvageRequest.CalculateSaleAgreedPlan(dateStart, totalSale, payments, currencyDescription).done(function (response) {
            if (response.success) {
                $.each(response.result, function (index, value) {
                    this.ExpirationDate = FormatDate(this.ExpirationDate);
                });
                $("#tblSaleAgreedPlan").UifDataTable({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static ValidateSale() {
        var salvage = $("#tblSalvages").UifDataTable('getSelected');
        if (salvage == null) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SalvageSelected, 'autoclose': true });
            return;
        } else {
            $("#_tabSale").trigger('click');
            ScrollTop();
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