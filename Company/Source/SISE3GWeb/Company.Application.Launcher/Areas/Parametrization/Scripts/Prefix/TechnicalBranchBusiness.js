
$(() => {
    new ParametrizationTechnicalBranchBusiness();
});


class ParametrizationTechnicalBranchBusiness extends Uif2.Page {
      
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
       
    }

    bindEvents() {
        $('#btnExit').click(this.Exit);

    }
   
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    
}