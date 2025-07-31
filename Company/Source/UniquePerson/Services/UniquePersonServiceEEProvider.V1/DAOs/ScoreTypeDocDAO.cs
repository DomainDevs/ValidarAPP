// -----------------------------------------------------------------------
// <copyright file="ScoreTypeDocDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonServices.V1.DAOs
{
    using Sistran.Company.Application.UniquePerson.Entities;
    using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    /// <summary>
    /// Contiene los procedimientos del tipo de documento datacrédito para la capa de datos 
    /// </summary>
    public class ScoreTypeDocDAO
    {
        /// <summary>
        /// Crear un nuevo tipo documento datacrédito
        /// </summary>
        /// <param name="scoreTypeDoc">Datos tipo documento datacrédito</param>
        /// <returns>Tipo de documento datacrédito creado</returns>
        public Models.ScoreTypeDoc CreateScoreTypeDoc(Models.ScoreTypeDoc scoreTypeDoc)
        {
            CompanyScoreTypeDoc scoreTypeDocEntity = EntityAssembler.CreateScoreTypeDoc(scoreTypeDoc);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(scoreTypeDocEntity);
            return ModelAssembler.CreateScoreTypeDoc(scoreTypeDocEntity);
        }

        /// <summary>
        ///  Actualizar tipo documento datacrédito
        /// </summary>
        /// <param name="scoreTypeDoc">Modelo tipo documento datacrédito</param>
        /// <returns>Tipo de documento datacrédito actualizado</returns>
        public Models.ScoreTypeDoc UpdateScoreTypeDoc(Models.ScoreTypeDoc scoreTypeDoc)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CompanyScoreTypeDoc.Properties.IdCardTypeScore, typeof(CompanyScoreTypeDoc).Name);
            filter.Equal();
            filter.Constant(scoreTypeDoc.IdCardTypeScore);

            List<Models.ScoreTypeDoc> scoreTypeDocs = this.GetScoreTypeDocByFilter(filter);

            if (scoreTypeDocs.Count > 0)
            {
                PrimaryKey key = CompanyScoreTypeDoc.CreatePrimaryKey(scoreTypeDoc.IdCardTypeScore);
                CompanyScoreTypeDoc scoreTypeDocEntity = (CompanyScoreTypeDoc)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                scoreTypeDocEntity.Description = scoreTypeDoc.Description;
                scoreTypeDocEntity.SmallDescription = scoreTypeDoc.SmallDescription;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(scoreTypeDocEntity);
                return ModelAssembler.CreateScoreTypeDoc(scoreTypeDocEntity);
            }
            else
            {
                return this.CreateScoreTypeDoc(scoreTypeDoc);
            }
        }

        /// <summary>
        /// Obtiene el tipo de documento datacrédito por identificador del tipo de documento
        /// </summary>
        /// <param name="idCardTypeScore">identificador del tipo de documento buscado.</param>
        /// <returns>Tipo de documento datacrédito obtenido</returns>
        public List<Models.ScoreTypeDoc> GetScoreTypeDocByIdCardTypeScore(int idCardTypeScore)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CompanyScoreTypeDoc.Properties.IdCardTypeScore, idCardTypeScore);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyScoreTypeDoc), filter.GetPredicate()));
            List<Models.ScoreTypeDoc> scoreTypeDoc = ModelAssembler.CreateScoreTypeDoc(businessCollection);
            return scoreTypeDoc;
        }

        /// <summary>
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito creados</returns>
        public List<Models.ScoreTypeDoc> GetAllScoreTypeDoc()
        {
            List<Models.ScoreTypeDoc> scoreTypeDocs = new List<Models.ScoreTypeDoc>();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyScoreTypeDoc)));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                scoreTypeDocs = ModelAssembler.CreateScoreTypeDoc(businessCollection);
            }

            foreach (Models.ScoreTypeDoc item in scoreTypeDocs)
            {
                List<Models.ScoreTypeDoc> score3gTypeDocObt = new List<Models.ScoreTypeDoc>();
                score3gTypeDocObt = this.GetScore3gTypeDocByIdCardTypeScore(item.IdCardTypeScore);
                if (score3gTypeDocObt != null && score3gTypeDocObt.Count > 0)
                {
                    item.IdScore3g = score3gTypeDocObt[0].IdScore3g;
                    item.IdCardTypeCode = score3gTypeDocObt[0].IdCardTypeCode;
                }
            }

            return scoreTypeDocs;
        }

        /// <summary>
        /// Elimina el tipo de documento datacrédito relacionado
        /// </summary>
        /// <param name="idCardTypeScore">Id del tipo documento datacrédito</param>
        /// <returns>Resultado de la eliminación</returns>
        public bool DeleteScoreTypeDoc(int idCardTypeScore)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CompanyScoreTypeDoc.Properties.IdCardTypeScore, idCardTypeScore);
                DataFacadeManager.Instance.GetDataFacade().Delete<CompanyScoreTypeDoc>(filter.GetPredicate());
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el tipo de documento datacrédito por filtro
        /// </summary>
        /// <param name="filter">Filtro realizado</param>
        /// <returns>Resultado del filtro aplicado</returns>
        public List<Models.ScoreTypeDoc> GetScoreTypeDocByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyScoreTypeDoc), filter.GetPredicate()));
            return ModelAssembler.CreateScoreTypeDoc(businessCollection);
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los tipos de documento datacrédito
        /// </summary>
        /// <param name="scoreTypeDocsAdded">Lista de tipos de documento datacrédito agregados</param>
        /// <param name="scoreTypeDocsEdited">Lista de tipos de documento datacrédito editados</param>
        /// <param name="scoreTypeDocsDeleted">Lista de tipos de documento datacrédito eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados</returns>
        public ParametrizationResponse<Models.ScoreTypeDoc> SaveScoreTypeDocs(List<Models.ScoreTypeDoc> scoreTypeDocsAdded, List<Models.ScoreTypeDoc> scoreTypeDocsEdited, List<Models.ScoreTypeDoc> scoreTypeDocsDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<Models.ScoreTypeDoc> returnScoreTypeDocs = new ParametrizationResponse<Models.ScoreTypeDoc>();
            stopWatch.Start();
            using (Context.Current)
            {
                // Agregar
                if (scoreTypeDocsAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (Models.ScoreTypeDoc item in scoreTypeDocsAdded)
                            {
                                CompanyScoreTypeDoc entityScoreTypeDoc = EntityAssembler.CreateScoreTypeDoc(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityScoreTypeDoc);
                                if (item.IdCardTypeCode > 0)
                                {
                                    item.IdScore3g = this.GetIdScore3g();
                                    CompanyScore3gTypeDoc entityScore3gTypeDoc = EntityAssembler.CreateScore3gTypeDoc(item);
                                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entityScore3gTypeDoc);
                                }
                            }

                            transaction.Complete();
                            returnScoreTypeDocs.TotalAdded = scoreTypeDocsAdded.Count;
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnScoreTypeDocs.ErrorAdded = "ErrorSaveScoreTypeDocsAdded";
                        }
                    }
                }

                // Modificar
                if (scoreTypeDocsEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (Models.ScoreTypeDoc item in scoreTypeDocsEdited)
                            {
                                PrimaryKey key = CompanyScoreTypeDoc.CreatePrimaryKey(item.IdCardTypeScore);
                                CompanyScoreTypeDoc scoreTypeDocEntity = new CompanyScoreTypeDoc(item.IdCardTypeScore);
                                scoreTypeDocEntity = (CompanyScoreTypeDoc)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                scoreTypeDocEntity.Description = item.Description;
                                scoreTypeDocEntity.SmallDescription = item.SmallDescription;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(scoreTypeDocEntity);

                                if (item.IdScore3g > 0)
                                {
                                    PrimaryKey keyScore3g = CompanyScore3gTypeDoc.CreatePrimaryKey(item.IdScore3g);
                                    CompanyScore3gTypeDoc score3gTypeDocEntity = new CompanyScore3gTypeDoc(item.IdScore3g);
                                    score3gTypeDocEntity = (CompanyScore3gTypeDoc)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyScore3g);
                                    score3gTypeDocEntity.IdCardTypeCode = item.IdCardTypeCode;
                                    score3gTypeDocEntity.IdCardTypeScore = item.IdCardTypeScore;
                                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(score3gTypeDocEntity);
                                }
                                else
                                {
                                    if (item.IdCardTypeCode > 0)
                                    {
                                        item.IdScore3g = this.GetIdScore3g();
                                        CompanyScore3gTypeDoc entityScore3gTypeDoc = EntityAssembler.CreateScore3gTypeDoc(item);
                                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entityScore3gTypeDoc);
                                    }
                                    else
                                    {
                                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                                        filter.PropertyEquals(CompanyScore3gTypeDoc.Properties.IdCardTypeScore, item.IdCardTypeScore);
                                        DataFacadeManager.Instance.GetDataFacade().Delete<CompanyScore3gTypeDoc>(filter.GetPredicate());
                                    }
                                }
                            }

                            transaction.Complete();
                            returnScoreTypeDocs.TotalModify = scoreTypeDocsEdited.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnScoreTypeDocs.ErrorModify = "ErrorSaveScoreTypeDocsEdited";
                        }
                    }
                }

                // Eliminar
                if (scoreTypeDocsDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (Models.ScoreTypeDoc item in scoreTypeDocsDeleted)
                            {
                                if (item.IdScore3g > 0)
                                {
                                    PrimaryKey keyScore3g = CompanyScore3gTypeDoc.CreatePrimaryKey(item.IdScore3g);
                                    CompanyScore3gTypeDoc score3gTypeDocEntity = new CompanyScore3gTypeDoc(item.IdScore3g);
                                    score3gTypeDocEntity = (CompanyScore3gTypeDoc)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyScore3g);
                                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(score3gTypeDocEntity);
                                }

                                PrimaryKey key = CompanyScoreTypeDoc.CreatePrimaryKey(item.IdCardTypeScore);
                                CompanyScoreTypeDoc scoreTypeDocEntity = new CompanyScoreTypeDoc(item.IdCardTypeScore);
                                scoreTypeDocEntity = (CompanyScoreTypeDoc)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(scoreTypeDocEntity);
                            }

                            transaction.Complete();
                            returnScoreTypeDocs.TotalDeleted = scoreTypeDocsDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnScoreTypeDocs.ErrorDeleted = "ErrorSaveScoreTypeDocsRelated";
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnScoreTypeDocs.ErrorDeleted = "ErrorSaveScoreTypeDocsRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnScoreTypeDocs.ErrorDeleted = "ErrorSaveScoreTypeDocsDeleted";
                        }
                    }
                }

                returnScoreTypeDocs.ReturnedList = this.GetAllScoreTypeDoc();
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.SaveScoreTypeDocs");
            return returnScoreTypeDocs;
        }

        /// <summary>
        /// Retorna el valor de la proxima key de la tabla Score3gTypeDoc
        /// </summary>
        /// <returns>Key dispobible</returns>
        public int GetIdScore3g()
        {
            int maxScore3gCode = 0;
            BusinessCollection lstScore3gTypeDoc = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                            .SelectObjects(typeof(CompanyScore3gTypeDoc)));

            if (lstScore3gTypeDoc.Count > 0)
            {
                maxScore3gCode = lstScore3gTypeDoc.Cast<CompanyScore3gTypeDoc>().Max(x => x.IdScore3g);

            }
            maxScore3gCode++;
            return maxScore3gCode;
        }

        /// <summary>
        /// Obtiene la asociación entre el tipo de documento SISE y el 
        /// tipo de documento datacrédito por el id del tipo documento datácredito
        /// </summary>
        /// <param name="idCardTypeScore">idetificador del tipo documento datcrédito</param>
        /// <returns>Resultado de la consulta</returns>
        public List<Models.ScoreTypeDoc> GetScore3gTypeDocByIdCardTypeScore(int idCardTypeScore)
        {
            List<Models.ScoreTypeDoc> score3gTypeDocObt = new List<Models.ScoreTypeDoc>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CompanyScore3gTypeDoc.Properties.IdCardTypeScore, idCardTypeScore);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyScore3gTypeDoc), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                foreach (CompanyScore3gTypeDoc item in businessCollection)
                {
                    score3gTypeDocObt = ModelAssembler.CreateScore3gTypeDoc(businessCollection);
                }
            }

            return score3gTypeDocObt;
        }
    }
}