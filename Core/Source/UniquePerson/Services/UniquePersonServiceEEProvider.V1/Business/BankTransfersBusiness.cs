using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class BankTransfersBusiness
    {
        public List<BankTransfers> CreateBankTransfers(List<BankTransfers> listCompanyBankTransfers)
        {
            List<BankTransfers> bankTransfers = new List<BankTransfers>();
            List<PersonAccountBank> companyBankTransfersEntity = EntityAssembler.CreatePersonAccountBank(listCompanyBankTransfers);
            foreach (PersonAccountBank personAccountBank in companyBankTransfersEntity)
            {
                personAccountBank.InscriptionDate = DateTime.Now;
                DataFacadeManager.Insert(personAccountBank);
                bankTransfers = GetPersonAccountBankByIndividualId(personAccountBank.IndividualId);
            }

            return bankTransfers;

        }

        public BankTransfers UpdateBankTransfers(BankTransfers companyBankTransfers)
        {

            PrimaryKey primaryKey = UniquePersonV1.Entities.PersonAccountBank.CreatePrimaryKey(companyBankTransfers.Id, companyBankTransfers.Individual);
            UniquePersonV1.Entities.PersonAccountBank entityCompanyBank = (UniquePersonV1.Entities.PersonAccountBank)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (entityCompanyBank != null)
            {
                entityCompanyBank.AccountTypeCode = companyBankTransfers.AccountType.Code;
                entityCompanyBank.Active = companyBankTransfers.ActiveAccount;
                entityCompanyBank.BankBranch = companyBankTransfers.BankBranch;
                entityCompanyBank.BankCode = companyBankTransfers.Bank.Id;
                entityCompanyBank.BankIntermediary = companyBankTransfers.IntermediaryBank;
                entityCompanyBank.BankSquare = companyBankTransfers.BankSquare;
                entityCompanyBank.Beneficiary = companyBankTransfers.PaymentBeneficiary;
                entityCompanyBank.CurrencyCode = companyBankTransfers.Currency.Id;
                entityCompanyBank.DefaultAccount = companyBankTransfers.DefaultAccount;
                entityCompanyBank.Number = companyBankTransfers.AccountNumber;


                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCompanyBank);
            }
            return ModelAssembler.CreateBankTransfers(entityCompanyBank);
        }

        /// <summary>
        /// Consultar razones sociales por individualID
        /// </summary>
        public List<BankTransfers> GetPersonAccountBankByIndividualId(int individualId)
        {
            List<BankTransfers> BankTransfers = new List<BankTransfers>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.PersonAccountBank.Properties.IndividualId, typeof(UniquePersonV1.Entities.PersonAccountBank).Name);
            filter.Equal();
            filter.Constant(individualId);

            PersonAccountBankViewV1 personAccountBankViewV1 = new PersonAccountBankViewV1();
            ViewBuilder builder = new ViewBuilder("PersonAccountBankViewV1");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, personAccountBankViewV1);
            
            if (personAccountBankViewV1.PersonAccountBank.Count > 0)
            {
                BankTransfers = ModelAssembler.CreateBanksTransfers(personAccountBankViewV1.PersonAccountBank);
                foreach (BankTransfers bankTransfers in BankTransfers)
                {
                    bankTransfers.Bank.Description = personAccountBankViewV1.Bank.Cast<Bank>().FirstOrDefault(x => x.BankCode == bankTransfers.Bank.Id)?.Description;
                    bankTransfers.AccountType.Description = personAccountBankViewV1.AccountType.Cast<Common.Entities.AccountType>().FirstOrDefault(x => x.AccountTypeCode == bankTransfers.AccountType.Code)?.Description;
                }
            }
            return BankTransfers;
        }
    }
}
