
class PolicySearch extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        //Cargar Sucursales
        PolicySearchRequest.GetBranchs().done(function (data) {
            if (data.success) {
                $('#selectBranch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        //Cargar Ramos
        PolicySearchRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        //Validacion de campos en el formulario de busqueda
        $("#inputInsuredPolicy").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputUserPolicy").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputEndorsementId").ValidatorKey(ValidatorType.Number, 3, 0);
        $("#inputPlatePolicy").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputEnginePolicy").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputChassisPolicy").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputAgentPrincipalPolicy").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    }
    //Seccion Eventos
    bindEvents() {
        $("#btnSearchPolicy").click(this.SearchPolicies);
        $("#inputAgentPrincipalPolicy").on('buttonClick', this.SearchAgentPrincipal);
        $("#inputAgentPrincipalPolicy").on('change', this.ValidateAgentAgencyPolicy);
        $("#inputInsuredPolicy").on('buttonClick', this.SearchInsured);
        $("#inputUserPolicy").on('buttonClick', this.SearchUser);
        $("#inputHolderPolicy").on('buttonClick', this.SearchHolder);
    }

    ///Buscar Polizas
    SearchPolicies() {
        //Validacion para no realizar busquedas sin asegurado o poliza 
        //if ((!$("#inputInsuredPolicy").val() || !$("#inputInsuredIdPolicy").val()) && !$("#inputPolicyNumber").val() ) {
        if (!$("#inputHolderPolicy").val() && !$("#inputAgentPrincipalPolicy").val() && !$("#inputInsuredPolicy").val() && !$("#inputPolicyNumber").val() && !$("#inputUserPolicy").val() ) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelPolicySearchValidate, 'autoclose': true });
        }
        else {
            $("#formSearchPolicy").validate();
            if ($("#formSearchPolicy").valid()) {
                //Valida los input y los setea en vacio borra un temporal de valor.
                if (!$("#inputInsuredPolicy").val() || !$("#inputInsuredIdPolicy").val()) {
                    $('#inputInsuredPolicy, #inputInsuredIdPolicy').val("");
                }
                if (!$("#inputHolderPolicy").val() || !$("#inputHolderIdPolicy").val()) {
                    $('#inputHolderPolicy, #inputHolderIdPolicy').val("");
                }
                var subscriptionSearchViewModel =
                {
                        Holder: $("#inputHolderPolicy").val(),
                        HolderId: $("#inputHolderPolicy").val() == "" ? null : $("#inputHolderIdPolicy").val(),
                        Insured: $("#inputInsuredPolicy").val(),
                        InsuredId: $("#inputInsuredPolicy").val() == "" ? null : $("#inputInsuredIdPolicy").val(),
                        AgentPrincipal: $("#inputAgentPrincipalPolicy").val(),
                        AgentPrincipalId: $("#inputAgentPrincipalIdPolicy").val(),
                        AgentAgency: $("#selectAgentAgencyPolicy").UifSelect("getSelected"),
                        BranchId: $("#selectBranch").UifSelect("getSelected"),
                        PrefixId: $("#selectPrefix").UifSelect("getSelected"),
                        EndorsementId: $("#inputEndorsementId").val(),
                        PolicyNumber: $("#inputPolicyNumber").val(),
                        Plate: $("#inputPlatePolicy").val(),
                        Engine: $("#inputEnginePolicy").val(),
                        Chassis: $("#inputChassisPolicy").val(),
                        UserId: $("#inputUserPolicy").val() == "" ? null : $("#inputUserIdPolicy").val(),
                        IssueDate: $("#inputIssueDatePolicy").val()
                    };
                lockScreen();
                PolicySearchRequest.SearchPolicies(subscriptionSearchViewModel).done(function (data) {
                    if (data.success) {
                        $('#tableSearchPolicy').UifDataTable({ sourceData: data.result });
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

    ///Buscar tomador
    SearchHolder() {
        if ($("#inputHolderPolicy").val().trim().length > 0) {
            SubscriptionSearch.GetSearchHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputHolderPolicy").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual, $("#inputHolderPolicy"), $("#inputHolderIdPolicy"));
        }
    }

    ///Buscar asegurado
    SearchInsured() {
        if ($("#inputInsuredPolicy").val().trim().length > 0) {
            SubscriptionSearch.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputInsuredPolicy").val(), InsuredSearchType.DocumentNumber, null, $("#inputInsuredPolicy"), $("#inputInsuredIdPolicy"));
        }
    }

    ///Buscar usuario
    SearchUser() {
        if ($("#inputUserPolicy").val().trim().length > 0) {
            SubscriptionSearch.GetUsersByDescription($("#inputUserPolicy"), $("#inputUserIdPolicy"));
        }
    }

    SearchAgentPrincipal() {
        agentSearchType = 1;
        SubscriptionSearch.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipalPolicy").val().trim(), $("#inputAgentPrincipalPolicy"), $("#inputAgentPrincipalIdPolicy"), $("#selectAgentAgencyPolicy"));
    }

    ValidateAgentAgencyPolicy() {
        if ($("#inputAgentPrincipalPolicy").val() == null || $("#inputAgentPrincipalPolicy").val() == 0 || $('#inputAgentPrincipalPolicy').val() == "") {
            var data;
            $('#selectAgentAgencyPolicy').UifSelect({ sourceData: data = null });
        }   
    }
}