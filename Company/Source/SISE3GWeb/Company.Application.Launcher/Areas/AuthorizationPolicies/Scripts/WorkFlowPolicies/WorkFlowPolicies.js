var userId;
class WorkFlowPolicies extends Uif2.Page {
    getInitialState() {
        WorkFlowPolicies.GetEventGroups();
        //WorkFlowPolicies.checkhierarchys();
        
    }

    bindEvents() {
        $("#btnSearchWorkFlowPolicies").on('click', WorkFlowPolicies.GetEventAuthorizations);
        $("#btnSaveEventAuthorizations").on('click', WorkFlowPolicies.CreateEventAuthorizations);
        $('#autocompleteUsers').on('itemSelected', WorkFlowPolicies.SelectedUser);

        $('#btnExitDelegation').on('click', WorkFlowPolicies.ExitWorkFlowWindow);
        
    }

    static ValidateSearch() {

        var msj = "";
        if ($("#dpFinishDate").UifDatepicker('getValue') == null || $("#datepicker").UifDatepicker('getValue') == "") {
            msj = AppResources.LabelDateEnd + "<br>"
        }
        if ($("#dpStartDate").UifDatepicker('getValue') == null || $("#datepicker").UifDatepicker('getValue') == "") {
            msj = AppResources.LabelCurrentDate + "<br>"
        }
        if ($("#dpStartDate").UifDatepicker('getValue') > $("#dpFinishDate").UifDatepicker('getValue')) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorFromDateToDate, 'autoclose': true })
            return false;
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }
        
        return true;
    }

    static ValidateCreate() {

        var msj = "";
        if ($("#txtObservations").val() == null || $("#txtObservations").val() == "") {
            msj = AppResources.LabelObservations + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + " <br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }

        return true;
    }

    static GetEventGroups() {
        WorkFlowPoliciesRequest.GetEventGroups().done(function (data) {
            if (data.success) {
                $('#selectEventGroup').UifSelect({
                    sourceData: data.result
                });
                $("#selectEventGroup").UifSelect("setSelected", data.result[0].Id);
                $("#selectEventGroup").UifSelect("disabled", true);
                WorkFlowPolicies.GetEventAuthorizationsByUserIdInitial();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static SelectedUser(event, itemSelected) {
        userId = itemSelected.Id;
    }

    static CreateEventAuthorizations() {
        if (WorkFlowPolicies.ValidateCreate()) {
            var eventAuthorizations = $("#tblPolicies").UifDataTable('getSelected');
            var description = $("#txtObservations").val();
            if (eventAuthorizations != null) {
                WorkFlowPoliciesRequest.CreateEventAuthorizations(eventAuthorizations, description).done(function (data) {
                    //WorkFlowPolicies.checkhierarchys();
                    if (data.success) {
                        $("#tblPolicies").UifDataTable('clear');
                        $("#txtObservations").val("");
                        var polizasAutorizadas = "";
                        var PolizasNoAutorizadas = "";
                        for (var i = 0; i < data.result.length; i++) {
                            if (data.result[i].AuthorizedInd) {
                                polizasAutorizadas +=  data.result.length -1 == i ? data.result[i].PolicyNumber + ". " : data.result[i].PolicyNumber+", "; 
                            }
                            else {
                                PolizasNoAutorizadas += data.result.length -1 == i ? data.result[i].PolicyNumber + ". " : data.result[i].PolicyNumber+", "; 
                            }
                        }
                        if (polizasAutorizadas != "") {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.PoliciesDelivered + " " + polizasAutorizadas , 'autoclose': true });
                        }

                        if (PolizasNoAutorizadas != "") {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.PoliciesNotDelivered + " " + PolizasNoAutorizadas, 'autoclose': true });
                        }


                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result , 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.SelectLeastOnePolicy , 'autoclose': true });
            }
        }
    }

    static GetEventAuthorizations() {
        $("#tblPolicies").UifDataTable('clear');
        if (WorkFlowPolicies.ValidateSearch()) {
            var eventGroupId = parseInt($("#selectEventGroup").UifSelect("getSelected"));
            var startDate = $("#dpStartDate").UifDatepicker('getValue');
            var finDate = $("#dpFinishDate").UifDatepicker('getValue');
            var finishDate = AddDayToDate(finDate, 1);
            WorkFlowPoliciesRequest.GetEventAuthorizationsByUserId(userId, eventGroupId, startDate, finishDate).done(function (data) {
                if (data.success) {
                    console.log(data.result);
                    if (data.result == 0) {
                        $.UifNotify('show', { 'type': 'danger', 'message': 'No se han encontrado registros', 'autoclose': true });
                    }
                    else {
                        $("#dpStartDate").UifDatepicker('clear');
                        $("#dpFinishDate").UifDatepicker('clear');
                        $('#autocompleteUsers').UifAutoComplete('clean');
                        $.each(data.result, function (index, value) {
                            this.EventDate = FormatDate(this.EventDate);
                        });
                        $("#tblPolicies").UifDataTable({ sourceData: data.result });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            })
        }
    }

    static GetEventAuthorizationsByUserIdInitial() {
            var eventGroupId = parseInt($("#selectEventGroup").UifSelect("getSelected"));
            WorkFlowPoliciesRequest.GetEventAuthorizationsByUserIdInitial(eventGroupId).done(function (data) {
                if (data.success) {
                    if (data.result == 0) {
                        
                    }
                    else {
                        $("#dpStartDate").UifDatepicker('clear');
                        $("#dpFinishDate").UifDatepicker('clear');
                        $('#autocompleteUsers').UifAutoComplete('clean');
                        $.each(data.result, function (index, value) {
                            this.EventDate = FormatDate(this.EventDate);
                        });
                        $("#tblPolicies").UifDataTable({ sourceData: data.result });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            })
    }

    static ExitWorkFlowWindow() {
        window.location = rootPath + "Home/Index";
    }

    
    //static checkhierarchys() {
    //    WorkFlowPoliciesRequest.ValidationAccessAndHierarchys().done(function (data) {
    //    if (data.success) {
    //        // Tiene Jerarquia no hacer nada
    //    }
    //    else {
    //        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
    //    }
    //}).fail(function (jqXHR, textStatus, errorThrown) {
    //    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
    //});
}