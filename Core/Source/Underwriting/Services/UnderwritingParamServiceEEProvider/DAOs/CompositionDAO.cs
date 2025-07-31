// -----------------------------------------------------------------------
// <copyright file="CompositionDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andrés Clavijo</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System.Collections.Generic;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using ENUMUD = Sistran.Core.Application.UnderwritingServices.Enums;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PAREN = Sistran.Core.Application.Parameters.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using System.Diagnostics;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using PARAMEN = Sistran.Core.Application.Parameters.Entities;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using COMMO = Sistran.Core.Application.CommonService.Models;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Linq;
    using System;
    using System.Data;
    using Sistran.Co.Application.Data;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;

    /// <summary>
    /// Acceso a DB de niveles de influencia
    /// </summary>
    class CompositionDAO
    {
        public UTMO.Result<List<ParamComposition>, UTMO.ErrorModel> GetParametrizationCompositions()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PAREN.CompositionType)));
                List<ParamComposition> parametrizationCompositions = ModelAssembler.CreateParametrizationComposition(businessCollection);
                return new UTMO.ResultValue<List<ParamComposition>, UTMO.ErrorModel>(parametrizationCompositions);
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetLevelClause");
                return new UTMO.ResultError<List<ParamComposition>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }
    }
}
