using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class InsuredObjectPrefixView : BusinessView
    {
         public BusinessCollection Prefix
         {
             get
             {
                 return this["Prefix"];
             }
         }
         public BusinessCollection PrefixLineBusiness
         {
             get
             {
                 return this["PrefixLineBusiness"];
             }
         }
         public BusinessCollection InsObjLinesBusiness
         {
             get
             {
                 return this["InsObjLineBusiness"];
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
