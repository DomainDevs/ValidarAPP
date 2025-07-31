using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesPerson = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.Queries;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using System.Data;
using System;
using System.Diagnostics;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class BeneficiaryDAO
    {
        /// <summary>
        /// Obtener lista de beneficiarios
        /// </summary>
        /// <param name="description">Id o nombre o razón social</param>
        /// <returns>Beneficiarios</returns>
        public List<Model.Beneficiary> GetBeneficiariesByDescription(string description, InsuredSearchType insuredSearchType, CustomerType? customerType = CustomerType.Individual)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            ObjectCriteriaBuilder filterCompany = new ObjectCriteriaBuilder();

            Int64 individualId = 0;
            Int64.TryParse(description, out individualId);

            if (individualId > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(EntitiesPerson.Person.Properties.IdCardNo, "p");
                        filter.Equal();
                        filter.Constant(description);
                        filterCompany.Property(EntitiesPerson.Company.Properties.TributaryIdNo, "c");
                        filterCompany.Equal();
                        filterCompany.Constant(description);
                        break;
                    case InsuredSearchType.IndividualId:
                        filter.Property(EntitiesPerson.Person.Properties.IndividualId, "p");
                        filter.Equal();
                        filter.Constant(individualId);
                        filterCompany.Property(EntitiesPerson.Company.Properties.IndividualId, "c");
                        filterCompany.Equal();
                        filterCompany.Constant(individualId);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string[] fullName = description.Split(' ');
                fullName = fullName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                filter.Property(EntitiesPerson.Person.Properties.Surname, "p");
                filter.Like();
                filter.Constant(fullName[0] + "%");

                if (fullName.Length > 1)
                {
                    filter.And();
                    filter.Property(EntitiesPerson.Person.Properties.MotherLastName, "p");
                    filter.Like();
                    filter.Constant(fullName[1] + "%");
                    if (fullName.Length > 2)
                    {
                        StringBuilder name = new StringBuilder();
                        for (int i = 2; i < fullName.Length; i++)
                        {
                            name.Append(fullName[i]);
                            name.Append(" ");
                        }
                        filter.And();
                        filter.Property(EntitiesPerson.Person.Properties.Name, "p");
                        filter.Like();
                        filter.Constant(name.ToString().Trim() + "%");
                    }
                }

                StringBuilder tradeName = new StringBuilder();
                for (int i = 0; i < fullName.Length; i++)
                {
                    tradeName.Append(fullName[i]);
                    tradeName.Append(" ");
                }

                filterCompany.Property(EntitiesPerson.Company.Properties.TradeName, "c");
                filterCompany.Like();
                filterCompany.Constant(tradeName.ToString().Trim() + "%");
            }

            #region Select Person
            SelectQuery select = new SelectQuery();
            select.MaxRows = 200;
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.IndividualId, "p")));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.Surname, "p"), "Surname"));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.MotherLastName, "p"), "MotherLastName"));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.Name, "p"), "name"));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.Gender, "p"), "tradename"));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.IdCardNo, "p")));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Person.Properties.IdCardTypeCode, "p")));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Individual.Properties.IndividualTypeCode, "ind")));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Insured.Properties.InsuredCode, "i")));
            select.AddSelectValue(new SelectValue(new Column(EntitiesPerson.DocumentType.Properties.Description, "d")));
            #endregion

            #region JoinPerson
            Join join = new Join(new ClassNameTable(typeof(EntitiesPerson.Person), "p"), new ClassNameTable(typeof(EntitiesPerson.Insured), "i"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(EntitiesPerson.Person.Properties.IndividualId, "p")
                .Equal()
                .Property(EntitiesPerson.Insured.Properties.IndividualId, "i").GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(EntitiesPerson.DocumentType), "d"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(EntitiesPerson.Person.Properties.IdCardTypeCode, "p")
                .Equal().Property(EntitiesPerson.DocumentType.Properties.IdDocumentType, "d")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(EntitiesPerson.Individual), "ind"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(EntitiesPerson.Person.Properties.IndividualId, "p")
                .Equal().Property(EntitiesPerson.Individual.Properties.IndividualId, "ind")
                .GetPredicate());
            #endregion

            select.Table = join;
            select.Where = filter.GetPredicate();


            #region Select Company
            SelectQuery select2 = new SelectQuery();
            select2.MaxRows = 200;
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.IndividualId, "c")));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.ManagerName, "c"), "Surname"));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.GeneralManagerName, "c"), "MotherLastName"));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.ContactName, "c"), "name"));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.TradeName, "c"), "tradename"));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.TributaryIdNo, "c")));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Company.Properties.TributaryIdTypeCode, "c")));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Individual.Properties.IndividualTypeCode, "ind")));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.Insured.Properties.InsuredCode, "i")));
            select2.AddSelectValue(new SelectValue(new Column(EntitiesPerson.DocumentType.Properties.Description, "d")));
            #endregion

            #region JoinCompany
            Join join2 = new Join(new ClassNameTable(typeof(EntitiesPerson.Company), "c"), new ClassNameTable(typeof(EntitiesPerson.Insured), "i"), JoinType.Left);
            join2.Criteria = (new ObjectCriteriaBuilder()
                .Property(EntitiesPerson.Company.Properties.IndividualId, "c")
                .Equal()
                .Property(EntitiesPerson.Insured.Properties.IndividualId, "i").GetPredicate());

            join2 = new Join(join2, new ClassNameTable(typeof(EntitiesPerson.DocumentType), "d"), JoinType.Left);
            join2.Criteria = (new ObjectCriteriaBuilder()
                .Property(EntitiesPerson.Company.Properties.TributaryIdTypeCode, "c")
                .Equal().Property(EntitiesPerson.DocumentType.Properties.IdDocumentType, "d")
                .GetPredicate());
            join2 = new Join(join2, new ClassNameTable(typeof(EntitiesPerson.Individual), "ind"), JoinType.Left);
            join2.Criteria = (new ObjectCriteriaBuilder()
                .Property(EntitiesPerson.Company.Properties.IndividualId, "c")
                .Equal().Property(EntitiesPerson.Individual.Properties.IndividualId, "ind")
                .GetPredicate());
            #endregion

            select2.Table = join2;
            select2.Where = filterCompany.GetPredicate();

            //UnionQuery union = new UnionQuery(select, select2);

            List<Model.Beneficiary> beneficiaries = new List<Model.Beneficiary>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    Model.Beneficiary beneficiary = new Model.Beneficiary();
                    string MotherLastName = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString() + " ";
                    beneficiary.IndividualId = (int)reader[EntitiesPerson.Person.Properties.IndividualId];
                    beneficiary.CustomerType = CustomerType.Individual;
                    beneficiary.Name = (string)reader["Surname"] + " " + MotherLastName + (string)reader["name"];
                    beneficiary.BeneficiaryType = new BeneficiaryType { Id = Utilities.Configuration.KeySettings.OnerousBeneficiaryTypeId };
                    beneficiary.IdentificationDocument = new IssuanceIdentificationDocument { Number = reader["IdCardNo"].ToString(), DocumentType = new IssuanceDocumentType { Id = Convert.ToInt32(reader["IdCardTypeCode"].ToString()), Description = reader["Description"].ToString() } };
                    beneficiary.CodeBeneficiary = (int) (reader["InsuredCode"]  ?? 0);
                    beneficiary.IndividualType = (int)reader[EntitiesPerson.Person.Properties.IndividualTypeCode] == (int)IndividualType.Person ? IndividualType.Person : IndividualType.Company;
                    beneficiaries.Add(beneficiary);
                }
            }

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select2))
            {

                while (reader.Read())
                {
                    Model.Beneficiary beneficiary = new Model.Beneficiary();
                    beneficiary.IndividualId = (int)reader[EntitiesPerson.Person.Properties.IndividualId];
                    beneficiary.CustomerType = CustomerType.Individual;
                    beneficiary.Name = reader["tradename"].ToString();
                    beneficiary.BeneficiaryType = new BeneficiaryType { Id = Utilities.Configuration.KeySettings.LeasingBeneficiaryTypeId };
                    beneficiary.IdentificationDocument = new IssuanceIdentificationDocument { Number = reader["TributaryIdNo"].ToString(), DocumentType = new IssuanceDocumentType { Id = Convert.ToInt32(reader["TributaryIdTypeCode"].ToString()), Description = reader["Description"].ToString() } };
                    beneficiary.IndividualType = (int)reader[EntitiesPerson.Person.Properties.IndividualTypeCode] == (int)IndividualType.Person ? IndividualType.Person : IndividualType.Company;
                    beneficiary.CodeBeneficiary = (int) (reader["InsuredCode"] ?? 0);
                    beneficiaries.Add(beneficiary);
                }
            }

            if (beneficiaries.Count == 1)
            {
                beneficiaries[0].CustomerType = CustomerType.Individual;
            }

            #region  prospect
            if (customerType == CustomerType.Prospect)
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                List<IssuanceInsured> beneficiariesprospect;
                beneficiariesprospect = insuredDAO.GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, 5);

                if(beneficiariesprospect?.Count>0)
                {
                    Model.Beneficiary beneficiary = new Model.Beneficiary();
                    
                    beneficiary.IndividualId = beneficiariesprospect[0].IndividualId;
                    beneficiary.CustomerType = CustomerType.Prospect;
                    beneficiary.Name =  beneficiariesprospect[0].Name?.ToString(); //(string)reader["Surname"] + " " + MotherLastName + (string)reader["name"];
                    //beneficiary.BeneficiaryType = new BeneficiaryType { Id = Utilities.Configuration.KeySettings.OnerousBeneficiaryTypeId };
                    beneficiary.IdentificationDocument = new IssuanceIdentificationDocument { Number = beneficiariesprospect[0].IdentificationDocument?.Number?.ToString(), DocumentType = new IssuanceDocumentType { Id = Convert.ToInt32(beneficiariesprospect[0].IdentificationDocument?.DocumentType?.Id.ToString()), Description = beneficiariesprospect[0].IdentificationDocument?.DocumentType?.SmallDescription?.ToString() } };
                    //beneficiary.CodeBeneficiary = (int)reader["InsuredCode"];
                    beneficiaries.Clear();
                    beneficiaries.Add(beneficiary);
                }
            }
            #endregion

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetBeneficiariesByDescription");
            return beneficiaries;
        }
    }
}