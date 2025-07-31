//Clase formulario principal
var formTextPretacalogueds = {};
var glbTextPrecataloguedEdit;
class TextPrecataloguedParametrization extends Uif2.Page {
    //Iniciar formulario
    getInitialState()
    {
        //new TextPrecataloguedDetail();
        TextPrecataloguedParametrization.GetLevels();
        TextPrecataloguedParametrization.GetCommercialBranch();
        TextPrecataloguedParametrization.GetCoveredRisk();
        
        $("#listText").UifListView({
            displayTemplate: "#templatelistText",
            source: null,
            selecttionType: 'single',
            height: 400
        });
        TextPrecataloguedParametrizationRequest.GetTextPrecatalogued();
        
        
        
    }
    //eventos Formulario
    bindEvents()
    {
        $('#btnAccept').click(this.TextPrecataloguedAdd);
        $('#btnTextPretacaloguedNew').click(TextPrecataloguedParametrization.clearFormTextPrecatalogued);
        $('#btnSaveTextPRecatalogued').click(TextPrecataloguedParametrization.CreateTextPrecatalogued);
        $('#Coverage').on("search", this.SearchTextCoverage);
        $('#ConditionLevelCode').on('itemSelected', (event, data, index) => { TextPrecataloguedParametrization.eventClick() });
        $('#tblResultListCoverage').on('rowSelected', this.SelectSearchCoverage);
        $('#divBranch').hide();
        $('#divCoveredRisk').hide();
        $("#divCoverage").hide();

        $("#listText").on('rowEdit', function (event, data, position) {           
            TextPrecataloguedParametrization.editTextPrecatalogued(event, data, position);
        })
        $('#btnExportTextPrecatalogued').click(TextPrecataloguedParametrization.sendExcelProtection);
        $('#inputTextPreSearch').on('buttonClick', this.SearchTextPreObject);
        $('#btnExitTextPre').click(TextPrecataloguedParametrization.Exit);
    }
    //Validar dependiendo tipo de nivel mostar 2. ramo Comercial 3. Tipo de Riesgo 5. Cobertura
    static eventClick() {
        console.log('event click');
        switch (parseInt($("#ConditionLevelCode").val())) {
            case TextPrecatalogued.Prefix: // ramo comercial
                $("#divBranch").show();
                $("#divCoveredRisk").hide();
                $("#divCoverage").hide();
                break;
            case TextPrecatalogued.RiskType:// tipo de riesgo
                $("#divCoveredRisk").show();
                $("#divBranch").hide();
                $("#divCoverage").hide();
                break;
            case TextPrecatalogued.Coverage:// Cobertura
                $("#divCoverage").show();
                $("#divBranch").hide();
                $("#divCoveredRisk").hide();
                break;
        }
    }

    static editTextPrecatalogued(event, data, index) {
        glbTextPrecataloguedEdit = data;
        glbTextPrecataloguedEdit.Index = index;
        $("#inputText").val(data.TextTitle);
        $("#ConditionLevelCode").UifSelect('setSelected', data.ConditionLevelCode);
        TextPrecataloguedParametrization.eventClick();
        $("#CommercialBranch").UifSelect('setSelected', data.CommercialBranch);
        $("#CoveredRisk").UifSelect('setSelected', data.CoveredRisk);
        $("#Coverage").val(data.Coverage)
        $("#TextBody").val(data.TextBody);
        $("#ConditionTextId").val(data.ConditionTextId)
        $("#CondTextLevelId").val(data.CondTextLevelId)
    }

    //cargar los niveles
    static GetLevels() {
        TextPrecataloguedParametrizationRequest.GetLevels().done(response => {
            let result = response.result;
            if (response.success) {
                $("#ConditionLevelCode").UifSelect({ sourceData: result });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }
    //Cargar ramos 
    static GetCommercialBranch() {
        TextPrecataloguedParametrizationRequest.GetCommercialBranch().done(response => {
            let result = response.result;
            if (response.success) {
                $("#CommercialBranch").UifSelect({ sourceData: result });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }
    //Cargar riesgos 
    static GetCoveredRisk() {
        TextPrecataloguedParametrizationRequest.GetCoveredRisk().done(response => {
            let result = response.result;
            if (response.success) {
                let comboConfig = { sourceData: result };
                $("#CoveredRisk").UifSelect(comboConfig);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }
    //Cargar Coberturas 
    SearchTextCoverage(event, value) {
        if ($.trim($("#Coverage").val()) != "") {   
            ClauseParametrization.GetTextCoverage($("#Coverage").val());
        }
    }
    //Metodo para pasar de un modal de cobertura a caja de texto.
    SelectSearchCoverage(event, dataCoverage, position) {
        $("#Coverage").data("Object", dataCoverage);
        $("#Coverage").val(dataCoverage.Description);
        $("#ObjectInsurance").val(dataCoverage.InsuredObjectServiceQueryModel.Description);
        $("#Protection").val(dataCoverage.PerilServiceQueryModel.Description);
        $('#modalListSearchCoverage').UifModal('hide');
    }
    //agregar a list View 
    TextPrecataloguedAdd()
    {
        var formTextPretacalogueds = $('#formTextPretacalogued').serializeObject();
        $('#formTextPretacalogued').validate();
        if ($('#formTextPretacalogued').valid())
        {
            var textPretacaloguedNew = {};
            var cadena = $('#inputText').val();
            textPretacaloguedNew.TextTitle = cadena.toUpperCase();
            textPretacaloguedNew.ConditionLevelCode = $('#ConditionLevelCode').UifSelect("getSelected");
            textPretacaloguedNew.DescriptionLevel = $('#ConditionLevelCode option:selected').text();
            if (formTextPretacalogueds.ConditionLevelCode == 2) {
                
                textPretacaloguedNew.DescriptionBranch = $('#CommercialBranch option:selected').text();
                textPretacaloguedNew.CommercialBranch = $('#CommercialBranch').val();
                textPretacaloguedNew.ConditionLevelId = $('#CommercialBranch').val();

            }
            if (formTextPretacalogueds.ConditionLevelCode == 3) {
                textPretacaloguedNew.DescriptionRiskCoverange = $('#CoveredRisk option:selected').text();
                textPretacaloguedNew.CoveredRisk = $('#CoveredRisk').val();
                textPretacaloguedNew.ConditionLevelId = $('#CoveredRisk').val();
            }
            if (formTextPretacalogueds.ConditionLevelCode == 5) {
                textPretacaloguedNew.DescriptionCoverange = $('#Coverage').val();
                textPretacaloguedNew.Coverage = $('#Coverage').val();
                textPretacaloguedNew.ConditionLevelId = $('#Coverage').val();
            }
            textPretacaloguedNew.ConditionTextId = $('#ConditionTextId').val();
            textPretacaloguedNew.TextBody = $('#TextBody').val();
            textPretacaloguedNew.StatusTypeService = $('#StatusTypeService').val();
            if ($('#CondTextLevelId').val() != "")
                textPretacaloguedNew.CondTextLevelId = $('#CondTextLevelId').val();
            else
                textPretacaloguedNew.CondTextLevelId = 0;

            if (textPretacaloguedNew.ConditionTextId > 0) {
                TextPrecataloguedParametrization.UpdateTextPrecatalogued(textPretacaloguedNew);
                
            }
            else {
                TextPrecataloguedParametrization.InsertTextPrecatalogued(textPretacaloguedNew);
                
            }

            TextPrecataloguedParametrization.clearFormTextPrecatalogued();
        }
    }
    static UpdateTextPrecatalogued(formTextPrecatalogueds) {
        formTextPrecatalogueds.StatusTypeService = ParametrizationStatus.Update;
        $("#listText").UifListView("editItem", glbTextPrecataloguedEdit.Index,formTextPrecatalogueds);
    }

    static InsertTextPrecatalogued(formTextPrecatalogueds) {
        formTextPrecatalogueds.StatusTypeService = ParametrizationStatus.Create;
        if (glbTextPrecataloguedEdit != null) {
            $("#listText").UifListView("editItem", glbTextPrecataloguedEdit.Index,formTextPrecatalogueds);
        }
        else {
            $("#listText").UifListView("addItem", formTextPrecatalogueds);
        }
    }
   

    static clearFormTextPrecatalogued()
    {
        ClearValidation("#formTextPretacalogued");
        $("#ConditionTextId").val("");
        $("CondTextLevelId").val("");
        $("#inputText").val("");
        $("#ConditionLevelCode").UifSelect("setSelected", null);
        $("#Coverage").val("");
        $("#TextBody").val("");
        $("#Coverage").data("Object", { Id: null, Description: null });
        $("#CoveredRisk").UifSelect("setSelected", null);
        $("#CommercialBranch").UifSelect("setSelected", null);
        $('#TextPrecatalagued').val("");
        $('#divBranch').hide();
        $('#divCoveredRisk').hide();
        $("#divCoverage").hide();
    }
    static getTextPrecatalogued(data) {

        $("#listText").UifListView({
            displayTemplate: "#templatelistText",
            sourceData: data,
            selectionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: TextPrecataloguedParametrization.deleteCallbackListTextPrecatalogued,
        });
    }
    static deleteCallbackListTextPrecatalogued(deferred, result) {
        deferred.resolve();
        if (result.ConditionTextId !== "" && result.ConditionTextId !== undefined) //Los registros q' esten en DB no se eliminan de la listview se muestra mensaje de eliminado
        {
            result.StatusTypeService = ParametrizationStatus.Delete;
            //glbListClauseDelete.push(result);
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listText").UifListView("addItem", result);
        }

    }
    static CreateTextPrecatalogued()
    {
        var textPrecatalogued = $("#listText").UifListView('getData');

        var texPrecataloguedFilter = textPrecatalogued.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        if (texPrecataloguedFilter.length > 0) {

            TextPrecataloguedParametrizationRequest.CreateTextPrecatalogued(texPrecataloguedFilter).done(function (data) {
                if (data.success) {
                    TextPrecataloguedParametrization.confirmTextPRecatalogued(data.result);
                }
                else {

                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
               
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTextPrecatalogued, 'autoclose': true });
            });            
        }
        
    }
    static confirmTextPRecatalogued(data) {
        glbListClauseDelete = [];
        TextPrecataloguedParametrization.clearFormTextPrecatalogued();
        TextPrecataloguedParametrizationRequest.GetTextPrecatalogued();
        if (data.Message === null) {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': data,
            'autoclose': true
        });
    }

    static sendExcelProtection() {
        TextPrecataloguedParametrization.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TextPrecatalogued/ExportFile',            
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    SearchTextPreObject() {
        var inputText = $('#inputTextPreSearch').val();
        if (inputText.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            request('Parametrization/TextPrecatalogued/GetTextPreByDescription', JSON.stringify({ description: inputText }), 'POST', Resources.Language.ErrorExistTextPre, TextPrecataloguedParametrization.ShowData)            
        }
        $('#inputTextPreSearch').val("");       
    }
    static ShowData(data) {
       
        if (data.length > 0) {
            $("#listText").UifListView("clear");
            $.each(data, function (index, val) {
                $("#listText").UifListView("addItem", val);
            });
        }
        else {

            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorExistTextPre, 'autoclose': true });
            $("#listText").UifListView("refresh");
        }
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }
}