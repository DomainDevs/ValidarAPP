var individualId;
var coverageReinsuranceCumulusDTOsByIndividual;
var coverageReinsuranceCumulusDTOsByConsortium;
var coverageReinsuranceCumulusDTOsByEconomicGroup;

class QueryCumulus extends Uif2.Page {

    getInitialState() {
        $("#divEconomicGroup").hide();
        QueryCumulus.getLineBusiness();
        QueryCumulus.getPrefixes();
    }

    bindEvents() {
        $("#chkByLineBusiness").on("click", QueryCumulus.checkByLineBusiness);
        $("#chkByPrefix").on("click", QueryCumulus.checkByPrefix);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#chkIncludeFuture").on("change", QueryCumulus.checkIncludeFuture);
        $("#chkEconomicGroup").on("change", QueryCumulus.checkEconomicGroup);
        $("#selectLineBusiness").on("itemSelected", QueryCumulus.getSubLineBusiness);
        $("#selectLineBusiness").on("itemSelected", QueryCumulus.getSubLineBusiness);
        $("#insuredFullName").on('search', QueryCumulus.SetDocumentNumber);
        $('#btnQueryCumulus').on('click', QueryCumulus.GetCumulusByIndividual);
        $('#tblIndividualDetail').on('rowSelected', QueryCumulus.SelectedRowModalIndividualDetail);
        $('#btnExit').on('click', QueryCumulus.Exit);
        $('#btnDetailExit').on('click', QueryCumulus.Exit);
        $('#btnExport').on('click', QueryCumulus.Export);
        $('#tableDetailCumulus').on('rowSelected', QueryCumulus.SelectedTable);
        $("#btnClear").on("click", QueryCumulus.clearInsured);
        $("#btnClear2").on("click", QueryCumulus.clearInsured);
        $("#tab1").on("click", QueryCumulus.enableControls);
        $("#tab2").on("click", function () {
            QueryCumulus.disableControls();
            QueryCumulus.GetCumulusDetailByIndividual();
        });
    }

    static checkByLineBusiness() {
        $("#selectPrefix").UifSelect("setSelected", null);
        $("#chkByPrefix").prop("checked", false);
        $("#chkByLineBusiness").prop("checked", true);
        $("#DivPrefix").hide();
        $("#DivLineBusiness").show();

    }

    static checkByPrefix() {
        $("#selectLineBusiness").UifSelect("setSelected", null);
        $("#selectSubLineBusiness").UifSelect("setSelected", null);
        $("#chkByLineBusiness").prop("checked", false);
        $("#chkByPrefix").prop("checked", true);
        $("#DivLineBusiness").hide();
        $("#DivPrefix").show();
    }

    static checkIncludeFuture() {
        if (!($("#chkIncludeFuture").prop("checked"))) {
            $("#chkIncludeFuture").prop("checked", false);
        }
    }

    static clearInsured() {
        $("#insuredFullName").val("");
        $("#insuredDocumentNumber").val("");
        QueryCumulus.clearForms();
    }

    static clearForms() {
        $("#selectLineBusiness").UifSelect("setSelected", null);
        $("#selectSubLineBusiness").UifSelect("setSelected", null);
        $("#cumulusDate").UifDatepicker('setValue', new Date());
        $("#chkIncludeFuture").prop("checked", true);
        $("#tableAcumulateTypeContract").UifDataTable('clear');
        $("#tableDetailCumulus").UifDataTable('clear');
        $("#tablePolicy").UifDataTable('clear');
        $("#tableEconomicGroup").UifDataTable('clear');
        $("#chkEconomicGroup").prop("checked", false);
        $("#AssignmentTotal").val("");
        $("#RetentionTotal").val("");
        $("#TotalCumuloPesos").val("");
    }

    static clearCumulusDetail() {
        $("#tableDetailCumulus").UifDataTable('clear');
        $("#tablePolicy").UifDataTable('clear');
        $("#tableEconomicGroup").UifDataTable('clear');
        $("#chkEconomicGroup").prop("checked", false);
    }

    static checkEconomicGroup() {
        if (!($("#chkEconomicGroup").prop("checked"))) {
            $("#chkEconomicGroup").prop("checked", false);
            $("#divEconomicGroup").hide();
        }
        else {
            if (coverageReinsuranceCumulusDTOsByEconomicGroup.length > 0) {
                $("#formQueryCumulus").validate();
                if ($("#formQueryCumulus").valid()) {
                    lockScreen();
                    var lineBusiness = $("#selectLineBusiness").UifSelect("getSelected") ? $("#selectLineBusiness").UifSelect("getSelected") : 0;
                    var cumulusDate = $("#cumulusDate").val();
                    var isFuture = $("#chkIncludeFuture").prop("checked");
                    var subLineBusiness = $("#selectSubLineBusiness").UifSelect("getSelected");
                    var prefixCd = $("#selectPrefix").UifSelect("getSelected") ? $("#selectPrefix").UifSelect("getSelected") : 0;
                    if (subLineBusiness == null || subLineBusiness == "") {
                        subLineBusiness = 0;
                    }
                    var economicGroupId = coverageReinsuranceCumulusDTOsByEconomicGroup[0].EconomicGroup.EconomicGroupId;
                    QueryCumulusRequest.GetDetailCumulusParticipantsEconomicGroup(economicGroupId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd).done(function (data) {
                        if (data.success) {
                            if (data.result.length > 0) {
                                $("#tableEconomicGroup").UifDataTable({ sourceData: data.result });
                                $("#divEconomicGroup").show();
                                $("#tableEconomicGroup").show();
                            }
                        }
                    }).always(function () {
                        unlockScreen();
                    });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.QueryNotData, 'autoclose': true });
            }
        }
    }

    static getLineBusiness() {
        QueryCumulusRequest.GetLineBusiness().done(function (data) {
            if (data.success) {
                $('#selectLineBusiness').UifSelect({ sourceData: data.result });
                $("#tableEconomicGroup").hide();
                $("#DivPrefix").hide();
                $("#chkByLineBusiness").prop("checked", true);
            }
        });
    }
    static getPrefixes() {
        QueryCumulusRequest.getPrefixes().done(function (result) {
            if (result.data.length > 0) {
                $('#selectPrefix').UifSelect({ sourceData: result.data });
            }
        });
    }

    static getSubLineBusiness() {
        if ($('#selectLineBusiness').val() == "") {
            $("#selectSubLineBusiness").UifSelect("setSelected", "");
            $("#selectSubLineBusiness").prop("disabled", true);
        }

        QueryCumulusRequest.GetSubLineBusiness($('#selectLineBusiness').val()).done(function (data) {
            if (data.success) {
                $("#selectSubLineBusiness").prop("disabled", false);
                $("#selectSubLineBusiness").UifSelect({ sourceData: data.result });
                $("#selectSubLineBusiness").UifSelect("setSelected", data.result[0].Id);
            }
        });
    }

    static SetDocumentNumber(event, selectedItem) {
        QueryCumulus.clearForms();
        var query = $("#insuredFullName").val();
        if (query.length >= 3) {
            QueryCumulusRequest.GetInsuredsByDescription(query).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        if (data.result.length == 1) {
                            QueryCumulus.SelectedRowModalIndividualDetail(null, data.result[0]);
                        }
                        else {
                            var dataStates = [];
                            $.each(data.result, function (index, value) {
                                dataStates.push({
                                    IndividualId: value.IndividualId,
                                    FullName: value.FullName,
                                    DocumentNumber: value.DocumentNumber
                                });
                            });
                            $('#tblIndividualDetail').UifDataTable('clear');
                            $("#tblIndividualDetail").UifDataTable('addRow', dataStates);
                            $('#modalIndividualDetail').UifModal('showLocal', Resources.Language.InsuredConsolidated);
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.InsuredNotExists, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static SelectedRowModalIndividualDetail(event, data, position) {
        $('#insuredDocumentNumber').val(data.DocumentNumber);
        $('#insuredFullName').val(data.FullName);
        individualId = data.IndividualId;
        $('#modalIndividualDetail').UifModal('hide');
    }

    static GetCumulusByIndividual() {
        $("#formQueryCumulus").validate();
        if ($("#formQueryCumulus").valid()) {
            lockScreen();
            var lineBusiness = $("#selectLineBusiness").UifSelect("getSelected") ? $("#selectLineBusiness").UifSelect("getSelected") : 0;
            var cumulusDate = $("#cumulusDate").val();
            var isFuture = $("#chkIncludeFuture").prop("checked");
            var subLineBusiness = $("#selectSubLineBusiness").UifSelect("getSelected");
            var prefixCd = $("#selectPrefix").UifSelect("getSelected") ? $("#selectPrefix").UifSelect("getSelected") : 0;
            if (subLineBusiness == null || subLineBusiness == "") {
                subLineBusiness = 0;
            }

            QueryCumulusRequest.GetCumulusByIndividual(individualId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd).done(function (data) {
                if (data.success) {
                    if (data.result.ContractReinsuranceCumulusDTOs.length > 0) {
                        $("#tableAcumulateTypeContract").UifDataTable('clear');
                        if (data.result.TotalCumulus > 0) {
                            $('#tab2').show();
                        }
                        else {
                            $('#tab2').hide();
                        }
                        $.each(data.result.ContractReinsuranceCumulusDTOs, function (index, value) {
                            var id = value.Contract.Currency.Id;
                            if (id > 0) {
                                var currentTime = new Date(parseInt(data.result.ContractReinsuranceCumulusDTOs[index].Contract.EstimatedDate.substr(6)));
                                var day = currentTime.getDate();
                                var month = currentTime.getMonth() + 1;
                                if (day < 10) { day = '0' + day; }
                                if (month < 10) { month = '0' + month; }
                                var date = day + "/" + month + "/" + currentTime.getFullYear();
                                if (date != $("#cumulusDate").val()) {
                                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ChangeRateDoesntExistDolar + " " + Resources.Language.ChangeRateDoesntExistEuro, 'autoclose': true });
                                }
                            }
                        });
                        $("#tableAcumulateTypeContract").UifDataTable({ sourceData: data.result.ContractReinsuranceCumulusDTOs });
                        $("#TotalCumuloPesos").val(FormatMoney(data.result.TotalCumulus));
                        $("#AssignmentTotal").val(FormatMoney(data.result.AssignmentTotalCumulus));
                        $("#RetentionTotal").val(FormatMoney(data.result.RetentionTotalCumulus));
                        $("#AssignmentTotal").prop("disabled", true);
                        $("#RetentionTotal").prop("disabled", true);
                    }
                    else {
                        $("#tableAcumulateTypeContract").UifDataTable('clear');
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true });
                    }
                }
            }).always(function () {
                unlockScreen();
            });
        }
    }

    static GetCumulusDetailByIndividual() {
        QueryCumulus.clearCumulusDetail();
        $("#formQueryCumulus").validate();
        if ($("#formQueryCumulus").valid()) {
            lockScreen();
            var lineBusiness = $("#selectLineBusiness").UifSelect("getSelected") ? $("#selectLineBusiness").UifSelect("getSelected") : 0;
            var cumulusDate = $("#cumulusDate").val();
            var isFuture = $("#chkIncludeFuture").prop("checked");
            var subLineBusiness = $("#selectSubLineBusiness").UifSelect("getSelected");
            var prefixCd = $("#selectPrefix").UifSelect("getSelected") ? $("#selectPrefix").UifSelect("getSelected") : 0;

            if (subLineBusiness == null || subLineBusiness == "") {
                subLineBusiness = 0;
            }

            QueryCumulusRequest.GetCumulusDetailByIndividual(individualId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd).done(function (data) {
                if (data.success) {
                    coverageReinsuranceCumulusDTOsByIndividual = data.result.CoverageReinsuranceCumulusDTOsByIndividual;
                    coverageReinsuranceCumulusDTOsByConsortium = data.result.CoverageReinsuranceCumulusDTOsByConsortium;
                    coverageReinsuranceCumulusDTOsByEconomicGroup = data.result.CoverageReinsuranceCumulusDTOsByEconomicGroup;

                    $("#tableDetailCumulus").UifDataTable({ sourceData: data.result.ReinsuranceCumulusDetailDTOs, order: [1, 'asc'] });
                    var value = { label: 'Id', value: 0 }
                    $("#tableDetailCumulus").UifDataTable('setSelect', value);
                    $("#tablePolicy").UifDataTable({ sourceData: coverageReinsuranceCumulusDTOsByIndividual, hiddenColumns: [1] });
                }
            }).always(function () {
                unlockScreen();
            });
        }
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static Export() {
        if ($("#tablePolicy").UifDataTable('getData').length > 0) {
            var coverageReinsuranceCumulusDTOs = QueryCumulus.getCoverageReinsuranceCumulusDTOs($("#tablePolicy").UifDataTable('getData'));
            const filename = `${$("#insuredFullName").val()}_${$("#insuredDocumentNumber").val()}`;
            QueryCumulusRequest.GenerateFileCumulusByIndividual(filename, coverageReinsuranceCumulusDTOs).done(function (data) {
                if (data.success) {
                    window.open(data.result);
                }
                else {
                    $.UifNotify("show", { 'type': "info", 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorThereIsNoDataToExport, 'autoclose': true });
        }
    }

    static getCoverageReinsuranceCumulusDTOs(coverageReinsuranceCumulus) {
        var coverageReinsuranceCumulusDTOs = [];
        $.each(coverageReinsuranceCumulus, function (index, value) {
            value.CoverageCurrentFrom = FormatDate(value.CoverageCurrentFrom);
            value.CoverageCurrentTo = FormatDate(value.CoverageCurrentTo);
            coverageReinsuranceCumulusDTOs.push(value);
        });
        return coverageReinsuranceCumulusDTOs;
    }

    static SelectedTable(event, data, position) {
        $("#tablePolicy").UifDataTable('clear')
        switch (data.Id) {
            case 0:
                $("#tablePolicy").UifDataTable({ sourceData: coverageReinsuranceCumulusDTOsByIndividual, hiddenColumns: [1] });
                break;
            case 1:
                $("#tablePolicy").UifDataTable({ sourceData: coverageReinsuranceCumulusDTOsByConsortium });
                break;
            case 2:
                $("#tablePolicy").UifDataTable({ sourceData: coverageReinsuranceCumulusDTOsByEconomicGroup, hiddenColumns: [1] });
                break;
        }
    }

    static enableControls() {
        $("#insuredFullName").UifAutoComplete("disabled", false);
        $("#cumulusDate").UifDatepicker('disabled', false);
        $("#chkIncludeFuture").attr("disabled", false);
        $("#selectLineBusiness").UifSelect("disabled", false);
        $("#selectSubLineBusiness").UifSelect("disabled", false);
        $('#btnQueryCumulus').attr("disabled", false);
        $("#selectPrefix").UifSelect("disabled", false);
    }

    static disableControls() {
        $("#insuredFullName").UifAutoComplete("disabled", true);
        $("#cumulusDate").UifDatepicker('disabled', true);
        $("#chkIncludeFuture").attr("disabled", true);
        $("#selectLineBusiness").UifSelect("disabled", true);
        $("#selectPrefix").UifSelect("disabled", true);
        $("#selectSubLineBusiness").UifSelect("disabled", true);
        $('#btnQueryCumulus').attr("disabled", true);
    }

}

$(document).ready(function () {
    new QueryCumulus();
    new QueryCumulusRequest();
});