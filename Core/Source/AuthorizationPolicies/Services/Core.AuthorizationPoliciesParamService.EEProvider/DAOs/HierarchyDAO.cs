// -----------------------------------------------------------------------
// <copyright file="HierarchyDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.DAOs
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UNUEN = Sistran.Core.Application.UniqueUser.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Defines the <see cref="HierarchyDAO" />
    /// </summary>
    public class HierarchyDAO
    {
        /// <summary>
        /// Consulta Jerarquias
        /// </summary>
        /// <returns>Retorna Jerarquias</returns>
        public UTMO.Result<List<ParamHierarchy>, UTMO.ErrorModel> GetParametrizationHierarchy()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNUEN.CoHierarchy)));
                List<ParamHierarchy> parametrizationModules = ModelAssembler.CreateHierarchies(businessCollection);
                return new UTMO.ResultValue<List<ParamHierarchy>, UTMO.ErrorModel>(parametrizationModules);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetSubModules");
                return new UTMO.ResultError<List<ParamHierarchy>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserParamService.EEProvider.DAOs");
            }
        }
    }
}
