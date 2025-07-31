var glbTempAllySalePoints = [];
var glbAllieds = [];
var allianceName = false;
var listAllieds = [];

class AlliedSalePoints extends Uif2.Page {
    getInitialState() {
        $('#inputIntermediary').val(null);
        $("#inputIntermediary").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    }
    bindEvents() {
        $('#btnAlliedSalePoints').on('click', this.saveAndLoad);
        $('#inputIntermediary').on('buttonClick', AlliedSalePoints.GetAgentAgencyByIdDescription);
        $("#selectAllieds").on('itemSelected', AlliedSalePoints.ChangeAllieds);
        $('#tblResultListAlliedSalePoints tbody').on('click', 'tr', AlliedSalePoints.AssignItem);
        $('#btnAlliedSalePointsSave').on('click', AlliedSalePoints.SaveAlliedSalePoints);
        $('#btnAlliedSalePointsClose').on('click', AlliedSalePoints.CleanFormAlliedSalePoint);
        $('#btnAlliedSalePointsAssignAll').on('click', AlliedSalePoints.AssignAll);
        $('#btnAlliedSalePointsUnAssignAll').on('click', AlliedSalePoints.UnassignAll);
    }

    saveAndLoad() {
        modalSave = true;
        AlliedSalePoints.loadPartialAlliedSalePoints();
        AlliedSalePoints.GetAlliedsList();
    }

    static loadPartialAlliedSalePoints() {
        if ($("#LoginName").val().trim().length > 0) {
            UniqueUser.showPanelsUser(MenuType.AlliedSalePoints);
            if ($('#colnamemodalAlliedSalePoints').length) {
            }
            else {
                $('#modalAlliedSalePoints div.modal-header').first().css({ 'background': '#365380', 'color': 'white' });
                $('#modalAlliedSalePoints div.modal-header').first().find("button").wrap("<div class='column-25 even pull-right'></div>");
                $('#modalAlliedSalePoints div.modal-header').first().find("h4").wrap("<div class='column-75 pull-left' id='colnamemodalAlliedSalePoints'></div>");
                $('#modalAlliedSalePoints div.modal-header').first().find("button").wrap("<div class='input-group uif-inputsearch-group'></div>");
                $('<input class="uif-input-search uif-inputsearch-pc form-control" style="background:#365380; border:none; color:white" readonly id="usernamemodal">').insertBefore($('#modalAlliedSalePoints div.modal-header').first().find("button"));
            }
            $("#usernamemodal").val($("#LoginName").val());
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchAllyIntermediaryWith, 'autoclose': true });
        }
    }

    static GetAllieds(individualId, agentAgencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "UniqueUser/UniqueUser/GetAllyByIntermediary",
            data: JSON.stringify({ individualId: individualId, agentAgencyId: agentAgencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (allianceName) {
                    if ($('#selectedAlliedSalePoints').text() == data.result.filter(x => x.AllianceId == glbAllySalePoints[0].AllianceId).shift().Description) {
                        $('#selectedAlliedSalePoints').text(data.result.filter(x => x.AllianceId == glbAllySalePoints[0].AllianceId).shift().Description);
                    }
                    else if (glbAllySalePoints.length > 0) {
                        $('#selectedAlliedSalePoints').text(AppResources.LabelVarious);
                    }
                    
                    allianceName = false;
                }
                else {
                    $("#selectAllieds").UifSelect({ sourceData: data.result });
                }
            }
        });
    }
    static ChangeAllieds() {
        AlliedSalePoints.GetAlliedSalePoints();

    }

    static ShowAlliedSalePoints(dataTable) {
        var indexItem = 0;

        $.each(dataTable, function (index, value) {
            indexItem = glbTempAllySalePoints.findIndex(x => x.AllianceId == value.AllianceId && x.BranchAllianceId == value.BranchAllianceId && x.SalePointAllianceId == value.SalePointAllianceId && x.IndividualId == value.IndividualId && x.AgentAgencyId == value.AgentAgencyId);
            if (indexItem >= 0) {
                value.IsAssign = glbTempAllySalePoints[indexItem].IsAssign;
            }
            else {
                indexItem = glbAllySalePoints.findIndex(x => x.AllianceId == value.AllianceId && x.BranchAllianceId == value.BranchAllianceId && x.SalePointAllianceId == value.SalePointAllianceId && x.IndividualId == value.IndividualId && x.AgentAgencyId == value.AgentAgencyId);
                if (indexItem >= 0) {
                    value.IsAssign = glbAllySalePoints[indexItem].IsAssign;
                }
            }
        });

        $('#tblResultListAlliedSalePoints').UifDataTable('clear');
        $('#tblResultListAlliedSalePoints').UifDataTable('addRow', dataTable);

        var table = dataTable;
        $.each(table, function (key, value) {
            if (this.IsAssign == true) {
                if (glbAllySalePoints.filter(x => x.SalePointAllianceId == this.SalePointAllianceId).length == 0) {
                    glbAllySalePoints.push(this);
                }                
            }
        })
    }
    static GetAlliedSalePoints() {
        var individualIdIntermediary = $("#inputIntermediary").data("Object").IndividualId;
        var agencyAgencyIdIntermediary = $("#inputIntermediary").data("Object").AgencyAgencyId;
        var allianceIdIntermediary = $("#selectAllieds").UifSelect("getSelected");
        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetAlliedSalePoints',
            data: JSON.stringify({ userId: glbUser.UserId, allianceId: allianceIdIntermediary, individualId: individualIdIntermediary, agentAgencyId: agencyAgencyIdIntermediary }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                var dataList = [];
                for (var i = 0; i < data.result.length; i++) {
                    dataList.push({
                        AllianceId: data.result[i].AllianceId,
                        BranchAllianceId: data.result[i].BranchAllianceId,
                        SalePointAllianceId: data.result[i].SalePointAllianceId,
                        IndividualId: data.result[i].IndividualId,
                        AgentAgencyId: data.result[i].AgentAgencyId,
                        IsAssign: data.result[i].IsAssign,
                        SalePointDescription: data.result[i].SalePointDescription,
                        BranchDescription: data.result[i].BranchDescription
                    });
                }
                AlliedSalePoints.ShowAlliedSalePoints(dataList);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchAllyIntermediary, 'autoclose': true });
         });

        
    }

    static GetAgentAgencyByIdDescription() {
        var description = $('#inputIntermediary').val();
        $("#selectAllieds").UifSelect("setSelected", null);
        if (description != "") {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetAgentAgencyByIdDescription',
                data: JSON.stringify({ description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $("#inputIntermediary").data("Object", data.result[0]);
                        $("#inputIntermediary").val(data.result[0].AgentCode + " - " + data.result[0].Description + " (" + data.result[0].IndividualId + ")");
                        AlliedSalePoints.GetAllieds(data.result[0].IndividualId, data.result[0].AgencyAgencyId);
                    }
                    else if (data.result.length > 1) {
                        modalListType = 4;
                        var dataList = [];
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                Clave: data.result[i].AgentCode,
                                Descripcion: data.result[i].Description,
                                CodigoAgencia: data.result[i].AgencyAgencyId
                            });
                        }
                        UniqueUser.ShowDefaultAllyIntermediary(dataList);
                        $('#modalDefaultSearchAllyIntermediary').UifModal('showLocal', AppResources.AllyIntermediary);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchAllyIntermediary, 'autoclose': true })
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchAllyIntermediary, 'autoclose': true })
            });
        }
    }

    static GetAgentAgencyByPrimaryKey(individualId, agentAgencyId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetAgentAgencyByPrimaryKey',
            data: JSON.stringify({ individualId: individualId, agentAgencyId: agentAgencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $("#inputIntermediary").data("Object", data.result);
                $("#inputIntermediary").val(data.result.AgentCode + " - " + data.result.Description + " (" + data.result.IndividualId + ")");
                AlliedSalePoints.GetAllieds(data.result.IndividualId, data.result.AgencyAgencyId);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageSearchAllyIntermediary, 'autoclose': true })
        });

    }

    static AssignItem() {
        var indexglbAllySalePoints;
        var rowEdit = $('#tblResultListAlliedSalePoints').DataTable().row(this).data();
        for (var i = 0; i < glbAllySalePoints.length; i++) {
            if (glbAllySalePoints[i].AllianceId == rowEdit.AllianceId && glbAllySalePoints[i].BranchAllianceId == rowEdit.BranchAllianceId && glbAllySalePoints[i].SalePointAllianceId == rowEdit.SalePointAllianceId && glbAllySalePoints[i].IndividualId == rowEdit.IndividualId && glbAllySalePoints[i].AgentAgencyId == rowEdit.AgentAgencyId) {
                indexglbAllySalePoints = i; 
                break;
            }
        }
        glbTempAllySalePoints = glbAllySalePoints;
        $('#tblResultListAlliedSalePoints').DataTable().row(this).data({
            AllianceId: rowEdit.AllianceId,
            BranchAllianceId: rowEdit.BranchAllianceId,
            SalePointAllianceId: rowEdit.SalePointAllianceId,
            IndividualId: rowEdit.IndividualId,
            AgentAgencyId: rowEdit.AgentAgencyId,
            IsAssign: rowEdit.IsAssign == true ? false : true,
            SalePointDescription: rowEdit.SalePointDescription,
            BranchDescription: rowEdit.BranchDescription
        });

        rowEdit = $('#tblResultListAlliedSalePoints').DataTable().row(this).data();
        var found = false;
        for (var i = 0; i < glbTempAllySalePoints.length; i++) {
            if (glbTempAllySalePoints[i].AllianceId == rowEdit.AllianceId && glbTempAllySalePoints[i].BranchAllianceId == rowEdit.BranchAllianceId && glbTempAllySalePoints[i].SalePointAllianceId == rowEdit.SalePointAllianceId && glbTempAllySalePoints[i].IndividualId == rowEdit.IndividualId && glbTempAllySalePoints[i].AgentAgencyId == rowEdit.AgentAgencyId) {             
                glbAllySalePoints[indexglbAllySalePoints] = rowEdit;
                found = true;
                break;
            }
        }
        if (!found) {
            glbTempAllySalePoints.push(rowEdit);
        }
        

    }

    static SaveAlliedSalePoints() {
        var indexItem = 0;
        var itemsSelected = [];
        itemsSelected = glbTempAllySalePoints;
        UniqueUser.hidePanelsUser(MenuType.AlliedSalePoints);
        AlliedSalePoints.CleanFormAlliedSalePoint();
        if (glbAllySalePoints.length == 0) {
            glbAllySalePoints = glbTempAllySalePoints;
        }
        else {

            $.each(glbTempAllySalePoints, function (index, value) {
                indexItem = glbAllySalePoints.findIndex(x => x.AllianceId == value.AllianceId && x.BranchAllianceId == value.BranchAllianceId && x.SalePointAllianceId == value.SalePointAllianceId && x.IndividualId == value.IndividualId && x.AgentAgencyId == value.AgentAgencyId);
                if (indexItem >= 0) {
                    glbAllySalePoints[indexItem] = value;
                }
                else {
                    glbAllySalePoints.push(value);
                }
            });
        }

        $.each(listAllieds, function (key, value) {
            if (glbAllySalePoints.filter(x => x.SalePointAllianceId == this.SalePointAllianceId).length == 0) {
                glbAllySalePoints.push(this);
            }
        })

        AlliedSalePoints.GetAlliedsText();

    }

    static AssignAll() {
        var table = $("#tblResultListAlliedSalePoints").UifDataTable('getData');
        $.each(table, function (key, value) {
            this.IsAssign = true;
            var found = false;
            for (var i = 0; i < glbTempAllySalePoints.length; i++) {
                if (glbTempAllySalePoints[i].AllianceId == value.AllianceId && glbTempAllySalePoints[i].BranchAllianceId == value.BranchAllianceId && glbTempAllySalePoints[i].SalePointAllianceId == value.SalePointAllianceId && glbTempAllySalePoints[i].IndividualId == value.IndividualId && glbTempAllySalePoints[i].AgentAgencyId == value.AgentAgencyId) {
                    glbTempAllySalePoints[i] = value;
                    found = true;
                    break;
                }
            }
            if (!found) {
                glbTempAllySalePoints.push(value);
            }
        });

        $.each(glbTempAllySalePoints, function (key, value) {
            if (glbAllySalePoints.filter(x => x.SalePointAllianceId == this.SalePointAllianceId).length == 0) {
                glbAllySalePoints.push(this);
            }
            else if (glbAllySalePoints.filter(x => x.SalePointAllianceId == this.SalePointAllianceId).length == 1) {
                glbAllySalePoints[glbAllySalePoints.indexOf(glbAllySalePoints.filter(x => x.SalePointAllianceId == this.SalePointAllianceId).shift())]= this;
            }
        })

        $("#tblResultListAlliedSalePoints").UifDataTable({ sourceData: table })
    }

    static UnassignAll() {
        var table = $("#tblResultListAlliedSalePoints").UifDataTable('getData');
        $.each(table, function (key, value) {
            this.IsAssign = false;
            var found = false;
            for (var i = 0; i < glbTempAllySalePoints.length; i++) {
                if (glbTempAllySalePoints[i].AllianceId == value.AllianceId && glbTempAllySalePoints[i].BranchAllianceId == value.BranchAllianceId && glbTempAllySalePoints[i].SalePointAllianceId == value.SalePointAllianceId && glbTempAllySalePoints[i].IndividualId == value.IndividualId && glbTempAllySalePoints[i].AgentAgencyId == value.AgentAgencyId) {
                    glbTempAllySalePoints[i] = value;
                    found = true;
                    break;
                }
            }
            if (!found) {
                glbTempAllySalePoints.push(value);
            }
        });

        $.each(glbAllySalePoints, function (key, value) {
            if (this.AllianceId == $("#selectAllieds").val()) {
                this.IsAssign = false;
            }
        })

        $("#tblResultListAlliedSalePoints").UifDataTable({ sourceData: table })
    }

    static CleanFormAlliedSalePoint() {
        glbTempAllySalePoints = [];
        $('#tblResultListAlliedSalePoints').UifDataTable('clear');
        $("#selectAllieds").UifSelect({ sourceData: null });
        $("#inputIntermediary").data("Object", null);
        $("#inputIntermediary").val('');
    }

    static GetUniqueUserSalePointText(individualId, userId) {
        if (userId == 0) {
            $('#selectedAlliedSalePoints').text('');
        }
        else {
            //return $.ajax({
            //    type: "POST",
            //    url: rootPath + "UniqueUser/UniqueUser/GetTextAllieds",
            //    data: JSON.stringify({ userId: userId }),
            //    dataType: "json",
            //    contentType: "application/json; charset=utf-8"
            //}).done(function (data) {
            //    if (data.success) {
            //        $('#selectedAlliedSalePoints').text('');
            //        $('#selectedAlliedSalePoints').text(data.result);
            //    }
            //});
        }
    }

    static GetAlliedsList() {
        if (glbUser.UserId == 0) {
            $('#selectedAlliedSalePoints').text('');
        }
        else {
            //return $.ajax({
            //    type: "POST",
            //    url: rootPath + "UniqueUser/UniqueUser/GetAlliedsByUserId",
            //    data: JSON.stringify({ userId: glbUser.UserId }),
            //    dataType: "json",
            //    contentType: "application/json; charset=utf-8"
            //}).done(function (data) {
            //    if (data.success) {
            //        listAllieds = data.result;
            //    }
            //});
        }
    }

    static GetAlliedsText() {
        if (glbUser.UserId == 0) {
            $('#selectedAlliedSalePoints').text('');
        }
        else {
            var IdAlliance = [];
            for (var i = 0; i < glbAllySalePoints.length; i++) {
                if (i > 0) {
                    if (glbAllySalePoints[i].AllianceId != glbAllySalePoints[i - 1].AllianceId && glbAllySalePoints[i].IsAssign)
                    {
                        IdAlliance.push({ AllianceId: glbAllySalePoints[i].AllianceId, IndividualId: glbAllySalePoints[i].IndividualId, AgentAgencyId: glbAllySalePoints[i].AgentAgencyId});
                    }
                }
                if (i == 0 && glbAllySalePoints[i].IsAssign) {
                    IdAlliance.push({ AllianceId: glbAllySalePoints[i].AllianceId, IndividualId: glbAllySalePoints[i].IndividualId, AgentAgencyId: glbAllySalePoints[i].AgentAgencyId });
                }
            }
            return $.ajax({
                type: "POST",
                url: rootPath + "UniqueUser/UniqueUser/GetAlliedsText",
                data: JSON.stringify({ listCptUniqueUserSalePointAlliance: IdAlliance }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $('#selectedAlliedSalePoints').text(data.result);
                }
            });
        }
    }
}

