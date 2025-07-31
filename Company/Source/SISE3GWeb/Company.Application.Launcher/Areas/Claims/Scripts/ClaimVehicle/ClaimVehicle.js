var vehicleColors = null;
var causeId = null;

class ClaimVehicle extends Uif2.Page {
    getInitialState() {
        $("#selectEndorsement").parent().parent().hide();
        ClaimVehicle.InitialClaimVehicle();
    }

    bindEvents() {
        // Buttons -  EstimationsPartial
        $("#btnSaveClaim").on('click', ClaimVehicle.CreateClaimVehicle);

        // DataTables
        $('#tableRiskVehicle').on('rowSelected', function (event, data, position) {
            ClaimEstimation.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(data);
        });

        $('#tableSubSinisterVehicle').on('rowEdit', ClaimEstimation.EditSubClaims);

        $('#tableSubSinisterVehicle').on('rowDelete', ClaimEstimation.DeleteSubclaims);
        $('#tableRiskVehicle').on('rowDeselected', function (event, data, position) {
            ClaimEstimation.DeselectedRisk(data);
        });
    }

    static InitialClaimVehicle() {
        if (noticeToClaim == null && modelSearchCriteria.claimId == null) {
            ClaimVehicle.GetPrefixesByCoveredRiskType();
        }

        Claim.GetBranches();

        ClaimVehicleRequest.GetVehicleColors().done(function (data) {
            if (data.success) {
                $('#selectVehicleColor').UifSelect({ sourceData: data.result });
                vehicleColors = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        ClaimVehicle.DoConvertNoticeVehicleToClaim();

        $('#rdbtnThirdParty').prop('disabled', true);
        $('#rdbtnInsured').prop('disabled', true);
        $('#titleClaimVehicle').show();
    }

    static GetPrefixesByCoveredRiskType(callback) {
        ClaimVehicleRequest.GetPrefixesByCoveredRiskType().done(function (data) {
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

    ////////////////////////////////////
    /// Sección Consulta Estimaciones
    static GetRisksByEndorsementId(endorsementId) {

        ClaimVehicleRequest.GetRisksByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {

                    $("#tableRiskVehicle").UifDataTable({ sourceData: data.result });
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

    static selectedDriverDocumentNumber(event, selectedItem) {
        if (selectedItem != null) {
            $('#inputDriverFullName').val(selectedItem.FullName);
            ClaimVehicle.loadDataDriverDocument(selectedItem.DocumentNumber);
        }
    }

    static selectedDriverFullName(event, selectedItem) {
        if (selectedItem != null) {
            $('#inputDriverDocumentNumber').val(selectedItem.DocumentNumber);
            ClaimVehicle.loadDataDriverDocument(selectedItem.DocumentNumber);
        }
    }

    static loadDataDriverDocument(documentNumber) {
        ClaimVehicleRequest.GetDriverByDocumentNumber(documentNumber).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#inputDriverLicenseType').val(data.result.LicenseNumber);
                    $('#inputDriverDataLicenseNumber').val(data.result.LicenseType);
                    $('#inputDriverDataVto').val(FormatDate(data.result.LicenseValidThru));
                    $('#inputDataAge').val(data.result.Age);
                    $('#inputDriverLicenseType').prop('disabled', true);
                    $('#inputDriverDataLicenseNumber').prop('disabled', true);
                    $('#inputDriverDataVto').prop('disabled', true);
                    $('#inputDataAge').prop('disabled', true);
                } else {
                    $('#inputDriverLicenseType').val('');
                    $('#inputDriverDataLicenseNumber').val('');
                    $('#inputDriverDataVto').val('');
                    $('#inputDataAge').val('');
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.DontExistDrivers, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static selectedThirdDocumentNumber(event, selectedItem) {
        if (selectedItem != null) {
            $('#inputThirdDataFullName').val(selectedItem.FullName);
        } else {
            ClaimVehicle.createThird();
        }
    }

    static selectedThirdDataFullName(event, selectedItem) {
        if (selectedItem != null) {
            $('#inputThirdDocumentNumber').val(selectedItem.DocumentNumber);
        } else {
            ClaimVehicle.createThird();
        }
    }

    static selectedThirdVehiclePlate(selectedItem) {

        ClaimVehicleRequest.GetVehicleByLicensePlate(selectedItem.Description).done(function (data) {
            if (data.success) {
                $('#inpuTthirdVehiclePlate').val(data.result.Plate);
                $('#inpuTVehicleMake').val(data.result.Make);
                $('#inpuTVehicleModel').val(data.result.Model);
                $('#inpuTthirdVehicleYear').val(data.result.Year);
                var color = vehicleColors.find(function (colors) {
                    return colors.Description === data.result.Color;
                });
                $('#selectVehicleColor').UifSelect('setSelected', vehicleColors.Id);
                $('#inpuTVehicleEngine').val(data.result.Motor);
                $('#inpuTVehicleChasis').val(data.result.Chasis);
                $('#inputVehicleVIN').val("");

                //////////////////////////////////////////////////

                $('#inpuTVehicleMake').prop('disabled', true);
                $('#inpuTVehicleModel').prop('disabled', true);
                $('#inpuTthirdVehicleYear').prop('disabled', true);
                $('#selectVehicleColor').prop('disabled', true);
                $('#inpuTVehicleEngine').prop('disabled', true);
                $('#inpuTVehicleChasis').prop('disabled', true);
                $('#inputVehicleVIN').prop('disabled', true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static DoConvertNoticeVehicleToClaim() {
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

        $("#selecDamageClassification").UifSelect('setSelected', noticeToClaim.DamageTypeId);
        $("#selecDamageClassification").UifSelect('disabled', true);
        $("#selectDamageResponsibility").UifSelect('setSelected', noticeToClaim.DamageResponsibilityId);
        $("#selectDamageResponsibility").UifSelect('disabled', true);
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

        $("#InputClaimDetail").val(noticeToClaim.Description);

        ClaimRequest.GetPolicyByEndorsementIdModuleType(noticeToClaim.EndorsementId).done(function (response) {
            $("#selectBranch").UifSelect('setSelected', response.result.BranchId);
            $("#selectBranch").UifSelect('disabled', true);
            $("#InputPolicyNumber").val(response.result.DocumentNumber);
            $("#InputPolicyNumber").UifInputSearch('disabled', true);

            ClaimVehicle.GetPrefixesByCoveredRiskType(function (prefixes) {
                $('#selectPrefix').UifSelect({ sourceData: prefixes });
                $('#selectPrefix').UifSelect('setSelected', response.result.PrefixId);
                $("#selectPrefix").UifSelect('disabled', true);

                Claim.GetEndorsementsByPrefixIdBranchIdPolicyNumber();
            });

            Claim.GetCausesByPrefixId(response.result.PrefixId, function (causes) {
                $("#selectCauseClaim").UifSelect({ sourceData: causes });

                $("#selectClaimCurrency").UifSelect("setSelected", "0");

                //Cargamos las cobertuas
                NoticeRequest.GetNoticeByNoticeId(noticeId).done(function (data) {
                    if (data.success) {
                        noticeToClaim.Coverages = data.result.Coverages;
                    }
                });

            });
        });
    }

    ///////////////////////////////////////
    /// Consulta la Denuncia desde la pantalla Busqueda
    static preLoadClaimVehicle() {

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
                $('#InputClaimDetail').val(response.result.Description);
                claimModifyId = response.result.Modifications[0].ClaimModifyId;
                causeId = response.result.CauseId;
                modelSearchCriteria.endorsementId = response.result.EndorsementId;
                $("#selecDamageClassification").UifSelect('setSelected', response.result.DamageTypeId);
                $("#selectDamageResponsibility").UifSelect('setSelected', response.result.DamageResponsabilityId);
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

                $("#vehicleClaimNumberEdit").show();
                $("#vehicleClaimNumberEdit").text($("#vehicleClaimNumberEdit").text() + response.result.Number);

                ClaimRequest.GetPolicyByEndorsementIdModuleType(modelSearchCriteria.endorsementId).done(function (response) {
                    if (response.success) {
                        var policyBranchId = response.result.BranchId;
                        var policyPrefixId = response.result.PrefixId;
                        policyCurrency = response.result.CurrencyId;
                        $('#selectBranch').UifSelect('setSelected', policyBranchId);
                        $('#InputPolicyNumber').val(response.result.DocumentNumber);

                        ClaimVehicle.GetPrefixesByCoveredRiskType(function (prefixes) {
                            $('#selectPrefix').UifSelect({ sourceData: prefixes, enable: false });
                            $('#selectPrefix').UifSelect('setSelected', policyPrefixId);

                            ClaimEstimation.GetEstimationsByClaimId(modelSearchCriteria.claimId);
                        });

                        Claim.GetCausesByPrefixId(policyPrefixId, function (causes) {
                            $("#selectCauseClaim").UifSelect({ sourceData: causes });
                            if (causeId > 0) {
                                $("#selectCauseClaim").UifSelect('setSelected', causeId);
                            }
                        });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    ///////////////////////////////////////
    /// Crear la Denuncia del Vehiculo
    static CreateClaimVehicle() {
        $("#formClaimLocation").validate();
        $("#formClaim").validate();
        if (!$("#formClaimLocation").valid() || !$("#formClaim").valid()) {
            ScrollTop();
            return;
        }
        if (policyConsulted && Claim.ValidateTableSummaryClaim()) {
            lockScreen();
            var claimVehicleDTO = BuildExecuteClaim();

            ClaimVehicleRequest.ExecuteClaimOperations(claimVehicleDTO).done(function (data) {
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

    static ClearDataDriver() {
        $('#inpuTthirdVehiclePlate').val("");
        $('#inpuTVehicleMake').val("");
        $('#inpuTVehicleModel').val("");
        $('#inpuTthirdVehicleYear').val("");
        $('#selectVehicleColor').UifSelect('setSelected', -1);
        $('#inpuTVehicleEngine').val("");
        $('#inpuTVehicleChasis').val("");
        $('#inputVehicleVIN').val("");
        $('#textareaObservations').val("");
        $('#inputDriverDocumentNumber').val("");
        $('#inputDriverFullName').val("");
        $('#inputDriverLicenseType').val("");
        $('#inputDriverDataLicenseNumber').val("");
        $('#inputDriverDataVto').val("");
        $('#inputDataAge').val("");

        // Habilitar Campos
        $('#inpuTthirdVehiclePlate').prop('disabled', false);
        $('#inpuTVehicleMake').prop('disabled', false);
        $('#inpuTVehicleModel').prop('disabled', false);
        $('#inpuTthirdVehicleYear').prop('disabled', false);
        $('#selectVehicleColor').prop('disabled', false);
        $('#inpuTVehicleEngine').prop('disabled', false);
        $('#inpuTVehicleChasis').prop('disabled', false);
        $('#inputVehicleVIN').prop('disabled', false);
        $('#textareaObservations').prop('disabled', false);
        $('#inputDriverDocumentNumber').prop('disabled', false);
        $('#inputDriverFullName').prop('disabled', false);
        $('#inputDriverLicenseType').prop('disabled', false);
        $('#inputDriverDataLicenseNumber').prop('disabled', false);
        $('#inputDriverDataVto').prop('disabled', false);
        $('#inputDataAge').prop('disabled', false);
    }
}