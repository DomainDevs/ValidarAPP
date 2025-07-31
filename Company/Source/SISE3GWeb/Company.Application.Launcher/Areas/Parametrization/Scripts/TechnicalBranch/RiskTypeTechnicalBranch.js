$(() => {

});


///Variables y Constantes
var LinesBusinessCoveredRiskType = {};
var Id_Risktype;
var Risktype;
var RiskTypeByLineBussiness = {};

class RiskTypeTechnicalBranch extends Uif2.Page {
    getInitialState() {

        $("#listViewRiskTypeTechnicalBranch").UifListView({
            displayTemplate: "#RiskTypeTechnicalBranchTemplate", delete: true, customAdd: true,
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
        $("#btnCancelModal").click(RiskTypeTechnicalBranch.clear);
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
            $.each($("#listViewRiskTypeTechnicalBranch").UifListView("getData"), function (key, value) {
                if (value.IdLineBusiness == Id_Risktype)
                    addRisk = false;
            });
            Risktype = $("#selectRiskTypeTechnical").UifSelect("getSelectedText")
            LinesBusinessCoveredRiskType.ListLineBusinessCoveredrisktype =
                {
                    IdLineBusiness: Id_Risktype,
                    IdRiskType: Risktype
                }
            if (addRisk)
                $("#listViewRiskTypeTechnicalBranch").UifListView("addItem", LinesBusinessCoveredRiskType.ListLineBusinessCoveredrisktype);
            RiskTypeTechnicalBranch.clear();
        }
    }

    /////Eliminar del list view de tipos de riesgo 
    //deleteItemToListView(event, data) {
    //    RiskTypeByLineBussiness = $("#listViewRiskTypeTechnicalBranch").UifListView('getData');
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