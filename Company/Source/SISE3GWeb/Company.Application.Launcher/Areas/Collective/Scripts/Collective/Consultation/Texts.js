$.ajaxSetup({ async: false });
//Codigo de la pagina Texts.cshtml
$(document).ready(function () { 

    $("#btnTexts").on('click', function () {
        $('#inputTextPrecataloged').val('');
        LoadPartialText();
    });

    $("#btnTextClose").on("click", function () {
        
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.Search);
    });
});

function LoadPartialText() {
    
    
    if (policy.Id > 0) {
        $("#labelPrecataloged").hide();
        $("#inputText").attr("disabled", "disabled");
        $("#inputTextObservations").attr("disabled", "disabled");
        $($("#inputTextPrecataloged").closest('.row')).hide()
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.Texts)

        if (policy.Text != undefined) {            
            $("#inputText").val(policy.Text.TextBody);
            $("#inputTextObservations").val(policy.Text.Observations);
        }
        else {
            $("#inputText").val('');
            $("#inputTextObservations").val('');
        }
    }
}
