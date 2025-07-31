var aditionalInformation = {};

class AditionalInformation extends Uif2.Page {

    

    getInitialState() {
        AditionalInformation.SetAditionalInformation(aditionalInformation);
    }

    bindEvents() {
        $("#IsScore").change(AditionalInformation.ValidateCheckScore);

        $('#btnAceptAditionalInformation').on('click', this.SaveAditionalInformation);
		$("#IsAlliance").prop("disabled", true);
		$("#IsMassive").prop("disabled", true);

    }

    static DisabledScore() {
        return !$("#IsScore").is(':checked');
    }

    static ValidateCheckScore() {
        $("#Score").prop('disabled', AditionalInformation.DisabledScore());
        if ($("#IsScore").is(':checked') == false) {
            $("#Score").val(null);
        }
    }

    static SetDefaultInformation() {
        aditionalInformation = {
			IsScore: false,
			IsAlliance: false,
			IsMassive: false,
            Score: null,
            Quote: null,
            Temporal: null
        };
        AditionalInformation.SetAditionalInformation(aditionalInformation);
        return aditionalInformation;
    }

    static GetAditionalInformation() {
        if (aditionalInformation == null || jQuery.isEmptyObject(aditionalInformation)) {
            return null;
        }
        return aditionalInformation;
    }

    static SetAditionalInformation(information) {
        if (information != null || !jQuery.isEmptyObject(information)) {
            $("#IsScore").prop('checked', information.IsScore);
            $("#IsAlliance").prop('checked', information.IsAlliance);
            $("#IsMassive").prop('checked', information.IsMassive);
            $("#Score").val(information.Score);
            $("#Quote").val(information.Quote);
            $("#Temporal").val(information.Temporal);
        }
        AditionalInformation.ValidateCheckScore();
        aditionalInformation = information;
    }

    static OpenAditionalInformation() {
        if (aditionalInformation == null || jQuery.isEmptyObject(aditionalInformation)) {
            aditionalInformation = AditionalInformation.SetDefaultInformation();
        }
        AditionalInformation.SetAditionalInformation(aditionalInformation);
        $("#modalAditionalInformation").UifModal('showLocal', 'Datos Adicionales');

    }

    SaveAditionalInformation() {
        aditionalInformation = {
            IsScore: $("#IsScore").is(':checked'),
            IsAlliance: $("#IsAlliance").is(':checked'),
            IsMassive: $("#IsMassive").is(':checked'),
            Score: $("#Score").val(),
            Quote: $("#Quote").val(),
            Temporal: $("#Temporal").val()
        };
        $("#modalAditionalInformation").UifModal('hide');

    }
}
