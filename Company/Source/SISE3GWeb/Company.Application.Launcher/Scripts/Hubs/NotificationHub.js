class NotificationHub {
    get Protocols() {
        return ["webSockets", "serverSentEvents", "longPolling"];
    }

    constructor() {
        this.Hub = $.connection.notificationHub;

        $.connection.hub.logging = true;
        $.connection.hub.start({ transport: this.Protocols }).done(this.Start.bind(this)).fail(this.FailConnected);
        $.connection.hub.disconnected(() => {
            console.warn(`Disconnected`);
        });
        $.connection.hub.reconnected(() => {
            console.warn(`Reconnected, connection ID=${$.connection.hub.id}`);
        });
        $.connection.hub.reconnecting(() => {
            console.warn("Reconnecting...");
        });

        this.Hub.client.Notificate = Notifications.Notificate;
        //this.Hub.client.NotificateWorkFlow = Notifications.initNotificationWorkFlow();
    }

    Start() {
        console.warn(`Now connected, connection ID=${$.connection.hub.id}`);
    }

    FailConnected() {
        console.error("Could not Connect!");
    }
}

var notificationHub;
$(notificationHub = new NotificationHub());