
class FinancialPlanParametrization extends Uif2.Page {
    bindEvents() {
        $("#btnExit").click(this.redirectIndex)
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }
}