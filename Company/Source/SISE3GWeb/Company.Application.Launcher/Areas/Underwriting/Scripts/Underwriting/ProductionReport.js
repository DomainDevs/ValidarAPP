
class ProductionReport extends Uif2.Page {
    getInitialState() {
        $("#inputAgentPrincipal").TextTransform(ValidatorType.UpperCase);
        $("#inputFrom").UifDatepicker('setValue', this.GetCurrentFromDate());
        $("#inputTo").UifDatepicker('setValue', this.GetCurrentToDate());
        ProductionReport.GetBranches();
        ProductionReport.GetPrefixes();
    }

    bindEvents() {
        $('#inputAgentPrincipal').on('buttonClick', this.SearchAgentPrincipal);
        $('#inputAgentPrincipal').on('blur', this.AgentBlurClean);
        $('#tableResults tbody').on('click', 'tr', this.SelectResults);
        $('#selectAgentAgency').on('itemSelected', this.ChangeAgentAgency);     
        $('#inputFrom').on("datepicker.change", this.GetCalculate);
        $('#selectPrefixCommercial').on('itemSelected', this.ChangePrefixCommercial);
        $('#btnSaveProduction').click(this.Export);
        $("#btnExitProduction").click(this.Exit);
    }

    static SearchAgentPrincipal() {
        agentSearchType = 1;
        ProductionReport.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim());
    }

    SelectResults() {
        switch (modalListType) {
            case 1:
                ProductionReport.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
                break;
            default:
                break;
        }
        $('#modalDefaultSearch').UifModal("hide");
    }

    static GetBranches() {
        ProductionReportRequest.GetBranches().done(function (data) {
            if (data.success) {
                $("#selectBranch").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

   

    static GetPrefixes() {
        ProductionReportRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $("#selectPrefixCommercial").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    ChangePrefixCommercial(event, selectedItem) {
        if (selectedItem.Id > 0) {
            if ($("#inputAgentPrincipal").data("Object") != undefined) {               
                ProductionReport.GetProductsByAgentIdPrefixId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, selectedItem.Id, 0);
            }
        }
        else {
            $("#selectProduct").UifSelect();
            $("#selectPolicyType").UifSelect();
        }
    }

    static GetPrefixesByAgentId(agentId, selectedId) {
        UnderwritingRequest.GetPrefixesByAgentId(agentId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectPrefixCommercial").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectPrefixCommercial").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPrefixesByAgentIdAgents(agentId) {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/ProductionReport/GetPrefixesByAgentIdAgents',
            data: JSON.stringify({ agentId: agentId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (!data.success) {
                $("#inputAgentsAgency").data("Object", null);
                $("#inputAgentsAgency").val('');
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.ErrorSearchPrefixAgent, 'autoclose': true });
        });
    }

    static GetProductsByAgentIdPrefixId(agentId, prefixId, selectedId) {
        var isCollective = ProductionReport.getQueryVariable("isCollective");

        if (isCollective != "true") {
            isCollective = false;
        }
        ProductionReportRequest.GetProductsByAgentIdPrefixId(agentId, prefixId, isCollective).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectProduct").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectProduct").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetAgenciesByAgentId(agentId, selectedId) {
        ProductionReportRequest.GetUserAgenciesByAgentId(agentId).done(function (data) {
            if (data.success) {
                if (agentSearchType == 1) {
                    $("#selectAgentAgency").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
                else if (agentSearchType == 2) {
                    $("#selectAgentAgency").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetAgenciesByAgentIdDescription(agentId, description) {
        var number = parseInt(description, 10);
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            ProductionReportRequest.GetUserAgenciesByAgentIdDescription(agentId, description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        if (glbPolicy == null) {
                            $("#selectBranch").UifSelect({ sourceData: ProductionReport.GetBranches() });
                            if (agentSearchType == 1) {
                                $("#inputAgentPrincipal").data("Object", data.result[0]);
                                $("#inputAgentPrincipal").val(data.result[0].Agent.FullName);                                
                                if ($("#selectPrefixCommercial").val()!="")
                                    ProductionReport.GetProductsByAgentIdPrefixId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, $("#selectPrefixCommercial").val(), 0);
                            }
                            else if (agentSearchType == 2) {
                                $("#inputAgentsAgency").data("Object", data.result[0]);
                                $("#inputAgentsAgency").val(data.result[0].FullName);                               
                            }                           
                        }
                        else if (ProductionReport.ExistProductAgentByAgency(data.result[0])) {
                            if (agentSearchType == 1) {
                                $("#inputAgentPrincipal").data("Object", data.result[0]);
                                $("#inputAgentPrincipal").val(data.result[0].Agent.FullName);
                            }
                            else if (agentSearchType == 2) {
                                $("#inputAgentsAgency").data("Object", data.result[0]);
                                $("#inputAgentsAgency").val(data.result[0].FullName);
                            }
                            ProductionReport.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, data.result[0].Id);
                        } else if (agentSearchType == 1) {
                            $("#inputAgentPrincipal").val(data.result[0].FullName);
                        }
                    }
                    else if (data.result.length > 1) {
                        modalListType = 1;
                        var dataList = [];

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].Agent.IndividualId,
                                Code: data.result[i].Code,
                                Description: data.result[i].Agent.FullName
                            });
                        }
                        ProductionReport.ShowDefaultResults(dataList);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelAgentPrincipal);
                    }
                    else {
                        if (agentSearchType == 1) {
                            $("#inputAgentPrincipal").data("Object", null);
                            $("#inputAgentPrincipal").val('');
                        }
                        else if (agentSearchType == 2) {
                            $("#inputAgentsAgency").data("Object", null);
                            $("#inputAgentsAgency").val('');
                        }
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchAgents, 'autoclose': true });
                    }
                }
                else {
                    if (agentSearchType == 1) {
                        $("#inputAgentPrincipal").data("Object", null);
                        $("#inputAgentPrincipal").val('');
                    }
                    else if (agentSearchType == 2) {
                        $("#inputAgentPrincipal").data("Object", null);
                        $("#inputAgentPrincipal").val('');
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchAgents, 'autoclose': true });
            });
        }
    }

    static ExistProductAgentByAgency(agency) {
        var result = false;
        ProductionReportRequest.ExistProductAgentByAgentIdPrefixIdProductId(agency.Agent.IndividualId, $("#selectPrefixCommercial").val(), $("#selectProduct").val()).done(function (data) {
            if (data.success) {
                if (data.result) {
                    result = data.result;
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageProductAgent, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchProductAgent, 'autoclose': true });
        });
        return result;
    }

    static GetBillingGroupByDescription(description) {
        var number = parseInt(description, 10);
        if (!isNaN(number) || description.length > 2) {
            ProductionReportRequest.GetBillingGroupByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $("#inputBillingGroup").data("Object", data.result[0]);
                        $("#inputBillingGroup").val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                        $("#inputRequest").data("Object", null);
                        $("#inputRequest").val("");
                        $('#inputRequest').UifInputSearch('disabled', false);
                    }
                    else if (data.result.length > 1) {
                        $("#inputRequest").data("Object", null);
                        modalListType = 3;
                        var dataList = { dataObject: [] };
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.dataObject.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        ProductionReport.ShowDefaultResults(dataList.dataObject);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelSelectBillingGroup);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchBillingGroup });
                        $("#inputRequest").data("Object", null);
                        $("#inputRequest").val("");
                        $('#inputRequest').UifInputSearch('disabled', true);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                    $("#inputRequest").data("Object", null);
                    $("#inputRequest").val("");
                    $('#inputRequest').UifInputSearch('disabled', true);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchBillingGroup });
                $("#inputRequest").data("Object", null);
                $('#inputRequest').UifInputSearch('disabled', true);
            });
        }
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    static ValidateAgency(agencyId) {
        var agentId = 0;
        if (agentSearchType == 1) {
            agentId = $("#inputAgentPrincipal").data("Object").Agent.IndividualId;
        }
        else if (agentSearchType == 2) {
            agentId = $("#inputAgentsAgent").data("Object").Agent.IndividualId;
        }
        ProductionReportRequest.GetAgencyByAgentIdAgencyId(agentId, agencyId).done(function (data) {
            if (data.success) {
                $("#inputAgentPrincipal").data("Object").Code = data.result.Code;
                $("#inputAgentPrincipal").data("Object").FullName = data.result.FullName;
                $("#inputAgentPrincipal").data("Object").Branch = data.result.Branch;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $('#selectAgentAgency').val(null);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
        });
    }

  
    static getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) {
                return pair[1];
            }
        }
    }

  
    ChangeAgentAgency(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ProductionReport.ValidateAgency(selectedItem.Id);
        }
        else {
            $("#selectAgentAgency").UifSelect();
        }
    }

    GetCurrentFromDate() {
        var currentTime = new Date();
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var date = day + "/" + month + "/" + year;
        return date;
    }

    GetCurrentToDate() {
        var currentTime = new Date();
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate() + 30;
        var year = currentTime.getFullYear();
        var date = day + "/" + month + "/" + year;
        return date;
    }

    GetCalculate(event, date) {
        var month = date.getMonth() + 1;
        var day = date.getDate() + 30;
        var year = date.getFullYear();
        var dateF = day + "/" + month + "/" + year;
        $("#inputTo").UifDatepicker('setValue', dateF);
    }


    Export() {        
        var agentId, agentAgencyId;
        if ($("#inputAgentPrincipal").UifSelect("getSelected") == "") {
            agentId = 0;
            agentAgencyId = 0;
        }
        else {
            agentId = $("#inputAgentPrincipal").data("Object").Code            
            agentAgencyId = $("#inputAgentPrincipal").data("Object").Code;
        }
        var branchId = $("#selectBranch").UifSelect("getSelected");      
        var prefixId = $("#selectPrefixCommercial").UifSelect("getSelected");
        var productId = $("#selectProduct").UifSelect("getSelected");
        var inputFrom = $("#inputFrom").UifSelect("getSelected");
        var inputTo = $("#inputTo").UifSelect("getSelected");
        if (productId < 0 || productId == "" || productId == null) {
            productId = -1;
        }
        if (branchId < 0 || branchId == "" || branchId == null) {
            branchId = -1;
        }        
       
       
        if (agentId < 0 || agentId == "" || agentId == null) {
            agentId = -1;
        }
        if (prefixId > 0) {
            ProductionReportRequest.GetFileProductionReport(agentId, branchId,  prefixId, productId, inputFrom, inputTo).done(function (data) {
                if (data.success) {
                    if (data.result == null)
                        $.UifNotify('show', { type: 'info', message: Resources.Language.ReportEmpty, autoclose: true });
                    else {                      
                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    }
                }
                else
                    $.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
            });
            
        }
        else {
            $.UifNotify('show', { type: 'danger', message: Resources.Language.ErrorPrefix, autoclose: true });
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    AgentBlurClean() {
        if ($("#inputAgentPrincipal").val() == 0) {
            $("#selectProduct").UifSelect({ sourceData: null });
        }
        else {
            ProductionReport.SearchAgentPrincipal();
        }
       
    }
}