var sessionTimeoutWarning;
var sessionTimeout;
var timeOnPageLoad = new Date();
var sessionWarningTimer;
var redirectToWelcomePageTimer;
(function () {
    $.when(GetSession(), GetSessionWarning())
        .done(function () {
            UpdateDateSession();
        })
        .fail(function () {
           
        });


})();
function GetSession() {
    var dfd = $.Deferred();
    $.post(rootPath + 'Layout/GetSession', function (data) {
        if (data.success) {
            if (data.result !== 0) {
                sessionTimeout = data.result;
            }
            dfd.resolve();
        }
        else {
            dfd.reject(sessionTimeoutWarning);
        }
    });
    return dfd;
}

function GetSessionWarning() {
    var dfd = $.Deferred();
    $.post(rootPath + 'Layout/GetSessionWarning', function (data) {
        if (data.success) {
            if (data.result !== 0) {
                sessionTimeoutWarning = data.result;
            }
            dfd.resolve();
        }
        else {
            dfd.reject(sessionTimeoutWarning);
        }

    });
    return dfd;
}

function UpdateDateSession() {
    $.post(rootPath + 'Account/UpdateDateSession', function (data) { });
}


//Session Warning 
function SessionWarning() {

    var minutesForExpiry = (parseInt(sessionTimeout) - parseInt(sessionTimeoutWarning));

    $.UifDialog('alert', { 'message': AppResources.MessageSessionExpire + minutesForExpiry + ' ' + AppResources.MessageExtendSession }, function (result) {
        if (result) {
            if (redirectToWelcomePageTimer !== null) {
                clearTimeout(redirectToWelcomePageTimer);
            }
            RefreshTime();
            UpdateDateSession();
        }
    });

    var currentTime = new Date();
    var timeForExpiry = timeOnPageLoad.setMinutes(timeOnPageLoad.getMinutes() + parseInt(sessionTimeout));

    if (Date.parse(currentTime) > timeForExpiry) {
        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSessionExpired, 'autoclose': true });
        window.location = rootPath + 'Account/LogOff';
    }
}

function RedirectToWelcomePage() {
    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSessionExpired, 'autoclose': true });
    window.location = rootPath + 'Account/LogOff';
}

$("body").on('click keypress', function () {
    RefreshTime();
});

function RefreshTime() {
    timeOnPageLoad = new Date();
    clearTimeout(sessionWarningTimer);
    clearTimeout(redirectToWelcomePageTimer);
    sessionWarningTimer = setTimeout('SessionWarning()', parseInt(sessionTimeoutWarning) * 60 * 1000);
    redirectToWelcomePageTimer = setTimeout('RedirectToWelcomePage()', parseInt(sessionTimeout) * 60 * 1000);
}