var objMortage =
    {
        bindEvents: function () {
            $("#Mortage-BuiltArea").ValidatorKey(ValidatorType.Number);
            $("#Mortage-MeasureArea").ValidatorKey(ValidatorType.Number);
            $("#Mortage-ValuationValue").ValidatorKey(ValidatorType.Number);

            objMortage.bindEventsMortage();
            objActions.bindEventsActions();
        },

        bindEventsMortage: function () {
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
                    objMortage.GetMortageCountriesByDescription($("#inputCountry-Mortage").val());
                }
            });
            $('#inputState-Mortage').on("search", function (event, value) {
                $('#inputCity-Mortage').val("");
                $('#inputDaneCodePn').val("");
                if ($("#inputState-Mortage").val().trim().length > 0) {
                    objMortage.GetMortageStatesByCountryIdByDescription($("#inputCountry-Mortage").data('Id'), $("#inputState-Mortage").val());
                }
            });
            $('#inputCity-Mortage').on("search", function (event, value) {
                $('#inputDaneCodePn').val("");
                if ($("#inputCity-Mortage").val().trim().length > 0) {
                    objMortage.GetMortageCitiesByCountryIdByStateIdByDescription($("#inputCountry-Mortage").data('Id'), $("#inputState-Mortage").data('Id'), $("#inputCity-Mortage").val());
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
        },

        //Seccion Funciones
        showMortage: function (data) {
            Guarantee.SetCountrieStateCityGuarantee("Mortage", data.InsuredGuarantee.Country, data.InsuredGuarantee.State, data.InsuredGuarantee.City);
            $("#Mortage-Address").val(data.InsuredGuarantee.Address);
            $("#Mortage-AdjusterNames").val(data.InsuredGuarantee.ExpertName);
            $("#Mortage-BuiltArea").val(data.InsuredGuarantee.BuiltArea);
            $("#Mortage-Company").val((data.InsuredGuarantee.InsuranceCompany != null) ? data.InsuredGuarantee.InsuranceCompany.Description : "");
            //$("#Mortage-Currency").val(data.InsuredGuarantee.Currency.Id);
            $("#Mortage-Currency").UifSelect("setSelected", data.InsuredGuarantee.Currency.Id);
            $("#Mortage-DeedNumber").val(data.InsuredGuarantee.RegistrationNumber);
            $("#Mortage-InsuredValue").val(FormatMoney(data.InsuredGuarantee.AppraisalAmount));
            $("#Mortage-MeasureArea").val(data.InsuredGuarantee.MeasureArea);
            $("#Mortage-NumberPolicy").val(data.InsuredGuarantee.PolicyNumber);
            $("#Mortage-Observations").val(data.InsuredGuarantee.Description);
            $("#Mortage-UnitOfMeasure").val(data.InsuredGuarantee.MeasurementType == null ? '' : data.InsuredGuarantee.MeasurementType.Id);
            $("#Mortage-ValuationDate").val(FormatDate(data.InsuredGuarantee.AppraisalDate));
            $("#Mortage-ValuationValue").val(FormatMoney(data.InsuredGuarantee.InsuranceAmount)); 
            $("#Mortage-AssetType").val(data.InsuredGuarantee.AssetTypeCode);
            //$("#Mortage-ValuationValue").val(FormatMoney(data.InsuredGuarantee.AppraisalAmount));
        },

        clearMortage: function () {
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
            $("#Mortage-UnitOfMeasure").UifSelect({ selectedId: 0 });
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
        },

        SaveMortage: function () {

            if (Guarantee.ValidateGuarantee()) {
                $("#Guarantee").validate();
                $("#AddMortage").validate();

                var guaranteeValid = $("#Guarantee").valid();
                var promissoryNoteValid = $("#AddMortage").valid();

                if (guaranteeValid && promissoryNoteValid) {
                    objMortage.pushMortage(guaranteeId);
                    return true;
                }
            }
            return false;
                
        },

        pushMortage: function (id) {

            var result = Guarantee.existGuarantee(id);
            var dataResult;

            if (result >= 0) {
                guarantee[result].Apostille = false;
                guarantee[result].InsuredGuarantee.AssetTypeCode = $("#Mortage-AssetType").val();
                guarantee[result].InsuredGuarantee.Apostille = false;
                guarantee[result].InsuredGuarantee.Branch.Id = $("#selectBranchGuarantee").val();
                guarantee[result].InsuredGuarantee.GuaranteeStatus = [];
                guarantee[result].InsuredGuarantee.GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                guarantee[result].InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;

                guarantee[result].InsuredGuarantee.Country.Id = $("#inputCountry-Mortage").data("Id");
                guarantee[result].InsuredGuarantee.State.Id = $("#inputState-Mortage").data("Id");
                guarantee[result].InsuredGuarantee.City.Id = $("#inputCity-Mortage").data("Id");

                
                guarantee[result].InsuredGuarantee.Address = $("#Mortage-Address").val();
                guarantee[result].InsuredGuarantee.AppraisalAmount = NotFormatMoney($("#Mortage-InsuredValue").val());

                guarantee[result].InsuredGuarantee.AppraisalDate = $("#Mortage-ValuationDate").val();
                guarantee[result].InsuredGuarantee.BuiltArea = $("#Mortage-BuiltArea").val();
                guarantee[result].InsuredGuarantee.Description = $("#Mortage-Observations").val();
                guarantee[result].InsuredGuarantee.ExpertName = $("#Mortage-AdjusterNames").val();

                guarantee[result].InsuredGuarantee.MeasureArea = $("#Mortage-MeasureArea").val();
                var MeasurementType = {};
                MeasurementType.Code = $("#Mortage-UnitOfMeasure").UifSelect("getSelected");
                guarantee[result].InsuredGuarantee.MeasurementType = MeasurementType;
                guarantee[result].InsuredGuarantee.PolicyNumber = $("#Mortage-NumberPolicy").val();
                guarantee[result].InsuredGuarantee.RegistrationNumber = $("#Mortage-DeedNumber").val();
                guarantee[result].InsuredGuarantee.InsuranceAmount = NotFormatMoney($("#Mortage-ValuationValue").val());
                guarantee[result].InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status = GuaranteeStatus;

                dataResult = guarantee[result];
            }
            else {
                guaranteeTmp.InsuredGuarantee.Apostille = false;
                guaranteeTmp.Code = GuaranteeType.Mortage;
                guaranteeTmp.Description = "HIPOTECA";
                guaranteeTmp.Apostille = false;

                var guaranteeType = {};
                guaranteeType.Code = GuaranteeType.Mortage;
                guaranteeType.Description = "HIPOTECA";

                guaranteeTmp.GuaranteeType = guaranteeType;

                var Branch = {};
                Branch.Id = $("#selectBranchGuarantee").val();
                Branch.Description = $("#selectBranchGuarantee option[value='" + Branch.Id + "']").text()

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text()

                var City = {};
                City.Id = $("#inputCity-Mortage").data("Id");
                City.Description = $("#inputCity-Mortage").data("Description");

                var State = {};
                State.Id = $("#inputState-Mortage").data("Id");
                State.Description = $("#inputState-Mortage").data("Description");

                var Country = {};
                Country.Id = $("#inputCountry-Mortage").data("Id");
                Country.Description = $("#inputCountry-Mortage").data("Description");
                               
                var Currency = {};
                Currency.Id = $("#Mortage-Currency").val();
                Currency.Description = $("#Mortage-Currency option[value='" + Currency.Id + "']").text();

                var MeasurementType = {};
                MeasurementType.Code = $("#Mortage-UnitOfMeasure").UifSelect("getSelected");
                MeasurementType.Description = $("#Mortage-UnitOfMeasure option[value='" + MeasurementType.Code + "']").text();

                //guaranteeTmp.InsuredGuarantee.Id = CommGuarantee.Mortage;
                guaranteeTmp.InsuredGuarantee.IndividualId = individualId;
                guaranteeTmp.InsuredGuarantee.Branch = Branch;

                guaranteeTmp.InsuredGuarantee.Currency = Currency;
                guaranteeTmp.InsuredGuarantee.Status = GuaranteeStatus;
                guaranteeTmp.InsuredGuarantee.City = City;
                guaranteeTmp.InsuredGuarantee.City.State = State;
                guaranteeTmp.InsuredGuarantee.City.State.Country = Country;
                
                
                guaranteeTmp.InsuredGuarantee.Code = guaranteeCode;
                guaranteeTmp.InsuredGuarantee.AssetTypeCode = $("#Mortage-AssetType").val();
                guaranteeTmp.InsuredGuarantee.Address = $("#Mortage-Address").val();
                guaranteeTmp.InsuredGuarantee.AppraisalAmount = NotFormatMoney($("#Mortage-InsuredValue").val());
                guaranteeTmp.InsuredGuarantee.AppraisalDate = $("#Mortage-ValuationDate").val();
                guaranteeTmp.InsuredGuarantee.BuiltArea = $("#Mortage-BuiltArea").val();
                guaranteeTmp.InsuredGuarantee.Description = $("#Mortage-Observations").val();
                guaranteeTmp.InsuredGuarantee.ExpertName = $("#Mortage-AdjusterNames").val();
                guaranteeTmp.InsuredGuarantee.MeasureArea = $("#Mortage-MeasureArea").val();
                guaranteeTmp.InsuredGuarantee.MeasurementType = MeasurementType;
                guaranteeTmp.InsuredGuarantee.PolicyNumber = $("#Mortage-NumberPolicy").val();
                guaranteeTmp.InsuredGuarantee.RegistrationNumber = $("#Mortage-DeedNumber").val();
                guaranteeTmp.InsuredGuarantee.InsuranceAmount = NotFormatMoney($("#Mortage-ValuationValue").val());
                guaranteeTmp.InsuredGuarantee.LastChangeDate = FormatDate(new Date());
                guaranteeTmp.InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked")  ? 1 : 0;

                dataResult = guaranteeTmp;
            }
            Guarantee.saveGuarantee(dataResult, GuaranteeType.Mortage);
        },

        GetParameterByDescription: function (description) {
            var dataResult = '';
            GuaranteeRequest.GetParameterByDescription(description).done(function (data) {
                if (data.success) {
                    dataResult = data.result;
                }
            });
            return dataResult;
        },

        GetMortageCountriesByDescription: function (Description) {
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
        },
        // State
        GetMortageStatesByCountryIdByDescription: function (CountryId, Description) {
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
        },
        // City
        GetMortageCitiesByCountryIdByStateIdByDescription: function (CountryId, StateId, Description) {
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
