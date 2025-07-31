class NotificationsRequest {

    static GetParameters() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "Notification/Notification/GetAuthorizationParametres"
        });
    }

    static GetNotification() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "Notification/Notification/GetNotification"
        });
    }

    static GetNotificationCount() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "Notification/Notification/GetNotificationCount"
        });
    }

    static UpdateAllNotificationDisabledByUser() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "Notification/Notification/UpdateAllNotificationDisabledByUser"
        });
    }

    static UpdateNotification(notificationUser) {
        return $.ajax({
            type: "POST",
            data: { "notificationUser": notificationUser },
            url: rootPath + "Notification/Notification/UpdateNotification"
        });
    }
    static UpdateNotificationParameter(id) {
        return $.ajax({
            type: "POST",
            data: { "notificationId": id },
            url: rootPath + "Notification/Notification/UpdateNotificationParameter"
        });
    }
}

class Notifications extends Uif2.Page {
    get DropDown() {
        return uif2.dropDown({
            source: rootPath + "Notification/Notification/Index",
            element: "#notificate1",
            direction: "bottom",
            width: 500,
            height: 375,
            loadedCallback: () => {
                this.initListView();
            }
        });
    }

    getInitialState() {
        this.dropDown = this.DropDown;
        this.initListView();
        //this.initNotificationWorkFlow();
        NotificationsRequest.GetNotificationCount().done(data => {
            if (data.success) {
                $("#countNotificate1").text(data.result);
            }
        });

        Notifications.eventNotificationWorkFlow();
    }

    bindEvents() {
        $("#notificate1").on("click", this.onClickNotificate.bind(this, true));
    }

    initListView() {
        $("#lvNotification").UifListView({
            sourceData: null,
            title: "Notificaciones",
            displayTemplate: "#template-notification",
            selectionType: "single",
            height: 300,
            delete: true,
            customDelete: true
        });
        $("#lvNotification").on("itemSelected", this.onSelectNotification);
        $("#lvNotification").on("rowDelete", this.onDeleteNotification);
        $("#btnDeleteAllNotification").click(this.onDeleteAllNotification);

    }

    static eventNotificationWorkFlow() {
        NotificationsRequest.GetParameters().done(data => {
            if (data.success) {
                if (data.result.length > 1) {
                    $("#countNotificate2").text(data.result[0]);
                    Notifications.initNotificationWorkFlow(data.result);
                } else {                    
                    $("#countNotificate2").text(data.result[0]);
                    $("#ulNotificationWorkFlow").html("<li>" +
                        "<a class= 'process' id ='btnSendNotification' href = '#' >" +
                        "<div>" +
                        "<div id='lblPendientesEntrega' class='process-name'>" + count[0].toString() + " Pólizas| Pendientes por entrega </div>" +
                        "</div>" +
                        " </a >" +
                        "<a class='delete' href='#'>" +
                        "<i class='fa fa-times'></i>" +
                        "</a>" +
                        "</li >");
                    $("#ulNotificationWorkFlow").on("itemSelected", $.UifNotify('show', { 'type': 'info', 'message': "El usuario no tiene permiso al modulo", 'autoclose': true }));
                }
            } else {
                    $("#countNotificate2").text("0");
                    $("#ulNotificationWorkFlow").is(":hidden");
                    $("#ulNotificationWorkFlow").attr("style", "visibility: hidden");               
            }
        });
    }

    static initNotificationWorkFlow(count) {
        if (count[1] == null) { count[1] = 0; }        
        $("#ulNotificationWorkFlow").html("<li>" +
            "<a class= 'process' id ='btnSendNotification' href = '" + count[1] .toString()+"' >" +
            "<div>" +
            "<div id='lblPendientesEntrega' class='process-name'>" + count[0].toString() +" Pólizas| Pendientes por entrega </div>" +
            "</div>" +
            " </a >" +
            "<a class='delete' href='#'>" +
            "<i class='fa fa-times'></i>" +
            "</a>" +
            "</li >");
        $("#ulNotificationWorkFlow").on("itemSelected", Notifications.onSelectPendingDelivery);
    }

   

    onSelectNotification(e, item) {
        if (item.NotificationType.Url !== undefined && item.NotificationType.Url !== null) {
            if (item.Parameters !== undefined && item.Parameters !== null) {
                const keyNames = Object.keys(item.Parameters);
                keyNames.forEach((key, index) => {
                    keyNames[index] = `${key}=${item.Parameters[key]}`;
                });

                if (item.NotificationType.Url.indexOf("?") !== -1) {
                    item.NotificationType.Url = `${item.NotificationType.Url}&${keyNames.join("&")}`;
                } else {
                    item.NotificationType.Url = `${item.NotificationType.Url}?${keyNames.join("&")}`;
                }
                if (item.NotificationType.Id == 4) {
                    item.NotificationType.Url += "&notificationId=" + item.Id
                }
                window.location = rootPath + item.NotificationType.Url.substr(1);
                item.Enabled = false;
                NotificationsRequest.UpdateNotification(item)
                    .done((result) => {
                        if (result.success) {
                            $("#lvNotification").UifListView("deleteItem", index);
                            Notifications.DeleteNotificate();
                        }
                    });
            }
        }
    }
    onClickNotificate(show) {
        NotificationsRequest.GetNotificationCount().done(data => {
            if (data.success) {
                $("#countNotificate1").text(data.result);
            }
        });
        NotificationsRequest.GetNotification().done((data) => {
            if (data.success) {
                $("#lvNotification").UifListView("clear");

                if (show === true) {
                    this.dropDown.show();
                }

                if (data.result.length !== 0) {
                    $("#countNotificate1").show();
                } else {
                    $("#countNotificate1").hide();
                }

                data.result.forEach((item) => {
                    item.DateStr = FormatDate(item.CreateDate);
                    $("#lvNotification").UifListView("addItem", item);
                });
            }
        });
    }

    onDeleteAllNotification() {
        NotificationsRequest.UpdateAllNotificationDisabledByUser().done((data) => {
            if (data.success) {
                $("#lvNotification").UifListView("clear");

                if (data.result.length !== 0) {
                    $("#countNotificate1").show();
                } else {
                    $("#countNotificate1").hide();
                }

                data.result.forEach((item) => {
                    item.DateStr = FormatDate(item.CreateDate);
                    $("#lvNotification").UifListView("addItem", item);
                });
            }
        });
    }


    onDeleteNotification(event, data) {
        const index = $("#lvNotification").UifListView("findIndex", (x) => { return x.Id === data.Id });
        data.Enabled = false;
        NotificationsRequest.UpdateNotification(data)
            .done((result) => {
                if (result.success) {
                    $("#lvNotification").UifListView("deleteItem", index);
                    Notifications.DeleteNotificate();
                }
            });
    }

    static Notificate(notification) {

        if (notification.NotificationType.Type == 10) {
            Notifications.eventNotificationWorkFlow();
        } else {
            const isHidden = $("#countNotificate1").is(":hidden");
            if (isHidden) {
                $("#countNotificate1").show();
                $("#countNotificate1").text("1");
            } else {
                const num = parseInt($("#countNotificate1").text());
                $("#countNotificate1").text(num + 1);
            }
            $("#lvNotification").UifListView("addItem", notification);
        }
        
    }
    static DeleteNotificate() {
        const count = $("#countNotificate1").text();

        if (count === "1") {
            $("#countNotificate1").hide();
        } else {
            const num = parseInt(count);
            $("#countNotificate1").text(num - 1);
        }
    }
}

$(new Notifications());
