using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ProductServices.EEProvider.Assemblers;
using Sistran.Core.Application.ProductServices.EEProvider.Entities.Views;
using Sistran.Core.Application.ProductServices.EEProvider.Resources;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using Model = Sistran.Core.Application.ProductServices.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PERSONEN = Sistran.Core.Application.UniquePerson.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using UniqueUserENT = Sistran.Core.Application.UniqueUser.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.ProductServices.EEProvider.DAOs
{
    /// <summary>
    /// Acceso Objeto Producto
    /// </summary>
    public class ProductDAO
    {
        /// <summary>
        /// obtiene la lista de productos a partir del ramo
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<Model.Product> ListProduct(int prefixCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, prefixCode);
            ProductDAO productDAO = new ProductDAO();
            IList productList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.Product), filter.GetPredicate(), null));
            return ConvertToModelProducts(productList);
        }

        /// <summary>
        /// obtiene la lista de productos a partir de 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public BusinessCollection ListProduct(Predicate filter, string[] sort)
        {
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                var products = daf.SelectObjects(typeof(PRODEN.Product), filter, sort);
                businessCollection = new BusinessCollection(products);
            }
            return businessCollection;
        }

        /// <summary>
        /// busca un PRODEN.Product por el productId
        /// </summary>
        /// <param name="productId">id del producto</param>
        /// <returns></returns>
        public PRODEN.Product FindProduct(int productId)
        {
            PRODEN.Product product = null;
            //PrimaryKey key = PRODEN.Product.CreatePrimaryKey(productId);
            //using (var daf = DataFacadeManager.Instance.GetDataFacade())
            //{
            //    product = (PRODEN.Product)daf.GetObjectByPrimaryKey(key);
            //}

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal()
            .Constant(productId);
            PRODEN.Product ProductEntities = DataFacadeManager.Instance.GetDataFacade().List(typeof(PRODEN.Product), filter.GetPredicate()).Cast<PRODEN.Product>().FirstOrDefault();

            return ProductEntities;
        }

        /// <summary>
        /// realiza el mapeo de un Entitie.PRODEN.Product a un Model.Product
        /// </summary>
        /// <param name="product"> Entitie.PRODEN.Product </param>
        /// <returns></returns>
        public Model.Product ConvertToModelProduct(PRODEN.Product product)
        {
            Model.Product p = new Model.Product
            {
                Id = product.ProductId,
                Prefix = new CommonModel.Prefix { Id = product.PrefixCode },
                AdditDisCommissPercentage = product.AdditionalCommissionPercentage,
                IsGreen = product.IsGreen,
                Description = product.Description,
                SmallDescription = product.SmallDescription,
                IncrementCommisionAdjustFactorPercentage = product.IncCommAdjustFactorPercentage,
                DecrementCommisionAdjustFactorPercentage = product.DecCommAdjustFactorPercentage,
                PreRuleSetId = product.PreRuleSetId,
                RuleSetId = product.RuleSetId,
                ScriptId = product.ScriptId,
                AdditionalCommissionPercentage = product.AdditCommissPercentage,
                StandardCommissionPercentage = product.StandardCommissionPercentage,
                StdDiscountCommPercentage = product.StdDiscountCommPercentage,
                SurchargeCommissionPercentage = product.SurchargeCommissionPercentage,
                IsInteractive = (bool)product.IsInteractive,
                IsCollective = (bool)product.IsCollective,
                IsMassive = (bool)product.IsMassive,
                IsRequest = product.IsRequest,
                IsFlatRate = product.IsFlatRate,
                CurrentFrom = product.CurrentFrom,
                CurrentTo = product.CurrentTo,
                Version = product.Version ?? 0,
                CalculateMinPremium = (bool)product.CalculateMinPremium
            };
            return p;
        }

        /// <summary>
        /// realiza el mapeo de una lista de Entitie.PRODEN.Product a una lista de Model.Product
        /// </summary>
        /// <param name="product">lista de Entitie.PRODEN.Product</param>
        /// <returns></returns>
        public List<Model.Product> ConvertToModelProducts(IList productList)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Model.Product> products = new List<Model.Product>();

            foreach (PRODEN.Product product in productList)
            {
                products.Add(ConvertToModelProduct(product));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ConvertToModelProducts");
            return products;
        }

        /// <summary>
        /// obtiene una lista de productos a partir del agentId y el prefixId
        /// </summary>
        /// <param name="agentId">id del agente</param>
        /// <param name="prefixId">id del ramo</param>
        /// <returns></returns>
        public List<Model.Product> GetProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ProductAgentView view = new ProductAgentView();
            ViewBuilder builder = new ViewBuilder("ProductAgentView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductAgent.Properties.IndividualId, typeof(PRODEN.ProductAgent).Name)
            .Equal()
            .Constant(agentId)
            .And()
            .Property(PRODEN.Product.Properties.PrefixCode, typeof(PRODEN.Product).Name)
            .Equal()
            .Constant(prefixId)
            .And()
            .OpenParenthesis()
            .Property(PRODEN.Product.Properties.CurrentTo, typeof(PRODEN.Product).Name)
            .IsNull()
            .Or()
            .Property(PRODEN.Product.Properties.CurrentTo, typeof(PRODEN.Product).Name)
            .GreaterEqual()
            .Constant(DateTime.Now)
            .CloseParenthesis();

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetProductsByAgentIdPrefixId");
            return ConvertToModelProducts(view.Products);
        }

        /// <summary>
        /// obtiene todos productos
        /// </summary>
        /// <returns></returns>
        public List<Model.Product> GetProducts()
        {
            BusinessCollection productList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.Product)));
            return ConvertToModelProducts(productList);
        }

        /// <summary>
        /// obtiene un producto a partie del productId
        /// </summary>
        /// <param name="productId">id del producto</param>
        /// <returns></returns>
        public Model.Product Find(int productId)
        {
            PRODEN.Product product = FindProduct(productId);
            return ConvertToModelProduct(product);
        }

        /// <summary>
        /// obtiene una lista de productos a partie del prefixCode
        /// </summary>
        /// <param name="prefixCode">id del ramo</param>
        /// <returns></returns>
        public List<Model.Product> GetProducts(int prefixCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, prefixCode);

            IList productList =
                new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(
                    typeof(PRODEN.Product), filter.GetPredicate(), new[] { PRODEN.Product.Properties.ProductId }));

            List<Model.Product> products = new List<Model.Product>();

            foreach (PRODEN.Product product in productList)
            {
                Model.Product p = new Model.Product();

                p.Description = product.Description;
                p.Id = product.ProductId;

                products.Add(p);
            }

            return products;
        }

        /// <summary>
        /// Obtener producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        public Model.Product GetProductById(int id)
        {

            //PrimaryKey key = PRODEN.Product.CreatePrimaryKey(id);
            //PRODEN.Product product = (PRODEN.Product)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);           

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal()
            .Constant(id);
            PRODEN.Product ProductEntities = DataFacadeManager.Instance.GetDataFacade().List(typeof(PRODEN.Product), filter.GetPredicate()).Cast<PRODEN.Product>().FirstOrDefault();
            return ConvertToModelProduct(ProductEntities);
        }

        /// <summary>
        /// Consulta si la placa existe producto para agente
        /// </summary>
        /// <param name="agentId">identificador agente</param>
        /// <param name="prefixId">identificador de ramo</param>
        /// <param name="productId">identificador de producto</param>
        /// <returns></returns>
        public Boolean ExistProductAgentByAgentIdPrefixIdProductId(int agentId, int prefixId, int productId)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentPrefix.Properties.IndividualId, typeof(AgentPrefix).Name)
            .Equal().Constant(agentId);
            filter.And();
            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductAgent.Properties.ProductId, typeof(PRODEN.ProductAgent).Name)
            .Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductAgent.Properties.IndividualId, typeof(PRODEN.ProductAgent).Name)
            .Equal().Constant(agentId);
            AgentPrefixProductView view = new AgentPrefixProductView();
            ViewBuilder builder = new ViewBuilder("AgentPrefixProductView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AgentPrefixes.Count > 0)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ExistProductAgentByAgentIdPrefixIdProductId");

                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ExistProductAgentByAgentIdPrefixIdProductId");

                return false;
            }

        }

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        public List<Model.Product> GetProductsByPrefixIdIsGreen(int prefixId, bool isGreen)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, "p", prefixId);
            if (isGreen)
            {
                filter.And();
                filter.PropertyEquals(PRODEN.Product.Properties.IsGreen, "p", isGreen);
            }

            IList productList = ListProduct(filter.GetPredicate(), null);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetProductsByPrefixIdIsGreen");

            return ConvertToModelProducts(productList);
        }

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="agentId">Id del agente</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        public List<Model.Product> GetProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ProductAgentView view = new ProductAgentView();
            ViewBuilder builder = new ViewBuilder("ProductAgentView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.PrefixCode, typeof(PRODEN.Product).Name)
            .Equal()
            .Constant(prefixId)
            .And()
            .OpenParenthesis()
            .Property(PRODEN.Product.Properties.CurrentTo, typeof(PRODEN.Product).Name)
            .IsNull()
            .Or()
            .Property(PRODEN.Product.Properties.CurrentTo, typeof(PRODEN.Product).Name)
            .GreaterEqual()
            .Constant(DateTime.Now);
            if (isGreen)
            {
                filter.And();
                filter.PropertyEquals(PRODEN.Product.Properties.IsGreen, typeof(PRODEN.Product).Name, isGreen);
            }
            if (agentId > 0)
            {
                filter.And();
                filter.Property(PRODEN.ProductAgent.Properties.IndividualId, typeof(PRODEN.ProductAgent).Name)
                .Equal()
                .Constant(agentId);
            }
            filter.CloseParenthesis();

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetProductsByAgentIdPrefixIdIsGreen");
            return ConvertToModelProducts(view.Products);
        }

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>      
        /// <returns>Lista de productos</returns>
        public List<Model.Product> GetProductsByPrefixId(int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, "p", prefixId);
            IList productList = ListProduct(filter.GetPredicate(), null);
            return ConvertToModelProducts(productList);

        }

        #region Modelos

        /// <summary>
        /// Obtienen una lista de productos que utilizan el guion
        /// </summary>
        /// <param name="ScriptId">id del guion</param>
        /// <returns></returns>
        public List<Model.Product> GetProductsByScriptId(int scriptId)
        {
            try
            {
                var listProduct = new List<Model.Product>();
                #region ProductScript
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "p")));
                select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "p")));

                select.Distinct = true;

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(PRODEN.Product.Properties.ScriptId, "p").Equal().Constant(scriptId);

                select.Table = new ClassNameTable(typeof(PRODEN.Product), "p");
                select.Where = where.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        int idProduct = (int)reader["ProductId"];
                        if (listProduct.Count(x => x.Id == idProduct) == 0)
                        {
                            listProduct.Add(new Model.Product()
                            {
                                Id = idProduct,
                                Description = (string)reader["Description"]
                            });
                        }
                    }
                }
                #endregion
                #region RiskScript
                select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "p")));
                select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "p")));

                select.Distinct = true;

                Join join = new Join(new ClassNameTable(typeof(PRODEN.Product), "p"), new ClassNameTable(typeof(PRODEN.ProductCoverRiskType), "pc"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(PRODEN.Product.Properties.ProductId, "p").Equal().Property(PRODEN.ProductCoverRiskType.Properties.ProductId, "pc").GetPredicate());


                where = new ObjectCriteriaBuilder();
                where.Property(PRODEN.ProductCoverRiskType.Properties.ScriptId, "pc").Equal().Constant(scriptId);

                select.Table = join;
                select.Where = where.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        int idProduct = (int)reader["ProductId"];
                        if (listProduct.Count(x => x.Id == idProduct) == 0)
                        {
                            listProduct.Add(new Model.Product()
                            {
                                Id = idProduct,
                                Description = (string)reader["Description"]
                            });
                        }
                    }
                }

                #endregion
                #region CoverageScript
                select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "p")));
                select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "p")));

                select.Distinct = true;

                join = new Join(new ClassNameTable(typeof(PRODEN.Product), "p"), new ClassNameTable(typeof(PRODEN.GroupCoverage), "pc"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(PRODEN.Product.Properties.ProductId, "p").Equal().Property(PRODEN.GroupCoverage.Properties.ProductId, "pc").GetPredicate());


                where = new ObjectCriteriaBuilder();
                where.Property(PRODEN.GroupCoverage.Properties.ScriptId, "pc").Equal().Constant(scriptId);

                select.Table = join;
                select.Where = where.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        int idProduct = (int)reader["ProductId"];
                        if (listProduct.Count(x => x.Id == idProduct) == 0)
                        {
                            listProduct.Add(new Model.Product()
                            {
                                Id = idProduct,
                                Description = (string)reader["Description"]
                            });
                        }
                    }
                }

                #endregion

                return listProduct;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en GetProductsByScriptId " + ex.Message);
            }
        }



        /// <summary>
        /// crea un Entitie.PRODEN.Product a partir de un Model.Product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="ProductEntities"></param>
        private void CreateProductEntity(Model.Product product, ref PRODEN.Product ProductEntities)
        {
            ProductEntities.CurrentTo = product.CurrentTo;
            ProductEntities.IsCollective = product.IsCollective;
            ProductEntities.IsGreen = product.IsGreen;
            ProductEntities.IsRequest = product.IsRequest;
            ProductEntities.IsFlatRate = product.IsFlatRate;
            ProductEntities.CalculateMinPremium = product.CalculateMinPremium;
            ProductEntities.RuleSetId = product.RuleSetId;
            ProductEntities.PreRuleSetId = product.PreRuleSetId;
            ProductEntities.ScriptId = product.ScriptId;
            ProductEntities.Description = product.Description;
            ProductEntities.SmallDescription = product.SmallDescription;
            ProductEntities.StandardCommissionPercentage = product.StandardCommissionPercentage;
            ProductEntities.AdditCommissPercentage = product.AdditionalCommissionPercentage;
        }

        /// <summary>
        /// Creates the copy product.
        /// </summary>
        /// <param name="copyProduct">The copy product.</param>
        /// <returns></returns>
        public virtual int CreateCopyProduct(Model.CopyProduct copyProduct)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("PRODUCT_ID", copyProduct.Id);
            parameters[1] = new NameValue("DECRIPTION", copyProduct.Description);
            parameters[2] = new NameValue("DECRIPTION_SMALL", copyProduct.SmallDescription);

            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("PROD.COPYPRODUCT", parameters);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.CreateCopyProduct");

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Obtener Producto
        /// </summary>
        /// <param name="productId">Id Producto</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Producto</returns>
        public Model.Product GetProductByProductIdPrefixId(int productId, int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name).Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.Product.Properties.PrefixCode, typeof(PRODEN.Product).Name).Equal().Constant(prefixId);

            ProductHardRiskTypeView view = new ProductHardRiskTypeView();
            ViewBuilder builder = new ViewBuilder("ProductHardRiskTypeView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);

            }

            if (view.Products.Count > 0)
            {
                Model.Product product = ConvertToModelProduct(view.Products.Cast<PRODEN.Product>().First());
                product.CoveredRisk = ModelAssembler.CreateCoveredRisk(view.ProductCoverRiskTypes.Cast<PRODEN.ProductCoverRiskType>().First());

                var hardriksType = view.HardRiskTypes.Cast<PARAMEN.HardRiskType>().FirstOrDefault(x => x.LineBusinessCode == prefixId).SubCoveredRiskTypeCode;
                
                if (hardriksType != null)
                {
                    product.CoveredRisk.SubCoveredRiskType = (SubCoveredRiskType)hardriksType;
                  
                }
                else
                {
                    throw new Exception(string.Format(ResourceError.ErrorHardRiskType, prefixId.ToString(), productId.ToString()));
                }

                return product;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar productos por ramo comercial y descripcion
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>      
        /// <param name="description">Descripcion del producto</param>      
        /// <returns>Lista de productos</returns>
        public List<Model.Product> GetProductsByPrefixIdByDescription(int prefixId, string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, "p", prefixId);
            filter.And().Property(PRODEN.Product.Properties.Description, "p").Like().Constant("%" + description + "%");
            IList productList = ListProduct(filter.GetPredicate(), null);
            return ConvertToModelProducts(productList);
        }


        /// <summary>
        /// Obtener productos por producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        public List<Model.Product> GetProductsByProduct(Model.Product product)
        {
            int ProductId = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<Model.Product> products = new List<Model.Product>();
            if (product.Prefix != null && product.Prefix.Id > 0)
            {
                filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, "p", product.Prefix.Id);
            }
            if (product.Description != null && product.Description.Trim() != "")
            {
                Int32.TryParse(product.Description, out ProductId);
                if (ProductId == 0)
                {
                    filter.And().Property(PRODEN.Product.Properties.Description, "p").Like().Constant("%" + product.Description + "%");
                }
                else
                {
                    filter.And().Property(PRODEN.Product.Properties.ProductId, "p").Equal().Constant(product.Description);
                }
            }

            if (product.CurrentFrom > DateTime.MinValue)
            {
                filter.And();
                filter.Property(PRODEN.Product.Properties.CurrentFrom, "p");
                filter.GreaterEqual();
                filter.Constant(product.CurrentFrom);
            }

            if (product.CurrentTo != null && product.CurrentTo > DateTime.MinValue)
            {
                filter.And();
                filter.Property(PRODEN.Product.Properties.CurrentTo, "p");
                filter.GreaterEqual();
                filter.Constant(product.CurrentTo);
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = 20;
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.CurrentFrom, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.CurrentTo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.Description, "pf")));
            #region Join
            Join join = new Join(new ClassNameTable(typeof(COMMEN.Prefix), "pf"), new ClassNameTable(typeof(PRODEN.Product), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(COMMEN.Prefix.Properties.PrefixCode, "pf")
                .Equal()
                .Property(PRODEN.Product.Properties.PrefixCode, "p")
                .GetPredicate());
            #endregion
            selectQuery.Table = join;
            //selectQuery.Table = new ClassNameTable(typeof(PRODEN.Product), "p");
            selectQuery.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    Model.Product productItem = new Model.Product();
                    productItem.Id = Convert.ToInt32(reader["ProductId"]);
                    productItem.Description = (string)reader["Description"];
                    productItem.CurrentFrom = (DateTime)reader["CurrentFrom"];
                    productItem.CurrentTo = reader.IsDBNull(3) ? (DateTime?)null : (DateTime)reader["CurrentTo"];
                    productItem.Prefix = new CommonModel.Prefix { Description = (string)reader[4] };
                    products.Add(productItem);
                }
            }
            return products;
        }

        //AJUSTE Agente del producto 
        public List<Model.ProductAgent> AdjustProductsAgent(ProductAgencyCommissionView productAgentView)
        {
            List<Model.ProductAgent> productAgents = new List<Model.ProductAgent>();
            var agentTypes = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.AgentType))).Cast<PARAMEN.AgentType>().ToList();
            object obj = new object();
            if (productAgentView != null && productAgentView.ProductAgentList != null && productAgentView.ProductAgentList.Count > 0)
            {
                productAgents = ModelAssembler.ProductAgents(productAgentView.ProductAgentList);
                var agents = productAgentView.AgentList.Cast<Agent>().ToList();
                List<PERSONEN.AgentAgency> AgentAgencyList = productAgentView.AgentAgencyList.Cast<AgentAgency>().ToList();
                TP.Parallel.ForEach(productAgents, productAgent =>
                {
                    var agent = agents.FirstOrDefault(x => x.IndividualId == productAgent.IndividualId);
                    var agenType = agentTypes.FirstOrDefault(x => x.AgentTypeCode == agent.AgentTypeCode);
                    productAgent.FullName = agent.CheckPayableTo;
                    productAgent.AgentType = new ProductAgentType { Id = agenType.AgentTypeCode, Description = agenType.Description };
                    var productAgentCommis = productAgentView.ProductAgencyCommissList.Cast<PRODEN.ProductAgencyCommiss>().ToList().Where(x => x.IndividualId == productAgent.IndividualId).ToList();
                    if (productAgentCommis != null && productAgentCommis.Count > 0)
                    {
                        productAgent.ProductAgencyCommiss = ModelAssembler.ProductAgencyCommissions(productAgentCommis);
                        TP.Parallel.ForEach(productAgent.ProductAgencyCommiss, productAgentCommission =>
                        {
                            var agentAgency = productAgentView.AgentAgencyList.Cast<PERSONEN.AgentAgency>().ToList().FirstOrDefault(x => x.IndividualId == productAgentCommission.IndividualId && x.AgentAgencyId == productAgentCommission.AgencyId);
                            productAgentCommission.AgentType = new ProductAgentType { Id = agentAgency.AgentTypeCode, Description = agentTypes.FirstOrDefault(x => x.AgentTypeCode == agentAgency.AgentTypeCode).Description };
                            productAgentCommission.AgencyName = AgentAgencyList.Where(x => x.IndividualId == productAgentCommission.IndividualId && x.AgentAgencyId == productAgentCommission.AgencyId).FirstOrDefault().Description;
                        });
                    }
                });
            }
            return productAgents;
        }


        public List<Model.ProductAgent> GetProductAgentByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Agentes por Producto
            List<Model.ProductAgent> productAgents = new List<Models.ProductAgent>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductAgent.Properties.ProductId, typeof(PRODEN.ProductAgent).Name).Equal().Constant(productId);
            ProductAgencyCommissionView viewProductAgencyCommission = new ProductAgencyCommissionView();
            ViewBuilder builderProductAgentView = new ViewBuilder("ProductAgencyCommissionView");
            builderProductAgentView.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builderProductAgentView, viewProductAgencyCommission);
            productAgents = AdjustProductsAgent(viewProductAgencyCommission);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetProductAgentByProductId");

            return productAgents;
        }

        public List<CommonModel.Currency> GetCurrencyByProductId(int productId)
        {
            List<CommonModel.Currency> currencies = null;
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Currency.Properties.CurrencyCode, "c")));
            #region joins
            Join join = new Join(new ClassNameTable(typeof(PRODEN.ProductCurrency), "pc"), new ClassNameTable(typeof(COMMEN.Currency), "c"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()).Property(COMMEN.Currency.Properties.CurrencyCode, "c").Equal().Property(PRODEN.ProductCurrency.Properties.CurrencyCode, "pc").GetPredicate();
            select.Table = join;
            #endregion
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductCurrency.Properties.ProductId, "pc");
            filter.Equal();
            filter.Constant(productId);
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    currencies = new List<CommonModel.Currency>() { new CommonModel.Currency { Id = Convert.ToInt32(reader["CurrencyCode"]) } };
                }
            }
            return currencies;
        }

        public List<Model.CoveredRisk> GetProductCoveredRiskTypeListByProductId(int productId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductCoverRiskType.Properties.ProductId, "pcr");
            filter.Equal();
            filter.Constant(productId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.ProductCoverRiskType), filter.GetPredicate()));
            return ModelAssembler.CreateCoveredRisks(businessCollection);

        }

        public List<Model.Product> GetSingleProductByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (productId > 0)
            {
                filter.PropertyEquals(PRODEN.Product.Properties.ProductId, "p", productId);
            }
            else
            {
                filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, "p", prefixId);
                filter.And().Property(PRODEN.Product.Properties.Description, "p").Like().Constant("%" + description + "%");
            }
            IList productList = ListProduct(filter.GetPredicate(), null);
            return ConvertToModelProducts(productList);
        }

        /// <summary>
        /// Obtiene si se debe calcular prima mínima para el producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool GetCalculateMinPremiumByProductId(int productId)
        {
            PRODEN.Product product = FindProduct(productId);
            return (bool)product.CalculateMinPremium;
        }


        /// <summary>
        /// Obtiene los agentes que estan asociados al producto de acuerdo al Id del Agent
        /// </summary>
        /// <param name="agentId">Id del Agent</param>
        /// <param name="productId">Id del Producto</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Agencias relacionadas con el producto</returns>  
        public List<ProductAgency> GetAgenciesByAgentIdDesciptionProductIdUserId(int agentId, string description, int productId, int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<UserAgency> agenciesUser = DelegateService.uniqueUserServiceCore.GetAgenciesByUserId(userId);
            List<ProductAgency> agencies = new List<ProductAgency>();
            List<ProductAgent> agents = new List<ProductAgent>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            bool bandAnd = false;
            if (agentId > 0)
            {
                filter.Property(PERSONEN.Agent.Properties.IndividualId, typeof(PERSONEN.Agent).Name);
                filter.Equal();
                filter.Constant(agentId);
                bandAnd = true;
            }

            Int32 agencyCode = 0;
            Int32.TryParse(description, out agencyCode);

            if (agencyCode > 0)
            {
                if (bandAnd)
                {
                    filter.And();
                }
                filter.Property(PERSONEN.AgentAgency.Properties.AgentCode, typeof(PERSONEN.AgentAgency).Name);
                filter.Equal();
                filter.Constant(agencyCode);
                bandAnd = true;

            }
            else
            {
                if (bandAnd)
                {
                    filter.And();
                }
                filter.Property(PERSONEN.AgentAgency.Properties.Description, typeof(PERSONEN.AgentAgency).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                bandAnd = true;
            }

            if (productId > 0)
            {
                if (bandAnd)
                {
                    filter.And();
                }
                filter.Property(PRODEN.ProductAgent.Properties.ProductId, typeof(PRODEN.ProductAgent).Name);
                filter.Equal();
                filter.Constant(productId);
                bandAnd = true;
            }

            if (userId > 0 && agenciesUser.Count > 0)
            {
                if (bandAnd)
                {
                    filter.And();
                }
                filter.Property(UniqueUserENT.UniqueUsers.Properties.UserId, typeof(UniqueUserENT.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(userId);
            }
            AgentProductAgentView agentProductAgentView = new AgentProductAgentView();
            ViewBuilder builder = new ViewBuilder("AgentProductAgentView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, agentProductAgentView);
            agencies = ModelAssembler.CreateAgencies(agentProductAgentView.AgentAgencies);
            agents = ModelAssembler.CreateAgents(agentProductAgentView.Agents);

            foreach (ProductAgency item in agencies)
            {
                item.Agent.FullName = agents.First(x => x.IndividualId == item.Agent.IndividualId).FullName;
                item.Agent.DateDeclined = agents.First(x => x.IndividualId == item.Agent.IndividualId).DateDeclined;
                item.Agent.AgentType = new ProductAgentType() { Id = agents.First(x => x.IndividualId == item.Agent.IndividualId).AgentType.Id };
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs.GetAgenciesByAgentIdDesciptionProductIdUserId");
            return agencies;
        }
        #region producto agentes
        public Model.ProductAgencyCommiss GetAgentcyByProductByIndivualId(int productId, int individualId, Int16 agencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductAgencyCommiss.Properties.ProductId, typeof(PRODEN.ProductAgencyCommiss).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductAgencyCommiss.Properties.IndividualId, typeof(PRODEN.ProductAgencyCommiss).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, typeof(PRODEN.ProductAgencyCommiss).Name);
            filter.Equal();
            filter.Constant(agencyId);
            Model.ProductAgencyCommiss agencyCommis = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                var agencyEntity = daf.List(typeof(PRODEN.ProductAgencyCommiss), filter.GetPredicate()).Cast<PRODEN.ProductAgencyCommiss>().FirstOrDefault();
                if (agencyEntity != null)
                {
                    agencyCommis = ModelAssembler.ProductAgencyCommission(agencyEntity);
                }
            }
            return agencyCommis;
        }
        #endregion
        #endregion

    }
}
