using Sistran.Company.Application.ReportsServices;
using Sistran.Company.Application.ReportsServices.Models;
using Sistran.Company.Application.ReportsServicesEEProvider.DAOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.ReportsServicesEEProvider.Resources;
using Sistran.Company.Application.ModelServices.Models.Reports;
using Sistran.Company.Application.ReportsServicesEEProvider.Assemblers;
using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.Utilities.Error;

namespace Sistran.Company.Application.ReportsServicesEEProvider
{
    public class ReportsServiceEEProvider : IReportsService
    {
        public ScoreCreditsServiceModel GetScoredCredit(int idCardTypeCd, string idCardNo)
        {
            ReportsDAO reportsDAO = new ReportsDAO();
            List<CompanyScoreCredit> companyScoreCredits = new List<CompanyScoreCredit>();

            Result<CompanyScoreCredits, ErrorModel> Result = reportsDAO.GetScoredCredit(idCardTypeCd, idCardNo);
            ScoreCreditsServiceModel objScoreCreditsServiceModel = new ScoreCreditsServiceModel();
            if (Result is ResultError<CompanyScoreCredits, ErrorModel>)
            {
                ErrorModel errorModelResult = (Result as ResultError<CompanyScoreCredits, ErrorModel>).Message;
                objScoreCreditsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                objScoreCreditsServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                CompanyScoreCredits resultValue = (Result as ResultValue<CompanyScoreCredits, ErrorModel>).Value;
                objScoreCreditsServiceModel = ModelServicesAssembler.CreateScoreCredits(resultValue);
            }

            return objScoreCreditsServiceModel;

        }

        public VehiclesParametersServiceModel GetVehicleParameter(int accionType, int vehicleYearInit, int vehicleYearEnd, string makeId, string modelId)
        {
            ReportsDAO reportsDAO = new ReportsDAO();
            List<VehicleParameter> vehicleParameters = new List<VehicleParameter>();

            Result<VehiclesParameters, ErrorModel> Result = reportsDAO.GetVehicle(accionType, vehicleYearInit, vehicleYearEnd, makeId, modelId);
            VehiclesParametersServiceModel vehiclesParametersServiceModel = new VehiclesParametersServiceModel();
            if (Result is ResultError<VehiclesParameters, ErrorModel>)
            {
                ErrorModel errorModelResult = (Result as ResultError<VehiclesParameters, ErrorModel>).Message;
                vehiclesParametersServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                vehiclesParametersServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                VehiclesParameters resultValue = (Result as ResultValue<VehiclesParameters, ErrorModel>).Value;
                vehiclesParametersServiceModel = ModelServicesAssembler.GetVehicles(resultValue);
            }

            return vehiclesParametersServiceModel;

        }
    }
}
