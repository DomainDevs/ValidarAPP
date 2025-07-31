var loadRiskAdditional = false;
class RiskPropertyAdditionalData extends Uif2.Page {
    getInitialState() {


        $("#inputInsured").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputAdditionalDataPrice").OnlyDecimals(UnderwritingDecimal);
        $("#inputLatitude").OnlyGeographicCoordinates("Latitude", AppResources.ErrorLatitude);
        $("#inputLongitude").OnlyGeographicCoordinates("Longitude", AppResources.ErrorLongitude);
        $("#inputAdditionalDataInsured").ValidatorKey(ValidatorType.Onlylettersandnumbers, ValidatorType.Onlylettersandnumbers, 1);
        $("#inputConstructionYear").ValidatorKey(1, 1, 1);
        $("#inputPML").val("100");
        RiskPropertyAdditionalData.GetConstructionType(0);
        RiskPropertyAdditionalData.GetRiskTypes(0);
        //RiskPropertyAdditionalData.GetRiskUses(0);
        RiskPropertyAdditionalData.PeriodDepositPremium(0)
        RiskPropertyAdditionalData.GetDeclarationPeriod(0);

    }
    bindEvents() {
        $("#inputAdditionalDataPrice").focusin(this.inputAdditionalDataPriceFocusin);
        $("#inputAdditionalDataPrice").focusout(this.inputAdditionalDataPriceFocusout);
        $("#btnAdditionalData").on('click', this.btnAdditionalDataOnClick);
        $("#inputAdditionalDataInsured").on('buttonClick', this.inputAdditionalDataInsuredOnClick);
        $("#btnAdditionalDataClose").on("click", this.btnAdditionalDataCloseOnClick);
        $("#btnAdditionalDataSave").on("click", this.btnAdditionalDataSaveOnClick);
        $("#inputConstructionYear").focusout(this.inputConstructionYearFocusout);
    }

    inputConstructionYearFocusout() {
        if ($(this).val() != "" && $.isNumeric($(this).val())) {
            var year = new Date;
            if ($(this).val() > year.getFullYear()) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateConstructionYear, 'autoclose': true });
                $("#inputRiskAge").val(0);
                $("#inputConstructionYear").val(0);
            }
            else {
                $("#inputRiskAge").val(Property.CalculateAgeByYear($(this).val()));
            }

        } else {
            $("#inputRiskAge").val(0);
        }
    }

    btnAdditionalDataSaveOnClick() {
        individualSearchType = 1;
        RiskPropertyAdditionalData.SaveAdditionalData();
    }

    btnAdditionalDataCloseOnClick() {
        individualSearchType = 1;
        RiskPropertyAdditionalData.ClearAdditionalData();
        Property.HidePanelsRisk(MenuType.AdditionalData);
    }

    inputAdditionalDataInsuredOnClick() {
        if ($("#inputAdditionalDataInsured").val().trim().length > 0) {
            Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputAdditionalDataInsured").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
        }
    }

    btnAdditionalDataOnClick() {
        individualSearchType = 3;
        RiskPropertyAdditionalData.LoadAdditionalData();
    }

    inputAdditionalDataPriceFocusout() {
        $(this).val(FormatMoney($(this).val()));
    }

    inputAdditionalDataPriceFocusin() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static LoadAdditionalData() {

        $("#formProperty").validate();
        if (glbRisk.Id == 0) {
            glbRisk.redirectController = MenuType.AdditionalData;
            Property.SaveRisk(MenuType.AdditionalData, 0);
            Property.HidePanelsRisk();
            Property.ShowPanelsRisk(MenuType.AdditionalData);
            RiskPropertyAdditionalData.LoadAdditionalDataRisk();
        }

        if (glbRisk.Id > 0) {
            Property.HidePanelsRisk();
            Property.ShowPanelsRisk(MenuType.AdditionalData);
            RiskPropertyAdditionalData.LoadAdditionalDataRisk();
            if ($("#withNomenclature").is(":checked")) {
                Property.CalculateBlock();
            }
        }



    }

    static LoadAdditionalDataRisk() {
        if (glbRisk.SecondInsured != null) {
            Property.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbRisk.SecondInsured.InsuredId, InsuredSearchType.IndividualId, CustomerType.Individual);
        }
        $("#inputConstructionYear").val(glbRisk.ConstructionYear);
        $("#inputRiskAge").val(Property.CalculateAgeByYear(glbRisk.ConstructionYear));
        if (glbRisk.ConstructionType != null) {
            $("#selectConstructionType").UifSelect("setSelected", glbRisk.ConstructionType.Id);
        }
        else {
            $("#selectConstructionType").UifSelect("setSelected", null);
        }
        $("#inputFloorNumber").val(glbRisk.FloorNumber);
        if (glbRisk.RiskType != null) {
            $("#selectRiskTypeLocation").UifSelect("setSelected", glbRisk.RiskType.Id);
        }
        //if (glbRisk.RiskUse != null) {
        //    $("#selectUseRisk").UifSelect("setSelected", glbRisk.RiskUse.Id);
        //}
        $("#inputLongitude").val(glbRisk.Longitude);
        $("#inputLatitude").val(glbRisk.Latitude);
        $("#inputPML").val(glbRisk.PML);
        $("#inputSquare").val(glbRisk.Square);
        $("#inputFloorNumber").val(glbRisk.FloorNumber);
        if (glbRisk.PrincipalRisk == true) {
            $("#checkPrincipalRisk").prop('checked', true);
        }
        else {
            $("#checkPrincipalRisk").prop('checked', false);
        }
    }
        


    static SaveAdditionalData() {

        $("#formAdditionalData").validate();

        if ($("#formAdditionalData").valid()) {
            var additionalData = $("#formAdditionalData").serializeObject();
            additionalData.ConstructionYear = $("#inputConstructionYear").val();
            additionalData.RiskAge = $("#inputRiskAge").val();
            additionalData.ConstructionType = $("#selectConstructionType").val();
            additionalData.FloorNumber = $("#inputFloorNumber").val();
            additionalData.RiskType = $("#selectRiskTypeLocation").val();
            additionalData.PML = $("#inputPML").val();
            additionalData.Square = $("#inputSquare").val();
            additionalData.PrincipalRisk = $("#checkPrincipalRisk").prop('checked');
            if ($("#inputAdditionalDataInsured").data("Object") != null) {
                additionalData.InsuredId = $("#inputAdditionalDataInsured").data("Object").IndividualId;
            }
            RiskPropertyAdditionalDataRequest.SaveAdditionalData($("#selectRisk").val(), additionalData).done(function (data) {
                if (data.success) {
                    Property.HidePanelsRisk(MenuType.AdditionalData);
                    glbRisk = data.result;
                    Property.LoadSubTitles(4);
                    Property.UpdateGlbRisk(glbRisk);
                }
                else {
                    $.UifNotify('show', { 'type': 'error', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveAdditionalData, 'autoclose': true });
            });
        }
    }

    static ClearAdditionalData() {
        $("#checkPrincipalRisk").is(':checked');
        $("#selectConstructionType").UifSelect("getSelected");
    }

    static GetConstructionType(selectedId) {
        RiskPropertyAdditionalDataRequest.GetConstructionType().done(function (data) {
            if (data.success) {
                $("#selectConstructionType").attr("disabled", false);
                if (selectedId == 0) {
                    $("#selectConstructionType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectConstructionType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }
    static GetRiskTypes(selectedId) {
        RiskPropertyAdditionalDataRequest.GetRiskTypes().done(function (data) {
            if (data.success) {
                $("#selectRiskTypeLocation").attr("disabled", false);
                if (selectedId == 0) {
                    $("#selectRiskTypeLocation").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRiskTypeLocation").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }
    static GetRiskUses(selectedId) {
        RiskPropertyAdditionalDataRequest.GetRiskUses().done(function (data) {
            if (data.success) {
                $("#selectUseRisk").attr("disabled", false);
                if (selectedId == 0) {
                    $("#selectUseRisk").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectUseRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }
    static GetDeclarationPeriod(selectedId) {
        RiskTransportRequest.GetDeclarationPeriods().done(function (data) {
            if (data.success) {
                $("#inputDeclarationPeriod").attr("disabled", false);
                if (selectedId == 0) {
                    $('#inputDeclarationPeriod').UifSelect({ sourceData: data.result, filter: true });
                }
                else {
                    $("#inputDeclarationPeriod").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }

    static PeriodDepositPremium(selectedId) {
        RiskTransportRequest.GetDeclarationPeriods().done(function (data) {
            if (data.success) {
                $("#inputBillingPeriodDepositPremium").attr("disabled", false);
                if (selectedId == 0) {
                    $('#inputBillingPeriodDepositPremium').UifSelect({ sourceData: data.result, filter: true });
                }
                else {
                    $("#inputBillingPeriodDepositPremium").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }
}

