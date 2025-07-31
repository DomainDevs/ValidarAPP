/*-------------------------------------------------------------------------------------------------------*/
//          PARA QUE FUNCIONE CORRECTAMENTE CON EL LAUNCHER         
/*-------------------------------------------------------------------------------------------------------*/
var oContractLevelCompany = {
    ContractLevelCompanyId: 0,
    Agent: {},
    Percentage: 0,
    CommissionPercentage: 0,
    ReservePremiumPercentage: 0,
    InterestReserveRelease: 0,
    Company: {},
    ContractLevel: {},
    AdditionalCommission: 0,
    DragLoss: 0,
    ReinsurerExpenditur: 0,
    ProfitSharingPercentage: 0,
    Presentation: 0,
    BrokerCommission: true,
    DifferentialCommissionPercentage: 0
};

var oCompany = {
    IndividualId: 0,
    Name: ""
};

var oContractLevel = {
    ContractLevelId: 0
};

var oAgent = {
    IndividualId: 0,
    FullName: ""

}

function validateSelectedCompany(individualId) {
    if (individualId > 0) {
        return true;
    } else {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.ReinsuredCompanyIsRequired, 'autoclose': true });
        return false;
    }
}

function clearAgent() {
    oAgent.IndividualId = 0;
    oAgent.FullName = "";
    $("#Agent_IndividualId").val("");
}

function clearCompany() {
    oCompany.IndividualId = 0;
    oCompany.Name = "";
    $("#Company_IndividualId").val("");
}

$("#ProportionalDiv").addClass('hidden');

$('#modalContractLevelCompany').on('change', '#inputSearchBroker ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelCompanyId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevelCompany').on('change', '#inputSearchCompany ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelCompanyId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevelCompany').on('change', '#Percentage ', function (event, selectedItem) {
    if (parseInt($("#ContractLevelCompanyId").val()) > 0) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MsjContractModified, 'autoclose': true });
    }
});

$('#modalContractLevelCompany').on('itemSelected', '#inputSearchBroker ', function (event, selectedItem) {
    $("#alertLayerComp").UifAlert('hide');
    if (selectedItem.IndividualId > 0) {
        $.ajax({
            url: REINS_ROOT + "Parameter/GetCompanyIdByContractLevelIdAndIndividualId",
            data: { "contractLevelId": $("#ContractLevelCode").val(), "individualId": selectedItem.IndividualId },
            success: function (dataSearch) {
                oAgent.IndividualId = selectedItem.IndividualId;
                oAgent.FullName = selectedItem.Name;
                $("#Agent_IndividualId").val(selectedItem.IndividualId);
            }
        });
    }
    else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageReinsuranceCompanyNotFound, 'autoclose': true });
        clearAgent();
    }

});

$('#modalContractLevelCompany').on('itemSelected', '#inputSearchCompany ', function (event, selectedItem) {
    $("#alertLayerComp").UifAlert('hide');
    if (selectedItem.IndividualId > 0) {
        $.ajax({
            url: REINS_ROOT + "Parameter/IsReinsurerActive",
            data: { "individualId": selectedItem.IndividualId },
            success: function (data) {
                if (data.success) {
                    if (!data.result) {
                        $.ajax({
                            url: REINS_ROOT + "Parameter/GetCompanyIdByContractLevelIdAndIndividualId",
                            data: { "contractLevelId": $("#ContractLevelCode").val(), "individualId": selectedItem.IndividualId },
                            success: function (dataSearch) {
                                if (dataSearch.result > 0) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.MessageValidateDuplicateReinsuranceCompany, 'autoclose': true });
                                    $('#inputSearchCompany').UifAutoComplete('clean');
                                    clearCompany();
                                }
                                else {
                                    oCompany.IndividualId = selectedItem.IndividualId;
                                    oCompany.Name = selectedItem.Name;
                                    $("#Company_IndividualId").val(selectedItem.IndividualId);
                                }
                            }
                        });
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.ReinsureIsNotActive, 'autoclose': true });
                        $('#inputSearchCompany').UifAutoComplete('clean');
                        clearCompany();
                    }
                }
            }
        });
        
    }
    else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageReinsuranceCompanyNotFound, 'autoclose': true });
        clearCompany();
    }
});

$('#modalContractLevelCompany').on('click', '#btnContractLevelCompany ', function (event, selectedItem) {
    //Grabación de Compañía del Nivel
    if (existAllocation == true) {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateReinsAllocationContract, 'autoclose': true });
        return;
    }

    $("#formContractLevelCompany").validate();

    if ($("#formContractLevelCompany").valid()) {

        if (oCompany.IndividualId == null || oCompany.IndividualId == undefined) {
            oCompany.IndividualId = $("#Company_IndividualId").val();
        }

        oContractLevelCompany.Company = oCompany;

        if (oAgent.IndividualId == null || oAgent.IndividualId == undefined) {
            oAgent.IndividualId = $("#Agent_IndividualId").val();
        }

        oContractLevelCompany.Agent = oAgent;

        oContractLevel.ContractLevelId = $("#ContractLevelCode").val();
        oContractLevelCompany.ContractLevel = oContractLevel;
        oContractLevelCompany.LevelCompanyId = $("#ContractLevelCompanyId").val();
        oContractLevelCompany.GivenPercentage = ReplaceDecimalPoint($("#PercentageAsig").val());
        oContractLevelCompany.ComissionPercentage = ReplaceDecimalPoint($("#CommissionPercentage").val());
        oContractLevelCompany.ReservePremiumPercentage = ReplaceDecimalPoint($("#ReservePremiumPercentage").val());
        oContractLevelCompany.InterestReserveRelease = ReplaceDecimalPoint($("#InterestReserveRelease").val());
        oContractLevelCompany.CompanyIndividual = oCompany.IndividualId;
        oContractLevelCompany.AdditionalCommissionPercentage = ReplaceDecimalPoint($("#AdditionalCommission").val());
        oContractLevelCompany.DragLossPercentage = ReplaceDecimalPoint($("#DragLoss").val());
        oContractLevelCompany.ReinsuranceExpensePercentage = ReplaceDecimalPoint($("#ReinsurerExpenditur").val());
        oContractLevelCompany.UtilitySharePercentage = ReplaceDecimalPoint($("#ProfitSharingPercentage").val());
        oContractLevelCompany.PresentationInformationType = $("#Presentation").val();
        oContractLevelCompany.IntermediaryCommission = ReplaceDecimalPoint($("#BrokerCommission").val());
        oContractLevelCompany.ClaimCommissionPercentage = ReplaceDecimalPoint($("#LossCommissionPercentage").val());
        oContractLevelCompany.DifferentialCommissionPercentage = ReplaceDecimalPoint($("#DifferentialCommissionPercentage").val());

        if (validateSelectedCompany(oContractLevelCompany.Company.IndividualId)) {
            $.ajax({
                async: false,
                type: "POST",
                url: REINS_ROOT + "Parameter/SaveContractLevelCompany",
                data: JSON.stringify({ "levelCompanyDTO": oContractLevelCompany }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    onAddContractLevelCompanyComplete(data);
                    clearCompany();
                    clearAgent();
                }
            });
        }
        
    }
});



