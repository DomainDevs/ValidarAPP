using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Email
    /// </summary>
    public class EmailDAO
    {
        /// <summary>
        /// Obtener lista de tipos de email
        /// </summary>
        /// <returns></returns>
        public List<models.EmailType> GetEmailTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EmailType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetEmailTypes");
            return ModelAssembler.CreateEmailTypes(businessCollection);
        }

        /// <summary>
        /// Obtener lista de emails asociados a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        public List<models.Email> GetEmailsByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Email.Properties.IndividualId, typeof(Email).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Email), filter.GetPredicate()));
            }

            List<models.Email> emails = ModelAssembler.CreateEmails(businessCollection);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(EmailType)));
            }
            List<models.EmailType> emailTypes = ModelAssembler.CreateEmailTypes(businessCollection);
            foreach (models.Email item in emails)
            {
                if (item.EmailType.Id != 0)
                {
                    item.EmailType = emailTypes.First(x => x.Id == item.EmailType.Id);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetEmailsByIndividualId");
            return emails;
        }

        /// <summary>
        /// Obtener lista de emails por un filtro especifico
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        public List<models.Email> GetEmailsByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Email), filter.GetPredicate()));
            }
            return ModelAssembler.CreateEmails(businessCollection);
        }

        /// <summary>
        /// Crear nuevo email
        /// </summary>
        /// <param name="email">Modelo email</param>
        /// <returns></returns>
        public models.Email CreateEmail(models.Email email, int individualId)
        {
            Email emailEntity = EntityAssembler.CreateEmail(email);
            emailEntity.IndividualId = individualId;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.InsertObject(emailEntity);
            }
            return ModelAssembler.CreateEmail(emailEntity);

        }

        /// <summary>
        /// Actualizar email
        /// </summary>
        /// <param name="email">Modelo email</param>
        /// <returns></returns>
        public models.Email UpdateEmail(models.Email email, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Email.Properties.IndividualId, typeof(Email).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(Email.Properties.DataId, typeof(Email).Name);
            filter.Equal();
            filter.Constant(email.Id);

            List<models.Email> emails = GetEmailsByFilter(filter);
            if (emails.Count > 0)
            {
                PrimaryKey key = Email.CreatePrimaryKey(individualId, email.Id);
                Email emailEntity = new Email();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    emailEntity = (Email)daf.GetObjectByPrimaryKey(key);
                }
                emailEntity.EmailTypeCode = email.EmailType.Id;
                emailEntity.Address = email.Description;
                emailEntity.IsMailingAddress = email.IsPrincipal;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.UpdateObject(emailEntity);
                }
                return ModelAssembler.CreateEmail(emailEntity);
            }
            else
            {
                return CreateEmail(email, individualId);

            }
        }
    }
}
