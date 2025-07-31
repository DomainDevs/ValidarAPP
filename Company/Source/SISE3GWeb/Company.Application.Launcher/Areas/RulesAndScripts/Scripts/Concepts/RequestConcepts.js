class RequestConcepts {
    /***
     * @summary 
     * Obtiene los conceptos asignados a la politica
     * @param {idPolicies} id de la politica
    **/
    static GetConceptDescriptionsByIdPolicies(idPolicies) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies },
            url: rootPath + "AuthorizationPolicies/Policies/GetConceptDescriptionsByIdPolicies"
        });
    }

    /***
    * @summary 
    *  Guarda los conceptos asociados a la politica
    * @param {idPolicies} id de la politica
    * @param {conceptDescriptions} lista de conceptos
    **/
    static SaveConceptDescriptions(idPolicies, conceptDescriptions) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies, "conceptDescriptions": conceptDescriptions },
            url: rootPath + "AuthorizationPolicies/Policies/SaveConceptDescriptions"
        });
    }

    /***
     * @summary 
     * btiene los conceptos segun el filtro 
    * @param {idEntity} id de la entidad
     * @param {filter} like de la descripcion
     **/
    static GetConceptsByFilter(listEntities, filter) {
        return $.ajax({
            type: "POST",
            data: { "listEntities": listEntities, "filter": filter },
            url: rootPath + "RulesAndScripts/Concepts/GetConceptsByFilter"
        });
    }

    /**
    * @summary 
    *  obtiene el comparador del del concepto para la condicion de la regla
    * @param {int} idConcept
    * id del concepto
    * @param {int} idEntity
    * id de la entidad
    **/
    static GetComparatorConcept(idConcept, idEntity) {
        return $.ajax({
            type: "POST",
            data: { "idConcept": idConcept, "idEntity": idEntity },
            url: rootPath + "RulesAndScripts/RuleSet/GetComparatorConcept"
        });
    }

    /**
    *@summary
    *Obtiene el concepto especifico con sus respectivos valores
    *@param (int) idEntity
    *id de la entidad</param>
    *@param (int) idConcept 
    *id del concepto
    *@param (int) conceptType
    *tipo de concepto
    **/
    static GetSpecificConceptWithVales(idConcept, idEntity, dependency, conceptType) {
        return $.ajax({
            type: "POST",
            data: {
                "idConcept": idConcept,
                "idEntity": idEntity,
                "dependency": dependency,
                "conceptType": conceptType
            },
            url: rootPath + "RulesAndScripts/Concepts/GetSpecificConceptWithVales"
        });
    }

    static GetConceptByIdConceptIdEntity(idConcept, idEntity) {
        return $.ajax({
            type: "POST",
            async: true,
            data: {
                "idConcept": idConcept,
                "idEntity": idEntity
            },
            url: rootPath + "RulesAndScripts/Concepts/GetConceptByIdConceptIdEntity"
        });
    }
}