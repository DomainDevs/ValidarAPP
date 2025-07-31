var autoCoveredRiskType = 1;

class NoticeVehicle extends Uif2.Page {
    getInitialState() {
        NoticeVehicle.InitialNoticeVehicle();
    }

    bindEvents() {
        // Validator
        $('#inputRiskPlate').ValidatorKey(ValidatorType.Onlylettersandnumbers, 1, 1);
        $("#inputRiskPlate").TextTransform(ValidatorType.UpperCase);

        // itemSelected
        $('#ddlVehicleMake').on('itemSelected', NoticeVehicle.LoadVehicleModel);
        $('#ddlVehicleModel').on('itemSelected', NoticeVehicle.LoadVehicleVersion);
        $('#ddlVehicleVersion').on('itemSelected', NoticeVehicle.LoadVehicleVersionYear);

        $("#inputRiskPlate").on('buttonClick', NoticeVehicle.GetRisksByPlate);

        // Change
        $('#btnSaveClaimNotice').on("click", NoticeVehicle.ExecuteNoticeOperations);

        // rowSelected
        $('#tableVehicleRisk').on('rowSelected', function (event, data, position) {
            NoticeVehicle.selectDataFromModalToForm(event, data);
        });
        $('#ObjectedDescription').ValidatorKey(ValidatorType.Number, 2, 1);

    }

    /////////////////////////////////////////////////
    // LoadPage()
    static InitialNoticeVehicle() {

        NoticeVehicle.GetVehicleMakes();
        NoticeVehicle.GetVehicleColors();

        NoticeVehicleRequest.GetDamageTypes().done(function (response) {
            if (response.success) {
                $('#ddlDamageType').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        NoticeVehicleRequest.GetDamageResponsibilities().done(function (response) {
            if (response.success) {
                $('#ddlDamageResponsibility').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        Notice.HideTextBoxClaimNoticeReasonOthers();
        NoticeVehicle.preLoadNoticeVehicle();
        if (modelSearchNewNotice.riskId != null && modelSearchNewNotice.endorsementId != null) {
            NoticeVehicle.ToNoticeVehicleByPolicy();
        }
    }

    static GetVehicleMakes() {
        NoticeVehicleRequest.GetVehicleMakes().done(function (response) {
            if (response.success) {
                $('#ddlVehicleMake').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleColors() {
        NoticeVehicleRequest.GetVehicleColors().done(function (response) {
            if (response.success) {
                $('#ddlColors').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    /////////////////////////////////////////////////
    // Consulta el Aviso desde la pantalla Busqueda
    static preLoadNoticeVehicle() {
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

                $('#Location').val(data.result.Location);
                $('#Country').UifSelect('setSelected', data.result.CountryId);

                Notice.GetStatesByCountryId(data.result.CountryId);
                $('#State').UifSelect('setSelected', data.result.StateId);

                Notice.GetCitiesByCountryIdStateId(data.result.CountryId, data.result.StateId);
                $('#City').UifSelect('setSelected', data.result.CityId);

                $('#Description').val(data.result.Description);
                $('#ObjectedDescription').val(data.result.NumberObjected);

                $('#ddlDamageType').UifSelect('setSelected', data.result.DamageTypeId);
                $('#ddlDamageResponsibility').UifSelect('setSelected', data.result.DamageResponsibilityId);
                $('#inputContactInformationName').val(data.result.ContactName);
                $('#inputContactInformationPhone').val(data.result.PhoneNumber);
                $('#inputContactInformationMail').val(data.result.Email);
                $('#Date').val(FormatDate(data.result.OcurrenceDate));
                $("#Number").val(data.result.Number);
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
                    NoticeVehicle.GetRiskVehicleByClaimNoticeId(data.result.Id);
                } else {
                    notice = data.result;
                    Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);
                    NoticeVehicle.GetRiskVehicleByRiskId(riskId);
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

    static ToNoticeVehicleByPolicy() {
        _endorsementId = modelSearchNewNotice.endorsementId;
        _coveredRiskType = autoCoveredRiskType;
        riskId = modelSearchNewNotice.riskId;

        NoticeVehicle.GetRiskVehicleByRiskId(riskId);
        Notice.GetPolicyByEndorsementIdModuleType(_endorsementId);

        modelSearchNewNotice.riskId = null;
        modelSearchNewNotice.endorsementId = null;
    }

    static GetRisksByPlate() {
        var query = $("#inputRiskPlate").val();

        NoticeVehicleRequest.GetRisksByPlate(query).done(function (response) {
            if (response.length > 0) {
                riskNum = response.result.RiskNumber;
                $('#tableVehicleRisk').UifDataTable({ sourceData: response });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.Vehicles);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistVehicles, 'autoclose': true });
            }
        });
    }

    static GetRiskVehicleByRiskId(riskId) {
        NoticeVehicleRequest.GetRiskVehicleByRiskId(riskId).done(function (data) {
            if (data.success) {
                riskNum = data.result.RiskNumber;
                Notice.GetInsuredsByIndividualId(data.result.InsuredId);
                $('#inputRiskPlate').val(data.result.Plate);

                $('#ddlVehicleMake').UifSelect('setSelected', data.result.MakeId);

                NoticeVehicle.GetVehicleModelsByVehicleMakeId(data.result.MakeId);
                $('#ddlVehicleModel').UifSelect('setSelected', data.result.ModelId);

                NoticeVehicle.GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(data.result.MakeId, data.result.ModelId);
                $('#ddlVehicleVersion').UifSelect('setSelected', data.result.VersionId);

                NoticeVehicle.GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(data.result.MakeId, data.result.ModelId, data.result.VersionId);
                $('#ddlVehicleVersionYear').UifSelect('setSelected', data.result.Year);

                $('#ddlColors').UifSelect('setSelected', data.result.ColorId);
                NoticeVehicle.LockList(true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetRiskVehicleByClaimNoticeId(claimNoticeId) {
        NoticeVehicleRequest.GetRiskVehicleByClaimNoticeId(claimNoticeId).done(function (data) {
            if (data.success) {
                $('#inputRiskPlate').val(data.result.Plate);

                $('#ddlVehicleMake').UifSelect('setSelected', data.result.MakeId);

                NoticeVehicle.GetVehicleModelsByVehicleMakeId(data.result.MakeId);
                $('#ddlVehicleModel').UifSelect('setSelected', data.result.ModelId);

                NoticeVehicle.GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(data.result.MakeId, data.result.ModelId);
                $('#ddlVehicleVersion').UifSelect('setSelected', data.result.VersionId);

                NoticeVehicle.GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(data.result.MakeId, data.result.ModelId, data.result.VersionId);
                $('#ddlVehicleVersionYear').UifSelect('setSelected', data.result.Year);

                $('#ddlColors').UifSelect('setSelected', data.result.ColorId);

                NoticeVehicle.LockList(true);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /////////////////////////////////////////////////
    // itemSelected de Vehiculo
    static LoadVehicleModel(event, selectedItem) {
        NoticeVehicle.GetVehicleModelsByVehicleMakeId(selectedItem.Id);
    }

    static GetVehicleModelsByVehicleMakeId(vehicleMakeId) {
        if (vehicleMakeId != "") {
            NoticeVehicleRequest.GetVehicleModelsByVehicleMakeId(vehicleMakeId).done(function (response) {
                if (response.success) {
                    $('#ddlVehicleModel').UifSelect({ sourceData: response.result });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#ddlVehicleModel").UifSelect();
            $("#ddlVehicleVersion").UifSelect();
            $("#ddlVehicleVersionYear").UifSelect();
        }
    }

    static GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(vehicleMakeId, vehicleModelId) {
        if (vehicleMakeId != "" && vehicleModelId != "") {
            NoticeVehicleRequest.GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(vehicleMakeId, vehicleModelId).done(function (response) {
                if (response.success) {
                    $('#ddlVehicleVersion').UifSelect({ sourceData: response.result });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#ddlVehicleVersion").UifSelect();
            $("#ddlVehicleVersionYear").UifSelect();
        }
    }

    static GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId) {
        if (vehicleMakeId != "" && vehicleModelId != "" && vehicleVersionId != "") {
            NoticeVehicleRequest.GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId).done(function (response) {
                if (response.success) {
                    $('#ddlVehicleVersionYear').UifSelect({ sourceData: response.result });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#ddlVehicleVersionYear").UifSelect();
        }
    }

    static LoadVehicleVersion(event, selectedItem) {
        var vehicleMakeId = $('#ddlVehicleMake').UifSelect('getSelected');
        var vehicleModelId = selectedItem.Id;
        NoticeVehicle.GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(vehicleMakeId, vehicleModelId);
    }

    static LoadVehicleVersionYear(event, selectedItem) {
        var vehicleMakeId = $('#ddlVehicleMake').UifSelect('getSelected');
        var vehicleModelId = $('#ddlVehicleModel').UifSelect('getSelected');
        var vehicleVersionId = selectedItem.Id;
        NoticeVehicle.GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId);
        NoticeVehicle.GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId);
    }

    /////////////////////////////////////////////////
    // Eventos DataTable
    static selectDataFromModalToForm(event, selectedRow) {
        plate = selectedRow.Plate;
        documentNumber = selectedRow.DocumentNumber;
        riskId = selectedRow.RiskId;
        riskNum = selectedRow.RiskNumber; //validar
        _endorsementId = selectedRow.EndorsementId;
        _coveredRiskType = selectedRow.CoveredRiskType;

        $("#inputRiskPlate").val(selectedRow.Plate);
        $('#ddlVehicleMake').UifSelect('setSelected', selectedRow.MakeId);

        NoticeVehicle.GetVehicleModelsByVehicleMakeId(selectedRow.MakeId);
        $('#ddlVehicleModel').UifSelect('setSelected', selectedRow.ModelId);

        NoticeVehicle.GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(selectedRow.MakeId, selectedRow.ModelId);
        $('#ddlVehicleVersion').UifSelect('setSelected', selectedRow.VersionId);

        NoticeVehicle.GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(selectedRow.MakeId, selectedRow.ModelId, selectedRow.VersionId);
        $('#ddlVehicleVersionYear').UifSelect('setSelected', selectedRow.Year);

        $('#ddlColors').UifSelect('setSelected', selectedRow.ColorId);
        Notice.GetInsuredsByIndividualId(selectedRow.InsuredId);
        Notice.GetPolicyByEndorsementIdModuleType(selectedRow.EndorsementId);
        NoticeVehicle.LockList(true);
    }

    static GetRisksByInsuredId(insuredId) {
        NoticeVehicleRequest.GetRisksByInsuredId(insuredId).done(function (data) {
            if (data.success) {
                riskNum = data.result.RiskNumber;
                $('#tableVehicleRisk').UifDataTable({ sourceData: data.result });
                $('#modalSearchbyInsured').UifModal('showLocal', Resources.Language.SearchByInsured);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /////////////////////////////////////////////////
    // Funcion para bloquear los campos del vehículo
    static LockList(disabled) {
        $("#ddlVehicleMake").UifSelect("disabled", disabled);
        $("#ddlVehicleModel").UifSelect("disabled", disabled);
        $("#ddlVehicleVersion").UifSelect("disabled", disabled);
        $("#ddlVehicleVersionYear").UifSelect("disabled", disabled);
        $("#ddlColors").UifSelect("disabled", disabled);
    }

    /////////////////////////////////////////////////
    // Limpiar tablas
    static ClearGrids() {
        $('#_coverageList').UifDataTable('clear');
        $('#_claimsList').UifDataTable('clear');
        $('#_companyCoinsuranceList').UifDataTable('clear');
    }

    /////////////////////////////////////////////////
    // Crear Aviso de Vehiculo
    static ExecuteNoticeOperations() {
        $("#frmNoticeVehicle").validate();
        $("#frmClaimNotice").validate();
        if ($("#frmNoticeVehicle").valid() && $("#frmClaimNotice").valid()) {
            lockScreen();

            var vehicleDTO = {};

            vehicleDTO.Year = $("#ddlVehicleVersionYear").UifSelect('getSelected');
            vehicleDTO.VersionId = $("#ddlVehicleVersion").UifSelect('getSelected');
            vehicleDTO.ModelId = $("#ddlVehicleModel").UifSelect('getSelected');
            vehicleDTO.MakeId = $("#ddlVehicleMake").UifSelect('getSelected');
            vehicleDTO.ColorId = $("#ddlColors").UifSelect('getSelected');
            vehicleDTO.Plate = $("#inputRiskPlate").val();
            vehicleDTO.RiskId = riskId;
            vehicleDTO.DriverName = $("#_driverInformation").val();

            if (_coveredRiskType == null)
                _coveredRiskType = autoCoveredRiskType;

            var noticeVehicleDTO = SetDataNoticeModel();
            var contactInformationModelDTO = SetDataInformation();
            var noticeCoveragesDTO = CoverageList();
            noticeVehicleDTO.Coverages = noticeCoveragesDTO;

            NoticeVehicleRequest.ExecuteNoticeOperations(noticeVehicleDTO, contactInformationModelDTO, vehicleDTO).done(function (response) {
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
                        NoticeVehicle.LockList(false);
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

    static CleanFieldsVehicle() {
        $("#ddlVehicleMake").UifSelect('setSelected', null);
        $("#ddlVehicleModel").UifSelect();
        $("#ddlVehicleVersion").UifSelect();
        $("#ddlVehicleVersionYear").UifSelect();
        $("#ddlColors").UifSelect('setSelected', null);
        $("#inputRiskPlate").val('');
        $("#_driverInformation").val("");
        $("#ddlDamageResponsibility").UifSelect('setSelected', null);
        $("#ddlDamageType").UifSelect('setSelected', null);
        $("#_aditionalInformation").val("");
        $("#inputdriverInformation").val("");
        NoticeVehicle.GetVehicleMakes();
        NoticeVehicle.GetVehicleColors();
    }
}