var AutomaticQuotaId = 0;
var utilitys = [];
var SummaryUtilitys = [];
var indicatortypeliquidity = [];
var indicatortypeindebtedness = [];
var indicatortypecosteffectiveness = [];
var indicatortypeActivity = [];
var glbAutomaticQuota;
var DateIni;
var RiskCenterDTO = {};
var RestrictiveDTO = {};
var PromissoryNoteSignatureDTO = {};
var ReportListSisconcDTO = {};
var utilityDTO = [];
var indicatorDTO = [];
var AgentProgramDTO = {};
var UtilityDetails = {};
//var Agent = {}; 
var DateFin;
var Script = "CUPO OPERATIVO CALIFICACIONES";
var AutomaticQuotaController;
var dynamicProperties = [];
var ProspectDTOS = {};
var EditData;
var fecha = new Date();
var glbThird;
var RestrictivePolicies = 0;
var updateInd;
var calc = 0;
var objectAgent = {};
class AutomaticQuota extends Uif2.Page {

    getInitialState() {
        AutomaticQuota.InitialAutomaticQuota();
        AutomaticQuotaController = 'AutomaticQuotaController';
        if (glbAutomaticQuota == null) {
            glbAutomaticQuota = { Id: 0, Object: 'AutomaticQuota', formAutomaticQuota: "#formAutomaticQuota", RecordScript: false, Class: AutomaticQuota, redirectData: false, CustomerType: 0, IndividualId: 0 };
        }
        glbAutomaticQuota.redirectData = false;
    }

    bindEvents() {
        $('#btnThird').on('click', AutomaticQuota.showThird);
        $('#btnUtility').on('click', AutomaticQuota.showUtility);
        $('#btnIndicators').on('click', AutomaticQuota.showIndicators);
        $('#tableUtilitys').on('rowEdit', AutomaticQuota.EditUtilitys);
        $('#tableSummaryUtilitys').on('rowEdit', AutomaticQuota.EditUtilitys);
        $('#tableLiquidity').on('rowEdit', AutomaticQuota.EditIndicator);
        $('#tableIndebtedness').on('rowEdit', AutomaticQuota.EditIndicator);
        $('#tableCosteffectiveness').on('rowEdit', AutomaticQuota.EditIndicator);
        $('#tableActivity').on('rowEdit', AutomaticQuota.EditIndicator);
        $("#InputAgent").on('buttonClick', AutomaticQuota.SearchAgentPrincipal);
        $('#tableResults tbody').on('click', 'tr', AutomaticQuota.SelectResults);
        $('#InputCiiuEconomicActivity').on('buttonClick', AutomaticQuota.SearchEconomyActivity);
        $('#InputCity').on("search", AutomaticQuota.SearchCity);
        $('#btnEditIndicatorsave').on('click', AutomaticQuota.AddtableIndicator);
        $('#SelectCountry').on("itemSelected", AutomaticQuota.GetStatesByCountryId);
        $('#SelectDepartment').on("itemSelected", AutomaticQuota.GetCitiesByCountryIdStateId);
        $("#btnCustomerKnowledge").on('click', AutomaticQuota.LoadScript);
        $('#btnEditUtilitySave').on('click', AutomaticQuota.SaveUtilitys);
        $("#btnSaveAutomaticQuota").click(AutomaticQuota.RecordAutomaticQuota);
        $("#btnThirdSave").click(AutomaticQuota.SaveAutomaticThirdJSON);
        $("#btnUtilitySave").click(AutomaticQuota.SaveAutomaticUtilitiesJSON);
        $("#btnIndicatorSave").click(AutomaticQuota.SaveAutomaticIndicatorJSON);
        //$("#inputTemporalAutomaticQuota").on('buttonClick', AutomaticQuota.GetAutomaticQuota);
        $("#inputTemporalAutomaticQuota").on('buttonClick', AutomaticQuota.GetAutomaticQuotaOperation);
        $("#btnShowAdvancedAq").on("click", AutomaticQuota.ShowAdvanced);
        $("#InputNIdentification").on("search", AutomaticQuota.GetPersonByDescriptionInsuredSearchTypeCustomerType);
        $("#InputForSolution").on("search", AutomaticQuota.GetUserByName);
        $("#InputDepartament").on("search", AutomaticQuota.SearchState);
        $('#InputCountry').on("search", AutomaticQuota.SearchCountry);
        $("#InputMainDebtor").focusin(AutomaticQuota.SumTotal);
        $("#InputMainDebtor").focusout(AutomaticQuota.SumTotal);
        $("#InputCodebtor").focusin(AutomaticQuota.SumTotal);
        $("#InputCodebtor").focusout(AutomaticQuota.SumTotal);
        $("#btnCalculate").click(AutomaticQuota.LoadScript);
        $("#btnCloseAutomaticQuota").click(AutomaticQuota.Close);
        $("#InputReconsiderationQuota").focusin(AutomaticQuota.QuotaReconsideration);
        $("#InputReconsiderationQuota").focusout(AutomaticQuota.QuotaReconsideration);
    }

    static InitialAutomaticQuota() {
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        AutomaticQuota.GetAgentProgram();
        AutomaticQuota.GetDocumentTypes();
        AutomaticQuota.GetGuaranteeStatus();
        AutomaticQuota.GetGuaranteesTypes();
        AutomaticQuota.GetdefaultValueCountry();
        AutomaticQuota.GetCountries();
        AutomaticQuota.GetUserSession();
        AutomaticQuota.GetDefaultIndicator();
        $("#DateIniUtilitys").text(DateIni);
        $("#DateFinUtilitys").text(DateFin);
        $("#DateIniSummaryUtilitys").text(DateIni);
        $("#DateFinSummaryUtilitys").text(DateFin);
        $("#DateIniLiquidity").text(DateIni);
        $("#DateFinLiquidity").text(DateFin);
        $("#DateIniIndebtedness").text(DateIni);
        $("#DateFinIndebtedness").text(DateFin);
        $("#DateIniCosteffectiveness").text(DateIni);
        $("#DateFinCosteffectiveness").text(DateFin);
        $("#DateIniActivity").text(DateIni);
        $("#DateFinActivity").text(DateFin);
        $("#DateIniEditTableUtilitys").text(DateIni);
        $("#DateFinEditTableUtilitys").text(DateFin);
        $("#DateIniEditTableIndicator").text(DateIni);
        $("#inputTemporalAutomaticQuota").ValidatorKey(ValidatorType.Number, 2, 0);
        $('#InputNIdentification').ValidatorKey(ValidatorType.Number, 2, 0);
        $("#InputSuggestedQuota").val('0');
        $("#InputLegalQuota").val('0');
        $("#InputCurrentQuota").val('0');
        $("#InputCurrentCumulu").val('0');
        $("#InputReconsiderationQuota").val('0');
        $("#InputElaborationQuotaDat").val(fecha.toLocaleDateString());
        $("#InputMainDebtor").val('0');
        $("#InputCodebtor").val('0');
        //Terceros
        AutomaticQuota.GetReportListSisconc();
        AutomaticQuota.GetRiskCenterList();
        AutomaticQuota.GetRestrictiveList();
        AutomaticQuota.GetPromissoryNoteSignature();
    }

    static showThird() {
        $("#formAutomaticQuota").validate();
        if ($("#formAutomaticQuota").valid()) {
            $('#modalAutomaticQuotaThid').UifModal('showLocal', Resources.Language.Third);
            if (glbThird == null || glbThird.Id == 0) {
                AutomaticQuota.SaveAutomaticGeneralJSON();
            } else {
                $("#SelectconsultRiskCenters").UifSelect("setSelected", glbThird.RiskCenterDTO.Id);
                $("#SelectRestrictiveListQuery").UifSelect("setSelected", glbThird.RestrictiveDTO.Id);
                $("#SelectSignaturePromissoryNoteLetterIntent").UifSelect("setSelected", glbThird.PromissoryNoteSignatureDTO.Id);
                $("#SelectSisconcReport").UifSelect("setSelected", glbThird.ReportListSisconcDTO.Id);
                $("#InputQueryCifin").val(FormatDate(glbThird.CifinQuery));
                $("#InputMainDebtor").val(glbThird.PrincipalDebtor);
                $("#InputCodebtor").val(glbThird.Cosigner);
                $("#InputTotal").val(glbThird.Total);
            }
        }
    }
    static showUtility() {
        $("#formAutomaticQuota").validate();
        if (RestrictivePolicies == 0) {
            if ($("#formAutomaticQuota").valid()) {
                if (glbThird != null) {
                    $('#modalAutomaticQuotaUtility').UifModal('showLocal', Resources.Language.Utility);
                    $('#EditTableUtilitys').hide();
                    if (utilitys.length == 0 && SummaryUtilitys.length == 0) {
                        AutomaticQuotaRequest.GetUtility().done(function (data) {
                            if (data.success) {
                                if (data.result) {
                                    data.result.forEach(function (item, index) {
                                        if (item.FormUtilitys == 1) {
                                            utilitys.push({
                                                Id: item.Id,
                                                Description: item.Description,
                                                DateIni: 0,
                                                DateFin: 0,
                                                AbsoludValue: 0,
                                                RelativeValue: 0 + '%',
                                                UtilityType: item.UtilitysTypeCd,
                                                UtilitySummary: item.UtilitysSummaryCd,
                                                FormUtilitys: item.FormUtilitys,
                                                utilityId: item.UtilityId
                                            });
                                        }
                                        if (item.FormUtilitys == 2) {
                                            SummaryUtilitys.push({
                                                Id: item.Id,
                                                Description: item.Description,
                                                DateIni: 0,
                                                DateFin: 0,
                                                AbsoludValue: 0,
                                                RelativeValue: 0 + '%',
                                                UtilityType: item.UtilitysTypeCd,
                                                UtilitySummary: item.UtilitysSummaryCd,
                                                FormUtilitys: item.FormUtilitys,
                                                utilityId: item.UtilityId
                                            });
                                        }
                                    });
                                    $('#tableUtilitys').UifDataTable('clear');
                                    $('#tableSummaryUtilitys').UifDataTable('clear');
                                    $('#tableUtilitys').UifDataTable('addRow', utilitys);
                                    $('#tableSummaryUtilitys').UifDataTable('addRow', SummaryUtilitys);
                                }
                            }
                        });
                    } else {
                        $('#tableUtilitys').UifDataTable('clear');
                        $('#tableSummaryUtilitys').UifDataTable('clear');
                        $('#tableUtilitys').UifDataTable('addRow', utilitys);
                        $('#tableSummaryUtilitys').UifDataTable('addRow', SummaryUtilitys);
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResources.ThirdPartySectionFirst });
                }
            }
        }
        else {
            AutomaticQuota.ValidateAuthorizationPoliciesThird(glbThird, true);
        }

    }
    static showIndicators() {
        if (RestrictivePolicies == 0) {
            $("#formAutomaticQuota").validate();
            if ($("#formAutomaticQuota").valid()) {
                $('#EditTableIndicator').hide();
                if (glbThird != null) {
                    if (utilitys.length != 0 && SummaryUtilitys.length != 0) {
                        $('#modalAutomaticQuotaIndicator').UifModal('showLocal', Resources.Language.Indicators);
                        if (indicatortypeliquidity.length == 0 && indicatortypeindebtedness.length == 0 && indicatortypecosteffectiveness.length == 0 && indicatortypeActivity.length == 0) {
                            AutomaticQuotaRequest.GetIndicatorConcepts().done(function (data) {
                                if (data.success) {
                                    if (data.result) {
                                        data.result.forEach(function (item) {
                                            if (item.IndicatorType == 1) {
                                                indicatortypeliquidity.push({
                                                    Id: item.Id,
                                                    Description: item.Description,
                                                    DateIni: 0,
                                                    DateFin: 0,
                                                    Observations: 0,
                                                    IndicatorType: item.IndicatorType
                                                });
                                            }
                                            if (item.IndicatorType == 2) {
                                                indicatortypeindebtedness.push({
                                                    Id: item.Id,
                                                    Description: item.Description,
                                                    DateIni: 0,
                                                    DateFin: 0,
                                                    Observations: 0,
                                                    IndicatorType: item.IndicatorType
                                                });
                                            }
                                            if (item.IndicatorType == 3) {
                                                indicatortypecosteffectiveness.push({
                                                    Id: item.Id,
                                                    Description: item.Description,
                                                    DateIni: 0,
                                                    DateFin: 0,
                                                    Observations: 0,
                                                    IndicatorType: item.IndicatorType
                                                });
                                            }
                                            if (item.IndicatorType == 4) {
                                                indicatortypeActivity.push({
                                                    Id: item.Id,
                                                    Description: item.Description,
                                                    DateIni: 0,
                                                    DateFin: 0,
                                                    Observations: 0,
                                                    IndicatorType: item.IndicatorType
                                                });
                                            }
                                        });
                                        $('#tableLiquidity').UifDataTable('clear');
                                        $('#tableIndebtedness').UifDataTable('clear');
                                        $('#tableCosteffectiveness').UifDataTable('clear');
                                        $('#tableActivity').UifDataTable('clear');
                                        $('#tableLiquidity').UifDataTable('addRow', indicatortypeliquidity);
                                        $('#tableIndebtedness').UifDataTable('addRow', indicatortypeindebtedness);
                                        $('#tableCosteffectiveness').UifDataTable('addRow', indicatortypecosteffectiveness);
                                        $('#tableActivity').UifDataTable('addRow', indicatortypeActivity);
                                    }
                                }
                            });
                        } else {
                            $('#tableLiquidity').UifDataTable('clear');
                            $('#tableIndebtedness').UifDataTable('clear');
                            $('#tableCosteffectiveness').UifDataTable('clear');
                            $('#tableActivity').UifDataTable('clear');
                            $('#tableLiquidity').UifDataTable('addRow', indicatortypeliquidity);
                            $('#tableIndebtedness').UifDataTable('addRow', indicatortypeindebtedness);
                            $('#tableCosteffectiveness').UifDataTable('addRow', indicatortypecosteffectiveness);
                            $('#tableActivity').UifDataTable('addRow', indicatortypeActivity);
                        }
                    } else {
                        $.UifDialog('alert', { 'message': AppResources.ThirdPartyandUtilitiessection });
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResources.ThirdPartySectionFirst });
                }
            }
        } else {
            AutomaticQuota.ValidateAuthorizationPoliciesThird(glbThird, true);
        }

    }
    static EditUtilitys(event, data, position) {
        if (data.Id != 7 && data.Id != 13 && data.Id != 20 && data.Id != 22 && data.Id != 24 && data.Id != 26) {
            $('#EditTableUtilitys').show();
            $('#InputUtilitysConcept').text(data.Description);
            $('#InputUtilitysDateIni').val(data.DateIni);
            $('#InputUtilitysDateEnd').val(data.DateFin);
            EditData = data;
        }
    }
    static EditIndicator(event, data, position) {
        $('#EditTableIndicator').show();
        //$('#ButtonEditTableIndicator').show();
        $('#InputIndicatorConcept').text(data.Description);
        //$('#InputIndicatorDateIni').val(data.DateIni);
        $('#InputIndicatorDateEnd').val(data.DateFin);
        $('#InputIndicatorAbsoludValue').val(data.AbsoludValue);
        $('#InputIndicatorRelativeValue').val(data.RelativeValue);
        EditData = data;
    }
    static GetReportListSisconc() {
        AutomaticQuotaRequest.GetReportListSisconc().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectSisconcReport").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetRiskCenterList() {
        AutomaticQuotaRequest.GetRiskCenterList().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectconsultRiskCenters").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetRestrictiveList() {
        AutomaticQuotaRequest.GetRestrictiveList().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectRestrictiveListQuery").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetPromissoryNoteSignature() {
        AutomaticQuotaRequest.GetPromissoryNoteSignature().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectSignaturePromissoryNoteLetterIntent").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetDocumentTypes() {
        AutomaticQuotaRequest.GetDocumentTypes().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectDocumentType").UifSelect({ sourceData: data.result.DocumentTypeServiceModel });
                }
            }
        });
    }
    static GetAgentProgram() {
        AutomaticQuotaRequest.GetAgentProgram().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectProgram").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetGuaranteeStatus() {
        AutomaticQuotaRequest.GetGuaranteeStatus().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectCounterGuaranteeState").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetGuaranteesTypes() {
        AutomaticQuotaRequest.GetGuaranteesTypes().done(function (data) {
            if (data.success) {
                if (data.result) {
                    data.result.forEach(function (item, index) {
                        item.Id = item.GuaranteeTypeId;
                    })
                    $("#SelectCounterGuaranteeType").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static SearchAgentPrincipal() {
        var description = $("#InputAgent").val().trim();
        AutomaticQuota.GetAgenciesByAgentIdDescription(0, description)

    }
    static GetAgenciesByAgentIdDescription(agentId, description) {
        AutomaticQuotaRequest.GetUserAgenciesByAgentIdDescription(agentId, description).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.length == 1) {
                        $("#InputAgent").data("Object", data.result[0]);
                        $("#InputAgent").val(data.result[0].FullName);
                    } else if (data.result.length > 1) {
                        modalListType = 1;
                        var dataList = [];

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].Agent.IndividualId,
                                Code: data.result[i].Code,
                                Description: data.result[i].Agent.FullName
                            });
                        }
                        AutomaticQuota.ShowDefaultResults(dataList);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelAgentPrincipal);
                    } else {
                        if (agentSearchType == 1) {
                            $("#InputAgent").data("Object", null);
                            $("#InputAgent").val('');
                        }
                        else if (agentSearchType == 2) {
                            $("#InputAgent").data("Object", null);
                            $("#InputAgent").val('');
                        }
                        $('#selectAgentAgency').UifSelect({ source: null });
                        showInfoToast(AppResources.MessageSearchAgents);
                    }
                }
            }
        });
    }
    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }
    static SelectResults(e) {
        switch (modalListType) {
            case 1:
                AutomaticQuota.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
                break;
            case 2:
                $('#InputCiiuEconomicActivity').val($(this).children()[2].innerHTML + ' (' + $(this).children()[0].innerHTML + ')');
                $("#InputCiiuEconomicActivity").data("Id", $(this).children()[0].innerHTML);
                break;
            case 3:
                $('#InputForSolution').val($(this).children()[2].innerHTML + ' (' + $(this).children()[0].innerHTML + ')');
                $("#InputForSolution").data("Id", $(this).children()[0].innerHTML);
                break;
            case 4:
                var table = $('#tableResults').UifDataTable('getData');
                var IndividualId = $(this).children()[0].innerHTML;

                table.forEach(function (item, index) {
                    glbAutomaticQuota.CustomerType = item.CustomerType;
                    if (item.Id == parseInt(IndividualId)) {
                        if (item.AssociationType.Id == 1) {
                            if (item.Description != "") {
                                $("#InputNameBusinnesName").val(item.Description);
                                $("#InputNameBusinnesName").data("Id", item.Id);
                                $("#InputNameBusinnesName").prop("disabled", true);
                            } else {
                                $("#InputNameBusinnesName").prop("disabled", false);
                            }
                            $("#InputNIdentification").val(parseInt(item.DocumentNum));
                            if (item.DocumentTypeId != 0) {
                                $('#SelectDocumentType').UifSelect("setSelected", item.DocumentTypeId);
                                $("#SelectDocumentType").prop("disabled", true);
                                glbAutomaticQuota.IndividualId = item.Id;
                                glbAutomaticQuota.CustomerTpe = item.CustomerTpeId;
                            } else {
                                $("#SelectDocumentType").prop("disabled", false);
                            }
                            if (item.EconomicActivity != null) {
                                $("#InputCiiuEconomicActivity").val(item.EconomicActivity);
                                AutomaticQuota.GetEconomyActivity(item.EconomicActivity.Id);
                                $("#InputCiiuEconomicActivity").prop("disabled", false);
                            } else {
                                $("#InputCiiuEconomicActivity").prop("disabled", false);
                            }
                            if (item.CompanyName != null) {
                                if (item.CompanyName.Email != null) {
                                    $("#InputEmail").val(item.CompanyName.Email.Description);
                                    $("#InputEmail").prop("disabled", true);
                                } else {
                                    $("#InputEmail").prop("disabled", false);
                                }
                                if (item.CompanyName.Phone != null) {
                                    $("#InputPhone").val(item.CompanyName.Phone.Description);
                                    $("#InputPhone").prop("disabled", true);
                                } else {
                                    $("#InputPhone").prop("disabled", false);
                                }
                                if (item.CompanyName.Address != null) {
                                    $("#InputFullAddress").val(item.CompanyName.Address.Description);
                                    $("#InputFullAddress").prop("disabled", true);
                                } else {
                                    $("#InputFullAddress").prop("disabled", false);
                                }
                            }
                            if (item.Id != null) {
                                AutomaticQuota.GetAddressByIndividualIdCompany(item.Id);
                                AutomaticQuota.GetAgencyByIndividualId(item.Id);
                                AutomaticQuota.GetOperatingQuotaEventByIndividualIdByLineBusinessId(item.Id);
                            }
                        } else {
                            $.UifDialog('alert', { 'message': AppResourcesPerson.MessageOperationQuotaConsortium });
                            $("#InputNIdentification").val('');
                        }
                        if (item.CustomerType == 1) {
                            $("#SelectDocumentType").prop("disabled", true);
                            $("#InputNameBusinnesName").prop("disabled", true);
                            $("#InputCountry").prop("disabled", true);
                            $("#InputDepartament").prop("disabled", true);
                            $("#InputCity").prop("disabled", true);
                            $("#InputFullAddress").prop("disabled", true);
                            $("#InputPhone").prop("disabled", true);
                            $("#InputEmail").prop("disabled", true);
                            $("#InputCiiuEconomicActivity").prop("disabled", true);
                            $("#InputAgent").prop('disabled', true);
                        } else if (item.CustomerType == 2) {
                            $("#SelectDocumentType").prop("disabled", false);
                            $("#InputNameBusinnesName").prop("disabled", false);
                            $("#InputCountry").prop("disabled", false);
                            $("#InputDepartament").prop("disabled", false);
                            $("#InputCity").prop("disabled", false);
                            $("#InputFullAddress").prop("disabled", false);
                            $("#InputPhone").prop("disabled", false);
                            $("#InputEmail").prop("disabled", false);
                            $("#InputCiiuEconomicActivity").prop("disabled", false);
                            $("#InputAgent").prop('disabled', false);
                        }
                    }
                });
                break;
            case 5:
                $('#InputCountry').val($(this).children()[2].innerHTML);
                $("#InputCountry").data("Id", $(this).children()[0].innerHTML);
                break;
            case 6:
                $('#InputDepartament').val($(this).children()[2].innerHTML);
                $("#InputDepartament").data("Id", $(this).children()[0].innerHTML);
                break;
            case 7:
                $('#InputCity').val($(this).children()[2].innerHTML);
                $("#InputCity").data("Id", $(this).children()[0].innerHTML);
                break;
            default:
                break;
        }
        $('#modalDefaultSearch').UifModal("hide");
    }
    static SearchEconomyActivity() {
        if ($('#InputCiiuEconomicActivity').val().trim() != "" && $('#InputCiiuEconomicActivity').val().trim() != 0) {
            AutomaticQuota.GetEconomyActivity($('#InputCiiuEconomicActivity').val().trim());
        }
    }
    static GetEconomyActivity(description) {
        AutomaticQuotaRequest.GetEconomicActivitiesByDescription(description).done(function (data) {
            if (data.success && data.result.length > 0) {
                if (data.result.length == 1) {
                    $("#InputCiiuEconomicActivity").val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                    $("#InputCiiuEconomicActivity").data("Id", data.result[0].Id)
                    $("#InputCiiuEconomicActivity").prop("disabled", true);
                } else if (data.result.length == 0) {
                    $("#InputCiiuEconomicActivity").prop("disabled", false);
                }
                else {
                    modalListType = 2;
                    var dataList = {
                        dataObject: []
                    };

                    for (var i = 0; i < data.result.length; i++) {
                        dataList.dataObject.push({
                            Id: data.result[i].Id,
                            Code: data.result[i].Id,
                            Description: data.result[i].Description
                        });
                    }
                    AutomaticQuota.ShowModalList(dataList.dataObject);
                    $('#modalDefaultSearch').UifModal('showLocal', AppResourcesPerson.SelectEconomicActivity);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $("#InputCiiuEconomicActivity").val('');
                $("#InputCiiuEconomicActivity").focus();
            }
        });
    }
    static ShowModalList(dataTable) {
        $('#tableResults').UifDataTable('clear');
        var table = $('#tableResults').DataTable();
        table.on('draw', function () {
            $('#tableResults tbody td:nth-child(1)').hide();
            $('#tableResults thead th:eq(0)').hide();
        });
        $('#tableResults').UifDataTable('addRow', dataTable);
        dataList = [];
    }
    static SearchCountry(event, value) {
        $('#InputDepartament').val("");
        $('#InputCity').val("");
        if ($("#InputCountry").val().trim().length > 0) {
            AutomaticQuota.GetCountriesByDescription($("#InputCountry").val());
        }
    }

    static SearchState(event, value) {
        $('#InputCity').val("");
        if ($("#InputDepartament").val().trim().length > 0) {
            AutomaticQuota.GetStatesByCountryIdByDescription($("#InputCountry").data('Id'), $("#InputDepartament").val());
        }
    }
    static SearchCity(event, value) {
        if ($("#InputCity").val().trim().length > 0) {
            AutomaticQuota.GetCitiesByCountryIdByStateIdByDescription($("#InputCountry").data('Id'), $("#InputDepartament").data('Id'), $("#InputCity").val());
        }
    }
    static AddtableIndicator() {
        var IndicatorSelected = {
            Id: EditData.Id,
            Description: EditData.Description,
            DateIni: EditData.DateIni,
            DateFin: EditData.DateFin,
            IndicatorType: EditData.IndicatorType,
            Observations: $("#InputObservations").val()
        };

        if (EditData.IndicatorType == 1) {
            var indexOfEstimation = $("#tableLiquidity").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(EditData.Id);
            $("#tableLiquidity").UifDataTable('editRow', IndicatorSelected, indexOfEstimation);
        } else if (EditData.IndicatorType == 2) {
            var indexOfEstimation = $("#tableIndebtedness").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(EditData.Id);
            $("#tableIndebtedness").UifDataTable('editRow', IndicatorSelected, indexOfEstimation);
        } else if (EditData.IndicatorType == 3) {
            var indexOfEstimation = $("#tableCosteffectiveness").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(EditData.Id);
            $("#tableCosteffectiveness").UifDataTable('editRow', IndicatorSelected, indexOfEstimation);
        } else if (EditData.IndicatorType == 4) {
            var indexOfEstimation = $("#tableActivity").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(EditData.Id);
            $("#tableActivity").UifDataTable('editRow', IndicatorSelected, indexOfEstimation);
        }
        $("#InputObservations").val('');
        $('#EditTableIndicator').hide();
        //$('#ButtonEditTableIndicator').hide();
    }
    static CompanyCountry(event, data, value) {
        $('#InputDepartament').val("");
        $('#InputCity').val("");
        if ($("#InputCountry").val().trim().length > 0) {
            PersonLegal.companyGetCountriesByDescription($("#InputCountry").val());
        }
    }
    static LoadScript() {
        if (glbThird != null) {
            if (utilitys.length != 0 && SummaryUtilitys.length != 0) {
                if (indicatortypeliquidity.length != 0 && indicatortypeindebtedness.length != 0 && indicatortypecosteffectiveness.length != 0 && indicatortypeActivity.length != 0) {
                    if (RestrictivePolicies == 0) {
                        AutomaticQuotaRequest.GetScriptsAutocomplete(Script).done(function (data) {
                            if (data.length == 1) {
                                ExecuteScript.Execute(data[0].ScriptId, glbAutomaticQuota.Object, glbAutomaticQuota.Class, dynamicProperties);
                                calc = 1;
                            }
                        });
                    } else {
                        AutomaticQuota.ValidateAuthorizationPoliciesThird(glbThird, true);
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResources.IndicatorPartySectionFirst });
                }
            } else {
                $.UifDialog('alert', { 'message': AppResources.ThirdPartyandUtilitiessection });
            }
        } else {
            $.UifDialog('alert', { 'message': AppResources.ThirdPartySectionFirst });
        }
    }
    static GetCountries() {
        AutomaticQuotaRequest.GetCountries().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectCountry").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetStatesByCountryId(event, selectedItem) {
        AutomaticQuotaRequest.GetStatesByCountryId(selectedItem.Id).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectDepartment").UifSelect({ sourceData: data.result });
                }
            }
        });
    }

    static GetCitiesByCountryIdStateId(event, selectedItem) {
        var countryId = $("#SelectCountry").UifSelect("getSelected");
        AutomaticQuotaRequest.GetCitiesByCountryIdStateId(countryId, selectedItem.Id).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#SelectCity").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static GetdefaultValueCountry() {
        AutomaticQuotaRequest.GetParameters().done(function (data) {
            if (data.success) {
                if (data.result) {
                    var dataCountry = {
                        Id: data.result[0].Value,
                        Description: data.result[0].TextParameter
                    }
                    DateIni = data.result[1].Value.TextParameter;
                    DateFin = data.result[2].Value.TextParameter;
                    countryParameter = dataCountry.Id;
                    $("#InputCountry").data(dataCountry);
                    $("#InputCountry").val(dataCountry.Description);

                }
            }
        });
    }
    static GetAutomaticQuotaOperation() {
        var Id = $('#inputTemporalAutomaticQuota').val().trim();
        AutomaticQuotaRequest.GetAutomaticQuotaDeserealizado(Id).done(function (data) {
            if (data.success) {
                if (data.result) {
                    //General
                    AutomaticQuotaId = data.result.AutomaticQuotaId;
                    glbAutomaticQuota.IndividualId = data.result.IndividualId;
                    $("#SelectDocumentType").UifSelect("setSelected", data.result.ProspecDTO.DocumentType);
                    $("#InputNIdentification").val(data.result.ProspecDTO.DocumentNumber);
                    $("#InputNameBusinnesName").val(data.result.ProspecDTO.BusinessName);
                    AutomaticQuota.GetCountryByCountryId(data.result.ProspecDTO.CountryCd);
                    AutomaticQuota.GetStatesByCountryIdByStateId(data.result.ProspecDTO.CountryCd, data.result.ProspecDTO.StateCd);
                    AutomaticQuota.GetCitiesByCountryIdByStateIdById(data.result.ProspecDTO.CountryCd, data.result.ProspecDTO.StateCd, data.result.ProspecDTO.City);
                    $("#InputFullAddress").val(data.result.ProspecDTO.Address);
                    $("#InputPhone").val(data.result.ProspecDTO.Phone);
                    $("#InputEmail").val(data.result.ProspecDTO.Email);
                    $("#InputDateConstitution").val(FormatDate(data.result.ProspecDTO.ConstitutionDate));
                    $("#InputLegalRepresentative").val(data.result.ProspecDTO.LegalRepresentative);
                    $("#InputReviewerLegal").val(data.result.ProspecDTO.FiscalReviewer);
                    if (data.result.ProspecDTO.EconomicActivity != 0) {
                        $("#InputCiiuEconomicActivity").val(data.result.ProspecDTO.EconomicActivity);
                        AutomaticQuota.GetEconomyActivity(data.result.ProspecDTO.EconomicActivity);
                    }
                    $("#InputAgent").val(data.result.Agent.Description);
                    $("#SelectProgram").UifSelect("setSelected", data.result.AgentProgramDTO.Id);
                    $("#SelectCountry").UifSelect("setSelected", data.result.CountryId);
                    if (data.result.StateId != 0) {
                        AutomaticQuotaRequest.GetStatesByCountryId(data.result.CountryId).done(function (data1) {
                            if (data1.success) {
                                if (data1.result) {
                                    $("#SelectDepartment").UifSelect({ sourceData: data1.result });
                                    $("#SelectDepartment").UifSelect("setSelected", data.result.StateId);
                                }
                            }
                        });
                    }
                    if (data.result.CountryId != 0 && data.result.StateId != 0) {
                        AutomaticQuotaRequest.GetCitiesByCountryIdStateId(data.result.CountryId, data.result.StateId).done(function (data2) {
                            if (data2.success) {
                                if (data2.result) {
                                    $("#SelectCity").UifSelect({ sourceData: data2.result });
                                    $("#SelectCity").UifSelect("setSelected", data.result.CityId);

                                }
                            }
                        });
                    }
                    $("#InputDepartament").on("search", AutomaticQuota.SearchState);
                    $("#SelectCity").UifSelect("setSelected", data.result.CityId);
                    $("#InputSuggestedQuota").val(data.result.SuggestedQuota);
                    $("#InputReconsiderationQuota").val(data.result.QuotaReConsideration);
                    $("#InputLegalQuota").val(data.result.LegalizedQuota);
                    $("#InputCurrentQuota").val(data.result.CurrentQuota);
                    $("#InputCurrentCumulu").val(data.result.CurrentCluster);
                    $("#InputElaborationQuotaDat").val(FormatDate(data.result.QuotaPreparationDate));
                    $("#InputForSolution").val(data.result.RequestedByName);
                    $("#InputElaboration").val(data.result.ElaboratedName);
                    $("#SelectCounterGuaranteeType").UifSelect("setSelected", data.result.TypeCollateral);
                    $("#SelectCounterGuaranteeState").UifSelect("setSelected", data.result.CollateralStatus);
                    $("#InputObservation").val(data.result.Observations);
                    glbAutomaticQuota.CustomerType = data.result.CustomerTpeId;
                    //Terceros
                    glbThird = data.result.ThirdDTO;
                    if (data.result.ThirdDTO.RiskCenterDTO.Id != 0) {
                        $("#SelectconsultRiskCenters").UifSelect("setSelected", data.result.ThirdDTO.RiskCenterDTO.Id);
                    }
                    if (data.result.ThirdDTO.RestrictiveDTO.Id != 0) {
                        $("#SelectRestrictiveListQuery").UifSelect("setSelected", data.result.ThirdDTO.RestrictiveDTO.Id);
                    }
                    if (data.result.ThirdDTO.PromissoryNoteSignatureDTO.Id != 0) {
                        $("#SelectSignaturePromissoryNoteLetterIntent").UifSelect("setSelected", data.result.ThirdDTO.PromissoryNoteSignatureDTO.Id);
                    }
                    if (data.result.ThirdDTO.ReportListSisconcDTO.Id != 0) {
                        $("#SelectSisconcReport").UifSelect("setSelected", data.result.ThirdDTO.ReportListSisconcDTO.Id);
                    }
                    $("#InputQueryCifin").val(FormatDate(data.result.ThirdDTO.CifinQuery));
                    $("#InputMainDebtor").val(data.result.ThirdDTO.PrincipalDebtor);
                    $("#InputCodebtor").val(data.result.ThirdDTO.Cosigner);
                    $("#InputTotal").val(data.result.ThirdDTO.Total);

                    //Utilidades
                    if (data.result.UtilityDTO != null) {
                        utilitys = [];
                        SummaryUtilitys = [];
                        data.result.UtilityDTO.forEach(function (item, index) {
                            var UtilitysSelected = {
                                Id: item.Id,
                                Description: item.Description,
                                DateIni: parseFloat(item.Start_Values).toFixed(2),
                                DateFin: parseFloat(item.End_value).toFixed(2),
                                AbsoludValue: item.Var_Abs,
                                RelativeValue: item.Var_Relativa + '%',
                                UtilityType: item.UtilityDetails.UtilitysTypeCd,
                                UtilitySummary: item.UtilityDetails.UtilitysSummaryCd,
                                FormUtilitys: item.UtilityDetails.FormUtilitys,
                                utilityId: item.UtilityDetails.utilityId
                            };
                            if (item.Id >= 1 && item.Id <= 17) {
                                utilitys.push(UtilitysSelected);
                            } else if (item.Id >= 17) {
                                SummaryUtilitys.push(UtilitysSelected);
                            }
                        })
                    }
                    //Indicadores
                    if (data.result.indicatorDTO != null) {
                        AutomaticQuota.GetDefaultIndicator();
                        data.result.indicatorDTO.forEach(function (item, index) {
                            indicatortypeliquidity.forEach(function (item1, index) {
                                if (item1.Id == item.ConceptIndicatorCd) {
                                    item1.DateIni = item.IndicatorIni;
                                    item1.DateFin = item.IndicatorFin;
                                    item1.Observation = item.Observation;
                                }
                            });
                            indicatortypeindebtedness.forEach(function (item2, index) {
                                if (item2.Id == item.ConceptIndicatorCd) {
                                    item2.DateIni = item.IndicatorIni;
                                    item2.DateFin = item.IndicatorFin;
                                    item2.Observation = item.Observation;
                                }
                            });
                            indicatortypecosteffectiveness.forEach(function (item3, index) {
                                if (item3.Id == item.ConceptIndicatorCd) {
                                    item3.DateIni = item.IndicatorIni;
                                    item3.DateFin = item.IndicatorFin;
                                    item3.Observation = item.Observation;
                                }
                            });
                            indicatortypeActivity.forEach(function (item4, index) {
                                if (item4.Id == item.ConceptIndicatorCd) {
                                    item4.DateIni = item.IndicatorIni;
                                    item4.DateFin = item.IndicatorFin;
                                    item4.Observation = item.Observation;
                                }
                            });
                        });
                    }
                    //Dynamic Property
                    if (data.result.DynamicProperties != null) {
                        dynamicProperties = data.result.DynamicProperties;
                    }
                }
            }
            else {
                $.UifDialog('alert', { 'message': data.result });
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Underwriting.ClearControls();
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
        });

    }
    static ShowAdvanced() {
        //AdvancedSearchAutomaticQuota.clearFieldAdv();
       
        $("#listviewSearchAutomatic").UifListView(
            {
                displayTemplate: "#searchAutomaticTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
        dropDownSearch.show();
        $("#panelSearchAutomatic").show();
        $("#selectTypePersonAq").UifSelect('disabled', false);
        AutomaticQuotaRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $("#selectTypePersonAq").UifSelect({ sourceData: data.result });
            }
        });
        
    }

    static SaveUtilitys() {
        if (EditData.FormUtilitys == 1) {
            var AbsoludValue = parseFloat(NotFormatMoney($('#InputUtilitysDateEnd').val())) - parseFloat(NotFormatMoney($('#InputUtilitysDateIni').val()));
            var RelativeValue = AbsoludValue / parseFloat(NotFormatMoney($('#InputUtilitysDateIni').val())) * 100;
            RelativeValue = RelativeValue.toFixed(2);
            var UtilitysSelected = {
                Id: EditData.Id,
                Description: EditData.Description,
                DateIni: parseFloat(NotFormatMoney($('#InputUtilitysDateIni').val()).replace(',', '.')).toFixed(2),
                DateFin: parseFloat(NotFormatMoney($('#InputUtilitysDateEnd').val()).replace(',', '.')).toFixed(2),
                AbsoludValue: AbsoludValue,
                RelativeValue: RelativeValue + '%',
                UtilityType: EditData.UtilityType,
                UtilitySummary: EditData.UtilitySummary,
                FormUtilitys: EditData.FormUtilitys,
                utilityId: EditData.utilityId
            };
            var indexOfEstimation = $("#tableUtilitys").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(EditData.Id);
            $("#tableUtilitys").UifDataTable('editRow', UtilitysSelected, indexOfEstimation);
            utilitys = $("#tableUtilitys").UifDataTable('getData');
            var utilitysCalc = utilitys;
            utilitysCalc.forEach(function (item, index) {
                var TotalActivos;
                var TotalPasivos;
                if (item.Id == 4) {//Activo Corriente
                    item.AbsoludValue = parseFloat(utilitys[3].DateFin) - parseFloat(utilitys[3].DateIni);
                    //item.DateFin = parseFloat(utilitys[0].DateFin) + parseFloat(utilitys[1].DateFin) + parseFloat(utilitys[2].DateFin);
                    //item.DateIni = parseFloat(utilitys[0].DateIni) + parseFloat(utilitys[1].DateIni) + parseFloat(utilitys[2].DateIni);
                    item.DateFin = parseFloat(utilitys[3].DateFin);
                    item.DateIni = parseFloat(utilitys[3].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 7) {//Total Activo
                    item.AbsoludValue = parseFloat(utilitys[3].AbsoludValue) + parseFloat(utilitys[5].AbsoludValue);
                    //item.AbsoludValue = parseFloat(utilitys[0].AbsoludValue) + parseFloat(utilitys[1].AbsoludValue) + parseFloat(utilitys[2].AbsoludValue) + parseFloat(utilitys[4].AbsoludValue);
                    //item.DateFin = parseFloat(utilitys[0].DateFin) + parseFloat(utilitys[1].DateFin) + parseFloat(utilitys[2].DateFin) + parseFloat(utilitys[4].DateFin);
                    //item.DateIni = parseFloat(utilitys[0].DateIni) + parseFloat(utilitys[1].DateIni) + parseFloat(utilitys[2].DateIni) + parseFloat(utilitys[4].DateIni);
                    item.DateFin = parseFloat(utilitys[3].DateFin) +  parseFloat(utilitys[5].DateFin);
                    item.DateIni = parseFloat(utilitys[3].DateIni) + parseFloat(utilitys[5].DateIni);

                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 6) {// Activos no Corrientes
                    item.AbsoludValue = parseFloat(utilitys[5].DateFin) - parseFloat(utilitys[5].DateIni); //parseFloat(utilitys[4].AbsoludValue);
                    item.DateFin = parseFloat(utilitys[5].DateFin);
                    item.DateIni = parseFloat(utilitys[5].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 10) {//Pasivo Corriente
                    item.AbsoludValue = parseFloat(utilitys[9].DateFin) - parseFloat(utilitys[9].DateIni);//parseFloat(utilitys[7].AbsoludValue) + parseFloat(utilitys[8].AbsoludValue);
                    //item.DateFin = parseFloat(utilitys[7].DateFin) + parseFloat(utilitys[8].DateFin);
                    //item.DateIni = parseFloat(utilitys[7].DateIni) + parseFloat(utilitys[8].DateIni);
                    item.DateFin = parseFloat(utilitys[9].DateFin);
                    item.DateIni = parseFloat(utilitys[9].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 13) {//Total pasivo
                    item.AbsoludValue = parseFloat(utilitys[9].AbsoludValue) + parseFloat(utilitys[11].AbsoludValue);
                    item.DateFin = parseFloat(utilitys[9].DateFin) +  parseFloat(utilitys[11].DateFin);
                    item.DateIni = parseFloat(utilitys[9].DateIni) +  parseFloat(utilitys[11].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 12) {//Pasivo no corriente
                    item.AbsoludValue = parseFloat(utilitys[11].DateFin) - parseFloat(utilitys[11].DateIni);//parseFloat(utilitys[10].AbsoludValue);
                    item.DateFin = parseFloat(utilitys[11].DateFin); //parseFloat(utilitys[10].DateFin);
                    item.DateIni = parseFloat(utilitys[11].DateIni); //parseFloat(utilitys[10].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 17) {//Total Patrimonio
                    item.AbsoludValue = parseFloat(utilitys[16].DateFin) - parseFloat(utilitys[16].DateIni);//item.AbsoludValue = parseFloat(utilitys[13].AbsoludValue) + parseFloat(utilitys[14].AbsoludValue) + parseFloat(utilitys[15].AbsoludValue);
                    item.DateFin = parseFloat(utilitys[16].DateFin);//item.DateFin = parseFloat(utilitys[13].DateFin) + parseFloat(utilitys[14].DateFin) + parseFloat(utilitys[15].DateFin);
                    item.DateIni = parseFloat(utilitys[16].DateIni); //item.DateIni = parseFloat(utilitys[13].DateIni) + parseFloat(utilitys[14].DateIni) + parseFloat(utilitys[15].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                }
            });
            $('#tableUtilitys').UifDataTable('clear');
            $('#tableUtilitys').UifDataTable('addRow', utilitysCalc);

        } else if (EditData.FormUtilitys == 2) {

            var AbsoludValue = parseFloat(NotFormatMoney($('#InputUtilitysDateEnd').val())) - parseFloat(NotFormatMoney($('#InputUtilitysDateIni').val()));
            var RelativeValue = AbsoludValue / parseFloat(NotFormatMoney($('#InputUtilitysDateIni').val())) * 100;
            RelativeValue = RelativeValue.toFixed(2);
            var SummaryUtilitysSelected = {
                Id: EditData.Id,
                Description: EditData.Description,
                DateIni: parseFloat(NotFormatMoney($('#InputUtilitysDateIni').val()).replace(',', '.')).toFixed(2),
                DateFin: parseFloat(NotFormatMoney($('#InputUtilitysDateEnd').val()).replace(',', '.')).toFixed(2),
                AbsoludValue: AbsoludValue,
                RelativeValue: RelativeValue + '%',
                UtilityType: EditData.UtilityType,
                UtilitySummary: EditData.UtilitySummary,
                FormUtilitys: EditData.FormUtilitys,
                utilityId: EditData.utilityId
            };
            var indexOfEstimation = $("#tableSummaryUtilitys").UifDataTable('getData').map(function (e) { return e.Id; }).indexOf(EditData.Id);
            $("#tableSummaryUtilitys").UifDataTable('editRow', SummaryUtilitysSelected, indexOfEstimation);
            SummaryUtilitys = $("#tableSummaryUtilitys").UifDataTable('getData');
            var SummaryUtilitysCalc = SummaryUtilitys;
            SummaryUtilitysCalc.forEach(function (item, index) {
                if (item.Id == 20) {//Utilidad Bruta
                    item.AbsoludValue = parseFloat(SummaryUtilitys[0].AbsoludValue) + parseFloat(SummaryUtilitys[1].AbsoludValue);
                    item.DateFin = parseFloat(SummaryUtilitys[0].DateFin) + parseFloat(SummaryUtilitys[1].DateFin);
                    item.DateIni = parseFloat(SummaryUtilitys[0].DateIni) + parseFloat(SummaryUtilitys[1].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 22) {//Utilidad Operacional
                    item.AbsoludValue = parseFloat(SummaryUtilitys[0].AbsoludValue) + parseFloat(SummaryUtilitys[1].AbsoludValue) + parseFloat(SummaryUtilitys[3].AbsoludValue);
                    item.DateFin = parseFloat(SummaryUtilitys[0].DateFin) + parseFloat(SummaryUtilitys[1].DateFin) + parseFloat(SummaryUtilitys[3].DateFin);
                    item.DateIni = parseFloat(SummaryUtilitys[0].DateIni) + parseFloat(SummaryUtilitys[1].DateIni) + parseFloat(SummaryUtilitys[3].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 24) {//Utilidad Antes de impuestos
                    item.AbsoludValue = parseFloat(SummaryUtilitys[0].AbsoludValue) + parseFloat(SummaryUtilitys[1].AbsoludValue) + parseFloat(SummaryUtilitys[3].AbsoludValue) + parseFloat(SummaryUtilitys[5].AbsoludValue);
                    item.DateFin = parseFloat(SummaryUtilitys[0].DateFin) + parseFloat(SummaryUtilitys[1].DateFin) + parseFloat(SummaryUtilitys[3].DateFin) + parseFloat(SummaryUtilitys[5].DateFin);
                    item.DateIni = parseFloat(SummaryUtilitys[0].DateIni) + parseFloat(SummaryUtilitys[1].DateIni) + parseFloat(SummaryUtilitys[3].DateIni) + parseFloat(SummaryUtilitys[5].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                } else if (item.Id == 26) {//Utilidad Neta
                    item.AbsoludValue = parseFloat(SummaryUtilitys[0].AbsoludValue) + parseFloat(SummaryUtilitys[1].AbsoludValue) + parseFloat(SummaryUtilitys[3].AbsoludValue) + parseFloat(SummaryUtilitys[5].AbsoludValue) + parseFloat(SummaryUtilitys[7].AbsoludValue);
                    item.DateFin = parseFloat(SummaryUtilitys[0].DateFin) + parseFloat(SummaryUtilitys[1].DateFin) + parseFloat(SummaryUtilitys[3].DateFin) + parseFloat(SummaryUtilitys[5].DateFin) + parseFloat(SummaryUtilitys[7].DateFin);
                    item.DateIni = parseFloat(SummaryUtilitys[0].DateIni) + parseFloat(SummaryUtilitys[1].DateIni) + parseFloat(SummaryUtilitys[3].DateIni) + parseFloat(SummaryUtilitys[5].DateIni) + parseFloat(SummaryUtilitys[7].DateIni);
                    if (item.AbsoludValue != 0 && item.DateIni != 0) {
                        item.RelativeValue = item.AbsoludValue.toFixed(2) / item.DateIni.toFixed(2) * 100;
                        item.RelativeValue = parseFloat(item.RelativeValue).toFixed(2);
                        item.RelativeValue = item.RelativeValue + '%';
                    }
                }
            });
            $('#tableSummaryUtilitys').UifDataTable('clear');
            $('#tableSummaryUtilitys').UifDataTable('addRow', SummaryUtilitysCalc);
        }
        $('#EditTableUtilitys').hide();
    }
    //guardado completo
    //static RecordAutomaticQuota() {
    //   llamar metodo de guardado AutomaticQuotaRequest.SaveAutomaticQuota(automatic).done(function (data) {
    //        if (data.success) {
    //            AutomaticQuota.ValidateAuthorizationPolicies(data.result).then(() => {
    //                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + "<br>" + Resources.Language.FormNumber, 'autoclose': true });

    //            });
    //        }
    //    });
    //}

    //modelo de recuperacion de data del formulario
    static GetautomaticDTOModel() {
        var automaticDTO = $("#formAutomaticQuota").serializeObject();
        automaticDTO.ProspectDTO = {};
        //se debe de completar todo el modelo
        automaticDTO.ProspectDTO.Document_Type = $('#SelectDocumentType').UifSelect("getSelected");
        automaticDTO.ProspectDTO.Document_Number = $('#InputNIdentification').val();
        automaticDTO.ProspectDTO.Business_name = $('#InputNameBusinnesName').val();
        automaticDTO.ProspectDTO.Country_cd = $("#InputCountry").data("Id");
        automaticDTO.ProspectDTO.State_cd = $('#InputDepartament').data("Id");
        automaticDTO.ProspectDTO.City = $('#InputCity').data("Id");
        automaticDTO.ProspectDTO.Address = $('#InputFullAddress').val();
        automaticDTO.ProspectDTO.Phone = $('#InputPhone').val();
        automaticDTO.ProspectDTO.Email = $('#InputEmail').val();
        automaticDTO.ProspectDTO.Constitution_Date = $('#InputDateConstitution').val();
        automaticDTO.ProspectDTO.Legal_Representative = $('#InputLegalRepresentative').val();
        automaticDTO.ProspectDTO.Fiscal_Reviewer = $('#InputReviewerLegal').val();
        automaticDTO.ProspectDTO.Economic_Activity = $('#InputCiiuEconomicActivity').data("Id")
        automaticDTO.ProspectDTO.Agent = $('#InputAgent').UifSelect("getSelected");
        automaticDTO.ProspectDTO.Country = $('#SelectCountry').UifSelect("getSelected");
        automaticDTO.ProspectDTO.Departament = $('#SelectDepartment').UifSelect("getSelected");
        automaticDTO.ProspectDTO.CityProgram = $('#SelectCity').UifSelect("getSelected");

        //Cupo de Vinculacion
        automaticDTO.SuggestedQuota = $('InputSuggestedQuota').val() == "" ? 0 : $('InputSuggestedQuota').val();
        automaticDTO.QuotaReConsideration = $('#InputReconsiderationQuota').val() == "" ? 0 : $('InputReconsiderationQuota').val();
        automaticDTO.LegalizedQuota = $('#InputLegalQuota').val() == "" ? 0 : $('InputLegalQuota').val;
        automaticDTO.CurrentQuota = $('#InputCurrentQuota').val() == "" ? 0 : $('InputCurrentQuota').val();
        automaticDTO.CurrentCluster = $('#InputCurrentCumulu').val() == "" ? 0 : $('InputCurrentCumulu').val();
        automaticDTO.RequestedBy = $('#InputForSolution').val() == "" ? 0 : $('InputForSolution').val();
        automaticDTO.Elaborated = $('#InputElaboration').val() == "" ? 0 : $('InputElaboration').val();
        automaticDTO.QuotaPreparationDate = $("#InputElaborationQuotaDat").UifDatepicker('getValue');
        automaticDTO.TypeCollateral = $('#SelectCounterGuaranteeType').UifSelect("getSelected") == "" ? 0 : $('#SelectCounterGuaranteeType').UifSelect("getSelected");
        automaticDTO.CollateralStatus = $('#SelectCounterGuaranteeState').UifSelect("getSelected") == "" ? 0 : $('#SelectCounterGuaranteeState').UifSelect("getSelected");
        automaticDTO.Observations = $('#InputObservation').val();



        return automaticDTO;
    }


    //guardado completo
    static RecordAutomaticQuota() {
        var modelGeneral = AutomaticQuota.CreateModelGeneral();
        if (calc == 1 || (glbThird.InfringementPolicies.length > 0 && glbThird.InfringementPolicies.filter(x => x.Type == 2))) {
            if (glbThird != null || (glbThird.InfringementPolicies.length > 0 && glbThird.InfringementPolicies.filter(x => x.Type == 2))) {
                if ((utilitys.length != 0 && SummaryUtilitys.length != 0) || (glbThird.InfringementPolicies.length > 0 && glbThird.InfringementPolicies.filter(x => x.Type == 2))) {
                    if ((indicatortypeliquidity.length != 0 && indicatortypeindebtedness.length != 0 && indicatortypecosteffectiveness.length != 0 && indicatortypeActivity.length != 0) || (glbThird.InfringementPolicies.length > 0 && glbThird.InfringementPolicies.filter(x => x.Type == 2))) {
                        AutomaticQuotaRequest.SaveAutomaticQuotaGeneral(modelGeneral, dynamicProperties).done(function (data) {
                            if (data.success) {
                                if (data.result.InfringementPolicies != null) {
                                    if (data.result.InfringementPolicies.length > 0) {
                                        AutomaticQuota.ValidateAuthorizationPolicies(data.result.InfringementPolicies).then(() => {
                                        });
                                    }
                                }
                                else if (data.result.utilityId > 0) {
                                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + "<br>" + Resources.Language.FormNumber, 'autoclose': true });
                                }

                            }
                            if (data.result.InfringementPolicies.length == 0) {
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.SaveOperationQuota, 'autoclose': true });
                                AutomaticQuota.ClearAfterSave();
                            }
                            
                        });
                    } else {
                        $.UifDialog('alert', { 'message': AppResources.IndicatorPartySectionFirst });
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResources.ThirdPartyandUtilitiessection });
                }
            } else {
                $.UifDialog('alert', { 'message': AppResources.ThirdPartySectionFirst });
            }
        } else {

            $.UifNotify('show', { 'type': 'info', 'message': AppResources.CalculateButtonSelectedFirst, 'autoclose': true });
        }
    }

    static SaveAutomaticIndicatorJSON() {
        setTimeout(function () {
            AutomaticQuota.SaveAutomaticUtilitiesJSON();  
        }, 500);
        
        AutomaticQuota.SaveAutomaticGeneralJSON();
    }

    //guardado perifericos.
    static SaveAutomaticGeneralJSON() {
        var automatic = AutomaticQuota.CreateModelGeneral();
        AutomaticQuotaRequest.SaveAutomaticQuotaGeneralJSON(automatic, dynamicProperties).done(function (data) {
            if (data.success) {
                AutomaticQuotaId = data.result.AutomaticQuotaId;
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + " - " + Resources.Language.LabelNumberTemporal + ": " + AutomaticQuotaId, 'autoclose': true });
            }
        });
    }

    static SaveAutomaticThirdJSON() {
        RestrictivePolicies = 0;
        var third = AutomaticQuota.CreateModelThird();
        AutomaticQuotaRequest.SaveAutomaticQuotaThirdJSON(third).done(function (data) {
            if (data.success) {
                glbThird = data.result;
                AutomaticQuota.SaveAutomaticGeneralJSON();
                if (glbThird.InfringementPolicies != null && glbThird.InfringementPolicies.length > 0) {
                    glbThird.InfringementPolicies.forEach(function (item, index) {
                        if (item.Type == 2) {
                            RestrictivePolicies = 1;
                        }
                    });
                    AutomaticQuota.ValidateAuthorizationPoliciesThird(data.result)
                    $("#InputLegalQuota").prop("disabled", false);
                    $("#InputSuggestedQuota").prop("disabled", true);
                    $("#InputReconsiderationQuota").prop("disabled", true);
                }
                $("#inputTemporalAutomaticQuota").val(data.result.Id);
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation, 'autoclose': true });
            }
        });
    }
    static SaveAutomaticUtilitiesJSON() {
        var automaticq = AutomaticQuota.CreateModelGeneral();
        automaticq.AutomaticQuotaId = AutomaticQuotaId;
        var utilities = AutomaticQuota.CreateModelUtility();
        AutomaticQuotaRequest.SaveAutomaticQuotaUtilityJSON(utilities, automaticq).done(function (data) {
            if (data.success) {
                if (data.result) {
                    if (data.result.indicatorDTO.length > 0) {
                        var Liquidity = $('#tableLiquidity').UifDataTable('getData');
                        var Indebtedness = $('#tableIndebtedness').UifDataTable('getData');
                        var Costeffectivess = $('#tableCosteffectiveness').UifDataTable('getData');
                        var Activity = $('#tableActivity').UifDataTable('getData');
                        data.result.indicatorDTO.forEach(function (item, index) {
                            if (item.TypeIndicatorCd == 1) {
                                Liquidity.forEach(function (item1) {
                                    if (item.ConceptIndicatorCd == item1.Id) {
                                        item1.DateIni = parseFloat(item.IndicatorIni).toFixed(2);
                                        item1.DateFin = parseFloat(item.IndicatorFin).toFixed(2);
                                    }
                                });

                            }
                            if (item.TypeIndicatorCd == 2) {
                                Indebtedness.forEach(function (item2) {
                                    if (item.ConceptIndicatorCd == item2.Id) {
                                        item2.DateIni = parseFloat(item.IndicatorIni).toFixed(2);
                                        item2.DateFin = parseFloat(item.IndicatorFin).toFixed(2);
                                    }
                                });

                            }
                            if (item.TypeIndicatorCd == 3) {
                                Costeffectivess.forEach(function (item3) {
                                    if (item.ConceptIndicatorCd == item3.Id) {
                                        item3.DateIni = parseFloat(item.IndicatorIni).toFixed(2);
                                        item3.DateFin = parseFloat(item.IndicatorFin).toFixed(2);
                                    }
                                });
                            }
                            if (item.TypeIndicatorCd == 4) {
                                Activity.forEach(function (item4) {
                                    if (item.ConceptIndicatorCd == item4.Id) {
                                        item4.DateIni = parseFloat(item.IndicatorIni).toFixed(2);
                                        item4.DateFin = parseFloat(item.IndicatorFin).toFixed(2);
                                    }
                                });
                            }


                        });
                    }
                    indicatortypeliquidity = Liquidity;
                    indicatortypeindebtedness = Indebtedness;
                    indicatortypecosteffectiveness = Costeffectivess;
                    indicatortypeActivity = Activity;
                    $('#tableLiquidity').UifDataTable('clear');
                    $('#tableIndebtedness').UifDataTable('clear');
                    $('#tableCosteffectiveness').UifDataTable('clear');
                    $('#tableActivity').UifDataTable('clear');
                    $('#tableLiquidity').UifDataTable('addRow', Liquidity);
                    $('#tableIndebtedness').UifDataTable('addRow', Indebtedness);
                    $('#tableCosteffectiveness').UifDataTable('addRow', Costeffectivess);
                    $('#tableActivity').UifDataTable('addRow', Activity);
                }
            }

            data.result.UtilityDTO = utilities;
            data.result.AutomaticQuotaId = AutomaticQuotaId;
            data.result.ProspecDTO.ConstitutionDate = $('#InputDateConstitution').val();
            data.result.ThirdDTO.CifinQuery = $('#InputQueryCifin').val() == '' ? new Date() : $('#InputQueryCifin').val();
            AutomaticQuotaRequest.SaveAutomaticQuotaGeneralJSON(data.result, dynamicProperties).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation, 'autoclose': true });
                }

            });

        });


    }


    //Se debe pasar el DTO de automaticQUota
    static ValidateAuthorizationPolicies(AutomaticQuotaDto) {
        return new Promise((resolve, reject) => {
            var policyType = LaunchPolicies.ValidateInfringementPolicies(AutomaticQuotaDto, true);
            let countAuthorization = AutomaticQuotaDto.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

            if (countAuthorization > 0) {
                if (countAuthorization > 0) {
                    LaunchPolicies.RenderViewAuthorizationPolicies(AutomaticQuotaDto, AutomaticQuotaId.toString(), FunctionType.AutomaticQuota);
                }
                reject();
            } else if (policyType === TypeAuthorizationPolicies.Restrictive) {
                $.UifNotify('show', { 'type': 'info', 'message': 'Cupo legalizado diligenciado', 'autoclose': true });
            } else {
                resolve();
            }
        });

    }

    static ValidateAuthorizationPoliciesThird(AutomaticQuotaDto) {
        var policyType = LaunchPolicies.ValidateInfringementPolicies(AutomaticQuotaDto.InfringementPolicies, true);
        let countAuthorization = AutomaticQuotaDto.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

        if (policyType === TypeAuthorizationPolicies.Restrictive) {
            if (countAuthorization > 0) {
                LaunchPolicies.RenderViewAuthorizationPolicies(AutomaticQuotaDto.InfringementPolicies);
            }

        }
    }

    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbAutomaticQuota.RecordScript = isModify;
        if (isModify) {
            AutomaticQuota.Calculate();
        }
    }

    static GetValidateQuota() {
        $("#formAutomaticQuota").validate();

        if ($("#formAutomaticQuota").valid()) {
            var automaticDTO = AutomaticQuota.GetRiskCenterList();

            AutomaticQuotaRequest.GetValidateQuota(automaticDTO, dynamicProperties).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        //Property.LoadRisk(data.result);
                        //Property.UpdateGlbRisk(data.result);
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': 'validacion', 'autoclose': true });
            });
        }
    }

    static GetUserByName(event, value) {
        AutomaticQuotaRequest.GetUserByName(value).done(function (data) {
            if (data.success) {
                if (data.result) {
                    if (data.result.length == 1) {
                        $("#InputForSolution").data(data.result[0]);
                        $("#InputForSolution").val(data.result[0].AccountName);

                    } else {
                        modalListType = 3;
                        var dataList = {
                            dataObject: []
                        };

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.dataObject.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        AutomaticQuota.ShowModalList(dataList.dataObject);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.SelectUser);
                    }
                }
            }
        });
    }
    static GetUserSession() {
        AutomaticQuotaRequest.GetUserSession().done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#InputElaboration").text(data.result.UserId);
                    $("#InputElaboration").val(data.result.AccountName);
                }
            }
        });
    }

    static CreateModelGeneral() {

        var automatic = {
            AutomaticQuotaId: AutomaticQuotaId,
            ProspectId: 1,
            IndividualId: glbAutomaticQuota.IndividualId,
            CustomerTpeId: glbAutomaticQuota.CustomerType,
            utilityId: AutomaticQuotaId,
            ProspecDTO: AutomaticQuota.CreateModelProspect(),
            AgentProgramDTO: AutomaticQuota.CreateProgramAgent(),
            SuggestedQuota: $('#InputSuggestedQuota').val() == "" ? 0 : $('#InputSuggestedQuota').val(),
            QuotaReConsideration: $('#InputReconsiderationQuota').val(),
            LegalizedQuota: $('#InputLegalQuota').val(),
            CurrentQuota: $('#InputCurrentQuota').val(),
            CurrentCluster: $('#InputCurrentCumulu').val() == "" ? 0 : $('InputCurrentCumulu').val(),
            RequestedByName: $('#InputForSolution').val(),
            ElaboratedName: $('#InputElaboration').val(),
            QuotaPreparationDate: $("#InputElaborationQuotaDat").val(),
            TypeCollateral: $('#SelectCounterGuaranteeType').UifSelect("getSelected") == "" ? 0 : $('#SelectCounterGuaranteeType').UifSelect("getSelected"),
            CollateralStatus: $('#SelectCounterGuaranteeState').UifSelect("getSelected") == "" ? 0 : $('#SelectCounterGuaranteeState').UifSelect("getSelected"),
            Observations: $('#InputObservation').val(),
            Agent: AutomaticQuota.CreateAgent(),
            ThirdDTO: AutomaticQuota.CreateModelThird(),
            UtilityDTO: AutomaticQuota.CreateModelUtility(),
            indicatorDTO: AutomaticQuota.CreateModelIndicator(),
            CountryId: $("#SelectCountry").UifSelect("getSelected"),
            StateId: $("#SelectDepartment").UifSelect("getSelected"),
            CityId: $("#SelectCity").UifSelect("getSelected")
        }
        return automatic;

    }
    static CreateModelProspect() {
        var ProspectModelsView = {
            DocumentType: $('#SelectDocumentType').UifSelect("getSelected"),
            DocumentNumber: $('#InputNIdentification').val(),
            BusinessName: $('#InputNameBusinnesName').val(),
            CountryCd: $("#InputCountry").data("Id"),
            StateCd: $('#InputDepartament').data("Id"),
            City: $('#InputCity').data("Id"),
            Address: $('#InputFullAddress').val(),
            Phone: $('#InputPhone').val(),
            Email: $('#InputEmail').val(),
            ConstitutionDate: $('#InputDateConstitution').val(),
            LegalRepresentative: $('#InputLegalRepresentative').val(),
            FiscalReviewer: $('#InputReviewerLegal').val(),
            EconomicActivity: $('#InputCiiuEconomicActivity').data("Id")


        }
        return ProspectModelsView;
    }

    static CreateModelThird() {
        var ThirdModelsView = {
            id: AutomaticQuotaId,
            PrincipalDebtor: $('#InputMainDebtor').val(),
            Cosigner: $('#InputCodebtor').val(),
            Total: $('#InputTotal').val(),
            RiskCenterDTO: AutomaticQuota.CreateRiskCenter(),
            RestrictiveDTO: AutomaticQuota.CreateRestrictive(),
            PromissoryNoteSignatureDTO: AutomaticQuota.CreateSignature(),
            ReportListSisconcDTO: AutomaticQuota.CreateSisconc(),
            CifinQuery: $('#InputQueryCifin').val() == '' ? new Date() : $('#InputQueryCifin').val()
        }
        return ThirdModelsView;
    }

    static CreateProgramAgent() {
        AgentProgramDTO = {
            Id: $('#SelectProgram').UifSelect("getSelected"),
            SmallDescription: $('#SelectProgram').UifSelect("getSelectedText"),
            Description: $('#SelectProgram').UifSelect("getSelectedText"),
            Enabled: 1
        }
        return AgentProgramDTO;
    }

    static CreateAgent() {
        var infoAgent = $('#InputAgent').val();
        var splitInforAgent = infoAgent.split('-');
        Agent = {
            Id: splitInforAgent[0],
            Description: splitInforAgent[1]
        }
        return Agent;
    }

    static CreateRiskCenter() {
        RiskCenterDTO = {
            id: $('#SelectconsultRiskCenters').UifSelect("getSelected"),
            SmallDescription: $('#SelectconsultRiskCenters').UifSelect("getSelectedText"),
            Description: $('#SelectconsultRiskCenters').UifSelect("getSelectedText"),
            Enabled: 1
        }
        return RiskCenterDTO;
    }

    static CreateRestrictive() {
        RestrictiveDTO = {
            id: $('#SelectRestrictiveListQuery').UifSelect("getSelected"),
            SmallDescription: $('#SelectRestrictiveListQuery').UifSelect("getSelectedText"),
            Description: $('#SelectRestrictiveListQuery').UifSelect("getSelectedText"),
            Enabled: 1
        }
        return RestrictiveDTO;
    }

    static CreateSignature() {
        PromissoryNoteSignatureDTO = {
            id: $('#SelectSignaturePromissoryNoteLetterIntent').UifSelect("getSelected"),
            SmallDescrption: $('#SelectSignaturePromissoryNoteLetterIntent').UifSelect("getSelectedText"),
            Description: $('#SelectSignaturePromissoryNoteLetterIntent').UifSelect("getSelectedText"),
            Enabled: 1
        }
        return PromissoryNoteSignatureDTO;
    }

    static CreateSisconc() {
        ReportListSisconcDTO = {
            id: $('#SelectSisconcReport').UifSelect("getSelected"),
            SmallDescription: $('#SelectSisconcReport').UifSelect("getSelectedText"),
            Description: $('#SelectSisconcReport').UifSelect("getSelectedText"),
            Enabled: 1
        }
        return ReportListSisconcDTO;
    }
    static CreateUtilityDetails(item) {
        UtilityDetails = {
            id: item.Id,
            FormUtilitys: item.FormUtilitys,
            utilityId: item.utilityId,
            UtilitysTypeCd: item.UtilityType
        }
        return UtilityDetails;
    }


    static CreateModelUtility() {
        utilityDTO = [];
        if (utilitys != null) {
            Object.keys(utilitys).forEach(function (index) {
                utilityDTO.push({
                    Id: utilitys[index].Id,
                    UtilityDetails: AutomaticQuota.CreateUtilityDetails(utilitys[index]),
                    Start_Values: parseFloat(utilitys[index].DateIni),
                    End_value: parseFloat(utilitys[index].DateFin),
                    Var_Abs: parseFloat(utilitys[index].AbsoludValue),
                    Var_Relativa: parseFloat(utilitys[index].RelativeValue),
                    Description: utilitys[index].Description

                });
            });
        }

        if (SummaryUtilitys != null) {
            Object.keys(SummaryUtilitys).forEach(function (index) {
                utilityDTO.push({
                    Id: SummaryUtilitys[index].Id,
                    UtilityDetails: AutomaticQuota.CreateUtilityDetails(SummaryUtilitys[index]),
                    Start_Values: parseFloat(SummaryUtilitys[index].DateIni),
                    End_value: parseFloat(SummaryUtilitys[index].DateFin),
                    Var_Abs: parseFloat(SummaryUtilitys[index].AbsoludValue),
                    Var_Relativa: parseFloat(SummaryUtilitys[index].RelativeValue),
                    Description: SummaryUtilitys[index].Description
                });
            });
        }

        return utilityDTO
    }

    static CreateModelIndicator() {
        indicatorDTO = [];

        if (indicatortypeliquidity.length > 0) {
            Object.keys(indicatortypeliquidity).forEach(function (index) {
                indicatorDTO.push({
                    ConceptIndicatorCd: indicatortypeliquidity[index].Id,
                    IndicatorIni: parseFloat(indicatortypeliquidity[index].DateIni),
                    IndicatorFin: parseFloat(indicatortypeliquidity[index].DateFin),
                    Observation: indicatortypeliquidity[index].Observation,
                    Description: indicatortypeliquidity[index].Description
                });
            });
        }
        if (indicatortypeindebtedness.length > 0) {
            Object.keys(indicatortypeindebtedness).forEach(function (index) {
                indicatorDTO.push({
                    ConceptIndicatorCd: indicatortypeliquidity[index].Id,
                    IndicatorIni: parseFloat(indicatortypeindebtedness[index].DateIni),
                    IndicatorFin: parseFloat(indicatortypeindebtedness[index].DateFin),
                    Observation: indicatortypeindebtedness[index].Observation,
                    Description: indicatortypeliquidity[index].Description
                });
            });
        }
        if (indicatortypecosteffectiveness.length > 0) {
            Object.keys(indicatortypecosteffectiveness).forEach(function (index) {
                indicatorDTO.push({
                    ConceptIndicatorCd: indicatortypecosteffectiveness[index].Id,
                    IndicatorIni: parseFloat(indicatortypecosteffectiveness[index].DateIni),
                    IndicatorFin: parseFloat(indicatortypecosteffectiveness[index].DateFin),
                    Observation: indicatortypecosteffectiveness[index].Observation,
                    Description: indicatortypeliquidity[index].Description
                });
            });
        }
        if (indicatortypeActivity.length > 0) {
            Object.keys(indicatortypeActivity).forEach(function (index) {
                indicatorDTO.push({
                    ConceptIndicatorCd: indicatortypeActivity[index].Id,
                    IndicatorIni: parseFloat(indicatortypeActivity[index].DateIni),
                    IndicatorFin: parseFloat(indicatortypeActivity[index].DateFin),
                    Observation: indicatortypeActivity[index].Observation,
                    Description: indicatortypeliquidity[index].Description
                });
            });
        }
        return indicatorDTO;
    }

    static GetPersonByDescriptionInsuredSearchTypeCustomerType(event, value) {
        AutomaticQuotaRequest.GetPersonByDescriptionInsuredSearchTypeCustomerType(value).done(function (data) {
            if (data.success) {
                AutomaticQuota.ClearProspect();
                if (data.result.length == 0 && individualSearchType == 2) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                }
                else if (data.result.length == 0) {

                    UnderwritingPersonOnline.ShowOnlinePerson();
                }
                else if (data.result.length == 1) {
                    glbAutomaticQuota.IndividualId = data.result[0].IndividualId;
                    glbAutomaticQuota.CustomerTpeId = data.result[0].CustomerTpeId;
                    if (data.result[0].AssociationType.Id == 1) {
                        glbAutomaticQuota.CustomerType = data.result[0].CustomerType;
                        if (data.result[0].Name != "") {
                            $("#InputNameBusinnesName").val(data.result[0].Name);
                            $("#InputNameBusinnesName").data("Id", data.result[0].IndividualId);
                            $("#InputNameBusinnesName").prop("disabled", true);
                        } else {
                            $("#InputNameBusinnesName").prop("disabled", false);
                        }
                        $("#InputNIdentification").val(data.result[0].IdentificationDocument.Number);
                        if (data.result[0].IdentificationDocument.DocumentType.Id != 0) {
                            $('#SelectDocumentType').UifSelect("setSelected", data.result[0].IdentificationDocument.DocumentType.Id);
                            $("#SelectDocumentType").prop("disabled", true);
                        } else {
                            $("#SelectDocumentType").prop("disabled", false);
                        }
                        if (data.result[0].EconomicActivity.Id != 0) {
                            $("#InputCiiuEconomicActivity").val(data.result[0].EconomicActivity.Id);
                            AutomaticQuota.GetEconomyActivity(data.result[0].EconomicActivity.Id);
                            $("#InputCiiuEconomicActivity").prop("disabled", true);
                        } else {
                            $("#InputCiiuEconomicActivity").prop("disabled", false);
                        }
                        if (data.result[0].IndividualId) {
                            AutomaticQuota.GetOperatingQuotaEventByIndividualIdByLineBusinessId(data.result[0].IndividualId);
                            $("#InputCurrentQuota").prop("disabled", true);
                            $("#InputCurrentCumulu").prop("disabled", true);
                        } else {
                            $("#InputCurrentQuota").prop("disabled", false);
                            $("#InputCurrentCumulu").prop("disabled", false);
                        }
                        if (data.result[0].CompanyName != null) {
                            if (data.result[0].CompanyName.Email != null) {
                                $("#InputFullAddress").val(data.result[0].CompanyName.Address.Description);
                                $("#InputFullAddress").prop("disabled", true);
                            } else {
                                $("#InputFullAddress").prop("disabled", false);
                            }
                            if (data.result[0].CompanyName.Email != null) {
                                $("#InputEmail").val(data.result[0].CompanyName.Email.Description);
                                $("#InputEmail").prop("disabled", true);
                            } else {
                                $("#InputEmail").prop("disabled", false);
                            }
                            if (data.result[0].CompanyName.Phone != null) {
                                $("#InputPhone").val(data.result[0].CompanyName.Phone.Description);
                                $("#InputPhone").prop("disabled", true);
                            } else {
                                $("#InputPhone").prop("disabled", false);
                            }
                        }
                        if (data.result[0].IndividualId != 0) {
                            AutomaticQuota.GetAddressByIndividualIdCompany(data.result[0].IndividualId);
                            AutomaticQuota.GetAgencyByIndividualId(data.result[0].IndividualId);
                        }
                    } else {
                        $("#InputNIdentification").val('');
                        $.UifDialog('alert', { 'message': AppResourcesPerson.MessageOperationQuotaConsortium });
                    }
                }
                else if (data.result.length > 1) {
                    for (var i = 0; i < data.result.length; i++) {
                        dataList.push({
                            Id: data.result[i].IndividualId,
                            CustomerType: data.result[i].CustomerType,
                            Code: data.result[i].InsuredId,
                            DocumentNum: data.result[i].IdentificationDocument.Number,
                            Description: data.result[i].Name,
                            CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                            DocumentType: data.result[i].IdentificationDocument.DocumentType.Description,
                            DocumentTypeId: data.result[i].IdentificationDocument.DocumentType.Id,
                            EconomicActivity: data.result[i].EconomicActivity,
                            CompanyName: data.result[i].CompanyName,
                            AssociationType: data.result[0].AssociationType,
                        });
                    }
                    modalListType = 4;
                    AutomaticQuota.ShowModalList(dataList);
                    $('#modalDefaultSearch').UifModal('showLocal', AppResources.SelectPersona);
                }
            }
            else {

                $('#modalNotificationPerson').UifModal('showLocal', "Crear Persona");

                if (individualSearchType == 2) {
                    $("#inputBeneficiaryName").data("Object", null);
                    $("#inputBeneficiaryName").data("Detail", null);
                    $("#inputBeneficiaryName").val('');
                }
                else {
                    $("#inputHolder").data("Object", null);
                    $("#inputHolder").data("Detail", null);
                    //$("#inputHolder").val('');
                }
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
        });
    }

    static GetCountriesByDescription(description) {
        if (description.length >= 3) {
            AutomaticQuotaRequest.GetCountriesByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataCountries = [];
                        $.each(data.result, function (index, value) {
                            dataCountries.push({
                                Id: value.Id,
                                Code: value.Id,
                                Description: value.Description
                            });
                        });
                        modalListType = 5;
                        AutomaticQuota.ShowModalList(dataCountries);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.ModalTitleCountries);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageNotFoundCountries, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }
    static GetStatesByCountryIdByDescription(countryId, description) {
        if (description.length >= 3) {
            if (countryId != undefined) {
                AutomaticQuotaRequest.GetStatesByCountryIdByDescription(countryId, description).done(function (data) {
                    if (data.success) {
                        if (data.result !== null && data.result.length > 0) {
                            var dataStates = [];
                            $.each(data.result, function (index, value) {
                                dataStates.push({
                                    Id: value.Id,
                                    Code: value.Id,
                                    Description: value.Description
                                });
                            });
                            modalListType = 6;
                            AutomaticQuota.ShowModalList(dataStates);
                            $('#modalDefaultSearch').UifModal('showLocal', AppResources.ModalTitleStates);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorDocumentControlCountry, 'autoclose': true })
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    static GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        if (description.length >= 3) {
            if (countryId != undefined && stateId != undefined) {
                AutomaticQuotaRequest.GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description).done(function (data) {
                    if (data.success) {
                        if (data.result !== null && data.result.length > 0) {
                            var dataCities = [];
                            $.each(data.result, function (index, value) {
                                dataCities.push({
                                    Id: value.Id,
                                    Code: value.Id,
                                    Description: value.Description
                                });
                            });
                            modalListType = 7;
                            AutomaticQuota.ShowModalList(dataCities);
                            $('#modalDefaultSearch').UifModal('showLocal', AppResources.ModalTitleCities);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorValidateCountryState, 'autoclose': true })
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }
    static SumTotal() {
        $("#InputTotal").val(parseFloat($("#InputMainDebtor").val()) + parseFloat($("#InputCodebtor").val()));
    }

    static QuotaReconsideration() {
        $("#InputLegalQuota").val(parseFloat($("#InputReconsiderationQuota").val()));
    }

    static GetDefaultIndicator() {
        if (indicatortypeliquidity.length == 0 && indicatortypeindebtedness.length == 0 && indicatortypecosteffectiveness.length == 0 && indicatortypeActivity.length == 0) {
            AutomaticQuotaRequest.GetIndicatorConcepts().done(function (data) {
                if (data.success) {
                    if (data.result) {
                        data.result.forEach(function (item) {
                            if (item.IndicatorType == 1) {
                                indicatortypeliquidity.push({
                                    Id: item.Id,
                                    Description: item.Description,
                                    DateIni: 0,
                                    DateFin: 0,
                                    Observations: 0,
                                    IndicatorType: item.IndicatorType
                                });
                            }
                            if (item.IndicatorType == 2) {
                                indicatortypeindebtedness.push({
                                    Id: item.Id,
                                    Description: item.Description,
                                    DateIni: 0,
                                    DateFin: 0,
                                    Observations: 0,
                                    IndicatorType: item.IndicatorType
                                });
                            }
                            if (item.IndicatorType == 3) {
                                indicatortypecosteffectiveness.push({
                                    Id: item.Id,
                                    Description: item.Description,
                                    DateIni: 0,
                                    DateFin: 0,
                                    Observations: 0,
                                    IndicatorType: item.IndicatorType
                                });
                            }
                            if (item.IndicatorType == 4) {
                                indicatortypeActivity.push({
                                    Id: item.Id,
                                    Description: item.Description,
                                    DateIni: 0,
                                    DateFin: 0,
                                    Observations: 0,
                                    IndicatorType: item.IndicatorType
                                });
                            }
                        });
                        $('#tableLiquidity').UifDataTable('clear');
                        $('#tableIndebtedness').UifDataTable('clear');
                        $('#tableCosteffectiveness').UifDataTable('clear');
                        $('#tableActivity').UifDataTable('clear');
                        $('#tableLiquidity').UifDataTable('addRow', indicatortypeliquidity);
                        $('#tableIndebtedness').UifDataTable('addRow', indicatortypeindebtedness);
                        $('#tableCosteffectiveness').UifDataTable('addRow', indicatortypecosteffectiveness);
                        $('#tableActivity').UifDataTable('addRow', indicatortypeActivity);
                    }
                }
            });
        }
    }
    static Calculate() {
        if (RestrictivePolicies == 0) {
        AutomaticQuotaRequest.ExecuteCalculate(AutomaticQuotaId, dynamicProperties).done(function (data) {
            if (data.success) {
                $('#InputSuggestedQuota').val(data.result.SuggestedQuota);
                data.result.AutomaticQuotaId = AutomaticQuotaId;
                data.result.ProspecDTO.Phone = $('#InputPhone').val();
                data.result.ThirdDTO.CifinQuery = $('#InputQueryCifin').val() == '' ? new Date() : $('#InputQueryCifin').val();
                data.result.ProspecDTO.ConstitutionDate = $('#InputDateConstitution').val();
                data.result.QuotaPreparationDate = $("#InputElaborationQuotaDat").val();
                data.result.CustomerTpeId = glbAutomaticQuota.CustomerType;
                objectAgent = AutomaticQuota.CreateAgent();
                data.result.Agent.Description = objectAgent.Description;
                data.result.Agent.Id = objectAgent.Id;
                AutomaticQuotaRequest.SaveAutomaticQuotaGeneralJSON(data.result, dynamicProperties).done(function (data) {
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation + " - " + Resources.Language.LabelNumberTemporal + ": " + AutomaticQuotaId, 'autoclose': true });
                    }
                });
            }
        });
    } else {
        AutomaticQuota.ValidateAuthorizationPoliciesThird(glbThird, true);
    }
}


    static GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId) {
        OperationQuotaCumulusRequest.GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId, 30).done(function (OperationQuota) {
            if (OperationQuota.success) {
                if (OperationQuota.result) {
                    if (OperationQuota.result.IndividualOperatingQuota != 0) {
                        if (OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT != null) {
                            $("#InputCurrentQuota").val(OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT);
                            $("#InputCurrentQuota").prop("disabled", true);
                        } else {
                            $("#InputCurrentQuota").prop("disabled", false);
                        }
                        if (OperationQuota.result.ApplyEndorsement.AmountCoverage != null) {
                            $("#InputCurrentCumulu").val(OperationQuota.result.ApplyEndorsement.AmountCoverage);
                            $("#InputCurrentCumulu").prop("disabled", true);
                        } else {
                            $("#InputCurrentCumulu").prop("disabled", false);
                        }
                    }
                }
            }
        });
    }
    static GetAddressByIndividualIdCompany(IndividualId) {
        AutomaticQuotaRequest.GetAddressByIndividualIdCompany(IndividualId).done(function (data) {
            if (data.success) {
                if (data.result) {
                    data.result.forEach(function (item, index) {
                        if (item.IsPrincipal) {
                            if (item.Description != "") {
                                $("#InputFullAddress").val(item.Description);
                                $("#InputFullAddress").prop("disabled", true);
                            } else {
                                $("#InputFullAddress").prop("disabled", false);
                            }
                            if (item.City.Description != "") {
                                $("#InputCity").val(item.City.Description);
                                $("#InputCity").data(item.City);
                                $("#InputCity").prop("disabled", true);
                            } else {
                                $("#InputCity").prop("disabled", false);
                            }
                            if (item.City.State.Description != "") {
                                $("#InputDepartament").val(item.City.State.Description);
                                $("#InputDepartament").data(item.City.State);
                                $("#InputDepartament").prop("disabled", true);
                            } else {
                                $("#InputDepartament").prop("disabled", false);
                            }
                            if (item.City.State.Country.Description != "") {
                                $("#InputCountry").val(item.City.State.Country.Description);
                                $("#InputCountry").data(item.City.State.Country);
                                $("#InputCountry").prop("disabled", true);
                            } else {
                                $("#InputCountry").prop("disabled", false);
                            }
                        }
                    });
                }
            }
        });
    }
    static GetAgencyByIndividualId(IndividualId) {
        AutomaticQuotaRequest.GetAgencyByIndividualId(IndividualId).done(function (data) {
            if (data.success) {
                if (data.result && data.result.length > 0) {
                    $("#InputAgent").val(data.result[0].FullName);
                    $("#InputAgent").prop('disabled', true);
                }
            }
        });
    }

    static GetCountryByCountryId(countryId) {
        AutomaticQuotaRequest.GetCountryByCountryId(countryId).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#InputCountry").val(data.result.Description);
                    $("#InputCountry").data(data.result);
                }
            }
        });
    }
    static GetStatesByCountryIdByStateId(countryId, stateId) {
        AutomaticQuotaRequest.GetStatesByCountryIdByStateId(countryId, stateId).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#InputDepartament").val(data.result.Description);
                    $("#InputDepartament").data(data.result);
                }
            }
        });
    }
    static GetCitiesByCountryIdByStateIdById(countryId, stateId, cityId) {
        AutomaticQuotaRequest.GetCitiesByCountryIdByStateIdById(countryId, stateId, cityId).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#InputCity").val(data.result.Description);
                    $("#InputCity").data(data.result);
                }
            }
        });
    }
    static ClearProspect() {
        $("#InputNameBusinnesName").val('');
        $("#InputNameBusinnesName").data("Id", '');
        $("#InputNameBusinnesName").prop("disabled", false);
        $('#SelectDocumentType').UifSelect("setSelected", '');
        $("#SelectDocumentType").prop("disabled", false);
        $("#InputCiiuEconomicActivity").val('');
        $("#InputCiiuEconomicActivity").prop("disabled", false);
        $("#InputCurrentQuota").val('');
        $("#InputCurrentCumulu").val('');
        $("#InputCurrentQuota").prop("disabled", false);
        $("#InputCurrentCumulu").prop("disabled", false);
        $("#InputFullAddress").val('');
        $("#InputFullAddress").prop("disabled", false);
        $("#InputEmail").val('');
        $("#InputEmail").prop("disabled", false);
        $("#InputPhone").val('');
        $("#InputPhone").prop("disabled", false);
        $("#InputFullAddress").val('');
        $("#InputFullAddress").prop("disabled", false);
        $("#InputCity").val('');
        $("#InputCity").data('');
        $("#InputCity").prop("disabled", false);
        $("#InputDepartament").val('');
        $("#InputDepartament").data('');
        $("#InputDepartament").prop("disabled", false);
        $("#InputCountry").val('');
        $("#InputCountry").data('');
        $("#InputCountry").prop("disabled", false);
        $("#InputAgent").val('');
        $("#InputAgent").prop('disabled', false);
    }

    static ClearAfterSave() {
        $("#InputNameBusinnesName").val('');
        $("#InputNameBusinnesName").data("Id", '');
        $("#InputNameBusinnesName").prop("disabled", false);
        $("#SelectDocumentType").UifSelect("setSelected", '');
        $("#SelectDocumentType").prop("disabled", false);
        $("#InputCiiuEconomicActivity").val('');
        $("#InputCiiuEconomicActivity").prop("disabled", false);
        $("#InputCurrentQuota").val('');
        $("#InputCurrentCumulu").val('');
        $("#InputCurrentQuota").prop("disabled", false);
        $("#InputCurrentCumulu").prop("disabled", false);
        $("#InputFullAddress").val('');
        $("#InputFullAddress").prop("disabled", false);
        $("#InputEmail").val('');
        $("#InputEmail").prop("disabled", false);
        $("#InputPhone").val('');
        $("#InputPhone").prop("disabled", false);
        $("#InputFullAddress").val('');
        $("#InputFullAddress").prop("disabled", false);
        $("#InputCity").val('');
        $("#InputCity").data('');
        $("#InputCity").prop("disabled", false);
        $("#InputDepartament").val('');
        $("#InputDepartament").data('');
        $("#InputDepartament").prop("disabled", false);
        $("#InputCountry").val('');
        $("#InputCountry").data('');
        $("#InputCountry").prop("disabled", false);
        $("#InputAgent").val('');
        $("#InputAgent").prop('disabled', false);
        $("#InputNIdentification").val('');
        $("#InputDateConstitution").val('');
        $("#InputLegalRepresentative").val('');
        $("#InputReviewerLegal").val('');
        $("#SelectProgram").UifSelect("setSelected", '');
        $("#SelectCountry").UifSelect("setSelected", '');
        $("#SelectDepartment").UifSelect("setSelected", '');
        $("#SelectCity").UifSelect("setSelected", '');
        $("#InputForSolution").val('');
        $("#SelectCounterGuaranteeType").UifSelect("setSelected", '');
        $("#SelectCounterGuaranteeState").UifSelect("setSelected", '');
        $("#InputObservation").val('');
        $("#SelectconsultRiskCenters").UifSelect("setSelected", '');
        $("#SelectRestrictiveListQuery").UifSelect("setSelected", '');
        $("#SelectSignaturePromissoryNoteLetterIntent").UifSelect("setSelected", '');
        $("#SelectSisconcReport").UifSelect("setSelected", '');
        $("#InputQueryCifin").val('');
        $("#InputMainDebtor").val('0');
        $("#InputCodebtor").val('0');
        $("#InputTotal").val('0');
        $("#InputSuggestedQuota").val('0');
        $("#InputReconsiderationQuota").val('0');
       //glbAutomaticQuota = { Id: 0 };
        glbThird = { Id: 0 };
        utilitys = [];
        SummaryUtilitys = [];
        indicatortypeliquidity = [];
        indicatortypeindebtedness = [];
        indicatortypecosteffectiveness = [];
        indicatortypeActivity = [];
        dynamicProperties = [];
    }
    static Close() {
        window.location = rootPath + "Home/Index";
    }
    static GetAutomaticQuota() {
        var Id = $('#inputTemporalAutomaticQuota').val().trim();
        AutomaticQuotaRequest.GetAutomaticQuota(Id).done(function (data) {
            if (data.success) {
                if (data.result) {
                    //General
                    AutomaticQuotaId = data.result.AutomaticQuotaId;
                    $("#SelectDocumentType").UifSelect("setSelected", data.result.ProspecDTO.DocumentType);
                    $("#InputNIdentification").val(data.result.ProspecDTO.DocumentNumber);
                    $("#InputNameBusinnesName").val(data.result.ProspecDTO.BusinessName);
                    AutomaticQuota.GetCountryByCountryId(data.result.ProspecDTO.CountryCd);
                    AutomaticQuota.GetStatesByCountryIdByStateId(data.result.ProspecDTO.CountryCd, data.result.ProspecDTO.StateCd);
                    AutomaticQuota.GetCitiesByCountryIdByStateIdById(data.result.ProspecDTO.CountryCd, data.result.ProspecDTO.StateCd, data.result.ProspecDTO.City);
                    $("#InputFullAddress").val(data.result.ProspecDTO.Address);
                    $("#InputPhone").val(data.result.ProspecDTO.Phone);
                    $("#InputEmail").val(data.result.ProspecDTO.Email);
                    $("#InputDateConstitution").val(FormatDate(data.result.ProspecDTO.ConstitutionDate));
                    $("#InputLegalRepresentative").val(data.result.ProspecDTO.LegalRepresentative);
                    $("#InputReviewerLegal").val(data.result.ProspecDTO.FiscalReviewer);
                    if (data.result.ProspecDTO.EconomicActivity != 0) {
                        $("#InputCiiuEconomicActivity").val(data.result.ProspecDTO.EconomicActivity);
                        AutomaticQuota.GetEconomyActivity(data.result.ProspecDTO.EconomicActivity);
                    }
                    $("#InputAgent").val(data.result.Agent.Description);
                    $("#SelectProgram").UifSelect("setSelected", data.result.AgentProgramDTO.Id);
                    $("#SelectCountry").UifSelect("setSelected", data.result.CountryId);
                    if (data.result.StateId != 0) {
                        AutomaticQuotaRequest.GetStatesByCountryId(data.result.CountryId).done(function (data1) {
                            if (data1.success) {
                                if (data1.result) {
                                    $("#SelectDepartment").UifSelect({ sourceData: data1.result });
                                    $("#SelectDepartment").UifSelect("setSelected", data.result.StateId);
                                }
                            }
                        });
                    }
                    if (data.result.CountryId != 0 && data.result.StateId != 0) {
                        AutomaticQuotaRequest.GetCitiesByCountryIdStateId(data.result.CountryId, data.result.StateId).done(function (data2) {
                            if (data2.success) {
                                if (data2.result) {
                                    $("#SelectCity").UifSelect({ sourceData: data2.result });
                                    $("#SelectCity").UifSelect("setSelected", data.result.CityId);

                                }
                            }
                        });
                    }
                    $("#InputDepartament").on("search", AutomaticQuota.SearchState);
                    $("#SelectCity").UifSelect("setSelected", data.result.CityId);
                    $("#InputSuggestedQuota").val(data.result.SuggestedQuota);
                    $("#InputReconsiderationQuota").val(data.result.QuotaReConsideration);
                    $("#InputLegalQuota").val(data.result.LegalizedQuota);
                    $("#InputCurrentQuota").val(data.result.CurrentQuota);
                    $("#InputCurrentCumulu").val(data.result.CurrentCluster);
                    $("#InputElaborationQuotaDat").val(FormatDate(data.result.QuotaPreparationDate));
                    $("#InputForSolution").val(data.result.RequestedByName);
                    $("#InputElaboration").val(data.result.ElaboratedName);
                    $("#SelectCounterGuaranteeType").UifSelect("setSelected", data.result.TypeCollateral);
                    $("#SelectCounterGuaranteeState").UifSelect("setSelected", data.result.CollateralStatus);
                    $("#InputObservation").val(data.result.Observations);
                    //Terceros
                    glbThird = data.result.ThirdDTO;
                    if (data.result.ThirdDTO.RiskCenterDTO.Id != 0) {
                        $("#SelectconsultRiskCenters").UifSelect("setSelected", data.result.ThirdDTO.RiskCenterDTO.Id);
                    }
                    if (data.result.ThirdDTO.RestrictiveDTO.Id != 0) {
                        $("#SelectRestrictiveListQuery").UifSelect("setSelected", data.result.ThirdDTO.RestrictiveDTO.Id);
                    }
                    if (data.result.ThirdDTO.PromissoryNoteSignatureDTO.Id != 0) {
                        $("#SelectSignaturePromissoryNoteLetterIntent").UifSelect("setSelected", data.result.ThirdDTO.PromissoryNoteSignatureDTO.Id);
                    }
                    if (data.result.ThirdDTO.ReportListSisconcDTO.Id != 0) {
                        $("#SelectSisconcReport").UifSelect("setSelected", data.result.ThirdDTO.ReportListSisconcDTO.Id);
                    }
                    $("#InputQueryCifin").val(FormatDate(data.result.ThirdDTO.CifinQuery));
                    $("#InputMainDebtor").val(data.result.ThirdDTO.PrincipalDebtor);
                    $("#InputCodebtor").val(data.result.ThirdDTO.Cosigner);
                    $("#InputTotal").val(data.result.ThirdDTO.Total);

                    //Utilidades
                    if (data.result.UtilityDTO != null) {
                        utilitys = [];
                        SummaryUtilitys = [];
                        data.result.UtilityDTO.forEach(function (item, index) {
                            var UtilitysSelected = {
                                Id: item.Id,
                                Description: item.Description,
                                DateIni: parseFloat(item.Start_Values).toFixed(2),
                                DateFin: parseFloat(item.End_value).toFixed(2),
                                AbsoludValue: item.Var_Abs,
                                RelativeValue: item.Var_Relativa + '%',
                                UtilityType: item.UtilityDetails.UtilitysTypeCd,
                                UtilitySummary: item.UtilityDetails.UtilitysSummaryCd,
                                FormUtilitys: item.UtilityDetails.FormUtilitys,
                                utilityId: item.UtilityDetails.utilityId
                            };
                            if (item.Id >= 1 && item.Id <= 17) {
                                utilitys.push(UtilitysSelected);
                            } else if (item.Id >= 17) {
                                SummaryUtilitys.push(UtilitysSelected);
                            }
                        })
                    }
                    //Indicadores
                    if (data.result.indicatorDTO != null) {
                        AutomaticQuota.GetDefaultIndicator();
                        data.result.indicatorDTO.forEach(function (item, index) {
                            indicatortypeliquidity.forEach(function (item1, index) {
                                if (item1.Id == item.ConceptIndicatorCd) {
                                    item1.DateIni = item.IndicatorIni;
                                    item1.DateFin = item.IndicatorFin;
                                    item1.Observation = item.Observation;
                                }
                            });
                            indicatortypeindebtedness.forEach(function (item2, index) {
                                if (item2.Id == item.ConceptIndicatorCd) {
                                    item2.DateIni = item.IndicatorIni;
                                    item2.DateFin = item.IndicatorFin;
                                    item2.Observation = item.Observation;
                                }
                            });
                            indicatortypecosteffectiveness.forEach(function (item3, index) {
                                if (item3.Id == item.ConceptIndicatorCd) {
                                    item3.DateIni = item.IndicatorIni;
                                    item3.DateFin = item.IndicatorFin;
                                    item3.Observation = item.Observation;
                                }
                            });
                            indicatortypeActivity.forEach(function (item4, index) {
                                if (item4.Id == item.ConceptIndicatorCd) {
                                    item4.DateIni = item.IndicatorIni;
                                    item4.DateFin = item.IndicatorFin;
                                    item4.Observation = item.Observation;
                                }
                            });
                        });
                    }
                    //Dynamic Property
                    if (data.result.DynamicProperties != null) {
                        dynamicProperties = data.result.DynamicProperties;
                    }
                }
            }
            else {
                $.UifDialog('alert', { 'message': data.result });
            }
        });
    }
}
