using System;
using System.Collections.Generic;
using System.Data;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Common.EndorsementWorkFlow.Entities;
using Sistran.Company.Application.ReversionEndorsement.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.ReversionEndorsement.EEProvider.DAOs
{
    public class ReversionDAO
    {
        public List<string> GetEndorsementWorkFlow(int PolyciId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@idRadicado", PolyciId);

            List<string> answerRadicado = new List<string>();
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                DataTable datas = dynamicDataAccess.ExecuteSPDataTable("ISS.GET_ENDORSEMENT_WORK_FLOW", parameters);


                foreach (DataRow objeData in datas.Rows)
                {
                    answerRadicado.Add(objeData.ItemArray[0].ToString());
                    answerRadicado.Add(objeData.ItemArray[2].ToString());                 
                }
            }
            return answerRadicado;
        }

        public bool CreateEndorsementWorkFlow(int? PolyciId, int? EndorsementId, string filingNumber, DateTime fiingDate)
        {
            try
            {
                EndorsementWorkFlow endorsement = EntityAssembler.CreateEndorsementWorkFlow(PolyciId, EndorsementId, filingNumber, fiingDate);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(endorsement);
                DataFacadeManager.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
