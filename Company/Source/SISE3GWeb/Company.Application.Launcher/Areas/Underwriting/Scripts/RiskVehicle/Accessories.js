//Codigo de la pagina Accessories.cshtml
var accessoryIndex = null;
var accesoryModifiedStatus = { Status: null, StatusDescription: null};
class RiskVehicleAccessories extends Uif2.Page {
    getInitialState() {
        $("#listAccesories").UifListView({ displayTemplate: "#accessoryTemplate", edit: true, delete: true, customEdit: true, customDelete: true, height: 200 });
        $("#inputAccessoryAmount").OnlyDecimals(UnderwritingDecimal);
        RiskVehicleAccessories.GetRateTypes(0);
        RiskVehicleAccessories.UpdateSummaryAccessories();
    }

    bindEvents() {
        $("#inputAccessoryAmount").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputAccessoryAmount").focusout(Underwriting.FormatMoneyOut);
        $("#btnAccesories").on('click', RiskVehicleAccessories.LoadModalAccessories);
        $("#btnAccessoryAccept").on("click", RiskVehicleAccessories.AddAccessory);
        $('#listAccesories').on('rowAdd', RiskVehicleAccessories.ClearAccessory);
        $('#listAccesories').on('rowEdit', this.AccessoryEdit);
        $('#listAccesories').on('rowDelete', this.AccessoryDelete);
        $("#btnAccessoryCancel").on('click', RiskVehicleAccessories.ClearAccessory);
        $("#btnAccessoryClose").on('click', this.AccessoryClose);
        $("#btnAccessorySave").on('click', this.AccessorySave);
    }

    AccessoryEdit(event, data, index) {
        accessoryIndex = index;
        accesoryModifiedStatus.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Modified)];
        accesoryModifiedStatus.Status = CoverageStatus.Modified;
        RiskVehicleAccessories.SetAccessory(data);
    }

    AccessoryDelete(event, data) {
        RiskVehicleAccessories.ClearAccessory();
        RiskVehicleAccessories.DeleteAccessory(data);
    }

    AccessoryClose() {
        $("#listAccesories").UifListView({ displayTemplate: "#accessoryTemplate", edit: true, delete: true, customEdit: true, customDelete: true, height: 200 });
        RiskVehicleAccessories.LoadAccessories(glbRisk.Accesories);
        RiskVehicleAccessories.UpdateSummaryAccessories();
    }

    AccessorySave() {
        if ($('#inputPriceAccesories').val() != $("#hiddenTotalNoOriginals").val()) {
            $('#inputPriceAccesories').val($("#hiddenTotalNoOriginals").val());
            $('#inputAmountInsured').text(0);
            $('#inputPremium').text(0);
            isnCalculate = false;
        }
        glbRisk.Accesories = $("#listAccesories").UifListView('getData');
        $('#modalAccessories').UifModal("hide");
    }

    static GetRateTypes(selectedIdRate) {
        CoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (selectedIdRate == 0) {
                    $("#selectAccessoryRateType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectAccessoryRateType").UifSelect({ sourceData: data.result, selectedId: selectedIdRate });
                }
                $("#selectAccessoryRateType").prop("disabled", true);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadModalAccessories() {
        if ($("#inputPlate").val().trim() != "") {
            RiskVehicleAccessories.ClearAccessory();
            $("#formAccesories").formReset();
            $('#modalAccessories').UifModal('showLocal', AppResources.LabelTitleAccesories + ': ' + $('#inputPlate').val());
            RiskVehicleAccessories.LoadAccessories(glbRisk.Accesories);
            RiskVehicleAccessories.UpdateSummaryAccessories();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorPlate, 'autoclose': true });
        }
    }

    static LoadAccessories(accessories) {
        $("#listAccesories").UifListView("refresh");
        if (accessories != null) {
            $.each(accessories, function (index, value) {
                $("#listAccesories").UifListView("addItem", this);
            })
        }
    }

    static GetAccessory() {
        var amountAccesory = 0;

        if ($("#inputAccessoryAmount").val().trim() != '') {
            amountAccesory = FormatMoney(NotFormatMoney($("#inputAccessoryAmount").val()))
        }
        var accessory = {
            Id: $("#selectAccessory").UifSelect("getSelected"),
            AccessoryId: 0,
            Description: $("#selectAccessory :selected").text(),
            Make: $("#inputAccessoryMake").val(),
            Amount: amountAccesory,
            Premium: FormatMoney(NotFormatMoney($("#inputAccessoryPremium").val())),
            Rate: $("#inputAccessoryRate").val(),
            RateType: $('#selectAccessoryRateType').UifSelect("getSelected"),
            AccumulatedPremium: $('#hiddenAccumulatedPremium').val(),
            IsOriginal: $("#chkIsOriginal").prop('checked')
        };

        if (localStorage.getItem("Status") != null && localStorage.getItem("Status") == 2) {
            accessory.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Included)];
            accessory.Status = CoverageStatus.Included;
        }
        else if (accesoryModifiedStatus.Status != CoverageStatus.Modified) {
            accessory.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Original)];
            accessory.Status = CoverageStatus.Original;
        }
        else {
            accessory.StatusDescription = accesoryModifiedStatus.StatusDescription;
            accessory.Status = accesoryModifiedStatus.Status;
        }

        if (accessory.IsOriginal) {
            accessory.Original = AppResources.LabelOriginal;
        }
        else {
            accessory.Original = AppResources.LabelNoOriginal;
        }

        return accessory;
    }

    static SetAccessory(accessory) {
        $("#selectAccessory").UifSelect("setSelected", accessory.Id);
        $('#inputAccessoryMake').val(accessory.Make);
        $('#inputAccessoryAmount').val(accessory.Amount);
        $('#chkIsOriginal').prop('checked', accessory.IsOriginal);
        $('#selectAccessoryRateType').UifSelect('setSelected', accessory.RateType);
        $('#inputAccessoryRate').val(accessory.Rate);
        $('#inputAccessoryPremium').val(accessory.Premium);
        $('#hiddenAccumulatedPremium').val(accessory.AccumulatedPremium);
    }

    static AddAccessory() {
        $("#formAccesories").validate();

        if ($("#formAccesories").valid()) {

            if (!$("#chkIsOriginal").prop('checked')) {
                if ($("#inputAccessoryAmount").val().trim() == '' || $("#inputAccessoryAmount").val() == 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorAccessoryAmount, 'autoclose': true });
                    return false;
                }
            }

            if (accessoryIndex == null) {
                if (!RiskVehicleAccessories.ValidateAccessory($("#selectAccessory").UifSelect("getSelected"))) {
                    $("#listAccesories").UifListView("addItem", RiskVehicleAccessories.GetAccessory());
                    RiskVehicleAccessories.UpdateSummaryAccessories();
                    RiskVehicleAccessories.ClearAccessory();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateAccessory, 'autoclose': true });
                }
            }
            else {
                $("#listAccesories").UifListView("editItem", accessoryIndex, RiskVehicleAccessories.GetAccessory());
                RiskVehicleAccessories.UpdateSummaryAccessories();
                RiskVehicleAccessories.ClearAccessory();
            }
        }
    }

    static DeleteAccessory(data) {
        var accessories = $("#listAccesories").UifListView('getData');
        $("#listAccesories").UifListView({ source: null, displayTemplate: "#accessoryTemplate", edit: true, delete: true, customEdit: true, customDelete: true, height: 200 });

        $.each(accessories, function (index, value) {
            if (this.Id != data.Id) {
                $("#listAccesories").UifListView("addItem", this);
            }
            else {
                if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && data.AccessoryId > 0) {
                    this.StatusDescription = AppResources[getAttributes(CoverageStatus, CoverageStatus.Excluded)];
                    this.Status = CoverageStatus.Excluded;
                    this.Amount = FormatMoney(NotFormatMoney(this.Amount) * -1);
                    $("#listAccesories").UifListView("addItem", this);
                }

            }
        });

        RiskVehicleAccessories.UpdateSummaryAccessories();
    }

    static ValidateAccessory(id) {
        var exists = false;

        $.each($("#listAccesories").UifListView('getData'), function (index, value) {
            if (this.Id == id) {
                exists = true;
            }
        });

        return exists;
    }

    static ClearAccessory() {
        accessoryIndex = null;
        $("#selectAccessory").UifSelect("setSelected", null);
        $('#inputAccessoryMake').val('');
        $('#inputAccessoryAmount').val('');
        $('#chkIsOriginal').prop('checked', false);
        $("#selectAccessoryRateType").UifSelect("setSelected", null);
        $('#inputAccessoryRate').val('');
        $('#inputAccessoryPremium').val('');
        $('#hiddenAccumulatedPremium').val('');
    }

    static UpdateSummaryAccessories() {
        var premium = 0;
        var amountInsured = 0;
        var totalNotOriginal = 0;

        $.each($("#listAccesories").UifListView('getData'), function (key, value) {
            if (this.Premium > 0) {
                premium += parseFloat(NotFormatMoney(this.Premium).replace(separatorDecimal, separatorThousands));
            }

            if (this.Amount) {
                amountInsured += parseFloat(NotFormatMoney(this.Amount).replace(separatorDecimal, separatorThousands));
            }

            if (this.IsOriginal == false) {
                totalNotOriginal += parseFloat(NotFormatMoney(this.Amount).replace(separatorDecimal, separatorThousands));
            }
        });

        $("#hiddenTotalNoOriginals").val(FormatMoney(totalNotOriginal, 2));
        $("#labelInsuredValue").text(FormatMoney(amountInsured, 2));
        $("#labelPremium").text(FormatMoney(premium, 2));
    }
}