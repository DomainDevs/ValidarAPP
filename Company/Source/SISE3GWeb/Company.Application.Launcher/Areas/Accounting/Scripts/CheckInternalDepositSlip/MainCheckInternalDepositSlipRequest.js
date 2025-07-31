class MainCheckInternalDepositSlipRequest {
    static GeBranchesforUser() {
        return $.ajax({
            type: 'GET',
            url: ACC_ROOT + 'Common/GetListBranchesbyUserName',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveInternalBallotDeposit(checkInternal) {
        return $.ajax({
            type: 'POST',
            url: ACC_ROOT + 'CheckInternalDepositSlip/SaveInternalBallotDeposit',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ 'tblChecksModel': checkInternal })
        });
    }

    static ValidateCashAmount(branchId, currencyId, cashAmount, date, paymentTicketId) {
        return $.ajax({
            type: 'GET',
            url: ACC_ROOT + 'CheckInternalDepositSlip/ValidateCashAmount',
            data: {
                'branchId': branchId, 'currencyId': currencyId, 'cashAmountAdmitted': cashAmount,
                'registerDate': date, 'paymentTicketId': paymentTicketId
            }
        });
    }
}