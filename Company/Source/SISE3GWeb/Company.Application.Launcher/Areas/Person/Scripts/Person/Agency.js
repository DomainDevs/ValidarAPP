var Agencies = [];
var rowAgencyId = -1;
var AgencyId = 0;
var heightListViewAgency = 256;
class Agency extends Uif2.Page {
    getInitialState() {
        BranchRequest.GetBranchs().done(function (data) {
            if (data.success) {
                $("#selectBranch").UifSelect({ sourceData: data.result });
            }
        });
        AgentRequest.GetAgentTypes().done(function (data) {
            if (data.success) {
                $("#selectTypeIntermediary").UifSelect({ sourceData: data.result });
                $("#selectTypeIntermediary").prop('disabled', false);
            }
        });
    }

    //Seccion Eventos
    bindEvents() {
        $("#btnAgency").click(this.BtnAgency);
        $("#btnNewAgency").click(Agency.ClearControlAgency);
        $("#btnacceptAgency").click(this.AcceptAgency);
        $("#btnrecordAgency").click(Agency.SaveAgencies);
        $("#btnCloseAgency").click(this.CloseAgency);
        $("#selectReasonLow").on('itemSelected', this.SelectedReasonLow);
        $('#listAgency').on('rowEdit', this.AgencyEdit);
    }

    BtnAgency() {
        if (Agency.ValidateAgentControl()) {
            Agency.ClearControlAgency();
            Agency.LoadAgency();
            $("#btnBottomAgency").show();
        }
    }

    static ClearControlAgency() {
        rowAgencyId = -1;
        AgencyId = 0;
        $("#inputkey").val("");
        $("#inputAgencyName").val($('#inputCompanyTradeName').val());
        $("#InputDateDeclined").val("");
        if (glbPolicy == null) {
            $("#selectReasonLow").UifSelect('setSelected', null);
            $("#selectBranch").UifSelect('setSelected', null);
        }
        $("#notesAgency").val("");
        $("#inputkey").val("");
        Agency.DisabledControlsAgency(false);
        $("#InputDateDeclined").prop("disabled", true);
        $("#selectTypeIntermediary").UifSelect('setSelected', null);
        $("#selectTypeIntermediary").prop('disabled', false);
    }

    AcceptAgency() {
        if (Agency.ValidateAgency()) {
            Agency.CreateAgency();
            Agency.ClearControlAgency();
        }
    }

    //seccion grabado
    static SaveAgencies() {
        var agencyAgent = $("#listAgency").UifListView('getData');
        var individualId = $('#CodPersonAgent').val();

        var agencyAgentFilter = agencyAgent.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        var agencyObject = { Agencies: agencyAgentFilter };

        if (agencyAgentFilter.length > 0) {
            AgentRequest.CreateAgent(agencyObject, individualId)
        }
        Agency.GetLoadAgenciesAgent(individualId);
    }

    static GetLoadAgenciesAgent(IndividualId) {
        Agency.LoadAgencyRequest(IndividualId);
    }

    CloseAgency() {
        Agency.ClearControlAgency();
        $("#btnBottomAgency").hide();
    }

    SelectedReasonLow(event, selectedItem) {
        if (selectedItem.Id > 0) {
            $("#InputDateDeclined").val(DateNowPerson);
        }
        else {
            $("#InputDateDeclined").val("");
        }
    }

    AgencyEdit(event, data, index) {
        rowAgencyId = index;
        Agency.EditAgency(data, index);
    }

    //Seccion Funciones
    static CleanObjectAgentAgency() {
        Agencies = [];
        $("#listAgency").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#AgencyTemplate", height: heightListViewAgency });
    }

    static DisabledControlsAgency(disabled) {
        $("#inputkey").prop("disabled", disabled);
        $("#selectReasonLow").prop("disabled", disabled);
        $("#InputDateDeclined").prop("disabled", disabled);
    }

    //Seccion Load

    static LoadAgencyRequest(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgenAgencytByIndividualId",
            data: JSON.stringify({ invidualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) { Agency.LoadAgency(data.result) }
        });
    }

    static LoadAgency(data) {

        $("#listAgency").UifListView({ sourceData: data, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#AgencyTemplate", height: heightListViewAgency });
    }

    //Seccion Creacion
    static CreateAgency() {

        var agencyTmp = Agency.CreateAgencyModel();
        if (rowAgencyId == -1) {
            $("#listAgency").UifListView("addItem", agencyTmp);
        }
        else {
            $("#listAgency").UifListView("editItem", rowAgencyId, agencyTmp);
        }
    }

    static CreateAgencyModel() {
        var Agency = {};

        Agency.Code = $("#inputkey").val();
        Agency.FullName = $("#inputAgencyName").val();
        Agency.BranchId = $("#selectBranch").UifSelect("getSelected");
        Agency.DescriptionBranch = $("#selectBranch").UifSelect("getSelectedText");
        if ($("#selectReasonLow").UifSelect("getSelected") != null && $("#selectReasonLow").UifSelect("getSelected") != "") {
            Agency.AgentDeclinedTypeId = $("#selectReasonLow").UifSelect("getSelected");
            Agency.Description = $("#selectReasonLow").UifSelect("getSelectedText");
            Agency.DateDeclined = $("#InputDateDeclined").val();
        }
        else {
            Agency.AgentDeclinedTypeId = null;
        }
        Agency.AgentId = $("#selectTypeIntermediary").UifSelect("getSelected");
        Agency.Annotations = $("#notesAgency").val();
        Agency.Id = AgencyId;
        Agency.StatusTypeService = $('#StatusTypeService').val();
        if (Agency.Code <= 0) {
            Agency.StatusTypeService = ParametrizationStatus.Create;
        }
        else {
            Agency.StatusTypeService = ParametrizationStatus.Update;
        }


        return Agency;
    }

    //seccion edit
    static EditAgency(data, index) {
        AgencyId = data.Id;
        $("#inputkey").val(data.Code);
        $("#inputAgencyName").val(data.FullName);
        $("#selectBranch").UifSelect("setSelected", data.BranchId);
        if (data.AgentDeclinedTypeId != null) {
            $("#selectReasonLow").UifSelect("setSelected", data.AgentDeclinedTypeId);
        }
        else {
            $("#selectReasonLow").UifSelect("setSelected", "");
        }
        if (data.DateDeclined != null) {
            $("#InputDateDeclined").UifDatepicker('setValue', data.DateDeclined);
        }
        else {
            $("#InputDateDeclined").UifDatepicker('clear');
        }
        $("#notesAgency").val(data.Annotations);
        $('#selectTypeIntermediary').UifSelect("setSelected", data.AgentId);
    }

    static FormatAgency() {
        if (Agencies != null) {
            $.each(Agencies, function (key, item) {
                item.DateDeclined = FormatDate(item.DateDeclined);
                var selector = "#selectBranch [value=" + item.Branch.Id + "]"
                item.Branch.Description = $(selector).text();
            });
        }
    }

    //seccion Validaciones
    static DuplicateAgency() {
        var duplicate = false;
        $.each($("#listAgency").UifListView('getData'), function (i, item) {
            if ($.trim(item.NameAgency) == $.trim($("#inputAgencyName").val())) {
                duplicate = true;
                return false;
            }
        });

        return duplicate;
    }

    static DuplicateKeyAgency(agencyKey) {
        var duplicate = false;
        $.each($("#listAgency").UifListView('getData'), function (key, item) {
            if (item.Code == $("#inputkey").val() && rowAgencyId != key) {
                duplicate = true;
                return false;
            }
        });
        return duplicate;
    }

    static DuplicateAgentAgency(agencyAgentId) {
        var duplicate = false;
        $.each($("#listAgency").UifListView('getData'), function (key, item) {
            if (item.AgentId == agencyAgentId && rowAgencyId != key) {
                duplicate = true;
                return false;
            }
        });
        return duplicate;
    }

    static ValidateAgency() {
        var msj = "";
        if ($("#inputkey").val() == "") {
            msj = msj + AppResourcesPerson.LabelKey + "<br>"
        }
        if ($("#inputAgencyName").val() == "") {
            msj = msj + AppResourcesPerson.LabelNameAgency + "<br>"
        }
        if ($("#selectBranch").UifSelect("getSelected") == null || $("#selectBranch").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelBranch + "<br>"
        }
        if ($("#selectReasonLow").UifSelect("getSelected") != null && $("#selectReasonLow").UifSelect("getSelected") != "") {
            if ($("#InputDateDeclined").val() == "") {
                msj = msj + AppResourcesPerson.LabelDeclinedDate + "<br>"
            }
            if ($("#selectReasonLow").UifSelect("getSelected") == "") {
                msj = msj + AppResourcesPerson.LabelReasonLow + " <br>"
            }
        }
        if ($("#selectTypeIntermediary").UifSelect("getSelected") == null || $("#selectTypeIntermediary").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.TypeIntermediary + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msj, 'autoclose': true })
            return false;
        } else if (Agency.DuplicateAgency()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageAgencyExist, 'autoclose': true });
            return false;
        }
        else if (Agency.DuplicateKeyAgency($("#inputkey").val())) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageKeyExist, 'autoclose': true });
            return false;
        }
        //else if (Agency.DuplicateAgentAgency($("#selectTypeIntermediary").val())) {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageKeyTypeExist + " " + $("#selectTypeIntermediary option:selected").text(), 'autoclose': true });
        //    return false;
        //}
        return true;

    }

    static ValidateAgentControl() {
        var msj = "";
        if ($("#selectTypeIntermediary").UifSelect("getSelected") == null || $("#selectTypeIntermediary").UifSelect("getSelected") == "") {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true })
            return false;
        }
        return true;
    }

    static GetAgentAgencyExist() {
        var result = false;
        AgentRequest.GetAgenAgencytByAgentCodeAndType($("#inputkey").val(), $("#selectTypeIntermediary").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                result = true;
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                result = false;
            }
        });
        return result;
    }
    //GetAgenAgencytByAgentCodeAndType
}