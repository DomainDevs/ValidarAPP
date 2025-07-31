//VARIABLES
var branchId = 0;
var agentType = 0;
var agentCode = 0;

var agentData = {};
agentData.item = {};
agentData.item.id = 0;
agentData.item.label = "";
agentData.item.AgentTypeId = 0;
agentData.item.document = "";

var agentDoc = $('#CancellationNoticesAgentDocumentNumber').val();
var agentName = $('CancellationNoticesAgentName').val();

function validateDays() {
    $("#alertCancellationNotices").UifAlert('hide');
    if ($("#daysFrom").val() != "" &&
               $("#daysTo").val() != "") {
        if (parseInt($("#daysFrom").val()) > parseInt($("#daysTo ").val())) {
            $("#alertCancellationNotices").UifAlert('show', Resources.WarningInvalidLogon, "danger");
            $("#daysTo ").val("");
        }
    }
}

function SetDownloadLink(pathFile) {
    $("#alertCancellationNotices").UifAlert('hide');
    var result = '@Url.Action("Download", new { controller = "CancellationNotices", pathFile = "param_name" })';
    result = result.replace("param_name", pathFile);
    $("#Form1").show();
    $("#dynamicLink").attr("href", result);

}

function ShowProgressBar() {
    var number = 0;
    var progress = setInterval(function () {
        var $bar = $('.progress-bar');

        if ($bar.width() >= 1168) {
            clearInterval(progress);
        } else {
            $bar.width($bar.width() + 117);
            number += 10;
        }

        $bar.text(number + "%");
    }, 800);
}

$('CancellationNoticesAgentName').attr("disabled", "disabled");
$('#CancellationNoticesAgentDocumentNumber').attr("disabled", "disabled");
$("#Form1").hide();
$("#progressBar").hide();
//$("#alertCancellationNotices").UifAlert('hide');

$("#CancellationNoticesAgentDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#CancellationNoticesAgentDocumentNumber').val(agentDoc);
    }, 50);
});

$("CancellationNoticesAgentName").on('blur', function (event) {
    setTimeout(function () {
        $('CancellationNoticesAgentName').val(agentName);
    }, 50);
});

$('#CancellationNoticesAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alertCancellationNotices").UifAlert('hide');
    $("#CancellationNoticesAgentDocumentNumber").val(selectedItem.DocumentNumber);
    $("CancellationNoticesAgentName").val(selectedItem.DocumentNumberName);
    agentType = selectedItem.AgentTypeId;
    agentCode = selectedItem.AgentId;
    agentDoc = selectedItem.DocumentNumber;
    agentName = selectedItem.DocumentNumberName;
});


$('CancellationNoticesAgentName').on('itemSelected', function (event, selectedItem) {
    $("#alertCancellationNotices").UifAlert('hide');
    $("#CancellationNoticesAgentDocumentNumber").val(selectedItem.DocumentNumber);
    $("CancellationNoticesAgentName").val(selectedItem.DocumentNumberName);
    agentCode = selectedItem.AgentId;
    agentType = selectedItem.AgentTypeId;
    agentDoc = selectedItem.DocumentNumber;
    agentName = selectedItem.DocumentNumberName;
});

//RADIOS
$('input[name=oneIntermediary]').change(function () {
    if ($(this).val() == 0) {
        $('CancellationNoticesAgentName').attr("disabled", "disabled");
        $('#CancellationNoticesAgentDocumentNumber').attr("disabled", "disabled");

        agentData.item.id = 0;
        agentData.item.label = "";
        agentData.item.AgentTypeId = 0;
        agentData.item.document = "";
        $('CancellationNoticesAgentName').val("");
        $('#CancellationNoticesAgentDocumentNumber').val("");

    } else if ($(this).val() == 1) {
        $('CancellationNoticesAgentName').attr("disabled", false);
        $('#CancellationNoticesAgentDocumentNumber').attr("disabled", false);
    }
});

//Días desde
$("#daysFrom").on('blur', function (event) {
    validateDays();
});

//Días Hasta
$("#daysTo").on('blur', function (event) {
    validateDays();
});

$('#CleanCancellationNotices').click(function () {
    $("#allIntermediaries").attr('checked', 'checked');
    $("#cancellationDate").val("");
    $("#effectiveDate").val("");
    $("#Form1").hide();
    $("#daysFrom").val("");
    $("#daysTo").val("");
    $("#CancellationNoticesAgentDocumentNumber").val("");
    $("CancellationNoticesAgentName").val("");
    $('CancellationNoticesAgentName').attr("disabled", "disabled");
    $('#CancellationNoticesAgentDocumentNumber').attr("disabled", "disabled");
    $("#progressBar").hide();
    $("#alertCancellationNotices").UifAlert('hide');
});


$('#ReportCancellationNotices').click(function () {
    $("#alertCancellationNotices").UifAlert('hide');
    var daysFrom = $("#daysFrom").val() == "" ? 0 : $("#daysFrom").val();
    var daysTo = $("#daysTo").val() == "" ? 0 : $("#daysTo").val();

    $("#CancellationNotices").validate();

    if ($("#CancellationNotices").valid()) {
        ShowProgressBar();
        $("#progressBar").show();

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "CancellationNotices/LoadCancellationReport",
            data: JSON.stringify(
                {
                    "cancellationDate": $("#cancellationDate").val(),
                    "effectiveDate": $("#effectiveDate").val(),
                    "daysFrom": daysFrom,
                    "daysTo": daysTo,
                    "agentCode": agentCode,
                    "agentType": agentType
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "0") {
                    $("#alertCancellationNotices").UifAlert('show', Resources.ThereIsNoData, "success");
                } else {
                    $("#alertCancellationNotices").UifAlert('hide');
                    SetDownloadLink(data);
                    $("#progressBar").hide();

                }
            }
        });
    }
});