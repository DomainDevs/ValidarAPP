//principal
var fieldExist = false;
var LaboralPersonalInformation = [];

class StaffLabour extends Uif2.Page {
    getInitialState() {
        StaffLabour.unBindEvents();
        PersonRequest.GetPersonTypesInformationPersonal().done(function (data) {
            if (data.success) {
                $("#selectPersonIndividualType").UifSelect({ sourceData: data.result });
            }
        });

        EducativeLevelRequest.GetEducativeLevels().done(function (data) {
            if (data.success) {
                $("#selectLevelEducation").UifSelect({ sourceData: data.result });
            }
        });

        HouseTypeRequest.GetHouseTypes().done(function (data) {
            if (data.success) {
                $("#selectTypeHouse").UifSelect({ sourceData: data.result });
            }
        });

        SocialLayerRequest.GetSocialLayers().done(function (data) {
            if (data.success) {
                $("#selectStratum").UifSelect({ sourceData: data.result });
            }
        });

        OccupationRequest.GetOccupations().done(function (data) {
            if (data.success) {
                $("#selectOccupation").UifSelect({ sourceData: data.result });
                $("#selectOtherOccupation").UifSelect({ sourceData: data.result });
            }
        });

        SpecialtyRequest.GetSpecialties().done(function (data) {
            if (data.success) {
                $("#selectSpecialty").UifSelect({ sourceData: data.result });
            }
        });

        IncomeLevelRequest.GetIncomeLevels().done(function (data) {
            if (data.success) {
                $("#selectIncomeLevel").UifSelect({ sourceData: data.result });
            }
        });
        StaffLabour.CleanObject();
    }

    bindEvents() {
        
        $('#inputNchildren').ValidatorKey(ValidatorType.Number);
        $('#inputPhoneConpany').ValidatorKey(ValidatorType.Number);
        $("#btnAcceptStaffLabour").click(this.AcceptStaffLabour);
        $("#btnCancelStaffLabour").click(this.CancelStaffLabour);
        $('#inputCountryInfoPn').on("search", this.SearchContryInfoPn);
        $("#TableInterestGroups tbody").on("click", "tr", this.AddRemovePersonInterest);
        $("#TableInterestGroups thead").on("click", "tr", this.SelectUnselectAll);
        $("#TableInterestGroups_paginate").click(StaffLabour.CheckInterestVal);
        $('#tblResultListCountryInfPn tbody').on('click', 'tr', this.SelectSearchCountriesInfPn);

    }
    static unBindEvents() {

        $('#inputNchildren').unbind();
        $('#inputPhoneConpany').unbind();
        $("#btnAcceptStaffLabour").unbind();
        $("#btnCancelStaffLabour").unbind();
        $('#inputCountryInfoPn').unbind();
        $("#TableInterestGroups tbody").unbind();
        $("#TableInterestGroups thead").unbind();
        $("#TableInterestGroups_paginate").unbind();
        $('#tblResultListCountryInfPn tbody').unbind();

    }
    

    AcceptStaffLabour() {
        if ($("#selectOccupation").val() == "" || $("#selectOccupation").val() == null) {
            $("#selectOccupation").UifSelect("setSelected", 99);
        }
        StaffLabour.FillLaboralPersonalInformation();
        if (StaffLabour.ValidateStaffLabour()) {
            StaffLabour.SavePersonJob();
            Persons.AddSubtitlesRightBar();
        }
    }

    CancelStaffLabour() {
        if (individualId == Person.New || isNew == 1) {
            if (LaboralPersonalInformation == null) {
                StaffLabour.ClearStaffLabor();
                StaffLabour.CleanObject();
            }
        }
    }

    SelectSearchCountriesInfPn() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCountryInfoPn").data(dataCountry);
        $("#inputCountryInfoPn").val(dataCountry.Description);
        $('#modalListSearchContryInfoPn').UifModal('hide');
    }


    SearchContryInfoPn(event, value) {
        if ($("#inputCountryInfoPn").val().trim().length > 0) {
            StaffLabour.GetCountriesByDescriptionInfoPn($('#inputCountryInfoPn').val());
        }
    }

    static GetCountriesByDescriptionInfoPn(description) {
        if (description.length >= 3) {
            CountryRequest.GetCountriesByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataCountries = [];
                        $.each(data.result, function (index, value) {
                            dataCountries.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });
                        $('#tblResultListCountryInfPn').UifDataTable('clear');
                        $("#tblResultListCountryInfPn").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchContryInfoPn').UifModal('showLocal', AppResourcesPerson.ModalTitleCountries);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    static ClearStaffLabor() {
        $("#selectPersonIndividualType").val("");
        $("#inputSpouseName").val("");
        $("#inputNchildren").val("");
        $("#selectStratum").val("");
        $("#selectLevelEducation").val("");
        $("#selectTypeHouse").val("");
        $("#selectOccupation").val("");
        $("#selectSpecialty").val("");
        $("#selectOtherOccupation").val("");
        $("#selectIncomeLevel").val("");
        $("#inputNameCompany").val("");
        $("#inputSector").val("");
        $("#inputofifce").val("");
        $("#inputPhoneConpany").val("");
        $("#inputContactCompany").val("");
    }

    static ValidateStaffLabour() {
        var msj = "";

        if ($("#selectOccupation").val() == "" || $("#selectOccupation").val() == null) {
            msj = msj + "Ocupación";
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msj, 'autoclose': true })
            return false;
        }

        return true;
    }

    AddRemovePersonInterest() {
        var rowEdit = $('#TableInterestGroups').DataTable().row(this).data();
        var dataPersonInterestGroup = { IndividualId: $("#lblPersonCode").val(), InterestGroupTypeId: rowEdit.InterestGroupTypeId }
        if (!$(this).hasClass("row-selected")) {
            LaboralPersonalInformation.PersonInterestGroup.push(dataPersonInterestGroup);
        }
        else {
            var indexLabPerson = LaboralPersonalInformation.PersonInterestGroup.indexOf(dataPersonInterestGroup);
            LaboralPersonalInformation.PersonInterestGroup.splice(indexLabPerson, 1);
        }
    }

    SelectUnselectAll() {
        var data = $('#TableInterestGroups').DataTable().rows().data();
        if ($("#TableInterestGroups thead").find(".glyphicon-unchecked").length == 1) {
            $.each(data, function (key, value) {
                var dataPersonInterestGroup = { IndividualId: $("#lblPersonCode").val(), InterestGroupTypeId: this.InterestGroupTypeId }
                LaboralPersonalInformation.PersonInterestGroup.push(dataPersonInterestGroup);
            });
        }
        else {
            LaboralPersonalInformation.PersonInterestGroup = [];
        }
    }

    static FillLaboralPersonalInformation() {

        LaboralPersonalInformation = {
            PersonType: $('#selectPersonIndividualType').val(),
            IncomeLevel: $('#selectIncomeLevel').val(),
            IndividualId: $("#lblPersonCode").val(),
            EducativeLevel: $('#selectLevelEducation').val(),
            CompanyName: $('#inputNameCompany').val(),
            Children: $('#inputNchildren').val(),
            Occupation: $('#selectOccupation').val(),
            Position: $('#inputofifce').val(),
            OtherOccupation: $('#selectOtherOccupation').val(),
            CompanyPhone: $('#inputPhoneConpany').val(),
            Speciality: $('#selectSpecialty').val(),
            HouseType: $('#selectTypeHouse').val(),
            SocialLayer: $('#selectStratum').val(),
            SpouseName: $('#inputSpouseName').val(),
            PersonCode: $('#selectPersonIndividualType').val(),
            JobSector: $('#inputSector').val(),
            Contact: $("#inputContactCompany").val(),
            BirthCountryId: $("#inputCountryInfoPn").data().Id,
            PersonInterestGroup: $("#TableInterestGroups").UifDataTable('getSelected'),
            Id: $("#lblPersonCode").val()
        };
    }

    static CleanObject() {
        LaboralPersonalInformation = {
            Personal: {
                PersonType: { PersonTypeCode: " " },
                EducativeLevel: { Id: " " },
                Children: " ",
                HouseType: { Id: " " },
                SocialLayer: { Id: " " },
                SpouseName: " ",
                Nationality: " ",
                PersonCode: " "

            },
            LaborPerson: {
                Occupation: { Id: " " },
                Speciality: { Id: " " },
                OtherOccupation: { Id: " " },
                IncomeLevel: { Id: " " },
                CompanyName: " ",
                JobSector: " ",
                Position: " ",
                CompanyPhone: { Id: " " }
            },
            PersonInterestGroup: []
        }
    }

    static SavePersonJob() {
        lockScreen();
        LabourPersonRequest.SaveLabour(LaboralPersonalInformation).done(function (data) {
            if (data.success) {
                var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                    if (countAuthorization > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonPersonalInf);
                    }
                } else {
                    InformationPersonalGlobal = LaboralPersonalInformation;
                    $.UifNotify('show', { 'type': 'info', 'message': 'Guardado con éxito.', 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            StaffLabour.CleanObject();
            unlockScreen();
        }).fail(() => unlockScreen());
    }

    static GetPersonInterestGroups(individualId) {
        PersonRequest.GetPersonInterestGroups(individualId).done(function (data) {
            InformationPersonalGlobal.PersonInterestGroup = data.result;
            LaboralPersonalInformation.PersonInterestGroup = data.result;
            StaffLabour.CheckInterestVal();
        });
    }

    static CheckInterestVal() {
        $.each($('#TableInterestGroups').UifDataTable('getData'), function (key, value) {
            var Description = this.Description;
            if (LaboralPersonalInformation.PersonInterestGroup.filter(x => x.InterestGroupTypeId == this.InterestGroupTypeId).length > 0) {
                $("#TableInterestGroups td:last-child:contains(" + Description + ")")
                    .parents("tr")
                    .addClass("row-selected")
            }
        });

        $("#TableInterestGroups tr.row-selected")
            .find("span")
            .removeClass("glyphicon-unchecked")
        $("#TableInterestGroups tr.row-selected")
            .find("span")
            .addClass("glyphicon-check")
    }

    static GetLabourPerson(data) {

        if (data.IndividualId > 0) {

            $('#selectPersonIndividualType').val(data.PersonType);
            $('#selectIncomeLevel').val(data.IncomeLevel);
            $("#lblPersonCode").text(data.Id);
            $('#selectLevelEducation').val(data.EducativeLevel);
            $('#inputNameCompany').val(data.CompanyName);
            $('#inputNchildren').val(data.Children);
            $('#selectOccupation').val(data.Occupation);
            $('#inputofifce').val(data.Position);
            $('#selectOtherOccupation').val(data.OtherOccupation);
            $('#inputPhoneConpany').val(data.CompanyPhone);
            $('#selectStratum').val(data.SocialLayer);
            $('#inputSpouseName').val(data.SpouseName);
            $('#inputSector').val(data.JobSector);
            $("#inputContactCompany").val(data.Contact);
            $('#selectSpecialty').val(data.Speciality);
            $('#selectTypeHouse').val(data.HouseType);

            if (data.PersonInterestGroup != null && Array.isArray(data.PersonInterestGroup)) {
                $('#TableInterestGroups').UifDataTable("unselect");
                data.PersonInterestGroup.forEach(function (item) {
                    let InterestGroupTypeId = item.InterestGroupTypeId;
                    $('#TableInterestGroups tbody tr:eq(' + InterestGroupTypeId + ' )').removeClass('row-selected').addClass('row-selected');
                    $('#TableInterestGroups tbody tr:eq(' + InterestGroupTypeId + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    

                });
            }


        } else {

            $('#selectPersonIndividualType').val("");
            $('#selectIncomeLevel').val("");
            $("#lblPersonCode").text("");
            $('#selectLevelEducation').val("");
            $('#inputNameCompany').val("");
            $('#inputNchildren').val("");
            $('#selectOccupation').val("");
            $('#inputofifce').val("");
            $('#selectOtherOccupation').val("");
            $('#inputPhoneConpany').val("");
            $('#selectStratum').val("");
            $('#inputSpouseName').val("");
            $('#inputSector').val("");
            $("#inputContactCompany").val("");
            //    $("#inputCountryInfoPn").val(data.BirthCountryId);
            $('#selectSpecialty').val("");
            $('#selectTypeHouse').val("");

            if ($("#TableInterestGroups").UifDataTable("getSelected") != null) {
                $.each($("#TableInterestGroups").UifDataTable("getData"), function (key, value) {
                    $('#TableInterestGroups tbody tr:eq(' + key + ')').removeClass('row-selected');
                    $('#TableInterestGroups tbody tr:eq(' + key + ') td button span').removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
                });
            }

        }

    }

}