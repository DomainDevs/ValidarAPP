var CommercialBouquetIndex = null;
//var vPrefixUser = {};
//$.ajaxSetup({ async: true });
var prefix;
var vPrefixUser;

class PrefixUserRequest {
    /**
  * @summary 
  * peticion ajax que Obtiene las jerarquias
  **/
    static GetPrefixUser() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/UniqueUser/GetUniqueUserPrefixUsersByUserId',
            data: JSON.stringify({ UserId: glbUser.UserId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class PrefixUser extends Uif2.Page {
    static deleteCallback(deferred, data) {
        //deferred.resolve();
    }
    getInitialState() {

    }
    bindEvents() {
        $('#btnPrefixUser').on('click', this.saveAndLoad);
        $('#btnPrefixUserSave').on('click', PrefixUser.SavePrefixUserBto);
    }

    saveAndLoad() {
       
            modalSave = true;
           
            if (glbUser != null) {
                if (glbUser.UserId == 0) {
                    $('#Password').attr("data-val", "false");
                    $("#formUser").validate();
                    if (($("#formUser").valid())) {
                        UniqueUser.showPanelsUser(MenuType.PrefixUser);
                        if (glbUser.Prefixes == undefined) {
                            PrefixUser.loadPartialPrefixUser(true);
                        }
                        
                    }                   

                }
                else {
                    UniqueUser.showPanelsUser(MenuType.PrefixUser);
                    if (glbUser.Prefixes == undefined) {
                        PrefixUser.loadPartialPrefixUser(true);
                    }
                }
            }
       

    }

    static loadPartialPrefixUser(showPanel) {
       
        PrefixUserRequest.GetPrefixUser().done(function (data) {
            prefix = data.result[0];
            vPrefixUser = data.result[1];
            if (data.success) {
                $('#tablePrefixUser').UifDataTable('clear');
                if (data.result[0].length > 0) {
                    $('#tablePrefixUser').UifDataTable('addRow', prefix);
                }

                $.each(prefix, function (id, item) {
                    
                    $.each(vPrefixUser, function (id2, item2) {
                        if (item.Id == item2.Id) {                            
                            $('#tablePrefixUser tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                            $('#tablePrefixUser tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                    });                 
                });

                if (prefix.length > 0) {
                    PrefixUser.SavePrefixUser(showPanel);
                }



                //$("#tablePrefixUser").find("button").attr("disabled", "disabled")
            }
            else {
                $('#tablePrefixUser').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchClauses, 'autoclose': true })
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchClauses, 'autoclose': true });
        });

    }

    static SavePrefixUserBto() {
        PrefixUser.SavePrefixUser(false);
    }

    static SavePrefixUser(showPanel) {
        var dtPrefixUser = $("#tablePrefixUser").UifDataTable('getSelected');
        if (dtPrefixUser != null) {
            glbUser.Prefixes = [];
            $.each(dtPrefixUser, function (index, value) {
                dtPrefixUser = {
                    Id: value.Id,
                    Description: value.Description,
                }
                glbUser.Prefixes.push(dtPrefixUser);
            });
            if (showPanel == false) {
                UniqueUser.hidePanelsUser(MenuType.PrefixUser);
            }
        }
       
              
    }


}



