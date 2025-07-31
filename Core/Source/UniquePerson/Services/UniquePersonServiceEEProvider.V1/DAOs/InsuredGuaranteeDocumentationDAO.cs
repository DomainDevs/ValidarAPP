using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// documentación asociada a una contragarantía
    /// </summary>
    public class InsuredGuaranteeDocumentationDAO
    {
        /// <summary>
        /// Consulta documentación asociada a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Documentación asociada a una contragarantía </returns>
        public List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation(int individualId, int guaranteeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties.IndividualId, typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties.GuaranteeId, typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation).Name);
            filter.Equal();
            filter.Constant(guaranteeId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetInsuredGuaranteeDocumentation");
            return ModelAssembler.CreateInsuredGuaranteeDocumentations(businessCollection);
        }

        /// <summary>
        /// Saves the guarantee documentation.
        /// </summary>
        /// <param name="listDocumentation">The list documentation.</param>
        /// <param name="guaranteeId">The guarantee identifier.</param>
        public void SaveGuaranteeDocumentation(List<Models.InsuredGuaranteeDocumentation> listDocumentation, int guaranteeId)
        {
            if (listDocumentation != null && listDocumentation.Count > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties.IndividualId, typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties).Name);
                filter.Equal();
                filter.Constant(listDocumentation.FirstOrDefault().IndividualId);
                filter.And();
                filter.Property(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties.GuaranteeId, typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties).Name);
                filter.Equal();
                filter.Constant(guaranteeId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation), filter.GetPredicate()));
                FacadeDAO.deleteCollection(businessCollection);

                foreach (Models.InsuredGuaranteeDocumentation documentation in listDocumentation)
                {
                    documentation.GuaranteeId = guaranteeId;
                    UniquePersonV1.Entities.InsuredGuaranteeDocumentation insuredGuaranteeDocumentationEntity = EntityAssembler.CreateInsuredGuaranteeDocumentation(documentation);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(insuredGuaranteeDocumentationEntity);
                }
            }
        }


        


        /// <summary>
        /// Obtener la documentacion recibida
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <returns></returns>
        public List<Models.GuaranteeRequiredDocument> GetDocumentationReceivedByGuaranteeId(int guaranteeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Models.GuaranteeRequiredDocument> guaranteeRequiredDocuments = new List<Models.GuaranteeRequiredDocument>();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(GuaranteeRequiredDocument.Properties.Description, "grd"), "Description"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeRequiredDocument.Properties.DocumentCode, "grd"), "DocumentCode"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeRequiredDocument.Properties.GuaranteeCode, "grd"), "GuaranteeCode"));

            Join join = new Join(new ClassNameTable(typeof(Guarantee), "g"), new ClassNameTable(typeof(GuaranteeRequiredDocument), "grd"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Guarantee.Properties.GuaranteeCode, "g")
                .Equal()
                .Property(GuaranteeRequiredDocument.Properties.GuaranteeCode, "grd")
                .GetPredicate());

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Guarantee.Properties.GuaranteeCode, "g");
            filter.Equal();
            filter.Constant(guaranteeId);

            select.Table = join;
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.GuaranteeRequiredDocument guaranteeRequiredDocument = null;
                while (reader.Read())
                {
                    guaranteeRequiredDocument = new Models.GuaranteeRequiredDocument
                    {
                        DocumentCode = (int)reader["DocumentCode"],
                        Description = (string)reader["Description"],
                        GuaranteeCode = (int)reader["GuaranteeCode"]
                    };
                    guaranteeRequiredDocuments.Add(guaranteeRequiredDocument);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetDocumentationReceivedByGuaranteeId");
            return guaranteeRequiredDocuments;
        }
    }
}

