var individualId = 0;
var idExclusion = 0;
var tableExclusionPerson = "#tableExclusionPerson";
var tableExclusionPolicy = "#tableExclusionPolicy";
var selectExclusionType = '#selectExclusionType';

$('#DivAgent').hide();
$('#DivExclusionPersonType').hide();
$('#DivInsured').hide();
$('#DivInsuredCode').hide();
$('#DivPolicy').hide();
$('#Divbranch').hide();
$('#DivExclusionPolicy').hide();

$(selectExclusionType).on('itemSelected', function (event, selectedItem) {
    /*Enum ExclusionTypes
        Policy  = 1,
        Agent   = 2,
        Insured = 3
    */

    $("#AlertExclusion").UifAlert('hide');
    $("#alertExclusionModal").UifAlert('hide');

    var controller = ACC_ROOT + "Parameters/GetExlusionPolicyByType?type=" + selectedItem.Id;

    if (selectedItem.Id == 1) {
        $('#DivPolicy').show();
        $('#Divbranch').show();
        $('#DivExclusionPolicy').show();

        $('#DivAgent').hide();
        $('#DivExclusionPersonType').hide();
        $('#DivInsured').hide();
        $('#DivInsuredCode').hide();

        $("#tableExclusionPolicy").UifDataTable({ source: controller });
    }
    else if (selectedItem.Id == 2) {
        $('#DivAgent').show();
        $('#DivExclusionPersonType').show()
        $('#DivAgentCode').show();

        $('#DivPolicy').hide();
        $('#Divbranch').hide();
        $('#DivInsured').hide();
        $('#DivInsuredCode').hide();
        $('#DivExclusionPolicy').hide();

        $("#tableExclusionPerson").UifDataTable({ source: controller });
    }
    else if (selectedItem.Id == 3) {
        $('#DivInsured').show();
        $('#DivExclusionPersonType').show()
        $('#DivInsuredCode').show();

        $('#DivAgent').hide();
        $('#DivAgentCode').hide();
        $('#DivPolicy').hide();
        $('#Divbranch').hide();
        $('#DivExclusionPolicy').hide();

        $("#tableExclusionPerson").UifDataTable({ source: controller });
    }
    else {
        $('#DivAgent').hide();
        $('#DivExclusionPersonType').hide()
        $('#DivAgentCode').hide();

        $('#DivPolicy').hide();
        $('#Divbranch').hide();
        $('#DivInsured').hide();
        $('#DivInsuredCode').hide();
        $('#DivExclusionPolicy').hide();
    }
});

/*Autocomplete*/
var agentDocumentNumber = $('#CancellAgentDocumentNumber').val();
var agentName = $('#CancellAgentName').val();
var insuredDocumentNumber = $('#CancellInsuredDocumentNumber').val();
var insuredName = $('#CancellInsuredName').val();

/*Agente*/
$("#CancellAgentDocumentNumber").on('itemSelected', function (event, selectedItem) {
    individualId = selectedItem.IndividualId;
    if (individualId > 0) {
        $("#CancellAgentDocumentNumber").val(selectedItem.DocumentNumber);
        $("#CancellAgentName").val(selectedItem.Name);
        agentDocumentNumber = selectedItem.DocumentNumber;
        agentName = selectedItem.Name;
    }
    else {
        $("#CancellAgentDocumentNumber").val("");
        $("#CancellAgentName").val("");
    }
});

$("#CancellAgentName").on('itemSelected', function (event, selectedItem) {
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $("#CancellAgentName").val(selectedItem.Name);
        $("#CancellAgentDocumentNumber").val(selectedItem.DocumentNumber);

        agentDocumentNumber = selectedItem.DocumentNumber;
        agentName = selectedItem.Name;
    }
    else {
        $("#CancellAgentName").val("");
        $("#CancellAgentDocumentNumber").val("");
    }
});
////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento //
////////////////////////////////////////////////////////////////////////
$("#CancellAgentDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#CancellAgentDocumentNumber').val(agentDocumentNumber);
    }, 50);
});

$("#CancellAgentName").on('blur', function (event) {
    setTimeout(function () {
        $('#CancellAgentName').val(agentName);
    }, 50);
});

/*Asegurado*/
$("#CancellInsuredDocumentNumber").on('itemSelected', function (event, selectedItem) {
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $("#CancellInsuredDocumentNumber").val(selectedItem.DocumentNumber);
        $("#CancellInsuredName").val(selectedItem.Name);
        insuredDocumentNumber = selectedItem.DocumentNumber;
        insuredName = selectedItem.Name;
    }
    else {
        $("#CancellInsuredDocumentNumber").val("");
        $("#CancellInsuredName").val("");
    }
});

$("#CancellAgentName").on('itemSelected', function (event, selectedItem) {
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $("#CancellInsuredName").val(selectedItem.Name);
        $("#CancellInsuredDocumentNumber").val(selectedItem.DocumentNumber);

        insuredDocumentNumber = selectedItem.DocumentNumber;
        insuredName = selectedItem.Name;
    }
    else {
        $("#CancellInsuredName").val("");
        $("#CancellInsuredDocumentNumber").val("");
    }
});
////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento //
////////////////////////////////////////////////////////////////////////
$("#CancellInsuredDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#CancellInsuredDocumentNumber').val(insuredDocumentNumber);
    }, 50);
});

$("#CancellInsuredName").on('blur', function (event) {
    setTimeout(function () {
        $('#CancellInsuredName').val(insuredName);
    }, 50);
});

/*fin Autocomplete*/

$(tableExclusionPerson).on('rowAdd', function (event, data) {
    $("#AlertExclusion").UifAlert('hide');
    $("#alertExclusionModal").UifAlert('hide');
    cleanExclusion();

    $('#modalExclusion').UifModal('showLocal', Resources.AddExclusion);
});

$(tableExclusionPolicy).on('rowAdd', function (event, data) {
    $("#AlertExclusion").UifAlert('hide');
    $("#alertExclusionModal").UifAlert('hide');
    cleanExclusion();
    $('#modalExclusion').UifModal('showLocal', Resources.AddExclusion);
});

$(tableExclusionPerson).on('rowDelete', function (event, data) {
    $("#AlertExclusion").UifAlert('hide');
    $("#alertExclusionModal").UifAlert('hide');

    idExclusion = data.Id;
    $('#modalDeleteExclusionForm').appendTo("body").modal('show');
});

$(tableExclusionPolicy).on('rowDelete', function (event, data) {
    $("#AlertExclusion").UifAlert('hide');
    $("#alertExclusionModal").UifAlert('hide');

    idExclusion = data.Id;
    $('#modalDeleteExclusionForm').appendTo("body").modal('show');
});

$('#saveExclusion').click(function () {
    var exclusionType = $(selectExclusionType).val();

    if (exclusionType == 1) {
        $("#addExclusionForm").validate();
        if ($("#addExclusionForm").valid()) {
            if (existsExclusionPolicy() == true) {
                $("#alertExclusionModal").UifAlert('show', Resources.PolicyExists, "warning");
            }
            else {
                /*Graba la póliza */
                saveExclusionPolicy();
            }
        }
    }
    else if (exclusionType == 2) {
        $("#addExclusionForm").validate();
        if ($("#addExclusionForm").valid()) {
            if (existsExclusionPerson("#CancellAgentDocumentNumber")) {
                $("#alertExclusionModal").UifAlert('show', Resources.AgentExists, "warning");
            }
            else {
                saveExlusionPerson();
            }
        }
    }
    else {
        $("#addExclusionForm").validate();
        if ($("#addExclusionForm").valid()) {
            if (existsExclusionPerson("#CancellInsuredDocumentNumber")) {
                $("#alertExclusionModal").UifAlert('show', Resources.InsuredExists, "warning");
            }
            else {
                saveExlusionPerson();
            }
        }
    }
});

$("#deleteExclusion").on('click', function () {

    $('#modalDeleteExclusionForm').modal('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteExclusion",
        data: { 'idExclusion': idExclusion },
    }).done(function (data) {
        if (data.success) {
            $("#AlertExclusion").UifAlert('show', Resources.DeleteSuccessfully, "success");

            var controller = ACC_ROOT + "Parameters/GetExlusionPolicyByType?type=" + $('#selectExclusionType').val();
            if ($('#selectExclusionType').val() == 1) {
                $(tableExclusionPolicy).UifDataTable({ source: controller });
            }
            else {
                $(tableExclusionPerson).UifDataTable({ source: controller });
            }
        }
        else {
            $("#AlertExclusion").UifAlert('show', data.result, "danger");
        }
    });
});

/*FUNCIONES*/
function saveExclusionPolicy() {
    $.ajax({
        url: ACC_ROOT + 'Parameters/GetPolicyId',
        data: { 'branchCode': $('#BranchCancellation').val(), 'prefixCode': $('#PrefixCancellation').val(), 'documentNumber': $('#PolicyCancellationNumber').val() },
    }).done(function (data) {
        if (data.aaData > 0) {
            $.ajax({
                url: ACC_ROOT + 'Parameters/SaveExclusion',
                data: {
                    'exclusionType': $('#selectExclusionType').val(), 'branchCode': $('#BranchCancellation').val(), 'prefixCode': $('#PrefixCancellation').val(),
                    'policyId': data.aaData, 'individualId': 0
                },
            }).success(function (data) {
                if (data.success) {
                    $("#AlertExclusion").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "Parameters/GetExlusionPolicyByType?type=" + $('#selectExclusionType').val();
                    $(tableExclusionPolicy).UifDataTable({ source: controller });
                }
                else {
                    $("#AlertExclusion").UifAlert('show', Resources.ErrorTransaction, "danger");
                }
                $('#modalExclusion').UifModal('hide');
            });
        }
        else {
            $("#alertExclusionModal").UifAlert('show', Resources.PolicyNotExists, "warning");
        }
    });
}

function saveExlusionPerson() {
    $.ajax({
        url: ACC_ROOT + 'Parameters/SaveExclusion',
        data: {
            'exclusionType': $('#selectExclusionType').val(), 'branchCode': 0, 'prefixCode': 0, 'policyId': 0, 'individualId': individualId
        },
    }).success(function (data) {
        if (data.success) {
            $("#AlertExclusion").UifAlert('show', Resources.SaveSuccessfully, "success");
            var controller = ACC_ROOT + "Parameters/GetExlusionPolicyByType?type=" + $('#selectExclusionType').val();
            $(tableExclusionPerson).UifDataTable({ source: controller });
        }
        else {
            $("#AlertExclusion").UifAlert('show', Resources.ErrorTransaction, "danger");
        }
        $('#modalExclusion').UifModal('hide');
    });
}

function cleanExclusion() {
    $('#BranchCancellation').val("");
    $('#PrefixCancellation').val("");
    $('#PolicyCancellationNumber').val("");
    $('#CancellAgentDocumentNumber').val("");
    $('#CancellAgentName').val("");
    $('#CancellInsuredCode').val("");
    $('#CancellInsuredDocumentNumber').val("");
    $('#CancellInsuredName').val("");
}

function existsExclusionPerson(documentNumber) {
    var exists = false;

    var fields = $(tableExclusionPerson).UifDataTable("getData");
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if ($(documentNumber).val() == fields[j].DocumentNumber) {
                exists = true;
                break;
            }
        }
    }
    return exists
}

function existsExclusionPolicy() {
    var exists = false;
    var fields = $(tableExclusionPolicy).UifDataTable("getData");
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if ($('#BranchCancellation').val() == fields[j].BranchCd && $('#PrefixCancellation').val() == fields[j].PrefixCd && $('#PolicyCancellationNumber').val() == fields[j].PolicyNumber) {
                exists = true;
                break;
            }
        }
    }
    return exists
}

