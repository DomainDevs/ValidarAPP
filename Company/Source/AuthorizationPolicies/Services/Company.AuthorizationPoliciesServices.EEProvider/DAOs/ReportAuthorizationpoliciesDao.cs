using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Company.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using ENUMUT = Sistran.Company.Application.Utilities.Enums;
using UNUEN = Sistran.Core.Application.AuthorizationPolicies.Entities;
using UTMO = Sistran.Company.Application.Utilities.Error;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class ReportAuthorizationpoliciesDao
    {
        /// <summary>
        /// Obtiene la lista de stados de eventos
        /// </summary>
        /// <returns>lista de usuarios autorizadores</returns>
        public UTMO.Result<List<Status>, UTMO.ErrorModel> GetAllStatus()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<Status> listStatus = new List<Status>();
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNUEN.Status)));
                listStatus = ModelAssembler.CreateListStatusParam(businessCollection);
                return new UTMO.ResultValue<List<Status>, UTMO.ErrorModel>(listStatus);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetDelegation");
                return new UTMO.ResultError<List<Status>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationPolicies.EEProvider.DAOs");
            }
        }

    }
}
