
var objActions =
    {
    bindEvents: function () {
        $("#Actions-Currency").on("binded", function () {
            Guarantee.GetdefaultValueCurrencyGuarantee();
        });
        objActions.bindEventsActions();
    },
    bindEventsActions: function () {
        
        window.TypeGuarantee = GuaranteeType.Actions;

          if (guaranteeCode == 15 || guaranteeTypeCode == 15)
            $("#IdNumberPolicy").show(); 

            $("#Actions-Currency").UifSelect("setSelected", "0" );

            // << EESGE-172 Control De Busqueda
            $("#inputCountry-Actions").keyup(function () {
                $("#inputCountry-Actions").val($("#inputCountry-Actions").val().toUpperCase());
            });

            $("#inputState-Actions").keyup(function () {
                $("#inputState-Actions").val($("#inputState-Actions").val().toUpperCase());
            });

            $("#inputCity-Actions").keyup(function () {
                $("#inputCity-Actions").val($("#inputCity-Actions").val().toUpperCase());
            });

        $('#inputCountry-Actions').on("search", function (event, value)
        {
            $('#inputState-Actions').val("");
            $('#inputCity-Actions').val("");
            $('#inputDaneCodePn').val("");

            if ($("#inputCountry-Actions").val().trim().length > 0) {
                objActions.GetActionsCountriesByDescription($("#inputCountry-Actions").val());
                }
        });

            $('#inputState-Actions').on("search", function (event, value) {
                $('#inputCity-Actions').val("");
                $('#inputDaneCodePn').val("");

                    if ($("#inputState-Actions").val().trim().length > 0) {
                    objActions.GetActionsStatesByCountryIdByDescription($("#inputCountry-Actions").data('Id'), $("#inputState-Actions").val());
                }
            });


            $('#inputCity-Actions').on("search", function (event, value) {
                $('#inputDaneCodePn').val("");
                if ($("#inputCity-Actions").val().trim().length > 0) {
                   objActions.GetActionsCitiesByCountryIdByStateIdByDescription($("#inputCountry-Actions").data('Id'), $("#inputState-Actions").data('Id'), $("#inputCity-Actions").val());
                }
            });

            $('#tblResultListCountriesActions tbody').on('click', 'tr', function (e) {
                var dataCountry = {
                    Id: $(this).children()[0].innerHTML,
                    Description: $(this).children()[1].innerHTML
                }
                    $("#inputCountry-Actions").data(dataCountry);
                    $("#inputCountry-Actions").val(dataCountry.Description);
                $('#modalListSearchCountriesActions').UifModal('hide');
        });

            $('#tblResultListStatesActions tbody').on('click', 'tr', function (e) {
                var dataState = {
                    Id: $(this).children()[0].innerHTML,
                    Description: $(this).children()[1].innerHTML
                }

                $("#inputState-Actions").data(dataState);
                $("#inputState-Actions").val(dataState.Description);
                $('#modalListSearchStatesActions').UifModal('hide');
        });

            $('#tblResultListCitiesActions tbody').on('click', 'tr', function (e) {
                var dataCities = {
                    Id: $(this).children()[0].innerHTML,
                    Description: $(this).children()[1].innerHTML
                }

                $("#inputCity-Actions").data(dataCities);
                    $("#inputCity-Actions").val(dataCities.Description);
                $('#modalListSearchCitiesActions').UifModal('hide');
            });

            //$('#Pledge-ValuationDate').on("datepicker.change", function (event, date) {

            //    var date = $('#Pledge-ValuationDate').val().split("/");
            //    if (isExpirationDate(date)) {
            //        $('#Pledge-ValuationDate').val("");
            //        $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateValuatGreaterCurrent + "<br>" })
            //    }
            //})
        },

        //Seccion Funciones
        showActions: function (data) {
            Guarantee.SetCountrieStateCityGuarantee("Actions", data.InsuredGuarantee.Country, data.InsuredGuarantee.State, data.InsuredGuarantee.City);

            //$("#Pledge-Company").val((data.InsuredGuarantee.InsuranceCompany != null) ? data.InsuredGuarantee.InsuranceCompany.Description : "");
            //$("#Pledge-Chassis").val(data.InsuredGuarantee.ChassisNro);
            
            
            //$("#Pledge-Engine").val(data.InsuredGuarantee.EngineNro);
            
            //$("#Pledge-InsuredValue").val(FormatMoney(data.InsuredGuarantee.InsuranceAmount));
            $("#Actions-NumberPolicy").val(data.InsuredGuarantee.PolicyNumber);
            
            $("#Actions-NominalValue").val(data.InsuredGuarantee.DocumentValueAmount);
            $("#Actions-Observations").val(data.InsuredGuarantee.Description);
            $("#Actions-Currency").UifSelect("setSelected", data.InsuredGuarantee.Currency.Id);
            $("#Actions-NumberPolicy").val(data.InsuredGuarantee.PolicyNumber);
            
        },

        clearActions: function () {
            $("#AddActions").get(0).reset();
            //$("#Pledge-Plate").val("");
            //$("#Pledge-Engine").val("");
            //$("#Pledge-Chassis").val("");
            //$("#Pledge-ValuationDate").val("");
            //$("#Pledge-Company").val("");
            //$("#Pledge-NumberPolicy").val("");
            //$("#Pledge-InsuredValue").val("");
            $("#Actions-NominalValue").val("");
            $("#Actions-Observations").val("");
            $("#Actions-Currency").UifSelect("setSelected", "0");
            $("#Actions-NumberPolicy").val("");
            
            

            


            $("#inputCountry-Actions").data({ Id: null, Description: null });
            $("#inputCountry-Actions").val("");
            $("#inputState-Actions").data({ Id: null, Description: null });
            $("#inputState-Actions").val("");
            $("#inputCity-Actions").data({ Id: null, Description: null });
            $("#inputCity-Actions").val("");
            Guarantee.GetdefaultValueCountryGuarantee();
        },

        SaveActions: function () {
            if (Guarantee.ValidateGuarantee()) {
                $("#Guarantee").validate();
                $("#AddActions").validate();

                var guaranteeValid = $("#Guarantee").valid();
                var promissoryNoteValid = $("#AddActions").valid();

                if (guaranteeValid && promissoryNoteValid) {
                    objActions.pushActions(guaranteeId);
                    return true;
                }
            }
            return false;
        },

        pushActions: function (id) {

            var result = Guarantee.existGuarantee(id);
            var dataResult;

            if (result >= 0) {
                guarantee[result].Apostille = false;
                guarantee[result].InsuredGuarantee.Branch.Id = $("#selectBranchGuarantee").val();
                guarantee[result].InsuredGuarantee.Branch.Description = $("#selectBranchGuarantee option[value='" + guarantee[result].InsuredGuarantee.Branch.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status.Id = $("#selectStatusGuarantee").val();
                guarantee[result].InsuredGuarantee.Status.Description = $("#selectStatusGuarantee  option[value='" + guarantee[result].InsuredGuarantee.Status.Id + "']").text();
                guarantee[result].InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;

                guarantee[result].InsuredGuarantee.Country.Id = $("#inputCountry-Actions").data("Id");
                guarantee[result].InsuredGuarantee.Country.Description = $("#inputCountry-Actions").data("Description");
                guarantee[result].InsuredGuarantee.State.Id = $("#inputState-Actions").data("Id");
                guarantee[result].InsuredGuarantee.State.Description = $("#inputState-Actions").data("Description");
                guarantee[result].InsuredGuarantee.City.Id = $("#inputCity-Actions").data("Id");
                guarantee[result].InsuredGuarantee.City.Description = $("#inputCity-Actions").data("Description");


                //if ($("#Pledge-Company").val() != null) {
                //    if (guarantee[result].InsuredGuarantee.InsuranceCompany == null) {
                //        var InsuranceCompany = {};
                //        InsuranceCompany.Description = $("#Pledge-Company").val();
                //    }
                //    else {
                //        guarantee[result].InsuredGuarantee.InsuranceCompany.Description = $("#Pledge-Company").val();
                //    }
                //}

                //guarantee[result].InsuredGuarantee.AppraisalAmount = NotFormatMoney($("#Pledge-ValuationValue").val());
                //guarantee[result].InsuredGuarantee.AppraisalDate = $("#Pledge-ValuationDate").val();
                //guarantee[result].InsuredGuarantee.ChassisNro = $("#Pledge-Chassis").val();
                //guarantee[result].InsuredGuarantee.Currency.Id = $("#Pledge-Currency").val();

                guarantee[result].InsuredGuarantee.Description = $("#Actions-Observations").val();
                guarantee[result].InsuredGuarantee.DocumentValueAmount = NotFormatMoney($("#Actions-NominalValue").val());
                guarantee[result].InsuredGuarantee.Currency.Id = $("#Actions-Currency").val();

                guarantee[result].InsuredGuarantee.PolicyNumber = $("#Actions-NumberPolicy").val();
                
                

                //guarantee[result].InsuredGuarantee.LicensePlate = $("#Pledge-Plate").val();
                //guarantee[result].InsuredGuarantee.PolicyNumber = $("#Pledge-NumberPolicy").val();
                //guarantee[result].InsuredGuarantee.InsuranceAmount = NotFormatMoney($("#Pledge-InsuredValue").val());
                //guarantee[result].InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status = GuaranteeStatus;

                dataResult = guarantee[result];
            }
            else {
                guaranteeTmp.Code = GuaranteeType.Actions;
                guaranteeTmp.Description = "PRENDA";
                guaranteeTmp.Apostille = false;

                var guaranteeType = {};
                guaranteeType.Code = GuaranteeType.Actions;
                guaranteeType.Description = "PRENDA";

                guaranteeTmp.GuaranteeType = guaranteeType;

                var Branch = {};
                Branch.Id = $("#selectBranchGuarantee").val();

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text()

                var City = {};
                City.Id = $("#inputCity-Actions").data("Id");
                City.Description = $("#inputCity-Actions").data("Description");

                var State = {};
                State.Id = $("#inputState-Actions").data("Id");
                State.Description = $("#inputState-Actions").data("Description");


                var Country = {};
                Country.Id = $("#inputCountry-Actions").data("Id");
                Country.Description = $("#inputCountry-Actions").data("Description");

               
                var Currency = {};
                Currency.Id = $("#Actions-Currency").val();
                Currency.Description = $("#Actions-Currency option[value='" + Currency.Id + "']").text();

                //var InsuranceCompany = {};
                //InsuranceCompany.Description = $("#Pledge-InsuranceCompany").val();

                //var MeasurementType = {};
                //MeasurementType.Code = $("#Pledge-UnitOfMeasure").val();
                //MeasurementType.Description = $("#Pledge-UnitOfMeasure option[value='" + MeasurementType.Description + "']").text();

                guaranteeTmp.InsuredGuarantee.Code = guaranteeCode;
                guaranteeTmp.InsuredGuarantee.IndividualId = individualId;
                guaranteeTmp.InsuredGuarantee.Branch = Branch;
                guaranteeTmp.InsuredGuarantee.Currency = Currency;
                guaranteeTmp.InsuredGuarantee.Status = GuaranteeStatus;
                guaranteeTmp.InsuredGuarantee.City = City;
                guaranteeTmp.InsuredGuarantee.City.State = State;
                guaranteeTmp.InsuredGuarantee.City.State.Country = Country;
                

                guaranteeTmp.InsuredGuarantee.Description = $("#Actions-Observations").val();
                guaranteeTmp.InsuredGuarantee.DocumentValueAmount = NotFormatMoney($("#Actions-NominalValue").val());
                guaranteeTmp.InsuredGuarantee.Currency.Id = $("#Actions-Currency").val();

                guaranteeTmp.InsuredGuarantee.PolicyNumber = $("#Actions-NumberPolicy").val();
                guaranteeTmp.InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;


                //    guaranteeTmp.InsuredGuarantee.InsuranceCompany.Description = InsuranceCompany;
                //guaranteeTmp.InsuredGuarantee.AppraisalAmount = NotFormatMoney($("#Pledge-ValuationValue").val());
                //guaranteeTmp.InsuredGuarantee.AppraisalDate = $("#Pledge-ValuationDate").val();
                //guaranteeTmp.InsuredGuarantee.ChassisNro = $("#Pledge-Chassis").val();
                //guaranteeTmp.InsuredGuarantee.Currency.Id = $("#Pledge-Currency").val();
                //guaranteeTmp.InsuredGuarantee.Description = $("#Pledge-Observations").val();
                //guaranteeTmp.InsuredGuarantee.EngineNro = $("#Pledge-Engine").val();
                //guaranteeTmp.InsuredGuarantee.LicensePlate = $("#Pledge-Plate").val();
                //guaranteeTmp.InsuredGuarantee.PolicyNumber = $("#Pledge-NumberPolicy").val();
                //guaranteeTmp.InsuredGuarantee.InsuranceAmount = NotFormatMoney($("#Pledge-InsuredValue").val());
                //guaranteeTmp.InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                dataResult = guaranteeTmp;
            }
            Guarantee.saveGuarantee(dataResult, GuaranteeType.Actions);
        },

        GetActionsCountriesByDescription: function (Description) {
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
                            $('#tblResultListCountriesActions').UifDataTable('clear');
                            $("#tblResultListCountriesActions").UifDataTable('addRow', dataCountries);
                            $('#modalListSearchCountriesActions').UifModal('showLocal', AppResources.ModalTitleCountries);
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
        GetActionsStatesByCountryIdByDescription: function (CountryId, Description) {
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

                            $('#tblResultListStatesActions').UifDataTable('clear');
                            $("#tblResultListStatesActions").UifDataTable('addRow', dataStates);
                            $('#modalListSearchStatesActions').UifModal('showLocal', AppResources.ModalTitleStates);
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
        GetActionsCitiesByCountryIdByStateIdByDescription: function (CountryId, StateId, Description) {
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
                            $('#tblResultListCitiesActions').UifDataTable('clear');
                            $("#tblResultListCitiesActions").UifDataTable('addRow', dataCities);
                            $('#modalListSearchCitiesActions').UifModal('showLocal', AppResources.ModalTitleCities);
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

