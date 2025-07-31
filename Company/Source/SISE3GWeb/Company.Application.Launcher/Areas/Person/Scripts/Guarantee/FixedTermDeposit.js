
class FixedTermDeposit extends Uif2.Page {

    getInitialState() { }

    bindEvents() {
        $("#FixedTermDeposit-NominalValue").ValidatorKey(ValidatorType.Number);
        //Seccion Eventos
        $("#FixedTermDeposit-ConstitutionDate").on("datepicker.change", function (event, date) {
            var date = $("#FixedTermDeposit-ConstitutionDate").val().split("/");
            if (isExpirationDate(date)) {
                $("#FixedTermDeposit-ConstitutionDate").val("");
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateConstGreaterCurrent + "<br>" })
            }
        });
        $("#FixedTermDeposit-ExpirationDate").on("datepicker.change", function (event, date) {
            var date = $("#FixedTermDeposit-ExpirationDate").val().split("/");
            if (isExpirationDate(date)) {
                $("#FixedTermDeposit-ExpirationDate").val("");
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterCurrent + "<br>" })
            }
        });
        window.TypeGuarantee = GuaranteeType.Fixedtermdeposit;
        $("#FixedTermDeposit-Currency").UifSelect({ selectedId: 0 });
        // << EESGE-172 Control De Busqueda
        $("#inputCountry-FixedTermDeposit").keyup(function () {
            $("#inputCountry-FixedTermDeposit").val($("#inputCountry-FixedTermDeposit").val().toUpperCase());
        });
        $("#inputState-FixedTermDeposit").keyup(function () {
            $("#inputState-FixedTermDeposit").val($("#inputState-FixedTermDeposit").val().toUpperCase());
        });
        $("#inputCity-FixedTermDeposit").keyup(function () {
            $("#inputCity-FixedTermDeposit").val($("#inputCity-FixedTermDeposit").val().toUpperCase());
        });
        $('#inputCountry-FixedTermDeposit').on("search", function (event, value) {
            $('#inputState-FixedTermDeposit').val("");
            $('#inputCity-FixedTermDeposit').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputCountry-FixedTermDeposit").val().trim().length > 0) {
                FixedTermDeposit.GetFixedTermDepositCountriesByDescription($("#inputCountry-FixedTermDeposit").val());
            }
        });
        $('#inputState-FixedTermDeposit').on("search", function (event, value) {
            $('#inputCity-FixedTermDeposit').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputState-FixedTermDeposit").val().trim().length > 0) {
                FixedTermDeposit.GetFixedTermDepositStatesByCountryIdByDescription($("#inputCountry-FixedTermDeposit").data('Id'), $("#inputState-FixedTermDeposit").val());
            }
        });
        $('#inputCity-FixedTermDeposit').on("search", function (event, value) {
            $('#inputDaneCodePn').val("");
            if ($("#inputCity-FixedTermDeposit").val().trim().length > 0) {
                FixedTermDeposit.GetFixedTermDepositCitiesByCountryIdByStateIdByDescription($("#inputCountry-FixedTermDeposit").data('Id'), $("#inputState-FixedTermDeposit").data('Id'), $("#inputCity-FixedTermDeposit").val());
            }
        });
        $('#tblResultListCountriesFTD tbody').on('click', 'tr', function (e) {
            var dataCountry = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }
            $("#inputCountry-FixedTermDeposit").data(dataCountry);
            $("#inputCountry-FixedTermDeposit").val(dataCountry.Description);
            $('#modalListSearchCountriesFTD').UifModal('hide');
        });
        $('#tblResultListStatesFTD tbody').on('click', 'tr', function (e) {
            var dataState = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputState-FixedTermDeposit").data(dataState);
            $("#inputState-FixedTermDeposit").val(dataState.Description);
            $('#modalListSearchStatesFTD').UifModal('hide');
        });
        $('#tblResultListCitiesFTD tbody').on('click', 'tr', function (e) {
            var dataCities = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputCity-FixedTermDeposit").data(dataCities);
            $("#inputCity-FixedTermDeposit").val(dataCities.Description);
            $('#modalListSearchCitiesFTD').UifModal('hide');
        });
    }

    static showFixedTermDeposit(data) {
        guaranteeTmp = data;
        Guarantee.SetCountrieStateCityGuarantee("FixedTermDeposit", data.Country, data.State, data.City);
        $("#FixedTermDeposit-ConstitutionDate").val(FormatDate(data.RegistrationDate));
        $("#FixedTermDeposit-Currency").val(data.Currency.Id);
        $("#FixedTermDeposit-DocumentNumber").val(data.DocumentNumber);
        $("#FixedTermDeposit-ExpirationDate").val(FormatDate(data.ExtDate));
        $("#FixedTermDeposit-IssuingEntity").val(data.IssuerName);
        $("#FixedTermDeposit-NominalValue").val(FormatMoney(data.DocumentValueAmount));
        $("#FixedTermDeposit-Observations").val(data.Description);
    }

    static clearFixedTermDeposit() {
        $("#AddFixedTermDeposit").get(0).reset();
        $("#FixedTermDeposit-DocumentNumber").val("");
        $("#FixedTermDeposit-IssuingEntity").val("");
        $("#FixedTermDeposit-ConstitutionDate").val("");
        $("#FixedTermDeposit-ExpirationDate").val("");
        $("#FixedTermDeposit-NominalValue").val("");
        $("#FixedTermDeposit-Observations").val("");
        $("#inputCountry-FixedTermDeposit").data({ Id: null, Description: null });
        $("#inputCountry-FixedTermDeposit").val("");
        $("#inputState-FixedTermDeposit").data({ Id: null, Description: null });
        $("#inputState-FixedTermDeposit").val("");
        $("#inputCity-FixedTermDeposit").data({ Id: null, Description: null });
        $("#inputCity-FixedTermDeposit").val("");
        Guarantee.GetdefaultValueCountryGuarantee();
    }

    static SaveFixedTermDeposit() {
        var data = Guarantee.GetinsuredGuarantee();
        if (Guarantee.ValidateGuarantee()) {
            $("#Guarantee").validate();
            $("#AddFixedTermDeposit").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddFixedTermDeposit").valid();

            if (guaranteeValid && promissoryNoteValid) {
                FixedTermDeposit.pushFixedTermDeposit(data.Id);
            }
        }
    }

    static pushFixedTermDeposit(id) {
        var ID = 0;
        if (guaranteeTmp != null) {
            ID = guaranteeTmp.Id;
        }

        var FixedTermDepositData = {
            Id: guaranteeTmp.Id,
            IndividualId: individualId,
            City: { Id: $("#inputCity-FixedTermDeposit").data("Id"), Description: null },
            Country: { Id: $("#inputCountry-FixedTermDeposit").data("Id"), Description: null },
            State: { Id: $("#inputState-FixedTermDeposit").data("Id"), Description: null },
            Branch: { Id: $("#selectBranchGuarantee").UifSelect("getSelected"), Description: $("#selectBranchGuarantee").UifSelect("getSelectedText") },
            Currency: { Id: $("#FixedTermDeposit-Currency").UifSelect("getSelected"), Description: null },
            ClosedInd: $('#IsClosed').is(':checked'),
            RegistrationDate: $("#FixedTermDeposit-ConstitutionDate").val(),
            ConstitutionDate: $("#FixedTermDeposit-ConstitutionDate").val(),
            ExtDate: $("#FixedTermDeposit-ExpirationDate").val(),
            LastChangeDate: DateNowPerson,
            Status: { Id: $("#selectStatusGuarantee").UifSelect("getSelected"), Description: $("#selectStatusGuarantee").UifSelect("getSelectedText") },
            DocumentNumber: $("#FixedTermDeposit-DocumentNumber").val(),
            DocumentValueAmount: NotFormatMoney($("#FixedTermDeposit-NominalValue").val()),
            Description: $("#FixedTermDeposit-Observations").val(),
            Guarantee: {
                Id: $("#selectGuaranteeList").UifSelect("getSelected"),
                Description: $("#selectGuaranteeList").UifSelect("getSelectedText"),
                HasApostille: false,
                HasPromissoryNote: false,
                GuaranteeType: { Id: GuaranteeType.Fixedtermdeposit, Description: null }
            },
            IssuerName: ($("#FixedTermDeposit-IssuingEntity").val() != "" ? $("#FixedTermDeposit-IssuingEntity").val() : "N/A")
        };

        GuaranteeRequest.CreateInsuredGuaranteeFixedTermDeposit(FixedTermDepositData).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $.UifDialog('alert', { 'message': AppResources.SuccessfullyGuaranteesSaved });
                    Guarantee.ClearFormByGuaranteeType(GuaranteeType.Fixedtermdeposit);
                    Guarantee.LoadGuarantees();
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            }
        });

    }

    static GetFixedTermDepositCountriesByDescription() {
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
                        $('#tblResultListCountriesFTD').UifDataTable('clear');
                        $("#tblResultListCountriesFTD").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountriesFTD').UifModal('showLocal', AppResources.ModalTitleCountries);
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

    static GetFixedTermDepositStatesByCountryIdByDescription(CountryId, Description) {
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

                        $('#tblResultListStatesFTD').UifDataTable('clear');
                        $("#tblResultListStatesFTD").UifDataTable('addRow', dataStates);
                        $('#modalListSearchStatesFTD').UifModal('showLocal', AppResources.ModalTitleStates);
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

    static GetFixedTermDepositCitiesByCountryIdByStateIdByDescription(CountryId, StateId, Description) {
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

                        $('#tblResultListCitiesFTD').UifDataTable('clear');
                        $("#tblResultListCitiesFTD").UifDataTable('addRow', dataCities);
                        $('#modalListSearchCitiesFTD').UifModal('showLocal', AppResources.ModalTitleCities);
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