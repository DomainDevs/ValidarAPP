
class TemporalSearch extends Uif2.Page {
    getInitialState() {
        //Validacion de campos en el formulario de busqueda
        $("#inputInsuredTemporary").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputUserTemporary").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputTemporaryNumber").ValidatorKey(ValidatorType.Number, 3, 0);
    }
    //Seccion Eventos
    bindEvents() {
        $("#btnSearchTemporary").click(this.SearchTemporary);
        $("#inputInsuredTemporary").on('buttonClick', this.SearchInsured);//?
        $("#inputUserTemporary").on('buttonClick', this.SearchUser);
    }

    ///Buscar temporario
    SearchTemporary() {
        if ((!$("#inputInsuredTemporary").val() || !$("#inputInsuredIdTemporary").val()) && !$("#inputTemporaryNumber").val()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelTemporalSearchValidate, 'autoclose': true });
        }
        else {
            $("#formSearchTemporary").validate();
            if ($("#formSearchTemporary").valid()) {
                if (!$("#inputInsuredTemporary").val() || !$("#inputInsuredIdTemporary").val()) {
                    $('#inputInsuredTemporary, #inputInsuredIdTemporary').val("");
                }
                var subscriptionSearchViewModel =
                    {
                        Insured: $("#inputInsuredTemporary").val(),
                        InsuredId: $("#inputInsuredTemporary").val() == "" ? null : $("#inputInsuredIdTemporary").val(),
                        TemporaryNumber: $("#inputTemporaryNumber").val(),
                        UserId: $("#inputUserTemporary").val() == "" ? null : $("#inputUserIdTemporary").val(),
                        IssueDate: $("#inputIssueDateTemporary").val()
                    };

                TemporalSearchRequest.SearchTemporaries(subscriptionSearchViewModel).done(function (data) {
                    if (data.success) {
                        $('#tableTemporal').UifDataTable({ sourceData: data.result });
                        if (data.result.length == 0) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorServiceNull, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

    ///Buscar asegurado
    SearchInsured() {
        if ($("#inputInsuredTemporary").val().trim().length > 0) {
            SubscriptionSearch.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputInsuredTemporary").val(), InsuredSearchType.DocumentNumber, null, $("#inputInsuredTemporary"), $("#inputInsuredIdTemporary"));
        }
    }

    ///Buscar usuario
    SearchUser() {
        if ($("#inputUserTemporary").val().trim().length > 0) {
            SubscriptionSearch.GetUsersByDescription($("#inputUserTemporary"), $("#inputUserIdTemporary"));
        }
    }
}