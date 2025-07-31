
class AutomaticSalaryUpdate extends Uif2.Page {

    getInitialState() {

        AutomaticSalaryUpdateRequest.GetBranches().done(function (response) {

            if (response.result) {
                $("#selectBranchClaim").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        AutomaticSalaryUpdateRequest.GetPrefixes().done(function (response) {

            if (response.result) {
                $("#selectPrefixClaim").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        AutomaticSalaryUpdateRequest.GetMinimumSalaryByYear(moment().format("YYYY")).done(function (data) {
            if (data.success) {
                $("#minimunSalaryMounth").val(FormatMoney(data.result.SalaryMinimumMounth));
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        $("#searchClaimTable").hide();
        $("#buttonsRescue").hide();
    }

    bindEvents() {
        $("#btnSearch").on('click', AutomaticSalaryUpdate.SearchClaim);
        $("#btnClean").on('click', AutomaticSalaryUpdate.CleanSearch);
        $("#tblClaims").on('rowSelected', AutomaticSalaryUpdate.LoadButtons);
        $("#tblClaims").on('selectAll', AutomaticSalaryUpdate.LoadButtons);
        $("#btnUpdateSalaries").on('click', AutomaticSalaryUpdate.UpdateEstimationsSalaries);
    }

    static SearchClaim() {
        $("#buttonsRescue").hide();

        if ((($("#inputDateOcurrenceFromClaim").val() != "") && ($("#inputDateOcurrenceToClaim").val() == "")) ||
            (($("#inputDateOcurrenceFromClaim").val() == "") && ($("#inputDateOcurrenceToClaim").val() != ""))) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfOcurrenceDates, 'autoclose': true });
        }
        else if ((($("#inputDateNoticeFromClaim").val() != "") && ($("#inputDateNoticeToClaim").val() == "")) ||
            (($("#inputDateNoticeFromClaim").val() == "") && ($("#inputDateNoticeToClaim").val() != ""))) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfNoticeDates, 'autoclose': true });
        }
        else {
            var validateDate = true;

            //Valida que la Fecha de Ocurrencia desde no sea mayor que Fecha de Ocurrencia Hasta
            if (($("#inputDateOcurrenceFromClaim").val() != "") && ($("#inputDateOcurrenceToClaim").val() != "")) {
                if (CompareClaimDates($("#inputDateOcurrenceFromClaim").val(), $("#inputDateOcurrenceToClaim").val())) {
                    var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateOcurrenceFromClaim").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateOcurrenceToClaim").val()) + '"';
                    $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
                    validateDate = false;
                }
            }

            //Valida que la Fecha de Aviso Desde no sea mayor que Fecha de Aviso Hasta
            if (($("#inputDateNoticeFromClaim").val() != "") && ($("#inputDateNoticeToClaim").val() != "")) {
                if (CompareClaimDates($("#inputDateNoticeFromClaim").val(), $("#inputDateNoticeToClaim").val())) {
                    var msgValidNoticeDate = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateNoticeFromClaim").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateNoticeToClaim").val()) + '"';
                    $.UifNotify('show', { 'type': 'danger', 'message': msgValidNoticeDate, 'autoclose': true });
                    validateDate = false;
                }
            }

            if (validateDate) {
                AutomaticSalaryUpdate.SetParametersClaim();
            }
        }

    }

    static SetParametersClaim() {
        var searchClaimModel = {
            BranchId: null,
            ClaimDateFrom: null,
            ClaimDateTo: null,
            ClaimNumber: null,
            NoticeDateFrom: null,
            NoticeDateTo: null,
            PrefixId: null,
            TemporaryNumber: null,
            UserId: null,
            IndividualId: null,
            HolderId: null,
            DocumentNumber: null
        };

        //Fechas de Ocurrencia
        if ($("#inputDateOcurrenceFromClaim").val() != "") {
            searchClaimModel.ClaimDateFrom = $("#inputDateOcurrenceFromClaim").val();
        }
        if ($("#inputDateOcurrenceToClaim").val() != "") {
            searchClaimModel.ClaimDateTo = $("#inputDateOcurrenceToClaim").val();
        }

        //Fechas de Aviso 
        if ($("#inputDateNoticeFromClaim").val() != "") {
            searchClaimModel.NoticeDateFrom = $("#inputDateNoticeFromClaim").val();
        }
        if ($("#inputDateNoticeToClaim").val() != "") {
            searchClaimModel.NoticeDateTo = $("#inputDateNoticeToClaim").val();
        }

        //Denuncia
        if ($("#inputClaimSearchClaimNumber").val() != "") {
            searchClaimModel.ClaimNumber = $("#inputClaimSearchClaimNumber").val();
        }

        //Sucursal
        if ($("#selectBranchClaim").val() != "" || $("#selectBranchClaim").val() != 0) {
            searchClaimModel.BranchId = $("#selectBranchClaim").val();
        }

        //Ramo
        if ($("#selectPrefixClaim").val() != "" || $("#selectPrefixClaim").val() != 0) {
            searchClaimModel.PrefixId = $("#selectPrefixClaim").val();
        }

        if ($("#inputDocumentNumber").val() != "") {
            searchClaimModel.DocumentNumber = $("#inputDocumentNumber").val();
        }

        searchClaimModel.IsMinimumSalary = true;

        $("#tblClaims").UifDataTable('clear');
        AutomaticSalaryUpdate.LoadGridClaim(searchClaimModel);
    }


    static LoadGridClaim(searchClaimModel) {
        lockScreen();
        $("#searchClaimTable").hide();

        AutomaticSalaryUpdateRequest.SearchClaimsBySalaryEstimation(searchClaimModel).done(function (response) {
            if (response.success) {

                $.each(response.result, function (index, value) {
                    this.Reserve = this.EstimateAmountAccumulate - this.PaymentValue;
                });
                $("#tblClaims").UifDataTable({ sourceData: response.result });
                $("#searchClaimTable").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });
    }

    static UpdateEstimationsSalaries() {

        var estimationsSelected = $("#tblClaims").UifDataTable("getSelected");                

        if (estimationsSelected != null) {

            AutomaticSalaryUpdateRequest.UpdateEstimationsSalaries(estimationsSelected).done(function (data) {

                if (data.success) {

                    $.UifDialog('alert', {
                        message: String.format(Resources.Language.MessageUpdateEstimationSalaries, data.result.length)
                    }, function (result) {
                    });

                    AutomaticSalaryUpdate.CleanSearch();

                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectItem, 'autoclose': true });
        }
    }

    static CleanSearch() {

        $("#selectBranchClaim").UifSelect('setSelected', null);
        $("#selectPrefixClaim").UifSelect('setSelected', null);
        $("#inputDocumentNumber").val("");
        $("#inputClaimSearchClaimNumber").val("");
        $("#inputDateOcurrenceFromClaim").val("");
        $("#inputDateOcurrenceToClaim").val("");
        $("#inputDateNoticeFromClaim").val("");
        $("#inputDateNoticeToClaim").val("");

        $("#searchClaimTable").hide();

        $("#buttonsRescue").hide();
    }

    static LoadButtons() {
        if ($("#tblClaims").UifDataTable('getSelected') != null) {
            $("#buttonsRescue").show();
        }        
    }

}