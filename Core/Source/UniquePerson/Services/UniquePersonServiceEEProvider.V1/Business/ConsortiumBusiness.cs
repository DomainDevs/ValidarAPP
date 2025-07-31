using AUPE = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using AUPS = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF;
using System.Data;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ConsortiumBusiness
    {
        public List<AUPS.Consortium> CreateConsortiums(List<AUPS.Consortium> consortiums)
        {
            return consortiums.Select(CreateConsortium).ToList();
        }

        public AUPS.Consortium CreateConsortium(AUPS.Consortium consortium)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            using (Transaction transaction = new Transaction())
            {
                if (consortium != null)
                {

                    try
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.Property(AUPE.CoConsortium.Properties.InsuredCode);
                        filter.Equal();
                        filter.Constant(consortium.InsuredCode);
                        SelectQuery selectQuery = new SelectQuery();
                        Function funtion = new Function(FunctionType.Max);

                        funtion.AddParameter(new Column(AUPE.CoConsortium.Properties.ConsortiumId));

                        selectQuery.Table = new ClassNameTable(typeof(AUPE.CoConsortium), "CoConsortium");
                        selectQuery.AddSelectValue(new SelectValue(funtion));
                        selectQuery.Where = filter.GetPredicate();
                        using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                        {
                            while (reader.Read())
                            {
                                consortium.ConsortiumId = (Convert.ToInt32(reader[0]) + 1);
                            }
                        }

                        AUPE.CoConsortium coConsortiumEntity = EntityAssembler.CreateConsortium(consortium);
                        DataFacadeManager.Insert(coConsortiumEntity);

                        transaction.Complete();
                        stopWatch.Stop();
                        return consortium;
                    }
                    catch (Exception)
                    {
                        stopWatch.Stop();
                        transaction.Dispose();
                        throw new BusinessException(Resources.Errors.ErrorCreateConsortium);
                    }
                }
            }
            return null;
        }

        public virtual Models.Consortium GetConsortiumByInsurendIdOnInvidualId(int InsuredId, int IndividualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AUPE.CoConsortium.Properties.InsuredCode, typeof(AUPE.CoConsortium).Name);
            filter.Equal();
            filter.Constant(InsuredId);
            filter.And();
            filter.Property(AUPE.CoConsortium.Properties.IndividualId, typeof(AUPE.CoConsortium).Name);
            filter.Equal();
            filter.Constant(IndividualId);

            AUPE.CoConsortium coConsortium = (AUPE.CoConsortium)DataFacadeManager.Instance.GetDataFacade().List(typeof(AUPE.CoConsortium), filter.GetPredicate()).FirstOrDefault();
            Models.Consortium consortiumModel = ModelAssembler.CreateConsortiums(coConsortium);
            if (coConsortium != null)
            {
                return consortiumModel;
            }
            return null;
        }

        public List<Models.Consortium> GetCoConsortiumsByInsuredCode(int insuredCode)
        {
            Entities.views.ConsorcioViewCoV1 view = new Entities.views.ConsorcioViewCoV1();
            ViewBuilder builder = new ViewBuilder("ConsorcioViewCoV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AUPE.Insured.Properties.InsuredCode, typeof(AUPE.Insured).Name);
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

        public virtual Models.Consortium UpdateConsortium(Models.Consortium consortium)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (consortium != null)
            {
                try
                {
                    PrimaryKey primaryKey = AUPE.CoConsortium.CreatePrimaryKey(consortium.InsuredCode, consortium.IndividualId);

                    AUPE.CoConsortium coConsortiumEntity = (AUPE.CoConsortium)DataFacadeManager.GetObject(primaryKey);
                    //coConsortiumEntity.InsuredCode = consortium.InsuredCode;
                    //coConsortiumEntity.ConsortiumId = consortium.ConsortiumId;
                    coConsortiumEntity.Enabled = consortium.Enabled;
                    coConsortiumEntity.IsMain = consortium.Ismain;
                    coConsortiumEntity.ParticipationRate = consortium.ParticipationRate;
                    coConsortiumEntity.StartDate = consortium.StartDate;

                    DataFacadeManager.Update(coConsortiumEntity);
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

        public bool DeleteConsortium(Models.Consortium coConsortiums)
        {
            PrimaryKey key = AUPE.CoConsortium.CreatePrimaryKey(coConsortiums.InsuredCode, coConsortiums.IndividualId);
            var consortium = (AUPE.CoConsortium)DataFacadeManager.GetObject(key);
            if (consortium != null)
            {
                DataFacadeManager.Delete(key);
                return true;
            }
            return false;
        }

        #region Consortium

        public Models.Consortium GetConsortiumByIndividualId(int IndividualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Models.Consortium consortium = new Models.Consortium();
            filter.PropertyEquals(AUPE.CoConsortium.Properties.IndividualId, typeof(AUPE.CoConsortium).Name, IndividualId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(AUPE.CoConsortium), filter.GetPredicate());
            List<Models.Consortium> consortiums = ModelAssembler.CreateCoConsortiums(businessObjects);
            if (consortiums.Count > 0)
            {
                consortium = consortiums.Last();
            }
            return consortium;
        }
        #endregion

        /// <summary>
        /// Consulta la UP.UserAssignedConsortium por usuario
        /// </summary>
        /// <param name="parameterFutureSociety"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Models.UserAssignedConsortium GetUserAssignedConsortiumByparameterFutureSocietyByuserId(int parameterFutureSociety, int userId)
        {
            Parameter parameter = new Parameter();
            Parameter updateParameter = new Parameter();
            ConsortiatedDAO consortiatedDAO = new ConsortiatedDAO();
            Models.UserAssignedConsortium userAssignedConsortium = new Models.UserAssignedConsortium();
            Models.UserAssignedConsortium userAssigned = new Models.UserAssignedConsortium();
            //Consulta de Parametro
            parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterFutureSociety);
            //Consulta a tabla
            userAssignedConsortium = consortiatedDAO.GetUserAssignedConsortiumByuserId(userId);
            if (userAssignedConsortium.NitAssignedConsortium != null)
            {
                userAssigned = consortiatedDAO.GetUserAssignedConsortiumByNitAssignedConsortium(userAssignedConsortium.NitAssignedConsortium);
            }
            if (userAssigned.NitAssignedConsortium != null && userAssigned.UserId > 0 && userAssigned.UserId != userId)
            {
                updateParameter = parameter;
                updateParameter.NumberParameter = updateParameter.NumberParameter + 1;
                updateParameter.Id = parameterFutureSociety;
                parameter = DelegateService.commonServiceCore.UpdateParameter(updateParameter);
                userAssignedConsortium = new Models.UserAssignedConsortium();
            }
            if (userAssignedConsortium.NitAssignedConsortium == null && userAssignedConsortium.UserId != userId)
            {
                //Insercion a tabla
                userAssignedConsortium.UserId = userId;
                userAssignedConsortium.NitAssignedConsortium = Convert.ToString(parameter.NumberParameter);
                userAssignedConsortium = consortiatedDAO.InsertUserAssignedConsortium(userAssignedConsortium);
                updateParameter = parameter;
                updateParameter.NumberParameter = updateParameter.NumberParameter + 1;
                updateParameter.Id = parameterFutureSociety;
                DelegateService.commonServiceCore.UpdateParameter(updateParameter);
                return userAssignedConsortium;
            }
            return userAssignedConsortium;

        }

        public bool DeleteUserAssignedConsortium(int parameterFutureSociety, int userId)
        {
            ConsortiatedDAO consortiatedDAO = new ConsortiatedDAO();
            Models.UserAssignedConsortium userAssignedConsortium = new Models.UserAssignedConsortium();
            Parameter parameter = new Parameter();
            Parameter updateParameter = new Parameter();
            userAssignedConsortium = consortiatedDAO.GetUserAssignedConsortiumByuserId(userId);
            int delete = consortiatedDAO.DeleteUserAssignedConsortium(parameterFutureSociety, userId);
            if (delete > 0)
            {
                parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterFutureSociety);
                updateParameter = parameter;
                updateParameter.NumberParameter = updateParameter.NumberParameter + 1;
                updateParameter.Id = parameterFutureSociety;
                DelegateService.commonServiceCore.UpdateParameter(updateParameter);
                return true;
            }
            return false;
        }
    }
}
