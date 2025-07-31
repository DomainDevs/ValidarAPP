var airCraftCoveredRiskType = 9;
class NoticeAirCraft extends Uif2.Page {

    getInitialState() {
        NoticeAirCraft.InitialNoticeAirCraft();
    }

    bindEvents() {
        $('#ClaimedAmount').ValidatorKey(ValidatorType.Number, 2, 1);
        $("#btnSaveClaimNotice").on('click', NoticeAirCraft.ExecuteNoticeOperations);
        $('#tableAirCraftRisk').on('rowSelected', function (event, data, position) {
            NoticeAirCraft.selectDataFromModalToForm(event, data);
        });

        $("#ddlAirCraftMake").on('itemSelected', function (event, selectedItem) {
            NoticeAirCraft.GetAirCraftModelsByMakeId(selectedItem.Id);
        });
        $('#ObjectedDescription').ValidatorKey(ValidatorType.Number, 2, 1);
    }

    /////////////////////////////////////////////////
    // LoadPage()
    static InitialNoticeAirCraft() {
        $("#coveragespanel").hide();

        if (modelSearchNotice.claimNoticeId == null && modelSearchNewNotice.riskId == null) {
            NoticeAirCraft.GetAirCraftMakes();
            NoticeAirCraft.GetAirCraftUsesByPrefixId();
            NoticeAirCraft.GetAirCraftRegisters();
            NoticeAirCraft.GetAirCraftOperators();
        }

        NoticeAirCraft.preLoadNoticeAirCraft();

        if (modelSearchNewNotice.riskId != null && modelSearchNewNotice.endorsementId != null) {
            NoticeAirCraft.ToNoticeAirCraftByPolicy();
        }

        //Se ocultan campos innecesarios para pantalla de casco aviación de lo general del aviso ClaimNotice.cshtml
        $("#inputdriverInformation").parent().hide();
        $("#OthersAffected").parent().hide();
    }

    /////////////////////////////////////////////////
    // Consulta el Aviso desde la pantalla Busqueda
    static preLoadNoticeAirCraft() {
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
                    NoticeAirCraft.GetRiskAirCraftByClaimNoticeId(data.result.Id);
                } else {
                    Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);
                    NoticeAirCraft.GetRiskAirCraftByRiskId(riskId);
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

    static ToNoticeAirCraftByPolicy() {
        _endorsementId = modelSearchNewNotice.endorsementId;
        _coveredRiskType = airCraftCoveredRiskType;
        riskId = modelSearchNewNotice.riskId;

        NoticeAirCraft.GetRiskAirCraftByRiskId(modelSearchNewNotice.riskId);
        Notice.GetPolicyByEndorsementIdModuleType(modelSearchNewNotice.endorsementId);
    }

    static GetRiskAirCraftByClaimNoticeId(claimNoticeId) {
        NoticeAirCraftRequest.GetRiskAirCraftByClaimNoticeId(claimNoticeId).done(function (data) {
            if (data.success) {
                $("#inputRegisterNumber").val(data.result.RegisterNumber);
                NoticeAirCraft.GetAirCraftMakes(function (makes) {
                    $("#ddlAirCraftMake").UifSelect({ sourceData: makes });
                    $("#ddlAirCraftMake").UifSelect('setSelected', data.result.MakeId);
                    NoticeAirCraft.GetAirCraftModelsByMakeId(data.result.MakeId, function (models) {
                        $("#ddlAirCraftModel").UifSelect({ sourceData: models });
                        $("#ddlAirCraftModel").UifSelect('setSeleted', data.result.ModelId);
                    });
                });

                NoticeAirCraft.GetAirCraftUsesByPrefixId(function (uses) {
                    $("#ddlAirCraftUse").UifSelect({ sourceData: uses });
                    $("#ddlAirCraftUse").UifSelect('setSelected', data.result.UseId);
                });

                NoticeAirCraft.GetAirCraftRegisters(function (registers) {
                    $("#ddlAirCraftRegister").UifSelect({ sourceData: registers });
                    $("#ddlAirCraftRegister").UifSelect('setSelected', data.result.RegisterId);
                });

                NoticeAirCraft.GetAirCraftOperators(function (operators) {
                    $("#ddlAirCraftOperator").UifSelect({ sourceData: operators });
                    $("#ddlAirCraftOperator").UifSelect('setSelected', data.result.OperatorId);
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskAirCraftByRiskId(riskId) {
        NoticeAirCraftRequest.GetRiskAirCraftByRiskId(riskId).done(function (data) {
            if (data.success) {
                Notice.GetInsuredsByIndividualId(data.result.InsuredId);

                $("#inputRegisterNumber").val(data.result.RegisterNumber);
                NoticeAirCraft.GetAirCraftMakes(function (makes) {
                    $("#ddlAirCraftMake").UifSelect({ sourceData: makes });
                    $("#ddlAirCraftMake").UifSelect('setSelected', data.result.MakeId);
                    NoticeAirCraft.GetAirCraftModelsByMakeId(data.result.MakeId, function (models) {
                        $("#ddlAirCraftModel").UifSelect({ sourceData: models });
                        $("#ddlAirCraftModel").UifSelect('setSelected', data.result.ModelId);
                    });
                });

                NoticeAirCraft.GetAirCraftUsesByPrefixId(function (uses) {
                    $("#ddlAirCraftUse").UifSelect({ sourceData: uses });
                    $("#ddlAirCraftUse").UifSelect('setSelected', data.result.UseId);
                });

                NoticeAirCraft.GetAirCraftRegisters(function (registers) {
                    $("#ddlAirCraftRegister").UifSelect({ sourceData: registers });
                    $("#ddlAirCraftRegister").UifSelect('setSelected', data.result.RegisterId);
                });

                NoticeAirCraft.GetAirCraftOperators(function (operators) {
                    $("#ddlAirCraftOperator").UifSelect({ sourceData: operators });
                    $("#ddlAirCraftOperator").UifSelect('setSelected', data.result.OperatorId);
                });
                NoticeAirCraft.LockList(true);
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

        $("#ddlAirCraftMake").UifSelect('setSelected', selectedRow.MakeId);
        NoticeAirCraft.GetAirCraftModelsByMakeId(selectedRow.MakeId, function (models) {
            $("#ddlAirCraftModel").UifSelect({ sourceData: models });
            $("#ddlAirCraftModel").UifSelect('setSelected', selectedRow.ModelId);
        });
        $("#ddlAirCraftUse").UifSelect('setSelected', selectedRow.UseId);
        $("#ddlAirCraftRegister").UifSelect('setSelected', selectedRow.RegisterId);
        $("#ddlAirCraftOperator").UifSelect('setSelected', selectedRow.OperatorId);
        $("#inputRegisterNumber").val(selectedRow.RegisterNumber);
        Notice.GetInsuredsByIndividualId(selectedRow.InsuredId);
        Notice.GetPolicyByEndorsementIdModuleType(selectedRow.EndorsementId);

        NoticeAirCraft.LockList(true);
    }

    static GetRisksByInsuredId(insuredId) {
        NoticeAirCraftRequest.GetRisksByInsuredId(insuredId).done(function (data) {
            if (data.success) {
                $('#tableAirCraftRisk').UifDataTable({ sourceData: data.result });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.SearchByInsured);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ExecuteNoticeOperations() {
        $("#frmNoticeAirCraft").validate();
        if ($("#frmNoticeAirCraft").valid()) {
            lockScreen();
            var airCraftDTO = {};

            airCraftDTO.MakeId = $("#ddlAirCraftMake").UifSelect('getSelected');
            airCraftDTO.ModelId = $("#ddlAirCraftModel").UifSelect('getSelected');
            airCraftDTO.UseId = $("#ddlAirCraftUse").UifSelect('getSelected');
            airCraftDTO.RegisterId = $("#ddlAirCraftRegister").UifSelect('getSelected');
            airCraftDTO.OperatorId = $("#ddlAirCraftOperator").UifSelect('getSelected');
            airCraftDTO.RegisterNumber = $("#inputRegisterNumber").val();

            if (_coveredRiskType == null)
                _coveredRiskType = airCraftCoveredRiskType;

            var noticeAirCraftDTO = SetDataNoticeModel();
            var contactInformationDTO = SetDataInformation();
            var noticeCoverageDTO = CoverageList();
            noticeAirCraftDTO.Coverages = noticeCoveragesDTO;

            NoticeAirCraftRequest.ExecuteNoticeOperations(noticeAirCraftDTO, contactInformationDTO, airCraftDTO, noticeCoverageDTO).done(function (response) {
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
                        NoticeAirCraft.LockList(false);
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

    static GetAirCraftMakes(callback) {
        NoticeAirCraftRequest.GetAirCraftMakes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlAirCraftMake").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetAirCraftModelsByMakeId(makeId, callback) {
        NoticeAirCraftRequest.GetAirCraftModelsByMakeId(makeId).done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlAirCraftModel").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetAirCraftUsesByPrefixId(callback) {
        NoticeAirCraftRequest.GetAirCraftUsesByPrefixId().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlAirCraftUse").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetAirCraftRegisters(callback) {
        NoticeAirCraftRequest.GetAirCraftRegisters().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlAirCraftRegister").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetAirCraftOperators(callback) {
        NoticeAirCraftRequest.GetAirCraftOperators().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $("#ddlAirCraftOperator").UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static CleanFieldsAirCraft() {
        $("#ddlAirCraftMake").UifSelect('setSelected', null);
        $("#ddlAirCraftModel").UifSelect();
        $("#ddlAirCraftUse").UifSelect('setSelected', null);
        $("#ddlAirCraftRegister").UifSelect('setSelected', null);
        $("#ddlAirCraftOperator").UifSelect('setSelected', null);
        $("#inputRegisterNumber").val("");
    }

    static LockList(disabled) {
        $("#ddlAirCraftMake").UifSelect('disabled', disabled);
        $("#ddlAirCraftModel").UifSelect('disabled', disabled);
        $("#ddlAirCraftUse").UifSelect('disabled', disabled);
        $("#ddlAirCraftRegister").UifSelect('disabled', disabled);
        $("#ddlAirCraftOperator").UifSelect('disabled', disabled);
        $("#inputRegisterNumber").prop('disabled', disabled);
    }
}