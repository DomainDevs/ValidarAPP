using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class PaymentAccountTypeBusiness
    {

        internal PaymentAccountType CreatePaymentAccountType(PaymentAccountType PaymentAccountType)
        {
            UniquePersonV1.Entities.PaymentAccountType entityPaymentAccountType = EntityAssembler.CreatePaymentAccountType(PaymentAccountType);
            DataFacadeManager.Insert(entityPaymentAccountType);
            return ModelAssembler.CreateAccountType(entityPaymentAccountType);
        }

        internal PaymentAccountType UpdatePaymentAccountType(PaymentAccountType PaymentAccountType)
        {
            UniquePersonV1.Entities.PaymentAccountType entityPaymentAccountType = EntityAssembler.CreatePaymentAccountType(PaymentAccountType);
            DataFacadeManager.Update(entityPaymentAccountType);
            return ModelAssembler.CreateAccountType(entityPaymentAccountType);
        }

        internal void DeletePaymentAccountType(int PaymentAccountTypeId)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.PaymentAccountType.CreatePrimaryKey(PaymentAccountTypeId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal PaymentAccountType GetPaymentAccountTypeByPaymentAccountTypeId(int PaymentAccountTypeId)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.PaymentAccountType.CreatePrimaryKey(PaymentAccountTypeId);
            UniquePersonV1.Entities.PaymentAccountType entityPaymentAccountType = (UniquePersonV1.Entities.PaymentAccountType)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateAccountType(entityPaymentAccountType);
        }

        internal List<PaymentAccountType> GetPaymentAccountTypes()
        {
            return ModelAssembler.CreateAccountTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.PaymentAccountType)));
        }

    }
}
