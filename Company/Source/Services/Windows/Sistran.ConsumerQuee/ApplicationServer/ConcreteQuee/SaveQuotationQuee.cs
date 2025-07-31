//using ApplicationServer.AbstractQuee;
//using ApplicationServer.WrapperServiceWCF;
//using Utiles.Extentions;
//using System;

//namespace ApplicationServer.ConcreteQuee
//{
//    public class SaveQuotationQuee : TemplateQuee
//    {
//        protected override void ActionQueeToExcecute(Object body)
//        {

//            string businessCollection = (string)body;
//            if (businessCollection != null)
//            {
//                using (WrapperServiceClient client = new WrapperServiceClient("BasicHttpBinding_IWrapperService"))
//                {
//                    client.CreateQuotationListFromJson(businessCollection);
//                }
//            }
//        }
//    }
//}