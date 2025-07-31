// -----------------------------------------------------------------------
// <copyright file="DetailTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System.Collections.Generic;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using UTIEN = Sistran.Core.Application.Utilities.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTIER = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using COMMO = Sistran.Core.Application.CommonService.Models;
    using MOLUNPARAM = Sistran.Core.Application.UnderwritingParamService.Models;

    /// <summary>
    /// Clase para tipo de detalle
    /// </summary>
    public class DetailTypeDAO
    {
        /// <summary>
        /// Obtiene los tipos de detalle
        /// </summary>
        /// <returns>Tipos de detalle</returns>
        public UTIER.Result<List<MOLUNPARAM.DetailType>, UTIER.ErrorModel> GetDetailTypes()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.DetailType)));
                List<MOLUNPARAM.DetailType> detailTypes = ModelAssembler.CreateDetailTypes(businessCollection);
                return new UTIER.ResultValue<List<MOLUNPARAM.DetailType>, UTIER.ErrorModel>(detailTypes);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorOccurredIn + " GetDetailTypes");
                return new UTIER.ResultError<List<MOLUNPARAM.DetailType>, UTIER.ErrorModel>(UTIER.ErrorModel.CreateErrorModel(listErrors, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los tipos de detalle relacionados
        /// </summary>
        /// <returns>listado de tipos de detalle</returns>
        public UTIER.Result<List<ParamDetailTypeDesc>, UTIER.ErrorModel> GetParamDetailTypeDescs()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.DetailType)));
                List<ParamDetailTypeDesc> detailTypes = ModelAssembler.CreateParamDetailTypeDescs(businessCollection);
                return new UTIER.ResultValue<List<ParamDetailTypeDesc>, UTIER.ErrorModel>(detailTypes);
            }
            catch (System.Exception ex)
            {
                return new UTIER.ResultError<List<ParamDetailTypeDesc>, UTIER.ErrorModel>(UTIER.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetDetailType }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los tipos de detalle relacionados a la cobertura
        /// </summary>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>listado de tipos de detalle</returns>
        public UTIER.Result<List<ParamDetailTypeDesc>, UTIER.ErrorModel> GetDetailTypesByCoverageId(int coverageId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.CoverDetailType.Properties.CoverageId, typeof(QUOEN.CoverDetailType).Name);
                filter.Equal();
                filter.Constant(coverageId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.CoverDetailType), filter.GetPredicate()));
                List<ParamDetailTypeDesc> detailTypes = ModelAssembler.CreateDetailTypesRelation(businessCollection);
                return new UTIER.ResultValue<List<ParamDetailTypeDesc>, UTIER.ErrorModel>(detailTypes);
            }
            catch (System.Exception ex)
            {
                return new UTIER.ResultError<List<ParamDetailTypeDesc>, UTIER.ErrorModel>(UTIER.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetDetailTypeRelation }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Creacion de deducibles asociados a la cobertura
        /// </summary>
        /// <param name="paramDetailTypeDesc">deducible a crear</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>deducible creado</returns>
        public UTIER.Result<ParamDetailTypeDesc, UTIER.ErrorModel> CreateParamDetailTypeDesc(ParamDetailTypeDesc paramDetailTypeDesc, int coverageId)
        {
            try
            {
                QUOEN.CoverDetailType coverDetailTypeEntity = EntityAssembler.CreateCoverDetailType(paramDetailTypeDesc, coverageId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(coverDetailTypeEntity);
                ParamDetailTypeDesc result = ModelAssembler.CreateDetailTypeRelation(coverDetailTypeEntity);
                return new UTIER.ResultValue<ParamDetailTypeDesc, UTIER.ErrorModel>(result);
            }
            catch (System.Exception ex)
            {
                return new UTIER.ResultError<ParamDetailTypeDesc, UTIER.ErrorModel>(UTIER.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBCreateDetailTypeRelation }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
