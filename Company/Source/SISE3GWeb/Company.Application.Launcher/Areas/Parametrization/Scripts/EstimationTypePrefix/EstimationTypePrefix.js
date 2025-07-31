var Id = null;
var dataSuccess = null;
var dataResult = null;
var prefixes = null;

class EstimationTypePrefix extends Uif2.Page {
    getInitialState() {
    }

    bindEvents() {
        $('#ddlEstimation').on('itemSelected', this.GetEstimationsType);
        $('#btnGrabar').on('click', this.SaveEstimationTypePrefix);
    };


    GetEstimationsType(event, selectedItem) {
        if ($('#ddlEstimation').val() != "") {
            $('#divTblEstimation').removeAttr("hidden");
            $('#divBtn').removeAttr("hidden");
            EstimationTypePrefix.GetLineBusinessByEstimation($('#ddlEstimation').val());
        }
        else {
            $('#divTblEstimation').attr("hidden", "hidden");
            $('#divBtn').attr("hidden", "hidden");
        };
    }

    static GetLineBusinessByEstimation(idEstimation) {


        EstimationTypePrefixRequest.GetPrefixes().done(function (data) {
            prefixes = data.data;

            EstimationTypePrefixRequest.GetLineBusinessByEstimation(idEstimation).done(function (data) {
                dataSuccess = data.success;
                dataResult = data.result;
                $('#tblEstimation').UifDataTable('clear');
                if (dataSuccess) {

                    for (var i = 0; i < prefixes.length; i++) {
                        $('#tblEstimation').UifDataTable('addRow', prefixes[i]);                   
                    }


                    Id=$("#tblEstimation thead tr:eq('0')  :contains('Id')").index();

                    if (Id == -1) {
                        Id = 0;
                    } 

                    for (var i = 0; i < 10; i++) {
                        for (var j = 0; j < dataResult.length; j++) {
                            if (dataResult[j].Id == parseInt($("#tblEstimation tbody tr:eq('" + i + "')  td:eq('"+Id+"')").text())) {
                                $("#tblEstimation tbody tr:eq('" + i + "')").removeClass('row-selected').addClass('row-selected');
                                $("#tblEstimation tbody tr:eq('" + i + "') td button span").removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                            }
                            else {
                                $('#tableClauses tbody tr:eq(' + i + ' )').removeClass('row-selected');
                                $('#tableClauses tbody tr:eq(' + i + ' ) td button span').removeClass('glyphicon glyphicon-unchecked');
                            }
                        }

                    }

                }
                $('.paginate_button').on('click', function () { settblEstimation() });
            });


        });
    }

    SaveEstimationTypePrefix() {
        var estimationTypeId = $('#ddlEstimation').val();
        var listPrefix = $("#tblEstimation").UifDataTable('getSelected');

        if (Id == null) {
            return null;
        }

        EstimationTypePrefixRequest.SaveEstimationTypePrefix(estimationTypeId, listPrefix).done(function (data) {
            if (data.success) {
                $.UifNotify('show', { 'type': 'success', 'message': 'Guardado con exito', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'warning', 'message': 'Ocurrió un error', 'autoclose': true });
            }
        });
    }
}

function settblEstimation() {
    if (dataResult == null) {
        return null;
    }
    for (var i = 0; i < 10; i++) {
        for (var j = 0; j < dataResult.length; j++) {
            if (dataResult[j].Id == parseInt($("#tblEstimation tbody tr:eq('" + i + "')  td:eq('" + Id +"')").text())) {
                $("#tblEstimation tbody tr:eq('" + i + "')").removeClass('row-selected').addClass('row-selected');
                $("#tblEstimation tbody tr:eq('" + i + "') td button span").removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
            }
            else {
                $('#tableClauses tbody tr:eq(' + i + ' )').removeClass('row-selected');
                $('#tableClauses tbody tr:eq(' + i + ' ) td button span').removeClass('glyphicon glyphicon-unchecked');
            }
        }

    }
}


