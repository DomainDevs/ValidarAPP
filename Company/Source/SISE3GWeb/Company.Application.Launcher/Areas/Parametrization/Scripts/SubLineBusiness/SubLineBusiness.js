
var glbTechnicalBranchEdit = null;
var dropDownSearchSubBranch;
class SubLineBusinessParametrization extends Uif2.Page {

    getInitialState() {
       $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        SubLineBusinessParametrization.GetLineBusiness();
        $("#listViewSearchAdvanced").UifListView({
            displayTemplate: "#SubBranchTemplate",
            source: null,
            selectionType: 'single',
            height: 350
        });
        request('Parametrization/SubLineBusiness/GetSubLinesBusinessById', null, 'GET', "Error busqueda sub ramo tecnico", SubLineBusinessParametrization.getTechnicalBranch)

    }
    bindEvents() {
        $('#listViewSearchAdvanced').on('rowEdit', SubLineBusinessParametrization.editTechnicalBranch);
        $("#btnNewSubBranch").click(SubLineBusinessParametrization.clear);
        $("#btnSubBranchAccept").click(this.addTechnicalBranch);
        $('#inputLoadId').on("search", this.SearchSubLineBusiness);
        $('#tblResultListSubLineBusinessTotal').on('rowSelected', this.SelectSearchSubLienBusinessByName);
        $("#btnExit").click(this.redirectIndex);
        $("#btnExport").click(this.exportExcel);
        $("#btnSaveSubBranch").click(SubLineBusinessParametrization.Save);

        
    }

    static getTechnicalBranch(data) {
        $("#listViewSearchAdvanced").UifListView({
            displayTemplate: "#SubBranchTemplate",
            sourceData: data,
            selectionType: 'single',
            height: 350,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: SubLineBusinessParametrization.deleteCallbackListSubLineBusiness,
        });
    }

    static editTechnicalBranch(event, result, index) {        
        SubLineBusinessParametrization.clear();
        glbTechnicalBranchEdit = result;   
        glbTechnicalBranchEdit.Index = index;
        $("#Id").val(result.Id);
        $("#inputDescriptionShort").val(result.SmallDescription);
        $("#inputDescription").val(result.Description);
        $("#selectLineBussines").UifSelect("setSelected", result.LineBusinessId);
        $("#selectLineBussines").UifSelect("disabled", true);
        $("#inputLoadId").val(result.Description);
        $("#LineBusinessDescription").val(result.LineBusinessDescription)
        
    }
    static clear() {
        glbTechnicalBranchEdit = null;
        $("#Id").val('');
        $("#inputDescription").val('');
        $("#inputDescriptionShort").val('');
        $("#selectLineBussines").UifSelect("setSelected", null);
        $("#selectLineBussines").UifSelect("disabled", false);
        $("#inputLoadId").val('');
        ClearValidation("#formSubBranch");
    }

    addTechnicalBranch() {
        if ($("#formSubBranch").valid()) {
            var formTechnicalBranch = $("#formSubBranch").serializeObject();
            formTechnicalBranch.LineBusinessDescription = $("#selectLineBussines").UifSelect("getSelectedText")
			if (parseInt(formTechnicalBranch.Id) > 0) {
                formTechnicalBranch.SStatusTypeService = ParametrizationStatus.Update;
                SubLineBusinessParametrization.UpdateTechnicalBranchParametrization(formTechnicalBranch);
            }
			else {
                formTechnicalBranch.StatusTypeService = ParametrizationStatus.Create;
                SubLineBusinessParametrization.InsertTechnicalBranchParametrization(formTechnicalBranch);
            }
            SubLineBusinessParametrization.clear();

		}
           
     }
    static UpdateTechnicalBranchParametrization(formTechnicalBranch) {
        formTechnicalBranch.StatusTypeService = ParametrizationStatus.Update;
        formTechnicalBranch.bandUpdate = true;

        const findDescription = function (element, index, array) {
            return element.Description === formTechnicalBranch.Description && index != glbTechnicalBranchEdit.Index;
        }

        var indexList = $("#listViewSearchAdvanced").UifListView("findIndex", findDescription);
        if (indexList < 0)
        {
            $("#listViewSearchAdvanced").UifListView("editItem", glbTechnicalBranchEdit.Index, formTechnicalBranch);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': "Ya existe un SubRamo Técnico con esa descripción", 'autoclose': true });
        }

        
    }
    static InsertTechnicalBranchParametrization(formTechnicalBranch) {
        formTechnicalBranch.StatusTypeService = ParametrizationStatus.Create;
        
        var itemModified = [];
        formTechnicalBranch.bandAdd = true; 
        const findDescription = function (element, index, array) {
            return element.Description === formTechnicalBranch.Description;
        }
        
        var index = $("#listViewSearchAdvanced").UifListView("findIndex", findDescription);

        if (glbTechnicalBranchEdit !== null) {
            $("#listViewSearchAdvanced").UifListView("editItem", glbTechnicalBranchEdit.Index, formTechnicalBranch);
        }
        else {
            if (index < 0)
            {
                $("#listViewSearchAdvanced").UifListView("addItem", formTechnicalBranch);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': "Ya existe un SubRamo Técnico con esa descripción", 'autoclose': true });
            }
        }
            
    }
    static GetLineBusiness() {
        SubLineBusinessRequest.GetLineBusiness().done(response => {
            let result = response.result;
            if (response.success) {
                $("#selectLineBussines").UifSelect({ sourceData: result });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    SearchSubLineBusiness(event, value) {

        if ($.trim($("#inputLoadId").val()) != "") {
            SubLineBusinessParametrization.GetSubLineBusiness($("#inputLoadId").val());
        }
    }

    static GetSubLineBusiness(description) {

        SubLineBusinessRequest.GetSubLineBusinessByNameAnTitle(description).done(function (data) {
            if (description.length >= 3) {
                if (data.success) {


                    if (data.result.SubLineBranchService.length > 0) {
                        if (data.result.SubLineBranchService.length === 1) {
                            $("#inputLoadId").data("Object", data.result.SubLineBranchService);
                            $("#inputLoadId").val(data.result.SubLineBranchService[0].Description);
                            $("#inputDescriptionShort").val(data.result.SubLineBranchService[0].SmallDescription);
                            $("#inputDescription").val(data.result.SubLineBranchService[0].Description);
                            $("#selectLineBussines").UifSelect("setSelected", data.result.SubLineBranchService[0].LineBusinessQuery.Id);
                        }
                        else {
                            SubLineBusinessParametrization.ShowListSubLineBusiness(data.result.SubLineBranchService);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchTexts, 'autoclose': true });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true });
            }
        });


    }

    static ShowListSubLineBusiness(dataTable) {
        $('#tblResultListSubLineBusinessTotal').UifDataTable('clear');
        $('#tblResultListSubLineBusinessTotal').UifDataTable('addRow', dataTable);
        $('#modalListSearchSubLineBusiness').UifModal('showLocal', AppResources.LabelSelectText);
    }

    SelectSearchSubLienBusinessByName(event, dataListSubLineBusiness, position) {

        $("#inputLoadId").data("Object", dataListSubLineBusiness);
        $("#inputLoadId").val(dataListSubLineBusiness.Description);
        $("#inputDescriptionShort").val(dataListSubLineBusiness.SmallDescription);
        $("#inputDescription").val(dataListSubLineBusiness.Description);
        $("#selectLineBussines").UifSelect("setSelected", dataListSubLineBusiness.LineBusinessQuery.Id);
        $("#selectLineBussines").UifSelect("disabled", true);
        $('#modalListSearchSubLineBusiness').UifModal('hide');
    }

    exportExcel() {
        request('Parametrization/SubLineBusiness/GenerateFileToExport', null, 'GET', AppResources.ErrorSearchPaymentPlan, SubLineBusinessParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    static Save() {
        var itemModified = [];
        var dataTable = $("#listViewSearchAdvanced").UifListView('getData');
        $.each(dataTable, function (index, value) {
            if (value.StatusTypeService != undefined && value.StatusTypeService != ParametrizationStatus.Original) {
                itemModified.push(value);
            }
        });
        if (itemModified.length > 0) {
            request('Parametrization/SubLineBusiness/Save', JSON.stringify({ subLineBusiness: itemModified }), 'POST', Resources.Language.ErrorSearch, SubLineBusinessParametrization.ResultSave);
        }
    }
    static ResultSave(data) {
        if (Array.isArray(data.data)) {
            SubLineBusinessParametrization.getTechnicalBranch(data.data);

            if (data.message.Message === "") {
                data.message.Message = 0;
            }
            $.UifNotify('show', {
                'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                AppResources.Aggregates + ':' + data.message.TotalAdded + '<br> ' +
                AppResources.Updated + ':' + data.message.TotalModified + '<br> ' +
                AppResources.Removed + ':' + data.message.TotalDeleted + '<br> ' +
                AppResources.Errors + ':' + data.message.Message,
                'autoclose': true
            });
            
        }
    }
	static deleteCallbackListSubLineBusiness(deferred, result) {
		deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined) //Se elimina unicamente si existe en DB
		{
			result.StatusTypeService = ParametrizationStatus.Delete;
			result.allowEdit = false;
			result.allowDelete = false; 
			//glbSubLineBusinessDelete.push(result);
			$("#listViewSearchAdvanced").UifListView("addItem", result);
        }
    }
}