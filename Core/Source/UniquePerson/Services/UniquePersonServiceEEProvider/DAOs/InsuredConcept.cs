using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Linq;
namespace Sistran.Core.Application.UniquePersonService.DAOs
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
            filter.Property(UniquePerson.Entities.InsuredConcept.Properties.InsuredCode, typeof(UniquePerson.Entities.InsuredConcept).Name);
            filter.Equal();
            filter.Constant(insuredConcept.InsuredCode);
            UniquePerson.Entities.InsuredConcept insuredConceptEntityUpdate = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                insuredConceptEntityUpdate = (UniquePerson.Entities.InsuredConcept)daf.List(typeof(UniquePerson.Entities.InsuredConcept), filter.GetPredicate()).FirstOrDefault();
            }
            if (insuredConceptEntityUpdate != null)
            {

                insuredConceptEntityUpdate.IsInsured = insuredConcept.IsInsured;
                insuredConceptEntityUpdate.IsHolder = insuredConcept.IsHolder;
                insuredConceptEntityUpdate.IsBeneficiary = insuredConcept.IsBeneficiary;
                insuredConceptEntityUpdate.IsConsortium = insuredConcept.IsConsortium;
                insuredConceptEntityUpdate.IsPayer = insuredConcept.IsPayer;
                insuredConceptEntityUpdate.IsRepresentative = insuredConcept.IsRepresentative;
                insuredConceptEntityUpdate.IsSurety = insuredConcept.IsSurety;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.UpdateObject(insuredConceptEntityUpdate);
                }
                return ModelAssembler.CreateInsuredConcept(insuredConceptEntityUpdate);

            }
            else
            {
                UniquePerson.Entities.InsuredConcept insuredConceptEntities = EntityAssembler.CreateInsuredConcept(insuredConcept);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.InsertObject(insuredConceptEntities);
                }
                return ModelAssembler.CreateInsuredConcept(insuredConceptEntities);
            }

        }
    }
}
