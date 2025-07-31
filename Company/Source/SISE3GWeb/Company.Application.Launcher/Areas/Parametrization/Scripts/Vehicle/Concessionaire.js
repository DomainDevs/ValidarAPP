
class Concessionaire extends Uif2.Page {

    /**
    * Funcion para inicilizar la vista
    */
    getInitialState() {

    }

    bindEvents() {
        $('#table').on('rowAdd', function (event) {
            $('#modalDialogConcessionaire').UifModal('showLocal', Resources.Language.Add);
        });
    }

}