//Codigo de la pagina AdditionalData.cshtml
class RiskVehicleAdditionalData extends Uif2.Page {
    getInitialState() {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputAdditionalDataPrice").attr("disabled", true);
        }
        else {
            $("#inputAdditionalDataPrice").attr("disabled", false);
        }
        $("#inputAdditionalDataInsured").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputAdditionalDataPrice").OnlyDecimals(UnderwritingDecimal);        
    }

    bindEvents() {
        $("#inputAdditionalDataPrice").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputAdditionalDataPrice").focusout(Underwriting.FormatMoneyOut);
        $("#btnAdditionalData").on('click', this.AdditionalDataLoad);
        $("#inputAdditionalDataInsured").on('buttonClick', this.SearchInsured);
        $("#btnAdditionalDataClose").on("click", this.AdditionalDataClose);
        $("#btnAdditionalDataSave").on("click", this.AdditionalDataSave);
    }

    AdditionalDataLoad() {
        individualSearchType = 3;
        RiskVehicleAdditionalData.LoadAdditionalData();
    }

    SearchInsured() {
        if ($("#inputAdditionalDataInsured").val().trim().length > 0) {
            RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputAdditionalDataInsured").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
        }
    }

    AdditionalDataClose() {
        individualSearchType = 1;
        RiskVehicle.ShowPanelsRisk(MenuType.Risk);
    }

    AdditionalDataSave() {
        individualSearchType = 1;
        RiskVehicleAdditionalData.SaveAdditionalData();
    }

    static LoadAdditionalData() {
        $("#formVehicle").validate();
        if (glbRisk.Id == 0) {
            glbRisk.redirectController = MenuType.AdditionalData;
            RiskVehicle.SaveRisk(MenuType.AdditionalData, 0);
        }

        if (glbRisk.Id > 0) {
            RiskVehicle.HidePanelsRisk();
            RiskVehicle.ShowPanelsRisk(MenuType.AdditionalData);
            RiskVehicleAdditionalData.LoadAdditionalDataRisk();
        }
    }

    static LoadAdditionalDataRisk() {
        if (glbRisk.SecondInsured != null) {
            $("#inputAdditionalDataInsured").data("Object", glbRisk.SecondInsured);
            $("#inputAdditionalDataInsured").data("Detail", RiskVehicle.GetIndividualDetails(glbRisk.SecondInsured));
            $("#inputAdditionalDataInsured").val(glbRisk.SecondInsured.Name);
        }
        $("#selectFuelType").UifSelect("setSelected", glbRisk.Version.Fuel.Id);
        if (glbRisk.Version.Body != null) {
            $("#selectBodyType").UifSelect("setSelected", glbRisk.Version.Body.Id);
        }

        $("#inputAdditionalDataPrice").val(FormatMoney(glbRisk.NewPrice));
    }

    static SaveAdditionalData() {

        $("#formAdditionalData").validate();

        if ($("#formAdditionalData").valid()) {
            var additionalData = $("#formAdditionalData").serializeObject();
            additionalData.NewPrice = NotFormatMoney($("#inputAdditionalDataPrice").val());

            if ($("#inputAdditionalDataInsured").data("Object") != null) {
                additionalData.InsuredId = $("#inputAdditionalDataInsured").data("Object").IndividualId;
            }
            var riskSelected = $("#selectRisk").UifSelect("getSelected");
            var riskId = (riskSelected === null || riskSelected === undefined || riskSelected==="") ? currentSavedRisk.Id : riskSelected;
            RiskVehicleAdditionalDataRequest.SaveAdditionalData(riskId, additionalData).done(function (data) {
                if (data.success) {
                    RiskVehicle.UpdateGlbRisk(data.result);
                    RiskVehicle.ShowPanelsRisk(MenuType.Risk);
                    RiskVehicle.LoadSubTitles(4);
                    $("#modalAdditionalData").UifModal('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveAdditionalData, 'autoclose': true });

            });
        }
    }    

    static GetFuels(selectedId) {
        RiskVehicleAdditionalDataRequest.GetFuels().done(function(data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectFuelType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectFuelType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }

    static GetBodies(selectedId) {
        RiskVehicleAdditionalDataRequest.GetBodies().done(function(data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectBodyType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectBodyType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });
    }
}