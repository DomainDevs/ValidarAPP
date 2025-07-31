
var objPledge =
    {
        bindEvents: function () {
            window.TypeGuarantee = GuaranteeType.Pledge;
            $("#Pledge-Currency").UifSelect("setSelected", "0" );

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
                    objPledge.GetPledgeStatesByCountryIdByDescription($("#inputCountry-Pledge").data('Id'), $("#inputState-Pledge").val());
                }
            });
            $('#inputCity-Pledge').on("search", function (event, value) {
                $('#inputDaneCodePn').val("");
                if ($("#inputCity-Pledge").val().trim().length > 0) {
                    objPledge.GetPledgeCitiesByCountryIdByStateIdByDescription($("#inputCountry-Pledge").data('Id'), $("#inputState-Pledge").data('Id'), $("#inputCity-Pledge").val());
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
        },

        //Seccion Funciones
        showPledge: function (data) {
            Guarantee.SetCountrieStateCityGuarantee("Pledge", data.InsuredGuarantee.Country, data.InsuredGuarantee.State, data.InsuredGuarantee.City);
            $("#Pledge-Company").val((data.InsuredGuarantee.InsuranceCompany != null) ? data.InsuredGuarantee.InsuranceCompany.Description : "");
            $("#Pledge-Chassis").val(data.InsuredGuarantee.ChassisNro);
            //$("#Pledge-Currency").val(data.InsuredGuarantee.Currency.Id);
            $("#Pledge-Currency").UifSelect("setSelected", data.InsuredGuarantee.Currency.Id);
            $("#Pledge-Engine").val(data.InsuredGuarantee.EngineNro);
            //$("#Pledge-InsuredValue").val(FormatMoney(data.InsuredGuarantee.InsuranceAmount));
            $("#Pledge-InsuredValue").val(FormatMoney(data.InsuredGuarantee.InsuranceAmount));
            $("#Pledge-NumberPolicy").val(data.InsuredGuarantee.PolicyNumber);
            $("#Pledge-Observations").val(data.InsuredGuarantee.Description);
            $("#Pledge-Plate").val(data.InsuredGuarantee.LicensePlate);
            $("#Pledge-ValuationDate").val(FormatDate(data.InsuredGuarantee.AppraisalDate));
            $("#Pledge-ValuationValue").val(FormatMoney(data.InsuredGuarantee.AppraisalAmount));
            //$("#Pledge-ValuationValue").val(FormatMoney(data.InsuredGuarantee.AppraisalAmount));
        },

        clearPledge: function () {
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
            $("#Pledge-Currency").UifSelect("setSelected", "0");


            $("#inputCountry-Pledge").data({ Id: null, Description: null });
            $("#inputCountry-Pledge").val("");
            $("#inputState-Pledge").data({ Id: null, Description: null });
            $("#inputState-Pledge").val("");
            $("#inputCity-Pledge").data({ Id: null, Description: null });
            $("#inputCity-Pledge").val("");
            Guarantee.GetdefaultValueCountryGuarantee();
        },

        SavePledge: function () {
            if (Guarantee.ValidateGuarantee()) {
                $("#Guarantee").validate();
                $("#AddPledge").validate();

                var guaranteeValid = $("#Guarantee").valid();
                var promissoryNoteValid = $("#AddPledge").valid();

                if (guaranteeValid && promissoryNoteValid) {
                    objPledge.pushPledge(guaranteeId);
                    return true;
                }
            }
            return false;
        },

        pushPledge: function (id) {

            var result = Guarantee.existGuarantee(id);
            var dataResult;

            if (result >= 0) {
                guarantee[result].Apostille = false;
                guarantee[result].InsuredGuarantee.Branch.Id = $("#selectBranchGuarantee").val();
                guarantee[result].InsuredGuarantee.Branch.Description = $("#selectBranchGuarantee option[value='" + guarantee[result].InsuredGuarantee.Branch.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status.Id = $("#selectStatusGuarantee").val();
                guarantee[result].InsuredGuarantee.Status.Description = $("#selectStatusGuarantee  option[value='" + guarantee[result].InsuredGuarantee.Status.Id + "']").text();
                guarantee[result].InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;

                guarantee[result].InsuredGuarantee.Country.Id = $("#inputCountry-Pledge").data("Id");
                guarantee[result].InsuredGuarantee.Country.Description = $("#inputCountry-Pledge").data("Description");
                guarantee[result].InsuredGuarantee.State.Id = $("#inputState-Pledge").data("Id");
                guarantee[result].InsuredGuarantee.State.Description = $("#inputState-Pledge").data("Description");
                guarantee[result].InsuredGuarantee.City.Id = $("#inputCity-Pledge").data("Id");
                guarantee[result].InsuredGuarantee.City.Description = $("#inputCity-Pledge").data("Description");


                if ($("#Pledge-Company").val() != null) {
                    if (guarantee[result].InsuredGuarantee.InsuranceCompany == null) {
                        var InsuranceCompany = {};
                        InsuranceCompany.Description = $("#Pledge-Company").val();
                    }
                    else {
                        guarantee[result].InsuredGuarantee.InsuranceCompany.Description = $("#Pledge-Company").val();
                    }
                }

                guarantee[result].InsuredGuarantee.AppraisalAmount = NotFormatMoney($("#Pledge-ValuationValue").val());
                guarantee[result].InsuredGuarantee.AppraisalDate = $("#Pledge-ValuationDate").val();
                guarantee[result].InsuredGuarantee.ChassisNro = $("#Pledge-Chassis").val();
                guarantee[result].InsuredGuarantee.Currency.Id = $("#Pledge-Currency").val();
                guarantee[result].InsuredGuarantee.Description = $("#Pledge-Observations").val();
                guarantee[result].InsuredGuarantee.EngineNro = $("#Pledge-Engine").val();
                guarantee[result].InsuredGuarantee.LicensePlate = $("#Pledge-Plate").val();
                guarantee[result].InsuredGuarantee.PolicyNumber = $("#Pledge-NumberPolicy").val();
                guarantee[result].InsuredGuarantee.InsuranceAmount = NotFormatMoney($("#Pledge-InsuredValue").val());
                guarantee[result].InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status = GuaranteeStatus;

                dataResult = guarantee[result];
            }
            else {
                guaranteeTmp.Code = GuaranteeType.Pledge;
                guaranteeTmp.Description = "PRENDA";
                guaranteeTmp.Apostille = false;

                var guaranteeType = {};
                guaranteeType.Code = GuaranteeType.Pledge;
                guaranteeType.Description = "PRENDA";

                guaranteeTmp.GuaranteeType = guaranteeType;

                var Branch = {};
                Branch.Id = $("#selectBranchGuarantee").val();

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text()

                var City = {};
                City.Id = $("#inputCity-Pledge").data("Id");
                City.Description = $("#inputCity-Pledge").data("Description");

                var State = {};
                State.Id = $("#inputState-Pledge").data("Id");
                State.Description = $("#inputState-Pledge").data("Description");


                var Country = {};
                Country.Id = $("#inputCountry-Pledge").data("Id");
                Country.Description = $("#inputCountry-Pledge").data("Description");

               
                var Currency = {};
                Currency.Id = $("#Pledge-Currency").val();
                Currency.Description = $("#Pledge-Currency option[value='" + Currency.Id + "']").text();

                var InsuranceCompany = {};
                InsuranceCompany.Description = $("#Pledge-InsuranceCompany").val();

                var MeasurementType = {};
                MeasurementType.Code = $("#Pledge-UnitOfMeasure").val();
                MeasurementType.Description = $("#Pledge-UnitOfMeasure option[value='" + MeasurementType.Description + "']").text();

                guaranteeTmp.InsuredGuarantee.Code = guaranteeCode;
                guaranteeTmp.InsuredGuarantee.IndividualId = individualId;
                guaranteeTmp.InsuredGuarantee.Branch = Branch;
                guaranteeTmp.InsuredGuarantee.Currency = Currency;
                guaranteeTmp.InsuredGuarantee.Status = GuaranteeStatus;
                guaranteeTmp.InsuredGuarantee.City = City;
                guaranteeTmp.InsuredGuarantee.City.State = State;
                guaranteeTmp.InsuredGuarantee.City.State.Country = Country;
                

                //    guaranteeTmp.InsuredGuarantee.InsuranceCompany.Description = InsuranceCompany;
                guaranteeTmp.InsuredGuarantee.AppraisalAmount = NotFormatMoney($("#Pledge-ValuationValue").val());
                guaranteeTmp.InsuredGuarantee.AppraisalDate = $("#Pledge-ValuationDate").val();
                guaranteeTmp.InsuredGuarantee.ChassisNro = $("#Pledge-Chassis").val();
                guaranteeTmp.InsuredGuarantee.Currency.Id = $("#Pledge-Currency").val();
                guaranteeTmp.InsuredGuarantee.Description = $("#Pledge-Observations").val();
                guaranteeTmp.InsuredGuarantee.EngineNro = $("#Pledge-Engine").val();
                guaranteeTmp.InsuredGuarantee.LicensePlate = $("#Pledge-Plate").val();
                guaranteeTmp.InsuredGuarantee.PolicyNumber = $("#Pledge-NumberPolicy").val();
                guaranteeTmp.InsuredGuarantee.InsuranceAmount = NotFormatMoney($("#Pledge-InsuredValue").val());
                guaranteeTmp.InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;
                guaranteeTmp.InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                dataResult = guaranteeTmp;
            }
            Guarantee.saveGuarantee(dataResult, GuaranteeType.Pledge);
        },

        GetPledgeCountriesByDescription: function (Description) {
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
        },
        // State
        GetPledgeStatesByCountryIdByDescription: function (CountryId, Description) {
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
        },
        // City
        GetPledgeCitiesByCountryIdByStateIdByDescription: function (CountryId, StateId, Description) {
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

