// -----------------------------------------------------------------------
// <copyright file="LegalRepresentativeSingDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonServices.DAOs
{
    using Sistran.Company.Application.UniquePerson.Entities;
    using Sistran.Company.Application.UniquePersonServices.EEProvider.Assemblers;
    using Sistran.Company.Application.UniquePersonServices.EEProvider.Resources;
    using Sistran.Company.Application.UniquePersonServices.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Clase DAO del objeto LegalRepresentativeSingDAO.
    /// </summary>
    public class LegalRepresentativeSingDAO
    {
        /// <summary>
        /// Obtiene la lista de Firma Representante Legal.
        /// </summary>
        /// <returns>Lista de Firma Representante Legal consultadas</returns>
        public Result<List<ParamLegalRepresentativeSing>, ErrorModel> GetLstCptLegalReprSign()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptLegalReprSign)));
                Result<List<ParamLegalRepresentativeSing>, ErrorModel> lstCptLegalReprSign = ModelAssembler.CreateLstCptLegalReprSign(businessCollection);
                if (lstCptLegalReprSign is ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>)
                {
                    return lstCptLegalReprSign;
                }
                else
                {
                    List<ParamLegalRepresentativeSing> resultValue = (lstCptLegalReprSign as ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoLegalRepresentativeSingWasFound);
                        return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstCptLegalReprSign;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryTheLegalRepresentativeSing);
                return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Método que obtine una Firma Representante Legal por Id
        /// </summary>
        /// <param name="ciaCode">Id Compañia</param>
        /// <param name="branchTypeCode">Id Sucursal</param>
        /// <param name="currentFrom">Fecha actual</param>
        /// <returns>una Firma Representante Legal consultada</returns>
        public Result<ParamLegalRepresentativeSing, ErrorModel> GetCptLegalReprSignByCiaCodeBranchTypeCodeCurrentFrom(decimal ciaCode, decimal branchTypeCode, DateTime currentFrom)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                PrimaryKey key = CptLegalReprSign.CreatePrimaryKey(ciaCode, branchTypeCode, currentFrom);
                CptLegalReprSign cptLegalReprSign = (CptLegalReprSign)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                Result<ParamLegalRepresentativeSing, ErrorModel> paramCptLegalReprSign = ModelAssembler.CreateCptLegalReprSign(cptLegalReprSign);
                if (paramCptLegalReprSign is ResultError<ParamLegalRepresentativeSing, ErrorModel>)
                {
                    return paramCptLegalReprSign;
                }
                else
                {
                    ParamLegalRepresentativeSing resultValue = (paramCptLegalReprSign as ResultValue<ParamLegalRepresentativeSing, ErrorModel>).Value;

                    if (resultValue == null)
                    {
                        errorModelListDescription.Add("No se encontro una Firma Representante Legal.");
                        return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return paramCptLegalReprSign;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de Firma Representante Legal. Comuniquese con el administrador");
                return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }


    }
}
