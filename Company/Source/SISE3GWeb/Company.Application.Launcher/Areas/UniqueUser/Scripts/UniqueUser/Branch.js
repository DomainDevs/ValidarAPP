var branchIndex = null;
var branch = {};
var salesPoint = {};
$.ajaxSetup({ async: true });
class Branches {
    static GetBranches() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/UniqueUser/GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}
class Branch extends Uif2.Page {
    getInitialState() {
        $("#listBranch").UifListView({ displayTemplate: "#branchTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Branch.deleteCallback, height: 300 });
        Branches.GetBranches().done(function (data) {
            if (data.success) {
                $('#selectBranch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {
        $('#btnBranchSale').on('click', this.saveAndLoad);
        $('#btnBranchAccept').on('click', this.addItemBranch);
        $('#btnNewBranch').on('click', Branch.clearPanel);
        $('#btnBranchSave').on('click', this.saveBranches);
        $('#listBranch').on('rowEdit', this.setBranch);
        $('#selectBranch').on('itemSelected', Branch.getSalesPoint);
        $('#btnAddPdv').on('click', this.assingAllPdv);
        $('#btnRemovePdv').on('click', this.removeAllPdv);
        $('#btnAssingAll').on('click', this.assingAllBranch);
        $('#btnRemoveAll').on('click', this.removeAllBranch);
    }

    saveAndLoad() {
        if ($("#LoginName").val().trim() != "") {
            if (glbUser.UserId == 0) {
                if (UniqueUser.validateForm()) {
                    Branch.loadPartialBranch();
                }
            }
            else {
                Branch.loadPartialBranch();
            }
        }
        else {
            UniqueUser.validateForm();
        }
    }

    static loadPartialBranch() {
        Branch.clearPanel();
        UniqueUser.showPanelsUser(MenuType.Branch);
        Branch.loadBranches();
    }

    static loadBranches() {
        $("#listBranch").UifListView({ displayTemplate: "#branchTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Branch.deleteCallback, height: 300 });

        if (glbUser.Branch != undefined) {
            $.each(glbUser.Branch, function (key, value) {
                if (this.SalePoints.length == 0) {
                    branch =
                        {
                            Description: this.Description,
                            Id: this.Id,
                            IsDefault: this.IsDefault,
                            SalePoints: [],
                            StatusType: 1
                        }
                    $("#listBranch").UifListView("addItem", branch);
                }
                else {
                    var id = this.Id;
                    var description = this.Description;
                    var defaultBranch = this.IsDefault;
                    $.each(this.SalePoints, function (index, value) {
                        branch =
                            {
                                Description: description,
                                Id: id,
                                SalePoints: { Id: this.Id, Description: this.Description, IsDefault: this.IsDefault },
                                IsDefault: defaultBranch,
                                StatusType: 1
                            }
                        $("#listBranch").UifListView("addItem", branch);
                    })
                }
            })
        }
    }

    static addBranchAllSalesPoint(branchAllSalesPoint) {
        $.each(salesPoint, function (key, salepoint) {
            var itemBranch = {
                Id: branchAllSalesPoint.Id,
                Description: branchAllSalesPoint.Description,
                IsDefault: branchAllSalesPoint.IsDefault,
                SalePoints: {
                    Id: salepoint.Id,
                    Description: salepoint.Description
                }
            }
            Branch.deleteBranchWithSalePointNull(itemBranch);
            Branch.validateItem(itemBranch);
        })

    }
    setBranch(event, data, index) {
        branchIndex = index;
        $("#chkSalesPoint").prop("checked", false);
        $("#chkBranch").prop("checked", false);
        $("#selectBranch").UifSelect("setSelected", data.Id);
        if (data.IsDefault) {
            $("#chkBranch").prop("checked", true);
        }
        var idPoint = 0
        if (data.SalePoints != undefined) {
            idPoint = data.SalePoints.Id
            if (data.SalePoints.IsDefault) {
                $("#chkSalesPoint").prop("checked", true);
            }
        }
        Branch.getSalesPointByBranchId(idPoint);
    }
    static getSalesPointByBranchId(selectId) {
        if ($("#selectBranch").val() != "") {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetSalePointsByBranchId',
                data: JSON.stringify({ branchId: $("#selectBranch").val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    salesPoint = data.result;
                    if (selectId != 0) {
                        $("#selectSalesPoint").UifSelect({ sourceData: data.result, selectedId: selectId });
                    }
                    else {
                        $("#selectSalesPoint").UifSelect({ sourceData: data.result });
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAssingPdv, 'autoclose': true })
            });
        }
    }
    static getSalesPoint() {
        if ($("#selectBranch").val() != "") {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetSalePointsByBranchId',
                data: JSON.stringify({ branchId: $("#selectBranch").val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    salesPoint = data.result;
                    $("#selectSalesPoint").UifSelect({ sourceData: data.result });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAssingPdv, 'autoclose': true })
            });
        }
    }
    static clearPanel() {
        branchIndex = null;
        $("#selectBranch").UifSelect("setSelected", null);
        $("#selectSalesPoint").UifSelect("setSelected", null);
        $("#selectHierarchy").UifSelect("setSelected", null);
        $("#chkSalesPoint").prop("checked", false);
        $("#chkBranch").prop("checked", false);
        ClearValidation('#formBranch');
    }
    static deleteCallback(deferred, data) {
        deferred.resolve();
    }
    saveBranches() {
        if (glbUser.Branch != undefined && glbUser.Branch.length <= 0 && $("#listBranch").UifListView('getData').length <= 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.UserWithOutBranch, 'autoclose': true })
        }
        else {
            var listBranchAndSalesPoint = $("#listBranch").UifListView('getData');
            var hasDefault = false;
            glbUser.Branch = [];
            $.each(listBranchAndSalesPoint, function (index, value) {
                var findBranch = false;
                if (value.IsDefault != undefined && value.IsDefault == true) {
                    hasDefault = true;
                }
                $.each(glbUser.Branch, function (ind, val) {
                    if (val.Id == value.Id) {
                        findBranch = true;
                        if (value.SalePoints != undefined) {
                            glbUser.Branch[ind].SalePoints.push({ Id: value.SalePoints.Id, Description: value.SalePoints.Description, IsDefault: value.SalePoints.IsDefault });
                        }
                    }
                })
                if (!findBranch) {
                    if (value.SalePoints != undefined) {
                        branch = {
                            Id: value.Id, Description: value.Description, IsDefault: value.IsDefault,
                            SalePoints: [{ Id: value.SalePoints.Id, Description: value.SalePoints.Description, IsDefault: value.SalePoints.IsDefault }]
                        }
                    }
                    else {
                        branch = {
                            Id: value.Id, Description: value.Description, IsDefault: value.IsDefault,
                            SalePoints: []
                        }
                    }
                    glbUser.Branch.push(branch);
                }

            });
            if (!hasDefault) {
                glbUser.Branch[0].IsDefault = true;
            }
            UniqueUser.hidePanelsUser(MenuType.Branch);
        }
        UniqueUser.LoadSubTitles();
    }

    addItemBranch() {
        $("#formBranch").validate();
        if ($("#formBranch").valid()) {
            if ($("#selectBranch").UifSelect("getSelected") != "") {
                var formBranch = $("#formBranch").serializeObject();
                var formSalePoints;
                var isDefault = false;

                if (formBranch.BranchId != "") {
                    if ($("#selectSalesPoint").UifSelect("getSelected") != "") {
                        if ($("#chkSalesPoint").is(':checked')) {
                            isDefault = true;
                        }
                        formSalePoints = {
                            Id: formBranch.SalesPointId,
                            IsDefault: isDefault,
                            Description: $("#selectSalesPoint").UifSelect("getSelectedText")
                        };
                    }

                    isDefault = false;
                    if ($("#chkBranch").is(':checked')) {
                        isDefault = true;
                    }
                    var itemBranch = {
                        Id: formBranch.BranchId,
                        Description: $("#selectBranch").UifSelect("getSelectedText"),
                        IsDefault: isDefault,
                        SalePoints: formSalePoints
                    }
                    //Branch.deleteBranchWithSalePointNull(itemBranch);
                    Branch.validateItem(itemBranch);
                }
            }
        }
    }
    static deleteBranchWithSalePointNull(itemBranch) {
        var listBranchAndSalesPoint = $("#listBranch").UifListView("getData");
        $.each(listBranchAndSalesPoint, function (key, value) {
            if ((value.SalePoints == undefined || value.SalePoints.length == 0) && value.Id == itemBranch.Id) {
                $("#listBranch").UifListView("deleteItem", key);
            }
        });
    }
    static validateItem(itemBranch) {
        var bandAddItem = true;
        var listBranchAndSalesPoint = $("#listBranch").UifListView("getData");
        $.each(listBranchAndSalesPoint, function (key, value) {
            if (key != branchIndex) {
                if (itemBranch.IsDefault == true && value.IsDefault == true) {
                    $("#listBranch").UifListView("getData")[key].IsDefault = false;
                }
                if (itemBranch.SalePoints != undefined && value.SalePoints != undefined) {
                    if (itemBranch.SalePoints.IsDefault == true && value.SalePoints.IsDefault == true) {
                        $("#listBranch").UifListView("getData")[key].SalePoints.IsDefault = false;
                    }
                }
                if (itemBranch.SalePoints != null || itemBranch.SalePoints == undefined) {
                    if (value.SalePoints != null) {
                        //if (value.Id == itemBranch.Id && value.SalePoints.Id != undefined && value.SalePoints.Id == itemBranch.SalePoints.Id) {
                        if (value.Id == itemBranch.Id) {
                            if (itemBranch.SalePoints.Id == "") {
                                if (value.SalePoints.Id == undefined || value.SalePoints.Id == "") {
                                    bandAddItem = false;
                                }
                            }
                            else {
                                if (value.SalePoints.Id == itemBranch.SalePoints.Id) {
                                    bandAddItem = false;
                                }
                            }
                        }
                        //}
                        else if (value.Id == itemBranch.Id && value.SalePoints == itemBranch.SalePoints) {
                            bandAddItem = false;
                        }
                    }
                    else if (value.Id == itemBranch.Id) {
                        bandAddItem = false;
                    }
                }
            }
        });
        if (bandAddItem) {
            if (branchIndex == null) {
                itemBranch.StatusTypeService = ParametrizationStatus.Create;
                $("#listBranch").UifListView("addItem", itemBranch);
            }
            else {
                itemBranch.StatusTypeService = ParametrizationStatus.Update;
                $('#listBranch').UifListView('editItem', branchIndex, itemBranch);
            }
            listViewColors("listBranch");
            Branch.clearPanel();
        }
    }

    assingAllPdv() {
        if ($("#selectSalesPoint").UifSelect("getSelectedText") != "") {
            branchIndex = null;
            var isDefault = false;
            if ($("#chkBranch").is(':checked')) {
                isDefault = true;
            }
            var branchAllSalesPoint = { Id: $("#selectBranch").UifSelect("getSelected"), Description: $("#selectBranch").UifSelect("getSelectedText"), DefaultBranch: isDefault, SalePoints: glbUser.Branch };
            Branch.addBranchAllSalesPoint(branchAllSalesPoint);
        }
    }
    assingAllBranch() {
        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetSalesPoint',
            data: JSON.stringify({ userBranches: glbUser.Branch }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                glbUser.Branch = data.result;
                Branch.loadBranches();
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchSuccessfullyModified, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAssingPdv, 'autoclose': true })
        });
    }

    removeAllPdv() {
        if ($("#selectSalesPoint").UifSelect("getSelectedText") != "") {
            glbUser.Branch = [];
            var listBranchAndSalesPoint = $("#listBranch").UifListView("getData");
            $.each(listBranchAndSalesPoint, function (key, value) {
                if (value.Id != $("#selectBranch").UifSelect("getSelected")) {
                    if (value.SalePoints != undefined) {
                        branch = {
                            Id: value.Id, Description: value.Description, DefaultBranch: value.DefaultBranch,
                            SalePoints: [{ Id: value.SalePoints.Id, Description: value.SalePoints.Description, IsDefault: value.SalePoints.IsDefault }]
                        }
                    }
                    else {
                        branch = {
                            Id: value.Id, Description: value.Description, DefaultBranch: value.DefaultBranch,
                            SalePoints: []
                        }
                    }
                    glbUser.Branch.push(branch);
                }
            });
            Branch.loadBranches();
        }
    }
    removeAllBranch() {
        glbUser.Branch = [];
        $("#listBranch").UifListView({ displayTemplate: "#branchTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: Branch.deleteCallback, height: 300 });
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.UserWithOutBranch, 'autoclose': true })
    }
}

