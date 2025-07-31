
namespace Sistran.Company.Application.ReportsServicesEEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Company.Application.ReportsServices.Models;
    using Sistran.Company.Application.ReportsServicesEEProvider.Assemblers;
    using Sistran.Company.Application.ReportsServicesEEProvider.Resources;
    using Sistran.Company.Application.Utilities.Error;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using ENUM=Sistran.Company.Application.Utilities.Enums;

    public class ReportsDAO
    {
        public object ErrorType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCardTypeCd"></param>
        /// <param name="idCardNo"></param>
        /// <returns></returns>
        public Result<CompanyScoreCredits, ErrorModel> GetScoredCredit(int idCardTypeCd, string idCardNo)
        {
            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("@ID_CARD_TYPE_CD", idCardTypeCd);
            parameters[1] = new NameValue("@ID_CARD_NO", idCardNo);

            Result<DataSet, ErrorModel> result = GenericExecuteStoredProcedured("[UP].[CPT_SP_GET_HISTORY_SCORE_CREDIT]", parameters);

            //List<CompanyScoreCredit> companyScoreCredits = new List<CompanyScoreCredit>();
            CompanyScoreCredits companyScoreCredits = new CompanyScoreCredits();


            if (result is ResultError<DataSet, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<DataSet, ErrorModel>).Message;
                return new ResultError<CompanyScoreCredits, ErrorModel>(ErrorModel.CreateErrorModel(errorModelResult.ErrorDescription, errorModelResult.ErrorType, null));
            }
            else
            {
                companyScoreCredits = ModelAssembler.CreateCompanyScoreCredits((result as ResultValue<DataSet, ErrorModel>).Value);

                if (companyScoreCredits.companyScoreCredit.Count == 0)
                {
                    return new ResultError<CompanyScoreCredits, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ScorCreditNotFound }, ENUM.ErrorType.NotFound, null));
                }
                else
                {
                    return new ResultValue<CompanyScoreCredits, ErrorModel>(companyScoreCredits);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCardTypeCd"></param>
        /// <param name="idCardNo"></param>
        /// <returns></returns>
        public Result<VehiclesParameters, ErrorModel> GetVehicle(int accionType, int vehicleYearInit, int vehicleYearEnd, string makeId, string ModelId)
        {
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("@ACTION_TYPE", accionType);
            parameters[1] = new NameValue("@VEHICLE_YEAR_INIT", vehicleYearInit);
            parameters[2] = new NameValue("@VEHICLE_YEAR_END", vehicleYearEnd);
            parameters[3] = new NameValue("@FV_MAKE_ID", makeId);
            parameters[4] = new NameValue("@FV_MODEL_ID", ModelId);

            Result<DataSet, ErrorModel> result = GenericExecuteStoredProcedured("[COMM].[CO_GET_VEHICLE_PARAMETER]", parameters);

            VehiclesParameters vehiclesParameters = new VehiclesParameters();


            if (result is ResultError<DataSet, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<DataSet, ErrorModel>).Message;
                return new ResultError<VehiclesParameters, ErrorModel>(ErrorModel.CreateErrorModel(errorModelResult.ErrorDescription, errorModelResult.ErrorType, null));
            }
            else
            {
                if (ModelId == null)
                {
                    vehiclesParameters = ModelAssembler.CreateVehiclesFasecolda((result as ResultValue<DataSet, ErrorModel>).Value);
                }
                else
                {
                    vehiclesParameters = ModelAssembler.CreateVehiclesParameters((result as ResultValue<DataSet, ErrorModel>).Value);
                }

                if (vehiclesParameters.vehicleParameters.Count == 0)
                {
                    return new ResultError<VehiclesParameters, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ScorCreditNotFound }, ENUM.ErrorType.NotFound, null));
                }
                else
                {
                    return new ResultValue<VehiclesParameters, ErrorModel>(vehiclesParameters);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NameStoredProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private Result<DataSet, ErrorModel> GenericExecuteStoredProcedured(string NameStoredProcedure, NameValue[] parameters)
        {
            DataSet dataSet;
            List<string> errorModel = new List<string>();
            try
            {
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dataSet = pdb.ExecuteSPDataSet(NameStoredProcedure, parameters);
                }

                return new ResultValue<DataSet, ErrorModel>(dataSet);
            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.ErrorQueryReports + NameStoredProcedure);
                return new ResultError<DataSet, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ENUM.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
