var agentSearchType = 0;
var dropDownProfile;
var optionMenu = [];
var currentDate = null;
var profile = [];
var userStatus = [];
var profileTable = [];
var glbAllySalePoints = [];
var modalSave = false;
var glbUserAccess = [];
$.ajaxSetup({ async: true });

class Profiles {
    static GetProfiles() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Profile/GetProfiles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class CurrentDateTime {
    static GetCurrentDatetime() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/UniqueUser/DateNow',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class UserRequest {
    /**
     * @summary 
     *  Consulta los usuarios en segun los parametros
     * @param {idHierarchy}id de jerarquia
     * @param {idModule}id modulo
     * @param {idSubmodule}id del submodulo
    **/
    static GetUsersByHierarchyModuleSubmodule(idHierarchy, idModule, idSubmodule) {
        return $.ajax({
            type: "POST",
            data: { "idHierarchy": idHierarchy, "idModule": idModule, "idSubmodule": idSubmodule },
            url: rootPath + "UniqueUser/UniqueUser/GetUsersByHierarchyModuleSubmodule"
        });
    }
}

class UniqueUser extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#NameSurname").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);

        Profiles.GetProfiles().done(function (data) {
            if (data.success) {
                $('#selectProfile').UifSelect({ sourceData: data.result });
                //profileTable = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        CurrentDateTime.GetCurrentDatetime().done(function (data) {
            if (data.success) {
                var creacion = UniqueUser.FormatDate(data.result);
                currentDate = UniqueUser.FormatDate(data.result);
                $("#inputCreationDate").UifDatepicker('setValue', UniqueUser.FormatDate(data.result));
                $("#inputLastModificationDate").UifDatepicker('setValue', UniqueUser.FormatDate(data.result));
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        dropDownProfile = uif2.dropDown({
            source: rootPath + 'UniqueUser/UniqueUser/Profiles',
            element: '#btnShowProfiles',
            align: 'left',
            width: 430,
            height: 450,
            loadedCallback: this.componentLoadedCallback
        });

        UserStatusType.GetUserStatus().done(function (data) {
            if (data.success) {
                userStatus = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {
        agentSearchType = 1;
        $('#chkDisabled').on('change', this.SetCurrentDate);
        $('#inputUser').on('buttonClick', UniqueUser.SearchUser);
        $('#NameSurname').on('buttonClick', this.GetPersonByIdDescription);
        $('#btnSaveUser').on('click', UniqueUser.buttonSaveUniqueUser);
        $('#btnNewUser').on('click', UniqueUser.ClearForm);
        $('#tableResults tbody').on('click', 'tr', this.SelectSearch);
        $('#btnAcceptNewPersonOnline').click(this.RedirectToPerson);
        $("#btnProfileSave").on("click", this.saveProfiles);
        $("#btnProfileClose").on("click", this.closeProfiles);
        $('#btnExit').click(this.Exit);
        $('#tableAllyIntermediary tbody').on('click', 'tr', this.SelectSearch);
        GetAccessButtonsByPath(window.location.pathname + window.location.search);
        UniqueUser.ClearForm();

        if (glbPersonOnline != null) {
            UniqueUser.GetPersonByIndividualId(glbPersonOnline.IndividualId);
        }

        $('#btnBranchSale').on('click', UniqueUser.a);
        //(Deshabilitar) --Reinicia la informacion de los perifericos
        //$('#selectProfile').on('itemSelected', UniqueUser.ClearBranchChangeProfile)

        $('#InputExpirationPasswordDate').on("datepicker.change", function (event, date) {
            UniqueUser.CalculateDaysDifference($("#InputExpirationPasswordDate").val(), moment().format("DD/MM/YYYY"));
        });
        $("#ExpirationDays").ValidatorKey(ValidatorType.Number, 2, 1);
        $("#ExpirationDays").on('change', function (event) {
            UniqueUser.CalculateVigencyDateByAddDays($("#ExpirationDays").val());
        });
        
        
    }
    static ClearBranchChangeProfile() {
        $('#selectedBranchSale').text('');
        $('#selectedHierarchy').text('');
        glbUser.Hierarchies = null;
        glbUser.Branch = null;
        //glbUser = { UserId: 0 };
    }

    static AdvancedSearchEvents() {
        $("#btnShowProfiles").on("click", function () {
            dropDownProfile.show();
        });
    }

    componentLoadedCallback() {
        UniqueUser.AdvancedSearchEvents();
    }

    static a() {
        $('#btnBranchSale').val('a')
    }

    closeProfiles() {
        dropDownProfile.hide();
    }

    SetCurrentDate() {
        if ($('#chkDisabled').is(':checked')) {
            $("#InputDisabledDate").UifDatepicker('setValue', currentDate);
        }
        else {
            $("#InputDisabledDate").UifDatepicker('setValue', null);
        }
    }

    static SearchUser(event, selectedItem) {
        $('#inputUserbutton').prop('disabled', true);
        UniqueUser.ClearForm();
        UniqueUser.getUserById(selectedItem, 0, 0);
        $('#inputUserbutton').prop('disabled', false);
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    static ClearForm() {
        glbUser = { UserId: 0 };
        //product = {};
        //tempModalProducts = {};
        //previousPrefix = null;
        $("#inputUser").val('');
        $("#NameSurname").data("Object", null);
        $("#NameSurname").val('');
        $("#LoginName").val('');
        $("#LoginName").focus();
        $("#Password").val('');
        $("#Password").removeAttr("placeholder");
        $("#Confirmation").val('');
        $("#Confirmation").removeAttr("placeholder");
        $('#chkBlocked').prop('checked', false);
        $('#chkDisabled').prop('checked', false);
        $("#InputDisabledDate").UifDatepicker('setValue', null);
        $("#inputLastModificationDate").UifDatepicker('setValue', null);
        $('#inputLastModificationDate').UifDatepicker('disabled', true);
        $('#inputCreationDate').UifDatepicker('disabled', true);
        $("#selectProfile").UifSelect("setSelected", null);
        $('#selectedHierarchy').text('');
        $('#selectedWork').text('');
        $('#selectedBranchSale').text('');
        $('#selectedAlliedSalePoints').text('');
        $('#selectedProduct').text('');
        $('#chkUpdatePassword').prop("checked", true);
        $("#inputCreationDate").UifDatepicker('setValue', currentDate);
        $("#inputLastModificationDate").UifDatepicker('setValue', currentDate);
        $("#inputDateExpirationUser").UifDatepicker('setValue', null);
        $("#InputExpirationPasswordDate").UifDatepicker('setValue', null);
        $("#ExpirationDays").val('');
        ClearValidation('#formUser');

        if (!modalSave) {
            glbAllySalePoints = [];
        }
    }
    

    static validateDates(userData) {
        if (glbUser.UserId != 0) {
            if (userData.UniqueUsersLogin != null && userData.UniqueUsersLogin.ExpirationDate != "") {
                var f = new Date();
                if (CompareDates(UniqueUser.FormatDate(userData.UniqueUsersLogin.ExpirationDate), (f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear())) != 1) {
                    return false;
                }
                else if (CompareDateEquals(userData.UniqueUsersLogin.ExpirationDate, currentDate) != 1) {
                    return false;
                }
            }

            if (userData.ExpirationDate != null && userData.ExpirationDate != "") {
                if (userData.CreationDate != null && userData.CreationDate != "") {
                    if (CompareDateEquals(userData.ExpirationDate, userData.CreationDate) != 1) {
                        return false;
                    }
                }
                else if (CompareDateEquals(userData.ExpirationDate, currentDate) != 1) {
                    return false;
                }
            }
        }
        else {
            if (userData.ExpirationDate != "" && CompareDateEquals(userData.ExpirationDate, currentDate) != 1) {
                return false;
            }

            if (userData.UniqueUsersLogin != null && userData.UniqueUsersLogin.ExpirationDate != "") {
                if (CompareDateEquals(userData.UniqueUsersLogin.ExpirationDate, currentDate) != 1) {
                    return false;
                }
            }
        }
        return true;
    }

    static buttonSaveUniqueUser() {
        modalSave = false;
        UniqueUser.saveUniqueUser();
    }

    static saveUniqueUser() {
        var resultOperation = false;
        if (glbUser.PersonId != null) {
            if ((!resultOperation) && (UniqueUser.validateLoginName())) {
                var userData = UniqueUser.getUserModel();
                if (UniqueUser.validateDates(userData)) {
                    if ($("#Password").val() == "" && glbUser.UserId == 0) {
                        if (!modalSave) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDocumentPassword, 'autoclose': true })
                        }
                    }
                    else {
                        if (glbUser.PersonId == null) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessagePersonEmpty, 'autoclose': true })
                        }
                        else {
                            if (glbUser.Branch == undefined || glbUser.Branch.length <= 0) {
                                if (!modalSave) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserWithOutBranch, 'autoclose': true })
                                }
                            }
                            else {
                                if (glbUser.Hierarchies == undefined || glbUser.Hierarchies.length <= 0) {
                                    if (!modalSave) {
                                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserWithOutHierarchy, 'autoclose': true })
                                    }
                                }
                                else {
                                    UserAgent.loadDataAgents();
                                    $.ajax({
                                        type: "POST",
                                        url: rootPath + 'UniqueUser/UniqueUser/SaveUniqueUser',
                                        data: JSON.stringify({ user: userData }),
                                        dataType: "json",
                                        contentType: "application/json; charset=utf-8"
                                    }).done(function (data) {
                                        if (data.success) {
                                            glbUser.UserId = data.result;
                                            resultOperation = true;
                                            if (!modalSave) {
                                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInformation, 'autoclose': true })
                                                UniqueUser.ClearForm();
                                            }
                                        }
                                        else {
                                            for (var i = 0; i < data.result.length; i++) {
                                                $.UifNotify('show', { 'type': 'danger', 'message': data.result[i], 'autoclose': true });
                                            }
                                            //$.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
                                            resultOperation = false;
                                        }
                                    }).fail(function (jqXHR, textStatus, errorThrown) {
                                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveUser, 'autoclose': true })
                                        resultOperation = false;
                                    });
                                }
                            }
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExpirationDateGreater, 'autoclose': true })
                }
            }
        }
        else {
            $('#Password').attr("data-val", "true");
            if (UniqueUser.validateForm()) {
                var userData = UniqueUser.getUserModel();
                if (UniqueUser.validateDates(userData)) {
                    if (glbUser.PersonId == null) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessagePersonEmpty, 'autoclose': true })
                    }
                    else {
                        if (glbUser.Branch == undefined || glbUser.Branch.length <= 0) {
                            if (!modalSave) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserWithOutBranch, 'autoclose': true })
                            }
                        }
                        else {
                            if (glbUser.Hierarchies == undefined || glbUser.Hierarchies.length <= 0) {
                                if (!modalSave) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserWithOutHierarchy, 'autoclose': true })
                                }
                            }
                            else {
                                UserAgent.loadDataAgents();
                                $.ajax({
                                    type: "POST",
                                    url: rootPath + 'UniqueUser/UniqueUser/SaveUniqueUser',
                                    data: JSON.stringify({ user: userData }),
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8"
                                }).done(function (data) {
                                    if (data.success) {
                                        glbUser.UserId = data.result;
                                        resultOperation = true;
                                        if (!modalSave) {
                                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInformation, 'autoclose': true })
                                            UniqueUser.ClearForm();
                                        }
                                    }
                                    else {
                                        for (var i = 0; i < data.result.length; i++) {
                                            $.UifNotify('show', { 'type': 'danger', 'message': data.result[i], 'autoclose': true });
                                        }
                                        //$.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
                                        resultOperation = false;
                                    }
                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveUser, 'autoclose': true })
                                    resultOperation = false;
                                });
                            }
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExpirationDateGreater, 'autoclose': true })
                }
            }
        }
        return resultOperation;
    }

    static validateForm() {
        $("#formUser").validate();
        return $("#formUser").valid();
    }

    static validateLoginName() {
        $("#LoginName").validate();
        return $("#LoginName").valid();
    }

    GetPersonByIdDescription() {
        var description = $('#NameSurname').val();
        var number = parseInt(description, 10);

        if (isNaN(number)) {
            number = "";
        }
        else {
            description = "";
        }

        if (number != "" || description != "") {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetPersonByIdDescription',
                data: JSON.stringify({ userId: number, description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $("#NameSurname").data("Object", data.result[0]);
                        $("#NameSurname").val(data.result[0].Name + " (" + data.result[0].IdentificationDocument.Number + ")");
                    }
                    else if (data.result.length > 1) {
                        modalListType = 2;
                        var dataList = [];

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name
                            });
                        }

                        UniqueUser.ShowDefaultResults(dataList);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.Users);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessagePerson, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchUsers, 'autoclose': true })
            });
        }
    }

    SelectSearch() {
        switch (modalListType) {
            case 1:
                UserAgent.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
                break;
            case 2:
                UniqueUser.GetPersonByIndividualId($(this).children()[0].innerHTML);
                break;
            case 3:
                UniqueUser.getUserById("", 0, $(this).children()[0].innerHTML);
                break;
            //case 4:
            //    AlliedSalePoints.GetAgentAgencyByPrimaryKey($(this).children()[0].innerHTML, $(this).children()[3].innerHTML);
            //    break;
            default:
                break;
        }

        $('#modalDefaultSearch').UifModal("hide");
        $('#modalDefaultSearchAllyIntermediary').UifModal("hide");
    }
    RedirectToPerson() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: glbUser
        };

        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#NameSurname").val().trim());
    }
    static LoadSubTitles() {
        $('#selectedHierarchy').text('');
        $('#selectedWork').text('');
        $('#selectedProduct').text('');
        $('#selectedBranchSale').text('');
        $('#selectedAlliedSalePoints').text('');

        if (glbUser.Hierarchies != undefined) {
            if (glbUser.Hierarchies.length > 1) {
                $('#selectedHierarchy').text(AppResources.LabelVarious);
            }

            if (glbUser.Hierarchies.length == 1) {
                $('#selectedHierarchy').text(glbUser.Hierarchies[0].Description);
            }
        }
        if (glbUser.Branch != null) {
            if (glbUser.Branch.length > 1) {
                $('#selectedBranchSale').text(AppResources.LabelVarious);
            }

            if (glbUser.Branch.length == 1) {
                $('#selectedBranchSale').text(glbUser.Branch[0].Description);
            }
        }
        if (glbUser.IndividualsRelation != null) {
            if (glbUser.IndividualsRelation.length > 1) {
                $('#selectedWork').text(AppResources.LabelVarious);
            }

            if (glbUser.IndividualsRelation.length == 1) {
                $('#selectedWork').text(glbUser.IndividualsRelation[0].ChildIndividual.FullName);
            }
        }
        //if (glbUser.UniqueUsersProduct != undefined) {
        //	if (glbUser.UniqueUsersProduct.length > 1) {
        //		$('#selectedProduct').text(AppResources.LabelVarious);
        //	}

        //	if (glbUser.UniqueUsersProduct.length == 1) {
        //		$('#selectedProduct').text(glbUser.UniqueUsersProduct[0].ProductDescription);
        //	}
        //}

        //if (glbAllySalePoints.length > 0) {
        //    if (glbAllySalePoints.length == 1) {
        //        $("#selectedAlliedSalePoints").text(glbAllySalePoints[0].SalePointDescription);
        //    }
        //    else {
        //        $('#selectedAlliedSalePoints').text(AppResources.LabelVarious);
        //    }
        //}
        //else {
        //    AlliedSalePoints.GetUniqueUserSalePointText(glbUser.individualId, glbUser.UserId);
        //}


    }

    static GetPersonByIndividualId(individualId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetPersonByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $("#NameSurname").data("Object", data.result);
                $("#NameSurname").val(data.result.Name + " (" + data.result.IdentificationDocument.Number + ")");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchUsers, 'autoclose': true })
        });
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    static ShowDefaultAllyIntermediary(dataTable) {
        $('#tableAllyIntermediary').UifDataTable('clear');
        $('#tableAllyIntermediary').UifDataTable('addRow', dataTable);
    }

    static getUserById(accountName, personId, id) {
        if (accountName != "" || personId != "" || id != "") {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetUserByAccountName',
                data: JSON.stringify({ accountName: accountName, personId: personId, userId: id }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (Array.isArray(data.result)) {
                        if (data.result.length === 1) {
                            UniqueUser.updateGlbUser(data.result[0]);
                            UniqueUser.loadUser(glbUser);
                        }
                        else if (data.result.length > 1) {
                            $("#NameSurname").val(accountName);
                            UniqueUser.showDataAdvanced(data.result);
                        }
                    }
                    else {
                        $.UifNotify('show',
                            { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                }
                else {
                    UniqueUser.ClearForm(true);
                    $.UifNotify('show',
                        { 'type': 'danger', 'message': AppResources.ErrorSearch, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                UniqueUser.ClearForm(true);
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearch, 'autoclose': true });
            });
        }
    }

    static showDataAdvanced(data) {
        UserAdvSearch.LoadUserAdvanced(data);
        dropDownSearch.show();
    }

    static updateGlbUser(data) {
        glbUser = data;
        //PrefixUser.loadPartialPrefixUser(false);
        $('#inputUser').val(data.AccountName);
    }

    static loadUser(userData) {
        $("#LoginName").val(userData.AccountName);
        $('#inputUser').val(userData.AccountName);
        $("#inputCreationDate").UifDatepicker('setValue', FormatDate(userData.CreationDate), 1);
        $("#inputLastModificationDate").UifDatepicker('setValue', FormatDate(userData.LastModificationDate), 1);
        $("#inputDateExpirationUser").UifDatepicker('setValue', FormatDate(userData.ExpirationDate), 1);
        $("#InputDisabledDate").UifDatepicker('setValue', FormatDate(userData.DisableDate), 1);
        UniqueUser.GetPersonByIndividualId(userData.PersonId);
        //glbUser.UniqueUsersProduct = [];
        //UserProduct.GetProductsByUserId().done(function (dataProduct) {
        //	if (dataProduct.success) {
        //		product = dataProduct.result;

        //		if (tempModalProducts.length == undefined) {
        //			tempModalProducts = dataProduct.result;
        //		}

        //		if (dataProduct.result.length > 0) {
        //			$.each(dataProduct.result, function (key, value) {
        //				if (value.Assign == true) {
        //					glbUser.UniqueUsersProduct.push(this);
        //				}
        //			});
        //		}
        //		UniqueUser.LoadSubTitles();
        //	}
        //	else {
        //		$.UifNotify('show', { 'type': 'info', 'message': dataProduct.result, 'autoclose': true });
        //	}
        //});

        $('#chkBlocked').prop('checked', false);
        if (glbUser.LockDate != null) {
            $('#chkBlocked').prop('checked', true);
        }

        $('#chkDisabled').prop('checked', false);
        if (glbUser.DisableDate != null) {
            $('#chkDisabled').prop('checked', true);
        }

        if (userData.Profiles != null && userData.Profiles.length > 0) {
            $("#selectProfile").UifSelect("setSelected", userData.Profiles[0].Id);
        }

        $("#Password").attr("placeholder", "********");
        $("#Confirmation").attr("placeholder", "********");


        if (userData.UniqueUsersLogin != null) {
            $("#InputExpirationPasswordDate").val(FormatDate(userData.UniqueUsersLogin.ExpirationDate));
            $('#chkUpdatePassword').prop("checked", userData.UniqueUsersLogin.MustChangePassword);
        }

        UniqueUser.CalculateDaysDifference(userData.UniqueUsersLogin.ExpirationDate, moment().format("DD/MM/YYYY"));

        ClearValidation("#formUser");
        UniqueUser.LoadSubTitles(userData.UniqueUsersLogin.ExpirationDate, moment().format("DD/MM/YYYY"));
        //AlliedSalePoints.GetUniqueUserSalePointText(glbUser.individualId, glbUser.UserId);
    }


    static CalculateDaysDifference(startDate, endDate) {
        if (startDate != null && endDate != null) {
            if (startDate.indexOf("Date") > -1) {
                startDate = FormatDate(startDate);
            }
            if (endDate.indexOf("Date") > -1) {
                endDate = FormatDate(endDate);
            }

            var startDatePart = startDate.split('/');
            var endDatePart = endDate.split('/');

            var compareStartDate = new Date(startDatePart[2], startDatePart[1] - 1, startDatePart[0]);
            var compareEndDate = new Date(endDatePart[2], endDatePart[1] - 1, endDatePart[0]);
            var dias = moment(compareStartDate).diff(compareEndDate, 'day')
            $("#ExpirationDays").val(dias);
    
        }
    }

    static CalculateVigencyDateByAddDays(addDays) {
        var expirationDate = moment().format("DD/MM/YYYY");        
        var expirationDatePart = expirationDate.split('/');

        expirationDate = new Date(expirationDatePart[2], expirationDatePart[1] - 1, expirationDatePart[0]);      

        $('#InputExpirationPasswordDate').UifDatepicker('setValue', moment(expirationDate).add(addDays, 'd').format("DD/MM/YYYY"));
    }

    //Ocultar paneles
    static hidePanelsUser(Menu) {
    switch (Menu) {
        case MenuType.UniqueUser:
            break;
        case MenuType.Hierarchy:
            $("#modalHierarchy").UifModal('hide');
            break;
        case MenuType.Branch:
            if (glbUser.SalesPoint != undefined && glbUser.SalesPoint.length < 0) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.UserWithOutBranch, 'autoclose': true })
            }
            else {
                $("#modalBranch").UifModal('hide');
            }
            break;
        //case MenuType.AlliedSalePoints:
        //    $("#modalAlliedSalePoints").UifModal('hide');
        //    break;
        case MenuType.Agent:
            $("#modalAgent").UifModal('hide');
            break;
        //case MenuType.PrefixUser:
        //    $("#modalPrefixUser").UifModal('hide');
        //    break;
        case MenuType.UserGroup:
            $("#modalGroup").UifModal('hide');
            break;
        default:
            break;
    }
}

    //Mostrar panel
    static showPanelsUser(Menu) {
    switch (Menu) {
        case MenuType.UniqueUser:
            break;
        case MenuType.Hierarchy:
            $("#modalHierarchy").UifModal('showLocal', AppResources.Hierarchies);
            break;
        case MenuType.Branch:
            $("#modalBranch").UifModal('showLocal', AppResources.BranchandSalePoint);
            break;
        //case MenuType.AlliedSalePoints:
        //	$("#modalAlliedSalePoints").UifModal('showLocal', AppResources.AlliedSalePoints);
        //	break;
        case MenuType.Agent:
            $("#modalAgent").UifModal('showLocal', AppResources.WorkWithIntermediary);
            break;
        //case MenuType.UserProduct:
        //	$("#modalProduct").UifModal('showLocal', AppResources.TitleProducts);
        //	break;
        //case MenuType.PrefixUser:
        //    $("#modalPrefixUser").UifModal('showLocal', AppResources.CommercialBouquet);
        //    break;
        case MenuType.UserGroup:
            $("#modalGroup").UifModal('showLocal', AppResources.UserGroup);
            break;
        case MenuType.Permissions:
            $("#modalPermissions").UifModal('showLocal', AppResources.Permisos);
            break;
        default:
            break;
    }
}

    static getUserModel() {
    glbUser.AccountName = $("#LoginName").val();
    var changePassword = 0;

    if ($('#chkUpdatePassword').is(':checked')) {
        changePassword = 1;
    }

    if ($("#NameSurname").data("Object") != null) {
        glbUser.PersonId = $("#NameSurname").data("Object").IndividualId;
    }
    else {
        glbUser.PersonId = null;
    }

    glbUser.Profiles = [];
    glbUser.Profiles.push({ Id: $("#selectProfile").UifSelect("getSelected") });

    if ($('#chkBlocked').is(':checked')) {
        glbUser.LockDate = currentDate;
    }
    else {
        glbUser.LockDate = null;
    }

    if ($('#chkDisabled').is(':checked')) {
        glbUser.DisableDate = UniqueUser.FormatDate($("#InputDisabledDate").val());
    }
    else {
        glbUser.DisableDate = null;
    }

    if (glbAllySalePoints.length > 0) {
        glbUser.CptUniqueUserSalePointAlliance = glbAllySalePoints;
    }

    glbUser.ExpirationDate = UniqueUser.FormatDate($("#inputDateExpirationUser").val());
    glbUser.CreationDate = UniqueUser.FormatDate($("#inputCreationDate").val());
    glbUser.UniqueUsersLogin = { ExpirationDate: UniqueUser.FormatDate($("#InputExpirationPasswordDate").val()), Password: $("#Password").val(), MustChangePassword: changePassword };
    //glbUser.UniqueUsersProduct = [];

    //if (product.length > 0) {
    //	$.each(product, function (key, value) {
    //		if (value.Assign == true) {
    //			glbUser.UniqueUsersProduct.push(this);
    //		}
    //	});
    //}

    return glbUser;
}

    static FormatDate(date) {
    if (date != null) {
        date = date.toString();

        if (date.length > 10) {
            var dateString = date.substr(0, 10);
            dateString = UniqueUser.convertDateFormat(dateString)
            var currentTime = new Date(dateString);
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var dayFormat = day < 10 ? "0" + day : "" + day;
            var monthFormat = month < 10 ? "0" + month : "" + month;
            var yearFormat = year < 1000 ? "0" + year : "" + year;
            var yearFormat = year < 100 ? "00" + year : "" + year;
            var yearFormat = year < 10 ? "000" + year : "" + year;
            var date = dayFormat + "/" + monthFormat + "/" + yearFormat;
        } 
    }
    return date;
}

    static convertDateFormat(dateString) {
    var info = dateString.split('/');
    var date = info[2] + '/' + info[1] + '/' + info[0];
    return date;
}
}
