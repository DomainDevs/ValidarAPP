using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.RulesScriptsServices
{
    [ServiceContract]
    public interface IConceptsService
    {
        /// <summary>
        /// Obtiene los conceptos segun el filtro 
        /// </summary>
        /// <param name="idEntity"></param>
        /// <param name="filter">like de la descripcion</param>
        /// <returns></returns>
        [OperationContract]
        List<_Concept> GetConceptsByFilter(List<int> idEntity, string filter);

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        _Concept GetConceptByIdConceptIdEntity(int idConcept, int idEntity);

        /// <summary>
        /// Obtiene el concepto especifico con sus respectivos valores
        /// </summary>
        /// <param name="idEntity">id de la entidad</param>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="conceptType">tipo de concepto</param>
        /// <returns></returns>
        [OperationContract]
        object GetSpecificConceptWithVales(int idConcept, int idEntity, string[] dependency, Enums.ConceptType conceptType);

        [OperationContract]
        string ExportConcepts();

        /// <summary>
        /// Obtiene el Concepto por descripción
        /// </summary>
        /// <param name="description">Description de Concepto</param>
        /// <returns>Bool de respuesta</returns>
        [OperationContract]
        bool? GetConceptsByDescription(string description);









        /***********************************/
        //ANTIGUO
        /***********************************/

        #region ANTIGUO

        /// <summary>
        /// obtiene todos los conceptos 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Concept> GetConcepts();

        /// <summary>
        /// obtiene todos los conceptos del Excel
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Concept> GetConceptsFile();

        [OperationContract]
        List<Concept> GetConceptsByIdModuleIdLevelDescription(int? idModule, int? idLevel, int filter, string description);
       

        /// <summary>
        /// Guarda los cambios realizados para los conceptos
        /// </summary>
        /// <param name="ConceptBasicAdd">lista de conceptos basicos a crear</param>
        /// <param name="ConceptBasicEdit">lista de conceptos basicos a editar</param>
        /// <param name="ConceptBasicDelete">lista de conceptos basicos a eliminar</param>
        /// <param name="ConceptListAdd">lista de conceptos lista a crear</param>
        /// <param name="ConceptListEdit">lista de conceptos lista a editar</param>
        /// <param name="ConceptListDelete">lista de conceptos lista a eliminar</param>
        /// <param name="ConceptRangeAdd">lista de conceptos rango a crear</param>
        /// <param name="ConceptRangeEdit">lista de conceptos rango a editar</param>
        /// <param name="ConceptRangeDelete">lista de conceptos rango a eliminar</param>
        /// <param name="ConceptReferenceAdd">lista de conceptos referencia a crear</param>
        /// <param name="ConceptReferenceEdit">lista de conceptos referencia a editar</param>
        /// <param name="ConceptReferenceDelete">lista de conceptos referencia a eliminar</param>
        /// <returns></returns>
        [OperationContract]
        void SaveConcepts(List<BasicConcept> ConceptBasicAdd, List<BasicConcept> ConceptBasicEdit, List<BasicConcept> ConceptBasicDelete,
            List<ListConcept> ConceptListAdd, List<ListConcept> ConceptListEdit, List<ListConcept> ConceptListDelete,
            List<RangeConcept> ConceptRangeAdd, List<RangeConcept> ConceptRangeEdit, List<RangeConcept> ConceptRangeDelete,
            List<ReferenceConcept> ConceptReferenceAdd, List<ReferenceConcept> ConceptReferenceEdit, List<ReferenceConcept> ConceptReferenceDelete
        );

        /// <summary>
        /// valida si el concepto esta siendo usado 
        /// </summary>
        /// <param name="conceptSrt">concepto serializado</param>
        /// <returns></returns>
        [OperationContract]
        bool IsInUse(Concept concept);

        /// <summary>
        /// obtiene una lista de tipos de conceptos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ConceptType> GetConceptTypes();

        /// <summary>
        /// obtiene los tipos de conceptos basicos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<BasicType> GetBasicTypes();

        /// <summary>
        /// obtiene los valores para los tipos de conceptos basicos
        /// </summary>
        /// <param name="conceptSrt">concepto basico serializado</param>
        /// <returns></returns>
        [OperationContract]
        BasicConcept GetBasicConceptsValues(BasicConcept concept);

        /// <summary>
        /// Otiene la entidad por IdEntity
        /// </summary>
        /// <param name="EntityId">Id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        Entity GetEntity(int EntityId);

        /// <summary>
        /// obtiene las entidades de tipo referencia
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity> GetForeignEntities(int packageId, int levelId);

        /// <summary>
        /// Obtiene la entidad dependiendo el modulo y el nivel
        /// </summary>
        /// <param name="IdPackage">modulo Id</param>
        /// <param name="IdLevel">nivel Id</param>
        /// <returns></returns>
        [OperationContract]
        Entity GetEntityByIdPackageIdLevel(int IdPackage, int IdLevel);

        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        [OperationContract]
        List<ListEntity> GetListEntity();

        /// <summary>
        /// Obtener las listas de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        [OperationContract]
        List<ListEntity> GetListEntityByDescription(string Description);

        /// <summary>
        /// Obtener los valores de lista por código de lista de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        [OperationContract]
        List<ListEntity> GetListEntityValueByListEntityCode(int listEntityCode);

        ///// <summary>
        ///// Crear una lista de valores.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>lista de valores</returns>
        //[OperationContract]
        //List<ListEntity> CreateListEntity(List<ListEntity> listEntity);

        ///// <summary>
        ///// Actualizar una lista de valores.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>lista de valores</returns>
        //[OperationContract]
        //List<ListEntity> UpdateListEntity(List<ListEntity> listEntity);

        ///// <summary>
        ///// Actualizar una lista de valores con valores creados.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>lista de valores</returns>
        //[OperationContract]
        //List<ListEntity> UpdateListEntityValueCreated(List<ListEntity> listEntity);

        ///// <summary>
        ///// Actualizar una lista de valores con valores editados.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>lista de valores</returns>
        //[OperationContract]
        //List<ListEntity> UpdateListEntityValueEdited(List<ListEntity> listEntity);

        ///// <summary>
        ///// Actualizar una lista de valores con valores eliminados.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>lista de valores</returns>
        //[OperationContract]
        //List<ListEntity> UpdateListEntityValueDeleted(List<ListEntity> listEntity);

        ///// <summary>
        ///// Eliminar una lista de valores.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>confirmacion booleana true/false</returns>
        //[OperationContract]
        //bool DeleteListEntity(List<ListEntity> listEntity);

        [OperationContract]
        List<ListEntity> ExecuteOperationListEntities(List<ListEntity> listEntity);

        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de rangos de valores</returns>
        [OperationContract]
        List<RangeEntity> GetRangeEntity();

        /// <summary>
        /// Obtener los rangos de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        [OperationContract]
        List<RangeEntity> GetRangeEntityByDescription(string Description);

        /// <summary>
        /// Obtener los rangos de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        [OperationContract]
        List<RangeEntity> GetRangeEntityValueByRangeEntityCode(int rangeEntityCode);

        [OperationContract]
        List<RangeEntity> ExecuteOperationRangeEntities(List<RangeEntity> rangeEntities);
        #endregion
    }
}
