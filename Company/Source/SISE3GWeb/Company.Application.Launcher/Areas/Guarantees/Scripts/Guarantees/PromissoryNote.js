var objPromissoryNote =
    {
        bindEvents: function () {
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
                    objPromissoryNote.GetPromissoryNoteCountriesByDescription($("#inputCountry-PromissoryNote").val());
                }
            });
            $('#inputState-PromissoryNote').on("search", function (event, value) {
                $('#inputCity-PromissoryNote').val("");
                $('#inputDaneCodePn').val("");
                if ($("#inputState-PromissoryNote").val().trim().length > 0) {
                    objPromissoryNote.GetPromissoryNoteStatesByCountryIdByDescription($("#inputCountry-PromissoryNote").data('Id'), $("#inputState-PromissoryNote").val());
                }
            });
            $('#inputCity-PromissoryNote').on("search", function (event, value) {
                $('#inputDaneCodePn').val("");
                if ($("#inputCity-PromissoryNote").val().trim().length > 0) {
                    objPromissoryNote.GetPromissoryNoteCitiesByCountryIdByStateIdByDescription($("#inputCountry-PromissoryNote").data('Id'), $("#inputState-PromissoryNote").data('Id'), $("#inputCity-PromissoryNote").val());
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
                if (objPromissoryNote.validateDate(constitution, expiration)) {
                    $('#PromissoryNote-ExpirationDate').val("");
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterDateConst + "<br>" })
                }
            });
            $('#PromissoryNote-ConstitutionDate').on("change", function (event, date) {
                var constitution = $('#PromissoryNote-ConstitutionDate').val().split("/");
                var expiration = $('#PromissoryNote-ExpirationDate').val().split("/");
                if (objPromissoryNote.validateDate(constitution, expiration)) {
                    $('#PromissoryNote-ExpirationDate').val("");
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterDateConst + "<br>" })
                }
            });
        },

        validateDate: function (Date1, Date2) {
            var date1 = Date1[1] + "/" + Date1[0] + "/" + Date1[2]
            var date2 = Date2[1] + "/" + Date2[0] + "/" + Date2[2]
            if ((Date.parse(date1)) > (Date.parse(date2))) {
                return true;
            }
            return false;
        },

        showPromissoryNote: function (data) {
            Guarantee.SetCountrieStateCityGuarantee("PromissoryNote", data.InsuredGuarantee.Country, data.InsuredGuarantee.State, data.InsuredGuarantee.City);
            $("#PromissoryNote-ConstitutionDate").UifDatepicker('setValue', data.InsuredGuarantee.RegistrationDate);
            //$("#PromissoryNote-Currency").val(data.InsuredGuarantee.Currency.Id);
            $("#PromissoryNote-Currency").UifSelect("setSelected", data.InsuredGuarantee.Currency.Id);
            $("#PromissoryNote-DocumentNumber").val(data.InsuredGuarantee.DocumentNumber);
            $("#PromissoryNote-ExpirationDate").UifDatepicker('setValue', data.InsuredGuarantee.ExpirationDate);
            $("#PromissoryNote-NominalValue").val(FormatMoney(data.InsuredGuarantee.DocumentValueAmount));
            //$("#PromissoryNote-NominalValue").val(FormatMoney(data.InsuredGuarantee.DocumentValueAmount));
            $("#PromissoryNote-Observations").val(data.InsuredGuarantee.Description);
            if (data.InsuredGuarantee.PromissoryNoteType != null)
            $("#PromissoryNote-PromissoryNoteType").UifSelect("setSelected",data.InsuredGuarantee.PromissoryNoteType.Id);
            //$("#PromissoryNote-PromissoryNoteType").val(data.InsuredGuarantee.PromissoryNoteType.Id);
            $("#PromissoryNote-SignatoriesNumber").val(data.InsuredGuarantee.SignatoriesNumber);
        },

        clearPromissoryNote: function () {
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
            //$("#PromissoryNote-PromissoryNoteType").val("");
            $("#PromissoryNote-PromissoryNoteType").UifSelect("setSelected", null);
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
        },

        SavePromissoryNote: function () {
            if (Guarantee.ValidateGuarantee()) {
                //Se agrega para agregar automaticamente el afianzado en persona natural
                if ($("#tableGuarantors").UifListView("getData").length != 0 && searchType == 1) {
                    Guarantee.LoadListGuarantors(guaranteeId);
                    $("#btnRecordGuarantors").trigger("click");
                }

                $("#Guarantee").validate();
                $("#AddPromissoryNote").validate();

                var guaranteeValid = $("#Guarantee").valid();
                var promissoryNoteValid = $("#AddPromissoryNote").valid();

                if (guaranteeValid && promissoryNoteValid) {
                    objPromissoryNote.pushPromissoryNote(guaranteeId);
                    return true;
                }
            }
            return false;
                
        },

        pushPromissoryNote: function (id) {

            var result = Guarantee.existGuarantee(id);
            var dataResult;

            if (result >= 0) {

                guarantee[result].InsuredGuarantee.Apostille = false;
                guarantee[result].Apostille = false;            
                guarantee[result].InsuredGuarantee.Branch.Id = $("#selectBranchGuarantee").val();
                guarantee[result].InsuredGuarantee.GuaranteeStatus = [];
                guarantee[result].InsuredGuarantee.GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                guarantee[result].InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;

                guarantee[result].InsuredGuarantee.Country.Id = $("#inputCountry-PromissoryNote").data("Id");
                guarantee[result].InsuredGuarantee.State.Id = $("#inputState-PromissoryNote").data("Id");
                guarantee[result].InsuredGuarantee.City.Id = $("#inputCity-PromissoryNote").data("Id");

                guarantee[result].InsuredGuarantee.RegistrationDate = $("#PromissoryNote-ConstitutionDate").val();
                guarantee[result].InsuredGuarantee.Currency.Id = $("#PromissoryNote-Currency").val();
                guarantee[result].InsuredGuarantee.DocumentNumber = $("#PromissoryNote-DocumentNumber").val();
                guarantee[result].InsuredGuarantee.ExpirationDate = $("#PromissoryNote-ExpirationDate").val();
                guarantee[result].InsuredGuarantee.DocumentValueAmount = NotFormatMoney($("#PromissoryNote-NominalValue").val());
                guarantee[result].InsuredGuarantee.Description = $("#PromissoryNote-Observations").val();
                guarantee[result].InsuredGuarantee.PromissoryNoteType = [];
                guarantee[result].InsuredGuarantee.PromissoryNoteType.Id = $("#PromissoryNote-PromissoryNoteType").val();
                guarantee[result].InsuredGuarantee.SignatoriesNumber = $("#PromissoryNote-SignatoriesNumber").val();

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();
                guarantee[result].InsuredGuarantee.Status = GuaranteeStatus;

                var PromissoryNoteType = {};
                PromissoryNoteType.Id = $("#PromissoryNote-PromissoryNoteType").val();
                guarantee[result].InsuredGuarantee.PromissoryNoteType = PromissoryNoteType;

                dataResult = guarantee[result];
            }
            else {
                guaranteeTmp.InsuredGuarantee.Apostille = false;
                guaranteeTmp.Code = GuaranteeType.PromissoryNote;
                guaranteeTmp.Description = "PAGARE";
                guaranteeTmp.Apostille = false;

                var Branch = {};
                Branch.Id = $("#selectBranchGuarantee").val();

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();
             
                var GuaranteeTypeTmp = {};
                GuaranteeTypeTmp.Id = $("#PromissoryNote-PromissoryNoteType").val();//GuaranteeType.PromissoryNote;
                GuaranteeTypeTmp.Description = $("#PromissoryNote-PromissoryNoteType option[value='" + GuaranteeTypeTmp.Id + "']").text();//"PAGARE"

                guaranteeTmp.GuaranteeType = GuaranteeTypeTmp;//GuaranteeStatus;
                
                var City = {};
                City.Id = parseInt($("#inputCity-PromissoryNote").data("Id"));
                 
                var State = {};
                State.Id = $("#inputState-PromissoryNote").data("Id");

                var Country = {};
                Country.Id = $("#inputCountry-PromissoryNote").data("Id");
                
                var Currency = {};
                Currency.Id = $("#PromissoryNote-Currency").val();

                var PromissoryNoteType = {};
                PromissoryNoteType.Id = $("#PromissoryNote-PromissoryNoteType").val();
                //PromissoryNoteType.Description = $("#PromissoryNote-PromissoryNoteType option[value='" + GuaranteeTypeTmp.Code + "']").text();
                
                guaranteeTmp.InsuredGuarantee.Code = guaranteeCode;
                guaranteeTmp.InsuredGuarantee.IndividualId = $('#ContractorId').val();// individualId;
                guaranteeTmp.InsuredGuarantee.Branch = Branch;
                guaranteeTmp.InsuredGuarantee.RegistrationDate = $("#PromissoryNote-ConstitutionDate").val();
                guaranteeTmp.InsuredGuarantee.Currency = Currency;
                guaranteeTmp.InsuredGuarantee.DocumentNumber = $("#PromissoryNote-DocumentNumber").val();
                guaranteeTmp.InsuredGuarantee.ExpirationDate = $("#PromissoryNote-ExpirationDate").val();
                guaranteeTmp.InsuredGuarantee.DocumentValueAmount = NotFormatMoney($("#PromissoryNote-NominalValue").val());
                guaranteeTmp.InsuredGuarantee.Description = $("#PromissoryNote-Observations").val();
                guaranteeTmp.InsuredGuarantee.PromissoryNoteType = PromissoryNoteType;
                guaranteeTmp.InsuredGuarantee.SignatoriesNumber = ($("#PromissoryNote-SignatoriesNumber").val() != 0 ? $("#PromissoryNote-SignatoriesNumber").val() : 0);
                guaranteeTmp.InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;
                guaranteeTmp.InsuredGuarantee.Status = GuaranteeStatus;
                guaranteeTmp.InsuredGuarantee.City = City;
                guaranteeTmp.InsuredGuarantee.City.State = State;
                guaranteeTmp.InsuredGuarantee.City.State.Country = Country;
                //try {
                //    guaranteeTmp.InsuredGuarantee.Guarantors[0].Adrress = " ";
                //    guaranteeTmp.InsuredGuarantee.Guarantors[0].CityText = " ";
                //} catch (err){
                //    var ex = err.message;
                //}
                
                dataResult = guaranteeTmp;
            }
            Guarantee.saveGuarantee(dataResult, GuaranteeType.PromissoryNote);
        },

        GetPromissoryNoteCountriesByDescription: function (Description) {
            if (Description.length >= 3) {
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Guarantees/Guarantees/GetCountriesByDescription',
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
        },
        // State
        GetPromissoryNoteStatesByCountryIdByDescription: function (CountryId, Description) {
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
        },
        // City
        GetPromissoryNoteCitiesByCountryIdByStateIdByDescription: function (CountryId, StateId, Description) {
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

