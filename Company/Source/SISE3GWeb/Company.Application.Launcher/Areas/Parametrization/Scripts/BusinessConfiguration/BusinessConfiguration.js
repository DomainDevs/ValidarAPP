var glbBusinessConfiguration = {};
var inputSearch = "";
var BusinessConfigurationIndex = null;
var BusinessConfigurationQueryIndex = null;
var glbBusinessConfigurationListSave = [];
var glbBusinessConfigurationAdded = [];
var glbBusinessConfigurationModified = [];
var glbBusinessConfigurationDelete = [];
var dropDownSearchAdvBusinessConfiguration = null;
var glbCurrentRequestEndorsement = [];
var glbCurrentItem = [];
var glbCurrentItemQuery = [];
var glbCurrentItemQueryDeleted = [];


$(() => {
    new ParametrizationBusinessConfiguration();
});


class BusinessConfigurations {

    /**
    *@summary Obtiene la lista de negocios y acuerdos de negocios
    *@returns {Array} Lista de negocios y acuerdos de negocios
    */
    static GetBusinessConfigurations() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/BusinessConfiguration/GetBusinessConfiguration',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Negocios
    *@returns {Array} Lista de negocios y acuerdos de negocios con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveBusinessConfigurations(glbBusinessConfigurationListSave) {
        return $.ajax({
            type: 'POST',
            url: 'SaveBusinessConfiguration',
            data: JSON.stringify({ listBusinessConfigurationViewModel: glbBusinessConfigurationListSave }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos para generar el Archivo excel de Negocios
    */
    static GenerateBusinessConfigurationFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateBusinessConfigurationFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class BusinessConfigurationControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get BusinessConfigurationTemplate() { return "#BusinessConfigurationTemplate"; }
    static get listBusinessConfiguration() { return $("#listBusinessConfiguration"); }
    static get btnNewBusinessConfiguration() { return $("#btnNewBusinessConfiguration"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnBusinessConfigurationAccept() { return $("#btnBusinessConfigurationAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get tableResults() { return $("#tableResults"); }
    static get formBusinessConfiguration() { return $("#formBusinessConfiguration"); }
    static get formBusinessConfigurationQuery() { return $("#formBusinessConfigurationQuery"); }
    static get btnSaveBusinessConfiguration() { return $("#btnSaveBusinessConfiguration"); }
    static get sendExcelBusinessConfiguration() { return $("#btnExport"); }
    static get btnBusinessConfigurationQuery() { return $("#btnBusinessConfigurationQuery"); }
    static get btnSearchRequest() { return $("#btnSearchRequest"); }
    static get listBusinessConfigurationQuery() { return $("#listBusinessConfigurationQuery"); }
    static get btnNewBusinessConfigurationQuery() { return $("#btnNewBusinessConfigurationQuery"); }
    static get btnAddBusinessConfigurationQuery() { return $("#btnAddBusinessConfigurationQuery"); }
    static get btnAceptBusinessConfigurationQuery() { return $("#btnAceptBusinessConfigurationQuery"); }
    //Controles de Business
    static get inputId() { return $("#inputId"); }
    static get selectPrefixCode() { return $("#selectPrefixCode"); }
    static get inputDescription() { return $("#inputDescription"); }
    static get inputIsEnable() { return $("#inputIsEnable"); }
    static get inputSearchBusinessConfiguration() { return $("#inputSearchBusinessConfiguration"); }
    //Controles de BusinessConfiguration
    static get inputIdBusinessConfiguration() { return $("#inputIdBusinessConfiguration"); }
    static get inputSearchRequest() { return $("#inputSearchRequest"); }
    static get selectProductCode() { return $("#selectProductCode"); }
    static get selectGroupCoverage() { return $("#selectGroupCoverage"); }
    static get selectAssistance() { return $("#selectAssistance"); }
    static get inputProductIdResponse() { return $("#inputProductIdResponse"); }
    static get inputSearchProduct() { return $("#inputSearchProduct"); }
    static get btnAdvancedSearchQuery() { return $("#btnAdvancedSearchQuery"); }
}

class ParametrizationBusinessConfiguration extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //request('Parametrization/BusinessConfiguration/GetBusinessConfiguration', null, 'GET', AppResources.ErrorSearchClauses, BusinessConfigurationControls.GetBusinessConfigurations);

        BusinessConfigurationControls.listBusinessConfiguration.UifListView({
            displayTemplate: "#BusinessConfigurationTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemBusinessConfiguration,
            customAdd: true,
            customEdit: true,
            height: 300
        });
        BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView({
            displayTemplate: "#BusinessConfigurationQueryTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemBusinessConfigurationQuery,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Negocios
        * @returns {Array} Lista de negocios y acuerdos de negocios
        */
        BusinessConfigurations.GetBusinessConfigurations().done(function (data) {
            if (data.success) {
                glbBusinessConfiguration = data.result;
                ParametrizationBusinessConfiguration.LoadBusinessConfigurations();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        PrefixRequest.GetPrefix().done(function (data) {
            if (data.success) {
                $("#selectPrefixCode").UifSelect({ sourceData: data.result, id: "PrefixCode", name: "PrefixDescription", filter: false });
            }
        });
    }


    //---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        BusinessConfigurationControls.inputSearchBusinessConfiguration.on('itemSelected', this.showBusinessConfigurationSerch);
        BusinessConfigurationControls.inputSearchBusinessConfiguration.on("buttonClick", this.showBusinessConfigurationSerch);
        BusinessConfigurationControls.btnBusinessConfigurationAccept.on("click", this.AddItemBusinessConfiguration);
        BusinessConfigurationControls.listBusinessConfiguration.on('rowEdit', ParametrizationBusinessConfiguration.ShowData);
        BusinessConfigurationControls.listBusinessConfiguration.on('rowDelete', this.DeleteItemBusinessConfiguration);
        BusinessConfigurationControls.btnSaveBusinessConfiguration.on("click", this.SaveBusinessConfiguration);
        BusinessConfigurationControls.btnNewBusinessConfiguration.on("click", ParametrizationBusinessConfiguration.CleanForm);
        BusinessConfigurationControls.btnNewBusinessConfigurationQuery.on("click", ParametrizationBusinessConfiguration.CleanFormBusinessConfiguration);
        BusinessConfigurationControls.btnExit.on("click", this.Exit);
        BusinessConfigurationControls.btnSearchRequest.on("click", this.ValidateRequest);
        BusinessConfigurationControls.sendExcelBusinessConfiguration.on("click", this.sendExcelBusinessConfiguration);
        BusinessConfigurationControls.btnBusinessConfigurationQuery.on("click", this.BtnBusinessConfigurationQuery);
        BusinessConfigurationControls.selectPrefixCode.on("change", this.ValidateRulesModal);
        BusinessConfigurationControls.inputDescription.on("change", this.ValidateRulesModal);
        BusinessConfigurationControls.selectPrefixCode.on('itemSelected', this.ValidateRulesModal);
        BusinessConfigurationControls.listBusinessConfigurationQuery.on('rowEdit', ParametrizationBusinessConfiguration.ShowDataQuery);
        BusinessConfigurationControls.listBusinessConfigurationQuery.on('rowDelete', this.DeleteItemBusinessConfigurationQuery);
        BusinessConfigurationControls.btnAddBusinessConfigurationQuery.on("click", this.AddItemBusinessConfigurationQuery);
        BusinessConfigurationControls.btnAceptBusinessConfigurationQuery.on("click", this.SaveBusinessConfigurationQuery);
        BusinessConfigurationControls.selectProductCode.on('itemSelected', this.ValidateProduct);
        BusinessConfigurationControls.inputSearchRequest.on("change", this.ValidateRulesRequest);
        BusinessConfigurationControls.btnAdvancedSearchQuery.on("change", this.AdvancedSearchQuery);
        $(document).on('change', '#inputDescription', function () { this.ValidateRulesModal });
    }



    static ShowSearchAdv(data) {

        $("#lvSearchAdvBusinessConfiguration").UifListView("clear");
        if (data) {
            data.forEach(item => {
                item.PrefixCodeText = item.PrefixCode.PrefixDescription;
                $("#lvSearchAdvBusinessConfiguration").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvBusinessConfiguration.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvBusinessConfiguration.hide();
    }

    /**
    * @summary Carga la lista de Negocios
    */
    static LoadBusinessConfigurations() {
        BusinessConfigurationControls.listBusinessConfiguration.UifListView("clear");
        $.each(glbBusinessConfiguration, function (key, value) {
            var BusinessConfigurations = {
                BusinessId: this.BusinessId,
                Description: this.Description,
                IsEnabled: this.IsEnabled,
                PrefixCodeText: this.PrefixCode.PrefixDescription,
                PrefixCodeId: this.PrefixCode.PrefixCode,
                PrefixCode: this.PrefixCode,
                ListBusinessConfigurationQueryViewModel: this.ListBusinessConfigurationQueryViewModel,
                StatusType: this.StatusType
            }
            BusinessConfigurationControls.listBusinessConfiguration.UifListView("addItem", BusinessConfigurations);
        });
    }

    /**
    * @summary Carga la lista de Acuerdos de Negocios
    */
    static LoadBusinessConfigurationsQuery(data) {
        BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView("clear");
        $.each(data, function (key, value) {
            if (this.Request == null) {
                var BusinessConfigurationsQuery = {
                    BusinessConfigurationId: this.BusinessConfigurationId,
                    BusinessId: this.BusinessId,
                    Request: this.Request,
                    RequestIdCode: null,
                    Product: this.Product,
                    ProductCode: this.Product.ProductId,
                    ProductText: this.Product.ProductDescription,
                    GroupCoverage: this.GroupCoverage,
                    GroupCoverageText: this.GroupCoverage.GroupCoverageSmallDescription,
                    GroupCoverageCode: this.GroupCoverage.GroupCoverageId,
                    Assistance: this.Assistance,
                    AssistanceText: this.Assistance.AssistanceDescription,
                    AssistanceCode: this.Assistance.AssistanceCode,
                    ProductIdResponse: this.ProductIdResponse,
                    StatusType: this.StatusType
                }
            }
            else {
                var BusinessConfigurationsQuery = {
                    BusinessConfigurationId: this.BusinessConfigurationId,
                    BusinessId: this.BusinessId,
                    Request: this.Request,
                    RequestIdCode: this.Request.RequestId,
                    Product: this.Product,
                    ProductCode: this.Product.ProductId,
                    ProductText: this.Product.ProductDescription,
                    GroupCoverage: this.GroupCoverage,
                    GroupCoverageText: this.GroupCoverage.GroupCoverageSmallDescription,
                    GroupCoverageCode: this.GroupCoverage.GroupCoverageId,
                    Assistance: this.Assistance,
                    AssistanceText: this.Assistance.AssistanceDescription,
                    AssistanceCode: this.Assistance.AssistanceCode,
                    ProductIdResponse: this.ProductIdResponse,
                    StatusType: this.StatusType
                }
            }

            BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView("addItem", BusinessConfigurationsQuery);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelBusinessConfiguration() {
        BusinessConfigurations.GenerateBusinessConfigurationFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }
    //----------------------------------------------------------//

    /**
    * @summary Limpia las listas para el CRUD de Negocios
    */
    static RefreshList() {
        glbBusinessConfigurationAdded = [];
        glbBusinessConfigurationModified = [];
        glbBusinessConfigurationDelete = [];
        glbBusinessConfigurationListSave = [];
    }



    //---------------------BUSCAR NEGOCIO-------------------//

    /**
    *@summary Inicia la busqueda de Negocios
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showBusinessConfigurationSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationBusinessConfiguration.SearchBusinessConfiguration(inputSearch);

    }

    /**
     *@summary Realiza la busqueda de Negocios
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchBusinessConfiguration(description) {
        var find = false;
        var data = [];
        var search = glbBusinessConfiguration;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            $.each(search, function (key, value) {
                if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                    includes(description.toLowerCase().sistranReplaceAccentMark()))
                ) {
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });


            if (find == false) {
                $.UifNotify('show',
                    {
                        'type': 'danger', 'message': Resources.Language.BusinessConfigurationNotFound, 'autoclose': true
                    })
            } else {
                ParametrizationBusinessConfiguration.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra los negocios Encontrados
    *@returns {object} *Obtiene el negocio encontrado y lo muestra 
    *@returns {Array} *Obtiene la lista de negocios y acuerdos de negocios que coiniciden con la busqueda
    */
    static ShowData(event, result, index) {
        glbCurrentItem = [];
        ParametrizationBusinessConfiguration.CleanForm();
        ParametrizationBusinessConfiguration.CleanFormBusinessConfiguration();
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ParametrizationBusinessConfiguration.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            BusinessConfigurationIndex = index;
            BusinessConfigurationControls.inputDescription.val(resultUnique.Description);
            $("#selectPrefixCode").UifSelect("setSelected", resultUnique.PrefixCode.PrefixCode);
            BusinessConfigurationControls.inputId.val(resultUnique.BusinessId);
            if (resultUnique.IsEnabled == true) {
                BusinessConfigurationControls.inputIsEnable.prop('checked', true);
            }
            else {
                BusinessConfigurationControls.inputIsEnable.prop('checked', false);
            }
            $("#btnBusinessConfigurationQuery").prop('disabled', false);
            if (resultUnique.BusinessId > 0) {
                $('#selectPrefixCode').UifSelect("disabled", true);
            }
            else {
                $('#selectPrefixCode').UifSelect("disabled", false);
            }
            glbCurrentItem = resultUnique;
            var businessQueryResult = [];
            $.each(resultUnique.ListBusinessConfigurationQueryViewModel, function (key, value) {
                if (this.StatusType == 4) {
                    glbCurrentItemQueryDeleted.push(this);
                }
                else {
                    businessQueryResult.push(this);
                }
            });
            ParametrizationBusinessConfiguration.LoadBusinessConfigurationsQuery(businessQueryResult);
        }
    }

    /**
    *@summary Muestra los acuerdos de negocios Encontrados
    *@returns {object} *Obtiene el acuerdo de negocio encontrado y lo muestra 
    *@returns {Array} *Obtiene la lista de acuerdos de negocios que coiniciden con la busqueda
    */
    static ShowDataQuery(event, result, index) {
        ParametrizationBusinessConfiguration.CleanFormBusinessConfiguration()
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            BusinessConfigurationQueryIndex = index;
            BusinessConfigurationControls.inputIdBusinessConfiguration.val(resultUnique.BusinessConfigurationId);
            if (resultUnique.Request == null) {
                BusinessConfigurationControls.inputSearchRequest.val("");
            }
            else {
                BusinessConfigurationControls.inputSearchRequest.val(resultUnique.Request.RequestId);
            };
            BusinessConfigurationControls.inputProductIdResponse.val(resultUnique.ProductIdResponse);
            ProductRequest.GetCurrentProductByPrefixCode($('#selectPrefixCode').UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    $("#selectProductCode").UifSelect({ sourceData: data.result, id: "ProductId", name: "ProductDescription", native: false, filter: true });
                    $("#selectProductCode").UifSelect("setSelected", resultUnique.Product.ProductId);
                    if (resultUnique.Request == null) {
                        $("#selectProductCode").UifSelect("disabled", false);
                    }
                    else {
                        $("#selectProductCode").UifSelect("disabled", true);
                    };
                    if ($('#selectProductCode').UifSelect("getSelected") == "") {
                        $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                        $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                    }
                    else {
                        GroupCoverageRequest.GetGroupCoverageByProductCode($('#selectProductCode').UifSelect("getSelected")).done(function (data) {
                            if (data.success) {
                                if (data.result.length > 0) {
                                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                                    $("#selectGroupCoverage").UifSelect("setSelected", resultUnique.GroupCoverage.GroupCoverageId);
                                }
                                else {
                                    $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                                }
                            }
                        });
                        AssistanceTypeRequest.GetAssistanceTypeByProductCode($('#selectProductCode').UifSelect("getSelected")).done(function (data) {
                            if (data.success) {
                                if (data.result.length > 0) {
                                    $("#selectAssistance").UifSelect({ sourceData: data.result, id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                                    $("#selectAssistance").UifSelect("setSelected", resultUnique.Assistance.AssistanceCode);
                                }
                                else {
                                    $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                                }
                            }
                        });
                    }
                }
            });
            glbCurrentItemQuery = resultUnique;
        }
    }
    /**
    *@summary Muestra los negocios y acuerdos de negocios Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de negocios y acuerdos de negocios encontrados
    */
    static ShowDefaultResult(dataTable) {
        BusinessConfigurationControls.tableResults.UifDataTable('clear');
        BusinessConfigurationControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR NEGOCIO-------------------//

    /**
    *@summary Obtiene el negocio seleccionado
    *@param {id} * ID del negocio
    */
    static GetBusinessConfiguration(id) {
        var find = false;
        var data = [];
        var search = glbBusinessConfiguration;
        $.each(search, function (key, value) {
            if (value.BusinessId == id) {
                BusinessConfigurationIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationBusinessConfiguration.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.BusinessConfigurationNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un negocio al listview y al array de negocios y acuerdos de negocios por agregar 
    */
    AddItemBusinessConfiguration() {
        BusinessConfigurationControls.formBusinessConfiguration.validate();
        if (BusinessConfigurationControls.formBusinessConfiguration.valid()) {
            if (glbCurrentItem != "") {
                var businessConfiguration = glbCurrentItem;
            }
            else {
                var businessConfiguration = {
                    BusinessId: null,
                    Description: null,
                    IsEnabled: null,
                    PrefixCode: {},
                    ListBusinessConfigurationQueryViewModel: []
                }
            }
            if ($(BusinessConfigurationControls.inputIsEnable).is(":checked")) {
                businessConfiguration.BusinessId = BusinessConfigurationControls.inputId.val();
                businessConfiguration.Description = BusinessConfigurationControls.inputDescription.val();
                businessConfiguration.PrefixCode.PrefixCode = $('#selectPrefixCode').UifSelect("getSelected");
                businessConfiguration.PrefixCode.PrefixDescription = $("#selectPrefixCode option:selected").html();
                businessConfiguration.PrefixCode.SmallDescription = $("#selectPrefixCode option:selected").html();
                businessConfiguration.PrefixCodeText = $("#selectPrefixCode option:selected").html();
                businessConfiguration.IsEnabled = true;
            }
            else {
                businessConfiguration.BusinessId = BusinessConfigurationControls.inputId.val();
                businessConfiguration.Description = BusinessConfigurationControls.inputDescription.val();
                businessConfiguration.PrefixCode.PrefixCode = $('#selectPrefixCode').UifSelect("getSelected");
                businessConfiguration.PrefixCode.PrefixDescription = $("#selectPrefixCode option:selected").html();
                businessConfiguration.PrefixCode.SmallDescription = $("#selectPrefixCode option:selected").html();
                businessConfiguration.PrefixCodeText = $("#selectPrefixCode option:selected").html();
                businessConfiguration.IsEnabled = false;
            }

            if (BusinessConfigurationIndex == null) {
                var list = BusinessConfigurationControls.listBusinessConfiguration.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.Description.toLowerCase().sistranReplaceAccentMark()
                        == businessConfiguration.Description.toLowerCase().sistranReplaceAccentMark();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorExistBusinessConfigurationName, 'autoclose': true
                    });
                }
                else {
                    businessConfiguration.StatusType = ParametrizationStatus.Create;
                    BusinessConfigurationControls.listBusinessConfiguration.UifListView("addItem", businessConfiguration);

                }
            }
            else {
                if (businessConfiguration.BusinessId != 0) {
                    businessConfiguration.StatusType = ParametrizationStatus.Update;
                    $.each(glbCurrentItemQueryDeleted, function (key, value) {
                        businessConfiguration.ListBusinessConfigurationQueryViewModel.push(this);
                    });
                } else {
                    businessConfiguration.StatusType = ParametrizationStatus.Create;
                }

                BusinessConfigurationControls.listBusinessConfiguration.UifListView('editItem', BusinessConfigurationIndex, businessConfiguration);
            }
            glbCurrentItemQueryDeleted = [];
			ParametrizationBusinessConfiguration.CleanForm();			
        }
    }

    /**
*@summary Agrega un acuerdo de negocio al listview y al array de negocios y acuerdos de negocios por agregar 
*/
    AddItemBusinessConfigurationQuery() {
        BusinessConfigurationControls.formBusinessConfigurationQuery.validate();
        if (BusinessConfigurationControls.formBusinessConfigurationQuery.valid()) {
            if (glbCurrentItemQuery != "") {
                var businessConfigurationQuery = glbCurrentItemQuery;
            }
            else {
                var businessConfigurationQuery = {
                    BusinessConfigurationId: null,
                    BusinessId: null,
                    Request: {},
                    Product: {},
                    GroupCoverage: {},
                    Assistance: {},
                    ProductIdResponse: null,
                    StatusType: null
                }
            }
            businessConfigurationQuery.BusinessConfigurationId = BusinessConfigurationControls.inputIdBusinessConfiguration.val();
            businessConfigurationQuery.BusinessId = BusinessConfigurationControls.inputId.val();
            if (BusinessConfigurationControls.inputSearchRequest.val() == "" || BusinessConfigurationControls.inputSearchRequest.val == null) {
                businessConfigurationQuery.Request = null;
                businessConfigurationQuery.RequestIdCode = null;
            }
            else {
                businessConfigurationQuery.Request.RequestEndorsementId = 0;
                businessConfigurationQuery.Request.RequestId = BusinessConfigurationControls.inputSearchRequest.val();
                businessConfigurationQuery.RequestIdCode = BusinessConfigurationControls.inputSearchRequest.val();
                businessConfigurationQuery.Request.ProductId = 0;
                businessConfigurationQuery.Request.PrefixCode = 0;

            }
            businessConfigurationQuery.Product.ProductId = $('#selectProductCode').UifSelect("getSelected");
            businessConfigurationQuery.ProductCode = $('#selectProductCode').UifSelect("getSelected");
            businessConfigurationQuery.Product.ProductDescription = $("#selectProductCode option:selected").html();
            businessConfigurationQuery.ProductText = $("#selectProductCode option:selected").html();
            businessConfigurationQuery.Product.ProductSmallDescription = $("#selectProductCode option:selected").html();
            businessConfigurationQuery.Product.ActiveProduct = true;
            businessConfigurationQuery.GroupCoverage.GroupCoverageId = $('#selectGroupCoverage').UifSelect("getSelected");
            businessConfigurationQuery.GroupCoverageCode = $('#selectGroupCoverage').UifSelect("getSelected");
            businessConfigurationQuery.GroupCoverageText = $("#selectGroupCoverage option:selected").html();
            businessConfigurationQuery.GroupCoverage.GroupCoverageSmallDescription = $("#selectGroupCoverage option:selected").html();
            businessConfigurationQuery.AssistanceCode = $('#selectAssistance').UifSelect("getSelected");
            businessConfigurationQuery.Assistance.AssistanceCode = $('#selectAssistance').UifSelect("getSelected");
            businessConfigurationQuery.AssistanceText = $("#selectAssistance option:selected").html();
            businessConfigurationQuery.Assistance.AssistanceDescription = $("#selectAssistance option:selected").html();
            businessConfigurationQuery.ProductIdResponse = BusinessConfigurationControls.inputProductIdResponse.val();
            if (BusinessConfigurationQueryIndex == null) {
                var list = BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return (item.Product.ProductId
                        == businessConfigurationQuery.Product.ProductId) &&
                        (item.GroupCoverage.GroupCoverageId == businessConfigurationQuery.GroupCoverage.GroupCoverageId) &&
                        (item.Assistance.AssistanceCode == businessConfigurationQuery.Assistance.AssistanceCode);
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorExistBusinessConfiguration, 'autoclose': true
                    });
                }
                else {
                    businessConfigurationQuery.StatusType = ParametrizationStatus.Create;
                    businessConfigurationQuery.Create = true;
                    BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView("addItem", businessConfigurationQuery);

                }
            }
            else {
                if (businessConfigurationQuery.BusinessConfigurationId != 0) {
                    businessConfigurationQuery.StatusType = ParametrizationStatus.Update;
                } else {
                    businessConfigurationQuery.StatusType = ParametrizationStatus.Create;
                }

                BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView('editItem', BusinessConfigurationQueryIndex, businessConfigurationQuery);
            }
            ParametrizationBusinessConfiguration.CleanFormBusinessConfiguration();
        }
    }

    /**
    *@summary Elimina un negocio del listview y lo agrega al array de negocios y acuerdos de negocios por eliminiar 
    */
    DeleteItemBusinessConfiguration(event, data) {
        event.resolve();
        if (data.BusinessId != 0) {
            data.StatusType = ParametrizationStatus.Delete;
            //glbBusinessConfigurationDelete.push(data);
            data.allowEdit = false;
            data.allowDelete = false;
            $("#listBusinessConfiguration").UifListView("addItem", data);
        }
       
        ParametrizationBusinessConfiguration.CleanForm();
    }

    /**
    *@summary Elimina un acuerdo de negocio del listview y actualiza el array de negocios y acuerdos de negocios por eliminiar 
    */
    DeleteItemBusinessConfigurationQuery(event, data) {
        
        if (data.Id != 0) {
            data.StatusType = ParametrizationStatus.Delete;
            glbCurrentItemQueryDeleted.push(data);
           
        }
        event.resolve();
        ParametrizationBusinessConfiguration.CleanFormBusinessConfiguration();
    }

    
    /**
    *@summary Inicia los procesos del CRUD para los Negocios con los arrays cargados 
    */
    SaveBusinessConfiguration() {
        glbBusinessConfigurationListSave = [];
        var businessData = BusinessConfigurationControls.listBusinessConfiguration.UifListView('getData');
        $.each(businessData, function (key, value) {
            if (this.StatusType == 2) {
                glbBusinessConfigurationListSave.push(this);
            }
            else if (this.StatusType == 3) {
                glbBusinessConfigurationListSave.push(this);
            }
            else if (this.StatusType == 4) {
                glbBusinessConfigurationListSave.push(this);
            }
        });
        $.each(glbBusinessConfigurationDelete, function (key, value) {
            glbBusinessConfigurationListSave.push(this);
        });
        if (glbBusinessConfigurationListSave.length > 0) {
            BusinessConfigurations.SaveBusinessConfigurations(glbBusinessConfigurationListSave)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationBusinessConfiguration.RefreshList();
                        glbBusinessConfiguration = data.result.data;
                        ParametrizationBusinessConfiguration.LoadBusinessConfigurations();
                        var itemEdit = [];                        
                        $.each(businessData, function (key, value) {
                            if (this.StatusType == 3 || this.StatusType == 2) {                                
                                if (this.StatusType == 2) {                                    
                                    itemEdit.push(glbBusinessConfiguration.filter(x => x.Description == this.Description).shift()); 
                                }
                                else {
                                    this.StatusType = 1;
                                    itemEdit.push(this); 
                                }                                
                            }
                        });
                        $.each(businessData, function (key, value) {
                            var index = businessData.indexOf(this);
                            var item = this;
                            $.each(itemEdit, function (key, value) {
                                if (this.Description == item.Description) {                                    
                                    BusinessConfigurationControls.listBusinessConfiguration.UifListView('editItem', index, this);
                                }
                            })  
                        });
                        
                        $.UifNotify('show', {                        
                            'type': 'info', 'message': data.result.message, 'autoclose': true
                        });
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': data.result, 'autoclose': true
                        });
                    }
                })
                .fail(function () {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorSaveBusinessConfigurations, 'autoclose': true
                    })

                });
        }

    }

    /**
    *@summary Actualiza los procesos del CRUD para los acuerdos de negocios con los arrays cargados 
    */
    SaveBusinessConfigurationQuery() {
        var list = BusinessConfigurationControls.listBusinessConfigurationQuery.UifListView('getData');
        glbCurrentItem.ListBusinessConfigurationQueryViewModel = list;
        ParametrizationBusinessConfiguration.CleanFormBusinessConfiguration();
        $('#modalBusinessConfigurationQuery').UifModal('hide');
    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista Negocios
    */
    static CleanForm() {
        ClearValidation("#formBusinessConfiguration");
        BusinessConfigurationIndex = null;
        BusinessConfigurationControls.inputDescription.val(null);
        BusinessConfigurationControls.inputSearchBusinessConfiguration.val(null);
        BusinessConfigurationControls.inputId.val(null);
        BusinessConfigurationControls.inputIsEnable.prop('checked', false);
        $("#selectPrefixCode").UifSelect("setSelected", null);
        $('#selectPrefixCode').UifSelect("disabled", false);
        $("#btnBusinessConfigurationQuery").prop('disabled', true);
        glbCurrentItem = [];
    }

    /**
    *@summary Limpia los controles de la vista Acuerdos de Negocios
    */
    static CleanFormBusinessConfiguration() {
        
        $('#inputSearchRequest').val(null);
        BusinessConfigurationQueryIndex = null;
        BusinessConfigurationControls.inputSearchRequest.val(null);
        BusinessConfigurationControls.inputProductIdResponse.val(null);
        BusinessConfigurationControls.inputIdBusinessConfiguration.val(null);
        $('#selectProductCode').UifSelect("disabled", false);
        $("#selectProductCode").UifSelect("setSelected", null);
        $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
        $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
        $("#selectProductCode-error").remove();
        $("#selectGroupCoverage-error").remove();
        $("#selectAssistance-error").remove();
        glbCurrentItemQuery = [];
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    /**
   *@summary Valida el formulario principal para habilitar el boton de acuerdos
   */
    ValidateRulesModal() {
        var isValid = $('#formBusinessConfiguration').validate().checkForm();
        if (isValid) {
            $("#btnBusinessConfigurationQuery").prop('disabled', false);
        }
        else {
            $("#btnBusinessConfigurationQuery").prop('disabled', true);
        }
    }

    ValidateRulesRequest() {
        var requestGet = $("#inputSearchRequest").val();
        if (requestGet == "") {
            $('#selectProductCode').UifSelect("disabled", false);
        }
        else {
            $("#selectProductCode").UifSelect("setSelected", null);
            $('#selectProductCode').UifSelect("disabled", true);
        }
    }
    /**
    *@summary Valida el formulario modal para habilitar el listado de coverturas y asistencias
    */
    ValidateProduct() {
        BusinessConfigurationControls.inputSearchRequest.val("");
        if ($('#selectProductCode').UifSelect("getSelected") == "") {
            $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
            $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
        }
        else {
            GroupCoverageRequest.GetGroupCoverageByProductCode($('#selectProductCode').UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        $("#selectGroupCoverage").UifSelect({ sourceData: data.result, id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                    }
                    else {
                        $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                    }
                }
            });
            AssistanceTypeRequest.GetAssistanceTypeByProductCode($('#selectProductCode').UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        $("#selectAssistance").UifSelect({ sourceData: data.result, id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                    }
                    else {
                        $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                    }
                }
            });
        }
    }

    /**
    *@summary Valida la solicitud agrupadora digitada
    */
    ValidateRequest() {
        var list = glbCurrentRequestEndorsement;
        var ifExist = list.filter(function (item) {
            return item.RequestId
                == BusinessConfigurationControls.inputSearchRequest.val();
        });
        if (ifExist.length == 0) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorQueryingGroupRequest, 'autoclose': true
            });
        }
        else {
            var resultUnique = null;
            if (ifExist.length == 1) {
                resultUnique = ifExist[0];
            }
            else if (ifExist.length == undefined) {
                resultUnique = ifExist;
            }
            if (resultUnique != null) {
                ProductRequest.GetCurrentProductByPrefixCode($('#selectPrefixCode').UifSelect("getSelected")).done(function (data) {
                    if (data.success) {
                        $("#selectProductCode").UifSelect({ sourceData: data.result, id: "ProductId", name: "ProductDescription", native: false, filter: true });
                        $("#selectProductCode").UifSelect("setSelected", resultUnique.ProductId);
                        if ($('#selectProductCode').UifSelect("getSelected") == "") {
                            $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                            $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                        }
                        else {
                            GroupCoverageRequest.GetGroupCoverageByProductCode($('#selectProductCode').UifSelect("getSelected")).done(function (data) {
                                if (data.success) {
                                    if (data.result.length > 0) {
                                        $("#selectGroupCoverage").UifSelect({ sourceData: data.result, id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                                    }
                                    else {
                                        $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                                    }
                                }
                            });
                            AssistanceTypeRequest.GetAssistanceTypeByProductCode($('#selectProductCode').UifSelect("getSelected")).done(function (data) {
                                if (data.success) {
                                    if (data.result.length > 0) {
                                        $("#selectAssistance").UifSelect({ sourceData: data.result, id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                                    }
                                    else {
                                        $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
                                    }
                                }
                            });
                        }
                    }
                });
            }
            else {
                $.UifNotify('show', {
                    'type': 'danger', 'message': Resources.Language.ErrorQueryingGroupRequest, 'autoclose': true
                });
            }

        }
    }


    BtnBusinessConfigurationQuery() {
        BusinessConfigurationControls.inputSearchRequest.val("");
        glbCurrentRequestEndorsement = [];
        RequestEndorsement.GetCurrentRequestEndorsementByPrefixCode($('#selectPrefixCode').UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                glbCurrentRequestEndorsement = data.result;
            }
        });
        ProductRequest.GetCurrentProductByPrefixCode($('#selectPrefixCode').UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                $("#selectProductCode").UifSelect({ sourceData: data.result, id: "ProductId", name: "ProductDescription", native: false, filter: true });
                $("#selectGroupCoverage").UifSelect({ sourceData: [{ "GroupCoverageId": "", "GroupCoverageSmallDescription": "" }], id: "GroupCoverageId", name: "GroupCoverageSmallDescription", native: false, filter: false });
                $("#selectAssistance").UifSelect({ sourceData: [{ "AssistanceCode": "", "AssistanceDescription": "" }], id: "AssistanceCode", name: "AssistanceDescription", native: false, filter: false });
            }
        });
        if (glbCurrentItem != "") {
        }
        else {
            var businessConfiguration = {
                BusinessId: null,
                Description: null,
                IsEnabled: null,
                PrefixCode: {},
                ListBusinessConfigurationQueryViewModel: []
            }
            glbCurrentItem = businessConfiguration;
        }
        var currentDescription = $('#inputDescription').val();
        var currentCode = $('#inputId').val();
        var titleModal = Resources.Language.LabelBusinessConfiguration + ': ' + currentDescription + '(' + currentCode + ')';
        $('#modalBusinessConfigurationQuery').UifModal('showLocal', titleModal);
        $(".modal-title:first").text(titleModal);
    }
}
function checknumber(e, value) {
    //Valida Caracteres
    var unicode = e.charCode ? e.charCode : e.keyCode;
    if (unicode > 31 && (unicode < 48 || unicode > 57))
        return false;
}
class PrefixRequest {
    static GetPrefix() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/BusinessConfiguration/GetPrefixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class RequestEndorsement {
    static GetCurrentRequestEndorsementByPrefixCode(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/BusinessConfiguration/GetCurrentRequestEndorsementByPrefixCode",
            data: JSON.stringify({
                prefixCode: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class ProductRequest {
    static GetCurrentProductByPrefixCode(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/BusinessConfiguration/GetCurrentProductByPrefixCode",
            data: JSON.stringify({
                prefixCode: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class GroupCoverageRequest {
    static GetGroupCoverageByProductCode(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/BusinessConfiguration/GetGroupCoverageByProductCode",
            data: JSON.stringify({
                productCode: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class AssistanceTypeRequest {
    static GetAssistanceTypeByProductCode(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/BusinessConfiguration/GetAssistanceTypeByProductCode",
            data: JSON.stringify({
                productCode: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}