
var vehicleBodySeleted = [];
class ParametrizationVehicleBody extends Uif2.Page {
    
    getInitialState() {

    }
    bindEvents() {

    }


    static OpenVehicleBody(vehicleTypeDescription) {
        $("#modalBodies").UifModal('showLocal', 'Asociar carrocería');
		$("#inputVehicleBodeTypeCode").val(vehicleTypeDescription);
    }

    static SaveBodies() {

        var bodies = [];
        $.each($("#tableVehicleBody").UifDataTable("getSelected"), function (key, value) {
            bodies.push(value.Id);
        });
        $('#modalBodies').UifModal("hide");
        return bodies;
    }

    static ClearBodies() {
        $.each($("#tableVehicleBody").UifDataTable("getData"), function (key, value) {
            
            $('#tableVehicleBody tbody tr:eq(' + key + ')').removeClass('row-selected');

            if ($('#tableVehicleBody tbody tr:eq(' + key + ') td button span').hasClass('glyphicon-check')) {
                $('#tableVehicleBody tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
            }

        });
    }

    static SelectBodies(bodies) {
        vehicleBodySeleted = bodies;
        if (bodies != null || bodies != undefined) {
            $.each($("#tableVehicleBody").UifDataTable("getData"), function (key, value) {
                
                var result = vehicleBodySeleted.find(function (element) {
                    return element == value.Id;
                });
                if (result != undefined)
                {
                    $('#tableVehicleBody tbody tr:eq(' + key + ')').addClass('row-selected');
                    $('#tableVehicleBody tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
                }
            })
        }
    }
    
}
