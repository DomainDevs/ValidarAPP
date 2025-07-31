var glbCoverangeActivePanelEdit = null;
class ConfigurationPanels extends Uif2.Page {
    getInitialState() {
        ConfigurationPanels.GetPrefixes();
    }

    bindEvents() {
        $('#selectPrefixesofClaims').on('itemSelected', this.selectPrefix);
        $('#selecLineBusinessofClaims').on('itemSelected', this.selectLineBusiness);
        $('#selectSubLineBusinessofClaims').on('itemSelected', this.selectSubLineBusiness);
        $('#btnAdd').on('click', ConfigurationPanels.ClaimCoverageActivePanel);
        $('#btnSave').on('click', ConfigurationPanels.AddClaimCoverageActivePanel);
        $('#btnCancel').on('click', ConfigurationPanels.ClearActivePanelPrincipal);
        $('#btnCancels').on('click', this.ClearActivePanelPrincipalModal);


        $("#tblActivePanel").on('rowEdit', function (event, data, position) {
            ConfigurationPanels.EditClaimCoverageActivePanel(data, position);
        });
    }

    static GetPrefixes() {
        ConfigurationPanelsRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefixesofClaims').UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetLineBusinessByPrefixId(prefixId) {
        ConfigurationPanelsRequest.GetLineBusinessByPrefixId(prefixId).done(function (data) {
            if (data.success) {
                $('#selecLineBusinessofClaims').UifSelect({ sourceData: data.result });
                $("#tblActivePanel").UifDataTable('clear');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetSubLineBusinessByLineBussinessId(lineBusinessId) {
        ConfigurationPanelsRequest.GetSubLineBusinessByLineBussinessId(lineBusinessId).done(function (data) {
            if (data.success) {

                $('#selectSubLineBusinessofClaims').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId) {
        ConfigurationPanelsRequest.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId).done(function (data) {
            if (data.success) {
                var listActivePanel = [];
                $.each(data.result, function (index, value) {
                    var activePanel = {};
                    activePanel.PrintDescription = this.PrintDescription;
                    activePanel.IsEnabledThird = this.IsEnabledThird ? 'SI' : 'NO';
                    activePanel.IsEnabledDriver = this.IsEnabledDriver ? 'SI' : 'NO';
                    activePanel.IsEnabledThirdPartyVehicle = this.IsEnabledThirdPartyVehicle ? 'SI' : 'NO';
                    activePanel.IsEnabledAffectedProperty = this.IsEnabledAffectedProperty ? 'SI' : 'NO';
                    activePanel.CoverageId = this.CoverageId
                    activePanel.Rows = this.Rows;
                    listActivePanel.push(activePanel);
                });

                $('#tblActivePanel').UifDataTable({ sourceData: listActivePanel });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }


    static ClaimCoverageActivePanel() {    
        if ($("#formConfigurationPanels").valid()) {
            var subLineBusiness = $('#selectSubLineBusinessofClaims').val();
            var lineBusiness = $('#selecLineBusinessofClaims').val();
            ConfigurationPanels.GetCoveragesByLineBusinessIdSubLineBusiness(lineBusiness, subLineBusiness);          
        } else {
            $(".error").css('color', 'red');
        }      

    }

    static GetCoveragesByLineBusinessIdSubLineBusiness(lineBusinessId, subLineBusinessId, coverageId = null) {
        ConfigurationPanelsRequest.GetCoveragesByLineBusinessIdSubLineBusiness(lineBusinessId, subLineBusinessId).done(function (data) {

            if (data.success) {
                if (coverageId != null) {
                    $('#selectCoverage').UifSelect({
                        sourceData: data.result,
                        selectedId: coverageId
                    });
                }
                else {
                    var coverageAssigneds = $("#tblActivePanel").UifDataTable("getData");
                    var converageAvailables = null;

                    converageAvailables = data.result.filter(function (coverage) {
                        return !coverageAssigneds.some(x => x.CoverageId == coverage.Id);
                    });

                    $('#selectCoverage').UifSelect({
                        sourceData: converageAvailables
                    });
                }
              
                if (data.result.length == 0) {                  
                    $.UifNotify('show', { 'type': 'danger', 'message': "No hay coverturas disponibles.", 'autoclose': true });
                } else {
                    $('#activePanelModel').UifModal('showLocal', '');
                }

                

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }



    selectPrefix(event, selectedItem) {
        ConfigurationPanels.GetLineBusinessByPrefixId(selectedItem.Id);
    }

    selectLineBusiness(event, selectedItem) {
        ConfigurationPanels.GetSubLineBusinessByLineBussinessId(selectedItem.Id);
    }

    selectSubLineBusiness(event, selectedItem) {
        var subLineBusiness = $('#selectSubLineBusinessofClaims').val();
        var lineBusiness = $('#selecLineBusinessofClaims').val();

        ConfigurationPanels.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusiness, subLineBusiness);
    }

    static AddClaimCoverageActivePanel() {
        var claimsCoverange = {};
        var coverangeId = $('#selectCoverage').UifSelect("getSelected");
        if (coverangeId == "") {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe selecionar una cobertura", 'autoclose': true });
            return;
        }

        claimsCoverange.CoverageId = $('#selectCoverage').UifSelect("getSelected");
        claimsCoverange.IsEnabledDriver = $('#checkDriver').prop('checked');
        claimsCoverange.IsEnabledThird = $('#checkThird').prop('checked');
        claimsCoverange.IsEnabledThirdPartyVehicle = $('#checkThirdV').prop('checked');
        claimsCoverange.IsEnabledAffectedProperty = $('#checkAffectedProperty').prop('checked');
        claimsCoverange.subLineBusiness = $('#selectSubLineBusinessofClaims').val();
        claimsCoverange.lineBusiness = $('#selecLineBusinessofClaims').val();
        ConfigurationPanels.SaveClaimCoverageActivePanel(claimsCoverange);
    }

    static ClearActivePanel() {
        $('#checkDriver').prop('checked', false);
        $('#checkThird').prop('checked', false);
        $('#checkThirdV').prop('checked', false);
        $('#checkAffectedProperty').prop('checked', false);
        $('#selectCoverage').UifSelect("setSelected", null);
        glbCoverangeActivePanelEdit = null;
    }

    static EditClaimCoverageActivePanel(data, index) {

        glbCoverangeActivePanelEdit = data;
        glbCoverangeActivePanelEdit.Index = index;
        var subLineBusiness = $('#selectSubLineBusinessofClaims').val();
        var lineBusiness = $('#selecLineBusinessofClaims').val();
        ConfigurationPanels.GetCoveragesByLineBusinessIdSubLineBusiness(lineBusiness, subLineBusiness, data.CoverageId);

        $('#selectCoverage').val(data.CoverageId);

        if (data.IsEnabledDriver == "SI") {
            $('#checkDriver').prop("checked", true);
        }

        if (data.IsEnabledThird == "SI") {
            $('#checkThird').prop("checked", true);
        }

        if (data.IsEnabledThirdPartyVehicle == "SI") {
            $('#checkThirdV').prop("checked", true);
        }

        if (data.IsEnabledAffectedProperty == "SI") {
            $('#checkAffectedProperty').prop("checked", true);
        }
        $('#activePanelModel').UifModal('showLocal', '');
    }

    static SaveClaimCoverageActivePanel(activePanel) {
        if (glbCoverangeActivePanelEdit != null) {
            ConfigurationPanelsRequest.UpdateClaimCoverageActivePanel(activePanel).done(function (data) {
                if (data.success) {
                    $('#activePanelModel').UifModal('hide');
                    ConfigurationPanels.GetCoveragesByLineBusinessIdSubLineBusinessId(activePanel.lineBusiness, activePanel.subLineBusiness);
                    $('#tblActivePanel').UifSelect({ sourceData: data.result });
                    $.UifNotify('show', { 'type': 'success', 'message': AppResources.UpdateActivePanel, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            ConfigurationPanelsRequest.SaveClaimCoverageActivePanel(activePanel).done(function (data) {
                if (data.success) {
                    $('#activePanelModel').UifModal('hide');
                    ConfigurationPanels.GetCoveragesByLineBusinessIdSubLineBusinessId(activePanel.lineBusiness, activePanel.subLineBusiness);
                    $('#tblActivePanel').UifSelect({ sourceData: data.result });
                    $.UifNotify('show', { 'type': 'success', 'message': AppResources.CreateActivePanel, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }

        ConfigurationPanels.ClearActivePanel();
    }

    static ClearActivePanelPrincipal() {
        $('#selectPrefixesofClaims').UifSelect("setSelected", null);
        $('#selecLineBusinessofClaims').UifSelect("setSelected", null);
        $('#selectSubLineBusinessofClaims').UifSelect("setSelected", null);
        $("#tblActivePanel").UifDataTable('clear')
    }

     ClearActivePanelPrincipalModal() {
        $('#checkDriver').prop('checked', false);
        $('#checkThird').prop('checked', false);
         $('#checkThirdV').prop('checked', false);
         $('#checkAffectedProperty').prop('checked', false);
        $('#selectCoverage').UifSelect("setSelected", null);
        glbCoverangeActivePanelEdit = null;
    }
}