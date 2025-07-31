//var lineBusinessId = null;
class ClaimsAssociation extends Uif2.Page {
    getInitialState() {
        CauseCoverage.GetPrefixes();
    }

    bindEvents() {
        $('#selectPrefix').on('itemSelected', ClaimsAssociation.SelectedPrefix);
        $('#selectLineBusiness').on('itemSelected', ClaimsAssociation.SelectedLineBusiness);
        $('#selectConcept').on('itemSelected', ClaimsAssociation.SelectedCoverage);
        $('#selectCoverage').on('itemSelected', ClaimsAssociation.SelectedCoverage);
        $('#tblConcepts').on('rowSelected', ClaimsAssociation.CreatePaymentConcept);
        $('#tblConceptsAssigned').on('rowDelete', ClaimsAssociation.DeletePaymentConcept);
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

    static SelectedPrefix(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsAssociationRequest.GetLinesBusinessByPrefixId(selectedItem.Id).done(function (data) {
                if (data.success) {
                    $('#selectLineBusiness').UifSelect({ sourceData: data.result });
                }
                ClaimsAssociationRequest.GetEstimationsType(selectedItem.Id).done(function (data) {
                    if (data.success) {
                        $('#selectConcept').UifSelect({ sourceData: data.result });
                    }
                });
            });
        }
        else {
            $('#selectLineBusiness').UifSelect();
        }

        $('#selectConcept').UifSelect();
        $('#selectCoverage').UifSelect();
        $('#tblConcepts').UifDataTable('clear');
        $('#tblConceptsAssigned').UifDataTable('clear');
    }

    static SelectedLineBusiness(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClaimsAssociationRequest.GetCoveragesByLineBusinessId($('#selectLineBusiness').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectCoverage').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectConcept').UifSelect();
        }

        $('#selectCoverage').UifSelect();
        $('#tblConcepts').UifDataTable('clear');
        $('#tblConceptsAssigned').UifDataTable('clear');
    }

    static SelectedCoverage(event, selectedItem) {
        $("#formAssociationConceptsofPaymentCoverage").validate();
        if ($("#formAssociationConceptsofPaymentCoverage").valid()) {
            var coverageId = $("#selectCoverage").UifSelect('getSelected');
            var estimationTypeId = $("#selectConcept").UifSelect('getSelected');
            if (coverageId > 0 && estimationTypeId > 0) {
                ClaimsAssociation.GetPaymentConcepts();
            }
        }
        else {
            $('#tblConcepts').UifDataTable('clear');
            $('#tblConceptsAssigned').UifDataTable('clear');
        }
    }

    static CreatePaymentConcept(event, selectedItem, position) {
        if (selectedItem.Id > 0) {
            var coveragePaymentConceptDTO = {
                coverageId: $("#selectCoverage").UifSelect('getSelected'),
                estimationTypeId: $("#selectConcept").UifSelect('getSelected'),
                conceptId: $("#tblConcepts").UifDataTable('getSelected')[0].Id
            }

            ClaimsAssociationRequest.CreatePaymentConcept(coveragePaymentConceptDTO).done(function (data) {
                if (data.success) {
                    $("#tblConcepts").UifDataTable('deleteRow', position);
                    $("#tblConceptsAssigned").UifDataTable('addRow', selectedItem);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static DeletePaymentConcept(event, selectedItem, position) {
        if (selectedItem.Id > 0) {
            var coverageId = $("#selectCoverage").UifSelect('getSelected');
            var estimationTypeId = $("#selectConcept").UifSelect('getSelected');
            var conceptId = selectedItem.Id;
            ClaimsAssociationRequest.DeletePaymentConcept(coverageId, estimationTypeId, conceptId).done(function (data) {
                if (data.success) {
                    $("#tblConcepts").UifDataTable('addRow', selectedItem);
                    $("#tblConceptsAssigned").UifDataTable('deleteRow', position);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBusinessId, subLineBusinessId, causeId) {
        ClaimsAssociationRequest.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBusinessId, subLineBusinessId, causeId).done(function (data) {
            if (data.success) {
                $('#tblConcepts').UifDataTable({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPaymentConcepts() {
        $('#tblConcepts').UifDataTable('clear');
        $("#tblConceptsAssigned").UifDataTable('clear');
        ClaimsAssociationRequest.GetPaymentConcepts().done(function (data) {
            if (data.success) {
                var coverageId = $("#selectCoverage").UifSelect('getSelected');
                var estimationTypeId = $("#selectConcept").UifSelect('getSelected');
                var conceptsAvailables = null;
                var conceptsAssigned = null;
                var concepts = data.result;
                ClaimsAssociationRequest.GetPaymentConceptsByCoverageIdEstimationTypeId(coverageId, estimationTypeId).done(function (data) {
                    if (data.success) {
                        conceptsAssigned = data.result;
                        $('#tblConceptsAssigned').UifDataTable({ sourceData: conceptsAssigned });

                        conceptsAvailables = concepts.filter(function (concept) {
                            return !conceptsAssigned.some(x => x.Id == concept.Id);
                        });

                        $('#tblConcepts').UifDataTable({ sourceData: conceptsAvailables });
                    }
                    else {
                        $('#tblConcepts').UifDataTable('clear');
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $('#tblConcepts').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}
