var guaranteeTmp = {};

class Mortgage extends Uif2.Page {

    getInitialState() {

    }

    bindEvents() {

        $("#Mortage-BuiltArea").ValidatorKey(ValidatorType.Number);
        $("#Mortage-MeasureArea").ValidatorKey(ValidatorType.Number);
        $("#Mortage-ValuationValue").ValidatorKey(ValidatorType.Number);

        $("#Mortage-Currency").on("binded", function () {
            Guarantee.GetdefaultValueCurrencyGuarantee();
        });

        $("#selectGuarantees").UifSelect("setSelected", null);
        window.TypeGuarantee = GuaranteeType.Mortage;

        $("#Mortage-UnitOfMeasure").UifSelect({ selectedId: 1 });

        // << EESGE-172 Control De Busqueda
        $("#inputCountry-Mortage").keyup(function () {
            $("#inputCountry-Mortage").val($("#inputCountry-Mortage").val().toUpperCase());
        });
        $("#inputState-Mortage").keyup(function () {
            $("#inputState-Mortage").val($("#inputState-Mortage").val().toUpperCase());
        });
        $("#inputCity-Mortage").keyup(function () {
            $("#inputCity-Mortage").val($("#inputCity-Mortage").val().toUpperCase());
        });
        $('#inputCountry-Mortage').on("search", function (event, value) {
            $('#inputState-Mortage').val("");
            $('#inputCity-Mortage').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputCountry-Mortage").val().trim().length > 0) {
                Mortgage.GetMortageCountriesByDescription($("#inputCountry-Mortage").val());
            }
        });
        $('#inputState-Mortage').on("search", function (event, value) {
            $('#inputCity-Mortage').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputState-Mortage").val().trim().length > 0) {
                Mortgage.GetMortageStatesByCountryIdByDescription($("#inputCountry-Mortage").data('Id'), $("#inputState-Mortage").val());
            }
        });
        $('#inputCity-Mortage').on("search", function (event, value) {
            $('#inputDaneCodePn').val("");
            if ($("#inputCity-Mortage").val().trim().length > 0) {
                Mortgage.GetMortageCitiesByCountryIdByStateIdByDescription($("#inputCountry-Mortage").data('Id'), $("#inputState-Mortage").data('Id'), $("#inputCity-Mortage").val());
            }
        });
        $('#tblResultListCountriesMortage tbody').on('click', 'tr', function (e) {
            var dataCountry = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }
            $("#inputCountry-Mortage").data(dataCountry);
            $("#inputCountry-Mortage").val(dataCountry.Description);
            $('#modalListSearchCountriesMortage').UifModal('hide');
        });
        $('#tblResultListStatesMortage tbody').on('click', 'tr', function (e) {
            var dataState = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputState-Mortage").data(dataState);
            $("#inputState-Mortage").val(dataState.Description);
            $('#modalListSearchStatesMortage').UifModal('hide');
        });
        $('#tblResultListCitiesMortage tbody').on('click', 'tr', function (e) {
            var dataCities = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputCity-Mortage").data(dataCities);
            $("#inputCity-Mortage").val(dataCities.Description);
            $('#modalListSearchCitiesMortage').UifModal('hide');
        });

        $('#Mortage-ValuationDate').on("datepicker.change", function (event, date) {

            var date = $('#Mortage-ValuationDate').val().split("/");
            if (isExpirationDate(date)) {
                $('#Mortage-ValuationDate').val("");
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateValuatGreaterCurrent + "<br>" })
            }
        })

        $('#Mortage-Company').on('itemSelected', function (event, selectedItem) {
            var dataCompany = { Id: selectedItem.Id, Description: selectedItem.Description }
            $("#Mortage-Company").data(dataCompany);
        });
    }

    //Seccion Funciones
    static showMortage(data) {
        guaranteeTmp = data
        Guarantee.SetCountrieStateCityGuarantee("Mortage", data.Country, data.State, data.City);
        var dataCompany = { Id: data.InsuranceCompany.Id, Description: data.InsuranceCompany.Description }
        $("#Mortage-Address").val(data.Address);
        $("#Mortage-AdjusterNames").val(data.ExpertName);
        $("#Mortage-BuiltArea").val(data.BuiltAreaQuantity);
        $("#Mortage-Company").data(dataCompany);
        $("#Mortage-Company").val(dataCompany.Description);
        $("#Mortage-Currency").val(data.Currency.Id);
        $("#Mortage-DeedNumber").val(data.RegistrationNumber);
        $("#Mortage-InsuredValue").val(FormatMoney(data.InsuranceValueAmount));
        $("#Mortage-MeasureArea").val(data.MeasureAreaQuantity);
        $("#Mortage-NumberPolicy").val(data.PolicyNumber);
        $("#Mortage-Observations").val(data.Description);
        $("#Mortage-UnitOfMeasure").val(data.MeasurementType.Id);
        $("#Mortage-ValuationDate").val(FormatDate(data.AppraisalDate));
        $("#Mortage-ValuationValue").val(FormatMoney(data.AppraisalAmount));
        $("#Mortage-AssetType").val(data.AssetType.Id);
    }

    static clearMortage() {
        $("#AddMortage").get(0).reset();
        $("#Mortage-Address").val("");
        $("#Mortage-DeedNumber").val("");
        $("#Mortage-ValuationValue").val("");
        $("#Mortage-ValuationDate").val("");
        $("#Mortage-AdjusterNames").val("");
        $("#Mortage-MeasureArea").val("");
        $("#Mortage-BuiltArea").val("");
        $("#Mortage-Company").val("");
        $("#Mortage-NumberPolicy").val("");
        $("#Mortage-InsuredValue").val("");
        $("#Mortage-Observations").val("");
        $("#Mortage-AssetType").val("");
        $("#inputCountry-Mortage").data({ Id: null, Description: null });
        $("#inputCountry-Mortage").val("");
        $("#inputState-Mortage").data({ Id: null, Description: null });
        $("#inputState-Mortage").val("");
        $("#inputCity-Mortage").data({ Id: null, Description: null });
        $("#inputCity-Mortage").val("");
        Guarantee.GetdefaultValueCountryGuarantee();
        Guarantee.GetdefaultValueCurrencyGuarantee();
    }

    static SaveMortage() {
        if (Guarantee.ValidateGuarantee()) {
            $("#Guarantee").validate();
            $("#AddMortage").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddMortage").valid();

            if (guaranteeValid && promissoryNoteValid) {
                Mortgage.pushMortage();
            }
        }
    }

    static pushMortage() {

        var ID = 0;
        var dateRegistration;
        if (guaranteeTmp != null) {
            ID = guaranteeTmp.Id;
            dateRegistration = guaranteeTmp.RegistrationDate;
        }

        var MortgageData = {
            Address: $("#Mortage-Address").val(),
            AppraisalAmount: NotFormatMoney($("#Mortage-InsuredValue").val()),
            AppraisalDate: $("#Mortage-ValuationDate").val(),
            AssetType: { Id: $("#Mortage-AssetType").UifSelect("getSelected"), Description: $("#Mortage-AssetType").UifSelect("getSelectedText") },
            Branch: { Id: $("#selectBranchGuarantee").UifSelect("getSelected"), Description: $("#selectBranchGuarantee").UifSelect("getSelectedText") },
            BuiltAreaQuantity: $("#Mortage-BuiltArea").val(),
            City: { Id: $("#inputCity-Mortage").data("Id"), Description: null },
            ClosedInd: $('#IsClosed').is(':checked'),
            Country: { Id: $("#inputCountry-Mortage").data("Id"), Description: null },
            Currency: { Id: $("#Mortage-Currency").UifSelect("getSelected"), Description: null },
            Description: $("#Mortage-Observations").val(),
            ExpertName: $("#Mortage-AdjusterNames").val(),
            RegistrationDate: dateRegistration,
            Guarantee: {
                Id: $("#selectGuaranteeList").UifSelect("getSelected"),
                Description: $("#selectGuaranteeList").UifSelect("getSelectedText"),
                HasApostille: false,
                HasPromissoryNote: false,
                GuaranteeType: { Id: GuaranteeType.Mortage, Description: null }
            },
            Id: ID,
            IndividualId: individualId,
            InsuranceCompany: { Id: $("#Mortage-Company").data("Id"), Description: $("#Mortage-Company").data("Description") },
            InsuranceValueAmount: NotFormatMoney($("#Mortage-ValuationValue").val()),
            MeasureAreaQuantity: $("#Mortage-MeasureArea").val(),
            MeasurementType: { Id: $("#Mortage-UnitOfMeasure").val(), Description: null },
            PolicyNumber: $("#Mortage-NumberPolicy").val(),
            LastChangeDate: DateNowPerson,
            RegistrationNumber: $("#Mortage-DeedNumber").val(),
            State: { Id: $("#inputState-Mortage").data("Id"), Description: null },
            Status: { Id: $("#selectStatusGuarantee").UifSelect("getSelected"), Description: $("#selectStatusGuarantee").UifSelect("getSelectedText") }
        };


        GuaranteeRequest.CreateInsuredGuaranteeMortgage(MortgageData).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $.UifDialog('alert', { 'message': AppResources.SuccessfullyGuaranteesSaved });
                    Guarantee.ClearFormByGuaranteeType(GuaranteeType.Mortage);
                    Guarantee.LoadGuarantees();
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            }
        });

    }

    static GetParameterByDescription(description) {
        var dataResult = '';
        GuaranteeRequest.GetParameterByDescription(description).done(function (data) {
            if (data.success) {
                dataResult = data.result;
            }
        });
        return dataResult;
    }

    static GetMortageCountriesByDescription(Description) {
        if (Description.length >= 3) {
            GuaranteeRequest.GetCountriesByDescription(Description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataCountries = [];
                        $.each(data.result, function (index, value) {
                            dataCountries.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });
                        $('#tblResultListCountriesMortage').UifDataTable('clear');
                        $("#tblResultListCountriesMortage").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountriesMortage').UifModal('showLocal', AppResources.ModalTitleCountries);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundCountries, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundCountries, 'autoclose': true })
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // State
    static GetMortageStatesByCountryIdByDescription(CountryId, Description) {
        if (Description.length >= 3) {
            GuaranteeRequest.GetStatesByCountryIdByDescription(CountryId, Description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataStates = [];
                        $.each(data.result, function (index, value) {
                            dataStates.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });

                        $('#tblResultListStatesMortage').UifDataTable('clear');
                        $("#tblResultListStatesMortage").UifDataTable('addRow', dataStates);
                        $('#modalListSearchStatesMortage').UifModal('showLocal', AppResources.ModalTitleStates);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundStates, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundStates, 'autoclose': true })
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // City
    static GetMortageCitiesByCountryIdByStateIdByDescription(CountryId, StateId, Description) {
        if (Description.length >= 3) {
            GuaranteeRequest.GetCitiesByCountryIdByStateIdByDescription(CountryId, StateId, Description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataCities = [];
                        $.each(data.result, function (index, value) {
                            dataCities.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });

                        $('#tblResultListCitiesMortage').UifDataTable('clear');
                        $("#tblResultListCitiesMortage").UifDataTable('addRow', dataCities);
                        $('#modalListSearchCitiesMortage').UifModal('showLocal', AppResources.ModalTitleCities);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundCities, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundCities, 'autoclose': true })
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

}
