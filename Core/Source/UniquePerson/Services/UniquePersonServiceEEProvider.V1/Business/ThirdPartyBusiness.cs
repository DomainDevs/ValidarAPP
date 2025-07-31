
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ThirdPartyBusiness
    {
                
        /// <summary>
        /// Consultar tercero por individualID
        /// </summary>
        public List<ThirdPerson> GetThirdByDescriptionInsuredSearchType(string description, InsuredSearchType insuredSearchType)
        {
            List<ThirdPerson> thirds = new List<ThirdPerson>();
            int maxRows = 50;

            try
            {
                thirds.AddRange(GetThirdPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
                thirds.AddRange(GetThirdCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

            return thirds;

        }


        public List<ThirdPerson> GetThirdPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<ThirdPerson> thirds = new List<ThirdPerson>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Person.Properties.IndividualId, "p");
                        filter.Like();
                        filter.Constant(identificationNumber.ToString() + "%");
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                        filter.Like();
                        filter.Constant(description + "%");
                        break;
                }
            }
            else
            {
                string[] fullName = description.Trim().Split(' ');
                fullName = fullName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                filter.Property(UniquePerson.Entities.Person.Properties.Surname, "p");
                filter.Like();
                filter.Constant("%" + fullName[0] + "%");

                if (fullName.Length > 1)
                {
                    filter.And();
                    filter.OpenParenthesis();
                    filter.Property(UniquePerson.Entities.Person.Properties.MotherLastName, "p");
                    filter.Like();
                    filter.Constant("%" + fullName[1] + "%");

                    filter.Or();
                    filter.Property(UniquePerson.Entities.Person.Properties.Name, "p");
                    filter.Like();
                    filter.Constant("%" + fullName[1] + "%");
                    filter.CloseParenthesis();

                    if (fullName.Length > 2)
                    {
                        string name = "";
                        int cont = 0;
                        string space = "";
                        for (int i = 2; i < fullName.Length; i++)
                        {
                            if (cont > 0)
                            {
                                space = " ";
                            }
                            name += space + fullName[i];
                            cont++;
                        }

                        filter.And();
                        filter.Property(UniquePerson.Entities.Person.Properties.Name, "p");
                        filter.Like();
                        filter.Constant("%" + name + "%");
                    }
                }

                filter.Or();
                filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                filter.Like();
                filter.Constant(fullName[0].ToString() + "%");
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IndividualId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Surname, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.MotherLastName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Name, "p")));            
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardTypeCode, "p")));                        
            selectQuery.AddSelectValue(new SelectValue(new Column(ThirdParty.Properties.IndividualId, "tp")));
            selectQuery.AddSelectValue(new SelectValue(new Column(ThirdParty.Properties.ThirdPartyCode, "tp")));

            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Person), "p"), new ClassNameTable(typeof(ThirdParty), "tp"), JoinType.Inner);

            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Person.Properties.IndividualId, "p")
                .Equal()
                .Property(ThirdParty.Properties.IndividualId, "tp").GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    ThirdPerson thirdPerson = new ThirdPerson
                    {
                        Id = Convert.ToInt32(reader["ThirdPartyCode"]),
                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
                        Fullname = String.Format("{0} {1} {2}", reader["Surname"].ToString(), reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(), reader["Name"].ToString()),
                        DocumentTypeId = Convert.ToInt32(reader["IdCardTypeCode"]),
                        DocumentNumber = reader["IdCardNo"].ToString()
                    };                    

                    thirds.Add(thirdPerson);
                }

            }

            return thirds;
        }

        public List<ThirdPerson> GetThirdCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<ThirdPerson> thirds = new List<ThirdPerson>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Company.Properties.IndividualId, "c");
                        filter.Like();
                        filter.Constant(identificationNumber.ToString() + "%");
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                        filter.Like();
                        filter.Constant(description + "%");
                        break;
                }
            }
            else
            {
                filter.Property(UniquePerson.Entities.Company.Properties.TradeName, "c");
                filter.Like();
                filter.Constant(description.Trim() + "%");
                filter.Or();
                filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                filter.Like();
                filter.Constant(description.Trim() + "%");
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.CoCompany.Properties.AssociationTypeCode, "cc")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.IndividualId, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TradeName, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(ThirdParty.Properties.IndividualId, "tp")));
            selectQuery.AddSelectValue(new SelectValue(new Column(ThirdParty.Properties.ThirdPartyCode, "tp")));

            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Company), "c"), new ClassNameTable(typeof(ThirdParty), "tp"), JoinType.Inner);

            join.Criteria = (new ObjectCriteriaBuilder().Property(UniquePerson.Entities.Company.Properties.IndividualId, "c").Equal().Property(ThirdParty.Properties.IndividualId, "tp").GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.CoCompany), "cc"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Company.Properties.IndividualId, "c")
                .Equal().Property(UniquePerson.Entities.CoCompany.Properties.IndividualId, "cc")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    ThirdPerson thirdPerson = new ThirdPerson
                    {
                        Id = Convert.ToInt32(reader["ThirdPartyCode"]),
                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
                        Fullname = reader["TradeName"].ToString(),
                        DocumentTypeId = Convert.ToInt32(reader["TributaryIdTypeCode"]),
                        DocumentNumber = reader["TributaryIdNo"].ToString()
                    };
                    
                    thirds.Add(thirdPerson);
                }
            }            

            return thirds;
        }
    }
}
