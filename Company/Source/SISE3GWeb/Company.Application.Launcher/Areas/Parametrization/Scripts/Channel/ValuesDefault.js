//principal
var fieldExist = false;
var agentSearchType = 1;
var LaboralPersonalInformation = [];
var valuesDefault = {};

$(() => {
    new ParametrizationValuesDefault();
});

class ValuesDefault {
    static GetPersonTypesInformationPersonal(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Channel/GetMaritalStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectMaritalStatus").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectMaritalStatus").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true  });
                }
            }
        });
    }
    
    static GetCountries(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Channel/GetCountries",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectCountry").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectCountry").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetStatesByCountryId(countryId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Channel/GetStatesByCountryId",
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectState").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectState").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetProducts(agentId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Channel/GetProductsByAgentIdPrefixId",
            data: JSON.stringify({ agentId: agentId, prefixId: 10 }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectProduct").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectProduct").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetPolicyTypesByProductId(productId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetPolicyTypesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectPolicyType").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectPolicyType").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }
    
    static GetLimitsRcByPrefixIdProductIdPolicyTypeId(productId, policyTypeId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetLimitsRcByPrefixIdProductIdPolicyTypeId',
            data: JSON.stringify({ prefixId: 10, productId: productId, policyTypeId: policyTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectLimitRC").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectLimitRC").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetGroupCoveragesProductId(productId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectGroupCoverage").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetCurrenciesByProductId(productId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetCurrenciesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectCurrency").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectCurrency").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetGetBranches(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectBranch").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectBranch").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    //Obtiene las agencias por intermediario 
    static GetAgenciesByAgentIdDescription(agentId, description, selectedAgentAgencyTypeId, selectedProductId) {
        var number = parseInt(description, 10);

        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/Channel/GetAgenciesByAgentIdDescription',
                data: JSON.stringify({ agentId: agentId, description: description}),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        $("#inputAgentPrincipal").data("AgentCode", data.result[0].Code);
                        $("#inputAgentPrincipal").val(data.result[0].Agent.FullName);
                        
                        if (selectedAgentAgencyTypeId == undefined) {
                            ValuesDefault.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, data.result[0].Id);
                        }
                        else {
                            ValuesDefault.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, selectedAgentAgencyTypeId);
                        }

                        ValuesDefault.GetProducts(data.result[0].Agent.IndividualId, selectedProductId);
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

    //Agrega las agencias en el select
    static GetAgenciesByAgentId(agentId, selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetAgenciesByAgentId',
            data: JSON.stringify({ agentId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectAgentAgency").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectAgentAgency").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }
    
    static GetUses(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetUses',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectUse").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectUse").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetColors(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetColors',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectColor").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectColor").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetBodies(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetBodies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectBodyType").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectBodyType").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetWorkerTypes(selectedId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Channel/GetWorkerTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == undefined) {
					$("#selectWorkerType").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                else {
					$("#selectWorkerType").UifSelect({ sourceData: data.result, selectedId: selectedId, native: false, filter: true });
                }
            }
        });
    }

    static GetUserById(userId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Channel/GetUserById',
            data: JSON.stringify({ userId: userId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

//StaffLabour
class ParametrizationValuesDefault extends Uif2.Page {
    getInitialState() {
		 $('#inputUser').UifAutoComplete({
            source: rootPath + "UniqueUser/UniqueUser/GetUsersByQuery",
            displayKey: "AccountName",
            queryParameter: "&query"
        });
    }

    bindEvents() {
        $("#btnAcceptValuesDefault").click(this.AcceptValuesDefault);
        $("#btnValuesDefault").click(this.showValuesDefault);
        $("#inputUser").on("itemSelected", this.assignUserId);
        $("#selectCountry").on('itemSelected', this.GetStatesByCountryId);
        $("#selectProduct").on('itemSelected', this.GetPolicyTypesByProductId);
        $("#selectPolicyType").on('itemSelected', this.GetLimitsRcByPrefixIdProductIdPolicyTypeId);
        $("#selectProduct").on('itemSelected', this.GetGroupCoveragesProductId);
        $("#selectProduct").on('itemSelected', this.GetCurrenciesByProductId);
        $("#inputAgentPrincipal").on('buttonClick', () => { this.InputAgentPrincipalSearch(); });
    }

    AcceptValuesDefault() {
        if (ParametrizationValuesDefault.ValidateValuesDefault()) {
            ParametrizationValuesDefault.getValuesDefault();
        }
    }

    GetStatesByCountryId() {
        ValuesDefault.GetStatesByCountryId($("#selectCountry").val());
    }

    GetPolicyTypesByProductId() {
        ValuesDefault.GetPolicyTypesByProductId($("#selectProduct").val());
    }

    GetLimitsRcByPrefixIdProductIdPolicyTypeId() {
        ValuesDefault.GetLimitsRcByPrefixIdProductIdPolicyTypeId($("#selectProduct").val(), $("#selectPolicyType").val());
    }

    GetGroupCoveragesProductId() {
        ValuesDefault.GetGroupCoveragesProductId($("#selectProduct").val());
    }

    GetCurrenciesByProductId() {
        ValuesDefault.GetCurrenciesByProductId($("#selectProduct").val());
    }
    
    static getValuesDefault() {
        valuesDefault.ProspectName = $('#inputName').val();
        valuesDefault.ProspectSurname = $('#inputLastName').val();
        valuesDefault.LicensePlate = $('#inputLicencesePlate').val();
        valuesDefault.CountryCode = $('#selectCountry').val();
        valuesDefault.StateCode = $('#selectState').val();
        valuesDefault.MaritalStatus = $('#selectMaritalStatus').val();
        valuesDefault.ProductId = $('#selectProduct').val();
        valuesDefault.PolicyTypeCode = $('#selectPolicyType').val();
        valuesDefault.LimitRcCode = $('#selectLimitRC').val();
        valuesDefault.CoverGroupId = $('#selectGroupCoverage').val();
        valuesDefault.CurrencyCode = $('#selectCurrency').val();
        valuesDefault.BranchCode = $('#selectBranch').val();
        valuesDefault.VehicleUseCode = $('#selectUse').val();
        valuesDefault.VehicleColorCode = $('#selectColor').val();
        valuesDefault.VehicleBodyCode = $('#selectBodyType').val();
        valuesDefault.WorkerTypeId = $('#selectWorkerType').val();
        valuesDefault.AgentCode = $("#inputAgentPrincipal").data().AgentCode;
        valuesDefault.AgentAgencyTypeCode = $('#selectAgentAgency').val();
        valuesDefault.AccountName = $('#inputUser').data().UserId;
        $("#modalValuesDefault").modal("toggle");
    }

    assignUserId(event, selectedItem) {
        if (selectedItem != null) {
            $("#inputUser").data("UserId", selectedItem.UserId);
        }
        else {
            $("#inputUser").data("UserId", null);
        }
    }
    
    showValuesDefault() {
        ParametrizationValuesDefault.ClearValuesDefault();
        ParametrizationValuesDefault.selectValuesDefault();

        $("#modalValuesDefault").UifModal('showLocal', Resources.Language.DefaultValues);
    }

    static selectValuesDefault() {
        $("#inputName").val(valuesDefault.ProspectName);
        $("#inputLastName").val(valuesDefault.ProspectSurname);
        $("#inputLicencesePlate").val(valuesDefault.LicensePlate);
        ValuesDefault.GetCountries(valuesDefault.CountryCode);
        ValuesDefault.GetPersonTypesInformationPersonal(valuesDefault.MaritalStatus);
        ValuesDefault.GetUses(valuesDefault.VehicleUseCode);
        ValuesDefault.GetColors(valuesDefault.VehicleColorCode);
        ValuesDefault.GetBodies(valuesDefault.VehicleBodyCode);
        ValuesDefault.GetWorkerTypes(valuesDefault.WorkerTypeId);
        ValuesDefault.GetGetBranches(valuesDefault.BranchCode);

        if (!$.isEmptyObject(valuesDefault)) {
            ValuesDefault.GetStatesByCountryId(valuesDefault.CountryCode, valuesDefault.StateCode);
            ValuesDefault.GetAgenciesByAgentIdDescription(0, valuesDefault.AgentCode, valuesDefault.AgentAgencyTypeCode, valuesDefault.ProductId);
            ValuesDefault.GetPolicyTypesByProductId(valuesDefault.ProductId, valuesDefault.PolicyTypeCode);
            ValuesDefault.GetLimitsRcByPrefixIdProductIdPolicyTypeId(valuesDefault.ProductId, valuesDefault.PolicyTypeCode, valuesDefault.LimitRcCode);
            ValuesDefault.GetGroupCoveragesProductId(valuesDefault.ProductId, valuesDefault.CoverGroupId);
            ValuesDefault.GetCurrenciesByProductId(valuesDefault.ProductId, valuesDefault.CurrencyCode);
            ValuesDefault.GetUserById(valuesDefault.AccountName).done(function (data) {
                if (data.success) {
					$("#inputUser").UifAutoComplete("setValue", data.result.AccountName);
                    $("#inputUser").data("UserId", data.result.UserId);
                }
            });
        }

    }

    InputAgentPrincipalSearch() {
        agentSearchType = 1;
        ValuesDefault.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim());
    }

    static ClearValuesDefault() {
        $('#inputName').val("");
        $('#inputLastName').val("");
        $('#inputLicencesePlate').val("");
        $('#selectCountry').val("");
        $('#selectMaritalStatus').val("");
        $('#selectBranch').val("");
        $('#selectUse').val("");
        $('#selectColor').val("");
        $('#selectBodyType').val("");
        $('#selectWorkerType').val("");
        $('#selectState').html("").prop('disabled', true);;
        $('#selectProduct').html("").prop('disabled', true);;
        $('#selectPolicyType').html("").prop('disabled', true);;
        $('#selectLimitRC').html("").prop('disabled', true);;
        $('#selectGroupCoverage').html("").prop('disabled', true);;
        $('#selectCurrency').html("").prop('disabled', true);;
        $('#selectAgentAgency').html("").prop('disabled', true);;
        $('#inputAgentPrincipal').val("");
        $("#inputAgentPrincipal").data("AgentCode", null);
		$('#inputUser').UifAutoComplete('setValue', "");
        $("#inputUser").data("inputUser", null);
    }
    
    static ValidateValuesDefault() {
        var error = "";
        if ($("#inputName").val() == "" || $("#inputName").val() <= 0) {
            error = error + "Nombre <br>";
        }
        if ($("#inputLastName").val() == "" || $("#inputLastName").val() <= 0) {
            error = error + "Apellido <br>";
        }
        if ($("#selectMaritalStatus").val() <= 0)
            error = error + "Estado Civil <br>";
        if ($("#selectCountry").val() <= 0)
            error = error + "Pais <br>";
        if ($("#selectState").val() <= 0)
            error = error + "Departamento <br>";
        if ($("#inputAgentPrincipal").val() <= "")
            error = error + "Intermediario Principal <br>";
        if ($("#selectAgentAgency").val() <= 0)
            error = error + "Agencia Intermediario <br>";
        if ($("#selectProduct").val() <= 0)
            error = error + "Producto <br>";
        if ($("#selectPolicyType").val() <= 0)
            error = error + "Tipo de Poliza <br>";
        if ($("#selectCurrency").val() <= -1)
            error = error + "Moneda <br>";
        if ($("#selectLimitRC").val() <= 0)
            error = error + "Limite de RC <br>";
        if ($("#selectGroupCoverage").val() <= 0)
            error = error + "Grupo de Cobertura <br>";
        if ($("#selectBranch").val() <= 0)
            error = error + "Sucursal <br>";
        if ($("#inputUser").val() <= "")
            error = error + "Usuario <br>";
        if ($("#inputLicencesePlate").val() == "" || $("#inputLicencesePlate").val() <= 0)
            error = error + "Placa <br>";
        if ($("#selectBodyType").val() <= 0)
            error = error + "Carrocería <br>";
        if ($("#selectUse").val() <= 0)
            error = error + "Uso <br>";
        if ($("#selectColor").val() <= 0)
            error = error + "Color <br>";
        if ($("#selectWorkerType").val() <= 0)
            error = error + "Tipo de Trabajador <br>";

        if (error != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.LabelInformative + "<br>" + error, 'autoclose': true });
            return false;
        } else {
            return true;
        }
    }
}