$(document).ready(function () {

    var insuredCode = 0;
    var control = 0;

    // función para limpiar los campos
    function cleanFields() {
        $('#alertForm').UifAlert('hide');
        $('#manager').val("");
        $('#official').val("");
        $('#policy').val("");
        $('#insuredName').val("");
        $('#documentNumber').val("");
        $('#requestDate').val("");
        $('#letter').val("");
        control = 0;
    }

    // evento click en el botón Limpiar
    $('#CleanLetter').on('click', function () {
        cleanFields();
    });

    var insuredDoc = $('#documentNumber').val();
    var insuredName = $('#insuredName').val();

    // evento select del autocomplete nro. de doc.
    $('#documentNumber').on('itemSelected', function (event, selectedItem) {
        $('#alertForm').UifAlert('hide');
        control = 0;
        $('#letter').val("");
        $('#insuredName').val(selectedItem.Name);
        insuredName = selectedItem.Name;
        insuredDoc = selectedItem.DocumentNumber;

        insuredCode = selectedItem.InsuredCode;
        var controller = ACC_ROOT + "CancellationNotices/GetInsuredPolicies?insuredCode=" + insuredCode;
        $('#policy').UifSelect({ source: controller });
    });

    // evento select del autocomplete nombre de asegurado
    $('#insuredName').on('itemSelected', function (event, selectedItem) {
        $('#alertForm').UifAlert('hide');
        control = 0;
        $('#letter').val("");
        $('#documentNumber').val(selectedItem.DocumentNumber);
        insuredName = selectedItem.Name;
        insuredDoc = selectedItem.DocumentNumber;

        insuredCode = selectedItem.InsuredCode;
        var controller = ACC_ROOT + "CancellationNotices/GetInsuredPolicies?insuredCode=" + insuredCode;
        $('#policy').UifSelect({ source: controller });
    });

    //pierde el foco el campo de nro. de doc.
    $("#documentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#documentNumber').val(insuredDoc);
        }, 50);
    });

    //pierde el foco el campo de nombre de asegurado
    $("#insuredName").on('blur', function (event) {
        setTimeout(function () {
            $('#insuredName').val(insuredName);
        }, 50);
    });

    // evento para buscar una carta legal existente
    $('#btnSearch').on('click', function () {
        $('#alertForm').UifAlert('hide');
        $("#alertSearch").UifAlert('hide');
        if ($('#letter').val() == "") {
            $("#alertSearch").UifAlert('show', Resources.EnterSearchCriteria, "danger");
            cleanFields();
        }
        else {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "CancellationNotices/ValidateBusinessLetter",
                data: JSON.stringify({ "businessLetter": $('#letter').val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data == false || data == null) {
                        $("#alertSearch").UifAlert('show', Resources.RegisterNotFound, "danger");
                        cleanFields();
                    }
                    else {
                        control = 1;

                        //autocomplete pierde el foco
                        insuredDoc = data.DocumentNumber;
                        insuredName = data.Insured;

                        insuredCode = data.InsuredCode;
                        $('#official').val(data.Official);
                        $('#manager').val(data.Manager);
                        $('#requestDate').val(data.Date);
                        $('#documentNumber').val(data.DocumentNumber);
                        $('#insuredName').val(data.Insured);

                        var controller = ACC_ROOT + "CancellationNotices/GetInsuredPolicies?insuredCode="
                           + data.PersonId;
                        $('#policy').UifSelect({ source: controller });

                        setTimeout(function () {
                            $('#policy').val(data.PolicyNumber);
                        }, 100)
                    }
                }
            });
        }
    });

    // evento click en el botón Aceptar
    $('#AcceptLetter').on('click', function () {
        $('#alertForm').UifAlert('hide');
        var insuredDoc = "";
        var insuredName = "";
        $('#report').validate();
        if ($("#report").valid()) {
            if ($('#policy').val() == null) {
                $("#alertForm").UifAlert('show', Resources.InsuredWithPolicies, "warning");
            }
            else {
                if (!$("#report").valid()) {
                    return false;
                }
                else {
                    if (control == 0) {
                        window.open(ACC_ROOT + "CancellationNotices/ShowBusinessLetterReport?date="
                        + $('#requestDate').val() + '&insuredCode=' + insuredCode + '&insuredName='
                        + $('#insuredName').val() + '&number=' + $('#policy').val() + '&official='
                        + $('#official').val() + '&manager=' + $('#manager').val(), 'mywindow',
                        'fullscreen=yes, scrollbars=auto');

                        setTimeout(function () {
                            cleanFields();
                        }, 1000);
                    }
                    else {
                        $.UifDialog('confirm', { 'message': Resources.MessageBusinessLetter   + $('#letter').val() + Resources.PrintBusinessLetter, 'title': Resources.TitleBusinessLetter }, function (result) {
                            if (result) {
                                window.open(ACC_ROOT + "CancellationNotices/ShowBusinessLetterReport?date="
                                + $('#requestDate').val() + '&insuredCode=' + insuredCode + '&insuredName='
                                + $('#insuredName').val() + '&number=' + $('#policy').val() + '&official='
                                + $('#official').val() + '&manager=' + $('#manager').val(), 'mywindow',
                                'fullscreen=yes, scrollbars=auto');

                                setTimeout(function () {
                                    cleanFields();
                                }, 1000);
                            }
                        });
                    }
                }
            }
        }

    });
});