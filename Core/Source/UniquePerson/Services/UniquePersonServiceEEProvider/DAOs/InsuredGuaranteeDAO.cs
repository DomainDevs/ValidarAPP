using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    ///  garantia del asegurado
    /// </summary>
    public class InsuredGuaranteeDAO
    {
        /// <summary>
        /// Crea nueva garantia del asegurado
        /// </summary>
        /// <param name="newGuarantee">Nueva bitacora garantia</param>
        /// <returns>Nueva bitacora garantia</returns>
        public Models.InsuredGuarantee CreateNewGuarantee(Models.InsuredGuarantee insuredGuarantee)
        {

            try
            {
                Parameter paramGuarantee = FacadeDAO.GetParameterById((int)ParameterType.Guarantee);
                if (paramGuarantee != null && paramGuarantee.NumberParameter.HasValue)
                {
                    paramGuarantee.NumberParameter = paramGuarantee.NumberParameter.Value + 1;
                    insuredGuarantee.Id = paramGuarantee.NumberParameter.Value;
                }
                else
                {
                    throw new Exception(String.Format("Error in CreateNewGuarantee", Enum.GetName(typeof(ParameterType), ParameterType.Guarantee)));
                }
                if (insuredGuarantee.Code == (int)Enums.GuaranteeType.Mortgage)
                {
                    Parameter paramGuaranteeNoteNum = FacadeDAO.GetParameterById((int)ParameterType.GuaranteeNote);
                    if (paramGuaranteeNoteNum != null && paramGuaranteeNoteNum.NumberParameter.HasValue)
                    {
                        paramGuaranteeNoteNum.NumberParameter = paramGuaranteeNoteNum.NumberParameter.Value + 1;
                        insuredGuarantee.DocumentNumber = paramGuaranteeNoteNum.NumberParameter.Value.ToString();
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(paramGuaranteeNoteNum);
                    }
                    else
                    {
                        throw new Exception(String.Format("Error in CreateNewGuarantee", Enum.GetName(typeof(ParameterType), ParameterType.GuaranteeNote)));
                    }
                }

                InsuredGuarantee insuredGuaranteeEntity = EntityAssembler.CreateInsuredGuarantee(insuredGuarantee);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(insuredGuaranteeEntity);
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(paramGuarantee);
                Models.InsuredGuarantee newInsuredGuarantee = ModelAssembler.CreateInsuredGuarantee(insuredGuaranteeEntity, insuredGuarantee.GuaranteeStatus);
                newInsuredGuarantee.Guarantors = insuredGuarantee.Guarantors;
                newInsuredGuarantee.listDocumentation = insuredGuarantee.listDocumentation;
                newInsuredGuarantee.listPrefix = insuredGuarantee.listPrefix;
                newInsuredGuarantee.InsuredGuaranteeLog = insuredGuarantee.InsuredGuaranteeLog;
                newInsuredGuarantee.InsuredGuaranteeLogObject = insuredGuarantee.InsuredGuaranteeLogObject;


                return newInsuredGuarantee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza garantia del asegurado
        /// </summary>
        /// <param name="newGuarantee">Bitacora garantia</param>
        /// <returns>Bitacora garantia</returns>
        public Models.InsuredGuarantee UpdateGuarantee(Models.InsuredGuarantee insuredGuarantee)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(InsuredGuarantee.Properties.IndividualId, typeof(InsuredGuarantee.Properties).Name);
                filter.Equal();
                filter.Constant(insuredGuarantee.IndividualId);
                filter.And();
                filter.Property(InsuredGuarantee.Properties.GuaranteeId, typeof(InsuredGuarantee.Properties).Name);
                filter.Equal();
                filter.Constant(insuredGuarantee.Id);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(InsuredGuarantee), filter.GetPredicate()));
                InsuredGuarantee updateInsuredGuarantee = (InsuredGuarantee)(businessCollection).FirstOrDefault();
                if (updateInsuredGuarantee != null)
                {
                    updateInsuredGuarantee.Description = insuredGuarantee.Description;
                    updateInsuredGuarantee.GuaranteeCode = insuredGuarantee.Code;
                    updateInsuredGuarantee.CurrencyCode = insuredGuarantee.Currency != null ? (int?)insuredGuarantee.Currency.Id : null;
                    updateInsuredGuarantee.CountryCode = insuredGuarantee.Country != null ? (int?)insuredGuarantee.Country.Id : null;
                    updateInsuredGuarantee.StateCode = insuredGuarantee.State != null ? (int?)insuredGuarantee.State.Id : null;
                    updateInsuredGuarantee.CityCode = insuredGuarantee.City != null ? (int?)insuredGuarantee.City.Id : null;
                    updateInsuredGuarantee.Address = insuredGuarantee.Address;
                    updateInsuredGuarantee.AppraisalDate = insuredGuarantee.AppraisalDate;
                    updateInsuredGuarantee.ExpertName = insuredGuarantee.ExpertName;
                    updateInsuredGuarantee.GuaranteeAmount = insuredGuarantee.GuaranteeAmount;
                    updateInsuredGuarantee.InsuranceValueAmount = insuredGuarantee.InsuranceAmount;
                    updateInsuredGuarantee.GuaranteePolicyNumber = insuredGuarantee.PolicyNumber;
                    updateInsuredGuarantee.IssuerName = insuredGuarantee.IssuerName;
                    updateInsuredGuarantee.DocumentNumber = insuredGuarantee.DocumentNumber;
                    updateInsuredGuarantee.ExpDate = insuredGuarantee.ExpirationDate;
                    updateInsuredGuarantee.GuaranteeStatusCode = insuredGuarantee.GuaranteeStatus != null ? (int?)insuredGuarantee.GuaranteeStatus.Code : null;
                    updateInsuredGuarantee.ClosedInd = insuredGuarantee.IsCloseInd;
                    updateInsuredGuarantee.PromissoryNoteTypeCode = insuredGuarantee.PromissoryNoteType != null ? (int?)insuredGuarantee.PromissoryNoteType.Id : null;
                    updateInsuredGuarantee.LineBusinessCode = insuredGuarantee.BusinessLineCode;
                    updateInsuredGuarantee.RegistrationNumber = insuredGuarantee.RegistrationNumber;
                    updateInsuredGuarantee.RegistrationDate = insuredGuarantee.RegistrationDate;
                    updateInsuredGuarantee.LastChangeDate = insuredGuarantee.LastChangeDate;
                    updateInsuredGuarantee.LicensePlate = insuredGuarantee.LicensePlate;
                    updateInsuredGuarantee.EngineSerNro = insuredGuarantee.EngineNro;
                    updateInsuredGuarantee.ChassisSerNo = insuredGuarantee.ChassisNro;
                    updateInsuredGuarantee.VehicleMakeCode = insuredGuarantee.VehicleMake;
                    updateInsuredGuarantee.VehicleModelCode = insuredGuarantee.VehicleModel;
                    updateInsuredGuarantee.VehicleVersionCode = insuredGuarantee.VehicleVersion;
                    updateInsuredGuarantee.BranchCode = insuredGuarantee.Branch != null ? (int?)insuredGuarantee.Branch.Id : null;
                    updateInsuredGuarantee.BuiltAreaQuantity = insuredGuarantee.BuiltArea;
                    updateInsuredGuarantee.DeedNumber = insuredGuarantee.DeedNumber;
                    updateInsuredGuarantee.AppraisalAmount = insuredGuarantee.AppraisalAmount;
                    updateInsuredGuarantee.SignatoriesNum = insuredGuarantee.SignatoriesNumber;
                    updateInsuredGuarantee.GuaranteeDescriptionOthers = insuredGuarantee.DescriptionOthers;
                    updateInsuredGuarantee.Apostille = insuredGuarantee.Apostille;
                    updateInsuredGuarantee.DocumentValueAmount = insuredGuarantee.DocumentValueAmount;
                    updateInsuredGuarantee.MeasurementTypeCode = insuredGuarantee.MeasurementType != null ? (int?)insuredGuarantee.MeasurementType.Code : null;
                    updateInsuredGuarantee.MeasureAreaQuantity = insuredGuarantee.MeasureArea;
                    updateInsuredGuarantee.InsuranceCompanyId = insuredGuarantee.InsuranceCompany != null ? (int?)insuredGuarantee.InsuranceCompany.Id : null;
                    updateInsuredGuarantee.InsuranceCompany = insuredGuarantee.InsuranceCompany != null ? insuredGuarantee.InsuranceCompany.Description : null;
                    updateInsuredGuarantee.AssetTypeCode = insuredGuarantee.AssetTypeCode;
                    updateInsuredGuarantee.RealstateMatriculation = insuredGuarantee.RealstateMatriculation;
                    updateInsuredGuarantee.ConstitutionDate = insuredGuarantee.ConstitutionDate;
                    updateInsuredGuarantee.MortgagerName = insuredGuarantee.MortgagerName;
                    updateInsuredGuarantee.DepositEntity = insuredGuarantee.DepositEntity;
                    updateInsuredGuarantee.DepositDate = insuredGuarantee.DepositDate;
                    updateInsuredGuarantee.Depositor = insuredGuarantee.Depositor;
                    updateInsuredGuarantee.Constituent = insuredGuarantee.Constituent;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(updateInsuredGuarantee);
                    Models.InsuredGuarantee newInsuredGuarantee = ModelAssembler.CreateInsuredGuarantee(updateInsuredGuarantee, insuredGuarantee.GuaranteeStatus);
                    newInsuredGuarantee.Guarantors = insuredGuarantee.Guarantors;
                    newInsuredGuarantee.listDocumentation = insuredGuarantee.listDocumentation;
                    newInsuredGuarantee.listPrefix = insuredGuarantee.listPrefix;
                    newInsuredGuarantee.InsuredGuaranteeLog = insuredGuarantee.InsuredGuaranteeLog;
                    newInsuredGuarantee.InsuredGuaranteeLogObject = insuredGuarantee.InsuredGuaranteeLogObject;
                    return newInsuredGuarantee;
                }
                else
                {
                    return CreateNewGuarantee(insuredGuarantee);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

