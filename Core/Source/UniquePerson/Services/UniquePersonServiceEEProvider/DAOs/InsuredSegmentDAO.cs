// -----------------------------------------------------------------------
// <copyright file="InsuredSegmentDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;    
    using Sistran.Core.Application.UniquePersonService.Assemblers;        
    using Sistran.Core.Application.UniquePersonService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using COMMMOD = Sistran.Core.Application.CommonService.Models;

    /// <summary>
    /// Clase Dao del objeto Insured Segment
    /// </summary>
    public class InsuredSegmentDAO
    {
        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<InsuredSegment> GetInsuredSegments()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.InsuredSegment)));
            List<InsuredSegment> insuredSegments = ModelAssembler.CreateInsuredSegments(businessCollection);
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePerson.Providers.GetInsuredSegments");
            return insuredSegments;
        }

        /// <summary>
        /// Obtiene el ID maximo para realizar el ingreso del nuevos Perfil de Asegurado.
        /// </summary>
        /// <returns>ID maximo</returns>
        public int GetIdInsuredSegment()
        {
            int maxInsuredSegment = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.InsuredSegment))).Cast<UniquePerson.Entities.InsuredSegment>().Max(x => x.InsSegmentCode);
            maxInsuredSegment++;
            return maxInsuredSegment;
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los Perfiles de Asegurado.
        /// </summary>
        /// <param name="insuredSegmentsAdded"> Lista de InsuredSegment(Perfiles de Asegurado) para ser agregados</param>
        /// <param name="insuredSegmentsEdited">Lista de InsuredSegment(Perfiles de Asegurado) para ser modificados</param>
        /// <param name="insuredSegmentsDeleted">Lista de InsuredSegment(Perfiles de Asegurado) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados</returns>
        public ParametrizationResponse<InsuredSegment> SaveInsuredSegments(List<InsuredSegment> insuredSegmentsAdded, List<InsuredSegment> insuredSegmentsEdited, List<InsuredSegment> insuredSegmentsDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<InsuredSegment> returnInsuredSegments = new ParametrizationResponse<InsuredSegment>();
            stopWatch.Start();
            using (Context.Current)
            {                
                if (insuredSegmentsAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (InsuredSegment item in insuredSegmentsAdded)
                            {
                                item.Id = this.GetIdInsuredSegment();
                                UniquePerson.Entities.InsuredSegment entityInsuredSegment = EntityAssembler.CreateInsuredSegment(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityInsuredSegment);
                            }

                            transaction.Complete();
                            returnInsuredSegments.TotalAdded = insuredSegmentsAdded.Count;
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnInsuredSegments.ErrorAdded = "ErrorSaveInsuredSegmentssAdded";
                        }
                    }
                }
                                                
                if (insuredSegmentsEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in insuredSegmentsEdited)
                            {
                                PrimaryKey key = UniquePerson.Entities.InsuredSegment.CreatePrimaryKey(item.Id);
                                UniquePerson.Entities.InsuredSegment insuredSegmentEntity = new UniquePerson.Entities.InsuredSegment(item.Id);
                                insuredSegmentEntity = (UniquePerson.Entities.InsuredSegment)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                insuredSegmentEntity.Description = item.LongDescription;
                                insuredSegmentEntity.SmallDescription = item.ShortDescription;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(insuredSegmentEntity);
                            }

                            transaction.Complete();
                            returnInsuredSegments.TotalModify = insuredSegmentsEdited.Count;
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnInsuredSegments.ErrorModify = "ErrorSaveInsuredSegmentssModify";
                        }
                    }
                }
                                                
                if (insuredSegmentsDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.Property(UniquePerson.Entities.InsuredSegment.Properties.InsSegmentCode).In().ListValue();
                            insuredSegmentsDeleted.ForEach(x => filter.Constant(x.Id));
                            filter.EndList();
                            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(UniquePerson.Entities.InsuredSegment), filter.GetPredicate());
                            transaction.Complete();
                            returnInsuredSegments.TotalDeleted = insuredSegmentsDeleted.Count;
                        }
                        catch (ForeignKeyException ex)
                        {
                            transaction.Dispose();
                            returnInsuredSegments.ErrorDeleted = "ErrorSaveInsuredSegmentssRelated";
                        }
                        catch (RelatedObjectException ex)
                        {
                            transaction.Dispose();
                            returnInsuredSegments.ErrorDeleted = "ErrorSaveInsuredSegmentssRelated";
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnInsuredSegments.ErrorDeleted = "ErrorSaveInsuredSegmentssDeleted";
                        }
                    }
                }
                
                returnInsuredSegments.ReturnedList = this.GetInsuredSegments();
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonServices.Providers.SaveInsuredSegments");
            return returnInsuredSegments;
        }
    }
}
