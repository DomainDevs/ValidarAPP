using Sistran.Company.Application.ModelServices.Models.Reports;
using Sistran.Company.Application.ReportsServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ReportsServicesEEProvider.Assemblers
{
    public class ModelServicesAssembler
    {
        public static ScoreCreditServiceModel CreateScoreCredit(CompanyScoreCredit companyScoreCredit)
        {
            return new ScoreCreditServiceModel
            {
                scoreCreditId = companyScoreCredit.scoreCreditId,
                individualId = companyScoreCredit.individualId,
                idCardTypeCd = companyScoreCredit.idCardTypeCd,
                idCardNo = companyScoreCredit.idCardNo,
                score = companyScoreCredit.score,
                responseCode = companyScoreCredit.responseCode,
                response = companyScoreCredit.response,
                dateRequest = companyScoreCredit.dateRequest,
                isDefaultValue = companyScoreCredit.isDefaultValue,
                userId = companyScoreCredit.userId,
                a1 = companyScoreCredit.a1,
                a2 = companyScoreCredit.a2,
                a3 = companyScoreCredit.a3,
                a4 = companyScoreCredit.a4,
                a5 = companyScoreCredit.a5,
                a6 = companyScoreCredit.a6,
                a7 = companyScoreCredit.a7,
                a8 = companyScoreCredit.a8,
                a9 = companyScoreCredit.a9,
                a10 = companyScoreCredit.a10,
                a11 = companyScoreCredit.a11,
                a12 = companyScoreCredit.a12,
                a13 = companyScoreCredit.a13,
                a14 = companyScoreCredit.a14,
                a15 = companyScoreCredit.a15,
                a16 = companyScoreCredit.a16,
                a17 = companyScoreCredit.a17,
                a18 = companyScoreCredit.a18,
                a19 = companyScoreCredit.a19,
                a20 = companyScoreCredit.a20,
                a21 = companyScoreCredit.a21,
                a22 = companyScoreCredit.a22,
                a23 = companyScoreCredit.a23,
                a24 = companyScoreCredit.a24,
                a25 = companyScoreCredit.a25,
                request = companyScoreCredit.response,
                userName = companyScoreCredit.UserName
            };
        }

        public static ScoreCreditValidServiceModel CreateScoreCreditValid(CompanyScoreCreditValid companyScoreCreditValid)
        {
            return new ScoreCreditValidServiceModel
            {
                idCardTypeCd = companyScoreCreditValid.idCardTypeCd,
                idCardNo = companyScoreCreditValid.idCardNo,
                score = companyScoreCreditValid.score,
                dateRequest = companyScoreCreditValid.dateRequest
            };
        }
        public static ScoreCreditsServiceModel CreateScoreCredits(CompanyScoreCredits companyScoreCredit)
        {
            ScoreCreditsServiceModel ScoreCreditsServiceModel = new ScoreCreditsServiceModel();
            List<ScoreCreditServiceModel> ListScoreCreditServiceModel = new List<ScoreCreditServiceModel>();

            if (companyScoreCredit.companyScoreCredit.Count > 0)
            {
                foreach (CompanyScoreCredit item in companyScoreCredit.companyScoreCredit)
                {
                    ListScoreCreditServiceModel.Add(ModelServicesAssembler.CreateScoreCredit(item));
                }
            }

            ScoreCreditsServiceModel.scoreCreditValid = ModelServicesAssembler.CreateScoreCreditValid(companyScoreCredit.companyScoreCreditValid);
            ScoreCreditsServiceModel.scoreCredits = ListScoreCreditServiceModel;
            ScoreCreditsServiceModel.ErrorTypeService = ModelServices.Enums.ErrorTypeService.Ok;
            return ScoreCreditsServiceModel;
        }

        public static VehiclesParametersServiceModel GetVehicles(VehiclesParameters resultValue)
        {
            VehiclesParametersServiceModel vehiclesParametersServiceModel = new VehiclesParametersServiceModel();
            List<VehicleParameterServiceModel> vehicleParameters = new List<VehicleParameterServiceModel>();
            foreach (var item in resultValue.vehicleParameters)
            {
                vehicleParameters.Add(GetVehicle(item));
            }
            vehiclesParametersServiceModel.vehicleParameter = vehicleParameters;

            return vehiclesParametersServiceModel;
        }

        private static VehicleParameterServiceModel GetVehicle(VehicleParameter item) => new VehicleParameterServiceModel
        {
            FasecoldaMakeId = item.FasecoldaMakeId,
            FasecoldaModelId = item.FasecoldaModelId,
            MakeDescription = item.MakeDescription,
            ModelDescription = item.ModelDescription,
            StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
            VehicleMakeCode = item.VehicleMakeCode,
            VehicleModelCode = item.VehicleModelCode,
            VehiclePrice = item.VehiclePrice,
            VehicleTypeCode = item.VehicleTypeCode,
            VehicleTypeDescription = item.VehicleTypeDescription,
            VehicleVersionCode = item.VehicleVersionCode,
            VehicleYear = item.VehicleYear,
            VersionDescription = item.VersionDescription
           
        };
    }
}
