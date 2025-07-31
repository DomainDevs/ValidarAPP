var vehicleUseSeleted = [];
$.ajaxSetup({ async: true });
class ParametrizationVehicleUse extends Uif2.Page {
	getInitialState() {

	}

	bindEvents() {

	}

	static OpenVehicleUse(vehicleBodyDescription) {
		$("#modalUses").UifModal('showLocal', 'Asociar carrocería');
		$("#inputVehicleBodyCode").val(vehicleBodyDescription);
	}

	static SaveUses() {
		var uses = [];
		var uses_v = [];
		var mensage = Resources.Language.Assignmenterror;
		uses_v  = $("#tableVehicleUse").UifDataTable("getSelected");
		if (uses_v == null) {
			$.UifNotify('show', {
				'type': 'danger',
				'message': mensage,
				'autoclose': false
			});
		} else {

			$.each($("#tableVehicleUse").UifDataTable("getSelected"), function (key, value) {
				uses.push(value.Id);

			});
			$('#modalUses').UifModal("hide");
			return uses;
		}
	}


	static ClearUses() {
		$.each($("#tableVehicleUse").UifDataTable("getData"), function (key, value) {
			$('#tableVehicleUse tbody tr:eq(' + key + ')').removeClass('row-selected');

			if ($('#tableVehicleUse tbody tr:eq(' + key + ') td button span').hasClass('glyphicon-check')) {
				$('#tableVehicleUse tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
			}
		});
	}

	static SelectUses(uses) {
		vehicleUseSeleted = uses;
		if (uses != null || uses != undefined) {
			$.each($("#tableVehicleUse").UifDataTable("getData"), function (key, value) {

				var result = vehicleUseSeleted.find(function (element) {
					return element == value.Id;
				});
				if (result != undefined) {
					$('#tableVehicleUse tbody tr:eq(' + key + ')').addClass('row-selected');
					$('#tableVehicleUse tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
				}
			})
		}
	}
}
