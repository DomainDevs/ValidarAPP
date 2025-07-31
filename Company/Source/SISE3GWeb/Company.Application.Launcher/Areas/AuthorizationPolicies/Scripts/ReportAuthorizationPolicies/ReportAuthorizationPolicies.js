class AjaxReportPolicies {
    // Datos carga select de estado de politicas
    static GetStatus() {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/ReportAuthorizationPolicies/GetAllStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    // Carga datos a consultar
    static GetPoliciesReport(form) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/ReportAuthorizationPolicies/GetPolicies",
            data: JSON.stringify({ parampolicies: form, status: form.status }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    /**
     * @summary 
     *   Solicitud ajax para la consulta de las politicas
     * @param {number} status estado de la politica
     * @param {Date} dateInit fecha Inicial
     * @param {Date} dateEnd fecha final
     * @return {Object<AuthorizationRequestGroups>} respuesta de la peticion
     */
    static GetAuthorizationRequestGroups(status, dateInit, dateEnd) {
        return $.ajax({
            type: "POST",
            data: { "Status": status, "strDateInit": dateInit, "strDateEnd": dateEnd },
            url: rootPath + "AuthorizationPolicies/ReportAuthorizationPolicies/GetAuthorizationRequestGroups"
        });
    }

    static GetDetailsAuthorizationRequestGroups(key, policiesId) {
        return $.ajax({
            type: "POST",
            data: { "key": key, "policiesId": policiesId },
            url: rootPath + "AuthorizationPolicies/ReportAuthorizationPolicies/GetDetailsAuthorizationRequestGroups"
        });
    }

    static GetAuthorizationAnswersByRequestId(requestId) {
        return $.ajax({
            type: "POST",
            data: { "requestId": requestId },
            url: rootPath + "AuthorizationPolicies/ReportAuthorizationPolicies/GetAuthorizationAnswersByRequestId"
        });
    }

    //REQ_#491
    //reas: Funcionalidad Exportar Excel
    //author: Diego A. Leon
    //date: 30/08/2018
    //Exportar Excel
    static GetExcelPoliciesReport(form) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/ReportAuthorizationPolicies/ExportFileReportAuthorizationPolicies",
            data: JSON.stringify({ parampolicies: form }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
}

class ReportAuthorizationPolicies extends Uif2.Page {

    getInitialState() {
        this.LoadStatus();

        $("#dateIniPolicy").UifDatepicker();
        $("#dateEndPolicy").UifDatepicker();
        $("#tableRequestsDetails").UifDataTable({ widthColumns: [{ width: "5%", column: 1 }], hiddenColumns: [0] });
        $("#btnBack").hide();
        $("#btnExitPolicyReport").on("click", () => window.location = rootPath + "Home/Index");
    }

    bindEvents() {
        $("#SearchReportPolicies").on("click", this.SearchReportPolicies.bind(this));
        $("#dateIniPolicy").on("datepicker.change", (e, dateStart) => {
            const dateEnd = $("#dateEndPolicy").UifDatepicker("getValue");
            if (dateEnd < dateStart) {
                $("#dateEndPolicy").UifDatepicker("setValue", dateStart);
            }
        });
        $("#dateEndPolicy").on("datepicker.change", (e, dateEnd) => {
            const dateStart = $("#dateIniPolicy").UifDatepicker("getValue");
            if (dateEnd < dateStart) {
                $("#dateIniPolicy").UifDatepicker("setValue", dateEnd);
            }
        });
        $("#btnSearchDetails").on("click", this.SearchDetails.bind(this));
        $("#modalRequestDitails").on("modal.opened", () => {
            $("#tableRequestsDetails").UifDataTable({ widthColumns: [{ width: "5%", column: 1 }], hiddenColumns: [0] });
        });
        $("#btnBack").on("click", this.BackModal);
        $("#btnSearchDetailsAnswer").on("click", this.SearchDetailsAnswer);
    }

    BackModal() {
        $("#btnBack").hide();
        $("#btnSearchDetailsAnswer").show();
        $("#divTableRequestsDetails").show();
        $("#divTableAnswers").hide();
    }

    SearchDetailsAnswer() {
        let selected = $("#tableRequestsDetails").UifDataTable("getSelected");

        if (selected !== null) {
            selected = selected[0];
            $("#btnBack").show();
            $("#btnSearchDetailsAnswer").hide();
            $("#divTableRequestsDetails").hide();
            $("#divTableAnswers").show();

            AjaxReportPolicies.GetAuthorizationAnswersByRequestId(selected.AuthorizationRequestId).done(data => {
                if (data.success) {
                    $("#tableAnswers").UifDataTable("clear");
                    data.result.forEach((item) => {
                        item.DateAnswer = FormatDate(item.DateAnswer);
                        $("#tableAnswers").UifDataTable("addRow", item);
                    });
                } else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.SelectItemList, 'autoclose': true });
        }
    }

    SearchDetails() {
        let selected = $("#TableRequests").UifDataTable("getSelected");
        if (selected !== null) {
            selected = selected[0];
            this.BackModal();
            AjaxReportPolicies.GetDetailsAuthorizationRequestGroups(selected.Key, selected.PoliciesId).done((data) => {
                if (data.success) {
                    $("#tableRequestsDetails").UifDataTable("clear");
                    data.result.forEach((item) => {
                        $("#tableRequestsDetails").UifDataTable("addRow", item);
                    });
                    $("#modalRequestDitails").UifModal("showLocal", `${selected.DescriptionPolicie} - ${selected.Reference}`);
                }
                else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.SelectItemList, 'autoclose': true });
        }
    }

    SearchReportPolicies() {
        const dataForm = this.GetForm();
        if (dataForm !== false) {
            AjaxReportPolicies.GetAuthorizationRequestGroups(dataForm.status, dataForm.dateStart, dataForm.dateEnd).done((data) => {
                if (data.success) {
                    $("#TableRequests").UifDataTable("clear");
                    data.result.forEach((item) => {
                        item.DateRequest = FormatDate(item.DateRequest);
                        $("#TableRequests").UifDataTable("addRow", item);
                    });
                }
                else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            });
        }
    }

    GetForm() {
        const data = {};
        data.dateStart = $("#dateIniPolicy").val();
        data.dateEnd = $("#dateEndPolicy").val();
        data.status = $("#IdStatus").UifMultiSelect("getSelected");

        if (data.dateStart === "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorInitialSelectionDate, 'autoclose': true });
            return false;
        }
        if (data.dateEnd === "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorFinalSelectionDate, 'autoclose': true });
            return false;
        }
        if (data.status === null) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorStatus, 'autoclose': true });
            return false;
        }

        return data;
    }

    // Funcion llena combox Status 
    LoadStatus() {
        AjaxReportPolicies.GetStatus().done(function (data) {
            if (data) {
                $("#IdStatus").UifMultiSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify("show", { 'type': "info", 'message': data.result, 'autoclose': true });
            }
        });
    }

    //REQ_#491
    //reas: Funcionalidad botón exportar
    //author: Diego A. Leon
    //date: 30/08/2018
    static ExportExcel() {
        const FormReportPolicies = ReportAuthorizationPolicies.GetForm();
        //REQ_#491
        //reas: Validación formulario al no tener Modelo
        //author: Diego A. Leon
        //date: 04/09/2018
        if (FormReportPolicies.status != null) {
            if (FormReportPolicies.dateIni != "") {
                if (FormReportPolicies.dateFin != "") {
                    if (FormReportPolicies.IdBranch != "") {
                        if (FormReportPolicies.IdPrefix != "") {
                            //Validar Fechas 
                            if (ReportAuthorizationPolicies.ValidateDate($("#dateIniPolicy").val(), $("#hiddenactualdate").val(), $("#dateEndPolicy").val())) {
                                if ($("#formReportPolicies").valid()) {
                                    AjaxReportPolicies.GetExcelPoliciesReport(FormReportPolicies).done(function (data) {
                                        if (data.success) {
                                            DownloadFile(data.result);
                                        }
                                        else {
                                            $.UifNotify("show", {
                                                'type': "danger", 'message': data.result, 'autoclose': true
                                            });
                                        }
                                    });
                                }
                            }
                        }
                        else {
                            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorPrefix, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorBranch, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorFinalSelectionDate, 'autoclose': true });
                }
            }
            else {
                $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorInitialSelectionDate, 'autoclose': true });
            }
        }
        else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorStatus, 'autoclose': true });
        }
    }

    //REQ_#491
    //reas: Funcionalidad adicional botón exportar
    //author: Diego A. Leon
    //date: 30/08/2018
    static ExportFileSuccess(data) {
        DownloadFile(data);
    }
}