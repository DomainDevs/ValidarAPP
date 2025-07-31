// -----------------------------------------------------------------------
// <copyright file="BusinessBusinessConfigurationDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Quotation.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Clase DAO del objeto BusinessBusinessConfiguration.
    /// </summary>
    public class BusinessBusinessConfigurationDAO
    {
        /// <summary>
        /// Realiza las operaciones CRUD para los negocios y acuerdos de negocios nuevos
        /// </summary>
        /// <param name="businessAdded">negocios para agregar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<ParamBusinessParamBusinessConfiguration> SaveNewBusiness(List<ParamBusinessParamBusinessConfiguration> businessAdded)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<ParamBusinessParamBusinessConfiguration> returnBusiness = new ParametrizationResponse<ParamBusinessParamBusinessConfiguration>();
            stopWatch.Start();
            using (Context.Current)
            {
                // Agregar
                if (businessAdded != null && businessAdded.Count>0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            int businessConfigurationAdded = 0;
                            foreach (ParamBusinessParamBusinessConfiguration item in businessAdded)
                            {
                                Business entityBusiness = EntityAssembler.CreateBusiness(item.ParamBusiness);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityBusiness);
                                ObjectCriteriaBuilder filterBusiness = new ObjectCriteriaBuilder();
                                filterBusiness.PropertyEquals(Business.Properties.Description, item.ParamBusiness.Description);
                                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Business), filterBusiness.GetPredicate()));
                                int idBusiness = 0;
                                foreach (Business itemBusiness in businessCollection)
                                {
                                    idBusiness = itemBusiness.BusinessId;
                                }
                                if (item.ParamBusinessConfiguration!=null)
                                {
                                    foreach (ParamBusinessConfiguration paramBusinessConfiguration in item.ParamBusinessConfiguration)
                                    {
                                        BusinessConfiguration entityBusinessConfiguration = EntityAssembler.CreateBusinessConfiguration(paramBusinessConfiguration);
                                        entityBusinessConfiguration.BusinessId = idBusiness;
                                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entityBusinessConfiguration);
                                    }
                                }
                                businessConfigurationAdded = businessConfigurationAdded + item.ParamBusinessConfiguration.Count;
                            }

                            transaction.Complete();
                            returnBusiness.TotalAdded = businessAdded.Count + businessConfigurationAdded;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusiness.ErrorAdded = "ErrorSaveBusinessAdded";
                        }
                    }
                }

                BusinessDAO businessDAO = new BusinessDAO();
                Result<List<ParamBusiness>, ErrorModel> result = businessDAO.GetBusiness();
                returnBusiness.ReturnedList = null;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveBusiness");
            return returnBusiness;
        }
    }
}
