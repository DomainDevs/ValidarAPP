var currentEditIndex = 0;
var currentPrefixCumulusID = 0;

function RowModel() {
    this.Id = 0;
    this.Prefix = {Id: 0, Description: ""};
    this.PrefixCumulus = { Id: 0, Description: ""};
    this.ExerciseType = 0;
    this.TypeExerciceDescripcion = "";
    this.IsLocation = 0;
}


$('#tblprefixcumulus').on('rowAdd', function (event, data) {
    $("#alertPrefixCumulus").UifAlert('hide');
    $('#modalPrefixCumulus').UifModal('showLocal', Resources.New);
    $('#modalPrefixCumulus').find('PrefixCumulusID').val('0');
    currentPrefixCumulusID = 0;
    $("#modalPrefixCumulus").find("#chkEnabled").prop('checked', true);
});

$('#tblprefixcumulus').on('rowEdit', function (event, data, position) {
    $("#alertPrefixCumulus").UifAlert('hide');

    currentEditIndex = position;
    currentPrefixCumulusID = data.PrefixCumulusId;
    $('#editForm').find('#PrefixCumulusID').val(data.PrefixCumulusId);
    $('#editForm').find("#SelectLineBusiness").val(data.PrefixCD);
    $('#editForm').find("#SelectLineBusinessCumulus").val(data.PrefixCumulusCD);
    $('#editForm').find("#SelectTypeExercice").val(data.TypeExercice);

    $('#editForm').find("#SelectLineBusiness").UifSelect("disabled", true);
    $('#editForm').find("#SelectLineBusinessCumulus").UifSelect("disabled", true);

    if (data.Location) {
        if (!$("#modalPrefixCumulusEdit").find("#Enable").hasClass('glyphicon glyphicon-check')) {
            $("#modalPrefixCumulusEdit").find("#Enable").removeClass('glyphicon glyphicon-unchecked');
            $("#modalPrefixCumulusEdit").find("#Enable").addClass('glyphicon glyphicon-check');
        }
        $("#modalPrefixCumulusEdit").find("#chkEnabled").prop('checked', true);
    }
    else {
        if ($("#modalPrefixCumulusEdit").find("#Enable").hasClass('glyphicon glyphicon-check')) {
            $("#modalPrefixCumulusEdit").find("#Enable").removeClass('glyphicon glyphicon-check');
            $("#modalPrefixCumulusEdit").find("#Enable").addClass('glyphicon glyphicon-unchecked');
        }
        $("#modalPrefixCumulusEdit").find("#chkEnabled").prop('checked', false);
    }

    $('#modalPrefixCumulusEdit').UifModal('showLocal', Resources.Modify);
});

//-->evento click del check edicion
$("#modalPrefixCumulus").find("#Enable").on('click', function () {

    if ($("#modalPrefixCumulus").find("#Enable").hasClass('glyphicon glyphicon-check')) {

        $("#modalPrefixCumulus").find("#Enable").removeClass('glyphicon glyphicon-check');
        $("#modalPrefixCumulus").find("#Enable").addClass('glyphicon glyphicon-unchecked');
        if ($("#modalPrefixCumulus").find("#Enable").hasClass('glyphicon glyphicon-check')) {
            $("#modalPrefixCumulus").find("#chkEnabled").prop('checked', true);
        } else {
            $("#modalPrefixCumulus").find("#chkEnabled").prop('checked', false);
        }
    } else {
        $("#modalPrefixCumulus").find("#Enable").removeClass('glyphicon glyphicon-unchecked');
        $("#modalPrefixCumulus").find("#Enable").addClass('glyphicon glyphicon-check');
        if ($("#modalPrefixCumulus").find("#Enable").hasClass('glyphicon glyphicon-check')) {
            $("#modalPrefixCumulus").find("#chkEnabled").prop('checked', true);
        } else {
            $("#modalPrefixCumulus").find("#chkEnabled").prop('checked', false);
        }
    }
});

$("#modalPrefixCumulusEdit").find("#Enable").on('click', function () {

    if ($("#modalPrefixCumulusEdit").find("#Enable").hasClass('glyphicon glyphicon-check')) {
        $("#modalPrefixCumulusEdit").find("#Enable").removeClass('glyphicon glyphicon-check');
        $("#modalPrefixCumulusEdit").find("#Enable").addClass('glyphicon glyphicon-unchecked');

        if ($("#modalPrefixCumulusEdit").find("#Enable").hasClass('glyphicon glyphicon-check')) {

            $("#modalPrefixCumulusEdit").find("#chkEnabled").prop('checked', true);
        } else {
            $("#modalPrefixCumulusEdit").find("#chkEnabled").prop('checked', false);
        }
    } else {

        $("#modalPrefixCumulusEdit").find("#Enable").removeClass('glyphicon glyphicon-unchecked');
        $("#modalPrefixCumulusEdit").find("#Enable").addClass('glyphicon glyphicon-check');

        if ($("#modalPrefixCumulusEdit").find("#Enable").hasClass('glyphicon glyphicon-check')) {
            $("#modalPrefixCumulusEdit").find("#chkEnabled").prop('checked', true);
        } else {
            $("#modalPrefixCumulusEdit").find("#chkEnabled").prop('checked', false);
        }
    }
});

//-->boton guardar nuevo de los modales
$('#btnSave').click(function () {
    if (($('#addForm').find('#SelectLineBusiness').UifSelect('getSelected') === null) || $('#addForm').find('#SelectLineBusiness').UifSelect('getSelected') == "") {
        $.UifDialog('alert', { 'message': Resources.SelectPrefix, 'title': Resources.PrefixReinsurances });
        return false;
    }

    if (($('#addForm').find('#SelectLineBusinessCumulus').UifSelect('getSelected') === null) || $('#addForm').find('#SelectLineBusinessCumulus').UifSelect('getSelected') == "") {
        $.UifDialog('alert', { 'message': Resources.SelectPrefixCumulus, 'title': Resources.PrefixReinsurances });
        return false;
    }

    if (($('#addForm').find('#SelectTypeExercice').UifSelect('getSelected') === null) || $('#addForm').find('#SelectTypeExercice').UifSelect('getSelected') == "") {
        $.UifDialog('alert', { 'message': Resources.SelectTypeExercice, 'title': Resources.PrefixReinsurances });
        return false;
    }

    var tableData = $('#tblprefixcumulus').UifDataTable('getData');
    if (tableData.length > 0) {
        for (var i = 0; i < tableData.length; i++) {
            var prefix = $('#SelectLineBusiness').UifSelect("getSelected");

            //La validaciòn es sòlo por el primer Ramo 
            if (tableData[i].PrefixCD == prefix) {
                $.UifDialog('alert', { 'message': Resources.PrefixIsParameterized, 'title': Resources.PrefixReinsurances });
                return false;
            }
        }
    }

    $("#addForm").validate();
    if ($("#addForm").valid()) {
        var rowModel = new RowModel();
        
        rowModel.Id = 0;
        rowModel.Prefix.Id = $("#SelectLineBusiness").UifSelect("getSelected");
        rowModel.Prefix.Description = "";
        rowModel.PrefixCumulus.Id = $("#SelectLineBusinessCumulus").UifSelect("getSelected");
        rowModel.PrefixCumulus.Description = "";
        rowModel.ExerciseType = $("#SelectTypeExercice").UifSelect("getSelected");
        rowModel.TypeExerciceDescripcion = "";

        if ($("#modalPrefixCumulus").find("#chkEnabled").is(":checked")) {
            rowModel.IsLocation = true;
        }
        else {
            rowModel.IsLocation = false;
        }
                
        $.ajax({
            type: "POST",
            url: REINS_ROOT + "Parameter/SaveLineBusinessCumulus",
            data: JSON.stringify({ "reinsurancePrefixDTO": rowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#addForm').formReset();
                $("#modalPrefixCumulus").UifModal('hide');

                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.SaveSuccessfully, 'autoclose': true });
                    $("#tblprefixcumulus").UifDataTable({ source: REINS_ROOT + "Parameter/GetPrefixsCumulus" });
                }
                else {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.SaveError, 'autoclose': true });
                }
            }
        });
    }
});

$('#btnSaveEdit').click(function () {
    if (($('#editForm').find('#SelectLineBusiness').UifSelect('getSelected') === null) || $('#editForm').find('#SelectLineBusiness').UifSelect('getSelected') == "") {
        $.UifDialog('alert', { 'message': Resources.SelectPrefix, 'title': Resources.PrefixReinsurances });
        return false;
    }

    if (($('#editForm').find('#SelectLineBusinessCumulus').UifSelect('getSelected') === null) || $('#editForm').find('#SelectLineBusinessCumulus').UifSelect('getSelected') == "") {
        $.UifDialog('alert', { 'message': Resources.SelectPrefixCumulus, 'title': Resources.PrefixReinsurances });
        return false;
    }

    if (($('#editForm').find('#SelectTypeExercice').UifSelect('getSelected') === null) || $('#editForm').find('#SelectTypeExercice').UifSelect('getSelected') == "") {
        $.UifDialog('alert', { 'message': Resources.SelectTypeExercice, 'title': Resources.PrefixReinsurances });
        return false;
    }
       
    $('#editForm').validate();
    if ($('#editForm').valid()) {
        var rowModel = new RowModel();

        rowModel.Id = currentPrefixCumulusID; 
        rowModel.Prefix.Id = $('#editForm').find("#SelectLineBusiness").UifSelect("getSelected");
        rowModel.Prefix.Description = "";
        rowModel.PrefixCumulus.Id = $('#editForm').find("#SelectLineBusinessCumulus").UifSelect("getSelected");
        rowModel.PrefixCumulus.Description = "";
        rowModel.ExerciseType = $('#editForm').find("#SelectTypeExercice").UifSelect("getSelected");
        rowModel.TypeExerciceDescripcion = "";

        if ($("#editForm").find("#chkEnabled").is(":checked")) {
            rowModel.IsLocation = true;
        }
        else {
            rowModel.IsLocation = false;
        }

        $.ajax({
            type: "POST",
            url: REINS_ROOT + "Parameter/UpdateLineBusinessCumulus",
            data: JSON.stringify({ "reinsurancePrefixDTO": rowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                $('#editForm').formReset();
                $("#modalPrefixCumulusEdit").UifModal('hide');

                if (data.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.SaveSuccessfully, 'autoclose': true });
                    $("#tblprefixcumulus").UifDataTable({ source: REINS_ROOT + "Parameter/GetPrefixsCumulus" });
                }
                else {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.SaveError, 'autoclose': true });
                }
            }
        });
    }

});

$('#tblprefixcumulus').on('rowDelete', function (event, data, position) {
    $.UifDialog('confirm', {
        'message': Resources.MsgWantContinue, 'title': Resources.Delete
          
    }, function (result) {
        if (result) {
            var rowModel = new RowModel();
            rowModel.Id = data.PrefixCumulusId;
            
            $.ajax({
                type: "POST",
                url: REINS_ROOT + "Parameter/DeleteLineBusinessCumulus",
                data: JSON.stringify({ "reinsurancePrefixDTO": rowModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'success', 'message': Resources.RecordSuccessfullyDeleted, 'autoclose': true });

                        $("#tblprefixcumulus").UifDataTable({ source: REINS_ROOT + "Parameter/GetPrefixsCumulus" });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.BusinessExceptionMsj, 'autoclose': true });
                    }
                }
            });
        }

    });
});
