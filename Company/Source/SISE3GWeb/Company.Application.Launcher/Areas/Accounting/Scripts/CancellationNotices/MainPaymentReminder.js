$(document).ready(function () {

    //VARIABLES
    var branchId = 0;
    var agentType = 0;
    var agentCode = 0;
    var individualId = 0;

    var agentData = {};
    agentData.item = {};
    agentData.item.id = 0;
    agentData.item.label = "";
    agentData.item.AgentTypeId = 0;
    agentData.item.document = "";

    // Variables para arreglar el problema de borrado que tienen los campos autocomplete cuando pierde foco
    var agentDocumentNumber = $('#PaymentReminderAgentDocumentNumber').val();
    var agentName = $('#PaymentReminderAgentName').val();


    function validateDays() {
        //$("#alertPaymentReminder").UifAlert('hide');
        if ($("#daysFrom").val() != "" &&
                   $("#daysTo").val() != "") {
            if (parseInt($("#daysFrom").val()) > parseInt($("#daysTo ").val())) {
                $("#alertPaymentReminder").UifAlert('show', Resources.WarningInvalidLogon, "danger");
                $("#daysTo ").val("");
            }
        }
    }

    function SetDownloadLink(pathFile) {
        //$("#alertPaymentReminder").UifAlert('hide');
        var result = '@Url.Action("Download", new { controller = "CancellationNotices", pathFile = "param_name" })';
        result = result.replace("param_name", pathFile);
        $("#dynamicLink").attr("href", result);
    }

    $("#daysFrom").on('blur', function (event) {
        validateDays();
    });

    $("#daysTo").on('blur', function (event) {
        validateDays();
    });

    $('#PaymentReminderAgentName').attr("disabled", "disabled");
    $('#PaymentReminderAgentDocumentNumber').attr("disabled", "disabled");
    //$("#alertPaymentReminder").UifAlert('hide');

    $('#PaymentReminderAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
        //$("#alertPaymentReminder").UifAlert('hide');
        $("#PaymentReminderAgentDocumentNumber").val(selectedItem.DocumentNumber);
        $("#PaymentReminderAgentName").val(selectedItem.DocumentNumberName);
        agentDocumentNumber = selectedItem.DocumentNumber;
        agentName = selectedItem.DocumentNumberName;
        agentType = selectedItem.AgentTypeId;
        agentCode = selectedItem.AgentId;
    });

    // Control de borrado de autocomplete en campo de número de documento.
    $("#PaymentReminderAgentDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#PaymentReminderAgentDocumentNumber').val(agentDocumentNumber);
        }, 50);
    });

    $('#PaymentReminderAgentName').on('itemSelected', function (event, selectedItem) {
        //$("#alertPaymentReminder").UifAlert('hide');
        $("#PaymentReminderAgentDocumentNumber").val(selectedItem.DocumentNumber);
        $("#PaymentReminderAgentName").val(selectedItem.DocumentNumberName);
        agentDocumentNumber = selectedItem.DocumentNumber;
        agentName = selectedItem.DocumentNumberName;
        agentCode = selectedItem.AgentId;
        agentType = selectedItem.AgentTypeId;
    });

    // Control de borrado de autocomplete en campo de nombre.
    $("#PaymentReminderAgentName").on('blur', function (event) {
        setTimeout(function () {
            $('#PaymentReminderAgentName').val(agentName);
        }, 50);
    });


    $('input[name=oneIntermediary]').change(function () {
        if ($(this).val() == 0) {
            $('#PaymentReminderAgentName').attr("disabled", "disabled");
            $('#PaymentReminderAgentDocumentNumber').attr("disabled", "disabled");
            agentData.item.id = 0;
            agentData.item.label = "";
            agentData.item.AgentTypeId = 0;
            agentData.item.document = "";
            $('#PaymentReminderAgentName').val("");
            $('#PaymentReminderAgentDocumentNumber').val("");
        }
        else if ($(this).val() == 1) {
            $('#PaymentReminderAgentName').attr("disabled", false);
            $('#PaymentReminderAgentDocumentNumber').attr("disabled", false);
        }
    });

    $('#CleanPaymentReminder').click(function () {
        //$("#alertPaymentReminder").UifAlert('hide');
        $("#allIntermediaries").attr('checked', 'checked');
        $("#cancellationDate").val("");
        $("#daysFrom").val("");
        $("#daysTo").val("");
        $("#PaymentReminderAgentDocumentNumber").val("");
        $("#PaymentReminderAgentName").val("");
        $('#PaymentReminderAgentName').attr("disabled", "disabled");
        $('#PaymentReminderAgentDocumentNumber').attr("disabled", "disabled");

    });

    $('#ReportPaymentReminder').click(function () {
        //$("#alertPaymentReminder").UifAlert('hide');
        $("#PaymentReminder").validate();

        if ($("#PaymentReminder").valid()) {

            //$("#alertPaymentReminder").UifAlert('hide');
            var daysFrom = $("#daysFrom").val() == "" ? 0 : $("#daysFrom").val();
            var daysTo = $("#daysTo").val() == "" ? 0 : $("#daysTo").val();

            var controller = ACC_ROOT + "CancellationNotices/ShowReportPaymentReminder?cancellationDate="
                + $("#cancellationDate").val() + "&daysFrom=" + daysFrom
                + "&daysTo=" + daysTo + "&agentCode=" + agentCode + "&agentType=" + agentType;

            window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');

        }
        else {
            $("#cancellationDate").focus();
            return false;
        }
    });

});