var ProtectionAll = [];
var ProtectionAssing = [];
var glbProtectionsAll = [];
var glbProtectionsAssing = [];
var ProtectionAssignOriginal = [];
var ProtectionAllOriginal = [];
var protection = {};
var w;
$(() => {
    new ParametrizationProtections();
});


class ProtectionControls {
    static get ProtectionTemplate() { return "#ProtectionsTemplate"; }
    static get listProtectionAll() { return $("#listviewProtectionTechnicalBranch"); }
    static get listProtectionAssing() { return $("#listviewProtectionTechnicalBranchAssing"); }
    static get btnAssignAll() { return $("#btnModalProtectionTechnicalBranchAssignAll"); }
    static get btnDeallocateAll() { return $("#btnModalProtectionTechnicalBranchDeallocateAll"); }
    static get btnAssign() { return $("#btnModalProtectionTechnicalBranchAssign"); }
    static get btnDeallocate() { return $("#btnModalProtectionTechnicalBranchDeallocate"); }
    static get btnCloseProtection() { return $("#btnCloseProtection"); }
    static get btnSaveProtection() { return $("#btnSaveProtection"); }
    static get ModalProtection() { return $("#ModalProtection"); }
}


class Protections {
    static GetPerilsAssign(idLineBusiness) {
        return $.ajax({
            type: 'POST',
            url: 'GetPerilsAssign/' + idLineBusiness,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}



class ParametrizationProtections extends Uif2.Page {
    getInitialState() {
        ProtectionControls.listProtectionAll.UifListView({ displayTemplate: ProtectionControls.ProtectionTemplate, edit: false, delete: false, customAdd: false, customEdit: true, customDelete: false, height: 300 });
        ProtectionControls.listProtectionAssing.UifListView({ displayTemplate: ProtectionControls.ProtectionTemplate, edit: false, delete: false, customAdd: false, customEdit: true, customDelete: false, height: 300 });
        Protections.GetPerilsAssign(0).done(function (data) {
            if (data.success) {
                glbProtectionsAll = data.result.PerilNotAssign;
                glbProtectionsAssing = data.result.PerilAssign;
                ProtectionAssignOriginal = data.result.PerilAssign;
                ProtectionAllOriginal = data.result.PerilNotAssign;
                ParametrizationProtections.loadProtections();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetAssigned() {
        return glbProtectionsAssing;
    }

    static ModalProtectionLoadFirstTime(id) {
        Protections.GetPerilsAssign(id).done(function (data) {
            if (data.success) {
                glbProtectionsAll = data.result.PerilNotAssign;
                glbProtectionsAssing = data.result.PerilAssign;
                ProtectionAssignOriginal = data.result.PerilAssign;
                ProtectionAllOriginal = data.result.PerilNotAssign;
                ParametrizationProtections.loadProtections();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {
        ProtectionControls.btnAssignAll.click(this.AssignAll);
        ProtectionControls.btnAssign.click(this.AssignProtection);
        ProtectionControls.btnDeallocateAll.click(this.ProtectionDeallocateAll);
        ProtectionControls.btnDeallocate.click(this.ProtectionDeallocate);
        ProtectionControls.btnCloseProtection.click(this.CloseProtection);
        ProtectionControls.btnSaveProtection.click(this.SaveProtectionAssigned);
        
    }

    
    CloseProtection() {
        glbProtectionsAssing = ProtectionAssignOriginal;
        glbProtectionsAll = ProtectionAllOriginal;
        ParametrizationProtections.loadProtections();
    }

    SaveProtectionAssigned() {
        var lstProteccionObjects = $("#listviewProtectionTechnicalBranchAssing").UifListView('getData');
        if (lstProteccionObjects != null && lstProteccionObjects.length > 0) {
           
            ProtectionControls.ModalProtection.UifModal("hide");
            if ($('#listviewProtectionTechnicalBranchAssing').UifListView('getData').length > 0) {
                $("#selectedProtections").text($('#listviewProtectionTechnicalBranchAssing').UifListView('getData').length);
            }
            listAsignedPeril = lstProteccionObjects;
            ParametrizationProtections.RefreshList();
        }
    }

    AssignAll() {
        ParametrizationProtections.CopyClauses(ProtectionControls.listProtectionAll.UifListView('getData'));
        ParametrizationProtections.RefreshList();
    }

    static CopyClauses(data) {
        if (ProtectionControls.listProtectionAll.UifListView('getData').length > 0) {
            data = ProtectionControls.listProtectionAssing.UifListView('getData').concat(data);
            ProtectionControls.listProtectionAssing.UifListView({ sourceData: data, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
            ProtectionControls.listProtectionAll.UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
        }
    }

    AssignProtection() {
        if (ProtectionControls.listProtectionAll.UifListView('getData').length > 0) {
            ProtectionControls.listProtectionAssing.prop("disabled", true);
            try {
                ParametrizationProtections.CopyProtectionSelected(ProtectionControls.listProtectionAll.UifListView('getSelected'))
            } catch (e) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMovingPaymentPlan, 'autoclose': true });
            } finally {
                ProtectionControls.btnAssign.prop("disabled", false);
            }
            ParametrizationProtections.RefreshList();
        }
    }

    static CopyProtectionSelected(data) {
        var ProtectionNoAsign = ProtectionControls.listProtectionAll.UifListView('getData');
        var ProtectionAsign = ProtectionControls.listProtectionAssing.UifListView('getData');
        $.each(data, function (index, data) {
            var findClause = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = ProtectionControls.listProtectionAll.UifListView("findIndex", findClause);
            ProtectionControls.listProtectionAll.UifListView("deleteItem", index);
        });
        ProtectionAsign = ProtectionAsign.concat(data);
        ProtectionControls.listProtectionAssing.UifListView({ sourceData: ProtectionAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
    }

    ProtectionDeallocateAll() {
        ParametrizationProtections.deallocateClauseAll(ProtectionControls.listProtectionAssing.UifListView('getData'));
        ParametrizationProtections.RefreshList();
    }

    static deallocateClauseAll(data) {
        if (ProtectionControls.listProtectionAssing.UifListView('getData').length > 0) {
            data = ProtectionControls.listProtectionAll.UifListView('getData').concat(data);
            ProtectionControls.listProtectionAll.UifListView({ sourceData: data, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
            ProtectionControls.listProtectionAssing.UifListView({ displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
        }
    }

    ProtectionDeallocate() {
        if (ProtectionControls.listProtectionAssing.UifListView('getData').length > 0) {
            ParametrizationProtections.deallocateClauseSelect(ProtectionControls.listProtectionAssing.UifListView('getSelected'))
            ParametrizationProtections.RefreshList();
        }
    }
    static deallocateClauseSelect(data) {
        var ProtectionNoAsign = ProtectionControls.listProtectionAll.UifListView('getData');
        var ProtectionAsign = ProtectionControls.listProtectionAssing.UifListView('getData');
        $.each(data, function (index, data) {
            var findClause = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = ProtectionControls.listProtectionAssing.UifListView("findIndex", findClause);
            ProtectionControls.listProtectionAssing.UifListView("deleteItem", index);
        });
        ProtectionNoAsign = ProtectionNoAsign.concat(data);
        ProtectionControls.listProtectionAll.UifListView({ sourceData: ProtectionNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
    }

    static RefreshList() {
        ProtectionAll = ProtectionControls.listProtectionAll.UifListView('getData');
        ProtectionAssing = ProtectionControls.listProtectionAssing.UifListView('getData');
        glbProtectionsAssing = ProtectionAssing;
    }
    
    static loadProtections() {
        ProtectionControls.listProtectionAssing.UifListView({ source: null, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
        ProtectionControls.listProtectionAll.UifListView({ source: null, displayTemplate: ProtectionControls.ProtectionTemplate, selectionType: 'multiple', height: 310 });
        $.each(glbProtectionsAll, function (key, value) {
            ProtectionControls.listProtectionAll.UifListView("addItem", this);
        });
        $.each(glbProtectionsAssing, function (key, value) {
            ProtectionControls.listProtectionAssing.UifListView("addItem", this);
        });
    }
}

