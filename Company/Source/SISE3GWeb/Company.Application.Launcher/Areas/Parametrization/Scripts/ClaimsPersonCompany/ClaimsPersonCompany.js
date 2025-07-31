//var lineBusinessId = null;
class ClaimsPersonCompany extends Uif2.Page
{
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
        ClaimsAssociationRequest.GetPrefixes().done(function (data) {
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
            ClaimsAssociationRequest.GetLinesBusinessByPrefixId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#selectLineBusiness').UifSelect({ sourceData: data.result });
                }
                ClaimsAssociationRequest.GetEstimationsType(selectedItem.Id).done(function (data) {
                    if (data.success) {
                        $('#selectSubLineBusiness').UifSelect({ sourceData: data.result });
                    }
                });
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
            ClaimsAssociationRequest.GetCoveragesByLineBusinessId($('#selectLineBusiness').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectCause').UifSelect({ sourceData: data.result });
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


  
    //SelectedSubLineBusiness(event, selectedItem) {
    //    if (selectedItem.Id > 0) {
    //        ConfigurationPanelsRequest.GetSubLineBusinessByLineBussinessId(selectedItem.Id).done(function (data) {
    //            if (data.success) {
    //                $('#selectCause').UifSelect({ sourceData: data.result });
    //            }
    //            else {
    //                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
    //            }
    //        });
    //    }
    //    else {
    //        $('#selectCause').UifSelect();
    //    }

    //    $('#tableCoverages').UifDataTable('clear');
    //    $('#tableCauseCoverages').UifDataTable('clear');
    //}

    SelectedCause(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsAssociation.GetCauseCoveragesByCauseId(
                $('#selectLineBusiness').UifSelect('getSelected'),
                $('#selectSubLineBusiness').UifSelect('getSelected'),
                selectedItem.Id);

            ClaimsAssociation.GetCauseCoveragesByCauseId(selectedItem.Id);
        }
        else {
            $('#tableCoverages').UifDataTable('clear');
            $('#tableCauseCoverages').UifDataTable('clear');
        }
    }

    CreateCauseCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var coverageDTO = {
                Id: selectedItem.Id
            };

            ClaimsAssociationRequest.CreateCauseCoverage($('#selectCause').UifSelect('getSelected'), coverageDTO).done(function (data) {
                if (data.success) {
                    ClaimsAssociation.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(
                        $('#selectLineBusiness').UifSelect('getSelected'),
                        $('#selectSubLineBusiness').UifSelect('getSelected'),
                        $('#selectCause').UifSelect('getSelected'));

                    ClaimsAssociation.GetCauseCoveragesByCauseId($('#selectCause').UifSelect('getSelected'));
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    DeleteCauseCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsAssociationRequest.DeleteCauseCoverage($('#selectCause').UifSelect('getSelected'), selectedItem.Id).done(function (data) {
                if (data.success) {
                    CauseCoverage.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(
                        $('#selectLineBusiness').UifSelect('getSelected'),
                        $('#selectSubLineBusiness').UifSelect('getSelected'),
                        $('#selectCause').UifSelect('getSelected'));

                    ClaimsAssociation.GetCauseCoveragesByCauseId($('#selectCause').UifSelect('getSelected'));
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBusinessId, subLineBusinessId, causeId) {
        ClaimsAssociationRequest.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBusinessId, subLineBusinessId, causeId).done(function (data) {
            if (data.success) {
                $('#tableCoverages').UifDataTable({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetCauseCoveragesByCauseId(causeId) {
        ClaimsAssociationRequest.GetCauseCoveragesByCauseId(causeId).done(function (data) {
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
