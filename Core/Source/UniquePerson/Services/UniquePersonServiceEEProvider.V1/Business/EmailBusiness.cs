using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ENUP = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class EmailBusiness
    {
        public List<MOUP.Email> CreateEmails(int individualId, List<MOUP.Email> emails)
        {
            foreach (MOUP.Email email in emails)
            {
                email.Id = CreateEmail(individualId, email).Id;

            }

            return emails;
        }

        public MOUP.Email CreateEmail(int individualId, MOUP.Email email)
        {
            ENUP.Email entityEmail = EntityAssembler.CreateEmail(email);

            entityEmail.IndividualId = individualId;
            entityEmail = DataFacadeManager.Insert(entityEmail) as ENUP.Email;
            return ModelAssembler.CreateEmail(entityEmail);

        }

        public List<MOUP.Email> UpdateEmails(int individualId, List<MOUP.Email> emails)
        {
            foreach (MOUP.Email email in emails)
            {
                UpdateEmail(individualId, email);

            }

            return emails;
        }

        public MOUP.Email UpdateEmail(int individualId, MOUP.Email email)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.Email.CreatePrimaryKey(individualId, email.Id);
            ENUP.Email entityEmail = (ENUP.Email)DataFacadeManager.GetObject(primaryKey);

            entityEmail.EmailTypeCode = email.EmailType.Id;
            entityEmail.Address = email.Description;
            entityEmail.IsMailingAddress = email.IsPrincipal;
            DataFacadeManager.Update(entityEmail);

            return ModelAssembler.CreateEmail(entityEmail);
        }

        public List<Models.Email> GetEmails(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Email.Properties.IndividualId, typeof(Email).Name);
            filter.Equal();
            filter.Constant(individualId);

            var businessCollection = DataFacadeManager.GetObjects(typeof(Email), filter.GetPredicate());

            List<MOUP.Email> emails = ModelAssembler.CreateEmails(businessCollection);

            businessCollection = DataFacadeManager.GetObjects(typeof(EmailType));

            List<MOUP.EmailType> emailTypes = ModelAssembler.CreateEmailTypes(businessCollection);

            foreach (MOUP.Email item in emails)
            {
                if (item.EmailType.Id != 0)
                {
                    item.EmailType = emailTypes.First(x => x.Id == item.EmailType.Id);
                }
            }
            return emails;
        }

        public List<Models.EmailType> GetEmailTypes()
        {
            try
            {
                return ModelAssembler.CreateEmailTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.EmailType)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}