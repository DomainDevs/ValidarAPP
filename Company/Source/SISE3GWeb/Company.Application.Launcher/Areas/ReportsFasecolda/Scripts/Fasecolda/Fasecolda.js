class ReportFasecolda extends Uif2.Page {

    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //$('#ModalPolicyInformation').UifModal('showLocal', "Detalle de Póliza");
        //$('#ModalSinisterInformation').UifModal('showLocal', "Detalle Siniestro");
    }
    bindEvents() {
        $('#btnSearchSisa').on('click', this.ValidateSearchSisa);
        $('#btnExitSearchSisa').on('click', this.exit);
        $('#TablePolicies').UifDataTable();
        $('#TableSimit').UifDataTable();
        $('#TableClaims').UifDataTable();
        $('#TableValueGuide ').UifDataTable();
        $('#TableVINSAE').UifDataTable();
    }
    exit() {
        window.location = rootPath + "Home/Index";
    }
    ValidateSearchSisa() {
        if (!$("#formSearchSisa").valid()) {
            ReportFasecolda.ValidateNotifySearchSisa();
        } else {
            alert("AjaxSearch")
            ReportFasecolda.clearFormSearch();
            ReportFasecolda.TabEnabledAll();
        }
    }




    static ValidateNotifySearchSisa() {
        var error = "";
        if ($("#inputPlate").val() === "") {
            error = error + AppResources.LabelLicencesePlate + "<br>";
        }
        else if ($("#Inputcaptcha").val() === "") {
            error = error + "Captchat" + "<br>";
        }
        if (error !== "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }
    static clearFormSearch() {
        $("#inputPlate").val('');
        $("#inputEngine").val('');
        $("#inputChassis").val('');
        $("#Inputcaptcha").val('');
        ReportFasecolda.clearValidation();
    }
    static clearValidation() {
        var validator = $("#formSearchSisa").validate();
        $('[name]', "#formSearchSisa").each(function () {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present
        });
        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }
    static TabDisabledAll(){
        $('#tabs').UifTabHeader("disabled", null, true);
    }
    static TabEnabledAll() {
        $('#tabs').UifTabHeader("disabled", null, false);
    }
    static RemoveActiveAllTabs() {
        $("li.active").removeClass("active");
        $("div.tab-pane").removeClass("active");
        
    }
}