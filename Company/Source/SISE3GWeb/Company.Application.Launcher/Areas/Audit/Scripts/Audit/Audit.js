var container = document.getElementById("ValueBefore");
var containerAfter = document.getElementById("ValueAfter");
var options = {
    "mode": "form",
    "indentation": 2,
    "name": AppResources.Rule,
    "sortObjectKeys": "true",
    onClassName: ClassName,
    onEvent: onEvent
};
function findNodeInJson(json, path){
    if(!json || path.length ===0) {
        return { field: undefined, value: undefined };
    }    
    var first = path[0];
    var remainingPath = path.slice(1);

    if(remainingPath.length === 0) {
      return {field: (typeof json[first] !== 'undefined' ? first : undefined), value: json[first]};
    } else {
        return findNodeInJson(json[first], remainingPath);
    }
}
function onEvent(node, event) {
    if(event.type=="click")
    {
        if(node.value!=undefined)
        {
            console.log(node.value)
           ValidateNode(node)
        }
    }
}
function ValidateNode(node,text) {
     
    var oppositeNode = findNodeInJson(editorAfter.get(), node.value);
    var isValueEqual = JSON.stringify(node.value) === JSON.stringify(oppositeNode.value);

    if (Array.isArray(node.value) && Array.isArray(oppositeNode.value)) {
        isValueEqual = text.every(function (e) {
            return oppositeNode.value.includes(e);
        });
    }

    if (node.field === oppositeNode.field && isValueEqual) {
        return 'the_same_element';
    } else {
        return 'different_element';
    }
}

function ClassName({ path, field, value }) {
    if (path.length > 0) {
        var thisNode = findNodeInJson(editor.get(), path);
        var oppositeNode = findNodeInJson(editorAfter.get(), path);
       // var thisNode = findNodeInJson(JSON.stringify(editor.get(), null, 2), path);
        //var oppositeNode = findNodeInJson(JSON.stringify(editorAfter.get(), null, 2), path);
        var isValueEqual = JSON.stringify(thisNode.value) === JSON.stringify(oppositeNode.value);

        if (Array.isArray(thisNode.value) && Array.isArray(oppositeNode.value)) {
            isValueEqual = thisNode.value.every(function (e) {
                return oppositeNode.value.includes(e);
            });
        }

        if (thisNode.field === oppositeNode.field && isValueEqual) {
            return 'the_same_element';
        } else {
            return 'different_element';
        }
    }  
  }
var editor = new JSONEditor(container, options);
var editorAfter = new JSONEditor(containerAfter, options);
var table = $('#tableAudit').dataTable();
$(() => {
    new Audit();
});
var idPackage=0;
class Audit extends Uif2.Page {
    getInitialState() {
        AuditRequest.GetTypeTransaction().done(function (data) {
            if (data.success) {
                $('#selectTypeTransaction').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        AuditRequest.GetPackages().done(function (data) {
            if (data.success) {
                $('#selectSchema').UifSelect({ sourceData: data.result, selectedId: data.result[0].Id });
                idPackage = $('#selectSchema').val();
                Audit.LoadinputObjectAudit();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputFrom").UifDatepicker("setValue", new Date());
        $("#inputTo").UifDatepicker('setValue', new Date());
      
    }

    bindEvents() {
        $("#inputFrom").on('datepicker.change', this.ChangeFrom);
        $("#inputTo").on('datepicker.change', this.ChangeTo);
        $("#btnSearchAudit").click(this.GetAudit);
        $("#btnExport").click(this.exportExcel);
        $("#btnExit").click(this.redirectIndex);
        $("#inputUserAudit").on("itemSelected", this.SetUser);
        $('#selectSchema').on('itemSelected', Audit.LoadinputObjectAudit);
        $("#inputObjectAudit").on("itemSelected", this.SetEntity);


    }
    exportExcel() {
        $('#formAudit').validate();
        if ($('#formAudit').valid()) {
            $("#btnSearchAudit").prop('disabled', true);
            var AuditModelView = Audit.CreateAuditModel();
            if (AuditModelView != null) {
                AuditRequest.GenerateFileToExport(AuditModelView).done(function (data) {
                    if (data.success) {
                        DownloadFile(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            $("#btnSearchAudit").prop('disabled', false);
        }
    }
    GetAudit() {
        $('#formAudit').validate();
        if ($('#formAudit').valid()) {
            $('#tableAudit').UifDataTable('clear');
            var AuditModelView = Audit.CreateAuditModel();
            if (AuditModelView != undefined) {
                AuditRequest.GetAudit(AuditModelView).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            $.each(data.result, function (entryIndex, entry) {
                                entry.Detail = '<a onclick="Audit.LoadAuditDetail(this)">Detalles</a>';
                            });
                            $('#tableAudit').UifDataTable({ sourceData: data.result, order: false });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }
    static CreateAuditModel() {
        var AuditModelView = $('#formAudit').serializeObject();
            AuditModelView.SchemaId = $('#selectSchema').val();
        if ($("#inputUserAudit").val() != "") {
            if ($("#inputUserAudit").data("User") != null) {
                AuditModelView.UserId = $("#inputUserAudit").data("User").UserId;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorNotSelectedUser, 'autoclose': true });
                return;
            }

        }
        if ($("#inputObjectAudit").val() != "") {
            if ($("#inputObjectAudit").data("entity") != null) {
                AuditModelView.ObjectId = $("#inputObjectAudit").data("entity").Id;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorNotSelectedEntity, 'autoclose': true });
                return;
            }
        }
        AuditModelView.CurrentFrom = new Date($("#inputFrom").val().toString().replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$3/$2/$1"))
        AuditModelView.CurrentTo = new Date($("#inputTo").val().toString().replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$3/$2/$1"))
        return AuditModelView;
    }
    SetUser(event, selectedItem) {
        $(this).data("User", selectedItem);
    }
    SetEntity(event, selectedItem) {
        $(this).data("entity", selectedItem);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }
    static LoadAuditDetail(data) {

        var result = table.fnGetData(data.parentElement.parentElement).Changes;
        if (data.parentElement.parentElement != null && table.fnGetData(data.parentElement.parentElement).ColumName.trim() != "") {
            if (result["0"].ValueBefore != null & result["0"].ValueBefore != "") {
                editor.set(jQuery.parseJSON(result["0"].ValueBefore));
            }
            else {
                editor.set("");
            }
            if (result["0"].ValueAfter != null & result["0"].ValueAfter != "") {
                editorAfter.set(jQuery.parseJSON(result["0"].ValueAfter));
            }
            else {
                editorAfter.set("");
            }
            editor.collapseAll();
            editorAfter.collapseAll();
            $('#fieldsAuditDetail').hide()
            $('#AuditDetail').show()
        }
        else {
            $('#AuditDetail').hide()
            $('#fieldsAuditDetail').show()
			$('#tableAuditDetail').UifDataTable({ sourceData: result, order: false  });
        }
	var pk = table.fnGetData(data.parentElement.parentElement).Pk;
        var columnDescriptionItem = table.fnGetData(data.parentElement.parentElement).ColumnDescription;
        if (columnDescriptionItem == null) {
            columnDescriptionItem = "";
        }
        if (!jQuery.isEmptyObject(pk)) {
            $.each(pk, function (key, value) {
                pk = columnDescriptionItem + " (" + value.Key+ ":" + value.Value + ")";
            });
        }
        else {
            pk = columnDescriptionItem + "";
        }
        $("#modalAuditDetail").UifModal("showLocal", table.fnGetData(data.parentElement.parentElement).ObjectName + ": " + pk);
        $("#modalAuditDetail").find(".modal-title").text(table.fnGetData(data.parentElement.parentElement).ObjectName + ": " + pk);
    }

     static LoadinputObjectAudit() {
        idPackage = $('#selectSchema').val();
        $("#inputObjectAudit").UifAutoComplete({
            source: rootPath + "Audit/Audit/GetEntityByQuery",
            displayKey: "Description",
            queryParameter: "idPackage=" + idPackage + "&query"

        });
    }
}    