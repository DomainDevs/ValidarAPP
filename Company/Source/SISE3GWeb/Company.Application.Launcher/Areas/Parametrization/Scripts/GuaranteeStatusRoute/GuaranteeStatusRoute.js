class GuaranteeStatusRoute extends Uif2.Page {


    getInitialState() {
        GuaranteeStatusRoute.InitialGuaranteeStatusRoute();
        GuaranteeStatusRoute.initialListView();
    }

    bindEvents() {

        //grabar GuaranteeStatusRoute
        $("#btnSave").on("click", function () {
            try {
                GuaranteeStatusRoute.SaveGuaranteeStatusRoute();
            } catch (e) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSavingGuaranteeStatus, 'autoclose': true });
            }
           
        });

        $("#btnCancel").on("click", function () {
            GuaranteeStatusRoute.CancelView();
        });

        $('#selectGuaranteeStatus').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id.trim() !== "") {
                GuaranteeStatusRoute.GetListGuaranteeStatus(event, selectedItem);
                GuaranteeStatusRoute.GetGuaranteeStatusRoutesByGuaranteeStatusId(event, selectedItem);
            }
            else {
                GuaranteeStatusRoute.initialListView();
            }
            
        });

        //Asignar Todos
        $("#btnGuaranteeStatusAssignAll").on("click", function () {
            try {
                GuaranteeStatusRoute.CopyGuaranteeStatus($("#listViewGuaranteeStatus").UifListView('getData'));
            } catch (e) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMovingGuaranteeStatus, 'autoclose': true });
            }
        });

        //Desasignar Todos
        $("#btnGuaranteeStatusDeallocateAll").on("click", function () {
            try {
                GuaranteeStatusRoute.deallocateGuaranteeStatusRoute($("#listviewGuaranteeStatusRoute").UifListView('getData'));
            } catch (e) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMovingGuaranteeStatus, 'autoclose': true });
            }
        });

        //Asignar Uno
        $("#btnGuaranteeStatusAssign").on("click", function () {
            if ($("#listViewGuaranteeStatus").UifListView('getData').length > 0) {
                try {
                    GuaranteeStatusRoute.CopyGuaranteeStatusSelected($("#listViewGuaranteeStatus").UifListView('getSelected'));
                } catch (e) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMovingGuaranteeStatus, 'autoclose': true });
                } 
            }
        });

        //Desasignar Uno
        $("#btnGuaranteeStatusDeallocate").on("click", function () {
            if ($("#listviewGuaranteeStatusRoute").UifListView('getData').length > 0) {
                try {
                    GuaranteeStatusRoute.deallocateGuaranteeStatusSelect($("#listviewGuaranteeStatusRoute").UifListView('getSelected'));
                } catch (e) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMovingGuaranteeStatus, 'autoclose': true });
                }
            }
        });
    }

    static GetGuaranteeStatusRoutesByGuaranteeStatusId(event, selectedItem) {
        GuaranteeStatusRouteRequest.GetGuaranteeStatusRoutesByGuaranteeStatusId(selectedItem.Id).done(function (data) {
            unlockScreen();
            if (data.success) {
                $("#listviewGuaranteeStatusRoute").UifListView({ sourceData: data.result, displayTemplate: "#tmpForGuaranteeStatusAssign", selectionType: 'multiple', height: 310 });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.GetGuaranteeStatusRoutes, 'autoclose': true })
            }
        });
    }

    static GetListGuaranteeStatus(event, selectedItem) {
        GuaranteeStatusRouteRequest.GetListGuaranteeStatus(selectedItem.Id).done(function (data) {
            unlockScreen();
            if (data.success) {
                $("#listViewGuaranteeStatus").UifListView({ sourceData: data.result, displayTemplate: "#tmpForGuaranteeStatus", selectionType: 'multiple', height: 310 });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.GetGuaranteeStatusRoutes, 'autoclose': true })
            }
        });

    }

    static InitialGuaranteeStatusRoute() {
        GuaranteeStatusRouteRequest.GetGuaranteeStatus().done(function (data) {
            if (data.success) {
                $('#selectGuaranteeStatus').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.GetGuaranteeStatusRoutes, 'autoclose': true })
            }
        });
    }

    static CopyGuaranteeStatus(data) {
        if ($("#listViewGuaranteeStatus").UifListView('getData').length > 0) {
            data = $("#listviewGuaranteeStatusRoute").UifListView('getData').concat(data);
            $("#listviewGuaranteeStatusRoute").UifListView({ sourceData: data, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForGuaranteeStatusAssign", selectionType: 'multiple', height: 310 });
            $("#listViewGuaranteeStatus").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForGuaranteeStatus", selectionType: 'multiple', height: 310 });
        }
    }

    static deallocateGuaranteeStatusRoute(data) {
        if ($("#listviewGuaranteeStatusRoute").UifListView('getData').length > 0) {
            data = $("#listViewGuaranteeStatus").UifListView('getData').concat(data);
            $("#listViewGuaranteeStatus").UifListView({ sourceData: data, displayTemplate: "#tmpForGuaranteeStatus", selectionType: 'multiple', height: 310 });
            $("#listviewGuaranteeStatusRoute").UifListView({ displayTemplate: "#tmpForGuaranteeStatusAssign", selectionType: 'multiple', height: 310 });
        }
    }

    static CopyGuaranteeStatusSelected(data) {
        
        var GuaranteeStatusAsign = $("#listviewGuaranteeStatusRoute").UifListView('getData');
        $.each(data, function (index, data) {
            data.IsSelected = true;

            var findGuaranteeStatus = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listViewGuaranteeStatus").UifListView("findIndex", findGuaranteeStatus);
            $("#listViewGuaranteeStatus").UifListView("deleteItem", index);
        });
        GuaranteeStatusAsign = GuaranteeStatusAsign.concat(data);
        $("#listviewGuaranteeStatusRoute").UifListView({ sourceData: GuaranteeStatusAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForGuaranteeStatusAssign", selectionType: 'multiple', height: 310 });
    }

    static deallocateGuaranteeStatusSelect(data) {
        var GuaranteeStatusNoAsign = $("#listViewGuaranteeStatus").UifListView('getData');
        $.each(data, function (index, data) {
            
            var findGuaranteeStatus = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listviewGuaranteeStatusRoute").UifListView("findIndex", findGuaranteeStatus);
            $("#listviewGuaranteeStatusRoute").UifListView("deleteItem", index);
        });
        GuaranteeStatusNoAsign = GuaranteeStatusNoAsign.concat(data);
        $("#listViewGuaranteeStatus").UifListView({ sourceData: GuaranteeStatusNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForGuaranteeStatus", selectionType: 'multiple', height: 310 });
    }

    static CancelView() {
        window.location = rootPath + "Home/Index";
    }

    static initialListView() {
        $("#listViewGuaranteeStatus").UifListView({ selectionType: 'multiple', height: 310 });
        $("#listviewGuaranteeStatusRoute").UifListView({ selectionType: 'multiple', height: 310 });
    }

    static SaveGuaranteeStatusRoute() { 
        var guaranteeStatusId = $("#selectGuaranteeStatus").val();
        if (guaranteeStatusId.trim() !== "") {
            var guaranteeStatusRouteAssign = $("#listviewGuaranteeStatusRoute").UifListView('getData');
            var allGuaranteeEstatusAssign = [];
            guaranteeStatusRouteAssign.forEach(function (item, index) {
                allGuaranteeEstatusAssign.push(item);
            })
            GuaranteeStatusRouteRequest.SaveGuaranteeStatusRoute(allGuaranteeEstatusAssign, guaranteeStatusId).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.SuccessSavingGuaranteeStatus, 'autoclose': true })
                    GuaranteeStatusRoute.initialListView();
                    GuaranteeStatusRoute.InitialGuaranteeStatusRoute();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSavingGuaranteeStatus, 'autoclose': true })
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorNotSelectedRegister, 'autoclose': true })
        }
        
    }
}