var glbRiskEventGroup;
var glbRiskList;
class AuthorizationPersonRiskList extends Uif2.Page {
    getInitialState() {
        AuthorizationPersonRiskList.InitialRiskList();
    }

    bindEvents() {
        //Busqueda
        $("#btnSearchSarlaftOperation").on('click', AuthorizationPersonRiskList.GetEventAuthorizationRiskList);

        //Seleccion en Tabla
        $('#tblAuthorizationPersonRiskList').on('rowSelected', AuthorizationPersonRiskList.GetRiskList);

        //Aceptar
        $("#btnAcceptSarlaftOperation").click(AuthorizationPersonRiskList.AuthorizeRiskList);

        //Rechazar
        $("#btnRejectSarlaftOperation").click(AuthorizationPersonRiskList.RejectRiskList);

        //Limpiar
        $("#btnExitSarlaftOperation").click(AuthorizationPersonRiskList.ClearFormRiskList);

        //Campos deshabilitados
        $("#riskDetailRequest").prop('disabled', true);
        $("#riskEventDate").prop('disabled', true);
        $("#inpDetail").prop('disabled', true);

        //Numeros
        $("#inpDocumentNo").on('keypress', AuthorizationPersonRiskList.OnlyNumbers);

    }

    static InitialRiskList() {
        AuthorizationPersonRiskListRequest.GetDocumentType().done(function (data) {
            if (data.success) {
                $("#selectDocumentType").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        AuthorizationPersonRiskListRequest.GetSarlaftEventGroup().done(function (data) {
            if (data.success) {
                $("#selectSarlaftEventGroup").UifSelect({ sourceData: data.result });
                $("#selectSarlaftEventGroup").UifSelect("setSelected", data.result[0].Id);
                $("#selectSarlaftEventGroup").UifSelect("disabled", true);
                glbRiskEventGroup = data.result[0].Id;
                AuthorizationPersonRiskList.GetAuthorizationReasons();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
            }
        });

    }

    static ValidateSearch() {

        var msj = "";

        if ($("#selectSarlaftEventGroup").UifSelect("getSelected") == null || $("#selectSarlaftEventGroup").UifSelect("getSelected") == "") {
            msj = AppResources.DocumentType + "<br>"
        }
        if ($("#inpDocumentNo").val() == null || $("#inpDocumentNo").val() == "") {
            msj = AppResources.DocumentNumber + "<br>"
        }
        if ($("#selectDocumentType").UifSelect("getSelected") == null || $("#selectDocumentType").UifSelect("getSelected") == "") {
            msj = AppResources.DocumentType + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }
        return true;
    }

    static GetEventAuthorizationRiskList() {
        if (AuthorizationPersonRiskList.ValidateSearch()) {
            var objAuthorizationRiskListModelView = {
                EventGroupId: parseInt($("#selectSarlaftEventGroup").UifSelect("getSelected")),
                DocumentNumber: $("#inpDocumentNo").val(), //+ Shared.CalculateDigitVerify($("#inpDocumentNo").val()),
                DocumentType: $("#selectDocumentType").UifSelect('getSelected')
            };
            if ($('#selectDocumentType').val() != 1
                && $('#selectDocumentType').val() != 3
                && $('#selectDocumentType').val() != 4
                && $('#selectDocumentType').val() != 6) {
                objAuthorizationRiskListModelView.DocumentNumber = $("#inpDocumentNo").val() +
                    Shared.CalculateDigitVerify($("#inpDocumentNo").val());
            }


            AuthorizationPersonRiskListRequest.GetEventAuthorizationsByUserId(objAuthorizationRiskListModelView).done(function (data) {
                if (data.success) {
                    $("#tblAuthorizationPersonRiskList").UifDataTable({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static GetRiskList(event, data1, position) {
        if (data1 !== null) {
            glbRiskList = data1;
            $("#riskEventDate").val(FormatDate(data1.EventDate));
            $("#inpDetail").val(data1.Detail);
            $("#riskDetailRequest").val(data1.RequestDetail);
            $("#selectReasons").UifSelect('setSelected', data.result[0].Id);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }

    static GetAuthorizationReasons() {
        AuthorizationSarlaftOperationRequest.GetAuthorizationReasons(glbRiskEventGroup).done(function (data) {
            if (data.success) {
                $("#selectRiskAuthReason").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static AuthorizeRiskList() {
        var authorizeRiskListViewModel = {
            AuthorizationId: glbRiskList.AuthorizationId,
            IsAuthorized: glbRiskList.IsAuthorized = true,
            AuthorizeReasonId: $("#selectRiskAuthReason").UifSelect('getSelected'),
            Description: $("#txtObservations").val()
        }
        $("#formSendRiskList").validate();
        if ($("#formSendRiskList").valid()) {
            AuthorizationPersonRiskListRequest.AuthorizeRiskList(authorizeRiskListViewModel).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': "Operacion en lista de Riesgo autorizada satisfactoriamente", 'autoclose': true });
                    AuthorizationPersonRiskList.ClearFormsolicitude();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Error al Autorizar Operacion en lista de Riesgo", 'autoclose': true });
                }
            });
        }
    }

    static RejectRiskList() {
        var authorizeRiskListViewModel = {
            AuthorizationId: glbRiskList.AuthorizationId,
            IsRejected: glbRiskList.IsRejected = true,
            RejectId: glbRiskList.RejectId = 1,
            AuthorizeReasonId: $("#selectRiskAuthReason").UifSelect('getSelected'),
            Description: $("#txtObservations").val()
        }
        if (authorizeRiskListViewModel.Description != "") {
            AuthorizationPersonRiskListRequest.AuthorizeRiskList(authorizeRiskListViewModel).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': "Operacion en lista de Riesgo rechazada satisfactoriamente", 'autoclose': true });
                    AuthorizationPersonRiskList.ClearFormsolicitude();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Error al Rechazar Operacion en lista de Riesgo", 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe Diligenciar Las Observaciones.", 'autoclose': true });
        }

    }

    static ClearFormsolicitude() {
        $("#riskEventDate").val("");
        $("#inpDetail").val("");
        $("#riskDetailRequest").val("");
        $("#selectRiskAuthReason").UifSelect("setSelected", null);
        $("#txtObservations").val("");
    }

    static ClearFormRiskList() {
        $("#inpDocumentNo").val("");
        $("#selectDocumentType").val("");
        $("#dateRiskList").val("");
        $("#riskEventDate").val("");
        $("#inpDetail").val("");
        $("#riskDetailRequest").val("");
        $("#selectRiskAuthReason").val("");
        $("#txtObservations").val("");
        $('#tblAuthorizationPersonRiskList').UifDataTable('clear');
    }

    static OnlyNumbers(event) {
        if (event.keyCode !== 8 && event.keyCode !== 46 && event.keyCode !== 37 && event.keyCode !== 39 && event.keyCode !== 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }
}