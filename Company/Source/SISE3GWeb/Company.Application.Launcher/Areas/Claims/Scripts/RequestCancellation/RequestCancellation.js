var ClaimsPaymentRequestDTO = null;
var ClaimsChargeRequestDTO = null;

class RequestCancellation extends Uif2.Page {

    getInitialState() {
        RequestCancellation.InitialRequestCancellation();
    }

    /** EVENTOS**/
    bindEvents() {
        $('#btnSearchRequest').on('click', RequestCancellation.GetPaymentChargeRequestByPrefixIdBranchIdNumber);
        $('#SaveRequestCancellation').on('click', RequestCancellation.SaveRequestCancellation);
        $('#inputRequestNumber').ValidatorKey(ValidatorType.Number, 2, 1);
    }

    /** METODOS GET**/
    static InitialRequestCancellation() {
        $("#paymentRequestInformation").hide();
        $("#ChargeRequestInformation").hide();

        if (modelSearchCriteria.branchId == null && modelSearchCriteria.prefixId == null && modelSearchCriteria.paymentRequestNumber == null) {
            RequestCancellation.GetBranches();
            RequestCancellation.GetPrefixes();
        }

        RequestCancellation.DoCancelPaymentChargeRequest();
    }

    static GetBranches(callback) {
        ClaimsPaymentRequest.GetBranches().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);

                $('#selectBranch').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetPaymentBranches(paymentBranchId) {
        ClaimsPaymentRequest.GetBranches().done(function (response) {
            if (response.success) {
                $('#selectPaymentBranch').UifSelect({ sourceData: response.result, enable: false });
                $('#selectPaymentBranch').UifSelect('setSelected', paymentBranchId);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetRecoveryBranches(recoveryBranchId) {
        ClaimsPaymentRequest.GetBranches().done(function (response) {
            if (response.success) {
                $('#selectRecoveryBranch').UifSelect({ sourceData: response.result, enable: false });
                $('#selectRecoveryBranch').UifSelect('setSelected', recoveryBranchId);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetPrefixes(callback) {
        ClaimsPaymentRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#selectPrefix").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static DoCancelPaymentChargeRequest() {
        if (modelSearchCriteria.paymentRequestNumber == null || modelSearchCriteria.branchId == null || modelSearchCriteria.prefixId == null)
            return;

        RequestCancellation.GetPrefixes(function (prefixes) {
            $('#selectPrefix').UifSelect({ sourceData: prefixes });
            $('#selectPrefix').UifSelect('setSelected', modelSearchCriteria.prefixId);

            RequestCancellation.GetBranches(function (branches) {
                $('#selectBranch').UifSelect({ sourceData: branches });
                $('#selectBranch').UifSelect('setSelected', modelSearchCriteria.branchId);
                $('#inputRequestNumber').val(modelSearchCriteria.paymentRequestNumber);
                $('#inputInformationCancelation').text('');
                $('#divLabelInformationCancellation').hide();
                RequestCancellation.GetPaymentChargeRequestByPrefixIdBranchIdNumber();
            });
        });
    }

    static GetPaymentSource(paymentSourceId, movementTypeId) {
        ClaimsPaymentRequest.GetPaymentSource().done(function (data) {
            if (data.success) {
                $('#selectRecoveryPaymentSource').UifSelect({ sourceData: data.result, enable: false });
                $("#selectRecoveryPaymentSource").UifSelect('setSelected', paymentSourceId);

                $('#selectPaymentSource').UifSelect({ sourceData: data.result, enable: false });
                $("#selectPaymentSource").UifSelect('setSelected', paymentSourceId);
                Payment.GetPaymentMovementTypesByPaymentSourceId(function (movementTypes) {
                    $("#selectRecoveryMovementType").UifSelect({ sourceData: movementTypes, enable: false });
                    $("#selectRecoveryMovementType").UifSelect('setSelected', movementTypeId);

                    $("#selectMovementType").UifSelect({ sourceData: movementTypes, enable: false });
                    $("#selectMovementType").UifSelect('setSelected', movementTypeId);
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGettingSourcePayment, 'autoclose': true });
            }
        });
    }

    static GetPersonTypes(payToId) {
        ClaimsPaymentRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $('#selectCollectTo').UifSelect({ sourceData: data.result, enable: false });
                $('#selectCollectTo').UifSelect('setSelected', payToId);

                $('#selectPayTo').UifSelect({ sourceData: data.result, enable: false });
                $('#selectPayTo').UifSelect('setSelected', payToId);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGettingWhomPayment, 'autoclose': true });
            }
        });
    }

    static GetCollectPersonTypes(collectToId) {
        ChargeRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $('#selectCollectTo').UifSelect({ sourceData: data.result, enable: false });
                $('#selectCollectTo').UifSelect('setSelected', collectToId);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGettingWhomPayment, 'autoclose': true });
            }
        });
    }

    static GetCurrency(currencyId) {
        ClaimsPaymentRequest.GetCurrency().done(function (data) {
            if (data.success) {
                $('#selectPaymentCurrency').UifSelect({ sourceData: data.result, enable: false });
                $('#selectPaymentCurrency').UifSelect('setSelected', currencyId);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetCurrency, 'autoclose': true });
            }
        });
    }

    static GetPaymentMethod(paymentMethodId) {
        ClaimsPaymentRequest.GetPaymentMethod().done(function (data) {
            if (data.success) {
                $('#selectPaymentMethod').UifSelect({ sourceData: data.result, enable: false });
                $('#selectPaymentMethod').UifSelect('setSelected', paymentMethodId);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetPaymentMethod, 'autoclose': true });
            }
        });
    }

    static GetPaymentChargeRequestByPrefixIdBranchIdNumber() {
        $("#paymentRequestInformation").hide();
        $("#ChargeRequestInformation").hide();

        $("#formRescue").validate();
        if ($("#formRescue").valid()) {

            var prefixId = $("#selectPrefix").UifSelect("getSelected");
            var branchId = $("#selectBranch").UifSelect("getSelected");
            var number = $("#inputRequestNumber").val();

            RequestCancellationRequest.GetPaymentChargeRequestByPrefixIdBranchIdNumber(prefixId, branchId, number).done(function (response) {
                if (response.success) {
                    if (response.result == null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PaymentRequestDontExists, 'autoclose': true });
                        $("#paymentRequestInformation").hide();
                        return null;
                    }

                    switch (response.result.PaymentRequestTypeId) {
                        case 1:
                            RequestCancellation.SetPaymentRequestInformation(response.result);
                            break;
                        case 2:
                            RequestCancellation.SetChargeRequestInformation(response.result);
                            break;
                        default:
                    }
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
    }

    static SetPaymentRequestInformation(paymentRequest) {
        ClaimsPaymentRequestDTO = paymentRequest;

        RequestCancellationRequest.GetPaymentRequestClaimsByPaymentRequestId(paymentRequest.Id).done(function (response) {
            if (response.success) {

                response.result[0].Claims.forEach(function (item, index) {
                    item.Total = (item.TotalConcept + item.TotalTax + item.TotalRetention)
                });

                $("#tblPaymentRequestClaims").UifDataTable({ sourceData: response.result[0].Claims, enable: false });

                $("#inputTotalFinish").val(FormatMoney(response.result[0].TotalAmount));
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        RequestCancellation.GetPaymentSource(paymentRequest.PaymentSourceId, paymentRequest.MovementTypeId);
        RequestCancellation.GetPersonTypes(paymentRequest.PersonTypeId);
        RequestCancellation.GetCurrency(paymentRequest.CurrencyId);
        RequestCancellation.GetPaymentMethod(paymentRequest.PaymentMethodId);
        RequestCancellation.GetPaymentBranches(paymentRequest.BranchId);
        $("#DocumentNumberInsured").val(paymentRequest.BeneficiaryDocumentNumber);
        $("#inputDescription").val(paymentRequest.Description);
        $("#FullNameInsured").val(paymentRequest.BeneficiaryFullName);
        $("#inputEstimatedDate").val(FormatDate(paymentRequest.EstimatedDate));
        $("#inputRegistrationDate").val(FormatDate(paymentRequest.RegistrationDate));
        $("#inputAccountingDate").val(FormatDate(paymentRequest.AccountingDate));
        $("#paymentRequestInformation").show();
        $('#inputInformationCancelation').text($("#selectBranch").UifSelect("getSelectedText").substring(0, 3) + '-' + $("#selectPrefix").UifSelect("getSelectedText").substring(0, 3) + '-' + paymentRequest.Number);
        $('#divLabelInformationCancellation').show();
    }

    static SetChargeRequestInformation(chargeRequest) {
        ClaimsPaymentRequestDTO = chargeRequest;

        RequestCancellationRequest.GetPaymentRequestClaimsByPaymentRequestId(chargeRequest.Id).done(function (response) {
            if (response.success) {
                $("#tblChargeRequestClaims").UifDataTable({ sourceData: response.result[0].Claims, enable: false });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        RequestCancellation.GetPaymentSource(chargeRequest.PaymentSourceId, chargeRequest.MovementTypeId);
        RequestCancellation.GetCollectPersonTypes(chargeRequest.PersonTypeId);
        RequestCancellation.GetRecoveryBranches(chargeRequest.BranchId);
        $("#DocumentNumberBeneficiary").val(chargeRequest.BeneficiaryDocumentNumber);
        $("#FullNameBeneficiary").val(chargeRequest.BeneficiaryFullName);
        $("#inputRecoveryRegistrationDate").val(FormatDate(chargeRequest.RegistrationDate));
        $("#inputRecoveryAccountingDate").val(FormatDate(chargeRequest.AccountingDate));
        $("#ChargeRequestInformation").show();
        $('#inputInformationCancelation').text($("#selectBranch").UifSelect("getSelectedText").substring(0, 3) + '-' + $("#selectPrefix").UifSelect("getSelectedText").substring(0, 3) + '-' + chargeRequest.Number);
        $('#divLabelInformationCancellation').show();
    }

    static SaveRequestCancellation() {
        lockScreen();
        var IsChargeRequest = false;

        if (!ClaimsPaymentRequestDTO)
            $.UifNotify('show', { 'type': 'info', 'message': 'Debe consultar una solicitud', 'autoclose': true });

        if (ClaimsPaymentRequestDTO.PaymentRequestTypeId == 2)
            IsChargeRequest = true;

        RequestCancellationRequest.SaveRequestCancellation(ClaimsPaymentRequestDTO.Id, IsChargeRequest).done(function (response) {
            if (response.success) {
                $.UifDialog('alert', {
                    message: String.format(Resources.Language.NumberCancellation + ' ' + response.result.Number + ' ' + Resources.Language.CreateSuccess)
                }, function (result) {
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
            RequestCancellation.ClearCancellationRequestInformation();
        }).always(function () {
            unlockScreen();
        });
    }

    static ClearCancellationRequestInformation() {
        $('#selectPrefix').UifSelect('disabled', false);
        $('#selectBranch').UifSelect('disabled', false);
        $('#selectBranch').UifSelect('setSelected', null);
        $('#selectPrefix').UifSelect('setSelected', null);
        $('#inputRequestNumber').val('');
        $('#inputRequestNumber').prop('disabled', false);
        $("#paymentRequestInformation").hide();
        $("#ChargeRequestInformation").hide();
        $('#inputInformationCancelation').text('');
        $('#divLabelInformationCancellation').hide();
        ClaimsPaymentRequestDTO = null;
    }
}