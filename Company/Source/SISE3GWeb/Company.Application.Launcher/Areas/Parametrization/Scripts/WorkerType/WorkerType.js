var workerIndex = null;
var workerStatus = null;
var workerCode = null;
var glbWorkertypesCreate = [];
var dropDownSearchAdvWorkerType = null;

$(() => {
    new ParamWorkerType();
});

class ParamWorkerType extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        ParamWorkerType.GetAllWorkerType().done(function (data) {
            if (data.success) {
                ParamWorkerType.LoadListViewWorkerType(data.result);
            }
        });
       ParamWorkerType.GetAllWorkerType().done(function (data) {
            if (data.success) {
                ParamWorkerType.LoadListViewWorkerType(data.result);
            }
        });

       dropDownSearchAdvWorkerType = uif2.dropDown({
           source: rootPath + 'Parametrization/WorkerType/WorkerTypeSearch',
           element: '#inputWorkerType',
           align: 'right',
           width: 600,
           height: 500,
           loadedCallback: function () { }
       });
    }
    bindEvents() {
        $('#btnAddWorkerType').click(this.AddWorkerType);
        $('#listViewWorkerType').on('rowEdit', ParamWorkerType.ShowData);
        $('#btnSaveWorkerType').click(this.SaveWorkerTypes);
        $('#inputWorkerType').on('buttonClick', this.SearchWorkerType);
        $('#btnNewWorkerType').click(ParamWorkerType.Clear);
        $('#btnExitWorkerType').click(this.Exit);
        $('#btnExportWorkerType').click(this.ExportFile);

        
    }

    
    AddWorkerType() {
        
        $("#formWorkerType").validate();
        if ($("#formWorkerType").valid()) {
            var worker = ParamWorkerType.GetForm();
            if (worker.IsEnabled) {
                worker.LabelWorkerTypeEnabled = Resources.Language.Enabled;
            }
            else {
                worker.LabelWorkerTypeEnabled = Resources.Language.Disabled;
            }
            if (workerIndex == null) {
                var lista = $("#listViewWorkerType").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == worker.Description.toUpperCase();
                });
                if (ifExist.length > 0 && workerStatus != ParametrizationStatus.Create) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistInfringementWorker, 'autoclose': true });
                }
                else {
                   ParamWorkerType.SetStatus(worker, ParametrizationStatus.Create);
                    $("#listViewWorkerType").UifListView("addItem", worker);
                }
            }
            else {
                if (workerIndex != undefined && workerStatus != undefined) {
                    ParamWorkerType.SetStatus(worker, workerStatus);
                } else {
                    ParamWorkerType.SetStatus(worker, ParametrizationStatus.Update);

                }
                $('#listViewWorkerType').UifListView('editItem', workerIndex, worker);
            }
            ParamWorkerType.Clear();
        }
    }


    SaveWorkerTypes() {
        ParamWorkerType.SetListToSend();
        var datos = { lstWorkerTypes: glbWorkertypesCreate };
        $.ajax({
            type: "POST",
            url: 'SaveWorkerTypes',
            data: JSON.stringify(datos),

            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            glbWorkertypesCreate = [];
            $.UifNotify('show', {
                'type': 'info', 'message': Resources.Language.MessageUpdate + ': <br>' +
                Resources.Language.Aggregates + ': ' + data.result.TotalAdded + '<br> ' +
                Resources.Language.Updated + ': ' + data.result.TotalModified + '<br> ' +
                Resources.Language.Removed + ': ' + data.result.TotalDeleted + '<br> ' +
                Resources.Language.Errors + ': ' + data.result.Message,
                'autoclose': true
            });
            ParamWorkerType.GetAllWorkerType().done(function (data) {
                if (data.success) {
                    ParamWorkerType.LoadListViewWorkerType(data.result);
                }
            });
        });
    }

   
    static LoadListViewWorkerType(data) {
        $("#listViewWorkerType").UifListView({ displayTemplate: "#WorkerTypeTemplate", edit: true, delete: false, customAdd: true, customEdit: true, height: 300 });
        $.each(data, function (key, value) {
            var workerType =
                {
                    Id: this.Id,
                    Description: this.Description,
                    SmallDescription: this.SmallDescription,
                    IsEnabled: this.IsEnabled
                };
            
            if (ParamWorkerType.IsEnabled) {
                ParamWorkerType.LabelWorkerTypeEnabled = Resources.Language.Enabled;
            }
            else {
                ParamWorkerType.LabelWorkerTypeEnabled = Resources.Language.Disabled;
            }
            $("#listViewWorkerType").UifListView("addItem", workerType);
        })
    }

    

    static GetAllWorkerType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/WorkerType/GetWorkerType',
            async: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }


    static SetListToSend() {
         var workerTypeData = $('#listViewWorkerType').UifListView('getData');
         $.each(workerTypeData, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.StatusTypeService != undefined) {
                glbWorkertypesCreate.push(value);
            }
        });
    }

     static Clear() {
         $("#chkIsEnabled").prop("checked", "");
         $("#inputWorkerTypeDescription").val("");
         $("#inputWorkerTypeDescription").focus();
         $("#inputWorkerTypeSmallDescription").val("");
         ClearValidation("#formWorkerType"); 
         workerIndex = null;
         workerStatus = null;
         workerCode = null;
     }

     static GetForm() {
         var data = {
         };
         $("#formWorkerType").serializeArray().map(function (x) {
             data[x.name] = x.value;
         });
         data.Id = workerCode;
         data.Description = $("#inputWorkerTypeDescription").val();
         data.IsEnabled = $("#chkIsEnabled").is(":checked");
         return data;
     }

     static SetStatus(object, status) {
         object.StatusTypeService = status;
     }
        
        
     SearchWorkerType() {
         var inputWorkerType = $('#inputWorkerType').val();
         if (inputParamWorkerType.length < 3) {
             $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
         } else if (inputParamWorkerType.length > 15) {
             $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMaximumChar, autoclose: false });
         }
         else {
             $.ajax({
                 type: "POST",
                 url: 'GetWorkerTypesByDescription',
                 data: JSON.stringify({ description: inputWorkerType }),
                 dataType: "json",
                 contentType: "application/json; charset=utf-8"
             }).done(function (data) {
                 if (data.success) {
                     var workerType = data.result;
                     if (data.result.length > 1) {
                         ParamWorkerType.ShowSearchAdv(workerType);
                     }
                     else {
                         $.each(workerType, function (key, value) {
                             var lista = $("#listViewWorkerType").UifListView('getData')
                             var index = lista.findIndex(function (item) {
                                 return item.Id == value.Id;
                             });
                             ParamWorkerType.ShowData(null, value, index);
                         });
                     }
                 }
                 else {
                     $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorDescription, 'autoclose': true });
                 }
             });
         }
         $('#inputWorkerType').val('');
     }


     static ShowData(event, result, index) {
         ParamWorkerType.Clear();
         workerIndex = index;
         workerStatus = result.StatusTypeService;
         workerCode = result.Id;
         $("#inputWorkerTypeDescription").val(result.Description);
         $("#inputWorkerTypeSmallDescription").val(result.SmallDescription);
         if (result.IsEnabled) {
             $('#chkIsEnabled').prop("checked", true);
         }
     }

     static ShowSearchAdv(data) {
         $("#lvSearchAdvWorkerType").UifListView({
             displayTemplate: "#WorkerTypeTemplate",
             selectionType: "single",
             height: 300
         });
         $("#btnCancelSearchAdv").on("click", ParamWorkerType.CancelSearchAdv);
         
         $("#btnOkSearchAdv").on("click", ParamWorkerType.OkSearchAdv);
         $("#lvSearchAdvWorkerType").UifListView("clear");

         if (data) {
             data.forEach(item => {
                 var worker =
                     {
                         Id: item.Id,
                         Description: item.Description,
                         SmallDescription: item.SmallDescription
                     };
                 $("#lvSearchAdvWorkerType").UifListView("addItem", worker);
             });
         }
         dropDownSearchAdvParamWorkerType.show();
        
     }

     static CancelSearchAdv() {
         dropDownSearchAdvParamWorkerType.hide();
     }

     static OkSearchAdv() {
         let data = $("#lvSearchAdvWorkerType").UifListView("getSelected");
         if (data.length === 1) {
             var worker =
                 {
                     Id: data[0].Id,
                     Description: data[0].Description,
                     SmallDescription: data[0].SmallDescription,
                     IsEnabled: data[0].IsEnabled
                 };
             var lista = $("#listViewWorkerType").UifListView('getData');
             var index = lista.findIndex(function (item) {
                 return item.Id == worker.Id;
             });
             ParamWorkerType.ShowData(null, worker, index);
         }
         dropDownSearchAdvParamWorkerType.hide();
     }

     Exit() {
         window.location = rootPath + "Home/Index";
     }

     ExportFile() {
         ParamWorkerType.GenerateFileToExport().done(function (data) {
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
     static GenerateFileToExport() {
         return $.ajax({
             type: 'POST',
             url: 'GenerateFileToExport',
             dataType: 'json',
             contentType: 'application/json; charset=utf-8',
         });
     }
   
}