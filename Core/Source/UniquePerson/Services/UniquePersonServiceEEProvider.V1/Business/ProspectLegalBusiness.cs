using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ProspectLegalBusiness
    {

        internal List<ProspectNatural> GetProspectLegalByDocument(string documentNumber)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Prospect.Properties.TributaryIdNo, typeof(UniquePersonV1.Entities.Prospect).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            var prospectCollection = DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Prospect), filter.GetPredicate());
            return ModelAssembler.CreateNaturalProspects(prospectCollection);
        }

        internal ProspectNatural UpdateProspectLegal(ProspectNatural prospectLegal)
        {
            UniquePersonV1.Entities.Prospect ProspectEntity = EntityAssembler.CreateProspectLegal(prospectLegal);
            ProspectEntity.BirthDate = null;
            DataFacadeManager.Update(ProspectEntity);
            return ModelAssembler.CreateModelProspect(ProspectEntity);
        }

        internal ProspectNatural CreateProspectLegal(ProspectNatural prospectLegal)
        {
            UniquePersonV1.Entities.Prospect ProspectEntity = EntityAssembler.CreateProspectLegal(prospectLegal);

            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);
            funtion.AddParameter(new Column(UniquePersonV1.Entities.Prospect.Properties.ProspectId));
            selectQuery.Table = new ClassNameTable(typeof(UniquePersonV1.Entities.Prospect), "ProspectId");
            selectQuery.AddSelectValue(new SelectValue(funtion));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    ProspectEntity.ProspectId = (Convert.ToInt32(reader[0]) + 1);
                }
            }
            ProspectEntity.BirthDate = null;
            DataFacadeManager.Insert(ProspectEntity);
            CreateCoProspect(ProspectEntity.ProspectId, CalculateDigitVerify(ProspectEntity.TributaryIdNo.Trim()));
            return ModelAssembler.CreateModelProspect(ProspectEntity);
        }

        internal void CreateCoProspect(int ProspectId, int VerifyDigit = -1)
        {
            CoProspect CoprospectNaturalentity = EntityAssembler.CreateCoProspect(ProspectId, VerifyDigit);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(CoprospectNaturalentity);
        }

        internal int CalculateDigitVerify(string documentNumber)
        {
            int[] vpri = new int[16];
            int x, y, z, i, dv1;
            string nit1 = documentNumber;

            try
            {
                z = documentNumber.Length;

                if (nit1.Length > 0)
                {

                    x = 0; y = 0; z = documentNumber.Length;
                    vpri[1] = 3;
                    vpri[2] = 7;
                    vpri[3] = 13;
                    vpri[4] = 17;
                    vpri[5] = 19;
                    vpri[6] = 23;
                    vpri[7] = 29;
                    vpri[8] = 37;
                    vpri[9] = 41;
                    vpri[10] = 43;
                    vpri[11] = 47;
                    vpri[12] = 53;
                    vpri[13] = 59;
                    vpri[14] = 67;
                    vpri[15] = 71;
                    for (i = 0; i < z; i++)
                    {
                        y = (int.Parse(nit1.Substring(i, 1)));
                        x += (y * vpri[z - i]);
                    }

                    y = x % 11;

                    if (y > 1)
                    {
                        dv1 = 11 - y;
                    }
                    else
                    {
                        dv1 = y;
                    }
                    return (int)dv1;
                }
                return -1;
            }
            catch (System.Exception)
            {
                return -1;
            }
        }

    }
}
