class CauseCoverage extends Uif2.Page {
    getInitialState() {
        CauseCoverage.GetPrefixes();
    }

    bindEvents() {
        $('#selectPrefix').on('itemSelected', this.SelectedPrefix);
        $('#selectLineBusiness').on('itemSelected', this.SelectedLineBusiness);
        $('#selectSubLineBusiness').on('itemSelected', this.SelectedSubLineBusiness);
        $('#selectCause').on('itemSelected', this.SelectedCause);
        $('#tableCoverages').on('rowSelected', this.CreateCauseCoverage);
        $('#tableCauseCoverages').on('rowSelected', this.DeleteCauseCoverage);
    }

    static GetPrefixes() {
        CauseCoverageRequest.GetPrefixes().done(function (data) {
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
            CauseCoverageRequest.GetLinesBusinessByPrefixId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#selectLineBusiness').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectLineBusiness').UifSelect();
        }

        $('#selectSubLineBusiness').UifSelect();
        $('#selectCause').UifSelect();
        $('#tableCoverages').UifDataTable('clear');
        $('#tableCauseCoverages').UifDataTable('clear');
    }

    SelectedLineBusiness(event, selectedItem) {
        if (selectedItem.Id > 0) {
            CauseCoverageRequest.GetSubLinesBusinessByLineBusinessId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#selectSubLineBusiness').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectSubLineBusiness').UifSelect();
        }

        $('#selectCause').UifSelect();
        $('#tableCoverages').UifDataTable('clear');
        $('#tableCauseCoverages').UifDataTable('clear');
    }

    SelectedSubLineBusiness(event, selectedItem) {
        if (selectedItem.Id > 0) {
            CauseCoverageRequest.GetCausesByPrefixId($('#selectPrefix').UifSelect('getSelected')).done(function (data) {
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

        $('#tableCoverages').UifDataTable('clear');
        $('#tableCauseCoverages').UifDataTable('clear');
    }

    SelectedCause(event, selectedItem) {
        if (selectedItem.Id > 0) {
            CauseCoverage.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(
                $('#selectLineBusiness').UifSelect('getSelected'),
                $('#selectSubLineBusiness').UifSelect('getSelected'),
                selectedItem.Id);

            CauseCoverage.GetCauseCoveragesByCauseId(selectedItem.Id);
        }
        else {
            $('#tableCoverages').UifDataTable('clear');
            $('#tableCauseCoverages').UifDataTable('clear');
        }
    }

    CreateCauseCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var coverageDTO = {
                Id: selectedItem.Id,
                Description: selectedItem.Description,
                CoverageId: selectedItem.CoverageId,
                InsuredObjectId: selectedItem.InsuredObjectId,
                SubLineBusinessCode: selectedItem.SubLineBusinessCode
            };

            CauseCoverageRequest.CreateCauseCoverage($('#selectCause').UifSelect('getSelected'), coverageDTO).done(function (data) {
                if (data.success) {
                    CauseCoverage.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(
                        $('#selectLineBusiness').UifSelect('getSelected'),
                        $('#selectSubLineBusiness').UifSelect('getSelected'),
                        $('#selectCause').UifSelect('getSelected'));

                    CauseCoverage.GetCauseCoveragesByCauseId($('#selectCause').UifSelect('getSelected'));
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    DeleteCauseCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            CauseCoverageRequest.DeleteCauseCoverage($('#selectCause').UifSelect('getSelected'), selectedItem.Id).done(function (data) {
                if (data.success) {
                    CauseCoverage.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(
                        $('#selectLineBusiness').UifSelect('getSelected'),
                        $('#selectSubLineBusiness').UifSelect('getSelected'),
                        $('#selectCause').UifSelect('getSelected'));

                    CauseCoverage.GetCauseCoveragesByCauseId($('#selectCause').UifSelect('getSelected'));
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBusinessId, subLineBusinessId, causeId) {
        CauseCoverageRequest.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBusinessId, subLineBusinessId, causeId).done(function (data) {
            if (data.success) {
                $('#tableCoverages').UifDataTable({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetCauseCoveragesByCauseId(causeId) {
        CauseCoverageRequest.GetCauseCoveragesByCauseId(causeId).done(function (data) {
            if (data.success) {
                $('#tableCauseCoverages').UifDataTable({ sourceData: data.result });
            }
            else {
                $('#tableCauseCoverages').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}