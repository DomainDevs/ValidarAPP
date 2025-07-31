var addItem = [];
var addTableItem = [];
var glbItemAdd = [];
var glbItemDelete = [];
var glbDateTablePrincipal = [];
var gblPrefixes = [];
var idEconomicGroup = 0;
var EconomicGroupInicial;
var maxDates = [];
var dateObject = {
    IndividualId: "",
    CurrentTo: ""
};
class EconomicGroup extends Uif2.Page {

    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        this.getEconomicGroup();
        $("#inputOperatingQuota").val(0);
        $("#inputDateValidity").prop('disabled', true);
        $('#tableInsuredResults').HideColums({ control: '#tableInsuredResults', colums: [1], checkColumnName: 'Habilitado' });
        EconomicGroupRequest.GetPrefixes().done(function (data) {
            gblPrefixes = data.result;
            $("#selectPrefix").UifSelect({ sourceData: data.result });
            $("#selectPrefix").prop("disabled", false);
        });
    }

    bindEvents() {
        $("#btnNewGroup").on('click', EconomicGroup.clearFields);
        $("#btnCancelGroup").on('click', this.CancelView);
        $("#btnAddPerson").on('click', this.showPersons);
        $("#SearchindividualId").on('buttonClick', this.getHolder);
        $('#tableInsuredResults tbody').on('click', 'tr', this.SelectInsuredResult);
        $("#btnSaveGroupEconomic").on('click', this.createEconomicGroup)
        $('#tableGroupEconomicResults').on('rowSelected', function (event, data, position) {
            var currentDate = moment().format("DD/MM/YYYY");
            var setSelected = {
                label: 'IndividualId',
                values: []
            };
            if ($("#rdEnabledGroup").is(":checked")) {
                var opq = parseInt(NotFormatMoney($("#inputOperatingQuota").val()));
                $.each(glbItemAdd, function (index, value) {
                    if (value.IndividualId == data.IndividualId) {
                        value.Enabled = true;
                    }
                });
                $.each(glbDateTablePrincipal, function (index, value) {
                    if (value.IndividualId == data.IndividualId) {
                        value.Enabled = true;
                        if (CompareDateEquals(value.CurrentTo, currentDate)) {
                            opq += parseInt(value.OperationQuoteAmount);
                        }
                        setSelected.values.push(data.IndividualId);
                        dateObject = {
                            IndividualId: "",
                            CurrentTo: ""
                        };
                        dateObject.IndividualId = data.IndividualId;
                        dateObject.CurrentTo = EconomicGroup.convertToDate(value.CurrentTo);
                        maxDates.push(dateObject);
                    }
                });
                $("#inputOperatingQuota").val(FormatMoney(opq, 2));
                var mxDate = EconomicGroup.getMaxDate(maxDates);
                $("#inputDateValidity").val(EconomicGroup.convertStringToDate(mxDate));
                $('#tableGroupEconomicResults').UifDataTable('setSelect', setSelected);
            } else {
                setSelected.values.push(glbDateTablePrincipal[position].IndividualId);
                $('#tableGroupEconomicResults').UifDataTable('setUnselect', setSelected);
                $.UifNotify('show', { 'type': 'info', 'message': 'Para habilitar participantes, el grupo económico debe estar habilitado', 'autoclose': true });
            }
        });
        $('#tableGroupEconomicResults').on('rowDeselected', function (event, data, position) {
            var setSelected = {
                label: 'IndividualId',
                values: []
            };
            var individualId = "";
            var actualopq = parseInt(NotFormatMoney($("#inputOperatingQuota").val()));
            var opq = parseInt(actualopq);
            $.each(glbItemAdd, function (index, value) {
                if (value.IndividualId == data.IndividualId) {
                    value.Enabled = false;
                }
            });
            $.each(glbDateTablePrincipal, function (index, value) {
                if (value.IndividualId == data.IndividualId) {
                    value.Enabled = false;
                    setSelected.values.push(data.IndividualId);
                    if (opq > 0) {
                        opq -= parseInt(value.OperationQuoteAmount);
                    }
                    individualId = value.IndividualId;
                }
            });
            for (var i = 0; i < maxDates.length; i++) {
                if (maxDates[i].IndividualId == individualId) {
                    maxDates.splice(i, 1);
                }
            }
            $("#inputOperatingQuota").val(FormatMoney(opq, 2));
            var mxDate = EconomicGroup.getMaxDate(maxDates);
            $("#inputDateValidity").val(EconomicGroup.convertStringToDate(mxDate));
            $('#tableGroupEconomicResults').UifDataTable('setUnselect', setSelected);
        });
        $("#btnSave").on('click', function () {
            var currentDate = moment().format("DD/MM/YYYY");
            var setSelected = {
                label: 'IndividualId',
                values: []
            };
            $.each(addItem, function (index, value) {
                glbItemAdd.push(value);
            });

            if (addTableItem.length > 0) {
                $.each(addTableItem, function (index, value) {
                    var actualopq = NotFormatMoney($("#inputOperatingQuota").val());
                    var opq = parseInt(value.OperationQuoteAmount);
                    if (Number.isNaN(actualopq)) {
                        $("#inputOperatingQuota").val(FormatMoney(opq, 2));
                    }
                    else {
                        if (CompareDateEquals(value.CurrentTo, currentDate)) {
                            opq += parseInt(actualopq);
                            $("#inputOperatingQuota").val(FormatMoney(opq, 2));
                        }
                    }
                    dateObject = {
                        IndividualId: "",
                        CurrentTo: ""
                    };
                    dateObject.IndividualId = value.IndividualId;
                    dateObject.CurrentTo = EconomicGroup.convertToDate(value.CurrentTo);
                    maxDates.push(dateObject);
                    var mxDate = EconomicGroup.getMaxDate(maxDates);
                    $("#inputDateValidity").val(EconomicGroup.convertStringToDate(mxDate));
                    if (CompareDateEquals(value.CurrentTo, currentDate)) {
                        $("#inputOperatingQuota").val(FormatMoney(opq, 2));
                    }
                    glbDateTablePrincipal.push(value);
                    setSelected.values.push(value.IndividualId);
                });

                $('#tableGroupEconomicResults').UifDataTable('addRow', addTableItem);
                $('#tableGroupEconomicResults').UifDataTable('setSelect', setSelected);
            }

            $('#ModalAddPersons').UifModal('hide');
            unlockScreen();
        });

        $('#searchNameGroup').on('search', function (event, value) {

            var number = parseInt(value, 10);
            if (value != "" || value != null && !isNaN(number)) {
                EconomicGroupRequest.GetEconomicGroupByDocument(value, value).done(data => {
                    if (data.success && data.result.length > 0) {
                        if (data.result.length == 1) {
                            EconomicGroup.mainGroupByIdDescription(data.result);

                        } else {
                            AdvancedSearchGroup.ShowAdvancedSearch();
                            AdvancedSearchGroup.LoadProductAdvanced(data.result);
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.EconomicGroupNoFound, 'autoclose': true });
                    }
                })
            } else
                if (value != "" || value != null && isNaN(number)) {
                    EconomicGroupRequest.GetEconomicGroupByDocument('', value);
                }
            EconomicGroupInicial = $("#tableGroupEconomicResults").UifDataTable('getData');
        });
        $("#inputDateValidity").on('change', function (e) {
            var currentDate = new Date();
            var dateObject = EconomicGroup.convertToDate($("#inputDateValidity").val());
            if (dateObject < currentDate) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanCurrentFromLessCurrent, 'autoclose': true });
            }
        });
        $("#selectPrefix").on('itemSelected', this.changePrefix);
        $("#rdEnabledGroup").on('change', this.changeEnabled);
        $('#tableGroupEconomicResults').on('selectAll', function (event) {
            var setSelected = {
                label: 'IndividualId',
                values: []
            };
            if ($("#rdEnabledGroup").is(":checked")) {
                if (glbDateTablePrincipal.length > 0) {
                    var opq = 0;
                    $.each(glbDateTablePrincipal, function (index, value) {
                        value.Enabled = true;
                        setSelected.values.push(value.IndividualId);
                        opq += parseInt(value.OperationQuoteAmount);
                    });
                    $("#inputOperatingQuota").val(FormatMoney(opq, 2));
                    $('#tableGroupEconomicResults').UifDataTable('clear');
                    $('#tableGroupEconomicResults').UifDataTable('addRow', glbDateTablePrincipal);
                    $('#tableGroupEconomicResults').UifDataTable('setSelect', setSelected);
                }
            } else {
                $.each(glbDateTablePrincipal, function (index, value) {
                    value.Enabled = false;
                    setSelected.values.push(value.IndividualId);
                });
                $('#tableGroupEconomicResults').UifDataTable('setUnselect', setSelected);
                $.UifNotify('show', { 'type': 'info', 'message': 'Para habilitar participantes, el grupo económico debe estar habilitado', 'autoclose': true });
            }
        });
        $('#tableGroupEconomicResults').on('desSelectAll', function (event) {
            if (glbDateTablePrincipal.length > 0) {
                var setSelected = {
                    label: 'IndividualId',
                    values: []
                };
                $.each(glbDateTablePrincipal, function (index, value) {
                    value.Enabled = false;
                    setSelected.values.push(value.IndividualId);
                });
                $("#inputOperatingQuota").val(0);
                $('#tableGroupEconomicResults').UifDataTable('clear');
                $('#tableGroupEconomicResults').UifDataTable('addRow', glbDateTablePrincipal);
                $('#tableGroupEconomicResults').UifDataTable('setUnselect', setSelected);
            }
        });
    }
    createEconomicGroup() {
        lockScreen();
        var economicGroup = EconomicGroup.setModelEconomicGroup();
        if (!economicGroup.Enabled && idEconomicGroup == 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateEconomicGroupEnabled, 'autoclose': true });
        } else
            if (economicGroup.EconomicGroupName == null || economicGroup.EconomicGroupName == '') {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorEconomicGroupNameEmpty, 'autoclose': true });
            } else if (economicGroup.TributaryIdType == null || economicGroup.TributaryIdType == '') {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorEconomicGroupTypeEmpty, 'autoclose': true });
            } else if (economicGroup.OperationQuoteAmount == null || economicGroup.OperationQuoteAmount == '') {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorEconomicGroupOpQuotaEmpty, 'autoclose': true });
            } else if (economicGroup.EnteredDate == null || economicGroup.EnteredDate == '') {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorEconomicGroupDateEmpty, 'autoclose': true });
            } else if (glbDateTablePrincipal.length == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorEconomicIndividualDetEmpty, 'autoclose': true });
            } else {
                EconomicGroupRequest.CreateEconomicGroup(economicGroup, glbItemAdd).done(function (data) {
                    if (data.success) {
                        if (data.result.EconomicGroupId > 0) {
                            idEconomicGroup = data.result.EconomicGroupId;
                            var tributaryId = data.result.TributaryIdNo;
                            $("#lblIdGroup").val(data.result.TributaryIdNo);
                            $("#selectType").val(data.result.TributaryIdType);
                            var EconomicGroupEventDTO;
                            EconomicGroupEventDTO = {};
                            EconomicGroupEventDTO.EconomicGroupEventEventType = EventEconomicGroup.CREATE_ECONOMIC_GROUP;
                            EconomicGroupEventDTO.EconomicGroupID = data.result.EconomicGroupId;
                            EconomicGroupEventDTO.IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO = {};
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO.EconomicGroupID = data.result.EconomicGroupId;
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO.EconomicGroupName = data.result.EconomicGroupName;
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO.Enable = data.result.Enabled;
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO.ValueOpQuota = data.result.OperationQuoteAmount;
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO.InitDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                            EconomicGroupEventDTO.EconomicGroupOperatingQuotaDTO.DeclineDate = FormatDate(data.result.EnteredDate);
                            EconomicGroupRequest.CreateEconomicGroupEvent(EconomicGroupEventDTO).done(function (response) {
                                if (response.success) {
                                    if (response.result) {
                                        var EconomicGroupEventDTOs = []
                                        glbDateTablePrincipal.forEach(function (item, index) {
                                            EconomicGroupEventDTOs[index] = {};
                                            EconomicGroupEventDTOs[index].EconomicGroupEventEventType = EventEconomicGroup.ENABLED_INDIVIDUAL_TO_ECONOMIC_GROUP;
                                            EconomicGroupEventDTOs[index].EconomicGroupID = data.result.EconomicGroupId;
                                            EconomicGroupEventDTOs[index].IndividualId = item.IndividualId;
                                            EconomicGroupEventDTOs[index].IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                                            EconomicGroupEventDTOs[index].EconomicgrouppartnersDTO = {};
                                            EconomicGroupEventDTOs[index].EconomicgrouppartnersDTO.EconomicGroupId = data.result.EconomicGroupId;
                                            EconomicGroupEventDTOs[index].EconomicgrouppartnersDTO.IndividualId = item.IndividualId;
                                            EconomicGroupEventDTOs[index].EconomicgrouppartnersDTO.Enable = item.Enabled;
                                            EconomicGroupEventDTOs[index].EconomicgrouppartnersDTO.InitDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                                            EconomicGroupEventDTOs[index].EconomicgrouppartnersDTO.DeclineDate = item.CurrentTo;
                                            if (!item.Enabled) {
                                                EconomicGroupEventDTOs[index].EconomicGroupEventEventType = EventEconomicGroup.DISABLED_INDIVIDUAL_TO_ECONOMIC_GROUP;
                                            }
                                        });
                                        EconomicGroupRequest.AssigendIndividualToEconomicGroupEvent(EconomicGroupEventDTOs).done(function (data) {
                                            if (response.success) {
                                                if (response.result) {
                                                    EconomicGroup.clearFields();
                                                    $.UifDialog('alert', { 'message': "El grupo se guardo con el ID " + tributaryId + " exitosamente." });
                                                }
                                            }
                                        });
                                    }
                                }
                                else {
                                    $.UifDialog('alert', { 'message': response.result });
                                }
                            });
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        unlockScreen();
    }

    showPersons() {
        $('#ModalAddPersons').UifModal('showLocal', 'Agregar participantes');
        $("#messageNoQuotaError").UifAlert('hide');
        $("#SearchindividualId").val('');
        $("#inputDocumentType").val('');
        $("#inputNroDocumento").val('');
        $("#inputBusinessName").val('');
    }

    setControlsSearch(data) {
        $("#selectType").val(data.result.CustomerType);
        $("#inputNameGroup").val(data.result.Name);
        $("#rdEnabledGroup").prop("checked", data.result.Enabled);
        $("#inputOperatingQuota").val(FormatMoney(data.result.OperationQuoteAmount, 2));
        $("#inputDateValidity").val(FormatDate());
    }

    static setModelEconomicGroup() {

        var listEconomicGroupDetail = []
        $.each(glbItemAdd, function (index, value) {
            var economicGroupDetail = {
                EconomicGroupId: 0,
                IndividualId: value.IndividualId,
                Enabled: $("#rdEnabledGroup").is(":checked") ? value.Enabled : false,
                DeclinedDate: new Date,
                Description: value.Description
            }

            if (!listEconomicGroupDetail.includes(economicGroupDetail)) { listEconomicGroupDetail.push(economicGroupDetail); }
        });
        var economicGroup = {
            EconomicGroupId: idEconomicGroup,
            EconomicGroupName: $("#inputNameGroup").val(),
            TributaryIdType: $("#selectType").val(),
            TributaryIdNo: $("#lblIdGroup").val(),
            VerifyDigit: 1,
            EnteredDate: $("#inputDateValidity").val(),
            OperationQuoteAmount: NotFormatMoney($("#inputOperatingQuota").val()),
            Enabled: $("#rdEnabledGroup").is(":checked"),
            EconomicGroupDetail: listEconomicGroupDetail
        }
        return economicGroup;
    }

    getEconomicGroup() {
        EconomicGroupRequest.GetTrbutaryType().done(function (data) {
            if (data.success && data.result.length > 0) {
                $("#selectType").UifSelect({ sourceData: data.result });
                $("#selectType").UifSelect("setSelected", null);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.EconomicGroupNoFound, 'autoclose': true });
            }
        });
    }

    ShowIndividualResults(dataTable) {
        $('#tableIndividual').UifDataTable('clear');
        $('#tableIndividual').UifDataTable('addRow', dataTable);
        unlockScreen();
    }

    static GetInsuredByIndividualId(arrayInsured) {
        $('#modalInsuredSearch').UifModal('hide');
    }

    getHolder() {
        addItem = [];
        addTableItem = [];
        EconomicGroupRequest.GetHolders($("#SearchindividualId").val()).done(function (data) {
            $("#messageNoQuotaError").UifAlert('hide');
            if (data.success) {
                if (data.result.length == 1 && data.result[0].InsuredId > 0) {
                    $("#SearchindividualId").val(data.result[0].Name);
                    $("#inputDocumentType").val(data.result[0].IdentificationDocument.DocumentType.Description);
                    $("#inputNroDocumento").val(data.result[0].IdentificationDocument.Number);
                    $("#inputBusinessName").val(data.result[0].Name);

                    if (data.result[0].DeclinedDate == null) {
                        var individualId = data.result[0].IndividualId;
                        EconomicGroupRequest.GetIndividualsDetail(individualId, idEconomicGroup).done(function (result) {
                            if (result.success && result.result.length > 0) {
                                var businessLine = $("#selectPrefix").UifSelect("getSelected");
                                var currentDate = new Date();
                                var havePrefixToAdd = false;

                                if (businessLine != "") {
                                    $.each(result.result, function (index, value) {
                                        var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                        if (EconomicGroup.convertToDate(currentTo) > currentDate && businessLine == value.LineBusinessId) {
                                            havePrefixToAdd = true;
                                        }
                                    });
                                    if (havePrefixToAdd) {
                                        $.each(result.result, function (index, value) {
                                            var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                            if (EconomicGroup.convertToDate(currentTo) > currentDate) {
                                                var dataItemList = {
                                                    IndividualId: data.result[0].IndividualId,
                                                    CustomerType: data.result[0].CustomerType,
                                                    Code: data.result[0].InsuredId,
                                                    DocumentNum: data.result[0].IdentificationDocument.Number,
                                                    Description: data.result[0].Name,
                                                    CustomerTypeDescription: data.result[0].CustomerTypeDescription,
                                                    DocumentType: data.result[0].IdentificationDocument.DocumentType.Description,
                                                    OperationQuoteAmount: value.AmountValue,
                                                    CurrentTo: value.CurrentTo != null ? FormatDate(value.CurrentTo) : '',
                                                    Enabled: true,
                                                    LineBusiness: value.LineBusinessId,
                                                    LineBusinessDescription: ''
                                                };
                                                var contain = false;
                                                $.each(glbItemAdd, function (index, value) {
                                                    if (dataItemList.IndividualId == value.IndividualId && value.LineBusiness == dataItemList.LineBusiness) {
                                                        contain = true;
                                                    }
                                                });
                                                if (!contain) {
                                                    if (dataItemList.DocumentType.includes('NIT')) {
                                                        dataItemList.CustomerTypeDescription = 'Compañía';
                                                    } else {
                                                        dataItemList.CustomerTypeDescription = 'Persona';
                                                    }

                                                    $.each(gblPrefixes, function (index, value) {
                                                        if (dataItemList.LineBusiness == value.Id) {
                                                            dataItemList.LineBusinessDescription = value.Description;
                                                        }
                                                    });

                                                    addItem.push(dataItemList);
                                                    if (businessLine == value.LineBusinessId) {
                                                        $("#messageNoQuotaError").UifAlert('hide');
                                                        addTableItem.push(dataItemList);
                                                    }
                                                }
                                            }
                                        });
                                    } else {
                                        $.each(result.result, function (index, value) {
                                            var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                            if (EconomicGroup.convertToDate(currentTo) < currentDate && businessLine == value.LineBusinessId) {
                                                //$("#messageNoQuotaError").UifAlert('show', 'El cupo operativo no está vigente.', "danger");
                                                //return false;
                                            } else {
                                                $("#messageNoQuotaError").UifAlert('show', 'El participante no tiene un cupo asignado al ramo.', "danger");
                                            }
                                        });

                                    }
                                } else {
                                    $.each(result.result, function (index, value) {
                                        var dataItemList = {
                                            IndividualId: data.result[0].IndividualId,
                                            CustomerType: data.result[0].CustomerType,
                                            Code: data.result[0].InsuredId,
                                            DocumentNum: data.result[0].IdentificationDocument.Number,
                                            Description: data.result[0].Name,
                                            CustomerTypeDescription: data.result[0].CustomerTypeDescription,
                                            DocumentType: data.result[0].IdentificationDocument.DocumentType.Description,
                                            OperationQuoteAmount: value.AmountValue,
                                            CurrentTo: value.CurrentTo != null ? FormatDate(value.CurrentTo) : '',
                                            Enabled: true,
                                            LineBusiness: value.LineBusinessId,
                                            LineBusinessDescription: ''
                                        };

                                        if (dataItemList.DocumentType.includes('NIT')) {
                                            dataItemList.CustomerTypeDescription = 'Compañía';
                                        } else {
                                            dataItemList.CustomerTypeDescription = 'Persona';
                                        }

                                        var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                        var currentDate = new Date();
                                        var businessLine = $("#selectPrefix").UifSelect("getSelected");
                                        //if (EconomicGroup.convertToDate(currentTo) > currentDate) {
                                        var contain = false;
                                        $.each(glbItemAdd, function (index, value) {
                                            if (dataItemList.IndividualId == value.IndividualId && value.LineBusiness == dataItemList.LineBusiness) {
                                                contain = true;
                                            }
                                        });
                                        //if (!contain) {
                                        if (businessLine == value.LineBusinessId || businessLine == "") {
                                            $.each(gblPrefixes, function (index, value) {
                                                if (dataItemList.LineBusiness == value.Id) {
                                                    dataItemList.LineBusinessDescription = value.Description;
                                                }
                                            });
                                        } else {
                                            $("#messageNoQuotaError").UifAlert('show', 'El participante no tiene cupo operativo asignado al ramo seleccionado', "danger");
                                        }
                                        addItem.push(dataItemList);
                                        $("#messageNoQuotaError").UifAlert('hide');
                                        addTableItem.push(dataItemList);
                                        //}
                                        //}
                                        //else {
                                        //    $("#messageNoQuotaError").UifAlert('show', 'El cupo operativo no está vigente', "danger");
                                        //}
                                    });
                                }
                            } else {
                                $("#messageNoQuotaError").UifAlert('show', result.result, "danger");
                            }
                        });
                    } else {
                        $("#messageNoQuotaError").UifAlert('show', "El asegurado se encuentra dado de baja.", "danger");
                    }
                } else if (data.result.length == 0 || data.result[0].InsuredId <= 0) {
                    $("#inputDocumentType").val('');
                    $("#inputNroDocumento").val('');
                    $("#inputBusinessName").val('');
                    $("#messageNoQuotaError").UifAlert('show', "El asegurado no se encontró o no existe.", "danger");
                } else {
                    var dataList = [];
                    if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            if (data.result[i].IdentificationDocument.DocumentType.Description.includes('NIT')) {
                                data.result[i].CustomerTypeDescription = 'Compañía';
                            } else {
                                data.result[i].CustomerTypeDescription = 'Persona';
                            }
                            var dataItem = {
                                IndividualId: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].InsuredId,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            };
                            dataList.push(dataItem);
                        }
                        EconomicGroup.SearchIndividualResults(dataList);
                        $('#modalInsuredSearch').UifModal('showLocal', 'Seleccione el participante');
                    }
                }
            } else {
                $("#inputDocumentType").val('');
                $("#inputNroDocumento").val('');
                $("#inputBusinessName").val('');
                $("#messageNoQuotaError").UifAlert('show', "El asegurado no se encontró o no existe.", "danger");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
        });
    }

    SelectInsuredResult(e) {
        addItem = [];
        addTableItem = [];
        var dataItem = {
            IndividualId: $(this).children()[0].innerHTML,
            CustomerType: $(this).children()[1].innerHTML,
            Code: $(this).children()[2].innerHTML,
            DocumentNum: $(this).children()[3].innerHTML,
            Description: $(this).children()[4].innerHTML,
            CustomerTypeDescription: $(this).children()[5].innerHTML,
            DocumentType: $(this).children()[6].innerHTML,
            OperationQuoteAmount: 0,
            CurrentTo: '',
            Enabled: true,
            LineBusiness: ''
        };

        if (dataItem.Code <= 0) {
            $("#SearchindividualId").val('');
            $("#inputDocumentType").val('');
            $("#inputNroDocumento").val('');
            $("#inputBusinessName").val('');
            $("#messageNoQuotaError").UifAlert('show', "El asegurado no se encontró o no existe.", "danger");
            EconomicGroup.GetInsuredByIndividualId(dataItem);
            return;
        } else {
            $("#SearchindividualId").val(dataItem.Description);
            $("#inputDocumentType").val(dataItem.DocumentType);
            $("#inputNroDocumento").val(dataItem.DocumentNum);
            $("#inputBusinessName").val(dataItem.Description);
        }
        EconomicGroupRequest.GetHolders(dataItem.DocumentNum).done(function (data) {
            if (data.result[0].DeclinedDate == null) {
                EconomicGroupRequest.GetIndividualsDetail(dataItem.IndividualId, idEconomicGroup).done(function (result) {
                    if (result.success && result.result.length > 0) {
                        var businessLine = $("#selectPrefix").UifSelect("getSelected");
                        var currentDate = new Date();
                        var havePrefixToAdd = false;
                        if (businessLine != "") {
                            $.each(result.result, function (index, value) {
                                var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                if (EconomicGroup.convertToDate(currentTo) > currentDate && businessLine == value.LineBusinessId) {
                                    havePrefixToAdd = true;
                                }
                            });
                            if (havePrefixToAdd) {
                                $.each(result.result, function (index, value) {
                                    var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                    if (EconomicGroup.convertToDate(currentTo) > currentDate) {
                                        var dataItemList = {
                                            IndividualId: dataItem.IndividualId,
                                            CustomerType: dataItem.CustomerType,
                                            Code: dataItem.Code,
                                            DocumentNum: dataItem.DocumentNum,
                                            Description: dataItem.Description,
                                            CustomerTypeDescription: dataItem.CustomerTypeDescription,
                                            DocumentType: dataItem.DocumentType,
                                            OperationQuoteAmount: value.AmountValue,
                                            CurrentTo: value.CurrentTo != null ? FormatDate(value.CurrentTo) : '',
                                            Enabled: true,
                                            LineBusiness: value.LineBusinessId
                                        };
                                        var contain = false;
                                        $.each(glbItemAdd, function (index, value) {
                                            if (dataItemList.IndividualId == value.IndividualId && value.LineBusiness == dataItemList.LineBusiness) {
                                                contain = true;
                                            }
                                        });
                                        if (!contain) {
                                            if (dataItemList.DocumentType.includes('NIT')) {
                                                dataItemList.CustomerTypeDescription = 'Compañía';
                                            } else {
                                                dataItemList.CustomerTypeDescription = 'Persona';
                                            }

                                            $.each(gblPrefixes, function (index, value) {
                                                if (dataItemList.LineBusiness == value.Id) {
                                                    dataItemList.LineBusinessDescription = value.Description;
                                                }
                                            });
                                            addItem.push(dataItemList);
                                            if (businessLine == value.LineBusinessId) {
                                                $("#messageNoQuotaError").UifAlert('hide');
                                                addTableItem.push(dataItemList);
                                            }
                                        }
                                    }
                                });
                            } else {
                                $.each(result.result, function (index, value) {
                                    var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                    if (EconomicGroup.convertToDate(currentTo) < currentDate && businessLine == value.LineBusinessId) {
                                        //$("#messageNoQuotaError").UifAlert('show', 'El cupo operativo no está vigente.', "danger");
                                        //return false;
                                    } else {
                                        $("#messageNoQuotaError").UifAlert('show', 'El participante no tiene un cupo asignado al ramo.', "danger");
                                    }
                                });
                            }
                        } else {
                            $.each(result.result, function (index, value) {
                                var dataItemList = {
                                    IndividualId: dataItem.IndividualId,
                                    CustomerType: dataItem.CustomerType,
                                    Code: dataItem.Code,
                                    DocumentNum: dataItem.DocumentNum,
                                    Description: dataItem.Description,
                                    CustomerTypeDescription: dataItem.CustomerTypeDescription,
                                    DocumentType: dataItem.DocumentType,
                                    OperationQuoteAmount: value.AmountValue,
                                    CurrentTo: value.CurrentTo != null ? FormatDate(value.CurrentTo) : '',
                                    Enabled: true,
                                    LineBusiness: value.LineBusinessId
                                };

                                if (dataItemList.DocumentType.includes('NIT')) {
                                    dataItemList.CustomerTypeDescription = 'Compañía';
                                } else {
                                    dataItemList.CustomerTypeDescription = 'Persona';
                                }

                                var currentTo = value.CurrentTo != null ? FormatDate(value.CurrentTo) : '';
                                var currentDate = new Date();
                                var businessLine = $("#selectPrefix").UifSelect("getSelected");
                                if (EconomicGroup.convertToDate(currentTo) > currentDate) {
                                    var contain = false;
                                    $.each(glbItemAdd, function (index, value) {
                                        if (dataItemList.IndividualId == value.IndividualId && value.LineBusiness == dataItemList.LineBusiness) {
                                            contain = true;
                                        }
                                    });
                                    if (!contain) {
                                        if (businessLine == value.LineBusinessId || businessLine == "") {
                                            $.each(gblPrefixes, function (index, value) {
                                                if (dataItemList.LineBusiness == value.Id) {
                                                    dataItemList.LineBusinessDescription = value.Description;
                                                }
                                            });
                                        } else {
                                            $("#messageNoQuotaError").UifAlert('show', 'El participante no tiene cupo operativo asignado al ramo seleccionado', "danger");
                                        }
                                        addItem.push(dataItemList);
                                        $("#messageNoQuotaError").UifAlert('hide');
                                        addTableItem.push(dataItemList);
                                    }
                                }
                                //else {
                                //    $("#messageNoQuotaError").UifAlert('show', 'El cupo operativo no está vigente', "danger");
                                //}
                            });
                        }
                    } else if (result.result.length == 0) {
                        $("#inputDocumentType").val('');
                        $("#inputNroDocumento").val('');
                        $("#inputBusinessName").val('');
                        $("#messageNoQuotaError").UifAlert('show', "El asegurado no se encontró o no existe.", "danger");
                    } else {
                        $("#messageNoQuotaError").UifAlert('show', result.result, "danger");
                    }
                });
            } else {
                $("#messageNoQuotaError").UifAlert('show', "El asegurado se encuentra dado de baja.", "danger");
            }
        });
        EconomicGroup.GetInsuredByIndividualId(dataItem);
    }

    static SearchIndividualResults(dataTable) {
        $('#tableInsuredResults').UifDataTable('clear');
        $('#tableInsuredResults').UifDataTable('addRow', dataTable);
    }

    static deleteCallbackItem(deferred, result) {
        deferred.resolve();
        result.Status = ParametrizationStatus.Delete;
        glbItemDelete.push(result);
        var newItems = [];
        $.each(glbDateTablePrincipal, function (index, value) {
            if (value.IndividualId != result.IndividualId) {
                newItems.push(value);
            }
        });
        glbDateTablePrincipal = newItems;
        $('#tableIndividual').UifDataTable('clear');
        $('#tableIndividual').UifDataTable('addRow', glbDateTablePrincipal);
    }

    static mainGroupByIdDescription(economicGroup) {
        EconomicGroupRequest.GetIndividualGroupDetail(economicGroup[0].EconomicGroupId).done(function (data) {
            var currentDate = new Date();
            var setSelected = {
                label: 'IndividualId',
                values: []
            };
            var systemDisabledParticipants = [];
            glbDateTablePrincipal = [];
            maxDates = [];
            glbItemAdd = [];
            idEconomicGroup = economicGroup[0].EconomicGroupId;
            $('#inputNameGroup').val(economicGroup[0].EconomicGroupName);
            $("#selectType").UifSelect("setSelected", economicGroup[0].TributaryIdType);
            $('#lblIdGroup').val(economicGroup[0].TributaryIdNo);
            $('#inputOperatingQuota').val(economicGroup[0].OperationQuoteAmount);
            $("#inputDateValidity").UifDatepicker('setValue', FormatDate(economicGroup[0].EnteredDate, 1));
            $('#rdEnabledGroup').prop('checked', economicGroup[0].Enabled);
            $("#selectPrefix").UifSelect("setSelected", null);
            if (data.result.length > 0) {
                var opq = 0;
                var mxDate = "";
                $.each(data.result, function (index, value) {
                    var declinedDate = new Date(parseInt(value.DeclinedDate.substr(6)));
                    if (declinedDate.getFullYear() < 1900) {
                        var enteredDate = new Date(parseInt(value.EnteredDate.substr(6)));
                        switch (value.IndividualType) {
                            case 1: value.IndividualType = 'Persona'
                                break;
                            case 2: value.IndividualType = 'Compañía'
                                break;
                        }
                        var document = value.IdentificationDocument.split('_')
                        var datetimeCurrent = value.EnteredDate;
                        var dataItem = {
                            DocumentNum: document[0],
                            Description: value.FullName,
                            OperationQuoteAmount: value.ExtendedProperties[0].Value,
                            CurrentTo: FormatDate(value.EnteredDate, 1),
                            CustomerTypeDescription: value.IndividualType,
                            IndividualId: value.IndividualId,
                            CustomerType: value.IndividualType,
                            Code: value.InsuredCode,
                            DocumentType: value.IndividualType == 'Persona' ? 'CC' : 'NIT',
                            Enabled: value.ExtendedProperties[2].Value,
                            LineBusiness: value.ExtendedProperties[1].Value,
                            LineBusinessDescription: ''
                        };
                        $.each(gblPrefixes, function (index, valuePrefix) {
                            if (dataItem.LineBusiness == valuePrefix.Id) {
                                dataItem.LineBusinessDescription = valuePrefix.Description;
                            }
                        });
                        glbItemAdd.push(dataItem);
                        //if (enteredDate >= currentDate) {
                        glbDateTablePrincipal.push(dataItem);
                        if (dataItem.Enabled) {
                            setSelected.values.push(dataItem.IndividualId);
                            if (enteredDate >= currentDate) {
                                opq += parseInt(dataItem.OperationQuoteAmount);
                            }
                            dateObject = {
                                IndividualId: "",
                                CurrentTo: ""
                            };
                            dateObject.IndividualId = dataItem.IndividualId;
                            dateObject.CurrentTo = EconomicGroup.convertToDate(dataItem.CurrentTo);
                            maxDates.push(dateObject);
                            mxDate = EconomicGroup.getMaxDate(maxDates);
                        }
                        //}
                    } else {
                        var newIndex = index == 0 ? index : index - 1;
                        if (value.IndividualId != data.result[newIndex].IndividualId || newIndex == 0) {
                            var searchDisabledParticipants = systemDisabledParticipants.filter(function (item) {
                                return item == value.FullName;
                            });
                            if (searchDisabledParticipants.length == 0) {
                                systemDisabledParticipants.push(value.FullName);
                            }
                        }
                    }
                });
                $.each(systemDisabledParticipants, function (index, value) {
                    $.UifNotify('show', { 'type': 'info', 'message': 'El participante ' + value + ' se encuentra dado de baja.', 'autoclose': true });
                });
                $("#inputDateValidity").val(EconomicGroup.convertStringToDate(mxDate));
                $("#inputOperatingQuota").val(FormatMoney(opq, 2));
                $('#tableGroupEconomicResults').UifDataTable('clear');
                if (glbDateTablePrincipal.length > 0) { $('#tableGroupEconomicResults').UifDataTable('addRow', glbDateTablePrincipal); }
                else { $.UifNotify('show', { 'type': 'info', 'message': AppResources.EconomicGroupNoParticipants, 'autoclose': true }); }
                $('#tableGroupEconomicResults').UifDataTable('setSelect', setSelected);
            } else {
                $('#tableGroupEconomicResults').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.EconomicGroupNoParticipants, 'autoclose': true });
            }

        });
    }

    static clearFields() {
        idEconomicGroup = 0;
        $('#inputNameGroup').val("");
        $('#searchNameGroup').val("");
        $("#selectType").UifSelect("setSelected", null);
        $('#lblIdGroup').val("");
        $('#inputOperatingQuota').val(0);
        $("#selectPrefix").UifSelect("setSelected", null);
        $("#inputDateValidity").UifDatepicker('setValue', null);
        $('#rdEnabledGroup').prop('checked', true);
        $('#tableGroupEconomicResults').UifDataTable('clear');
        glbItemAdd = [];
        glbDateTablePrincipal = [];
        maxDates = [];
        addItem = [];
        addTableItem = [];
    }

    CancelView() {
        if (glbPolicy == null) {
            window.location = rootPath + "Home/Index";
        }
        else {
            if (glbPolicy.TemporalType == TemporalType.Quotation) {
                glbPersonOnline = null;
                router.run("prtQuotation");
            }
            else {
                glbPersonOnline = null;
                router.run("prtTemporal");
            }
        }
    }

    changePrefix() {
        if (glbItemAdd.length > 0) {
            var setSelected = {
                label: 'IndividualId',
                values: []
            };
            var changeDataTable = [];
            var currentDate = new Date();
            var selectedItem = $("#selectPrefix").UifSelect("getSelected");
            var operatingQuota = 0;
            $.each(glbItemAdd, function (index, value) {
                var currentTo = EconomicGroup.convertToDate(value.CurrentTo);
                if (currentTo >= currentDate && (value.LineBusiness == selectedItem || selectedItem == "")) {
                    if (value.Enabled) {
                        setSelected.values.push(value.IndividualId);
                        operatingQuota += parseInt(value.OperationQuoteAmount);
                    }
                    changeDataTable.push(value);
                }
            });
            $("#inputOperatingQuota").val(FormatMoney(operatingQuota, 2));
            $('#tableGroupEconomicResults').UifDataTable('clear');
            if (changeDataTable.length > 0) {
                $('#tableGroupEconomicResults').UifDataTable('addRow', changeDataTable);
                $('#tableGroupEconomicResults').UifDataTable('setSelect', setSelected);
                glbDateTablePrincipal = changeDataTable;
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ParticipantNoQuota, 'autoclose': true });
                glbDateTablePrincipal = [];
                maxDates = [];
            }
        }
    }

    changeEnabled() {
        var setSelected = {
            label: 'IndividualId',
            values: []
        };
        var oq = 0;
        var enabledGroup = $("#rdEnabledGroup").is(':checked');
        $.each(glbItemAdd, function (index, value) {
            value.Enabled = enabledGroup;
            setSelected.values.push(value.IndividualId);
        });
        $.each(glbDateTablePrincipal, function (index, value) {
            value.Enabled = enabledGroup;
            if (value.Enabled) { oq += parseInt(value.OperationQuoteAmount); }
            setSelected.values.push(value.IndividualId);
        });
        $("#inputOperatingQuota").val(FormatMoney(oq, 2));
        if (enabledGroup) { $('#tableGroupEconomicResults').UifDataTable('setSelect', setSelected); }
        else { $('#tableGroupEconomicResults').UifDataTable('setUnselect', setSelected); }

    }

    static convertToDate(datestring) {
        var dateString = datestring;
        var dateParts = dateString.split("/");
        var dateObject = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
        return dateObject;
    }

    static convertStringToDate(inputFormat) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        var d = new Date(inputFormat)
        return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/')
    }

    static getMaxDate(maxDatesArray) {
        var maxDate = "";
        for (var i = 0; i < maxDatesArray.length; i++) {
            if (i > 0) {
                if (maxDatesArray[i].CurrentTo > maxDate) {
                    maxDate = maxDatesArray[i].CurrentTo;
                }
            } else {
                maxDate = maxDatesArray[i].CurrentTo;
            }
        }
        return maxDate;
    }
}
