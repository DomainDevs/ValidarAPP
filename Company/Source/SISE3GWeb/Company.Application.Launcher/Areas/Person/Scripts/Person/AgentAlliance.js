var glbAlliancesToDelete = [];
var agentAllianceIndex = null;
var agentAllianceStatus = null;

class AgentAlliance extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        AgentAlliance.GetAlliances().done(function (data) {
            if (data.success) {
                $("#selectAlliance").UifSelect({ sourceData: data.result });
            }
        });
    }

    bindEvents() {
        $('#btnNewAlliance').click(AgentAlliance.Clear);
        $('#btnAddAlliance').click(this.AddAlliance);
        $('#listAlliance').on('rowEdit', AgentAlliance.ShowData);
    }

    AddAlliance() {
        $("#formAlliance").validate();
        if ($("#formAlliance").valid()) {
            var allied = AgentAlliance.GetForm();
            if (allied.IsSpecialImpression) {
                allied.ImpressionDescription = AppResourcesPerson.LabelSpecialImpression; 
            }
            if (agentAllianceIndex == null) {
                var lista = $("#listAlliance").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    var itemDescriptions = item.AgencyDescription + item.AllianceDescription;
                    var alliedDescriptions = allied.AgencyDescription + allied.AllianceDescription;
                    return itemDescriptions.toUpperCase() == alliedDescriptions.toUpperCase();
                });
                if (ifExist.length > 0 && agentAllianceStatus != "create") {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.ErrorExistAlliance, 'autoclose': true });
                }
                else {
                    AgentAlliance.SetStatus(allied, "create");
                    $("#listAlliance").UifListView("addItem", allied);
                }
            }
            else {
                if (agentAllianceIndex != undefined && agentAllianceStatus != undefined) {
                    AgentAlliance.SetStatus(allied, agentAllianceStatus);
                } else {
                    AgentAlliance.SetStatus(allied, "update");
                }
                $('#listAlliance').UifListView('editItem', agentAllianceIndex, allied);
            }
            AgentAlliance.Clear();
        }
    }

    static GetAlliances() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Alliance/GetAllAlliances',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static LoadAgencies() {
        var individualId = $("#lblPersonCode").val();
        AgentRequest.GetAgenciesByAgentId(individualId).done(function (data) {
            if (data.success) {
                $("#selectAcency").UifSelect({ sourceData: data.result });
            }
        });
        AgentAlliance.LoadListViewAlliance(individualId).done(function (data) {
            $("#listAlliance").UifListView({ displayTemplate: "#AllianceTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: AgentAlliance.DeleteAlliance, height: 300 });
            $.each(data.result, function (key, value) {
                var agency =
                    {
                        IndividualId: this.IndividualId,
                        AgencyAgencyId: this.AgencyAgencyId,
                        AgencyDescription: $("#selectAcency option[value=" + this.AgencyAgencyId + "]").text(),
                        AllianceId: this.AllianceId,
                        AllianceDescription: $("#selectAlliance option[value=" + this.AllianceId + "]").text(),
                        IsSpecialImpression: this.IsSpecialImpression
                    };
                if (agency.IsSpecialImpression) {
                    agency.ImpressionDescription = AppResourcesPerson.LabelSpecialImpression;
                }
                $("#listAlliance").UifListView("addItem", agency);
            })
        });
    }

    static LoadListViewAlliance(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgenciesAgentByAgentId",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static DeleteAlliance(event, data, index) {
        if (data.Status != "create") {
            data.Status = "delete";
            glbAlliancesToDelete.push(data);
        }
        event.resolve();
        AgentAlliance.Clear();
    }

    static Clear() {
        $('#selectAcency').UifSelect("setSelected", null);
        $('#selectAlliance').UifSelect("setSelected", null);
        $("#chkIsSpecialImpression").prop("checked", "");
        agentAllianceIndex = null;
        agentAllianceStatus = null;
    }

    static GetForm() {
        var data = {
        };
        $("#formAlliance").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.IndividualId = $("#lblPersonCode").val();
        data.AgencyDescription = $('#selectAcency option:selected').text();
        data.AllianceDescription = $('#selectAlliance option:selected').text();
        data.IsSpecialImpression = $("#chkIsSpecialImpression").is(":checked");
        return data;
    }

    static ShowData(event, result, index) {
        AgentAlliance.Clear();
        agentAllianceIndex = index;
        agentAllianceStatus = result.Status;
        $('#selectAcency').UifSelect("setSelected", result.AgencyAgencyId);
        $('#selectAlliance').UifSelect("setSelected", result.AllianceId);
        if (result.IsSpecialImpression) {
            $('#chkIsSpecialImpression').prop("checked", true);
        }
    }

    static SetStatus(object, status) {
        object.Status = status;
    }
}