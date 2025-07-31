$(document).ready(function () {

    var insuredCode = 0;

    // función para limpiar los campos
    function cleanFields() {
        $('#manager').val("");
        $('#official').val("");
        $('#policy').val("");
        $('#insuredName').val("");
        $('#documentNumber').val("");
        $('#requestDate').val("");
        $('#alertForm').UifAlert('hide');
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
        $('#insuredName').val(selectedItem.Name);
        insuredName = selectedItem.Name;
        insuredDoc = selectedItem.DocumentNumber;

        insuredCode = selectedItem.InsuredCode;
        var controller = ACC_ROOT + "/CancellationNotices/GetInsuredPolicies?insuredCode=" + insuredCode;
        $('#policy').UifSelect({ source: controller });
    });

    // evento select del autocomplete nombre de asegurado
    $('#insuredName').on('itemSelected', function (event, selectedItem) {
        $('#alertForm').UifAlert('hide');
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

    // evento click en el botón Aceptar
    $('#AcceptLetter').on('click', function () {
        $('#alertForm').UifAlert('hide');
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
                    window.open(ACC_ROOT + "CancellationNotices/ShowCollectionLetterReport?date="
                    + $('#requestDate').val() + '&insured=' + insuredCode + '&number=' + $('#policy').val()
                    + '&official=' + $('#official').val() + '&manager=' + $('#manager').val(), 'mywindow',
                    'fullscreen=yes, scrollbars=auto');

                    setTimeout(function () {
                        cleanFields();
                    }, 1000);
                }
            }
        }
    });
});