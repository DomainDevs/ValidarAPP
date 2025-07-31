using Sistran.Company.Application.ReportsServices.Models;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Company.Application.ReportsServicesEEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region ScoreCredit
        public static CompanyScoreCredit CreateCompanyScoreCredit(DataRow dr)
        {
            return new CompanyScoreCredit
            {
                scoreCreditId = Convert.ToInt32(dr["SCORE_CREDIT_ID"]),
                individualId = Convert.ToInt32(dr["INDIVIDUAL_ID"]),
                idCardTypeCd = Convert.ToInt32(dr["ID_CARD_TYPE_CD"]),
                idCardNo = Convert.ToString(dr["ID_CARD_NO"]),
                score = Convert.ToString(dr["SCORE"]),
                responseCode = Convert.ToInt32(dr["RESPONSE_CODE"]),
                response = Convert.ToString(dr["RESPONSE"]),
                dateRequest = Convert.ToDateTime(dr["DATE_REQUEST"]),
                isDefaultValue = Convert.ToBoolean(dr["IS_DEFAULT_VALUE"]),
                userId = Convert.ToInt32(dr["USER_ID"]),
                a1 = Convert.ToString(dr["A1"]),
                a2 = Convert.ToString(dr["A2"]),
                a3 = Convert.ToString(dr["A3"]),
                a4 = Convert.ToString(dr["A4"]),
                a5 = Convert.ToString(dr["A5"]),
                a6 = Convert.ToString(dr["A6"]),
                a7 = Convert.ToString(dr["A7"]),
                a8 = Convert.ToString(dr["A8"]),
                a9 = Convert.ToString(dr["A9"]),
                a10 = Convert.ToString(dr["A10"]),
                a11 = Convert.ToString(dr["A11"]),
                a12 = Convert.ToString(dr["A12"]),
                a13 = Convert.ToString(dr["A13"]),
                a14 = Convert.ToString(dr["A14"]),
                a15 = Convert.ToString(dr["A15"]),
                a16 = Convert.ToString(dr["A16"]),
                a17 = Convert.ToString(dr["A17"]),
                a18 = Convert.ToString(dr["A18"]),
                a19 = Convert.ToString(dr["A19"]),
                a20 = Convert.ToString(dr["A20"]),
                a21 = Convert.ToString(dr["A21"]),
                a22 = Convert.ToString(dr["A22"]),
                a23 = Convert.ToString(dr["A23"]),
                a24 = Convert.ToString(dr["A24"]),
                a25 = Convert.ToString(dr["A25"]),
                request = Convert.ToString(dr["REQUEST"]),
                UserName = Convert.ToString(dr["ACCOUNT_NAME"])
            };
        }

        public static CompanyScoreCreditValid CreateCompanyScoreCreditValid(DataRow dr)
        {
            return new CompanyScoreCreditValid
            {
                idCardTypeCd = Convert.ToInt32(dr["ID_CARD_TYPE_CD"]),
                idCardNo = Convert.ToString(dr["ID_CARD_NO"]),
                score = Convert.ToString(dr["SCORE"]),
                dateRequest = Convert.ToDateTime(dr["DATE_REQUEST"])
            };
        }

        public static CompanyScoreCredits CreateCompanyScoreCredits(DataSet ds)
        {
            CompanyScoreCredits companyScoreCredits = new CompanyScoreCredits();
            List<CompanyScoreCredit> companyScoreCredit = new List<CompanyScoreCredit>();
            CompanyScoreCreditValid companyScoreCreditValid = new CompanyScoreCreditValid();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                companyScoreCredit.Add(CreateCompanyScoreCredit(dr));
            }
            companyScoreCredits.companyScoreCredit = companyScoreCredit;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                companyScoreCreditValid = CreateCompanyScoreCreditValid(dr);
            }
            companyScoreCredits.companyScoreCreditValid = companyScoreCreditValid;

            return companyScoreCredits;
        }      

        public static VehiclesParameters CreateVehiclesParameters(DataSet ds)
        {
            VehiclesParameters vehiclesParameters = new VehiclesParameters();
            List<VehicleParameter> vehicleParameters = new List<VehicleParameter>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vehicleParameters.Add(CreateVehicleParameter(dr));
            }
            vehiclesParameters.vehicleParameters = vehicleParameters;

            return vehiclesParameters;
        }

        public static VehiclesParameters CreateVehiclesFasecolda(DataSet ds)
        {
            VehiclesParameters vehiclesParameters = new VehiclesParameters();
            List<VehicleParameter> vehicleParameters = new List<VehicleParameter>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vehicleParameters.Add(CreateVehicleFasecolda(dr));
            }
            vehiclesParameters.vehicleParameters = vehicleParameters;

            return vehiclesParameters;
        }

        private static VehicleParameter CreateVehicleFasecolda(DataRow dr) => new VehicleParameter
        {
            VehicleMakeCode = Convert.ToInt32(dr["VEHICLE_MAKE_CD"]),
            MakeDescription = Convert.ToString(dr["MAKE_DESCRIPTION"]),
            VehicleModelCode = Convert.ToInt32(dr["VEHICLE_MODEL_CD"]),
            ModelDescription = Convert.ToString(dr["MODEL_DESCRIPTION"]),
            VehicleVersionCode = Convert.ToInt32(dr["VEHICLE_VERSION_CD"]),
            VersionDescription = Convert.ToString(dr["VEHICLE_DESCRIPTION"]),
            VehicleTypeCode = Convert.ToInt32(dr["VEHICLE_TYPE_CD"]),
            VehicleTypeDescription = Convert.ToString(dr["VEHICLE_TYPE_DESCRIPTION"]),
            VehicleYear = Convert.ToInt32(dr["VEHICLE_YEAR"]),
            VehiclePrice = Convert.ToInt32(dr["VEHICLE_PRICE"]),          
            FasecoldaMakeId = Convert.ToString(dr["FASECOLDA_MAKE_ID"]),
            FasecoldaModelId = Convert.ToString(dr["FASECOLDA_MODEL_ID"]),
        };

        private static VehicleParameter CreateVehicleParameter(DataRow dr) => new VehicleParameter
        {
            VehicleMakeCode = Convert.ToInt32(dr["VEHICLE_MAKE_CD"]),
            MakeDescription = Convert.ToString(dr["MAKE_DESCRIPTION"]),
            VehicleModelCode = Convert.ToInt32(dr["VEHICLE_MODEL_CD"]),
            ModelDescription = Convert.ToString(dr["MODEL_DESCRIPTION"]),
            VehicleVersionCode = Convert.ToInt32(dr["VEHICLE_VERSION_CD"]),
            VersionDescription = Convert.ToString(dr["VERSION_DESCRIPTION"]),
            VehiclePrice = Convert.ToInt32(dr["VEHICLE_PRICE"]),
            VehicleTypeCode = Convert.ToInt32(dr["VEHICLE_TYPE_CD"]),          
            VehicleTypeDescription = Convert.ToString(dr["TYPE_DESCRIPTION"]),           
            FasecoldaMakeId = Convert.ToString(dr["FASECOLDA_MAKE_ID"]),
            FasecoldaModelId = Convert.ToString(dr["FASECOLDA_MODEL_ID"]),
        };
        #endregion
    }
}
