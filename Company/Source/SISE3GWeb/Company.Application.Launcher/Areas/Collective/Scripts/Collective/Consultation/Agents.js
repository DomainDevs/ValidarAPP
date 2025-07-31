$.ajaxSetup({ async: false });
$(document).ready(function () {
    $("#btnAgents").on('click', function () {
        if (policy.Id > 0) {
           // HidePanelsSearch();
            //$("#modalAgentsSearch #row1").hide()
            //$("#modalAgentsSearch #row2").hide()
            //$("#modalAgentsSearch #row3").hide()
            // $(".uif-panel-pc").UifPanel('setTitle', 'Detalle de comisiones');
            LoadPartialAgents();
        }
        //LoadAgencyPrincipal(policy.Agencies);
    });

    $("#btnAgentsClose").on("click", function () {
        
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.Search);
    });
});

//Metodos
function LoadPartialAgents() {
    $('#formAgents').formReset();
    ShowPanelsSearch(MenuType.Agents);
    HideControls();
    LoadAgencies(policy.Agencies);
}

function HideControls() {
    $('#AddAgent').hide();
    $('#listAgent').removeClass('uif-col-6');
    $('#listAgent').addClass('uif-col-12');
    $('#btnAgentsCancel').hide();
    $('#btnAgentsAccept').hide();
    $("#btnsAgent").hide();
    $("#summaryAgent").removeClass('uif-col-6')
    $('#summaryAgent').addClass('uif-col-12');
    $("#summaryAgent").css("margin-left","2px");
}


function LoadAgencies(agencies) {
    $('#listAgencies').UifListView({ displayTemplate: '#agencyTemplate', add: false, edit: false, delete: false, customEdit: true, customDelete: true, height: 250 });
    var totalParticipation = 0;

    if (agencies != null) {
        $.each(agencies, function (index, value) {
            var totalAmount = 0;
            totalParticipation += parseFloat(this.Participation);
            this.Participation = FormatMoney(this.Participation);
            $.each(this.Commissions, function (index, value) {
                this.Percentage = FormatMoney(this.Percentage);
                this.PercentageAdditional = FormatMoney(this.PercentageAdditional);
                this.CalculateBase = FormatMoney(this.CalculateBase);
                totalAmount += parseFloat(this.Amount);
                this.Amount = FormatMoney(this.Amount);
            });
            this.TotalPercentage = this.Commissions[0].Percentage;
            this.TotalAmount = FormatMoney(totalAmount);
            $('#listAgencies').UifListView('addItem', this);
        });

        
        $('#labelAgentsTotalParticipation').text(FormatMoney(totalParticipation));
    }
}
