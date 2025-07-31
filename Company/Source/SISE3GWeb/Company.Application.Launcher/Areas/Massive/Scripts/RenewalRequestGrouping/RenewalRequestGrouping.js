var glbRequest = null;

$(() => {
    new RenewalRequestGrouping();
$("#inputRequest").data("renewalRequest", null);
});

class RenewalRequestGrouping extends Uif2.Page {
    getInitialState() {
        ObjectCoInsurance.bindEvents();
        RenewalRequestGrouping.HideModal();
        RenewalRequestGrouping.DisabledControls();
        $("#btnAgents").prop("disabled", true);
        $("textarea").TextTransform(ValidatorType.UpperCase);
        $("#CurrentFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#CurrentTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputRenewalRequest").ValidatorKey(ValidatorType.Number, 0, 0);
        RenewalRequestGroupingRequest.GetPrefixes().done(function (dataPrefix) {
            if (dataPrefix.success) {
                $('#selectPrefix').UifSelect({ sourceData: dataPrefix.result });
                $('#selectPrefix').prop('disabled', true)
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        RenewalRequestGroupingRequest.GetBranches().done(function (dataBranch) {
            if (dataBranch.success) {
                $('#selectBranch').UifSelect({ sourceData: dataBranch.result });
                $('#selectBranch').prop('disabled', true)

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {

        $('#inputRenewalRequest').on('buttonClick', RenewalRequestGrouping.GetRequestByRequestId);
        $('#btnRequestGroupingSave').on('click', this.SaveRenewalRequest);
        $('#btnAgents').click(this.ShowAgent);
        $('#btnBusinessType').click(this.ShowCoInsurance);
        $('#btnExit').click(this.Exit);
        $('#btnNewRenewal').click(RenewalRequestGrouping.ClearNewRenewal);

    }
    static GetRequestByRequestId() {
        var description = $('#inputRenewalRequest').val().trim();
        var number = parseInt(description, 10);
        if((!isNaN(number) && number != 0) || description.length > 2) {
            RenewalRequestGroupingRequest.GetRequestByRequestId($('#inputRenewalRequest').val().trim()).done(function (data) {
                if (data.success) {
                    data.result.RequestDate = FormatDate(data.result.RequestDate);
                    data.result.CoRequestEndorsement["0"].Product.CurrentFrom = FormatDate(data.result.CoRequestEndorsement["0"].Product.CurrentFrom);
                    data.result.CoRequestEndorsement["0"].CurrentFrom = FormatDate(data.result.CoRequestEndorsement["0"].CurrentFrom);
                    data.result.CoRequestEndorsement["0"].CurrentTo = FormatDate(data.result.CoRequestEndorsement["0"].CurrentTo);
                    data.result.CoRequestEndorsement["0"].EndorsementDate = FormatDate(data.result.CoRequestEndorsement["0"].EndorsementDate);
                    data.result.CoRequestEndorsement["0"].Holder.DeclinedDate = FormatDate(data.result.CoRequestEndorsement["0"].Holder.DeclinedDate);
                    $("#selectBranch").UifSelect("setSelected", data.result.Branch.Id);
                    $("#selectBranch").UifSelect("setSelected", data.result.Branch.Id);
                    $("#inputRequest").val(data.result.Id);
                    $("#inputRequest").data("renewalRequest", data.result);
                    glbRequest=data.result;
                    $("#inputDescription").val(data.result.Description);
                    RenewalRequestGroupingRequest.GetProductByProductId(data.result.CoRequestEndorsement["0"].Product.Id).done(function (dataProduct) {
                        if (dataProduct.success) {
                            var product = [];
                            product.push(dataProduct.result);
                            $('#selectProduct').UifSelect({ sourceData: product });
                            $('#selectProduct').UifSelect("setSelected", dataProduct.result.Id);

                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': dataProduct.result, 'autoclose': true });
                        }
                    });
                    RenewalRequestGroupingRequest.GetPolicyTypesByProductId(data.result.CoRequestEndorsement["0"].Product.Id).done(function (dataPolicyTypes) {
                        if (dataPolicyTypes.success) {
                            $('#selectPolicyType').UifSelect({ sourceData: dataPolicyTypes.result });
                            $('#selectPolicyType').UifSelect("setSelected", data.result.CoRequestEndorsement["0"].PolicyType.Id);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': dataPolicyTypes.result, 'autoclose': true });
                        }
                    });
                    RenewalRequestGroupingRequest.GetPaymentPlanByProductId(data.result.CoRequestEndorsement["0"].Product.Id).done(function (dataPaymentPlan) {
                        if (dataPaymentPlan.success) {
                            $('#selectPaymentPlan').UifSelect({ sourceData: dataPaymentPlan.result });
                            $('#selectPaymentPlan').UifSelect("setSelected", data.result.CoRequestEndorsement["0"].PaymentPlan.Id);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': dataPaymentPlan.result, 'autoclose': true });
                        }
                    });

                    RenewalRequestGroupingRequest.GetHoldersById(data.result.CoRequestEndorsement[0].Holder.IndividualId).done(function (dataHolder) {
                        if (dataHolder.success) {
                            $("#inputHolder").val(dataHolder.result.Name);
                            var holder = { Id: dataHolder.result.Id, Name: dataHolder.result.Name }
                            $("#inputHolder").data("Holder", holder);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': dataHolder.result, 'autoclose': true });
                        }
                    });
                    var totalParticipation = 0;
                    for (var i in data.result.CoRequestEndorsement["0"].CoRequestAgent) {
                        data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.DateDeclined = FormatDate(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.DateDeclined);
                        data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.DateCurrent = FormatDate(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.DateCurrent);
                        data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.DateDeclined = FormatDate(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.DateDeclined);
                        data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.DateModification = FormatDate(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.DateModification);
                        totalParticipation += parseFloat(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Participation.toString().replace(separatorDecimal, separatorThousands));
                        if (data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.IsPrincipal == true) {
                            $("#inputAgentPrincipal").val(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.FullName);
                            var agencyId = data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Id;
                            RenewalRequestGroupingRequest.GetAgencyByAgentIdByAgencyId(data.result.CoRequestEndorsement[0].CoRequestAgent[i].Agency.Agent.IndividualId, agencyId).done(function (dataAgency) {
                                if (dataAgency.success) {
                                    var agency = [];
                                    agency.push(dataAgency.result);
                                    $('#selectMainAgentAgency').UifSelect({ sourceData: agency, selectedId: agencyId });
                                    $("#selectMainAgentAgency").data("Agency", agencyId);
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': dataAgency.result, 'autoclose': true });
                                }
                            });
                            // break;
                        }
                    }
                    $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" + data.result.CoRequestEndorsement[0].CoRequestAgent.length + ") " + Resources.Language.LabelCommission + ": " + FormatMoney(totalParticipation) + "%");
                    $("#lblAgentsTotalParticipation").text(FormatMoney(totalParticipation) + "%");
                    $("#inputAgentsParticipation").val(100 - FormatMoney(totalParticipation));
                    var currentTo = FormatDate(data.result.CoRequestEndorsement["0"].CurrentTo);
                    $("#CurrentFrom").UifDatepicker('setValue', currentTo);
                    $("#CurrentTo").UifDatepicker('setValue', AddToDate(currentTo, 0, 0, 1));
                    if (data.result.CoRequestEndorsement["0"].IsOpenEffect) {
                        $("#Open").prop("checked", true);
                    }
                    else {
                        $("#Closed").prop("checked", true);
                    }
                    RenewalRequestGroupingRequest.GetBusinessTypeById(data.result.BusinessType).done(function (dataBusinessType) {
                        if (dataBusinessType.success) {
                            $('#selectedBusinessType').text(dataBusinessType.result);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': dataBusinessType.result, 'autoclose': true });
                        }
                    });
                    ObjectCoInsurance.ShowPanelBusinessType(data.result.BusinessType);
                    switch (data.result.BusinessType) {
                        case BusinessType.Accepted:
                            ObjectCoInsurance.LoadCoinsuranceAccepted();
                            break;
                        case  BusinessType.Assigned:
                            ObjectCoInsurance.LoadCoinsuranceAssigned();
                            break;                       
                    }
                    RenewalRequestGrouping.DisabledControls();
                    $("#btnAgentsAccept").prop('disabled', false);
                    $("#btnAgentsSave").prop('disabled', false);
                    $("#btnAgents").prop("disabled", false);
                    $("#btnRequestGroupingSave").prop('disabled', false);


                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });

        }
    }
    ShowAgent() {
        if ($("#inputRequest").data("renewalRequest") != null) {
            $('#listAgencies').UifListView({ sourceData: $("#inputRequest").data("renewalRequest").CoRequestEndorsement[0].CoRequestAgent, displayTemplate: '#agencyTemplate', add: false, edit: true, delete: true, customEdit: true, customDelete: true, height: 250 });
            RenewalRequestGrouping.ShowPanel(MenuTypeGrouping.AGENTS)
        }
    }
    ShowCoInsurance() {
        if ($("#inputRequest").data("renewalRequest") != null) {
            $("#selectCoInsBusinessType").UifSelect("setSelected", $("#inputRequest").data("renewalRequest").BusinessType);          
            ObjectCoInsurance.LoadCoinsurance();
            RenewalRequestGrouping.ShowPanel(MenuType.CoInsurance)
            
        }


    }
    static ShowPanel(Menu) {
        switch (Menu) {
            case MenuTypeGrouping.REQUESTGROUPING:
                break;
            case MenuTypeGrouping.AGENTS:
                $("#modalAgents").UifModal('showLocal', Resources.Language.SelectAgent);
                break;
            case MenuType.CoInsurance:
                $("#modalCoInsurance").UifModal('showLocal', Resources.Language.LabelDataBusinessType);
                break;

        }
    }
    static HidePanels(Menu) {
        switch (Menu) {
            case MenuTypeGrouping.REQUESTGROUPING:
                break;
            case MenuTypeGrouping.AGENTS:
                $('#modalAgents').UifModal('hide');
                break;
            case MenuType.CoInsurance:
                $("#modalCoInsurance").UifModal('hide');
                break;

        }
    }


    Exit() {
        window.location = rootPath + "Home/Index";
    }
    static ClearForm() {
        $("#requestForm").formReset();
        $("#inputBillingGroup").val("");
        $('#inputBillingGroup').data('Object', null);
        $("#inputRequest").val("");
        $('#inputRequest').data('Object', null);
        $("#selectBranch").UifSelect("setSelected", null);
        $("#inputDescription").val("");
        $("#inputAgentPrincipal").val("");
        $("#inputAgentPrincipal").data("Object", null);
        $("#selectBranch").UifSelect({ selectedId: 0 });
        $("#selectMainAgentAgency").UifSelect({ source: null });        
        $("#inputHolder").val("");
        $("#inputHolder").data("Object", null);
        $("#selectPrefixCommercial").UifSelect({ source: null });        
        $("#selectPolicyType").UifSelect({ source: null });        
        $("#selectPaymentPlan").UifSelect({ source: null });        
        $("#selectProduct").UifSelect({ source: null });        
        $("#CurrentFrom").UifDatepicker('setValue', GetCurrentFromDate());
        $("#CurrentTo").UifDatepicker('setValue', AddToDate($("#CurrentFrom").val(), 0, 0, 1));
        $("#CurrentFrom").data("dateFrom", $("#CurrentFrom").val());
        $("#Annotations").val("");
        $("#selectedAgents").text("");
        $("#lblAgentsTotalParticipation").text("");
        $("#inputRequest").data("renewalRequest", null);
        $("#inputRenewalRequest").val("");
        $("#selectedBusinessType").text("");
    }

    static DisabledControls() {
        $("#btnAgentsAccept").prop('disabled', true);
        $("#btnAgentsSave").prop('disabled', true);
        $("#btnRequestGroupingSave").prop('disabled', true);
        $("#selectMainAgentAgency").prop('disabled', true);        
        $("#inputRequest").prop('disabled', true);
        $("#inputDescription").prop('disabled', true);
        $("#inputAgentPrincipal").prop('disabled', true);
        $("#inputHolder").prop('disabled', true);
        $("#selectPrefixCommercial").prop('disabled', true);
        $("#selectProduct").prop('disabled', true);
        $("#selectPolicyType").prop('disabled', true);
        $("#selectBranch").prop('disabled', true);
        $('#CurrentFrom').UifDatepicker('disabled', true);
        $('#Closed').prop('disabled', true);
        $('#Open').prop('disabled', true);
    }
    static HideModal() {
        $('#modalAgents').UifModal('hide');
        $("#modalCoInsurance").UifModal('hide');
    }
    SaveRenewalRequest() {
        if (CompareDates($("#CurrentFrom").val(), $("#CurrentTo").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateFinalDate, 'autoclose': true })           
        }
        else
        {
            $("#btnRequestGroupingSave").prop('disabled', true);
            RenewalRequestGrouping.createRequest();
            RenewalRequestGroupingRequest.SaveRenewalRequest($("#inputRequest").data("renewalRequest")).done(function (data) {
                $("#btnRequestGroupingSave").prop('disabled', false);
                if (data.success) {
                    $('#inputRenewalRequest').val($("#inputRequest").data("renewalRequest").Id)
                    RenewalRequestGrouping.GetRequestByRequestId();                
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageRenewalRequest, 'autoclose': true });

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }
    static createRequest() {
        $("#inputRequest").data("renewalRequest").CoRequestEndorsement[0].CurrentFrom = $("#CurrentFrom").val();
        $("#inputRequest").data("renewalRequest").CoRequestEndorsement[0].CurrentTo = $("#CurrentTo").val();
        $("#inputRequest").data("renewalRequest").CoRequestEndorsement[0].PaymentPlan = { Id: $("#selectPaymentPlan").UifSelect("getSelected") };
        $("#inputRequest").data("renewalRequest").CoRequestEndorsement[0].Annotations = $("#Annotations").val();
        $("#inputRequest").data("renewalRequest").BusinessType=glbRequest.BusinessType;
        $("#inputRequest").data("renewalRequest").InsuranceCompanies=glbRequest.InsuranceCompanies;
    }

    static ClearNewRenewal()
    {
        RenewalRequestGrouping.DisabledControls();
        RenewalRequestGrouping.ClearForm();
    }

}
class RenewalRequestGroupingRequest {
    static GetRequestByRequestId(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetRequestByRequestId',
            data: JSON.stringify({ requestId: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetPolicyTypesByProductId(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetPolicyTypesByProductId',
            data: JSON.stringify({ productId: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetPaymentPlanByProductId(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetPaymentPlanByProductId',
            data: JSON.stringify({ productId: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static SaveRenewalRequest(request) {
        return $.ajax({
            type: 'POST',
            url: 'SaveRenewalRequest',
            data: JSON.stringify({ model: request }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'

        });
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

    static GetBranches() {
        return $.ajax({
            type: 'POST',
            url: 'GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: 'GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetProductByProductId(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetProductByProductId',
            data: JSON.stringify({ productId: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetHoldersById(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetHoldersById',
            data: JSON.stringify({ id: id, searchTypeId: InsuredSearchType.IndividualId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetAgencyByAgentIdByAgencyId(id, agencyId) {
        return $.ajax({
            type: 'POST',
            url: 'GetAgencyByAgentIdByAgencyId',
            data: JSON.stringify({ agentId: id, agencyId: agencyId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

}