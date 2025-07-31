using ENTAUT =Sistran.Core.Application.AuthorizationPolicies.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class PersonOperationDAO
    {
        public object AuthorizationRequest { get; private set; }

        /// <summary>
        /// crear registro de política ejecutada
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.PersonOperation CreatePersonOperation(Models.PersonOperation personOperation)
        {
            TmpPersonOperation entityPerson = EntityAssembler.CreatePersonOperation(personOperation);
            DataFacadeManager.Insert(entityPerson);
            personOperation.OperationId = entityPerson.OperationId;
            return personOperation;

        }

        /// <summary>
        /// Consultar registro de política ejecutada
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.PersonOperation GetPersonOperation(int OperationId)
        {
            PrimaryKey primaryKey = TmpPersonOperation.CreatePrimaryKey(OperationId);
            var personCollection = (TmpPersonOperation)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            return ModelAssembler.CreatePersonOperation(personCollection);
        }

        /// <summary>
        /// Consultar politicas generadas
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public  List<Models.PersonOperation> GetOperationTmp(int IndividualId)
        {
            List<Models.PersonOperation> listPersonOperation = new List<Models.PersonOperation>();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(TmpPersonOperation.Properties.OperationId,"tmo")));
            select.AddSelectValue( new SelectValue(new Column(TmpPersonOperation.Properties.FunctionId, "tmo")));
            select.AddSelectValue(new SelectValue(new Column(TmpPersonOperation.Properties.IndividualId, "tmo")));

            select.AddSelectValue(new SelectValue(new Column(ENTAUT.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(ENTAUT.AuthorizationAnswer.Properties.Enabled, "aa")));
            
            select.AddSelectValue(new SelectValue(new Column(ENTAUT.AuthorizationRequest.Properties.StatusId, "ar")));

            Join join = new Join(new ClassNameTable(typeof(TmpPersonOperation), "tmo"),
                new ClassNameTable(typeof(ENTAUT.AuthorizationRequest), "ar"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(ENTAUT.AuthorizationRequest.Properties.FunctionId, "ar").Equal()
                    .Property(TmpPersonOperation.Properties.FunctionId, "tmo")

                    .And().Property(TmpPersonOperation.Properties.OperationId, "tmo").Equal()
                    .Property(ENTAUT.AuthorizationRequest.Properties.Key, "ar").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(ENTAUT.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                   .Property(ENTAUT.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal()
                   .Property(ENTAUT.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()
                   
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(TmpPersonOperation.Properties.IndividualId, "tmo").Equal().Constant(IndividualId);
            where.And().Property(ENTAUT.AuthorizationAnswer.Properties.Enabled, "aa").Equal().Constant(1);
            where.And().Property(ENTAUT.AuthorizationRequest.Properties.StatusId, "ar").In();
            where.ListValue();
            where.Constant(1);
            where.Constant(3);
            where.EndList();

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    listPersonOperation.Add(new Models.PersonOperation
                    {
                        IndividualId = (int)reader["IndividualId"],
                        FunctionId = (int)reader["FunctionId"],
                        StatusId = (int)reader["StatusId"],
                        RequestId = (int)reader["AuthorizationRequestId"]

                    });
                }
            }

            return listPersonOperation;
            
        }

    }
}
