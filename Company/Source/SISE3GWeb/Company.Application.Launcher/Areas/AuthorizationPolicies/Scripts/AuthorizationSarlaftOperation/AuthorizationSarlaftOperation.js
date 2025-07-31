var userId;
var glbEventGroup;
var authorizeId;
var gblSuspectOperation;
class AuthorizationSarlaftOperation extends Uif2.Page {
    getInitialState() {
        AuthorizationSarlaftOperation.InitialSuspectOperation();

    }

    bindEvents() {
        //Autocompletar Usuario
        $('#autocompleteUsers').on('itemSelected', AuthorizationSarlaftOperation.SelectedUser);

        //Busqueda
        $("#btnSearchSarlaftOperation").click(AuthorizationSarlaftOperation.SearchSuspectOperations);

        //Seleccion de la Tabla
        $('#tblAuthorizationSarlaftOperation').on('rowSelected', AuthorizationSarlaftOperation.GetSuspectOperation);

        //Campos Deshabilitados
        $("#inpDetail").prop('disabled', true);
        $("#RequestDetail").prop('disabled', true);
        $("#EventDate").prop('disabled', true);

        //Aceptar Operacion Sospechosa
        $("#btnAcceptSarlaftOperation").click(AuthorizationSarlaftOperation.AuthorizeSuspectOperation);

        //Rechazar Operacion Sospechosa
        $("#btnRejectSarlaftOperation").click(AuthorizationSarlaftOperation.RejectSuspectOperation);

        //Limpiar Pantalla
        $("#btnExitSarlaftOperation").click(AuthorizationSarlaftOperation.ClearFormSuspectOperation);
        
        //Numeros
        $("#inpDocumentNo").on('keypress', AuthorizationSarlaftOperation.OnlyNumbers);
        $("#inpFormNumber").on('keypress', AuthorizationSarlaftOperation.OnlyNumbers);
        $("#inpSignYear").on('keypress', AuthorizationSarlaftOperation.OnlyNumbers);
        $("#inpDocumentNumber").on('keypress', AuthorizationSarlaftOperation.OnlyNumbers);
        $("#inpIncome").on('keypress', AuthorizationSarlaftOperation.OnlyNumbers);
        $("#inpIncome").focusin(AuthorizationSarlaftOperation.NotFormatMoneyIn);
        $("#inpIncome").focusout(AuthorizationSarlaftOperation.FormatMoneyOut);
    }

    static InitialSuspectOperation() {
        AuthorizationSarlaftOperationRequest.GetSarlaftEventGroup().done(function (data) {
            if (data.success) {
                $("#selectSarlaftEventGroup").UifSelect({ sourceData: data.result });
                $("#selectSarlaftEventGroup").UifSelect('setSelected', data.result[0].Id);
                $("#selectSarlaftEventGroup").UifSelect('disabled', true);
                glbEventGroup = data.result[0].Id;
                AuthorizationSarlaftOperation.GetAuthorizationReasons();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static SearchSuspectOperations() {
        if (!$("#inpDocumentNo").val() && !$("#inpFormNumber").val() && !$("#inpSignYear").val() && !$("#selectSarlaftEventGroup").UifSelect("getSelected") && !$("#inpDocumentNumber").val()
            && !$("#autocompleteUsers").val()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelQuotationSearchValidate, 'autoclose': true });
        }
        else {
            $("#formSearchSuspectOp").validate()
            if ($("#formSearchSuspectOp").valid()) {
                var authorizationSuspectOperationViewModel = {
                    EventGroupId: $("#selectSarlaftEventGroup").UifSelect("getSelected"),
                    DocumentNumber: $("#inpDocumentNo").val(),
                    FormNumber: $("#inpFormNumber").val(),
                    Year: $("#inpSignYear") == "" ? null : $("#inpSignYear").val(),
                    IndividualId: $("#inpDocumentNumber").val(),
                    User: $("#autocompleteUsers").val(),
                    UserId: $("#autocompleteUsers") == "" ? null : userId
                }
                AuthorizationSarlaftOperationRequest.SearchSuspectOperations(authorizationSuspectOperationViewModel).done(function (data) {
                    if (data.success) {
                        $('#autocompleteUsers').UifAutoComplete('clean');
                        let gblSuspectOperation = data.result;
                        $("#tblAuthorizationSarlaftOperation").UifDataTable({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }
    static GetSuspectOperation(event, data1, position) {
        if (data1 != null) {
            gblSuspectOperation = data1;
            $("#inpDetail").val(data1.Detail);
            $("#inpIncome").val(data1.Assets);
            $("#RequestDetail").val(data1.RequestDetail);
            $("#EventDate").val(FormatDate(data1.EventDate));
            AuthorizationSarlaftOperation.GetAuthorizationReasons();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }

    static GetAuthorizationReasons() {
        AuthorizationSarlaftOperationRequest.GetAuthorizationReasons(glbEventGroup).done(function (data) {
            if (data.success) {
                $("#selectReasons").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static AuthorizeSuspectOperation() {
        $("#formSendSuspectOp").validate();
        $("#formSearchSuspectOp").validate();
        var authorizeSuspectOperationViewModel = {
            AuthorizationId: gblSuspectOperation.AuthorizationId,
            IsAuthorized: gblSuspectOperation.IsAuthorized = true,
            AuthorizeReasonId: $("#selectReasons").UifSelect('getSelected'),
            Assets: $("#inpIncome").val(),
            Description: $("#txtObservations").val()
        }
        if ($("#formSendSuspectOp").valid() && $("#formSearchSuspectOp").valid()) {
            AuthorizationSarlaftOperationRequest.AuthorizeSuspectOperation(authorizeSuspectOperationViewModel).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': "Operacion Sospechosa Autorizada satisfactoriamente", 'autoclose': true });
                    AuthorizationSarlaftOperation.ClearFormsolicitude();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Error al Autorizar Operacion Sospechosa", 'autoclose': true });
                }
            });
        }
    }

    static SelectedUser(event, itemSelected) {
        userId = itemSelected.Id;
    }

    static RejectSuspectOperation() {
        var authorizeSuspectOperationViewModel = {
            AuthorizationId: gblSuspectOperation.AuthorizationId,
            IsRejected: gblSuspectOperation.IsRejected = true,
            RejectId: gblSuspectOperation.RejectId = 1,
            AuthorizeReasonId: $("#selectReasons").UifSelect('getSelected'),
            Assets: $("#inpIncome").val(),
            Description: $("#txtObservations").val()
        }
        if (authorizeSuspectOperationViewModel.Description != "")
        {
            AuthorizationSarlaftOperationRequest.AuthorizeSuspectOperation(authorizeSuspectOperationViewModel).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': "Operacion Sospechosa rechazada satisfactoriamente", 'autoclose': true });
                    AuthorizationSarlaftOperation.ClearFormsolicitude();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Error al Rechazar Operacion Sospechosa", 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe Diligenciar Las Observaciones.", 'autoclose': true });
        }    
    }

    static ClearFormsolicitude() {
        $("#EventDate").val("");
        $("#inpDetail").val("");
        $("#RequestDetail").val("");
        $("#selectReasons").UifSelect("setSelected", null);
        $("#inpIncome").val("");
        $("#txtObservations").val("");
    }

    static ClearFormSuspectOperation()
    {
        $("#inpDocumentNo").val("");
        $("#inpFormNumber").val("");
        $("#inpSignYear").val("");
        $("#inpDocumentNumber").val("");
        $("#autocompleteUsers").val("");
        $("#EventDate").val("");
        $("#inpDetail").val("");
        $("#RequestDetail").val("");
        $("#selectReasons").val("");
        $("#inpIncome").val("");
        $("#txtObservations").val("");
        $('#tblAuthorizationSarlaftOperation').UifDataTable('clear');
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        $(this).val(FormatMoney($(this).val()));
    }

    static OnlyNumbers(event) {
        if (event.keyCode !== 8 && event.keyCode !== 46 && event.keyCode !== 37 && event.keyCode !== 39 && event.keyCode !== 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }
}