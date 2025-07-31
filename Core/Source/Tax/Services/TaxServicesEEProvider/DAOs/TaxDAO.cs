using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.TaxServices.EEProvider.Assemblers;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.TaxServices.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.TaxServices.DAOs
{
    /// <summary>
    ///  estados de contragarantías
    /// </summary>
    public class TaxDAO
    {
        /// <summary>
        /// Obtiene listado de impuestos
        /// </summary>
        public List<Models.Tax> GetTax()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.Tax)));
            return ModelAssembler.CreateTaxs(businessCollection);
        }

        /// <summary>
        /// Obtiene las condiciones de los impuestos
        /// </summary>
        public List<TaxCondition> GetTaxCondition()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.TaxCondition)));
            return ModelAssembler.CreateTaxConditions(businessCollection);
        }

        /// <summary>
        /// Obtiene las condiciones de los impuestos por id de impuesto
        /// </summary>
        public List<TaxCondition> GetTaxConditionByTaxId(int taxId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxCondition.Properties.TaxCode, typeof(TAXEN.TaxCondition).Name);
            filter.Equal();
            filter.Constant(taxId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.TaxCondition), filter.GetPredicate()));
            return ModelAssembler.CreateTaxConditions(businessCollection);
        }
        public List<TaxCategory> GetTaxCategory()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.TaxCategory)));
            return ModelAssembler.CreateTaxCategory(businessCollection);
        }

        public List<TaxCategory> GetTaxCategoryByTaxId(int taxId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxCategory.Properties.TaxCode, typeof(TAXEN.TaxCategory).Name);
            filter.Equal();
            filter.Constant(taxId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.TaxCategory), filter.GetPredicate()));
            return ModelAssembler.CreateTaxCategory(businessCollection);
        }

        public List<Models.Tax> GetTaxesByIndividualId(int IndividualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.IndividualTax.Properties.IndividualId, typeof(TAXEN.IndividualTax).Name);
            filter.Equal();
            filter.Constant(IndividualId);

            TaxIndividualTaxView view = new TaxIndividualTaxView();
            ViewBuilder builder = new ViewBuilder("TaxIndividualTaxView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Taxes.Count > 0)
            {
                return ModelAssembler.CreateTaxs(view.Taxes);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<TaxCategoryCondition> GetTaxPeriodRatesByTaxesTaxAttributes(IEnumerable<TaxCategoryCondition> taxes, IEnumerable<TaxAttribute> attributes)
        {
            Func<TaxCategoryCondition, TaxCategoryCondition> GetTaxPeriodRateByTax = (tax) =>
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TAXEN.TaxAttribute.Properties.TaxCode, typeof(TAXEN.TaxAttribute).Name);
                filter.Equal();
                filter.Constant(tax.Id);

                TaxAttributeView taxAttributeView = new TaxAttributeView();
                ViewBuilder viewBuilder = new ViewBuilder("TaxAttributeView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, taxAttributeView);

                if (taxAttributeView.TaxAttributes.Count > 0)
                {
                    filter.Clear();
                    filter.Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name);
                    filter.Equal();
                    filter.Constant(tax.Id);
                    bool emptyFilter = false;
                    taxAttributeView.TaxAttributeTypes.Cast<TAXEN.TaxAttributeType>().ToList().ForEach(attributeType =>
                    {
                        switch (attributeType.Description)
                        {
                            case "TAX_CONDITION_CODE":
                                if (Convert.ToInt32(tax.TaxCondition?.Id) > 0)
                                {
                                    if (!emptyFilter)
                                        filter.And();
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name, tax.TaxCondition.Id);
                                    emptyFilter = false;
                                }
                                break;
                            case "TAX_CATEGORY_CODE":
                                if (Convert.ToInt32(tax.TaxCategory?.Id) > 0)
                                {
                                    if (!emptyFilter)
                                        filter.And();
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.TaxCategoryCode, typeof(TAXEN.TaxRate).Name, tax.TaxCategory.Id);
                                    emptyFilter = false;
                                }
                                break;
                            case "CITY_CODE":
                                if (Convert.ToInt32(attributes.FirstOrDefault(x => x.Description == "CITY_CODE")?.Value) > 0)
                                {
                                    if (!emptyFilter)
                                        filter.And();
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.CityCode, typeof(TAXEN.TaxRate).Name, attributes.FirstOrDefault(x => x.Description == "CITY_CODE")?.Value);
                                    emptyFilter = false;
                                }
                                break;
                            case "STATE_CODE":
                                if (Convert.ToInt32(attributes.FirstOrDefault(x => x.Description == "STATE_CODE")?.Value) > 0)
                                {
                                    if (!emptyFilter)
                                        filter.And();
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.StateCode, typeof(TAXEN.TaxRate).Name, attributes.FirstOrDefault(x => x.Description == "STATE_CODE")?.Value);
                                    emptyFilter = false;
                                }
                                break;
                            case "COUNTRY_CODE":
                                if (Convert.ToInt32(attributes.FirstOrDefault(x => x.Description == "COUNTRY_CODE")?.Value) > 0)
                                {
                                    if (!emptyFilter)
                                        filter.And();
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name, attributes.FirstOrDefault(x => x.Description == "COUNTRY_CODE")?.Value);
                                    emptyFilter = false;
                                }
                                break;
                            case "ECONOMIC_ACTIVITY_CODE":
                                if (tax.EconomicActivityTaxId > 0)
                                {
                                    if (!emptyFilter)
                                        filter.And();
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.EconomicActivityTaxCode, typeof(TAXEN.TaxRate).Name, tax.EconomicActivityTaxId);
                                    emptyFilter = false;
                                }
                                break;
                            case "BRANCH_CODE":
                                if (!emptyFilter)
                                    filter.And();
                                if (Convert.ToInt32(attributes.FirstOrDefault(x => x.Description == "BRANCH_CODE")?.Value) > 0)
                                {
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name, attributes.FirstOrDefault(x => x.Description == "BRANCH_CODE").Value);
                                    emptyFilter = false;
                                }
                                else
                                {
                                    filter.Property(TAXEN.TaxRate.Properties.BranchCode);
                                    filter.IsNull();
                                    emptyFilter = false;
                                }
                                break;
                            case "LINE_BUSINESS_CODE":
                                if (!emptyFilter)
                                    filter.And();
                                if (Convert.ToInt32(attributes.FirstOrDefault(x => x.Description == "LINE_BUSINESS_CODE")?.Value) > 0)
                                {
                                    filter.PropertyEquals(TAXEN.TaxRate.Properties.LineBusinessCode, typeof(TAXEN.TaxRate).Name, attributes.FirstOrDefault(x => x.Description == "LINE_BUSINESS_CODE").Value);
                                    emptyFilter = false;
                                }
                                else
                                {
                                    filter.Property(TAXEN.TaxRate.Properties.LineBusinessCode);
                                    filter.IsNull();
                                    emptyFilter = false;
                                }
                                break;
                        }
                    });
                }
                else
                {
                    return tax;
                }

                TaxAccountingConceptTaxView accountingConceptTaxView = new TaxAccountingConceptTaxView();
                viewBuilder = new ViewBuilder("TaxAccountingConceptTaxView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, accountingConceptTaxView);

                if (accountingConceptTaxView.TaxPeriodRates.Any())
                {
                    TAXEN.TaxPeriodRate entityTaxPeriodRate = accountingConceptTaxView.TaxPeriodRates.Cast<TAXEN.TaxPeriodRate>().First();
                    tax.TaxPeriodRate = ModelAssembler.CreateTaxPeriodRate(entityTaxPeriodRate);
                    tax.AccountingConceptId = accountingConceptTaxView.TaxRates.Cast<TAXEN.TaxRate>().FirstOrDefault(m => m.TaxRateId == entityTaxPeriodRate.TaxRateId).AccountingConceptCode;
                }

                return tax;
            };

            return taxes.Select(tax => GetTaxPeriodRateByTax(tax));
        }

        public IEnumerable<TaxAttribute> GetTaxAttributesByTaxId(int taxId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxAttribute.Properties.TaxCode, typeof(TAXEN.TaxAttribute).Name);
            filter.Equal();
            filter.Constant(taxId);

            TaxAttributeView taxAttributeView = new TaxAttributeView();
            ViewBuilder viewBuilder = new ViewBuilder("TaxAttributeView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, taxAttributeView);

            if (taxAttributeView.TaxAttributes.Count > 0)
            {
                Func<TaxAttribute, TaxAttribute> GetTaxAttributeDecription = (taxAttribute) =>
                {
                    if (taxAttributeView.TaxAttributeTypes.Count > 0)
                    {
                        taxAttribute.Description = taxAttributeView.TaxAttributeTypes.Cast<TAXEN.TaxAttributeType>().First(x => x.TaxAttributeTypeCode == taxAttribute.Id).Description;
                    }

                    return taxAttribute;
                };

                return ModelAssembler.CreateTaxAttributes(taxAttributeView.TaxAttributes).Select(taxAttribute => GetTaxAttributeDecription(taxAttribute));
            }

            return new List<TaxAttribute>();
        }

        public IEnumerable<TaxAttribute> GetTaxAttributes()
        {
            return ModelAssembler.CreateTaxAttributesByTaxAttributeTypes(DataFacadeManager.GetObjects(typeof(TAXEN.TaxAttributeType)));
        }

        public IEnumerable<IndividualTaxCategoryCondition> GetIndividualTaxCategoryConditionByIndividualIdRoleId(int individualId, int? roleId = 0)
        {
            try
            {
                List<IndividualTaxCategoryCondition> individualTaxCategoryConditions = new List<IndividualTaxCategoryCondition>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(TAXEN.IndividualTax.Properties.IndividualId, individualId);
                if (roleId > 0)
                {
                    filter.And();
                    filter.PropertyEquals(TAXEN.IndividualTax.Properties.RoleCode, roleId);
                }
                filter.And();
                filter.PropertyEquals(TAXEN.Tax.Properties.Enabled, 1);

                #region Selects

                SelectQuery selectQuery = new SelectQuery();

                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.IndividualTax.Properties.IndividualId, typeof(TAXEN.IndividualTax).Name), "IndividualId"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name), "TaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name), "TaxConditionCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.StateCode, typeof(TAXEN.TaxRate).Name), "StateCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name), "CountryCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxCategoryCode, typeof(TAXEN.TaxRate).Name), "TaxCategoryCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.EconomicActivityTaxCode, typeof(TAXEN.TaxRate).Name), "EconomicActivityTaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name), "BranchCode"));

                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.Description, typeof(TAXEN.Tax).Name), "TaxDescription"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.RateTypeCode, typeof(TAXEN.Tax).Name), "RateTypeCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.Enabled, typeof(TAXEN.Tax).Name), "IsEnabled"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.IsRetention, typeof(TAXEN.Tax).Name), "IsRetention"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.RetentionTaxCode, typeof(TAXEN.Tax).Name), "RetentionTaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.BaseConditionTaxCode, typeof(TAXEN.Tax).Name), "BaseConditionTaxCode"));

                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxCategory.Properties.Description, typeof(TAXEN.TaxCategory).Name), "TaxCategoryDescription"));

                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxCondition.Properties.Description, typeof(TAXEN.TaxCondition).Name), "TaxConditionDescription"));
                
                selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, typeof(COMMEN.Branch).Name), "BranchDescription"));

                #endregion

                #region Joins

                Join join = new Join(new ClassNameTable(typeof(TAXEN.IndividualTax), typeof(TAXEN.IndividualTax).Name), new ClassNameTable(typeof(TAXEN.TaxRate), typeof(TAXEN.TaxRate).Name), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TAXEN.IndividualTax.Properties.TaxRateCode, typeof(TAXEN.IndividualTax).Name)
                    .Equal()
                    .Property(TAXEN.TaxRate.Properties.TaxRateId, typeof(TAXEN.TaxRate).Name)
                    ).GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(TAXEN.Tax), typeof(TAXEN.Tax).Name), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(TAXEN.Tax.Properties.TaxCode, typeof(TAXEN.Tax).Name)
                    ).GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(TAXEN.TaxCondition), typeof(TAXEN.TaxCondition).Name), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(TAXEN.TaxCondition.Properties.TaxCode, typeof(TAXEN.TaxCondition).Name)
                    .And()
                    .Property(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(TAXEN.TaxCondition.Properties.TaxConditionCode, typeof(TAXEN.TaxCondition).Name)
                    ).GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(TAXEN.TaxCategory), typeof(TAXEN.TaxCategory).Name), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(TAXEN.TaxCategory.Properties.TaxCode, typeof(TAXEN.TaxCategory).Name)
                    .And()
                    .Property(TAXEN.TaxRate.Properties.TaxCategoryCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(TAXEN.TaxCategory.Properties.TaxCategoryCode, typeof(TAXEN.TaxCategory).Name)
                    ).GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), typeof(COMMEN.Branch).Name), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name)
                    ).GetPredicate();

                #endregion

                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        IndividualTaxCategoryCondition individualTaxCategoryCondition = new IndividualTaxCategoryCondition
                        {
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            CountryId = Convert.ToInt32(reader["CountryCode"]),
                            StateId = Convert.ToInt32(reader["StateCode"]),
                            Tax = new TaxCategoryCondition
                            {
                                Id = Convert.ToInt32(reader["TaxCode"]),
                                Description = Convert.ToString(reader["TaxDescription"]),
                                RateType = (Enums.RateType)Convert.ToInt32(reader["RateTypeCode"]),
                                IsEnable = Convert.ToBoolean(reader["IsEnabled"]),
                                IsRetention = Convert.ToBoolean(reader["IsRetention"]),
                                RetentionTaxId = Convert.ToInt32(reader["RetentionTaxCode"]),
                                BaseConditionTaxId = Convert.ToInt32(reader["BaseConditionTaxCode"]),
                                TaxCategory = new TaxCategory
                                {
                                    Id = Convert.ToInt32(reader["TaxCategoryCode"]),
                                    Description = Convert.ToString(reader["TaxCategoryDescription"])
                                },
                                TaxCondition = new TaxCondition
                                {
                                    Id = Convert.ToInt32(reader["TaxConditionCode"]),
                                    Description = Convert.ToString(reader["TaxConditionDescription"])
                                },
                                Branch = new CommonService.Models.Branch
                                {
                                    Id = Convert.ToInt32(reader["BranchCode"]),
                                    Description = Convert.ToString(reader["BranchDescription"])
                                },
                                EconomicActivityTaxId = Convert.ToInt32(reader["EconomicActivityTaxCode"])
                            }
                        };

                        individualTaxCategoryConditions.Add(individualTaxCategoryCondition);
                    }
                }

                return individualTaxCategoryConditions;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the individual tax exemption by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="taxCode">The tax code.</param>
        /// <param name="currentFrom">The current from.</param>
        /// <returns></returns>
        public IndividualTaxExemptionDTO GetIndividualTaxExemptionByIndividualId(int individualId, int taxCode, DateTime currentFrom)
        {
            TAXEN.IndividualTaxExemption individualTaxExemptions = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.IndividualTaxExemption.Properties.IndividualId, typeof(TAXEN.IndividualTaxExemption).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(TAXEN.IndividualTaxExemption.Properties.TaxCode, typeof(TAXEN.IndividualTaxExemption).Name);
            filter.Equal();
            filter.Constant(taxCode);
            filter.And();
            filter.Property(TAXEN.IndividualTaxExemption.Properties.CurrentFrom, typeof(TAXEN.IndividualTaxExemption).Name);
            filter.LessEqual();
            filter.Constant(currentFrom);
            individualTaxExemptions = DataFacadeManager.Instance.GetDataFacade().List(typeof(TAXEN.IndividualTaxExemption), filter.GetPredicate()).Cast<TAXEN.IndividualTaxExemption>().FirstOrDefault();

            return ModelAssembler.CreateIndividualTaxExemptionDTO(individualTaxExemptions);
        }
        #region Economic Activity Tax

        /// <summary>
        /// Obtiene listado de Actividades economicas de tasas
        /// </summary>
        public List<Models.EconomicActivityTax> GetEconomicActivitiesTax()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.EconomicActivityTax)));
            return ModelAssembler.CreateEconomicActivitesTax(businessCollection);
        }


        #endregion
        #region tax components
        /// <summary>
        /// Gets the tax by tax ids.
        /// </summary>
        /// <param name="taxId">The tax identifier.</param>
        /// <returns></returns>
        internal TaxComponentDTO GetTaxComponentByTaxId(int taxId)
        {
            TAXEN.TaxComponent taxComponent = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxComponent.Properties.ComponentCode, typeof(TAXEN.TaxComponent).Name);
            filter.Equal().Constant(taxId);
            taxComponent = DataFacadeManager.Instance.GetDataFacade().List(typeof(TAXEN.TaxComponent), filter.GetPredicate()).Cast<TAXEN.TaxComponent>().FirstOrDefault();
            return ModelAssembler.CreateTaxComponent(taxComponent);
        }

        /// <summary>
        /// Gets the taxs by tax ids.
        /// </summary>
        /// <param name="taxId">The tax identifier.</param>
        /// <returns></returns>
        internal List<TaxComponentDTO> GetTaxComponentsByTaxIds(List<int> taxId)
        {
            List<TAXEN.TaxComponent> taxComponents = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxComponent.Properties.TaxCode, typeof(TAXEN.TaxComponent).Name);
            filter.In();
            filter.ListValue();
            taxId.ForEach(a =>
            {
                filter.Constant(a);
            });
            filter.EndList();

            taxComponents = DataFacadeManager.Instance.GetDataFacade().List(typeof(TAXEN.TaxComponent), filter.GetPredicate()).Cast<TAXEN.TaxComponent>().ToList();

            return ModelAssembler.CreateTaxsComponents(taxComponents);
        }

        internal List<TaxComponentDTO> GetTaxComponentsByComponentsIds(List<int> ComponentsIds)
        {
            List<TAXEN.TaxComponent> taxComponents = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxComponent.Properties.ComponentCode, typeof(TAXEN.TaxComponent).Name);
            filter.In();
            filter.ListValue();
            ComponentsIds.ForEach(a =>
            {
                filter.Constant(a);
            });
            filter.EndList();

            taxComponents = DataFacadeManager.Instance.GetDataFacade().List(typeof(TAXEN.TaxComponent), filter.GetPredicate()).Cast<TAXEN.TaxComponent>().ToList();

            return ModelAssembler.CreateTaxsComponents(taxComponents);
        }

        /// <summary>
        ///Obtiene la tasa de los impuestos.
        /// </summary>
        /// <param name="taxId">identificador Impuesto Iva</param>
        /// <param name="branchId">Sucursal</param>
        /// <param name="lineBusinessId">RamoTecnico</param>
        /// <returns></returns>
        internal List<TaxPayerDTO> GetTaxPayerIds(List<int> taxIds, Int16 branchId, int lineBusinessId)
        {
            List<TaxPayerDTO> taxPayerDTOs = new List<TaxPayerDTO>();
            List<TaxPayerDTO> taxPayerDTOsBase = new List<TaxPayerDTO>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name);
            filter.In();
            filter.ListValue();
            taxIds.ForEach(a =>
            {
                filter.Constant(a);
            });
            filter.EndList();
            filter.And();
            filter.PropertyEquals(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name, branchId);
            filter.And();
            filter.PropertyEquals(TAXEN.TaxRate.Properties.LineBusinessCode, typeof(TAXEN.TaxRate).Name, lineBusinessId);
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name), TAXEN.TaxRate.Properties.TaxCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name), TAXEN.TaxRate.Properties.TaxConditionCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxPeriodRate.Properties.Rate, typeof(TAXEN.TaxPeriodRate).Name), TAXEN.TaxPeriodRate.Properties.Rate));
            selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxPeriodRate.Properties.CurrentFrom, typeof(TAXEN.TaxPeriodRate).Name), TAXEN.TaxPeriodRate.Properties.CurrentFrom));
            Join join = new Join(new ClassNameTable(typeof(TAXEN.TaxRate), typeof(TAXEN.TaxRate).Name),
                                     new ClassNameTable(typeof(TAXEN.TaxCondition), typeof(TAXEN.TaxCondition).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(TAXEN.TaxCondition.Properties.TaxCode, typeof(TAXEN.TaxCondition).Name)
                .Equal()
                .Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name)
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(TAXEN.TaxPeriodRate), typeof(TAXEN.TaxPeriodRate).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(TAXEN.TaxPeriodRate.Properties.TaxRateId, typeof(TAXEN.TaxPeriodRate).Name)
                .Equal()
                .Property(TAXEN.TaxRate.Properties.TaxRateId, typeof(TAXEN.TaxRate).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    taxPayerDTOs.Add(new TaxPayerDTO
                    {
                        Id = reader[TAXEN.TaxRate.Properties.TaxCode] == DBNull.Value ? 0 : Convert.ToInt32(reader[TAXEN.TaxRate.Properties.TaxCode]),
                        TaxConditionId = reader[TAXEN.TaxRate.Properties.TaxConditionCode] == DBNull.Value ? 0 : Convert.ToInt32(reader[TAXEN.TaxRate.Properties.TaxConditionCode]),
                        Rate = reader[TAXEN.TaxPeriodRate.Properties.Rate] == DBNull.Value ? 0 : Convert.ToInt32(reader[TAXEN.TaxPeriodRate.Properties.Rate]),
                        CurrentFrom = Convert.ToDateTime(reader[TAXEN.TaxPeriodRate.Properties.CurrentFrom])
                    });
                }
            }
            taxPayerDTOsBase = taxPayerDTOs.GroupBy(m => new { m.Id }).Select(z => z.OrderByDescending(a => a.CurrentFrom).First()).ToList();
            return taxPayerDTOsBase;
        }


        #endregion tax components
    }
}