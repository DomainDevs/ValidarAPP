using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Contexts;
using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Et = Sistran.Company.Application.MassiveServices.EEProvider.Entities;
using Sistran.Core.Framework.BAF;
using EtUnder = Sistran.Core.Application.UnderwritingServices.EEProvider.Entities;
using Sistran.Core.Application.UniquePersonService.DAOs;
using Sistran.Core.Application.UniquePersonService.Models;
using System.Diagnostics;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UnderwritingServices.Enums;


namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class CoRequestDAO
    {

        /// <summary>
        /// Guarda en base de datos una nueva solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public Models.CoRequest SaveCoRequest(Models.CoRequest request, int userId)
        {


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Transaction.Created += delegate(object sender, TransactionEventArgs e) { };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate(object sender, TransactionEventArgs e) { };
                    transaction.Disposed += delegate(object sender, TransactionEventArgs e) { };

                    try
                    {
                        Models.CoRequest coRequest = CreateNewCoRequest(request);

                        Models.CoRequestEndorsement coRequestEndorsement = coRequest.CoRequestEndorsement.FirstOrDefault();

                        CoRequestEndorsementDAO requestEndorsementDAO = new CoRequestEndorsementDAO();
                        requestEndorsementDAO.CreateCoRequestEndorsement(coRequestEndorsement, userId, coRequest.Id);

                        CoRequestAgentDAO requestAgentDAO = new CoRequestAgentDAO();
                        requestAgentDAO.CreateAgents(coRequestEndorsement.CoRequestAgent, coRequestEndorsement.Id);


                        transaction.Complete();

                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.SaveCoRequest");

                        return coRequest;
                    }

                    catch (Exception exception)
                    {
                        throw new BusinessException("SaveCoRequest", exception);
                    }

                }
            }

        }

        /// <summary>
        /// Guarda en base de datos Renovacion solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public Models.CoRequest SaveRenewalRequest(Models.CoRequest request, int userId)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        PrimaryKey key = Entities.CoRequest.CreatePrimaryKey(request.Id);
                        Entities.CoRequest coRequestEntity = (Entities.CoRequest)MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(key);
                        if (coRequestEntity != null)
                        {
                            coRequestEntity.BusinessTypeCode = (int)request.BusinessType;
                            MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().UpdateObject(coRequestEntity);
                        }
                        Models.CoRequestEndorsement requestEndorsement = request.CoRequestEndorsement.FirstOrDefault();
                        int count = requestEndorsement.DocumentNum + 1;
                        requestEndorsement.DocumentNum = count;
                        CoRequestEndorsementDAO requestEndorsementDAO = new CoRequestEndorsementDAO();
                        requestEndorsement = requestEndorsementDAO.CreateCoRequestEndorsement(requestEndorsement, userId, request.Id);
                        CoRequestAgentDAO requestAgentDAO = new CoRequestAgentDAO();
                        requestAgentDAO.CreateAgents(requestEndorsement.CoRequestAgent, requestEndorsement.Id);
                        CoRequestCoinsuranceDAO requestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        switch (request.BusinessType)
                        {
                            case BusinessType.Accepted:
                                filter = new ObjectCriteriaBuilder();
                                filter.PropertyEquals(Entities.CoRequestCoinsuranceAccepted.Properties.RequestId, request.Id);
                                filter.And();
                                filter.PropertyEquals(Entities.CoRequestCoinsuranceAccepted.Properties.RequestEndorsementId, requestEndorsement.Id);
                                MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().Delete<Entities.CoRequestCoinsuranceAccepted>(filter.GetPredicate());
                                requestCoinsuranceDAO.CreateCoRequestCoinsuranceAccepted(request.InsuranceCompanies.FirstOrDefault(), request.Id, requestEndorsement.Id);
                                break;
                            case BusinessType.Assigned:
                                filter = new ObjectCriteriaBuilder();
                                filter.PropertyEquals(Entities.CoRequestCoinsuranceAssigned.Properties.RequestId, request.Id);
                                filter.And();
                                filter.PropertyEquals(Entities.CoRequestCoinsuranceAssigned.Properties.RequestEndorsementId, requestEndorsement.Id);
                                MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().Delete<Entities.CoRequestCoinsuranceAssigned>(filter.GetPredicate());
                                requestCoinsuranceDAO.CreateCoRequestCoinsuranceAssigned(request.InsuranceCompanies, request.Id, requestEndorsement.Id);
                                break;
                        }
                        transaction.Complete();
                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.SaveCoRequest");
                        request.CoRequestEndorsement[0] = requestEndorsement;
                        return request;
                    }

                    catch (Exception exception)
                    {
                        throw new BusinessException("SaveCoRequest", exception);
                    }

                }
            }

        }
        /// <summary>
        /// Crea una nueva solicitud agrupadora
        /// </summary>
        /// <param name="coRequest"> Modelo de la solicitud agrupadora </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public Models.CoRequest CreateNewCoRequest(Models.CoRequest coRequest)
        {
            int paramCoRequestId = 1023;//CoRequest

            try
            {
                CommonService3GProvider commonProvider = new CommonService3GProvider();
                Parameter paramCoRequest = commonProvider.GetExtendedParameterByParameterId(paramCoRequestId);

                if (paramCoRequest != null && paramCoRequest.NumberParameter.HasValue)
                {
                    paramCoRequest.NumberParameter = paramCoRequest.NumberParameter.Value + 1;
                    coRequest.Id = paramCoRequest.NumberParameter.Value;
                }
                else
                {
                    throw new Exception();
                }

                Entities.CoRequest coRequestEntity = EntityAssembler.CreateCoRequest(coRequest);
                MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().InsertObject(coRequestEntity);
                commonProvider.UpdateExtendedParameter(paramCoRequest);

                Models.CoRequest newCoRequest = ModelAssembler.CreateCoRequest(coRequestEntity);
                newCoRequest.CoRequestEndorsement = coRequest.CoRequestEndorsement;

                return newCoRequest;
            }
            catch (Exception exception)
            {
                throw new BusinessException("CreateNewCoRequest", exception);
            }
        }

        /// <summary>
        /// Buscar Solicitudes por código
        /// </summary>
        /// <param name="requestId">Codigo de solicitud</param>        
        /// <returns></returns>    
        public Models.CoRequest GetCoRequestByRequestId(int requestId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                CoRequestView view = new CoRequestView();
                ViewBuilder builder = new ViewBuilder("CoRequestView");

                //Se consulta el max documentnum de la solicitud
                SelectQuery select = new SelectQuery();
                Function function = new Function(FunctionType.Max);
                function.AddParameter(new Column(Et.CoRequestEndorsement.Properties.DocumentNum, typeof(Et.CoRequestEndorsement).Name));
                select.AddSelectValue(new SelectValue(function, "DocumentNum"));
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Et.CoRequestEndorsement.Properties.RequestId, typeof(Et.CoRequestEndorsement).Name);
                filter.Equal();
                filter.Constant(requestId);
                select.Table = new ClassNameTable(typeof(Et.CoRequestEndorsement));
                select.Where = filter.GetPredicate();
                int maxDocumentNum = 0;
                using (IDataReader reader = MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        maxDocumentNum = reader.GetInt32(0);
                    }
                }

                filter.Clear();
                filter.Property(Et.CoRequest.Properties.RequestId, typeof(Et.CoRequest).Name);
                filter.Equal();
                filter.Constant(requestId);
                filter.And();
                filter.Property(Et.CoRequestEndorsement.Properties.DocumentNum, typeof(Et.CoRequestEndorsement).Name);
                filter.Equal();
                filter.Constant(maxDocumentNum);


                builder.Filter = filter.GetPredicate();
                MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().FillView(builder, view);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.GetCoRequestByRequestId");

                Models.CoRequest request = ModelAssembler.CreateCoRequests(view.CoRequest, view.CoRequestEndorsement, view.CoRequestEndorsementAgent, view.CoRequestAgent, view.Agent, view.BillingGroup).FirstOrDefault();
                if (request != null && request.CoRequestEndorsement != null)
                {
                    CoRequestCoinsuranceDAO coRequestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
                    switch (request.BusinessType)
                    {
                        case BusinessType.Accepted:
                            request.InsuranceCompanies = new List<CoInsuranceCompany>();
                            request.InsuranceCompanies.Add(coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAcceptedByRequedIdByRequestEndorsementId(request.Id, request.CoRequestEndorsement.FirstOrDefault().Id));
                            break;
                        case BusinessType.Assigned:
                            request.InsuranceCompanies = coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAssignedByRequedIdByRequestEndorsementId(request.Id, request.CoRequestEndorsement.FirstOrDefault().Id);
                            break;
                    }
                    request.CoRequestEndorsement.FirstOrDefault().Corequest = null;
                }
                return request;
            }
            catch (Exception exception)
            {
                throw new BusinessException("GetCoRequestByRequestId", exception);
            }
        }

        /// <summary>
        /// Buscar Solicitudes por código o descripción
        /// </summary>
        /// <param name="requestId">Codigo</param>
        /// <param name="description">Descripción</param>
        /// <param name="billingGroupId">Codigo grupo de facturacion</param>
        /// <returns></returns>        
        public List<Models.CoRequest> GetCoRequestByRequestIdDescriptionBillingGroupId(int requestId, string description, int billingGroupId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                CoRequestView view = new CoRequestView();
                ViewBuilder builder = new ViewBuilder("CoRequestView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                int maxDocumentNum = 0;
                if (requestId != 0)
                {
                    //Se consulta el max documentnum de la solicitud
                    SelectQuery select = new SelectQuery();
                    Function function = new Function(FunctionType.Max);
                    function.AddParameter(new Column(Et.CoRequestEndorsement.Properties.DocumentNum, typeof(Et.CoRequestEndorsement).Name));
                    select.AddSelectValue(new SelectValue(function, "DocumentNum"));
                    select.Table = new ClassNameTable(typeof(Et.CoRequestEndorsement));
                    select.Where = filter.GetPredicate();


                    using (IDataReader reader = MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().Select(select))
                    {
                        while (reader.Read())
                        {
                            maxDocumentNum = reader.GetInt32(0);
                        }
                    }
                }

                filter.Clear();

                if (requestId == 0)
                {
                    filter.Property(Et.CoRequest.Properties.Description, typeof(Entities.CoRequest).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }
                else
                {
                    filter.Property(Et.CoRequestEndorsement.Properties.RequestId, typeof(Et.CoRequestEndorsement).Name);
                    filter.Equal();
                    filter.Constant(requestId);
                }
                filter.And();
                filter.Property(Et.CoRequestEndorsement.Properties.DocumentNum, typeof(Et.CoRequestEndorsement).Name);
                filter.Equal();
                filter.Constant(maxDocumentNum);
                filter.And();
                filter.Property(EtUnder.BillingGroup.Properties.BillingGroupCode, typeof(EtUnder.BillingGroup).Name);
                filter.Equal();
                filter.Constant(billingGroupId);

                builder.Filter = filter.GetPredicate();
                MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().FillView(builder, view);
                List<Models.CoRequest> coRequests = ModelAssembler.CreateCoRequests(view.CoRequest, view.CoRequestEndorsement, view.CoRequestEndorsementAgent, view.CoRequestAgent, view.Agent, view.BillingGroup);

                if (coRequests.Count == 1)
                {
                    foreach (Models.CoRequestAgent item in coRequests[0].CoRequestEndorsement[0].CoRequestAgent)
                    {
                        AgencyDAO agencyDAO = new AgencyDAO();
                        Agency agency = agencyDAO.GetAgencyByAgentIdAgentAgencyId(item.Agency.Agent.IndividualId, item.Agency.Id);
                        item.Agency.Code = agency.Code;
                        item.Agency.FullName = agency.FullName;
                        item.Agency.Branch = agency.Branch;
                    }
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.GetCoRequestByRequestIdDescriptionBillingGroupId");

                return coRequests;
            }
            catch (Exception exception)
            {
                throw new BusinessException("GetCoRequestByRequestIdDescriptionBillingGroupId", exception);
            }
        }

        /// <summary>
        /// Buscar Solicitudes Por Id o Descripción
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<Models.CoRequest> GetCoRequestByDescription(string description)
        {
            List<Models.CoRequest> coRequests = new List<Models.CoRequest>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            int requestId = 0;
            int.TryParse(description, out requestId);

            if (requestId > 0)
            {
                filter.Property(Entities.CoRequest.Properties.RequestId, typeof(Entities.CoRequest).Name).Equal().Constant(requestId);
            }
            else
            {
                filter.Property(Entities.CoRequest.Properties.Description, typeof(Entities.CoRequest).Name).Like().Constant("%" + description + "%");
            }

            BusinessCollection businessCollection = new BusinessCollection(MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.CoRequest), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                coRequests = ModelAssembler.CreateCoRequests(businessCollection);
            }

            return coRequests;
        }
    }
}