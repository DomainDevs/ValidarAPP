var ConsortiumMembers = [];
var ConsortiumMembersRowId = -1;
var ConsortiumMembersId = 0;
var ConsortiumInsuredCode = 0;
var heightListViewConsortium = 256;
var identyCompany = 0;
var identyPErson = 0;
var ConsortiumMembersNew = [];

class ConsortiumMember extends Uif2.Page {
    getInitialState() {
        this.InitializeConsortiumMembers();
        //ConsortiumMember.OnChecked();
    }

    bindEvents() {
        $("#btnAddConsortiumMembers").click(this.AddConsortiumMembers);
        $("#btnClearConsortiumMembers").click(ConsortiumMember.clearConsortiumMembers);
        //$("#btnAcceptTypePartnership").click(function (event, selectedItem) {
        //    $("#modalConsortiumMembers").UifModal('hide');
        //});
        $("#btnAcceptTypePartnership").click(this.AcceptTypePartnership);
        $("#inputConsortiumMembersId").on('buttonClick', this.SearchConsortiumMembersId);
        $('#listConsortiumMembers').on('rowEdit', this.ConsortiumMembersEdit);
        $("#chkDisabledConsortiumMembers").on("change", this.CheckDisabled);
    }

    InitializeConsortiumMembers() {
        $("#inputConsortiumMembersId").ValidatorKey(ValidatorType.Onlylettersandnumbers, ValidatorType.Onlylettersandnumbers, 1);
    }

    AddConsortiumMembers() {
        if (ConsortiumMember.ValidateConsortiumMembers()) {
            ConsortiumMember.CreateConsortiumMembers();
            ConsortiumMember.clearConsortiumMembers();
        }
    }

    static clearConsortiumMembers() {
        ConsortiumMembersRowId = -1;
        ConsortiumMembersId = 0;
        $("#InputPercentConsortium").val("");
        $("#inputConsortiumMembersId").val("");
        $("#chkPrincipalConsortiumMembers").prop("checked", false);
        $("#chkPrincipalConsortiumMembers").prop("disabled", false);
        $("#inputConsortiumMembersId").data("Object", null);
        $("#inputConsortiumMembersId").prop('disabled', false);
        $("#chkDisabledConsortiumMembers").prop("checked", false);
        $('#dateConsortiumMembers').UifDatepicker('clear');
        $("#dateConsortiumMembers").prop('disabled', true);
    }


    AcceptTypePartnership() {
        if (individualId != undefined && individualId != 0 && ConsortiumMember.ValidateConsortiumMembersMain() && ConsortiumMember.ValidateConsortiumMembersPercentageTotal()) {
            ConsortiumMember.SaveConsortiumMembers();
        } else {
            $("#modalConsortiumMembers").UifModal('hide');
        }
    }

    SearchConsortiumMembersId() {
        if ($("#inputConsortiumMembersId").val().trim().length > 0) {
            ConsortiumMember.GetHoldersByDescription($("#inputConsortiumMembersId").val().trim(), InsuredSearchType.DocumentNumber, InsuredSearchType.IndividualId);
        }
    }

    ConsortiumMembersEdit(event, data, index) {

        ConsortiumMembersRowId = index;
        ConsortiumMember.EditConsortiumMembers(data, index);
        $("#inputConsortiumMembersId").prop('disabled', true);
    }

    CheckDisabled() {
        if ($("#chkDisabledConsortiumMembers").is(':checked')) {
            $("#dateConsortiumMembers").val(FormatFullDate(moment().format('YYYY/MM/DD')));
            $("#chkPrincipalConsortiumMembers").prop("disabled", true);
            $("#chkPrincipalConsortiumMembers").prop("checked", false);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ConsortiumDisabled, 'autoclose': true });
        } else {
            $('#dateConsortiumMembers').UifDatepicker('clear');
            $("#chkPrincipalConsortiumMembers").prop("disabled", false);
        }
    }

    static CleanObjectConsortiumMembers() {
        ConsortiumMembers = [];
        $("#listConsortiumMembers").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#ConsortiumMembersTemplate", height: heightListViewConsortium, deleteCallback: ConsortiumMember.deleteConsortium });
    }

    //Creacion
    static CreateConsortiumMembers() {

        ConsortiumMembers = ConsortiumMember.CreateConsortiumMembersModel();
        ConsortiumMembers.IsModify = true;
        if (ConsortiumMembersRowId == -1) {
            $("#listConsortiumMembers").UifListView("addItem", ConsortiumMembers);
        }
        else {
            $("#listConsortiumMembers").UifListView("editItem", ConsortiumMembersRowId, ConsortiumMembers);
        }
    }

    static CreateConsortiumMembersModel() {
        var datos = $("#inputConsortiumMembersId").data("Object");
        ConsortiumMembers = {
            InsuredCode: ConsortiumInsuredCode != 0 ? ConsortiumInsuredCode : insuredTmp, //0,
            IndividualId: datos.IndividualId || $("#inputConsortiumMembersId").data("IndividualId"),
            //ConsortiumId: ConsortiumMembersId,
            ConsortiumId: Person.ConsortiumMembers[ConsortiumMembersRowId] != null && Person.ConsortiumMembers[ConsortiumMembersRowId].ConsortiumId != 0 ? Person.ConsortiumMembers[ConsortiumMembersRowId].ConsortiumId : 0,
            ParticipationRate: $("#InputPercentConsortium").val(),
            IsMain: $('#chkPrincipalConsortiumMembers').is(':checked'),
            StartDate: DateNowPerson,
            Enabled: !($('#chkDisabledConsortiumMembers').is(':checked')),
            PersonIdentificationNumber: $("#inputConsortiumMembersId").data("PersonIdentificationNumber") || datos.IdentificationDocument.Number,
            FullName: $("#inputConsortiumMembersId").val()

        };

        return ConsortiumMembers;
    }

    static SaveConsortiumMembers() {
        ConsortiumMembersNew = $("#listConsortiumMembers").UifListView('getData');
        var consortiumMemberFilter = ConsortiumMembersNew.filter(function (item) {
            return item.IsModify
        });
        if (ConsortiumMembersNew != undefined && ConsortiumMembersNew != null) {
            var ConsortiumEventDTO = {};
            ConsortiumEventDTO.ConsortiumEventEventType = EventConsortium.CREATE_CONSORTIUM;
            ConsortiumEventDTO.IndividualConsortiumID = parseInt($("#lblCompanyCode").val());
            ConsortiumEventDTO.IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
            ConsortiumEventDTO.consortiumDTO = {};
            ConsortiumEventDTO.consortiumDTO.IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
            ConsortiumEventDTO.consortiumDTO.ConsotiumId = company.Id;
            ConsortiumEventDTO.consortiumDTO.ConsortiumName = $('#inputCompanyTradeName').val();
            ConsortiumEventDTO.consortiumDTO.UpdateDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
            ConsortiumEventDTO.consortiumDTO.AssociationType = $('#selectTypePartnership').val();
            ConsortiumEventDTO.consortiumDTO.AssociationTypeDesc = $('#selectCompanyDocumentType').UifSelect("getSelectedText");

            var ConsortiumEventDTOs = [];
            ConsortiumMembersNew.forEach(function (item, index) {
                ConsortiumEventDTOs[index] = {};
                ConsortiumMembersNew[index].ParticipationRate = parseFloat(item.ParticipationRate);
                ConsortiumEventDTOs[index].ConsortiumEventEventType = EventConsortium.INICIAL_EVENT;
                ConsortiumEventDTOs[index].IndividualConsortiumID = parseInt($("#lblCompanyCode").val());
                ConsortiumEventDTOs[index].IndividualId = item.IndividualId;
                ConsortiumEventDTOs[index].IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO = {};
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.IndividualConsortiumId = company.Id;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.IndividualPartnerId = item.IndividualId;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.PartnerName = item.FullName;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.ConsortiumId = item.InsuredCode;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.InitDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.EndDate = null;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.ParticipationRate = parseFloat(item.ParticipationRate);
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.Enabled = item.Enabled;
                if (ConsortiumEventParticipant != undefined) {
                    ConsortiumEventParticipant.forEach(function (item1, index1) {
                        if (ConsortiumEventDTOs[index].IndividualId == item1.IndividualId) {
                            ConsortiumEventDTOs[index].ConsortiumEventEventType = EventConsortium.MODIFY_INDIVIDUAL_TO_CONSORTIUM;
                        }
                    });
                }
                if (item.Enabled == false) {
                    ConsortiumEventDTOs[index].ConsortiumEventEventType = EventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM;
                    ConsortiumEventDTOs[index].ConsortiumpartnersDTO.ParticipationRate = 0;
                    ConsortiumMembersNew[index].ParticipationRate = 0;
                }
            });
            ConsortiumMembersNew[0].ConsortiumEventDTO = ConsortiumEventDTO;
            ConsortiumMembersNew[0].ConsortiumEventDTOs = ConsortiumEventDTOs;
        }

        var individualId = $('#lblCompanyCode').val();
        if (individualId == "" || individualId == undefined) {
            Person.ConsortiumMembers = consortiumMemberFilter;
            $('#btnCancelTypePartnership').click();
        }
        else if (individualId > 0 && ConsortiumMembersNew != null) {
            lockScreen();
            ConsortiumRequest.CreateConsortium(ConsortiumMembersNew, individualId).done(function (data) {
                if (data.success) {
                    if (data.result) {
                        let countAuthorization = 0;

                        var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result[0].InfringementPolicies, true);
                        if (data.result[0].InfringementPolicies != undefined) {
                            countAuthorization = data.result[0].InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                        }

                        if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                            if (countAuthorization > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result[0].InfringementPolicies, data.result[0].OperationId, FunctionType.PersonConsortiates);
                            }
                        }
                        $("#modalConsortiumMembers").UifModal('hide');
                        Person.ConsortiumMembers = $("#listConsortiumMembers").UifListView('getData');
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ShowSaveData, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorCreateConsortium, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());
        }
    }


    static LoadConsortium(individual) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetConsortiumIndividualId',
            data: JSON.stringify({ individual: individual }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            //success: function (data) { ConsortiumMember.LoadConsortiumMembers(data.result) }
        }).done(function (response) {
            if (response.success === true) {
                $.UifProgress('close');

                ConsortiumMember.LoadConsortiumMembers(response.result);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        }).fail(function (data) {
            $.UifNotify('show', { 'type': 'info', 'message': data.Message, 'autoclose': true });
            $.UifProgress('close');
        });
    }
    //Seccion Load
    static LoadConsortiumMembers(data) {
        Person.ConsortiumMembers = data;
        $("#listConsortiumMembers").UifListView({ sourceData: data, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#ConsortiumMembersTemplate", height: heightListViewConsortium, deleteCallback: ConsortiumMember.deleteConsortium });
    }

    static GetHoldersByDescription(description) {
        var number = parseInt(description, 10);
        if (!isNaN(number) || description.length > 2) {
            InsuredRequest.GetInsuredsByDescription(description, 1).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageConsortiumNoExists, 'autoclose': false });
                    }
                    if (data.result.length == 1) {
                        $("#inputConsortiumMembersId").data("Object", data.result[0]);
                        $("#inputConsortiumMembersId").val(data.result[0].Name + ' ' + (data.result[0].Surname ? data.result[0].Surname : "") + ' ' + ((data.result[0].SecondSurname ? data.result[0].SecondSurname : "")).trimEnd());
                    }
                    else if (data.result.length > 1) {
                        modalListType = ModalListTypePerson.Holder;
                        var dataList = { dataObject: [] };
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.dataObject.push({
                                Id: data.result[i].IndividualId,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                TypePerson: data.result[i].IndividualType
                            });
                        }
                        Persons.ShowModalListAll(dataList.dataObject);
                        $('#modalDialogListAll').UifModal('showLocal', AppResourcesPerson.MessageHolder);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
                }
            });
        }
    }

    static GetHoldersByIndividualId(individualId, type) {
        InsuredRequest.GetInsuredsByDescription(individualId, type).done(function (data) {
            if (data.success) {
                if (data.result.length == 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageConsortiumNoExists, 'autoclose': false });
                }
                if (data.result.length == 1) {
                    $("#inputConsortiumMembersId").data("Object", data.result[0]);
                    $("#inputConsortiumMembersId").val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                }
                else if (data.result.length > 1) {
                    modalListType = ModalListTypePerson.Holder;
                    var dataList = { dataObject: [] };
                    for (var i = 0; i < data.result.length; i++) {
                        dataList.dataObject.push({
                            Id: data.result[i].IndividualId,
                            Code: data.result[i].IdentificationDocument.Number,
                            Description: data.result[i].Name,
                            TypePerson: data.result[i].IndividualType
                        });
                    }
                    Persons.ShowModalListAll(dataList.dataObject);
                    $('#modalDialogListAll').UifModal('showLocal', AppResourcesPerson.MessageHolder);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
            }
        });

    }
    //Seccion Edit
    static EditConsortiumMembers(data, index) {

        ConsortiumInsuredCode = data.InsuredCode;
        identyPErson = data.PersonIdentificationNumber;
        identyCompany = data.CompanyIdentifationNumber;


        $("#inputConsortiumMembersId").val(data.FullName);
        data.Object = { IndividualId: data.IndividualId };
        $("#inputConsortiumMembersId").data(data);
        $('#chkPrincipalConsortiumMembers').prop("checked", data.IsMain);
        $("#InputPercentConsortium").val(data.ParticipationRate);
        $('#chkDisabledConsortiumMembers').prop("checked", !(data.Enabled));

        if ($("#chkDisabledConsortiumMembers").is(':checked')) {
            $('#dateConsortiumMembers').UifDatepicker('setValue', data.StartDate);
        } else {
            $('#dateConsortiumMembers').UifDatepicker('clear');
        }
    }

    //Seccion Validaciones
    static ValidateConsortiumMembers() {
        var msj = "";
        if ($("#inputConsortiumMembersId").data("Object") == null || $("#inputConsortiumMembersId").data("Object") == "") {
            msj = msj + AppResourcesPerson.MessageConsortiumMembersEmpty + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msj, 'autoclose': true })
            return false;
        } else if (ConsortiumMember.PrincipalConsortiumMembers()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorConsortiumMain, 'autoclose': true });
            return false;
        }
        else if (ConsortiumMember.DuplicateConsortiumMembers($("#inputConsortiumMembersId").val())) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageConsortiumDuplicate, 'autoclose': true });
            return false;
        }
        else if (!ConsortiumMember.ValidateConsortiumMembersPercentage()) {
            return false;
        }
        return true;
    }

    static ValidateConsortiumMembersPercentage() {
        var percentageTotal = 0;
        var listConsortiumMembers = $("#listConsortiumMembers").UifListView('getData');
        if ($("#InputPercentConsortium").val() != "") {
            if ($("#InputPercentConsortium").val() > 0 && $("#InputPercentConsortium").val() <= 100) {
                if (listConsortiumMembers.length > 0) {
                    $.each(listConsortiumMembers, function (key, value) {
                        if (this.IndividualId != $("#inputConsortiumMembersId").data("Object").IndividualId) {
                            if (ConsortiumMembersRowId != key)
                                if (this.Enabled == true) {
                                    percentageTotal = percentageTotal + parseFloat(this.ParticipationRate);
                                }
                        }
                    });
                }
                if (!$("#chkDisabledConsortiumMembers").is(':checked')) {
                    percentageTotal = parseFloat(percentageTotal, 10) + parseFloat($("#InputPercentConsortium").val(), 10);
                }

                if (percentageTotal > 100 || $("#InputPercentConsortium").val() > 100) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPercentage });
                    return false;
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPercentageHigher });
                return false;
            }
        } else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPercentageEmpty });
            return false;
        }

        return true
    }

    static DuplicateConsortiumMembers(individualId) {
        var duplicate = false;
        $.each($("#listConsortiumMembers").UifListView('getData'), function (key, value) {
            if (this.IndividualId == individualId && key != ConsortiumMembersRowId) {
                duplicate = true;
                return false;
            }
        });
        return duplicate;
    }

    static PrincipalConsortiumMembers() {
        var duplicate = false;
        var IsMain = $("#chkPrincipalConsortiumMembers").is(":checked");
        if (IsMain) {
            $.each($("#listConsortiumMembers").UifListView('getData'), function (i, item) {
                if (item.IsMain == IsMain && i != ConsortiumMembersRowId && item.Enabled == true) {
                    duplicate = true;
                    return false;
                }
            });
        }
        return duplicate;
    }

    static ValidateConsortiumMembersMain() {
        var validate = false;
        var countMain = 0;
        $.each($("#listConsortiumMembers").UifListView('getData'), function (i, item) {
            if (item.IsMain == 1 && item.Enabled == true) {
                countMain++;
            }
        });
        if (countMain != 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageConsortiumMain, 'autoclose': true })
        }
        else {
            validate = true;
        }
        return validate;
    }

    static ValidateConsortiumMembersPercentageTotal() {
        var percentageTotal = 0;
        if ($("#listConsortiumMembers").UifListView('getData').length > 0) {
            $.each($("#listConsortiumMembers").UifListView('getData'), function (key, value) {
                if (this.Enabled == true) {
                    percentageTotal = percentageTotal + parseFloat(this.ParticipationRate);
                }

            });

        }
        if (percentageTotal == 100) {
            return true;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorPercentageTotal, 'autoclose': true })
            return false;
        }
    }

    static deleteConsortium(deferred, data) {
        if (data.IndividualId > 0) {
            var ConsortiumParticipant = data;
            var IndividualId = ConsortiumParticipant.IndividualId;
            ConsortiumRequest.DeleteConsortium(data).done(function (data) {
                if (data.success) {
                    ConsortiumRequest.GetConsortiumEventByIndividualId(IndividualId).done(function (data) {
                        if (data.success) {
                            var ConsortiumEventDTOs = [];
                            if (data.result) {
                                data.result.forEach(function (item, index) {
                                    ConsortiumEventDTOs = {};
                                    ConsortiumEventDTOs.ConsortiumEventEventType = EventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM;
                                    ConsortiumEventDTOs.IndividualConsortiumID = company.Id;
                                    ConsortiumEventDTOs.IndividualId = data.result[0].IndividualId;
                                    ConsortiumEventDTOs.IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO = {};
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.IndividualConsortiumId = company.Id;
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.IndividualPartnerId = data.result[0].IndividualId;
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.PartnerName = ConsortiumParticipant.FullName;
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.ConsortiumId = data.result[0].IndividualConsortiumID;
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.InitDate = FormatFullDate(moment().format('MM/DD/YYYY H:mm'));
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.EndDate = null;
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.ParticipationRate = 0;
                                    ConsortiumEventDTOs.ConsortiumpartnersDTO.Enabled = false;
                                });
                                ConsortiumRequest.AssigendIndividualToConsotiumEvent([ConsortiumEventDTOs]).done(function (data) {
                                    if (data.success) {
                                        if (data.result) {
                                            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.DeleteConsortium, 'autoclose': true });
                                        }
                                    }
                                });

                            }
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageDeleteConsortium, 'autoclose': true });
                }
            });
            deferred.resolve();
        }

    }
}