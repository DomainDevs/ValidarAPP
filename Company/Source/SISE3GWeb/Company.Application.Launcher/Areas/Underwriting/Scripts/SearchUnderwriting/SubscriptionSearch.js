//Variable para determinar busqueda: 1 -> Cotizacion, 2 -> Poliza, 3 -> Temporal
var itemSelected = 0;
class SubscriptionSearch extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#divSearchQuotation').hide();
        $('#footerQuotation').hide();
        $('#divSearchPolicy').hide();
        $('#footerPolicy').hide();
        $('#divSearchTemporal').hide();
        $('#footerTemporal').hide();
    }
    //Seccion Eventos
    bindEvents() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#selectSearchType').on('itemSelected', this.ChangeSearchType);
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividualResults);
        $('#tableResults tbody').on('click', 'tr', this.SelectResults);
        $('#tableUserResults tbody').on('click', 'tr', this.SelectUserResults);
        $("#btnExportQuotations").click(this.sendExcelQuotation);
        $("#btnExportPolicies").click(this.sendExcelPolicy);
        $("#btnNewVersion").click(this.CreateNewVersionQuotation);
    }

    ///Evento para mostrar los diferentes formularios de busqueda dependiendo la seleccion en el "select"
    ChangeSearchType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            if (selectedItem.Id == 1) {
                itemSelected = 1;
                $('#divSearchQuotation').show();
                $('#footerQuotation').show();
                $('#divSearchPolicy').hide();
                $('#footerPolicy').hide();
                $('#divSearchTemporal').hide();
                $('#footerTemporal').hide();
            }
            else if (selectedItem.Id == 2) {
                itemSelected = 2;
                $('#divSearchPolicy').show();
                $('#footerPolicy').show();
                $('#divSearchQuotation').hide();
                $('#footerQuotation').hide();
                $('#divSearchTemporal').hide();
                $('#footerTemporal').hide();
            }
            else if (selectedItem.Id == 3) {
                itemSelected = 3;
                $('#divSearchTemporal').show();
                $('#footerTemporal').show();
                $('#divSearchQuotation').hide();
                $('#footerQuotation').hide();
                $('#divSearchPolicy').hide();
                $('#footerPolicy').hide();
            }
        }
        else {
            itemSelected = 0;
            $('#divSearchQuotation').hide();
            $('#footerQuotation').hide();
            $('#divSearchPolicy').hide();
            $('#footerPolicy').hide();
            $('#divSearchTemporal').hide();
            $('#footerTemporal').hide();
        }
    }

    ///Evento al dar clic en un item del modal de busqueda.
    SelectIndividualResults(e) {
        //llenar input con nombre completo y numero de documento entre parentesis
        switch (itemSelected) {

            case 1:
                $("#inputInsured").val($(this).children()[3].innerHTML + " (" + $(this).children()[2].innerHTML + ")");
                $("#inputInsuredId").val($(this).children()[0].innerHTML);
                break;
            case 2:
                $("#inputInsuredPolicy").val($(this).children()[3].innerHTML + " (" + $(this).children()[2].innerHTML + ")");
                $("#inputInsuredIdPolicy").val($(this).children()[0].innerHTML);
                break;
            case 3:
                $("#inputInsuredTemporary").val($(this).children()[3].innerHTML + " (" + $(this).children()[2].innerHTML + ")");
                $("#inputInsuredIdTemporary").val($(this).children()[0].innerHTML);
                break;
            case 4:
                $("#inputHolderPolicy").val($(this).children()[3].innerHTML + " (" + $(this).children()[2].innerHTML + ")");
                $("#inputHolderIdPolicy").val($(this).children()[0].innerHTML);
                itemSelected = 2;
                break;
            default:
        }
        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    ///Evento al dar clic en un item del modal de busqueda (Usuario principal).
    SelectUserResults(e) {
        //llenar input con nombre completo y numero de documento entre parentesis
        switch (itemSelected) {

            case 1:
                $("#inputUser").val($(this).children()[1].innerHTML + " (" + $(this).children()[0].innerHTML + ")");
                $("#inputUserId").val($(this).children()[0].innerHTML);
                break;
            case 2:
                $("#inputUserPolicy").val($(this).children()[1].innerHTML + " (" + $(this).children()[0].innerHTML + ")");
                $("#inputUserIdPolicy").val($(this).children()[0].innerHTML);
                break;
            case 3:
                $("#inputUserTemporary").val($(this).children()[1].innerHTML + " (" + $(this).children()[0].innerHTML + ")");
                $("#inputUserIdTemporary").val($(this).children()[0].innerHTML);
                break;
            default:
        }
        $('#modalUserSearch').UifModal("hide");
    }

    ///Evento al dar clic en un item del modal de busqueda (Intermediario principal).
    SelectResults(e) {
        //llenar input con nombre completo y numero de documento entre parentesis
        switch (itemSelected) {

            case 1:
                $("#inputAgentPrincipal").val($(this).children()[2].innerHTML + " (" + $(this).children()[1].innerHTML + ")");
                $("#inputAgentPrincipalId").val($(this).children()[0].innerHTML);
                SubscriptionSearch.GetAgenciesByAgentId($(this).children()[0].innerHTML, $(this).children()[3].innerHTML, $("#selectAgentAgency"));
                break;
            case 2:
                $("#inputAgentPrincipalPolicy").val($(this).children()[2].innerHTML + " (" + $(this).children()[1].innerHTML + ")");
                $("#inputAgentPrincipalIdPolicy").val($(this).children()[0].innerHTML);
                SubscriptionSearch.GetAgenciesByAgentId($(this).children()[0].innerHTML, $(this).children()[3].innerHTML, $("#selectAgentAgencyPolicy"));
                break;
            case 3:

                break;
            default:
        }
        $('#modalDefaultSearch').UifModal("hide");
    }

    static GetUsersByDescription(input, hidden) {
        var number = parseInt(input.val(), 10);
        var dataList = [];
        if ((!isNaN(number) || input.val().length > 2) && (input.val() != 0)) {
            SubscriptionSearchRequest.GetUsersByDescription(input.val()).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0 && individualSearchType == 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchUsers, 'autoclose': true });
                    }
                    else if (data.result.length == 1) {
                        input.data("Object", data.result[0]);
                        input.val(data.result[0].AccountName + ' (' + data.result[0].UserId + ')');
                        hidden.val(data.result[0].UserId);
                    }
                    else if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                UserId: data.result[i].UserId,
                                Name: data.result[i].AccountName
                            });
                        }

                        $('#tableUserResults').UifDataTable('clear');
                        $('#tableUserResults').UifDataTable('addRow', dataList);

                        $('#modalUserSearch').UifModal('showLocal', AppResources.SelectUser);

                    }
                }
                else {
                    input.data("Object", null);
                    input.data("Detail", null);
                    input.val('');
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchUser, 'autoclose': true });
            });
        }
    }

    static GetSearchHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, input, hidden) {
        var number = parseInt(description, 10);
        //$("#SearchindividualId").val('');
        //$("#SearchCodeId").val('');
        var dataList = [];
        if (description != undefined) {
            if ((!isNaN(number) || description.length > 2) && (description != 0)) {
                SubscriptionSearchRequest.GetSearchHolders(description, insuredSearchType, customerType, 1).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 0 && individualSearchType == 2) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                        }
                        else if (data.result.length == 0) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                        }
                        else if (data.result.length == 1) {
                            input.data("Object", data.result[0]);
                            input.val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                            hidden.val(data.result[0].IndividualId);
                        }
                        else if (data.result.length > 1) {
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].IdentificationDocument.Number,
                                    DocumentNum: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            itemSelected = 4;
                            $('#tableIndividualResults').UifDataTable('clear');
                            $('#tableIndividualResults').UifDataTable('addRow', dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);
                        }
                    }
                    else {
                        input.data("Object", null);
                        input.data("Detail", null);
                        input.val('');
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
                });
            }
        }

    }

    ///Buscar registros dependiendo la busqueda realizada en inputs de asegurado
    static GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, input, hidden) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            SubscriptionSearchRequest.GetHolders(description, insuredSearchType, customerType, 1).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0 && individualSearchType == 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchInsureds, 'autoclose': true });
                    }
                    else if (data.result.length == 1) {
                        input.data("Object", data.result[0]);
                        input.val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                        hidden.val(data.result[0].IndividualId);
                    }
                    else if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            });
                        }

                        $('#tableIndividualResults').UifDataTable('clear');
                        $('#tableIndividualResults').UifDataTable('addRow', dataList);

                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);

                    }
                }
                else {
                    input.data("Object", null);
                    input.data("Detail", null);
                    input.val('');
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
            });
        }
    }

    ///Buscar Intermediario Principal
    static GetAgenciesByAgentIdDescription(agentId, description, input, hidden, select) {
        var number = parseInt(description, 10);

        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            SubscriptionSearchRequest.GetUserAgenciesByAgentIdDescription(agentId, description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        if (agentSearchType == 1) {
                            input.data("Object", data.result[0]);
                            input.val(data.result[0].Agent.FullName + " (" + data.result[0].Agent.IndividualId + ")");
                            hidden.val(data.result[0].Agent.IndividualId);
                            SubscriptionSearch.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, data.result[0].Id, select);
                        }
                    }
                    else if (data.result.length > 1) {
                        modalListType = 1;
                        var dataList = [];

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                IndividualId: data.result[i].Agent.IndividualId,
                                Code: data.result[i].Code,
                                Description: data.result[i].Agent.FullName,
                                Id: data.result[i].Id
                            });
                        }
                        $('#tableResults').UifDataTable('clear');
                        $('#tableResults').UifDataTable('addRow', dataList);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelAgentPrincipal);
                    }
                    else {
                        input.data("Object", null);
                        input.val('');
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchAgents, 'autoclose': true });
                    }
                }
                else {
                    if (agentSearchType == 1) {
                        input.data("Object", null);
                        input.val('');
                    }
                    else if (agentSearchType == 2) {
                        input.data("Object", null);
                        input.val('');
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchAgents, 'autoclose': true });
            });
        }
    }

    ///Buscar agencias por id de intermediario
    static GetAgenciesByAgentId(agentId, selectedId, select) {
        SubscriptionSearchRequest.GetUserAgenciesByAgentId(agentId).done(function (data) {
            if (data.success) {
                select.UifSelect({ sourceData: data.result, selectedId: selectedId });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }


    /**
     * Funcion para descargar excel cotizaciones
     */
    sendExcelQuotation() {

        if (!$("#inputInsured").val() && !$("#inputQuotationNumber").val() && !$("#inputAgentPrincipal").val() && !$("#selectPrefixCommercial").UifSelect("getSelected") && !$("#inputQuotationNumber").val()
            && !$("#inputVersion").val() && !$("#inputPlate").val() && !$("#inputEngine").val() && !$("#inputChassis").val() && !$("#inputUser").val() && !$("#inputIssueDate").val()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelQuotationSearchValidate, 'autoclose': true });
        }
        else {
            if ($("#tableQuotation").UifDataTable("getData").length > 0) {
                var subscriptionSearchViewModel =
                    {
                        Insured: $("#inputInsured").val(),
                        InsuredId: $("#inputInsuredId").val(),
                        AgentPrincipal: $("#inputAgentPrincipal").val(),
                        AgentPrincipalId: $("#inputAgentPrincipalId").val(),
                        AgentAgency: $("#selectAgentAgency").UifSelect("getSelected"),
                        PrefixCommercialId: $("#selectPrefixCommercial").UifSelect("getSelected"),
                        QuotationNumber: $("#inputQuotationNumber").val(),
                        Version: $("#inputVersion").val(),
                        Plate: $("#inputPlate").val(),
                        Engine: $("#inputEngine").val(),
                        Chassis: $("#inputChassis").val(),
                        User: $("#inputUser").val(),
                        IssueDate: $("#inputIssueDate").val()
                    };

                SubscriptionSearchRequest.GenerateFileToExport(1, subscriptionSearchViewModel).done(function (data) {
                    if (data.success) {
                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorServiceNull, 'autoclose': true });
            }
        }
    }

    /**
     * Funcion para descargar excel polizas
     */
    sendExcelPolicy() {

        if (!$("#inputInsuredPolicy").val() && !$("#inputAgentPrincipalPolicy").val() && !$("#selectBranch").UifSelect("getSelected") && !$("#selectPrefix").UifSelect("getSelected")
            && !$("#inputEndorsementId").val() && !$("#inputPolicyNumber").val() && !$("#inputPlatePolicy").val() && !$("#inputEnginePolicy").val() && !$("#inputChassisPolicy").val()
            && !$("#inputUserPolicy").val() && !$("#inputIssueDatePolicy").val()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelPolicySearchValidate, 'autoclose': true });
        }
        else {
            if ($("#tableSearchPolicy").UifDataTable("getData").length > 0) {
                var subscriptionSearchViewModel =
                    {
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
                SubscriptionSearchRequest.GenerateFileToExport(2, subscriptionSearchViewModel).done(function (data) {
                    if (data.success) {
                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorServiceNull, 'autoclose': true });
            }
        }
    }

    CreateNewVersionQuotation() {
        if ($("#tableQuotation").UifDataTable('getSelected') != null) {
            $("#formSearchQuotation").validate();
            if ($("#formSearchQuotation").valid()) {
                SubscriptionSearchRequest.CreateNewVersionQuotation($("#tableQuotation").UifDataTable('getSelected')[0].OperationId).done(function (data) {
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelVersion + ' ' + data.result.Endorsement.QuotationVersion + ' ' + AppResources.MessageCreate, 'autoclose': true });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCreateNewVersionQuotation, 'autoclose': true });
                });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.Assignmenterror, 'autoclose': true });
        }
    }
}