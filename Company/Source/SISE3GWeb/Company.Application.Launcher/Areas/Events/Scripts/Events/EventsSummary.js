$.ajaxSetup({ async: false });
$(function () {
    disabledEventsSummaryDetails();
    $('#modalEventsSummary').UifModal();
    $('#modalEventsSummary').UifModal('showLocal', Resources.Language.TitleSolicitudeAuthorizationEvent);

    //inicializa el listView resumen de los eventros generados
    $("#lvwEventsSummary").UifListView(
        {
            source: rootPath + 'Events/Notification/GetEventsSForAutorize?IdUser=0&IdModule=' + $("#hdnIdModule").val() + '&IdSubModule=' + $("#hdnIdSubModule").val() + '&IdTemp=' + $("#hdnIdTemp").val() + "&Type=" + $("#hdnType").val(),
            customEdit: true,
            edit: true,
            displayTemplate: "#display-template-EventsSummary",
            height: 400
        });

    SearchUsersByEvent();

    /*accion al editar un evento*/
    $("#lvwEventsSummary").on('rowEdit', function (event, data) {
        $("#formEvensSummary input[id='ResultId']").val(data.ResultId);
        $("#formEvensSummary input[id='GroupEventId']").val(data.GroupEventId);
        $("#formEvensSummary input[id='EventId']").val(data.EventId);
        $("#formEvensSummary input[id='DelegationId']").val(data.DelegationId);
        $("#formEvensSummary TextArea[id='ReasonRequest']").val(data.ReasonRequest);
        LoadValuesToEvent(data);
        $("#panelEventsSummaryDetails").show();
        enableEventsSummaryDetails();
    });

    /*accion al cancelar la edicion de autorizacion*/
    $("#btnCancelEvent").on("click", function () {
        disabledEventsSummaryDetails();
    });

    /*envio del formulario (de forma local)*/
    $("#formEvensSummary").submit(function () {
        if ($("#formEvensSummary").valid()) {
            var ResultId = $("#formEvensSummary input[id='ResultId']").val();
            var ReasonRequest = $("#formEvensSummary TextArea[id='ReasonRequest']").val();
            var IdAuthorizer = $("#formEvensSummary select[id='IdAuthorizer']").val();
            var IdNotifier = $("#formEvensSummary select[id='IdNotifier']").val();

            var item = $("#" + ResultId);

            $(item).find("#IdAuthorizer").val(IdAuthorizer);
            $(item).find("#IdNotifier").val(IdNotifier);
            $(item).find("#ReasonRequest").val(ReasonRequest);

            disabledEventsSummaryDetails();
        }
        return false;
    });

    /*enviado de los eventos para solicitar la autorizacion*/
    $("#btnSaveEvents").on("click", function () {
        var emptyField = " ";
        var ListItems = [];
        var reasons = true;

        $(".EventItem").each(function (index, value) {

            ListItems.push({
                ResultId: $(value).attr("id"),
                ModuleId: $(value).find("#ModuleId").val(),
                SubModuleId: $(value).find("#SubModuleId").val(),
                GroupEventId: $(value).find("#GroupEventId").val(),
                EventId: $(value).find("#EventId").val(),
                DelegationId: $(value).find("#DelegationId").val(),
                IdAuthorizer: $(value).find("#IdAuthorizer").val(),
                IdNotifier: $(value).find("#IdNotifier").val(),
                ReasonRequest: $(value).find("#ReasonRequest").val(),
                IdTemp: $(value).find("#IdTemp").val(),
                Operation2Id: emptyField
            });

            if (reasons && $(value).children("#ReasonRequest").val() === "") {
                reasons = false;
            }
        });

        if (reasons) {
            AjaxRequest(rootPath + "Events/Notification/SaveAuthorizationEvent", "POST", { "delegation": ListItems}  ,
                function (result) {
                    var type = $("#hdnType").val();
                    $("#Script-Events").html("");
                    $(".modal-backdrop").remove();
                    switch (type) {
                        case "individual":
                        case "sarlaft":
                            $.UifDialog('alert', { 'message': result.result }, function () {
                                window.location = rootPath + "Home/index";
                            });
                            break;
                        case "collectiveGeneral":
                            $.UifDialog('alert', { 'message': result.result }, function () {
                                ObjectTemporal.RedirectCollective('Collective/Collective');
                            });
                            break;
                        case "collectiveLoadReady":
                            collectiveLoadReady();
                            break;
                        case "collectiveLoadSearch":
                            collectiveLoadSearch();
                            break;
                    }

                }, null, null);
        } else {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.NotAllEventsHaveRequest, 'autoclose': true });
        }
    });
})

/*busca los usuarios para el evento (autorizadores y notificadores)*/
function SearchUsersByEvent() {
    $(".EventItem").each(function (index, value) {
        var GroupEventId = $(value).find("#GroupEventId").val();
        var EventId = $(value).find("#EventId").val();
        var DelegationId = $(value).find("#DelegationId").val();

        var listUsers = GetUsers(EventId, GroupEventId, DelegationId);
        var userDefaul = 0;

        $.each(listUsers, function (index, item) {
            if (item.AuthorizedInd == true) {
                $(value).find("#IdAuthorizer").val(item.UserId);
            }
            if (item.NotificatedInd == true) {
                $(value).find("#IdNotifier").val(item.UserId);
            }
            if (item.NotificatedDefault == true) {
                userDefaul = item.UserId;
            }
            if (userDefaul != 0) {
                $(value).find("#IdNotifier").val(userDefaul);
            }
        });
    });
}

/*carga los valores para lso usuarios */
function LoadValuesToEvent(data) {
    var listUsers = GetUsers(data.EventId, data.GroupEventId, data.DelegationId);
    $("#formEvensSummary select[id='IdAuthorizer']").html("");
    $("#formEvensSummary select[id='IdNotifier']").html("");
    $.each(listUsers, function (index, value) {
        if (value.AuthorizedInd == true) {
            $("#formEvensSummary select[id='IdAuthorizer']").append($('<option>', {
                value: value.UserId,
                text: value.UserName
            }));
        }
        if (value.NotificatedInd == true) {
            $("#formEvensSummary select[id='IdNotifier']").append($('<option>', {
                value: value.UserId,
                text: value.UserName
            }));
        }
    });
    var item = $("#" + data.ResultId);
    $("#formEvensSummary select[id='IdAuthorizer']").val($(item).children("#IdAuthorizer").val());
    $("#formEvensSummary select[id='IdNotifier']").val($(item).children("#IdNotifier").val());
    $("#formEvensSummary TextArea[id='ReasonRequest']").val($(item).children("#ReasonRequest").val());
}

/*obtien los diferentes usuarios que pueden autorizar o notificar un evento*/
function GetUsers(EventId, GroupEventId, DelegationId) {
    var listUsers;
    AjaxRequest(rootPath + "Events/Notification/GetUsersByDelegation", "POST", { "IdEvent": EventId, "IdGroup": GroupEventId, "IdDelegation": DelegationId },
        function (result) {
            listUsers = result;
        }, null, null);
    return listUsers;
}

function disabledEventsSummaryDetails() {
    $("#IdAuthorizer").prop("disabled", true);
    $("#IdNotifier").prop("disabled", true);
    $("#ReasonRequest").prop("disabled", true);
    $("#IdAuthorizer").UifSelect();
    $("#IdNotifier").UifSelect();
    $("#ReasonRequest").val("");
}

function enableEventsSummaryDetails() {
    $("#IdAuthorizer").prop("disabled", false);
    $("#IdNotifier").prop("disabled", false);
    $("#ReasonRequest").prop("disabled", false);
}