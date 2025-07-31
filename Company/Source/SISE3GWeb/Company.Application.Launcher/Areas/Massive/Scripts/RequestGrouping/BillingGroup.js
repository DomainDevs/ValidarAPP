$.ajaxSetup({ async: false });
$(document).ready(function () {

    $('#btnBillingGroupSave').prop("disabled", false);
    $("#inputBillingGroupDescription").ValidatorKey(ValidatorType.lettersandnumbersSpecial, ValidatorType.lettersandnumbersSpecial, 1);
    $("#btnBillingGroupSave").on('click', function () {        
        saveBillingGroup();        
    });
});

//Guarda el grupo de facturación
function saveBillingGroup()
{
    
    $("#formBillingGroup").validate();
    if ($("#formBillingGroup").valid())
    {        
        $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/RequestGrouping/SaveBillingGroup',
            data: JSON.stringify({ billingGroup: $("#formBillingGroup").serializeObject() }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    if (data.result != null) {                        
                        $("#inputBillingGroupId").val(data.result.Id);
                        $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MsgSuccessfullySaveBillingGroup });
                        $("#inputBillingGroup").data("Object", data.result);
                        $("#inputBillingGroup").val(data.result.Description + ' (' + data.result.Id + ')');                        
                        $('#modalBillingGroup').UifModal("hide");
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorOccurredSaveBillingGroup });
                }
            }
        });
    }
}