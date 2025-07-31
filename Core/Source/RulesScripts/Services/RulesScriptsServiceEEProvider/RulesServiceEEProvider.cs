using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Helper;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using RulesModels = Sistran.Core.Application.RulesScriptsServices.Models;
using SCREN = Sistran.Core.Application.Script.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    using Sistran.Core.Application.Utilities.DataFacade;
    using System.Threading.Tasks;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class RulesServiceEEProvider : IRulesService
    {
        #region RuleSet
        /// <summary>
        /// Exportar listado de reglas
        /// </summary>
        /// <returns>excel de reglas</returns>       
        public string GenerateFileToRules()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                List<MRules._RuleSet> ruleSets = ruleSetDao.GetRuleSets();
                if (ruleSets.Count > 0)
                {
                    FileDAO fileDAO = new FileDAO();
                    return fileDAO.GenerateFileToRules(ruleSets);
                }
                return "";
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GenerateFileToRules", e);
            }
        }
        /// <summary>
        /// Genera archivo txt con las reglas con errores
        /// </summary>
        public void GetRulesWithExceptions()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                ruleSetDao.GetRulesWithExceptions();
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRulesWithErrors", e);
            }
        }

        /// <summary>
        /// Obtiene los paquetes de reglas por el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id los niveles a buscar</param>
        /// <param name="withDecisionTable">incluir tablas de decision</param>
        /// <param name="isPolicie">es una politica</param>
        /// <param name="filter">like para la descripcion</param>        
        /// <returns></returns>        
        public List<MRules._RuleSet> GetRulesByFilter(int idPackage, List<int> levels, bool withDecisionTable, bool isPolicie, string filter, bool maxRows)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetRulesByFilter(idPackage, levels, withDecisionTable, isPolicie, filter, maxRows);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRulesByFilter", e);
            }
        }

        /// <summary>
        /// Obtiene los paquetes de reglas para la busqueda avanzada
        /// </summary>
        /// <param name="ruleSet">filtro de regla</param>        
        /// <returns>lista de paquetes de reglas</returns>
        public List<MRules._RuleSet> GetRulesByRuleSet(MRules._RuleSet ruleSet)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetRulesByRuleSet(ruleSet);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRulesByRuleSet", e);
            }
        }

        /// <summary>
        /// obtiene los paquetes de reglas que son DT
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        public List<MRules._RuleSet> GetRulesDecisionTable(int idPackage)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetRulesDecisionTable(idPackage);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRulesDecisionTable", e);
            }
        }

        /// <summary>
        /// Obtiene el paquete de regla completo, con sus respectivas reglas del xml
        /// </summary>
        /// <param name="idRuleSet">id de la regla</param>
        /// <param name="deserializeXml">saber si se deserializa el XML</param>
        /// <returns></returns>
        public MRules._RuleSet GetRuleSetByIdRuleSet(int idRuleSet, bool deserializeXml)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetRuleSetByIdRuleSet(idRuleSet, deserializeXml);
            }
            catch (AggregateException e)
            {
                string message = e.Message;
                e.InnerExceptions.ToList().ForEach(b => message = message + b.Message);
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// obtiene el comparador del del concepto para la condicion de la regla
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public List<MRules._Comparator> GetComparatorConcept(int idConcept, int idEntity)
        {
            try
            {
                _ConceptDao conceptDao = new _ConceptDao();
                return conceptDao.GetComparatorConcept(idConcept, idEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetComparatorConcept", ex);
            }

        }

        /// <summary>
        /// obtiene los tipos de comparadores para la condicion
        /// </summary>
        /// <returns></returns>
        public List<MRules._ComparatorType> GetConditionComparatorType()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetConditionComparatorType();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConditionComparatorType", ex);
            }
        }

        /// <summary>
        /// obtiene los tipos de comparadores para la accion
        /// </summary>
        /// <returns></returns>
        public List<MRules._ComparatorType> GetActionComparatorType()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetActionComparatorType();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetActionComparatorType", ex);
            }
        }

        /// <summary>
        /// Valida la expresion matematica, y la setea de forma correcta
        /// </summary>
        /// <returns></returns>
        public string ValidateExpression(string expression)
        {
            try
            {
                ExpressionParserHelper parserHelper = new ExpressionParserHelper();
                return parserHelper.ValidateExpression(expression);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en ValidateExpression", e);
            }
        }


        /// <summary>
        /// Obtine los tipos de acciones para la regla
        /// </summary>
        /// <returns></returns>
        public List<MRules._ActionType> GetActionType()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetActionType();
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetActionType", e);
            }
        }

        /// <summary>
        /// Obtine los tipos de invocaciones para la accion
        /// </summary>
        /// <returns></returns>
        public List<MRules._InvokeType> GetInvokeType()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetInvokeType();
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetInvokeType", e);
            }
        }


        /// <summary>
        /// Obtine los tipos de operadores aritmeticos para la accion
        /// </summary>
        /// <returns></returns>
        public List<MRules._ArithmeticOperatorType> GetArithmeticOperatorType()
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.GetArithmeticOperatorType();
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetInvokeType", e);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a crear</param>
        /// <returns></returns>
        public async Task<MRules._RuleSet> ImportRuleSet(MRules._RuleSet ruleSet)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return await ruleSetDao.ImportRuleSet(ruleSet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }

        /// <summary>
        ///  Realiza la creacion de un paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a crear</param>
        /// <returns></returns>
        public async Task<MRules._RuleSet> _CreateRuleSet(MRules._RuleSet ruleSet)
        {
            try
            {
                XmlHelperWriter xmlHelperWriter = new XmlHelperWriter();

                _RuleSetDao ruleSetDao = new _RuleSetDao();
                return ruleSetDao.CreateRuleSet(ruleSet, await xmlHelperWriter.GetXmlByRuleSet(ruleSet));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        ///  Realiza la modificacion del paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a modificar</param>
        /// <returns></returns>
        public async Task<MRules._RuleSet> UpdateRuleSet(MRules._RuleSet ruleSet)
        {
            try
            {
                XmlHelperWriter xmlHelperWriter = new XmlHelperWriter();
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                if (ruleSet.Rules == null)
                {
                    return ruleSetDao.UpdateRuleSet(ruleSet, null);
                }
                return ruleSetDao.UpdateRuleSet(ruleSet, await xmlHelperWriter.GetXmlByRuleSet(ruleSet));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSetId"></param>
        public void _DeleteRuleSet(int ruleSetId)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                ruleSetDao.DeleteRuleSet(ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en DeleteRuleSet", ex);
            }
        }

        #endregion

        #region Function

        /// <summary>
        /// Obtiene las funciones de reglas que concuerden con la busqueda
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id de los niveles</param>
        /// <returns></returns>
        public List<MRules._RuleFunction> GetRuleFunctionsByIdPackageLevels(int idPackage, List<int> levels)
        {
            try
            {
                _RuleFunctionDao functionDao = new _RuleFunctionDao();
                return functionDao.GetRuleFunctionsByIdPackageLevels(idPackage, levels);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRuleFunctionsByIdPackageLevels", e);
            }
        }


        #endregion

        #region Package
        /// <summary>
        /// Obtiene los paquetes habilitados
        /// </summary>
        /// <returns></returns>
        public List<MRules._Package> _GetPackages()
        {
            try
            {
                _PackageDao packageDao = new _PackageDao();
                return packageDao.GetPackages();
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetPackages", e);
            }
        }


        #endregion

        #region Levels

        /// <summary>
        /// Obtiene los niveles por el paquete
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <returns></returns>
        public List<MRules._Level> GetLevelsByIdPackage(int idPackage)
        {
            try
            {
                _LevelDao levelDao = new _LevelDao();
                return levelDao.GetLevelsByIdPackage(idPackage);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetLevelsByIdPackage", e);
            }
        }


        #endregion

        #region Position
        public List<Entity> GetEntitiesByPackageIdLevelId(int packageId, int levelId)
        {
            try
            {
                _PositionEntityDao positionEntityDao = new _PositionEntityDao();
                return positionEntityDao.GetEntitiesByPackageIdLevelId(packageId, levelId);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetLevelsByIdPackage", e);
            }
        }
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
        public List<MRules._RuleBase> GetDecisionTableByFilter(int? idPackage, List<int> levels, bool isPolicie, string filter, int tableId, DateTime? dateFrom, bool? isPublished)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                return ruleBaseDao.GetDecisionTableByFilter(idPackage, levels, isPolicie, filter, tableId, dateFrom, isPublished);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en obtener listado de tablas de decisión", e);
            }
        }

        /// <summary>
        /// elimina la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        public void _DeleteTableDecision(int ruleBaseId)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                ruleBaseDao.DeleteTableDecision(ruleBaseId);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en DeleteTableDecision", e);
            }
        }

        /// <summary>
        /// obtiene la cabecera de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        public List<MRules._Concept> GetConceptsConditionByRuleBaseId(int ruleBaseId)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                return ruleBaseDao.GetConceptsConditionByRuleBaseId(ruleBaseId);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetConceptsConditionByRuleBaseId", e);
            }
        }

        /// <summary>
        /// obtiene la cabecera de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        public List<MRules._Concept> GetConceptsActionByRuleBaseId(int ruleBaseId)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                return ruleBaseDao.GetConceptsActionByRuleBaseId(ruleBaseId);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetConceptsActionByRuleBaseId", e);
            }
        }

        /// <summary>
        /// obtiene la Data de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <returns></returns>
        public List<MRules._Rule> GetTableDecisionBody(int ruleBaseId)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                return ruleBaseDao.GetTableDecisionBody(ruleBaseId);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en GetTableDecisionBody", e);
            }
        }

        /// <summary>
        /// Realiza la el guardado de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns>Modelo RuleBase creado</returns>
        public MRules._RuleBase CreateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition, List<MRules._Concept> conceptAction)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                return ruleBaseDao.CreateTableDecisionHead(ruleBase, conceptCondition, conceptAction);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Realiza la actualizacion de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns></returns>
        public void UpdateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition, List<MRules._Concept> conceptAction)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                ruleBaseDao.UpdateTableDecisionHead(ruleBase, conceptCondition, conceptAction);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Realiza el guardado de la DT
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <param name="rulesAdd">Reglas para agregar</param>
        /// <param name="rulesEdit">reglas para editar</param>
        /// <param name="rulesDelete">Reglas para eliminar</param>
        /// <returns></returns>
        public void SaveDecisionTable(int ruleBaseId, List<MRules._Rule> rulesAdd, List<MRules._Rule> rulesEdit, List<MRules._Rule> rulesDelete)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                ruleBaseDao.SaveDecisionTable(ruleBaseId, rulesAdd, rulesEdit, rulesDelete);
            }
            catch (Exception e)
            {
                throw new BusinessException("error en SaveDecisionTable", e);
            }
        }

        /// <summary>
        /// valida y publica una TD
        /// </summary>
        /// <param name="ruleBaseId">id dT a publicar</param>
        /// <param name="isEvent">si la Dt es un evento</param>
        /// <returns></returns>
        public async Task<List<int>> PublishDecisionTable(int ruleBaseId, bool isEvent)
        {
            try
            {
                _RuleBaseDao ruleBaseDao = new _RuleBaseDao();
                return await ruleBaseDao.PublishDecisionTable(ruleBaseId, isEvent);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error publicando una tabla de decisión", e);
            }
        }

        /// <summary>
        /// Realiza el mapeo y el guardado de la tabla de decision del excel
        /// </summary>
        /// <param name="pathXml"></param>
        /// <param name="pathXls"></param>
        /// <param name="save"></param>
        /// <param name="isEvent"></param>
        public async Task<_DecisionTableMappingResult> _LoadDecisionTableFromFile(string pathXml, string pathXls, bool save, bool isEvent)
        {
            try
            {
                _DecisionTableLoader decisionTableLoader = new _DecisionTableLoader(pathXml, pathXls);

                if (decisionTableLoader.ReadMappingFile())
                {
                    decisionTableLoader.CreateDataSet();
                    _DecisionTableMappingResult resultReadExcelFile = decisionTableLoader.ReadExcelFile(); //Genera un archivo excel en caso de haber error
                    if (!String.IsNullOrEmpty(resultReadExcelFile?.ErrorMessage))
                    {
                        return new _DecisionTableMappingResult() { ErrorMessage = resultReadExcelFile.ErrorMessage };
                    }
                    else if (!String.IsNullOrEmpty(resultReadExcelFile?.FileExceptions))
                    {
                        return new _DecisionTableMappingResult { FileExceptions = resultReadExcelFile.FileExceptions };
                    }
                    else if (save)
                    {
                        var result = await TP.Task.Run(() => decisionTableLoader.CreateRuleBase(isEvent));
                        decisionTableLoader.DeleteFiles();
                        return result;
                    }
                }
                return new _DecisionTableMappingResult { DataSet = decisionTableLoader.GetDataSet() };
            }
            catch (Exception ex)
            {
                return new _DecisionTableMappingResult { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Exporta archivo excel de la tabla de desicion 
        /// </summary>
        /// <param name="ruleBaseId">id dT a exportar</param>        
        /// <returns>ruta del archivo para descargar</returns>
        public string ExportDecisionTable(int ruleBaseId)
        {
            try
            {
                _FileDao fileDao = new _FileDao();
                return fileDao.ExportDecisionTable(ruleBaseId);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error al exportar " + e.Message);
            }
        }

        /// <summary>
        /// Exporta archivo excel de la tabla de desicion 
        /// </summary>
        /// <returns>ruta del archivo para descargar</returns>
        public string ExportDecisionTables()
        {
            try
            {
                _FileDao fileDao = new _FileDao();
                return fileDao.ExportDecisionTables();
            }
            catch (Exception e)
            {
                throw new BusinessException("error en ExportDecisionTable", e);
            }
        }

        /// <summary>
        /// Exporta tabla de decisiones con error en data
        /// </summary>
        /// <param name="exceptions">excepciones a exportar</param>
        /// <returns>url a descargar archivo</returns>
        public string GenerateFileToDecisionTableByExceptions(List<string[]> exceptions)
        {
            try
            {
                FileDAO fileDao = new FileDAO();
                return fileDao.GenerateFileToDecisionTableByExceptions(exceptions);
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GenerateFileToDecisionTableByExceptions", ex);
            }
        }
        #endregion


        /***********************************/
        //ANTIGUO
        /***********************************/
        #region Antiguo
        public static string EQUAL = "Igual al";
        public static string EQUAL_SMALLDESC = "=";
        public static string GREATER = "Mayor que el";
        public static string GREATER_OR_EQUAL = "Mayor o igual al";
        public static string GREATER_OR_EQUAL_SMALLDESC = ">=";
        public static string GREATER_SMALLDESC = ">";
        public static string IS_DEFINED = "Esta definido";
        public static string IS_NOT_DEFINED = "No esta definido";
        public static string IS_NOT_NULL = "No nulo";
        public static string IS_NULL = "Nulo";
        public static string LBL_ADD = "Sumarle";
        public static string LBL_ASSIGN = "Asignarle";
        public static string LBL_ASSIGNACTION = "Al concepto";
        public static string LBL_COCNEPTVALUE = "Concepto";
        public static string LBL_CONCEPTVALUE = "Concepto";
        public static string LBL_CONSTANTVALUE = " Valor";
        public static string LBL_DIVIDE = " Dividirlo por";
        public static string LBL_INVOKE = " Invocar";
        public static string LBL_INVOKEACTION = "la acción";
        public static string LBL_INVOKEFUNCTION = "la función";
        public static string LBL_INVOKEMESSAGE = "el mensaje";
        public static string LBL_INVOKERULESET = "paquete de reglas";
        public static string LBL_IS = "ES";
        public static string LBL_IS_INDISTINCT = "es INDISTINTO             ";
        public static string LBL_MULTIPLY = "Multiplicarlo por";
        public static string LBL_OF = "de";
        public static string LBL_ROUND = "Redondear";
        public static string LBL_SUBSTRACT = "Restarle";
        public static string LBL_TEMPORARY_STORE = "Al valor temporal";
        public static string LBL_TEMPORARYASSIGNACTION = "Al valor temporal ";
        public static string LBL_TEMPORARYASSIGNACTION1 = "Al valor temporal";
        public static string LBL_TEMPORARYVALUE = "Valor temporal";
        public static string LBL_THE = "el";
        public static string LBL_THEN = "Entonces";
        public static string LBL_THERULE = "la Regla";
        public static string LESS = "Menor que el";
        public static string LESS_OR_EQUAL = "Menor o igual al";
        public static string LESS_OR_EQUAL_SMALLDESC = "<=";
        public static string LESS_SMALLDESC = "<";
        public static string NOT_EQUAL = "Distinto que el";


        Dictionary<PrimaryKey, SCREN.Concept> concepts;

        public RulesServiceEEProvider()
        {
            concepts = new Dictionary<PrimaryKey, SCREN.Concept>();
        }

        /// <summary>
        /// Obtener los Modulos
        /// </summary>
        /// <returns>Retornas un Listados de Package</returns>
        public List<RulesModels.Package> GetPackages()
        {
            try
            {
                BusinessCollection packageList = PackageDAO.ListPackage(null, null);
                return ModelAssembler.CreatePackages(packageList);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetPackages", ex);
            }

        }

        /// <summary>
        /// Obtener todos los Nivels por modulos
        /// </summary>
        /// <param name="packageId">Id del Modulo</param>
        /// <returns>Retorna un Listado de Level</returns>
        public List<RulesModels.Level> GetLevels(int packageId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Entities.Level.Properties.PackageId, packageId);

                BusinessCollection levelList = LevelDAO.ListLevel(filter.GetPredicate(), new[] { Entities.Level.Properties.LevelId });

                return ModelAssembler.CreateLevels(levelList);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetLevels", ex);
            }

        }

        /// <summary>
        /// Obtiene Todas los Paquetes de Reglas
        /// </summary>
        /// <returns></returns>
        public List<RulesModels.RuleSet> GetAllRuleSets(bool IsEvent)
        {
            try
            {
                return RuleSetDAO.GetRuleSets(IsEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetAllRuleSets", ex);
            }
        }

        /// <summary>
        /// obtiene todas las reglas a partie del levelID
        /// </summary>
        /// <param name="LevelId">levelID</param>
        /// <returns></returns>
        public List<RulesModels.RuleSet> GetAllRuleSetsByLevelId(int LevelId)
        {
            try
            {
                return RuleSetDAO.GetRuleSets(false).Where(x => x.LevelId == LevelId).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetAllRuleSetsByLevelId", ex);
            }
        }

        /// <summary>
        /// Obtiene Todas los Paquetes de Reglas Por Modulo, Nivel y Producto
        /// </summary>
        /// <param name="packageId">Id del Modulo</param>
        /// <param name="levelId">Id del Nivel</param>
        /// <param name="productId">Id del Producto</param>
        /// <returns></returns>
        public List<RulesModels.RuleSet> GetRuleSetByPackageIdLevelIdProductId(int? packageId, int? levelId, int? productId)
        {
            try
            {
                return RuleSetDAO.GetRuleSetByPackageIdLevelIdProductId(packageId, levelId, productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetByPackageIdLevelIdProductId", ex);
            }

        }

        /// <summary>
        /// Obtiene los conceptos
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        public RulesModels.Concept GetConcept(int conceptId, int entityId)
        {
            try
            {
                SCREN.Concept concept;

                if (!concepts.TryGetValue(SCREN.Concept.CreatePrimaryKey(conceptId, entityId), out concept))
                {
                    concept = ConceptDAO.GetConceptByConceptIdEntityId(conceptId, entityId);
                    concepts.Add(concept.PrimaryKey, concept);
                }

                return ModelAssembler.CreateConcept(concept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConcept", ex);
            }

        }

        /// <summary>
        /// obtiene todas las acciones 
        /// </summary>
        /// <param name="ruleSetId">id del paquete de reglas</param>
        /// <param name="ruleId">id de la regla</param>
        /// <returns></returns>
        public List<RulesModels.Action> GetActions(int ruleSetId, int ruleId)
        {
            try
            {
                return ActionDAO.GetActions(ruleSetId, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetActions", ex);
            }

        }

        /// <summary>
        /// Obtiene todas las condiciones 
        /// </summary>
        /// <param name="ruleSetId">Id del paquete de reglas</param>
        /// <param name="ruleId">id de la regla</param>
        /// <returns></returns>
        public List<Condition> GetConditions(int ruleSetId, int ruleId)
        {
            try
            {
                return ConditionDAO.GetConditions(ruleSetId, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConditions", ex);
            }

        }

        /// <summary>
        /// obtiene los nombre de las reglas
        /// </summary>
        /// <param name="ruleSetId">id del paquete de reglas</param>
        /// <returns></returns>
        public List<RulesModels.RuleComposite> GetRuleNames(int ruleSetId)
        {
            try
            {
                return RuleSetDAO.GetRuleNames(ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleNames", ex);
            }

        }

        /// <summary>
        /// obtiene el paquete de regalas con sus reglas acciones y condiciones 
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public RuleSetComposite GetRuleSetComposite(int ruleSetId)
        {
            try
            {
                RuleSetComposite ruleSetComposite = new RuleSetComposite();

                RuleEditorData ruleEditorData = RuleSetDAO.FillRuleEditorData(ruleSetId);
                ruleSetComposite.RuleComposites = new List<RuleComposite>();

                int j = 0;
                foreach (RuleDTO r in ruleEditorData.Rules)
                {
                    RuleComposite ruleComposite = new RuleComposite { RuleId = j, RuleName = r.Name };
                    j++;

                    // Condiciones
                    ruleComposite.Conditions = new List<Condition>();

                    int i = 0;
                    foreach (ConditionDTO conditionDTO in r.Conditions)
                    {
                        Condition condition = this.FillCondition(conditionDTO);
                        condition.Id = i;
                        i++;

                        ruleComposite.Conditions.Add(condition);
                    }

                    // Acciones
                    ruleComposite.Actions = new List<RulesModels.Action>();

                    int k = 0;
                    foreach (ActionDTO actionDTO in r.Actions)
                    {
                        RulesModels.Action action = this.FillAction(actionDTO);
                        action.Id = k;
                        k++;
                        ruleComposite.Actions.Add(action);
                    }

                    ruleSetComposite.RuleComposites.Add(ruleComposite);
                }

                return ruleSetComposite;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetComposite", ex);
            }

        }

        /// <summary>
        /// obtiene la condicion 
        /// </summary>
        /// <param name="conditionDTO"></param>
        /// <returns></returns>
        private Condition FillCondition(ConditionDTO conditionDTO)
        {
            try
            {
                return ConditionDAO.FillCondition(conditionDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FillCondition", ex);
            }

        }

        /// <summary>
        /// obtiene la accion
        /// </summary>
        /// <param name="actionDTO"></param>
        /// <returns></returns>
        private RulesModels.Action FillAction(ActionDTO actionDTO)
        {
            try
            {
                return ActionDAO.FillAction(actionDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FillAction", ex);
            }

        }

        /// <summary>
        /// Obneter los datos del SearchCombo
        /// </summary>
        /// <param name="entityId">Id de la entidad</param>
        /// <param name="filter">Datos adicionales de filtrado de la condicion</param>
        /// <returns></returns>
        public string GetDataFromFilter(int entityId, List<ConditionFilter> filter)
        {
            try
            {
                return RuleSetDAO.GetDataFromFilter(entityId, filter);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDataFromFilter", ex);
            }

        }

        /// <summary>
        /// Obtener los Comparadores
        /// </summary>
        /// <returns></returns>
        public List<Comparator> GetComparators()
        {
            try
            {
                // validar los comparadores para los tipos de controles
                //texto igual y diferente
                return ModelAssembler.CreateComparators(RuleConditionComparatorDAO.GetComparators(null, null));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetComparators", ex);
            }

        }

        /// <summary>
        /// Obtener las propiedades de Filtrado
        /// </summary>
        /// <param name="entityId">Id de la Entidad</param>
        /// <returns></returns>
        public List<PropertyFilter> GetPropertyFilter(int entityId)
        {
            try
            {
                return RuleSetDAO.GetPropertyFilter(entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetPropertyFilter", ex);
            }
        }

        /// <summary>
        /// Comparardor del Concepto
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        public List<ComparatorConcept> GetConceptComparator(int conceptId, int entityId)
        {
            try
            {
                return ConceptDAO.GetConceptComparator(conceptId, entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptComparator", ex);
            }

        }

        /// <summary>
        /// Obtencion del control del concepto
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        public RulesModels.ConceptControl GetConceptControl(int conceptId, int entityId)
        {
            try
            {
                return RuleSetDAO.GetConceptControl(conceptId, entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptControl", ex);
            }

        }

        /// <summary>
        /// Obtencion de los conceptos dinamicos
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        public object GetDynamicConcept(int conceptId, int entityId)
        {
            try
            {
                return RuleSetDAO.GetDynamicConcept(conceptId, entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDynamicConcept", ex);
            }

        }

        /// <summary>
        /// Obtiene los tipos de codigo de la accion 
        /// </summary>
        /// <returns></returns>
        public List<ActionTypeCode> GetActionTypeCollection()
        {
            try
            {
                List<ActionTypeCode> data = new List<RulesModels.ActionTypeCode> {
                new ActionTypeCode { Code = 1, Descripcion = "Al Concepto" },
                new ActionTypeCode { Code = 2, Descripcion = "Invocar" },
                new ActionTypeCode { Code = 3, Descripcion = "Al Valor Temporal" }
            };

                return data;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetActionTypeCollection", ex);
            }

        }

        /// <summary>
        /// Obtiene los tipos de funciones
        /// </summary>
        /// <returns></returns>
        public List<ListAction> GetFunctionTypes()
        {
            try
            {
                List<ListAction> data = new List<RulesModels.ListAction> {
                new ListAction  { Code = 1, Descripcion = "Mensaje" },
                new ListAction  { Code = 2, Descripcion = "Paquete de Reglas" },
                new ListAction  { Code = 3, Descripcion = "La Funcion" }
                };

                return data;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetFunctionTypes", ex);
            }

        }

        /// <summary>
        /// Obtiene los tipos de operadores
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns></returns>
        public List<RulesModels.Operator> GetOperationTypes(int conceptId, int entityId)
        {
            try
            {
                return RuleSetDAO.GetOperationTypes(conceptId, entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetOperationTypes", ex);
            }

        }

        /// <summary>
        /// Gets the value types.
        /// </summary>
        /// <returns></returns>
        public List<RulesModels.ValueType> GetValueTypes()
        {
            try
            {
                List<RulesModels.ValueType> data = new List<RulesModels.ValueType> {
                new RulesModels.ValueType { Code = 1, Description = "Valor" },
                new RulesModels.ValueType { Code = 2, Description = "Concepto" },
                new RulesModels.ValueType { Code = 3, Description = "Resultado de la Epxpresion" },
                new RulesModels.ValueType { Code = 4, Description = "Valor Temporal" }
            };

                return data;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetValueTypes", ex);
            }

        }

        /// <summary>
        /// crear el paquete de regalas con sus reglas acciones y condiciones
        /// </summary>
        /// <param name="ruleSetComposite"></param>
        /// <returns></returns>
        public bool CreateRuleSet(RulesModels.RuleSetComposite ruleSetComposite)
        {
            try
            {
                return RuleSetDAO.CreateRuleSet(ruleSetComposite);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleSet", ex);
            }

        }

        /// <summary>
        /// Obtiene las funsiones de las reglas
        /// </summary>
        /// <returns></returns>
        public List<RulesModels.RuleFunction> GetRuleFunctions()
        {
            try
            {
                IList ruleFunctionList = RuleFunctionDAO.ListRuleFunction(null, null);
                return RuleFunctionDAO.ConvertToRuleFunctionsModel(ruleFunctionList);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleFunctions", ex);
            }
        }

        /// <summary>
        /// Obtiene el listado de las tablas de decision 
        /// </summary>
        /// <returns></returns>
        public List<RulesModels.RuleBase> GetDecisionTableList()
        {
            try
            {
                return DecisionTableDAO.GetDecisionTableList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDecisionTableList", ex);
            }

        }

        /// <summary>
        /// Obtiene las condiciones de los conceptos
        /// </summary>
        /// <param name="id">id de la regla</param>
        /// <returns></returns>
        public List<RulesModels.Concept> GetConditionConcept(int ruleBaseId)
        {
            try
            {
                return DecisionTableDAO.GetConditionConcept(ruleBaseId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConditionConcept", ex);
            }

        }

        /// <summary>
        /// obtiene las acciones de los conceptos
        /// </summary>
        /// <param name="id">id de la regla</param>
        /// <returns></returns>
        public List<RulesModels.Concept> GetActionConcept(int ruleBaseId)
        {
            try
            {
                return DecisionTableDAO.GetActionConcept(ruleBaseId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetActionConcept", ex);
            }

        }

        /// <summary>
        /// valida la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la tabla de decision</param>
        /// <returns></returns>         
        public void ValidateDecisionTableComposite(int ruleBaseId)
        {
            try
            {
                #region obtener los conceptos Condiciones y su orden
                List<RulesModels.Concept> conceptsCondition = RuleConditionDAO.GetConditionConcept(ruleBaseId);

                string[] conceptConditionControls = new string[conceptsCondition.Count];
                for (int i = 0; i < conceptsCondition.Count; i++)//1-2-3-4
                {
                    RulesModels.ConceptControl conceptControl = GetConceptControl(conceptsCondition[i].ConceptId, conceptsCondition[i].EntityId);

                    switch (conceptControl.ConceptControlCode)
                    {
                        case 4://lista                        
                            conceptConditionControls[i] = JsonConvert.SerializeObject(((RulesModels.ListBoxControl)conceptControl).ListListEntityValues);
                            break;
                        case 5://refetencia
                            conceptConditionControls[i] = GetDataFromFilter(((RulesModels.SearchComboControl)conceptControl).ForeignEntity, null);
                            break;
                    }
                }
                #endregion                

                #region experimento 1                    
                //List<Entities.RuleCondition> ruleConditionList = RuleConditionDAO.ListRuleCondition(ruleBaseId, null).Cast<Entities.RuleCondition>().ToList();
                BusinessCollection ruleConditionList = RuleConditionDAO.ListRuleCondition(ruleBaseId, null);

                //tomo el conjunto 

                //valido por tipo de conjunto 
                int numero = 0;
                switch (numero)
                {
                    case 1:// igual 
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }

                //SI EL CONJUNTO ES IGUAL ESTA MAL                
                var query = from t1 in RuleConditionDAO.ListRuleCondition(ruleBaseId, null).Cast<Entities.RuleCondition>().ToList()
                                //where t1.ConceptId == 0 && t1.CondValue == "3" && t1.ComparatorCode == 6                            
                            select new
                            {
                                RuleBaseId = t1.RuleBaseId,
                                RuleId = t1.RuleId,
                                ConditionId = t1.ConditionId
                            };


                if (query.Count() > 1)
                {
                    //esta mal
                }


                #endregion

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDecisionTableComposite", ex);
            }

        }

        /// <summary>
        /// obtiene los datos de la tabla de decision con sus reglas acciones y condiciones
        /// </summary>
        /// <param name="ruleBaseId">id de la tabla de decision</param>
        /// <returns></returns>
        public DecisionTableComposite GetDecisionTableComposite(int ruleBaseId)
        {
            try
            {
                Exception error = null;
                object _object = new object();
                #region Variables
                const int BOOLEAN_LIST_CODE = 2;


                List<Comparator> comparators = GetComparators();
                comparators[0].ComparatorCode = 7;
                comparators[1].ComparatorCode = 13;
                comparators[2].ComparatorCode = 14;
                comparators[3].ComparatorCode = 15;
                comparators[4].ComparatorCode = 16;
                comparators[5].ComparatorCode = 6;

                Dictionary<RulesModels.Concept, List<RulesModels.Operator>> operators = new Dictionary<RulesModels.Concept, List<RulesModels.Operator>>();

                List<RulesModels.Concept> conceptsCondition = RuleConditionDAO.GetConditionConcept(ruleBaseId);
                string[] conceptConditionControls = new string[conceptsCondition.Count];

                List<RulesModels.Concept> conceptsAction = RuleActionDAO.GetActionConcept(ruleBaseId);
                string[] conceptActionControls = new string[conceptsAction.Count];

                Dictionary<RulesModels.Concept, RulesModels.ConceptControl> conceptControls = new Dictionary<RulesModels.Concept, RulesModels.ConceptControl>();

                var ListRuleCondition = RuleConditionDAO.ListRuleCondition(ruleBaseId, null).Cast<Entities.RuleCondition>().ToList();
                var ListRuleAction = RuleActionDAO.ListRuleAction(ruleBaseId, null).Cast<Entities.RuleAction>().ToList();
                #endregion

                #region obtener los conceptos Condiciones y su orden
                for (int i = 0; i < conceptsCondition.Count; i++)//1-2-3-4
                {
                    RulesModels.ConceptControl conceptControl = GetConceptControl(conceptsCondition[i].ConceptId, conceptsCondition[i].EntityId);

                    switch (conceptControl.ConceptControlCode)
                    {
                        case 4://lista                        
                            conceptConditionControls[i] = JsonConvert.SerializeObject(((RulesModels.ListBoxControl)conceptControl).ListListEntityValues);
                            break;
                        case 5://refetencia
                            conceptConditionControls[i] = GetDataFromFilter(((RulesModels.SearchComboControl)conceptControl).ForeignEntity, null);
                            break;
                    }

                    if (!conceptControls.ContainsKey(conceptsCondition[i]))
                    {
                        conceptControls.Add(conceptsCondition[i], conceptControl);
                    }
                }
                #endregion

                #region obtener los conceptos Acciones y su orden

                for (int i = 0; i < conceptsAction.Count; i++)//1-2-3-4
                {
                    RulesModels.ConceptControl conceptControl = GetConceptControl(conceptsAction[i].ConceptId, conceptsAction[i].EntityId);

                    switch (conceptControl.ConceptControlCode)
                    {
                        case 4://lista                        
                            conceptActionControls[i] = JsonConvert.SerializeObject(((RulesModels.ListBoxControl)conceptControl).ListListEntityValues);
                            break;
                        case 5://refetencia
                            conceptActionControls[i] = GetDataFromFilter(((RulesModels.SearchComboControl)conceptControl).ForeignEntity, null);
                            break;
                    }

                    if (!conceptControls.ContainsKey(conceptsAction[i]))
                    {
                        conceptControls.Add(conceptsAction[i], conceptControl);
                    }

                    if (!operators.ContainsKey(conceptsAction[i]))
                    {
                        operators.Add(conceptsAction[i], GetOperationTypes(conceptsAction[i].ConceptId, conceptsAction[i].EntityId));
                    }
                }
                #endregion

                #region Table Composite
                DecisionTableComposite dtc = new DecisionTableComposite();

                dtc.RuleBase = RuleBaseDAO.GetRuleBase(ruleBaseId);
                dtc.RulesComposite = new List<RuleComposite>();

                //var contNombre = 0;
                var rules = RuleBaseDAO.GetRules(ruleBaseId);
                var numProcess = 50;
                var numThread = Math.Max(rules.Count, numProcess) / numProcess;

                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < numThread; i++)
                {
                    List<RuleComposite> list = new List<RuleComposite>();
                    var length = numProcess;
                    var ii = i;
                    var thread = new Thread(delegate ()
                    {
                        int contNombre = 0;

                        if (ii == numThread - 1)
                        {
                            length = rules.Count - (numProcess * ii);
                        }

                        try
                        {
                            foreach (Entities.Rule rule in rules.GetRange(numProcess * ii, length))
                            //foreach (Entities.Rule rule in rules)
                            {
                                RuleComposite ruleComposite = new RuleComposite();

                                ruleComposite.RuleId = rule.RuleId;
                                ruleComposite.RuleName = "r_" + (numProcess * ii + contNombre);
                                //ruleComposite.RuleName = "r_" + contNombre;
                                ruleComposite.IsTable = true;

                                #region lleno condiciones
                                ruleComposite.Conditions = new List<Condition>();
                                var ruleConditionList = ListRuleCondition.Where(x => x.RuleId == rule.RuleId).ToList();//RuleConditionDAO.ListRuleCondition(ruleBaseId, rule.RuleId).Cast<Entities.RuleCondition>().ToList();

                                ruleConditionList.OrderBy(x => new { x.RuleId, x.ConditionId });

                                int conditionId = 0;

                                if (ruleConditionList.Count != 0)
                                {
                                    foreach (Entities.RuleCondition rc in ruleConditionList)
                                    {
                                        Condition condition = new Condition();
                                        condition.Concept = conceptsCondition.Where(x => x.ConceptId == rc.ConceptId && x.EntityId == rc.EntityId).FirstOrDefault();//this.GetConcept(rc.ConceptId, rc.EntityId);
                                        condition.ConceptControl = conceptControls.Where(x => x.Key == condition.Concept).FirstOrDefault().Value;//RuleSetDAO.GetConceptControl(condition.Concept.ConceptId, condition.Concept.EntityId);

                                        if (rc.ComparatorCode != null)
                                        {
                                            condition.Comparator = comparators[rc.ComparatorCode.Value - 1];
                                        }
                                        condition.Value = rc.CondValue;
                                        if (rc.CondValue == null || string.IsNullOrEmpty((rc.CondValue).Trim()))
                                        {
                                            condition.DescriptionValue = "Indistinto";
                                        }
                                        else
                                        {
                                            switch (condition.Concept.ConceptControlCode)
                                            {
                                                case Enums.ConceptControlType.NumberEditor:
                                                    if ((int)condition.ConceptControl.BasicType == 3)
                                                    {
                                                        condition.DescriptionValue = rc.CondValue;
                                                        condition.Value = (rc.CondValue.IndexOf(".") != -1) ? rc.CondValue.Replace('.', ',') : rc.CondValue;
                                                    }
                                                    else
                                                    {
                                                        condition.Value = (rc.CondValue.IndexOf(".") != -1) ? (rc.CondValue.Remove(rc.CondValue.IndexOf("."))) : (rc.CondValue);
                                                        condition.DescriptionValue = rc.CondValue;
                                                    }
                                                    break;
                                                case Enums.ConceptControlType.DateEditor:
                                                    IFormatProvider culture = new CultureInfo("en-US", true);
                                                    DateTime dateVal = DateTime.ParseExact(rc.CondValue, "MM/dd/yyyy HH:mm:ss", culture);
                                                    condition.Value = String.Format("{0:dd/MM/yyyy}", dateVal);
                                                    condition.DescriptionValue = String.Format("{0:dd/MM/yyyy}", dateVal);
                                                    break;
                                                case Enums.ConceptControlType.ListBox://lista
                                                    var ListValue = JArray.Parse(conceptConditionControls[conditionId]).Where(s => s["ListValueCode"].Value<string>() == (rc.CondValue)).ToList()[0]["ListValue"];
                                                    condition.DescriptionValue = ListValue.ToString();
                                                    condition.Value = rc.CondValue;
                                                    break;
                                                case Enums.ConceptControlType.SearchCombo://refetencia
                                                    var Descripcion = JArray.Parse(conceptConditionControls[conditionId]).Where(s => s["Id"].Value<string>() == (rc.CondValue)).ToList()[0]["Descripción"];
                                                    condition.DescriptionValue = Descripcion.ToString();
                                                    condition.Value = rc.CondValue;
                                                    break;
                                                default:
                                                    condition.DescriptionValue = rc.CondValue;
                                                    break;
                                            }
                                        }
                                        conditionId++;
                                        condition.Id = rc.ConditionId;
                                        ruleComposite.Conditions.Add(condition);
                                    }
                                }
                                else
                                {
                                    foreach (var c in conceptsCondition)
                                    {
                                        Condition condition = new Condition();
                                        condition.Concept = conceptsCondition.Where(x => x.ConceptId == c.ConceptId && x.EntityId == c.EntityId).FirstOrDefault();

                                        condition.DescriptionValue = "Indistinto";
                                        condition.Id = ++conditionId;
                                        ruleComposite.Conditions.Add(condition);
                                    }
                                }
                                #endregion

                                #region lleno acciones
                                ruleComposite.Actions = new List<RulesModels.Action>();
                                var ruleActionList = ListRuleAction.Where(x => x.RuleId == rule.RuleId).ToList();//RuleActionDAO.ListRuleAction(ruleBaseId, rule.RuleId);

                                int actionId = 0;
                                if (ruleActionList.Count() != 0)
                                {
                                    foreach (Entities.RuleAction ra in ruleActionList)
                                    {
                                        RulesModels.Action action = new RulesModels.Action();
                                        action.ConceptLeft = conceptsAction.Where(x => x.ConceptId == ra.ConceptId.Value && x.EntityId == ra.EntityId.Value).FirstOrDefault();
                                        action.ConceptControl = conceptControls.Where(x => x.Key == action.ConceptLeft).FirstOrDefault().Value;

                                        if (ra.OperatorCode != null)
                                        {
                                            action.Operator = operators.Where(x => x.Key == action.ConceptLeft).FirstOrDefault().Value[ra.OperatorCode.Value - 1];
                                        }
                                        action.ValueRight = ra.ActionValue;

                                        if (ra.ActionValue == null || string.IsNullOrEmpty((ra.ActionValue).Trim()))
                                        {
                                            action.Expression = "Indistinto";
                                        }
                                        else
                                        {
                                            switch (action.ConceptLeft.ConceptControlCode)
                                            {
                                                case Enums.ConceptControlType.NumberEditor:

                                                    if ((int)action.ConceptControl.BasicType == 3)
                                                    {
                                                        action.Expression = ra.ActionValue;
                                                        action.ValueRight = (ra.ActionValue.IndexOf(".") != -1) ? ra.ActionValue.Replace('.', ',') : ra.ActionValue;
                                                    }
                                                    else
                                                    {
                                                        action.ValueRight = (ra.ActionValue.IndexOf(".") != -1) ? (ra.ActionValue.Remove(ra.ActionValue.IndexOf("."))) : (ra.ActionValue);
                                                        action.Expression = ra.ActionValue;
                                                    }
                                                    break;

                                                case Enums.ConceptControlType.DateEditor:
                                                    IFormatProvider culture = new CultureInfo("en-US", true);
                                                    DateTime dateVal = DateTime.ParseExact(ra.ActionValue, "MM/dd/yyyy HH:mm:ss", culture);
                                                    action.ValueRight = string.Format("{0:dd/MM/yyyy}", dateVal);
                                                    action.Expression = string.Format("{0:dd/MM/yyyy}", dateVal);
                                                    break;
                                                case Enums.ConceptControlType.ListBox://lista  
                                                    var lc = ListConceptDAO.FindListConcept(action.ConceptLeft.ConceptId, action.ConceptLeft.EntityId);
                                                    if (lc.ListEntityCode == BOOLEAN_LIST_CODE)
                                                    {
                                                        var ListValue = JArray.Parse(conceptActionControls[actionId]).Where(s => s["ListValueCode"].Value<string>() == (Convert.ToBoolean(ra.ActionValue) ? 1 : 0).ToString()).ToList()[0]["ListValue"];
                                                        action.Expression = ListValue.ToString();
                                                        action.ValueRight = (ra.ActionValue == "false") ? "1" : "0";
                                                    }
                                                    else
                                                    {
                                                        var ListValue = JArray.Parse(conceptActionControls[actionId]).Where(s => s["ListValueCode"].Value<string>() == (ra.ActionValue)).ToList()[0]["ListValue"];
                                                        action.Expression = ListValue.ToString();
                                                        action.ValueRight = ra.ActionValue;
                                                    }
                                                    break;
                                                case Enums.ConceptControlType.SearchCombo://refetencia
                                                    var Descripcion = JArray.Parse(conceptActionControls[actionId]).Where(s => s["Id"].Value<string>() == (ra.ActionValue)).ToList()[0]["Descripción"];
                                                    action.Expression = Descripcion.ToString();
                                                    action.ValueRight = ra.ActionValue;
                                                    break;
                                                default:
                                                    action.Expression = ra.ActionValue;
                                                    break;
                                            }
                                        }
                                        actionId++;
                                        action.Id = ra.ActionId;
                                        ruleComposite.Actions.Add(action);
                                    }
                                }
                                else
                                {
                                    foreach (var c in conceptsAction)
                                    {
                                        RulesModels.Action action = new RulesModels.Action();
                                        action.ConceptLeft = conceptsAction.Where(x => x.ConceptId == c.ConceptId && x.EntityId == c.EntityId).FirstOrDefault();// this.GetConcept(c.ConceptId, c.EntityId);
                                                                                                                                                                //action.Operator = ;                        
                                                                                                                                                                //action.ValueRight = ;
                                        action.Expression = "Indistinto";
                                        action.Id = ++actionId;
                                        ruleComposite.Actions.Add(action);
                                    }
                                }
                                #endregion

                                //dtc.RulesComposite.Add(ruleComposite);
                                list.Add(ruleComposite);
                                //contNombre++;
                            }
                            lock (_object)
                            {
                                dtc.RulesComposite.AddRange(list);
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (_object)
                            {
                                if (error == null)
                                {
                                    error = ex;
                                }
                            }
                        }
                    });
                    threads.Add(thread);
                }
                #endregion


                Array.ForEach(threads.ToArray(), delegate (Thread t) { t.Start(); });
                Array.ForEach(threads.ToArray(), delegate (Thread t) { t.Join(); });

                if (error != null)
                {
                    throw new BusinessException(error.Message, error);
                }

                //dtc.RulesComposite.Sort(new RulesCompositeComparer(conceptsCondition));

                return dtc;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDecisionTableComposite", ex);
            }

        }

        /// <summary>
        /// actualiza la regla
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <returns></returns>
        public bool PostDecisionTable(int ruleBaseId)
        {
            try
            {

                DecisionTableComposite DecisionTableComposite = GetDecisionTableComposite(ruleBaseId);
                RulesModels.RuleSetComposite RuleSetComposite = new RulesModels.RuleSetComposite();

                RuleSetComposite.RuleSet = new RulesModels.RuleSet();
                RuleSetComposite.RuleSet.RuleSetId = DecisionTableComposite.RuleBase.RuleBaseId;
                RuleSetComposite.RuleSet.Description = DecisionTableComposite.RuleBase.Description;
                RuleSetComposite.RuleSet.PackageId = DecisionTableComposite.RuleBase.PackageId;
                RuleSetComposite.RuleSet.LevelId = DecisionTableComposite.RuleBase.LevelId;
                RuleSetComposite.RuleSet.CurrentFrom = DecisionTableComposite.RuleBase.CurrentFrom;
                RuleSetComposite.RuleSet.IsTable = true;

                RuleSetComposite.RuleComposites = DecisionTableComposite.RulesComposite;

                RuleBaseDAO.UpdateRuleBase(RuleBaseDAO.FindRuleBase(DecisionTableComposite.RuleBase.RuleBaseId), true);
                return CreateRuleSet(RuleSetComposite);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateTableDecision", ex);
            }

        }

        /// <summary>
        /// crea los datos de la tabla de decision 
        /// </summary>
        /// <param name="RuleBase">la parte de la parametirzacion de la tabla de decision </param>
        /// <param name="Condiciones">las condiciones de la tabla de decision</param>
        /// <param name="Acciones">las acciones de la tabla de decision</param>
        /// <returns></returns>
        public bool CreateTableDecision(RulesModels.RuleBase RuleBase, List<RulesModels.Concept> Condiciones, List<RulesModels.Concept> Acciones)
        {
            try
            {
                if (RuleBase.RuleBaseId == 0)
                {
                    RuleBaseDAO.CreateTableDecision(RuleBase, Condiciones, Acciones);
                }
                else
                {
                    RuleBaseDAO.UpdateTableDecision(RuleBase, Condiciones, Acciones);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateTableDecision", ex);
            }

        }

        /// <summary>
        /// crea los datos de la tabla de decision las filas con sus acciones y condiciones
        /// </summary>
        /// <param name="DecisionTableComposite"></param>
        /// <returns></returns>
        public bool CreateTableDecisionRow(DecisionTableComposite RulesAdd, DecisionTableComposite RulesEdit, DecisionTableComposite RulesDelete)
        {
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e) { };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e) { };
                    try
                    {
                        RuleBaseDAO.UpdateRuleBase(RuleBaseDAO.FindRuleBase(RulesAdd.RuleBase.RuleBaseId), false);

                        #region insertar
                        if (RulesAdd.RulesComposite != null)
                        {
                            foreach (var RulesComposit in RulesAdd.RulesComposite)
                            {
                                #region rule
                                //guardo el rule para el orden de la regla
                                Entities.Rule rule = new Entities.Rule(RulesAdd.RuleBase.RuleBaseId);
                                rule.RuleBaseId = RulesAdd.RuleBase.RuleBaseId;
                                rule.RuleId = RulesComposit.RuleId;
                                rule.Order = 0;

                                RuleDAO.CreateRule(rule);
                                #endregion

                                #region condiciones

                                //guardo las condicones
                                foreach (var Condition in RulesComposit.Conditions)
                                {
                                    Entities.RuleCondition ruleCondition = new Entities.RuleCondition(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId, Condition.Id);
                                    ruleCondition.RuleBaseId = RulesAdd.RuleBase.RuleBaseId;
                                    ruleCondition.RuleId = RulesComposit.RuleId;
                                    ruleCondition.ConditionId = Condition.Id;
                                    ruleCondition.EntityId = Condition.Concept.EntityId;
                                    ruleCondition.ConceptId = Condition.Concept.ConceptId;
                                    if (Condition.Comparator != null)
                                    {
                                        ruleCondition.ComparatorCode = Condition.Comparator.ComparatorCode;

                                        switch (Condition.Comparator.ComparatorCode)
                                        {
                                            case 7:
                                                ruleCondition.ComparatorCode = 1;
                                                break;
                                            case 13:
                                                ruleCondition.ComparatorCode = 2;
                                                break;
                                            case 14:
                                                ruleCondition.ComparatorCode = 3;
                                                break;
                                            case 15:
                                                ruleCondition.ComparatorCode = 4;
                                                break;
                                            case 16:
                                                ruleCondition.ComparatorCode = 5;
                                                break;
                                            case 6:
                                                ruleCondition.ComparatorCode = 6;
                                                break;
                                            default:
                                                break;
                                        }

                                    }

                                    ruleCondition.RuleValueTypeCode = 1;
                                    ruleCondition.CondValue = Convert.ToString(Condition.Value);//REVISAR JONATHAN              
                                    ruleCondition.OrderNum = 0;
                                    RuleConditionDAO.CreateRuleCondition(ruleCondition);
                                };
                                #endregion

                                #region acciones
                                //guardo las acciones
                                foreach (var Action in RulesComposit.Actions)
                                {
                                    Entities.RuleAction ruleAction = new Entities.RuleAction(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId, Action.Id);
                                    ruleAction.RuleBaseId = RulesAdd.RuleBase.RuleBaseId;
                                    ruleAction.RuleId = RulesComposit.RuleId;
                                    ruleAction.ActionId = Action.Id;
                                    ruleAction.ActionTypeCode = 1;
                                    ruleAction.EntityId = Action.ConceptLeft.EntityId;
                                    ruleAction.ConceptId = Action.ConceptLeft.ConceptId;
                                    if (Action.Operator != null)
                                    {
                                        ruleAction.OperatorCode = Action.Operator.OperatorCode;

                                        switch (Action.Operator.OperatorCode)
                                        {
                                            case 5:
                                                ruleAction.OperatorCode = 1;
                                                break;
                                            case 0:
                                                ruleAction.OperatorCode = 2;
                                                break;
                                            case 1:
                                                ruleAction.OperatorCode = 3;
                                                break;
                                            case 2:
                                                ruleAction.OperatorCode = 4;
                                                break;
                                            case 3:
                                                ruleAction.OperatorCode = 5;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    ruleAction.ValueTypeCode = 1;

                                    var lc = ListConceptDAO.FindListConcept(Action.ConceptLeft.ConceptId, Action.ConceptLeft.EntityId);
                                    if (lc != null && lc.ListEntityCode == 2)
                                    {
                                        ruleAction.ActionValue = Action.ValueRight == "0" ? "False" : "True";
                                    }
                                    else
                                    {
                                        ruleAction.ActionValue = Convert.ToString(Action.ValueRight);
                                    }
                                    ruleAction.OrderNum = 0;
                                    RuleActionDAO.CreateRuleAction(ruleAction);
                                };
                                #endregion

                            };
                        }

                        #endregion

                        #region editar
                        if (RulesEdit.RulesComposite != null)
                        {
                            foreach (var RulesComposit in RulesEdit.RulesComposite)
                            {
                                Entities.Rule rule = RuleDAO.GetRule(new Entities.Rule(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId));
                                rule.Order = 0;
                                RuleDAO.UpdateRule(rule);

                                #region condiciones
                                //edito las condicones
                                foreach (var Condition in RulesComposit.Conditions)
                                {
                                    Entities.RuleCondition ruleCondition = RuleConditionDAO.GetRuleCondition(new Entities.RuleCondition(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId, Condition.Id));
                                    ruleCondition.EntityId = Condition.Concept.EntityId;
                                    ruleCondition.ConceptId = Condition.Concept.ConceptId;
                                    if (Condition.Comparator != null)
                                    {
                                        ruleCondition.ComparatorCode = Condition.Comparator.ComparatorCode;

                                        switch (Condition.Comparator.ComparatorCode)
                                        {
                                            case 7:
                                                ruleCondition.ComparatorCode = 1;
                                                break;
                                            case 13:
                                                ruleCondition.ComparatorCode = 2;
                                                break;
                                            case 14:
                                                ruleCondition.ComparatorCode = 3;
                                                break;
                                            case 15:
                                                ruleCondition.ComparatorCode = 4;
                                                break;
                                            case 16:
                                                ruleCondition.ComparatorCode = 5;
                                                break;
                                            case 6:
                                                ruleCondition.ComparatorCode = 6;
                                                break;
                                            default:
                                                break;
                                        }

                                    }

                                    ruleCondition.RuleValueTypeCode = 1;
                                    ruleCondition.CondValue = Convert.ToString(Condition.Value);//REVISAR JONATHAN              
                                    ruleCondition.OrderNum = 0;
                                    RuleConditionDAO.UpdateRuleCondition(ruleCondition);
                                };
                                #endregion

                                #region acciones
                                //guardo las acciones
                                foreach (var Action in RulesComposit.Actions)
                                {
                                    Entities.RuleAction ruleAction = RuleActionDAO.GetRuleAction(new Entities.RuleAction(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId, Action.Id));
                                    ruleAction.ActionTypeCode = 1;
                                    ruleAction.EntityId = Action.ConceptLeft.EntityId;
                                    ruleAction.ConceptId = Action.ConceptLeft.ConceptId;
                                    if (Action.Operator != null)
                                    {
                                        ruleAction.OperatorCode = Action.Operator.OperatorCode;

                                        switch (Action.Operator.OperatorCode)
                                        {
                                            case 5:
                                                ruleAction.OperatorCode = 1;
                                                break;
                                            case 0:
                                                ruleAction.OperatorCode = 2;
                                                break;
                                            case 1:
                                                ruleAction.OperatorCode = 3;
                                                break;
                                            case 2:
                                                ruleAction.OperatorCode = 4;
                                                break;
                                            case 3:
                                                ruleAction.OperatorCode = 5;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    ruleAction.ValueTypeCode = 1;
                                    var lc = ListConceptDAO.FindListConcept(Action.ConceptLeft.ConceptId, Action.ConceptLeft.EntityId);
                                    if (lc != null && lc.ListEntityCode == 2)
                                    {
                                        ruleAction.ActionValue = Action.ValueRight == "0" ? "False" : "True";
                                    }
                                    else
                                    {
                                        ruleAction.ActionValue = Convert.ToString(Action.ValueRight);
                                    }
                                    ruleAction.OrderNum = 0;
                                    RuleActionDAO.UpdateRuleAction(ruleAction);
                                };
                                #endregion

                            };
                        }

                        #endregion

                        #region eliminar
                        if (RulesDelete.RulesComposite != null)
                        {
                            foreach (var RulesComposit in RulesDelete.RulesComposite)
                            {
                                #region rule
                                //se elimina la regla
                                Entities.Rule rule = RuleDAO.GetRule(new Entities.Rule(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId));
                                RuleDAO.DeleteRule(rule);
                                #endregion

                                #region condiciones

                                //se eliminan las condiciones
                                foreach (var Condition in RulesComposit.Conditions)
                                {
                                    Entities.RuleCondition ruleCondition = RuleConditionDAO.GetRuleCondition(new Entities.RuleCondition(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId, Condition.Id));
                                    RuleConditionDAO.DeleteRuleCondition(ruleCondition);
                                };
                                #endregion

                                #region acciones
                                //guardo las acciones
                                foreach (var Action in RulesComposit.Actions)
                                {
                                    Entities.RuleAction ruleAction = RuleActionDAO.GetRuleAction(new Entities.RuleAction(RulesAdd.RuleBase.RuleBaseId, RulesComposit.RuleId, Action.Id));
                                    RuleActionDAO.DeleteRuleAction(ruleAction);
                                };
                                #endregion

                            };
                        }
                        #endregion

                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException("Error CreateTableDecisionRow" + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// elimina la tabla de decision
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteTableDecision(int Id)
        {
            try
            {
                //elimino                     
                NameValue[] pars = new NameValue[4];
                pars[0] = new NameValue("RULE_BASE_ID", Id);
                pars[1] = new NameValue("ENTITY_ID", System.DBNull.Value);
                pars[2] = new NameValue("CONCEPT_ID", System.DBNull.Value);
                pars[3] = new NameValue("ALL_TABLE", 1);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPNonQuery("SCR.DELETE_CONTENT_TABLE_DECISION", pars);
                }

                return true;
            }
            catch (Exception)
            {
                throw new BusinessException("Error al guardar la tabla de decision"); throw;
            }
        }

        /// <summary>
        /// Obtener lista de valores por entidad
        /// </summary>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="entityId">id de la entidad</param>
        /// <returns>Lista de valores</returns>
        public List<RulesModels.ListEntityValue> GetListEntityValuesByConceptIdEntityId(int conceptId, int entityId)
        {
            try
            {
                return ListEntityValueDAO.GetListEntityValuesByConceptIdEntityId(conceptId, entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntityValuesByConceptIdEntityId", ex);
            }

        }

        /// <summary>
        /// Gets the concepts by level identifier.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <returns></returns>
        public List<RulesModels.RuleSet> GetConceptsByLevelId(int levelId)
        {
            try
            {
                return RuleSetDAO.GetConceptsByLevelId(levelId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptsByLevelId", ex);
            }

        }

        /// <summary>
        /// Obtener reglas por nivel
        /// </summary>
        /// <param name="level">Nivel</param>
        /// <returns></returns>
        public List<RulesModels.RuleSet> GetRuleSetByLevels(List<RulesModels.Level> level)
        {
            try
            {
                return RuleSetDAO.GetRuleSetByLevels(level);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetByLevels", ex);
            }

        }

        /// <summary>
        /// Obtener paquetes de reglas por nivel
        /// </summary>
        /// <param name="levelId">nivel por id</param>
        /// <returns></returns>
        public List<RulesModels.RuleSet> GetRuleSetDTOsByLevelId(int levelId, bool IsEvent)
        {
            try
            {
                return RuleSetDAO.GetRuleSetDTOsByLevelId(levelId, IsEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetDTOsByLevelId", ex);
            }

        }

        /// <summary>
        /// Obtener paquetes de reglas por nivel
        /// </summary>
        /// <param name="levelId">nivel por id</param>
        /// <returns></returns>
        public int[] GetLevelsEntity(Enums.Level levels)
        {
            try
            {
                return LevelDAO.GetLevelsEntity(levels);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetQuestionsByLevel", ex);
            }

        }

        /// <summary>
        /// Realiza el mapeo y el guardado de la tabla de decision del excel
        /// </summary>
        /// <param name="pathXml"></param>
        /// <param name="pathXls"></param>
        public DecisionTableMappingResult LoadDecisionTableFromFile(string pathXml, string pathXls, bool save)
        {
            try
            {
                DecisionTableLoader dtl = new DecisionTableLoader(pathXml, pathXls);

                if (dtl.ReadMappingFile())
                {
                    dtl.CreateDataSet();
                    if (dtl.ReadExcelFile())
                    {
                        if (save)
                        {
                            var ruleBase = dtl.InsertRules();
                            var result = dtl.InsertDataRules(ruleBase);
                            dtl.DeleteFiles();
                            return result;
                        }
                    }
                }

                return new DecisionTableMappingResult
                {
                    DataSet = dtl.GetDataSet()
                };
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene la reglas a ejecutar para un proceso especifico
        /// </summary>
        /// <param name="processType"></param>
        public RuleProcessRuleSet GetRulestByRulsetProcessType(int processType)
        {
            try
            {
                RulsetProcessDAO rulsetProcessDao = new RulsetProcessDAO();
                return rulsetProcessDao.GetRulestByRulsetProcessType(processType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteRuleSet(int ruleSetId)
        {
            try
            {
                RuleSetDAO.DeleteRuleSet(ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        public List<RulesModels.RuleSet> GetRuleSetByIds(List<int> ids)
        {
            try
            {

                return RuleSetDAO.GetRuleSetByIds(ids);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRuleSetByIds", e);
            }
        }

        public _RuleSet GetRuleSetById(int ruleSetId)
        {
            try
            {
                return RuleSetDAO.GetRuleSetById(ruleSetId);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetRuleSetById", e);
            }
        }

        /// <summary>
        /// Exportar listado de reglas
        /// </summary>
        /// <returns>excel de reglas</returns>       
        public string GenerateFileToDecisionTable(List<Exception> exceptions)
        {
            try
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();
                List<MRules._RuleSet> ruleSets = ruleSetDao.GetRuleSets();
                if (ruleSets.Count > 0)
                {
                    FileDAO fileDAO = new FileDAO();
                    return fileDAO.GenerateFileToRules(ruleSets);
                }
                return "";
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GenerateFileToRules", e);
            }
        }
        #region precarga
        public void LoadScripts()
        {

            TP.Task.Run(() =>
            {
                try
                {
                    ScriptDAO.FindScript(-1);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            });
            TP.Task.Run(() =>
            {
                try
                {
                    EntityDAO.FindEntity(-1);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            });
            TP.Task.Run(() =>
            {
                try
                {
                    ObjectCriteriaBuilder filterNode = new ObjectCriteriaBuilder();
                    filterNode.PropertyEquals(Entities.Node.Properties.ScriptId, -1);
                    NodeDAO.GetListNode(filterNode, null);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            });
            TP.Task.Run(() =>
            {
                try
                {
                    ObjectCriteriaBuilder filterNode = new ObjectCriteriaBuilder();
                    filterNode.PropertyEquals(Entities.Question.Properties.QuestionId, -1);
                    QuestionDAO.ListQuestions(filterNode.GetPredicate(), null);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }

            });
            TP.Task.Run(() =>
            {
                try
                {
                    ObjectCriteriaBuilder filterNode = new ObjectCriteriaBuilder();
                    filterNode.PropertyEquals(Entities.NodeQuestion.Properties.NodeId, -1);
                    NodeQuestionDAO.ListNodeQuestion(filterNode.GetPredicate(), null);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }

            });
            TP.Task.Run(() =>
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Entities.Edge.Properties.EdgeId, -1);
                    EdgeDAO.ListEdge(filter.GetPredicate(), null);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }

            });
            TP.Task.Run(() =>
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Entities.Edge.Properties.EdgeId, -1);
                    EdgeAnswerDAO.FindEdgeAnswer(-1, -1, -1);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }

            });
            TP.Task.Run(() =>
            {
                try
                {
                    DAOs.RuleSetDAO.GetConceptControl(-1, -1);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }

            });


        }
        #endregion preccarga

    }
}
