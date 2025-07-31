//Variables Globales
var gblindex = null;
var partnerId = null;
var partnerStatus = null;
var docTypesList = {};
var PartnerModel = {};
var CoPartnerModel = {};
var glbPartner = {};
var itemEdit = {};
var gblDocNum;
//var listPartners = [];
var glbEditPartners = [];
var glbElementEdit = null;

class PartnersParam extends Uif2.Page {
    getInitialState() {
        $("#listPartners").UifListView({
            source: null,
            customDelete: true,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: true,
            displayTemplate: "#ListPartnersTemplate",
            height: 200
        });
        PartnersParam.InitialPartners();
    }

    //Seccion Eventos
    bindEvents() {

        //Numeros
        $("#partnerDocumentNumber").on('keypress', PartnersParam.UpperCase);
        //$("#SocietyDocumentNumber").on('keypress', PartnersParam.UpperCase);
        $("#partnerParticipation").OnlyPercentage();
        //$("#SocietyPhone").on('keypress', PartnersParam.OnlyNumbers);
        //$("#partnerDocumentNumber").on('keypress', PartnersParam.OnlyNumbers);
        //$('#selectPartnerDocumentType').on('itemSelected', PartnersParam.ChangePartnerTypeAndNumberDocument);
        //Letras
        $("#namePartner").TextTransform(ValidatorType.UpperCase);

        //Funcionalidades y eventos
        $('#listPartners').on('rowEdit', PartnersParam.EditPartner);
        $('#listPartners').on('rowDelete', PartnersParam.DeletePartner);
        $('#btnPartnersSave').on('click', PartnersParam.SavePartner);
        $('#btnPartnersAdd').on('click', PartnersParam.ExecuteOperationPN);
        $('#btnPartnersNew').on('click', PartnersParam.PartnersNew);

        $('#btnFinalBeneficiary').on('click', this.FinalBeneficiarySarlaft);


    }

    static InitialPartners() {
        SarlaftRequest.GetTypeDocument().done(function (data) {
            if (data.success) {
                $("#selectPartnerDocumentType").UifSelect({ sourceData: data.result });

                docTypesList = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        //SarlaftRequest.LoadInitialLegalData(2).done(function (data) {
        //    if (data.success) {
        //        $("#selectSocietyCompanyType").UifSelect({ sourceData: data.result.CompanyTypes });
        //    }
        //    else {
        //        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        //    }
        //});

        //SarlaftRequest.GetRoles().done(function (data) {
        //    if (data.success) {
        //        $("#selectPartnerProfile").UifSelect({ sourceData: data.result });
        //    }
        //    else {
        //        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        //    }
        //});

    }

    static Validate() {

        var msj = "";

        // if ($("#partnerDocumentNumber").UifSelect("getSelected") === null || $("#partnerDocumentNumber").UifSelect("getSelected") === "") {
        //     msj = AppResources.ErrorCheckRequiredFields;
        // }

        // if ($("#namePartner").UifSelect("getSelected") === null || $("#namePartner").UifSelect("getSelected") === "") {
        //     msj = AppResources.ErrorCheckRequiredFields;
        // }

        // if ($("#selectPartnerDocumentType").UifSelect("getSelected") === null || $("#selectPartnerDocumentType").UifSelect("getSelected") === "") {
        //     msj = AppResources.ErrorCheckRequiredFields;
        // }

        // /******/

        // if ($("#selectSocietyDocumentType").UifSelect("getSelected") === null || $("#selectSocietyDocumentType").UifSelect("getSelected") === "") {
        //     msj = AppResources.ErrorCheckRequiredFields;
        // }

        // if ($("#SocietyDocumentNumber").UifSelect("getSelected") === null || $("#SocietyDocumentNumber").UifSelect("getSelected") === "") {
        //     msj = AppResources.ErrorCheckRequiredFields;
        // }
        // /*****/

        if ($("#partnerParticipation").val() == "" || parseFloat($("#partnerParticipation").val()) > 100 || $("#partnerParticipation").val() == "0") {
            $.UifNotify('show', { 'type': 'danger', 'message': "% Participación debe estar en el rando de 1 - 100", 'autoclose': true });
            return false;
        }


        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields, 'autoclose': true });
            return false;
        }
        
        return true;
    }

    static ClearPartnerFields() {
        tableFinalBeneficiary = {};
        gblindex = null;
        partnerId = null;
        partnerStatus = null;
        $('#tableParticipants').UifDataTable('clear');
        FinalBeneficiary.ClearData();
        $('#partnerDocumentNumber').val("");
        $('#namePartner').val("");
        $('#selectPartnerDocumentType').UifSelect("setSelected", null);
        $('#rdActive').prop('checked', false);
        $('#Id').val("");
        $('#Status').val("");
        $('#partnerParticipation').val("");
        $('#partnerOccupation').val("");
        $('#SocietyShareholder').val("");
        $('#NameSociety').val("");
        $('#SocietyDocumentNumber').val("");
        $('#SocietyConstitution').val("");
        $('#SocietyAdress').val("");
        $('#SocietyPhone').val("");

        // $('#selectPartnerProfile').UifSelect("setSelected", null);
        $('#selectPartnerNationality').val("");
        //$('#selectSocietyDocumentType').UifSelect("setSelected", null);
        //$('#selectSocietyCompanyType').UifSelect("setSelected", null);
        $('#partnerParticipation').val("");


    }

    static GetPartners() {
        glblValidatePartner = false;
        if (gblSarlaft.PartnerDTO == undefined || gblSarlaft.PartnerDTO.length == 0) {
            var partnerSarlaftId = 0;
            if (newSarlaftId != null) {
                partnerSarlaftId = newSarlaftId
            }
            else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
                partnerSarlaftId = gblSarlaft.SarlaftDTO.Id;
            }
            else {
                partnerSarlaftId = $('#SarlaftId').val();
            }
            SarlaftRequest.GetPartnersByIndividualId(gblIndivualId, partnerSarlaftId).done(function (data) {
                if (data.success) {
                    if (gblSarlaft.PartnerDTO != undefined) {
                        if (gblSarlaft.PartnerDTO.length > 0) {
                            $.each(gblSarlaft.PartnerDTO, function (index, value) {
                                var position = data.result.findIndex(x => x.IdCardNumero === value.IdCardNumero);
                                if (position == -1) {
                                    data.result.push(value);
                                }
                            });
                        }
                    }
                    PartnersParam.FillDataPartners(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            PartnersParam.FillDataPartners(gblSarlaft.PartnerDTO);
        }

    }

    static FillDataPartners(data) {
        delete data._id;
        glbPartner = data;
        $("#listPartners").UifListView("refresh");
        //Filtro Tipo de documento
        var entityData = docTypesList;
        var i;
        var j;
        for (i = 0; i < data.length; i++) {
            for (j = 0; j < entityData.length; j++) {
                if (parseInt(data[i].DocumentTypeId) === entityData[j].Id) {
                    data[i].DocumentTypeDescription = "";
                    data[i].DocumentTypeDescription = entityData[j].DescriptionLong;
                }
            }
        }
        var listPartners = [];
        if (gblSarlaft.PartnerDTO != undefined) {
            if (gblSarlaft.PartnerDTO.length > 0) {
                $.each(glbPartner, function (index, item) {
                    if (gblSarlaft.PartnerDTO.find(x => x.Id == item.Id) != undefined) {
                        if (gblSarlaft.PartnerDTO.find(x => x.Id == item.Id).Status != ParametrizationStatus.Delete) {
                            listPartners.push(item);
                        }
                    }
                });
            }
        }
        else {
            listPartners = glbPartner;
        }

        $.each(listPartners, function (index, item) {
            $("#listPartners").UifListView("addItem", item);
        });
    }

    static ExecuteOperationPN() {
        $('#formPartners').validate();
        if ($('#formPartners').valid()) {
            if (PartnersParam.Validate() && PartnersParam.TotalParticipation(gblindex)) {
                PartnerModel = $("#formPartners").serializeObject();
                PartnerModel.IndividualId = gblIndivualId;
                PartnerModel.Participation = parseFloat(PartnerModel.Participation.toString().replace(",", "."));
                gblDocNum = PartnerModel.IdCardNumero;
                PartnerModel.DocumentTypeDescription = $("#selectPartnerDocumentType").UifSelect("getSelectedSource").DescriptionLong;
                if ($('#selectPartnerDocumentType').val() != 1
                    && $('#selectPartnerDocumentType').val() != 3
                    && $('#selectPartnerDocumentType').val() != 4
                    && $('#selectPartnerDocumentType').val() != 6) {

                    if (PartnerModel.Id == null || PartnerModel.Id == "") {
                        gblDocNum = gblDocNum + Shared.CalculateDigitVerify(gblDocNum);
                    }
                    PartnerModel.IdCardNumero = gblDocNum;
                }
                if (PartnerModel.Status == "" || PartnerModel.Id == 0) {
                    PartnerModel.Id = 0;
                    PartnerModel.Status = ParametrizationStatus.Create;
                }
                else {
                    if (newSarlaftId == null) {
                        PartnerModel.Status = ParametrizationStatus.Update;
                    }
                    else {
                        PartnerModel.Status = ParametrizationStatus.Create;
                    }
                }
                if ($("#rdActive").is(':checked')) {
                    PartnerModel.Active = true;
                }
                else {
                    PartnerModel.Active = false;
                }

                PartnerModel.FinalBeneficiary = $('#tableParticipants').UifDataTable('getData');
                $('#tableParticipants').UifDataTable('clear');
                FinalBeneficiary.ClearData();
                //PartnersParam.RefreshPartner(PartnerModel);
                if (gblindex !== null) {
                    $("#listPartners").UifListView('editItem', gblindex, PartnerModel);
                    gblindex = null;
                }
                else {
                    $("#listPartners").UifListView('addItem', PartnerModel);
                }
                PartnersParam.ClearPartnerFields();
                PartnersParam.FinalBeneficiaryPersonNatural(null);
                partnerStatus = null;
                gblindex = null;

                var lsBeneficiaries = PartnerModel.FinalBeneficiary.filter(e => e.IdCardNumero !== PartnerModel.IdCardNumero);
                
                if (lsBeneficiaries.length == 0) {
                    $.UifNotify('show', { 'type': 'danger ', 'message': 'Algunos Accionistas / Asociados no tienen asignado los beneficiarios', 'autoclose': true });
                }
            }
        }
    }

    static FinalBeneficiaryPersonNatural(finalBeneficiary) {
        if (finalBeneficiary != null) {
            if ($('#tableParticipants').UifDataTable('getData').length == 0) {
                if (finalBeneficiary.DocumentTypeId != 2 && finalBeneficiary.DocumentTypeId != 16) {
                    $('#tableParticipants').UifDataTable('addRow', finalBeneficiary);
                }
            }
            else {
                var participants = $('#tableParticipants').UifDataTable('getData'); 
                $('#tableParticipants').UifDataTable('clear');
                participants.forEach(function (item) {
                    if (item.DocumentTypeId != 2 && item.DocumentTypeId != 16 && item.IdCardNumero == finalBeneficiary.IdCardNumero && item.DocumentTypeId == finalBeneficiary.DocumentTypeId) {
                        item.TradeName = finalBeneficiary.TradeName;
                    }
                    $('#tableParticipants').UifDataTable('addRow', item);
                });
            }
        }
        else if ($("#listPartners").UifListView('getData').length > 0) {
            $("#listPartners").UifListView('getData').forEach(function (item) {
                if (item.DocumentTypeId != 2 && item.DocumentTypeId != 16) {
                    if (item.FinalBeneficiary != null && item.FinalBeneficiary.length > 0) {
                        if (item.FinalBeneficiary.find(x => x.IdCardNumero == item.IdCardNumero && x.DocumentTypeId == item.DocumentTypeId) != undefined) {
                            item.FinalBeneficiary.find(x => x.IdCardNumero == item.IdCardNumero && x.DocumentTypeId == item.DocumentTypeId).TradeName = item.TradeName;
                        }
                    }
                    else {
                        var beneficiaryArray = [];
                        var beneficiary = { DocumentTypeId: "0", IdCardNumero: "0", TradeName: "" };
                        beneficiary.DocumentTypeId = item.DocumentTypeId;
                        beneficiary.IdCardNumero = item.IdCardNumero;
                        beneficiary.TradeName = item.TradeName;
                        beneficiaryArray.push($.extend({}, beneficiary));
                        item.FinalBeneficiary = beneficiaryArray;
                    }
                }
            });
        }
    }

    static GetCoPartner(PartnerModel) {
        PartnerModel.CoPartners = {};

        PartnerModel.CoPartners.Id = PartnerModel.Id;
        PartnerModel.CoPartners.IndividualId = PartnerModel.IndividualId;
        PartnerModel.CoPartners.IdCardNumero = $('#SocietyDocumentNumber').val();
        PartnerModel.CoPartners.DocumentTypeId = $('#selectSocietyDocumentType').UifSelect('getSelected');

        PartnerModel.CoPartners.Participation = $('#partnerParticipation').val();
        PartnerModel.CoPartners.Occupation = $('#partnerOccupation').val();
        PartnerModel.CoPartners.IdProfileCd = $('#selectPartnerProfile').UifSelect('getSelected');
        PartnerModel.CoPartners.Nationality = $('#selectPartnerNationality').val();

        PartnerModel.CoPartners.SocietyHolder = $('#SocietyShareholder').val();
        PartnerModel.CoPartners.SocietyName = $('#NameSociety').val();
        PartnerModel.CoPartners.Constitutionyear = $('#SocietyConstitution').val();

        PartnerModel.CoPartners.Address = $('#SocietyAdress').val();
        PartnerModel.CoPartners.Phone = $('#SocietyPhone').val();
        PartnerModel.CoPartners.IdCompanyTypeCd = $('#selectSocietyCompanyType').UifSelect('getSelected');

    }

    static SetCoPartner(Partner) {

        if (Partner.CoPartners != null) {
            $('#SocietyDocumentNumber').val(Partner.CoPartners.IdCardNumero);
            $('#selectSocietyDocumentType').UifSelect('setSelected', Partner.CoPartners.DocumentTypeId);

            $('#partnerParticipation').val(Partner.CoPartners.Participation);
            $('#partnerOccupation').val(Partner.CoPartners.Occupation);
            $('#selectPartnerProfile').UifSelect('setSelected', Partner.CoPartners.IdProfileCd);
            $('#selectPartnerNationality').val(Partner.CoPartners.Nationality);

            $('#SocietyShareholder').val(Partner.CoPartners.SocietyHolder);
            $('#NameSociety').val(Partner.CoPartners.SocietyName);
            $('#SocietyConstitution').val(Partner.CoPartners.Constitutionyear);

            $('#SocietyAdress').val(Partner.CoPartners.Address);
            $('#SocietyPhone').val(Partner.CoPartners.Phone);
            $('#selectSocietyCompanyType').UifSelect('setSelected', Partner.CoPartners.IdCompanyTypeCd);
        }
    }

    static SavePartner() {
        if ($("#listPartners").UifListView('getData').length > 0) {
            var listPartners = $("#listPartners").UifListView('getData');

            if ($("#partnerDocumentNumber").val() == "" && $("#selectPartnerDocumentType").val() == "" && $("#namePartner").val() == "" && $("#partnerParticipation").val() == "") {
                listPartners.forEach(function (item) {

                    if (item.FinalBeneficiary == undefined || item.FinalBeneficiary == null || item.FinalBeneficiary.length == 0) {
                        $.UifNotify('show', { 'type': 'danger ', 'message': 'Algunos Accionistas / Asociados no tienen asignado los beneficiarios', 'autoclose': true });
                    }
                });

                $.UifNotify('show', { 'type': 'success ', 'message': AppResources.SavePartnerRepresentativeSuccessfully, 'autoclose': true });
                PartnersParam.ClearPartnerFields();
                //Volver a añadir a sarlaft con los cambios
                if (gblSarlaft.PartnerDTO == undefined) {
                    gblSarlaft.PartnerDTO = [];
                }

                if (newSarlaftId != null) {
                    $.each(listPartners, function (index, item) {
                        if (item.Status != ParametrizationStatus.Create) {
                            item.Status = ParametrizationStatus.Create;
                        }
                    });
                }

                if (gblSarlaft.PartnerDTO.length > 0) {
                    if (gblSarlaft.PartnerDTO.some(x => x.Status == ParametrizationStatus.Delete)) {
                        var partnerTemporal = gblSarlaft.PartnerDTO.filter(x => x.Status != ParametrizationStatus.Delete);
                        if (partnerTemporal.length > 0) {
                            $.each(partnerTemporal, function (index, item) {
                                var position = gblSarlaft.PartnerDTO.findIndex(x => x.Id == item.Id);
                                if (position !== -1) {
                                    gblSarlaft.PartnerDTO.splice(position, 1);
                                }
                            });
                            $.each(listPartners, function (index, item) {
                                gblSarlaft.PartnerDTO.push(item);
                            });
                        }
                        else {
                            $.each(listPartners, function (index, item) {
                                gblSarlaft.PartnerDTO.push(item);
                            });
                        }
                    }
                    else {
                        gblSarlaft.PartnerDTO = listPartners;
                    }
                }
                else {
                    gblSarlaft.PartnerDTO = listPartners;
                }

                partnerStatus = null;
                $('#modalPartner').UifModal('hide');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': "Hay información pendiente por agregar", 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Debe añadir minimo un accionista/asociado", 'autoclose': true });
        }

    }

    static EditPartner(event, partner, index) {
        gblindex = index;
        $('#tableParticipants').UifDataTable('clear');
        FinalBeneficiary.ClearData();
        $('#Id').val(partner.Id);
        $('#Status').val(partner.Status);
        $('#IndividualId').val(partner.IndividualId);
        $('#partnerDocumentNumber').val(partner.IdCardNumero);
        $('#namePartner').val(partner.TradeName);
        $('#selectPartnerDocumentType').UifSelect('setSelected', partner.DocumentTypeId);
        if (partner.Active) {
            $('#rdActive').prop('checked', true);
        }
        else {
            $('#rdActive').prop('checked', false);
        }

        $('#partnerParticipation').val(partner.Participation);
        $('#tableParticipants').UifDataTable('clear');
        if (partner.FinalBeneficiary != null && partner.FinalBeneficiary != "" && partner.FinalBeneficiary.length > 0) {
            partner.FinalBeneficiary.forEach(item => {

                $('#tableParticipants').UifDataTable('addRow', item);
            });
        }
    }

    static DeletePartner(event, data) {
        $.UifDialog('confirm', {
            title: AppResources.WishContinue,
            message: AppResources.RemovePartner
        }, function (result) {
            if (result) {
                const index = $("#listPartners").UifListView("findIndex", (x) => { return x._id === data._id; });
                if (data.Id > 0) {
                    if (gblSarlaft.PartnerDTO != undefined) {
                        if (gblSarlaft.PartnerDTO.length > 0) {
                            if (gblSarlaft.PartnerDTO.find(x => x.Id == data.Id) != undefined) {
                                if (newSarlaftId == null) {
                                    gblSarlaft.PartnerDTO.find(x => x.Id == data.Id).Status = ParametrizationStatus.Delete;
                                }
                                else {
                                    var position = gblSarlaft.PartnerDTO.findIndex(x => x.Id == data.Id);
                                    if (position != -1) {
                                        gblSarlaft.PartnerDTO.splice(position, 1);
                                    }
                                }
                            }
                        }
                    }
                    //const index = $("#listPartners").UifListView("findIndex", (x) => { return x._id === data._id; });
                    $("#listPartners").UifListView("deleteItem", index);
                    //$("#listPartners").UifListView("refresh");
                }
                else {
                    if (gblSarlaft.PartnerDTO != undefined) {
                        if (gblSarlaft.PartnerDTO.length > 0) {
                            if (gblSarlaft.PartnerDTO.some(x => x._id == data._id && x.Id == 0)) {
                                var position = gblSarlaft.PartnerDTO.findIndex(x => x._id == data._id && x.Id == 0);
                                if (position != -1) {
                                    gblSarlaft.PartnerDTO.splice(position, 1);
                                }
                            }
                        }
                    }
                    $("#listPartners").UifListView("deleteItem", index);
                }
            }
            PartnersParam.ClearPartnerFields();
            //$('#modalPartner').UifModal('hide');
        });
    }

    static RefreshPartners() {
        var partnerSarlaftId = 0;
        if (newSarlaftId != null) {
            partnerSarlaftId = newSarlaftId
        }
        else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
            partnerSarlaftId = gblSarlaft.SarlaftDTO.Id;
        }
        SarlaftRequest.GetPartnersByIndividualId(gblIndivualId, partnerSarlaftId).done(function (data) {
            if (data.success) {
                //Filtro Tipo de documento
                var entityData = docTypesList;
                var i;
                var j;
                for (i = 0; i < data.result.length; i++) {
                    for (j = 0; j < entityData.length; j++) {
                        if (data.result[i].DocumentTypeId == entityData[j].Id) {
                            data.result[i].DocumentTypeDescription = "";
                            data.result[i].DocumentTypeDescription = entityData[j].DescriptionLong;
                        }
                    }
                }
                $.each(data.result, function (index, item) {
                    item.index = index;
                    $("#listPartners").UifListView("addItem", item);
                });
                if (data.result !== null) {
                    gblSarlaft.PartnerDTO = data.result;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static DisabledFinalBeneficiary() {
        $("#selectDocumentTypeFb").prop("disabled", true);
        $("#DocumentNumberFb").prop("disabled", true);
        $("#nameFinalBeneficiary").prop("disabled", true);
        $("#btnFinalBeneficiaryAdd").prop("disabled", true);
        $("#btnFinalBeneficiarySave").prop("disabled", true);
    }

    static DisabledPartners() {
        $("#partnerDocumentNumber").prop("disabled", true);
        $('#namePartner').prop("disabled", true);
        $('#rdActive').prop("disabled", true);
        $('#selectPartnerDocumentType').UifSelect("disabled", true);

        $("#listPartners").UifListView({
            customDelete: false,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: false,
            displayTemplate: "#ListPartnersTemplate",
            height: 200
        });

        $("#btnPartnersSave").hide();

        $('#partnerParticipation').prop("disabled", true);
        $('#partnerOccupation').prop("disabled", true);
        $('#selectPartnerNationality').prop("disabled", true);
        //$('#selectPartnerProfile').UifSelect("disabled", true);

        $('#SocietyShareholder').prop("disabled", true);
        $('#NameSociety').prop("disabled", true);
        $('#SocietyDocumentNumber').prop("disabled", true);
        $('#SocietyConstitution').prop("disabled", true);
        $('#SocietyAdress').prop("disabled", true);
        $('#SocietyPhone').prop("disabled", true);

        //$('#selectSocietyDocumentType').UifSelect("disabled", true);
        //$('#selectSocietyCompanyType').UifSelect("disabled", true);

        $("#btnPartnersNew").prop("disabled", true);
        $("#btnPartnersAdd").prop("disabled", true);
        PartnersParam.DisabledFinalBeneficiary();
    }

    static EnabledFinalBeneficiary() {
        $("#selectDocumentTypeFb").prop("disabled", false);
        $("#DocumentNumberFb").prop("disabled", false);
        $("#nameFinalBeneficiary").prop("disabled", false);
        $("#btnFinalBeneficiaryAdd").prop("disabled", false);
        $("#btnFinalBeneficiarySave").prop("disabled", false);
    }

    static EnabledPartners() {
        $("#partnerDocumentNumber").prop("disabled", false);
        $('#namePartner').prop("disabled", false);
        $('#rdActive').prop("disabled", false);
        $('#selectPartnerDocumentType').UifSelect("disabled", false);

        $("#listPartners").UifListView({
            customDelete: true,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: true,
            displayTemplate: "#ListPartnersTemplate",
            height: 200
        });

        $("#btnPartnersSave").show();

        $('#partnerParticipation').prop("disabled", false);
        //$('#partnerOccupation').prop("disabled", false);
        //$('#selectPartnerNationality').prop("disabled", false);
        //$('#selectPartnerProfile').UifSelect("disabled", false);

        //$('#SocietyShareholder').prop("disabled", false);
        //$('#NameSociety').prop("disabled", false);
        $('#SocietyDocumentNumber').prop("disabled", false);
        $('#SocietyConstitution').prop("disabled", false);
        $('#SocietyAdress').prop("disabled", false);
        $('#SocietyPhone').prop("disabled", false);

        //$('#selectSocietyDocumentType').UifSelect("disabled", false);
        //$('#selectSocietyCompanyType').UifSelect("disabled", false);

        $("#btnPartnersNew").prop("disabled", false);
        $("#btnPartnersAdd").prop("disabled", false);

        PartnersParam.EnabledFinalBeneficiary();
    }

    static OnlyNumbers(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    static PartnersNew() {

        PartnersParam.ClearPartnerFields();

    }

    static PartnersAdd() {

        $('#formPartners').validate();

        if (PartnersParam.Validate()) {


        }
    }

    FinalBeneficiarySarlaft() {
        var partner = $("#formPartners").serializeObject();
        if (partner.IdCardNumero != "" && partner.DocumentTypeId != "") {
            var beneficiary = { DocumentTypeId: "0", IdCardNumero: "0", TradeName: "" };
            beneficiary.DocumentTypeId = partner.DocumentTypeId;
            beneficiary.IdCardNumero = partner.IdCardNumero;
            beneficiary.TradeName = partner.TradeName;
            PartnersParam.FinalBeneficiaryPersonNatural($.extend({}, beneficiary));
        }
        
        $('#modalFinalBeneficiary').UifModal('showLocal', Resources.Language.LabelFinalBeneficiary);
        $('#tableParticipants tbody td').find('.div-button-single').children('.select-button').hide();
    }

    static RefreshPartner(PartnerModel) {
        if (PartnerModel.Id != "undefined" && PartnerModel.Id != null && PartnerModel.Id != "") {
            //glbDeletedPartners.push(PartnerModel);
            //PartnersParam.SavePartner(PartnerModel);
            const index = $("#listPartners").UifListView("findIndex", (x) => { return x._id === PartnerModel.Id; });
            $("#listPartners").UifListView("deleteItem", index);
        }
    }

    static TotalParticipation(index) {
        var partnerModel = $("#formPartners").serializeObject();
        var listPartnres = $("#listPartners").UifListView('getData');
        var participation = parseFloat(partnerModel.Participation.toString().replace(",", "."));
        var FlagParticipation = true;

        if (partnerModel.Id != "" && partnerModel.Status != "") {
            if (index != -1) {
                listPartnres.splice(index, 1);
            }
        }

        if (listPartnres != null && listPartnres.length > 0) {
            $.each(listPartnres, function (index, item) {
                participation += (parseFloat(item.Participation.toString().replace(",", ".")));
                if (participation > 100) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMajorPercentage, 'autoclose': true });
                    FlagParticipation = false;
                    return false;
                }
            });
        }

        return FlagParticipation;
    }

    static ChangePartnerTypeAndNumberDocument() {
        if ($("#selectPartnerDocumentType").UifSelect("getSelectedSource").IsAlphanumeric) {
            $('#partnerDocumentNumber').val("");
            $('#partnerDocumentNumber').ValidatorKey(ValidatorType.lettersandnumbersSpecial, 0, 0);
        } else {
            $('#partnerDocumentNumber').val("");
            var regex = /^\d+$/;
            if (!regex.exec($('#partnerDocumentNumber').val())) {
                $('#partnerDocumentNumber').val("");
            }
            $("#partnerDocumentNumber").ValidatorKey(ValidatorType.lettersandnumbersSpecial, 0, 0);
        }
    }
}