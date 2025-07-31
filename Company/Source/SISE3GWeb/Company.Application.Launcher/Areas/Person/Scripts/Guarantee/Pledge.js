var guaranteeTmp = {};

class Pledge extends Uif2.Page {

    getInitialState() {

    }

    bindEvents() {
        window.TypeGuarantee = GuaranteeType.Pledge;
        $("#Pledge-Currency").UifSelect({ selectedId: 0 });

        // << EESGE-172 Control De Busqueda
        $("#inputCountry-Pledge").keyup(function () {
            $("#inputCountry-Pledge").val($("#inputCountry-Pledge").val().toUpperCase());
        });
        $("#inputState-Pledge").keyup(function () {
            $("#inputState-Pledge").val($("#inputState-Pledge").val().toUpperCase());
        });
        $("#inputCity-Pledge").keyup(function () {
            $("#inputCity-Pledge").val($("#inputCity-Pledge").val().toUpperCase());
        });
        $('#inputCountry-Pledge').on("search", function (event, value) {
            $('#inputState-Pledge').val("");
            $('#inputCity-Pledge').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputCountry-Pledge").val().trim().length > 0) {
                GetPledgeCountriesByDescription($("#inputCountry-Pledge").val());
            }
        });
        $('#inputState-Pledge').on("search", function (event, value) {
            $('#inputCity-Pledge').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputState-Pledge").val().trim().length > 0) {
                Pledge.GetPledgeStatesByCountryIdByDescription($("#inputCountry-Pledge").data('Id'), $("#inputState-Pledge").val());
            }
        });
        $('#inputCity-Pledge').on("search", function (event, value) {
            $('#inputDaneCodePn').val("");
            if ($("#inputCity-Pledge").val().trim().length > 0) {
                Pledge.GetPledgeCitiesByCountryIdByStateIdByDescription($("#inputCountry-Pledge").data('Id'), $("#inputState-Pledge").data('Id'), $("#inputCity-Pledge").val());
            }
        });
        $('#tblResultListCountriesPledge tbody').on('click', 'tr', function (e) {
            var dataCountry = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }
            $("#inputCountry-Pledge").data(dataCountry);
            $("#inputCountry-Pledge").val(dataCountry.Description);
            $('#modalListSearchCountriesPledge').UifModal('hide');
        });
        $('#tblResultListStatesPledge tbody').on('click', 'tr', function (e) {
            var dataState = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputState-Pledge").data(dataState);
            $("#inputState-Pledge").val(dataState.Description);
            $('#modalListSearchStatesPledge').UifModal('hide');
        });
        $('#tblResultListCitiesPledge tbody').on('click', 'tr', function (e) {
            var dataCities = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputCity-Pledge").data(dataCities);
            $("#inputCity-Pledge").val(dataCities.Description);
            $('#modalListSearchCitiesPledge').UifModal('hide');
        });

        $('#Pledge-ValuationDate').on("datepicker.change", function (event, date) {

            var date = $('#Pledge-ValuationDate').val().split("/");
            if (isExpirationDate(date)) {
                $('#Pledge-ValuationDate').val("");
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateValuatGreaterCurrent + "<br>" })
            }
        })
    }

    static showPledge(data) {
        guaranteeTmp = data;
        Guarantee.SetCountrieStateCityGuarantee("Pledge", data.Country, data.State, data.City);
        var dataCompany = { Id: data.InsuranceCompany.Id, Description: data.InsuranceCompany.Description }
        $("#Pledge-Company").data(dataCompany);
        $("#Pledge-Company").val(dataCompany.Description);
        $("#Pledge-Chassis").val(data.ChassisNumer);
        $("#Pledge-Currency").val(data.Currency.Id);
        $("#Pledge-Engine").val(data.EngineNumer);
        $("#Pledge-InsuredValue").val(FormatMoney(data.InsuranceValueAmount));
        $("#Pledge-NumberPolicy").val(data.PolicyNumber);
        $("#Pledge-Observations").val(data.Description);
        $("#Pledge-Plate").val(data.LicensePlate);
        $("#Pledge-ValuationDate").val(FormatDate(data.AppraisalDate));
        $("#Pledge-ValuationValue").val(FormatMoney(data.AppraisalAmount));
        $('#IsClosed').prop('checked', data.ClosedInd);
    }

    static clearPledge() {
        $("#AddPledge").get(0).reset();
        $("#Pledge-Plate").val("");
        $("#Pledge-Engine").val("");
        $("#Pledge-Chassis").val("");
        $("#Pledge-ValuationDate").val("");
        $("#Pledge-Company").val("");
        $("#Pledge-NumberPolicy").val("");
        $("#Pledge-InsuredValue").val("");
        $("#Pledge-ValuationValue").val("");
        $("#Pledge-Observations").val("");

        $("#inputCountry-Pledge").data({ Id: null, Description: null });
        $("#inputCountry-Pledge").val("");
        $("#inputState-Pledge").data({ Id: null, Description: null });
        $("#inputState-Pledge").val("");
        $("#inputCity-Pledge").data({ Id: null, Description: null });
        $("#inputCity-Pledge").val("");
        Guarantee.GetdefaultValueCountryGuarantee();
    }

    static SavePledge() {
        if (Guarantee.ValidateGuarantee()) {
            $("#Guarantee").validate();
            $("#AddPledge").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddPledge").valid();

            if (guaranteeValid && promissoryNoteValid) {
                Pledge.pushPledge();
            }
        }
    }

    static pushPledge() {
        var ID = 0;
        var dateRegistration;
        if (guaranteeTmp != null) {
            ID = guaranteeTmp.Id;
            dateRegistration = guaranteeTmp.RegistrationDate;
        }
        var PledgeData = {
            AppraisalAmount: NotFormatMoney($("#Pledge-ValuationValue").val()),
            AppraisalDate: $("#Pledge-ValuationDate").val(),
            Branch: { Id: $("#selectBranchGuarantee").UifSelect("getSelected"), Description: $("#selectBranchGuarantee").UifSelect("getSelectedText") },
            City: { Id: $("#inputCity-Pledge").data("Id"), Description: null },
            ClosedInd: $('#IsClosed').is(':checked'),
            Country: { Id: $("#inputCountry-Pledge").data("Id"), Description: null },
            ChassisNumer: $("#Pledge-Chassis").val(),
            Currency: { Id: $("#Pledge-Currency").UifSelect("getSelected"), Description: $("#Pledge-Currency").UifSelect("getSelectedText") },
            Description: $("#Pledge-Observations").val(),
            EngineNumer: $("#Pledge-Engine").val(),
            Guarantee: {
                Id: $("#selectGuaranteeList").UifSelect("getSelected"),
                Description: $("#selectGuaranteeList").UifSelect("getSelectedText"),
                HasApostille: false,
                HasPromissoryNote: false,
                GuaranteeType: { Id: GuaranteeType.Pledge, Description: null }
            },
            RegistrationDate: dateRegistration,
            Id: ID,
            IndividualId: individualId,
            InsuranceCompany: { Id: $("#Pledge-Company").data("Id"), Description: $("#Pledge-Company").data("Description") },
            InsuranceValueAmount: NotFormatMoney($("#Pledge-InsuredValue").val()),
            PolicyNumber: $("#Pledge-NumberPolicy").val(),
            LastChangeDate: DateNowPerson,
            LicensePlate: $("#Pledge-Plate").val(),
            State: { Id: $("#inputState-Pledge").data("Id"), Description: null },
            Status: { Id: $("#selectStatusGuarantee").UifSelect("getSelected"), Description: $("#selectStatusGuarantee").UifSelect("getSelectedText") }
        };

        GuaranteeRequest.CreateInsuredGuaranteePledge(PledgeData).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $.UifDialog('alert', { 'message': AppResources.SuccessfullyGuaranteesSaved });
                    Guarantee.ClearFormByGuaranteeType(GuaranteeType.Pledge);
                    Guarantee.LoadGuarantees();
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            }
        });

    }

    static GetPledgeCountriesByDescription(Description) {
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
                        $('#tblResultListCountriesPledge').UifDataTable('clear');
                        $("#tblResultListCountriesPledge").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountriesPledge').UifModal('showLocal', AppResources.ModalTitleCountries);
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

    static GetPledgeStatesByCountryIdByDescription(countryId, Description) {
        if (Description.length >= 3) {
            GuaranteeRequest.GetStatesByCountryIdByDescription(countryId, Description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataStates = [];
                        $.each(data.result, function (index, value) {
                            dataStates.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });

                        $('#tblResultListStatesPledge').UifDataTable('clear');
                        $("#tblResultListStatesPledge").UifDataTable('addRow', dataStates);
                        $('#modalListSearchStatesPledge').UifModal('showLocal', AppResources.ModalTitleStates);
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
    static GetPledgeCitiesByCountryIdByStateIdByDescription(CountryId, StateId, Description) {
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
                        $('#tblResultListCitiesPledge').UifDataTable('clear');
                        $("#tblResultListCitiesPledge").UifDataTable('addRow', dataCities);
                        $('#modalListSearchCitiesPledge').UifModal('showLocal', AppResources.ModalTitleCities);
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



