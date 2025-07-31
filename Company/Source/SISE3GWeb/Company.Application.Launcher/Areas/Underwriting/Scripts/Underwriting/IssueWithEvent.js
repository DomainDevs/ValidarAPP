//Codigo de la pagina IssueWithEvent.cshtml
class IssueWithEventRequest {
    static GetIssueWithPolicies(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/Underwriting/GetIssueWithPolicies",
            data: { temporalId: temporalId }
        });
    }

    static CreatePolicyWihtoutPolicies(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/Underwriting/CreatePolicyWihtoutPolicies",
            data: { temporalId: temporalId }
        });
    }
}

class IssueWithEvent extends Uif2.Page {
    getInitialState() {
        $("#TablePolicies").UifDataTable();

        this.GetIssueWithPolicies();
    }

    bindEvents() {
        $("#btnIssuePolicy").click(this.IssuePolicy.bind(this));
        $("#btnExit").on("click", this.Exit.bind(this));
        $("#txtSearch").on("search", this.Search.bind(this));
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    IssuePolicy() {
        let policiesToIssue = $("#TablePolicies").UifDataTable("getSelected");

        if (policiesToIssue != null && policiesToIssue.length === 1) {

            IssueWithEventRequest.CreatePolicyWihtoutPolicies(policiesToIssue[0].TemporalId)
                .done(data => {
                    if (data.success) {
                        let message = data.result;
                        $.UifDialog("alert", { "message": message });
                        this.GetIssueWithPolicies();
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                }).fail(() => {
                    $.UifNotify("show",
                        { 'type': "danger", 'message': AppResources.ErrorRecordTemporary, 'autoclose': true });
                });
        }
    }

    /**
     * Obtine el listado de las polizas que fueron autorizadas
     * y renderiza en la tabla 
     * @param {number} temporalId
     * Numero del temporal
     */
    GetIssueWithPolicies(temporalId) {
        IssueWithEventRequest.GetIssueWithPolicies(temporalId)
            .done(data => {
                if (data.success) {
                    $("#TablePolicies").UifDataTable("clear");
                    data.result.forEach(item => {
                        item.DateRequest = FormatDate(item.DateRequest);
                        $("#TablePolicies").UifDataTable("addRow", item);
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
    }

    Search(e, value) {
        $("#txtSearch").val("");
        this.GetIssueWithPolicies(value);
    }
}