$(() => {
    new Partner();
});

var partnerIndex = null;
var heightPartnerListView = 256;
class Partner extends Uif2.Page {
    getInitialState() {
        this.InitializePartner();
    }
    
    //Seccion Eventos
    bindEvents() {
        $("#InputDocumentNumberPartner").focusout(this.DocumentNumberPartner);
        $('#btnnewPartner').click(Partner.clearControlPartner);
        $('#btncreatePartner').click(this.BtncreatePartner);
        $("#btnAcceptPartner").click(this.AcceptPartner);
        $('#listPartner').on('rowEdit', this.PartnerEdit);
    }

    InitializePartner() {
        $("#InputDocumentNumberPartner").ValidatorKey();
        $("#InputNamePartner").ValidatorKey(ValidatorType.lettersandnumbersAccent);
    }

    DocumentNumberPartner() {
        if($.trim($("#InputDocumentNumberPartner").val()) != "")
        {
            Partner.searchParner();
        }
    }

    static clearControlPartner() {
        $("#InputDocumentNumberPartner").val("");
        $("#InputNamePartner").val("");
        $("#selectDocumentPartner").UifSelect("setSelected", null);
        $("#_active").prop("checked", true);
        Partner.DisabledPartner(false);
        partnerIndex = null;
        if (defaultValues != null) {
            Persons.SetDefaultValues(defaultValues);
        }
        else {
            Persons.GetDefaultValues();
        }
    }

    BtncreatePartner() {
        if (Partner.ValidateControls()) {
            Partner.CreatePartner();
            Partner.clearControlPartner();
        }
    }

 
    AcceptPartner() {
        Partner.SavePartners()
        Partner.clearControlPartner();
    }

    PartnerEdit(event, data, index) {
        
        Partner.clearControlPartner();
        Partner.EditPartner(data, index);
        Partner.DisabledPartner(true);
    }

    static CreatePartner() {
        $("#fronPartner").validate();
        if ($("#fronPartner").valid()) {
            var partner = Partner.CreatePartnerModel();
            if (partnerIndex == null) {
                var list = $("#listPartner").UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.IdentificationDocumentNumber.toUpperCase() && item.DocumentTypeId == partner.IdentificationDocumentNumber.toUpperCase() && partner.DocumentTypeId;
                });

                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.MessagePartner, 'autoclose': true });
                }
                else
                {
                    partner.statusTypeService = ParametrizationStatus.Create;
                    $("#listPartner").UifListView("addItem", partner);
                }
            }else {
                    partner.statusTypeService = ParametrizationStatus.Update;
                $("#listPartner").UifListView("editItem", partnerIndex, partner);
                }
                Partner.clearControlPartner();
            }
        }

    static CreatePartnerModel() {

        var partner = {
            IndividualId: individualId,
            PartnerId: $("#inputPartnerId").val(),
            TradeName: $('#InputNamePartner').val(),
            IdentificationDocumentNumber : $('#InputDocumentNumberPartner').val(),
            DocumentTypeId: $('#selectDocumentPartner').UifSelect('getSelected')
            
        };
        if ($("#_active").is(':checked')) {
            partner.Active = true;
        }
        else {
            partner.Active = false;
        }
        if (partner.PartnerId > 0) {
            partner.statusTypeService = ParametrizationStatus.Update;
        } else {
            partner.statusTypeService = ParametrizationStatus.Create;
        }
        return partner;
    }


    //Seccion Load
    static LoadPartners(dta) {
        $("#listPartner").UifListView({ sourceData: dta, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#partnerTemplate", height: heightPartnerListView });
        
    }

    static searchParner() {
        PartnerRequest.GetAplicationPartnerByDocumentIdDocumentTypeIndividualId($("#InputDocumentNumberPartner").val(), $("#selectDocumentPartner").val(), individualId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#InputNamePartner').val(data.result["TradeName"]);
                }
                else {
                    $('#InputNamePartner').val("");
                }
            }
        });
    }

    //seccion grabado
    static SavePartners() {
        var Partners = null;
        if ($("#listPartner").UifListView('getData').length > 0) {
            Partners = $("#listPartner").UifListView('getData');    
            PartnerRequest.CreateAplicationPartner(Partners);
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.SuccessfulCreatedPartner, 'autoclose': true });
        }
        
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.ErrorEmpty, 'autoclose': false })
        }

    }

    //seccion edit
    static EditPartner(data, index) {
        partnerIndex = index;
        $('#InputDocumentNumberPartner').val(data.IdentificationDocumentNumber)
        $('#InputNamePartner').val(data.TradeName);
        $('#inputPartnerId').val(data.PartnerId);
        $('#selectDocumentPartner').UifSelect('setSelected', data.DocumentTypeId);
        if (data.Active) {
            $('#_active').prop('checked', true);
        }
        else {
            $('#_active').prop('checked', false);
        }
    }

    static DisabledPartner(enabled) {
        $("#InputDocumentNumberPartner").prop("disabled", enabled);
        $("#selectDocumentPartner").prop("disabled", enabled);
    }

    static CleanObjectPartner() {
        $("#listPartner").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#partnerTemplate", height: heightPartnerListView });
    }

    //seccion Validaciones
    static ValidateControls() {
        var msj = "";
        if ($("#InputDocumentNumberPartner").val() == "") {
            msj = AppResourcesPerson.LabelNumberDocument + "<br>"
        }
        if ($("#selectDocumentPartner").val() == "") {
            msj = msj + AppResourcesPerson.LabelDocument + "<br>"
        }
        if ($("#InputNamePartner").val() == "") {
            msj = msj + AppResourcesPerson.LabelName + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true });
            return false;
        } else
            if (Partner.DuplicatePartner($("#InputDocumentNumberPartner").val(), $("#selectDocumentPartner").val())) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessagePartner, 'autoclose': true });
                return false;
            }
        return true;

    }

    static DuplicatePartner(documentPartner, typeDocument) {
        var duplicate = false;
        $.each($("#listPartner").UifListView('getData'), function (i, value) {
            if (this.DocumentTypeId == typeDocument && this.IdentificationDocumentNumber == documentPartner && partnerIndex != i) {
                duplicate = true;
                return false;
            }
        });
        return duplicate;
    }
}














