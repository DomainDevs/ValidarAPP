using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class InsuredSegmentDAO
    {
        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<InsuredSegmentV1> GetInsuredSegments()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.InsuredSegment)));
            List<InsuredSegmentV1> insuredSegments = ModelAssembler.CreateInsuredSegments(businessCollection);

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
            int maxInsuredSegment = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.InsuredSegment))).Cast<entities.InsuredSegment>().Max(x => x.InsSegmentCode);
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
        public ParametrizationResponse<InsuredSegmentV1> SaveInsuredSegments(List<InsuredSegmentV1> insuredSegmentsAdded, List<InsuredSegmentV1> insuredSegmentsEdited, List<InsuredSegmentV1> insuredSegmentsDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<InsuredSegmentV1> returnInsuredSegments = new ParametrizationResponse<InsuredSegmentV1>();
            stopWatch.Start();
            using (Context.Current)
            {
                if (insuredSegmentsAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (InsuredSegmentV1 item in insuredSegmentsAdded)
                            {
                                item.Id = this.GetIdInsuredSegment();
                                entities.InsuredSegment entityInsuredSegment = EntityAssembler.CreateInsuredSegment(item);
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
                                PrimaryKey key = entities.InsuredSegment.CreatePrimaryKey(item.Id);
                                entities.InsuredSegment insuredSegmentEntity = new entities.InsuredSegment(item.Id);
                                insuredSegmentEntity = (entities.InsuredSegment)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
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
                            filter.Property(entities.InsuredSegment.Properties.InsSegmentCode).In().ListValue();
                            insuredSegmentsDeleted.ForEach(x => filter.Constant(x.Id));
                            filter.EndList();
                            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(entities.InsuredSegment), filter.GetPredicate());
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


