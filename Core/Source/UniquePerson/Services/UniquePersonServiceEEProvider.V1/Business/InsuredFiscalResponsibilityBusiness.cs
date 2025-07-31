using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using EntityUP = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using System.Data;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class InsuredFiscalResponsibilityBusiness
    {
        public List<Models.InsuredFiscalResponsibility> CreateInsuredFiscalResponsibility(List<Models.InsuredFiscalResponsibility> listInsuredFiscalResponsibilities)
        {
            List<Models.InsuredFiscalResponsibility> insuredFiscalResponsibility = new List<Models.InsuredFiscalResponsibility>();
            List<EntityUP.InsuredFiscalResponsibility> entityCompanyFiscalResponsibility = EntityAssembler.CreateListInsuredFiscalResponsibility(listInsuredFiscalResponsibilities);
            foreach (EntityUP.InsuredFiscalResponsibility fiscal in entityCompanyFiscalResponsibility)
            {
                DataFacadeManager.Insert(fiscal);
            }
            return ModelAssembler.CreateListFiscalResponsibilities(entityCompanyFiscalResponsibility);

        }

        public Models.InsuredFiscalResponsibility UpdateInsuredFiscalResponsibility(Models.InsuredFiscalResponsibility companyFiscal)
        {

            PrimaryKey primaryKey = EntityUP.InsuredFiscalResponsibility.CreatePrimaryKey(companyFiscal.Id, companyFiscal.IndividualId);
            EntityUP.InsuredFiscalResponsibility entityCompanyFiscal = (EntityUP.InsuredFiscalResponsibility)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (entityCompanyFiscal != null)
            {
                entityCompanyFiscal.IndividualId = companyFiscal.IndividualId;
                entityCompanyFiscal.InsuredCode = companyFiscal.InsuredId;
                entityCompanyFiscal.FiscalResponsibilityId = companyFiscal.FiscalResponsabilityId;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCompanyFiscal);
            }
            return ModelAssembler.CreateInsuredFiscalResponsibility(entityCompanyFiscal);
        }

        /// <summary>
        /// Consultar razones sociales por individualID
        /// </summary>
        public List<InsuredFiscalResponsibility> GetFiscalResponsibilityByIndividualId(int individualId)
        {

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(EntityUP.InsuredFiscalResponsibility.Properties.IndividualId, "ir")));
            select.AddSelectValue(new SelectValue(new Column(EntityUP.InsuredFiscalResponsibility.Properties.InsuredCode, "ir")));
            select.AddSelectValue(new SelectValue(new Column(EntityUP.InsuredFiscalResponsibility.Properties.FiscalResponsibilityId, "ir")));
            select.AddSelectValue(new SelectValue(new Column(EntityUP.InsuredFiscalResponsibility.Properties.Id, "ir")));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.FiscalResponsibility.Properties.Code, "fr")));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.FiscalResponsibility.Properties.Description, "fr")));

            Join join = new Join(new ClassNameTable(typeof(EntityUP.InsuredFiscalResponsibility), "ir"), new ClassNameTable(typeof(UniquePersonV1.Entities.FiscalResponsibility), "fr"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EntityUP.InsuredFiscalResponsibility.Properties.FiscalResponsibilityId, "ir").Equal()
                    .Property(UniquePersonV1.Entities.FiscalResponsibility.Properties.Id, "fr").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EntityUP.InsuredFiscalResponsibility.Properties.IndividualId, "ir").Equal().Constant(individualId);

            select.Table = join;
            select.Where = where.GetPredicate();

            List<InsuredFiscalResponsibility> result = new List<InsuredFiscalResponsibility>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    result.Add(new InsuredFiscalResponsibility
                    {
                        IndividualId = (int)reader["IndividualId"],
                        InsuredId = (int)reader["InsuredCode"],
                        FiscalResponsabilityId = (int)reader["FiscalResponsibilityId"],
                        FiscalResponsabilityDescription = (string)reader["Description"],
                        Code = (string)reader["Code"],
                        Id = (int)reader["Id"]

                    });


                }
            }
            return result;

           
        }

        public bool DeleteFiscalResponsibility(Models.InsuredFiscalResponsibility coFiscal)
        {
            PrimaryKey key = EntityUP.InsuredFiscalResponsibility.CreatePrimaryKey(coFiscal.Id, coFiscal.IndividualId);
            var fiscal = (EntityUP.InsuredFiscalResponsibility)DataFacadeManager.GetObject(key);
            if (fiscal != null)
            {
                DataFacadeManager.Delete(key);
                return true;
            }
            return false;
        }
    }
}
