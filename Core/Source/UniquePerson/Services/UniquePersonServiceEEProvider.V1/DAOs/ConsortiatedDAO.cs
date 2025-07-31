using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class ConsortiatedDAO
    {
        /// <summary>
        /// Obtiene Consorcio Por asegurdo y Por InvidualID
        /// </summary>
        /// <param name="InsuredId">Representa el Id del Asegurado</param>
        /// <param name="IndividualId">Representa el Id del Inviduo</param>
        /// <returns>retorna la Respuesta del Consorcio</returns>
        public virtual Models.Consortium GetConsortiumByInsurendIdOnInvidualId(int InsuredId, int IndividualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoConsortium.Properties.InsuredCode, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(InsuredId);
            filter.And();
            filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(IndividualId);

            CoConsortium coConsortium = (CoConsortium)DataFacadeManager.Instance.GetDataFacade().List(typeof(CoConsortium), filter.GetPredicate()).FirstOrDefault();
            Models.Consortium consortiumModel = ModelAssembler.CreateConsortiums(coConsortium);
            if (coConsortium != null)
            {
                return consortiumModel;
            }
            return null;
        }
        /// <summary>
        /// Obtiene el Consorcio Por Asegurado
        /// </summary>
        /// <param name="insuredCode">Representa el id del Asegurado </param>
        /// <returns></returns>
        public List<Models.Consortium> GetCoConsortiumsByInsuredCode(int insuredCode)
        {
            Entities.views.ConsorcioViewCoV1 view = new Entities.views.ConsorcioViewCoV1();
            ViewBuilder builder = new ViewBuilder("CoConsorcioViewV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Insured.Properties.InsuredCode, typeof(Insured).Name);
            filter.Equal();
            filter.Constant(insuredCode);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.Consortium> consortiums = ModelAssembler.CreateCoConsortiums(view);
            if (consortiums.Count > 0)
            {
                return consortiums;
            }
            return null;
        }
        /// <summary>
        /// Crea uno o varios Consorciados   
        /// </summary>
        /// <param name="consortium">Datos del consorciado</param>
        /// <returns>Retorna Si la entidad ya se encuentra llena si no envia un null.</returns>
        public virtual List<Models.Consortium> CreateConsortium(List<Models.Consortium> consortium)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };

            using (Transaction transaction = new Transaction())
            {
                if (consortium != null)
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {

                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {

                    };
                    try
                    {
                        foreach (Models.Consortium item in consortium)
                        {
                            CoConsortium coConsortiumEntity = EntityAssembler.CreateConsortium(item);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(coConsortiumEntity);
                        }
                        transaction.Complete();
                        stopWatch.Stop();
                        return consortium;
                    }
                    catch (Exception ex)
                    {
                        stopWatch.Stop();
                        transaction.Dispose();
                        throw new BusinessException("Error in Create Consortium", ex);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Actualiza los consorcios 
        /// </summary>
        /// <param name="consortium">Datos del Consorcio</param>
        /// <returns>Retrona la informacion a Actualizar</returns>
        public virtual Models.Consortium UpdateConsortium(Models.Consortium consortium)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (consortium != null)
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name);
                    filter.Equal();
                    filter.Constant(consortium.IndividualId);
                    filter.And();
                    filter.Property(CoConsortium.Properties.InsuredCode, typeof(CoConsortium).Name);
                    filter.Equal();
                    filter.Constant(consortium.InsuredCode);

                    CoConsortium coConsortiumEntity = EntityAssembler.CreateConsortium(consortium);
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(coConsortiumEntity);
                    var result = ModelAssembler.CreateConsortiums(coConsortiumEntity);
                    return result;
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    throw new BusinessException("Error in Update Consortium", ex);
                }

            }

            return null;
        }

        /// <summary>
        /// Elimina el consorciado
        /// </summary>
        /// <param name="consortium"></param>
        /// <returns></returns>
        public bool DeleteConsortium(int InsuredCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CoConsortium.Properties.InsuredCode, InsuredCode);
            int pru = DataFacadeManager.Instance.GetDataFacade().Delete<CoConsortium>(filter.GetPredicate());
            if (pru > 0)
            {
                return true;
            }
            return false;
        }

        public List<Models.Consortium> GetConsortiumsByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(individualId);

            return ModelAssembler.CreateCoConsortiums(DataFacadeManager.GetObjects(typeof(CoConsortium), filter.GetPredicate()));
        }
        /// <summary>
        /// Consulta la UP.UserAssignedConsortium por usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Models.UserAssignedConsortium GetUserAssignedConsortiumByuserId(int userId)
        {
            Models.UserAssignedConsortium userAssignedConsortium = new Models.UserAssignedConsortium();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UserAssignedConsortium.Properties.UserId, typeof(UserAssignedConsortium).Name, userId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UserAssignedConsortium), filter.GetPredicate());
            List<Models.UserAssignedConsortium> userAssignedConsortia = ModelAssembler.CreateUserAssignedConsortiums(businessObjects);
            if (userAssignedConsortia.Count > 0)
            {
                return userAssignedConsortia.First();
            }
            return userAssignedConsortium;
        }
    
        /// <summary>
        /// Inserta en la UP.UserassignedConsortium
        /// </summary>
        /// <param name="userAssignedConsortium"></param>
        /// <returns></returns>
        public Models.UserAssignedConsortium InsertUserAssignedConsortium(Models.UserAssignedConsortium userAssignedConsortium)
        {
            Models.UserAssignedConsortium assignedConsortium = new Models.UserAssignedConsortium();
            UserAssignedConsortium entityUserAssignedConsortium = EntityAssembler.CreateUserAssignedConsortium(userAssignedConsortium);
            DataFacadeManager.Insert(entityUserAssignedConsortium);
            assignedConsortium = ModelAssembler.CreateUserAssignedConsortium(entityUserAssignedConsortium);
            return assignedConsortium;
        }
        /// <summary>
        ///  Elimina y actualiza el parametro del documento de tipos de consorcio
        /// </summary>
        /// <param name="parameterFutureSociety"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteUserAssignedConsortium(int parameterFutureSociety, int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UserAssignedConsortium.Properties.UserId, userId);
            int delete = DataFacadeManager.Instance.GetDataFacade().Delete<UserAssignedConsortium>(filter.GetPredicate());
            return delete;
        }

        public Models.UserAssignedConsortium GetUserAssignedConsortiumByNitAssignedConsortium(string NitAssignedConsortium)
        {
            Models.UserAssignedConsortium userAssignedConsortium = new Models.UserAssignedConsortium();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UserAssignedConsortium.Properties.NitAssociationType, typeof(UserAssignedConsortium).Name, NitAssignedConsortium);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UserAssignedConsortium), filter.GetPredicate());
            List<Models.UserAssignedConsortium> userAssignedConsortia = ModelAssembler.CreateUserAssignedConsortiums(businessObjects);
            if (userAssignedConsortia.Count > 0)
            {
                return userAssignedConsortia.First();
            }
            return userAssignedConsortium;
        }
    }
}
