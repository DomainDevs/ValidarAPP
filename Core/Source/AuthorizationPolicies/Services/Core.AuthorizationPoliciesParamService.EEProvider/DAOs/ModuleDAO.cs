// -----------------------------------------------------------------------
// <copyright file="ModuleDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.DAOs
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UNUEN = Sistran.Core.Application.UniqueUser.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Defines the <see cref="ModuleDAO" />
    /// </summary>
    public class ModuleDAO
    {
        /// <summary>
        /// Consulta modulos
        /// </summary>
        /// <returns>Retorna modulos</returns>
        public UTMO.Result<List<ParamModule>, UTMO.ErrorModel> GetParametrizationModules()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
         
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UNUEN.Modules.Properties.Enabled, typeof(UNUEN.Modules).Name);
                //REQ_483
                //purpose: Corrección script like por equal
                //author: Germán F. Grimaldi
                //date: 06/08/2018
                //inicio
                //filter.Like();
                filter.Equal();
                //Fin
                filter.Constant(1);

                ModuleParametrizationView view = new ModuleParametrizationView();
                ViewBuilder builder = new ViewBuilder("ModuleParametrizationView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                


                //BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNUEN.Modules), filter.GetPredicate()));
                List<ParamModule> parametrizationModules = ModelAssembler.CreateModules(view.Modules);
                List<ParamSubModule> parametrizationSubModule = ModelAssembler.CreateSubModules(view.SubModules);
                foreach (var item in parametrizationModules)
                {
                    item.SubModules = parametrizationSubModule.Where(b => b.Id == item.Id).ToList();
                    for (int i = 0; i < item.SubModules.Count; i++)
                    {
                        item.SubModules[i].Description = view.SubModules.Cast<UNUEN.Submodules>().ToList().Where(b => b.ModuleCode == item.Id).FirstOrDefault().Description;
                    }
                }

                return new UTMO.ResultValue<List<ParamModule>, UTMO.ErrorModel>(parametrizationModules);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetDelegation");
                return new UTMO.ResultError<List<ParamModule>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserParamService.EEProvider.DAOs");
            }
        }
    }
}
