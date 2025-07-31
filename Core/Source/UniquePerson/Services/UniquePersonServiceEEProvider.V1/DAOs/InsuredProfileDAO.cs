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

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class InsuredProfileDAO
    {
        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<InsuredProfile> GetInsuredProfiles()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredProfile)));
            List<InsuredProfile> insuredProfiles = ModelAssembler.CreateInsuredProfiles(businessCollection);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePerson.Providers.GetInsuredProfiles");
            return insuredProfiles;
        }

        /// <summary>
        /// Obtiene el ID maximo para realizar el ingreso del nuevos Perfil de Asegurado.
        /// </summary>
        /// <returns>ID maximo</returns>
        public int GetIdInsuredProfile()
        {
            int maxInsuredProfile = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredProfile))).Cast<UniquePersonV1.Entities.InsuredProfile>().Max(x => x.InsProfileCode);
            maxInsuredProfile++;
            return maxInsuredProfile;
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los Perfiles de Asegurado.
        /// </summary>
        /// <param name="insuredProfilesAdded"> Lista de InsuredProfile(Perfiles de Asegurado) para ser agregados</param>
        /// <param name="insuredProfilesEdited">Lista de InsuredProfile(Perfiles de Asegurado) para ser modificados</param>
        /// <param name="insuredProfilesDeleted">Lista de InsuredProfile(Perfiles de Asegurado) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados</returns>
        public ParametrizationResponse<InsuredProfile> SaveInsuredSegments(List<InsuredProfile> insuredProfilesAdded, List<InsuredProfile> insuredProfilesEdited, List<InsuredProfile> insuredProfilesDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<InsuredProfile> returnInsuredProfiles = new ParametrizationResponse<InsuredProfile>();
            stopWatch.Start();
            using (Context.Current)
            {
                if (insuredProfilesAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (InsuredProfile item in insuredProfilesAdded)
                            {
                                item.Id = this.GetIdInsuredProfile();
                                UniquePersonV1.Entities.InsuredProfile entityInsuredProfile = EntityAssembler.CreateInsuredProfile(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityInsuredProfile);
                            }

                            transaction.Complete();
                            returnInsuredProfiles.TotalAdded = insuredProfilesAdded.Count;
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnInsuredProfiles.ErrorAdded = "ErrorSaveInsuredProfilessAdded";
                        }
                    }
                }

                if (insuredProfilesEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in insuredProfilesEdited)
                            {
                                PrimaryKey key = UniquePersonV1.Entities.InsuredProfile.CreatePrimaryKey(item.Id);
                                UniquePersonV1.Entities.InsuredProfile insuredProfileEntity = new UniquePersonV1.Entities.InsuredProfile(item.Id);
                                insuredProfileEntity = (UniquePersonV1.Entities.InsuredProfile)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                insuredProfileEntity.Description = item.Description;
                                insuredProfileEntity.SmallDescription = item.SmallDescription;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(insuredProfileEntity);
                            }

                            transaction.Complete();
                            returnInsuredProfiles.TotalModify = insuredProfilesEdited.Count;
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnInsuredProfiles.ErrorModify = "ErrorSaveInsuredProfilessModify";
                        }
                    }
                }

                if (insuredProfilesDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.Property(UniquePersonV1.Entities.InsuredProfile.Properties.InsProfileCode).In().ListValue();
                            insuredProfilesDeleted.ForEach(x => filter.Constant(x.Id));
                            filter.EndList();
                            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(UniquePersonV1.Entities.InsuredProfile), filter.GetPredicate());
                            transaction.Complete();
                            returnInsuredProfiles.TotalDeleted = insuredProfilesDeleted.Count;
                        }
                        catch (ForeignKeyException ex)
                        {
                            transaction.Dispose();
                            returnInsuredProfiles.ErrorDeleted = "ErrorSaveInsuredProfilessRelated";
                        }
                        catch (RelatedObjectException ex)
                        {
                            transaction.Dispose();
                            returnInsuredProfiles.ErrorDeleted = "ErrorSaveInsuredProfilessRelated";
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            returnInsuredProfiles.ErrorDeleted = "ErrorSaveInsuredProfilessDeleted";
                        }
                    }
                }

                returnInsuredProfiles.ReturnedList = this.GetInsuredProfiles();
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonServices.Providers.SaveInsuredProfiles");
            return returnInsuredProfiles;
        }
    }
}
