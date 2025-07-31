// -----------------------------------------------------------------------
// <copyright file="VehicleTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using COMMOD = Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using ENUM = Sistran.Core.Application.UnderwritingParamService.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Enums;
    using EntRules = Sistran.Core.Application.Script.Entities;

    /// <summary>
    /// Dao para Tipo de ejecucion
    /// </summary>
    public class RuleSetDAO
    {
        /// <summary>
        /// Obtener Objetos Del Seguro 
        /// </summary>       
        /// <returns>Objetos Del Seguro</returns>
        public UTMO.Result<List<ParamRuleSet>, UTMO.ErrorModel> GetRuleSet()
        {
            using (Transaction transaction = new Transaction())
            {
                List<ParamRuleSet> paramRateType = new List<ParamRuleSet>();
                int level = (int)ENUM.RuleSetLevel.ExpenseComponent;
                var filter = new ObjectCriteriaBuilder();
                filter.Property(EntRules.RuleSet.Properties.LevelId, typeof(EntRules.RuleSet).Name);
                filter.Equal();
                filter.Constant(level);    
                List<string> errorModel = new List<string>();
                try
                {
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EntRules.RuleSet), filter.GetPredicate()));
                    Result<List<ParamRuleSet>, ErrorModel> lstParamRuleSet = ModelAssembler.CreateRuleSet(businessCollection);
                    if (lstParamRuleSet is ResultError<List<ParamRuleSet>, ErrorModel>)
                    {
                        return lstParamRuleSet;
                    }
                    else
                    {
                        List<ParamRuleSet> resultValue = (lstParamRuleSet as ResultValue<List<ParamRuleSet>, ErrorModel>).Value;

                        if (resultValue.Count == 0)
                        {
                            errorModel.Add("No se encuentra El metodo de pago.");
                            return new ResultError<List<ParamRuleSet>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, null));
                        }
                        else
                        {
                            return lstParamRuleSet;
                        }

                    }
                }
                catch (Exception ex)
                {
                    errorModel.Add("Ocurrio un error inesperado en la consulta de reglas de negocio. Comuniquese con el administrador");
                    return new ResultError<List<ParamRuleSet>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
                }
            }
        }
    }
}
