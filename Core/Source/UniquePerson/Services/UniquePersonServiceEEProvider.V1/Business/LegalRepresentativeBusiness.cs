using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using modelsCommon = Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class LegalRepresentativeBusiness
    {

        /// <summary>
        /// Guarda los datos d eun representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns>
        public Models.LegalRepresentative CrateRepresentLegal(Models.LegalRepresentative legalRepresent)
        {
            IndividualLegalRepresent legalRepresentEntity = EntityAssembler.CreateLegalRepresent(legalRepresent);
            DataFacadeManager.Insert(legalRepresentEntity);
            return ModelAssembler.CreateLegalRepresent(legalRepresentEntity);
        }

        /// <summary>
        /// Actualiza los datos d eun representante legal
        /// </summary>
        /// <param name="legalRepresent"></param>
        /// <returns></returns>
        public Models.LegalRepresentative UpdateRepresentLegal(Models.LegalRepresentative legalRepresent)
        {
            IndividualLegalRepresent legalRepresentEntity = EntityAssembler.CreateLegalRepresent(legalRepresent);
            DataFacadeManager.Update(legalRepresentEntity);
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
            PrimaryKey primaryKey = IndividualLegalRepresent.CreatePrimaryKey(individualId);
            IndividualLegalRepresent legalRepresentEntity = (IndividualLegalRepresent)DataFacadeManager.GetObject(primaryKey);
            if (legalRepresentEntity != null)
            {
                var result = ModelAssembler.CreateLegalRepresent(legalRepresentEntity);
                List<modelsCommon.State> states = DelegateService.commonServiceCore.GetStatesByCountryId(result.City.State.Country.Id);
                result.City.State.Description = states.First(x => x.Id == result.City.State.Id).Description;
                List<modelsCommon.Country> contries = DelegateService.commonServiceCore.GetCountries();
                result.City.State.Country.Description = contries.First(x => x.Id == result.City.State.Country.Id).Description;
                return result;
            }
            else
            {
                return null;
            }
        }

    }
}
