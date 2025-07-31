var fidelityCoveredRiskType = 7;
class NoticeFidelity extends Uif2.Page {

    getInitialState() {
        NoticeFidelity.InitialNoticeFidelity();
    }

    bindEvents() {        
        $('#ClaimedAmount').ValidatorKey(ValidatorType.Number, 2, 1);
        $("#btnSaveClaimNotice").on('click', NoticeFidelity.ExecuteNoticeOperations);
        $('#tableFidelityRisk').on('rowSelected', function (event, data, position) {
            NoticeFidelity.selectDataFromModalToForm(event, data);
        });
        $('#ObjectedDescription').ValidatorKey(ValidatorType.Number, 2, 1);

    }

    /////////////////////////////////////////////////
    // LoadPage()
    static InitialNoticeFidelity() {
        $("#coveragespanel").hide();

        if (modelSearchNotice.claimNoticeId == null && modelSearchNewNotice.riskId == null) {
            NoticeFidelity.GetRiskCommercialClasses();
            NoticeFidelity.GetOccupations();
        }

        NoticeFidelity.preLoadNoticeFidelity();

        if (modelSearchNewNotice.riskId != null && modelSearchNewNotice.endorsementId != null) {
            NoticeFidelity.ToNoticeFidelityByPolicy();
        }

        //Se ocultan campos innecesarios para pantalla de manejo de lo general del aviso ClaimNotice.cshtml
        $("#inputdriverInformation").parent().hide();
        $("#OthersAffected").parent().hide();
        $("#RadioNotice").parent().parent().parent().hide();
    }

    static GetRiskCommercialClasses(callback) {
        NoticeFidelityRequest.GetRiskCommercialClasses().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlActivityRisk").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetOccupations(callback) {
        NoticeFidelityRequest.GetOccupations().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlOccupation").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    /////////////////////////////////////////////////
    // Consulta el Aviso desde la pantalla Busqueda
    static preLoadNoticeFidelity() {
        if (modelSearchNotice.claimNoticeId == null)
            return;

        NoticeRequest.GetNoticeByNoticeId(modelSearchNotice.claimNoticeId).done(function (data) {
            if (data.success) {
                _endorsementId = data.result.EndorsementId;
                individualId = data.result.IndividualId;
                _coveredRiskType = data.result.CoveredRiskType;
                riskId = data.result.RiskId;
                noticeId = data.result.Id;//
                noticeStateId = data.result.NoticeStateId;//

                $('#Description').val(data.result.Description);
                $('#ObjectedDescription').val(data.result.NumberObjected);
                $("#Number").val(data.result.Number);
                $("#_noticeDate").UifDatepicker(FormatDate(data.result.NoticeDate));
                $('#Location').val(data.result.Location);
                $("#Date").val(FormatDate(data.result.OcurrenceDate));

                $('#Country').UifSelect('setSelected', data.result.CountryId);

                Notice.GetStatesByCountryId(data.result.CountryId);
                $('#State').UifSelect('setSelected', data.result.StateId);

                Notice.GetCitiesByCountryIdStateId(data.result.CountryId, data.result.StateId);
                $('#City').UifSelect('setSelected', data.result.CityId);

                $('#inputContactInformationName').val(data.result.ContactName);
                $('#inputContactInformationPhone').val(data.result.PhoneNumber);
                $('#inputContactInformationMail').val(data.result.Email);
                $("#OthersAffected").val(data.result.OthersAffected);//
                $("#inputInternalConsecutive").val(data.result.InternalConsecutive);
                $("#ClaimedAmount").val(FormatMoney(data.result.ClaimedAmount));//
                $("#inputdriverInformation").val(data.result.ClaimReasonOthers);//
                switch (data.result.NoticeReasonId) {//
                    case 1:
                        $("#RadioNotice").attr("checked", true);
                        break;
                    case 2:
                        $("#RadioNotice1").attr("checked", true);
                        break;
                    case 3:
                        $("#RadioNotice2").attr("checked", true);
                        break;
                }

                if (riskId == 0) {
                    NoticeFidelity.GetRiskFidelityByClaimNoticeId(data.result.Id);
                } else {
                    Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);
                    NoticeFidelity.GetRiskFidelityByRiskId(riskId);
                }

                setTimeout(function () {
                    data.result.Coverages.forEach(item => {

                        $("#ddlEstimationType").val(item.EstimateTypeId);
                        $("#victim").val((item.IsInsured) ? TypeVictim.Insured : TypeVictim.Third);

                        var dataCoverage = {
                            'AffectedType': $("#victim").UifSelect("getSelectedText"),
                            'Name': item.FullName,
                            'EstimationTypeDescription': $("#ddlEstimationType").UifSelect("getSelectedText"),
                            'EstimationTypeId': item.EstimateTypeId,
                            'CoverageName': item.CoverageName,
                            'EstimatedValue': item.EstimateAmount,
                            'InsuredAmount': item.InsurableAmount,
                            'IsInsured': item.IsInsured,
                            'IsProspect': item.IsProspect,
                            'RiskNum': data.result.RiskNumber,
                            'CoverageNumber': item.CoverNum,
                            'CoverageId': item.CoverageId,
                            'DocumentTypeId': item.DocumentTypeId,
                            'DocumentNumber': item.DocumentNumber,
                            'IndividualId': item.IndividualId,
                            'CurrencyId': policyCurrencyId,
                            'CurrencyDescription': policyCurrencyDescription
                        };

                        $("#coveragesTable").UifDataTable('addRow', dataCoverage);        //debe ir el id de la cobertura
                    });
                }, 3000); 
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ToNoticeFidelityByPolicy() {
        _endorsementId = modelSearchNewNotice.endorsementId;
        _coveredRiskType = airCraftCoveredRiskType;
        riskId = modelSearchNewNotice.riskId;

        NoticeFidelity.GetRiskFidelityByRiskId(modelSearchNewNotice.riskId);
        Notice.GetPolicyByEndorsementIdModuleType(modelSearchNewNotice.endorsementId);
    }

    static GetRiskFidelityByClaimNoticeId(claimNoticeId) {
        NoticeFidelityRequest.GetRiskFidelityByClaimNoticeId(claimNoticeId).done(function (data) {
            if (data.success) {
                NoticeFidelity.GetRiskCommercialClasses(function (riskCommercialClasess) {
                    $("#ddlActivityRisk").UifSelect({ sourceData: riskCommercialClasess });
                    $("#ddlActivityRisk").UifSelect('setSelected', data.result.CommercialClassId);
                });

                NoticeFidelity.GetOccupations(function (occupations) {
                    $("#ddlOccupation").UifSelect({ sourceData: occupations });
                    $("#ddlOccupation").UifSelect('setSelected', data.result.OccupationId);
                });

                $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(data.result.DiscoveryDate));
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskFidelityByRiskId(riskId) {
        NoticeFidelityRequest.GetRiskFidelityByRiskId(riskId).done(function (data) {
            if (data.success) {
                Notice.GetInsuredsByIndividualId(selectedRow.InsuredId);

                NoticeFidelity.GetRiskCommercialClasses(function (riskCommercialClasess) {
                    $("#ddlActivityRisk").UifSelect({ sourceData: riskCommercialClasess });
                    $("#ddlActivityRisk").UifSelect('setSelected', data.result.CommercialClassId);
                });

                NoticeFidelity.GetOccupations(function (occupations) {
                    $("#ddlOccupation").UifSelect({ sourceData: occupations });
                    $("#ddlOccupation").UifSelect('setSelected', data.result.OccupationId);
                });

                $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(data.result.DiscoveryDate));
                NoticeFidelity.LockList(true);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static selectDataFromModalToForm(event, selectedRow) {
        _enabled = false;
        documentNumber = selectedRow.DocumentNum;
        riskId = selectedRow.RiskId;
        _endorsementId = selectedRow.EndorsementId;
        _coveredRiskType = selectedRow.CoveredRiskType;

        $("#ddlActivityRisk").UifSelect('setSelected', selectedRow.CommercialClassId);
        $("#ddlOccupation").UifSelect('setSelected', selectedRow.OccupationId);
        $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(selectedRow.DiscoveryDate));

        Notice.GetInsuredsByIndividualId(selectedRow.InsuredId);
        Notice.GetPolicyByEndorsementIdModuleType(selectedRow.EndorsementId);

        NoticeFidelity.LockList(true);
    }

    static GetRisksByInsuredId(insuredId) {
        NoticeFidelityRequest.GetRisksByInsuredId(insuredId).done(function (data) {
            if (data.success) {

                $.each(data.result, function (index, value) {
                    this.DiscoveryDate = FormatDate(this.DiscoveryDate);
                });

                $('#tableFidelityRisk').UifDataTable({ sourceData: data.result });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.SearchByInsured);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ExecuteNoticeOperations() {
        $("#frmNoticeFidelity").validate();
        if ($("#frmNoticeFidelity").valid()) {
            lockScreen();
            var fidelityDTO = {};

            fidelityDTO.CommercialClassId = $("#ddlActivityRisk").UifSelect('getSelected');
            fidelityDTO.OccupationId = $("#ddlOccupation").UifSelect('getSelected');
            fidelityDTO.DiscoveryDate = $("#inputDiscoveryDate").UifDatepicker('getValue');
            fidelityDTO.Description = $("#ddlOccupation").UifSelect('getSelectedSource').Description;

            if (_coveredRiskType == null)
                _coveredRiskType = fidelityCoveredRiskType;

            var noticeFidelityDTO = SetDataNoticeModel();
            var contactInformationDTO = SetDataInformation();
            var noticeCoverageDTO = CoverageList();
            noticeFidelityDTO.Coverages = noticeCoveragesDTO;

            NoticeFidelityRequest.ExecuteNoticeOperations(noticeFidelityDTO, contactInformationDTO, fidelityDTO).done(function (response) {
              
                if (response.success) {
                    LaunchPolicies.ValidateInfringementPolicies(response.result.AuthorizationPolicies, true);
                    var countAutorizationPolicies = response.result.AuthorizationPolicies.filter(x => x.Type == TypeAuthorizationPolicies.Authorization || x.Type == TypeAuthorizationPolicies.Restrictive).length;
                    if (countAutorizationPolicies > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(response.result.AuthorizationPolicies, response.result.TemporalId, FunctionType.ClaimNotice);
                    } else if (response.result.Id > 0) {
                        $.UifDialog('alert', {
                            message: String.format(Resources.Language.SuccessfullySavedRecordWithNoticeNumber + ": {0}.", response.result.Number)
                        }, function (result) {
                        });
                        Notice.CleanFields();
                        NoticeFidelity.LockList(false);
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }                
            }).always(function () {
                unlockScreen();
            });
        } else {
            ScrollTop();
        }
    }

    static CleanFieldsFidelity() {
        $("#ddlActivityRisk").UifSelect('setSelected', null);
        $("#ddlOccupation").UifSelect('setSelected', null);
        $("#inputDiscoveryDate").UifDatepicker('clear');
    }

    static LockList(disabled) {
        $("#ddlActivityRisk").UifSelect('disabled', disabled);
        $("#ddlOccupation").UifSelect('disabled', disabled);
        $("#inputDiscoveryDate").UifDatepicker('disabled', disabled);
    }
}