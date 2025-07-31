using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Linq;
namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Conceptos Asegurado
    /// </summary>
    public class InsuredConceptDAO
    {

        /// <summary>
        /// Crear Conceptos Asegurado
        /// </summary>
        /// <param name="insuredConcept">The insured concept.</param>
        /// <returns></returns>
        public Models.InsuredConcept CreateInsuredConcept(Models.InsuredConcept insuredConcept)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.InsuredConcept.Properties.InsuredCode, typeof(UniquePersonV1.Entities.InsuredConcept).Name);
            filter.Equal();
            filter.Constant(insuredConcept.InsuredCode);
            UniquePersonV1.Entities.InsuredConcept insuredConceptEntityUpdate = (UniquePersonV1.Entities.InsuredConcept)DataFacadeManager.Instance.GetDataFacade().List(typeof(UniquePersonV1.Entities.InsuredConcept), filter.GetPredicate()).FirstOrDefault();
            if (insuredConceptEntityUpdate != null)
            {

                insuredConceptEntityUpdate.IsInsured = insuredConcept.IsInsured;
                insuredConceptEntityUpdate.IsHolder = insuredConcept.IsHolder;
                insuredConceptEntityUpdate.IsBeneficiary = insuredConcept.IsBeneficiary;
                insuredConceptEntityUpdate.IsConsortium = insuredConcept.IsConsortium;
                insuredConceptEntityUpdate.IsPayer = insuredConcept.IsPayer;
                insuredConceptEntityUpdate.IsRepresentative = insuredConcept.IsRepresentative;
                insuredConceptEntityUpdate.IsSurety = insuredConcept.IsSurety;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(insuredConceptEntityUpdate);
                return ModelAssembler.CreateInsuredConcept(insuredConceptEntityUpdate);

            }
            else
            {
                UniquePersonV1.Entities.InsuredConcept insuredConceptEntities = EntityAssembler.CreateInsuredConcept(insuredConcept);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(insuredConceptEntities);
                return ModelAssembler.CreateInsuredConcept(insuredConceptEntities);
            }

        }
    }
}
