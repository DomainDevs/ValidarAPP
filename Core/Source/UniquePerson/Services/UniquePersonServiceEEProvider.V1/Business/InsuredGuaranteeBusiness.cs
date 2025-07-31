using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Linq;
using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.Enums;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class InsuredGuaranteeBusiness
    {

        public List<Models.GuaranteeInsuredGuarantee> GetInsuredGuaranteesByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            string tableAlias = typeof(InsuredGuarantee).Name;
            filter.PropertyEquals(InsuredGuarantee.Properties.IndividualId, tableAlias, individualId);

            GuaranteeInsuredGuaranteeViewV1 view = new GuaranteeInsuredGuaranteeViewV1();
            ViewBuilder builder = new ViewBuilder("GuaranteeInsuredGuaranteeViewV1");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (!view.InsuredGuarantees.Any())
            {
                return null;
            }

            List<Models.InsuredGuarantee> insuredGuarantees = ModelAssembler.CreateInsuredGuarantees(view.InsuredGuarantees);
            List<Models.Guarantee> guarantees = ModelAssembler.CreateGuarantees(view.Guarantees);

            List<Models.GuaranteeInsuredGuarantee> list = new List<Models.GuaranteeInsuredGuarantee>();

            foreach (Models.InsuredGuarantee item in insuredGuarantees)
            {
                Models.GuaranteeInsuredGuarantee model = new Models.GuaranteeInsuredGuarantee();
                model.Id = item.Id;
                model.IndividualId = item.IndividualId;
                model.Description = guarantees.First(x => x.Id == item.Guarantee.Id).Description + " " + item.Id;
                model.typeId = guarantees.First(x => x.Id == item.Guarantee.Id).Id;
                list.Add(model);
            }

            return list;
        }

        private int GetInsuredGuarantee()
        {

            Parameter paramGuarantee = FacadeDAO.GetParameterById((int)ParameterType.Guarantee);
            if (paramGuarantee != null && paramGuarantee.NumberParameter.HasValue)
            {
                paramGuarantee.NumberParameter = paramGuarantee.NumberParameter.Value + 1;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(paramGuarantee);
                return paramGuarantee.NumberParameter.Value;
            }
            else
            {
                throw new Exception(String.Format("Error in CreateNewGuarantee", Enum.GetName(typeof(ParameterType), ParameterType.Guarantee)));
            }
        }

        #region Mortgage 

        public Models.InsuredGuaranteeMortgage GetInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id)
        {
            PrimaryKey primaryKey = InsuredGuarantee.CreatePrimaryKey(individualId, id);
            InsuredGuarantee InsuredGuaranteeEntity = (InsuredGuarantee)DataFacadeManager.GetObject(primaryKey);

            if (InsuredGuaranteeEntity != null)
            {
                GuaranteeBusiness business = new GuaranteeBusiness();
                var result = ModelAssembler.CreateInsuredGuaranteeMortgage(InsuredGuaranteeEntity);
                result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
                result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Models.InsuredGuaranteeMortgage CreateInsuredGuaranteeMortgage(Models.InsuredGuaranteeMortgage insuredGuaranteeMortgage)
        {


            insuredGuaranteeMortgage.Id = GetInsuredGuarantee();
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteeMortgage(insuredGuaranteeMortgage);
            DataFacadeManager.Insert(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteeMortgage(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        public Models.InsuredGuaranteeMortgage UpdateInsuredGuaranteeMortgage(Models.InsuredGuaranteeMortgage insuredGuaranteeMortgage)
        {
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteeMortgage(insuredGuaranteeMortgage);
            if (insuredGuarantee.RegistrationDate.Value.Year < 1111)
            {
                insuredGuarantee.RegistrationDate = null;
            }
            DataFacadeManager.Update(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteeMortgage(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        #endregion

        #region Pledge

        public Models.InsuredGuaranteePledge GetInsuredGuaranteePledgeByIndividualIdById(int individualId, int id)
        {
            PrimaryKey primaryKey = InsuredGuarantee.CreatePrimaryKey(individualId, id);
            InsuredGuarantee InsuredGuaranteeEntity = (InsuredGuarantee)DataFacadeManager.GetObject(primaryKey);

            if (InsuredGuaranteeEntity != null)
            {
                GuaranteeBusiness business = new GuaranteeBusiness();
                var result = ModelAssembler.CreateInsuredGuaranteePledge(InsuredGuaranteeEntity);
                result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
                result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Models.InsuredGuaranteePledge CreateInsuredGuaranteePledge(Models.InsuredGuaranteePledge insuredGuaranteePledge)
        {
            insuredGuaranteePledge.Id = GetInsuredGuarantee();
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteePledge(insuredGuaranteePledge);
            DataFacadeManager.Insert(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteePledge(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        public Models.InsuredGuaranteePledge UpdateInsuredGuaranteePledge(Models.InsuredGuaranteePledge insuredGuaranteePledge)
        {
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteePledge(insuredGuaranteePledge);
            if (insuredGuarantee.RegistrationDate.Value.Year < 1911)
            {
                insuredGuarantee.RegistrationDate = null;
            }
            DataFacadeManager.Update(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteePledge(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        #endregion

        #region PromissoryNote

        public Models.InsuredGuaranteePromissoryNote GetInsuredGuaranteePromissoryNoteByIndividualIdById(int individualId, int guaranteeid)
        {
            PrimaryKey primaryKey = InsuredGuarantee.CreatePrimaryKey(individualId, guaranteeid);
            InsuredGuarantee InsuredGuaranteeEntity = (InsuredGuarantee)DataFacadeManager.GetObject(primaryKey);

            if (InsuredGuaranteeEntity != null)
            {
                GuaranteeBusiness business = new GuaranteeBusiness();

                var result = ModelAssembler.CreateInsuredGuaranteePromissoryNote(InsuredGuaranteeEntity);
                result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
                result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Models.InsuredGuaranteePromissoryNote CreateInsuredGuaranteePromissoryNote(Models.InsuredGuaranteePromissoryNote guaranteePromissoryNote)
        {
            guaranteePromissoryNote.Id = GetInsuredGuarantee();
            guaranteePromissoryNote.DocumentNumber = CreateDocumentNumnber().ToString();
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteePromissoryNote(guaranteePromissoryNote);
            DataFacadeManager.Insert(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteePromissoryNote(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        public Models.InsuredGuaranteePromissoryNote UpdateInsuredGuaranteePromissoryNote(Models.InsuredGuaranteePromissoryNote guaranteePromissoryNote)
        {
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteePromissoryNote(guaranteePromissoryNote);

            if (insuredGuarantee.RegistrationDate.Value.Year < 1911)
            {
                insuredGuarantee.RegistrationDate = null;
            }
            DataFacadeManager.Update(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteePromissoryNote(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        private int CreateDocumentNumnber()
        {

            Parameter paramGuaranteeNoteNum = FacadeDAO.GetParameterById((int)ParameterType.GuaranteeNote);
            if (paramGuaranteeNoteNum != null && paramGuaranteeNoteNum.NumberParameter.HasValue)
            {
                paramGuaranteeNoteNum.NumberParameter = paramGuaranteeNoteNum.NumberParameter.Value + 1;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(paramGuaranteeNoteNum);
                return paramGuaranteeNoteNum.NumberParameter.Value;
            }
            else
            {
                throw new Exception(String.Format("Error in CreateNewGuarantee", Enum.GetName(typeof(ParameterType), ParameterType.GuaranteeNote)));
            }
        }

        #endregion

        #region FixedTermDeposit

        public Models.InsuredGuaranteeFixedTermDeposit GetInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id)
        {
            PrimaryKey primaryKey = InsuredGuarantee.CreatePrimaryKey(individualId, id);
            InsuredGuarantee InsuredGuaranteeEntity = (InsuredGuarantee)DataFacadeManager.GetObject(primaryKey);

            if (InsuredGuaranteeEntity != null)
            {
                GuaranteeBusiness business = new GuaranteeBusiness();
                var result = ModelAssembler.CreateInsuredGuaranteeFixedTermDeposit(InsuredGuaranteeEntity);
                result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
                result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Models.InsuredGuaranteeFixedTermDeposit CreateInsuredGuaranteeFixedTermDeposit(Models.InsuredGuaranteeFixedTermDeposit guaranteeFixedTermDeposit)
        {
            guaranteeFixedTermDeposit.Id = GetInsuredGuarantee();
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteeFixedTermDeposit(guaranteeFixedTermDeposit);
            DataFacadeManager.Insert(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteeFixedTermDeposit(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        public Models.InsuredGuaranteeFixedTermDeposit UpdateInsuredGuaranteeFixedTermDeposit(Models.InsuredGuaranteeFixedTermDeposit insuredGuaranteeFixed)
        {
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteeFixedTermDeposit(insuredGuaranteeFixed);
            if (insuredGuarantee.RegistrationDate.Value.Year < 1911)
            {
                insuredGuarantee.RegistrationDate = null;
            }
            DataFacadeManager.Update(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteeFixedTermDeposit(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.City = DelegateService.commonServiceCore.GetCityByCity(result.City);
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        #endregion

        #region Others

        public Models.InsuredGuaranteeOthers GetInsuredGuaranteeOthersByIndividualIdById(int individualId, int id)
        {
            PrimaryKey primaryKey = InsuredGuarantee.CreatePrimaryKey(individualId, id);
            InsuredGuarantee InsuredGuaranteeEntity = (InsuredGuarantee)DataFacadeManager.GetObject(primaryKey);

            if (InsuredGuaranteeEntity != null)
            {
                GuaranteeBusiness business = new GuaranteeBusiness();
                var result = ModelAssembler.CreateInsuredGuaranteeOthers(InsuredGuaranteeEntity);
                result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Models.InsuredGuaranteeOthers CreateInsuredGuaranteeOthers(Models.InsuredGuaranteeOthers guaranteeFixedTermDeposit)
        {
            guaranteeFixedTermDeposit.Id = GetInsuredGuarantee();
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteeOthers(guaranteeFixedTermDeposit);
            DataFacadeManager.Insert(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteeOthers(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        public Models.InsuredGuaranteeOthers UpdateInsuredGuaranteeOthers(Models.InsuredGuaranteeOthers insuredGuaranteeFixed)
        {
            InsuredGuarantee insuredGuarantee = EntityAssembler.CreateInsuredGuaranteeOthers(insuredGuaranteeFixed);
            if (insuredGuarantee.RegistrationDate.Value.Year < 1911)
            {
                insuredGuarantee.RegistrationDate = null;
            }
            DataFacadeManager.Update(insuredGuarantee);
            var result = ModelAssembler.CreateInsuredGuaranteeOthers(insuredGuarantee);
            GuaranteeBusiness business = new GuaranteeBusiness();
            result.Guarantee = business.GetGuaranteeByGuaranteeId(result.Guarantee.Id);
            return result;
        }

        #endregion

    }
}
