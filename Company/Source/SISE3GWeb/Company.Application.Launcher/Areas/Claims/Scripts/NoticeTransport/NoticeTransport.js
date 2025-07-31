var transportCoveredRiskType = 8;
class NoticeTransport extends Uif2.Page {
    getInitialState() {
        NoticeTransport.InitialNoticeTransport();
    }

    bindEvents() {
        $('#ClaimedAmount').ValidatorKey(ValidatorType.Number, 2, 1);
        $("#btnSaveClaimNotice").on('click', NoticeTransport.ExecuteNoticeOperations);
        $('#tableTransportRisk').on('rowSelected', function (event, data, position) {
            NoticeTransport.selectDataFromModalToForm(event, data);
        });
        $('#ObjectedDescription').ValidatorKey(ValidatorType.Number, 2, 1);

    }

    /////////////////////////////////////////////////
    // LoadPage()
    static InitialNoticeTransport() {
        $("#coveragespanel").hide();

        NoticeTransport.preLoadNoticeTransport();

        if (modelSearchNewNotice.riskId != null && modelSearchNewNotice.endorsementId != null) {
            NoticeTransport.ToNoticeTransportByPolicy();
        }

        //Se ocultan campos innecesarios para pantalla de fianzas de lo general del aviso ClaimNotice.cshtml
        $("#inputdriverInformation").parent().hide();
        $("#OthersAffected").parent().hide();
    }

    /////////////////////////////////////////////////
    // Consulta el Aviso desde la pantalla Busqueda
    static preLoadNoticeTransport() {
        if (modelSearchNotice.claimNoticeId == null)
            return;

        NoticeRequest.GetNoticeByNoticeId(modelSearchNotice.claimNoticeId).done(function (data) {
            if (data.success) {
                _endorsementId = data.result.EndorsementId;
                individualId = data.result.IndividualId;
                _coveredRiskType = data.result.CoveredRiskType;
                riskId = data.result.RiskId;
                noticeId = data.result.Id;
                noticeStateId = data.result.NoticeStateId;

                $('#Description').val(data.result.Description);
                $('#ObjectedDescription').val(data.result.NumberObjected);
                $("#Number").val(data.result.Number);
                $("#_noticeDate").UifDatepicker("setValue",FormatDate(data.result.NoticeDate));
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
                $("#OthersAffected").val(data.result.OthersAffected);
                $("#inputInternalConsecutive").val(data.result.InternalConsecutive);
                $("#ClaimedAmount").val(FormatMoney(data.result.ClaimedAmount));
                $("#inputdriverInformation").val(data.result.ClaimReasonOthers);
                switch (data.result.NoticeReasonId) {
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
                    NoticeTransport.GetRiskTransportByClaimNoticeId(data.result.Id);
                } else {
                    Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);
                    NoticeTransport.GetRiskTransportByRiskId(riskId);
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

    static ToNoticeTransportByPolicy() {
        _endorsementId = modelSearchNewNotice.endorsementId;
        _coveredRiskType = transportCoveredRiskType;
        riskId = modelSearchNewNotice.riskId;

        NoticeTransport.GetRiskTransportByRiskId(modelSearchNewNotice.riskId);
        Notice.GetPolicyByEndorsementIdModuleType(modelSearchNewNotice.endorsementId);
    }

    static GetRiskTransportByClaimNoticeId(claimNoticeId) {
        NoticeTransportRequest.GetRiskTransportByClaimNoticeId(claimNoticeId).done(function (data) {
            if (data.success) {
                $("#inputCargoType").val(data.result.CargoTypeDescription);
                $("#inputPackagingType").val(data.result.PackagingTypeDescription);
                $("#inputOriginTransport").val(data.result.CityFromDescription);
                $("#inputDestinyTransport").val(data.result.CityToDescription);
                $("#inputTransportType").val(data.result.ViaTypeDescription);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskTransportByRiskId(riskId) {
        NoticeTransportRequest.GetRiskTransportByRiskId(riskId).done(function (data) {
            if (data.success) {
                Notice.GetInsuredsByIndividualId(data.result.InsuredId);
                $("#inputCargoType").val(data.result.CargoTypeDescription);
                $("#inputPackagingType").val(data.result.PackagingTypeDescription);
                $("#inputOriginTransport").val(data.result.CityFromDescription);
                $("#inputDestinyTransport").val(data.result.CityToDescription);
                $("#inputTransportType").val(data.result.ViaTypeDescription);

                NoticeTransport.LockList(true);
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

        $("#inputCargoType").val(selectedRow.CargoTypeDescription);
        $("#inputPackagingType").val(selectedRow.PackagingTypeDescription);
        $("#inputOriginTransport").val(selectedRow.CityFromDescription);
        $("#inputDestinyTransport").val(selectedRow.CityToDescription);
        $("#inputTransportType").val(selectedRow.ViaTypeDescription);
        Notice.GetInsuredsByIndividualId(selectedRow.InsuredId);
        Notice.GetPolicyByEndorsementIdModuleType(selectedRow.EndorsementId);

        NoticeTransport.LockList(true);
    }

    static GetRisksByInsuredId(insuredId) {
        NoticeTransportRequest.GetRisksByInsuredId(insuredId).done(function (data) {
            if (data.success) {
                console.log(data);
                $('#tableTransportRisk').UifDataTable({ sourceData: data.result });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.SearchByInsured);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ExecuteNoticeOperations() {
        $("#frmNoticeTransport").validate();
        if ($("#frmNoticeTransport").valid()) {
            lockScreen();
            var transportDTO = {};

            transportDTO.CargoTypeDescription = $("#inputCargoType").val();
            transportDTO.PackagingTypeDescription = $("#inputPackagingType").val();
            transportDTO.CityFromDescription = $("#inputOriginTransport").val();
            transportDTO.CityToDescription = $("#inputDestinyTransport").val();
            transportDTO.ViaTypeDescription = $("#inputTransportType").val();

            if (_coveredRiskType == null)
                _coveredRiskType = transportCoveredRiskType;

            var noticeTransportDTO = SetDataNoticeModel();
            var contactInformationDTO = SetDataInformation();
            var noticeCoverageDTO = CoverageList();
            noticeTransportDTO.Coverages = noticeCoveragesDTO;

            NoticeTransportRequest.ExecuteNoticeOperations(noticeTransportDTO, contactInformationDTO, transportDTO).done(function (response) {
             
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
                        NoticeTransport.LockList(false);
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

    static CleanFieldsTransport() {
        $("#inputCargoType").val("");
        $("#inputPackagingType").val("");
        $("#inputOriginTransport").val("");
        $("#inputDestinyTransport").val("");
        $("#inputTransportType").val("");
    }

    static LockList(disabled) {
        $("#inputCargoType").prop('disabled', disabled);
        $("#inputPackagingType").prop('disabled', disabled);
        $("#inputOriginTransport").prop('disabled', disabled);
        $("#inputDestinyTransport").prop('disabled', disabled);
        $("#inputTransportType").prop('disabled', disabled);
    }
}