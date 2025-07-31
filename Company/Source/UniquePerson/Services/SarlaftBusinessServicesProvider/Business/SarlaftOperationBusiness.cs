using Sistran.Company.Application.SarlaftBusinessServices.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF;
using System.Data;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class SarlaftOperationsBusiness
    {
        public List<CompanySarlaftOperation> GetInternationalOperationsBySarlaftId(int sarlafId)
        {
            List<CompanySarlaftOperation> companySarlaftOperations = new List<CompanySarlaftOperation>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UPCEN.SarlaftOperation.Properties.SarlaftId, typeof(UPCEN.SarlaftOperation).Name);
            filter.Equal();
            filter.Constant(sarlafId);

            SarlaftOperationView sarlaftOperationView = new SarlaftOperationView();
            ViewBuilder viewBuilder = new ViewBuilder("SarlaftOperationView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, sarlaftOperationView);

            if(sarlaftOperationView.SarlaftOperations.Count > 0)
            {
                companySarlaftOperations = ModelAssembler.CreateCompanySarlaftOperations(sarlaftOperationView.SarlaftOperations);

                foreach (CompanySarlaftOperation companySarlaftOperation in companySarlaftOperations)
                {
                    companySarlaftOperation.CompanyOperationType.Description = sarlaftOperationView.OperationTypes.Cast<COMMEN.OperationType>().First(x => x.OperationTypeCode == companySarlaftOperation.CompanyOperationType.Id).SmallDescription;
                    companySarlaftOperation.CompanyProductType.Description = sarlaftOperationView.ProductTypes.Cast<UPCEN.ProductType>().First(x => x.ProductTypeCode == companySarlaftOperation.CompanyProductType.Id).Description;
                }

                return companySarlaftOperations;
            }
            else
            {
                return companySarlaftOperations;
            }
        }

        public CompanySarlaftOperation CreateInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            UPCEN.SarlaftOperation entitySarlaftOperation = EntityAssembler.CreateInternationalOperation(companySarlaftOperation);

            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);

            funtion.AddParameter(new Column(UPCEN.SarlaftOperation.Properties.SarlaftOperationId));

            selectQuery.Table = new ClassNameTable(typeof(UPCEN.SarlaftOperation));
            selectQuery.AddSelectValue(new SelectValue(funtion));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    entitySarlaftOperation.SarlaftOperationId = (Convert.ToInt32(reader[0]) + 1);
                }
            }
            
            DataFacadeManager.Insert(entitySarlaftOperation);
            return ModelAssembler.CreateCompanySarlaftOperation(entitySarlaftOperation);
        }

        public CompanySarlaftOperation UpdateInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            UPCEN.SarlaftOperation entitySarlaftOperation = EntityAssembler.CreateInternationalOperation(companySarlaftOperation);
            DataFacadeManager.Update(entitySarlaftOperation);
            return ModelAssembler.CreateCompanySarlaftOperation(entitySarlaftOperation);
        }

        public void DeleteInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            PrimaryKey internationalOperationPrimaryKey = UPCEN.SarlaftOperation.CreatePrimaryKey(companySarlaftOperation.Id, companySarlaftOperation.SarlaftId);
            DataFacadeManager.Delete(internationalOperationPrimaryKey);
        }
        

    }
}
