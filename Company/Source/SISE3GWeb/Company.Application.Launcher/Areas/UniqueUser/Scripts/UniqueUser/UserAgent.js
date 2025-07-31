var agentIndex = null;
var agent = {};
$.ajaxSetup({ async: true });
class UserAgent extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputAgent").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#listAgent").UifListView({ displayTemplate: "#agencyTemplate",  delete: true, customAdd: true,  deleteCallback: UserAgent.deleteCallback, height: 300 });
    }
    bindEvents() {
        $('#btnAgent').on('click', this.saveAndLoad);
        $('#btnAgentAccept').on('click', this.addItemAgent);
        $('#btnAgentCancel').on('click', UniqueUser.hidePanelsUser(MenuType.Agent));
        $('#btnNewAgent').on('click', UserAgent.clearPanel);
        $('#btnAgentSave').on('click', this.saveAgents);
        $("#inputAgent").on('buttonClick', this.searchAgent);
        $('#AddAgent').on('click', this.addAgent);
        $('#listAgent').on('rowEdit', this.setAgent);
        // $('#listAgent').on('rowDelete', this.deleteItemAgent);
    }

    saveAndLoad() {
        if ($("#LoginName").val().trim() != "") {
            if (glbUser.UserId == 0) {
                if (UniqueUser.validateForm()) {
                    UserAgent.loadPartialAgent();
                }
            }
            else {
                UserAgent.loadPartialAgent();
            }
        }
        else {
            UniqueUser.validateForm();
        }
    }

    static deleteCallback(deferred, data) {
        deferred.resolve();
    }

    saveAgents() {
        var agents = $("#listAgent").UifListView('getData');        
        glbUser.IndividualsRelation = [];
        $.each(agents, function (index, value) {
            agent = {
                Agency: { Code: value.AgencyCode, Id: value.AgencyId, },
                ChildIndividual: { FullName: value.FullName, IndividualId: value.Code },
                IndividualRelationAppId: value.IndividualRelationAppId,
                RelationTypeId: 1,
                IndividualId: value.Code,
                Id: glbUser.PersonId
            }
            glbUser.ParentIndividualId = glbUser.PersonId;
            glbUser.IndividualsRelation.push(agent);
        });

        UniqueUser.hidePanelsUser(MenuType.Agent);
        UniqueUser.LoadSubTitles();
    }

    static loadDataAgents() {
        var agents = $("#listAgent").UifListView('getData');
        glbUser.IndividualsRelation = [];
        $.each(agents, function (index, value) {
            agent = {
                Agency: { Code: value.AgencyCode, Id: value.AgencyId, },
                ChildIndividual: { FullName: value.FullName, IndividualId: value.Code },
                IndividualRelationAppId: value.IndividualRelationAppId,
                RelationTypeId: 1,
                IndividualId: value.Code,
                Id: glbUser.PersonId
            }
            glbUser.ParentIndividualId = glbUser.PersonId;
            glbUser.IndividualsRelation.push(agent);
        });

        UniqueUser.hidePanelsUser(MenuType.Agent);
        UniqueUser.LoadSubTitles();
    }

    searchAgent() {
        agentSearchType = 2;
        UserAgent.GetAgenciesByAgentIdDescription(0, $("#inputAgent").val().trim());
    }

    static GetAgenciesByAgentIdDescription(number, description) {
        if ((number != 0 || description.length != "")) {
            $.ajax({
                type: "POST",
                url: rootPath + 'UniqueUser/UniqueUser/GetAgenciesByAgentIdDescription',
                data: JSON.stringify({ agentId: number, description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $("#inputAgent").data("Object", data.result[0]);
                        $("#inputAgent").val(data.result[0].FullName);
                    }
                    else if (data.result.length > 1) {
                        modalListType = 1;
                        var dataList = [];

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].Agent.IndividualId,
                                Code: data.result[i].Code,
                                Description: data.result[i].Agent.FullName,
                                IndividualRelationAppId: data.result[i].IndividualRelationAppId
                            });
                        }
                        UniqueUser.ShowDefaultResults(dataList);
                        $('#modalDefaultSearch').UifModal('showLocal', Resources.Language.LabelAgentPrincipal);
                    }
                    else {
                        $("#inputAgent").data("Object", null);
                        $("#inputAgent").val('');
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchAgents, 'autoclose': true })
                    }
                }
                else {
                    $("#inputAgent").data("Object", null);
                    $("#inputAgent").val('');
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchAgents, 'autoclose': true })
            });
        }
    }

    addItemAgent() {
        $("#formAgent").validate();
        if ($("#formAgent").valid() && $("#inputAgent").data("Object") != null) {
            agent =
                {
                    FullName: $("#inputAgent").data("Object").FullName,
                    AgencyCode: $("#inputAgent").data("Object").Code,
                    AgencyId: $("#inputAgent").data("Object").Id,
                    Code: $("#inputAgent").data("Object").Agent.IndividualId,
                    IndividualRelationAppId: null
                }
            if (agentIndex == null) {
                var agents = $("#listAgent").UifListView('getData');
                var addflag = true;
                $.each(agents, function (index, value) {
                    if ((this.Code == agent.Code && this.AgencyCode == agent.AgencyCode)) {
                        addflag = false;
                    }
                });
                if (addflag) {
                    agent.StatusType = 2;
                    $("#listAgent").UifListView("addItem", agent);
                }
            }
            else {
                agent.StatusType = 3;
                $('#listAgent').UifListView('editItem', agentIndex, agent);
            }
            listViewColors("listAgent");
            UserAgent.clearPanel();
        }
    }

    setAgent(event, data, index) {
        agentIndex = index;
        UserAgent.GetAgenciesByAgentIdDescription(data.Code, data.AgencyCode);
    }

    static clearPanel() {
        agentIndex = null;
        $("#inputAgent").data("Object", null);
        $("#inputAgent").val('');
        ClearValidation('#formAgent');
    }

    static loadPartialAgent() {
        UserAgent.clearPanel();
        UniqueUser.showPanelsUser(MenuType.Agent);
        UserAgent.loadAgents();
    }

    static loadAgents() {        
        $("#listAgent").UifListView({ source: null, displayTemplate: "#agencyTemplate",  delete: true, customAdd: true,  deleteCallback: UserAgent.deleteCallback, height: 300 });
        if (glbUser.IndividualsRelation != undefined) {
            $.each(glbUser.IndividualsRelation, function (key, value) {
                agent =
                    {
                        FullName: this.ChildIndividual.FullName,
                        AgencyCode: this.Agency.Code,
                        AgencyId: this.Agency.Id,
                        Code: this.ChildIndividual.IndividualId,
                        IndividualRelationAppId: this.IndividualRelationAppId,
                        StatusType: 1
                    }
                $("#listAgent").UifListView("addItem", agent);
            })
        }
    }
}

