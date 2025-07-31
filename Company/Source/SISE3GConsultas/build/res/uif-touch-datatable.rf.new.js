/*global jQuery*/
/*global $*/
/*DataTable Plugin*/
/*DataTable Version 1.10.16*/
(function ($) {
  $.fn.UifDataTable = function (method) {
    var config;
    var self = $(this);
  
    var defaults = {
      selectionType: 'single',
      source: null,
      hiddenColumns: [],
      propertyNames: [],
      searchableColumns: [],
      widthColumns: [],
      edit: false,
      delete: false,
      add: false,
      filter: true,
      paginate: true,
      pageSize: 10,
      title: '',
      exporttocsv: false,
      exporttoprint: false,
      exporttoexcel: null,
      exportpath: '/Export',
      exportname: 'archivo',
      order: true,
      serverSide: false,
      sourceData: null,
      commandColumnWidth:'150px',
      errorSourceCallBack: null,
      alignActions: '',
      exportSelected: false,
      checkColumnName: '_isChecked',
      showCheckAll: true
    };
  
    config = $.extend({}, defaults, method);
  
    var methods = {
      init: function () {
        return this.each(function () {
          var columnsDefinitions = {
            columnDefs: [],
            propertyNames: [],
            hidden: [],
            export: [],
            date: [],
            boolean: [],
            currency: [],
            noOrder: [],
          };
          self.removeClass('uif-table');
          self.addClass('uif-table-pc');
          var oTable = self.dataTable();
          oTable.fnDestroy();
          var tableId = self.attr('id');
          var tableSelector = '#' + tableId;
          var tableWrapper = tableSelector + '_wrapper';
          
          helpers.mapDataAttributes(self, config);
          helpers.createFilterSection(self, config);
          helpers.createCommandColumn(self, config, columnsDefinitions);
          helpers.createTableColumns(self, columnsDefinitions,config);
          helpers.createTableHeader(self, config, columnsDefinitions);
          helpers.createTableProcessing(self,tableId);
          helpers.prepareColumnMetadataForPlugin(config, columnsDefinitions);
          var exportClientSide=helpers.exportClientSide(config, self);
          var initComplete = function () {
            helpers.updateStyles(oTable, config);
            helpers.loadedCheckData(oTable);
            if((config.exporttocsv || config.exporttoexcel || config.exporttoprint) && !config.serverSide){
              var button=oTable.api().buttons().container().find('.btn-export').removeClass().addClass('btn btn-default btn-export');
              $(this).closest('.panel-table').find('.panel-actions').prepend(button);
            }
            oTable.off('click', '.select-button');
            oTable.on('click', '.select-button', function() {
              helpers.attachSelectEvent.call($(this), self, oTable, config);
            });
            oTable.off('click', '.selectall-button');
            oTable.on('click', '.selectall-button', function(){
              helpers.attachSelectAllEvent.call($(this), self);
            });
            oTable.off('click', '.edit-button');
            oTable.on('click', '.edit-button', function(){
              helpers.attachEditEvent.call($(this), self, oTable, config);
            });
            oTable.off('click', '.delete-button');
            oTable.on('click', '.delete-button', function(){
              helpers.attachDeleteEvent.call($(this), self, oTable, config);
            });
            self.trigger('uift_inited');
          };
  
          if (config.serverSide === true)
            config.order = false;
          
          var ajax;
          if(config.source){
            ajax = {
              'url': config.source,
              'type':'POST',
              data : function(data) {
                data.columns = JSON.stringify(data.columns);
                return data;
              },
              error: function(jqXHR, exception) {
                if (config.errorSourceCallBack != null && typeof(config.errorSourceCallBack) === 'function' ) {
                  config.errorSourceCallBack(jqXHR, exception);
                }
              }
            };
          }
          else{
            config.serverSide=false;
          }
          self.data('config', config);
          if (config.source || config.sourceData) { 
            self.dataTable({
              'bProcessing': false,
              //'sAjaxSource': config.source,
              'ajax':ajax,
              'language': $.fn.UifDataTable.language,
              'aoColumnDefs': columnsDefinitions.columnDefs,
              'fnInitComplete': initComplete,
              'bDestroy': true,
              'bFilter': config.filter,
              'autoWidth': true,
              'aoColumns': columnsDefinitions.propertyNames,
              'bPaginate': config.paginate,
              'ordering': config.order,
              'bLengthChange': config.paginate,
              'fnDrawCallback': function(){
                self.trigger('uift_drawed');
              },
              'preDrawCallback': function() {
                self.trigger('uift_preDrawed');
              },
              'dom':exportClientSide.dom,
              'buttons':exportClientSide.buttons,
              'iDisplayLength': config.pageSize,
              'serverSide': config.serverSide,
              'pagingType': 'simple_numbers',
              'data': config.sourceData,
            });
          } else {
            self.dataTable({
              'language': $.fn.UifDataTable.language,
              'fnInitComplete': initComplete,
              'bDestroy': true,
              'bFilter': config.filter,
              'autoWidth': true,
              'bPaginate': config.paginate,
              'ordering': config.order,
              'bLengthChange': config.paginate,
              'dom':exportClientSide.dom,
              'buttons':exportClientSide.buttons,
              'iDisplayLength': config.pageSize,
              'serverSide': config.serverSide,
              'pagingType': 'simple_numbers',
            });
          }
          
          helpers.filterTable(tableSelector,config);
          helpers.makeTableFullWidth(self);
          
          helpers.createPaggingSection(tableWrapper, config);
          helpers.createHorizontalScroll(self);
        });
      },
  
      addRow: function (rowdata) {
        var oTable = $(this).dataTable();
        oTable.fnAddData(rowdata);
      }, 
  
      editRow: function (rowdata, rowIndex) {
        var oTable = $(this).dataTable();
        oTable.fnUpdate(rowdata, rowIndex);
      },
  
      deleteRow: function (rowIndex) {
        var oTable = $(this).dataTable();
        oTable.fnDeleteRow(rowIndex);
      },
  
      getSelected: function () {
        var oTable = $(this).dataTable();
        if (oTable.$('.row-selected').length >= 1) {
          var selectedRows = [];
          for (var i = 0; i < oTable.$('.row-selected').length; ++i) {
            selectedRows.push(oTable.fnGetData(oTable.$('.row-selected')[i]));
          }
          return selectedRows;
        }
        else {
          return null;
        }
      },
  
      next: function () {
        if ($(this).data('selectiontype')) {
          config.selectionType = $(this).data('selectiontype');
        }
        if (config.selectionType === 'single') {
          var oTable = $(this).dataTable();
          var row = $(this).find('.row-selected');
          var next = row.next();
          if (next.length > 0) {
            row.removeClass('row-selected');
            //next.addClass('row-selected');
            var aData = oTable.fnGetData(next[0]);
            oTable.trigger('rowSelected', [aData]);
            next.find('button')[0].click();
          }
        }
        else {
          $.error('El método "' + method + '" sólo es soportado con selectionType single!');
        }
      },
  
      previous: function () {
        if ($(this).data('selectiontype')) {
          config.selectionType = $(this).data('selectiontype');
        }
        if (config.selectionType === 'single') {
          var oTable = $(this).dataTable();
          var row = $(this).find('.row-selected');
          var prev = row.prev();
  
          if (prev.length > 0) {
            row.removeClass('row-selected');
            //prev.addClass('row-selected');
            var aData = oTable.fnGetData(prev[0]);
            oTable.trigger('rowSelected', [aData]);
            prev.find('button')[0].click();
          }
        }
        else {
          $.error('El método "' + method + '" sólo es soportado con selectionType single!');
  
        }
      },
      getData: function () {
        var dataArray = $(this).DataTable().rows().data();
        var arraytoReturn = [];
        dataArray.each(function (value) {
          arraytoReturn.push(value);
        });
        return arraytoReturn;
      },
      clear: function () {
        var oTable = $(this).dataTable();
        oTable.fnClearTable();
      },
      unselect: function () {
        var oTable = $(this).dataTable();
        oTable.$('tr.row-selected').removeClass('row-selected');
        oTable.$('tr').find('span.glyphicon-check').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
      },
      redraw: function () {
        var oTable = $(this).DataTable();
        //oTable.fnAdjustColumnSizing();
        oTable.columns.adjust().draw(); 
      },
      setSelect: function (value){
        //{ label:'Id', values :[1,4,5] }
        var oTable = $(this).dataTable();
        var configu = $(this).data('config');
        if (configu.selectionType === 'single') {
          oTable.$('tr.row-selected').removeClass('row-selected');
          oTable.$('tr').find('span.glyphicon-check').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
          $.each(oTable.$('tr'),function(){
            var row = this;
            var dataRow = oTable.fnGetData(oTable.$(row));
            if(dataRow[value.label] == value.value){
              $(row).addClass('row-selected');
              $(row).find('span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
            }
          });
        }
        else {
          $.each(oTable.$('tr'),function(){
            var row = this;
            var dataRow = oTable.fnGetData(oTable.$(row));
            value.values.map(function(data){
              if(dataRow[value.label] == data){
                $(row).addClass('row-selected');
                $(row).find('span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
              }
            });
          });
        }
      },
      setUnselect: function (value){
        //{ label:'Id', values :[1,4,5] }
        var oTable = $(this).dataTable();
        $.each(oTable.$('tr'),function(){
          var row=this;
          var dataRow =oTable.fnGetData(oTable.$(row));
          value.values.map(function(data){
            if(dataRow[value.label]==data){
              $(row).removeClass('row-selected');
              $(row).find('span').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
            }
          });
        });
      },
      setLoading: function (value) {
        var tableId = $(this).attr('id');
        var tableWrapper = '#'+tableId + '_wrapper';
        var processing=$('#'+tableId+'_processing');
        if(value){
          processing.css('display','block');
          processing.css('height',$(tableWrapper).height()+'px');
        }
        else{
          processing.css('display','none');
        }
      },

      getRowIndex: function (data) {
      //{ label:'Id', values :[1,4,5] }
        var oTable = $(this).dataTable();
        var result=[];
        $.each(oTable.$('tr'),function(){
          var row=this;
          var dataRow = oTable.fnGetData(oTable.$(row)[0]);
          var position = oTable.fnGetPosition(oTable.$(row)[0]);
          data.values.map(function(val){
            if(dataRow[data.label]==val){
              var addItem = { index: position };
              addItem.data = dataRow;
              result.push(addItem);
            }
          });
        });
        return result;
      },
      setRowColor: function (index, color) {
        var oTable = $(this).DataTable();
        $(oTable.row(index).node()).css('background',color);
      },
      setCellColor: function (data, color) {
        //[{row: 1, cell: 3}]
        var oTable = $(this).DataTable();
        var config = $(this).data('config');
        var validateColumnAction = (config.selectionType === 'single' || config.selectionType === 'multiple' || config.edit || config.delete);
        data.map(function (value) {
          var row = $(oTable.row(value.row).node());
          if (config.alignActions === 'left' && validateColumnAction) {
            $($(row).find('td')[value.cell + 1]).css('background',color);
          } else {
            $($(row).find('td')[value.cell]).css('background',color);
          }
        });
      },
      resetDefaultColor: function () {
        var oTable = $(this).dataTable();
        $(oTable.$('tr')).css('background','');
        $(oTable.$('tr').find('td')).css('background','');
      },
      order: function (data) {
        var table = $(this).DataTable();
        table
            .order(data)
            .draw();
      }
    };
  
    var helpers = {
      updateStyles : function(dataTable, config) {
        dataTable.find('.th-actions').css('cssText', 'width: '+ config.commandColumnWidth +' !important;');
      },
      loadedCheckData: function (dataTable) {
        dataTable.$('tr').find('.glyphicon-check').closest('tr').addClass('row-selected');
      },
      mapDataAttributes: function (dataTable, config) {
        
        if (config.source === null) {
          if (dataTable.data('source')) {
            config.source = dataTable.data('source');
          }
        }
        if (dataTable.data('selectiontype')) {
          config.selectionType = dataTable.data('selectiontype');
        }
        if (dataTable.data('edit')) {
          config.edit = dataTable.data('edit');
        }
        if (dataTable.data('delete')) {
          config.delete = dataTable.data('delete');
        }
        if (dataTable.data('add')) {
          config.add = dataTable.data('add');
        }
        if (dataTable.data('exporttocsv')) {
          config.exporttocsv = dataTable.data('exporttocsv');
        }
        if (dataTable.data('exportname')) {
          config.exportname = dataTable.data('exportname');
        }
        if (dataTable.data('filter') !== null) {
          config.filter = dataTable.data('filter');
        }
        if (dataTable.data('paginate') != null) {
          config.paginate = dataTable.data('paginate');
        }
        if (dataTable.data('pagesize') && dataTable.data('pagesize') !== null) {
          config.pageSize = dataTable.data('pagesize');
        }
        if (dataTable.data('title') != null) {
          config.title = dataTable.data('title');
        }
        if (dataTable.data('sort') === false) {
          config.order = dataTable.data('sort');
        }
        if (dataTable.data('serverside') !== null) {
          config.serverSide = dataTable.data('serverside');
        }
        if (dataTable.data('exportpath') != null) {
          config.exportpath = dataTable.data('exportpath');
        }
        if (dataTable.data('commandcolumnwidth') != null) {
          config.commandColumnWidth = dataTable.data('commandcolumnwidth');
        }
        if (dataTable.data('exporttoprint')) {
          config.exporttoprint = dataTable.data('exporttoprint');
        }
        if (dataTable.data('exporttoexcel') === true) {
          config.exporttoexcel = dataTable.data('exporttoexcel');
        } else if (dataTable.data('exporttoexcel') === false) {
          config.exporttoexcel = dataTable.data('exporttoexcel');
        }

        if (dataTable.data('alignactions')) {
          config.alignActions = dataTable.data('alignactions');
        }
        if (!config.alignActions) {
          config.alignActions = $.fn.UifDataTable.alignActions;
        }
        if (config.exporttoexcel === null) {
          config.exporttoexcel = $.fn.UifDataTable.exporttoexcel;
        }
        if (dataTable.data('exportselected')) {
          config.exportSelected = dataTable.data('exportselected');
        }
        if (dataTable.data('checkcolumnname')) {
          config.checkColumnName = dataTable.data('checkcolumnname');
        }
        if (dataTable.data('showselectall') === false) {
          config.showCheckAll = dataTable.data('showselectall');
        }
      },
      createCommandColumn: function (dataTable, config, columnsDefinitions) {
          //Select, Edit, Delete buttons
        var classDesign =  'div-button-single';
        if (config.selectionType === 'multiple') {
          classDesign =  'div-button-multiple';
        }
        
        var htmlButtons = '<div class="'+ classDesign +'">';
          
        if (config.selectionType === 'single' || config.selectionType === 'multiple') {
          htmlButtons += ' <button class="btn btn-default btn-xs select-button btn-table" onclick="return false"><span class="glyphicon glyphicon-unchecked"></span></button>';
        }
        if (config.edit) {
          htmlButtons += ' <button class="btn btn-default btn-xs edit-button btn-table" onclick="return false"><span class="fa fa-pencil"></span></button>';
        }
        if (config.delete) {
          htmlButtons += ' <button class="btn btn-default btn-xs delete-button btn-table" onclick="return false"><span class="fa fa-trash"></span></button>';
        }
  
        htmlButtons += '</div>';
        if (config.selectionType === 'single' || config.selectionType === 'multiple' || config.edit || config.delete) {
          var position  = config.alignActions === 'left' ? 0 : -1;
          dataTable.find('thead tr th.actions').remove();
          var titleColumn = '';
          if (config.selectionType === 'multiple' && config.showCheckAll) {
            titleColumn = '<div class="'+ classDesign +'">' + '<button class="btn btn-default btn-xs selectall-button btn-table" data-checked="false" onclick="return false"><span class="glyphicon glyphicon-unchecked"></span></button></div>' ;
          }
          if (position === 0) {
            if(dataTable.find('tfoot').length > 0) {
              dataTable.find(' thead tr').prepend('<th class="actions th-actions"></th>');
              dataTable.find(' tfoot tr').prepend('<th style="text-align:center"></th>');
            } else {
              dataTable.find(' thead tr').prepend('<th class="actions th-actions"></th>');
            }
          } else {
            if(dataTable.find('tfoot').length > 0) {
              dataTable.find(' thead tr').append('<th class="actions th-actions"></th>');
              dataTable.find(' tfoot tr').append('<th style="text-align:center"></th>');
            } else {
              dataTable.find(' thead tr').append('<th class="actions th-actions"></th>');
            }
          }
          
          var countChecks = 0;
          columnsDefinitions.columnDefs.push({
            'aTargets': [position],
            'mRender': function (data, type, row) {
              if (countChecks === 1 && config.selectionType === 'single')
                return htmlButtons;
              countChecks++;
              if (row[config.checkColumnName]) {
                return htmlButtons.replace('unchecked','check');
              }
              return htmlButtons;
            },
            'mData': null,
            'bSortable': false,
            'sTitle': titleColumn,
            'sWidth': '1px'//config.commandColumnWidth
          });
        }
      },
      exportClientSide:function(config, dataTable){
        var result={
          dom:'t<\'row tfooter\'<\'uif-col col-md-4 col-sm-4 col-xs-4\'fi><\'uif-col col-md-2 col-sm-2 col-xs-2\'l><\'uif-col col-md-6 col-sm-6 col-xs-6\'p>>',
          buttons:[]
        };

        var rows = null;

        if (config.exportSelected) {
          rows = function (idx, data, node) {
            if($(dataTable).find('.row-selected').length > 0) {
              return $(node).hasClass('row-selected');
            }
            return true;
          };
        } 

        if (!config.serverSide) {
          if (config.exporttocsv || config.exporttoexcel || config.exporttoprint) {
            result.dom='B '+result.dom;
          }
          if(config.exporttocsv){
            result.buttons.push({
              'extend':'csv',
              'text':'<span class="glyphicon glyphicon-export"></span> CSV',
              'className': 'btn btn-default btn-export',
              'filename':config.exportname,
              'title':null,
              'messageTop':'',
              'sheetName':config.exportname,
              'exportOptions': {
                rows: rows,
              }
            });
          }
          if (config.exporttoexcel) {
            result.buttons.push({
              'extend':'excelHtml5',
              'text':'<span class="glyphicon glyphicon-th-list"></span> Exportar',
              'className': 'btn btn-default btn-export',
              'filename':config.exportname,
              'title':null,
              'messageTop':'',
              'sheetName':config.exportname,
              'exportOptions': {
                rows: rows,
              }
            });
          }
          if (config.exporttoprint) {
            result.buttons.push({
              'extend':'print',
              'text':'<span class="glyphicon glyphicon-print"></span> PRINT',
              'className': 'btn btn-default btn-export',
              'filename':config.exportname,
              'title':config.title,
              'messageTop':'',
              'sheetName':config.exportname,
              'exportOptions': {
                rows: rows,
              }
            });
          }
        }
        
        return result;
      },
      createTableColumns: function (dataTable, columnsDefinitions, config) {
          //Table Header
        dataTable.find('thead tr th').each(function (index, value) {
            //Visible
          if ($(this).data('visible') === false) {
            columnsDefinitions.hidden.push(index);
          }
            //Bind to json Property
          if ($(this).data('property')) {
            var columnWidth='';
            if($(this).data('width')){
              columnWidth=$(this).data('width');
            }
            if(config.widthColumns.length>0){
              if (config.alignActions === 'left') {
                config.widthColumns.filter(function(row){if (row.column == (index - 1)) { columnWidth = row.width;} });
              } else {
                config.widthColumns.filter(function(row){if (row.column == index) { columnWidth = row.width;}});
              } 
            }
            if(columnWidth){
              columnsDefinitions.propertyNames.push({ 'mData': $(this).data('property'), 'sWidth':columnWidth });
            }
            else{
              columnsDefinitions.propertyNames.push({ 'mData': $(this).data('property') });
            }
            columnsDefinitions.export.push({ 'Text': value.innerText, 'Bind': $(this).data('property') });
          }
          else {
              //Export Column Only
            columnsDefinitions.export.push({ 'Text': value.innerText, 'Bind': null });
          }
            //No order Column
          if ($(this).data('noorder')) {
            columnsDefinitions.noOrder.push(index);
          }
            //Column type
          if ($(this).data('type')) {
            if ($(this).data('type') === 'date') {
              columnsDefinitions.date.push(index);
            }
            if ($(this).data('type') === 'boolean') {
              columnsDefinitions.boolean.push(index);
            }
            if ($(this).data('type') === 'currency') {
              columnsDefinitions.currency.push(index);
            }
          }
        });
      },
      exporttocsv: function (config, columnsDefinitions) {
        var json = JSON.stringify({
          Columns: columnsDefinitions.export,
          RelativePath: config.source+(config.source.includes('?')?'&export=true':'?export=true'),
          FileName: config.exportname,
        });
  
        $.ajax({
          type: 'POST',
          url: config.exportpath + '/GenerateCSV',
          data: json,
          dataType: 'json',
          contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
          window.location = config.exportpath + '/Download?fileKey=' + data;
        });
      },
      exporttoexcel: function (config, columnsDefinitions) {
        var json = JSON.stringify({
          Columns: columnsDefinitions.export,
          RelativePath: config.source+(config.source.includes('?')?'&export=true':'?export=true'),
          FileName: config.exportname,
        });
  
        $.ajax({
          type: 'POST',
          url: config.exportpath + '/GenerateExcel',
          data: json,
          dataType: 'json',
          contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
          window.location = config.exportpath + '/DownloadExcel?fileKey=' + data;
        });
      },
      createTableProcessing:function(dataTable,tableId){
        dataTable.parent().find('.processing').remove();
        var processing = $('<div id="'+tableId+'_processing" class="processing">\
            <div class="processing_content">Cargando...\
            </div>\
        </div>');
        dataTable.parent().find('.panel-heading').after(processing);
      },
      createTableHeader: function (dataTable, config, columnsDefinitions) {
        if (dataTable.closest('.panel-table').length === 0) {
          var panel = $('<div class="panel-table"/>');
          var title='';
          if (config.title.length>0)
            title='<h3 class="panel-title">' + config.title + '</h3>';
          var panelHeading = $('<div class="panel-heading panel-heading-table">\
                                            '+title+'\
                                              <div class="panel-actions">\
                                              </div>\
                                          </div>');
          dataTable.wrap(panel);
          dataTable.parent().prepend(panelHeading);
          if (config.exporttocsv && config.serverSide) {
            panelHeading.find('.panel-actions').append('<a class="btn btn-default exporttocsv-button" onclick="return false"><span class="glyphicon glyphicon-export"></span> CSV</a>');
          }
          if (config.exporttoprint && config.serverSide) {
            panelHeading.find('.panel-actions').append('<a class="btn btn-default exporttoprint-button" onclick="return false"><span class="glyphicon glyphicon-print"></span> PRINT</a>');
          }
          if (config.exporttoexcel && config.serverSide) {
            panelHeading.find('.panel-actions').append('<a class="btn btn-default exporttoexcel-button" onclick="return false"><span class="glyphicon glyphicon-th-list"></span> Excel</a>');
          }
          if (config.add) {
            panelHeading.find('.panel-actions').append('<a class="btn btn-default add-button" onclick="return false"><span class="glyphicon glyphicon-plus"></span> Agregar</a>');
            panelHeading.find('.add-button').click(function () {
              dataTable.trigger('rowAdd');
            });
          }
          if(!(config.title.length>0) && !config.add && !config.exporttocsv && !config.exporttoprint && !config.exporttoexcel){
            panelHeading.css({
              'display':'none'
            });
          }
        }
        if (config.exporttocsv && config.serverSide) {
          dataTable.closest('.panel-table').find('.exporttocsv-button').unbind('click');
          dataTable.closest('.panel-table').find('.exporttocsv-button').click(helpers.exporttocsv.bind(this, config, columnsDefinitions));
        }
        if (config.exporttoexcel && config.serverSide) {
          dataTable.closest('.panel-table').find('.exporttoexcel-button').unbind('click');
          dataTable.closest('.panel-table').find('.exporttoexcel-button').click(helpers.exporttoexcel.bind(this, config, columnsDefinitions));
        }
        if (config.exporttoprint && config.serverSide) {
          //do someting serverside
        }
      },
      filterTable:function(tableSelector,config){
        if(config.filter){
          $(tableSelector+'_filter').remove();
          var table = $(tableSelector).dataTable();
          table.api().columns().every( function () {
            var that = this;
            $(this.footer()).find('input').on( 'keyup change', function () {
              if ( that.search() !== this.value ) {
                that.search( this.value ).draw();
              }
            });
          });
        }
      },
      createFilterSection: function (datatable, config) {
        if (config.filter) {
          datatable.find('tfoot').remove();
          var foot = $('<tfoot><tr></tr></tfoot>');
          var filterAllColumns = true;
          $.each(datatable.find('thead th'),function(){
            if($(this).data('searchable')!=undefined){
              filterAllColumns = false;
            }
          });
          if(config.searchableColumns.length>0)
            filterAllColumns = false;
          $.each(datatable.find('thead th'),function(i){
            if(!$(this).hasClass('actions')){
              if(config.searchableColumns.length == 0){
                if(filterAllColumns){
                  foot.find('tr').append('<th><input type="text"/></th>');
                }else if($(this).data('searchable') === true){
                  foot.find('tr').append('<th><input type="text"/></th>');
                }else{
                  foot.find('tr').append('<th></th>');
                }
              }else{
                var findRecord = config.searchableColumns.filter(function(row){return row == i;});
                if(findRecord.length > 0){
                  foot.find('tr').append('<th><input type="text"/></th>');
                }else{
                  foot.find('tr').append('<th></th>');
                }
              }
            }
          });
          datatable.find('thead').after(foot);
        }
      },
      createPaggingSection: function (tableWrapper, config) {
        if (!config.paginate) {
          $(tableWrapper).find('.tfooter').remove();
        }
      },
      createHorizontalScroll: function (dataTable) {
        dataTable.wrap('<div class="scrolling" />');
      },
      makeTableFullWidth: function(dataTable){
        dataTable.addClass('display table table-hover');
        dataTable.css('width', '100%');
      },
      prepareColumnMetadataForPlugin: function (config, columnsDefinitions) {
          //Hiden Columns
        var validateColumnAction = (config.selectionType === 'single' || config.selectionType === 'multiple' || config.edit || config.delete);
        if (config.alignActions === 'left' && validateColumnAction) {
          config.hiddenColumns.map(function(item, index) {
            config.hiddenColumns[index] = item + 1;
          });
        }
        $.extend(columnsDefinitions.hidden, config.hiddenColumns);
        if (columnsDefinitions.hidden.length > 0) {
          columnsDefinitions.columnDefs.push({
            'aTargets': columnsDefinitions.hidden,
            'bVisible': false,
          });
        }
          //PropertyNames
        var propertyNamesConfig = [];
        var i;
        for (i = 0; i < config.propertyNames.length; ++i) {
          propertyNamesConfig.push({ 'mData': config.propertyNames[i] });
        }
        if (config.selectionType === 'single' || config.selectionType === 'multiple' || config.edit || config.delete) {
          if (config.alignActions === 'left') {
            columnsDefinitions.propertyNames.unshift({ 'mData': null });
            propertyNamesConfig.unshift({ 'mData': null });
          } else {
            columnsDefinitions.propertyNames.push({ 'mData': null });
            propertyNamesConfig.push({ 'mData': null });
          }
        }
        $.extend({},columnsDefinitions.propertyNames, propertyNamesConfig);
          //Date Columns
        if (columnsDefinitions.date.length > 0) {
          columnsDefinitions.columnDefs.push({
            'sType': 'date-uk',
            'aTargets': columnsDefinitions.date,
            'mRender': function (data) {
              if(!data)
                return data;
              var date = moment(data);
              return date.format('DD/MM/YYYY');
            },
          });
        }
          //Boolean Columns
        if (columnsDefinitions.boolean.length > 0) {
          columnsDefinitions.columnDefs.push({
            'aTargets': columnsDefinitions.boolean,
            'mRender': function (data) {
              if (data === true) {
                return '<span class="glyphicon glyphicon-ok"></span>';
              }
              return '';
            },
          });
        }
          //Currency Columns
        if (columnsDefinitions.currency.length > 0) {
          columnsDefinitions.columnDefs.push({
            'aTargets': columnsDefinitions.currency,
            'sClass': 'justified-right',
            'mRender': function (data) {
              return $.number(data, $.uif2.defaults.decimalPlaces, $.uif2.defaults.decimalSeparator, $.uif2.defaults.thousandsSeparator);
            },
          });
        }
          //NoOrder Columns
        if (columnsDefinitions.noOrder.length > 0) {
          columnsDefinitions.columnDefs.push({
            'aTargets': columnsDefinitions.noOrder,
            'bSortable': false,
          });
        }
      },
      attachSelectEvent: function (dataTable, dataTablePlugin, config) {
        var aData;
        var position;
        if (config.selectionType === 'single') {
          if($(this).closest('tr').hasClass('row-selected')){
            $(this).closest('tr').removeClass('row-selected');
            $(this).closest('tr').find('span.glyphicon-check').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
            aData = dataTablePlugin.fnGetData($(this).closest('tr')[0]);
            position = dataTablePlugin.fnGetPosition($(this).closest('tr')[0]);
            dataTable.trigger('rowDeselected', [aData, position]);
          }else{
            dataTable.$('tr').removeClass('row-selected');
            dataTable.$('tr').find('span.glyphicon-check').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
            $(this).closest('tr').addClass('row-selected');
            $(this).find('span.glyphicon-unchecked').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
            aData = dataTablePlugin.fnGetData($(this).closest('tr')[0]);
            position = dataTablePlugin.fnGetPosition($(this).closest('tr')[0]);
            dataTable.trigger('rowSelected', [aData, position]);
          }
        }
        if (config.selectionType === 'multiple') {
          if ($(this).closest('tr').hasClass('row-selected')) {
            $(this).closest('tr').removeClass('row-selected');
            $(this).closest('tr').find('span.glyphicon-check').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
            aData = dataTablePlugin.fnGetData($(this).closest('tr')[0]);
            position = dataTablePlugin.fnGetPosition($(this).closest('tr')[0]);
            dataTable.trigger('rowDeselected', [aData, position]);
          }
          else {
            $(this).closest('tr').addClass('row-selected');
            $(this).closest('tr').find('span.glyphicon-unchecked').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
            aData = dataTablePlugin.fnGetData($(this).closest('tr')[0]);
            position = dataTablePlugin.fnGetPosition($(this).closest('tr')[0]);
            dataTable.trigger('rowSelected', [aData, position]);
          }
        }
      },
      attachSelectAllEvent: function (dataTable) {
        if ($(this).data('checked') === false) {
          $(this).find('span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
          dataTable.$('tr', {'filter':'applied'}).addClass('row-selected');
          dataTable.$('tr', {'filter':'applied'}).find('span.glyphicon-unchecked').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
          $(this).data('checked', true);
          dataTable.trigger('selectAll');
        }
        else {
          $(this).find('span').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
          dataTable.$('tr', {'filter':'applied'}).removeClass('row-selected');
          $(this).data('checked', false);
          dataTable.$('tr', {'filter':'applied'}).find('span.glyphicon-check').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
          dataTable.trigger('desSelectAll');
        }
      },
      attachEditEvent: function (dataTable, dataTablePlugin) {
        /*if (config.selectionType === 'single') {
          if(!$(this).closest('tr').hasClass('row-selected')){
            dataTable.$('tr').removeClass('row-selected');
            $(this).closest('tr').addClass('row-selected');
          }
        }*/
        var row = $(this).closest('tr')[0];
        var aData = dataTablePlugin.fnGetData(row);
        var position = dataTablePlugin.fnGetPosition(row);
        dataTable.trigger('rowEdit', [aData, position]);
      },
      attachDeleteEvent: function (dataTable, dataTablePlugin, config) {
        if (config.selectionType === 'single') {
          dataTablePlugin.$('tr').removeClass('row-selected');
          $(this).closest('tr').addClass('row-selected');
        }
        var row = $(this).closest('tr')[0];
        var aData = dataTablePlugin.fnGetData(row);
        var position = dataTablePlugin.fnGetPosition(row);
        dataTable.trigger('rowDelete', [aData, position]);
      },
    };
    if (methods[method]) {
      return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
    } else if (typeof method === 'object' || !method) {
      return methods.init.apply(this, arguments);
    } else {
      $.error('El método "' + method + '" no existe dentro de DataTable Plugin!');
    }
  
  };
})(jQuery);
  
(function ($) {
  $.fn.UifDataTable.alignActions = 'right';
  $.fn.UifDataTable.exporttoexcel = false;
  $.fn.UifDataTable.language = {
    'sCheckAll': 'Seleccionar Todo',
    'sProcessing':     'Procesando...',
    'sLengthMenu':     '_MENU_',
    'sZeroRecords':    'No se encontraron resultados',
    'sEmptyTable':     'Ningún dato disponible en esta tabla',
    'sInfo':           'Mostrando del _START_ al _END_ de _TOTAL_',
    'sInfoEmpty':      'Mostrando del 0 al 0 de 0',
    'sInfoFiltered':   '(filtrado de un total de _MAX_ registros)',
    'sInfoPostFix':    '',
    'sSearch':         'Buscar:',
    'sUrl':            '',
    'sInfoThousands':  ',',
    'sLoadingRecords': 'Cargando...',
    'oPaginate': {
      'sFirst':    'Primero',
      'sLast':     'Último',
      'sNext':     'SIG >>',
      'sPrevious': '<< ANT'
    },
    'oAria': {
      'sSortAscending':  ': Activar para ordenar la columna de manera ascendente',
      'sSortDescending': ': Activar para ordenar la columna de manera descendente'
    }
  };
  
}(jQuery));

$(document).ready(function () {
  $('.uif-table').each(function () {
    $(this).UifDataTable();
  });
});
  