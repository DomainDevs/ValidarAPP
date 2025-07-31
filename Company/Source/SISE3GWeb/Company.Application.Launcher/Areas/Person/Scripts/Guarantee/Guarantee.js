//Variables globales
//$.ajaxSetup({ async: false });
var dataGuarantees;
var guaranteeId = "";
var individualId = "";
var guaranteeStatusCode = "";
var guaranteeTypeCode = "";
var numberDoc = "";
var guaranteeCode = "";
var guarantee = null;
var listBinacle = [];
var guaranteeTmp = {};
var policyModel = {};
var paramGuarantee = $('#ParamGuarantee').val();
var searchType = null; //$("#searchType").val();
var guaranteesType = null;
var parametersDefault = null;
var insuredGuaranteeTmp = {};
var GuarantorTmp = {};
var countryParameterProperty = 0;
var validateAddGuarantee = false;

class Guarantee_old extends Uif2.Page {
    getInitialState() {
        // Guarantee.GetCountriesGuarantee();
        Guarantee.InitializeGuarantee();
        Guarantee.HidePanelsGuarantee();
        Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        $('#panelButtonsRight').hide();
        Guarantee.loadGuaranteesTypes();
        Guarantee.AddListViewGuarantees();
        Guarantee.LoadGuarantees();
        //  Guarantee.GuaranteeEvents();
        objPrefixAssocieted.LoadPartialPrefixAssociated();
        //Guarantee.changeGuaranteeTypeShow("Mortage");
       // Guarantee.loatTitleGuaranteesTypes();
    }
    bindEvents() {
        $(".changeToUpper").keyup(function () {
            $(this).val($(this).val().toUpperCase());
        });

        $(".numberWithCommas").focusout(function () {
            var num = parseInt($(this).val());
            if (!isNaN(num)) {
                var form = FormatMoney(num);
                $(this).val(form);
            }
        });

        $(".numberWithCommas").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#selectGuarantees').on('itemSelected', function (event, selectedItem) {
            var data;
            $.each(dataGuarantees, function (index, val) {
                if (dataGuarantees[index].DoumentGuarantee == selectedItem.Id) {
                    data = dataGuarantees[index];
                }
            });

            if (data != null) {
                $('#panelButtonsRight').show();
                guaranteeId = data.InsuredGuarantee.Id;
                guaranteeStatusCode = data.InsuredGuarantee.GuaranteeStatus.Code;
                guaranteeCode = data.Code;
                numberDoc = data.DoumentGuarantee;
                Guarantee.LoadGuaranteeById(data.InsuredGuarantee.Id);

                objDocumentationReceived.LoadPartialDocumentationReceived();
                objPrefixAssocieted.LoadPartialPrefixAssociated();
                objBinnacle.LoadPartialBinnacle();
            }
            else {
                Guarantee.AddListViewGuarantees();

            }
            if (selectedItem.Id <= 0) {
                $("#selectGuaranteeList").UifSelect("setSelected", null);
            }

        });
        $('#btnAddGuarantees').on('click', function () {
            $("#selectGuarantees").UifSelect("setSelected", null);
            Guarantee.AddListViewGuarantees();
            Guarantee.ChangeSelectedType();
        });

        $('#Mortage-Company').on('itemSelected', function (event, selectedItem) {
            var result = Guarantee.existGuarantee(guaranteeId);

            var InsuranceCompany = {};
            InsuranceCompany.Description = selectedItem.Description;
            InsuranceCompany.Id = selectedItem.Id;

            if (result >= 0) {
                if (guarantee[result].InsuredGuarantee.InsuranceCompany == null) {
                    guarantee[result].InsuredGuarantee.InsuranceCompany = InsuranceCompany;
                }
                else {
                    guarantee[result].InsuredGuarantee.InsuranceCompany.Description = InsuranceCompany.Description;
                    guarantee[result].InsuredGuarantee.InsuranceCompany.Id = InsuranceCompany.Id;
                }
            }
            else {
                guaranteeTmp.InsuredGuarantee.InsuranceCompany = InsuranceCompany;
            }
        });

        $('#Pledge-Company').on('itemSelected', function (event, selectedItem) {
            var result = Guarantee.existGuarantee(guaranteeId);

            var InsuranceCompany = {};
            InsuranceCompany.Description = selectedItem.Description;
            InsuranceCompany.Id = selectedItem.Id;

            if (result >= 0) {
                if (guarantee[result].InsuredGuarantee.InsuranceCompany == null) {
                    guarantee[result].InsuredGuarantee.InsuranceCompany = InsuranceCompany;
                }
                else {
                    guarantee[result].InsuredGuarantee.InsuranceCompany.Description = InsuranceCompany.Description;
                    guarantee[result].InsuredGuarantee.InsuranceCompany.Id = InsuranceCompany.Id;
                }
            }
            else {
                guaranteeTmp.InsuredGuarantee.InsuranceCompany = InsuranceCompany;
            }
        });
        $("#btnCancelGuarantee").click(function () {
            Guarantee.ReturnPerson();
        });

        $("#btnRecordGuarantee").click(function () {
            if (window.TypeGuarantee == GuaranteeType.PromissoryNote)
                objPromissoryNote.SavePromissoryNote();
            if (window.TypeGuarantee == GuaranteeType.Mortage)
                objMortage.SaveMortage();
            if (window.TypeGuarantee == GuaranteeType.Pledge)
                objPledge.SavePledge();
            if (window.TypeGuarantee == GuaranteeType.Fixedtermdeposit)
                objFixedTermDeposit.SaveFixedTermDeposit();
            if (window.TypeGuarantee == GuaranteeType.Others)
                objOther.SaveOthers();

        });

        $("#selectGuaranteeList").UifSelect("setSelected", CommGuarantee.Mortage);

        $("#selectGuaranteeList").on("itemSelected", function (event, selectedItem) {
            $("#selectGuarantees").UifSelect("setSelected", null);
            Guarantee.hideTabGuarantee();
            if (selectedItem.Id > 0) {
                Guarantee.ChangeSelectedType();
            }
        });
    }

    static ChangeSelectedType() {
        //Guarantee.loatTitleGuaranteesTypes();
        $("#titleGuaranteesTypes").text($("#selectGuaranteeList").UifSelect("getSelectedText"));
        var gType = $("#selectGuaranteeList").UifSelect("getSelectedSource");
        if (gType != null && gType != undefined) {
            guaranteeCode = gType.GuaranteeType.Id;
        }
        else {
            guaranteeCode = 0;
        }
        
        Guarantee.ShowGuaranteeType(guaranteeCode);
        window.TypeGuarantee = guaranteeCode
        // Guarantee.bindsEventsGuaranteeType(guaranteeCode);
        Guarantee.clearForms();
    }
    //Seccion Funciones
    static InitializeGuarantee() {
        //asign saveGuarantee
        $("#ContractorId").val(guaranteeModel.ContractorId);     
        $("#inputContractorName").val(guaranteeModel.ContractorNumber); 
        $("#ContractorDocumentType").val(guaranteeModel.ContractorDocumentType);
        searchType = guaranteeModel.searchType;

        guaranteeId = "";
        guaranteeStatusCode = "";
        numberDoc = guaranteeId;

        if (parametersDefault == null) {
            ParameterRequest.GetParameters().done(function (data) {
                if (data.success) {
                    parametersDefault = data.result;
                    Guarantee.GetdefaultValueCountryGuarantee();
                    Guarantee.GetdefaultValueCurrencyGuarantee();
                }
            });
        }
        else {
            Guarantee.GetdefaultValueCountryGuarantee();
            Guarantee.GetdefaultValueCurrencyGuarantee();
        }

        individualId = $("#ContractorId").val();
        $.uif2.helpers.setGlobalTitle($("#inputContractorName").val() + "- " + $("#ContractorDocumentType").val() + " " + numberDoc);
        $("#numberWithCommas").OnlyDecimals(2);
        $("#PromissoryNote-NominalValue").OnlyDecimals(2);
        $('#DateNow').UifDatepicker('disabled', true);

    }

    static GetdefaultValueCurrencyGuarantee() {
        if (parametersDefault != null) {
            var pCurrency = parametersDefault.find(Guarantee.GetParameterByDescriptionGurantee, ['Currency']);
            if (pCurrency != undefined) {
                $("#PromissoryNote-Currency").UifSelect("setSelected", pCurrency.Value);
                $("#Mortage-Currency").UifSelect("setSelected", pCurrency.Value);
            }
        }
    }

    static GetdefaultValueCountryGuarantee() {
        if (parametersDefault != null) {
            var pCountry = parametersDefault.find(Guarantee.GetParameterByDescriptionGurantee, ['Country']);
            if (pCountry != undefined) {
                var dataCountry = {
                    Id: pCountry.Value,
                    Description: pCountry.TextParameter
                }
                $("#inputCountry-Mortage").data(dataCountry);
                $("#inputCountry-Mortage").val(dataCountry.Description);
                $("#inputCountry-FixedTermDeposit").data(dataCountry);
                $("#inputCountry-FixedTermDeposit").val(dataCountry.Description);
                $("#inputCountry-Pledge").data(dataCountry);
                $("#inputCountry-Pledge").val(dataCountry.Description);
                $("#inputCountry-PromissoryNote").data(dataCountry);
                $("#inputCountry-PromissoryNote").val(dataCountry.Description);
            }
        }
    }

    static GetParameterByDescriptionGurantee(parameter) {
        return parameter.Description == this[0];
    }
    
    static LoadListView(data) {
        if (data != null && data.length > 0) {
            dataGuarantees = data;
            $.each(dataGuarantees, function (index, val) {
                dataGuarantees[index].DoumentGuarantee = data[index].InsuredGuarantee.Id;
                dataGuarantees[index].DescriptionDocument = data[index].Description + " " + data[index].InsuredGuarantee.Id;
                dataGuarantees[index].InsuredGuarantee.ValueAmount = FormatMoney(dataGuarantees[index].InsuredGuarantee.DocumentValueAmount != null ? dataGuarantees[index].InsuredGuarantee.DocumentValueAmount : 0)
                dataGuarantees[index].InsuredGuarantee.IsCloseInd = Guarantee.GetOpen(dataGuarantees[index].InsuredGuarantee.IsCloseInd);
                guarantee[index].InsuredGuarantee.RegistrationDate = FormatDate(guarantee[index].InsuredGuarantee.RegistrationDate);
                guarantee[index].InsuredGuarantee.ExpirationDate = FormatDate(guarantee[index].InsuredGuarantee.ExpirationDate);
                guarantee[index].InsuredGuarantee.AppraisalDate = FormatDate(guarantee[index].InsuredGuarantee.AppraisalDate);
            });
            $("#selectGuarantees").UifSelect({ sourceData: dataGuarantees, id: "DocumentGuarantee", name: "DescriptionDocument" });
        }
    }

    static LoadListGuarantors(id) {
        var result = Guarantee.existGuarantee(id);

        $("#tableGuarantors").UifListView(
            {
                customAdd: true,
                customEdit: true,
                delete: true,
                deleteCallback: objGuarantors.deleteGuarantor,
                edit: true, displayTemplate: "#templateGuarantors",
                addTemplate: '#add-template',
                height: 300
            });

        if (result >= 0) {
            if (guarantee[result].InsuredGuarantee.Guarantors != null) {

                $("#NumberDocument-Guarantors").val(guarantee[result].InsuredGuarantee.DocumentNumber);

                $.each(guarantee[result].InsuredGuarantee.Guarantors, function (index, val) {
                    $("#tableGuarantors").UifListView("addItem", guarantee[result].InsuredGuarantee.Guarantors[index]);
                });
            }
        }

        else if (guaranteeId == 0) {
            // if (guaranteeTmp.Guarantors != null) {
            if (guaranteeTmp.InsuredGuarantee.Guarantors != null) {
                $.each(guaranteeTmp.InsuredGuarantee.Guarantors, function (index, val) {
                    $("#tableGuarantors").UifListView("addItem", guaranteeTmp.InsuredGuarantee.Guarantors[index]);
                });
            }
            else if (guaranteeTmp.InsuredGuarantee.Guarantors == null && searchType == 1) {

                GuarantorTmp.Name = $('#inputContractorName').val();
                GuarantorTmp.CardNro = $('#ContractorNumber').val();
                GuarantorTmp.Adrress = $('#Address').val();
                GuarantorTmp.PhoneNumber = $("#PhoneNumber").val();
                GuarantorTmp.CityText = $("#CityText").val();
                GuarantorTmp.IndividualId = $('#ContractorId').val();
                $("#tableGuarantors").UifListView("addItem", GuarantorTmp);
            }
        }
    }

    static AddListViewGuarantees() {
        $('#panelButtonsRight').show();
        Guarantee.newGuarantee();
        guaranteeTmp = {};
        insuredGuaranteeTmp = {};
        guaranteeTmp.InsuredGuarantee = insuredGuaranteeTmp;
        GuarantorsTmp = null;
        guaranteeId = 0;
        GuarantorsTmp = [];
        GuarantorTmp = {};
        Guarantee.ShowGuarantee(0);
        objDocumentationReceived.LoadPartialDocumentationReceived();
        objPrefixAssocieted.LoadPartialPrefixAssociated();
        objBinnacle.LoadPartialBinnacle();
        $("#btnNewDocumentatioReceived").trigger("click");
        $("#btnNewBranchAssociated").trigger("click");

    }

    static LoadGuaranteeById(id) {
        for (var i = 0; i < guarantee.length; i++) {
            var result = -1;
            if (guarantee[i].InsuredGuarantee.Id == id) {
                Guarantee.ShowGuarantee(guarantee[i]);
                break;
            }
        }
    }

    static ShowGuarantee(data) {
        Guarantee.hideTabGuarantee();
        if (data != 0) {
            Guarantee.changeTab(data);
            switch (data.GuaranteeType.Code) {
                case GuaranteeType.Mortage:
                    objMortage.showMortage(data);
                    break;
                case GuaranteeType.Pledge:
                    objPledge.showPledge(data);
                    break;
                case GuaranteeType.Fixedtermdeposit:
                    objFixedTermDeposit.showFixedTermDeposit(data);
                    break;
                case GuaranteeType.PromissoryNote:
                    objPromissoryNote.showPromissoryNote(data);
                    break;
                case GuaranteeType.Others:
                    objOther.showOthers(data);
                    break;
                default:
                    break;
            }
            Guarantee.showGuaranteeForm(data);
        }
        if (data == 0) {
            guaranteeCode = 0;
        }

    }

    static selectGuaranteeCodeEnum() {
        var idTab = $("#selectGuaranteeList").UifSelect("getSelected");
        guaranteeTmp.Code = idTab;
    }

    static SetCountrieStateCityGuarantee(guarantee, country, state, city) {
        if (country != null) {
            var dataCountry = {
                Id: country.Id,
                Description: country.Description
            }
            $("#inputCountry-" + guarantee).data(dataCountry);
            $("#inputCountry-" + guarantee).val(dataCountry.Description);
        }
        if (state != null) {
            var dataState = {
                Id: state.Id,
                Description: state.Description
            }
            $("#inputState-" + guarantee).data(dataState);
            $("#inputState-" + guarantee).val(dataState.Description);
        }

        if (city != null) {
            var dataCity = {
                Id: city.Id,
                Description: city.Description
            }
            $("#inputCity-" + guarantee).data(dataCity);
            $("#inputCity-" + guarantee).val(dataCity.Description);
        }
    }

    static existGuarantee(id) {
        var result = -1;
        for (var i = 0; i < guarantee.length; i++) {
            if (guarantee[i].InsuredGuarantee.Id != null && guarantee[i].InsuredGuarantee.Id == id) {
                result = i;
                break;
            }
        }
        return result;
    }

    static changeTab(tab) {      
        Guarantee.clearForms();
        $("#selectGuaranteeList").UifSelect("setSelected", tab.Id);
        //Guarantee.loatTitleGuaranteesTypes();
        window.TypeGuarantee = tab.GuaranteeType.Id;
        Guarantee.ShowGuaranteeType(tab.GuaranteeType.Id);
      
    }

    static showGuaranteeForm(data) {
        $("#newGuaranteeForm").show();
        $("#selectBranchGuarantee").val(data.InsuredGuarantee.Branch.Id);
        $("#selectStatusGuarantee").val(data.InsuredGuarantee.GuaranteeStatus.Code);

        if (data.InsuredGuarantee.IsCloseInd == "SI") {
            $('#IsClosed').prop('checked', false);
        }
        else {
            $('#IsClosed').prop('checked', true);
        }
    }

    static GetOpen(openEnabled) {
        switch (openEnabled) {
            case true:
                return "NO"
                break;
            default:
                return "SI"
        }
    }

    static newGuarantee() {
        $("#newGuaranteeForm").show();
        Guarantee.clearForms();
        guaranteeId = "";
    }

    static clearForms() {        
        objPromissoryNote.clearPromissoryNote();
        objFixedTermDeposit.clearFixedTermDeposit();
        objMortage.clearMortage();
        objPledge.clearPledge();
        objActions.clearActions();
        objOther.clearOthers();
        Guarantee.clearGuaranteeForm();       
    }
    static ClearFormByGuaranteeType(guaranteeType) {
        switch (guaranteeType) {
            case GuaranteeType.PromissoryNote:
                objPromissoryNote.clearPromissoryNote();
                break;
            case GuaranteeType.Mortage:
                objMortage.clearMortage();
                break;
            case GuaranteeType.Pledge:
                objPledge.clearPledge();
                objActions.clearActions();
                break;
            case GuaranteeType.Fixedtermdeposit:
                objFixedTermDeposit.clearFixedTermDeposit();
                break;
            case GuaranteeType.Others:
                objOther.clearOthers();
                break;
            default:
                break;
        }
        Guarantee.clearGuaranteeForm();
    }
    static clearGuaranteeForm() {
        var idSelectGuaranteesTypes = $("#selectGuaranteeList").UifSelect("getSelected");
        $("#Guarantee").get(0).reset()
        $("#selectGuaranteeList").UifSelect("setSelected", idSelectGuaranteesTypes);
        $("#selectBranchGuarantee").val("");
        $("#selectStatusGuarantee").val("");
        $('#IsClosed').attr('checked', false);
        $('#selectedDocumentationReceived').html("(Sin datos)");
        $('#selectedPrefixAssociated').html("");
        objPrefixAssocieted.LoadPartialPrefixAssociated();
    }

    static HidePanelsGuarantee() {
        //Página principal contragarantía
        $("#modalGuarantee").hide();
        $("#buttonsGuarantee").hide();

        //Panel documentación recibida
        $("#modalDocumentatioReceived").UifModal("hide");

        //Panel ramos asociados
        $("#modalPrefixAssociated").UifModal("hide");

        //Panel bitácora
        $("#modalBinnacle").UifModal("hide");

        //Panel Vincular póliza
        $("#modalBindPolicy").hide();
        $("#buttonsBindPolicy").hide();

        //Panel contragarantes
        $("#modalGuarantors").UifModal("hide");


    }

    static ShowPanelsGuarantee(Menu) {
        switch (Menu) {
            case MenuType.GUARANTEE:
                $("#modalGuarantee").show();
                $("#buttonsGuarantee").show();
                break;

            case MenuType.DOCUMENTATION:
                $("#modalGuarantee").show();
                $("#buttonsGuarantee").show();
                $("#modalDocumentatioReceived").UifModal('showLocal', AppResources.LabelDocumentationReceived);
                break;
            case MenuType.PREFIXASSOCIATED:
                $("#modalGuarantee").show();
                $("#buttonsGuarantee").show();
                $("#modalPrefixAssociated").UifModal('showLocal', AppResources.LabelBindPolicy);
                break;
            case MenuType.BINNACLE:
                $("#modalGuarantee").show();
                $("#buttonsGuarantee").show();
                $("#modalBinnacle").UifModal('showLocal', AppResources.LabelBinnacle);
                break;
            case MenuType.BINDPOLICY:
                $("#modalBindPolicy").show();
                $("#buttonsBindPolicy").show();
                break;

            case MenuType.GUARANTORS:
                $("#modalGuarantors").UifModal('showLocal', AppResources.LabelGuarantors);
                break;
            default:
                break;
        }
    }

    static LoadGuarantees() {
        if (guarantee == null) {
            GuaranteeRequest.GetInsuredGuaranteeByIndividualId(individualId).done(function(data) {
                if (data.success) {
                    guarantee = data.result;
                    Guarantee.LoadListView(data.result);
                }
            });
        }
        else {
            Guarantee.LoadListView(guarantee);
        }

    }

    static GetStatesByCountry(select, id) {
        countryParameterProperty = $("#selectCountry-" + select).val();

        if (countryParameterProperty > 0) {
            GuaranteeRequest.GetStatesByCountryId(countryParameterProperty).done(function (data) {
                if (data.success) {
                    $("#selectState-" + select).UifSelect({ source: controller });
                }
            });
        }
        else {
            $("#selectState-" + select).UifSelect();
        }
        $("#selectCity-" + select).UifSelect({ selectedId: 0 });
    }

    //static GetCountriesGuarantee() {
    //    $.ajax({
    //        type: "POST",
    //        url: rootPath + 'Person/Guarantee/GetCountries',
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8"
    //    }).done(function (data) {
    //    }).fail(function (jqXHR, textStatus, errorThrown) {
    //        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryingCountries + ' : ' + data.result.ErrorMsg, 'autoclose': true })
    //    });
    //}

    static validateDocumentationAndPrefix() {
        var result = 0;
        var docNumber = 0;
        var prefixNumber = 0;
        var existsGuarantee = Guarantee.existGuarantee(guaranteeId);

        if (existsGuarantee >= 0) {
            docNumber = guarantee[result].InsuredGuarantee.listDocumentation.length
            prefixNumber = guarantee[result].InsuredGuarantee.listPrefix.length
        }
        else if (guaranteeId == 0) {
            docNumber = guaranteeTmp.InsuredGuarantee.listDocumentation.length
            prefixNumber = guaranteeTmp.InsuredGuarantee.listPrefix.length
        }

        if (docNumber <= 0) {
            $.UifDialog('alert', { 'message': AppResources.MustHaveDocumentation });
        }
        else if (prefixNumber <= 0) {
            $.UifDialog('alert', { 'message': AppResources.MustHaveAssociatedBranches });
        }
        else {
            result = 1;
        }

        return result;
    }


    static saveGuarantee(data, guaranteeType) {
        GuaranteeRequest.SaveInsuredGuarantee(data).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $.UifDialog('alert', { 'message': AppResources.SuccessfullyGuaranteesSaved });
                    Guarantee.ClearFormByGuaranteeType(guaranteeType);
                    var result = Guarantee.existGuarantee(data.result.InsuredGuarantee.Id);
                    if (result < 0) {
                        guarantee.push(data.result);
                    }
                    Guarantee.LoadGuarantees();
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            }
        });
    }

    static updateList(data) {
        var id = data.InsuredGuarantee.Id;
        var result = Guarantee.existGuarantee(id);

        if (result < 0) {
            guarantee.push(data);

            var tmpGuarante = data;

            tmpGuarante.InsuredGuarantee.ValueAmount = FormatMoney(data.InsuredGuarantee.DocumentValueAmount != null ? data.InsuredGuarantee.DocumentValueAmount : 0)

            tmpGuarante.InsuredGuarantee.IsCloseInd = Guarantee.GetOpen(data.InsuredGuarantee.IsCloseInd);

            tmpGuarante.InsuredGuarantee.RegistrationDate = FormatDate(data.InsuredGuarantee.RegistrationDate);
            tmpGuarante.InsuredGuarantee.ExpirationDate = FormatDate(data.InsuredGuarantee.ExpirationDate);
            tmpGuarante.InsuredGuarantee.AppraisalDate = FormatDate(data.InsuredGuarantee.AppraisalDate);

            $("#listViewGuarantees").UifListView("addItem", tmpGuarante);
        }
    }

    static ReturnPerson() {
        var person = { IndividualId: individualId };
        switch (parseInt(searchType, 10)) {
            case TypePerson.PersonNatural:
                window.location = rootPath + 'Common/Main/Main?type=7&typeGuarantee=1&IndividualId=' + individualId;
                break;
            case TypePerson.PersonLegal:
                window.location = rootPath + 'Common/Main/Main?type=7&typeGuarantee=2&IndividualId=' + individualId;
                break;
            default:
                break;
        }
    }

    static ValidateGuarantee() {
        var error = "";

        if (window.TypeGuarantee == undefined || window.TypeGuarantee == null)
            window.TypeGuarantee = GuaranteeType.PromissoryNote;

        if ($('#selectBranchGuarantee').val() == "" || $('#selectBranchGuarantee').val() == undefined)
            error = error + "Sucursal <br>";
        if ($('#selectStatusGuarantee').val() == "" || $('#selectStatusGuarantee').val() == undefined)
            error = error + "Estado <br>";

        if (window.TypeGuarantee == GuaranteeType.PromissoryNote) {

            if ($('#inputCountry-PromissoryNote').data("Id") == '' || $('#inputCountry-PromissoryNote').data("Id") == undefined || $('#inputCountry-PromissoryNote').data("Id") == null)
                error = error + "Pais <br>";
            if ($('#inputState-PromissoryNote').data("Id") == '' || $('#inputState-PromissoryNote').data("Id") == undefined || $('#inputState-PromissoryNote').data("Id") == null)
                error = error + "Departamento <br>";
            if ($('#inputCity-PromissoryNote').data("Id") == '' || $('#inputCity-PromissoryNote').data("Id") == undefined || $('#inputCity-PromissoryNote').data("Id") == null)
                error = error + "Municipio <br>";
            if ($('#PromissoryNote-ConstitutionDate').val() == '' || $('#PromissoryNote-ConstitutionDate').val() == undefined)
                error = error + "Fecha Constitución<br>";
            if ($('#PromissoryNote-NominalValue').val() == '' || $('#PromissoryNote-NominalValue').val() == undefined)
                error = error + "Valor Nominal<br>";
            if ($('#PromissoryNote-Currency').val() == '' || $('#PromissoryNote-Currency').val() == undefined)
                error = error + "Moneda<br>";
            if ($('#PromissoryNote-PromissoryNoteType').val() == '' || $('#PromissoryNote-PromissoryNoteType').val() == undefined)
                error = error + "Tipo de Pagare<br>";
            if (searchType == 1) {
                if (parseInt($('#PromissoryNote-SignatoriesNumber').val(), 10) < 1 || $('#PromissoryNote-SignatoriesNumber').val() == undefined)
                    error = error + "Nro. Firmantes<br>";
            }
        }
        if (window.TypeGuarantee == GuaranteeType.Mortage) {
            if ($('#inputState-Mortage').data("Id") == '' || $('#inputState-Mortage').data("Id") == undefined || $('#inputState-Mortage').data("Id") == null)
                error = error + "Departamento<br>";
            if ($('#inputCity-Mortage').data("Id") == '' || $('#inputCity-Mortage').data("Id") == undefined || $('#inputCity-Mortage').data("Id") == null)
                error = error + "Municipio<br>";
            if ($('#Mortage-AssetType').val() == '' || $('#Mortage-AssetType').val() == undefined)
                error = error + "Tipo de bien<br>";
            if ($('#Mortage-DeedNumber').val() == '' || $('#Mortage-DeedNumber').val() == undefined)
                error = error + "Nro. de escritura<br>";
            if ($('#Mortage-ValuationValue').val() == '' || $('#Mortage-ValuationValue').val() == undefined)
                error = error + "Valor Avalúo<br>";
        }
        if (window.TypeGuarantee == GuaranteeType.Pledge) {
            if ($('#inputState-Pledge').data("Id") == '' || $('#inputState-Pledge').data("Id") == undefined || $('#inputState-Pledge').data("Id") == null)
                error = error + "Departamento<br>";
            if ($('#inputCity-Pledge').data("Id") == '' || $('#inputCity-Pledge').data("Id") == undefined || $('#inputCity-Pledge').data("Id") == null)
                error = error + "Municipio<br>";
            if ($('#Pledge-Plate').val() == '' || $('#Pledge-Plate').val() == undefined)
                error = error + "Placa<br>";
        }
        if (window.TypeGuarantee == GuaranteeType.Fixedtermdeposit) {
            if ($("#inputState-FixedTermDeposit").data("Id") == '' || $("#inputState-FixedTermDeposit").data("Id") == undefined || $("#inputState-FixedTermDeposit").data("Id") == null)
                error = error + "Departamento<br>";
            if ($("#inputCity-FixedTermDeposit").data("Id") == '' || $("#inputCity-FixedTermDeposit").data("Id") == undefined || $("#inputCity-FixedTermDeposit").data("Id") == null)
                error = error + "Municipio<br>";
            if ($('#FixedTermDeposit-DocumentNumber').val() == '' || $('#FixedTermDeposit-DocumentNumber').val() == undefined)
                error = error + "Número Documento<br>";
            if ($('#FixedTermDeposit-IssuingEntity').val() == '' || $('#FixedTermDeposit-IssuingEntity').val() == undefined)
                error = error + "Entidad Emisora<br>";
            if ($('#FixedTermDeposit-ConstitutionDate').val() == '' || $('#FixedTermDeposit-ConstitutionDate').val() == undefined)
                error = error + "Fecha Constitución<br>";
            if ($('#FixedTermDeposit-NominalValue').val() == '' || $('#FixedTermDeposit-NominalValue').val() == undefined)
                error = error + "Valor nominal<br>";

        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }


    }


    static ShowGuaranteeType(id) {
        switch (parseInt(id)) {
            case GuaranteeType.Mortage:
                Guarantee.changeGuaranteeTypeShow("Mortage");
                break;
            case GuaranteeType.Pledge:
                Guarantee.changeGuaranteeTypeShow("Pledge");
                break;
            case GuaranteeType.Fixedtermdeposit:
                Guarantee.changeGuaranteeTypeShow("CDT");
                break;
            case GuaranteeType.PromissoryNote:
                Guarantee.changeGuaranteeTypeShow("PromissoryNote");
                break;
            case GuaranteeType.Others:
                Guarantee.changeGuaranteeTypeShow("Others");
                break;
            default:
                $("#selectGuaranteeList").UifSelect("setSelected", null);
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorNoExistModuleGuaranteesTypes, 'autoclose': true })
                $("#titleGuaranteesTypes").text("");
                break;
        }
    }


    static bindsEventsGuaranteeType(id) {
        switch (parseInt(id)) {
            case GuaranteeType.Mortage:
                bindEventsMortage();
                break;
            case GuaranteeType.Pledge:
                bindEventsPledge();
                break;
            case GuaranteeType.Fixedtermdeposit:
                bindEventsCDT();
                break;
            case GuaranteeType.PromissoryNote:
                bindEventsPromissoryNote();
                break;
            case GuaranteeType.Others:
                objOther.bindEventsOthers();
                break;
            default:
                break;
        }
    }

    static hideTabGuarantee() {
        $("#tabPromissoryNote").hide();
        $("#tabMortage").hide();
        $("#tabPledge").hide();
        $("#tabActions").hide();
        $("#tabCDT").hide();
        $("#tabOthers").hide();
        $("#titleGuaranteesTypes").text("");
    }

    static changeGuaranteeTypeShow(tag) {
        $("#tab" + tag).show();
    }

    static loadGuaranteesTypes() {
        if (guaranteesType == null) {
            GuaranteeRequest.GetGuarantees(countryParameterProperty).done(function (data) {
                if (data.success) {
                    guaranteesType = data.result;
                    $("#selectGuaranteeList").UifSelect({ sourceData: guaranteesType, selectedId: GuaranteeType.Mortage });
                  //  window.TypeGuarantee = GuaranteeType.Mortage;
                   // $("#titleGuaranteesTypes").text($("#selectGuaranteeList").UifSelect("getSelectedText"));
                    Guarantee.ChangeSelectedType();
                }
            });
        }
        else {
            $("#selectGuaranteeList").UifSelect({ sourceData: guaranteesType, selectedId: GuaranteeType.Mortage });
           // window.TypeGuarantee = GuaranteeType.Mortage;
           // $("#titleGuaranteesTypes").text($("#selectGuaranteeList").UifSelect("getSelectedText"));
            Guarantee.ChangeSelectedType();
        }
      
    }

    //static loatTitleGuaranteesTypes() {
    //    $("#titleGuaranteesTypes").text($("#selectGuaranteeList").UifSelect("getSelectedSource").GuaranteeType.Description);
    //}
}
