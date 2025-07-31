// -----------------------------------------------------------------------
// <copyright file="ComponentRelationDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using System.Collections.Generic;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using PROEN = Sistran.Core.Application.Product.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;

    public class ComponentRelationDAO
    {
        /// <summary>
        /// Consulta componentes
        /// </summary>
        /// <returns>Retorna lista de componentes</returns>
        public UTMO.Result<List<Component>, UTMO.ErrorModel> GetComponent()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Component)));
                List<Component> paymentMethod = ModelAssembler.CreateComponentRelations(businessCollection);
                return new UTMO.ResultValue<List<Component>, UTMO.ErrorModel>(paymentMethod);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetFirtsComponent);
                return new UTMO.ResultError<List<Component>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Metodo para crear relacion componentes plan financiero
        /// </summary>
        /// <param name="firtsPayComponent">Recibe level</param>
        /// <returns>Retorna ParamClauseLevel</returns>
        public UTMO.Result<ParamFirstPayComponent, UTMO.ErrorModel> CreateFirstComponent(ParamFirstPayComponent firtsPayComponent, int financialPlanId)
        {
            try
            {
                PROEN.FirstPayComponent firtsPayComponentEntity = EntityAssembler.CreateFirtsPayComponent(firtsPayComponent, financialPlanId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(firtsPayComponentEntity);
                ParamFirstPayComponent levelResult = ModelAssembler.CreateFirstPayComponent(firtsPayComponentEntity);
                return new UTMO.ResultValue<ParamFirstPayComponent, UTMO.ErrorModel>(levelResult);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedCreatingFirtsComponent);
                return new UTMO.ResultError<ParamFirstPayComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
