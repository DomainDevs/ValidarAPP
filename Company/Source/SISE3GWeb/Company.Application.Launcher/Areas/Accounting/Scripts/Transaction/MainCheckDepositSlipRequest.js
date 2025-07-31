class MainCheckDepositSlipRequest {
    static GeBranchesforUser() {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + 'Common/GetListBranchesbyUserName',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}