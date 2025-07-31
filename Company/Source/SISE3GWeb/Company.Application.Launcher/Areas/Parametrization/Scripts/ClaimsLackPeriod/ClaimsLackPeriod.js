class ClaimsLackPeriod extends Uif2.Page
{
    getInitialState() {
        ClaimsLackPeriod.GetPrefixes();
    }

    bindEvents() {
        $('#selectPrefix').on('itemSelected', this.SelectedPrefix);
        $('#selectCause').on('itemSelected', this.SelectedCause);
        $("#tblLackPeriod").on("rowAdd", ClaimsLackPeriod.addLackPeriod);
    }

    static GetPrefixes() {
        ClaimsLackPeriodRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    SelectedPrefix(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsLackPeriodRequest.GetCausesByPrefixId($('#selectPrefix').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectCause').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectCause').UifSelect();
        }

        $('#tblsubcause').UifDataTable('clear');
    }

    SelectedCause(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsLackPeriodRequest.GetCauseCoveragesByCauseId($('#selectCause').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $("#tblLackPeriod").UifDataTable({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectCause').UifSelect();
        }

        $('#tblsubcause').UifDataTable('clear');
    }
    static addLackPeriod () {

    }
    static getlackperiod() {

    }
    static deletelackperiod() {

    }
}
