var modalListType = 0; //Controla el listado que se mostrará en tableResults
var agenciesTmp = [];
var agencies = [];
var requestGroup = {};
var readOnlyRequest = {};
var validator;
var agentSearchType = 1;
var agentPrincipal = {};
var comissionProduct = 0;
var glbRequest = [];

//$(() => {
//    new RequestGrouping();
//    $("#inputRequest").data("request", null);
//});

class RequestGrouping extends Uif2.Page {

    getInitialState() {
        RequestGrouping.ShowPanel(MenuTypeGrouping.REQUESTGROUPING); //Muestra el panel principal
        //$('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $("#inputHolder").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputAgentPrincipal").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputRequest").ValidatorKey(ValidatorType.lettersandnumbersSpecial, 4, 0);
        $("#inputBillingGroup").ValidatorKey(ValidatorType.lettersandnumbersSpecial, 4, 0);
        $("#inputDescription").ValidatorKey(ValidatorType.lettersandnumbersSpecial, 1, 1);
        //cambia las minusculas a mayusculas en los input
       

        $('#DateNow').UifDatepicker('disabled', true);
        $("#CurrentFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#CurrentTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        RequestGrouping.createRequest();



        this.LoadDates();
        this.GetModuleDateIssue();

        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("textarea").TextTransform(ValidatorType.UpperCase);
    }

    bindEvents() {
       
        $("#btnAddBillingGroup").click(() => { this.LoadModalBillingGroup(); });
        $("#CurrentFrom").on("datepicker.change", (event, date) => { this.CurrentFromChange(); });
        $("#CurrentTo").on("datepicker.change", (event, date) => { this.CompareDates(); });

        //Muestra el panel de intermediarios
        $('#btnAgents').click(() => { this.BtnAgents(); });

        //Muestra el panel de tipos de negocio
        $('#btnBusinessType').click(() => { this.BtnBusinessType(); });

        //Realiza búsqueda de solicitud por descripción o código
        $("#inputRequest").on('buttonClick', () => { this.InputRequestSearch(); });

        //Realiza búsqueda de tomador por descripción o código
        $("#inputBillingGroup").on('buttonClick', () => { this.InputBillingGroupSearch(); });

        $("#btnExit").click(() => { this.Exit(); });
        $('#selectAgentAgency').on('itemSelected', (event, selectedItem) => { this.SeletedAgentAgency(selectedItem); });

        //Realiza búsqueda de tomador por descripción o código
        $("#inputHolder").on('buttonClick', () => { this.InputHolderSearch(); });

        //Realiza búsqueda de intermediario
        $("#inputAgentPrincipal").on('buttonClick', () => { this.InputAgentPrincipalSearch(); });

        $("#inputAgentsAgent").on('buttonClick', () => { this.InputAgentsAgentSearch(); });

        //Carga listado de opciones para productos
        $('#selectPrefixCommercial').on('itemSelected', (event, selectedItem) => { this.SelectPrefixCommercialLoad(selectedItem); });

        //evento al cambiar el select de sucursal
        $('#selectProduct').on('itemSelected', (event, selectedItem) => { this.SelectProductLoad(selectedItem); });

        //Seleccionar un elemento de la modal de busqueda
        $('#tableResults tbody').on('click', 'tr', this.TableResultSelected);

        //Guarda la solicitud agrupadora
        $('#btnRequestGroupingSave').click(() => { this.BtnRequestGroupingSave(); });

        //Evento boton nuevo
        $('#btnRequestGroupingNew').click(() => { RequestGrouping.cleanForm(); });

    }

    CurrentFromChange() {
        var currentDateFrom = $("#CurrentFrom").data("dateFrom");
        if (currentDateFrom != $("#CurrentFrom").val()) {
            $("#CurrentTo").UifDatepicker('setValue', AddToDate($("#CurrentFrom").val(), 0, 0, 1));
        }
        RequestGrouping.CalculateDays();
    }
    CompareDates() {
        if (CompareDates($("#CurrentFrom").val(), $("#CurrentTo").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateFinalDate, 'autoclose': true })
            $("#CurrentTo").UifDatepicker('setValue', GetCurrentFromDate());
        }
        RequestGrouping.CalculateDays();
    }
    BtnAgents() {
        validator = $("#requestForm").validate();
        var requestValidate = $("#requestForm").valid();

        //Si el formulario es válido
        if (requestValidate) {
            RequestGrouping.ShowPanel(MenuTypeGrouping.AGENTS);
            if (!readOnlyRequest.IsReadOnly) {
                agenciesTmp = agencies.slice(0);
            }

            LoadListViewAgencies();
        }
    }
    BtnBusinessType() {
        validator = $("#requestForm").validate();
        var requestValidate = $("#requestForm").valid();

        //Si el formulario es válido
        if (requestValidate) {
            this.ShowCoInsurance();
        }
    }

    InputRequestSearch() {
        if ($("#inputRequest").val().trim().length > 0) {
            RequestGrouping.GetCoRequestByDescription($("#inputRequest").val().trim());
        }
    }

    InputBillingGroupSearch() {
        if ($("#inputBillingGroup").val().trim().length > 0) {
            RequestGrouping.GetBillingGroupByDescription($("#inputBillingGroup").val().trim());
        }
    }
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    SeletedAgentAgency(selectedItem) {
        if (selectedItem.Id > 0) {
            this.ValidateAgency(selectedItem.Id);
        }
        else {
            $("#selectAgentAgency").UifSelect();
        }
    }
    ValidateAgency(agencyId) {

        var agentId = 0;
        agentId = $("#inputAgentPrincipal").data("Object").Agent.IndividualId;
        $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetAgencyByAgentIdAgencyId',
            data: JSON.stringify({ agentId: agentId, agencyId: agencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $("#inputAgentPrincipal").data("Object").Code = data.result.Code;
                $("#inputAgentPrincipal").data("Object").FullName = data.result.FullName;
                $("#inputAgentPrincipal").data("Object").Branch = data.result.Branch;
                RequestGrouping.LoadAgencies();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
                $('#selectAgentAgency').val(null);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Underwriting.ErrorSaveTemporary, 'autoclose': true })
        });
    }
    //Obtiene una solicitud agrupadora por Identificador o nombre
    static GetCoRequestByDescription(description) {

        var billingGroupId = 0;
        if ($("#inputBillingGroup").data("Object") != null) {
            billingGroupId = $("#inputBillingGroup").data("Object").Id;
        }
        if (billingGroupId != 0) {
            var number = parseInt(description, 10);
            if (!isNaN(number) || description.length > 2) {
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Massive/RequestGrouping/GetCoRequestByRequestIdDescription',
                    data: JSON.stringify({ description: description, billingGroupId: billingGroupId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            $("#inputRequest").data("Object", data.result[0]);
                            $("#inputRequest").val(data.result[0].Request.Description + ' (' + data.result[0].Request.Id + ')');
                            RequestGrouping.ShowRequest(data.result[0]);
                            RequestGrouping.DisablePage();
                        }
                        else if (data.result.length > 1) {
                            modalListType = 3;
                            RequestGrouping.LoadListView(data);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchRequest });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorWhenRequestingRequest });
                });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectBillingGroup });
        }
    }
    //Obtiene grupo de facturación por Identificador o nombre
    static GetBillingGroupByDescription(description) {

        var number = parseInt(description, 10);
        if (!isNaN(number) || description.length > 2) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Massive/RequestGrouping/GetBillingGroupByDescription',
                data: JSON.stringify({ description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $("#inputBillingGroup").data("Object", data.result[0]);
                        $("#inputBillingGroup").val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                    }
                    else if (data.result.length > 1) {
                        modalListType = 4;
                        RequestGrouping.LoadListView(data);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchBillingGroup });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchBillingGroup });
            });
        }
    }


    //Muestra la información de una solicitud agrupadora
    static ShowRequest(data) {

        glbRequest = data.Request;
        readOnlyRequest.IsReadOnly = true;
        agenciesTmp = data.RequestEndorsement.CoRequestAgent.slice(0);
        //LoadListViewAgenciesReadOnly();

        $("#selectBranch").UifSelect({ selectedId: data.Request.Branch.Id });
        $("#inputDescription").val(data.Request.Description);
        RequestGrouping.GetHoldersById(data.RequestEndorsement.Holder.IndividualId, InsuredSearchType.IndividualId);
        //RequestGrouping.GetComissionByProductId(data.RequestEndorsement.Product.Id);
        $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" + agenciesTmp.length + ") " + Resources.Language.LabelCommission + ": " + FormatMoney(comissionProduct) + "%");
        RequestGrouping.LoadAgenciesByAgent(data.PrincipalAgent.Agency.Agent.IndividualId, data.PrincipalAgent.Agency.Id, data.Request.Prefix.Id, data.RequestEndorsement.Product.Id);
        RequestGrouping.GetPolicyTypesByProductId(data.RequestEndorsement.Product.Id);
        readOnlyRequest.PolicyTypeSelected = data.RequestEndorsement.PolicyType.Id;
        if (data.RequestEndorsement.PaymentPlan != null) {
            readOnlyRequest.PaymentPlan = data.RequestEndorsement.PaymentPlan.Id;
        }
        RequestGrouping.GetPaymentPlanByProductId(data.RequestEndorsement.Product.Id);

        $("#CurrentFrom").val(FormatDate(data.RequestEndorsement.CurrentFrom));
        $("#CurrentTo").val(FormatDate(data.RequestEndorsement.CurrentTo));
        $("#Annotations").val(data.RequestEndorsement.Annotations);

        if (data.RequestEndorsement.IsOpenEffect) {
            $("#Open").prop("checked", true)
        }
        else {
            $("#Closed").prop("checked", true)
        }

        RequestGrouping.GetBusinessTypeById(glbRequest.BusinessType).done(function (dataBusinessType) {
            if (dataBusinessType.success) {
                $('#selectedBusinessType').text(dataBusinessType.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': dataBusinessType.result, 'autoclose': true });
            }
        });

    }

    //Deshabilita los controles de la página
    static DisablePage() {
        $("#inputRequest").prop("disabled", true);
        $("#selectBranch").prop("disabled", true);
        $("#inputDescription").prop("disabled", true);
        $("#inputAgentPrincipal").prop("disabled", true);
        $("#selectAgentAgency").prop("disabled", true);
        $("#inputHolder").prop("disabled", true);
        $("#selectPrefixCommercial").prop("disabled", true);
        $("#selectPolicyType").prop("disabled", true);
        $("#selectPaymentPlan").prop("disabled", true);
        $("#Closed").prop("disabled", true);
        $("#Open").prop("disabled", true);
        $("#CurrentFrom").prop("disabled", true);
        $("#CurrentTo").prop("disabled", true);
        $("#Annotations").prop("disabled", true);
        $("#inputAgentsAgent").prop("disabled", true);
        $("#AgencyId").prop("disabled", true);
        $("#inputAgentsParticipation").prop("disabled", true);
        $("#selectProduct").prop("disabled", true);

        $("#btnRequestGroupingSave").prop("disabled", true);
        $("#btnAgentsAccept").prop("disabled", true);
        $("#btnAgentsSave").prop("disabled", true);

        $('#CurrentFrom').UifDatepicker('disabled', true);
        $('#CurrentTo').UifDatepicker('disabled', true);
    }

    InputHolderSearch() {
        if ($("#inputHolder").val().trim().length > 0) {
            this.GetHoldersByDescription($("#inputHolder").val().trim());
        }
    }
    InputAgentPrincipalSearch() {
        agentSearchType = 1;
        RequestGrouping.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim());
    }
    InputAgentsAgentSearch() {
        agentSearchType = 2;
        RequestGrouping.GetAgenciesByAgentIdDescription(0, $("#inputAgentsAgent").val().trim());
    }
    SelectPrefixCommercialLoad(selectedItem) {
        if (selectedItem.Id > 0) {

            if ($("#inputAgentPrincipal").data("Object") != null && $("#inputAgentPrincipal").data("Object").Agent.IndividualId != null) {

                RequestGrouping.GetProductsByAgentIdPrefixId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, selectedItem.Id, 0);
            }
        }
        else {
            $("#selectProduct").UifSelect();
            $("#selectPolicyType").UifSelect();
        }
    }
    SelectProductLoad(selectedItem) {
        if (selectedItem.Id > 0) {
            RequestGrouping.GetPolicyTypesByProductId($("#selectProduct").UifSelect("getSelected"));
            RequestGrouping.GetPaymentPlanByProductId($("#selectProduct").UifSelect("getSelected"));
            //RequestGrouping.GetComissionByProductId($("#selectProduct").UifSelect("getSelected"));
            $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" + agenciesTmp.length + ") " + Resources.Language.LabelCommission + ": " + FormatMoney(comissionProduct) + "%");
        }
        else {
            $("#selectPolicyType").UifSelect({ source: null });
            $("#selectPaymentPlan").UifSelect({ source: null });
        }
    }
    //Obtiene los tipos de poliza por producto
    static GetPolicyTypesByProductId(productId) {
        $("#selectPolicyType").UifSelect();

        $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetPolicyTypesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (data.result.length >= 1) {
                    RequestGrouping.SetLoadTypesPolicy(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoPolicyTypesFound, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'error', 'message': Resources.Language.ErrorSearchPolicyType, 'autoclose': true });
        });
    }

    //Carga los tipos de poliza seleccionando el tipo por defecto del producto
    static SetLoadTypesPolicy(data) {
        var selectedId = 0;
        $("#selectPolicyType").prop("disabled", false);
        $("#selectPolicyType").append(new Option("- Seleccione un item - ", ""));
        if (Object.getOwnPropertyNames(readOnlyRequest).length > 0) {
            selectedId = readOnlyRequest.PolicyTypeSelected;
        }
        $.each(data, function (i, item) {
            if (item.IsDefault) {
                selectedId = item.Id;
            }
            if (item.Id == selectedId) {
                $("#selectPolicyType").append($('<option>', {
                    value: item.Id,
                    text: item.Description,
                    selected: true
                }));
            }
            else {
                $("#selectPolicyType").append($('<option>', {
                    value: item.Id,
                    text: item.Description
                }));
            }
        });
    }

    //Obtiene los planes de pago por producto
    static GetPaymentPlanByProductId(productId) {
        $("#selectPaymentPlan").UifSelect();

        $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetPaymentPlanByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (data.result.length >= 1) {
                    RequestGrouping.SetLoadPaymentPlan(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoFoundPaymentPlans })
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPaymentPlan })
        });
    }
    //Carga los planes de pago seleccionando el por defecto del producto 
    static SetLoadPaymentPlan(data) {
        var selectedId = 0;
        $("#selectPaymentPlan").prop("disabled", false);
        $("#selectPaymentPlan").append(new Option(Resources.Language.SelectAItem, ""));
        if (Object.getOwnPropertyNames(readOnlyRequest).length > 0) {
            selectedId = readOnlyRequest.PaymentPlan;
        }
        $.each(data, function (i, item) {
            if (item.IsDefault && selectedId == 0) {
                selectedId = item.Id;
            }
            if (item.Id == selectedId) {
                $("#selectPaymentPlan").append($('<option>', {
                    value: item.Id,
                    text: item.Description,
                    selected: true
                }));
            }
            else {
                $("#selectPaymentPlan").append($('<option>', {
                    value: item.Id,
                    text: item.Description
                }));
            }
        });
    }

    //Obtiene el listado de opciones para ramo comercial
    static GetPrefixesByAgentId(agentId, selectedId) {
        var controller = rootPath + 'Massive/RequestGrouping/GetPrefixesByAgentId?agentId=' + agentId;
        if (selectedId == 0) {
            $("#selectPrefixCommercial").UifSelect({ source: controller });
        }
        else {
            $("#selectPrefixCommercial").UifSelect({ source: controller, selectedId: selectedId });
        }

        if ($("#selectPrefixCommercial option").length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageAgentWithoutPrefix })
        }
    }

    //Obtiene los productos por intermediario y ramo comercial
    static GetProductsByAgentIdPrefixId(agentId, prefixId, selectedId) {
        var controller = rootPath + 'Massive/RequestGrouping/GetProductsByAgentIdPrefixId?agentId=' + agentId + '&prefixId=' + prefixId + '&isCollective=' + false;
        if (selectedId == 0) {
            $("#selectProduct").UifSelect({ source: controller });
        }
        else {
            $("#selectProduct").UifSelect({ source: controller, selectedId: selectedId });
        }

        if ($("#selectProduct option").length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AgentNoAssociatedProducts })
        }
    }

    //Obtiene las agencia del agente seleccionado
    static LoadAgenciesByAgent(agentId, agencySelected, prefixSelected, productSelected) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetAgentByIndividualId',
            data: JSON.stringify({ individualId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            $("#inputAgentPrincipal").data("Object", data.result);
            var agenciesLoad = data.result.Agencies;
            $.each(agenciesLoad, function (key, value) {
                if (this.Id == agencySelected) {
                    return false;
                }
            });
            $("#inputAgentPrincipal").val(data.result.FullName);

            RequestGrouping.GetPrefixesByAgentId(agentId, prefixSelected);//Carga los ramos por intermediario        
            RequestGrouping.GetAgenciesByAgentId(agentId, agencySelected); //Carga las agencias del intermediario        
            RequestGrouping.GetProductsByAgentIdPrefixId(agentId, prefixSelected, productSelected);
        });
    }

    //Obtiene las agencias por intermediario 
    static GetAgenciesByAgentIdDescription(agentId, description) {
        var number = parseInt(description, 10);

        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            $.ajax({
                type: "POST",
                url: rootPath + 'RequestGrouping/GetAgenciesByAgentIdDescription',
                data: JSON.stringify({ agentId: agentId, description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        if (agentSearchType == 1) {
                            $("#inputAgentPrincipal").data("Object", data.result[0]);
                            $("#inputAgentPrincipal").val(data.result[0].Agent.FullName);
                            RequestGrouping.GetPrefixesByAgentId(data.result[0].Agent.IndividualId, 0);
                        }
                        else if (agentSearchType == 2) {
                            $("#inputAgentsAgent").data("Object", data.result[0]);
                            $("#inputAgentsAgent").val(data.result[0].Agent.FullName);
                        }
                        RequestGrouping.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, data.result[0].Id);
                        RequestGrouping.LoadAgencies();
                    }
                    else if (data.result.length > 1) {
                        modalListType = 2;
                        RequestGrouping.LoadListView(data);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNotAgents });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryAgents });
            });
        }
    }

    //Muestra listado de intermediario o tomador
    static ShowModalList(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }
    TableResultSelected() {
        switch (modalListType) {
            case 1:
                RequestGrouping.GetHoldersById($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
                break;
            case 2:
                RequestGrouping.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
                break;
            case 3:
                RequestGrouping.GetCoRequestByDescription($(this).children()[1].innerHTML);
                break;
            case 4:
                RequestGrouping.GetBillingGroupByDescription($(this).children()[1].innerHTML);
                break;
            default:
                break;
        }
        $('#modalDialogList').UifModal("hide");
    }
    static LoadListView(data) {

        var dataList = { dataObject: [] };

        switch (modalListType) {
            case 1:
                for (var i = 0; i < data.result.length; i++) {
                    dataList.dataObject.push({
                        Id: data.result[i].IndividualId,
                        Code: data.result[i].IdentificationDocument.Number,
                        Description: data.result[i].Name
                    });
                }
                $('#modalDialogList').UifModal('showLocal', Resources.Language.LabelSelectHolder);
                break;
            case 2:
                for (var i = 0; i < data.result.length; i++) {
                    dataList.dataObject.push({
                        Id: data.result[i].Agent.IndividualId,
                        Code: data.result[i].Code,
                        Description: data.result[i].Agent.FullName
                    });
                }
                $('#modalDialogList').UifModal('showLocal', Resources.Language.SelectMainAgent);
                break;
            case 3:
                for (var i = 0; i < data.result.length; i++) {
                    dataList.dataObject.push({
                        Id: data.result[i].Request.Id,
                        Code: data.result[i].Request.Id,
                        Description: data.result[i].Request.Description
                    });
                }
                $('#modalDialogList').UifModal('showLocal', Resources.Language.LabelSelectRequest);
                break;
            case 4:
                for (var i = 0; i < data.result.length; i++) {
                    dataList.dataObject.push({
                        Id: data.result[i].Id,
                        Code: data.result[i].Id,
                        Description: data.result[i].Description
                    });
                }
                $('#modalDialogList').UifModal('showLocal', Resources.Language.LabelSelectBillingGroup);
                break;
            default:
                break;
        }

        RequestGrouping.ShowModalList(dataList.dataObject);
    }

    //Consultá tomador por código o descripción
    GetHoldersByDescription(description) {


        var number = parseInt(description, 10);

        if (!isNaN(number) || description.length > 2) {

            $.ajax({
                type: "POST",
                url: rootPath + 'Massive/RequestGrouping/GetHoldersByDescription',
                data: JSON.stringify({ description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {

                if (data.success) {

                    if (data.result.length == 1) {

                        $("#inputHolder").data("Object", data.result[0]);
                        $("#inputHolder").val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                        detailType = 1;
                    }

                    else if (data.result.length > 1) {
                        modalListType = 1;
                        RequestGrouping.LoadListView(data);
                    }

                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchHolders });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchHolders });
            });
        }
    }

    //Consulta un tomador dado su identificador
    static GetHoldersById(id, SearchType) {


        var number = parseInt(id, 10);

        if (!isNaN(number) || description.length > 2) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Massive/RequestGrouping/GetHoldersById',
                data: JSON.stringify({ id: id, searchTypeId: SearchType }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {

                if (data.success) {
                    if (data.result != null) {
                        $("#inputHolder").data("Object", data.result);
                        $("#inputHolder").val(data.result.Name + ' (' + data.result.IdentificationDocument.Number + ')');
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchHolders });
                    }
                }

                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchHolders });
            });
        }
    }

    //Agrega las agencias en el select
    static GetAgenciesByAgentId(agentId, selectedId) {
        var controller = rootPath + 'Massive/RequestGrouping/GetAgenciesByAgentId?agentId=' + agentId;
        if (agentSearchType == 1) {
            $("#selectAgentAgency").UifSelect({ source: controller, selectedId: selectedId });
        }
        else if (agentSearchType == 2) {
            $("#selectAgentsAgency").UifSelect({ source: controller, selectedId: selectedId });
        }
    }

    //Ocultar paneles
    static HidePanels(Menu) {

        switch (Menu) {
            case MenuTypeGrouping.REQUESTGROUPING:
                break;
            case MenuTypeGrouping.AGENTS:
                $("#modalAgents").UifModal('hide');
                break;
            case MenuTypeGrouping.CoInsurance:
                $("#modalCoInsurance").UifModal('hide');
                break;
        }
    }

    //Mostrar panel
    static ShowPanel(Menu) {
        switch (Menu) {
            case MenuTypeGrouping.REQUESTGROUPING:
                break;
            case MenuTypeGrouping.AGENTS:
                $("#modalAgents").UifModal('showLocal', Resources.Language.SelectAgent);
                break;
            case MenuTypeGrouping.CoInsurance:
                $("#modalCoInsurance").UifModal('showLocal', Resources.Language.LabelDataBusinessType);
                break;
        }
    }

    //Carga el intermediario principal en arreglo temporal
    static LoadAgencies() {

        if (!readOnlyRequest.IsReadOnly) {

            var agency = {
                Id: $('#selectAgentAgency').UifSelect('getSelected'),
                IsPrincipal: true,
                Participation: 100,
                Agent: {
                    IndividualId: $("#inputAgentPrincipal").data("Object").Agent.IndividualId,
                    FullName: $("#inputAgentPrincipal").data("Object").FullName
                },
                Code: $("#inputAgentPrincipal").data("Object").Code
            };

            agencies = [];
            agenciesTmp = [];
            requestGroup.CompanyRequestEndorsements[0].Agencies = [];
            requestGroup.CompanyRequestEndorsements[0].Agencies.push(agency);
            agencies.push(agency);
            agenciesTmp.push(agency);

            if (agenciesTmp != null) {
                $.each(agenciesTmp, function (key, value) {
                    if (this.IsPrincipal == true || agenciesTmp.length == 1) {
                        if (this.Participation == 0) {
                            this.Participation = 100;
                        }
                        $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" + agenciesTmp.length + ") " + Resources.Language.LabelCommission + ": " + FormatMoney(comissionProduct) + "%");
                    }
                });
            }
        }
    }

    //Crea el objeto con la información de la solicitud agrupadora
    static createRequest() {
        //CoRequest
        requestGroup.RequestDate = new Date();
        requestGroup.Branch = { Id: $("#selectBranch").UifSelect("getSelected") };
        requestGroup.Prefix = { Id: $("#selectPrefixCommercial").UifSelect("getSelected") };
        requestGroup.Description = $("#inputDescription").val();
        requestGroup.BusinessType = glbRequest.BusinessType;
        requestGroup.InsuranceCompanies = glbRequest.InsuranceCompanies;
        if ($("#inputBillingGroup").data("Object") != null) {
            requestGroup.BillingGroup = { Id: $("#inputBillingGroup").data("Object").Id };
        }

        if (requestGroup.CompanyRequestEndorsements == null || requestGroup.CompanyRequestEndorsements.length <= 0) {
            requestGroup.CompanyRequestEndorsements = [];

            //Endorsement
            var Endorsement = {};

            Endorsement.EndorsementDate = new Date();
            Endorsement.Holder = { IndividualId: ($("#inputHolder").data("Object") != null ? $("#inputHolder").data("Object").IndividualId : "") };
            Endorsement.CurrentFrom = $("#CurrentFrom").val();
            Endorsement.CurrentTo = $("#CurrentTo").val();

            var radioValue = $("input[name='effect']:checked").val();

            if (radioValue == "Open") {
                Endorsement.IsOpenEffect = true;
            }
            else {
                Endorsement.IsOpenEffect = false;
            }


            Endorsement.PaymentPlan = { Id: $("#selectPaymentPlan").UifSelect("getSelected") };
            Endorsement.Annotations = $("#Annotations").val();
            Endorsement.Product = { Id: $("#selectProduct").UifSelect("getSelected") };
            Endorsement.Prefix = { Id: $("#selectPrefixCommercial").UifSelect("getSelected") };
            Endorsement.PolicyType = { Id: $("#selectPolicyType").UifSelect("getSelected") };
            Endorsement.GiftAmount = 0;
            Endorsement.IssueExpensesAmount = 0;
            Endorsement.MonthPayerDay = 0;
            Endorsement.EndorsementType = 1;
            Endorsement.DocumentNumber = 0;
            Endorsement.Agencies = [];

            requestGroup.CompanyRequestEndorsements.push(Endorsement);
        }
        else {
            requestGroup.CompanyRequestEndorsements[0].EndorsementDate = new Date();
            requestGroup.CompanyRequestEndorsements[0].Holder = { IndividualId: ($("#inputHolder").data("Object") != null ? $("#inputHolder").data("Object").IndividualId : "") };
            requestGroup.CompanyRequestEndorsements[0].CurrentFrom = $("#CurrentFrom").val();
            requestGroup.CompanyRequestEndorsements[0].CurrentTo = $("#CurrentTo").val();

            var radioValue = $("input[name='effect']:checked").val();

            if (radioValue == "Open") {
                requestGroup.CompanyRequestEndorsements[0].IsOpenEffect = true;
            }
            else {
                requestGroup.CompanyRequestEndorsements[0].IsOpenEffect = false;
            }

            requestGroup.CompanyRequestEndorsements[0].PaymentPlan = { Id: $("#selectPaymentPlan").UifSelect("getSelected") };
            requestGroup.CompanyRequestEndorsements[0].Annotations = $("#Annotations").val();
            requestGroup.CompanyRequestEndorsements[0].Product = { Id: $("#selectProduct").UifSelect("getSelected") };
            requestGroup.CompanyRequestEndorsements[0].Prefix = { Id: $("#selectPrefixCommercial").UifSelect("getSelected") };
            requestGroup.CompanyRequestEndorsements[0].PolicyType = { Id: $("#selectPolicyType").UifSelect("getSelected") };
            requestGroup.CompanyRequestEndorsements[0].GiftAmount = 0;
            requestGroup.CompanyRequestEndorsements[0].IssueExpensesAmount = 0;
            requestGroup.CompanyRequestEndorsements[0].MonthPayerDay = 0;
            requestGroup.CompanyRequestEndorsements[0].EndorsementType = 1;
            requestGroup.CompanyRequestEndorsements[0].DocumentNumber = 0;
        }
    }
    BtnRequestGroupingSave() {
        validator = $("#requestForm").validate();
        var requestValidate = $("#requestForm").valid();

        //Si el formulario es válido
        if (requestValidate) {
            if ($("#inputBillingGroup").data("Object") == null) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.RequiredBillingGroup, 'autoclose': true });
            }
            else {
                RequestGrouping.createRequest(); //Crea el objeto en memoria
                RequestGrouping.saveRequest(); //Realiza llamado Ajax
            }
        }
    }
    static saveRequest() {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/RequestGrouping/SaveCoRequest',
            data: JSON.stringify({ model: requestGroup }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $.UifDialog('alert', { 'message': Resources.Language.GroupRequestCreatedNo + ": " + data.result.Id });
                        RequestGrouping.DisablePage();
                        $("#inputRequest").val(data.result.Id);
                        RequestGrouping.cleanForm();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            }
        });
    }
    static enableForm() {
        $("#inputRequest").removeAttr('disabled');
        $("#selectBranch").removeAttr('disabled');
        $("#inputDescription").removeAttr('disabled');
        $("#inputAgentPrincipal").removeAttr('disabled');
        $("#selectAgentAgency").removeAttr('disabled');
        $("#inputHolder").removeAttr('disabled');
        $("#selectPrefixCommercial").removeAttr('disabled');
        $("#selectProduct").removeAttr('disabled');
        $("#selectPolicyType").removeAttr('disabled');
        $("#selectPaymentPlan").removeAttr('disabled');
        $("#btnAgentsAccept").removeAttr("disabled");

        $('#CurrentFrom').UifDatepicker('disabled', false);
        $('#CurrentTo').UifDatepicker('disabled', false);

        $("#Annotations").removeAttr('disabled');
        $("#btnRequestGroupingSave").removeAttr('disabled');
        $("#inputAgentsAgent").removeAttr('disabled');
        $("#selectAgentsAgency").removeAttr('disabled');
        $("#inputAgentsParticipation").removeAttr('disabled');
        $("#btnAgentsSave").removeAttr('disabled');
        $("#Closed").removeAttr('disabled');
        $("#Open").removeAttr('disabled');
    }
    static CalculateDays() {

        var aFecha1 = $("#CurrentFrom").val().toString().split('/');
        var aFecha2 = $("#CurrentTo").val().toString().split('/');
        var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
    }
    static cleanForm() {
        RequestGrouping.enableForm();
        $("#inputBillingGroup").val("");
        $('#inputBillingGroup').data('Object', null);
        $("#inputRequest").val("");
        $('#inputRequest').data('Object', null);
        $("#selectBranch").UifSelect("setSelected", "");
        $("#inputDescription").val("");
        $("#inputAgentPrincipal").val("");
        $("#inputAgentPrincipal").data("Object", null);

        $("#selectBranch").UifSelect({ selectedId: 0 });

        $("#selectAgentAgency").UifSelect({ source: null });
        $("#selectAgentAgency").UifSelect("setSelected", "");

        $("#inputHolder").val("");
        $("#inputHolder").data("Object", null);

        $("#selectPrefixCommercial").UifSelect({ source: null });
        $("#selectPrefixCommercial").UifSelect("setSelected", "");

        $("#selectPolicyType").UifSelect({ source: null });
        $("#selectPolicyType").UifSelect("setSelected", "");

        $("#selectPaymentPlan").UifSelect({ source: null });
        $("#selectPaymentPlan").UifSelect("setSelected", "");

        $("#selectProduct").UifSelect({ source: null });
        $("#selectProduct").UifSelect("setSelected", "");

        $("#CurrentFrom").UifDatepicker('setValue', GetCurrentFromDate());
        $("#CurrentTo").UifDatepicker('setValue', AddToDate($("#CurrentFrom").val(), 0, 0, 1));

        $("#CurrentFrom").data("dateFrom", $("#CurrentFrom").val());
        RequestGrouping.CalculateDays();
        $("#Annotations").val("");
        $("#selectedAgents").text("");
        $("#selectedBusinessType").text("");
        $("#lblAgentsTotalParticipation").text("");
        $("#requestForm").formReset();

        CoInsurance.ClearAll();

        readOnlyRequest = {};
        requestGroup = {};
        agencies = [];
        agenciesTmp = [];
        RequestGrouping.createRequest();
        comissionProduct = 0;
        glbRequest = [];

    }
    //Carga Modal para crear grupo de facturación
    LoadModalBillingGroup() {

        $("#formBillingGroup").formReset();
        $('#modalBillingGroup').UifModal('showLocal', Resources.Language.GroupInformation);

    }

    LoadDates() {
        $("#CurrentFrom").UifDatepicker('setValue', GetCurrentFromDate());
        $("#CurrentTo").UifDatepicker('setValue', AddToDate($("#CurrentFrom").val(), 0, 0, 1));
        $("#CurrentFrom").data("dateFrom", $("#CurrentFrom").val());
        RequestGrouping.CalculateDays();
    }

    GetModuleDateIssue() {
        $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetModuleDateIssue',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $("#inputIssueDate").text(FormatFullDate(data.result));
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Underwriting.ErrorSearchDateIssue, 'autoclose': true });
        });
    }
    ShowCoInsurance() {
        if (glbRequest != null && glbRequest.BusinessType != null) {
            $("#selectCoInsBusinessType").UifSelect("setSelected", glbRequest.BusinessType);
            CoInsurance.LoadCoinsurance();
        }
        RequestGrouping.ShowPanel(MenuTypeGrouping.CoInsurance)
    }

    static GetBusinessTypeById(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetBusinessTypeById',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

}