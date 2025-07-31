using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// contragarantes
    /// </summary>
    public class GuarantorDAO
    {
        /// <summary>
        /// Obtiene los contragarantes de una contragarantía
        /// </summary>
        /// <returns> Listado de contragarantías </returns>
        public List<Models.Guarantor> GetGuarantorsByGuaranteeId(int id)
        {

            SelectQuery select = new SelectQuery();
            select.Table = new ClassNameTable(typeof(UniquePersonV1.Entities.Guarantor), "g");

            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.Adrress, "g"), "Adrress"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.CityText, "g"), "CityText"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.GuaranteeId, "g"), "GuaranteeId"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.GuarantorId, "g"), "GuarantorId"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.GuarantorName, "g"), "GuarantorName"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.IdCardNo, "g"), "IdCardNo"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.IndividualId, "g"), "IndividualId"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.PhoneNumber, "g"), "PhoneNumber"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.TradeName, "g"), "TradeName"));
            select.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Guarantor.Properties.TributaryIdNo, "g"), "TributaryIdNo"));


            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Guarantor.Properties.GuaranteeId, "g");
            filter.Equal();
            filter.Constant(id);

            select.Where = filter.GetPredicate();

            List<Models.Guarantor> guarantors = new List<Models.Guarantor>();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.Guarantor guarantor = null;

                while (reader.Read())
                {
                    guarantor = new Models.Guarantor();
                    guarantor.Adrress = (string)reader["Adrress"];
                    guarantor.CardNro = ((string)reader["IdCardNo"]) != null ? (string)reader["IdCardNo"] : " ";
                    guarantor.CityText = (string)reader["CityText"];
                    guarantor.GuaranteeId = (int)reader["GuaranteeId"];
                    guarantor.GuarantorId = (int)reader["GuarantorId"];
                    guarantor.IndividualId = (int)reader["IndividualId"];
                    guarantor.Name = ((string)reader["GuarantorName"]) != null ? (string)reader["GuarantorName"] : " ";
                    guarantor.PhoneNumber = Convert.ToInt64(reader["PhoneNumber"]);
                    guarantor.TradeName = ((string)reader["TradeName"]) != null ? (string)reader["TradeName"] : " ";
                    guarantor.TributaryIdNo = ((string)reader["TributaryIdNo"]) != null ? (string)reader["TributaryIdNo"] : " ";
                    guarantors.Add(guarantor);
                }
            }

            return guarantors;
        }

        /// <summary>
        /// Saves the guarantors.
        /// </summary>
        /// <param name="guarantors">The guarantors.</param>
        /// <param name="guaranteeId">The guarantee identifier.</param>
        public void SaveGuarantors(List<Models.Guarantor> guarantors, int guaranteeId)
        {
            if (guarantors != null && guarantors.Count > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UniquePersonV1.Entities.Guarantor.Properties.IndividualId, typeof(UniquePersonV1.Entities.InsuredGuarantee.Properties).Name);
                filter.Equal();
                filter.Constant(guarantors.FirstOrDefault().IndividualId);
                filter.And();
                filter.Property(UniquePersonV1.Entities.Guarantor.Properties.GuaranteeId, typeof(UniquePersonV1.Entities.InsuredGuarantee.Properties).Name);
                filter.Equal();
                filter.Constant(guaranteeId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Guarantor), filter.GetPredicate()));

                FacadeDAO.deleteCollection(businessCollection);
                int guarantor_id = 0;
                foreach (Models.Guarantor guarantor in guarantors)
                {
                    guarantor_id += 1;
                    guarantor.GuaranteeId = guaranteeId;
                    guarantor.GuarantorId = guarantor_id;
                    guarantor.TributaryIdNo = guarantor.TributaryIdNo != null ? guarantor.TributaryIdNo : " ";
                    guarantor.Name = guarantor.Name != null ? guarantor.Name : " ";
                    guarantor.TradeName = guarantor.TradeName != null ? guarantor.TradeName : " ";
                    guarantor.CardNro = guarantor.CardNro != null ? guarantor.CardNro : " ";


                    UniquePersonV1.Entities.Guarantor guarantorEntity = EntityAssembler.CreateGuarantor(guarantor);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(guarantorEntity);
                }
            }
        }
    }
}
