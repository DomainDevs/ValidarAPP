using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers;
using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Services;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Field = Sistran.Core.Services.UtilitiesServices.Models.Field;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Framework.Queries;
using System.Data;
using ENUM = Sistran.Company.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.DAOs
{
    public class MinPremiunRelationDAO
    {
        public CompanyParamMinPremiunRelation Create(CompanyParamMinPremiunRelation param)
        {
            try
            {
                // param.Id = this.GetAll().OrderByDescending(x => x.Id).First().Id + 1;
                var entity = EntityAssembler.CreateEntity(param);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.InsertObject(entity);
                }

                if (entity.MinPremiumRelId != 0)
                {
                    return ModelAssembler.CreateCompanyParam(entity);
                }
                else
                {
                    throw new BusinessException("Error en MinPremiunRelationDAO.Create ");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyParamMinPremiunRelation Update(CompanyParamMinPremiunRelation param)
        {

            PrimaryKey primaryKey = MinPremiumRelation.CreatePrimaryKey(param.Id);
            var entity = (MinPremiumRelation)DataFacadeManager.GetObject(primaryKey);
            entity.BranchCode = param.Branch.Id;
            entity.CurrencyCode = param.Currency.Id;
            entity.EndoTypeCode = param.EndorsementType.Id;
            entity.Key1 = param.Product.Id;
            entity.Key2 = (param.MinPremiunRange != null ? param.MinPremiunRange.Id : param.GroupCoverage != null ? param.GroupCoverage.Id : null);
            entity.PrefixCode = param.Prefix.Id;
            entity.RiskMinPremium = param.RiskMinPremiun;
            entity.SubsMinPremium = param.SubMinPremiun;
            entity.CalculateProrate = false;
            entity.ValidityType = 0;
            DataFacadeManager.Update(entity);
            return param;
        }

        public List<CompanyParamMinPremiunRelation> GetAll()
        {
            try
            {
                var response = new List<CompanyParamMinPremiunRelation>();
                var view = new MinPremiunRelationView();
                var builder = new ViewBuilder("MinPremiunRelationView");
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
                
                foreach (var entity in view.MinPremiumRelation)
                {
                    var minPremiumRelation = (MinPremiumRelation)entity;
                    var branch = view.Branch.Count > 0 ? view.Branch.Cast<Branch>().FirstOrDefault(x => x.BranchCode == minPremiumRelation.BranchCode) : null;
                    var currency = view.Currency.Count > 0 ? view.Currency.Cast<Currency>().First(x => x.CurrencyCode == minPremiumRelation.CurrencyCode) : null;
                    var endoType = view.EndorsementType.Count > 0 ? view.EndorsementType.Cast<EndorsementType>().First(x => x.EndoTypeCode == minPremiumRelation.EndoTypeCode) : null;
                    var product = view.Product.Count > 0 ? view.Product.Cast<Product>().First(x => x.ProductId == minPremiumRelation.Key1) : null;
                    var prefix = view.Prefix.Count > 0 ? view.Prefix.Cast<Prefix>().First(x => x.PrefixCode == minPremiumRelation.PrefixCode) : null;
                    var companyParam = ModelAssembler.CreateCompanyParam(minPremiumRelation, branch, currency, endoType, product, null, null, prefix);

                    response.Add(companyParam);
                }

                return response;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("excepcion en MinPremiunRelationDAO.GetAll", ex);
            }
        }

        public List<CompanyParamMinPremiunRelation> GetByPrefixIdAndProductName(int PrefixId, string ProductName)
        {
            var lst = this.GetAll();

            //Busco por Ramo
            lst = lst.Where(x => x.Prefix.Id.Equals(PrefixId)).ToList();

            //Busco por nombre del producto
            if (lst.Count > 0)
            {
                lst = lst.Where(x => x.Product.Description.ToUpper().Contains(ProductName.ToUpper())).ToList();
            }

            return lst;
        }

        public string Delete(int id)
        {
            PrimaryKey primaryKey = MinPremiumRelation.CreatePrimaryKey(id);
            var result = DataFacadeManager.Delete(primaryKey);
            return result.ToString();
        }
        public string GenerateExcel(List<CompanyParamMinPremiunRelation> List, string fileName)
        {
            try
            {
                var fileProcessValue = new Sistran.Core.Services.UtilitiesServices.Models.FileProcessValue()
                {
                    Key1 = (int)Sistran.Core.Services.UtilitiesServices.Enums.FileProcessType.ParametrizationMinPremiunRelation,
                };

                var file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    /* Ordeno de forma ascendiente por el ID */
                    List = List.OrderBy(x => x.Id).ToList();

                    foreach (var item in List)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();

                        fields[0].Value = item.Id.ToString();
                        fields[1].Value = item.EndorsementType.Description;
                        fields[2].Value = item.Currency.Description;
                        fields[3].Value = item.Branch.Description;
                        fields[4].Value = item.Prefix.Description;
                        fields[5].Value = item.Product.Description;
                        fields[6].Value = (item.GroupCoverage != null ? item.GroupCoverage.Description : (item.MinPremiunRange != null ? item.MinPremiunRange.Description : string.Empty));
                        fields[7].Value = item.RiskMinPremiun.ToString();
                        fields[8].Value = item.SubMinPremiun.ToString();

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    return DelegateService.utilitiesService.GenerateFile(file);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Obtiene listado de grupo de coberuras por producto id
        /// </summary>
        /// <param name="productoId"></param>
        /// <returns></returns>
        public List<CompanyParamCoverage> GetCoverageByPrefixId(int productoId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            try
            {
                filter.Property(ProductGroupCover.Properties.ProductId);
                filter.Equal();
                filter.Constant(productoId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filter.GetPredicate()));
                return businessCollection.Select(x => ModelAssembler.CreateGroupcoverageToCompany((ProductGroupCover)x)).OrderBy(p => p.Description).ToList();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<CompanyParamCoverage> GetAllMinRange()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MinPremiumRange), filter.GetPredicate()));
                return businessCollection.Select(x => ModelAssembler.CreateMinRangeToCompany((MinPremiumRange)x)).OrderBy(p => p.Description).ToList();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<CompanyParamMinPremiunRelation> GetListMinPremiumRelation()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<CompanyParamMinPremiunRelation> result = new List<CompanyParamMinPremiunRelation>();
            SelectQuery SelectQuery = new SelectQuery();
            #region Autos
            filter.Property(MinPremiumRelation.Properties.PrefixCode, "mp");
            filter.Equal();
            filter.Constant((int)ENUM.PrefixType.Automoviles);
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRelation.Properties.MinPremiumRelId, "mp"), "MinRelationId"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRelation.Properties.SubsMinPremium, "mp"), "SubsMinPremium"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRelation.Properties.RiskMinPremium, "mp"), "RiskMinPremium"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Prefix.Properties.Description, "p"), "DescriptionPrefix"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Branch.Properties.Description, "b"), "DescriptionBanch"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(EndorsementType.Properties.Description, "e"), "DescriptionEndo"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Currency.Properties.Description, "c"), "Currency"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Product.Properties.Description, "d"), "product"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ProductGroupCover.Properties.SmallDescription, "g"), "smallDescription"));
            #endregion Select
            Join join = new Join(new ClassNameTable(typeof(MinPremiumRelation), "mp"), new ClassNameTable(typeof(Prefix), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.PrefixCode, "mp")
                .Equal()
                .Property(Prefix.Properties.PrefixCode, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Branch), "b"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.BranchCode, "mp")
                .Equal()
                .Property(Branch.Properties.BranchCode, "b")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(EndorsementType), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(EndorsementType.Properties.EndoTypeCode, "e")
                .Equal()
                .Property(MinPremiumRelation.Properties.EndoTypeCode, "mp")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Currency), "c"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.CurrencyCode, "mp")
                .Equal()
                .Property(Currency.Properties.CurrencyCode, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Product), "d"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.Key1, "mp")
                .Equal()
                .Property(Product.Properties.ProductId, "d")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ProductGroupCover), "g"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.Key2, "mp")
                .Equal()
                .Property(ProductGroupCover.Properties.CoverGroupId, "g")
                .And()
                .Property(MinPremiumRelation.Properties.Key1, "mp")
                .Equal()
                .Property(ProductGroupCover.Properties.ProductId, "g")
                .GetPredicate());

            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    CompanyParamMinPremiunRelation companyParamMinPremiunRelation = new CompanyParamMinPremiunRelation();
                    companyParamMinPremiunRelation.Id = (int)reader["MinRelationId"];
                    companyParamMinPremiunRelation.EndorsementType = new CompanyParamEndorsementType { Description = (string)reader["DescriptionEndo"] };
                    companyParamMinPremiunRelation.Currency = new CompanyParamCurrency { Description = (string)reader["Currency"] };
                    companyParamMinPremiunRelation.Branch = new CompanyParamBranch { Description = (string)reader["DescriptionBanch"] };
                    companyParamMinPremiunRelation.Prefix = new CompanyParamPrefix { Description = (string)reader["DescriptionPrefix"] };
                    companyParamMinPremiunRelation.Product = new CompanyParamProduct { Description = (string)reader["product"] };
                    companyParamMinPremiunRelation.GroupCoverage = new CompanyParamGroupCoverage { Description = (string)reader["smallDescription"] };
                    companyParamMinPremiunRelation.RiskMinPremiun = (decimal)reader["RiskMinPremium"];
                    companyParamMinPremiunRelation.SubMinPremiun = (decimal)reader["SubsMinPremium"];
                    result.Add(companyParamMinPremiunRelation);
                }
            }
            #endregion
            #region surety
            filter = new ObjectCriteriaBuilder();
            SelectQuery = new SelectQuery();
            filter.Property(MinPremiumRelation.Properties.PrefixCode, "mp");
            filter.Equal();
            filter.Constant((int)ENUM.PrefixType.Surety);
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRelation.Properties.MinPremiumRelId, "mp"), "MinRelationId"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRelation.Properties.SubsMinPremium, "mp"), "SubsMinPremium"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRelation.Properties.RiskMinPremium, "mp"), "RiskMinPremium"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Prefix.Properties.Description, "p"), "DescriptionPrefix"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Branch.Properties.Description, "b"), "DescriptionBanch"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(EndorsementType.Properties.Description, "e"), "DescriptionEndo"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Currency.Properties.Description, "c"), "Currency"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(Product.Properties.Description, "d"), "product"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(MinPremiumRange.Properties.Description, "g"), "smallDescription"));
            #endregion Select
            join = new Join(new ClassNameTable(typeof(MinPremiumRelation), "mp"), new ClassNameTable(typeof(Prefix), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.PrefixCode, "mp")
                .Equal()
                .Property(Prefix.Properties.PrefixCode, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Branch), "b"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.BranchCode, "mp")
                .Equal()
                .Property(Branch.Properties.BranchCode, "b")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(EndorsementType), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(EndorsementType.Properties.EndoTypeCode, "e")
                .Equal()
                .Property(MinPremiumRelation.Properties.EndoTypeCode, "mp")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Currency), "c"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.CurrencyCode, "mp")
                .Equal()
                .Property(Currency.Properties.CurrencyCode, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Product), "d"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.Key1, "mp")
                .Equal()
                .Property(Product.Properties.ProductId, "d")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(MinPremiumRange), "g"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MinPremiumRelation.Properties.Key2, "mp")
                .Equal()
                .Property(MinPremiumRange.Properties.MinPremiumRangeId, "g")
                .GetPredicate());


            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    CompanyParamMinPremiunRelation companyParamMinPremiunRelation = new CompanyParamMinPremiunRelation();
                    companyParamMinPremiunRelation.Id = (int)reader["MinRelationId"];
                    companyParamMinPremiunRelation.EndorsementType = new CompanyParamEndorsementType { Description = (string)reader["DescriptionEndo"] };
                    companyParamMinPremiunRelation.Currency = new CompanyParamCurrency { Description = (string)reader["Currency"] };
                    companyParamMinPremiunRelation.Branch = new CompanyParamBranch { Description = (string)reader["DescriptionBanch"] };
                    companyParamMinPremiunRelation.Prefix = new CompanyParamPrefix { Description = (string)reader["DescriptionPrefix"] };
                    companyParamMinPremiunRelation.Product = new CompanyParamProduct { Description = (string)reader["product"] };
                    companyParamMinPremiunRelation.GroupCoverage = new CompanyParamGroupCoverage { Description = (string)reader["smallDescription"] };
                    companyParamMinPremiunRelation.RiskMinPremiun = (decimal)reader["RiskMinPremium"];
                    companyParamMinPremiunRelation.SubMinPremiun = (decimal)reader["SubsMinPremium"];
                    result.Add(companyParamMinPremiunRelation);
                }
            }
            #endregion
            return result;
        }
    }
}
