class EstimationTypeStatus extends Uif2.Page {
    getInitialState() {
        EstimationTypeStatus.GetEstimationTypes();
        $("#listReason").UifListView({
            source: null,
            customDelete: true,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: true,
            displayTemplate: "#templateReason",
            title: Resources.Language.LabelReasons,
            height: 200
        });
    }

    bindEvents() {
        $('#selectEstimationTypes').on('itemSelected', this.SelectedEstimationTypes);
        $('#selectPrefix').on('itemSelected', EstimationTypeStatus.fillReasons);
        $('#tableStatuses').on('rowEdit', this.ShowModalEdit);
        $('#tableStatuses').on('rowDelete', this.DeleteStatusByEstimationType);
        $('#btnNew').on('click', this.ShowModalCreate);
        $("#btnReasons").click(this.BtnReasons);
        $("#btnSendStatus").click(this.CreateStatusByEstimationType);

        /* Razones */
        $("#btnAddReason").click(this.addReasons);
        $("#btnCancelEditReason").click(this.cancelReason);
        $("#listReason").on('rowEdit', this.editReason);
        $("#listReason").on('rowDelete', this.deleteReason);
        $("#btnSaveReason").click(this.saveReasons);
    }

    static GetEstimationTypes() {
        EstimationTypeStatusRequest.GetEstimationTypes().done(function (data) {
            if (data.success) {
                $('#selectEstimationTypes').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetPrefixes() {
        EstimationTypeStatusRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    SelectedEstimationTypes(event, selectedItem) {
        $('#tableStatuses').UifDataTable('clear');
        if (selectedItem.Id > 0) {
            EstimationTypeStatus.GetStatusesByEstimationTypeId(selectedItem.Id);
        }
    }

    ShowModalEdit(event, result, index) {
        if (result) {
            $("#lblId").val(result.Id);
            $("#txtDescription").val(result.Description);
            $("#txtInternalCode").val(result.InternalCode);

            $("#modalEstimationTypeStatus").UifModal("showLocal", Resources.Language.EstimationTypeStatus);
        }
    }

    /* guardar estado */
    CreateStatusByEstimationType() {
        $("#formModalEstimationtypeStatus").validate();
        if ($("#formModalEstimationtypeStatus").valid()) {
            var statusDTO = {
                Id: $('#selectEstimationTypeStatus').UifSelect("getSelected"),
                EstimationType:
                {
                    Id: $('#selectEstimationTypes').UifSelect("getSelected")
                }
            };

            EstimationTypeStatusRequest.CreateStatusByEstimationType(statusDTO).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInformation, 'autoclose': true });
                    EstimationTypeStatus.GetStatusesByEstimationTypeId($('#selectEstimationTypes').UifSelect("getSelected"));
                    EstimationTypeStatus.HideModalEstimationTypeStatus();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    DeleteStatusByEstimationType(data, selectedItem) {
        $.UifDialog('confirm', { 'message': Resources.Language.MessageConfirmDelete }, function (result) {
            if (result) {
                var statusDTO = {
                    Id: selectedItem.Id,
                    EstimationType:
                    {
                        Id: $('#selectEstimationTypes').UifSelect("getSelected")
                    }
                };

                EstimationTypeStatusRequest.DeleteStatusByEstimationType(statusDTO).done(function (data) {
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MsgCorrectRemoval, 'autoclose': true });
                        EstimationTypeStatus.GetStatusesByEstimationTypeId($('#selectEstimationTypes').UifSelect("getSelected"));
                        EstimationTypeStatus.HideModalEstimationTypeStatus();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        });
    }

    BtnReasons() {
        if ($("#tableStatuses").UifDataTable("getSelected") == null) {
            $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorSelectAStatus, 'autoclose': true });
            return;
        }
        EstimationTypeStatus.GetPrefixes();
        EstimationTypeStatus.clearReason();
        $("#modalReason").UifModal("showLocal", Resources.Language.StatusReason);
    }

    static fillReasons() {
        EstimationTypeStatusRequest.GetReasonsByStatusIdPrefixId($('#tableStatuses').UifDataTable("getSelected")[0].Id, $("#selectPrefix").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                $("#listReason").UifListView("refresh");
                $.each(data.result, function (entryIndex, entry) {
                    $("#listReason").UifListView("addItem", this);
                });

                $("#btnAddReason").show();
                $("#btnSaveReason").hide();
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
    }

    /* Limpiar formularo de razones */
    static clearReason() {
        $("#inputDescription").val("");
        $("#lblIdEstimationTypeStatus").val("0");
        $("#lblIdReason").val("0");
        $("#btnAddReason").show();
        $("#btnSaveReason").hide();
        $("#listReason").UifListView('clear');
        $("#selectPrefix").UifSelect("setSelected", null);
    }

    /**
     * evento del listview razones
     * @param {any} event
     * @param {any} result
     * @param {any} index
     */
    editReason(event, result, index) {
        $("#lblIdEstimationTypeStatus").val($('#tableStatuses').UifDataTable("getSelected")[0].Id);
        $("#lblIdReason").val(result.Id);
        $("#inputDescription").val(result.Description);
        $("#btnAddReason").hide();
        $("#btnSaveReason").show();
    }

    /*Create Reason*/
    addReasons() {
        $("#formReason").validate();
        if ($("#formReason").valid()) {
            var reasonDTO = {
                Id: 0,
                Description: $("#inputDescription").val(),
                EstimationTypeStatusId: $('#tableStatuses').UifDataTable("getSelected")[0].Id,
                PrefixId: $("#selectPrefix").UifSelect("getSelected"),
                Enabled: $("#enabledEstimationType").is(':checked')
            }
            EstimationTypeStatusRequest.CreateReason(reasonDTO).done(function (data) {
                if (data.success) {
                    EstimationTypeStatus.clearReason();
                    EstimationTypeStatus.fillReasons();
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    /*Update Reason*/
    saveReasons() {
        $("#formReason").validate();
        if ($("#formReason").valid()) {
            var reasonDTO = {
                Id: $("#lblIdReason").val(),
                Description: $("#inputDescription").val(),
                EstimationTypeStatusId: $('#tableStatuses').UifDataTable("getSelected")[0].Id,
                PrefixId: $("#selectPrefix").UifSelect("getSelected"),
                Enabled: $("#enabledEstimationType").is(':checked')
            }

            EstimationTypeStatusRequest.UpdateReason(reasonDTO).done(function (data) {
                if (data.success) {
                    EstimationTypeStatus.clearReason();
                    EstimationTypeStatus.fillReasons();
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
                }
            });
        }
    }

    /*Delete Reason*/
    deleteReason(event, result, index) {
        $("#lblIdReason").val(result.Id);
        $.UifDialog('confirm', { 'message': Resources.Language.MessageConfirmDelete }, function (result) {
            if (result) {
                EstimationTypeStatusRequest.DeleteReason($("#lblIdReason").val(), $('#tableStatuses').UifDataTable("getSelected")[0].Id, $("#selectPrefix").UifSelect("getSelected")).done(function (data) {
                    if (data.success) {
                        EstimationTypeStatus.clearReason();
                        EstimationTypeStatus.fillReasons();
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
            }
        });
    }

    /**
     * Boton Cancelar formulario razones
     * @param {any} event
     * @param {any} result
     * @param {any} index
     */
    cancelReason(event, result, index) {
        $("#inputDescription").val("");
        $("#lblIdReason").val("0");
        $("#lblIdEstimationTypeStatus").val("0");
        $("#btnAddReason").show();
        $("#btnSaveReason").hide();
        $('#enabledEstimationType').val("");
    }

    /* Modal crear estado */
    ShowModalCreate() {
        $("#formEstimationType").validate();
        if ($("#formEstimationType").valid()) {
            $('#lblId').val("0");
            var estimationTypeId = $("#selectEstimationTypes").val();
            if (estimationTypeId.trim() !== "") {
                EstimationTypeStatus.GetEstimationTypeStatusUnassignedByEstimationTypeId(estimationTypeId).then(function (data) {
                    if (data.length > 0) {
                        $("#modalEstimationTypeStatus").UifModal("showLocal", Resources.Language.EstimationTypeStatus);
                    }
                    else
                    {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AllEstimationTypeStatusesAssined, 'autoclose': true });
                    }
                });
            }
        }
    }

    /* ocultar modal crear estado */
    static HideModalEstimationTypeStatus() {
        $('#lblId').val("0");
        $('#txtDescription').val("");
        $('#txtInternalCode').val("");
        $("#modalEstimationTypeStatus").UifModal("hide");
    }


    /**
     * Traer estados por contepto de estimacion
     * @param {any} estimationTypeId
     */
    static GetStatusesByEstimationTypeId(estimationTypeId) {
        EstimationTypeStatusRequest.GetStatusesByEstimationTypeId(estimationTypeId).done(function (data) {
            if (data.success) {
                $('#tableStatuses').UifDataTable('clear');
                $.each(data.result, function (index, value) {
                    this.Enabled = this.Enabled ? Resources.Language.Enabled : Resources.Language.Disabled;
                });
                $('#tableStatuses').UifDataTable({ sourceData: data.result });
            }
            else {
                $('#tableStatuses').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }
    /* Traer todos los estados para relacionar con tipo de estimacion */
    static GetEstimationTypeStatusUnassignedByEstimationTypeId(estimationTypeId) {
        return new Promise(function (success) {
            EstimationTypeStatusRequest.GetEstimationTypeStatusUnassignedByEstimationTypeId(estimationTypeId).done(function (data) {
                if (data.success) {
                    $('#selectEstimationTypeStatus').UifSelect({ sourceData: data.result });
                    success(data.result);
                }
                else {
                    $('#selectEstimationTypeStatus').UifDataTable('clear');
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        });
    }
}