using System;
using System.Collections.Specialized;
using System.Collections;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Helpers;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class DynamicConceptValueDAO
    {
        private const int BOOLEAN_LIST_CODE = 2;

        /// <summary>
        /// obtiene un ListDictionary a partir de un IList
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        public ListDictionary GetDynamicConceptValueDTO(IList primaryKeys)
        {
            try
            {
                ListDictionary dict = new ListDictionary();
                ListDictionary dConceptLists = new ListDictionary();

                foreach (PrimaryKey key in primaryKeys)
                {
                    IDictionary values = ReadConcepts(key);
                    if (values != null && values.Count > 0)
                    {
                        System.Type @class = key.Class;
                        IDictionary concepts = (IDictionary)dConceptLists[@class];

                        if (concepts == null)
                        {
                            concepts = this.ReadDynamicConceptDefinitions(@class);
                            if (concepts == null)
                            {
                                throw new Exception("No existen conceptos dinamicos para la clase: " + @class.FullName);
                            }

                            dConceptLists.Add(@class, concepts);
                        }

                        ConceptValueDTODictionary dcs = new ConceptValueDTODictionary();
                        dict.Add(key, dcs);

                        foreach (DictionaryEntry de in values)
                        {
                            string name = (string)de.Key;
                            SCREN.Concept c = (SCREN.Concept)concepts[name];

                            if (c == null)
                            {
                                throw new Exception("No existe el concepto dinamico: " + name);
                            }

                            ConceptValueDTO dto = new ConceptValueDTO(key, c.EntityId, c.ConceptId);
                            dto.Value = de.Value;

                            dcs.Add(dto);
                        }
                    }
                }
                return dict;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDynamicConceptValueDTO", ex);
            }
           
        }

        /// <summary>
        /// obtiene un IDictionary a partir de un System.Type
        /// </summary>
        /// <param name="class"></param>
        /// <returns></returns>
        private IDictionary ReadDynamicConceptDefinitions(System.Type @class)
        {
            try
            {
                ObjectCriteriaBuilder b = new ObjectCriteriaBuilder();
                b.Property(PARAMEN.Entity.Properties.EntityName);
                b.Equal();
                b.Constant(@class.Name);

                IList entityList = EntityDAO.ListEntity(b.GetPredicate(), null);
                if (entityList.Count == 0)
                {
                    throw new Exception("Entidad no definida: " + @class.FullName);
                }

                PARAMEN.Entity e = (PARAMEN.Entity)entityList[0];

                b.Clear();
                b.Property(SCREN.Concept.Properties.EntityId);
                b.Equal();
                b.Constant(e.EntityId);
                b.And();
                b.Property(SCREN.Concept.Properties.IsStatic);
                b.Equal();
                b.Constant(false);

                IList conceptList = ConceptDAO.GetConceptsByFilterSort(b.GetPredicate(), null);

                ListDictionary dict = new ListDictionary();

                foreach (SCREN.Concept c in conceptList)
                {
                    dict.Add(c.ConceptName, c);
                }
                return dict;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ReadDynamicConceptDefinitions", ex);
            }
           
        }

        /// <summary>
        /// obtiene un IDictionary a partir de PrimaryKey
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        private IDictionary ReadConcepts(PrimaryKey primaryKey)
        {
            try
            {
                IDictionary conceptsDictionary = new HybridDictionary();

                PARAMEN.Entity entity = EntityDAO.GetEntityByName(primaryKey.Class.Name);

                DynamicConceptRelation dynamicConceptRelation = FindDynamicConceptRelation(entity, primaryKey);

                if (dynamicConceptRelation != null)
                {
                    IList dynamicConceptValueList = ListDynamicConceptValues(dynamicConceptRelation);
                    foreach (DynamicConceptValue dynamicConceptValue in dynamicConceptValueList)
                    {
                        AddDynamicConceptValueToDictionary(entity, dynamicConceptValue, conceptsDictionary);
                    }
                }
                return conceptsDictionary;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ReadConcepts", ex);
            }
            
        }

        /// <summary>
        /// agrega un concepto al diccionario a parir de entity, DynamicConceptValue, IDictionary 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dynamicConceptValue"></param>
        /// <param name="conceptsDictionary"></param>
        protected virtual void AddDynamicConceptValueToDictionary(PARAMEN.Entity entity, DynamicConceptValue dynamicConceptValue, IDictionary conceptsDictionary)
        {
            try
            {
                SCREN.Concept concept = DAOs.ConceptDAO.GetConceptByConceptIdEntityId(dynamicConceptValue.ConceptId, entity.EntityId);

                int basicType = -1;
                int listId = -1;
                if (concept.ConceptTypeCode == (int)ConceptType.Types.Basic)
                {
                    basicType = GetBasicConceptTypeCode(concept);
                }
                else if (concept.ConceptTypeCode == (int)ConceptType.Types.List)
                {
                    listId = GetListConceptCode(concept);
                }
                object value = ConvertToTypeCode(concept.ConceptTypeCode, basicType, dynamicConceptValue.ConceptValue, listId);
                conceptsDictionary.Add(concept.ConceptName, value);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener AddDynamicConceptValueToDictionary", ex);
            }
            
        }

        /// <summary>
        /// obtiene el ListEntityCode a partir del Concept
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        protected virtual int GetListConceptCode(SCREN.Concept concept)
        {
            try
            {
                // Busco la entidad
                ListConcept listConcept = ListConceptDAO.GetListConceptByConceptIdEntityId(concept.ConceptId, concept.EntityId);

                if (listConcept == null)
                {
                    throw new BusinessException("PRPERR_CANNOT_FIND_LIST_CONCEPT_FOR_CONCEPT",
                                                    new[] {concept.ConceptId.ToString(), 
                                                              concept.EntityId.ToString()});
                }

                return listConcept.ListEntityCode;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListConceptCode", ex);
            }
           
        }

        [Obsolete]///TODO:Copy Paste de DynamicConceptPersister. Por favor, reescribir la lógica de conversión de valores de conceptos dinámicos
        private object ConvertToTypeCode(int typeCode, int basicType, string value, int listId)
        {
            try
            {
                if (value == null || value == " ")
                {
                    return null;
                }

                switch ((ConceptType.Types)typeCode)
                {
                    case ConceptType.Types.List:
                        if (listId == BOOLEAN_LIST_CODE)
                        {
                            bool res;
                            if (bool.TryParse(value, out res))
                            {
                                return res;
                            }

                            int intRes;
                            if (int.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out intRes))
                            {
                                return (intRes != 0);
                            }

                            throw new Exception(string.Format("Invalid boolean value: {0}.", value));
                        }

                        goto case ConceptType.Types.Range;

                    case ConceptType.Types.Range:
                    case ConceptType.Types.Reference:
                        return Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture);

                    case ConceptType.Types.Basic:
                        switch ((BasicType.Types)basicType)
                        {
                            case BasicType.Types.Boolean:
                                return Convert.ToBoolean(value, System.Globalization.CultureInfo.InvariantCulture);
                            case BasicType.Types.Date:
                                return Convert.ToDateTime(value, System.Globalization.CultureInfo.InvariantCulture);
                            case BasicType.Types.Decimal:
                                return Convert.ToDecimal(value, System.Globalization.CultureInfo.InvariantCulture);
                            case BasicType.Types.Number:
                                return Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture);
                            case BasicType.Types.Long:
                                return Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);

                            case BasicType.Types.Text:
                                return value;

                            default:
                                throw new Exception("Basic type not supported: " + basicType.ToString());
                        }
                    default:
                        throw new Exception("Concept type not supported: " + basicType.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ConvertToTypeCode", ex);
            }
            
        }

        /// <summary>
        /// obtiene el BasicTypeCode a partir del Concept
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        protected virtual int GetBasicConceptTypeCode(SCREN.Concept concept)
        {
            try
            {
                SCREN.BasicConcept basicConcept = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(concept.ConceptId, concept.EntityId);
                if (basicConcept == null)
                {
                    throw new BusinessException("PRPERR_CANNOT_FIND_BASIC_CONCEPT_FOR_CONCEPT", new object[] { concept.ConceptName, concept.ConceptId, concept.EntityId });
                }
                return basicConcept.BasicTypeCode;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetBasicConceptTypeCode", ex);
            }
           
        }

        /// <summary>
        /// obtiene una lista  de DynamicConceptValues  a partir de dynamicConceptRelation
        /// </summary>
        /// <param name="dynamicConceptRelation"></param>
        /// <returns></returns>
        protected virtual IList ListDynamicConceptValues(DynamicConceptRelation dynamicConceptRelation)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(DynamicConceptValue.Properties.DynamicId);
                filter.Equal();
                filter.Constant(dynamicConceptRelation.DynamicId);

                IList dynamicConceptValues = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(DynamicConceptValue), filter.GetPredicate());

                return dynamicConceptValues;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListDynamicConceptValues", ex);
            }
           
        }

        /// <summary>
        /// obtiene un DynamicConceptRelation a aprtir de entity,primaryKey
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        protected virtual DynamicConceptRelation FindDynamicConceptRelation(PARAMEN.Entity entity, PrimaryKey primaryKey)
        {
            try
            {
                DynamicConceptRelation relation = null;

                IList keyConceptList = ConceptDAO.GetConceptsByEntity(entity);

                //BuildPredicate
                int keyCount = 5;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(DynamicConceptRelation.Properties.EntityId);
                filter.Equal();
                filter.Constant(entity.EntityId);

                string keyPropertyName;

                for (int index = 0; index < keyCount; index++)
                {
                    keyPropertyName = string.Format("Key{0}", index + 1);
                    filter.And();
                    filter.Property(keyPropertyName);
                    if (keyConceptList.Count > index)
                    {
                        SCREN.Concept keyConcept = (SCREN.Concept)keyConceptList[index];
                        filter.Equal();
                        filter.Constant(primaryKey[keyConcept.ConceptName]);
                    }
                    else
                    {
                        filter.IsNull();
                    }
                }

                IList dynamicConceptRelations = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(DynamicConceptRelation), filter.GetPredicate());

                if (dynamicConceptRelations != null && dynamicConceptRelations.Count == 1)
                {
                    relation = (DynamicConceptRelation)dynamicConceptRelations[0];
                }

                return relation;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindDynamicConceptRelation", ex);
            }
           
        }


        /// <summary>
        /// obtiene un GetValueDynamicConcept a partir del conceptId, entityId
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static object GetValueDynamicConceptByConceptIdEntiteId(int conceptId, int entityId)
        {
            
            try
            {
                SCREN.Concept concept = ConceptDAO.GetConceptByConceptIdEntityId(conceptId, entityId);
                if (concept != null)
                {
                    object conceptValue;
                    switch ((ConceptType.Types)concept.ConceptTypeCode)
                    {
                        case ConceptType.Types.Basic:
                            #region Valor por default para concepto de tipo basic
                            SCREN.BasicConcept basicConcept = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(conceptId, entityId);
                            switch ((BasicType.Types)basicConcept.BasicTypeCode)
                            {
                                case BasicType.Types.Number:
                                    conceptValue = 0;
                                    break;
                                case BasicType.Types.Text:
                                    conceptValue = string.Empty;
                                    break;
                                case BasicType.Types.Decimal:
                                    conceptValue = (decimal)0;
                                    break;
                                case BasicType.Types.Date:
                                    conceptValue = new DateTime();
                                    break;
                                case BasicType.Types.Long:
                                    conceptValue = (long)0;
                                    break;
                                case BasicType.Types.Boolean:
                                    conceptValue = false;
                                    break;
                                default:
                                    conceptValue = null;
                                    break;
                            }
                            #endregion
                            break;
                        case ConceptType.Types.Range:
                            conceptValue = 0;
                            break;
                        case ConceptType.Types.List:
                            ListConcept listConcept = ListConceptDAO.GetListConceptByConceptIdEntityId(conceptId, entityId);
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.Property(SCREN.ListEntityValue.Properties.ListEntityCode);
                            filter.Equal();
                            filter.Constant(listConcept.ListEntityCode);
                            BusinessCollection listEntityValue = DataFacadeManager.Instance.GetDataFacade().List(typeof(SCREN.ListEntityValue), filter.GetPredicate(), new string[] { SCREN.ListEntityValue.Properties.ListValueCode });
                            if (listEntityValue.Count > 0)
                            {
                                conceptValue = ((SCREN.ListEntityValue)listEntityValue[0]).ListValueCode;
                            }
                            else
                            {
                                conceptValue = 0;
                            }
                            if (listConcept.ListEntityCode == 2)
                            {
                                conceptValue = Convert.ToBoolean(conceptValue);
                            }
                            break;
                        case ConceptType.Types.Reference:
                            ReferenceConcept referenceConcept = (ReferenceConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(ReferenceConcept.CreatePrimaryKey(conceptId, entityId));
                            if (referenceConcept != null)
                            {
                                conceptValue = referenceConcept.FentityId;
                            }
                            else
                            {
                                conceptValue = 0;
                            }
                            break;
                        default:
                            conceptValue = null;
                            break;
                    }

                    if (concept.IsNullable)
                    {
                        conceptValue = null;
                    }
                    else if (!concept.IsStatic)
                    {
                        conceptValue = MapHelper.ChangeType(conceptValue, conceptValue.GetType());
                    }

                    return conceptValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetValueDynamicConceptByConceptIdEntiteId", ex);
            }
        }
    }
}
