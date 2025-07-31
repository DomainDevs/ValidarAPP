//VARIABLES
var agentType = 0;
var agentCode = 0;
var partialBalance = 0;

var agentDoc = $('#CancelledAgentDocumentNumber').val();
var CancelledAgentName = $('#CancelledAgentName').val();

/*
**Valida días ingresados
*/
function validateDays() {
    //$("#alertCancelledPolicies").UifAlert('hide');
    if ($("#daysFrom").val() != "" &&
               $("#daysTo").val() != "") {
        if (parseInt($("#daysFrom").val()) > parseInt($("#daysTo ").val())) {
            $("#alertCancelledPolicies").UifAlert('show', Resources.WarningInvalidLogon, "danger");
            $("#daysTo ").val("");
        }
    }
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

//Genera el link de descarga del archivo de excel
function SetDownloadLink(pathFile) {
    //$("#alertCancelledPolicies").UifAlert('hide');
    var result = '@Url.Action("Download", new { controller = "CancellationNotices", pathFile = "param_name" })';
    result = result.replace("param_name", pathFile);
    $("#Form1").show();
    $("#dynamicLink").attr("href", result);

}

//FORMATOS
$("#progressBar").hide();
$("#Form1").hide();
//$("#alertCancelledPolicies").UifAlert('hide');

$('#CancelledAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
    //$("#alertCancelledPolicies").UifAlert('hide');
    $("#CancelledAgentDocumentNumber").val(selectedItem.DocumentNumber);
    $("#CancelledAgentName").val(selectedItem.DocumentNumberName);
    agentType = selectedItem.AgentTypeId;
    agentCode = selectedItem.AgentId;
    agentDoc = selectedItem.DocumentNumber;
    CancelledAgentName = selectedItem.DocumentNumberName;
});

$('#CancelledAgentName').on('itemSelected', function (event, selectedItem) {
    //$("#alertCancelledPolicies").UifAlert('hide');
    $("#CancelledAgentDocumentNumber").val(selectedItem.DocumentNumber);
    $("#CancelledAgentName").val(selectedItem.DocumentNumberName);
    agentCode = selectedItem.AgentId;
    agentType = selectedItem.AgentTypeId;
    agentDoc = selectedItem.DocumentNumber;
    CancelledAgentName = selectedItem.DocumentNumberName;
});

//Días desde
$("#daysFrom").on('blur', function (event) {
    validateDays();
});

//Días Hasta
$("#daysTo").on('blur', function (event) {
    validateDays();
});


$("#CancelledAgentDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#CancelledAgentDocumentNumber').val(agentDoc);
    }, 50);
});

$("#CancelledAgentName").on('blur', function (event) {
    setTimeout(function () {
        $('#CancelledAgentName').val(CancelledAgentName);
    }, 50);
});

//RADIOS
$('input[name=partialBalance]').change(function () {
    if ($(this).val() == 0) {
        partialBalance = 0;

    } else if ($(this).val() == 1) {
        partialBalance = 1;
    }
});

$('#CleanCancelledPolices').click(function () {
    $("#partialBalance").attr('checked', 'checked');
    $("#cancellationDate").val("");
    $("#daysFrom").val("");
    $("#daysTo").val("");
    $("#CancelledAgentDocumentNumber").val("");
    $("#CancelledAgentName").val("");
    $("#progressBar").hide();
    $("#Form1").hide();
    //$("#alertCancelledPolicies").UifAlert('hide');
});

//botón Aceptar
$('#ReportCancelledPolices').click(function () {

    //$("#alertCancelledPolicies").UifAlert('hide');
    var daysFrom = $("#daysFrom").val() == "" ? 0 : $("#daysFrom").val();
    var daysTo = $("#daysTo").val() == "" ? 0 : $("#daysTo").val();

    $("#frmCancellation").validate();

    if ($("#frmCancellation").valid()) {
        ShowProgressBar();
        $("#progressBar").show();

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "CancellationNotices/LoadCancelledPoliciesReport",
            data: JSON.stringify(
                {
                    "cancellationDate": $("#cancellationDate").val(),
                    "daysFrom": daysFrom,
                    "daysTo": daysTo,
                    "agentCode": agentCode,
                    "agentType": agentType,
                    "partialBalance": partialBalance
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "0") {
                    $("#alertCancelledPolicies").UifAlert('show', Resources.ThereIsNoData, "success");
                } else {
                    //$("#alertCancelledPolicies").UifAlert('hide');
                    SetDownloadLink(data);
                    $("#progressBar").hide();
                }
            }
        });
    }
});