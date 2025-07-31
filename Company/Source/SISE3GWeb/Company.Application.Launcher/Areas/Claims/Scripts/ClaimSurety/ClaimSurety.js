var currentEditConceptIndex = 0;
var currentPaymentConceptId = 0;
var rowsConcept = [];
var chargeRequest = {};
var currentRecoveryOrSalvage = {};
var currentPositionRecoveryOrSalvage = 0;
var individualId = 0;
var causeId = null;
class ClaimSurety extends Uif2.Page {

    getInitialState() {
        $("#selecDamageClassification").parent().parent().hide();
        $("#selectDamageResponsibility").parent().parent().hide();
        $('#titleClaimSurety').show();
        $("#formClaimLocation").hide();
        $("#ClaimLocation").parent().hide();
        ClaimSurety.InitialSurety();
    }

    /** EVENTOS**/
    bindEvents() {
        $('#btnCloseEstimation').on('click', ClaimSurety.CloseEstimation);
        $("#btnSaveClaim").on('click', ClaimSurety.CreateClaimSurety);

        $('#tableRiskSurety').on('rowSelected', function (event, data, position) {
            ClaimEstimation.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(data);
        });
        $('#rdbtnInsured').on('click', ClaimVehicle.ClickRdbtnInsured);
        $('#rdbtnThirdParty').on('click', ClaimVehicle.ClickdRbtnThirdParty);
        $('#tableRiskSurety').on('rowDeselected', function (event, data, position) {
            ClaimEstimation.DeselectedRisk(data);
        });

        $('#tableSubSinisterSurety').on('rowEdit', ClaimEstimation.EditSubClaims);
        $('#tableSubSinisterSurety').on('rowDelete', ClaimEstimation.DeleteSubclaims);
    }

    /** METODOS GET**/
    static InitialSurety() {
        if (noticeToClaim == null && modelSearchCriteria.claimId == null) {
            ClaimSurety.GetPrefixesByCoveredRiskType();
        }       
        Claim.GetBranches();
        ClaimSurety.DoConvertNoticeSuretyToClaim();
    }

    static GetPrefixesByCoveredRiskType(callback) {
        ClaimSuretyRequest.GetPrefixesByCoveredRiskType().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
                var suretyPrefix = [];
                $.each(data.result, function (index, value) {
                    if (this.Id != 1) {
                        suretyPrefix.push(value);
                    }
                });
                $('#selectPrefix').UifSelect({ sourceData: suretyPrefix });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static DoConvertNoticeSuretyToClaim() {
        if (noticeToClaim == null && modelSearchNewClaimByNotice.claimBranch == null)
            return;

        //SETEO DE CONSTANTES
        noticeId = noticeToClaim.Id;
        $('#_endorsementId').val(noticeToClaim.EndorsementId);
        $('#_riskId').val(noticeToClaim.RiskId);

        $("#InputDateIncident").UifDatepicker('setValue', noticeToClaim.OcurrenceDate);
        $("#InputDateIncident").UifDatepicker('disabled', true);
        $("#InputDateNotice").UifDatepicker('setValue', noticeToClaim.NoticeDate);
        $('#InputDateNotice').UifDatepicker('disabled', true);
        $("#selectBranchClaim").UifSelect('setSelected', modelSearchNewClaimByNotice.claimBranch);
        $("#selectBranchClaim").UifSelect('disabled', true);
        $("#inputClaimedAmount").val(FormatMoney(noticeToClaim.ClaimedAmount));

        $("#InputClaimDetail").val(noticeToClaim.Description);

        ClaimRequest.GetPolicyByEndorsementIdModuleType(noticeToClaim.EndorsementId).done(function (response) {
            Claim.GetJudicialDecisionDateIsActiveByPrefixId(response.result.PrefixId);
            $("#selectBranch").UifSelect('setSelected', response.result.BranchId);
            $("#selectBranch").UifSelect('disabled', true);
            $("#InputPolicyNumber").val(response.result.DocumentNumber);
            $("#InputPolicyNumber").UifInputSearch('disabled', true);

            ClaimSurety.GetPrefixesByCoveredRiskType(function (prefixes) {
                $("#selectPrefix").UifSelect({ sourceData: prefixes });
                $("#selectPrefix").UifSelect('setSelected', response.result.PrefixId);
                $("#selectPrefix").UifSelect('disabled', true);

                Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
            });

            Claim.GetCausesByPrefixId(response.result.PrefixId, function (causes) {
                $("#selectCauseClaim").UifSelect({ sourceData: causes });

                //Cargamos las cobertuas
                NoticeRequest.GetNoticeByNoticeId(noticeId).done(function (data) {
                    if (data.success) {
                        noticeToClaim.Coverages = data.result.Coverages;
                    }
                });
            });
        });
    }

    static preLoadClaimSurety() {

        if (modelSearchCriteria.claimId == null)
            return;

        $('#InputDateNotice').UifDatepicker('disabled', true);
        $('#InputDateIncident').UifDatepicker('disabled', true);
        $('#InputTimeIncident').UifTimepicker('disabled', true);
        $('#selectBranchClaim').UifSelect('disabled', true);
        $('#selectBranch').UifSelect('disabled', true);
        $('#selectPrefix').UifSelect('disabled', true);
        $('#InputPolicyNumber').UifInputSearch('disabled', true);
        $("#selectClaimCurrency").UifSelect("disabled", true);

        ClaimRequest.GetClaimByClaimId(modelSearchCriteria.claimId).done(function (response) {
            if (response.success) {
                $("#InputClaimDetail").val(response.result.Description);
                claimModifyId = response.result.Modifications[0].ClaimModifyId;
                causeId = response.result.CauseId;
                modelSearchCriteria.endorsementId = response.result.EndorsementId;

                $("#InputDateNotice").UifDatepicker('setValue', moment(response.result.NoticeDate).format("DD/MM/YYYY"));
                $('#selectBranchClaim').UifSelect('setSelected', response.result.BranchId);
                $("#InputDateIncident").UifDatepicker('setValue', moment(response.result.OccurrenceDate).format("DD/MM/YYYY"));
                $("#InputTimeIncident").UifTimepicker('setValue', moment(response.result.OccurrenceDate).format("HH:mm"));
                $("#InputJudicialDecisionDate").UifDatepicker('setValue', FormatDate(response.result.JudicialDecisionDate));

                $("#rdbTotal").prop("checked", response.result.IsTotalParticipation);
                $("#rdbCompanyParticipation").prop("checked", !response.result.IsTotalParticipation);

                // Seteo de Constantes
                $('#_endorsementId').val(response.result.EndorsementId);
                $('#_claimId').val(response.result.ClaimId);
                $('#_number').val(response.result.Number);

                $("#suretyClaimNumberEdit").show();
                $("#suretyClaimNumberEdit").text($("#suretyClaimNumberEdit").text() + response.result.Number);

                ClaimRequest.GetPolicyByEndorsementIdModuleType(modelSearchCriteria.endorsementId).done(function (response) {
                    if (response.success) {
                        Claim.GetJudicialDecisionDateIsActiveByPrefixId(response.result.PrefixId);
                        var policyBranchId = response.result.BranchId;
                        var policyPrefixId = response.result.PrefixId;
                        policyCurrency = response.result.CurrencyId;
                        $('#selectBranch').UifSelect('setSelected', policyBranchId);
                        $('#InputPolicyNumber').val(response.result.DocumentNumber);
                                                                        
                        var endorsementList = [];
                        var objEndorsement = { Id: response.result.EndorsementId, Description: response.result.EndorsementDocumentNum };
                        endorsementList.push(objEndorsement);
                        $("#selectEndorsement").UifSelect({ sourceData: endorsementList});
                        $("#selectEndorsement").UifSelect('setSelected', modelSearchCriteria.endorsementId);
                        $("#selectEndorsement").UifSelect('disabled', true);

                        ClaimSurety.GetPrefixesByCoveredRiskType(function (prefixes) {
                            $("#selectPrefix").UifSelect({ sourceData: prefixes, enable: false });
                            $("#selectPrefix").UifSelect('setSelected', policyPrefixId);

                            ClaimEstimation.GetEstimationsByClaimId(modelSearchCriteria.claimId);
                        });

                        Claim.GetCausesByPrefixId(policyPrefixId, function (causes) {
                            $("#selectCauseClaim").UifSelect({ sourceData: causes });
                            $("#selectCauseClaim").UifSelect('setSelected', causeId);
                        });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetRisksByEndorsementIdPrefixId(endorsementId, prefixId) {

        ClaimSuretyRequest.GetRisksByEndorsementIdPrefixId(endorsementId, prefixId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    if (data.result[0].CourtNum == null) {
                        data.result[0].CourtNum = data.result[0].ArticleDescription;
                    }

                    $("#tableRiskSurety").UifDataTable({ sourceData: data.result });

                    //Convertimos cobertuas del aviso en estimaciones
                    if (noticeToClaim != null) {
                        Claim.ConvertNoticeCoverageToEstimation(data.result);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EndorsementDontHaveRisks, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }    

    static CreateClaimSurety() {
        $("#formClaim").validate();
        if (!$("#formClaim").valid()) {
            ScrollTop();
            return;
        }

        if (policyConsulted && Claim.ValidateTableSummaryClaim()) {
            lockScreen();
            var claimSuretyDTO = BuildExecuteClaim();

            ClaimSuretyRequest.ExecuteClaimOperations(claimSuretyDTO).done(function (data) {
                if (data.success) {
                    LaunchPolicies.ValidateInfringementPolicies(data.result.AuthorizationPolicies, true);
                    var countAutorizationPolicies = data.result.AuthorizationPolicies.filter(x => x.Type == TypeAuthorizationPolicies.Authorization || x.Type == TypeAuthorizationPolicies.Restrictive).length;
                    if (countAutorizationPolicies > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.AuthorizationPolicies, data.result.TemporalId, FunctionType.Claim);
                    } else {
                        $.UifDialog('alert', {
                            message: String.format(Resources.Language.MessageClaimCreateSuccess, $("#selectPrefix").UifSelect("getSelectedText").substring(0, 3), $("#InputPolicyNumber").val(), data.result.Number)
                        }, function (result) {
                        });
                        Claim.cleanForm();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }                
            }).always(function () {
                unlockScreen();
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UnlessACoverage, 'autoclose': true });
        }
    }

    static CloseEstimation() {
        $('#titleEstimation').hide();
        $('#claimGeneral').show();
        $('#titleClaim').show();
        $('#btnSaveClaim').show();
        $('#claimEstimation').hide();
        $('#btnCloseEstimation').hide();
    }
}