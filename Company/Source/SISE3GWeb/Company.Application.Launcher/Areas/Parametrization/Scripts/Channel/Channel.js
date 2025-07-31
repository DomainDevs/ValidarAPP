var glbChannel = {};
var inputSearch = "";
var modalListType;
var ChannelIndex = null;
var glbChannelAdded = [];
var glbChannelModified = [];
var glbChannelDelete = [];
var dropDownSearchAdvChannel = null;

$(() => {
    new ParametrizationChannel();
});

class Channels {
    /**
    *@summary Obtiene la lista de Sucursales
    *@returns {Array} Lista de canales
    */
    static GetChannels() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Channel/GetChannels',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para las Sucursales
    *@returns {Array} Lista de canales con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveChannels(glbChannelAdded, glbChannelModified, glbChannelDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveServiceQuotationSources',
            data: JSON.stringify({ listAdded: glbChannelAdded, listModified: glbChannelModified, listDeleted: glbChannelDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para las Sucursales
    */
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetServiceQuotationParameterBySourceCode(sourceCode) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Channel/GetServiceQuotationParameterBySourceCode',
            data: JSON.stringify({ sourceCode: sourceCode }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class ChannelControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get ChannelTemplate() { return "#ChannelTemplate"; }
    static get listChannel() { return $("#listChannel"); }
    static get btnNewChannel() { return $("#btnNewChannel"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnChannelAccept() { return $("#btnChannelAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputId() { return $("#inputId"); }
    static get inputDescription() { return $("#inputDescription"); }
    static get inputIsEnabled() { return $("#inputIsEnabled"); }
    static get inputIsScore() { return $("#inputIsScore"); }
    static get inputIsFine() { return $("#inputIsFine"); }
    static get inputSearchChannel() { return $("#inputSearchChannel"); }
    static get tableResults() { return $("#tableResults"); }
    static get formChannel() { return $("#formChannel"); }
    static get btnSaveChannel() { return $("#btnSaveChannel"); }
    static get sendExcelChannel() { return $("#btnExport"); }
}

class ParametrizationChannel extends Uif2.Page {
    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        ChannelControls.listChannel.UifListView({
            displayTemplate: "#ChannelTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemChannel,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Sucursales
        * @returns {Array} Lista de canales
        */
        Channels.GetChannels().done(function (data) {
            if (data.success) {
                glbChannel = data.result;
                ParametrizationChannel.LoadChannels();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
                
    }
    
    //---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        ChannelControls.inputSearchChannel.on('itemSelected', this.showChannelSerch);
        ChannelControls.inputSearchChannel.on("buttonClick", this.showChannelSerch);
        ChannelControls.btnChannelAccept.on("click", this.AddItemChannel);
        ChannelControls.listChannel.on('rowEdit', ParametrizationChannel.ShowData);
        ChannelControls.listChannel.on('rowDelete', this.DeleteItemChannel);
        ChannelControls.btnSaveChannel.on("click", this.SaveChannel);
        ChannelControls.btnNewChannel.on("click", ParametrizationChannel.CleanForm);
        ChannelControls.btnExit.on("click", this.Exit);
        ChannelControls.sendExcelChannel.on("click", this.sendExcelChannel);
    }

    static GetServiceQuotationParameterBySourceCode(canal) {
        //alert(canal.Id)
        if (canal.Id != "") {
            Channels.GetServiceQuotationParameterBySourceCode(canal.Id).done(function (data) {
                //alert(JSON.stringify(data))
                if (data.success) {
                    valuesDefault = data.result;
                }
            });
        } else {
            valuesDefault = canal.ValuesDefault;
        }
    }
    
    static ShowSearchAdv(data) {
        $("#lvSearchAdvChannel").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvChannel").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvChannel.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvChannel.hide();
    }
    
    /**
    * @summary Carga la lista de Sucursales
    */
    static LoadChannels() {
        //alert(JSON.stringify(glbChannel))
        ChannelControls.listChannel.UifListView("clear");
        $.each(glbChannel, function (key, value) {
            var servicios = "";
            if (this.IsScore && this.IsFine) {
                servicios = 'Servicios: Data Crédito, Multas';
            } else if (this.IsScore && !this.IsFine) {
                servicios = 'Servicios: Data Crédito';
            }
            else if (!this.IsScore && this.IsFine) {
                servicios = 'Servicios: Multas';
            }
            var Channels = {
                Id: this.Id,
                DetailDescription: this.DetailDescription,
                IsEnabled: this.IsEnabled,
                IsScore: this.IsScore,
                IsFine: this.IsFine,
                servicios: servicios
            }
            ChannelControls.listChannel.UifListView("addItem", Channels);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelChannel() {
        Channels.GenerateFileToExport().done(function (data) {
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
    * @summary Limpia las listas para el CRUD de Sucursales
    */
    static RefreshList() {
        glbChannelAdded = [];
        glbChannelModified = [];
        glbChannelDelete = [];
    }
    
    //---------------------BUSCAR SUCURSAL-------------------//

    /**
    *@summary Inicia la busqueda de Sucursales
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showChannelSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationChannel.SearchChannel(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de Sucursales
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchChannel(description) {
        var find = false;
        var data = [];
        var search = glbChannel;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            $.each(search, function (key, value) {
                if ((value.DetailDescription.toLowerCase().sistranReplaceAccentMark().
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
                            'type': 'danger', 'message': Resources.Language.ChannelNotFound, 'autoclose': true
                        })
            } else {
                ParametrizationChannel.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra las canales Encontradas
    *@returns {object} *Obtiene la canal encontrada y la muestra 
    *@returns {Array} *Obtiene la lista de canales que coiniciden con la busqueda
    */
    static ShowData(event, result, index) {
        //alert(8)
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ParametrizationChannel.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            ChannelIndex = index;
            ChannelControls.inputId.val(resultUnique.Id);
            ChannelControls.inputDescription.val(resultUnique.DetailDescription);
            if (resultUnique.IsEnabled) {
                ChannelControls.inputIsEnabled.prop("checked", resultUnique.IsEnabled);
            } else {
                ChannelControls.inputIsEnabled.prop("checked", resultUnique.IsEnabled);
            }
            if (resultUnique.IsScore) {
                ChannelControls.inputIsScore.prop("checked", resultUnique.IsScore);
            } else {
                ChannelControls.inputIsScore.prop("checked", resultUnique.IsScore);
            }
            if (resultUnique.IsFine) {
                ChannelControls.inputIsFine.prop("checked", resultUnique.IsFine);
            } else {
                ChannelControls.inputIsFine.prop("checked", resultUnique.IsFine);
            }

            ParametrizationChannel.GetServiceQuotationParameterBySourceCode(resultUnique);
        }
    }

    /**
    *@summary Muestra las canales Encontradas
    *@param {dataTable} * Captura el control donde se mostrara la lista de canales encontradas
    */
    static ShowDefaultResult(dataTable) {
        ChannelControls.tableResults.UifDataTable('clear');
        ChannelControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR SUCURSAL-------------------//

    /**
    *@summary Obtiene la canal seleccionada
    *@param {id} * ID de la canal
    */
    static GetChannel(id) {
        var find = false;
        var data = [];
        var search = glbChannel;
        $.each(search, function (key, value) {
            if (value.Id == id) {
                ChannelIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationChannel.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ProtectionNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega una canal al listview y al array de canales por agregar 
    */
    AddItemChannel() {
        ChannelControls.formChannel.validate();
        if (ChannelControls.formChannel.valid()) {
            if (!$.isEmptyObject(valuesDefault)) {
                var servicios = "";
                if (ChannelControls.inputIsScore.is(':checked') && ChannelControls.inputIsFine.is(':checked')) {
                    servicios = 'Servicios: Data Crédito, Multas';
                }
                else if (ChannelControls.inputIsScore.is(':checked') && !ChannelControls.inputIsFine.is(':checked')) {
                    servicios = 'Servicios: Data Crédito';
                }
                else if (!ChannelControls.inputIsScore.is(':checked') && ChannelControls.inputIsFine.is(':checked')) {
                    servicios = 'Servicios: Multas';
                }
                var channel = {
                    Comments: " ",
                    Id: ChannelControls.inputId.val(),
                    Description: ChannelControls.inputDescription.val(),
                    DetailDescription: ChannelControls.inputDescription.val(),
                    IsEnabled: ChannelControls.inputIsEnabled.is(':checked'),
                    IsScore: ChannelControls.inputIsScore.is(':checked'),
                    IsFine: ChannelControls.inputIsFine.is(':checked'),
                    ValuesDefault: valuesDefault,
                    servicios: servicios
                }
                if (ChannelIndex == null) {
                    var list = ChannelControls.listChannel.UifListView('getData');
                    var ifExist = list.filter(function (item) {
                        return item.DetailDescription.toLowerCase().sistranReplaceAccentMark()
                            == channel.DetailDescription.toLowerCase().sistranReplaceAccentMark();
                    });
                    if (ifExist.length > 0) {
                        $.UifNotify('show', {
                            'type': 'danger', 'message': Resources.Language.ErrorExistChannelName, 'autoclose': true
                        });
                    }
                    else {
                        channel.Status = ParametrizationStatus.Create;
                        ChannelControls.listChannel.UifListView("addItem", channel);
                    }
                }
                else {
                    if (channel.Id != "") {
                        channel.Status = ParametrizationStatus.Update;
                    } else {
                        channel.Status = ParametrizationStatus.Create;
                    }

                    ChannelControls.listChannel.UifListView('editItem', ChannelIndex, channel);
                }
                ParametrizationChannel.CleanForm();
            }
            else {
                $.UifNotify('show', {
                    'type': 'danger', 'message': "Los valores por defecto son obligatorios", 'autoclose': true
                });
            }
        }
    }

    /**
    *@summary Elimina una canal del listview y lo agrega al array de canales por eliminiar 
    */
    DeleteItemChannel(event, data) {
        event.resolve();
        if (data.Id != "" && data.Id != 0 ) {
            data.Status = ParametrizationStatus.Delete;
            //glbChannelDelete.push(data);
            data.allowEdit = false;
            data.allowDelete = false;
            $("#listChannel").UifListView("addItem", data);
        }
       
        ParametrizationChannel.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para las Sucursales con los arrays cargados 
    */
    SaveChannel() {
        var channels = ChannelControls.listChannel.UifListView('getData');
        $.each(channels, function (key, value) {
            if (this.Status == ParametrizationStatus.Create) {
                glbChannelAdded.push(this);
            }
            else if (this.Status == ParametrizationStatus.Update) {
                glbChannelModified.push(this);
            }
            else if (this.Status == ParametrizationStatus.Delete) {
                glbChannelDelete.push(this);
            }
        });
        //alert(JSON.stringify(glbChannelAdded))
        //alert(JSON.stringify(glbChannelModified))
        if (glbChannelAdded.length > 0 || glbChannelModified.length > 0 || glbChannelDelete.length > 0) {
            Channels.SaveChannels(glbChannelAdded, glbChannelModified, glbChannelDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationChannel.RefreshList();
                        glbChannel = data.result.data;
                        ParametrizationChannel.LoadChannels();
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
                        'type': 'danger', 'message': Resources.Language.ErrorSaveChannel, 'autoclose': true
                    })
                });
        }
    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        ChannelIndex = null;
        ChannelControls.inputId.val(null);
        ChannelControls.inputDescription.val(null);
        ChannelControls.inputDescription.focus();
        ChannelControls.inputSearchChannel.val(null);
        ChannelControls.inputIsEnabled.prop("checked", false);
        ChannelControls.inputIsScore.prop("checked", false);
        ChannelControls.inputIsFine.prop("checked", false);
        valuesDefault = {};
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

