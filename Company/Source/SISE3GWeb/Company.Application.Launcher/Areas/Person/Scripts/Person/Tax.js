var taxDelete = [];
var positionTaxEdit = null;
var dataTaxEdit = {};
var currentDateFrom = null;
var currentDateUntil = null;
var currentOfficialBulletin = null;
var IndividualTaxExeptionIdPerson = 0;
var Id = 0;
var CountriesList = [];


class PersonTax extends Uif2.Page {
    getInitialState() {
        PersonTax.LoadCountries();
        PersonTax.LoadPrefixes();
        PersonTax.LoadBranches();
        PersonTax.LoadEconomicActivitiesTax();
        $("#listVTax").UifListView({
            source: null,
            height: 300,
            displayTemplate: '#display-tax',
            edit: true,
            customEdit: true,
            delete: true,
            deleteCallback: PersonTax.deleteCallback
        });
    }

    bindEvents() {


        $("#btnAgentsAccept").click(this.startAddListVTax);
        $("#btnAgentsCancel").click(PersonTax.clearSelectedTax);
        $("#btnAcceptTax").click(this.startSaveTax);
        //$("#listVTax").click('rowDelete', this.startDeleteTax)
        $("#selectTax").on("itemSelected", this.TaxSelected);
        $("#listVTax").on('rowEdit', this.EditTax);
        $('#modalTax').on('opened.modal', this.modalTaxOpen);
        $("#listVTax").on('rowDelete', this.deleteCallback);
        $("#btnCancelTax").click(PersonTax.clearSelectedTax);
        $('#Country').on('itemSelected', PersonTax.LoadStatesByCountry);
        $('#StateCode').on('itemSelected', PersonTax.LoadCitiesByCountryIdStateId);
        $('#Prefix').on('itemSelected', PersonTax.GetCoveragesByLinesBusinessId);
        $('#InputExtentPercentage').val("0");
    }

    static LoadEconomicActivitiesTax() {
        EconomicActivityTaxRequest.GetEconomicActivitiesTax().done(function (response) {
            if (response.success) {
                $('#EconomicActivity').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetCoveragesByLinesBusinessId(event, selectedItem) {
        var prefixId = selectedItem.Id;

        PersonRequest.GetCoveragesByLinesBusinessId(prefixId).done(function (response) {
            if (response.success) {
                $('#Coverage').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadBranches() {
        BranchRequest.GetBranchs().done(function (response) {
            if (response.success) {
                $('#TechnicalBranch').UifSelect({ sourceData: response.result });
            }
            else {

            }
        });
    }

    static LoadCountries() {
        CountryRequest.GetCountries().done(function (response) {
            if (response.success) {
                CountriesList = response.result;
                $('#Country').UifSelect({ sourceData: response.result, selectedId: $('#inputCountryPn').data("Id") || $('#inputCompanyCountry').data("Id") });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadStatesByCountry(event, selectedItem) {
        var CountryId = selectedItem.Id;
        TaxRequest.GetStatesByStateTaxId(CountryId).done(function (response) {
            if (response.success) {
                $('#StateCode').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadCitiesByCountryIdStateId(event, selectedItem) {
        var countryId = $("#Country").UifSelect("getSelected");
        var stateId = selectedItem.Id;
        TaxRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
            if (response.success) {
                $('#City').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadPrefixes() {
        PrefixRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                $('#Prefix').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static loadDates() {
        PersonRequest.GetCurrentDatetime().done(function (data) {
            if (data.success) {
                var dateFormat = FormatDate(data.result);
                var currentDateUntil = FormatDate(AddToDate(data.result, 30, 0, 0));

                $("#inputDateUntil").UifDatepicker('setValue', FormatDate(currentDateUntil));

                $("#inputDateFrom").UifDatepicker('setValue', dateFormat);

                $("#inputOfficialBulletin").UifDatepicker('setValue', dateFormat);

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }


    TaxSelected(event, selectedItem) {
        PersonTax.clearAttributesTax();
        PersonTax.loadTaxCondition(selectedItem);
        PersonTax.loadTaxCategory(selectedItem);
        PersonTax.ShowHideSelects(selectedItem.Id);
    }

    static clearAttributesTax() {
        $("#Country").UifSelect("setSelected", null);
        $("#StateCode").UifSelect("setSelected", null);
        $("#City").UifSelect("setSelected", null);
        $("#City").UifSelect("disabled", true);
        $("#EconomicActivity").UifSelect("setSelected", null);
        $("#Prefix").UifSelect("setSelected", null);
        $("#Coverage").UifSelect("setSelected", null);
        $("#TechnicalBranch").UifSelect("setSelected", null);
    }

    EditTax(event, data, position) {
        PersonTax.rowEditTax(data, position);
    }

    modalTaxOpen() {

        positionTaxEdit = null;
    }

    static ShowHideSelects(taxId) {
        TaxRequest.getTaxAttributeTypeByTaxId(taxId).done(function (response) {
            var FeeAppliesArray = ["TaxRateConditionDiv", "TaxRateCategoryDiv", "TaxRateStateDiv", "TaxRateCountryDiv", "TaxRateEconomicActivityDiv",
                "TaxRateTechnicalBranchDiv", "TaxRateLineBusinessDiv", "TaxRateCoverageDiv", "TaxRateCityDiv"];

            var FeesApplies = response.result;
            var i = 0;
            var j = 0;

            for (i = 0; i < 9; i++) {
                $('#' + FeeAppliesArray[i] + '').hide();
            }
            for (i = 0; i < 9; i++) {
                for (j = 0; j < FeesApplies.length; j++) {
                    if ((i + 1) == FeesApplies[j].Id) {
                        $('#' + FeeAppliesArray[i] + '').show();
                    }
                }
            }

        });
    }

    static rowEditTax(data, position) {
        positionTaxEdit = position;
        dataTaxEdit = data;
        $("#selectTax").UifSelect("setSelected", data.TaxId);
        $("#selectTax").trigger("itemSelected", $("#selectTax").UifSelect("getSelectedSource"));
        $("#selectRole").UifSelect("setSelected", data.RoleId);

        $("#StateCode").UifSelect("setSelected", data.StateCode);
        ($("#inputDateFrom").UifSelect("setSelected", data.Datefrom));
        ($("#inputDateUntil").UifSelect("setSelected", data.DateUntil));
        ($("#inputOfficialBulletin").UifSelect("setSelected", data.OfficialBulletinDate));
        $("#InputExtentPercentage").val(data.ExtentPercentage);
        $("#InputResolutionNumber").val(data.ResolutionNumber);
        if (data.IndividualTaxExemptionId != 'undefined') {
            IndividualTaxExeptionIdPerson = data.IndividualTaxExemptionId;
        } else {
            IndividualTaxExeptionIdPerson = 0;
        }
        if (data.Id != null) {
            Id = data.Id;
        } else {
            Id = 0;
        }
        TaxRequest.getTaxRateById(data.TaxRateId).done(function (response) {
            if (response.success) {
                dataTaxEdit.TaxCondition = response.result.TaxCondition.Id;
                $("#Country").UifSelect("setSelected", response.result.TaxState.IdCountry);
                $("#StateCode").UifSelect("setSelected", response.result.TaxState.IdState);
                $("#City").UifSelect("setSelected", response.result.TaxState.IdCity);
                $("#EconomicActivity").UifSelect("setSelected", response.result.EconomicActivity.Id);
                $("#Prefix").UifSelect("setSelected", response.result.LineBusiness.Id);
                $("#Coverage").UifSelect("setSelected", response.result.Coverage.Id);
                $("#TechnicalBranch").UifSelect("setSelected", response.result.Branch.Id);

                var controller = rootPath + "Person/Person/GetTaxConditionByTaxId?taxId=" + data.TaxId;
                $("#selectTaxCondition").UifSelect({ source: controller, selectedId: response.result.TaxCondition.Id });
                var controller = rootPath + "Person/Person/GetTaxCategoryByTaxId?taxId=" + data.TaxId;
                $("#selectTaxCategory").UifSelect({ source: controller, selectedId: response.result.TaxCategory.Id });
            }
        });

    }

    static GetCurrentDatetime() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/UniqueUser/DateNow',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static clearTax() {
        $("#selectTax").UifSelect("setSelected", null);
        $("#selectTaxCondition").UifSelect("setSelected", null);
        $("#InputExtentPercentage").val("", null);
        $("#InputResolutionNumber").val("", null);
        $("#StateCode").UifSelect("setSelected", null);

        $("#inputOfficialBulletin").UifDatepicker('setValue', null);
        $("#inputOfficialBulletin").UifDatepicker('setValue', currentDateUntil);
        PersonTax.loadDates();
        if ($("#listVTax").UifListView('getData') > 0) {
            $("#listVTax").UifListView("clear");
        }
    }

    static loadDataTax(tax) {
        taxDelete = [];
        if ($("#listVTax").UifListView('getData').length > 0) {
            $("#listVTax").UifListView("clear");
        }
        $.each(tax, function (index, item) {
            item.DateUntil = (FormatDate(item.DateUntil, 1));
            item.Datefrom = (FormatDate(item.Datefrom, 1));
            item.OfficialBulletinDate = (FormatDate(item.OfficialBulletinDate, 1));
            $("#listVTax").UifListView("addItem", item);
        });
    }

    static loadTaxCondition(selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = rootPath + "Person/Person/GetTaxConditionByTaxId?taxId=" + selectedItem.Id;
            $("#selectTaxCondition").UifSelect({ source: controller, enabled: true });
        }
        else {
            $("#selectTaxCondition").UifSelect();
        }
    }

    static loadTaxCategory(selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = rootPath + "Person/Person/GetTaxCategoryByTaxId?taxId=" + selectedItem.Id;
            $("#selectTaxCategory").UifSelect({ source: controller });
        }
        else {
            $("#selectTaxCategory").UifSelect();
        }
    }

    static clearSelectedTax() {
        $("#selectTax").val(null);
        $("#selectTaxCondition").val(null);
        $("#selectTaxCategory").val(null);
        $('#StateCode').val(null);
        $('#InputExtentPercentage').val("0");
        $('#InputResolutionNumber').val("");
        $('#inputDateFrom').val("");
        $("#inputDateUntil").val("");
        $("#inputOfficialBulletin").val("");
        $("#Country").val("");
        $("#City").val("");
        $("#EconomicActivity").val("");        
        $("#Id").val("");
        PersonTax.loadDates();
        dataTaxEdit = {};
        Id = 0;
        positionTaxEdit = null;
    }

    static getTaxAttributesValue() {
        var TaxAttributesValue = {
            TaxId: $("#selectTax").UifSelect("getSelected"),
            TaxConditionId: $("#selectTaxCondition").UifSelect("getSelected"),
            TaxCategoryId: $("#selectTaxCategory").UifSelect("getSelected"),
            CountryCode: $("#Country").UifSelect("getSelected"),
            StateCode: $("#StateCode").UifSelect("getSelected"),
            CityCode: $("#City").UifSelect("getSelected"),
            EconomicActivityCode: $("#EconomicActivity").UifSelect("getSelected"),
            PrefixId: $("#Prefix").UifSelect("getSelected"),
            CoverageId: $("#Coverage").UifSelect("getSelected"),
            TechnicalBranchId: $("#TechnicalBranch").UifSelect("getSelected")
        };
        return TaxAttributesValue;
    }

    async startAddListVTax() {

        if ($("#formTax").valid()) {

            var TaxAttributesValue = PersonTax.getTaxAttributesValue();
            var response = await TaxRequest.getTaxRateByTaxIdByAttributes(TaxAttributesValue.TaxId, TaxAttributesValue.TaxConditionId, TaxAttributesValue.TaxCategoryId,
                TaxAttributesValue.CountryCode, TaxAttributesValue.StateCode, TaxAttributesValue.CityCode,
                TaxAttributesValue.EconomicActivityCode, TaxAttributesValue.PrefixId,
                TaxAttributesValue.CoverageId, TaxAttributesValue.TechnicalBranchId);

            if (response.success) {
                var taxPerson = PersonTax.createObjectTax();
                if (PersonTax.validateTax(taxPerson) && PersonTax.validateTaxExistsLV(taxPerson)) {

                    dataTaxEdit.CountryId = taxPerson.CountryId;
                    dataTaxEdit.TaxId = taxPerson.TaxId;
                    dataTaxEdit.TaxCondition = taxPerson.TaxCondition;
                    dataTaxEdit.TaxCategory = taxPerson.TaxCategoryId;
                    dataTaxEdit.StateCode = taxPerson.StateCode;
                    dataTaxEdit.Datefrom = taxPerson.Datefrom;
                    dataTaxEdit.DateUntil = taxPerson.DateUntil;
                    dataTaxEdit.ExtentPercentage = taxPerson.ExtentPercentage;
                    dataTaxEdit.OfficialBulletinDate = taxPerson.OfficialBulletinDate;
                    dataTaxEdit.TotalRetention = taxPerson.TotalRetention;
                    dataTaxEdit.ResolutionNumber = taxPerson.ResolutionNumber;
                    dataTaxEdit.IndividualTaxExemptionId = taxPerson.IndividualTaxExemptionId;
                    dataTaxEdit.TaxConditionDescription = taxPerson.TaxConditionDescription;
                    dataTaxEdit.TaxDescription = taxPerson.TaxDescription;
                    dataTaxEdit.TaxCategoryDescription = taxPerson.TaxCategoryDescription;
                    dataTaxEdit.StateCodeDescription = taxPerson.StateCodeDescription;
                    dataTaxEdit.Id = taxPerson.Id;
                    dataTaxEdit.RoleId = taxPerson.RoleId;
                    dataTaxEdit.RoleDescription = taxPerson.RoleDescription;
                    dataTaxEdit.TaxRateId = response.result.Id;
                    if (taxPerson.Id > 0) {
                        dataTaxEdit.status = ParametrizationStatus.Update;
                        $("#listVTax").UifListView("editItem", positionTaxEdit, dataTaxEdit);
                    }
                    else {
                        dataTaxEdit.status = ParametrizationStatus.Create;
                        $("#listVTax").UifListView("addItem", dataTaxEdit);
                    }

                    dataTaxEdit.IndividualId = $('#lblPersonCode').val() || taxPerson.IndividualId;
                    dataTaxEdit.TaxCategoryId = taxPerson.TaxCategoryId;
                    PersonTax.clearSelectedTax();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        }
    }

    static createObjectTax() {
        var taxPerson = {
            CountryId: $('#inputCompanyCountry').data("Id") || $('#inputCountryPn').data().Id,
            TaxId: $("#selectTax").UifSelect("getSelected"),
            TaxDescription: $("#selectTax").UifSelect("getSelectedText"),
            TaxCondition: $("#selectTaxCondition").UifSelect("getSelected"),
            TaxConditionDescription: $("#selectTaxCondition").UifSelect("getSelectedText"),
            TaxCategoryId: $("#selectTaxCategory").UifSelect("getSelected"),
            TaxCategoryDescription: $("#selectTaxCategory").UifSelect("getSelectedText"),
            StateCode: $("#StateCode").UifSelect("getSelected"),
            StateCodeDescription: $("#StateCode").UifSelect("getSelectedText"),
            Datefrom: $("#inputDateFrom").val(),
            DateUntil: $("#inputDateUntil").val(),
            ExtentPercentage: $("#InputExtentPercentage").val(),
            OfficialBulletinDate: $("#inputOfficialBulletin").val(),
            ResolutionNumber: $("#InputResolutionNumber").val(),
            TotalRetention: $('#TotalRetention').is(':checked'),
            IndividualId: individualId,
            IndividualTaxExemptionId: IndividualTaxExeptionIdPerson,
            Id: Id,
            RoleId: $("#selectRole").UifSelect("getSelected"),
            RoleDescription: $("#selectRole").UifSelect("getSelectedText")
        };
        return taxPerson;
    }

    static validateTax(taxPerson) {
        var propertiesRequired = { TaxId: "Impuesto", TaxCondition: "Condicion" };
        var band = true;
        $.each(propertiesRequired, function (index, value) {
            if (taxPerson.TaxId === null || taxPerson.TaxId === "" || taxPerson.TaxCondition === null || taxPerson.TaxCondition === "") {
                band = false;
                $.UifNotify('show', { 'type': 'danger', 'message': value + " " + AppResourcesPerson.Required, 'autoclose': true });
                return band;
            }
        });
        return band;
    }

    static validateTaxExistsLV(taxPerson) {
        var listDataTax = $("#listVTax").UifListView("getData");
        var band = true;

        let value = listDataTax.find(x => x.TaxId == taxPerson.TaxId);
        if (value) {
            if (positionTaxEdit != null) {
                if (dataTaxEdit.TaxId != taxPerson.TaxId || dataTaxEdit.TaxCondition != taxPerson.TaxCondition) {
                    band = false;
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.ErrorTaxExists, 'autoclose': true });
                }
            }
            else {
                band = false;
                $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.ErrorTaxExists, 'autoclose': true });
            }
        }
        return band;
    }

    static addListViewTax(taxPerson) {

        $("#listVTax").UifListView("addItem", taxPerson);
    }

    startSaveTax() {
        var dataTax = $("#listVTax").UifListView("getData");
        if (dataTax != null) {
            $.each(taxDelete, function (id, item) {
                dataTax.push(item);
            });
            PersonTax.saveTax();
        }
    }

    startDeleteTax() {
        var dataTax = $("#listVTax").UifListView("getData");
        if (dataTax != null) {
            $.each(taxDelete, function (id, item) {
                dataTax.push(item);
            });
            PersonTax.deleteTax();
        }
    }

    static saveTax(dataTax) {
        var dataTax = [];

        dataTax = $("#listVTax").UifListView('getData');

        var dataTaxFilter = dataTax.filter(function (item) {
            if (item.StateCode == "" || item.StateCode == null || item.StateCode == undefined) { item.StateCode = 1; item.CountryId = 18; }

            return item.IsModify
        });



        if ($("#listVTax").UifListView('getData').length > 0) {
            var taxes = dataTax.filter(function (dataTaxItem) {
                return dataTaxItem.status != 4;
            })

            lockScreen();
            TaxRequest.SaveTax(taxes).done(function (result) {
                if (result.success && result.result != 'Error to create Tax') {
                    let infringementPoliciesData = false;
                    result.result.forEach(function (data) {
                        if (data.InfringementPolicies != null) {
                            infringementPoliciesData = true;
                        }
                        else {
                            return infringementPoliciesData = false;

                        }
                    });
                    if (infringementPoliciesData) {
                        const infringementPolicies = result.result.map(x => { return x.InfringementPolicies; }).flat();
                        const policyType = LaunchPolicies.ValidateInfringementPolicies(infringementPolicies, true);
                        const countAuthorization = infringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                        if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                            if (countAuthorization > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(infringementPolicies, result.result[0].OperationId, FunctionType.PersonTaxes);
                            }
                        } else {
                            PersonTax.clearTax();
                            PersonTax.loadDataTax(result.result);
                            $("#modalTax").UifModal("hide");
                            dataTax.status = 2;
                            $.UifNotify('show', { 'type': 'info', 'message': 'Impuesto Guardado', 'autoclose': true });
                        }
                    }
                    else {
                        PersonTax.clearTax();
                        PersonTax.loadDataTax(result.result);
                        $("#modalTax").UifModal("hide");
                        dataTax.status = 2;
                        $.UifNotify('show', { 'type': 'info', 'message': 'Impuesto Guardado', 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': result.result, 'autoclose': true });
                    $("#modalTax").UifModal("hide");
                    PersonTax.clearTax();
                }
                unlockScreen();
            }).fail(() => unlockScreen());

        } else {
            $("#modalTax").UifModal("hide");
            PersonTax.clearTax();

        }
    }

    static deleteTax(dataTax) {
        dataTax.status = 4;
        TaxRequest.DeleteTax(dataTax).done(function (data) {

            if (data.success) {

                $.UifNotify('show', { 'type': 'info', 'message': 'Impuesto eliminado', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static deleteCallback(deferred, data) {
        PersonTax.deleteTax(data);
        taxDelete.push(data);
        deferred.resolve();
        PersonTax.clearTax();
    }
}