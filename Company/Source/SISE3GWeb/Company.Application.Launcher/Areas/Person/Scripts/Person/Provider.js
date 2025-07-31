var paymentConceptDelete = [];
var positionEditSpecialities = null;
var GobalAccountingConcepts = null;
var result = null;


class Provider extends Uif2.Page {
    getInitialState() {
        Provider.dataInitialSupplier();
    }

    static dataInitialSupplier() {
        $("#codePersonProvider").val("");
        $("#codeProvider").val("");
        $("#inputObservationProvider").val("");
        $("#selectProviderType").UifSelect("setSelected", null);
        $("#selectProviderDeclined").UifSelect("setSelected", null);
        $("#selectPaymentConcept").UifSelect("setSelected", null);
        $("#dateProviderCreation").UifDatepicker("setValue", new Date());
        $("#SupplierdateModificationDate").UifDatepicker("setValue", new Date());
        $('#checkProvider').prop('checked', false);
        $("#listVPaymentConcept").UifListView({
            sourceData: null,
            delete: true,
            customDelete: false,
            height: 300,
            displayTemplate: '#display-paymentConcepts',
            deleteCallback: Provider.deleteCallback
        });
    }


    bindEvents() {
        $("#btnProvider").click(this.showSupplier);
        $("#btnPaymentConceptsAdd").click(this.addPaymentConceptListView);


        $("#btnAcceptProvider").click(this.processInsertProviderPerson);
        $("#selectProviderDeclined").on("itemSelected", this.SelectedProviderDeclined);
        $("#modalProvider").on('closed.modal', this.closeModal);
        $("#listVPaymentConcept").on('rowDelete', Provider.deleteCallback);
        $("#selectProviderType").on("itemSelected", Provider.GetSupplierProfiles);
    }
    
    showSupplier() {
        //$("#dateProviderCreation").UifDatepicker("setValue", new Date());
        var individualId = $('#lblPersonCode').val() || $('#lblCompanyCode').val();
        if (individualId == Person.New || individualId <= 0 || individualId == undefined) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
        else {
            $.UifProgress('show');
            ProviderRequest.GetSupplierTypes().done(function (data) {
                if (data.success) {
                    $("#selectProviderType").UifSelect({ sourceData: data.result });
                }
            });

            ProviderRequest.GetSupplierDeclinedType().done(function (data) {
                if (data.success) {
                    $("#selectProviderDeclined").UifSelect({ sourceData: data.result });
                }
            });

            ProviderRequest.GetGroupSupplier().done(function (data) {
                if (data.success) {
                    $("#selectGroupSupplier").UifMultiSelect({ sourceData: data.result });
                }
            });

            ProviderRequest.GetAccountingConcepts().done(function (data) {
                if (data.success) {
                    $.each(data.result, function (index, value) {
                        value.Description = value.Id + " - " + value.Description;  
                    });
                    $("#selectPaymentConcept").UifSelect({ sourceData: data.result });
                    GobalAccountingConcepts = data.result;
                }
            });
            $("#codePersonProvider").val(individualId);
            ProviderRequest.GetSupplierByIndividualId(individualId);
            Persons.ShowPanelsPerson(RolType.Provider);
        }
    }


    static GetProvider(data) {

        if (data.IndividualId > 0) {
            $('#codePerson').text(data.IndividualId);
            $('#codeProvider').text(data.Id);
            $('#Id').val(data.Id);
            $('#selectProviderType').val(data.ProviderTypeId);
            $('#dateProviderCreation').val(FormatDate(data.CreationDate));
            $('#SupplierdateModificationDate').val(FormatDate(data.ModificationDate));
            $('#SupplierDateDeclinationDate').val(FormatDate(data.DeclinationDate));
            $('#selectProviderDeclined').val(data.ProviderDeclinedTypeId);
            $('#inputObservationProvider').val(data.Observation);
            if (data.GroupSupplier != null && data.GroupSupplier.length > 0) {
                $.each(data.GroupSupplier, function (index, value) {
                    $('#selectGroupSupplier').UifMultiSelect('setSelected', value.Id);
                });
            }

            $('#listVPaymentConcept').UifListView({
                sourceData: data.ProviderPaymentConcepts,
                delete: true,
                customDelete: false,
                height: 300,
                displayTemplate: '#display-paymentConcepts',
                deleteCallback: Provider.deleteCallback
            });

            //se agregan los PaymentConcept a la lista
            Provider.addPaymentConceptToList(data.Id);
            Provider.GetSupplierProfiles(data.SupplierProfileId);
        }
        else {
            Provider.clearProvider();
        } 

    }

    static deleteCallback(deferred, data) {
        data.Id = -1;
        paymentConceptDelete.push(data);
        deferred.resolve();
    }

    static addPaymentConceptToList(SupplierId) {

        if (GobalAccountingConcepts != null) {
            ProviderRequest.GetSupplierAccountingConceptsBySupplierId(SupplierId).done(function (data) {
                var ItemPaymentConceptAdd = null;

                var indexAccountingConcept = -1;
                if (data.success) {
                    result = data.result;
                    for (var i = 0; i < result.length; i++) {
                        indexAccountingConcept = GobalAccountingConcepts.findIndex(j => j.Id === result[i].AccountingConceptId);
                        if (indexAccountingConcept != -1) {
                            ItemPaymentConceptAdd = { PaymentConceptId: GobalAccountingConcepts[indexAccountingConcept].Id, Description: GobalAccountingConcepts[indexAccountingConcept].Description, Id: 0 };
                            $("#listVPaymentConcept").UifListView("addItem", ItemPaymentConceptAdd);
                        }
                    }
                }
            });
        }

    }

    processInsertProviderPerson() {
        var objBasicInformationProvider = $("#formBasicInformationProvider").serializeObject();
        if ($("#lblCompanyCode").val() != undefined && $("#lblCompanyCode").val() != "") {
            objBasicInformationProvider.IndividualId = $("#lblCompanyCode").val();
        }
        else if ($("#lblPersonCode").val() != undefined && $("#lblPersonCode").val() != "") {
            objBasicInformationProvider.IndividualId = $("#lblPersonCode").val();
        }
        objBasicInformationProvider.Id = $("#codeProvider").html();
        objBasicInformationProvider.ProviderSpeciality = $("#selectSpeciality").UifSelect("getSelectedSource");
        objBasicInformationProvider.PaymentConcept = $("#listVPaymentConcept").UifListView("getData");
        var groupSupplier = { Id: 0, Description: "" };
        var groupSupplierList = [];
        $.each($('#selectGroupSupplier').UifMultiSelect('getSelected'), function (index, value) {
            groupSupplier.Id = value;
            groupSupplierList.push($.extend({}, groupSupplier));
        });
        objBasicInformationProvider.GroupSupplier = groupSupplierList;
        if (typeof (objBasicInformationProvider.ProviderSpeciality) !== "undefined") {
            objBasicInformationProvider.SupplierProfileId = objBasicInformationProvider.ProviderSpeciality.Id;
        }

        if (Provider.validateProvider(objBasicInformationProvider)) {
            lockScreen();
            ProviderRequest.CreateSupplier(objBasicInformationProvider).done(function (data) {
                if (data.success) {

                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonProvider);
                        }
                    } else {
                        Provider.clearProvider();
                        Provider.loadProviderPerson(data.result);
                        $("#modalProvider").UifModal("hide");
                        $.UifNotify('show', { 'type': 'info', 'message': 'Guardado con éxito.', 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());
        }
    }

    static validateProvider(objBasicInformationProvider) {
        if (Provider.validateBasicInformation(objBasicInformationProvider) && Provider.validatePaymentConcept(objBasicInformationProvider)) {
            return true;
        }
    }

    addPaymentConceptListView() {
        var ItemPaymentConcept = $("#selectPaymentConcept").UifSelect("getSelectedSource");
        var ItemPaymentConceptAdd = { PaymentConceptId: ItemPaymentConcept.Id, Description: ItemPaymentConcept.Description, Id: 0 };
        var ItemslistVPaymentConcept = $("#listVPaymentConcept").UifListView("getData");

        var Exists = ItemslistVPaymentConcept.findIndex(i => i.PaymentConceptId === ItemPaymentConceptAdd.PaymentConceptId);
        if (Exists == -1) {
            $("#listVPaymentConcept").UifListView("addItem", ItemPaymentConceptAdd);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessagePaymentConceptExisting, 'autoclose': true });
        }
    }

    //wmw fin usado

    SelectedProviderDeclined(event, selectedItem) {
        if (selectedItem.Id != "") {
            $("#SupplierDateDeclinationDate").UifDatepicker("setValue", DateNowPerson);
        }
        else {
            $("#SupplierDateDeclinationDate").UifDatepicker("clear");
        }
    }

    static GetSupplierProfiles(supplierTypeId) {
        if ($("#selectProviderType").UifSelect("getSelected") != "") {
            ProviderRequest.GetSupplierProfiles($("#selectProviderType").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    $("#selectSpeciality").UifSelect({ sourceData: data.result });
                    if (supplierTypeId > 0) {
                        $("#selectSpeciality").UifSelect("setSelected", supplierTypeId);
                    }
                }
            });
        }
    }

    closeModal() {
        $('#tabsProvider a:first').tab('show');
        $('#editActionSpecialties').UifInline('hide');
    }

    static clearProvider() {
        $("#codePersonProvider").val("");
        $("#codeProvider").val("");
        $("#inputObservationProvider").val("");
        $("#selectProviderType").UifSelect("setSelected", null);
        $("#selectProviderDeclined").UifSelect("setSelected", null);
        $("#selectPaymentConcept").UifSelect("setSelected", null);
        $("#selectSpeciality").UifSelect({ sourceData: null });
        $("#dateProviderCreation").UifDatepicker("setValue", new Date());
        $("#SupplierdateModificationDate").UifDatepicker("setValue", new Date());
        $("#SupplierDateDeclinationDate").UifDatepicker("clear");
        $('#checkProvider').prop('checked', false);
        $('#selectGroupSupplier').UifMultiSelect('deSelectAll');
        if ($("#listVPaymentConcept").UifListView('getData').length > 0) {
            $("#listVPaymentConcept").UifListView("clear");
        }
    }



    static loadProviderPerson(objInformationProvider) {
        if (objInformationProvider != null && objInformationProvider.Id > 0) {
            paymentConceptDelete = [];
            $("#codeProvider").val(objInformationProvider.Id);
            $("#inputObservationProvider").val(objInformationProvider.Observation);
            $("#selectProviderType").UifSelect("setSelected", objInformationProvider.ProviderTypeId);
            $("#selectProviderDeclined").UifSelect("setSelected", objInformationProvider.ProviderDeclinedTypeId);
            if (objInformationProvider.GroupSupplier != null && objInformationProvider.GroupSupplier.length > 0) {
                $.each(objInformationProvider.GroupSupplier, function (index, value) {
                    $('#selectGroupSupplier').UifMultiSelect('setSelected', value.Id);
                });
            }
            Provider.assignDate("#dateProviderCreation", objInformationProvider.CreationDate);
            Provider.assignDate("#SupplierdateModificationDate", objInformationProvider.ModificationDate);
            Provider.assignDate("#SupplierDateDeclinationDate", objInformationProvider.DeclinationDate);
            $('#checkProvider').prop('checked', true);
            Provider.loadProviderPaymentConcepts(objInformationProvider.ProviderPaymentConcept);
        }
        else {
            Provider.dataInitialSupplier();
        }
    }

    static loadProviderPaymentConcepts(PaymentConcept) {
        $("#listVPaymentConcept").UifListView({
            sourceData: PaymentConcept,
            delete: true,
            height: 300,
            displayTemplate: '#display-paymentConcepts',
            deleteCallback: Provider.deleteCallback
        })
    }

    static assignDate(tag, date) {
        if (date != null && date != "") {
            $(tag).UifDatepicker("setValue", FormatDate(date));
        }
        else {
            $(tag).UifDatepicker("clear");
        }
    }

    static getObjectSpeciality(objBasicInformationProvider) {
        var specialties = $("#tableSpecialtiesProvider").UifDataTable("getData");
        var specialtiesProvider = [];
        $.each(specialties, function (id, item) {
            if (item.Assigned) {
                specialtiesProvider.push({ SpecialityId: item.Id })
            }
            if (item.IsDefault) {
                objBasicInformationProvider.SpecialityDefault = item.Id;
            }
        })
        return specialtiesProvider;
    }


    static validateBasicInformation(objBasicInformationProvider) {
        var arrayPropertiesRequired = { IndividualId: "Codigo de persona", ProviderTypeId: "Tipo de proveedor", CreationDate: "Fecha de creación", Speciality : "Especialidad" };

        var band = true;

        $.each(arrayPropertiesRequired, function (index, value) {
            if (objBasicInformationProvider[index] == null || objBasicInformationProvider[index] == "" || objBasicInformationProvider[index] == "0" || objBasicInformationProvider[index] == undefined) {
                $.UifNotify('show', { 'type': 'danger', 'message': value + " " + AppResourcesPerson.ErrorIsRrequired, 'autoclose': true });
                band = false;
                return false;
            }
        });
        return band;
    }

    static validatePaymentConcept(objBasicInformationProvider) {
        if (objBasicInformationProvider.PaymentConcept.length > 0) {
            objBasicInformationProvider.ProviderPaymentConcepts = [];
            $.each(objBasicInformationProvider.PaymentConcept, function (index, value) {
                objBasicInformationProvider.ProviderPaymentConcepts.push(value)
            });
            //$.each(paymentConceptDelete, function (index, value) {
            //    objBasicInformationProvider.ProviderPaymentConcepts.push(value)
            //});
            return true;
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.EnterLeastTypePaymentConcept, 'autoclose': true });
        }
    }


}