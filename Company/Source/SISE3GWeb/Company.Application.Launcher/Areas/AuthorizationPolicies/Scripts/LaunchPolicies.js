class LaunchPolicies extends Uif2.Page {
    getInitialState() { }

    bindEvents() { }

    /**
     *@summary 
    * Completa los datos del modal para la solicitud de politicas
    * @param  {listPolicies} lista de las politicas
     * @param {key} llave de que identifica de quien es la politica
    */
    static RenderViewAuthorizationPolicies(listPolicies, key, functionType) {
        $("#hdnKey").val(key);
        $("#hdnFunctionType").val(functionType);

        let restrictive = listPolicies.filter(item => {
            return item.Type === TypeAuthorizationPolicies.Restrictive;
        });

        if (restrictive.length === 0) {
            let athorization = listPolicies.filter(item => {
                return item.Type === TypeAuthorizationPolicies.Authorization;
            });

            SummaryAuthorization.SetListPolicies(athorization);
        } else {
            let restriciveMessage = "";
            let tmpListPolicies = [];
            
            for (let i = 0; i < restrictive.length; i++) {
                let tmpPolicie = restrictive.filter((item) => {
                    return item.IdPolicies === restrictive[i].IdPolicies;
                });
                tmpPolicie[0].Count = tmpPolicie.length;

                if (tmpListPolicies.indexOf(tmpPolicie[0]) === -1) {
                    tmpListPolicies.push(tmpPolicie[0]);
                    restriciveMessage += `*${tmpPolicie[0].Message} (${tmpPolicie[0].Count})\n`;
                }
            }

            $.UifDialog("alert", { 'message': restriciveMessage, 'title': "Politicas restrictivas" });
        }
    }

    /**
    * @summary 
     * Valida las politicas, y lanza los respectivos mensajes
     * segun el tipo de politica
    * @param  {listPolicies} lista de las politicas
    */
    static ValidateInfringementPolicies(listPolicies, showMessage) {
        let tmpListPolicies = [];
        if (listPolicies != null) {
            let hasRestrictive = false;
            let restriciveMessage = "";
            for (let i = 0; i < listPolicies.length; i++) {
                let tmpPolicie = listPolicies.filter((item) => {
                    return item.IdPolicies === listPolicies[i].IdPolicies && item.Message === listPolicies[i].Message;
                });
                tmpPolicie[0].Count = tmpPolicie.length;

                if (tmpListPolicies.indexOf(tmpPolicie[0]) === -1) {
                    tmpListPolicies.push(tmpPolicie[0]);
                    if (tmpPolicie[0].Type === TypeAuthorizationPolicies.Restrictive) {
                        hasRestrictive = true;
                        restriciveMessage += `*${tmpPolicie[0].Message} (${tmpPolicie[0].Count})\n`;
                    }
                    else if (tmpPolicie[0].Type === TypeAuthorizationPolicies.Notification || tmpPolicie[0].Type === TypeAuthorizationPolicies.Authorization) {
                        if (showMessage !== false && hasRestrictive === false) {
                            if (tmpPolicie[0].Count > 1) {
                                $.UifNotify("show",
                                    {
                                        'type': "warning",
                                        'message': tmpPolicie[0].Message + " (" + tmpPolicie[0].Count + ")",
                                        'autoclose': true
                                    });
                            } else {
                                $.UifNotify("show",
                                    {
                                        'type': "warning",
                                        'message': tmpPolicie[0].Message,
                                        'autoclose': true
                                    });
                            }
                        }
                    }
                }
            }

            if (hasRestrictive == true) {
                $.UifDialog("alert", { 'message': restriciveMessage, 'title': "Politicas restrictivas", "class": "modal-sm modal-restritive" });
                return TypeAuthorizationPolicies.Restrictive;
            }

            if (listPolicies.length > 0)
                return TypeAuthorizationPolicies.Notification;
            else
                return TypeAuthorizationPolicies.Nothing;
        }
    }
}


(() => { new LaunchPolicies(); })();