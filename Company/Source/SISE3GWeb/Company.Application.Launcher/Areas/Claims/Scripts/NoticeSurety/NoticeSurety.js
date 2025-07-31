var suretyCoveredRiskType = 7;
class NoticeSurety extends Uif2.Page {
    getInitialState() {
        NoticeSurety.InitialNoticeSurety();
    }

    bindEvents() {
        // ItemSelected
        $('#ClaimedAmount').ValidatorKey(ValidatorType.Number, 2, 1);
        $("#inputSurety").on('buttonClick', NoticeSurety.GetRisksSuretyByInsured);

        // Click
        $("#btnSaveClaimNotice").on('click', NoticeSurety.ExecuteNoticeOperations);

        // DataTables
        $('#tableSuretyRisk').on('rowSelected', function (event, data, position) {
            NoticeSurety.selectDataFromModalToForm(event, data);
        });
        $('#ObjectedDescription').ValidatorKey(ValidatorType.Number, 2, 1);

    }

    /////////////////////////////////////////////////
    // LoadPage()
    static InitialNoticeSurety() {
        $("#coveragespanel").hide();

        NoticeSuretyRequest.GetBranches().done(function (response) {
            if (response.success) {
                $("#_branch").UifSelect({ sourceData: response.result });
                $("#_branch").prop('disabled', 'disabled');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeSuretyRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                $("#_prefix").UifSelect({ sourceData: response.result });
                $("#_prefix").prop('disabled', 'disabled');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeSurety.preLoadNoticeSurety();

        if (modelSearchNewNotice.riskId != null && modelSearchNewNotice.endorsementId != null) {
            NoticeSurety.ToNoticeSuretyByPolicy();
        }


        //Se ocultan campos innecesarios para pantalla de fianzas de lo general del aviso ClaimNotice.cshtml
        $("#panelLocation").hide();
        $("#Location").parent().hide();
        $("#inputdriverInformation").parent().hide();
        $("#OthersAffected").parent().hide();
    }

    /////////////////////////////////////////////////
    // Consulta el Aviso desde la pantalla Busqueda
    static preLoadNoticeSurety() {
        if (modelSearchNotice.claimNoticeId == null)
            return;

        NoticeRequest.GetNoticeByNoticeId(modelSearchNotice.claimNoticeId).done(function (data) {
            if (data.success) {
                _endorsementId = data.result.EndorsementId;
                individualId = data.result.IndividualId;
                _coveredRiskType = data.result.CoveredRiskType;
                riskId = data.result.RiskId;
                riskNum = data.result.RiskNumber;
                noticeId = data.result.Id;
                noticeStateId = data.result.NoticeStateId;
                $("#ObjectedDescription").val(data.result.NumberObjected);
                $("#inputContactInformationName").val(data.result.ContactName);
                $("#inputContactInformationPhone").val(data.result.PhoneNumber);
                $("#inputContactInformationMail").val(data.result.Email);
                $("#Description").val(data.result.Description);
                $("#Number").val(data.result.Number);
                $('#Date').UifDatepicker("setValue", moment(data.result.OcurrenceDate).format("DD/MM/YYYY"));                
                $("#InputTimeIncident").UifTimepicker('setValue', moment(data.result.OcurrenceDate).format("HH:mm"));
                $("#_noticeDate").UifDatepicker("setValue",FormatDate(data.result.NoticeDate));
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
                    NoticeSurety.GetRiskSuretyByClaimNoticeId(data.result.Id);
                } else {
                    Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);
                    NoticeSurety.GetRiskSuretyByRiskIdPrefixId(riskId, modelSearchNotice.prefixId);
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

    static ToNoticeSuretyByPolicy() {
        _endorsementId = modelSearchNewNotice.endorsementId;
        _coveredRiskType = suretyCoveredRiskType;
        riskId = modelSearchNewNotice.riskId;

        NoticeSurety.GetRiskSuretyByRiskIdPrefixId(modelSearchNewNotice.riskId, modelSearchNewNotice.prefixId);
        Notice.GetPolicyByEndorsementIdModuleType(modelSearchNewNotice.endorsementId);
    }

    static GetRisksSuretyByInsured() {
        var query = $("#inputSurety").val();
        lockScreen();
        NoticeSuretyRequest.GetRisksSuretyByInsured(query).done(function (response) {            
            if (response.length > 0) {
                $.each(response, function (index, value) {
                    this.BondedFullName = this.IdentificationDocument + "(" + this.Bonded + ")";
                });

                $('#tableSuretyRisk').UifDataTable({ sourceData: response });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.Bonded);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });
    }

    static GetRiskSuretyByClaimNoticeId(claimNoticeId) {
        NoticeSuretyRequest.GetRiskSuretyByClaimNoticeId(claimNoticeId).done(function (data) {
            if (data.success) {
                $('#txtContractNumber').val(data.result.BidNumber);
                $('#txtNumber').val(data.result.CourtNum);

                NoticeSurety.LockList(true);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskSuretyByRiskIdPrefixId(riskId, prefixId) {
        NoticeSuretyRequest.GetRiskSuretyByRiskIdPrefixId(riskId, prefixId).done(function (data) {
            if (data.success) {
                if (data.result == null) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EndorsementDontHaveRisks, 'autoclose': true });
                }
                else {
                    Notice.GetInsuredsByIndividualId(data.result.InsuredId);
                    riskNum = data.result.RiskNumber;
                    $('#inputSurety').val(data.result.IdentificationDocument + "(" + data.result.Bonded + ")");
                    $('#txtContractNumber').val(data.result.BidNumber);
                    $('#txtNumber').val(data.result.CourtNum);
                    NoticeSurety.LockList(true);
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static selectDataFromModalToForm(event, selectedRow) {
        _enabled = false;
        documentNumber = selectedRow.DocumentNum;
        riskId = selectedRow.RiskId;
        riskNum = selectedRow.RiskNum;
        _endorsementId = selectedRow.EndorsementId;
        _coveredRiskType = selectedRow.CoveredRiskType;

        $('#inputSurety').val(selectedRow.IdentificationDocument + "(" + selectedRow.Bonded + ")");
        $('#txtContractNumber').val(selectedRow.BidNumber);

        NoticeRequest.GetInsuredsByIndividualId(selectedRow.InsuredId).done(function (response) {
            if (response.success && response.result.length > 0) {
                $('#insuredDocumentNumber').UifAutoComplete('setValue', response.result[0].DocumentNumber);
                $('#insuredFullName').UifAutoComplete('setValue', response.result[0].FullName);
                $("#txtInsuredName").val(response.result[0].FullName);
            }
        });

        Notice.GetPolicyByEndorsementIdModuleType(selectedRow.EndorsementId);
        NoticeSurety.LockList(true);
    }

    static GetRisksByInsuredId(insuredId) {
        lockScreen();
        NoticeSuretyRequest.GetRisksByInsuredId(insuredId).done(function (data) {            
            if (data.success) {
                riskNum = data.result.RiskNum;
                $('#tableSuretyRisk').UifDataTable({ sourceData: data.result });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.SearchByInsured);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });
    }

    static ExecuteNoticeOperations() {
        $("#frmNoticeSurety").validate();
        $("#frmClaimNotice").validate();
        if ($("#frmNoticeSurety").valid() && $("#frmClaimNotice").valid()) {
            lockScreen();
            var suretyDTO = {};

            suretyDTO.SuretyName = $("#inputSurety").val();
            suretyDTO.SuretyDocumentNumber = $("#inputSurety").val();
            suretyDTO.BidNumber = $("#txtContractNumber").val();
            suretyDTO.CourtNum = $("#txtNumber").val();

            if (_coveredRiskType == null)
                _coveredRiskType = suretyCoveredRiskType;

            var noticeSuretyDTO = SetDataNoticeModel();
            var contactInformationDTO = SetDataInformation();
            var noticeCoverageDTO = CoverageList();
            noticeSuretyDTO.Coverages = noticeCoverageDTO;

            NoticeSuretyRequest.ExecuteNoticeOperations(noticeSuretyDTO, contactInformationDTO, suretyDTO).done(function (response) {

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
                        NoticeSurety.LockList(false);
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

    static CleanFieldsSurety() {
        $('#inputSurety').val('');
        $('#txtContractNumber').val("");
        $('#txtNumber').val("");
        $('#txtInsuredName').val("");
        $('#ClaimedAmount').val("");
    }

    static LockList(disabled) {
        $('#txtContractNumber').prop("disabled", disabled);
        $('#txtNumber').prop("disabled", disabled);
        $('#txtInsuredName').prop("disabled", disabled);
    }
}