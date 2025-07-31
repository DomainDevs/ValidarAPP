class ReportCexper extends Uif2.Page {

    getInitialState()
    {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }
    bindEvents()
    {
        $('#btnSearchCexper').on('click', this.ValidateSearchCexper);
        $('#btnExitSearchCexper').on('click', this.exit);
        $('#TypeDocumentFilter').on('itemSelected', function (event, selectedItem)
        {
            if (selectedItem.Id)
            {

            }
            else
            {
                $('#TablePolicies').UifDataTable({ sourceData: []});
            }
        });
        $('#TypeDocumentHistoricalSinister').on('itemSelected', function (event, selectedItem)
        {
            if (selectedItem.Id)
            {
                //
            } else
            {
                $('#TableHistoricalSinister').UifDataTable({ sourceData: [] });
            }
        }); 
    }
    exit()
    {
        window.location = rootPath + "Home/Index";
    }
    ValidateSearchCexper()
    {
        if (!$("#formSearchCexper").valid())
        {
            ReportCexper.ValidateNotifySearchCexper();
        }
        else
        {
            alert("AjaxSearch")
            ReportCexper.clearFormSearch();
        }
    }
    static ValidateNotifySearchCexper()
    {
        var error = "";
        if ($("#inputPlate").val() == "") {
            error = error + AppResources.LabelLicencesePlate + "<br>";
        }
        else if ($("#Inputcaptcha").val() == "") {
            error = error + "Captchat" + "<br>";
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }
    static clearFormSearch() {
        $("#inputPlate").val('');
        $("#TypeDocument").UifSelect();
        $("#InputDocumentNumber").val('');
        $("#Inputcaptcha").val('');
        ReportCexper.clearValidation();
    }
    static clearValidation() {
        var validator = $("#formSearchCexper").validate();
        $('[name]', "#formSearchCexper").each(function () {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present
        });
        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }
}