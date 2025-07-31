var oLine = {
    LineId: 0,
    CumulusType: {
        CumulusTypeId: 0
    },
    Description: ""
};

$("#CumulusTypeId").click(function () {
    $('#msgValidateCumulusType').UifAlert('hide');
});

function acceptDialogModalLine() {
    var lineExist = false;
    $("#formLine").validate();
    if ($("#formLine").valid()) {

        //ARMA EL OBJETO
        oLine.LineId = $("#LineId").val();
        oLine.CumulusType.CumulusTypeId = $('#CumulusTypeId').val();
        oLine.Description = $("#Description").val();

        var lstLines = $("#tableLine").UifDataTable("getData");
        $.each(lstLines, function (index, value) {
            if ((oLine.LineId != value.LineId) && ($.trim(value.Description) == $.trim(oLine.Description)) && (oLine.CumulusType.CumulusTypeId == value.CumulusTypeId)) {
                lineExist = true;
            }
        });

        if (lineExist) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.DuplicateLineDescription, 'autoclose': true });
        }
        else {
            SaveLine(oLine).done(function (data) {
                if (data.success) {
                    onAddComplete(data);
                }
            });
        }
    }
};

function SaveLine(oLine) {
    return $.ajax({
        type: "POST",
        url: REINS_ROOT + "Parameter/SaveLine",
        data: JSON.stringify({ lineDTO: oLine }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}
