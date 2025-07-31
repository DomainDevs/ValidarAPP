using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Representante Legal
    /// </summary>
    public class LegalRepresentativeDAO
    {
        /// <summary>
        /// Guarda los datos d eun representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns>
        public Models.LegalRepresentative CrateRepresentLegal(Models.LegalRepresentative legalRepresent, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            legalRepresent.Id = individualId;
            IndividualLegalRepresent legalRepresentEntity = EntityAssembler.CreateLegalRepresent(legalRepresent);
            legalRepresentEntity.IndividualId = individualId;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(legalRepresentEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CrateRepresentLegal");
            return ModelAssembler.CreateLegalRepresent(legalRepresentEntity);
        }
        /// <summary>
        /// Buscar Representante Legal 
        /// </summary>
        /// <param name="idCardNo">Numero de documento</param>
        /// <param name="idCardTypeCode">tipo de documento</param>
        /// <returns></returns>
        public Models.LegalRepresentative GetLegalRepresentByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualLegalRepresent.Properties.IndividualId, typeof(IndividualLegalRepresent).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualLegalRepresent), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetLegalRepresentByIndividualId");
            return ModelAssembler.CreateLegalRepresents(businessCollection).FirstOrDefault();

        }
        /// <summary>
        /// Actualizar informacion del Representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns>
        public Models.LegalRepresentative UpdateLegalRepresent(Models.LegalRepresentative legalRepresent, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualLegalRepresent.Properties.IndividualId, typeof(IndividualLegalRepresent).Name);
            filter.Equal();
            filter.Constant(individualId);



            IndividualLegalRepresent LegaLRepresentEntity = (IndividualLegalRepresent)DataFacadeManager.Instance.GetDataFacade().List(typeof(IndividualLegalRepresent), filter.GetPredicate()).FirstOrDefault();

            if (LegaLRepresentEntity == null)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateLegalRepresent");
                return CrateRepresentLegal(legalRepresent, individualId);
            }
            else
            {
                long phone;
                bool a = Int64.TryParse(legalRepresent.Phone, out phone);
                LegaLRepresentEntity.ExpeditionDate = legalRepresent.ExpeditionDate;
                LegaLRepresentEntity.LegalRepresentativeName = legalRepresent.FullName;
                LegaLRepresentEntity.BirthDate = legalRepresent.BirthDate;
                LegaLRepresentEntity.Nationality = legalRepresent.Nationality;
                LegaLRepresentEntity.Phone = String.IsNullOrEmpty(legalRepresent.Phone) ? null : (long?)phone;
                LegaLRepresentEntity.CellPhone = String.IsNullOrEmpty(legalRepresent.CellPhone) ? null : (long?)Int64.Parse(legalRepresent.CellPhone);
                LegaLRepresentEntity.Email = legalRepresent.Email;
                LegaLRepresentEntity.CurrencyCode = legalRepresent.AuthorizationAmount.Currency.Id;
                LegaLRepresentEntity.Description = legalRepresent.Description;
                LegaLRepresentEntity.Address = legalRepresent.Address;
                LegaLRepresentEntity.CountryCode = legalRepresent.City.State.Country.Id;
                LegaLRepresentEntity.StateCode = legalRepresent.City.State.Id;
                LegaLRepresentEntity.City = legalRepresent.City.Description;
                LegaLRepresentEntity.ExpeditionPlace = legalRepresent.ExpeditionPlace;
                LegaLRepresentEntity.BirthPlace = legalRepresent.BirthPlace;
                LegaLRepresentEntity.JobTitle = legalRepresent.JobTitle;
                LegaLRepresentEntity.IdCardNo = legalRepresent.IdentificationDocument.Number;
                LegaLRepresentEntity.AuthorizationAmount = legalRepresent.AuthorizationAmount.Value;
                LegaLRepresentEntity.IdCardTypeCode = legalRepresent.IdentificationDocument.DocumentType.Id;
                LegaLRepresentEntity.CityCode = legalRepresent.City.Id;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(LegaLRepresentEntity);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateLegalRepresent");
                return ModelAssembler.CreateLegalRepresent(LegaLRepresentEntity);
            }


        }


    }
}
