var objFixedTermDeposit =
    {
        bindEvents: function () {
            $("#FixedTermDeposit-NominalValue").ValidatorKey(ValidatorType.Number);
            //Seccion Eventos
            $("#FixedTermDeposit-ConstitutionDate").on("datepicker.change", function (event, date) {
                var constitution = $('#FixedTermDeposit-ConstitutionDate').val().split("/");
                var expiration = $('#FixedTermDeposit-ExpirationDate').val().split("/");
                if (objPromissoryNote.validateDate(constitution, expiration)) {
                    $('#FixedTermDeposit-ExpirationDate').val("");
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterDateConst + "<br>" })
                }
            });
            $("#FixedTermDeposit-ExpirationDate").on("datepicker.change", function (event, date) {
                var constitution = $('#FixedTermDeposit-ConstitutionDate').val().split("/");
                var expiration = $('#FixedTermDeposit-ExpirationDate').val().split("/");
                if (objPromissoryNote.validateDate(constitution, expiration)) {
                    $('#FixedTermDeposit-ExpirationDate').val("");
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterDateConst + "<br>" })
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
                    objFixedTermDeposit.GetFixedTermDepositCountriesByDescription($("#inputCountry-FixedTermDeposit").val());
                }
            });
            $('#inputState-FixedTermDeposit').on("search", function (event, value) {
                $('#inputCity-FixedTermDeposit').val("");
                $('#inputDaneCodePn').val("");
                if ($("#inputState-FixedTermDeposit").val().trim().length > 0) {
                    objFixedTermDeposit.GetFixedTermDepositStatesByCountryIdByDescription($("#inputCountry-FixedTermDeposit").data('Id'), $("#inputState-FixedTermDeposit").val());
                }
            });
            $('#inputCity-FixedTermDeposit').on("search", function (event, value) {
                $('#inputDaneCodePn').val("");
                if ($("#inputCity-FixedTermDeposit").val().trim().length > 0) {
                    objFixedTermDeposit.GetFixedTermDepositCitiesByCountryIdByStateIdByDescription($("#inputCountry-FixedTermDeposit").data('Id'), $("#inputState-FixedTermDeposit").data('Id'), $("#inputCity-FixedTermDeposit").val());
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
        },

        //Seccion Funciones
        showFixedTermDeposit: function (data) {
            Guarantee.SetCountrieStateCityGuarantee("FixedTermDeposit", data.InsuredGuarantee.Country, data.InsuredGuarantee.State, data.InsuredGuarantee.City);
            $("#FixedTermDeposit-ConstitutionDate").val(data.InsuredGuarantee.RegistrationDate);
            $("#FixedTermDeposit-Currency").val(data.InsuredGuarantee.Currency.Id);
            $("#FixedTermDeposit-DocumentNumber").val(data.InsuredGuarantee.DocumentNumber);
            $("#FixedTermDeposit-ExpirationDate").val(FormatDate(data.InsuredGuarantee.ExpirationDate));
            $("#FixedTermDeposit-IssuingEntity").val(data.InsuredGuarantee.IssuerName);
            $("#FixedTermDeposit-NominalValue").val(FormatMoney(data.InsuredGuarantee.DocumentValueAmount));
            $("#FixedTermDeposit-Observations").val(data.InsuredGuarantee.Description);
        },

        clearFixedTermDeposit: function () {
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
        },

        SaveFixedTermDeposit: function () {

            if (Guarantee.ValidateGuarantee()) {
                $("#Guarantee").validate();
                $("#AddFixedTermDeposit").validate();

                var guaranteeValid = $("#Guarantee").valid();
                var promissoryNoteValid = $("#AddFixedTermDeposit").valid();

                if (guaranteeValid && promissoryNoteValid) {
                    objFixedTermDeposit.pushFixedTermDeposit(guaranteeId);
                    return true;
                }
            }
            return false;
        },

        pushFixedTermDeposit: function (id) {

            var result = Guarantee.existGuarantee(id);
            var dataResult;

            if (result >= 0) {
                guarantee[result].Apostille = false;
                guarantee[result].InsuredGuarantee.Branch.Id = $("#selectBranchGuarantee").val();
                if (guarantee[result].InsuredGuarantee.GuaranteeStatus== undefined) {
                    guarantee[result].InsuredGuarantee.GuaranteeStatus = {};
                }
                guarantee[result].InsuredGuarantee.GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                guarantee[result].InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;

                guarantee[result].InsuredGuarantee.Country.Id = $("#inputCountry-FixedTermDeposit").data("Id");
                guarantee[result].InsuredGuarantee.City.Id = $("#inputCity-FixedTermDeposit").data("Id");
                guarantee[result].InsuredGuarantee.State.Id = $("#inputState-FixedTermDeposit").data("Id");

                guarantee[result].InsuredGuarantee.RegistrationDate = $("#FixedTermDeposit-ConstitutionDate").val();
                guarantee[result].InsuredGuarantee.Currency.Id = $("#FixedTermDeposit-Currency").val();
                guarantee[result].InsuredGuarantee.DocumentNumber = $("#FixedTermDeposit-DocumentNumber").val();
                guarantee[result].InsuredGuarantee.ExpirationDate = $("#FixedTermDeposit-ExpirationDate").val();
                guarantee[result].InsuredGuarantee.IssuerName = ($("#FixedTermDeposit-IssuingEntity").val() != "" ? $("#FixedTermDeposit-IssuingEntity").val() : "N/A");
                guarantee[result].InsuredGuarantee.DocumentValueAmount = NotFormatMoney($("#FixedTermDeposit-NominalValue").val());
                guarantee[result].InsuredGuarantee.Description = $("#FixedTermDeposit-Observations").val();
                guarantee[result].InsuredGuarantee.LastChangeDate = FormatDate(new Date());
                guarantee[result].InsuredGuarantee.Apostille = false;
                guarantee[result].InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status = GuaranteeStatus;

                dataResult = guarantee[result];
            }
            else {
                guaranteeTmp.Code = GuaranteeType.Fixedtermdeposit;
                guaranteeTmp.Description = "CERTIFICADO DE DEPOSITO A TERMINO (CDTïS)";
                guaranteeTmp.Apostille = false;


                var guaranteeType = {};
                guaranteeType.Code = GuaranteeType.Fixedtermdeposit;
                guaranteeType.Description = "CERTIFICADO DE DEPOSITO A TERMINO (CDTïS)";

                guaranteeTmp.GuaranteeType = guaranteeType;

                var Branch = {};
                Branch.Id = $("#selectBranchGuarantee").val();

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text()

                var City = {};
                City.Id = $("#inputCity-FixedTermDeposit").data("Id");

                var State = {};
                State.Id = $("#inputState-FixedTermDeposit").data("Id");

                var Country = {};
                Country.Id = $("#inputCountry-FixedTermDeposit").data("Id");
                
                var Currency = {};
                Currency.Id = $("#FixedTermDeposit-Currency").val();

                guaranteeTmp.InsuredGuarantee.Code = guaranteeCode;
                guaranteeTmp.InsuredGuarantee.IndividualId = individualId;
                guaranteeTmp.InsuredGuarantee.Branch = Branch;

                guaranteeTmp.InsuredGuarantee.Currency = Currency;
                guaranteeTmp.InsuredGuarantee.Status = GuaranteeStatus;
                guaranteeTmp.InsuredGuarantee.City = City;
                guaranteeTmp.InsuredGuarantee.City.State = State;
                guaranteeTmp.InsuredGuarantee.City.State.Country = Country;
                guaranteeTmp.InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                guaranteeTmp.InsuredGuarantee.RegistrationDate = $("#FixedTermDeposit-ConstitutionDate").val();
                guaranteeTmp.InsuredGuarantee.DocumentNumber = $("#FixedTermDeposit-DocumentNumber").val();
                guaranteeTmp.InsuredGuarantee.ExpirationDate = $("#FixedTermDeposit-ExpirationDate").val();
                guaranteeTmp.InsuredGuarantee.IssuerName = ($("#FixedTermDeposit-IssuingEntity").val() != "" ? $("#FixedTermDeposit-IssuingEntity").val() : "N/A");
                guaranteeTmp.InsuredGuarantee.DocumentValueAmount = NotFormatMoney($("#FixedTermDeposit-NominalValue").val());
                guaranteeTmp.InsuredGuarantee.Description = $("#FixedTermDeposit-Observations").val();
                guaranteeTmp.InsuredGuarantee.LastChangeDate = FormatDate(new Date());
                guaranteeTmp.InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;

                guaranteeTmp.InsuredGuarantee.Apostille = false;
                dataResult = guaranteeTmp;
            }
            Guarantee.saveGuarantee(dataResult, GuaranteeType.Fixedtermdeposit);
        },

        GetFixedTermDepositCountriesByDescription: function (Description) {
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
        },
        // State
        GetFixedTermDepositStatesByCountryIdByDescription: function (CountryId, Description) {
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
        },
        // City
        GetFixedTermDepositCitiesByCountryIdByStateIdByDescription: function (CountryId, StateId, Description) {
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

