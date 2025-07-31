var causeId = null;
class ClaimLocation extends Uif2.Page {

    getInitialState() {
        $("#selectEndorsement").parent().parent().hide();
        ClaimLocation.InitialClaimLocation();
    }

    bindEvents() {
        // Buttons -  EstimationsPartial
        $("#btnSaveClaim").on('click', ClaimLocation.CreateClaimLocation);

        $('#tableRiskLocation').on('rowSelected', function (event, data, position) {
            ClaimEstimation.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(data);
        });
        $('#rdbtnInsured').on('click', ClaimVehicle.ClickRdbtnInsured);
        $('#rdbtnThirdParty').on('click', ClaimVehicle.ClickdRbtnThirdParty);
        $('#tableRiskLocation').on('rowDeselected', function (event, data, position) {
            ClaimEstimation.DeselectedRisk(data);
        });
        $('#tableSubSinisterLocation').on('rowEdit', ClaimEstimation.EditSubClaims);
        $('#tableSubSinisterLocation').on('rowDelete', ClaimEstimation.DeleteSubclaims);
    }

    static InitialClaimLocation() {
        if (noticeToClaim == null && modelSearchCriteria.claimId == null) {
            ClaimLocation.GetPrefixesByCoveredRiskType();
        }
        Claim.GetBranches();
        
        $('#selecDamageClassification').parent().parent().hide();
        $('#selectDamageResponsibility').parent().parent().hide();
        $('#titleClaimLocation').show();

        ClaimLocation.DoConvertNoticeLocationToClaim();
    }

    static GetPrefixesByCoveredRiskType(callback) {
        ClaimLocationRequest.GetPrefixesByCoveredRiskType().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static DoConvertNoticeLocationToClaim() {
        if (noticeToClaim == null && modelSearchNewClaimByNotice.claimBranch == null)
            return;

        //SETEO DE CONSTANTES
        noticeId = noticeToClaim.Id;

        //Cargamos las cobertuas
        NoticeRequest.GetNoticeByNoticeId(noticeId).done(function (data) {
            if (data.success) {
                noticeToClaim.Coverages = data.result.Coverages;
            }
        });

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
        
        $("#ClaimLocation").val(noticeToClaim.Location);
        $("#ddlCountry").UifSelect('setSelected', noticeToClaim.CountryId);

        Claim.GetStatesByCountryId(noticeToClaim.CountryId, function (states) {
            $("#ddlState").UifSelect({ sourceData: states });
            $("#ddlState").UifSelect('setSelected', noticeToClaim.StateId);
        });

        Claim.GetCitiesByCountryIdStateId(noticeToClaim.CountryId, noticeToClaim.StateId, function (cities) {
            $("#ddlCity").UifSelect({ sourceData: cities });
            $("#ddlCity").UifSelect('setSelected', noticeToClaim.CityId);
            Claim.LockClaimLocation(true);
        });

        ClaimRequest.GetPolicyByEndorsementIdModuleType(noticeToClaim.EndorsementId).done(function (response) {
            $("#selectBranch").UifSelect('setSelected', response.result.BranchId);
            $("#selectBranch").UifSelect('disabled', true);
            $("#InputPolicyNumber").val(response.result.DocumentNumber);
            $("#InputPolicyNumber").UifInputSearch('disabled', true);

            ClaimLocation.GetPrefixesByCoveredRiskType(function (prefixes) {
                $('#selectPrefix').UifSelect({ sourceData: prefixes });
                $('#selectPrefix').UifSelect('setSelected', response.result.PrefixId);
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

    static preLoadClaimLocation() {

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
                $("#ClaimLocation").val(response.result.Location);
                $("#ddlCountry").UifSelect('setSelected', response.result.CountryId);
                Claim.GetStatesByCountryId(response.result.CountryId, function (states) {
                    $("#ddlState").UifSelect({ sourceData: states });
                    $("#ddlState").UifSelect('setSelected', response.result.StateId);
                });
                Claim.GetCitiesByCountryIdStateId(response.result.CountryId, response.result.StateId, function (cities) {
                    $("#ddlCity").UifSelect({ sourceData: cities });
                    $("#ddlCity").UifSelect('setSelected', response.result.CityId);
                });

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

                $("#locationClaimNumberEdit").show();
                $("#locationClaimNumberEdit").text($("#locationClaimNumberEdit").text() + response.result.Number);

                ClaimRequest.GetPolicyByEndorsementIdModuleType(modelSearchCriteria.endorsementId).done(function (response) {
                    if (response.success) {
                        var policyBranchId = response.result.BranchId;
                        var policyPrefixId = response.result.PrefixId;
                        policyCurrency = response.result.CurrencyId;
                        $('#selectBranch').UifSelect('setSelected', policyBranchId);
                        $('#InputPolicyNumber').val(response.result.DocumentNumber);

                        ClaimLocation.GetPrefixesByCoveredRiskType(function (prefixes) {
                            $('#selectPrefix').UifSelect({ sourceData: prefixes, enable: false });
                            $('#selectPrefix').UifSelect('setSelected', policyPrefixId);

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

    static GetRisksByEndorsementId(endorsementId) {
        ClaimLocationRequest.GetRisksByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#tableRiskLocation").UifDataTable({ sourceData: data.result });

                    //Convertimos cobertuas del aviso en estimaciones
                    if (noticeToClaim != null) {
                        Claim.ConvertNoticeCoverageToEstimation(data.result);
                    }

                   
                    $("#_individual").val(data.result.InsuredId);
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EndorsementDontHaveRisks, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static CreateClaimLocation() {
        $("#formClaimLocation").validate();
        $("#formClaim").validate();
        if (!$("#formClaimLocation").valid() || !$("#formClaim").valid()) {
            ScrollTop();
            return;
        }
        if (policyConsulted && Claim.ValidateTableSummaryClaim()) {
            lockScreen();
            var claimLocationDTO = BuildExecuteClaim();

            ClaimLocationRequest.ExecuteClaimOperations(claimLocationDTO).done(function (data) {
                if (data.success) {
                    LaunchPolicies.ValidateInfringementPolicies(data.result.AuthorizationPolicies, true);
                    var countAutorizationPolicies = data.result.AuthorizationPolicies.filter(x => x.Type == TypeAuthorizationPolicies.Authorization || x.Type == TypeAuthorizationPolicies.Restrictive).length;
                    if (countAutorizationPolicies > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.AuthorizationPolicies, data.result.TemporalId, FunctionType.Claim);
                    } else {
                        $.UifDialog('alert', {
                            message: String.format(Resources.Language.MessageClaimCreateSuccess, $("#selectPrefix").UifSelect("getSelectedText").substring(0, 3), $("#InputPolicyNumber").val(), data.result.Number)
                        }, function (result) { });
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
}