var guaranteeTmp = {};

class PromissoryNote extends Uif2.Page {

    getInitialState() {
    }

    bindEvents() {

        window.TypeGuarantee = GuaranteeType.PromissoryNote;
        $("#PromissoryNote-ConstitutionDate").UifDatepicker('setValue', GetCurrentFromDate());
        // << EESGE-172 Control De Busqueda
        $("#inputCountry-PromissoryNote").keyup(function () {
            $("#inputCountry-PromissoryNote").val($("#inputCountry-PromissoryNote").val().toUpperCase());
        });
        $("#inputState-PromissoryNote").keyup(function () {
            $("#inputState-PromissoryNote").val($("#inputState-PromissoryNote").val().toUpperCase());
        });
        $("#inputCity-PromissoryNote").keyup(function () {
            $("#inputCity-PromissoryNote").val($("#inputCity-PromissoryNote").val().toUpperCase());
        });
        $('#inputCountry-PromissoryNote').on("search", function (event, value) {
            $('#inputState-PromissoryNote').val("");
            $('#inputCity-PromissoryNote').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputCountry-PromissoryNote").val().trim().length > 0) {
                PromissoryNote.GetPromissoryNoteCountriesByDescription($("#inputCountry-PromissoryNote").val());
            }
        });
        $('#inputState-PromissoryNote').on("search", function (event, value) {
            $('#inputCity-PromissoryNote').val("");
            $('#inputDaneCodePn').val("");
            if ($("#inputState-PromissoryNote").val().trim().length > 0) {
                PromissoryNote.GetPromissoryNoteStatesByCountryIdByDescription($("#inputCountry-PromissoryNote").data('Id'), $("#inputState-PromissoryNote").val());
            }
        });
        $('#inputCity-PromissoryNote').on("search", function (event, value) {
            $('#inputDaneCodePn').val("");
            if ($("#inputCity-PromissoryNote").val().trim().length > 0) {
                PromissoryNote.GetPromissoryNoteCitiesByCountryIdByStateIdByDescription($("#inputCountry-PromissoryNote").data('Id'), $("#inputState-PromissoryNote").data('Id'), $("#inputCity-PromissoryNote").val());
            }
        });
        $('#tblResultListCountriesPromissoryNote tbody').on('click', 'tr', function (e) {
            var dataCountry = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }
            $("#inputCountry-PromissoryNote").data(dataCountry);
            $("#inputCountry-PromissoryNote").val(dataCountry.Description);
            $('#modalListSearchCountriesPromissoryNote').UifModal('hide');
        });
        $('#tblResultListStatesPromissoryNote tbody').on('click', 'tr', function (e) {
            var dataState = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputState-PromissoryNote").data(dataState);
            $("#inputState-PromissoryNote").val(dataState.Description);
            $('#modalListSearchStatesPromissoryNote').UifModal('hide');
        });
        $('#tblResultListCitiesPromissoryNote tbody').on('click', 'tr', function (e) {
            var dataCities = {
                Id: $(this).children()[0].innerHTML,
                Description: $(this).children()[1].innerHTML
            }

            $("#inputCity-PromissoryNote").data(dataCities);
            $("#inputCity-PromissoryNote").val(dataCities.Description);
            $('#modalListSearchCitiesPromissoryNote').UifModal('hide');
        });

        $('#PromissoryNote-ExpirationDate').on("change", function (event, date) {
            var constitution = $('#PromissoryNote-ConstitutionDate').val().split("/");
            var expiration = $('#PromissoryNote-ExpirationDate').val().split("/");
            if (PromissoryNote.validateDate(constitution, expiration)) {
                $('#PromissoryNote-ExpirationDate').val("");
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterDateConst + "<br>" })
            }
        });

        $("#btnGuarantors").on("click", function () {

            $("#Guarantee").validate();
            $("#AddPromissoryNote").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddPromissoryNote").valid();

            if (guaranteeValid && promissoryNoteValid) {
                var guarantee = Guarantee.GetinsuredGuarantee();
                if (guarantee.Id != null) {
                    Guarantors.LoadPartialGuarantors();
                    Guarantors.loadTableGuarantors();
                    $("#IndividualName-Guarantors").text($("#inputContractorName").text());
                    $("#NumberDocument-Guarantors").text(numberDoc);
                } else {
                    $.UifDialog('alert', { 'message': "se debe crear garantia" });
                }
            }
        });
    }

    static validateDate(Date1, Date2) {
        var date1 = Date1[1] + "/" + Date1[0] + "/" + Date1[2]
        var date2 = Date2[1] + "/" + Date2[0] + "/" + Date2[2]
        if ((Date.parse(date1)) > (Date.parse(date2))) {
            return true;
        }
        return false;
    }

    static showPromissoryNote(data) {
        guaranteeTmp = data;
        Guarantee.SetCountrieStateCityGuarantee("PromissoryNote", data.Country, data.State, data.City);
        $("#PromissoryNote-ConstitutionDate").UifDatepicker('setValue', data.RegistrationDate);
        $("#PromissoryNote-Currency").val(data.Currency.Id);
        $("#PromissoryNote-DocumentNumber").val(data.DocumentNumber);
        $("#PromissoryNote-ExpirationDate").UifDatepicker('setValue', data.ExtDate);
        $("#PromissoryNote-NominalValue").val(FormatMoney(data.DocumentValueAmount));
        $("#PromissoryNote-Observations").val(data.Description);
        $("#PromissoryNote-PromissoryNoteType").val(data.PromissoryNoteType.Id);
        $("#PromissoryNote-SignatoriesNumber").val(data.SignatoriesNumber);
    }

    static clearPromissoryNote() {
        $("#AddPromissoryNote").get(0).reset();
        $("#PromissoryNote-ConstitutionDate").val("");
        $("#PromissoryNote-ExpirationDate").val("");
        $("#PromissoryNote-NominalValue").val("");
        //Se agrega para agregar automaticamente el afianzado en persona natural
        if (searchType == 1) {
            $("#PromissoryNote-SignatoriesNumber").val(1);
        }
        else {
            $("#PromissoryNote-SignatoriesNumber").val(0);
        }
        $("#PromissoryNote-Observations").val("");
        $("#PromissoryNote-PromissoryNoteType").val("");
        $("#PromissoryNote-DocumentNumber").val("");
        $("#tableGuarantors").UifListView();
        $("#inputCountry-PromissoryNote").data({ Id: null, Description: null });
        $("#inputCountry-PromissoryNote").val("");
        $("#inputState-PromissoryNote").data({ Id: null, Description: null });
        $("#inputState-PromissoryNote").val("");
        $("#inputCity-PromissoryNote").data({ Id: null, Description: null });
        $("#inputCity-PromissoryNote").val("");
        Guarantee.GetdefaultValueCountryGuarantee();
        Guarantee.GetdefaultValueCurrencyGuarantee();
    }



    static SavePromissoryNote() {
        if (Guarantee.ValidateGuarantee()) {

            $("#Guarantee").validate();
            $("#AddPromissoryNote").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddPromissoryNote").valid();

            if (guaranteeValid && promissoryNoteValid) {
                PromissoryNote.pushPromissoryNote();
            }
        }
    }

    static pushPromissoryNote() {

        var ID = 0;
        var dateRegistration;
        if (guaranteeTmp != null) {
            ID = guaranteeTmp.Id;
            dateRegistration = guaranteeTmp.RegistrationDate;
        }

        var PromissoryNoteData = {
            Id: guaranteeTmp.Id,
            IndividualId: individualId,
            City: { Id: $("#inputCity-PromissoryNote").data("Id"), Description: null },
            Country: { Id: $("#inputCountry-PromissoryNote").data("Id"), Description: null },
            State: { Id: $("#inputState-PromissoryNote").data("Id"), Description: null },
            Branch: { Id: $("#selectBranchGuarantee").UifSelect("getSelected"), Description: $("#selectBranchGuarantee").UifSelect("getSelectedText") },
            Currency: { Id: $("#PromissoryNote-Currency").UifSelect("getSelected"), Description: null },
            ClosedInd: $('#IsClosed').is(':checked'),
            ConstitutionDate: $("#PromissoryNote-ConstitutionDate").val(),
            ExtDate: $("#PromissoryNote-ExpirationDate").val(),
            LastChangeDate: DateNowPerson,
            Status: { Id: $("#selectStatusGuarantee").UifSelect("getSelected"), Description: $("#selectStatusGuarantee").UifSelect("getSelectedText") },
            PromissoryNoteType: { Id: $("#PromissoryNote-PromissoryNoteType").UifSelect("getSelected"), Description: $("#PromissoryNote-PromissoryNoteType").UifSelect("getSelectedText") },
            SignatoriesNumber: $("#PromissoryNote-SignatoriesNumber").val(),
            DocumentNumber: $("#PromissoryNote-DocumentNumber").val(),
            DocumentValueAmount: NotFormatMoney($("#PromissoryNote-NominalValue").val()),
            Description: $("#PromissoryNote-Observations").val(),
            Guarantee: {
                Id: $("#selectGuaranteeList").UifSelect("getSelected"),
                Description: $("#selectGuaranteeList").UifSelect("getSelectedText"),
                HasApostille: false,
                HasPromissoryNote: false,
                GuaranteeType: { Id: GuaranteeType.PromissoryNote, Description: "PAGARE" }
            },
            RegistrationDate: dateRegistration,
            InsuranceCompany: { Id: $("#Mortage-Company").data("Id"), Description: $("#Mortage-Company").data("Description") },
            InsuranceValueAmount: NotFormatMoney($("#Mortage-ValuationValue").val()),
            MeasureAreaQuantity: $("#Mortage-MeasureArea").val(),
            MeasurementType: { Id: $("#Mortage-UnitOfMeasure").val(), Description: null },
            PolicyNumber: $("#Mortage-NumberPolicy").val(),
            RegistrationNumber: $("#Mortage-DeedNumber").val(),
        };

        GuaranteeRequest.CreateInsuredGuaranteePromissoryNote(PromissoryNoteData).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $.UifDialog('alert', { 'message': AppResources.SuccessfullyGuaranteesSaved });
                    Guarantee.ClearFormByGuaranteeType(GuaranteeType.PromissoryNote);
                    Guarantee.LoadGuarantees();
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            }
        });

    }

    static GetPromissoryNoteCountriesByDescription(Description) {
        if (Description.length >= 3) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Person/Guarantee/GetCountriesByDescription',
                data: JSON.stringify({ Description: Description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataCountries = [];
                        $.each(data.result, function (index, value) {
                            dataCountries.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });
                        $('#tblResultListCountriesPromissoryNote').UifDataTable('clear');
                        $("#tblResultListCountriesPromissoryNote").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountriesPromissoryNote').UifModal('showLocal', AppResources.ModalTitleCountries);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundCountries, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageNotFoundCountries, 'autoclose': true })
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryingCountries + ' : ' + data.result.ErrorMsg, 'autoclose': true })
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    static GetPromissoryNoteStatesByCountryIdByDescription(CountryId, Description) {
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

                        $('#tblResultListStatesPromissoryNote').UifDataTable('clear');
                        $("#tblResultListStatesPromissoryNote").UifDataTable('addRow', dataStates);
                        $('#modalListSearchStatesPromissoryNote').UifModal('showLocal', AppResources.ModalTitleStates);
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

    static GetPromissoryNoteCitiesByCountryIdByStateIdByDescription(CountryId, StateId, Description) {
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

                        $('#tblResultListCitiesPromissoryNote').UifDataTable('clear');
                        $("#tblResultListCitiesPromissoryNote").UifDataTable('addRow', dataCities);
                        $('#modalListSearchCitiesPromissoryNote').UifModal('showLocal', AppResources.ModalTitleCities);
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

