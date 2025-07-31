
class QuotationSearch extends Uif2.Page {

    getInitialState() {
        QuotationSearchRequest.GetPrefixes(null).done(function (data) {
            if (data.success) {
                $('#selectPrefixCommercial').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        //Validacion de campos en el formulario de busqueda
        $("#inputInsured").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputUser").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputQuotationNumber").ValidatorKey(ValidatorType.Number, 3, 0);
        $("#inputVersion").ValidatorKey(ValidatorType.Number, 3, 0);
        $("#inputPlate").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputEngine").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputChassis").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputAgentPrincipal").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    }
    //Seccion Eventos
    bindEvents() {
        $("#btnSearchQuotation").click(this.SearchQuotations);
        $("#inputAgentPrincipal").on('buttonClick', this.SearchAgentPrincipal);
        $("#inputAgentPrincipal").on('change', this.ValidateAgentAgencyQuotation);
        $("#inputInsured").on('buttonClick', this.SearchInsured);
        $("#inputUser").on('buttonClick', this.SearchUser);
    }

    ///Buscar cotizaciones
    SearchQuotations() {
        //REQ_#992: Se modifica para cumplir criterio de validación     14/11/2018
        if ((!$("#inputInsured").val() || !$("#inputInsuredId").val()) && !$("#inputQuotationNumber").val()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelQuotationSearchValidate, 'autoclose': true });
        }
        else {
            $("#formSearchQuotation").validate();
            if ($("#formSearchQuotation").valid()) {
                if (!$("#inputInsured").val() || !$("#inputInsuredId").val())
                {
                    $('#inputInsured, #inputInsuredId').val("");
                }
                var subscriptionSearchViewModel =
                    {
                        Insured: $("#inputInsured").val(),
                        InsuredId: $("#inputInsured").val() == "" ? null : $("#inputInsuredId").val(),
                        AgentPrincipal: $("#inputAgentPrincipal").val(),
                        AgentPrincipalId: $("#inputAgentPrincipalId").val(),
                        AgentAgency: $("#selectAgentAgency").UifSelect("getSelected"),
                        PrefixId: $("#selectPrefixCommercial").UifSelect("getSelected"),
                        QuotationNumber: $("#inputQuotationNumber").val(),
                        Version: $("#inputVersion").val(),
                        Plate: $("#inputPlate").val(),
                        Engine: $("#inputEngine").val(),
                        Chassis: $("#inputChassis").val(),
                        UserId: $("#inputUser") == "" ? null : $("#inputUserId").val(),
                        IssueDate: $("#inputIssueDate").val()
                    };

                QuotationSearchRequest.SearchQuotations(subscriptionSearchViewModel).done(function (data) {
                    if (data.success) {
                        data.result = data.result.map(function (data) {
                            data.Date = FormatDate(data.Date);
                            return data;
                        });
                        $('#tableQuotation').UifDataTable({ sourceData: data.result });
                        if (data.result.length == 0) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorServiceNull, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

    ///Buscar asegurado
    SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            SubscriptionSearch.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val(), InsuredSearchType.DocumentNumber, null, $("#inputInsured"), $("#inputInsuredId"));
        }
    }

    ///Buscar usuario
    SearchUser() {
        if ($("#inputUser").val().trim().length > 0) {
            SubscriptionSearch.GetUsersByDescription($("#inputUser"), $("#inputUserId"));
        }
    }

    SearchAgentPrincipal() {
        agentSearchType = 1;
        SubscriptionSearch.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim(), $("#inputAgentPrincipal"), $("#inputAgentPrincipalId"), $("#selectAgentAgency"));
    }

    ValidateAgentAgencyQuotation() {
        if ($("#inputAgentPrincipal").val() == null || $("#inputAgentPrincipal").val() == 0 || $('#inputAgentPrincipal').val() == "") {
            var data;
            $('#selectAgentAgency').UifSelect({ sourceData: data = null });
        }   
    }
}