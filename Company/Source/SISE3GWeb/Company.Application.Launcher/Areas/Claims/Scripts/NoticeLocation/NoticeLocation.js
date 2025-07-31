var locationCoveredRiskType = 2;
class NoticeLocation extends Uif2.Page {
    getInitialState() {
        NoticeLocation.InitialNoticeLocation();
    }

    bindEvents() {
        $('#ClaimedAmount').ValidatorKey(ValidatorType.Number, 2, 1);
        $("#inputRiskLocation").TextTransform(ValidatorType.UpperCase);

        // itemSelected
        $('#CountryLocation').on('itemSelected', NoticeLocation.GetStatesByCountryId);
        $('#StateLocation').on('itemSelected', NoticeLocation.SelectState);
        $("#inputRiskLocation").on('buttonClick', NoticeLocation.GetRiskLocationByAddress);

        // DataTables
        $('#tableLocationRisk').on('rowSelected', function (event, data, position) {
            NoticeLocation.selectDataFromModalToForm(event, data);
        });
        $('#ObjectedDescription').ValidatorKey(ValidatorType.Number, 2, 1);
        $('#btnSaveClaimNotice').on("click", NoticeLocation.ExecuteNoticeOperations);
    }

    static InitialNoticeLocation() {
        NoticeLocation.preLoadNoticeLocation();

        NoticeLocation.GetCountries();

        if (modelSearchNewNotice.riskId != null && modelSearchNewNotice.endorsementId != null) {
            NoticeLocation.ToNoticeLocationByPolicy();
        }

        $('#inputdriverInformation').parent().parent().hide();
    }

    static GetCountries() {
        NoticeRequest.GetCountries().done(function (response) {
            if (response.success) {
                $('#CountryLocation').UifSelect({ sourceData: response.result });
                if (modelSearchNotice.claimNoticeId == null) {
                    NoticeRequest.GetDefaultCountry().done(function (data) {
                        if (data.success) {
                            $("#CountryLocation").UifSelect("setSelected", data.result);
                            $('#CountryLocation').trigger("change");
                            $("#Country").UifSelect("setSelected", data.result);
                            $('#Country').trigger("change");
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    /////////////////////////////////////////////////
    // Consulta el Aviso desde la pantalla Busqueda
    static preLoadNoticeLocation() {
        if (modelSearchNotice.claimNoticeId == null)
            return;

        NoticeRequest.GetNoticeByNoticeId(modelSearchNotice.claimNoticeId).done(function (response) {
            if (response.success) {
                _endorsementId = response.result.EndorsementId;
                individualId = response.result.IndividualId;
                _coveredRiskType = response.result.CoveredRiskType;
                riskId = response.result.RiskId;
                riskNum = response.result.RiskNumber;
                noticeId = response.result.Id;
                noticeStateId = response.result.NoticeStateId;

                $('#Description').val(response.result.Description);
                $('#ObjectedDescription').val(response.result.NumberObjected);
                $("#Number").val(response.result.Number);
                $("#_noticeDate").UifDatepicker("setValue", FormatDate(response.result.NoticeDate));
                $('#Location').val(response.result.Location);
                $("#Date").val(FormatDate(response.result.OcurrenceDate));

                $('#Country').UifSelect('setSelected', response.result.CountryId);

                Notice.GetStatesByCountryId(response.result.CountryId);
                $('#State').UifSelect('setSelected', response.result.StateId);

                Notice.GetCitiesByCountryIdStateId(response.result.CountryId, response.result.StateId);
                $('#City').UifSelect('setSelected', response.result.CityId);
                $('#inputContactInformationName').val(response.result.ContactName);
                $('#inputContactInformationPhone').val(response.result.PhoneNumber);
                $('#inputContactInformationMail').val(response.result.Email);
                $("#OthersAffected").val(response.result.OthersAffected);//
                $("#inputInternalConsecutive").val(response.result.InternalConsecutive);
                $("#ClaimedAmount").val(FormatMoney(response.result.ClaimedAmount));//
                $("#inputdriverInformation").val(response.result.ClaimReasonOthers);//
                switch (response.result.NoticeReasonId) {//
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
                    NoticeLocation.GetRiskLocationByClaimNoticeId(response.result.Id);
                } else {
                    Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);
                    NoticeLocation.GetRiskLocationByRiskId(riskId);
                }
                setTimeout(function () {
                    response.result.Coverages.forEach(item => {

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
                            'RiskNum': response.result.RiskNumber,
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

    static ToNoticeLocationByPolicy() {
        _endorsementId = modelSearchNewNotice.endorsementId;
        _coveredRiskType = locationCoveredRiskType;
        riskId = modelSearchNewNotice.riskId;

        NoticeLocation.GetRiskLocationByRiskId(riskId);
        Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);

        modelSearchNewNotice.riskId = null;
        modelSearchNewNotice.endorsementId = null;
    }

    static GetRiskLocationByAddress() {
        var query = $("#inputRiskLocation").val();

        NoticeLocationRequest.GetRiskLocationByAddress(query).done(function (response) {
            if (response.length > 0) {
                $('#tableLocationRisk').UifDataTable({ sourceData: response });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.Property);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoExistRisk, 'autoclose': true });
            }
        });
    }

    static GetRiskLocationByClaimNoticeId(claimNoticeId) {
        NoticeLocationRequest.GetRiskLocationByClaimNoticeId(claimNoticeId).done(function (data) {
            if (data.success) {
                $('#inputRiskLocation').val(data.result.FullAddress);

                $('#CountryLocation').UifSelect('setSelected', data.result.CountryId);

                var country = {
                    Id: data.result.CountryId
                };

                NoticeLocation.GetStatesByCountryId(null, country, function (states) {
                    $('#StateLocation').UifSelect({ sourceData: states, enable: false });
                    $('#StateLocation').UifSelect('setSelected', data.result.StateId);

                    NoticeLocation.GetCitiesByCountryIdStateId(data.result.CountryId, data.result.StateId, function (cities) {
                        $('#CityLocation').UifSelect({ sourceData: cities, enable: false });
                        $('#CityLocation').UifSelect('setSelected', data.result.CityId);
                    });
                });

                NoticeLocation.LockList(true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskLocationByRiskId(riskId) {
        NoticeLocationRequest.GetRiskLocationByRiskId(riskId).done(function (data) {
            if (data.success) {
                Notice.GetInsuredsByIndividualId(data.result.InsuredId);
                riskNum = data.result.RiskNumber;
                $('#inputRiskLocation').val(data.result.FullAddress);

                $('#CountryLocation').UifSelect('setSelected', data.result.CountryId);

                var country = {
                    Id: data.result.CountryId
                };

                NoticeLocation.GetStatesByCountryId(null, country, function (states) {
                    $('#StateLocation').UifSelect({ sourceData: states, enable: false });
                    $('#StateLocation').UifSelect('setSelected', data.result.StateId);

                    NoticeLocation.GetCitiesByCountryIdStateId(data.result.CountryId, data.result.StateId, function (cities) {
                        $('#CityLocation').UifSelect({ sourceData: cities, enable: false });
                        $('#CityLocation').UifSelect('setSelected', data.result.CityId);
                    });
                });
                NoticeLocation.LockList(true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetStatesByCountryId(event, selectedItem, callback) {
        if (selectedItem != "" && selectedItem !== undefined && selectedItem != null) {
            NoticeRequest.GetStatesByCountryId(selectedItem.Id).done(function (response) {
                if (response.success) {
                    if (callback)
                        return callback(response.result);
                    $('#StateLocation').UifSelect({ sourceData: response.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#selectDepartment").UifSelect('setSelected', null);
            $('#selectCity').UifSelect('setSelected', null);
        }
    }

    static SelectState(event, state) {
        var countryId = $('#CountryLocation').UifSelect('getSelected');
        NoticeLocation.GetCitiesByCountryIdStateId(countryId, state.Id);
    }

    static GetCitiesByCountryIdStateId(countryId, stateId, callback) {
        if (countryId != "" && countryId !== undefined && countryId != null
            && stateId != "" && stateId !== undefined && stateId != null) {
            NoticeRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
                if (response.success) {
                    if (callback)
                        return callback(response.result);
                    $('#CityLocation').UifSelect({ sourceData: response.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectCity').UifSelect('setSelected', null);
        }
    }

    static GetRisksByInsuredId(insuredId) {
        NoticeLocationRequest.GetRisksByInsuredId(insuredId).done(function (data) {
            if (data.success) {
                riskNum = data.result.RiskNumber;
                $('#tableLocationRisk').UifDataTable({ sourceData: data.result });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.SearchByInsured);
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
        riskNum = selectedRow.RiskNumber;

        $('#inputRiskLocation').val(selectedRow.FullAddress);
        $('#CountryLocation').UifSelect('setSelected', selectedRow.CountryId);

        var country = {
            Id: selectedRow.CountryId
        };

        NoticeLocation.GetStatesByCountryId(null, country);
        $('#StateLocation').UifSelect('setSelected', selectedRow.StateId);

        NoticeLocation.GetCitiesByCountryIdStateId(selectedRow.CountryId, selectedRow.StateId);
        $('#CityLocation').UifSelect('setSelected', selectedRow.CityId);

        Notice.GetInsuredsByIndividualId(selectedRow.InsuredId);
        Notice.GetPolicyByEndorsementIdModuleType(selectedRow.EndorsementId);
        NoticeLocation.LockList(true);
    }

    /////////////////////////////////////////////////
    // Crear Aviso de Propiedad
    static ExecuteNoticeOperations() {
        $("#frmNoticeLocation").validate();
        $("#frmClaimNotice").validate();
        if ($("#frmNoticeLocation").valid() && $("#frmClaimNotice").valid()) {
            lockScreen();

            var locationDTO = {};

            locationDTO.FullAddress = $("#inputRiskLocation").val();
            locationDTO.CountryId = $("#CountryLocation").UifSelect('getSelected');
            locationDTO.StateId = $("#StateLocation").UifSelect('getSelected');
            locationDTO.CityId = $("#CityLocation").UifSelect('getSelected');

            if (_coveredRiskType == null)
                _coveredRiskType = locationCoveredRiskType;

            var noticeLocationDTO = SetDataNoticeModel();
            var contactInformationDTO = SetDataInformation();
            var noticeCoveragesDTO = CoverageList();
            noticeLocationDTO.Coverages = noticeCoveragesDTO;

            NoticeLocationRequest.ExecuteNoticeOperations(noticeLocationDTO, contactInformationDTO, locationDTO).done(function (response) {
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
                        NoticeLocation.LockList(false);
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
        }
        else {
            ScrollTop();
        }
    }

    static CleanFieldsLocation() {
        $('#CountryLocation').UifSelect('setSelected', null);
        $('#StateLocation').UifSelect();
        $('#CityLocation').UifSelect();
        $('#inputRiskLocation').val('')

        NoticeLocation.GetCountries();
    }

    static LockList(disabled) {
        $("#CountryLocation").prop("disabled", disabled);
        $("#StateLocation").prop("disabled", disabled);
        $("#CityLocation").prop("disabled", disabled);
    }
}