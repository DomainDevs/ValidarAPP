using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class InsuredObjectView : BusinessView
    {
         public BusinessCollection GroupInsuredObjects
         {
             get
             {
                 return this["GroupInsuredObject"];
             }
         }

         public BusinessCollection InsuredObjects
         {
             get
             {
                 return this["InsuredObject"];
             }
         }
    }
}
