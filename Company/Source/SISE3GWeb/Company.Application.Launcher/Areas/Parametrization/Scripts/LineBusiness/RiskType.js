$(() => {

});


///Variables y Constantes
var LinesBusinessCoveredRiskType = {};
var Id_Risktype;
var Risktype;
var RiskTypeByLineBussiness = {};

class RiskTypeLineBusiness extends Uif2.Page {
    getInitialState() {

        $("#listViewRiskTypeLineBusiness").UifListView({
            displayTemplate: "#RiskTypeLineBusinessTemplate", delete: true, customAdd: true,
            deleteCallback: (deferred, data) => { deferred.resolve(); }
            , height: 300
        });
        //CoveredRiskType.GetRiskType().done(function (data) {
        //    if (data.success) {
        //        $('#selectRiskTypeTechnical').UifSelect({ sourceData: data.result });
        //    }
        //    else {
        //        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        //    }
        //});
    }

    bindEvents() {
        $("#btnCancelModal").click(RiskTypeLineBusiness.clear);
        $("#btnSaveModal").click(this.saveToListView);
    }
    static clear() {
        $("#selectRiskTypeTechnical").UifSelect("setSelected", null);
        $("#selectRiskTypeTechnical").UifSelect("disabled", false);
    }

    ///-------Guardar en List View RiskType
    saveToListView(event, selectedItem) {
        var addRisk = true;
        Id_Risktype = $("#selectRiskTypeTechnical").UifSelect("getSelected")
        if (Id_Risktype != "") {
            $.each($("#listViewRiskTypeLineBusiness").UifListView("getData"), function (key, value) {
                if (value.IdLineBusiness == Id_Risktype)
                    addRisk = false;
            });
            Risktype = $("#selectRiskTypeTechnical").UifSelect("getSelectedText")
            LinesBusinessCoveredRiskType.ListLineBusinessCoveredrisktype =
                {
                    Id: Id_Risktype,
                    IdRiskType: Risktype
                }
            if (addRisk)
                $("#listViewRiskTypeLineBusiness").UifListView("addItem", LinesBusinessCoveredRiskType.ListLineBusinessCoveredrisktype);
            RiskTypeLineBusiness.clear();
        }
    }

    /////Eliminar del list view de tipos de riesgo 
    //deleteItemToListView(event, data) {
    //    RiskTypeByLineBussiness = $("#listViewRiskTypeLineBusiness").UifListView('getData');
    //    $.each(RiskTypeByLineBussiness, function (index, value) {
    //        //if () {
    //        //    value.Status = 'Deleted';
    //        //    glbSubBranchDelete.push(value);
    //        //}
    //        //else {
    //        //    $("#listViewSearchAdvanced").UifListView("addItem", this);
    //        //}
    //    });
    //    ParametrizationTechnicalSubBranch.clear();
    //}
}