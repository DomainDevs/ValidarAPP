using Sistran.Core.Application.RulesScriptsServices.Models;
//using MUti= Sistran.Core.Application.UtilitiesServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Application.RulesScriptsServices
{

    [ServiceContract]
    public interface IRulesService
    {
        #region RuleSet

        /// <summary>
        /// Obtiene los paquetes de reglas por el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id los niveles a buscar</param>
        /// <param name="withDecisionTable"></param>
        /// <param name="isPolicie">es una politica</param>
        /// <param name="filter">like para la descripcion</param>        
        /// <returns></returns>
        [OperationContract]
        List<_RuleSet> GetRulesByFilter(int idPackage, List<int> levels, bool withDecisionTable, bool isPolicie, string filter, bool maxRow);

        /// <summary>
        /// Obtiene los paquetes de reglas para la busqueda avanzada
        /// </summary>
        /// <param name="ruleSet">filtro de regla</param>        
        /// <returns>lista de paquetes de reglas</returns>
        [OperationContract]
        List<MRules._RuleSet> GetRulesByRuleSet(MRules._RuleSet ruleSet);

        /// <summary>
        /// obtiene los paquetes de reglas que son DT
        /// </summary>
        /// <param name="idPackage">id del paquete</param>

        [OperationContract]
        List<MRules._RuleSet> GetRulesDecisionTable(int idPackage);

        /// <summary>
        /// Obtiene el paquete de regla completo, con sus respectivas reglas del xml
        /// </summary>
        /// <param name="idRuleSet">id de la regla</param>
        /// <param name="deserializeXml">saber si se deserializa el XML</param>
        /// <returns></returns>
        [OperationContract]
        MRules._RuleSet GetRuleSetByIdRuleSet(int idRuleSet, bool deserializeXml);

        /// <summary>
        /// obtiene el comparador del del concepto para la condicion de la regla
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        List<MRules._Comparator> GetComparatorConcept(int idConcept, int idEntity);

        /// <summary>
        /// obtiene los tipos de comparadores para la condicion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MRules._ComparatorType> GetConditionComparatorType();

        /// <summary>
        /// obtiene los tipos de comparadores para la condicion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MRules._ComparatorType> GetActionComparatorType();

        /// <summary>
        /// Valida la expresion matematica, y la setea de forma correcta
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string ValidateExpression(string expression);

        /// <summary>
        /// Obtine los tipos de acciones para la regla
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MRules._ActionType> GetActionType();

        /// <summary>
        /// Obtine los tipos de invocaciones para la accion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MRules._InvokeType> GetInvokeType();

        /// <summary>
        /// Obtine los tipos de operadores aritmeticos para la accion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MRules._ArithmeticOperatorType> GetArithmeticOperatorType();


        [OperationContract]
        Task<MRules._RuleSet> ImportRuleSet(MRules._RuleSet ruleSet);

        /// <summary>
        ///  Realiza el guardado del paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a guardar</param>
        /// <returns></returns>
        [OperationContract]
        Task<MRules._RuleSet> _CreateRuleSet(MRules._RuleSet ruleSet);

        /// <summary>
        ///  Realiza la modificacion del paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a modificar</param>
        /// <returns></returns>
        [OperationContract]
        Task<MRules._RuleSet> UpdateRuleSet(MRules._RuleSet ruleSet);


        [OperationContract]
        void _DeleteRuleSet(int ruleSetId);

        /// <summary>
        /// Exportar listado de reglas
        /// </summary>
        /// <returns>excel de reglas</returns>        
        [OperationContract]
        string GenerateFileToRules();

        /// <summary>
        /// Genera archivo txt con las reglas con errores
        /// </summary>
        [OperationContract]
        void GetRulesWithExceptions();
        #endregion

        #region Function
        /// <summary>
        /// Obtiene las funciones de reglas que concuerden con la busqueda
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id de los niveles</param>
        /// <returns></returns>
        [OperationContract]
        List<MRules._RuleFunction> GetRuleFunctionsByIdPackageLevels(int idPackage, List<int> levels);

        #endregion

        #region Package
        /// <summary>
        /// Obtiene los paquetes habilitados
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MRules._Package> _GetPackages();
        #endregion


        #region Levels

        /// <summary>
        /// Obtiene los niveles por el paquete
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <returns></returns>
        [OperationContract]
        List<MRules._Level> GetLevelsByIdPackage(int idPackage);

        #endregion


        #region Position

        /// <summary>
        /// Obtiene las entities de la tabla positionEntity por paquete y nivel
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Entity> GetEntitiesByPackageIdLevelId(int packageId, int levelId);
        #endregion

        #region Decision table

        /// <summary>
        /// Obtiene el listado de tablas de decision por el filtro indicado
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">lista de los niveles</param>
        /// <param name="isPolicie">si es politica</param>
        /// <param name="filter">filtro like de la descripcion</param>
        /// <returns></returns>
        [OperationContract]
        List<_RuleBase> GetDecisionTableByFilter(int? idPackage, List<int> levels, bool isPolicie, string filter, int tableId, System.DateTime? dateFrom, bool? isPublished);

        /// <summary>
        /// elimina la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        [OperationContract]
        void _DeleteTableDecision(int ruleBaseId);

        /// <summary>
        /// obtiene la cabecera de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        [OperationContract]
        List<_Concept> GetConceptsConditionByRuleBaseId(int ruleBaseId);

        /// <summary>
        /// obtiene la cabecera de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        [OperationContract]
        List<_Concept> GetConceptsActionByRuleBaseId(int ruleBaseId);

        /// <summary>
        /// obtiene la Data de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <returns></returns>
        [OperationContract]
        List<MRules._Rule> GetTableDecisionBody(int ruleBaseId);

        /// <summary>
        /// Realiza la el guardado de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns>Modelo RuleBase creado</returns>
        [OperationContract]
        MRules._RuleBase CreateTableDecisionHead(_RuleBase ruleBase, List<_Concept> conceptCondition, List<_Concept> conceptAction);


        /// <summary>
        /// Realiza la actualizacion de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns></returns>
        [OperationContract]
        void UpdateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition, List<MRules._Concept> conceptAction);


        /// <summary>
        /// Realiza el guardado de la DT
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <param name="rulesAdd">Reglas para agregar</param>
        /// <param name="rulesEdit">reglas para editar</param>
        /// <param name="rulesDelete">Reglas para eliminar</param>
        /// <returns></returns>
        [OperationContract]
        void SaveDecisionTable(int ruleBaseId, List<MRules._Rule> rulesAdd, List<MRules._Rule> rulesEdit,
            List<MRules._Rule> rulesDelete);

        /// <summary>
        /// valida y publica una TD
        /// </summary>
        /// <param name="ruleBaseId">id dT a publicar</param>
        /// <param name="isEvent"></param>
        /// <returns></returns>
        [OperationContract]
        Task<List<int>> PublishDecisionTable(int ruleBaseId, bool isEvent);

        /// <summary>
        /// Realiza el mapeo y el guardado de la tabla de decision del excel
        /// </summary>
        /// <param name="pathXml"></param>
        /// <param name="pathXls"></param>
        /// <param name="save"></param>
        /// <param name="isEvent"></param>
        [OperationContract]
        Task<_DecisionTableMappingResult> _LoadDecisionTableFromFile(string pathXml, string pathXls, bool save, bool isEvent);

        /// <summary>
        /// Exporta archivo excel de la tabla de desicion 
        /// </summary>
        /// <param name="ruleBaseId">id dT a exportar</param>        
        /// <returns>ruta del archivo para descargar</returns>
        [OperationContract]
        string ExportDecisionTable(int ruleBaseId);

        /// <summary>
        /// Exporta archivo excel de la tabla de desicion 
        /// </summary>
        /// <param name="ruleBaseId">id dT a exportar</param>        
        /// <returns>ruta del archivo para descargar</returns>
        [OperationContract]
        string ExportDecisionTables();

        /// <summary>
        /// Exporta tabla de decisiones con error en data
        /// </summary>
        /// <param name="exceptions">excepciones a exportar</param>
        /// <returns>url a descargar archivo</returns>
        [OperationContract]
        string GenerateFileToDecisionTableByExceptions(List<string[]> exceptions);
        #endregion


        /***********************************/
        //ANTIGUO
        /***********************************/
        #region Antiguo

        /// <summary>
        /// Obtener los Modulos
        /// </summary>
        /// <returns>Retornas un Listados de Package</returns>
        [OperationContract]
        List<Package> GetPackages();

        /// <summary>
        /// Obtener todos los Nivels por modulos
        /// </summary>
        /// <param name="packageId">Id del Modulo</param>
        /// <returns>Retorna un Listado de Level</returns>
        [OperationContract]
        List<Level> GetLevels(int packageId);

        /// <summary>
        /// Obtiene Todas los Paquetes de Reglas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RuleSet> GetAllRuleSets(bool IsEvent);

        /// <summary>
        /// Obtiene Todas los Paquetes de Reglas Por Modulo, Nivel y Producto
        /// </summary>
        /// <param name="packageId">Id del Modulo</param>
        /// <param name="levelId">Id del Nivel</param>
        /// <param name="productId">Id del Producto</param>
        /// <returns></returns>
        [OperationContract]
        List<RuleSet> GetRuleSetByPackageIdLevelIdProductId(int? packageId, int? levelId, int? productId);

        /// <summary>
        /// Obtiene los conceptos
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        Concept GetConcept(int conceptId, int entityId);

        /// <summary>
        /// obtiene todas las acciones 
        /// </summary>
        /// <param name="ruleSetId">id del paquete de reglas</param>
        /// <param name="ruleId">id de la regla</param>
        /// <returns></returns>
        [OperationContract]
        List<Action> GetActions(int ruleSetId, int ruleId);

        /// <summary>
        /// obtiene los nombre de las reglas
        /// </summary>
        /// <param name="ruleSetId">id del paquete de reglas</param>
        /// <returns></returns>
        [OperationContract]
        List<RuleComposite> GetRuleNames(int ruleSetId);

        /// <summary>
        /// Obneter los datos del SearchCombo
        /// </summary>
        /// <param name="entityId">Id de la entidad</param>
        /// <param name="filter">Datos adicionales de filtrado de la condicion</param>
        /// <returns></returns>
        [OperationContract]
        string GetDataFromFilter(int entityId, List<ConditionFilter> filter);

        /// <summary>
        /// Obtener los Comparadores
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Comparator> GetComparators();

        /// <summary>
        /// Obtener las propiedades de Filtrado
        /// </summary>
        /// <param name="entityId">Id de la Entidad</param>
        /// <returns></returns>
        [OperationContract]
        List<PropertyFilter> GetPropertyFilter(int entityId);

        /// <summary>
        /// Obtiene todas las condiciones 
        /// </summary>
        /// <param name="ruleSetId">Id del paquete de reglas</param>
        /// <param name="ruleId">id de la regla</param>
        /// <returns></returns>
        [OperationContract]
        List<Condition> GetConditions(int ruleSetId, int ruleId);

        /// <summary>
        /// Comparardor del Concepto
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        List<ComparatorConcept> GetConceptComparator(int conceptId, int entityId);

        /// <summary>
        /// Obtencion del control del concepto
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        ConceptControl GetConceptControl(int conceptId, int entityId);

        /// <summary>
        /// Obtencion de los conceptos dinamicos
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        object GetDynamicConcept(int conceptId, int entityId);

        /// <summary>
        /// Obtiene los tipos de codigo de la accion 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ActionTypeCode> GetActionTypeCollection();

        /// <summary>
        /// Obtiene los tipos de funciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ListAction> GetFunctionTypes();

        /// <summary>
        /// Obtiene los tipos de operadores
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        List<Operator> GetOperationTypes(int conceptId, int entityId);

        /// <summary>
        /// Obtiene las funsiones de las reglas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RuleFunction> GetRuleFunctions();

        /// <summary>
        /// crear el paquete de regalas con sus reglas acciones y condiciones
        /// </summary>
        /// <param name="ruleSetComposite"></param>
        /// <returns></returns>
        [OperationContract]
        bool CreateRuleSet(RuleSetComposite ruleSetComposite);

        /// <summary>
        /// Obtiene el listado de las tablas de decision 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RuleBase> GetDecisionTableList();

        /// <summary>
        /// Obtiene las condiciones de los conceptos
        /// </summary>
        /// <param name="id">id de la regla</param>
        /// <returns></returns>
        [OperationContract]
        List<Concept> GetConditionConcept(int id);

        /// <summary>
        /// obtiene las acciones de los conceptos
        /// </summary>
        /// <param name="id">id de la regla</param>
        /// <returns></returns>
        [OperationContract]
        List<Concept> GetActionConcept(int id);

        /// <summary>
        /// obtiene el paquete de regalas con sus reglas acciones y condiciones 
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        [OperationContract]
        RuleSetComposite GetRuleSetComposite(int ruleSetId);

        /// <summary>
        /// obtiene los datos de la tabla de decision con sus reglas acciones y condiciones
        /// </summary>
        /// <param name="ruleBaseId">id de la tabla de decision</param>
        /// <returns></returns>
        [OperationContract]
        DecisionTableComposite GetDecisionTableComposite(int ruleBaseId);

        /// <summary>
        /// valida la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la tabla de decision</param>
        /// <returns></returns>
        [OperationContract]
        void ValidateDecisionTableComposite(int ruleBaseId);

        /// <summary>
        /// actualiza la regla
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <returns></returns>
        [OperationContract]
        bool PostDecisionTable(int ruleBaseId);

        /// <summary>
        /// crea los datos de la tabla de decision 
        /// </summary>
        /// <param name="RuleBase">la parte de la parametirzacion de la tabla de decision </param>
        /// <param name="Condiciones">las condiciones de la tabla de decision</param>
        /// <param name="Acciones">las acciones de la tabla de decision</param>
        /// <returns></returns>
        [OperationContract]
        bool CreateTableDecision(RuleBase RuleBase, List<Concept> Condiciones, List<Concept> Acciones);


        /// <summary>
        /// crea los datos de la tabla de decision las filas con sus acciones y condiciones
        /// </summary>
        /// <param name="DecisionTableComposite"></param>
        /// <returns></returns>
        [OperationContract]
        bool CreateTableDecisionRow(DecisionTableComposite RulesAdd, DecisionTableComposite RulesEdit, DecisionTableComposite RulesDelete);

        /// <summary>
        /// elimina la tabla de decision
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteTableDecision(int Id);

        /// <summary>
        /// Obtener lista de valores por entidad
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns>Lista de valores</returns>
        [OperationContract]
        List<ListEntityValue> GetListEntityValuesByConceptIdEntityId(int conceptId, int entityId);

        /// <summary>
        /// Gets the value types.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ValueType> GetValueTypes();

        /// <summary>
        /// Gets the concepts by level identifier.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<RuleSet> GetConceptsByLevelId(int levelId);

        /// <summary>
        /// Obtener reglas por nivel
        /// </summary>
        /// <param name="level">Nivel</param>
        /// <returns></returns>
        [OperationContract]
        List<RuleSet> GetRuleSetByLevels(List<Level> level);

        /// <summary>
        /// Obtener paquetes de reglas por nivel
        /// </summary>
        /// <param name="levelId">nivel por id</param>
        /// <returns></returns>
        [OperationContract]
        List<RuleSet> GetRuleSetDTOsByLevelId(int levelId, bool IsEvent);

        /// <summary>
        /// Obtener paquetes de reglas por nivel
        /// </summary>
        /// <param name="levelId">nivel por id</param>
        /// <returns></returns>
        [OperationContract]
        int[] GetLevelsEntity(Enums.Level levels);

        /// <summary>
        /// Realiza el mapeo y el guardado de la tabla de decision del excel
        /// </summary>
        /// <param name="pathXml"></param>
        /// <param name="pathXls"></param>
        [OperationContract]
        DecisionTableMappingResult LoadDecisionTableFromFile(string pathXml, string pathXls, bool save);

        /// <summary>
        /// Obtiene la reglas a ejecutar para un proceso especifico
        /// </summary>
        /// <param name="processType"></param>
        [OperationContract]
        RuleProcessRuleSet GetRulestByRulsetProcessType(int processType);

        [OperationContract]
        void DeleteRuleSet(int ruleSetId);

        [OperationContract]
        List<RuleSet> GetRuleSetByIds(List<int> ids);

        [OperationContract]
        _RuleSet GetRuleSetById(int ruleSetId);
        #endregion
        #region precarga
        [OperationContract]
        void LoadScripts();
        #endregion pre
    }
}
